﻿using System;
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

namespace ZQTMS.UI
{
    public partial class frmWayBillAdd_3PL : BaseForm
    {
        public frmWayBillAdd_3PL()
        {
            InitializeComponent();
        }

        public frmWayBillAdd_3PL(string unum)
        {
            InitializeComponent();
            Upd_Num = int.Parse(unum);
            this.Text = "改单申请";
        }

        public int isModify = 0;
        public string BillNO = "";
        public DataRow dr;
        public int Upd_Num = 0;//1 改单申请 0开单
        static frmWayBillAdd_3PL fwb;
        private int RowCount = 1;
        private DataSet dsModified = new DataSet();

        public static frmWayBillAdd_3PL Get_frmWayBillAdd { get { if (fwb == null || fwb.IsDisposed)fwb = new frmWayBillAdd_3PL(); return fwb; } }

        private void comboBoxEdit10_SelectedValueChanged(object sender, EventArgs e)
        {
            NowPay.Text = "";
            FetchPay.Text = "";
            MonthPay.Text = "";
            ShortOwePay.Text = "";
            BefArrivalPay.Text = "";

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
                ShortOwePay.Enabled = true;
                BefArrivalPay.Enabled = true;

            }
            else
            {
                NowPay.Enabled = false;
                FetchPay.Enabled = false;
                MonthPay.Enabled = false;
                ShortOwePay.Enabled = false;
                BefArrivalPay.Enabled = false;
            }

        }

