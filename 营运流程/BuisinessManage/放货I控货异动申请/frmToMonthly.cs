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
    public partial class frmToMonthly : BaseForm
    {
        public string crrBillNO = "";
        private DataRow dr;
        private DataTable dt_;

        public frmToMonthly()
        {
            InitializeComponent();
        }
        
        private void frmToMonthly_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("提付转月结");//xj/2019/5/29
            if (!string.IsNullOrEmpty(crrBillNO.Trim()))
            {
                GetDataByBillNO();
            }
            textEdit9.EditValue = CommonClass.UserInfo.UserName;
            DataEdit1.DateTime = DateTime.Now;
            BindCust();
            CommonClass.SetUser(ToMonthPayMan,"全部",false);
            ToMonthPayMan.EditValue = CommonClass.UserInfo.UserName;
            ToMonthPayDate.DateTime = CommonClass.gbdate;
        }

        private void jjfs_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            { 
                string comp = jjfs.Text;
                DataRow[] drs_ = dt_.Select(string.Format("FullName='{0}' ", comp));
                if (drs_.Length > 0)
                {
                    gsmc.Text = drs_[0]["FullName"].ToString();
                    khbh.Text = drs_[0]["basCustContractID"].ToString();
                    lxr.Text = drs_[0]["CustLinkName"].ToString();
                    lxsj.Text = drs_[0]["SendCustMobile"].ToString();
                }
            }
            catch
            {
            }
        }

        public void BindCust() 
        {
            jjfs.Properties.Items.Clear();
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASCUSTCONTRACT", list);
                DataSet ds = SqlHelper.GetDataSet(sps); 

                if (ds == null || ds.Tables.Count == 0) return;
                dt_ = ds.Tables[0];

                foreach (DataRow drs in ds.Tables[0].Rows)
                {
                    jjfs.Properties.Items.Add(drs["FullName"].ToString());
                    //if (drs["FullName"].ToString() == "深圳市博通云创供应链管理有限公")
                    //    MsgBox.ShowError("已经找到" + drs["FullName"].ToString());
                }
                jjfs.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            } 
        }

        private void textEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string billno = Q_BillNO.Text.Trim();
                if (crrBillNO != billno && !string.IsNullOrEmpty(billno))
                {
                    crrBillNO = billno;
                    GetDataByBillNO();
                }
            }
        }
 
        /// <summary>
        /// 根据运单编号获取运单
        /// </summary>
        private bool GetDataByBillNO()
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillNO", crrBillNO));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_ByID", list);
            DataSet ds = SqlHelper.GetDataSet(sps);

            if (ds == null || ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0)
            {
                dr = null;
                MsgBox.ShowOK("运单号不存在！");
                return false;
            }

            dr = ds.Tables[0].Rows[0];

            BillNO.EditValue = dr["BillNO"];
            VehicleNo.EditValue = dr["VehicleNo"];
            BillState.EditValue = dr["BillState"];
            TransferMode.EditValue = dr["TransferMode"];
            TransitMode.EditValue = dr["TransitMode"];
            CusOderNo.EditValue = dr["CusOderNo"];
            ConsigneeCellPhone.EditValue = dr["ConsigneeCellPhone"];
            ConsigneeName.EditValue = dr["ConsigneeName"];
            ConsigneeCompany.EditValue = dr["ConsigneeCompany"];
            PickGoodsSite.EditValue = dr["PickGoodsSite"];
            ReceivAddress.EditValue = dr["ReceivAddress"];
            ConsignorCellPhone.EditValue = dr["ConsignorCellPhone"];
            ConsignorName.EditValue = dr["ConsignorName"];
            ConsignorCompany.EditValue = dr["ConsignorCompany"];
            ReceivMode.EditValue = dr["ReceivMode"];
            CusNo.EditValue = dr["CusNo"];
            ReceivOrderNo.EditValue = dr["ReceivOrderNo"];
            Salesman.EditValue = dr["Salesman"];
            // NoticeState.EditValue = dr["NoticeState"];
            GoodsType.EditValue = dr["GoodsType"];
            Varieties.EditValue = dr["Varieties"];
            Num.EditValue = dr["Num"];
            LeftNum.EditValue = dr["LeftNum"];
            FeeWeight.EditValue = dr["FeeWeight"];
            FeeVolume.EditValue = dr["FeeVolume"];
            Weight.EditValue = dr["Weight"];
            Volume.EditValue = dr["Volume"];
            FeeType.EditValue = dr["FeeType"];
            Freight.EditValue = dr["Freight"];
            DeliFee.EditValue = dr["DeliFee"];
            ReceivFee.EditValue = dr["ReceivFee"];
            DeclareValue.EditValue = dr["DeclareValue"];
            SupportValue.EditValue = dr["SupportValue"];
            Tax.EditValue = dr["Tax"];
            InformationFee.EditValue = dr["InformationFee"];
            Expense.EditValue = dr["Expense"];
            NoticeFee.EditValue = dr["NoticeFee"];
            DiscountTransfer.EditValue = dr["DiscountTransfer"];
            CollectionPay.EditValue = dr["CollectionPay"];
            AgentFee.EditValue = dr["AgentFee"];
            FuelFee.EditValue = dr["FuelFee"];
            UpstairFee.EditValue = dr["UpstairFee"];
            HandleFee.EditValue = dr["HandleFee"];
            ForkliftFee.EditValue = dr["ForkliftFee"];
            StorageFee.EditValue = dr["StorageFee"];
            CustomsFee.EditValue = dr["CustomsFee"];
            packagFee.EditValue = dr["packagFee"];
            FrameFee.EditValue = dr["FrameFee"];
            //ChangeFee.EditValue = dr["ChangeFee"];
            OtherFee.EditValue = dr["OtherFee"];
            ReceiptFee.EditValue = dr["ReceiptFee"];
            ReceiptCondition.EditValue = dr["ReceiptCondition"];
            FreightAmount.EditValue = dr["FreightAmount"];
            //CouponsNo.EditValue = dr["CouponsNo"];
            CouponsAmount.EditValue = dr["CouponsAmount"];
            PaymentMode.EditValue = dr["PaymentMode"];
            PayMode.EditValue = dr["PayMode"];
            NowPay.EditValue = dr["NowPay"];
            FetchPay.EditValue = dr["FetchPay"];
            MonthPay.EditValue = dr["MonthPay"];
            ShortOwePay.EditValue = dr["ShortOwePay"];
            BefArrivalPay.EditValue = dr["BefArrivalPay"];
            BillRemark.EditValue = dr["BillRemark"];
            ModifyRemark.EditValue = dr["SignRemark"];
            WebDate.EditValue = dr["WebDate"];
            OtherTotalFee.EditValue = dr["OtherTotalFee"];
            BillMan.EditValue = dr["BillMan"];
            begWeb.EditValue = dr["begWeb"];
            Package.EditValue = dr["Package"];
            //MiddleDate.EditValue = dr["MiddleDate"];
            ModifyRemark.EditValue = dr["ModifyRemark"];
 

            if (dr["IsInvoice"] != null && dr["IsInvoice"].ToString() == "1")
                IsInvoice.Checked = true;
            else
                IsInvoice.Checked = false;


            if (dr["NoticeState"].ToString().ToString() == "1")
            {  
                NoticeState.Checked = true; 
            }
            else
            { 
                NoticeState.Checked = false;
            }

            return true;
        }
 

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (dr == null)
            { 
                MsgBox.ShowOK("请选择一个运单放货！");
                return;
            }  

            if (!HasApply())
            {
                MsgBox.ShowOK("已存在【转月结】申请，不能重新申请！");
                return;
            }

            string gsmc_ = gsmc.Text.Trim();
            string fkfs_ = fkfs.Text.Trim();
            string lxr_ = lxr.Text.Trim();
            string lxsj_ = lxsj.Text.Trim();
            string khbh_ = khbh.Text.Trim();
            string fkfs1 = PaymentMode.Text.Trim();

            if (string.IsNullOrEmpty(gsmc_))
            {
                MsgBox.ShowOK("月结客户名称能为空！");
                gsmc.Focus();
                return;
            }

            if (string.IsNullOrEmpty(fkfs_))
            {
                MsgBox.ShowOK("付款方式不能为空！");
                fkfs.Focus();
                return;
            }

            if (fkfs1 != "提付")      //whf20190718
            {
                MsgBox.ShowOK("付款方式为“提付”才能做【提付转月结】申请！");
                return;
            }

            if (string.IsNullOrEmpty(khbh_))
            {
                MsgBox.ShowOK("月结客户编号不能为空！");
                return;
            }

            if (string.IsNullOrEmpty(lxr_))
            {
                MsgBox.ShowOK("月结客户联系人不能为空！");
                return;
            }


            if (string.IsNullOrEmpty(lxsj_))
            {
                MsgBox.ShowOK("月结客户联系方式不能为空！");
                return;
            }

            //zb20190716 lms4127 判断是否核销
            List<SqlPara> list1 = new List<SqlPara>();
            list1.Add(new SqlPara("BillNO", crrBillNO));
            SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_VerifState", list1);
            DataSet dset = SqlHelper.GetDataSet(spe);
            if (dset != null || dset.Tables[1].Rows.Count > 0)
            {
                if (dset.Tables[1].Rows[0]["FetchPayState"].ToString() != "")
                {
                    if (Convert.ToInt32(dset.Tables[1].Rows[0]["FetchPayState"]) == 1)
                    {
                        MsgBox.ShowOK("已核销提付款不允许申请提付改欠！");
                        return;
                    }
                }
            }

  
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillNO", dr["BillNO"]));
            list.Add(new SqlPara("PayMode", fkfs_));
            list.Add(new SqlPara("ConsignorCompany", gsmc_));
            list.Add(new SqlPara("CusNo", khbh_));
            list.Add(new SqlPara("ConsignorCellPhone", lxsj_));
            list.Add(new SqlPara("ConsignorName", lxr_));
   
            list.Add(new SqlPara("ApplyWeb", CommonClass.UserInfo.WebName));
            list.Add(new SqlPara("ApplyMan", CommonClass.UserInfo.UserName)); 
            list.Add(new SqlPara("BillingWeb", dr["BegWeb"]));
            list.Add(new SqlPara("BillingDate", dr["BillDate"]));
            list.Add(new SqlPara("BeginSite", dr["StartSite"]));
            list.Add(new SqlPara("EndSite", dr["DestinationSite"]));
            list.Add(new SqlPara("ToMonthPayMan", ToMonthPayMan.EditValue));
            list.Add(new SqlPara("ToMonthPayDate", ToMonthPayDate.DateTime));
            SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_ToMonthlyApply", list);
            int result = SqlHelper.ExecteNonQuery(sps);
            if (result > 0)
            {
                MsgBox.ShowOK("操作成功！");
                return;
            }
            else
            {
                MsgBox.ShowOK("操作失败！");
                return;
            } 
        }
 
 
        private bool HasApply()
        {
            bool bl = false;
            try
            {
                List<SqlPara> list1 = new List<SqlPara>();
                list1.Add(new SqlPara("BillNO", dr["BillNO"]));
                list1.Add(new SqlPara("ApplyType", "转月结"));
                SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_BILLAPPLY_HasApplying", list1);
                DataSet ds = SqlHelper.GetDataSet(sps1);
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    bl = false;
                else
                    bl = true;
            }
            catch 
            {
                bl = false;

            }
            return bl;
        }

 

    }
}
