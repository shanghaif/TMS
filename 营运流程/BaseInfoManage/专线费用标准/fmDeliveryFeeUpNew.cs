using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using DevExpress.XtraEditors;
using System.Threading;
using System.Data.SqlClient;
using System.IO;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using DevExpress.XtraGrid.Columns;

namespace ZQTMS.UI
{
    public partial class fmDeliveryFeeUpNew : BaseForm
    {
        public fmDeliveryFeeUpNew()
        {
            InitializeComponent();
        }

        private void barBtnImport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            InportExcel();
        }

        private void barBtnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barBtnUpload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            panel1.Visible = true;
            //List<SqlPara> list = CommonClass.GetParaList(gridView1);
            //if (list.Count == 0) return;

            //if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_BASDELIVERYFEE_UPLOAD_GX", list)) == 0) return;
            //MsgBox.ShowOK("上传成功!");
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            DataTable dt = ((System.Data.DataView)(gridView1.DataSource)).Table;
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("companyid1", CompanyID.Text.Trim()));
            list.Add(new SqlPara("Tb", dt));

            if (MsgBox.ShowYesNo("确定要上传到这个公司吗？") != DialogResult.Yes) return;

            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_BASDELIVERYFEE_UPLOAD_GX", list)) == 0) return;
            MsgBox.ShowOK("上传成功!");
            panel1.Visible = false;
        }

        private void ShowInformation(string msg)
        {
            XtraMessageBox.Show(msg, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool ShowQuestion()
        {
            return DialogResult.Yes == XtraMessageBox.Show("停止上传？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        private void InportExcel()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "选择结算送货费文件";
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
                        gridView1.Columns.Add(column);
                        i++;
                    }
                }
                if (ds != null)
                {
                    gridControl1.DataSource = ds.Tables[0];
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
                c["省份"].ColumnName = "Province";
                c["城市"].ColumnName = "City";
                c["区县"].ColumnName = "Area";
                c["乡镇街道"].ColumnName = "Street";
                c["交接方式"].ColumnName = "TransferMode";
                c["中心公里数"].ColumnName = "CenterKilometres";
                c["所属站点"].ColumnName = "sitename";
                c["300以下"].ColumnName = "w0_300";
                c["300-1000"].ColumnName = "w300_1000";
                c["1000-3000"].ColumnName = "w1000_3000";
                c["3000-5000"].ColumnName = "w3000_5000";
                c["5000-8000"].ColumnName = "w5000_8000";
                c["8000以上"].ColumnName = "w8000_";
                c["备注"].ColumnName = "Remark";
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void fmDeliveryFeeUp_Load(object sender, EventArgs e)
        {
            GetCompanyId();
            BarMagagerOper.SetBarPropertity(bar1); //如果有具体的工具条，就引用其实例
        }

        private void barBtnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(gridView1, "送货费，中转费标准");
        }

        private void barBtnDown_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string file = "\\结算送货费模板.xls";
                FolderBrowserDialog sfd = new FolderBrowserDialog();
                sfd.Description = "另存为";
                if (sfd.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }
                File.Copy(System.Windows.Forms.Application.StartupPath + file, sfd.SelectedPath + file, true);
                XtraMessageBox.Show("下载成功!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message + "\r保存失败，请重新下载!如果再次失败请关闭程序，再次下载!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridView1_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            //if (e != null && e.Column.FieldName == "yf_type")
            //{
            //    if ((e.Value + "").Trim() == "1")
            //        e.DisplayText = "快件";
            //    else
            //        e.DisplayText = "普件";
            //}
        }

        private void tsmiDeleteRow_Click(object sender, EventArgs e)
        {
            int rowHandle = gridView1.FocusedRowHandle;
            if (rowHandle < 0) return;
            if (XtraMessageBox.Show("确定删除该行？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel) return;
            gridView1.DeleteRow(rowHandle);
        }

        public void GetCompanyId()
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_USP_GET_companyid");
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    CompanyID.Properties.Items.Add(dr[0]);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }

    
    }
}