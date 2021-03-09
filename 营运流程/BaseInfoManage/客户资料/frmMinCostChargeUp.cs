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

namespace ZQTMS.UI.BaseInfoManage.客户资料
{
    public partial class frmMinCostChargeUp : ZQTMS.Tool.BaseForm
    {
        public frmMinCostChargeUp()
        {
            InitializeComponent();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //导入
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "选择标准文件";
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

                DataTable dt = NpoiOperExcel.ExcelToDataTable(ofd.FileName, false);
                int i = 1;
                foreach (DataColumn columns in dt.Columns)
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
                        myGridView2.Columns.Add(column);
                        i++;
                    }
                }
                if (dt != null)
                {

                    SetColumnName(dt.Columns);
                    myGridControl2.DataSource = dt;
                }
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
                c["发货单位"].ColumnName = "ConsignorCompany";
                c["发货联系人"].ColumnName = "ConsignorName";
                c["联系电话"].ColumnName = "ConsignorCellPhone";
                c["联系手机"].ColumnName = "ConsignorPhone";
                c["所属网点"].ColumnName = "BelongWeb";
                c["付款方式"].ColumnName = "PaymentMode";




            }
            catch (Exception ex)
            {
                //MsgBox.ShowException(ex);
                MsgBox.ShowError("请检查自己导入的文档是否为标准格式！");
            }

        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {

                //上传
                if (myGridView2.RowCount == 0)
                {
                    return;
                }
                DataTable dt = ((System.Data.DataView)(myGridView2.DataSource)).Table;
                if (dt.Columns.Contains("序号"))
                {
                    dt.Columns.Remove("序号");
                    dt.AcceptChanges();
                }
                if (dt.Columns.Contains("选择"))
                {
                    dt.Columns.Remove("选择");
                    dt.AcceptChanges();
                }
                //dt.Columns["ConsignorCompany"].SetOrdinal(0);
                //dt.Columns["ConsignorName"].SetOrdinal(1);
                //dt.Columns["ConsignorCellPhone"].SetOrdinal(2);
                //dt.Columns["BelongWeb"].SetOrdinal(3);
                //dt.Columns["PaymentMode"].SetOrdinal(4);
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("Tb", dt));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "QSP_UPLoad_MinCostCharge", list);
                int k = SqlHelper.ExecteNonQuery(sps);
                if (k > 0)
                {

                    MsgBox.ShowOK("批量上传成功");
                    this.Close();
                }

                else
                {
                    MsgBox.ShowError("上传失败");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.ToString());

            }
            
        }

        private void frmMinCostChargeUp_Load(object sender, EventArgs e)
        {

            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView2);
            GridOper.SetGridViewProperty(myGridView2);
            BarMagagerOper.SetBarPropertity(bar5); //如果有具体的工具条，就引用其实例 
        }
    }
}
