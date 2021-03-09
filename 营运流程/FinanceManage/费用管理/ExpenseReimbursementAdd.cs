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
using System.Text.RegularExpressions;

namespace ZQTMS.UI
{
    public partial class ExpenseReimbursementAdd : BaseForm
    {
        public ExpenseReimbursementAdd()
        {
            InitializeComponent();
        }

        public string id = "";
        public DataSet dsshipper = new DataSet();//汇款客户资料 打开银行信息平台就开始提取
        DataSet dsFeeType = new DataSet();

        private void btnSave_Click(object sender, EventArgs e)
        {
            string posPattern = @"^[0-9]+(.[0-9]{1,2})?$";//验证正数正则
            if (ApplyDate.Text.Trim() == "")
            {
                MsgBox.ShowOK("申报日期必填!");
                return;
            }
            if (CauseName.Text.Trim() == "")
            {
                MsgBox.ShowOK("申报事业部必填!");
                return;
            } if (AreaName.Text.Trim() == "")
            {
                MsgBox.ShowOK("申报大区必填!");
                return;
            } if (ApplyDept.Text.Trim() == "")
            {
                MsgBox.ShowOK("申报部门必填!");
                return;
            } if (FeeType.Text.Trim() == "")
            {
                MsgBox.ShowOK("费用类型必填!");
                return;
            } if (FeeProject.Text.Trim() == "")
            {
                MsgBox.ShowOK("项目必填!");
                return;
            } if (AssumeDept.Text.Trim() == "")
            {
                MsgBox.ShowOK("承担部门必填!");
                return;
            }
            if (BelongMonth.Text.Trim() == "")
            {
                MsgBox.ShowOK("所属月份必填!");
                return;
            } if (Money.Text.Trim() == "")
            {
                MsgBox.ShowOK("金额必填!");
                return;
            }
            if (ApplyMan.Text.Trim() == "")
            {
                MsgBox.ShowOK("申报人必填!");
                return;
            }
            if (!Regex.IsMatch(Money.Text, posPattern))
            {
                MsgBox.ShowOK("输入金额格式不正确!");
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ApplyCause",CauseName.Text.Trim()));
                list.Add(new SqlPara("ApplyArea", AreaName.Text.Trim()));
                list.Add(new SqlPara("ApplyDept", ApplyDept.Text.Trim()));
                list.Add(new SqlPara("ApplyMan", ApplyMan.Text.Trim()));
                list.Add(new SqlPara("ApplyDate", ApplyDate.Text.Trim()));
                list.Add(new SqlPara("FeeType", FeeType.Text.Trim()));
                list.Add(new SqlPara("FeeProject", FeeProject.Text.Trim()));
                list.Add(new SqlPara("BelongMonth",BelongYear.Text.Trim()+ BelongMonth.Text.Trim()));
                list.Add(new SqlPara("Money", Money.Text.Trim()));
                list.Add(new SqlPara("Remark", Remark.Text.Trim()));
                list.Add(new SqlPara("AssumeDept", AssumeDept.Text.Trim()));
                if (id == "")
                {
                    list.Add(new SqlPara("OperType", 0));
                }
                else
                {
                    list.Add(new SqlPara("OperType",1));
                    list.Add(new SqlPara("ID",id));
                }


                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_Expensereimbursement",list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    this.Close();
                }
            
            }catch(Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        private DataSet GetFeeType()
        { 
          try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_FeeType", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                return ds;
            
            }catch(Exception ex)
            {
                MsgBox.ShowException(ex);
                return null;
            }
        }

