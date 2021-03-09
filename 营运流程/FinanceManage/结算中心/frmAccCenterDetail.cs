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
using DevExpress.XtraGrid.Views.Grid;

namespace ZQTMS.UI
{
    public partial class frmAccCenterDetail : BaseForm
    {
        public frmAccCenterDetail()
        {
            InitializeComponent();
        }

        private void frmChargeApply_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("结算对账汇总明细");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, myGridView2, myGridView3, myGridView4, myGridView5, myGridView6, myGridView7, myGridView8, myGridView9, myGridView10,myGridView11,myGridView12,myGridView13,myGridView14);
            GridOper.SetGridViewProperty(myGridView1, myGridView2, myGridView3, myGridView4, myGridView5, myGridView6, myGridView7, myGridView8, myGridView9, myGridView10,myGridView11,myGridView12,myGridView13,myGridView14);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());

            CommonClass.SetCause(CauseName, true);
            CommonClass.SetArea(AreaName, CauseName.Text);

            CauseName.Text = CommonClass.UserInfo.CauseName;
            AreaName.Text = CommonClass.UserInfo.AreaName;
            SettlementAcc.Text = CommonClass.UserInfo.WebName;

            //不是总部的财务，事业部固定
            if (CommonClass.UserInfo.SiteName != "总部" && CommonClass.UserInfo.WebName.ToString().Contains("财务"))
            {
                CauseName.Enabled = false;
            }
        }
        private void setweb()
        {
            DataRow[] dr = CommonClass.dsWeb.Tables[0].Select("WebRole='加盟'");
            for (int i = 0; i < dr.Length; i++)
            {
                SettlementAcc.Properties.Items.Add(dr[i]["WebName"]);
            }
            if (CommonClass.UserInfo.SiteName != "总部")
            {
                SettlementAcc.Enabled = false;
                SettlementAcc.Text = CommonClass.UserInfo.WebName;
            }
            else
            {
                SettlementAcc.Properties.Items.Add("全部");
                SettlementAcc.Text = "全部";
            }
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("causename", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text.Trim()));
                list.Add(new SqlPara("areaname", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text.Trim()));
                list.Add(new SqlPara("PaySite", SettlementAcc.Text.Trim() == "全部" ? "%%" : SettlementAcc.Text.Trim()));
                
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ACCCENTER", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];

                List<SqlPara> list_2 = new List<SqlPara>();
                list_2.Add(new SqlPara("t1", bdate.DateTime));
                list_2.Add(new SqlPara("t2", edate.DateTime));
                list_2.Add(new SqlPara("causename", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text.Trim()));
                list_2.Add(new SqlPara("areaname", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text.Trim()));
                list_2.Add(new SqlPara("PaySite", SettlementAcc.Text.Trim() == "全部" ? "%%" : SettlementAcc.Text.Trim()));
                SqlParasEntity sps_2 = new SqlParasEntity(OperType.Query, "QSP_GET_ACCCENTER_YE", list_2);
                DataSet ds_2 = SqlHelper.GetDataSet(sps_2);

                if (ds_2.Tables.Count > 1)
                {
                    decimal acc1 = 0, acc2 = 0, acc3 = 0;
                    if (ds_2.Tables[0].Rows.Count > 0)
                    {
                        acc1 = ConvertType.ToDecimal(ds_2.Tables[0].Rows[0]["FeeAcc"]);
                    }
                    if (ds_2.Tables[1].Rows.Count > 0)
                    {
                        acc2 = ConvertType.ToDecimal(ds_2.Tables[1].Rows[0]["FeeAcc"]);
                    }
                    acc3 = acc1 + acc2;
                    textEdit1.Text = acc1.ToString();
                    textEdit2.Text = acc2.ToString();
                    textEdit3.Text = acc3.ToString();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "结算对账汇总");   
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void SettlementAcc_EditValueChanged(object sender, EventArgs e)
        {
            //PaySite.Properties.Items.Clear();
            //DataRow[] dr = CommonClass.dsWeb.Tables[0].Select("BelongCause='" + SettlementAcc.Text.Trim() + "'");
            //if (dr.Length > 0)
            //{
            //    for (int i = 0; i < dr.Length; i++)
            //    {
            //        PaySite.Properties.Items.Add(dr[i]["WebName"]);
            //    }
            //    PaySite.Properties.Items.Add("全部");
            //    PaySite.Text = "全部";
            //}
            //else
            //{
            //    PaySite.Properties.Items.Add(SettlementAcc.Text.Trim());
            //    PaySite.Properties.Items.Add("全部");
            //    PaySite.Text = "全部";
            //}
        }

        private void myGridView1_DoubleClick(object sender, EventArgs e)
        {
            //try
            //{
            //    int rows = myGridView1.FocusedRowHandle;
            //    if (rows < 0) return;
            //    List<SqlPara> list = new List<SqlPara>();
            //    list.Add(new SqlPara("t1", bdate.DateTime));
            //    list.Add(new SqlPara("t2", edate.DateTime));
            //    list.Add(new SqlPara("PaySite", myGridView1.GetRowCellValue(rows, "PaySite").ToString()));
            //    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ACCCENTER_DETAIL", list);
            //    DataSet ds = SqlHelper.GetDataSet(sps);
            //    if (ds == null || ds.Tables.Count == 0) return;
            //    myGridControl2.DataSource = ds.Tables[0];
            //    xtraTabControl1.SelectedTabPageIndex = 1;
            //}
            //catch (Exception ex)
            //{
            //    MsgBox.ShowException(ex);
            //}
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel((GridView)myGridControl2.MainView, "结算对账明细");
        }

        private void myGridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.Column.FieldName == "acc1")
            {
                myGridControl2.MainView = myGridView3;
                myGridView3.Columns["AllFee"].AppearanceCell.BackColor = Color.Yellow;
                myGridView3.Columns["PaymentAmout"].AppearanceCell.BackColor = Color.Yellow;
                myGridView3.Columns["GrossProfit"].AppearanceCell.BackColor = Color.Yellow;
                myGridView3.Columns["GrossProfitRate"].AppearanceCell.BackColor = Color.Yellow;
                getDetail("QSP_GET_ACCCENTER_DETAIL_ACC1");
            }
            else if (e.Column.FieldName == "acc2")
            {
                myGridControl2.MainView = myGridView4;
                getDetail("QSP_GET_ACCCENTER_DETAIL_ACC2");
            }
            else if (e.Column.FieldName == "acc3")
            {
                myGridControl2.MainView = myGridView5;
                getDetail("QSP_GET_ACCCENTER_DETAIL_ACC3");
            }
            else if (e.Column.FieldName == "acc4")
            {
                myGridControl2.MainView = myGridView6;
                getDetail("QSP_GET_ACCCENTER_DETAIL_ACC3"); //maohui20180505
                //getDetail("QSP_GET_ACCCENTER_DETAIL_ACC4");
            }
            else if (e.Column.FieldName == "acc5")
            {
                myGridControl2.MainView = myGridView7;
                getDetail("QSP_GET_ACCCENTER_DETAIL_ACC5");
            }
            else if (e.Column.FieldName == "acc6")
            {
                myGridControl2.MainView = myGridView8;
                getDetail("QSP_GET_ACCCENTER_DETAIL_ACC6");
            }
            else if (e.Column.FieldName == "acc7")
            {
                myGridControl2.MainView = myGridView9;
                getDetail("QSP_GET_ACCCENTER_DETAIL_ACC7");
            }
            else if (e.Column.FieldName == "acc9")
            {
                myGridControl2.MainView = myGridView10;
                getDetail("QSP_GET_ACCCENTER_DETAIL_ACC8");
            }
            else if (e.Column.FieldName == "acc10")
            {
                myGridControl2.MainView = myGridView11;
                getDetail("QSP_GET_ACCCENTER_DETAIL_ACC9");
            }
            else if (e.Column.FieldName == "acc11")
            {
                myGridControl2.MainView = myGridView12;
                getDetail("QSP_GET_ACCCENTER_DETAIL_ACC10");
            }
            else if (e.Column.FieldName == "acc12")  //maohui20180505
            {
                myGridControl2.MainView = myGridView13;
                getDetail("QSP_GET_ACCCENTER_DETAIL_ACC11");
            }
            else if (e.Column.FieldName == "acc13")  //maohui20180505
            {
                myGridControl2.MainView = myGridView14;
                getDetail("QSP_GET_ACCCENTER_DETAIL_ACC11");
            }
            else
            {
                myGridControl2.MainView = myGridView2;

                try
                {
                    int rows = myGridView1.FocusedRowHandle;
                    if (rows < 0) return;
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("t1", bdate.DateTime));
                    list.Add(new SqlPara("t2", edate.DateTime));
                    list.Add(new SqlPara("PaySite", myGridView1.GetRowCellValue(rows, "PaySite").ToString()));
                    list.Add(new SqlPara("ChangeDate", myGridView1.GetRowCellValue(rows, "ChangeDate").ToString()));
                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ACCCENTER_DETAIL", list);
                    DataSet ds = SqlHelper.GetDataSet(sps);
                    if (ds == null || ds.Tables.Count == 0) return;
                    myGridControl2.DataSource = ds.Tables[0];
                    xtraTabControl1.SelectedTabPageIndex = 1;
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }


            }
        }

        private void getDetail(string ProcedureName)
        {
            try
            {
                int rows = myGridView1.FocusedRowHandle;
                if (rows < 0) return;
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ChangeDate", myGridView1.GetRowCellValue(rows, "ChangeDate").ToString())); 
                list.Add(new SqlPara("PaySite", myGridView1.GetRowCellValue(rows, "PaySite").ToString()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, ProcedureName, list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl2.DataSource = ds.Tables[0];
                xtraTabControl1.SelectedTabPageIndex = 1;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void CauseName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(AreaName, CauseName.Text);
            CommonClass.SetCauseWeb(SettlementAcc, CauseName.Text, AreaName.Text);
        }

        private void AreaName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(SettlementAcc, CauseName.Text, AreaName.Text);
        }



    }
}