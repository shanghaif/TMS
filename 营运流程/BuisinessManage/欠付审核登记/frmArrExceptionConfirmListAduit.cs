using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraBars;
using System.Reflection;
using DevExpress.XtraGrid;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmArrExceptionConfirmListAduit : BaseForm
    {
        public frmArrExceptionConfirmListAduit()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView9);
            GridOper.SetGridViewProperty(myGridView9);
            BarMagagerOper.SetBarPropertity(bar3);
            GridOper.RestoreGridLayout(myGridView9);
            FixColumn fix = new FixColumn(myGridView9, barSubItem2);

            bdate.DateTime = DateTime.Parse(DateTime.Now.AddDays(-1).ToShortDateString() + " 00:00:00");
            edate.DateTime = DateTime.Parse(DateTime.Now.ToShortDateString() + " 23:59:59");

            CommonClass.SetCause(Cuase, true);
            Cuase.EditValue = CommonClass.UserInfo.CauseName;
            Area.EditValue = CommonClass.UserInfo.AreaName;
            web.EditValue = CommonClass.UserInfo.WebName;
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate", edate.DateTime));
                list.Add(new SqlPara("cuasename", Cuase.Text.Trim()));
                list.Add(new SqlPara("areaname", Area.Text.Trim()));
                list.Add(new SqlPara("webname", web.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "[QSP_Get_WayBill_ArrConfirm_Exception]", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl8.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.ToString());
            }
            finally
            {
                if (myGridView9.RowCount < 1000) myGridView9.BestFitColumns();
            }
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rows = myGridView9.FocusedRowHandle;
            if (rows < 0) return;
            if (myGridView9.GetRowCellValue(rows, "ArrComBedState").ToString() == "2")
            {
                MsgBox.ShowOK("�õ�����Ѿ���ˣ�������ѡ��");
                return;
            }

            if (MsgBox.ShowYesNo("ȷ��ת�����ˣ�") != DialogResult.Yes) return;
            try
            {
                string BillNo = Convert.ToString(myGridView9.GetRowCellValue(rows, "BillNo"));

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", BillNo));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "[USP_ADD_ArrConfirm_Exception_Out]", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                cbRetrieve_Click(null, null);
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rows = myGridView9.FocusedRowHandle;
            if (rows < 0) return;
            if (Convert.ToString(myGridView9.GetRowCellValue(rows, "ArrComBedState")) == "2") return;
            if (MsgBox.ShowYesNo("������˲����棡�Ƿ������") != DialogResult.Yes) return;
            try
            {
                string BillNo = Convert.ToString(myGridView9.GetRowCellValue(rows, "BillNo"));

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", BillNo));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "[USP_ADD_ArrConfirm_Exception_Bad]", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                cbRetrieve_Click(null, null);
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void Cuase_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(Area, Cuase.Text, true);
            CommonClass.SetCauseWeb(web, Cuase.Text, Area.Text);
        }

        private void Area_EditValueChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(web, Cuase.Text.Trim(), Area.Text.Trim());
        }

        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView9);
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView9);
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView9.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView9);
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView9);
        }
    }
}