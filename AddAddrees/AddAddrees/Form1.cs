using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AddAddrees
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Object lockAdd = new Object();
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
        string phoneRandom()
        {
            string sdts = "";
            Random rnd = new Random();
            string[] sdt = { "086", "096", "097", "098",
                          "032", "033", "034", "035",
                          "036", "037" ,"038","039","088","091","094","083","084","085","081","082","090","093",
                          "070","079","077","076","078","092", "056", "058"};
            int dem = r.Next(sdt.Length);
            sdts = sdt[dem] + randomStrNum(7);
            return sdts;
        }
        Object locker = new object();
        Random r = new Random();
        string randomAdd()
        {
            StreamReader read = new StreamReader("listaddress.txt");
            string add = "";
            String DataAdd;
            lock (locker)
            {
                DataAdd = read.ReadToEnd();
                
            }
            read.Close();
            String[] listAdds = DataAdd.Split(
                 new[] { "\r\n", "\r", "\n" },
                 StringSplitOptions.None
             );
            
            int dem = r.Next(0,listAdds.Length);
            add = listAdds[dem];
            return add;
        }
        string randomname()
        {
            StreamReader read = new StreamReader("name.txt");
            string name = "";
            String DataName;
            lock (locker)
            {
                DataName = read.ReadToEnd();

            }
            read.Close();
            String[] listname = DataName.Split(
                 new[] { "\r\n", "\r", "\n" },
                 StringSplitOptions.None
             );

            int dem1 = r.Next(0, listname.Length);
            int dem2 = r.Next(0, listname.Length);
            
            name = listname[dem1]+ randomStrNum(2)+ listname[dem2]+ r.Next(0,99);
            return name;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            List<String> Devices = KAutoHelper.ADBHelper.GetDevices();
            String[] Device = new String[Devices.Count];
            int dem = 0;
            foreach (var item in Devices)
            {
                Device[dem] = item;
                dem++;
            }
            
            for (int i = 0; i < Device.Length; i++)
            {
                int iThread = i;
                int a = i;
                new Thread(() =>
                {
                    //Thread.CurrentThread.IsBackground = true;
                    //while (true)
                    //{
                    //    AdbCommand("scrcpy -s " +Device[iThread] + " --max-size 640");
                    //    //AdbCommand("scrcpy --max-size 640");
                    //}
                    
                    AdbCommand("scrcpy -s " + Device[iThread] + " --max-size 640 "+ "--window-x "+a*400+" --window-y "+200+ " --stay-awake");

                }).Start();
                //System.Diagnostics.Process process = new System.Diagnostics.Process();
                //System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                //startInfo.FileName = "cmd.exe";
                //startInfo.Arguments = "scrcpy";
                //process.StartInfo = startInfo;
                //process.Start();
            }
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
        
        private void button2_Click(object sender, EventArgs e)
        {
            List<String> Devices = KAutoHelper.ADBHelper.GetDevices();
            String[] Device = new String[Devices.Count];
            int dem = 0;
            foreach (var item in Devices)
            {
                Device[dem] = item;
                dem++;
            }

            for (int i = 0; i < Device.Length; i++)
            {
                int iThread = i;
                int a = i;
                new Thread(() =>
                {

                    AdbCommand("adb -s "+ Device[iThread] + " reboot bootloader");
                    Thread.Sleep(4000);
                    //AdbCommand("fastboot -s " + Device[iThread] + " --w");
                    //AdbCommand("fastboot -w " + Device[iThread]);
                    AdbCommand("fastboot -s " + Device[iThread] + " erase userdata");
                    Thread.Sleep(4000);
                    AdbCommand("fastboot -s " + Device[iThread] + " erase cache");
                    Thread.Sleep(3000);
                    AdbCommand("fastboot -s " + Device[iThread] + " reboot");
                    //AdbCommand("fastboot -s " + Device[iThread] + " reboot");
                    // -s "+ Device[iThread]+"
                }).Start();
                
            }
        }

        
        private void button3_Click(object sender, EventArgs e)
        {
            List<String> Devices = KAutoHelper.ADBHelper.GetDevices();
            String[] Device = new String[Devices.Count];
            int dem = 0;
            foreach (var item in Devices)
            {
                Device[dem] = item;
                dem++;
            }

            for (int i = 0; i < Device.Length; i++)
            {
                int iThread = i;
                int a = i;
                new Thread(() =>
                {
                    AdbCommand("adb -s " + Device[iThread] + " install " + linkapk.Text );

                }).Start();

            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<String> Devices = KAutoHelper.ADBHelper.GetDevices();
            String[] Device = new String[Devices.Count];
            int dem = 0;
            foreach (var item in Devices)
            {
                Device[dem] = item;
                dem++;
            }

            for (int i = 0; i < Device.Length; i++)
            {
                int iThread = i;
                int a = i;
                new Thread(() =>
                {
                    AdbCommand("adb -s " + Device[iThread] + " shell pm clear com.lazada.android");

                }).Start();

            }
        }
        
        int nextList = 0;
       
        private void button5_Click(object sender, EventArgs e)
        {
            
            List<String> Devices = KAutoHelper.ADBHelper.GetDevices();
            String[] Device = new String[Devices.Count];
            int dem = 0;

            foreach (var item in Devices)
            {
                Device[dem] = item;
                dem++;
            }

            for (int i = 0; i < Device.Length; i++)
            {
                
                int iThread = i;
                int a = i;
                new Thread(() =>
                {
                    string add = randomAdd();
                    string phone = phoneRandom();
                    if (checkBox1.Checked)
                    {
                        //đây à dung r 
                        Address1(Device[iThread], add, phone);
                    }
                    if (checkBox2.Checked)
                    {
                        Address2(Device[iThread], add, phone);
                    }
                    if (checkBox3.Checked)
                    {
                        Address3(Device[iThread], add, phone);
                    }
                    if (checkBox4.Checked)
                    {
                        Address4(Device[iThread], add, phone);
                    }
                    if (checkBox5.Checked)
                    {
                        Address5(Device[iThread], add, phone);
                    }
                    if (checkBox6.Checked)
                    {
                        Address6(Device[iThread], add, phone);
                    }
                    if (checkBox7.Checked)
                    {
                        Address7(Device[iThread], add, phone);
                    }
                }).Start();

            }
        }
        void Address1(String deviceID, String Add, String Sdt)
        {
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 47.3, 15.9);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 14.8, 15.4);
            Thread.Sleep(500);
            KAutoHelper.ADBHelper.InputText(deviceID, randomname()+kiHieu.Text);
            Thread.Sleep(500);
            //KAutoHelper.ADBHelper.TapByPercent(deviceID, 13.4, 23.7);
            //Thread.Sleep(500);
            //KAutoHelper.ADBHelper.InputText(deviceID, Add);
            //Thread.Sleep(500);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 35.1, 39.2);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.SwipeByPercent(deviceID, 49.3, 91.7, 49.3, 11.7, 1000);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.SwipeByPercent(deviceID, 49.3, 91.7, 49.3, 40, 1000);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 13.8, 75.3);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.SwipeByPercent(deviceID, 49.3, 91.7, 49.3, 11.7, 500);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.SwipeByPercent(deviceID, 49.3, 91.7, 49.3, 11.7, 500);
            Thread.Sleep(500);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 9.4, 81.1);
            Thread.Sleep(500);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 17.2, 22.3);
            Thread.Sleep(500);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 13.1, 61.9);
            Thread.Sleep(500);
            KAutoHelper.ADBHelper.InputText(deviceID, SDTRandom());
            //Thread.Sleep(500);
            //KAutoHelper.ADBHelper.TapByPercent(deviceID, 47.3, 90.2);
        }
        void Address2(String deviceID, String Add, String Sdt)
        {
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 47.3, 15.9);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 14.8, 15.4);
            Thread.Sleep(500);
            KAutoHelper.ADBHelper.InputText(deviceID, randomname() + kiHieu.Text);
            Thread.Sleep(500);
            //KAutoHelper.ADBHelper.TapByPercent(deviceID, 13.4, 23.7);
            //Thread.Sleep(500);
            //KAutoHelper.ADBHelper.InputText(deviceID, Add);
            //Thread.Sleep(500);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 35.1, 39.2);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.SwipeByPercent(deviceID, 49.3, 91.7, 49.3, 11.7, 1000);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.SwipeByPercent(deviceID, 49.3, 91.7, 49.3, 40, 1000);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 13.8, 75.3);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.SwipeByPercent(deviceID, 49.3, 91.7, 49.3, 11.7, 500);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.SwipeByPercent(deviceID, 49.3, 91.7, 49.3, 11.7, 500);
            Thread.Sleep(500);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 14.1, 73.1);
            Thread.Sleep(500);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 14.8, 15.4);
            Thread.Sleep(500);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 13.1, 61.9);
            Thread.Sleep(500);
            KAutoHelper.ADBHelper.InputText(deviceID, SDTRandom());
        }
        void Address3(String deviceID, String Add, String Sdt)
        {
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 47.3, 15.9);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 14.8, 15.4);
            Thread.Sleep(500);
            KAutoHelper.ADBHelper.InputText(deviceID, randomname() + kiHieu.Text);
            Thread.Sleep(500);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 35.1, 39.2);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.SwipeByPercent(deviceID, 49.3, 91.7, 49.3, 11.7, 1000);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.SwipeByPercent(deviceID, 49.3, 91.7, 49.3, 40, 1000);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 13.8, 75.3);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.SwipeByPercent(deviceID, 49.3, 91.7, 49.3, 11.7, 500);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.SwipeByPercent(deviceID, 49.3, 91.7, 49.3, 11.7, 500);
            Thread.Sleep(500);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 9.0, 17.7);
            Thread.Sleep(500);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 13.1, 45.0);
            Thread.Sleep(500);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 13.1, 61.9);
            Thread.Sleep(500);
            KAutoHelper.ADBHelper.InputText(deviceID, SDTRandom());


        }
        void Address4(String deviceID, String Add, String Sdt)
        {
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 47.3, 15.9);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 14.8, 15.4);
            Thread.Sleep(500);
            KAutoHelper.ADBHelper.InputText(deviceID, randomname() + kiHieu.Text);
            Thread.Sleep(500);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 35.1, 39.2);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.SwipeByPercent(deviceID, 49.3, 91.7, 49.3, 11.7, 1000);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.SwipeByPercent(deviceID, 49.3, 91.7, 49.3, 40, 1000);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 13.8, 75.3);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 15.8, 91.4);
            Thread.Sleep(500);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 12.1, 22.1);
            Thread.Sleep(500);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 13.1, 61.9);
            Thread.Sleep(500);
            KAutoHelper.ADBHelper.InputText(deviceID, SDTRandom());


        }
        void Address5(String deviceID, String Add, String Sdt)
        {
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 47.3, 15.9);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 14.8, 15.4);
            Thread.Sleep(500);
            KAutoHelper.ADBHelper.InputText(deviceID, randomname() + kiHieu.Text);
            Thread.Sleep(500);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 35.1, 39.2);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.SwipeByPercent(deviceID, 49.3, 91.7, 49.3, 11.7, 1000);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.SwipeByPercent(deviceID, 49.3, 91.7, 49.3, 40, 1000);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 13.8, 75.3);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.SwipeByPercent(deviceID, 49.3, 91.7, 49.3, 11.7, 500);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.SwipeByPercent(deviceID, 49.3, 91.7, 49.3, 11.7, 500);
            Thread.Sleep(500);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 12.7, 65.5);//Nam tu liem
            Thread.Sleep(500);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 16.5, 37.9);//My dinh 1
            Thread.Sleep(500);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 13.1, 61.9);
            Thread.Sleep(500);
            KAutoHelper.ADBHelper.InputText(deviceID, SDTRandom());


        }
        void Address6(String deviceID, String Add, String Sdt)
        {
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 47.3, 15.9);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 14.8, 15.4);
            Thread.Sleep(500);
            KAutoHelper.ADBHelper.InputText(deviceID, randomname() + kiHieu.Text);
            Thread.Sleep(500);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 35.1, 39.2);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.SwipeByPercent(deviceID, 49.3, 91.7, 49.3, 11.7, 1000);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.SwipeByPercent(deviceID, 49.3, 91.7, 49.3, 40, 1000);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 13.8, 75.3);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.SwipeByPercent(deviceID, 49.3, 91.7, 49.3, 11.7, 500);
            //Thread.Sleep(1000);
            //KAutoHelper.ADBHelper.SwipeByPercent(deviceID, 49.3, 91.7, 49.3, 11.7, 500);
            Thread.Sleep(500);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 9.0, 33.8);
            Thread.Sleep(500);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 17.8, 38.6);
            //Thread.Sleep(500);
            //KAutoHelper.ADBHelper.TapByPercent(deviceID, 16.5, 37.9);//My dinh 1
            Thread.Sleep(500);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 13.1, 61.9);
            Thread.Sleep(500);
            KAutoHelper.ADBHelper.InputText(deviceID, SDTRandom());


        }
        void Address7(String deviceID, String Add, String Sdt)
        {
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 10.7, 34.1);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 13.1, 14.3);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 12.1, 37.2);
            Thread.Sleep(1000);
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 10.4, 14.0);
        }
        private void ten_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            List<String> Devices = KAutoHelper.ADBHelper.GetDevices();
            String[] Device = new String[Devices.Count];
            int dem = 0;

            foreach (var item in Devices)
            {
                Device[dem] = item;
                dem++;
            }

            for (int i = 0; i < Device.Length; i++)
            {

                int iThread = i;
                int a = i;
                new Thread(() =>
                {
                    deSau(Device[iThread]);

                }).Start();

            }
        }
        void deSau(String deviceID)
        {
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 78.8, 60.1);// de sau
        }

        private void button7_Click(object sender, EventArgs e)
        {

            List<String> Devices = KAutoHelper.ADBHelper.GetDevices();
            String[] Device = new String[Devices.Count];
            int dem = 0;
            string[] ListAcc = rbListAcc.Text.Split(
               new[] { "\n", "\r", "\r\n" }, StringSplitOptions.None);
            foreach (var item in Devices)
            {
                Device[dem] = item;
                dem++;
            }
            int next = 0;
            for (int i = 0; i < Device.Length; i++)
            {

                int iThread = i;
                int a = i;
                Thread t = new Thread(() =>
                {
                    string add = randomAdd();
                    string phone = phoneRandom();
                    string acc = null;
                    lock (locker)
                    {
                        if (next >= Device.Length) return;
                        acc = ListAcc[next];
                        next++;

                    }
                    String[] parseAcc = acc.Split('|');
                    KAutoHelper.ADBHelper.TapByPercent(Devices[iThread], 32.1, 74.8);
                    Thread.Sleep(200);
                    KAutoHelper.ADBHelper.TapByPercent(Devices[iThread], 82.5, 64.1);
                    Thread.Sleep(6000);
                    KAutoHelper.ADBHelper.TapByPercent(Devices[iThread], 22.9, 41.8);
                    Thread.Sleep(200);
                    KAutoHelper.ADBHelper.InputText(Devices[iThread], parseAcc[0]);
                    Thread.Sleep(200);
                    KAutoHelper.ADBHelper.TapByPercent(Devices[iThread], 16.5, 47.5);
                    Thread.Sleep(200);
                    KAutoHelper.ADBHelper.InputText(Devices[iThread], parseAcc[1]);
                    Thread.Sleep(400);
                    KAutoHelper.ADBHelper.TapByPercent(Devices[iThread], 50.0, 55.0);
                });
                    t.Start();
                    t.IsBackground = true;
            }

        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
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

