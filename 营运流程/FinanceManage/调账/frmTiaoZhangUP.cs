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
    public partial class frmTiaoZhangUP : BaseForm
    {
        public frmTiaoZhangUP()
        {
            InitializeComponent();

        }
　


        private void barBtnFilter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }


        private void barBtnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1,"应收账户档案");
        }

        private void barBtnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void fmDirectSendFee_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar1); //如果有具体的工具条，就引用其实例  
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "客户调账文件";
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
                if (ds.Tables[0] == null)
                    return;
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
            if (myGridView1.RowCount == 0)
            {
                return;
            }

            DataTable dt = ((System.Data.DataView)(myGridView1.DataSource)).Table;
            //检测数据是否有错
            for (int i = 0; i < dt.Rows.Count; i++) 
            {
                if (ConvertType.ToString(dt.Rows[i]["InOrOut"]) != "支出" && ConvertType.ToString(dt.Rows[i]["InOrOut"]) != "收入")
                {
                    MsgBox.ShowOK("请检查收支类型！");
                    return;
                }
                if (ConvertType.ToString(dt.Rows[i]["InOrOut"]) == "收入")
                {
                    if (ConvertType.ToString(dt.Rows[i]["Project"]) != "扣返其他费")
                    {
                        MsgBox.ShowOK("请检查收入的汇总项目！");
                        return;
                    }
                }
                if (ConvertType.ToString(dt.Rows[i]["InOrOut"]) == "支出")
                {
                    if (ConvertType.ToString(dt.Rows[i]["Project"]) != "扣返其他费" && ConvertType.ToString(dt.Rows[i]["Project"]) != "异动款项")
                    {
                        MsgBox.ShowOK("请检查支出的汇总项目！");
                        return;
                    }
                }
                if (ConvertType.ToString(dt.Rows[i]["InOrOut"]) == "收入" && ConvertType.ToString(dt.Rows[i]["Project"]) == "扣返其他费")
                {
                    int count = 0;
                    string[] sbList = CommonClass.Arg.TotalOtherAcc.Split(',');
                    if (sbList.Length > 0)
                    {
                        for (int j = 0; j < sbList.Length; j++)
                         {
                             if ((ConvertType.ToString(dt.Rows[i]["FeeType"]) == sbList[j]))
                             {
                              count++;
                             }
                         }
                    }
                    if (count == 0)
                    {
                        MsgBox.ShowOK("请检查收入的费用类型！");
                        return;
                    }
                }

                if (ConvertType.ToString(dt.Rows[i]["InOrOut"]) == "支出" && ConvertType.ToString(dt.Rows[i]["Project"]) == "扣返其他费")
                {

                    int count = 0;
                    string[] sbList = CommonClass.Arg.TotalOtherAccOut.Split(',');
                    if (sbList.Length > 0)
                    {
                        for (int j = 0; j < sbList.Length; j++)
                         {
                             if ((ConvertType.ToString(dt.Rows[i]["FeeType"]) == sbList[j]))
                             {
                              count++;
                             }
                         }
                    }
                    if (count == 0)
                    {
                        MsgBox.ShowOK("请检查支出的费用类型！");
                        return;
                    }
                }
                if (ConvertType.ToString(dt.Rows[i]["ToMan"]) =="")
                {
                    MsgBox.ShowOK("请检查调账账户名称！");
                    return;
                }
                if (ConvertType.ToString(dt.Rows[i]["ToMan"]) != "")
                {
                    int count = 0;
                    for (int j = 0; j < CommonClass.dsWeb.Tables[0].Rows.Count;j++ )
                    {
                        if (ConvertType.ToString(CommonClass.dsWeb.Tables[0].Rows[j]["WebName"]) == ConvertType.ToString(dt.Rows[i]["ToMan"]))
                        {
                            count++;
                        }
                    }
                    if (count == 0 )
                    {
                        MsgBox.ShowOK("请检查调账账户名称！");
                        return;
                    }
                }
                if (StringHelper.IsDecimal(ConvertType.ToString(dt.Rows[i]["Account"])) == false || ConvertType.ToDecimal(dt.Rows[i]["Account"]) < 0)
                {
                    MsgBox.ShowOK("请检查调账金额！");
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", ConvertType.ToString(dt.Rows[i]["BillNo"])));
                DataSet dtBill= SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "P_QSP_GET_Tiaozhang_CHKBillNo", list));
                if (dtBill == null || Convert.ToInt32(dtBill.Tables[0].Rows[0]["NUM"]) == 0 && checkBox1.Checked == true && ConvertType.ToString(dt.Rows[i]["BillNo"]) != "")
                {
                    MsgBox.ShowOK("请检查单号是否正确！");
                    return;
                }
            }
            dt.Columns.Remove("序号");
            dt.AcceptChanges();
            DataTable newDt = new DataTable();
            newDt.Columns.Add("InOrOut", typeof(string));
            newDt.Columns.Add("Project", typeof(string));
            newDt.Columns.Add("FeeType", typeof(string));
            newDt.Columns.Add("Account", typeof(string));
            newDt.Columns.Add("ToMan", typeof(string));
            newDt.Columns.Add("Remark", typeof(string));
            newDt.Columns.Add("BillNo", typeof(string));
            for (int i = 0; i < dt.Rows.Count;i++)
            {
                DataRow dr = newDt.NewRow();
                dr["InOrOut"] = ConvertType.ToString(dt.Rows[i]["InOrOut"]);
                dr["Project"] = ConvertType.ToString(dt.Rows[i]["Project"]);
                dr["FeeType"] = ConvertType.ToString(dt.Rows[i]["FeeType"]);
                dr["Account"] = ConvertType.ToString(dt.Rows[i]["Account"]);
                dr["ToMan"] = ConvertType.ToString(dt.Rows[i]["ToMan"]);
                dr["Remark"] = ConvertType.ToString(dt.Rows[i]["Remark"]);
                dr["BillNo"] = ConvertType.ToString(dt.Rows[i]["BillNo"]);
                newDt.Rows.Add(dr);
            }
            string msg = "";
            if (CommonClass.BasUpload(newDt, "USP_ADD_TIAOZHANG_UPLOAD", out msg))
            {

                MsgBox.ShowOK(msg);
                this.Close();
            }
            else
            {
                MsgBox.ShowError(msg);
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
                c["收支类型"].ColumnName = "InOrOut";
                c["汇总项目"].ColumnName = "Project";
                c["费用类型"].ColumnName = "FeeType";
                c["金额"].ColumnName = "Account";  //maohui20180531
                c["调账账户名称"].ColumnName = "ToMan";
                c["摘要"].ColumnName = "Remark";
                c["运单号"].ColumnName = "BillNo";
               

            }
            catch (Exception ex)
            {
                //MsgBox.ShowException(ex);
                MsgBox.ShowError("请检查自己导入的文档是否为标准格式！");
            }

        }

    }
}
