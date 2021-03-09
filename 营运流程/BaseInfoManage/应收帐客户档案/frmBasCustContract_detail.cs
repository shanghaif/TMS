using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
namespace ZQTMS.UI
{
    public partial class frmBasCustContract_detail : BaseForm
    {
        public DataRow dr = null;
        public frmBasCustContract_detail()
        {
            InitializeComponent();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                if (string.IsNullOrEmpty(basCustContractID.Text.Trim()))
                {
                    Guid gid = Guid.NewGuid();
                    list.Add(new SqlPara("basCustContractID", gid));
                    basCustContractID.Text = gid.ToString();
                }
                else
                    list.Add(new SqlPara("basCustContractID", basCustContractID.Text.Trim()));
                list.Add(new SqlPara("ShortName", ShortName.Text.Trim()));
                list.Add(new SqlPara("FullName", FullName.Text.Trim()));
                list.Add(new SqlPara("CpyLegalPerson", CpyLegalPerson.Text.Trim()));
                list.Add(new SqlPara("RegistCapital", RegistCapital.Text.Trim()));
                list.Add(new SqlPara("RegistAdd", RegistAdd.Text.Trim()));
                list.Add(new SqlPara("RunAdd", RunAdd.Text.Trim()));
                list.Add(new SqlPara("CustLinkName", CustLinkName.Text.Trim()));
                list.Add(new SqlPara("SendCustTel", SendCustTel.Text.Trim()));
                list.Add(new SqlPara("SendCustMobile", SendCustMobile.Text.Trim()));
                list.Add(new SqlPara("CheckBillLinkName", CheckBillLinkName.Text.Trim()));
                list.Add(new SqlPara("CheckBillTel", CheckBillTel.Text.Trim()));
                list.Add(new SqlPara("ContractNo", ContractNo.Text.Trim()));

                if (BeginDate.Text.Trim() == "")
                    list.Add(new SqlPara("BeginDate", DBNull.Value));
                else
                    list.Add(new SqlPara("BeginDate", BeginDate.DateTime));

                if (EndDate.Text.Trim() == "")
                    list.Add(new SqlPara("EndDate", DBNull.Value));
                else
                    list.Add(new SqlPara("EndDate", EndDate.DateTime));

                if (ContractDate.Text.Trim() == "")
                    list.Add(new SqlPara("ContractDate", DBNull.Value));
                else
                    list.Add(new SqlPara("ContractDate", ContractDate.DateTime));

                list.Add(new SqlPara("crDate", DateTime.Now.ToString()));
                list.Add(new SqlPara("ApplyName", ApplyName.Text.Trim()));
                list.Add(new SqlPara("UnitDeptName", UnitDeptName.Text.Trim()));
                list.Add(new SqlPara("CreditDays", ConvertType.ToDecimal(CreditDays.Text)));
                list.Add(new SqlPara("CreditLimit", ConvertType.ToDecimal(CreditLimit.Text)));
                list.Add(new SqlPara("PayCycle", PayCycle.Text.Trim()));
                list.Add(new SqlPara("MonthlyDelayDays", ConvertType.ToDecimal(MonthlyDelayDays.Text)));
                list.Add(new SqlPara("MonthlyDelayLimit", ConvertType.ToDecimal(MonthlyDelayLimit.Text)));
                list.Add(new SqlPara("ReturnBillDelayDays", ConvertType.ToInt32(ReturnBillDelayDays.Text)));
                list.Add(new SqlPara("Operator", Operator.Text.Trim()));
                list.Add(new SqlPara("CustTypeValue", CustTypeValue.Text.Trim()));
                list.Add(new SqlPara("MonthSiteName", MonthSiteName.Text.Trim()));
                list.Add(new SqlPara("MonthWebName", MonthWebName.Text.Trim()));
                list.Add(new SqlPara("GatheringMan",txtGatheringMan.Text.Trim()));//zxw 2016-12-28
                list.Add(new SqlPara("TaxNo", TaxNo.Text.Trim()));//hj 2018-03-16
                list.Add(new SqlPara("UserBack", UserBack.Text.Trim()));//hj 2018-03-16
                list.Add(new SqlPara("UserAccount", UserAccount.Text.Trim()));//hj 2018-03-16
                list.Add(new SqlPara("FetchToOwe", FetchToOwe.Checked == true ? 1 : 0));//hj 20181101提付改欠管控

                list.Add(new SqlPara("AttriWeb", cboAttriWeb.Text.Trim()));//归属网点
                list.Add(new SqlPara("Attribution", txtAttribution.Text.Trim()));//归属人
                list.Add(new SqlPara("AttriPhone", txtAttriPhone.Text.Trim()));//归属人电话

                list.Add(new SqlPara("PaymentMethod", PaymentMethod.Text.Trim()));//付款方式
                list.Add(new SqlPara("Salesman", textEdit1.Text.Trim()));//业务员--PK2019.3.2
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BASCUSTCONTRACT", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();

                    PaymentMethod.Text = "";
                    ShortName.Text = "";
                    FullName.Text = "";
                    CpyLegalPerson.Text = "";
                    RegistCapital.Text = "";
                    RegistAdd.Text = "";
                    RunAdd.Text = "";
                    CustLinkName.Text = "";
                    SendCustTel.Text = "";
                    SendCustMobile.Text = "";
                    CheckBillLinkName.Text = "";
                    CheckBillTel.Text = "";
                    ContractNo.Text = "";
                    BeginDate.Text = "";
                    EndDate.Text = "";
                    ContractDate.Text = "";
                    CrDate.Text = "";
                    ApplyName.Text = "";
                    UnitDeptName.Text = "";
                    CreditDays.Text = "";
                    CreditLimit.Text = "";
                    PayCycle.Text = "";
                    MonthlyDelayDays.Text = "";
                    MonthlyDelayLimit.Text = "";
                    ReturnBillDelayDays.Text = "";
                    Operator.Text = "";
                    CustTypeValue.Text = "";
                    MonthSiteName.Text = "";
                    MonthWebName.Text = "";
                    txtGatheringMan.Text = ""; 
                    TaxNo.Text = "";
                    UserBack.Text = "";
                    UserAccount.Text = "";
                    FetchToOwe.Checked = false;
                    textEdit1.Text = "";
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void frmBasCustContract_detail_Load(object sender, EventArgs e)
        {
            CommonClass.SetSite(MonthSiteName, false);
            CrDate.DateTime = CommonClass.gcdate;
            UnitDeptName.Text = CommonClass.UserInfo.WebName;
            ApplyName.Text = CommonClass.UserInfo.UserName;
            CommonClass.SetWeb(cboAttriWeb, true);
            try
            {
                if (dr == null) return;
                label20.Text = "修改日期";
                basCustContractID.EditValue = dr["basCustContractID"];
                PaymentMethod.EditValue = dr["PaymentMethod"];
                ShortName.EditValue = dr["ShortName"];
                FullName.EditValue = dr["FullName"];
                CpyLegalPerson.EditValue = dr["CpyLegalPerson"];
                RegistCapital.EditValue = dr["RegistCapital"];
                RegistAdd.EditValue = dr["RegistAdd"];
                RunAdd.EditValue = dr["RunAdd"];
                CustLinkName.EditValue = dr["CustLinkName"];
                SendCustTel.EditValue = dr["SendCustTel"];
                SendCustMobile.EditValue = dr["SendCustMobile"];
                CheckBillLinkName.EditValue = dr["CheckBillLinkName"];
                CheckBillTel.EditValue = dr["CheckBillTel"];
                ContractNo.EditValue = dr["ContractNo"];
                BeginDate.EditValue = dr["BeginDate"];
                EndDate.EditValue = dr["EndDate"];
                ContractDate.EditValue = dr["ContractDate"];
                CrDate.EditValue = dr["modifiedDate"];
                ApplyName.EditValue = dr["ApplyName"];
                UnitDeptName.EditValue = dr["UnitDeptName"];
                CreditDays.EditValue = dr["CreditDays"];
                CreditLimit.EditValue = dr["CreditLimit"];
                PayCycle.EditValue = dr["PayCycle"];
                MonthlyDelayDays.EditValue = dr["MonthlyDelayDays"];
                MonthlyDelayLimit.EditValue = dr["MonthlyDelayLimit"];
                ReturnBillDelayDays.EditValue = dr["ReturnBillDelayDays"];
                Operator.EditValue = dr["Operator"];
                CustTypeValue.EditValue = dr["CustTypeValue"];
                MonthSiteName.EditValue = dr["MonthSiteName"];
                MonthWebName.EditValue = dr["MonthWebName"];
                txtGatheringMan.EditValue = dr["GatheringMan"];//zxw 2016-12-28
                TaxNo.EditValue = dr["TaxNo"];
                UserBack.EditValue = dr["UserBack"];
                UserAccount.EditValue = dr["UserAccount"];
                FetchToOwe.Checked = dr["FetchToOwe"].ToString() == "是" ? FetchToOwe.Checked = true : FetchToOwe.Checked = false;//hj20181031
                if (CrDate.Text.Trim() == "")
                    CrDate.DateTime = CommonClass.gcdate;
                textEdit1.EditValue = dr["Salesman"];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void MonthSiteName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CommonClass.SetWeb(MonthWebName, MonthSiteName.Text, false);
        }

        public String webNames { get; set; }
        private void buttonEdit1_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (MonthSiteName.Text == "")
            {
                MsgBox.ShowOK("请选择月结站点");
                return;
            }
            frmMonthWebNameEdit frm = new frmMonthWebNameEdit(MonthSiteName.Text, MonthWebName.Text);
            frm.Owner = this;
            frm.ShowDialog();
            if (frm.isModify)
            {
                this.MonthWebName.Text = webNames;
            }
        }
    }
}