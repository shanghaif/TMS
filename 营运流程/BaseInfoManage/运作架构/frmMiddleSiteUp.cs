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

namespace ZQTMS.UI
{
    public partial class frmMiddleSiteUp : BaseForm
    {
        public frmMiddleSiteUp()
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
            //if (gridView1.RowCount == 0) return;
            //List<SqlPara> list = CommonClass.GetParaList(gridView1);
            //if (list == null || list.Count == 0) return;
            //try
            //{
            //    if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_basMiddleSite_UPLOAD", list)) == 0) return;
            //    MsgBox.ShowOK("上传成功!");
            //    gridControl1.DataSource = null;
            //}
            //catch (Exception ex)
            //{
            //    MsgBox.ShowOK(ex.Message);
            //}
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount == 0) return;
            //List<SqlPara> list = CommonClass.GetParaList(gridView1);
            //if (list == null || list.Count == 0) return;
            DataTable dt = ((System.Data.DataView)(gridView1.DataSource)).Table;
            //list.Add(new SqlPara("companyid1", CompanyID.Text.Trim()));
            if (MsgBox.ShowYesNo("确定要上传到这个公司吗？") != DialogResult.Yes) return;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("Tb", dt));
                list.Add(new SqlPara("companyid1", CompanyID.Text.Trim()));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_basMiddleSite_UPLOAD_ALL", list)) == 0) return;
                MsgBox.ShowOK("上传成功!");
                panel1.Visible = false;
                gridControl1.DataSource = null;
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
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
            ofd.Title = "选择结算始发操作费文件";
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
                MsgBox.ShowException(ex, "加载结算始发操作费失败");
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
                c["隶属站点"].ColumnName = "SiteName";
                c["目的地"].ColumnName = "Destination";
                c["所在省份"].ColumnName = "MiddleProvince";
                c["所在城市"].ColumnName = "MiddleCity";
                c["所在区县"].ColumnName = "MiddleArea";
                c["所在街道"].ColumnName = "MiddleStreet";
                c["网点名称"].ColumnName = "WebName";
                c["状态"].ColumnName = "MiddleStatus";
                c["服务类型"].ColumnName = "Type";
                c["经度"].ColumnName = "MiddleLon";
                c["纬度"].ColumnName = "MiddleLat";
                c["自提库位"].ColumnName = "FetchStorageLoca";
                c["送货库位"].ColumnName = "SendStorageLoca";
                c["归属地"].ColumnName = "Ascription";
                c["省份库位"].ColumnName = "ShengStore";
                c["区县库位"].ColumnName = "AreaStore";
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void fmDepartureOptFeeUp_Load(object sender, EventArgs e)
        {
            BarMagagerOper.SetBarPropertity(bar1); //如果有具体的工具条，就引用其实例
            GetCompanyId();
        }

        private void barBtnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(gridView1, "二级中转城市数据");
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
        }

        private void barBtnDown_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string file = "\\结算始发操作费模板.xls";
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