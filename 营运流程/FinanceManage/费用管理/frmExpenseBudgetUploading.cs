using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using System.Text.RegularExpressions;

namespace ZQTMS.UI
{
    public partial class frmExpenseBudgetUploading : BaseForm
    {
        public frmExpenseBudgetUploading()
        {
            InitializeComponent();
        }

        private void frmExpenseBudgetUploading_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            //CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3);
        }
        //退出
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
        //导入
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "选择费用预算文件";
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
            //将EXCEL导入到Datatable
            DataTable dt = NpoiOperExcel.ExcelToDataTable(ofd.FileName, false);
            SetColumnName(dt.Columns);
            myGridControl1.DataSource = dt;
        }

         private void SetColumnName(DataColumnCollection c)
        {
            try
            {
                foreach (DataColumn dc in c)
                {
                    dc.ColumnName = dc.ColumnName.Trim();
                }
                c["预算事业部"].ColumnName = "BudgetCause";
                c["预算大区"].ColumnName = "BudgetArea";
                c["预算部门"].ColumnName = "BudgetWeb";//预算网点
                c["登记人"].ColumnName = "RegisterMan";
                c["项目"].ColumnName = "FeeProject";
                c["费用类型"].ColumnName = "FeeType";
                c["所属月份"].ColumnName = "BelongMonth";
                c["预算金额"].ColumnName = "BudgetMoney";
                c["登记部门"].ColumnName = "RegisterDept";
            }
            catch (Exception ex)
            {
                //MsgBox.ShowException(ex);
                MsgBox.ShowError("请检查自己导入的文档是否为标准格式！");
            }

        }
        //检测是否包含中文
        public  static bool ContainChinese(string input)
         {
             string pattern = "[\u4e00-\u9fbb]";
             return Regex.IsMatch(input, pattern);
         }
        private bool isMoney(string budgetMoney)
        {
            bool ismony = true;
            try
            {
                decimal aa = Convert.ToDecimal(budgetMoney);
                ismony = false;
            }
            catch
            { 
                
            }
            return ismony;
        }
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.RowCount == 0)
            {
                return;
            }
            //DataTable dt = ((System.Data.DataView)(myGridView1.DataSource)).Table;
            //if (dt.Columns.Contains("序号"))
            //{
            //    dt.Columns.Remove("序号");
            //    dt.AcceptChanges();
            //}
            //for (int i = 0; i < myGridView1.RowCount; i++)
            //{
            //    string m = myGridView1.GetRowCellValue(i, "BelongMonth").ToString();
            //    if (ContainChinese(m))
            //    {
            //        MsgBox.ShowOK("月份中仅能输入数字,请核对后重新上传!");
            //        return;
            //    }
            //}

            //string patten = @"^[2][0][12][0-9](0[1-9]|1[0-2])$";////201001-202912
            string budgetCause = "", budgetArea = "", budgetWeb = "", registerMan = "", feeProject = "", feeType = "", belongMonth = "", budgetMoney = "", registerDept = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                //if (!Regex.IsMatch(dt.Rows[i]["BelongMonth"].ToString(), patten))
                //{
                //    MsgBox.ShowOK("所属月份格式不正确请选择!");
                //    return;
                //}
                //string currentTime = DateTime.Now.ToString("YYMM");// 
                string currentYear = DateTime.Now.Year.ToString();
                string currentMonth = DateTime.Now.Month.ToString();
                int currentTime = Convert.ToInt32(currentYear + currentMonth);//201811
                string mothExp = "^[0-9]*$";  //^[0-9]*$
                int beMonth = Convert.ToInt32(myGridView1.GetRowCellValue(i, "BelongMonth").ToString().Substring(0, 6));//上传的所属月份，截取前六位和当前日期进行比较
                string monthLength = myGridView1.GetRowCellValue(i, "BelongMonth").ToString();
                if (beMonth != currentTime)
                {
                    MsgBox.ShowOK("预算月份非当前月份，无法导入!");
                    return;
                }
                if (isMoney(myGridView1.GetRowCellValue(i, "BudgetMoney").ToString()))
                {
                    MsgBox.ShowOK("输入的金额格式错误,请检查！");
                    return;
                }
                if (!Regex.IsMatch(monthLength, mothExp) || monthLength.Length != 6)
                {
                    MsgBox.ShowOK("输入的月份格式错误，请检查！");
                    return;
                }
                budgetCause += myGridView1.GetRowCellValue(i, "BudgetCause").ToString() + "@";
                budgetArea += myGridView1.GetRowCellValue(i, "BudgetArea").ToString() + "@";
                budgetWeb += myGridView1.GetRowCellValue(i, "BudgetWeb").ToString() + "@";
                registerMan += myGridView1.GetRowCellValue(i, "RegisterMan").ToString() + "@";
                feeProject += myGridView1.GetRowCellValue(i, "FeeProject").ToString() + "@";
                feeType += myGridView1.GetRowCellValue(i, "FeeType").ToString() + "@";
                belongMonth += myGridView1.GetRowCellValue(i, "BelongMonth").ToString() + "@";
                budgetMoney += myGridView1.GetRowCellValue(i, "BudgetMoney").ToString() + "@";
                registerDept += myGridView1.GetRowCellValue(i, "RegisterDept").ToString() + "@";
            }
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("budgetCause", budgetCause));
            list.Add(new SqlPara("budgetArea", budgetArea));
            list.Add(new SqlPara("budgetWeb", budgetWeb));
            list.Add(new SqlPara("registerMan", registerMan));
            list.Add(new SqlPara("feeProject", feeProject));
            list.Add(new SqlPara("feeType", feeType));
            list.Add(new SqlPara("belongMonth", belongMonth));
            list.Add(new SqlPara("budgetMoney", budgetMoney));
            list.Add(new SqlPara("registerDept", registerDept));
            SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_ExpenseBudget_UPLOAD", list);
            if (SqlHelper.ExecteNonQuery(sps) > 0)
            {
                MsgBox.ShowOK();
                Close();
            }
            //DataRow dr = dt.NewRow();
            //string msg = "";
            //if (CommonClass.BasUpload(dt, "USP_ADD_ExpenseBudget_UPLOAD", out msg))
            //{
            //    MsgBox.ShowOK(msg);
            //    this.Close();
            //}
            //else
            //{
            //    MsgBox.ShowError(msg);
            //}

        }
    }
}