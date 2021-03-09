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

namespace ZQTMS.UI
{
    public partial class frmAddCusData : BaseForm
    {
        public frmAddCusData()
        {
            InitializeComponent();
        }
        public DataRow dr = null;

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddCusData_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.SetCause(BelongCause,false);
            CommonClass.SetSite(BelongSite,false);
            CommonClass.SetUser(Salesman, "全部");

            string[] PaymentModeList = CommonClass.Arg.PaymentMode.Split(',');
            if (PaymentModeList.Length > 0)
            {
                for (int i = 0; i < PaymentModeList.Length; i++)
                {
                    PayWay.Properties.Items.Add(PaymentModeList[i]);
                }
                PayWay.SelectedIndex = 0;
            }
            string[] CustomTypeModeList = CommonClass.Arg.CustomType.Split(',');
            if (CustomTypeModeList.Length > 0)
            {
                for (int i = 0; i < CustomTypeModeList.Length; i++)
                {
                    CusType.Properties.Items.Add(CustomTypeModeList[i]);
                }
                CusType.SelectedIndex = 0;
            }
            string[] CustomTagModeList = CommonClass.Arg.CustomTag.Split(',');
            if (CustomTagModeList.Length > 0)
            {
                for (int i = 0; i < CustomTagModeList.Length; i++)
                {
                    CusTag.Properties.Items.Add(CustomTagModeList[i]);
                }
                CusTag.SelectedIndex = 0;
            }
            string[] OfenVarietiesModeList = CommonClass.Arg.OfenVarieties.Split(',');
            if (OfenVarietiesModeList.Length > 0)
            {
                for (int i = 0; i < OfenVarietiesModeList.Length; i++)
                {
                    AlwaysSend.Properties.Items.Add(OfenVarietiesModeList[i]);
                }
                AlwaysSend.SelectedIndex = 0;
            }
            string[] PackagModeList = CommonClass.Arg.Packag.Split(',');
            if (PackagModeList.Length > 0)
            {
                for (int i = 0; i < PackagModeList.Length; i++)
                {
                    GoodsPackag.Properties.Items.Add(PackagModeList[i]);
                }
                GoodsPackag.SelectedIndex = 0;
            }
            string[] StowagePlanModeList = CommonClass.Arg.StowagePlan.Split(',');
            if (StowagePlanModeList.Length > 0)
            {
                for (int i = 0; i < StowagePlanModeList.Length; i++)
                {
                    LoadRequir.Properties.Items.Add(StowagePlanModeList[i]);
                }
                LoadRequir.SelectedIndex = 0;
            }
            string[] SendRequirModeList = CommonClass.Arg.SendRequir.Split(',');
            if (SendRequirModeList.Length > 0)
            {
                for (int i = 0; i < SendRequirModeList.Length; i++)
                {
                    SendRequir.Properties.Items.Add(SendRequirModeList[i]);
                }
                SendRequir.SelectedIndex = 0;
            }
            string[] MiddleRequirModeList = CommonClass.Arg.MiddleRequir.Split(',');
            if (MiddleRequirModeList.Length > 0)
            {
                for (int i = 0; i < MiddleRequirModeList.Length; i++)
                {
                    MidRequir.Properties.Items.Add(MiddleRequirModeList[i]);
                }
                MidRequir.SelectedIndex = 0;
            }
            string[] DenominatedTypeModeList = CommonClass.Arg.DenominatedType.Split(',');
            if (DenominatedTypeModeList.Length > 0)
            {
                for (int i = 0; i < DenominatedTypeModeList.Length; i++)
                {
                    CusFeeType.Properties.Items.Add(DenominatedTypeModeList[i]);
                }
                CusFeeType.SelectedIndex = 0;
            }



            if (dr != null) 
            {
               //CustID.EditValue = dr["CustID"];
                CustNo.EditValue = dr["CustNo"];
                CusName.EditValue = dr["CusName"];
                CusType.EditValue = dr["CusType"];
                CusTag.EditValue = dr["CusTag"];
                ContactMan.EditValue = dr["ContactMan"];
                ContactPhone.EditValue = dr["ContactPhone"];
                ContactCellPhone.EditValue = dr["ContactCellPhone"];
                CusEmail.EditValue = dr["CusEmail"];
                CusAddress.EditValue = dr["CusAddress"];
                PayWay.EditValue = dr["PayWay"];
                CooperateDate.EditValue = dr["CooperateDate"];
                BankName.EditValue = dr["BankName"];
                BankUserName.EditValue = dr["BankUserName"];
                BankAdd.EditValue = dr["BankAdd"];
                BankAccount.EditValue = dr["BankAccount"];
                BelongSite.EditValue = dr["BelongSite"];
                BelongWeb.EditValue = dr["BelongWeb"];
                BelongArea.EditValue = dr["BelongArea"];
                AlwaysSend.EditValue = dr["AlwaysSend"];
                GoodsPackag.EditValue = dr["GoodsPackag"];
                LoadRequir.EditValue = dr["LoadRequir"];
                SendRequir.EditValue = dr["SendRequir"];
                MidRequir.EditValue = dr["MidRequir"];
                Salesman.EditValue = dr["Salesman"];
                CusRemark.EditValue = dr["CusRemark"];
                CusFeeType.EditValue = dr["CusFeeType"];
                CusDiscount.EditValue = dr["CusDiscount"];
                BelongCause.EditValue = dr["BelongCause"];
                BelongDep.EditValue = dr["BelongDep"];
                CusRemarkInfo.EditValue = dr["CusRemarkInfo"];
            }
        }

