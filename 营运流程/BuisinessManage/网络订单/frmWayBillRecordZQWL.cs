using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using DevExpress.XtraGrid.Columns;
using System.Data.OleDb;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using System.Net;

namespace ZQTMS.UI
{
    public partial class frmWayBillRecordZQWL : BaseForm
    {
        DataSet dsNew = new DataSet();
        DataSet ds = new DataSet();
        int b = 0, billnum = 0;
 
        string BillNos = string.Empty; //运单号
        string BillDates = string.Empty; //下单时间
        string StartSites = string.Empty;
        string TransferModes = string.Empty;
        string DestinationSites = string.Empty;
        string TransferSites = string.Empty;
        //string TransitLiness = string.Empty;
        string TransitModes = string.Empty;
        string CusOderNos = string.Empty;
        string Packages = string.Empty; //hj20190118
        string ConsignorCompanys = string.Empty;//hj20190118
        string ConsigneeCellPhones = string.Empty;
        string ConsigneeNames = string.Empty;
        string ConsigneeCompanys = string.Empty;//hj20190118
        string ReceivAddresss = string.Empty;//hj20190118
        string PickGoodsSites = string.Empty;
        string ReceivProvinces = string.Empty;
        string ReceivCitys = string.Empty;
        string ReceivAreas = string.Empty;
        string ConsignorNames = string.Empty;
        string ConsignorCellPhones = string.Empty;
        string Varietiess = string.Empty;
        string Nums = string.Empty;
        string FeeWeights = string.Empty;
        string FeeVolumes = string.Empty;
        string Weights = string.Empty;
        string Volumes = string.Empty;
        string Freights = string.Empty;
        string DeliFees = string.Empty;
        string ReceivFees = string.Empty;
        string DiscountTransfers = string.Empty;
        string CollectionPays = string.Empty;
        string PaymentModes = string.Empty;
        string PaymentAmouts = string.Empty;
        string NowPays = string.Empty;
        string FetchPays = string.Empty;
        string MonthPays = string.Empty;
        string ReceiptPays = string.Empty;
        string BillMans = string.Empty;
        string BegWebs = string.Empty;
        string ActualFreights = string.Empty;
        string OperationWeights = string.Empty;//结算重量
        string companyids = string.Empty;//公司ID
        string BillStates = string.Empty;//运单状态
        string ReceiptConditions = string.Empty;
        string BillRemarks = string.Empty;
        string Salesmans = string.Empty;
        /* //2020-11-24  新增模板列 */
        string SupportValue = string.Empty;//保价费 是否保价IsSupportValue
        string IsSupportValue = string.Empty;
        string DeclareValue = string.Empty;  //声明价值
        string HandleFee = string.Empty; //装卸费 是否装卸 IsHandleFee
        string IsHandleFee = string.Empty;
        string MatPay = string.Empty;//垫付费 是否垫付IsHandleFee
        string IsMatPay = string.Empty;
        string AgentFee = string.Empty; //手续费（代收货款手续费)
        string UpstairFee = string.Empty; //上楼费 是否上楼 IsUpstairFee
        string IsUpstairFee = string.Empty;
        string OtherFee = string.Empty; //其它费  其它费合计OtherTotalFee  是否收其它费IsOtherFee
        string IsOtherFee = string.Empty;
        string orderSn = string.Empty; //订单号
        string OptTimes = string.Empty; //开单时间
        string GoodNos = string.Empty; //货物编号
        string GoodStates = string.Empty;  //货物状态

        string areaName = string.Empty; //事业部
        string causeName = string.Empty; //大区
        string depName = string.Empty; //部门
        string SiteName = string.Empty; //站点
        string WebName = string.Empty; //网点
        string userName = string.Empty; //登录人
        public frmWayBillRecordZQWL()
        {
            InitializeComponent();
        }

        private void frmWayBillRecordZQWL_Load(object sender, EventArgs e)
        {
            //QSP_GET_waybill_sc_TX
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, myGridView2);
            GridOper.SetGridViewProperty(myGridView1, myGridView2);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例 
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "选择文件";
            ofd.Filter = "Microsoft Execl文件|*.xls;*.xlsx";
            ofd.FilterIndex = 1;
            ofd.DefaultExt = "xls";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (!ofd.SafeFileName.EndsWith(".xls") && !ofd.SafeFileName.EndsWith(".xlsx"))
                {
                    XtraMessageBox.Show("请选择Excel文件!", "文件导入失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!ofd.CheckFileExists)
                {
                    XtraMessageBox.Show("文件不存在，请重新选择!", "文件导入失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ds = DsExecl(ofd.FileName);
                int i = 1;
                foreach (DataColumn columns in ds.Tables[0].Columns)
                {
                    if (columns.ColumnName.IndexOf('-') > 0)
                    {
                        GridColumn column = new GridColumn();
                        column.Caption = columns.ColumnName;
                        column.FieldName = columns.ColumnName;
                        column.Name = "gridColumnDt" + i;
                        column.Visible = true;
                        column.VisibleIndex = 6 + i;
                        column.Width = 80;
                        myGridView1.Columns.Add(column);
                        i++;
                    }
                }
                if (ds != null)
                {
                    myGridControl1.DataSource = ds.Tables[0];

                }
            }
        }

        private DataSet DsExecl(string filePath)
        {
            //string str = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + filePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\""; //此连接可以兼容2003和2007
            //string str = "Provider = Microsoft.Jet.OLEDB.12.0; Data Source = " + filePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\""; //此连接必须要安装2007
            string str = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + filePath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\""; //此连接智能读取2003格式
            OleDbConnection Conn = new OleDbConnection(str);
            try
            {
                Conn.Open();
                System.Data.DataTable dt = Conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                string tablename = "", sql = "";
                w_import_select_table wi = new w_import_select_table();
                wi.dt = dt;
                if (wi.ShowDialog() != DialogResult.Yes)
                { return null; }
                tablename = wi.listBoxControl1.Text.Trim();
                sql = "select * from [" + tablename + "]";

                OleDbDataAdapter da = new OleDbDataAdapter(sql, Conn);
                DataSet ds = new DataSet();
                da.Fill(ds, tablename);

                try
                {
                    SetColumnName(ds.Tables[0].Columns);
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex, "转换失败!\r\n请检查EXCEL列头是否与模板一致！");
                    return null;
                }
                return ds;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, ex.Message);
                return null;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open) Conn.Close();
            }
        }

