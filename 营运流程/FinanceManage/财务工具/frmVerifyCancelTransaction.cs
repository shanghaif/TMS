using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.SqlDAL;
using ZQTMS.Tool;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class frmVerifyCancelTransaction : BaseForm
    {
        public frmVerifyCancelTransaction()
        {
            InitializeComponent();
        }

        private void frmVerifyCancelTransaction_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("费用异动反核销");//xj2019/5/28
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            if (VoucherNo.EditValue == null || VoucherNo.Text == "")
            {
                MsgBox.ShowError("请输入凭证号！");
                return;
            }
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("VoucherNo", VoucherNo.Text.Trim()));

            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_VerifyOffAccount_CancelVerify_Transaction", list);
            DataSet ds = SqlHelper.GetDataSet(sps);

            if (ds == null || ds.Tables.Count == 0) return;
            myGridControl1.DataSource = ds.Tables[0];
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "VerifyOffAccountID").ToString());
                Guid id1 = new Guid(myGridView1.GetRowCellValue(rowhandle, "BillAccountID").ToString());
                Guid id2 = new Guid(myGridView1.GetRowCellValue(rowhandle, "ID").ToString());
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("VerifyOffAccountID", id));
                list.Add(new SqlPara("BillAccountID", id1));
                list.Add(new SqlPara("ID", id2));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_VerifyOffAccount_CancelVerify_Transaction", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    return;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
    }
}
