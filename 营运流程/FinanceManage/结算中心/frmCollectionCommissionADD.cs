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
    public partial class frmCollectionCommissionADD : BaseForm
    {
        public string type = "";
        public string ID = "";
        public frmCollectionCommissionADD() 
        {
            InitializeComponent();
        }

        private void frmCollectionCommissionADD_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.SetCause(Cause, false);
            CommonClass.SetArea(Area, Cause.Text.Trim(), false);
            CommonClass.SetWeb(web, Area.Text);
            DataDepartment.Text = CommonClass.UserInfo.WebName;
            Registrant.Text = CommonClass.UserInfo.UserName;
            RegistrationDate.EditValue = DateTime.Now.ToString();
            if (type == "1")
            {
                try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("ID",ID));
                    SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_CollectionCommission_ID", list);
                    DataSet ds = SqlHelper.GetDataSet(spe);
                    Cause.Text = ds.Tables[0].Rows[0]["CauseName"].ToString();
                    Area.Text = ds.Tables[0].Rows[0]["AreaName"].ToString();
                    web.Text = ds.Tables[0].Rows[0]["WebName"].ToString();
                    month.Text = ds.Tables[0].Rows[0]["month"].ToString();
                    Revenue.Text = ds.Tables[0].Rows[0]["Revenue"].ToString();
                    RegistrationDate.Text = ds.Tables[0].Rows[0]["RegistrationDate"].ToString();
                    Registrant.Text = ds.Tables[0].Rows[0]["Registrant"].ToString();
                    DataDepartment.Text = ds.Tables[0].Rows[0]["DataDepartment"].ToString();
                    CostType.Text = ds.Tables[0].Rows[0]["CostType"].ToString();
                    Amount.Text = ds.Tables[0].Rows[0]["Amount"].ToString();
                    abstracts.Text = ds.Tables[0].Rows[0]["abstract"].ToString();
                    BillNo.Text = ds.Tables[0].Rows[0]["BillNo"].ToString();

                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }

            }



        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (Cause.EditValue == "")
            {
                MsgBox.ShowOK("事业部为必填项！");
                return;
            }
            if (Area.EditValue == "")
            {
                MsgBox.ShowOK("大区为必填项！");
                return;
            }
            if (web.EditValue == "")
            {
                MsgBox.ShowOK("网点为必填项！");
                return;
            }
            if (Amount.Text == "")
            {
                MsgBox.ShowOK("金额为必填项！");
                return;
            }
            //if (!vidvaliNum(Amount.EditValue))
            //{
            //    MsgBox.ShowOK("请输入正确的金额！");
            //    return;
            //}
            if (abstracts.Text == "")
            {
                MsgBox.ShowOK("摘要为必填项！");
                return;
            }
            
            if (type == "1")
            {
                try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("type", "修改"));
                    list.Add(new SqlPara("ID", ID));
                    list.Add(new SqlPara("CauseName", Cause.EditValue)); 
                    list.Add(new SqlPara("AreaName", Area.EditValue));
                    list.Add(new SqlPara("WebName", web.EditValue));
                    list.Add(new SqlPara("month", month.EditValue));
                    list.Add(new SqlPara("Revenue", Revenue.EditValue));
                    list.Add(new SqlPara("RegistrationDate", RegistrationDate.EditValue));
                    list.Add(new SqlPara("Registrant", Registrant.EditValue));
                    list.Add(new SqlPara("DataDepartment", DataDepartment.EditValue));
                    list.Add(new SqlPara("CostType", CostType.EditValue));
                    list.Add(new SqlPara("Amount", Amount.EditValue));
                    list.Add(new SqlPara("abstract", abstracts.EditValue));
                    list.Add(new SqlPara("BillNo", BillNo.Text.Trim()));
                    SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_ADD_CollectionCommission", list);
                    if (SqlHelper.ExecteNonQuery(spe) > 0)
                    {
                        MsgBox.ShowOK();
                        this.Close();
                        return;
                    }    

                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }

            }

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", Guid.NewGuid().ToString()));
                list.Add(new SqlPara("type", "新增"));
                list.Add(new SqlPara("CauseName", Cause.EditValue));
                list.Add(new SqlPara("AreaName", Area.EditValue));
                list.Add(new SqlPara("WebName", web.EditValue));
                list.Add(new SqlPara("month", month.EditValue));
                list.Add(new SqlPara("Revenue", Revenue.EditValue));
                list.Add(new SqlPara("RegistrationDate", RegistrationDate.EditValue));
                list.Add(new SqlPara("Registrant", Registrant.EditValue));
                list.Add(new SqlPara("DataDepartment", DataDepartment.EditValue));
                list.Add(new SqlPara("CostType", CostType.EditValue));
                list.Add(new SqlPara("Amount", Amount.EditValue));
                list.Add(new SqlPara("abstract", abstracts.EditValue));
             
                list.Add(new SqlPara("BillNo", BillNo.Text.Trim()));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_ADD_CollectionCommission", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    MsgBox.ShowOK();
                    this.Close();
                    return;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        private bool vidvaliNum(Object num)
        {
            try {
                decimal money = decimal.Parse(num.ToString());
                return true;
            }
            catch (Exception e) {
                return false;
            }
        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void Cause_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            CommonClass.SetArea(Area, Cause.Text);
            CommonClass.SetCauseWeb(web, Cause.Text, Area.Text);
        }

        private void Area_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(web, Cause.Text, Area.Text);
        }

        private void month_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}