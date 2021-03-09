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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.Utils;

namespace ZQTMS.UI
{
    public partial class BillDeliveryDetail : BaseForm
    {
        public BillDeliveryDetail()
        {
            InitializeComponent();
        }

        public string isCancel = "";
        public string OperateType = "";//当前操作类型,New(新增),update(修改),EditRemark(修改备注),OnlyView(只查看),dispatch(派车),Relation(关联)
        public Guid DeliveryID = Guid.Empty;
        private string DeliState = "";//派车状态
        public string DeliVehTypeValue = "";//接受选择的派车类型，ReceiveGoods=接货；SendGoods=送货；ShortBarge=短驳；LongDistance=长途
        string SettleStateValue = "";
        public string vehFareVerifyState = "";  //核销状态，已核销的不能关联运单

        private string lonAndlat = "";  //经纬度
        private string loadLonLat = "";    //派车显示的经纬度

        private void BillDeliveryDetail_Load(object sender, EventArgs e)
        {
            SetDeliCusName(DeliCusName);        //zhengjiafeng20181023
            if (!CommonClass.FormSet(this, false, true)) return;
            CommonClass.GetGridViewColumns(myGridView1, myGridView2);
            GridOper.SetGridViewProperty(myGridView1, myGridView2);
            BarMagagerOper.SetBarPropertity(bar1); //如果有具体的工具条，就引用其实例
            DataSet msDs = CommonClass.dsMiddleSite;
            PreToTime.DateTime = CommonClass.gcdate;
            DispatchTime.DateTime = CommonClass.gcdate;
            if (msDs != null && msDs.Tables.Count > 0)
            {
                gridControl9.DataSource = msDs.Tables[0];
            }
            else
            {
                MsgBox.ShowOK("正在加载基础资料，请稍等！");
                this.Close();
                return;
            }
            //if (OperateType == "dispatch")
            //{
                DataSet carDs = CommonClass.dsCar;
                if (carDs == null)
                {
                    MsgBox.ShowOK("正在加载基础资料，请稍等！");
                    this.Close();
                    return;
                }
                myGridControl2.DataSource = CommonClass.dsCar.Tables[0];
            //}
            gridColumn4.VisibleIndex = 10;

            //3PL可以随便填写 到站、承运部门
            if (CommonClass.UserInfo.UserDB == UserDB.ZQTMS3PL)
                ArriveSite.Properties.TextEditStyle = BearDepart.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;

            AddSelectData();
            if (OperateType.Equals("New"))
            //if (true)
            {
                this.Text = "派车申请";
                string DeliVehType = "";
                if (!string.IsNullOrEmpty(DeliVehTypeValue.Trim()))
                {
                    if (DeliVehTypeValue.Equals("ReceiveGoods"))
                    {
                        radioButton1.Checked = true;
                        DeliVehType = radioButton1.Text;
                        BearDepart.Properties.ReadOnly = true;

                    }
                    else if (DeliVehTypeValue.Equals("SendGoods"))
                    {
                        radioButton2.Checked = true;
                        DeliVehType = radioButton2.Text;
                    }
                    else if (DeliVehTypeValue.Equals("ShortBarge"))
                    {
                        radioButton3.Checked = true;
                        DeliVehType = radioButton3.Text;
                        //BearDepart.Properties.ReadOnly = true;
                    }
                    else
                    {
                        radioButton4.Checked = true;
                        DeliVehType = radioButton4.Text;
                        BearDepart.Properties.ReadOnly = true;
                    }
                }
               
                DeliTime.Text = CommonClass.gcdate.ToString("yyyy-MM-dd");
                PickGoodsTime.Text = CommonClass.gcdate.ToString("yyyy-MM-dd HH:mm");
                DeliRegMan.Text = CommonClass.UserInfo.UserName;
                DeliCode.Text = getNo(CommonClass.UserInfo.SiteName, DeliVehType);
                SettleStateValue = "未付款";
                SetControlDisable(false, true, false);
                AcceptDepart.Properties.ReadOnly = true;
                this.Text = "派车申请";
                //2018.3.15wbw
                VehiclesDemand.Text = "正常用车";
                BearDepart.Text = CommonClass.UserInfo.WebName;

            }
            else
            {
                #region 非新增
                try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("DeliId", DeliveryID));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDELIVERY_ByID", list);
                    DataSet ds = SqlHelper.GetDataSet(sps);

                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                    DataRow dr = ds.Tables[0].Rows[0];
                    DeliCode.EditValue = dr["DeliCode"];
                    DeliTime.EditValue = dr["DeliTime"];
                    if (dr["DeliVehType"].ToString().Equals("接货")  )
                    {
                        radioButton1.Checked = true;
                        //BearDepart.Enabled = false;
                        DeliVehTypeValue = "ReceiveGoods";
                        if (isCancel == "作废")
                        {
                            barButtonItem2.Enabled = false;
                            barButtonItem3.Enabled = false;
                            uctLab.Visible = true;
                            panelControl1.Enabled = false;
                            uctLab.BringToFront();

                        }
                    }
                    else if (dr["DeliVehType"].ToString().Equals("送货") || dr["DeliVehType"].ToString().Equals("二次送货"))
                    {
                        radioButton2.Checked = true;
                        DeliVehTypeValue = "SendGoods";
                        if (isCancel == "作废")
                        {
                            barButtonItem2.Enabled = false;
                            barButtonItem3.Enabled = false;
                            uctLab.Visible = true;
                            panelControl1.Enabled = false;
                            uctLab.BringToFront();
                        }
                    }
                    else if (dr["DeliVehType"].ToString().Equals("短驳"))
                    {
                        radioButton3.Checked = true;
                        //BearDepart.Enabled = false;
                        DeliVehTypeValue = "ShortBarge";
                        if (isCancel == "作废")
                        {
                            barButtonItem2.Enabled = false;
                            barButtonItem3.Enabled = false;

                            panelControl1.Enabled = false;
                            uctLab.Visible = true;
                            uctLab.BringToFront();
                        }
                    }
                    else
                    {
                        radioButton4.Checked = true;
                        //BearDepart.Enabled = false;
                        DeliVehTypeValue = "LongDistance";
                        if (isCancel == "作废")
                        {
                            barButtonItem2.Enabled = false;
                            barButtonItem3.Enabled = false;
                            uctLab.Visible = true;
                            panelControl1.Enabled = false;
                            uctLab.BringToFront();
                        }
                    }
                    ControlWeb.Enabled = false;
                    VehiclesDemand.Enabled = false;
                    DeliCusName.EditValue = dr["DeliCusName"];
                    DeliCusPhone.EditValue = dr["DeliCusPhone"];
                    CarWeb.EditValue = dr["CarWeb"];
                    GoodsName.EditValue = dr["GoodsName"];
                    LoadPlace.EditValue = dr["LoadPlace"];
                    ArriveSite.EditValue = dr["ArriveSite"];
                    Num.EditValue = dr["Num"];
                    Weight.EditValue = dr["Weight"];
                    Volume.EditValue = dr["Volume"];
                    Freight.EditValue = dr["Freight"];
                    Payment.EditValue = dr["Payment"];
                    CollectFee.EditValue = dr["CollectFee"];
                    PickGoodsTime.EditValue = dr["PickGoodsTime"];
                    BearDepart.EditValue = dr["BearDepart"];
                    DocuAttached.EditValue = dr["DocuAttached"];
                    DeliRemark.EditValue = dr["DeliRemark"];
                    DeliRegMan.EditValue = dr["DeliRegMan"];
                    UserCarDepart.EditValue = dr["UserCarDepart"];
                    VehicleLength.EditValue = dr["VehicleLength"];
                    VehicleType.EditValue = dr["VehicleType"];
                    DepartContactMan.EditValue = dr["DepartContactMan"];
                    DepartTel.EditValue = dr["DepartTel"];
                    DeliState = dr["DeliState"].ToString();
                    VehicleNum.EditValue = dr["VehicleNum"];
                    DriverName.EditValue = dr["DriverName"];
                    DriverPhone.EditValue = dr["DriverPhone"];
                    DispatchTime.EditValue = dr["DispatchTime"];
                    PreToTime.EditValue = dr["PreToTime"];
                    VehFare.EditValue = dr["VehFare"];
                    ////jl 送到网点，推送App 2018-11-09
                    //ckbPushApp.Checked = Convert.ToInt32(dr["IsSendWbps"].ToString()) == 1 ? true : false;
                    string acceptDepart = ConvertType.ToString(dr["AcceptDepart"]);
                    if (!string.IsNullOrEmpty(acceptDepart))
                    {
                        AcceptDepart.EditValue = acceptDepart;
                    }
                    SettleState.EditValue = dr["SettleState"];
                    SettleStateValue = dr["SettleState"].ToString();
                    IsRent.EditValue = dr["IsRent"];
                    ControlRemark.EditValue = dr["ControlRemark"];
                    AcceptMan.EditValue = dr["AcceptMan"];
                    ControlWeb.EditValue = dr["ControlWeb"];
                    VehiclesDemand.EditValue = dr["VehiclesDemand"];
                    lonAndlat = ConvertType.ToString(dr["LonAndLat"]);
                    loadLonLat = ConvertType.ToString(dr["LonAndLat"]);
                    vehFareVerifyState = ConvertType.ToString(dr["VehFareVerifyState"]);
                    VehicleWidth.EditValue = dr["VehicleWidth"];
                    VehicleHeight.EditValue = dr["VehicleHeight"];
                    if (ConvertType.ToString(dr["smsToDriver"]) == "已发")
                    {
                        //checkBox1.Checked = true;
                    }
                    if (ConvertType.ToString(dr["smsToCustom"]) == "已发")
                    {
                        //checkBox2.Checked = true;
                    }
                    if (OperateType.Equals("OnlyView"))
                    {
                        SetControlDisable(true, true, true);
                        AcceptDepart.Properties.ReadOnly = true;
                        button1.Enabled = false;
                        button2.Enabled = false;
                        BillNo.Enabled = false;
                        ReceivFee.Enabled = false;
                        barButtonItem2.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                        barButtonItem3.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                        this.Text = "派车单";
                        if (isCancel == "作废")
                        {
                            checkBox1.Enabled = false;
                            checkBox2.Enabled = false;
                            uctLab.Visible = true;
                            panelControl1.Enabled = false;
                            uctLab.BringToFront();
                        }
                    }
                    //else if (OperateType.Equals("dispatch"))
                    //{
                    //    AcceptMan.Text = CommonClass.UserInfo.UserName;
                    //    SetControlDisable(true, false, true);
                    //    button1.Enabled = false;
                    //    button2.Enabled = false;
                    //    this.Text = "派车单";
                    //    AcceptMan.Properties.ReadOnly = true;
                    //    //DispatchTime.Properties.ReadOnly = false;
                    //    //PreToTime.Properties.ReadOnly = false;
                    //}
                    else if (OperateType.Equals("EditRemark"))
                    {
                        SetControlDisable(true, true, false);
                        AcceptDepart.Properties.ReadOnly = true;
                        button1.Enabled = false;
                        button2.Enabled = false;
                        DeliRemark.Enabled = true;
                        this.Text = "派车申请";
                        DeliRemark.Properties.ReadOnly = false;
                    }
                    else if (OperateType.Equals("update"))
                    {
                        SetControlDisable(false, true, false);
                        AcceptDepart.Properties.ReadOnly = true;
                        button1.Enabled = false;
                        button2.Enabled = false;
                        this.Text = "派车申请";
                    }
                    else
                    {
                        AcceptMan.Text = CommonClass.UserInfo.UserName;
                        //SetControlDisable(true, false, true);
                        //button1.Enabled = false;
                        //button2.Enabled = false;
                        //this.Text = "派车单";
                        //AcceptMan.Properties.ReadOnly = true;

                        this.Text = "派车单";
                        SetControlDisable(true, true, true);
                        //AcceptDepart.Properties.ReadOnly = true;
                        //barButtonItem3.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                        //barButtonItem2.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    }
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
                #endregion 非新增
            }
            if (!radioButton1.Checked)
            {
                VehiclesDemand.Enabled = false;
                VehiclesDemand.Text = "正常用车";
            }
            DeliCode.Focus();
        }

