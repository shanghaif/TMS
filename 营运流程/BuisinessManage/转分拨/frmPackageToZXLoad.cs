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

namespace ZQTMS.UI
{
    public partial class frmPackageToZXLoad : BaseForm
    {
        private DataSet dataset1 = new DataSet();
        private DataSet dataset3 = new DataSet(), dsZXSite;
        GridHitInfo hitInfo = null;
        static frmPackageToZXLoad fsl;


        /// <summary>
        /// 获取窗体对像
        /// </summary>
        public static frmPackageToZXLoad Get_frmPackageToZXLoad { get { if (fsl == null || fsl.IsDisposed) fsl = new frmPackageToZXLoad(); return fsl; } }

        public frmPackageToZXLoad()
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
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_DEPARTURE_FB_LOAD");
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
        bool isCalculate = false;
        private void myGridControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GridViewMove.Move(myGridView1, dataset1, dataset3);
            isCalculate = false;
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

        private void w_send_load_Load(object sender, EventArgs e)
        {
            //bool isb = CommonClass.UserInfo.IsAutoBill;
            //string label = CommonClass.UserInfo.LabelName;
            //string enve = CommonClass.UserInfo.EnvelopeName;
            if (!CommonClass.FormSet(this, false, true)) return;
            CommonClass.GetGridViewColumns(myGridView1, false, myGridView2, myGridView3);
            GridOper.SetGridViewProperty(myGridView1, myGridView2, myGridView3);
            BarMagagerOper.SetBarPropertity(bar1, bar2); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1, myGridView2);
            // CommonClass.SetSite(AcceptSiteName, false);
            SetSite(AcceptSiteName, false);
            // CommonClass.SetWeb(AcceptWebName, false);
            SendDate.DateTime = CommonClass.gcdate;
            //车辆信息
            if (CommonClass.dsCar != null && CommonClass.dsCar.Tables.Count > 0) myGridControl3.DataSource = CommonClass.dsCar.Tables[0];

            //dsZXSite = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_ALL_COMPANY"));
            //string tmp = "";
            //if (dsZXSite != null && dsZXSite.Tables.Count > 0 && dsZXSite.Tables[0].Rows.Count > 0)
            //{
            //    foreach (DataRow dr in dsZXSite.Tables[0].Rows)
            //    {
            //        tmp = ConvertType.ToString(dr["companyid"]) + "|" + ConvertType.ToString(dr["gsjc"]);
            //        if (tmp != "" && !AcceptCompanyId.Properties.Items.Contains(tmp))
            //            AcceptCompanyId.Properties.Items.Add(tmp);
            //    }
            //}

            txtBatch.Text = GetMaxInOneVehicleFlag(CommonClass.UserInfo.SiteName);
        }

