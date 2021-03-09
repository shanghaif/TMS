using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using System.Collections;
using System.Data.OleDb;
using System.IO;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.Controls;
using System.Threading;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using Newtonsoft.Json;
using DevExpress.XtraGrid;
using ZQTMS.UI.其他费用;

namespace ZQTMS.UI
{
    public partial class frmSendLoad_GX : BaseForm
    {
        private DataSet dataset1 = new DataSet();
        private DataSet dataset3 = new DataSet();
        GridHitInfo hitInfo = null;
        static frmSendLoad_GX fsl;
        string[] NotStreet = null;
        public decimal costrate = 0;

        /// <summary>
        /// 获取窗体对像
        /// </summary>
        public static frmSendLoad_GX Get_frmSendLoad { get { if (fsl == null || fsl.IsDisposed) fsl = new frmSendLoad_GX(); return fsl; } }

        public frmSendLoad_GX()
        {
            InitializeComponent();
            barEditItem1.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem1_EditValueChanging);
            barEditItem1.Edit.KeyDown += new KeyEventHandler(barEditItem1_KeyDown);
            barEditItem2.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem2_EditValueChanging);
            barEditItem2.Edit.KeyDown += new KeyEventHandler(barEditItem2_KeyDown);
        }

        private void getdata()
        {
            if (dataset1 != null)
            {
                dataset1.Tables.Clear();
                dataset3.Tables.Clear();
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SEND_LOAD", list);
                dataset1 = SqlHelper.GetDataSet(sps);
                if (dataset1 == null || dataset1.Tables.Count == 0) return;

                dataset3 = dataset1.Clone();
                myGridControl1.DataSource = dataset1.Tables[0];
                myGridControl2.DataSource = dataset3.Tables[0];


            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                if (myGridView1.RowCount < 1000) myGridView1.BestFitColumns();
            }
        }

        private void myGridControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GridViewMove.Move(myGridView1, dataset1, dataset3);
        }

        private void myGridControl2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GridViewMove.Move(myGridView2, dataset3, dataset1);
        }

        private void myGridControl2_DragDrop(object sender, DragEventArgs e)
        {
            GridViewMove.Move(myGridView1, dataset1, dataset3);
        }

        private void myGridControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (hitInfo == null) return;
            Rectangle dragRect = new Rectangle(new Point(
            hitInfo.HitPoint.X - SystemInformation.DragSize.Width / 2,
           hitInfo.HitPoint.Y - SystemInformation.DragSize.Height / 2), SystemInformation.DragSize);
            if (!dragRect.Contains(new Point(e.X, e.Y)))
            {
                if (hitInfo.InRowCell)
                    myGridControl1.DoDragDrop("我要过去了....", DragDropEffects.All);
            }
        }

        private void myGridControl2_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void myGridControl1_MouseDown(object sender, MouseEventArgs e)
        {
            hitInfo = myGridView1.CalcHitInfo(new Point(e.X, e.Y));
        }

        private void myGridControl2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (hitInfo == null) return;
            Rectangle dragRect = new Rectangle(new Point(
            hitInfo.HitPoint.X - SystemInformation.DragSize.Width / 2,
            hitInfo.HitPoint.Y - SystemInformation.DragSize.Height / 2), SystemInformation.DragSize);
            if (!dragRect.Contains(new Point(e.X, e.Y)))
            {
                if (hitInfo.InRowCell)
                    myGridControl2.DoDragDrop("我要过去了....", DragDropEffects.All);
            }
        }

        private void myGridControl2_MouseDown(object sender, MouseEventArgs e)
        {
            hitInfo = myGridView2.CalcHitInfo(new Point(e.X, e.Y));
        }

        private void myGridControl1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void myGridControl3_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void myGridControl1_DragDrop(object sender, DragEventArgs e)
        {
            GridViewMove.Move(myGridView2, dataset3, dataset1);
        }

        private void senddirect()
        {
            string sendcarno = SendCarNO.Text.Trim();
            string sendbatch = SendBatch.Text.Trim();
            if ((sendcarno == "") || (sendbatch == ""))
            {
                XtraMessageBox.Show("车号、发车批次、必须填写。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            bool flag = false;
            if (NotStreet == null)
            {
                flag = false;
            }
            else
            {
                flag = true;
            }
            //for (int i = 0; i < myGridView2.RowCount; i++)
            //{
            //    hj20180720 无锡放开第四级地址的必填限制
            //    if (flag)
            //    {
            //        if (GridOper.GetRowCellValueString(myGridView2, i, "SendToProvince") == "" || GridOper.GetRowCellValueString(myGridView2, i, "SendToCity") == "" || GridOper.GetRowCellValueString(myGridView2, i, "SendToArea") == "" )
            //        {
            //            MsgBox.ShowOK("请填写第" + (i + 1) + "行送货地址!");
            //            return;
            //        }
            //    }
            //    else
            //    {
            //        if (GridOper.GetRowCellValueString(myGridView2, i, "SendToProvince") == "" || GridOper.GetRowCellValueString(myGridView2, i, "SendToCity") == "" || GridOper.GetRowCellValueString(myGridView2, i, "SendToArea") == "" || GridOper.GetRowCellValueString(myGridView2, i, "SendToStreet") == "")
            //        {
            //            MsgBox.ShowOK("请填写第" + (i + 1) + "行送货地址!");
            //            return;
            //        }
            //    }
            //}

            string billnos = "", sendpcss = "", accsends = "", IdStr = "", SendToProvinceStr = "", SendToCityStr = "", SendToAreaStr = "", SendToStreet = "", DeliveryFee_C = "";
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                billnos += GridOper.GetRowCellValueString(myGridView2, i, "BillNo") + ",";
                sendpcss += ConvertType.ToInt32(myGridView2.GetRowCellValue(i, "sendqty")) + ",";
                accsends += ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "AccSend")) + ",";
                IdStr += ConvertType.ToString(myGridView2.GetRowCellValue(i, "Id")) + ",";

                SendToProvinceStr += GridOper.GetRowCellValueString(myGridView2, i, "SendToProvince") + ",";
                SendToCityStr += GridOper.GetRowCellValueString(myGridView2, i, "SendToCity") + ",";
                SendToAreaStr += GridOper.GetRowCellValueString(myGridView2, i, "SendToArea") + ",";
                SendToStreet += GridOper.GetRowCellValueString(myGridView2, i, "SendToStreet") + ",";
                DeliveryFee_C += ConvertType.ToDecimal(GridOper.GetRowCellValueString(myGridView2, i, "DeliveryFee_C")) + ",";
            }
            if (billnos == "") return;
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("SendCarNo", sendcarno));
            list.Add(new SqlPara("SendBatch", sendbatch));
            list.Add(new SqlPara("SendDate", SendDate.DateTime));
            list.Add(new SqlPara("SendOperator", CommonClass.UserInfo.UserName));
            list.Add(new SqlPara("SendDriver", SendDriver.Text.Trim()));
            list.Add(new SqlPara("SendDesc", SendDesc.Text.Trim()));
            list.Add(new SqlPara("SendDriverPhone", SendDriverPhone.Text.Trim()));
            list.Add(new SqlPara("SendWeb", CommonClass.UserInfo.WebName));
            list.Add(new SqlPara("SendSite", CommonClass.UserInfo.SiteName));
            list.Add(new SqlPara("billnos", billnos));
            list.Add(new SqlPara("AccSends", accsends));
            list.Add(new SqlPara("SendPCSs", sendpcss));
            list.Add(new SqlPara("IdStr", IdStr));
            list.Add(new SqlPara("SendToProvinceStr", SendToProvinceStr));
            list.Add(new SqlPara("SendToCityStr", SendToCityStr));
            list.Add(new SqlPara("SendToAreaStr", SendToAreaStr));
            list.Add(new SqlPara("SendToStreetStr", SendToStreet));
            list.Add(new SqlPara("DeliveryFee_CStr", DeliveryFee_C));

            int isSendWbps = 0;
            if (ckbPushApp.Checked)
            {
                isSendWbps = 1;
            }
            list.Add(new SqlPara("IsSendWbps", isSendWbps));

            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_SEND_3", list)) == 0) return;
            dataset3.Tables[0].Rows.Clear();
            MsgBox.ShowOK();

            //打印送货单
            if (checkEdit3.Checked)
            {
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_SEND_DETAIL_PRINT", new List<SqlPara> { new SqlPara("SendBatch", sendbatch) }));
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    row["NowCompany"] = CommonClass.UserInfo.gsqc;
                }

                string shqd = "送货清单";
                if (File.Exists(Application.StartupPath + "\\Reports\\" + "送货清单" + "per.grf"))
                {
                    shqd = shqd + "per";
                }
                //frmRuiLangService.Print("送货清单.grf", ds, CommonClass.UserInfo.gsqc);
                frmPrintRuiLang fpr = new frmPrintRuiLang(shqd, ds);
                fpr.ShowDialog();
            }

            Clear();
            //List<SqlPara> listQuery = new List<SqlPara>();
            //listQuery.Add(new SqlPara("BillNOStr", billnos));
            //SqlParasEntity spsQuery = new SqlParasEntity(OperType.Query, "QSP_GET_BillSendGoodSynInfo", listQuery);
            //DataSet dsQuery = SqlHelper.GetDataSet(spsQuery);
            //if (dsQuery == null || dsQuery.Tables[0].Rows.Count == 0) return;
            //string dsJson = JsonConvert.SerializeObject(dsQuery);
            //RequestModel<string> request = new RequestModel<string>();
            //request.Request = dsJson;
            //request.OperType = 0;
            //string json = JsonConvert.SerializeObject(request);
            //string url = "http://localhost:42936/KDLMSService/ZQTMSBillSendGoodsSyn";
            ////string url = "http://lms.dekuncn.com:7016/KDLMSService/ZQTMSBillSendGoodsSyn";
            //ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
            //if (model.State != "200")
            //{
            //    MsgBox.ShowOK(model.Message);
            //}
            //BillSendGoodsSyn(billnos);
            //CommonSyn.BillSendGoodsSyn(billnos, sendbatch);//zaj 分拨同步 2018-4-10
            CommonSyn.USP_ADD_SEND_3_SYN(billnos, sendcarno, SendDate.DateTime);//yzw 
            string timeBillNos = billnos.Replace(",", "@");
            //CommonSyn.TimeCancelSyn(timeBillNos, "", CommonClass.UserInfo.WebName, "USP_ADD_SEND_3");//时效取消同步 LD 2018-4-27
            //CommonSyn.TraceSyn(null, billnos.Replace(",", "@"), 12, "送货上门", 1, null, null);
        }

        //private void BillSendGoodsSyn(string billnos)
        //{
        //    try
        //    {
        //        List<SqlPara> listQuery = new List<SqlPara>();
        //        listQuery.Add(new SqlPara("BillNOStr", billnos));
        //        SqlParasEntity spsQuery = new SqlParasEntity(OperType.Query, "QSP_GET_BillSendGoodSynInfo", listQuery);
        //        DataSet dsQuery = SqlHelper.GetDataSet(spsQuery);
        //        if (dsQuery == null || dsQuery.Tables[0].Rows.Count == 0) return;
        //        string dsJson = JsonConvert.SerializeObject(dsQuery);
        //        RequestModel<string> request = new RequestModel<string>();
        //        request.Request = dsJson;
        //        request.OperType = 0;
        //        string json = JsonConvert.SerializeObject(request);
        //        //string url = "http://localhost:42936/KDLMSService/ZQTMSBillSendGoodsSyn";
        //        //string url = "http://lms.dekuncn.com:7016/KDLMSService/ZQTMSBillSendGoodsSyn";
        //        string url = HttpHelper.urlBillSendSys;
        //        ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
        //        if (model.State != "200")
        //        {
        //            MsgBox.ShowOK(model.Message);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MsgBox.ShowException(ex);
        //    }
        //}

        private void sendtosite()
        {
            string sendcarno = SendCarNO.Text.Trim();
            string sendbatch = SendBatch.Text.Trim();

            string SendToSite = "", SendToWeb = "";
            SendToSite = edsite.Text.Trim();
            SendToWeb = edweb.Text.Trim();

            if (SendToSite == "")
            {
                XtraMessageBox.Show("必须选择分公司", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                edsite.Focus();
                return;
            }

            if (SendToWeb == "")
            {
                XtraMessageBox.Show("必须选择下级网点", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                edweb.Focus();
                return;
            }

            if (sendcarno == "" || sendbatch == "")
            {
                XtraMessageBox.Show("车号、发车批次、必须填写。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (sendcarno == "") SendCarNO.Focus();
                return;
            }
            //检测是否为加盟
            if (!ClearAccsend(GridOper.GetGridViewColumn(myGridView2, "accsend"))) return;

            string WebRole = "";
            DataRow[] webdr = CommonClass.dsWeb.Tables[0].Select("WebName='" + SendToSite + "'");
            if (webdr.Length > 0)
            {
                WebRole = webdr[0]["WebRole"].ToString();
            }

            string billnos = "", sendpcss = "", accsends = "", IdStr = "";
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                billnos += GridOper.GetRowCellValueString(myGridView2, i, "BillNo") + ",";
                sendpcss += ConvertType.ToInt32(myGridView2.GetRowCellValue(i, "sendqty")) + ",";
                accsends += ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "AccSend")) + ",";
                IdStr += ConvertType.ToString(myGridView2.GetRowCellValue(i, "Id")) + ",";

                decimal FetchPay = ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "FetchPay"));
                if (FetchPay >= 5000 && WebRole == "加盟")
                {
                    MsgBox.ShowOK("运单号：" + myGridView2.GetRowCellValue(i, "BillNo") + "提付款超过5000元，不能转给加盟商！");
                    return;
                }
            }
            if (billnos == "") return;
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("SendCarNo", sendcarno));
            list.Add(new SqlPara("SendBatch", sendbatch));
            list.Add(new SqlPara("SendDate", SendDate.DateTime));
            list.Add(new SqlPara("SendOperator", CommonClass.UserInfo.UserName));
            list.Add(new SqlPara("SendDriver", SendDriver.Text.Trim()));
            list.Add(new SqlPara("SendDesc", SendDesc.Text.Trim()));
            list.Add(new SqlPara("SendToSite", SendToSite));
            list.Add(new SqlPara("SendToWeb", SendToWeb));
            list.Add(new SqlPara("SendDriverPhone", SendDriverPhone.Text.Trim()));
            list.Add(new SqlPara("SendWeb", CommonClass.UserInfo.WebName));
            list.Add(new SqlPara("SendSite", CommonClass.UserInfo.WebName));
            list.Add(new SqlPara("billnos", billnos));
            list.Add(new SqlPara("AccSends", accsends));
            list.Add(new SqlPara("SendPCSs", sendpcss));
            list.Add(new SqlPara("IdStr", IdStr));

            int isSendWbps = 0;
            if (ckbPushApp.Checked)
            {
                isSendWbps = 1;
            }
            list.Add(new SqlPara("IsSendWbps", isSendWbps));

            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_SEND_TOSITE", list)) == 0) return;
            dataset3.Tables[0].Rows.Clear();
            MsgBox.ShowOK();

            //打印送货单
            if (checkEdit3.Checked)
            {
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_SEND_DETAIL_PRINT", new List<SqlPara> { new SqlPara("SendBatch", sendbatch) }));
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    row["NowCompany"] = CommonClass.UserInfo.gsqc;
                }
                //frmRuiLangService.Print("送货清单.grf", ds, CommonClass.UserInfo.gsqc);
                string shqd = "送货清单";
                if (File.Exists(Application.StartupPath + "\\Reports\\" + "送货清单" + "per.grf"))
                {
                    shqd = shqd + "per";
                }


                frmPrintRuiLang fpr = new frmPrintRuiLang(shqd, ds);
                fpr.ShowDialog();
                frmSendDetail.USP_ADD_SENDACCDETAIL(sendbatch);
            }
            //BillSendGoodsSyn(billnos);
            //CommonSyn.BillSendGoodsSyn(billnos, sendbatch);//zaj 分拨同步 2018-4-10
            //yzw  转二级同步
            CommonSyn.SEND_TOSITE_SYN(SendToWeb, SendDate.DateTime, billnos);

            billnos = billnos.Replace(",", "@");
            //CommonSyn.TimeSendUptSyn(billnos, CommonClass.UserInfo.WebName, SendToWeb, "USP_ADD_SEND_TOSITE");//同步送货修改时效 LD 2018-4-27
            //CommonSyn.TraceSyn(null, billnos, 11, "转送二级", 1, null, null);
            Clear();
        }

        private void save()
        {
            myGridView2.ClearColumnsFilter();
            myGridView2.PostEditor();
            myGridView2.UpdateCurrentRow();
            myGridView2.UpdateSummary();
            if (myGridView2.RowCount == 0)
            {
                XtraMessageBox.Show("没有选择送货清单,请在第①步中挑选清单!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            decimal AccSend = ConvertType.ToDecimal(GridOper.GetGridViewColumn(myGridView2, "AccSend").SummaryItem.SummaryValue);
            if (AccSend == 0)
            {
                if (XtraMessageBox.Show("没有填写送货费!\r\n填写方法：在第①步中选择工具栏上的分摊送货费，也可以手动填写送货费。\r\n\r\n确定不需要填写送货费，直接保存?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;
            }
            SendDate.DateTime = CommonClass.gcdate;

            if (radioGroup2.SelectedIndex == 0)
            {
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    if (myGridView2.GetRowCellValue(i, "TransferMode").ToString() == "自提")
                    {
                        MsgBox.ShowOK("送货清单中有自提货物，请检查！");
                        return;
                    }
                }
            }

            int a = 0;
            if (checkEdit1.Checked)
            {
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    if (ConvertType.ToInt32(myGridView2.GetRowCellValue(i, "ischecked")) == 1)
                    {
                        a++;
                        break;
                    }
                }
                if (a == 0)
                {
                    XtraMessageBox.Show("保存失败!\r\n\r\n您选择了“短信通知发货人”，却没有选择要发送短信的运单!\r\n\r\n请在第①步送货清单中勾选要发送短信的运单!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            if (checkEdit1.Checked)
            {
                sms.send_shipper(myGridView2, this, SendDate.DateTime, SendDriver.Text.Trim(), SendDriverPhone.Text.Trim());
            }
            if (checkEdit4.Checked)
            {
                if (checkEdit1.Checked) //先给发货人发完短信，ischecked会被置为2
                {
                    for (int i = 0; i < myGridView2.RowCount; i++)
                    {
                        myGridView2.SetRowCellValue(i, "ischecked", 1);
                    }
                }
                sms.send_consignee(myGridView2, this, SendDate.DateTime, SendDriver.Text.Trim(), SendDriverPhone.Text.Trim());
            }
            if(checkEdit2.Checked)
            {
                string BillNoStr = "";                //gyy
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    BillNoStr += GridOper.GetRowCellValueString(myGridView2, i, "BillNo") + "@";

                }
                PrintQSD(BillNoStr);

            }
            //送货上门
            if (radioGroup2.SelectedIndex == 0)
            {
                SendBatch.Text = GetMaxInOneVehicleFlag(); //抢批次
                if (SendBatch.Text.Trim() == "") return;
                senddirect();
            }
            if (radioGroup2.SelectedIndex == 1)//转二级
            {
                DataRow[] dr = CommonClass.dsWeb.Tables[0].Select(string.Format("SiteName like '%{0}%' and WebName like '%{1}%'", edsite.Text.Trim(), edweb.Text.Trim()));
                if (dr.Length == 0)
                {
                    MsgBox.ShowOK("选择的下级送货站点和网点不匹配！请检查！");
                    return;
                }

                SendBatch.Text = GetMaxInOneVehicleFlag(); //抢批次
                if (SendBatch.Text.Trim() == "") return;
                sendtosite();
            }
            if (radioGroup2.SelectedIndex == 2)//转二级到ZQTMS
            {
                DataRow[] dr = CommonClass.dsWebZQTMS.Tables[0].Select(string.Format("SiteName like '%{0}%' and WebName like '%{1}%'", edsiteZQTMS.Text.Trim(), edwebZQTMS.Text.Trim()));
                if (dr.Length == 0)
                {
                    MsgBox.ShowOK("选择的下级送货站点和网点不匹配！请检查！");
                    return;
                }

                SendBatch.Text = GetMaxInOneVehicleFlag(); //抢批次
                if (SendBatch.Text.Trim() == "") return;
                sendtositeZQTMS();
            }
        }
        private void PrintQSD(string BillNoStr)
        {
         try 
	{	   
            if (CommonClass.UserInfo.companyid == "486")
            {
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
                    else if (CommonClass.UserInfo.companyid == "486" && CommonClass.UserInfo.WebName == "宝虹武汉东西湖营业部")
                    {
                        name = "宝虹广州总部配送部_签收单";
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

                frmRuiLangService.Print(name, SqlHelper.GetDataTable(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTBIll_TX_1", new List<SqlPara> { new SqlPara("BillNoStr", BillNoStr) })), "");

            }
            else
            {

                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILLINFO_PRINT_ARRIVED_QSD", new List<SqlPara> { new SqlPara("BillNoStr", BillNoStr) }));
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    MsgBox.ShowOK("没有找到选中的运单信息,打印失败,(请检查网络或运单是否已被删除)!");
                    return;
                }
                //jl20181127
                if (CommonClass.UserInfo.WebName == "上海青浦操作部"
                    || CommonClass.UserInfo.WebName == "上海青浦操作部1"
                    || CommonClass.UserInfo.WebName == "杭州操作部"
                    || CommonClass.UserInfo.WebName == "杭州操作部1"
                    || CommonClass.UserInfo.WebName == "无锡二级分拨中心"
                    || CommonClass.UserInfo.WebName == "无锡二级分拨中心1"
                    || CommonClass.UserInfo.WebName == "宁波操作部"
                    || CommonClass.UserInfo.WebName == "宁波操作部1"
                    || CommonClass.UserInfo.WebName == "济南二级分拨中心"
                    || CommonClass.UserInfo.WebName == "济南二级分拨中心1"
                    || CommonClass.UserInfo.WebName == "无锡二级分拨中心"
                    || CommonClass.UserInfo.WebName == "无锡二级分拨中心1"
                    || CommonClass.UserInfo.WebName == "武汉二级分拨中心"
                    || CommonClass.UserInfo.WebName == "武汉二级分拨中心1"
                    || CommonClass.UserInfo.WebName == "广州操作部"
                    || CommonClass.UserInfo.WebName == "广州操作部1"
                    || CommonClass.UserInfo.WebName == "东莞大坪分拨中心"
                    || CommonClass.UserInfo.WebName == "东莞大坪分拨中心1"
                    || CommonClass.UserInfo.WebName == "青岛二级分拨中心"
                    || CommonClass.UserInfo.WebName == "青岛二级分拨中心1")
                {
                    frmRuiLangService.Print("提货单大坪", ds.Tables[0], CommonClass.UserInfo.gsqc);
                }
                else
                {
                    frmRuiLangService.Print("提货单", ds.Tables[0], CommonClass.UserInfo.gsqc);
                }

            }    
		
	}
	catch (Exception)
	{
		
		throw;
	}
            




        }
        private void PrintSendBills(string inoneflag)
        {
            //w_ht_print whp = new w_ht_print();
            //whp.printtype = "打印送货清单";
            //whp.inoneflag = inoneflag;
            //whp.Show();
        }

        private void Clear()
        {
            radioGroup2.SelectedIndex = 0;
            SendBatch.Text = GetMaxInOneVehicleFlag();
            SendDate.DateTime = CommonClass.gcdate;
            SendCarNO.Text = SendDriver.Text = SendDriverPhone.Text = SendDesc.Text = "";
        }

        private string GetMaxInOneVehicleFlag()
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("bcode", CommonClass.UserInfo.LoginSiteCode));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SENDFLAG", list);
            DataSet ds = SqlHelper.GetDataSet(sps);
            if (ds == null || ds.Tables.Count == 0) return "";

            return ConvertType.ToString(ds.Tables[0].Rows[0][0]);
        }

        private void w_send_load_Load(object sender, EventArgs e)
        {
            if (!CommonClass.FormSet(this, false, true)) return;
            CommonClass.GetGridViewColumns(myGridView1, false, myGridView2, myGridView3);
            GridOper.SetGridViewProperty(myGridView1, myGridView2, myGridView3);
            BarMagagerOper.SetBarPropertity(bar1, bar2); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1, myGridView2);
            GridOper.CreateStyleFormatCondition(myGridView1, "LckDate", FormatConditionEnum.Equal, 0, Color.FromArgb(255, 255, 255));//颜色固定--白色
            GridOper.CreateStyleFormatCondition(myGridView1, "LckDate", FormatConditionEnum.Equal, 1, Color.FromArgb(193, 255, 193));//颜色固定--绿色
            GridOper.CreateStyleFormatCondition(myGridView1, "LckDate", FormatConditionEnum.Greater, 1, Color.LightBlue);//颜色固定--浅蓝色
            //plh20191029

            //获取送货费列
            GridColumn gc = GridOper.GetGridViewColumn(myGridView2, "AccSend");
            gc.ColumnEdit = this.repositoryItemPopupContainerEdit2;

            //edsite.Properties.Items.Add(CommonClass.UserInfo.SiteName);
            CommonClass.SetSite(edsite, false);
            CommonClass.SetSiteZQTMS(edsiteZQTMS, false);//maohui20181108
            SendDate.DateTime = CommonClass.gcdate;
            SendBatch.Text = GetMaxInOneVehicleFlag();

            try
            {
                CommonClass.AreaManager.FillCityToImageComBoxEdit(edsheng, "0");
                myGridControl3.DataSource = CommonClass.dsCar.Tables[0];
            }
            catch { }
            //hj20180904
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_gsNotStreet"));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                NotStreet = ds.Tables[0].Rows[0]["companyid"].ToString().Split(',');
            }
        }

        /// <summary>
        /// 获取二级中转市县的目的网点
        /// </summary>
        private void GetBasMiddleSiteWebNameBySite()
        {
            edweb.Properties.Items.Clear();
            string siteName = edsite.Text.Trim();
            if (siteName == "" || CommonClass.dsMiddleSite == null || CommonClass.dsMiddleSite.Tables.Count == 0) return;
            DataRow[] drs = CommonClass.dsMiddleSite.Tables[0].Select("SiteName='" + siteName + "'");
            if (drs == null || drs.Length == 0) return;
            foreach (DataRow dr in drs)
            {
                siteName = ConvertType.ToString(dr["WebName"]);
                if (siteName != "" && !edweb.Properties.Items.Contains(siteName))
                    edweb.Properties.Items.Add(siteName);
            }
        }
        public bool isDeficit(decimal ActualFreight, decimal AccSend)
        {
            try
            {
                bool ischeck = false;
                ActualFreight=ActualFreight==0?1:ActualFreight;
                costrate = Math.Round(AccSend / ActualFreight, 2);
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SendWebName", CommonClass.UserInfo.WebName.ToString().Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_CostControlsList_rate_2", list);
                DataSet ds_check = SqlHelper.GetDataSet(sps);
                if (ds_check != null && ds_check.Tables.Count > 0 && ds_check.Tables[0].Rows.Count > 0)
                {
                    if (costrate > Convert.ToDecimal(ds_check.Tables[0].Rows[0]["TargetcostRate"].ToString()))
                    {


                        ischeck = true;
                    }


                }
                return ischeck;
            }

            catch (Exception)
            {

                throw;
            }
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (radioGroup2.SelectedIndex == 0 && CommonClass.CheckKongHuo(myGridView2, 1))
            {
                MsgBox.ShowOK("选择的清单包含控货的运单,不能送货!");
                return;
            }
            if (radioGroup2.SelectedIndex == 0) //送货上门
            {
                edsite.Text = "";
                edweb.Text = "";
            }
            try
            {
                if (CommonClass.UserInfo.companyid == "485")
                {
                    decimal AccSendSum = 0, ActualFreightSum = 0;

                    for (int i = 0; i < myGridView2.RowCount; i++)
                    {
                        AccSendSum += ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "AccSend"));
                        ActualFreightSum += ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "ActualFreight"));
                    }



                    if (isDeficit(ActualFreightSum, AccSendSum))
                    {
                        frmIsCostDeficits Cost = new frmIsCostDeficits();
                        Cost.DepartureBatch = SendBatch.Text.Trim();
                        Cost.SendWebName = CommonClass.UserInfo.WebName.ToString().Trim();
                        Cost.actual_rate = costrate;
                        Cost.MenuType = "送货亏损";
                        Cost.ShowDialog();
                        if (Cost.isprint == true)
                        {
                            save();
                        }

                    }

                    else
                    {
                        save();
                    }
                }

                else
                {
                    save();
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 查询运单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //cc.ShowBillDetail(myGridView2);
        }

        private void 查看运单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //cc.ShowBillDetail(myGridView1);
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            getdata();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "送货库存清单");
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridViewMove.Move(myGridView1, dataset1, dataset3);
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView1.SelectAll();
            GridViewMove.Move(myGridView1, dataset1, dataset3);
        }

        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridViewMove.Move(myGridView2, dataset3, dataset1);
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView2.SelectAll();
            GridViewMove.Move(myGridView2, dataset3, dataset1);
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = tp3;
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barCheckItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            //cc.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem15_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView2);
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView2);
        }

        private void barEditItem1_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue == null) return;
            string szfilter = e.NewValue.ToString().Trim();
            if (szfilter != "")
            {
                myGridView1.ClearColumnsFilter();
                myGridView1.Columns["BillNo"].FilterInfo = new ColumnFilterInfo("[BillNo] LIKE " + "'%" + szfilter + "%'", "");
            }
            else
                myGridView1.ClearColumnsFilter();
        }

        private void barEditItem1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || ((DevExpress.XtraEditors.TextEdit)sender).Text == "" || myGridView1.RowCount > 1)
            {
                e.Handled = true;
                return;
            }
            myGridView1.SelectRow(0);
            GridViewMove.Move(myGridView1, dataset1, dataset3);
            ((DevExpress.XtraEditors.TextEdit)sender).Text = "";

            myGridView1.ClearColumnsFilter();
            e.Handled = true;
        }

        private void barEditItem2_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue == null) return;
            string szfilter = e.NewValue.ToString().Trim();
            if (szfilter != "")
            {
                myGridView2.ClearColumnsFilter();
                myGridView2.Columns["BillNo"].FilterInfo = new ColumnFilterInfo("[BillNo] LIKE" + "'%" + szfilter + "%'", "");
            }
            else
                myGridView2.ClearColumnsFilter();
        }

        private void barEditItem2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || ((DevExpress.XtraEditors.TextEdit)sender).Text == "" || myGridView2.RowCount > 1)
            {
                e.Handled = true;
                return;
            }
            myGridView2.SelectRow(0);
            GridViewMove.Move(myGridView2, dataset3, dataset1);
            ((DevExpress.XtraEditors.TextEdit)sender).Text = "";

            myGridView2.ClearColumnsFilter();
            e.Handled = true;
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            myGridView2.ClearColumnsFilter();
            if (myGridView2.RowCount == 0)
            {
                XtraMessageBox.Show("请先挑选要送货的运单!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (textEdit3.Text.Trim() == "")
            {
                XtraMessageBox.Show("请输入整车送货费!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                decimal qtytotal = Convert.ToDecimal(myGridView2.Columns["Num"].SummaryItem.SummaryValue);//总件数
                int type = radioGroup1.SelectedIndex;
                decimal acctotal = Convert.ToDecimal(textEdit3.Text.Trim());//待分摊的送货费
                decimal sum = 0, accrow = 0;

                string filedname = "AccSend"; //分摊送货费

                if (type == 0)
                {//按件数分摊
                    sum = 0;
                    for (int i = 0; i < myGridView2.RowCount; i++)
                    {
                        accrow = Math.Floor(acctotal * ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "Num")) / qtytotal);
                        sum += accrow;
                        if (i == myGridView2.RowCount - 1)
                        {
                            myGridView2.SetRowCellValue(i, filedname, accrow + (acctotal - sum));
                        }
                        else
                        {
                            myGridView2.SetRowCellValue(i, filedname, accrow);
                        }
                    }
                    barEditItem4.EditValue = "按件数";
                }
                else if (type == 1)
                {
                    decimal a = ConvertType.ToDecimal(myGridView2.Columns["Freight"].SummaryItem.SummaryValue);//运费合计
                    sum = 0;

                    for (int i = 0; i < myGridView2.RowCount; i++)
                    {
                        accrow = Math.Floor(acctotal * ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "Freight")) / a);
                        sum += accrow;

                        if (i == myGridView2.RowCount - 1)
                        {
                            myGridView2.SetRowCellValue(i, filedname, accrow + (acctotal - sum));
                        }
                        else
                        {
                            myGridView2.SetRowCellValue(i, filedname, accrow);
                        }
                    }
                    barEditItem4.EditValue = "按运费";
                }
                else if (type == 2)
                {
                    decimal avg = Math.Floor(acctotal / Convert.ToDecimal(myGridView2.RowCount));//按票
                    for (int i = 0; i < myGridView2.RowCount; i++)
                    {
                        if (i == myGridView2.RowCount - 1)
                        {
                            myGridView2.SetRowCellValue(i, filedname, acctotal - avg * (myGridView2.RowCount - 1));
                        }
                        else
                        {
                            myGridView2.SetRowCellValue(i, filedname, avg);
                        }
                    }
                    barEditItem4.EditValue = "按票";
                }
                else  //type=3  按重量比例
                {
                    decimal a = Convert.ToDecimal(myGridView2.Columns["Weight"].SummaryItem.SummaryValue);//重量合计
                    sum = 0;

                    for (int i = 0; i < myGridView2.RowCount; i++)
                    {
                        accrow = Math.Floor(acctotal * ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "Weight")) / a);
                        sum += accrow;

                        if (i == myGridView2.RowCount - 1)
                        {
                            myGridView2.SetRowCellValue(i, filedname, accrow + (acctotal - sum));
                        }
                        else
                        {
                            myGridView2.SetRowCellValue(i, filedname, accrow);
                        }
                    }
                    barEditItem4.EditValue = "按重量";
                }

                myGridView2.UpdateSummary();
                XtraMessageBox.Show("分摊成功!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                myGridView2.Focus();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                myGridView2.SetRowCellValue(i, "accsendout", 0);
            }
            XtraMessageBox.Show("取消分摊成功!实际送货费已清零!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    myGridView2.SetRowCellValue(i, "ischecked", 1);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void myGridView2_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            try
            {
                GridColumn gc = ((DevExpress.XtraGrid.Views.Grid.GridView)sender).FocusedColumn;
                if (e == null || gc == null || gc.FieldName != "sendqty") return;
                int oldvalue = ConvertType.ToInt32(myGridView2.GetFocusedRowCellValue("sendremainqty"));//库存件数
                int newvalue = ConvertType.ToInt32(e.Value);//填的件数
                if (newvalue <= 0)
                {
                    e.Valid = false;
                    e.ErrorText = "实发件数必须大于0！";
                }
                else if (newvalue > oldvalue)
                {
                    e.Valid = false;
                    e.ErrorText = string.Format("实发件数不能大于库存件数(当前库存{0}件)！", oldvalue);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void myGridView2_InvalidValueException(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            MsgBox.ShowError(e.ErrorText);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MsgBox.ShowTip("按件数分摊：该单件数/本车清单总件数", linkLabel1, 5);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MsgBox.ShowTip("按运费分摊：基本运费/总基本运费", linkLabel2, 5);
        }

        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem16_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem17_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView2.Guid.ToString());
        }
        /// <summary>
        /// 选择下级网点时过滤没设置可接收二级网点的数据
        /// </summary>
        /// <param name="cb"></param>
        /// <param name="SiteName"></param>
        /// <param name="isall"></param>
        private void SetWeb(ComboBoxEdit cb, string SiteName, bool isall)
        {
            try
            {

                if (CommonClass.dsWeb == null || CommonClass.dsWeb.Tables.Count == 0) return;
                if (SiteName == "全部") SiteName = "%%";
                DataRow[] dr = CommonClass.dsWeb.Tables[0].Select("SiteName like '" + SiteName + "' and IsAcceptejSend=1");
                cb.Properties.Items.Clear();
                cb.Text = "";
                for (int i = 0; i < dr.Length; i++)
                {
                    cb.Properties.Items.Add(dr[i]["WebName"]);
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                    cb.Text = "全部";
                }
            }
            catch (Exception)
            {
                MsgBox.ShowOK("正在加载基础资料，请稍等！");
            }
        }


        private void edsite_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (edsite.Text.Trim() == "") return;
            SetWeb(edweb, edsite.Text.Trim(), false);
            //    GetBasMiddleSiteWebNameBySite();//到二级中转市县里找网点
            if (edweb.Properties.Items.Contains(CommonClass.UserInfo.WebName))
                edweb.Properties.Items.Remove(CommonClass.UserInfo.WebName);// 不用转到当前网点
            edweb.Text = "";
        }

        private void radioGroup2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup2.SelectedIndex == 0)
            {
                edsite.Text = edweb.Text = "";
                edsite.Enabled = edweb.Enabled = false;
                edsiteZQTMS.Enabled = edwebZQTMS.Enabled = false;
            }
            if (radioGroup2.SelectedIndex == 1)
            {
                edsite.Enabled = edweb.Enabled = true;
                edsiteZQTMS.Enabled = edwebZQTMS.Enabled = false;
            }
            if (radioGroup2.SelectedIndex == 2)
            {
                edsite.Text = edweb.Text = "";
                edsite.Enabled = edweb.Enabled = false;
                edsiteZQTMS.Enabled = edwebZQTMS.Enabled = true;
            }
            //else
            //{
            //    edsite.Enabled = edweb.Enabled = true;
            //}
        }

        private void myGridControl3_Leave(object sender, EventArgs e)
        {
            myGridControl3.Visible = SendCarNO.Focused;
        }

        private void SetCarInfo()
        {
            int rowhandle = myGridView3.FocusedRowHandle;
            if (rowhandle < 0) return;

            DataRow dr = myGridView3.GetDataRow(rowhandle);
            if (dr == null) return;

            myGridControl3.Visible = false;
            SendCarNO.EditValue = dr["CarNo"];
            SendDriver.EditValue = dr["DriverName"];
            SendDriverPhone.EditValue = dr["DriverPhone"];
        }

        private void myGridControl3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                myGridControl3.Visible = false;
            }
            if (e.KeyCode == Keys.Enter)
            {
                SetCarInfo();
            }
        }

        private void myGridView3_DoubleClick(object sender, EventArgs e)
        {
            SetCarInfo();
        }

        private void SendCarNO_Enter(object sender, EventArgs e)
        {
            myGridControl3.Left = SendCarNO.Left;
            myGridControl3.Top = SendCarNO.Top + SendCarNO.Height + 2;
            myGridControl3.Visible = true;
        }

        private void SendCarNO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
                myGridControl3.Focus();
            if (e.KeyCode == Keys.Escape)
            {
                myGridControl3.Visible = false;
            }
        }

        private void SendCarNO_Leave(object sender, EventArgs e)
        {
            myGridControl3.Visible = myGridControl3.Focused;
        }

        private void barEditItem4_ShownEditor(object sender, ItemClickEventArgs e)
        {
            textEdit3.Focus();
        }

        private void textEdit3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) simpleButton6.PerformClick();
        }

        private void myGridView2_RowCountChanged(object sender, EventArgs e)
        {
            //if (myGridView2.RowCount < myGridView2RowCount) return;
            //myGridView2RowCount = myGridView2.RowCount;
            //int rowhandle = myGridView2.RowCount - 1;
            //if (rowhandle < 0) return;
            //string s = ConvertType.ToString(myGridView2.GetRowCellValue(rowhandle, "BespeakContent"));
            //if (s != "")
            //{
            //    if (XtraMessageBox.Show("预约信息:" + s + "\r\n是否剔除此单？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.OK) return;
            //    myGridView2.SelectRow(rowhandle);
            //    GridViewMove.Move(myGridView2, dataset3, dataset1);//移回去
            //}
        }

        private void SendCarNO_EditValueChanging(object sender, ChangingEventArgs e)
        {
            if (e.NewValue == null) return;
            string value = e.NewValue.ToString();
            myGridView3.Columns["CarNo"].FilterInfo = new ColumnFilterInfo(
                    "[CarNo] LIKE " + "'%" + value + "%'"
                    + " OR [DriverName] LIKE" + "'%" + value + "%'"
                    + " OR [DriverPhone] LIKE" + "'%" + value + "%'",
                    "");
        }

        private void edsheng_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.AreaManager.FillCityToImageComBoxEdit(edcity, edsheng.EditValue);
        }

        private void edcity_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.AreaManager.FillCityToImageComBoxEdit(edarea, edcity.EditValue);
        }

        private void edarea_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.AreaManager.FillCityToImageComBoxEdit(edtown, edarea.EditValue);
        }

        private void edtown_SelectedIndexChanged(object sender, EventArgs e)
        {
            //SetFee();
        }

        private void repositoryItemPopupContainerEdit2_Popup(object sender, EventArgs e)
        {
            int rowhandle = myGridView2.FocusedRowHandle;
            if (rowhandle < 0) return;

            //this.edsheng.SelectedIndexChanged -= new System.EventHandler(this.edsheng_SelectedIndexChanged);
            //this.edcity.SelectedIndexChanged -= new System.EventHandler(this.edcity_SelectedIndexChanged);
            //this.edarea.SelectedIndexChanged -= new System.EventHandler(this.edarea_SelectedIndexChanged);
            //this.edtown.SelectedIndexChanged -= new System.EventHandler(this.edtown_SelectedIndexChanged);

            edbillno.Text = GridOper.GetRowCellValueString(myGridView2, rowhandle, "BillNo");
            edweight.Text = ConvertType.ToDecimal(myGridView2.GetRowCellValue(rowhandle, "Weight"), "");
            edvolumn.Text = ConvertType.ToDecimal(myGridView2.GetRowCellValue(rowhandle, "Volume"), "");

            CommonClass.SetSelectIndex(GridOper.GetRowCellValueString(myGridView2, rowhandle, "SendToProvince"), edsheng);
            CommonClass.SetSelectIndex(GridOper.GetRowCellValueString(myGridView2, rowhandle, "SendToCity"), edcity);
            CommonClass.SetSelectIndex(GridOper.GetRowCellValueString(myGridView2, rowhandle, "SendToArea"), edarea);
            CommonClass.SetSelectIndex(GridOper.GetRowCellValueString(myGridView2, rowhandle, "SendToStreet"), edtown);
            textEdit4.Text = ConvertType.ToDecimal(myGridView2.GetRowCellValue(rowhandle, "AccSend"), "");

            //this.edsheng.SelectedIndexChanged += new System.EventHandler(this.edsheng_SelectedIndexChanged);
            //this.edcity.SelectedIndexChanged += new System.EventHandler(this.edcity_SelectedIndexChanged);
            //this.edarea.SelectedIndexChanged += new System.EventHandler(this.edarea_SelectedIndexChanged);
            //this.edtown.SelectedIndexChanged += new System.EventHandler(this.edtown_SelectedIndexChanged);
        }

        private void SetFee()
        {
            if (CommonClass.dsSendPrice == null || CommonClass.dsSendPrice.Tables.Count == 0 || CommonClass.dsSendPrice.Tables[0].Rows.Count == 0) return;
            int rows = myGridView2.FocusedRowHandle;
            if (rows < 0) return;
            string TransferMode = myGridView2.GetRowCellValue(rows, "TransferMode").ToString();
            decimal Weight = ConvertType.ToDecimal(myGridView2.GetRowCellValue(rows, "OperationWeight").ToString());
            decimal DeliveryFee = ConvertType.ToDecimal(myGridView2.GetRowCellValue(rows, "DeliveryFee").ToString());
            if (TransferMode == "网点送货" || TransferMode == "中转送货")
            {
                string sql = "Province='" + edsheng.Text.Trim()
                    + "' and City='" + edcity.Text.Trim()
                    + "' and Area='" + edarea.Text.Trim()
                    + "' and Street='" + edtown.Text.Trim()
                    + "' and TransferMode='" + TransferMode + "'";
                DataRow[] drDeliveryFee = CommonClass.dsSendPrice.Tables[0].Select(sql);
                if (drDeliveryFee.Length > 0)
                {
                    decimal w0_200 = ConvertType.ToDecimal(drDeliveryFee[0]["w0_200"]);
                    decimal w200_1000 = ConvertType.ToDecimal(drDeliveryFee[0]["w200_1000"]);
                    decimal w1000_3000 = ConvertType.ToDecimal(drDeliveryFee[0]["w1000_3000"]);
                    decimal w3000_5000 = ConvertType.ToDecimal(drDeliveryFee[0]["w3000_5000"]);
                    decimal w5000_100000 = ConvertType.ToDecimal(drDeliveryFee[0]["w5000_100000"]);
                    decimal DeliveryFee_C = 0;
                    if (Weight >= 0 && Weight <= 200)
                    {
                        DeliveryFee_C = w0_200;
                    }
                    else if (Weight >= 200 && Weight <= 1000)
                    {
                        DeliveryFee_C = w200_1000;
                    }
                    else if (Weight >= 1000 && Weight <= 3000)
                    {
                        DeliveryFee_C = w1000_3000;
                    }
                    else if (Weight >= 3000 && Weight <= 5000)
                    {
                        DeliveryFee_C = w3000_5000;
                    }
                    else if (Weight > 5000)
                    {
                        DeliveryFee_C = w5000_100000;
                    }
                    if (DeliveryFee != DeliveryFee_C)
                    {
                        myGridView2.SetRowCellValue(rows, "DeliveryFee_C", DeliveryFee_C);
                    }
                    else
                    {
                        myGridView2.SetRowCellValue(rows, "DeliveryFee_C", 0);
                    }
                }
                else
                {
                    myGridView2.SetRowCellValue(rows, "DeliveryFee_C", 0);
                }
            }
            else
            {
                myGridView2.SetRowCellValue(rows, "DeliveryFee_C", 0);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            int rowhandle = myGridView2.FocusedRowHandle;
            if (rowhandle < 0) return;

            myGridView2.SetRowCellValue(rowhandle, "SendToProvince", edsheng.Text.Trim());
            myGridView2.SetRowCellValue(rowhandle, "SendToCity", edcity.Text.Trim());
            myGridView2.SetRowCellValue(rowhandle, "SendToArea", edarea.Text.Trim());
            myGridView2.SetRowCellValue(rowhandle, "SendToStreet", edtown.Text.Trim());

            myGridView2.SetRowCellValue(rowhandle, "AccSend", ConvertType.ToDecimal(textEdit4.Text));
            SetFee();
            edsheng.Text = textEdit4.Text = "";
        }

        private void edweb_TextChanged(object sender, EventArgs e)
        {
            label8.Text = "";
            string siteName = edsite.Text.Trim();
            string webName = edweb.Text.Trim();
            if (siteName == "" || webName == "" || CommonClass.dsWeb == null || CommonClass.dsWeb.Tables.Count == 0) return;

            foreach (DataRow dr in CommonClass.dsWeb.Tables[0].Rows)
            {
                if (ConvertType.ToString(dr["SiteName"]) == siteName && ConvertType.ToString(dr["WebName"]) == webName)
                {
                    label8.Text = ConvertType.ToString(dr["WebRole"]);
                    break;
                }
            }
            ClearAccsend(GridOper.GetGridViewColumn(myGridView2, "accsend"));
        }

        private bool ClearAccsend(GridColumn gc)
        {
            if (gc == null) return false;
            gc.OptionsColumn.AllowEdit = gc.OptionsColumn.AllowFocus = label8.Text != "加盟";
            if (label8.Text != "加盟") return true;

            if (MsgBox.ShowYesNo("转二级的网点为加盟，将清除所有实际送货费，是否继续？") != DialogResult.Yes) return false;

            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                myGridView2.SetRowCellValue(i, gc, 0);
            }
            return true;//通过
        }

        private void edsiteZQTMS_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (edsiteZQTMS.Text.Trim() == "")
            {
                return;
            }
            //SetWeb(edweb, edsite.Text.Trim(), false);
            SetWebZQTMS(edwebZQTMS, edsiteZQTMS.Text.Trim(), false);
            //GetBasMiddleSiteWebNameBySite();//到二级中转市县里找网点
            if (edweb.Properties.Items.Contains(CommonClass.UserInfo.WebName))
                edweb.Properties.Items.Remove(CommonClass.UserInfo.WebName);// 不用转到当前网点
            edweb.Text = "";
        }

        private void SetWebZQTMS(ComboBoxEdit cb, string SiteName, bool isall)
        {
            try
            {
                if (CommonClass.dsWebZQTMS == null || CommonClass.dsWebZQTMS.Tables.Count == 0) return;
                if (SiteName == "全部") SiteName = "%%";
                DataRow[] dr = CommonClass.dsWebZQTMS.Tables[0].Select("SiteName like '" + SiteName + "' and IsAcceptejSend=1");
                cb.Properties.Items.Clear();
                cb.Text = "";
                for (int i = 0; i < dr.Length; i++)
                {
                    cb.Properties.Items.Add(dr[i]["WebName"]);
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                    cb.Text = "全部";
                }
            }
            catch (Exception)
            {
                MsgBox.ShowOK("正在加载基础资料，请稍等！");
            }
        }

        private void edwebZQTMS_TextChanged(object sender, EventArgs e)
        {
            label12.Text = "";
            string siteName = edsiteZQTMS.Text.Trim();
            string webName = edwebZQTMS.Text.Trim();
            if (siteName == "" || webName == "" || CommonClass.dsWebZQTMS == null || CommonClass.dsWebZQTMS.Tables.Count == 0) return;

            foreach (DataRow dr in CommonClass.dsWebZQTMS.Tables[0].Rows)
            {
                if (ConvertType.ToString(dr["SiteName"]) == siteName && ConvertType.ToString(dr["WebName"]) == webName)
                {
                    label12.Text = ConvertType.ToString(dr["WebRole"]);
                    break;
                }
            }
            ClearAccsend(GridOper.GetGridViewColumn(myGridView2, "accsend"));
        }

        private void sendtositeZQTMS()
        {
            string sendcarno = SendCarNO.Text.Trim();
            string sendbatch = SendBatch.Text.Trim();

            string SendToSite = "", SendToWeb = "";
            SendToSite = edsiteZQTMS.Text.Trim();
            SendToWeb = edwebZQTMS.Text.Trim();

            if (SendToSite == "")
            {
                XtraMessageBox.Show("必须选择分公司", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                edsiteZQTMS.Focus();
                return;
            }

            if (SendToWeb == "")
            {
                XtraMessageBox.Show("必须选择下级网点", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                edwebZQTMS.Focus();
                return;
            }

            if (sendcarno == "" || sendbatch == "")
            {
                XtraMessageBox.Show("车号、发车批次、必须填写。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (sendcarno == "") SendCarNO.Focus();
                return;
            }
            //检测是否为加盟
            if (!ClearAccsend(GridOper.GetGridViewColumn(myGridView2, "accsend"))) return;

            string WebRole = "";
            DataRow[] webdr = CommonClass.dsWeb.Tables[0].Select("WebName='" + SendToSite + "'");
            if (webdr.Length > 0)
            {
                WebRole = webdr[0]["WebRole"].ToString();
            }

            string billnos = "", sendpcss = "", accsends = "", IdStr = "";
            string billnosLMS = "", billnosZQTMS = "", sendpcssLMS = "", sendpcssZQTMS = "", accsendsLMS = "", accsendsZQTMS = "", IdStrLMS = "", IdStrZQTMS = "";
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                billnos += GridOper.GetRowCellValueString(myGridView2, i, "BillNo") + ",";
                sendpcss += ConvertType.ToInt32(myGridView2.GetRowCellValue(i, "sendqty")) + ",";
                accsends += ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "AccSend")) + ",";
                IdStr += ConvertType.ToString(myGridView2.GetRowCellValue(i, "Id")) + ",";
                //maohui将LMS的运单数据集同步到ZQTMS去
                if (Convert.ToString(myGridView2.GetRowCellValue(i, "SystemSource")) == "LMS")
                {
                    billnosLMS += GridOper.GetRowCellValueString(myGridView2, i, "BillNo") + ",";
                    sendpcssLMS += ConvertType.ToInt32(myGridView2.GetRowCellValue(i, "sendqty")) + ",";
                    accsendsLMS += ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "AccSend")) + ",";
                    IdStrLMS += ConvertType.ToString(myGridView2.GetRowCellValue(i, "Id")) + ",";
                }
                else
                {
                    billnosZQTMS += GridOper.GetRowCellValueString(myGridView2, i, "BillNo") + ",";
                    sendpcssZQTMS += ConvertType.ToInt32(myGridView2.GetRowCellValue(i, "sendqty")) + ",";
                    accsendsZQTMS += ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "AccSend")) + ",";
                    IdStrZQTMS += ConvertType.ToString(myGridView2.GetRowCellValue(i, "Id")) + ",";
                }

                decimal FetchPay = ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "FetchPay"));
                if (FetchPay >= 5000 && WebRole == "加盟")
                {
                    MsgBox.ShowOK("运单号：" + myGridView2.GetRowCellValue(i, "BillNo") + "提付款超过5000元，不能转给加盟商！");
                    return;
                }
            }
            if (billnos == "") return;
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("SendCarNo", sendcarno));
            list.Add(new SqlPara("SendBatch", sendbatch));
            list.Add(new SqlPara("SendDate", SendDate.DateTime));
            list.Add(new SqlPara("SendOperator", CommonClass.UserInfo.UserName));
            list.Add(new SqlPara("SendDriver", SendDriver.Text.Trim()));
            list.Add(new SqlPara("SendDesc", SendDesc.Text.Trim()));
            list.Add(new SqlPara("SendToSite", SendToSite));
            list.Add(new SqlPara("SendToWeb", SendToWeb));
            list.Add(new SqlPara("SendDriverPhone", SendDriverPhone.Text.Trim()));
            list.Add(new SqlPara("SendWeb", CommonClass.UserInfo.WebName));
            list.Add(new SqlPara("SendSite", CommonClass.UserInfo.WebName));
            list.Add(new SqlPara("billnos", billnos));
            list.Add(new SqlPara("AccSends", accsends));
            list.Add(new SqlPara("SendPCSs", sendpcss));
            list.Add(new SqlPara("IdStr", IdStr));
            list.Add(new SqlPara("IsSyn", 1));

            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_SEND_TOSITE", list)) == 0) return;
            dataset3.Tables[0].Rows.Clear();
            MsgBox.ShowOK();

            //打印送货单
            if (checkEdit3.Checked)
            {
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_SEND_DETAIL_PRINT", new List<SqlPara> { new SqlPara("SendBatch", sendbatch) }));
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    row["NowCompany"] = CommonClass.UserInfo.gsqc;
                }
                //frmRuiLangService.Print("送货清单.grf", ds, CommonClass.UserInfo.gsqc);
                //frmSendDetail.USP_ADD_SENDACCDETAIL(sendbatch);

                string shqd = "送货清单";
                if (File.Exists(Application.StartupPath + "\\Reports\\" + "送货清单" + "per.grf"))
                {
                    shqd = shqd + "per";
                }
                frmPrintRuiLang fpr = new frmPrintRuiLang(shqd, ds);

                //frmPrintRuiLang fpr = new frmPrintRuiLang("送货清单.grf", ds);
                fpr.ShowDialog();
            }
            if (radioGroup2.SelectedIndex == 2)//maohui20181109
            {
                if (billnosLMS != "")
                {
                    List<SqlPara> listSYN = new List<SqlPara>();
                    listSYN.Add(new SqlPara("billnosLMS", billnosLMS));
                    listSYN.Add(new SqlPara("AcceptSiteName", edsiteZQTMS.Text.Trim()));
                    listSYN.Add(new SqlPara("AcceptWebName", edwebZQTMS.Text.Trim()));
                    listSYN.Add(new SqlPara("CarNo", sendcarno));
                    listSYN.Add(new SqlPara("DriverName", SendDriver.Text.Trim()));
                    listSYN.Add(new SqlPara("DriverPhone", SendDriverPhone.Text.Trim()));
                    listSYN.Add(new SqlPara("Remark", SendDesc.Text.Trim()));
                    listSYN.Add(new SqlPara("Batch", SendBatch.Text.Trim()));
                    listSYN.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));
                    //if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_FB_InsertToLMS", listSYN)) == 0)
                    //{
                    //    return;
                    //}
                    CommonSyn.LMSSynZQTMS(listSYN, "转二级运单数据同步", "UAP_ADD_FB_ONWaybill_SendToSite");
                }
                //list.Remove(new SqlPara("billnos", billnos));
                //list.Remove(new SqlPara("AccSends", accsends));
                //list.Remove(new SqlPara("SendPCSs", sendpcss));
                //list.Remove(new SqlPara("IdStr", IdStr));
                //list.Add(new SqlPara("billnos", billnosLMS));
                //list.Add(new SqlPara("AccSends", accsendsLMS));
                //list.Add(new SqlPara("SendPCSs", sendpcssLMS));
                //list.Add(new SqlPara("IdStr", IdStrLMS));
                CommonSyn.LMSSynZQTMS(list, "转二级同步", "UAP_ADD_FB_ON_SendToSite");

                List<SqlPara> listLMS = new List<SqlPara>();
                listLMS.Add(new SqlPara("SendBatch", sendbatch));
                listLMS.Add(new SqlPara("LoginAreaName", CommonClass.UserInfo.AreaName));
                listLMS.Add(new SqlPara("LoginCauseName", CommonClass.UserInfo.CauseName));
                listLMS.Add(new SqlPara("LoginDepartName", CommonClass.UserInfo.DepartName));
                listLMS.Add(new SqlPara("LoginSiteName", CommonClass.UserInfo.SiteName));
                listLMS.Add(new SqlPara("LoginWebName", CommonClass.UserInfo.WebName));
                listLMS.Add(new SqlPara("LoginUserAccount", CommonClass.UserInfo.UserAccount));
                listLMS.Add(new SqlPara("LoginUserName", CommonClass.UserInfo.UserName));
                CommonSyn.LMSSynZQTMS(listLMS, "转送到加盟二级网点扣费", "USP_ADD_SENDACCDETAIL_LMSSynZQTMS");

                billnosLMS = billnosLMS.Replace(",", "@");
                CommonSyn.TimeSendUptSyn(billnosLMS, CommonClass.UserInfo.WebName, SendToWeb, "USP_ADD_SEND_TOSITE");//同步送货修改时效 LD 2018-4-27
                CommonSyn.TraceSyn(null, billnosLMS, 11, "转送二级", 1, null, null);
            }
            if (billnosZQTMS != "")
            {
                CommonSyn.BillSendGoodsSyn(billnosZQTMS, sendbatch);//zaj 分拨同步 2018-4-10
                billnosZQTMS = billnosZQTMS.Replace(",", "@");
                CommonSyn.TimeSendUptSyn(billnosZQTMS, CommonClass.UserInfo.WebName, SendToWeb, "USP_ADD_SEND_TOSITE");//同步送货修改时效 LD 2018-4-27
                CommonSyn.TraceSyn(null, billnosZQTMS, 11, "转送二级", 1, null, null);
            }
            Clear();
        }

        private void barButtonItem21_ItemClick(object sender, ItemClickEventArgs e)
        {
                frmSelectOneBillSign frm = new frmSelectOneBillSign();
               frm.SignType = "SelectOneSendLoad";
               frm.webName = CommonClass.UserInfo.WebName;
                frm.siteName = CommonClass.UserInfo.SiteName;
               frm.ShowDialog();

                if (frm.rltDs != null && frm.rltDs.Tables.Count > 0)
               {
                    dataset1.Clear();
                   dataset3.Clear();
                    myGridView1.ClearColumnsFilter();
                   myGridView2.ClearColumnsFilter();

                    dataset1 = frm.rltDs;
                    dataset3 = frm.rltDs.Clone();
                   myGridControl1.DataSource = dataset1.Tables[0];
                   myGridControl2.DataSource = dataset3.Tables[0];
               }
            }
        }
    }
