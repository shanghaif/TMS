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
    public partial class frmConfirmWithText : BaseForm
    {
        public string Reson;
        public string Type;

        public frmConfirmWithText()
        {
            InitializeComponent();
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            Reson = OperContent.Text.Trim();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Reson = OperContent.Text.Trim();
        }

        private void frmConfirmWithText_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Type.Trim()))
            {
                this.Text = "确定是否进行【" + Type + "】操作？";
                label2.Text = "【" + Type + "】原因：";
            }
            
        }
    }
}