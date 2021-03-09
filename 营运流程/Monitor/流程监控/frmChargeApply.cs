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
using Newtonsoft.Json;
namespace ZQTMS.UI
{
    public partial class frmChargeApply : BaseForm
    {
        public string crrBillNO = "";
        private DataRow dr;
        public frmChargeApply()
        {
            InitializeComponent();
        }
        private string sBegWeb = "";
        private string sTagetWeb = "";
        private void frmChargeApply_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("网点费用异动");//xj/2019/5/29
            if (!string.IsNullOrEmpty(crrBillNO.Trim()))
            {
                GetDataByBillNO();
            }
            CommonClass.SetSite(zdhxzd, false);
            textEdit9.EditValue = CommonClass.UserInfo.UserName;
            DataEdit1.DateTime = DateTime.Now;

            switch (jsfyfkf.Text)
            {
                case "发货方":
                    if (zjkm.Text == "送货费" || zjkm.Text == "装卸费" || zjkm.Text == "上楼费" || zjkm.Text == "进仓费" || zjkm.Text == "叉车费" || zjkm.Text == "报关费" || zjkm.Text == "其他费")
                    {
                        lzdhxwd.Visible = true;
                        zdhxwd.Visible = true;
                        label1.Visible = true;
                        label5.Visible = true; 
                        label6.Visible = true;
                        txtTerminal.Visible = true;
                    }
                    else
                    {
                        lzdhxwd.Visible = false;
                        zdhxwd.Visible = false;
                        label1.Visible = false;
                        txtTerminal.Visible = false;
                        label5.Visible = false;
                        label6.Visible = false;
                        txtTerminal.Text = "0";
                    }
                    break;
            }

            

            if (CommonClass.UserInfo.WebRole=="加盟")
            {
                jsfyfkf.Enabled = false;
                jjfs.Enabled = false;
                if (zjkm.Properties.Items.Count > 0)
                {
                    zjkm.Properties.Items.Clear();  
                }
                zjkm.Properties.Items.Add("送货费");
                zjkm.Properties.Items.Add("装卸费");
                zjkm.Properties.Items.Add("上楼费");
                zjkm.Properties.Items.Add("进仓费");
                zjkm.Properties.Items.Add("叉车费");
                zjkm.Properties.Items.Add("其他费");
            }
            labelControl13.Visible = false;
            labelControl63.Visible = false;
            comboBoxEdit1.Visible = false;
            comboBoxEdit2.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            SetSite(comboBoxEdit1, false);
        }


