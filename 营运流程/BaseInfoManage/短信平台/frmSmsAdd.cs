using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;

namespace ZQTMS.UI
{
    public partial class frmSmsAdd : BaseForm
    {
        public frmSmsAdd()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            XtraMessageBox.Show("您的资金已注入，您现在的余额为：", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void w_sms_add_Load(object sender, EventArgs e)
        {

        }
    }
}