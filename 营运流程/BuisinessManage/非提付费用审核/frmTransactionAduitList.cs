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
    public partial class frmTransactionAduitList : BaseForm
    {
        public frmTransactionAduitList()
        {
            InitializeComponent();
        }
        int TransactionType;

        private void frmShortConnOutbound_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem4);

            DateTime bdt = CommonClass.gbdate;
            bdt = bdt.AddDays(-1);
            bdt = bdt.AddHours(8);
            bdate.DateTime = bdt;
            DateTime edt = CommonClass.gedate;
            edt = edt.AddHours(7 - edt.Hour);
            edate.DateTime = edt;
            CommonClass.SetCause(CauseName, true);
            CauseName.EditValue = CommonClass.UserInfo.CauseName;
            AreaName.EditValue = CommonClass.UserInfo.AreaName;
            WebName.EditValue = CommonClass.UserInfo.WebName;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            getData();
        }
        public void getData()
        {
            
            if (bdate.DateTime.Date > edate.DateTime.Date)
            {
                MsgBox.ShowError("开始日期不能大于结束日期");
                return;
            }
            if (type.Text.Trim().ToString() == "提付异动")
            {
                TransactionType = 0;
            }
            else if (type.Text.Trim().ToString() == "非提付异动")
            {
                TransactionType = 1;
            }else
            {
                TransactionType = 2;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate", edate.DateTime));
                list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text.Trim()));
                list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text.Trim()));
                list.Add(new SqlPara("WebName", WebName.Text.Trim() == "全部" ? "%%" : WebName.Text.Trim()));
                list.Add(new SqlPara("TransactionType", TransactionType));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_TRANSACTIONFORADUIT_TWO", list);
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
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            try
            {
                string sShowOK = "现付异动转异常金额：" + ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "ChangePlusFee")) + "\r\n是否转入部门：" + ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "BegWeb")) + "\r\n是否继续？";

                if (MsgBox.ShowYesNo(sShowOK) != DialogResult.Yes)
                {
                    return;
                }

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("billNo", myGridView1.GetRowCellValue(rowhandle, "ID").ToString()));
                list.Add(new SqlPara("EXFee", myGridView1.GetRowCellValue(rowhandle, "ChangePlusFee")));
                list.Add(new SqlPara("ExtFeeType", "非提付异动"));
                list.Add(new SqlPara("ExtInDate", CommonClass.gcdate));
                list.Add(new SqlPara("ExtInMen", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("ExtSite", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("ExtWeb", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("ExtCause", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("ExtArea", CommonClass.UserInfo.AreaName));
                list.Add(new SqlPara("ExtType", "非提付异动转异常"));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_INTO_TRANSACTIONEXCEPTION_NOWPAY", list);


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

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
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
           
            try
            {
                myGridView1.PostEditor();
                string sBillNo = "",  sBillNo1 = "";
                string sAmount = "",sAmount1 = "";
                string sNowPayVerifBalance = "", sFetchPayVerifBalance = "";
                float sumAmount = 0, sumAmount1 = 0;
                int num = 0, num1 = 0;
                int n = 0;
                if (myGridView1.RowCount > 0)
                {
                    for (int i = 0; i < myGridView1.RowCount; i++)
                    {
                        if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) > 0)
                        {
                            if (ConvertType.ToString(myGridView1.GetRowCellValue(i, "FetchPay")) != "0.00")
                            {
                                sumAmount1 = sumAmount1 + ConvertType.ToFloat(myGridView1.GetRowCellValue(i, "Amount"));
                                sBillNo1 = sBillNo1 + myGridView1.GetRowCellValue(i, "ID") + "@";
                                sAmount1 = sAmount1 + myGridView1.GetRowCellValue(i, "Amount") + "@";
                                sFetchPayVerifBalance = sFetchPayVerifBalance + myGridView1.GetRowCellValue(i, "NowPayVerifBalance") + "@";
                                num1++;

                            }
                            else
                            {

                                sumAmount = sumAmount + ConvertType.ToFloat(myGridView1.GetRowCellValue(i, "Amount"));
                                sBillNo = sBillNo + myGridView1.GetRowCellValue(i, "ID") + "@";
                                sAmount = sAmount + myGridView1.GetRowCellValue(i, "Amount") + "@";
                                sNowPayVerifBalance = sNowPayVerifBalance + myGridView1.GetRowCellValue(i, "NowPayVerifBalance") + "@";
                                num++;
                            }
                        }

                    }
                }
                if (num+num1 ==0)
                {
                    MsgBox.ShowOK("请选择数据！");
                    return;
                }
                string sShowOK = "异动审核总票数：" + (num +num1)
                   + "\r\n异动审核总金额：" + ConvertType.ToString(sumAmount + sumAmount1) + "\r\n异动审核人：" + CommonClass.UserInfo.UserName + "\r\n是否继续？";

                if (MsgBox.ShowYesNo(sShowOK) != DialogResult.Yes)
                {
                    return;
                }
                if (num > 0)
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("billNo", sBillNo));
                    list.Add(new SqlPara("AduitDate", CommonClass.gcdate));
                    list.Add(new SqlPara("Amount", sAmount));
                    list.Add(new SqlPara("AduitBillState", "非提付异动"));
                    list.Add(new SqlPara("AduitMan", CommonClass.UserInfo.UserName));
                    list.Add(new SqlPara("AduitSite", CommonClass.UserInfo.SiteName));
                    list.Add(new SqlPara("AduitWeb", CommonClass.UserInfo.WebName));
                    list.Add(new SqlPara("AduitCause", CommonClass.UserInfo.CauseName));
                    list.Add(new SqlPara("AduitArea", CommonClass.UserInfo.AreaName));
                    list.Add(new SqlPara("NowPayVerifBalance", sNowPayVerifBalance));
                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_CHECK_TRANSACTIONADUIT_NOWPAY", list);
                    if (SqlHelper.ExecteNonQuery(sps) > 0)
                    { 
                    n++;
                    }
                }
                if (num1 > 0)
                {

                    List<SqlPara> list1 = new List<SqlPara>();
                    list1.Add(new SqlPara("billNo", sBillNo1));
                    list1.Add(new SqlPara("AduitDate", CommonClass.gcdate));
                    list1.Add(new SqlPara("Amount", sAmount1));
                    list1.Add(new SqlPara("AduitBillState", "提付异动"));
                    list1.Add(new SqlPara("AduitMan", CommonClass.UserInfo.UserName));
                    list1.Add(new SqlPara("AduitSite", CommonClass.UserInfo.SiteName));
                    list1.Add(new SqlPara("AduitWeb", CommonClass.UserInfo.WebName));
                    list1.Add(new SqlPara("AduitCause", CommonClass.UserInfo.CauseName));
                    list1.Add(new SqlPara("AduitArea", CommonClass.UserInfo.AreaName));
                    list1.Add(new SqlPara("FetchPayVerifBalance", sFetchPayVerifBalance));
                    SqlParasEntity sps1 = new SqlParasEntity(OperType.Execute, "USP_CHECK_TRANSACTIONADUIT_FETCHPAY", list1);

                    if (SqlHelper.ExecteNonQuery(sps1) > 0)
                    {
                        //MsgBox.ShowOK();
                        //getData();
                        n++;
                    }
                }
                if (n > 0) 
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
            CommonClass.SetCauseWeb(WebName, CauseName.Text.Trim(), AreaName.Text.Trim());
        }

        private void AreaName_EditValueChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(WebName, CauseName.Text.Trim(), AreaName.Text.Trim());
        }

        /// <summary>
        /// 筛选条件框的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectCondition_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                (sender as ComboBoxEdit).SelectAll();
            }
            catch { }
        }
    }
}