        /// <summary>
        /// 根据运单编号获取运单
        /// </summary>
        private bool GetDataByBillNO()
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillNO", crrBillNO));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_ByID_Auto", list);
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

            zdhxzd.Text = dr["TransferSite"].ToString();
            if (zdhxwd.Visible == true)
            {
                zdhxwd.Text = dr["TargetWeb"].ToString();
            }
            else
            {
                zdhxwd.Text = "";
            }

            if (dr["IsInvoice"] != null && dr["IsInvoice"].ToString() == "1")
                IsInvoice.Checked = true;
            else
                IsInvoice.Checked = false;


            if (dr["NoticeState"].ToString().ToString() == "1")
            {
                //ConsigneeCompany_K.EditValue = "88888888";
                //ConsigneeName_K.EditValue = "88888888";
                //ConsigneePhone_K.EditValue = "88888888";
                //ConsigneeCellPhone_K.EditValue = "88888888";
                //ConsigneeCompany_K.Enabled = false;
                //ConsigneeName_K.Enabled = false;
                //ConsigneePhone_K.Enabled = false;
                //ConsigneeCellPhone_K.Enabled = false;

                NoticeState.Checked = true;
                //IsReleaseGoods.Checked = false;
            }
            else
            {
                //ConsigneeCompany_K.EditValue = dr["ConsigneeCompany_K"];
                //ConsigneeName_K.EditValue = dr["ConsigneeName_K"];
                //ConsigneePhone_K.EditValue = dr["ConsigneePhone_K"];
                //ConsigneeCellPhone_K.EditValue = dr["ConsigneeCellPhone_K"];
                //ConsigneeCompany_K.Enabled = true;
                //ConsigneeName_K.Enabled = true;
                //ConsigneePhone_K.Enabled = true;
                //ConsigneeCellPhone_K.Enabled = true;

                //NoticeState.Checked = true;
                //IsReleaseGoods.Checked = true;
                NoticeState.Checked = false;
            }
            if (!string.IsNullOrEmpty(MonthPay.Text.Trim()) && Convert.ToDecimal(MonthPay.Text.Trim()) > 0)
            {
                ydzj.Enabled = false;
                ydzj.Text = "0";
            }

            return true;
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
            if (string.IsNullOrEmpty(zdhxzd.Text) && string.IsNullOrEmpty(comboBoxEdit1.Text))  //maohui20180516
            {
                MsgBox.ShowOK("指定核销站点不能为空！");
                return;
            }
            
            //if (!string.IsNullOrEmpty(ShortOwePay.Text.Trim()) && Convert.ToDecimal(ShortOwePay.Text.Trim()) > 0)      //whf20190722
            //{
            //    MsgBox.ShowOK("有短欠金额，不能做提付异动！");
            //    return;
            //}


            if (dr["BillState"].ToString() == "0")
            {
                MsgBox.ShowOK("此单为新开状态，请直接修改");
                return;
            }   //plh20191207
            string _jjfs = jjfs.Text;
            string _zjkm = zjkm.Text;
            double _ydzj = 0, Terminal=0;

            string str = Convert.ToString(TransferMode.EditValue);      //zhengjiafneg20181019
            if (str.Equals("送货") && !string.IsNullOrEmpty(jjfs.Text.Trim()))
            {
                MsgBox.ShowOK("此单已是送货，无需改为自提改送，直接加上异动金额即可！");
                return;
            }

            if (!StringHelper.IsDecimal(ydzj.Text.Trim()) && !string.IsNullOrEmpty(ydzj.Text.Trim()))  //maohui20180516
            {
                MsgBox.ShowOK("异动增加金额不合法，请重新输入！");
                ydzj.Focus();
                return;
            }
            if (!StringHelper.IsDecimal(txtTerminal.Text.Trim()) && !string.IsNullOrEmpty(txtTerminal.Text.Trim()))
            {
                MsgBox.ShowOK("终端结算费格式不合法，请重新输入！");
                txtTerminal.Focus();
                return;
            }
            if(this.jsfyfkf.Text.Trim() == "收货方"&&ydzj.Text.Trim()=="")
            {
                MsgBox.ShowOK("异动增加金额格式不合法，请重新输入！");
                ydzj.Focus();
                return;

            }
            if (this.jsfyfkf.Text.Trim() == "收货方" && Convert.ToDecimal(ydzj.Text.Trim()) < Convert.ToDecimal(txtTerminal.Text.Trim()))
            {
                MsgBox.ShowOK("终端结算费不能大于异动增加金额，请重新输入！");
                ydzj.Focus();
                return;
            }
            _ydzj = ConvertType.ToDouble(ydzj.Text.Trim());
            Terminal = ConvertType.ToDouble(txtTerminal.Text.Trim());
            //Terminal = decimal.Parse(txtTerminal.Text.Trim()); 
            //try
            //{
            //    _ydzj = decimal.Parse(ydzj.Text.Trim());
                
            //}
            //catch
            //{
            //    MsgBox.ShowOK("异动增加金额不合法，请重新输入！");
            //    ydzj.Text = "";
            //    ydzj.Focus();
            //    return;
            //}
            string _jsfyfkf = jsfyfkf.Text;
            string _zdhxd = ""; //zdhxzd.Text;  //maohui20180516
            if (zdhxzd.Visible)
            {
                _zdhxd = zdhxzd.Text;
            }
            else if (comboBoxEdit1.Visible)
            {
                _zdhxd = comboBoxEdit1.Text.Trim();
            }
            string _ydyy = ydyy.Text;
            DateTime date = DataEdit1.DateTime;
            string _ydry = textEdit9.Text;
            string recWeb = "";
            string _SerialNumber = CommonClass.gcdate.ToString("yyyyMMddHHmmsss") + new Random().Next(1000, 10000);
            int recType=0;
            if (jsfyfkf.Text == "发货方")
            {
                if (zjkm.Text == "送货费" || zjkm.Text == "装卸费" || zjkm.Text == "上楼费" || zjkm.Text == "进仓费" || zjkm.Text == "叉车费" || zjkm.Text == "报关费" || zjkm.Text == "其他费")
                {
                    //recWeb = zdhxwd.Text.Trim();  //maohui20180516
                    if (zdhxwd.Visible)
                    {
                        recWeb = zdhxwd.Text.Trim();
                    }
                    else if (comboBoxEdit2.Visible)
                    {
                        recWeb = comboBoxEdit2.Text.Trim();
                    }
                    if (string.IsNullOrEmpty(recWeb))
                    {
                        MsgBox.ShowOK("请选择终端接收网点！");
                        return;
                    }
                    recType = 1;
                }
                else {
                    recWeb = "";
                    recType = 0;
                }
            }

            try
            {
                List<SqlPara> list1 = new List<SqlPara>();
                list1.Add(new SqlPara("BillNO", dr["BillNO"]));
                list1.Add(new SqlPara("ApplyType", "运费异动"));
                SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_BILLAPPLY_HasApplying_Auto", list1);
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
                list.Add(new SqlPara("RecWeb", recWeb));//终端接收网点
                list.Add(new SqlPara("RecType", recType));//推送类型
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
                list.Add(new SqlPara("AmountMoney", Terminal));//终端结算费
                list.Add(new SqlPara("SerialNumber", _SerialNumber));//序列号

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLFREIGHTCHANGES_AUTO", list);
                int result = SqlHelper.ExecteNonQuery(sps);
                if (result > 0)
                {
                    MsgBox.ShowOK("操作成功！");

                    #region //财务增减费用接口
                    {
                        List<SqlPara> lists = new List<SqlPara>();
                        lists.Add(new SqlPara("BillNo", crrBillNO));
                        SqlParasEntity spsa = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_ByID_Auto_BillNo", lists);
                        DataSet dds = SqlHelper.GetDataSet(spsa);
                        DataRow ddr = dds.Tables[0].Rows[0];
                        if (ddr["BegWeb"].ToString() == "三方")
                        {
                            Dictionary<string, object> hashMap1 = new Dictionary<string, object>();
                            hashMap1.Add("carriageSn", crrBillNO);
                            hashMap1.Add("derectFlag", 1);
                            hashMap1.Add("fee", _ydzj);
                            hashMap1.Add("changeReason", _ydyy);
                            hashMap1.Add("fileName", "");
                            hashMap1.Add("changeId", Convert.ToString(ddr["ApplyID"]));
                            string json1 = JsonConvert.SerializeObject(hashMap1);
                            string url1 = "http://120.76.141.227:9882/umsv2.biz/open/fee/change/trunk_freight_change";
                            try
                            {
                                HttpHelper.HttpPostJava(json1, url1);
                                MessageBox.Show("推送成功");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                    #endregion

                    //CommonSyn.FreightChangesADDSyn(crrBillNO, recType); //maohui20180514

                    #region hj20190423 费用异动同步
                    List<SqlPara> listSYN = new List<SqlPara>();
                    listSYN.Add(new SqlPara("BillNO", crrBillNO));
                    listSYN.Add(new SqlPara("ApplyType", "运费异动"));
                    listSYN.Add(new SqlPara("SerialNumber", _SerialNumber));
                    SqlParasEntity speSyn = new SqlParasEntity(OperType.Query, "QSP_GETZQTMSCHARGEAPPLY_SYN", listSYN);
                    DataSet dsSyn = SqlHelper.GetDataSet(speSyn);
                    if (dsSyn != null && dsSyn.Tables.Count > 0 && dsSyn.Tables[0].Rows.Count > 0)
                    {
                        CommonSyn.BILLAPPLYYDSYN(dsSyn);

                    }
                    #endregion


                  
                    this.Close();
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

        private void fun()
        {
            #region //财务增减费用接口
            {
                List<SqlPara> lists = new List<SqlPara>();
                lists.Add(new SqlPara("BillNo", "210204000007"));
                SqlParasEntity spsa = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_ByID_Auto_BillNo", lists);
                DataSet dds = SqlHelper.GetDataSet(spsa);
                DataRow ddr = dds.Tables[0].Rows[0];
                if (ddr["BegWeb"].ToString() == "三方")
                {
                    Dictionary<string, object> hashMap1 = new Dictionary<string, object>();
                    hashMap1.Add("carriageSn", "210204000003");
                    hashMap1.Add("derectFlag", 1);
                    hashMap1.Add("fee", 12.3);
                    hashMap1.Add("changeReason", "财务增减费用接口");
                    hashMap1.Add("fileName", "");
                    hashMap1.Add("changeId",Convert.ToString(ddr["ApplyID"]));
                    string json1 = JsonConvert.SerializeObject(hashMap1);
                    string url1 = "http://120.76.141.227:9882/umsv2.biz/open/fee/change/trunk_freight_change";
                    try
                    {
                        HttpHelper.HttpPostJava(json1, url1);
                        MessageBox.Show("推送成功");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        MessageBox.Show("推送失败");
                    }
                }
            }
            #endregion
        }


        private void Q_BillNO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string billno = Q_BillNO.Text.Trim();
                if (crrBillNO != billno && !string.IsNullOrEmpty(billno))
                {
                    crrBillNO = billno;
                    GetDataByBillNO();
                }
                List<SqlPara> list = new List<SqlPara>();  //maohui20180515
                list.Add(new SqlPara("BillNo", billno));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_IsFB", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if ((ds.Tables.Count > 0 && ds != null && ds.Tables[0].Rows.Count > 0))
                {
                    labelControl86.Visible = false;
                    lzdhxwd.Visible = false;
                    zdhxzd.Visible = false;
                    zdhxwd.Visible = false;
                    label4.Visible = false;
                    label5.Visible = false;
                    labelControl13.Visible = true;
                    labelControl63.Visible = true;
                    comboBoxEdit1.Visible = true;
                    comboBoxEdit2.Visible = true;
                    label7.Visible = true;
                    label8.Visible = true;
                }
            }
        }

        public static void SetWeb(ComboBoxEdit cb, string SiteName, bool isall)
        {
            if (CommonClass.dsWeb == null || CommonClass.dsWeb.Tables.Count == 0) return;
            try
            {
                if (SiteName == "全部") SiteName = "%%";
                DataRow[] dr = CommonClass.dsWeb.Tables[0].Select("SiteName like '" + SiteName + "'");
                cb.Properties.Items.Clear();
                cb.Text = "";
                for (int i = 0; i < dr.Length; i++)
                {
                    //string webName = dr[i]["WebName"].ToString();
                    //if (webName.Contains("测试"))
                    //{
                    //    continue;
                    //}
                    cb.Properties.Items.Add(dr[i]["WebName"]);
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                    cb.Text = "全部";
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }


        private void jsfyfkf_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (jsfyfkf.Text == "收货方")
            {
                if (zdhxzd.Visible)  //maohui20180516
                {
                    //lzdhxwd.Visible = false;
                    //zdhxwd.Visible = false;
                    //label1.Visible = false;
                    label5.Visible = false;
                    label6.Visible = false;
                    //txtTerminal.Visible = false;
                    //txtTerminal.Text = "0";
                    lzdhxwd.Visible = true;
                    zdhxwd.Visible = true;
                    label1.Visible = true;
                    txtTerminal.Visible = true;


                }
                else if (comboBoxEdit1.Visible)
                {
                    labelControl86.Visible = false;
                    zdhxzd.Visible = false;
                    label4.Visible = false;
                    lzdhxwd.Visible = false;
                    zdhxwd.Visible = false;
                    label1.Visible = false;
                    label5.Visible = false;
                    label6.Visible = false;
                    txtTerminal.Visible = false;
                    txtTerminal.Text = "0";
                    comboBoxEdit2.Visible = false;
                    labelControl63.Visible = false;
                    label8.Visible = false;
                }
            }
            else if (jsfyfkf.Text == "发货方")
            {
                if (zjkm.Text == "送货费" || zjkm.Text == "装卸费" || zjkm.Text == "上楼费" || zjkm.Text == "进仓费" || zjkm.Text == "叉车费" || zjkm.Text == "报关费" || zjkm.Text == "其他费")
                {
                    if (zdhxzd.Visible) //maohui20180516
                    {
                        lzdhxwd.Visible = true;
                        zdhxwd.Visible = true;
                        label1.Visible = true;
                        label5.Visible = true;
                        label6.Visible = true;
                        txtTerminal.Visible = true;
                    }
                    else if (comboBoxEdit1.Visible)
                    {
                        label1.Visible = true;
                        txtTerminal.Visible = true;
                        label6.Visible = true;
                        labelControl63.Visible = true;
                        comboBoxEdit2.Visible = true;
                        label8.Visible = true;
                    }
                }
            }

        }

        private void zjkm_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (jsfyfkf.Text)
            {
                case "发货方":
                    if (zjkm.Text == "送货费" || zjkm.Text == "装卸费" || zjkm.Text == "上楼费" || zjkm.Text == "进仓费" || zjkm.Text == "叉车费" || zjkm.Text == "报关费" || zjkm.Text == "其他费")
                    {
                        if (zdhxzd.Visible) //maohui20180516
                        {
                            lzdhxwd.Visible = true;
                            zdhxwd.Visible = true;
                            label1.Visible = true;
                            label5.Visible = true;
                            label6.Visible = true;
                            txtTerminal.Visible = true;
                        }
                        else if (comboBoxEdit1.Visible)
                        {
                            label1.Visible = true;
                            txtTerminal.Visible = true;
                            label6.Visible = true;
                            labelControl63.Visible = true;
                            comboBoxEdit2.Visible = true;
                            label8.Visible = true;
                        }
                    }
                    else
                    {
                        if (zdhxzd.Visible)  //maohui20180516
                        {
                            lzdhxwd.Visible = false;
                            zdhxwd.Visible = false;
                            label1.Visible = false;
                            label5.Visible = false;
                            label6.Visible = false;
                            txtTerminal.Visible = false;
                            txtTerminal.Text = "0";
                        }
                        else if (comboBoxEdit1.Visible)
                        {
                            label1.Visible = false;
                            txtTerminal.Visible = false;
                            label6.Visible = false;
                            labelControl63.Visible = false;
                            comboBoxEdit2.Visible = false;
                            label8.Visible = false;
                            txtTerminal.Text = "0";
                        }
                    }
                    break;
                case "收货方":
                    if (zdhxzd.Visible)  //maohui20180516
                    {
                        lzdhxwd.Visible = false;
                        zdhxwd.Visible = false;
                        label1.Visible = false;
                        label5.Visible = false;
                        label6.Visible = false;
                        txtTerminal.Visible = false;
                        txtTerminal.Text = "0";
                    }
                    else if (comboBoxEdit1.Visible)
                    {
                        label1.Visible = false;
                        txtTerminal.Visible = false;
                        label6.Visible = false;
                        labelControl63.Visible = false;
                        comboBoxEdit2.Visible = false;
                        label8.Visible = false;
                        txtTerminal.Text = "0";
                    }
                    break;
            }
        }

        private void zdhxzd_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            SetWeb(zdhxwd, zdhxzd.Text, false);
        }

        public static void SetSite(ComboBoxEdit cb, bool isall)  //maohui20180515
        {
            List<SqlPara> list = new List<SqlPara>();
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BasSite_ForFreightChanges", list);
            DataSet dsSite = SqlHelper.GetDataSet(sps);

            if (dsSite == null || dsSite.Tables.Count == 0 || dsSite.Tables[0].Rows.Count == 0) 
                return;
            try
            {
                for (int i = 0; i < dsSite.Tables[0].Rows.Count; i++)
                {
                   cb.Properties.Items.Add(dsSite.Tables[0].Rows[i]["SiteName"].ToString());
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e) //maohui20180515
        {
            comboBoxEdit2.Properties.Items.Clear();
            SetWeb1(comboBoxEdit2, comboBoxEdit1.Text.Trim(), false);
        }

        public static void SetWeb1(ComboBoxEdit cb, string SiteName, bool isall) //maohui20180515
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                if (SiteName == "全部")
                {
                    SiteName = "%%";
                }
                list.Add(new SqlPara("SiteName", SiteName));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BasWeb_ForFreightChanges", list);
                DataSet dsWeb = SqlHelper.GetDataSet(sps);
                if (dsWeb == null || dsWeb.Tables.Count == 0 || dsWeb.Tables[0].Rows.Count == 0)
                    return;
                cb.Properties.Items.Clear();
                cb.Text = "";
                for (int i = 0; i < dsWeb.Tables[0].Rows.Count; i++)
                {
                    cb.Properties.Items.Add(dsWeb.Tables[0].Rows[i]["WebName"].ToString());
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                    cb.Text = "全部";
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

    }
}