        /// <summary>
        /// 加载下拉选项
        /// </summary>
        void AddSelectData()
        {
            string[] IsRentList = CommonClass.Arg.IsRent.Split(',');
            if (IsRentList.Length > 0)
            {
                for (int i = 0; i < IsRentList.Length; i++)
                {
                    IsRent.Properties.Items.Add(IsRentList[i]);
                }
                IsRent.SelectedIndex = 0;
            }

            string[] AcceptDepartList = CommonClass.Arg.AcceptDepart.Split(',');
            if (AcceptDepartList.Length > 0)
            {
                for (int i = 0; i < AcceptDepartList.Length; i++)
                {
                    AcceptDepart.Properties.Items.Add(AcceptDepartList[i]);
                }
                AcceptDepart.SelectedIndex = 0;

                DataSet accdepDs = CommonClass.dsWeb;
                if (accdepDs != null && accdepDs.Tables.Count > 0)
                {
                    DataRow[] rows = accdepDs.Tables[0].Select("WebName='" + CommonClass.UserInfo.WebName + "'");
                    if (rows != null && rows.Length > 0)
                    {
                        string isSendVehicle = ConvertType.ToString(rows[0]["IsSendVehicle"]);
                        if (isSendVehicle != "是")
                        {
                            AcceptDepart.Text = AcceptDepartList[0];
                            AcceptDepart.Properties.ReadOnly = true;
                        }
                    }
                }
            }

            string[] SettleStateList = CommonClass.Arg.SettleState.Split(',');
            if (SettleStateList.Length > 0)
            {
                for (int i = 0; i < SettleStateList.Length; i++)
                {
                    SettleState.Properties.Items.Add(SettleStateList[i]);
                }
                SettleState.SelectedIndex = 0;
            }

            string[] VehicleTypeList = CommonClass.Arg.VehicleType.Split(',');
            if (VehicleTypeList.Length > 0)
            {
                for (int i = 0; i < VehicleTypeList.Length; i++)
                {
                    VehicleType.Properties.Items.Add(VehicleTypeList[i]);
                }
                //VehicleType.SelectedIndex = 0;
            }

            string[] VehicleLengthList = CommonClass.Arg.VehicleLength.Split(',');
            if (VehicleLengthList.Length > 0)
            {
                for (int i = 0; i < VehicleLengthList.Length; i++)
                {
                    VehicleLength.Properties.Items.Add(VehicleLengthList[i]);
                }
                //VehicleLength.SelectedIndex = 0;
            }

            string[] PaymentList = CommonClass.Arg.PaymentMode.Split(',');
            if (PaymentList.Length > 0)
            {
                for (int i = 0; i < PaymentList.Length; i++)
                {
                    Payment.Properties.Items.Add(PaymentList[i]);
                }
                Payment.SelectedIndex = 0;
            }
            CommonClass.SetSite(CarWeb, false);
            CarWeb.Text = CommonClass.UserInfo.SiteName;
            CommonClass.SetSite(ArriveSite, false);
        }

