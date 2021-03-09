using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class frmAppointmentSend : BaseForm
    {
        public string crrBillNO = "";
        private DataRow dr; //运单

        public frmAppointmentSend()
        {
            InitializeComponent();
        }

        public frmAppointmentSend(string BillNo)
        {
            InitializeComponent();
            this.crrBillNO = BillNo;
        }

        private void frmAppointmentSend_Load(object sender, EventArgs e)
        {
            BespeakTime.DateTime = CommonClass.gcdate;
            customStartDate.DateTime = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            customEndDate.DateTime = Convert.ToDateTime(DateTime.Now.ToShortDateString());

            if (!getWayBillByBillNO())
            {
                MsgBox.ShowOK("没有找到运单，可能已被删除！");
                return;
            }
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            if (crrBillNO == "")
            {
                MsgBox.ShowOK("请选择一个运单！");
                return;
            }
            if (CarType.Text.Trim() == "")
            {
                MsgBox.ShowOK("车型要求不能为空！");
                return;
            }
            DateTime customStarttime = customStartDate.DateTime;//zxw 客户收货时间从
            DateTime customEndtime = customEndDate.DateTime;//zxw 客户收货时间至
            if (customEndtime < customStarttime)
            {
                MsgBox.ShowOK("客户收货截止时间不能小于起始时间！");
                return;
            }

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BespeakID", Guid.NewGuid()));
                list.Add(new SqlPara("BespeakDate", BespeakTime.DateTime));
                list.Add(new SqlPara("Operator", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("BespeakDept", CommonClass.UserInfo.DepartName));
                list.Add(new SqlPara("customStarttime", customStarttime));
                list.Add(new SqlPara("customEndtime", customEndtime));
                list.Add(new SqlPara("CarTypeRequir", CarType.Text.Trim()));
                list.Add(new SqlPara("BringManRequir", BringMan.Text.Trim()));
                list.Add(new SqlPara("OperRequir", OperRequire.Text.Trim()));

                string str = "";
                str += CarType.Text.Trim() + "|" + BringMan.Text.Trim() + "|" + OperRequire.Text.Trim();


                list.Add(new SqlPara("BespeakRequir", str));
                list.Add(new SqlPara("BillNO", crrBillNO));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BLLBESPEAKSENDGOODS", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.ToString());
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 根据运单编号获取运单
        /// </summary>
        private bool getWayBillByBillNO()
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillNO", crrBillNO));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_ByID", list);
            DataSet ds = SqlHelper.GetDataSet(sps);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return false;
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
            NoticeState.EditValue = dr["NoticeState"];
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
            ChangeFee.EditValue = dr["ChangeFee"];
            OtherFee.EditValue = dr["OtherFee"];
            IsInvoice.EditValue = dr["IsInvoice"];
            ReceiptFee.EditValue = dr["ReceiptFee"];
            ReceiptCondition.EditValue = dr["ReceiptCondition"];
            FreightAmount.EditValue = dr["FreightAmount"];
            CouponsNo.EditValue = dr["CouponsNo"];
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
            ModifyRemark.EditValue = dr["ModifyRemark"];
            Package.EditValue = dr["Package"];


            //MiddleRemark.EditValue = dr["MiddleRemark"];
            //AccMiddlePay.EditValue = dr["AccMiddlePay"];
            //MiddleOperator.EditValue = dr["MiddleOperator"];
            //MiddleBillNo.EditValue = dr["MiddleBillNo"];
            //MiddleEndSitePhone.EditValue = dr["MiddleEndSitePhone"];
            //MiddleStartSitePhone.EditValue = dr["MiddleStartSitePhone"];
            //MiddleCarrier.EditValue = dr["MiddleCarrier"];
            //MiddleDate.EditValue = dr["MiddleDate"];
            //MiddleBatch.EditValue = dr["MiddleBatch"];


            List<SqlPara> list1 = new List<SqlPara>();
            list1.Add(new SqlPara("BillNO", crrBillNO));
            SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_BLLBESPEAKSENDGOODS_ByBillNO", list1);
            DataSet ds1 = SqlHelper.GetDataSet(sps1);

            if (ds1.Tables[0].Rows.Count > 0)
            {
                DataRow dr1 = ds1.Tables[0].Rows[0];
                BespeakTime.EditValue = dr1["BespeakDate"];

            }
            return true;
        }

    }
}