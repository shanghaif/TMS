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

namespace ZQTMS.UI
{
    public partial class frmInternalTransactionListAdd : BaseForm
    {
        public frmInternalTransactionListAdd()
        {
            InitializeComponent();
        }
        public int type = 0, a = 0;//0新增
        public string ID = "";

        private void frmInternalTransactionListAdd_Load(object sender, EventArgs e)
        {
            CommonClass.SetCause(BearSubject, false);
            CommonClass.SetWeb_Cause(BearDep, BearSubject.Text, false);
            CommonClass.SetCause(BenefitSubject, false);
            CommonClass.SetWeb_Cause(BenefitDep, BenefitSubject.Text, false);
            getInsideType();
            if (type == 0)
            {
                CreateSerialNumber();

                year.Text = DateTime.Now.Year.ToString();
                month.Text = DateTime.Now.Month.ToString();
            }
               
            else if (type == 1)
            {
                this.Text = "修改内部往来清单";
                GetDataByID();
                a++;

            }

              
           
           
            AddYear(year);
        }
        public void getInsideType()
        {
            SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_InternalType");
            DataSet ds = SqlHelper.GetDataSet(spe);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    InsideType.Properties.Items.Add(ds.Tables[0].Rows[i]["InsideType"]);
                }
            }
        }

        /// <summary>
        /// 修改提取数据
        /// </summary>
        public void GetDataByID()
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("ID", ID));
            SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_InternalTransactionList_BY_ID", list);
            DataSet ds = SqlHelper.GetDataSet(spe);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                SerialNumber.Text = dr["SerialNumber"].ToString();
                ReportNumber.Text = dr["ReportNumber"].ToString();
                BearSubject.Text = dr["BearSubject"].ToString();
                BearDep.Text = dr["BearDep"].ToString();
                BenefitSubject.Text = dr["BenefitSubject"].ToString();
                BenefitDep.Text = dr["BenefitDep"].ToString();
                string Period = dr["Period"].ToString();
                if (Period != "")
                {
                    int start = Period.IndexOf("年");
                    int end = Period.IndexOf("月");
                    year.Text = Period.Substring(0, start );
                    month.Text = Period.Substring(start + 1, end - start - 1);
                }
                Amount.Text = dr["Amount"].ToString();
                InsideType.Text = dr["InsideType"].ToString();
                Remark.Text = dr["Remark"].ToString();
            }
        }
        /// <summary>
        /// 生成往来编号
        /// </summary>
        public void CreateSerialNumber()
        {
            Random rd = new Random();
            string sn = "nw";
            sn += DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + rd.Next(100000, 999999);
            SerialNumber.Text = sn;
        }
        /// <summary>
        /// 生成年份
        /// </summary>
        public void AddYear(ComboBoxEdit cb)
        {
            int year = DateTime.Now.Year;
            for (int i = 0; i < 12; i++)
            {
                cb.Properties.Items.Add(year - 12 + i);
            }
            for (int i = 0; i < 12; i++)
            {
                cb.Properties.Items.Add(year + i);
            }
        }
        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (SerialNumber.Text.Trim() == "")
            {
                MsgBox.ShowOK("往来编号为空,请重试!");
                return;
            }
            if (BearSubject.Text.Trim() == "")
            {
                MsgBox.ShowOK("请选择承担主体");
                return;
            }
            if (BearDep.Text.Trim() == "")
            {
                MsgBox.ShowOK("请选择承担部门");
                return;
            }
            if (BenefitSubject.Text.Trim() == "")
            {
                MsgBox.ShowOK("请选择受益主体");
                return;
            }
            if (BenefitDep.Text.Trim() == "")
            {
                MsgBox.ShowOK("请选择受益部门");
                return;
            }
            if (year.Text.Trim() == "" || month.Text.Trim() == "")
            {
                MsgBox.ShowOK("请选择所属期间");
                return;
            }
            if (ConvertType.ToDecimal(Amount.Text.Trim()) < 0)
            {
                MsgBox.ShowOK("金额输入非法!");
                return;
            }
            if (InsideType.Text.Trim() == "")
            {
                MsgBox.ShowOK("请输入内部往来类型");
                return;
            }
            if (type == 1 && ID == "")
            {
                MsgBox.ShowOK("未获取到参数!");
                this.Close();
            }
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("SerialNumber", SerialNumber.Text.Trim()));
            list.Add(new SqlPara("ReportNumber", ReportNumber.Text.Trim()));
            list.Add(new SqlPara("BearSubject", BearSubject.Text.Trim()));
            list.Add(new SqlPara("BearDep", BearDep.Text.Trim()));
            list.Add(new SqlPara("BenefitSubject", BenefitSubject.Text.Trim()));
            list.Add(new SqlPara("BenefitDep", BenefitDep.Text.Trim()));
            list.Add(new SqlPara("Period", year.Text.Trim() + "年" + month.Text.Trim() + "月"));
            list.Add(new SqlPara("Amount", Amount.Text.Trim()));
            list.Add(new SqlPara("InsideType", InsideType.Text.Trim()));
            list.Add(new SqlPara("Remark", Remark.Text.Trim()));
            list.Add(new SqlPara("type", type));
            list.Add(new SqlPara("ID", ID));
            SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "USP_InternalTransactionList", list);
            if (SqlHelper.ExecteNonQuery(spe) > 0)
            {
                MsgBox.ShowOK();
                this.Close();
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BearSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (type == 1)
            {
                if (a != 0)
                {
                    CommonClass.SetWeb_Cause(BearDep, BearSubject.Text, false);
                }
            }
            else
            {
                CommonClass.SetWeb_Cause(BearDep, BearSubject.Text, false);

            }

        }

        private void BenefitSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (type == 1)
            {
                if (a != 0)
                {
                    CommonClass.SetWeb_Cause(BenefitDep, BenefitSubject.Text, false);
                }
            }
            else
            {
                CommonClass.SetWeb_Cause(BenefitDep, BenefitSubject.Text, false);
            }
        }

    }
}