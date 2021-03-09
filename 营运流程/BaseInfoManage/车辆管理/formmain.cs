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
    public partial class formmain : BaseForm
    {
        public formmain()
        {
            InitializeComponent();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //w_weixiu_dengji weixiudengji = new w_weixiu_dengji();
            //weixiudengji.MdiParent = this;
            //weixiudengji.Show();
            CommonClass.FormSet(this);
            BarMagagerOper.SetBarPropertity(bar2);
            MessageBox.Show(dateEdit1.EditValue.ToString());
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            w_jiayou_dengji jiayou = new w_jiayou_dengji();
            jiayou.MdiParent = this;
            jiayou.Show();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            w_baoyang_dengji baoyang = new w_baoyang_dengji();
            baoyang.MdiParent = this;
            baoyang.Show();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            w_tyre_manage tyre = new w_tyre_manage();
            tyre.MdiParent = this;
            tyre.Show();
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            w_cheliang_dengji che = new w_cheliang_dengji();
            che.MdiParent = this;
            che.Show();
        }

        private void formmain_Load(object sender, EventArgs e)
        {
            this.dateEdit1.EditValue = DateTime.Now;
        }
    }
}