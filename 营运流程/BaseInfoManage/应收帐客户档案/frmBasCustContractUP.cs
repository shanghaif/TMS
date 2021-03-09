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
    public partial class frmBasCustContractUP : BaseForm
    {
        public frmBasCustContractUP()
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
            ofd.Title = "应收张客户档案文件";
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
            if (myGridView1.RowCount == 0)
            {
                return;
            }
            DataTable dt = ((System.Data.DataView)(myGridView1.DataSource)).Table;
            dt.Columns.Remove("序号");
            dt.AcceptChanges();
            string msg = "";
            if (CommonClass.BasUpload(dt, "USP_ADD_BASCUSTCONTRACT_UPLOAD", out msg))
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
                c["发货单位"].ColumnName = "ShortName";
                c["客户全名"].ColumnName = "FullName";
                c["法人代表"].ColumnName = "CpyLegalPerson";
                c["注册资本"].ColumnName = "RegistCapital";
                c["注册地址"].ColumnName = "RegistAdd";
                c["经营"].ColumnName = "RunAdd";
                c["联系人"].ColumnName = "CustLinkName";
                c["联系电话"].ColumnName = "SendCustTel";
                c["联系手机"].ColumnName = "SendCustMobile";
                c["对账联系人"].ColumnName = "CheckBillLinkName";
                c["对账电话"].ColumnName = "CheckBillTel";
                c["合同编号"].ColumnName = "ContractNo";
                c["合同时间起"].ColumnName = "BeginDate";
                c["合同终止时间"].ColumnName = "EndDate";
                c["合同签订日期"].ColumnName = "ContractDate";
                c["登记日期"].ColumnName = "crDate";
                c["申请人"].ColumnName = "ApplyName";
                c["申请部门"].ColumnName = "UnitDeptName";
                c["信誉天数"].ColumnName = "CreditDays";
                c["信誉额度"].ColumnName = "CreditLimit";
                c["结款周期"].ColumnName = "PayCycle";
                c["月结延期天数"].ColumnName = "MonthlyDelayDays";
                c["月结延期额度"].ColumnName = "MonthlyDelayLimit";
                c["回单延期天数"].ColumnName = "ReturnBillDelayDays";
                c["经办人"].ColumnName = "Operator";
                c["客户类型"].ColumnName = "CustTypeValue";
                c["月结站点"].ColumnName = "MonthSiteName";
                c["月结网点"].ColumnName = "MonthWebName";

                c["对账人QQ"].ColumnName = "CheckBillLinkQQ";
                c["对账人微信"].ColumnName = "CheckBillLinkWeChat";
                c["对账人邮箱"].ColumnName = "CheckBillLinkEmail";
                c["含税"].ColumnName = "Tax";
                c["加点"].ColumnName = "Punctuate";
                c["付款联系人"].ColumnName = "Paymentcontact";
                c["付款联系电话"].ColumnName = "Paymentcontactphone";
                c["客服责任人"].ColumnName = "Serviceresponsibler";
                c["回单返回要求"].ColumnName = "Returnrequest";
                c["是否开票"].ColumnName = "Invoice";

            }
            catch (Exception ex)
            {
                //MsgBox.ShowException(ex);
                MsgBox.ShowError("请检查自己导入的文档是否为标准格式！");
            }

        }

    }
}
