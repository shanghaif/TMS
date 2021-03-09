using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using DevExpress.XtraGrid.Columns;
using DevExpress.Utils;
using System.Diagnostics;
using DevExpress.XtraEditors.Controls;
using System.Threading;
using DevExpress.XtraReports.UI;
using System.Collections;
using Newtonsoft.Json;

namespace ZQTMS.UI
{
    public partial class frmWayBillAgentAdd_JMGX_Upgrade : BaseForm
    {
        string str = "";
        decimal banlance = 0;
        int state = 0;
        DataSet ds1 = new DataSet();

        public frmWayBillAgentAdd_JMGX_Upgrade()
        {
            InitializeComponent();
            if (CommonClass.UserInfo.companyid == "132" && isModify != 1 && Upd_Num == 0)
            {
                this.IsReceiptFee.Checked = true;
                this.ReceiptCondition.Text = "签回单";
            }
        }

        public frmWayBillAgentAdd_JMGX_Upgrade(string unum)
        {
            InitializeComponent();
            Upd_Num = int.Parse(unum);
            this.Text = "改单申请";
        }

        public int isModify = 0;
        public string BillNO = "";
        public DataRow dr;
        //hj20180905 送货不用填写街道的公司
        //string[] NotStreet = null;
        //定义全局省市区街道，保存这四个字段
        string ReceivProvince1 = "";
        string ReceivCity1 = "";
        string ReceivArea1 = "";
        string ReceivStreet1 = "";


        /// <summary>
        /// 0开单 1改单申请 
        /// </summary>
        public int Upd_Num = 0;
        frmWayBillAgentAdd_JMGX_Upgrade fwb;
        //zaj 2017-11-19 将mygridcontrol2设置成只显示一行
        private int RowCount = 2;
        private DataSet dsModified = new DataSet();
        public DataRow xmdr = null;
        public string xmbillno = "";

        public DataRow alidr = null;
        public string alibillno = "";
        public XtraReport rpt;//为了加快打印标签的显示速度
        int isCompany = 0; //公司权限标识，默认0
        public DataSet dsDenominatedFee = new DataSet();//客户计价标准maohui20181228
        public string gsDenominatedFee = "";//标记是否需要客户计价maohui20181229

        //public frmWayBillAgentAdd_JMGX_Upgrade Get_frmWayBillAdd_JMGX_HD
        //{
        //    get
        //    {
        //        if (fwb == null || fwb.IsDisposed)
        //        {
        //            fwb = new frmWayBillAdd_JMGX_HD();
        //        }
        //        return fwb;
        //    }
        //}

        private void comboBoxEdit10_SelectedValueChanged(object sender, EventArgs e)
        {
            NowPay.Text = "";
            FetchPay.Text = "";
            MonthPay.Text = "";
            OwePay.Text = "";
            BefArrivalPay.Text = "";
            ReceiptPay.Text = "";
            ShortOwePay.Text = "";

            if (PaymentMode.Text.Trim() == "现付")
            {
                NowPay.Text = PaymentAmout.Text;
            }
            if (PaymentMode.Text.Trim() == "提付")
            {
                FetchPay.Text = PaymentAmout.Text;
            }
            if (PaymentMode.Text.Trim() == "月结")
            {
                MonthPay.Text = PaymentAmout.Text;
            }
            if (PaymentMode.Text.Trim() == "短欠")
            {
                ShortOwePay.Text = PaymentAmout.Text;
                //NoticeState.Checked = true; hj20181114
            }

            //zaj 2017-11-22
            if (PaymentMode.Text.Trim() == "回单付")
            {
                ReceiptPay.Text = PaymentAmout.Text;
            }
            if (PaymentMode.Text.Trim() == "欠付")
            {
                OwePay.Text = PaymentAmout.Text;
            }
            if (PaymentMode.Text.Trim() == "货到前付")
            {
                BefArrivalPay.Text = PaymentAmout.Text;
                NoticeState.Checked = true;
            }
            else
            {
                if (Upd_Num == 0 && isModify != 1)
                {
                    NoticeState.Checked = false;
                }

            }
            if (PaymentMode.Text.Trim() == "提付改欠")
            {
                FetchPay.Text = PaymentAmout.Text;
            }
            // zaj 当付款方式为免费时 清空数据
            if (PaymentMode.Text.Trim() == "免费")
            {
                NowPay.Text = "";
                FetchPay.Text = "";
                MonthPay.Text = "";
                OwePay.Text = "";
                ReceiptPay.Text = "";
                ShortOwePay.Text = "";
                BefArrivalPay.Text = "";


            }
            //if (PaymentMode.Text.Trim() == "货到前付")
            //{
            //    BefArrivalPay.Text = PaymentAmout.Text;
            //}

            if (PaymentMode.Text.Trim() == "两笔付")
            {
                NowPay.Text = "";
                FetchPay.Text = "";
                MonthPay.Text = "";
                ShortOwePay.Text = "";
                BefArrivalPay.Text = "";

                NowPay.Enabled = true;
                FetchPay.Enabled = true;
                MonthPay.Enabled = true;
                OwePay.Enabled = false;
                //BefArrivalPay.Enabled = false;
                BefArrivalPay.Enabled = true;
                ReceiptPay.Enabled = true;
                ShortOwePay.Enabled = true;

            }
            else
            {
                NowPay.Enabled = false;
                FetchPay.Enabled = false;
                MonthPay.Enabled = false;
                OwePay.Enabled = false;
                BefArrivalPay.Enabled = false;
                ReceiptPay.Enabled = false;
                ShortOwePay.Enabled = false;
            }
        }

        private void comboBoxEdit4_Enter(object sender, EventArgs e)
        {
            //gcjiehuodanhao.Left = VehicleNum.Left;
            //gcjiehuodanhao.Top = VehicleNum.Top + VehicleNum.Height;
            //gcjiehuodanhao.Visible = true;
            //gcjiehuodanhao.BringToFront();

        }

        private void comboBoxEdit4_Leave(object sender, EventArgs e)
        {
            if (!gcjiehuodanhao.Focused)
            {
                gcjiehuodanhao.Visible = false;
            }
        }

        private void comboBoxEdit9_Leave(object sender, EventArgs e)
        {
            if (!gridControl4.Focused)
            {
                gridControl4.Visible = false;
            }
        }

        private void comboBoxEdit9_Enter(object sender, EventArgs e)
        {
            //gridControl4.Left = PickGoodsSite.Left;
            //gridControl4.Top = PickGoodsSite.Top + PickGoodsSite.Height;
            //gridControl4.Visible = true;
            //gridControl4.BringToFront();
            PickGoodsSite.ShowPopup();
        }

        //退出关闭
        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        //修改运单号状态方法
        private void UpdateBillNoState()
        {
            try
            {
                if (!string.IsNullOrEmpty(this.BillNo.Text.Trim()))
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("billno", this.BillNo.Text.Trim()));
                    //list.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));
                    list.Add(new SqlPara("webid", CommonClass.UserInfo.WebName));
                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_BillNo_State", list);
                    if (SqlHelper.ExecteNonQuery(sps) <= 0)
                    {
                        MsgBox.ShowOK("运单号状态还原失败，请联系管理员手动修改！");
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void maolilv_Enter(object sender, EventArgs e)
        {
            gridControl8.Left = ConsignorCompany.Left + 248;
            gridControl8.Top = ConsignorCompany.Top + ConsignorCompany.Height + 88;
            gridControl8.Visible = true;
            gridControl8.BringToFront();

            //if (ConsignorCompany.Text.Trim() == "")
            //{
            //    gridView8.ClearColumnsFilter();
            //}
            //if (myGridView1.RowCount == 0) return;
            //gridControl8.BringToFront();
            //gridControl8.Left = ConsignorCompany.Left;
            //gridControl8.Top = ConsignorCompany.Top + ConsignorCompany.Height - 180;
            //gridControl8.Visible = true;
        }
        private void ConsignorCompany_Enter(object sender, EventArgs e)
        {
            gridControl8.Left = ConsignorCompany.Left + 80;
            gridControl8.Top = ConsignorCompany.Top + ConsignorCompany.Height + 88;
            gridControl8.Visible = true;
            gridControl8.BringToFront();

        }

        private void maolilv_Leave(object sender, EventArgs e)
        {
            if (!gridControl8.Focused)
            {
                gridControl8.Visible = false;
            }
        }

        private void textEdit14_Enter(object sender, EventArgs e)
        {
            gridControl7.Left = ConsigneeCompany.Left + 248;
            gridControl7.Top = ConsigneeCompany.Top + ConsigneeCompany.Height + 110;
            gridControl7.Visible = true;
            gridControl7.BringToFront();
        }
        private void ConsigneeCompany_Enter(object sender, EventArgs e)
        {
            gridControl7.Left = ConsigneeCompany.Left + 80;
            gridControl7.Top = ConsigneeCompany.Top + ConsigneeCompany.Height + 110;
            gridControl7.Visible = true;
            gridControl7.BringToFront();
        }

        private void textEdit14_Leave(object sender, EventArgs e)
        {
            if (!gridControl7.Focused)
            {
                gridControl7.Visible = false;
            }
        }

        private void frmCreateOrder_Load(object sender, EventArgs e)
        {
            TransferSite.Enabled = false;

            // ywc20190425 LMS-3373 暂时关闭落地配功能
            radioGroup1.Visible = false;

            #region 加载检测
            if (CommonClass.UserInfo.UserName == null || CommonClass.Arg.TransferMode == null)
            {
                MsgBox.ShowOK("没有用户信息，不能开单！");
                this.Close();
                return;
            }
            //zb20190723
            if (CommonClass.dsMiddleSite == null && CommonClass.dsMiddleSite.Tables.Count == 0)
            {
                MsgBox.ShowOK("正在加载数据，请稍后.....");
                this.Close();
                return;
            }
            if (CommonClass.dsMiddleSite_WayBill == null && CommonClass.dsMiddleSite_WayBill.Tables.Count == 0)
            {
                MsgBox.ShowOK("正在加载数据，请稍后.....");
                this.Close();
                return;
            }

            #endregion            }
            //二级中转市县信息HJ20180912
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, myGridView2);
            GridOper.SetGridViewProperty(myGridView1, myGridView2);

            DataSet dsite = new DataSet();
            GridOper.RestoreGridLayout(gridView2, "货差货损记录2");
            //myGridControl1.DataSource = CommonClass.dsMiddleSite.Tables[0];
            try
            {
                if (CommonClass.UserInfo.companyid == "309" || CommonClass.UserInfo.companyid == "132" || CommonClass.UserInfo.companyid == "488" || CommonClass.UserInfo.companyid == "485" || CommonClass.UserInfo.companyid == "490" || CommonClass.UserInfo.companyid == "484" || CommonClass.UserInfo.companyid == "483")
                {
                    dsite = CommonClass.dsMiddleSite_WayBill;
                    if (dsite.Tables.Count > 0 && dsite != null)
                    {
                        myGridControl1.DataSource = dsite.Tables[0];
                    }
                }
                else
                {
                    dsite = CommonClass.dsMiddleSite;
                    if (dsite.Tables.Count > 0 && dsite != null)
                    {
                        myGridControl1.DataSource = dsite.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                return;
            }
            CommonClass.SetSite(TransferSite, true);
            //CommonClass.SetSite(StartSite, true);
            string[] str = { "东莞", "中山", "广州", "佛山", "无锡", "江门", "珠海", "玉林", "北流", "武汉", "太仓",
                             "深圳", "贵阳", "昆明", "重庆", "成都", "恩施"};
            if (str.Length > 0)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    StartSite.Properties.Items.Add(str[i]);
                }
                StartSite.SelectedIndex = 0;
            }

            TransferSite.Properties.Items.Remove("全部");
            //StartSite.Properties.Items.Remove("全部");
            CommonClass.SetWeb(PickGoodsSite, true);
            PickGoodsSite.Properties.Items.Remove("全部");
            SetProvince();

            if (Upd_Num == 1)//改单申请过来的
            {
                lb_Upd_BillNO.Visible = true;
                Upd_BillNO.Visible = true;
                BillNo.Enabled = false;
            }
            else
            {
                //DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BillNo", null));
                //if(ds!=null || ds.Tables[0].Rows.Count>0)
                //{
                //    BillNo.Text = ds.Tables[0].Rows[0]["BillNo"].ToString();
                //}                   

                lb_Upd_BillNO.Visible = false;
                Upd_BillNO.Visible = false;
                BillNo.Enabled = true;
            }
            DraftGUID.Text = Guid.NewGuid().ToString();
            //BarMagagerOper.SetBarPropertity(bar2);
            //SetProvince();
            setSite();

            #region 货物信息
            DataTable DtView2 = new DataTable();
            DtView2.Columns.Add("GoodsType", typeof(string));
            DtView2.Columns.Add("Varieties", typeof(string));
            DtView2.Columns.Add("Package", typeof(string));
            DtView2.Columns.Add("Num", typeof(int));
            DtView2.Columns.Add("FeeWeight", typeof(string));

            DtView2.Columns.Add("FeeVolume", typeof(string));
            DtView2.Columns.Add("Weight", typeof(string));
            DtView2.Columns.Add("Volume", typeof(string));
            DtView2.Columns.Add("WeightPrice", typeof(string));
            DtView2.Columns.Add("VolumePrice", typeof(string));
            //新增长宽高 hj20180925
            DtView2.Columns.Add("length", typeof(string));
            DtView2.Columns.Add("width", typeof(string));
            DtView2.Columns.Add("height", typeof(string));


            DtView2.Columns.Add("FeeType", typeof(string));
            //公司167，168基本运费要输入小数点 20180705
            if (CommonClass.UserInfo.companyid == "167" || CommonClass.UserInfo.companyid == "168" || CommonClass.UserInfo.companyid == "152")
            {
                DtView2.Columns.Add("Freight", typeof(decimal));
            }
            else
            {
                DtView2.Columns.Add("Freight", typeof(decimal));
            }
            for (int i = 0; i < RowCount; i++)
            {
                DtView2.Rows.Add(DtView2.NewRow());
            }
            gridControl2.DataSource = DtView2;
            #endregion

            #region 计费信息
            DataTable DtView1 = new DataTable();
            if (CommonClass.UserInfo.companyid == "167")
            {
                DtView1.Columns.Add("DeliFee", typeof(decimal));
            }
            else
            {
                DtView1.Columns.Add("DeliFee", typeof(int));
            }
            DtView1.Columns.Add("DeclareValue", typeof(decimal));
            if (CommonClass.UserInfo.companyid == "167")
            {
                DtView1.Columns.Add("SupportValue", typeof(decimal));
                DtView1.Columns.Add("Tax", typeof(decimal));
                DtView1.Columns.Add("InformationFee", typeof(decimal));

                DtView1.Columns.Add("Expense", typeof(decimal));
                DtView1.Columns.Add("DiscountTransfer", typeof(decimal));
                DtView1.Columns.Add("CollectionPay", typeof(decimal));
                DtView1.Columns.Add("AgentFee", typeof(decimal));
                DtView1.Columns.Add("FuelFee", typeof(decimal));


                DtView1.Columns.Add("UpstairFee", typeof(decimal));
                DtView1.Columns.Add("HandleFee", typeof(decimal));
                DtView1.Columns.Add("ForkliftFee", typeof(decimal));
                DtView1.Columns.Add("StorageFee", typeof(decimal));
                DtView1.Columns.Add("CustomsFee", typeof(decimal));
                DtView1.Columns.Add("MatPay", typeof(decimal));//HJ20180911

                DtView1.Columns.Add("packagFee", typeof(decimal));
                DtView1.Columns.Add("FrameFee", typeof(decimal));
                DtView1.Columns.Add("ChangeFee", typeof(decimal));
                DtView1.Columns.Add("OtherFee", typeof(decimal));
                DtView1.Columns.Add("NoticeFee", typeof(decimal));

                DtView1.Columns.Add("ReceiptFee", typeof(decimal));
                DtView1.Columns.Add("ReceivFee", typeof(decimal));
                DtView1.Columns.Add("WarehouseFee", typeof(decimal));
                DtView1.Columns.Add("DeliveryFee", typeof(decimal));
            }
            else
            {
                DtView1.Columns.Add("SupportValue", typeof(int));
                DtView1.Columns.Add("Tax", typeof(int));
                DtView1.Columns.Add("InformationFee", typeof(int));

                DtView1.Columns.Add("Expense", typeof(int));
                DtView1.Columns.Add("DiscountTransfer", typeof(int));
                DtView1.Columns.Add("CollectionPay", typeof(int));
                DtView1.Columns.Add("AgentFee", typeof(int));
                DtView1.Columns.Add("FuelFee", typeof(int));


                DtView1.Columns.Add("UpstairFee", typeof(int));
                DtView1.Columns.Add("HandleFee", typeof(int));
                DtView1.Columns.Add("ForkliftFee", typeof(int));
                DtView1.Columns.Add("StorageFee", typeof(int));
                DtView1.Columns.Add("CustomsFee", typeof(int));
                DtView1.Columns.Add("MatPay", typeof(decimal));//HJ20180911

                DtView1.Columns.Add("packagFee", typeof(int));
                DtView1.Columns.Add("FrameFee", typeof(int));
                DtView1.Columns.Add("ChangeFee", typeof(int));
                DtView1.Columns.Add("OtherFee", typeof(int));
                DtView1.Columns.Add("NoticeFee", typeof(int));

                DtView1.Columns.Add("ReceiptFee", typeof(int));
                DtView1.Columns.Add("ReceivFee", typeof(int));
                DtView1.Columns.Add("WarehouseFee", typeof(int));
                DtView1.Columns.Add("DeliveryFee", typeof(int));
            }
            DtView1.Columns.Add("OperationWeight", typeof(decimal));
            DtView1.Columns.Add("OptWeight", typeof(decimal));

            DtView1.Rows.Add(DtView1.NewRow());
            gridControl1.DataSource = DtView1;
            #endregion

            #region 其它费用
            //DataTable DtView3 = new DataTable();
            //DtView3.Columns.Add("UpstairFee", typeof(decimal));
            //DtView3.Columns.Add("HandleFee", typeof(decimal));
            //DtView3.Columns.Add("ForkliftFee", typeof(decimal));
            //DtView3.Columns.Add("StorageFee", typeof(decimal));
            //DtView3.Columns.Add("CustomsFee", typeof(decimal));

            //DtView3.Columns.Add("packagFee", typeof(decimal));
            //DtView3.Columns.Add("FrameFee", typeof(decimal));
            //DtView3.Columns.Add("ChangeFee", typeof(decimal));
            //DtView3.Columns.Add("OtherFee", typeof(decimal));
            //DtView3.Columns.Add("NoticeFee", typeof(decimal));

            //DtView3.Columns.Add("ReceiptFee", typeof(decimal));
            //DtView3.Columns.Add("ReceivFee", typeof(decimal));

            //DtView3.Rows.Add();
            //gridControl3.DataSource = DtView3;
            #endregion

            #region 下拉数据
            //zaj 2017-11-15 将交接方式换成 "自提", "送货", "中心直送"
            //string[] TransferModeList = CommonClass.Arg.TransferMode.Split(',');
            //if (TransferModeList.Length > 0)
            //{
            //    for (int i = 0; i < TransferModeList.Length; i++)
            //    {
            //        TransferMode.Properties.Items.Add(TransferModeList[i]);
            //    }
            //    //TransferMode.SelectedIndex = 0;
            //}
            //TransferMode.Properties.Items.Add("中心直送");

            string[] TransferModes = { "自提", "送货", "司机直送" };
            for (int i = 0; i < TransferModes.Length; i++)
            {
                TransferMode.Properties.Items.Add(TransferModes[i]);
            }

            //string[] TransitModeList = CommonClass.Arg.TransitMode.Split(',');
            string[] TransitModeList = { "川胜整车", "川胜项目" };
            if (TransitModeList.Length > 0)
            {
                for (int i = 0; i < TransitModeList.Length; i++)
                {
                    TransitMode.Properties.Items.Add(TransitModeList[i]);
                }
                TransitMode.SelectedIndex = 0;
            }

            string[] PaymentModeList = CommonClass.Arg.PaymentMode.Split(',');
            if (PaymentModeList.Length > 0)
            {
                for (int i = 0; i < PaymentModeList.Length; i++)
                {
                    PaymentMode.Properties.Items.Add(PaymentModeList[i]);
                }
                //PaymentMode.SelectedIndex = 0;
            }
            DataRow[] drqk = CommonClass.dsWeb.Tables[0].Select("WebName='" + CommonClass.UserInfo.WebName + "'");
            if (drqk.Length > 0)
            {
                string IsQk = drqk[0]["IsQk"].ToString();
                if (IsQk == "开启")
                {
                    PaymentMode.Properties.Items.Add("短欠");
                }
            }
            string[] VarietiesList = CommonClass.Arg.Varieties.Split(',');
            for (int i = 0; i < VarietiesList.Length; i++)
            {
                repositoryItemComboBox1.Items.Add(VarietiesList[i]);
            }

            string[] PackageList = CommonClass.Arg.Packag.Split(','); ;
            for (int i = 0; i < PackageList.Length; i++)
            {
                repositoryItemComboBox2.Items.Add(PackageList[i]);
            }
            //string[] ReceivingWayList = CommonClass.Arg.ReceivingWay.Split(','); ;
            //for (int i = 0; i < ReceivingWayList.Length; i++)
            //{
            //    ReceivMode.Properties.Items.Add(ReceivingWayList[i]);
            //}
            string[] ReceiptRequirList = CommonClass.Arg.ReceiptRequir.Split(','); ;
            for (int i = 0; i < ReceiptRequirList.Length; i++)
            {
                ReceiptCondition.Properties.Items.Add(ReceiptRequirList[i]);
            }

            #endregion

            #region 结算信息
            //DataTable DtViewJS = new DataTable();
            //DtViewJS.Columns.Add("MainlineFee", typeof(decimal));
            //DtViewJS.Columns.Add("TransferFee", typeof(decimal));
            //DtViewJS.Columns.Add("DepartureOptFee", typeof(decimal));
            //DtViewJS.Columns.Add("TerminalOptFee", typeof(decimal));
            //DtViewJS.Columns.Add("TerminalAllotFee", typeof(decimal));
            //DtViewJS.Columns.Add("DeliveryFee", typeof(decimal));


            //DtViewJS.Columns.Add("ReceiptFee_C", typeof(decimal));//结算回单费
            //DtViewJS.Columns.Add("NoticeFee_C", typeof(decimal));//结算等通知放货费
            //DtViewJS.Columns.Add("SupportValue_C", typeof(decimal));//结算保价费

            //DtViewJS.Columns.Add("AgentFee_C", typeof(decimal));//结算代收手续费
            //DtViewJS.Columns.Add("PackagFee_C", typeof(decimal));//结算包装费
            //DtViewJS.Columns.Add("OtherFee_C", typeof(decimal));//结算其它费

            //DtViewJS.Columns.Add("HandleFee_C", typeof(decimal));//结算装卸费
            //DtViewJS.Columns.Add("StorageFee_C", typeof(decimal));//结算进仓费
            //DtViewJS.Columns.Add("WarehouseFee_C", typeof(decimal));//结算仓储费

            //DtViewJS.Columns.Add("ForkliftFee_C", typeof(decimal));//结算叉车费
            //DtViewJS.Columns.Add("Tax_C", typeof(decimal));//结算税金
            //DtViewJS.Columns.Add("ChangeFee_C", typeof(decimal));//结算改单费

            //DtViewJS.Columns.Add("UpstairFee_C", typeof(decimal));//结算上楼费
            //DtViewJS.Columns.Add("CustomsFee_C", typeof(decimal));//结算报关费
            //DtViewJS.Columns.Add("FrameFee_C", typeof(decimal));//结算代打木架费

            //DtViewJS.Columns.Add("Expense_C", typeof(decimal));//结算工本费
            //DtViewJS.Columns.Add("FuelFee_C", typeof(decimal));//结算燃油费
            //DtViewJS.Columns.Add("InformationFee_C", typeof(decimal));//结算信息费
            //DtViewJS.Rows.Add(DtViewJS.NewRow());
            //gridView8.BestFitColumns();
            //gridControl6.DataSource = DtViewJS;
            #endregion


            // 2018/06/20 上海中强强制购买保险
            //if (CommonClass.UserInfo.companyid == "105")// || CommonClass.UserInfo.companyid == "106"
            //{
            //    IsSupportValue.Checked = true;
            //}

            gridView2.SetRowCellValue(0, "Package", "纸箱");
            //CommonClass.SetProvince(ReceivProvince);

            StartSite.Text = CommonClass.UserInfo.SiteName;
            CauseName.EditValue = CommonClass.UserInfo.CauseName;
            AreaName.EditValue = CommonClass.UserInfo.AreaName;
            DepName.EditValue = CommonClass.UserInfo.DepartName;
            BillMan.EditValue = CommonClass.UserInfo.UserName;
            begWeb.EditValue = CommonClass.UserInfo.WebName;


            gridView2.Appearance.FocusedCell.BackColor = Color.Yellow;
            gridView2.Appearance.HorzLine.BackColor = Color.Gray;
            gridView2.Appearance.VertLine.BackColor = Color.Gray;

            gridView1.Appearance.FocusedCell.BackColor = Color.Yellow;
            gridView1.Appearance.HorzLine.BackColor = Color.Gray;
            gridView2.Appearance.VertLine.BackColor = Color.Gray;

            gridView1.Appearance.FocusedCell.BackColor = Color.Yellow;
            gridView1.Appearance.HorzLine.BackColor = Color.Gray;
            gridView1.Appearance.VertLine.BackColor = Color.Gray;

            //yzw 强制投保
            //IsSupportValue.Checked = true;--maohui20181225

            if (isModify == 1)
            {
                GetWayBill();
                SetFeeNew();
            }
            DestinationSite.Focus();

            if (CommonClass.dsBasCust.Tables.Count > 0) gridControl8.DataSource = CommonClass.dsBasCust.Tables[0];
            if (CommonClass.dsBasReceiveCust.Tables.Count > 0) gridControl7.DataSource = CommonClass.dsBasReceiveCust.Tables[0];
            QSP_GET_DELIVERY_BILL();

            if (xmdr != null)
            {
                SetXmOrder();
            }
            if (alidr != null)
            {
                SetAliOrder();
            }
            #region 绑定草稿
            CommonClass.SetWeb(CgbegWeb, "全部", true);
            CgbegWeb.Text = CommonClass.UserInfo.WebName;
            CgbegWeb.Enabled = UserRight.GetRight("121187");//有权限才可以选择
            #endregion

            ////根据网点名称加载运单号，条件：公司被授权允许自动加载运单号情况下LD
            //if (isModify == 0)
            //{
            //    GetBillNoForText();
            //}
            //验证公司权限
            if (isModify == 0)
            {
                //zaj 2017-1-13
                //ValiteCompanyPower();
                //if (isCompany == 1)
                if (CommonClass.UserInfo.IsAutoBill == true)
                {
                    this.BillNo.Enabled = false;
                }
            }

            if (Upd_Num == 0 && isModify == 0)//限定死不允许专线及落地配互相之间修改maohui20181206
            {
                radioGroup1.Enabled = true;
            }
            //maohui20180726(佳安物流放开到站限制)(后加上168吉上广185远德再加上210复兴)
            //if (CommonClass.UserInfo.companyid == "167" || CommonClass.UserInfo.companyid == "185" || CommonClass.UserInfo.companyid == "168" || CommonClass.UserInfo.companyid == "210")  
            //{
            //    DestinationSite.Enabled = true;
            //}

            // xtraTabControl1.TabPages.Remove(xtraTabPage4);
            dsDenominatedFee = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_basDenominatedFee"));
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_COMPANY_KT"));
            if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
            {
                gsDenominatedFee = ds.Tables[0].Rows[0]["gsDenominatedFee"].ToString();
            }

            gridView2.Columns["height"].Visible = false;
            gridView2.Columns["width"].Visible = false;
            gridView2.Columns["length"].Visible = false;
        }

        //验证公司权限
        //private void ValiteCompanyPower()
        //{
        //    try
        //    {
        //        //验证公司是否被授权
        //        List<SqlPara> list = new List<SqlPara>();
        //        //list.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));
        //        SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_COMPANY_BY_ID", list);
        //        DataTable dt = SqlHelper.GetDataTable(sps);
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            if (dt.Rows[0]["gsState"].ToString() == "1")
        //            {
        //                isCompany = 1;
        //            }
        //            else
        //            {
        //                isCompany = 0;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MsgBox.ShowException(ex);
        //    }
        //}

        //加载运单号方法
        private string GetBillNoForText()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                //list.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));
                list.Add(new SqlPara("webid", CommonClass.UserInfo.WebName));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_p_bill_billnoInfo_BillNo", list);
                DataTable dt = SqlHelper.GetDataTable(sps);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["billno"].ToString();
                }
                else
                {
                    MsgBox.ShowOK("您已没有运单号可用，请先向所属公司申请运单号后再做开单！");
                    this.Close();
                    this.Dispose();
                    return "";
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                return "";
            }
        }

        private void SetAliOrder()
        {
            DataRow dr = alidr;
            ConsignorName.Text = dr["sender_name"].ToString();
            ConsignorCompany.Text = dr["sender_companyName"].ToString();
            ConsignorPhone.Text = dr["sender_phone"].ToString();
            ConsignorCellPhone.Text = dr["sender_mobile"].ToString();

            ConsigneeName.Text = dr["receiver_name"].ToString();
            ConsigneeCompany.Text = dr["receiver_companyName"].ToString();
            ConsigneePhone.Text = dr["receiver_phone"].ToString();
            ConsigneeCellPhone.Text = dr["receiver_mobile"].ToString();
            //ReceivAddress.Text = dr["receiver_address"].ToString();

            //ReceivProvince.Text = dr["receiver_province"].ToString();
            //ReceivCity.Text = dr["receiver_city"].ToString();
            //ReceivArea.Text = dr["receiver_county"].ToString();

            //第一行
            gridView2.SetRowCellValue(0, "Varieties", dr["cargoName"].ToString());
            gridView2.SetRowCellValue(0, "Package", dr["packageService"].ToString());
            gridView2.SetRowCellValue(0, "Num", dr["totalNumber"].ToString());
            gridView2.SetRowCellValue(0, "Weight", dr["totalWeight"].ToString());
            gridView2.SetRowCellValue(0, "Volume", dr["totalVolume"].ToString());
            gridView2.SetRowCellValue(0, "FeeWeight", dr["totalWeight"].ToString());
            gridView2.SetRowCellValue(0, "FeeVolume", dr["totalVolume"].ToString());
            gridView2.SetRowCellValue(0, "Freight", dr["transportPrice"].ToString());

            //第二行
            gridView1.SetRowCellValue(0, "DeliFee", dr["deliveryPrice"].ToString());
            gridView1.SetRowCellValue(0, "AgentFee", dr["codPrice"].ToString());//代收货款费
            gridView1.SetRowCellValue(0, "CollectionPay", dr["codValue"].ToString());//代收货款
            gridView1.SetRowCellValue(0, "NoticeFee", dr["waitNotifySendPrice"].ToString());//等通知发货费
            gridView1.SetRowCellValue(0, "DeclareValue", dr["insuranceValue"].ToString());//申明价值
            gridView1.SetRowCellValue(0, "SupportValue", dr["insurancePrice"].ToString());//保险费
            gridView1.SetRowCellValue(0, "InformationFee", dr["smsNotifyPrice"].ToString());//短信费
            gridView1.SetRowCellValue(0, "OtherFee", dr["otherPrice"].ToString());//其它费
            gridView1.SetRowCellValue(0, "ReceiptFee", dr["backSignBillPrice"].ToString());//回单费
            gridView1.SetRowCellValue(0, "PackagFee", dr["packageServicePrice"].ToString());//包装费
            gridView1.SetRowCellValue(0, "FuelFee", dr["fuelSurchargePrice"].ToString());//燃油费
            gridView1.SetRowCellValue(0, "ReceivFee", dr["vistReceivePrice"].ToString());//实际接货费


            ReceiptCondition.Text = dr["backSignBill"].ToString();//回单要求
            BillRemark.Text = dr["remark"].ToString();//备注
            checkEdit3.Checked = false;
        }

        private void SetXmOrder()
        {
            try
            {
                BillNo.Enabled = false;
                dr = xmdr;

                ConsignorCompany.EditValueChanging -= new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsignorCompany_EditValueChanging);
                ConsignorName.EditValueChanging -= new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsignorCompany_EditValueChanging);

                ConsigneeCompany.EditValueChanging -= new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsigneeCompany_EditValueChanging);
                ConsigneeName.EditValueChanging -= new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsigneeCompany_EditValueChanging);

                gridView1.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView2_CellValueChanged);
                gridView2.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView2_CellValueChanged);

                ConsigneeCellPhone.EditValue = ConvertType.ToInt32(dr["NoticeState"]) == 0 ? dr["ConsigneeCellPhone"] : dr["ConsigneeCellPhone_K"];
                ConsigneePhone.EditValue = ConvertType.ToInt32(dr["NoticeState"]) == 0 ? dr["ConsigneePhone"] : dr["ConsigneePhone_K"];
                ConsigneeName.EditValue = ConvertType.ToInt32(dr["NoticeState"]) == 0 ? dr["ConsigneeName"] : dr["ConsigneeName_K"];
                ConsigneeCompany.EditValue = ConvertType.ToInt32(dr["NoticeState"]) == 0 ? dr["ConsigneeCompany"] : dr["ConsigneeCompany_K"];
                if (CommonClass.UserInfo.companyid == "239")
                {
                    Address.EditValue = ConvertType.ToInt32(dr["NoticeState"]) == 0 ? dr["ConsigneeCellPhone"] : dr["ConsigneeCellPhone_K"];
                }


                PickGoodsSite.EditValue = dr["PickGoodsSite"];
                //ReceivProvince.EditValue = dr["ReceivProvince"];
                //ReceivCity.EditValue = dr["ReceivCity"];
                // ReceivArea.EditValue = dr["ReceivArea"];
                //ReceivStreet.EditValue = dr["ReceivStreet"];
                Province.EditValue = dr["ReceivProvince"];
                City.EditValue = dr["ReceivCity"];
                Address.EditValue = dr["ReceivAddress"];
                ConsignorCellPhone.EditValue = dr["ConsignorCellPhone"];
                ConsignorPhone.EditValue = dr["ConsignorPhone"];
                ConsignorName.EditValue = dr["ConsignorName"];
                ConsignorCompany.EditValue = dr["ConsignorCompany"];
                //ReceivOrderNo1.EditValue = dr["ReceivMode"];


                ValuationType.EditValue = dr["ValuationType"];
                ReceivOrderNo.EditValue = dr["ReceivOrderNo"];
                Salesman.EditValue = dr["Salesman"];

                BillNo.EditValue = dr["BillNo"];
                VehicleNo.EditValue = dr["VehicleNo"];
                StartSite.EditValue = CommonClass.UserInfo.SiteName;
                TransferMode.EditValue = dr["TransferMode"];
                DestinationSite.EditValue = dr["DestinationSite"];
                TransferSite.EditValue = dr["TransferSite"];

                TransitMode.EditValue = dr["TransitMode"];
                CusOderNo.EditValue = dr["CusOderNo"];

                gridView2.SetRowCellValue(0, "GoodsType", dr["GoodsType"]);
                gridView2.SetRowCellValue(0, "Varieties", dr["Varieties"]);
                gridView2.SetRowCellValue(0, "Package", dr["Package"]);
                gridView2.SetRowCellValue(0, "Num", dr["Num"]);

                gridView2.SetRowCellValue(0, "FeeWeight", dr["FeeWeight"]);
                gridView2.SetRowCellValue(0, "FeeVolume", dr["FeeVolume"]);
                gridView2.SetRowCellValue(0, "Weight", dr["Weight"]);
                gridView2.SetRowCellValue(0, "Volume", dr["Volume"]);
                // gridView2.SetRowCellValue(0, "WeightPrice", dr["WeightPrice"]);

                //gridView2.SetRowCellValue(0, "VolumePrice", dr["VolumePrice"]);
                //gridView2.SetRowCellValue(0, "FeeType", dr["FeeType"]);
                gridView2.SetRowCellValue(0, "Freight", dr["Freight"]);

                //gridView1.SetRowCellValue(0, "DeliFee", dr["DeliFee"]);
                //gridView1.SetRowCellValue(0, "ReceivFee", dr["ReceivFee"]);
                //gridView1.SetRowCellValue(0, "DeclareValue", dr["DeclareValue"]);
                //gridView1.SetRowCellValue(0, "SupportValue", dr["SupportValue"]);
                //gridView1.SetRowCellValue(0, "Rate", dr["Rate"]);

                //gridView1.SetRowCellValue(0, "Tax", dr["Tax"]);
                //gridView1.SetRowCellValue(0, "InformationFee", dr["InformationFee"]);
                //gridView1.SetRowCellValue(0, "Expense", dr["Expense"]);
                //gridView1.SetRowCellValue(0, "NoticeFee", dr["NoticeFee"]);
                //gridView1.SetRowCellValue(0, "DiscountTransfer", dr["DiscountTransfer"]);

                //gridView1.SetRowCellValue(0, "CollectionPay", dr["CollectionPay"]);
                //gridView1.SetRowCellValue(0, "AgentFee", dr["AgentFee"]);
                //gridView1.SetRowCellValue(0, "FuelFee", dr["FuelFee"]);

                //gridView1.SetRowCellValue(0, "UpstairFee", dr["UpstairFee"]);
                //gridView1.SetRowCellValue(0, "HandleFee", dr["HandleFee"]);
                //gridView1.SetRowCellValue(0, "ForkliftFee", dr["ForkliftFee"]);
                //gridView1.SetRowCellValue(0, "StorageFee", dr["StorageFee"]);
                //gridView1.SetRowCellValue(0, "CustomsFee", dr["CustomsFee"]);

                //gridView1.SetRowCellValue(0, "packagFee", dr["packagFee"]);
                //gridView1.SetRowCellValue(0, "FrameFee", dr["FrameFee"]);
                //gridView1.SetRowCellValue(0, "ChangeFee", dr["ChangeFee"]);
                //gridView1.SetRowCellValue(0, "OtherFee", dr["OtherFee"]);
                //gridView1.SetRowCellValue(0, "ReceiptFee", dr["ReceiptFee"]);

                //gridView1.SetRowCellValue(0, "FreightAmount", dr["FreightAmount"]);
                //gridView1.SetRowCellValue(0, "ActualFreight", dr["ActualFreight"]);
                //gridView1.SetRowCellValue(0, "WarehouseFee", dr["WarehouseFee"]);

                ReceiptCondition.EditValue = dr["ReceiptCondition"];
                IsInvoice.Checked = Convert.ToInt32(dr["IsInvoice"].ToString() == "" ? "0" : dr["IsInvoice"].ToString()) > 0;


                PaymentMode.EditValue = dr["PaymentMode"];
                PaymentAmout.EditValue = dr["PaymentAmout"];

                //NowPay.EditValue = dr["NowPay"];
                //FetchPay.EditValue = dr["FetchPay"];
                //MonthPay.EditValue = dr["MonthPay"];
                //ShortOwePay.EditValue = dr["ShortOwePay"];
                //BefArrivalPay.EditValue = dr["BefArrivalPay"];
                BillRemark.EditValue = dr["BillRemark"];
                WebPlatform.EditValue = dr["WebPlatform"];
                WebBillNo.EditValue = dr["WebBillNo"];
                DisTranName.EditValue = dr["DisTranName"];
                DisTranBank.EditValue = dr["DisTranBank"];
                DisTranAccount.EditValue = dr["DisTranAccount"];
                CollectionName.EditValue = dr["CollectionName"];
                CollectionBank.EditValue = dr["CollectionBank"];
                CollectionAccount.EditValue = dr["CollectionAccount"];
                CauseName.EditValue = CauseName.Text.Trim();
                AreaName.EditValue = AreaName.Text.Trim();
                DepName.EditValue = DepName.Text.Trim();

                DisTranBranch.EditValue = dr["DisTranBranch"];
                CollectionBranch.EditValue = dr["CollectionBranch"];
                BillMan.EditValue = CommonClass.UserInfo.UserName;
                begWeb.EditValue = CommonClass.UserInfo.WebName;

                //string sg = dr["DeliveryFee"].ToString();
                //gridView8.SetRowCellValue(0, "MainlineFee", dr["MainlineFee"]);
                //gridView1.SetRowCellValue(0, "DeliveryFee", dr["DeliveryFee"]);
                //gridView8.SetRowCellValue(0, "TransferFee", dr["TransferFee"]);
                //gridView8.SetRowCellValue(0, "DepartureOptFee", dr["DepartureOptFee"]);
                //gridView8.SetRowCellValue(0, "TerminalOptFee", dr["TerminalOptFee"]);
                //gridView8.SetRowCellValue(0, "TerminalAllotFee", dr["TerminalAllotFee"]);

                //gridView8.SetRowCellValue(0, "ReceiptFee_C", dr["ReceiptFee_C"]);
                //gridView8.SetRowCellValue(0, "NoticeFee_C", dr["NoticeFee_C"]);
                //gridView8.SetRowCellValue(0, "SupportValue_C", dr["SupportValue_C"]);

                //gridView8.SetRowCellValue(0, "AgentFee_C", dr["AgentFee_C"]);
                //gridView8.SetRowCellValue(0, "PackagFee_C", dr["PackagFee_C"]);
                //gridView8.SetRowCellValue(0, "OtherFee_C", dr["OtherFee_C"]);

                //gridView8.SetRowCellValue(0, "HandleFee_C", dr["HandleFee_C"]);
                //gridView8.SetRowCellValue(0, "StorageFee_C", dr["StorageFee_C"]);
                //gridView8.SetRowCellValue(0, "WarehouseFee_C", dr["WarehouseFee_C"]);

                //gridView8.SetRowCellValue(0, "ForkliftFee_C", dr["ForkliftFee_C"]);
                //gridView8.SetRowCellValue(0, "Tax_C", dr["Tax_C"]);
                //gridView8.SetRowCellValue(0, "ChangeFee_C", dr["ChangeFee_C"]);

                //gridView8.SetRowCellValue(0, "UpstairFee_C", dr["UpstairFee_C"]);
                //gridView8.SetRowCellValue(0, "CustomsFee_C", dr["CustomsFee_C"]);
                //gridView8.SetRowCellValue(0, "FrameFee_C", dr["FrameFee_C"]);

                //gridView8.SetRowCellValue(0, "Expense_C", dr["Expense_C"]);
                //gridView8.SetRowCellValue(0, "FuelFee_C", dr["FuelFee_C"]);
                //gridView8.SetRowCellValue(0, "InformationFee_C", dr["InformationFee_C"]);

                AlienGoods.Checked = Convert.ToInt32(dr["AlienGoods"].ToString() == "" ? "0" : dr["AlienGoods"].ToString()) > 0;
                GoodsVoucher.Checked = Convert.ToInt32(dr["GoodsVoucher"].ToString() == "" ? "0" : dr["GoodsVoucher"].ToString()) > 0;
                PreciousGoods.Checked = Convert.ToInt32(dr["PreciousGoods"].ToString() == "" ? "0" : dr["PreciousGoods"].ToString()) > 0;
                NoticeState.Checked = Convert.ToInt32(dr["NoticeState"].ToString() == "" ? "0" : dr["NoticeState"].ToString()) > 0;

                IsReceiptFee.Checked = ConvertType.ToInt32(dr["IsReceiptFee"]) > 0;
                IsSupportValue.Checked = ConvertType.ToInt32(dr["IsSupportValue"]) > 0;
                IsAgentFee.Checked = ConvertType.ToInt32(dr["IsAgentFee"]) > 0;
                IsPackagFee.Checked = ConvertType.ToInt32(dr["IsPackagFee"]) > 0;
                IsOtherFee.Checked = ConvertType.ToInt32(dr["IsOtherFee"]) > 0;
                IsHandleFee.Checked = ConvertType.ToInt32(dr["IsHandleFee"]) > 0;

                IsStorageFee.Checked = ConvertType.ToInt32(dr["IsStorageFee"]) > 0;
                IsWarehouseFee.Checked = ConvertType.ToInt32(dr["IsWarehouseFee"]) > 0;
                IsForkliftFee.Checked = ConvertType.ToInt32(dr["IsForkliftFee"]) > 0;
                IsChangeFee.Checked = ConvertType.ToInt32(dr["IsChangeFee"]) > 0;
                IsUpstairFee.Checked = ConvertType.ToInt32(dr["IsUpstairFee"]) > 0;
                IsCustomsFee.Checked = ConvertType.ToInt32(dr["IsCustomsFee"]) > 0;
                IsFrameFee.Checked = ConvertType.ToInt32(dr["IsFrameFee"]) > 0;
                IsMatPay.Checked = ConvertType.ToInt32(dr["IsMatPay"]) > 0;//HJ20180911
                IsRebates.Checked = ConvertType.ToInt32(dr["IsRebates"]) > 0;//HJ20180911

                gridView1.SetRowCellValue(0, "OperationWeight", ConvertType.ToDecimal(dr["OperationWeight"]));
                gridView1.SetRowCellValue(0, "OptWeight", ConvertType.ToDecimal(dr["OptWeight"]));

                IsCenterBack.Text = dr["IsCenterBack"].ToString();



                #region 吴沐鸿需求 2016-10-27
                TransitMode.Properties.Items.Clear();
                TransitMode.Text = "";
                TransitMode.Properties.Items.Add("中强快线");
                TransitMode.Properties.Items.Add("中强专线");
                TransitMode.Properties.Items.Add("中强整车");

                ConsignorCompany.EditValue = dr["BegWeb"]; //发货单位、发货人 默认为 下单部门名称
                ConsignorName.EditValue = dr["BegWeb"];
                VehicleNum.Text = ""; //接货方式  清空
                //ReceivAddress.EditValue = dr["ReceivAddress"];
                CusOderNo.EditValue = dr["CusOderNo"]; //客户单号 默认为 项目系统运单号

                gridView2.SetRowCellValue(0, "Freight", 0); //清空费用信息
                gridView2.SetRowCellValue(1, "Freight", 0);

                gridView1.SetRowCellValue(0, "DeliFee", 0);
                gridView1.SetRowCellValue(0, "SupportValue", 0);
                gridView1.SetRowCellValue(0, "Tax", 0);
                gridView1.SetRowCellValue(0, "InformationFee", 0);
                gridView1.SetRowCellValue(0, "Expense", 0);
                gridView1.SetRowCellValue(0, "DiscountTransfer", 0);
                gridView1.SetRowCellValue(0, "FuelFee", 0);
                gridView1.SetRowCellValue(0, "UpstairFee", 0);
                gridView1.SetRowCellValue(0, "HandleFee", 0);
                gridView1.SetRowCellValue(0, "ForkliftFee", 0);
                gridView1.SetRowCellValue(0, "StorageFee", 0);
                gridView1.SetRowCellValue(0, "CustomsFee", 0);
                gridView1.SetRowCellValue(0, "packagFee", 0);
                gridView1.SetRowCellValue(0, "FrameFee", 0);
                gridView1.SetRowCellValue(0, "ChangeFee", 0);
                gridView1.SetRowCellValue(0, "OtherFee", 0);
                gridView1.SetRowCellValue(0, "NoticeFee", 0);
                gridView1.SetRowCellValue(0, "ReceiptFee", 0);
                gridView1.SetRowCellValue(0, "ReceivFee", 0);
                gridView1.SetRowCellValue(0, "WarehouseFee", 0);
                gridView1.SetRowCellValue(0, "DeliveryFee", 0);
                gridView1.SetRowCellValue(0, "MatPay", 0);//HJ20180911

                PaymentAmout.EditValue = "";
                NowPay.EditValue = "";
                FetchPay.EditValue = "";
                MonthPay.EditValue = "";
                OwePay.EditValue = "";
                BefArrivalPay.EditValue = "";
                #endregion

                //if (ds.Tables[1].Rows.Count > 0)
                //{
                //    int NumFirst = ConvertType.ToInt32(dr["Num"]);
                //    decimal FeeWeightFirst = ConvertType.ToDecimal(dr["FeeWeight"]);
                //    decimal FeeVolumeFirst = ConvertType.ToDecimal(dr["FeeVolume"]);
                //    decimal WeightFirst = ConvertType.ToDecimal(dr["Weight"]);
                //    decimal VolumeFirst = ConvertType.ToDecimal(dr["Volume"]);
                //    decimal FreightFirst = ConvertType.ToDecimal(dr["Freight"]);

                //    int NumSecond = 0;
                //    decimal FeeWeightSecond = 0, FeeVolumeSecond = 0, WeightSecond = 0, VolumeSecond = 0, FreightSecond = 0;

                //    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                //    {
                //        gridView2.SetRowCellValue(i + 1, "Varieties", ds.Tables[1].Rows[i]["Varieties"].ToString());
                //        gridView2.SetRowCellValue(i + 1, "Package", ds.Tables[1].Rows[i]["Package"].ToString());
                //        gridView2.SetRowCellValue(i + 1, "Num", ds.Tables[1].Rows[i]["Num"].ToString());
                //        gridView2.SetRowCellValue(i + 1, "FeeWeight", ds.Tables[1].Rows[i]["FeeWeight"].ToString());
                //        gridView2.SetRowCellValue(i + 1, "FeeVolume", ds.Tables[1].Rows[i]["FeeVolume"].ToString());

                //        gridView2.SetRowCellValue(i + 1, "Weight", ds.Tables[1].Rows[i]["Weight"].ToString());
                //        gridView2.SetRowCellValue(i + 1, "Volume", ds.Tables[1].Rows[i]["Volume"].ToString());
                //        gridView2.SetRowCellValue(i + 1, "WeightPrice", ds.Tables[1].Rows[i]["WeightPrice"].ToString());
                //        gridView2.SetRowCellValue(i + 1, "VolumePrice", ds.Tables[1].Rows[i]["VolumePrice"].ToString());
                //        gridView2.SetRowCellValue(i + 1, "FeeType", ds.Tables[1].Rows[i]["FeeType"].ToString());
                //        gridView2.SetRowCellValue(i + 1, "Freight", ds.Tables[1].Rows[i]["Freight"].ToString());


                //        NumSecond += ConvertType.ToInt32(ds.Tables[1].Rows[i]["Num"]);
                //        FeeWeightSecond += ConvertType.ToDecimal(ds.Tables[1].Rows[i]["FeeWeight"]);
                //        FeeVolumeSecond += ConvertType.ToDecimal(ds.Tables[1].Rows[i]["FeeVolume"]);
                //        WeightSecond += ConvertType.ToDecimal(ds.Tables[1].Rows[i]["Weight"]);
                //        VolumeSecond += ConvertType.ToDecimal(ds.Tables[1].Rows[i]["Volume"]);
                //        FreightSecond += ConvertType.ToDecimal(ds.Tables[1].Rows[i]["Freight"]);
                //    }

                //    gridView2.SetRowCellValue(0, "Num", NumFirst - NumSecond);
                //    gridView2.SetRowCellValue(0, "FeeWeight", FeeWeightFirst - FeeWeightSecond);
                //    gridView2.SetRowCellValue(0, "FeeVolume", FeeVolumeFirst - FeeVolumeSecond);
                //    gridView2.SetRowCellValue(0, "Weight", WeightFirst - WeightSecond);
                //    gridView2.SetRowCellValue(0, "Volume", VolumeFirst - VolumeSecond);
                //    gridView2.SetRowCellValue(0, "Freight", FreightFirst - FreightSecond);
                //}
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                ConsignorCompany.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsignorCompany_EditValueChanging);
                ConsignorName.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsignorCompany_EditValueChanging);

                ConsigneeCompany.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsigneeCompany_EditValueChanging);
                ConsigneeName.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsigneeCompany_EditValueChanging);

                gridView1.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView2_CellValueChanged);
                gridView2.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView2_CellValueChanged);
            }
        }

        //获取当天的派车单号
        private void QSP_GET_DELIVERY_BILL()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_DELIVERY_BILL", list);
                gcjiehuodanhao.DataSource = SqlHelper.GetDataTable(sps);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void GetWayBill()
        {
            try
            {
                BillNo.Enabled = false;
                //ReceivProvince.TextChanged -= new EventHandler(ReceivProvince_TextChanged);
                //ReceivProvince.EditValueChanged -= new EventHandler(ReceivProvince_EditValueChanged);

                //ReceivCity.EditValueChanged -= new EventHandler(this.ReceivCity_EditValueChanged);
                //ReceivCity.TextChanged -= new EventHandler(this.ReceivProvince_TextChanged);
                //ReceivArea.EditValueChanged -= new EventHandler(this.ReceivArea_EditValueChanged);
                //ReceivArea.TextChanged -= new EventHandler(this.ReceivProvince_TextChanged);
                //ReceivStreet.EditValueChanged -= new EventHandler(this.ReceivStreet_EditValueChanged);
                //ReceivStreet.TextChanged -= new EventHandler(this.ReceivProvince_TextChanged);
                //ReceivAddress.EditValueChanged -= new EventHandler(this.ReceivAddress_EditValueChanged);
                //int num = CommonClass.GetAduitState(BillNO);
                //if (num > 0)
                //{
                //    MsgBox.ShowOK("运单已经审核或账款确认，不能执行！");
                //    return;
                //}
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNO", BillNO));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_ByID", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    MsgBox.ShowOK("查无此单，请检查!");
                    return;
                }
                dsModified = ds.Copy();
                isModify = 1;
                dr = ds.Tables[0].Rows[0];

                ConsignorCompany.EditValueChanging -= new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsignorCompany_EditValueChanging);
                ConsignorName.EditValueChanging -= new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsignorCompany_EditValueChanging);

                ConsigneeCompany.EditValueChanging -= new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsigneeCompany_EditValueChanging);
                ConsigneeName.EditValueChanging -= new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsigneeCompany_EditValueChanging);

                //gridView1.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView2_CellValueChanged);
                gridView2.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView2_CellValueChanged);
                TransitMode.EditValueChanged -= new EventHandler(TransitMode_EditValueChanged);

                ConsigneeCellPhone.EditValue = ConvertType.ToInt32(dr["NoticeState"]) == 0 ? dr["ConsigneeCellPhone"] : dr["ConsigneeCellPhone_K"];
                ConsigneePhone.EditValue = ConvertType.ToInt32(dr["NoticeState"]) == 0 ? dr["ConsigneePhone"] : dr["ConsigneePhone_K"];
                if (CommonClass.UserInfo.companyid == "239")
                {
                    Address.EditValue = ConvertType.ToInt32(dr["NoticeState"]) == 0 ? dr["ConsigneePhone"] : dr["ConsigneePhone_K"];
                }
                ConsigneeName.EditValue = ConvertType.ToInt32(dr["NoticeState"]) == 0 ? dr["ConsigneeName"] : dr["ConsigneeName_K"];
                ConsigneeCompany.EditValue = ConvertType.ToInt32(dr["NoticeState"]) == 0 ? dr["ConsigneeCompany"] : dr["ConsigneeCompany_K"];
                //hj20181018 
                ReceivProvince1 = dr["ReceivProvince"].ToString();
                ReceivCity1 = dr["ReceivCity"].ToString();
                ReceivArea1 = dr["ReceivArea"].ToString();
                ReceivStreet1 = dr["ReceivStreet"].ToString();
                //ReceivProvince.EditValue = dr["ReceivProvince"];
                billDate = dr["BillDate"].ToString();
                //ReceivCity.EditValue = dr["ReceivCity"];
                //ReceivArea.EditValue = dr["ReceivArea"];
                ReceivStreet.EditValue = dr["ReceivStreet"];
                Province.EditValue = dr["ReceivProvince"];
                City.EditValue = dr["ReceivCity"];
                Address.EditValue = dr["ReceivAddress"];
                ConsignorCellPhone.EditValue = dr["ConsignorCellPhone"];
                ConsignorPhone.EditValue = dr["ConsignorPhone"];
                ConsignorName.EditValue = dr["ConsignorName"];
                ConsignorCompany.EditValue = dr["ConsignorCompany"];
                //ReceivOrderNo.EditValue = dr["ReceivMode"];


                ValuationType.EditValue = dr["ValuationType"];
                ReceivOrderNo.EditValue = dr["ReceivOrderNo"];
                //yzw
                VehicleNum.EditValue = dr["VehicleNum"];
                DriverName.EditValue = dr["DriverName1"];
                Salesman.EditValue = dr["Salesman"];

                BillNo.EditValue = dr["BillNo"];
                VehicleNo.EditValue = dr["VehicleNo"];
                StartSite.EditValue = dr["StartSite"];
                TransferMode.EditValue = dr["TransferMode"];
                DestinationSite.EditValue = dr["DestinationSite"];
                TransferSite.EditValue = dr["TransferSite"];

                CusOderNo.EditValue = dr["CusOderNo"];

                gridView2.SetRowCellValue(0, "GoodsType", dr["GoodsType"]);
                gridView2.SetRowCellValue(0, "Varieties", dr["Varieties"]);
                gridView2.SetRowCellValue(0, "Package", dr["Package"]);
                gridView2.SetRowCellValue(0, "Num", dr["Num"]);

                gridView2.SetRowCellValue(0, "FeeWeight", dr["FeeWeight"]);
                gridView2.SetRowCellValue(0, "FeeVolume", dr["FeeVolume"]);
                gridView2.SetRowCellValue(0, "Weight", dr["Weight"]);
                gridView2.SetRowCellValue(0, "Volume", dr["Volume"]);
                gridView2.SetRowCellValue(0, "WeightPrice", dr["WeightPrice"]);

                gridView2.SetRowCellValue(0, "VolumePrice", dr["VolumePrice"]);
                gridView2.SetRowCellValue(0, "FeeType", dr["FeeType"]);
                gridView2.SetRowCellValue(0, "Freight", ConvertType.ToDecimal(dr["Freight"]));

                gridView1.SetRowCellValue(0, "DeliFee", ConvertType.ToDecimal(dr["DeliFee"]));
                gridView1.SetRowCellValue(0, "ReceivFee", ConvertType.ToDecimal(dr["ReceivFee"]));
                gridView1.SetRowCellValue(0, "DeclareValue", ConvertType.ToDecimal(dr["DeclareValue"]));
                gridView1.SetRowCellValue(0, "SupportValue", ConvertType.ToDecimal(dr["SupportValue"]));
                gridView1.SetRowCellValue(0, "Rate", ConvertType.ToDecimal(dr["Rate"]));

                gridView1.SetRowCellValue(0, "Tax", ConvertType.ToDecimal(dr["Tax"]));
                gridView1.SetRowCellValue(0, "InformationFee", ConvertType.ToDecimal(dr["InformationFee"]));
                gridView1.SetRowCellValue(0, "Expense", ConvertType.ToDecimal(dr["Expense"]));
                gridView1.SetRowCellValue(0, "NoticeFee", ConvertType.ToDecimal(dr["NoticeFee"]));
                gridView1.SetRowCellValue(0, "DiscountTransfer", ConvertType.ToDecimal(dr["DiscountTransfer"]));

                gridView1.SetRowCellValue(0, "CollectionPay", ConvertType.ToDecimal(dr["CollectionPay"]));
                gridView1.SetRowCellValue(0, "AgentFee", ConvertType.ToDecimal(dr["AgentFee"]));
                gridView1.SetRowCellValue(0, "FuelFee", ConvertType.ToDecimal(dr["FuelFee"]));

                gridView1.SetRowCellValue(0, "UpstairFee", ConvertType.ToDecimal(dr["UpstairFee"]));
                gridView1.SetRowCellValue(0, "HandleFee", ConvertType.ToDecimal(dr["HandleFee"]));
                gridView1.SetRowCellValue(0, "ForkliftFee", ConvertType.ToDecimal(dr["ForkliftFee"]));
                gridView1.SetRowCellValue(0, "StorageFee", ConvertType.ToDecimal(dr["StorageFee"]));
                gridView1.SetRowCellValue(0, "CustomsFee", ConvertType.ToDecimal(dr["CustomsFee"]));

                gridView1.SetRowCellValue(0, "packagFee", ConvertType.ToDecimal(dr["packagFee"]));
                gridView1.SetRowCellValue(0, "FrameFee", ConvertType.ToDecimal(dr["FrameFee"]));
                gridView1.SetRowCellValue(0, "ChangeFee", ConvertType.ToDecimal(dr["ChangeFee"]));
                gridView1.SetRowCellValue(0, "OtherFee", ConvertType.ToDecimal(dr["OtherFee"]));
                gridView1.SetRowCellValue(0, "ReceiptFee", ConvertType.ToDecimal(dr["ReceiptFee"]));

                gridView1.SetRowCellValue(0, "FreightAmount", ConvertType.ToDecimal(dr["FreightAmount"]));
                gridView1.SetRowCellValue(0, "ActualFreight", ConvertType.ToDecimal(dr["ActualFreight"]));
                gridView1.SetRowCellValue(0, "WarehouseFee", ConvertType.ToDecimal(dr["WarehouseFee"]));
                gridView1.SetRowCellValue(0, "MatPay", ConvertType.ToDecimal(dr["MatPay"]));//HJ20180911

                ReceiptCondition.EditValue = dr["ReceiptCondition"];
                IsInvoice.Checked = Convert.ToInt32(dr["IsInvoice"].ToString() == "" ? "0" : dr["IsInvoice"].ToString()) > 0;


                PaymentMode.EditValue = dr["PaymentMode"];
                PaymentAmout.EditValue = dr["PaymentAmout"];

                NowPay.EditValue = dr["NowPay"];
                FetchPay.EditValue = dr["FetchPay"];
                MonthPay.EditValue = dr["MonthPay"];
                //ZAJ 
                ReceiptPay.EditValue = dr["ReceiptPay"];
                ShortOwePay.EditValue = dr["ShortOwePay"];
                OwePay.EditValue = dr["OwePay"];
                BefArrivalPay.EditValue = dr["BefArrivalPay"];
                BillRemark.EditValue = dr["BillRemark"];
                WebPlatform.EditValue = dr["WebPlatform"];
                WebBillNo.EditValue = dr["WebBillNo"];
                DisTranName.EditValue = dr["DisTranName"];
                DisTranBank.EditValue = dr["DisTranBank"];
                DisTranAccount.EditValue = dr["DisTranAccount"];
                CollectionName.EditValue = dr["CollectionName"];
                CollectionBank.EditValue = dr["CollectionBank"];
                CollectionAccount.EditValue = dr["CollectionAccount"];
                CauseName.EditValue = dr["CauseName"];
                AreaName.EditValue = dr["AreaName"];
                DepName.EditValue = dr["DepName"];

                DisTranBranch.EditValue = dr["DisTranBranch"];
                CollectionBranch.EditValue = dr["CollectionBranch"];
                BillMan.EditValue = dr["BillMan"];
                begWeb.EditValue = dr["begWeb"];
                PickGoodsSite.EditValue = dr["PickGoodsSite"];
                //ReceivAddress.EditValue = dr["ReceivAddress"];



                string sg = dr["DeliveryFee"].ToString();
                //gridView8.SetRowCellValue(0, "MainlineFee", dr["MainlineFee"]);
                //gridView1.SetRowCellValue(0, "DeliveryFee", ConvertType.ToDecimal(dr["DeliveryFee"]));
                //gridView8.SetRowCellValue(0, "TransferFee", dr["TransferFee"]);
                //gridView8.SetRowCellValue(0, "DepartureOptFee", dr["DepartureOptFee"]);
                //gridView8.SetRowCellValue(0, "TerminalOptFee", dr["TerminalOptFee"]);
                //gridView8.SetRowCellValue(0, "TerminalAllotFee", dr["TerminalAllotFee"]);

                //gridView8.SetRowCellValue(0, "ReceiptFee_C", dr["ReceiptFee_C"]);
                //gridView8.SetRowCellValue(0, "NoticeFee_C", dr["NoticeFee_C"]);
                //gridView8.SetRowCellValue(0, "SupportValue_C", dr["SupportValue_C"]);

                //gridView8.SetRowCellValue(0, "AgentFee_C", dr["AgentFee_C"]);
                //gridView8.SetRowCellValue(0, "PackagFee_C", dr["PackagFee_C"]);
                //gridView8.SetRowCellValue(0, "OtherFee_C", dr["OtherFee_C"]);

                //gridView8.SetRowCellValue(0, "HandleFee_C", dr["HandleFee_C"]);
                //gridView8.SetRowCellValue(0, "StorageFee_C", dr["StorageFee_C"]);
                //gridView8.SetRowCellValue(0, "WarehouseFee_C", dr["WarehouseFee_C"]);

                //gridView8.SetRowCellValue(0, "ForkliftFee_C", dr["ForkliftFee_C"]);
                //gridView8.SetRowCellValue(0, "Tax_C", dr["Tax_C"]);
                //gridView8.SetRowCellValue(0, "ChangeFee_C", dr["ChangeFee_C"]);

                //gridView8.SetRowCellValue(0, "UpstairFee_C", dr["UpstairFee_C"]);
                //gridView8.SetRowCellValue(0, "CustomsFee_C", dr["CustomsFee_C"]);
                //gridView8.SetRowCellValue(0, "FrameFee_C", dr["FrameFee_C"]);

                //gridView8.SetRowCellValue(0, "Expense_C", dr["Expense_C"]);
                //gridView8.SetRowCellValue(0, "FuelFee_C", dr["FuelFee_C"]);
                //gridView8.SetRowCellValue(0, "InformationFee_C", dr["InformationFee_C"]);

                MainlineFee.Text = dr["MainlineFee"].ToString();
                DeliveryFee.Text = dr["DeliveryFee"].ToString();
                TransferFee.Text = dr["TransferFee"].ToString();
                DepartureOptFee.Text = dr["DepartureOptFee"].ToString();
                TerminalOptFee.Text = dr["TerminalOptFee"].ToString();
                TerminalAllotFee.Text = dr["TerminalAllotFee"].ToString();
                ReceiptFee_C.Text = dr["ReceiptFee_C"].ToString();
                NoticeFee_C.Text = dr["NoticeFee_C"].ToString();
                SupportValue_C.Text = dr["SupportValue_C"].ToString();
                AgentFee_C.Text = dr["AgentFee_C"].ToString();
                PackagFee_C.Text = dr["PackagFee_C"].ToString();
                OtherFee_C.Text = dr["OtherFee_C"].ToString();
                HandleFee_C.Text = dr["HandleFee_C"].ToString();
                StorageFee_C.Text = dr["StorageFee_C"].ToString();
                WarehouseFee_C.Text = dr["WarehouseFee_C"].ToString();
                ForkliftFee_C.Text = dr["ForkliftFee_C"].ToString();
                Tax_C.Text = dr["Tax_C"].ToString();
                ChangeFee_C.Text = dr["ChangeFee_C"].ToString();
                UpstairFee_C.Text = dr["UpstairFee_C"].ToString();
                CustomsFee_C.Text = dr["CustomsFee_C"].ToString();
                FrameFee_C.Text = dr["FrameFee_C"].ToString();
                Expense_C.Text = dr["Expense_C"].ToString();
                FuelFee_C.Text = dr["FuelFee_C"].ToString();
                InformationFee_C.Text = dr["InformationFee_C"].ToString();


                AlienGoods.Checked = Convert.ToInt32(dr["AlienGoods"].ToString() == "" ? "0" : dr["AlienGoods"].ToString()) > 0;
                GoodsVoucher.Checked = Convert.ToInt32(dr["GoodsVoucher"].ToString() == "" ? "0" : dr["GoodsVoucher"].ToString()) > 0;
                PreciousGoods.Checked = Convert.ToInt32(dr["PreciousGoods"].ToString() == "" ? "0" : dr["PreciousGoods"].ToString()) > 0;
                NoticeState.Checked = Convert.ToInt32(dr["NoticeState"].ToString() == "" ? "0" : dr["NoticeState"].ToString()) > 0;

                IsReceiptFee.Checked = ConvertType.ToInt32(dr["IsReceiptFee"]) > 0;
                IsSupportValue.Checked = ConvertType.ToInt32(dr["IsSupportValue"]) > 0;
                IsAgentFee.Checked = ConvertType.ToInt32(dr["IsAgentFee"]) > 0;
                IsPackagFee.Checked = ConvertType.ToInt32(dr["IsPackagFee"]) > 0;
                IsOtherFee.Checked = ConvertType.ToInt32(dr["IsOtherFee"]) > 0;
                IsHandleFee.Checked = ConvertType.ToInt32(dr["IsHandleFee"]) > 0;

                IsStorageFee.Checked = ConvertType.ToInt32(dr["IsStorageFee"]) > 0;
                IsWarehouseFee.Checked = ConvertType.ToInt32(dr["IsWarehouseFee"]) > 0;
                IsForkliftFee.Checked = ConvertType.ToInt32(dr["IsForkliftFee"]) > 0;
                IsChangeFee.Checked = ConvertType.ToInt32(dr["IsChangeFee"]) > 0;
                IsUpstairFee.Checked = ConvertType.ToInt32(dr["IsUpstairFee"]) > 0;
                IsCustomsFee.Checked = ConvertType.ToInt32(dr["IsCustomsFee"]) > 0;
                IsFrameFee.Checked = ConvertType.ToInt32(dr["IsFrameFee"]) > 0;
                IsMatPay.Checked = ConvertType.ToInt32(dr["IsMatPay"]) > 0;//HJ20180911
                IsRebates.Checked = ConvertType.ToInt32(dr["IsRebates"]) > 0;//HJ20180911回扣已返
                gridView1.SetRowCellValue(0, "OperationWeight", ConvertType.ToDecimal(dr["OperationWeight"]));
                gridView1.SetRowCellValue(0, "OptWeight", ConvertType.ToDecimal(dr["OptWeight"]));

                IsCenterBack.Text = dr["IsCenterBack"].ToString();
                //luohui
                IsViewTraceFee.Checked = Convert.ToInt32(dr["IsViewTrace"].ToString() == "" ? "0" : dr["IsViewTrace"].ToString()) > 0;
                IsSortFee.Checked = Convert.ToInt32(dr["IsSortFee"].ToString() == "" ? "0" : dr["IsSortFee"].ToString()) > 0;


                if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    int NumFirst = ConvertType.ToInt32(dr["Num"]);
                    decimal FeeWeightFirst = ConvertType.ToDecimal(dr["FeeWeight"]);
                    decimal FeeVolumeFirst = ConvertType.ToDecimal(dr["FeeVolume"]);
                    decimal WeightFirst = ConvertType.ToDecimal(dr["Weight"]);
                    decimal VolumeFirst = ConvertType.ToDecimal(dr["Volume"]);
                    decimal FreightFirst = ConvertType.ToDecimal(dr["Freight"]);

                    int NumSecond = 0;
                    decimal FeeWeightSecond = 0, FeeVolumeSecond = 0, WeightSecond = 0, VolumeSecond = 0, FreightSecond = 0;

                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        gridView2.SetRowCellValue(i + 1, "Varieties", ds.Tables[1].Rows[i]["Varieties"].ToString());
                        gridView2.SetRowCellValue(i + 1, "Package", ds.Tables[1].Rows[i]["Package"].ToString());
                        gridView2.SetRowCellValue(i + 1, "Num", ds.Tables[1].Rows[i]["Num"].ToString());
                        gridView2.SetRowCellValue(i + 1, "FeeWeight", ds.Tables[1].Rows[i]["FeeWeight"].ToString());
                        gridView2.SetRowCellValue(i + 1, "FeeVolume", ds.Tables[1].Rows[i]["FeeVolume"].ToString());

                        gridView2.SetRowCellValue(i + 1, "Weight", ds.Tables[1].Rows[i]["Weight"].ToString());
                        gridView2.SetRowCellValue(i + 1, "Volume", ds.Tables[1].Rows[i]["Volume"].ToString());
                        gridView2.SetRowCellValue(i + 1, "WeightPrice", ds.Tables[1].Rows[i]["WeightPrice"].ToString());
                        gridView2.SetRowCellValue(i + 1, "VolumePrice", ds.Tables[1].Rows[i]["VolumePrice"].ToString());
                        gridView2.SetRowCellValue(i + 1, "FeeType", ds.Tables[1].Rows[i]["FeeType"].ToString());
                        gridView2.SetRowCellValue(i + 1, "Freight", ds.Tables[1].Rows[i]["Freight"].ToString());

                        NumSecond += ConvertType.ToInt32(ds.Tables[1].Rows[i]["Num"]);
                        FeeWeightSecond += ConvertType.ToDecimal(ds.Tables[1].Rows[i]["FeeWeight"]);
                        FeeVolumeSecond += ConvertType.ToDecimal(ds.Tables[1].Rows[i]["FeeVolume"]);
                        WeightSecond += ConvertType.ToDecimal(ds.Tables[1].Rows[i]["Weight"]);
                        VolumeSecond += ConvertType.ToDecimal(ds.Tables[1].Rows[i]["Volume"]);
                        FreightSecond += ConvertType.ToDecimal(ds.Tables[1].Rows[i]["Freight"]);
                    }

                    gridView2.SetRowCellValue(0, "Num", NumFirst - NumSecond);
                    gridView2.SetRowCellValue(0, "FeeWeight", FeeWeightFirst - FeeWeightSecond);
                    gridView2.SetRowCellValue(0, "FeeVolume", FeeVolumeFirst - FeeVolumeSecond);
                    gridView2.SetRowCellValue(0, "Weight", WeightFirst - WeightSecond);
                    gridView2.SetRowCellValue(0, "Volume", VolumeFirst - VolumeSecond);
                    gridView2.SetRowCellValue(0, "Freight", FreightFirst - FreightSecond);
                }
                TransitMode.EditValue = dr["TransitMode"];

                //获取尺寸hj20180926
                string billno = BillNo.Text;
                string Varieties1 = gridView2.GetRowCellValue(0, "Varieties").ToString();
                string Package1 = gridView2.GetRowCellValue(0, "Package").ToString();
                string Varieties2 = gridView2.GetRowCellValue(1, "Varieties").ToString();
                string Package2 = gridView2.GetRowCellValue(1, "Package").ToString();


                List<SqlPara> list2 = new List<SqlPara>();
                //if (Varieties2 != "" || Varieties2 != null)
                //{
                //    list2.Add(new SqlPara("Varieties2", Varieties2));
                //    list2.Add(new SqlPara("Package2", Package2));
                //}
                list2.Add(new SqlPara("BillNo", billno));
                //list2.Add(new SqlPara("Varieties1", Varieties1));
                //list2.Add(new SqlPara("Package1", Package1));
                SqlParasEntity sps2 = new SqlParasEntity(OperType.Query, "USP_GET_SpecialGoodsSize", list2);
                DataSet ds2 = SqlHelper.GetDataSet(sps2);
                if (ds2.Tables[0].Rows.Count == 1)
                {
                    string variet = ds2.Tables[0].Rows[0]["Varieties"].ToString();
                    string pack = ds2.Tables[0].Rows[0]["Package"].ToString();
                    decimal len = Convert.ToDecimal(ds2.Tables[0].Rows[0]["Length"]);
                    decimal wid = Convert.ToDecimal(ds2.Tables[0].Rows[0]["Width"]);
                    decimal hei = Convert.ToDecimal(ds2.Tables[0].Rows[0]["Height"]);
                    if (Varieties1 == variet && Package1 == pack)
                    {
                        gridView2.SetRowCellValue(0, "length", len);
                        gridView2.SetRowCellValue(0, "width", wid);
                        gridView2.SetRowCellValue(0, "height", hei);

                        //第二行为0设空
                        gridView2.SetRowCellValue(1, "length", 0);
                        gridView2.SetRowCellValue(1, "width", 0);
                        gridView2.SetRowCellValue(1, "height", 0);
                    }
                    else
                    {
                        gridView2.SetRowCellValue(1, "length", len);
                        gridView2.SetRowCellValue(1, "width", wid);
                        gridView2.SetRowCellValue(1, "height", hei);
                    }
                }
                else if (ds2.Tables[0].Rows.Count == 2)
                {
                    string variet = ds2.Tables[0].Rows[0]["Varieties"].ToString();
                    string pack = ds2.Tables[0].Rows[0]["Package"].ToString();
                    decimal len = Convert.ToDecimal(ds2.Tables[0].Rows[0]["Length"]);
                    decimal wid = Convert.ToDecimal(ds2.Tables[0].Rows[0]["Width"]);
                    decimal hei = Convert.ToDecimal(ds2.Tables[0].Rows[0]["Height"]);
                    decimal len2 = Convert.ToDecimal(ds2.Tables[0].Rows[1]["Length"]);
                    decimal wid2 = Convert.ToDecimal(ds2.Tables[0].Rows[1]["Width"]);
                    decimal hei2 = Convert.ToDecimal(ds2.Tables[0].Rows[1]["Height"]);
                    if (Varieties1 == variet && Package1 == pack)
                    {
                        gridView2.SetRowCellValue(0, "length", len);
                        gridView2.SetRowCellValue(0, "width", wid);
                        gridView2.SetRowCellValue(0, "height", hei);
                        gridView2.SetRowCellValue(1, "length", len2);
                        gridView2.SetRowCellValue(1, "width", wid2);
                        gridView2.SetRowCellValue(1, "height", hei2);
                    }
                    else
                    {
                        gridView2.SetRowCellValue(0, "length", len2);
                        gridView2.SetRowCellValue(0, "width", wid2);
                        gridView2.SetRowCellValue(0, "height", hei2);
                        gridView2.SetRowCellValue(1, "length", len);
                        gridView2.SetRowCellValue(1, "width", wid);
                        gridView2.SetRowCellValue(1, "height", hei);
                    }

                }
                //luohui
                MarketSuper.Checked = ConvertType.ToInt32(dr["MarketSuper"]) == 1;
              

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                ConsignorCompany.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsignorCompany_EditValueChanging);
                ConsignorName.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsignorCompany_EditValueChanging);

                ConsigneeCompany.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsigneeCompany_EditValueChanging);
                ConsigneeName.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsigneeCompany_EditValueChanging);

                //gridView1.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView2_CellValueChanged);
                gridView2.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView2_CellValueChanged);
                TransitMode.EditValueChanged += new EventHandler(TransitMode_EditValueChanged);

                //ReceivProvince.TextChanged += new EventHandler(ReceivProvince_TextChanged);
                //ReceivProvince.EditValueChanged += new EventHandler(ReceivProvince_EditValueChanged);
                //ReceivCity.EditValueChanged += new EventHandler(this.ReceivCity_EditValueChanged);
                //ReceivCity.TextChanged += new EventHandler(this.ReceivProvince_TextChanged);
                //ReceivArea.EditValueChanged += new EventHandler(this.ReceivArea_EditValueChanged);
                //ReceivArea.TextChanged += new EventHandler(this.ReceivProvince_TextChanged);
                //ReceivStreet.EditValueChanged += new EventHandler(this.ReceivStreet_EditValueChanged);
                //ReceivStreet.TextChanged += new EventHandler(this.ReceivProvince_TextChanged);
                // ReceivAddress.EditValueChanged += new EventHandler(this.ReceivAddress_EditValueChanged);
            }
            //zaj 20180620 改单不允许修改保险信息
            if ((Upd_Num == 1 || isModify == 1) && (CommonClass.UserInfo.companyid == "105"))//|| CommonClass.UserInfo.companyid == "106"
            {
                IsSupportValue.Enabled = false;

                gridView1.Columns["DeclareValue"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["DeclareValue"].OptionsColumn.AllowFocus = false;

                gridView1.Columns["SupportValue"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["SupportValue"].OptionsColumn.AllowFocus = false;
                IsViewTraceFee.Enabled = false;//视图跟踪
            }
        }

        private void ConsigneeCellPhone_EditValueChanged(object sender, EventArgs e)
        {

        }
        private void gridView4_DoubleClick(object sender, EventArgs e)
        {
            setSite();
        }

        private void gcdaozhan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                setSite();
            }
        }

        private void setSite()
        {
            try
            {
                int rows = gridView4.FocusedRowHandle;
                if (rows < 0) return;
                TransferSite.Properties.Items.Clear();
                DataRow dr_ = myGridView1.GetDataRow(rows);
                //ReceivStreet.Text = dr_["MiddleStreet"].ToString();
                string SiteName = dr_["SiteName"].ToString();
                if (SiteName != "")
                {
                    //TransferSite.Text = dr_["SiteName"].ToString();
                    string[] SiteNames = SiteName.Split(',');
                    for (int i = 0; i < SiteNames.Length; i++)
                    {
                        TransferSite.Properties.Items.Add(SiteNames[i]);
                    }
                    //TransferSite.Text = SiteNames[0];
                }
                else
                {
                    TransferSite.Text = CommonClass.UserInfo.SiteName;
                    TransitMode.Text = "中强项目";
                }


                ReceivStreet2.Focus();
                gcdaozhan.Visible = false;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        #region zaj 2017-11-29
        //private void setSite1()
        //{
        //    DataTable MiddleSiteTable = CommonClass.dsMiddleSite.Tables[0];
        //    string sql = "MiddleProvince='" + ReceivProvince.Text.Trim() + "' and MiddleCity='" + ReceivCity.Text.Trim() + "' and MiddleArea='" + ReceivArea.Text.Trim() + "'";
        //    DataRow dr = MiddleSiteTable.Select(sql)[0];
        //    DataRow[] drs = MiddleSiteTable.Select(sql);
        //    bool flag = false;
        //    if (TransferSite.Text.Trim() != "")
        //    {
        //        for (int i = 0; i < drs.Length; i++)
        //        {
        //            if (TransferSite.Text.Trim() == drs[i]["SiteName"].ToString().Trim())
        //            {
        //                flag = true;
        //                break;
        //            }
        //        }
        //    }
        //    if (flag) return;

        //    TransferSite.Properties.Items.Clear();
        //    string SiteName = dr["SiteName"].ToString();
        //    if (SiteName != "")
        //    {
        //        //TransferSite.Text = dr_["SiteName"].ToString();
        //        string[] SiteNames = SiteName.Split(',');
        //        for (int i = 0; i < SiteNames.Length; i++)
        //        {
        //            TransferSite.Properties.Items.Add(SiteNames[i]);
        //        }
        //        TransferSite.Text = SiteNames[0];
        //    }
        //    else
        //    {
        //        TransferSite.Text = CommonClass.UserInfo.SiteName;
        //        TransitMode.Text = "中强项目";
        //    }
        //}
        #endregion

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveCheck(false);
        }
        //public int iPrescription { get; set; }
        //string strLatestDepartTime = string.Empty;
        string NewPickGoodsSite = "";
        decimal NewDeliveryFee = 0;
        private void SaveCheck(bool isFee)
        {

            NewPickGoodsSite = "";
            //NewDeliveryFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "DeliveryFee"));
            gridView1.PostEditor();
            gridView2.PostEditor();
            //回单付检测
            if (CommonClass.UserInfo.companyid != "484")   //zb20190603 484公司解除回单付限制
            {
                if (PaymentMode.Text == "回单付")
                {
                    try
                    {
                        List<SqlPara> list = new List<SqlPara>();
                        list.Add(new SqlPara("ConsignorCompany", ConsignorCompany.Text.Trim()));
                        list.Add(new SqlPara("ConsignorName", ConsignorName.Text.Trim()));
                        SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_CustContractCheck_HDF", list);
                        DataSet dsCust = SqlHelper.GetDataSet(sps);
                        if (dsCust == null || dsCust.Tables.Count == 0 || dsCust.Tables[0].Rows.Count == 0)
                        {
                            MsgBox.ShowOK("发货单位[" + ConsignorCompany.Text.Trim() + "]不是回单付客户，不能开回单付！");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        MsgBox.ShowException(ex);
                    }
                }
            }

            //客户单号检测
            if (Upd_Num == 0 && isModify == 0)
            {
                if (CusOderNo.Text != "")
                {
                    try
                    {
                        List<SqlPara> list = new List<SqlPara>();
                        list.Add(new SqlPara("CusOderNo", CusOderNo.Text.Trim()));
                        SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_CusOderNo_TX", list);
                        DataSet dsCust = SqlHelper.GetDataSet(sps);
                        int a = Convert.ToInt32(dsCust.Tables[0].Rows[0]["a"]);
                        if (a > 0)
                        {
                            MsgBox.ShowOK("该客户单号已使用！");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        MsgBox.ShowException(ex);
                    }
                }
            }

            ////交接方式检测
            //try
            //{
            //    List<SqlPara> list = new List<SqlPara>();
            //    list.Add(new SqlPara("Province", Province.Text.Trim()));
            //    list.Add(new SqlPara("City", City.Text.Trim()));
            //    list.Add(new SqlPara("ReceivStreet", ReceivStreet.Text.Trim()));
            //    list.Add(new SqlPara("TransferMode", TransferMode.Text.Trim()));
            //    SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_basMiddleSite_309_TXJC", list);
            //    DataSet ds = SqlHelper.GetDataSet(spe);
            //    if (ds.Tables[0].Rows.Count < 1)
            //    {
            //        if (MsgBox.ShowYesNo("当前目的网点不支持" + TransferMode.Text+"是否继续保存？") != DialogResult.Yes) return;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MsgBox.ShowException(ex);
            //}


            try
            {
                #region 基本信息检测
                //zaj 注释必需输入运单号判断 2017-11-10
                //if (BillNo.Text.Trim() == "")
                //{
                //    MsgBox.ShowOK("必须输入运单号");
                //    BillNo.Focus();
                //    return;
                //}
                //hj20181101如果付款方式为提付改欠时做判断
                if (ConsignorCompany.Text.Trim() == "")
                {
                    MsgBox.ShowOK("必须选择发货单位");
                    TransferMode.Focus();
                    return;
                }
                if (PaymentMode.Text.Trim() == "提付改欠")
                {
                    DataSet ds1 = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_COMPANY_KD"));
                    string gsControl = ds1.Tables[0].Rows[0]["gsControl"].ToString();
                    DataSet ds2 = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BASCUSTCONTRACT_KD", new List<SqlPara> { new SqlPara("ConsignorCompany", ConsigneeCompany.Text.Trim()) }));
                    if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                    {
                        string FetchToOwe = ds2.Tables[0].Rows[0]["FetchToOwe"].ToString();
                        if (FetchToOwe != "是" || gsControl != "是")
                        {
                            MsgBox.ShowOK("该收货单位未开通提付改欠！");
                            return;
                        }

                    }
                    else
                    {
                        MsgBox.ShowOK("该收货单位未开通提付改欠！");
                        return;
                    }
                }

                if (DestinationSite.Text.Trim() == "")
                {
                    MsgBox.ShowOK("必须输入到站");
                    DestinationSite.Focus();
                    return;
                }
                //zb20190528 整车,交接方式必须为司机直送
                if (TransitMode.Text.Trim().ToString().Contains("整车"))
                {
                    if (TransferMode.Text.Trim().ToString() != "司机直送")
                    {
                        MsgBox.ShowOK("整车的交接方式必须为司机直送！");
                        return;
                    }
                }

                //通过登陆的账号，判断是否必须填写街道地址 
                //ZB20190504
                if (!TransitMode.Text.Trim().Contains("项目") && !TransitMode.Text.Trim().Contains("整车"))
                {
                    DataSet ds3 = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_COMPANY_BY_ID", new List<SqlPara>()));
                    if (ds3.Tables[0].Rows.Count > 0 && ds3 != null)
                    {
                        int isStreet = Convert.ToInt32(ds3.Tables[0].Rows[0]["gsNotStreet"].ToString());
                        //0代表必须填写街道地址，1代表不需
                        if (isStreet == 0)
                        {
                            if (this.ReceivStreet.Text.Trim() == "") //ZB20190427
                            {
                                MsgBox.ShowOK("必须输入街道地址!");
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (this.ReceivStreet.Text.Trim() == "") //ZB20190427
                        {
                            MsgBox.ShowOK("必须输入街道地址!");
                            return;
                        }
                    }
                }

                if (TransferSite.Text.Trim() == "")
                {
                    MsgBox.ShowOK("中转地不能为空");
                    TransferSite.Focus();
                    return;
                }
                //hj20180706
                //if (ConsignorCompany.Text.Trim() == "")
                //{
                //    MsgBox.ShowOK("必须输入发货公司名称");
                //    ConsignorCompany.Focus();
                //    return;
                //}
                if (ConsignorName.Text.Trim() == "")
                {
                    MsgBox.ShowOK("必须输入发货联系人");
                    ConsignorName.Focus();
                    return;
                }
                if (ConsignorCellPhone.Text.Trim() == "" && ConsignorPhone.Text.Trim() == "")
                {
                    MsgBox.ShowOK("发货联系方式输入一项");
                    return;
                }
                //hj20180706
                //if (ConsigneeCompany.Text.Trim() == "")
                //{
                //    MsgBox.ShowOK("必须输入收货公司名称");
                //    ConsigneeCompany.Focus();
                //    return;
                //}
                if (ConsigneeName.Text.Trim() == "")
                {
                    MsgBox.ShowOK("必须输入收货联系人");
                    ConsignorName.Focus();
                    return;
                }
                if (ConsigneeCellPhone.Text.Trim() == "" && ConsigneePhone.Text.Trim() == "")
                {
                    MsgBox.ShowOK("收货联系方式输入一项");
                    return;
                }
                //zaj 注释目的网点必填判断 2017-11-10
                //if (PickGoodsSite.Text.Trim() == "" && TransitMode.Text.Trim() != "中强项目")
                //{
                //    MsgBox.ShowOK("必须选择目的网点");
                //    PickGoodsSite.Focus();
                //    return;
                //}

                if (ConvertType.ToDecimal(gridView2.GetRowCellValue(0, "Num")) <= 0)
                {
                    MsgBox.ShowOK("必须输入正确的件数");
                    PickGoodsSite.Focus();
                    return;
                }

                if (ConvertType.ToDecimal(gridView2.GetRowCellValue(0, "FeeWeight")) <= 0)
                {
                    MsgBox.ShowOK("必须输入正确的计费重量");
                    PickGoodsSite.Focus();
                    return;
                }

                if (ConvertType.ToDecimal(gridView2.GetRowCellValue(0, "FeeVolume")) <= 0)
                {
                    MsgBox.ShowOK("必须输入正确的计费体积");
                    PickGoodsSite.Focus();
                    return;
                }
                //zaj 2017-11-29
                //if (CommonClass.UserInfo.companyid == "167")
                //{
                //    if (ReceivProvince.Text.Trim() == "" || ReceivCity.Text.Trim() == "" || ReceivArea.Text.Trim() == "" || ReceivStreet.Text.Trim() == "")
                //    {
                //        MsgBox.ShowOK("必须选择省市区街道");
                //        ReceivProvince.Focus();
                //        return;
                //    }
                //}
                //else
                //{
                //    if (ReceivProvince.Text.Trim() == "" || ReceivCity.Text.Trim() == "" || ReceivArea.Text.Trim() == "")
                //    {
                //        MsgBox.ShowOK("必须选择省市区街道");
                //        ReceivProvince.Focus();
                //        return;
                //    }
                //}
                //if (ReceivProvince.Text.Trim() == "" || ReceivCity.Text.Trim() == "" || ReceivArea.Text.Trim() == "")
                //{
                //    MsgBox.ShowOK("必须选择省市区街道");
                //    ReceivProvince.Focus();
                //    return;
                //}

                //hj20180721 回单要求为空的不能开回单付
                //if ((ReceiptCondition.Text.Trim() == "" || ReceiptCondition.Text.Trim() == "附清单") && PaymentMode.Text.Trim() == "回单付")
                //{
                //    MsgBox.ShowOK("没有回单要求，不能开回单付！");
                //    ReceiptCondition.Focus();
                //    return;
                //}
                string siteNames = "";
                try
                {
                    //验证公司是否被授权
                    List<SqlPara> list = new List<SqlPara>();
                    //list.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));
                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_COMPANY_BY_ID", list);
                    DataSet ds = SqlHelper.GetDataSet(sps);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        siteNames = ds.Tables[0].Rows[0]["Sitenames"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
                //if (siteNames.Contains(TransferSite.Text.Trim()) && ReceivStreet.Text.Trim() == "")
                //{
                //    MsgBox.ShowOK("必须选择街道");
                //    ReceivStreet.Focus();
                //    return;
                //}

                //bool flag = true;
                //if (NotStreet == null)
                //{
                //    flag = true;
                //}
                //else
                //{
                //    flag = false;
                //}

                //if (TransferMode.Text.Contains("送") && ReceivStreet.Text.Trim() == ""&&flag)
                //{
                //    MsgBox.ShowOK("必须选择街道");
                //    ReceivStreet.Focus();
                //    return;
                //}
                //zaj 20180702限制106 119街道必填
                //if ((CommonClass.UserInfo.companyid == "106" || CommonClass.UserInfo.companyid == "119") && ReceivStreet.Text.Trim()=="")
                //{
                //    MsgBox.ShowOK("必须选择街道");
                //    ReceivStreet.Focus();
                //    return;
                //}

                if (TransferMode.Text.Trim() == "")
                {
                    MsgBox.ShowOK("必须选择交接方式");
                    TransferMode.Focus();
                    //ReceivMode.Focus();
                    return;
                }

                if (PreciousGoods.Checked && TransitMode.Text.Trim() != "中强快线")
                {
                    MsgBox.ShowOK("只有中强快线才能勾选必走货!");
                    return;
                }


                //if (ReceivMode.Text.Trim() == "")
                //{
                //    MsgBox.ShowOK("必须选择接货方式");
                //    //TransferMode.Focus();
                //    ReceivMode.Focus();
                //    return;
                //}
                //if (TransitMode.Text.Trim() == "")
                //{
                //    MsgBox.ShowOK("必须选择运输方式");
                //    TransitMode.Focus();
                //    return;
                //}
                //if (TransferSite.Text.Trim().Contains("香港") && TransferMode.Text.Trim() == "网点送货")
                //{
                //    MsgBox.ShowOK("香港不允许网点送货！");
                //    TransferMode.Focus();
                //    return;
                //}


                #endregion

                #region 检测加盟账户余额-开单结算费是否满足开单条件
                if (CommonClass.UserInfo.WebRole == "加盟")
                {
                    Decimal total_acc = 0;
                    try
                    {
                        List<SqlPara> list = new List<SqlPara>();
                        SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ACCOUNT_BIllSTATE", list);
                        DataSet ds = SqlHelper.GetDataSet(sps);
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                total_acc += Convert.ToDecimal(ds.Tables[0].Rows[i]["heji"].ToString());
                            }

                        }
                        if ((CommonClass.GetWebAcc(CommonClass.UserInfo.WebName) - Convert.ToDecimal(this.heji.Text) - total_acc) < 0)
                        {
                            MsgBox.ShowOK("账户余额不足，请充值!");
                            return;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        MsgBox.ShowException(ex);
                    }


                }

                #endregion




                #region 费用检测
                decimal Num = Convert.ToDecimal(gridView2.GetRowCellValue(0, "Num").ToString() == "" ? "0" : gridView2.GetRowCellValue(0, "Num").ToString());
                decimal FeeWeight = Convert.ToDecimal(gridView2.GetRowCellValue(0, "FeeWeight").ToString() == "" ? "0" : gridView2.GetRowCellValue(0, "FeeWeight").ToString());
                decimal FeeVolume = Convert.ToDecimal(gridView2.GetRowCellValue(0, "FeeVolume").ToString() == "" ? "0" : gridView2.GetRowCellValue(0, "FeeVolume").ToString());

                decimal Weight = Convert.ToDecimal(gridView2.GetRowCellValue(0, "Weight").ToString() == "" ? "0" : gridView2.GetRowCellValue(0, "Weight").ToString());
                decimal Volume = Convert.ToDecimal(gridView2.GetRowCellValue(0, "Volume").ToString() == "" ? "0" : gridView2.GetRowCellValue(0, "Volume").ToString());
                decimal WeightPrice = Convert.ToDecimal(gridView2.GetRowCellValue(0, "WeightPrice").ToString() == "" ? "0" : gridView2.GetRowCellValue(0, "WeightPrice").ToString());
                decimal VolumePrice = Convert.ToDecimal(gridView2.GetRowCellValue(0, "VolumePrice").ToString() == "" ? "0" : gridView2.GetRowCellValue(0, "VolumePrice").ToString());
                decimal Freight = Convert.ToDecimal(gridView2.GetRowCellValue(0, "Freight").ToString() == "" ? "0" : gridView2.GetRowCellValue(0, "Freight").ToString());

                decimal DeliFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "DeliFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "DeliFee").ToString());
                decimal DeclareValue = Convert.ToDecimal(gridView1.GetRowCellValue(0, "DeclareValue").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "DeclareValue").ToString());
                decimal SupportValue = Convert.ToDecimal(gridView1.GetRowCellValue(0, "SupportValue").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "SupportValue").ToString());
                decimal Tax = Convert.ToDecimal(gridView1.GetRowCellValue(0, "Tax").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "Tax").ToString());
                decimal InformationFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "InformationFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "InformationFee").ToString());

                decimal Expense = Convert.ToDecimal(gridView1.GetRowCellValue(0, "Expense").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "Expense").ToString());
                decimal DiscountTransfer = Convert.ToDecimal(gridView1.GetRowCellValue(0, "DiscountTransfer").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "DiscountTransfer").ToString());
                decimal CollectionPay = Convert.ToDecimal(gridView1.GetRowCellValue(0, "CollectionPay").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "CollectionPay").ToString());
                decimal AgentFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "AgentFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "AgentFee").ToString());
                decimal FuelFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "FuelFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "FuelFee").ToString());

                decimal UpstairFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "UpstairFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "UpstairFee").ToString());
                decimal HandleFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "HandleFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "HandleFee").ToString());
                decimal ForkliftFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "ForkliftFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "ForkliftFee").ToString());
                decimal StorageFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "StorageFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "StorageFee").ToString());
                decimal CustomsFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "CustomsFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "CustomsFee").ToString());
                decimal packagFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "packagFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "packagFee").ToString());

                decimal MatPay = Convert.ToDecimal(gridView1.GetRowCellValue(0, "MatPay").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "MatPay").ToString());//HJ20180911
                decimal FrameFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "FrameFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "FrameFee").ToString());
                decimal ChangeFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "ChangeFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "ChangeFee").ToString());
                decimal OtherFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "OtherFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "OtherFee").ToString());
                decimal NoticeFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "NoticeFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "NoticeFee").ToString());
                decimal ReceiptFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "ReceiptFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "ReceiptFee").ToString());
                decimal ReceivFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "ReceivFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "ReceivFee").ToString());

                decimal WarehouseFee = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "WarehouseFee"));

                decimal NowPay_s = ConvertType.ToDecimal(NowPay.Text);
                decimal FetchPay_s = ConvertType.ToDecimal(FetchPay.Text);
                decimal MonthPay_s = ConvertType.ToDecimal(MonthPay.Text);
                decimal ShortOwePay_s = ConvertType.ToDecimal(ShortOwePay.Text);
                decimal BefArrivalPay_s = ConvertType.ToDecimal(BefArrivalPay.Text);
                //zaj
                decimal ReceiptPay_s = ConvertType.ToDecimal(ReceiptPay.Text);
                decimal OwePay_s = ConvertType.ToDecimal(OwePay.Text);
                if (Num < 0 || FeeWeight < 0 || FeeVolume < 0
                    || Weight < 0 || Volume < 0 || WeightPrice < 0 || VolumePrice < 0 || Freight < 0
                    || DeliFee < 0 || DeclareValue < 0 || SupportValue < 0 || Tax < 0 || InformationFee < 0
                    || Expense < 0 || DiscountTransfer < 0 || CollectionPay < 0 || AgentFee < 0 || FuelFee < 0
                    || UpstairFee < 0 || HandleFee < 0 || ForkliftFee < 0 || StorageFee < 0 || CustomsFee < 0 || packagFee < 0
                    || FrameFee < 0 || ChangeFee < 0 || OtherFee < 0 || NoticeFee < 0 || ReceiptFee < 0 || ReceivFee < 0
                    || WarehouseFee < 0 || NowPay_s < 0 || FetchPay_s < 0 || MonthPay_s < 0 || ShortOwePay_s < 0 || BefArrivalPay_s < 0 || ReceiptPay_s < 0 || OwePay_s < 0 || MatPay < 0)
                {
                    MsgBox.ShowOK("输入数字不能小于0，请检查！");
                    return;
                }


                #region 附加费检测新
                //回单检测
                if (IsReceiptFee.Checked && ReceiptFee <= 0)
                {
                    if (MsgBox.ShowYesNo("您勾选了回单,没有填写回单费是否继续?") != DialogResult.Yes) return;
                }
                //控货检测
                if (NoticeState.Checked && NoticeFee <= 0)
                {
                    if (MsgBox.ShowYesNo("您勾选了控货,没有填写等通知放货费是否继续?") != DialogResult.Yes) return;
                }
                //代收检测
                if (IsAgentFee.Checked)
                {
                    if (CollectionPay <= 0)
                    {
                        if (MsgBox.ShowYesNo("您勾选了代收,没有填写代收货款是否继续?") != DialogResult.Yes) return;
                    }
                    if (AgentFee <= 0)
                    {
                        if (MsgBox.ShowYesNo("您勾选了代收,没有填写代收手续费是否继续?") != DialogResult.Yes) return;
                    }

                }
                //保险检测
                if (IsSupportValue.Checked && SupportValue <= 0)
                {
                    if (MsgBox.ShowYesNo("您勾选了保险,没有填写保价费是否继续?") != DialogResult.Yes) return;
                }
                //包装检测
                if (IsPackagFee.Checked && packagFee <= 0)
                {
                    if (MsgBox.ShowYesNo("您勾选了包装,没有填写包装费是否继续?") != DialogResult.Yes) return;
                }
                //木架检测
                if (IsFrameFee.Checked && FrameFee <= 0)
                {
                    if (MsgBox.ShowYesNo("您勾选了木架,没有填写代打木架费是否继续?") != DialogResult.Yes) return;
                }
                //上楼检测
                if (IsUpstairFee.Checked && UpstairFee <= 0)
                {
                    if (MsgBox.ShowYesNo("您勾选了上楼,没有填写上楼费是否继续?") != DialogResult.Yes) return;
                }
                //装卸费检测
                if (IsHandleFee.Checked && HandleFee <= 0)
                {
                    if (MsgBox.ShowYesNo("您勾选了装卸费,没有填写装卸费是否继续?") != DialogResult.Yes) return;
                }
                //叉车费检测
                if (IsForkliftFee.Checked && ForkliftFee <= 0)
                {
                    if (MsgBox.ShowYesNo("您勾选了叉车,没有填写叉车费是否继续?") != DialogResult.Yes) return;
                }
                //进仓费检测
                if (IsStorageFee.Checked && StorageFee <= 0)
                {
                    if (MsgBox.ShowYesNo("您勾选了进仓,没有填写进仓费是否继续?") != DialogResult.Yes) return;
                }
                //仓储费检测
                if (IsWarehouseFee.Checked && WarehouseFee <= 0)
                {
                    if (MsgBox.ShowYesNo("您勾选了仓储,没有填写仓储费是否继续?") != DialogResult.Yes) return;
                }
                //报关费检测
                if (IsCustomsFee.Checked && CustomsFee <= 0)
                {
                    if (MsgBox.ShowYesNo("您勾选了报关,没有填写报关费是否继续?") != DialogResult.Yes) return;
                }
                //发票费检测
                if (IsInvoice.Checked && Tax <= 0)
                {
                    if (MsgBox.ShowYesNo("您勾选了发票,没有填写税金是否继续?") != DialogResult.Yes) return;
                }
                //改单费检测
                if (IsChangeFee.Checked && ChangeFee <= 0)
                {
                    if (MsgBox.ShowYesNo("您勾选了改单,没有填写改单费是否继续?") != DialogResult.Yes) return;
                }
                //垫付检测
                if (IsMatPay.Checked && MatPay <= 0)
                {
                    if (MsgBox.ShowYesNo("您勾选了垫付,没有填写垫付费是否继续?") != DialogResult.Yes) return;
                }
                //其他费检测
                if (IsOtherFee.Checked && OtherFee <= 0)
                {
                    if (MsgBox.ShowYesNo("您勾选了其他,没有填写其他费是否继续?") != DialogResult.Yes) return;
                }

                #endregion

                //zaj 2017-11-15
                //if (TransferMode.Text.Trim() == "网点送货" && DeliFee == 0)
                //{
                //    if (MsgBox.ShowYesNo("交接方式为网点送货,但是没有填开单送货费,是否继续保存？") != DialogResult.Yes)
                //        return;
                //}
                //if (TransferMode.Text.Trim() == "送货" && DeliFee == 0)
                //{
                //    if (MsgBox.ShowYesNo("交接方式为送货,但是没有填开单送货费,是否继续保存？") != DialogResult.Yes)
                //        return;
                //}


                //if (TransferMode.Text == "网点送货" || TransferMode.Text == "中转送货" || TransferMode.Text == "中心直送")
                //{
                //    if (ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "DeliFee")) < ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "DeliveryFee")))
                //    {
                //        MsgBox.ShowOK("开单送货费必须大于等于结算送货费，请检查！");
                //        return;
                //    }
                //}
                //if (ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "OperationWeight")) == 0)
                //{
                //    MsgBox.ShowOK("结算重量必须大于0，请检查！");
                //    return;
                //}
                //if (ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "OptWeight")) == 0)
                //{
                //    MsgBox.ShowOK("操作重量必须大于0，请检查！");
                //    return;
                //}
                for (int i = 0; i < RowCount; i++)
                {
                    if (ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "Num")) != 0
                        || ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeWeight")) != 0
                        || ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeVolume")) != 0
                        || ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "Weight")) != 0
                        || ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "Volume")) != 0
                        || ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "Freight")) != 0
                        )
                        if (GridOper.GetRowCellValueString(gridView2, i, "Varieties") == "" || GridOper.GetRowCellValueString(gridView2, i, "Package") == "")
                        {
                            MsgBox.ShowOK("第" + (i + 1) + "行必须录入品名、包装，请检查！");
                            return;
                        }
                }
                #endregion

                //if (StartSite.Text == "深圳" && ReceivCity.Text == "深圳市" && TransitMode.Text.Trim() != "中强城际")
                //{
                //    MsgBox.ShowOK("深圳到深圳必须选择中强城际！");
                //    TransitMode.Focus();
                //    return;
                //}
                //ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "DiscountTransfer")) > 1000
                //    &&
                ///ljp 于2016-09-17 注释，暂时去除判断
                //if ( (DisTranName.Text.Trim() == "" || DisTranBank.Text.Trim() == "" || DisTranBranch.Text.Trim() == "" || DisTranAccount.Text.Trim() == ""))
                //{
                //    MsgBox.ShowOK("有折扣折让金额必须填写银行卡信息！");
                //    xtraTabControl1.SelectedTabPage = xtraTabPage2;
                //    DisTranName.Focus();
                //    return;
                //}

                //if (ReceivProvince.Text.Trim() == "短欠" && !NoticeState.Checked)
                //{
                //    MsgBox.ShowOK("短欠必须控货！");
                //    return;
                //}

                //if (StartSite.Text.Trim() != CommonClass.UserInfo.SiteName && xmdr == null)
                //{
                //    MsgBox.ShowOK("本运单开单站点为：" + begWeb.Text.Trim() + "，不属于当前站点：" + CommonClass.UserInfo.WebName + "，不能保存！");
                //    return;
                //}

                #region 省市检测
                //DataRow[] dr = ((DataTable)gridControl3.DataSource).Select("MiddleProvince='" + ReceivProvince.Text.Trim() + "'");
                //if (dr.Length == 0)
                //{
                //    ReceivProvince.Focus();
                //    MsgBox.ShowOK("请选择正确的省份！");
                //    return;
                //}

                //DataRow[] dr1 = ((DataTable)gridControl9.DataSource).Select("MiddleCity='" + ReceivCity.Text.Trim() + "'");
                //if (dr1.Length == 0)
                //{
                //    ReceivCity.Focus();
                //    MsgBox.ShowOK("请选择正确的城市！");
                //    return;
                //}

                //DataRow[] dr2 = ((DataTable)gridControl10.DataSource).Select("MiddleArea='" + ReceivArea.Text.Trim() + "'");
                //if (dr2.Length == 0)
                //{
                //    ReceivArea.Focus();
                //    MsgBox.ShowOK("请选择正确区县！");
                //    return;
                //}
                //if (TransferMode.Text.Trim() != "自提")
                //{
                //    if (NotStreet == null)
                //    {
                //        flag = true;
                //    }
                //    else
                //    {
                //        flag = false;
                //    }

                //    if (flag)
                //    {
                //        DataRow[] dr3 = ((DataTable)gcdaozhan.DataSource).Select("MiddleStreet='" + ReceivStreet.Text.Trim() + "'");
                //        if (dr3.Length == 0)
                //        {
                //            ReceivStreet.Focus();
                //            MsgBox.ShowOK("请选择正确街道！");
                //            return;
                //        }
                //    }
                //}
                #endregion

                #region 验证四级地址是否包含选择的交接方式，不包含则提示错误
                //try
                //{

                //    //验证公司是否被授权
                //    if (TransferMode.Text.Trim() != "自提")
                //    {
                //        bool isLimit = true;
                //        string sql = "MiddleProvince='" + ReceivProvince.Text.Trim() + "' and MiddleCity='" + ReceivCity.Text.Trim() + "' and MiddleArea='" + ReceivArea.Text.Trim() + "' and MiddleStreet='"+ReceivStreet.Text.Trim()+"'";
                //        DataRow[] drs = CommonClass.dsMiddleSite.Tables[0].Select(sql);
                //        if (drs.Length > 0)
                //        {
                //            for (int i = 0; i < drs.Length; i++)
                //            {
                //                if (drs[i]["Type"].ToString() != "自提")
                //                {
                //                    isLimit = false;
                //                    // MsgBox.ShowOK("您选择的四级地址只能开自提单！");
                //                    // return;
                //                }                             
                //            }

                //        }
                //        if (isLimit)
                //        {
                //            if (NotStreet == null)
                //            {
                //                flag = true;
                //            }
                //            else
                //            {
                //                flag = false;
                //            }

                //            if (flag)
                //            {
                //                MsgBox.ShowOK("您选择的四级地址只能开自提单！");
                //                return;
                //            }
                //        }
                //    }
                //    //List<SqlPara> list = new List<SqlPara>();
                //    //list.Add(new SqlPara("MiddleProvince", ReceivProvince.Text.Trim()));
                //    //list.Add(new SqlPara("MiddleCity", ReceivCity.Text.Trim()));
                //    //list.Add(new SqlPara("MiddleArea", ReceivArea.Text.Trim()));
                //    //list.Add(new SqlPara("MiddleStreet", ReceivStreet.Text.Trim()));
                //    //list.Add(new SqlPara("Type", TransferMode.Text.Trim()));
                //    //SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_CHECK_TransferMode", list);
                //    //DataSet ds = SqlHelper.GetDataSet(sps);
                //    //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)
                //    //{
                //    //    MsgBox.ShowOK("选择的省份、城市、区县、街道 不存在《" + this.TransferMode.Text.Trim() + "》的交接方式，请重新选择！");
                //    //    this.TransferMode.Focus();
                //    //    return;
                //    //}
                //}
                //catch (Exception ex)
                //{
                //    MsgBox.ShowException(ex);
                //}
                #endregion

                #region 结算送货费检测
                //如需放开请找老司机
                /*decimal DeliveryFee = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "DeliveryFee"));
                if ((TransitMode.Text == "中强专线" || TransitMode.Text == "中强快线") || TransitMode.Text == "中强城际")// 
                {
                    if (TransferMode.Text == "网点送货" || TransferMode.Text.Trim() == "中转送货")
                    {
                        if (DeliveryFee == 0)
                        {
                            MsgBox.ShowOK("无结算送货费，请切换交接方式！");
                            return;
                        }
                    }
                }
                */
                #endregion

                #region 结算中转费
                /*DataRow[] drTransferFee = CommonClass.dsTransferFee.Tables[0].Select("TransferSite='" + TransferSite.Text.Trim() + "' and ToProvince='" + ReceivProvince.Text + "' and ToCity='" + ReceivCity.Text + "' and ToArea='" + ReceivArea.Text.Trim() + "'");
                if (drTransferFee.Length > 0 && TransferMode.Text != "网点送货")
                {
                    decimal HeavyPrice = ConvertType.ToDecimal(drTransferFee[0]["HeavyPrice"]);//重货
                    decimal LightPrice = ConvertType.ToDecimal(drTransferFee[0]["LightPrice"]);//轻货
                    decimal ParcelPriceMin = ConvertType.ToDecimal(drTransferFee[0]["ParcelPriceMin"]);//最低一票
                    decimal TransferFeeAll = 0;
                    for (int i = 0; i < RowCount; i++)
                    {
                        decimal w = ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeWeight"));
                        decimal v = ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeVolume"));
                        string Package = gridView2.GetRowCellValue(i, "Package").ToString();
                        decimal fee = Math.Max(w * HeavyPrice, v * LightPrice);
                        if (Package != "纸箱" && Package != "纤袋" && Package != "膜")
                        {
                            TransferFeeAll += fee * (decimal)1.1;
                        }
                        else
                        {
                            TransferFeeAll += fee;
                        }
                    }
                    decimal acc = Math.Max(TransferFeeAll, ParcelPriceMin);
                    gridView8.SetRowCellValue(0, "TransferFee", Math.Round(acc, 2));
                }
                */
                #endregion

                #region 附加费
                /*
                if (!CommonClass.UserInfo.WebName.Contains("客户部"))
                {
                    #region 保价费
                    if (DeclareValue > 0)
                    {
                        DataRow[] SurchDr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='保价费'");
                        if (SurchDr.Length > 0)
                        {
                            decimal OutLowest = ConvertType.ToDecimal(SurchDr[0]["OutLowest"]);
                            decimal SupportValue_s = Math.Max(DeclareValue * (decimal)(0.001), OutLowest);
                            if (SupportValue < SupportValue_s)
                            {
                                MsgBox.ShowOK("保价费低于标准，不能保存！");
                                return;
                            }
                        }
                    }
                    #endregion

                    #region 税金
                    if (IsInvoice.Checked)
                    {
                        DataRow[] SurchDr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='税金'");
                        if (SurchDr.Length > 0)
                        {
                            decimal OutLowest = ConvertType.ToDecimal(SurchDr[0]["OutLowest"]);
                            decimal PaymentAmout_s = (ConvertType.ToDecimal(PaymentAmout.Text) - Tax) * (decimal)0.08;
                            decimal Tax_s = Math.Max(PaymentAmout_s, OutLowest);
                            if (Tax < Tax_s)
                            {
                                MsgBox.ShowOK("税金低于标准，不能保存！");
                                return;
                            }
                        }
                    }
                    #endregion

                    #region 回单费
                    if (ReceiptCondition.Text.Trim() == "签回单" && PaymentMode.Text.Trim() != "月结")
                    {
                        DataRow[] SurchDr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='回单费'");
                        if (SurchDr.Length > 0)
                        {
                            decimal OutLowest = ConvertType.ToDecimal(SurchDr[0]["OutLowest"]);
                            decimal ReceiptFee_s = Math.Max(5, OutLowest);
                            if (ReceiptFee < ReceiptFee_s)
                            {
                                MsgBox.ShowOK("回单费低于标准，不能保存！");
                                return;
                            }
                        }
                    }
                    #endregion

                    #region 代收手续费
                    if (IsAgentFee.Checked)
                    {
                        DataRow[] SurchDr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='代收手续费'");
                        if (SurchDr.Length > 0)
                        {
                            decimal OutLowest = ConvertType.ToDecimal(SurchDr[0]["OutLowest"]);
                            decimal AgentFee_s = Math.Max(CollectionPay * (decimal)0.003, OutLowest);
                            if (ConvertType.ToDecimal(PaymentAmout.Text) <= (ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "DiscountTransfer")) / (decimal)3))
                            {
                                AgentFee_s = Math.Max(ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "DiscountTransfer")) * (decimal)0.003, OutLowest);
                            }
                            if (AgentFee < AgentFee_s)
                            {
                                MsgBox.ShowOK("代收手续费低于标准，不能保存！");
                                return;
                            }
                        }
                    }
                    #endregion

                    #region 控货费
                    if (NoticeState.Checked)
                    {
                        DataRow[] SurchDr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='控货费'");
                        if (SurchDr.Length > 0)
                        {
                            decimal OutLowest = ConvertType.ToDecimal(SurchDr[0]["OutLowest"]);
                            decimal NoticeFee_s = Math.Max(10, OutLowest);
                            if (NoticeFee < NoticeFee_s)
                            {
                                MsgBox.ShowOK("控货费低于标准，不能保存！");
                                return;
                            }
                        }
                    }
                    #endregion
                }
                */
                #endregion

                #region  3顿8方业务
                //string err = "";
                //bool isJudge = false;

                //decimal ttlFeeWeight = 0, ttlFeeVolume = 0;
                //for (int i = 0; i < RowCount; i++)
                //{
                //    if (gridView2.GetRowCellValue(i, "Varieties").ToString() != "")
                //    {
                //        ttlFeeWeight += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeWeight"));
                //        ttlFeeVolume += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeVolume"));
                //    }
                //}
                //if (ttlFeeWeight >= 3000 || ttlFeeVolume >= 8)
                //{
                //    //maohui20180903 (取消3吨8方限制)
                //    //err = "始发站为" + StartSite.Text.Trim() + "，收货城市为" + ReceivCity.Text.Trim() + "，业务满足3吨8个方，";
                //    //if (TransferMode.Text.Trim() == "自提")
                //    //{
                //    //    if ((ReceivCity.Text.Trim().Equals("深圳市") || ReceivCity.Text.Trim().Equals("东莞市")) && !PickGoodsSite.Text.Equals("东莞大坪分拨中心"))
                //    //    {
                //    //        isJudge = true;
                //    //        err += "\r\n\r\n目的网点将由【" + PickGoodsSite.Text.Trim() + "】改为【东莞大坪分拨中心】，结算送货费改为【0】";
                //    //        NewPickGoodsSite = "东莞大坪分拨中心";
                //    //        NewDeliveryFee = 0;     // 中心自提免结算送货费
                //    //    }
                //    //}
                //    //else
                //    //{
                //    //    if (TransferSite.Text.Trim().Equals("深圳") && !PickGoodsSite.Text.Equals("东莞大坪分拨中心"))
                //    //    {
                //    //        isJudge = true;
                //    //        err += "\r\n\r\n目的网点将由【" + PickGoodsSite.Text.Trim() + "】改为【东莞大坪分拨中心】";
                //    //        NewPickGoodsSite = "东莞大坪分拨中心";
                //    //    }


                //    //}
                //    //err += "，是否继续保存？";
                //}
                //if (isJudge)
                //{
                //    if (MsgBox.ShowYesNo(err) == DialogResult.No)
                //    {
                //        return;
                //    }
                //}

                //if (!PickGoodsSite.Text.Trim().Contains("海口秀英营业部")
                //   && !PickGoodsSite.Text.Trim().Contains("惠州博罗龙溪营业部")
                //   && !CommonClass.UserInfo.WebName.Equals("深港营业部")
                //   && !ReceivProvince.Text.Trim().Equals("香港")
                //   && !PickGoodsSite.Text.Trim().Equals("泰州泰兴营业部")
                //   && !PickGoodsSite.Text.Trim().Equals("东莞虎门北栅营业部")
                //    //  && !PickGoodsSite.Text.Trim().Equals("东莞清溪清凤路营业部")
                //   )
                //{
                //    string err = "";
                //    bool isJudge = false;
                //    string startSiteToJHL = "青岛、成都、重庆、徐州、天津";   // 到深圳落货金华伦的始发站点
                //    ////yzw ZQTMS-2872 全国各网点发往武汉网点符合条件转向武汉二级分拨中心
                //    //string WHSpecialWebs = "武汉黄陂汉口北营业部,武汉汉南陡埠村营业部,武汉黄陂前川路营业部,武汉沌口汽配城营业部,武汉新洲高新街营业部";
                //    decimal ttlFeeWeight = 0, ttlFeeVolume = 0;
                //    for (int i = 0; i < RowCount; i++)
                //    {
                //        if (gridView2.GetRowCellValue(i, "Varieties").ToString() != "")
                //        {
                //            ttlFeeWeight += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeWeight"));
                //            ttlFeeVolume += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeVolume"));
                //        }
                //    }

                //    if (ttlFeeWeight >= 3000 || ttlFeeVolume >= 8)
                //    {
                //        err = "始发站为" + StartSite.Text.Trim() + "，收货城市为" + City.Text.Trim() + "，业务满足3吨8个方，";
                //        if (TransferMode.Text.Trim() == "自提")
                //        {
                //            if ((City.Text.Trim().Equals("深圳市") || City.Text.Trim().Equals("东莞市")) && !PickGoodsSite.Text.Equals("东莞大坪分拨中心") && !startSiteToJHL.Contains(StartSite.Text.Trim()))
                //            {
                //                isJudge = true;
                //                err += "\r\n\r\n目的网点将由【" + PickGoodsSite.Text.Trim() + "】改为【东莞大坪分拨中心】，结算送货费改为【0】";
                //                NewPickGoodsSite = "东莞大坪分拨中心";
                //                NewDeliveryFee = 0;     // 中心自提免结算送货费
                //            }
                //            //yzw 其他到深圳落货金华伦的始发站点
                //            if ((City.Text.Trim().Equals("深圳市") || City.Text.Trim().Equals("东莞市")) && !PickGoodsSite.Text.Equals("深圳金华伦营业部") && startSiteToJHL.Contains(StartSite.Text.Trim()))
                //            {
                //                isJudge = true;
                //                err += "\r\n\r\n目的网点将由【" + PickGoodsSite.Text.Trim() + "】改为【深圳金华伦营业部】，结算送货费改为【0】";
                //                NewPickGoodsSite = "深圳金华伦营业部";
                //                NewDeliveryFee = 0;     // 中心自提免结算送货费
                //            }

                //            //二级中转市县萧山站点，单票满足3吨或者8个方以上的自提货物（含）由杭州操作部操作 2018.03.26 ccd
                //            if (PickGoodsSite.Text.Equals("萧山操作部"))
                //            {
                //                isJudge = true;
                //                err = "单票满足3吨或者8个方以上的自提货物（含）由杭州操作部操作\r\n\r\n目的网点将由【萧山操作部】改为【杭州操作部】";
                //                NewPickGoodsSite = "杭州操作部";
                //            }
                //            if (TransferSite.Text.Trim().Equals("无锡") && !PickGoodsSite.Text.Equals("无锡二级分拨中心"))
                //            {
                //                isJudge = true;
                //                err += "\r\n\r\n目的网点将由【" + PickGoodsSite.Text.Trim() + "】改为【无锡二级分拨中心】";
                //                NewPickGoodsSite = "无锡二级分拨中心";
                //                NewDeliveryFee = 0;     // 中心自提免结算送货费
                //            }
                //            //tc20180813
                //            if (PickGoodsSite.Text.Trim().Equals("武汉李桥营业部")
                //                || PickGoodsSite.Text.Trim().Equals("武汉乘丰营业部")
                //                || PickGoodsSite.Text.Trim().Equals("武汉汉阳营业部")
                //                || PickGoodsSite.Text.Trim().Equals("武汉新琪安营业部"))
                //            {
                //                isJudge = true;
                //                err += "\r\n\r\n目的网点将由【" + PickGoodsSite.Text.Trim() + "】改为【武汉二级分拨中心】，结算送货费改为【0】";
                //                NewPickGoodsSite = "武汉二级分拨中心";
                //                NewDeliveryFee = 0;     // 中心自提免结算送货费
                //            }
                //            ////yzw ZQTMS-2872
                //            //if (WHSpecialWebs.Contains(PickGoodsSite.Text.Trim()))
                //            //{
                //            //    List<SqlPara> listWH = new List<SqlPara>();
                //            //    listWH.Add(new SqlPara("PickGoodsSite", PickGoodsSite.Text.Trim()));
                //            //    listWH.Add(new SqlPara("ReceivStreet", ReceivStreet.Text.Trim()));
                //            //    listWH.Add(new SqlPara("TransferMode", "自提"));
                //            //    SqlParasEntity speWH = new SqlParasEntity(OperType.Query, "QSP_3T8FWH_Web", listWH);
                //            //    DataSet ds = SqlHelper.GetDataSet(speWH);
                //            //    if (ds != null && ds.Tables[0].Rows.Count > 0)
                //            //    {
                //            //        isJudge = true;
                //            //        err = "业务满足3吨8个方,目的网点将由【" + PickGoodsSite.Text.Trim() + "】改为【" + ds.Tables[0].Rows[0]["NewWeb"].ToString() + "】，结算送货费改为【0】";
                //            //        NewPickGoodsSite = ds.Tables[0].Rows[0]["NewWeb"].ToString();
                //            //        NewDeliveryFee = 0;     // 中心自提免结算送货费
                //            //    }
                //            //}

                //        }
                //        else
                //        {
                //            if (TransferSite.Text.Trim().Equals("深圳") && !PickGoodsSite.Text.Equals("东莞大坪分拨中心") && !startSiteToJHL.Contains(StartSite.Text.Trim()))
                //            {
                //                isJudge = true;
                //                err += "\r\n\r\n目的网点将由【" + PickGoodsSite.Text.Trim() + "】改为【东莞大坪分拨中心】";
                //                NewPickGoodsSite = "东莞大坪分拨中心";
                //            }
                //            //yzw 其他到深圳落货金华伦的始发站点
                //            if ((TransferSite.Text.Trim().Equals("深圳")) && !PickGoodsSite.Text.Equals("深圳金华伦营业部") && startSiteToJHL.Contains(StartSite.Text.Trim()))
                //            {
                //                isJudge = true;
                //                err += "\r\n\r\n目的网点将由【" + PickGoodsSite.Text.Trim() + "】改为【深圳金华伦营业部】";
                //                NewPickGoodsSite = "深圳金华伦营业部";

                //            }
                //            if (TransferSite.Text.Trim().Equals("无锡") && !PickGoodsSite.Text.Equals("无锡二级分拨中心") && !PickGoodsSite.Text.Equals("常州武进戴洛路营业部"))
                //            {
                //                isJudge = true;
                //                err += "\r\n\r\n目的网点将由【" + PickGoodsSite.Text.Trim() + "】改为【无锡二级分拨中心】";
                //                NewPickGoodsSite = "无锡二级分拨中心";
                //            }

                //            ////yzw ZQTMS-2872
                //            //if (WHSpecialWebs.Contains(PickGoodsSite.Text.Trim()) && TransferMode.Text.Trim() == "送货")
                //            //{
                //            //    List<SqlPara> listWH = new List<SqlPara>();
                //            //    listWH.Add(new SqlPara("PickGoodsSite", PickGoodsSite.Text.Trim()));
                //            //    listWH.Add(new SqlPara("ReceivStreet", ReceivStreet.Text.Trim()));
                //            //    listWH.Add(new SqlPara("TransferMode", "送货"));
                //            //    SqlParasEntity speWH = new SqlParasEntity(OperType.Query, "QSP_3T8FWH_Web", listWH);
                //            //    DataSet ds = SqlHelper.GetDataSet(speWH);
                //            //    if (ds != null && ds.Tables[0].Rows.Count > 0)
                //            //    {
                //            //        isJudge = true;
                //            //        err = "业务满足3吨8个方,目的网点将由【" + PickGoodsSite.Text.Trim() + "】改为【" + ds.Tables[0].Rows[0]["NewWeb"].ToString() + "】";
                //            //        NewPickGoodsSite = ds.Tables[0].Rows[0]["NewWeb"].ToString();

                //            //    }
                //            //}

                //        }

                //        err += "，是否继续保存？";
                //    }

                //    if (isJudge)
                //    {
                //        if (MsgBox.ShowYesNo(err) == DialogResult.No)
                //        {
                //            return;
                //        }
                //    }
                //}
                #endregion

                //yzw ZQTMS-2443昆山站点设置3吨8方限制
                #region 昆山站点设置3吨8方
                //string transferSite = TransferSite.Text.Trim();//中转地
                //string pickGoodsSite = PickGoodsSite.Text.Trim();//目的网点
                //decimal alllFeeWeight = 0, alllFeeVolume = 0;
                //for (int i = 0; i < RowCount; i++)
                //{
                //    if (gridView2.GetRowCellValue(i, "Varieties").ToString() != "")
                //    {
                //        alllFeeWeight += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeWeight"));
                //        alllFeeVolume += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeVolume"));
                //    }
                //}
                //if ((alllFeeWeight >= 3000 || alllFeeVolume >= 8) && transferSite == "昆山" && pickGoodsSite != "昆山操作部")
                //{
                //    string b = TransitMode.Text.Trim();
                //    if (MsgBox.ShowYesNo("满足三吨八个方,目的网点将变更为昆山操作部,是否确认?") == DialogResult.Yes)
                //    {
                //        PickGoodsSite.Text = "昆山操作部";
                //        TransitMode.Text = b;
                //        SetFee();

                //    }
                //    else
                //    {
                //        return;
                //    }
                //}
                #endregion

                //if (TransferSite.Text.Trim().Equals("深圳") && TransferMode.Text.Trim().Equals("专车直送") && !PickGoodsSite.Text.Trim().Equals("东莞大坪分拨中心") && !ReceivProvince.Text.Trim().Equals("香港"))
                //{


                //    PickGoodsSite.EditValue = "东莞大坪分拨中心";
                //    MsgBox.ShowOK("中转地为深圳，交接方式为专车直送，\n目的网点将改成东莞大坪分拨中心");
                //}
                #region 3吨8个方 发往松江站点
                //if (TransferSite.Text.Trim().Equals("松江"))
                //{
                //    string err = "";
                //    bool isJudge = false;

                //    decimal ttlFeeWeight = 0, ttlFeeVolume = 0;
                //    for (int i = 0; i < RowCount; i++)
                //    {
                //        if (gridView2.GetRowCellValue(i, "Varieties").ToString() != "")
                //        {
                //            ttlFeeWeight += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeWeight"));
                //            ttlFeeVolume += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeVolume"));
                //        }
                //    }

                //    if ((ttlFeeWeight >= 3000 || ttlFeeVolume >= 8) && !PickGoodsSite.Text.Equals("上海青浦操作部"))
                //    {
                //        err = "发往松江站点，业务满足3吨8个方，";
                //        isJudge = true;
                //        err += "\r\n\r\n目的网点将由【" + PickGoodsSite.Text.Trim() + "】改为【上海青浦操作部】";
                //        NewPickGoodsSite = "上海青浦操作部";
                //        err += "，是否继续保存？";
                //    }
                //    if (isJudge)
                //    {
                //        if (MsgBox.ShowYesNo(err) == DialogResult.No)
                //        {
                //            return;
                //        }
                //    }
                //}
                #endregion

                #region 金额检测
                //if (CommonClass.UserInfo.companyid !="152" && ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "CollectionPay")) > 0
                //            && (CollectionName.Text.Trim() == "" || CollectionBank.Text.Trim() == "" || CollectionBranch.Text.Trim() == "" || CollectionAccount.Text.Trim() == ""))
                //{
                //    MsgBox.ShowOK("有代收货款金额必须填写银行卡信息！");
                //    xtraTabControl1.SelectedTabPage = xtraTabPage3;
                //    DisTranName.Focus();
                //    return;
                //}
                //if (ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "CollectionPay")) > 0 && IsCenterBack.Text.Trim() == "")
                //{
                //    MsgBox.ShowOK("有代收货款金额必须选择返款类型！");
                //    xtraTabControl1.SelectedTabPage = xtraTabPage3;
                //    DisTranName.Focus();
                //    return;
                //}

                if (ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "CollectionPay")) > 0
                    && (ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "CollectionPay")) > ConvertType.ToDecimal(FetchPay.Text.Trim())))
                {
                    MsgBox.ShowOK("提付金额不能小于代收货款！");
                    return;
                }

                //if (TransitMode.Text.Trim() == "中强整车" && VehicleNo.Text.Trim() == "")
                //{
                //    MsgBox.ShowOK("整车直送必须录入整车申请号！");
                //    return;
                //}
                //zaj 2017-11-22 加上回单付
                if (PaymentMode.Text.Trim() != "免费" &&
                    (ConvertType.ToDecimal(NowPay.Text)
                    + ConvertType.ToDecimal(FetchPay.Text)
                    + ConvertType.ToDecimal(MonthPay.Text)
                    + ConvertType.ToDecimal(ShortOwePay.Text)
                    + ConvertType.ToDecimal(ReceiptPay.Text) + ConvertType.ToDecimal(OwePay.Text)
                    + ConvertType.ToDecimal(BefArrivalPay.Text)) == 0
                    )
                {
                    MsgBox.ShowOK("请输入运费");
                    return;
                }
                if (ConvertType.ToDecimal(NowPay.Text)
                    + ConvertType.ToDecimal(FetchPay.Text)
                    + ConvertType.ToDecimal(MonthPay.Text)
                    + ConvertType.ToDecimal(ShortOwePay.Text)
                    + ConvertType.ToDecimal(BefArrivalPay.Text)
                    + ConvertType.ToDecimal(ReceiptPay.Text) + ConvertType.ToDecimal(OwePay.Text)
                    != ConvertType.ToDecimal(PaymentAmout.Text))
                {
                    MsgBox.ShowOK("两笔付金额不等于总金额：" + ConvertType.ToDecimal(PaymentAmout.Text));
                    return;
                }
                //zaj
                if (PaymentMode.Text.Trim() == "两笔付")
                {
                    if (NowPay_s > 0 && (FetchPay_s + MonthPay_s + ShortOwePay_s + BefArrivalPay_s + ReceiptPay_s + OwePay_s) == 0)
                    {
                        MsgBox.ShowOK("两笔付必须有两种付款类型");
                        return;
                    }
                    if (FetchPay_s > 0 && (NowPay_s + MonthPay_s + ShortOwePay_s + BefArrivalPay_s + ReceiptPay_s + OwePay_s) == 0)
                    {
                        MsgBox.ShowOK("两笔付必须有两种付款类型");
                        return;
                    }
                    if (MonthPay_s > 0 && (NowPay_s + FetchPay_s + ShortOwePay_s + BefArrivalPay_s + ReceiptPay_s + OwePay_s) == 0)
                    {
                        MsgBox.ShowOK("两笔付必须有两种付款类型");
                        return;
                    }
                    if (ShortOwePay_s > 0 && (NowPay_s + FetchPay_s + MonthPay_s + BefArrivalPay_s + ReceiptPay_s + OwePay_s) == 0)
                    {
                        MsgBox.ShowOK("两笔付必须有两种付款类型");
                        return;
                    }
                    if (BefArrivalPay_s > 0 && (NowPay_s + FetchPay_s + MonthPay_s + ShortOwePay_s + ReceiptPay_s + OwePay_s) == 0)
                    {
                        MsgBox.ShowOK("两笔付必须有两种付款类型");
                        return;
                    }

                    if (ReceiptPay_s > 0 && (BefArrivalPay_s + NowPay_s + FetchPay_s + MonthPay_s + ShortOwePay_s + OwePay_s) == 0)
                    {
                        MsgBox.ShowOK("两笔付必须有两种付款类型");
                        return;
                    }
                    if (OwePay_s > 0 && (BefArrivalPay_s + NowPay_s + FetchPay_s + MonthPay_s + ShortOwePay_s + ReceiptPay_s) == 0)
                    {
                        MsgBox.ShowOK("两笔付必须有两种付款类型");
                        return;
                    }
                }
                #endregion

                #region 检测最低运费

                if (PaymentMode.Text.Trim() != "免费")
                {
                    // 非专线上浮10%
                    // 非网点送货按目的网点省市区取
                    string province = "", city = "", area = "";
                    if (TransferMode.Text == "网点送货")
                    {
                        DataRow[] webdr = CommonClass.dsWeb.Tables[0].Select("WebName='" + PickGoodsSite.Text.Trim() + "'");
                        if (webdr.Length > 0)
                        {
                            province = webdr[0]["WebProvince"].ToString();
                            city = webdr[0]["WebCity"].ToString();
                            area = webdr[0]["WebArea"].ToString();
                        }
                    }
                    else
                    {
                        province = Province.Text.Trim();
                        city = City.Text.Trim();
                        area = ReceivArea.Text.Trim();
                    }
                    List<SqlPara> list_Fee = new List<SqlPara>();
                    list_Fee.Add(new SqlPara("StartSite", StartSite.Text.Trim()));
                    list_Fee.Add(new SqlPara("Province", province));
                    list_Fee.Add(new SqlPara("City", city));
                    list_Fee.Add(new SqlPara("Area", area));
                    list_Fee.Add(new SqlPara("TransferMode", TransferMode.Text.Trim()));
                    SqlParasEntity sps_fee = new SqlParasEntity(OperType.Query, "QSP_GET_BASFREIGHTFEE_KD", list_Fee);
                    DataSet dsFee = SqlHelper.GetDataSet(sps_fee);
                    if (dsFee != null && dsFee.Tables.Count > 0 && dsFee.Tables[0].Rows.Count > 0)
                    {
                        decimal ParcelPriceMin = ConvertType.ToDecimal(dsFee.Tables[0].Rows[0]["ParcelPriceMin"]);//最低一票
                        decimal HeavyPrice = ConvertType.ToDecimal(dsFee.Tables[0].Rows[0]["HeavyPrice"]);//重量单价
                        decimal LightPrice = ConvertType.ToDecimal(dsFee.Tables[0].Rows[0]["LightPrice"]);//体积单价

                        decimal fee = Math.Max(ParcelPriceMin, Math.Max(HeavyPrice * FeeWeight, LightPrice * FeeVolume));

                        // if (TransitMode.Text.Trim() != "中强专线")
                        // {
                        //     fee = fee + (fee * (decimal)0.15);
                        // }

                        //// ljp 2017-03-11 加入深圳站点按最低价格的50%来计算
                        // if (StartSite.Text.Trim().Equals("深圳"))
                        // {
                        //     fee = fee * (decimal)0.5;

                        // }

                        //  decimal PaymentAmout_s = ConvertType.ToDecimal(PaymentAmout.Text);
                        decimal PaymentAmout_s = 0;
                        for (int i = 0; i < RowCount; i++)
                        {
                            if (gridView2.GetRowCellValue(i, "Varieties").ToString() != "")
                            {
                                PaymentAmout_s += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "Freight"));
                            }
                        }
                        if (CommonClass.UserInfo.Discount > 0)//判断是否有折扣
                        {
                            PaymentAmout_s = PaymentAmout_s * (CommonClass.UserInfo.Discount / 10);
                        }
                        if (fee > PaymentAmout_s)
                        {
                            if (MsgBox.ShowYesNo("当前运费低于最低运费标准(" + fee + ")\r\n 是否继续开单?") != DialogResult.Yes) return;
                            //if (isFee)
                            //{
                            //    frmWayBillPower wf = new frmWayBillPower();
                            //    if (wf.ShowDialog() == DialogResult.Yes)
                            //    {
                            //        if (wf.UserAccount.Text.Trim() == "" || wf.Authorize.Text.Trim() == "")
                            //        {
                            //            MsgBox.ShowOK("必须输入总监工号与授权号！");
                            //            return;
                            //        }
                            //        List<SqlPara> list_Power = new List<SqlPara>();
                            //        list_Power.Add(new SqlPara("UserAccount", wf.UserAccount.Text.Trim()));
                            //        list_Power.Add(new SqlPara("Authorize", wf.Authorize.Text.Trim()));
                            //        SqlParasEntity sps_Power = new SqlParasEntity(OperType.Query, "QSP_GET_Authorize", list_Power);
                            //        DataSet dsPower = SqlHelper.GetDataSet(sps_Power);
                            //        if (dsPower != null && dsPower.Tables.Count > 0 && dsPower.Tables[0].Rows.Count > 0)
                            //        {
                            //            BillMan.Text = dsPower.Tables[0].Rows[0]["UserName"].ToString();
                            //        }
                            //        else
                            //        {
                            //            MsgBox.ShowOK("必须输入总监工号与授权号不正确，请检查！");
                            //            return;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        return;
                            //    }
                            //}
                            //else
                            //{
                            //    MsgBox.ShowOK("基本运费低于运费标准，请检查！");
                            //    return;
                            //}
                        }
                    }
                }

                #endregion

                #region 深圳站点300公斤及以下强制投保
                if (Upd_Num != 1 && isModify != 1)
                {
                    //if (!begWeb.Text.Equals("上海闵行昆阳营业部"))//begWeb.Text.Equals("深圳金鹏行营业部") && !begWeb.Text.Equals("成都上达营业部")
                    //{
                    //// 非月结且没勾选投保
                    //if (MonthPay_s <= 0 && IsSupportValue.Checked == false && PaymentMode.Text != "免费" && (CommonClass.UserInfo.companyid == "105"))//|| CommonClass.UserInfo.companyid == "106"
                    //{
                    //    MsgBox.ShowOK("必须选择保险");
                    //    return;
                    //}

                    // 非月结且没勾选投保
                    //if (MonthPay_s <= 0 && IsSupportValue.Checked == false && PaymentMode.Text != "免费" )//&& (CommonClass.UserInfo.companyid == "105"))//|| CommonClass.UserInfo.companyid == "106"
                    //{
                    //    MsgBox.ShowOK("必须选择保险");
                    //    return;
                    //}
                    //}

                    if (IsSupportValue.Checked == true && DeclareValue <= 0)
                    {
                        MsgBox.ShowOK("声明价值必须大于0");
                        return;
                    }
                }
                #endregion

                #region 声明价值不能超过100万
                //if (DeclareValue > 1000000)
                //{
                //    MsgBox.ShowOK("请输入小于100万的申明价值!");
                //    return;
                //}
                #endregion

                #region 检测单号
                if (isModify == 0 && xmdr == null)
                {
                    //zaj 当输入的运单号不为空时验证
                    if (BillNo.Text.Trim() != "")
                    {

                        int billno_state = 4;
                        string billno_new = BillNo.Text.Trim();
                        List<SqlPara> list = new List<SqlPara>();
                        list.Add(new SqlPara("billno", BillNo.Text.Trim()));
                        list.Add(new SqlPara("webid", begWeb.Text.Trim()));
                        SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_QSP_GET_BILL_EXIST", list);
                        DataSet dsBill = SqlHelper.GetDataSet(sps);
                        if (dsBill != null && dsBill.Tables.Count > 0 && dsBill.Tables[0].Rows.Count > 0)
                        {
                            billno_state = ConvertType.ToInt32(dsBill.Tables[0].Rows[0]["state"]);
                        }
                        if (billno_state != 1)
                        {
                            string msg = "";
                            if (billno_state == 4)
                            {
                                msg = "本网点中没有单号为 " + billno_new + " 的托运单\r\n\r\n或者没有在系统中为本网点分配单号 " + billno_new + " ,请确认!";
                            }
                            else if (billno_state == 2)
                            {
                                msg = "本网点中的托运单号 " + billno_new + " 已经使用,请确认!";
                            }
                            else if (billno_state == 3)
                            {
                                msg = "本网点中的托运单号 " + billno_new + " 已经作废,不能重复使用,请确认!";
                            }
                            if (msg != "")
                            {
                                MsgBox.ShowOK(msg);
                                BillNo.Focus();
                                BillNo.Select(0, billno_new.Length);
                                return;
                            }
                        }
                    }
                }
                #endregion

                #region 应收账款检测

                if (MonthPay_s > 0 && CommonClass.UserInfo.IsLimitMonthPay == true)
                {
                    bool isnochcke = false;

                    if (CommonClass.Arg.ContractCheck.Contains(CommonClass.UserInfo.WebName))
                    {
                        isnochcke = true;
                    }
                    //hj20181101
                    DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_COMPANY_KD"));
                    string gsControl = ds.Tables[0].Rows[0]["gsControl"].ToString();
                    if (gsControl == "是")
                    {
                        isnochcke = false;
                    }
                    if (isnochcke == false) //增加判断，跳过青岛野狐营业部,安庆金鹏行营业部,合肥金鹏行营业部,深圳金鹏行营业部的月结客户判断,吴沐鸿要求新增三个网点不用月结客户监控2016-12-31
                    {
                        List<SqlPara> list = new List<SqlPara>();
                        list.Add(new SqlPara("ConsignorCompany", ConsignorCompany.Text.Trim()));
                        list.Add(new SqlPara("ConsignorName", ConsignorName.Text.Trim()));
                        SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_CustContractCheck", list);
                        DataSet dsCust = SqlHelper.GetDataSet(sps);
                        if (dsCust == null || dsCust.Tables.Count == 0 || dsCust.Tables[0].Rows.Count == 0)
                        {
                            MsgBox.ShowOK("发货单位[" + ConsignorCompany.Text.Trim() + "]不是月结客户，不能开月结！");
                            return;
                        }
                        int remaindays = ConvertType.ToInt32(dsCust.Tables[0].Rows[0]["remaindays"].ToString()); //月结剩余天数
                        decimal remainacc = ConvertType.ToDecimal(dsCust.Tables[0].Rows[0]["remainacc"].ToString()); //月结剩余额度
                        string PayCycle = ConvertType.ToString(dsCust.Tables[0].Rows[0]["PayCycle"]);//结款周期

                        if (PayCycle == "")
                        {
                            MsgBox.ShowOK(string.Format("没有为[{0}]设定结款周期，请联系财务人员设置后再保存!", ConsignorCompany.Text.Trim()));
                            return;
                        }
                        if (PayCycle == "七天结" || PayCycle == "半月结")
                        {
                            MsgBox.ShowOK(string.Format("发货单位[{0}]是[{1}]，不能开月结付款方式!", ConsignorCompany.Text.Trim(), PayCycle));
                            return;
                        }

                        if (remaindays < 0)
                        {
                            MsgBox.ShowOK("发货单位：" + ConsignorCompany.Text.Trim() + " 欠款 " + Math.Abs(remaindays) + "天，不能开单！");
                            return;
                        }
                        else if (remaindays <= 5)
                        {
                            MsgBox.ShowOK("发货单位：" + ConsignorCompany.Text.Trim() + " 还有" + Math.Abs(remaindays) + "天欠款到期，请及时通知发货人付款！");
                        }

                        if (isModify > 0) remainacc += (MonthPay_s);

                        if (remainacc <= 0)
                        {
                            MsgBox.ShowOK("发货单位：" + ConsignorCompany.Text.Trim() + " 欠款额度超过合同额度，不能开单！");
                            return;
                        }
                    }
                }

                #endregion

                #region 查询时效
                //string Province = "", City = "", Area = "";
                //Province = ReceivProvince.Text.Trim();
                //City = ReceivCity.Text.Trim();
                //Area = ReceivArea.Text.Trim();

                //List<SqlPara> list_Fee = new List<SqlPara>();
                //list_Fee.Add(new SqlPara("StartSite", StartSite.Text.Trim()));
                //list_Fee.Add(new SqlPara("Province", Province));
                //list_Fee.Add(new SqlPara("City", City));
                //list_Fee.Add(new SqlPara("Area", Area));
                //list_Fee.Add(new SqlPara("TransferMode", TransferMode.Text.Trim()));
                //list_Fee.Add(new SqlPara("TransitMode", "中强专线"));
                //list_Fee.Add(new SqlPara("TransferSite", TransferSite.Text.Trim()));
                //SqlParasEntity sps_fee = new SqlParasEntity(OperType.Query, "QSP_GET_BASFREIGHTFEE_KD", list_Fee);
                //DataSet dsFee = SqlHelper.GetDataSet(sps_fee);
                //if (dsFee.Tables[0].Rows.Count > 0)
                //{
                //    int iTempPrescription = 0;
                //    int.TryParse(dsFee.Tables[0].Rows[0]["Prescription"].ToString(), out iTempPrescription);
                //    iPrescription = iTempPrescription;//时效
                //    strLatestDepartTime = dsFee.Tables[0].Rows[0]["LatestDepartTime"].ToString();//最晚发车时间
                //}
                //else
                //{
                //    iPrescription = 0;//时效
                //    strLatestDepartTime = string.Empty;//最晚发车时间
                //}
                #endregion

                #region 检测接货费
                //ZJF20181110
                if (PaymentMode.Text.Trim() != "月结")
                {
                    //if (VehicleNum.Text.Trim() != "" &&
                    //   ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "ReceivFee")) == 0)
                    //{
                    //    MsgBox.ShowOK("关联接货车号后必须填写接货费");
                    //    return;
                    //}
                    //maohui20181226
                    //if (VehicleNum.Text.Trim() == "" &&
                    //   ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "ReceivFee")) != 0)
                    //{
                    //    MsgBox.ShowOK("填写了接货费，请关联接货车号");
                    //    return;
                    //}
                }


                #endregion

                #region 检测月结、回单付客户开单余额与天数

                if (PaymentMode.Text == "回单付" || PaymentMode.Text == "月结")
                {
                    try
                    {
                        List<SqlPara> list = new List<SqlPara>();
                        list.Add(new SqlPara("ConsignorCompany", ConsignorCompany.Text.Trim()));
                        SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SHIPPERMONTH_BYCONSIGN", list);
                        DataSet dsCust = SqlHelper.GetDataSet(sps);
                        if (dsCust.Tables[0].Rows.Count > 0)
                        {
                            int spdays = Convert.ToInt32(dsCust.Tables[0].Rows[0]["surplusDays"]);
                            decimal leftcredit = Convert.ToDecimal(dsCust.Tables[0].Rows[0]["LeftCredit"]);
                            if (spdays <= 10)
                            {
                                if (leftcredit <= 20000)
                                {
                                    MsgBox.ShowOK("此客户剩余额度低于两万，开单天数低于10天");
                                }
                                else
                                {
                                    MsgBox.ShowOK("此客户开单剩余天数为" + spdays + "天");
                                }
                            }
                            if (leftcredit <= 20000)
                            {
                                MsgBox.ShowOK("此客户开单剩余额度低于两万");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MsgBox.ShowException(ex);
                    }

                }


                #endregion

                #region 检测运输方式
                //ZJF20181116

                //if (TransferMode.Text == "司机直送" && TransitMode.Text != "中强整车")
                //{
                //    MsgBox.ShowOK("司机直送必须是中强整车");
                //    return;
                //}
                //maohui20181225
                if (TransitMode.Text.Trim() == "中强整车" && TransferMode.Text.Trim() != "司机直送")
                {
                    MsgBox.ShowOK("开单运输方式选择中强整车，交接方式必须选择司机直送!");
                    return;
                }



                #endregion

                #region 检测加盟账户余额-开单结算费是否满足开单条件
                if (CommonClass.UserInfo.WebRole == "加盟")
                {
                    Decimal total_acc = 0;
                    try
                    {
                        List<SqlPara> list = new List<SqlPara>();
                        SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ACCOUNT_BIllSTATE", list);
                        DataSet ds = SqlHelper.GetDataSet(sps);
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                total_acc += Convert.ToDecimal(ds.Tables[0].Rows[i]["heji"].ToString());
                            }

                        }
                        if ((CommonClass.GetWebAcc(CommonClass.UserInfo.WebName) - Convert.ToDecimal(this.heji.Text) - total_acc) < 0)
                        {
                            MsgBox.ShowOK("账户余额不足，请充值!");
                            return;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        MsgBox.ShowException(ex);
                    }


                }

                #endregion
            

                if (Upd_Num == 1)
                    Savebill_Upd_Apply();
                else
                    Savebill();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void Savebill()
        {
            gridView1.PostEditor();
            gridView2.PostEditor();
            string GoodsType = GridOper.GetRowCellValueString(gridView2, 0, "GoodsType");
            string Varieties = gridView2.GetRowCellValue(0, "Varieties").ToString();
            string Package = gridView2.GetRowCellValue(0, "Package").ToString();
            decimal Num = Convert.ToDecimal(gridView2.GetRowCellValue(0, "Num").ToString() == "" ? "0" : gridView2.GetRowCellValue(0, "Num").ToString());
            decimal FeeWeight = Convert.ToDecimal(gridView2.GetRowCellValue(0, "FeeWeight").ToString() == "" ? "0" : gridView2.GetRowCellValue(0, "FeeWeight").ToString());
            decimal FeeVolume = Convert.ToDecimal(gridView2.GetRowCellValue(0, "FeeVolume").ToString() == "" ? "0" : gridView2.GetRowCellValue(0, "FeeVolume").ToString());

            decimal Weight = Convert.ToDecimal(gridView2.GetRowCellValue(0, "Weight").ToString() == "" ? "0" : gridView2.GetRowCellValue(0, "Weight").ToString());
            decimal Volume = Convert.ToDecimal(gridView2.GetRowCellValue(0, "Volume").ToString() == "" ? "0" : gridView2.GetRowCellValue(0, "Volume").ToString());
            decimal WeightPrice = Convert.ToDecimal(gridView2.GetRowCellValue(0, "WeightPrice").ToString() == "" ? "0" : gridView2.GetRowCellValue(0, "WeightPrice").ToString());
            decimal VolumePrice = Convert.ToDecimal(gridView2.GetRowCellValue(0, "VolumePrice").ToString() == "" ? "0" : gridView2.GetRowCellValue(0, "VolumePrice").ToString());
            string FeeType = gridView2.GetRowCellValue(0, "FeeType").ToString();
            decimal Freight = Convert.ToDecimal(gridView2.GetRowCellValue(0, "Freight").ToString() == "" ? "0" : gridView2.GetRowCellValue(0, "Freight").ToString());

            decimal DeliFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "DeliFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "DeliFee").ToString());
            decimal DeclareValue = Convert.ToDecimal(gridView1.GetRowCellValue(0, "DeclareValue").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "DeclareValue").ToString());
            decimal SupportValue = Convert.ToDecimal(gridView1.GetRowCellValue(0, "SupportValue").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "SupportValue").ToString());
            decimal Tax = Convert.ToDecimal(gridView1.GetRowCellValue(0, "Tax").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "Tax").ToString());
            decimal InformationFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "InformationFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "InformationFee").ToString());

            decimal Expense = Convert.ToDecimal(gridView1.GetRowCellValue(0, "Expense").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "Expense").ToString());
            decimal DiscountTransfer = Convert.ToDecimal(gridView1.GetRowCellValue(0, "DiscountTransfer").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "DiscountTransfer").ToString());
            decimal CollectionPay = Convert.ToDecimal(gridView1.GetRowCellValue(0, "CollectionPay").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "CollectionPay").ToString());
            decimal AgentFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "AgentFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "AgentFee").ToString());
            decimal FuelFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "FuelFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "FuelFee").ToString());

            decimal UpstairFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "UpstairFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "UpstairFee").ToString());
            decimal HandleFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "HandleFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "HandleFee").ToString());
            decimal ForkliftFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "ForkliftFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "ForkliftFee").ToString());
            decimal StorageFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "StorageFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "StorageFee").ToString());
            decimal CustomsFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "CustomsFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "CustomsFee").ToString());
            decimal packagFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "packagFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "packagFee").ToString());

            decimal MatPay = Convert.ToDecimal(gridView1.GetRowCellValue(0, "MatPay").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "MatPay").ToString());//HJ20180911
            decimal FrameFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "FrameFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "FrameFee").ToString());
            decimal ChangeFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "ChangeFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "ChangeFee").ToString());
            decimal OtherFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "OtherFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "OtherFee").ToString());
            decimal NoticeFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "NoticeFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "NoticeFee").ToString());
            decimal ReceiptFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "ReceiptFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "ReceiptFee").ToString());
            decimal ReceivFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "ReceivFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "ReceivFee").ToString());

            decimal WarehouseFee = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "WarehouseFee"));
            decimal OptWeight = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "OptWeight"));

            string VarietiesStr = "", PackageStr = "", NumStr = "", FeeWeightStr = "", FeeVolumeStr = "",
                WeightStr = "", VolumeStr = "", WeightPriceStr = "", VolumePriceStr = "", FeeTypeStr = "", FreightStr = "";

            decimal ActualFreight = ConvertType.ToDecimal(PaymentAmout.Text) - DiscountTransfer - CollectionPay - MatPay;
            //int type = radioGroup1.SelectedIndex;
            #region 获取配载目的网点
            string LoadEweb = "";
            DataRow[] drweb = CommonClass.dsWeb.Tables[0].Select("StartSiteRange like '%" + StartSite.Text + "%' and SiteName='" + TransferSite.Text + "' ");
            if (drweb.Length > 0)
            {
                LoadEweb = drweb[0]["WebName"].ToString();
            }
            #endregion

            #region 获取库位
            //string StorageLocation = "";
            //string filter = "MiddleProvince='{0}' and MiddleCity='{1}' and MiddleArea='{2}' and MiddleStreet='{3}'";
            //DataRow[] drm = CommonClass.dsMiddleSite.Tables[0].Select(string.Format(filter, ReceivProvince.Text.Trim(), ReceivCity.Text.Trim(), ReceivArea.Text.Trim(), ReceivStreet.Text.Trim()));
            //if (drm.Length > 0)
            //{
            //    string trans = TransferMode.Text.Trim();
            //    StorageLocation = trans == "自提" ? drm[0]["FetchStorageLoca"].ToString() : drm[0]["SendStorageLoca"].ToString();
            //}
            #endregion

            for (int i = 1; i < RowCount; i++)
            {
                if (gridView2.GetRowCellValue(i, "Varieties").ToString() != "")
                {
                    VarietiesStr += gridView2.GetRowCellValue(i, "Varieties").ToString() + "@";
                    PackageStr += gridView2.GetRowCellValue(i, "Package").ToString() + "@";
                    NumStr += ConvertType.ToInt32(gridView2.GetRowCellValue(i, "Num")) + "@";
                    FeeWeightStr += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeWeight")) + "@";
                    FeeVolumeStr += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeVolume")) + "@";

                    WeightStr += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "Weight")) + "@";
                    VolumeStr += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "Volume")) + "@";
                    WeightPriceStr += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "WeightPrice")) + "@";
                    VolumePriceStr += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "VolumePrice")) + "@";
                    FeeTypeStr += gridView2.GetRowCellValue(i, "FeeType").ToString() + "@";
                    FreightStr += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "Freight")) + "@";
                    //合计到第一行
                    Num += ConvertType.ToInt32(gridView2.GetRowCellValue(i, "Num"));
                    FeeWeight += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeWeight"));
                    FeeVolume += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeVolume"));
                    Weight += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "Weight"));
                    Volume += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "Volume"));
                    Freight += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "Freight"));
                }
            }
            #region 如果开单人员没有输入运单则系统自动生成运单 zaj 2017-11-10，LD 2017-11-27
            string No = "";
            //if (BillNo.Text.Trim() == "")
            //{
            //    DataSet dsBillNo = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BillNo", null));

            //    if (dsBillNo != null || dsBillNo.Tables[0].Rows.Count > 0)
            //    {
            //        No = dsBillNo.Tables[0].Rows[0]["BillNo"].ToString();
            //    }
            //}
            //else
            //{
            //    No = BillNo.Text.Trim();
            //}
            //if (No == "")
            //{
            //    MsgBox.ShowOK("生成运单失败，请重新保存！");
            //    return;
            //}
            //zaj 2018-1-13
            //if (isCompany == 1) 
            if (CommonClass.UserInfo.IsAutoBill == true && isModify == 0)
            {
                No = GetBillNoForText(); //加载运单号
                if (string.IsNullOrEmpty(No))
                {
                    //MsgBox.ShowOK("加载运单号失败，请检查网络后重新保存！");
                    return;
                }
            }
            else
            {
                No = this.BillNo.Text.Trim(); //加载运单号
                if (string.IsNullOrEmpty(No))
                {
                    MsgBox.ShowOK("必须输入运单号！");
                    return;
                }
            }
            #endregion

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillId", 0));
            //list.Add(new SqlPara("BillNo", BillNo.Text.Trim()));
            list.Add(new SqlPara("BillNo", No));
            list.Add(new SqlPara("VehicleNo", VehicleNo.Text.Trim()));
            list.Add(new SqlPara("BillDate", System.DateTime.Now));
            //zb20190527
            if (CommonClass.UserInfo.companyid == "485" && TransitMode.Text.Trim() == "川胜项目")
            {
                list.Add(new SqlPara("BillState", 0));
            }
            else
            {
                list.Add(new SqlPara("BillState", 0));
            }
            list.Add(new SqlPara("StartSite", StartSite.Text.Trim()));
            list.Add(new SqlPara("TransferMode", TransferMode.Text.Trim()));
            list.Add(new SqlPara("DestinationSite", DestinationSite.Text.Trim()));
            list.Add(new SqlPara("TransferSite", TransferSite.Text.Trim()));
            list.Add(new SqlPara("TransitMode", TransitMode.Text.Trim()));
            list.Add(new SqlPara("CusOderNo", CusOderNo.Text.Trim()));

            list.Add(new SqlPara("ConsigneeCellPhone", NoticeState.Checked ? "888888" : ConsigneeCellPhone.Text.Trim()));
            list.Add(new SqlPara("ConsigneePhone", NoticeState.Checked ? "888888" : ConsigneePhone.Text.Trim()));
            list.Add(new SqlPara("ConsigneeName", NoticeState.Checked ? "888888" : ConsigneeName.Text.Trim()));
            list.Add(new SqlPara("ConsigneeCompany", NoticeState.Checked ? "888888" : ConsigneeCompany.Text.Trim()));
            //list.Add(new SqlPara("PickGoodsSite", PickGoodsSite.Text.Trim()));
            list.Add(new SqlPara("PickGoodsSite", NewPickGoodsSite.Trim() == "" ? PickGoodsSite.Text.Trim() : NewPickGoodsSite));//zaj20180710

            list.Add(new SqlPara("ReceivProvince", ReceivProvince1));
            list.Add(new SqlPara("ReceivCity", ReceivCity1));
            list.Add(new SqlPara("ReceivArea", ReceivArea1));
            list.Add(new SqlPara("ReceivStreet", ReceivStreet1));
            if (CommonClass.UserInfo.companyid != "239")
            {
                //list.Add(new SqlPara("ReceivAddress", Province.Text.Trim() + City.Text.Trim() + Address.Text.Trim()));
                list.Add(new SqlPara("ReceivAddress", Address.Text.Trim()));
            }
            else
            {
                list.Add(new SqlPara("ReceivAddress", Address.Text.Trim()));
            }

            list.Add(new SqlPara("ConsignorCellPhone", ConsignorCellPhone.Text.Trim()));
            list.Add(new SqlPara("ConsignorPhone", ConsignorPhone.Text.Trim()));
            list.Add(new SqlPara("ConsignorName", ConsignorName.Text.Trim()));
            list.Add(new SqlPara("ConsignorCompany", ConsignorCompany.Text.Trim()));
            //list.Add(new SqlPara("ReceivMode", ReceivOrderNo.Text.Trim()));
            list.Add(new SqlPara("ValuationType", ValuationType.Text.Trim()));
            list.Add(new SqlPara("ReceivOrderNo", ReceivOrderNo.Text.Trim()));
            list.Add(new SqlPara("Salesman", Salesman.Text.Trim()));
            list.Add(new SqlPara("AlienGoods", AlienGoods.Checked ? 1 : 0));
            list.Add(new SqlPara("GoodsVoucher", GoodsVoucher.Checked ? 1 : 0));
            list.Add(new SqlPara("PreciousGoods", PreciousGoods.Checked ? 1 : 0)); //luohui
            list.Add(new SqlPara("NoticeState", NoticeState.Checked ? 1 : 0));
            list.Add(new SqlPara("GoodsType", GoodsType));
            list.Add(new SqlPara("Varieties", Varieties));
            list.Add(new SqlPara("Package", Package));
            list.Add(new SqlPara("Num", Num));
            list.Add(new SqlPara("LeftNum", Num));
            list.Add(new SqlPara("FeeWeight", FeeWeight));
            list.Add(new SqlPara("FeeVolume", FeeVolume));
            list.Add(new SqlPara("Weight", Weight));
            list.Add(new SqlPara("Volume", Volume));

            list.Add(new SqlPara("WeightPrice", WeightPrice));
            list.Add(new SqlPara("VolumePrice", VolumePrice));
            list.Add(new SqlPara("FeeType", FeeType));
            list.Add(new SqlPara("Freight", Freight));
            list.Add(new SqlPara("DeliFee", DeliFee));

            list.Add(new SqlPara("ReceivFee", ReceivFee));
            list.Add(new SqlPara("DeclareValue", DeclareValue));
            list.Add(new SqlPara("SupportValue", SupportValue));
            list.Add(new SqlPara("Rate", 0));
            list.Add(new SqlPara("Tax", Tax));

            list.Add(new SqlPara("InformationFee", InformationFee));
            list.Add(new SqlPara("Expense", Expense));
            list.Add(new SqlPara("NoticeFee", NoticeFee));
            list.Add(new SqlPara("DiscountTransfer", DiscountTransfer));
            list.Add(new SqlPara("CollectionPay", CollectionPay));

            list.Add(new SqlPara("AgentFee", AgentFee));
            list.Add(new SqlPara("FuelFee", FuelFee));
            list.Add(new SqlPara("UpstairFee", UpstairFee));
            list.Add(new SqlPara("HandleFee", HandleFee));
            list.Add(new SqlPara("ForkliftFee", ForkliftFee));

            list.Add(new SqlPara("StorageFee", StorageFee));
            list.Add(new SqlPara("CustomsFee", CustomsFee));
            list.Add(new SqlPara("packagFee", packagFee));
            list.Add(new SqlPara("FrameFee", FrameFee));
            list.Add(new SqlPara("ChangeFee", ChangeFee));
            list.Add(new SqlPara("MatPay", MatPay));//HJ20180911
            list.Add(new SqlPara("OtherFee", OtherFee));
            list.Add(new SqlPara("IsInvoice", IsInvoice.Checked ? 1 : 0));
            list.Add(new SqlPara("ReceiptFee", ReceiptFee));
            list.Add(new SqlPara("ReceiptCondition", ReceiptCondition.Text.Trim()));
            list.Add(new SqlPara("FreightAmount", 0));
            list.Add(new SqlPara("ActualFreight", ActualFreight));


            list.Add(new SqlPara("PaymentMode", PaymentMode.Text.Trim()));
            list.Add(new SqlPara("PaymentAmout", PaymentAmout.Text.Trim() == "" ? "0" : PaymentAmout.Text.Trim()));

            list.Add(new SqlPara("NowPay", NowPay.Text.Trim() == "" ? "0" : NowPay.Text.Trim()));
            list.Add(new SqlPara("FetchPay", FetchPay.Text.Trim() == "" ? "0" : FetchPay.Text.Trim()));
            list.Add(new SqlPara("MonthPay", MonthPay.Text.Trim() == "" ? "0" : MonthPay.Text.Trim()));
            list.Add(new SqlPara("ShortOwePay", ShortOwePay.Text.Trim() == "" ? "0" : ShortOwePay.Text.Trim()));
            list.Add(new SqlPara("BefArrivalPay", BefArrivalPay.Text.Trim() == "" ? "0" : BefArrivalPay.Text.Trim()));
            //zaj 加上回单付 2017-11-22
            list.Add(new SqlPara("ReceiptPay", ReceiptPay.Text.Trim() == "" ? "0" : ReceiptPay.Text.Trim()));
            list.Add(new SqlPara("OwePay", OwePay.Text.Trim() == "" ? "0" : OwePay.Text.Trim()));
            list.Add(new SqlPara("BillRemark", BillRemark.Text.Trim()));
            list.Add(new SqlPara("WebPlatform", WebPlatform.Text.Trim()));
            list.Add(new SqlPara("WebBillNo", WebBillNo.Text.Trim()));
            list.Add(new SqlPara("DisTranName", DisTranName.Text.Trim()));
            list.Add(new SqlPara("DisTranBank", DisTranBank.Text.Trim()));
            list.Add(new SqlPara("DisTranAccount", DisTranAccount.Text.Trim()));
            list.Add(new SqlPara("CollectionName", CollectionName.Text.Trim()));
            list.Add(new SqlPara("CollectionBank", CollectionBank.Text.Trim()));
            list.Add(new SqlPara("CollectionAccount", CollectionAccount.Text.Trim()));
            list.Add(new SqlPara("CauseName", CauseName.Text));
            list.Add(new SqlPara("AreaName", AreaName.Text));
            list.Add(new SqlPara("DepName", DepName.Text));
            list.Add(new SqlPara("OtherTotalFee", "0"));
            list.Add(new SqlPara("BillTotalFee", "0"));
            list.Add(new SqlPara("DisTranBranch", DisTranBranch.Text.Trim()));
            list.Add(new SqlPara("CollectionBranch", CollectionBranch.Text.Trim()));
            list.Add(new SqlPara("BillUserId", CommonClass.UserInfo.UserName));
            list.Add(new SqlPara("BillMan", BillMan.Text));
            list.Add(new SqlPara("DeviceSource", "0"));
            list.Add(new SqlPara("begWeb", begWeb.Text));
            list.Add(new SqlPara("isModify", isModify));

            list.Add(new SqlPara("WarehouseFee", WarehouseFee));

            list.Add(new SqlPara("VarietiesStr", VarietiesStr));
            list.Add(new SqlPara("PackageStr", PackageStr));
            list.Add(new SqlPara("NumStr", NumStr));
            list.Add(new SqlPara("FeeWeightStr", FeeWeightStr));
            list.Add(new SqlPara("FeeVolumeStr", FeeVolumeStr));

            list.Add(new SqlPara("WeightStr", WeightStr));
            list.Add(new SqlPara("VolumeStr", VolumeStr));
            list.Add(new SqlPara("WeightPriceStr", WeightPriceStr));
            list.Add(new SqlPara("VolumePriceStr", VolumePriceStr));
            list.Add(new SqlPara("FeeTypeStr", FeeTypeStr));
            list.Add(new SqlPara("FreightStr", FreightStr));

            list.Add(new SqlPara("IsReceiptFee", IsReceiptFee.Checked ? 1 : 0));
            list.Add(new SqlPara("IsSupportValue", IsSupportValue.Checked ? 1 : 0));
            list.Add(new SqlPara("IsAgentFee", IsAgentFee.Checked ? 1 : 0));
            list.Add(new SqlPara("IsPackagFee", IsPackagFee.Checked ? 1 : 0));
            list.Add(new SqlPara("IsOtherFee", IsOtherFee.Checked ? 1 : 0));

            list.Add(new SqlPara("IsHandleFee", IsHandleFee.Checked ? 1 : 0));
            list.Add(new SqlPara("IsStorageFee", IsStorageFee.Checked ? 1 : 0));
            list.Add(new SqlPara("IsWarehouseFee", IsWarehouseFee.Checked ? 1 : 0));
            list.Add(new SqlPara("IsForkliftFee", IsForkliftFee.Checked ? 1 : 0));
            list.Add(new SqlPara("IsChangeFee", IsChangeFee.Checked ? 1 : 0));

            list.Add(new SqlPara("IsUpstairFee", IsUpstairFee.Checked ? 1 : 0));
            list.Add(new SqlPara("IsCustomsFee", IsCustomsFee.Checked ? 1 : 0));
            list.Add(new SqlPara("IsFrameFee", IsFrameFee.Checked ? 1 : 0));
            list.Add(new SqlPara("IsMatPay", IsMatPay.Checked ? 1 : 0));//HJ20180911
            list.Add(new SqlPara("IsRebates", IsRebates.Checked ? 1 : 0));//HJ20180911回扣已返

            list.Add(new SqlPara("MainlineFee", ConvertType.ToDecimal(MainlineFee.Text.Trim())));
            list.Add(new SqlPara("DeliveryFee", ConvertType.ToDecimal(DeliveryFee.Text.Trim())));
            //list.Add(new SqlPara("DeliveryFee", NewDeliveryFee));//3吨八方业务计算出的送货费zaj2018-7-10
            list.Add(new SqlPara("TransferFee", ConvertType.ToDecimal(TransferFee.Text.Trim())));
            list.Add(new SqlPara("DepartureOptFee", ConvertType.ToDecimal(DepartureOptFee.Text.Trim())));
            list.Add(new SqlPara("TerminalOptFee", ConvertType.ToDecimal(TerminalOptFee.Text.Trim())));
            list.Add(new SqlPara("TerminalAllotFee", ConvertType.ToDecimal(TerminalAllotFee.Text.Trim())));

            list.Add(new SqlPara("ReceiptFee_C", ConvertType.ToDecimal(ReceiptFee_C.Text.Trim())));
            list.Add(new SqlPara("NoticeFee_C", ConvertType.ToDecimal(NoticeFee_C.Text.Trim())));

            list.Add(new SqlPara("SupportValue_C", ConvertType.ToDecimal(SupportValue_C.Text.Trim())));
            list.Add(new SqlPara("AgentFee_C", ConvertType.ToDecimal(AgentFee_C.Text.Trim())));
            list.Add(new SqlPara("PackagFee_C", ConvertType.ToDecimal(PackagFee_C.Text.Trim())));
            list.Add(new SqlPara("OtherFee_C", ConvertType.ToDecimal(OtherFee_C.Text.Trim())));
            list.Add(new SqlPara("HandleFee_C", ConvertType.ToDecimal(HandleFee_C.Text.Trim())));

            list.Add(new SqlPara("StorageFee_C", ConvertType.ToDecimal(StorageFee_C.Text.Trim())));
            list.Add(new SqlPara("WarehouseFee_C", ConvertType.ToDecimal(WarehouseFee_C.Text.Trim())));
            list.Add(new SqlPara("ForkliftFee_C", ConvertType.ToDecimal(ForkliftFee_C.Text.Trim())));
            list.Add(new SqlPara("Tax_C", ConvertType.ToDecimal(Tax_C.Text.Trim())));
            list.Add(new SqlPara("ChangeFee_C", ConvertType.ToDecimal(ChangeFee_C.Text.Trim())));

            list.Add(new SqlPara("UpstairFee_C", ConvertType.ToDecimal(UpstairFee_C.Text.Trim())));
            list.Add(new SqlPara("CustomsFee_C", ConvertType.ToDecimal(CustomsFee_C.Text.Trim())));
            list.Add(new SqlPara("FrameFee_C", ConvertType.ToDecimal(FrameFee_C.Text.Trim())));
            list.Add(new SqlPara("Expense_C", ConvertType.ToDecimal(Expense_C.Text.Trim())));
            list.Add(new SqlPara("FuelFee_C", ConvertType.ToDecimal(FuelFee_C.Text.Trim())));
            list.Add(new SqlPara("InformationFee_C", ConvertType.ToDecimal(InformationFee_C.Text.Trim())));

            list.Add(new SqlPara("ConsigneeCellPhone_K", ConsigneeCellPhone.Text.Trim()));
            list.Add(new SqlPara("ConsigneePhone_K", ConsigneePhone.Text.Trim()));
            list.Add(new SqlPara("ConsigneeName_K", ConsigneeName.Text.Trim()));
            list.Add(new SqlPara("ConsigneeCompany_K", ConsigneeCompany.Text.Trim()));

            list.Add(new SqlPara("OperationWeight", ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "OperationWeight"))));

            list.Add(new SqlPara("IsCenterBack", IsCenterBack.Text));
            //luohui
            list.Add(new SqlPara("IsViewTrace", IsViewTraceFee.Checked ? 1 : 0));
            list.Add(new SqlPara("IsSortFee", IsSortFee.Checked ? 1 : 0));

            list.Add(new SqlPara("LoadEweb", LoadEweb));
            list.Add(new SqlPara("OptWeight", OptWeight));
            list.Add(new SqlPara("MarketSuper", MarketSuper.Checked ? 1 : 0)); //luohui


            list.Add(new SqlPara("Type", radioGroup1.SelectedIndex));//0是专线，1是落地配


            //list.Add(new SqlPara("StorageLocation", StorageLocation));

            //list.Add(new SqlPara("ProRemark", ProRemark));
            //list.Add(new SqlPara("Prescription", iPrescription));//时效字段
            //list.Add(new SqlPara("LatestDepartTime", strLatestDepartTime));//最晚发车时间

            //hj新增长宽高尺寸 20180925
            decimal Length1 = 0;
            decimal Width1 = 0;
            decimal Height1 = 0;
            string Varieties1 = "";
            string Package1 = "";

            decimal Length2 = 0;
            decimal Width2 = 0;
            decimal Height2 = 0;
            string Varieties2 = "";
            string Package2 = "";

            if (gridView2.GetRowCellValue(0, "length").ToString() == "" || gridView2.GetRowCellValue(0, "length").ToString() == null)
            {
                gridView2.SetRowCellValue(0, "length", 0);
            }
            if (gridView2.GetRowCellValue(0, "width").ToString() == "" || gridView2.GetRowCellValue(0, "width").ToString() == null)
            {
                gridView2.SetRowCellValue(0, "width", 0);
            }
            if (gridView2.GetRowCellValue(0, "height").ToString() == "" || gridView2.GetRowCellValue(0, "height").ToString() == null)
            {
                gridView2.SetRowCellValue(0, "height", 0);
            }

            if (gridView2.GetRowCellValue(1, "length").ToString() == "" || gridView2.GetRowCellValue(1, "length").ToString() == null)
            {
                gridView2.SetRowCellValue(1, "length", 0);
            }
            if (gridView2.GetRowCellValue(1, "width").ToString() == "" || gridView2.GetRowCellValue(1, "width").ToString() == null)
            {
                gridView2.SetRowCellValue(1, "width", 0);
            }
            if (gridView2.GetRowCellValue(1, "height").ToString() == "" || gridView2.GetRowCellValue(1, "height").ToString() == null)
            {
                gridView2.SetRowCellValue(1, "height", 0);
            }
            if (gridView2.GetRowCellValue(1, "Varieties").ToString().Trim() == "" || gridView2.GetRowCellValue(1, "Varieties").ToString().Trim() == null || gridView2.GetRowCellValue(1, "Num").ToString().Trim() == "" || gridView2.GetRowCellValue(1, "Num").ToString().Trim() == null)
            {
            }
            else
            {
                Length2 = Convert.ToDecimal(gridView2.GetRowCellValue(1, "length"));
                Width2 = Convert.ToDecimal(gridView2.GetRowCellValue(1, "width"));
                Height2 = Convert.ToDecimal(gridView2.GetRowCellValue(1, "height"));
                Varieties2 = gridView2.GetRowCellValue(1, "Varieties").ToString();
                Package2 = gridView2.GetRowCellValue(1, "Package").ToString();
                list.Add(new SqlPara("Length2", Length2));
                list.Add(new SqlPara("Width2", Width2));
                list.Add(new SqlPara("Height2", Height2));
                list.Add(new SqlPara("Varieties2", Varieties2));
                list.Add(new SqlPara("Package2", Package2));
            }
            Length1 = Convert.ToDecimal(gridView2.GetRowCellValue(0, "length"));
            Width1 = Convert.ToDecimal(gridView2.GetRowCellValue(0, "width"));
            Height1 = Convert.ToDecimal(gridView2.GetRowCellValue(0, "height"));
            Varieties1 = gridView2.GetRowCellValue(0, "Varieties").ToString();
            Package1 = gridView2.GetRowCellValue(0, "Package").ToString();
            list.Add(new SqlPara("Length1", Length1));
            list.Add(new SqlPara("Width1", Width1));
            list.Add(new SqlPara("Height1", Height1));
            list.Add(new SqlPara("Varieties1", Varieties1));
            list.Add(new SqlPara("Package1", Package1));


            SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_WAYBILL_GX_KT", list);
            if (SqlHelper.ExecteNonQuery(sps) > 0)
            {
                MsgBox.ShowOK("操作成功，运单号：" + No);
                //hj20180925 
                gridView2.SetRowCellValue(0, "length", 0);
                gridView2.SetRowCellValue(0, "width", 0);
                gridView2.SetRowCellValue(0, "height", 0);
                gridView2.SetRowCellValue(1, "length", 0);
                gridView2.SetRowCellValue(1, "width", 0);
                gridView2.SetRowCellValue(1, "height", 0);
                string billNo = BillNo.Text.Trim();
                TransferMode.Text = "";
                DriverName.Text = "";
                heji.Text = "";
                #region 保存条码  另外服务自动加条码,程序无须再加
                //Thread th = new Thread(() =>
                //{
                //    try
                //    {
                //        SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_BARCODE", new List<SqlPara> { new SqlPara("BillNo", billNo) }));
                //    }
                //    catch { }
                //});
                //th.IsBackground = true;
                //th.Start();
                #endregion

                if (checkEdit1.Checked)
                {
                    //string name = CommonClass.UserInfo.IsAutoBill == false ? "托运单" : "托运单(打印条码)";
                    string name = "";
                    if (CommonClass.UserInfo.BookNote == "")
                    {
                        name = CommonClass.UserInfo.IsAutoBill == false ? "托运单" : "托运单(打印条码)";
                    }
                    else
                    {
                        name = CommonClass.UserInfo.BookNote;
                    }
                    frmRuiLangService.Print(name, SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTBIll_TX", new List<SqlPara> { new SqlPara("BillNo", No) })));
                }
                if (checkEdit2.Checked && rpt != null)
                {
                    DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTLABEL_DEV2", new List<SqlPara> { new SqlPara("BillNo", No) }));
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        MsgBox.ShowOK("没有找到要打印的运单,请稍后再试");
                        return;
                    }

                    frmPrintLabelDev fpld = new frmPrintLabelDev(ds.Tables[0]);
                    fpld.rpt = rpt;
                    fpld.ShowDialog();

                    /*锐浪打印
                    frmPrintLabel fpl = new frmPrintLabel(billNo, SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTLABEL", new List<SqlPara> { new SqlPara("BillNo", billNo) })));
                    fpl.ShowDialog();
                     */
                }
                if (checkEdit3.Checked)
                {
                    if (CommonClass.UserInfo.companyid == "240")
                    {
                        frmRuiLangService.Print("富成达信封", SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTENVELOPE", new List<SqlPara> { new SqlPara("BillNo", No) })));
                    }
                    if (CommonClass.UserInfo.companyid == "159")
                    {
                        frmRuiLangService.Print("金邦物流信封", SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTENVELOPE", new List<SqlPara> { new SqlPara("BillNo", No) })));
                    }
                    if (CommonClass.UserInfo.companyid == "167")  //maohui20180630
                    {
                        frmRuiLangService.Print("佳安信封", SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTENVELOPE", new List<SqlPara> { new SqlPara("BillNo", No) })));
                    }
                    if (CommonClass.UserInfo.companyid == "155")  //hj20180705
                    {
                        frmRuiLangService.Print("东尚信封", SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTENVELOPE", new List<SqlPara> { new SqlPara("BillNo", No) })));
                    }
                    if (CommonClass.UserInfo.companyid == "210")
                    {
                        frmRuiLangService.Print("苏州复兴信封", SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTENVELOPE", new List<SqlPara> { new SqlPara("BillNo", No) })));
                    }    //HZ20180829
                    if (CommonClass.UserInfo.companyid == "216")
                    {
                        frmRuiLangService.Print("上海天陆物流信封", SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTENVELOPE", new List<SqlPara> { new SqlPara("BillNo", No) })));
                    }    //HZ20180829
                    if (CommonClass.UserInfo.companyid == "231")
                    {
                        frmRuiLangService.Print("江苏远中信封", SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTENVELOPE", new List<SqlPara> { new SqlPara("BillNo", No) })));
                    }    //HZ20180829
                    if (CommonClass.UserInfo.companyid == "239")
                    {
                        frmRuiLangService.Print("可通信封", SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTENVELOPE", new List<SqlPara> { new SqlPara("BillNo", No) })));
                    }
                    //zb20190603
                    if (CommonClass.UserInfo.companyid == "485")
                    {
                        frmRuiLangService.Print("川胜集团信封", SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTENVELOPE", new List<SqlPara> { new SqlPara("BillNo", billNo) })));
                    }


                    if (CommonClass.UserInfo.companyid != "159" && CommonClass.UserInfo.companyid != "240" && CommonClass.UserInfo.companyid != "167" && CommonClass.UserInfo.companyid != "155" && CommonClass.UserInfo.companyid != "210" && CommonClass.UserInfo.companyid != "216" && CommonClass.UserInfo.companyid != "231" && CommonClass.UserInfo.companyid != "239" && CommonClass.UserInfo.companyid != "485")
                    {
                        frmRuiLangService.Print("信封", SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTENVELOPE", new List<SqlPara> { new SqlPara("BillNo", No) })));
                    }
                }
                if (xmdr != null)
                {
                    xmbillno = billNo;
                    xmdr = null;
                    this.Close();
                }

                //打印回扣凭证 tuxin20181023
                if (HKPZ.Checked)
                {
                    SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTVOUCHERS", new List<SqlPara> { new SqlPara("BillNo", No) });
                    DataSet ds = SqlHelper.GetDataSet(sps1);
                    ds.Tables[0].Columns.Add("NowCompany");
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        row["NowCompany"] = CommonClass.UserInfo.gsqc;
                    }
                    frmRuiLangService.Print("回扣凭证", ds);
                }


                if (alidr != null)
                {
                    alibillno = billNo;
                    this.Close();
                }
                //如果是修改运单打开的页面点保存之后将此页面关闭 zaj 2018-3-1
                if (isModify == 1)
                {
                    this.Close();
                }
                clear();

                ////根据网点名称加载运单号，条件：公司被授权允许自动加载运单号情况下LD
                //if (isModify == 0)
                //{
                //    GetBillNoForText();
                //}
                //验证公司权限
                if (isModify == 0)
                {
                    //zaj 2017-1-13
                    //int OldIsCompany = isCompany;//记录公司权限旧值
                    //ValiteCompanyPower();
                    //if (isCompany == 1)
                    //{
                    //    this.BillNo.Enabled = false;
                    //    if (isCompany != OldIsCompany)
                    //    {
                    //        MsgBox.ShowOK("根据上属公司权限已转为自动生成运单号.");
                    //    }
                    //}
                    //else if (isCompany != OldIsCompany)
                    //{
                    //    this.BillNo.Enabled = true;
                    //    this.BillNo.Focus();
                    //    MsgBox.ShowOK("根据上属公司权限已转为手动输入运单号.");
                    //}
                }
            }
        }

        private void gridControl8_DoubleClick(object sender, EventArgs e)
        {

            setCus();
        }

        private void setCus()
        {
            try
            {
                int rows = gridView21.FocusedRowHandle;
                if (rows < 0) return;

                ConsignorCompany.EditValueChanging -= new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsignorCompany_EditValueChanging);
                ConsignorName.EditValueChanging -= new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsignorCompany_EditValueChanging);


                ConsignorCompany.EditValue = gridView21.GetRowCellValue(rows, "CusName").ToString();
                ConsignorName.EditValue = gridView21.GetRowCellValue(rows, "ContactMan").ToString();
                ConsignorCellPhone.EditValue = gridView21.GetRowCellValue(rows, "ContactCellPhone").ToString();
                ConsignorPhone.EditValue = gridView21.GetRowCellValue(rows, "ContactPhone").ToString();
                Salesman.EditValue = gridView21.GetRowCellValue(rows, "Salesman").ToString();
                if (gridView21.GetRowCellValue(rows, "CusRemarkInfo").ToString() != "")
                {
                    BillRemark.EditValue = "发货人要求:" + gridView21.GetRowCellValue(rows, "CusRemarkInfo").ToString();
                }
                gridControl8.Visible = false;
                //ReceivMode.Focus();
                ConsigneeCompany.Focus();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                ConsignorCompany.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsignorCompany_EditValueChanging);
                ConsignorName.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsignorCompany_EditValueChanging);
            }
        }

        private void gridControl7_DoubleClick(object sender, EventArgs e)
        {
            setReceiveCust();
        }

        private void setReceiveCust()
        {
            try
            {
                int rows = gridView22.FocusedRowHandle;
                if (rows < 0) return;
                string remark = BillRemark.Text.Trim();
                ConsigneeCompany.EditValueChanging -= new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsigneeCompany_EditValueChanging);
                ConsigneeName.EditValueChanging -= new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsigneeCompany_EditValueChanging);

                ConsigneeCompany.EditValue = gridView22.GetRowCellValue(rows, "RCName").ToString();
                ConsigneeName.EditValue = gridView22.GetRowCellValue(rows, "ContactMan").ToString();
                ConsigneeCellPhone.EditValue = gridView22.GetRowCellValue(rows, "ContactCellPhone").ToString();
                ConsigneePhone.EditValue = gridView22.GetRowCellValue(rows, "ContactPhone").ToString();
                if (CommonClass.UserInfo.companyid == "239")
                {
                    Address.EditValue = gridView22.GetRowCellValue(rows, "RecievAddress").ToString();
                }
                if (gridView22.GetRowCellValue(rows, "CusRemarkInfo").ToString() != "")
                {
                    BillRemark.EditValue = remark + "  收货要求:" + gridView22.GetRowCellValue(rows, "CusRemarkInfo").ToString();//hj20181107
                }
                //if (TransferMode.Text != "自提")
                //{
                //    ReceivAddress.EditValue = gridView9.GetRowCellValue(rows, "RecievAddress");
                //}
                gridControl7.Visible = false;
                TransitMode.Focus();//HJ20180418
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                ConsigneeCompany.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsigneeCompany_EditValueChanging);
                ConsigneeName.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsigneeCompany_EditValueChanging);
            }
        }

        private void ConsignorCompany_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            { gridControl8.Focus(); }
            if (e.KeyCode == Keys.Escape)
            {
                gridControl8.Visible = false;
            }
        }

        private void gridControl8_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                setCus();
            }
        }

        private void ConsigneeCompany_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            { gridControl7.Focus(); }
            if (e.KeyCode == Keys.Escape)
            {
                gridControl7.Visible = false;
            }
        }

        private void gridControl7_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                setReceiveCust();
            }
        }

        private void ConsignorCompany_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue != null)
            {
                //ConsignorCellPhone.Text = ""; //hj20180706
                //ConsignorPhone.Text = "";
                gridView21.ClearColumnsFilter();
                gridView21.Columns["CusName"].FilterInfo = new ColumnFilterInfo(
                    "[CusName] LIKE " + "'%" + e.NewValue.ToString() + "%'"
                    + " OR [ContactMan] LIKE" + "'%" + e.NewValue.ToString() + "%'"
                    + " OR [ContactPhone] LIKE" + "'%" + e.NewValue.ToString() + "%'"
                    + " OR [ContactCellPhone] LIKE" + "'%" + e.NewValue.ToString() + "%'",
                    "");

                //gridView9.ClearColumnsFilter();
                //gridView9.Columns["CusID"].FilterInfo = new ColumnFilterInfo( "[CusID] LIKE " + "'%" + e.NewValue.ToString() + "%'"  );

            }
            else
            {
                gridView21.ClearColumnsFilter();
                gridView22.ClearColumnsFilter();
            }
        }

        private void ConsigneeCompany_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue != null)
            {
                //ConsigneeCellPhone.Text = ""; //hj20180706
                //ConsigneePhone.Text = "";
                gridView22.ClearColumnsFilter();
                gridView22.Columns["RCName"].FilterInfo = new ColumnFilterInfo(
                    "[RCName] LIKE " + "'%" + e.NewValue.ToString() + "%'"
                    + " OR [ContactMan] LIKE" + "'%" + e.NewValue.ToString() + "%'"
                    + " OR [ContactPhone] LIKE" + "'%" + e.NewValue.ToString() + "%'"
                    + " OR [ContactCellPhone] LIKE" + "'%" + e.NewValue.ToString() + "%'"
                    + " OR [RecievAddress] LIKE" + "'%" + e.NewValue.ToString() + "%'",
                    "");
            }
            else
            {
                gridView22.ClearColumnsFilter();
            }
        }

        //代收货款自动带出代收手续费  ZJF20181122
        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                gridView1.PostEditor();
                gridView1.UpdateCurrentRow();
                gridView1.UpdateSummary();
                gridView2.PostEditor();
                gridView2.UpdateCurrentRow();
                gridView2.UpdateSummary();
                getAccAll();
                if (e.Column.FieldName == "CollectionPay" || e.Column.FieldName == "DeclareValue")
                {
                    SetFeeNew();
                    //decimal AgentFee = Math.Round(ConvertType.ToDecimal(gridView1.GetRowCellValue(e.RowHandle, "CollectionPay")), 2)
                    //    * ConvertType.ToDecimal(CommonClass.Arg.CommissionRate);
                    //gridView1.SetRowCellValue(e.RowHandle, "AgentFee", AgentFee);
                }
            }

            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void gridView2_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                gridView1.PostEditor();
                gridView1.UpdateCurrentRow();
                gridView1.UpdateSummary();
                gridView2.PostEditor();
                gridView2.UpdateCurrentRow();
                gridView2.UpdateSummary();
                getAccAll();
                if (e.Column.FieldName == "height")
                {
                    //hj20180927
                    decimal length = Math.Round(ConvertType.ToDecimal(gridView2.GetRowCellValue(e.RowHandle, "length")), 2);
                    decimal width = Math.Round(ConvertType.ToDecimal(gridView2.GetRowCellValue(e.RowHandle, "width")), 2);
                    decimal height = Math.Round(ConvertType.ToDecimal(gridView2.GetRowCellValue(e.RowHandle, "height")), 2);
                    gridView2.SetRowCellValue(e.RowHandle, "FeeVolume", Math.Round((length * width * height) / 1000000));
                }
                //hj20181030 客户计价
                if (e.Column.FieldName == "FeeWeight" || e.Column.FieldName == "FeeVolume")
                {
                    try
                    {
                        for (int i = 0; i < RowCount; i++)
                        {
                            decimal FeeWeight;
                            if (gridView2.GetRowCellValue(i, "FeeWeight") == null)
                            {
                                FeeWeight = Convert.ToDecimal(gridView2.GetRowCellValue(i, "FeeWeight"));
                            }
                            else
                            {
                                FeeWeight = 0;
                            }
                            double FeeVolume;
                            if (gridView2.GetRowCellValue(i, "FeeVolume") == null)
                            {
                                FeeVolume = Convert.ToDouble(gridView2.GetRowCellValue(i, "FeeVolume"));
                            }
                            else
                            {
                                FeeVolume = 0;
                            }
                            int FeeNum;
                            if (gridView2.GetRowCellValue(i, "Num") == null)
                            {
                                FeeNum = Convert.ToInt32(gridView2.GetRowCellValue(i, "Num"));
                            }
                            else
                            {
                                FeeNum = 0;
                            }
                            if (FeeWeight < 25 && FeeVolume < 0.1 && FeeNum < 10)
                            {
                                gridView2.SetRowCellValue(i, "Freight", ConvertType.ToDecimal(10 * FeeNum));
                            }
                            else
                            {
                                if (gsDenominatedFee == "是")
                                {
                                    SetDenominatedFee();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MsgBox.ShowException(ex);
                    }

                }
                //maohui20181229
                if (e.Column.FieldName == "Num")
                {
                    try
                    {
                        for (int i = 0; i < RowCount; i++)
                        {
                            decimal FeeWeight;
                            if (gridView2.GetRowCellValue(i, "FeeWeight") == null)
                            {
                                FeeWeight = Convert.ToDecimal(gridView2.GetRowCellValue(i, "FeeWeight"));
                            }
                            else
                            {
                                FeeWeight = 0;
                            }
                            double FeeVolume;
                            if (gridView2.GetRowCellValue(i, "FeeVolume") == null)
                            {
                                FeeVolume = Convert.ToDouble(gridView2.GetRowCellValue(i, "FeeVolume"));
                            }
                            else
                            {
                                FeeVolume = 0;
                            }
                            int FeeNum;
                            if (gridView2.GetRowCellValue(i, "Num") == null)
                            {
                                FeeNum = Convert.ToInt32(gridView2.GetRowCellValue(i, "Num"));
                            }
                            else
                            {
                                FeeNum = 0;
                            }
                            if (FeeWeight < 25 && FeeVolume < 0.1 && FeeNum < 10)
                            {
                                gridView2.SetRowCellValue(i, "Freight", ConvertType.ToDecimal(10 * FeeNum));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MsgBox.ShowException(ex);
                    }
                }
                if (e.Column.FieldName == "FeeWeight" || e.Column.FieldName == "FeeVolume" || e.Column.FieldName == "WeightPrice" || e.Column.FieldName == "VolumePrice")
                {
                    decimal FeeWeight = Math.Round(ConvertType.ToDecimal(gridView2.GetRowCellValue(e.RowHandle, "FeeWeight")), 2);
                    decimal FeeVolume = Math.Round(ConvertType.ToDecimal(gridView2.GetRowCellValue(e.RowHandle, "FeeVolume")), 2);
                    decimal WeightPrice = ConvertType.ToDecimal(gridView2.GetRowCellValue(e.RowHandle, "WeightPrice"));
                    decimal VolumePrice = ConvertType.ToDecimal(gridView2.GetRowCellValue(e.RowHandle, "VolumePrice"));
                    decimal accw = FeeWeight * WeightPrice;
                    decimal accv = FeeVolume * VolumePrice;
                    decimal acc = 0;

                    if (CommonClass.UserInfo.companyid == "167" || CommonClass.UserInfo.companyid == "168" || CommonClass.UserInfo.companyid == "152")
                    {
                        acc = Math.Round(Math.Max(accw, accv), 2);
                    }
                    else
                    {
                        acc = Math.Round(Math.Max(accw, accv), 0);
                    }
                    gridView2.SetRowCellValue(e.RowHandle, "Freight", acc);
                    if (WeightPrice == 0 && VolumePrice == 0)
                    {
                        gridView2.SetRowCellValue(e.RowHandle, "FeeType", "");
                    }
                    else
                    {
                        if (accw < accv)
                        {
                            gridView2.SetRowCellValue(e.RowHandle, "FeeType", "计方");
                        }
                        else
                        {
                            gridView2.SetRowCellValue(e.RowHandle, "FeeType", "计重");
                        }
                    }
                    gridView2.SetRowCellValue(e.RowHandle, "Weight", FeeWeight);
                    gridView2.SetRowCellValue(e.RowHandle, "Volume", FeeVolume);
                }

                if (e.Column.FieldName == "FeeWeight" || e.Column.FieldName == "FeeVolume" || e.Column.FieldName == "Package" || e.Column.FieldName == "Num" || e.Column.FieldName == "Varieties")
                {
                    decimal OperationWeight = 0;
                    for (int i = 0; i < RowCount; i++)
                    {
                        if (gridView2.GetRowCellValue(i, "Varieties").ToString() != "")
                        {
                            decimal w = Math.Round(ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeWeight")), 2);
                            decimal v = Math.Round(ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeVolume")), 2);
                            string Package = gridView2.GetRowCellValue(i, "Package").ToString();

                            if (Package != "纸箱" && Package != "纤袋" && Package != "膜")
                            {
                                // OperationWeight += Math.Max(w, v / (decimal)3.8 * 1000) * (decimal)1.1;
                                //zaj 2017-12-6
                                OperationWeight += Math.Max(w, v / (decimal)3.8 * 1000) * (decimal)1.05;

                            }
                            else
                            {
                                OperationWeight += Math.Max(w, v / (decimal)3.8 * 1000);
                            }
                        }
                    }
                    gridView1.SetRowCellValue(0, "OperationWeight", Math.Round(OperationWeight, 2));

                    #region 操作重量
                    //decimal Num = ConvertType.ToDecimal(gridColumn67.SummaryItem.SummaryValue);
                    //decimal FeeWeight = Math.Round(ConvertType.ToDecimal(gridColumn68.SummaryItem.SummaryValue), 2);
                    //decimal FeeVolume = Math.Round(ConvertType.ToDecimal(gridColumn69.SummaryItem.SummaryValue), 2);
                    //decimal weight_cz = FeeWeight;
                    //if (Num > 0)
                    //{
                    //    if (FeeWeight / Convert.ToDecimal(1000) * Convert.ToDecimal(4) - FeeVolume < 0)
                    //    {
                    //        weight_cz = Math.Round(FeeVolume / Convert.ToDecimal(4) * Convert.ToDecimal(1000), 2);
                    //    }
                    //    if (weight_cz / Num > 200)
                    //    {
                    //        weight_cz = Math.Round(weight_cz * Convert.ToDecimal(0.5), 2);
                    //    }
                    //}
                    //当品名中包括家私、家具、服装包、马桶字样是操作重量上浮35% zaj 20180705
                    string varieties = "";
                    decimal weight_tmp = 0;
                    decimal weight_cz = 0;
                    decimal Num = 0;
                    decimal FeeWeight = 0;
                    decimal FeeVolume = 0;

                    for (int i = 0; i < RowCount; i++)
                    {
                        varieties = gridView2.GetRowCellValue(i, "Varieties").ToString().Replace(" ", "");
                        Num = ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "Num"));
                        FeeWeight = ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeWeight"));
                        FeeVolume = ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeVolume"));
                        weight_tmp = FeeWeight;

                        if (Num > 0)
                        {
                            if (FeeWeight / Convert.ToDecimal(1000) * Convert.ToDecimal(4) - FeeVolume < 0)
                            {
                                weight_tmp = Math.Round(FeeVolume / Convert.ToDecimal(4) * Convert.ToDecimal(1000), 2);
                            }
                            if (weight_tmp / Num > 200)
                            {
                                weight_tmp = Math.Round(weight_tmp * Convert.ToDecimal(0.5), 2);
                            }

                        }
                        if (varieties.Contains("家私") || varieties.Contains("家具") || varieties.Contains("服装包") || varieties.Contains("马桶") || varieties.Contains("服装"))
                        {
                            weight_cz = weight_cz + weight_tmp * (decimal)1.35;
                        }
                        else
                        {
                            weight_cz = weight_cz + weight_tmp;
                        }
                    }
                    gridView1.SetRowCellValue(0, "OptWeight", Math.Round(weight_cz, 2));
                    #endregion

                    //SetFee();
                    SetFeeNew();//zaj 2018-4-15新结算
                }

                #region 附加费结算标准
                if (e.Column.FieldName == "UpstairFee"
                    || e.Column.FieldName == "DeclareValue"//"SupportValue"
                    || e.Column.FieldName == "Tax"
                    || e.Column.FieldName == "ReceiptFee"
                    || e.Column.FieldName == "AgentFee"
                    || e.Column.FieldName == "HandleFee"
                    || e.Column.FieldName == "ForkliftFee"
                    || e.Column.FieldName == "NoticeFee"
                    || e.Column.FieldName == "StorageFee"
                    || e.Column.FieldName == "WarehouseFee"
                    || e.Column.FieldName == "Num"
                    || e.Column.FieldName == "OperationWeight"
                    || e.Column.FieldName == "CollectionPay"
                    || e.Column.FieldName == "DiscountTransfer"
                    || e.Column.FieldName == "Freight"
                    )
                {
                    SetFeeNew();
                }
                #endregion

                #region 附加费结算标准
                #region 作废 2018-4-15 zaj


                //#region 上楼费
                //if (e.Column.FieldName == "UpstairFee")
                //{
                //    decimal UpstairFee = Convert.ToDecimal(gridView1.GetRowCellValue(e.RowHandle, "UpstairFee"));
                //    if (UpstairFee > 0)
                //    {
                //        DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='上楼费' ");
                //        if (dr.Length > 0)
                //        {
                //            decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);
                //            decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]);
                //            decimal UpstairFee_C = Math.Round(Math.Max(InnerLowest, UpstairFee * InnerStandard), 2);
                //            gridView8.SetRowCellValue(0, "UpstairFee_C", UpstairFee_C);
                //        }
                //    }
                //    else
                //    {
                //        gridView8.SetRowCellValue(0, "UpstairFee_C", 0);
                //    }
                //}
                //#endregion

                //#region 保价费
                //if (e.Column.FieldName == "SupportValue")
                //{
                //    decimal SupportValue = Convert.ToDecimal(gridView1.GetRowCellValue(e.RowHandle, "SupportValue"));
                //    if (SupportValue > 0)
                //    {
                //        DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='保价费' ");
                //        if (dr.Length > 0)
                //        {
                //            decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);
                //            decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]);
                //            decimal SupportValue_C = Math.Round(Math.Max(InnerLowest, SupportValue * InnerStandard), 2);
                //            gridView8.SetRowCellValue(0, "SupportValue_C", SupportValue_C);
                //        }
                //    }
                //    else
                //    {
                //        gridView8.SetRowCellValue(0, "SupportValue_C", 0);
                //    }
                //}
                //#endregion

                //#region 结算税金
                //if (e.Column.FieldName == "Tax")
                //{
                //    decimal Tax = Convert.ToDecimal(gridView1.GetRowCellValue(e.RowHandle, "Tax"));
                //    if (Tax > 0)
                //    {
                //        DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='税金' ");
                //        if (dr.Length > 0)
                //        {
                //            decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);
                //            decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]);
                //            decimal Tax_C = Math.Round(Math.Max(InnerLowest, Tax * InnerStandard), 2);
                //            gridView8.SetRowCellValue(0, "Tax_C", Tax_C);
                //        }
                //    }
                //    else
                //    {
                //        gridView8.SetRowCellValue(0, "Tax_C", 0);
                //    }
                //}
                //#endregion

                //#region 回单费
                //if (e.Column.FieldName == "ReceiptFee")
                //{
                //    decimal ReceiptFee = Convert.ToDecimal(gridView1.GetRowCellValue(e.RowHandle, "ReceiptFee"));
                //    if (ReceiptFee > 0)
                //    {
                //        DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='回单费' ");
                //        if (dr.Length > 0)
                //        {
                //            decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);
                //            decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]);
                //            decimal ReceiptFee_C = Math.Round(Math.Max(InnerLowest, ReceiptFee * InnerStandard), 2);
                //            gridView8.SetRowCellValue(0, "ReceiptFee_C", ReceiptFee_C);
                //        }
                //    }
                //    else
                //    {
                //        gridView8.SetRowCellValue(0, "ReceiptFee_C", 0);
                //    }
                //}
                //#endregion

                //#region 代收手续费
                //if (e.Column.FieldName == "AgentFee")
                //{
                //    decimal AgentFee = Convert.ToDecimal(gridView1.GetRowCellValue(e.RowHandle, "AgentFee"));
                //    if (AgentFee > 0)
                //    {
                //        DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='代收手续费' ");
                //        if (dr.Length > 0)
                //        {
                //            decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);
                //            decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]);
                //            decimal AgentFee_C = Math.Round(Math.Max(InnerLowest, AgentFee * InnerStandard), 2);
                //            gridView8.SetRowCellValue(0, "AgentFee_C", AgentFee_C);
                //        }
                //    }
                //    else
                //    {
                //        gridView8.SetRowCellValue(0, "AgentFee_C", 0);
                //    }
                //}
                //#endregion

                //#region 装卸费
                //if (e.Column.FieldName == "HandleFee")
                //{
                //    decimal HandleFee = Convert.ToDecimal(gridView1.GetRowCellValue(e.RowHandle, "HandleFee"));
                //    if (HandleFee > 0)
                //    {
                //        DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='装卸费' ");
                //        if (dr.Length > 0)
                //        {
                //            decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);
                //            decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]);
                //            decimal HandleFee_C = Math.Round(Math.Max(InnerLowest, HandleFee * InnerStandard), 2);
                //            gridView8.SetRowCellValue(0, "HandleFee_C", HandleFee_C);
                //        }
                //    }
                //    else
                //    {
                //        gridView8.SetRowCellValue(0, "HandleFee_C", 0);
                //    }
                //}
                //#endregion

                //#region 叉车费
                //if (e.Column.FieldName == "ForkliftFee")
                //{
                //    decimal ForkliftFee = Convert.ToDecimal(gridView1.GetRowCellValue(e.RowHandle, "ForkliftFee"));
                //    if (ForkliftFee > 0)
                //    {
                //        DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='叉车费' ");
                //        if (dr.Length > 0)
                //        {
                //            decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);
                //            decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]);
                //            decimal ForkliftFee_C = Math.Round(Math.Max(InnerLowest, ForkliftFee * InnerStandard), 2);
                //            gridView8.SetRowCellValue(0, "ForkliftFee_C", ForkliftFee_C);
                //        }
                //    }
                //    else
                //    {
                //        gridView8.SetRowCellValue(0, "ForkliftFee_C", 0);
                //    }
                //}
                //#endregion

                //#region 控货费
                //if (e.Column.FieldName == "NoticeFee")
                //{
                //    decimal NoticeFee = Convert.ToDecimal(gridView1.GetRowCellValue(e.RowHandle, "NoticeFee"));
                //    if (NoticeFee > 0)
                //    {
                //        DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='控货费' ");
                //        if (dr.Length > 0)
                //        {
                //            decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);
                //            decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]);
                //            decimal NoticeFee_C = Math.Round(Math.Max(InnerLowest, NoticeFee * InnerStandard), 2);
                //            gridView8.SetRowCellValue(0, "NoticeFee_C", NoticeFee_C);
                //        }
                //    }
                //    else
                //    {
                //        gridView8.SetRowCellValue(0, "NoticeFee_C", 0);
                //    }
                //}
                //#endregion

                //#region 进仓费
                //if (e.Column.FieldName == "StorageFee")
                //{
                //    decimal StorageFee = Convert.ToDecimal(gridView1.GetRowCellValue(e.RowHandle, "StorageFee"));
                //    if (StorageFee > 0)
                //    {
                //        DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='进仓费' ");
                //        if (dr.Length > 0)
                //        {
                //            decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);
                //            decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]);
                //            decimal StorageFee_C = Math.Round(Math.Max(InnerLowest, StorageFee * InnerStandard), 2);
                //            gridView8.SetRowCellValue(0, "StorageFee_C", StorageFee_C);
                //        }
                //    }
                //    else
                //    {
                //        gridView8.SetRowCellValue(0, "StorageFee_C", 0);
                //    }
                //}
                //#endregion

                //#region 仓储费
                //if (e.Column.FieldName == "WarehouseFee")
                //{
                //    decimal WarehouseFee = Convert.ToDecimal(gridView1.GetRowCellValue(e.RowHandle, "WarehouseFee"));
                //    if (WarehouseFee > 0)
                //    {
                //        DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='仓储费' ");
                //        if (dr.Length > 0)
                //        {
                //            decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);
                //            decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]);
                //            decimal WarehouseFee_C = Math.Round(Math.Max(InnerLowest, WarehouseFee * InnerStandard), 2);
                //            gridView8.SetRowCellValue(0, "WarehouseFee_C", WarehouseFee_C);
                //        }
                //    }
                //    else
                //    {
                //        gridView8.SetRowCellValue(0, "WarehouseFee_C", 0);
                //    }
                //}
                //#endregion

                //#region 工本费
                //if (e.Column.FieldName == "Num")
                //{
                //    int Num = 0;
                //    for (int i = 0; i < RowCount; i++)
                //    {
                //        Num += ConvertType.ToInt32(gridView2.GetRowCellValue(i, "Num"));
                //    }
                //    if (Num > 0)
                //    {
                //        DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='工本费' ");
                //        if (dr.Length > 0)
                //        {
                //            decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);
                //            decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]);
                //            decimal Expense_C = Math.Round(InnerStandard * Num, 2);
                //            gridView8.SetRowCellValue(0, "Expense_C", Expense_C);
                //        }
                //    }
                //    else
                //    {
                //        gridView8.SetRowCellValue(0, "Expense_C", 0);
                //    }
                //}
                //#endregion

                #endregion

                #endregion

                //gridView8.SetRowCellValue(0, "DeliveryFee", gridView1.GetRowCellValue(0, "DeliveryFee"));
                DeliveryFee.Text = gridView1.GetRowCellValue(0, "DeliveryFee").ToString();

                if (TransitMode.Text == "中强整车")//maohui20180913
                {
                    if (e.Column.FieldName == "MainlineFee")
                    {
                        //gridView8.SetRowCellValue(0, "MainlineFee", gridView1.GetRowCellValue(0, "MainlineFee"));
                        MainlineFee.Text = gridView1.GetRowCellValue(0, "MainlineFee").ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void getAccAll()
        {
            try
            {
                //第一行
                decimal Freight = 0;
                for (int i = 0; i < RowCount; i++)
                {
                    if (gridView2.GetRowCellValue(i, "Varieties").ToString() != "")
                    {
                        Freight += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "Freight"));
                    }
                }

                //第二行
                decimal DeliFee = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "DeliFee"));
                //decimal DeclareValue = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "DeclareValue"));
                decimal SupportValue = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "SupportValue"));
                decimal Tax = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "Tax"));
                decimal InformationFee = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "InformationFee"));

                decimal Expense = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "Expense"));
                decimal CollectionPay = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "CollectionPay"));
                decimal FuelFee = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "FuelFee"));
                decimal AgentFee = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "AgentFee"));


                //第三行
                decimal UpstairFee = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "UpstairFee"));
                decimal HandleFee = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "HandleFee"));
                decimal ForkliftFee = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "ForkliftFee"));
                decimal StorageFee = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "StorageFee"));
                decimal CustomsFee = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "CustomsFee"));
                decimal MatPay = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "MatPay"));//HJ20180911

                decimal packagFee = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "packagFee"));
                decimal FrameFee = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "FrameFee"));
                decimal ChangeFee = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "ChangeFee"));
                decimal OtherFee = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "OtherFee"));
                decimal NoticeFee = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "NoticeFee"));

                decimal ReceiptFee = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "ReceiptFee"));
                decimal ReceivFee = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "ReceivFee"));

                decimal WarehouseFee = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "WarehouseFee"));
                decimal DiscountTransfer = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "DiscountTransfer"));

                PaymentAmout.Text = (Freight
                    + DeliFee + SupportValue + Tax + InformationFee
                    + Expense + CollectionPay + FuelFee + AgentFee
                    + UpstairFee + HandleFee + ForkliftFee
                    + StorageFee
                    + CustomsFee
                    + packagFee + FrameFee + ChangeFee + OtherFee + NoticeFee
                    + ReceiptFee + ReceivFee + WarehouseFee + DiscountTransfer + MatPay
                    ) + "";
                //现付,提付,月结,两笔付,货到前付
                if (PaymentMode.Text.Trim() == "现付")
                {
                    NowPay.Text = PaymentAmout.Text;
                }
                if (PaymentMode.Text.Trim() == "提付")
                {
                    FetchPay.Text = PaymentAmout.Text;
                }
                if (PaymentMode.Text.Trim() == "月结")
                {
                    MonthPay.Text = PaymentAmout.Text;
                }
                if (PaymentMode.Text.Trim() == "短欠")
                {
                    ShortOwePay.Text = PaymentAmout.Text;
                }
                if (PaymentMode.Text.Trim() == "货到前付")
                {
                    BefArrivalPay.Text = PaymentAmout.Text;
                }
                if (PaymentMode.Text.Trim() == "回单付")
                {
                    ReceiptPay.Text = PaymentAmout.Text;
                }
                if (PaymentMode.Text.Trim() == "欠付")
                {
                    OwePay.Text = PaymentAmout.Text;
                }
                if (PaymentMode.Text.Trim() == "提付改欠") //hj20181120
                {
                    FetchPay.Text = PaymentAmout.Text;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError("计算运费：" + ex.Message);
            }
        }


        private void clear()
        {
            try
            {
                gridView1.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView2_CellValueChanged);
                gridView2.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView2_CellValueChanged);

                //string str = "";
                //Stopwatch st = new Stopwatch();
                //st.Start();
                BillNo.EditValue = "";
                VehicleNo.EditValue = "";
                //StartSite.EditValue = "";
                //TransferMode.EditValue = "";
                DestinationSite.EditValue = "";
                TransferSite.EditValue = "";

                //TransitMode.EditValue = "";
                CusOderNo.EditValue = "";
                ConsigneeCellPhone.EditValue = "";
                ConsigneePhone.EditValue = "";
                ConsigneeName.EditValue = "";
                ConsigneeCompany.EditValue = "";
                PickGoodsSite.EditValue = "";

                //ReceivCity.EditValue = "";
                //ReceivArea.EditValue = "";
                //ReceivStreet.EditValue = "";
                //ReceivAddress.EditValue = "";
                Province.EditValue = "";
                City.EditValue = "";
                Address.EditValue = "";
                ConsignorCellPhone.EditValue = "";
                ConsignorPhone.EditValue = "";
                ConsignorName.EditValue = "";
                ConsignorCompany.EditValue = "";
                VehicleNum.EditValue = "";


                ValuationType.EditValue = "";
                ReceivOrderNo.EditValue = "";
                Salesman.EditValue = "";
                AlienGoods.Checked = false;
                GoodsVoucher.Checked = false;
                PreciousGoods.Checked = false;
                NoticeState.Checked = false;
                //st.Stop();
                //str += st.ElapsedMilliseconds.ToString() + "\r\n";
                //st.Start();

                IsViewTraceFee.Checked = false;  //luohui
                //isSendTimer = 0;
                //isSendTimerNight = 0;
                //isControlLoad = 0;
                //isUpStair = 0;
                ////20180728
                //exactTime = "";
                //exactTimeNight = "";
                //elevatorCondition = "";
                IsSortFee.Checked = false;  //luohui
                for (int i = 0; i < RowCount; i++)
                {
                    gridView2.SetRowCellValue(i, "GoodsType", "");
                    gridView2.SetRowCellValue(i, "Varieties", "");
                    gridView2.SetRowCellValue(i, "Package", "");
                    gridView2.SetRowCellValue(i, "Num", "0");

                    gridView2.SetRowCellValue(i, "FeeWeight", "0");
                    gridView2.SetRowCellValue(i, "FeeVolume", "0");
                    gridView2.SetRowCellValue(i, "Weight", "0");
                    gridView2.SetRowCellValue(i, "Volume", "0");
                    gridView2.SetRowCellValue(i, "WeightPrice", "0");

                    gridView2.SetRowCellValue(i, "VolumePrice", "0");
                    gridView2.SetRowCellValue(i, "FeeType", "");
                    gridView2.SetRowCellValue(i, "Freight", "0");
                }

                gridView2.SetRowCellValue(0, "Package", "纸箱");

                gridView1.SetRowCellValue(0, "DeliFee", "0");
                gridView1.SetRowCellValue(0, "ReceivFee", "0");
                gridView1.SetRowCellValue(0, "DeclareValue", "0");
                gridView1.SetRowCellValue(0, "SupportValue", "0");
                gridView1.SetRowCellValue(0, "Rate", "0");

                gridView1.SetRowCellValue(0, "Tax", "0");
                gridView1.SetRowCellValue(0, "InformationFee", "0");
                gridView1.SetRowCellValue(0, "Expense", "0");
                gridView1.SetRowCellValue(0, "NoticeFee", "0");
                gridView1.SetRowCellValue(0, "DiscountTransfer", "0");

                gridView1.SetRowCellValue(0, "CollectionPay", "0");
                gridView1.SetRowCellValue(0, "AgentFee", "0");
                gridView1.SetRowCellValue(0, "FuelFee", "0");

                gridView1.SetRowCellValue(0, "UpstairFee", "0");
                gridView1.SetRowCellValue(0, "HandleFee", "0");
                gridView1.SetRowCellValue(0, "ForkliftFee", "0");
                gridView1.SetRowCellValue(0, "StorageFee", "0");
                gridView1.SetRowCellValue(0, "CustomsFee", "0");
                gridView1.SetRowCellValue(0, "MatPay", "0");//HJ20180911

                gridView1.SetRowCellValue(0, "packagFee", "0");
                gridView1.SetRowCellValue(0, "FrameFee", "0");
                gridView1.SetRowCellValue(0, "ChangeFee", "0");
                gridView1.SetRowCellValue(0, "OtherFee", "0");
                gridView1.SetRowCellValue(0, "ReceiptFee", "0");

                gridView1.SetRowCellValue(0, "FreightAmount", "0");
                gridView1.SetRowCellValue(0, "ActualFreight", "0");
                gridView1.SetRowCellValue(0, "WarehouseFee", "0");

                //st.Stop();
                //str += st.ElapsedMilliseconds.ToString() + "\r\n";
                //st.Start();
                ReceiptCondition.EditValue = "";
                IsInvoice.Checked = false;



                PaymentMode.EditValue = "提付";
                PaymentAmout.EditValue = "";

                NowPay.EditValue = "";
                FetchPay.EditValue = "";
                MonthPay.EditValue = "";
                ShortOwePay.EditValue = "";
                BefArrivalPay.EditValue = "";
                BillRemark.EditValue = "";
                WebPlatform.EditValue = "";
                WebBillNo.EditValue = "";
                DisTranName.EditValue = "";
                DisTranBank.EditValue = "";
                DisTranAccount.EditValue = "";
                CollectionName.EditValue = "";
                CollectionBank.EditValue = "";
                CollectionAccount.EditValue = "";
                CauseName.EditValue = CommonClass.UserInfo.CauseName;
                AreaName.EditValue = CommonClass.UserInfo.AreaName;
                DepName.EditValue = CommonClass.UserInfo.DepartName;

                DisTranBranch.EditValue = "";
                CollectionBranch.EditValue = "";
                BillMan.EditValue = CommonClass.UserInfo.UserName;
                begWeb.EditValue = CommonClass.UserInfo.WebName;

                //st.Stop();
                //str += st.ElapsedMilliseconds.ToString() + "\r\n";
                //st.Start();
                MainlineFee.Text = "0";
                DeliveryFee.Text = "0";
                TransferFee.Text = "0";
                DepartureOptFee.Text = "0";
                TerminalOptFee.Text = "0";
                TerminalAllotFee.Text = "0";

                ReceiptFee_C.Text = "0";
                NoticeFee_C.Text = "0";
                SupportValue_C.Text = "0";

                AgentFee_C.Text = "0";
                PackagFee_C.Text = "0";
                OtherFee_C.Text = "0";

                HandleFee_C.Text = "0";
                StorageFee_C.Text = "0";
                WarehouseFee_C.Text = "0";

                ForkliftFee_C.Text = "0";
                Tax_C.Text = "0";
                ChangeFee_C.Text = "0";

                UpstairFee_C.Text = "0";
                CustomsFee_C.Text = "0";
                FrameFee_C.Text = "0";

                Expense_C.Text = "0";
                FuelFee_C.Text = "0";
                InformationFee_C.Text = "0";

                //st.Stop();
                //str += st.ElapsedMilliseconds.ToString() + "\r\n";
                //st.Start();
                IsReceiptFee.Checked = false;
                IsSupportValue.Checked = false;
                IsAgentFee.Checked = false;
                IsPackagFee.Checked = false;
                IsOtherFee.Checked = false;
                IsHandleFee.Checked = false;
                IsStorageFee.Checked = false;
                IsWarehouseFee.Checked = false;
                IsForkliftFee.Checked = false;
                IsChangeFee.Checked = false;
                IsUpstairFee.Checked = false;
                IsCustomsFee.Checked = false;
                IsFrameFee.Checked = false;
                IsMatPay.Checked = false;//HJ20180911
                IsRebates.Checked = false;//HJ20180911

                DraftGUID.Text = Guid.NewGuid().ToString();

                gridView1.SetRowCellValue(0, "OperationWeight", 0);
                gridView1.SetRowCellValue(0, "OptWeight", 0);


                IsCenterBack.Text = "";



                //st.Stop();
                //str += st.ElapsedMilliseconds.ToString() + "\r\n";
                //MsgBox.ShowOK(str);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                gridView1.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView2_CellValueChanged);
                gridView2.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView2_CellValueChanged);
            }
        }

        private void Upd_BillNO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && Upd_BillNO.Text != BillNo.Text)
            {
                BillNO = Upd_BillNO.Text;
                //如果走改单流程 以下操作不允许变
                this.IsSupportValue.Enabled = false; //保价费不能改
                this.PreciousGoods.Enabled = false; //必走货不能改
                if (CommonClass.UserInfo.companyid == "105")//|| CommonClass.UserInfo.companyid == "106"
                {
                    this.IsSupportValue.Enabled = false; //保价费不能改zaj20180620
                }
                GetWayBill();
                int Billstatus = Convert.ToInt32(dr["BillState"]);//毛慧20171226

                if (Billstatus == 9 || Billstatus == 14)
                {
                    //if (Billstatus == 9)
                    //{
                    //    MsgBox.ShowError("此运单状态为中转，暂时无法进行改单申请！如需更改，请先退回库存取消中转！");
                    //    this.Close();
                    //}
                    //if (Billstatus == 14)
                    //{
                    //    MsgBox.ShowError("此运单状态为送货，暂时无法进行改单申请！如需更改，请先退回库存取消送货！");
                    //    this.Close();
                    //}
                    //if (Billstatus == 16)
                    //{
                    //    //if()
                    //    MsgBox.ShowError("此运单状态为签收，暂时无法进行改单申请！如需更改，请先退回库存取消签收！");
                    //    this.Close();
                    //}
                }
                //hj 分拨的运单还未接收不能改单
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_FBState", new List<SqlPara> { new SqlPara("BillNo", BillNO) }));
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    MsgBox.ShowError("ZQTMS系统还没有拨入接收,不允许更改运单!只有拨入接收后才能更改!");
                    this.Close();
                }
                if (Upd_Num == 1)//改单申请过来的
                {

                    SetFeeNew();
                }
            }
        }

        private string billDate = "";
        public int num_old = 0;
        //改单申请 保存
        private void Savebill_Upd_Apply()
        {
            gridView1.PostEditor();
            gridView2.PostEditor();


            if (begWeb.Text.Trim() != CommonClass.UserInfo.WebName && xmdr == null)
            {

                MsgBox.ShowOK("本运单开单网点为：" + begWeb.Text.Trim() + "，不属于当前网点：" + CommonClass.UserInfo.WebName + "，不能保存！");
                return;
            }


            //加盟改单锁定
            if (CommonClass.QSP_LOCK_8(billDate))
            {
                return;
            }
            if (!HasApply())
            {
                MsgBox.ShowOK("已存在【改单申请】，不能重新申请！");
                return;
            }

            List<SqlPara> list1 = new List<SqlPara>();  //毛慧20171229 查询运单审核状态
            list1.Add(new SqlPara("BillNO", BillNO));
            SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_VerifState", list1);
            DataSet ds1 = SqlHelper.GetDataSet(sps1);

            if (ds1 == null || ds1.Tables.Count == 0)
            {
                MsgBox.ShowOK("查无此单，请检查!");
                return;
            }
            int IsAccept = 0;//是否已拨入接收
            if (ds1.Tables.Count == 3)
            {
                IsAccept = ds1.Tables[2].Rows.Count;
            }

            int changes_num = 0;
            string GoodsType = gridView2.GetRowCellValue(0, "GoodsType").ToString();
            string Varieties = gridView2.GetRowCellValue(0, "Varieties").ToString();
            string Package = gridView2.GetRowCellValue(0, "Package").ToString();
            decimal Num = Convert.ToDecimal(gridView2.GetRowCellValue(0, "Num").ToString() == "" ? "0" : gridView2.GetRowCellValue(0, "Num").ToString());
            decimal FeeWeight = Convert.ToDecimal(gridView2.GetRowCellValue(0, "FeeWeight").ToString() == "" ? "0" : gridView2.GetRowCellValue(0, "FeeWeight").ToString());
            decimal FeeVolume = Convert.ToDecimal(gridView2.GetRowCellValue(0, "FeeVolume").ToString() == "" ? "0" : gridView2.GetRowCellValue(0, "FeeVolume").ToString());

            decimal Weight = Convert.ToDecimal(gridView2.GetRowCellValue(0, "Weight").ToString() == "" ? "0" : gridView2.GetRowCellValue(0, "Weight").ToString());
            decimal Volume = Convert.ToDecimal(gridView2.GetRowCellValue(0, "Volume").ToString() == "" ? "0" : gridView2.GetRowCellValue(0, "Volume").ToString());
            decimal WeightPrice = Convert.ToDecimal(gridView2.GetRowCellValue(0, "WeightPrice").ToString() == "" ? "0" : gridView2.GetRowCellValue(0, "WeightPrice").ToString());
            decimal VolumePrice = Convert.ToDecimal(gridView2.GetRowCellValue(0, "VolumePrice").ToString() == "" ? "0" : gridView2.GetRowCellValue(0, "VolumePrice").ToString());
            string FeeType = gridView2.GetRowCellValue(0, "FeeType").ToString();
            decimal Freight = Convert.ToDecimal(gridView2.GetRowCellValue(0, "Freight").ToString() == "" ? "0" : gridView2.GetRowCellValue(0, "Freight").ToString());

            decimal DeliFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "DeliFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "DeliFee").ToString());
            decimal DeclareValue = Convert.ToDecimal(gridView1.GetRowCellValue(0, "DeclareValue").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "DeclareValue").ToString());
            decimal SupportValue = Convert.ToDecimal(gridView1.GetRowCellValue(0, "SupportValue").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "SupportValue").ToString());
            decimal Tax = Convert.ToDecimal(gridView1.GetRowCellValue(0, "Tax").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "Tax").ToString());
            decimal InformationFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "InformationFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "InformationFee").ToString());

            decimal Expense = Convert.ToDecimal(gridView1.GetRowCellValue(0, "Expense").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "Expense").ToString());
            decimal DiscountTransfer = Convert.ToDecimal(gridView1.GetRowCellValue(0, "DiscountTransfer").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "DiscountTransfer").ToString());
            decimal CollectionPay = Convert.ToDecimal(gridView1.GetRowCellValue(0, "CollectionPay").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "CollectionPay").ToString());
            decimal AgentFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "AgentFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "AgentFee").ToString());
            decimal FuelFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "FuelFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "FuelFee").ToString());

            decimal UpstairFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "UpstairFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "UpstairFee").ToString());
            decimal HandleFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "HandleFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "HandleFee").ToString());
            decimal ForkliftFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "ForkliftFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "ForkliftFee").ToString());
            decimal StorageFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "StorageFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "StorageFee").ToString());
            decimal CustomsFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "CustomsFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "CustomsFee").ToString());
            decimal packagFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "packagFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "packagFee").ToString());
            decimal MatPay = Convert.ToDecimal(gridView1.GetRowCellValue(0, "MatPay").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "MatPay").ToString());//HJ20180911垫付费

            decimal FrameFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "FrameFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "FrameFee").ToString());
            decimal ChangeFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "ChangeFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "ChangeFee").ToString());
            decimal OtherFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "OtherFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "OtherFee").ToString());
            decimal NoticeFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "NoticeFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "NoticeFee").ToString());
            decimal ReceiptFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "ReceiptFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "ReceiptFee").ToString());
            decimal ReceivFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "ReceivFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "ReceivFee").ToString());

            decimal WarehouseFee = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "WarehouseFee").ToString());

            decimal ActualFreight = ConvertType.ToDecimal(PaymentAmout.Text) - CollectionPay - DiscountTransfer - MatPay; //实收金额

            decimal ReceiptFee_C = ConvertType.ToDecimal(this.ReceiptFee_C.Text.Trim().ToString());
            decimal NoticeFee_C = ConvertType.ToDecimal(this.NoticeFee_C.Text.Trim().ToString());
            decimal SupportValue_C = ConvertType.ToDecimal(this.SupportValue_C.Text.Trim().ToString());
            decimal AgentFee_C = ConvertType.ToDecimal(this.AgentFee_C.Text.Trim().ToString());
            decimal PackagFee_C = ConvertType.ToDecimal(this.PackagFee_C.Text.Trim().ToString());

            decimal OtherFee_C = ConvertType.ToDecimal(this.OtherFee_C.Text.Trim().ToString());
            decimal HandleFee_C = ConvertType.ToDecimal(this.HandleFee_C.Text.Trim().ToString());
            decimal StorageFee_C = ConvertType.ToDecimal(this.StorageFee_C.Text.Trim().ToString());
            decimal WarehouseFee_C = ConvertType.ToDecimal(this.WarehouseFee_C.Text.Trim().ToString());
            decimal ForkliftFee_C = ConvertType.ToDecimal(this.ForkliftFee_C.Text.Trim().ToString());

            decimal Tax_C = ConvertType.ToDecimal(this.Tax_C.Text.Trim().ToString());
            decimal ChangeFee_C = ConvertType.ToDecimal(this.ChangeFee_C.Text.Trim().ToString());
            decimal UpstairFee_C = ConvertType.ToDecimal(this.UpstairFee_C.Text.Trim().ToString());
            decimal CustomsFee_C = ConvertType.ToDecimal(this.CustomsFee_C.Text.Trim().ToString());
            decimal FrameFee_C = ConvertType.ToDecimal(this.FrameFee_C.Text.Trim().ToString());

            decimal Expense_C = ConvertType.ToDecimal(this.Expense_C.Text.Trim().ToString());
            decimal FuelFee_C = ConvertType.ToDecimal(this.FuelFee_C.Text.Trim().ToString());
            decimal InformationFee_C = ConvertType.ToDecimal(this.InformationFee_C.Text.Trim().ToString());

            decimal MainlineFee = ConvertType.ToDecimal(this.MainlineFee.Text.Trim().ToString());
            decimal DeliveryFee = ConvertType.ToDecimal(this.DeliveryFee.Text.Trim().ToString());
            //decimal DeliveryFee = NewDeliveryFee;//zaj 20180710
            decimal TransferFee = ConvertType.ToDecimal(this.TransferFee.Text.Trim().ToString());
            decimal DepartureOptFee = ConvertType.ToDecimal(this.DepartureOptFee.Text.Trim().ToString());
            decimal TerminalOptFee = ConvertType.ToDecimal(this.TerminalOptFee.Text.Trim().ToString());
            decimal TerminalAllotFee = ConvertType.ToDecimal(this.TerminalAllotFee.Text.Trim().ToString());

            decimal OperationWeight = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "OperationWeight")); //结算重量
            decimal OptWeight = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "OptWeight"));  //操作重量
            num_old = Convert.ToInt32(dr["Num"]);

            string VarietiesStr = "", PackageStr = "", NumStr = "", FeeWeightStr = "", FeeVolumeStr = "",
                WeightStr = "", VolumeStr = "", WeightPriceStr = "", VolumePriceStr = "", FeeTypeStr = "", FreightStr = "", companyids = "";

            for (int i = 1; i < RowCount; i++)
            {
                if (gridView2.GetRowCellValue(i, "Varieties").ToString() != "")
                {
                    VarietiesStr += gridView2.GetRowCellValue(i, "Varieties").ToString() + "@";
                    PackageStr += gridView2.GetRowCellValue(i, "Package").ToString() + "@";
                    NumStr += ConvertType.ToInt32(gridView2.GetRowCellValue(i, "Num")) + "@";
                    FeeWeightStr += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeWeight")) + "@";
                    FeeVolumeStr += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeVolume")) + "@";

                    WeightStr += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "Weight")) + "@";
                    VolumeStr += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "Volume")) + "@";
                    WeightPriceStr += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "WeightPrice")) + "@";
                    VolumePriceStr += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "VolumePrice")) + "@";
                    FeeTypeStr += gridView2.GetRowCellValue(i, "FeeType").ToString() + "@";
                    FreightStr += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "Freight")) + "@";
                    companyids += CommonClass.UserInfo.companyid + "@";
                    //合计到第一行
                    Num += ConvertType.ToInt32(gridView2.GetRowCellValue(i, "Num"));
                    FeeWeight += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeWeight"));
                    FeeVolume += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeVolume"));
                    Weight += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "Weight"));
                    Volume += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "Volume"));
                    Freight += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "Freight"));

                }
            }

            int state_x = ConvertType.ToInt32(dr["BillState"].ToString());
            string acctype_x = dr["PaymentMode"].ToString();
            string acctype = PaymentMode.Text.Trim();
            decimal accnow_x = ConvertType.ToDecimal(dr["NowPay"].ToString());
            decimal accnow_c = ConvertType.ToDecimal(NowPay.Text.Trim());

            //if (state_x == 14 || state_x == 16)
            //{
            //    //20180803，LD，唐克辉注释
            //    if (acctype_x == "提付" && acctype == "现付" && IsAccept > 0)
            //    {
            //        MsgBox.ShowOK("运单已出库并已被转分拨接收，提付不能改现付！");
            //        return;
            //    }
            //}
            if (accnow_x > accnow_c)
            {
                //MsgBox.ShowOK("现付金额不能低于原单金额！");
                // return;
            }
            string changes_str = "";
            string sqlstr = "update WayBill  set ";
            int Sign_num = 0;//hj20180503 判断签收后是否改动除付款方式为(月结，回单付，货到前付)以外的信息
            int OtherInfo = 0;

            if (dr["VehicleNo"].ToString() != VehicleNo.Text.Trim())
            {
                changes_str += "【整车申请号】 由:" + dr["VehicleNo"].ToString() + " 改为:" + VehicleNo.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " VehicleNo='" + VehicleNo.Text.Trim() + "' "; changes_num++; Sign_num++;
            }
            if (dr["StartSite"].ToString() != StartSite.Text.Trim())
            {
                changes_str += "【始发站】 由:" + dr["StartSite"].ToString() + " 改为:" + StartSite.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " StartSite='" + StartSite.Text.Trim() + "' "; changes_num++; Sign_num++;
            }
            if (dr["TransferMode"].ToString() != TransferMode.Text.Trim())
            {
                changes_str += "【交接方式】 由:" + dr["TransferMode"].ToString() + " 改为:" + TransferMode.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " TransferMode='" + TransferMode.Text.Trim() + "' "; changes_num++; Sign_num++;
            }
            if (dr["DestinationSite"].ToString() != DestinationSite.Text.Trim())
            {
                changes_str += "【到站】 由:" + dr["DestinationSite"].ToString() + " 改为:" + DestinationSite.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " DestinationSite='" + DestinationSite.Text.Trim() + "' "; changes_num++; Sign_num++;
            }
            if (dr["TransferSite"].ToString() != TransferSite.Text.Trim())
            {
                //20180803，LD，唐克辉注释
                string billstate = dr["billstate"].ToString();//hj20180502
                if ((billstate == "5" || billstate == "6" || billstate == "10" || billstate == "12") && IsAccept > 0)
                {
                    MsgBox.ShowError("运单状态为“发车、又发、转送到网点、又转送到网点”，并已被转分拨接收时，中转地不能修改!");
                    TransferSite.Text = dr["TransferSite"].ToString();
                    return;
                }
                changes_str += "【中转地】 由:" + dr["TransferSite"].ToString() + " 改为:" + TransferSite.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " TransferSite='" + TransferSite.Text.Trim() + "' "; changes_num++; Sign_num++;
            }

            if (dr["TransitMode"].ToString() != TransitMode.Text.Trim())
            {
                changes_str += "【运输方式】 由:" + dr["TransitMode"].ToString() + " 改为:" + TransitMode.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " TransitMode='" + TransitMode.Text.Trim() + "' "; changes_num++; Sign_num++;
            }
            if (dr["CusOderNo"].ToString() != CusOderNo.Text.Trim())
            {
                changes_str += "【客户单号】 由:" + dr["CusOderNo"].ToString() + " 改为:" + CusOderNo.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " CusOderNo='" + CusOderNo.Text.Trim() + "' "; changes_num++; Sign_num++;
            }

            //hj20180628
            string isNoticeState = "0";
            if (NoticeState.Checked)
            {
                isNoticeState = "1";
            }
            if (dr["NoticeState"].ToString() == "0" && dr["NoticeState"].ToString() != isNoticeState)
            {
                changes_str += "【收货人手机】 由:" + dr["ConsigneeCellPhone_K"].ToString() + " 改为:" + ConsigneeCellPhone.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneeCellPhone_K='" + ConsigneeCellPhone.Text.Trim() + "' "; changes_num++; Sign_num++;
                sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneeCellPhone='888888' "; changes_num++; Sign_num++;

                changes_str += "【收货人座机】 由:" + dr["ConsigneePhone_K"].ToString() + " 改为:" + ConsigneePhone.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneePhone_K='" + ConsigneePhone.Text.Trim() + "' "; changes_num++; Sign_num++;
                sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneePhone='888888' "; changes_num++; Sign_num++;

                changes_str += "【收货联系人】 由:" + dr["ConsigneeName"].ToString() + " 改为:" + ConsigneeName.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneeName_K='" + ConsigneeName.Text.Trim() + "' "; changes_num++; Sign_num++;
                sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneeName='888888' "; changes_num++; Sign_num++;

                changes_str += "【收货公司名称】 由:" + dr["ConsigneeCompany_K"].ToString() + " 改为:" + ConsigneeCompany.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneeCompany_K='" + ConsigneeCompany.Text.Trim() + "' "; changes_num++; Sign_num++;
                sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneeCompany='888888' "; changes_num++; Sign_num++;
            }
            else
            {
                if (dr["ConsigneeCellPhone_K"].ToString() != ConsigneeCellPhone.Text.Trim())
                {
                    changes_str += "【收货人手机】 由:" + dr["ConsigneeCellPhone_K"].ToString() + " 改为:" + ConsigneeCellPhone.Text.Trim();
                    sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneeCellPhone='" + ConsigneeCellPhone.Text.Trim() + "' "; changes_num++; Sign_num++;
                    sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneeCellPhone_K='" + ConsigneeCellPhone.Text.Trim() + "' "; changes_num++; Sign_num++;
                }
                if (dr["ConsigneePhone_K"].ToString() != ConsigneePhone.Text.Trim())
                {
                    changes_str += "【收货人座机】 由:" + dr["ConsigneePhone_K"].ToString() + " 改为:" + ConsigneePhone.Text.Trim();
                    sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneePhone='" + ConsigneePhone.Text.Trim() + "' "; changes_num++; Sign_num++;
                    sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneePhone_K='" + ConsigneePhone.Text.Trim() + "' "; changes_num++; Sign_num++;
                }
                if (dr["ConsigneeName_K"].ToString() != ConsigneeName.Text.Trim())
                {
                    changes_str += "【收货联系人】 由:" + dr["ConsigneeName_K"].ToString() + " 改为:" + ConsigneeName.Text.Trim();
                    sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneeName='" + ConsigneeName.Text.Trim() + "' "; changes_num++; Sign_num++;
                    sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneeName_K='" + ConsigneeName.Text.Trim() + "' "; changes_num++; Sign_num++;
                }
                if (dr["ConsigneeCompany_K"].ToString() != ConsigneeCompany.Text.Trim())
                {
                    changes_str += "【收货公司名称】 由:" + dr["ConsigneeCompany_K"].ToString() + " 改为:" + ConsigneeCompany.Text.Trim();
                    sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneeCompany='" + ConsigneeCompany.Text.Trim() + "' "; changes_num++; Sign_num++;
                    sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneeCompany_K='" + ConsigneeCompany.Text.Trim() + "' "; changes_num++; Sign_num++;
                }
            }


            //if (NoticeState.Checked)
            //{
            //    if (dr["ConsigneeCellPhone_K"].ToString() != ConsigneeCellPhone.Text.Trim())
            //    {
            //        changes_str += "【收货人手机】 由:" + dr["ConsigneeCellPhone_K"].ToString() + " 改为:" + ConsigneeCellPhone.Text.Trim();
            //        sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneeCellPhone_K='" + ConsigneeCellPhone.Text.Trim() + "' "; changes_num++; Sign_num++;
            //        //sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneeCellPhone='888888' "; changes_num++;
            //    }
            //    if (dr["ConsigneePhone_K"].ToString() != ConsigneePhone.Text.Trim())
            //    {
            //        changes_str += "【收货人座机】 由:" + dr["ConsigneePhone_K"].ToString() + " 改为:" + ConsigneePhone.Text.Trim();
            //        sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneePhone_K='" + ConsigneePhone.Text.Trim() + "' "; changes_num++; Sign_num++;
            //        //sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneePhone='888888' "; changes_num++;
            //    }
            //    if (dr["ConsigneeName_K"].ToString() != ConsigneeName.Text.Trim())
            //    {
            //        changes_str += "【收货联系人】 由:" + dr["ConsigneeName"].ToString() + " 改为:" + ConsigneeName.Text.Trim();
            //        sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneeName_K='" + ConsigneeName.Text.Trim() + "' "; changes_num++; Sign_num++;
            //        //sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneeName='888888' "; changes_num++;
            //    }
            //    if (dr["ConsigneeCompany_K"].ToString() != ConsigneeCompany.Text.Trim())
            //    {
            //        changes_str += "【收货公司名称】 由:" + dr["ConsigneeCompany_K"].ToString() + " 改为:" + ConsigneeCompany.Text.Trim();
            //        sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneeCompany_K='" + ConsigneeCompany.Text.Trim() + "' "; changes_num++; Sign_num++;
            //        //sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneeCompany='888888' "; changes_num++;
            //    }
            //}
            //else
            //{
            //    if (dr["ConsigneeCellPhone_K"].ToString() != ConsigneeCellPhone.Text.Trim())
            //    {
            //        changes_str += "【收货人手机】 由:" + dr["ConsigneeCellPhone_K"].ToString() + " 改为:" + ConsigneeCellPhone.Text.Trim();
            //        sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneeCellPhone='" + ConsigneeCellPhone.Text.Trim() + "' "; changes_num++; Sign_num++;
            //        sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneeCellPhone_K='" + ConsigneeCellPhone.Text.Trim() + "' "; changes_num++; Sign_num++;
            //    }
            //    if (dr["ConsigneePhone_K"].ToString() != ConsigneePhone.Text.Trim())
            //    {
            //        changes_str += "【收货人座机】 由:" + dr["ConsigneePhone_K"].ToString() + " 改为:" + ConsigneePhone.Text.Trim();
            //        sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneePhone='" + ConsigneePhone.Text.Trim() + "' "; changes_num++; Sign_num++;
            //        sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneePhone_K='" + ConsigneePhone.Text.Trim() + "' "; changes_num++; Sign_num++;
            //    }
            //    if (dr["ConsigneeName_K"].ToString() != ConsigneeName.Text.Trim())
            //    {
            //        changes_str += "【收货联系人】 由:" + dr["ConsigneeName_K"].ToString() + " 改为:" + ConsigneeName.Text.Trim();
            //        sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneeName='" + ConsigneeName.Text.Trim() + "' "; changes_num++; Sign_num++;
            //        sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneeName_K='" + ConsigneeName.Text.Trim() + "' "; changes_num++; Sign_num++;
            //    }
            //    if (dr["ConsigneeCompany_K"].ToString() != ConsigneeCompany.Text.Trim())
            //    {
            //        changes_str += "【收货公司名称】 由:" + dr["ConsigneeCompany_K"].ToString() + " 改为:" + ConsigneeCompany.Text.Trim();
            //        sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneeCompany='" + ConsigneeCompany.Text.Trim() + "' "; changes_num++; Sign_num++;
            //        sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneeCompany_K='" + ConsigneeCompany.Text.Trim() + "' "; changes_num++; Sign_num++;
            //    }
            //}

            //}

            //if (dr["PickGoodsSite"].ToString() != PickGoodsSite.Text.Trim())
            //{
            //    changes_str += "【目的网点】 由:" + dr["PickGoodsSite"].ToString() + " 改为:" + PickGoodsSite.Text.Trim();
            //    sqlstr += (changes_num > 0 ? " , " : " ") + " PickGoodsSite='" + PickGoodsSite.Text.Trim() + "' "; changes_num++; Sign_num++;
            //}

            //满足3吨8方的条件会修改目的网点 mengdi 2017-05-20
            if (!string.IsNullOrEmpty(NewPickGoodsSite))
            {
                if (dr["PickGoodsSite"].ToString() != NewPickGoodsSite)
                {
                    changes_str += "【目的网点】 由:" + dr["PickGoodsSite"].ToString() + " 改为:" + NewPickGoodsSite;
                    sqlstr += (changes_num > 0 ? " , " : " ") + " PickGoodsSite='" + NewPickGoodsSite + "' "; changes_num++; OtherInfo++;
                }
            }
            else
            {
                if (dr["PickGoodsSite"].ToString() != PickGoodsSite.Text.Trim())
                {
                    changes_str += "【目的网点】 由:" + dr["PickGoodsSite"].ToString() + " 改为:" + PickGoodsSite.Text.Trim();
                    sqlstr += (changes_num > 0 ? " , " : " ") + " PickGoodsSite='" + PickGoodsSite.Text.Trim() + "' "; changes_num++; OtherInfo++;
                }
            }

            if (dr["ReceivProvince"].ToString() != Province.Text.Trim())
            {
                changes_str += "【收货省】 由:" + dr["ReceivProvince"].ToString() + " 改为:" + Province.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ReceivProvince='" + Province.Text.Trim() + "' "; changes_num++; Sign_num++;
            }
            if (dr["ReceivCity"].ToString() != City.Text.Trim())
            {
                changes_str += "【收货市】 由:" + dr["ReceivCity"].ToString() + " 改为:" + City.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ReceivCity='" + City.Text.Trim() + "' "; changes_num++; Sign_num++;
            }
            if (dr["ReceivArea"].ToString() != ReceivArea1)
            {
                changes_str += "【收货区】 由:" + dr["ReceivArea"].ToString() + " 改为:" + ReceivArea1;
                sqlstr += (changes_num > 0 ? " , " : " ") + " ReceivArea='" + ReceivArea1 + "' "; changes_num++; Sign_num++;
            }
            if (dr["ReceivStreet"].ToString() != ReceivStreet.Text.Trim())
            {
                changes_str += "【收货街道】 由:" + dr["ReceivStreet"].ToString() + " 改为:" + ReceivStreet.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ReceivStreet='" + ReceivStreet.Text.Trim() + "' "; changes_num++; Sign_num++;
            }
            if (dr["ReceivAddress"].ToString() != Address.Text.Trim())
            {
                //string billstate = dr["billstate"].ToString();//HJ20180502
                //if (billstate == "1" || billstate == "3" || billstate == "9" || billstate == "14")
                //{
                //    MsgBox.ShowError("货物在“短驳、再短驳、中转、送货”运单状态时不能修改收货地址!");
                //    ReceivProvince.Text = dr["ReceivProvince"].ToString();
                //    ReceivCity.Text = dr["ReceivCity"].ToString();
                //    ReceivArea.Text = dr["ReceivArea"].ToString();
                //    ReceivStreet.Text = dr["ReceivStreet"].ToString();
                //    ReceivAddress.Text = dr["ReceivAddress"].ToString();
                //    return;
                //}
                //if (billstate == "5" || billstate == "6" || billstate == "10" || billstate == "12")
                //{
                //    if (dr["TransferSite"].ToString() != TransferSite.Text.Trim())
                //    {
                //        MsgBox.ShowError("货物在“发车、又发、转送到网点、又转送到网点”运单状态时,只能修改同属于中转地以下的地址!");
                //        ReceivProvince.Text = dr["ReceivProvince"].ToString();
                //        ReceivCity.Text = dr["ReceivCity"].ToString();
                //        ReceivArea.Text = dr["ReceivArea"].ToString();
                //        ReceivStreet.Text = dr["ReceivStreet"].ToString();
                //        ReceivAddress.Text = dr["ReceivAddress"].ToString();
                //        return;
                //    }
                //}
                changes_str += "【收货详细地址】 由:" + dr["ReceivAddress"].ToString() + " 改为:" + Address.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ReceivAddress='" + Address.Text.Trim() + "' "; changes_num++; Sign_num++;
            }
            if (dr["ConsignorCellPhone"].ToString() != ConsignorCellPhone.Text.Trim())
            {
                changes_str += "【发货人手机】 由:" + dr["ConsignorCellPhone"].ToString() + " 改为:" + ConsignorCellPhone.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ConsignorCellPhone='" + ConsignorCellPhone.Text.Trim() + "' "; changes_num++; Sign_num++;
            }
            if (dr["ConsignorPhone"].ToString() != ConsignorPhone.Text.Trim())
            {
                changes_str += "【发货人座机】 由:" + dr["ConsignorPhone"].ToString() + " 改为:" + ConsignorPhone.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ConsignorPhone='" + ConsignorPhone.Text.Trim() + "' "; changes_num++; Sign_num++;
            }
            if (dr["ConsignorName"].ToString() != ConsignorName.Text.Trim())
            {
                changes_str += "【发货联系人】 由:" + dr["ConsignorName"].ToString() + " 改为:" + ConsignorName.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ConsignorName='" + ConsignorName.Text.Trim() + "' "; changes_num++; Sign_num++;
            }
            if (dr["ConsignorCompany"].ToString() != ConsignorCompany.Text.Trim())
            {
                changes_str += "【发货公司名称】 由:" + dr["ConsignorCompany"].ToString() + " 改为:" + ConsignorCompany.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ConsignorCompany='" + ConsignorCompany.Text.Trim() + "' "; changes_num++; Sign_num++;
            }
            //if (dr["ReceivMode"].ToString() != ReceivOrderNo1.Text.Trim())
            //{
            //    changes_str += "【接货方式】 由:" + dr["ReceivMode"].ToString() + " 改为:" + ReceivOrderNo1.Text.Trim();
            //    sqlstr += (changes_num > 0 ? " , " : " ") + " ReceivMode='" + ReceivOrderNo1.Text.Trim() + "' "; changes_num++; Sign_num++;
            //}


            if (dr["ValuationType"].ToString() != ValuationType.Text.Trim())
            {
                changes_str += "【计价类型】 由:" + dr["ValuationType"].ToString() + " 改为:" + ValuationType.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ValuationType='" + ValuationType.Text.Trim() + "' "; changes_num++; Sign_num++;
            }
            if (dr["ReceivOrderNo"].ToString() != ReceivOrderNo.Text.Trim())
            {
                changes_str += "【接货单号】 由:" + dr["ReceivOrderNo"].ToString() + " 改为:" + ReceivOrderNo.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ReceivOrderNo='" + ReceivOrderNo.Text.Trim() + "' "; changes_num++; Sign_num++;
            }
            if (dr["Salesman"].ToString() != Salesman.Text.Trim())
            {
                changes_str += "【业务员】 由:" + dr["Salesman"].ToString() + " 改为:" + Salesman.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " Salesman='" + Salesman.Text.Trim() + "' "; changes_num++; Sign_num++;
            }
            if (dr["AlienGoods"].ToString() != (AlienGoods.Checked ? "1" : "0"))
            {
                changes_str += "【异形货物】 由:" + dr["AlienGoods"].ToString() + " 改为:" + (AlienGoods.Checked ? "1" : "0");
                sqlstr += (changes_num > 0 ? " , " : " ") + " AlienGoods='" + (AlienGoods.Checked ? 1 : 0) + "' "; changes_num++; Sign_num++;
            }
            if (dr["GoodsVoucher"].ToString() != (GoodsVoucher.Checked ? "1" : "0"))
            {
                changes_str += "【货物单据】 由:" + dr["GoodsVoucher"].ToString() + " 改为:" + (GoodsVoucher.Checked ? "1" : "0");
                sqlstr += (changes_num > 0 ? " , " : " ") + " GoodsVoucher=" + (GoodsVoucher.Checked ? 1 : 0) + " "; changes_num++; Sign_num++;
            }
            if (dr["PreciousGoods"].ToString() != (PreciousGoods.Checked ? "1" : "0"))
            {
                changes_str += "【【必走货】】 由:" + dr["PreciousGoods"].ToString() + " 改为:" + (PreciousGoods.Checked ? "1" : "0");
                sqlstr += (changes_num > 0 ? " , " : " ") + " PreciousGoods='" + (PreciousGoods.Checked ? 1 : 0) + "' "; changes_num++; Sign_num++;
            }

            if (dr["GoodsType"].ToString() != GoodsType)
            {
                changes_str += "【货物类型】 由:" + dr["GoodsType"].ToString() + " 改为:" + GoodsType;
                sqlstr += (changes_num > 0 ? " , " : " ") + " GoodsType='" + GoodsType + "' "; changes_num++; Sign_num++;
            }
            if (dr["Varieties"].ToString() != Varieties)
            {
                changes_str += "【品名】 由:" + dr["Varieties"].ToString() + " 改为:" + Varieties;
                sqlstr += (changes_num > 0 ? " , " : " ") + " Varieties='" + Varieties + "' "; changes_num++; Sign_num++;
            }
            if (dr["Package"].ToString() != Package)
            {
                changes_str += "【包装】 由:" + dr["Package"].ToString() + " 改为:" + Package;
                sqlstr += (changes_num > 0 ? " , " : " ") + " Package='" + Package + "' "; changes_num++; Sign_num++;
            }
            if (ConvertType.ToDecimal(dr["Num"]) != Num)
            {
                //changes_str += "【件数】 由:" + dr["Num"].ToString() + " 改为:" + Num.ToString();
                //sqlstr += (changes_num > 0 ? " , " : " ") + " Num='" + Num.ToString() + "' "; changes_num++; Sign_num++;

                //int BillState = ConvertType.ToInt32(dsModified.Tables[0].Rows[0]["BillState"]);
                //int LeftNum = ConvertType.ToInt32(dsModified.Tables[0].Rows[0]["LeftNum"]);
                //if (BillState < 5)
                //{
                //    sqlstr += (changes_num > 0 ? " , " : " ") + " LeftNum='" + Num.ToString() + "' "; changes_num++; Sign_num++;
                //}
                //else if (BillState >= 5 && LeftNum > 0)
                //{
                //    int SendNum = ConvertType.ToInt32(dsModified.Tables[2].Rows[0]["Num"]);
                //    sqlstr += (changes_num > 0 ? " , " : " ") + " LeftNum='" + (Num - SendNum) + "' "; changes_num++; Sign_num++;
                //}
                int BillState = ConvertType.ToInt32(dsModified.Tables[0].Rows[0]["BillState"]);
                int LeftNum = ConvertType.ToInt32(dsModified.Tables[0].Rows[0]["LeftNum"]);
                if (BillState <= 5)  //maohui2018080903(发车状态下，不能修改件数)
                {
                    changes_str += "【件数】 由:" + dr["Num"].ToString() + " 改为:" + Num.ToString();
                    sqlstr += (changes_num > 0 ? " , " : " ") + " Num='" + Num.ToString() + "' "; changes_num++; Sign_num++;
                    if (BillState < 5)
                    {
                        sqlstr += (changes_num > 0 ? " , " : " ") + " LeftNum='" + Num.ToString() + "' "; changes_num++;
                    }
                    else if (BillState >= 5 && LeftNum > 0)
                    {
                        int SendNum = ConvertType.ToInt32(dsModified.Tables[2].Rows[0]["Num"]);
                        sqlstr += (changes_num > 0 ? " , " : " ") + " LeftNum='" + (Num - SendNum) + "' "; changes_num++;
                    }
                }
                else
                {
                    MsgBox.ShowError("货物已发车，不能修改件数！");
                    gridView2.SetRowCellValue(0, "Num", num_old);
                    gridView2.UpdateCurrentRow();
                    labelControl53.Focus();
                    return;
                }
            }
            //if (dr["LeftNum"].ToString() != Num.ToString())
            //{
            //    changes_str += "【剩余件数】 由:" + dr["LeftNum"].ToString() + " 改为:" + Num.ToString();
            //    sqlstr += (changes_num > 0 ? " , " : " ") + " LeftNum='" + Num.ToString() + "' "; changes_num++;
            //}
            if (ConvertType.ToDecimal(dr["FeeWeight"]) != FeeWeight)
            {
                changes_str += "【计费重量】 由:" + dr["FeeWeight"].ToString() + " 改为:" + FeeWeight.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " FeeWeight='" + FeeWeight.ToString() + "' "; changes_num++; Sign_num++;
            }
            if (ConvertType.ToDecimal(dr["FeeVolume"]) != FeeVolume)
            {
                changes_str += "【计费体积】 由:" + dr["FeeVolume"].ToString() + " 改为:" + FeeVolume.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " FeeVolume='" + FeeVolume.ToString() + "' "; changes_num++; Sign_num++;
            }
            if (ConvertType.ToDecimal(dr["Weight"]) != Weight)
            {
                changes_str += "【重量】 由:" + dr["Weight"].ToString() + " 改为:" + Weight.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " Weight='" + Weight.ToString() + "' "; changes_num++; Sign_num++;
            }
            if (ConvertType.ToDecimal(dr["Volume"]) != Volume)
            {
                changes_str += "【体积】 由:" + dr["Volume"].ToString() + " 改为:" + Volume.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " Volume='" + Volume.ToString() + "' "; changes_num++; Sign_num++;
            }
            if (ConvertType.ToDecimal(dr["WeightPrice"]) != WeightPrice)
            {
                changes_str += "【重量单价】 由:" + dr["WeightPrice"].ToString() + " 改为:" + WeightPrice.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " WeightPrice='" + WeightPrice.ToString() + "' "; changes_num++; Sign_num++;
            }
            if (ConvertType.ToDecimal(dr["VolumePrice"]) != VolumePrice)
            {
                changes_str += "【体积单价】 由:" + dr["VolumePrice"].ToString() + " 改为:" + VolumePrice.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " VolumePrice='" + VolumePrice.ToString() + "' "; changes_num++; Sign_num++;
            }
            if (dr["FeeType"].ToString() != FeeType.ToString())
            {
                changes_str += "【计费类型】 由:" + dr["FeeType"].ToString() + " 改为:" + FeeType.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " FeeType='" + FeeType.ToString() + "' "; changes_num++; Sign_num++;
            }
            if (ConvertType.ToDecimal(dr["Freight"]) != Freight)
            {
                changes_str += "【基本运费】 由:" + dr["Freight"].ToString() + " 改为:" + Freight.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " Freight='" + Freight.ToString() + "' "; changes_num++; //Sign_num++;(maohui20180801)
            }
            if (ConvertType.ToDecimal(dr["DeliFee"]) != DeliFee)
            {
                changes_str += "【送货费】 由:" + dr["DeliFee"].ToString() + " 改为:" + DeliFee.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " DeliFee='" + DeliFee.ToString() + "' "; changes_num++; //Sign_num++;(maohui20180801)
            }
            if (ConvertType.ToDecimal(dr["ReceivFee"]) != ReceivFee)
            {
                changes_str += "【接货费】 由:" + dr["ReceivFee"].ToString() + " 改为:" + ReceivFee.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ReceivFee='" + ReceivFee.ToString() + "' "; changes_num++; //Sign_num++;(maohui20180801)
            }
            if (ConvertType.ToDecimal(dr["DeclareValue"]) != DeclareValue)
            {
                changes_str += "【声明价值】 由:" + dr["DeclareValue"].ToString() + " 改为:" + DeclareValue.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " DeclareValue='" + DeclareValue.ToString() + "' "; changes_num++; Sign_num++;
            }
            if (ConvertType.ToDecimal(dr["SupportValue"]) != SupportValue)
            {
                changes_str += "【保价费】 由:" + dr["SupportValue"].ToString() + " 改为:" + SupportValue.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " SupportValue='" + SupportValue.ToString() + "' "; changes_num++; //Sign_num++;(maohui20180801)
            }
            if (ConvertType.ToDecimal(dr["Tax"]) != Tax)
            {
                changes_str += "【税金】 由:" + dr["Tax"].ToString() + " 改为:" + Tax.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " Tax='" + Tax.ToString() + "' "; changes_num++; //Sign_num++;(maohui20180801)
            }
            if (ConvertType.ToDecimal(dr["InformationFee"]) != InformationFee)
            {
                changes_str += "【信息费】 由:" + dr["InformationFee"].ToString() + " 改为:" + InformationFee.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " InformationFee='" + InformationFee.ToString() + "' "; changes_num++; //Sign_num++;(maohui20180801)
            }
            if (ConvertType.ToDecimal(dr["Expense"]) != Expense)
            {
                changes_str += "【工本费】 由:" + dr["Expense"].ToString() + " 改为:" + Expense.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " Expense='" + Expense.ToString() + "' "; changes_num++; //Sign_num++;(maohui20180801)
            }
            if (ConvertType.ToDecimal(dr["NoticeFee"]) != NoticeFee)
            {
                changes_str += "【等通知放货费】 由:" + dr["NoticeFee"].ToString() + " 改为:" + NoticeFee.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " NoticeFee='" + NoticeFee.ToString() + "' "; changes_num++; //Sign_num++;(maohui20180801)
            }
            if (ConvertType.ToDecimal(dr["DiscountTransfer"]) != DiscountTransfer)
            {
                //20180803，LD，唐克辉注释
                if (ds1.Tables[0].Rows[0]["DisTranVerifState"].ToString() != "")
                {
                    if (Convert.ToInt32(ds1.Tables[0].Rows[0]["DisTranVerifState"]) == 1) //毛慧20171229
                    {
                        MsgBox.ShowError("此单折扣折让在回扣核销中已核销，请先反核销再申请改单！");
                        return;
                    }
                }
                changes_str += "【折扣折让】 由:" + dr["DiscountTransfer"].ToString() + " 改为:" + DiscountTransfer.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " DiscountTransfer='" + DiscountTransfer.ToString() + "' "; changes_num++; //Sign_num++;(maohui20180801)
            }
            if (ConvertType.ToDecimal(dr["CollectionPay"]) != CollectionPay)
            {
                //20180803，LD，唐克辉注释
                if (ds1.Tables[0].Rows[0]["CollectionPayBackState"].ToString() != "")  //maohui20180510
                {
                    if (Convert.ToInt32(ds1.Tables[0].Rows[0]["CollectionPayBackState"]) == 1)
                    {
                        MsgBox.ShowError("此单代收货款已核销，请先反核销再申请改单！");
                        return;
                    }
                }
                changes_str += "【代收货款】 由:" + dr["CollectionPay"].ToString() + " 改为:" + CollectionPay.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " CollectionPay='" + CollectionPay.ToString() + "' "; changes_num++; //Sign_num++;(maohui20180801)
            }
            if (ConvertType.ToDecimal(dr["AgentFee"]) != AgentFee)
            {
                changes_str += "【代收手续费】 由:" + dr["AgentFee"].ToString() + " 改为:" + AgentFee.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " AgentFee='" + AgentFee.ToString() + "' "; changes_num++; //Sign_num++;(maohui20180801)
            }
            if (ConvertType.ToDecimal(dr["FuelFee"]) != FuelFee)
            {
                changes_str += "【燃油费】 由:" + dr["FuelFee"].ToString() + " 改为:" + FuelFee.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " FuelFee='" + FuelFee.ToString() + "' "; changes_num++; //Sign_num++;(maohui20180801)
            }
            if (ConvertType.ToDecimal(dr["UpstairFee"]) != UpstairFee)
            {
                changes_str += "【上楼费】 由:" + dr["UpstairFee"].ToString() + " 改为:" + UpstairFee.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " UpstairFee='" + UpstairFee.ToString() + "' "; changes_num++; //Sign_num++;(maohui20180801)
            }
            if (ConvertType.ToDecimal(dr["HandleFee"]) != HandleFee)
            {
                changes_str += "【装卸费】 由:" + dr["HandleFee"].ToString() + " 改为:" + HandleFee.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " HandleFee='" + HandleFee.ToString() + "' "; changes_num++; //Sign_num++;(maohui20180801)
            }
            if (ConvertType.ToDecimal(dr["ForkliftFee"]) != ForkliftFee)
            {
                changes_str += "【叉车费】 由:" + dr["ForkliftFee"].ToString() + " 改为:" + ForkliftFee.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ForkliftFee='" + ForkliftFee.ToString() + "' "; changes_num++; //Sign_num++;(maohui20180801)
            }
            if (ConvertType.ToDecimal(dr["StorageFee"]) != StorageFee)
            {
                changes_str += "【进仓费】 由:" + dr["StorageFee"].ToString() + " 改为:" + StorageFee.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " StorageFee='" + StorageFee.ToString() + "' "; changes_num++; //Sign_num++;(maohui20180801)
            }
            if (ConvertType.ToDecimal(dr["CustomsFee"]) != CustomsFee)
            {
                changes_str += "【报关费】 由:" + dr["CustomsFee"].ToString() + " 改为:" + CustomsFee.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " CustomsFee='" + CustomsFee.ToString() + "' "; changes_num++; //Sign_num++;(maohui20180801)
            }
            //HJ20180911
            if (ConvertType.ToDecimal(dr["MatPay"]) != MatPay)
            {
                changes_str += "【垫付费】 由:" + dr["MatPay"].ToString() + " 改为:" + MatPay.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " MatPay='" + MatPay.ToString() + "' "; changes_num++; //Sign_num++;(maohui20180801)
            }
            if (ConvertType.ToDecimal(dr["packagFee"]) != packagFee)
            {
                changes_str += "【包装费】 由:" + dr["packagFee"].ToString() + " 改为:" + packagFee.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " packagFee='" + packagFee.ToString() + "' "; changes_num++; //Sign_num++;(maohui20180801)
            }
            if (ConvertType.ToDecimal(dr["FrameFee"]) != FrameFee)
            {
                changes_str += "【代打木架费】 由:" + dr["FrameFee"].ToString() + " 改为:" + FrameFee.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " FrameFee='" + FrameFee.ToString() + "' "; changes_num++; //Sign_num++;(maohui20180801)
            }
            if (ConvertType.ToDecimal(dr["ChangeFee"]) != ChangeFee)
            {
                changes_str += "【改单费】 由:" + dr["ChangeFee"].ToString() + " 改为:" + ChangeFee.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ChangeFee='" + ChangeFee.ToString() + "' "; changes_num++; //Sign_num++;(maohui20180801)
            }
            if (ConvertType.ToDecimal(dr["OtherFee"]) != OtherFee)
            {
                changes_str += "【其它费】 由:" + dr["OtherFee"].ToString() + " 改为:" + OtherFee.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " OtherFee='" + OtherFee.ToString() + "' "; changes_num++; //Sign_num++;(maohui20180801)
            }
            if (ConvertType.ToDecimal(dr["ReceiptFee"]) != ReceiptFee)
            {
                changes_str += "【回单费】 由:" + dr["ReceiptFee"].ToString() + " 改为:" + ReceiptFee.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ReceiptFee='" + ReceiptFee.ToString() + "' "; changes_num++; //Sign_num++;(maohui20180801)
            }
            if (ConvertType.ToDecimal(dr["WarehouseFee"]) != WarehouseFee)
            {
                changes_str += "【仓储费】 由:" + dr["WarehouseFee"].ToString() + " 改为:" + WarehouseFee.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " WarehouseFee='" + WarehouseFee.ToString() + "' "; changes_num++; //Sign_num++;(maohui20180801)
            }

            if (dr["ReceiptCondition"].ToString() != ReceiptCondition.Text.ToString())
            {
                changes_str += "【回单要求】 由:" + dr["ReceiptCondition"].ToString() + " 改为:" + ReceiptCondition.Text.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ReceiptCondition='" + ReceiptCondition.Text.Trim() + "' "; changes_num++; Sign_num++;
            }


            if (dr["PaymentMode"].ToString() != PaymentMode.Text.ToString())
            {
                if (dr["BillState"].ToString() == "16") //hj20180503
                {
                    //20180803，LD，唐克辉注释
                    if ((PaymentMode.Text.ToString() != "月结" && PaymentMode.Text.ToString() != "回单付" && PaymentMode.Text.ToString() != "货到前付" && PaymentMode.Text.ToString() != "欠付") && IsAccept > 0)
                    {
                        MsgBox.ShowError("签收状态的运单，并已被转分拨接收，只能修改付款方式为月结，回单付，货到前付，欠付!");
                        return;
                    }

                    if (dr["PaymentMode"].ToString() == "现付")
                    {
                        MsgBox.ShowError("此运单为签收状态，现付不能修改付款方式!");
                        return;
                    }
                }
                //hj 分拨的运单不能修改付款方式为回单付,货到前付,欠付
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_FBState_1", new List<SqlPara> { new SqlPara("BillNo", BillNO) }));
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    //20180803，LD，唐克辉注释
                    if ((PaymentMode.Text.ToString() == "回单付" || PaymentMode.Text.ToString() == "货到前付" || PaymentMode.Text.ToString() == "欠付") && IsAccept > 0)
                    {
                        MsgBox.ShowError("分拨到中心的运单，并已被转分拨接收，不能修改付款方式为回单付，货到前付，欠付!");
                        return;
                    }
                }

                changes_str += "【付款方式】 由:" + dr["PaymentMode"].ToString() + " 改为:" + PaymentMode.Text.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " PaymentMode='" + PaymentMode.Text.Trim() + "' "; changes_num++;
            }
            if (dr["PaymentAmout"].ToString() != (PaymentAmout.Text.Trim() == "" ? "0" : PaymentAmout.Text.Trim()))
            {
                changes_str += "【总运费】 由:" + dr["PaymentAmout"].ToString() + " 改为:" + (PaymentAmout.Text.Trim() == "" ? "0" : PaymentAmout.Text.Trim());
                sqlstr += (changes_num > 0 ? " , " : " ") + " PaymentAmout='" + (PaymentAmout.Text.Trim() == "" ? "0" : PaymentAmout.Text.Trim()) + "' "; changes_num++; //Sign_num++;(maohui20180801)
            }
            if (ConvertType.ToDecimal(dr["ActualFreight"].ToString()) != ActualFreight)
            {
                changes_str += "【实收金额】 由:" + dr["ActualFreight"].ToString() + " 改为:" + ActualFreight;
                sqlstr += (changes_num > 0 ? " , " : " ") + " ActualFreight='" + ActualFreight + "' "; changes_num++; //Sign_num++;(maohui20180801)
            }

            if (ConvertType.ToDecimal(dr["NowPay"]) != ConvertType.ToDecimal(NowPay.Text))
            {
                if (ds1.Tables[1].Rows[0]["NowPayState"].ToString() != "")
                {
                    if (Convert.ToInt32(ds1.Tables[1].Rows[0]["NowPayState"]) == 1) //毛慧20171229
                    {
                        MsgBox.ShowError("此单现付费用已核销，请先反核销再申请改单！");
                        return;
                    }
                }
                if (dr["BillState"].ToString() == "16" && IsAccept > 0)  //hj20180503
                {
                    MsgBox.ShowError("此单已经签收，并已被转分拨接收，现付费用不能修改!");
                    return;
                }
                string s = NowPay.Text.ToString();
                s = s == "" ? "0" : s;
                changes_str += "【现付】 由:" + dr["NowPay"].ToString() + " 改为:" + s;
                sqlstr += (changes_num > 0 ? " , " : " ") + " NowPay='" + s + "' "; changes_num++; //Sign_num++;(maohui20180801)
            }
            if (ConvertType.ToDecimal(dr["FetchPay"]) != ConvertType.ToDecimal(FetchPay.Text.Trim()))
            {
                //20180803，LD，唐克辉注释
                if (ds1.Tables[1].Rows[0]["FetchPayState"].ToString() != "")
                {
                    if (Convert.ToInt32(ds1.Tables[1].Rows[0]["FetchPayState"]) == 1) //毛慧20171229
                    {
                        MsgBox.ShowError("此单提付费用已在提付核销中核销，请先反核销再申请改单！");
                        return;
                    }
                }
                changes_str += "【提付】 由:" + dr["FetchPay"].ToString() + " 改为:" + (FetchPay.Text.Trim() == "" ? "0" : FetchPay.Text.Trim());
                sqlstr += (changes_num > 0 ? " , " : " ") + " FetchPay='" + (FetchPay.Text.Trim() == "" ? "0" : FetchPay.Text.Trim()) + "' "; changes_num++;//Sign_num++;(maohui20180801)
            }
            if (ConvertType.ToDecimal(dr["MonthPay"]) != ConvertType.ToDecimal(MonthPay.Text))
            {
                //20180803，LD，唐克辉注释
                if (ds1.Tables[1].Rows[0]["MonthPayState"].ToString() != "")
                {
                    if (Convert.ToInt32(ds1.Tables[1].Rows[0]["MonthPayState"]) == 1) //毛慧20171229
                    {
                        MsgBox.ShowError("此单月结费用已在月结核销中核销，请先反核销再申请改单！");
                        return;
                    }
                }

                changes_str += "【月结】 由:" + dr["MonthPay"].ToString() + " 改为:" + (MonthPay.Text.Trim() == "" ? "0" : MonthPay.Text.Trim());
                sqlstr += (changes_num > 0 ? " , " : " ") + " MonthPay='" + (MonthPay.Text.Trim() == "" ? "0" : MonthPay.Text.Trim()) + "' "; changes_num++;//Sign_num++;(maohui20180801)
            }
            if (ConvertType.ToDecimal(dr["ShortOwePay"]) != ConvertType.ToDecimal(ShortOwePay.Text))
            {
                changes_str += "【短欠】 由:" + dr["ShortOwePay"].ToString() + " 改为:" + (ShortOwePay.Text.Trim() == "" ? "0" : ShortOwePay.Text.Trim());
                sqlstr += (changes_num > 0 ? " , " : " ") + " ShortOwePay='" + (ShortOwePay.Text.Trim() == "" ? "0" : ShortOwePay.Text.Trim()) + "' "; changes_num++; Sign_num++;
            }
            if (ConvertType.ToDecimal(dr["BefArrivalPay"]) != ConvertType.ToDecimal(BefArrivalPay.Text))
            {
                //20180803，LD，唐克辉注释
                if (ds1.Tables[1].Rows[0]["BefArrivalPayVerifState"].ToString() != "")
                {
                    if (Convert.ToInt32(ds1.Tables[1].Rows[0]["BefArrivalPayVerifState"]) == 1) //毛慧20171229
                    {
                        MsgBox.ShowError("此单货到前付费用已在货到前付核销中核销，请先反核销再申请改单！");
                        return;
                    }
                }
                changes_str += "【货到前付】 由:" + dr["BefArrivalPay"].ToString() + " 改为:" + (BefArrivalPay.Text.Trim() == "" ? "0" : BefArrivalPay.Text.Trim());
                sqlstr += (changes_num > 0 ? " , " : " ") + " BefArrivalPay='" + (BefArrivalPay.Text.Trim() == "" ? "0" : BefArrivalPay.Text.Trim()) + "' "; changes_num++;//Sign_num++;(maohui20180801)
            }
            //zaj 2017-11-22
            if (ConvertType.ToDecimal(dr["ReceiptPay"]) != ConvertType.ToDecimal(ReceiptPay.Text))
            {
                //20180803，LD，唐克辉注释
                if (ds1.Tables[1].Rows[0]["ReceiptPayVerifState"].ToString() != "")
                {
                    if (Convert.ToInt32(ds1.Tables[1].Rows[0]["ReceiptPayVerifState"]) == 1) //毛慧20171229
                    {
                        MsgBox.ShowError("此单回单付费用已在回单付核销中核销，请先反核销再申请改单！");
                        return;
                    }
                }
                changes_str += "【回单付】 由:" + dr["ReceiptPay"].ToString() + " 改为:" + (ReceiptPay.Text.Trim() == "" ? "0" : ReceiptPay.Text.Trim());
                sqlstr += (changes_num > 0 ? " , " : " ") + " ReceiptPay='" + (ReceiptPay.Text.Trim() == "" ? "0" : ReceiptPay.Text.Trim()) + "' "; changes_num++;//Sign_num++;(maohui20180801)
            }
            if (ConvertType.ToDecimal(dr["OwePay"]) != ConvertType.ToDecimal(OwePay.Text))
            {
                //20180803，LD，唐克辉注释
                if (ds1.Tables[1].Rows[0]["OwePayVerifState"].ToString() != "")
                {
                    if (Convert.ToInt32(ds1.Tables[1].Rows[0]["OwePayVerifState"]) == 1) //毛慧20171229
                    {
                        MsgBox.ShowError("此单欠付费用已在欠付核销中核销，请先反核销再申请改单！");
                        return;
                    }
                }
                changes_str += "【欠付】 由:" + dr["OwePay"].ToString() + " 改为:" + (OwePay.Text.Trim() == "" ? "0" : OwePay.Text.Trim());
                sqlstr += (changes_num > 0 ? " , " : " ") + " OwePay='" + (OwePay.Text.Trim() == "" ? "0" : OwePay.Text.Trim()) + "' "; changes_num++;//Sign_num++;(maohui20180801)
            }
            if (dr["BillRemark"].ToString() != BillRemark.Text.Trim())
            {
                changes_str += "【备注】 由:" + dr["BillRemark"].ToString() + " 改为:" + BillRemark.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " BillRemark='" + BillRemark.Text.Trim() + "' "; changes_num++; Sign_num++;
            }
            if (dr["WebPlatform"].ToString() != WebPlatform.Text.Trim())
            {
                changes_str += "【网络平台】 由:" + dr["WebPlatform"].ToString() + " 改为:" + WebPlatform.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " WebPlatform='" + WebPlatform.Text.Trim() + "' "; changes_num++; Sign_num++;
            }
            if (dr["WebBillNo"].ToString() != WebBillNo.Text.Trim())
            {
                changes_str += "【网络订单号】 由:" + dr["WebBillNo"].ToString() + " 改为:" + WebBillNo.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " WebBillNo='" + WebBillNo.Text.Trim() + "' "; changes_num++; Sign_num++;
            }
            if (dr["DisTranName"].ToString() != DisTranName.Text.Trim())
            {
                changes_str += "【折扣折让收款名称】 由:" + dr["DisTranName"].ToString() + " 改为:" + DisTranName.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " DisTranName='" + DisTranName.Text.Trim() + "' "; changes_num++; Sign_num++;
            }
            if (dr["DisTranBank"].ToString() != DisTranBank.Text.Trim())
            {
                changes_str += "【折扣折让收款银行】 由:" + dr["DisTranBank"].ToString() + " 改为:" + DisTranBank.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " DisTranBank='" + DisTranBank.Text.Trim() + "' "; changes_num++; Sign_num++;
            }
            if (dr["DisTranAccount"].ToString() != DisTranAccount.Text.Trim())
            {
                changes_str += "【折扣折让收款账号】 由:" + dr["DisTranAccount"].ToString() + " 改为:" + DisTranAccount.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " DisTranAccount='" + DisTranAccount.Text.Trim() + "' "; changes_num++; Sign_num++;
            }
            if (dr["CollectionName"].ToString() != CollectionName.Text.Trim())
            {
                changes_str += "【代收货款收款名称】 由:" + dr["CollectionName"].ToString() + " 改为:" + CollectionName.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " CollectionName='" + CollectionName.Text.Trim() + "' "; changes_num++; Sign_num++;
            }
            if (dr["CollectionBank"].ToString() != CollectionBank.Text.Trim())
            {
                changes_str += "【代收货款收款银行】 由:" + dr["CollectionBank"].ToString() + " 改为:" + CollectionBank.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " CollectionBank='" + CollectionBank.Text.Trim() + "' "; changes_num++; Sign_num++;
            }
            if (dr["CollectionAccount"].ToString() != CollectionAccount.Text.Trim())
            {
                changes_str += "【代收货款收款账号】 由:" + dr["CollectionAccount"].ToString() + " 改为:" + CollectionAccount.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " CollectionAccount='" + CollectionAccount.Text.Trim() + "' "; changes_num++; Sign_num++;
            }
            if (dr["CauseName"].ToString() != CauseName.Text.Trim())
            {
                changes_str += "【开单事业部】 由:" + dr["CauseName"].ToString() + " 改为:" + CauseName.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " CauseName='" + CauseName.Text.Trim() + "' "; changes_num++; Sign_num++;
            }
            if (dr["AreaName"].ToString() != AreaName.Text.Trim())
            {
                changes_str += "【开单大区】 由:" + dr["AreaName"].ToString() + " 改为:" + AreaName.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " AreaName='" + AreaName.Text.Trim() + "' "; changes_num++; Sign_num++;
            }
            if (dr["DepName"].ToString() != DepName.Text.Trim())
            {
                changes_str += "【开单部门】 由:" + dr["DepName"].ToString() + " 改为:" + DepName.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " DepName='" + DepName.Text.Trim() + "' "; changes_num++; Sign_num++;
            }
            if (dr["DisTranBranch"].ToString() != DisTranBranch.Text.Trim())
            {
                changes_str += "【折扣折让所属支行】 由:" + dr["DisTranBranch"].ToString() + " 改为:" + DisTranBranch.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " DisTranBranch='" + DisTranBranch.Text.Trim() + "' "; changes_num++; Sign_num++;
            }
            if (dr["CollectionBranch"].ToString() != CollectionBranch.Text.Trim())
            {
                changes_str += "【代收款所属支行】 由:" + dr["CollectionBranch"].ToString() + " 改为:" + CollectionBranch.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " CollectionBranch='" + CollectionBranch.Text.Trim() + "' "; changes_num++; Sign_num++;
            }
            if (dr["BillMan"].ToString() != BillMan.Text.Trim())
            {
                changes_str += "【开单人】 由:" + dr["BillMan"].ToString() + " 改为:" + BillMan.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " BillMan='" + BillMan.Text.Trim() + "' "; changes_num++; Sign_num++;
            }
            if (dr["begWeb"].ToString() != begWeb.Text.Trim())
            {
                changes_str += "【开单网点】 由:" + dr["begWeb"].ToString() + " 改为:" + begWeb.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " begWeb='" + begWeb.Text.Trim() + "' "; changes_num++; Sign_num++;
            }


            int NoticeState_s = NoticeState.Checked ? 1 : 0;
            int IsInvoice_s = IsInvoice.Checked ? 1 : 0;
            int IsReceiptFee_s = IsReceiptFee.Checked ? 1 : 0;
            int IsSupportValue_s = IsSupportValue.Checked ? 1 : 0;
            int IsAgentFee_s = IsAgentFee.Checked ? 1 : 0;

            int IsPackagFee_s = IsPackagFee.Checked ? 1 : 0;
            int IsOtherFee_s = IsOtherFee.Checked ? 1 : 0;
            int IsHandleFee_s = IsHandleFee.Checked ? 1 : 0;
            int IsStorageFee_s = IsStorageFee.Checked ? 1 : 0;
            int IsWarehouseFee_s = IsWarehouseFee.Checked ? 1 : 0;

            int IsForkliftFee_s = IsForkliftFee.Checked ? 1 : 0;
            int IsChangeFee_s = IsChangeFee.Checked ? 1 : 0;
            int IsUpstairFee_s = IsUpstairFee.Checked ? 1 : 0;
            int IsCustomsFee_s = IsCustomsFee.Checked ? 1 : 0;
            int IsFrameFee_s = IsFrameFee.Checked ? 1 : 0;
            int IsMatPay_s = IsMatPay.Checked ? 1 : 0;//HJ20180911
            int IsRebates_s = IsRebates.Checked ? 1 : 0;//HJ20180911

            int NoticeState_c = ConvertType.ToInt32(dr["NoticeState"]);
            int IsInvoice_c = ConvertType.ToInt32(dr["IsInvoice"]);
            int IsReceiptFee_c = ConvertType.ToInt32(dr["IsReceiptFee"]);
            int IsSupportValue_c = ConvertType.ToInt32(dr["IsSupportValue"]);
            int IsAgentFee_c = ConvertType.ToInt32(dr["IsAgentFee"]);

            int IsPackagFee_c = ConvertType.ToInt32(dr["IsPackagFee"]);
            int IsOtherFee_c = ConvertType.ToInt32(dr["IsOtherFee"]);
            int IsHandleFee_c = ConvertType.ToInt32(dr["IsHandleFee"]);
            int IsStorageFee_c = ConvertType.ToInt32(dr["IsStorageFee"]);
            int IsWarehouseFee_c = ConvertType.ToInt32(dr["IsWarehouseFee"]);

            int IsForkliftFee_c = ConvertType.ToInt32(dr["IsForkliftFee"]);
            int IsChangeFee_c = ConvertType.ToInt32(dr["IsChangeFee"]);
            int IsUpstairFee_c = ConvertType.ToInt32(dr["IsUpstairFee"]);
            int IsCustomsFee_c = ConvertType.ToInt32(dr["IsCustomsFee"]);
            int IsMatPay_c = ConvertType.ToInt32(dr["IsMatPay"]);//HJ20180911
            int IsRebates_c = ConvertType.ToInt32(dr["IsRebates"]);//HJ20180911
            int IsFrameFee_c = ConvertType.ToInt32(dr["IsFrameFee"]);


            //luohui
            int IsViewTrace_s = IsViewTraceFee.Checked ? 1 : 0;
            int IsSortFee_s = IsSortFee.Checked ? 1 : 0;
            int IsViewTrace_c = ConvertType.ToInt32(dr["IsViewTrace"].ToString() == "" ? "0" : dr["IsViewTrace"].ToString());
            int IsSortFee_c = ConvertType.ToInt32(dr["IsSortFee"].ToString() == "" ? "0" : dr["IsSortFee"].ToString());
            if (IsViewTrace_s != IsViewTrace_c)
            {
                changes_str += "【视图跟踪】 由:" + (IsViewTrace_c > 0 ? "是" : "否") + " 改为:" + (IsViewTrace_s > 0 ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " IsViewTrace=" + IsViewTrace_s + " "; changes_num++;
            }
            if (IsSortFee_s != IsSortFee_c)
            {
                changes_str += "【分拣】 由:" + (IsSortFee_c > 0 ? "是" : "否") + " 改为:" + (IsSortFee_s > 0 ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " IsSortFee=" + IsSortFee_s + " "; changes_num++;
            }

            if (NoticeState_s != NoticeState_c)
            {
                changes_str += "【控货】 由:" + (NoticeState_c > 0 ? "是" : "否") + " 改为:" + (NoticeState_s > 0 ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " NoticeState=" + NoticeState_s + " "; changes_num++; Sign_num++;
            }
            if (IsInvoice_s != IsInvoice_c)
            {
                changes_str += "【发票】 由:" + (IsInvoice_c > 0 ? "是" : "否") + " 改为:" + (IsInvoice_s > 0 ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " IsInvoice=" + IsInvoice_s + " "; changes_num++; Sign_num++;
            }
            if (IsReceiptFee_s != IsReceiptFee_c)
            {
                changes_str += "【回单】 由:" + (IsReceiptFee_c > 0 ? "是" : "否") + " 改为:" + (IsReceiptFee_s > 0 ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " IsReceiptFee=" + IsReceiptFee_s + " "; changes_num++; Sign_num++;
            }
            if (IsSupportValue_s != IsSupportValue_c)
            {
                changes_str += "【保价】 由:" + (IsSupportValue_c > 0 ? "是" : "否") + " 改为:" + (IsSupportValue_s > 0 ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " IsSupportValue=" + IsSupportValue_s + " "; changes_num++; Sign_num++;
            }
            if (IsAgentFee_s != IsAgentFee_c)
            {
                changes_str += "【代收】 由:" + (IsAgentFee_c > 0 ? "是" : "否") + " 改为:" + (IsAgentFee_s > 0 ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " IsAgentFee=" + IsAgentFee_s + " "; changes_num++; Sign_num++;
            }

            if (IsPackagFee_s != IsPackagFee_c)
            {
                changes_str += "【包装】 由:" + (IsPackagFee_c > 0 ? "是" : "否") + " 改为:" + (IsPackagFee_s > 0 ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " IsPackagFee=" + IsPackagFee_s + " "; changes_num++; Sign_num++;
            }
            if (IsOtherFee_s != IsOtherFee_c)
            {
                changes_str += "【其它】 由:" + (IsOtherFee_c > 0 ? "是" : "否") + " 改为:" + (IsOtherFee_s > 0 ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " IsOtherFee=" + IsOtherFee_s + " "; changes_num++; Sign_num++;
            }
            if (IsHandleFee_s != IsHandleFee_c)
            {
                changes_str += "【装卸】 由:" + (IsHandleFee_c > 0 ? "是" : "否") + " 改为:" + (IsHandleFee_s > 0 ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " IsHandleFee=" + IsHandleFee_s + " "; changes_num++; Sign_num++;
            }
            if (IsStorageFee_s != IsStorageFee_c)
            {
                changes_str += "【进仓】 由:" + (IsStorageFee_c > 0 ? "是" : "否") + " 改为:" + (IsStorageFee_s > 0 ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " IsStorageFee=" + IsStorageFee_s + " "; changes_num++; Sign_num++;
            }
            if (IsWarehouseFee_s != IsWarehouseFee_c)
            {
                changes_str += "【仓储】 由:" + (IsWarehouseFee_c > 0 ? "是" : "否") + " 改为:" + (IsWarehouseFee_s > 0 ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " IsWarehouseFee=" + IsWarehouseFee_s + " "; changes_num++; Sign_num++;
            }

            if (IsForkliftFee_s != IsForkliftFee_c)
            {
                changes_str += "【叉车】 由:" + (IsForkliftFee_c > 0 ? "是" : "否") + " 改为:" + (IsForkliftFee_s > 0 ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " IsForkliftFee=" + IsForkliftFee_s + " "; changes_num++; Sign_num++;
            }
            if (IsChangeFee_s != IsChangeFee_c)
            {
                changes_str += "【改单】 由:" + (IsChangeFee_c > 0 ? "是" : "否") + " 改为:" + (IsChangeFee_s > 0 ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " IsChangeFee=" + IsChangeFee_s + " "; changes_num++; Sign_num++;
            }
            if (IsUpstairFee_s != IsUpstairFee_c)
            {
                changes_str += "【上楼】 由:" + (IsUpstairFee_c > 0 ? "是" : "否") + " 改为:" + (IsUpstairFee_s > 0 ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " IsUpstairFee=" + IsUpstairFee_s + " "; changes_num++; Sign_num++;
            }
            if (IsCustomsFee_s != IsCustomsFee_c)
            {
                changes_str += "【报关】 由:" + (IsCustomsFee_c > 0 ? "是" : "否") + " 改为:" + (IsCustomsFee_s > 0 ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " IsCustomsFee=" + IsCustomsFee_s + " "; changes_num++; Sign_num++;
            }
            //HJ20180911
            if (IsMatPay_s != IsMatPay_c)
            {
                changes_str += "【垫付】 由:" + (IsMatPay_c > 0 ? "是" : "否") + " 改为:" + (IsMatPay_s > 0 ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " IsMatPay=" + IsMatPay_s + " "; changes_num++; Sign_num++;
            }
            //HJ20180911
            if (IsRebates_s != IsRebates_c)
            {
                changes_str += "【回扣已返】 由:" + (IsRebates_c > 0 ? "是" : "否") + " 改为:" + (IsRebates_s > 0 ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " IsRebates=" + IsRebates_s + " "; changes_num++; Sign_num++;
            }
            if (IsFrameFee_s != IsFrameFee_c)
            {
                changes_str += "【木架】 由:" + (IsFrameFee_c > 0 ? "是" : "否") + " 改为:" + (IsFrameFee_s > 0 ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " IsFrameFee=" + IsFrameFee_s + " "; changes_num++; Sign_num++;
            }

            if (ConvertType.ToDecimal(dr["ReceiptFee_C"]) != ReceiptFee_C)
            {
                changes_str += "【结算回单费】 由:" + dr["ReceiptFee_C"].ToString() + " 改为:" + WarehouseFee;
                sqlstr += (changes_num > 0 ? " , " : " ") + " ReceiptFee_C=" + ReceiptFee_C + " "; changes_num++; //Sign_num++;maohui20180802
            }
            if (ConvertType.ToDecimal(dr["NoticeFee_C"]) != NoticeFee_C)
            {
                changes_str += "【结算控货费】 由:" + dr["NoticeFee_C"].ToString() + " 改为:" + NoticeFee_C;
                sqlstr += (changes_num > 0 ? " , " : " ") + " NoticeFee_C=" + NoticeFee_C + " "; changes_num++; //Sign_num++;maohui20180802
            }
            if (ConvertType.ToDecimal(dr["SupportValue_C"]) != SupportValue_C)
            {
                changes_str += "【结算保价费】 由:" + dr["SupportValue_C"].ToString() + " 改为:" + SupportValue_C;
                sqlstr += (changes_num > 0 ? " , " : " ") + " SupportValue_C=" + SupportValue_C + " "; changes_num++; //Sign_num++;maohui20180802
            }
            if (ConvertType.ToDecimal(dr["AgentFee_C"]) != AgentFee_C)
            {
                changes_str += "【结算代收手续费】 由:" + dr["AgentFee_C"].ToString() + " 改为:" + AgentFee_C;
                sqlstr += (changes_num > 0 ? " , " : " ") + " AgentFee_C=" + AgentFee_C + " "; changes_num++; //Sign_num++;maohui20180802
            }
            if (ConvertType.ToDecimal(dr["PackagFee_C"]) != PackagFee_C)
            {
                changes_str += "【结算包装费】 由:" + dr["PackagFee_C"].ToString() + " 改为:" + PackagFee_C;
                sqlstr += (changes_num > 0 ? " , " : " ") + " PackagFee_C=" + PackagFee_C + " "; changes_num++; //Sign_num++;maohui20180802
            }

            if (ConvertType.ToDecimal(dr["OtherFee_C"]) != OtherFee_C)
            {
                changes_str += "【结算其它费】 由:" + dr["OtherFee_C"].ToString() + " 改为:" + OtherFee_C;
                sqlstr += (changes_num > 0 ? " , " : " ") + " OtherFee_C=" + OtherFee_C + " "; changes_num++; //Sign_num++;maohui20180802
            }
            if (ConvertType.ToDecimal(dr["HandleFee_C"]) != HandleFee_C)
            {
                changes_str += "【结算装卸费】 由:" + dr["HandleFee_C"].ToString() + " 改为:" + HandleFee_C;
                sqlstr += (changes_num > 0 ? " , " : " ") + " HandleFee_C=" + HandleFee_C + " "; changes_num++; //Sign_num++;maohui20180802
            }
            if (ConvertType.ToDecimal(dr["StorageFee_C"]) != StorageFee_C)
            {
                changes_str += "【结算进仓费】 由:" + dr["StorageFee_C"].ToString() + " 改为:" + StorageFee_C;
                sqlstr += (changes_num > 0 ? " , " : " ") + " StorageFee_C=" + StorageFee_C + " "; changes_num++; //Sign_num++;maohui20180802
            }
            if (ConvertType.ToDecimal(dr["WarehouseFee_C"]) != WarehouseFee_C)
            {
                changes_str += "【结算仓储费】 由:" + dr["WarehouseFee_C"].ToString() + " 改为:" + WarehouseFee_C;
                sqlstr += (changes_num > 0 ? " , " : " ") + " WarehouseFee_C=" + WarehouseFee_C + " "; changes_num++; //Sign_num++;maohui20180802
            }
            if (ConvertType.ToDecimal(dr["ForkliftFee_C"]) != ForkliftFee_C)
            {
                changes_str += "【结算叉车费】 由:" + dr["ForkliftFee_C"].ToString() + " 改为:" + ForkliftFee_C;
                sqlstr += (changes_num > 0 ? " , " : " ") + " ForkliftFee_C=" + ForkliftFee_C + " "; changes_num++; //Sign_num++;maohui20180802
            }

            if (ConvertType.ToDecimal(dr["Tax_C"]) != Tax_C)
            {
                changes_str += "【结算税金费】 由:" + dr["Tax_C"].ToString() + " 改为:" + Tax_C;
                sqlstr += (changes_num > 0 ? " , " : " ") + " Tax_C=" + Tax_C + " "; changes_num++; //Sign_num++;maohui20180802
            }
            if (ConvertType.ToDecimal(dr["ChangeFee_C"]) != ChangeFee_C)
            {
                changes_str += "【结算改单费】 由:" + dr["ChangeFee_C"].ToString() + " 改为:" + ChangeFee_C;
                sqlstr += (changes_num > 0 ? " , " : " ") + " ChangeFee_C=" + ChangeFee_C + " "; changes_num++; //Sign_num++;maohui20180802
            }
            if (ConvertType.ToDecimal(dr["UpstairFee_C"]) != UpstairFee_C)
            {
                changes_str += "【结算上楼费】 由:" + dr["UpstairFee_C"].ToString() + " 改为:" + UpstairFee_C;
                sqlstr += (changes_num > 0 ? " , " : " ") + " UpstairFee_C=" + UpstairFee_C + " "; changes_num++; //Sign_num++;maohui20180802
            }
            if (ConvertType.ToDecimal(dr["CustomsFee_C"]) != CustomsFee_C)
            {
                changes_str += "【结算报关费】 由:" + dr["CustomsFee_C"].ToString() + " 改为:" + CustomsFee_C;
                sqlstr += (changes_num > 0 ? " , " : " ") + " CustomsFee_C=" + CustomsFee_C + " "; changes_num++; //Sign_num++;maohui20180802
            }
            if (ConvertType.ToDecimal(dr["FrameFee_C"]) != FrameFee_C)
            {
                changes_str += "【结算木架费】 由:" + dr["FrameFee_C"].ToString() + " 改为:" + FrameFee_C;
                sqlstr += (changes_num > 0 ? " , " : " ") + " FrameFee_C=" + FrameFee_C + " "; changes_num++; //Sign_num++;maohui20180802
            }

            if (ConvertType.ToDecimal(dr["Expense_C"]) != Expense_C)
            {
                changes_str += "【结算工本费】 由:" + dr["Expense_C"].ToString() + " 改为:" + Expense_C;
                sqlstr += (changes_num > 0 ? " , " : " ") + " Expense_C=" + Expense_C + " "; changes_num++; //Sign_num++;maohui20180802
            }
            if (ConvertType.ToDecimal(dr["FuelFee_C"]) != FuelFee_C)
            {
                changes_str += "【结算燃油费】 由:" + dr["FuelFee_C"].ToString() + " 改为:" + FuelFee_C;
                sqlstr += (changes_num > 0 ? " , " : " ") + " FuelFee_C=" + FuelFee_C + " "; changes_num++; //Sign_num++;maohui20180802
            }
            if (ConvertType.ToDecimal(dr["InformationFee_C"]) != InformationFee_C)
            {
                changes_str += "【结算信息费】 由:" + dr["InformationFee_C"].ToString() + " 改为:" + InformationFee_C;
                sqlstr += (changes_num > 0 ? " , " : " ") + " InformationFee_C=" + InformationFee_C + " "; changes_num++; //Sign_num++;maohui20180802
            }
            if (ConvertType.ToDecimal(dr["MainlineFee"]) != MainlineFee)//1
            {
                changes_str += "【结算干线费】 由:" + dr["MainlineFee"].ToString() + " 改为:" + MainlineFee;
                sqlstr += (changes_num > 0 ? " , " : " ") + " MainlineFee=" + MainlineFee + " "; changes_num++; //Sign_num++;maohui20180802
            }
            if (ConvertType.ToDecimal(dr["DeliveryFee"]) != DeliveryFee)//2
            {
                changes_str += "【结算送货费】 由:" + dr["DeliveryFee"].ToString() + " 改为:" + DeliveryFee;
                sqlstr += (changes_num > 0 ? " , " : " ") + " DeliveryFee=" + DeliveryFee + " "; changes_num++; //Sign_num++;maohui20180802
            }
            if (ConvertType.ToDecimal(dr["TransferFee"]) != TransferFee)//3
            {
                changes_str += "【结算中转费】 由:" + dr["TransferFee"].ToString() + " 改为:" + TransferFee;
                sqlstr += (changes_num > 0 ? " , " : " ") + " TransferFee=" + TransferFee + " "; changes_num++; //Sign_num++;maohui20180802
            }
            if (ConvertType.ToDecimal(dr["DepartureOptFee"]) != DepartureOptFee)//4
            {
                changes_str += "【结算始发操作费】 由:" + dr["DepartureOptFee"].ToString() + " 改为:" + DepartureOptFee;
                sqlstr += (changes_num > 0 ? " , " : " ") + " DepartureOptFee=" + DepartureOptFee + " "; changes_num++; //Sign_num++;maohui20180802
            }
            if (ConvertType.ToDecimal(dr["TerminalOptFee"]) != TerminalOptFee)//5
            {
                changes_str += "【结算终端操作费】 由:" + dr["TerminalOptFee"].ToString() + " 改为:" + TerminalOptFee;
                sqlstr += (changes_num > 0 ? " , " : " ") + " TerminalOptFee=" + TerminalOptFee + " "; changes_num++; //Sign_num++;maohui20180802
            }

            if (ConvertType.ToDecimal(dr["TerminalAllotFee"]) != TerminalAllotFee)//6
            {
                changes_str += "【结算终端分拨费】 由:" + dr["TerminalAllotFee"].ToString() + " 改为:" + TerminalAllotFee;
                sqlstr += (changes_num > 0 ? " , " : " ") + " TerminalAllotFee=" + TerminalAllotFee + " "; changes_num++; //Sign_num++;maohui20180802
            }
            if (ConvertType.ToDecimal(dr["OperationWeight"]) != OperationWeight)//6
            {
                changes_str += "【结算重量】 由:" + dr["OperationWeight"].ToString() + " 改为:" + OperationWeight;
                sqlstr += (changes_num > 0 ? " , " : " ") + " OperationWeight=" + OperationWeight + " "; changes_num++; Sign_num++;
            }
            if (ConvertType.ToDecimal(dr["OptWeight"]) != OptWeight)//6
            {
                changes_str += "【操作重量】 由:" + dr["OptWeight"].ToString() + " 改为:" + OptWeight;
                sqlstr += (changes_num > 0 ? " , " : " ") + " OptWeight=" + OptWeight + " "; changes_num++; Sign_num++;
            }

            //luohui 商超
            if (ConvertType.ToInt32(dr["MarketSuper"]) != ConvertType.ToInt32(MarketSuper.Checked ? "1" : "0"))//6
            {
                changes_str += "【商超】 由:" + (ConvertType.ToInt32(dr["MarketSuper"]) == 1 ? "是" : "否") + " 改为:" + (MarketSuper.Checked ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " MarketSuper=" + ConvertType.ToInt32(MarketSuper.Checked ? "1" : "0") + " "; changes_num++; Sign_num++;
            }
            string sqlSub = "delete from WayBillSub where billno='" + BillNo.Text.Trim() + "' ";
            sqlSub += "insert WayBillSub (BillNo,Varieties,Package,Num,FeeWeight,FeeVolume,Weight,Volume,WeightPrice,VolumePrice,FeeType,Freight,companyid)"
                + "SELECT '" + BillNo.Text.Trim() + "', Varieties,Package,Num,FeeWeight,FeeVolume,Weight,Volume,WeightPrice,VolumePrice,FeeType,Freight,companyid "
                + "FROM dbo.L_SplitStr_BillSub_Clone(" + (string.Format("'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}' ",
                new object[]{ VarietiesStr, PackageStr ,NumStr,FeeWeightStr,FeeVolumeStr,
                    WeightStr,VolumeStr,WeightPriceStr,VolumePriceStr,FeeTypeStr,FreightStr,companyids})) + ",'@')";

            //hj20180925  长宽高尺寸
            string billno = BillNo.Text;
            string Varieties1 = gridView2.GetRowCellValue(0, "Varieties").ToString();
            string Package1 = gridView2.GetRowCellValue(0, "Package").ToString();
            string length1 = gridView2.GetRowCellValue(0, "length").ToString();
            string width1 = gridView2.GetRowCellValue(0, "width").ToString();
            string height1 = gridView2.GetRowCellValue(0, "height").ToString();

            string Varieties2 = gridView2.GetRowCellValue(1, "Varieties").ToString();
            string Package2 = gridView2.GetRowCellValue(1, "Package").ToString();
            string length2 = gridView2.GetRowCellValue(1, "length").ToString();
            string width2 = gridView2.GetRowCellValue(1, "width").ToString();
            string height2 = gridView2.GetRowCellValue(1, "height").ToString();


            //为空时转换为0 maohui20180724取消注释
            if (length1 == "" || length1 == null)
            {
                length1 = "0";
            }
            if (width1 == "" || width1 == null)
            {
                width1 = "0";
            }
            if (height1 == "" || height1 == null)
            {
                height1 = "0";
            }

            if (length2 == "" || length2 == null)
            {
                length2 = "0";
            }
            if (width2 == "" || width2 == null)
            {
                width2 = "0";
            }
            if (height2 == "" || height2 == null)
            {
                height2 = "0";
            }

            string sqlstr1 = "";
            List<SqlPara> list2 = new List<SqlPara>();
            list2.Add(new SqlPara("BillNo", billno));
            SqlParasEntity sps2 = new SqlParasEntity(OperType.Query, "USP_GET_SpecialGoodsSize", list2);
            DataSet ds2 = SqlHelper.GetDataSet(sps2);
            if (ds2.Tables[0].Rows.Count == 1)
            {
                string variet = ds2.Tables[0].Rows[0]["Varieties"].ToString();
                string pack = ds2.Tables[0].Rows[0]["Package"].ToString();
                decimal len = Convert.ToDecimal(ds2.Tables[0].Rows[0]["Length"]);
                decimal wid = Convert.ToDecimal(ds2.Tables[0].Rows[0]["Width"]);
                decimal hei = Convert.ToDecimal(ds2.Tables[0].Rows[0]["Height"]);

                if (variet == Varieties1 && pack == Package1)
                {
                    if (Varieties2 == "" || Varieties2 == null)
                    {
                        if (len.ToString() != length1 || wid.ToString() != width1 || hei.ToString() != height1)
                            sqlstr1 = string.Format("update SpecialGoodsSize set Length='{0}',Width='{1}',Height='{2}' where BillNo='{3}' and Varieties='{4}' and Package='{5}'", length1, width1, height1, billno, Varieties1, Package1);
                        changes_num++;
                    }
                    else
                    {
                        if (len.ToString() != length2 || wid.ToString() != width2 || hei.ToString() != height2)
                            sqlstr1 = string.Format(" update SpecialGoodsSize set Length='{0}',Width='{1}',Height='{2}' where BillNo='{3}' and Varieties='{4}' and Package='{5}'", length1, width1, height1, billno, Varieties1, Package1);
                        string sqlstr3 = string.Format("insert into SpecialGoodsSize (BillNo,Length,Width,Height,Varieties,Package)  values('{0}','{1}','{2}','{3}','{4}','{5}')", billno, length2, width2, height2, Varieties2, Package2);
                        sqlstr1 += sqlstr3;
                        changes_num++;
                    }
                }

            }
            else if (ds2.Tables[0].Rows.Count == 2 && (Varieties2 != "" && Varieties2 != null))
            {
                decimal len = Convert.ToDecimal(ds2.Tables[0].Rows[0]["Length"]);
                decimal wid = Convert.ToDecimal(ds2.Tables[0].Rows[0]["Width"]);
                decimal hei = Convert.ToDecimal(ds2.Tables[0].Rows[0]["Height"]);
                decimal len2 = Convert.ToDecimal(ds2.Tables[0].Rows[0]["Length"]);
                decimal wid2 = Convert.ToDecimal(ds2.Tables[0].Rows[0]["Width"]);
                decimal hei2 = Convert.ToDecimal(ds2.Tables[0].Rows[0]["Height"]);
                if (len.ToString() != length1 || wid.ToString() != width1 || hei.ToString() != height1 || len2.ToString() != length2 || wid2.ToString() != width2 || hei2.ToString() != height2)
                    sqlstr1 = string.Format(" update SpecialGoodsSize set Length='{0}',Width='{1}',Height='{2}' where BillNo='{3}' and Varieties='{4}' and Package='{5}'", length1, width1, height1, billno, Varieties1, Package1);
                string sqlstr3 = string.Format(" update SpecialGoodsSize set Length='{0}',Width='{1}',Height='{2}' where BillNo='{3}' and Varieties='{4}' and Package='{5}'", length2, width2, height2, billno, Varieties2, Package2);
                sqlstr1 += sqlstr3;
                changes_num++;
            }


            if (changes_num == 0)
            {
                MsgBox.ShowOK("单据没有修改，不能提交改单申请!");
                return;
            }
            //20180803，LD，唐克辉注释
            if (changes_str.Contains("【现付】") || changes_str.Contains("【提付】") || changes_str.Contains("【月结】") || changes_str.Contains("【短欠】") || changes_str.Contains("【货到前付】"))
            {
                if (CommonClass.GetAduitState(BillNO) > 0)
                {
                    MsgBox.ShowOK("运单已经审核或账款确认，不能执行！");
                    return;
                }
            }
            //20180803，LD，唐克辉注释
            if (changes_str.Contains("【交接方式】"))
            {
                if ("中心直送,中转送货,网点送货,送货".Contains(dr["TransferMode"].ToString()) == false || "中心直送,中转送货,网点送货,送货".Contains(TransferMode.Text.Trim()) == false)
                {
                    if (CommonClass.GetOutState(BillNO) > 0 && IsAccept > 0)
                    {
                        MsgBox.ShowOK("运单已经出库，并已被转分拨接收，不能修改交接方式！");
                        return;
                    }
                }
            }

            sqlstr += " where BillNo='" + BillNo.Text.Trim() + "' " + sqlSub + "" + sqlstr1;
            //20180803，LD，唐克辉注释
            //hj20180503
            //if (Sign_num > 0 && (dr["BillState"].ToString() == "16" || dr["BillState"].ToString() == "9" || dr["BillState"].ToString() == "14"))  //maohui20180801(签收，中转，送货状态可修改运费)
            if (Sign_num > 0 && (dr["BillState"].ToString() == "16" && IsAccept > 0))
            {
                //MsgBox.ShowError("此运单状态不允许修改其他信息，暂时无法进行改单申请！如需更改，请先退回库存或者反签收！");
                MsgBox.ShowError("此运单状态为签收，并已被转分拨接收，暂时无法进行改单申请！如需更改，请先反签收！");
                return;
            }


            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillNO", BillNo.Text.Trim()));
            list.Add(new SqlPara("ApplyContent", changes_str));
            //list.Add(new SqlPara("SqlStr", sqlstr.Replace("'","''")));
            //zaj
            list.Add(new SqlPara("SqlStr", sqlstr));


            list.Add(new SqlPara("ApplyWeb", CommonClass.UserInfo.WebName));
            list.Add(new SqlPara("ApplyMan", CommonClass.UserInfo.UserName));
            list.Add(new SqlPara("BillingWeb", dr["BegWeb"]));
            list.Add(new SqlPara("BillingDate", dr["BillDate"]));
            list.Add(new SqlPara("BeginSite", dr["StartSite"]));
            list.Add(new SqlPara("EndSite", dr["DestinationSite"]));
            SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BillChangesApply", list);
            int result = SqlHelper.ExecteNonQuery(sps);
            if (result > 0)
            {
                //hj20180815 判断勾选自动执行的公司，做完改单申请后自动执行
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_COMPANY_BY_ID_1", new List<SqlPara>() { new SqlPara("BillNo", BillNo.Text.Trim()) }));
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0 || ds.Tables[1].Rows.Count == 0)
                {
                    MsgBox.ShowOK("操作成功！");
                    #region hj20190425 转分拨同步修改
                    try
                    {
                        List<SqlPara> listFB = new List<SqlPara>();
                        listFB.Add(new SqlPara("BillNo", BillNo.Text.Trim()));
                        listFB.Add(new SqlPara("ApplyWeb", CommonClass.UserInfo.WebName));
                        listFB.Add(new SqlPara("ApplyMan", CommonClass.UserInfo.UserName));
                        listFB.Add(new SqlPara("ApplyContent", changes_str));
                        SqlParasEntity speFB = new SqlParasEntity(OperType.Query, "QSP_GET_BillApply_TOFB", listFB);
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
                            //dty.Add("EndsFBite", dsFB.Tables[0].Rows[0]["EndsFBite"].ToString());
                            dty.Add("BillingWeb", dsFB.Tables[0].Rows[0]["BillingWeb"].ToString());
                            dty.Add("ApplyWeb", dsFB.Tables[0].Rows[0]["ApplyWeb"].ToString());
                            dty.Add("ApplyMan", dsFB.Tables[0].Rows[0]["ApplyMan"].ToString());
                            dty.Add("ApplyType", dsFB.Tables[0].Rows[0]["ApplyType"].ToString());
                            dty.Add("LastState", dsFB.Tables[0].Rows[0]["LastState"].ToString());
                            dty.Add("SqlStr", dsFB.Tables[0].Rows[0]["SqlStr"].ToString());
                            dty.Add("ModifyType", dsFB.Tables[0].Rows[0]["ModifyType"].ToString());
                            dty.Add("ModifyProject", dsFB.Tables[0].Rows[0]["ModifyProject"].ToString());
                            dty.Add("ChangeBasInfoOrFeeInfo", dsFB.Tables[0].Rows[0]["ChangeBasInfoOrFeeInfo"].ToString());
                            string json = JsonConvert.SerializeObject(dty);
                            List<SqlPara> listAsy = new List<SqlPara>();
                            listAsy.Add(new SqlPara("FuntionName", "USP_ZQTMS_BILLAPPLYSYN_ZQTMS"));
                            listAsy.Add(new SqlPara("FaceUrl", ""));
                            listAsy.Add(new SqlPara("FaceJson", json));
                            listAsy.Add(new SqlPara("BillNos", BillNo.Text.Trim() + ","));
                            listAsy.Add(new SqlPara("Batch", ""));
                            listAsy.Add(new SqlPara("NodeName", "改单申请"));
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
                    this.Close();
                    return;
                }
                string ApplyID = ds.Tables[1].Rows[0]["ApplyID"].ToString();
                if (ds.Tables[0].Rows[0]["IsAutomaticGaiDan"].ToString() == "1")
                {
                    List<SqlPara> list_ZD = new List<SqlPara>();
                    list_ZD.Add(new SqlPara("ApplyID", ApplyID));
                    list_ZD.Add(new SqlPara("Man", CommonClass.UserInfo.UserName));
                    list_ZD.Add(new SqlPara("Reson", "自动执行"));
                    SqlParasEntity sps_ZD = new SqlParasEntity(OperType.Execute, "QSP_UpdateApplyState_ZD", list_ZD);
                    int result_ZD = SqlHelper.ExecteNonQuery(sps_ZD);
                    if (result_ZD > 0)
                    {
                        MsgBox.ShowOK("操作成功！");
                        this.Close();
                        return;
                    }
                }
                MsgBox.ShowOK("操作成功！");
                #region hj20190425 转分拨同步修改
                try
                {
                    List<SqlPara> listFB = new List<SqlPara>();
                    listFB.Add(new SqlPara("BillNo", BillNo.Text.Trim()));
                    listFB.Add(new SqlPara("ApplyWeb", CommonClass.UserInfo.WebName));
                    listFB.Add(new SqlPara("ApplyMan", CommonClass.UserInfo.UserName));
                    listFB.Add(new SqlPara("ApplyContent", changes_str));
                    SqlParasEntity speFB = new SqlParasEntity(OperType.Query, "QSP_GET_BillApply_TOFB", listFB);
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
                        //dty.Add("EndsFBite", dsFB.Tables[0].Rows[0]["EndsFBite"].ToString());
                        dty.Add("BillingWeb", dsFB.Tables[0].Rows[0]["BillingWeb"].ToString());
                        dty.Add("ApplyWeb", dsFB.Tables[0].Rows[0]["ApplyWeb"].ToString());
                        dty.Add("ApplyMan", dsFB.Tables[0].Rows[0]["ApplyMan"].ToString());
                        dty.Add("ApplyType", dsFB.Tables[0].Rows[0]["ApplyType"].ToString());
                        dty.Add("LastState", dsFB.Tables[0].Rows[0]["LastState"].ToString());
                        dty.Add("SqlStr", dsFB.Tables[0].Rows[0]["SqlStr"].ToString());
                        dty.Add("ModifyType", dsFB.Tables[0].Rows[0]["ModifyType"].ToString());
                        dty.Add("ModifyProject", dsFB.Tables[0].Rows[0]["ModifyProject"].ToString());
                        dty.Add("ChangeBasInfoOrFeeInfo", dsFB.Tables[0].Rows[0]["ChangeBasInfoOrFeeInfo"].ToString());
                        string json = JsonConvert.SerializeObject(dty);
                        List<SqlPara> listAsy = new List<SqlPara>();
                        listAsy.Add(new SqlPara("FuntionName", "USP_ZQTMS_BILLAPPLYSYN_ZQTMS"));
                        listAsy.Add(new SqlPara("FaceUrl", ""));
                        listAsy.Add(new SqlPara("FaceJson", json));
                        listAsy.Add(new SqlPara("BillNos", BillNo.Text.Trim() + ","));
                        listAsy.Add(new SqlPara("Batch", ""));
                        listAsy.Add(new SqlPara("NodeName", "改单申请"));
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
                this.Close();
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
                list1.Add(new SqlPara("BillNO", BillNo.Text.Trim()));
                list1.Add(new SqlPara("ApplyType", "改单申请"));
                SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_BILLAPPLY_HasApplying_Basics", list1);
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

        private void gridView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (gridView2.FocusedColumn != null)
                {
                    int VisibleColumnsCount = gridView2.VisibleColumns.Count; //显示出来的列数(隐藏列不统计在内)
                    if (gridView2.FocusedColumn.VisibleIndex == VisibleColumnsCount - 1)
                    {
                        gridView1.Focus();
                        gridView1.FocusedColumn = gridColumn52;
                        e.Handled = true;
                    }
                }
            }
        }

        private void WebPlatform_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (WebPlatform.Text.Trim() != "")
            {
                WebBillNo.Properties.ReadOnly = false;
            }
            else
            {
                WebBillNo.Properties.ReadOnly = true;
                WebBillNo.Text = "";
            }
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (IsReceiptFee.Checked)
            {
                gridView1.Columns["ReceiptFee"].Visible = true;
                gridView1.Columns["ReceiptFee"].OptionsColumn.AllowEdit = true;
                gridView1.Columns["ReceiptFee"].OptionsColumn.AllowFocus = true;
            }
            else
            {
                gridView1.Columns["ReceiptFee"].Visible = false;
                gridView1.Columns["ReceiptFee"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["ReceiptFee"].OptionsColumn.AllowFocus = false;
                gridView1.SetRowCellValue(0, "ReceiptFee", "0");
                ReceiptFee_C.Text = "0";
            }
            SetFeeToReceiptFee();
        }

        private void IsInvoice_CheckedChanged(object sender, EventArgs e)
        {
            if (IsInvoice.Checked)
            {
                gridView1.Columns["Tax"].Visible = true;
                gridView1.Columns["Tax"].OptionsColumn.AllowEdit = true;
                gridView1.Columns["Tax"].OptionsColumn.AllowFocus = true;
            }
            else
            {
                gridView1.Columns["Tax"].Visible = false;
                gridView1.Columns["Tax"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["Tax"].OptionsColumn.AllowFocus = false;
                gridView1.SetRowCellValue(0, "Tax", "0");
            }
            SetFeeToTaxFee();

        }

        private void NoticeState_CheckedChanged(object sender, EventArgs e)
        {
            if (NoticeState.Checked)
            {
                gridView1.Columns["NoticeFee"].Visible = true;
                gridView1.Columns["NoticeFee"].OptionsColumn.AllowEdit = true;
                gridView1.Columns["NoticeFee"].OptionsColumn.AllowFocus = true;
            }
            else
            {
                gridView1.Columns["NoticeFee"].Visible = false;
                gridView1.Columns["NoticeFee"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["NoticeFee"].OptionsColumn.AllowFocus = false;
                gridView1.SetRowCellValue(0, "NoticeFee", "0");
            }
            SetFeeToNoticeFee();
        }

        private void checkEdit5_CheckedChanged(object sender, EventArgs e)
        {
            str = this.BillRemark.Text.Trim();
            if (IsUpstairFee.Checked)
            {
                gridView1.Columns["UpstairFee"].Visible = true;
                gridView1.Columns["UpstairFee"].OptionsColumn.AllowEdit = true;
                gridView1.Columns["UpstairFee"].OptionsColumn.AllowFocus = true;
                if (str.IndexOf("上楼") < 0 || string.IsNullOrEmpty(this.BillRemark.Text.Trim().ToString()))
                {
                    str = this.BillRemark.Text.Trim().ToString() + "上楼，";
                }
                this.BillRemark.Text = str;
            }
            else
            {
                gridView1.Columns["UpstairFee"].Visible = false;
                gridView1.Columns["UpstairFee"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["UpstairFee"].OptionsColumn.AllowFocus = false;
                gridView1.SetRowCellValue(0, "UpstairFee", "0");
                if (str.Contains("上楼"))
                {
                    str = str.Replace("上楼，", "");
                }
                this.BillRemark.Text = str;
            }
            //SetFeeNew();
            SetFeeToUpstairFee();

        }

        private void checkEdit8_CheckedChanged(object sender, EventArgs e)
        {
            if (IsAgentFee.Checked)
            {
                gridView1.Columns["CollectionPay"].Visible = true;
                gridView1.Columns["CollectionPay"].OptionsColumn.AllowEdit = true;
                gridView1.Columns["CollectionPay"].OptionsColumn.AllowFocus = true;


                gridView1.Columns["AgentFee"].Visible = true;
                gridView1.Columns["AgentFee"].OptionsColumn.AllowEdit = true;
                gridView1.Columns["AgentFee"].OptionsColumn.AllowFocus = true;

            }
            else
            {
                gridView1.Columns["CollectionPay"].Visible = false;
                gridView1.Columns["CollectionPay"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["CollectionPay"].OptionsColumn.AllowFocus = false;
                gridView1.SetRowCellValue(0, "CollectionPay", "0");


                gridView1.Columns["AgentFee"].Visible = false;
                gridView1.Columns["AgentFee"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["AgentFee"].OptionsColumn.AllowFocus = false;
                gridView1.SetRowCellValue(0, "AgentFee", "0");
            }
            SetFeeToAgentFee();
        }

        private void checkEdit7_CheckedChanged(object sender, EventArgs e)
        {
            if (IsPackagFee.Checked)
            {
                gridView1.Columns["packagFee"].Visible = true;
                gridView1.Columns["packagFee"].OptionsColumn.AllowEdit = true;
                gridView1.Columns["packagFee"].OptionsColumn.AllowFocus = true;
            }
            else
            {
                gridView1.Columns["packagFee"].Visible = false;
                gridView1.Columns["packagFee"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["packagFee"].OptionsColumn.AllowFocus = false;
                gridView1.SetRowCellValue(0, "packagFee", "0");
            }
        }

        private void checkEdit6_CheckedChanged(object sender, EventArgs e)
        {
            if (IsFrameFee.Checked)
            {
                gridView1.Columns["FrameFee"].Visible = true;
                gridView1.Columns["FrameFee"].OptionsColumn.AllowEdit = true;
                gridView1.Columns["FrameFee"].OptionsColumn.AllowFocus = true;
            }
            else
            {
                gridView1.Columns["FrameFee"].Visible = false;
                gridView1.Columns["FrameFee"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["FrameFee"].OptionsColumn.AllowFocus = false;
                gridView1.SetRowCellValue(0, "FrameFee", "0");
            }
        }

        private void checkEdit9_CheckedChanged(object sender, EventArgs e)
        {
            if (IsCustomsFee.Checked)
            {
                gridView1.Columns["CustomsFee"].Visible = true;
                gridView1.Columns["CustomsFee"].OptionsColumn.AllowEdit = true;
                gridView1.Columns["CustomsFee"].OptionsColumn.AllowFocus = true;
            }
            else
            {
                gridView1.Columns["CustomsFee"].Visible = false;
                gridView1.Columns["CustomsFee"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["CustomsFee"].OptionsColumn.AllowFocus = false;
                gridView1.SetRowCellValue(0, "CustomsFee", "0");
            }
        }

        private void checkEdit12_CheckedChanged(object sender, EventArgs e)
        {
            str = this.BillRemark.Text.Trim();
            if (IsHandleFee.Checked)
            {
                gridView1.Columns["HandleFee"].Visible = true;
                gridView1.Columns["HandleFee"].OptionsColumn.AllowEdit = true;
                gridView1.Columns["HandleFee"].OptionsColumn.AllowFocus = true;
                if (str.IndexOf("装卸") < 0 || string.IsNullOrEmpty(this.BillRemark.Text.Trim().ToString()))
                {
                    str = this.BillRemark.Text.Trim().ToString() + "装卸，";
                }
                this.BillRemark.Text = str;
            }
            else
            {
                gridView1.Columns["HandleFee"].Visible = false;
                gridView1.Columns["HandleFee"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["HandleFee"].OptionsColumn.AllowFocus = false;
                gridView1.SetRowCellValue(0, "HandleFee", "0");
                if (str.Contains("装卸"))
                {
                    str = str.Replace("装卸，", "");
                }
                this.BillRemark.Text = str;
            }
            SetFeeToHandleFee();

        }

        private void checkEdit10_CheckedChanged(object sender, EventArgs e)
        {
            if (IsWarehouseFee.Checked)
            {
                gridView1.Columns["WarehouseFee"].Visible = true;
                gridView1.Columns["WarehouseFee"].OptionsColumn.AllowEdit = true;
                gridView1.Columns["WarehouseFee"].OptionsColumn.AllowFocus = true;
            }
            else
            {
                gridView1.Columns["WarehouseFee"].Visible = false;
                gridView1.Columns["WarehouseFee"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["WarehouseFee"].OptionsColumn.AllowFocus = false;
                gridView1.SetRowCellValue(0, "WarehouseFee", "0");
            }
            SetFeeToWarehouseFee();

        }

        private void checkEdit4_CheckedChanged(object sender, EventArgs e)
        {
            if (IsSupportValue.Checked)
            {
                gridView1.Columns["DeclareValue"].Visible = true;
                gridView1.Columns["DeclareValue"].OptionsColumn.AllowEdit = true;
                gridView1.Columns["DeclareValue"].OptionsColumn.AllowFocus = true;


                gridView1.Columns["SupportValue"].Visible = true;
                gridView1.Columns["SupportValue"].OptionsColumn.AllowEdit = true;
                gridView1.Columns["SupportValue"].OptionsColumn.AllowFocus = true;
              
            }
            else
            {
                gridView1.Columns["DeclareValue"].Visible = false;
                gridView1.Columns["DeclareValue"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["DeclareValue"].OptionsColumn.AllowFocus = false;
                gridView1.SetRowCellValue(0, "DeclareValue", "0");

                gridView1.Columns["SupportValue"].Visible = false;
                gridView1.Columns["SupportValue"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["SupportValue"].OptionsColumn.AllowFocus = false;
                gridView1.SetRowCellValue(0, "SupportValue", "0");
                
            }
            SetFeeToSupportFee();
        }

        private void checkEdit13_CheckedChanged(object sender, EventArgs e)
        {
            if (IsForkliftFee.Checked)
            {
                gridView1.Columns["ForkliftFee"].Visible = true;
                gridView1.Columns["ForkliftFee"].OptionsColumn.AllowEdit = true;
                gridView1.Columns["ForkliftFee"].OptionsColumn.AllowFocus = true;
            }
            else
            {
                gridView1.Columns["ForkliftFee"].Visible = false;
                gridView1.Columns["ForkliftFee"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["ForkliftFee"].OptionsColumn.AllowFocus = false;
                gridView1.SetRowCellValue(0, "ForkliftFee", "0");
            }
            SetFeeToForkliftFee();

        }

        private void checkEdit2_CheckedChanged(object sender, EventArgs e)
        {
            if (IsOtherFee.Checked)
            {
                gridView1.Columns["OtherFee"].Visible = true;
                gridView1.Columns["OtherFee"].OptionsColumn.AllowEdit = true;
                gridView1.Columns["OtherFee"].OptionsColumn.AllowFocus = true;
            }
            else
            {
                gridView1.Columns["OtherFee"].Visible = false;
                gridView1.Columns["OtherFee"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["OtherFee"].OptionsColumn.AllowFocus = false;
                gridView1.SetRowCellValue(0, "OtherFee", "0");
            }
        }

        private void checkEdit11_CheckedChanged(object sender, EventArgs e)
        {
            if (IsStorageFee.Checked)
            {

                //MarketSuper.Checked = false;  zb20190610
                if (dr == null)//新开
                {
                    gridView1.SetRowCellValue(0, "StorageFee", "0");
                }
                gridView1.Columns["StorageFee"].Visible = true;
                gridView1.Columns["StorageFee"].OptionsColumn.AllowEdit = true;
                gridView1.Columns["StorageFee"].OptionsColumn.AllowFocus = true;
            }
            else
            {
                gridView1.Columns["StorageFee"].Visible = false;
                gridView1.Columns["StorageFee"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["StorageFee"].OptionsColumn.AllowFocus = false;
                gridView1.SetRowCellValue(0, "StorageFee", "0");
            }
            SetFeeToStorageFee();
        }

        private void GoodsVoucher_CheckedChanged(object sender, EventArgs e)
        {

        }

        //private void PreciousGoods_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (PreciousGoods.Checked)
        //    {
        //        IsSupportValue.Checked = true;
        //        IsSupportValue.Enabled = false;
        //    }
        //    else
        //    {
        //        IsSupportValue.Checked = false;
        //        IsSupportValue.Enabled = true;
        //    }
        //}

        private void gcjiehuodanhao_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SetDeliCode();
            }
        }

        private void SetDeliCode()
        {
            try
            {
                int rows = gridView5.FocusedRowHandle;
                if (rows < 0) return;
                DataRow dr_ = gridView5.GetDataRow(rows);
                VehicleNum.Text = dr_["VehicleNum"].ToString();
                DriverName.Text = dr_["DriverName"].ToString();
                ReceivOrderNo.Text = dr_["DeliCode"].ToString();
                gcjiehuodanhao.Visible = false;
                // Salesman.Focus();
                gridControl2.Focus();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }

        private void gridView5_DoubleClick(object sender, EventArgs e)
        {
            SetDeliCode();
        }

        private void ReceivOrderNo_EditValueChanged(object sender, EventArgs e)
        {
            gridColumn7.FilterInfo = new ColumnFilterInfo("DeliCode LIKE '%" + ReceivOrderNo.Text.Trim() + "%'");
        }

        private void checkEdit3_CheckedChanged(object sender, EventArgs e)
        {
            if (IsChangeFee.Checked)
            {
                gridView1.Columns["ChangeFee"].Visible = true;
                gridView1.Columns["ChangeFee"].OptionsColumn.AllowEdit = true;
                gridView1.Columns["ChangeFee"].OptionsColumn.AllowFocus = true;
            }
            else
            {
                gridView1.Columns["ChangeFee"].Visible = false;
                gridView1.Columns["ChangeFee"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["ChangeFee"].OptionsColumn.AllowFocus = false;
                gridView1.SetRowCellValue(0, "ChangeFee", "0");
            }
        }

        private void ReceiptCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ReceiptCondition.Text.Trim() != "")
            {
                IsReceiptFee.Checked = true;
                checkEdit3.Enabled = true;
            }
            else
            {
                IsReceiptFee.Checked = false;
                if (CommonClass.UserInfo.companyid != "167")
                {
                    checkEdit3.Enabled = false;
                    checkEdit3.Checked = false;
                }
            }
        }

        private void SetFee()
        {
            try
            {
                gridView1.PostEditor();
                gridView1.UpdateCurrentRow();
                gridView1.UpdateSummary();
                gridView2.PostEditor();
                gridView2.UpdateCurrentRow();
                gridView2.UpdateSummary();

                string Bsite = StartSite.Text.Trim();
                string MiddleSite = TransferSite.Text.Trim();
                //decimal Weight = ConvertType.ToDecimal(gridColumn68.SummaryItem.SummaryValue);
                decimal Volume = ConvertType.ToDecimal(gridColumn69.SummaryItem.SummaryValue);
                decimal Weight = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "OperationWeight"));

                #region 结算干线费
                //if (CommonClass.dsMainlineFee_ZX != null && CommonClass.dsMainlineFee_ZX.Tables.Count > 0)
                //{
                //    //将结算干线费标准换成专线结算干线费标准 zaj 2017-12-8
                //    //DataRow[] drMainlineFee = CommonClass.dsMainlineFee_ZX.Tables[0].Select("FromSite='" + Bsite + "' and TransferSite='" + MiddleSite + "' and TransportMode='" + TransitMode.Text.Trim() + "'");
                //    DataRow[] drMainlineFee = CommonClass.dsMainlineFee_ZX.Tables[0].Select("FromSite='" + Bsite + "' and TransferSite='" + MiddleSite + "'");

                //    if (drMainlineFee.Length > 0)
                //    {
                //        decimal ParcelPriceMin = ConvertType.ToDecimal(drMainlineFee[0]["ParcelPriceMin"]);//最低一票
                //        decimal HeavyPrice = ConvertType.ToDecimal(drMainlineFee[0]["HeavyPrice"]);//重货
                //        decimal LightPrice = ConvertType.ToDecimal(drMainlineFee[0]["LightPrice"]);//轻货
                //        decimal MainlineFeeAll = 0;

                //        try
                //        {
                //            List<SqlPara> list = new List<SqlPara>();
                //            list.Add(new SqlPara("beginSite", StartSite.Text.Trim()));
                //            list.Add(new SqlPara("endSite", TransferSite.Text.Trim()));
                //            SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_BASMAINLINEFEETZ_NEW", list);
                //            DataSet ds = SqlHelper.GetDataSet(spe);

                //            for (int i = 0; i < RowCount; i++)
                //            {
                //                decimal w = Math.Round(ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeWeight")), 2);
                //                decimal v = Math.Round(ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeVolume")), 2);
                //                string Package = gridView2.GetRowCellValue(i, "Package").ToString();
                //                decimal fee = Math.Max(w * HeavyPrice, v * LightPrice);

                //                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                //                {
                //                    if (Weight <= 300)
                //                    {
                //                        fee = fee * (decimal)1.2;
                //                    }
                //                    if (Weight <= 8000 && Weight > 3000)
                //                    {
                //                        fee = fee * (decimal)0.98;
                //                    }
                //                    if (Weight > 8000)
                //                    {
                //                        fee = fee * (decimal)0.95;
                //                    }
                //                }
                //                else
                //                {
                //                    // 货满足重泡比（计费重量/计费体积）>1/1.5或重泡比（计费重量/计费体积）<1/7的结算干线费（深圳始发到全国的，且满足3T以上不含3T）打95折
                //                    if (StartSite.Text == "深圳" && Weight > 3000 && v != 0)
                //                    {
                //                        if (((w / (decimal)1000 / v) > (decimal)(1.0 / 1.5)) || ((w / (decimal)1000 / v) < (decimal)(1.0 / 7.0)))
                //                        {
                //                            fee = fee * (decimal)0.95;
                //                        }
                //                    }
                //                }

                //                if (Package != "纸箱" && Package != "纤袋" && Package != "膜")
                //                {
                //                    MainlineFeeAll += fee * (decimal)1.1;
                //                    //zaj 2017-8-29 费用比例参数化
                //                    //MainlineFeeAll += fee * Convert.ToDecimal(CommonClass.Arg.MainlineFeeRate);
                //                }
                //                else
                //                {
                //                    MainlineFeeAll += fee;
                //                }
                //            }
                //            // 如果勾选了异形货，结算费用上浮50%
                //            if (AlienGoods.Checked == true)
                //            {
                //                MainlineFeeAll = MainlineFeeAll * (decimal)1.5;
                //            }
                //            decimal acc = Math.Max(MainlineFeeAll, ParcelPriceMin);

                //            gridView8.SetRowCellValue(0, "MainlineFee", Math.Round(acc, 2));
                //        }
                //        catch (Exception ex)
                //        {
                //            MsgBox.ShowException(ex);
                //        }
                //    }
                //    else
                //    {
                //        gridView8.SetRowCellValue(0, "MainlineFee", 0);
                //    }
                //}
                #endregion

                #region 结算中转费
                //if (CommonClass.dsTransferFee_ZX != null && CommonClass.dsTransferFee_ZX.Tables.Count > 0)
                //{
                //    //将结算中转费标准换成专线结算中转费标准 zaj 2017-12-8
                //    DataRow[] drTransferFee = CommonClass.dsTransferFee_ZX.Tables[0].Select("TransferSite='" + MiddleSite + "' and ToProvince='" + ReceivProvince.Text + "' and ToCity='" + ReceivCity.Text + "' and ToArea='" + ReceivArea.Text.Trim() + "'");
                //    if (drTransferFee.Length > 0 && TransferMode.Text != "网点送货" && TransitMode.Text != "中强项目")
                //    {
                //        decimal HeavyPrice = ConvertType.ToDecimal(drTransferFee[0]["HeavyPrice"]);//重货
                //        decimal LightPrice = ConvertType.ToDecimal(drTransferFee[0]["LightPrice"]);//轻货
                //        decimal ParcelPriceMin = ConvertType.ToDecimal(drTransferFee[0]["ParcelPriceMin"]);//最低一票
                //        decimal TransferFeeAll = 0;
                //        for (int i = 0; i < RowCount; i++)
                //        {
                //            decimal w = Math.Round(ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeWeight")), 2);
                //            decimal v = Math.Round(ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeVolume")), 2);
                //            string Package = gridView2.GetRowCellValue(i, "Package").ToString();
                //            decimal fee = Math.Max(w * HeavyPrice, v * LightPrice);
                //            if (Package != "纸箱" && Package != "纤袋" && Package != "膜")
                //            {
                //                TransferFeeAll += fee * (decimal)1.1;
                //            }
                //            else
                //            {
                //                TransferFeeAll += fee;
                //            }
                //        }
                //        decimal acc = Math.Max(TransferFeeAll, ParcelPriceMin);
                //        gridView8.SetRowCellValue(0, "TransferFee", Math.Round(acc, 2));
                //    }
                //    else
                //    {
                //        gridView8.SetRowCellValue(0, "TransferFee", 0);
                //    }
                //}
                #endregion

                #region 结算送货费
                ////if (TransitMode.Text == "中强专线" || TransitMode.Text == "中强快线" || TransitMode.Text == "中强城际")
                ////{
                //if (ReceivProvince.Text.Trim() == "香港")
                //{
                //    if (TransferMode.Text.Contains("送") && CommonClass.dsSendPriceHK != null && CommonClass.dsSendPriceHK.Tables.Count > 0)
                //    {
                //        string sql = "Province='" + ReceivProvince.Text.Trim()
                //            + "' and City='" + ReceivCity.Text.Trim()
                //            + "' and Area='" + ReceivArea.Text.Trim()
                //            + "' and Street='" + ReceivStreet.Text.Trim()
                //            + "' and " + Weight + ">=w1"
                //            + " and " + Weight + " <w2";
                //        DataRow[] drDeliveryFee = CommonClass.dsSendPriceHK.Tables[0].Select(sql);
                //        if (drDeliveryFee.Length > 0)
                //        {
                //            string fmtext = drDeliveryFee[0]["Expression"].ToString();
                //            double Additional = ConvertType.ToDouble(drDeliveryFee[0]["Additional"].ToString());
                //            fmtext = fmtext.Replace("w", Weight.ToString());
                //            DataTable dt = new DataTable();
                //            //ExecMsg = string.Empty;
                //            double DeliveryFee = Math.Round(double.Parse(dt.Compute(fmtext, "").ToString()), 2, MidpointRounding.AwayFromZero);
                //            gridView1.SetRowCellValue(0, "DeliveryFee", DeliveryFee + Additional);
                //        }
                //        else
                //        {
                //            gridView1.SetRowCellValue(0, "DeliveryFee", 0);
                //        }
                //    }
                //    else
                //    {
                //        gridView1.SetRowCellValue(0, "DeliveryFee", 0);
                //    }
                //}
                //else
                //{
                //    //zaj 2017-11-15
                //    if ((TransferMode.Text == "网点送货" || TransferMode.Text == "中转送货") || TransferMode.Text == "送货" && CommonClass.dsSendPrice_ZX != null && CommonClass.dsSendPrice_ZX.Tables.Count > 0)
                //    {
                //        string sql = "Province='" + ReceivProvince.Text.Trim()
                //            + "' and City='" + ReceivCity.Text.Trim()
                //            + "' and Area='" + ReceivArea.Text.Trim()
                //            + "' and Street='" + ReceivStreet.Text.Trim() + "'";
                //        //  + "' and TransferMode='" + TransferMode.Text.Trim() 
                //        //+ "' and " + Weight + ">=MinWeight"
                //        //+ " and " + Weight + " <MaxWeight";
                //        //  DataRow[] drDeliveryFee = CommonClass.dsSendPrice.Tables[0].Select(sql);
                //        DataRow[] drDeliveryFee = CommonClass.dsSendPrice_ZX.Tables[0].Select(sql);//
                //        if (drDeliveryFee.Length > 0)
                //        {
                //            decimal w0_200 = ConvertType.ToDecimal(drDeliveryFee[0]["w0_200"]);
                //            decimal w200_1000 = ConvertType.ToDecimal(drDeliveryFee[0]["w200_1000"]);
                //            decimal w1000_3000 = ConvertType.ToDecimal(drDeliveryFee[0]["w1000_3000"]);
                //            decimal w3000_5000 = ConvertType.ToDecimal(drDeliveryFee[0]["w3000_5000"]);
                //            decimal w5000_100000 = ConvertType.ToDecimal(drDeliveryFee[0]["w5000_100000"]);
                //            //decimal DeliveryFee = ConvertType.ToDecimal(drDeliveryFee[0]["DeliveryFee"]);
                //            decimal DeliveryFee = 0;
                //            if (Weight >= 0 && Weight <= 200)
                //            {
                //                DeliveryFee = w0_200;
                //            }
                //            else if (Weight >= 200 && Weight <= 1000)
                //            {
                //                DeliveryFee = w200_1000;
                //            }
                //            else if (Weight >= 1000 && Weight <= 3000)
                //            {
                //                DeliveryFee = w1000_3000;
                //            }
                //            else if (Weight >= 3000 && Weight <= 5000)
                //            {
                //                DeliveryFee = w3000_5000;
                //            }
                //            else if (Weight > 5000)
                //            {
                //                DeliveryFee = w5000_100000;
                //            }
                //            gridView1.SetRowCellValue(0, "DeliveryFee", Math.Round(DeliveryFee, 2));
                //        }
                //        //else
                //        //{
                //        //    //gridView1.SetRowCellValue(0, "DeliveryFee", 0);
                //        //    //Tool.ToolTip.ShowTip("交接方式：" + TransferMode.Text + "无结算送货费！", ReceivStreet, ToolTipLocation.RightTop);
                //        //    //TransferMode.Text = "";
                //        //    if (TransferMode.Text == "网点送货")
                //        //    {
                //        //        DataRow[] drDirectSendFee = CommonClass.dsDirectSendFee.Tables[0].Select("CenterName='" + PickGoodsSite.Text.Trim() + "'");
                //        //        double GPSLng = 0, GPSLat = 0, MiddleLon = 0, MiddleLat = 0, Price = 0;
                //        //        if (drDirectSendFee.Length > 0)
                //        //        {
                //        //            GPSLng = ConvertType.ToDouble(drDirectSendFee[0]["GPSLng"].ToString());
                //        //            GPSLat = ConvertType.ToDouble(drDirectSendFee[0]["GPSLat"].ToString());
                //        //            Price = ConvertType.ToDouble(drDirectSendFee[0]["Price"].ToString());
                //        //        }
                //        //        if (gcdaozhan.DataSource != null)
                //        //        {
                //        //            DataTable table = (DataTable)gcdaozhan.DataSource;
                //        //            DataRow[] dr = table.Select("MiddleStreet='" + ReceivStreet.Text.Trim() + "'");
                //        //            if (dr.Length > 0)
                //        //            {
                //        //                MiddleLon = ConvertType.ToDouble(dr[0]["MiddleLon"].ToString());
                //        //                MiddleLat = ConvertType.ToDouble(dr[0]["MiddleLat"].ToString());
                //        //            }
                //        //            double gl = HarvenSin.Distance(GPSLat, GPSLng, MiddleLat, MiddleLon);
                //        //            decimal acc = ((decimal)gl * (Weight / 1000) * (decimal)Price);
                //        //            gridView1.SetRowCellValue(0, "DeliveryFee", Math.Round(acc, 2));
                //        //        }
                //        //    }
                //        //    else
                //        //    {
                //        //        gridView1.SetRowCellValue(0, "DeliveryFee", 0);
                //        //    }
                //        //}
                //    }
                //    else if (TransferMode.Text == "司机直送" && CommonClass.dsDirectDriverFee != null && CommonClass.dsDirectDriverFee.Tables.Count > 0)
                //    {
                //        DataRow[] drDirectDriverFee = CommonClass.dsDirectDriverFee.Tables[0].Select("WebName='" + PickGoodsSite.Text.Trim() + "'");
                //        double GPSLng = 0, GPSLat = 0, MiddleLon = 0, MiddleLat = 0, Price = 0, LowPrice = 0;
                //        if (drDirectDriverFee.Length > 0)
                //        {
                //            GPSLng = ConvertType.ToDouble(drDirectDriverFee[0]["Lng"].ToString());
                //            GPSLat = ConvertType.ToDouble(drDirectDriverFee[0]["Lat"].ToString());
                //            Price = ConvertType.ToDouble(drDirectDriverFee[0]["KmPrice"].ToString());
                //            LowPrice = ConvertType.ToDouble(drDirectDriverFee[0]["LowPrice"].ToString());
                //        }
                //        if (gcdaozhan.DataSource != null)
                //        {
                //            DataTable table = (DataTable)gcdaozhan.DataSource;
                //            DataRow[] dr = table.Select("MiddleStreet='" + ReceivStreet.Text.Trim() + "'");
                //            if (dr.Length > 0)
                //            {
                //                MiddleLon = ConvertType.ToDouble(dr[0]["MiddleLon"].ToString());
                //                MiddleLat = ConvertType.ToDouble(dr[0]["MiddleLat"].ToString());
                //            }
                //            double gl = HarvenSin.Distance(GPSLat, GPSLng, MiddleLat, MiddleLon);
                //            decimal acc = Math.Max(((decimal)gl * (decimal)Price), (decimal)LowPrice);//* (Weight / 1000)
                //            gridView1.SetRowCellValue(0, "DeliveryFee", Math.Round(acc, 2));
                //        }
                //    }
                //    else if (TransferMode.Text == "中心直送" && CommonClass.dsDirectSendFee != null && CommonClass.dsDirectSendFee.Tables.Count > 0)
                //    {
                //        DataRow[] drDirectSendFee = CommonClass.dsDirectSendFee.Tables[0].Select("CenterName='" + PickGoodsSite.Text.Trim() + "'");
                //        double GPSLng = 0, GPSLat = 0, MiddleLon = 0, MiddleLat = 0, Price = 0, OperationWeight = 0;
                //        if (drDirectSendFee.Length > 0)
                //        {
                //            GPSLng = ConvertType.ToDouble(drDirectSendFee[0]["GPSLng"].ToString());
                //            GPSLat = ConvertType.ToDouble(drDirectSendFee[0]["GPSLat"].ToString());
                //            Price = ConvertType.ToDouble(drDirectSendFee[0]["Price"].ToString());
                //            OperationWeight = ConvertType.ToDouble(drDirectSendFee[0]["OperationWeight"].ToString());
                //        }
                //        if (gcdaozhan.DataSource != null)
                //        {
                //            DataTable table = (DataTable)gcdaozhan.DataSource;
                //            DataRow[] dr = table.Select("MiddleStreet='" + ReceivStreet.Text.Trim() + "'");
                //            if (dr.Length > 0)
                //            {
                //                MiddleLon = ConvertType.ToDouble(dr[0]["MiddleLon"].ToString());
                //                MiddleLat = ConvertType.ToDouble(dr[0]["MiddleLat"].ToString());
                //            }
                //            decimal Weight_1 = Math.Max(Weight, (decimal)OperationWeight);
                //            double gl = HarvenSin.Distance(GPSLat, GPSLng, MiddleLat, MiddleLon);
                //            //decimal acc = ((decimal)gl * (Weight_1 / 1000) * (decimal)Price);
                //            //gridView1.SetRowCellValue(0, "DeliveryFee", Math.Round(acc, 2));
                //            #region 中心直送结算标准

                //            decimal acc = 0;
                //            if (Weight_1 >= 0 && Weight_1 < 3000)
                //            {
                //                if (gl >= 0 && gl < 100)
                //                {
                //                    acc = ((decimal)gl * (Weight_1 / 1000) * (decimal)2.5);
                //                    acc = Math.Max(acc, 200);//最低结算价
                //                    acc = Math.Min(acc, 800);//最高结算价
                //                }
                //                else if (gl >= 100 && gl < 200)
                //                {
                //                    acc = ((decimal)gl * (Weight_1 / 1000) * (decimal)2.5);
                //                    acc = Math.Max(acc, 700);//最低结算价
                //                    acc = Math.Min(acc, 1600);//最高结算价
                //                }
                //                else if (gl >= 200 && gl < 300)
                //                {
                //                    acc = ((decimal)gl * (Weight_1 / 1000) * (decimal)2.5);
                //                    acc = Math.Max(acc, 1400);//最低结算价
                //                    acc = Math.Min(acc, 2400);//最高结算价
                //                }
                //                else if (gl >= 300 && gl < 400)
                //                {
                //                    acc = ((decimal)gl * (Weight_1 / 1000) * (decimal)2.5);
                //                    acc = Math.Max(acc, 2100);//最低结算价
                //                    acc = Math.Min(acc, 3200);//最高结算价
                //                }
                //                else if (gl >= 400)
                //                {
                //                    acc = ((decimal)gl * (Weight_1 / 1000) * (decimal)2.5);
                //                    acc = Math.Max(acc, 2800);//最低结算价
                //                    acc = Math.Min(acc, 4800);//最高结算价
                //                }
                //            }
                //            else if (Weight_1 >= 3000 && Weight_1 < 8000)
                //            {
                //                if (gl >= 0 && gl < 100)
                //                {
                //                    acc = ((decimal)gl * (Weight_1 / 1000) * (decimal)2);
                //                    acc = Math.Max(acc, 500);//最低结算价
                //                    acc = Math.Min(acc, 1200);//最高结算价
                //                }
                //                else if (gl >= 100 && gl < 200)
                //                {
                //                    acc = ((decimal)gl * (Weight_1 / 1000) * (decimal)2);
                //                    acc = Math.Max(acc, 800);//最低结算价
                //                    acc = Math.Min(acc, 2400);//最高结算价
                //                }
                //                else if (gl >= 200 && gl < 300)
                //                {
                //                    acc = ((decimal)gl * (Weight_1 / 1000) * (decimal)1.8);
                //                    acc = Math.Max(acc, 1600);//最低结算价
                //                    acc = Math.Min(acc, 3000);//最高结算价
                //                }
                //                else if (gl >= 300 && gl < 400)
                //                {
                //                    acc = ((decimal)gl * (Weight_1 / 1000) * (decimal)1.8);
                //                    acc = Math.Max(acc, 2400);//最低结算价
                //                    acc = Math.Min(acc, 4000);//最高结算价
                //                }
                //                else if (gl >= 400)
                //                {
                //                    acc = ((decimal)gl * (Weight_1 / 1000) * (decimal)1.8);
                //                    acc = Math.Max(acc, 3200);//最低结算价
                //                    acc = Math.Min(acc, 6000);//最高结算价
                //                }
                //            }
                //            else if (Weight_1 >= 8000 && Weight_1 < 15000)
                //            {
                //                if (gl >= 0 && gl < 100)
                //                {
                //                    acc = ((decimal)gl * (Weight_1 / 1000) * (decimal)1.5);
                //                    acc = Math.Max(acc, 800);//最低结算价
                //                    acc = Math.Min(acc, 1500);//最高结算价
                //                }
                //                else if (gl >= 100 && gl < 200)
                //                {
                //                    acc = ((decimal)gl * (Weight_1 / 1000) * (decimal)1.5);
                //                    acc = Math.Max(acc, 1200);//最低结算价
                //                    acc = Math.Min(acc, 3000);//最高结算价
                //                }
                //                else if (gl >= 200 && gl < 300)
                //                {
                //                    acc = ((decimal)gl * (Weight_1 / 1000) * (decimal)1.2);
                //                    acc = Math.Max(acc, 2000);//最低结算价
                //                    acc = Math.Min(acc, 3600);//最高结算价
                //                }
                //                else if (gl >= 300 && gl < 400)
                //                {
                //                    acc = ((decimal)gl * (Weight_1 / 1000) * (decimal)1.2);
                //                    acc = Math.Max(acc, 3000);//最低结算价
                //                    acc = Math.Min(acc, 4800);//最高结算价
                //                }
                //                else if (gl >= 400)
                //                {
                //                    acc = ((decimal)gl * (Weight_1 / 1000) * (decimal)1.2);
                //                    acc = Math.Max(acc, 4000);//最低结算价
                //                    acc = Math.Min(acc, 7200);//最高结算价
                //                }
                //            }
                //            else if (Weight_1 >= 15000 && Weight_1 < 33000)
                //            {
                //                if (gl >= 0 && gl < 100)
                //                {
                //                    acc = ((decimal)gl * (Weight_1 / 1000) * (decimal)1);
                //                    acc = Math.Max(acc, 1200);//最低结算价
                //                    acc = Math.Min(acc, 2200);//最高结算价
                //                }
                //                else if (gl >= 100 && gl < 200)
                //                {
                //                    acc = ((decimal)gl * (Weight_1 / 1000) * (decimal)1);
                //                    acc = Math.Max(acc, 1800);//最低结算价
                //                    acc = Math.Min(acc, 4000);//最高结算价
                //                }
                //                else if (gl >= 200 && gl < 300)
                //                {
                //                    acc = ((decimal)gl * (Weight_1 / 1000) * (decimal)0.7);
                //                    acc = Math.Max(acc, 3000);//最低结算价
                //                    acc = Math.Min(acc, 5400);//最高结算价
                //                }
                //                else if (gl >= 300 && gl < 400)
                //                {
                //                    acc = ((decimal)gl * (Weight_1 / 1000) * (decimal)0.7);
                //                    acc = Math.Max(acc, 4500);//最低结算价
                //                    acc = Math.Min(acc, 7200);//最高结算价
                //                }
                //                else if (gl >= 400)
                //                {
                //                    acc = ((decimal)gl * (Weight_1 / 1000) * (decimal)0.7);
                //                    acc = Math.Max(acc, 6000);//最低结算价
                //                    acc = Math.Min(acc, 10800);//最高结算价
                //                }
                //            }
                //            else if (Weight_1 >= 33000 && Weight_1 < 60000)
                //            {
                //                if (gl >= 0 && gl < 100)
                //                {
                //                    acc = ((decimal)gl * (Weight_1 / 1000) * (decimal)0.7);
                //                    acc = Math.Max(acc, 2000);//最低结算价
                //                    acc = Math.Min(acc, 3000);//最高结算价
                //                }
                //                else if (gl >= 100 && gl < 200)
                //                {
                //                    acc = ((decimal)gl * (Weight_1 / 1000) * (decimal)0.7);
                //                    acc = Math.Max(acc, 2500);//最低结算价
                //                    acc = Math.Min(acc, 6000);//最高结算价
                //                }
                //                else if (gl >= 200 && gl < 300)
                //                {
                //                    acc = ((decimal)gl * (Weight_1 / 1000) * (decimal)0.5);
                //                    acc = Math.Max(acc, 3600);//最低结算价
                //                    acc = Math.Min(acc, 6900);//最高结算价
                //                }
                //                else if (gl >= 300 && gl < 400)
                //                {
                //                    acc = ((decimal)gl * (Weight_1 / 1000) * (decimal)0.5);
                //                    acc = Math.Max(acc, 4500);//最低结算价
                //                    acc = Math.Min(acc, 9200);//最高结算价
                //                }
                //                else if (gl >= 400)
                //                {
                //                    acc = ((decimal)gl * (Weight_1 / 1000) * (decimal)0.5);
                //                    acc = Math.Max(acc, 7200);//最低结算价
                //                    acc = Math.Min(acc, 13800);//最高结算价
                //                }
                //            }
                //            gridView1.SetRowCellValue(0, "DeliveryFee", Math.Round(acc, 2));

                //            #endregion
                //        }
                //    }
                //    else
                //    {
                //        gridView1.SetRowCellValue(0, "DeliveryFee", 0);
                //    }

                //    //if (TransferMode.Text == "网点送货" && PickGoodsSite.SelectedIndex != 0)
                //    //{
                //    //    DataRow[] drDirectSendFee = CommonClass.dsDirectSendFee.Tables[0].Select("CenterName='" + PickGoodsSite.Text.Trim() + "'");
                //    //    double GPSLng = 0, GPSLat = 0, MiddleLon = 0, MiddleLat = 0, Price = 0;
                //    //    if (drDirectSendFee.Length > 0)
                //    //    {
                //    //        GPSLng = ConvertType.ToDouble(drDirectSendFee[0]["GPSLng"].ToString());
                //    //        GPSLat = ConvertType.ToDouble(drDirectSendFee[0]["GPSLat"].ToString());
                //    //        Price = ConvertType.ToDouble(drDirectSendFee[0]["Price"].ToString());
                //    //    }
                //    //    if (gcdaozhan.DataSource != null)
                //    //    {
                //    //        DataTable table = (DataTable)gcdaozhan.DataSource;
                //    //        DataRow[] dr = table.Select("MiddleStreet='" + ReceivStreet.Text.Trim() + "'");
                //    //        if (dr.Length > 0)
                //    //        {
                //    //            MiddleLon = ConvertType.ToDouble(dr[0]["MiddleLon"].ToString());
                //    //            MiddleLat = ConvertType.ToDouble(dr[0]["MiddleLat"].ToString());
                //    //        }
                //    //        double gl = HarvenSin.Distance(GPSLat, GPSLng, MiddleLat, MiddleLon);
                //    //        decimal acc = ((decimal)gl * (Weight / 1000) * (decimal)Price);
                //    //        gridView1.SetRowCellValue(0, "DeliveryFee", Math.Round(acc, 2));
                //    //    }
                //    //}
                //}
                ////}
                ////else
                ////{
                ////    gridView1.SetRowCellValue(0, "DeliveryFee", 0);
                ////}

                #endregion

                #region 结算始发操作费
                if (CommonClass.dsDepartureOptFee_ZX != null && CommonClass.dsDepartureOptFee_ZX.Tables.Count > 0)
                {
                    //DataRow[] drDepartureOptFee = CommonClass.dsDepartureOptFee.Tables[0].Select("FromSite='" + Bsite + "' and TransitMode='" + TransitMode.Text.Trim() + "'");
                    //将结算始发操作费标准换成专线始发终端操作费标准 dsDepartureOptFee-》dsDepartureOptFee_ZX zaj 2017-12-8
                    if (TransitMode.Text.Trim() != "中强整车")
                    {
                        DataRow[] drDepartureOptFee = CommonClass.dsDepartureOptFee_ZX.Tables[0].Select("FromSite='" + Bsite + "'and TransitMode='" + TransitMode.Text.Trim() + "'");
                        if (drDepartureOptFee.Length > 0)
                        {

                            decimal HeavyPrice = ConvertType.ToDecimal(drDepartureOptFee[0]["HeavyPrice"]);//重货
                            decimal LightPrice = ConvertType.ToDecimal(drDepartureOptFee[0]["LightPrice"]);//轻货
                            decimal ParcelPriceMin = ConvertType.ToDecimal(drDepartureOptFee[0]["ParcelPriceMin"]);//最低一票
                            decimal acc = Weight * HeavyPrice;
                            acc = Math.Max(acc, ParcelPriceMin);
                            DepartureOptFee.Text = Math.Round(acc, 2).ToString();//luohui

                        }
                        else
                        {

                            DepartureOptFee.Text = "0";
                        }
                    }
                    else
                    {
                        DepartureOptFee.Text = "0";
                    }

                    //SetFeeToPreciousGoodFee();
                    if (PreciousGoods.Checked)
                    {
                        //decimal dec = ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "DepartureOptFee").ToString());
                        // gridView8.SetRowCellValue(0, "DepartureOptFee", dec + 30);
                        decimal dec = ConvertType.ToDecimal(DepartureOptFee.Text.Trim());
                        DepartureOptFee.Text = (dec + 30).ToString();
                    }
                }
                #endregion

                #region 结算终端操作费
                if (CommonClass.dsTerminalOptFee_ZX != null && CommonClass.dsTerminalOptFee_ZX.Tables.Count > 0)
                {
                    //将结算终端操作费标准换成干线结算终端操作费标准 dsTerminalOptFee-》dsTerminalOptFee_ZX zaj 2017-12-8
                    if (TransitMode.Text.Trim() != "中强整车")
                    {
                        DataRow[] drTerminalOptFee = CommonClass.dsTerminalOptFee_ZX.Tables[0].Select("TransferSite='" + MiddleSite + "'and TransitMode='" + TransitMode.Text.Trim() + "'");
                        if (drTerminalOptFee.Length > 0 && TransitMode.Text != "中强项目" && TransitMode.Text != "中强城际")
                        {
                            decimal HeavyPrice = ConvertType.ToDecimal(drTerminalOptFee[0]["HeavyPrice"]);//重货
                            decimal LightPrice = ConvertType.ToDecimal(drTerminalOptFee[0]["LightPrice"]);//轻货
                            decimal ParcelPriceMin = ConvertType.ToDecimal(drTerminalOptFee[0]["ParcelPriceMin"]);//最低一票
                            decimal acc = Weight * HeavyPrice;
                            acc = Math.Max(acc, ParcelPriceMin);
                            TerminalOptFee.Text = Math.Round(acc, 2).ToString();


                            //decimal acc = Math.Max(Weight * HeavyPrice, ParcelPriceMin);
                            //gridView8.SetRowCellValue(0, "TerminalOptFee", Math.Round(acc, 2));
                        }
                        else
                        {
                            TerminalOptFee.Text = "0.00";
                        }
                    }
                    else
                    {
                        TerminalOptFee.Text = "0.00";
                    }

                }
                #endregion

                #region 结算终端分拨费
                //if (CommonClass.dsTerminalAllotFee != null && CommonClass.dsTerminalAllotFee.Tables.Count > 0)
                //{
                //    DataRow[] drTerminalAllotFee = CommonClass.dsTerminalAllotFee.Tables[0].Select("ToSite='" + MiddleSite + "'");
                //    if (drTerminalAllotFee.Length > 0 && TransitMode.Text != "中强项目")
                //    {
                //        decimal HeavyPrice = ConvertType.ToDecimal(drTerminalAllotFee[0]["HeavyPrice"]);//重货
                //        decimal LightPrice = ConvertType.ToDecimal(drTerminalAllotFee[0]["LightPrice"]);//轻货
                //        decimal ParcelPriceMin = ConvertType.ToDecimal(drTerminalAllotFee[0]["ParcelPriceMin"]);//最低一票
                //        decimal acc = Math.Max(Weight * HeavyPrice, Volume * LightPrice);
                //        acc = Math.Max(acc, ParcelPriceMin);
                //        gridView8.SetRowCellValue(0, "TerminalAllotFee", Math.Round(acc, 2));
                //        //decimal acc = Math.Max(Weight * HeavyPrice, ParcelPriceMin);
                //        //gridView8.SetRowCellValue(0, "TerminalAllotFee", Math.Round(acc, 2));

                //    }
                //    else
                //    {
                //        gridView8.SetRowCellValue(0, "TerminalAllotFee", 0);
                //    }
                //}
                #endregion
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        #region 客户计价体系 hj20181030
        private void SetDenominatedFee()
        {
            try
            {
                //DataSet ds=SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query,"QSP_GET_basDenominatedFee"));
                string consignorCompany = ConsignorCompany.Text.Trim();
                DataRow[] drDenominatedFee;
                if (TransitMode.Text.Trim().Contains("快线"))
                {
                    drDenominatedFee = dsDenominatedFee.Tables[0].Select("Province='" + ReceivProvince1 + "' and City='" + ReceivCity1 + "' and Area='" + ReceivArea1 + "' and CusName ='" + consignorCompany + "'" + " and TransitMode ='" + "快线" + "'");
                }
                else
                {
                    drDenominatedFee = dsDenominatedFee.Tables[0].Select("Province='" + ReceivProvince1 + "' and City='" + ReceivCity1 + "' and Area='" + ReceivArea1 + "' and CusName ='" + consignorCompany + "'" + " and isnull(TransitMode,'') <>'" + "快线" + "'");
                }
                if (drDenominatedFee.Length > 0)
                {

                    for (int i = 0; i < RowCount; i++)
                    {
                        for (int j = 0; j < drDenominatedFee.Length; j++)
                        {
                            decimal feeWeight = ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeWeight"));
                            decimal feeVolume = ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeVolume"));
                            decimal WeightMax = ConvertType.ToDecimal(drDenominatedFee[j]["WeightMax"]);
                            decimal WeightMin = ConvertType.ToDecimal(drDenominatedFee[j]["WeightMin"]);
                            decimal VolumeMax = ConvertType.ToDecimal(drDenominatedFee[j]["VolumeMax"]);
                            decimal VolumeMin = ConvertType.ToDecimal(drDenominatedFee[j]["VolumeMin"]);
                            decimal HeavyPrice = 0;
                            if (feeWeight < WeightMin || feeWeight > WeightMax)
                            {
                                HeavyPrice = 0;
                            }
                            else
                            {
                                HeavyPrice = ConvertType.ToDecimal(drDenominatedFee[j]["HeavyPrice"]);//重货
                            }
                            decimal HeavyPriceMin = ConvertType.ToDecimal(drDenominatedFee[j]["HeavyPriceMin"]);//重货最低一票
                            decimal LightPrice = 0;
                            if (feeVolume < VolumeMin || feeVolume > VolumeMax)
                            {
                                LightPrice = 0;
                            }
                            else
                            {
                                LightPrice = ConvertType.ToDecimal(drDenominatedFee[j]["LightPrice"]);//轻货
                            }
                            decimal LightPriceMin = ConvertType.ToDecimal(drDenominatedFee[j]["LightPriceMin"]);//轻货最低一票
                            decimal acc1 = ConvertType.ToDecimal(Math.Max(HeavyPrice * feeWeight, HeavyPriceMin));
                            decimal acc2 = ConvertType.ToDecimal(Math.Max(LightPrice * feeVolume, LightPriceMin));

                            //if ((feeWeight > WeightMin && feeWeight < WeightMax) || (feeVolume > VolumeMin && feeVolume < VolumeMax) || (feeWeight > WeightMin && feeWeight < WeightMax && feeVolume > VolumeMin && feeVolume < VolumeMax))
                            //{
                            gridView2.SetRowCellValue(i, "WeightPrice", ConvertType.ToDecimal(drDenominatedFee[j]["HeavyPrice"]));
                            gridView2.SetRowCellValue(i, "VolumePrice", ConvertType.ToDecimal(drDenominatedFee[j]["LightPrice"]));
                            gridView1.SetRowCellValue(0, "DeliFee", ConvertType.ToDecimal(drDenominatedFee[j]["DeliFee"]));
                            gridView1.SetRowCellValue(0, "ReceivFee", ConvertType.ToDecimal(drDenominatedFee[j]["ReceivFee"]));
                            gridView2.SetRowCellValue(i, "Freight", ConvertType.ToDecimal(Math.Max(acc1, acc2)));
                            //}
                        }
                    }

                }
                else
                {
                    DataRow[] drDenominatedFee1;
                    if (TransitMode.Text.Trim().Contains("快线"))
                    {
                        drDenominatedFee1 = dsDenominatedFee.Tables[0].Select("Province='" + ReceivProvince1 + "' and City='" + ReceivCity1 + "' and Area='" + ReceivArea1 + "' and CusName ='" + consignorCompany + "'" + " and TransitMode ='" + "快线" + "'");
                    }
                    else
                    {
                        drDenominatedFee1 = dsDenominatedFee.Tables[0].Select("Province='" + ReceivProvince1 + "' and City='" + ReceivCity1 + "' and Area='" + ReceivArea1 + "' and CusName ='" + consignorCompany + "'" + " and isnull(TransitMode,'') <>'" + "快线" + "'");
                    }
                    // = dsDenominatedFee.Tables[0].Select("Province='" + ReceivProvince1 + "' and City='" + ReceivCity1 + "' and CusName ='" + consignorCompany + "'");
                    if (drDenominatedFee1.Length > 0)
                    {

                        for (int i = 0; i < RowCount; i++)
                        {
                            for (int j = 0; j < drDenominatedFee1.Length; j++)
                            {
                                decimal feeWeight = ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeWeight"));
                                decimal feeVolume = ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeVolume"));
                                decimal WeightMax = ConvertType.ToDecimal(drDenominatedFee1[j]["WeightMax"]);
                                decimal WeightMin = ConvertType.ToDecimal(drDenominatedFee1[j]["WeightMin"]);
                                decimal VolumeMax = ConvertType.ToDecimal(drDenominatedFee1[j]["VolumeMax"]);
                                decimal VolumeMin = ConvertType.ToDecimal(drDenominatedFee1[j]["VolumeMin"]);
                                decimal HeavyPrice = 0;
                                if (feeWeight < WeightMin || feeWeight > WeightMax)
                                {
                                    HeavyPrice = 0;
                                }
                                else
                                {
                                    HeavyPrice = ConvertType.ToDecimal(drDenominatedFee[j]["HeavyPrice"]);//重货
                                }
                                decimal HeavyPriceMin = ConvertType.ToDecimal(drDenominatedFee[j]["HeavyPriceMin"]);//重货最低一票
                                decimal LightPrice = 0;
                                if (feeVolume < VolumeMin || feeVolume > VolumeMax)
                                {
                                    LightPrice = 0;
                                }
                                else
                                {
                                    LightPrice = ConvertType.ToDecimal(drDenominatedFee[j]["LightPrice"]);//轻货
                                }
                                decimal LightPriceMin = ConvertType.ToDecimal(drDenominatedFee1[j]["LightPriceMin"]);//轻货最低一票
                                decimal acc1 = ConvertType.ToDecimal(Math.Max(HeavyPrice * feeWeight, HeavyPriceMin));
                                decimal acc2 = ConvertType.ToDecimal(Math.Max(LightPrice * feeVolume, LightPriceMin));

                                //if ((feeWeight >= WeightMin && feeWeight < WeightMax) || (feeVolume >= VolumeMin && feeVolume < VolumeMax) || (feeWeight > WeightMin && feeWeight < WeightMax && feeVolume > VolumeMin && feeVolume < VolumeMax))
                                //{
                                gridView2.SetRowCellValue(i, "WeightPrice", ConvertType.ToDecimal(drDenominatedFee1[j]["HeavyPrice"]));
                                gridView2.SetRowCellValue(i, "VolumePrice", ConvertType.ToDecimal(drDenominatedFee1[j]["LightPrice"]));
                                gridView1.SetRowCellValue(0, "DeliFee", ConvertType.ToDecimal(drDenominatedFee1[j]["DeliFee"]));
                                gridView1.SetRowCellValue(0, "ReceivFee", ConvertType.ToDecimal(drDenominatedFee1[j]["ReceivFee"]));
                                gridView2.SetRowCellValue(i, "Freight", ConvertType.ToDecimal(Math.Max(acc1, acc2)));
                                //}
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }


        #endregion



        #region 设置结算费用
        /// <summary>
        /// 设置结算费用
        /// </summary>
        private void SetFeeNew()
        {
            try
            {

                //设置结算费按新标准 对外=结算 Chirs.Song

                gridView1.PostEditor();
                gridView1.UpdateCurrentRow();
                gridView1.UpdateSummary();
                gridView2.PostEditor();
                gridView2.UpdateCurrentRow();
                gridView2.UpdateSummary();

                string Bsite = StartSite.Text.Trim(); // 始发站
                string MiddleSite = TransferSite.Text.Trim(); // 中转地
                decimal Volume = ConvertType.ToDecimal(gridColumn69.SummaryItem.SummaryValue); // 计费体积
                decimal Weight = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "OperationWeight"));//结算重量
                if (PaymentMode.Text == "免费")
                {
                    //gridView8.SetRowCellValue(0, "MainlineFee", 0);
                    //gridView8.SetRowCellValue(0, "DeliveryFee", 0);
                    //gridView8.SetRowCellValue(0, "TransferFee", 0);
                    //gridView8.SetRowCellValue(0, "DepartureOptFee", 0);
                    //gridView8.SetRowCellValue(0, "TerminalOptFee", 0);
                    //gridView8.SetRowCellValue(0, "TerminalAllotFee", 0);
                    //gridView8.SetRowCellValue(0, "DepartureAllotFee", 0);

                    //gridView8.SetRowCellValue(0, "ReceiptFee_C", 0);
                    //gridView8.SetRowCellValue(0, "NoticeFee_C", 0);

                    //gridView8.SetRowCellValue(0, "SupportValue_C", 0);
                    //gridView8.SetRowCellValue(0, "AgentFee_C", 0);
                    //gridView8.SetRowCellValue(0, "PackagFee_C", 0);
                    //gridView8.SetRowCellValue(0, "OtherFee_C", 0);
                    //gridView8.SetRowCellValue(0, "HandleFee_C", 0);

                    //gridView8.SetRowCellValue(0, "StorageFee_C", 0);
                    //gridView8.SetRowCellValue(0, "WarehouseFee_C", 0);
                    //gridView8.SetRowCellValue(0, "ForkliftFee_C", 0);
                    //gridView8.SetRowCellValue(0, "Tax_C", 0);
                    //gridView8.SetRowCellValue(0, "ChangeFee_C", 0);

                    //gridView8.SetRowCellValue(0, "UpstairFee_C", 0);
                    //gridView8.SetRowCellValue(0, "CustomsFee_C", 0);
                    //gridView8.SetRowCellValue(0, "FrameFee_C", 0);
                    //gridView8.SetRowCellValue(0, "Expense_C", 0);
                    //gridView8.SetRowCellValue(0, "FuelFee_C", 0);
                    //gridView8.SetRowCellValue(0, "InformationFee_C", 0);
                    //return;
                }

                #region 结算干线费
                if (TransitMode.Text != "中强整车")
                {
                    string TransitModeStr = TransitMode.Text.Trim();
                    //if (TransitModeStr == "一票通")
                    //{
                    //    TransitModeStr = "中强快线";
                    //}
                    DataRow[] drMainlineFee;
                    if (CommonClass.UserInfo.companyid == "239")
                    {
                        drMainlineFee = CommonClass.dsMainlineFee_ZX.Tables[0].Select("FromSite='" + Bsite + "' and TransferSite='" + MiddleSite + "' and TransportMode='可通专线'");//TransportMode='" + TransitModeStr + "'
                    }
                    else
                    {
                        drMainlineFee = CommonClass.dsMainlineFee_ZX.Tables[0].Select("FromSite='" + Bsite + "' and TransferSite='" + MiddleSite + "'and TransportMode='" + TransitMode.Text.Trim() + "'");//TransportMode='" + TransitModeStr + "'
                    }
                    if (drMainlineFee.Length > 0)
                    {
                        decimal ParcelPriceMin = ConvertType.ToDecimal(drMainlineFee[0]["ParcelPriceMin"]);//最低一票
                        decimal HeavyPrice = ConvertType.ToDecimal(drMainlineFee[0]["HeavyPrice"]);//重货
                        decimal LightPrice = ConvertType.ToDecimal(drMainlineFee[0]["LightPrice"]);//轻货
                        decimal MainlineFeeAll = 0;

                        try
                        {
                            List<SqlPara> list = new List<SqlPara>();
                            list.Add(new SqlPara("beginSite", StartSite.Text.Trim()));
                            list.Add(new SqlPara("endSite", TransferSite.Text.Trim()));
                            SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_BASMAINLINEFEETZ_NEW", list);
                            DataSet ds = SqlHelper.GetDataSet(spe);

                            for (int i = 0; i < RowCount; i++)
                            {
                                decimal w = Math.Round(ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeWeight")), 2);
                                decimal v = Math.Round(ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeVolume")), 2);
                                string Package = gridView2.GetRowCellValue(i, "Package").ToString();
                                decimal fee = Math.Max(w * HeavyPrice, v * LightPrice);

                                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                {
                                    if (CommonClass.UserInfo.companyid == "266")//266西南战区系统结算干线费上浮
                                    {
                                        DataRow[] rows = ds.Tables[0].Select(" LineName IN ('深圳始发','江沪始发','浙江始发','京津始发','山东始发','华中始发','川渝始发','云贵始发','广州始发','潮州始发','中山始发')");
                                        if (rows.Length > 0)
                                        {
                                            if (Weight <= 300)
                                            {
                                                fee = fee * (decimal)1.25;
                                            }
                                            if (Weight > 300 && Weight <= 1000)
                                            {
                                                fee = fee * (decimal)1.1;
                                            }
                                        }
                                        else
                                        {
                                            if (Weight <= 300)
                                            {
                                                fee = fee * (decimal)1.2;
                                            }
                                        }
                                    }
                                    //else
                                    //{
                                    //    DataRow[] rows = ds.Tables[0].Select("attribute='第三批'");
                                    //    if (Weight <= 300)
                                    //    {
                                    //        fee = fee * (decimal)1.2;
                                    //    }
                                    //}
                                    //if (rows.Length <= 0)  //maohui20180728（取消第1批第2批单票3-8吨以上结算干线费打98折的政策）
                                    //{
                                    //    if (Weight <= 8000 && Weight > 3000)
                                    //    {
                                    //        fee = fee * (decimal)0.98;
                                    //    }
                                    //}
                                    //if (Weight > 8000)
                                    //{
                                    //    fee = fee * (decimal)0.95;
                                    //}
                                }
                                else
                                {
                                    // 货满足重泡比（计费重量/计费体积）>1/1.5或重泡比（计费重量/计费体积）<1/7的结算干线费（深圳始发到全国的，且满足3T以上不含3T）打95折
                                    if (StartSite.Text == "深圳" && Weight > 3000 && v != 0)
                                    {
                                        if (((w / (decimal)1000 / v) > (decimal)(1.0 / 1.5)) || ((w / (decimal)1000 / v) < (decimal)(1.0 / 7.0)))
                                        {
                                            fee = fee * (decimal)0.95;
                                        }
                                    }
                                }


                                if (Package != "纸箱" && Package != "纤袋" && Package != "膜")
                                {
                                    //MainlineFeeAll += fee * (decimal)1.1;
                                    //zaj 2017-8-29 费用比例参数化
                                    // MainlineFeeAll += fee * Convert.ToDecimal(CommonClass.Arg.MainlineFeeRate);
                                    MainlineFeeAll += fee * Convert.ToDecimal(1.05);

                                }
                                else
                                {
                                    MainlineFeeAll += fee;
                                }
                            }

                            decimal acc = 0;

                            // 如果勾选了异形货，结算费用上浮50%
                            if (AlienGoods.Checked == true)
                            {
                                MainlineFeeAll = MainlineFeeAll * (decimal)1.5;
                            }
                            acc = Math.Max(MainlineFeeAll, ParcelPriceMin);

                            //gridView8.SetRowCellValue(0, "MainlineFee", Math.Round(acc, 2));
                            MainlineFee.Text = Math.Round(acc, 2).ToString();

                        }
                        catch (Exception ex)
                        {
                            MsgBox.ShowException(ex);
                        }
                    }
                    else
                    {
                        //gridView8.SetRowCellValue(0, "MainlineFee", 0);
                        MainlineFee.Text = "0";
                    }
                }
                #endregion

                if (TransitMode.Text.Trim() != "中强整车")
                {
                    #region 结算中转费
                    //中强项目和司机直送不计算结算中转费
                    if (!TransferMode.Text.Equals("司机直送") && TransitMode.Text.Trim() != "中强项目")
                    {
                        DataRow[] drTransferFee;
                        if (CommonClass.UserInfo.companyid == "239")
                        {
                            drTransferFee = CommonClass.dsTransferFee_ZX.Tables[0].Select("TransferSite='" + MiddleSite + "' and ToProvince='"
                            + ReceivProvince1 + "' and ToCity='" + ReceivCity1 + "' and ToArea='" + ReceivArea1 + "' and TransitMode='可通专线'");
                        }
                        else
                        {
                            drTransferFee = CommonClass.dsTransferFee_ZX.Tables[0].Select("FromSite like '%" + Bsite + "%' and TransferSite='" + MiddleSite + "' and ToProvince='"
                                + ReceivProvince1 + "' and ToCity='" + ReceivCity1 + "' and ToArea='" + ReceivArea1 + "'and TransitMode='" + TransitMode.Text.Trim() + "'");
                        }
                        if (drTransferFee.Length > 0)
                        {
                            decimal HeavyPrice = ConvertType.ToDecimal(drTransferFee[0]["HeavyPrice"]);//重货
                            decimal LightPrice = ConvertType.ToDecimal(drTransferFee[0]["LightPrice"]);//轻货
                            decimal ParcelPriceMin = ConvertType.ToDecimal(drTransferFee[0]["ParcelPriceMin"]);//最低一票
                            decimal TransferFeeAll = 0;
                            decimal allFeeWeight = 0;
                            decimal allFeeVolume = 0;
                            for (int i = 0; i < RowCount; i++)
                            {
                                decimal w = Math.Round(ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeWeight")), 2);
                                decimal v = Math.Round(ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeVolume")), 2);
                                string Package = gridView2.GetRowCellValue(i, "Package").ToString();
                                decimal fee = Math.Max(w * HeavyPrice, v * LightPrice);
                                allFeeWeight += w;
                                allFeeVolume += v;
                                if (ReceivProvince1 != "香港" && ReceivProvince1 != "海南省")
                                {
                                    // （300KG及以下的按标准上调1.5倍，3T以上（不含3T）打8折 lyj 2017/12/06
                                    // 300KG及以下的按标准上调1.2倍，3T以上（不含3T）打9折 ccd 2018.03.15
                                    if (Weight <= 300)
                                    {
                                        fee = fee * (decimal)1.2;
                                    }
                                    if (MiddleSite != "南昌")  //maohui20180731(中转地为南昌的，取消3吨以上打九折)
                                    {
                                        if (Weight > 3000)
                                        {
                                            fee = fee * (decimal)0.9;
                                        }
                                    }
                                }

                                if (Package != "纸箱" && Package != "纤袋" && Package != "膜")
                                {
                                    // TransferFeeAll += fee * (decimal)1.1;
                                    //zaj 2017-8-29 费用比例参数化
                                    //TransferFeeAll += fee * Convert.ToDecimal(CommonClass.Arg.TransferFeeRate);
                                    TransferFeeAll += fee * Convert.ToDecimal(1.05);

                                }
                                else
                                {
                                    TransferFeeAll += fee;
                                }
                            }
                            decimal acc = 0;

                            // 如果勾选了异形货，结算费用上浮50%
                            if (AlienGoods.Checked == true)
                            {
                                TransferFeeAll = TransferFeeAll * (decimal)1.5;
                            }
                            acc = Math.Max(TransferFeeAll, ParcelPriceMin);

                            if (ReceivProvince1 == "香港" && TransferMode.Text.Trim() != "自提")
                            {
                                string allFeeType = "";
                                if (allFeeWeight > allFeeVolume / (decimal)3.8 * 1000)
                                {
                                    // 则说明总体是计重
                                    allFeeType = "计重";
                                }
                                else
                                {
                                    // 总体计方
                                    allFeeType = "计方";
                                }
                                if (allFeeType == "计重" && allFeeWeight < 200)
                                {
                                    acc = ParcelPriceMin;
                                }
                                if (allFeeType == "计方" && allFeeVolume < (decimal)1.2)
                                {
                                    acc = ParcelPriceMin;
                                }
                            }

                            //gridView8.SetRowCellValue(0, "TransferFee", Math.Round(acc, 2));
                            TransferFee.Text = Math.Round(acc, 2).ToString();
                        }
                        else
                        {
                            //gridView8.SetRowCellValue(0, "TransferFee", 0);
                            TransferFee.Text = "0";
                        }
                    }
                    else
                    {
                        //gridView8.SetRowCellValue(0, "TransferFee", 0);
                        TransferFee.Text = "0";
                    }
                    #endregion

                    #region 结算送货费
                    //if ((TransitMode.Text == "中强专线" || TransitMode.Text == "中强快线" || TransitMode.Text == "中强城际" || TransitMode.Text == "一票通" || TransitMode.Text == "中强项目"
                    //    || TransitMode.Text == "中强普线")//20180408 新增普线
                    //    && !(TransferMode.Text.Trim() == "司机直送"))//isLineWeb() &&
                    //{
                    if (ReceivProvince1 == "香港")
                    {
                        if (TransferMode.Text.Contains("送") && TransferMode.Text.Trim() != "专车直送")
                        {
                            //当收货省为香港时，结算送货费计费不取系统结算重量，后台计算计费重量、计费体积按1：6折算取大值乘以结算送货费单价 2018.03.19 ccd
                            //decimal weight_temp = 0;
                            //decimal FeeWeight_temp = 0;
                            //decimal FeeVolumer_temp = 0;
                            double DeliveryFee = 0;
                            double Additional = 0;
                            for (int i = 0; i < RowCount; i++)
                            {
                                decimal weight_temp = 0;
                                decimal FeeWeight = Math.Round(ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeWeight")), 2);
                                decimal FeeVolume = Math.Round(ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeVolume")), 2);

                                decimal FeeVolumer = 0;
                                string Package = gridView2.GetRowCellValue(i, "Package").ToString();
                                if (Package != "纸箱" && Package != "纤袋" && Package != "膜")
                                {
                                    FeeWeight = FeeWeight * (decimal)1.05;
                                    FeeVolumer = FeeVolume / (decimal)6 * 1000 * (decimal)1.05;
                                }
                                else
                                {
                                    FeeVolumer = FeeVolume / (decimal)6 * 1000;
                                }
                                //FeeWeight_temp += FeeWeight;
                                //FeeVolumer_temp += FeeVolumer;
                                weight_temp = Math.Round(Math.Max(FeeWeight, FeeVolumer), 2);

                                //weight_temp=Math.Round(Math.Max(FeeWeight_temp, FeeVolumer_temp), 2);


                                string sql = "Province='" + ReceivProvince1
                                    + "' and City='" + ReceivCity1
                                    + "' and Area='" + ReceivArea1
                                    + "' and Street='" + ReceivStreet1
                                    + "' and " + weight_temp + ">=w1"
                                    + " and " + weight_temp + " <w2";
                                DataRow[] drDeliveryFee = CommonClass.dsSendPriceHK.Tables[0].Select(sql);
                                if (drDeliveryFee.Length > 0)
                                {
                                    string fmtext = drDeliveryFee[0]["Expression"].ToString();
                                    double Additional_temp = ConvertType.ToDouble(drDeliveryFee[0]["Additional"].ToString());
                                    if (Additional_temp > Additional)
                                    {
                                        Additional = Additional_temp;
                                    }
                                    fmtext = fmtext.Replace("w", weight_temp.ToString());
                                    DataTable dt = new DataTable();
                                    //ExecMsg = string.Empty;
                                    DeliveryFee = DeliveryFee + Math.Round(double.Parse(dt.Compute(fmtext, "").ToString()), 2, MidpointRounding.AwayFromZero);

                                    // 当为轻货时，香港结算送货费按表达式计算完成后打6折，提出人：方俊杰
                                    // 这里暂定只要包含计方就算作计方

                                    //取消系统香港结算送货费的轻货打6折方案 2018.03.19 ccd

                                    //string FeeType = "";
                                    //for (int i = 0; i < RowCount; i++)
                                    //{
                                    //    FeeType += gridView2.GetRowCellValue(i, "FeeType").ToString();
                                    //}

                                    //if (FeeType.Contains("计方"))
                                    //{
                                    //    DeliveryFee = DeliveryFee * 0.6;
                                    //}
                                    /*
                                    if (AlienGoods.Checked)//毛慧20171028--异形货上浮50%
                                    {
                                        DeliveryFee += DeliveryFee * 0.5;
                                    }
                                    gridView1.SetRowCellValue(0, "DeliveryFee", DeliveryFee + Additional);
                                    decimal newDeliveryFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "DeliveryFee"));//检查香港结算送货费是否小于最低一票--毛慧20171027
                                    List<SqlPara> list1 = new List<SqlPara>();//检查香港结算送货费是否小于最低一票--毛慧20171027
                                    SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_basDeliveryFeeHK", list1);
                                    DataTable dt1 = SqlHelper.GetDataTable(sps1);
                                    DataRow[] arrs = dt1.Select("Province='" + ReceivProvince.Text.Trim() + "' and City='" + ReceivCity.Text.Trim() + "' and Area='" + ReceivArea.Text.Trim() + "' and Street='" + ReceivStreet.Text.Trim() + "'");
                                    decimal temp = 0;
                                    if (arrs != null && arrs.Length > 0)
                                    {
                                        foreach (DataRow arr in arrs)
                                        {
                                            if (Convert.ToDecimal(arr["lowestprice"].ToString()) > temp)
                                            {
                                                temp = Convert.ToDecimal(arr["lowestprice"].ToString());
                                            }
                                        }
                                    }
                                    if ((newDeliveryFee - Convert.ToDecimal(Additional)) < temp)
                                    {
                                        gridView1.SetRowCellValue(0, "DeliveryFee", (temp + Convert.ToDecimal(Additional)));
                                    }
                                    else
                                    {
                                        gridView1.SetRowCellValue(0, "DeliveryFee", newDeliveryFee);
                                    }
                                    */
                                }
                                //else
                                //{
                                //gridView1.SetRowCellValue(0, "DeliveryFee", 0);
                                //}
                            }
                            List<SqlPara> list1 = new List<SqlPara>();//检查香港结算送货费是否小于最低一票--毛慧20171027
                            SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_basDeliveryFeeHK", list1);
                            DataTable dt1 = SqlHelper.GetDataTable(sps1);
                            DataRow[] arrs = dt1.Select("Province='" + ReceivProvince1 + "' and City='" + ReceivCity1 + "' and Area='" + ReceivArea1 + "' and Street='" + ReceivStreet1 + "'");
                            double temp = 0;
                            if (arrs != null && arrs.Length > 0)
                            {
                                foreach (DataRow arr in arrs)
                                {
                                    if (Convert.ToDouble(arr["lowestprice"].ToString()) > temp)
                                    {
                                        temp = Convert.ToDouble(arr["lowestprice"].ToString());
                                    }
                                }
                            }
                            if (temp > DeliveryFee)
                            {
                                DeliveryFee = temp;
                            }
                            if (AlienGoods.Checked)//毛慧20171028--异形货上浮50%
                            {
                                //DeliveryFee += DeliveryFee * 0.5;
                                DeliveryFee += DeliveryFee * 2;//gxh20181206:LMS-1977
                            }
                            gridView1.SetRowCellValue(0, "DeliveryFee", DeliveryFee + Additional);
                        }
                        else if (TransferMode.Text.Trim() == "专车直送")
                        {
                            gridView1.SetRowCellValue(0, "DeliveryFee", gridView1.GetRowCellValue(0, "DeliFee"));//结算送货费等于开单送货费 2018.03.28 ccd
                        }
                        else
                        {
                            gridView1.SetRowCellValue(0, "DeliveryFee", 0);
                        }
                    }
                    else
                    {

                        decimal maxFee = 400;
                        if (TransitMode.Text.Trim() == "中强快线")
                        {
                            maxFee = maxFee * (decimal)1.25;
                        }
                        if (TransitMode.Text.Trim() == "一票通")
                        {
                            maxFee = maxFee * (decimal)1.05;
                        }
                        if (TransferMode.Text == "送货" || TransferMode.Text == "司机直送")//zaj 2018-6-7 司机直送 交接方式结算送货费按送货标准结算
                        {
                            ////送货费调整 2018.03.15 ccd
                            ////string TransitModeStr = TransitMode.Text.Trim();
                            //string sql = "Province='" + ReceivProvince.Text.Trim()
                            //    + "' and City='" + ReceivCity.Text.Trim()
                            //    + "' and Area='" + ReceivArea.Text.Trim()
                            //    + "' and Street='" + ReceivStreet2.Text.Trim() + "'";
                            ////+ "' and TransportMode='" + TransitModeStr + "'";
                            //HJ20181017
                            string sql = "";
                            if (CommonClass.UserInfo.companyid == "239")
                            {
                                sql = "Province='" + ReceivProvince1
                                 + "' and City='" + ReceivCity1
                                 + "' and Area='" + ReceivArea1 + "'";
                                //+ "' and Street='" + ReceivStreet1 + "'";hj20181102可通结算送货费只根据到区来结算
                            }
                            else
                            {
                                sql = "Province='" + ReceivProvince1
                                     + "' and City='" + ReceivCity1
                                     + "' and Area='" + ReceivArea1
                               + "' and Street='" + ReceivStreet1 + "'";
                            }

                            //DataRow[] drDeliveryFee = CommonClass.dsSendPriceNew.Tables[0].Select(sql);
                            //if (drDeliveryFee == null || drDeliveryFee.Length <= 0)
                            //{
                            //    sql = "Province='全国' and City='全国' and Area='全国' and Street='全国' and TransportMode='" + TransitModeStr + "'"; 
                            //    drDeliveryFee = CommonClass.dsSendPriceNew.Tables[0].Select(sql);
                            //}

                            DataRow[] drDeliveryFee = CommonClass.dsSendPrice_ZX.Tables[0].Select(sql);

                            if (drDeliveryFee != null && drDeliveryFee.Length > 0)
                            {
                                decimal DeliveryFee = getDeliveryFee1(drDeliveryFee, Weight);

                                //lyj 2017-10-16 一票通结算干线费上浮5%
                                if (TransitMode.Text.Trim() == "一票通")
                                {
                                    DeliveryFee = DeliveryFee + (DeliveryFee * Convert.ToDecimal("0.05"));
                                }
                                //lyj 2017-10-16 快线结算干线费上浮25%
                                if (TransitMode.Text.Trim() == "中强快线")
                                {
                                    DeliveryFee = DeliveryFee + (DeliveryFee * Convert.ToDecimal("0.25"));
                                }

                                // 如果勾选了异形货，结算费用上浮50%
                                if (AlienGoods.Checked == true)
                                {
                                    //DeliveryFee = DeliveryFee * (decimal)1.5;
                                    DeliveryFee = DeliveryFee * (decimal)2;
                                }

                                // 最低一票30， 最高400封顶
                                //if (DeliveryFee < 30)
                                //{
                                //    DeliveryFee = 30;
                                //}
                                //if (DeliveryFee > maxFee)
                                //{
                                //    DeliveryFee = maxFee;
                                //}

                                //中强整车结算送货费为0 2018.03.15 ccd
                                if (TransitMode.Text.Trim() == "中强整车")
                                {
                                    DeliveryFee = 0;
                                }
                                //中强项目结算送货费为0 2018.03.15 ccd
                                if (TransitMode.Text.Trim() == "中强项目")
                                {
                                    DeliveryFee = 0;
                                }

                                gridView1.SetRowCellValue(0, "DeliveryFee", Math.Round(DeliveryFee, 2));
                            }
                            else
                            {
                                gridView1.SetRowCellValue(0, "DeliveryFee", 0);
                            }
                        }
                        else if (TransferMode.Text == "自提")
                        {
                            #region LMS-2594
                            // 当交接方式为自提，结算中转费为0才计算结算送货费  lyj
                            //decimal transferFee = ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "TransferFee"));
                            //decimal transferFee = ConvertType.ToDecimal(TransferFee.Text.Trim());
                            //if (transferFee <= 0)
                            //{
                            //    // 检查目的网点是否被包含在参数表中心自提免费网点字段的值里
                            //    if (CommonClass.Arg.PickUpFreeWeb.Contains(PickGoodsSite.Text.Trim()) && PickGoodsSite.Text.Trim() != "")//CommonClass.Arg.PickUpFreeWeb.Contains(PickGoodsSite.Text.Trim())zaj20180702当目的网点为空时跳过
                            //    {
                            //        gridView1.SetRowCellValue(0, "DeliveryFee", 0);
                            //    }
                            //    else
                            //    {
                            //        //送货费调整 2018.03.15 ccd
                            //        //string TransitModeStr = TransitMode.Text.Trim();
                            //        //string sql = "Province='" + ReceivProvince1
                            //        //    + "' and City='" + ReceivCity1
                            //        //    + "' and Area='" + ReceivArea1
                            //        //    + "' and Street='" + ReceivStreet1 + "'";
                            //        string sql = "";
                            //        if (CommonClass.UserInfo.companyid == "239")
                            //        {
                            //             sql = "Province='" + ReceivProvince1
                            //              + "' and City='" + ReceivCity1
                            //              + "' and Area='" + ReceivArea1 + "'";
                            //            //+ "' and Street='" + ReceivStreet1 + "'";hj20181102可通结算送货费只根据到区来结算
                            //        }
                            //        else
                            //        {
                            //             sql = "Province='" + ReceivProvince1
                            //                  + "' and City='" + ReceivCity1
                            //                  + "' and Area='" + ReceivArea1
                            //            + "' and Street='" + ReceivStreet1 + "'";
                            //        }
                            //        //+ "' and TransportMode='" + TransitModeStr + "'";

                            //        //DataRow[] drDeliveryFee = CommonClass.dsSendPriceNew.Tables[0].Select(sql);
                            //        //if (drDeliveryFee == null || drDeliveryFee.Length <= 0)
                            //        //{
                            //        //    sql = "Province='全国' and City='全国' and Area='全国' and Street='全国' and TransportMode='" + TransitModeStr + "'";
                            //        //    drDeliveryFee = CommonClass.dsSendPriceNew.Tables[0].Select(sql);
                            //        //}
                            //        DataRow[] drDeliveryFee = CommonClass.dsSendPrice_ZX.Tables[0].Select(sql);

                            //        if (drDeliveryFee != null && drDeliveryFee.Length > 0)
                            //        {
                            //            decimal DeliveryFee = getDeliveryFee1(drDeliveryFee, Weight);

                            //            //lyj 2017-10-16 一票通结算干线费上浮5%
                            //            if (TransitMode.Text.Trim() == "一票通")
                            //            {
                            //                DeliveryFee = DeliveryFee + (DeliveryFee * Convert.ToDecimal("0.05"));
                            //            }
                            //            //lyj 2017-10-16 快线结算干线费上浮25%
                            //            if (TransitMode.Text.Trim() == "中强快线")
                            //            {
                            //                DeliveryFee = DeliveryFee + (DeliveryFee * Convert.ToDecimal("0.25"));
                            //            }

                            //            // 如果勾选了异形货，结算费用上浮50%
                            //            if (AlienGoods.Checked == true)
                            //            {
                            //                //DeliveryFee = DeliveryFee * (decimal)1.5;
                            //                DeliveryFee = DeliveryFee * (decimal)2;
                            //            }

                            //            // 最低一票30， 最高400封顶
                            //            //if (DeliveryFee < 30)
                            //            //{
                            //            //    DeliveryFee = 30;
                            //            //}
                            //            //if (DeliveryFee > maxFee)
                            //            //{
                            //            //    DeliveryFee = maxFee;
                            //            //}



                            //            //非大车直达场站的自提货物按结算送货费标准结算一半送货费调整为按结算送货费标准结算1/4送货费 2018.03.15 ccd
                            //            //DeliveryFee = DeliveryFee * (decimal)0.5;
                            //            DeliveryFee = DeliveryFee * (decimal)0.25;

                            //            //中强整车结算送货费为0 2018.03.15 ccd
                            //            if (TransitMode.Text.Trim() == "中强整车")
                            //            {
                            //                DeliveryFee = 0;
                            //            }
                            //            //中强项目结算送货费为0 2018.03.15 ccd
                            //            if (TransitMode.Text.Trim() == "中强项目")
                            //            {
                            //                DeliveryFee = 0;
                            //            }

                            //            gridView1.SetRowCellValue(0, "DeliveryFee", Math.Round(DeliveryFee, 2));
                            //        }
                            //        else
                            //        {
                            //            gridView1.SetRowCellValue(0, "DeliveryFee", 0);
                            //        }

                            //}
                            //}
                            //else
                            //{
                            //gridView1.SetRowCellValue(0, "DeliveryFee", 0);
                            //}
                            #endregion
                            gridView1.SetRowCellValue(0, "DeliveryFee", 0);
                        }
                        else if (TransferMode.Text.Trim() == "专车直送")
                        {
                            gridView1.SetRowCellValue(0, "DeliveryFee", gridView1.GetRowCellValue(0, "DeliFee"));//结算送货费等于开单送货费 2018.03.28 ccd
                        }
                        else
                        {
                            gridView1.SetRowCellValue(0, "DeliveryFee", 0);
                        }
                        /*
                        else if (TransferMode.Text == "司机直送")
                        {
                            if (gcdaozhan.DataSource != null)
                            {
                                string TransitModeStr = TransitMode.Text.Trim();
                                   
                                string sql = "Province='" + ReceivProvince.Text.Trim()
                                    + "' and City='" + ReceivCity.Text.Trim()
                                    + "' and Area='" + ReceivArea.Text.Trim()
                                    + "' and Street='" + ReceivStreet.Text.Trim()
                                    + "' and TransportMode='" + TransitModeStr + "'";
                                 
                                DataRow[] drDeliveryFee = CommonClass.dsSendPriceNew.Tables[0].Select(sql);
                                if (drDeliveryFee == null || drDeliveryFee.Length <= 0)
                                {
                                    sql = "Province='全国' and City='全国' and Area='全国' and Street='全国' and TransportMode='" + TransitModeStr + "'";
                                    drDeliveryFee = CommonClass.dsSendPriceNew.Tables[0].Select(sql);
                                }

                                if (drDeliveryFee == null || drDeliveryFee.Length > 0)
                                {
                                    decimal DeliveryFee = getDeliveryFee(drDeliveryFee, Weight);

                                    ////lyj 2017-10-16 一票通结算干线费上浮5%
                                    //if (TransitMode.Text.Trim() == "一票通")
                                    //{
                                    //    DeliveryFee = DeliveryFee + (DeliveryFee * Convert.ToDecimal("0.05"));
                                    //}
                                    ////lyj 2017-10-16 快线结算干线费上浮25%
                                    //if (TransitMode.Text.Trim() == "中强快线")
                                    //{
                                    //    DeliveryFee = DeliveryFee + (DeliveryFee * Convert.ToDecimal("0.25"));
                                    //}

                                    // 如果勾选了异形货，结算费用上浮50%
                                    if (AlienGoods.Checked == true)
                                    {
                                        DeliveryFee = DeliveryFee * (decimal)1.5;
                                    }

                                    // 最低一票300， 最高500封顶
                                    if (DeliveryFee < 300)
                                    {
                                        DeliveryFee = 300;
                                    }
                                    if (DeliveryFee > 500)
                                    {
                                        DeliveryFee = 500;
                                    }

                                    gridView1.SetRowCellValue(0, "DeliveryFee", Math.Round(DeliveryFee, 2));
                                }
                                else 
                                {
                                    gridView1.SetRowCellValue(0, "DeliveryFee", 0);
                                }
                            }
                        }
                        else
                        {
                            gridView1.SetRowCellValue(0, "DeliveryFee", 0);
                        } 
                        */
                    }

                    //}
                    //else
                    //{
                    //    gridView1.SetRowCellValue(0, "DeliveryFee", 0);
                    //}

                    #endregion

                    #region 结算始发操作费
                    DataRow[] drDepartureOptFee;
                    if (CommonClass.UserInfo.companyid == "239")
                    {
                        drDepartureOptFee = CommonClass.dsDepartureOptFee_ZX.Tables[0].Select("FromSite='" + Bsite + "' and TransitMode='可通专线'");
                    }
                    else
                    {
                        drDepartureOptFee = CommonClass.dsDepartureOptFee_ZX.Tables[0].Select("FromSite='" + Bsite + "'and TransitMode='" + TransitMode.Text.Trim() + "'");
                    }
                    if (drDepartureOptFee.Length > 0)
                    {
                        decimal HeavyPrice = ConvertType.ToDecimal(drDepartureOptFee[0]["HeavyPrice"]);//重货
                        decimal LightPrice = ConvertType.ToDecimal(drDepartureOptFee[0]["LightPrice"]);//轻货
                        decimal ParcelPriceMin = ConvertType.ToDecimal(drDepartureOptFee[0]["ParcelPriceMin"]);//最低一票
                        decimal acc = Weight * HeavyPrice;

                        //结算始发操作费调整为24元/吨 2018.03.15 ccd
                        //decimal acc = Weight * (decimal)0.024;

                        ////lyj 2017-10-16 一票通结算干线费上浮5%
                        //if (TransitMode.Text.Trim() == "一票通")
                        //{
                        //    acc = acc + (acc * Convert.ToDecimal("0.05"));
                        //}
                        ////lyj 2017-10-16 快线结算干线费上浮25%
                        //if (TransitMode.Text.Trim() == "中强快线")
                        //{
                        //    acc = acc + (acc * Convert.ToDecimal("0.25"));
                        //}

                        // 如果勾选了异形货，结算费用上浮50%
                        if (AlienGoods.Checked == true)
                        {
                            acc = acc * (decimal)1.5;
                        }

                        acc = Math.Max(acc, ParcelPriceMin);

                        //gridView8.SetRowCellValue(0, "DepartureOptFee", Math.Round(acc, 2));
                        DepartureOptFee.Text = Math.Round(acc, 2).ToString();

                    }
                    else
                    {
                        //gridView8.SetRowCellValue(0, "DepartureOptFee", 0);
                        DepartureOptFee.Text = "0";
                    }
                    //SetFeeToPreciousGoodFee();
                    if (PreciousGoods.Checked)
                    {
                        //decimal dec = ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "DepartureOptFee").ToString());
                        // gridView8.SetRowCellValue(0, "DepartureOptFee", dec + 30);
                        decimal dec = ConvertType.ToDecimal(DepartureOptFee.Text.Trim());
                        DepartureOptFee.Text = (dec + 30).ToString();
                    }
                    #endregion

                    #region 结算终端操作费
                    DataRow[] drTerminalOptFee;
                    if (CommonClass.UserInfo.companyid == "239")
                    {
                        drTerminalOptFee = CommonClass.dsTerminalOptFee_ZX.Tables[0].Select("TransferSite='" + MiddleSite + "' and TransitMode='可通专线'");
                    }
                    else
                    {
                        drTerminalOptFee = CommonClass.dsTerminalOptFee_ZX.Tables[0].Select("TransferSite='" + MiddleSite + "'and TransitMode='" + TransitMode.Text.Trim() + "'");
                    }
                    if (drTerminalOptFee.Length > 0 && TransitMode.Text != "中强项目" && TransitMode.Text != "中强城际"
                        && TransferMode.Text != "司机直送")
                    {
                        if ((TransitMode.Text == "一票通" && StartSite.Text == "深圳" && TransferSite.Text == "深圳"))
                        {
                            //gridView8.SetRowCellValue(0, "TerminalOptFee", 0);
                            TerminalOptFee.Text = "0";
                        }
                        else
                        {
                            decimal HeavyPrice = ConvertType.ToDecimal(drTerminalOptFee[0]["HeavyPrice"]);//重货
                            decimal LightPrice = ConvertType.ToDecimal(drTerminalOptFee[0]["LightPrice"]);//轻货
                            decimal ParcelPriceMin = ConvertType.ToDecimal(drTerminalOptFee[0]["ParcelPriceMin"]);//最低一票
                            decimal acc = Weight * HeavyPrice;

                            //结算终端操作费调整为24元/吨 2018.03.15 ccd
                            //decimal acc = Weight * (decimal)0.024;

                            ////lyj 2017-10-16 一票通结算干线费上浮5%
                            //if (TransitMode.Text.Trim() == "一票通")
                            //{
                            //    acc = acc + (acc * Convert.ToDecimal("0.05"));
                            //}
                            ////lyj 2017-10-16 快线结算干线费上浮25%
                            //if (TransitMode.Text.Trim() == "中强快线")
                            //{
                            //    acc = acc + (acc * Convert.ToDecimal("0.25"));
                            //}

                            // 如果勾选了异形货，结算费用上浮50%
                            if (AlienGoods.Checked == true)
                            {
                                acc = acc * (decimal)1.5;
                            }

                            acc = Math.Max(acc, ParcelPriceMin);
                            //gridView8.SetRowCellValue(0, "TerminalOptFee", Math.Round(acc, 2));
                            TerminalOptFee.Text = Math.Round(acc, 2).ToString();
                        }
                    }
                    else
                    {
                        //gridView8.SetRowCellValue(0, "TerminalOptFee", 0);
                        TerminalOptFee.Text = "0";
                    }

                    #endregion
                }
                else
                {
                    //gridView8.SetRowCellValue(0, "TransferFee", 0);
                    gridView1.SetRowCellValue(0, "DeliveryFee", 0);
                    //gridView8.SetRowCellValue(0, "DepartureOptFee", 0);
                    //gridView8.SetRowCellValue(0, "TerminalOptFee", 0);
                    TransferFee.Text = "0";
                    DepartureOptFee.Text = "0";
                    TerminalOptFee.Text = "0";

                }

                #region 结算终端分拨费

                //gridView8.SetRowCellValue(0, "TerminalAllotFee", 0);

                TerminalAllotFee.Text = "0";

                //DataRow[] drTerminalAllotFee = CommonClass.dsTerminalAllotFee.Tables[0].Select("ToSite='" + MiddleSite + "'");
                //if (drTerminalAllotFee.Length > 0 && TransitMode.Text != "中强项目")
                //{
                //    // 不要结算分拨费了
                //    //decimal HeavyPrice = ConvertType.ToDecimal(drTerminalAllotFee[0]["HeavyPrice"]);//重货
                //    //decimal LightPrice = ConvertType.ToDecimal(drTerminalAllotFee[0]["LightPrice"]);//轻货
                //    //decimal ParcelPriceMin = ConvertType.ToDecimal(drTerminalAllotFee[0]["ParcelPriceMin"]);//最低一票
                //    //decimal acc = Math.Max(Weight * HeavyPrice, Volume * LightPrice);
                //    //acc = Math.Max(acc, ParcelPriceMin);
                //    //gridView8.SetRowCellValue(0, "TerminalAllotFee", Math.Round(acc, 2));
                //    //decimal acc = Math.Max(Weight * HeavyPrice, ParcelPriceMin);
                //    //gridView8.SetRowCellValue(0, "TerminalAllotFee", Math.Round(acc, 2));
                //    gridView8.SetRowCellValue(0, "TerminalAllotFee", 0);

                //}
                //else
                //{
                //    gridView8.SetRowCellValue(0, "TerminalAllotFee", 0);
                //}

                #endregion

                #region 结算始发分拨费
                //DataRow[] drTerminalAllotFee = CommonClass.dsDepartureAllotFee_ZX.Tables[0].Select("BegWeb='"
                //+ begWeb.Text.Trim() + "' and TransferSite='" + TransferSite.Text.Trim() + "' and FromSite='" + StartSite.Text.Trim() + "'");

                //if (drTerminalAllotFee == null || drTerminalAllotFee.Length <= 0)
                //{
                //    drTerminalAllotFee = CommonClass.dsDepartureAllotFee_ZX.Tables[0].Select("BegWeb='"
                //    + begWeb.Text.Trim() + "' and TransferSite='全部'");
                //}

                //if (drTerminalAllotFee != null && drTerminalAllotFee.Length > 0)
                //{
                //    decimal HeavyPrice = ConvertType.ToDecimal(drTerminalAllotFee[0]["HeavyPrice"]);//重货
                //    decimal LightPrice = ConvertType.ToDecimal(drTerminalAllotFee[0]["LightPrice"]);//轻货
                //    decimal ParcelPriceMin = ConvertType.ToDecimal(drTerminalAllotFee[0]["ParcelPriceMin"]);//最低一票
                //    decimal acc = Math.Max(Weight * HeavyPrice, Volume * LightPrice);
                //    acc = Math.Max(acc, ParcelPriceMin);
                //    gridView8.SetRowCellValue(0, "DepartureAllotFee", Math.Round(acc, 2));


                //}
                //else
                //{
                //    gridView8.SetRowCellValue(0, "DepartureAllotFee", 0);
                //}
                #endregion

                #region 附加费结算标准

                #region 上楼费
                SetFeeToUpstairFee();
                #endregion

                #region 装卸费
                SetFeeToHandleFee();
                #endregion

                #region 保价费
                SetFeeToSupportFee();
                #endregion

                #region 结算税金
                SetFeeToTaxFee();
                #endregion

                #region 回单费
                SetFeeToReceiptFee();
                #endregion

                #region 代收手续费
                SetFeeToAgentFee();
                #endregion

                #region 叉车费
                SetFeeToForkliftFee();
                #endregion

                #region 控货费
                SetFeeToNoticeFee();
                #endregion

                #region 进仓费
                SetFeeToStorageFee();
                #endregion

                #region 仓储费
                SetFeeToWarehouseFee();
                #endregion

                #region 工本费
                if (TransitMode.Text != "中强整车")
                {
                    int Num = 0;
                    for (int i = 0; i < RowCount; i++)
                    {
                        Num += ConvertType.ToInt32(gridView2.GetRowCellValue(i, "Num"));
                    }
                    if (Num > 0)
                    {
                        //DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='工本费' ");
                        //if (dr.Length > 0)
                        //{
                        //    decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);
                        //    decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]);
                        //    decimal Expense_C = Math.Round(InnerStandard * Num, 2);
                        //    gridView8.SetRowCellValue(0, "Expense_C", Expense_C);
                        //}
                        //取消工本费10元/吨的叠加计算方式，调整工本费结算逻辑为0.1元/件 2018.03.15 ccd
                        decimal Expense_C = (decimal)0.1 * Num;
                        //gridView8.SetRowCellValue(0, "Expense_C", Expense_C);
                        this.Expense_C.Text = Expense_C.ToString();
                    }
                    else
                    {
                        //gridView8.SetRowCellValue(0, "Expense_C", 0);
                        this.Expense_C.Text = "0";
                    }
                }
                else
                {
                    // gridView8.SetRowCellValue(0, "Expense_C", 0);
                    this.Expense_C.Text = "0";
                }
                #endregion

                #region 商超费
                SetFeeToStorageXFee();
                #endregion

                #region 必走货
                //SetFeeToPreciousGoodFee();
                #endregion


                // 两个网格的结算送货费同步
                //gridView8.SetRowCellValue(0, "DeliveryFee", gridView1.GetRowCellValue(0, "DeliveryFee"));
                this.DeliveryFee.Text = (gridView1.GetRowCellValue(0, "DeliveryFee")).ToString();
                #endregion

                setheji();//maohui20180921(结算费用合计)
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }



        //maohui20180921(结算信息合计)
        private void setheji()
        {
            Decimal MainlineFee1 = ConvertType.ToDecimal(MainlineFee.Text.Trim());
            Decimal DeliveryFee1 = ConvertType.ToDecimal(DeliveryFee.Text.Trim());
            Decimal TransferFee1 = ConvertType.ToDecimal(TransferFee.Text.Trim());
            Decimal DepartureOptFee1 = ConvertType.ToDecimal(DepartureOptFee.Text.Trim());
            Decimal TerminalOptFee1 = ConvertType.ToDecimal(TerminalOptFee.Text.Trim());
            Decimal TerminalAllotFee1 = ConvertType.ToDecimal(TerminalAllotFee.Text.Trim());
            //Decimal DepartureAllotFee1 = ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "DepartureAllotFee"));
            Decimal ReceiptFee_C1 = ConvertType.ToDecimal(ReceiptFee_C.Text.Trim());
            Decimal NoticeFee_C1 = ConvertType.ToDecimal(NoticeFee_C.Text.Trim());
            Decimal SupportValue_C1 = ConvertType.ToDecimal(SupportValue_C.Text.Trim());
            Decimal AgentFee_C1 = ConvertType.ToDecimal(AgentFee_C.Text.Trim());
            Decimal PackagFee_C1 = ConvertType.ToDecimal(PackagFee_C.Text.Trim());
            Decimal OtherFee_C1 = ConvertType.ToDecimal(OtherFee_C.Text.Trim());
            Decimal HandleFee_C1 = ConvertType.ToDecimal(HandleFee_C.Text.Trim());
            Decimal StorageFee_C1 = ConvertType.ToDecimal(StorageFee_C.Text.Trim());
            Decimal WarehouseFee_C1 = ConvertType.ToDecimal(WarehouseFee_C.Text.Trim());
            Decimal ForkliftFee_C1 = ConvertType.ToDecimal(ForkliftFee_C.Text.Trim());
            Decimal Tax_C1 = ConvertType.ToDecimal(Tax_C.Text.Trim());
            Decimal ChangeFee_C1 = ConvertType.ToDecimal(ChangeFee_C.Text.Trim());
            Decimal UpstairFee_C1 = ConvertType.ToDecimal(UpstairFee_C.Text.Trim());
            Decimal CustomsFee_C1 = ConvertType.ToDecimal(CustomsFee_C.Text.Trim());
            Decimal FrameFee_C1 = ConvertType.ToDecimal(FrameFee_C.Text.Trim());
            Decimal Expense_C1 = ConvertType.ToDecimal(Expense_C.Text.Trim());
            Decimal FuelFee_C1 = ConvertType.ToDecimal(FuelFee_C.Text.Trim());
            Decimal InformationFee_C1 = ConvertType.ToDecimal(InformationFee_C.Text.Trim());
            decimal PaymentAmout1 = ConvertType.ToDecimal(PaymentAmout.Text);
            //合计
            Decimal heji = MainlineFee1 + DeliveryFee1 + TransferFee1 + DepartureOptFee1 + TerminalOptFee1 + TerminalAllotFee1 + ReceiptFee_C1
                         + NoticeFee_C1 + SupportValue_C1 + AgentFee_C1 + PackagFee_C1 + OtherFee_C1 + HandleFee_C1 + StorageFee_C1 + WarehouseFee_C1
                         + ForkliftFee_C1 + Tax_C1 + ChangeFee_C1 + UpstairFee_C1 + CustomsFee_C1 + FrameFee_C1 + Expense_C1 + FuelFee_C1 + InformationFee_C1;


            //毛利
            Decimal maoli = PaymentAmout1 - (MainlineFee1 + DeliveryFee1 + TransferFee1 + DepartureOptFee1 + ReceiptFee_C1 + NoticeFee_C1 + SupportValue_C1 + AgentFee_C1
                          + PackagFee_C1 + OtherFee_C1 + HandleFee_C1 + StorageFee_C1 + WarehouseFee_C1 + ForkliftFee_C1 + Tax_C1 + ChangeFee_C1 + UpstairFee_C1 + CustomsFee_C1
                          + FrameFee_C1 + Expense_C1 + FuelFee_C1 + InformationFee_C1 + TerminalOptFee1 + TerminalAllotFee1);

            //毛利率
            Decimal maolilv;
            if (PaymentAmout1 != 0)
            {
                //gridView8.SetRowCellValue(0, "heji", heji);
                maolilv = Math.Round(maoli / PaymentAmout1, 2);
                this.heji.Text = heji.ToString();
                this.maoli.Text = maoli.ToString();
                this.maolilv.Text = maolilv.ToString();
            }
            //zb20190619
            else
            {
                this.heji.Text = "0.00";
                this.maoli.Text = "0.00";
                this.maolilv.Text = "0.00";
            }
        }


        /// <summary>
        /// 计算结算送货费
        /// </summary>
        /// <param name="drDeliveryFee"></param>
        //private decimal getDeliveryFee(DataRow[] drDeliveryFee, decimal Weight)
        //{
        //    decimal w0_300 = ConvertType.ToDecimal(drDeliveryFee[0]["w0_300"]);
        //    decimal w300_500 = ConvertType.ToDecimal(drDeliveryFee[0]["w300_500"]);
        //    decimal w500_800 = ConvertType.ToDecimal(drDeliveryFee[0]["w500_800"]);
        //    decimal w800_1000 = ConvertType.ToDecimal(drDeliveryFee[0]["w800_1000"]);
        //    decimal w1000_2000 = ConvertType.ToDecimal(drDeliveryFee[0]["w1000_2000"]);
        //    decimal w2000_3000 = ConvertType.ToDecimal(drDeliveryFee[0]["w2000_3000"]);
        //    decimal w3000_100000 = ConvertType.ToDecimal(drDeliveryFee[0]["w3000_100000"]);

        //    decimal v0_300 = ConvertType.ToDecimal(drDeliveryFee[0]["v0_300"]);
        //    decimal v300_500 = ConvertType.ToDecimal(drDeliveryFee[0]["v300_500"]);
        //    decimal v500_800 = ConvertType.ToDecimal(drDeliveryFee[0]["v500_800"]);
        //    decimal v800_1000 = ConvertType.ToDecimal(drDeliveryFee[0]["v800_1000"]);
        //    decimal v1000_2000 = ConvertType.ToDecimal(drDeliveryFee[0]["v1000_2000"]);
        //    decimal v2000_3000 = ConvertType.ToDecimal(drDeliveryFee[0]["v2000_3000"]);
        //    decimal v3000_100000 = ConvertType.ToDecimal(drDeliveryFee[0]["v3000_100000"]);

        //    //decimal DeliveryFee = ConvertType.ToDecimal(drDeliveryFee[0]["DeliveryFee"]);
        //    decimal DeliveryFee = 0;
        //    decimal wDeliveryFee = 0;
        //    decimal vDeliveryFee = 0;

        //    for (int i = 0; i < RowCount; i++)
        //    {
        //        decimal w = ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeWeight"));
        //        decimal v = ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeVolume"));
        //        string Package = gridView2.GetRowCellValue(i, "Package").ToString();

        //        string FeeType = gridView2.GetRowCellValue(i, "FeeType").ToString();

        //        if (Weight >= 0 && Weight <= 300)
        //        {
        //            wDeliveryFee = w0_300 * w;
        //            vDeliveryFee = v0_300 * v;
        //        }
        //        else if (Weight >= 300 && Weight <= 500)
        //        {
        //            wDeliveryFee = w300_500 * w;
        //            vDeliveryFee = v300_500 * v;
        //        }
        //        else if (Weight >= 500 && Weight <= 800)
        //        {
        //            wDeliveryFee = w500_800 * w;
        //            vDeliveryFee = v500_800 * v;
        //        }
        //        else if (Weight >= 800 && Weight <= 1000)
        //        {
        //            wDeliveryFee = w800_1000 * w;
        //            vDeliveryFee = v800_1000 * v;
        //        }
        //        else if (Weight >= 1000 && Weight <= 2000)
        //        {
        //            wDeliveryFee = w1000_2000 * w;
        //            vDeliveryFee = v1000_2000 * v;
        //        }
        //        else if (Weight >= 2000 && Weight <= 3000)
        //        {
        //            wDeliveryFee = w2000_3000 * w;
        //            vDeliveryFee = v2000_3000 * v;
        //        }
        //        else if (Weight > 3000)
        //        {
        //            wDeliveryFee = w3000_100000 * w;
        //            vDeliveryFee = v3000_100000 * v;
        //        }

        //        if (Package != "纸箱" && Package != "纤袋" && Package != "膜")
        //        {

        //            wDeliveryFee = wDeliveryFee * Convert.ToDecimal(CommonClass.Arg.MainlineFeeRate);
        //            vDeliveryFee = vDeliveryFee * Convert.ToDecimal(CommonClass.Arg.MainlineFeeRate);

        //        }


        //        if (FeeType == "计重")
        //        {
        //            DeliveryFee += wDeliveryFee;
        //        }
        //        else
        //        {
        //            DeliveryFee += vDeliveryFee;
        //        }
        //    }
        //    return DeliveryFee;
        //}

        /// <summary>
        /// 计算结算送货费 2018.03.15 ccd
        /// </summary>
        /// <param name="drDeliveryFee"></param>
        private decimal getDeliveryFee1(DataRow[] drDeliveryFee, decimal Weight)
        {
            decimal DeliveryFee = 0;
            decimal w0_300 = ConvertType.ToDecimal(drDeliveryFee[0]["w0_300"]);
            decimal w300_1000 = ConvertType.ToDecimal(drDeliveryFee[0]["w300_1000"]);
            decimal w1000_3000 = ConvertType.ToDecimal(drDeliveryFee[0]["w1000_3000"]);
            decimal w3000_5000 = ConvertType.ToDecimal(drDeliveryFee[0]["w3000_5000"]);
            decimal w5000_8000 = ConvertType.ToDecimal(drDeliveryFee[0]["w5000_8000"]);
            decimal w8000_ = ConvertType.ToDecimal(drDeliveryFee[0]["w8000_"]);//8吨以上


            if (Weight >= 0 && Weight <= 300)
            {
                DeliveryFee = w0_300;
            }
            else if (Weight > 300 && Weight <= 1000)
            {
                DeliveryFee = w300_1000;
            }
            else if (Weight > 1000 && Weight <= 3000)
            {
                DeliveryFee = w1000_3000;
            }
            else if (Weight > 3000 && Weight <= 5000)
            {
                DeliveryFee = w3000_5000;
            }
            else if (Weight > 5000 && Weight <= 8000)
            {
                DeliveryFee = w5000_8000;
            }
            if (Weight > 8000)
            {
                DeliveryFee = w8000_;
            }
            return DeliveryFee;
        }



        #region 结算函数
        /// <summary>
        /// 代收货款结算
        /// </summary>
        private void SetFeeToAgentFee()
        {
            string billno = this.BillNo.Text;
            decimal InnerLowest = 0;
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillNo", billno));
            DataRow[] dr = null;
            decimal SupportValue_C = 0;
            decimal InnerStandard = 0;
            //修改/改单申请
            if (isModify == 1 || Upd_Num == 1)
            {
                SqlParasEntity sqs = new SqlParasEntity(OperType.Query, "QSP_GET_billDepartureFBList_ByBillNo", list);
                DataSet ds = SqlHelper.GetDataSet(sqs);
                //判断是否为转分拨
                if (ds != null || ds.Tables.Count > 0 || ds.Tables[0].Rows.Count > 0)
                {
                    dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='代收手续费' ");
                }
                else
                {
                    dr = CommonClass.dsSurchargeFee_ZX.Tables[0].Select("ProjectType='代收手续费' ");
                }
                //判断是否维护了结算标准
                if (dr.Length > 0)
                {
                    InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]); //代收手续费结算标准
                    InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);  //最低一票标准
                }

            }
            else
            {
                dr = CommonClass.dsSurchargeFee_ZX.Tables[0].Select("ProjectType='代收手续费' ");
                //判断是否维护了结算标准
                if (dr.Length > 0)
                {
                    InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]); //代收手续费结算标准 
                    InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);  //最低一票标准
                }

            }

            if (!IsAgentFee.Checked)// && ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "DiscountTransfer")) == 0)
            {
                //gridView8.SetRowCellValue(0, "AgentFee_C", 0);
                this.AgentFee_C.Text = "0.00";
            }
            else
            {
                decimal CollectionPayFee = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "CollectionPay"));
                decimal AgentFee_C = CollectionPayFee * InnerStandard;   //zb20190610
                //gridView1.SetRowCellValue(0, "AgentFee", AgentFee_C);
                if (AgentFee_C < InnerLowest)
                {
                    this.AgentFee_C.Text = InnerLowest.ToString("0.00");
                }
                else
                {
                    this.AgentFee_C.Text = AgentFee_C.ToString("0.00");
                }
            }
            //decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);
            //if (ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "CollectionPay")) > 0)
            //{
            //decimal AgentFee_C = Math.Round(Math.Max(InnerLowest, CollectionPayFee * InnerStandard), 2);
            //gridView8.SetRowCellValue(0, "AgentFee_C", AgentFee_C);
            //}
            //else
            //{
            //    //折扣折让/（总运费-折扣折让）
            //    if (ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "DiscountTransfer")) > 0 && (ConvertType.ToDecimal(PaymentAmout.Text) - ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "DiscountTransfer"))) > 0 && ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "DiscountTransfer")) / (ConvertType.ToDecimal(PaymentAmout.Text) - ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "DiscountTransfer"))) >= 3)
            //    {
            //        decimal DiscountTransferFee = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "DiscountTransfer"));
            //        decimal AgentFee_C = Math.Round(Math.Max(InnerLowest, DiscountTransferFee * InnerStandard), 2);
            //        gridView8.SetRowCellValue(0, "AgentFee_C", AgentFee_C);
            //    }
            //    else
            //    {
            //        gridView8.SetRowCellValue(0, "AgentFee_C", InnerLowest);
            //    }
            //}
            setheji();//maohui20180921(结算费用合计)

        }

        /// <summary>
        /// 叉车费结算
        /// </summary>
        private void SetFeeToForkliftFee()
        {
            decimal ForkliftFee = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "ForkliftFee"));
            if (ForkliftFee > 0)
            {
                DataRow[] dr = CommonClass.dsSurchargeFee_ZX.Tables[0].Select("ProjectType='叉车费' ");
                if (dr.Length > 0)
                {
                    decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);
                    decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]);
                    decimal ForkliftFee_C = Math.Round(Math.Max(InnerLowest, ForkliftFee * InnerStandard), 2);
                    //gridView8.SetRowCellValue(0, "ForkliftFee_C", ForkliftFee_C);
                    this.ForkliftFee_C.Text = ForkliftFee_C.ToString("0.00");
                }
            }
            else
            {
                //gridView8.SetRowCellValue(0, "ForkliftFee_C", 0);
                this.ForkliftFee_C.Text = "0";
            }
            setheji();
        }

        /// <summary>
        /// 税金结算
        /// </summary>
        private void SetFeeToTaxFee()
        {
            if (!IsInvoice.Checked)
            {
                //gridView8.SetRowCellValue(0, "Tax_C", 0);
                Tax_C.Text = "0";
            }
            else
            {
                DataRow[] dr = CommonClass.dsSurchargeFee_ZX.Tables[0].Select("ProjectType='税金' ");
                if (dr.Length > 0)
                {
                    decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);
                    decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]);
                    //税金算法  总运费-代收货款-税金输入 DiscountTransfer
                    decimal Tax = ConvertType.ToDecimal(PaymentAmout.Text) - ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "CollectionPay")) - ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "Tax"));
                    //if (Tax > 0)
                    //{
                    decimal Tax_C = Math.Round(Math.Max(InnerLowest, Tax * InnerStandard), 2);
                    //gridView8.SetRowCellValue(0, "Tax_C", Tax_C);
                    this.Tax_C.Text = Tax_C.ToString("0.00");
                    //}
                    //else
                    //    gridView8.SetRowCellValue(0, "Tax_C", InnerLowest);
                }
            }
            setheji();//maohui20180921(结算费用合计)
        }
        /// <summary>
        /// 保价费结算
        /// </summary>
        private void SetFeeToSupportFee()
        {
            string billno = this.BillNo.Text;
            decimal InnerLowest = 0;
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillNo", billno));
            DataRow[] dr = null;
            decimal SupportValue_C = 0;
            decimal InnerStandard = 0;
            //修改/改单申请
            if (isModify == 1 || Upd_Num == 1)
            {
                SqlParasEntity sqs = new SqlParasEntity(OperType.Query, "QSP_GET_billDepartureFBList_ByBillNo", list);
                DataSet ds = SqlHelper.GetDataSet(sqs);
                //判断是否为转分拨
                if (ds != null || ds.Tables.Count > 0 || ds.Tables[0].Rows.Count > 0)
                {
                    dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='保价费' ");
                    InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);  //最低一票标准
                }
                else
                {
                    dr = CommonClass.dsSurchargeFee_ZX.Tables[0].Select("ProjectType='保价费' ");
                    InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);  //最低一票标准
                }
                //判断是否维护了结算标准
                if (dr.Length > 0)
                {
                    InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]); //保价费结算标准 
                    InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);  //最低一票标准
                }

            }
            //新开
            else
            {
                dr = CommonClass.dsSurchargeFee_ZX.Tables[0].Select("ProjectType='保价费' ");
                //判断是否维护了结算标准
                if (dr.Length > 0)
                {
                    InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]); //保价费结算标准 
                    InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);  //最低一票标准
                }
            }
            if (!IsSupportValue.Checked)
            {
                //gridView8.SetRowCellValue(0, "SupportValue_C", 0);
                this.SupportValue_C.Text = "0.00";
            }
            else
            {

                // decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);
                decimal SupportValue = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "DeclareValue"));
                // decimal SupportValue = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "DeclareValue"));
                // decimal SupportValue_C = Math.Round(Math.Max(InnerLowest, SupportValue * InnerStandard), 2);
                SupportValue_C = InnerStandard * SupportValue;
               // this.gridView1.SetRowCellValue(0, "SupportValue", SupportValue_C);
                //最低一票
                if (SupportValue_C < InnerLowest)
                {
                    this.SupportValue_C.Text = InnerLowest.ToString("0.00");
                }
                else
                {
                    this.SupportValue_C.Text = SupportValue_C.ToString("0.00");
                }
            }

            //decimal SupportValue = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "DeclareValue"));
            //if (SupportValue < 5000)
            //{
            //    SupportValue_C = 3;
            //}
            //else
            //{
            //    SupportValue_C = 3 + (SupportValue - 5000) * (decimal)0.0005;
            //}
            ////gridView8.SetRowCellValue(0, "SupportValue_C", SupportValue_C);
            //this.SupportValue_C.Text = SupportValue_C.ToString("0.00");


            setheji();//maohui20180921(结算费用合计)
        }
        /// <summary>
        /// 必走货费结算 luohui
        /// </summary>
        private void SetFeeToPreciousGoodFee()
        {
            decimal DepartureOptFee = 0;
            if (PreciousGoods.Checked)
            {
                //必走货算在始发操作费
                //商超费
                DataRow[] dr = CommonClass.dsSurchargeFee_ZX.Tables[0].Select("ProjectType='必走货' ");
                if (dr.Length > 0)
                {
                    //decimal DepartureOptFee = ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "DepartureOptFee"));
                    decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]); //最低一票金额 
                    DepartureOptFee = DepartureOptFee + InnerLowest;
                    this.DepartureOptFee.Text = DepartureOptFee.ToString("0.00");


                }
            }
        }
        /// <summary>
        /// 商超费结算
        /// </summary>
        private void SetFeeToStorageXFee()
        {
            decimal InnerStandard = 0;
            decimal InnerStandard1 = 0;
            double InnerLowest1 = 0;
            DataRow[] dr = null;
            DataRow[] dr1 = null;

            string billno = this.BillNo.Text;
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillNo", billno));
            //判断是否为修改或改单申请
            if (isModify == 1 || Upd_Num == 1)
            {
                SqlParasEntity sqs = new SqlParasEntity(OperType.Query, "QSP_GET_billDepartureFBList_ByBillNo", list);
                DataSet ds = SqlHelper.GetDataSet(sqs);
                //判断是否为转分拨
                if (ds != null || ds.Tables.Count > 0 || ds.Tables[0].Rows.Count > 0)
                {
                    dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='商超费' ");
                    dr1 = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='进仓费' ");
                }
                else
                {
                    dr = CommonClass.dsSurchargeFee_ZX.Tables[0].Select("ProjectType='商超费' ");
                    dr1 = CommonClass.dsSurchargeFee_ZX.Tables[0].Select("ProjectType='进仓费' ");
                }
                //判断是否维护了结算标准
                if (dr.Length > 0)
                {
                    InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]); //商超费结算标准 
                }
                if (dr1.Length > 0)
                {
                    InnerStandard1 = ConvertType.ToDecimal(dr1[0]["InnerStandard"]); //进仓费结算标准
                    InnerLowest1 = ConvertType.ToDouble(dr1[0]["InnerLowest"]);  //最低一票标准
                }

            }
            //新开
            else
            {
                dr = CommonClass.dsSurchargeFee_ZX.Tables[0].Select("ProjectType='商超费' ");
                dr1 = CommonClass.dsSurchargeFee_ZX.Tables[0].Select("ProjectType='进仓费' ");
                //判断是否维护了结算标准
                if (dr.Length > 0)
                {
                    InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]); //商超费结算标准 
                }
                if (dr1.Length > 0)
                {
                    InnerStandard1 = ConvertType.ToDecimal(dr1[0]["InnerStandard"]); //进仓费结算标准
                    InnerLowest1 = ConvertType.ToDouble(dr1[0]["InnerLowest"]);  //最低一票标准

                }
            }
            decimal OperationWeight_1 = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "OperationWeight")); //结算重量
            if (!MarketSuper.Checked)
            {
                if (IsStorageFee.Checked)
                {
                    //结算费用小于最低一票
                    if (Convert.ToDouble((InnerStandard1 * OperationWeight_1 / 1000 + 50)) <= InnerLowest1)
                    {
                        this.StorageFee_C.Text = InnerLowest1.ToString("0.00");
                    }
                    else
                    {
                        this.StorageFee_C.Text = Convert.ToDouble((InnerStandard1 * OperationWeight_1 / 1000 + 50)).ToString("0.00");
                    }
                   
                }
                else
                {
                    this.StorageFee_C.Text = "0.00";
                }
            }
            else
            {
                if (IsStorageFee.Checked)
                {
                    if (Convert.ToDouble((InnerStandard1 * OperationWeight_1 / 1000 + 50)) <= InnerLowest1)
                    {
                        this.StorageFee_C.Text = (Convert.ToDouble(InnerStandard) + InnerLowest1).ToString("0.00");
                    }
                    else
                    {
                        this.StorageFee_C.Text = (Convert.ToDouble(InnerStandard) + Convert.ToDouble((InnerStandard1 * OperationWeight_1 / 1000 + 50))).ToString("0.00"); //zb20190610 
                    }
                    
                }
                else
                {
                    this.StorageFee_C.Text = InnerStandard.ToString("0.00");
                    
                }

            }

            setheji();
        }
        /// <summary>
        /// 进仓费结算
        /// </summary>
        private void SetFeeToStorageFee()
        {
            decimal InnerStandard = 0;
            decimal InnerStandard1 = 0;
            double InnerLowest1 = 0;
            DataRow[] dr = null;
            DataRow[] dr1 = null;

            string billno = this.BillNo.Text;
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillNo", billno));
            //判断是否为修改或改单申请
            if (isModify == 1 || Upd_Num == 1)
            {
                SqlParasEntity sqs = new SqlParasEntity(OperType.Query, "QSP_GET_billDepartureFBList_ByBillNo", list);
                DataSet ds = SqlHelper.GetDataSet(sqs);
                //判断是否为转分拨
                if (ds != null || ds.Tables.Count > 0 || ds.Tables[0].Rows.Count > 0)
                {
                    dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='商超费' ");
                    dr1 = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='进仓费' ");
                }
                else
                {
                    dr = CommonClass.dsSurchargeFee_ZX.Tables[0].Select("ProjectType='商超费' ");
                    dr1 = CommonClass.dsSurchargeFee_ZX.Tables[0].Select("ProjectType='进仓费' ");
                }
                //判断是否维护了结算标准
                if (dr.Length > 0)
                {
                    InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]); //商超费结算标准 
                }
                if (dr1.Length > 0)
                {
                    InnerStandard1 = ConvertType.ToDecimal(dr1[0]["InnerStandard"]); //进仓费结算标准 
                    InnerLowest1 = ConvertType.ToDouble(dr1[0]["InnerLowest"]);  //最低一票标准
                }

            }
            //新开
            else
            {
                dr = CommonClass.dsSurchargeFee_ZX.Tables[0].Select("ProjectType='商超费' ");
                dr1 = CommonClass.dsSurchargeFee_ZX.Tables[0].Select("ProjectType='进仓费' ");
                //判断是否维护了结算标准
                if (dr.Length > 0)
                {
                    InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]); //商超费结算标准
                }
                if (dr1.Length > 0)
                {
                    InnerStandard1 = ConvertType.ToDecimal(dr1[0]["InnerStandard"]); //进仓费结算标准 
                    InnerLowest1 = ConvertType.ToDouble(dr1[0]["InnerLowest"]);  //最低一票标准
                }
            }
            decimal OperationWeight_1 = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "OperationWeight")); //结算重量
            if (!MarketSuper.Checked)
            {
                if (IsStorageFee.Checked)
                {
                    //结算费用小于最低一票
                    if (Convert.ToDouble((InnerStandard1 * OperationWeight_1 / 1000 + 50)) <= InnerLowest1)
                    {
                        this.StorageFee_C.Text = InnerLowest1.ToString("0.00");
                    }
                    else
                    {
                        this.StorageFee_C.Text = Convert.ToDouble((InnerStandard1 * OperationWeight_1 / 1000 + 50)).ToString("0.00");
                    }
                   
                }
                else
                {
                    this.StorageFee_C.Text = "0.00";
                }
            }
            else
            {
                if (IsStorageFee.Checked)
                {
                    //gridView1.SetRowCellValue(0, "StorageFee", InnerStandard1 + InnerStandard);
                    //最低一票
                    if (Convert.ToDouble((InnerStandard1 * OperationWeight_1 / 1000 + 50)) <= InnerLowest1)
                    {
                        this.StorageFee_C.Text = (Convert.ToDouble(InnerStandard) + InnerLowest1).ToString("0.00");
                    }
                    else
                    {
                        this.StorageFee_C.Text = (Convert.ToDouble(InnerStandard) + Convert.ToDouble((InnerStandard1 * OperationWeight_1 / 1000 + 50))).ToString("0.00"); //zb20190610 
                    }
                }
                else
                {
                    this.StorageFee_C.Text = InnerStandard.ToString("0.00");
                }

            }
            //SetFeeToStorageXFee();
            setheji();//maohui20180921(结算费用合计)
        }

        /// <summary>
        /// 仓储费结算
        /// </summary>
        private void SetFeeToWarehouseFee()
        {
            decimal WarehouseFee = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "WarehouseFee"));
            if (WarehouseFee > 0)
            {
                DataRow[] dr = CommonClass.dsSurchargeFee_ZX.Tables[0].Select("ProjectType='仓储费' ");
                if (dr.Length > 0)
                {
                    decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);
                    decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]);
                    decimal WarehouseFee_C = Math.Round(Math.Max(InnerLowest, WarehouseFee * InnerStandard), 2);
                    //gridView8.SetRowCellValue(0, "WarehouseFee_C", WarehouseFee_C);
                    this.WarehouseFee_C.Text = WarehouseFee_C.ToString("0.00");
                }
            }
            else
            {
                //gridView8.SetRowCellValue(0, "WarehouseFee_C", 0);
                this.WarehouseFee_C.Text = "0";
            }
            setheji();
        }
        //<summary>
        //控货费结算
        //</summary>
        private void SetFeeToNoticeFee()
        {
            if (!NoticeState.Checked)
            {
                //gridView8.SetRowCellValue(0, "NoticeFee_C", 0);
                NoticeFee_C.Text = "0";
            }
            else
            {
                DataRow[] dr = CommonClass.dsSurchargeFee_ZX.Tables[0].Select("ProjectType='控货费' ");
                if (dr.Length > 0)
                {
                    //控货费费 最低10元一票 
                    decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);
                    //gridView8.SetRowCellValue(0, "NoticeFee_C", InnerLowest);
                    this.NoticeFee_C.Text = InnerLowest.ToString("0.00");
                }
            }
            setheji();//maohui20180921(结算费用合计)
        }
        //<summary>
        //装卸费结算   
        //</summary>
        private void SetFeeToHandleFee()
        {
            decimal HandleFee_C1 = 0;//装卸费
            decimal SortFee_C1 = 0;//分拣费
            decimal OperationWeight_1 = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "OperationWeight")); //结算重量
            if (!IsHandleFee.Checked && !IsSortFee.Checked)
            {

                HandleFee_C.Text = "0";
            }
            else
            {
                if (IsHandleFee.Checked)
                {
                    DataRow[] dr = CommonClass.dsSurchargeFee_ZX.Tables[0].Select("ProjectType='装卸费' ");
                    if (dr.Length > 0)
                    {
                        decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);
                        decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]);
                        decimal HandleFee = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "HandleFee"));
                        HandleFee_C1 = Math.Round(Math.Max(InnerLowest, (OperationWeight_1 * InnerStandard) / 1000), 2);

                    }
                }
                if (IsSortFee.Checked)
                {
                    DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='分拣费' ");
                    if (dr.Length > 0)
                    {
                        decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);
                        decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]);
                        SortFee_C1 = Math.Round(Math.Max(InnerLowest, (OperationWeight_1 * InnerStandard) / 1000), 2);
                    }
                }
                HandleFee_C.Text = (HandleFee_C1 + SortFee_C1).ToString();

            }
            //setheji();//maohui20180921(结算费用合计)
        }
        //<summary>
        //上楼费结算
        //</summary>
        private void SetFeeToUpstairFee()
        {
            if (!IsUpstairFee.Checked)
            {
                //gridView8.SetRowCellValue(0, "UpstairFee_C", 0);
                UpstairFee_C.Text = "0";
            }
            else
            {
                DataRow[] dr = CommonClass.dsSurchargeFee_ZX.Tables[0].Select("ProjectType='上楼费' ");
                if (dr.Length > 0)
                {
                    decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]); //最低一票金额
                    decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]); //结算标准 
                    decimal UpstairFee = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "UpstairFee"));
                    //if (UpstairFee > 0)
                    //{
                    decimal OperationWeight_1 = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "OperationWeight")); //结算重量
                    //if (OperationWeight_1 <= 0)
                    //{
                    //    MsgBox.ShowOK("上楼费必须输入结算重量");
                    //    return;
                    //}
                    //结算标准 * 结算重量 >= 最低一票标准
                    decimal UpstairFee_C = Math.Round(Math.Max(InnerLowest, (OperationWeight_1 * InnerStandard) / 1000), 2);
                    //gridView8.SetRowCellValue(0, "UpstairFee_C", UpstairFee_C);
                    this.UpstairFee_C.Text = UpstairFee_C.ToString("0.00");
                    //}
                    //else
                    //    gridView8.SetRowCellValue(0, "UpstairFee_C", InnerLowest);
                }
                else
                {
                    //提示 没有附加费配置
                    // MsgBox.ShowOK("必须存在附加费！");
                    return;
                }
            }
            setheji();//maohui20180921(结算费用合计)
        }
        //<summary>
        //回单费结算
        //</summary>
        private void SetFeeToReceiptFee()
        {
            if (!IsReceiptFee.Checked)
            {
                //gridView8.SetRowCellValue(0, "ReceiptFee_C", 0);
                ReceiptFee_C.Text = "0";
            }
            else
            {
                DataRow[] dr = CommonClass.dsSurchargeFee_ZX.Tables[0].Select("ProjectType='回单费' ");
                if (dr.Length > 0)
                {
                    decimal ReceiptFee = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "ReceiptFee"));
                    decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);
                    decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]);

                    // 如果是大车直达则取最低一票，否则取标准
                    //decimal transferFee = ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "TransferFee"));
                    decimal transferFee = ConvertType.ToDecimal(TransferFee.Text);
                    if (transferFee <= 0 && CommonClass.Arg.PickUpFreeWeb.Contains(PickGoodsSite.Text.Trim()) && PickGoodsSite.Text.Trim() != "")//&& CommonClass.Arg.PickUpFreeWeb.Contains(PickGoodsSite.Text.Trim())
                    {
                        //gridView8.SetRowCellValue(0, "ReceiptFee_C", InnerLowest);
                        ReceiptFee_C.Text = InnerLowest.ToString("0.00");
                    }
                    else
                    {
                        //gridView8.SetRowCellValue(0, "ReceiptFee_C", InnerStandard);
                        ReceiptFee_C.Text = InnerStandard.ToString("0.00");
                    }
                }
                else
                {
                    //提示 没有附加费配置
                    // MsgBox.ShowOK("必须存在附加费！");
                    return;
                }
            }
            setheji();//maohui20180921(结算费用合计)
        }

        /// 视图跟踪费  luohui
        /// </summary>
        private void SetFeeToViewTraceFee()
        {
            if (!IsViewTraceFee.Checked)
            {

                OtherFee_C.Text = "0";
            }
            else
            {
                DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='视图跟踪费' ");
                if (dr.Length > 0)
                {
                    decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]);
                    decimal OtherFee_C = 0;
                    OtherFee_C = Math.Round(Math.Max(OtherFee_C, OtherFee_C * InnerStandard), 2);
                    this.OtherFee_C.Text = InnerStandard.ToString("0.00");

                    //gridView8.SetRowCellValue(0, "OtherFee_C", Math.Round(InnerStandard, 2));

                }
                else
                {
                    //提示 没有附加费配置
                    MsgBox.ShowOK("必须存在附加费！");
                    return;
                }
            }
        }
        #endregion

        #endregion


        private void TransferSite_EditValueChanged(object sender, EventArgs e)
        {

            //zaj 2018-2-27
            // SetPickGoodsSite();
            //SetFee();
            SetFeeNew();//zaj 2018-4-15新结算
        }

        private void TransitMode_EditValueChanged(object sender, EventArgs e)
        {
            if (TransitMode.Text != "中强整车")
            {
                VehicleNo.Enabled = false;
                VehicleNo.Text = "";
            }
            else
            {
                VehicleNo.Enabled = true;
            }
            //SetFee();
            SetFeeNew();//zaj 2018-4-15新结算
        }

        //private void ReceivStreet_Enter(object sender, EventArgs e)
        //{
        //    gcdaozhan.Left = ReceivStreet.Left;
        //    gcdaozhan.Top = ReceivStreet.Top + ReceivStreet.Height;
        //    gcdaozhan.Visible = true;
        //    gcdaozhan.BringToFront();
        //}

        //private void ReceivStreet_EditValueChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        gridColumn5.FilterInfo = new ColumnFilterInfo("MiddleStreet LIKE '%" + ReceivStreet.Text.Trim() + "%'");
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}

        //private void ReceivStreet_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Down)
        //    {
        //        gcdaozhan.Focus();
        //    }
        //}

        //private void ReceivStreet_Leave(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DataTable table = (DataTable)gcdaozhan.DataSource;
        //        //TransitMode.Properties.Items.Clear();
        //        if (ReceivStreet.Text.Trim() != "" && !gcdaozhan.Focused)
        //        {

        //            DataRow[] dr = table.Select("MiddleStreet='" + ReceivStreet.Text.Trim() + "'");
        //            if (dr.Length == 0)
        //            {
        //                ReceivStreet.Focus();
        //                Tool.ToolTip.ShowTip("请选择正确的街道", ReceivStreet, ToolTipLocation.RightTop);
        //                return;
        //            }
        //            else
        //            {
        //                //string SiteName = dr[0]["SiteName"].ToString();
        //                //TransferSite.Text = SiteName == "" ? CommonClass.UserInfo.SiteName : SiteName;
        //                //if (SiteName == "")
        //                //{
        //                //    TransitMode.Properties.Items.Add("中强项目");
        //                //    TransitMode.Properties.Items.Add("中强整车");
        //                //}
        //                //else if (SiteName != "" && StartSite.Text == TransferSite.Text)
        //                //{
        //                //    TransitMode.Properties.Items.Add("中强城际");
        //                //    TransitMode.Properties.Items.Add("中强整车");
        //                //}
        //                //else
        //                //{
        //                //    TransitMode.Properties.Items.Add("中强快线");
        //                //    TransitMode.Properties.Items.Add("中强专线");
        //                //    TransitMode.Properties.Items.Add("中强整车");
        //                //}
        //            }
        //        }
        //        if (!gcdaozhan.Focused)
        //        {
        //            gcdaozhan.Visible = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MsgBox.ShowOK(ex.Message);
        //    }
        //}

        //private void ReceivProvince_Enter(object sender, EventArgs e)
        //{
        //    //ReceivProvince.ShowPopup();
        //    gridControl3.Left = ReceivProvince.Left;
        //    gridControl3.Top = ReceivProvince.Top + ReceivProvince.Height;
        //    gridControl3.Visible = true;
        //    gridControl3.BringToFront();
        //}

        //private void ReceivCity_Enter(object sender, EventArgs e)
        //{
        //    //ReceivCity.ShowPopup();
        //    gridControl9.Left = ReceivCity.Left;
        //    gridControl9.Top = ReceivCity.Top + ReceivCity.Height;
        //    gridControl9.Visible = true;
        //    gridControl9.BringToFront();
        //}

        //private void ReceivArea_Enter(object sender, EventArgs e)
        //{
        //    //ReceivArea.ShowPopup();
        //    gridControl10.Left = ReceivArea.Left;
        //    gridControl10.Top = ReceivArea.Top + ReceivArea.Height;
        //    gridControl10.Visible = true;
        //    gridControl10.BringToFront();
        //}

        //private void TransferMode_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        bool flag = false;
        //        if (NotStreet == null)
        //        {
        //            flag = false;
        //        }
        //        else
        //        {
        //            flag = true;
        //        }

        //        if (TransferMode.Text == "自提")
        //        {
        //            //hj20180719
        //            if (flag)
        //            {
        //                ReceivAddress.Text = ReceivProvince.Text + ReceivCity.Text + ReceivArea.Text + ReceivStreet.Text;
        //                ReceivAddress.Enabled = true;
        //                setSite1();
        //            }
        //            else
        //            {
        //                ReceivAddress.Text = "";
        //                ReceivAddress.Enabled = false;
        //                //zaj 2017-11-29
        //                setSite1();
        //            }
        //        }
        //        //hj20180412 江西的详细地址只显示三级
        //        else if (CommonClass.UserInfo.companyid == "106" || CommonClass.UserInfo.companyid == "119")
        //        {
        //            ReceivAddress.Text = ReceivProvince.Text + ReceivCity.Text + ReceivArea.Text;
        //            ReceivAddress.Enabled = true;
        //        }
        //       //hj20180719 无锡送货不用必填四级地址
        //        else if (flag)
        //        {
        //            ReceivAddress.Text = ReceivProvince.Text + ReceivCity.Text + ReceivArea.Text + ReceivStreet.Text;
        //            ReceivAddress.Enabled = true;
        //            setSite1();
        //        }
        //        else
        //        {
        //            ReceivAddress.Text = ReceivProvince.Text + ReceivCity.Text + ReceivArea.Text + ReceivStreet.Text;
        //            ReceivAddress.Enabled = true;
        //        }

        //        DataTable table = (DataTable)gcdaozhan.DataSource;
        //        // TransitMode.Properties.Items.Clear();//zaj 2017-12-22
        //        // TransitMode.Text = "";//zaj 2017-12-22
        //        if (ReceivStreet.Text.Trim() != "" && !gcdaozhan.Focused)
        //        {
        //            DataRow[] dr = table.Select("MiddleProvince='" + ReceivProvince.Text.Trim() + "' AND MiddleStreet='" + ReceivStreet.Text.Trim() + "'");
        //            if (dr.Length > 0)
        //            {
        //                string SiteName = dr[0]["SiteName"].ToString();
        //                TransferSite.Properties.Items.Clear();
        //                if (SiteName != "")
        //                {
        //                    //zaj 2018-2-27
        //                    //string[] SiteNames = SiteName.Split(',');
        //                    //for (int i = 0; i < SiteNames.Length; i++)
        //                    //{
        //                    //    TransferSite.Properties.Items.Add(SiteNames[i]);
        //                    //}
        //                    //TransferSite.Text = SiteNames[0];
        //                }
        //                else
        //                {
        //                    TransferSite.Text = SiteName == "" ? CommonClass.UserInfo.SiteName : SiteName;
        //                }

        //                //todo ljp 2016-09-19 加入判断城际不受直达站点限制
        //                if (StartSite.Text != TransferSite.Text) //非城际范围
        //                {

        //                    DataRow[] drendsite = CommonClass.dsSite.Tables[0].Select("SiteName='" + StartSite.Text.Trim() + "' and EndSiteRang like '%" + TransferSite.Text.Trim() + "%'");

        //                    if (SiteName == "" || drendsite.Length == 0)
        //                    {
        //                        // TransitMode.Properties.Items.Add("中强项目");//zaj 2017-12-22
        //                        //TransitMode.Properties.Items.Add("中强整车");
        //                    }
        //                    else if (SiteName != "" && StartSite.Text == TransferSite.Text)
        //                    {
        //                        //if (TransferMode.Text.Trim() == "网点送货" || TransferMode.Text.Trim() == "自提")
        //                        {
        //                            // TransitMode.Properties.Items.Add("中强城际");//zaj 2017-12-22
        //                            // TransitMode.Properties.Items.Add("中强整车");//zaj 2017-12-22
        //                            //TransitMode.Properties.Items.Add("中强项目");//zaj 2017-12-22
        //                        }
        //                        //else
        //                        //{
        //                        //    if (TransferMode.Text.Trim() == "司机直送")
        //                        //    {
        //                        //        TransitMode.Properties.Items.Add("中强城际");
        //                        //        TransitMode.Properties.Items.Add("中强整车");
        //                        //        TransitMode.Properties.Items.Add("中强项目");

        //                        //    }
        //                        //    else
        //                        //    {
        //                        //        TransitMode.Properties.Items.Add("中强项目");
        //                        //        TransitMode.Properties.Items.Add("中强整车");
        //                        //    }
        //                        //}
        //                    }
        //                    else
        //                    {
        //                        // TransitMode.Properties.Items.Add("中强快线");//zaj 2017-12-22
        //                        // TransitMode.Properties.Items.Add("中强专线");//zaj 2017-12-22
        //                        //TransitMode.Properties.Items.Add("中强整车");//zaj 2017-12-22
        //                        // TransitMode.Properties.Items.Add("中强项目");//zaj 2017-12-22
        //                    }
        //                }
        //                else //城际范围 新加
        //                {
        //                    if (SiteName == "")
        //                    {
        //                        // TransitMode.Properties.Items.Add("中强项目");//zaj 2017-12-22
        //                    }

        //                    if (SiteName != "" && StartSite.Text == TransferSite.Text)
        //                    {
        //                        //if (TransferMode.Text.Trim() == "网点送货" || TransferMode.Text.Trim() == "自提")
        //                        {
        //                            //TransitMode.Properties.Items.Add("中强城际");//zaj 2017-12-22
        //                            // TransitMode.Properties.Items.Add("中强整车");//zaj 2017-12-22
        //                            // TransitMode.Properties.Items.Add("中强项目");//zaj 2017-12-22
        //                        }
        //                        //else
        //                        //{
        //                        //    if (TransferMode.Text.Trim() == "司机直送")
        //                        //    {
        //                        //        TransitMode.Properties.Items.Add("中强城际");
        //                        //        TransitMode.Properties.Items.Add("中强整车");
        //                        //        TransitMode.Properties.Items.Add("中强项目");

        //                        //    }
        //                        //    else
        //                        //    {
        //                        //        TransitMode.Properties.Items.Add("中强项目");
        //                        //        TransitMode.Properties.Items.Add("中强整车");
        //                        //    }

        //                        //}
        //                    }
        //                    else if (StartSite.Text.Trim() != TransferSite.Text.Trim())
        //                    {
        //                        //TransitMode.Properties.Items.Add("中强快线");//zaj 2017-12-22
        //                        // TransitMode.Properties.Items.Add("中强专线");//zaj 2017-12-22
        //                        //TransitMode.Properties.Items.Add("中强整车");//zaj 2017-12-22
        //                        //TransitMode.Properties.Items.Add("中强项目");//zaj 2017-12-22
        //                    }
        //                }
        //            }
        //        }
        //        //SetFee();
        //        SetFeeNew();//zaj 2018-4-15新结算
        //    }
        //    catch (Exception ex)
        //    {
        //        //MsgBox.ShowOK(ex.Message);
        //    }
        //}

        private void ReceivAddress_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    gridControl2.Focus();
            //    gridView2.FocusedRowHandle = 0;
            //    gridView2.FocusedColumn = gridView2.Columns["Varieties"];
            //    gridView2.ShowEditor();
            //}
        }

        private void gridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (gridView1.FocusedColumn != null)
                {
                    int VisibleColumnsCount = gridView1.VisibleColumns.Count; //显示出来的列数(隐藏列不统计在内)
                    if (gridView1.FocusedColumn.VisibleIndex == VisibleColumnsCount - 1)
                    {
                        PaymentMode.Focus();
                        e.Handled = true;
                    }
                }
            }
        }

        //private void ReceivProvince_TextChanged(object sender, EventArgs e)
        //{
        //    ReceivAddress.Text = TransferMode.Text == "自提" ? "" : ReceivProvince.Text + ReceivCity.Text + ReceivArea.Text + ReceivStreet.Text;
        //    DestinationSite.Text = ReceivProvince.Text + " " + ReceivCity.Text + " " + ReceivArea.Text;



        //    if (!string.IsNullOrEmpty(ReceivProvince.Text.Trim())) ///ljp 2017-03-18 增加香港地区去除网点送货
        //    {
        //        //zaj 2017-11-15 增加香港地区去除网点送货
        //        //if (ReceivProvince.Text.Trim().Contains("香港"))
        //        //{
        //        //    if (TransferMode.Properties.Items.Contains("网点送货"))
        //        //    {
        //        //        TransferMode.Properties.Items.Remove("网点送货");
        //        //    }
        //        //}
        //        //else
        //        //{
        //        //    if (!TransferMode.Properties.Items.Contains("网点送货"))
        //        //    {
        //        //        TransferMode.Properties.Items.Add("网点送货");
        //        //    }
        //        //}
        //    }

        //    TransferMode.Text = "";
        //    PickGoodsSite.Text = "";
        //    TransitMode.Text = "";
        //    SetPickGoodsSite();
        //}

        //private void ReceivAddress_EditValueChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //hj20180827
        //        bool flag = true;

        //        if (NotStreet == null)
        //        {
        //            flag = true;
        //        }
        //        else
        //        {
        //            flag = false;
        //        }
        //        if (TransferMode.Text == "自提")
        //        {
        //            //hj20180712
        //            if (flag)
        //            {
        //                ReceivAddress.Text = "";
        //                return;
        //            }
        //        }

        //        //string addr = ReceivProvince.Text + ReceivCity.Text + ReceivArea.Text + ReceivStreet.Text;
        //        //zaj 街道信息可以删除到区县 2017-12-19
        //       // string addr = ReceivProvince.Text + ReceivCity.Text + ReceivArea.Text;

        //        if (ReceivAddress.Text.Length < addr.Length)
        //        {
        //            ReceivAddress.Text = addr;
        //        }
        //        string eadd = ReceivAddress.Text.Substring(0, addr.Length);
        //        if (addr != eadd)
        //        {
        //            //ReceivAddress.Text = addr;
        //            //hj20180418
        //            ReceivAddress.Text = ReceivProvince.Text + ReceivCity.Text + ReceivArea.Text + ReceivStreet.Text;
        //        }
        //    }
        //    catch (Exception) { }
        //}

        private void ReceivCity_Click(object sender, EventArgs e)
        {
            //ReceivCity.ShowPopup();
        }

        private void ReceivArea_Click(object sender, EventArgs e)
        {
            //ReceivArea.ShowPopup();
        }

        //private void ReceivProvince_EditValueChanged(object sender, EventArgs e)
        //{
        //    ReceivCity.Text = "";
        //    ReceivArea.Text = "";
        //    ReceivStreet.Text = "";
        //    SetCity();
        //    LoadStreet();
        //}

        //private void ReceivCity_EditValueChanged(object sender, EventArgs e)
        //{
        //    ReceivArea.Text = "";
        //    ReceivStreet.Text = "";
        //    SetArea();
        //    LoadStreet();
        //    DestinationSite.Text = ReceivCity.Text + ReceivArea.Text;
        //}

        //private void ReceivArea_EditValueChanged(object sender, EventArgs e)
        //{
        //    //CommonClass.SetStreet(ReceivStreet, ReceivProvince.Text.Trim(), ReceivCity.Text.Trim(), ReceivArea.Text.Trim());

        //    ReceivStreet.Text = "";
        //    LoadStreet();
        //    DestinationSite.Text = ReceivCity.Text + ReceivArea.Text;
        //}

        //private void LoadStreet()
        //{
        //    try
        //    {
        //        ReceivStreet.Text = "";
        //        DataTable tmp = new DataTable();
        //        string sql = "MiddleProvince='" + ReceivProvince.Text.Trim() + "' and MiddleCity='" + ReceivCity.Text.Trim() + "' and MiddleArea='" + ReceivArea.Text.Trim() + "'";

        //        DataTable MiddleSiteTable = CommonClass.dsMiddleSite.Tables[0];
        //        DataRow[] drsite = MiddleSiteTable.Select(sql);
        //        if (drsite.Length > 0)
        //        {
        //            tmp = drsite[0].Table.Clone();
        //            foreach (DataRow row in drsite)
        //            {
        //                string sql2 = "Province='" + ReceivProvince.Text.Trim() + "' and City='" + ReceivCity.Text.Trim() + "' and Area='" + ReceivArea.Text.Trim() + "' and " + "Street='" + row["MiddleStreet"] + "'";
        //                DataTable dt_Send = CommonClass.dsSendPrice.Tables[0];
        //                DataRow[] drprice = dt_Send.Select(sql2);
        //                if (drprice.Length > 0 && row["type"].ToString().Contains("送"))
        //                {
        //                    foreach (DataRow item in drprice)
        //                    {
        //                        //row["w0_200"] = item["w0_200"];
        //                        //row["w200_1000"] = item["w200_1000"];
        //                        //row["w1000_3000"] = item["w1000_3000"];
        //                        //row["w3000_5000"] = item["w3000_5000"];
        //                        //row["w5000_100000"] = item["w5000_100000"];
        //                        row["TransferMode"] = item["TransferMode"];
        //                        tmp.ImportRow(row);
        //                    }
        //                }
        //                else
        //                {
        //                    tmp.ImportRow(row);
        //                }
        //            }
        //        }
        //        gcdaozhan.DataSource = tmp;

        //        //DataSet ds = new DataSet();
        //        //ds.Tables.Add(CommonClass.dsMiddleSite.Tables[0]);
        //        //ds.Tables.Add(CommonClass.dsSendPrice.Tables[0]);
        //        //DataColumn[] parentcolumn = new DataColumn[]{
        //        //CommonClass.dsMiddleSite.Tables[0].Columns["MiddleProvince"],
        //        //CommonClass.dsMiddleSite.Tables[0].Columns["MiddleCity"],
        //        //CommonClass.dsMiddleSite.Tables[0].Columns["MiddleArea"],
        //        //CommonClass.dsMiddleSite.Tables[0].Columns["MiddleStreet"]};

        //        //DataColumn[] childcolumn = new DataColumn[]{
        //        //CommonClass.dsSendPrice.Tables[0].Columns["Province"],
        //        //CommonClass.dsSendPrice.Tables[0].Columns["City"],
        //        //CommonClass.dsSendPrice.Tables[0].Columns["Area"],
        //        //CommonClass.dsSendPrice.Tables[0].Columns["Street"]};

        //        //DataRelation Rel = new DataRelation("RelationColumn", parentcolumn, childcolumn);
        //        //ds.Relations.Add(Rel);
        //    }
        //    catch (Exception ex)
        //    {
        //        MsgBox.ShowException(ex);
        //    }
        //}

        //private void SetProvince()
        //{
        //    string site = CommonClass.UserInfo.SiteName;
        //    DataTable DtPro = new DataTable();
        //    DtPro.Columns.Add("MiddleProvince", typeof(string));
        //    List<string> list = new List<string>();
        //    for (int i = 0; i < CommonClass.dsMiddleSite.Tables[0].Rows.Count; i++)
        //    {
        //        if (!list.Contains(CommonClass.dsMiddleSite.Tables[0].Rows[i]["MiddleProvince"].ToString()))
        //        {
        //            list.Add(CommonClass.dsMiddleSite.Tables[0].Rows[i]["MiddleProvince"].ToString());
        //        }
        //    }
        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        DtPro.Rows.Add(new object[] { list[i] });
        //    }
        //    gridControl3.DataSource = DtPro;
        //}

        //private void ReceivProvince_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        //{
        //    if (e.NewValue != null)
        //    {
        //        gridView3.ClearColumnsFilter();
        //        gridView3.Columns["MiddleProvince"].FilterInfo = new ColumnFilterInfo("[MiddleProvince] LIKE " + "'%" + e.NewValue.ToString() + "%'");
        //    }
        //    else
        //    {
        //        gridView3.ClearColumnsFilter();
        //    }
        //}

        //private void gridControl3_DoubleClick(object sender, EventArgs e)
        //{
        //    setProvinceText();
        //}

        //private void setProvinceText()
        //{
        //    int rows = gridView3.FocusedRowHandle;
        //    if (rows < 0) return;
        //    DataRow dr = gridView3.GetDataRow(rows);
        //    ReceivProvince.Text = dr["MiddleProvince"].ToString();
        //    gridControl3.Visible = false;
        //    ReceivCity.Focus();
        //}

        private void SetProvince()
        {
            DataTable DtPro = new DataTable();
            DtPro.Columns.Add("MiddleProvince", typeof(string));
            List<string> list = new List<string>();
            for (int i = 0; i < CommonClass.dsMiddleSite.Tables[0].Rows.Count; i++)
            {
                if (!list.Contains(CommonClass.dsMiddleSite.Tables[0].Rows[i]["MiddleProvince"].ToString()))
                {
                    list.Add(CommonClass.dsMiddleSite.Tables[0].Rows[i]["MiddleProvince"].ToString());
                }
            }
            for (int i = 0; i < list.Count; i++)
            {
                Province.Properties.Items.Add(list[i]);
            }
            //gridControl3.DataSource = DtPro;
        }

        private void SetCity()
        {
            City.Properties.Items.Clear();
            DataTable DtPro = new DataTable();
            DtPro.Columns.Add("MiddleCity", typeof(string));
            List<string> list = new List<string>();
            DataRow[] CityRow = CommonClass.dsMiddleSite.Tables[0].Select("MiddleProvince='" + Province.Text.Trim() + "'");
            for (int i = 0; i < CityRow.Length; i++)
            {
                if (!list.Contains(CityRow[i]["MiddleCity"].ToString()))
                {
                    list.Add(CityRow[i]["MiddleCity"].ToString());
                }
            }
            for (int i = 0; i < list.Count; i++)
            {
                City.Properties.Items.Add(list[i]);
            }
            //gridControl9.DataSource = DtPro;
        }

        //private void SetArea()
        //{
        //    DataTable DtPro = new DataTable();
        //    DtPro.Columns.Add("MiddleArea", typeof(string));
        //    List<string> list = new List<string>();
        //    DataRow[] CityRow = CommonClass.dsMiddleSite.Tables[0].Select("MiddleProvince='" + ReceivProvince.Text.Trim() + "' and MiddleCity='" + ReceivCity.Text.Trim() + "'");
        //    for (int i = 0; i < CityRow.Length; i++)
        //    {
        //        if (!list.Contains(CityRow[i]["MiddleArea"].ToString()))
        //        {
        //            list.Add(CityRow[i]["MiddleArea"].ToString());
        //        }
        //    }
        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        DtPro.Rows.Add(new object[] { list[i] });
        //    }
        //    gridControl10.DataSource = DtPro;
        //}

        //private void ReceivCity_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        //{
        //    if (e.NewValue != null)
        //    {
        //        gridView11.ClearColumnsFilter();
        //        gridView11.Columns["MiddleCity"].FilterInfo = new ColumnFilterInfo("[MiddleCity] LIKE " + "'%" + e.NewValue.ToString() + "%'");
        //    }
        //    else
        //    {
        //        gridView11.ClearColumnsFilter();
        //    }
        //}

        //private void ReceivArea_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        //{
        //    if (e.NewValue != null)
        //    {
        //        gridView12.ClearColumnsFilter();
        //        gridView12.Columns["MiddleArea"].FilterInfo = new ColumnFilterInfo("[MiddleArea] LIKE " + "'%" + e.NewValue.ToString() + "%'");
        //    }
        //    else
        //    {
        //        gridView12.ClearColumnsFilter();
        //    }
        //}

        //private void gridControl9_DoubleClick(object sender, EventArgs e)
        //{
        //    setCityText();
        //}

        //private void setCityText()
        //{
        //    int rows = gridView11.FocusedRowHandle;
        //    if (rows < 0) return;
        //    DataRow dr = gridView11.GetDataRow(rows);
        //    ReceivCity.Text = dr["MiddleCity"].ToString();
        //    gridControl9.Visible = false;
        //    ReceivArea.Focus();
        //}

        //private void gridControl10_DoubleClick(object sender, EventArgs e)
        //{
        //    setAreaText();
        //}

        //private void setAreaText()
        //{
        //    int rows = gridView12.FocusedRowHandle;
        //    if (rows < 0) return;
        //    DataRow dr = gridView12.GetDataRow(rows);
        //    ReceivArea.Text = dr["MiddleArea"].ToString();
        //    gridControl10.Visible = false;
        //    ReceivStreet.Focus();
        //}

        //private void gridControl3_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        setProvinceText();
        //    }
        //}

        //private void gridControl9_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        setCityText();
        //    }
        //}

        //private void gridControl10_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        setAreaText();
        //    }
        //}

        //private void ReceivProvince_Leave(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (ReceivProvince.Text.Trim() != "" && !gridControl3.Focused)
        //        {
        //            DataTable table = (DataTable)gridControl3.DataSource;
        //            DataRow[] dr = table.Select("MiddleProvince='" + ReceivProvince.Text.Trim() + "'");
        //            if (dr.Length == 0)
        //            {
        //                ReceivProvince.Focus();
        //                Tool.ToolTip.ShowTip("请选择正确省份", ReceivProvince, ToolTipLocation.RightTop);
        //                return;
        //            }
        //        }

        //        if (gridView3.RowCount == 1)
        //        {
        //            ReceivProvince.Text = gridView3.GetRowCellValue(0, "MiddleProvince").ToString();
        //        }

        //        if (!gridControl3.Focused)
        //        {
        //            gridControl3.Visible = false;
        //        }
        //    }
        //    catch (Exception)
        //    { }
        //}

        //private void ReceivCity_Leave(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (ReceivCity.Text.Trim() != "" && !gridControl9.Focused)
        //        {
        //            DataTable table = (DataTable)gridControl9.DataSource;
        //            DataRow[] dr = table.Select("MiddleCity='" + ReceivCity.Text.Trim() + "'");
        //            if (dr.Length == 0)
        //            {
        //                ReceivCity.Focus();
        //                Tool.ToolTip.ShowTip("输入错误", ReceivCity, ToolTipLocation.RightTop);
        //                return;
        //            }
        //        }

        //        if (gridView11.RowCount == 1)
        //        {
        //            ReceivCity.Text = gridView11.GetRowCellValue(0, "MiddleCity").ToString();
        //        }
        //        if (!gridControl9.Focused)
        //        {
        //            gridControl9.Visible = false;
        //        }
        //    }
        //    catch (Exception)
        //    { }
        //}

        //private void ReceivArea_Leave(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (ReceivArea.Text.Trim() != "" && !gridControl10.Focused)
        //        {
        //            DataTable table = (DataTable)gridControl10.DataSource;
        //            DataRow[] dr = table.Select("MiddleArea='" + ReceivArea.Text.Trim() + "'");
        //            if (dr.Length == 0)
        //            {
        //                ReceivArea.Focus();
        //                Tool.ToolTip.ShowTip("输入错误", ReceivArea, ToolTipLocation.RightTop);
        //                return;
        //            }
        //        }

        //        if (gridView12.RowCount == 1)
        //        {
        //            ReceivArea.Text = gridView12.GetRowCellValue(0, "MiddleArea").ToString();
        //        }
        //        if (!gridControl10.Focused)
        //        {
        //            gridControl10.Visible = false;
        //        }
        //        //zaj
        //        // setSite1();
        //    }
        //    catch (Exception)
        //    { }
        //}

        //private void ReceivProvince_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Down)
        //    { gridControl3.Focus(); }
        //    if (e.KeyCode == Keys.Escape)
        //    {
        //        gridControl3.Visible = false;
        //    }
        //}

        //private void ReceivCity_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Down)
        //    { gridControl9.Focus(); }
        //    if (e.KeyCode == Keys.Escape)
        //    {
        //        gridControl9.Visible = false;
        //    }
        //}

        //private void ReceivArea_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Down)
        //    { gridControl10.Focus(); }
        //    if (e.KeyCode == Keys.Escape)
        //    {
        //        gridControl10.Visible = false;
        //    }
        //}

        private void ConsigneeName_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (ConsigneeName.Text.Trim() == "") return;
            frmRecentPrice frm = new frmRecentPrice();
            frm.customername = ConsigneeName.Text.Trim();
            frm.Show();
        }

        private void ConsignorName_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (ConsignorName.Text.Trim() == "") return;
            frmRecentPrice frm = new frmRecentPrice();
            frm.customername = ConsignorName.Text.Trim();
            frm.Show();
        }


        //private void BillNo_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        ReceivProvince.Focus();
        //    }
        //}

        private void TransferMode_TextChanged(object sender, EventArgs e)
        {
            //SetPickGoodsSite();
        }

        //private void SetPickGoodsSite()
        //{
        //    try
        //    {

        //        bool flag = false;
        //        if (NotStreet == null)
        //        {
        //            flag = false;
        //        }
        //        else
        //        {
        //            flag = true;
        //        }
        //        if (TransferMode.Text.Trim() == "") return;
        //        string type = "";
        //        if (TransferMode.Text.Trim() == "自提")
        //        {
        //            type = "自提";
        //        }
        //        else
        //        {
        //            type = "送";
        //        }
        //        PickGoodsSite.Properties.Items.Clear();
        //        PickGoodsSite.Text = "";
        //        if (type == "自提" && ReceivStreet.Text.Trim() == "")
        //        {
        //            //DataRow[] dr = ((DataTable)gcdaozhan.DataSource).Select(" type like '%" + type + "%'");
        //            DataRow[] dr = ((DataTable)gcdaozhan.DataSource).Select(" SiteName='" + TransferSite.Text.Trim() + "' and type like '%" + type + "%'");

        //            //if (TransferMode.Text.Trim() != "中心直送")
        //            //{
        //            for (int i = 0; i < dr.Length; i++)
        //            {
        //                if (!PickGoodsSite.Properties.Items.Contains(dr[i]["WebName"]))
        //                    PickGoodsSite.Properties.Items.Add(dr[i]["WebName"]);
        //            }
        //            //}
        //            ArrayList list = new ArrayList();
        //            for (int i = 0; i < dr.Length; i++)
        //            {
        //                if (!list.Contains(dr[i]["WebName"]))
        //                    list.Add(dr[i]["WebName"]);
        //            }
        //            //if (TransferMode.Text.Trim() == "中心直送")
        //            //{
        //            //    DataRow[] drCenter = CommonClass.dsDirectSendFee.Tables[0].Select("SiteName='" + TransferSite.Text + "'");
        //            //    for (int i = 0; i < drCenter.Length; i++)
        //            //    {
        //            //        if (!list.Contains(drCenter[i]["CenterName"]))//如果四级地址的目的网点是中心直送网点就移除掉目的网点
        //            //        {
        //            //            PickGoodsSite.Properties.Items.Add(drCenter[i]["CenterName"]);
        //            //        }
        //            //        else
        //            //        {
        //            //            PickGoodsSite.Properties.Items.Remove(drCenter[i]["CenterName"]);
        //            //            PickGoodsSite.Text = "";
        //            //        }
        //            //    }
        //            //}
        //            if (PickGoodsSite.Properties.Items.Count > 0)
        //            {
        //                PickGoodsSite.SelectedIndex = 0;
        //            }

        //        }
        //        //hj20180719 无锡送货不用必填四级地址
        //        else if (type == "送" && ReceivStreet.Text.Trim() == "" &&  flag)
        //        {
        //            DataRow[] dr = ((DataTable)gcdaozhan.DataSource).Select(" SiteName='" + TransferSite.Text.Trim() + "' and type like '%" + type + "%'");

        //            for (int i = 0; i < dr.Length; i++)
        //            {
        //                if (!PickGoodsSite.Properties.Items.Contains(dr[i]["WebName"]))
        //                    PickGoodsSite.Properties.Items.Add(dr[i]["WebName"]);
        //            }

        //            ArrayList list = new ArrayList();
        //            for (int i = 0; i < dr.Length; i++)
        //            {
        //                if (!list.Contains(dr[i]["WebName"]))
        //                    list.Add(dr[i]["WebName"]);
        //            }
        //            if (PickGoodsSite.Properties.Items.Count > 0)
        //            {
        //                PickGoodsSite.SelectedIndex = 0;
        //            }
        //        }
        //        //DataRelation dr = new DataRelation(）
        //        else
        //        {

        //            //DataRow[] dr = ((DataTable)gcdaozhan.DataSource).Select("MiddleStreet='" + ReceivStreet.Text + "' and type like '%" + type + "%'");
        //            DataRow[] dr = ((DataTable)gcdaozhan.DataSource).Select(" SiteName='" + TransferSite.Text.Trim() + "' and MiddleStreet='" + ReceivStreet.Text + "' and type like '%" + type + "%'");
        //            // if (TransferMode.Text.Trim() != "中心直送")
        //            // {
        //            for (int i = 0; i < dr.Length; i++)
        //            {
        //                if (!PickGoodsSite.Properties.Items.Contains(dr[i]["WebName"]))
        //                    PickGoodsSite.Properties.Items.Add(dr[i]["WebName"]);
        //            }
        //            //  }
        //            ArrayList list = new ArrayList();
        //            for (int i = 0; i < dr.Length; i++)
        //            {
        //                if (!list.Contains(dr[i]["WebName"]))
        //                    list.Add(dr[i]["WebName"]);
        //            }
        //            //if (TransferMode.Text.Trim() == "中心直送")
        //            //{
        //            //    DataRow[] drCenter = CommonClass.dsDirectSendFee.Tables[0].Select("SiteName='" + TransferSite.Text + "'");
        //            //    for (int i = 0; i < drCenter.Length; i++)
        //            //    {
        //            //        if (!list.Contains(drCenter[i]["CenterName"]))//如果四级地址的目的网点是中心直送网点就移除掉目的网点
        //            //        {
        //            //            PickGoodsSite.Properties.Items.Add(drCenter[i]["CenterName"]);
        //            //        }
        //            //        else
        //            //        {
        //            //            PickGoodsSite.Properties.Items.Remove(drCenter[i]["CenterName"]);
        //            //            PickGoodsSite.Text = "";
        //            //        }
        //            //    }
        //            //}
        //            if (PickGoodsSite.Properties.Items.Count > 0)
        //            {
        //                PickGoodsSite.SelectedIndex = 0;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //MsgBox.ShowOK(ex.Message);
        //    }

        //    #region  作废
        //    //try
        //    //{

        //    //    #region zaj 2017-12-16更换取目的网点的方式
        //    //    List<SqlPara> list = new List<SqlPara>();
        //    //    list.Add(new SqlPara("WebName",CommonClass.UserInfo.WebName));
        //    //    list.Add(new SqlPara("TransferSite",TransferSite.Text.Trim()));
        //    //   // SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_EndWeb", list);
        //    //    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_EndWeb_Clone", list);

        //    //    DataSet ds = SqlHelper.GetDataSet(sps);
        //    //    if (ds == null || ds.Tables[0].Rows.Count <= 0)
        //    //    {
        //    //        PickGoodsSite.Properties.Items.Clear();
        //    //        PickGoodsSite.Text = "";
        //    //        return;
        //    //    }
        //    //    PickGoodsSite.Properties.Items.Clear();
        //    //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //    //    {
        //    //        PickGoodsSite.Properties.Items.Add(ds.Tables[0].Rows[i]["EndWeb"].ToString());
        //    //    }
        //    //        //string[] pickGoodsSiteList = ds.Tables[0].Rows[0]["EndWeb"].ToString().Split(',');
        //    //        //for (int i = 0; i < pickGoodsSiteList.Length; i++)
        //    //        //{
        //    //        //    PickGoodsSite.Properties.Items.Add(pickGoodsSiteList[i]);
        //    //        //}
        //    //    #endregion
        //    //        if (PickGoodsSite.Properties.Items.Count > 0)
        //    //        {
        //    //            PickGoodsSite.SelectedIndex = 0;
        //    //        }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    //MsgBox.ShowOK(ex.Message);
        //    //}

        //    #endregion
        //}

        private void WebBillNo_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BillNo.Text.Trim() == "")
            {
                MsgBox.ShowOK("请输入运单号！");
                return;
            }

            #region 检测单号
            if (isModify == 0)
            {
                int billno_state = 4;
                string billno_new = BillNo.Text.Trim();
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("billno", BillNo.Text.Trim()));
                list.Add(new SqlPara("webid", begWeb.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_QSP_GET_BILL_EXIST", list);
                DataSet dsBill = SqlHelper.GetDataSet(sps);
                if (dsBill != null && dsBill.Tables.Count > 0 && dsBill.Tables[0].Rows.Count > 0)
                {
                    billno_state = ConvertType.ToInt32(dsBill.Tables[0].Rows[0]["state"]);
                }
                if (billno_state != 1)
                {
                    string msg = "";
                    if (billno_state == 4)
                    {
                        msg = "本网点中没有单号为 " + billno_new + " 的托运单\r\n\r\n或者没有在系统中为本网点分配单号 " + billno_new + " ,请确认!";
                    }
                    else if (billno_state == 2)
                    {
                        msg = "本网点中的托运单号 " + billno_new + " 已经使用,请确认!";
                    }
                    else if (billno_state == 3)
                    {
                        msg = "本网点中的托运单号 " + billno_new + " 已经作废,不能重复使用,请确认!";
                    }
                    if (msg != "")
                    {
                        MsgBox.ShowOK(msg);
                        BillNo.Focus();
                        BillNo.Select(0, billno_new.Length);
                        return;
                    }
                }
            }
            #endregion
            Savebill__Draft();
        }

        private void Savebill__Draft()
        {
            string GoodsType = gridView2.GetRowCellValue(0, "GoodsType").ToString();
            string Varieties = gridView2.GetRowCellValue(0, "Varieties").ToString();
            string Package = gridView2.GetRowCellValue(0, "Package").ToString();
            decimal Num = Convert.ToDecimal(gridView2.GetRowCellValue(0, "Num").ToString() == "" ? "0" : gridView2.GetRowCellValue(0, "Num").ToString());
            decimal FeeWeight = Convert.ToDecimal(gridView2.GetRowCellValue(0, "FeeWeight").ToString() == "" ? "0" : gridView2.GetRowCellValue(0, "FeeWeight").ToString());
            decimal FeeVolume = Convert.ToDecimal(gridView2.GetRowCellValue(0, "FeeVolume").ToString() == "" ? "0" : gridView2.GetRowCellValue(0, "FeeVolume").ToString());

            decimal Weight = Convert.ToDecimal(gridView2.GetRowCellValue(0, "Weight").ToString() == "" ? "0" : gridView2.GetRowCellValue(0, "Weight").ToString());
            decimal Volume = Convert.ToDecimal(gridView2.GetRowCellValue(0, "Volume").ToString() == "" ? "0" : gridView2.GetRowCellValue(0, "Volume").ToString());
            decimal WeightPrice = Convert.ToDecimal(gridView2.GetRowCellValue(0, "WeightPrice").ToString() == "" ? "0" : gridView2.GetRowCellValue(0, "WeightPrice").ToString());
            decimal VolumePrice = Convert.ToDecimal(gridView2.GetRowCellValue(0, "VolumePrice").ToString() == "" ? "0" : gridView2.GetRowCellValue(0, "VolumePrice").ToString());
            string FeeType = gridView2.GetRowCellValue(0, "FeeType").ToString();
            decimal Freight = Convert.ToDecimal(gridView2.GetRowCellValue(0, "Freight").ToString() == "" ? "0" : gridView2.GetRowCellValue(0, "Freight").ToString());

            decimal DeliFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "DeliFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "DeliFee").ToString());
            decimal DeclareValue = Convert.ToDecimal(gridView1.GetRowCellValue(0, "DeclareValue").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "DeclareValue").ToString());
            decimal SupportValue = Convert.ToDecimal(gridView1.GetRowCellValue(0, "SupportValue").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "SupportValue").ToString());
            decimal Tax = Convert.ToDecimal(gridView1.GetRowCellValue(0, "Tax").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "Tax").ToString());
            decimal InformationFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "InformationFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "InformationFee").ToString());

            decimal Expense = Convert.ToDecimal(gridView1.GetRowCellValue(0, "Expense").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "Expense").ToString());
            decimal DiscountTransfer = Convert.ToDecimal(gridView1.GetRowCellValue(0, "DiscountTransfer").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "DiscountTransfer").ToString());
            decimal CollectionPay = Convert.ToDecimal(gridView1.GetRowCellValue(0, "CollectionPay").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "CollectionPay").ToString());
            decimal AgentFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "AgentFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "AgentFee").ToString());
            decimal FuelFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "FuelFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "FuelFee").ToString());

            decimal UpstairFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "UpstairFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "UpstairFee").ToString());
            decimal HandleFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "HandleFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "HandleFee").ToString());
            decimal ForkliftFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "ForkliftFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "ForkliftFee").ToString());
            decimal StorageFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "StorageFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "StorageFee").ToString());
            decimal CustomsFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "CustomsFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "CustomsFee").ToString());
            decimal packagFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "packagFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "packagFee").ToString());
            decimal MatPay = Convert.ToDecimal(gridView1.GetRowCellValue(0, "MatPay").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "MatPay").ToString());//HJ20180911

            decimal FrameFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "FrameFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "FrameFee").ToString());
            decimal ChangeFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "ChangeFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "ChangeFee").ToString());
            decimal OtherFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "OtherFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "OtherFee").ToString());
            decimal NoticeFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "NoticeFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "NoticeFee").ToString());
            decimal ReceiptFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "ReceiptFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "ReceiptFee").ToString());
            decimal ReceivFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "ReceivFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "ReceivFee").ToString());

            decimal WarehouseFee = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "WarehouseFee"));

            decimal ActualFreight = ConvertType.ToDecimal(PaymentAmout.Text) + CollectionPay + DiscountTransfer;


            string VarietiesStr = "", PackageStr = "", NumStr = "", FeeWeightStr = "", FeeVolumeStr = "",
                WeightStr = "", VolumeStr = "", WeightPriceStr = "", VolumePriceStr = "", FeeTypeStr = "", FreightStr = "";

            for (int i = 1; i < RowCount; i++)
            {
                if (gridView2.GetRowCellValue(i, "Varieties").ToString() != "")
                {
                    VarietiesStr += gridView2.GetRowCellValue(i, "Varieties").ToString() + "@";
                    PackageStr += gridView2.GetRowCellValue(i, "Package").ToString() + "@";
                    NumStr += ConvertType.ToInt32(gridView2.GetRowCellValue(i, "Num")) + "@";
                    FeeWeightStr += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeWeight")) + "@";
                    FeeVolumeStr += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeVolume")) + "@";

                    WeightStr += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "Weight")) + "@";
                    VolumeStr += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "Volume")) + "@";
                    WeightPriceStr += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "WeightPrice")) + "@";
                    VolumePriceStr += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "VolumePrice")) + "@";
                    FeeTypeStr += gridView2.GetRowCellValue(i, "FeeType").ToString() + "@";
                    FreightStr += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "Freight")) + "@";
                    //合计到第一行
                    Num += ConvertType.ToInt32(gridView2.GetRowCellValue(i, "Num"));
                    FeeWeight += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeWeight"));
                    FeeVolume += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeVolume"));
                    Weight += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "Weight"));
                    Volume += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "Volume"));
                    Freight += ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "Freight"));
                }
            }
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillId", DraftGUID.Text.Trim()));
            list.Add(new SqlPara("BillNo", BillNo.Text.Trim()));
            list.Add(new SqlPara("VehicleNo", VehicleNo.Text.Trim()));
            list.Add(new SqlPara("BillDate", System.DateTime.Now));
            list.Add(new SqlPara("BillState", 0));
            list.Add(new SqlPara("StartSite", StartSite.Text.Trim()));
            list.Add(new SqlPara("TransferMode", TransferMode.Text.Trim()));
            list.Add(new SqlPara("DestinationSite", DestinationSite.Text.Trim()));
            list.Add(new SqlPara("TransferSite", TransferSite.Text.Trim()));

            list.Add(new SqlPara("TransitMode", TransitMode.Text.Trim()));
            list.Add(new SqlPara("CusOderNo", CusOderNo.Text.Trim()));

            list.Add(new SqlPara("ConsigneeCellPhone", NoticeState.Checked ? "888888" : ConsigneeCellPhone.Text.Trim()));
            list.Add(new SqlPara("ConsigneePhone", NoticeState.Checked ? "888888" : ConsigneePhone.Text.Trim()));
            list.Add(new SqlPara("ConsigneeName", NoticeState.Checked ? "888888" : ConsigneeName.Text.Trim()));
            list.Add(new SqlPara("ConsigneeCompany", NoticeState.Checked ? "888888" : ConsigneeCompany.Text.Trim()));
            list.Add(new SqlPara("PickGoodsSite", PickGoodsSite.Text.Trim()));

            list.Add(new SqlPara("ReceivProvince", ReceivProvince1));
            list.Add(new SqlPara("ReceivCity", ReceivCity1));
            list.Add(new SqlPara("ReceivArea", ReceivArea1));
            list.Add(new SqlPara("ReceivStreet", ReceivStreet1));

            if (CommonClass.UserInfo.companyid != "239")
            {
                list.Add(new SqlPara("ReceivAddress", Province.Text.Trim() + City.Text.Trim() + Address.Text.Trim()));
            }
            else
            {
                list.Add(new SqlPara("ReceivAddress", Address.Text.Trim()));
            }

            list.Add(new SqlPara("ConsignorCellPhone", ConsignorCellPhone.Text.Trim()));
            list.Add(new SqlPara("ConsignorPhone", ConsignorPhone.Text.Trim()));
            list.Add(new SqlPara("ConsignorName", ConsignorName.Text.Trim()));
            list.Add(new SqlPara("ConsignorCompany", ConsignorCompany.Text.Trim()));
            //list.Add(new SqlPara("ReceivMode", ReceivOrderNo1.Text.Trim()));


            list.Add(new SqlPara("ValuationType", ValuationType.Text.Trim()));
            list.Add(new SqlPara("ReceivOrderNo", ReceivOrderNo.Text.Trim()));
            list.Add(new SqlPara("Salesman", Salesman.Text.Trim()));
            list.Add(new SqlPara("AlienGoods", AlienGoods.Checked ? 1 : 0));
            list.Add(new SqlPara("GoodsVoucher", GoodsVoucher.Checked ? 1 : 0));
            list.Add(new SqlPara("PreciousGoods", PreciousGoods.Checked ? 1 : 0));
            list.Add(new SqlPara("NoticeState", NoticeState.Checked ? 1 : 0));

            list.Add(new SqlPara("GoodsType", GoodsType));
            list.Add(new SqlPara("Varieties", Varieties));
            list.Add(new SqlPara("Package", Package));
            list.Add(new SqlPara("Num", Num));
            list.Add(new SqlPara("LeftNum", Num));
            list.Add(new SqlPara("FeeWeight", FeeWeight));
            list.Add(new SqlPara("FeeVolume", FeeVolume));
            list.Add(new SqlPara("Weight", Weight));
            list.Add(new SqlPara("Volume", Volume));

            list.Add(new SqlPara("WeightPrice", WeightPrice));
            list.Add(new SqlPara("VolumePrice", VolumePrice));
            list.Add(new SqlPara("FeeType", FeeType));
            list.Add(new SqlPara("Freight", Freight));
            list.Add(new SqlPara("DeliFee", DeliFee));

            list.Add(new SqlPara("ReceivFee", ReceivFee));
            list.Add(new SqlPara("DeclareValue", DeclareValue));
            list.Add(new SqlPara("SupportValue", SupportValue));
            list.Add(new SqlPara("Rate", 0));
            list.Add(new SqlPara("Tax", Tax));

            list.Add(new SqlPara("InformationFee", InformationFee));
            list.Add(new SqlPara("Expense", Expense));
            list.Add(new SqlPara("NoticeFee", NoticeFee));
            list.Add(new SqlPara("DiscountTransfer", DiscountTransfer));
            list.Add(new SqlPara("CollectionPay", CollectionPay));

            list.Add(new SqlPara("AgentFee", AgentFee));
            list.Add(new SqlPara("FuelFee", FuelFee));
            list.Add(new SqlPara("UpstairFee", UpstairFee));
            list.Add(new SqlPara("HandleFee", HandleFee));
            list.Add(new SqlPara("ForkliftFee", ForkliftFee));

            list.Add(new SqlPara("StorageFee", StorageFee));
            list.Add(new SqlPara("CustomsFee", CustomsFee));
            list.Add(new SqlPara("packagFee", packagFee));
            list.Add(new SqlPara("FrameFee", FrameFee));
            list.Add(new SqlPara("ChangeFee", ChangeFee));
            list.Add(new SqlPara("MatPay", MatPay));//HJ20180911

            list.Add(new SqlPara("OtherFee", OtherFee));
            list.Add(new SqlPara("IsInvoice", IsInvoice.Checked ? 1 : 0));
            list.Add(new SqlPara("ReceiptFee", ReceiptFee));
            list.Add(new SqlPara("ReceiptCondition", ReceiptCondition.Text.Trim()));
            list.Add(new SqlPara("FreightAmount", 0));
            list.Add(new SqlPara("ActualFreight", ActualFreight));


            list.Add(new SqlPara("PaymentMode", PaymentMode.Text.Trim()));
            list.Add(new SqlPara("PaymentAmout", PaymentAmout.Text.Trim() == "" ? "0" : PaymentAmout.Text.Trim()));

            list.Add(new SqlPara("NowPay", NowPay.Text.Trim() == "" ? "0" : NowPay.Text.Trim()));
            list.Add(new SqlPara("FetchPay", FetchPay.Text.Trim() == "" ? "0" : FetchPay.Text.Trim()));
            list.Add(new SqlPara("MonthPay", MonthPay.Text.Trim() == "" ? "0" : MonthPay.Text.Trim()));
            list.Add(new SqlPara("ShortOwePay", ShortOwePay.Text.Trim() == "" ? "0" : ShortOwePay.Text.Trim()));
            list.Add(new SqlPara("BefArrivalPay", BefArrivalPay.Text.Trim() == "" ? "0" : BefArrivalPay.Text.Trim()));
            list.Add(new SqlPara("BillRemark", BillRemark.Text.Trim()));
            list.Add(new SqlPara("WebPlatform", WebPlatform.Text.Trim()));
            list.Add(new SqlPara("WebBillNo", WebBillNo.Text.Trim()));
            list.Add(new SqlPara("DisTranName", DisTranName.Text.Trim()));
            list.Add(new SqlPara("DisTranBank", DisTranBank.Text.Trim()));
            list.Add(new SqlPara("DisTranAccount", DisTranAccount.Text.Trim()));
            list.Add(new SqlPara("CollectionName", CollectionName.Text.Trim()));
            list.Add(new SqlPara("CollectionBank", CollectionBank.Text.Trim()));
            list.Add(new SqlPara("CollectionAccount", CollectionAccount.Text.Trim()));
            list.Add(new SqlPara("CauseName", CauseName.Text));
            list.Add(new SqlPara("AreaName", AreaName.Text));
            list.Add(new SqlPara("DepName", DepName.Text));
            list.Add(new SqlPara("OtherTotalFee", "0"));
            list.Add(new SqlPara("BillTotalFee", "0"));
            list.Add(new SqlPara("DisTranBranch", DisTranBranch.Text.Trim()));
            list.Add(new SqlPara("CollectionBranch", CollectionBranch.Text.Trim()));
            list.Add(new SqlPara("BillUserId", CommonClass.UserInfo.UserName));
            list.Add(new SqlPara("BillMan", BillMan.Text));
            list.Add(new SqlPara("DeviceSource", "0"));
            list.Add(new SqlPara("begWeb", begWeb.Text));
            list.Add(new SqlPara("isModify", isModify));

            list.Add(new SqlPara("WarehouseFee", WarehouseFee));

            list.Add(new SqlPara("VarietiesStr", VarietiesStr));
            list.Add(new SqlPara("PackageStr", PackageStr));
            list.Add(new SqlPara("NumStr", NumStr));
            list.Add(new SqlPara("FeeWeightStr", FeeWeightStr));
            list.Add(new SqlPara("FeeVolumeStr", FeeVolumeStr));

            list.Add(new SqlPara("WeightStr", WeightStr));
            list.Add(new SqlPara("VolumeStr", VolumeStr));
            list.Add(new SqlPara("WeightPriceStr", WeightPriceStr));
            list.Add(new SqlPara("VolumePriceStr", VolumePriceStr));
            list.Add(new SqlPara("FeeTypeStr", FeeTypeStr));
            list.Add(new SqlPara("FreightStr", FreightStr));

            list.Add(new SqlPara("IsReceiptFee", IsReceiptFee.Checked ? 1 : 0));
            list.Add(new SqlPara("IsSupportValue", IsSupportValue.Checked ? 1 : 0));
            list.Add(new SqlPara("IsAgentFee", IsAgentFee.Checked ? 1 : 0));
            list.Add(new SqlPara("IsPackagFee", IsPackagFee.Checked ? 1 : 0));
            list.Add(new SqlPara("IsOtherFee", IsOtherFee.Checked ? 1 : 0));

            list.Add(new SqlPara("IsHandleFee", IsHandleFee.Checked ? 1 : 0));
            list.Add(new SqlPara("IsStorageFee", IsStorageFee.Checked ? 1 : 0));
            list.Add(new SqlPara("IsWarehouseFee", IsWarehouseFee.Checked ? 1 : 0));
            list.Add(new SqlPara("IsForkliftFee", IsForkliftFee.Checked ? 1 : 0));
            list.Add(new SqlPara("IsChangeFee", IsChangeFee.Checked ? 1 : 0));

            list.Add(new SqlPara("IsUpstairFee", IsUpstairFee.Checked ? 1 : 0));
            list.Add(new SqlPara("IsCustomsFee", IsCustomsFee.Checked ? 1 : 0));
            list.Add(new SqlPara("IsFrameFee", IsFrameFee.Checked ? 1 : 0));
            list.Add(new SqlPara("IsMatPay", IsMatPay.Checked ? 1 : 0));//HJ20180911
            list.Add(new SqlPara("IsRebates", IsRebates.Checked ? 1 : 0));//HJ20180911


            list.Add(new SqlPara("MainlineFee", ConvertType.ToDecimal(MainlineFee.Text.Trim())));
            list.Add(new SqlPara("DeliveryFee", ConvertType.ToDecimal(DeliveryFee.Text.Trim())));
            list.Add(new SqlPara("TransferFee", ConvertType.ToDecimal(TransferFee.Text.Trim())));
            list.Add(new SqlPara("DepartureOptFee", ConvertType.ToDecimal(DepartureOptFee.Text.Trim())));
            list.Add(new SqlPara("TerminalOptFee", ConvertType.ToDecimal(TerminalOptFee.Text.Trim())));
            list.Add(new SqlPara("TerminalAllotFee", ConvertType.ToDecimal(TerminalAllotFee.Text.Trim())));


            list.Add(new SqlPara("ReceiptFee_C", ConvertType.ToDecimal(ReceiptFee_C.Text.Trim())));
            list.Add(new SqlPara("NoticeFee_C", ConvertType.ToDecimal(NoticeFee_C.Text.Trim())));

            list.Add(new SqlPara("SupportValue_C", ConvertType.ToDecimal(SupportValue_C.Text.Trim())));
            list.Add(new SqlPara("AgentFee_C", ConvertType.ToDecimal(AgentFee_C.Text.Trim())));
            list.Add(new SqlPara("PackagFee_C", ConvertType.ToDecimal(PackagFee_C.Text.Trim())));
            list.Add(new SqlPara("OtherFee_C", ConvertType.ToDecimal(OtherFee_C.Text.Trim())));
            list.Add(new SqlPara("HandleFee_C", ConvertType.ToDecimal(HandleFee_C.Text.Trim())));

            list.Add(new SqlPara("StorageFee_C", ConvertType.ToDecimal(StorageFee_C.Text.Trim())));
            list.Add(new SqlPara("WarehouseFee_C", ConvertType.ToDecimal(WarehouseFee_C.Text.Trim())));
            list.Add(new SqlPara("ForkliftFee_C", ConvertType.ToDecimal(ForkliftFee_C.Text.Trim())));
            list.Add(new SqlPara("Tax_C", ConvertType.ToDecimal(Tax_C.Text.Trim())));
            list.Add(new SqlPara("ChangeFee_C", ConvertType.ToDecimal(ChangeFee_C.Text.Trim())));

            list.Add(new SqlPara("UpstairFee_C", ConvertType.ToDecimal(UpstairFee_C.Text.Trim())));
            list.Add(new SqlPara("CustomsFee_C", ConvertType.ToDecimal(CustomsFee_C.Text.Trim())));
            list.Add(new SqlPara("FrameFee_C", ConvertType.ToDecimal(FrameFee_C.Text.Trim())));
            list.Add(new SqlPara("Expense_C", ConvertType.ToDecimal(Expense_C.Text.Trim())));
            list.Add(new SqlPara("FuelFee_C", ConvertType.ToDecimal(FuelFee_C.Text.Trim())));
            list.Add(new SqlPara("InformationFee_C", ConvertType.ToDecimal(InformationFee_C.Text.Trim())));

            list.Add(new SqlPara("ConsigneeCellPhone_K", ConsigneeCellPhone.Text.Trim()));
            list.Add(new SqlPara("ConsigneePhone_K", ConsigneePhone.Text.Trim()));
            list.Add(new SqlPara("ConsigneeName_K", ConsigneeName.Text.Trim()));
            list.Add(new SqlPara("ConsigneeCompany_K", ConsigneeCompany.Text.Trim()));

            list.Add(new SqlPara("OperationWeight", ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "OperationWeight"))));
            list.Add(new SqlPara("OptWeight", ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "OptWeight"))));

            SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_WAYBILL_Draft", list);
            if (SqlHelper.ExecteNonQuery(sps) > 0)
            {
                MsgBox.ShowOK();
                clear();
            }
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (panel5.Visible)
            {
                panel5.Visible = false;
            }
            else
            {
                //List<SqlPara> list = new List<SqlPara>();
                //list.Add(new SqlPara("BillMan", CommonClass.UserInfo.UserName));
                //SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_Draft", list);
                //DataSet ds = SqlHelper.GetDataSet(sps);
                //gridControl5.DataSource = ds.Tables[0];
                panel5.Left = 373;
                panel5.Top = 0;
                panel5.Visible = true;
                panel5.BringToFront();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            panel5.Visible = false;
        }

        private void gridControl5_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GetWayBill_Draft();
        }

        private void GetWayBill_Draft()
        {
            try
            {
                int rows = gridView7.FocusedRowHandle;
                if (rows < 0) return;
                string BillId = gridView7.GetRowCellValue(rows, "BillId").ToString();
                panel5.Visible = false;
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillId", BillId));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_Draft_ID", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                dsModified = ds.Copy();
                //isModify = 1;
                dr = ds.Tables[0].Rows[0];

                ConsignorCompany.EditValueChanging -= new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsignorCompany_EditValueChanging);
                ConsignorName.EditValueChanging -= new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsignorCompany_EditValueChanging);

                ConsigneeCompany.EditValueChanging -= new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsigneeCompany_EditValueChanging);
                ConsigneeName.EditValueChanging -= new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsigneeCompany_EditValueChanging);


                gridView1.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView2_CellValueChanged);
                gridView2.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView2_CellValueChanged);

                ConsigneeCellPhone.EditValue = ConvertType.ToInt32(dr["NoticeState"]) == 0 ? dr["ConsigneeCellPhone"] : dr["ConsigneeCellPhone_K"];
                ConsigneePhone.EditValue = ConvertType.ToInt32(dr["NoticeState"]) == 0 ? dr["ConsigneePhone"] : dr["ConsigneePhone_K"];
                ConsigneeName.EditValue = ConvertType.ToInt32(dr["NoticeState"]) == 0 ? dr["ConsigneeName"] : dr["ConsigneeName_K"];
                if (CommonClass.UserInfo.companyid == "239")
                {
                    Address.EditValue = ConvertType.ToInt32(dr["NoticeState"]) == 0 ? dr["ConsigneeCompany"] : dr["ConsigneeCompany_K"];
                }


                PickGoodsSite.EditValue = dr["PickGoodsSite"];
                //ReceivProvince.EditValue = dr["ReceivProvince"];
                // ReceivCity.EditValue = dr["ReceivCity"];
                //ReceivArea.EditValue = dr["ReceivArea"];
                //ReceivStreet.EditValue = dr["ReceivStreet"];
                Province.EditValue = dr["ReceivProvince"];
                City.EditValue = dr["ReceivCity"];
                Address.EditValue = dr["ReceivAddress"];
                ConsignorCellPhone.EditValue = dr["ConsignorCellPhone"];
                ConsignorPhone.EditValue = dr["ConsignorPhone"];
                ConsignorName.EditValue = dr["ConsignorName"];
                ConsignorCompany.EditValue = dr["ConsignorCompany"];
                //ReceivOrderNo1.EditValue = dr["ReceivMode"];


                ValuationType.EditValue = dr["ValuationType"];
                ReceivOrderNo.EditValue = dr["ReceivOrderNo"];
                Salesman.EditValue = dr["Salesman"];


                BillNo.EditValue = dr["BillNo"];
                VehicleNo.EditValue = dr["VehicleNo"];
                StartSite.EditValue = dr["StartSite"];
                TransferMode.EditValue = dr["TransferMode"];
                DestinationSite.EditValue = dr["DestinationSite"];
                TransferSite.EditValue = dr["TransferSite"];

                TransitMode.EditValue = dr["TransitMode"];
                CusOderNo.EditValue = dr["CusOderNo"];

                AlienGoods.Checked = Convert.ToInt32(dr["AlienGoods"].ToString() == "" ? "0" : dr["AlienGoods"].ToString()) > 0;
                GoodsVoucher.Checked = Convert.ToInt32(dr["GoodsVoucher"].ToString() == "" ? "0" : dr["GoodsVoucher"].ToString()) > 0;
                PreciousGoods.Checked = Convert.ToInt32(dr["PreciousGoods"].ToString() == "" ? "0" : dr["PreciousGoods"].ToString()) > 0; //luohui
                NoticeState.Checked = Convert.ToInt32(dr["NoticeState"].ToString() == "" ? "0" : dr["NoticeState"].ToString()) > 0;

                gridView2.SetRowCellValue(0, "GoodsType", dr["GoodsType"]);
                gridView2.SetRowCellValue(0, "Varieties", dr["Varieties"]);
                gridView2.SetRowCellValue(0, "Package", dr["Package"]);
                gridView2.SetRowCellValue(0, "Num", dr["Num"]);

                gridView2.SetRowCellValue(0, "FeeWeight", dr["FeeWeight"]);
                gridView2.SetRowCellValue(0, "FeeVolume", dr["FeeVolume"]);
                gridView2.SetRowCellValue(0, "Weight", dr["Weight"]);
                gridView2.SetRowCellValue(0, "Volume", dr["Volume"]);
                gridView2.SetRowCellValue(0, "WeightPrice", dr["WeightPrice"]);

                gridView2.SetRowCellValue(0, "VolumePrice", dr["VolumePrice"]);
                gridView2.SetRowCellValue(0, "FeeType", dr["FeeType"]);
                gridView2.SetRowCellValue(0, "Freight", dr["Freight"]);

                gridView1.SetRowCellValue(0, "DeliFee", dr["DeliFee"]);
                gridView1.SetRowCellValue(0, "ReceivFee", dr["ReceivFee"]);
                gridView1.SetRowCellValue(0, "DeclareValue", dr["DeclareValue"]);
                gridView1.SetRowCellValue(0, "SupportValue", dr["SupportValue"]);
                gridView1.SetRowCellValue(0, "Rate", dr["Rate"]);

                gridView1.SetRowCellValue(0, "Tax", dr["Tax"]);
                gridView1.SetRowCellValue(0, "InformationFee", dr["InformationFee"]);
                gridView1.SetRowCellValue(0, "Expense", dr["Expense"]);
                gridView1.SetRowCellValue(0, "NoticeFee", dr["NoticeFee"]);
                gridView1.SetRowCellValue(0, "DiscountTransfer", dr["DiscountTransfer"]);

                gridView1.SetRowCellValue(0, "CollectionPay", dr["CollectionPay"]);
                gridView1.SetRowCellValue(0, "AgentFee", dr["AgentFee"]);
                gridView1.SetRowCellValue(0, "FuelFee", dr["FuelFee"]);

                gridView1.SetRowCellValue(0, "UpstairFee", dr["UpstairFee"]);
                gridView1.SetRowCellValue(0, "HandleFee", dr["HandleFee"]);
                gridView1.SetRowCellValue(0, "ForkliftFee", dr["ForkliftFee"]);
                gridView1.SetRowCellValue(0, "StorageFee", dr["StorageFee"]);
                gridView1.SetRowCellValue(0, "CustomsFee", dr["CustomsFee"]);
                gridView1.SetRowCellValue(0, "MatPay", dr["MatPay"]);//HJ20180911

                gridView1.SetRowCellValue(0, "packagFee", dr["packagFee"]);
                gridView1.SetRowCellValue(0, "FrameFee", dr["FrameFee"]);
                gridView1.SetRowCellValue(0, "ChangeFee", dr["ChangeFee"]);
                gridView1.SetRowCellValue(0, "OtherFee", dr["OtherFee"]);
                gridView1.SetRowCellValue(0, "ReceiptFee", dr["ReceiptFee"]);

                gridView1.SetRowCellValue(0, "FreightAmount", dr["FreightAmount"]);
                gridView1.SetRowCellValue(0, "ActualFreight", dr["ActualFreight"]);
                gridView1.SetRowCellValue(0, "WarehouseFee", dr["WarehouseFee"]);


                ReceiptCondition.EditValue = dr["ReceiptCondition"];
                IsInvoice.Checked = Convert.ToInt32(dr["IsInvoice"].ToString() == "" ? "0" : dr["IsInvoice"].ToString()) > 0;


                PaymentMode.EditValue = dr["PaymentMode"];
                PaymentAmout.EditValue = dr["PaymentAmout"];

                NowPay.EditValue = dr["NowPay"];
                FetchPay.EditValue = dr["FetchPay"];
                MonthPay.EditValue = dr["MonthPay"];
                ShortOwePay.EditValue = dr["ShortOwePay"];
                BefArrivalPay.EditValue = dr["BefArrivalPay"];
                BillRemark.EditValue = dr["BillRemark"];
                WebPlatform.EditValue = dr["WebPlatform"];
                WebBillNo.EditValue = dr["WebBillNo"];
                DisTranName.EditValue = dr["DisTranName"];
                DisTranBank.EditValue = dr["DisTranBank"];
                DisTranAccount.EditValue = dr["DisTranAccount"];
                CollectionName.EditValue = dr["CollectionName"];
                CollectionBank.EditValue = dr["CollectionBank"];
                CollectionAccount.EditValue = dr["CollectionAccount"];
                CauseName.EditValue = dr["CauseName"];
                AreaName.EditValue = dr["AreaName"];
                DepName.EditValue = dr["DepName"];

                DisTranBranch.EditValue = dr["DisTranBranch"];
                CollectionBranch.EditValue = dr["CollectionBranch"];
                BillMan.EditValue = dr["BillMan"];
                begWeb.EditValue = dr["begWeb"];





                IsReceiptFee.Checked = ConvertType.ToInt32(dr["IsReceiptFee"]) > 0;
                IsSupportValue.Checked = ConvertType.ToInt32(dr["IsSupportValue"]) > 0;
                IsAgentFee.Checked = ConvertType.ToInt32(dr["IsAgentFee"]) > 0;
                IsPackagFee.Checked = ConvertType.ToInt32(dr["IsPackagFee"]) > 0;
                IsOtherFee.Checked = ConvertType.ToInt32(dr["IsOtherFee"]) > 0;
                IsHandleFee.Checked = ConvertType.ToInt32(dr["IsHandleFee"]) > 0;

                IsStorageFee.Checked = ConvertType.ToInt32(dr["IsStorageFee"]) > 0;
                IsWarehouseFee.Checked = ConvertType.ToInt32(dr["IsWarehouseFee"]) > 0;
                IsForkliftFee.Checked = ConvertType.ToInt32(dr["IsForkliftFee"]) > 0;
                IsChangeFee.Checked = ConvertType.ToInt32(dr["IsChangeFee"]) > 0;
                IsUpstairFee.Checked = ConvertType.ToInt32(dr["IsUpstairFee"]) > 0;
                IsCustomsFee.Checked = ConvertType.ToInt32(dr["IsCustomsFee"]) > 0;
                IsFrameFee.Checked = ConvertType.ToInt32(dr["IsFrameFee"]) > 0;
                IsMatPay.Checked = ConvertType.ToInt32(dr["IsMatPay"]) > 0;//HJ20180911
                IsRebates.Checked = ConvertType.ToInt32(dr["IsRebates"]) > 0;//HJ20180911

                DraftGUID.Text = dr["BillId"].ToString();

                if (ds.Tables[1].Rows.Count > 0)
                {
                    int NumFirst = ConvertType.ToInt32(dr["Num"]);
                    decimal FeeWeightFirst = ConvertType.ToDecimal(dr["FeeWeight"]);
                    decimal FeeVolumeFirst = ConvertType.ToDecimal(dr["FeeVolume"]);
                    decimal WeightFirst = ConvertType.ToDecimal(dr["Weight"]);
                    decimal VolumeFirst = ConvertType.ToDecimal(dr["Volume"]);
                    decimal FreightFirst = ConvertType.ToDecimal(dr["Freight"]);

                    int NumSecond = 0;
                    decimal FeeWeightSecond = 0, FeeVolumeSecond = 0, WeightSecond = 0, VolumeSecond = 0, FreightSecond = 0;

                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        gridView2.SetRowCellValue(i + 1, "Varieties", ds.Tables[1].Rows[i]["Varieties"].ToString());
                        gridView2.SetRowCellValue(i + 1, "Package", ds.Tables[1].Rows[i]["Package"].ToString());
                        gridView2.SetRowCellValue(i + 1, "Num", ds.Tables[1].Rows[i]["Num"].ToString());
                        gridView2.SetRowCellValue(i + 1, "FeeWeight", ds.Tables[1].Rows[i]["FeeWeight"].ToString());
                        gridView2.SetRowCellValue(i + 1, "FeeVolume", ds.Tables[1].Rows[i]["FeeVolume"].ToString());

                        gridView2.SetRowCellValue(i + 1, "Weight", ds.Tables[1].Rows[i]["Weight"].ToString());
                        gridView2.SetRowCellValue(i + 1, "Volume", ds.Tables[1].Rows[i]["Volume"].ToString());
                        gridView2.SetRowCellValue(i + 1, "WeightPrice", ds.Tables[1].Rows[i]["WeightPrice"].ToString());
                        gridView2.SetRowCellValue(i + 1, "VolumePrice", ds.Tables[1].Rows[i]["VolumePrice"].ToString());
                        gridView2.SetRowCellValue(i + 1, "FeeType", ds.Tables[1].Rows[i]["FeeType"].ToString());
                        gridView2.SetRowCellValue(i + 1, "Freight", ds.Tables[1].Rows[i]["Freight"].ToString());


                        NumSecond += ConvertType.ToInt32(ds.Tables[1].Rows[i]["Num"]);
                        FeeWeightSecond += ConvertType.ToDecimal(ds.Tables[1].Rows[i]["FeeWeight"]);
                        FeeVolumeSecond += ConvertType.ToDecimal(ds.Tables[1].Rows[i]["FeeVolume"]);
                        WeightSecond += ConvertType.ToDecimal(ds.Tables[1].Rows[i]["Weight"]);
                        VolumeSecond += ConvertType.ToDecimal(ds.Tables[1].Rows[i]["Volume"]);
                        FreightSecond += ConvertType.ToDecimal(ds.Tables[1].Rows[i]["Freight"]);
                    }

                    gridView2.SetRowCellValue(0, "Num", NumFirst - NumSecond);
                    gridView2.SetRowCellValue(0, "FeeWeight", FeeWeightFirst - FeeWeightSecond);
                    gridView2.SetRowCellValue(0, "FeeVolume", FeeVolumeFirst - FeeVolumeSecond);
                    gridView2.SetRowCellValue(0, "Weight", WeightFirst - WeightSecond);
                    gridView2.SetRowCellValue(0, "Volume", VolumeFirst - VolumeSecond);
                    gridView2.SetRowCellValue(0, "Freight", FreightFirst - FreightSecond);
                }

                //gridView8.SetRowCellValue(0, "MainlineFee", dr["MainlineFee"]);
                gridView1.SetRowCellValue(0, "DeliveryFee", dr["DeliveryFee"]);
                //gridView8.SetRowCellValue(0, "TransferFee", dr["TransferFee"]);
                //gridView8.SetRowCellValue(0, "DepartureOptFee", dr["DepartureOptFee"]);
                //gridView8.SetRowCellValue(0, "TerminalOptFee", dr["TerminalOptFee"]);
                //gridView8.SetRowCellValue(0, "TerminalAllotFee", dr["TerminalAllotFee"]);

                //gridView8.SetRowCellValue(0, "ReceiptFee_C", dr["ReceiptFee_C"]);
                //gridView8.SetRowCellValue(0, "NoticeFee_C", dr["NoticeFee_C"]);
                //gridView8.SetRowCellValue(0, "SupportValue_C", dr["SupportValue_C"]);

                //gridView8.SetRowCellValue(0, "AgentFee_C", dr["AgentFee_C"]);
                //gridView8.SetRowCellValue(0, "PackagFee_C", dr["PackagFee_C"]);
                //gridView8.SetRowCellValue(0, "OtherFee_C", dr["OtherFee_C"]);

                //gridView8.SetRowCellValue(0, "HandleFee_C", dr["HandleFee_C"]);
                //gridView8.SetRowCellValue(0, "StorageFee_C", dr["StorageFee_C"]);
                //gridView8.SetRowCellValue(0, "WarehouseFee_C", dr["WarehouseFee_C"]);

                //gridView8.SetRowCellValue(0, "ForkliftFee_C", dr["ForkliftFee_C"]);
                //gridView8.SetRowCellValue(0, "Tax_C", dr["Tax_C"]);
                //gridView8.SetRowCellValue(0, "ChangeFee_C", dr["ChangeFee_C"]);

                //gridView8.SetRowCellValue(0, "UpstairFee_C", dr["UpstairFee_C"]);
                //gridView8.SetRowCellValue(0, "CustomsFee_C", dr["CustomsFee_C"]);
                //gridView8.SetRowCellValue(0, "FrameFee_C", dr["FrameFee_C"]);

                //gridView8.SetRowCellValue(0, "Expense_C", dr["Expense_C"]);
                //gridView8.SetRowCellValue(0, "FuelFee_C", dr["FuelFee_C"]);
                //gridView8.SetRowCellValue(0, "InformationFee_C", dr["InformationFee_C"]);

                MainlineFee.Text = dr["MainlineFee"].ToString();
                TransferFee.Text = dr["TransferFee"].ToString();
                DepartureOptFee.Text = dr["DepartureOptFee"].ToString();
                TerminalOptFee.Text = dr["TerminalOptFee"].ToString();
                TerminalAllotFee.Text = dr["TerminalAllotFee"].ToString();
                ReceiptFee_C.Text = dr["ReceiptFee_C"].ToString();
                NoticeFee_C.Text = dr["NoticeFee_C"].ToString();
                SupportValue_C.Text = dr["SupportValue_C"].ToString();
                AgentFee_C.Text = dr["AgentFee_C"].ToString();
                PackagFee_C.Text = dr["PackagFee_C"].ToString();
                OtherFee_C.Text = dr["OtherFee_C"].ToString();
                HandleFee_C.Text = dr["HandleFee_C"].ToString();
                StorageFee_C.Text = dr["StorageFee_C"].ToString();
                WarehouseFee_C.Text = dr["WarehouseFee_C"].ToString();
                ForkliftFee_C.Text = dr["ForkliftFee_C"].ToString();
                Tax_C.Text = dr["Tax_C"].ToString();
                ChangeFee_C.Text = dr["ChangeFee_C"].ToString();
                UpstairFee_C.Text = dr["UpstairFee_C"].ToString();
                CustomsFee_C.Text = dr["CustomsFee_C"].ToString();
                FrameFee_C.Text = dr["FrameFee_C"].ToString();
                Expense_C.Text = dr["Expense_C"].ToString();
                FuelFee_C.Text = dr["FuelFee_C"].ToString();
                InformationFee_C.Text = dr["InformationFee_C"].ToString();

                gridView1.SetRowCellValue(0, "OperationWeight", dr["OperationWeight"]);
                gridView1.SetRowCellValue(0, "OptWeight", dr["OptWeight"]);

                //获取尺寸 hj20180925
                string billno = BillNo.Text;
                string Varieties1 = gridView2.GetRowCellValue(0, "Varieties").ToString();
                string Package1 = gridView2.GetRowCellValue(0, "Package").ToString();
                string Varieties2 = gridView2.GetRowCellValue(1, "Varieties").ToString();
                string Package2 = gridView2.GetRowCellValue(1, "Package").ToString();


                List<SqlPara> list2 = new List<SqlPara>();
                //if (Varieties2 != "" || Varieties2 != null)
                //{
                //    list2.Add(new SqlPara("Varieties2", Varieties2));
                //    list2.Add(new SqlPara("Package2", Package2));
                //}
                list2.Add(new SqlPara("BillNo", billno));
                //list2.Add(new SqlPara("Varieties1", Varieties1));
                //list2.Add(new SqlPara("Package1", Package1));
                SqlParasEntity sps2 = new SqlParasEntity(OperType.Query, "USP_GET_SpecialGoodsSize", list2);
                DataSet ds2 = SqlHelper.GetDataSet(sps2);
                if (ds2.Tables[0].Rows.Count == 1)
                {
                    string variet = ds2.Tables[0].Rows[0]["Varieties"].ToString();
                    string pack = ds2.Tables[0].Rows[0]["Package"].ToString();
                    decimal len = Convert.ToDecimal(ds2.Tables[0].Rows[0]["Length"]);
                    decimal wid = Convert.ToDecimal(ds2.Tables[0].Rows[0]["Width"]);
                    decimal hei = Convert.ToDecimal(ds2.Tables[0].Rows[0]["Height"]);
                    if (Varieties1 == variet && Package1 == pack)
                    {
                        gridView2.SetRowCellValue(0, "length", len);
                        gridView2.SetRowCellValue(0, "width", wid);
                        gridView2.SetRowCellValue(0, "height", hei);

                        //第二行为0设空
                        gridView2.SetRowCellValue(1, "length", 0);
                        gridView2.SetRowCellValue(1, "width", 0);
                        gridView2.SetRowCellValue(1, "height", 0);
                    }
                    else
                    {
                        gridView2.SetRowCellValue(1, "length", len);
                        gridView2.SetRowCellValue(1, "width", wid);
                        gridView2.SetRowCellValue(1, "height", hei);
                    }
                }
                else if (ds2.Tables[0].Rows.Count == 2)
                {
                    string variet = ds2.Tables[0].Rows[0]["Varieties"].ToString();
                    string pack = ds2.Tables[0].Rows[0]["Package"].ToString();
                    decimal len = Convert.ToDecimal(ds2.Tables[0].Rows[0]["Length"]);
                    decimal wid = Convert.ToDecimal(ds2.Tables[0].Rows[0]["Width"]);
                    decimal hei = Convert.ToDecimal(ds2.Tables[0].Rows[0]["Height"]);
                    decimal len2 = Convert.ToDecimal(ds2.Tables[0].Rows[1]["Length"]);
                    decimal wid2 = Convert.ToDecimal(ds2.Tables[0].Rows[1]["Width"]);
                    decimal hei2 = Convert.ToDecimal(ds2.Tables[0].Rows[1]["Height"]);
                    if (Varieties1 == variet && Package1 == pack)
                    {
                        gridView2.SetRowCellValue(0, "length", len);
                        gridView2.SetRowCellValue(0, "width", wid);
                        gridView2.SetRowCellValue(0, "height", hei);
                        gridView2.SetRowCellValue(1, "length", len2);
                        gridView2.SetRowCellValue(1, "width", wid2);
                        gridView2.SetRowCellValue(1, "height", hei2);
                    }
                    else
                    {
                        gridView2.SetRowCellValue(0, "length", len2);
                        gridView2.SetRowCellValue(0, "width", wid2);
                        gridView2.SetRowCellValue(0, "height", hei2);
                        gridView2.SetRowCellValue(1, "length", len);
                        gridView2.SetRowCellValue(1, "width", wid);
                        gridView2.SetRowCellValue(1, "height", hei);
                    }

                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                ConsignorCompany.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsignorCompany_EditValueChanging);
                ConsignorName.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsignorCompany_EditValueChanging);

                ConsigneeCompany.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsigneeCompany_EditValueChanging);
                ConsigneeName.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsigneeCompany_EditValueChanging);

                gridView1.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView2_CellValueChanged);
                gridView2.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView2_CellValueChanged);

            }
        }

        private void TransferMode_Enter(object sender, EventArgs e)
        {
            TransferMode.ShowPopup();
            //Province.Focus();
            TransitMode.Focus();
        }

        private void TransitMode_Enter(object sender, EventArgs e)
        {
            TransitMode.ShowPopup();
            PickGoodsSite.Focus();
        }

        private void PickGoodsSite_EditValueChanged(object sender, EventArgs e)
        {
            TransitMode.Text = "";
        }

        private void CusOderNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //gridControl2.Focus();
                //gridView2.FocusedRowHandle = 0;
                //gridView2.FocusedColumn = gridView2.Columns["Varieties"];
                //gridView2.ShowEditor();
            }
        }

        //private void gcdaozhan_Leave(object sender, EventArgs e)
        //{
        //    if (ReceivStreet.Text.Trim() != "")
        //    {
        //        DataTable table = (DataTable)gcdaozhan.DataSource;
        //        DataRow[] dr = table.Select("MiddleStreet='" + ReceivStreet.Text.Trim() + "'");
        //        if (dr.Length == 0)
        //        {
        //            ReceivStreet.Focus();
        //            Tool.ToolTip.ShowTip("请选择正确的街道", ReceivStreet, ToolTipLocation.RightTop);
        //            return;
        //        }
        //    }
        //    gcdaozhan.Visible = ReceivStreet.Focused;
        //}

        private void panel6_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {


        }

        private void IsReceiptFee_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                gridControl1.Focus();
                gridView1.FocusedRowHandle = 0;
                gridView1.FocusedColumn = gridColumn52;
                gridView1.ShowEditor();
            }
        }

        private void ReceiptCondition_Enter(object sender, EventArgs e)
        {
            ReceiptCondition.ShowPopup();
        }

        private void WebPlatform_Enter(object sender, EventArgs e)
        {
            WebPlatform.ShowPopup();
        }

        //private void gridControl3_Leave(object sender, EventArgs e)
        //{
        //    if (ReceivProvince.Text.Trim() != "")
        //    {
        //        DataTable table = (DataTable)gridControl3.DataSource;
        //        DataRow[] dr = table.Select("MiddleProvince='" + ReceivProvince.Text.Trim() + "'");
        //        if (dr.Length == 0)
        //        {
        //            ReceivProvince.Focus();
        //            Tool.ToolTip.ShowTip("请选择正确省份", ReceivProvince, ToolTipLocation.RightTop);
        //            return;
        //        }
        //    }
        //    gridControl3.Visible = ReceivProvince.Focused;
        //}

        //private void gridControl9_Leave(object sender, EventArgs e)
        //{
        //    if (ReceivCity.Text.Trim() != "")
        //    {
        //        DataTable table = (DataTable)gridControl9.DataSource;
        //        DataRow[] dr = table.Select("MiddleCity='" + ReceivCity.Text.Trim() + "'");
        //        if (dr.Length == 0)
        //        {
        //            ReceivCity.Focus();
        //            Tool.ToolTip.ShowTip("输入错误", ReceivCity, ToolTipLocation.RightTop);
        //            return;
        //        }
        //    }
        //    gridControl9.Visible = ReceivCity.Focused;
        //}

        //private void gridControl10_Leave(object sender, EventArgs e)
        //{
        //    if (ReceivArea.Text.Trim() != "")
        //    {
        //        DataTable table = (DataTable)gridControl10.DataSource;
        //        DataRow[] dr = table.Select("MiddleArea='" + ReceivArea.Text.Trim() + "'");
        //        if (dr.Length == 0)
        //        {
        //            ReceivArea.Focus();
        //            Tool.ToolTip.ShowTip("输入错误", ReceivArea, ToolTipLocation.RightTop);
        //            return;
        //        }
        //    }
        //    gridControl10.Visible = ReceivArea.Focused;
        //}

        //private void ReceivAddress_Enter(object sender, EventArgs e)
        //{
        //    ReceivAddress.Select(ReceivAddress.Text.Length, 0);
        //}


        private void IsCenterBack_CheckedChanged(object sender, EventArgs e)
        {
            //if (IsCenterBack.Checked)
            //{
            //    CollectionName.Enabled = true;
            //    CollectionBank.Enabled = true;
            //    CollectionBranch.Enabled = true;
            //    CollectionAccount.Enabled = true;
            //}
            //else
            //{
            //    CollectionName.Enabled = false;
            //    CollectionBank.Enabled = false;
            //    CollectionBranch.Enabled = false;
            //    CollectionAccount.Enabled = false;

            //    CollectionName.Text = "";
            //    CollectionBank.Text = "";
            //    CollectionBranch.Text = "";
            //    CollectionAccount.Text = "";
            //}
        }

        private void PickGoodsSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            //SetFee();
            //SetFeeNew();//zaj 2018-4-15新结算
        }



        private void frmWayBillAdd_JMGX_HD_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.F3)
            //{
            //    fmDeliveryFee fm = new fmDeliveryFee();
            //    fm.ShowDialog();
            //}
            //else if (e.KeyCode == Keys.F4)
            //{
            //    fmMainlineFee fm = new fmMainlineFee();
            //    fm.ShowDialog();
            //}
            //else if (e.KeyCode == Keys.F5)
            //{
            //    fmTransferFee fm = new fmTransferFee();
            //    fm.ShowDialog();
            //}
            //else if (e.KeyCode == Keys.F6)
            //{
            //    fmSurchargeFee fm = new fmSurchargeFee();
            //    fm.ShowDialog();
            //}
            if (e.KeyCode == Keys.F2) //hj20180720查看二级中转市县
            {
                fmMiddleSite fm = new fmMiddleSite();
                fm.ShowDialog();
            }
        }

        //private void ReceivMode_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ReceivOrderNo1.Text.Trim() != "客户自送")
        //    {
        //        ReceivOrderNo.Enabled = true;
        //    }
        //    else
        //    {
        //        ReceivOrderNo.Enabled = false;
        //        ReceivOrderNo.Text = "";
        //    }
        //}

        //private void ReceivMode_Enter(object sender, EventArgs e)
        //{
        //    ReceivOrderNo1.ShowPopup();
        //}

        private void ReceivOrderNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            { gcjiehuodanhao.Focus(); }
            if (e.KeyCode == Keys.Escape)
            { gcjiehuodanhao.Visible = false; }
        }

        private void gcjiehuodanhao_Leave(object sender, EventArgs e)
        {
            gcjiehuodanhao.Visible = ReceivOrderNo.Focused;
        }


        private void BillNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = StringHelper.NumChange(e.KeyChar);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string web = CgbegWeb.Text.Trim() == "全部" ? "%%" : CgbegWeb.Text.Trim();
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillMan", web == "%%" ? "%%" : CommonClass.UserInfo.UserName));
            list.Add(new SqlPara("BegWeb", web));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_Draft_Ex", list);
            DataSet ds = SqlHelper.GetDataSet(sps);
            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count == 0)
                return;
            gridControl5.DataSource = ds.Tables[0];
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            int rows = gridView7.FocusedRowHandle;
            if (rows < 0) return;
            string billno = gridView7.GetRowCellValue(rows, "BillNo").ToString(); ;
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTLABEL_DEV_Draft", new List<SqlPara> { new SqlPara("BillNo", billno) }));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                MsgBox.ShowOK("没有找到要打印的运单,请稍后再试");
                return;
            }
            frmPrintLabelDev fpld = new frmPrintLabelDev(ds.Tables[0]);
            fpld.rpt = rpt;
            fpld.ShowDialog();
        }

        private void TransferMode_Click(object sender, EventArgs e)
        {
            TransferMode.ShowPopup();
        }

        //关闭时，调用 UpdateBillNoState 方法
        private void frmWayBillAdd_JMGX_HD_FormClosing(object sender, FormClosingEventArgs e)
        {
            ////关闭后还原票据明细表的运单号状态
            //if (isModify == 0 && this.BillNo.Enabled != true)
            //{
            //    UpdateBillNoState();
            //}
        }


        private void TransferMode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                PickGoodsSite.Focus();
            }
        }

        //private void ReceivMode_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (e.KeyChar == (char)Keys.Enter)
        //    {
        //        TransitMode.Focus();
        //    }
        //}

        private void TransitMode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                CusOderNo.Focus();
            }
        }

        private void AlienGoods_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void MatPay_CheckedChanged(object sender, EventArgs e)
        {
            if (IsMatPay.Checked)
            {
                gridView1.Columns["MatPay"].Visible = true;
                gridView1.Columns["MatPay"].OptionsColumn.AllowEdit = true;
                gridView1.Columns["MatPay"].OptionsColumn.AllowFocus = true;
            }
            else
            {
                gridView1.Columns["MatPay"].Visible = false;
                gridView1.Columns["MatPay"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["MatPay"].OptionsColumn.AllowFocus = false;
                gridView1.SetRowCellValue(0, "MatPay", "0");
            }
        }

        private void MatPay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                gridControl1.Focus();
                gridView1.FocusedRowHandle = 0;
                gridView1.FocusedColumn = gridColumn52;
                gridView1.ShowEditor();
            }
        }

        //HJ20180912
        private void DestinationSite_EditValueChanging(object sender, ChangingEventArgs e)
        {
            if (e.NewValue != null)
            {
                myGridView1.ClearColumnsFilter();
                //myGridView1.Columns["DestinationSite"].FilterInfo = new ColumnFilterInfo("[DestinationSite] LIKE " + "'%" + e.NewValue.ToString() + "%'" + " OR [DestinationSite] LIKE" + "'%" + e.NewValue.ToString() + "%'", "");
                //myGridView1.Columns["MiddleProvince"].FilterInfo = new ColumnFilterInfo(
                //"[MiddleProvince] LIKE " + "'%" + e.NewValue.ToString() + "%'"
                //+ " OR [MiddleCity] LIKE" + "'%" + e.NewValue.ToString() + "%'"
                //+ "OR [MiddleArea] LIKE" + "'%" + e.NewValue.ToString() + "%'"
                //+ " OR [MiddleStreet] LIKE " + "'%" + e.NewValue.ToString() + "%'",
                //"");
                //LMS-2597
                myGridView1.Columns["MiddleProvince"].FilterInfo = new ColumnFilterInfo(
                  "[MiddleProvince] LIKE " + "'%" + e.NewValue.ToString() + "%'"
                        + " OR [MiddleCity] LIKE" + "'%" + e.NewValue.ToString() + "%'"
                        + " OR [MiddleArea] LIKE" + "'%" + e.NewValue.ToString() + "%'",
                        "");
            }
            else
            {
                myGridView1.ClearColumnsFilter();
            }
        }

        private void DestinationSite_Enter(object sender, EventArgs e)
        {
            //if (DestinationSite.Text.Trim() == "")
            //{
            //    myGridView1.ClearColumnsFilter();
            //}
            //if (myGridView1.RowCount == 0) return;
            myGridControl1.Left = DestinationSite.Left + 248;
            myGridControl1.Top = DestinationSite.Top + DestinationSite.Height + 66;
            myGridControl1.Visible = true;
            myGridControl1.BringToFront();
        }

        private void DestinationSite_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                myGridControl1.Focus();
            }
            if (e.KeyCode == Keys.Escape)
            {
                myGridControl1.Visible = false;
                //TransferSite.Focus();
            }

        }

        private void DestinationSite_Leave(object sender, EventArgs e)
        {
            DataTable table = (DataTable)myGridControl1.DataSource;
            if (DestinationSite.Text.Trim() != "" && !myGridControl1.Focused)
            {

                DataRow[] dr = table.Select("MiddleProvince+MiddleCity+MiddleArea='" + DestinationSite.Text.Trim() + "'");
                if (dr.Length == 0)
                {
                    DestinationSite.Focus();
                    Tool.ToolTip.ShowTip("请选择正确的地址", DestinationSite, ToolTipLocation.RightTop);
                    return;
                }
            }

            if (!myGridControl1.Focused)
            {
                myGridControl1.Visible = false;
            }
        }


        private void myGridControl1_DoubleClick(object sender, EventArgs e)
        {
            SetMiddleSite();
        }

        private void myGridControl1_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Escape)
            //{
            //    myGridControl1.Visible = false;
            //}
            //if (e.KeyCode == Keys.Enter)
            //{
            //    SetMiddleSite();
            //}
            if (e.KeyCode == Keys.Enter)
            {
                SetMiddleSite();
            }
        }

        private void SetMiddleSite()
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            DataRow dr = myGridView1.GetDataRow(rowhandle);
            ReceivProvince1 = dr["MiddleProvince"].ToString();
            ReceivCity1 = dr["MiddleCity"].ToString();
            ReceivArea1 = dr["MiddleArea"].ToString();
            //ReceivStreet1 = dr["MiddleStreet"].ToString();
            ReceivStreet1 = "";
            try
            {
                if (CommonClass.UserInfo.companyid != "239")    //ZJF20181117
                {
                    if (ReceivArea1 == "全部")
                    {
                        DestinationSite.EditValue = ReceivProvince1 + ReceivCity1;
                        Address.EditValue = ReceivProvince1 + ReceivCity1;
                        ReceivStreet.EditValue = ReceivStreet1;
                    }
                    else if (ReceivStreet1 == "全部")
                    {
                        Address.EditValue = ReceivProvince1 + ReceivCity1 + ReceivArea1;
                        ReceivStreet.EditValue = ReceivStreet1;
                    }
                    else
                    {
                        DestinationSite.EditValue = ReceivProvince1 + ReceivCity1 + ReceivArea1;
                        Address.EditValue = ReceivProvince1 + ReceivCity1 + ReceivArea1 + ReceivStreet1;
                        ReceivStreet.EditValue = ReceivStreet1;
                    }
                }
                else
                {
                    if (ReceivArea1 == "全部")
                    {
                        DestinationSite.EditValue = ReceivProvince1 + ReceivCity1;
                        //Address.EditValue = ReceivProvince1 + ReceivCity1;
                        ReceivStreet.EditValue = ReceivStreet1;
                    }
                    else if (ReceivStreet1 == "全部")
                    {
                        //Address.EditValue = ReceivProvince1 + ReceivCity1 + ReceivArea1;
                        ReceivStreet.EditValue = ReceivStreet1;
                    }
                    else
                    {
                        DestinationSite.EditValue = ReceivProvince1 + ReceivCity1 + ReceivArea1;
                        //Address.EditValue = ReceivProvince1 + ReceivCity1 + ReceivArea1 + ReceivStreet1;
                        ReceivStreet.EditValue = ReceivStreet1;
                    }
                }
                if (dr["SiteName"].ToString() == "")
                {
                    TransferSite.EditValue = CommonClass.UserInfo.SiteName;
                }
                else
                {
                    TransferSite.EditValue = dr["SiteName"];
                }
                if (CommonClass.UserInfo.companyid == "485")
                {
                    PickGoodsSite.Text = "";
                }
                else
                {
                    PickGoodsSite.EditValue = dr["WebName"];
                }
                Province.EditValue = dr["MiddleProvince"];
                City.EditValue = dr["MiddleCity"];

            }
            catch { }
            myGridControl1.Visible = false;
            CusOderNo.Focus();
            //myGridControl2.DataSource 
            DataRow[] dr1 = CommonClass.dsMiddleSite.Tables[0].Select("MiddleProvince='" +
            ReceivProvince1 + "'and MiddleCity='" + ReceivCity1 + "'and MiddleArea='" + ReceivArea1 + "'");//2019.5.24路由替换

            DataTable dt1 = CommonClass.dsMiddleSite.Tables[0].Clone();
            dt1.Clear();
            foreach (DataRow dr3 in dr1)
            {
                dt1.Rows.Add(dr3.ItemArray);
            }
            myGridControl2.DataSource = dt1;
            ReceivStreet.EditValue = "";
        }

        private void TransferSite_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Escape)
            //{
            //    myGridControl1.Visible = false;
            //}
            if (e.KeyCode == Keys.Enter)
            {
                CusOderNo.Focus();
            }

        }

        //private void PaymentMode_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //}

        private void VehicleNum_Enter(object sender, EventArgs e)
        {
            //if (isModify != 1 && Upd_Num != 1)
            //{
            gcjiehuodanhao.Left = VehicleNum.Left + 80;
            gcjiehuodanhao.Top = VehicleNum.Top + VehicleNum.Height + 183;
            gcjiehuodanhao.Visible = true;
            gcjiehuodanhao.BringToFront();
            //}
        }

        private void VehicleNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            { gcjiehuodanhao.Focus(); }
            if (e.KeyCode == Keys.Escape)
            { gcjiehuodanhao.Visible = false; }
            if (e.KeyCode == Keys.Back)
            {
                VehicleNum.Text = "";
                DriverName.Text = "";
                ReceivOrderNo.Text = "";
            }
        }

        private void VehicleNum_Leave(object sender, EventArgs e)
        {
            if (!gcjiehuodanhao.Focused)
            {
                gcjiehuodanhao.Visible = false;
            }
        }

        private void Province_EditValueChanged(object sender, EventArgs e)
        {
            SetCity();
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DestinationSite.Text = "";
            TransferSite.Text = "";
            PickGoodsSite.Text = "";
            if (radioGroup1.SelectedIndex == 1)
            {
                DestinationSite.Text = CommonClass.UserInfo.SiteName;
                TransferSite.Text = CommonClass.UserInfo.SiteName;
                PickGoodsSite.Text = CommonClass.UserInfo.WebName;

            }
        }

        private void TransferMode_EditValueChanged(object sender, EventArgs e)
        {
            //if (TransferMode.Text.Trim() == "送货")
            //{
            //    CommonClass.dsMiddleSite.Tables[0]
            //}
        }

        private void myGridControl1_Leave(object sender, EventArgs e)
        {
            myGridControl1.Visible = DestinationSite.Focused;
        }

        private void StartSite_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                DestinationSite.Focus();
            }
        }

        private void TransferMode_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                PickGoodsSite.Focus();
            }
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //GridOper.SaveGridLayout(gridView2, gridView2.Name);
            GridOper.SaveGridLayout(gridView2, "货差货损记录2");
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(gridView2, gridView2.Name);
        }

        private void Address_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                TransitMode.Focus();
            }
        }

        private void Address_Leave(object sender, EventArgs e)
        {
            TransitMode.Focus();
        }

        private void TransferMode_Leave(object sender, EventArgs e)
        {
            PickGoodsSite.Focus();
        }

        private void PaymentAmout_TextChanged(object sender, EventArgs e)
        {
            setheji();
        }

        private void TransferMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetFeeNew();
        }

        private void ReceivStreet_Leave(object sender, EventArgs e)
        {
            if (!myGridControl2.Focused)
            {
                myGridControl2.Visible = false;
            }
        }

        private void ReceivStreet_EditValueChanging(object sender, ChangingEventArgs e)
        {

            //myGridView2.ClearColumnsFilter();
            //myGridView2.Columns["MiddleStreet"].FilterInfo = new ColumnFilterInfo(
            //"[MiddleStreet] LIKE " + "'%" + e.NewValue.ToString() + "%'", "");
        }

        private void ReceivStreet_Enter(object sender, EventArgs e)
        {
            myGridControl2.Left = DestinationSite.Left + 635;
            myGridControl2.Top = DestinationSite.Top + DestinationSite.Height + 66;
            myGridControl2.Visible = true;
            myGridControl2.BringToFront();
        }

        private void ReceivStreet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                myGridControl2.Focus();
            }
            if (e.KeyCode == Keys.Escape)
            {
                myGridControl2.Visible = false;
                //TransferSite.Focus();
            }
        }

        private void myGridControl2_Leave(object sender, EventArgs e)
        {
            myGridControl2.Visible = ReceivStreet.Focused;
        }

        private void ReceivStreet_DoubleClick(object sender, EventArgs e)
        {
            SetStreet();
        }

        private void myGridControl2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SetStreet();
            }
        }

        public void SetStreet()
        {

            int rowhandle = myGridView2.FocusedRowHandle;
            if (rowhandle < 0) return;
            DataRow dr = myGridView2.GetDataRow(rowhandle);
            ReceivProvince1 = dr["MiddleProvince"].ToString();
            ReceivCity1 = dr["MiddleCity"].ToString();
            ReceivArea1 = dr["MiddleArea"].ToString();
            ReceivStreet1 = dr["MiddleStreet"].ToString();
            ReceivStreet.EditValue = ReceivStreet1;
            Address.EditValue = ReceivProvince1 + ReceivCity1 + ReceivArea1 + ReceivStreet1;
            if (CommonClass.UserInfo.companyid == "485")
            {
                PickGoodsSite.EditValue = dr["WebName"];
                PickGoodsSite.Properties.ReadOnly = true;
            }
            myGridControl2.Visible = false;
            TransferSite.Focus();

        }

        private void myGridControl2_DoubleClick(object sender, EventArgs e)
        {
            SetStreet();
        }

        //自动弹出  ZJF20181123
        private void TransferMode_Enter_1(object sender, EventArgs e)
        {
            TransferMode.ShowPopup();
        }

        private void TransitMode_Enter_1(object sender, EventArgs e)
        {
            TransitMode.ShowPopup();
        }

        private void ReceiptCondition_Enter_1(object sender, EventArgs e)
        {
            ReceiptCondition.ShowPopup();
        }

        private void PickGoodsSite_Enter(object sender, EventArgs e)
        {
            PickGoodsSite.ShowPopup();
        }

        //luohui 商超
        private void MarketSuper_CheckedChanged(object sender, EventArgs e)
        {
            if (MarketSuper.Checked)
            {

                gridView1.Columns["StorageFee"].Visible = true;
                gridView1.Columns["StorageFee"].OptionsColumn.AllowEdit = true;
                gridView1.Columns["StorageFee"].OptionsColumn.AllowFocus = true;
            }
            else
            {
                gridView1.Columns["StorageFee"].Visible = false;
                gridView1.Columns["StorageFee"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["StorageFee"].OptionsColumn.AllowFocus = false;
                gridView1.SetRowCellValue(0, "StorageFee", "0");
            }
            SetFeeNew();
        }

        //分拣luohui
        private void IsSortFee_CheckedChanged(object sender, EventArgs e)
        {
            SetFeeToHandleFee();
        }
        //必走货 luohui
        private void PreciousGoods_CheckedChanged(object sender, EventArgs e)
        {
            SetFeeNew();
        }

        //视图跟踪费 luohui
        private void IsViewTraceFee_CheckedChanged(object sender, EventArgs e)
        {
            SetFeeToViewTraceFee();
        }
        private void AlienGoods_CheckedChanged_1(object sender, EventArgs e)
        {
            SetFeeNew();
        }
        private void TransitMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TransitMode.Text.Trim().ToString() == "川胜项目" || TransitMode.Text.Trim().ToString() == "川胜整车")
            {
                this.StartSite.AllowDrop = true;
                this.StartSite.Enabled = true;
            }

            SetFeeNew();
        }

        private void DestinationSite_EditValueChanged(object sender, EventArgs e)
        {
            if (gsDenominatedFee == "是")
            {
                SetDenominatedFee();
            }
        }

        private void ConsignorCompany_EditValueChanged(object sender, EventArgs e)
        {
            if (gsDenominatedFee == "是")
            {
                SetDenominatedFee();
            }
            ConsignorName.Text = ConsignorCompany.Text;
        }

        private void ReceiptCondition_EditValueChanged(object sender, EventArgs e)
        {
            if (ReceiptCondition.Text.Trim() != "")
            {
                IsReceiptFee.Checked = true;
                checkEdit3.Enabled = true;
            }
            else
            {
                IsReceiptFee.Checked = false;
                if (CommonClass.UserInfo.companyid != "167")
                {
                    checkEdit3.Enabled = false;
                    checkEdit3.Checked = false;
                }
            }
        }
    }
}