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

namespace ZQTMS.UI
{
    public partial class frmSettlementValidation : BaseForm
    {
        DataSet ds = new DataSet();
        public frmSettlementValidation()
        {
            InitializeComponent();
        }

        private void frmSettlementValidation_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3);
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            CommonClass.SetSite(StartSite, true);
            CommonClass.SetSite(TransferSite, true);
            CommonClass.SetWeb(BegWeb, StartSite.Text);
            CommonClass.SetCause(CauseName, true);
            CommonClass.SetArea(AreaName, CauseName.Text);
            CommonClass.SetUser(BillMan, BegWeb.Text);
            CauseName.Text = CommonClass.UserInfo.CauseName;
            AreaName.Text = CommonClass.UserInfo.AreaName;
            StartSite.Text = CommonClass.UserInfo.SiteName;
            TransferSite.Text = "全部";
            BegWeb.Text = CommonClass.UserInfo.WebName;
            BillMan.Text = CommonClass.UserInfo.UserName;
            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());

        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //提取2017.10.31wbw
        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.EditValue));
                list.Add(new SqlPara("edate", edate.EditValue));
                list.Add(new SqlPara("CauseName", CauseName.Text.Trim()=="全部"? "%%":CauseName.Text.Trim()));
                list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text.Trim()));
                list.Add(new SqlPara("BegWeb", BegWeb.Text.Trim() == "全部" ? "%%" : BegWeb.Text.Trim()));
                list.Add(new SqlPara("BillMan", BillMan.Text.Trim() == "全部" ? "%%" : BillMan.Text.Trim()));
                list.Add(new SqlPara("StartSite", StartSite.Text.Trim() == "全部" ? "%%" : StartSite.Text.Trim()));
                list.Add(new SqlPara("TransferSite", TransferSite.Text.Trim() == "全部" ? "%%" : TransferSite.Text.Trim()));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_SettlementStatisticsCLONE", list);
                 ds = SqlHelper.GetDataSet(spe);
                 if (ds != null)
                 {
                     myGridControl1.DataSource = ds.Tables[0];
                 }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        //计算
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                decimal FeeWeight = Convert.ToDecimal(ds.Tables[0].Rows[i]["FeeWeight"]);//计费重量
                decimal FeeVolume = Convert.ToDecimal(ds.Tables[0].Rows[i]["FeeVolume"]);//计费体积
                decimal OperationWeight = Convert.ToDecimal(ds.Tables[0].Rows[i]["OperationWeight"]);//结算重量
                string Package = ds.Tables[0].Rows[i]["Package"].ToString();
                string TransitMode = ds.Tables[0].Rows[i]["TransitMode"].ToString();//运输方式
                string ReceivProvince = ds.Tables[0].Rows[i]["ReceivProvince"].ToString();
                string ReceivCity = ds.Tables[0].Rows[i]["ReceivCity"].ToString();
                string ReceivArea = ds.Tables[0].Rows[i]["ReceivArea"].ToString();
                string ReceivStreet = ds.Tables[0].Rows[i]["ReceivStreet"].ToString();
                string FeeType = ds.Tables[0].Rows[i]["FeeType"].ToString();
                string TransferMode = ds.Tables[0].Rows[i]["TransferMode"].ToString();
                string WebRole = ds.Tables[0].Rows[i]["WebRole"].ToString();
                string BegWeb = ds.Tables[0].Rows[i]["BegWeb"].ToString();

                if ((FeeVolume / (decimal)3.8) * 1000 >= FeeWeight)
                {
                    if (Package != "纸箱" && Package != "纤袋" && Package != "膜")
                    {
                        ds.Tables[0].Rows[i]["jszlyz"] = Math.Round((FeeVolume / (decimal)3.8) * 1000 * (decimal)1.05, 2);//结算重量验证
                        ds.Tables[0].Rows[i]["jszlcy"] = Math.Round((FeeVolume / (decimal)3.8) * 1000 * (decimal)1.05, 2) - OperationWeight;//结算重量差异
                       
                    }
                    else
                    {
                        ds.Tables[0].Rows[i]["jszlyz"] = Math.Round((FeeVolume / (decimal)3.8) * 1000, 2);//结算重量验证
                        ds.Tables[0].Rows[i]["jszlcy"] = Math.Round((FeeVolume / (decimal)3.8) * 1000, 2) - OperationWeight;//结算重量差异
                    }
                }
                else
                {
                    if (Package != "纸箱" && Package != "纤袋" && Package != "膜")
                    {
                        ds.Tables[0].Rows[i]["jszlyz"] = Math.Round(FeeWeight * (decimal)1.05, 2);//结算重量验证
                        ds.Tables[0].Rows[i]["jszlcy"] = Math.Round(FeeWeight * (decimal)1.05, 2) - OperationWeight;//结算重量差异
                    }
                    else
                    {
                        ds.Tables[0].Rows[i]["jszlyz"] = FeeWeight;//结算重量验证
                        ds.Tables[0].Rows[i]["jszlcy"] = FeeWeight - OperationWeight;//结算重量差异
                    }
                }
                decimal jszlyz = Convert.ToDecimal(ds.Tables[0].Rows[i]["jszlyz"]);//结算重量验证
                string AlienGoods = ds.Tables[0].Rows[i]["AlienGoods"].ToString();//是否为异性货

                #region 结算始发操作费jssfczfyz，jssfczfcy
                string StartSite = ds.Tables[0].Rows[i]["StartSite"].ToString();
                DataRow[] drsfczf = CommonClass.dsDepartureOptFee_ZX.Tables[0].Select("FromSite='" + StartSite + "' and TransitMode='中强专线'");
                if (drsfczf.Length > 0 && ds.Tables[0].Rows[i]["TransitMode"].ToString() != "中强项目")
                {
                    decimal HeavyPrice = ConvertType.ToDecimal(drsfczf[0]["HeavyPrice"]);//重货
                    decimal LightPrice = ConvertType.ToDecimal(drsfczf[0]["LightPrice"]);//轻货
                    decimal ParcelPriceMin = ConvertType.ToDecimal(drsfczf[0]["ParcelPriceMin"]);//最低一票
                    decimal DepartureOptFee = Convert.ToDecimal(ds.Tables[0].Rows[i]["DepartureOptFee"]);
                    decimal acc = jszlyz * HeavyPrice;

                    ds.Tables[0].Rows[i]["jssfczfyz"] = acc ;
                    ds.Tables[0].Rows[i]["jssfczfcy"] = acc - DepartureOptFee;
                            if (AlienGoods == "1")
                            {
                                acc = acc * (decimal)1.5;
                                ds.Tables[0].Rows[i]["jssfczfyz"] = acc;
                                ds.Tables[0].Rows[i]["jssfczfcy"] = acc - DepartureOptFee;
                            }
                    if (acc < ParcelPriceMin)
                    {
                            ds.Tables[0].Rows[i]["jssfczfyz"] = ParcelPriceMin;
                            ds.Tables[0].Rows[i]["jssfczfcy"] = ParcelPriceMin - DepartureOptFee;
                       
                    }
                }
                #endregion


                #region 结算始发分拨费jssffbfyz，jssffbfcy 
                string begWeb = ds.Tables[0].Rows[i]["begWeb"].ToString();
                DataRow[] drTerminalAllotFee = CommonClass.dsDepartureAllotFee_ZX.Tables[0].Select("BegWeb='" + begWeb + "' and TransferSite='" + ds.Tables[0].Rows[i]["TransferSite"].ToString() + "' and FromSite='" + ds.Tables[0].Rows[0]["StartSite"].ToString() + "'");
                if(drTerminalAllotFee == null || drTerminalAllotFee.Length <= 0)
                {
                    drTerminalAllotFee = CommonClass.dsDepartureAllotFee_ZX.Tables[0].Select("BegWeb='"+ begWeb + "' and TransferSite='全部'");
                }

                if (drTerminalAllotFee != null && drTerminalAllotFee.Length > 0)
                {
                        decimal DepartureAllotFee = ConvertType.ToDecimal(ds.Tables[0].Rows[i]["DepartureAllotFee"]);//始发分拨费
                        decimal HeavyPrice = ConvertType.ToDecimal(drTerminalAllotFee[0]["HeavyPrice"]);//重货
                        decimal LightPrice = ConvertType.ToDecimal(drTerminalAllotFee[0]["LightPrice"]);//轻货
                        decimal ParcelPriceMin = ConvertType.ToDecimal(drTerminalAllotFee[0]["ParcelPriceMin"]);//最低一票
                        decimal acc = Math.Max(jszlyz * HeavyPrice, jszlyz * LightPrice);
                        acc = Math.Max(acc, ParcelPriceMin);
                        ds.Tables[0].Rows[i]["jssffbfyz"] = acc;
                        ds.Tables[0].Rows[i]["jssffbfcy"] = acc - DepartureAllotFee;
                }
                else
                {
                    decimal DepartureAllotFee = ConvertType.ToDecimal(ds.Tables[0].Rows[i]["DepartureAllotFee"]);//始发分拨费
                    ds.Tables[0].Rows[i]["jssffbfyz"] = 0;
                    ds.Tables[0].Rows[i]["jssffbfcy"] = -DepartureAllotFee;
                }
                #endregion


                #region //结算干线费jsgxfyz，jsgxfcy
                if(ds.Tables[0].Rows[i]["TransitMode"] != "中强整车")
                {
                string Bsite = ds.Tables[0].Rows[i]["StartSite"].ToString();
                string MiddleSite = ds.Tables[0].Rows[i]["TransferSite"].ToString();
                string TransitModeStr = ds.Tables[0].Rows[i]["TransitMode"].ToString();
                decimal MainlineFee = ConvertType.ToDecimal(ds.Tables[0].Rows[i]["MainlineFee"]);
                DataRow[] drjsgxf = CommonClass.dsMainlineFee_ZX.Tables[0].Select("FromSite='" + Bsite + "' and TransferSite='" + MiddleSite + "' and TransportMode='中强专线'");
                if (drjsgxf.Length > 0)
                {
                    decimal ParcelPriceMin = ConvertType.ToDecimal(drjsgxf[0]["ParcelPriceMin"]);//最低一票
                    decimal HeavyPrice = ConvertType.ToDecimal(drjsgxf[0]["HeavyPrice"]);//重货
                    decimal LightPrice = ConvertType.ToDecimal(drjsgxf[0]["LightPrice"]);//轻货
                    decimal w = Math.Round(FeeWeight, 2);
                    decimal v = Math.Round(FeeVolume, 2);
                    decimal acc = Math.Max(HeavyPrice * w, LightPrice * v);

                    try
                    {
                        List<SqlPara> list = new List<SqlPara>();
                        list.Add(new SqlPara("beginSite", Bsite));
                        list.Add(new SqlPara("endSite", MiddleSite));
                        SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASMAINLINEFEETZ_NEW", list);
                        DataSet ds1 = SqlHelper.GetDataSet(sps);
                        if (ds1!= null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                        {
                            DataRow[] rows = ds.Tables[0].Select("attribute='第三批'");
                            if(jszlyz <= 300)
                            {
                                acc = acc*(decimal)1.2;
                            }
                        }
                        else
                        {
                            if (Bsite == "深圳" && jszlyz > 3000 && v != 0)
                            {
                                if (((w / (decimal)1000 / v) > (decimal)(1.0 / 1.5)) || ((w / (decimal)1000 / v) < (decimal)(1.0 / 7.0)))
                                {
                                    acc = acc * (decimal)0.95;
                                }
                            }
                        }
                        if (Package != "纸箱" && Package != "纤袋" && Package != "膜")
                        {
                            acc = acc * (decimal)1.05;
                        }
                        else
                        {

                        }
                        if(AlienGoods == "1")
                        {
                            acc = acc * (decimal)1.5;
                        }
                        acc = Math.Max(acc,ParcelPriceMin);
                        ds.Tables[0].Rows[i]["jsgxfyz"] = acc;
                        ds.Tables[0].Rows[i]["jsgxfcy"] = acc - MainlineFee;
                    }
                    catch (Exception ex)
                        {
                            MsgBox.ShowException(ex);
                        }

                }
                else
                {
                    ds.Tables[0].Rows[i]["jsgxfyz"] = 0;
                    ds.Tables[0].Rows[i]["jsgxfcy"] = - MainlineFee;
                }
                }   
                #endregion


                #region //结算终端分拨费jszdfbfyz，jszdfbfcy不做

                //基本运费 jbyfyz，jbyfcy

                //decimal Freight = ConvertType.ToDecimal(ds.Tables[0].Rows[i]["Freight"]);//基本运费
                //if (TransitMode == "中强快线")
                //{
                //    List<SqlPara> list_FeeKX = new List<SqlPara>();
                //    list_FeeKX.Add(new SqlPara("StartSite", StartSite));
                //    list_FeeKX.Add(new SqlPara("Province", ReceivProvince));
                //    list_FeeKX.Add(new SqlPara("City", ReceivCity));
                //    list_FeeKX.Add(new SqlPara("Area", ReceivArea));
                //    list_FeeKX.Add(new SqlPara("TransferMode", TransferMode));
                //    SqlParasEntity sps_feeKX = new SqlParasEntity(OperType.Query, "QSP_GET_BASFREIGHTFEE_KDKX", list_FeeKX);
                //    DataSet dsFeeKX = SqlHelper.GetDataSet(sps_feeKX);
                //    if (dsFeeKX.Tables[0].Rows.Count > 0)
                //    {
                //        decimal ParcelPriceMin = ConvertType.ToDecimal(dsFeeKX.Tables[0].Rows[0]["ParcelPriceMin"]);//最低一票
                //        decimal HeavyPrice = ConvertType.ToDecimal(dsFeeKX.Tables[0].Rows[0]["HeavyPrice"]);//重量单价
                //        decimal LightPrice = ConvertType.ToDecimal(dsFeeKX.Tables[0].Rows[0]["LightPrice"]);//体积单价
                //        decimal fee = Math.Round(Math.Max(ParcelPriceMin, Math.Max(HeavyPrice * FeeWeight, LightPrice * FeeVolume)),2);
                //        ds.Tables[0].Rows[i]["jbyfyz"] = fee;
                //        ds.Tables[0].Rows[i]["jbyfcy"] = fee - Freight;
                //    }

                //}
                //else if (TransitMode == "一票通")
                //{
                //    List<SqlPara> list_FeeKX = new List<SqlPara>();
                //    list_FeeKX.Add(new SqlPara("StartSite", StartSite));
                //    list_FeeKX.Add(new SqlPara("Province", ReceivProvince));
                //    list_FeeKX.Add(new SqlPara("City", ReceivCity));
                //    list_FeeKX.Add(new SqlPara("Area", ReceivArea));
                //    list_FeeKX.Add(new SqlPara("TransferMode", TransferMode));
                //    SqlParasEntity sps_feeKX = new SqlParasEntity(OperType.Query, "QSP_GET_BASFREIGHTFEE_KDYPT", list_FeeKX);
                //    DataSet dsFeeKX = SqlHelper.GetDataSet(sps_feeKX);
                //    if (dsFeeKX.Tables[0].Rows.Count > 0)
                //    {
                //        decimal ParcelPriceMin = ConvertType.ToDecimal(dsFeeKX.Tables[0].Rows[0]["ParcelPriceMin"]);//最低一票
                //        decimal HeavyPrice = ConvertType.ToDecimal(dsFeeKX.Tables[0].Rows[0]["HeavyPrice"]);//重量单价
                //        decimal LightPrice = ConvertType.ToDecimal(dsFeeKX.Tables[0].Rows[0]["LightPrice"]);//体积单价
                //        decimal fee = Math.Round(Math.Max(ParcelPriceMin, Math.Max(HeavyPrice * FeeWeight, LightPrice * FeeVolume)),2);
                //        ds.Tables[0].Rows[i]["jbyfyz"] = fee;
                //        ds.Tables[0].Rows[i]["jbyfcy"] = fee - Freight;

                //    }


                //}
                //else
                //{
                //    List<SqlPara> list_Fee = new List<SqlPara>();
                //    list_Fee.Add(new SqlPara("StartSite", StartSite));
                //    list_Fee.Add(new SqlPara("Province", ReceivProvince));
                //    list_Fee.Add(new SqlPara("City", ReceivCity));
                //    list_Fee.Add(new SqlPara("Area", ReceivArea));
                //    list_Fee.Add(new SqlPara("TransferMode", TransferMode));
                //    list_Fee.Add(new SqlPara("TransitMode", TransitMode));
                //    SqlParasEntity sps_fee = new SqlParasEntity(OperType.Query, "QSP_GET_BASFREIGHTFEE_KD", list_Fee);
                //     DataSet dsFee = SqlHelper.GetDataSet(sps_fee);
                //     if (dsFee.Tables[0].Rows.Count > 0)
                //     {
                //         decimal ParcelPriceMin = ConvertType.ToDecimal(dsFee.Tables[0].Rows[0]["ParcelPriceMin"]);//最低一票
                //         decimal HeavyPrice = ConvertType.ToDecimal(dsFee.Tables[0].Rows[0]["HeavyPrice"]);//重量单价
                //         decimal LightPrice = ConvertType.ToDecimal(dsFee.Tables[0].Rows[0]["LightPrice"]);//体积单价
                //         decimal fee = Math.Round(Math.Max(ParcelPriceMin, Math.Max(HeavyPrice * FeeWeight, LightPrice * FeeVolume)),2);
                //         ds.Tables[0].Rows[i]["jbyfyz"] = fee;
                //         ds.Tables[0].Rows[i]["jbyfcy"] = fee - Freight;

                //     }
                //     else
                //     {
                //         ds.Tables[0].Rows[i]["jbyfyz"] = 0;
                //         ds.Tables[0].Rows[i]["jbyfcy"] = 0 - Freight;

                //     }

                //}
                #endregion


                #region //结算终端操作费jszdczfyz，jszdczfcy
                string TransferSite = ds.Tables[0].Rows[i]["TransferSite"].ToString();
                decimal TerminalOptFee = Convert.ToDecimal(ds.Tables[0].Rows[i]["TerminalOptFee"]);
                DataRow[] drTerminalOptFee = CommonClass.dsTerminalOptFee_ZX.Tables[0].Select("TransferSite='" + TransferSite + "' and TransitMode='中强专线'");
                if (drTerminalOptFee.Length > 0 && TransitMode != "中强项目" && TransitMode != "中强城际" && TransferMode != "司机直送")
                {
                    if ((TransferMode == "一票通" && ds.Tables[0].Rows[i]["StartSite"].ToString() == "深圳" && ds.Tables[0].Rows[i]["TransferSite"] == "深圳"))
                    {
                        ds.Tables[0].Rows[i]["jszdczfyz"] = (decimal)0;
                        ds.Tables[0].Rows[i]["jszdczfcy"] = (decimal)0 - TerminalOptFee;
                    }
                    else
                    {
                        decimal HeavyPrice = ConvertType.ToDecimal(drTerminalOptFee[0]["HeavyPrice"]);//重货
                        decimal LightPrice = ConvertType.ToDecimal(drTerminalOptFee[0]["LightPrice"]);//轻货
                        decimal ParcelPriceMin = ConvertType.ToDecimal(drTerminalOptFee[0]["ParcelPriceMin"]);//最低一票
                        decimal acc = HeavyPrice * jszlyz;
                        if (AlienGoods == "1")
                        {
                            acc = acc * (decimal)1.5;
                            ds.Tables[0].Rows[i]["jszdczfyz"] = acc;
                            ds.Tables[0].Rows[i]["jszdczfcy"] = acc - TerminalOptFee;
                        }
                        if (acc <= ParcelPriceMin)
                        {

                            ds.Tables[0].Rows[i]["jszdczfyz"] = ParcelPriceMin;
                            ds.Tables[0].Rows[i]["jszdczfcy"] = ParcelPriceMin - TerminalOptFee;
                        }
                    }
                }
                else
                {
                    ds.Tables[0].Rows[i]["jszdczfyz"] = (decimal)0;
                    ds.Tables[0].Rows[i]["jszdczfcy"] = (decimal)0 - TerminalOptFee;
                }
                #endregion


                #region //结算送货费jsshfyz，jsshfcy	
                
                decimal DeliveryFee2 = Convert.ToDecimal(ds.Tables[0].Rows[i]["DeliveryFee"]);
                if (ReceivProvince == "香港")
                {
                    if (TransferMode.Contains("送") && TransferMode != "专车直送")
                    {
                        string sql = "Province='" + ReceivProvince
                                    + "' and City='" + ReceivCity
                                    + "' and Area='" + ReceivArea
                                    + "' and Street='" + ReceivStreet
                                    + "' and " + jszlyz + ">=w1"
                                    + " and " + jszlyz + " <w2";
                        DataRow[] drDeliveryFee = CommonClass.dsSendPriceHK.Tables[0].Select(sql);
                        if (drDeliveryFee.Length > 0)
                        {
                            string fmtext = drDeliveryFee[0]["Expression"].ToString();
                            decimal Additional = ConvertType.ToDecimal(drDeliveryFee[0]["Additional"].ToString());
                            fmtext = fmtext.Replace("w", jszlyz.ToString());
                            if(Additional <= 0)
                            {
                                Additional = 0;
                            }
                            DataTable dt = new DataTable();
                            decimal DeliveryFee = Math.Round(decimal.Parse(dt.Compute(fmtext, "").ToString()), 2, MidpointRounding.AwayFromZero);
                            List<SqlPara> list1 = new List<SqlPara>();//检查香港结算送货费是否小于最低一票--毛慧20171027
                            SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_basDeliveryFeeHK", list1);
                            DataTable dt1 = SqlHelper.GetDataTable(sps1);
                            DataRow[] arrs = dt1.Select("Province='" + ReceivProvince + "' and City='" + ReceivCity + "' and Area='" + ReceivArea + "' and Street='" + ReceivStreet + "'");
                            decimal temp = 0;
                            if (arrs != null && arrs.Length > 0)
                            {
                                foreach (DataRow arr in arrs)
                                {
                                    if (Convert.ToDecimal(arr["lowestprice"].ToString()) > temp)
                                    {
                                        temp = Convert.ToDecimal(arr["lowestprice"].ToString());
                                    }
                                }
                            }
                            if (temp > DeliveryFee)
                            {
                                DeliveryFee = temp;
                            }
                            if (AlienGoods == "1")//毛慧20171028--异形货上浮50%
                            {
                                DeliveryFee += DeliveryFee * (decimal)0.5;
                            }
                            ds.Tables[0].Rows[i]["jsshfyz"] = DeliveryFee + Additional;
                            ds.Tables[0].Rows[i]["jsshfcy"] = DeliveryFee + Additional - DeliveryFee2;
                        }
                    }
                }
                else
                {
                    decimal maxFee = 400;
                    if (TransitMode == "中强快线")
                    {
                        //ds.Tables[0].Rows[i]["jsshfyz"] = (decimal)400 * (decimal)1.25;
                        //ds.Tables[0].Rows[i]["jsshfcy"] = (decimal)400 * (decimal)1.25 - DeliveryFee2;
                        maxFee = maxFee * (decimal)1.25;
                    }
                    if (TransitMode == "一票通")
                    {
                        //ds.Tables[0].Rows[i]["jsshfyz"] = (decimal)400 * (decimal)1.05;
                        //ds.Tables[0].Rows[i]["jsshfcy"] = (decimal)400 * (decimal)1.05 - DeliveryFee2;
                        maxFee = maxFee * (decimal)1.05;
                    }
                    if (TransferMode == "送货" || TransferMode == "司机直送")
                    {
                        string sql = "Province='" + ReceivProvince
                                       + "' and City='" + ReceivCity
                                       + "' and Area='" + ReceivArea
                                       + "' and Street='" + ReceivStreet
                                        + "'";
                        DataRow[] drDeliveryFee = CommonClass.dsSendPrice_ZX.Tables[0].Select(sql);
                        if (drDeliveryFee == null || drDeliveryFee.Length <= 0)
                        {
                            ds.Tables[0].Rows[i]["jsshfyz"] = (decimal)0;
                            ds.Tables[0].Rows[i]["jsshfcy"] = (decimal)0 - DeliveryFee2;
                        }
                        if (drDeliveryFee != null && drDeliveryFee.Length > 0)
                        {
                            decimal DeliveryFee = getDeliveryFee1(drDeliveryFee, OperationWeight);
                            if (TransitMode == "一票通")
                            {
                                DeliveryFee = DeliveryFee * (decimal)1.05;
                            }
                            if (TransitMode == "中强快线")
                            {
                                DeliveryFee = DeliveryFee * (decimal)1.25;
                            }
                            if (AlienGoods == "1")
                            {
                                DeliveryFee = DeliveryFee * (decimal)1.5;
                            }
                            if (BegWeb.Contains("经理人"))
                            {
                                DeliveryFee = DeliveryFee * (decimal)1.1;
                            }
                            if (DeliveryFee < 30)
                            {
                                DeliveryFee = 30;
                            }
                            if (DeliveryFee > 400)
                            {
                                DeliveryFee = 400;
                            }

                            if (TransitMode == "中强整车")
                            {
                                DeliveryFee = 0;
                            }

                            if (TransitMode == "中强项目")
                            {
                                DeliveryFee = 0;
                            }
                            ds.Tables[0].Rows[i]["jsshfyz"] = DeliveryFee;
                            ds.Tables[0].Rows[i]["jsshfcy"] = DeliveryFee - DeliveryFee2;
                        }

                    }
                    else if (TransferMode == "自提")
                    {
                        decimal transferFee = Convert.ToDecimal(ds.Tables[0].Rows[i]["TransferFee"]);
                        string PickGoodsSite = ds.Tables[0].Rows[i]["PickGoodsSite"].ToString();
                        decimal DeliveryFee = 0;
                        if (transferFee <= 0)
                        {
                            if (CommonClass.Arg.PickUpFreeWeb.Contains(PickGoodsSite))
                            {
                                DeliveryFee = 0;
                            }
                            else
                            {
                                string sql = "Province='" + ReceivProvince
                                               + "' and City='" + ReceivCity
                                               + "' and Area='" + ReceivArea
                                               + "' and Street='" + ReceivStreet + "'";
                                //+ "' and TransportMode='" + TransitModeStr + "'";
                                DataRow[] drDeliveryFee = CommonClass.dsSendPrice_ZX.Tables[0].Select(sql);
                                //if (drDeliveryFee == null || drDeliveryFee.Length <= 0)
                                //{
                                //    sql = "Province='全国' and City='全国' and Area='全国' and Street='全国' and TransportMode='" + TransitModeStr + "'";
                                //    drDeliveryFee = CommonClass.dsSendPrice.Tables[0].Select(sql);
                                //}
                                if (drDeliveryFee == null || drDeliveryFee.Length > 0)
                                {
                                    DeliveryFee = getDeliveryFee1(drDeliveryFee, OperationWeight);
                                    if (TransitMode == "一票通")
                                    {
                                        DeliveryFee = DeliveryFee * (decimal)1.05;
                                    }
                                    if (TransitMode == "中强快线")
                                    {
                                        DeliveryFee = DeliveryFee * (decimal)1.25;
                                    }
                                    if (AlienGoods == "1")
                                    {
                                        DeliveryFee = DeliveryFee * (decimal)1.5;
                                    }
                                    DeliveryFee = DeliveryFee * (decimal)0.25;
                                    if (TransitMode == "中强整车")
                                    {
                                        DeliveryFee = 0;
                                    }
                                    if (TransitMode == "中强项目")
                                    {
                                        DeliveryFee = 0;
                                    }
                                    ds.Tables[0].Rows[i]["jsshfyz"] = DeliveryFee;
                                    ds.Tables[0].Rows[i]["jsshfcy"] = DeliveryFee - DeliveryFee2;

                                }
                            }
                        }
                        else
                        {
                            ds.Tables[0].Rows[i]["jsshfyz"] = 0;
                            ds.Tables[0].Rows[i]["jsshfcy"] = 0 - DeliveryFee2;
                        }
                    }
                }
                #endregion


                #region //结算中转费jszzfyz，jszzfcy
                
                decimal TransferFee = Convert.ToDecimal(ds.Tables[0].Rows[i]["TransferFee"]);
                if (TransitMode != "中强整车")
                {
                    if (TransitMode != "司机直送" && TransitMode != "中强项目")
                    {
                        DataRow[] drTransferFee = CommonClass.dsTransferFee_ZX.Tables[0].Select("TransferSite='" + ds.Tables[0].Rows[i]["TransferSite"].ToString() + "' and ToProvince='"
                            + ds.Tables[0].Rows[i]["ReceivProvince"].ToString() + "' and ToCity='" + ds.Tables[0].Rows[i]["ReceivCity"] + "' and ToArea='" + ds.Tables[0].Rows[i]["ReceivArea"].ToString() + "' and TransitMode='中强专线'");
                        if (drTransferFee.Length > 0)
                        {
                            decimal HeavyPrice = ConvertType.ToDecimal(drTransferFee[0]["HeavyPrice"]);//重货
                            decimal LightPrice = ConvertType.ToDecimal(drTransferFee[0]["LightPrice"]);//轻货
                            decimal ParcelPriceMin = ConvertType.ToDecimal(drTransferFee[0]["ParcelPriceMin"]);//最低一票
                            decimal fee = Math.Round(Math.Max(FeeWeight * HeavyPrice, FeeVolume * LightPrice), 2);

                            ds.Tables[0].Rows[i]["jszzfyz"] = fee;
                            ds.Tables[0].Rows[i]["jszzfcy"] = fee - TransferFee;

                            if (ReceivProvince != "香港" && ReceivProvince != "海南省")
                            {
                                if (FeeWeight <= 300)
                                {
                                    fee = fee * (decimal)1.2;
                                    ds.Tables[0].Rows[i]["jszzfyz"] = fee;
                                    ds.Tables[0].Rows[i]["jszzfcy"] = fee - TransferFee;
                                }

                                if (FeeVolume > 3000)
                                {
                                    fee = fee * (decimal)0.9;
                                    ds.Tables[0].Rows[i]["jszzfyz"] = fee;
                                    ds.Tables[0].Rows[i]["jszzfcy"] = fee - TransferFee;
                                }
                            }
                            if (Package != "纸箱" && Package != "纤袋" && Package != "膜")
                            {
                                fee = Math.Round(fee * (decimal)1.05, 2);
                                ds.Tables[0].Rows[i]["jszzfyz"] = fee;
                                ds.Tables[0].Rows[i]["jszzfcy"] = fee - TransferFee;
                            }

                            if (AlienGoods == "1")
                            {
                                fee = fee * (decimal)1.5;
                                ds.Tables[0].Rows[i]["jszzfyz"] = fee;
                                ds.Tables[0].Rows[i]["jszzfcy"] = fee - TransferFee;
                            }

                            if (fee <= ParcelPriceMin)
                            {
                                ds.Tables[0].Rows[i]["jszzfyz"] = ParcelPriceMin;
                                ds.Tables[0].Rows[i]["jszzfcy"] = ParcelPriceMin - TransferFee;
                            }
                        }
                        else
                        {
                            ds.Tables[0].Rows[i]["jszzfyz"] = 0;
                            ds.Tables[0].Rows[i]["jszzfcy"] = 0 - TransferFee;
                        }

                    }
                    ds.Tables[0].Rows[i]["jszzfyz"] = 0;
                    ds.Tables[0].Rows[i]["jszzfcy"] = 0 - TransferFee;
                }
                 #endregion


                #region //结算回单费jshdfyz，jshdfcy
                string IsReceiptFee = ds.Tables[0].Rows[i]["IsReceiptFee"].ToString();
                decimal ReceiptFee_C = Convert.ToDecimal(ds.Tables[0].Rows[i]["ReceiptFee_C"].ToString());
                if (IsReceiptFee == "1")
                {
                    DataRow[] drhdf = CommonClass.dsSurchargeFee_ZX.Tables[0].Select("ProjectType='回单费' ");
                    if (drhdf.Length > 0)
                    {
                        decimal InnerLowest = ConvertType.ToDecimal(drhdf[0]["InnerLowest"]);
                        decimal InnerStandard = ConvertType.ToDecimal(drhdf[0]["InnerStandard"]);
                        ds.Tables[0].Rows[i]["jshdfyz"] = InnerStandard;
                        ds.Tables[0].Rows[i]["jshdfcy"] = InnerStandard - ReceiptFee_C;
                    }
                }
                else
                {
                    ds.Tables[0].Rows[i]["jshdfyz"] = 0;
                    ds.Tables[0].Rows[i]["jshdfcy"] = 0 - ReceiptFee_C;
                }
                #endregion


                #region //结算控货费jskhfyz，jskhfcy 
                string NoticeState = ds.Tables[0].Rows[i]["NoticeState"].ToString();
                decimal NoticeFee_C = Convert.ToDecimal(ds.Tables[0].Rows[i]["NoticeFee_C"]);
                if (NoticeState == "1")
                {
                    DataRow[] drkhf = CommonClass.dsSurchargeFee_ZX.Tables[0].Select("ProjectType='控货费' ");
                    if (drkhf.Length > 0)
                    {
                        decimal InnerLowest = ConvertType.ToDecimal(drkhf[0]["InnerLowest"]);
                        ds.Tables[0].Rows[i]["jskhfyz"] = InnerLowest;
                        ds.Tables[0].Rows[i]["jskhfcy"] = InnerLowest - NoticeFee_C;
                    }
                }
                else
                {
                    ds.Tables[0].Rows[i]["jskhfyz"] = 0;
                    ds.Tables[0].Rows[i]["jskhfcy"] = 0 - NoticeFee_C;
                }
                #endregion


                #region//结算保价费jsbjfyz，jsbjfcy 
                string IsSupportValue = ds.Tables[0].Rows[i]["IsSupportValue"].ToString();
                decimal SupportValue_C = Convert.ToDecimal(ds.Tables[0].Rows[i]["SupportValue_C"]);//声明价值
                if (IsSupportValue == "1")
                {
                        decimal DeclareValue = Convert.ToDecimal(ds.Tables[0].Rows[i]["DeclareValue"]);//声明价值
                        if (DeclareValue < 5000)
                        {
                            ds.Tables[0].Rows[i]["jsbjfyz"] = (decimal)3;
                            ds.Tables[0].Rows[i]["jsbjfcy"] = (decimal)3 - SupportValue_C;
                        }
                        else
                        {
                            ds.Tables[0].Rows[i]["jsbjfyz"] = (decimal)3 + (DeclareValue - 5000) * (decimal)0.0005;
                            ds.Tables[0].Rows[i]["jsbjfcy"] = (decimal)3 + (DeclareValue - 5000) * (decimal)0.0005 - SupportValue_C;
                        }
                }
                else
                {
                    ds.Tables[0].Rows[i]["jsbjfyz"] = 0;
                    ds.Tables[0].Rows[i]["jsbjfcy"] = 0 - SupportValue_C;
                }
                #endregion


                #region //结算代收手续费jsdssxfyz，jsdssxfcy .
                string IsAgentFee = ds.Tables[0].Rows[i]["IsAgentFee"].ToString();
                decimal AgentFee_C = Convert.ToDecimal(ds.Tables[0].Rows[i]["AgentFee_C"]);//结算代收手续费
                if (IsAgentFee == "1")
                {
                    DataRow[] drdssxf = CommonClass.dsSurchargeFee_ZX.Tables[0].Select("ProjectType='代收手续费' ");
                    if (drdssxf.Length > 0)
                    {
                        decimal InnerLowest = ConvertType.ToDecimal(drdssxf[0]["InnerLowest"]);
                        decimal InnerStandard = ConvertType.ToDecimal(drdssxf[0]["InnerStandard"]);
                        decimal CollectionPay = Convert.ToDecimal(ds.Tables[0].Rows[i]["CollectionPay"]);//代收货款
                        if (CollectionPay > 0)
                        {
                            if (CollectionPay * InnerStandard > InnerLowest)
                            {
                                ds.Tables[0].Rows[i]["jsdssxfyz"] = Math.Round(CollectionPay * InnerStandard,2);
                                ds.Tables[0].Rows[i]["jsdssxfcy"] = Math.Round(CollectionPay * InnerStandard,2) - AgentFee_C;
                            }
                            else
                            {
                                ds.Tables[0].Rows[i]["jsdssxfyz"] = InnerLowest;
                                ds.Tables[0].Rows[i]["jsdssxfcy"] = InnerLowest - AgentFee_C;
                            }
                        }
                    }
                }
                else
                {
                    ds.Tables[0].Rows[i]["jsdssxfyz"] = 0;
                    ds.Tables[0].Rows[i]["jsdssxfcy"] = 0 - AgentFee_C;
                }
                #endregion


                #region //结算装卸费jszxfyz，jszxfcy
                string IsHandleFee = ds.Tables[0].Rows[i]["IsHandleFee"].ToString();
                decimal HandleFee_C = Convert.ToDecimal(ds.Tables[0].Rows[i]["HandleFee_C"]);
                if (IsHandleFee == "1")
                {
                    DataRow[] drzxf = CommonClass.dsSurchargeFee_ZX.Tables[0].Select("ProjectType='装卸费' ");
                    if (drzxf.Length > 0)
                    {
                        decimal InnerLowest = ConvertType.ToDecimal(drzxf[0]["InnerLowest"]);
                        decimal InnerStandard = ConvertType.ToDecimal(drzxf[0]["InnerStandard"]);
                        if (jszlyz * InnerStandard / 1000 > InnerLowest)
                        {
                            ds.Tables[0].Rows[i]["jszxfyz"] = Math.Round(jszlyz * InnerStandard / 1000,2);
                            ds.Tables[0].Rows[i]["jszxfcy"] = Math.Round(jszlyz * InnerStandard / 1000,2) - HandleFee_C;
                        }
                        else
                        {
                            ds.Tables[0].Rows[i]["jszxfyz"] = InnerLowest;
                            ds.Tables[0].Rows[i]["jszxfcy"] = InnerLowest - HandleFee_C;
                        }
                    }
                }
                else
                {
                    ds.Tables[0].Rows[i]["jszxfyz"] = 0;
                    ds.Tables[0].Rows[i]["jszxfcy"] = 0 - HandleFee_C;
                }
                #endregion


                #region //结算进仓费jsjcfyz，jsjcfcy
                string IsStorageFee = ds.Tables[0].Rows[i]["IsStorageFee"].ToString();
                decimal StorageFee_C = Convert.ToDecimal(ds.Tables[0].Rows[i]["StorageFee_C"]);
                if (IsStorageFee == "1")
                {
                    DataRow[] drjcf = CommonClass.dsSurchargeFee_ZX.Tables[0].Select("ProjectType='进仓费' ");
                    if (drjcf.Length > 0)
                    {
                        decimal InnerLowest = ConvertType.ToDecimal(drjcf[0]["InnerLowest"]);
                        decimal InnerStandard = ConvertType.ToDecimal(drjcf[0]["InnerStandard"]);

                        ds.Tables[0].Rows[i]["jsjcfyz"] = Math.Round(Math.Max(InnerLowest, FeeVolume * InnerStandard + 30), 2);
                        ds.Tables[0].Rows[i]["jsjcfcy"] = Math.Round(Math.Max(InnerLowest, FeeVolume * InnerStandard + 30), 2) - StorageFee_C;
                    }
                }
                else
                {
                     ds.Tables[0].Rows[i]["jsjcfyz"] = 0;
                     ds.Tables[0].Rows[i]["jsjcfcy"] = 0 - StorageFee_C;
                }
                #endregion

            //    //结算仓储费jsccfyz，jsccfcy不做




            //    //结算叉车费jsccfyz1，jsccfcy1不做


                #region //结算税金jssjyz，jssjcy 
                string IsInvoice = ds.Tables[0].Rows[i]["IsInvoice"].ToString();
                decimal Tax_C = Convert.ToDecimal(ds.Tables[0].Rows[i]["Tax_C"]);
                if (IsInvoice == "1")
                {
                    DataRow[] drsj = CommonClass.dsSurchargeFee_ZX.Tables[0].Select("ProjectType='税金' ");
                    if (drsj.Length > 0)
                    {
                        decimal InnerLowest = ConvertType.ToDecimal(drsj[0]["InnerLowest"]);
                        decimal InnerStandard = ConvertType.ToDecimal(drsj[0]["InnerStandard"]);
                        decimal PaymentAmout = Convert.ToDecimal(ds.Tables[0].Rows[i]["PaymentAmout"]);
                        decimal Tax = Convert.ToDecimal(ds.Tables[0].Rows[i]["Tax"]);
                        decimal CollectionPay = Convert.ToDecimal(ds.Tables[0].Rows[i]["CollectionPay"]);
                        if (PaymentAmout * InnerStandard > InnerLowest)
                        {
                            ds.Tables[0].Rows[i]["jssjyz"] = Math.Round((PaymentAmout-CollectionPay-Tax) * InnerStandard,2);
                            ds.Tables[0].Rows[i]["jssjcy"] = Math.Round((PaymentAmout-CollectionPay-Tax) * InnerStandard,2) - Tax_C;
                        }
                        else
                        {
                            ds.Tables[0].Rows[i]["jssjyz"] = InnerLowest;
                            ds.Tables[0].Rows[i]["jssjcy"] = InnerLowest - Tax_C;
                        }
                    }
                }
                else
                {
                    ds.Tables[0].Rows[i]["jssjyz"] = 0;
                    ds.Tables[0].Rows[i]["jssjcy"] = 0 - Tax_C;
                }
                #endregion


                #region //结算上楼费jsslfyz，jsslfcy
                string IsUpstairFee = ds.Tables[0].Rows[i]["IsUpstairFee"].ToString();
                decimal UpstairFee_C = Convert.ToDecimal(ds.Tables[0].Rows[i]["UpstairFee_C"]);
                 if (IsUpstairFee == "1")
                 {
                     DataRow[] drslf = CommonClass.dsSurchargeFee_ZX.Tables[0].Select("ProjectType='上楼费' ");
                     if (drslf.Length > 0)
                     {
                         decimal InnerLowest = ConvertType.ToDecimal(drslf[0]["InnerLowest"]); //最低一票金额
                         decimal InnerStandard = ConvertType.ToDecimal(drslf[0]["InnerStandard"]); //结算标准 
                         if (jszlyz * InnerStandard / 1000 > InnerLowest)
                         {
                             ds.Tables[0].Rows[i]["jsslfyz"] = Math.Round(jszlyz * InnerStandard / 1000,2);
                             ds.Tables[0].Rows[i]["jsslfcy"] =  Math.Round(jszlyz * InnerStandard / 1000,2) - UpstairFee_C;
                         }
                         else
                         {
                             ds.Tables[0].Rows[i]["jsslfyz"] = InnerLowest;
                             ds.Tables[0].Rows[i]["jsslfcy"] = InnerLowest - UpstairFee_C;
                         }
                     }
                 }
                 else
                 {
                     ds.Tables[0].Rows[i]["jsslfyz"] = 0;
                     ds.Tables[0].Rows[i]["jsslfcy"] = 0 - UpstairFee_C;
                 }
                #endregion


                 #region //结算工本费jsgbfyz，jsgbfcy
                 //DataRow[] drgbf = CommonClass.dsSurchargeFee.Tables[0].Select("ProjectType='工本费' ");
                //if (drgbf.Length > 0)
                //{
                    //decimal InnerLowest = ConvertType.ToDecimal(drgbf[0]["InnerLowest"]); //最低一票金额
                    //decimal InnerStandard = ConvertType.ToDecimal(drgbf[0]["InnerStandard"]); //结算标准 
                decimal Num = Convert.ToDecimal(ds.Tables[0].Rows[i]["Num"]);
                decimal Expense_C = Convert.ToDecimal(ds.Tables[0].Rows[i]["Expense_C"]);
                 #endregion


                ds.Tables[0].Rows[i]["jsgbfyz"] = Math.Round((decimal)0.1 * Num,2) ;
                ds.Tables[0].Rows[i]["jsgbfcy"] = Math.Round((decimal)0.1 * Num - Expense_C,2);

            }
            myGridControl1.DataSource = ds.Tables[0];
        }

        private void CauseName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(AreaName, CauseName.Text);
            CommonClass.SetCauseWeb(BegWeb, CauseName.Text, AreaName.Text);
        }

        private void AreaName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(BegWeb, CauseName.Text, AreaName.Text);
        }


        /// <summary>
        /// 计算结算送货费
        /// </summary>
        /// <param name="drDeliveryFee"></param>
        private decimal getDeliveryFee(DataRow[] drDeliveryFee, decimal Weight, decimal w, decimal v, string Package, string FeeType)
        {
            //decimal w0_300 = ConvertType.ToDecimal(drDeliveryFee[0]["w0_300"]);
            //decimal w300_500 = ConvertType.ToDecimal(drDeliveryFee[0]["w300_500"]);
            //decimal w500_800 = ConvertType.ToDecimal(drDeliveryFee[0]["w500_800"]);
            //decimal w800_1000 = ConvertType.ToDecimal(drDeliveryFee[0]["w800_1000"]);
            //decimal w1000_2000 = ConvertType.ToDecimal(drDeliveryFee[0]["w1000_2000"]);
            //decimal w2000_3000 = ConvertType.ToDecimal(drDeliveryFee[0]["w2000_3000"]);
            //decimal w3000_100000 = ConvertType.ToDecimal(drDeliveryFee[0]["w3000_100000"]);

            //decimal v0_300 = ConvertType.ToDecimal(drDeliveryFee[0]["v0_300"]);
            //decimal v300_500 = ConvertType.ToDecimal(drDeliveryFee[0]["v300_500"]);
            //decimal v500_800 = ConvertType.ToDecimal(drDeliveryFee[0]["v500_800"]);
            //decimal v800_1000 = ConvertType.ToDecimal(drDeliveryFee[0]["v800_1000"]);
            //decimal v1000_2000 = ConvertType.ToDecimal(drDeliveryFee[0]["v1000_2000"]);
            //decimal v2000_3000 = ConvertType.ToDecimal(drDeliveryFee[0]["v2000_3000"]);
            //decimal v3000_100000 = ConvertType.ToDecimal(drDeliveryFee[0]["v3000_100000"]);

            ////decimal DeliveryFee = ConvertType.ToDecimal(drDeliveryFee[0]["DeliveryFee"]);
            decimal DeliveryFee = 0;
            //decimal wDeliveryFee = 0;
            //decimal vDeliveryFee = 0;
            ////decimal w = 0;
            ////decimal v = 0;
            ////string Package = "";
            ////string FeeType = "";
            
            ////     w = Convert.ToDecimal(ds.Tables[0].Rows[0]["FeeWeight"]); ;
            ////     v = Convert.ToDecimal(ds.Tables[0].Rows[0]["FeeVolume"]); ;
            ////     Package = ds.Tables[0].Rows[0]["Package"].ToString();

            ////     FeeType = ds.Tables[0].Rows[0]["FeeType"].ToString();
            
            //    if (Weight >= 0 && Weight <= 300)
            //    {
            //        wDeliveryFee = w0_300 * w;
            //        vDeliveryFee = v0_300 * v;
            //    }
            //    else if (Weight >= 300 && Weight <= 500)
            //    {
            //        wDeliveryFee = w300_500 * w;
            //        vDeliveryFee = v300_500 * v;
            //    }
            //    else if (Weight >= 500 && Weight <= 800)
            //    {
            //        wDeliveryFee = w500_800 * w;
            //        vDeliveryFee = v500_800 * v;
            //    }
            //    else if (Weight >= 800 && Weight <= 1000)
            //    {
            //        wDeliveryFee = w800_1000 * w;
            //        vDeliveryFee = v800_1000 * v;
            //    }
            //    else if (Weight >= 1000 && Weight <= 2000)
            //    {
            //        wDeliveryFee = w1000_2000 * w;
            //        vDeliveryFee = v1000_2000 * v;
            //    }
            //    else if (Weight >= 2000 && Weight <= 3000)
            //    {
            //        wDeliveryFee = w2000_3000 * w;
            //        vDeliveryFee = v2000_3000 * v;
            //    }
            //    else if (Weight > 3000)
            //    {
            //        wDeliveryFee = w3000_100000 * w;
            //        vDeliveryFee = v3000_100000 * v;
            //    }

            //    if (Package != "纸箱" && Package != "纤袋" && Package != "膜")
            //    {

            //        wDeliveryFee = wDeliveryFee * Convert.ToDecimal(CommonClass.Arg.MainlineFeeRate);
            //        vDeliveryFee = vDeliveryFee * Convert.ToDecimal(CommonClass.Arg.MainlineFeeRate);

            //    }


            //    if (FeeType == "计重")
            //    {
            //        DeliveryFee += wDeliveryFee;
            //    }
            //    else
            //    {
            //        DeliveryFee += vDeliveryFee;
            //    }
            
            return DeliveryFee;
        }

        private DataTable LineWebs = null;
        private void loadLineWeb()
        {
            try
            {
                LineWebs = SqlHelper.GetDataTable(new SqlParasEntity(OperType.Query, "QSP_GET_LINEWEB"));
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private Boolean isLineWeb()
        {
            if (LineWebs == null)
            {
                loadLineWeb();
            }

            DataRow[] dr = LineWebs.Select("WebName= '" + CommonClass.UserInfo.WebName + "'");
            if (dr.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
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