        public static void SetSite(ComboBoxEdit cb, bool isall)
        {
            //DataSet dsSite= CommonClass.dsSite;
            List<SqlPara> list = new List<SqlPara>();
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASSITE_101", list);
            DataSet dsSite = SqlHelper.GetDataSet(sps);

            if (dsSite == null || dsSite.Tables.Count == 0) return;

            try
            {
                string sql = "AllocateCompanyID like '%" + CommonClass.UserInfo.companyid + "%'";
                //DataRow[] drs = dsSite.Tables[0].Select(sql);
                //if (drs.Length <= 0) return;

                for (int i = 0; i < dsSite.Tables[0].Rows.Count; i++)
                {
                    if (dsSite.Tables[0].Rows[i]["AllocateCompanyID"].ToString().Contains(CommonClass.UserInfo.companyid))
                    {
                        cb.Properties.Items.Add(dsSite.Tables[0].Rows[i]["SiteName"].ToString());
                    }

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

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //string billnos = "";
            //for (int i = 0; i < myGridView2.RowCount; i++)
            //{ 
            //    string paymentMode=myGridView2.GetRowCellValue(i,"PaymentMode").ToString().Trim();
            //    if (paymentMode != "现付" && paymentMode != "提付" && paymentMode != "月结" && paymentMode != "短欠")
            //    {
            //        billnos = billnos + myGridView2.GetRowCellValue(i, "BillNo").ToString() + ",";
            //    }
            //}
            //if (billnos.Trim() != "")
            //{
            //    MsgBox.ShowOK("运单:" + billnos + ",付款方式不是[现付],[提付],[月结],[短欠],不能进行转分拨！");
            //    return;
            //}
            if (isCalculate == false)
            {
                MsgBox.ShowOK("请先在第①步中点计算结算费按钮，计算结算费再分拨！");
                return;
            }
            if (myGridView2.RowCount == 0)
            {
                MsgBox.ShowOK("请选择要分拨的清单!");
                xtraTabControl1.SelectedTabPage = tp1;
                return;
            }
            string site = AcceptSiteName.Text;
            if (site == "")
            {
                MsgBox.ShowOK("请选择接收站点!");
                AcceptSiteName.Focus();
                return;
            }
            string webName = AcceptWebName.Text;
            if (webName == "")
            {
                MsgBox.ShowOK("请选择网点!");
                AcceptWebName.Focus();
                return;
            }
            string carNo = CarNo.Text.Trim();
            if (carNo == "")
            {
                MsgBox.ShowOK("请填写车号!");
                CarNo.Focus();
                return;
            }
            string driverName = DriverName.Text.Trim();
            if (driverName == "")
            {
                MsgBox.ShowOK("请填写司机名称!");
                DriverName.Focus();
                return;
            }
            string driverPhone = DriverPhone.Text.Trim();
            if (driverPhone == "")
            {
                MsgBox.ShowOK("请填司机电话!");
                DriverPhone.Focus();
                return;
            }
            string BillNoStr = "";
            string DeliveryFeeStr = "";//送货费
            string TransferFeeStr = "";//中转费
            string TerminalOptFeeStr = "";//终端操作费
            string TaxStr = "";//结算税金
            string SupportValueStr = "";//保价费
            string StorageFeeStr = "";//进仓费
            string NoticeFeeStr = "";//控货费
            string HandleFeeStr = "";//装卸费
            string UpstairFeeStr = "";//上楼费
            string ReceiptFeeStr = "";//回单费

            string companyids = "";//公司ID ZAJ 2018-4-18
            string billNoMsg = "";


            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                BillNoStr += GridOper.GetRowCellValueString(myGridView2, i, "BillNo") + "@";
                DeliveryFeeStr += GridOper.GetRowCellValueString(myGridView2, i, "DeliveryFee") + "@";
                TransferFeeStr += GridOper.GetRowCellValueString(myGridView2, i, "TransferFee") + "@";
                TerminalOptFeeStr += GridOper.GetRowCellValueString(myGridView2, i, "TerminalOptFee") + "@";
                TaxStr += GridOper.GetRowCellValueString(myGridView2, i, "Tax_C") + "@";
                SupportValueStr += GridOper.GetRowCellValueString(myGridView2, i, "SupportValue_C") + "@";
                StorageFeeStr += GridOper.GetRowCellValueString(myGridView2, i, "StorageFee_C") + "@";
                NoticeFeeStr += GridOper.GetRowCellValueString(myGridView2, i, "NoticeFee_C") + "@";
                HandleFeeStr += GridOper.GetRowCellValueString(myGridView2, i, "HandleFee_C") + "@";
                UpstairFeeStr += GridOper.GetRowCellValueString(myGridView2, i, "UpstairFee_C") + "@";
                ReceiptFeeStr += GridOper.GetRowCellValueString(myGridView2, i, "ReceiptFee_C") + "@";
                companyids += GridOper.GetRowCellValueString(myGridView2, i, "companyid") + "@";
                if (Convert.ToInt32( GridOper.GetRowCellValueString(myGridView2, i, "TerminalOptFee"))== 0)
                {
                    billNoMsg += GridOper.GetRowCellValueString(myGridView2, i, "BillNo") + ",";
                }
            }
            if (BillNoStr == "") return;
            if (billNoMsg != "")//zaj 2018-6-4 限制无结算终端操作费，不能转分拨
            {
                MsgBox.ShowOK("运单号："+billNoMsg+",无结算终端操作费,请检查是否导入结算标准，如果有导入结算标准，仍然无结算，请退出lms系统重新登陆即可！");
                return;
            }
            if (MsgBox.ShowYesNo("确定分拨？") != DialogResult.Yes) return;
            //回单库存2018.5.10  zaj
            string BillNO = "";
            string ReceiptCondition = "";
            string HDBillno = "";
            string bb = "";
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                string tiaojian = myGridView2.GetRowCellValue(i, "ReceiptCondition").ToString();
                if (tiaojian != "" && tiaojian != "附清单")//tiaojian == "签回单"
                {
                    BillNO += myGridView2.GetRowCellValue(i, "BillNo") + "@";
                    ReceiptCondition += myGridView2.GetRowCellValue(i, "ReceiptCondition") + "@";
                }
            }
            frmReturnStockDBCK frm = new frmReturnStockDBCK();
            frm.Billno = BillNO;
            frm.ReceiptCondition = ReceiptCondition;
            frm.type = "分拨";
            frm.ShowDialog();
            HDBillno = frm.aa;
            bb = frm.bb;
            if (bb == "1")
            {
                return;
            }
            try
            {
                //hj申请改单未执行的不让转分波
                DataSet ds1 = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILLAPPLY_ZX", new List<SqlPara> { (new SqlPara("BillNoStr", BillNoStr)) }));
                if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (DataRow row in ds1.Tables[0].Rows)
                    {
                        sb.Append(row["BillNO"].ToString() + "\n");
                    }
                    MsgBox.ShowError("有运单改单申请还未执行，请执行后再转分波！单号如下：\n" + sb.ToString());
                    return;
                }
                string batch = GetMaxInOneVehicleFlag(CommonClass.UserInfo.SiteName);
                string db = CommonClass.UserInfo.UserDB.ToString();
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNoStr", BillNoStr));
                list.Add(new SqlPara("AcceptSiteName", site));
                list.Add(new SqlPara("AcceptWebName", webName));
                list.Add(new SqlPara("CarNo", carNo));
                list.Add(new SqlPara("DriverName", driverName));
                list.Add(new SqlPara("DriverPhone", driverPhone));
                list.Add(new SqlPara("Remark", Remark.Text.Trim()));
                list.Add(new SqlPara("Batch", batch));
                list.Add(new SqlPara("DeliveryFeeStr", DeliveryFeeStr));
                list.Add(new SqlPara("TransferFeeStr", TransferFeeStr));
                list.Add(new SqlPara("TerminalOptFeeStr", TerminalOptFeeStr));
                list.Add(new SqlPara("TaxStr", TaxStr));
                list.Add(new SqlPara("SupportValueStr", SupportValueStr));
                list.Add(new SqlPara("StorageFeeStr", StorageFeeStr));
                list.Add(new SqlPara("NoticeFeeStr", NoticeFeeStr));
                list.Add(new SqlPara("HandleFeeStr", HandleFeeStr));
                list.Add(new SqlPara("UpstairFeeStr", UpstairFeeStr));
                list.Add(new SqlPara("ReceiptFeeStr", ReceiptFeeStr));
                list.Add(new SqlPara("companyids", companyids));
                list.Add(new SqlPara("HDBillno", HDBillno));


                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_For_Allocate", list));

                if (ds == null || ds.Tables[0].Rows.Count <= 0) return;
                string dsJson = JsonConvert.SerializeObject(ds);
                string resultJson = string.Empty;
                bool istrue = DataOper.Compress(dsJson, ref resultJson);//压缩文件
                if (istrue)
                {
                    RequestModel<string> request = new RequestModel<string>();
                    request.Request = resultJson;
                    request.OperType = 0;
                    string json = JsonConvert.SerializeObject(request);
                    ResponseModelClone<string> result = HttpHelper.HttpPost(json, HttpHelper.urlAllocateToArtery);

                    //ResponseModelClone<string> result = HttpHelper.HttpPost(json, "http://localhost:42936/KDLMSService/AllocateToArtery");

                    // ResponseModelClone<string> result = HttpHelper.HttpPost(json, "http://192.168.16.112:99//KDLMSService/AllocateToArtery");


                    List<SqlPara> list1 = new List<SqlPara>();
                    list1.Add(new SqlPara("BillNoStr", BillNoStr));
                    list1.Add(new SqlPara("AcceptSiteName", site));
                    list1.Add(new SqlPara("AcceptWebName", webName));
                    list1.Add(new SqlPara("CarNo", carNo));
                    list1.Add(new SqlPara("DriverName", driverName));
                    list1.Add(new SqlPara("DriverPhone", driverPhone));
                    list1.Add(new SqlPara("Remark", Remark.Text.Trim()));
                    list1.Add(new SqlPara("Batch", batch));
                    // list1.Add(new SqlPara("Batch", batch));
                    //hj 结算费用20180410
                    list1.Add(new SqlPara("DeliveryFeeStr", DeliveryFeeStr));
                    list1.Add(new SqlPara("TransferFeeStr", TransferFeeStr));
                    list1.Add(new SqlPara("TerminalOptFeeStr", TerminalOptFeeStr));
                    list1.Add(new SqlPara("TaxStr", TaxStr));
                    list1.Add(new SqlPara("SupportValueStr", SupportValueStr));
                    list1.Add(new SqlPara("StorageFeeStr", StorageFeeStr));
                    list1.Add(new SqlPara("NoticeFeeStr", NoticeFeeStr));
                    list1.Add(new SqlPara("HandleFeeStr", HandleFeeStr));
                    list1.Add(new SqlPara("UpstairFeeStr", UpstairFeeStr));
                    list1.Add(new SqlPara("ReceiptFeeStr", ReceiptFeeStr));
                    list1.Add(new SqlPara("HDBillno", HDBillno));

                    if (result.State == "200")
                    {
                        try
                        {
                            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_PACKAGE_FB_TO_ZX", list1)) == 0)
                            {
                                #region 如果执行存储过程失败则回滚调用wcf存储过程更新的数据
                              
                                RequestModel<string> requestForCancel = new RequestModel<string>();
                                requestForCancel.Request = batch;
                                requestForCancel.OperType = 0;
                                string jsonForCancel = JsonConvert.SerializeObject(requestForCancel);
                                ResponseModelClone<string> resultForCancel = HttpHelper.HttpPost(jsonForCancel, HttpHelper.urlCancelAllocate);
                                #endregion

                                #region 记录日志
                                List<SqlPara> listLog = new List<SqlPara>();
                                listLog.Add(new SqlPara("BillNo", BillNoStr));
                                listLog.Add(new SqlPara("Batch", batch));
                                listLog.Add(new SqlPara("ErrorNode", "分拨执行本地数据库返回值为0!"));
                                listLog.Add(new SqlPara("ExceptMessage", result.Message));
                                SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                                SqlHelper.ExecteNonQuery(spsLog);
                                MsgBox.ShowOK("分拨失败！");
                                #endregion
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            #region 如果执行存储过程失败则回滚调用wcf存储过程更新的数据

                            RequestModel<string> requestForCancel = new RequestModel<string>();
                            requestForCancel.Request = batch;
                            requestForCancel.OperType = 0;
                            string jsonForCancel = JsonConvert.SerializeObject(requestForCancel);
                            ResponseModelClone<string> resultForCancel = HttpHelper.HttpPost(jsonForCancel, HttpHelper.urlCancelAllocate);
                            #endregion

                            #region 记录日志
                            List<SqlPara> listLog = new List<SqlPara>();
                            listLog.Add(new SqlPara("BillNo", BillNoStr));
                            listLog.Add(new SqlPara("Batch", batch));
                            listLog.Add(new SqlPara("ErrorNode", "分拨执行本地数据库异常!"));
                            listLog.Add(new SqlPara("ExceptMessage", ex.Message));
                            SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                            SqlHelper.ExecteNonQuery(spsLog);
                            MsgBox.ShowOK("分拨失败："+ex.Message);
                            return;
                            #endregion
                        }
                        dataset3.Tables[0].Rows.Clear();
                        SendDate.DateTime = CommonClass.gcdate;
                        AcceptSiteName.Text = AcceptWebName.Text = CarNo.Text = DriverName.Text = DriverPhone.Text = Remark.Text = "";
                        txtBatch.Text = GetMaxInOneVehicleFlag(CommonClass.UserInfo.SiteName);
                        MsgBox.ShowOK("分拨成功!");
                        CommonSyn.TraceSyn(null, BillNoStr, 17, "分拨到中心", 1, null, null);
                    }
                    else
                    {
                        MsgBox.ShowOK(result.Message);

                        #region 记录日志
                        List<SqlPara> listLog = new List<SqlPara>();
                        listLog.Add(new SqlPara("BillNo", BillNoStr));
                        listLog.Add(new SqlPara("Batch", batch));
                        listLog.Add(new SqlPara("ErrorNode", "分拨调用wcf失败"));
                        listLog.Add(new SqlPara("ExceptMessage",result.Message));
                        SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                        SqlHelper.ExecteNonQuery(spsLog);
                        #endregion
                    }
                }
                else
                {
                    MsgBox.ShowOK("转分拨压缩失败，请稍后再试！");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

 

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            getdata();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
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

        private void myGridControl3_Leave(object sender, EventArgs e)
        {
            myGridControl3.Visible = CarNo.Focused;
        }

        private void SetCarInfo()
        {
            int rowhandle = myGridView3.FocusedRowHandle;
            if (rowhandle < 0) return;

            DataRow dr = myGridView3.GetDataRow(rowhandle);
            if (dr == null) return;

            myGridControl3.Visible = false;
            CarNo.EditValue = dr["CarNo"];
            DriverName.EditValue = dr["DriverName"];
            DriverPhone.EditValue = dr["DriverPhone"];
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
            myGridControl3.Left = CarNo.Left;
            myGridControl3.Top = CarNo.Top + CarNo.Height + 2;
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

        private void 查询运单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            CommonClass.ShowBillSearch(GridOper.GetRowCellValueString(myGridView1, rowhandle, "BillNo"));
        }

        private void AcceptSiteName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //AcceptWebName.Properties.Items.Clear();
            //AcceptWebName.Text = "";
            //if (dsZXSite == null || dsZXSite.Tables.Count < 3 || dsZXSite.Tables[2].Rows.Count == 0) return;

            //string siteName = AcceptSiteName.Text, companyid = "";
            //DataRow[] drs = dsZXSite.Tables[2].Select(string.Format("companyid='{0}' AND SiteName='{1}'", companyid, siteName));
            //if (drs == null || drs.Length == 0) return;

            //foreach (DataRow dr in drs)
            //{
            //    companyid = ConvertType.ToString(dr["WebName"]);
            //    if (companyid != "" && !AcceptWebName.Properties.Items.Contains(companyid))
            //        AcceptWebName.Properties.Items.Add(companyid);
            //}
            //AcceptWebName.SelectedIndex = 0;
        }

        private void AcceptSiteName_TextChanged(object sender, EventArgs e)
        {
            AcceptWebName.Properties.Items.Clear();
            // CommonClass.SetWeb(AcceptWebName, AcceptSiteName.Text.Trim(), false);
            SetWeb1(AcceptWebName, AcceptSiteName.Text.Trim(), false);
            AcceptWebName.SelectedIndex = 0;

        }

        public static void SetWeb1(ComboBoxEdit cb, string SiteName, bool isall)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASWEB_101", list);
                DataSet dsWeb = SqlHelper.GetDataSet(sps);
                if (dsWeb == null || dsWeb.Tables.Count == 0) return;

                if (SiteName == "全部") SiteName = "%%";
                DataRow[] dr = dsWeb.Tables[0].Select("SiteName like '" + SiteName + "' and AllocateCompanyID like '%" + CommonClass.UserInfo.companyid + "%'");
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
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }


        public string GetMaxInOneVehicleFlag(string bsite)
        {
            DataSet dsflag = new DataSet();
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bsite", bsite));
                list.Add(new SqlPara("bcode", CommonClass.UserInfo.LoginSiteCode));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_AllocateBatch", list);
                dsflag = SqlHelper.GetDataSet(sps);

                return ConvertType.ToString(dsflag.Tables[0].Rows[0][0]);
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK("产生发车批次失败：\r\n" + ex.Message);
                return "";
            }
        }

