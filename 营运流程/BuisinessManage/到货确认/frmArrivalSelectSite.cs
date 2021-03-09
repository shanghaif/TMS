using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Common;
using ZQTMS.Tool;

namespace ZQTMS.UI
{
    public partial class frmArrivalSelectSite : BaseForm
    {
        string siteName;
        string webName;
        string addReason;

        public string SiteName
        {
            get { return siteName; }
            set { siteName = value; }
        }

        public string WebName
        {
            get { return webName; }
            set { webName = value; }
        }

        public string AddReason
        {
            get { return addReason; }
            set { addReason = value; }
        }

        public frmArrivalSelectSite()
        {
            InitializeComponent();
        }

        private void frmArrivalSelectSite_Load(object sender, EventArgs e)
        {
            CommonClass.SetSite(comboBoxEdit1, false);
        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(comboBoxEdit2, comboBoxEdit1.Text, false);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            siteName = comboBoxEdit1.Text.Trim();
            if (siteName == "")
            {
                MsgBox.ShowOK("请选择站点!");
                comboBoxEdit1.Focus();
                return;
            }
            webName = comboBoxEdit2.Text.Trim();
            if (webName == "")
            {
                MsgBox.ShowOK("请选择网点!");
                comboBoxEdit2.Focus();
                return;
            }
            addReason = Reason.Text.Trim();
            if (addReason == "")
            {
                MsgBox.ShowOK("请填写原因!");
                Reason.Focus();
                return;
            }
            this.DialogResult = DialogResult.OK;
        }
    }
}
