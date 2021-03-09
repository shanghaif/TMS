using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;


namespace ZQTMS.UI
{
    public partial class frmFetchPayMiConfirmList : BaseForm
    {
        public frmFetchPayMiConfirmList()
        {
            InitializeComponent();
        }
        GridColumn gcIsseleckedMode;
        private void frmShortConnOutbound_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            FixColumn fix = new FixColumn(myGridView1, barSubItem4);

            DateTime bdt = CommonClass.gbdate;
            bdt = bdt.AddHours(-16);
            bdate.DateTime = bdt;
            DateTime edt = CommonClass.gedate;
            edt = edt.AddHours(-16);
            edate.DateTime = edt;
            CommonClass.SetCause(CauseName, true);
            CauseName.EditValue = CommonClass.UserInfo.CauseName;
            AreaName.EditValue = CommonClass.UserInfo.AreaName;
            WebName.EditValue = CommonClass.UserInfo.WebName;
            gcIsseleckedMode = GridOper.GetGridViewColumn(myGridView1, "ischecked");
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            getData();
        }
        public void getData()
        {
            if (bdate.DateTime.Date > edate.DateTime.Date)
            {
                XtraMessageBox.Show("开始日期不能大于结束日期", "日期选择错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.EditValue));
                list.Add(new SqlPara("edate", edate.EditValue));
                list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text.Trim()));
                list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text.Trim()));
                list.Add(new SqlPara("WebName", WebName.Text.Trim() == "全部" ? "%%" : WebName.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_FETCHPAYFORADUIT_MID_CONFIRM", list);
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
        private void simpleButton7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barCheckItem3_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barCheckItem4_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            //string sBillNo = "", sAmount = "";
            //float Amount = 0, FetchPayVerifBalance = 0;
            //float sumAmount = 0;
            int rows = myGridView1.FocusedRowHandle;
            if (rows < 0) return;
            if (myGridView1.GetRowCellValue(rows, "BillNo") == null)
            {
                return;
            }
            if (ConvertType.ToString(myGridView1.GetRowCellValue(rows, "DisTranVerifState")) != "")
            {
                MsgBox.ShowError("已审核的不能取消！");
                return;
            }
            if (MsgBox.ShowYesNo("确定取消确认?") != DialogResult.Yes) return;
            try
            {
                

                //for (int i = 0; i < myGridView1.RowCount; i++)
                //{
                //    if (ConvertType.ToString(myGridView1.GetRowCellValue(i, "ischecked")) == "1")
                //    {
                //        if (ConvertType.ToString(myGridView1.GetRowCellValue(i, "DisTranVerifState")) != "")
                //        {
                //            MsgBox.ShowError("请勿选中回扣已核销的单号！");
                //            return;
                //        }
                //        Amount = ConvertType.ToFloat(myGridView1.GetRowCellValue(i, "Amount"));
                //        FetchPayVerifBalance = ConvertType.ToFloat(myGridView1.GetRowCellValue(i, "FetchPayVerifBalance"));
                //        if (Amount <= 0 || Amount > FetchPayVerifBalance) continue;

                //        sBillNo += myGridView1.GetRowCellValue(i, "BillNo") + "@";//运单号
                //        sAmount += Amount + "@";//审核的金额
                //        sumAmount = sumAmount + Amount;
                //    }
                //}
                //if (sBillNo == "") return;
                //string sShowOK = "中转提付取消确认总票数：" + ConvertType.ToString(myGridView1.RowCount)
                //    + "\r\n中转提付取消确认总金额：" + ConvertType.ToString(sumAmount) + "\r\n中转提付取消确认人：" + CommonClass.UserInfo.UserName + "\r\n是否继续？";

                //if (MsgBox.ShowYesNo(sShowOK) != DialogResult.Yes)
                //{
                //    return;
                //}

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNoStr", Convert.ToString(myGridView1.GetRowCellValue(rows, "BillNo"))));
     

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_CHECK_FETCHPAYADUIT_MID_CONFIRM_CANCEL", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    getData();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btnAduit_ItemClick(object sender, ItemClickEventArgs e)
        {
            //if (myGridView1.RowCount == 0) return;

            //myGridView1.PostEditor();
            //string sBillNo = "", sAmount = "";
            //float Amount = 0, FetchPayVerifBalance = 0;
            //float sumAmount = 0;
            //try
            //{
                

            //    for (int i = 0; i < myGridView1.RowCount; i++)
            //    {
            //        if (ConvertType.ToString(myGridView1.GetRowCellValue(i, "ischecked")) == "1")
            //        {
            //            Amount = ConvertType.ToFloat(myGridView1.GetRowCellValue(i, "Amount"));
            //            FetchPayVerifBalance = ConvertType.ToFloat(myGridView1.GetRowCellValue(i, "FetchPayVerifBalance"));
            //            if (Amount <= 0 || Amount > FetchPayVerifBalance) continue;

            //            sBillNo += myGridView1.GetRowCellValue(i, "BillNo") + "@";//运单号
            //            sAmount += Amount + "@";//审核的金额
            //            sumAmount = sumAmount + Amount;
            //        }
            //    }
            //    if (sBillNo == "") return;
            //    string sShowOK = "中转提付确认总票数：" + ConvertType.ToString(myGridView1.RowCount)
            //        + "\r\n中转提付确认总金额：" + ConvertType.ToString(sumAmount) + "\r\n中转提付确认人：" + CommonClass.UserInfo.UserName + "\r\n是否继续？";

            //    if (MsgBox.ShowYesNo(sShowOK) != DialogResult.Yes)
            //    {
            //        return;
            //    }
            //   // if (MsgBox.ShowYesNo("是否审核？\r\r请确认审核金额是否正确！") != DialogResult.Yes) return;

            //    List<SqlPara> list = new List<SqlPara>();
            //    list.Add(new SqlPara("BillNoStr", sBillNo));
            //    list.Add(new SqlPara("ConfirmMan", CommonClass.UserInfo.UserName));
            //    list.Add(new SqlPara("ConfirmWeb", CommonClass.UserInfo.WebName));
            //    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_CHECK_FETCHPAYADUIT_MID_CONFIRM", list);
            //    if (SqlHelper.ExecteNonQuery(sps) > 0)
            //    {
            //        MsgBox.ShowOK();
            //        getData();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MsgBox.ShowException(ex);
            //}
            frmMidConfirmDetail frm = new frmMidConfirmDetail();
            frm.ShowDialog();
        }

        private void CauseName_EditValueChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(AreaName, CauseName.EditValue.ToString());
            AreaName.EditValue = CommonClass.UserInfo.AreaName;
        }

        private void AreaName_EditValueChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(WebName, CauseName.Text.Trim(), AreaName.Text.Trim());
        }

        private void myGridView1_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e == null || myGridView1.FocusedRowHandle < 0) return;
            try
            {
                float FetchPayVerifBalance = ConvertType.ToFloat(myGridView1.GetFocusedRowCellValue("FetchPayVerifBalance"));
                float Amount = ConvertType.ToFloat(e.Value);
                if (Amount <= 0)
                {
                    e.Valid = false;
                    e.ErrorText = "审核金额必须大于0!";
                    return;
                }
                if (Amount > FetchPayVerifBalance)
                {
                    e.Valid = false;
                    e.ErrorText = "审核金额不能大于余额!";
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void myGridView1_InvalidValueException(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            MsgBox.ShowError(e.ErrorText);
        }
        private void chkALL_CheckedChanged(object sender, EventArgs e)
        {
            int a = chkALL.Checked == true ? 1 : 0;
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                myGridView1.SetRowCellValue(i, gcIsseleckedMode, a);
            }
        }
    }
}