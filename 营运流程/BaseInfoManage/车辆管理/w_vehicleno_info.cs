using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class w_vehicleno_info : BaseForm
    {
        public w_vehicleno_info()
        {
            InitializeComponent();
        }

        private void tyreno_EditValueChanged(object sender, EventArgs e)
        {

        }

      
        private void exit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void w_vehicleno_info_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            BarMagagerOper.SetBarPropertity(bar2); 
        }

        private void saveadd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }
    }
}