        private void SetColumnName(DataColumnCollection c)
        {
            try
            {
                foreach (DataColumn dc in c)
                {
                    dc.ColumnName = dc.ColumnName.Trim();
                }

                #region 专线名称版
                /*列*/   
                c["订单号"].ColumnName = "orderSn"; //订单号  
                c["运单号"].ColumnName = "billSn"; //2020-11-24 运单号启用  有运单号取已有运单号，没有从运单号库中生成
                c["开单时间"].ColumnName = "orderTime"; //业务时间取BillDate这个值，开单日期取当前时间
                //c["始发站"].ColumnName = "StartSite"; //2020-11-24 短线停用 默认长沙
                c["交接方式"].ColumnName = "TransferMode"; //2020-11-24 等同于中强的送货方式 自提和送货 isSend 是=送货  否=自提
                //c["到站"].ColumnName = "DestinationSite";  //2020-11-24 短线停用  默认省市区+
                //c["中转地"].ColumnName = "TransferSite"; //2020-11-24 短线停用  默认长沙
                //c["运输方式"].ColumnName = "TransitMode";
                //c["客户单号"].ColumnName = "CusOderNo";
                c["发货公司名称"].ColumnName = "ConsignorCompany"; //2020-11-24 等同于发货地址  sendAddress 1
                c["发货人"].ColumnName = "sendMan"; //2020-11-24  发货联系人
                c["发货人手机"].ColumnName = "sendPhone"; //2020-11-24  发货人手机

                c["收货人手机"].ColumnName = "receivePhone";
                c["收货联系人"].ColumnName = "receiveMan";
                c["收货公司名称"].ColumnName = "ConsigneeCompany"; //receiveCompany
                c["收货详细地址"].ColumnName = "receiveAddress"; //2020-11-24  等同于收货地址
                //c["目的网点"].ColumnName = "PickGoodsSite";  //目的网点暂不需要
                c["收货省"].ColumnName = "receiveProvince";
                c["收货市"].ColumnName = "receiveCity";
                c["收货区"].ColumnName = "receiveArea";

                c["包装"].ColumnName = "packing"; //2020-11-24 
                c["品名"].ColumnName = "productName";
                c["件数"].ColumnName = "qty";

                //c["计费重量"].ColumnName = "FeeWeight";
                //c["计费体积"].ColumnName = "FeeVolume";
                c["重量"].ColumnName = "Weight"; 
                c["体积"].ColumnName = "Volume";

                //c["折扣折让"].ColumnName = "DiscountTransfer";
                c["代收货款"].ColumnName = "collectionAmount";
                c["总运费"].ColumnName = "PaymentAmout";
                //c["现付"].ColumnName = "NowPay";
                //c["提付"].ColumnName = "FetchPay";
                c["开单人"].ColumnName = "OptMan";  // 开单网点=开单人
                c["操作时间"].ColumnName = "OptTime";  
                //c["公司ID"].ColumnName = "companyid"; 
                //c["运单状态"].ColumnName = "BillState";
                c["回单要求"].ColumnName = "receiptInfo";  //2020-11-24  回单要求 可选择要几份回单等同于要求回单+数量
                c["备注"].ColumnName = "remarks"; //2020-11-24  备注信息
                //c["业务员"].ColumnName = "Salesman";

                /* //2020-11-24  新增模板列 */

                //c["月结"].ColumnName = "MonthPay"; //字段未有
                c["回单付"].ColumnName = "ReceiptPay"; //字段未有
                c["开单送货费"].ColumnName = "DeliFee";   //2020-11-24 等同于送货费 字段未有
                c["接货费"].ColumnName = "ReceivFee";  //2020-11-24  等同于提货费 字段未有
                c["付款方式"].ColumnName = "PaymentMode";   //字段未有
                c["基本运费"].ColumnName = "Freight"; //字段未有
                c["货物编号"].ColumnName = "ID"; //货物编号 字段未有 GoodNo
                c["承运状态"].ColumnName = "BILLSTATE"; //承运状态 字段未有 GoodState
                c["开单网点"].ColumnName = "BegWeb";  // 字段未有
                c["保价费"].ColumnName = "SupportValue";//保价费 是否保价IsSupportValue
                c["声明价值"].ColumnName = "DeclareValue";//声明价值
                c["装卸费"].ColumnName = "HandleFee";//装卸费 是否装卸 IsHandleFee
                c["垫付费"].ColumnName = "MatPay";//垫付费
                c["代收货款手续费"].ColumnName = "AgentFee";//手续费（代收货款手续费)
                c["上楼费"].ColumnName = "UpstairFee";//上楼费 是否上楼 IsUpstairFee
                c["其它费"].ColumnName = "OtherFee";//其它费  其它费合计OtherTotalFee  是否收其它费IsOtherFee
                #endregion

            }
            catch (Exception ex)
            {
                //MsgBox.ShowException(ex);
                MsgBox.ShowError("请检查自己导入的文档是否为标准格式！");
            }

        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.RowCount == 0)
            {
                return;
            }

            BillNos = "";
            BillDates = "";
            StartSites = "";
            TransferModes = "";
            DestinationSites = "";
            TransferSites = "";
            //TransitLiness = "";
            TransitModes = "";
            CusOderNos = "";
            Packages = ""; //hj20190118
            ConsignorCompanys = "";//hj20190118
            ConsigneeCellPhones = "";
            ConsigneeNames = "";
            ConsigneeCompanys = "";//hj20190118
            ReceivAddresss = "";//hj20190118
            PickGoodsSites = "";
            ReceivProvinces = "";
            ReceivCitys = "";
            ReceivAreas = "";
            ConsignorNames = "";
            ConsignorCellPhones = "";
            Varietiess = "";
            Nums = "";
            FeeWeights = "";
            FeeVolumes = "";
            Weights = "";
            Volumes = "";
            Freights = "";
            DeliFees = "";
            ReceivFees = "";
            DiscountTransfers = "";
            CollectionPays = "";
            PaymentModes = "";
            PaymentAmouts = "";
            NowPays = "";
            FetchPays = "";
            MonthPays = "";
            ReceiptPays = "";
            BillMans = "";
            BegWebs = "";
            ActualFreights = "";
            OperationWeights = "";//结算重量
            companyids = "";//公司ID
            BillStates = "";
            ReceiptConditions = "";
            BillRemarks = "";
            Salesmans = "";

