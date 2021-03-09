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
using DevExpress.XtraEditors;
using System.Data.OleDb;

namespace ZQTMS.UI
{
    public partial class fmHKFeeUp : BaseForm
    {
        public fmHKFeeUp()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SendExpression", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl2.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barBtnFilter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridView1.OptionsView.ShowAutoFilterRow = !gridView1.OptionsView.ShowAutoFilterRow;
        }

        private void barBtnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadData();
        }

        private void barBtnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(gridView1, "结算始发操作费");
        }

        private void barBtnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barBtnImport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            InportExcel();
        }

        private void InportExcel()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "选择结算干线费文件";
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
                gridControl1.DataSource = ds.Tables[0];
                //if (ds != null)
                //{
                //    ds.Tables[0].Columns.Add("ToProvince");
                //    ds.Tables[0].Columns.Add("ToCity");
                //    ds.Tables[0].Columns.Add("ToArea");
                //    gridControl1.DataSource = ds.Tables[0];
                //}
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
                MsgBox.ShowException(ex, "加载结算干线费失败");
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
                c["公里数"].ColumnName = "kilometre";
                c["附加费"].ColumnName = "Additional";
                c["备注"].ColumnName = "Remark";
                c["报价名称"].ColumnName = "ExpressionName";
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ExpressionId", ExpressionId.Text));
                list.Add(new SqlPara("w1", ConvertType.ToDecimal(w1.Text)));
                list.Add(new SqlPara("w2", ConvertType.ToDecimal(w2.Text)));
                list.Add(new SqlPara("Expression", Expression.Text));
                list.Add(new SqlPara("ExpressionName", ExpressionName.Text));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_SEND_Expression", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    clear();
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void fmHKFeeUp_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView2);
            GridOper.SetGridViewProperty(myGridView2);
            BarMagagerOper.SetBarPropertity(bar1); //如果有具体的工具条，就引用其实例  
            LoadData();
            clear();
        }

        private void clear()
        {
            ExpressionId.Text = Guid.NewGuid().ToString();
            w1.Text = "";
            w2.Text = "";
            Expression.Text = "";
            ExpressionName.Text = "";
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            try
            {
                int rowhandle = myGridView2.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView2.GetRowCellValue(rowhandle, "ExpressionId").ToString());

                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ExpressionId", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_SEND_Expression", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    myGridView2.DeleteRow(rowhandle);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Expression.Text = Expression.Text + simpleButton1.Text;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Expression.Text = Expression.Text + simpleButton2.Text;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Expression.Text = Expression.Text + simpleButton3.Text;
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            Expression.Text = Expression.Text + simpleButton4.Text;
        }

        private void myGridView2_Click(object sender, EventArgs e)
        {
            try
            {
                int rowhandle = myGridView2.FocusedRowHandle;
                if (rowhandle < 0) return;
                ExpressionId.Text = myGridView2.GetRowCellValue(rowhandle, "ExpressionId").ToString();
                w1.Text = myGridView2.GetRowCellValue(rowhandle, "w1").ToString();
                w2.Text = myGridView2.GetRowCellValue(rowhandle, "w2").ToString();
                Expression.Text = myGridView2.GetRowCellValue(rowhandle, "Expression").ToString();
                ExpressionName.Text = myGridView2.GetRowCellValue(rowhandle, "ExpressionName").ToString();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            List<SqlPara> list = CommonClass.GetParaList(gridView1);
            if (list.Count == 0) return;

            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_basDeliveryFeeHK", list)) == 0) return;
            MsgBox.ShowOK("上传成功!");
        }
    }
}