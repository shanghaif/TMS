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
    public partial class JMfrmFetchSign : BaseForm
    {
        public JMfrmFetchSign()
        {
            InitializeComponent();
        }
        sysUserInfoInfo UserInfo = new sysUserInfoInfo();
        public int isFetch = 0;

        //运单信息
        public DataRow dr = null;
        private void JMfrmFetchSign_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            //加载运单数据
            if (dr != null)
            {
                BillNo.EditValue = dr["BillNO"];
                StartSite.EditValue = dr["StartSite"];
                DestinationSite.EditValue = dr["DestinationSite"];
                CusOderNo.EditValue = dr["CusOderNo"];
                BillDate.EditValue = dr["BillDate"];
                ConsignorName.EditValue = dr["ConsignorName"];
                ConsignorCellPhone.EditValue = dr["ConsignorCellPhone"];
                ConsignorCompany.EditValue = dr["ConsignorCompany"];
                ConsigneeName.EditValue = dr["ConsigneeName"];
                ConsigneeCellPhone.EditValue = dr["ConsigneeCellPhone"];
                ConsigneeCompany.EditValue = dr["ConsigneeCompany"];
                Varieties.EditValue = dr["Varieties"];
                Num.EditValue = dr["Num"];
                FetchPayVerifState.Checked = ConvertType.ToInt32(dr["FetchPayVerifState"]) == 1;
                FetchToMonth.Checked = ConvertType.ToInt32(dr["FetchToMonth"]) == 1;
                FetchPay.EditValue = dr["FetchPay"];
                ActualFreight.EditValue = dr["ActualFreight"];
                OperateState.Checked = ConvertType.ToInt32(dr["OperateState"]) == 1;
                OperateType.EditValue = dr["OperateType"];
                OperateTime.EditValue = dr["OperateTime"];
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SignNO", Guid.NewGuid()));
                list.Add(new SqlPara("BillNO", BillNo.Text.Trim()));
                list.Add(new SqlPara("SignType", "提货签收"));
                list.Add(new SqlPara("SignMan", SignMan.Text.Trim()));
                list.Add(new SqlPara("SignManCardID", SignManCardID.Text.Trim()));
                list.Add(new SqlPara("AgentMan", AgentMan.Text.Trim()));
                list.Add(new SqlPara("AgentCardId", AgentCardId.Text.Trim()));
                list.Add(new SqlPara("SignDate", SignDate.Text.Trim()));
                list.Add(new SqlPara("SignDesc", SignDesc.Text.Trim()));
                list.Add(new SqlPara("SignOperator", UserInfo.UserName));
                list.Add(new SqlPara("SignSite", UserInfo.SiteName));
                list.Add(new SqlPara("SignWeb", UserInfo.WebName));
                list.Add(new SqlPara("SignContent", SignContent.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLSIGN", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void btnSignUpload_Click(object sender, EventArgs e)
        {

        }

        private void btnSavePrint_Click(object sender, EventArgs e)
        {
            btnSave_Click(sender, e);
            print();
        }
        private void print()
        {

        }
    }
}