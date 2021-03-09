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
    public partial class frmCarrierUnitImport : BaseForm
    {
        public frmCarrierUnitImport()
        {
            InitializeComponent();
        }

        private void myGridControl1_Click(object sender, EventArgs e)
        {

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
            string[] oldnames = { "公司名称", "单位所属站点", "隶属分拨中心", "最低一票", "重货单价", "轻货单价", "送货费", "到站联系人", "到站电话", "到站手机", "到站地址", "业务员", "业务电话", "业务手机", "信誉等级", "是否签订合同", "公司法人", "收取押金" };
            string[] newnames = { "CompanyName", "CUSite", "CUWeb", "MinimumBill", "HeavyPrice", "LightPrice", "DeliFee", "AriiveMan", "ArrivePhone", "ArriveCellPhone", "ArriveAddress", "Salesman", "SalesPhone", "SalesCellPhone", "CreditLevel", "IsSigned", "LegalPerson",  "Deposit" };
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
            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_CarrierUnit_IMPORT", new List<SqlPara>() { new SqlPara("Tb", dt)})) > 0)
            {
                MsgBox.ShowOK("上传成功！");
                this.Close();
            }
            else
            {
                MsgBox.ShowError("上传失败！请先上传数据！");
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            
        }

        private Boolean checkData(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                if (dt.Rows[i]["CompanyName"] == null || dt.Rows[i]["CompanyName"].ToString() == "")
                {
                    MsgBox.ShowOK("第" + (i + 1) + "公司名称不能为空,请检查！");
                    return false;
                }

                if (dt.Rows[i]["CUSite"] == null || dt.Rows[i]["CUSite"].ToString() == "")
                {
                    MsgBox.ShowOK("第" + (i + 1) + "单位所属站点不能为空,请检查！");
                    return false;
                }

                if (dt.Rows[i]["CUWeb"] == null || dt.Rows[i]["CUWeb"].ToString() == "")
                {
                    MsgBox.ShowOK("第" + (i + 1) + "隶属分拨中心不能为空,请检查！");
                    return false ;
                }
               
                if (dt.Rows[i]["MinimumBill"] == null || dt.Rows[i]["MinimumBill"].ToString() == "" )
                {
                    MsgBox.ShowOK("第" + (i + 1) + "最低一票不能为空,请检查！");
                    return false;
                }
              
                if (dt.Rows[i]["HeavyPrice"] == null || dt.Rows[i]["HeavyPrice"].ToString() == "" )
                {
                    MsgBox.ShowOK("第" + (i + 1) + "重货单价不能为空,请检查！");
                    return false;
                }

                if (dt.Rows[i]["LightPrice"] == null || dt.Rows[i]["LightPrice"].ToString() == "")
                {
                    MsgBox.ShowOK("第" + (i + 1) + "轻货单价不能为空,请检查！");
                    return false;
                }
              
                if (dt.Rows[i]["DeliFee"] == null || dt.Rows[i]["DeliFee"].ToString() == ""   )
                {
                    MsgBox.ShowOK("第" + (i + 1) + "送货费不能为空,请检查！");
                    return false;
                }
               
                if (dt.Rows[i]["AriiveMan"] == null || dt.Rows[i]["AriiveMan"].ToString() == ""  )
                {
                    MsgBox.ShowOK("第" + (i + 1) + "到站联系人不能为空,请检查！");
                    return false;
                }
               
                if (dt.Rows[i]["ArrivePhone"] == null || dt.Rows[i]["ArrivePhone"].ToString() == ""  )
                {
                    MsgBox.ShowOK("第" + (i + 1) + "到站电话不能为空,请检查！");
                    return false;
                }
              
                if (dt.Rows[i]["ArriveCellPhone"] == null || dt.Rows[i]["ArriveCellPhone"].ToString() == ""  )
                {
                    MsgBox.ShowOK("第" + (i + 1) + "到站手机不能为空,请检查！");
                    return false;
                }
              
                if (dt.Rows[i]["ArriveAddress"] == null || dt.Rows[i]["ArriveAddress"].ToString() == "")
                {
                    MsgBox.ShowOK("第" + (i + 1) + "到站地址不能为空,请检查！");
                    return false;
                }
             
                if (dt.Rows[i]["Salesman"] == null || dt.Rows[i]["Salesman"].ToString() == "" )
                {
                    MsgBox.ShowOK("第" + (i + 1) + "业务员不能为空,请检查！");
                    return false;
                }
            
                if (dt.Rows[i]["SalesPhone"] == null || dt.Rows[i]["SalesPhone"].ToString() == "" )
                {
                    MsgBox.ShowOK("第" + (i + 1) + "业务电话不能为空,请检查！");
                    return false;
                }
      
                if (dt.Rows[i]["SalesCellPhone"] == null || dt.Rows[i]["SalesCellPhone"].ToString() == ""  )
                {
                    MsgBox.ShowOK("第" + (i + 1) + "业务手机不能为空,请检查！");
                    return false;
                }
              
                if (dt.Rows[i]["CreditLevel"] == null || dt.Rows[i]["CreditLevel"].ToString() == ""  )
                {
                    MsgBox.ShowOK("第" + (i + 1) + "信誉等级不能为空,请检查！");
                    return false;
                }
              
                if (dt.Rows[i]["IsSigned"] == null || dt.Rows[i]["IsSigned"].ToString() == ""  )
                {
                    MsgBox.ShowOK("第" + (i + 1) + "是否签订合同不能为空,请检查！");
                    return false;
                }
            }
            return true;
        }

    }
}