        private void barBtnComputer_ItemClick(object sender, ItemClickEventArgs e)
        {
            int count = myGridView2.RowCount;
            for (int i = 0; i < count; i++)
            {
                CalculateFee(i);
            }
            MsgBox.ShowOK("计算成功");
            isCalculate = true;
        }

        private void CalculateFee(int i)
        {
            decimal TransferFee = 0;//结算中转费
            decimal DeliveryFee = 0;//结算送货费
            decimal Tax_C = 0;//结算税金
            decimal TerminalOptFee = 0;//结算终端操作费
            decimal SupportValue_C = 0;//结算保价费
            decimal StorageFee_C = 0;//结算进仓费
            decimal NoticeFee_C = 0;//控货费
            decimal HandleFee_C = 0;//装卸费
            decimal UpstairFee_C = 0;//上楼费
            decimal ReceiptFee_C = 0;//回单费
            decimal AgentFee_C = 0;//代收手续费 zaj 2018-4-13

            string billNo = myGridView2.GetRowCellValue(i, "BillNo").ToString();
            string transitMode = "中强专线";
            string receivProvince = myGridView2.GetRowCellValue(i, "ReceivProvince").ToString();
            string receivCity = myGridView2.GetRowCellValue(i, "ReceivCity").ToString();
            string receivArea = myGridView2.GetRowCellValue(i, "ReceivArea").ToString();
            string receivStreet = myGridView2.GetRowCellValue(i, "ReceivStreet").ToString();
            string transferSite = myGridView2.GetRowCellValue(i, "TransferSite").ToString();
            decimal feeWeight = ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "FeeWeight").ToString());
            decimal feeVolume = ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "FeeVolume").ToString());
            string Package = myGridView2.GetRowCellValue(i, "Package").ToString();
            int AlienGoods = Convert.ToInt32(myGridView2.GetRowCellValue(i, "AlienGoods").ToString().Trim() == "" ? "0" : myGridView2.GetRowCellValue(i, "AlienGoods").ToString().Trim());
            decimal OperationWeight = ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "OperationWeight").ToString());
            string TransferMode = myGridView2.GetRowCellValue(i, "TransferMode").ToString();
            string FeeType = myGridView2.GetRowCellValue(i, "FeeType").ToString();
            string PickGoodsSite = myGridView2.GetRowCellValue(i, "PickGoodsSite").ToString();

            int IsInvoice = Convert.ToInt32(myGridView2.GetRowCellValue(i, "IsInvoice").ToString().Trim() == "" ? "0" : myGridView2.GetRowCellValue(i, "IsInvoice").ToString().Trim());
            int IsSupportValue = Convert.ToInt32(myGridView2.GetRowCellValue(i, "IsSupportValue").ToString().Trim() == "" ? "0" : myGridView2.GetRowCellValue(i, "IsSupportValue").ToString().Trim());
            int PreciousGoods = Convert.ToInt32(myGridView2.GetRowCellValue(i, "PreciousGoods").ToString().Trim() == "" ? "0" : myGridView2.GetRowCellValue(i, "PreciousGoods").ToString().Trim());
            int IsStorageFee = Convert.ToInt32(myGridView2.GetRowCellValue(i, "IsStorageFee").ToString().Trim() == "" ? "0" : myGridView2.GetRowCellValue(i, "IsStorageFee").ToString().Trim());
            int NoticeState = Convert.ToInt32(myGridView2.GetRowCellValue(i, "NoticeState").ToString().Trim() == "" ? "0" : myGridView2.GetRowCellValue(i, "NoticeState").ToString().Trim());
            int IsHandleFee = Convert.ToInt32(myGridView2.GetRowCellValue(i, "IsHandleFee").ToString().Trim() == "" ? "0" : myGridView2.GetRowCellValue(i, "IsHandleFee").ToString().Trim());
            int IsUpstairFee = Convert.ToInt32(myGridView2.GetRowCellValue(i, "IsUpstairFee").ToString().Trim() == "" ? "0" : myGridView2.GetRowCellValue(i, "IsUpstairFee").ToString().Trim());
            int IsReceiptFee = Convert.ToInt32(myGridView2.GetRowCellValue(i, "IsReceiptFee").ToString().Trim() == "" ? "0" : myGridView2.GetRowCellValue(i, "IsReceiptFee").ToString().Trim());
            int IsAgentFee = Convert.ToInt32(myGridView2.GetRowCellValue(i, "IsAgentFee").ToString().Trim() == "" ? "0" : myGridView2.GetRowCellValue(i, "IsAgentFee").ToString().Trim());//2018-4-13 zaj


            decimal CollectionPay = ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "CollectionPay").ToString());
            decimal Tax = ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "Tax").ToString());
            decimal PaymentAmout = ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "PaymentAmout").ToString());
            string companyid = myGridView2.GetRowCellValue(i, "companyid").ToString();


            //if (TransferMode == "自提" && receivStreet == "")
            //{
            //    List<SqlPara> list = new List<SqlPara>();
            //    list.Add(new SqlPara("ReceivProvince", receivProvince));
            //    list.Add(new SqlPara("ReceivCity", receivCity));
            //    list.Add(new SqlPara("ReceivArea", receivArea));
            //    list.Add(new SqlPara("TransferSite", transferSite));
            //    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ADDRESS_ZT", list);
            //    DataSet ds = SqlHelper.GetDataSet(sps);
            //    if (ds != null && ds.Tables[0].Rows.Count > 0)
            //    {
            //        receivProvince = ds.Tables[0].Rows[0]["WebProvince"].ToString();
            //        receivCity = ds.Tables[0].Rows[0]["WebCity"].ToString();
            //        receivArea = ds.Tables[0].Rows[0]["WebArea"].ToString();
            //        receivStreet = ds.Tables[0].Rows[0]["WebStreet"].ToString();
            //    }

            //}

            #region  计算中转费    old code

            //DataRow[] drTransferFee = CommonClass.dsTransferFee.Tables[0].Select("TransferSite='" + transferSite + "' and ToProvince='" + receivProvince + "' and ToCity='" + receivCity + "' and ToArea='" + receivArea + "'");
            //if (drTransferFee.Length > 0)
            //{
            //    decimal HeavyPrice = ConvertType.ToDecimal(drTransferFee[0]["HeavyPrice"]);//重货
            //    decimal LightPrice = ConvertType.ToDecimal(drTransferFee[0]["LightPrice"]);//轻货
            //    decimal ParcelPriceMin = ConvertType.ToDecimal(drTransferFee[0]["ParcelPriceMin"]);//最低一票
            //    decimal TransferFeeAll = 0;
            //    decimal fee = Math.Max(feeWeight * HeavyPrice, feeVolume * LightPrice);
            //    if (receivProvince != "香港" && receivProvince != "海南省")
            //    {
            //        if (OperationWeight <= 300)
            //        {
            //            fee = fee * (decimal)1.5;
            //        }
            //        if (OperationWeight > 3000)
            //        {
            //            fee = fee * (decimal)0.8;
            //        }
            //    }
            //    if (Package != "纸箱" && Package != "纤袋" && Package != "膜")
            //    {
            //        TransferFeeAll += fee * (decimal)1.05;
            //    }
            //    else
            //    {
            //        TransferFeeAll += fee;
            //    }
            //    if (AlienGoods == 1)
            //    {
            //        TransferFeeAll = TransferFeeAll * (decimal)1.5;
            //    }
            //    TransferFee = Math.Max(TransferFeeAll, ParcelPriceMin);
            //    if (receivProvince == "香港")
            //    {
            //        string allFeeType = "";
            //        if (feeWeight > feeVolume / (decimal)3.8 * 1000)
            //        {
            //            allFeeType = "计重";
            //        }
            //        else
            //        {
            //            // 总体计方
            //            allFeeType = "计方";
            //        }
            //        if (allFeeType == "计重" && feeWeight < 200)
            //        {
            //            TransferFeeAll = ParcelPriceMin;
            //        }
            //        if (allFeeType == "计方" && feeVolume < (decimal)1.2)
            //        {
            //            TransferFeeAll = ParcelPriceMin;
            //        }
            //    }
            //}

            #endregion

            #region  计算中转费 new code
            //中强项目和司机直送不计算结算中转费
            if (!TransferMode.Equals("司机直送") && transitMode.Trim() != "中强项目")
            {
                DataRow[] drTransferFee = CommonClass.dsTransferFee.Tables[0].Select("TransferSite='" + transferSite + "' and ToProvince='"
                    + receivProvince + "' and ToCity='" + receivCity + "' and ToArea='" + receivArea + "' and TransitMode='" + transitMode + "' and companyid='" + companyid + "'");
                if (drTransferFee.Length > 0)
                {
                    decimal HeavyPrice = ConvertType.ToDecimal(drTransferFee[0]["HeavyPrice"]);//重货
                    decimal LightPrice = ConvertType.ToDecimal(drTransferFee[0]["LightPrice"]);//轻货
                    decimal ParcelPriceMin = ConvertType.ToDecimal(drTransferFee[0]["ParcelPriceMin"]);//最低一票
                    decimal TransferFeeAll = 0;
                    decimal allFeeWeight = 0;
                    decimal allFeeVolume = 0;
                    // for (int i = 0; i < RowCount; i++)
                    // {
                    decimal w = Math.Round(feeWeight, 2);
                    decimal v = Math.Round(feeVolume, 2);
                    //string Package = gridView2.GetRowCellValue(i, "Package").ToString();
                    decimal fee = Math.Max(w * HeavyPrice, v * LightPrice);
                    allFeeWeight += w;
                    allFeeVolume += v;
                    if (receivProvince != "香港" && receivProvince != "海南省")
                    {
                        // （300KG及以下的按标准上调1.5倍，3T以上（不含3T）打8折 lyj 2017/12/06
                        // 300KG及以下的按标准上调1.2倍，3T以上（不含3T）打9折 ccd 2018.03.15
                        if (OperationWeight <= 300)
                        {
                            fee = fee * (decimal)1.2;
                        }

                        if (OperationWeight > 3000)
                        {
                            fee = fee * (decimal)0.9;
                        }
                    }

                    if (Package != "纸箱" && Package != "纤袋" && Package != "膜")
                    {
                        // TransferFeeAll += fee * (decimal)1.1;
                        //zaj 2017-8-29 费用比例参数化
                        TransferFeeAll += fee * (decimal)1.05;
                    }
                    else
                    {
                        TransferFeeAll += fee;
                    }
                    // }
                    decimal acc = 0;

                    // 如果勾选了异形货，结算费用上浮50%
                    if (AlienGoods == 1)
                    {
                        TransferFeeAll = TransferFeeAll * (decimal)1.5;
                    }
                    acc = Math.Max(TransferFeeAll, ParcelPriceMin);

                    if (receivProvince.Trim() == "香港" && TransferMode.Trim() != "自提")
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
                    TransferFee = acc;
                    //gridView8.SetRowCellValue(0, "TransferFee", Math.Round(acc, 2));
                }
                else
                {
                    TransferFee = 0; //gridView8.SetRowCellValue(0, "TransferFee", 0);
                }
            }
            else
            {
                TransferFee = 0; //gridView8.SetRowCellValue(0, "TransferFee", 0);
            }
            #endregion

            #region 计算送货费
            if (receivProvince == "香港")
            {
                #region old code
                //if (TransferMode.Contains("送"))
                //{
                //    string sql = "Province='" + receivProvince
                //                    + "' and City='" + receivCity
                //                    + "' and Area='" + receivArea
                //                    + "' and Street='" + receivStreet
                //                    + "' and " + OperationWeight + ">=w1"
                //                    + " and " + OperationWeight + " <w2";
                //    DataRow[] drDeliveryFee = CommonClass.dsSendPriceHK.Tables[0].Select(sql);
                //    if (drDeliveryFee.Length > 0)
                //    {
                //        string fmtext = drDeliveryFee[0]["Expression"].ToString();
                //        double Additional = ConvertType.ToDouble(drDeliveryFee[0]["Additional"].ToString());
                //        fmtext = fmtext.Replace("w", OperationWeight.ToString());
                //        DataTable dt = new DataTable();
                //        DeliveryFee = Math.Round(decimal.Parse(dt.Compute(fmtext, "").ToString()), 2, MidpointRounding.AwayFromZero);
                //        if (FeeType == "计方")
                //        {
                //            DeliveryFee = DeliveryFee * (decimal)0.6;
                //        }
                //        DeliveryFee = DeliveryFee + (decimal)Additional;
                //        //香港送货费跟ZQTMS比缺少最低一票验证
                //    }
                //}
                #endregion
                #region new code
                if (TransferMode.Contains("送"))
                {
                    //当收货省为香港时，结算送货费计费不取系统结算重量，后台计算计费重量、计费体积按1：6折算取大值乘以结算送货费单价 2018.03.19 ccd
                    //decimal weight_temp = 0;
                    //decimal FeeWeight_temp = 0;
                    //decimal FeeVolumer_temp = 0;
                    // double DeliveryFee = 0;
                    double Additional = 0;
                    //for (int i = 0; i < RowCount; i++)
                    //{
                    decimal weight_temp = 0;
                    decimal FeeWeight = Math.Round(feeWeight, 2);
                    decimal FeeVolume = Math.Round(feeVolume, 2);

                    decimal FeeVolumer = 0;
                    // string Package = gridView2.GetRowCellValue(i, "Package").ToString();
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


                    string sql = "Province='" + receivProvince
                        + "' and City='" + receivCity
                        + "' and Area='" + receivArea
                        + "' and Street='" + receivStreet
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

                        DeliveryFee = DeliveryFee + Convert.ToDecimal(Math.Round(double.Parse(dt.Compute(fmtext, "").ToString()), 2, MidpointRounding.AwayFromZero));

                        // 当为轻货时，香港结算送货费按表达式计算完成后打6折，提出人：方俊杰
                        // 这里暂定只要包含计方就算作计方

                        //取消系统香港结算送货费的轻货打6折方案 2018.03.19 ccd


                    }

                    //}
                    List<SqlPara> list1 = new List<SqlPara>();//检查香港结算送货费是否小于最低一票--毛慧20171027
                    SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_basDeliveryFeeHK", list1);
                    DataTable dt1 = SqlHelper.GetDataTable(sps1);
                    DataRow[] arrs = dt1.Select("Province='" + receivProvince + "' and City='" + receivCity + "' and Area='" + receivArea + "' and Street='" + receivStreet + "'");
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
                    if (Convert.ToDecimal(temp) > DeliveryFee)
                    {
                        DeliveryFee = Convert.ToDecimal(temp);
                    }
                    if (AlienGoods == 1)//毛慧20171028--异形货上浮50%
                    {
                        DeliveryFee += DeliveryFee * (decimal)0.5;
                    }
                    //gridView1.SetRowCellValue(0, "DeliveryFee", DeliveryFee + Additional);
                }
                else
                {
                    DeliveryFee = 0;
                }
                #endregion

            }
            else
            {
                #region old code
                //decimal maxFee = 400;
                //if (transitMode == "中强快线")
                //{
                //    maxFee = maxFee * (decimal)1.25;
                //}
                //if (transitMode == "一票通")
                //{
                //    maxFee = maxFee * (decimal)1.05;
                //}
                //if (TransferMode == "送货")
                //{
                //    string sql = "Province='" + receivProvince
                //                    + "' and City='" + receivCity
                //                    + "' and Area='" + receivArea
                //                    + "' and Street='" + receivStreet
                //                    + "' and TransferMode='" + transitMode + "'";
                //    DataRow[] drDeliveryFee = CommonClass.dsSendPrice.Tables[0].Select(sql);
                //    if (drDeliveryFee == null || drDeliveryFee.Length <= 0)
                //    {
                //        sql = "Province='全国' and City='全国' and Area='全国' and Street='全国' and TransferMode='" + transitMode + "'";
                //        drDeliveryFee = CommonClass.dsSendPrice.Tables[0].Select(sql);
                //    }
                //    if (drDeliveryFee != null && drDeliveryFee.Length > 0)
                //    {
                //       DeliveryFee= getDeliveryFee(drDeliveryFee, OperationWeight,feeWeight,feeVolume,Package,FeeType);
                //    }
                //    if (AlienGoods == 1)
                //    {
                //        DeliveryFee = DeliveryFee * (decimal)1.5;
                //    }
                //    // 最低一票30， 最高400封顶
                //    if (DeliveryFee < 50)
                //    {
                //        DeliveryFee = 50;
                //    }
                //    if (DeliveryFee > maxFee)
                //    {
                //        DeliveryFee = maxFee;
                //    }
                //}
                //else if (TransferMode == "自提")
                //{
                //    if (TransferFee <= 0)
                //    {
                //        string sql = "Province='" + receivProvince
                //                             + "' and City='" + receivCity
                //                             + "' and Area='" + receivArea
                //                             + "' and Street='" + receivStreet
                //                             + "' and TransferMode='" + TransferMode + "'";
                //        DataRow[] drDeliveryFee = CommonClass.dsSendPrice.Tables[0].Select(sql);
                //        if (drDeliveryFee == null || drDeliveryFee.Length <= 0)
                //        {
                //            sql = "Province='全国' and City='全国' and Area='全国' and Street='全国' and TransferMode='" + TransferMode + "'";
                //            drDeliveryFee = CommonClass.dsSendPrice.Tables[0].Select(sql);
                //        }
                //        if (drDeliveryFee == null || drDeliveryFee.Length > 0)
                //        {
                //            DeliveryFee = getDeliveryFee(drDeliveryFee, OperationWeight, feeWeight, feeVolume, Package, FeeType);
                //        }
                //        if (AlienGoods == 1)
                //        {
                //            DeliveryFee = DeliveryFee * (decimal)1.5;
                //        }
                //        // 最低一票50， 最高400封顶
                //        if (DeliveryFee < 50)
                //        {
                //            DeliveryFee = 50;
                //        }
                //        if (DeliveryFee > maxFee)
                //        {
                //            DeliveryFee = maxFee;
                //        }
                //        DeliveryFee = DeliveryFee * (decimal)0.5;

                //    }
                //}
                #endregion

                #region new code
                decimal maxFee = 400;
                if (transferSite == "中强快线")
                {
                    maxFee = maxFee * (decimal)1.25;
                }
                if (transferSite == "一票通")
                {
                    maxFee = maxFee * (decimal)1.05;
                }
                if (TransferMode == "送货")
                {
                    //送货费调整 2018.03.15 ccd
                    //string TransitModeStr = TransitMode.Text.Trim();
                    string sql = "Province='" + receivProvince
                        + "' and City='" + receivCity
                        + "' and Area='" + receivArea
                        + "' and Street='" + receivStreet + "' and companyid='" + companyid + "'";
                    //+ "' and TransportMode='" + TransitModeStr + "'";


                    //DataRow[] drDeliveryFee = CommonClass.dsSendPriceNew.Tables[0].Select(sql);
                    //if (drDeliveryFee == null || drDeliveryFee.Length <= 0)
                    //{
                    //    sql = "Province='全国' and City='全国' and Area='全国' and Street='全国' and TransportMode='" + TransitModeStr + "'"; 
                    //    drDeliveryFee = CommonClass.dsSendPriceNew.Tables[0].Select(sql);
                    //}

                    DataRow[] drDeliveryFee = CommonClass.dsSendPrice.Tables[0].Select(sql);

                    if (drDeliveryFee != null && drDeliveryFee.Length > 0)
                    {
                        DeliveryFee = getDeliveryFee1(drDeliveryFee, OperationWeight);

                        //lyj 2017-10-16 一票通结算干线费上浮5%
                        if (transitMode == "一票通")
                        {
                            DeliveryFee = DeliveryFee + (DeliveryFee * Convert.ToDecimal("0.05"));
                        }
                        //lyj 2017-10-16 快线结算干线费上浮25%
                        if (transitMode == "中强快线")
                        {
                            DeliveryFee = DeliveryFee + (DeliveryFee * Convert.ToDecimal("0.25"));
                        }

                        // 如果勾选了异形货，结算费用上浮50%
                        if (AlienGoods == 1)
                        {
                            DeliveryFee = DeliveryFee * (decimal)1.5;
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
                        if (transitMode == "中强整车")
                        {
                            DeliveryFee = 0;
                        }
                        //中强项目结算送货费为0 2018.03.15 ccd
                        if (transitMode == "中强项目")
                        {
                            DeliveryFee = 0;
                        }

                        //gridView1.SetRowCellValue(0, "DeliveryFee", Math.Round(DeliveryFee, 2));
                    }
                    else
                    {
                        DeliveryFee = 0;
                    }
                }
                else if (TransferMode == "自提")
                {
                    // 当交接方式为自提，结算中转费为0才计算结算送货费  lyj
                    // decimal transferFee = ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "TransferFee"));
                    if (TransferFee <= 0)
                    {
                        // 检查目的网点是否被包含在参数表中心自提免费网点字段的值里
                        if (CommonClass.Arg.PickUpFreeWeb.Contains(PickGoodsSite) && PickGoodsSite != "")//CommonClass.Arg.PickUpFreeWeb.Contains(PickGoodsSite.Text.Trim())
                        {
                            DeliveryFee = 0;//gridView1.SetRowCellValue(0, "DeliveryFee", 0);
                        }
                        else
                        {
                            //送货费调整 2018.03.15 ccd
                            //string TransitModeStr = TransitMode.Text.Trim();
                            string sql = "Province='" + receivProvince
                                + "' and City='" + receivCity
                                + "' and Area='" + receivArea
                                + "' and Street='" + receivStreet + "' and companyid='" + companyid + "'";
                            //+ "' and TransportMode='" + TransitModeStr + "'";

                            //DataRow[] drDeliveryFee = CommonClass.dsSendPriceNew.Tables[0].Select(sql);
                            //if (drDeliveryFee == null || drDeliveryFee.Length <= 0)
                            //{
                            //    sql = "Province='全国' and City='全国' and Area='全国' and Street='全国' and TransportMode='" + TransitModeStr + "'";
                            //    drDeliveryFee = CommonClass.dsSendPriceNew.Tables[0].Select(sql);
                            //}
                            DataRow[] drDeliveryFee = CommonClass.dsSendPrice.Tables[0].Select(sql);

                            if (drDeliveryFee != null && drDeliveryFee.Length > 0)
                            {
                                DeliveryFee = getDeliveryFee1(drDeliveryFee, OperationWeight);

                                //lyj 2017-10-16 一票通结算干线费上浮5%
                                if (transitMode == "一票通")
                                {
                                    DeliveryFee = DeliveryFee + (DeliveryFee * Convert.ToDecimal("0.05"));
                                }
                                //lyj 2017-10-16 快线结算干线费上浮25%
                                if (transitMode == "中强快线")
                                {
                                    DeliveryFee = DeliveryFee + (DeliveryFee * Convert.ToDecimal("0.25"));
                                }

                                // 如果勾选了异形货，结算费用上浮50%
                                if (AlienGoods == 1)
                                {
                                    DeliveryFee = DeliveryFee * (decimal)1.5;
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



                                //非大车直达场站的自提货物按结算送货费标准结算一半送货费调整为按结算送货费标准结算1/4送货费 2018.03.15 ccd
                                //DeliveryFee = DeliveryFee * (decimal)0.5;
                                DeliveryFee = DeliveryFee * (decimal)0.25;

                                //中强整车结算送货费为0 2018.03.15 ccd
                                if (transitMode == "中强整车")
                                {
                                    DeliveryFee = 0;
                                }
                                //中强项目结算送货费为0 2018.03.15 ccd
                                if (transitMode == "中强项目")
                                {
                                    DeliveryFee = 0;
                                }

                                //  gridView1.SetRowCellValue(0, "DeliveryFee", Math.Round(DeliveryFee, 2));
                            }
                            else
                            {
                                DeliveryFee = 0; //gridView1.SetRowCellValue(0, "DeliveryFee", 0);
                            }

                        }
                    }
                    else
                    {
                        DeliveryFee = 0;//gridView1.SetRowCellValue(0, "DeliveryFee", 0);
                    }

                }
                else
                {
                    DeliveryFee = 0; //gridView1.SetRowCellValue(0, "DeliveryFee", 0);
                }

                #endregion

            }
            #region 作废
            //if (TransferMode == "送货" || TransferMode == "中心直送" || TransferMode == "自提")
            //{
            //    string sql = "Province='" + receivProvince
            //                + "' and City='" + receivCity
            //                + "' and Area='" + receivArea
            //                + "' and Street='" + receivStreet + "'";
            //    DataRow[] drDeliveryFee = CommonClass.dsSendPrice.Tables[0].Select(sql);
            //    if (drDeliveryFee.Length > 0)
            //    {
            //        decimal w0_200 = ConvertType.ToDecimal(drDeliveryFee[0]["w0_200"]);
            //        decimal w200_1000 = ConvertType.ToDecimal(drDeliveryFee[0]["w200_1000"]);
            //        decimal w1000_3000 = ConvertType.ToDecimal(drDeliveryFee[0]["w1000_3000"]);
            //        decimal w3000_5000 = ConvertType.ToDecimal(drDeliveryFee[0]["w3000_5000"]);
            //        decimal w5000_100000 = ConvertType.ToDecimal(drDeliveryFee[0]["w5000_100000"]);

            //        decimal Weight = OperationWeight;
            //        if (Weight >= 0 && Weight <= 200)
            //        {
            //            DeliveryFee = w0_200;
            //        }
            //        else if (Weight >= 200 && Weight <= 1000)
            //        {
            //            DeliveryFee = w200_1000;
            //        }
            //        else if (Weight >= 1000 && Weight <= 3000)
            //        {
            //            DeliveryFee = w1000_3000;
            //        }
            //        else if (Weight >= 3000 && Weight <= 5000)
            //        {
            //            DeliveryFee = w3000_5000;
            //        }
            //        else if (Weight > 5000)
            //        {
            //            DeliveryFee = w5000_100000;
            //        }
            //        if (Package != "纸箱" && Package != "纤袋" && Package != "膜")
            //        {
            //            DeliveryFee = DeliveryFee * (decimal)1.05;
            //        }
            //        if (AlienGoods == 1)
            //        {
            //            DeliveryFee = DeliveryFee * (decimal)1.5;
            //        }
            //        decimal maxFee = 400;
            //        if (DeliveryFee < 30)
            //        {
            //            DeliveryFee = 30;
            //        }
            //        if (DeliveryFee > maxFee)
            //        {
            //            DeliveryFee = 400;
            //        }
            //        if (TransferMode == "自提" && TransferFee > 0)
            //        {
            //            DeliveryFee = 0;
            //        }
            //        if (TransferMode == "自提" && TransferFee == 0)
            //        {
            //            DeliveryFee = DeliveryFee * (decimal)0.5;
            //        }
            //    }
            //}
            #endregion

            #endregion

            #region  终端操作费 old code
            //DataRow[] drTerminalOptFee = CommonClass.dsTerminalOptFee.Tables[0].Select("TransferSite='" + transferSite + "'");
            //if (drTerminalOptFee.Length > 0)
            //{
            //    decimal HeavyPrice = ConvertType.ToDecimal(drTerminalOptFee[0]["HeavyPrice"]);//重货
            //    decimal LightPrice = ConvertType.ToDecimal(drTerminalOptFee[0]["LightPrice"]);//轻货
            //    decimal ParcelPriceMin = ConvertType.ToDecimal(drTerminalOptFee[0]["ParcelPriceMin"]);//最低一票
            //    decimal Weight = OperationWeight;
            //    decimal acc = Math.Max(Weight * HeavyPrice, feeVolume * LightPrice);
            //    if (AlienGoods == 1)
            //    {
            //        acc = acc * (decimal)1.5;
            //    }
            //    acc = Math.Max(acc, ParcelPriceMin);
            //    TerminalOptFee = acc;
            //}
            #endregion

            #region 终端操作费 new code
            DataRow[] drTerminalOptFee = CommonClass.dsTerminalOptFee.Tables[0].Select("TransferSite='" + transferSite + "' and TransitMode='" + transitMode.Trim() + "' and companyid='" + companyid + "'");
            if (drTerminalOptFee.Length > 0 && transitMode != "中强项目" && transitMode != "中强城际"
                && TransferMode != "司机直送")
            {
                //  if ((transitMode == "一票通" && StartSite.Text == "深圳" && transferSite == "深圳"))
                //  {
                //    TransferFee = 0;//gridView8.SetRowCellValue(0, "TerminalOptFee", 0);
                // }
                // else
                // {
                decimal HeavyPrice = ConvertType.ToDecimal(drTerminalOptFee[0]["HeavyPrice"]);//重货
                decimal LightPrice = ConvertType.ToDecimal(drTerminalOptFee[0]["LightPrice"]);//轻货
                decimal ParcelPriceMin = ConvertType.ToDecimal(drTerminalOptFee[0]["ParcelPriceMin"]);//最低一票
                decimal acc = OperationWeight * HeavyPrice;



                // 如果勾选了异形货，结算费用上浮50%
                if (AlienGoods == 1)
                {
                    acc = acc * (decimal)1.5;
                }

                acc = Math.Max(acc, ParcelPriceMin);
                TerminalOptFee = acc;//gridView8.SetRowCellValue(0, "TerminalOptFee", Math.Round(acc, 2));
                //}
            }
            else
            {
                TerminalOptFee = 0;//gridView8.SetRowCellValue(0, "TerminalOptFee", 0);
            }

            #endregion

            #region  附加费
            #region 结算税金
            if (IsInvoice == 1)
            {
                DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='税金' and companyid='" + companyid + "'");
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
                DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='保价费' and companyid='" + companyid + "'");
                if (dr.Length > 0)
                {
                    decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);
                    decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]);
                    decimal SupportValue = ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "SupportValue").ToString());
                    SupportValue_C = Math.Round(Math.Max(InnerLowest, SupportValue * InnerStandard), 2);
                }
            }
            #endregion

            #region 进仓费
            if (IsStorageFee == 1)
            {
                DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='进仓费' and companyid='" + companyid + "'");
                if (dr.Length > 0)
                {
                    decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]); //最低一票金额
                    decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]); //结算标准 
                    decimal StorageFee = ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "StorageFee").ToString());

                    decimal OperationWeight_1 = OperationWeight; //结算重量

                    //结算标准 * 结算重量 >= 最低一票标准
                    StorageFee_C = Math.Round(Math.Max(InnerLowest, OperationWeight_1 * InnerStandard), 2);
                }
            }
            #endregion
            #region 控货费
            if (NoticeState == 1)
            {
                DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='控货费' and companyid='" + companyid + "'");
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
                DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='装卸费' and companyid='" + companyid + "'");
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
                DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='上楼费' and companyid='" + companyid + "'");
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
                DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='回单费' and companyid='" + companyid + "'");
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

            #region 代收货款结算
            if (IsAgentFee == 1)
            {
                DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='代收手续费' and companyid='" + companyid + "'");
                if (dr.Length > 0)
                {
                    decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);
                    decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]);
                    AgentFee_C = Math.Round(Math.Max(InnerLowest, CollectionPay * InnerStandard), 2);
                }
            }
            #endregion
            #endregion
            myGridView2.SetRowCellValue(i, "DeliveryFee", DeliveryFee);
            myGridView2.SetRowCellValue(i, "TransferFee", TransferFee);
            myGridView2.SetRowCellValue(i, "TerminalOptFee", TerminalOptFee);
            myGridView2.SetRowCellValue(i, "Tax_C", Tax_C);
            myGridView2.SetRowCellValue(i, "SupportValue_C", SupportValue_C);
            myGridView2.SetRowCellValue(i, "StorageFee_C", StorageFee_C);
            myGridView2.SetRowCellValue(i, "NoticeFee_C", NoticeFee_C);
            myGridView2.SetRowCellValue(i, "HandleFee_C", HandleFee_C);
            myGridView2.SetRowCellValue(i, "UpstairFee_C", UpstairFee_C);
            myGridView2.SetRowCellValue(i, "ReceiptFee_C", ReceiptFee_C);
            myGridView2.SetRowCellValue(i, "AgentFee_C", AgentFee_C);
        }


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
    }
}