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

namespace ZQTMS.UI
{
    public partial class frmInterOrderToSend : BaseForm
    {
        /// <summary>
        /// 选择的交接单号
        /// </summary>
        public string InterNoStr { get; set; }
        /// <summary>
        /// 运单号信息表
        /// </summary>
        public DataTable dtBillNo { get; set; }
        /// <summary>
        /// 是否为转二级
        /// </summary>
        public bool IsSendToSite { get; set; }

        public frmInterOrderToSend()
        {
            InitializeComponent();
            barEditItem2.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem2_EditValueChanging);
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

            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                if (GridOper.GetRowCellValueString(myGridView2, i, "SendToProvince") == "" || GridOper.GetRowCellValueString(myGridView2, i, "SendToCity") == "" || GridOper.GetRowCellValueString(myGridView2, i, "SendToArea") == "" || GridOper.GetRowCellValueString(myGridView2, i, "SendToStreet") == "")
                {
                    MsgBox.ShowOK("请填写第" + (i + 1) + "行送货地址!");
                    return;
                }
            }

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

            //先执行修改交接单信息
            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_SET_INTERORDER_SEND", new List<SqlPara> { new SqlPara("InterNoStr", InterNoStr), new SqlPara("BillNoStr", billnos), new SqlPara("InterStatus", "已送货"), new SqlPara("SendBatch", sendbatch) })) == 0) return;

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

            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_SEND_3", list)) == 0) return;
            myGridControl2.DataSource = null;
            MsgBox.ShowOK();

            //打印送货单
            if (checkEdit3.Checked)
            {
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_SEND_DETAIL_PRINT", new List<SqlPara> { new SqlPara("SendBatch", sendbatch) }));
                if (ds == null || ds.Tables.Count == 0) return;
                frmRuiLangService.Print("送货清单.grf", ds, CommonClass.UserInfo.gsqc);
            }
            CommonSyn.TraceSyn(null, billnos.Replace(",", "@"), 12, "送货上门", 1, null, null);
            Clear();
        }

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

            //先执行修改交接单信息
            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_SET_INTERORDER_SEND", new List<SqlPara> { new SqlPara("InterNoStr", InterNoStr), new SqlPara("BillNoStr", billnos), new SqlPara("InterStatus", "已转二级"), new SqlPara("SendBatch", sendbatch) })) == 0) return;

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

            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_SEND_TOSITE", list)) == 0) return;
            myGridControl2.DataSource = null;
            MsgBox.ShowOK();

            //打印送货单
            if (checkEdit3.Checked)
            {
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_SEND_DETAIL_PRINT", new List<SqlPara> { new SqlPara("SendBatch", sendbatch) }));
                if (ds == null || ds.Tables.Count == 0) return;
                frmRuiLangService.Print("送货清单.grf", ds, CommonClass.UserInfo.gsqc);
                USP_ADD_SENDACCDETAIL(sendbatch);
            }
            CommonSyn.TraceSyn(null, billnos.Replace(",","@"), 11, "转送二级", 1, null, null);
            Clear();
        }

        public static void USP_ADD_SENDACCDETAIL(string SendBatch)
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("SendBatch", SendBatch));
            SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_SENDACCDETAIL", list));
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
            //送货上门
            if (radioGroup2.SelectedIndex == 0)
            {
                SendBatch.Text = GetMaxInOneVehicleFlag(); //抢批次
                if (SendBatch.Text.Trim() == "") return;
                senddirect();
            }
            else//转二级
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
            radioGroup2.SelectedIndex = IsSendToSite ? 1 : 0;

            if (!CommonClass.FormSet(this, false, true)) return;
            CommonClass.GetGridViewColumns(myGridView2, false, myGridView3);
            GridOper.SetGridViewProperty(myGridView2, myGridView3);
            BarMagagerOper.SetBarPropertity(bar2); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView2);

            //获取送货费列
            GridColumn gc = GridOper.GetGridViewColumn(myGridView2, "AccSend");
            if (gc != null) gc.ColumnEdit = this.repositoryItemPopupContainerEdit2;

            CommonClass.SetSite(edsite, false);
            SendDate.DateTime = CommonClass.gcdate;
            SendBatch.Text = GetMaxInOneVehicleFlag();

            try
            {
                CommonClass.AreaManager.FillCityToImageComBoxEdit(edsheng, "0");
                myGridControl3.DataSource = CommonClass.dsCar.Tables[0];
            }
            catch { }

            getdata();
        }

        /// <summary>
        /// 提取数据
        /// </summary>
        private void getdata()
        {
            if (InterNoStr == "" || dtBillNo == null || dtBillNo.Rows.Count == 0) return;

            //提取送货库存
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SEND_LOAD", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;

                DataTable dtLoad = ds.Tables[0].Copy();
                DataTable dtNewLoad = ds.Tables[0].Clone();
                string billno = "", DeleteBillNoStr = "";
                int qty = 0, qty2 = 0;
                DataRow[] drs = null;
                for (int i = 0; i < dtBillNo.Rows.Count; i++)
                {
                    billno = ConvertType.ToString(dtBillNo.Rows[i]["BillNo"]);
                    drs = dtLoad.Select("BillNo='" + billno + "'");
                    if (drs == null || drs.Length == 0)
                    {
                        DeleteBillNoStr += billno + ",";
                        continue;
                    }

                    qty = ConvertType.ToInt32(dtBillNo.Rows[i]["Num"]);//扫描件数
                    qty2 = ConvertType.ToInt32(drs[0]["sendqty"]);//库存件数
                    drs[0]["sendqty"] = qty > qty2 ? qty2 : qty;//修改送货件数,扫多少件数就送多少件
                    dtNewLoad.ImportRow(drs[0]);
                }
                linkLabel3.Text = "不在当前库存的运单:" + DeleteBillNoStr;
                myGridControl2.DataSource = dtNewLoad;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                if (myGridView2.RowCount < 1000) myGridView2.BestFitColumns();
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
                save();
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

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = tp3;
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem15_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView2);
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView2);
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
            //GetBasMiddleSiteWebNameBySite();//到二级中转市县里找网点
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
            }
            else
            {
                edsite.Enabled = edweb.Enabled = true;
            }
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

        private void SendCarNO_EditValueChanging(object sender, ChangingEventArgs e)
        {
            if (e.NewValue == null) return;
            string value = e.NewValue.ToString();
            GridColumn gcCarNo = GridOper.GetGridViewColumn(myGridView3, "CarNo");
            if (gcCarNo == null) return;
            gcCarNo.FilterInfo = new ColumnFilterInfo(
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
    }
}