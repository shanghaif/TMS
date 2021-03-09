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
    public partial class JMfrmChangesApply : BaseForm
    {
        public string crrBillNO = "";
        private DataRow dr;

        public JMfrmChangesApply()
        {
            InitializeComponent();
        }

        private void frmChangesApply_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(crrBillNO.Trim()))
            {
                GetDataByBillNO();
            }
            CommonClass.SetSite(zdhxzd, false);
            textEdit9.EditValue = CommonClass.UserInfo.UserName;
            DataEdit1.DateTime = DateTime.Now;
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
                ConsigneeCompany_K.EditValue = "88888888";
                ConsigneeName_K.EditValue = "88888888";
                ConsigneePhone_K.EditValue = "88888888";
                ConsigneeCellPhone_K.EditValue = "88888888";
                ConsigneeCompany_K.Enabled = false;
                ConsigneeName_K.Enabled = false;
                ConsigneePhone_K.Enabled = false;
                ConsigneeCellPhone_K.Enabled = false;

                NoticeState.Checked = true;
                IsReleaseGoods.Checked = false;
            }
            else
            {
                ConsigneeCompany_K.EditValue = dr["ConsigneeCompany_K"];
                ConsigneeName_K.EditValue = dr["ConsigneeName_K"];
                ConsigneePhone_K.EditValue = dr["ConsigneePhone_K"];
                ConsigneeCellPhone_K.EditValue = dr["ConsigneeCellPhone_K"];
                ConsigneeCompany_K.Enabled = true;
                ConsigneeName_K.Enabled = true;
                ConsigneePhone_K.Enabled = true;
                ConsigneeCellPhone_K.Enabled = true;

                NoticeState.Checked = true;
                IsReleaseGoods.Checked = true;
                NoticeState.Checked = false;
            }

            return true;
        }

        private void checkEdit3_CheckedChanged(object sender, EventArgs e)
        {
            if (IsReleaseGoods.Checked && dr != null)
            {
                ConsigneeCompany_K.EditValue = dr["ConsigneeCompany_K"];
                ConsigneeName_K.EditValue = dr["ConsigneeName_K"];
                ConsigneePhone_K.EditValue = dr["ConsigneePhone_K"];
                ConsigneeCellPhone_K.EditValue = dr["ConsigneeCellPhone_K"];
                ConsigneeCompany_K.Enabled = true;
                ConsigneeName_K.Enabled = true;
                ConsigneePhone_K.Enabled = true;
                ConsigneeCellPhone_K.Enabled = true;

            }
            else
            {
                if (!IsReleaseGoods.Checked)
                {
                    ConsigneeCompany_K.EditValue = dr["ConsigneeCompany"];
                    ConsigneeName_K.EditValue = dr["ConsigneeName"];
                    ConsigneePhone_K.EditValue = dr["ConsigneePhone"];
                    ConsigneeCellPhone_K.EditValue = dr["ConsigneeCellPhone"];
                }
                ConsigneeCompany_K.Enabled = false;
                ConsigneeName_K.Enabled = false;
                ConsigneePhone_K.Enabled = false;
                ConsigneeCellPhone_K.Enabled = false;

            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (dr == null)
            {
                MsgBox.ShowOK("请选择一个运单放货！");
                return;
            }
            if (!IsReleaseGoods.Checked)
            {
                MsgBox.ShowOK("请先勾选放货!");
                return;
            }
            if (dr["NoticeState"].ToString() == "0")
            {
                MsgBox.ShowOK("该单已经是放货状态，不需要放货!");
                return;
            }

            if (!HasApply())
            {
                MsgBox.ShowOK("已存在【控货/放货申请】，不能重新申请！");
                return;
            }

            string k1 = ConsigneeCompany_K.Text;
            string k2 = ConsigneeName_K.Text;
            string k3 = ConsigneePhone_K.Text;
            string k4 = ConsigneeCellPhone_K.Text;

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillNO", crrBillNO));
            list.Add(new SqlPara("ConsigneeCompany_K", k1));
            list.Add(new SqlPara("ConsigneeName_K", k2));
            list.Add(new SqlPara("ConsigneePhone_K", k3));
            list.Add(new SqlPara("ConsigneeCellPhone_K", k4));
            list.Add(new SqlPara("ApplyWeb", CommonClass.UserInfo.WebName));
            list.Add(new SqlPara("ApplyMan", CommonClass.UserInfo.UserName));
            list.Add(new SqlPara("BillingWeb", dr["BegWeb"]));
            list.Add(new SqlPara("BillingDate", dr["BillDate"]));
            list.Add(new SqlPara("BeginSite", dr["StartSite"]));
            list.Add(new SqlPara("EndSite", dr["DestinationSite"]));
            SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_WayBill_ChagesApply_ReleaseGoods", list);
            int result = SqlHelper.ExecteNonQuery(sps);
            if (result > 0)
            {
                MsgBox.ShowOK("操作成功！");
            }
            else
            {
                MsgBox.ShowOK("操作失败！");
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (dr == null)
            {
                MsgBox.ShowOK("请选择一个运单控货！");
                return;
            }
            if (dr["NoticeState"].ToString() == "1")
            {
                MsgBox.ShowOK("该单已经是控货状态！");
                return;
            }

            if (!HasApply())
            {
                MsgBox.ShowOK("已存在【控货/放货申请】，不能重新申请！");
                return;
            }

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillNO", crrBillNO));
            list.Add(new SqlPara("ApplyWeb", CommonClass.UserInfo.WebName));
            list.Add(new SqlPara("ApplyMan", CommonClass.UserInfo.UserName));

            list.Add(new SqlPara("BillingWeb", dr["BegWeb"]));
            list.Add(new SqlPara("BillingDate", dr["BillDate"]));
            list.Add(new SqlPara("BeginSite", dr["StartSite"]));
            list.Add(new SqlPara("EndSite", dr["DestinationSite"]));


            SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_WayBill_ChagesApply_ControlGoods", list);
            int result = SqlHelper.ExecteNonQuery(sps);
            if (result > 0)
            {
                MsgBox.ShowOK("操作成功！");
            }
            else
            {
                MsgBox.ShowOK("操作失败！");
            }
        }

        private bool HasApply()
        {
            bool bl = false;
            try
            {
                List<SqlPara> list1 = new List<SqlPara>();
                list1.Add(new SqlPara("BillNO", dr["BillNO"]));
                list1.Add(new SqlPara("ApplyType", "控货/放货"));
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

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (dr == null)
            {
                MsgBox.ShowOK("请选择一个运单控货！");
                return;
            }
            string _jjfs = jjfs.Text;
            string _zjkm = zjkm.Text;
            decimal _ydzj = 0;
            try
            {
                _ydzj = decimal.Parse(ydzj.Text.Trim());
            }
            catch
            {
                MsgBox.ShowOK("异动增加金额不合法，请重新输入！");
                ydzj.Text = "";
                ydzj.Focus();
                return;
            }
            string _jsfyfkf = jsfyfkf.Text;
            string _zdhxd = zdhxzd.Text;
            string _ydyy = ydyy.Text;
            DateTime date = DataEdit1.DateTime;
            string _ydry = textEdit9.Text;

            try
            {
                List<SqlPara> list1 = new List<SqlPara>();
                list1.Add(new SqlPara("BillNO", dr["BillNO"]));
                list1.Add(new SqlPara("ApplyType", "运费异动"));
                SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_BILLAPPLY_HasApplying", list1);
                DataSet ds = SqlHelper.GetDataSet(sps1);
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    MsgBox.ShowOK("已存在【运费异动申请】，不能重新申请！");
                    return;
                }

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", Guid.NewGuid()));
                list.Add(new SqlPara("BillNO", crrBillNO));
                list.Add(new SqlPara("ChangeTransfType", _jjfs));//交接方式调整
                list.Add(new SqlPara("ChangePlusFee", _ydzj));//异动增加费用
                list.Add(new SqlPara("ChangePlusObj", _zjkm));//异动增加科目
                list.Add(new SqlPara("PlusFeePayer", _jsfyfkf));//加收费用付款方
                list.Add(new SqlPara("CancelSite", _zdhxd));//指定核销站点
                list.Add(new SqlPara("ChangeBecuase", _ydyy));//异动原因
                list.Add(new SqlPara("ChangeDate", date));//异动日期
                list.Add(new SqlPara("ChangeMan", _ydry));//异动人员 
                list.Add(new SqlPara("ChangeArea", CommonClass.UserInfo.AreaName));//异动大区
                list.Add(new SqlPara("ChangeCause", CommonClass.UserInfo.CauseName));//异动事业部

                list.Add(new SqlPara("ApplyWeb", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("ApplyMan", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("BillingWeb", dr["BegWeb"]));
                list.Add(new SqlPara("BillingDate", dr["BillDate"]));
                list.Add(new SqlPara("BeginSite", dr["StartSite"]));
                list.Add(new SqlPara("EndSite", dr["DestinationSite"]));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLFREIGHTCHANGES", list);
                int result = SqlHelper.ExecteNonQuery(sps);
                if (result > 0)
                {
                    MsgBox.ShowOK("操作成功！");
                }
                else
                {
                    MsgBox.ShowOK("操作失败！");
                }
            }
            catch
            {
                MsgBox.ShowOK("操作失败！");
            }
        }
    }
}