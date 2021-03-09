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
    public partial class frmAPFeeToWeightInfoEdit : BaseForm
    {
        public DataRow dr = null;
        public frmAPFeeToWeightInfoEdit()
        {
            InitializeComponent();
        }

        private void frmAPFeeToWeightInfoEdit_Load(object sender, EventArgs e)
        {
            txtCompanyId.Text = CommonClass.UserInfo.companyid;
            //GetTrader();
            if (dr != null)
            {
                txtBillNo.EditValue = dr["BillNo"];
                txtOfferName.EditValue = dr["OfferName"];
                txtCommonCarrier.EditValue = dr["CommonCarrier"];
                txtTransferDeliverFee.EditValue = dr["TransferDeliverFee"];
                txtContractBegin.EditValue = dr["ContractBegin"];
                txtContractEnd.EditValue = dr["ContractEnd"];
                txtOrigin.EditValue = dr["Origin"];
                txtDestination.EditValue = dr["Destination"];
                txtMerchant1.EditValue = dr["Merchant1"];
                txtMerchant2.EditValue = dr["Merchant2"];
                txtFirstNumWeight.EditValue = dr["FirstNumWeight"];
                txtPrice.EditValue = dr["Price"];
                txtMinimumCharge.EditValue = dr["MinimumCharge"];
                txtRemarks.EditValue = dr["Remarks"];
                txtBeginWeightOne.EditValue = dr["BeginWeightOne"];
                txtEndWeightOne.EditValue = dr["EndWeightOne"];
                txtWeightOnePrice.EditValue = dr["WeightOnePrice"];
                txtBeginWeightTwo.EditValue = dr["BeginWeightTwo"];
                txtEndWeightTwo.EditValue = dr["EndWeightTwo"];
                txtWeightTwoPrice.EditValue = dr["WeightTwoPrice"];
                txtBeginWeightThree.EditValue = dr["BeginWeightThree"];
                txtEndWeightThree.EditValue = dr["EndWeightThree"];
                txtWeightThreePrice.EditValue = dr["WeightThreePrice"];
                txtBeginWeightFour.EditValue = dr["BeginWeightFour"];
                txtEndWeightFour.EditValue = dr["EndWeightFour"];
                txtWeightFourPrice.EditValue = dr["WeightFourPrice"];
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
        //            txtMerchant1.Properties.Items.Add(dr[0]);
        //            txtMerchant2.Properties.Items.Add(dr[0]);
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
                list.Add(new SqlPara("id", dr == null ? Guid.NewGuid() : dr["WeightID"]));
                list.Add(new SqlPara("BillNo", txtBillNo.Text.Trim()));
                list.Add(new SqlPara("OfferName", txtOfferName.Text.Trim()));
                list.Add(new SqlPara("CommonCarrier", txtCommonCarrier.Text.Trim()));
                list.Add(new SqlPara("TransferDeliverFee", txtTransferDeliverFee.Text.Trim()));
                list.Add(new SqlPara("ContractBegin", txtContractBegin.Text.Trim()));
                list.Add(new SqlPara("ContractEnd", txtContractEnd.Text.Trim()));
                list.Add(new SqlPara("Origin", txtOrigin.Text.Trim()));
                list.Add(new SqlPara("Destination", txtDestination.Text.Trim()));
                list.Add(new SqlPara("Merchant1", txtMerchant1.Text.Trim()));
                list.Add(new SqlPara("Merchant2", txtMerchant2.Text.Trim()));
                list.Add(new SqlPara("FirstNumWeight", txtFirstNumWeight.Text.Trim()));
                list.Add(new SqlPara("Price", txtPrice.Text.Trim()));
                list.Add(new SqlPara("MinimumCharge", txtMinimumCharge.Text.Trim()));
                list.Add(new SqlPara("Remarks", txtRemarks.Text.Trim()));
                list.Add(new SqlPara("BeginWeightOne", txtBeginWeightOne.Text.Trim()));
                list.Add(new SqlPara("EndWeightOne", txtEndWeightOne.Text.Trim()));
                list.Add(new SqlPara("WeightOnePrice", txtWeightOnePrice.Text.Trim()));
                list.Add(new SqlPara("BeginWeightTwo", txtBeginWeightTwo.Text.Trim()));
                list.Add(new SqlPara("EndWeightTwo", txtEndWeightTwo.Text.Trim()));
                list.Add(new SqlPara("WeightTwoPrice", txtWeightTwoPrice.Text.Trim()));
                list.Add(new SqlPara("BeginWeightThree", txtBeginWeightThree.Text.Trim()));
                list.Add(new SqlPara("EndWeightThree", txtEndWeightThree.Text.Trim()));
                list.Add(new SqlPara("WeightThreePrice", txtWeightThreePrice.Text.Trim()));
                list.Add(new SqlPara("BeginWeightFour", txtBeginWeightFour.Text.Trim()));
                list.Add(new SqlPara("EndWeightFour", txtEndWeightFour.Text.Trim()));
                list.Add(new SqlPara("WeightFourPrice", txtWeightFourPrice.Text.Trim()));
                list.Add(new SqlPara("Operator", txtOperator.Text.Trim()));
                txtOperationTime.Text = DateTime.Now.ToString();
                list.Add(new SqlPara("OperationTime ", txtOperationTime.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BasAPFeeToWeight", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    frmAPFeeToWeightInfo ower = (frmAPFeeToWeightInfo)this.Owner;
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
