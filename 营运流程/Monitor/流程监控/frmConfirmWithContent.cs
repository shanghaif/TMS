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
    public partial class frmConfirmWithContent : BaseForm
    {
        public string Reson;
        public string Type;
        public string txtApplyContent;

        public frmConfirmWithContent()
        {
            InitializeComponent();
        }

        public frmConfirmWithContent(string type, string reson)
        {
            this.Type = type;
            this.Reson = reson;
        }

        private void frmConfirmWithContent_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Type.Trim()))
            {
                this.Text = "确定是否进行【" + Type + "】操作？";
                label2.Text = "【" + Type + "】原因：";
            }
            txtapply.Text = txtApplyContent;
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            Reson = txtOperReason.Text.Trim();
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }
    }
}