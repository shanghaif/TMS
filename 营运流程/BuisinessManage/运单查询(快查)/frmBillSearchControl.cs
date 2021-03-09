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
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Card.ViewInfo;
using ZQTMS.Lib;
using DevExpress.XtraGrid.Views.Grid;
using gregn6Lib;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList.Nodes;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Threading;


namespace ZQTMS.UI
{
    public partial class frmBillSearchControl : BaseForm
    {
        #region 全局变量
        /// <summary>
        /// 运单编号
        /// </summary>
        string ContractNO = string.Empty;//配载合同
        public string crrBillNO = "";
        private DataRow dr; //运单

        GridppReport Report = new GridppReport();
        private DataSet ds_print;

        static frmBillSearchControl fbs;
        WebClient wc = new WebClient();

        Dictionary<string, int> DicWeigth = new Dictionary<string, int>();
        Dictionary<string, int> DicWeigth1 = new Dictionary<string, int>();
        int NewHeigh = 0;
        private string billstateNode;//用于判断快找字段隐藏节点

        /// <summary>
        /// 修改备注
        /// </summary>
        public string modifyRemark
        {
            get { return ModifyRemark.EditValue + ""; }
            set { ModifyRemark.EditValue = value; }
        }
        #endregion

        #region 窗体事件
        public frmBillSearchControl()
        {
            InitializeComponent();
        }

        public frmBillSearchControl(string BillNo)
        {
            InitializeComponent();
            this.crrBillNO = BillNo;
        }

        /// <summary>
        /// 窗口加载
        /// </summary> 
        private void frmBillSearch_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView7, myGridView9, myGridView10, myGridView11, myGridView12, myGridView1, myGridView3, myGridView14, myGridView6, myGridView22, myGridView15, myGridView23, myGridView25, myGridView26, myGridView27, myGridView29);
            GridOper.SetGridViewProperty(myGridView7, myGridView9, myGridView10, myGridView11, myGridView12, myGridView1, myGridView14, myGridView3, myGridView6, myGridView22, myGridView15, myGridView23, myGridView25, myGridView26, myGridView27, myGridView29);

            GetOrSetCondition(1);

            myGridView8.OptionsView.ShowAutoFilterRow = false;
            myGridView8.OptionsView.ShowFooter = false;
            myGridView9.OptionsView.ShowAutoFilterRow = false;
            myGridView9.OptionsView.ShowFooter = false;
            myGridView10.OptionsView.ShowAutoFilterRow = false;
            myGridView10.OptionsView.ShowFooter = false;
            myGridView11.OptionsView.ShowAutoFilterRow = false;
            myGridView11.OptionsView.ShowFooter = false;
            myGridView12.OptionsView.ShowAutoFilterRow = false;
            myGridView12.OptionsView.ShowFooter = false;


            gridView3.OptionsView.ShowFooter = false;
            //bdate.DateTime = CommonClass.gbdate;
            //edate.DateTime = CommonClass.gedate;

            //zb20190723
            if (this.Tag != null)
            {
                this.crrBillNO = this.Tag.ToString();
            }


            //hj20180828返货单号
            //setfhdh();
            GridOper.RestoreGridLayout(myGridView9, myGridView10, myGridView11, myGridView12);
            if (dr != null) return;
            if (!string.IsNullOrEmpty(crrBillNO))
            {
                //Seach(crrBillNO);
                LoadDataByBillNO();
            }
            else
                showBillBase();
            //HJ20180906
            string[] Field = null;
            string setNode = "";//快找字段隐藏生效节点
            string hiddenWeb = "";//是否隐藏本网点
            bool hiddenWebTem = true;
            DataSet ds_Hidden = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_HiddenFieldConfig", new List<SqlPara>() { new SqlPara("WebName", CommonClass.UserInfo.WebName), new SqlPara("MenuName", "快找界面") }));
            if (ds_Hidden != null && ds_Hidden.Tables.Count > 0 && ds_Hidden.Tables[0].Rows.Count > 0)
            {
                Field = ds_Hidden.Tables[0].Rows[0]["Field"].ToString().Split(',');
                setNode = ds_Hidden.Tables[0].Rows[0]["SetNode"].ToString();//获取生效节点
                hiddenWeb = ds_Hidden.Tables[0].Rows[0]["hiddenWeb"].ToString();
                if (hiddenWeb.Equals("是") && BegWeb1.Text.Equals(CommonClass.UserInfo.WebName))
                {
                    hiddenWebTem = true;
                }
                else if (hiddenWeb.Equals("否") && BegWeb1.Text.Equals(CommonClass.UserInfo.WebName))
                {
                    hiddenWebTem = false;
                }
                else
                {
                    hiddenWebTem = true;
                }
            }

            bool isHiddenNode = false;
            if (setNode.Equals("新开保存"))//判断隐藏字段开始生效节点逻辑
            {
                isHiddenNode = true;
            }
            else if (setNode.Equals("短驳到货") && (billstateNode.Equals("短驳到货") || billstateNode.Equals("拨入接收")))
            {
                isHiddenNode = true;
            }
            else if (setNode.Equals("拨入接收") && billstateNode.Equals("拨入接收"))
            {
                isHiddenNode = true;
            }

            if (Field != null && isHiddenNode && hiddenWebTem)
            {
                for (int i = 0; i < Field.Length; i++)
                {
                    Controls.Find(Field[i].ToString().Trim(), true)[0].Hide();
                }
            }

