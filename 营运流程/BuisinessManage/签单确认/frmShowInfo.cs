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
    public partial class frmShowInfo : BaseForm
    {
        private int _RowCount;
        private float _CollectionPay;
        private float _AccTotal;

        /// <summary>
        /// 代收货款
        /// </summary>
        public float CollectionPay
        {
            get { return _CollectionPay; }
            set { _CollectionPay = value; }
        }

        /// <summary>
        /// 应收总额
        /// </summary>
        public float AccTotal
        {
            get { return _AccTotal; }
            set { _AccTotal = value; }
        }

        /// <summary>
        /// 总票数
        /// </summary>
        public int RowCount
        {
            get { return _RowCount; }
            set { _RowCount = value; }
        }

        public frmShowInfo()
        {
            InitializeComponent();
        }

        public frmShowInfo(float CollectionPay, float AccTotal, int RowCount)
        {
            InitializeComponent();
            this._CollectionPay = CollectionPay;
            this._AccTotal = AccTotal;
            this._RowCount = RowCount;
        }

        private void w_arrived_cofirm_Load(object sender, EventArgs e)
        {

            label2.Text = "代收货款：" + _CollectionPay + "    应收总额：" + AccTotal;
            simpleButton1.Focus();
        }
    }
}
