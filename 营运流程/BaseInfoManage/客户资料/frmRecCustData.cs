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

namespace ZQTMS.UI
{
    public partial class frmRecCustData : ZQTMS.Tool.BaseForm
    {
        public frmRecCustData()
        {
            InitializeComponent();
        }
        private void frmRecCustData_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("收货资料");//xj/2019/5/28
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar11); //如果有具体的工具条，就引用其实例
            freshData();
        }
        private void barBtnRecCusAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmAddRecCustData frm = new frmAddRecCustData();
            frm.ShowDialog();
            freshData();
        }

        private void barBtnRecCusMod_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "RCID").ToString());

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("RCID", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASRECEIVECUST_ByID", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                DataRow dr = ds.Tables[0].Rows[0];
                frmAddRecCustData frm = new frmAddRecCustData();
                frm.dr = dr;
                frm.ShowDialog();
                freshData();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barBtnRecCusDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "RCID").ToString());

                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("RCID", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_BASRECEIVECUST", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();

                    myGridView1.DeleteRow(rowhandle);
                    myGridView1.PostEditor();
                    myGridView1.UpdateCurrentRow();
                    myGridView1.UpdateSummary();
                    DataTable dt = myGridControl1.DataSource as DataTable;
                    dt.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barBtnRecCusFilter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barBtnRecCusFresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            freshData();
        }

        private void barBtnRecCusExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "收货人信息");
        }

        private void barBtnRecCusExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }
        private void freshData() 
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASRECEIVECUST", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmRecCustDataImport frm = new frmRecCustDataImport();
            frm.ShowDialog();
        }
    }
}
