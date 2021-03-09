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

namespace ZQTMS.UI
{
    public partial class frmAccToTalDetail : BaseForm
    {
        public frmAccToTalDetail()
        {
            InitializeComponent();
        }

        private void frmChargeApply_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("结算账户汇总明细");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, myGridView2);
            GridOper.SetGridViewProperty(myGridView1, myGridView2);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
            //setweb();

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
        //private void setweb()
        //{
        //    //for (int i = 0; i < CommonClass.dsCause.Tables[0].Rows.Count; i++)
        //    //{
        //    //    SettlementAcc.Properties.Items.Add(CommonClass.dsCause.Tables[0].Rows[i]["CauseName"]);
        //    //}
        //    for (int i = 0; i < CommonClass.dsWeb.Tables[0].Rows.Count; i++)
        //    {
        //        SettlementAcc.Properties.Items.Add(CommonClass.dsWeb.Tables[0].Rows[i]["WebName"]);
        //    }
        //    if (CommonClass.UserInfo.SiteName != "总部")
        //    {
        //        SettlementAcc.Enabled = false;
        //        SettlementAcc.Text = CommonClass.UserInfo.WebName;
        //    }
        //    else
        //    {
        //        SettlementAcc.Properties.Items.Add("全部");
        //        SettlementAcc.Text = "全部";
        //    }
        //}

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("PaySite", SettlementAcc.Text.Trim() == "全部" ? "%%" : SettlementAcc.Text.Trim()));
                list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text.Trim()));
                list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ACCTOTAL", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];

                List<SqlPara> list_2 = new List<SqlPara>();
                list_2.Add(new SqlPara("t1", bdate.DateTime));
                list_2.Add(new SqlPara("t2", edate.DateTime));
                list_2.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text.Trim()));
                list_2.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text.Trim()));
                list_2.Add(new SqlPara("Webname", SettlementAcc.Text.Trim() == "全部" ? "%%" : SettlementAcc.Text.Trim()));
                SqlParasEntity sps_2 = new SqlParasEntity(OperType.Query, "QSP_GET_ACCTOTAL_YE", list_2);
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

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)//审核
        {
            //int rowhandle = myGridView1.FocusedRowHandle;
            //if (rowhandle < 0) return;
            //frmSetCreditLimit frm = new frmSetCreditLimit();
            //frm.sId = myGridView1.GetRowCellValue(rowhandle, "ID").ToString();
            //frm.sAccountName = myGridView1.GetRowCellValue(rowhandle, "AccountName").ToString();
            //frm.sAccountNO = myGridView1.GetRowCellValue(rowhandle, "AccountNO").ToString();
            //frm.sAccountType = myGridView1.GetRowCellValue(rowhandle, "AccountType").ToString();
            //frm.ShowDialog();
            //cbRetrieve_Click(sender, null);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "结算账户汇总");
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //int rowhandle = myGridView1.FocusedRowHandle;
            //if (rowhandle < 0) return;
            //frmSetAdjustLimit frm = new frmSetAdjustLimit();
            //frm.sId = myGridView1.GetRowCellValue(rowhandle, "ID").ToString();
            //frm.sUsingCredit = myGridView1.GetRowCellValue(rowhandle, "UsingCredit").ToString();
            //frm.sLeftCredit = myGridView1.GetRowCellValue(rowhandle, "LeftCredit").ToString();
            //frm.sCreditLimit = myGridView1.GetRowCellValue(rowhandle, "CreditLimit").ToString();
            //frm.ShowDialog();
            //cbRetrieve_Click(sender, null);
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
            try
            {
                int rows = myGridView1.FocusedRowHandle;
                if (rows < 0) return;
                string SerialNumber = myGridView1.GetRowCellValue(rows, "InoneFlag").ToString();

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SerialNumber", SerialNumber));
                list.Add(new SqlPara("PaySite", myGridView1.GetRowCellValue(rows, "WebName").ToString()));
                if (SerialNumber == "") 
                {
                    list.Add(new SqlPara("t1", bdate.DateTime));
                    list.Add(new SqlPara("t2", edate.DateTime));
                }
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ACCTOTAL_DETAIL", list);
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

        private void Cause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(AreaName, CauseName.Text);
            CommonClass.SetCauseWeb(SettlementAcc, CauseName.Text, AreaName.Text);
        }
        private void Area_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(SettlementAcc, CauseName.Text, AreaName.Text);
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView2, "结算账户明细");
        }
    }
}