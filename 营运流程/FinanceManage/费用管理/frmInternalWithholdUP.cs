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
    public partial class frmInternalWithholdUP : BaseForm
    {
        public frmInternalWithholdUP()
        {
            InitializeComponent();
        }

        private void frmInternalWithholdUP_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3);
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "选择代扣款文件";
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

               //c["序号"].ColumnName="ID";
                c["运单号"].ColumnName = "BillNo";
               // c["登记日期"].ColumnName = "RecordDate";
                c["项目"].ColumnName = "Item";
                c["费用类型"].ColumnName = "FeeType";
                c["费用所属月份"].ColumnName = "FeeMonth";
                c["摘要"].ColumnName = "Remark";
                c["金额"].ColumnName = "Money";
                c["承担部门"].ColumnName = "ResponsibleDepartment";
                c["收益部门"].ColumnName = "RevenueDepartement";
              
                

            }
            catch (Exception ex)
            {
              
                MsgBox.ShowError("请检查自己导入的文档是否为标准格式！");
            }

        }
        //检测是否包含中文
        public  static bool ContainChinese(string input)
         {
             string pattern = "[\u4e00-\u9fbb]";
             return Regex.IsMatch(input, pattern);
         }

        //上传
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.RowCount == 0)
            {
                return;
            }
            DataTable dt = ((System.Data.DataView)(myGridView1.DataSource)).Table;
            if (dt.Columns.Contains("序号"))
            {
                dt.Columns.Remove("序号");
                dt.AcceptChanges();
            }
            //for (int i = 0; i < myGridView1.RowCount; i++)
            //{
            //    string m = myGridView1.GetRowCellValue(i, "FeeMonth").ToString();
            //    if (ContainChinese(m))
            //    {
            //        MsgBox.ShowOK("月份中仅能输入数字,请核对后重新上传!");
            //        return;
            //    }
            //}
            string patten = @"^[2][0][12][0-9](0[1-9]|1[0-2])$";//201001-202912
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!Regex.IsMatch(dt.Rows[i]["FeeMonth"].ToString(), patten))
                {
                    MsgBox.ShowOK("所属月份格式不正确请选择!");
                    return;
                }
            }

            DataRow dr = dt.NewRow();
            string msg = "";
            if (CommonClass.BasUpload(dt, "USP_ADD_WITHHOLDIN_UP", out msg))
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