            Point pt1 = new Point(780, 60);
            ucTransparentLabel2.Location = pt1;
            ucTransparentLabel2.BringToFront();
            Point pt = new Point(860, 60);
            ucTransparentLabel1.Location = pt;
            ucTransparentLabel1.BringToFront();

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillNO", crrBillNO));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_IsPaymentAmoutClear", list);
            DataSet ds = SqlHelper.GetDataSet(sps);
            if (ds.Tables != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
            {
                ucTransparentLabel2.Text = "运费已结";
            }

            DataSet ds1 = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_OperateType", new List<SqlPara> { new SqlPara("BillNo", crrBillNO) }));
            if (ds1.Tables != null && ds1.Tables.Count != 0 && ds1.Tables[0].Rows.Count != 0)
            {
                ucTransparentLabel1.Text = ds1.Tables[0].Rows[0]["OperateType"].ToString();
            }

            //当回单要求为附清单或者为空时不显示水印 20181127 zaj
            if (!string.IsNullOrEmpty(crrBillNO))
            {
                if (dr == null) return;
                if (dr["ReceiptCondition"].ToString() == "" || dr["ReceiptCondition"].ToString() == "附清单")
                {
                    ucTransparentLabel1.Visible = false;
                }
                else
                {
                    ucTransparentLabel1.Visible = true;
                }
            }
            ReBind();
            foreach (DevExpress.XtraGrid.Columns.GridColumn column in myGridView26.Columns.View.Columns)
            {
                DicWeigth.Add(column.Name, column.Width);
            }
            int width = 0;
            foreach (DevExpress.XtraGrid.Columns.GridColumn column in myGridView1.Columns.View.Columns)
            {
                DicWeigth1.Add(column.Name, column.Width);
                width += column.Width;
            }
            myGridControl1.Width = width;
            NewHeigh = gridControl2.Size.Height;
        }

        //private void setfhdh()
        //{
        //    try
        //    {
        //        List<SqlPara> list = new List<SqlPara>();
        //        list.Add(new SqlPara("BillNO", crrBillNO));
        //        SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_ReturnModule_bybillno", list);
        //        DataSet ds = SqlHelper.GetDataSet(spe);
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            string Billno2 = ds.Tables[0].Rows[0]["Billno"].ToString();
        //            string ReturnNo = ds.Tables[0].Rows[0]["ReturnNo"].ToString();
        //            if (crrBillNO == Billno2 && ReturnNo != "")
        //            {
        //                label76.Visible = true;
        //                label77.Visible = true;
        //                label76.Text = "返货单号";
        //                label77.Text = ReturnNo;
        //            }
        //            if (crrBillNO == ReturnNo && Billno2 != "")
        //            {
        //                label76.Visible = true;
        //                label77.Visible = true;
        //                label76.Text = "原货单号";
        //                label77.Text = Billno2;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MsgBox.ShowException(ex);
        //    }
        //}

        /// <summary>
        /// 1获取条件;2保存条件
        /// </summary>
        /// <param name="type"></param>
        private void GetOrSetCondition(int type)
        {
            if (type == 1)
            {
                c1.Text = API.ReadINI("frmBillSearchControl", "c1", "", frmRuiLangService.configFileName);
                g1.Text = API.ReadINI("frmBillSearchControl", "g1", "", frmRuiLangService.configFileName);

                c2.Text = API.ReadINI("frmBillSearchControl", "c2", "", frmRuiLangService.configFileName);
                g2.Text = API.ReadINI("frmBillSearchControl", "g2", "", frmRuiLangService.configFileName);

                c3.Text = API.ReadINI("frmBillSearchControl", "c3", "", frmRuiLangService.configFileName);
                g3.Text = API.ReadINI("frmBillSearchControl", "g3", "", frmRuiLangService.configFileName);
            }
            else
            {
                API.WriteINI("frmBillSearchControl", "c1", c1.Text, frmRuiLangService.configFileName);
                API.WriteINI("frmBillSearchControl", "g1", g1.Text, frmRuiLangService.configFileName);

                API.WriteINI("frmBillSearchControl", "c2", c2.Text, frmRuiLangService.configFileName);
                API.WriteINI("frmBillSearchControl", "g2", g2.Text, frmRuiLangService.configFileName);

                API.WriteINI("frmBillSearchControl", "c3", c3.Text, frmRuiLangService.configFileName);
                API.WriteINI("frmBillSearchControl", "g3", g3.Text, frmRuiLangService.configFileName);

            }
        }

        /// <summary>
        /// 异常登记
        /// </summary> 
        private void simpleButton14_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(crrBillNO))
            {
                MsgBox.ShowOK("请先查找运单！");
                return;
            }
            fmBadBillAdd fm = new fmBadBillAdd();
            fm.billNo = crrBillNO;
            fm.ShowDialog();
        }

        /// <summary>
        /// 找列表
        /// </summary> 
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            dockPanel1.Show();
        }

        /// <summary>
        /// 找
        /// </summary> 
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            GetOrSetCondition(2);
            Seach();
            ReBind();
        }
        /// <summary>
        /// 如果是488转过去的运单 隐藏部分信息
        /// </summary>
        private void ReBind()
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillNO", crrBillNO));
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_ByID_Rebind", list));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ConsignorName1.Text = "***";
                ConsignorCompany1.Text = "***";
                ConsignorCellPhone1.Text = "***";
                //WeightPrice1.Text = "";
                //ActualFreight1.Text = "";
                //ActualFreight1.Text = "";
                DiscountTransfer1.Text = "";
                PaymentAmout1.Text = "0.00";
                DataRow dr = ds.Tables[0].Rows[0];
                if ((dr[0] == DBNull.Value ? "" : dr[0]).ToString().Trim() != "提付")
                {
                    NowPay1.Text = "";
                    ShortOwePay1.Text = "";
                    MonthPay1.Text = "";
                    Freight1.Text = "0.00";
                }



            }


        }


        /// <summary>
        /// 体积描述
        /// </summary> 
        private void simpleButton16_Click(object sender, EventArgs e)
        {
            if (myGridControl6.Visible == false)
            {
                myGridControl6.Left = simpleButton16.Left + simpleButton16.Width - myGridControl6.Width;
                myGridControl6.Top = panelControl10.Top + simpleButton16.Height;
                myGridControl6.Visible = true;
                myGridControl6.BringToFront();
            }
            else
            {
                myGridControl6.Visible = false;
            }
        }

        /// <summary>
        /// 预约送货
        /// </summary> 
        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 客户查询登记
        /// </summary> 
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(crrBillNO))
            {
                MsgBox.ShowOK("请先查找运单！");
                return;
            }
            frmCustSearchLog_Add fm = new frmCustSearchLog_Add();
            fm.BillNO = crrBillNO;
            fm.ShowDialog();
            getCUSTQUERRYLOG();
        }

        /// <summary>
        /// 修改备注按钮点击事件
        /// </summary> 
        //private void simpleButton17_Click(object sender, EventArgs e)
        //{

        //}

        /// <summary>
        /// 列表表头处理
        /// </summary> 
        private void cardView1_CustomDrawCardCaption(object sender, DevExpress.XtraGrid.Views.Card.CardCaptionCustomDrawEventArgs e)
        {
            if (gridControl2.Tag == null) return;
            if (e.RowHandle >= 0)
            {
                int count = Convert.ToInt32(gridControl2.Tag);
                (e.CardInfo as CardInfo).CaptionInfo.CardCaption = string.Format("第 {0} / {1} 次查询", e.RowHandle + 1, count);
            }
        }
        #endregion

        #region 数据加载

        /// <summary>
        /// 窗口数据加载控制器
        /// </summary>
        public void LoadDataByBillNO()
        {
            if (!getWayBillByBillNO())
            {
                MsgBox.ShowOK("对不起!没有找到该单！");
                this.Close();
                return;
            }

            //getBILLSIGNByBillNO();//获取签收信息，不用了
            //getMiddleByBillNO();

            //BindDeliveryGrid();//接货信息,不在这查了
            //BindShortConnGrid();//短驳信息,不在这查了
            //BindDepartureGrid();//配载信息,不在这查了
            //BindSENDGOODSGrid();//送货信息,不在这查了

            //BindModifiedLog();//改单记录,不在这查了

            getInWayFollowLog();//大车在途跟踪记录
            getBillTraceInfo();//轨迹信息 
            //getMiddleFollowLog();//中转跟踪记录,不在这查了
            //getCUSTQUERRYLOG();//客户查询记录,不在这查了

            //BindBespeakSendGoodsGrid();//预约送货,不在这查了

            //getVerifyOffAccount();//费用信息,不在这查了

            BindBespeakAppreciation();//增值服务
          
        }

        /// <summary>
        /// 查找列表
        /// </summary>
        private void Seach()
        {
            string ac1 = "", ac2 = "", ac3 = "", ac3Like = "", ac2Like = "", ac1Like = "";
            string ag1 = "", ag2 = "", ag3 = "";
            string av1 = "", av2 = "", av3 = "";
            string sql = "", sql1 = "";
            try
            {
                ac1 = c1.Text.Trim();
                ac2 = c2.Text.Trim();
                ac3 = c3.Text.Trim();

                #region g1
                if (g1.Text.Trim() == "等于")
                {
                    ag1 = " = ";
                }
                else if (g1.Text.Trim() == "大于")
                {
                    ag1 = " > ";
                }
                else if (g1.Text.Trim() == "小于")
                {
                    ag1 = "  < ";
                }
                else if (g1.Text.Trim() == "大于等于")
                {
                    ag1 = " >= ";
                }
                else if (g1.Text.Trim() == "小于等于")
                {
                    ag1 = " <= ";
                }
                else if (g1.Text.Trim() == "不等于")
                {
                    ag1 = " <> ";
                }
                else if (g1.Text.Trim() == "为空")
                {
                    ag1 = " is null ";
                }
                else if (g1.Text.Trim() == "不为空")
                {
                    ag1 = " is not null ";
                }
                else
                {
                    if (g1.Text.Trim() == "包含")
                    {
                        ag1 = " like ";
                    }
                }
                #endregion

                #region g2
                if (g2.Text.Trim() == "等于")
                {
                    ag2 = " = ";
                }
                else if (g2.Text.Trim() == "大于")
                {
                    ag2 = " > ";
                }
                else if (g2.Text.Trim() == "小于")
                {
                    ag2 = " < ";
                }
                else if (g2.Text.Trim() == "大于等于")
                {
                    ag2 = " >= ";
                }
                else if (g2.Text.Trim() == "小于等于")
                {
                    ag2 = " <= ";
                }
                else if (g2.Text.Trim() == "不等于")
                {
                    ag2 = " <> ";
                }
                else if (g2.Text.Trim() == "为空")
                {
                    ag2 = " is null ";
                }
                else if (g2.Text.Trim() == "不为空")
                {
                    ag2 = " is not null ";
                }
                else
                {
                    if (g2.Text.Trim() == "包含")
                    {
                        ag2 = " like ";
                    }
                }
                #endregion

                #region g3
                if (g3.Text.Trim() == "等于")
                {
                    ag3 = " = ";
                }
                else if (g3.Text.Trim() == "大于")
                {
                    ag3 = " > ";
                }
                else if (g3.Text.Trim() == "小于")
                {
                    ag3 = " < ";
                }
                else if (g3.Text.Trim() == "大于等于")
                {
                    ag3 = " >= ";
                }
                else if (g3.Text.Trim() == "小于等于")
                {
                    ag3 = " <= ";
                }
                else if (g3.Text.Trim() == "不等于")
                {
                    ag3 = " <> ";
                }
                else if (g3.Text.Trim() == "为空")
                {
                    ag3 = " is null ";
                }
                else if (g3.Text.Trim() == "不为空")
                {
                    ag3 = " is not null ";
                }
                else
                {
                    if (g3.Text.Trim() == "包含")
                    {
                        ag3 = " like ";
                    }
                }
                #endregion

                #region ac1
                if (ac1 == "到站")
                {
                    ac1 = " DestinationSite ";
                }
                else if (ac1 == "品名")
                {
                    ac1 = " Varieties ";
                }
                else if (ac1 == "收货人")
                {
                    ac1 = " ConsigneeName ";
                }
                else if (ac1 == "发货人")
                {
                    ac1 = " ConsignorName ";
                }
                else if (ac1 == "发货单位")
                {
                    ac1 = " ConsignorCompany ";
                }
                else if (ac1 == "收货单位")
                {
                    ac1 = " ConsigneeCompany ";
                }
                else if (ac1 == "件数")
                {
                    ac1 = " Num ";
                }
                else if (ac1 == "发站")
                {
                    ac1 = " StartSite ";
                }
                else if (ac1 == "运单号")
                {
                    ac1 = " BillNo ";
                }
                else if (ac1 == "电话")
                {
                    ac1 = " (ConsignorPhone='{0}' or ConsignorCellPhone='{0}' or ConsigneePhone='{0}' or ConsigneeCellPhone='{0}') ";
                    ac1Like = " (ConsignorPhone like '%{0}%' or ConsignorCellPhone like '%{0}%' or ConsigneePhone like '%{0}%' or ConsigneeCellPhone like '%{0}%') ";
                }
                else if (ac1 == "中转单号")
                {
                    ac1 = " MiddleBillNo ";
                }
                else if (ac1 == "备注")
                {
                    ac1 = " BillRemark ";
                }
                else if (ac1 == "客户单号")
                {
                    ac1 = "CusOderNo";
                }
                //else if (ac1 == "全部")
                //{
                //    ac1 = "%%";
                //}
                #endregion

                #region ac2
                if (ac2 == "到站")
                {
                    ac2 = " DestinationSite ";
                }
                else if (ac2 == "品名")
                {
                    ac2 = " Varieties ";
                }
                else if (ac2 == "收货人")
                {
                    ac2 = " ConsigneeName ";
                }
                else if (ac2 == "发货人")
                {
                    ac2 = " ConsignorName ";
                }
                else if (ac2 == "发货单位")
                {
                    ac2 = " ConsignorCompany ";
                }
                else if (ac2 == "收货单位")
                {
                    ac2 = " ConsigneeCompany ";
                }
                else if (ac2 == "件数")
                {
                    ac2 = " Num ";
                }
                else if (ac2 == "发站")
                {
                    ac2 = " StartSite ";
                }
                else if (ac2 == "运单号")
                {
                    ac2 = " BillNo ";
                }
                else if (ac2 == "电话")
                {
                    ac2 = " (ConsignorPhone='{0}' or ConsignorCellPhone='{0}' or ConsigneePhone='{0}' or ConsigneeCellPhone='{0}') ";
                    ac2Like = " (ConsignorPhone like '%{0}%' or ConsignorCellPhone like '%{0}%' or ConsigneePhone like '%{0}%' or ConsigneeCellPhone like '%{0}%') ";
                }
                else if (ac2 == "中转单号")
                {
                    ac2 = " MiddleBillNo ";
                }
                else if (ac2 == "备注")
                {
                    ac2 = " BillRemark ";
                }
                else if (ac2 == "客户单号")
                {
                    ac2 = "CusOderNo";
                }
                //else if (ac1 == "全部")
                //{
                //    ac1 = "%%";
                //}
                #endregion
                #region ac3
                if (ac3 == "到站")
                {
                    ac3 = " DestinationSite ";
                }
                else if (ac3 == "品名")
                {
                    ac3 = " Varieties ";
                }
                else if (ac3 == "收货人")
                {
                    ac3 = " ConsigneeName ";
                }
                else if (ac3 == "发货人")
                {
                    ac3 = " ConsignorName ";
                }
                else if (ac3 == "发货单位")
                {
                    ac3 = " ConsignorCompany ";
                }
                else if (ac3 == "收货单位")
                {
                    ac3 = " ConsigneeCompany ";
                }
                else if (ac3 == "件数")
                {
                    ac3 = " Num ";
                }
                else if (ac3 == "发站")
                {
                    ac3 = " StartSite ";
                }
                else if (ac3 == "运单号")
                {
                    ac3 = " BillNo ";
                }
                else if (ac3 == "电话")
                {
                    ac3 = " (ConsignorPhone='{0}' or ConsignorCellPhone='{0}' or ConsigneePhone='{0}' or ConsigneeCellPhone='{0}') ";
                    ac3Like = " (ConsignorPhone like '%{0}%' or ConsignorCellPhone like '%{0}%' or ConsigneePhone like '%{0}%' or ConsigneeCellPhone like '%{0}%') ";
                }
                else if (ac3 == "中转单号")
                {
                    ac3 = " MiddleBillNo ";
                }
                else if (ac3 == "备注")
                {
                    ac3 = " BillRemark ";
                }
                else if (ac3 == "客户单号")
                {
                    ac3 = "CusOderNo";
                }
                //else if (ac1 == "全部")
                //{
                //    ac1 = "%%";
                //}
                #endregion

                av1 = v1.Text.Trim();
                av2 = v2.Text.Trim();
                av3 = v3.Text.Trim();

                #region 检查是否选择了条件
                if (av1 != "")
                {
                    if (!CheckEmpty(c1))
                    {
                        return;
                    }
                    if (!CheckEmpty(g1))
                    {
                        return;
                    }
                }

                if (av2 != "")
                {
                    if (!CheckEmpty(c2))
                    {
                        return;
                    }
                    if (!CheckEmpty(g2))
                    {
                        return;
                    }
                }

                if (av3 != "")
                {
                    if (!CheckEmpty(c3))
                    {
                        return;
                    }
                    if (!CheckEmpty(g3))
                    {
                        return;
                    }
                }
                #endregion

                if (av1 == "" && av2 == "" && av3 == "") return;

                #region 当g1选择为空和不为空的条件时
                if (g1.Text.Trim() == "为空" || g1.Text.Trim() == "不为空")
                {
                    if (ac1 != "" && ag1 != "" && av1 == "")
                    {
                        if (c1.Text.Trim() == "电话")
                        {
                            if (ag1.Trim() == "like")
                            {
                                sql += string.Format(ac1Like, av1) + " and ";
                            }
                            else
                            {
                                sql += string.Format(ac1, av1) + " and ";
                            }
                        }
                        else
                        {
                            if (ag1.Trim() == "like")
                            {
                                sql = ac1 + ag1 + " and ";
                                sql1 = ag1 + "and companyid in (SELECT companyid FROM dbo.GetAllSubCompany(" + CommonClass.UserInfo.companyid + ")) ";
                            }
                            else
                            {
                                sql += ac1 + ag1 + " and ";
                                sql1 += ag1 + "and companyid in (SELECT companyid FROM dbo.GetAllSubCompany(" + CommonClass.UserInfo.companyid + ")) ";
                            }
                        }
                    }
                    else if (ac1 != "" && ag1 != "" && av1 != "")
                    {
                        MsgBox.ShowError("查询为空或不为空的情况不需要填参数值!");
                        return;
                    }

                }
                #endregion


                if (ac1 != "" && ag1 != "" && av1 != "")
                {
                    if (c1.Text.Trim() == "电话")
                    {
                        if (ag1.Trim() == "like")
                        {
                            sql += string.Format(ac1Like, av1) + " and ";
                        }
                        else
                        {
                            sql += string.Format(ac1, av1) + " and ";
                        }
                    }
                    else
                    {
                        if (ag1.Trim() == "like")
                        {
                            sql = ac1 + ag1 + "'%" + av1 + "%'" + " and ";
                            sql1 = ag1 + "'%" + av1 + "%'" + "and companyid in (SELECT companyid FROM dbo.GetAllSubCompany(" + CommonClass.UserInfo.companyid + ")) ";
                        }
                        else
                        {
                            sql += ac1 + ag1 + "'" + av1 + "'" + " and ";
                            sql1 += ag1 + "'" + av1 + "'" + "and companyid in (SELECT companyid FROM dbo.GetAllSubCompany(" + CommonClass.UserInfo.companyid + ")) ";
                        }
                    }
                }
                #region 当g2选择为空和不为空的条件时
                if (g2.Text.Trim() == "为空" || g2.Text.Trim() == "不为空")
                {
                    if (ac2.Trim() != "" && ag2 != "" && av2 == "")
                    {
                        if (c2.Text.Trim() == "电话")
                        {
                            if (ag2.Trim() == "like")
                            {
                                sql += string.Format(ac2Like, av2) + " and ";
                            }
                            else
                            {
                                sql += string.Format(ac2, av2) + " and ";
                            }
                        }
                        else
                        {
                            if (ag2.Trim() == "like")
                            {
                                sql += ac2 + ag2 + " and ";
                                sql1 += "and" + ac2 + ag2;
                            }
                            else
                            {
                                sql += ac2 + ag2 + " and ";
                                sql1 += "and" + ac2 + ag2;
                            }
                        }
                    }
                    else if (ac2.Trim() != "" && ag2 != "" && av2 != "")
                    {
                        MsgBox.ShowError("查询为空或不为空的情况不需要填参数值!");
                        return;
                    }
                }
                #endregion

                if (ac2.Trim() != "" && ag2 != "" && av2 != "")
                {
                    if (c2.Text.Trim() == "电话")
                    {
                        if (ag2.Trim() == "like")
                        {
                            sql += string.Format(ac2Like, av2) + " and ";
                        }
                        else
                        {
                            sql += string.Format(ac2, av2) + " and ";
                        }
                    }
                    else
                    {
                        if (ag2.Trim() == "like")
                        {
                            sql += ac2 + ag2 + "'%" + av2 + "%'" + " and ";
                            sql1 += "and" + ac2 + ag2 + "'%" + av2 + "%'";
                        }
                        else
                        {
                            sql += ac2 + ag2 + "'" + av2 + "'" + " and ";
                            sql1 += "and" + ac2 + ag2 + "'" + av2 + "'";
                        }
                    }
                }

                #region 当g3选择为空和不为空的条件时
                if (g3.Text.Trim() == "为空" || g3.Text.Trim() == "不为空")
                {
                    if (ac3 != "" && ag3 != "" && av3 == "")
                    {
                        if (c3.Text.Trim() == "电话")
                        {
                            if (ag3.Trim() == "like")
                            {
                                sql += string.Format(ac3Like, av3) + " and ";
                            }
                            else
                            {
                                sql += string.Format(ac3, av3) + " and ";
                            }
                        }
                        else
                        {
                            if (ag3.Trim() == "like")
                            {
                                sql += ac3 + ag3 + " and ";
                                sql1 += "and" + ac3 + ag3;
                            }
                            else
                            {
                                sql += ac3 + ag3 + " and ";
                                sql += "and" + ac3 + ag3;
                            }
                        }
                    }
                    else if (ac3 != "" && ag3 != "" && av3 != "")
                    {
                        MsgBox.ShowError("查询为空或不为空的情况不需要填参数值!");
                        return;
                    }
                }
                #endregion
                if (ac3 != "" && ag3 != "" && av3 != "")
                {
                    if (c3.Text.Trim() == "电话")
                    {
                        if (ag3.Trim() == "like")
                        {
                            sql += string.Format(ac3Like, av3) + " and ";
                        }
                        else
                        {
                            sql += string.Format(ac3, av3) + " and ";
                        }
                    }
                    else
                    {
                        if (ag3.Trim() == "like")
                        {
                            sql += ac3 + ag3 + "'%" + av3 + "%'" + " and ";
                            sql1 += "and" + ac3 + ag3 + "'%" + av3 + "%'";
                        }
                        else
                        {
                            sql += ac3 + ag3 + "'" + av3 + "'" + " and ";
                            sql1 += "and" + ac3 + ag3 + "'" + av3 + "'";//LMS-7004
                        }
                    }
                }

                sql = sql.Trim();
                if (sql != "")
                {
                    if (sql.Substring(sql.Length - 3, 3) == "and")
                    {
                        sql = sql.Substring(0, sql.Length - 3);
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
            if (sql == "") return;
            string dateKind = "";

            if (comboBoxEdit1.Text.Trim() == "半个月之内")
            {
                dateKind = "1";
            }
            if (comboBoxEdit1.Text.Trim() == "一个月之内")
            {
                dateKind = "2";
            }
            if (comboBoxEdit1.Text.Trim() == "二个月之内")
            {
                dateKind = "3";
            }
            if (comboBoxEdit1.Text.Trim() == "三个月之内")
            {
                dateKind = "4";
            }
            if (comboBoxEdit1.Text.Trim() == "六个月之内")
            {
                dateKind = "5";
            }
            if (comboBoxEdit1.Text.Trim() == "一年之内")
            {
                dateKind = "6";
            }
            if (comboBoxEdit1.Text.Trim() == "全部")
            {
                dateKind = "7";
            }

            //zaj 2017-11-19
            // sql = "SELECT BillNo,BillDate,ConsignorName,ConsigneeName,Varieties,Num,TransferSite,BegWeb,Weight,Volume FROM WayBill WHERE " + sql.Replace("'", "''");
            if (ac1 == "全部" && av1 != "") //hj20180630 加一个全部的筛选功能
            {
                sql = "SELECT BillNo,BillDate,ConsignorName,ConsigneeName,Varieties,Num,TransferSite,BegWeb,Weight,Volume,DestinationSite,ReceivAddress FROM WayBill WITH(NOLOCK) WHERE DestinationSite" + sql1 + "and billstate<>100"
                    + "UNION ALL SELECT BillNo,BillDate,ConsignorName,ConsigneeName,Varieties,Num,TransferSite,BegWeb,Weight,Volume,DestinationSite,ReceivAddress FROM WayBill WITH(NOLOCK) WHERE Varieties" + sql1 + "and billstate<>100"
                    + "UNION ALL SELECT BillNo,BillDate,ConsignorName,ConsigneeName,Varieties,Num,TransferSite,BegWeb,Weight,Volume,DestinationSite,ReceivAddress FROM WayBill WITH(NOLOCK) WHERE ConsigneeName" + sql1 + "and billstate<>100"
                    + "UNION ALL SELECT BillNo,BillDate,ConsignorName,ConsigneeName,Varieties,Num,TransferSite,BegWeb,Weight,Volume,DestinationSite,ReceivAddress FROM WayBill WITH(NOLOCK) WHERE ConsignorName" + sql1 + "and billstate<>100"
                    + "UNION ALL SELECT BillNo,BillDate,ConsignorName,ConsigneeName,Varieties,Num,TransferSite,BegWeb,Weight,Volume,DestinationSite,ReceivAddress FROM WayBill WITH(NOLOCK) WHERE ConsignorCompany" + sql1 + "and billstate<>100"
                    + "UNION ALL SELECT BillNo,BillDate,ConsignorName,ConsigneeName,Varieties,Num,TransferSite,BegWeb,Weight,Volume,DestinationSite,ReceivAddress FROM WayBill WITH(NOLOCK) WHERE ConsigneeCompany" + sql1 + "and billstate<>100"
                    + "UNION ALL SELECT BillNo,BillDate,ConsignorName,ConsigneeName,Varieties,Num,TransferSite,BegWeb,Weight,Volume,DestinationSite,ReceivAddress FROM WayBill WITH(NOLOCK) WHERE Num" + sql1 + "and billstate<>100"
                    + "UNION ALL SELECT BillNo,BillDate,ConsignorName,ConsigneeName,Varieties,Num,TransferSite,BegWeb,Weight,Volume,DestinationSite,ReceivAddress FROM WayBill WITH(NOLOCK) WHERE StartSite" + sql1 + "and billstate<>100"
                    + "UNION ALL SELECT BillNo,BillDate,ConsignorName,ConsigneeName,Varieties,Num,TransferSite,BegWeb,Weight,Volume,DestinationSite,ReceivAddress FROM WayBill WITH(NOLOCK) WHERE BillNo" + sql1 + "and billstate<>100"
                    + "UNION ALL SELECT BillNo,BillDate,ConsignorName,ConsigneeName,Varieties,Num,TransferSite,BegWeb,Weight,Volume,DestinationSite,ReceivAddress FROM WayBill WITH(NOLOCK) WHERE BillRemark" + sql1 + "and billstate<>100"
                    + "UNION ALL SELECT BillNo,BillDate,ConsignorName,ConsigneeName,Varieties,Num,TransferSite,BegWeb,Weight,Volume,DestinationSite,ReceivAddress FROM WayBill WITH(NOLOCK) WHERE CusOderNo" + sql1 + "and billstate<>100"
                    + "UNION ALL SELECT BillNo,BillDate,ConsignorName,ConsigneeName,Varieties,Num,TransferSite,BegWeb,Weight,Volume,DestinationSite,ReceivAddress FROM WayBill WITH(NOLOCK) WHERE ConsignorPhone" + sql1 + "and billstate<>100"
                    + "UNION ALL SELECT BillNo,BillDate,ConsignorName,ConsigneeName,Varieties,Num,TransferSite,BegWeb,Weight,Volume,DestinationSite,ReceivAddress FROM WayBill WITH(NOLOCK) WHERE ConsignorCellPhone" + sql1 + "and billstate<>100"
                    + "UNION ALL SELECT BillNo,BillDate,ConsignorName,ConsigneeName,Varieties,Num,TransferSite,BegWeb,Weight,Volume,DestinationSite,ReceivAddress FROM WayBill WITH(NOLOCK) WHERE ConsigneePhone" + sql1 + "and billstate<>100"
                    + "UNION ALL SELECT BillNo,BillDate,ConsignorName,ConsigneeName,Varieties,Num,TransferSite,BegWeb,Weight,Volume,DestinationSite,ReceivAddress FROM WayBill WITH(NOLOCK) WHERE ConsigneeCellPhone" + sql1;
            }
            else
            {
                sql = "SELECT BillNo,BillDate,ConsignorName,ConsigneeName,Varieties,Num,TransferSite,BegWeb,Weight,Volume,DestinationSite,ReceivAddress FROM WayBill WITH(NOLOCK) WHERE " + sql;
            }
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("sql", sql));
            list.Add(new SqlPara("dateKind", dateKind));

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILLNO_EXECSQL", list));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                MsgBox.ShowOK("找不到符合条件的运单!");
                return;
            }
            crrBillNO = ConvertType.ToString(ds.Tables[0].Rows[0][0]);//运单号

            xtraTabControl1.SelectedTabPageIndex = 2;          //whf20190731 begin
            Point pt1 = new Point(780, 60);
            ucTransparentLabel3.Location = pt1;
            Point pt = new Point(860, 60);
            ucTransparentLabel4.Location = pt;

            List<SqlPara> list1 = new List<SqlPara>();
            list1.Add(new SqlPara("BillNO", crrBillNO));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_IsPaymentAmoutClear", list1);
            DataSet ds2 = SqlHelper.GetDataSet(sps);
            if (ds2.Tables != null && ds2.Tables.Count != 0 && ds2.Tables[0].Rows.Count != 0)
            {
                ucTransparentLabel2.Visible = false;                     //避免2跟3重叠
                ucTransparentLabel3.Text = "运费已结";
            }
            else 
            {
                ucTransparentLabel3.Text = "运费未结";
                ucTransparentLabel2.Visible = false;  
            }

            DataSet ds1 = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_OperateType", new List<SqlPara> { new SqlPara("BillNo", crrBillNO) }));
            if (ds1.Tables != null && ds1.Tables.Count != 0 && ds1.Tables[0].Rows.Count != 0)
            {
                ucTransparentLabel4.Text = ds1.Tables[0].Rows[0]["OperateType"].ToString();
                ucTransparentLabel1.Visible = false;  
            }
            else 
            {
                ucTransparentLabel4.Text = "回单未返";
                ucTransparentLabel1.Visible = false;  
            }                                                   //whf20190731 end
            //单号列表
            myGridControl16.DataSource = ds.Tables[0];
            if (myGridView22.RowCount > 1) dockPanel1.Show();
            else dockPanel1.Hide();
            if (myGridView22.RowCount < 1000) myGridView22.BestFitColumns();

            LoadDataByBillNO();
            //whf20190731 begin
            Thread.Sleep(1000);
            xtraTabControl1.SelectedTabPageIndex = 0;  //whf20190731 end
        }

        //检测文本框是否为空
        private bool CheckEmpty(DevExpress.XtraEditors.ComboBoxEdit comboBox)
        {
            if (comboBox.Text.Trim() == "")
            {
                comboBox.Focus();
                MsgBox.ShowOK("请选择查询条件!");
                return false; //检测不通过
            }
            return true;
        }

        //private void Seach(string BillNO)
        //{
        //    try
        //    {
        //        SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_ByID", getCrrBillNOQryPara());
        //        DataSet ds = SqlHelper.GetDataSet(sps);
        //        if (ds == null || ds.Tables.Count == 0) return;
        //        myGridControl16.DataSource = ds.Tables[0];
        //        if (myGridView22.RowCount > 1) dockPanel1.Show();
        //        else dockPanel1.Hide();
        //        if (myGridView22.RowCount < 1000) myGridView22.BestFitColumns();
        //    }
        //    catch (Exception ex)
        //    {
        //        MsgBox.ShowException(ex);
        //    }
        //}
        /// <summary>
        /// 提取在途跟踪记录
        /// </summary> 
        private void getInWayFollowLog()
        {
            //try
            //{
            //    textEdit1.Text = textEdit2.Text = textEdit3.Text = textEdit4.Text = textEdit5.Text = textEdit6.Text = textEdit7.Text = textEdit8.Text = textEdit9.Text = textEdit10.Text = "";

            //    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTUREFOLLOW_ByBillNO", getCrrBillNOQryPara());
            //    DataSet ds = SqlHelper.GetDataSet(sps);
            //    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

            //    DataRow drw;

            //    for (int i = 0; i < 5; i++)
            //    {
            //        if (ds.Tables[0].Rows.Count >= (i + 1))
            //        {
            //            drw = ds.Tables[0].Rows[i];

            //            switch (i)
            //            {
            //                case 0:
            //                    textEdit1.EditValue = drw["FollowDate"];
            //                    textEdit2.Text = drw["FollowContent"] + "";
            //                    break;
            //                case 1:
            //                    textEdit3.EditValue = drw["FollowDate"];
            //                    textEdit4.Text = drw["FollowContent"] + "";
            //                    break;
            //                case 2:
            //                    textEdit5.EditValue = drw["FollowDate"];
            //                    textEdit6.Text = drw["FollowContent"] + "";
            //                    break;
            //                case 3:
            //                    textEdit7.EditValue = drw["FollowDate"];
            //                    textEdit8.Text = drw["FollowContent"] + "";
            //                    break;
            //                case 4:
            //                    textEdit9.EditValue = drw["FollowDate"];
            //                    textEdit10.Text = drw["FollowContent"] + "";
            //                    break;
            //                default: break;
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MsgBox.ShowException(ex);
            //}
        }
        /// <summary>
        /// 提取中转跟踪记录
        /// </summary>
        private void getMiddleFollowLog()
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLMIDDLETRACE_ByBillNO", getCrrBillNOQryPara());
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;

                myGridControl1.DataSource = ds.Tables[0];
                if (ds.Tables[0].Rows.Count > 7)
                {
                    myGridControl1.Size = new Size(myGridControl1.Size.Width, myGridControl1.Size.Height + (ds.Tables[0].Rows.Count - 7) * 30);
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        /// <summary>
        /// 提取客户查询登记记录
        /// </summary>
        private void getCUSTQUERRYLOG()
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLCUSTQUERRYLOG_ByBillNO", getCrrBillNOQryPara());
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                gridControl2.DataSource = ds.Tables[0];
                gridControl2.Tag = ds.Tables[0].Rows.Count + "";
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        /// <summary>
        /// 绑定接货信息
        /// </summary>
        private void BindDeliveryGrid()
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_billDelivery_ByBillNO", getCrrBillNOQryPara());
                myGridControl8.DataSource = SqlHelper.GetDataTable(sps);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 绑定短驳信息
        /// </summary>
        private void BindShortConnGrid()
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ShortConn_ByBillNO", getCrrBillNOQryPara());
                myGridControl9.DataSource = SqlHelper.GetDataTable(sps);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 绑定配载信息
        /// </summary>
        private void BindDepartureGrid()
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_billDeparture_ByBillNO", getCrrBillNOQryPara());
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds != null && ds.Tables.Count > 0)
                {
                    myGridControl10.DataSource = ds.Tables[0];
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ContractNO = ds.Tables[0].Rows[0]["ContractNO"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 绑定送货信息
        /// </summary>
        private void BindSENDGOODSGrid()
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SENDRECORD_ByBillNo", getCrrBillNOQryPara());
                myGridControl11.DataSource = SqlHelper.GetDataTable(sps);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //异常跟踪
        private void BindUnsualDetail()
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_EXCEPTION_ByBillNO", getCrrBillNOQryPara());
               // myGridControl21.DataSource = SqlHelper.GetDataTable(sps);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 绑定预约送货
        /// </summary>
        private void BindBespeakSendGoodsGrid()
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BLLBESPEAKSENDGOODS_ByBillNO", getCrrBillNOQryPara());
                //myGridControl3.DataSource = SqlHelper.GetDataTable(sps);
                myGridControl19.DataSource = SqlHelper.GetDataTable(sps);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        //hj注释
        //private void showBillBase()
        //{
        //    try
        //    {
        //        List<SqlPara> list = new List<SqlPara>();
        //        list.Add(new SqlPara("BillNO", crrBillNO));
        //       // SqlParasEntity sps = new SqlParasEntity(OperType.QueryThreeTable, "QSP_GET_WAYBILL_ByID_QuickSearch", list);
        //        SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_ByID_QuickSearch", list);//zaj

        //        DataSet ds1 = SqlHelper.GetDataSet(sps);
        //        if (ds1 == null || ds1.Tables.Count == 0) return;
        //        if (ds1.Tables[0].Rows.Count > 0) dr = ds1.Tables[0].Rows[0];
        //        if (ds1.Tables.Count > 1)
        //        {
        //            gcstate.DataSource = ds1.Tables[1];
        //            gvstate.BestFitColumns();
        //        }

        //        string path_str = Application.StartupPath + "\\Reports\\运单快找信息.grf";
        //        if (File.Exists(path_str))
        //        {
        //            try
        //            {
        //                if (axGRDisplayViewer1.Report != null)
        //                    axGRDisplayViewer1.Stop();

        //                Report.LoadFromFile(path_str);
        //                Report.LoadDataFromXML(ds1.GetXml());
        //                axGRDisplayViewer1.Report = Report;

        //                axGRDisplayViewer1.Start();
        //                imageComboBoxEdit1.EditValue = dr == null ? "" : dr["BillState"];
        //            }
        //            catch (Exception ex)
        //            {
        //                MsgBox.ShowOK(ex.Message);
        //            }
        //        }
        //    }
        //    catch  { }
        //}

        //hj20180906
        private void showBillBase()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNO", crrBillNO));
                // SqlParasEntity sps = new SqlParasEntity(OperType.QueryThreeTable, "QSP_GET_WAYBILL_ByID_QuickSearch", list);
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_ByID_QuickSearch", list);//zaj

                DataSet ds1 = SqlHelper.GetDataSet(sps);
                if (ds1 == null || ds1.Tables.Count == 0 || ds1.Tables[0].Rows.Count == 0) return;
                dr = ds1.Tables[0].Rows[0];

                string BillState = dr["BillState"].ToString();

                int temBillState = int.Parse(BillState);

                if (temBillState < 7)
                {
                    billstateNode = "新开保存";
                }
                else if (temBillState > 6 && temBillState < 17)
                {
                    billstateNode = "短驳到货";
                }
                else if (temBillState > 16)
                {
                    billstateNode = "拨入接收";
                }

                switch (BillState)
                {
                    case "0":
                        BillState = "新开";
                        break;
                    case "1":
                        BillState = "短驳";
                        break;
                    case "2":
                        BillState = "短驳接收";
                        break;
                    case "3":
                        BillState = "再短驳";
                        break;
                    case "4":
                        BillState = "再短驳接收";
                        break;
                    case "5":
                        BillState = "发车";
                        break;
                    case "6":
                        BillState = "又发";
                        break;
                    case "7":
                        BillState = "到货";
                        break;
                    case "8":
                        BillState = "又到";
                        break;
                    case "9":
                        BillState = "中转";
                        break;
                    case "10":
                        BillState = "转送到网点";
                        break;
                    case "11":
                        BillState = "转送接收";
                        break;
                    case "12":
                        BillState = "又转送到网点";
                        break;
                    case "13":
                        BillState = "又转送接收";
                        break;
                    case "14":
                        BillState = "送货";
                        break;
                    case "15":
                        BillState = "送货退回";
                        break;
                    case "16":
                        BillState = "签收";
                        break;
                    case "17":
                        BillState = "分拨到干线";
                        break;
                    case "18":
                        BillState = "干线接收";
                        break;
                    case "19":
                        BillState = "分拨到终端";
                        break;
                    case "20":
                        BillState = "终端接收";
                        break;
                    case "99":
                        BillState = "退货";
                        break;
                    case "100":
                        BillState = "作废";
                        break;
                }
                string PartyType = dr["PartyType"].ToString();
                if (PartyType == "POS")
                {
                    BillState += "(pos机:";
                }
                if (PartyType == "WX")
                {
                    BillState += "(微信:";
                }
                BillState += dr["PartyAmount"].ToString();
                if (PartyType == "POS" || PartyType == "WX")
                {
                    BillState += ")";
                }
                BillState1.Text = BillState;
                BillDate1.Text = dr["BillDate"].ToString();
                TransferMode1.Text = dr["TransferMode"].ToString();
                TransferSite1.Text = dr["TransferSite"].ToString();
                BillNo1.Text = dr["BillNo"].ToString();
                StartSite1.Text = dr["StartSite"].ToString();
                DestinationSite1.Text = dr["DestinationSite"].ToString();
                BegWeb1.Text = dr["BegWeb"].ToString();
                PickGoodsSite1.Text = dr["PickGoodsSite"].ToString();
                TransitMode1.Text = dr["TransitMode"].ToString();
                //TransitLines1.Text = dr["TransitLines"].ToString();
                CusOderNo1.Text = dr["CusOderNo"].ToString();
                Salesman1.Text = dr["Salesman"].ToString();
                ConsignorName1.Text = dr["ConsignorName"].ToString();
                ConsignorCompany1.Text = dr["ConsignorCompany"].ToString();
                ConsignorCellPhone1.Text = dr["ConsignorCellPhone"].ToString();
                //CusNo1.Text = dr["CusNo"].ToString();
                ConsigneeName1.Text = dr["ConsigneeName"].ToString();
                ConsigneeCompany1.Text = dr["ConsigneeCompany"].ToString();
                ConsigneeCellPhone1.Text = dr["ConsigneeCellPhone"].ToString();
                ReceivMode1.Text = dr["ReceivMode"].ToString();
                ReceivAddress1.Text = dr["ReceivAddress"].ToString();
                VehicleNo1.Text = dr["VehicleNo"].ToString();
                ReceivOrderNo1.Text = dr["ReceivOrderNo"].ToString();
                EndDate.Text = dr["EndDate"].ToString();   //zjf20181128
                //label87.Text = dr[""].ToString();
                BillMan1.Text = dr["BillMan"].ToString();
                Varieties1.Text = dr["Varieties"].ToString();
                Num1.Text = dr["Num"].ToString();
                LeftNum1.Text = dr["LeftNum"].ToString();
                Package1.Text = dr["Package"].ToString();
                FeeWeight1.Text = dr["FeeWeight"].ToString();
                FeeVolume1.Text = dr["FeeVolume"].ToString();
                Weight1.Text = dr["Weight"].ToString();
                Volume1.Text = dr["Volume"].ToString();

                FeeType1.Text = dr["FeeType"].ToString();
                OptWeight1.Text = dr["OptWeight"].ToString();
                OperationWeight1.Text = dr["OperationWeight"].ToString();
                Freight1.Text = dr["Freight"].ToString();
                PaymentAmout1.Text = dr["PaymentAmout"].ToString();
                ReceiptCondition1.Text = dr["ReceiptCondition"].ToString();
                ReceiptFee1.Text = dr["ReceiptFee"].ToString() == "0.00" ? "" : dr["ReceiptFee"].ToString();
                DeclareValue1.Text = dr["DeclareValue"].ToString() == "0.00" ? "" : dr["DeclareValue"].ToString();
                SupportValue1.Text = dr["SupportValue"].ToString() == "0.00" ? "" : dr["SupportValue"].ToString();
                CollectionPay1.Text = dr["CollectionPay"].ToString() == "0.00" ? "" : dr["CollectionPay"].ToString();
                AgentFee1.Text = dr["AgentFee"].ToString() == "0.00" ? "" : dr["AgentFee"].ToString();
                InformationFee1.Text = dr["InformationFee"].ToString() == "0.00" ? "" : dr["InformationFee"].ToString();
                Expense1.Text = dr["Expense"].ToString() == "0.00" ? "" : dr["Expense"].ToString();
                FuelFee1.Text = dr["FuelFee"].ToString() == "0.00" ? "" : dr["FuelFee"].ToString();
                Tax1.Text = dr["Tax"].ToString() == "0.00" ? "" : dr["Tax"].ToString();
                DiscountTransfer1.Text = dr["DiscountTransfer"].ToString() == "0.00" ? "" : dr["DiscountTransfer"].ToString();
                DeliFee1.Text = dr["DeliFee"].ToString() == "0.00" ? "" : dr["DeliFee"].ToString();
                //PayMode1.Text = dr["PayMode"].ToString() == "0.00" ? "" : dr["PayMode"].ToString();
                ReceivFee1.Text = dr["ReceivFee"].ToString() == "0.00" ? "" : dr["ReceivFee"].ToString();
                NoticeFee1.Text = dr["NoticeFee"].ToString() == "0.00" ? "" : dr["NoticeFee"].ToString();
                PackagFee1.Text = dr["PackagFee"].ToString() == "0.00" ? "" : dr["PackagFee"].ToString();
                FrameFee1.Text = dr["FrameFee"].ToString() == "0.00" ? "" : dr["FrameFee"].ToString();
                StorageFee1.Text = dr["StorageFee"].ToString() == "0.00" ? "" : dr["StorageFee"].ToString();
                CustomsFee1.Text = dr["CustomsFee"].ToString() == "0.00" ? "" : dr["CustomsFee"].ToString();
                WarehouseFee1.Text = dr["WarehouseFee"].ToString() == "0.00" ? "" : dr["WarehouseFee"].ToString();
                HandleFee1.Text = dr["HandleFee"].ToString() == "0.00" ? "" : dr["HandleFee"].ToString();
                UpstairFee1.Text = dr["UpstairFee"].ToString() == "0.00" ? "" : dr["UpstairFee"].ToString();
                ForkliftFee1.Text = dr["ForkliftFee"].ToString() == "0.00" ? "" : dr["ForkliftFee"].ToString();

                MiddleSendFee1.Text = dr["MiddleSendFee"].ToString() == "0.00" ? "" : dr["MiddleSendFee"].ToString();
                ChangeFee1.Text = dr["ChangeFee"].ToString() == "0.00" ? "" : dr["ChangeFee"].ToString();
                OtherFee1.Text = dr["OtherFee"].ToString() == "0.00" ? "" : dr["OtherFee"].ToString();
                OtherTotalFee1.Text = dr["OtherTotalFee"].ToString() == "0.00" ? "" : dr["OtherTotalFee"].ToString();
                AmountMoney1.Text = dr["AmountMoney"].ToString() == "0.00" ? "" : dr["AmountMoney"].ToString();
                IsRebates1.Text = dr["IsRebates"].ToString() == "1" ? "是" : "否";
                NoticeState1.Text = dr["NoticeState"].ToString() == "1" ? "是" : "否";
                IsInvoice1.Text = dr["IsInvoice"].ToString() == "1" ? "是" : "否";
                MiddleBackFee1.Text = dr["MiddleBackFee"].ToString() == "0.00" ? "" : dr["MiddleBackFee"].ToString();
                DeliveryFee1.Text = dr["DeliveryFee"].ToString() == "0.000" ? "" : dr["DeliveryFee"].ToString();
                TransferFee1.Text = dr["TransferFee"].ToString() == "0.000" ? "" : dr["TransferFee"].ToString();
                ChangeAddFee1.Text = dr["ChangeAddFee"].ToString() == "0.00" ? "" : dr["ChangeAddFee"].ToString();
                changeType1.Text = dr["changeType"].ToString();
                NowPay1.Text = dr["NowPay"].ToString() == "0.00" ? "" : dr["NowPay"].ToString();
                FetchPay1.Text = dr["FetchPay"].ToString() == "0.00" ? "" : dr["FetchPay"].ToString();
                MonthPay1.Text = dr["MonthPay"].ToString() == "0.00" ? "" : dr["MonthPay"].ToString();
                BefArrivalPay1.Text = dr["BefArrivalPay"].ToString() == "0.00" ? "" : dr["BefArrivalPay"].ToString();
                ReceiptPay1.Text = dr["ReceiptPay"].ToString() == "0.00" ? "" : dr["ReceiptPay"].ToString();
                ShortOwePay1.Text = dr["ShortOwePay"].ToString() == "0.00" ? "" : dr["ShortOwePay"].ToString();
                MiddleConfirmState1.Text = dr["MiddleConfirmState"].ToString();
                FeeChange1.Text = dr["FeeChange"].ToString();
                //label229.Text = dr[""].ToString();
                BillRemark1.Text = dr["BillRemark"].ToString();
                ModifyRemark1.Text = dr["ModifyRemark"].ToString();
                PaymentMode1.Text = dr["PaymentMode"].ToString();
                MiddleDate1.Text = dr["MiddleDate"].ToString();
                MiddleCarrier1.Text = dr["MiddleCarrier"].ToString();
                MiddleStartSitePhone1.Text = dr["MiddleStartSitePhone"].ToString();
                MiddleEndSitePhone1.Text = dr["MiddleEndSitePhone"].ToString();
                MiddleBillNo1.Text = dr["MiddleBillNo"].ToString();
                MiddleOperator1.Text = dr["MiddleOperator"].ToString();
                AccMiddlePay1.Text = dr["AccMiddlePay"].ToString();
                MiddleBatch1.Text = dr["MiddleBatch"].ToString();
                MiddleRemark1.Text = dr["MiddleRemark"].ToString();
                MiddleFetchAddress1.Text = dr["MiddleFetchAddress"].ToString();
                SignMan1.Text = dr["SignMan"].ToString();
                SignTime1.Text = dr["SignTime"].ToString();
                SignManCardID1.Text = dr["SignManCardID"].ToString();
                //label281.Text = dr["AgentMan"].ToString();
                //label279.Text = dr["AgentCardId"].ToString();
                SignOperator1.Text = dr["SignOperator"].ToString();
                //label273.Text = dr["SignType"].ToString();
                SignRemark1.Text = dr["SignRemark"].ToString();
                //label269.Text = dr[""].ToString();
                //label267.Text = dr[""].ToString();
                DispatchTime1.Text = dr["DispatchTime"].ToString();
                SCDate1.Text = dr["SCDate"].ToString();
                WebDate1.Text = dr["WebDate"].ToString();
                PrintDate1.Text = dr["Operdate"].ToString();
                AcceptBillDate1.Text = dr["AcceptBillDate"].ToString();
                SendTime1.Text = dr["SendTime"].ToString();
                MatPay1.Text = dr["MatPay"].ToString();
                SignTime1.Text = dr["SignTime"].ToString();
                Varieties2.Text = dr["Varieties1"].ToString();
                Num2.Text = dr["Num1"].ToString();
                Package2.Text = dr["Package1"].ToString();
                FeeWeight2.Text = dr["FeeWeight1"].ToString();
                FeeVolume2.Text = dr["FeeVolume1"].ToString();
                Weight2.Text = dr["Weight1"].ToString();
                Volume2.Text = dr["Volume1"].ToString();
                FeeType2.Text = dr["FeeType1"].ToString();
                //OutTime.Text = dr["OutTime"].ToString();//hj20181205增加出库时间

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 根据运单编号获取运单
        /// </summary>
        private bool getWayBillByBillNO()
        {
            //SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_ByID", getCrrBillNOQryPara());
            //ds_print = SqlHelper.GetDataSet(sps);

            //if (ds_print.Tables[0] == null && ds_print.Tables[0].Rows.Count == 0) return false;

            //if (ds_print.Tables[1] != null)
            //{
            //    myGridControl6.DataSource = ds_print.Tables[1];
            //}
            //if (ds_print.Tables[0].Rows.Count == 0) return false;     
            showBillBase();
            //dr = ds_print.Tables[0].Rows[0];
            if (dr == null) return false;

            modifyRemark = dr["ModifyRemark"] + "";
            //BillNO.EditValue = dr["BillNO"];
            //VehicleNo.EditValue = dr["VehicleNo"];
            //BillState.EditValue = dr["BillState"];
            //TransferMode.EditValue = dr["TransferMode"];
            //TransitMode.EditValue = dr["TransitMode"];
            //CusOderNo.EditValue = dr["CusOderNo"];
            //ConsigneeCellPhone.EditValue = dr["ConsigneeCellPhone"];
            //ConsigneeName.EditValue = dr["ConsigneeName"];
            //ConsigneeCompany.EditValue = dr["ConsigneeCompany"];
            //PickGoodsSite.EditValue = dr["PickGoodsSite"];
            //ReceivAddress.EditValue = dr["ReceivAddress"];
            //ConsignorCellPhone.EditValue = dr["ConsignorCellPhone"];
            //ConsignorName.EditValue = dr["ConsignorName"];
            //ConsignorCompany.EditValue = dr["ConsignorCompany"];
            //ReceivMode.EditValue = dr["ReceivMode"];
            //CusNo.EditValue = dr["CusNo"];
            //ReceivOrderNo.EditValue = dr["ReceivOrderNo"];
            //Salesman.EditValue = dr["Salesman"];
            //NoticeState.EditValue = dr["NoticeState"];
            //GoodsType.EditValue = dr["GoodsType"];
            //Varieties.EditValue = dr["Varieties"];
            //Num.EditValue = dr["Num"];
            //LeftNum.EditValue = dr["LeftNum"];
            //FeeWeight.EditValue = dr["FeeWeight"];
            //FeeVolume.EditValue = dr["FeeVolume"];
            //Weight.EditValue = dr["Weight"];
            //Volume.EditValue = dr["Volume"];
            //FeeType.EditValue = dr["FeeType"];
            //Freight.EditValue = dr["Freight"];
            //DeliFee.EditValue = dr["DeliFee"];
            //ReceivFee.EditValue = dr["ReceivFee"];
            //DeclareValue.EditValue = dr["DeclareValue"];
            //SupportValue.EditValue = dr["SupportValue"];
            //Tax.EditValue = dr["Tax"];
            //InformationFee.EditValue = dr["InformationFee"];
            //Expense.EditValue = dr["Expense"];
            //NoticeFee.EditValue = dr["NoticeFee"];
            //DiscountTransfer.EditValue = dr["DiscountTransfer"];
            //CollectionPay.EditValue = dr["CollectionPay"];
            //AgentFee.EditValue = dr["AgentFee"];
            //FuelFee.EditValue = dr["FuelFee"];
            //UpstairFee.EditValue = dr["UpstairFee"];
            //HandleFee.EditValue = dr["HandleFee"];
            //ForkliftFee.EditValue = dr["ForkliftFee"];
            //StorageFee.EditValue = dr["StorageFee"];
            //CustomsFee.EditValue = dr["CustomsFee"];
            //packagFee.EditValue = dr["packagFee"];
            //FrameFee.EditValue = dr["FrameFee"];
            //ChangeFee.EditValue = dr["ChangeFee"];
            //OtherFee.EditValue = dr["OtherFee"];
            //IsInvoice.EditValue = dr["IsInvoice"];
            //ReceiptFee.EditValue = dr["ReceiptFee"];
            //ReceiptCondition.EditValue = dr["ReceiptCondition"];
            //FreightAmount.EditValue = dr["PaymentAmout"];
            //CouponsNo.EditValue = dr["CouponsNo"];
            //CouponsAmount.EditValue = dr["CouponsAmount"];
            //PaymentMode.EditValue = dr["PaymentMode"];
            //PayMode.EditValue = dr["PayMode"];
            //NowPay.EditValue = dr["NowPay"];
            //FetchPay.EditValue = dr["FetchPay"];
            //MonthPay.EditValue = dr["MonthPay"];
            //ShortOwePay.EditValue = dr["ShortOwePay"];
            //BefArrivalPay.EditValue = dr["BefArrivalPay"];
            //BillRemark.EditValue = dr["BillRemark"];
            //ModifyRemark.EditValue = dr["SignRemark"];
            //WebDate.EditValue = dr["WebDate"];
            //OtherTotalFee.EditValue = dr["OtherTotalFee"];
            //BillMan.EditValue = dr["BillMan"];
            //begWeb.EditValue = dr["begWeb"];
            //Package.EditValue = dr["Package"];
            //MiddleDate.EditValue = dr["MiddleDate"];
            //ModifyRemark.EditValue = dr["ModifyRemark"];

            ////中转信息
            //MiddleFetchAddress.EditValue = dr["MiddleFetchAddress"];
            //MiddleRemark.EditValue = dr["MiddleRemark"];
            //AccMiddlePay.EditValue = dr["AccMiddlePay"];
            //MiddleOperator.EditValue = dr["MiddleOperator"];
            //MiddleBillNo.EditValue = dr["MiddleBillNo"];
            //MiddleEndSitePhone.EditValue = dr["MiddleEndSitePhone"];
            //MiddleStartSitePhone.EditValue = dr["MiddleStartSitePhone"];
            //MiddleCarrier.EditValue = dr["MiddleCarrier"];
            //MiddleDate.EditValue = dr["MiddleDate"];
            //MiddleBatch.EditValue = dr["MiddleBatch"];
            return true;
        }
        /// <summary>
        /// 根据运单编号获取中转信息
        /// </summary>
        private void getMiddleByBillNO()
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLMIDDLE_ByBillNO", getCrrBillNOQryPara());
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0) return;

                DataRow dtr = ds.Tables[0].Rows[0];
                MiddleFetchAddress.EditValue = dtr["MiddleFetchAddress"];
                MiddleRemark.EditValue = dtr["MiddleRemark"];
                AccMiddlePay.EditValue = dtr["AccMiddlePay"];
                MiddleOperator.EditValue = dtr["MiddleOperator"];
                MiddleBillNo.EditValue = dtr["MiddleBillNo"];
                MiddleEndSitePhone.EditValue = dtr["MiddleEndSitePhone"];
                MiddleStartSitePhone.EditValue = dtr["MiddleStartSitePhone"];
                MiddleCarrier.EditValue = dtr["MiddleCarrier"];
                MiddleDate.EditValue = dtr["MiddleDate"];
                MiddleBatch.EditValue = dtr["MiddleBatch"];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 根据运单编号获取签收信息
        /// </summary>
        private void getBILLSIGNByBillNO()
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLSIGN_ByBillNO", getCrrBillNOQryPara());
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0) return;

                DataRow dtr = ds.Tables[0].Rows[0];
                SignMan.EditValue = dtr["SignMan"];
                SignManCardID.EditValue = dtr["SignManCardID"];
                SignOperator.EditValue = dtr["SignOperator"];
                AgentMan.EditValue = dtr["AgentMan"];

                AgentCardId.EditValue = dtr["AgentCardId"];
                SignDate.EditValue = dtr["SignDate"];
                SignType.EditValue = dtr["SignType"];
                SignContent.EditValue = dtr["SignContent"];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 参数列表 根据参数运单编号查找
        /// </summary> 
        private List<SqlPara> getCrrBillNOQryPara()
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillNO", crrBillNO));
            return list;
        }
        #endregion

        /// <summary>
        /// 显示快找
        /// </summary>
        public static void ShowBillSearch()
        {
            frmBillSearchControl f = new frmBillSearchControl();
            f.Show();
        }

        /// <summary>
        /// 显示快找,传运单号查找
        /// </summary>
        /// <param name="BillNo">运单号</param>
        public static void ShowBillSearch(string BillNo)
        {
            if (BillNo == "") return;
            if (fbs == null || fbs.IsDisposed) fbs = new frmBillSearchControl();
            fbs.crrBillNO = BillNo;
            fbs.LoadDataByBillNO();
            fbs.Show();
            fbs.TopMost = true;
            fbs.TopMost = false;
        }

        /// <summary>
        /// 显示快找
        /// </summary>
        public static void ShowBillSearch(GridView gv, string fileName)
        {
            if (gv == null || gv.RowCount == 0 || gv.FocusedRowHandle < 0) return;
            string billno = ConvertType.ToString(gv.GetRowCellValue(gv.FocusedRowHandle, fileName));
            if (billno == "") return;
            if (fbs == null || fbs.IsDisposed) fbs = new frmBillSearchControl();
            fbs.crrBillNO = billno;
            fbs.LoadDataByBillNO();
            fbs.Show();
            fbs.TopMost = true;
            fbs.TopMost = false;
        }

        /// <summary>
        /// 绑定改单记录
        /// </summary>
        private void BindModifiedLog()
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "GET_MODIFIED_WAYBILL", getCrrBillNOQryPara());
                myGridControl13.DataSource = SqlHelper.GetDataTable(sps);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            gridControl1.Visible = true;
            gridControl5.Visible = false;
            if (string.IsNullOrEmpty(crrBillNO))
            {
                MsgBox.ShowOK("请先查找运单！");
                return;
            }

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("unit", crrBillNO));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BAD_TYD_Q", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                gridControl1.DataSource = ds.Tables[0];

                BindUnsualDetail();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void repositoryItemHyperLinkEdit2_Click(object sender, EventArgs e)
        {
            fmFileShow fm = new fmFileShow();
            fm.billNo = crrBillNO;
            fm.billType = 1;
            fm.ShowDialog();
        }

        private void frmBillSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Dispose();
        }

        private void v1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) simpleButton5.PerformClick();
        }
        private void BindBespeakAppreciation()
        {
            try
            {
                //SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ReceiptbyBillNo", getCrrBillNOQryPara());

                //DataSet ds = SqlHelper.GetDataSet(sps);
                //if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                //SqlParasEntity sps2 = new SqlParasEntity(OperType.Query, "QSP_GET_BILLSIGN_ByBillNO", getCrrBillNOQryPara());
                //DataSet ds2 = SqlHelper.GetDataSet(sps);
                //if (ds2 == null || ds2.Tables[0] == null || ds2.Tables[0].Rows.Count == 0) return;

                SqlParasEntity sps2 = new SqlParasEntity(OperType.Query, "QSP_GET_ReceiptbyBillNo_New", getCrrBillNOQryPara());
                DataSet ds2 = SqlHelper.GetDataSet(sps2);
                if (ds2 != null && ds2.Tables.Count > 0)
                {
                    myGridControl17.DataSource = ds2.Tables[0];
                }
                //回单情况查询
                //foreach (DataRow dr in ds.Tables[0].Rows)
                //{
                //    if (ConvertType.ToString(dr["OperateType"]) == "回单寄出")
                //    {
                //        lab1.Text = ConvertType.ToString(dr["OperateTime"]);
                //        lab2.Text = ConvertType.ToString(dr["OperateState"]);
                //        lab3.Text = ConvertType.ToString(dr["Operator"]);
                //    }
                //    if (ConvertType.ToString(dr["OperateType"]) == "回单返回")
                //    {
                //        lab4.Text = ConvertType.ToString(dr["OperateTime"]);
                //        lab5.Text = ConvertType.ToString(dr["OperateState"]);
                //        lab6.Text = ConvertType.ToString(dr["Operator"]);
                //    }
                //    if (ConvertType.ToString(dr["OperateType"]) == "回单返厂")
                //    {
                //        lab7.Text = ConvertType.ToString(dr["OperateTime"]);
                //        lab8.Text = ConvertType.ToString(dr["OperateState"]);
                //        lab9.Text = ConvertType.ToString(dr["Operator"]);
                //    }
                //    if (ConvertType.ToString(dr["OperateType"]) == "客户取单")
                //    {
                //        lab10.Text = ConvertType.ToString(dr["OperateTime"]);
                //        lab11.Text = ConvertType.ToString(dr["OperateState"]);
                //        lab12.Text = ConvertType.ToString(dr["Operator"]);
                //    }
                //}
                SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_ByID", getCrrBillNOQryPara());
                DataSet ds1 = SqlHelper.GetDataSet(sps1);
                if (ds1 == null || ds1.Tables.Count == 0 || ds1.Tables[0].Rows.Count == 0) return;
                DataRow dtr = ds1.Tables[0].Rows[0];
                //labCollectionPay.Text = ConvertType.ToString(dtr["CollectionName"]);
                //labCollectionAccount.Text = ConvertType.ToString(dtr["CollectionAccount"]);
                //labCollectionBank.Text = ConvertType.ToString(dtr["CollectionBank"]);
                //labCollectionBranch.Text = ConvertType.ToString(dtr["CollectionBranch"]);

                labDiscountTransfer.Text = ConvertType.ToString(dtr["DisTranName"]);
                labDisTranAccount.Text = ConvertType.ToString(dtr["DisTranAccount"]);
                labDisTranBank.Text = ConvertType.ToString(dtr["DisTranBank"]);
                labDisTranBranch.Text = ConvertType.ToString(dtr["DisTranBranch"]);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //费用及改单记录
        private void getVerifyOffAccount()
        {
            if (string.IsNullOrEmpty(crrBillNO))
            {
                MsgBox.ShowOK("请先查找运单！");
                return;
            }

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("billno", crrBillNO));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_VerifyOffAccount", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl2.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {

            if (xtraTabControl1.SelectedTabPage == xtraTabPage3)
            {
                comboBoxEdit2.Properties.Items.Clear();
                if (myGridView11.GridControl.DataSource != null)
                {
                    DataTable dt = myGridView11.GridControl.DataSource as DataTable;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        comboBoxEdit2.Properties.Items.Add(dt.Rows[0]["CarNo"]);
                    }
                    int count = comboBoxEdit2.Properties.Items.Count;
                    if (count > 0)
                    {
                        comboBoxEdit2.Text = comboBoxEdit2.Properties.Items[count - 1].ToString();
                    }
                }
            }

            if (string.IsNullOrEmpty(crrBillNO)) return;
            //车辆信息
            if (xtraTabControl1.SelectedTabPage == xtraTabPage2)
            {
                BindDeliveryGrid();//接货信息
                BindShortConnGrid();//短驳信息
                BindDepartureGrid();//配载信息
                BindSENDGOODSGrid();//送货信息
                QSP_GET_OPERLOG_BillNO();
                myGridView9.BestFitColumns();
                myGridView10.BestFitColumns();
                myGridView11.BestFitColumns();
                myGridView12.BestFitColumns();
            }
            else if (xtraTabControl1.SelectedTabPage == xtraTabPage3)
            {
                getMiddleFollowLog();//中转跟踪记录
                getCUSTQUERRYLOG();//客户查询记录
            }
            //费用及改单记录
            else if (xtraTabControl1.SelectedTabPage == xtraTabPage4)
            {
                BindModifiedLog();//改单记录
                getVerifyOffAccount();//费用信息
            }
            //else if (xtraTabControl1.SelectedTabPage == xtraTabPage7)
            //{
            //    simpleButton2.PerformClick();
            //}
            //else if (xtraTabControl1.SelectedTabPage == xtraTabPage8)
            //{
            //    BindBespeakSendGoodsGrid();//预约送货
            //}
            else if (xtraTabControl1.SelectedTabPage == xtraTabPage10)
            {
                QSP_GET_BILLACCDETAIL();
                QSP_GET_Tz_BillNo();
                QSP_GET_WAYBILLC_Fee_ByID();
            }
            else
            {
                //if (xtraTabControl1.SelectedTabPage == xtraTabPage11) QSP_GET_OPERLOG_BillNO();
            }
        }

        private void QSP_GET_OPERLOG_BillNO()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("billno", crrBillNO));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_OPERLOG_BillNO", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl12.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
            getBillRepertory();
        }

        //hh20181221  可通版快找界面增加“所在库存”列表显示库存信息
        private void getBillRepertory()
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillNo", this.crrBillNO));
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BillRepertory", list));
            myGridControl18.DataSource = ds.Tables[0];
            myGridView27.BestFitColumns();
        }

        private void QSP_GET_BILLACCDETAIL()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("billno", crrBillNO));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLACCDETAIL", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;

                myGridControl5.DataSource = ds.Tables[0];
                if (ds.Tables[1].Rows.Count == 0) return;//防止报“位置0处没有任何误”错误 hj
                PaymentAmout.Text = ds.Tables[1].Rows[0]["ActualFreight"].ToString();
                LunFeatch.Text = ds.Tables[1].Rows[0]["unFetchCharge"].ToString();
                LendAccount.Text = ds.Tables[1].Rows[0]["AmountMoney"].ToString();
                accjs.Text = ds.Tables[1].Rows[0]["accjs"].ToString();
                accml.Text = ds.Tables[1].Rows[0]["accml"].ToString();
                mllv.Text = ds.Tables[1].Rows[0]["mllv"].ToString();
                jsml.Text = ds.Tables[1].Rows[0]["jsml"].ToString();

            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void QSP_GET_Tz_BillNo()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("billno", crrBillNO));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_Tz_BillNo", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;

                myGridControl14.DataSource = ds.Tables[0];

            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void QSP_GET_WAYBILLC_Fee_ByID()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("billno", crrBillNO));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILLC_Fee_ByID", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;

                myGridControl7.DataSource = ds.Tables[0];

            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(crrBillNO))
            //{
            //    MsgBox.ShowOK("请先查找运单！");
            //    return;
            //}
            //try
            //{
            //    textEdit11.Text = dr["ConsignorName"] + "";
            //    List<SqlPara> list = new List<SqlPara>();
            //    list.Add(new SqlPara("ConsignorName", dr["ConsignorName"]));
            //    list.Add(new SqlPara("t1", bdate.DateTime));
            //    list.Add(new SqlPara("t2", edate.DateTime));

            //    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_MiddleFollow_Quick", list);
            //    DataSet ds = SqlHelper.GetDataSet(sps);

            //    if (ds == null || ds.Tables.Count == 0) return;
            //    myGridControl15.DataSource = ds.Tables[0];
            //}
            //catch (Exception ex)
            //{
            //    MsgBox.ShowError(ex.Message);
            //}
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            string vno = comboBoxEdit2.Text.Trim();////取车牌号出来
            if (vno == "")
            {
                MsgBox.ShowOK("没有车牌号，无法定位!");
                return;
            }
            List<VehicleModel> list = E6GPS.GetVehiclePosition(vno);
            if (list == null || list.Count == 0) return;

            string pos = list[0].Provice + list[0].City + list[0].District + list[0].RoadName;
            MsgBox.ShowOK(pos);
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileInfo fi = new FileInfo(openFileDialog1.FileName);
                //if (Math.Round(((double)fi.Length) / 1024, 2) > 200)
                //{
                //    MsgBox.ShowOK("图片大小不能大于200KB");
                //    return;
                //}
                labelControl99.Text = this.openFileDialog1.FileName;
            }
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(crrBillNO))
            {
                MsgBox.ShowOK("请先查找运单！");
                return;
            }

            Thread th = new Thread(UpLoadFile);
            th.IsBackground = true;
            th.Start();
        }

        private void UpLoadFile()
        {
            string path = labelControl99.Text.Trim();
            if (path == "未选择")
            {
                MsgBox.ShowError("请选择一个图片文件！");
                return;
            }
            if (path.EndsWith(".jpg") || path.EndsWith(".jpeg") || path.EndsWith(".png"))
            {
                PicDeal.SendSmallImage(path, ref path, 800, 600);
            }
            string ResultPath = string.Format("/Files/{0}/{3}-{1}{2}", DateTime.Now.ToString("yyyy-MM-dd"), Guid.NewGuid().ToString(), Path.GetExtension(path), crrBillNO);
            if (!this.IsHandleCreated) return;
            try
            {
                this.Invoke((MethodInvoker)delegate
                {
                    labelControl1.Text = "文件上传中，请稍后";
                    simpleButton8.Enabled = false;
                    simpleButton9.Enabled = false;
                    timer1.Enabled = true;
                    Application.DoEvents();
                });

                byte[] bt = wc.UploadFile(new Uri(string.Format("{1}/FileUpLoad.ashx?Path={0}", ResultPath, FileUpload.UpFileUrl)), "POST", path);
                string json = Encoding.UTF8.GetString(bt);
                frmUpLoadFile.UploadResult result = JsonConvert.DeserializeObject<frmUpLoadFile.UploadResult>(json);

                if (result.State == 1)
                {
                    if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_SAVE_WAYBILL_FILE", new List<SqlPara> { new SqlPara("BillNo", crrBillNO), new SqlPara("FilePath", ResultPath), new SqlPara("FileName", Path.GetFileName(openFileDialog1.FileName)) })) == 0) result.State = 0;
                }

                if (!this.IsHandleCreated) return;
                this.Invoke((MethodInvoker)delegate
                {
                    if (result.State == 1)
                    {
                        labelControl1.Text = "上传成功!";
                    }
                    else
                    {
                        labelControl1.Text = "上传失败!";
                    }
                    timer1.Enabled = false;
                    simpleButton8.Enabled = true;
                    simpleButton9.Enabled = true;
                });
            }
            catch (Exception ex)
            {
                if (IsHandleCreated)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        labelControl1.Text = "上传失败  原因：" + ex.Message;
                        timer1.Enabled = false;
                        simpleButton8.Enabled = true;
                        simpleButton9.Enabled = true;
                    });
                }
            }
            finally
            {
                if (IsHandleCreated)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        labelControl99.Text = "未选择";
                    });
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (labelControl1.Text.Trim().Length <= 20)
            {
                labelControl1.Text += ".";
            }
            else
                labelControl1.Text = "文件上传中，请稍后";
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(crrBillNO))
            {
                MsgBox.ShowOK("请先查找运单！");
                return;
            }

            try
            {
                myGridControl4.DataSource = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_FILE", new List<SqlPara> { new SqlPara("BillNo", crrBillNO) })).Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
            finally
            {
                if (myGridView20.RowCount < 1000) myGridView20.BestFitColumns();
            }
        }

        private void myGridView20_DoubleClick(object sender, EventArgs e)
        {
            FileUpload.ShowImg(myGridView20);
        }

        private void myGridView20_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e == null || e.RowHandle < 0) return;
            e.Value = e.RowHandle + 1;
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (string.IsNullOrEmpty(crrBillNO)) return;
            Clipboard.SetText(crrBillNO);

            //IGRField field = Report.FieldByName("运单号");
            //if (field == null) return;
            //Clipboard.SetText(field.AsString);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (string.IsNullOrEmpty(crrBillNO)) return;

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("billno", crrBillNO));
                list.Add(new SqlPara("BillType", 0));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BillPic_By_BillNo", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    MsgBox.ShowOK("本单暂无签收图片!");
                    return;
                }

                string filename = ds.Tables[0].Rows[0]["FilePath"].ToString();
                string isSynData = ds.Tables[0].Rows[0]["IsSynData"].ToString();//是否同步过来的数据 zaj 20108-6-2
                string bdPath = System.Windows.Forms.Application.StartupPath + "/TempFile";
                if (!Directory.Exists(bdPath)) Directory.CreateDirectory(bdPath);
                string bdFileName = bdPath + filename.Substring(filename.LastIndexOf("/"));

                if (!File.Exists(bdFileName))
                {
                    WebClient wc = new WebClient();
                    string loadUrl = isSynData == "1" ? FileUpload.UpFileUrlZQTMS + filename : FileUpload.UpFileUrl + filename;//是否同步过来的数据 zaj 20108-5-2
                    wc.DownloadFile(loadUrl, bdFileName);
                }

                frmShowPic frm = new frmShowPic();
                frm.imgPath = bdFileName;
                frm.Show();

            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView9, myGridView10, myGridView11, myGridView12);
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView9.Guid.ToString(), myGridView10.Guid.ToString(), myGridView11.Guid.ToString(), myGridView12.Guid.ToString());
        }

        private void gridView5_DoubleClick(object sender, EventArgs e)
        {
            int rows = gridView5.FocusedRowHandle;
            if (rows < 0) return;
            fmBadBillDeal wb = new fmBadBillDeal();
            wb.gv = gridView5;
            wb.look = 11;
            wb.ShowDialog();
        }

        private void myGridView22_Click(object sender, EventArgs e)
        {
            if (myGridView22.FocusedRowHandle < 0) return;
            crrBillNO = GridOper.GetRowCellValueString(myGridView22, "BillNo");
            LoadDataByBillNO();
        }

        private void simpleButton10_Click(object sender, EventArgs e)//zxw 补传图片 2016-12-20
        {

            int rowHandle = gridView3.FocusedRowHandle;
            if (rowHandle < 0)
            {
                MsgBox.ShowOK("请先选择一条异常信息！");
                return;
            }
            else
            {
                fmFileUploadM fm = new fmFileUploadM();
                crrBillNO = GridOper.GetRowCellValueString(gridView3, "BillNo");
                fm.ishowdel = false;
                fm.UserName = CommonClass.UserInfo.UserName;
                fm.billNo = crrBillNO;
                fm.UpType = "upadd";
                fm.ShowDialog();
            }
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            // if (myGridView1.FocusedRowHandle < 0) return;
            string billNo = crrBillNO;
            if (billNo == "")
            {
                MsgBox.ShowOK("没有找到要打印的运单,请稍后再试");
                return;
            }

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTLABEL_DEV2", new List<SqlPara> { new SqlPara("BillNo", billNo) }));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                MsgBox.ShowOK("没有找到要打印的运单,请稍后再试");
                return;
            }

            frmPrintLabelDev fpld = new frmPrintLabelDev(ds.Tables[0]);
            fpld.ShowDialog();
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(crrBillNO))
            {
                MsgBox.ShowOK("请先查找运单！");
                return;
            }
            fmPettypayAdd fm = new fmPettypayAdd();
            fm.billNo = crrBillNO;
            fm.ShowDialog();
        }

        private void imageComboBoxEdit1_MouseEnter(object sender, EventArgs e)
        {
            try { (sender as Control).Focus(); }
            catch { }
        }

        //hj注释
        //private void imageComboBoxEdit1_MouseLeave(object sender, EventArgs e)
        //{
        //    axGRDisplayViewer1.Focus();
        //    Thread th = new Thread(() =>
        //    {
        //        Thread.Sleep(20);
        //        if (!this.IsHandleCreated) return;
        //        this.Invoke((MethodInvoker)delegate
        //        {
        //            gcstate.Visible = imageComboBoxEdit1.Focused || gcstate.Focused;
        //        });
        //    });
        //    th.IsBackground = true;
        //    th.Start();
        //}

        //hj注释
        //private void gcstate_MouseLeave(object sender, EventArgs e)
        //{
        //    axGRDisplayViewer1.Focus();
        //    Thread th = new Thread(() =>
        //    {
        //        Thread.Sleep(20);
        //        if (!this.IsHandleCreated) return;
        //        this.Invoke((MethodInvoker)delegate
        //        {
        //            gcstate.Visible = imageComboBoxEdit1.Focused || gcstate.Focused;
        //        });
        //    });
        //    th.IsBackground = true;
        //    th.Start();
        //}

        //hj注释
        //private void imageComboBoxEdit1_Enter(object sender, EventArgs e)
        //{
        //    gcstate.Visible = true;
        //}

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (string.IsNullOrEmpty(crrBillNO)) return;

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("billno", crrBillNO));
                list.Add(new SqlPara("BillType", 7));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BillPic_By_BillNo", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    MsgBox.ShowOK("本单暂无回单图片!");
                    return;
                }

                string filename = ds.Tables[0].Rows[0]["FilePath"].ToString();
                string isSynData = ds.Tables[0].Rows[0]["IsSynData"].ToString();//是否同步过来的数据 zaj 20108-5-2
                string bdPath = System.Windows.Forms.Application.StartupPath + "/TempFile";
                if (!Directory.Exists(bdPath)) Directory.CreateDirectory(bdPath);
                string bdFileName = bdPath + filename.Substring(filename.LastIndexOf("/"));

                if (!File.Exists(bdFileName))
                {
                    WebClient wc = new WebClient();
                    string loadUrl = isSynData == "1" ? FileUpload.UpFileUrlZQTMS + filename : FileUpload.UpFileUrl + filename;//是否同步过来的数据 zaj 20108-5-2
                    wc.DownloadFile(loadUrl, bdFileName);//是否同步过来的数据 zaj 20108-5-2
                }

                frmShowPic frm = new frmShowPic();
                frm.imgPath = bdFileName;
                frm.Show();

            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        //hj增加轨迹信息 20180426
        private void getBillTraceInfo()
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BillTrace_ByBillNO", getCrrBillNOQryPara());
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                DataRow drw;
                //label36.Text = crrBillNO;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    drw = ds.Tables[0].Rows[i];
                    if (i < 14)
                    {
                        switch (i)
                        {
                            case 0:
                                txteTrace1.EditValue = drw["tracedate"] + "  " + drw["optwebname"];
                                txtTrace1.Text = drw["content"].ToString();
                                break;
                            case 1:
                                txteTrace2.EditValue = drw["tracedate"] + "  " + drw["optwebname"];
                                txtTrace2.Text = drw["content"].ToString();
                                break;
                            case 2:
                                txteTrace3.EditValue = drw["tracedate"] + "  " + drw["optwebname"];
                                txtTrace3.Text = drw["content"].ToString();
                                break;
                            case 3:
                                txteTrace4.EditValue = drw["tracedate"] + "  " + drw["optwebname"];
                                txtTrace4.Text = drw["content"].ToString();
                                break;
                            case 4:
                                txteTrace5.EditValue = drw["tracedate"] + "  " + drw["optwebname"];
                                txtTrace5.Text = drw["content"].ToString();
                                break;
                            case 5:
                                txteTrace6.EditValue = drw["tracedate"] + "  " + drw["optwebname"];
                                txtTrace6.Text = drw["content"].ToString();
                                break;
                            case 6:
                                txteTrace7.EditValue = drw["tracedate"] + "  " + drw["optwebname"];
                                txtTrace7.Text = drw["content"].ToString();
                                break;
                            case 7:
                                txteTrace8.EditValue = drw["tracedate"] + "  " + drw["optwebname"];
                                txtTrace8.Text = drw["content"].ToString();
                                break;
                            case 8:
                                txteTrace9.EditValue = drw["tracedate"] + "  " + drw["optwebname"];
                                txtTrace9.Text = drw["content"].ToString();
                                break;
                            case 9:
                                txteTrace10.EditValue = drw["tracedate"] + "  " + drw["optwebname"];
                                txtTrace10.Text = drw["content"].ToString();
                                break;
                            case 10:
                                txteTrace11.EditValue = drw["tracedate"] + "  " + drw["optwebname"];
                                txtTrace11.Text = drw["content"].ToString();
                                break;
                            case 11:
                                txteTrace12.EditValue = drw["tracedate"] + "  " + drw["optwebname"];
                                txtTrace12.Text = drw["content"].ToString();
                                break;
                            case 12:
                                txteTrace13.EditValue = drw["tracedate"] + "  " + drw["optwebname"];
                                txtTrace13.Text = drw["content"].ToString();
                                break;
                            case 13:
                                txteTrace14.EditValue = drw["tracedate"] + "  " + drw["optwebname"];
                                txtTrace14.Text = drw["content"].ToString();
                                break;
                            case 14:
                                txteTrace15.EditValue = drw["tracedate"] + "  " + drw["optwebname"];
                                txtTrace15.Text = drw["content"].ToString();
                                break;
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void simpleButton19_Click(object sender, EventArgs e)  //maohui20180619
        {
            if (string.IsNullOrEmpty(crrBillNO))
            {
                MsgBox.ShowOK("请先查找运单！");
                return;
            }
            fmBadBillAddClone fm = new fmBadBillAddClone();
            fm.billNo = crrBillNO;
            fm.Show();
        }

        private void simpleButton15_Click(object sender, EventArgs e)  //maohui20180619
        {
            gridControl5.Visible = false;
            gridControl1.Visible = true;
            if (string.IsNullOrEmpty(crrBillNO))
            {
                MsgBox.ShowOK("请先查找运单！");
                return;
            }

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("unit", crrBillNO));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_AbnormalRegistration_Q", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                gridControl1.DataSource = ds.Tables[0];

                BindUnsualDetail();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void simpleButton18_Click(object sender, EventArgs e)//maohui20180619
        {
            int rowHandle = gridView3.FocusedRowHandle;
            if (rowHandle < 0)
            {
                MsgBox.ShowOK("请先选择一条异常信息！");
                return;
            }
            else
            {
                fmFileUploadM fm = new fmFileUploadM();
                crrBillNO = GridOper.GetRowCellValueString(gridView3, "BillNo");
                fm.ishowdel = false;
                fm.UserName = CommonClass.UserInfo.UserName;
                fm.billNo = crrBillNO;
                fm.UpType = "upadd";
                fm.ShowDialog();
            }
        }

        private void gridView3_DoubleClick(object sender, EventArgs e)//maohui20180619
        {
            //int rows = gridView3.FocusedRowHandle;
            //string billno = gridView3.GetRowCellValue(rows, "BillNo").ToString();
            //if (rows < 0) return;
            //fmBadBillDealClone wb = new fmBadBillDealClone();
            //wb.gv = gridView3;
            //wb.look = 11;
            //wb.crrBillNO = billno;
            //wb.ShowDialog();
            int rows = gridView3.FocusedRowHandle;
            if (rows < 0) return;
            fmBadBillDeal wb = new fmBadBillDeal();
            wb.gv = gridView3;
            wb.look = 11;
            wb.ShowDialog();
        }

        private void repositoryItemHyperLinkEdit1_Click(object sender, EventArgs e)//maohui20180619
        {
            fmFileShow fm = new fmFileShow();
            fm.billNo = crrBillNO;
            fm.billType = 1;
            fm.ShowDialog();
        }

        /// <summary>
        /// 修改备注按钮点击事件
        /// </summary> 
        private void simpleButton17_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(crrBillNO))
            {
                MsgBox.ShowOK("请先查找运单！");
                return;
            }
            frmUpdateDescr fm = new frmUpdateDescr();
            fm.ModifyRemark = modifyRemark;
            fm.BillNO = crrBillNO;
            fm.ShowDialog();
            modifyRemark = fm.ModifyRemark;

            getWayBillByBillNO();
        }

        private void xtraTabControl1_Click(object sender, EventArgs e)
        {

           
            //Point pt1 = new Point(780, 60);
            //ucTransparentLabel3.Location = pt1;
            ////ucTransparentLabel2.BringToFront();
            //Point pt = new Point(860, 60);
            //ucTransparentLabel4.Location = pt;
            ////ucTransparentLabel1.BringToFront();

            //List<SqlPara> list = new List<SqlPara>();
            //list.Add(new SqlPara("BillNO", crrBillNO));
            //SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_IsPaymentAmoutClear", list);
            //DataSet ds = SqlHelper.GetDataSet(sps);
            //if (ds.Tables != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
            //{
            //    ucTransparentLabel3.Text = "运费已结";
            //}
            //DataSet ds1 = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_OperateType", new List<SqlPara> { new SqlPara("BillNo", crrBillNO) }));
            //if (ds1.Tables != null && ds1.Tables.Count != 0 && ds1.Tables[0].Rows.Count != 0)
            //{
            //    ucTransparentLabel4.Text = ds1.Tables[0].Rows[0]["OperateType"].ToString();
            //}
        }

        private void 打印标签ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // if (myGridView1.FocusedRowHandle < 0) return;
            string billNo = BillNo1.Text.Trim();
            if (billNo == "")
            {
                MsgBox.ShowOK("没有找到要打印的运单,请稍后再试");
                return;
            }

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTLABEL_DEV2", new List<SqlPara> { new SqlPara("BillNo", billNo) }));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) 
            {
                MsgBox.ShowOK("没有找到要打印的运单,请稍后再试");
                return;
            }

            frmPrintLabelDev fpld = new frmPrintLabelDev(ds.Tables[0]);
            fpld.ShowDialog();
        }

        private void 打印信封ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRuiLangService.Print("信封", SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTENVELOPE", new List<SqlPara> { new SqlPara("BillNo", BillNo1.Text.Trim()) })));
        }

        private void ddtPrint_Click(object sender, EventArgs e)
        {
            this.contextMenuStrip2.Show(ddtPrint, 0, ddtPrint.Height);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (string.IsNullOrEmpty(crrBillNO)) return;

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("billno", crrBillNO));
                list.Add(new SqlPara("BillType", 7));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BillPic_By_BillNo", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    MsgBox.ShowOK("本单暂无回单图片!");
                    return;
                }
                string bdPath = System.Windows.Forms.Application.StartupPath + "/TempFile";
                if (!Directory.Exists(bdPath)) Directory.CreateDirectory(bdPath);

                List<imgs> lst = new List<imgs>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    imgs img = new imgs();
                    string filename = ds.Tables[0].Rows[i]["FilePath"].ToString();
                    string bdFileName = bdPath + filename.Substring(filename.LastIndexOf("/"));
                    string isSynData = ds.Tables[0].Rows[i]["IsSynData"].ToString();//是否同步过来的数据 zaj 20108-6-27

                    if (!File.Exists(bdFileName))
                    {
                        WebClient wc = new WebClient();
                        //string loadUrl = isSynData == "1" ? FileUpload.UpFileUrlZQTMS + filename : FileUpload.UpFileUrl + filename;//是否同步过来的数据 zaj 20108-6-27
                        string loadUrl;
                        if (ConvertType.ToInt32(ds.Tables[0].Rows[i]["FileSource"]) == 1)
                        {
                            loadUrl = FileUpload.UpFileUrlTX + filename;
                        }
                        else
                        {
                            loadUrl = isSynData == "1" ? FileUpload.UpFileUrlZQTMS + filename : FileUpload.UpFileUrl + filename;//是否同步过来的数据 zaj 20108-6-27
                        }
                        //  wc.DownloadFile(FileUpload.UpFileUrl + filename, bdFileName);
                        wc.DownloadFile(loadUrl, bdFileName);

                    }
                    img.BdFileName = bdFileName;
                    lst.Add(img);
                }
                //string filename = ds.Tables[0].Rows[0]["FilePath"].ToString();
                //string isSynData = ds.Tables[0].Rows[0]["IsSynData"].ToString();//是否同步过来的数据 zaj 20108-5-2
                //string bdPath = System.Windows.Forms.Application.StartupPath + "/TempFile";
                //if (!Directory.Exists(bdPath)) Directory.CreateDirectory(bdPath);
                //string bdFileName = bdPath + filename.Substring(filename.LastIndexOf("/"));

                //if (!File.Exists(bdFileName))
                //{
                //    WebClient wc = new WebClient();
                //    string loadUrl = isSynData == "1" ? FileUpload.UpFileUrlZQTMS + filename : FileUpload.UpFileUrl + filename;//是否同步过来的数据 zaj 20108-5-2
                //    wc.DownloadFile(loadUrl, bdFileName);//是否同步过来的数据 zaj 20108-5-2
                //}

                frmShowPic frm = new frmShowPic();
                frm.imgs = lst;
                frm.Show();


            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void linkLabel2_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (string.IsNullOrEmpty(crrBillNO)) return;

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("billno", crrBillNO));
                list.Add(new SqlPara("BillType", 0));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BillPic_By_BillNo", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    MsgBox.ShowOK("本单暂无签收图片!");
                    return;
                }
                string bdPath = System.Windows.Forms.Application.StartupPath + "/TempFile";
                if (!Directory.Exists(bdPath)) Directory.CreateDirectory(bdPath);

                List<imgs> lst = new List<imgs>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    imgs img = new imgs();
                    string filename = ds.Tables[0].Rows[i]["FilePath"].ToString();
                    string bdFileName = bdPath + filename.Substring(filename.LastIndexOf("/"));
                    string isSynData = ds.Tables[0].Rows[i]["IsSynData"].ToString();//是否同步过来的数据 zaj 20108-6-27

                    if (!File.Exists(bdFileName))
                    {
                        WebClient wc = new WebClient();
                        string loadUrl = isSynData == "1" ? FileUpload.UpFileUrlZQTMS + filename : FileUpload.UpFileUrl + filename;//是否同步过来的数据 zaj 20108-6-27
                        //  wc.DownloadFile(FileUpload.UpFileUrl + filename, bdFileName);
                        wc.DownloadFile(loadUrl, bdFileName);

                    }
                    img.BdFileName = bdFileName;
                    lst.Add(img);
                }

                //string filename = ds.Tables[0].Rows[0]["FilePath"].ToString();
                //string isSynData = ds.Tables[0].Rows[0]["IsSynData"].ToString();//是否同步过来的数据 zaj 20108-6-2
                //string bdPath = System.Windows.Forms.Application.StartupPath + "/TempFile";
                //if (!Directory.Exists(bdPath)) Directory.CreateDirectory(bdPath);
                //string bdFileName = bdPath + filename.Substring(filename.LastIndexOf("/"));

                //if (!File.Exists(bdFileName))
                //{
                //    WebClient wc = new WebClient();
                //    string loadUrl = isSynData == "1" ? FileUpload.UpFileUrlZQTMS + filename : FileUpload.UpFileUrl + filename;//是否同步过来的数据 zaj 20108-5-2
                //    wc.DownloadFile(loadUrl, bdFileName);
                //}
                frmShowPic frm = new frmShowPic();
                frm.imgs = lst;
                frm.Show();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        //好多车位置实时查询
        private void sp_HaoDuoChe_Click(object sender, EventArgs e)
        {
            BindDepartureGrid();
            if (string.IsNullOrEmpty(ContractNO))
            {
                MsgBox.ShowOK("合同编号为空，查询失败！");
                return;
            }
            DataTable dt = CommonSyn.GetHaoDuoCheCodeData(ContractNO, CommonClass.UserInfo.token);//好多车任务车辆实时位置查询
            if (dt != null && dt.Rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("司机名称：" + dt.Rows[0]["driverName"].ToString() + "\n");
                sb.Append("车牌号：" + dt.Rows[0]["carNumber"].ToString() + "\n");
                sb.Append("车辆经度：" + dt.Rows[0]["carLongitude"].ToString() + "\n");
                sb.Append("车辆纬度：" + dt.Rows[0]["carLatitude"].ToString() + "\n");
                sb.Append("车辆地址：" + dt.Rows[0]["carAddress"].ToString() + "\n");
                sb.Append("司机电话：" + dt.Rows[0]["driverphone"].ToString() + "\n");
                sb.Append("司机经度：" + dt.Rows[0]["phoneLongitude"].ToString() + "\n");
                sb.Append("司机纬度：" + dt.Rows[0]["phoneLatitude"].ToString() + "\n");
                sb.Append("司机地址：" + dt.Rows[0]["address"].ToString() + "\n");

                MessageBox.Show(sb.ToString(), "订单实时位置查询结果：", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
        }

        private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (dr == null)
            {
                MsgBox.ShowOK("请先查出运单信息,再进行收款操作！");
                return;
            } 
            frmCheckstand c = new frmCheckstand();
            c.type = 1;
            c.billNo = dr["billNo"].ToString(); ;
            c.paymentAmout = dr["PaymentAmout"].ToString();//ds1.Tables[0].Rows[0]["PaymentAmout"].ToString();
            c.ShowDialog();
        }

        private void myGridView26_ColumnWidthChanged(object sender, DevExpress.XtraGrid.Views.Base.ColumnEventArgs e)
        {

            int weigth_M = e.Column.Width;
            int oldweigth = DicWeigth[e.Column.Name];
            myGridControl12.Size = new Size(myGridControl12.Size.Width + (weigth_M - oldweigth), myGridControl12.Size.Height);
        }

        private void myGridView1_ColumnWidthChanged(object sender, DevExpress.XtraGrid.Views.Base.ColumnEventArgs e)
        {

            int weigth_M = e.Column.Width;
            int oldweigth = DicWeigth1[e.Column.Name];
            myGridControl1.Size = new Size(myGridControl1.Size.Width + (weigth_M - oldweigth), myGridControl1.Size.Height);
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(crrBillNO))
            {
                MsgBox.ShowOK("请先查找运单！");
                return;
            }
            //frmBespokSend fm = new frmBespokSend();
            //fm.BillNO = crrBillNO;
            //fm.ShowDialog();
            frmAppointmentSend frm = new frmAppointmentSend();
            frm.crrBillNO = crrBillNO;
            frm.ShowDialog();
            BindBespeakSendGoodsGrid();
        }

        private void 打印托运单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.BegWeb1.Text.ToString().Trim() == CommonClass.UserInfo.WebName.ToString().Trim())
            {
                string billNo = BillNo1.Text.Trim();
                string name = "";
                if (CommonClass.UserInfo.BookNote == "")
                {


                    name = CommonClass.UserInfo.IsAutoBill == false ? "托运单" : "托运单(打印条码)";
                }
                else
                {

                    if (CommonClass.UserInfo.companyid == "486" && CommonClass.UserInfo.WebName == "宝虹广州项目部")
                    {
                        name = "宝虹广州项目部托运单";
                    }
                    else if (CommonClass.UserInfo.companyid == "486" && CommonClass.UserInfo.WebName == "宝虹长沙星沙营业部")
                    {
                        name = "宝虹长沙星沙营业部托运单";
                    }
                    else if (CommonClass.UserInfo.companyid == "486" && CommonClass.UserInfo.WebName == "武汉遵义营业部")
                    {
                        name = "武汉遵义营业部托运单";
                    }
                    else
                    {
                        name = CommonClass.UserInfo.BookNote;
                    }

                }

                frmRuiLangService.Print(name, SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTBIll_TX", new List<SqlPara> { new SqlPara("BillNo", billNo) })));
            }
        }

    

    }
}