            /* //2020-11-24  新增模板列 */
            SupportValue = "";//保价费 是否保价IsSupportValue
            IsSupportValue = "";
            DeclareValue = "";  //声明价值
            HandleFee = ""; //装卸费 是否装卸 IsHandleFee
            IsHandleFee = "";
            MatPay = "";//垫付费 是否垫付IsHandleFee
            IsMatPay = "";
            AgentFee = ""; //手续费（代收货款手续费)
            UpstairFee = ""; //上楼费 是否上楼 IsUpstairFee
            IsUpstairFee = "";
            OtherFee = ""; //其它费  其它费合计OtherTotalFee  是否收其它费IsOtherFee
            IsOtherFee = "";

            orderSn = ""; //订单号
            OptTimes = ""; //开单时间
            GoodNos = ""; //货物编号
            GoodStates = "";  //货物状态
            areaName = ""; //大区
            causeName = ""; //事业部
            depName = ""; //部门
            SiteName = ""; //站点
            WebName = ""; //网点
            userName = ""; //登录人
            //只有101的账号可以导入
            //if (CommonClass.UserInfo.companyid != "101" && CommonClass.UserInfo.companyid != "485" && CommonClass.UserInfo.companyid != "486")
            //{
            //    MsgBox.ShowOK("您不能使用此功能！");
            //    return;
            //}

