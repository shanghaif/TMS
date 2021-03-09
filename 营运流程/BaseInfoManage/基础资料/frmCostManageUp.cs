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
namespace ZQTMS.UI.BaseInfoManage.基础资料
{
    public partial class frmCostManageUp : ZQTMS.Tool.BaseForm
    {
        public frmCostManageUp()
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
                c["项目类型"].ColumnName = "ProjectType";
                c["始发站"].ColumnName = "StartSite";
                c["目的站"].ColumnName = "DestinationSite";
                c["送货网点"].ColumnName = "SendWebName";
                c["中转网点"].ColumnName = "MiddleWebName";
                c["目标成本率"].ColumnName = "TargetcostRate";
                c["负责人"].ColumnName = "ChargePerson";
                c["手机号"].ColumnName = "TelPhone";
                c["备注"].ColumnName = "Remark";
               



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
                dt.Columns["ProjectType"].SetOrdinal(0);
                dt.Columns["StartSite"].SetOrdinal(1);
                dt.Columns["DestinationSite"].SetOrdinal(2);
                dt.Columns["SendWebName"].SetOrdinal(3);
                dt.Columns["MiddleWebName"].SetOrdinal(4);
                dt.Columns["TargetcostRate"].SetOrdinal(5);
                dt.Columns["ChargePerson"].SetOrdinal(6);
                dt.Columns["TelPhone"].SetOrdinal(7);
                dt.Columns["Remark"].SetOrdinal(8);
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("Tb", dt));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "QSP_UPLoad_CostControls", list);
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

        private void frmCostManageUp_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView2);
            GridOper.SetGridViewProperty(myGridView2);
            BarMagagerOper.SetBarPropertity(bar5); //如果有具体的工具条，就引用其实例 
        }
    }
}
