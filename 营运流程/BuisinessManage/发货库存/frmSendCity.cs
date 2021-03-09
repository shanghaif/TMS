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
    public partial class frmSendCity : BaseForm
    {
        public frmSendCity()
        {
            InitializeComponent();
        }

        private void frmSendCity_Load(object sender, EventArgs e)
        {
            CommonClass.SetSite(SiteName,false);

            if (CommonClass.UserInfo.WebName != "深圳广源省际操作部")
            {
                SiteName.Text = CommonClass.UserInfo.SiteName;
                WebName.Text = CommonClass.UserInfo.WebName;
                SiteName.Enabled = false;
                WebName.Enabled = false;
            }


        }

        private void SiteName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(WebName,SiteName.Text, false);
        }
    }
}
