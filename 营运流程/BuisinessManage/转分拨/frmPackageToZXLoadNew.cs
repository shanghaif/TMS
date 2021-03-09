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
    public partial class frmPackageToZXLoadNew : BaseForm
    {
        private DataSet dataset1 = new DataSet();
        private DataSet dataset3 = new DataSet(), dsZXSite;
        GridHitInfo hitInfo = null;
        static frmPackageToZXLoadNew fsl;

        /// <summary>
        /// 获取窗体对像
        /// </summary>
        public static frmPackageToZXLoadNew Get_frmPackageToZXLoadNew { get { if (fsl == null || fsl.IsDisposed) fsl = new frmPackageToZXLoadNew(); return fsl; } }

        public frmPackageToZXLoadNew()
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
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_DEPARTURE_FB_LOAD_485");
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

        private void w_send_load_Load(object sender, EventArgs e)
        {
            if (!CommonClass.FormSet(this, false, true)) return;
            CommonClass.GetGridViewColumns(myGridView1, false, myGridView2, myGridView3);
            GridOper.SetGridViewProperty(myGridView1, myGridView2, myGridView3);
            BarMagagerOper.SetBarPropertity(bar1, bar2); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1, myGridView2);

            SendDate.DateTime = CommonClass.gcdate;
            //车辆信息
            if (CommonClass.dsCar != null && CommonClass.dsCar.Tables.Count > 0) myGridControl3.DataSource = CommonClass.dsCar.Tables[0];

            dsZXSite = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_ALL_COMPANY_TX"));
            string tmp = "";
            if (dsZXSite != null && dsZXSite.Tables.Count > 0 && dsZXSite.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsZXSite.Tables[0].Rows)
                {
                    tmp = ConvertType.ToString(dr["companyid"]) + "|" + ConvertType.ToString(dr["gsjc"]);
                    if (tmp != "" && !AcceptCompanyId.Properties.Items.Contains(tmp))
                        AcceptCompanyId.Properties.Items.Add(tmp);
                }
            }
            AcceptCompanyId.Properties.Items.Add("100|ZQTMS");
            txtBatch.Text = GetMaxInOneVehicleFlag(CommonClass.UserInfo.SiteName);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (myGridView2.RowCount == 0)
            {
                MsgBox.ShowOK("请选择要分拨的清单!");
                xtraTabControl1.SelectedTabPage = tp1;
                return;
            }

            string companyid = AcceptCompanyId.Text.Split('|')[0];
            if (companyid == "")
            {
                MsgBox.ShowOK("请选择接收的公司!");
                AcceptCompanyId.Focus();
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
            string sfczfs = "";
            string MainlineFee_rStr = "";//分拨干线费

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
                MainlineFee_rStr += GridOper.GetRowCellValueString(myGridView2, i, "MainlineFee") + "@";
                companyids += GridOper.GetRowCellValueString(myGridView2, i, "companyid") + "@";
                sfczfs += 0 + "@";



                if (Convert.ToDecimal(GridOper.GetRowCellValueString(myGridView2, i, "TerminalOptFee")) <= 0)
                {
                    billNoMsg += GridOper.GetRowCellValueString(myGridView2, i, "BillNo") + ",";
                }
            }
            if (BillNoStr == "") return;

            if (MsgBox.ShowYesNo("确定分拨？") != DialogResult.Yes) return;
            //回单库存2018.5.10  zaj
            string BillNO = "";
            string ReceiptCondition = "";
            string HDBillno = "";
            string bb = "";
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                string tiaojian = myGridView2.GetRowCellValue(i, "ReceiptCondition").ToString();
                //if (tiaojian == "签回单")
                if (tiaojian != "")
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
                    MsgBox.ShowError("有运单改单申请还未执行，请执行后再转分拨！单号如下：\n" + sb.ToString());
                    return;
                }
                #region
                //string DepartureOptFee = "";//结算始发操作费
                //string TerminalOptFee = "";//结算终端操作费
                //string MainlineFee = "";//结算干线费
                //string DeliveryFee = "";//结算送货费
                //string TransferFee = "";//结算中转费
                //string DepartureAllotFee = "";//结算始发分拨费

                //#region 转分拨结算费用重算
                //string StartSite = "";//始发站
                //string TransitMode = "";//运输方式
                //string TransferSite = "";//中转地
                //decimal FeeWeight = 0;//计费重量
                //decimal FeeVolume = 0;//计费体积
                //int AlienGoods = 0;//异性货物
                //string ReceivProvince = "";//收货省
                //string ReceivCity = "";//收货市
                //string ReceivArea = "";//收货区
                //string ReceivStreet = "";//收货街道
                //string TransferMode = "";//交接方式
                //string Package = "";//包装
                //string BegWeb = "";//开单网点
                //decimal OperationWeight=0;

                //for (int i = 0; i < myGridView2.RowCount; i++)
                //{
                //    StartSite = myGridView2.GetRowCellValue(i, "StartSite").ToString();
                //    TransitMode = myGridView2.GetRowCellValue(i, "TransitMode").ToString();
                //    TransferSite = myGridView2.GetRowCellValue(i, "TransferSite").ToString();
                //    FeeWeight = Convert.ToDecimal( myGridView2.GetRowCellValue(i, "FeeWeight").ToString());
                //    FeeVolume =Convert.ToDecimal( myGridView2.GetRowCellValue(i, "FeeVolume").ToString());
                //    AlienGoods = Convert.ToInt32(myGridView2.GetRowCellValue(i, "AlienGoods").ToString());
                //    ReceivProvince = myGridView2.GetRowCellValue(i, "ReceivProvince").ToString();
                //    ReceivCity = myGridView2.GetRowCellValue(i, "ReceivCity").ToString();
                //    ReceivArea = myGridView2.GetRowCellValue(i, "ReceivArea").ToString();
                //    ReceivStreet = myGridView2.GetRowCellValue(i, "ReceivStreet").ToString();
                //    TransferMode = myGridView2.GetRowCellValue(i, "TransferMode").ToString();
                //    Package = myGridView2.GetRowCellValue(i, "Package").ToString();
                //    BegWeb = myGridView2.GetRowCellValue(i, "BegWeb").ToString();
                //    OperationWeight = Convert.ToDecimal(myGridView2.GetRowCellValue(i, "OperationWeight").ToString());


                //    //结算始发操作费
                //    decimal DepartureOptFee1 = Convert.ToDecimal( myGridView2.GetRowCellValue(i, "DepartureOptFee").ToString());
                //    DataRow[] drDepartureOptFee = CommonClass.dsDepartureOptFee_PT.Tables[0].Select("FromSite='" + StartSite + "'and TransitMode='" + TransitMode + "'" + "and companyid='" + companyid + "'");
                //    if (drDepartureOptFee.Length > 0)
                //    {
                //        decimal HeavyPrice = ConvertType.ToDecimal(drDepartureOptFee[0]["HeavyPrice"]);//重货
                //        decimal LightPrice = ConvertType.ToDecimal(drDepartureOptFee[0]["LightPrice"]);//轻货
                //        decimal ParcelPriceMin = ConvertType.ToDecimal(drDepartureOptFee[0]["ParcelPriceMin"]);//最低一票
                //        decimal acc = OperationWeight * HeavyPrice;
                //        acc = Math.Max(acc, ParcelPriceMin);
                //        DepartureOptFee += acc+"@";
                //    }
                //    else
                //    {
                //        DepartureOptFee += DepartureOptFee1+"@";
                //    }

                //    //结算终端操作费
                //    decimal TerminalOptFee1 =Convert.ToDecimal( myGridView2.GetRowCellValue(i, "TerminalOptFee").ToString());
                //    DataRow[] drTerminalOptFee = CommonClass.dsTerminalOptFee_PT.Tables[0].Select("TransferSite='" + TransferSite + "'and TransitMode='" + TransitMode + "'" + "and companyid='" + companyid + "'");
                //    if (drTerminalOptFee.Length > 0)
                //    {
                //        decimal HeavyPrice = ConvertType.ToDecimal(drTerminalOptFee[0]["HeavyPrice"]);//重货
                //        decimal LightPrice = ConvertType.ToDecimal(drTerminalOptFee[0]["LightPrice"]);//轻货
                //        decimal ParcelPriceMin = ConvertType.ToDecimal(drTerminalOptFee[0]["ParcelPriceMin"]);//最低一票
                //        decimal acc = OperationWeight * HeavyPrice;
                //        acc = Math.Max(acc, ParcelPriceMin);
                //        TerminalOptFee += acc+"@";

                //    }
                //    else
                //    {
                //        TerminalOptFee += TerminalOptFee1+"@";
                //    }

                //    //结算干线费 
                //    decimal MainlineFee1 = Convert.ToDecimal( myGridView2.GetRowCellValue(i, "MainlineFee").ToString());
                //    DataRow[] drMainlineFee = CommonClass.dsMainlineFeePT.Tables[0].Select("FromSite='" + StartSite + "' and TransferSite='" + TransferSite + "'and TransportMode='" + TransitMode + "'" + "and companyid='" + companyid + "'");
                //    if (drMainlineFee.Length > 0)
                //    {
                //        decimal ParcelPriceMin = ConvertType.ToDecimal(drMainlineFee[0]["ParcelPriceMin"]);//最低一票
                //        decimal HeavyPrice = ConvertType.ToDecimal(drMainlineFee[0]["HeavyPrice"]);//重货
                //        decimal LightPrice = ConvertType.ToDecimal(drMainlineFee[0]["LightPrice"]);//轻货
                //        decimal fee = 0;

                //        fee = Math.Max(FeeWeight * HeavyPrice, FeeVolume * LightPrice);
                //        if (AlienGoods == 1)
                //        {
                //            fee = fee * (decimal)1.5;
                //        }

                //        fee = Math.Max(fee, ParcelPriceMin);
                //        MainlineFee += fee+"@";

                //    }
                //    else
                //    {
                //        MainlineFee += MainlineFee1 + "@";
                //    }

                //    //结算送货费
                //    decimal DeliveryFee1 =Convert.ToDecimal( myGridView2.GetRowCellValue(i, "DeliveryFee").ToString());
                //    decimal DeliveryFee2 = 0;
                //    if (ReceivProvince == "香港")
                //    {
                //        DeliveryFee += DeliveryFee1 + "@";
                //    }
                //    else
                //    {

                //            string sql = "";
                //            sql = "Province='" + ReceivProvince
                //                    + "' and City='" + ReceivCity
                //                    + "' and Area='" + ReceivArea
                //              + "' and Street='" + ReceivStreet + "'"+ "and companyid='" + companyid + "'";

                //            DataRow[] drDeliveryFee = CommonClass.dsSendPrice_PT.Tables[0].Select(sql);
                //            if (drDeliveryFee != null && drDeliveryFee.Length > 0)
                //            {
                //                if (TransferMode == "送货" || TransferMode == "司机直送")
                //                {
                //                    DeliveryFee2 = getDeliveryFee1(drDeliveryFee, OperationWeight);
                //                    if (AlienGoods == 1)
                //                    {
                //                        DeliveryFee2 = DeliveryFee2 * (decimal)2;
                //                    }
                //                }
                //                else if (TransferMode == "自提")
                //                {
                //                    DeliveryFee2 = 0;
                //                }
                //                else if (TransferMode == "专车直送")
                //                {
                //                    DeliveryFee2 = 0;
                //                }
                //                DeliveryFee += DeliveryFee2 + "@";

                //            }
                //            else
                //            {
                //                DeliveryFee += DeliveryFee1 + "@";
                //            }

                //    }


                //    //结算中转费
                //    decimal TransferFee1 = Convert.ToDecimal(myGridView2.GetRowCellValue(i, "TransferFee").ToString());
                //    decimal TransferFee2 = 0;

                //        DataRow[] drTransferFee = CommonClass.dsTransferFee_PT.Tables[0].Select("FromSite like '%" + StartSite + "%' and TransferSite='" + TransferSite + "' and ToProvince='"
                //                + ReceivProvince + "' and ToCity='" + ReceivCity + "' and ToArea='" + ReceivArea + "'and TransitMode='" + TransitMode + "'"+ "and companyid='" + companyid + "'");
                //        if (drTransferFee.Length > 0)
                //        {
                //            if (TransferMode != "司机直送")
                //            {
                //                decimal HeavyPrice = ConvertType.ToDecimal(drTransferFee[0]["HeavyPrice"]);//重货
                //                decimal LightPrice = ConvertType.ToDecimal(drTransferFee[0]["LightPrice"]);//轻货
                //                decimal ParcelPriceMin = ConvertType.ToDecimal(drTransferFee[0]["ParcelPriceMin"]);//最低一票
                //                TransferFee2 = Math.Max(FeeWeight * HeavyPrice, FeeVolume * LightPrice);
                //                if (AlienGoods == 1)
                //                {
                //                    TransferFee2 = TransferFee2 * (decimal)1.5;
                //                }
                //                TransferFee2 = Math.Max(TransferFee2, ParcelPriceMin);
                //            }
                //            else
                //            {
                //                TransferFee2 = 0;
                //            }
                //            TransferFee += TransferFee2 + "@";

                //        }
                //        else
                //        {
                //            TransferFee += TransferFee1 + "@";
                //        }


                //    //结算始发分拨费
                //        decimal DepartureAllotFee1 = Convert.ToDecimal(myGridView2.GetRowCellValue(i, "DepartureAllotFee").ToString());
                //        decimal DepartureAllotFee2 = 0;
                //        DataRow[] drTerminalAllotFee = CommonClass.dsDepartureAllotFee_PT.Tables[0].Select("BegWeb='"
                //        + BegWeb + "' and TransferSite='" + TransferSite + "' and FromSite='" + StartSite + "'");

                //        if (drTerminalAllotFee != null && drTerminalAllotFee.Length > 0)
                //        {
                //            decimal HeavyPrice = ConvertType.ToDecimal(drTerminalAllotFee[0]["HeavyPrice"]);//重货
                //            decimal LightPrice = ConvertType.ToDecimal(drTerminalAllotFee[0]["LightPrice"]);//轻货
                //            decimal ParcelPriceMin = ConvertType.ToDecimal(drTerminalAllotFee[0]["ParcelPriceMin"]);//最低一票
                //            DepartureAllotFee2 = Math.Max(FeeWeight * HeavyPrice, FeeVolume * LightPrice);
                //            DepartureAllotFee2 = Math.Max(DepartureAllotFee2, ParcelPriceMin);
                //            DepartureAllotFee += DepartureAllotFee2 + "@";

                //        }
                //        else
                //        {
                //            DepartureAllotFee += DepartureAllotFee1 + "@";
                //        }
                //}






                //#endregion
                #endregion






                string batch = GetMaxInOneVehicleFlag(CommonClass.UserInfo.SiteName);
                List<SqlPara> list1 = new List<SqlPara>();
                list1.Add(new SqlPara("BillNoStr", BillNoStr));
                list1.Add(new SqlPara("AcceptSiteName", site));
                list1.Add(new SqlPara("AcceptWebName", webName));
                list1.Add(new SqlPara("CarNo", carNo));
                list1.Add(new SqlPara("DriverName", driverName));
                list1.Add(new SqlPara("DriverPhone", driverPhone));
                list1.Add(new SqlPara("Remark", Remark.Text.Trim()));
                list1.Add(new SqlPara("Batch", batch));

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
                list1.Add(new SqlPara("MainlineFee_rStr", MainlineFee_rStr));//转分拨干线费
                list1.Add(new SqlPara("AcceptCompanyId", companyid));//接收公司ID


                if (companyid == "100")
                {
                    string db = CommonClass.UserInfo.UserDB.ToString();
                    Dictionary<string, string> dty = new Dictionary<string, string>();
                    dty.Add("BillNoStr", BillNoStr);
                    dty.Add("AcceptCompanyId", "100");
                    dty.Add("AcceptSiteName", site);
                    dty.Add("AcceptWebName", webName);
                    dty.Add("CarNo", carNo);
                    dty.Add("DriverName", driverPhone);
                    dty.Add("DriverPhone", driverPhone);
                    dty.Add("Remark", Remark.Text.Trim());
                    dty.Add("AreaName", CommonClass.UserInfo.AreaName);
                    dty.Add("CauseName", CommonClass.UserInfo.CauseName);
                    dty.Add("DepartName", CommonClass.UserInfo.DepartName);
                    dty.Add("SiteName", CommonClass.UserInfo.SiteName);
                    dty.Add("WebName", CommonClass.UserInfo.WebName);
                    dty.Add("UserAccount", CommonClass.UserInfo.UserAccount);
                    dty.Add("UserName", CommonClass.UserInfo.UserName);
                    dty.Add("Batch", batch);
                    dty.Add("HDBillno", HDBillno);
                    dty.Add("companyid", CommonClass.UserInfo.companyid);
                    dty.Add("sfczf", sfczfs);  //始发操作费
                    dty.Add("zdczf", TerminalOptFeeStr); //终端操作费
                    dty.Add("mainlineFee", MainlineFee_rStr);  //结算干线费
                    dty.Add("transferFee", TransferFeeStr);  //结算中转费
                    dty.Add("deliveryFee", DeliveryFeeStr);  //结算送货费

                    RequestModel<string> request1 = new RequestModel<string>();
                    request1.Request = JsonConvert.SerializeObject(dty);
                    request1.OperType = 0;


                    if (companyid != "309" && companyid != "488" && companyid != "490" && companyid != "132" && companyid != "485" && companyid != "484")
                    {
                        ResponseModelClone<string> result = HttpHelper.HttpPost(JsonConvert.SerializeObject(request1), "http://lms.dekuncn.com:7016/CenterSynService/AllocateToBranch");//正式库
                        //ResponseModelClone<string> result = HttpHelper.HttpPost(JsonConvert.SerializeObject(request1), "http://192.168.16.236:8080/CenterSynService/AllocateToBranch");//测试
                        if (result.State == "200")
                        {
                            try
                            {
                                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_PACKAGE_FB_TO_ZX_485", list1)) == 0)//USP_ADD_PACKAGE_FB_TO_ZX_TX
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
                                MsgBox.ShowOK("分拨失败：" + ex.Message);
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
                            listLog.Add(new SqlPara("ExceptMessage", result.Message));
                            SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                            SqlHelper.ExecteNonQuery(spsLog);
                            #endregion
                        }

                    }
                }
                else
                {
                    SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "USP_ADD_PACKAGE_FB_TO_ZX_485", list1);//USP_ADD_PACKAGE_FB_TO_ZX_TX
                    if (SqlHelper.ExecteNonQuery(spe) > 0)
                    {
                        dataset3.Tables[0].Rows.Clear();
                        AcceptCompanyId.Text = CarNo.Text = DriverName.Text = DriverPhone.Text = Remark.Text = "";
                        txtBatch.Text = GetMaxInOneVehicleFlag(CommonClass.UserInfo.SiteName);
                        SendDate.DateTime = CommonClass.gcdate;
                        MsgBox.ShowOK("分拨成功!");
                    }
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

        private void AcceptCompanyId_SelectedIndexChanged(object sender, EventArgs e)
        {
            //AcceptSiteName.Properties.Items.Clear();
            //AcceptWebName.Properties.Items.Clear();
            //AcceptSiteName.Text = AcceptWebName.Text = "";
            //if (dsZXSite == null || dsZXSite.Tables.Count < 2 || dsZXSite.Tables[1].Rows.Count == 0) return;

            //string companyid = AcceptCompanyId.Text.Split('|')[0];
           // DataRow[] drs = dsZXSite.Tables[1].Select("companyid='" + companyid + "'");
          //  DataRow[] drs = dsZXSite.Tables[1].Select("BelongToCompany like '%" + companyid + "%'");

            //if (drs == null || drs.Length == 0) return;

            //foreach (DataRow dr in drs)
            //{
            //    companyid = ConvertType.ToString(dr["SiteName"]);
            //    if (companyid != "" && !AcceptSiteName.Properties.Items.Contains(companyid))
            //        AcceptSiteName.Properties.Items.Add(companyid);
            //}
            if (AcceptCompanyId.Text.Trim() == "100|ZQTMS")
            {
                AcceptSiteName.Properties.Items.Clear();
                AcceptSiteName.Properties.Items.Add("贵阳");
            }

            AcceptSiteName.SelectedIndex = 0;
        }

        private void AcceptSiteName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //AcceptWebName.Properties.Items.Clear();
            //AcceptWebName.Text = "";
            //if (dsZXSite == null || dsZXSite.Tables.Count < 3 || dsZXSite.Tables[2].Rows.Count == 0) return;

            //string companyid = AcceptCompanyId.Text.Split('|')[0];
            //string siteName = AcceptSiteName.Text;
            //DataRow[] drs = dsZXSite.Tables[2].Select(string.Format("companyid='{0}' AND SiteName='{1}'", companyid, siteName));
            //if (drs == null || drs.Length == 0) return;

            //foreach (DataRow dr in drs)
            //{
            //    companyid = ConvertType.ToString(dr["WebName"]);
            //    if (companyid != "" && !AcceptWebName.Properties.Items.Contains(companyid))
            //        AcceptWebName.Properties.Items.Add(companyid);
            //}
            if (AcceptCompanyId.Text.Trim() == "100|ZQTMS")
            {
                AcceptWebName.Properties.Items.Clear();
                AcceptWebName.Properties.Items.Add("贵阳操作部");
            }

            AcceptWebName.SelectedIndex = 0;
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
    }
}