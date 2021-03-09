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
    public partial class frmDepartureAccDetail : BaseForm
    {
        public frmDepartureAccDetail()
        {
            InitializeComponent();
        }

        private void frmDepartureAccDetail_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1,myGridView2);
            GridOper.SetGridViewProperty(myGridView1,myGridView2);
            BarMagagerOper.SetBarPropertity(bar1,bar4); //如果有具体的工具条，就引用其实例
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
            GridOper.RestoreGridLayout(myGridView2, myGridView2.Guid.ToString());
            FixColumn fix = new FixColumn(myGridView1,barSubItem2);
            FixColumn fix1 = new FixColumn(myGridView2,barSubItem4);
            CommonClass.SetCause(Cause, true);
            Cause.EditValue = CommonClass.UserInfo.CauseName;
            Area.EditValue = CommonClass.UserInfo.AreaName;
            Web.EditValue = CommonClass.UserInfo.WebName;
        }

        private void Cause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(Area, Cause.Text, true);
        }

        private void Area_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Web.Properties.Items.Count != 0 || Web.Text != "全部")
                {
                    Web.Properties.Items.Clear();
                    Web.Text = "全部";
                }
                string causename = Cause.Text.Trim();
                string areaname = Area.Text.Trim();
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("causename", causename == "全部" ? "%%" : causename));
                list.Add(new SqlPara("areaname", areaname == "全部" ? "%%" : areaname));
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "Get_basSettleCenterAccount", list));
                if (ds == null || ds.Tables.Count == 0) return;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Web.Properties.Items.Add(ds.Tables[0].Rows[i]["AccountName"]);
                }
                Web.Properties.Items.Add("全部");  
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("Cause", Cause.Text.Trim() == "全部" ? "%%" : Cause.Text.Trim()));
                list.Add(new SqlPara("Area", Area.Text.Trim() == "全部" ? "%%" : Area.Text.Trim()));
                list.Add(new SqlPara("Web", Web.Text.Trim() == "全部" ? "%%" : Web.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_Get_DepartureAccDetail", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch(Exception ex)
            {
                MsgBox.ShowException(ex);
                return;
            }
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "到车结算汇总表");
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView2);
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView2);
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView2.Guid.ToString());
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView2);
        }

        private void barButtonItem14_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView2, "到车结算汇总明细表");
        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem16_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            showdetail();
        }

        private void myGridView1_DoubleClick(object sender, EventArgs e)
        {
            showdetail();
        }

        private void showdetail()
        {
            try
            {
                if (myGridView1.FocusedRowHandle < 0)
                {
                    MsgBox.ShowError("请选择一条数据！");
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", myGridView1.GetFocusedRowCellValue("DepartureBatch").ToString()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_Get_DepartureAcc", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl2.DataSource = ds.Tables[0];

                xtraTabControl1.SelectedTabPageIndex = 1;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                return;
            }
        }
    }
}
