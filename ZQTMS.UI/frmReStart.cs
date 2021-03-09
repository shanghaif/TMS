using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class frmReStart : BaseForm
    {
        public frmReStart()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            frmMainNew f = this.MdiParent as frmMainNew;
            this.Close();
            if (f == null) return;
            if (MsgBox.ShowYesNo("确定重启软件？") != DialogResult.Yes) return;
            f.Dispose();
            System.Diagnostics.Process.Start(Application.StartupPath + "\\ZQTMS.exe");
        }

        private void frmReStart_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("重启软件");//xj/2019/5/29
        }
    }
}