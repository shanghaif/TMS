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
    public partial class frmFetchPayAduitList : BaseForm
    {
        public frmFetchPayAduitList()
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
            gcIsseleckedMode = GridOper.GetGridViewColumn(myGridView1, "ischecked");
            gcIsseleckedMode.Visible = false;
            chkALL.Visible = false;
            CommonClass.SetSite(siteName, true);
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
            if (ConvertType.ToString(type.EditValue) == "已出库")
            {
                try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("bdate", bdate.EditValue));
                    list.Add(new SqlPara("edate", edate.EditValue));
                    list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text.Trim()));
                    list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text.Trim()));
                    list.Add(new SqlPara("WebName", WebName.Text.Trim() == "全部" ? "%%" : WebName.Text.Trim()));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_FETCHPAYFORADUIT", list);
                    DataSet ds = SqlHelper.GetDataSet(sps);

                    if (ds == null || ds.Tables.Count == 0) return;
                    DataRow[] dr;
                    //for (int i = ds.Tables[0].Rows.Count - 1; i >= 0; i--)
                    //{
                    //    string s = ConvertType.ToString(ds.Tables[0].Rows[i]["BillNo"]);

                    //    dr = ds.Tables[0].Select("BillNo='" + ConvertType.ToString(ds.Tables[0].Rows[i]["BillNo"])+"'");
                    //    if (dr.Length > 1)
                    //    {
                    //        for (int j = 1; j < dr.Length; j++)
                    //        {
                    //            ds.Tables[0].Rows.Remove(dr[j]);
                    //            //if (ds.Tables[0].Columns.Contains("FetchPay")) dr[j]["FetchPay"] = DBNull.Value;
                    //            //if (ds.Tables[0].Columns.Contains("NowPay")) dr[j]["NowPay"] = DBNull.Value;
                    //            //if (ds.Tables[0].Columns.Contains("ShortOwePay")) dr[j]["ShortOwePay"] = DBNull.Value;
                    //            //if (ds.Tables[0].Columns.Contains("MonthPay")) dr[j]["MonthPay"] = DBNull.Value;
                    //        }
                    //    }
                    //    Application.DoEvents();
                    //}
                    myGridControl1.DataSource = ds.Tables[0];
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
                finally
                {
                }
            }
            else
            {
                try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("bdate", bdate.DateTime));
                    list.Add(new SqlPara("edate", edate.DateTime));
                    list.Add(new SqlPara("siteName", siteName.Text.Trim() == "全部" ? "%%" : siteName.Text.Trim()));
                    list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text.Trim()));
                    list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text.Trim()));
                    list.Add(new SqlPara("WebName", WebName.Text.Trim() == "全部" ? "%%" : WebName.Text.Trim()));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_FETCHPAYFORADUIT_NOOUT", list);
                    DataSet ds = SqlHelper.GetDataSet(sps);

                    if (ds == null || ds.Tables.Count == 0) return;
                    DataRow[] dr;
                    for (int i = ds.Tables[0].Rows.Count - 1; i >= 0; i--)
                    {
                        string s = ConvertType.ToString(ds.Tables[0].Rows[i]["BillNo"]);

                        dr = ds.Tables[0].Select("BillNo='" + ConvertType.ToString(ds.Tables[0].Rows[i]["BillNo"]) + "'");
                        if (dr.Length > 1)
                        {
                            for (int j = 1; j < dr.Length; j++)
                            {
                                ds.Tables[0].Rows.Remove(dr[j]);
                                //if (ds.Tables[0].Columns.Contains("FetchPay")) dr[j]["FetchPay"] = DBNull.Value;
                                //if (ds.Tables[0].Columns.Contains("NowPay")) dr[j]["NowPay"] = DBNull.Value;
                                //if (ds.Tables[0].Columns.Contains("ShortOwePay")) dr[j]["ShortOwePay"] = DBNull.Value;
                                //if (ds.Tables[0].Columns.Contains("MonthPay")) dr[j]["MonthPay"] = DBNull.Value;
                            }
                        }
                        Application.DoEvents();
                    }
                    myGridControl1.DataSource = ds.Tables[0];
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
                finally
                {
                }
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
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            try
            {
                string sShowOK = "提付异常金额：" + ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "FetchPay")) + "\r\n是否转入部门：" + ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "BegWeb")) + "\r\n是否继续？";

                if (MsgBox.ShowYesNo(sShowOK) != DialogResult.Yes)
                {
                    return;
                }

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("billNo", myGridView1.GetRowCellValue(rowhandle, "BillNo")));
                list.Add(new SqlPara("EXFee", myGridView1.GetRowCellValue(rowhandle, "FetchPay")));
                list.Add(new SqlPara("ExtFeeType", "提付"));
                list.Add(new SqlPara("ExtInDate", CommonClass.gcdate));
                list.Add(new SqlPara("ExtInMen", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("ExtSite", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("ExtWeb", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("ExtCause", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("ExtArea", CommonClass.UserInfo.AreaName));
                list.Add(new SqlPara("ExtType", "提付转异常"));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_INTO_FETCHPAYEXCEPTION", list);
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
            if (myGridView1.RowCount == 0) return;

            myGridView1.PostEditor();
            string sBillNo = "", sAmount = "";
            float Amount = 0, FetchPayVerifBalance = 0;
            float sumAmount = 0;
            try
            {

                if (ConvertType.ToString(type.Text) == "已出库")
                {
                    for (int i = 0; i < myGridView1.RowCount; i++)
                    {
                        Amount = ConvertType.ToFloat(myGridView1.GetRowCellValue(i, "Amount"));
                        FetchPayVerifBalance = ConvertType.ToFloat(myGridView1.GetRowCellValue(i, "FetchPayVerifBalance"));
                        if (Amount <= 0 || Amount > FetchPayVerifBalance) continue;

                        sBillNo += myGridView1.GetRowCellValue(i, "BillNo") + "@";//运单号
                        sAmount += Amount + "@";//审核的金额
                        sumAmount = sumAmount + Amount;
                    }
                }
                else
                {
                    for (int i = 0; i < myGridView1.RowCount; i++)
                    {
                        if (ConvertType.ToString(myGridView1.GetRowCellValue(i, "ischecked")) == "1")
                        {
                            Amount = ConvertType.ToFloat(myGridView1.GetRowCellValue(i, "Amount"));
                            FetchPayVerifBalance = ConvertType.ToFloat(myGridView1.GetRowCellValue(i, "FetchPayVerifBalance"));
                            if (Amount <= 0 || Amount > FetchPayVerifBalance) continue;

                            sBillNo += myGridView1.GetRowCellValue(i, "BillNo") + "@";//运单号
                            sAmount += Amount + "@";//审核的金额
                            sumAmount = sumAmount + Amount;
                        }
                    }
                }
                if (sBillNo == "") return;
                string sShowOK = "提付审核总票数：" + ConvertType.ToString(myGridView1.RowCount)
                    + "\r\n提付审核总金额：" + ConvertType.ToString(sumAmount) + "\r\n提付审核人：" + CommonClass.UserInfo.UserName + "\r\n是否继续？";

                if (MsgBox.ShowYesNo(sShowOK) != DialogResult.Yes)
                {
                    return;
                }
                // if (MsgBox.ShowYesNo("是否审核？\r\r请确认审核金额是否正确！") != DialogResult.Yes) return;

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNoStr", sBillNo));
                list.Add(new SqlPara("AmountStr", sAmount));
                list.Add(new SqlPara("AduitBillState", "提付"));
                list.Add(new SqlPara("AduitMan", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("AduitSite", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("AduitWeb", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("AduitCause", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("AduitArea", CommonClass.UserInfo.AreaName));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_CHECK_FETCHPAYADUIT", list);
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
                float Amount = ConvertType.ToFloat(myGridView1.GetFocusedRowCellValue("Amount"));
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

        private void type_EditValueChanged(object sender, EventArgs e)
        {
            if (ConvertType.ToString(type.EditValue) == "未出库")
            {
                siteName.Visible = true;
                WebName.Visible = false;
                label5.Text = "站点";
                AreaName.Enabled = false;
                CauseName.Enabled = false;
                barButtonItem10.Enabled = false;
                gcIsseleckedMode.Visible = true;
                chkALL.Visible = true;
                getData();
            }
            else
            {
                siteName.Visible = false;
                WebName.Visible = true;
                label5.Text = "网点";
                AreaName.Enabled = true;
                CauseName.Enabled = true;
                barButtonItem10.Enabled = true;
                gcIsseleckedMode.Visible = false;
                chkALL.Visible = false;
                getData();
            }
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