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
using System.Data.OleDb;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;

namespace ZQTMS.UI
{
    public partial class frmFreightFee : BaseForm
    {
        public frmFreightFee()
        {
            InitializeComponent();

        }
        GridColumn gcIsseleckedMode;
        private void LoadData()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASFREIGHTFEE", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            gcIsseleckedMode = GridOper.GetGridViewColumn(myGridView1, "ischecked"); // luohui
        }


        private void barBtnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmFreightFeeAdd frm = new frmFreightFeeAdd();
            frm.ShowDialog();
        }

        private void barBtnUpdate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "FreightId").ToString());

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("FreightId", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASFREIGHTFEE_ByID", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                DataRow dr = ds.Tables[0].Rows[0];

                frmFreightFeeAdd frm = new frmFreightFeeAdd();
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
            //try
            //{
            //    int rowhandle = myGridView1.FocusedRowHandle;
            //    if (rowhandle < 0) return;
            //    Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "FreightId").ToString());

            //    if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
            //    {
            //        return;
            //    }
            //    List<SqlPara> list = new List<SqlPara>();
            //    list.Add(new SqlPara("FreightId", id));

            //    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_BASFREIGHTFEE", list);
            //    if (SqlHelper.ExecteNonQuery(sps) > 0)
            //    {
            //        MsgBox.ShowOK();

            //        myGridView1.DeleteRow(rowhandle);
            //        myGridView1.PostEditor();
            //        myGridView1.UpdateCurrentRow();
            //        myGridView1.UpdateSummary();
            //        DataTable dt = myGridControl1.DataSource as DataTable;
            //        dt.AcceptChanges();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MsgBox.ShowException(ex);
            //}


            //批量删除 luohui
            try
            {
                if (myGridView1.RowCount == 0) return;

                myGridView1.PostEditor();

                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                string FreightId = "";
                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    if (ConvertType.ToString(myGridView1.GetRowCellValue(i, "ischecked")) == "1")
                    {
                        FreightId += myGridView1.GetRowCellValue(i, "FreightId") + "@";//单号
                    }
                }
                if (FreightId == "") return;

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("FreightId", FreightId));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_BASFREIGHTFEE", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    LoadData();
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
            GridOper.ExportToExcel(myGridView1,"对外最低价格标准");
        }

        private void barBtnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void fmDirectSendFee_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("标准价格");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar1); //如果有具体的工具条，就引用其实例 
            FixColumn fix = new FixColumn(myGridView1, barSubItem3);
            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
            LoadData();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "选择最低价格标准文件";
            ofd.Filter = "Microsoft Execl文件|*.xls;*.xlsx";
            ofd.FilterIndex = 1;
            ofd.DefaultExt = "xls";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (!ofd.SafeFileName.EndsWith(".xls") && !ofd.SafeFileName.EndsWith(".xlsx"))
                {
                    XtraMessageBox.Show("请选择Excel文件!", "文件导入失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!ofd.CheckFileExists)
                {
                    XtraMessageBox.Show("文件不存在，请重新选择!", "文件导入失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DataSet ds = DsExecl(ofd.FileName);
                int i = 1;
                foreach (DataColumn columns in ds.Tables[0].Columns)
                {
                    if (columns.ColumnName.IndexOf('-') > 0)
                    {
                        GridColumn column = new GridColumn();
                        column.Caption = columns.ColumnName;
                        column.FieldName = columns.ColumnName;
                        column.Name = "gridColumnDt" + i;
                        column.Visible = true;
                        column.VisibleIndex = 6 + i;
                        column.Width = 80;
                        myGridView1.Columns.Add(column);
                        i++;
                    }
                }
                if (ds != null)
                {
                    myGridControl1.DataSource = ds.Tables[0];

                }
            }
        }

        private DataSet DsExecl(string filePath)
        {
            //string str = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + filePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\""; //此连接可以兼容2003和2007
            //string str = "Provider = Microsoft.Jet.OLEDB.12.0; Data Source = " + filePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\""; //此连接必须要安装2007
            string str = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + filePath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\""; //此连接智能读取2003格式
            OleDbConnection Conn = new OleDbConnection(str);
            try
            {
                Conn.Open();
                System.Data.DataTable dt = Conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                string tablename = "", sql = "";
                w_import_select_table wi = new w_import_select_table();
                wi.dt = dt;
                if (wi.ShowDialog() != DialogResult.Yes)
                { return null; }
                tablename = wi.listBoxControl1.Text.Trim();
                sql = "select * from [" + tablename + "]";

                OleDbDataAdapter da = new OleDbDataAdapter(sql, Conn);
                DataSet ds = new DataSet();
                da.Fill(ds, tablename);

                try
                {
                    SetColumnName(ds.Tables[0].Columns);
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex, "转换失败!\r\n请检查EXCEL列头是否与模板一致！");
                    return null;
                }
                return ds;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "加载结算送货费失败");
                return null;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open) Conn.Close();
            }
        }
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmFreightFeeUP frm = new frmFreightFeeUP();
            frm.ShowDialog();
        }
        private void SetColumnName(DataColumnCollection c)
        {
            try
            {
                foreach (DataColumn dc in c)
                {
                    dc.ColumnName = dc.ColumnName.Trim();
                }
                c["始发站"].ColumnName = "StartSite";
                c["省"].ColumnName = "Province";
                c["市"].ColumnName = "City";
                c["区县"].ColumnName = "Area";
                c["中转站"].ColumnName = "TransferSite";
                c["最低一票"].ColumnName = "ParcelPriceMin";
                c["重货单价"].ColumnName = "HeavyPrice";
                c["轻货单价"].ColumnName = "LightPrice";
                c["运输方式"].ColumnName = "TransitMode";
                c["时效"].ColumnName = "Prescription";
                c["备注"].ColumnName = "Remark";
            }
            catch (Exception ex)
            {
                //MsgBox.ShowException(ex);
                MsgBox.ShowError("请检查自己导入的文档是否为标准格式！");
            }

        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "FreightId").ToString());

                if (MsgBox.ShowYesNo("确认启用？") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("FreightId", id));
                list.Add(new SqlPara("FreightStatus", "启用"));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_MODIFIED_FreightStatus", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    myGridView1.SetRowCellValue(rowhandle, "FreightStatus", "启用");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "FreightId").ToString());

                if (MsgBox.ShowYesNo("确认禁用？") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("FreightId", id));
                list.Add(new SqlPara("FreightStatus", "禁用"));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_MODIFIED_FreightStatus", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    myGridView1.SetRowCellValue(rowhandle, "FreightStatus", "禁用");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem14_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        //luohui全选
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
