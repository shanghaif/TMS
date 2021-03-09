using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class frmUpdateDriverTakePay : BaseForm
    {

        public string _batchNo, _diverTakePay;
        public string batchNo
        {
            get { return _batchNo; }
            set { _batchNo = value; }
        }
        public string diverTakePay
        {
            get { return _diverTakePay; }
            set { _diverTakePay = value; }
        }
        public frmUpdateDriverTakePay()
        {
            InitializeComponent();
        }

        private void frmUpdateDriverTakePay_Load(object sender, EventArgs e)
        {
            txtBatchNo.Text = batchNo;
            txtDriverTakePay.Text = diverTakePay;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("batch", batchNo));
                list.Add(new SqlPara("driveTakePay", txtDriverTakePay.Text.Trim()));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_UPDATE_DriverTakePay", list)) == 0) return;
                MsgBox.ShowOK();
            } 
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}