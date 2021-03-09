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
    public partial class frmIncomeExpendMonth : BaseForm
    {
        public frmIncomeExpendMonth()
        {
            InitializeComponent();
        }

        private void frmIncomeExpendMonth_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            BarMagagerOper.SetBarPropertity(bar3);
            FixColumn fix = new FixColumn(bandedGridView1, barSubItem1);
            FixColumn fix1 = new FixColumn(bandedGridView1, barSubItem2);
            GridOper.RestoreGridLayout(bandedGridView1, this.Text);
            dateEdit1.EditValue = CommonClass.gbdate.AddDays(-7);
            dateEdit2.EditValue = CommonClass.gbdate;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", dateEdit1.EditValue));
                list.Add(new SqlPara("edate", dateEdit2.EditValue));
                list.Add(new SqlPara("Site", comboBoxEdit1.Text.Trim() == "全部" ? "%%" : comboBoxEdit1.Text.Trim()));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_IncomeExpend_Month", list);
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

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(bandedGridView1);
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(bandedGridView1, this.Text);
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(bandedGridView1);
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
                list.Add(new SqlPara("bdate", dateEdit1.EditValue));
                list.Add(new SqlPara("edate", dateEdit2.EditValue));
                list.Add(new SqlPara("Site", comboBoxEdit1.Text.Trim() == "全部" ? "%%" : comboBoxEdit1.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_IncomeExpend_Month_Detail", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                frmIncomeExpendMonthDetail frm = new frmIncomeExpendMonthDetail();
                frm.ds = ds;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                return;
            }
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            showdetail();
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(bandedGridView1);
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void bandedGridView1_DoubleClick(object sender, EventArgs e)
        {
            showdetail();
        }
    }
}
