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
    public partial class fmDenominatedFee : BaseForm
    {
        public fmDenominatedFee()
        {
            InitializeComponent();

        }
        GridColumn gcIsseleckedMode;

        private void fmTransferFee_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("客户计价标准");//xj/2019/5/28
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar1); //如果有具体的工具条，就引用其实例  
            LoadData();
            gcIsseleckedMode = GridOper.GetGridViewColumn(myGridView1, "ischecked");
        }

        private void LoadData()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_basDenominatedFee", list);
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
            fmDenominatedFeeAddNew frm = new fmDenominatedFeeAddNew();
            frm.ShowDialog(); 
        }

        private void barBtnUpdate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "DenominatedFeeID").ToString());

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DenominatedFeeID", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_basDenominatedFee_ByID_GX", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                DataRow dr = ds.Tables[0].Rows[0];

                fmDenominatedFeeAddNew frm = new fmDenominatedFeeAddNew();
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
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "DenominatedFeeID").ToString());

                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                string DenominatedFeeIDs = "";
                string companyids = "";
                string rowStr = "";
                if (rowhandle >= 0)
                {
                    for (int i = 0; i < myGridView1.RowCount; i++)
                    {
                        if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) > 0)
                        {
                            DenominatedFeeIDs += myGridView1.GetRowCellValue(i, "DenominatedFeeID").ToString() + "@";
                            companyids += myGridView1.GetRowCellValue(i, "companyid").ToString() + "@";
                            rowStr = rowStr + i.ToString() + "@"; 
                        }
                    }
                }
                string[] rows = rowStr.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
                if (DenominatedFeeIDs == "") return;
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DenominatedFeeID", DenominatedFeeIDs));
                list.Add(new SqlPara("companyids", companyids)); ;

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_basDenominatedFee", list);
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
            GridOper.ExportToExcel(myGridView1, "客户计价标准");
        }

        private void barBtnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barBtnImport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            fmDenominatedFeeUpNew up = new fmDenominatedFeeUpNew();
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

    }
}
