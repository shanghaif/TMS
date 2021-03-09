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
using DevExpress.XtraGrid.Columns;
using System.Data.OleDb;

namespace ZQTMS.UI
{
    public partial class frmOperationAgingUP : BaseForm
    {
        public frmOperationAgingUP()
        {
            InitializeComponent();
        }

        private void myGridControl1_Click(object sender, EventArgs e)
        {

        }

        private void frmOperationAgingUP_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar2); //如果有具体的工具条，就引用其实例 
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "选择省际干线运行时效文件";
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

        private void SetColumnName(DataColumnCollection c)
        {
            try
            {
                foreach (DataColumn dc in c)
                {
                    dc.ColumnName = dc.ColumnName.Trim();
                }
                c["始发网点"].ColumnName = "startweb";
                c["目的网点"].ColumnName = "endweb";
                c["标准运行时间"].ColumnName = "runtime";
                c["备注"].ColumnName = "remarks";
                c["始发站点"].ColumnName = "StartSite";
                c["目的站点"].ColumnName = "EndSite";
                c["标准发车时间"].ColumnName = "StandardDepartureTime";
                c["班次"].ColumnName = "Shift";
                c["车型"].ColumnName = "Models";
                c["平板标准运行时间"].ColumnName = "FlatStandardTime";
                c["平板标准到达时间"].ColumnName = "FlatStandardArrivalTime";
                c["标准到达时间"].ColumnName = "StandardArrivalTime";
                c["公里数"].ColumnName = "Kilometre";

            }
            catch (Exception ex)
            {
                //MsgBox.ShowException(ex);
                MsgBox.ShowError("请检查自己导入的文档是否为标准格式！");
            }

        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.RowCount == 0)
            {
                return;
            }
            DataTable dt = ((System.Data.DataView)(myGridView1.DataSource)).Table;           
            dt.Columns.Remove("序号");
            dt.AcceptChanges();
            string msg = "";
            dt.Columns["startweb"].SetOrdinal(0);
            dt.Columns["endweb"].SetOrdinal(1);
            dt.Columns["runtime"].SetOrdinal(2);
            dt.Columns["remarks"].SetOrdinal(3);
            dt.Columns["StartSite"].SetOrdinal(4);
            dt.Columns["EndSite"].SetOrdinal(5);
            dt.Columns["StandardDepartureTime"].SetOrdinal(6);
            dt.Columns["Shift"].SetOrdinal(7);
            dt.Columns["Models"].SetOrdinal(8);
            dt.Columns["FlatStandardTime"].SetOrdinal(9);
            dt.Columns["FlatStandardArrivalTime"].SetOrdinal(10);
            dt.Columns["StandardArrivalTime"].SetOrdinal(11);
            dt.Columns["Kilometre"].SetOrdinal(12);
            if (CommonClass.BasUpload(dt, "USP_ADD_GX_YXSJB_UPLOAD", out msg))
            {

                MsgBox.ShowOK(msg);
                this.Close();
            }
            else
            {
                MsgBox.ShowError(msg);
            }
        }
    }
}