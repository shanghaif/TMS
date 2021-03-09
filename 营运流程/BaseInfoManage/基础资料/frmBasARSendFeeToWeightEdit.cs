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
    public partial class frmBasARSendFeeToWeightEdit : BaseForm
    {
        public DataRow dr = null;       

        public frmBasARSendFeeToWeightEdit()
        {
            InitializeComponent();
        }

        //load
        private void frmBasARSendFeeToWeightEdit_Load(object sender, EventArgs e)
        {
            //GetTravellingTrader();
            if (dr != null)
            {
                txtBillNo.EditValue = dr["BillNo"];
                OfferName.EditValue = dr["OfferName"];
                Merchant.EditValue = dr["Merchant"];
                //FeeAccount.EditValue = dr["FreightCharge"];
                //FeeAccount.EditValue = dr["FeeAccount"];
                FreightCharge.EditValue = dr["FreightCharge"];
                ContractBegin.EditValue = dr["ContractBegin"];
                ContractEnd.EditValue = dr["ContractEnd"];
                Origin.EditValue = dr["Origin"];
                Destination.EditValue = dr["Destination"];
                SaleProject.EditValue = dr["SaleProject"];
                MinimumCharge.EditValue = dr["MinimumCharge"];
                Remark.Text = dr["Remark"].ToString();
                BeginWeightOne.EditValue = dr["BeginWeightOne"];
                EndWeightOne.EditValue = dr["EndWeightOne"];
                WeightOnePrice.EditValue = dr["WeightOnePrice"];
                EndWeightTwo.EditValue = dr["EndWeightTwo"];
                WeightTwoPrice.EditValue = dr["WeightTwoPrice"];                
                BeginWeightTwo.EditValue = dr["BeginWeightTwo"];
                BeginWeightThree.EditValue = dr["BeginWeightThree"];
                EndWeightThree.EditValue = dr["EndWeightThree"];
                WeightThreePrice.EditValue = dr["WeightThreePrice"];
                WeightFourPrice.EditValue = dr["WeightFourPrice"];
                BeginWeightFour.EditValue = dr["BeginWeightFour"];
                EndWeightFour.EditValue = dr["EndWeightFour"];
                Operator.EditValue = dr["Operator"];
                OperationTime.EditValue = dr["OperationTime"];
                CompanyID.EditValue = dr["CompanyID"];        
            }

        }

        //public void GetTravellingTrader()
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
        //            Merchant.Properties.Items.Add(dr[0]);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MsgBox.ShowException(ex);
        //    }
        //}

        //保存
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try{
                if (txtBillNo.Text.Trim() == "")
                {
                    MsgBox.ShowOK("运单号不可以为空！");
                    return;
                }
                if (OfferName.Text.Trim() == "")
                {
                    MsgBox.ShowOK("报价名称不可以为空！");
                    return;
                }

                List<SqlPara> list = new List<SqlPara>();

                list.Add(new SqlPara("weightID", dr == null ? Guid.NewGuid() : dr["weightID"]));
                list.Add(new SqlPara("BillNo", txtBillNo.Text.Trim()));
                list.Add(new SqlPara("OfferName", OfferName.Text.Trim()));
                list.Add(new SqlPara("Merchant", Merchant.Text.Trim()));
                //list.Add(new SqlPara("FreightCharge", FeeAccount.Text.Trim()));
                //list.Add(new SqlPara("FeeAccount", FeeAccount.Text.Trim()));
                list.Add(new SqlPara("FreightCharge", FreightCharge.Text.Trim()));
                list.Add(new SqlPara("ContractBegin", ContractBegin.DateTime));
                list.Add(new SqlPara("ContractEnd", ContractEnd.DateTime));
                list.Add(new SqlPara("Origin", Origin.Text.Trim()));
                list.Add(new SqlPara("Destination", Destination.Text.Trim()));
                list.Add(new SqlPara("SaleProject", SaleProject.Text.Trim()));
                list.Add(new SqlPara("MinimumCharge", MinimumCharge.Text.Trim()));
                list.Add(new SqlPara("Remark", Remark.Text.Trim()));
                list.Add(new SqlPara("BeginWeightOne", BeginWeightOne.Text.Trim()));
                list.Add(new SqlPara("EndWeightOne", EndWeightOne.Text.Trim()));
                list.Add(new SqlPara("WeightOnePrice", WeightOnePrice.Text.Trim()));
                list.Add(new SqlPara("BeginWeightTwo", BeginWeightTwo.Text.Trim()));
                list.Add(new SqlPara("EndWeightTwo", EndWeightTwo.Text.Trim()));
                list.Add(new SqlPara("WeightTwoPrice", WeightTwoPrice.Text.Trim()));
                list.Add(new SqlPara("BeginWeightThree", BeginWeightThree.Text.Trim()));
                list.Add(new SqlPara("EndWeightThree", EndWeightThree.Text.Trim()));
                list.Add(new SqlPara("WeightThreePrice", WeightThreePrice.Text.Trim()));
                list.Add(new SqlPara("BeginWeightFour", BeginWeightFour.Text.Trim()));
                list.Add(new SqlPara("EndWeightFour", EndWeightFour.Text.Trim()));
                list.Add(new SqlPara("WeightFourPrice", WeightFourPrice.Text.Trim()));
                list.Add(new SqlPara("Operator", Operator.Text.Trim()));
                OperationTime.Text = DateTime.Now.ToString(); 
                list.Add(new SqlPara("OperationTime", OperationTime.Text));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BasARSendFeeToWeight", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK("保存成功！");
                    frmBasARSendFeeToWeight frm = (frmBasARSendFeeToWeight)this.Owner;
                    frm.getdate();
                    this.Close();
                    return;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }


        //取消
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
