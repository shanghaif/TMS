using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Common;
using ZQTMS.Tool;
using DevExpress.XtraEditors;
using System.Data.OleDb;
using ZQTMS.SqlDAL;
using DevExpress.XtraGrid.Columns;

namespace ZQTMS.UI
{
    public partial class frmRecCustDataImport : BaseForm
    {
        public frmRecCustDataImport()
        {
            InitializeComponent();
        }


        private void frmCarrierUnitImport_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            GridOper.SetGridViewProperty(myGridView1);
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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
            string[] oldnames = { "隶属发货人", "隶属站点", "所属区域", "收货单位", "联系人", "联系电话", "手机号码", "收货地址省份", "收货地址城市", "收货地址区/县", "收货地址乡镇/街道", "收货详细地址", "到站", "中转站", "交接方式", "目的网点","合作日期","所属网点"};
            string[] newnames = { "CusID", "BelongSite", "BelongArea", "RCName", "ContactMan", "ContactPhone", "ContactCellPhone", "RecievAddProv", "RecievAddCity", "RecievAddArea", "RecievAddStreet", "RecievAddress", "ArriveSite", "MidSite", "TransferMode", "DestinationWeb", "CooperateDate",  "BelongWeb"};
            try
            {
                for (int i = 0; i < oldnames.Length; i++)
                {
                  c[oldnames[i]].ColumnName=  newnames[i];
                }
            }
            catch(Exception ex)
            {
                MsgBox.ShowError("请检查自己导入的文档是否为标准格式！");
            }
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataTable dt = myGridControl1.DataSource as DataTable;
            if (dt.Columns.Contains("序号"))
            {
                dt.Columns.Remove("序号");
                dt.AcceptChanges();
            }

            if (!checkData(dt)) // 数据检测
            {
                return;
            }
            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_BasReceiveCust_IMPORT", new List<SqlPara>() { new SqlPara("Tb", dt) })) > 0)
            {
                MsgBox.ShowOK("上传成功！");
                this.Close();
            }
            else
            {
                MsgBox.ShowError("上传失败！请先上传数据！");
            }
        }


        private Boolean checkData(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                if (dt.Rows[i]["BelongSite"] == null || dt.Rows[i]["BelongSite"].ToString() == "")
                {
                    MsgBox.ShowOK("第" + (i + 1)+ "行" + "所属站点不能为空,请检查！");
                    return false;
                }

                //if (dt.Rows[i]["ModifyDate"] == null || dt.Rows[i]["ModifyDate"].ToString() == "")
                //{
                //    MsgBox.ShowOK("第" + (i + 1) + "行" + "修改日期不能为空,请检查！");
                //    return false;
                //}
            }
            return true;
        }

    }
}