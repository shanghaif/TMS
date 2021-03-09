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
        /// ��ȡ�������
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
                    myGridControl1.DoDragDrop("��Ҫ��ȥ��....", DragDropEffects.All);
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
                    myGridControl2.DoDragDrop("��Ҫ��ȥ��....", DragDropEffects.All);
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
            BarMagagerOper.SetBarPropertity(bar1, bar2); //����о���Ĺ���������������ʵ��
            GridOper.RestoreGridLayout(myGridView1, myGridView2);
            // CommonClass.SetSite(AcceptSiteName, false);
            SetSite(AcceptSiteName, false);
            // CommonClass.SetWeb(AcceptWebName, false);
            SendDate.DateTime = CommonClass.gcdate;
            //������Ϣ
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
                    cb.Properties.Items.Add("ȫ��");
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
            //    if (paymentMode != "�ָ�" && paymentMode != "�Ḷ" && paymentMode != "�½�" && paymentMode != "��Ƿ")
            //    {
            //        billnos = billnos + myGridView2.GetRowCellValue(i, "BillNo").ToString() + ",";
            //    }
            //}
            //if (billnos.Trim() != "")
            //{
            //    MsgBox.ShowOK("�˵�:" + billnos + ",���ʽ����[�ָ�],[�Ḷ],[�½�],[��Ƿ],���ܽ���ת�ֲ���");
            //    return;
            //}
            if (isCalculate == false)
            {
                MsgBox.ShowOK("�����ڵڢٲ��е�������Ѱ�ť�����������ٷֲ���");
                return;
            }
            if (myGridView2.RowCount == 0)
            {
                MsgBox.ShowOK("��ѡ��Ҫ�ֲ����嵥!");
                xtraTabControl1.SelectedTabPage = tp1;
                return;
            }
            string site = AcceptSiteName.Text;
            if (site == "")
            {
                MsgBox.ShowOK("��ѡ�����վ��!");
                AcceptSiteName.Focus();
                return;
            }
            string webName = AcceptWebName.Text;
            if (webName == "")
            {
                MsgBox.ShowOK("��ѡ������!");
                AcceptWebName.Focus();
                return;
            }
            string carNo = CarNo.Text.Trim();
            if (carNo == "")
            {
                MsgBox.ShowOK("����д����!");
                CarNo.Focus();
                return;
            }
            string driverName = DriverName.Text.Trim();
            if (driverName == "")
            {
                MsgBox.ShowOK("����д˾������!");
                DriverName.Focus();
                return;
            }
            string driverPhone = DriverPhone.Text.Trim();
            if (driverPhone == "")
            {
                MsgBox.ShowOK("����˾���绰!");
                DriverPhone.Focus();
                return;
            }
            string BillNoStr = "";
            string DeliveryFeeStr = "";//�ͻ���
            string TransferFeeStr = "";//��ת��
            string TerminalOptFeeStr = "";//�ն˲�����
            string TaxStr = "";//����˰��
            string SupportValueStr = "";//���۷�
            string StorageFeeStr = "";//���ַ�
            string NoticeFeeStr = "";//�ػ���
            string HandleFeeStr = "";//װж��
            string UpstairFeeStr = "";//��¥��
            string ReceiptFeeStr = "";//�ص���

            string companyids = "";//��˾ID ZAJ 2018-4-18
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
            if (billNoMsg != "")//zaj 2018-6-4 �����޽����ն˲����ѣ�����ת�ֲ�
            {
                MsgBox.ShowOK("�˵��ţ�"+billNoMsg+",�޽����ն˲�����,�����Ƿ�������׼������е�������׼����Ȼ�޽��㣬���˳�lmsϵͳ���µ�½���ɣ�");
                return;
            }
            if (MsgBox.ShowYesNo("ȷ���ֲ���") != DialogResult.Yes) return;
            //�ص����2018.5.10  zaj
            string BillNO = "";
            string ReceiptCondition = "";
            string HDBillno = "";
            string bb = "";
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                string tiaojian = myGridView2.GetRowCellValue(i, "ReceiptCondition").ToString();
                if (tiaojian != "" && tiaojian != "���嵥")//tiaojian == "ǩ�ص�"
                {
                    BillNO += myGridView2.GetRowCellValue(i, "BillNo") + "@";
                    ReceiptCondition += myGridView2.GetRowCellValue(i, "ReceiptCondition") + "@";
                }
            }
            frmReturnStockDBCK frm = new frmReturnStockDBCK();
            frm.Billno = BillNO;
            frm.ReceiptCondition = ReceiptCondition;
            frm.type = "�ֲ�";
            frm.ShowDialog();
            HDBillno = frm.aa;
            bb = frm.bb;
            if (bb == "1")
            {
                return;
            }
            try
            {
                //hj����ĵ�δִ�еĲ���ת�ֲ�
                DataSet ds1 = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILLAPPLY_ZX", new List<SqlPara> { (new SqlPara("BillNoStr", BillNoStr)) }));
                if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (DataRow row in ds1.Tables[0].Rows)
                    {
                        sb.Append(row["BillNO"].ToString() + "\n");
                    }
                    MsgBox.ShowError("���˵��ĵ����뻹δִ�У���ִ�к���ת�ֲ����������£�\n" + sb.ToString());
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
                bool istrue = DataOper.Compress(dsJson, ref resultJson);//ѹ���ļ�
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
                    //hj �������20180410
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
                                #region ���ִ�д洢����ʧ����ع�����wcf�洢���̸��µ�����
                              
                                RequestModel<string> requestForCancel = new RequestModel<string>();
                                requestForCancel.Request = batch;
                                requestForCancel.OperType = 0;
                                string jsonForCancel = JsonConvert.SerializeObject(requestForCancel);
                                ResponseModelClone<string> resultForCancel = HttpHelper.HttpPost(jsonForCancel, HttpHelper.urlCancelAllocate);
                                #endregion

                                #region ��¼��־
                                List<SqlPara> listLog = new List<SqlPara>();
                                listLog.Add(new SqlPara("BillNo", BillNoStr));
                                listLog.Add(new SqlPara("Batch", batch));
                                listLog.Add(new SqlPara("ErrorNode", "�ֲ�ִ�б������ݿⷵ��ֵΪ0!"));
                                listLog.Add(new SqlPara("ExceptMessage", result.Message));
                                SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                                SqlHelper.ExecteNonQuery(spsLog);
                                MsgBox.ShowOK("�ֲ�ʧ�ܣ�");
                                #endregion
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            #region ���ִ�д洢����ʧ����ع�����wcf�洢���̸��µ�����

                            RequestModel<string> requestForCancel = new RequestModel<string>();
                            requestForCancel.Request = batch;
                            requestForCancel.OperType = 0;
                            string jsonForCancel = JsonConvert.SerializeObject(requestForCancel);
                            ResponseModelClone<string> resultForCancel = HttpHelper.HttpPost(jsonForCancel, HttpHelper.urlCancelAllocate);
                            #endregion

                            #region ��¼��־
                            List<SqlPara> listLog = new List<SqlPara>();
                            listLog.Add(new SqlPara("BillNo", BillNoStr));
                            listLog.Add(new SqlPara("Batch", batch));
                            listLog.Add(new SqlPara("ErrorNode", "�ֲ�ִ�б������ݿ��쳣!"));
                            listLog.Add(new SqlPara("ExceptMessage", ex.Message));
                            SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                            SqlHelper.ExecteNonQuery(spsLog);
                            MsgBox.ShowOK("�ֲ�ʧ�ܣ�"+ex.Message);
                            return;
                            #endregion
                        }
                        dataset3.Tables[0].Rows.Clear();
                        SendDate.DateTime = CommonClass.gcdate;
                        AcceptSiteName.Text = AcceptWebName.Text = CarNo.Text = DriverName.Text = DriverPhone.Text = Remark.Text = "";
                        txtBatch.Text = GetMaxInOneVehicleFlag(CommonClass.UserInfo.SiteName);
                        MsgBox.ShowOK("�ֲ��ɹ�!");
                        CommonSyn.TraceSyn(null, BillNoStr, 17, "�ֲ�������", 1, null, null);
                    }
                    else
                    {
                        MsgBox.ShowOK(result.Message);

                        #region ��¼��־
                        List<SqlPara> listLog = new List<SqlPara>();
                        listLog.Add(new SqlPara("BillNo", BillNoStr));
                        listLog.Add(new SqlPara("Batch", batch));
                        listLog.Add(new SqlPara("ErrorNode", "�ֲ�����wcfʧ��"));
                        listLog.Add(new SqlPara("ExceptMessage",result.Message));
                        SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                        SqlHelper.ExecteNonQuery(spsLog);
                        #endregion
                    }
                }
                else
                {
                    MsgBox.ShowOK("ת�ֲ�ѹ��ʧ�ܣ����Ժ����ԣ�");
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
        /// ѡ���¼�����ʱ����û���ÿɽ��ն������������
        /// </summary>
        /// <param name="cb"></param>
        /// <param name="SiteName"></param>
        /// <param name="isall"></param>
        private void SetWeb(ComboBoxEdit cb, string SiteName, bool isall)
        {
            try
            {
                if (CommonClass.dsWeb == null || CommonClass.dsWeb.Tables.Count == 0) return;
                if (SiteName == "ȫ��") SiteName = "%%";
                DataRow[] dr = CommonClass.dsWeb.Tables[0].Select("SiteName like '" + SiteName + "' and IsAcceptejSend=1");
                cb.Properties.Items.Clear();
                cb.Text = "";
                for (int i = 0; i < dr.Length; i++)
                {
                    cb.Properties.Items.Add(dr[i]["WebName"]);
                }
                if (isall)
                {
                    cb.Properties.Items.Add("ȫ��");
                    cb.Text = "ȫ��";
                }
            }
            catch (Exception)
            {
                MsgBox.ShowOK("���ڼ��ػ������ϣ����Եȣ�");
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

        private void ��ѯ�˵�ToolStripMenuItem_Click(object sender, EventArgs e)
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

                if (SiteName == "ȫ��") SiteName = "%%";
                DataRow[] dr = dsWeb.Tables[0].Select("SiteName like '" + SiteName + "' and AllocateCompanyID like '%" + CommonClass.UserInfo.companyid + "%'");
                cb.Properties.Items.Clear();
                cb.Text = "";
                for (int i = 0; i < dr.Length; i++)
                {
                    cb.Properties.Items.Add(dr[i]["WebName"]);
                }
                if (isall)
                {
                    cb.Properties.Items.Add("ȫ��");
                    cb.Text = "ȫ��";
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
                MsgBox.ShowOK("������������ʧ�ܣ�\r\n" + ex.Message);
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
            MsgBox.ShowOK("����ɹ�");
            isCalculate = true;
        }

        private void CalculateFee(int i)
        {
            decimal TransferFee = 0;//������ת��
            decimal DeliveryFee = 0;//�����ͻ���
            decimal Tax_C = 0;//����˰��
            decimal TerminalOptFee = 0;//�����ն˲�����
            decimal SupportValue_C = 0;//���㱣�۷�
            decimal StorageFee_C = 0;//������ַ�
            decimal NoticeFee_C = 0;//�ػ���
            decimal HandleFee_C = 0;//װж��
            decimal UpstairFee_C = 0;//��¥��
            decimal ReceiptFee_C = 0;//�ص���
            decimal AgentFee_C = 0;//���������� zaj 2018-4-13

            string billNo = myGridView2.GetRowCellValue(i, "BillNo").ToString();
            string transitMode = "��ǿר��";
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


            //if (TransferMode == "����" && receivStreet == "")
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

            #region  ������ת��    old code

            //DataRow[] drTransferFee = CommonClass.dsTransferFee.Tables[0].Select("TransferSite='" + transferSite + "' and ToProvince='" + receivProvince + "' and ToCity='" + receivCity + "' and ToArea='" + receivArea + "'");
            //if (drTransferFee.Length > 0)
            //{
            //    decimal HeavyPrice = ConvertType.ToDecimal(drTransferFee[0]["HeavyPrice"]);//�ػ�
            //    decimal LightPrice = ConvertType.ToDecimal(drTransferFee[0]["LightPrice"]);//���
            //    decimal ParcelPriceMin = ConvertType.ToDecimal(drTransferFee[0]["ParcelPriceMin"]);//���һƱ
            //    decimal TransferFeeAll = 0;
            //    decimal fee = Math.Max(feeWeight * HeavyPrice, feeVolume * LightPrice);
            //    if (receivProvince != "���" && receivProvince != "����ʡ")
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
            //    if (Package != "ֽ��" && Package != "�˴�" && Package != "Ĥ")
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
            //    if (receivProvince == "���")
            //    {
            //        string allFeeType = "";
            //        if (feeWeight > feeVolume / (decimal)3.8 * 1000)
            //        {
            //            allFeeType = "����";
            //        }
            //        else
            //        {
            //            // ����Ʒ�
            //            allFeeType = "�Ʒ�";
            //        }
            //        if (allFeeType == "����" && feeWeight < 200)
            //        {
            //            TransferFeeAll = ParcelPriceMin;
            //        }
            //        if (allFeeType == "�Ʒ�" && feeVolume < (decimal)1.2)
            //        {
            //            TransferFeeAll = ParcelPriceMin;
            //        }
            //    }
            //}

            #endregion

            #region  ������ת�� new code
            //��ǿ��Ŀ��˾��ֱ�Ͳ����������ת��
            if (!TransferMode.Equals("˾��ֱ��") && transitMode.Trim() != "��ǿ��Ŀ")
            {
                DataRow[] drTransferFee = CommonClass.dsTransferFee.Tables[0].Select("TransferSite='" + transferSite + "' and ToProvince='"
                    + receivProvince + "' and ToCity='" + receivCity + "' and ToArea='" + receivArea + "' and TransitMode='" + transitMode + "' and companyid='" + companyid + "'");
                if (drTransferFee.Length > 0)
                {
                    decimal HeavyPrice = ConvertType.ToDecimal(drTransferFee[0]["HeavyPrice"]);//�ػ�
                    decimal LightPrice = ConvertType.ToDecimal(drTransferFee[0]["LightPrice"]);//���
                    decimal ParcelPriceMin = ConvertType.ToDecimal(drTransferFee[0]["ParcelPriceMin"]);//���һƱ
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
                    if (receivProvince != "���" && receivProvince != "����ʡ")
                    {
                        // ��300KG�����µİ���׼�ϵ�1.5����3T���ϣ�����3T����8�� lyj 2017/12/06
                        // 300KG�����µİ���׼�ϵ�1.2����3T���ϣ�����3T����9�� ccd 2018.03.15
                        if (OperationWeight <= 300)
                        {
                            fee = fee * (decimal)1.2;
                        }

                        if (OperationWeight > 3000)
                        {
                            fee = fee * (decimal)0.9;
                        }
                    }

                    if (Package != "ֽ��" && Package != "�˴�" && Package != "Ĥ")
                    {
                        // TransferFeeAll += fee * (decimal)1.1;
                        //zaj 2017-8-29 ���ñ���������
                        TransferFeeAll += fee * (decimal)1.05;
                    }
                    else
                    {
                        TransferFeeAll += fee;
                    }
                    // }
                    decimal acc = 0;

                    // �����ѡ�����λ�����������ϸ�50%
                    if (AlienGoods == 1)
                    {
                        TransferFeeAll = TransferFeeAll * (decimal)1.5;
                    }
                    acc = Math.Max(TransferFeeAll, ParcelPriceMin);

                    if (receivProvince.Trim() == "���" && TransferMode.Trim() != "����")
                    {
                        string allFeeType = "";
                        if (allFeeWeight > allFeeVolume / (decimal)3.8 * 1000)
                        {
                            // ��˵�������Ǽ���
                            allFeeType = "����";
                        }
                        else
                        {
                            // ����Ʒ�
                            allFeeType = "�Ʒ�";
                        }
                        if (allFeeType == "����" && allFeeWeight < 200)
                        {
                            acc = ParcelPriceMin;
                        }
                        if (allFeeType == "�Ʒ�" && allFeeVolume < (decimal)1.2)
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

            #region �����ͻ���
            if (receivProvince == "���")
            {
                #region old code
                //if (TransferMode.Contains("��"))
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
                //        if (FeeType == "�Ʒ�")
                //        {
                //            DeliveryFee = DeliveryFee * (decimal)0.6;
                //        }
                //        DeliveryFee = DeliveryFee + (decimal)Additional;
                //        //����ͻ��Ѹ�ZQTMS��ȱ�����һƱ��֤
                //    }
                //}
                #endregion
                #region new code
                if (TransferMode.Contains("��"))
                {
                    //���ջ�ʡΪ���ʱ�������ͻ��ѼƷѲ�ȡϵͳ������������̨����Ʒ��������Ʒ������1��6����ȡ��ֵ���Խ����ͻ��ѵ��� 2018.03.19 ccd
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
                    if (Package != "ֽ��" && Package != "�˴�" && Package != "Ĥ")
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

                        // ��Ϊ���ʱ����۽����ͻ��Ѱ����ʽ������ɺ��6�ۣ�����ˣ�������
                        // �����ݶ�ֻҪ�����Ʒ��������Ʒ�

                        //ȡ��ϵͳ��۽����ͻ��ѵ������6�۷��� 2018.03.19 ccd


                    }

                    //}
                    List<SqlPara> list1 = new List<SqlPara>();//�����۽����ͻ����Ƿ�С�����һƱ--ë��20171027
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
                    if (AlienGoods == 1)//ë��20171028--���λ��ϸ�50%
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
                //if (transitMode == "��ǿ����")
                //{
                //    maxFee = maxFee * (decimal)1.25;
                //}
                //if (transitMode == "һƱͨ")
                //{
                //    maxFee = maxFee * (decimal)1.05;
                //}
                //if (TransferMode == "�ͻ�")
                //{
                //    string sql = "Province='" + receivProvince
                //                    + "' and City='" + receivCity
                //                    + "' and Area='" + receivArea
                //                    + "' and Street='" + receivStreet
                //                    + "' and TransferMode='" + transitMode + "'";
                //    DataRow[] drDeliveryFee = CommonClass.dsSendPrice.Tables[0].Select(sql);
                //    if (drDeliveryFee == null || drDeliveryFee.Length <= 0)
                //    {
                //        sql = "Province='ȫ��' and City='ȫ��' and Area='ȫ��' and Street='ȫ��' and TransferMode='" + transitMode + "'";
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
                //    // ���һƱ30�� ���400�ⶥ
                //    if (DeliveryFee < 50)
                //    {
                //        DeliveryFee = 50;
                //    }
                //    if (DeliveryFee > maxFee)
                //    {
                //        DeliveryFee = maxFee;
                //    }
                //}
                //else if (TransferMode == "����")
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
                //            sql = "Province='ȫ��' and City='ȫ��' and Area='ȫ��' and Street='ȫ��' and TransferMode='" + TransferMode + "'";
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
                //        // ���һƱ50�� ���400�ⶥ
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
                if (transferSite == "��ǿ����")
                {
                    maxFee = maxFee * (decimal)1.25;
                }
                if (transferSite == "һƱͨ")
                {
                    maxFee = maxFee * (decimal)1.05;
                }
                if (TransferMode == "�ͻ�")
                {
                    //�ͻ��ѵ��� 2018.03.15 ccd
                    //string TransitModeStr = TransitMode.Text.Trim();
                    string sql = "Province='" + receivProvince
                        + "' and City='" + receivCity
                        + "' and Area='" + receivArea
                        + "' and Street='" + receivStreet + "' and companyid='" + companyid + "'";
                    //+ "' and TransportMode='" + TransitModeStr + "'";


                    //DataRow[] drDeliveryFee = CommonClass.dsSendPriceNew.Tables[0].Select(sql);
                    //if (drDeliveryFee == null || drDeliveryFee.Length <= 0)
                    //{
                    //    sql = "Province='ȫ��' and City='ȫ��' and Area='ȫ��' and Street='ȫ��' and TransportMode='" + TransitModeStr + "'"; 
                    //    drDeliveryFee = CommonClass.dsSendPriceNew.Tables[0].Select(sql);
                    //}

                    DataRow[] drDeliveryFee = CommonClass.dsSendPrice.Tables[0].Select(sql);

                    if (drDeliveryFee != null && drDeliveryFee.Length > 0)
                    {
                        DeliveryFee = getDeliveryFee1(drDeliveryFee, OperationWeight);

                        //lyj 2017-10-16 һƱͨ������߷��ϸ�5%
                        if (transitMode == "һƱͨ")
                        {
                            DeliveryFee = DeliveryFee + (DeliveryFee * Convert.ToDecimal("0.05"));
                        }
                        //lyj 2017-10-16 ���߽�����߷��ϸ�25%
                        if (transitMode == "��ǿ����")
                        {
                            DeliveryFee = DeliveryFee + (DeliveryFee * Convert.ToDecimal("0.25"));
                        }

                        // �����ѡ�����λ�����������ϸ�50%
                        if (AlienGoods == 1)
                        {
                            DeliveryFee = DeliveryFee * (decimal)1.5;
                        }

                        // ���һƱ30�� ���400�ⶥ
                        //if (DeliveryFee < 30)
                        //{
                        //    DeliveryFee = 30;
                        //}
                        //if (DeliveryFee > maxFee)
                        //{
                        //    DeliveryFee = maxFee;
                        //}

                        //��ǿ���������ͻ���Ϊ0 2018.03.15 ccd
                        if (transitMode == "��ǿ����")
                        {
                            DeliveryFee = 0;
                        }
                        //��ǿ��Ŀ�����ͻ���Ϊ0 2018.03.15 ccd
                        if (transitMode == "��ǿ��Ŀ")
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
                else if (TransferMode == "����")
                {
                    // �����ӷ�ʽΪ���ᣬ������ת��Ϊ0�ż�������ͻ���  lyj
                    // decimal transferFee = ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "TransferFee"));
                    if (TransferFee <= 0)
                    {
                        // ���Ŀ�������Ƿ񱻰����ڲ���������������������ֶε�ֵ��
                        if (CommonClass.Arg.PickUpFreeWeb.Contains(PickGoodsSite) && PickGoodsSite != "")//CommonClass.Arg.PickUpFreeWeb.Contains(PickGoodsSite.Text.Trim())
                        {
                            DeliveryFee = 0;//gridView1.SetRowCellValue(0, "DeliveryFee", 0);
                        }
                        else
                        {
                            //�ͻ��ѵ��� 2018.03.15 ccd
                            //string TransitModeStr = TransitMode.Text.Trim();
                            string sql = "Province='" + receivProvince
                                + "' and City='" + receivCity
                                + "' and Area='" + receivArea
                                + "' and Street='" + receivStreet + "' and companyid='" + companyid + "'";
                            //+ "' and TransportMode='" + TransitModeStr + "'";

                            //DataRow[] drDeliveryFee = CommonClass.dsSendPriceNew.Tables[0].Select(sql);
                            //if (drDeliveryFee == null || drDeliveryFee.Length <= 0)
                            //{
                            //    sql = "Province='ȫ��' and City='ȫ��' and Area='ȫ��' and Street='ȫ��' and TransportMode='" + TransitModeStr + "'";
                            //    drDeliveryFee = CommonClass.dsSendPriceNew.Tables[0].Select(sql);
                            //}
                            DataRow[] drDeliveryFee = CommonClass.dsSendPrice.Tables[0].Select(sql);

                            if (drDeliveryFee != null && drDeliveryFee.Length > 0)
                            {
                                DeliveryFee = getDeliveryFee1(drDeliveryFee, OperationWeight);

                                //lyj 2017-10-16 һƱͨ������߷��ϸ�5%
                                if (transitMode == "һƱͨ")
                                {
                                    DeliveryFee = DeliveryFee + (DeliveryFee * Convert.ToDecimal("0.05"));
                                }
                                //lyj 2017-10-16 ���߽�����߷��ϸ�25%
                                if (transitMode == "��ǿ����")
                                {
                                    DeliveryFee = DeliveryFee + (DeliveryFee * Convert.ToDecimal("0.25"));
                                }

                                // �����ѡ�����λ�����������ϸ�50%
                                if (AlienGoods == 1)
                                {
                                    DeliveryFee = DeliveryFee * (decimal)1.5;
                                }

                                // ���һƱ30�� ���400�ⶥ
                                //if (DeliveryFee < 30)
                                //{
                                //    DeliveryFee = 30;
                                //}
                                //if (DeliveryFee > maxFee)
                                //{
                                //    DeliveryFee = maxFee;
                                //}



                                //�Ǵ�ֱ�ﳡվ��������ﰴ�����ͻ��ѱ�׼����һ���ͻ��ѵ���Ϊ�������ͻ��ѱ�׼����1/4�ͻ��� 2018.03.15 ccd
                                //DeliveryFee = DeliveryFee * (decimal)0.5;
                                DeliveryFee = DeliveryFee * (decimal)0.25;

                                //��ǿ���������ͻ���Ϊ0 2018.03.15 ccd
                                if (transitMode == "��ǿ����")
                                {
                                    DeliveryFee = 0;
                                }
                                //��ǿ��Ŀ�����ͻ���Ϊ0 2018.03.15 ccd
                                if (transitMode == "��ǿ��Ŀ")
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
            #region ����
            //if (TransferMode == "�ͻ�" || TransferMode == "����ֱ��" || TransferMode == "����")
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
            //        if (Package != "ֽ��" && Package != "�˴�" && Package != "Ĥ")
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
            //        if (TransferMode == "����" && TransferFee > 0)
            //        {
            //            DeliveryFee = 0;
            //        }
            //        if (TransferMode == "����" && TransferFee == 0)
            //        {
            //            DeliveryFee = DeliveryFee * (decimal)0.5;
            //        }
            //    }
            //}
            #endregion

            #endregion

            #region  �ն˲����� old code
            //DataRow[] drTerminalOptFee = CommonClass.dsTerminalOptFee.Tables[0].Select("TransferSite='" + transferSite + "'");
            //if (drTerminalOptFee.Length > 0)
            //{
            //    decimal HeavyPrice = ConvertType.ToDecimal(drTerminalOptFee[0]["HeavyPrice"]);//�ػ�
            //    decimal LightPrice = ConvertType.ToDecimal(drTerminalOptFee[0]["LightPrice"]);//���
            //    decimal ParcelPriceMin = ConvertType.ToDecimal(drTerminalOptFee[0]["ParcelPriceMin"]);//���һƱ
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

            #region �ն˲����� new code
            DataRow[] drTerminalOptFee = CommonClass.dsTerminalOptFee.Tables[0].Select("TransferSite='" + transferSite + "' and TransitMode='" + transitMode.Trim() + "' and companyid='" + companyid + "'");
            if (drTerminalOptFee.Length > 0 && transitMode != "��ǿ��Ŀ" && transitMode != "��ǿ�Ǽ�"
                && TransferMode != "˾��ֱ��")
            {
                //  if ((transitMode == "һƱͨ" && StartSite.Text == "����" && transferSite == "����"))
                //  {
                //    TransferFee = 0;//gridView8.SetRowCellValue(0, "TerminalOptFee", 0);
                // }
                // else
                // {
                decimal HeavyPrice = ConvertType.ToDecimal(drTerminalOptFee[0]["HeavyPrice"]);//�ػ�
                decimal LightPrice = ConvertType.ToDecimal(drTerminalOptFee[0]["LightPrice"]);//���
                decimal ParcelPriceMin = ConvertType.ToDecimal(drTerminalOptFee[0]["ParcelPriceMin"]);//���һƱ
                decimal acc = OperationWeight * HeavyPrice;



                // �����ѡ�����λ�����������ϸ�50%
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

            #region  ���ӷ�
            #region ����˰��
            if (IsInvoice == 1)
            {
                DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='˰��' and companyid='" + companyid + "'");
                if (dr.Length > 0)
                {
                    decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);
                    decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]);
                    //˰���㷨  ���˷�-���ջ���-˰������ DiscountTransfer
                    decimal Tax1 = PaymentAmout - CollectionPay - Tax;
                    Tax_C = Math.Round(Math.Max(InnerLowest, Tax1 * InnerStandard), 2);
                }
            }
            #endregion
            #region  ���۷�
            if (IsSupportValue == 1)
            {
                DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='���۷�' and companyid='" + companyid + "'");
                if (dr.Length > 0)
                {
                    decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);
                    decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]);
                    decimal SupportValue = ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "SupportValue").ToString());
                    SupportValue_C = Math.Round(Math.Max(InnerLowest, SupportValue * InnerStandard), 2);
                }
            }
            #endregion

            #region ���ַ�
            if (IsStorageFee == 1)
            {
                DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='���ַ�' and companyid='" + companyid + "'");
                if (dr.Length > 0)
                {
                    decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]); //���һƱ���
                    decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]); //�����׼ 
                    decimal StorageFee = ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "StorageFee").ToString());

                    decimal OperationWeight_1 = OperationWeight; //��������

                    //�����׼ * �������� >= ���һƱ��׼
                    StorageFee_C = Math.Round(Math.Max(InnerLowest, OperationWeight_1 * InnerStandard), 2);
                }
            }
            #endregion
            #region �ػ���
            if (NoticeState == 1)
            {
                DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='�ػ���' and companyid='" + companyid + "'");
                if (dr.Length > 0)
                {
                    //�ػ��ѷ� ���10ԪһƱ 
                    decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);
                    NoticeFee_C = InnerLowest;

                }
            }
            #endregion
            #region װж��
            if (IsHandleFee == 1)
            {
                DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='װж��' and companyid='" + companyid + "'");
                if (dr.Length > 0)
                {
                    decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);
                    decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]);
                    //decimal HandleFee = ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "HandleFee"));

                    decimal OperationWeight_1 = OperationWeight; //��������             
                    HandleFee_C = Math.Round(Math.Max(InnerLowest, (OperationWeight_1 * InnerStandard) / 1000), 2);
                }
            }
            #endregion

            #region ��¥��
            if (IsUpstairFee == 1)
            {
                DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='��¥��' and companyid='" + companyid + "'");
                if (dr.Length > 0)
                {
                    decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]); //���һƱ���
                    decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]); //�����׼ 

                    decimal OperationWeight_1 = OperationWeight; //��������

                    //�����׼ * �������� >= ���һƱ��׼
                    UpstairFee_C = Math.Round(Math.Max(InnerLowest, (OperationWeight_1 * InnerStandard) / 1000), 2);
                }
            }
            #endregion
            #region �ص���
            if (IsReceiptFee == 1)
            {
                DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='�ص���' and companyid='" + companyid + "'");
                if (dr.Length > 0)
                {

                    decimal InnerLowest = ConvertType.ToDecimal(dr[0]["InnerLowest"]);
                    decimal InnerStandard = ConvertType.ToDecimal(dr[0]["InnerStandard"]);
                    //if (ReceiptFee > 0)
                    //{
                    //�ص��� ���5ԪһƱ
                    //decimal ReceiptFee_C = Math.Round(Math.Max(InnerLowest, ReceiptFee * InnerStandard), 2);
                    ReceiptFee_C = InnerLowest;
                }

            }
            #endregion

            #region ���ջ������
            if (IsAgentFee == 1)
            {
                DataRow[] dr = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='����������' and companyid='" + companyid + "'");
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
        /// ��������ͻ��� 2018.03.15 ccd
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
            decimal w8000_ = ConvertType.ToDecimal(drDeliveryFee[0]["w8000_"]);//8������


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
        /// ��������ͻ���
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

            if (Package != "ֽ��" && Package != "�˴�" && Package != "Ĥ")
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

            //if (FeeType == "����")
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