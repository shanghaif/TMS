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
using DevExpress.XtraEditors;

namespace ZQTMS.UI
{
    public partial class frmProfitsStatisiticals : BaseForm
    {
        public frmProfitsStatisiticals()
        {
            InitializeComponent();
        }



        private void frmProfitsStatisiticals_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("利润统计表");//xj/2019/5/28
            CommonClass.FormSet(this, false, true);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3);
            GridOper.RestoreGridLayout(myGridView1);
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            bsite.Text = CommonClass.UserInfo.SiteName;
            CommonClass.SetSite(bsite, true);
            CommonClass.SetSite(esite, true);
            CommonClass.SetWeb(webname, true);
            webname.Text = CommonClass.UserInfo.WebName;
        }



        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("bsite", bsite.Text.Trim() == "全部" ? "%%" : bsite.Text.Trim()));
                list.Add(new SqlPara("esite", esite.Text.Trim() == "全部" ? "%%" : esite.Text.Trim()));
                list.Add(new SqlPara("webname", webname.Text.Trim() == "全部" ? "%%" : webname.Text.Trim()));

                //gridControl1.DataSource = SqlHelper.GetDataTable(new SqlParasEntity(OperType.Query, "QSP_Get_ProfitsStatistical_Detail", list));

                 SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_Get_ProfitsStatistical_Detail", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                gridControl1.DataSource = ds.Tables[0];

                decimal a = 0, b = 0;
                for (int i = 0; i < bandedGridView1.RowCount; i++)
                { 
                a = a+ConvertType.ToDecimal(ds.Tables[0].Rows[i]["HappenAmount"]);
                b = b + ConvertType.ToDecimal(ds.Tables[0].Rows[i]["ZCHappenAmount"]);
                }
                edaccyestoday.Text = Math.Round(a, 2).ToString();

                edacctoday.Text = Math.Round(b, 2).ToString();

                decimal accyestoday = 0;
                if (edaccyestoday.Text.Trim() != "")
                {
                    accyestoday = Convert.ToDecimal(edaccyestoday.Text.Trim());
                }

                decimal acctoday = 0;
                if (edacctoday.Text.Trim() != "")
                {
                    acctoday = Convert.ToDecimal(edacctoday.Text.Trim());
                }
                edaccnow.Text = Math.Round(accyestoday - acctoday, 2).ToString();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }


            


        }

        //双击弹出明细
        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            int rowhandle = bandedGridView1.FocusedRowHandle;
            //int rowhandle = gridControl1.fo
            if (rowhandle < 0 ) return;
            string VerifyOffType = bandedGridView1.GetRowCellValue(rowhandle, "IncomeProject").ToString();
            string VerifyOffType1 = bandedGridView1.GetRowCellValue(rowhandle, "SpendingProject").ToString();
            if (VerifyOffType != "") myGridControl1.DataSource = SqlHelper.GetDataTable(new SqlParasEntity(OperType.Query, "QSP_Get_ProfitsStatistical_Summary", new List<SqlPara> { new SqlPara("VerifyOffType", VerifyOffType), new SqlPara("VerifyOffType1", VerifyOffType1), new SqlPara("t1", bdate.DateTime), new SqlPara("t2", edate.DateTime), new SqlPara("bsite", bsite.Text.Trim() == "全部" ? "%%" : bsite.Text.Trim()), new SqlPara("esite", esite.Text.Trim() == "全部" ? "%%" : esite.Text.Trim()), new SqlPara("webname", webname.Text.Trim() == "全部" ? "%%" : webname.Text.Trim()) }));
            xtraTabControl1.SelectedTabPageIndex = 1;

            if (VerifyOffType1 != "") myGridControl1.DataSource = SqlHelper.GetDataTable(new SqlParasEntity(OperType.Query, "QSP_Get_ProfitsStatistical_Summary", new List<SqlPara> { new SqlPara("VerifyOffType", VerifyOffType), new SqlPara("VerifyOffType1", VerifyOffType1), new SqlPara("t1", bdate.DateTime), new SqlPara("t2", edate.DateTime), new SqlPara("bsite", bsite.Text.Trim() == "全部" ? "%%" : bsite.Text.Trim()), new SqlPara("esite", esite.Text.Trim() == "全部" ? "%%" : esite.Text.Trim()), new SqlPara("webname", webname.Text.Trim() == "全部" ? "%%" : webname.Text.Trim()) }));
            xtraTabControl1.SelectedTabPageIndex = 1;
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 0)
            {
                GridOper.AllowAutoFilter(bandedGridView1);
            }
            if (xtraTabControl1.SelectedTabPageIndex == 1)
            {
                GridOper.AllowAutoFilter(myGridView1);
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 1)
            {
                GridOper.SaveGridLayout(myGridView1);
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 1)
            {
                GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
            }
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 0)
            {
                GridOper.ExportToExcel(bandedGridView1, "利润统计汇总");
            }
            if (xtraTabControl1.SelectedTabPageIndex == 1)
            {
                GridOper.ExportToExcel(myGridView1, "利润统计明细");
            }
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }


        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = bandedGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            string VerifyOffType = bandedGridView1.GetRowCellValue(rowhandle, "IncomeProject").ToString();
            string VerifyOffType1 = bandedGridView1.GetRowCellValue(rowhandle, "SpendingProject").ToString();
            if (VerifyOffType != "") myGridControl1.DataSource = SqlHelper.GetDataTable(new SqlParasEntity(OperType.Query, "QSP_Get_ProfitsStatistical_Summary", new List<SqlPara> { new SqlPara("VerifyOffType", VerifyOffType), new SqlPara("VerifyOffType1", VerifyOffType1), new SqlPara("t1", bdate.DateTime), new SqlPara("t2", edate.DateTime), new SqlPara("bsite", bsite.Text.Trim() == "全部" ? "%%" : bsite.Text.Trim()), new SqlPara("esite", esite.Text.Trim() == "全部" ? "%%" : esite.Text.Trim()), new SqlPara("webname", webname.Text.Trim() == "全部" ? "%%" : webname.Text.Trim()) }));
            xtraTabControl1.SelectedTabPageIndex = 1;

            if (VerifyOffType1 != "") myGridControl1.DataSource = SqlHelper.GetDataTable(new SqlParasEntity(OperType.Query, "QSP_Get_ProfitsStatistical_Summary", new List<SqlPara> { new SqlPara("VerifyOffType", VerifyOffType), new SqlPara("VerifyOffType1", VerifyOffType1), new SqlPara("t1", bdate.DateTime), new SqlPara("t2", edate.DateTime), new SqlPara("bsite", bsite.Text.Trim() == "全部" ? "%%" : bsite.Text.Trim()), new SqlPara("esite", esite.Text.Trim() == "全部" ? "%%" : esite.Text.Trim()), new SqlPara("webname", webname.Text.Trim() == "全部" ? "%%" : webname.Text.Trim()) }));
            xtraTabControl1.SelectedTabPageIndex = 1;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
