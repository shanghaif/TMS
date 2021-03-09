using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using System.Reflection;
using System.IO;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class frmReReport : BaseForm
    {
        public frmReReport()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            Form f = this.MdiParent;
        
            this.Close();
            if (f == null) return;
            if (MsgBox.ShowYesNo("确定重置报表？") != DialogResult.Yes) return;
            string url = Application.StartupPath + "\\Reports";
            try
            {
                DirectoryInfo Folder = new DirectoryInfo(url);
                foreach (FileInfo file in Folder.GetFiles())
                {
                    if (file.Name.Substring(0, file.Name.LastIndexOf('.')).EndsWith("per"))
                    {
                        file.Delete();
                    }
                }
                MsgBox.ShowOK("重置成功！");
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
           
     
           
        }

        private void frmReReport_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("重置报表");//xj/2019/5/29
        }
    }
}