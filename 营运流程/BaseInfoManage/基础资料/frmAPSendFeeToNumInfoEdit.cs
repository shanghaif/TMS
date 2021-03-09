using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class frmAPSendFeeToNumInfoEdit : BaseForm
    {
        public DataRow dr = null;
        public frmAPSendFeeToNumInfoEdit()
        {
            InitializeComponent();
        }

        private void frmAPSendFeeToNumInfoEdit_Load(object sender, EventArgs e)
        {
            txtCompanyId.Text = CommonClass.UserInfo.companyid;
            //GetTrader();
            if (dr != null)
            {
                txtBillNo.EditValue = dr["BillNo"];
                txtOfferName.EditValue = dr["OfferName"];
                txtCommonCarrier.EditValue = dr["CommonCarrier"];
                txtTransferFee.EditValue = dr["TransferFee"];
                txtContractBegin.EditValue = dr["ContractBegin"];
                txtContractEnd.EditValue = dr["ContractEnd"];
                txtOrigin.EditValue = dr["Origin"];
                txtDestination.EditValue = dr["Destination"];
                txtMerchant.EditValue = dr["Merchant"];
                txtCombine.EditValue = dr["Combine"];
                txtSalesProject.EditValue = dr["SalesProject"];
                txtMinimumCharge.EditValue = dr["MinimumCharge"];
                txtRemarks.EditValue = dr["Remarks"];
                txtBeginNumOne.EditValue = dr["BeginNumOne"];
                txtEndNumOne.EditValue = dr["EndNumOne"];
                txtNumOnePrice.EditValue = dr["NumOnePrice"];
                txtBeginNumTwo.EditValue = dr["BeginNumTwo"];
                txtEndNumTwo.EditValue = dr["EndNumTwo"];
                txtNumTwoPrice.EditValue = dr["NumTwoPrice"];
                txtBeginNumThree.EditValue = dr["BeginNumThree"];
                txtEndNumThree.EditValue = dr["EndNumThree"];
                txtNumThreePrice.EditValue = dr["NumThreePrice"];
                txtBeginNumFour.EditValue = dr["BeginNumFour"];
                txtEndNumFour.EditValue = dr["EndNumFour"];
                txtNumFourPrice.EditValue = dr["NumFourPrice"];
                txtBeginNumFive.EditValue = dr["BeginNumFive"];
                txtEndNumFive.EditValue = dr["EndNumFive"];
                txtNumFivePrice.EditValue = dr["NumFivePrice"];
                txtCompanyId.EditValue = dr["CompanyId"];
                txtOperator.EditValue = dr["Operator"];
                txtOperationTime.EditValue = dr["OperationTime"];
                txtCompanyId.Enabled = false;
            }
        }

        //public void GetTrader()
        //{
        //    try
        //    {
        //        SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_USP_GET_traderid");
        //        DataSet ds = SqlHelper.GetDataSet(sps);
        //        if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
        //        {
        //            return;
        //        }
        //        foreach (DataRow dr in ds.Tables[0].Rows)
        //        {
        //            txtMerchant.Properties.Items.Add(dr[0]);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MsgBox.ShowException(ex);
        //    }
        //}

        //保存
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Control item in this.Controls)
                {
                    if (item.GetType() == typeof(TextEdit) || item.GetType() == typeof(DateEdit))
                    {
                        if (item.Text.Trim() == "")
                        {
                            MsgBox.ShowOK("每一项都必须填写!");
                            return;
                        }
                    }
                }

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("id", dr == null ? Guid.NewGuid() : dr["numID"]));
                list.Add(new SqlPara("BillNo", txtBillNo.Text.Trim()));
                list.Add(new SqlPara("OfferName", txtOfferName.Text.Trim()));
                list.Add(new SqlPara("CommonCarrier", txtCommonCarrier.Text.Trim()));
                list.Add(new SqlPara("TransferFee", txtTransferFee.Text.Trim()));
                list.Add(new SqlPara("ContractBegin", txtContractBegin.Text.Trim()));
                list.Add(new SqlPara("ContractEnd", txtContractEnd.Text.Trim()));
                list.Add(new SqlPara("Origin", txtOrigin.Text.Trim()));
                list.Add(new SqlPara("Destination", txtDestination.Text.Trim()));
                list.Add(new SqlPara("Merchant", txtMerchant.Text.Trim()));
                list.Add(new SqlPara("Combine", txtCombine.Text.Trim()));
                list.Add(new SqlPara("SalesProject", txtSalesProject.Text.Trim()));
                list.Add(new SqlPara("MinimumCharge", txtMinimumCharge.Text.Trim()));
                list.Add(new SqlPara("Remarks", txtRemarks.Text.Trim()));
                list.Add(new SqlPara("BeginNumOne", txtBeginNumOne.Text.Trim()));
                list.Add(new SqlPara("EndNumOne", txtEndNumOne.Text.Trim()));
                list.Add(new SqlPara("NumOnePrice", txtNumOnePrice.Text.Trim()));
                list.Add(new SqlPara("BeginNumTwo", txtBeginNumTwo.Text.Trim()));
                list.Add(new SqlPara("EndNumTwo", txtEndNumTwo.Text.Trim()));
                list.Add(new SqlPara("NumTwoPrice", txtNumTwoPrice.Text.Trim()));
                list.Add(new SqlPara("BeginNumThree", txtBeginNumThree.Text.Trim()));
                list.Add(new SqlPara("EndNumThree", txtEndNumThree.Text.Trim()));
                list.Add(new SqlPara("NumThreePrice", txtNumThreePrice.Text.Trim()));
                list.Add(new SqlPara("BeginNumFour", txtBeginNumFour.Text.Trim()));
                list.Add(new SqlPara("EndNumFour", txtEndNumFour.Text.Trim()));
                list.Add(new SqlPara("NumFourPrice", txtNumFourPrice.Text.Trim()));
                list.Add(new SqlPara("BeginNumFive", txtBeginNumFive.Text.Trim()));
                list.Add(new SqlPara("EndNumFive", txtEndNumFive.Text.Trim()));
                list.Add(new SqlPara("NumFivePrice", txtNumFivePrice.Text.Trim()));
                list.Add(new SqlPara("Operator", txtOperator.Text.Trim()));
                txtOperationTime.Text = DateTime.Now.ToString();
                list.Add(new SqlPara("OperationTime ", txtOperationTime.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BasAPSendFeeToNum", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    frmAPSendFeeToNumInfo ower = (frmAPSendFeeToNumInfo)this.Owner;
                    ower.updata();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //退出
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
