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
using DevExpress.XtraGrid;
using Newtonsoft.Json;

namespace ZQTMS.UI
{
    public partial class frmWebApplyList : BaseForm
    {
        public frmWebApplyList()
        {
            InitializeComponent();
        }
        
        private void frmWebApplyList_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("网点异动确定");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;

            GridOper.CreateStyleFormatCondition(myGridView1, "ExcuteState", FormatConditionEnum.Equal, "1", Color.GreenYellow);//执行状态
            GridOper.CreateStyleFormatCondition(myGridView1, "LastState", FormatConditionEnum.Equal, "否决", Color.Red);//否决
            GridOper.CreateStyleFormatCondition(myGridView1, "LastState", FormatConditionEnum.Equal, "取消", Color.Red);//取消
            GridOper.CreateStyleFormatCondition(myGridView1, "LastState", FormatConditionEnum.Equal, "平台审核", Color.Yellow);//平台审核
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExecuteItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;
            string CurrWeb = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle ,"RecWeb").ToString();
            if (CurrWeb != CommonClass.UserInfo.WebName)
            {
                MsgBox.ShowOK("非当前接收网点不能执行！");
                return;
            }
            UpdateApplyState("执行");
        }
        /// <summary>
        /// 否决
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;
            string CurrWeb = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "RecWeb").ToString();
            if (CurrWeb != CommonClass.UserInfo.WebName)
            {
                MsgBox.ShowOK("非当前接收网点不能否决！");
                return;
            }
            UpdateApplyState("否决");
        }

        /// <summary>
        /// 取消申请
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ApplyCancelItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;
            string CurrApplyWeb = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "ApplyWeb").ToString();
            if (CurrApplyWeb != CommonClass.UserInfo.WebName)
            {
                MsgBox.ShowOK("非当前申请网点不能取消申请！");
                return;
            }

            DialogResult result = MsgBox.ShowYesNo("确定是否取消？");
            if (result == DialogResult.Yes)
                UpdateApplyState("取消");
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ApplyDate_begin", bdate.DateTime));
                list.Add(new SqlPara("ApplyDate_end",edate.DateTime));
                list.Add(new SqlPara("ApplyWeb", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("RecWeb",CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("ExcuteState", (executeState.Text == "全部" ? 99 : (executeState.Text == "已执行" ? 1 : 2))));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLAPPLY_AUTO",list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            } 
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UpdateApplyState(string Type)
        {
            try
            {
                if (myGridView1.FocusedRowHandle < 0) return;

                DataRow dr = myGridView1.GetDataRow(myGridView1.FocusedRowHandle);

                if (dr == null || dr["ApplyID"] == null)
                {
                    MsgBox.ShowOK("数据异常！");
                    return;
                }

                if (dr["ApplyType"].ToString() == "改单申请" && Type == "执行"
                    && (dr["ApplyContent"].ToString().Contains("现付")
                    || dr["ApplyContent"].ToString().Contains("提付")
                    || dr["ApplyContent"].ToString().Contains("月结")
                    || dr["ApplyContent"].ToString().Contains("短欠")
                    || dr["ApplyContent"].ToString().Contains("货到前付")))//
                {
                    int num = CommonClass.GetAduitState(dr["BillNo"].ToString());
                    if (num > 0)
                    {
                        MsgBox.ShowOK("运单已经审核或账款确认，不能执行！");
                        return;
                    }
                }

                string reson = "";
                if (Type != "取消")
                {
                    frmConfirmWithContent frm = new frmConfirmWithContent();
                    frm.Type = Type;
                    frm.txtApplyContent = GridOper.GetRowCellValueString(myGridView1, "ApplyContent");
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
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "[QSP_UpdateApplyState_Auto]", list);
                int result = SqlHelper.ExecteNonQuery(sps);
                if (result > 0)
                {
                    MsgBox.ShowOK("操作成功！");
                    List<SqlPara> listone = new List<SqlPara>();

                    if (Type == "取消")
                    {
                        myGridView1.SetRowCellValue(myGridView1.FocusedRowHandle, "LastState", "7");
                        myGridView1.SetRowCellValue(myGridView1.FocusedRowHandle, "CancelMan", CommonClass.UserInfo.UserName);
                        myGridView1.SetRowCellValue(myGridView1.FocusedRowHandle, "CancelDate", CommonClass.gcdate);

                        CommonSyn.FreightChangesCancelSyn(dr["ApplyID"].ToString()); //maohui20180514

                        #region //hj20190425 同步
                        listone.Add(new SqlPara("ApplyID", dr["ApplyID"].ToString()));
                        listone.Add(new SqlPara("Type", "取消"));
                        SqlParasEntity speone = new SqlParasEntity(OperType.Query, "QSP_GET_ZQTMSBILLAPPLYDYSYN", listone);
                        DataSet dsone = SqlHelper.GetDataSet(speone);
                        if (dsone != null && dsone.Tables.Count > 0 && dsone.Tables[0].Rows.Count > 0)
                        {
                            CommonSyn.BILLAPPLYYExSYN(dsone, "取消");
                        }
                        #endregion
                    }
                    if (Type == "执行")
                    {
                        myGridView1.SetRowCellValue(myGridView1.FocusedRowHandle, "ExcuteState", "1");
                        myGridView1.SetRowCellValue(myGridView1.FocusedRowHandle, "ExcuteMan", CommonClass.UserInfo.UserName);
                        myGridView1.SetRowCellValue(myGridView1.FocusedRowHandle, "ExcuteDate", CommonClass.gcdate);

                       #region ZQTMS数据同步 ZAJ 2017-1-18  //maohui20180514注释
                       // string billNo = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "BillNo").ToString();
                       // List<SqlPara> listQuery = new List<SqlPara>();
                       // listQuery.Add(new SqlPara("BillNo", billNo));
                       // SqlParasEntity spsQuery = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BYBILLNO", listQuery);
                       // DataSet dsQuery = SqlHelper.GetDataSet(spsQuery);
                       // if (dsQuery == null || dsQuery.Tables[0].Rows.Count == 0) return;
                       // decimal TransferFee = 0;//结算中转费
                       // decimal DeliveryFee = 0;//结算送货费
                       // decimal Tax_C = 0;//结算税金
                       // decimal TerminalOptFee = 0;//结算终端操作费
                       // decimal SupportValue_C = 0;//结算保价费
                       // decimal StorageFee_C = 0;//结算进仓费
                       // decimal NoticeFee_C = 0;//控货费
                       // decimal HandleFee_C = 0;//装卸费
                       // decimal UpstairFee_C = 0;//上楼费
                       // decimal ReceiptFee_C = 0;//回单费
                       // DataSet dsSend = new DataSet();
                       // DataTable dt = new DataTable();
                       // DataColumn dc1 = new DataColumn("ApplyID", typeof(string));
                       // DataColumn dc2 = new DataColumn("TransferFee", typeof(decimal));
                       // DataColumn dc3 = new DataColumn("DeliveryFee", typeof(decimal));
                       // DataColumn dc4 = new DataColumn("Tax_C", typeof(decimal));
                       // DataColumn dc5 = new DataColumn("TerminalOptFee", typeof(decimal));
                       // DataColumn dc6 = new DataColumn("SupportValue_C", typeof(decimal));
                       // DataColumn dc7 = new DataColumn("StorageFee_C", typeof(decimal));
                       // DataColumn dc8 = new DataColumn("NoticeFee_C", typeof(decimal));
                       // DataColumn dc9 = new DataColumn("HandleFee_C", typeof(decimal));
                       // DataColumn dc10 = new DataColumn("UpstairFee_C", typeof(decimal));
                       // DataColumn dc11 = new DataColumn("ReceiptFee_C", typeof(decimal));
                       // DataColumn dc12 = new DataColumn("Type", typeof(string));
                       // dt.Columns.Add(dc1);
                       // dt.Columns.Add(dc2);
                       // dt.Columns.Add(dc3);
                       // dt.Columns.Add(dc4);
                       // dt.Columns.Add(dc5);
                       // dt.Columns.Add(dc6);
                       // dt.Columns.Add(dc7);
                       // dt.Columns.Add(dc8);
                       // dt.Columns.Add(dc9);
                       // dt.Columns.Add(dc10);
                       // dt.Columns.Add(dc11);
                       // dt.Columns.Add(dc12);
                       // DataRow dr1 = dt.NewRow();
                       // dr1["ApplyID"] = dr["ApplyID"];
                       // dr1["TransferFee"] = TransferFee;
                       // dr1["DeliveryFee"] = DeliveryFee;
                       // dr1["Tax_C"] = Tax_C;
                       // dr1["TerminalOptFee"] = TerminalOptFee;
                       // dr1["SupportValue_C"] = SupportValue_C;
                       // dr1["StorageFee_C"] = StorageFee_C;
                       // dr1["NoticeFee_C"] = NoticeFee_C;
                       // dr1["HandleFee_C"] = HandleFee_C;
                       // dr1["UpstairFee_C"] = UpstairFee_C;
                       // dr1["ReceiptFee_C"] = ReceiptFee_C;
                       // dr1["Type"] = "运费异动";
                       // dt.Rows.Add(dr1);
                       // dsSend.Tables.Add(dt);
                       // string dsJson = JsonConvert.SerializeObject(dsSend);
                       // RequestModel<string> request = new RequestModel<string>();
                       // request.Request = dsJson;
                       // request.OperType = 0;
                       // string json = JsonConvert.SerializeObject(request);
                       // //http://192.168.16.112:99//KDLMSService/AllocateToArtery
                       // // ResponseModelClone<string> model = HttpHelper.HttpPost(json, "http://lms.dekuncn.com:7016/KDLMSService/AllocateToArtery");
                       //  //ResponseModelClone<string> model = HttpHelper.HttpPost(json, HttpHelper.urlWebApplySyn);
                       //ResponseModelClone<string> model = HttpHelper.HttpPost(json, HttpHelper.urlWebApplySyn);
                       // if(model.State!="200")
                       // {
                       //     MsgBox.ShowOK(model.Message);
                       // }
                        
                        #endregion

                        //CommonSyn.FreightChangesExcute(dr["ApplyID"].ToString()); //maohui20180514
                        #region //yzw 同步
                        listone.Add(new SqlPara("ApplyID", dr["ApplyID"].ToString()));
                        listone.Add(new SqlPara("Type", "执行"));
                        SqlParasEntity speone = new SqlParasEntity(OperType.Query, "QSP_GET_LMSBILLAPPLYDYSYN", listone);
                        DataSet dsone = SqlHelper.GetDataSet(speone);
                        if (dsone != null && dsone.Tables.Count > 0 && dsone.Tables[0].Rows.Count > 0)
                        {
                            CommonSyn.BILLAPPLYEXSYN(dsone, "执行");
                        }
                        #endregion

                    }
                    else if (Type == "否决")
                    {
                        myGridView1.SetRowCellValue(myGridView1.FocusedRowHandle, "VetoMan", CommonClass.UserInfo.UserName);
                        myGridView1.SetRowCellValue(myGridView1.FocusedRowHandle, "VetoDate", CommonClass.gcdate);

                        //CommonSyn.FreightChangesVeto(dr["ApplyID"].ToString());  //maohui20180514
                        #region //yzw 同步
                        listone.Add(new SqlPara("ApplyID", dr["ApplyID"].ToString()));
                        listone.Add(new SqlPara("Type", "否决"));
                        SqlParasEntity speone = new SqlParasEntity(OperType.Query, "QSP_GET_LMSBILLAPPLYDYSYN", listone);
                        DataSet dsone = SqlHelper.GetDataSet(speone);
                        if (dsone != null && dsone.Tables.Count > 0 && dsone.Tables[0].Rows.Count > 0)
                        {
                            CommonSyn.BILLAPPLYEXSYN(dsone, "否决");
                        }
                        #endregion

                    }

                    CommonClass.SetOperLog(dr["ApplyID"].ToString(), "", "", CommonClass.UserInfo.UserName, "改单审核", "改单审核" + Type + "操作");
                }
                else
                {
                    MsgBox.ShowOK("操作失败！");
                }
            }
            catch (Exception ex)
            {
                string errmsg = ex.Message.ToString();
                MsgBox.ShowOK(errmsg.Replace("数据库访问异常：", ""));
            }
        }
    }
}