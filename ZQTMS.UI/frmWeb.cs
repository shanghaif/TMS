using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraEditors.Controls;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using ZQTMS.Common;
using DevExpress.XtraCharts;

namespace ZQTMS.UI
{
    public partial class frmWeb : BaseForm
    {
        public frmWeb()
        {
            InitializeComponent();
        }

        private void frmWeb_Load(object sender, EventArgs e)
        {
            GetMainPic(); 
        }

        /// <summary>
        /// 加载主界面的图片
        /// </summary>
        private void GetMainPic()
        {
            try
            {
                string path = Path.Combine(Application.StartupPath, "mainweb.jpg");
                if (!File.Exists(path)) return;

                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);

                int byteLength = (int)fs.Length;
                byte[] fileBytes = new byte[byteLength];
                fs.Read(fileBytes, 0, byteLength);
                fs.Close();
                fs.Dispose();

                pictureEdit1.Image = Image.FromStream(new MemoryStream(fileBytes));
            }
            catch (Exception)
            {
            }
        }
    }
}