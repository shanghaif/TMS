using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmBargainingAdd : BaseForm
    {
        public string billno { get; set; }
        public string site { get; set; }
        public string web { get; set; }
        public string transLine { get; set; }
        public string transMode { get; set; }
        public string varieties { get; set; }
        public string num { get; set; }
        public string mainFee { get; set; }
        public string mainWeight { get; set; }
        public string perPrice { get; set; }
        public string newMainfee { get; set; }
        public string id { get; set; }
        public string actualFee { get; set; }
        public string transitSite { get; set; }
        public string remark { get; set; }
        public string statu { get; set; }
        public string deliveryFeePer { get; set; }
        public string deliveryFee { get; set; }
        public string newDeliveryFee { get; set; }
        public string acceptCompany { get; set; }
        DataSet dsCompany = new DataSet(); 
        public frmBargainingAdd()
        {
            InitializeComponent();
        }

        private void frmBargainingAdd_Load(object sender, EventArgs e)
        {
            dsCompany = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_COMPANY"));
            string tmp = "";
            if (dsCompany != null && dsCompany.Tables.Count > 0 && dsCompany.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsCompany.Tables[0].Rows)
                {
                    tmp = ConvertType.ToString(dr["companyid"]) + "|" + ConvertType.ToString(dr["gsjc"]);
                    if (tmp != "" && !AcceptCompanyId.Properties.Items.Contains(tmp))
                        AcceptCompanyId.Properties.Items.Add(tmp);
                }
                AcceptCompanyId.Properties.Items.Add("239|苏州可通");

            }

            if (!string.IsNullOrEmpty(id) && statu =="1")
            {
                txtBillNo.Properties.ReadOnly = true;
                txtBillNo.Text = billno;
                LSite.Text = site;
                LWeb.Text = web;
                LTransLine.Text = transLine;
                LTransType.Text = transMode;
                Lgoodsname.Text = varieties;
                LNum.Text = num;
                LManLineFee.Text = mainFee == "" ? "0" : mainFee;
                LMainweight.Text = mainWeight;
                txtperPrice.Text = perPrice == "" ? "0" : perPrice;
                LNewMainFee.Text = newMainfee == "" ? "0" : newMainfee; ;
                LActualFee.Text = actualFee;
                LTransitSite.Text = transitSite;
                txtRemark.Text = remark;
                DeliveryFee.Text = deliveryFee == "" ? "0" : deliveryFee;
                LNewDeliveryFee.Text = newDeliveryFee == "" ? "0" : newDeliveryFee;
                txtperPrice1.Text = deliveryFeePer == "" ? "0" : deliveryFeePer;
                AcceptCompanyId.Text = acceptCompany;
                if (ConvertType.ToDecimal(mainFee) ==0)
                {
                    this.txtperPrice.Properties.ReadOnly = true;
                }
                if (ConvertType.ToDecimal(deliveryFee) == 0)
                {
                    this.txtperPrice1.Properties.ReadOnly = true;
                }


            }
        }
        private string billNo = "";
        public Boolean isOK = false;
        /// <summary>
        /// 议价确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            decimal oldMainLineFee = ConvertType.ToDecimal(LManLineFee.Text);
            decimal newMainLineFee = ConvertType.ToDecimal(LNewMainFee.Text);
            string billno =txtBillNo.Text.Trim();  //zb20190715
            string companyid = AcceptCompanyId.Text.Split('|')[0];
            
            if (newMainLineFee > (oldMainLineFee * (decimal)1.3) || newMainLineFee < (oldMainLineFee * (decimal)0.7))
            {
                frmBargainningDialog dialog = new frmBargainningDialog();
                dialog.Owner = this;
                dialog.ShowDialog();
                if (!isOK)
                {
                    return;
                }
            }

            try
            {
                List<SqlPara> lst = new List<SqlPara>();
                if (string.IsNullOrEmpty( LSite.Text))
                {
                    MsgBox.ShowOK("请先查询运单信息！");
                    return;
                }
                //zb20190715
                if(!string.IsNullOrEmpty(billno))
                {
                    string confirmState = "";
                    string applyCompanyid = "";
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("billNo", txtBillNo.Text.Trim()));
                    DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_SZBargainin_By_BillNo", list));
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            confirmState = ds.Tables[0].Rows[i]["confirmState"].ToString();
                            applyCompanyid = ds.Tables[0].Rows[i]["ApplyWebName"].ToString();
                            if (statu != "1")
                            {
                                if (confirmState != "否决")
                                {
                                    MsgBox.ShowOK("存在待执行/已执行的数据，不允许再次新增！");
                                    this.Close();
                                    return;
                                    
                                }
                            }
                            else if (applyCompanyid != CommonClass.UserInfo.WebName)
                            {
                                MsgBox.ShowOK("只有申请网点才可以修改！");
                                this.Close();
                                return;
                               
                            }
                        }
                    }
                }
                if (ConvertType.ToDecimal(LNewMainFee.Text.Trim()) < 0 || ConvertType.ToDecimal(LNewDeliveryFee.Text.Trim()) < 0)
                {
                    MsgBox.ShowOK("议价后结算干线费/结算送货费小于0");
                    return;
                }
                if (LNewMainFee.Text.Trim() == "" || ConvertType.ToDecimal(LNewMainFee.Text.Trim()) == 0 || LNewDeliveryFee.Text.Trim() == "" || ConvertType.ToDecimal(LNewDeliveryFee.Text.Trim()) == 0)
                {
                    if (MsgBox.ShowYesNo("议价后结算干线费/议价后结算送货费为0 \r\n是否继续保存？") == DialogResult.No) return;
                }
                string idstr = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id;
                lst.Add(new SqlPara("ID", idstr));
                lst.Add(new SqlPara("billNo", billno));
                lst.Add(new SqlPara("siteName", LSite.Text));
                lst.Add(new SqlPara("webName", LWeb.Text));
                lst.Add(new SqlPara("mainLinefee", LManLineFee.Text));
                lst.Add(new SqlPara("perMainLine", txtperPrice.Text.Trim()));
                lst.Add(new SqlPara("newMainLinefee", LNewMainFee.Text));
                lst.Add(new SqlPara("Varieties", Lgoodsname.Text));
                lst.Add(new SqlPara("Num", LNum.Text));
                lst.Add(new SqlPara("TransferSite",LTransitSite.Text));
                lst.Add(new SqlPara("TransitMode",LTransType.Text));
                lst.Add(new SqlPara("TransitLines", LTransLine.Text));
                lst.Add(new SqlPara("ActualFreight",LActualFee.Text));
                lst.Add(new SqlPara("OperationWeight", LMainweight.Text));
                lst.Add(new SqlPara("remark",txtRemark.Text.Trim()));
                lst.Add(new SqlPara("DeliveryFee", DeliveryFee.Text));
                lst.Add(new SqlPara("NewDeliveryFee", LNewDeliveryFee.Text));
                lst.Add(new SqlPara("DeliveryFeePer", txtperPrice1.Text.Trim()));
                lst.Add(new SqlPara("AcceptCompanyId", companyid));

                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute,"QSP_ADD_BillNoInfo",lst))>0)
                {
                    billno = billno + "@";
                    CommonSyn.ADDBillRoute(billno, CommonClass.UserInfo.companyid, companyid);
                    #region 239公司执行异步同步
                    if (companyid == "239")
                    {
                        List<SqlPara> listSYN = new List<SqlPara>();
                        listSYN.Add(new SqlPara("ID", idstr));
                        SqlParasEntity speSyn = new SqlParasEntity(OperType.Query, "QSP_GET_BargainingInfo_ID", listSYN);
                        DataSet dsSyn = SqlHelper.GetDataSet(speSyn);
                        if (dsSyn != null && dsSyn.Tables.Count > 0 && dsSyn.Tables[0].Rows.Count > 0)
                        {
                            CommonSyn.BargainAPPLYYDSYN(dsSyn);
                        }

                    }
                    #endregion
                    MsgBox.ShowOK();
                    txtBillNo.Focus();   
                    this.DialogResult = DialogResult.Yes;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void txtperPrice_EditValueChanged(object sender, EventArgs e)
        {
            decimal perPrice = 0;
            decimal LManLineFee =0;
            try
            {
                if (!StringHelper.IsDecimal(txtperPrice.Text.Trim()))
                {
                    MsgBox.ShowError("干线议价减少金额格式不正确！");
                    return;
                }
                if (!StringHelper.IsDecimal(txtperPrice1.Text.Trim()))
                {
                    MsgBox.ShowError("议价减少送货费格式不正确！");
                    return;
                }
                LManLineFee = ConvertType.ToDecimal(this.LManLineFee.Text.Trim().ToString());
                perPrice = ConvertType.ToDecimal(txtperPrice.Text.Trim());
                LNewMainFee.Text = ConvertType.ToDecimal((LManLineFee - perPrice)).ToString();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        private void btnQuery_Click(object sender, EventArgs e)
        {
            billNo = txtBillNo.Text.Trim();
            if (string.IsNullOrEmpty(billNo))
            {
                MsgBox.ShowOK("请输入运单号！");
                return;
            }
          

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BillNoInfo", new List<SqlPara>() { new SqlPara("billNo", txtBillNo.Text.Trim()) }));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                LSite.Text = "";
                LWeb.Text = "";
                LTransLine.Text = "";
                LTransType.Text = "";
                Lgoodsname.Text = "";
                LNum.Text = "0";
                LManLineFee.Text = "0";
                LMainweight.Text = "0";
                LActualFee.Text = "0";
                LTransitSite.Text = "";
                MsgBox.ShowError("该运单不存在，请检查!");
                return;
            }
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
               
                if (dr["BegWeb"].ToString() != CommonClass.UserInfo.WebName)
                {
                    MsgBox.ShowError("不是本网点开的单，不允许议价!");
                    return;
                }
                LSite.Text = dr["StartSite"].ToString();
                LWeb.Text = dr["BegWeb"].ToString();
                LTransLine.Text = dr["TransitLines"].ToString();
                LTransType.Text = dr["TransitMode"].ToString();
                Lgoodsname.Text = dr["Varieties"].ToString();
                LNum.Text = dr["Num"].ToString();
                LManLineFee.Text = dr["MainlineFee"].ToString();
                LMainweight.Text = dr["OperationWeight"].ToString();
                LActualFee.Text = dr["ActualFreight"].ToString();
                LTransitSite.Text = dr["TransferSite"].ToString();
                DeliveryFee.Text = dr["DeliveryFee"].ToString();
                if (ConvertType.ToDecimal(this.LManLineFee.Text) == 0 && ConvertType.ToDecimal(this.DeliveryFee.Text) == 0)
                {
                    MsgBox.ShowError("结算干线费与结算送货费为0,不能议价!");
                    return;
                }
                if (ConvertType.ToDecimal(this.LManLineFee.Text) == 0)
                {
                    this.txtperPrice.Properties.ReadOnly = true;
                }
                if (ConvertType.ToDecimal(this.DeliveryFee.Text) == 0)
                {
                    this.txtperPrice1.Properties.ReadOnly = true;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtperPrice1_EditValueChanged(object sender, EventArgs e)
        {
            decimal txtperPrice1 = 0;
            decimal LDeliveryFee = 0;
            try
            {
                if (!StringHelper.IsDecimal(this.txtperPrice1.Text.Trim()))
                {
                    MsgBox.ShowError("议价单价格式不正确！");
                    return;
                }
                LDeliveryFee = ConvertType.ToDecimal(this.DeliveryFee.Text.Trim().ToString());
                txtperPrice1 = ConvertType.ToDecimal(this.txtperPrice1.Text.Trim());
                LNewDeliveryFee.Text = ConvertType.ToDecimal((LDeliveryFee - txtperPrice1)).ToString();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
    }
}