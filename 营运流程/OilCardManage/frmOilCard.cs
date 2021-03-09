using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using DevExpress.XtraEditors;

namespace ZQTMS.UI
{
    public partial class frmOilCard : BaseForm
    {
        public frmOilCard()
        {
            InitializeComponent();
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_OILCARD_RECORD", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                if (myGridView1.RowCount < 2000) myGridView1.BestFitColumns();
            }
        }

        private void WayBillRecord_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("油卡记录");//xj/2019/5/28
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例

            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;

            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "油卡记录");
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmOilCardAdd foc = frmOilCardAdd.Get_frmOilCardAdd;
            foc.Show();
            foc.TopMost = true;
            foc.TopMost = false;
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;

            frmOilCardAdd foc = frmOilCardAdd.Get_frmOilCardAdd;
            foc.Show();
            foc._OilCardNo = GridOper.GetRowCellValueString(myGridView1, rowhandle, "OilCardNo");
            foc.Company.Text = GridOper.GetRowCellValueString(myGridView1, rowhandle, "Company");
            foc.getdata();
            foc.TopMost = true;
            foc.TopMost = false;
        }
    }
}