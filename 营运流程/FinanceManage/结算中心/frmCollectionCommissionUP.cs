using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using System.Data.OleDb;
using DevExpress.XtraGrid.Columns;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmCollectionCommissionUP : BaseForm
    {
        public frmCollectionCommissionUP()
        {
            InitializeComponent();
        }

        private void frmCollectionCommissionUP_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar4); //如果有具体的工具条，就引用其实例 
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "代收代扣款");
        }

        //导入
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "选择代收代扣款文件";
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

            }
            DataTable dt = NpoiOperExcel.ExcelToDataTable(ofd.FileName, false);
            SetColumnName(dt.Columns);
            bool val = ValidationDate(dt.Rows);
            if (val)myGridControl1.DataSource = dt;

        }

        private void SetColumnName(DataColumnCollection c)
        {
            try
            {
                foreach (DataColumn dc in c)
                {
                    dc.ColumnName = dc.ColumnName.Trim();
                }
                //c["事业部"].ColumnName = "CauseName";
                //c["大区名称"].ColumnName = "AreaName";
                c["网点名称"].ColumnName = "WebName";
                c["登记日期"].ColumnName = "RegistrationDate";
                c["月份"].ColumnName = "month";
                c["费用类型"].ColumnName = "Revenue";
                c["收支类型"].ColumnName = "CostType";
                c["金额"].ColumnName = "Amount";
                c["登记人"].ColumnName = "Registrant";
                c["数据来源部门"].ColumnName = "DataDepartment";
                c["摘要"].ColumnName = "abstract";
                c["运单编号"].ColumnName = "BillNo";

            }
            catch (Exception ex)
            {
                //MsgBox.ShowException(ex);
                MsgBox.ShowError("请检查自己导入的文档是否为标准格式！");
            }

        }

        private bool ValidationDate(DataRowCollection r){
            bool flag = false;
            try
            {
                string[] paytype ={"支出", "收入"};
                string[] freetype = { "理赔款", "付罚款", "收奖励", "用车成本", "其他费", "其他办公材料费" };
                
                string ptvalue = null;
                string ftvalue = null;
                for (int i=0;i< r.Count;i++)
                {
                    string ptype = r[i]["CostType"].ToString().Trim();
                    string ftype = r[i]["Revenue"].ToString().Trim();
                    
                    for (int j = 0; j < paytype.Length; j++)
                    {
                        flag = false;
                        ptvalue = paytype[j];
                        if (ptype == ptvalue) {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag) {
                        MsgBox.ShowError("请检查文档的第"+ (i+1) + "行收支类型错误！");
                        return flag;
                    }
                    for (int k = 0; k < freetype.Length; k++)
                    {
                        flag = false;
                        ftvalue = freetype[k];
                        if (ftype == ftvalue)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        MsgBox.ShowError("请检查文档的第" + (i + 1) + "行费用类型错误！");
                        return flag;
                    }
                }
            }
            catch(Exception ex)
            {
                MsgBox.ShowError("请检查自己导入的文档是否为标准格式！");
            }
            return flag;
        }

        //上传
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.RowCount==0)
            {
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_BASWEBup", list);
                DataSet ds = SqlHelper.GetDataSet(spe);
                string WebName1 = "";
                string WebName2 = "";

                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    WebName2 = myGridView1.GetRowCellValue(i, "WebName").ToString();
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        WebName1 = ds.Tables[0].Rows[j]["WebName"].ToString();
                        if (WebName1 == WebName2)
                        {
                            break;
                        }
                        if (j == (ds.Tables[0].Rows.Count-1) && WebName1 != WebName2)
                        {
                            MsgBox.ShowOK("上传的网点名称包含错误信息！");
                            return;
                        }
                    }
                   
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

            DataTable dt = ((System.Data.DataView)(myGridView1.DataSource)).Table;
            if (dt.Columns.Contains("序号"))
            {
                dt.Columns.Remove("序号");
                dt.AcceptChanges();
            }
           
            
            DataRow dr = dt.NewRow();
            string msg = "";
            if (CommonClass.BasUpload(dt, "USP_ADD_CollectionCommission_UPLOAD", out msg))
            {

                MsgBox.ShowOK(msg);
                this.Close();
            }
            else
            {
                MsgBox.ShowError(msg);
            }


        }


    }
}