using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmBillDeliveryAdd : BaseForm
    {
        public frmBillDeliveryAdd()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 接货
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            ShowDetail("ReceiveGoods");
        }

        /// <summary>
        /// 送货
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            ShowDetail("SendGoods");
        }

        /// <summary>
        /// 短驳
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            ShowDetail("ShortBarge");
        }

        /// <summary>
        /// 长途
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            ShowDetail("LongDistance");
        }

        /// <summary>
        /// 弹出详细窗体并传入派车类型
        /// </summary>
        void ShowDetail(string SelectValue)
        {
            BillDeliveryDetail fm = new BillDeliveryDetail();
            fm.OperateType = "New";
            fm.DeliveryID = Guid.NewGuid();
            fm.DeliVehTypeValue = SelectValue;
            fm.Show();

            this.Close();
        }
    }
}