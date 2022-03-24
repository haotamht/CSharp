using KAutoHelper;
using Newtonsoft.Json;
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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        Object locker = new object();
        private void button1_Click(object sender, EventArgs e)
        {
            string[] keyProxys = keyproxytm.Text.Split(
                     new[] { "\r\n", "\r", "\n" },
                     StringSplitOptions.None
                 );
            string[] ListAcc = accfb.Text.Split(
                new[] { "\r\n", "\r", "\n" },
                    StringSplitOptions.None);

            List<String> Devices = KAutoHelper.ADBHelper.GetDevices();
            int dem = 0;
            String[] Device = new String[Devices.Count];
            foreach (var item in Devices)
            {
                Device[dem] = item;
                dem++;
            }
            int nextIndex = 0;
            for (int i = 0; i < keyProxys.Length; i++)
            {
                try
                {
                    int iThread = i;
                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        while (true)
                        {


                            string userName = null;
                            lock (locker)
                            {
                                if (nextIndex >= ListAcc.Length) return;
                                userName = ListAcc[nextIndex];
                                nextIndex++;
                            }
                            String[] Parse = userName.Split('|');
                            String proxy = GetProxy_TM(keyProxys[iThread]);
                            String[] ProxyParse = proxy.Split(':');
                            Invoke(new Action(() =>
                            {
                                //richTextBox1.Text += "Đang Fake IP:PORT " + proxy + " cho devices " + Device[iThread] + '\n';
                            }));
                            Fakeip(Device[iThread], ProxyParse[0], ProxyParse[1]);
                            Invoke(new Action(() =>
                            {
                                richTextBox1.Text += "Đa Fake IP:PORT " + proxy + " cho devices " + Device[iThread] + '\n';
                            }));
                            Reglzd(Device[iThread], otpsim.Text, loaisim.Text, Parse[0], Parse[1], get2fa(Parse[2]));
                            Thread.Sleep(2000);
                            RemoveIp(Device[iThread]);
                            Thread.Sleep(2000);
                            AdbCommand("adb -s " + Device[iThread] + " shell pm clear com.android.browser");
                            Thread.Sleep(1000);
                            AdbCommand("adb -s " + Device[iThread] + " shell pm clear com.android.browser");
                            Thread.Sleep(2000);
                            Invoke(new Action(() =>
                            {
                                richTextBox1.Text += "Dong LD " + Device[iThread] + '\n';
                            }));
                            ldplayer.Close("name", Device[iThread]);
                            Thread.Sleep(5000);
                            String imei = "8651660" + randomStrNum(9);
                            Invoke(new Action(() =>
                            {
                                richTextBox1.Text += "change property " + imei + '\n';
                            }));
                            ldplayer.Change_Property("name", Device[iThread], "--imei " + imei + " --model " + imei + " --manufacturer " + imei);
                            Invoke(new Action(() =>
                            {
                                richTextBox1.Text += "Mo LD " + imei + '\n';
                            }));
                            ldplayer.Open("name", Device[iThread]);
                            Invoke(new Action(() =>
                            {
                                richTextBox1.Text += "Cho 30s " + '\n';
                            }));
                            Thread.Sleep(30000);





                        }



                    }).Start();
                }
                catch (Exception err)
                {

                    Invoke(new Action(() =>
                    {
                        richTextBox1.Text += err.ToString();
                    }));
                }

            }
        }
        string get2fa(string ma2fa)
        {
            HttpRequest http = new HttpRequest();
            string html = http.Get("http://2fa.live/tok/" + ma2fa).ToString();
            var result = JsonConvert.DeserializeObject<code2fa>(html);
            return result.token;
        }

        Writer write = new Writer("Acc//account.txt");
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

                        Invoke(new Action(() =>
                        {

                            richTextBox1.Text += "Chờ " + time + '\n';
                        }));
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


        void AdbCommand(string command) // FASTBOOT
        {
            string cmdCommand = command;

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
        void Fakeip(String devices, String ip, String port)
        {
            AdbCommand("adb -s " + devices + " shell settings put global http_proxy " + ip + ":" + port);
        }
        void RemoveIp(String devices)
        {
            AdbCommand("adb -s " + devices + " shell settings put global http_proxy :0");
        }

        LDPlayer ldplayer = new LDPlayer();

        string randomStrNum(int stringlength)
        {
            Random r = new Random();
            string chars = "0123456789";
            string nums = null;
            for (int i = 0; i < stringlength; i++)
            {
                nums += chars.Substring(r.Next(0, chars.Length - 1), 1);
            }
            return nums;
        }
        void Reglzd(String deviceID, String keyapi, String network, string acc, String pass, String fa)
        {
            try
            {
                AdbCommand("adb -s " + deviceID + " shell am start -n com.android.browser/com.android.browser.BrowserActivity");
                Thread.Sleep(4000);
                KAutoHelper.ADBHelper.TapByPercent(deviceID, 39.8, 7.7);
                Thread.Sleep(200);
                KAutoHelper.ADBHelper.InputText(deviceID, "https://bit.ly/3vVOY1M");
                Thread.Sleep(5000);
                KAutoHelper.ADBHelper.Key(deviceID, KAutoHelper.ADBKeyEvent.KEYCODE_ENTER);
                Thread.Sleep(4000);
                KAutoHelper.ADBHelper.TapByPercent(deviceID, 29.8, 51.9);
                KAutoHelper.ADBHelper.InputText(deviceID, acc);
                Thread.Sleep(500);
                KAutoHelper.ADBHelper.TapByPercent(deviceID, 31.3, 58.9);
                KAutoHelper.ADBHelper.InputText(deviceID, pass);
                Thread.Sleep(2000);
                KAutoHelper.ADBHelper.TapByPercent(deviceID, 45.5, 66.8);
                Thread.Sleep(2000);
                KAutoHelper.ADBHelper.TapByPercent(deviceID, 23.8, 41.3);
                Thread.Sleep(4000);
                KAutoHelper.ADBHelper.InputText(deviceID, fa);
                Thread.Sleep(4000);
                KAutoHelper.ADBHelper.TapByPercent(deviceID, 44.3, 63.3);
                Thread.Sleep(2000);
                KAutoHelper.ADBHelper.TapByPercent(deviceID, 44.0, 60.1);
                Thread.Sleep(4000);
                KAutoHelper.ADBHelper.TapByPercent(deviceID, 50.0, 47.4);
                Thread.Sleep(2000);
                KAutoHelper.ADBHelper.TapByPercent(deviceID, 89.4, 45.7);
                Thread.Sleep(2000);
                KAutoHelper.ADBHelper.TapByPercent(deviceID, 46.4, 59.9);
                Thread.Sleep(4000);
                KAutoHelper.ADBHelper.TapByPercent(deviceID, 46.7, 82.6);
                Thread.Sleep(6000);
                KAutoHelper.ADBHelper.TapByPercent(deviceID, 44.9, 8.1);
                Thread.Sleep(500);
                KAutoHelper.ADBHelper.InputText(deviceID, "https://my-m.lazada.vn/member/info-change?wh_weex=true&chooseType=none&changeType=phone&phone=&email=&spm=a2o4n.account_info.change_mobile.1");
                Thread.Sleep(500);
                KAutoHelper.ADBHelper.Key(deviceID, KAutoHelper.ADBKeyEvent.KEYCODE_ENTER);
                Thread.Sleep(5000);
                KAutoHelper.ADBHelper.TapByPercent(deviceID, 22.9, 43.3);
                Thread.Sleep(5000);
                OtpSim otps = new OtpSim();
                String phone = null;
                String session = null;
                phone = otps.GetPhone(keyapi, "2", network);
                session = otps.session;
                KAutoHelper.ADBHelper.InputText(deviceID, phone);
                Thread.Sleep(200);
                KAutoHelper.ADBHelper.TapByPercent(deviceID, 85.5, 52.8);
                String otp = null;
                int dem = 0;
                Thread.Sleep(10000);
                while (otp == null && dem < 5)
                {
                    otp = otps.GetOTPSMS(keyapi, session);

                    if (otp == null)
                    {
                        Thread.Sleep(2000);
                        dem++;
                    }
                    else
                    {
                        break;
                    }
                }
                if (otp != null)
                {
                    KAutoHelper.ADBHelper.TapByPercent(deviceID, 10.0, 54.0);
                    Thread.Sleep(2000);
                    KAutoHelper.ADBHelper.InputText(deviceID, otp);
                    Thread.Sleep(2000);
                    KAutoHelper.ADBHelper.TapByPercent(deviceID, 46.4, 67.3);
                    Thread.Sleep(5000);
                    KAutoHelper.ADBHelper.TapByPercent(deviceID, 40.4, 8.1);
                    Thread.Sleep(2000);
                    KAutoHelper.ADBHelper.InputText(deviceID, "https://member-m.lazada.vn/user/setting?");
                    KAutoHelper.ADBHelper.Key(deviceID, KAutoHelper.ADBKeyEvent.KEYCODE_ENTER);
                    Thread.Sleep(4000);
                    KAutoHelper.ADBHelper.TapByPercent(deviceID, 36.8, 35.0);
                    Thread.Sleep(2000);
                    KAutoHelper.ADBHelper.TapByPercent(deviceID, 75.9, 64.0);
                    Thread.Sleep(2000);
                    KAutoHelper.ADBHelper.TapByPercent(deviceID, 82.2, 48.4);//Queen mat khau
                    Thread.Sleep(5000);
                    KAutoHelper.ADBHelper.TapByPercent(deviceID, 10.3, 31.4);
                    KAutoHelper.ADBHelper.InputText(deviceID, phone);
                    Thread.Sleep(2000);
                    KAutoHelper.ADBHelper.SwipeByPercent(deviceID, 11.8, 52.9, 95.2, 53.1, 500);
                    Thread.Sleep(7000);
                    KAutoHelper.ADBHelper.TapByPercent(deviceID, 46.4, 70.7);
                    Thread.Sleep(2000);
                    String newsession = otps.CallbackSim(keyapi, "2", phone);
                    KAutoHelper.ADBHelper.TapByPercent(deviceID, 80.4, 57.5);
                    Thread.Sleep(10000);
                    String otp2 = null;
                    int dem2 = 0;
                    while (dem2 < 5 && otp2 == null)
                    {
                        otp2 = otps.GetOTPSMS(keyapi, newsession);
                        if (otp2 == null)
                        {
                            Thread.Sleep(2000);
                            dem2++;
                        }
                        else break;
                    }
                    if (otp2 != null)
                    {
                        KAutoHelper.ADBHelper.TapByPercent(deviceID, 22.3, 59.4);
                        KAutoHelper.ADBHelper.InputText(deviceID, otp2);
                        Thread.Sleep(200);
                        KAutoHelper.ADBHelper.TapByPercent(deviceID, 47.9, 71.4);
                        Thread.Sleep(5000);
                        KAutoHelper.ADBHelper.TapByPercent(deviceID, 11.5, 46.5);
                        KAutoHelper.ADBHelper.InputText(deviceID, password.Text);
                        Thread.Sleep(200);
                        KAutoHelper.ADBHelper.TapByPercent(deviceID, 10.9, 57.3);
                        KAutoHelper.ADBHelper.InputText(deviceID, password.Text);
                        Thread.Sleep(200);
                        KAutoHelper.ADBHelper.TapByPercent(deviceID, 49.7, 69.5);
                        write.WriteToFileLine(phone + "|" + password.Text);
                        Thread.Sleep(3000);
                    }
                }

            }
            catch (Exception err)
            {
                RemoveIp(deviceID);
                Thread.Sleep(2000);
                AdbCommand("adb -s " + deviceID + " shell pm clear com.android.browser");
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
    public class code2fa
    {
        public string token { get; set; }
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
    public class OtpSim
    {
        public string session;
        public string GetPhone(string apikey, string idService, string network)
        {
            string phone = null;
            HttpRequest http = new HttpRequest();
            string html = http.Get("http://otpsim.com/api/phones/request?token=" + apikey + "&service=" + idService + "&network=" + network).ToString();
            try
            {
                var res = JsonConvert.DeserializeObject<PhoneOTPSIM>(html);
                if (res.status_code == 200)
                {
                    phone = res.data.phone_number;
                    session = res.data.session;
                }
            }
            catch
            {
                phone = null;

            }
            return phone;
        }
        public string GetOTPSMS(string apikey, string session)
        {
            string otp = null;
            HttpRequest http = new HttpRequest();
            string html = http.Get("http://otpsim.com/api/sessions/" + session + "?token=" + apikey).ToString();

            try
            {
                var res0 = JsonConvert.DeserializeObject<SMS>(html);

                if (res0.data.messages != null)
                {
                    otp = res0.data.messages[0].otp;
                }


            }
            catch
            {
                otp = null;

            }
            return otp;
        }
        public string GETOTPVOICE(string apikey, string session)
        {
            string otp = null;
            HttpRequest http = new HttpRequest();
            string html = http.Get("http://otpsim.com/api/sessions/" + session + "?token=" + apikey).ToString();

            try
            {
                var res0 = JsonConvert.DeserializeObject<VoiceSMS>(html);

                if (res0.data.messages != null)
                {
                    otp = res0.data.messages[0].otp;
                }


            }
            catch
            {
                otp = null;

            }
            return otp;
        }
        public string CallbackSim(string apikey, string service, string number)
        {
            string newsession = null;
            HttpRequest http = new HttpRequest();
            string html = http.Get("http://otpsim.com/api/phones/request?token=" + apikey + "&service=" + service + "&number=" + number).ToString();
            try
            {
                var res0 = JsonConvert.DeserializeObject<CallBackSim>(html);
                if (res0.status_code == 200 && res0.success == true)
                {
                    newsession = res0.data.session;
                }
            }
            catch
            {
                newsession = null;

            }
            return newsession;
        }

    }
    public class TinsoftProxy
    {
        public bool success { get; set; }
        public string proxy { get; set; }
        public int location { get; set; }
        public string next_change { get; set; }
        public int timeout { get; set; }
    }

    public class TinSoftFail
    {
        public bool success { get; set; }
        public string description { get; set; }
        public int next_change { get; set; }
    }
}
