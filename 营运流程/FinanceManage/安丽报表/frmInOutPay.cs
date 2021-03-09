using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmInOutPay : BaseForm
    {
        public frmInOutPay()
        {
            InitializeComponent();
        }

        private void frmIncomeExpend_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("收入支出汇总");
            CommonClass.FormSet(this);
            BarMagagerOper.SetBarPropertity(bar1,bar4);
            FixColumn fix = new FixColumn(bandedGridView1, barSubItem2);
            FixColumn fix1 = new FixColumn(gridView2, barSubItem5);
            GridOper.RestoreGridLayout(bandedGridView1, this.Text);
            dateEdit1.EditValue = CommonClass.gbdate.AddDays(-7);
            dateEdit2.EditValue = CommonClass.gbdate;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", dateEdit1.EditValue));
                list.Add(new SqlPara("t2", dateEdit2.EditValue));
                list.Add(new SqlPara("bsite", bsite.Text.Trim() == "全部" ? "%%" : bsite.Text.Trim()));
                list.Add(new SqlPara("esite", esite.Text.Trim() == "全部" ? "%%" : esite.Text.Trim()));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_InOutPay_1", list);
                DataSet ds = SqlHelper.GetDataSet(spe);
                if (ds.Tables.Count == 0 || ds.Tables[0] == null) return;
                gridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                return;
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(bandedGridView1, this.Text);
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(bandedGridView1, this.Text);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(bandedGridView1);
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(bandedGridView1);
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(bandedGridView1);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            showdetail();
        }

        private void bandedGridView1_DoubleClick(object sender, EventArgs e)
        {
            showdetail();
        }

        private void showdetail()
        {
            try
            {
                if (bandedGridView1.FocusedRowHandle < 0)
                {
                    MsgBox.ShowError("请选择一条数据！");
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("typeway", Convert.ToString(bandedGridView1.GetFocusedRowCellValue("typeway"))));
                list.Add(new SqlPara("t1", dateEdit1.EditValue));
                list.Add(new SqlPara("t2", dateEdit2.EditValue));
                list.Add(new SqlPara("bsite", bsite.Text.Trim() == "全部" ? "%%" : bsite.Text.Trim()));
                list.Add(new SqlPara("esite", esite.Text.Trim() == "全部" ? "%%" : esite.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_InOutPay_Detail_1", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                gridControl2.DataSource = ds.Tables[0];

                xtraTabControl1.SelectedTabPageIndex = 1;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                return;
            }
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(gridView2);
        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(gridView2, this.Text);
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(gridView2);
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(gridView2, this.Text);
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(gridView2);
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            showdetail();
        }
    }
}
