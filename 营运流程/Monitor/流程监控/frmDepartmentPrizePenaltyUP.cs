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
using System.IO;
using System.Data.OleDb;
using DevExpress.XtraGrid.Columns;

namespace ZQTMS.UI
{
    public partial class frmDepartmentPrizePenaltyUP : BaseForm
    {
        public frmDepartmentPrizePenaltyUP()
        {
            InitializeComponent();
        }

        private void frmDepartmentPrizePenaltyUP_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3);
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "选择代收代扣款文件";
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

            }
            //DataTable dt = NpoiOperExcel.ExcelToDataTable(ofd.FileName, false);
            //SetColumnName(dt.Columns);
            //myGridControl1.DataSource = dt;
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

            myGridControl1.DataSource = ds.Tables[0];
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
            catch(Exception ex)
            {
                MsgBox.ShowException(ex, "加载部门奖罚数据失败！");
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

                c["所属中心"].ColumnName = "BelongCenter";
                c["登记部门"].ColumnName = "DJWeb";
                c["登记时间"].ColumnName = "DJTime";
                c["月份"].ColumnName = "TheMonth";
                c["运单号"].ColumnName = "Billno";
                c["奖罚类型"].ColumnName = "Type";
                c["责任部门"].ColumnName = "ResponsibilityWeb";
                c["责任人"].ColumnName = "ResponsibilityMan";
                c["金额"].ColumnName = "Money";
                c["摘要"].ColumnName = "Abstract";
                c["登记人"].ColumnName = "RegisterMan";
                
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
            if (dt.Columns.Contains("序号"))
            {
                dt.Columns.Remove("序号");
                dt.AcceptChanges();
            }
            try
            {
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_RewardData_UPLOAD", new List<SqlPara>() { new SqlPara("Tb", dt) })) == 0) return;
                MsgBox.ShowOK("上传成功！");
                myGridControl1.DataSource = null;

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

         
        }
    }
}