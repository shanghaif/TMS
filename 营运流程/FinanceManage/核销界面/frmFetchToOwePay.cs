﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using DevExpress.XtraGrid;

namespace ZQTMS.UI
{
    public partial class frmFetchToOwePay : BaseForm
    {
        public frmFetchToOwePay()
        {
            InitializeComponent();
        }

        private void frmDiscountTransfer_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("提付转欠款核销");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例 
            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
            //设置颜色
            GridOper.CreateStyleFormatCondition(myGridView1, "VerifyStatus", FormatConditionEnum.Equal, "已核销", Color.LightGreen);
            GridOper.CreateStyleFormatCondition(myGridView1, "VerifyStatus", FormatConditionEnum.Equal, "部分核销", Color.Yellow);

            CommonClass.SetCause(Cause, true);
            bsite.Text = CommonClass.UserInfo.SiteName;
            esite.Text = "全部";

            Cause.Text = CommonClass.UserInfo.CauseName;
            Area.Text = CommonClass.UserInfo.AreaName;
            
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;

            CommonClass.SetWeb(sqWeb, "全部");
            CommonClass.SetSite(bsite, true);
            CommonClass.SetSite(esite, true);
            //CommonClass.SetArea(Area, Cause.Text, true);
            //CommonClass.SetCauseWeb(web, Cause.Text, Area.Text);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);

            //不是总部的财务，事业部固定
            //if (CommonClass.UserInfo.SiteName != "总部" && CommonClass.UserInfo.WebName.ToString().Contains("财务"))
            //{
            //    Cause.Enabled = false;
            //}
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Cause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(Area, Cause.Text);
        }

        //private void Area_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    CommonClass.SetCauseWeb(web, Cause.Text, Area.Text);
        //}

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "提付转欠款核销明细");
        }
        frmFetchToOwePayLoad ww;
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //frmFetchToOwePayLoad ww = new frmFetchToOwePayLoad();
            //ww.ShowDialog();
            string frm = "frmFetchToOwePayLoad";
            if (CommonClass.CheckFormIsOpen(frm) == false)
            {
                ww = new frmFetchToOwePayLoad();
                ww.Show();
            }
            else
            {
                ww.Focus();
            }
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("CauseName", Cause.Text.Trim() == "全部" ? "%%" : Cause.Text.Trim()));
                list.Add(new SqlPara("AreaName", Area.Text.Trim() == "全部" ? "%%" : Area.Text.Trim()));
                list.Add(new SqlPara("BegWeb", sqWeb.Text.Trim() == "全部" ? "%%" : sqWeb.Text.Trim()));
                //list.Add(new SqlPara("BegWeb", sqWeb.Text));
                list.Add(new SqlPara("StartSite", bsite.Text.Trim() == "全部" ? "%%" : bsite.Text.Trim()));
                list.Add(new SqlPara("TransferSite", esite.Text.Trim() == "全部" ? "%%" : esite.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_FetchToOwePay_NEW", list);
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
            //设置颜色显示
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
    }
}