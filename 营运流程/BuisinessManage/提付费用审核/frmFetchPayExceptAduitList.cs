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


namespace ZQTMS.UI
{
    public partial class frmFetchPayExceptAduitList : BaseForm
    {
        public frmFetchPayExceptAduitList()
        {
            InitializeComponent();
        }

        private void frmShortConnOutbound_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem4);
            DateTime bdt = CommonClass.gbdate;
            bdt = bdt.AddDays(-2);
            bdt = bdt.AddHours(18);
            bdate.DateTime = bdt;
            DateTime edt = CommonClass.gedate;
            edt = edt.AddDays(-1);
            edt = edt.AddHours(17 - edt.Hour);
            edate.DateTime = edt;
            CommonClass.SetCause(CauseName, true);
            CauseName.EditValue = CommonClass.UserInfo.CauseName;
            AreaName.EditValue = CommonClass.UserInfo.AreaName;
            WebName.EditValue = CommonClass.UserInfo.WebName;
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

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_EXCEPTFORADUIT_FETCHPAY", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            getData();
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
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            try
            {
                string sShowOK = "转异常金额：" + ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "EXFee")) + "\r\n是否转入部门：" + ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "BegWeb")) + "\r\n是否继续？";

                if (MsgBox.ShowYesNo(sShowOK) != DialogResult.Yes)
                {
                    return;
                }

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("billNo", myGridView1.GetRowCellValue(rowhandle, "billno_c")));
                list.Add(new SqlPara("EXFee", myGridView1.GetRowCellValue(rowhandle, "EXFee")));
                list.Add(new SqlPara("ExtFeeType", myGridView1.GetRowCellValue(rowhandle, "ExtFeeType")));
                list.Add(new SqlPara("ExtInDate", CommonClass.gcdate));
                list.Add(new SqlPara("ExtInMen", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("ExtSite", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("ExtWeb", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("ExtCause", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("ExtArea", CommonClass.UserInfo.AreaName));
                if ((myGridView1.GetRowCellValue(rowhandle, "ExtFeeType").ToString() == "提付"))
                    list.Add(new SqlPara("ExtType", "提付转异常"));
                if ((myGridView1.GetRowCellValue(rowhandle, "ExtFeeType").ToString() == "提付异动"))
                    list.Add(new SqlPara("ExtType", "提付异动转异常"));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_INTO_EXCEPTEXCEPTION_FETCHPAY", list);
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

        private void btnAddOtherFee_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView1.PostEditor();
            myGridView1.UpdateCurrentRow();
            myGridView1.UpdateSummary();
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if (myGridView1.GetRowCellValue(i, "ExtType").ToString() == "提付异动转正常")
                {
                    if (ConvertType.ToFloat(myGridView1.GetRowCellValue(i, "Amount")) != ConvertType.ToFloat(myGridView1.GetRowCellValue(i, "EXFee")))
                    {
                        MsgBox.ShowError("异动金额不能不分审核！");
                        return;
                    }
                }
            }
            FetchPayAduit();
            FetchTransactionAduit();
            MidFetchPayAduit();
            getData();
        }
        private void FetchPayAduit()
        {
            try
            {
                string sBillNo = "";
                string sAmount = "";
                string sFetchPayVerifBalance = "";
                float sumAmount = 0;
                if (myGridView1.RowCount > 0)
                {
                    for (int i = 0; i < myGridView1.RowCount; i++)
                    {
                        if (myGridView1.GetRowCellValue(i, "ExtType").ToString() == "提付转正常")
                        {
                            sumAmount = sumAmount + ConvertType.ToFloat(myGridView1.GetRowCellValue(i, "Amount"));
                            sBillNo = sBillNo + myGridView1.GetRowCellValue(i, "billno_c") + "@";
                            sAmount = sAmount + myGridView1.GetRowCellValue(i, "Amount") + "@";
                            sFetchPayVerifBalance = sFetchPayVerifBalance + '0' + '@';
                        }
                    }
                }


                if (sBillNo == "") return;
                string sShowOK = "提付转异常审核总票数：" + ConvertType.ToString(myGridView1.RowCount)
                    + "\r\n提付转异常审核总金额：" + ConvertType.ToString(sumAmount) + "\r\n提付转异常审核人：" + CommonClass.UserInfo.UserName + "\r\n是否继续？";

                if (MsgBox.ShowYesNo(sShowOK) != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("billNo", sBillNo));
                list.Add(new SqlPara("AduitDate", CommonClass.gcdate));
                list.Add(new SqlPara("Amount", sAmount));
                list.Add(new SqlPara("AduitBillState", "提付转正常"));
                list.Add(new SqlPara("AduitMan", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("AduitSite", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("AduitWeb", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("AduitCause", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("AduitArea", CommonClass.UserInfo.AreaName));
                list.Add(new SqlPara("FetchPayVerifBalance", sFetchPayVerifBalance));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_CHECK_EXCEPTADUIT_FETCHPAY", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK("提付转异常审核完毕！");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        private void MidFetchPayAduit()
        {
            try
            {
                string sBillNo = "";
                string sAmount = "";
                string sFetchPayVerifBalance = "";
                float sumAmount = 0;
                if (myGridView1.RowCount > 0)
                {
                    for (int i = 0; i < myGridView1.RowCount; i++)
                    {
                        if (myGridView1.GetRowCellValue(i, "ExtType").ToString() == "中转提付转正常")
                        {
                            sumAmount = sumAmount + ConvertType.ToFloat(myGridView1.GetRowCellValue(i, "Amount"));
                            sBillNo = sBillNo + myGridView1.GetRowCellValue(i, "billno_c") + "@";
                            sAmount = sAmount + myGridView1.GetRowCellValue(i, "Amount") + "@";
                            sFetchPayVerifBalance = sFetchPayVerifBalance + '0' + '@';
                        }
                    }
                }


                if (sBillNo == "") return;
                string sShowOK = "中转提付转异常审核总票数：" + ConvertType.ToString(myGridView1.RowCount)
                    + "\r\n中转提付转异常审核总金额：" + ConvertType.ToString(sumAmount) + "\r\n中转提付转异常审核人：" + CommonClass.UserInfo.UserName + "\r\n是否继续？";

                if (MsgBox.ShowYesNo(sShowOK) != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("billNo", sBillNo));
                list.Add(new SqlPara("AduitDate", CommonClass.gcdate));
                list.Add(new SqlPara("Amount", sAmount));
                list.Add(new SqlPara("AduitBillState", "中转提付转正常"));
                list.Add(new SqlPara("AduitMan", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("AduitSite", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("AduitWeb", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("AduitCause", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("AduitArea", CommonClass.UserInfo.AreaName));
                list.Add(new SqlPara("FetchPayVerifBalance", sFetchPayVerifBalance));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_CHECK_EXCEPTADUIT_FETCHPAY", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK("中转提付转异常审核完毕！");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        private void FetchTransactionAduit()
        {
            try
            {
                string sBillNo = "";
                string sAmount = "";
                string sFetchPayVerifBalance = "";
                float sumAmount = 0;
                if (myGridView1.RowCount > 0)
                {
                    for (int i = 0; i < myGridView1.RowCount; i++)
                    {
                        if (myGridView1.GetRowCellValue(i, "ExtType").ToString() == "提付异动转正常")
                        {
                            sumAmount = sumAmount + ConvertType.ToFloat(myGridView1.GetRowCellValue(i, "Amount"));
                            sBillNo = sBillNo + myGridView1.GetRowCellValue(i, "billno_c") + "@";
                            sAmount = sAmount + myGridView1.GetRowCellValue(i, "Amount") + "@";
                            sFetchPayVerifBalance = sFetchPayVerifBalance + '0' + '@';
                        }
                    }
                }

                if (sBillNo == "") return;
                string sShowOK = "提付异动转异常审核总票数：" + ConvertType.ToString(myGridView1.RowCount)
                    + "\r\n提付异动转异常审核总金额：" + ConvertType.ToString(sumAmount) + "\r\n提付异动转异常审核人：" + CommonClass.UserInfo.UserName + "\r\n是否继续？";

                if (MsgBox.ShowYesNo(sShowOK) != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("billNo", sBillNo));
                list.Add(new SqlPara("AduitDate", CommonClass.gcdate));
                list.Add(new SqlPara("Amount", sAmount));
                list.Add(new SqlPara("AduitBillState", "提付异动转正常"));
                list.Add(new SqlPara("AduitMan", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("AduitSite", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("AduitWeb", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("AduitCause", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("AduitArea", CommonClass.UserInfo.AreaName));
                list.Add(new SqlPara("FetchPayVerifBalance", sFetchPayVerifBalance));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_CHECK_EXCEPTADUIT_FETCHPAY", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        private void CauseName_EditValueChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(AreaName, CauseName.EditValue.ToString());
            CommonClass.SetCauseWeb(WebName, CauseName.Text, AreaName.Text);
        }

        private void AreaName_EditValueChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(WebName, CauseName.Text, AreaName.Text);
        }
    }
}