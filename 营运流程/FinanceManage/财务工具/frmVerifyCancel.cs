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
    public partial class frmVerifyCancel : BaseForm
    {
        public frmVerifyCancel()
        {
            InitializeComponent();
        }

        private void VerifyProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            string project = VerifyProject.EditValue.ToString();
            lblTypeText.Text = GetTypeText();
        }

        private string GetTypeText()
        {
            string project = VerifyProject.EditValue.ToString();
            switch (project)
            {
                case "0":
                    return "请输入运单号";
                case "1":
                    return "请输入大车批次";
                case "2":
                    return "请输入派车单号";
                case "3":
                    return "请输入短驳批次";
                case "4":
                    return "请输入凭证号";
                default:
                    return "";
            }
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            GetVerify();
        }

        private void GetVerify()
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;

                if (VerifyProject.EditValue == null)
                {
                    MsgBox.ShowError("请选择反核销项目");
                    return;
                }
                if (BillNo.Text.Trim() == "")
                {
                    MsgBox.ShowError(GetTypeText());
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("VerifyType", VerifyProject.EditValue));
                list.Add(new SqlPara("billno", BillNo.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_VerifyOffAccount_CancelVerify", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                if (myGridView1.RowCount < 1000) myGridView1.BestFitColumns();
            }
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmVerifyCancel_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("应收应付反核销");//xj/2019/5/28
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar1); //如果有具体的工具条，就引用其实例
            this.checkBox_selectAll.Visible= false;
        }
        /// <summary>
        /// 反核销
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barCancelVerify_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;

                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "VerifyOffAccountID").ToString());
                Guid BillAccountID = new Guid(myGridView1.GetRowCellValue(rowhandle, "BillAccountID").ToString());
                string verifyStatus = myGridView1.GetRowCellValue(rowhandle, "VerifyStatus").ToString();
                if (verifyStatus == "取消")
                {
                    MsgBox.ShowError("本凭证以前经过反核销,现在是未核销状态。不能再反核销。");
                    return;
                }
                if (MsgBox.ShowYesNo("确定要反核销该凭证吗") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("VerifyOffAccountID", id));
                list.Add(new SqlPara("BillAccountID", BillAccountID));//hj 20180123
                Random r1 = new Random();
                int a1 = r1.Next(10, 100);
                string voucherNo ="I" + DateTime.Now.ToString("yyyyMMddHHmmss")+a1;
                list.Add(new SqlPara("voucherNos", voucherNo));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_Update_VerifyOffAccount_Cancel_2", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    myGridView1.SetRowCellValue(rowhandle, "VerifyStatus", "取消");
                    DataTable dt = myGridControl1.DataSource as DataTable;
                    dt.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        //导出
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void checkBox_selectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_selectAll.Checked == true)
            {
                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    myGridView1.SetRowCellValue(i, "ischecked", true);
                }
            }
            if (checkBox_selectAll.Checked == false)
            {
                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    myGridView1.SetRowCellValue(i, "ischecked", false);
                }
            }
        }
    }
}
