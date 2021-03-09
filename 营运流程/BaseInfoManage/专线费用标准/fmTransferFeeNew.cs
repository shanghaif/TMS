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
using DevExpress.XtraGrid.Columns;

namespace ZQTMS.UI
{
    public partial class fmTransferFeeNew : BaseForm
    {
        public fmTransferFeeNew()
        {
            InitializeComponent();

        }
        GridColumn gcIsseleckedMode;

        private void fmTransferFee_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("结算中转费");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar1); //如果有具体的工具条，就引用其实例  
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
            LoadData();
            gcIsseleckedMode = GridOper.GetGridViewColumn(myGridView1, "ischecked");
        }

        private void LoadData()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASTRANSFERFEE_GX", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }


        private void barBtnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            fmTransferFeeAddNew frm = new fmTransferFeeAddNew();
            frm.ShowDialog(); 
        }

        private void barBtnUpdate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "TransferFeeID").ToString());

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("TransferFeeID", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASTRANSFERFEE_ByID_GX", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                DataRow dr = ds.Tables[0].Rows[0];

                fmTransferFeeAddNew frm = new fmTransferFeeAddNew();
                frm.dr = dr;
                frm.ShowDialog(); 
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barBtnDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                myGridView1.PostEditor();
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "TransferFeeID").ToString());

                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                string TransferFeeIDs = "";
                string companyids = "";
                string rowStr = "";
                if (rowhandle >= 0)
                {
                    for (int i = 0; i < myGridView1.RowCount; i++)
                    {
                        if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) > 0)
                        {
                            TransferFeeIDs += myGridView1.GetRowCellValue(i, "TransferFeeID").ToString() + "@";
                            companyids += myGridView1.GetRowCellValue(i, "companyid").ToString() + "@";
                            rowStr = rowStr + i.ToString() + "@"; 
                        }
                    }
                }
                string[] rows = rowStr.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
                if (TransferFeeIDs == "") return;
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("TransferFeeID", TransferFeeIDs));
                list.Add(new SqlPara("companyids", companyids)); ;

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_BASTRANSFERFEE_GX", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    if (rows.Length > 0)
                    {
                        int i = 0;
                        foreach (var row in rows)
                        {
                            myGridView1.DeleteRow(Convert.ToInt32(row) - i);
                            i++;

                        }
                    }

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

        private void barBtnFilter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barBtnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadData();
        }

        private void barBtnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "结算中转费");
        }

        private void barBtnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barBtnImport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            fmTransferFeeUpNew up = new fmTransferFeeUpNew();
            up.ShowDialog();
        }

        private void chkALL_CheckedChanged(object sender, EventArgs e)
        {
            int a = chkALL.Checked == true ? 1 : 0;
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                myGridView1.SetRowCellValue(i, gcIsseleckedMode, a);
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

    }
}