        private void ExpenseReimbursementAdd_Load(object sender, EventArgs e)
        {
            CommonClass.SetCause(CauseName, false);
           

            CauseName.EditValue = CommonClass.UserInfo.CauseName;
            AreaName.EditValue = CommonClass.UserInfo.AreaName;
            ApplyDept.EditValue = CommonClass.UserInfo.WebName;
            CommonClass.SetWeb(ApplyDept, false);
            CommonClass.SetWeb(AssumeDept, false);
            AssumeDept.Properties.Items.Add("");
            AssumeDept.EditValue = CommonClass.UserInfo.WebName;
            CommonClass.SetUser(ApplyMan, ApplyDept.Text,false);
            ApplyDate.EditValue = CommonClass.gcdate;
            ApplyMan.Text = CommonClass.UserInfo.UserName;
             dsFeeType = GetFeeType();
            if (dsFeeType != null && dsFeeType.Tables.Count > 0 && dsFeeType.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = dsFeeType.Tables[0].Select("ParentID=0");
                if(drs.Length>0)
                {
                    FeeType.Properties.Items.Clear();
                    for (int i = 0; i < drs.Length; i++)
                    {
                        FeeType.Properties.Items.Add(drs[i]["TypeName"].ToString());
                    }
                }
                
            
            }
            BelongYear.Properties.Items.Clear();
            BelongYear.Properties.Items.Add(DateTime.Now.Year - 1);
            BelongYear.Properties.Items.Add(DateTime.Now.Year );
            BelongYear.Properties.Items.Add(DateTime.Now.Year+1);

            BelongYear.Text = DateTime.Now.Year.ToString();


            int mon = DateTime.Now.Month;
            if (mon < 10) { BelongMonth.Text = "0" + mon.ToString(); }
            else { BelongMonth.Text = mon.ToString(); }


            if (id.Trim() != "")
            {
                DataSet ds = GetExpensereimbursementByID();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ApplyDate.EditValue = ds.Tables[0].Rows[0]["ApplyDate"];
                    CauseName.Text = ds.Tables[0].Rows[0]["ApplyCause"].ToString();
                    AreaName.Text = ds.Tables[0].Rows[0]["ApplyArea"].ToString();
                    ApplyDept.Text = ds.Tables[0].Rows[0]["ApplyDept"].ToString();
                    FeeType.Text = ds.Tables[0].Rows[0]["FeeType"].ToString();
                    FeeProject.Text = ds.Tables[0].Rows[0]["FeeProject"].ToString();
                    AssumeDept.Text = ds.Tables[0].Rows[0]["AssumeDept"].ToString();
                    BelongMonth.Text = ds.Tables[0].Rows[0]["BelongMonth"].ToString().Substring(4,2);
                    BelongYear.Text = ds.Tables[0].Rows[0]["BelongMonth"].ToString().Substring(0,4);

                    Money.Text = ds.Tables[0].Rows[0]["Money"].ToString();
                    Remark.Text = ds.Tables[0].Rows[0]["Remark"].ToString();
                }
            }
           
        }

        private DataSet GetExpensereimbursementByID()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", id));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_Expensereimbursement_ByID", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                return ds;


            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                return null;
            }
        }

        private void CauseName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(AreaName, CauseName.Text);
           
        }

        private void AreaName_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void AssumeDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CommonClass.SetUser(ApplyMan, ApplyDept.Text);
        }

        private void ApplyDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetUser(ApplyMan, ApplyDept.Text,false);
        }

        private void btnCanccel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 选定科目类型后触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FeeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string feeType = FeeType.Text.Trim();
            string sql = "TypeName='" + feeType + "' and ParentID=0";

            DataRow[] drs = dsFeeType.Tables[0].Select(sql);
            int id = 0;
            if (drs.Length > 0)
            {
                id = Convert.ToInt32(drs[0]["FeeID"]);
            }
            FeeProject.Text = "";
            FeeProject.Properties.Items.Clear();
            string sqlstr = "ParentID=" + id;
            DataRow[] drs1 = dsFeeType.Tables[0].Select(sqlstr);
            if (drs1.Length > 0)
            {
                for (int i = 0; i < drs1.Length; i++)
                {
                    FeeProject.Properties.Items.Add(drs1[i]["TypeName"].ToString());
                }
            }
        }

        private void FeeProject_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 点击项目触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FeeProject_Properties_Click(object sender, EventArgs e)
        {
            string feeType = FeeType.Text.Trim();
            if (string.IsNullOrEmpty(feeType))
            {
                MsgBox.ShowOK("请先选择科目类型!");
                return;
            }
        }

    }
}
