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
using Newtonsoft.Json;

namespace ZQTMS.UI
{
    public partial class frmChangesApply : BaseForm
    {
        public string crrBillNO = "";
        private DataRow dr;

        public frmChangesApply()
        {
            InitializeComponent();
        }
        private string sBegWeb = "";
        private string sTagetWeb = "";
        private void frmChangesApply_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("放货申请");//xj/2019/5/29
            if (!string.IsNullOrEmpty(crrBillNO.Trim()))
            {
                GetDataByBillNO();
            }
            CommonClass.SetSite(zdhxzd, false);
            textEdit9.EditValue = CommonClass.UserInfo.UserName;
            DataEdit1.DateTime = DateTime.Now;
            //是否控货转放货自动审核
            if (GetCheckIsAutomaticFangHuo())
            {
                this.sb_Automatic.Visible = true;
            }
            else
            {
                this.sb_Automatic.Visible = false;
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
            sBegWeb = ConvertType.ToString(dr["BegWeb"]);
            sTagetWeb = ConvertType.ToString(dr["TargetWeb"]);

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
            if (begWeb.Text != CommonClass.UserInfo.WebName)
            {
                MsgBox.ShowOK("开单网点才能做【控货/放货申请】！");
                return;
            }



            if (dr["BillState"].ToString() == "0")
            {
                MsgBox.ShowOK("此单为新开状态，请直接修改");
                return;
            }   //plh20191207
            string k1 = ConsigneeCompany_K.Text;
            string k2 = ConsigneeName_K.Text;
            string k3 = ConsigneePhone_K.Text;
            string k4 = ConsigneeCellPhone_K.Text;

            List<SqlPara> list2 = new List<SqlPara>();
            list2.Add(new SqlPara("BillNO", crrBillNO));
            SqlParasEntity sps2 = new SqlParasEntity(OperType.Query, "QSP_GET_BILLAPPLY_LastState", list2);
            DataSet ds2 = SqlHelper.GetDataSet(sps2);
            if (ds2 != null && ds2.Tables[0] != null && ds2.Tables[0].Rows.Count > 0)
            {
                MsgBox.ShowOK("改单未执行，不允许申请放货！");
                return;
            }


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
                CommonSyn.ReleaseSyn(crrBillNO, k1, k2, k3, k4);//zaj 放货同步 2018-4-26

                #region hj20190425 转分拨放货申请同步
                try
                {
                    List<SqlPara> listFB = new List<SqlPara>();
                    listFB.Add(new SqlPara("BillNo", crrBillNO));
                    //listFB.Add(new SqlPara("ApplyContent", changes_str));
                    SqlParasEntity speFB = new SqlParasEntity(OperType.Query, "QSP_GET_BillApply_TOFB_2", listFB);
                    DataSet dsFB = SqlHelper.GetDataSet(speFB);
                    if (dsFB != null && dsFB.Tables.Count > 0 && dsFB.Tables[0].Rows.Count > 0)
                    {
                        Dictionary<string, string> dty = new Dictionary<string, string>();
                        dty.Add("BillNO", dsFB.Tables[0].Rows[0]["BillNO"].ToString());
                        dty.Add("ApplyID", dsFB.Tables[0].Rows[0]["ApplyID"].ToString());
                        dty.Add("BillingDate", dsFB.Tables[0].Rows[0]["BillingDate"].ToString());
                        dty.Add("ApplyContent", dsFB.Tables[0].Rows[0]["ApplyContent"].ToString());
                        dty.Add("ApplyDate", dsFB.Tables[0].Rows[0]["ApplyDate"].ToString());
                        dty.Add("BeginSite", dsFB.Tables[0].Rows[0]["BeginSite"].ToString());
                        dty.Add("EndSite", dsFB.Tables[0].Rows[0]["EndSite"].ToString());
                        dty.Add("BillingWeb", dsFB.Tables[0].Rows[0]["BillingWeb"].ToString());
                        dty.Add("ApplyWeb", dsFB.Tables[0].Rows[0]["ApplyWeb"].ToString());
                        dty.Add("ApplyMan", dsFB.Tables[0].Rows[0]["ApplyMan"].ToString());
                        dty.Add("ApplyType", dsFB.Tables[0].Rows[0]["ApplyType"].ToString());
                        dty.Add("LastState", dsFB.Tables[0].Rows[0]["LastState"].ToString());
                        dty.Add("SqlStr", dsFB.Tables[0].Rows[0]["SqlStr"].ToString());
                        //dty.Add("companyid", dsFB.Tables[0].Rows[0]["companyid"].ToString());
                        string json = JsonConvert.SerializeObject(dty);
                        List<SqlPara> listAsy = new List<SqlPara>();
                        listAsy.Add(new SqlPara("FuntionName", "USP_ZQTMS_BILLAPPLYSYN_KT"));
                        listAsy.Add(new SqlPara("FaceUrl", ""));
                        listAsy.Add(new SqlPara("FaceJson", json));
                        listAsy.Add(new SqlPara("BillNos", crrBillNO + ","));
                        listAsy.Add(new SqlPara("Batch", ""));
                        listAsy.Add(new SqlPara("NodeName", "放货申请"));
                        listAsy.Add(new SqlPara("SystemSource", "LMS同星"));
                        listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));
                        SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                        SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);

                    }
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
                #endregion
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
            if (jsfyfkf.Text == "发货方" && CommonClass.UserInfo.WebName != sBegWeb)
            {
                MsgBox.ShowOK("只有开单网点才可以做非提付异动！");
                return;
            }
            if (jsfyfkf.Text == "收货方" && CommonClass.UserInfo.WebName != sTagetWeb)
            {
                MsgBox.ShowOK("只有收货网点才可以做提付异动！");
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
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        //查询是否有权限做：是否控货转放货自动审核
        private bool GetCheckIsAutomaticFangHuo() 
        {
            try
            {
                List<SqlPara> list1 = new List<SqlPara>();
                SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_COMPANY_BY_ID", list1);
                DataSet ds = SqlHelper.GetDataSet(sps1);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["IsAutomaticFangHuo"].ToString() == "1")
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError("是否控货转放货自动审核：" + ex.Message);
            }
            return false;
        }

