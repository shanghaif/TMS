using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.UI;
using ZQTMS.Tool;

namespace ZQTMS.UI
{
    public partial class frmBargainningDialog : BaseForm
    {
        public frmBargainningDialog()
        {
            InitializeComponent();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            frmBargainingAdd frm = (frmBargainingAdd)this.Owner;
            frm.isOK = true;
            this.Close();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            frmBargainingAdd frm = (frmBargainingAdd)this.Owner;
            frm.isOK = false;
            this.Close();
        }
    }
}
