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
    public partial class frmNewBudget : BaseForm
    {
        public frmNewBudget()
        {
            InitializeComponent();
        }

        //public string Cause1 = "", Area1 = "", Web1 = "", FeeProject1 = "", FeeType1 = "", BelongMonth1 = "", BudgetMoney1 = "", BudgetBalance1 = "";
        public string id = "";
        public int type;//0新增，1修改
        //public DataSet dsshipper = new DataSet();//汇款客户资料 打开银行信息平台就开始提取
        DataSet dsFeeType = new DataSet();



        private void frmNewBudget_Load_1(object sender, EventArgs e)
        {
           // CommonClass.GetGridViewColumns(myGridView2);
            CommonClass.SetWeb(registerDept, false);
            CommonClass.SetCause(Cause, true);
            CommonClass.SetArea(Area, Cause.Text,false);
            CommonClass.SetWeb(Web, Area.Text,false);
            Cause.EditValue = CommonClass.UserInfo.CauseName;
            Area.EditValue = CommonClass.UserInfo.AreaName;
            Web.EditValue = CommonClass.UserInfo.WebName;
            registerDept.EditValue = CommonClass.UserInfo.WebName;
         
            //FindDate.DateTime = CommonClass.gcdate;
            //CommonClass.FormSet(this);
            //CommonClass.GetGridViewColumns(myGridView2);
            //GridOper.SetGridViewProperty(myGridView2);
            // BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条,就引用其实例

            CommonClass.SetCause(Cause, false);
            dsFeeType = GetFeeType();
            if (dsFeeType != null && dsFeeType.Tables.Count > 0 && dsFeeType.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = dsFeeType.Tables[0].Select("ParentID=0");
                if (drs.Length > 0)
                {
                    FeeType.Properties.Items.Clear();
                    for (int i = 0; i < drs.Length; i++)
                    {
                        FeeType.Properties.Items.Add(drs[i]["TypeName"].ToString());
                    }
                }


            }


            //zaj
            BelongYear.Properties.Items.Clear();
            BelongYear.Properties.Items.Add(DateTime.Now.Year - 1);
            BelongYear.Properties.Items.Add(DateTime.Now.Year);
            BelongYear.Properties.Items.Add(DateTime.Now.Year + 1);

            BelongYear.Text = DateTime.Now.Year.ToString();

            int mon = DateTime.Now.Month;
            if (mon < 10) { BelongMonth.Text = "0" + mon.ToString(); }
            else { BelongMonth.Text = mon.ToString(); }






        }

        //事业部下拉
        private void Cause_SelectedIndexChanged(object sender, EventArgs e)
        {
           // CommonClass.SetArea(Area, Cause.Text);
            CommonClass.SetArea(Area, Cause.Text,false);
            //CommonClass.SetCauseWeb(Web, Cause.Text, Area.Text);
        }
        //大区下拉
        private void Area_SelectedIndexChanged(object sender, EventArgs e)
        {
           CommonClass.SetCauseWeb(Web, Cause.Text, Area.Text,false);
         
           
        }
        
       
        //取消
        private void button2_Click(object sender, EventArgs e)
        {
            Close();

        }
        //确定
        private void button1_Click(object sender, EventArgs e)
        {
            string posPattern = @"^[0-9]+(.[0-9]{1,2})?$";//验证正数正则
            if (Cause.Text.Trim().ToString() == (""))
            {
                MsgBox.ShowOK("请选择事业部！");
                return;
            }
          
            if (Area.Text.Trim().ToString() == (""))
            {
                MsgBox.ShowOK("请选择大区！");
                return;
            }
            if (Web.Text.Trim().ToString() == (""))
            {
                MsgBox.ShowOK("请选择网点！");
                return;
            }

            if (FeeType.Text.Trim().ToString() == (""))
            {
                MsgBox.ShowOK("请选择费用类型！");
                return;
            }
            if (FeeProject.Text.Trim().ToString() == (""))
            {
                MsgBox.ShowOK("请输入项目！");
                return;
            }
            
            if (BelongMonth.Text.Trim().ToString() == (""))
            {
                MsgBox.ShowOK("请选择所属月份！");
                return;
            }
            //string m = BelongMonth.Text.Trim().ToString();
            //if (m != "1" && m != "2" && m != "3" && m != "4" && m != "5" && m != "6" && m != "7" && m != "8" && m != "9" && m != "10" && m != "11" && m != "12")
            //{
            //    MsgBox.ShowOK("月份中仅能输入1~12中的整数,请核对后重新添加!");
            //    return;
            //}
            if (BudgetMoney.Text.Trim().ToString() == (""))
            {
                MsgBox.ShowOK("请输入预算金额！");
                return;
            }
            if (!Regex.IsMatch(BudgetMoney.Text, posPattern))
            {
                MsgBox.ShowOK("输入金额格式不正确!");
                return;
            }
            //if (BudgetBalance.Text.Trim().ToString() == (""))
            //{
            //    MsgBox.ShowOK("请输入预算余额");
            //    return;
            //}

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BudgetCause", Cause.Text.Trim()));
                list.Add(new SqlPara("BudgetArea", Area.Text.Trim()));
                list.Add(new SqlPara("BudgetWeb", Web.Text.Trim()));
                list.Add(new SqlPara("FeeProject", FeeProject.Text.Trim()));
                list.Add(new SqlPara("FeeType", FeeType.Text.Trim()));
                list.Add(new SqlPara("BelongMonth",BelongYear.Text.Trim()+ BelongMonth.Text.Trim()));
                list.Add(new SqlPara("BudgetMoney", BudgetMoney.Text.Trim()));
                //list.Add(new SqlPara("BudgetBalance", BudgetBalance.Text.Trim()));
                //list.Add(new SqlPara("ID1", textBox1.Text.Trim()));
                list.Add(new SqlPara("ID", id));
                list.Add(new SqlPara("type", type));
                list.Add(new SqlPara("registerDept", registerDept.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_ExpenseBudget", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }

        }
        //public void getDepartureInfo() 
        //{
        //    try {
        //        Cause.Text = Cause1;
        //        Area.Text = Area1;
        //        Web.Text = Web1;
        //        FeeProject.Text = FeeProject1;
        //        FeeType.Text = FeeType1;
        //        BelongMonth.Text = BelongMonth1;
        //        BudgetMoney.Text = BudgetMoney1;
        //        BudgetBalance.Text = BudgetBalance1;
        //        textBox1.Text = id;


        //        if (Cause.EditValue != Cause1|| Area.Text != Area1 || Web.Text != Web1 || FeeProject.Text != FeeProject1 || FeeType.Text != FeeType1 || BelongMonth.Text != BelongMonth1 || BudgetMoney.Text != BudgetMoney1||BudgetBalance.Text != BudgetBalance1)
        //        {
        //            button1.Enabled = true;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        MsgBox.ShowOK(ex.Message);
        //    }

        //}


        public void getdata()
        {
            if (id == "") return;
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_ExpenseBudget_BY_ID", new List<SqlPara> { new SqlPara("ID", id) }));

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
            Cause.EditValue = ds.Tables[0].Rows[0]["BudgetCause"];
            Area.EditValue = ds.Tables[0].Rows[0]["BudgetArea"];
            Web.EditValue = ds.Tables[0].Rows[0]["BudgetWeb"];
            FeeProject.Text = ds.Tables[0].Rows[0]["FeeProject"].ToString();
            FeeType.Text = ds.Tables[0].Rows[0]["FeeType"].ToString();
            BelongMonth.Text = ds.Tables[0].Rows[0]["BelongMonth"].ToString().Substring(4,2);
            BudgetMoney.Text = ds.Tables[0].Rows[0]["BudgetMoney"].ToString();
            BelongYear.Text = ds.Tables[0].Rows[0]["BelongMonth"].ToString().Substring(0,4);
            registerDept.Text = ds.Tables[0].Rows[0]["RegisterDept"].ToString();
           // BudgetBalance.Text = ds.Tables[0].Rows[0]["BudgetBalance"].ToString();

        }
        private DataSet GetFeeType()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_FeeType", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                return ds;

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                return null;
            }
        }

      

        //private void BudgetBalance_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (((int)e.KeyChar < 48 || (int)e.KeyChar > 57) && (int)e.KeyChar != 8 && (int)e.KeyChar != 46)
        //    {
        //        e.Handled = true;

        //        if ((int)e.KeyChar == 46)
        //        {
        //            if (BudgetMoney.Text.Length <= 0)
        //            {
        //                e.Handled = true;
        //                MessageBox.Show("小数点不能在第一位!");//小数点不能在第一位
        //            }
        //        }
        //    }
        //}

        private void FeeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string feeType = FeeType.Text.Trim();
            //string sql = "TypeName='" + feeType + "' and ParentID=0";

            //DataRow[] drs = dsFeeType.Tables[0].Select(sql);
            //int id = 0;
            //if (drs.Length > 0)
            //{
            //    id = Convert.ToInt32(drs[0]["FeeID"]);
            //}
            //FeeProject.Text = "";
            //FeeProject.Properties.Items.Clear();
            //string sqlstr = "ParentID=" + id;
            //DataRow[] drs1 = dsFeeType.Tables[0].Select(sqlstr);
            //if (drs1.Length > 0)
            //{
            //    for (int i = 0; i < drs1.Length; i++)
            //    {
            //        FeeProject.Properties.Items.Add(drs1[i]["TypeName"].ToString());
            //    }
            //}
        }

        private void BudgetMoney_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (((int)e.KeyChar < 48 || (int)e.KeyChar > 57) && (int)e.KeyChar != 8 && (int)e.KeyChar != 46)
            {
                e.Handled = true;

                if ((int)e.KeyChar == 46)
                {
                    if (BudgetMoney.Text.Length <= 0)
                    {
                        e.Handled = true;
                        MessageBox.Show("小数点不能在第一位!");//小数点不能在第一位
                    }
                }
            }
        }




        }
    }

        
  

