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
namespace ZQTMS.UI.BaseInfoManage.客户资料
{
    public partial class frmMinChargeAdd : ZQTMS.Tool.BaseForm
    {
        public DataRow dr = null;
        public frmMinChargeAdd()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ConsignorCompany.Text))
                {
                    MsgBox.ShowOK("发货单位不能为空！");
                    return;
                }
                if (string.IsNullOrEmpty(ConsignorName.Text))
                {

                    MsgBox.ShowOK("发货联系人不能为空");
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", dr == null ? Guid.NewGuid() : dr["ID"]));
                list.Add(new SqlPara("ConsignorCompany", ConsignorCompany.Text.Trim()));
                list.Add(new SqlPara("ConsignorName", ConsignorName.Text.ToString()));
                list.Add(new SqlPara("ConsignorCellPhone", ConsignorCellPhone.Text.ToString()));
                list.Add(new SqlPara("ConsignorPhone", ConsignorPhone.Text.ToString()));
                list.Add(new SqlPara("BelongWeb", BelongWeb.Text.ToString()));
                list.Add(new SqlPara("PaymentMode", PaymentMode.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_MinCostCharge", list);//保存
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    this.Close();
                }
            }
            catch (Exception)
            {
                
                throw;
            }

           
        }

        private void frmMinChargeAdd_Load(object sender, EventArgs e)
        {

            CommonClass.FormSet(this);
            CommonClass.SetWeb(BelongWeb, false);
            PaymentMode.Properties.Items.Add("提付");
            PaymentMode.Properties.Items.Add("月结");
            PaymentMode.Properties.Items.Add("回单付");
            PaymentMode.Properties.Items.Add("短欠");
            PaymentMode.Properties.Items.Add("现付");
            PaymentMode.Properties.Items.Add("货到前付");
            PaymentMode.Properties.Items.Add("全部");
            BelongWeb.Text = CommonClass.UserInfo.WebName;
            if (dr != null)
            {
                this.ConsignorCompany.EditValue = dr["ConsignorCompany"];
                this.ConsignorName.EditValue = dr["ConsignorName"];
                this.ConsignorCellPhone.EditValue = dr["ConsignorCellPhone"];
                this.ConsignorPhone.EditValue = dr["ConsignorPhone"];
                this.BelongWeb.Text = dr["BelongWeb"].ToString();
                this.PaymentMode.EditValue = dr["PaymentMode"];
   



            }
        }
       
    }
}