        /// <summary>
        /// 获得随机编号
        /// </summary>
        /// <param name="CarWeb">站点</param>
        /// <param name="DeliVehType">派车类型</param>
        /// <returns></returns>
        string getNo(string CarWeb, string DeliVehType)
        {
            string rltNo = "";
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bsite", CarWeb));
                list.Add(new SqlPara("DeliVehType", DeliVehType));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_billDelivery_DeliCode", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    rltNo = "";
                }
                else
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    rltNo = dr.ItemArray[0].ToString();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError("产生单号失败！" + ex.Message);
                DeliCode.Text = "";
            }
            return rltNo;
        }

        private DataSet ds = null;
        /// <summary>
        /// 加载关联运单列表
        /// </summary>
        void AddOrderList()
        {
            if (!string.IsNullOrEmpty(DeliCode.Text.Trim()))
            {
                List<SqlPara> lst = new List<SqlPara>();
                lst.Add(new SqlPara("DeliCode", DeliCode.Text));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDELICONWAYBILL_DeliCode", lst);
                if (ds != null)
                {
                    ds = null;
                }
                ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0)
                {
                    return;
                }
                else
                {
                    myGridControl1.DataSource = ds.Tables[0];
                }
            }
        }

        /// <summary>
        /// 控件是否可编辑
        /// </summary>
        /// <param name="Enabled">申请模块是否可编辑</param>
        /// <param name="DriverPanelEnabled">司机信息模块是否可编辑</param>
        void SetControlDisable(bool ApplyPanelEnabled, bool DriverPanelEnabled, bool PanelVisible)
        {
            //申请模块
            BearDepart.Properties.ReadOnly = ApplyPanelEnabled;
            //DeliCode.Enabled = ApplyPanelEnabled;
            if (ApplyPanelEnabled)
            {
                radioButton1.Enabled = false;
                radioButton2.Enabled = false;
                radioButton3.Enabled = false;
                radioButton4.Enabled = false;
            }
            else
            {
                radioButton1.Enabled = true;
                radioButton2.Enabled = true;
                radioButton3.Enabled = true;
                radioButton4.Enabled = true;
            }

            DeliTime.Properties.ReadOnly = ApplyPanelEnabled;
            DeliCusName.Properties.ReadOnly = ApplyPanelEnabled;
            DeliCusPhone.Properties.ReadOnly = ApplyPanelEnabled;
            CarWeb.Properties.ReadOnly = ApplyPanelEnabled;
            GoodsName.Properties.ReadOnly = ApplyPanelEnabled;
            LoadPlace.Properties.ReadOnly = ApplyPanelEnabled;
            ArriveSite.Properties.ReadOnly = ApplyPanelEnabled;
            Num.Properties.ReadOnly = ApplyPanelEnabled;
            Weight.Properties.ReadOnly = ApplyPanelEnabled;
            Volume.Properties.ReadOnly = ApplyPanelEnabled;
            Freight.Properties.ReadOnly = ApplyPanelEnabled;
            Payment.Properties.ReadOnly = ApplyPanelEnabled;
            CollectFee.Properties.ReadOnly = ApplyPanelEnabled;
            PickGoodsTime.Properties.ReadOnly = ApplyPanelEnabled;
            DocuAttached.Properties.ReadOnly = ApplyPanelEnabled;
            DeliRemark.Properties.ReadOnly = ApplyPanelEnabled;
            //DeliRegMan.Properties.ReadOnly = ApplyPanelEnabled;
            UserCarDepart.Properties.ReadOnly = ApplyPanelEnabled;
            VehicleLength.Properties.ReadOnly = ApplyPanelEnabled;
            VehicleType.Properties.ReadOnly = ApplyPanelEnabled;
            DepartContactMan.Properties.ReadOnly = ApplyPanelEnabled;
            DepartTel.Properties.ReadOnly = ApplyPanelEnabled;
            VehicleWidth.Properties.ReadOnly = ApplyPanelEnabled;
            VehicleHeight.Properties.ReadOnly = ApplyPanelEnabled;
            //司机信息模块 
            panelControl2.Visible = PanelVisible;
            //VehicleNum.Properties.ReadOnly = DriverPanelEnabled;
            //DriverName.Properties.ReadOnly = DriverPanelEnabled;
            //DriverPhone.Properties.ReadOnly = DriverPanelEnabled;
            //DispatchTime.Properties.ReadOnly = DriverPanelEnabled;
            //PreToTime.Properties.ReadOnly = DriverPanelEnabled;
            //VehFare.Properties.ReadOnly = DriverPanelEnabled;

            IsRent.Properties.ReadOnly = DriverPanelEnabled;
            //ControlRemark.Properties.ReadOnly = DriverPanelEnabled;
            AcceptMan.Properties.ReadOnly = DriverPanelEnabled;

            if (!PanelVisible)
            {
                this.Height = 450;
            }
            //3)派车类型为“接货”才须关联运单
            if (DeliVehTypeValue.Equals("ReceiveGoods") || DeliVehTypeValue.Equals("SendGoods"))
            {
                //关联订单模块
                if (OperateType.Equals("Relation") || OperateType.Equals("OnlyView"))
                {
                    panelControl1.Visible = true;
                    AddOrderList();
                }
                else
                {
                    panelControl1.Visible = false;
                }
            }
            else
            {
                panelControl1.Visible = false;
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Save();
        }

        private void Save()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = null;

                //根据类型掉不同的过程
               if (OperateType.Equals("dispatch"))
               //if (OperateType.Equals("Relation"))
                {
                    #region 派车
                    if (string.IsNullOrEmpty(VehicleNum.Text.Trim()))
                    {
                        MsgBox.ShowOK("车牌号不允许为空，请输入！");
                        return;
                    }
                    //if (string.IsNullOrEmpty(DriverName.Text.Trim()))
                    //{
                    //    MsgBox.ShowOK("司机姓名不允许为空，请输入！");
                    //    return;
                    //}
                                //zhengjiafeng20180727
                    if (string.IsNullOrEmpty(VehFare.Text.Trim()) || VehFare.Text.Equals("0"))  
                    {
                        if (MsgBox.ShowYesNo("未输入派车费，是否保存？") != DialogResult.Yes)
                        {
                            return;
                        }
                    }               
                    //if (string.IsNullOrEmpty(AcceptDepart.Text.Trim()))
                    //{
                    //    MsgBox.ShowOK("受理部门不允许为空，请输入！");
                    //    return;
                    //}
                    //if (string.IsNullOrEmpty(IsRent.Text.Trim()))
                    //{
                    //    MsgBox.ShowOK("是否外租不允许为空，请输入！");
                    //    return;
                    //}

                    list.Add(new SqlPara("DeliId", DeliveryID));
                    list.Add(new SqlPara("DeliCode", DeliCode.Text.Trim()));
                    list.Add(new SqlPara("VehicleNum", VehicleNum.Text.Trim()));
                    list.Add(new SqlPara("DriverName", DriverName.Text.Trim()));
                    list.Add(new SqlPara("DriverPhone", DriverPhone.Text.Trim()));
                    list.Add(new SqlPara("DispatchTime", DispatchTime.Text.Trim()));
                    list.Add(new SqlPara("PreToTime", PreToTime.Text.Trim()));
                    list.Add(new SqlPara("VehFare", VehFare.EditValue));
                    list.Add(new SqlPara("AcceptDepart", AcceptDepart.Text.Trim()));
                    list.Add(new SqlPara("SettleState", SettleState.Text.Trim()));
                    list.Add(new SqlPara("IsRent", IsRent.Text.Trim()));
                    list.Add(new SqlPara("ControlRemark", ControlRemark.Text.Trim()));
                    list.Add(new SqlPara("AcceptMan", AcceptMan.Text.Trim()));
                    list.Add(new SqlPara("Cooperation", Cooperation));
                    list.Add(new SqlPara("ControlWeb", ControlWeb.Text.Trim()));
                    ////jl 2018-11-08 是否推送App
                    //int isSendWbps = 0;
                    //if (ckbPushApp.Checked)
                    //{
                    //    isSendWbps = 1;
                    //}
                    //list.Add(new SqlPara("IsSendWbps", isSendWbps));
                    if (checkBox1.Checked)
                    {
                        list.Add(new SqlPara("smsToDriver", "已发"));
                    }
                    if (checkBox2.Checked)
                    {
                        list.Add(new SqlPara("smsToCustom", "已发"));
                    }
                    sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLDELIVERY_Driver_Base", list);
                    #endregion
                }
                else
                {
                    #region 新增
                    string DeliVehType = "";
                    if (radioButton1.Checked)
                    {
                        DeliVehType = radioButton1.Text;
                    }
                    else if (radioButton2.Checked)
                    {
                        DeliVehType = radioButton2.Text;
                    }
                    else if (radioButton3.Checked)
                    {
                        DeliVehType = radioButton3.Text;
                    }
                    else
                    {
                        DeliVehType = radioButton4.Text;
                    }

                    if (string.IsNullOrEmpty(DeliVehType.Trim()))
                    {
                        MsgBox.ShowOK("业务类型不允许为空，请输入！");
                        return;
                    }
                    if (string.IsNullOrEmpty(VehiclesDemand.Text.Trim()))
                    {
                        MsgBox.ShowOK("用车要求不允许为空，请输入！");
                        VehiclesDemand.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(DeliCusName.Text.Trim()))
                    {
                        MsgBox.ShowOK("客户不允许为空，请输入！");
                        return;
                    }
                    if (string.IsNullOrEmpty(DeliCusPhone.Text.Trim()))
                    {
                        MsgBox.ShowOK("客户联系电话不允许为空，请输入！");
                        return;
                    }
                    if (string.IsNullOrEmpty(CarWeb.Text.Trim()))
                    {
                        MsgBox.ShowOK("申请站点不允许为空，请输入！");
                        return;
                    }
                    if (string.IsNullOrEmpty(GoodsName.Text.Trim()))
                    {
                        MsgBox.ShowOK("品名不允许为空，请输入！");
                        return;
                    }
                    if (string.IsNullOrEmpty(LoadPlace.Text.Trim()))
                    {
                        MsgBox.ShowOK("装货地不允许为空，请输入！");
                        return;
                    }
                    if (string.IsNullOrEmpty(Num.Text.Trim()) || Num.Text.Equals("0"))
                    {
                        MsgBox.ShowOK("件数不允许为空，请输入！");
                        return;
                    }
                    if ((string.IsNullOrEmpty(Weight.Text.Trim()) || Weight.Text.Equals("0")) &&
                        (string.IsNullOrEmpty(Volume.Text.Trim()) || Volume.Text.Equals("0")))
                    {
                        MsgBox.ShowOK("重量和体积不能同时为空，请输入！");
                        return;
                    }
                    //if (string.IsNullOrEmpty(Weight.Text.Trim()) || Weight.Text.Equals("0"))
                    //{
                    //    MsgBox.ShowOK("重量不允许为空，请输入！");
                    //    return;
                    //}
                    //if (CarWeb.Text.Trim() == "深圳" && ControlWeb.Text.Trim() == "")
                    //{
                    //    MsgBox.ShowOK("调车方向不能为空！，请输入！");
                    //    return;
                    //}
                    if (ArriveSite.Text.Trim() == "")
                    {
                        MsgBox.ShowOK("到站不能为空！，请输入！");
                        return;
                    }
                    //if (string.IsNullOrEmpty(Volume.Text.Trim()) || Volume.Text.Equals( "0"))
                    //{
                    //    MsgBox.ShowOK("体积不允许为空，请输入！");
                    //    return;
                    //}
                    if (string.IsNullOrEmpty(UserCarDepart.Text.Trim()))
                    {
                        MsgBox.ShowOK("需车部门不允许为空，请输入！");
                        return;
                    }
                    //if (string.IsNullOrEmpty(DepartTel.Text.Trim()))
                    //{
                    //    MsgBox.ShowOK("部门联系电话不允许为空，请输入！");
                    //    return;
                    //}

                    //hz 用车申请-新增派车申请（接货、短驳）模块的承担部门为必填项（可录入或手选筛选部门）
                    if ((radioButton1.Checked || radioButton3.Checked) && string.IsNullOrEmpty(BearDepart.Text.Trim()))
                    {
                        MsgBox.ShowOK("当前用车方式承担部门为必填项!");
                        return;
                    }


                    if (OperateType.Equals("New"))
                    {
                        DeliCode.Text = getNo(CommonClass.UserInfo.SiteName, DeliVehType);
                        if (DeliCode.Text.Trim() == "")
                        {
                            return;
                        }
                    }

                    list.Add(new SqlPara("DeliId", DeliveryID));
                    list.Add(new SqlPara("DeliCode", DeliCode.Text.Trim()));
                    list.Add(new SqlPara("DeliMan", ""));
                    list.Add(new SqlPara("DeliTime", DeliTime.Text.Trim()));
                    list.Add(new SqlPara("DeliVehType", DeliVehType));
                    list.Add(new SqlPara("VehiclesDemand", VehiclesDemand.Text.Trim()));
                    list.Add(new SqlPara("DeliCusName", DeliCusName.Text.Trim()));
                    list.Add(new SqlPara("DeliCusPhone", DeliCusPhone.Text.Trim()));
                    list.Add(new SqlPara("CarWeb", CarWeb.Text.Trim()));
                    list.Add(new SqlPara("GoodsName", GoodsName.Text.Trim()));
                    list.Add(new SqlPara("LoadPlace", LoadPlace.Text.Trim()));
                    list.Add(new SqlPara("ArriveSite", ArriveSite.Text.Trim()));
                    list.Add(new SqlPara("ContactMan", ""));
                    list.Add(new SqlPara("Num", Num.Text.Trim()));
                    list.Add(new SqlPara("Weight", Weight.Text.Trim()));
                    list.Add(new SqlPara("Volume", Volume.Text.Trim()));
                    list.Add(new SqlPara("Freight", Freight.Text.Trim()));
                    list.Add(new SqlPara("Payment", Payment.Text.Trim()));
                    list.Add(new SqlPara("CollectFee", CollectFee.Text.Trim()));
                    list.Add(new SqlPara("Vehicle", ""));
                    list.Add(new SqlPara("PickGoodsTime", PickGoodsTime.Text.Trim()));
                    list.Add(new SqlPara("BearDepart", BearDepart.Text.Trim()));
                    list.Add(new SqlPara("DocuAttached", DocuAttached.Text.Trim()));
                    list.Add(new SqlPara("DeliRemark", DeliRemark.Text.Trim()));
                    list.Add(new SqlPara("DeliRegMan", DeliRegMan.Text.Trim()));

                    if (OperateType.Equals("New"))
                    {
                        list.Add(new SqlPara("DeliState", "未派车"));
                    }
                    else
                    {
                        list.Add(new SqlPara("DeliState", DeliState));
                    }

                    list.Add(new SqlPara("UserCarDepart", UserCarDepart.Text.Trim()));
                    list.Add(new SqlPara("VehicleLength", VehicleLength.Text.Trim()));
                    list.Add(new SqlPara("VehicleType", VehicleType.Text.Trim()));
                    list.Add(new SqlPara("DepartContactMan", DepartContactMan.Text.Trim()));
                    list.Add(new SqlPara("DepartTel", DepartTel.Text.Trim()));
                    list.Add(new SqlPara("ControlWeb", ControlWeb.Text.Trim()));

                    list.Add(new SqlPara("SettleState", SettleStateValue));
                    if (checkBox1.Checked)
                    {
                        list.Add(new SqlPara("smsToDriver", "已发"));
                    }
                    if (checkBox2.Checked)
                    {
                        list.Add(new SqlPara("smsToCustom", "已发"));
                    }
                    list.Add(new SqlPara("LonAndLat", lonAndlat));
                    list.Add(new SqlPara("VehicleWidth", VehicleWidth.Text.Trim()));
                    list.Add(new SqlPara("VehicleHeight", VehicleHeight.Text.Trim()));

                    sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLDELIVERY", list);
                    #endregion
                }
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    this.Close();
                }
                #region 发送短信
                if (checkBox1.Checked)
                {
                    string chauffermb = DriverPhone.Text.Trim();
                    string chauffer = DriverName.Text.Trim();
                    string daishoustr = "";
                    if (Payment.Text.Trim() == "现付")
                    {
                        decimal acctrans = Convert.ToDecimal(CollectFee.Text.Trim() == "" ? "0" : CollectFee.Text.Trim());
                        if (acctrans > 0)
                            daishoustr = "代收" + acctrans + "元,";
                    }
                    string content = "";
                    string sWeight = ConvertType.ToString(Math.Round(ConvertType.ToDecimal(Weight.Text.Trim()), 2));
                    string sVolume = ConvertType.ToString(Math.Round(ConvertType.ToDecimal(Volume.Text.Trim()), 3));
                    string sMessage = "";
                    if(DepartContactMan.Text.Trim() != "" && DepartTel.Text.Trim() != "")
                    {
                        sMessage ="部门联系人：" +DepartContactMan.Text.Trim() +"部门联系电话："+ DepartTel.Text.Trim();
                    }
                    if(DepartContactMan.Text.Trim() != "" && DepartTel.Text.Trim() == "")
                    {
                         sMessage ="部门联系人：" +DepartContactMan.Text.Trim();
                    }
                    if(DepartContactMan.Text.Trim() == "" && DepartTel.Text.Trim() != "")
                    {
                        sMessage = "部门联系电话：" + DepartTel.Text.Trim();
                    }
                    if (PreToTime.Text.Trim() == "1900-01-01 00:00")
                        content = chauffer + "驾驶员，到" + LoadPlace.Text.Trim() + "提" + Num.Text.Trim() + "件," + sWeight + "KG," + sVolume + "方" + GoodsName.Text.Trim() + "，" + daishoustr
                        + "联系人：" + DeliCusName.Text.Trim() + ",联系电话：" + DeliCusPhone.Text.Trim() + ",申请部门：" + UserCarDepart.Text.Trim() + "，" + sMessage + "，目的地：" + ArriveSite.Text.Trim() + "，备注：" + DeliRemark.Text.Trim() + "【中强集团】";
                    else
                        content = chauffer + "驾驶员，" + PreToTime.Text.Trim() + "到" + LoadPlace.Text.Trim() + "提" + Num.Text.Trim() + "件," + sWeight + "KG," + sVolume + "方" + GoodsName.Text.Trim() + "，" + daishoustr
                         + "联系人：" + DeliCusName.Text.Trim() + ",联系电话：" + DeliCusPhone.Text.Trim() + ",申请部门：" + UserCarDepart.Text.Trim() + "，" + sMessage + "，目的地：" + ArriveSite.Text.Trim() + "，备注：" + DeliRemark.Text.Trim() + "【中强集团】";
                    if (chauffermb != "")
                    {
                        if (chauffermb.Substring(0, 1) == "1")
                        {
                            if (sms.SaveSMSS("0", chauffer, chauffermb, content, CommonClass.gcdate, "0"))
                            {
                                sms.sendsms(chauffermb, content);
                            }
                        }
                    }
                }
                if (checkBox2.Checked)
                {
                    string chauffermb = DeliCusPhone.Text.Trim();
                    string chauffer = DeliCusName.Text.Trim();
                    string content = "";
                    if (PreToTime.DateTime <= DateTime.Now)
                        content = DeliCusName.Text.Trim() + ",您好！我司将有车辆前来提货。：" + VehicleNum.Text.Trim() + ",司机：" + DriverName.Text.Trim() + ",电话：" + DriverPhone.Text.Trim() + ",请保持电话畅通！【中强集团】";
                    else
                        content = DeliCusName.Text.Trim() + ",您好！我司将有车辆前来提货。：" + VehicleNum.Text.Trim() + ",司机：" + DriverName.Text.Trim() + ",电话：" + DriverPhone.Text.Trim() + ",预计" + PreToTime.Text.Trim() + "到达！,请保持电话畅通！【中强集团】";
                    if (chauffermb != "")
                    {
                        if (chauffermb.Substring(0, 1) == "1")
                        {
                            if (sms.SaveSMSS("0", chauffer, chauffermb, content, CommonClass.gcdate, "0"))
                            {
                                sms.sendsms(chauffermb, content);
                            }
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private bool IsVehFareVerifyState(string DeliCode)
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("DeliCode", DeliCode));

            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_DELIVERY_BY_DELICODE_KT", list);
            DataSet ds = SqlHelper.GetDataSet(sps);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                if ((dr["VehFareVerifyState"] == DBNull.Value ? "0" : dr["VehFareVerifyState"]).ToString() == "1")
                {
                    return true;
                }
                else return false;

            }
            else return false;

        }

        /// <summary>
        /// 从列表删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {


                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "DeliCBId").ToString());
                if (CommonClass.QSP_LOCK_1(myGridView1.GetRowCellValue(rowhandle, "BillNo").ToString(), myGridView1.GetRowCellValue(rowhandle, "BillDate").ToString()))
                {
                    return;
                }
                if (IsVehFareVerifyState(myGridView1.GetRowCellValue(rowhandle, "DeliCode").ToString()))
                {
                    MsgBox.ShowError("派车费已核销，不允许删除运单！");
                    return;
                }
                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DeliCBId", id));
                list.Add(new SqlPara("ShareWay", ShareWay.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_BILLDELICONWAYBILL", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();

                    myGridView1.DeleteRow(rowhandle);
                    myGridView1.PostEditor();
                    myGridView1.UpdateCurrentRow();
                    myGridView1.UpdateSummary();
                    DataTable dt = myGridControl1.DataSource as DataTable;
                    dt.AcceptChanges();

                    //重新加载列表
                    AddOrderList();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 加入到列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (vehFareVerifyState == "1")
                {
                    MsgBox.ShowOK("已做过核销，无法再关联运单！");
                    return;
                }
                if (string.IsNullOrEmpty(BillNo.Text.Trim()))
                {
                    MsgBox.ShowOK("运单号不能为空！");
                    return;
                }
                //if (ReceivFee.Text.Trim() == "")
                //{
                //    MsgBox.ShowOK("本单接货费不能为空！");
                //    ReceivFee.Focus();
                //    return;
                //}
                //if (ConvertType.ToInt32(ReceivFee.Text.Trim()) <= 0)
                //{
                //    MsgBox.ShowOK("本单接货费不能小于0！");
                //    ReceivFee.Focus();
                //    return;
                //}
                if (ConvertType.ToInt32(Numing.Text.Trim()) <= 0)
                {
                    MsgBox.ShowOK("接货件数不能小于0！");
                    Num1.Focus();
                    return;
                }
                if ((Convert.ToInt32(Numed.Text.Trim()) + Convert.ToInt32(Numing.Text.Trim())) > Convert.ToInt32(Num1.Text.Trim()))
                {
                    MsgBox.ShowOK("关联接货总件数不能大于运单总件数！");
                    return;
                }
                bool isFound = false;
                if (ds != null && ds.Tables.Count > 0)
                {

                    DataRow[] drs = ds.Tables[0].Select(" BillNo='" + BillNo.Text.Trim() + "'");
                    if (drs.Length > 0)
                    {
                        isFound = true;
                    }
                    //foreach(DataRow dr in ds.Tables[0].Rows)
                    //{
                    //    if(dr["BillNo"].ToString()==BillNo.Text.Trim())
                    //    {
                    //        isFound = true;
                    //        break;
                    //    }
                    //}
                }
                if (!isFound)
                {
                    if (CommonClass.QSP_LOCK_1(BillNo.Text.Trim(), BillDate.Text.Trim()))
                    {
                        return;
                    }


                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("DeliCBId", Guid.NewGuid()));
                    list.Add(new SqlPara("DeliCode", DeliCode.Text.Trim()));
                    list.Add(new SqlPara("BillNo", BillNo.Text.Trim()));
                    list.Add(new SqlPara("ReceivFee", ReceivFee.Text.Trim()));
                    list.Add(new SqlPara("CustNo", CustNo.Text.Trim()));
                    list.Add(new SqlPara("Num", Numing.Text.Trim()));
                    list.Add(new SqlPara("ShareWay",ShareWay.Text.Trim()));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLDELICONWAYBILL", list);
                    if (SqlHelper.ExecteNonQuery(sps) > 0)
                    {
                        MsgBox.ShowOK();

                        //重新加载列表
                        AddOrderList();
                        BillNo.Text = "";
                        CustNo.Text = "";
                        Num1.Text = "";
                        ReceivFeeed.Text = "0";
                        Numed.Text = "0";
                        Numing.Text = "0";
                    }
                }
                else
                {
                    MsgBox.ShowOK("不可重复关联运单！");
                    return;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void VehicleNum_Enter(object sender, EventArgs e)
        {
            if (VehicleNum.Properties.ReadOnly) return;
            myGridControl2.Left = VehicleNum.Left;
            myGridControl2.Top = panelControl2.Top + VehicleNum.Top + VehicleNum.Height;
            myGridControl2.Visible = true;
            myGridControl2.BringToFront();
        }

        private void VehicleNum_Leave(object sender, EventArgs e)
        {
            myGridControl2.Visible = myGridControl2.Focused;
        }

        private void BillNo_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(BillNo.Text.Trim()))
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", BillNo.Text));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BillDeliveryDetail", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    MsgBox.ShowOK("您输入的运单号不存在，请确认！");
                    BillNo.Focus();
                    return;
                }
                else
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    CustNo.Text = dr["CusOderNo"].ToString();
                    BillDate.Text = dr["BillDate"].ToString();
                    Num1.Text = dr["Num"].ToString();
                    if (ds != null && ds.Tables.Count != 0 && ds.Tables[1].Rows.Count != 0)
                    {
                        DataRow dr1 = ds.Tables[1].Rows[0];
                        Numed.Text = dr1["num"].ToString();
                        ReceivFeeed.Text = dr1["ReceivFee"].ToString();
                        Numing.Text = ds.Tables[0].Rows[0]["Num"].ToString();
                    }

                }
            }
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            DeliCode.Text = getNo(CommonClass.UserInfo.SiteName, radioButton1.Text);
            BearDepart.Properties.ReadOnly = false;
            //BearDepart.Text = "";
            VehiclesDemand.Enabled = true;
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            DeliCode.Text = getNo(CommonClass.UserInfo.SiteName, radioButton2.Text);
            BearDepart.Properties.ReadOnly = true;
            VehiclesDemand.Enabled = false;
            VehiclesDemand.Text = "正常用车";
        }

        private void radioButton3_Click(object sender, EventArgs e)
        {
            DeliCode.Text = getNo(CommonClass.UserInfo.SiteName, radioButton3.Text);
            BearDepart.Properties.ReadOnly = false;
            //BearDepart.Text = "";
            VehiclesDemand.Enabled = false;
            VehiclesDemand.Text = "正常用车";
        }

        private void radioButton4_Click(object sender, EventArgs e)
        {
            DeliCode.Text = getNo(CommonClass.UserInfo.SiteName, radioButton4.Text);
            BearDepart.Properties.ReadOnly = false;
            //BearDepart.Text = "";
            VehiclesDemand.Enabled = false;
            VehiclesDemand.Text = "正常用车";
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmBillDeliveryAdd fm = new frmBillDeliveryAdd();
            fm.Show();
            this.Close();
        }

        /// <summary>
        /// 选择车辆事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public string Cooperation = "";
        private void myGridView2_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                int rowhandle = myGridView2.FocusedRowHandle;
                if (rowhandle < 0) return;
                DataRow dr = myGridView2.GetDataRow(rowhandle);
                VehicleNum.Text = ConvertType.ToString(dr["CarNo"]);
                DriverName.Text = ConvertType.ToString(dr["DriverName"]);
                DriverPhone.Text = ConvertType.ToString(dr["DriverPhone"]);
                Cooperation = ConvertType.ToString(dr["Cooperation"]);
                myGridControl2.Visible = false;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void CarWeb_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CommonClass.SetWeb(UserCarDepart, CarWeb.Text);
            CommonClass.SetWeb(BearDepart, CarWeb.Text, false);
            UserCarDepart.Text = CommonClass.UserInfo.WebName;
            BearDepart.Text = "";

            if (ArriveSite.Text.Trim() == "深圳" || ArriveSite.Text.Trim() == "深圳")
            {
                //ControlWeb.Properties.Items.Add("");
                //ControlWeb.Properties.Items.Add("深圳大坪");
                //ControlWeb.Properties.Items.Add("深圳金华伦");
                //DataSet ds = CommonClass.dsSite;
                //if (ds != null && ds.Tables.Count > 0)
                //{
                //    DataRow[] rows = ds.Tables[0].Select("SiteName='" + ArriveSite.Text.Trim() + "'");
                //    if (rows != null && rows.Length > 0)
                //    {
                //        ControlWeb.Text = ConvertType.ToString(rows[0]["ControlWeb"]);
                //    }
                //}
            }
            else
            {
                //ControlWeb.Text = "";
                //ControlWeb.Properties.Items.Clear();
            }
            //if (CarWeb.Text == "深圳")
            //{
            //    ControlWeb.Properties.ReadOnly = false;
            //    ControlWeb.Properties.Items.Add("深圳金华伦");
            //    ControlWeb.Properties.Items.Add("深圳广源");
            //    //ControlWeb.SelectedIndex = 1;
            //}
        }

        //zhengjiafeng20181023
        public void SetDeliCusName(DevExpress.XtraEditors.ComboBoxEdit DeliCusName)
        {
            List<SqlPara> list = new List<SqlPara>();
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_DeliCusName", list));
            if (ds == null || ds.Tables.Count == 0) return;
            DeliCusName.EditValue = "";
            DeliCusName.Properties.Items.Clear();
            for(int i = 0 ;i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow dr = ds.Tables[0].Rows[i];
                DeliCusName.Properties.Items.AddRange(new object[] { dr["DeliCusName"] });
            }
           
        }

        private void LoadPlace_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue != null)
            {
                gridView9.ClearColumnsFilter();
                gridView9.Columns["MiddleProvince"].FilterInfo = new ColumnFilterInfo(
                    "[MiddleStreet] LIKE " + "'%" + e.NewValue.ToString() + "%'"
                    + " OR [MiddleProvince] LIKE" + "'%" + e.NewValue.ToString() + "%'"
                    + " OR [MiddleCity] LIKE" + "'%" + e.NewValue.ToString() + "%'"
                    + " OR [MiddleArea] LIKE" + "'%" + e.NewValue.ToString() + "%'",
                    "");
            }
            else
            {
                gridView9.ClearColumnsFilter();
            }
        }

        private void LoadPlace_Enter(object sender, EventArgs e)
        {
            if (LoadPlace.Properties.ReadOnly) return;
            gridControl9.Left = LoadPlace.Left;
            gridControl9.Top = LoadPlace.Top + LoadPlace.Height;
            gridControl9.Visible = true;
            gridControl9.BringToFront();
        }

        private void LoadPlace_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            { gridControl9.Focus(); }
            if (e.KeyCode == Keys.Escape)
            {
                gridControl9.Visible = false;
            }
        }

        private void LoadPlace_Leave(object sender, EventArgs e)
        {
            gridControl9.Visible = gridControl9.Focused;
        }

        private void gridControl9_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int rowhandle = gridView9.FocusedRowHandle;
                if (rowhandle < 0) return;
                DataRow dr = gridView9.GetDataRow(rowhandle);
                LoadPlace.EditValue = ConvertType.ToString(dr["MiddleProvince"]) + ConvertType.ToString(dr["MiddleCity"]) + ConvertType.ToString(dr["MiddleArea"]) + ConvertType.ToString(dr["MiddleStreet"]);
                gridControl9.Visible = false;
                DeliCusName.Focus();
                lonAndlat = ConvertType.ToString(dr["MiddleLon"]) + "|" + ConvertType.ToString(dr["MiddleLat"]);  //经纬度
            }
            if (e.KeyCode == Keys.Escape)
            {
                gridControl9.Visible = false;
            }
        }

        private void gridControl9_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int rowhandle = gridView9.FocusedRowHandle;
            if (rowhandle < 0) return;
            DataRow dr = gridView9.GetDataRow(rowhandle);
            LoadPlace.EditValue = ConvertType.ToString(dr["MiddleProvince"]) + ConvertType.ToString(dr["MiddleCity"]) + ConvertType.ToString(dr["MiddleArea"]) + ConvertType.ToString(dr["MiddleStreet"]);
            lonAndlat = ConvertType.ToString(dr["MiddleLon"]) + "|" + ConvertType.ToString(dr["MiddleLat"]);  //经纬度
            gridControl9.Visible = false;
            DeliCusName.Focus();
        }

        private void ControlWeb_Enter(object sender, EventArgs e)
        {
            //ControlWeb.ShowPopup();
        }

        private void ArriveSite_Enter(object sender, EventArgs e)
        {
            ArriveSite.ShowPopup();
        }

        private void Payment_Enter(object sender, EventArgs e)
        {
            Payment.ShowPopup();
        }

        private void VehicleLength_Enter(object sender, EventArgs e)
        {
            VehicleLength.ShowPopup();
        }

        private void VehicleType_Enter(object sender, EventArgs e)
        {
            VehicleType.ShowPopup();
        }

        private void BearDepart_Enter(object sender, EventArgs e)
        {
            BearDepart.ShowPopup();
        }

        private void VehicleNum_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue != null)
            {
                myGridView2.ClearColumnsFilter();
                myGridView2.Columns["CarNo"].FilterInfo = new ColumnFilterInfo(
                    "[CarNo] LIKE " + "'%" + e.NewValue.ToString() + "%'"
                    + " OR [DriverName] LIKE" + "'%" + e.NewValue.ToString() + "%'"
                    + " OR [DriverPhone] LIKE" + "'%" + e.NewValue.ToString() + "%'",
                    "");
            }
            else
            {
                myGridView2.ClearColumnsFilter();
            }
        }

        private void VehicleNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            { myGridControl2.Focus(); }
            if (e.KeyCode == Keys.Escape)
            {
                myGridControl2.Visible = false;
            }
        }

        private void ArriveSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ControlWeb.Properties.Items.Clear();
            //ControlWeb.Text = "";
            //if (ArriveSite.Text.Trim() == "深圳" || ArriveSite.Text.Trim() == "深圳")
            //{
            //    DataSet ds = CommonClass.dsSite;
            //    if (ds != null && ds.Tables.Count > 0)
            //    {
            //        //ControlWeb.Properties.Items.Add("");
            //       // ControlWeb.Properties.Items.Add("大坪始发");
            //        ControlWeb.Properties.Items.Add("金华伦始发");
            //        //DataRow[] rows = ds.Tables[0].Select("SiteName='" + ArriveSite.Text.Trim() + "'");
            //        //if (rows != null && rows.Length > 0)
            //        //{
            //        //    string[] ControlWebs = ConvertType.ToString(rows[0]["ControlWeb"]).Split(',');
            //        //    if (ControlWebs == null || ControlWebs.Length <= 0)
            //        //    {
            //        //        return;
            //        //    }
            //        //    for (int i = 0; i < ControlWebs.Length; i++)
            //        //    {   
            //        //        ControlWeb.Properties.Items.Add(ControlWebs[i]);
            //        //    }
            //        //}
            //    }
            //}
            //else if (ArriveSite.Text.Trim() == "青岛" )
            //{
               
            //    ControlWeb.Properties.Items.Add("宝安始发");
            //    ControlWeb.Properties.Items.Add("金鹏始发");

            //}
            //else if (ArriveSite.Text.Trim() == "济南" )
            //{

            //    ControlWeb.Properties.Items.Add("宝安始发");
            //    ControlWeb.Properties.Items.Add("金鹏始发");
            //}
            //else if (ArriveSite.Text.Trim() == "北京" )
            //{
            //    ControlWeb.Properties.Items.Add("宝安始发");
            //}
            //else if (ArriveSite.Text.Trim() == "上海" )
            //{
            //    ControlWeb.Properties.Items.Add("宝安始发");
            //}
            //else if (ArriveSite.Text.Trim() == "贵阳")
            //{
            //    ControlWeb.Properties.Items.Add("宝安始发");
            //    ControlWeb.Properties.Items.Add("金华伦始发");

            //}
            //else if (ArriveSite.Text.Trim() == "杭州")
            //{
            //    ControlWeb.Properties.Items.Add("广源始发");          

            //}
            //else if (ArriveSite.Text.Trim() == "宁波")
            //{
            //    ControlWeb.Properties.Items.Add("广源始发");

            //}
            //else if (ArriveSite.Text.Trim() == "武汉")
            //{
            //    ControlWeb.Properties.Items.Add("金鹏始发");

            //}
            //else if (ArriveSite.Text.Trim() == "临沂")
            //{
            //    ControlWeb.Properties.Items.Add("金鹏始发");

            //}
            //else if (ArriveSite.Text.Trim() == "西安")
            //{
            //    ControlWeb.Properties.Items.Add("金鹏始发");

            //}
            //else if (ArriveSite.Text.Trim() == "成都")
            //{
            //    ControlWeb.Properties.Items.Add("金华伦始发");

            //}
            //else if (ArriveSite.Text.Trim() == "重庆")
            //{
            //    ControlWeb.Properties.Items.Add("金华伦始发");

            //}
            //else if (ArriveSite.Text.Trim() == "南宁")
            //{
            //    ControlWeb.Properties.Items.Add("金华伦始发");

            //}
            //else if (ArriveSite.Text.Trim() == "桂林")
            //{
            //    ControlWeb.Properties.Items.Add("金华伦始发");

            //}
            //else if (ArriveSite.Text.Trim() == "柳州")
            //{
            //    ControlWeb.Properties.Items.Add("金华伦始发");

            //}
            //else if (ArriveSite.Text.Trim() == "昆明")
            //{
            //    ControlWeb.Properties.Items.Add("金华伦始发");

            //}
            //else
            //{
            //    ControlWeb.Properties.Items.Clear();
            //    ControlWeb.Text = "";
            //}
            //ControlWeb.Properties.Items.Add("大坪始发");
            //ControlWeb.Properties.Items.Add("");
        }

        private void VehiclesDemand_Enter(object sender, EventArgs e)
        {
            VehiclesDemand.ShowPopup();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintCom();
        }

        private void PrintCom()
        {
            string deliCodeStr = DeliCode.Text.Trim();
            if (deliCodeStr == "") return;

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_DELIVERY_BY_DELICODE_KT", new List<SqlPara> { new SqlPara("DeliCode", deliCodeStr) }));
            if (ds == null || ds.Tables.Count == 0) return;

            frmPrintRuiLang fpr = new frmPrintRuiLang("派车单.grf", ds);
            fpr.ShowDialog();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Save();
            PrintCom();
        }

        private void ConsignorName_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //显示最近10公里以内车辆列表
            frmCircularVehicles fmV = new frmCircularVehicles();
            fmV.lonAndLat = loadLonLat;
            fmV.ShowDialog();
            VehicleNum.Text = fmV.vehiclesNo;
        }

        private void repositoryItemButtonEdit1_Click(object sender, EventArgs e)
        {
            int rowhandle = myGridView2.FocusedRowHandle;
            if (rowhandle < 0) return;
            DataRow dr = myGridView2.GetDataRow(rowhandle);
            if (dr != null)
            {
                E6GPS.GetVehiclePositionMap(ConvertType.ToString(dr["CarNo"]));
            }
        }

        private void linkLabel1_Click(object sender, EventArgs e)
        {
            if (VehicleNum.Text.Trim() != "")
            {
                List<VehicleModel> list = E6GPS.GetVehiclePosition(VehicleNum.Text);  //弹出当前车辆位置
                if (list != null && list.Count > 0)
                {
                    MsgBox.ShowOK("当前车辆所处位置：" + list[0].Provice + list[0].City + list[0].District + list[0].AreaName + list[0].RoadName);
                }
            }
        }

        private void gridControl9_Leave(object sender, EventArgs e)
        {
            gridControl9.Visible = LoadPlace.Focused;
        }

        private void myGridControl2_Leave(object sender, EventArgs e)
        {
            myGridControl2.Visible = VehicleNum.Focused;
        }

     

        private void Numing_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button2.Focus();
            }
        }

    }
}