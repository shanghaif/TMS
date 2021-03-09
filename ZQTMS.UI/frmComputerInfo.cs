using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;

namespace ZQTMS.UI
{
    public partial class frmComputerInfo : BaseForm
    {
        public frmComputerInfo()
        {
            InitializeComponent();
        }

        public string md5 = "";
        public string useraccount = "";

        private void frmComputerInfo_Load(object sender, EventArgs e)
        {
            memoEdit1.Text = string.Format("我的账号：{0}\r\n验证信息：\r\n{1}", useraccount, md5);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(memoEdit1.Text.Trim());
            }
            catch (Exception)
            {
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
