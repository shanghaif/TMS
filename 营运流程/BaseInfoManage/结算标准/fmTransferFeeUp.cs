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
    public partial class fmTransferFeeUp : BaseForm
    {
        public fmTransferFeeUp()
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
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //List<SqlPara> list = CommonClass.GetParaList(gridView1);
            //if (list.Count == 0) return;
            //list.Add(new SqlPara("companyid1", CompanyID.Text.Trim()));

            //if (MsgBox.ShowYesNo("确定要上传到这个公司吗？") != DialogResult.Yes) return;
            //if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_BASTRANSFERFEE_UPLOAD", list)) == 0) return;
            //MsgBox.ShowOK("上传成功!");
            if (gridView1.RowCount == 0) return;
            DataTable dt = ((System.Data.DataView)(gridView1.DataSource)).Table;
            //ZJF20181126
            dt.Columns["FromSite"].SetOrdinal(0);
            dt.Columns["TransferSite"].SetOrdinal(1);
            dt.Columns["ToProvince"].SetOrdinal(2);
            dt.Columns["ToCity"].SetOrdinal(3);
            dt.Columns["ToArea"].SetOrdinal(4);
            dt.Columns["HeavyPrice"].SetOrdinal(5);
            dt.Columns["LightPrice"].SetOrdinal(6);
            dt.Columns["ParcelPriceMin"].SetOrdinal(7);
            dt.Columns["Remark"].SetOrdinal(8);
            dt.Columns["TransitMode"].SetOrdinal(9);
        

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("Tb", dt));
                list.Add(new SqlPara("companyid1", CompanyID.Text.Trim()));
                if (MsgBox.ShowYesNo("确定要上传到这个公司吗？") != DialogResult.Yes) return;
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute,"USP_ADD_BASTRANSFERFEE_UPLOAD",list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK("上传成功！");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
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
            ofd.Title = "选择结算中转费文件";
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
                MsgBox.ShowException(ex, "加载结算中转费失败");
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
                c["始发站"].ColumnName = "FromSite";
                c["中转站"].ColumnName = "TransferSite";
                c["到站省份"].ColumnName = "ToProvince";
                c["到站城市"].ColumnName = "ToCity";
                c["到站区县"].ColumnName = "ToArea";
                c["重货(元/公斤)"].ColumnName = "HeavyPrice";
                c["轻货(元/方)"].ColumnName = "LightPrice";
                c["最低一票"].ColumnName = "ParcelPriceMin";
                c["备注"].ColumnName = "Remark";
                c["运输方式"].ColumnName = "TransitMode";
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void fmTransferFeeUp_Load(object sender, EventArgs e)
        {
            GetCompanyId();
            BarMagagerOper.SetBarPropertity(bar1); //如果有具体的工具条，就引用其实例
        }

        private void barBtnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(gridView1, "结算中转费");
        }

        private void barBtnDown_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string file = "\\结算中转费模板.xls";
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