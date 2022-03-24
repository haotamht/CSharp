using EAGetMail;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace getMail
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                String[] Parse = accmail.Text.Split('|');
                String otp = null;
                int dem = 0;
                Thread.Sleep(1000);
                while (otp == null && dem < 6)
                {
                    otp = getOTPHotmail(Parse[0], Parse[1]);

                    if (otp == null)
                    {
                        Thread.Sleep(2000);
                        dem++;
                    }
                    else
                    {
                        DeleteAllMail(Parse[0], Parse[1]);
                        break;
                    }
                }
                Invoke(new Action(() =>
                {

                    richTextBox1.Text += otp + '\n';
                }));
            }
            catch
            {
                MessageBox.Show("Lỗi");
            }
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}


