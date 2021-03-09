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
    public partial class JMfrmAppointmentSend : BaseForm
    {
        public string crrBillNO = "";
        private DataRow dr; //运单

        public JMfrmAppointmentSend()
        {
            InitializeComponent();
        }

        public JMfrmAppointmentSend(string BillNo)
        {
            InitializeComponent();
            this.crrBillNO = BillNo;
        }

        private void frmAppointmentSend_Load(object sender, EventArgs e)
        {
            BespeakTime.DateTime = CommonClass.gcdate;

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

            if (BespeakContent.Text.Trim() == "")
            {
                MsgBox.ShowOK("预约送货内容不能为空！");
                return;
            }

            try
            { 
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BespeakID", Guid.NewGuid()));
                list.Add(new SqlPara("BespeakDate", BespeakTime.DateTime));
                list.Add(new SqlPara("BespeakRemark", BespeakContent.Text.Trim()));
                list.Add(new SqlPara("Operator", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("BespeakDept", CommonClass.UserInfo.DepartName));

                string str = "";
                if (checkBox1.Checked)
                    str += (str == "" ? "" : ",") + checkBox1.Text;
                if (checkBox2.Checked)
                    str += (str == "" ? "" : ",") + checkBox2.Text;
                if (checkBox3.Checked)
                    str += (str == "" ? "" : ",") + checkBox3.Text;
                if (checkBox4.Checked)
                    str += (str == "" ? "" : ",") + checkBox4.Text;
                if (checkBox5.Checked)
                    str += (str == "" ? "" : ",") + checkBox5.Text;
                if (checkBox6.Checked)
                    str += (str == "" ? "" : ",") + checkBox6.Text;
                if (checkBox7.Checked)
                    str += (str == "" ? "" : ",") + checkBox7.Text;
                if (checkBox8.Checked)
                    str += (str == "" ? "" : ",") + checkBox8.Text;
                if (checkBox9.Checked)
                    str += (str == "" ? "" : ",") + checkBox9.Text;
                if (checkBox10.Checked)
                    str += (str == "" ? "" : ",") + checkBox10.Text; 

                list.Add(new SqlPara("BespeakRequir", str));
                list.Add(new SqlPara("BillNO", crrBillNO));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BLLBESPEAKSENDGOODS", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
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
 
            if (ds.Tables[0].Rows.Count == 0) return false;
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
                BespeakContent.EditValue = dr1["BespeakRemark"];

                string str = dr1["BespeakRequir"] + "";
                string[] arr = str.Split(',');

                foreach (string s in arr)
                {
                    if (s.Trim() == checkBox1.Text.Trim())
                        checkBox1.Checked = true;
                    if (s.Trim() == checkBox2.Text.Trim())
                        checkBox2.Checked = true;
                    if (s.Trim() == checkBox3.Text.Trim())
                        checkBox3.Checked = true;
                    if (s.Trim() == checkBox4.Text.Trim())
                        checkBox4.Checked = true;
                    if (s.Trim() == checkBox5.Text.Trim())
                        checkBox5.Checked = true;
                    if (s.Trim() == checkBox6.Text.Trim())
                        checkBox6.Checked = true;
                    if (s.Trim() == checkBox7.Text.Trim())
                        checkBox7.Checked = true;
                    if (s.Trim() == checkBox8.Text.Trim())
                        checkBox8.Checked = true;
                    if (s.Trim() == checkBox9.Text.Trim())
                        checkBox9.Checked = true;
                    if (s.Trim() == checkBox10.Text.Trim())
                        checkBox10.Checked = true;
                }
            }
            return true;
        }
    }
}