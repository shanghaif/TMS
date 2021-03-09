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
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid;
using Newtonsoft.Json;

namespace ZQTMS.UI
{
    public partial class frmApplyList : BaseForm
    {
        public string ApplyType = "控货/放货";
        public string ReasonApproval = null;

        public frmApplyList()
        {
            InitializeComponent();
        }

        public frmApplyList(string atype)
        {
            InitializeComponent();
            ApplyType = atype;

            this.Text += "-->" + ApplyType;
        }

        private void frmApplyList_Load(object sender, EventArgs e)
        {
            if (ApplyType == "改单申请")
                CommonClass.InsertLog("改单审批");//xj/2019/5/29
            if (ApplyType == "控货/放货")
                CommonClass.InsertLog("放货控货审批");//xj/2019/5/29
            if (ApplyType == "转月结")
                CommonClass.InsertLog("转月结审批");//xj/2019/5/29
            if (ApplyType == "运费异动")
                CommonClass.InsertLog("运费异动审批");//xj/2019/5/29
            CommonClass.FormSet(this);
            GetRight(); 

            CommonClass.GetGridViewColumns(myGridView9);
            GridOper.SetGridViewProperty(myGridView9);
            BarMagagerOper.SetBarPropertity(bar3);
            GridOper.RestoreGridLayout(myGridView9);
            FixColumn fix = new FixColumn(myGridView9, barSubItem2);

            bdate.DateTime = CommonClass.gbdate.AddDays(-1);
            edate.DateTime = CommonClass.gedate;

            CommonClass.SetWeb(sqWeb, "全部");

            GridOper.CreateStyleFormatCondition(myGridView9, "AuditingState", FormatConditionEnum.Equal, "1", Color.FromArgb(192, 192, 255));//审核状态
            GridOper.CreateStyleFormatCondition(myGridView9, "ApprovalState", FormatConditionEnum.Equal, "1", Color.FromArgb(255, 255, 128));//审批状态
            GridOper.CreateStyleFormatCondition(myGridView9, "ExcuteState", FormatConditionEnum.Equal, "1", Color.Lime);//执行状态
            GridOper.CreateStyleFormatCondition(myGridView9, "LastState", FormatConditionEnum.Equal, "否决", Color.Red);//否决
            GridOper.CreateStyleFormatCondition(myGridView9, "LastState", FormatConditionEnum.Equal, "取消", Color.Red);//否决
        }

