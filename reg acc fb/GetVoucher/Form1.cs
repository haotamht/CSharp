using EAGetMail;
using Nancy.Json;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using xNet;

namespace GetVoucher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        StreamReader read = new StreamReader("token.txt");
        bool isVeryAcc(IWebDriver driver, string content)
        {
            return driver.PageSource.IndexOf(content) != -1;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadAddress();
            listView1.Width = 810;
            token = read.ReadToEnd();
            read.Close();

            if (token == "")
            {
                MessageBox.Show("Đăng Nhập Sử Dụng");
            }
            else
            {
                historylength = GetInfo(token);
                if (historylength > 0)
                {
                    button2.Enabled = false;
                    button2.Text = "Đã Đăng Nhập";

                }


            }
        }
        String GetProxyShopLike(String key)
        {
            string proxy = null;
            while (proxy == null)
            {
                HttpRequest http = new HttpRequest();
                String html = http.Get("http://proxy.shoplike.vn/Api/getNewProxy?access_token=" + key + "&location=&provider=").ToString();
                var res = JsonConvert.DeserializeObject<ProxyShopLike>(html);
                if (res.status == "success")
                {
                    proxy = res.data.proxy;
                    break;
                }
                else
                {
               
                    int time = TachSo(html);
                   
                    Thread.Sleep(time * 1000);
                   
                }
            }
            return proxy;
        }
        String GetProxy_TM(String key)
        {
            string proxy = null;
            while (proxy == null)
            {
                try
                {
                    HttpRequest http = new HttpRequest();
                    String Data = @"{""api_key"":""" + key + @""",""id_location"":1}";
                    string html = http.Post("https://tmproxy.com/api/proxy/get-new-proxy", Data, "application/json").ToString();
                    var result = JsonConvert.DeserializeObject<TmProxy>(html);
                    if (result.code == 0)
                    {
                        proxy = result.data.https;
                        break;
                    }
                    else
                    {


                        int time = TachSo(result.message);


                        Thread.Sleep(time * 1000);
                    }


                }
                catch
                {


                }
            }
            return proxy;
        }
        int TachSo(string input)
        {
            int i = 1;
            string[] numbers = Regex.Split(input, @"\D+");
            foreach (string value in numbers)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    i = int.Parse(value);
                }
            }
            return i;
        }
        string GET_proxy(string key)
        {
            string proxy = null;
            string res = null;
            while (true)
            {
                try
                {
                    HttpRequest http = new HttpRequest();
                    string html = http.Get("http://proxy.tinsoftsv.com/api/changeProxy.php?key=" + key + "&location=0").ToString();
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    var obj = jss.Deserialize<dynamic>(html);
                    res = Convert.ToString(obj["success"]);
                    if (res == "False")
                    {
                        string time = Convert.ToString(obj["description"]);
                        time = Regex.Match(time, @"wait (\d+)s for next change!").Groups[1].Value;
                        Thread.Sleep(int.Parse(time == "" ? "0" : time) * 1000);
                        Thread.Sleep(5000);
                    }
                    else
                    {
                        proxy = Convert.ToString(obj["proxy"]);
                        break;
                    }
                }
                catch (Exception)
                {

                }

            }
            return proxy;
        }
        Writer writer = new Writer("token.txt");
        String token = null;
        int historylength = 0;
        private void button2_Click(object sender, EventArgs e)
        {
            HttpRequest http = new HttpRequest();

            String Data = @"{""username"":""" + textBox3.Text + @""",""password"":""" + textBox4.Text + @"""}";
            String html = http.Post("https://appserverlzd.herokuapp.com/api/user/login", Data, "application/json; charset=utf-8").ToString();
            if (html.IndexOf("true") != -1)
            {
                var res = JsonConvert.DeserializeObject<SellClone>(html);
                writer.WriteToFile(res.token);
                token = res.token;
                historylength = GetInfo(token);
                MessageBox.Show("Thành công");
            }
            else
            {
                MessageBox.Show("Đăng nhập thất bại");
            }
        }
        int GetInfo(String token)
        {
            int result = 5;

            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (historylength <= 0)
            {
                MessageBox.Show("Đăng nhập để sử dụng");
            }
            else
            {
                if (tbOderID.Text == "" || tbXpath.Text == "")
                {
                    MessageBox.Show("Vui lòng điền đầy đủ");
                }
                else
                {

                    GetVoucher();

                }
            }
        }
        Object _lockNextUsernameIndex = new object();
        int nextUsernameIndex = 0;
        int nextList = 0;
        Object lockAgent = new object();
        Writer AccVery = new Writer(@"AccLoi\Very.txt");
        Writer ChuaDoiDuoc = new Writer(@"AccLoi\Checklai.txt");
        Writer AccSaiPass = new Writer(@"AccLoi\SaiPass.txt");//Getmaok
        Writer Getmaok = new Writer(@"DaLayMa\LayThanhCong.txt");
        Writer AccChuaMua = new Writer(@"AccLoi\LayThatbai.txt");
        Writer ThemThanhCong = new Writer(@"AddDiaChi\themthanhcong.txt");
        void GetVoucher()
        {
            String XpathThanhPho = File.ReadAllText("XpathThanhPho.txt");
            String XpathHuyen = File.ReadAllText("XpathHuyen.txt");
            String[] XpathXa = File.ReadAllLines("XpathXa.txt");
            StreamReader readuser = new StreamReader("listuseragent.txt");
            String DataUser = readuser.ReadToEnd();
            String[] listUSer = DataUser.Split(
                 new[] { "\r\n", "\r", "\n" },
                 StringSplitOptions.None
             );
            StreamReader readproxy = new StreamReader("proxy.txt");
            String rbKeyProxy = readproxy.ReadToEnd();
            readproxy.Close();
            string[] keyProxys = rbKeyProxy.Split(
                  new[] { "\r\n", "\r", "\n" },
                  StringSplitOptions.None
              );

            HttpRequest http = new HttpRequest();
            String Datapost = @"{""oderid"":""" + tbOderID.Text + @"""}";
            http.AddHeader("Authorization", "Bearer " + token);
            String html = http.Post("https://appserverlzd.herokuapp.com/oder", Datapost, "application/json; charset=utf-8").ToString();
            var res = JsonConvert.DeserializeObject<DataOder>(html);
            if (res.status == false)
            {
                MessageBox.Show("Kiểm tra lai thông tin đơn hàng!");
            }
            else
            {
                AdbCommand("Del sourceacc.txt");
                int start = 0;
                int end = res.info.Length - 1;
                if (tbStart.Text != "")
                {
                    start = int.Parse(tbStart.Text) - 1;
                }
                if (tbEnd.Text != "")
                {
                    end = int.Parse(tbEnd.Text) - 1;
                }

                if (end > res.info.Length)
                {
                    MessageBox.Show("Đơn hàng chỉ có " + res.info.Length + " acc");
                }
                else
                {
                    Writer Accmua = new Writer("sourceacc.txt");
                    for (int i = start; i <= end; i++)
                    {
                        Accmua.WriteToFileLine(res.info[i].username + "|" + res.info[i].password);
                    }
                }
                String[] username = File.ReadAllLines("sourceacc.txt");

                for (int iThr = 0; iThr < keyProxys.Length; iThr++)
                {



                    int _iThread = iThr;

                    Thread t = new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;

                        while (true)
                        {

                            ListViewItem item = new ListViewItem();
                            int indexitem = item.Index;
                            item.Text = _iThread.ToString();
                            ListViewItem.ListViewSubItem sub1 = new ListViewItem.ListViewSubItem();
                            sub1.Text = "";
                            ListViewItem.ListViewSubItem sub2 = new ListViewItem.ListViewSubItem();
                            sub2.Text = "";
                            ListViewItem.ListViewSubItem sub3 = new ListViewItem.ListViewSubItem();
                            sub3.Text = "Đang Set Up";
                            Invoke((new Action(() =>
                            {
                                item.SubItems.Add(sub1);
                                item.SubItems.Add(sub2);
                                item.SubItems.Add(sub3);

                                listView1.Items.Add(item);
                            })));
                            string _username = "";
                            string useragents = "";
                            lock (_lockNextUsernameIndex)
                            {
                                if (nextUsernameIndex >= username.Length) return;

                                _username = username[nextUsernameIndex];
                                nextUsernameIndex++;
                            }
                            lock (lockAgent)
                            {
                                if (nextList >= listUSer.Length) nextList = 0; ;

                                useragents = listUSer[nextList];
                                nextList++;
                            }

                            string keyProxy = keyProxys[_iThread];
                            String proxy = null;
                            if (cbTinsoft.Checked)
                            {
                                proxy = GET_proxy(keyProxy);
                            }
                            if (cbTm.Checked)
                            {
                                proxy = GetProxy_TM(keyProxy);
                            }
                            string[] usernamePase = _username.Split('|');
                            string[] proxyParse = proxy.Split(':');
                            FirefoxDriverService cService = FirefoxDriverService.CreateDefaultService(@"GeckoDriver19", System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\geckodriver.exe");
                            cService.FirefoxBinaryPath = @"C:\Program Files\Mozilla Firefox\firefox.exe";
                            cService.HideCommandPromptWindow = true;

                            FirefoxOptions firefoxOptions = new FirefoxOptions();
                            // firefoxOptions.AddArguments("--headless");
                            firefoxOptions.SetPreference("dom.webdriver.enabled", false);
                            firefoxOptions.SetPreference("webdriver_enable_native_events", false);
                            firefoxOptions.SetPreference("webdriver_assume_untrusted_issuer", false);
                            firefoxOptions.SetPreference("media.peerconnection.enabled", false);
                            firefoxOptions.SetPreference("media.navigator.permission.disabled", true);
                            firefoxOptions.AddArgument("--disable-blink-features=AutomationControlled");
                            firefoxOptions.SetPreference("permissions.default.stylesheet", 2);
                            firefoxOptions.SetPreference("permissions.default.image", 2);
                            firefoxOptions.SetPreference("dom.ipc.plugins.enabled.libflashplayer.so", false);

                            firefoxOptions.SetPreference("webdriver_enable_native_events", false);
                            firefoxOptions.SetPreference("webdriver_assume_untrusted_issuer", false);
                            firefoxOptions.SetPreference("media.peerconnection.enabled", false);
                            firefoxOptions.SetPreference("media.navigator.permission.disabled", true);

                            firefoxOptions.SetPreference("general.useragent.override", useragents);

                            firefoxOptions.SetPreference("network.proxy.type", 1);
                            firefoxOptions.SetPreference("network.proxy.http", proxyParse[0]);
                            firefoxOptions.SetPreference("network.proxy.http_port", int.Parse(proxyParse[1]));
                            firefoxOptions.SetPreference("network.proxy.ssl", proxyParse[0]);
                            firefoxOptions.SetPreference("network.proxy.ssl_port", int.Parse(proxyParse[1]));
                            IWebDriver driver = new FirefoxDriver(cService, firefoxOptions);

                            driver.Manage().Window.Size = new Size(1920, 1080); //tai sao lai la keyProxy ? vi so luong lay tu key keyProxys.length la so luong keyProxy.length la do dai cua key
                            driver.Manage().Window.Position = new Point(Screen.PrimaryScreen.Bounds.Width / keyProxys.Length * _iThread + 3, 0);
                            WebDriverWait wait;

                            try
                            {
                                Invoke((new Action(() =>
                                {
                                    sub1.Text = usernamePase[0];
                                    sub2.Text = usernamePase[1];
                                    sub3.Text = "Vào Đăng Nhập";

                                    item.SubItems.Add(sub1);
                                    item.SubItems.Add(sub2);
                                    item.SubItems.Add(sub3);
                                })));
                                driver.Navigate().GoToUrl("https://member-m.lazada.vn/user/login?");
                                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                                wait.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/div[2]/div/div[2]/div/div[2]/form/div/div[1]/div[1]/input")));
                                driver.FindElement(By.XPath("/html/body/div[2]/div/div[2]/div/div[2]/form/div/div[1]/div[1]/input")).SendKeys(usernamePase[0]);

                                Thread.Sleep(500);
                                driver.FindElement(By.XPath("/html/body/div[2]/div/div[2]/div/div[2]/form/div/div[1]/div[2]/input")).SendKeys(usernamePase[1]);
                                Thread.Sleep(500);
                                driver.FindElement(By.XPath("/html/body/div[2]/div/div[2]/div/div[2]/form/div/div[2]/div[1]/button")).Click();
                                Thread.Sleep(8000);
                                if (isVeryAcc(driver, "Để bảo vệ bảo mật tài khoản của bạn, chúng tôi cần xác minh danh tính của bạn"))
                                {
                                    Invoke((new Action(() =>
                                    {

                                        sub3.Text = "Acc very";
                                        item.SubItems.Add(sub1);
                                        item.SubItems.Add(sub2);
                                        item.SubItems.Add(sub3);
                                    })));
                                    AccVery.WriteToFileLine(usernamePase[0] + "|" + usernamePase[1] + "|Very");
                                    driver.Dispose();
                                    driver.Quit();
                                    Invoke((new Action(() =>
                                    {

                                        listView1.Items.RemoveAt(item.Index);

                                    })));
                                }
                                else
                                {
                                    try
                                    {
                                        Invoke((new Action(() =>
                                        {

                                            sub3.Text = "Dung Lượng";
                                            item.SubItems.Add(sub1);
                                            item.SubItems.Add(sub2);
                                            item.SubItems.Add(sub3);
                                        })));
                                        driver.FindElement(By.ClassName("sufei-dialog")).Click();
                                        ChuaDoiDuoc.WriteToFileLine(usernamePase[0] + "|" + usernamePase[1] + "|" + usernamePase[2] + "|" + usernamePase[3] + "|" + usernamePase[4] + "|" + usernamePase[5] + "|Dung Luong");
                                        driver.Dispose();
                                        driver.Quit();
                                        Invoke((new Action(() =>
                                        {

                                            listView1.Items.RemoveAt(item.Index);

                                        })));
                                    }
                                    catch
                                    {
                                        if (isVeryAcc(driver, "Chào mừng đến với Lazada. Đăng nhập ngay!"))
                                        {
                                            Invoke((new Action(() =>
                                            {

                                                sub3.Text = "Sai Pass";
                                                item.SubItems.Add(sub1);
                                                item.SubItems.Add(sub2);
                                                item.SubItems.Add(sub3);
                                            })));
                                            AccSaiPass.WriteToFileLine(usernamePase[0] + "|" + usernamePase[1] + "|Sai pass");
                                            driver.Quit();
                                            driver.Dispose();
                                            Invoke((new Action(() =>
                                            {

                                                listView1.Items.RemoveAt(item.Index);

                                            })));
                                        }
                                        else
                                        {
                                            try
                                            {
                                                Invoke((new Action(() =>
                                                {

                                                    sub3.Text = "Vào Trang Mã Giảm Giá";
                                                    item.SubItems.Add(sub1);
                                                    item.SubItems.Add(sub2);
                                                    item.SubItems.Add(sub3);
                                                })));
                                                int count = 0;
                                                String[] Xpaths = tbXpath.Text.Split('|');
                                                driver.Navigate().GoToUrl("https://pages.lazada.vn/wow/gcp/route/lazada/vn/upr_1000345_lazada/channel/vn/upr-router/vn?spm=a2o4n.tm80010326.menu.2.3d9669e2qlG9sU&hybrid=1&data_prefetch=true&wh_pid=/lazada/channel/vn/voucher/claimvoucher&scm=1003.4.icms-zebra-5000379-2586391.OTHER_6042140477_7211275");
                                                wait.Until(ExpectedConditions.ElementExists(By.XPath(Xpaths[0])));
                                                for (int z = 0; z < Xpaths.Length; z++)
                                                {
                                                    try
                                                    {
                                                        driver.FindElement(By.XPath(Xpaths[z])).Click();
                                                        count++;
                                                    }
                                                    catch
                                                    {


                                                    }

                                                }

                                                Getmaok.WriteToFileLine(usernamePase[0] + "|" + usernamePase[1] + "|" + count.ToString());
                                                Thread.Sleep(2000);
                                                driver.Quit();
                                                driver.Dispose();
                                                Invoke((new Action(() =>
                                                {

                                                    sub3.Text = "Lấy Thành công " + count.ToString() + " mã";
                                                    item.SubItems.Add(sub1);
                                                    item.SubItems.Add(sub2);
                                                    item.SubItems.Add(sub3);
                                                })));
                                                Thread.Sleep(2000);
                                                Invoke((new Action(() =>
                                                {

                                                    listView1.Items.RemoveAt(item.Index);

                                                })));
                                            }
                                            catch
                                            {
                                                Invoke((new Action(() =>
                                                {

                                                    sub3.Text = "Không Tìm Thấy Voucher";
                                                    item.SubItems.Add(sub1);
                                                    item.SubItems.Add(sub2);
                                                    item.SubItems.Add(sub3);
                                                })));
                                                AccChuaMua.WriteToFileLine(usernamePase[0] + "|" + usernamePase[1]);
                                                Invoke((new Action(() =>
                                                {

                                                    listView1.Items.RemoveAt(item.Index);

                                                })));
                                            }


                                        }

                                    }





                                }



                            }
                            catch
                            {
                                Invoke((new Action(() =>
                                {

                                    listView1.Items.RemoveAt(item.Index);

                                })));
                                ChuaDoiDuoc.WriteToFileLine(_username);
                                driver.Dispose();
                                driver.Quit();

                            }

                        }




                    });

                    t.Start();



                }
            }




        }
        private void button3_Click(object sender, EventArgs e)
        {
            AdbCommand("proxy.txt");
        }
        void DeleteAllMail(String username, String password)
        {
            MailServer oServer = new MailServer("imap-mail.outlook.com",
                  username, password, ServerProtocol.Imap4);
            oServer.SSLConnection = true;
            oServer.Port = 993;
            MailClient oClient = new MailClient("TryIt");
            oClient.Connect(oServer);

            MailInfo[] infos = oClient.GetMailInfos();
            for (int i = 0; i < infos.Length; i++)
            {
                MailInfo info = infos[i];

                oClient.Delete(info);
            }
        }
        String getOTPHotmail(String username, String password)
        {
            String otp = null;
            MailServer oServer = new MailServer("imap-mail.outlook.com",
                  username, password, ServerProtocol.Imap4);
            oServer.SSLConnection = true;
            oServer.Port = 993;
            MailClient oClient = new MailClient("TryIt");
            oClient.Connect(oServer);

            MailInfo[] infos = oClient.GetMailInfos();
            if (infos.Length > 0)
            {
                for (int i = 0; i < infos.Length; i++)
                {
                    MailInfo info = infos[i];
                    Mail oMail = oClient.GetMail(info);
                    if (oMail.From.Name == "Lazada Vietnam")
                    {
                        otp = Regex.Match(oMail.TextBody, @"\d{6}").ToString();
                    }

                    // Mark email as deleted from IMAP4 server.
                    oClient.Delete(info);
                }
            }
            else
            {
                otp = null;
            }

            return otp;
        }
        void AdbCommand(string content) // FASTBOOT
        {
            string cmdCommand = content;

            Process cmd = new Process();

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;

            cmd.StartInfo = startInfo;
            cmd.Start();

            cmd.StandardInput.WriteLine(cmdCommand);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
        }
        Random r = new Random();
        string randomsdt()
        {

            string sdt = null;
            string chars = "0123456789";
            string[] dauso = { "086", "082", "038", "056", "088", "077", "032", "033", "034", "035", "037", "038", "039", "096", "091", "094" };
            string nums = null;
            for (int i = 0; i < 7; i++)
            {
                nums += chars.Substring(r.Next(0, chars.Length - 1), 1);
            }
            sdt = dauso[r.Next(0, dauso.Length)] + nums;

            return sdt;
        }


        string DiaChi()
        {
            String[] addresss = File.ReadAllLines("address.txt");
            string diachi = null;
            string[] cucoc = { "Tòa nhà", "Chung Cư", "Căn hộ", "Ngách", "Ngõ", "Hộ gia cư" };
            diachi = cucoc[r.Next(0, cucoc.Length)] + " số " + r.Next(0, 1000).ToString() + "," + " Đường " + addresss[r.Next(0, addresss.Length)] + " Tầng " + r.Next(0, 100).ToString() + " Lô " + r.Next(0, 100).ToString() + "/" + r.Next(0, 100).ToString();

            return diachi;
        }
        private void button4_Click(object sender, EventArgs e)
        {

            ThemDiaChi();


        }
        string randomStrNum(int stringlength) //
        {

            string chars = "0123456789abcdefghijklmnopqrstuvwxtz";
            string nums = null;
            for (int i = 0; i < stringlength; i++)
            {
                nums += chars.Substring(r.Next(0, chars.Length - 1), 1);
            }
            return nums;
        }
        // Random_Adress.Form1
        // Token: 0x06000004 RID: 4 RVA: 0x00002210 File Offset: 0x00000410
        string SDTRandom()
        {
            string chars = "0123456789";
            string[] dauso = new string[]
            {
        "086",
        "082",
        "038",
        "056",
        "088",
        "077",
        "032",
        "033",
        "034",
        "035",
        "037",
        "038",
        "039",
        "096",
        "091",
        "094"
            };
            string nums = null;
            for (int i = 0; i < 7; i++)
            {
                nums += chars.Substring(this.r.Next(0, chars.Length - 1), 1);
            }
            return dauso[this.r.Next(0, dauso.Length)] + nums;
        }
        String randomphuong()
        {
            string phuong = null;

            string[] sophuong = File.ReadAllLines("PhuongNgauNhien.txt");

            for (int i = 0; i < sophuong.Length; i++)
            {
                phuong = sophuong[r.Next(0, sophuong.Length)];
            }
            return phuong;
        }


        string randomTenCoDau()
        {
            string[] array = File.ReadAllLines("TenCoDau.txt");
            string[] array2 = new string[]
            {
        "Q",
        "W",
        "E",
        "R",
        "T",
        "Y",
        "U",
        "I",
        "O",
        "P",
        "A",
        "S",
        "D",
        "F",
        "G",
        "H",
        "J",
        "K",
        "L",
        "X",
        "C",
        "V",
        "B",
        "N",
        "M"
            };
            return this.r.Next(10, 99).ToString() +array2[r.Next(0,array2.Length)]+" "+ this.r.Next(10, 99).ToString() + array2[r.Next(0, array2.Length)] + " " + this.r.Next(10, 99).ToString() + array2[r.Next(0, array2.Length)] + " " + this.r.Next(10, 99).ToString() + array2[r.Next(0, array2.Length)] + " " +  array[this.r.Next(0, array.Length)];
        }
        String RandomTen()
        {
            string FileHo = File.ReadAllText("Ho.txt");
            string[] FileHoNe = FileHo.Split(new string[]
            {
        "\r\n",
        "\r",
        "\n"
            }, StringSplitOptions.None);
            string FileDemTen = File.ReadAllText("DemTen.txt");
            string[] FileDemTenNe = FileDemTen.Split(new string[]
            {
        "\r\n",
        "\r",
        "\n"
            }, StringSplitOptions.None);
            string Hone = null;
            for (int i = 0; i < FileHoNe.Length; i++)
            {
                Hone = FileHoNe[this.r.Next(0, FileHoNe.Length)];
            }
            string tenne = null;
            for (int j = 0; j < FileDemTenNe.Length; j++)
            {
                tenne = FileDemTenNe[this.r.Next(0, FileDemTenNe.Length)];
            }
            return Hone + " " + tenne;
        }
        void ThemDiaChi()
        {
            try
            {
                String XpathThanhPho = File.ReadAllText("XpathThanhPho.txt");
                String XpathHuyen = File.ReadAllText("XpathHuyen.txt");
                String[] XpathXa = File.ReadAllLines("XpathXa.txt");
                //Useragent
                string[] keyProxys = File.ReadAllLines("proxy.txt");


                String[] username = File.ReadAllLines("sourceacc.txt");
                for (int iThr = 0; iThr < keyProxys.Length; iThr++)
                {



                    int _iThread = iThr;

                    Thread t = new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;

                        while (true)
                        {
                            ListViewItem item = new ListViewItem();
                            int indexitem = item.Index;
                            item.Text = _iThread.ToString();
                            ListViewItem.ListViewSubItem sub1 = new ListViewItem.ListViewSubItem();
                            sub1.Text = "";
                            ListViewItem.ListViewSubItem sub2 = new ListViewItem.ListViewSubItem();
                            sub2.Text = "";
                            ListViewItem.ListViewSubItem sub3 = new ListViewItem.ListViewSubItem();
                            sub3.Text = "Đang Set Up";
                            Invoke((new Action(() =>
                            {
                                item.SubItems.Add(sub1);
                                item.SubItems.Add(sub2);
                                item.SubItems.Add(sub3);

                                listView1.Items.Add(item);
                            })));
                            string _username = "";
                         
                            String buildd = randomStrNum(9);
                            String devices = randomStrNum(4);
                            string user = "Mozilla/5.0 (Linux; U; Android 4.3; en-us; SM-" + devices + " Build/" + buildd + ") AppleWebKit/534.30 (KHTML, like Gecko) Version/4.0 Mobile Safari/534.30";
                            //string useragents = user;
                            lock (_lockNextUsernameIndex)
                            {
                                if (nextUsernameIndex >= username.Length) return;

                                _username = username[nextUsernameIndex];
                                nextUsernameIndex++;
                            }


                            string keyProxy = keyProxys[_iThread];
                            string[] usernamePase = _username.Split('|');
                            String proxy = null;
                            if (cbTinsoft.Checked)
                            {
                                proxy = GetProxyShopLike(keyProxy);
                            }
                            if (cbTm.Checked)
                            {
                                proxy = GetProxy_TM(keyProxy);
                            }


                            Invoke((new Action(() =>
                            {
                                sub1.Text = usernamePase[0];
                                sub2.Text = usernamePase[1];
                                sub3.Text = "Fake ip:" + proxy;

                                item.SubItems.Add(sub1);
                                item.SubItems.Add(sub2);
                                item.SubItems.Add(sub3);
                            })));
                            string[] proxyParse = proxy.Split(':');
                            FirefoxDriverService cService = FirefoxDriverService.CreateDefaultService(@"GeckoDriver19", System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\geckodriver.exe");
                            cService.FirefoxBinaryPath = @"C:\Program Files\Mozilla Firefox\firefox.exe";
                            cService.HideCommandPromptWindow = true;

                            FirefoxOptions firefoxOptions = new FirefoxOptions();
                            //firefoxOptions.AddArguments("--headless");
                            firefoxOptions.SetPreference("dom.webdriver.enabled", false);
                            firefoxOptions.SetPreference("webdriver_enable_native_events", false);
                            firefoxOptions.SetPreference("webdriver_assume_untrusted_issuer", false);
                            firefoxOptions.SetPreference("media.peerconnection.enabled", false);
                            firefoxOptions.SetPreference("media.navigator.permission.disabled", true);
                            firefoxOptions.AddArgument("--disable-blink-features=AutomationControlled");
                            firefoxOptions.SetPreference("permissions.default.stylesheet", 2);
                            firefoxOptions.SetPreference("permissions.default.image", 2);
                            firefoxOptions.SetPreference("dom.ipc.plugins.enabled.libflashplayer.so", false);

                            firefoxOptions.SetPreference("webdriver_enable_native_events", false);
                            firefoxOptions.SetPreference("webdriver_assume_untrusted_issuer", false);
                            firefoxOptions.SetPreference("media.peerconnection.enabled", false);
                            firefoxOptions.SetPreference("media.navigator.permission.disabled", true);

                            firefoxOptions.SetPreference("general.useragent.override", user);

                            firefoxOptions.SetPreference("network.proxy.type", 1);
                            firefoxOptions.SetPreference("network.proxy.http", proxyParse[0]);
                            firefoxOptions.SetPreference("network.proxy.http_port", int.Parse(proxyParse[1]));
                            firefoxOptions.SetPreference("network.proxy.ssl", proxyParse[0]);
                            firefoxOptions.SetPreference("network.proxy.ssl_port", int.Parse(proxyParse[1]));

                            IWebDriver driver = new FirefoxDriver(cService, firefoxOptions);

                            driver.Manage().Window.Size = new Size(400, 1000); //tai sao lai la keyProxy ? vi so luong lay tu key keyProxys.length la so luong keyProxy.length la do dai cua key
                            driver.Manage().Window.Position = new Point(Screen.PrimaryScreen.Bounds.Width / keyProxys.Length * _iThread + 3, 0);
                            WebDriverWait wait;

                            try
                            {
                                Invoke((new Action(() =>
                                {
                                    sub1.Text = usernamePase[0];
                                    sub2.Text = usernamePase[1];
                                    sub3.Text = "Vào Đăng Nhập";

                                    item.SubItems.Add(sub1);
                                    item.SubItems.Add(sub2);
                                    item.SubItems.Add(sub3);
                                })));
                                //driver.Navigate().GoToUrl("https://facebook.com");
                                //Thread.Sleep(2000);
                                //String[] dataccounts = _username.Split('|');


                                //String cookie = dataccounts[2];
                                //var temp = cookie.Split(';');
                                //foreach (var item1 in temp)
                                //{
                                //    var temp2 = item1.Split('=');
                                //    if (temp2.Count() > 1)
                                //    {
                                //        var cooke = new Cookie(temp2[0].Trim(), temp2[1].Trim());
                                //        driver.Manage().Cookies.AddCookie(cooke);
                                //    }
                                //}
                                driver.Navigate().GoToUrl("https://member-m.lazada.vn/user/login?");
                                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

                                wait.Until(ExpectedConditions.ElementExists(By.XPath(@"/html/body/div[1]/div/div/div/div/div[3]/div/div[2]/div[1]")));
                                driver.FindElement(By.XPath(@"/html/body/div[1]/div/div/div/div/div[3]/div/div[2]/div[1]")).Click();
                                ////*[@id="m_login_email"]
                                wait.Until(ExpectedConditions.ElementExists(By.XPath(@"//*[@id=""m_login_email""]")));
                                driver.FindElement(By.XPath(@"//*[@id=""m_login_email""]")).SendKeys(usernamePase[0]);
                                Thread.Sleep(500);
                                driver.FindElement(By.XPath(@"//*[@id=""m_login_password""]")).SendKeys(usernamePase[1]);
                                Thread.Sleep(500);
                                driver.FindElement(By.XPath(@"/html/body/div[1]/div/div[3]/div[1]/div/div[2]/div/div/form/div[5]/div[1]/button")).Click();
                                Thread.Sleep(8000);

                                wait.Until(ExpectedConditions.ElementExists(By.XPath(@"/html/body/div/div/div[3]/div/div[1]/form/div/div/div[1]/div/section[2]/div[1]/div/div/div[2]/div/div/a/div/span")));
                                driver.FindElement(By.XPath(@"/html/body/div/div/div[3]/div/div[1]/form/div/div/div[1]/div/section[2]/div[1]/div/div/div[2]/div/div/a/div/span")).Click();
                                Thread.Sleep(500);
                                driver.FindElement(By.XPath(@"/html/body/div/div/div[3]/div/div[1]/form/div/div[2]/div[1]/div/section[1]/div/div/div/fieldset/label[2]/div/div[2]/div/span")).Click();
                                Thread.Sleep(500);
                                driver.FindElement(By.XPath(@"/html/body/div/div/div[3]/div/div[1]/form/div/div[2]/div[2]/footer/div/div[1]/button")).Click();
                                Thread.Sleep(500);
                                wait.Until(ExpectedConditions.ElementExists(By.XPath(@"/html/body/div[2]/div[2]/div/span")));
                                driver.FindElement(By.XPath(@"/html/body/div[2]/div[2]/div/span")).Click();

                                Thread.Sleep(4000);
                                if (driver.PageSource.IndexOf("baxia-dialog-content") > -1)
                                {
                                    driver.SwitchTo().Frame("baxia-dialog-content");
                                    Thread.Sleep(2000);
                                    IWebElement slider = driver.FindElement(By.Id("nc_1_n1t"));
                                    Actions action = new Actions(driver);
                                    action.DragAndDropToOffset(slider, 340, 45).Perform();

                                    driver.SwitchTo().DefaultContent();

                                }
                                String name = RandomTen();
                                Thread.Sleep(2000);
                                wait.Until(ExpectedConditions.ElementExists(By.XPath(@"/html/body/div[2]/div/ul[1]/li[1]/a")));
                                driver.Navigate().GoToUrl("https://member.lazada.vn/address?#/create");
                                wait.Until(ExpectedConditions.ElementExists(By.XPath(@"//*[@id=""container""]/div/div[2]/div[1]/div")));
                                Thread.Sleep(500);
                                driver.FindElement(By.XPath(@"//*[@id=""container""]/div/div[2]/div[1]/div")).Click();
                                Thread.Sleep(1200);
                                driver.FindElement(By.XPath(@"//*[@id=""container""]/div/div/div/div[1]/div[1]/input")).SendKeys(name);
                                Thread.Sleep(200);
                                driver.FindElement(By.XPath(@"//*[@id=""container""]/div/div/div/div[1]/div[2]/input")).SendKeys(randomTenCoDau() + tbKiHieu.Text);
                                Thread.Sleep(500);
                                driver.FindElement(By.XPath(@"//*[@id=""container""]/div/div/div/div[1]/div[3]/select")).Click();
                                Thread.Sleep(500);
                                driver.FindElement(By.XPath(XpathThanhPho)).Click(); // Ha Noi
                                Thread.Sleep(1200);
                                driver.FindElement(By.XPath(@"//*[@id=""container""]/div/div/div/div[1]/div[4]/select")).Click();
                                Thread.Sleep(500);
                                driver.FindElement(By.XPath(XpathHuyen)).Click(); // thanh xuân, edit chỗ này
                                Thread.Sleep(1200);
                                driver.FindElement(By.XPath(@"//*[@id=""container""]/div/div/div/div[1]/div[5]/select")).Click();
                                Thread.Sleep(1200);
                                string phuong = randomphuong();
                                driver.FindElement(By.XPath(XpathXa[r.Next(0, XpathXa.Length)])).Click(); // Có thể sửa Random nhiều phường bằng dấu |
                                Thread.Sleep(500);

                                driver.FindElement(By.XPath(@"//*[@id=""container""]/div/div/div/div[1]/div[6]/input")).SendKeys(randomsdt());
                                Thread.Sleep(400);
                                driver.FindElement(By.XPath(@"//*[@id=""container""]/div/div/div/div[3]/button")).Click();
                                Thread.Sleep(4000);
                                String mpvc = "";
                                try
                                {
                                    driver.Navigate().GoToUrl("https://s.lazada.vn/s.05oq4");
                                    wait.Until(ExpectedConditions.ElementExists(By.XPath(@"/html/body/section/div[2]/div/div/div/div[2]/div/div/div/a[1]/div/div[2]/div/span")));
                                    Thread.Sleep(400);
                                    for (int m = 0; m < 7; m++)
                                    {
                                        try
                                        {
                                            driver.FindElement(By.XPath($"/html/body/section/div[2]/div/div/div/div[2]/div/div/div/a[{m.ToString()}]/div/div[2]/div/span")).Click();
                                            Thread.Sleep(400);
                                        }
                                        catch
                                        {


                                        }
                                    }

                                    mpvc = "Mpvc";


                                }
                                catch
                                {


                                }
                                String Rename = "";
                                try
                                {

                                    driver.Navigate().GoToUrl("https://my-m.lazada.vn/member/account-info?");
                                    Thread.Sleep(3000);
                                    while(driver.PageSource.IndexOf("baxia-dialog-content") > -1)
                                    {
                                      
                                        Thread.Sleep(3000);
                                        driver.SwitchTo().Frame("baxia-dialog-content");
                                        Thread.Sleep(2000);
                                        IWebElement slider = driver.FindElement(By.Id("nc_1_n1z"));
                                        Actions action = new Actions(driver);
                                        action.DragAndDropToOffset(slider, 340, 45).Perform();

                                        driver.SwitchTo().DefaultContent();
                                        Thread.Sleep(1000);
                                        driver.Navigate().Refresh();

                                    }
                                    //if (driver.PageSource.IndexOf("baxia-dialog-content") > -1)
                                    //{
                                       

                                    //}
                                    Thread.Sleep(4000);
                                    wait.Until(ExpectedConditions.ElementExists(By.XPath(@"/html/body/div[2]/div[1]/div[1]/div[1]")));
                                    Thread.Sleep(400);

                                    driver.FindElement(By.XPath(@"/html/body/div[2]/div[1]/div[1]/div[1]")).Click();
                                    Thread.Sleep(1000);
                                    driver.FindElement(By.XPath(@"/html/body/div[2]/div[1]/div[1]/div[2]/div/div/div/div/div[2]/div/div[1]/div/img")).Click();
                                    Thread.Sleep(500);
                                    //
                                    driver.FindElement(By.XPath(@"/html/body/div[2]/div[1]/div[1]/div[2]/div/div/div/div/div[2]/div/div[1]/div/input")).SendKeys(name);
                                    Thread.Sleep(1000);
                                    ///html/body/div[2]/div[1]/div[1]/div[2]/div/div/div/div/div[2]/div/div[2]/span
                                    driver.FindElement(By.XPath(@"/html/body/div[2]/div[1]/div[1]/div[2]/div/div/div/div/div[2]/div/div[2]/span")).Click();
                                    Thread.Sleep(2000);
                                    
                                    Thread.Sleep(1000);
                                    driver.FindElement(By.XPath($"/html/body/div[2]/div[2]/div[2]/span[2]")).Click();
                                    Thread.Sleep(1000);
                                    driver.FindElement(By.XPath(@"/html/body/div[2]/div/div/div[2]/div[1]/div/div/div/input")).SendKeys(usernamePase[2]);
                                    Thread.Sleep(3000);
                                    DeleteAllMail(usernamePase[2], usernamePase[3]);
                                    Thread.Sleep(1000);
                                    driver.FindElement(By.XPath($"/html/body/div[2]/div/div/div[2]/div[2]/div/div/div/span")).Click();
                                    //getOTPHotmail
                                    Thread.Sleep(6000);
                                    driver.FindElement(By.XPath(@"/html/body/div[2]/div/div/div[2]/div[2]/div/div[1]/div/input")).SendKeys(getOTPHotmail(usernamePase[2], usernamePase[3]));
                                    Thread.Sleep(1000);
                                    driver.FindElement(By.XPath($"/html/body/div[2]/div/div/div[3]/div/span")).Click();
                                    Thread.Sleep(1000);
                                    //driver.Navigate().GoToUrl("https://member.lazada.vn/user/forget-password?spm=a2o4n.login_signup.0.0.4ac05d0aHwGnqv");
                                    Thread.Sleep(5000);
                                    //driver.FindElement(By.XPath(@"/html/body/div[2]/div/div/div[1]/div/input")).SendKeys(usernamePase[2]);
                                    //IWebElement slider = driver.FindElement(By.Id("nc_1_n1t"));
                                    //Actions action1 = new Actions(driver);
                                    //action1.DragAndDropToOffset(slider, 340, 45).Perform();
                                    Rename = "Rename";


                                }
                                catch
                                {


                                }
                                ThemThanhCong.WriteToFileLine(usernamePase[2] + "|" + usernamePase[3] + "|" + mpvc + "|" + Rename);
                                Invoke((new Action(() =>
                                {
                                    sub1.Text = usernamePase[0];
                                    sub2.Text = usernamePase[1];
                                    sub3.Text = "Thêm Thành Công";

                                    item.SubItems.Add(sub1);
                                    item.SubItems.Add(sub2);
                                    item.SubItems.Add(sub3);
                                })));
                                driver.Dispose();
                                driver.Quit();

                                Thread.Sleep(2000);










                            }
                            catch
                            {

                                Invoke((new Action(() =>
                                {
                                    sub1.Text = usernamePase[0];
                                    sub2.Text = usernamePase[1];
                                    sub3.Text = "Lỗi Không Xác Định";

                                    item.SubItems.Add(sub1);
                                    item.SubItems.Add(sub2);
                                    item.SubItems.Add(sub3);
                                })));
                                ChuaDoiDuoc.WriteToFileLine(_username);
                                driver.Dispose();
                                driver.Quit();


                            }

                        }




                    });

                    t.Start();



                }
            }
            catch 
            {

                
            }
           


        }
        int indexProvince;
        String province = null;
        int indexDistrics;
        String Districs = null;
        int indexwards;
        String wards = null;


        void LoadAddress()
        {
            String data = File.ReadAllText(@"Local\local.json");
            var res = JsonConvert.DeserializeObject<LocalVietNam>(data);
            for (int i = 0; i < res.data.Length; i++)
            {

                comboBox1.Items.Add(res.data[i].name);
            }

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Invoke(new Action(() =>
            {
                comboBox2.Text = "";
            }));
            comboBox2.Items.Clear();
            ComboBox cb = sender as ComboBox;
            indexProvince = cb.SelectedIndex;


            String data = File.ReadAllText(@"Local\local.json");
            var res = JsonConvert.DeserializeObject<LocalVietNam>(data);
            for (int i = 0; i < res.data[indexProvince].districts.Length; i++)
            {

                comboBox2.Items.Add(res.data[indexProvince].districts[i].name);
            }
            province = res.data[indexProvince].id;

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Invoke(new Action(() =>
            {
                comboBox3.Text = "";
            }));
            comboBox3.Items.Clear();
            ComboBox cb = sender as ComboBox;
            indexDistrics = cb.SelectedIndex;

            String data = File.ReadAllText(@"Local\local.json");
            var res = JsonConvert.DeserializeObject<LocalVietNam>(data);
            for (int i = 0; i < res.data[indexProvince].districts[indexDistrics].wards.Length; i++)
            {

                comboBox3.Items.Add(res.data[indexProvince].districts[indexDistrics].wards[i].prefix + " " + res.data[indexProvince].districts[indexDistrics].wards[i].name);
            }
            Districs = res.data[indexProvince].districts[indexDistrics].id;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            indexwards = cb.SelectedIndex;
            String data = File.ReadAllText(@"Local\local.json");
            var res = JsonConvert.DeserializeObject<LocalVietNam>(data);
            wards = res.data[indexProvince].districts[indexDistrics].wards[indexwards].id;

        }

        private void button5_Click(object sender, EventArgs e)
        {
            AdbCommand("cd AddDiaChi & themthanhcong.txt");
        }

        private void button6_Click(object sender, EventArgs e)
        {

            AdbCommand("cd DaLayMa &LayThanhCong.txt");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AdbCommand("sourceacc.txt");
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            string ccv = randomTenCoDau();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            MessageBox.Show(randomTenCoDau());
        }
    }
    public class TmProxy
    {
        public int code { get; set; }
        public string message { get; set; }
        public DataProxy data { get; set; }
    }
    public class DataProxy
    {
        public string ip_allow { get; set; }
        public string location_name { get; set; }
        public string socks5 { get; set; }
        public string https { get; set; }
        public int timeout { get; set; }
        public int next_request { get; set; }
        public string expired_at { get; set; }
    }
    public class DataOder
    {
        public bool status { get; set; }
        public string oderid { get; set; }
        public Info[] info { get; set; }
    }

    public class Info
    {
        public string username { get; set; }
        public string password { get; set; }
        public string upload { get; set; }
    }

    public class SellClone
    {
        public bool status { get; set; }
        public String token { get; set; }
    }

    public class SellCloneData
    {
        public bool status { get; set; }
        public Datum[] data { get; set; }
    }

    public class Datum
    {
        public string role { get; set; }
        public int balance { get; set; }
        public string[] historybuy { get; set; }
        public string[] historytopup { get; set; }
        public string _id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int __v { get; set; }
    }
    public class ProxyShopLike
    {
        public string status { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public string location { get; set; }
        public string proxy { get; set; }
        public string auth { get; set; }
    }
    public class Writer
    {
        public string Filepath { get; set; }
        private static object locker = new Object();

        public Writer(string filepath)
        {
            this.Filepath = filepath;
        }

        public void WriteToFile(string text)
        {
            lock (locker)
            {
                using (FileStream file = new FileStream(Filepath, FileMode.Append, FileAccess.Write, FileShare.Read))
                using (StreamWriter writer = new StreamWriter(file, Encoding.Unicode))
                {
                    writer.Write(text.ToString());
                }
            }

        }
        public void WriteToFileLine(string text)
        {
            WriteToFile(text + "\r\n");

        }
    }
}
