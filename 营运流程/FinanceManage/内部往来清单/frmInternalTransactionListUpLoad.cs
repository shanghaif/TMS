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
using System.Text.RegularExpressions;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmInternalTransactionListUpLoad : BaseForm
    {
        public frmInternalTransactionListUpLoad()
        {
            InitializeComponent();
        }
        public string allInsideType = "";
        private void frmInternalTransactionListUpLoad_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            GridOper.SetGridViewProperty(myGridView1);
            getInsideType();
        }
        public void getInsideType()
        {
            SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_InternalType");
            DataSet ds = SqlHelper.GetDataSet(spe);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    allInsideType += ds.Tables[0].Rows[i]["InsideType"].ToString() + ","; 
                }
            }
        }
        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "承运单位信息";
            ofd.Filter = "Microsoft Excel文件|*.xls;*.xlsx";
            ofd.FilterIndex = 1;
            ofd.DefaultExt = "xls";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (!ofd.SafeFileName.EndsWith(".xls") && !ofd.SafeFileName.EndsWith("xlsx"))
                {
                    XtraMessageBox.Show("请选择Excel文件！", "文件导入失败！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!ofd.CheckFileExists)
                {
                    XtraMessageBox.Show("文件不存在，请重新选择！", "文件导入失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                DataTable dt = NpoiOperExcel.ExcelToDataTable2(ofd.FileName, true);
                if (dt == null)
                {
                    return;
                }
                SetColumnName(dt.Columns);
                dt.Columns.Add("SerialNumber", typeof(string));
                Random rd = new Random();
                string ex = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["SerialNumber"] = "";
                    string sn = "nw";
                    sn += DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + rd.Next(100000, 999999);
                    while (ex.Contains(sn))
                    {
                        sn = "nw";
                        sn += DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + rd.Next(100000, 999999);
                    }

                     dt.Rows[i]["SerialNumber"]= sn;
                    ex += sn + ",";
                }
                myGridControl1.DataSource = dt;
               
            }
        }
        private void SetColumnName(DataColumnCollection c)
        {
            string[] oldnames = {  "报表编号", "承担主体", "承担部门", "受益主体", "受益部门", "所属期间", "金额", "内部往来类型", "备注" };
            string[] newnames = {  "ReportNumber", "BearSubject", "BearDep", "BenefitSubject", "BenefitDep", "Period", "Amount", "InsideType", "Remark" };
            try
            {
                for (int i = 0; i < oldnames.Length; i++)
                {
                    c[oldnames[i]].ColumnName = newnames[i];
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError("请检查自己导入的文档是否为标准格式！");
            }
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
            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "ASP_ADD_InternalTransactionList_IMPORT", new List<SqlPara>() { new SqlPara("Tb", dt) })) > 0)
            {
                MsgBox.ShowOK("上传成功！");
                this.Close();
            }
            else
            {
                MsgBox.ShowError("上传失败！请先上传数据！");
            }
        }
        public DataTable dtCause = CommonClass.dsCause.Tables[0];
        public DataTable dtWeb = CommonClass.dsWeb.Tables[0];

        private Boolean checkData(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                if (dt.Rows[i]["BearSubject"] == null || dt.Rows[i]["BearSubject"].ToString() == "")
                {
                    MsgBox.ShowOK("第" + (i + 1) + "行承担主体不能为空,请检查！");
                    return false;
                }

                if (dt.Rows[i]["BearDep"] == null || dt.Rows[i]["BearDep"].ToString() == "")
                {
                    MsgBox.ShowOK("第" + (i + 1) + "行承担部门不能为空,请检查！");
                    return false;
                }

                if (dt.Rows[i]["BenefitSubject"] == null || dt.Rows[i]["BenefitSubject"].ToString() == "")
                {
                    MsgBox.ShowOK("第" + (i + 1) + "行受益主体不能为空,请检查！");
                    return false;
                }
                if (dt.Rows[i]["BenefitDep"] == null || dt.Rows[i]["BenefitDep"].ToString() == "")
                {
                    MsgBox.ShowOK("第" + (i + 1) + "行受益部门不能为空,请检查！");
                    return false;
                }
                //
                if (dt.Rows[i]["Period"] == null || dt.Rows[i]["Period"].ToString() == "")
                {
                    MsgBox.ShowOK("第" + (i + 1) + "行所属期间不能为空,请检查！");
                    return false;
                }
                if (!(dt.Rows[i]["Period"].ToString().Contains("年") && dt.Rows[i]["Period"].ToString().Contains("月")))
                {
                    MsgBox.ShowOK("所属期间格式不正确,正确格式:2018年01月");
                    return false;
                }
                if (dt.Rows[i]["Period"].ToString().Length != 8)
                {
                    MsgBox.ShowOK("所属期间格式不正确,正确格式:2018年01月");
                    return false;
                }

                if (dt.Rows[i]["Amount"] == null || dt.Rows[i]["Amount"].ToString() == "")
                {
                    MsgBox.ShowOK("第" + (i + 1) + "行金额不能为空,请检查！");
                    return false;
                }

                //检测中文
                string Amount = myGridView1.GetRowCellValue(i, "Amount").ToString();
                if (ContainChinese(Amount))
                {
                    MsgBox.ShowOK("金额格式不正确,请核对后重新上传!");
                    return false;
                }
                try
                {
                    decimal a =Convert.ToDecimal(dt.Rows[i]["Amount"].ToString());
                }
                catch(Exception ex)
                {
                    MsgBox.ShowOK("第" + (i + 1) + "行金额格式不正确,请检查！");
                    return false;
                }

                if (dt.Rows[i]["InsideType"] == null || dt.Rows[i]["InsideType"].ToString() == "")
                {
                    MsgBox.ShowOK("第" + (i + 1) + "行内部往来类型不能为空,请检查！");
                    return false;
                }
                if (allInsideType != "")
                {
                    if (!(allInsideType.Contains(dt.Rows[i]["InsideType"].ToString())))
                    {
                        MsgBox.ShowOK("第" + (i + 1) + "行内部往来类型不存在系统中,请检查！");
                        return false;
                    }
                }



                //检测主体和部门是否符合
                string BearSubject = dt.Rows[i]["BearSubject"].ToString();//承担主体
                string BearDep = dt.Rows[i]["BearDep"].ToString();//部门

                string BenefitSubject = dt.Rows[i]["BenefitSubject"].ToString();//受益主体
                string BenefitDep = dt.Rows[i]["BenefitDep"].ToString();//部门
                if (dtCause == null || dtCause.Rows.Count <= 0 || dtWeb == null || dtWeb.Rows.Count <= 0)
                {
                    MsgBox.ShowOK("参数获取错误!");
                    return false;
                }
                //主体
                DataRow[] dr = dtCause.Select("CauseName='"+BearSubject+"'");
                if(dr.Length==0)
                {
                    MsgBox.ShowOK("第" + (i + 1) + "行承担主体不存在,请核对!");
                    return false;
                }
                DataRow[] dr2 = dtCause.Select("CauseName= '" + BenefitSubject+"'");
                if (dr2.Length == 0)
                {
                    MsgBox.ShowOK("第" + (i + 1) + "行受益主体不存在,请核对!");
                    return false;
                }
                //部门
                DataRow[] dr3 = dtWeb.Select("BelongCause='" + BearSubject + "' and WebName ='" + BearDep + "'");
                if (dr3.Length == 0)
                {
                    MsgBox.ShowOK("第" + (i + 1) + "行承担部门不属于承担主体,请核对!");
                    return false;
                }
                DataRow[] dr4 = dtWeb.Select("BelongCause='" + BenefitSubject + "' and WebName ='" + BenefitDep + "'");
                if (dr4.Length == 0)
                {
                    MsgBox.ShowOK("第" + (i + 1) + "行受益部门不属于受益主体,请核对!");
                    return false;
                }
               
            }
            return true;
        }
        //检测是否包含中文
        public static bool ContainChinese(string input)
        {
            string pattern = "[\u4e00-\u9fbb]";
            return Regex.IsMatch(input, pattern);
        }
       

    }
}