        private void comboBoxEdit4_Enter(object sender, EventArgs e)
        {
            gcjiehuodanhao.Left = ReceivMode.Left;
            gcjiehuodanhao.Top = ReceivMode.Top + ReceivMode.Height;
            gcjiehuodanhao.Visible = true;
            gcjiehuodanhao.BringToFront();
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



        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void textEdit9_Enter(object sender, EventArgs e)
        {
            gridControl8.Left = ConsignorCompany.Left;
            gridControl8.Top = ConsignorCompany.Top + ConsignorCompany.Height;
            gridControl8.Visible = true;
            gridControl8.BringToFront();
        }

        private void textEdit9_Leave(object sender, EventArgs e)
        {
            if (!gridControl8.Focused)
            {
                gridControl8.Visible = false;
            }
        }

        private void textEdit14_Enter(object sender, EventArgs e)
        {
            gridControl7.Left = ConsigneeCompany.Left;
            gridControl7.Top = ConsigneeCompany.Top + ConsigneeCompany.Height;
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
            #region 加载检测
            if (CommonClass.dsSendPrice.Tables.Count == 0
                || CommonClass.dsMainlineFee.Tables.Count == 0
                || CommonClass.dsDepartureOptFee.Tables.Count == 0
                || CommonClass.dsDepartureAllotFee.Tables.Count == 0
                || CommonClass.dsTransferFee.Tables.Count == 0
                || CommonClass.dsTerminalOptFee.Tables.Count == 0
                || CommonClass.dsTerminalAllotFee.Tables.Count == 0
                || CommonClass.UserInfo.UserName == null
                || CommonClass.Arg.TransferMode == null)
            {
                MsgBox.ShowOK("正在加载基础资料，请稍等！");
                this.Close();
                return;
            }
            #endregion

            if (Upd_Num == 1)//改单申请过来的
            {
                lb_Upd_BillNO.Visible = true;
                Upd_BillNO.Visible = true;
                BillNo.Enabled = false;
            }
            else
            {
                lb_Upd_BillNO.Visible = false;
                Upd_BillNO.Visible = false;
                BillNo.Enabled = true;
            }
            //BillNo.Enabled = false;
            //BillNo.Text = CommonClass.gcdate.ToString("yyMMddHHMMsss") + new Random().Next(11, 99);
            DraftGUID.Text = Guid.NewGuid().ToString();
            //BarMagagerOper.SetBarPropertity(bar2);
            SetProvince();

            #region 货物信息
            DataTable DtView2 = new DataTable();
            DtView2.Columns.Add("GoodsType", typeof(string));
            DtView2.Columns.Add("Varieties", typeof(string));
            DtView2.Columns.Add("Package", typeof(string));
            DtView2.Columns.Add("Num", typeof(string));
            DtView2.Columns.Add("FeeWeight", typeof(string));

            DtView2.Columns.Add("FeeVolume", typeof(string));
            DtView2.Columns.Add("Weight", typeof(string));
            DtView2.Columns.Add("Volume", typeof(string));
            DtView2.Columns.Add("WeightPrice", typeof(string));
            DtView2.Columns.Add("VolumePrice", typeof(string));

            DtView2.Columns.Add("FeeType", typeof(string));
            DtView2.Columns.Add("Freight", typeof(string));
            for (int i = 0; i < RowCount; i++)
            {
                DtView2.Rows.Add();
            }
            gridControl2.DataSource = DtView2;
            #endregion

            #region 计费信息
            DataTable DtView1 = new DataTable();
            DtView1.Columns.Add("DeliFee", typeof(decimal));
            DtView1.Columns.Add("DeclareValue", typeof(decimal));
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

            DtView1.Columns.Add("packagFee", typeof(decimal));
            DtView1.Columns.Add("FrameFee", typeof(decimal));
            DtView1.Columns.Add("ChangeFee", typeof(decimal));
            DtView1.Columns.Add("OtherFee", typeof(decimal));
            DtView1.Columns.Add("NoticeFee", typeof(decimal));

            DtView1.Columns.Add("ReceiptFee", typeof(decimal));
            DtView1.Columns.Add("ReceivFee", typeof(decimal));
            DtView1.Columns.Add("WarehouseFee", typeof(decimal));
            DtView1.Columns.Add("DeliveryFee", typeof(decimal));
            DtView1.Columns.Add("OperationWeight", typeof(decimal));
            DtView1.Columns.Add("OptWeight", typeof(decimal));

            DtView1.Rows.Add();
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
            string[] TransferModeList = CommonClass.Arg.TransferMode.Split(',');
            if (TransferModeList.Length > 0)
            {
                for (int i = 0; i < TransferModeList.Length; i++)
                {
                    TransferMode.Properties.Items.Add(TransferModeList[i]);
                }
                //TransferMode.SelectedIndex = 0;
            }

            //string[] TransitModeList = CommonClass.Arg.TransitMode.Split(',');
            //if (TransitModeList.Length > 0)
            //{
            //    for (int i = 0; i < TransitModeList.Length; i++)
            //    {
            //        TransitMode.Properties.Items.Add(TransitModeList[i]);
            //    }
            //    TransitMode.SelectedIndex = 0;
            //}

            string[] PaymentModeList = CommonClass.Arg.PaymentMode.Split(',');
            if (PaymentModeList.Length > 0)
            {
                for (int i = 0; i < PaymentModeList.Length; i++)
                {
                    PaymentMode.Properties.Items.Add(PaymentModeList[i]);
                }
                //PaymentMode.SelectedIndex = 0;
            }
            string[] VarietiesList = CommonClass.Arg.Varieties.Split(','); ;
            for (int i = 0; i < VarietiesList.Length; i++)
            {
                repositoryItemComboBox1.Items.Add(VarietiesList[i]);
            }

            string[] PackageList = CommonClass.Arg.Packag.Split(','); ;
            for (int i = 0; i < PackageList.Length; i++)
            {
                repositoryItemComboBox2.Items.Add(PackageList[i]);
            }
            gridView2.SetRowCellValue(0, "Package", "纸箱");
            string[] ReceivingWayList = CommonClass.Arg.ReceivingWay.Split(','); ;
            for (int i = 0; i < ReceivingWayList.Length; i++)
            {
                ReceivMode.Properties.Items.Add(ReceivingWayList[i]);
            }
            string[] ReceiptRequirList = CommonClass.Arg.ReceiptRequir.Split(','); ;
            for (int i = 0; i < ReceiptRequirList.Length; i++)
            {
                ReceiptCondition.Properties.Items.Add(ReceiptRequirList[i]);
            }
            string[] PayModeList = CommonClass.Arg.PayMode.Split(','); ;
            for (int i = 0; i < PayModeList.Length; i++)
            {
                PayMode.Properties.Items.Add(PayModeList[i]);
            }
            try
            {
                if (CommonClass.dsWeb == null || CommonClass.dsWeb.Tables.Count == 0)
                    return;
                String SiteName = "%%";
                DataRow[] dr = CommonClass.dsWeb.Tables[0].Select(string.Format("SiteName like '{0}' and WebRole<>'3PL'", SiteName));
                ReceiveWeb.Properties.Items.Clear();
                ReceiveWeb.Properties.Items.Add("无");
                for (int i = 0; i < dr.Length; i++)
                {
                    ReceiveWeb.Properties.Items.Add(dr[i]["WebName"]);
                }
            }
            catch (Exception)
            {
                MsgBox.ShowOK("正在加载基础资料，请稍等！");
            }

            #endregion

            #region 结算信息
            DataTable DtViewJS = new DataTable();
            DtViewJS.Columns.Add("MainlineFee", typeof(decimal));
            DtViewJS.Columns.Add("TransferFee", typeof(decimal));
            DtViewJS.Columns.Add("DepartureOptFee", typeof(decimal));
            DtViewJS.Columns.Add("TerminalOptFee", typeof(decimal));
            DtViewJS.Columns.Add("TerminalAllotFee", typeof(decimal));
            DtViewJS.Columns.Add("DeliveryFee", typeof(decimal));


            DtViewJS.Columns.Add("ReceiptFee_C", typeof(decimal));//结算回单费
            DtViewJS.Columns.Add("NoticeFee_C", typeof(decimal));//结算等通知放货费
            DtViewJS.Columns.Add("SupportValue_C", typeof(decimal));//结算保价费

            DtViewJS.Columns.Add("AgentFee_C", typeof(decimal));//结算代收手续费
            DtViewJS.Columns.Add("PackagFee_C", typeof(decimal));//结算包装费
            DtViewJS.Columns.Add("OtherFee_C", typeof(decimal));//结算其它费

            DtViewJS.Columns.Add("HandleFee_C", typeof(decimal));//结算装卸费
            DtViewJS.Columns.Add("StorageFee_C", typeof(decimal));//结算进仓费
            DtViewJS.Columns.Add("WarehouseFee_C", typeof(decimal));//结算仓储费

            DtViewJS.Columns.Add("ForkliftFee_C", typeof(decimal));//结算叉车费
            DtViewJS.Columns.Add("Tax_C", typeof(decimal));//结算税金
            DtViewJS.Columns.Add("ChangeFee_C", typeof(decimal));//结算改单费

            DtViewJS.Columns.Add("UpstairFee_C", typeof(decimal));//结算上楼费
            DtViewJS.Columns.Add("CustomsFee_C", typeof(decimal));//结算报关费
            DtViewJS.Columns.Add("FrameFee_C", typeof(decimal));//结算代打木架费

            DtViewJS.Columns.Add("Expense_C", typeof(decimal));//结算工本费
            DtViewJS.Columns.Add("FuelFee_C", typeof(decimal));//结算燃油费
            DtViewJS.Columns.Add("InformationFee_C", typeof(decimal));//结算信息费
            DtViewJS.Rows.Add();
            gridView8.BestFitColumns();
            gridControl6.DataSource = DtViewJS;

            #endregion

            //CommonClass.SetProvince(ReceivProvince);
            CommonClass.SetSite(StartSite, false);

            StartSite.Text = CommonClass.UserInfo.SiteName;
            CauseName.EditValue = CommonClass.UserInfo.CauseName;
            AreaName.EditValue = CommonClass.UserInfo.AreaName;
            DepName.EditValue = CommonClass.UserInfo.DepartName;
            BillMan.EditValue = CommonClass.UserInfo.UserName;
            begWeb.EditValue = CommonClass.UserInfo.WebName;
            OperBespeakTime.EditValue = DBNull.Value;

            gridView2.Appearance.FocusedCell.BackColor = Color.Yellow;
            gridView2.Appearance.HorzLine.BackColor = Color.Gray;
            gridView2.Appearance.VertLine.BackColor = Color.Gray;

            gridView1.Appearance.FocusedCell.BackColor = Color.Yellow;
            gridView1.Appearance.HorzLine.BackColor = Color.Gray;
            gridView2.Appearance.VertLine.BackColor = Color.Gray;

            gridView1.Appearance.FocusedCell.BackColor = Color.Yellow;
            gridView1.Appearance.HorzLine.BackColor = Color.Gray;
            gridView1.Appearance.VertLine.BackColor = Color.Gray;

            if (isModify == 1)
            {
                GetWayBill();
            }
            DestinationSite.Focus();

            gridControl8.DataSource = CommonClass.dsBasCust.Tables[0];
            gridControl7.DataSource = CommonClass.dsBasReceiveCust.Tables[0];
            QSP_GET_DELIVERY_BILL();
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
                DataSet ds = SqlHelper.GetDataSet(sps);
                gcjiehuodanhao.DataSource = ds.Tables[0];
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
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNO", BillNO));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_ByID", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                dsModified = ds.Copy();
                isModify = 1;
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
                ConsigneeCompany.EditValue = ConvertType.ToInt32(dr["NoticeState"]) == 0 ? dr["ConsigneeCompany"] : dr["ConsigneeCompany_K"];

                PickGoodsSite.EditValue = dr["PickGoodsSite"];
                ReceivProvince.EditValue = dr["ReceivProvince"];
                ReceivCity.EditValue = dr["ReceivCity"];
                ReceivArea.EditValue = dr["ReceivArea"];
                ReceivStreet.EditValue = dr["ReceivStreet"];
                ReceivAddress.EditValue = dr["ReceivAddress"];
                ConsignorCellPhone.EditValue = dr["ConsignorCellPhone"];
                ConsignorPhone.EditValue = dr["ConsignorPhone"];
                ConsignorName.EditValue = dr["ConsignorName"];
                ConsignorCompany.EditValue = dr["ConsignorCompany"];
                ReceivMode.EditValue = dr["ReceivMode"];
                CusNo.EditValue = dr["CusNo"];
                CusType.EditValue = dr["CusType"];
                ValuationType.EditValue = dr["ValuationType"];
                ReceivOrderNo.EditValue = dr["ReceivOrderNo"];
                Salesman.EditValue = dr["Salesman"];


                BillNo.EditValue = dr["BillNo"];
                VehicleNo.EditValue = dr["VehicleNo"];
                StartSite.EditValue = dr["StartSite"];
                TransferMode.EditValue = dr["TransferMode"];
                DestinationSite.EditValue = dr["DestinationSite"];
                TransferSite.EditValue = dr["TransferSite"];
                TransitLines.EditValue = dr["TransitLines"];
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
                CouponsNo.EditValue = dr["CouponsNo"];
                CouponsAmount.EditValue = dr["CouponsAmount"];
                PaymentMode.EditValue = dr["PaymentMode"];
                PaymentAmout.EditValue = dr["PaymentAmout"];
                PayMode.EditValue = dr["PayMode"];
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
                ReceiveWeb.EditValue = dr["ReceiveWeb"];


                string sg = dr["DeliveryFee"].ToString();
                gridView8.SetRowCellValue(0, "MainlineFee", dr["MainlineFee"]);
                gridView1.SetRowCellValue(0, "DeliveryFee", dr["DeliveryFee"]);
                gridView8.SetRowCellValue(0, "TransferFee", dr["TransferFee"]);
                gridView8.SetRowCellValue(0, "DepartureOptFee", dr["DepartureOptFee"]);
                gridView8.SetRowCellValue(0, "TerminalOptFee", dr["TerminalOptFee"]);
                gridView8.SetRowCellValue(0, "TerminalAllotFee", dr["TerminalAllotFee"]);

                gridView8.SetRowCellValue(0, "ReceiptFee_C", dr["ReceiptFee_C"]);
                gridView8.SetRowCellValue(0, "NoticeFee_C", dr["NoticeFee_C"]);
                gridView8.SetRowCellValue(0, "SupportValue_C", dr["SupportValue_C"]);

                gridView8.SetRowCellValue(0, "AgentFee_C", dr["AgentFee_C"]);
                gridView8.SetRowCellValue(0, "PackagFee_C", dr["PackagFee_C"]);
                gridView8.SetRowCellValue(0, "OtherFee_C", dr["OtherFee_C"]);

                gridView8.SetRowCellValue(0, "HandleFee_C", dr["HandleFee_C"]);
                gridView8.SetRowCellValue(0, "StorageFee_C", dr["StorageFee_C"]);
                gridView8.SetRowCellValue(0, "WarehouseFee_C", dr["WarehouseFee_C"]);

                gridView8.SetRowCellValue(0, "ForkliftFee_C", dr["ForkliftFee_C"]);
                gridView8.SetRowCellValue(0, "Tax_C", dr["Tax_C"]);
                gridView8.SetRowCellValue(0, "ChangeFee_C", dr["ChangeFee_C"]);

                gridView8.SetRowCellValue(0, "UpstairFee_C", dr["UpstairFee_C"]);
                gridView8.SetRowCellValue(0, "CustomsFee_C", dr["CustomsFee_C"]);
                gridView8.SetRowCellValue(0, "FrameFee_C", dr["FrameFee_C"]);

                gridView8.SetRowCellValue(0, "Expense_C", dr["Expense_C"]);
                gridView8.SetRowCellValue(0, "FuelFee_C", dr["FuelFee_C"]);
                gridView8.SetRowCellValue(0, "InformationFee_C", dr["InformationFee_C"]);

                AlienGoods.Checked = Convert.ToInt32(dr["AlienGoods"].ToString() == "" ? "0" : dr["AlienGoods"].ToString()) > 0;
                GoodsVoucher.Checked = Convert.ToInt32(dr["GoodsVoucher"].ToString() == "" ? "0" : dr["GoodsVoucher"].ToString()) > 0;
                PreciousGoods.Checked = Convert.ToInt32(dr["GoodsVoucher"].ToString() == "" ? "0" : dr["GoodsVoucher"].ToString()) > 0;
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

                gridView1.SetRowCellValue(0, "OperationWeight", ConvertType.ToDecimal(dr["OperationWeight"]));
                gridView1.SetRowCellValue(0, "OptWeight", ConvertType.ToDecimal(dr["OptWeight"]));

                IsCenterBack.Text = dr["IsCenterBack"].ToString();
                OperBespeakTime.EditValue = dr["OperBespeakTime"];
                OperBespeakContent.Text = dr["OperBespeakContent"].ToString();



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
                DataRow dr_ = gridView4.GetDataRow(rows);
                ReceivStreet.Text = dr_["MiddleStreet"].ToString();
                string SiteName = dr_["SiteName"].ToString();
                if (SiteName != "")
                {
                    //TransferSite.Text = dr_["SiteName"].ToString();
                    string[] SiteNames = SiteName.Split(',');
                    for (int i = 0; i < SiteNames.Length; i++)
                    {
                        TransferSite.Properties.Items.Add(SiteNames[i]);
                    }
                    TransferSite.Text = SiteNames[0];
                }
                else
                {
                    TransferSite.Text = CommonClass.UserInfo.SiteName;
                    TransitMode.Text = "中强项目";
                }
                //PickGoodsSite.Properties.Items.Clear();
                //PickGoodsSite.Properties.Items.Add(dr_["WebName"]);
                //PickGoodsSite.Text = dr_["WebName"].ToString() ;
                //DestinationSite.Text = dr_["MiddleCity"].ToString() + "" + dr_["MiddleArea"].ToString();
                //TransitLines.Text = StartSite.Text.Trim() + "-" + TransferSite.Text.Trim() + "-" + dr_["Destination"].ToString();
                //string type = dr_["type"].ToString();
                //if (type.Contains("+"))
                //{
                //    TransferMode.Text = "网点送货";
                //}
                //else
                //{
                //    if (type.Contains("送"))
                //    {
                //        TransferMode.Text = "网点送货";
                //    }
                //    else
                //    {
                //        TransferMode.Text = "自提";
                //    }

                //}

                ReceivStreet.Focus();
                gcdaozhan.Visible = false;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridView1.PostEditor();
            gridView2.PostEditor();
            try
            {
                //if (BillNo.Text.Trim() == "")
                //{
                //    MsgBox.ShowOK("必须输入运单号");
                //    BillNo.Focus();
                //    return;
                //}
                if (DestinationSite.Text.Trim() == "")
                {
                    MsgBox.ShowOK("必须输入到站");
                    DestinationSite.Focus();
                    return;
                }
                if (TransferSite.Text.Trim() == "")
                {
                    MsgBox.ShowOK("中转地不能为空");
                    TransferSite.Focus();
                    return;
                }

                if (ConsignorCompany.Text.Trim() == "")
                {
                    MsgBox.ShowOK("必须输入发货公司名称");
                    ConsignorCompany.Focus();
                    return;
                }
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

                if (ConsigneeCompany.Text.Trim() == "")
                {
                    MsgBox.ShowOK("必须输入收货公司名称");
                    ConsigneeCompany.Focus();
                    return;
                }
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
                if (ReceiveWeb.Text.Trim() == "")
                {
                    MsgBox.ShowOK("必须选择接收网点");
                    return;
                } 

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

                if (ReceivProvince.Text.Trim() == "" || ReceivCity.Text.Trim() == "" || ReceivArea.Text.Trim() == "" || ReceivStreet.Text.Trim() == "")
                {
                    MsgBox.ShowOK("必须选择省市区街道");
                    ReceivProvince.Focus();
                    return;
                }

                if (TransferMode.Text.Contains("送") && ReceivStreet.Text.Trim() == "")
                {
                    MsgBox.ShowOK("必须选择街道");
                    ReceivStreet.Focus();
                    return;
                }

                if (TransferMode.Text.Trim() == "")
                {
                    MsgBox.ShowOK("必须选择交接方式");
                    TransferMode.Focus();
                    return;
                } 

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
                if (Num < 0 || FeeWeight < 0 || FeeVolume < 0
                    || Weight < 0 || Volume < 0 || WeightPrice < 0 || VolumePrice < 0 || Freight < 0
                    || DeliFee < 0 || DeclareValue < 0 || SupportValue < 0 || Tax < 0 || InformationFee < 0
                    || Expense < 0 || DiscountTransfer < 0 || CollectionPay < 0 || AgentFee < 0 || FuelFee < 0
                    || UpstairFee < 0 || HandleFee < 0 || ForkliftFee < 0 || StorageFee < 0 || CustomsFee < 0 || packagFee < 0
                    || FrameFee < 0 || ChangeFee < 0 || OtherFee < 0 || NoticeFee < 0 || ReceiptFee < 0 || ReceivFee < 0
                    || WarehouseFee < 0 || NowPay_s < 0 || FetchPay_s < 0 || MonthPay_s < 0 || ShortOwePay_s < 0 || BefArrivalPay_s < 0)
                {
                    MsgBox.ShowOK("输入数字不能小于0，请检查！");
                    return;
                }


                #endregion 

                DataRow[] dr = ((DataTable)gridControl3.DataSource).Select("MiddleProvince='" + ReceivProvince.Text.Trim() + "'");
                if (dr.Length == 0)
                {
                    ReceivProvince.Focus();
                    MsgBox.ShowOK("请选择正确的省份！");
                    return;
                }

                DataRow[] dr1 = ((DataTable)gridControl9.DataSource).Select("MiddleCity='" + ReceivCity.Text.Trim() + "'");
                if (dr1.Length == 0)
                {
                    ReceivCity.Focus();
                    MsgBox.ShowOK("请选择正确的城市！");
                    return;
                }

                DataRow[] dr2 = ((DataTable)gridControl10.DataSource).Select("MiddleArea='" + ReceivArea.Text.Trim() + "'");
                if (dr2.Length == 0)
                {
                    ReceivArea.Focus();
                    MsgBox.ShowOK("请选择正确区县！");
                    return;
                }

                DataRow[] dr3 = ((DataTable)gcdaozhan.DataSource).Select("MiddleStreet='" + ReceivStreet.Text.Trim() + "'");
                if (dr3.Length == 0)
                {
                    ReceivStreet.Focus();
                    MsgBox.ShowOK("请选择正确街道！");
                    return;
                }

                if (ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "CollectionPay")) > 0
                    && (CollectionName.Text.Trim() == "" || CollectionBank.Text.Trim() == "" || CollectionBranch.Text.Trim() == "" || CollectionAccount.Text.Trim() == ""))
                {
                    MsgBox.ShowOK("有代收货款金额必须填写银行卡信息！");
                    xtraTabControl1.SelectedTabPage = xtraTabPage3;
                    DisTranName.Focus();
                    return;
                }
                if (ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "CollectionPay")) > 0 && IsCenterBack.Text.Trim() == "")
                {
                    MsgBox.ShowOK("有代收货款金额必须选择返款类型！");
                    xtraTabControl1.SelectedTabPage = xtraTabPage3;
                    DisTranName.Focus();
                    return;
                }

                if (ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "CollectionPay")) > 0
                    && (ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "CollectionPay")) > ConvertType.ToDecimal(FetchPay.Text.Trim())))
                {
                    MsgBox.ShowOK("提付金额不能小于代收货款！");
                    return;
                }

                if (TransitMode.Text.Trim() == "中强整车" && VehicleNo.Text.Trim() == "")
                {
                    MsgBox.ShowOK("整车直送必须录入整车申请号！");
                    return;
                }

                if (PaymentMode.Text.Trim() != "免费" &&
                    (ConvertType.ToDecimal(NowPay.Text)
                    + ConvertType.ToDecimal(FetchPay.Text)
                    + ConvertType.ToDecimal(MonthPay.Text)
                    + ConvertType.ToDecimal(ShortOwePay.Text)
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
                    != ConvertType.ToDecimal(PaymentAmout.Text))
                {
                    MsgBox.ShowOK("两笔付金额不等于总金额：" + ConvertType.ToDecimal(PaymentAmout.Text));
                    return;
                }

                if (PaymentMode.Text.Trim() == "两笔付")
                {
                    if (NowPay_s > 0 && (FetchPay_s + MonthPay_s + ShortOwePay_s + BefArrivalPay_s) == 0)
                    {
                        MsgBox.ShowOK("两笔付必须有两种付款类型");
                        return;
                    }
                    if (FetchPay_s > 0 && (NowPay_s + MonthPay_s + ShortOwePay_s + BefArrivalPay_s) == 0)
                    {
                        MsgBox.ShowOK("两笔付必须有两种付款类型");
                        return;
                    }
                    if (MonthPay_s > 0 && (NowPay_s + FetchPay_s + ShortOwePay_s + BefArrivalPay_s) == 0)
                    {
                        MsgBox.ShowOK("两笔付必须有两种付款类型");
                        return;
                    }
                    if (ShortOwePay_s > 0 && (NowPay_s + FetchPay_s + MonthPay_s + BefArrivalPay_s) == 0)
                    {
                        MsgBox.ShowOK("两笔付必须有两种付款类型");
                        return;
                    }
                    if (BefArrivalPay_s > 0 && (NowPay_s + FetchPay_s + MonthPay_s + ShortOwePay_s) == 0)
                    {
                        MsgBox.ShowOK("两笔付必须有两种付款类型");
                        return;
                    }
                }

                #region 检测单号
                //if (isModify == 0)
                //{
                //    int billno_state = 4;
                //    string billno_new = BillNo.Text.Trim();
                //    List<SqlPara> list = new List<SqlPara>();
                //    list.Add(new SqlPara("billno", BillNo.Text.Trim()));
                //    list.Add(new SqlPara("webid", begWeb.Text.Trim()));
                //    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_QSP_GET_BILL_EXIST", list);
                //    DataSet dsBill = SqlHelper.GetDataSet(sps);
                //    if (dsBill.Tables[0].Rows.Count > 0)
                //    {
                //        billno_state = ConvertType.ToInt32(dsBill.Tables[0].Rows[0]["state"]);
                //    }
                //    if (billno_state != 1)
                //    {
                //        string msg = "";
                //        if (billno_state == 4)
                //        {
                //            msg = "本网点中没有单号为 " + billno_new + " 的托运单\r\n\r\n或者没有在系统中为本网点分配单号 " + billno_new + " ,请确认!";
                //        }
                //        else if (billno_state == 2)
                //        {
                //            msg = "本网点中的托运单号 " + billno_new + " 已经使用,请确认!";
                //        }
                //        else if (billno_state == 3)
                //        {
                //            msg = "本网点中的托运单号 " + billno_new + " 已经作废,不能重复使用,请确认!";
                //        }
                //        if (msg != "")
                //        {
                //            MsgBox.ShowOK(msg);
                //            BillNo.Focus();
                //            BillNo.Select(0, billno_new.Length);
                //            return;
                //        }
                //    }
                //}
                #endregion

                #region 应收账款检测

                //decimal MonthPay_s = ConvertType.ToDecimal(MonthPay.Text);
                //if (MonthPay_s > 0)
                //{
                //    List<SqlPara> list = new List<SqlPara>();
                //    list.Add(new SqlPara("ConsignorCompany", ConsignorCompany.Text.Trim()));
                //    list.Add(new SqlPara("ConsignorName", ConsignorName.Text.Trim()));
                //    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_CustContractCheck", list);
                //    DataSet dsCust = SqlHelper.GetDataSet(sps);
                //    if (dsCust.Tables[0].Rows.Count > 0)
                //    {
                //        //月结剩余天数
                //        int remaindays = ConvertType.ToInt32(dsCust.Tables[0].Rows[0]["remaindays"].ToString());
                //        //月结剩余额度
                //        decimal remainacc = ConvertType.ToDecimal(dsCust.Tables[0].Rows[0]["remainacc"].ToString());

                //        if (remaindays < 0)
                //        {
                //            MsgBox.ShowOK("发货单位：" + ConsignorCompany.Text.Trim() + " 欠款 " + Math.Abs(remaindays) + "天，不能开单！");
                //            return;
                //        }
                //        else if (remaindays <= 5)
                //        {
                //            MsgBox.ShowOK("发货单位：" + ConsignorCompany.Text.Trim() + " 还有" + Math.Abs(remaindays) + "天欠款到期，请及时通知发货人付款！");
                //        }

                //        if (isModify > 0) remainacc += (MonthPay_s);

                //        if (remainacc <= 0)
                //        {
                //            MsgBox.ShowOK("发货单位：" + ConsignorCompany.Text.Trim() + " 欠款额度超过合同额度，不能开单！");
                //            return;
                //        }
                //    }
                //    else
                //    {
                //        MsgBox.ShowOK("发货单位：" + ConsignorCompany.Text.Trim() + " 不存在，不能开单！");
                //        return;
                //    }
                //}

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

            decimal ActualFreight = ConvertType.ToDecimal(PaymentAmout.Text) - DiscountTransfer - CollectionPay;

            #region 获取配载目的网点
            string LoadEweb = "";
            DataRow[] drweb = CommonClass.dsWeb.Tables[0].Select("StartSiteRange='" + StartSite.Text + "' and SiteName='" + TransferSite.Text + "' ");
            if (drweb.Length > 0)
            {
                LoadEweb = drweb[0]["WebName"].ToString();
            }
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
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillId", 0));
            list.Add(new SqlPara("BillNo", BillNo.Text.Trim()));
            list.Add(new SqlPara("VehicleNo", VehicleNo.Text.Trim()));
            list.Add(new SqlPara("BillDate", System.DateTime.Now));
            list.Add(new SqlPara("BillState", 0));
            list.Add(new SqlPara("StartSite", StartSite.Text.Trim()));
            list.Add(new SqlPara("TransferMode", TransferMode.Text.Trim()));
            list.Add(new SqlPara("DestinationSite", DestinationSite.Text.Trim()));
            list.Add(new SqlPara("TransferSite", TransferSite.Text.Trim()));
            list.Add(new SqlPara("TransitLines", TransitLines.Text.Trim()));
            list.Add(new SqlPara("TransitMode", TransitMode.Text.Trim()));
            list.Add(new SqlPara("CusOderNo", CusOderNo.Text.Trim()));

            list.Add(new SqlPara("ConsigneeCellPhone", NoticeState.Checked ? "888888" : ConsigneeCellPhone.Text.Trim()));
            list.Add(new SqlPara("ConsigneePhone", NoticeState.Checked ? "888888" : ConsigneePhone.Text.Trim()));
            list.Add(new SqlPara("ConsigneeName", NoticeState.Checked ? "888888" : ConsigneeName.Text.Trim()));
            list.Add(new SqlPara("ConsigneeCompany", NoticeState.Checked ? "888888" : ConsigneeCompany.Text.Trim()));

            list.Add(new SqlPara("PickGoodsSite", PickGoodsSite.Text.Trim()));
            list.Add(new SqlPara("ReceivProvince", ReceivProvince.Text.Trim()));
            list.Add(new SqlPara("ReceivCity", ReceivCity.Text.Trim()));
            list.Add(new SqlPara("ReceivArea", ReceivArea.Text.Trim()));
            list.Add(new SqlPara("ReceivStreet", ReceivStreet.Text.Trim()));
            list.Add(new SqlPara("ReceivAddress", ReceivAddress.Text.Trim()));
            list.Add(new SqlPara("ConsignorCellPhone", ConsignorCellPhone.Text.Trim()));
            list.Add(new SqlPara("ConsignorPhone", ConsignorPhone.Text.Trim()));
            list.Add(new SqlPara("ConsignorName", ConsignorName.Text.Trim()));
            list.Add(new SqlPara("ConsignorCompany", ConsignorCompany.Text.Trim()));
            list.Add(new SqlPara("ReceivMode", ReceivMode.Text.Trim()));
            list.Add(new SqlPara("CusNo", CusNo.Text.Trim()));
            list.Add(new SqlPara("CusType", CusType.Text.Trim()));
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

            list.Add(new SqlPara("OtherFee", OtherFee));
            list.Add(new SqlPara("IsInvoice", IsInvoice.Checked ? 1 : 0));
            list.Add(new SqlPara("ReceiptFee", ReceiptFee));
            list.Add(new SqlPara("ReceiptCondition", ReceiptCondition.Text.Trim()));
            list.Add(new SqlPara("FreightAmount", 0));
            list.Add(new SqlPara("ActualFreight", ActualFreight));
            list.Add(new SqlPara("CouponsNo", CouponsNo.Text.Trim()));
            list.Add(new SqlPara("CouponsAmount", CouponsAmount.Text.Trim() == "" ? "0" : CouponsAmount.Text.Trim()));
            list.Add(new SqlPara("PaymentMode", PaymentMode.Text.Trim()));
            list.Add(new SqlPara("PaymentAmout", PaymentAmout.Text.Trim() == "" ? "0" : PaymentAmout.Text.Trim()));
            list.Add(new SqlPara("PayMode", PayMode.Text.Trim()));
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


            list.Add(new SqlPara("MainlineFee", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "MainlineFee"))));
            list.Add(new SqlPara("DeliveryFee", ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "DeliveryFee"))));
            list.Add(new SqlPara("TransferFee", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "TransferFee"))));
            list.Add(new SqlPara("DepartureOptFee", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "DepartureOptFee"))));
            list.Add(new SqlPara("TerminalOptFee", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "TerminalOptFee"))));
            list.Add(new SqlPara("TerminalAllotFee", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "TerminalAllotFee"))));

            list.Add(new SqlPara("ReceiptFee_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "ReceiptFee_C"))));
            list.Add(new SqlPara("NoticeFee_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "NoticeFee_C"))));

            list.Add(new SqlPara("SupportValue_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "SupportValue_C"))));
            list.Add(new SqlPara("AgentFee_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "AgentFee_C"))));
            list.Add(new SqlPara("PackagFee_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "PackagFee_C"))));
            list.Add(new SqlPara("OtherFee_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "OtherFee_C"))));
            list.Add(new SqlPara("HandleFee_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "HandleFee_C"))));

            list.Add(new SqlPara("StorageFee_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "StorageFee_C"))));
            list.Add(new SqlPara("WarehouseFee_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "WarehouseFee_C"))));
            list.Add(new SqlPara("ForkliftFee_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "ForkliftFee_C"))));
            list.Add(new SqlPara("Tax_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "Tax_C"))));
            list.Add(new SqlPara("ChangeFee_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "ChangeFee_C"))));

            list.Add(new SqlPara("UpstairFee_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "UpstairFee_C"))));
            list.Add(new SqlPara("CustomsFee_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "CustomsFee_C"))));
            list.Add(new SqlPara("FrameFee_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "FrameFee_C"))));
            list.Add(new SqlPara("Expense_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "Expense_C"))));
            list.Add(new SqlPara("FuelFee_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "FuelFee_C"))));
            list.Add(new SqlPara("InformationFee_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "InformationFee_C"))));

            list.Add(new SqlPara("ConsigneeCellPhone_K", ConsigneeCellPhone.Text.Trim()));
            list.Add(new SqlPara("ConsigneePhone_K", ConsigneePhone.Text.Trim()));
            list.Add(new SqlPara("ConsigneeName_K", ConsigneeName.Text.Trim()));
            list.Add(new SqlPara("ConsigneeCompany_K", ConsigneeCompany.Text.Trim()));

            list.Add(new SqlPara("OperationWeight", ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "OperationWeight"))));
            list.Add(new SqlPara("OperTime", ConvertType.ToInt32(OperTime.Text.Trim())));
            list.Add(new SqlPara("IsCenterBack", IsCenterBack.Text));
            list.Add(new SqlPara("OperBespeakTime", OperBespeakTime.EditValue == DBNull.Value ? DBNull.Value : OperBespeakTime.EditValue));
            list.Add(new SqlPara("OperBespeakContent", OperBespeakContent.Text.Trim()));
            list.Add(new SqlPara("LoadEweb", LoadEweb));
            list.Add(new SqlPara("OptWeight", OptWeight));
            list.Add(new SqlPara("ReceiveWeb", ReceiveWeb.Text.Trim()));

            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "USP_ADD_WAYBILL_3PL", list);
            DataSet ds = SqlHelper.GetDataSet(sps);
            if (ds.Tables[0].Rows.Count > 0)
            {
                MsgBox.ShowOK();
                string billNo = ds.Tables[0].Rows[0]["BillNo"].ToString();
                if (checkEdit1.Checked)
                {
                    frmRuiLangService.Print("托运单", SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTBIll", new List<SqlPara> { new SqlPara("BillNo", billNo) })));
                }
                if (checkEdit2.Checked)
                {
                    frmPrintLabel fpl = new frmPrintLabel(billNo, SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTLABEL", new List<SqlPara> { new SqlPara("BillNo", billNo) })));
                    fpl.ShowDialog();
                }
                if (checkEdit3.Checked)
                {
                    frmRuiLangService.Print("信封", SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTENVELOPE", new List<SqlPara> { new SqlPara("BillNo", billNo) })));
                }
                clear();
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
                int rows = gridView10.FocusedRowHandle;
                if (rows < 0) return;

                ConsignorCompany.EditValueChanging -= new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsignorCompany_EditValueChanging);
                ConsignorName.EditValueChanging -= new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsignorCompany_EditValueChanging);


                ConsignorCompany.EditValue = gridView10.GetRowCellValue(rows, "CusName").ToString();
                ConsignorName.EditValue = gridView10.GetRowCellValue(rows, "ContactMan").ToString();
                ConsignorCellPhone.EditValue = gridView10.GetRowCellValue(rows, "ContactCellPhone").ToString();
                ConsignorPhone.EditValue = gridView10.GetRowCellValue(rows, "ContactPhone").ToString();


                CusType.EditValue = gridView10.GetRowCellValue(rows, "CusType").ToString();
                CusNo.EditValue = gridView10.GetRowCellValue(rows, "CustNo").ToString();


                gridControl8.Visible = false;
                ReceivMode.Focus();
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
                int rows = gridView9.FocusedRowHandle;
                if (rows < 0) return;

                ConsigneeCompany.EditValueChanging -= new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsigneeCompany_EditValueChanging);
                ConsigneeName.EditValueChanging -= new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.ConsigneeCompany_EditValueChanging);

                ConsigneeCompany.EditValue = gridView9.GetRowCellValue(rows, "RCName").ToString();
                ConsigneeName.EditValue = gridView9.GetRowCellValue(rows, "ContactMan").ToString();
                ConsigneeCellPhone.EditValue = gridView9.GetRowCellValue(rows, "ContactCellPhone").ToString();
                ConsigneePhone.EditValue = gridView9.GetRowCellValue(rows, "ContactPhone").ToString();
                gridControl7.Visible = false;
                ReceivAddress.Focus();
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
                ConsignorCellPhone.Text = "";
                ConsignorPhone.Text = "";
                gridView10.ClearColumnsFilter();
                gridView10.Columns["CusName"].FilterInfo = new ColumnFilterInfo(
                    "[CusName] LIKE " + "'%" + e.NewValue.ToString() + "%'"
                    + " OR [ContactMan] LIKE" + "'%" + e.NewValue.ToString() + "%'"
                    + " OR [ContactPhone] LIKE" + "'%" + e.NewValue.ToString() + "%'"
                    + " OR [ContactCellPhone] LIKE" + "'%" + e.NewValue.ToString() + "%'",
                    "");
            }
            else
            {
                gridView10.ClearColumnsFilter();
            }
        }

        private void ConsigneeCompany_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue != null)
            {
                ConsigneeCellPhone.Text = "";
                ConsigneePhone.Text = "";
                gridView9.ClearColumnsFilter();
                gridView9.Columns["RCName"].FilterInfo = new ColumnFilterInfo(
                    "[RCName] LIKE " + "'%" + e.NewValue.ToString() + "%'"
                    + " OR [ContactMan] LIKE" + "'%" + e.NewValue.ToString() + "%'"
                    + " OR [ContactPhone] LIKE" + "'%" + e.NewValue.ToString() + "%'"
                    + " OR [ContactCellPhone] LIKE" + "'%" + e.NewValue.ToString() + "%'",
                    "");
            }
            else
            {
                gridView9.ClearColumnsFilter();
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
                if (e.Column.FieldName == "FeeWeight" || e.Column.FieldName == "FeeVolume" || e.Column.FieldName == "WeightPrice" || e.Column.FieldName == "VolumePrice")
                {
                    decimal FeeWeight = ConvertType.ToDecimal(gridView2.GetRowCellValue(e.RowHandle, "FeeWeight"));
                    decimal FeeVolume = ConvertType.ToDecimal(gridView2.GetRowCellValue(e.RowHandle, "FeeVolume"));
                    decimal WeightPrice = ConvertType.ToDecimal(gridView2.GetRowCellValue(e.RowHandle, "WeightPrice"));
                    decimal VolumePrice = ConvertType.ToDecimal(gridView2.GetRowCellValue(e.RowHandle, "VolumePrice"));
                    decimal accw = FeeWeight * WeightPrice;
                    decimal accv = FeeVolume * VolumePrice;
                    decimal acc = Math.Max(accw, accv);
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

                if (e.Column.FieldName == "FeeWeight" || e.Column.FieldName == "FeeVolume" || e.Column.FieldName == "Package" || e.Column.FieldName == "Num")
                {
                    decimal OperationWeight = 0;
                    for (int i = 0; i < RowCount; i++)
                    {
                        decimal w = ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeWeight"));
                        decimal v = ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeVolume"));
                        string Package = gridView2.GetRowCellValue(i, "Package").ToString();

                        if (Package != "纸箱")
                        {
                            OperationWeight += Math.Max(w, v / (decimal)3.8 * 1000) * (decimal)1.1;
                        }
                        else
                        {
                            OperationWeight += Math.Max(w, v / (decimal)3.8 * 1000);
                        }
                    }
                    gridView1.SetRowCellValue(0, "OperationWeight", Math.Round(OperationWeight, 2));

                    #region 操作重量
                    decimal Num = ConvertType.ToDecimal(gridColumn67.SummaryItem.SummaryValue);
                    decimal FeeWeight = ConvertType.ToDecimal(gridColumn68.SummaryItem.SummaryValue);
                    decimal FeeVolume = ConvertType.ToDecimal(gridColumn69.SummaryItem.SummaryValue);
                    decimal weight_cz = FeeWeight;
                    if (Num > 0)
                    {
                        if (FeeWeight / Convert.ToDecimal(1000) * Convert.ToDecimal(4) - FeeVolume < 0)
                        {
                            weight_cz = Math.Round(FeeVolume / Convert.ToDecimal(4) * Convert.ToDecimal(1000), 2);
                        }
                        if (weight_cz / Num > 200)
                        {
                            weight_cz = Math.Round(weight_cz * Convert.ToDecimal(0.5), 2);
                        }
                    }
                    gridView1.SetRowCellValue(0, "OptWeight", Math.Round(weight_cz, 2));
                    #endregion

                    SetFee();
                }

                gridView8.SetRowCellValue(0, "ReceiptFee_C", gridView1.GetRowCellValue(e.RowHandle, "ReceiptFee"));
                gridView8.SetRowCellValue(0, "NoticeFee_C", gridView1.GetRowCellValue(e.RowHandle, "NoticeFee"));
                gridView8.SetRowCellValue(0, "SupportValue_C", gridView1.GetRowCellValue(e.RowHandle, "SupportValue"));

                gridView8.SetRowCellValue(0, "AgentFee_C", gridView1.GetRowCellValue(e.RowHandle, "AgentFee"));
                gridView8.SetRowCellValue(0, "PackagFee_C", gridView1.GetRowCellValue(e.RowHandle, "PackagFee"));
                gridView8.SetRowCellValue(0, "OtherFee_C", gridView1.GetRowCellValue(e.RowHandle, "OtherFee"));

                gridView8.SetRowCellValue(0, "HandleFee_C", gridView1.GetRowCellValue(e.RowHandle, "HandleFee"));
                gridView8.SetRowCellValue(0, "StorageFee_C", gridView1.GetRowCellValue(e.RowHandle, "StorageFee"));
                gridView8.SetRowCellValue(0, "WarehouseFee_C", gridView1.GetRowCellValue(e.RowHandle, "WarehouseFee"));

                gridView8.SetRowCellValue(0, "ForkliftFee_C", gridView1.GetRowCellValue(e.RowHandle, "ForkliftFee"));
                gridView8.SetRowCellValue(0, "Tax_C", gridView1.GetRowCellValue(e.RowHandle, "Tax"));
                gridView8.SetRowCellValue(0, "ChangeFee_C", gridView1.GetRowCellValue(e.RowHandle, "ChangeFee"));

                gridView8.SetRowCellValue(0, "UpstairFee_C", gridView1.GetRowCellValue(e.RowHandle, "UpstairFee"));
                gridView8.SetRowCellValue(0, "CustomsFee_C", gridView1.GetRowCellValue(e.RowHandle, "CustomsFee"));
                gridView8.SetRowCellValue(0, "FrameFee_C", gridView1.GetRowCellValue(e.RowHandle, "FrameFee"));

                gridView8.SetRowCellValue(0, "Expense_C", gridView1.GetRowCellValue(e.RowHandle, "Expense"));
                gridView8.SetRowCellValue(0, "FuelFee_C", gridView1.GetRowCellValue(e.RowHandle, "FuelFee"));
                gridView8.SetRowCellValue(0, "InformationFee_C", gridView1.GetRowCellValue(e.RowHandle, "InformationFee"));
                gridView8.SetRowCellValue(0, "DeliveryFee", gridView1.GetRowCellValue(e.RowHandle, "DeliveryFee"));


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
                    + UpstairFee + HandleFee + ForkliftFee + StorageFee + CustomsFee
                    + packagFee + FrameFee + ChangeFee + OtherFee + NoticeFee
                    + ReceiptFee + ReceivFee + WarehouseFee + DiscountTransfer
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
            }
            catch (Exception ex)
            {
                MsgBox.ShowError("计算运费：" + ex.Message);
            }
        }

        private void PaymentMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PaymentMode.Text == "现付" || PaymentMode.Text == "两笔付")
            {
                //PayMode.Enabled = true;
            }
            else
            {
                //PayMode.Enabled = false;
                //PayMode.Text="现金";
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
                //BillNo.Text = CommonClass.gcdate.ToString("yyMMddHHMMsss") + new Random().Next(11, 99);
                VehicleNo.EditValue = "";
                //StartSite.EditValue = "";
                //TransferMode.EditValue = "";
                DestinationSite.EditValue = "";
                TransferSite.EditValue = "";
                TransitLines.EditValue = "";
                //TransitMode.EditValue = "";
                CusOderNo.EditValue = "";
                ConsigneeCellPhone.EditValue = "";
                ConsigneePhone.EditValue = "";
                ConsigneeName.EditValue = "";
                ConsigneeCompany.EditValue = "";
                PickGoodsSite.EditValue = "";
                ReceivProvince.EditValue = "";
                ReceivCity.EditValue = "";
                ReceivArea.EditValue = "";
                ReceivStreet.EditValue = "";
                ReceivAddress.EditValue = "";
                ConsignorCellPhone.EditValue = "";
                ConsignorPhone.EditValue = "";
                ConsignorName.EditValue = "";
                ConsignorCompany.EditValue = "";
                ReceivMode.EditValue = "";
                CusNo.EditValue = "";
                CusType.EditValue = "";
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

                CouponsNo.EditValue = "";
                CouponsAmount.EditValue = "";
                PaymentMode.EditValue = "提付";
                PaymentAmout.EditValue = "";
                PayMode.EditValue = "";
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
                gridView8.SetRowCellValue(0, "MainlineFee", "0");
                gridView1.SetRowCellValue(0, "DeliveryFee", "0");
                gridView8.SetRowCellValue(0, "TransferFee", "0");
                gridView8.SetRowCellValue(0, "DepartureOptFee", "0");
                gridView8.SetRowCellValue(0, "TerminalOptFee", "0");
                gridView8.SetRowCellValue(0, "TerminalAllotFee", "0");

                gridView8.SetRowCellValue(0, "ReceiptFee_C", "0");
                gridView8.SetRowCellValue(0, "NoticeFee_C", "0");
                gridView8.SetRowCellValue(0, "SupportValue_C", "0");

                gridView8.SetRowCellValue(0, "AgentFee_C", "0");
                gridView8.SetRowCellValue(0, "PackagFee_C", "0");
                gridView8.SetRowCellValue(0, "OtherFee_C", "0");

                gridView8.SetRowCellValue(0, "HandleFee_C", "0");
                gridView8.SetRowCellValue(0, "StorageFee_C", "0");
                gridView8.SetRowCellValue(0, "WarehouseFee_C", "0");

                gridView8.SetRowCellValue(0, "ForkliftFee_C", "0");
                gridView8.SetRowCellValue(0, "Tax_C", "0");
                gridView8.SetRowCellValue(0, "ChangeFee_C", "0");

                gridView8.SetRowCellValue(0, "UpstairFee_C", "0");
                gridView8.SetRowCellValue(0, "CustomsFee_C", "0");
                gridView8.SetRowCellValue(0, "FrameFee_C", "0");

                gridView8.SetRowCellValue(0, "Expense_C", "0");
                gridView8.SetRowCellValue(0, "FuelFee_C", "0");
                gridView8.SetRowCellValue(0, "InformationFee_C", "0");

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

                DraftGUID.Text = Guid.NewGuid().ToString();

                gridView1.SetRowCellValue(0, "OperationWeight", 0);
                gridView1.SetRowCellValue(0, "OptWeight", 0);

                OperTime.Text = "";
                IsCenterBack.Text = "";
                OperBespeakTime.EditValue = DBNull.Value;
                OperBespeakContent.Text = "";

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
                GetWayBill();
            }
        }

        //改单申请 保存
        private void Savebill_Upd_Apply()
        {
            if (!HasApply())
            {
                MsgBox.ShowOK("已存在【改单申请】，不能重新申请！");
                return;
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

            decimal FrameFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "FrameFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "FrameFee").ToString());
            decimal ChangeFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "ChangeFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "ChangeFee").ToString());
            decimal OtherFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "OtherFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "OtherFee").ToString());
            decimal NoticeFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "NoticeFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "NoticeFee").ToString());
            decimal ReceiptFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "ReceiptFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "ReceiptFee").ToString());
            decimal ReceivFee = Convert.ToDecimal(gridView1.GetRowCellValue(0, "ReceivFee").ToString() == "" ? "0" : gridView1.GetRowCellValue(0, "ReceivFee").ToString());

            decimal WarehouseFee = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "WarehouseFee").ToString());

            decimal ActualFreight = ConvertType.ToDecimal(PaymentAmout.Text) + CollectionPay + DiscountTransfer;

            decimal ReceiptFee_C = ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "ReceiptFee_C").ToString());
            decimal NoticeFee_C = ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "NoticeFee_C").ToString());
            decimal SupportValue_C = ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "SupportValue_C").ToString());
            decimal AgentFee_C = ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "AgentFee_C").ToString());
            decimal PackagFee_C = ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "PackagFee_C").ToString());

            decimal OtherFee_C = ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "OtherFee_C").ToString());
            decimal HandleFee_C = ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "HandleFee_C").ToString());
            decimal StorageFee_C = ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "StorageFee_C").ToString());
            decimal WarehouseFee_C = ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "WarehouseFee_C").ToString());
            decimal ForkliftFee_C = ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "ForkliftFee_C").ToString());

            decimal Tax_C = ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "Tax_C").ToString());
            decimal ChangeFee_C = ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "ChangeFee_C").ToString());
            decimal UpstairFee_C = ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "UpstairFee_C").ToString());
            decimal CustomsFee_C = ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "CustomsFee_C").ToString());
            decimal FrameFee_C = ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "FrameFee_C").ToString());

            decimal Expense_C = ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "Expense_C").ToString());
            decimal FuelFee_C = ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "FuelFee_C").ToString());
            decimal InformationFee_C = ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "InformationFee_C").ToString());

            decimal MainlineFee = ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "MainlineFee").ToString());
            decimal DeliveryFee = ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "DeliveryFee").ToString());
            decimal TransferFee = ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "TransferFee").ToString());
            decimal DepartureOptFee = ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "DepartureOptFee").ToString());
            decimal TerminalOptFee = ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "TerminalOptFee").ToString());
            decimal TerminalAllotFee = ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "TerminalAllotFee").ToString());

            decimal OperationWeight = ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "OperationWeight"));
            decimal OptWeight = ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "OptWeight"));

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

            string changes_str = "", acc = "", acc2 = "";
            string sqlstr = "update WayBill  set ";

            if (dr["VehicleNo"].ToString() != VehicleNo.Text.Trim())
            {
                changes_str += "【整车申请号】 由:" + dr["VehicleNo"].ToString() + " 改为:" + VehicleNo.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " VehicleNo='" + VehicleNo.Text.Trim() + "' "; changes_num++;
            }
            if (dr["StartSite"].ToString() != StartSite.Text.Trim())
            {
                changes_str += "【始发站】 由:" + dr["StartSite"].ToString() + " 改为:" + StartSite.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " StartSite='" + StartSite.Text.Trim() + "' "; changes_num++;
            }
            if (dr["TransferMode"].ToString() != TransferMode.Text.Trim())
            {
                changes_str += "【交接方式】 由:" + dr["TransferMode"].ToString() + " 改为:" + TransferMode.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " TransferMode='" + TransferMode.Text.Trim() + "' "; changes_num++;
            }
            if (dr["DestinationSite"].ToString() != DestinationSite.Text.Trim())
            {
                changes_str += "【到站】 由:" + dr["DestinationSite"].ToString() + " 改为:" + DestinationSite.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " DestinationSite='" + DestinationSite.Text.Trim() + "' "; changes_num++;
            }
            if (dr["TransferSite"].ToString() != TransferSite.Text.Trim())
            {
                changes_str += "【中转地】 由:" + dr["TransferSite"].ToString() + " 改为:" + TransferSite.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " TransferSite='" + TransferSite.Text.Trim() + "' "; changes_num++;
            }
            if (dr["TransitLines"].ToString() != TransitLines.Text.Trim())
            {
                changes_str += "【运输线路】 由:" + dr["TransitLines"].ToString() + " 改为:" + TransitLines.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " TransitLines='" + TransitLines.Text.Trim() + "' "; changes_num++;
            }
            if (dr["TransitMode"].ToString() != TransitMode.Text.Trim())
            {
                changes_str += "【运输方式】 由:" + dr["TransitMode"].ToString() + " 改为:" + TransitMode.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " TransitMode='" + TransitMode.Text.Trim() + "' "; changes_num++;
            }
            if (dr["CusOderNo"].ToString() != CusOderNo.Text.Trim())
            {
                changes_str += "【客户单号】 由:" + dr["CusOderNo"].ToString() + " 改为:" + CusOderNo.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " CusOderNo='" + CusOderNo.Text.Trim() + "' "; changes_num++;
            }

            if (NoticeState.Checked)
            {
                if (dr["ConsigneeCellPhone_K"].ToString() != ConsigneeCellPhone.Text.Trim())
                {
                    changes_str += "【收货人手机】 由:" + dr["ConsigneeCellPhone_K"].ToString() + " 改为:" + ConsigneeCellPhone.Text.Trim();
                    sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneeCellPhone_K='" + ConsigneeCellPhone.Text.Trim() + "' "; changes_num++;
                    sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneeCellPhone='888888' "; changes_num++;
                }
                if (dr["ConsigneePhone_K"].ToString() != ConsigneePhone.Text.Trim())
                {
                    changes_str += "【收货人座机】 由:" + dr["ConsigneePhone_K"].ToString() + " 改为:" + ConsigneePhone.Text.Trim();
                    sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneePhone_K='" + ConsigneePhone.Text.Trim() + "' "; changes_num++;
                    sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneePhone='888888' "; changes_num++;
                }
                if (dr["ConsigneeName_K"].ToString() != ConsigneeName.Text.Trim())
                {
                    changes_str += "【收货联系人】 由:" + dr["ConsigneeName"].ToString() + " 改为:" + ConsigneeName.Text.Trim();
                    sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneeName='888888' "; changes_num++;
                }
                if (dr["ConsigneeCompany_K"].ToString() != ConsigneeCompany.Text.Trim())
                {
                    changes_str += "【收货公司名称】 由:" + dr["ConsigneeCompany_K"].ToString() + " 改为:" + ConsigneeCompany.Text.Trim();
                    sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneeCompany='888888' "; changes_num++;
                }
            }
            else
            {
                if (dr["ConsigneeCellPhone_K"].ToString() != ConsigneeCellPhone.Text.Trim())
                {
                    changes_str += "【收货人手机】 由:" + dr["ConsigneeCellPhone_K"].ToString() + " 改为:" + ConsigneeCellPhone.Text.Trim();
                    sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneeCellPhone='" + ConsigneeCellPhone.Text.Trim() + "' "; changes_num++;
                    sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneeCellPhone_K='" + ConsigneeCellPhone.Text.Trim() + "' "; changes_num++;
                }
                if (dr["ConsigneePhone_K"].ToString() != ConsigneePhone.Text.Trim())
                {
                    changes_str += "【收货人座机】 由:" + dr["ConsigneePhone_K"].ToString() + " 改为:" + ConsigneePhone.Text.Trim();
                    sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneePhone='" + ConsigneePhone.Text.Trim() + "' "; changes_num++;
                    sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneePhone_K='" + ConsigneePhone.Text.Trim() + "' "; changes_num++;
                }
                if (dr["ConsigneeName_K"].ToString() != ConsigneeName.Text.Trim())
                {
                    changes_str += "【收货联系人】 由:" + dr["ConsigneeName_K"].ToString() + " 改为:" + ConsigneeName.Text.Trim();
                    sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneeName='" + ConsigneeName.Text.Trim() + "' "; changes_num++;
                    sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneeName_K='" + ConsigneeName.Text.Trim() + "' "; changes_num++;
                }
                if (dr["ConsigneeCompany_K"].ToString() != ConsigneeCompany.Text.Trim())
                {
                    changes_str += "【收货公司名称】 由:" + dr["ConsigneeCompany_K"].ToString() + " 改为:" + ConsigneeCompany.Text.Trim();
                    sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneeCompany='" + ConsigneeCompany.Text.Trim() + "' "; changes_num++;
                    sqlstr += (changes_num > 0 ? " , " : " ") + " ConsigneeCompany_K='" + ConsigneeCompany.Text.Trim() + "' "; changes_num++;
                }
            }

            if (dr["PickGoodsSite"].ToString() != PickGoodsSite.Text.Trim())
            {
                changes_str += "【目的网点】 由:" + dr["PickGoodsSite"].ToString() + " 改为:" + PickGoodsSite.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " PickGoodsSite='" + PickGoodsSite.Text.Trim() + "' "; changes_num++;
            }
            if (dr["ReceivProvince"].ToString() != ReceivProvince.Text.Trim())
            {
                changes_str += "【收货省】 由:" + dr["ReceivProvince"].ToString() + " 改为:" + ReceivProvince.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ReceivProvince='" + ReceivProvince.Text.Trim() + "' "; changes_num++;
            }
            if (dr["ReceivCity"].ToString() != ReceivCity.Text.Trim())
            {
                changes_str += "【收货市】 由:" + dr["ReceivCity"].ToString() + " 改为:" + ReceivCity.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ReceivCity='" + ReceivCity.Text.Trim() + "' "; changes_num++;
            }
            if (dr["ReceivArea"].ToString() != ReceivArea.Text.Trim())
            {
                changes_str += "【收货区】 由:" + dr["ReceivArea"].ToString() + " 改为:" + ReceivArea.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ReceivArea='" + ReceivArea.Text.Trim() + "' "; changes_num++;
            }
            if (dr["ReceivStreet"].ToString() != ReceivStreet.Text.Trim())
            {
                changes_str += "【收货街道】 由:" + dr["ReceivStreet"].ToString() + " 改为:" + ReceivStreet.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ReceivStreet='" + ReceivStreet.Text.Trim() + "' "; changes_num++;
            }
            if (dr["ReceivAddress"].ToString() != ReceivAddress.Text.Trim())
            {
                changes_str += "【收货详细地址】 由:" + dr["ReceivAddress"].ToString() + " 改为:" + ReceivAddress.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ReceivAddress='" + ReceivAddress.Text.Trim() + "' "; changes_num++;
            }
            if (dr["ConsignorCellPhone"].ToString() != ConsignorCellPhone.Text.Trim())
            {
                changes_str += "【发货人手机】 由:" + dr["ConsignorCellPhone"].ToString() + " 改为:" + ConsignorCellPhone.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ConsignorCellPhone='" + ConsignorCellPhone.Text.Trim() + "' "; changes_num++;
            }
            if (dr["ConsignorPhone"].ToString() != ConsignorPhone.Text.Trim())
            {
                changes_str += "【发货人座机】 由:" + dr["ConsignorPhone"].ToString() + " 改为:" + ConsignorPhone.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ConsignorPhone='" + ConsignorPhone.Text.Trim() + "' "; changes_num++;
            }
            if (dr["ConsignorName"].ToString() != ConsignorName.Text.Trim())
            {
                changes_str += "【发货联系人】 由:" + dr["ConsignorName"].ToString() + " 改为:" + ConsignorName.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ConsignorName='" + ConsignorName.Text.Trim() + "' "; changes_num++;
            }
            if (dr["ConsignorCompany"].ToString() != ConsignorCompany.Text.Trim())
            {
                changes_str += "【发货公司名称】 由:" + dr["ConsignorCompany"].ToString() + " 改为:" + ConsignorCompany.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ConsignorCompany='" + ConsignorCompany.Text.Trim() + "' "; changes_num++;
            }
            if (dr["ReceivMode"].ToString() != ReceivMode.Text.Trim())
            {
                changes_str += "【接货方式】 由:" + dr["ReceivMode"].ToString() + " 改为:" + ReceivMode.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ReceivMode='" + ReceivMode.Text.Trim() + "' "; changes_num++;
            }
            if (dr["CusNo"].ToString() != CusNo.Text.Trim())
            {
                changes_str += "【客户编号】 由:" + dr["CusNo"].ToString() + " 改为:" + CusNo.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " CusNo='" + CusNo.Text.Trim() + "' "; changes_num++;
            }
            if (dr["CusType"].ToString() != CusType.Text.Trim())
            {
                changes_str += "【客户类型】 由:" + dr["CusType"].ToString() + " 改为:" + CusType.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " CusType='" + CusType.Text.Trim() + "' "; changes_num++;
            }
            if (dr["ValuationType"].ToString() != ValuationType.Text.Trim())
            {
                changes_str += "【计价类型】 由:" + dr["ValuationType"].ToString() + " 改为:" + ValuationType.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ValuationType='" + ValuationType.Text.Trim() + "' "; changes_num++;
            }
            if (dr["ReceivOrderNo"].ToString() != ReceivOrderNo.Text.Trim())
            {
                changes_str += "【接货单号】 由:" + dr["ReceivOrderNo"].ToString() + " 改为:" + ReceivOrderNo.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ReceivOrderNo='" + ReceivOrderNo.Text.Trim() + "' "; changes_num++;
            }
            if (dr["Salesman"].ToString() != Salesman.Text.Trim())
            {
                changes_str += "【业务员】 由:" + dr["Salesman"].ToString() + " 改为:" + Salesman.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " Salesman='" + Salesman.Text.Trim() + "' "; changes_num++;
            }
            if (dr["AlienGoods"].ToString() != (AlienGoods.Checked ? "1" : "0"))
            {
                changes_str += "【异形货物】 由:" + dr["AlienGoods"].ToString() + " 改为:" + (AlienGoods.Checked ? "1" : "0");
                sqlstr += (changes_num > 0 ? " , " : " ") + " AlienGoods='" + (AlienGoods.Checked ? 1 : 0) + "' "; changes_num++;
            }
            if (dr["GoodsVoucher"].ToString() != (GoodsVoucher.Checked ? "1" : "0"))
            {
                changes_str += "【货物单据】 由:" + dr["GoodsVoucher"].ToString() + " 改为:" + (GoodsVoucher.Checked ? "1" : "0");
                sqlstr += (changes_num > 0 ? " , " : " ") + " GoodsVoucher=" + (GoodsVoucher.Checked ? 1 : 0) + " "; changes_num++;
            }
            if (dr["PreciousGoods"].ToString() != (PreciousGoods.Checked ? "1" : "0"))
            {
                changes_str += "【贵重货物】 由:" + dr["PreciousGoods"].ToString() + " 改为:" + (PreciousGoods.Checked ? "1" : "0");
                sqlstr += (changes_num > 0 ? " , " : " ") + " PreciousGoods='" + (PreciousGoods.Checked ? 1 : 0) + "' "; changes_num++;
            }
            if (dr["NoticeState"].ToString() != (NoticeState.Checked ? "1" : "0"))
            {
                changes_str += "【是否控货】 由:" + dr["NoticeState"].ToString() + " 改为:" + (NoticeState.Checked ? "1" : "0");
                sqlstr += (changes_num > 0 ? " , " : " ") + " NoticeState='" + (NoticeState.Checked ? 1 : 0) + "' "; changes_num++;
            }

            if (dr["GoodsType"].ToString() != GoodsType)
            {
                changes_str += "【货物类型】 由:" + dr["GoodsType"].ToString() + " 改为:" + GoodsType;
                sqlstr += (changes_num > 0 ? " , " : " ") + " GoodsType='" + GoodsType + "' "; changes_num++;
            }
            if (dr["Varieties"].ToString() != Varieties)
            {
                changes_str += "【品名】 由:" + dr["Varieties"].ToString() + " 改为:" + Varieties;
                sqlstr += (changes_num > 0 ? " , " : " ") + " Varieties='" + Varieties + "' "; changes_num++;
            }
            if (dr["Package"].ToString() != Package)
            {
                changes_str += "【包装】 由:" + dr["Package"].ToString() + " 改为:" + Package;
                sqlstr += (changes_num > 0 ? " , " : " ") + " Package='" + Package + "' "; changes_num++;
            }

            acc = ConvertType.ToDecimal(dr["Num"], "0");
            acc2 = ConvertType.ToDecimal(Num, "0");
            if (acc != acc2)
            {
                changes_str += "【件数】 由:" + acc + " 改为:" + acc2;
                sqlstr += (changes_num > 0 ? " , " : " ") + " Num='" + acc2 + "' "; changes_num++;

                int BillState = ConvertType.ToInt32(dsModified.Tables[0].Rows[0]["BillState"]);
                int LeftNum = ConvertType.ToInt32(dsModified.Tables[0].Rows[0]["LeftNum"]);
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
            //if (dr["LeftNum"].ToString() != Num.ToString())
            //{
            //    changes_str += "【剩余件数】 由:" + dr["LeftNum"].ToString() + " 改为:" + Num.ToString();
            //    sqlstr += (changes_num > 0 ? " , " : " ") + " LeftNum='" + Num.ToString() + "' "; changes_num++;
            //}

            acc = ConvertType.ToDecimal(dr["FeeWeight"], "0");
            acc2 = ConvertType.ToDecimal(FeeWeight, "0");
            if (acc != acc2)
            {
                changes_str += "【计费重量】 由:" + acc + " 改为:" + acc2;
                sqlstr += (changes_num > 0 ? " , " : " ") + " FeeWeight='" + acc2 + "' "; changes_num++;
            }

            acc = ConvertType.ToDecimal(dr["FeeVolume"], "0");
            acc2 = ConvertType.ToDecimal(FeeVolume, "0");
            if (acc != acc2)
            {
                changes_str += "【计费体积】 由:" + acc + " 改为:" + acc2;
                sqlstr += (changes_num > 0 ? " , " : " ") + " FeeVolume='" + acc2 + "' "; changes_num++;
            }

            acc = ConvertType.ToDecimal(dr["Weight"], "0");
            acc2 = ConvertType.ToDecimal(Weight, "0");
            if (acc != acc2)
            {
                changes_str += "【重量】 由:" + acc + " 改为:" + acc2;
                sqlstr += (changes_num > 0 ? " , " : " ") + " Weight='" + acc2 + "' "; changes_num++;
            }

            acc = ConvertType.ToDecimal(dr["Volume"], "0");
            acc2 = ConvertType.ToDecimal(Volume, "0");
            if (acc != acc2)
            {
                changes_str += "【体积】 由:" + acc + " 改为:" + acc2;
                sqlstr += (changes_num > 0 ? " , " : " ") + " Volume='" + acc2 + "' "; changes_num++;
            }

            acc = ConvertType.ToDecimal(dr["WeightPrice"], "0");
            acc2 = ConvertType.ToDecimal(WeightPrice, "0");
            if (acc != acc2)
            {
                changes_str += "【重量单价】 由:" + acc + " 改为:" + acc2;
                sqlstr += (changes_num > 0 ? " , " : " ") + " WeightPrice='" + acc2 + "' "; changes_num++;
            }

            acc = ConvertType.ToDecimal(dr["VolumePrice"], "0");
            acc2 = ConvertType.ToDecimal(VolumePrice, "0");
            if (acc != acc2)
            {
                changes_str += "【体积单价】 由:" + acc + " 改为:" + acc2;
                sqlstr += (changes_num > 0 ? " , " : " ") + " VolumePrice='" + acc2 + "' "; changes_num++;
            }
            if (dr["FeeType"].ToString() != FeeType.ToString())
            {
                changes_str += "【计费类型】 由:" + dr["FeeType"].ToString() + " 改为:" + FeeType.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " FeeType='" + FeeType.ToString() + "' "; changes_num++;
            }

            acc = ConvertType.ToDecimal(dr["Freight"], "0");
            acc2 = ConvertType.ToDecimal(Freight, "0");
            if (acc != acc2)
            {
                changes_str += "【基本运费】 由:" + acc + " 改为:" + acc2;
                sqlstr += (changes_num > 0 ? " , " : " ") + " Freight='" + acc2 + "' "; changes_num++;
            }

            acc = ConvertType.ToDecimal(dr["DeliFee"], "0");
            acc2 = ConvertType.ToDecimal(DeliFee, "0");
            if (acc != acc2)
            {
                changes_str += "【送货费】 由:" + acc + " 改为:" + acc2;
                sqlstr += (changes_num > 0 ? " , " : " ") + " DeliFee='" + acc2 + "' "; changes_num++;
            }
            if (dr["ReceivFee"].ToString() != ReceivFee.ToString())
            {
                changes_str += "【接货费】 由:" + dr["ReceivFee"].ToString() + " 改为:" + ReceivFee.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ReceivFee='" + ReceivFee.ToString() + "' "; changes_num++;
            }
            if (dr["DeclareValue"].ToString() != DeclareValue.ToString())
            {
                changes_str += "【声明价值】 由:" + dr["DeclareValue"].ToString() + " 改为:" + DeclareValue.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " DeclareValue='" + DeclareValue.ToString() + "' "; changes_num++;
            }
            if (dr["SupportValue"].ToString() != SupportValue.ToString())
            {
                changes_str += "【保价费】 由:" + dr["SupportValue"].ToString() + " 改为:" + SupportValue.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " SupportValue='" + SupportValue.ToString() + "' "; changes_num++;
            }
            if (dr["Tax"].ToString() != Tax.ToString())
            {
                changes_str += "【税金】 由:" + dr["Tax"].ToString() + " 改为:" + Tax.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " Tax='" + Tax.ToString() + "' "; changes_num++;
            }
            if (dr["InformationFee"].ToString() != InformationFee.ToString())
            {
                changes_str += "【信息费】 由:" + dr["InformationFee"].ToString() + " 改为:" + InformationFee.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " InformationFee='" + InformationFee.ToString() + "' "; changes_num++;
            }
            if (dr["Expense"].ToString() != Expense.ToString())
            {
                changes_str += "【工本费】 由:" + dr["Expense"].ToString() + " 改为:" + Expense.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " Expense='" + Expense.ToString() + "' "; changes_num++;
            }
            if (dr["NoticeFee"].ToString() != NoticeFee.ToString())
            {
                changes_str += "【等通知放货费】 由:" + dr["NoticeFee"].ToString() + " 改为:" + NoticeFee.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " NoticeFee='" + NoticeFee.ToString() + "' "; changes_num++;
            }
            if (dr["DiscountTransfer"].ToString() != DiscountTransfer.ToString())
            {
                changes_str += "【折扣折让】 由:" + dr["DiscountTransfer"].ToString() + " 改为:" + DiscountTransfer.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " DiscountTransfer='" + DiscountTransfer.ToString() + "' "; changes_num++;
            }
            if (dr["CollectionPay"].ToString() != CollectionPay.ToString())
            {
                changes_str += "【代收货款】 由:" + dr["CollectionPay"].ToString() + " 改为:" + CollectionPay.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " CollectionPay='" + CollectionPay.ToString() + "' "; changes_num++;
            }
            if (dr["AgentFee"].ToString() != AgentFee.ToString())
            {
                changes_str += "【代收手续费】 由:" + dr["AgentFee"].ToString() + " 改为:" + AgentFee.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " AgentFee='" + AgentFee.ToString() + "' "; changes_num++;
            }
            if (dr["FuelFee"].ToString() != FuelFee.ToString())
            {
                changes_str += "【燃油费】 由:" + dr["FuelFee"].ToString() + " 改为:" + FuelFee.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " FuelFee='" + FuelFee.ToString() + "' "; changes_num++;
            }
            if (dr["UpstairFee"].ToString() != UpstairFee.ToString())
            {
                changes_str += "【上楼费】 由:" + dr["UpstairFee"].ToString() + " 改为:" + UpstairFee.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " UpstairFee='" + UpstairFee.ToString() + "' "; changes_num++;
            }
            if (dr["HandleFee"].ToString() != HandleFee.ToString())
            {
                changes_str += "【装卸费】 由:" + dr["HandleFee"].ToString() + " 改为:" + HandleFee.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " HandleFee='" + HandleFee.ToString() + "' "; changes_num++;
            }
            if (dr["ForkliftFee"].ToString() != ForkliftFee.ToString())
            {
                changes_str += "【叉车费】 由:" + dr["ForkliftFee"].ToString() + " 改为:" + ForkliftFee.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ForkliftFee='" + ForkliftFee.ToString() + "' "; changes_num++;
            }
            if (dr["StorageFee"].ToString() != StorageFee.ToString())
            {
                changes_str += "【进仓费】 由:" + dr["StorageFee"].ToString() + " 改为:" + StorageFee.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " StorageFee='" + StorageFee.ToString() + "' "; changes_num++;
            }
            if (dr["CustomsFee"].ToString() != CustomsFee.ToString())
            {
                changes_str += "【报关费】 由:" + dr["CustomsFee"].ToString() + " 改为:" + CustomsFee.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " CustomsFee='" + CustomsFee.ToString() + "' "; changes_num++;
            }
            if (dr["packagFee"].ToString() != packagFee.ToString())
            {
                changes_str += "【包装费】 由:" + dr["packagFee"].ToString() + " 改为:" + packagFee.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " packagFee='" + packagFee.ToString() + "' "; changes_num++;
            }
            if (dr["FrameFee"].ToString() != FrameFee.ToString())
            {
                changes_str += "【代打木架费】 由:" + dr["FrameFee"].ToString() + " 改为:" + FrameFee.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " FrameFee='" + FrameFee.ToString() + "' "; changes_num++;
            }
            if (dr["ChangeFee"].ToString() != ChangeFee.ToString())
            {
                changes_str += "【改单费】 由:" + dr["ChangeFee"].ToString() + " 改为:" + ChangeFee.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ChangeFee='" + ChangeFee.ToString() + "' "; changes_num++;
            }
            if (dr["OtherFee"].ToString() != OtherFee.ToString())
            {
                changes_str += "【其它费】 由:" + dr["OtherFee"].ToString() + " 改为:" + OtherFee.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " OtherFee='" + OtherFee.ToString() + "' "; changes_num++;
            }
            //if (dr["IsInvoice"].ToString() != (IsInvoice.Checked ? "1" : "0"))
            //{
            //    changes_str += "【是否开发票】 由:" + dr["IsInvoice"].ToString() + " 改为:" + (IsInvoice.Checked ? "1" : "0");
            //    sqlstr += (changes_num > 0 ? " , " : " ") + " IsInvoice='" + (IsInvoice.Checked ? "1" : "0") + "' "; changes_num++;
            //}
            if (dr["ReceiptFee"].ToString() != ReceiptFee.ToString())
            {
                changes_str += "【回单费】 由:" + dr["ReceiptFee"].ToString() + " 改为:" + ReceiptFee.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ReceiptFee='" + ReceiptFee.ToString() + "' "; changes_num++;
            }
            if (dr["WarehouseFee"].ToString() != WarehouseFee.ToString())
            {
                changes_str += "【仓储费】 由:" + dr["WarehouseFee"].ToString() + " 改为:" + WarehouseFee.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " WarehouseFee='" + WarehouseFee.ToString() + "' "; changes_num++;
            }

            if (dr["ReceiptCondition"].ToString() != ReceiptCondition.Text.ToString())
            {
                changes_str += "【回单要求】 由:" + dr["ReceiptCondition"].ToString() + " 改为:" + ReceiptCondition.Text.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " ReceiptCondition='" + ReceiptCondition.Text.Trim() + "' "; changes_num++;
            }
            if (dr["CouponsNo"].ToString() != CouponsNo.Text.ToString())
            {
                changes_str += "【优惠卷号】 由:" + dr["CouponsNo"].ToString() + " 改为:" + CouponsNo.Text.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " CouponsNo='" + CouponsNo.Text.Trim() + "' "; changes_num++;
            }
            if (dr["CouponsAmount"].ToString() != CouponsAmount.Text.ToString())
            {
                changes_str += "【优惠金额】 由:" + dr["CouponsAmount"].ToString() + " 改为:" + CouponsAmount.Text.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " CouponsAmount='" + CouponsAmount.Text.Trim() + "' "; changes_num++;
            }
            if (dr["PaymentMode"].ToString() != PaymentMode.Text.ToString())
            {
                changes_str += "【付款方式】 由:" + dr["PaymentMode"].ToString() + " 改为:" + PaymentMode.Text.ToString();
                sqlstr += (changes_num > 0 ? " , " : " ") + " PaymentMode='" + PaymentMode.Text.Trim() + "' "; changes_num++;
            }
            if (dr["PaymentAmout"].ToString() != (PaymentAmout.Text.Trim() == "" ? "0" : PaymentAmout.Text.Trim()))
            {
                changes_str += "【总运费】 由:" + dr["PaymentAmout"].ToString() + " 改为:" + (PaymentAmout.Text.Trim() == "" ? "0" : PaymentAmout.Text.Trim());
                sqlstr += (changes_num > 0 ? " , " : " ") + " PaymentAmout='" + (PaymentAmout.Text.Trim() == "" ? "0" : PaymentAmout.Text.Trim()) + "' "; changes_num++;
            }
            if (ConvertType.ToDecimal(dr["ActualFreight"].ToString()) != ActualFreight)
            {
                changes_str += "【实收金额】 由:" + dr["ActualFreight"].ToString() + " 改为:" + ActualFreight;
                sqlstr += (changes_num > 0 ? " , " : " ") + " ActualFreight='" + ActualFreight + "' "; changes_num++;
            }

            acc = ConvertType.ToDecimal(dr["NowPay"], "0");
            acc2 = ConvertType.ToDecimal(NowPay.Text, "0");
            if (acc != acc2)
            {
                changes_str += "【现付】 由:" + acc + " 改为:" + acc2;
                sqlstr += (changes_num > 0 ? " , " : " ") + " NowPay='" + acc2 + "' "; changes_num++;
            }

            acc = ConvertType.ToDecimal(dr["FetchPay"], "0");
            acc2 = ConvertType.ToDecimal(FetchPay.Text, "0");
            if (acc != acc2)
            {
                changes_str += "【提付】 由:" + acc + " 改为:" + acc2;
                sqlstr += (changes_num > 0 ? " , " : " ") + " FetchPay='" + acc2 + "' "; changes_num++;
            }

            acc = ConvertType.ToDecimal(dr["MonthPay"], "0");
            acc2 = ConvertType.ToDecimal(MonthPay.Text, "0");
            if (acc != acc2)
            {
                changes_str += "【月结】 由:" + acc + " 改为:" + acc2;
                sqlstr += (changes_num > 0 ? " , " : " ") + " MonthPay='" + acc2 + "' "; changes_num++;
            }

            acc = ConvertType.ToDecimal(dr["ShortOwePay"], "0");
            acc2 = ConvertType.ToDecimal(ShortOwePay.Text, "0");
            if (acc != acc2)
            {
                changes_str += "【短欠】 由:" + acc + " 改为:" + acc2;
                sqlstr += (changes_num > 0 ? " , " : " ") + " ShortOwePay='" + acc2 + "' "; changes_num++;
            }

            acc = ConvertType.ToDecimal(dr["BefArrivalPay"], "0");
            acc2 = ConvertType.ToDecimal(BefArrivalPay.Text, "0");
            if (acc != acc2)
            {
                changes_str += "【货到前付】 由:" + acc + " 改为:" + acc2;
                sqlstr += (changes_num > 0 ? " , " : " ") + " BefArrivalPay='" + acc2 + "' "; changes_num++;
            }
            if (dr["BillRemark"].ToString() != BillRemark.Text.Trim())
            {
                changes_str += "【备注】 由:" + dr["BillRemark"].ToString() + " 改为:" + BillRemark.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " BillRemark='" + BillRemark.Text.Trim() + "' "; changes_num++;
            }
            if (dr["WebPlatform"].ToString() != WebPlatform.Text.Trim())
            {
                changes_str += "【网络平台】 由:" + dr["WebPlatform"].ToString() + " 改为:" + WebPlatform.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " WebPlatform='" + WebPlatform.Text.Trim() + "' "; changes_num++;
            }
            if (dr["WebBillNo"].ToString() != WebBillNo.Text.Trim())
            {
                changes_str += "【网络订单号】 由:" + dr["WebBillNo"].ToString() + " 改为:" + WebBillNo.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " WebBillNo='" + WebBillNo.Text.Trim() + "' "; changes_num++;
            }
            if (dr["DisTranName"].ToString() != DisTranName.Text.Trim())
            {
                changes_str += "【折扣折让收款名称】 由:" + dr["DisTranName"].ToString() + " 改为:" + DisTranName.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " DisTranName='" + DisTranName.Text.Trim() + "' "; changes_num++;
            }
            if (dr["DisTranBank"].ToString() != DisTranBank.Text.Trim())
            {
                changes_str += "【折扣折让收款银行】 由:" + dr["DisTranBank"].ToString() + " 改为:" + DisTranBank.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " DisTranBank='" + DisTranBank.Text.Trim() + "' "; changes_num++;
            }
            if (dr["DisTranAccount"].ToString() != DisTranAccount.Text.Trim())
            {
                changes_str += "【折扣折让收款账号】 由:" + dr["DisTranAccount"].ToString() + " 改为:" + DisTranAccount.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " DisTranAccount='" + DisTranAccount.Text.Trim() + "' "; changes_num++;
            }
            if (dr["CollectionName"].ToString() != CollectionName.Text.Trim())
            {
                changes_str += "【代收货款收款名称】 由:" + dr["CollectionName"].ToString() + " 改为:" + CollectionName.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " CollectionName='" + CollectionName.Text.Trim() + "' "; changes_num++;
            }
            if (dr["CollectionBank"].ToString() != CollectionBank.Text.Trim())
            {
                changes_str += "【代收货款收款银行】 由:" + dr["CollectionBank"].ToString() + " 改为:" + CollectionBank.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " CollectionBank='" + CollectionBank.Text.Trim() + "' "; changes_num++;
            }
            if (dr["CollectionAccount"].ToString() != CollectionAccount.Text.Trim())
            {
                changes_str += "【代收货款收款账号】 由:" + dr["CollectionAccount"].ToString() + " 改为:" + CollectionAccount.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " CollectionAccount='" + CollectionAccount.Text.Trim() + "' "; changes_num++;
            }
            if (dr["CauseName"].ToString() != CauseName.Text.Trim())
            {
                changes_str += "【开单事业部】 由:" + dr["CauseName"].ToString() + " 改为:" + CauseName.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " CauseName='" + CauseName.Text.Trim() + "' "; changes_num++;
            }
            if (dr["AreaName"].ToString() != AreaName.Text.Trim())
            {
                changes_str += "【开单大区】 由:" + dr["AreaName"].ToString() + " 改为:" + AreaName.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " AreaName='" + AreaName.Text.Trim() + "' "; changes_num++;
            }
            if (dr["DepName"].ToString() != DepName.Text.Trim())
            {
                changes_str += "【开单部门】 由:" + dr["DepName"].ToString() + " 改为:" + DepName.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " DepName='" + DepName.Text.Trim() + "' "; changes_num++;
            }
            if (dr["DisTranBranch"].ToString() != DisTranBranch.Text.Trim())
            {
                changes_str += "【折扣折让所属支行】 由:" + dr["DisTranBranch"].ToString() + " 改为:" + DisTranBranch.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " DisTranBranch='" + DisTranBranch.Text.Trim() + "' "; changes_num++;
            }
            if (dr["CollectionBranch"].ToString() != CollectionBranch.Text.Trim())
            {
                changes_str += "【代收款所属支行】 由:" + dr["CollectionBranch"].ToString() + " 改为:" + CollectionBranch.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " CollectionBranch='" + CollectionBranch.Text.Trim() + "' "; changes_num++;
            }
            if (dr["BillMan"].ToString() != BillMan.Text.Trim())
            {
                changes_str += "【开单人】 由:" + dr["BillMan"].ToString() + " 改为:" + BillMan.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " BillMan='" + BillMan.Text.Trim() + "' "; changes_num++;
            }
            if (dr["begWeb"].ToString() != begWeb.Text.Trim())
            {
                changes_str += "【开单网点】 由:" + dr["begWeb"].ToString() + " 改为:" + begWeb.Text.Trim();
                sqlstr += (changes_num > 0 ? " , " : " ") + " begWeb='" + begWeb.Text.Trim() + "' "; changes_num++;
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
            int IsFrameFee_c = ConvertType.ToInt32(dr["IsFrameFee"]);

            if (NoticeState_s != NoticeState_c)
            {
                changes_str += "【控货】 由:" + (NoticeState_c > 0 ? "是" : "否") + " 改为:" + (NoticeState_s > 0 ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " NoticeState=" + NoticeState_s + " "; changes_num++;
            }
            if (IsInvoice_s != IsInvoice_c)
            {
                changes_str += "【发票】 由:" + (IsInvoice_c > 0 ? "是" : "否") + " 改为:" + (IsInvoice_s > 0 ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " IsInvoice=" + IsInvoice_s + " "; changes_num++;
            }
            if (NoticeState_s != NoticeState_c)
            {
                changes_str += "【回单】 由:" + (NoticeState_c > 0 ? "是" : "否") + " 改为:" + (NoticeState_s > 0 ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " NoticeState=" + NoticeState_s + " "; changes_num++;
            }
            if (IsSupportValue_s != IsSupportValue_c)
            {
                changes_str += "【保价】 由:" + (IsSupportValue_c > 0 ? "是" : "否") + " 改为:" + (IsSupportValue_s > 0 ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " IsSupportValue=" + IsSupportValue_s + " "; changes_num++;
            }
            if (IsAgentFee_s != IsAgentFee_c)
            {
                changes_str += "【代收】 由:" + (IsAgentFee_c > 0 ? "是" : "否") + " 改为:" + (IsAgentFee_s > 0 ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " IsAgentFee=" + IsAgentFee_s + " "; changes_num++;
            }

            if (IsPackagFee_s != IsPackagFee_c)
            {
                changes_str += "【包装】 由:" + (IsPackagFee_c > 0 ? "是" : "否") + " 改为:" + (IsPackagFee_s > 0 ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " IsPackagFee=" + IsPackagFee_s + " "; changes_num++;
            }
            if (IsOtherFee_s != IsOtherFee_c)
            {
                changes_str += "【其它】 由:" + (IsOtherFee_c > 0 ? "是" : "否") + " 改为:" + (IsOtherFee_s > 0 ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " IsOtherFee=" + IsOtherFee_s + " "; changes_num++;
            }
            if (IsHandleFee_s != IsHandleFee_c)
            {
                changes_str += "【装卸】 由:" + (IsHandleFee_c > 0 ? "是" : "否") + " 改为:" + (IsHandleFee_s > 0 ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " IsHandleFee=" + IsHandleFee_s + " "; changes_num++;
            }
            if (IsStorageFee_s != IsStorageFee_c)
            {
                changes_str += "【进仓】 由:" + (IsStorageFee_c > 0 ? "是" : "否") + " 改为:" + (IsStorageFee_s > 0 ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " IsStorageFee=" + IsStorageFee_s + " "; changes_num++;
            }
            if (IsWarehouseFee_s != IsWarehouseFee_c)
            {
                changes_str += "【仓储】 由:" + (IsWarehouseFee_c > 0 ? "是" : "否") + " 改为:" + (IsWarehouseFee_s > 0 ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " IsWarehouseFee=" + IsWarehouseFee_s + " "; changes_num++;
            }

            if (IsForkliftFee_s != IsForkliftFee_c)
            {
                changes_str += "【叉车】 由:" + (IsForkliftFee_c > 0 ? "是" : "否") + " 改为:" + (IsForkliftFee_s > 0 ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " IsForkliftFee=" + IsForkliftFee_s + " "; changes_num++;
            }
            if (IsChangeFee_s != IsChangeFee_c)
            {
                changes_str += "【改单】 由:" + (IsChangeFee_c > 0 ? "是" : "否") + " 改为:" + (IsChangeFee_s > 0 ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " IsChangeFee=" + IsChangeFee_s + " "; changes_num++;
            }
            if (IsUpstairFee_s != IsUpstairFee_c)
            {
                changes_str += "【上楼】 由:" + (IsUpstairFee_c > 0 ? "是" : "否") + " 改为:" + (IsUpstairFee_s > 0 ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " IsUpstairFee=" + IsUpstairFee_s + " "; changes_num++;
            }
            if (IsCustomsFee_s != IsCustomsFee_c)
            {
                changes_str += "【报关】 由:" + (IsCustomsFee_c > 0 ? "是" : "否") + " 改为:" + (IsCustomsFee_s > 0 ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " IsCustomsFee=" + IsCustomsFee_s + " "; changes_num++;
            }
            if (IsFrameFee_s != IsFrameFee_c)
            {
                changes_str += "【木架】 由:" + (IsFrameFee_c > 0 ? "是" : "否") + " 改为:" + (IsFrameFee_s > 0 ? "是" : "否");
                sqlstr += (changes_num > 0 ? " , " : " ") + " IsFrameFee=" + IsFrameFee_s + " "; changes_num++;
            }

            if (dr["WarehouseFee"].ToString() != WarehouseFee.ToString())
            {
                changes_str += "【仓储费】 由:" + dr["WarehouseFee"].ToString() + " 改为:" + WarehouseFee;
                sqlstr += (changes_num > 0 ? " , " : " ") + " WarehouseFee=" + WarehouseFee + " "; changes_num++;
            }

            if (ConvertType.ToDecimal(dr["ReceiptFee_C"]) != ReceiptFee_C)
            {
                changes_str += "【结算回单费】 由:" + dr["ReceiptFee_C"].ToString() + " 改为:" + WarehouseFee;
                sqlstr += (changes_num > 0 ? " , " : " ") + " ReceiptFee_C=" + ReceiptFee_C + " "; changes_num++;
            }
            if (ConvertType.ToDecimal(dr["NoticeFee_C"]) != NoticeFee_C)
            {
                changes_str += "【结算控货费】 由:" + dr["NoticeFee_C"].ToString() + " 改为:" + NoticeFee_C;
                sqlstr += (changes_num > 0 ? " , " : " ") + " NoticeFee_C=" + NoticeFee_C + " "; changes_num++;
            }
            if (ConvertType.ToDecimal(dr["SupportValue_C"]) != SupportValue_C)
            {
                changes_str += "【结算保价费】 由:" + dr["SupportValue_C"].ToString() + " 改为:" + SupportValue_C;
                sqlstr += (changes_num > 0 ? " , " : " ") + " SupportValue_C=" + SupportValue_C + " "; changes_num++;
            }
            if (ConvertType.ToDecimal(dr["AgentFee_C"]) != AgentFee_C)
            {
                changes_str += "【结算代收手续费】 由:" + dr["AgentFee_C"].ToString() + " 改为:" + AgentFee_C;
                sqlstr += (changes_num > 0 ? " , " : " ") + " AgentFee_C=" + AgentFee_C + " "; changes_num++;
            }
            if (ConvertType.ToDecimal(dr["PackagFee_C"]) != PackagFee_C)
            {
                changes_str += "【结算包装费】 由:" + dr["PackagFee_C"].ToString() + " 改为:" + PackagFee_C;
                sqlstr += (changes_num > 0 ? " , " : " ") + " PackagFee_C=" + PackagFee_C + " "; changes_num++;
            }

            if (ConvertType.ToDecimal(dr["OtherFee_C"]) != OtherFee_C)
            {
                changes_str += "【结算其它费】 由:" + dr["OtherFee_C"].ToString() + " 改为:" + OtherFee_C;
                sqlstr += (changes_num > 0 ? " , " : " ") + " OtherFee_C=" + OtherFee_C + " "; changes_num++;
            }
            if (ConvertType.ToDecimal(dr["HandleFee_C"]) != HandleFee_C)
            {
                changes_str += "【结算装卸费】 由:" + dr["HandleFee_C"].ToString() + " 改为:" + HandleFee_C;
                sqlstr += (changes_num > 0 ? " , " : " ") + " HandleFee_C=" + HandleFee_C + " "; changes_num++;
            }
            if (ConvertType.ToDecimal(dr["StorageFee_C"]) != StorageFee_C)
            {
                changes_str += "【结算进仓费】 由:" + dr["StorageFee_C"].ToString() + " 改为:" + StorageFee_C;
                sqlstr += (changes_num > 0 ? " , " : " ") + " StorageFee_C=" + StorageFee_C + " "; changes_num++;
            }
            if (ConvertType.ToDecimal(dr["WarehouseFee_C"]) != WarehouseFee_C)
            {
                changes_str += "【结算仓储费】 由:" + dr["WarehouseFee_C"].ToString() + " 改为:" + WarehouseFee_C;
                sqlstr += (changes_num > 0 ? " , " : " ") + " WarehouseFee_C=" + WarehouseFee_C + " "; changes_num++;
            }
            if (ConvertType.ToDecimal(dr["ForkliftFee_C"]) != ForkliftFee_C)
            {
                changes_str += "【结算叉车费】 由:" + dr["ForkliftFee_C"].ToString() + " 改为:" + ForkliftFee_C;
                sqlstr += (changes_num > 0 ? " , " : " ") + " ForkliftFee_C=" + ForkliftFee_C + " "; changes_num++;
            }

            if (ConvertType.ToDecimal(dr["Tax_C"]) != Tax_C)
            {
                changes_str += "【结算税金费】 由:" + dr["Tax_C"].ToString() + " 改为:" + Tax_C;
                sqlstr += (changes_num > 0 ? " , " : " ") + " Tax_C=" + Tax_C + " "; changes_num++;
            }
            if (ConvertType.ToDecimal(dr["ChangeFee_C"]) != ChangeFee_C)
            {
                changes_str += "【结算改单费】 由:" + dr["ChangeFee_C"].ToString() + " 改为:" + ChangeFee_C;
                sqlstr += (changes_num > 0 ? " , " : " ") + " ChangeFee_C=" + ChangeFee_C + " "; changes_num++;
            }
            if (ConvertType.ToDecimal(dr["UpstairFee_C"]) != UpstairFee_C)
            {
                changes_str += "【结算上楼费】 由:" + dr["UpstairFee_C"].ToString() + " 改为:" + UpstairFee_C;
                sqlstr += (changes_num > 0 ? " , " : " ") + " UpstairFee_C=" + UpstairFee_C + " "; changes_num++;
            }
            if (ConvertType.ToDecimal(dr["CustomsFee_C"]) != CustomsFee_C)
            {
                changes_str += "【结算报关费】 由:" + dr["CustomsFee_C"].ToString() + " 改为:" + CustomsFee_C;
                sqlstr += (changes_num > 0 ? " , " : " ") + " CustomsFee_C=" + CustomsFee_C + " "; changes_num++;
            }
            if (ConvertType.ToDecimal(dr["FrameFee_C"]) != FrameFee_C)
            {
                changes_str += "【结算木架费】 由:" + dr["FrameFee_C"].ToString() + " 改为:" + FrameFee_C;
                sqlstr += (changes_num > 0 ? " , " : " ") + " FrameFee_C=" + FrameFee_C + " "; changes_num++;
            }

            if (ConvertType.ToDecimal(dr["Expense_C"]) != Expense_C)
            {
                changes_str += "【结算工本费】 由:" + dr["Expense_C"].ToString() + " 改为:" + Expense_C;
                sqlstr += (changes_num > 0 ? " , " : " ") + " Expense_C=" + Expense_C + " "; changes_num++;
            }
            if (ConvertType.ToDecimal(dr["FuelFee_C"]) != FuelFee_C)
            {
                changes_str += "【结算燃油费】 由:" + dr["FuelFee_C"].ToString() + " 改为:" + FuelFee_C;
                sqlstr += (changes_num > 0 ? " , " : " ") + " FuelFee_C=" + FuelFee_C + " "; changes_num++;
            }
            if (ConvertType.ToDecimal(dr["InformationFee_C"]) != InformationFee_C)
            {
                changes_str += "【结算信息费】 由:" + dr["InformationFee_C"].ToString() + " 改为:" + InformationFee_C;
                sqlstr += (changes_num > 0 ? " , " : " ") + " InformationFee_C=" + InformationFee_C + " "; changes_num++;
            }
            if (ConvertType.ToDecimal(dr["MainlineFee"]) != MainlineFee)//1
            {
                changes_str += "【结算干线费】 由:" + dr["MainlineFee"].ToString() + " 改为:" + MainlineFee;
                sqlstr += (changes_num > 0 ? " , " : " ") + " MainlineFee=" + MainlineFee + " "; changes_num++;
            }
            if (ConvertType.ToDecimal(dr["DeliveryFee"]) != DeliveryFee)//2
            {
                changes_str += "【结算送货费】 由:" + dr["DeliveryFee"].ToString() + " 改为:" + DeliveryFee;
                sqlstr += (changes_num > 0 ? " , " : " ") + " DeliveryFee=" + DeliveryFee + " "; changes_num++;
            }
            if (ConvertType.ToDecimal(dr["TransferFee"]) != TransferFee)//3
            {
                changes_str += "【结算中转费】 由:" + dr["TransferFee"].ToString() + " 改为:" + TransferFee;
                sqlstr += (changes_num > 0 ? " , " : " ") + " TransferFee=" + TransferFee + " "; changes_num++;
            }
            if (ConvertType.ToDecimal(dr["DepartureOptFee"]) != DepartureOptFee)//4
            {
                changes_str += "【结算始发操作费】 由:" + dr["DepartureOptFee"].ToString() + " 改为:" + DepartureOptFee;
                sqlstr += (changes_num > 0 ? " , " : " ") + " DepartureOptFee=" + DepartureOptFee + " "; changes_num++;
            }
            if (ConvertType.ToDecimal(dr["TerminalOptFee"]) != TerminalOptFee)//5
            {
                changes_str += "【结算终端操作费】 由:" + dr["TerminalOptFee"].ToString() + " 改为:" + TerminalOptFee;
                sqlstr += (changes_num > 0 ? " , " : " ") + " TerminalOptFee=" + TerminalOptFee + " "; changes_num++;
            }

            if (ConvertType.ToDecimal(dr["TerminalAllotFee"]) != TerminalAllotFee)//6
            {
                changes_str += "【结算终端分拨费】 由:" + dr["TerminalAllotFee"].ToString() + " 改为:" + TerminalAllotFee;
                sqlstr += (changes_num > 0 ? " , " : " ") + " TerminalAllotFee=" + TerminalAllotFee + " "; changes_num++;
            }
            if (ConvertType.ToDecimal(dr["OperationWeight"]) != OperationWeight)//6
            {
                changes_str += "【结算重量】 由:" + dr["OperationWeight"].ToString() + " 改为:" + OperationWeight;
                sqlstr += (changes_num > 0 ? " , " : " ") + " OperationWeight=" + OperationWeight + " "; changes_num++;
            }
            if (ConvertType.ToDecimal(dr["OptWeight"]) != OptWeight)//6
            {
                changes_str += "【操作重量】 由:" + dr["OptWeight"].ToString() + " 改为:" + OptWeight;
                sqlstr += (changes_num > 0 ? " , " : " ") + " OptWeight=" + OptWeight + " "; changes_num++;
            }

            string sqlSub = "delete from WayBillSub where billno='" + BillNo.Text.Trim() + "' ";
            sqlSub += "insert WayBillSub (BillNo,Varieties,Package,Num,FeeWeight,FeeVolume,Weight,Volume,WeightPrice,VolumePrice,FeeType,Freight)"
                + "SELECT '" + BillNo.Text.Trim() + "', Varieties,Package,Num,FeeWeight,FeeVolume,Weight,Volume,WeightPrice,VolumePrice,FeeType,Freight "
                + "FROM dbo.L_SplitStr_BillSub(" + (string.Format("'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}' ",
                new object[]{ VarietiesStr, PackageStr ,NumStr,FeeWeightStr,FeeVolumeStr,
                    WeightStr,VolumeStr,WeightPriceStr,VolumePriceStr,FeeTypeStr,FreightStr})) + ",'@')";

            if (changes_num == 0)
            {
                MsgBox.ShowOK("单据没有修改，不能提交改单申请!");
                return;
            }
            if (changes_str.Contains("【现付】") || changes_str.Contains("【提付】") || changes_str.Contains("【月结】") || changes_str.Contains("【短欠】") || changes_str.Contains("【货到前付】"))
            {
                if (CommonClass.GetAduitState(BillNO) > 0)
                {
                    MsgBox.ShowOK("运单已经审核或账款确认，不能执行！");
                    return;
                }
            }

            sqlstr += " where BillNo='" + BillNo.Text.Trim() + "' " + sqlSub;

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillNO", BillNo.Text.Trim()));
            list.Add(new SqlPara("ApplyContent", changes_str));
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
                MsgBox.ShowOK("操作成功！");
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
                SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_BILLAPPLY_HasApplying", list1);
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
            }
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
        }

        private void checkEdit5_CheckedChanged(object sender, EventArgs e)
        {
            if (IsUpstairFee.Checked)
            {
                gridView1.Columns["UpstairFee"].Visible = true;
                gridView1.Columns["UpstairFee"].OptionsColumn.AllowEdit = true;
                gridView1.Columns["UpstairFee"].OptionsColumn.AllowFocus = true;
            }
            else
            {
                gridView1.Columns["UpstairFee"].Visible = false;
                gridView1.Columns["UpstairFee"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["UpstairFee"].OptionsColumn.AllowFocus = false;
                gridView1.SetRowCellValue(0, "UpstairFee", "0");
            }

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
            if (IsHandleFee.Checked)
            {
                gridView1.Columns["HandleFee"].Visible = true;
                gridView1.Columns["HandleFee"].OptionsColumn.AllowEdit = true;
                gridView1.Columns["HandleFee"].OptionsColumn.AllowFocus = true;
            }
            else
            {
                gridView1.Columns["HandleFee"].Visible = false;
                gridView1.Columns["HandleFee"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["HandleFee"].OptionsColumn.AllowFocus = false;
                gridView1.SetRowCellValue(0, "HandleFee", "0");
            }
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
        }

        private void GoodsVoucher_CheckedChanged(object sender, EventArgs e)
        {
            if (GoodsVoucher.Checked)
            {
                gridView2.SetRowCellValue(0, "Varieties", "票据");
            }
            else
            {
                gridView2.SetRowCellValue(0, "Varieties", "纸箱");
            }
        }

        private void PreciousGoods_CheckedChanged(object sender, EventArgs e)
        {
            if (PreciousGoods.Checked)
            {
                IsSupportValue.Checked = true;
                IsSupportValue.Enabled = false;
            }
            else
            {
                IsSupportValue.Checked = false;
                IsSupportValue.Enabled = true;
            }
        }

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
                ReceivOrderNo.Text = dr_["DeliCode"].ToString();

                gcjiehuodanhao.Visible = false;
                Salesman.Focus();
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
                checkEdit3.Enabled = false;
                checkEdit3.Checked = false;
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
                //DataRow[] drMainlineFee = CommonClass.dsMainlineFee.Tables[0].Select("FromSite='" + Bsite + "' and TransferSite='" + MiddleSite + "' and TransportMode='" + TransitMode.Text.Trim() + "'");
                //if (drMainlineFee.Length > 0)
                //{
                //    decimal ParcelPriceMin = ConvertType.ToDecimal(drMainlineFee[0]["ParcelPriceMin"]);//最低一票
                //    decimal HeavyPrice = ConvertType.ToDecimal(drMainlineFee[0]["HeavyPrice"]);//重货
                //    decimal LightPrice = ConvertType.ToDecimal(drMainlineFee[0]["LightPrice"]);//轻货
                //    decimal MainlineFeeAll = 0;
                //    for (int i = 0; i < RowCount; i++)
                //    {
                //        decimal w = ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeWeight"));
                //        decimal v = ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeVolume"));
                //        string Package = gridView2.GetRowCellValue(i, "Package").ToString();
                //        decimal fee = Math.Max(w * HeavyPrice, v * LightPrice);
                //        if (Package != "纸箱")
                //        {
                //            MainlineFeeAll += fee * (decimal)1.1;
                //        }
                //        else
                //        {
                //            MainlineFeeAll += fee;
                //        }
                //    }
                //    decimal acc = Math.Max(MainlineFeeAll, ParcelPriceMin);
                //    gridView8.SetRowCellValue(0, "MainlineFee", Math.Round(acc,2));
                //}
                //else
                //{
                //    gridView8.SetRowCellValue(0, "MainlineFee", 0);
                //}
                #endregion

                #region 结算中转费
                //DataRow[] drTransferFee = CommonClass.dsTransferFee.Tables[0].Select("FromSite='" + Bsite + "' and TransferSite='" + MiddleSite + "' ");
                //if (drTransferFee.Length > 0 && TransferMode.Text!="网点送货")
                //{
                //    decimal HeavyPrice = ConvertType.ToDecimal(drTransferFee[0]["HeavyPrice"]);//重货
                //    decimal LightPrice = ConvertType.ToDecimal(drTransferFee[0]["LightPrice"]);//轻货
                //    decimal ParcelPriceMin = ConvertType.ToDecimal(drTransferFee[0]["ParcelPriceMin"]);//最低一票
                //    decimal TransferFeeAll = 0;
                //    for (int i = 0; i < RowCount; i++)
                //    {
                //        decimal w = ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeWeight"));
                //        decimal v = ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "FeeVolume"));
                //        string Package = gridView2.GetRowCellValue(i, "Package").ToString();
                //        decimal fee = Math.Max(w * HeavyPrice, v * LightPrice);
                //        if (Package != "纸箱")
                //        {
                //            TransferFeeAll += fee * (decimal)1.1;
                //        }
                //        else
                //        {
                //            TransferFeeAll += fee;
                //        }
                //    }
                //    decimal acc =  Math.Max(TransferFeeAll, ParcelPriceMin);
                //    gridView8.SetRowCellValue(0, "TransferFee", Math.Round(acc,2));
                //}
                //else
                //{
                //    gridView8.SetRowCellValue(0, "TransferFee", 0);
                //}
                #endregion

                #region 结算始发操作费
                //DataRow[] drDepartureOptFee = CommonClass.dsDepartureOptFee.Tables[0].Select("FromSite='" + Bsite + "'");
                //if (drDepartureOptFee.Length > 0)
                //{
                //    decimal HeavyPrice = ConvertType.ToDecimal(drDepartureOptFee[0]["HeavyPrice"]);//重货
                //    decimal LightPrice = ConvertType.ToDecimal(drDepartureOptFee[0]["LightPrice"]);//轻货
                //    decimal ParcelPriceMin = ConvertType.ToDecimal(drDepartureOptFee[0]["ParcelPriceMin"]);//最低一票
                //    //decimal acc = Math.Max(Weight * HeavyPrice, Volume * LightPrice);
                //    //acc = Math.Max(acc, ParcelPriceMin);
                //    //gridView8.SetRowCellValue(0, "DepartureOptFee", acc);

                //    decimal acc = Math.Max(Weight * HeavyPrice, ParcelPriceMin);
                //    gridView8.SetRowCellValue(0, "DepartureOptFee", Math.Round(acc,2));

                //}
                //else
                //{
                //    gridView8.SetRowCellValue(0, "DepartureOptFee", 0);
                //}
                #endregion

                #region 结算终端操作费
                //DataRow[] drTerminalOptFee = CommonClass.dsTerminalOptFee.Tables[0].Select("TransferSite='" + MiddleSite + "'");
                //if (drTerminalOptFee.Length > 0)
                //{
                //    decimal HeavyPrice = ConvertType.ToDecimal(drTerminalOptFee[0]["HeavyPrice"]);//重货
                //    decimal LightPrice = ConvertType.ToDecimal(drTerminalOptFee[0]["LightPrice"]);//轻货
                //    decimal ParcelPriceMin = ConvertType.ToDecimal(drTerminalOptFee[0]["ParcelPriceMin"]);//最低一票
                //    //decimal acc = Math.Max(Weight * HeavyPrice, Volume * LightPrice);
                //    //acc = Math.Max(acc, ParcelPriceMin);
                //    //gridView8.SetRowCellValue(0, "TerminalOptFee", acc);


                //    decimal acc = Math.Max(Weight * HeavyPrice, ParcelPriceMin);
                //    gridView8.SetRowCellValue(0, "TerminalOptFee", Math.Round(acc,2));
                //}
                //else
                //{
                //    gridView8.SetRowCellValue(0, "TerminalOptFee", 0);
                //}

                #endregion

                #region 结算终端分拨费
                //DataRow[] drTerminalAllotFee = CommonClass.dsTerminalAllotFee.Tables[0].Select("ToSite='" + MiddleSite + "'");
                //if (drTerminalAllotFee.Length > 0)
                //{
                //    decimal HeavyPrice = ConvertType.ToDecimal(drTerminalAllotFee[0]["HeavyPrice"]);//重货
                //    decimal LightPrice = ConvertType.ToDecimal(drTerminalAllotFee[0]["LightPrice"]);//轻货
                //    decimal ParcelPriceMin = ConvertType.ToDecimal(drTerminalAllotFee[0]["ParcelPriceMin"]);//最低一票
                //    //decimal acc = Math.Max(Weight * HeavyPrice, Volume * LightPrice);
                //    //acc = Math.Max(acc, ParcelPriceMin);
                //    //gridView8.SetRowCellValue(0, "TerminalAllotFee", acc);
                //    decimal acc = Math.Max(Weight * HeavyPrice, ParcelPriceMin);
                //    gridView8.SetRowCellValue(0, "TerminalAllotFee", Math.Round(acc,2));

                //}
                //else
                //{
                //    gridView8.SetRowCellValue(0, "TerminalAllotFee", 0);
                //}

                #endregion
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }

        private void TransferSite_EditValueChanged(object sender, EventArgs e)
        {
            TransitLines.Text = StartSite.Text + "-" + TransferSite.Text + "-" + ReceivCity.Text + "" + ReceivArea.Text;
            SetFee();
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
            SetFee();
        }

        private void ReceivStreet_Enter(object sender, EventArgs e)
        {
            gcdaozhan.Left = ReceivStreet.Left;
            gcdaozhan.Top = ReceivStreet.Top + ReceivStreet.Height;
            gcdaozhan.Visible = true;
            gcdaozhan.BringToFront();
        }

        private void ReceivStreet_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                gridColumn5.FilterInfo = new ColumnFilterInfo("MiddleStreet LIKE '%" + ReceivStreet.Text.Trim() + "%'");
            }
            catch (Exception)
            {

            }

        }

        private void ReceivStreet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                gcdaozhan.Focus();
            }
        }

        private void ReceivStreet_Leave(object sender, EventArgs e)
        {
            try
            {
                DataTable table = (DataTable)gcdaozhan.DataSource;
                //TransitMode.Properties.Items.Clear();
                if (ReceivStreet.Text.Trim() != "" && !gcdaozhan.Focused)
                {

                    DataRow[] dr = table.Select("MiddleStreet='" + ReceivStreet.Text.Trim() + "'");
                    if (dr.Length == 0)
                    {
                        ReceivStreet.Focus();
                        Tool.ToolTip.ShowTip("请选择正确的街道", ReceivStreet, ToolTipLocation.RightTop);
                        return;
                    }
                    else
                    { 
                    }
                }
                if (!gcdaozhan.Focused)
                {
                    gcdaozhan.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }


        private void ReceivProvince_Enter(object sender, EventArgs e)
        {
            //ReceivProvince.ShowPopup();
            gridControl3.Left = ReceivProvince.Left;
            gridControl3.Top = ReceivProvince.Top + ReceivProvince.Height;
            gridControl3.Visible = true;
            gridControl3.BringToFront();
        }

        private void ReceivCity_Enter(object sender, EventArgs e)
        {
            //ReceivCity.ShowPopup();
            gridControl9.Left = ReceivCity.Left;
            gridControl9.Top = ReceivCity.Top + ReceivCity.Height;
            gridControl9.Visible = true;
            gridControl9.BringToFront();
        }

        private void ReceivArea_Enter(object sender, EventArgs e)
        {
            //ReceivArea.ShowPopup();
            gridControl10.Left = ReceivArea.Left;
            gridControl10.Top = ReceivArea.Top + ReceivArea.Height;
            gridControl10.Visible = true;
            gridControl10.BringToFront();
        }

        private void TransferMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable table = (DataTable)gcdaozhan.DataSource;
                TransitMode.Properties.Items.Clear();
                //TransitMode.Text = "";
                if (ReceivStreet.Text.Trim() != "" && !gcdaozhan.Focused)
                {

                    DataRow[] dr = table.Select("MiddleStreet='" + ReceivStreet.Text.Trim() + "'");
                    if (dr.Length > 0)
                    {
                        string SiteName = dr[0]["SiteName"].ToString();
                        TransferSite.Properties.Items.Clear();
                        if (SiteName != "")
                        {
                            string[] SiteNames = SiteName.Split(',');
                            for (int i = 0; i < SiteNames.Length; i++)
                            {
                                TransferSite.Properties.Items.Add(SiteNames[i]);
                            }
                            TransferSite.Text = SiteNames[0];
                        }
                        else
                        {
                            TransferSite.Text = SiteName == "" ? CommonClass.UserInfo.SiteName : SiteName;
                        }

                        DataRow[] drendsite = CommonClass.dsSite.Tables[0].Select("SiteName='" + StartSite.Text.Trim() + "' and EndSiteRang like '%" + TransferSite.Text.Trim() + "%'"); 
                    }
                }
                SetFee();
            }
            catch (Exception ex)
            {
                //MsgBox.ShowOK(ex.Message);
            }


        }

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

        private void ReceivProvince_TextChanged(object sender, EventArgs e)
        {
            ReceivAddress.Text = ReceivProvince.Text + ReceivCity.Text + ReceivArea.Text + ReceivStreet.Text;
            //DestinationSite.Text = ReceivCity.Text;// ReceivProvince.Text + " " + ReceivCity.Text + " " + ReceivArea.Text;
            TransitLines.Text = StartSite.Text + "-" + TransferSite.Text + "-" + ReceivCity.Text + "-" + ReceivArea.Text;
            TransferMode.Text = "";
            PickGoodsSite.Text = "";
            //TransitMode.Text = "";
            SetPickGoodsSite();
        }

        private void ReceivAddress_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string addr = ReceivProvince.Text + ReceivCity.Text + ReceivArea.Text + ReceivStreet.Text;
                if (ReceivAddress.Text.Length < addr.Length)
                {
                    ReceivAddress.Text = addr;
                }
                string eadd = ReceivAddress.Text.Substring(0, addr.Length);
                if (addr != eadd)
                {
                    ReceivAddress.Text = addr;
                }
            }
            catch (Exception) { }

        }

        private void ReceivProvince_Click(object sender, EventArgs e)
        {

        }

        private void ReceivCity_Click(object sender, EventArgs e)
        {
            //ReceivCity.ShowPopup();
        }

        private void ReceivArea_Click(object sender, EventArgs e)
        {
            //ReceivArea.ShowPopup();
        }

        private void ReceivProvince_EditValueChanged(object sender, EventArgs e)
        {
            ReceivCity.Text = "";
            ReceivArea.Text = "";
            ReceivStreet.Text = "";
            SetCity();
            LoadStreet();
        }

        private void ReceivCity_EditValueChanged(object sender, EventArgs e)
        {
            ReceivArea.Text = "";
            ReceivStreet.Text = "";
            SetArea();
            LoadStreet();
            //DestinationSite.Text = ReceivCity.Text + ReceivArea.Text;
        }

        private void ReceivArea_EditValueChanged(object sender, EventArgs e)
        {
            //CommonClass.SetStreet(ReceivStreet, ReceivProvince.Text.Trim(), ReceivCity.Text.Trim(), ReceivArea.Text.Trim());

            ReceivStreet.Text = "";
            LoadStreet();
            //DestinationSite.Text = ReceivCity.Text + ReceivArea.Text;
        }

        private void LoadStreet()
        {
            try
            {
                ReceivStreet.Text = "";
                DataTable tmp = new DataTable();
                string sql = "MiddleProvince='" + ReceivProvince.Text.Trim() + "' and MiddleCity='" + ReceivCity.Text.Trim() + "' and MiddleArea='" + ReceivArea.Text.Trim() + "'";

                DataTable MiddleSiteTable = CommonClass.dsMiddleSite.Tables[0];
                DataRow[] drsite = MiddleSiteTable.Select(sql);
                if (drsite.Length > 0)
                {
                    tmp = drsite[0].Table.Clone();
                    foreach (DataRow row in drsite)
                    {
                        string sql2 = "Province='" + ReceivProvince.Text.Trim() + "' and City='" + ReceivCity.Text.Trim() + "' and Area='" + ReceivArea.Text.Trim() + "' and " + "Street='" + row["MiddleStreet"] + "'";
                        DataRow[] drprice = CommonClass.dsSendPrice.Tables[0].Select(sql2);
                        if (drprice.Length > 0 && row["type"].ToString().Contains("送"))
                        {
                            foreach (DataRow item in drprice)
                            {
                                row["w0_200"] = item["w0_200"];
                                row["w200_1000"] = item["w200_1000"];
                                row["w1000_3000"] = item["w1000_3000"];
                                row["w3000_5000"] = item["w3000_5000"];
                                row["w5000_100000"] = item["w5000_100000"];
                                row["TransferMode"] = item["TransferMode"];
                                tmp.ImportRow(row);
                            }
                        }
                        else
                        {
                            tmp.ImportRow(row);
                        }
                    }
                }
                gcdaozhan.DataSource = tmp;



                //DataSet ds = new DataSet();
                //ds.Tables.Add(CommonClass.dsMiddleSite.Tables[0]);
                //ds.Tables.Add(CommonClass.dsSendPrice.Tables[0]);
                //DataColumn[] parentcolumn = new DataColumn[]{
                //CommonClass.dsMiddleSite.Tables[0].Columns["MiddleProvince"],
                //CommonClass.dsMiddleSite.Tables[0].Columns["MiddleCity"],
                //CommonClass.dsMiddleSite.Tables[0].Columns["MiddleArea"],
                //CommonClass.dsMiddleSite.Tables[0].Columns["MiddleStreet"]};


                //DataColumn[] childcolumn = new DataColumn[]{
                //CommonClass.dsSendPrice.Tables[0].Columns["Province"],
                //CommonClass.dsSendPrice.Tables[0].Columns["City"],
                //CommonClass.dsSendPrice.Tables[0].Columns["Area"],
                //CommonClass.dsSendPrice.Tables[0].Columns["Street"]};

                //DataRelation Rel = new DataRelation("RelationColumn", parentcolumn, childcolumn);
                //ds.Relations.Add(Rel);

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }

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
                DtPro.Rows.Add(new object[] { list[i] });
            }
            gridControl3.DataSource = DtPro;
        }

        private void ReceivProvince_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue != null)
            {
                gridView3.ClearColumnsFilter();
                gridView3.Columns["MiddleProvince"].FilterInfo = new ColumnFilterInfo("[MiddleProvince] LIKE " + "'%" + e.NewValue.ToString() + "%'");
            }
            else
            {
                gridView3.ClearColumnsFilter();
            }
        }

        private void gridControl3_DoubleClick(object sender, EventArgs e)
        {
            setProvinceText();
        }

        private void setProvinceText()
        {
            int rows = gridView3.FocusedRowHandle;
            if (rows < 0) return;
            DataRow dr = gridView3.GetDataRow(rows);
            ReceivProvince.Text = dr["MiddleProvince"].ToString();
            gridControl3.Visible = false;
            ReceivCity.Focus();
        }

        private void SetCity()
        {
            DataTable DtPro = new DataTable();
            DtPro.Columns.Add("MiddleCity", typeof(string));
            List<string> list = new List<string>();
            DataRow[] CityRow = CommonClass.dsMiddleSite.Tables[0].Select("MiddleProvince='" + ReceivProvince.Text.Trim() + "'");
            for (int i = 0; i < CityRow.Length; i++)
            {
                if (!list.Contains(CityRow[i]["MiddleCity"].ToString()))
                {
                    list.Add(CityRow[i]["MiddleCity"].ToString());
                }
            }
            for (int i = 0; i < list.Count; i++)
            {
                DtPro.Rows.Add(new object[] { list[i] });
            }
            gridControl9.DataSource = DtPro;
        }

        private void SetArea()
        {
            DataTable DtPro = new DataTable();
            DtPro.Columns.Add("MiddleArea", typeof(string));
            List<string> list = new List<string>();
            DataRow[] CityRow = CommonClass.dsMiddleSite.Tables[0].Select("MiddleProvince='" + ReceivProvince.Text.Trim() + "' and MiddleCity='" + ReceivCity.Text.Trim() + "'");
            for (int i = 0; i < CityRow.Length; i++)
            {
                if (!list.Contains(CityRow[i]["MiddleArea"].ToString()))
                {
                    list.Add(CityRow[i]["MiddleArea"].ToString());
                }
            }
            for (int i = 0; i < list.Count; i++)
            {
                DtPro.Rows.Add(new object[] { list[i] });
            }
            gridControl10.DataSource = DtPro;
        }

        private void ReceivCity_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue != null)
            {
                gridView11.ClearColumnsFilter();
                gridView11.Columns["MiddleCity"].FilterInfo = new ColumnFilterInfo("[MiddleCity] LIKE " + "'%" + e.NewValue.ToString() + "%'");
            }
            else
            {
                gridView11.ClearColumnsFilter();
            }
        }

        private void ReceivArea_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue != null)
            {
                gridView12.ClearColumnsFilter();
                gridView12.Columns["MiddleArea"].FilterInfo = new ColumnFilterInfo("[MiddleArea] LIKE " + "'%" + e.NewValue.ToString() + "%'");
            }
            else
            {
                gridView12.ClearColumnsFilter();
            }
        }

        private void gridControl9_DoubleClick(object sender, EventArgs e)
        {
            setCityText();
        }

        private void setCityText()
        {
            int rows = gridView11.FocusedRowHandle;
            if (rows < 0) return;
            DataRow dr = gridView11.GetDataRow(rows);
            ReceivCity.Text = dr["MiddleCity"].ToString();
            gridControl9.Visible = false;
            ReceivArea.Focus();
        }

        private void gridControl10_DoubleClick(object sender, EventArgs e)
        {
            setAreaText();
        }

        private void setAreaText()
        {
            int rows = gridView12.FocusedRowHandle;
            if (rows < 0) return;
            DataRow dr = gridView12.GetDataRow(rows);
            ReceivArea.Text = dr["MiddleArea"].ToString();
            gridControl10.Visible = false;
            ReceivStreet.Focus();
        }

        private void gridControl3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                setProvinceText();
            }
        }

        private void gridControl9_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                setCityText();
            }
        }

        private void gridControl10_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                setAreaText();
            }
        }

        private void ReceivProvince_Leave(object sender, EventArgs e)
        {
            try
            {
                if (ReceivProvince.Text.Trim() != "" && !gridControl3.Focused)
                {
                    DataTable table = (DataTable)gridControl3.DataSource;
                    DataRow[] dr = table.Select("MiddleProvince='" + ReceivProvince.Text.Trim() + "'");
                    if (dr.Length == 0)
                    {
                        ReceivProvince.Focus();
                        Tool.ToolTip.ShowTip("请选择正确省份", ReceivProvince, ToolTipLocation.RightTop);
                        return;
                    }
                }

                if (gridView3.RowCount == 1)
                {
                    ReceivProvince.Text = gridView3.GetRowCellValue(0, "MiddleProvince").ToString();
                }

                if (!gridControl3.Focused)
                {
                    gridControl3.Visible = false;
                }
            }
            catch (Exception)
            { }
        }

        private void ReceivCity_Leave(object sender, EventArgs e)
        {
            try
            {
                if (ReceivCity.Text.Trim() != "" && !gridControl9.Focused)
                {
                    DataTable table = (DataTable)gridControl9.DataSource;
                    DataRow[] dr = table.Select("MiddleCity='" + ReceivCity.Text.Trim() + "'");
                    if (dr.Length == 0)
                    {
                        ReceivCity.Focus();
                        Tool.ToolTip.ShowTip("输入错误", ReceivCity, ToolTipLocation.RightTop);
                        return;
                    }
                }

                if (gridView11.RowCount == 1)
                {
                    ReceivCity.Text = gridView11.GetRowCellValue(0, "MiddleCity").ToString();
                }
                if (!gridControl9.Focused)
                {
                    gridControl9.Visible = false;
                }
            }
            catch (Exception)
            { }
        }

        private void ReceivArea_Leave(object sender, EventArgs e)
        {
            try
            {
                if (ReceivArea.Text.Trim() != "" && !gridControl10.Focused)
                {
                    DataTable table = (DataTable)gridControl10.DataSource;
                    DataRow[] dr = table.Select("MiddleArea='" + ReceivArea.Text.Trim() + "'");
                    if (dr.Length == 0)
                    {
                        ReceivArea.Focus();
                        Tool.ToolTip.ShowTip("输入错误", ReceivArea, ToolTipLocation.RightTop);
                        return;
                    }
                }

                if (gridView12.RowCount == 1)
                {
                    ReceivArea.Text = gridView12.GetRowCellValue(0, "MiddleArea").ToString();
                }
                if (!gridControl10.Focused)
                {
                    gridControl10.Visible = false;
                }
            }
            catch (Exception)
            { }
        }

        private void ReceivProvince_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            { gridControl3.Focus(); }
            if (e.KeyCode == Keys.Escape)
            {
                gridControl3.Visible = false;
            }
        }

        private void ReceivCity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            { gridControl9.Focus(); }
            if (e.KeyCode == Keys.Escape)
            {
                gridControl9.Visible = false;
            }
        }

        private void ReceivArea_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            { gridControl10.Focus(); }
            if (e.KeyCode == Keys.Escape)
            {
                gridControl10.Visible = false;
            }
        }

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

        private void gridControl2_Click(object sender, EventArgs e)
        {

        }

        private void BillNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ReceivProvince.Focus();
            }
        }

        private void TransferMode_TextChanged(object sender, EventArgs e)
        {
            SetPickGoodsSite();
        }

        private void SetPickGoodsSite()
        {
            try
            {
                if (TransferMode.Text.Trim() == "") return;
                string type = "";
                if (TransferMode.Text.Trim() == "自提")
                {
                    type = "自提";
                }
                else
                {
                    type = "送";
                }
                //DataRelation dr = new DataRelation(）
                PickGoodsSite.Properties.Items.Clear();
                DataRow[] dr = ((DataTable)gcdaozhan.DataSource).Select("MiddleStreet='" + ReceivStreet.Text + "' and type like '%" + type + "%'");
                for (int i = 0; i < dr.Length; i++)
                {
                    PickGoodsSite.Properties.Items.Add(dr[i]["WebName"]);
                }
                if (PickGoodsSite.Properties.Items.Count > 0)
                {
                    PickGoodsSite.SelectedIndex = 0;
                }

                if (TransferMode.Text.Trim() == "网点送货")
                {
                    DataRow[] drCenter = CommonClass.dsDirectSendFee.Tables[0].Select("SiteName='" + TransferSite.Text + "'");
                    for (int i = 0; i < drCenter.Length; i++)
                    {
                        if (!PickGoodsSite.Properties.Items.Contains(drCenter[i]["CenterName"]))
                        {
                            PickGoodsSite.Properties.Items.Add(drCenter[i]["CenterName"]);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                //MsgBox.ShowOK(ex.Message);
            }
        }

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
                if (dsBill.Tables[0].Rows.Count > 0)
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
            list.Add(new SqlPara("TransitLines", TransitLines.Text.Trim()));
            list.Add(new SqlPara("TransitMode", TransitMode.Text.Trim()));
            list.Add(new SqlPara("CusOderNo", CusOderNo.Text.Trim()));

            list.Add(new SqlPara("ConsigneeCellPhone", NoticeState.Checked ? "888888" : ConsigneeCellPhone.Text.Trim()));
            list.Add(new SqlPara("ConsigneePhone", NoticeState.Checked ? "888888" : ConsigneePhone.Text.Trim()));
            list.Add(new SqlPara("ConsigneeName", NoticeState.Checked ? "888888" : ConsigneeName.Text.Trim()));
            list.Add(new SqlPara("ConsigneeCompany", NoticeState.Checked ? "888888" : ConsigneeCompany.Text.Trim()));

            list.Add(new SqlPara("PickGoodsSite", PickGoodsSite.Text.Trim()));
            list.Add(new SqlPara("ReceivProvince", ReceivProvince.Text.Trim()));
            list.Add(new SqlPara("ReceivCity", ReceivCity.Text.Trim()));
            list.Add(new SqlPara("ReceivArea", ReceivArea.Text.Trim()));
            list.Add(new SqlPara("ReceivStreet", ReceivStreet.Text.Trim()));
            list.Add(new SqlPara("ReceivAddress", ReceivAddress.Text.Trim()));
            list.Add(new SqlPara("ConsignorCellPhone", ConsignorCellPhone.Text.Trim()));
            list.Add(new SqlPara("ConsignorPhone", ConsignorPhone.Text.Trim()));
            list.Add(new SqlPara("ConsignorName", ConsignorName.Text.Trim()));
            list.Add(new SqlPara("ConsignorCompany", ConsignorCompany.Text.Trim()));
            list.Add(new SqlPara("ReceivMode", ReceivMode.Text.Trim()));
            list.Add(new SqlPara("CusNo", CusNo.Text.Trim()));
            list.Add(new SqlPara("CusType", CusType.Text.Trim()));
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

            list.Add(new SqlPara("OtherFee", OtherFee));
            list.Add(new SqlPara("IsInvoice", IsInvoice.Checked ? 1 : 0));
            list.Add(new SqlPara("ReceiptFee", ReceiptFee));
            list.Add(new SqlPara("ReceiptCondition", ReceiptCondition.Text.Trim()));
            list.Add(new SqlPara("FreightAmount", 0));
            list.Add(new SqlPara("ActualFreight", ActualFreight));
            list.Add(new SqlPara("CouponsNo", CouponsNo.Text.Trim()));
            list.Add(new SqlPara("CouponsAmount", CouponsAmount.Text.Trim() == "" ? "0" : CouponsAmount.Text.Trim()));
            list.Add(new SqlPara("PaymentMode", PaymentMode.Text.Trim()));
            list.Add(new SqlPara("PaymentAmout", PaymentAmout.Text.Trim() == "" ? "0" : PaymentAmout.Text.Trim()));
            list.Add(new SqlPara("PayMode", PayMode.Text.Trim()));
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

            list.Add(new SqlPara("MainlineFee", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "MainlineFee"))));
            list.Add(new SqlPara("DeliveryFee", ConvertType.ToDecimal(gridView1.GetRowCellValue(0, "DeliveryFee"))));
            list.Add(new SqlPara("TransferFee", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "TransferFee"))));
            list.Add(new SqlPara("DepartureOptFee", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "DepartureOptFee"))));
            list.Add(new SqlPara("TerminalOptFee", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "TerminalOptFee"))));
            list.Add(new SqlPara("TerminalAllotFee", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "TerminalAllotFee"))));


            list.Add(new SqlPara("ReceiptFee_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "ReceiptFee_C"))));
            list.Add(new SqlPara("NoticeFee_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "NoticeFee_C"))));

            list.Add(new SqlPara("SupportValue_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "SupportValue_C"))));
            list.Add(new SqlPara("AgentFee_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "AgentFee_C"))));
            list.Add(new SqlPara("PackagFee_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "PackagFee_C"))));
            list.Add(new SqlPara("OtherFee_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "OtherFee_C"))));
            list.Add(new SqlPara("HandleFee_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "HandleFee_C"))));

            list.Add(new SqlPara("StorageFee_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "StorageFee_C"))));
            list.Add(new SqlPara("WarehouseFee_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "WarehouseFee_C"))));
            list.Add(new SqlPara("ForkliftFee_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "ForkliftFee_C"))));
            list.Add(new SqlPara("Tax_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "Tax_C"))));
            list.Add(new SqlPara("ChangeFee_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "ChangeFee_C"))));

            list.Add(new SqlPara("UpstairFee_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "UpstairFee_C"))));
            list.Add(new SqlPara("CustomsFee_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "CustomsFee_C"))));
            list.Add(new SqlPara("FrameFee_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "FrameFee_C"))));
            list.Add(new SqlPara("Expense_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "Expense_C"))));
            list.Add(new SqlPara("FuelFee_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "FuelFee_C"))));
            list.Add(new SqlPara("InformationFee_C", ConvertType.ToDecimal(gridView8.GetRowCellValue(0, "InformationFee_C"))));

            list.Add(new SqlPara("ConsigneeCellPhone_K", ConsigneeCellPhone.Text.Trim()));
            list.Add(new SqlPara("ConsigneePhone_K", ConsigneePhone.Text.Trim()));
            list.Add(new SqlPara("ConsigneeName_K", ConsigneeName.Text.Trim()));
            list.Add(new SqlPara("ConsigneeCompany_K", ConsigneeCompany.Text.Trim()));

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
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillMan", CommonClass.UserInfo.UserName));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_Draft", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                gridControl5.DataSource = ds.Tables[0];
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
                ConsigneeCompany.EditValue = ConvertType.ToInt32(dr["NoticeState"]) == 0 ? dr["ConsigneeCompany"] : dr["ConsigneeCompany_K"];

                PickGoodsSite.EditValue = dr["PickGoodsSite"];
                ReceivProvince.EditValue = dr["ReceivProvince"];
                ReceivCity.EditValue = dr["ReceivCity"];
                ReceivArea.EditValue = dr["ReceivArea"];
                ReceivStreet.EditValue = dr["ReceivStreet"];
                ReceivAddress.EditValue = dr["ReceivAddress"];
                ConsignorCellPhone.EditValue = dr["ConsignorCellPhone"];
                ConsignorPhone.EditValue = dr["ConsignorPhone"];
                ConsignorName.EditValue = dr["ConsignorName"];
                ConsignorCompany.EditValue = dr["ConsignorCompany"];
                ReceivMode.EditValue = dr["ReceivMode"];
                CusNo.EditValue = dr["CusNo"];
                CusType.EditValue = dr["CusType"];
                ValuationType.EditValue = dr["ValuationType"];
                ReceivOrderNo.EditValue = dr["ReceivOrderNo"];
                Salesman.EditValue = dr["Salesman"];

                BillNo.EditValue = dr["BillNo"];
                VehicleNo.EditValue = dr["VehicleNo"];
                StartSite.EditValue = dr["StartSite"];
                TransferMode.EditValue = dr["TransferMode"];
                DestinationSite.EditValue = dr["DestinationSite"];
                TransferSite.EditValue = dr["TransferSite"];
                TransitLines.EditValue = dr["TransitLines"];
                TransitMode.EditValue = dr["TransitMode"];
                CusOderNo.EditValue = dr["CusOderNo"];

                AlienGoods.Checked = Convert.ToInt32(dr["AlienGoods"].ToString() == "" ? "0" : dr["AlienGoods"].ToString()) > 0;
                GoodsVoucher.Checked = Convert.ToInt32(dr["GoodsVoucher"].ToString() == "" ? "0" : dr["GoodsVoucher"].ToString()) > 0;
                PreciousGoods.Checked = Convert.ToInt32(dr["GoodsVoucher"].ToString() == "" ? "0" : dr["GoodsVoucher"].ToString()) > 0;
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
                CouponsNo.EditValue = dr["CouponsNo"];
                CouponsAmount.EditValue = dr["CouponsAmount"];
                PaymentMode.EditValue = dr["PaymentMode"];
                PaymentAmout.EditValue = dr["PaymentAmout"];
                PayMode.EditValue = dr["PayMode"];
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

                gridView8.SetRowCellValue(0, "MainlineFee", dr["MainlineFee"]);
                gridView1.SetRowCellValue(0, "DeliveryFee", dr["DeliveryFee"]);
                gridView8.SetRowCellValue(0, "TransferFee", dr["TransferFee"]);
                gridView8.SetRowCellValue(0, "DepartureOptFee", dr["DepartureOptFee"]);
                gridView8.SetRowCellValue(0, "TerminalOptFee", dr["TerminalOptFee"]);
                gridView8.SetRowCellValue(0, "TerminalAllotFee", dr["TerminalAllotFee"]);

                gridView8.SetRowCellValue(0, "ReceiptFee_C", dr["ReceiptFee_C"]);
                gridView8.SetRowCellValue(0, "NoticeFee_C", dr["NoticeFee_C"]);
                gridView8.SetRowCellValue(0, "SupportValue_C", dr["SupportValue_C"]);

                gridView8.SetRowCellValue(0, "AgentFee_C", dr["AgentFee_C"]);
                gridView8.SetRowCellValue(0, "PackagFee_C", dr["PackagFee_C"]);
                gridView8.SetRowCellValue(0, "OtherFee_C", dr["OtherFee_C"]);

                gridView8.SetRowCellValue(0, "HandleFee_C", dr["HandleFee_C"]);
                gridView8.SetRowCellValue(0, "StorageFee_C", dr["StorageFee_C"]);
                gridView8.SetRowCellValue(0, "WarehouseFee_C", dr["WarehouseFee_C"]);

                gridView8.SetRowCellValue(0, "ForkliftFee_C", dr["ForkliftFee_C"]);
                gridView8.SetRowCellValue(0, "Tax_C", dr["Tax_C"]);
                gridView8.SetRowCellValue(0, "ChangeFee_C", dr["ChangeFee_C"]);

                gridView8.SetRowCellValue(0, "UpstairFee_C", dr["UpstairFee_C"]);
                gridView8.SetRowCellValue(0, "CustomsFee_C", dr["CustomsFee_C"]);
                gridView8.SetRowCellValue(0, "FrameFee_C", dr["FrameFee_C"]);

                gridView8.SetRowCellValue(0, "Expense_C", dr["Expense_C"]);
                gridView8.SetRowCellValue(0, "FuelFee_C", dr["FuelFee_C"]);
                gridView8.SetRowCellValue(0, "InformationFee_C", dr["InformationFee_C"]);
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
        }

        private void TransitMode_Enter(object sender, EventArgs e)
        {
            TransitMode.ShowPopup();
        }

        private void PickGoodsSite_EditValueChanged(object sender, EventArgs e)
        {
            //TransitMode.Text = "";
        }

        private void CusOderNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                gridControl2.Focus();
                gridView2.FocusedRowHandle = 0;
                gridView2.FocusedColumn = gridView2.Columns["Varieties"];
                gridView2.ShowEditor();
            }
        }

        private void gcdaozhan_Leave(object sender, EventArgs e)
        {
            if (ReceivStreet.Text.Trim() != "")
            {
                DataTable table = (DataTable)gcdaozhan.DataSource;
                DataRow[] dr = table.Select("MiddleStreet='" + ReceivStreet.Text.Trim() + "'");
                if (dr.Length == 0)
                {
                    ReceivStreet.Focus();
                    Tool.ToolTip.ShowTip("请选择正确的街道", ReceivStreet, ToolTipLocation.RightTop);
                    return;
                }
            }
        }

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

        private void gridControl3_Leave(object sender, EventArgs e)
        {
            if (ReceivProvince.Text.Trim() != "")
            {
                DataTable table = (DataTable)gridControl3.DataSource;
                DataRow[] dr = table.Select("MiddleProvince='" + ReceivProvince.Text.Trim() + "'");
                if (dr.Length == 0)
                {
                    ReceivProvince.Focus();
                    Tool.ToolTip.ShowTip("请选择正确省份", ReceivProvince, ToolTipLocation.RightTop);
                    return;
                }
            }
        }

        private void gridControl9_Leave(object sender, EventArgs e)
        {
            if (ReceivCity.Text.Trim() != "")
            {
                DataTable table = (DataTable)gridControl9.DataSource;
                DataRow[] dr = table.Select("MiddleCity='" + ReceivCity.Text.Trim() + "'");
                if (dr.Length == 0)
                {
                    ReceivCity.Focus();
                    Tool.ToolTip.ShowTip("输入错误", ReceivCity, ToolTipLocation.RightTop);
                    return;
                }
            }
        }

        private void gridControl10_Leave(object sender, EventArgs e)
        {
            if (ReceivArea.Text.Trim() != "")
            {
                DataTable table = (DataTable)gridControl10.DataSource;
                DataRow[] dr = table.Select("MiddleArea='" + ReceivArea.Text.Trim() + "'");
                if (dr.Length == 0)
                {
                    ReceivArea.Focus();
                    Tool.ToolTip.ShowTip("输入错误", ReceivArea, ToolTipLocation.RightTop);
                    return;
                }
            }
        }

        private void ReceivAddress_Enter(object sender, EventArgs e)
        {
            ReceivAddress.Select(ReceivAddress.Text.Length, 0);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            OperTime.Text = (ConvertType.ToInt32(OperTime.Text) + 1).ToString();
        }

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
            SetFee();
        }

        private void frmWayBillAdd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                fmDeliveryFee fm = new fmDeliveryFee();
                fm.ShowDialog();
            }
            else if (e.KeyCode == Keys.F4)
            {
                fmMainlineFee fm = new fmMainlineFee();
                fm.ShowDialog();
            }
            else if (e.KeyCode == Keys.F5)
            {
                fmTransferFee fm = new fmTransferFee();
                fm.ShowDialog();
            }
            else if (e.KeyCode == Keys.F6)
            {
                fmSurchargeFee fm = new fmSurchargeFee();
                fm.ShowDialog();
            }
        }

        private void ReceivMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ReceivMode.Text.Trim() != "客户自送")
            {
                ReceivOrderNo.Enabled = true;
            }
            else
            {
                ReceivOrderNo.Enabled = false;
                ReceivOrderNo.Text = "";
            }
        }

        private void ReceivMode_Enter(object sender, EventArgs e)
        {
            ReceivMode.ShowPopup();
        }

        private void ReceivOrderNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            { gcjiehuodanhao.Focus(); }
            if (e.KeyCode == Keys.Escape)
            { gcjiehuodanhao.Visible = false; }
        }

        private void BillNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = StringHelper.NumChange(e.KeyChar);
        }
    }
}