        private void SiteAll()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASSITE", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0)
                {
                    return;
                }
                BelongSite.Properties.Items.Clear();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    BelongSite.Properties.Items.Add(ds.Tables[0].Rows[i]["SiteName"]);
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        private void getWeb(string SiteName)
        {
            try
            {
                 List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASWEB", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0)
                {
                    return;
                }
                DataRow[] dtRows = ds.Tables[0].Select("SiteName='" + SiteName + "'");
                BelongWeb.Properties.Items.Clear();
                for (int i = 0; i < dtRows.Length; i++)
                {
                    BelongWeb.Properties.Items.Add(dtRows[i]["WebName"]);
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void AreaAll()
        {
            try
            {
                 List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASAREA", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds != null && ds.Tables.Count != 0)
                {
                    if (ds != null && ds.Tables.Count != 0)
                    {
                        BelongArea.Properties.Items.Clear();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)

                            BelongArea.Properties.Items.Add(ds.Tables[0].Rows[i]["AreaName"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (!checkdata())
            {
                MsgBox.ShowError("单位,联系人,电话,手机为必填项,请检查是否为空！");
                return; 
            }
            if (CusFeeType.Text.Trim() == "折扣价" && CusDiscount.Text.Trim() == "")
            { 
                MsgBox.ShowError("请输入折扣！"); 
                return; 
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("CustID", dr == null ? Guid.NewGuid() : dr["CustID"]));
                list.Add(new SqlPara("CustNo", CustNo.Text.Trim()));
                list.Add(new SqlPara("CusName", CusName.Text.Trim()));
                list.Add(new SqlPara("CusType", CusType.Text.Trim()));
                list.Add(new SqlPara("CusTag", CusTag.Text.Trim()));
                list.Add(new SqlPara("ContactMan", ContactMan.Text.Trim()));
                list.Add(new SqlPara("ContactPhone", ContactPhone.Text.Trim()));
                list.Add(new SqlPara("ContactCellPhone", ContactCellPhone.Text.Trim()));
                list.Add(new SqlPara("CusEmail", CusEmail.Text.Trim()));
                list.Add(new SqlPara("CusAddress", CusAddress.Text.Trim()));
                list.Add(new SqlPara("PayWay", PayWay.Text.Trim()));
                list.Add(new SqlPara("CooperateDate", CooperateDate.Text.Trim()));
                list.Add(new SqlPara("BankName", BankName.Text.Trim()));
                list.Add(new SqlPara("BankUserName", BankUserName.Text.Trim()));
                list.Add(new SqlPara("BankAdd", BankAdd.Text.Trim()));
                list.Add(new SqlPara("BankAccount", BankAccount.Text.Trim()));
                list.Add(new SqlPara("BelongSite", BelongSite.Text.Trim()));
                list.Add(new SqlPara("BelongWeb", BelongWeb.Text.Trim()));
                list.Add(new SqlPara("BelongArea", BelongArea.Text.Trim()));
                list.Add(new SqlPara("AlwaysSend", AlwaysSend.Text.Trim()));
                list.Add(new SqlPara("GoodsPackag", GoodsPackag.Text.Trim()));
                list.Add(new SqlPara("LoadRequir", LoadRequir.Text.Trim()));
                list.Add(new SqlPara("SendRequir", SendRequir.Text.Trim()));
                list.Add(new SqlPara("MidRequir", MidRequir.Text.Trim()));
                list.Add(new SqlPara("Salesman", Salesman.Text.Trim()));
                list.Add(new SqlPara("CusRemark", CusRemark.Text.Trim()));
                list.Add(new SqlPara("CusFeeType", CusFeeType.Text.Trim()));
                list.Add(new SqlPara("CusDiscount", CusDiscount.Text.Trim()));
                list.Add(new SqlPara("CusRemarkInfo", CusRemarkInfo.Text.Trim()));//hj20181031增加备注信息字段，开单的时候关联

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BASCUST", list);
                if(SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    this.Close();
                }
                  }
            catch(Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void BelongSite_TextChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(BelongWeb,BelongSite.Text.Trim(),false);
            BelongWeb.SelectedIndex = 0;
        }

        private void BelongCause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(BelongArea,BelongCause.Text.Trim(),false);
            BelongArea.SelectedIndex = 0;
        }

        private void BelongArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(BelongDep, BelongArea.Text.Trim(),false);
            BelongDep.SelectedIndex = 0;
        }

        private void simpleButton2_Click_1(object sender, EventArgs e)
        {
            if (dr == null || dr["CustNo"] == null)
            {
                MsgBox.ShowOK("客户编号不存在！");
                return;
            }
            frmSenderFreightPrice frm = new frmSenderFreightPrice();
            frm.CustNo = dr["CustNo"].ToString();
            frm.ShowDialog();


        }

        private void CusFeeType_EditValueChanged(object sender, EventArgs e)
        {
            if (CusFeeType.Text.Trim() == "折扣价")
            
                CusDiscount.Properties.ReadOnly = false;
            else
                CusDiscount.Properties.ReadOnly = true;
        }
        private bool checkdata() 
        {
            if(CusName.Text.Trim() == "" || ContactMan.Text.Trim() == ""|| ContactPhone.Text.Trim() == ""|| ContactCellPhone.Text.Trim() == "")
                return false;
            else
                return true;

        }
    }
}
