using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.Common;
using System.IO;
using System.Net;
using ZQTMS.SqlDAL;


namespace ZQTMS.UI
{
    public partial class frmClaimDetail : BaseForm
    {
        public string billNoStr = "";
        public Boolean isClose = false;
        private DataRow dr = null;

        public frmClaimDetail()
        {
            InitializeComponent();
        }

        private void frmClaimDetail_Load(object sender, EventArgs e)
        {
            getData();
            getApplyData();
            printBtn.Enabled = UserRight.GetRight("212181");
            CommonClass.SetWeb(DutyDepart, false);
        }

        private void getApplyData()
        {
            try
            {
                //List<SqlPara> list = new List<SqlPara>();
                //list.Add(new SqlPara("BillNo", billNoStr));
                //SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_Claim_Apply_byBillNo", list);
                //DataSet ds = SqlHelper.GetDataSet(sps);

                //调用ZQTMS理赔审批信息
                DataSet ds = CommonSyn.GetZQTMSClaimMessage(billNoStr, "", "", "", "", "", "QSP_GET_Claim_Apply_byBillNo");//QSP_GET_Claim_Apply_byBillNo

                if (ds == null || ds.Tables.Count == 0) return;
                this.listView1.BeginUpdate();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = (ds.Tables[0].Rows.Count - i) + "";
                    lvi.SubItems.Add(dr["LinkName"].ToString());
                    lvi.SubItems.Add(dr["ChangeDate"].ToString());
                    lvi.SubItems.Add(dr["DealStatus"].ToString());
                    lvi.SubItems.Add(dr["DealOpinion"].ToString());
                    lvi.SubItems.Add(dr["InAffirmMoney"].ToString());
                    lvi.SubItems.Add(dr["ApplyMan"].ToString());
                    lvi.SubItems.Add(dr["ApplyDate"].ToString());
                    lvi.SubItems.Add(dr["ApplyResult"].ToString());
                    this.listView1.Items.Add(lvi);
                }

                this.listView1.EndUpdate();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void getData()
        {
            try
            {
                //List<SqlPara> list = new List<SqlPara>();
                //list.Add(new SqlPara("BillNo", billNoStr));
                //SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_Claim_Detail_byBillNo", list);
                //DataSet ds = SqlHelper.GetDataSet(sps);

                //调用ZQTMS理赔审批信息
                DataSet ds = CommonSyn.GetZQTMSClaimMessage(billNoStr, "", "", "", "", "", "QSP_GET_Claim_Detail_byBillNo");//QSP_GET_Claim_Detail_byBillNo
                if (ds == null || ds.Tables.Count == 0) return;

                dr = ds.Tables[0].Rows[0];
                ClaimMan.Text = dr["ClaimMan"].ToString();
                ClaimWeb.Text = dr["ClaimWeb"].ToString();
                ClaimDate.Text = dr["ClaimDate"].ToString();
                billNo.Text = dr["BillNO"].ToString();
                BegWeb.Text = dr["BegWeb"].ToString();
                BillDate.Text = dr["BillDate"].ToString();
                PickGoodsSite.Text = dr["PickGoodsSite"].ToString();
                TransitMode.Text = dr["TransitMode"].ToString();
                Varieties.Text = dr["Varieties"].ToString();
                Package.Text = dr["Package"].ToString();
                Num.Text = dr["Num"].ToString() + "/" + dr["Num"].ToString() + "/" + dr["Num"].ToString();
                ConsignorName.Text = dr["ConsignorName"].ToString();
                ConsignorCompany.Text = dr["ConsignorCompany"].ToString();
                ConsignorPhone.Text = dr["ConsignorPhone"].ToString() + "/" + dr["ConsignorCellPhone"].ToString();
                AllFee.Text = dr["AllFee"].ToString();
                ConsigneeName.Text = dr["ConsigneeName"].ToString();
                ConsigneeCompany.Text = dr["ConsigneeCompany"].ToString();
                ConsigneePhone.Text = dr["ConsigneePhone"].ToString() + "/" + dr["ConsigneeCellPhone"].ToString();
                SignDate.Text = dr["SignDate"].ToString();
                ClaimerType.Text = dr["ClaimerType"].ToString();
                PaymentMode.Text = dr["PaymentMode"].ToString();
                Claimer.Text = dr["Claimer"].ToString();
                ClaimerPhone.Text = dr["ClaimerPhone"].ToString();
                ClaimMoney.Text = dr["ClaimMoney"].ToString();
                BankUserName.Text = dr["BankUserName"].ToString();
                BankName.Text = dr["BankName"].ToString();
                BankNO.Text = dr["BankNO"].ToString();
                ExceptContent.Text = dr["ClaimReason"].ToString();
                ExceptType.Text = dr["ExceptType"].ToString();
                DutyDepart.Text = dr["DutyDepart"].ToString();
                DutyMan.Text = dr["DutyMan"].ToString();
                ClaimAuditMoney.Text = dr["ClaimAuditMoney"].ToString();
                AddClaimMan.Text = dr["AddClaimMan"].ToString();

                if (ConvertType.ToInt32(dr["ClaimState"]) == 1)
                {
                    ClaimerType.Properties.ReadOnly = false;
                //    PaymentMode.ReadOnly = false;
                    Claimer.ReadOnly = false;
                    ClaimerPhone.ReadOnly = false;
                    ClaimMoney.ReadOnly = false;
                    BankUserName.ReadOnly = false;
                    BankName.ReadOnly = false;
                    BankNO.ReadOnly = false;
                    barButtonItem2.Enabled = false;
                }
                if (ConvertType.ToInt32(dr["ClaimState"]) == 4)
                {
                    DutyDepart.Properties.ReadOnly = false;
                    DutyMan.ReadOnly = false;
                  //  ClaimAuditMoney.ReadOnly = false;
                }
                if (ConvertType.ToInt32(dr["ClaimState"]) == 8)
                {
                    ClaimAuditMoney.ReadOnly = false;
                    AddClaimMan.ReadOnly = false;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
          //  if (dr["PaymentMode"].ToString() != "提付" && dr["PaymentMode"].ToString() != "月结" 
          //      && dr["PaymentMode"].ToString() != "两笔付" && ClaimerType.Text == "抵账")
          //  {
          //      MsgBox.ShowOK("只有付款方式为提付或月结的运单才能选择抵账");
          //      return;
          //  }

          //  frmClaimDialog frm = new frmClaimDialog(dr);
          //  frm.dr = dr;
          //  frm.BillNo = billNoStr;
          //  frm.applyResult = "同意";
          //  if (ConvertType.ToInt32(dr["ClaimState"]) == 4)
          //  {
          //      if (DutyDepart.Text.Trim() == "")
          //      {
          //          MsgBox.ShowError("必须填写责任部门！");
          //          return;
          //      }
          //      if (DutyMan.Text.Trim() == "")
          //      {
          //          MsgBox.ShowError("必须填写责任人！");
          //          return;
          //      }
          //      frm.DutyDepart = DutyDepart.Text.Trim();
          //      frm.DutyMan = DutyMan.Text.Trim();
          //  }
          //  if (ConvertType.ToInt32(dr["ClaimState"]) == 1)
          //  {
          //      frm.ClaimerType = ClaimerType.Text.Trim();
          ////      frm.PaymentMode = PaymentMode.Text.Trim();
          //      frm.Claimer = Claimer.Text.Trim();
          //      frm.ClaimerPhone = ClaimerPhone.Text.Trim();
          //      frm.ClaimMoney = ClaimMoney.Text.Trim();
          //      frm.BankUserName = BankUserName.Text.Trim();
          //      frm.BankName = BankName.Text.Trim();
          //      frm.BankNO = BankNO.Text.Trim();
          //  }
          //  if (ConvertType.ToInt32(dr["ClaimState"]) == 8)
          //  {
          //      if (ClaimAuditMoney.Text.Trim() == "")
          //      {
          //          MsgBox.ShowError("必须填写理赔金额！");
          //          return;
          //      }
          //      if (AddClaimMan.Text.Trim() == "")
          //      {
          //          MsgBox.ShowError("必须填写追偿人！");
          //          return;
          //      }
          //      frm.ClaimAuditMoney = ClaimAuditMoney.Text.Trim();
          //      frm.AddClaimMan = AddClaimMan.Text.Trim();
          //  }
          //  frm.Owner = this;
          //  frm.ShowDialog();
          //  if (isClose)
          //  {
          //      frmClaimApproval parentFrm = (frmClaimApproval)this.Owner;
          //      parentFrm.isClose = true;
          //      this.Close();
          //  }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //frmClaimDialog frm = new frmClaimDialog(dr);
            //frm.BillNo = billNoStr;
            //frm.applyResult = "否决";
            //frm.ShowDialog(this);
            //if (isClose)
            //{
            //    //frmClaimApproval parentFrm = (frmClaimApproval)this.Owner;
            //    //parentFrm.isClose = true;
            //    this.Close();
            //}
        }

        private void printBtn_Click(object sender, EventArgs e)
        {
            if (dr["ClaimState"] == null || (dr["ClaimState"].ToString() != "12" && dr["ClaimState"].ToString() != "13"))
            {
                MsgBox.ShowOK("当前状态不能打印");
                return;
            }

            if (dr["ClaimPrintDate"] != null && dr["ClaimPrintDate"].ToString() != "")
            {
                MsgBox.ShowOK("该流程已经打印过,不能再打印");
                return;
            }

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_ClaimDetail_Print", new List<SqlPara> { new SqlPara("BillNo", dr["BillNO"].ToString()) }));
            if (ds == null || ds.Tables.Count == 0) return;

            // QSP_GET_LinkDetail_Print
            DataSet subDs = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_LinkDetail_Print", new List<SqlPara> { new SqlPara("BillNo", dr["BillNO"].ToString()) }));
            if (subDs == null || subDs.Tables.Count == 0) return;

            frmPrintRuiLang fpr = new frmPrintRuiLang("理赔申请", ds, subDs);
            fpr.ShowDialog();
        }

        private void lookImage_Click(object sender, EventArgs e)
        {
            frmImageLookNew look = new frmImageLookNew();
            look.signNo = dr["SignNO"].ToString();
            look.billNo = dr["BillNo"].ToString();
            look.ClaimWeb = dr["ClaimWeb"].ToString();
            look.ShowDialog();
        }

        //实现快查
        private void button1_Click(object sender, EventArgs e)
        {
            string billNo=this.billNo.Text;
            CommonClass.ShowBillSearch(billNo);
        }
    }
}