        private void GetRight()
        {
            string ins = this.GetType().FullName;
            if (ApplyType == "改单申请")
            {
                barButtonItem8.Enabled = UserRight.GetRight(ins + "#barButtonItem8", "46121211");
                barButtonItem11.Enabled = UserRight.GetRight(ins + "#barButtonItem11", "46121212");
                barButtonItem9.Enabled = UserRight.GetRight(ins + "#barButtonItem9", "46121213");
                barButtonItem10.Enabled = UserRight.GetRight(ins + "#barButtonItem10", "46121215");
                barButtonItem12.Enabled = UserRight.GetRight(ins + "#barButtonItem12", "46121216");
                barButtonItem13.Enabled = UserRight.GetRight(ins + "#barButtonItem13", "46121217");
                barButtonItem5.Enabled = UserRight.GetRight(ins + "#barButtonItem5", "46121218");
                barButtonItem15.Enabled = UserRight.GetRight(ins + "#barButtonItem15", "46121214");
            }
            if (ApplyType == "控货/放货")
            {
                barButtonItem8.Enabled = UserRight.GetRight(ins + "#barButtonItem8", "46121411");
                barButtonItem11.Enabled = UserRight.GetRight(ins + "#barButtonItem11", "46121412");
                barButtonItem9.Enabled = UserRight.GetRight(ins + "#barButtonItem9", "46121413");
                barButtonItem10.Enabled = UserRight.GetRight(ins + "#barButtonItem10", "46121415");
                barButtonItem12.Enabled = UserRight.GetRight(ins + "#barButtonItem12", "46121416");
                barButtonItem13.Enabled = UserRight.GetRight(ins + "#barButtonItem13", "46121417");
                barButtonItem5.Enabled = UserRight.GetRight(ins + "#barButtonItem5", "46121418");
                barButtonItem15.Enabled = UserRight.GetRight(ins + "#barButtonItem15", "46121414");
            }
            if (ApplyType == "转月结")
            {
                barButtonItem8.Enabled = UserRight.GetRight(ins + "#barButtonItem8", "46121611");
                barButtonItem11.Enabled = UserRight.GetRight(ins + "#barButtonItem11", "46121612");
                barButtonItem9.Enabled = UserRight.GetRight(ins + "#barButtonItem9", "46121613");
                barButtonItem10.Enabled = UserRight.GetRight(ins + "#barButtonItem10", "46121615");
                barButtonItem12.Enabled = UserRight.GetRight(ins + "#barButtonItem12", "46121616");
                barButtonItem13.Enabled = UserRight.GetRight(ins + "#barButtonItem13", "46121617");
                barButtonItem5.Enabled = UserRight.GetRight(ins + "#barButtonItem5", "46121618");
                barButtonItem15.Enabled = UserRight.GetRight(ins + "#barButtonItem15", "46121614");
            }
            if (ApplyType == "运费异动")
            {
                barButtonItem8.Enabled = UserRight.GetRight(ins + "#barButtonItem8", "46122111");
                barButtonItem11.Enabled = UserRight.GetRight(ins + "#barButtonItem11", "46122112");
                barButtonItem9.Enabled = UserRight.GetRight(ins + "#barButtonItem9", "46122113");
                barButtonItem10.Enabled = UserRight.GetRight(ins + "#barButtonItem10", "46122115");
                barButtonItem12.Enabled = UserRight.GetRight(ins + "#barButtonItem12", "46122116");
                barButtonItem13.Enabled = UserRight.GetRight(ins + "#barButtonItem13", "46122117");
                barButtonItem5.Enabled = UserRight.GetRight(ins + "#barButtonItem5", "46122118");
                barButtonItem15.Enabled = UserRight.GetRight(ins + "#barButtonItem15", "46122114");
            }
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            string a = comboBoxEdit1.Text;
            string b = comboBoxEdit2.Text;
            string c = comboBoxEdit3.Text;

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ApplyDate_begin", bdate.DateTime));
                list.Add(new SqlPara("ApplyDate_end", edate.DateTime));
                list.Add(new SqlPara("ApplyWeb", sqWeb.Text));
                list.Add(new SqlPara("ApplyType", ApplyType));
                list.Add(new SqlPara("AuditingState", (a == "全部" ? 99 : (a == "已审核" ? 1 : 2))));
                list.Add(new SqlPara("ApprovalState", (b == "全部" ? 99 : (b == "已审批" ? 1 : 2))));
                list.Add(new SqlPara("ExcuteState", (c == "全部" ? 99 : (c == "已执行" ? 1 : 2))));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "[QSP_GET_BILLAPPLY]", list);
                myGridControl8.DataSource = SqlHelper.GetDataTable(sps);
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.ToString());
            }
            finally
            {
                if (myGridView9.RowCount < 1000) myGridView9.BestFitColumns();
            }
        }




        /// <summary>
        /// 判断运单当前是否为转分拨运单
        /// </summary>
        /// <returns>true or false</returns>
        private bool CheckBillFB()
        {
            DataRow dr = myGridView9.GetDataRow(myGridView9.FocusedRowHandle);
            string billno = dr["BillNo"].ToString();
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillNo", billno));
            DataSet fbds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_billDepartureFBList_ByBillNo", list));
            if (fbds != null && fbds.Tables.Count > 0 && fbds.Tables[0].Rows.Count > 0) return true;
            return false;
        }
        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)//审核
        {
            UpdateApplyState("审核", true);
            //转分拨后运单取消自动审批功能 shbo2020-05-28
            if (CheckBillFB()) return;

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_COMPANY_KT"));
            if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
            {
                string isAutomaticGaiDanSP = ds.Tables[0].Rows[0]["IsAutomaticGaiDanSP"].ToString();
                string isAutomaticGaiDanZX = ds.Tables[0].Rows[0]["IsAutomaticGaiDanZX"].ToString();
                if (isAutomaticGaiDanSP == "是")
                {
                    UpdateApplyState("审批", false);
                    if (isAutomaticGaiDanZX == "是")
                    {
                        UpdateApplyState("执行", false);
                    }
                }
            }

        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)//审批
        {
            UpdateApplyState("审批", true);
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_COMPANY_KT"));
            if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
            {
                string isAutomaticGaiDanZX = ds.Tables[0].Rows[0]["IsAutomaticGaiDanZX"].ToString();
                if (isAutomaticGaiDanZX == "是")
                {
                    UpdateApplyState("执行", false);
                }
            }
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)//执行
        {
            if (CheckBillFB())
            {
                MsgBox.ShowError("运单已分拨，请在终端网点执行！");
                return;
            };
            UpdateApplyState("执行", true);
          
        }

        //private void UpdateApplyState(string Type, bool Sta)
        //{
        //    try
        //    {
        //        if (myGridView9.FocusedRowHandle < 0) return;

        //        DataRow dr = myGridView9.GetDataRow(myGridView9.FocusedRowHandle);

        //        if (dr == null || dr["ApplyID"] == null)
        //        {
        //            MsgBox.ShowOK("数据异常！");
        //            return;
        //        }

        //        if (dr["ApplyType"].ToString() == "改单申请" //&& Type == "执行" 
        //            && (dr["ApplyContent"].ToString().Contains("【现付】")
        //            || dr["ApplyContent"].ToString().Contains("【提付】")
        //            || dr["ApplyContent"].ToString().Contains("【月结】")
        //            || dr["ApplyContent"].ToString().Contains("短欠")
        //            || dr["ApplyContent"].ToString().Contains("【货到前付】")
        //            || dr["ApplyContent"].ToString().Contains("【欠付】")
        //            || dr["ApplyContent"].ToString().Contains("【回单付】")
        //            || dr["ApplyContent"].ToString().Contains("【代收货款】")
        //            || dr["ApplyContent"].ToString().Contains("【折扣折让】")))//
        //        {
        //            int num = CommonClass.GetAduitState(dr["BillNo"].ToString());
        //            if (num > 0)
        //            {
        //                MsgBox.ShowOK("运单已经审核或账款确认，不能执行！");
        //                return;
        //            }
        //            List<SqlPara> list1 = new List<SqlPara>();  //maohui20180510 查询运单审核状态
        //            list1.Add(new SqlPara("BillNO", dr["BillNo"].ToString()));
        //            SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_VerifState", list1);
        //            DataSet ds1 = SqlHelper.GetDataSet(sps1);
        //            if (ds1 == null || ds1.Tables.Count == 0)
        //            {
        //                MsgBox.ShowOK("查无此单，请检查!");
        //                return;
        //            }
        //            if (dr["ApplyContent"].ToString().Contains("【现付】"))
        //            {
        //                if (ds1.Tables[1].Rows[0]["NowPayState"].ToString() != "")
        //                {
        //                    if (Convert.ToInt32(ds1.Tables[1].Rows[0]["NowPayState"]) == 1)
        //                    {
        //                        MsgBox.ShowError("此单现付已核销，请先反核销再申请改单！");
        //                        return;
        //                    }
        //                }
        //            }
        //            if (dr["ApplyContent"].ToString().Contains("【提付】"))
        //            {
        //                if (ds1.Tables[1].Rows[0]["FetchPayState"].ToString() != "")
        //                {
        //                    if (Convert.ToInt32(ds1.Tables[1].Rows[0]["FetchPayState"]) == 1)
        //                    {
        //                        MsgBox.ShowError("此单提付已核销，请先反核销再申请改单！");
        //                        return;
        //                    }
        //                }
        //            }
        //            if (dr["ApplyContent"].ToString().Contains("【月结】"))
        //            {
        //                if (ds1.Tables[1].Rows[0]["MonthPayState"].ToString() != "")
        //                {
        //                    if (Convert.ToInt32(ds1.Tables[1].Rows[0]["MonthPayState"]) == 1)
        //                    {
        //                        MsgBox.ShowError("此单月结已核销，请先反核销再申请改单！");
        //                        return;
        //                    }
        //                }
        //            }
        //            if (dr["ApplyContent"].ToString().Contains("【欠付】"))
        //            {
        //                if (ds1.Tables[1].Rows[0]["OwePayVerifState"].ToString() != "")
        //                {
        //                    if (Convert.ToInt32(ds1.Tables[1].Rows[0]["OwePayVerifState"]) == 1)
        //                    {
        //                        MsgBox.ShowError("此单欠付已核销，请先反核销再申请改单！");
        //                        return;
        //                    }
        //                }
        //            }
        //            if (dr["ApplyContent"].ToString().Contains("【货到前付】"))
        //            {
        //                if (ds1.Tables[1].Rows[0]["BefArrivalPayVerifState"].ToString() != "")
        //                {
        //                    if (Convert.ToInt32(ds1.Tables[1].Rows[0]["BefArrivalPayVerifState"]) == 1)
        //                    {
        //                        MsgBox.ShowError("此单货到前付已核销，请先反核销再申请改单！");
        //                        return;
        //                    }
        //                }
        //            }
        //            if (dr["ApplyContent"].ToString().Contains("【回单付】"))
        //            {
        //                if (ds1.Tables[1].Rows[0]["ReceiptPayVerifState"].ToString() != "")
        //                {
        //                    if (Convert.ToInt32(ds1.Tables[1].Rows[0]["ReceiptPayVerifState"]) == 1)
        //                    {
        //                        MsgBox.ShowError("此单回单付已核销，请先反核销再申请改单！");
        //                        return;
        //                    }
        //                }
        //            }
        //            if (dr["ApplyContent"].ToString().Contains("【代收货款】"))
        //            {
        //                if (ds1.Tables[0].Rows[0]["CollectionPayBackState"].ToString() != "")
        //                {
        //                    if (Convert.ToInt32(ds1.Tables[0].Rows[0]["CollectionPayBackState"]) == 1)
        //                    {
        //                        MsgBox.ShowError("此单代收货款已核销，请先反核销再申请改单！");
        //                        return;
        //                    }
        //                }
        //            }
        //            if (dr["ApplyContent"].ToString().Contains("【折扣折让】"))
        //            {
        //                if (ds1.Tables[0].Rows[0]["DisTranVerifState"].ToString() != "")
        //                {
        //                    if (Convert.ToInt32(ds1.Tables[0].Rows[0]["DisTranVerifState"]) == 1)
        //                    {
        //                        MsgBox.ShowError("此单折扣转让已核销，请先反核销再申请改单！");
        //                        return;
        //                    }
        //                }
        //            }
        //        }

        //        //string reson = "";
        //        if (Type != "取消" && Sta)
        //        {
        //            frmConfirmWithTxt frm = new frmConfirmWithTxt();
        //            frm.Type = Type;
        //            frm.ApplyContent.Text = GridOper.GetRowCellValueString(myGridView9, "ApplyContent");
        //            frm.ShowDialog();
        //            if (frm.DialogResult != DialogResult.Yes)
        //            {
        //                return;
        //            }

        //            //reson = frm.Reson;
        //            ReasonApproval = frm.Reson;
        //        }
        //        List<SqlPara> list = new List<SqlPara>();
        //        list.Add(new SqlPara("ApplyID", dr["ApplyID"]));
        //        list.Add(new SqlPara("Type", Type));
        //        list.Add(new SqlPara("Man", CommonClass.UserInfo.UserName));
        //        list.Add(new SqlPara("Reson", ReasonApproval));
        //        //list.Add(new SqlPara("Reson", reson));
        //        SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "[QSP_UpdateApplyState]", list);
        //        int result = SqlHelper.ExecteNonQuery(sps);
        //        if (result > 0)
        //        {
        //            if (Sta) { MsgBox.ShowOK("操作成功！"); }
        //            //hj 同步
        //            List<SqlPara> listone = new List<SqlPara>();
        //            if (Type == "审核")
        //            {
        //                myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "AuditingState", "1");
        //                myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "AuditingMan", CommonClass.UserInfo.UserName);
        //                myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "AuditingDate", CommonClass.gcdate);

        //                #region //hj20180228 同步
        //                listone.Add(new SqlPara("ApplyID", dr["ApplyID"].ToString()));
        //                listone.Add(new SqlPara("Type", "审核"));
        //                SqlParasEntity speone = new SqlParasEntity(OperType.Query, "QSP_GET_ZQTMSBILLAPPLYSYN", listone);
        //                DataSet dsone = SqlHelper.GetDataSet(speone);
        //                if (dsone != null && dsone.Tables.Count > 0 && dsone.Tables[0].Rows.Count > 0)
        //                {
        //                    CommonSyn.BILLAPPLYSYN(dsone, "审核");
        //                }
        //                #endregion
        //            }
        //            else if (Type == "审批")
        //            {
        //                myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "ApprovalState", "1");
        //                myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "ApprovalMan", CommonClass.UserInfo.UserName);
        //                myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "ApprovalDate", CommonClass.gcdate);
        //                #region 审批后同步到ZQTMS一条记录,由ZQTMS执行 hj20180504
        //                //string billNo1 = myGridView9.GetRowCellValue(myGridView9.FocusedRowHandle, "BillNO").ToString();
        //                //CommonSyn.ReviewSyn(billNo1);
        //                #endregion

        //                #region hj20190228 同步
        //                listone.Add(new SqlPara("ApplyID", dr["ApplyID"].ToString()));
        //                listone.Add(new SqlPara("Type", "审批"));
        //                SqlParasEntity speone = new SqlParasEntity(OperType.Query, "QSP_GET_ZQTMSBILLAPPLYSYN", listone);
        //                DataSet dsone = SqlHelper.GetDataSet(speone);
        //                if (dsone != null && dsone.Tables.Count > 0 && dsone.Tables[0].Rows.Count > 0)
        //                {
        //                    CommonSyn.BILLAPPLYSYN(dsone, "审批");
        //                }
        //                #endregion
        //            }
        //            else if (Type == "执行")
        //            {
        //                myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "ExcuteState", "1");
        //                myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "ExcuteMan", CommonClass.UserInfo.UserName);
        //                myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "ExcuteDate", CommonClass.gcdate);
        //                #region ZQTMS数据同步 ZAJ 2017-1-18
        //                string billNo = myGridView9.GetRowCellValue(myGridView9.FocusedRowHandle, "BillNO").ToString();
        //                List<SqlPara> listQuery = new List<SqlPara>();
        //                listQuery.Add(new SqlPara("BillNo", billNo));
        //                SqlParasEntity spsQuery = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BYBILLNO", listQuery);
        //                DataSet dsQuery = SqlHelper.GetDataSet(spsQuery);
        //                //if (dsQuery == null || dsQuery.Tables[0].Rows.Count == 0 || dsQuery.Tables.Count==0) return;
        //                if (dsQuery == null || dsQuery.Tables.Count == 0 || dsQuery.Tables[0].Rows.Count == 0) return;
        //                decimal TransferFee = 0;//结算中转费
        //                decimal DeliveryFee = 0;//结算送货费
        //                decimal Tax_C = 0;//结算税金
        //                decimal TerminalOptFee = 0;//结算终端操作费
        //                decimal SupportValue_C = 0;//结算保价费
        //                decimal StorageFee_C = 0;//结算进仓费
        //                decimal NoticeFee_C = 0;//控货费
        //                decimal HandleFee_C = 0;//装卸费
        //                decimal UpstairFee_C = 0;//上楼费
        //                decimal ReceiptFee_C = 0;//回单费

        //                CalculateFee(dsQuery, out TransferFee, out DeliveryFee, out Tax_C, out TerminalOptFee, out SupportValue_C, out StorageFee_C, out NoticeFee_C,
        //                   out HandleFee_C, out UpstairFee_C, out ReceiptFee_C);
        //                DataSet dsSend = new DataSet();
        //                DataTable dt = new DataTable();
        //                DataColumn dc1 = new DataColumn("ApplyID", typeof(string));
        //                DataColumn dc2 = new DataColumn("TransferFee", typeof(decimal));
        //                DataColumn dc3 = new DataColumn("DeliveryFee", typeof(decimal));
        //                DataColumn dc4 = new DataColumn("Tax_C", typeof(decimal));
        //                DataColumn dc5 = new DataColumn("TerminalOptFee", typeof(decimal));
        //                DataColumn dc6 = new DataColumn("SupportValue_C", typeof(decimal));
        //                DataColumn dc7 = new DataColumn("StorageFee_C", typeof(decimal));
        //                DataColumn dc8 = new DataColumn("NoticeFee_C", typeof(decimal));
        //                DataColumn dc9 = new DataColumn("HandleFee_C", typeof(decimal));
        //                DataColumn dc10 = new DataColumn("UpstairFee_C", typeof(decimal));
        //                DataColumn dc11 = new DataColumn("ReceiptFee_C", typeof(decimal));
        //                DataColumn dc12 = new DataColumn("Type", typeof(string));
        //                dt.Columns.Add(dc1);
        //                dt.Columns.Add(dc2);
        //                dt.Columns.Add(dc3);
        //                dt.Columns.Add(dc4);
        //                dt.Columns.Add(dc5);
        //                dt.Columns.Add(dc6);
        //                dt.Columns.Add(dc7);
        //                dt.Columns.Add(dc8);
        //                dt.Columns.Add(dc9);
        //                dt.Columns.Add(dc10);
        //                dt.Columns.Add(dc11);
        //                dt.Columns.Add(dc12);
        //                DataRow dr1 = dt.NewRow();
        //                dr1["ApplyID"] = dr["ApplyID"];
        //                dr1["TransferFee"] = TransferFee;
        //                dr1["DeliveryFee"] = DeliveryFee;
        //                dr1["Tax_C"] = Tax_C;
        //                dr1["TerminalOptFee"] = TerminalOptFee;
        //                dr1["SupportValue_C"] = SupportValue_C;
        //                dr1["StorageFee_C"] = StorageFee_C;
        //                dr1["NoticeFee_C"] = NoticeFee_C;
        //                dr1["HandleFee_C"] = HandleFee_C;
        //                dr1["UpstairFee_C"] = UpstairFee_C;
        //                dr1["ReceiptFee_C"] = ReceiptFee_C;
        //                dr1["Type"] = "改单申请";
        //                dt.Rows.Add(dr1);
        //                dsSend.Tables.Add(dt);
        //                string dsJson = JsonConvert.SerializeObject(dsSend);
        //                RequestModel<string> request = new RequestModel<string>();
        //                request.Request = dsJson;
        //                request.OperType = 0;
        //                string json = JsonConvert.SerializeObject(request);
        //                //http://192.168.16.112:99//KDLMSService/AllocateToArtery
        //                // ResponseModelClone<string> model = HttpHelper.HttpPost(json, "http://lms.dekuncn.com:7016/KDLMSService/AllocateToArtery");
        //                ResponseModelClone<string> model = HttpHelper.HttpPost(json, HttpHelper.urlWebApplySyn);
        //                // ResponseModelClone<string> model = HttpHelper.HttpPost(json, "http://localhost:42936/KDLMSService/LmsUpdateWayBill");
        //                if (model.State != "200")
        //                {
        //                    MsgBox.ShowOK(model.Message);
        //                }


        //                #endregion
        //                //xj改单执行同步
        //                CommonSyn.BillApplyExcute(dr["ApplyID"].ToString());
        //            }
        //            else if (Type == "否决")
        //            {
        //                myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "VetoMan", CommonClass.UserInfo.UserName);
        //                myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "VetoDate", CommonClass.gcdate);

        //                #region hj20190228 同步
        //                listone.Add(new SqlPara("ApplyID", dr["ApplyID"].ToString()));
        //                listone.Add(new SqlPara("Type", "否决"));
        //                SqlParasEntity speone = new SqlParasEntity(OperType.Query, "QSP_GET_ZQTMSBILLAPPLYSYN", listone);
        //                DataSet dsone = SqlHelper.GetDataSet(speone);
        //                if (dsone != null && dsone.Tables.Count > 0 && dsone.Tables[0].Rows.Count > 0)
        //                {
        //                    CommonSyn.BILLAPPLYSYN(dsone, "否决");
        //                }
        //                #endregion
        //            }
        //            CommonClass.SetOperLog(dr["ApplyID"].ToString(), "", "", CommonClass.UserInfo.UserName, "改单审核", "改单审核" + Type + "操作");

        //        }
        //        else
        //        {
        //            MsgBox.ShowOK("操作成失败！");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string errmsg = ex.Message.ToString();
        //        MsgBox.ShowOK(errmsg.Replace("数据库访问异常：", ""));
        //    }
        //}


        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)//取消
        {
            DialogResult result = MsgBox.ShowYesNo("确定是否取消？");
            if (result == DialogResult.Yes)
                UpdateApplyState("取消");
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)//否决
        {
            UpdateApplyState("否决");
        }

        private void UpdateApplyState(string Type)
        {
            try
            {
                if (myGridView9.FocusedRowHandle < 0) return;

                DataRow dr = myGridView9.GetDataRow(myGridView9.FocusedRowHandle);

                if (dr == null || dr["ApplyID"] == null)
                {
                    MsgBox.ShowOK("数据异常！");
                    return;
                }

                if (dr["ApplyType"].ToString() == "改单申请" //&& Type == "执行" 
                    && (dr["ApplyContent"].ToString().Contains("【现付】")
                    || dr["ApplyContent"].ToString().Contains("【提付】")
                    || dr["ApplyContent"].ToString().Contains("【月结】")
                    || dr["ApplyContent"].ToString().Contains("短欠")
                    || dr["ApplyContent"].ToString().Contains("【货到前付】")
                    || dr["ApplyContent"].ToString().Contains("【欠付】")
                    || dr["ApplyContent"].ToString().Contains("【回单付】")
                    || dr["ApplyContent"].ToString().Contains("【代收货款】")
                    || dr["ApplyContent"].ToString().Contains("【折扣折让】")))//
                {
                    int num = CommonClass.GetAduitState(dr["BillNo"].ToString());
                    if (num > 0)
                    {
                        MsgBox.ShowOK("运单已经审核或账款确认，不能执行！");
                        return;
                    }
                    List<SqlPara> list1 = new List<SqlPara>();  //maohui20180510 查询运单审核状态
                    list1.Add(new SqlPara("BillNO", dr["BillNo"].ToString()));
                    SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_VerifState", list1);
                    DataSet ds1 = SqlHelper.GetDataSet(sps1);
                    if (ds1 == null || ds1.Tables.Count == 0)
                    {
                        MsgBox.ShowOK("查无此单，请检查!");
                        return;
                    }
                    if (dr["ApplyContent"].ToString().Contains("【现付】"))
                    {
                        if (ds1.Tables[1].Rows[0]["NowPayState"].ToString() != "")
                        {
                            if (Convert.ToInt32(ds1.Tables[1].Rows[0]["NowPayState"]) == 1)
                            {
                                MsgBox.ShowError("此单现付已核销，请先反核销再申请改单！");
                                return;
                            }
                        }
                    }
                    if (dr["ApplyContent"].ToString().Contains("【提付】"))
                    {
                        if(ds1.Tables[1].Rows[0]["FetchPayState"].ToString() != "")
                        {
                            if (Convert.ToInt32(ds1.Tables[1].Rows[0]["FetchPayState"]) == 1)
                            {
                                MsgBox.ShowError("此单提付已核销，请先反核销再申请改单！");
                                return;
                            }
                        }
                    }
                    if (dr["ApplyContent"].ToString().Contains("【月结】"))
                    {
                        if (ds1.Tables[1].Rows[0]["MonthPayState"].ToString() != "")
                        {
                            if (Convert.ToInt32(ds1.Tables[1].Rows[0]["MonthPayState"]) == 1)
                            {
                                MsgBox.ShowError("此单月结已核销，请先反核销再申请改单！");
                                return;
                            }
                        }
                    }
                    if (dr["ApplyContent"].ToString().Contains("【欠付】"))
                    {
                        if (ds1.Tables[1].Rows[0]["OwePayVerifState"].ToString() != "")
                        {
                            if (Convert.ToInt32(ds1.Tables[1].Rows[0]["OwePayVerifState"]) == 1)
                            {
                                MsgBox.ShowError("此单欠付已核销，请先反核销再申请改单！");
                                return;
                            }
                        }
                    }
                    if (dr["ApplyContent"].ToString().Contains("【货到前付】"))
                    {
                        if (ds1.Tables[1].Rows[0]["BefArrivalPayVerifState"].ToString() != "")
                        {
                            if (Convert.ToInt32(ds1.Tables[1].Rows[0]["BefArrivalPayVerifState"]) == 1)
                            {
                                MsgBox.ShowError("此单货到前付已核销，请先反核销再申请改单！");
                                return;
                            }
                        }
                    }
                    if (dr["ApplyContent"].ToString().Contains("【回单付】"))
                    {
                        if (ds1.Tables[1].Rows[0]["ReceiptPayVerifState"].ToString() != "")
                        {
                            if (Convert.ToInt32(ds1.Tables[1].Rows[0]["ReceiptPayVerifState"]) == 1)
                            {
                                MsgBox.ShowError("此单回单付已核销，请先反核销再申请改单！");
                                return;
                            }
                        }
                    }
                    if (dr["ApplyContent"].ToString().Contains("【代收货款】"))
                    {
                        if (ds1.Tables[0].Rows[0]["CollectionPayBackState"].ToString() != "")
                        {
                            if (Convert.ToInt32(ds1.Tables[0].Rows[0]["CollectionPayBackState"]) == 1)
                            {
                                MsgBox.ShowError("此单代收货款已核销，请先反核销再申请改单！");
                                return;
                            }
                        }
                    }
                    if (dr["ApplyContent"].ToString().Contains("【折扣折让】"))
                    {
                        if (ds1.Tables[0].Rows[0]["DisTranVerifState"].ToString() != "")
                        {
                            if (Convert.ToInt32(ds1.Tables[0].Rows[0]["DisTranVerifState"]) ==  1)
                            {
                                MsgBox.ShowError("此单折扣转让已核销，请先反核销再申请改单！");
                                return;
                            }
                        }
                    }
                }

                string reson = "";
                if (Type != "取消")
                {
                    frmConfirmWithTxt frm = new frmConfirmWithTxt();
                    frm.Type = Type;
                    frm.ApplyContent.Text = GridOper.GetRowCellValueString(myGridView9, "ApplyContent");
                    frm.ShowDialog();
                    if (frm.DialogResult != DialogResult.Yes)
                    {
                        return;
                    }

                    reson = frm.Reson;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ApplyID", dr["ApplyID"]));
                list.Add(new SqlPara("Type", Type)); 
                list.Add(new SqlPara("Man", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("Reson", reson));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "[QSP_UpdateApplyState]", list);
                int result = SqlHelper.ExecteNonQuery(sps);
                if (result > 0)
                {
                    MsgBox.ShowOK("操作成功！");
                    List<SqlPara> listone = new List<SqlPara>(); //hj20190425
                    if (Type == "审核")
                    {
                        myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "AuditingState", "1");
                        myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "AuditingMan", CommonClass.UserInfo.UserName);
                        myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "AuditingDate", CommonClass.gcdate);

                        #region //hj20190425 同步
                        listone.Add(new SqlPara("ApplyID", dr["ApplyID"].ToString()));
                        listone.Add(new SqlPara("Type", "审核"));
                        SqlParasEntity speone = new SqlParasEntity(OperType.Query, "QSP_GET_ZQTMSBILLAPPLYSYN", listone);
                        DataSet dsone = SqlHelper.GetDataSet(speone);
                        if (dsone != null && dsone.Tables.Count > 0 && dsone.Tables[0].Rows.Count > 0)
                        {
                            CommonSyn.BILLAPPLYSYN(dsone, "审核");
                        }
                        #endregion
                    }
                    else if (Type == "审批")
                    {
                        myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "ApprovalState", "1");
                        myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "ApprovalMan", CommonClass.UserInfo.UserName);
                        myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "ApprovalDate", CommonClass.gcdate);
                        #region hj20190425 同步
                        listone.Add(new SqlPara("ApplyID", dr["ApplyID"].ToString()));
                        listone.Add(new SqlPara("Type", "审批"));
                        SqlParasEntity speone = new SqlParasEntity(OperType.Query, "QSP_GET_ZQTMSBILLAPPLYSYN", listone);
                        DataSet dsone = SqlHelper.GetDataSet(speone);
                        if (dsone != null && dsone.Tables.Count > 0 && dsone.Tables[0].Rows.Count > 0)
                        {
                            CommonSyn.BILLAPPLYSYN(dsone, "审批");
                        }
                        #endregion
                    }
                    else if (Type == "执行")
                    {
                        myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "ExcuteState", "1");
                        myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "ExcuteMan", CommonClass.UserInfo.UserName);
                        myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "ExcuteDate", CommonClass.gcdate);
                        //yzw 执行同步
                        CommonSyn.BillApplyExcute(dr["ApplyID"].ToString());
                    }
                    else if (Type == "否决")
                    {
                        myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "VetoMan", CommonClass.UserInfo.UserName);
                        myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "VetoDate", CommonClass.gcdate);
                    }
                    else if (Type == "取消")   //whf20190805
                    {
                        myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "LastState", "7");
                        myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "CancelMan", CommonClass.UserInfo.UserName);
                        myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "CancelDate", CommonClass.gcdate);
                       
                        listone.Add(new SqlPara("ApplyID", dr["ApplyID"].ToString()));
                        listone.Add(new SqlPara("Type", "取消"));
                        SqlParasEntity speone = new SqlParasEntity(OperType.Query, "QSP_GET_ZQTMSBILLAPPLYSYN", listone);
                        DataSet dsone = SqlHelper.GetDataSet(speone);
                        if (dsone != null && dsone.Tables.Count > 0 && dsone.Tables[0].Rows.Count > 0)
                        {
                            CommonSyn.BILLAPPLYSYN(dsone, "取消");
                        }

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
         private void UpdateApplyState(string Type, bool Sta)
        {
            try
            {
                if (myGridView9.FocusedRowHandle < 0) return;

                DataRow dr = myGridView9.GetDataRow(myGridView9.FocusedRowHandle);

                if (dr == null || dr["ApplyID"] == null)
                {
                    MsgBox.ShowOK("数据异常！");
                    return;
                }

                if (dr["ApplyType"].ToString() == "改单申请" //&& Type == "执行" 
                    && (dr["ApplyContent"].ToString().Contains("【现付】")
                    || dr["ApplyContent"].ToString().Contains("【提付】")
                    || dr["ApplyContent"].ToString().Contains("【月结】")
                    || dr["ApplyContent"].ToString().Contains("短欠")
                    || dr["ApplyContent"].ToString().Contains("【货到前付】")
                    || dr["ApplyContent"].ToString().Contains("【欠付】")
                    || dr["ApplyContent"].ToString().Contains("【回单付】")
                    || dr["ApplyContent"].ToString().Contains("【代收货款】")
                    || dr["ApplyContent"].ToString().Contains("【折扣折让】")))//
                {
                    int num = CommonClass.GetAduitState(dr["BillNo"].ToString());
                    if (num > 0)
                    {
                        MsgBox.ShowOK("运单已经审核或账款确认，不能执行！");
                        return;
                    }
                    List<SqlPara> list1 = new List<SqlPara>();  //maohui20180510 查询运单审核状态
                    list1.Add(new SqlPara("BillNO", dr["BillNo"].ToString()));
                    SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_VerifState", list1);
                    DataSet ds1 = SqlHelper.GetDataSet(sps1);
                    if (ds1 == null || ds1.Tables.Count == 0)
                    {
                        MsgBox.ShowOK("查无此单，请检查!");
                        return;
                    }
                    if (dr["ApplyContent"].ToString().Contains("【现付】"))
                    {
                        if (ds1.Tables[1].Rows[0]["NowPayState"].ToString() != "")
                        {
                            if (Convert.ToInt32(ds1.Tables[1].Rows[0]["NowPayState"]) == 1)
                            {
                                MsgBox.ShowError("此单现付已核销，请先反核销再申请改单！");
                                return;
                            }
                        }
                    }
                    if (dr["ApplyContent"].ToString().Contains("【提付】"))
                    {
                        if(ds1.Tables[1].Rows[0]["FetchPayState"].ToString() != "")
                        {
                            if (Convert.ToInt32(ds1.Tables[1].Rows[0]["FetchPayState"]) == 1)
                            {
                                MsgBox.ShowError("此单提付已核销，请先反核销再申请改单！");
                                return;
                            }
                        }
                    }
                    if (dr["ApplyContent"].ToString().Contains("【月结】"))
                    {
                        if (ds1.Tables[1].Rows[0]["MonthPayState"].ToString() != "")
                        {
                            if (Convert.ToInt32(ds1.Tables[1].Rows[0]["MonthPayState"]) == 1)
                            {
                                MsgBox.ShowError("此单月结已核销，请先反核销再申请改单！");
                                return;
                            }
                        }
                    }
                    if (dr["ApplyContent"].ToString().Contains("【欠付】"))
                    {
                        if (ds1.Tables[1].Rows[0]["OwePayVerifState"].ToString() != "")
                        {
                            if (Convert.ToInt32(ds1.Tables[1].Rows[0]["OwePayVerifState"]) == 1)
                            {
                                MsgBox.ShowError("此单欠付已核销，请先反核销再申请改单！");
                                return;
                            }
                        }
                    }
                    if (dr["ApplyContent"].ToString().Contains("【货到前付】"))
                    {
                        if (ds1.Tables[1].Rows[0]["BefArrivalPayVerifState"].ToString() != "")
                        {
                            if (Convert.ToInt32(ds1.Tables[1].Rows[0]["BefArrivalPayVerifState"]) == 1)
                            {
                                MsgBox.ShowError("此单货到前付已核销，请先反核销再申请改单！");
                                return;
                            }
                        }
                    }
                    if (dr["ApplyContent"].ToString().Contains("【回单付】"))
                    {
                        if (ds1.Tables[1].Rows[0]["ReceiptPayVerifState"].ToString() != "")
                        {
                            if (Convert.ToInt32(ds1.Tables[1].Rows[0]["ReceiptPayVerifState"]) == 1)
                            {
                                MsgBox.ShowError("此单回单付已核销，请先反核销再申请改单！");
                                return;
                            }
                        }
                    }
                    if (dr["ApplyContent"].ToString().Contains("【代收货款】"))
                    {
                        if (ds1.Tables[0].Rows[0]["CollectionPayBackState"].ToString() != "")
                        {
                            if (Convert.ToInt32(ds1.Tables[0].Rows[0]["CollectionPayBackState"]) == 1)
                            {
                                MsgBox.ShowError("此单代收货款已核销，请先反核销再申请改单！");
                                return;
                            }
                        }
                    }
                    if (dr["ApplyContent"].ToString().Contains("【折扣折让】"))
                    {
                        if (ds1.Tables[0].Rows[0]["DisTranVerifState"].ToString() != "")
                        {
                            if (Convert.ToInt32(ds1.Tables[0].Rows[0]["DisTranVerifState"]) ==  1)
                            {
                                MsgBox.ShowError("此单折扣转让已核销，请先反核销再申请改单！");
                                return;
                            }
                        }
                    }
                }

               // string reson = "";
                if (Type != "取消" && Sta)
                {
                    frmConfirmWithTxt frm = new frmConfirmWithTxt();
                    frm.Type = Type;
                    frm.ApplyContent.Text = GridOper.GetRowCellValueString(myGridView9, "ApplyContent");
                    frm.ShowDialog();
                    if (frm.DialogResult != DialogResult.Yes)
                    {
                        return;
                    }

                    //reson = frm.Reson;
                    ReasonApproval = frm.Reson;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ApplyID", dr["ApplyID"]));
                list.Add(new SqlPara("Type", Type));
                list.Add(new SqlPara("Man", CommonClass.UserInfo.UserName));
                //list.Add(new SqlPara("Reson", reson));
                list.Add(new SqlPara("Reson", ReasonApproval));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "[QSP_UpdateApplyState]", list);
                int result = SqlHelper.ExecteNonQuery(sps);
                if (result > 0)
                {
                    if (Sta) { MsgBox.ShowOK("操作成功！"); }
                    List<SqlPara> listone = new List<SqlPara>(); //hj20190425
                    if (Type == "审核")
                    {
                        myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "AuditingState", "1");
                        myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "AuditingMan", CommonClass.UserInfo.UserName);
                        myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "AuditingDate", CommonClass.gcdate);

                        #region //hj20190425 同步
                        listone.Add(new SqlPara("ApplyID", dr["ApplyID"].ToString()));
                        listone.Add(new SqlPara("Type", "审核"));
                        SqlParasEntity speone = new SqlParasEntity(OperType.Query, "QSP_GET_ZQTMSBILLAPPLYSYN", listone);
                        DataSet dsone = SqlHelper.GetDataSet(speone);
                        if (dsone != null && dsone.Tables.Count > 0 && dsone.Tables[0].Rows.Count > 0)
                        {
                            CommonSyn.BILLAPPLYSYN(dsone, "审核");
                        }
                        #endregion
                    }
                    else if (Type == "审批")
                    {
                        myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "ApprovalState", "1");
                        myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "ApprovalMan", CommonClass.UserInfo.UserName);
                        myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "ApprovalDate", CommonClass.gcdate);
                        #region hj20190425 同步
                        listone.Add(new SqlPara("ApplyID", dr["ApplyID"].ToString()));
                        listone.Add(new SqlPara("Type", "审批"));
                        SqlParasEntity speone = new SqlParasEntity(OperType.Query, "QSP_GET_ZQTMSBILLAPPLYSYN", listone);
                        DataSet dsone = SqlHelper.GetDataSet(speone);
                        if (dsone != null && dsone.Tables.Count > 0 && dsone.Tables[0].Rows.Count > 0)
                        {
                            CommonSyn.BILLAPPLYSYN(dsone, "审批");
                        }
                        #endregion
                    }
                    else if (Type == "执行")
                    {
                        myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "ExcuteState", "1");
                        myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "ExcuteMan", CommonClass.UserInfo.UserName);
                        myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "ExcuteDate", CommonClass.gcdate);
                        //yzw 执行同步
                        CommonSyn.BillApplyExcute(dr["ApplyID"].ToString());
                    }
                    else if (Type == "否决")
                    {
                        myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "VetoMan", CommonClass.UserInfo.UserName);
                        myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "VetoDate", CommonClass.gcdate);
                    }
                    else if (Type == "取消")   //whf20190805
                    {
                        myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "LastState", "7");
                        myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "CancelMan", CommonClass.UserInfo.UserName);
                        myGridView9.SetRowCellValue(myGridView9.FocusedRowHandle, "CancelDate", CommonClass.gcdate);
                       
                        listone.Add(new SqlPara("ApplyID", dr["ApplyID"].ToString()));
                        listone.Add(new SqlPara("Type", "取消"));
                        SqlParasEntity speone = new SqlParasEntity(OperType.Query, "QSP_GET_ZQTMSBILLAPPLYSYN", listone);
                        DataSet dsone = SqlHelper.GetDataSet(speone);
                        if (dsone != null && dsone.Tables.Count > 0 && dsone.Tables[0].Rows.Count > 0)
                        {
                            CommonSyn.BILLAPPLYSYN(dsone, "取消");
                        }

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

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView9, "改单记录");
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView9);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView9);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView9.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView9);
        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            UpdateApplyState("总部代执行");
        }
    }
}