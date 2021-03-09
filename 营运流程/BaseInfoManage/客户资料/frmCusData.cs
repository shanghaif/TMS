using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Common;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmCusData : ZQTMS.Tool.BaseForm
    {
        public frmCusData()
        {
            InitializeComponent();
        }

        private void frmCusData_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("发货资料");//xj/2019/5/28
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar11); //如果有具体的工具条，就引用其实例
            freshData();
        }

        private void barBtnCusAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmAddCusData frm = new frmAddCusData();
            frm.ShowDialog();

        }

        private void barBtnCusMod_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "CustID").ToString());

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("CustID", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASCUST_ByID", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                DataRow dr = ds.Tables[0].Rows[0];
                frmAddCusData frm = new frmAddCusData();
                frm.dr = dr;
                frm.ShowDialog();
            }
             catch (Exception ex)
             {
                 MsgBox.ShowException(ex);
             }
        }

        private void barBtnCusDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "CustID").ToString());

                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("CustID", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_BASCUST", list);
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

        private void barBtnCusFilter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barBtnCusFresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            freshData();
        }

        private void barBtnvCusExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, this.Text);
        }

        private void barBtnCusExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
        private void freshData() 
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASCUST", list);
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
            frmCusDataImport frm = new frmCusDataImport();
            frm.ShowDialog();
        }
    }
}