            for (int j = 0; j < myGridView1.RowCount; j++)
            {   
                System.Threading.Thread.Sleep(300);
                if (myGridView1.RowCount >= 50)
                {
                    if (myGridView2.RowCount % 50 >= 0)
                    {
                        DataTable table = ds.Tables[0];
                        dsNew = ds.Clone();
                        DataTable table2 = dsNew.Tables[0];
                        //table2.Rows.Clear();
                        for (int k = 0; k < 50; k++)
                        {
                            table2.Rows.Add(table.Rows[k].ItemArray);
                        }
                        table2.AcceptChanges();
                        myGridControl2.DataSource = dsNew.Tables[0];

                        for (int i = 0; i < myGridView2.RowCount; i++)
                        {
                            /*新增*/
                            orderSn += myGridView2.GetRowCellValue(i, "orderSn").ToString() + "@"; 
                            BillNos += myGridView2.GetRowCellValue(i, "billSn").ToString() + "@"; //运单
                            BillDates += "@";//myGridView2.GetRowCellValue(i, "orderTime").ToString() + "@"; //开单时间
                            TransferModes += myGridView2.GetRowCellValue(i, "TransferMode").ToString() + "@"; //交接方式
                            Packages += myGridView2.GetRowCellValue(i, "packing").ToString() + "@"; //包装
                            Varietiess += myGridView2.GetRowCellValue(i, "productName").ToString() + "@"; //品名
                            Nums += myGridView2.GetRowCellValue(i, "qty").ToString() + "@"; //件数
                            ConsignorNames += myGridView2.GetRowCellValue(i, "sendMan").ToString() + "@";//发货人
                            ConsignorCompanys += myGridView2.GetRowCellValue(i, "ConsignorCompany").ToString() + "@";//发货公司
                            ConsignorCellPhones += myGridView2.GetRowCellValue(i, "sendPhone").ToString() + "@";//发货人手机

                            ConsigneeNames += myGridView2.GetRowCellValue(i, "receiveMan").ToString() + "@";//收货人
                            ConsigneeCellPhones += myGridView2.GetRowCellValue(i, "receivePhone").ToString() + "@";//收货人手机
                            ConsigneeCompanys += myGridView2.GetRowCellValue(i, "ConsigneeCompany").ToString() + "@";//收货公司
                            ReceivAddresss += myGridView2.GetRowCellValue(i, "receiveAddress").ToString() + "@";//收货详细地址
                             
                            //收货人省市区
                            ReceivProvinces += myGridView2.GetRowCellValue(i, "receiveProvince").ToString() + "@";
                            ReceivCitys += myGridView2.GetRowCellValue(i, "receiveCity").ToString() + "@";
                            ReceivAreas += myGridView2.GetRowCellValue(i, "receiveArea").ToString() + "@";

                            Weights += (myGridView2.GetRowCellValue(i, "Weight").ToString() == "" ? "0" : myGridView2.GetRowCellValue(i, "Weight").ToString()) + "@"; //重量
                            Volumes += (myGridView2.GetRowCellValue(i, "Volume").ToString() == "" ? "0" : myGridView2.GetRowCellValue(i, "Volume").ToString()) + "@";//体积

                            CollectionPays += (myGridView2.GetRowCellValue(i, "collectionAmount").ToString() == "" ? "0" : myGridView2.GetRowCellValue(i, "collectionAmount").ToString()) + "@";//代收货款
                            AgentFee += (myGridView2.GetRowCellValue(i, "AgentFee").ToString() == "" ? "0" : myGridView2.GetRowCellValue(i, "AgentFee").ToString()) + "@";//代收货款手续费
                            Freights += (myGridView2.GetRowCellValue(i, "Freight").ToString() == "" ? "0" : myGridView2.GetRowCellValue(i, "Freight").ToString()) + "@"; //基本运费

                            //Weights += myGridView2.GetRowCellValue(i, "Weight").ToString() + "@"; //重量
                            //Volumes += myGridView2.GetRowCellValue(i, "Volume").ToString() + "@"; //体积

                            //ReceivAreas += myGridView2.GetRowCellValue(i, "collectionAmount").ToString() + "@";//代收货款
                            //AgentFee += myGridView2.GetRowCellValue(i, "AgentFee").ToString() + "@";//代收货款手续费
                            //Freights += myGridView2.GetRowCellValue(i, "Freight").ToString() + "@";//基本运费
                            PaymentAmouts += myGridView2.GetRowCellValue(i, "PaymentAmout").ToString() + "@";//总运费合计
                            BillMans += myGridView2.GetRowCellValue(i, "OptMan").ToString() + "@";//开单人
                            OptTimes += "@";// myGridView2.GetRowCellValue(i, "OptTime").ToString() + "@";//开单时间
                            BillRemarks += myGridView2.GetRowCellValue(i, "remarks").ToString() + "@"; //备注
                            GoodNos += myGridView2.GetRowCellValue(i, "ID").ToString() + "@"; //货物批号
                            GoodStates += myGridView2.GetRowCellValue(i, "BILLSTATE").ToString() + "@"; //货物状态
                            BegWebs += myGridView2.GetRowCellValue(i, "BegWeb").ToString() + "@"; //开单网点
                            ReceiptConditions += myGridView2.GetRowCellValue(i, "receiptInfo").ToString() + "@"; //回单要求
                            ReceiptPays += (myGridView2.GetRowCellValue(i, "ReceiptPay").ToString() == "" ? "0" : myGridView2.GetRowCellValue(i, "ReceiptPay").ToString()) + "@"; //回单费 
                            DeliFees += (myGridView2.GetRowCellValue(i, "DeliFee").ToString() == "" ? "0" : myGridView2.GetRowCellValue(i, "DeliFee").ToString()) + "@"; //送货费 
                            ReceivFees += (myGridView2.GetRowCellValue(i, "ReceivFee").ToString() == "" ? "0" : myGridView2.GetRowCellValue(i, "ReceivFee").ToString()) + "@"; //接货费
                            PaymentModes += myGridView2.GetRowCellValue(i, "PaymentMode").ToString() + "@"; //付款方式
                            SupportValue += (myGridView2.GetRowCellValue(i, "SupportValue").ToString() == "" ? "0" : myGridView2.GetRowCellValue(i, "SupportValue").ToString()) + "@";// Convert.ToDecimal(myGridView2.GetRowCellValue(i, "SupportValue").ToString()) + "@"; //保价费 是否保价IsSupportValue
                            DeclareValue += (myGridView2.GetRowCellValue(i, "DeclareValue").ToString() == "" ? "0" : myGridView2.GetRowCellValue(i, "DeclareValue").ToString()) + "@";// Convert.ToDecimal(myGridView2.GetRowCellValue(i, "DeclareValue").ToString()) + "@";  //声明价值
                            HandleFee += (myGridView2.GetRowCellValue(i, "HandleFee").ToString() == "" ? "0" : myGridView2.GetRowCellValue(i, "HandleFee").ToString()) + "@";// Convert.ToDecimal(myGridView2.GetRowCellValue(i, "HandleFee").ToString()) + "@";  //装卸费 
                            MatPay += (myGridView2.GetRowCellValue(i, "MatPay").ToString() == "" ? "0" : myGridView2.GetRowCellValue(i, "MatPay").ToString()) + "@";//Convert.ToDecimal(myGridView2.GetRowCellValue(i, "MatPay").ToString()) + "@";
                            UpstairFee += (myGridView2.GetRowCellValue(i, "UpstairFee").ToString() == "" ? "0" : myGridView2.GetRowCellValue(i, "UpstairFee").ToString()) + "@";//Convert.ToDecimal(myGridView2.GetRowCellValue(i, "UpstairFee").ToString()) + "@";  //上楼费
                            OtherFee += (myGridView2.GetRowCellValue(i, "OtherFee").ToString() == "" ? "0" : myGridView2.GetRowCellValue(i, "OtherFee").ToString()) + "@";//Convert.ToDecimal(myGridView2.GetRowCellValue(i, "OtherFee").ToString()) + "@"; //其它费

                            #region 基础信息
                            areaName += CommonClass.UserInfo.AreaName + "@";
                            causeName += CommonClass.UserInfo.CauseName + "@";
                            depName += CommonClass.UserInfo.DepartName + "@";
                            SiteName += CommonClass.UserInfo.SiteName + "@";
                            WebName += CommonClass.UserInfo.WebName + "@";
                            userName += CommonClass.UserInfo.UserName + "@";
                            #endregion
                            #region 校验
                            //if (myGridView2.GetRowCellValue(i, "BillDate").ToString() == "")
                            //{
                            //    MsgBox.ShowOK("存在开单时间为空的数据，请检查！！！");
                            //    return;
                            //} 
                            //if (myGridView2.GetRowCellValue(i, "TransferMode").ToString() == "")
                            //{
                            //    MsgBox.ShowOK("存在运输方式为空的数据，请检查！！！");
                            //    return;
                            //} 
                            //if (myGridView2.GetRowCellValue(i, "TransitMode").ToString() == "")
                            //{
                            //    MsgBox.ShowOK("存在运输方式为空的数据，请检查！！！");
                            //    return;
                            //}
                            //if (myGridView2.GetRowCellValue(i, "ConsigneeCellPhone").ToString() == "")
                            //{
                            //    MsgBox.ShowOK("存在收货人手机为空的数据，请检查！！！");
                            //    return;
                            //}
                            //if (myGridView2.GetRowCellValue(i, "ConsigneeName").ToString() == "")
                            //{
                            //    MsgBox.ShowOK("存在收货联系人为空的数据，请检查！！！");
                            //    return;
                            //}
                            //if (myGridView2.GetRowCellValue(i, "PickGoodsSite").ToString() == "")
                            //{
                            //    MsgBox.ShowOK("存在目的网点为空的数据，请检查！！！");
                            //    return;
                            //}
                            //if (myGridView2.GetRowCellValue(i, "ReceivProvince").ToString() == "")
                            //{
                            //    MsgBox.ShowOK("存在收货省为空的数据，请检查！！！");
                            //    return;
                            //}
                            //if (myGridView2.GetRowCellValue(i, "ReceivCity").ToString() == "")
                            //{
                            //    MsgBox.ShowOK("存在收货市为空的数据，请检查！！！");
                            //    return;
                            //}
                            //if (myGridView2.GetRowCellValue(i, "ConsignorName").ToString() == "")
                            //{
                            //    MsgBox.ShowOK("存在发货联系人为空的数据，请检查！！！");
                            //    return;
                            //}
                            //if (myGridView2.GetRowCellValue(i, "ConsignorCellPhone").ToString() == "")
                            //{
                            //    MsgBox.ShowOK("存在发货人手机为空的数据，请检查！！！");
                            //    return;
                            //}
                            //if (myGridView2.GetRowCellValue(i, "Varieties").ToString() == "")
                            //{
                            //    MsgBox.ShowOK("存在品名为空的数据，请检查！！！");
                            //    return;
                            //}
                            //if (myGridView2.GetRowCellValue(i, "Num").ToString() == "")
                            //{
                            //    MsgBox.ShowOK("存在件数为空的数据，请检查！！！");
                            //    return;
                            //}
                            //if (ConvertType.ToDecimal(myGridView1.GetRowCellValue(i, "FeeWeight").ToString()) <= 0)
                            //{
                            //    MsgBox.ShowOK("计费重量不能小于等于0，请检查！！！");
                            //    return;
                            //}
                            //if (ConvertType.ToDecimal(myGridView1.GetRowCellValue(i, "FeeVolume").ToString()) <= 0)
                            //{
                            //    MsgBox.ShowOK("计费体积不能小于等于0，请检查！！！");
                            //    return;
                            //}
                            //if (myGridView2.GetRowCellValue(i, "Weight").ToString() == "")
                            //{
                            //    MsgBox.ShowOK("存在重量为空的数据，请检查！！！");
                            //    return;
                            //}
                            //if (myGridView2.GetRowCellValue(i, "Volume").ToString() == "")
                            //{
                            //    MsgBox.ShowOK("存在体积为空的数据，请检查！！！");
                            //    return;
                            //}
                            //if (myGridView2.GetRowCellValue(i, "Freight").ToString() == "")
                            //{
                            //    MsgBox.ShowOK("存在基本运费为空的数据，请检查！！！");
                            //    return;
                            //}
                            //if (myGridView2.GetRowCellValue(i, "DeliFee").ToString() == "")
                            //{
                            //    MsgBox.ShowOK("存在开单送货费为空的数据，请检查！！！");
                            //    return;
                            //}
                            //if (myGridView2.GetRowCellValue(i, "PaymentMode").ToString() == "")
                            //{
                            //    MsgBox.ShowOK("存在付款方式为空的数据，请检查！！！");
                            //    return;
                            //}
                            //if (myGridView2.GetRowCellValue(i, "PaymentAmout").ToString() == "")
                            //{
                            //    MsgBox.ShowOK("存在总运费为空的数据，请检查！！！");
                            //    return;
                            //}
                            //if (myGridView2.GetRowCellValue(i, "BillMan").ToString() == "")
                            //{
                            //    MsgBox.ShowOK("存在开单人为空的数据，请检查！！！");
                            //    return;
                            //}
                            //if (myGridView2.GetRowCellValue(i, "BegWeb").ToString() == "")
                            //{
                            //    MsgBox.ShowOK("存在开单网点为空的数据，请检查！！！");
                            //    return;
                            //}
                            //if (myGridView2.GetRowCellValue(i, "BillState").ToString() == "")
                            //{
                            //    MsgBox.ShowOK("存在运单状态为空的数据，请检查！！！");
                            //    return;
                            //}
                            //网点检查
                            //string a = myGridView2.GetRowCellValue(i, "BegWeb").ToString();
                            //string c = myGridView2.GetRowCellValue(i, "companyid").ToString();
                            //getdata(a, c);
                            #endregion
                        }
                        //GetBillNo(BillNos);
                        //if (billnum == 1) return;

                        List<SqlPara> list = new List<SqlPara>(); //orderSn
                        list.Add(new SqlPara("orderSn", orderSn)); //订单
                        list.Add(new SqlPara("billSn", BillNos)); //运单
                        list.Add(new SqlPara("orderTime", BillDates)); //开单时间
                        list.Add(new SqlPara("TransferMode", TransferModes)); //交接方式
                        list.Add(new SqlPara("packing", Packages)); //包装
                        list.Add(new SqlPara("productName", Varietiess)); //品名
                        list.Add(new SqlPara("qty", Nums)); //件数
                        list.Add(new SqlPara("sendMan", ConsignorNames)); //发货人
                        list.Add(new SqlPara("sendAddress", ConsignorCompanys)); //发货公司
                        list.Add(new SqlPara("sendPhone", ConsignorCellPhones));//发货电话
                        list.Add(new SqlPara("receiveMan", ConsigneeNames));//收货人
                        list.Add(new SqlPara("receivePhone", ConsigneeCellPhones)); //收货人手机
                        list.Add(new SqlPara("receiveCompany", ConsigneeCompanys)); //收货人公司
                        list.Add(new SqlPara("receiveAddress", ReceivAddresss));//收货详细地址

                        list.Add(new SqlPara("receiveProvince", ReceivProvinces));//收货省
                        list.Add(new SqlPara("receiveCity", ReceivCitys));//收货市
                        list.Add(new SqlPara("receiveArea", ReceivAreas)); //收货区
                        list.Add(new SqlPara("Weights", Weights)); //重量
                        list.Add(new SqlPara("Volumes", Volumes)); //体积

                        list.Add(new SqlPara("collectionAmount", CollectionPays)); //代收货款
                        list.Add(new SqlPara("AgentFee", AgentFee)); //代收货款手续费
                        list.Add(new SqlPara("Freight", Freights)); //基本运费
                        list.Add(new SqlPara("PaymentAmout", PaymentAmouts)); //总运费合计
                        list.Add(new SqlPara("OptMan", BillMans)); //开单人
                        list.Add(new SqlPara("OptTime", OptTimes)); //开单时间
                        list.Add(new SqlPara("remarks", BillRemarks)); //备注
                        list.Add(new SqlPara("GoodNo", GoodNos)); //货物批号
                        list.Add(new SqlPara("GoodState", GoodStates)); //货物状态
                        list.Add(new SqlPara("BegWeb", BegWebs)); //开单网点
                        list.Add(new SqlPara("receiptInfo", ReceiptConditions)); //回单要求
                        list.Add(new SqlPara("ReceiptPay", ReceiptPays)); //回单费
                        list.Add(new SqlPara("DeliFee", DeliFees)); //送货费
                        list.Add(new SqlPara("ReceivFee", ReceivFees)); //接货费
                        list.Add(new SqlPara("PaymentMode", PaymentModes)); //付款方式
                        list.Add(new SqlPara("SupportValue", SupportValue)); //保价费
                        list.Add(new SqlPara("DeclareValue", DeclareValue)); //声明价值
                        list.Add(new SqlPara("HandleFee", HandleFee)); //装卸费
                        list.Add(new SqlPara("MatPay", MatPay)); //垫付费
                        list.Add(new SqlPara("UpstairFee", UpstairFee)); //上楼费
                        list.Add(new SqlPara("OtherFee", OtherFee)); //其它费 

                        list.Add(new SqlPara("causeName", causeName)); //事业部 
                        list.Add(new SqlPara("areaName", areaName)); //大区
                        list.Add(new SqlPara("depName", depName)); //部门 
                        list.Add(new SqlPara("SiteName", SiteName)); //站点 
                        list.Add(new SqlPara("WebName", WebName)); //网点 
                        list.Add(new SqlPara("userName", userName)); //登录人 

                        SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "USP_ADD_WAYBILLWL_UPLOAD", list);
                        if (SqlHelper.ExecteNonQuery(spe) > 0)
                        {
                            MsgBox.ShowOK();
                            dsNew.Clear();
                        }

                        if (dsNew.Tables[0].Rows.Count == 0)
                        {
                            for (int m = 0; m < 50; m++)
                            {
                                ds.Tables[0].Rows[m].Delete();
                            }
                            ds.AcceptChanges();
                        }
                        if (ds.Tables[0].Rows.Count == 0 || myGridView1.RowCount == 0) return;
                        if (myGridView1.RowCount < 50 && myGridView1.RowCount > 0)
                        {
                            updata();
                            return;
                        }


                    }
                }
                if (myGridView1.RowCount < 50 && myGridView1.RowCount > 0)
                {
                    updata();
                    return;
                }


            }            

        }
        private void getdata(string a, string c)
        {
            try
            {
                b = 0;
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("a", a));
                list.Add(new SqlPara("c", c));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "USP_GET_BASWEB_WEBNAME", list);
                DataSet ds = SqlHelper.GetDataSet(spe);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    b = 1;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void updata()
        {
             BillNos = "";
             BillDates = "";
             StartSites = "";
             TransferModes = "";
             DestinationSites = "";
             TransferSites = "";
             //TransitLiness = "";
             TransitModes = "";
             CusOderNos = "";
             Packages = ""; //hj20190118
             ConsignorCompanys = "";//hj20190118
             ConsigneeCellPhones = "";
             ConsigneeNames = "";
             ConsigneeCompanys = "";//hj20190118
             ReceivAddresss = "";//hj20190118
             PickGoodsSites = "";
             ReceivProvinces = "";
             ReceivCitys = "";
             ReceivAreas = "";
             ConsignorNames = "";
             ConsignorCellPhones = "";
             Varietiess = "";
             Nums = "";
             FeeWeights = "";
             FeeVolumes = "";
             Weights = "";
             Volumes = "";
             Freights = "";
             DeliFees = "";
             ReceivFees = "";
             DiscountTransfers = "";
             CollectionPays = "";
             PaymentModes = "";
             PaymentAmouts = "";
             NowPays = "";
             FetchPays = "";
             MonthPays = "";
             ReceiptPays = "";
             BillMans = "";
             BegWebs = "";
             ActualFreights = "";
             OperationWeights = "";//结算重量
             companyids = "";//公司ID
             BillStates = "";
             ReceiptConditions = "";
             BillRemarks = "";
             Salesmans = "";

             /* //2020-11-24  新增模板列 */
            SupportValue = "";//保价费 是否保价IsSupportValue
            IsSupportValue = "";
            DeclareValue = "";  //声明价值
            HandleFee = ""; //装卸费 是否装卸 IsHandleFee
            IsHandleFee = "";
            MatPay = "";//垫付费 是否垫付IsHandleFee
            IsMatPay = "";
            AgentFee = ""; //手续费（代收货款手续费)
            UpstairFee = ""; //上楼费 是否上楼 IsUpstairFee
            IsUpstairFee = "";
            OtherFee = ""; //其它费  其它费合计OtherTotalFee  是否收其它费IsOtherFee
            IsOtherFee = "";

            orderSn = ""; //订单号
            OptTimes = ""; //开单时间
            GoodNos = ""; //货物编号
            GoodStates = "";  //货物状态
            orderSn = ""; //订单号
            OptTimes = ""; //开单时间
            GoodNos = ""; //货物编号
            GoodStates = "";  //货物状态
            areaName = ""; //事业部
            causeName = ""; //大区
            depName = ""; //部门
            SiteName = ""; //站点
            WebName = ""; //网点
            userName = ""; //登录人
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                /*新增*/
                orderSn += myGridView1.GetRowCellValue(i, "orderSn").ToString() + "@";
                BillNos += myGridView1.GetRowCellValue(i, "billSn").ToString() + "@"; //运单
                BillDates += "@";// myGridView1.GetRowCellValue(i, "orderTime").ToString() + "@"; //开单时间
                TransferModes += myGridView1.GetRowCellValue(i, "TransferMode").ToString() + "@"; //交接方式
                Packages += myGridView1.GetRowCellValue(i, "packing").ToString() + "@"; //包装
                Varietiess += myGridView1.GetRowCellValue(i, "productName").ToString() + "@"; //品名
                Nums += myGridView1.GetRowCellValue(i, "qty").ToString() + "@"; //件数
                ConsignorNames += myGridView1.GetRowCellValue(i, "sendMan").ToString() + "@";//发货人
                ConsignorCompanys += myGridView1.GetRowCellValue(i, "ConsignorCompany").ToString() + "@";//发货公司
                ConsignorCellPhones += myGridView1.GetRowCellValue(i, "sendPhone").ToString() + "@";//发货人手机

                ConsigneeNames += myGridView1.GetRowCellValue(i, "receiveMan").ToString() + "@";//收货人
                ConsigneeCellPhones += myGridView1.GetRowCellValue(i, "receivePhone").ToString() + "@";//收货人手机
                ConsigneeCompanys += myGridView1.GetRowCellValue(i, "ConsigneeCompany").ToString() + "@";//收货公司
                ReceivAddresss += myGridView1.GetRowCellValue(i, "receiveAddress").ToString() + "@";//收货详细地址

                //收货人省市区
                ReceivProvinces += myGridView1.GetRowCellValue(i, "receiveProvince").ToString() + "@";
                ReceivCitys += myGridView1.GetRowCellValue(i, "receiveCity").ToString() + "@";
                ReceivAreas += myGridView1.GetRowCellValue(i, "receiveArea").ToString() + "@";

                Weights += (myGridView1.GetRowCellValue(i, "Weight").ToString() == "" ? "0" : myGridView1.GetRowCellValue(i, "Weight").ToString()) + "@"; //重量
                Volumes += (myGridView1.GetRowCellValue(i, "Volume").ToString() == "" ? "0" : myGridView1.GetRowCellValue(i, "Volume").ToString()) + "@";//体积

                CollectionPays += (myGridView1.GetRowCellValue(i, "collectionAmount").ToString() == "" ? "0" : myGridView1.GetRowCellValue(i, "collectionAmount").ToString()) + "@";//代收货款
                AgentFee += (myGridView1.GetRowCellValue(i, "AgentFee").ToString() == "" ? "0" : myGridView1.GetRowCellValue(i, "AgentFee").ToString()) + "@";//代收货款手续费
                Freights += (myGridView1.GetRowCellValue(i, "Freight").ToString() == "" ? "0" : myGridView1.GetRowCellValue(i, "Freight").ToString()) + "@"; //基本运费
                PaymentAmouts += myGridView1.GetRowCellValue(i, "PaymentAmout").ToString() + "@";//总运费合计
                BillMans += myGridView1.GetRowCellValue(i, "OptMan").ToString() + "@";//开单人
                OptTimes += "@"; //myGridView1.GetRowCellValue(i, "OptTime").ToString() + "@";//开单时间
                BillRemarks += myGridView1.GetRowCellValue(i, "remarks").ToString() + "@"; //备注
                GoodNos += myGridView1.GetRowCellValue(i, "ID").ToString() + "@"; //货物批号
                GoodStates += myGridView1.GetRowCellValue(i, "BILLSTATE").ToString() + "@"; //货物状态
                BegWebs += myGridView1.GetRowCellValue(i, "BegWeb").ToString() + "@"; //开单网点
                ReceiptConditions += myGridView1.GetRowCellValue(i, "receiptInfo").ToString() + "@"; //回单要求
                ReceiptPays += (myGridView1.GetRowCellValue(i, "ReceiptPay").ToString() == "" ? "0" : myGridView1.GetRowCellValue(i, "ReceiptPay").ToString()) + "@"; //回单费 
                DeliFees += (myGridView1.GetRowCellValue(i, "DeliFee").ToString() == "" ? "0" : myGridView1.GetRowCellValue(i, "DeliFee").ToString()) + "@"; //送货费 
                ReceivFees += (myGridView1.GetRowCellValue(i, "ReceivFee").ToString() == "" ? "0" : myGridView1.GetRowCellValue(i, "ReceivFee").ToString()) + "@"; //接货费
                PaymentModes += myGridView1.GetRowCellValue(i, "PaymentMode").ToString() + "@"; //付款方式

                SupportValue += (myGridView1.GetRowCellValue(i, "SupportValue").ToString() == "" ? "0" : myGridView1.GetRowCellValue(i, "SupportValue").ToString()) + "@";// Convert.ToDecimal(myGridView1.GetRowCellValue(i, "SupportValue").ToString()) + "@"; //保价费 是否保价IsSupportValue
                DeclareValue += (myGridView1.GetRowCellValue(i, "DeclareValue").ToString() == "" ? "0" : myGridView1.GetRowCellValue(i, "DeclareValue").ToString()) + "@";// Convert.ToDecimal(myGridView1.GetRowCellValue(i, "DeclareValue").ToString()) + "@";  //声明价值
                HandleFee += (myGridView1.GetRowCellValue(i, "HandleFee").ToString() == "" ? "0" : myGridView1.GetRowCellValue(i, "HandleFee").ToString()) + "@";// Convert.ToDecimal(myGridView1.GetRowCellValue(i, "HandleFee").ToString()) + "@";  //装卸费 
                MatPay += (myGridView1.GetRowCellValue(i, "MatPay").ToString() == "" ? "0" : myGridView1.GetRowCellValue(i, "MatPay").ToString()) + "@";//Convert.ToDecimal(myGridView1.GetRowCellValue(i, "MatPay").ToString()) + "@";
                UpstairFee += (myGridView1.GetRowCellValue(i, "UpstairFee").ToString() == "" ? "0" : myGridView1.GetRowCellValue(i, "UpstairFee").ToString()) + "@";//Convert.ToDecimal(myGridView1.GetRowCellValue(i, "UpstairFee").ToString()) + "@";  //上楼费
                OtherFee += (myGridView1.GetRowCellValue(i, "OtherFee").ToString() == "" ? "0" : myGridView1.GetRowCellValue(i, "OtherFee").ToString()) + "@";//Convert.ToDecimal(myGridView1.GetRowCellValue(i, "OtherFee").ToString()) + "@"; //其它费

                //SupportValue += Convert.ToDecimal(myGridView1.GetRowCellValue(i, "SupportValue").ToString()) + "@"; //保价费 是否保价IsSupportValue
                //DeclareValue += Convert.ToDecimal(myGridView1.GetRowCellValue(i, "DeclareValue").ToString()) + "@";  //声明价值
                //HandleFee += Convert.ToDecimal(myGridView1.GetRowCellValue(i, "HandleFee").ToString()) + "@";  //装卸费 
                //MatPay += Convert.ToDecimal(myGridView1.GetRowCellValue(i, "MatPay").ToString()) + "@";
                //UpstairFee += Convert.ToDecimal(myGridView1.GetRowCellValue(i, "UpstairFee").ToString()) + "@";  //上楼费
                //OtherFee += Convert.ToDecimal(myGridView1.GetRowCellValue(i, "OtherFee").ToString()) + "@"; //其它费

                #region 基础信息
                areaName += CommonClass.UserInfo.AreaName + "@";
                causeName += CommonClass.UserInfo.CauseName + "@";
                depName += CommonClass.UserInfo.DepartName + "@";
                SiteName += CommonClass.UserInfo.SiteName + "@";
                WebName += CommonClass.UserInfo.WebName + "@";
                userName += CommonClass.UserInfo.UserName + "@";
                #endregion
            }

            //GetBillNo(BillNos);
            //if (billnum == 1) return;
            List<SqlPara> list = new List<SqlPara>(); //orderSn
            list.Add(new SqlPara("orderSn", orderSn)); //订单
            list.Add(new SqlPara("billSn", BillNos)); //运单
            list.Add(new SqlPara("orderTime", BillDates)); //开单时间
            list.Add(new SqlPara("TransferMode", TransferModes)); //交接方式
            list.Add(new SqlPara("packing", Packages)); //包装
            list.Add(new SqlPara("productName", Varietiess)); //品名
            list.Add(new SqlPara("qty", Nums)); //件数
            list.Add(new SqlPara("sendMan", ConsignorNames)); //发货人
            list.Add(new SqlPara("sendAddress", ConsignorCompanys)); //发货公司
            list.Add(new SqlPara("sendPhone", ConsignorCellPhones));//发货电话
            list.Add(new SqlPara("receiveMan", ConsigneeNames));//收货人
            list.Add(new SqlPara("receivePhone", ConsigneeCellPhones)); //收货人手机
            list.Add(new SqlPara("receiveCompany", ConsigneeCompanys)); //收货人公司
            list.Add(new SqlPara("receiveAddress", ReceivAddresss));//收货详细地址

            list.Add(new SqlPara("receiveProvince", ReceivProvinces));//收货省
            list.Add(new SqlPara("receiveCity", ReceivCitys));//收货市
            list.Add(new SqlPara("receiveArea", ReceivAreas)); //收货区
            list.Add(new SqlPara("Weights", Weights)); //重量
            list.Add(new SqlPara("Volumes", Volumes)); //体积

            list.Add(new SqlPara("collectionAmount", CollectionPays)); //代收货款
            list.Add(new SqlPara("AgentFee", AgentFee)); //代收货款手续费
            list.Add(new SqlPara("Freight", Freights)); //基本运费
            list.Add(new SqlPara("PaymentAmout", PaymentAmouts)); //总运费合计
            list.Add(new SqlPara("OptMan", BillMans)); //开单人
            list.Add(new SqlPara("OptTime", OptTimes)); //开单时间
            list.Add(new SqlPara("remarks", BillRemarks)); //备注
            list.Add(new SqlPara("GoodNo", GoodNos)); //货物批号
            list.Add(new SqlPara("GoodState", GoodStates)); //货物状态
            list.Add(new SqlPara("BegWeb", BegWebs)); //开单网点
            list.Add(new SqlPara("receiptInfo", ReceiptConditions)); //回单要求
            list.Add(new SqlPara("ReceiptPay", ReceiptPays)); //回单费
            list.Add(new SqlPara("DeliFee", DeliFees)); //送货费
            list.Add(new SqlPara("ReceivFee", ReceivFees)); //接货费
            list.Add(new SqlPara("PaymentMode", PaymentModes)); //付款方式
            list.Add(new SqlPara("SupportValue", SupportValue)); //保价费
            list.Add(new SqlPara("DeclareValue", DeclareValue)); //声明价值
            list.Add(new SqlPara("HandleFee", HandleFee)); //装卸费
            list.Add(new SqlPara("MatPay", MatPay)); //垫付费
            list.Add(new SqlPara("UpstairFee", UpstairFee)); //上楼费
            list.Add(new SqlPara("OtherFee", OtherFee)); //其它费 
            list.Add(new SqlPara("causeName", causeName)); //事业部 
            list.Add(new SqlPara("areaName", areaName)); //大区
            list.Add(new SqlPara("depName", depName)); //部门 
            list.Add(new SqlPara("SiteName", SiteName)); //站点 
            list.Add(new SqlPara("WebName", WebName)); //网点 
            list.Add(new SqlPara("userName", userName)); //登录人 

            SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "USP_ADD_WAYBILLWL_UPLOAD", list);//USP_ADD_WAYBILL_UPLOAD_TX_485
            if (SqlHelper.ExecteNonQuery(spe) > 0)
            {
                MsgBox.ShowOK();
                ds.Clear();
            }
        }

        public class word
        {
            public string name;
            public string path;
        }
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            List<word> list = new List<word>();
            word wd = new word();
            wd.name = "订单模板";
            wd.path = "http://8.129.7.49/订单模板.xls"; //
            list.Add(wd);
            Download(list);
        }

        public static void Download(List<word> list)
        {
            try
            {

                WebClient client = new WebClient();
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Microsoft Word文件(*.doc;*.docx)|*.doc;*.docx";//定义文件格式  
                saveFileDialog.FilterIndex = 2;
                saveFileDialog.RestoreDirectory = true;//保存对话框是否记忆上次打开的目录  
                saveFileDialog.FileName = "订单模板.xls";
                saveFileDialog.Title = "导出Excel模板文件到";
                //点了保存按钮进入  
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        //saveFileDialog.FileName = "matternam"+i;
                        string URL = list[i].path;
                        string localFilePath = System.IO.Path.GetFullPath(saveFileDialog.FileName);//获得文件路径，含文件名  
                        string fileNameExt = localFilePath.Substring(localFilePath.LastIndexOf("\\") + 1);//获取文件名，不带路径  
                        string FilePath = localFilePath.Substring(0, localFilePath.LastIndexOf("\\"));//获取文件路径，不带文件名  
                        //string newFileName = DateTime.Now.ToString("yyyyMMdd") + fileNameExt; //给文件名前加上时间  
                        //saveFileDialog.FileName.Insert(1, "dameng");//在文件名里加字符  
                        localFilePath = FilePath + "\\" + list[i].name + ".xls";
                        client.DownloadFile(URL, localFilePath);
                    }
                    MessageBox.Show("已下载完成！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK("文件下载异常：\r\n" + ex.Message);
            }
        }
    }
}