        //自动执行（审核、审批、执行）流程
        private void sb_Automatic_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.Q_BillNO.Text))
                {
                    return;
                }
                List<SqlPara> list1 = new List<SqlPara>();
                list1.Add(new SqlPara("BillNO", this.Q_BillNO.Text.Trim()));
                list1.Add(new SqlPara("ApplyType", "控货/放货"));
                list1.Add(new SqlPara("type", "自动"));
                SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_BILLAPPLY_HasApplying", list1);
                DataSet ds = SqlHelper.GetDataSet(sps1);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 1)
                    {
                        MsgBox.ShowOK("当前运单存在两条或以上的放货申请记录，不支持自动（审核、审批、执行）操作！");
                        return;
                    }

                    UpdateApplyState("自动", ds.Tables[0].Rows[0]);
                }
                else
                {
                    MsgBox.ShowOK("没有需要审核的运单，请先做担保放货后，再做自动（审核、审批、执行）操作！");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        //执行方法
        private void UpdateApplyState(string Type, DataRow dr)
        {
            try
            {
                if (dr == null || dr["ApplyID"] == null)
                {
                    MsgBox.ShowOK("数据异常！");
                    return;
                }
                string reson = "自动（审核、审批、执行）操作流程";
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ApplyID", dr["ApplyID"]));
                list.Add(new SqlPara("Type", Type));
                list.Add(new SqlPara("Man", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("Reson", reson));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "[QSP_UpdateApplyState_Automatic]", list);
                int result = SqlHelper.ExecteNonQuery(sps);
                if (result > 0)
                {
                    MsgBox.ShowOK("操作成功！");
                    
                    if (Type == "审批")
                    {
                        #region 审批后同步到ZQTMS一条记录,由ZQTMS执行 hj20180504
                        string billNo1 = dr["BillNO"].ToString();
                        CommonSyn.ReviewSyn(billNo1);
                        #endregion
                    }
                    else if (Type == "执行")
                    {
                        #region ZQTMS数据同步 ZAJ 2017-1-18
                        string billNo = dr["BillNO"].ToString();
                        List<SqlPara> listQuery = new List<SqlPara>();
                        listQuery.Add(new SqlPara("BillNo", billNo));
                        SqlParasEntity spsQuery = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BYBILLNO", listQuery);
                        DataSet dsQuery = SqlHelper.GetDataSet(spsQuery);
                        //if (dsQuery == null || dsQuery.Tables[0].Rows.Count == 0 || dsQuery.Tables.Count==0) return;
                        if (dsQuery == null || dsQuery.Tables.Count == 0 || dsQuery.Tables[0].Rows.Count == 0) return;
                        decimal TransferFee = 0;//结算中转费
                        decimal DeliveryFee = 0;//结算送货费
                        decimal Tax_C = 0;//结算税金
                        decimal TerminalOptFee = 0;//结算终端操作费
                        decimal SupportValue_C = 0;//结算保价费
                        decimal StorageFee_C = 0;//结算进仓费
                        decimal NoticeFee_C = 0;//控货费
                        decimal HandleFee_C = 0;//装卸费
                        decimal UpstairFee_C = 0;//上楼费
                        decimal ReceiptFee_C = 0;//回单费

                        CalculateFee(dsQuery, out TransferFee, out DeliveryFee, out Tax_C, out TerminalOptFee, out SupportValue_C, out StorageFee_C, out NoticeFee_C,
                           out HandleFee_C, out UpstairFee_C, out ReceiptFee_C);
                        DataSet dsSend = new DataSet();
                        DataTable dt = new DataTable();
                        DataColumn dc1 = new DataColumn("ApplyID", typeof(string));
                        DataColumn dc2 = new DataColumn("TransferFee", typeof(decimal));
                        DataColumn dc3 = new DataColumn("DeliveryFee", typeof(decimal));
                        DataColumn dc4 = new DataColumn("Tax_C", typeof(decimal));
                        DataColumn dc5 = new DataColumn("TerminalOptFee", typeof(decimal));
                        DataColumn dc6 = new DataColumn("SupportValue_C", typeof(decimal));
                        DataColumn dc7 = new DataColumn("StorageFee_C", typeof(decimal));
                        DataColumn dc8 = new DataColumn("NoticeFee_C", typeof(decimal));
                        DataColumn dc9 = new DataColumn("HandleFee_C", typeof(decimal));
                        DataColumn dc10 = new DataColumn("UpstairFee_C", typeof(decimal));
                        DataColumn dc11 = new DataColumn("ReceiptFee_C", typeof(decimal));
                        DataColumn dc12 = new DataColumn("Type", typeof(string));
                        dt.Columns.Add(dc1);
                        dt.Columns.Add(dc2);
                        dt.Columns.Add(dc3);
                        dt.Columns.Add(dc4);
                        dt.Columns.Add(dc5);
                        dt.Columns.Add(dc6);
                        dt.Columns.Add(dc7);
                        dt.Columns.Add(dc8);
                        dt.Columns.Add(dc9);
                        dt.Columns.Add(dc10);
                        dt.Columns.Add(dc11);
                        dt.Columns.Add(dc12);
                        DataRow dr1 = dt.NewRow();
                        dr1["ApplyID"] = dr["ApplyID"];
                        dr1["TransferFee"] = TransferFee;
                        dr1["DeliveryFee"] = DeliveryFee;
                        dr1["Tax_C"] = Tax_C;
                        dr1["TerminalOptFee"] = TerminalOptFee;
                        dr1["SupportValue_C"] = SupportValue_C;
                        dr1["StorageFee_C"] = StorageFee_C;
                        dr1["NoticeFee_C"] = NoticeFee_C;
                        dr1["HandleFee_C"] = HandleFee_C;
                        dr1["UpstairFee_C"] = UpstairFee_C;
                        dr1["ReceiptFee_C"] = ReceiptFee_C;
                        dr1["Type"] = "改单申请";
                        dt.Rows.Add(dr1);
                        dsSend.Tables.Add(dt);
                        string dsJson = JsonConvert.SerializeObject(dsSend);
                        RequestModel<string> request = new RequestModel<string>();
                        request.Request = dsJson;
                        request.OperType = 0;
                        string json = JsonConvert.SerializeObject(request);
                        //http://192.168.16.112:99//KDLMSService/AllocateToArtery
                        // ResponseModelClone<string> model = HttpHelper.HttpPost(json, "http://lms.dekuncn.com:7016/KDLMSService/AllocateToArtery");
                        ResponseModelClone<string> model = HttpHelper.HttpPost(json, HttpHelper.urlWebApplySyn);
                        // ResponseModelClone<string> model = HttpHelper.HttpPost(json, "http://localhost:42936/KDLMSService/LmsUpdateWayBill");
                        if (model.State != "200")
                        {
                            MsgBox.ShowOK(model.Message);
                        }


                        #endregion
                    }
                    CommonClass.SetOperLog(dr["ApplyID"].ToString(), "", "", CommonClass.UserInfo.UserName, "改单审核", "改单审核" + Type + "操作");
                }
                else
                {
                    MsgBox.ShowOK("操作成失败！");
                }
            }
            catch (Exception ex)
            {
                string errmsg = ex.Message.ToString();
                MsgBox.ShowOK(errmsg.Replace("数据库访问异常：", ""));
            }
        }

        //结算中转费、结算送货费、终端操作费、附加费 的计算
        private void CalculateFee(DataSet ds, out decimal TransferFee, out decimal DeliveryFee, out decimal Tax_C, out decimal TerminalOptFee, out decimal SupportValue_C,
            out decimal StorageFee_C, out decimal NoticeFee_C, out decimal HandleFee_C, out decimal UpstairFee_C, out decimal ReceiptFee_C)
        {
            TransferFee = 0;//结算中转费
            DeliveryFee = 0;//结算送货费
            Tax_C = 0;//结算税金
            TerminalOptFee = 0;//结算终端操作费
            SupportValue_C = 0;//结算保价费
            StorageFee_C = 0;//结算进仓费
            NoticeFee_C = 0;//控货费
            HandleFee_C = 0;//装卸费
            UpstairFee_C = 0;//上楼费
            ReceiptFee_C = 0;//回单费

            string billNo = ds.Tables[0].Rows[0]["BillNo"].ToString();
            string transitMode = "中强专线";
            string receivProvince = ds.Tables[0].Rows[0]["ReceivProvince"].ToString();
            string receivCity = ds.Tables[0].Rows[0]["ReceivCity"].ToString();
            string receivArea = ds.Tables[0].Rows[0]["ReceivArea"].ToString();
            string receivStreet = ds.Tables[0].Rows[0]["ReceivStreet"].ToString();
            string transferSite = ds.Tables[0].Rows[0]["TransferSite"].ToString();
            decimal feeWeight = ConvertType.ToDecimal(ds.Tables[0].Rows[0]["FeeWeight"].ToString());
            decimal feeVolume = ConvertType.ToDecimal(ds.Tables[0].Rows[0]["FeeVolume"].ToString());
            string Package = ds.Tables[0].Rows[0]["Package"].ToString();
            int AlienGoods = Convert.ToInt32(ds.Tables[0].Rows[0]["AlienGoods"].ToString() == "" ? "0" : ds.Tables[0].Rows[0]["AlienGoods"].ToString());
            decimal OperationWeight = ConvertType.ToDecimal(ds.Tables[0].Rows[0]["OperationWeight"].ToString());
            string TransferMode = ds.Tables[0].Rows[0]["TransferMode"].ToString();
            string FeeType = ds.Tables[0].Rows[0]["FeeType"].ToString();
            int IsInvoice = Convert.ToInt32(ds.Tables[0].Rows[0]["IsInvoice"].ToString() == "" ? "0" : ds.Tables[0].Rows[0]["IsInvoice"].ToString());
            int IsSupportValue = Convert.ToInt32(ds.Tables[0].Rows[0]["IsSupportValue"].ToString() == "" ? "0" : ds.Tables[0].Rows[0]["IsSupportValue"].ToString());
            int PreciousGoods = Convert.ToInt32(ds.Tables[0].Rows[0]["PreciousGoods"].ToString() == "" ? "0" : ds.Tables[0].Rows[0]["PreciousGoods"].ToString());
            int IsStorageFee = Convert.ToInt32(ds.Tables[0].Rows[0]["IsStorageFee"].ToString() == "" ? "0" : ds.Tables[0].Rows[0]["IsStorageFee"].ToString());
            int NoticeState = Convert.ToInt32(ds.Tables[0].Rows[0]["NoticeState"].ToString() == "" ? "0" : ds.Tables[0].Rows[0]["NoticeState"].ToString());
            int IsHandleFee = Convert.ToInt32(ds.Tables[0].Rows[0]["IsHandleFee"].ToString() == "" ? "0" : ds.Tables[0].Rows[0]["IsHandleFee"].ToString());
            int IsUpstairFee = Convert.ToInt32(ds.Tables[0].Rows[0]["IsUpstairFee"].ToString() == "" ? "0" : ds.Tables[0].Rows[0]["IsUpstairFee"].ToString());
            int IsReceiptFee = Convert.ToInt32(ds.Tables[0].Rows[0]["IsReceiptFee"].ToString() == "" ? "0" : ds.Tables[0].Rows[0]["IsReceiptFee"].ToString());

            decimal CollectionPay = ConvertType.ToDecimal(ds.Tables[0].Rows[0]["CollectionPay"].ToString());
            decimal Tax = ConvertType.ToDecimal(ds.Tables[0].Rows[0]["Tax"].ToString());

            decimal PaymentAmout = ConvertType.ToDecimal(ds.Tables[0].Rows[0]["PaymentAmout"].ToString());

            #region  计算中转费

            DataRow[] drTransferFee = CommonClass.dsTransferFee.Tables[0].Select("TransferSite='" + transferSite + "' and ToProvince='" + receivProvince + "' and ToCity='" + receivCity + "' and ToArea='" + receivArea + "'");
            if (drTransferFee.Length > 0)
            {
                decimal HeavyPrice = ConvertType.ToDecimal(drTransferFee[0]["HeavyPrice"]);//重货
                decimal LightPrice = ConvertType.ToDecimal(drTransferFee[0]["LightPrice"]);//轻货
                decimal ParcelPriceMin = ConvertType.ToDecimal(drTransferFee[0]["ParcelPriceMin"]);//最低一票
                decimal TransferFeeAll = 0;
                decimal fee = Math.Max(feeWeight * HeavyPrice, feeVolume * LightPrice);
                if (receivProvince != "香港" && receivProvince != "海南省")
                {
                    if (OperationWeight <= 300)
                    {
                        fee = fee * (decimal)1.5;
                    }
                    if (OperationWeight > 3000)
                    {
                        fee = fee * (decimal)0.8;
                    }
                }
                if (Package != "纸箱" && Package != "纤袋" && Package != "膜")
                {
                    TransferFeeAll += fee * (decimal)1.05;
                }
                else
                {
                    TransferFeeAll += fee;
                }
                if (AlienGoods == 1)
                {
                    TransferFeeAll = TransferFeeAll * (decimal)1.5;
                }
                TransferFee = Math.Max(TransferFeeAll, ParcelPriceMin);
                if (receivProvince == "香港")
                {
                    string allFeeType = "";
                    if (feeWeight > feeVolume / (decimal)3.8 * 1000)
                    {
                        allFeeType = "计重";
                    }
                    else
                    {
                        // 总体计方
                        allFeeType = "计方";
                    }
                    if (allFeeType == "计重" && feeWeight < 200)
                    {
                        TransferFee = ParcelPriceMin;
                    }
                    if (allFeeType == "计方" && feeVolume < (decimal)1.2)
                    {
                        TransferFee = ParcelPriceMin;
                    }
                }
            }

            #endregion

            #region 计算送货费
            if (receivProvince == "香港")
            {
                if (TransferMode.Contains("送"))
                {
                    string sql = "Province='" + receivProvince
                                    + "' and City='" + receivCity
                                    + "' and Area='" + receivArea
                                    + "' and Street='" + receivStreet
                                    + "' and " + OperationWeight + ">=w1"
                                    + " and " + OperationWeight + " <w2";
                    DataRow[] drDeliveryFee = CommonClass.dsSendPriceHK.Tables[0].Select(sql);
                    if (drDeliveryFee.Length > 0)
                    {
                        string fmtext = drDeliveryFee[0]["Expression"].ToString();
                        double Additional = ConvertType.ToDouble(drDeliveryFee[0]["Additional"].ToString());
                        fmtext = fmtext.Replace("w", OperationWeight.ToString());
                        DataTable dt = new DataTable();
                        DeliveryFee = Math.Round(decimal.Parse(dt.Compute(fmtext, "").ToString()), 2, MidpointRounding.AwayFromZero);
                        if (FeeType == "计方")
                        {
                            DeliveryFee = DeliveryFee * (decimal)0.6;
                        }
                        DeliveryFee = DeliveryFee + (decimal)Additional;
                        //香港送货费跟ZQTMS比缺少最低一票验证
                    }
                }

            }
            else
            {
                decimal maxFee = 400;
                if (transitMode == "中强快线")
                {
                    maxFee = maxFee * (decimal)1.25;
                }
                if (transitMode == "一票通")
                {
                    maxFee = maxFee * (decimal)1.05;
                }
                if (TransferMode == "送货")
                {
                    string sql = "Province='" + receivProvince
                                    + "' and City='" + receivCity
                                    + "' and Area='" + receivArea
                                    + "' and Street='" + receivStreet
                                    + "' and TransferMode='" + transitMode + "'";
                    DataRow[] drDeliveryFee = CommonClass.dsSendPrice.Tables[0].Select(sql);
                    if (drDeliveryFee == null || drDeliveryFee.Length <= 0)
                    {
                        sql = "Province='全国' and City='全国' and Area='全国' and Street='全国' and TransferMode='" + transitMode + "'";
                        drDeliveryFee = CommonClass.dsSendPrice.Tables[0].Select(sql);
                    }
                    if (drDeliveryFee != null && drDeliveryFee.Length > 0)
                    {
                        DeliveryFee = getDeliveryFee(drDeliveryFee, OperationWeight, feeWeight, feeVolume, Package, FeeType);
                    }
                    if (AlienGoods == 1)
                    {
                        DeliveryFee = DeliveryFee * (decimal)1.5;
                    }
                    // 最低一票30， 最高400封顶
                    if (DeliveryFee < 50)
                    {
                        DeliveryFee = 50;
                    }
                    if (DeliveryFee > maxFee)
                    {
                        DeliveryFee = maxFee;
                    }
                }
                else if (TransferMode == "自提")
                {
                    if (TransferFee <= 0)
                    {
                        string sql = "Province='" + receivProvince
                                             + "' and City='" + receivCity
                                             + "' and Area='" + receivArea
                                             + "' and Street='" + receivStreet
                                             + "' and TransferMode='" + TransferMode + "'";
                        DataRow[] drDeliveryFee = CommonClass.dsSendPrice.Tables[0].Select(sql);
                        if (drDeliveryFee == null || drDeliveryFee.Length <= 0)
                        {
                            sql = "Province='全国' and City='全国' and Area='全国' and Street='全国' and TransferMode='" + TransferMode + "'";
                            drDeliveryFee = CommonClass.dsSendPrice.Tables[0].Select(sql);
                        }
                        if (drDeliveryFee == null || drDeliveryFee.Length > 0)
                        {
                            DeliveryFee = getDeliveryFee(drDeliveryFee, OperationWeight, feeWeight, feeVolume, Package, FeeType);
                        }
                        if (AlienGoods == 1)
                        {
                            DeliveryFee = DeliveryFee * (decimal)1.5;
                        }
                        // 最低一票50， 最高400封顶
                        if (DeliveryFee < 50)
                        {
                            DeliveryFee = 50;
                        }
                        if (DeliveryFee > maxFee)
                        {
                            DeliveryFee = maxFee;
                        }
                        DeliveryFee = DeliveryFee * (decimal)0.5;

                    }
                }


            }


            #endregion

            #region  终端操作费
            DataRow[] drTerminalOptFee = CommonClass.dsTerminalOptFee.Tables[0].Select("TransferSite='" + transferSite + "'");
            if (drTerminalOptFee.Length > 0)
            {
                decimal HeavyPrice = ConvertType.ToDecimal(drTerminalOptFee[0]["HeavyPrice"]);//重货
                decimal LightPrice = ConvertType.ToDecimal(drTerminalOptFee[0]["LightPrice"]);//轻货
                decimal ParcelPriceMin = ConvertType.ToDecimal(drTerminalOptFee[0]["ParcelPriceMin"]);//最低一票
                decimal Weight = OperationWeight;
                decimal acc = Math.Max(Weight * HeavyPrice, feeVolume * LightPrice);
                if (AlienGoods == 1)
                {
                    acc = acc * (decimal)1.5;
                }
                acc = Math.Max(acc, ParcelPriceMin);
                TerminalOptFee = acc;
            }
            #endregion

            #region  附加费
            #region 结算税金
            if (IsInvoice == 1)
            {
                DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='税金' ");
                if (dr.Length > 0)
                {
                    decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);
                    decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]);
                    //税金算法  总运费-代收货款-税金输入 DiscountTransfer
                    decimal Tax1 = PaymentAmout - CollectionPay - Tax;
                    Tax_C = Math.Round(Math.Max(InnerLowest, Tax1 * InnerStandard), 2);
                }
            }
            #endregion
            #region  保价费
            if (IsSupportValue == 1)
            {
                DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='保价费' ");
                if (dr.Length > 0)
                {
                    decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);
                    decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]);
                    decimal SupportValue = ConvertType.ToDecimal(ds.Tables[0].Rows[0]["SupportValue"].ToString());
                    SupportValue_C = Math.Round(Math.Max(InnerLowest, SupportValue * InnerStandard), 2);
                }
            }
            #endregion

            #region 进仓费
            if (IsStorageFee == 1)
            {
                DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='进仓费' ");
                if (dr.Length > 0)
                {
                    decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]); //最低一票金额
                    decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]); //结算标准 
                    decimal StorageFee = ConvertType.ToDecimal(ds.Tables[0].Rows[0]["StorageFee"].ToString());

                    decimal OperationWeight_1 = OperationWeight; //结算重量

                    //结算标准 * 结算重量 >= 最低一票标准
                    StorageFee_C = Math.Round(Math.Max(InnerLowest, OperationWeight_1 * InnerStandard), 2);
                }
            }
            #endregion
            #region 控货费
            if (NoticeState == 1)
            {
                DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='控货费' ");
                if (dr.Length > 0)
                {
                    //控货费费 最低10元一票 
                    decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);
                    NoticeFee_C = InnerLowest;

                }
            }
            #endregion
            #region 装卸费
            if (IsHandleFee == 1)
            {
                DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='装卸费' ");
                if (dr.Length > 0)
                {
                    decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);
                    decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]);
                    //decimal HandleFee = ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "HandleFee"));

                    decimal OperationWeight_1 = OperationWeight; //结算重量             
                    HandleFee_C = Math.Round(Math.Max(InnerLowest, (OperationWeight_1 * InnerStandard) / 1000), 2);
                }
            }
            #endregion

            #region 上楼费
            if (IsUpstairFee == 1)
            {
                DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='上楼费' ");
                if (dr.Length > 0)
                {
                    decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]); //最低一票金额
                    decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]); //结算标准 

                    decimal OperationWeight_1 = OperationWeight; //结算重量

                    //结算标准 * 结算重量 >= 最低一票标准
                    UpstairFee_C = Math.Round(Math.Max(InnerLowest, (OperationWeight_1 * InnerStandard) / 1000), 2);
                }
            }
            #endregion
            #region 回单费
            if (IsReceiptFee == 1)
            {
                DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='回单费' ");
                if (dr.Length > 0)
                {

                    decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);
                    decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]);
                    //if (ReceiptFee > 0)
                    //{
                    //回单费 最低5元一票
                    //decimal ReceiptFee_C = Math.Round(Math.Max(InnerLowest, ReceiptFee * InnerStandard), 2);
                    ReceiptFee_C = InnerLowest;
                }

            }
            #endregion
            #endregion
            //myGridView2.SetRowCellValue(i, "DeliveryFee", DeliveryFee);
            //myGridView2.SetRowCellValue(i, "TransferFee", TransferFee);
            //myGridView2.SetRowCellValue(i, "TerminalOptFee", TerminalOptFee);
            //myGridView2.SetRowCellValue(i, "Tax_C", Tax_C);
            //myGridView2.SetRowCellValue(i, "SupportValue_C", SupportValue_C);
            //myGridView2.SetRowCellValue(i, "StorageFee_C", StorageFee_C);
            //myGridView2.SetRowCellValue(i, "NoticeFee_C", NoticeFee_C);
            //myGridView2.SetRowCellValue(i, "HandleFee_C", HandleFee_C);
            //myGridView2.SetRowCellValue(i, "UpstairFee_C", UpstairFee_C);
            //myGridView2.SetRowCellValue(i, "ReceiptFee_C", ReceiptFee_C);
        }

        /// <summary>
        /// 计算结算送货费
        /// </summary>
        /// <param name="drDeliveryFee"></param>
        private decimal getDeliveryFee(DataRow[] drDeliveryFee, decimal Weight, decimal FeeWeight, decimal FeeVolume, string package, string feeType)
        {
            decimal w0_300 = ConvertType.ToDecimal(drDeliveryFee[0]["w0_300"]);
            decimal w300_500 = ConvertType.ToDecimal(drDeliveryFee[0]["w300_500"]);
            decimal w500_800 = ConvertType.ToDecimal(drDeliveryFee[0]["w500_800"]);
            decimal w800_1000 = ConvertType.ToDecimal(drDeliveryFee[0]["w800_1000"]);
            decimal w1000_2000 = ConvertType.ToDecimal(drDeliveryFee[0]["w1000_2000"]);
            decimal w2000_3000 = ConvertType.ToDecimal(drDeliveryFee[0]["w2000_3000"]);
            decimal w3000_100000 = ConvertType.ToDecimal(drDeliveryFee[0]["w3000_100000"]);

            decimal v0_300 = ConvertType.ToDecimal(drDeliveryFee[0]["v0_300"]);
            decimal v300_500 = ConvertType.ToDecimal(drDeliveryFee[0]["v300_500"]);
            decimal v500_800 = ConvertType.ToDecimal(drDeliveryFee[0]["v500_800"]);
            decimal v800_1000 = ConvertType.ToDecimal(drDeliveryFee[0]["v800_1000"]);
            decimal v1000_2000 = ConvertType.ToDecimal(drDeliveryFee[0]["v1000_2000"]);
            decimal v2000_3000 = ConvertType.ToDecimal(drDeliveryFee[0]["v2000_3000"]);
            decimal v3000_100000 = ConvertType.ToDecimal(drDeliveryFee[0]["v3000_100000"]);

            //decimal DeliveryFee = ConvertType.ToDecimal(drDeliveryFee[0]["DeliveryFee"]);
            decimal DeliveryFee = 0;
            decimal wDeliveryFee = 0;
            decimal vDeliveryFee = 0;

            // for (int i = 0; i < RowCount; i++)
            // {
            decimal w = FeeWeight;
            decimal v = FeeVolume;
            string Package = package;

            string FeeType = feeType;

            if (Weight >= 0 && Weight <= 300)
            {
                wDeliveryFee = w0_300 * w;
                vDeliveryFee = v0_300 * v;
            }
            else if (Weight >= 300 && Weight <= 500)
            {
                wDeliveryFee = w300_500 * w;
                vDeliveryFee = v300_500 * v;
            }
            else if (Weight >= 500 && Weight <= 800)
            {
                wDeliveryFee = w500_800 * w;
                vDeliveryFee = v500_800 * v;
            }
            else if (Weight >= 800 && Weight <= 1000)
            {
                wDeliveryFee = w800_1000 * w;
                vDeliveryFee = v800_1000 * v;
            }
            else if (Weight >= 1000 && Weight <= 2000)
            {
                wDeliveryFee = w1000_2000 * w;
                vDeliveryFee = v1000_2000 * v;
            }
            else if (Weight >= 2000 && Weight <= 3000)
            {
                wDeliveryFee = w2000_3000 * w;
                vDeliveryFee = v2000_3000 * v;
            }
            else if (Weight > 3000)
            {
                wDeliveryFee = w3000_100000 * w;
                vDeliveryFee = v3000_100000 * v;
            }

            if (Package != "纸箱" && Package != "纤袋" && Package != "膜")
            {

                wDeliveryFee = wDeliveryFee * Convert.ToDecimal(1.05);
                vDeliveryFee = vDeliveryFee * Convert.ToDecimal(1.05);

            }
            if (FeeVolume / (decimal)(3.8) * 1000 < FeeWeight)
            {
                DeliveryFee += wDeliveryFee;
            }
            else
            {
                DeliveryFee += vDeliveryFee;
            }

            //if (FeeType == "计重")
            //{
            //    DeliveryFee += wDeliveryFee;
            //}
            //else
            //{
            //    DeliveryFee += vDeliveryFee;
            //}
            // }
            return DeliveryFee;
        }
    }
}