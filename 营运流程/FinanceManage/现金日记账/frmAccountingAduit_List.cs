using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using DevExpress.XtraEditors;

namespace ZQTMS.UI
{
    public partial class frmAccountingAduit_List : BaseForm
    {
        public frmAccountingAduit_List()
        {
            InitializeComponent();
        }
        public string VoucherNo = "", VerifyOffType="";

        private void frmAccountingAduit_List_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("VoucherNo", VoucherNo));
                list.Add(new SqlPara("VerifyOffType", VerifyOffType));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLACCOUNT_Audit_List", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }
    }
}