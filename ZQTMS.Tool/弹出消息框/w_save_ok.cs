using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace ZQTMS.Tool
{
    public partial class w_save_ok : XtraForm
    {
        public w_save_ok()
        {
            InitializeComponent();
        }
        Timer timer = new Timer();
        DateTime t;
        private void w_save_ok_Load(object sender, EventArgs e)
        {
            t = DateTime.Now;
            timer.Interval = 100;
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now.Subtract(t).TotalMilliseconds >= 300)
            {
                this.Close();
                timer.Stop();
            }
        }
    }
}