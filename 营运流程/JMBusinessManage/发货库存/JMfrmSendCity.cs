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
    public partial class JMfrmSendCity : BaseForm
    {
        public JMfrmSendCity()
        {
            InitializeComponent();
        }

        private void JMfrmSendCity_Load(object sender, EventArgs e)
        {
            CommonClass.SetSite(SiteName,false);
        }

        private void SiteName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(WebName,SiteName.Text, false);
        }
    }
}
