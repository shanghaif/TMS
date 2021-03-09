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
    public partial class frmWayBillRecordZQUP : BaseForm
    {
        DataSet dsNew = new DataSet();
        DataSet ds = new DataSet();
        int b = 0, billnum = 0;

        string BillNos = string.Empty;
        string BillDates = string.Empty;
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
        
        public frmWayBillRecordZQUP()
        {
            InitializeComponent();
        }

        private void frmWayBillRecordZQUP_Load(object sender, EventArgs e)
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
                c["运单号"].ColumnName = "BillNo"; //2020-11-24 运单号启用  有运单号取已有运单号，没有从运单号库中生成
                c["业务时间"].ColumnName = "BillDate"; //业务时间取BillDate这个值，开单日期取当前时间
                c["始发站"].ColumnName = "StartSite"; //2020-11-24 短线停用
                c["交接方式"].ColumnName = "TransferMode"; //2020-11-24 等同于中强的送货方式
                c["到站"].ColumnName = "DestinationSite";  //2020-11-24 短线停用
                c["中转地"].ColumnName = "TransferSite"; //2020-11-24 短线停用
                c["运输方式"].ColumnName = "TransitMode";
                c["客户单号"].ColumnName = "CusOderNo";
                c["发货公司名称"].ColumnName = "ConsignorCompany"; //2020-11-24 等同于发货地址
                c["发货联系人"].ColumnName = "ConsignorName"; //2020-11-24  发货联系人
                c["发货人手机"].ColumnName = "ConsignorCellPhone"; //2020-11-24  发货人手机

                c["收货人手机"].ColumnName = "ConsigneeCellPhone";
                c["收货联系人"].ColumnName = "ConsigneeName";
                c["收货公司名称"].ColumnName = "ConsigneeCompany"; //2020-11-24 等同于收货地址 此处同下
                c["收货详细地址"].ColumnName = "ReceivAddress"; //2020-11-24  等同于收货地址
                c["目的网点"].ColumnName = "PickGoodsSite";
                c["收货省"].ColumnName = "ReceivProvince";
                c["收货市"].ColumnName = "ReceivCity";
                c["收货区"].ColumnName = "ReceivArea";

                c["包装"].ColumnName = "Package"; //2020-11-24 
                c["品名"].ColumnName = "Varieties";
                c["件数"].ColumnName = "Num";

                //c["计费重量"].ColumnName = "FeeWeight";
                //c["计费体积"].ColumnName = "FeeVolume";
                c["重量"].ColumnName = "Weight";
                c["体积"].ColumnName = "Volume";

                c["基本运费"].ColumnName = "Freight";
                c["开单送货费"].ColumnName = "DeliFee";   //2020-11-24 等同于送货费
                c["接货费"].ColumnName = "ReceivFee";  //2020-11-24  等同于提货费
                //c["折扣折让"].ColumnName = "DiscountTransfer";
                c["代收货款"].ColumnName = "CollectionPay";
                c["付款方式"].ColumnName = "PaymentMode";
                //c["总运费"].ColumnName = "PaymentAmout";
                //c["现付"].ColumnName = "NowPay";
                //c["提付"].ColumnName = "FetchPay";
                c["月结"].ColumnName = "MonthPay";
                c["回单付"].ColumnName = "ReceiptPay";
                c["开单人"].ColumnName = "BillMan";
                c["开单网点"].ColumnName = "BegWeb";  //2020-11-24  默认中强分拨中心
                c["公司ID"].ColumnName = "companyid"; 
                //c["运单状态"].ColumnName = "BillState";
                c["回单要求"].ColumnName = "ReceiptCondition";  //2020-11-24  回单要求 可选择要几份回单等同于要求回单+数量
                c["备注"].ColumnName = "BillRemark"; //2020-11-24  备注信息
                //c["业务员"].ColumnName = "Salesman";

                /* //2020-11-24  新增模板列 */
                c["保价费"].ColumnName = "SupportValue";//保价费 是否保价IsSupportValue
                c["声明价值"].ColumnName = "DeclareValue";//声明价值
                c["装卸费"].ColumnName = "HandleFee";//装卸费 是否装卸 IsHandleFee
                c["垫付费"].ColumnName = "MatPay";//垫付费
                c["代收货款手续费"].ColumnName = "AgentFee";//手续费（代收货款手续费)
                c["上楼费"].ColumnName = "UpstairFee";//上楼费 是否上楼 IsUpstairFee
                c["其它费"].ColumnName = "OtherFee";//其它费  其它费合计OtherTotalFee  是否收其它费IsOtherFee

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

            //只有101的账号可以导入
            //if (CommonClass.UserInfo.companyid != "101" && CommonClass.UserInfo.companyid != "485" && CommonClass.UserInfo.companyid != "486")
            //{
            //    MsgBox.ShowOK("您不能使用此功能！");
            //    return;
            //}

            for (int j = 0; j < myGridView1.RowCount; j++)
            {   
                System.Threading.Thread.Sleep(300);
                if (myGridView1.RowCount >= 250)
                {
                    if (myGridView2.RowCount % 50 >= 0)
                    {
                        DataTable table = ds.Tables[0];
                        dsNew = ds.Clone();
                        DataTable table2 = dsNew.Tables[0];
                        //table2.Rows.Clear();
                        for (int k = 0; k < 250; k++)
                        {
                            table2.Rows.Add(table.Rows[k].ItemArray);
                        }
                        table2.AcceptChanges();
                        myGridControl2.DataSource = dsNew.Tables[0];

                        for (int i = 0; i < myGridView2.RowCount; i++)
                        {
                            BillNos += myGridView2.GetRowCellValue(i, "BillNo").ToString() + "@";
                            BillDates += myGridView2.GetRowCellValue(i, "BillDate").ToString() + "@";
                            StartSites += myGridView2.GetRowCellValue(i, "StartSite").ToString() + "@";
                            TransferModes += myGridView2.GetRowCellValue(i, "TransferMode").ToString() + "@";
                            DestinationSites += myGridView2.GetRowCellValue(i, "DestinationSite").ToString() + "@";
                            TransferSites += myGridView2.GetRowCellValue(i, "TransferSite").ToString() + "@";
                            //TransitLiness += myGridView2.GetRowCellValue(i, "TransitLines").ToString() + "@";  //hj20190118注释
                            TransitModes += myGridView2.GetRowCellValue(i, "TransitMode").ToString() + "@";
                            CusOderNos += myGridView2.GetRowCellValue(i, "CusOderNo").ToString() + "@";
                            Packages += myGridView2.GetRowCellValue(i, "Package").ToString() + "@";//hj20190118
                            ConsignorCompanys += myGridView2.GetRowCellValue(i, "ConsignorCompany").ToString() + "@";//hj20190118
                            ConsigneeCellPhones += myGridView2.GetRowCellValue(i, "ConsigneeCellPhone").ToString() + "@";
                            ConsigneeNames += myGridView2.GetRowCellValue(i, "ConsigneeName").ToString() + "@";
                            ConsigneeCompanys += myGridView2.GetRowCellValue(i, "ConsigneeCompany").ToString() + "@";//hj20190118
                            ReceivAddresss += myGridView2.GetRowCellValue(i, "ReceivAddress").ToString() + "@";//hj20190118
                            PickGoodsSites += myGridView2.GetRowCellValue(i, "PickGoodsSite").ToString() + "@";
                            ReceivProvinces += myGridView2.GetRowCellValue(i, "ReceivProvince").ToString() + "@";
                            ReceivCitys += myGridView2.GetRowCellValue(i, "ReceivCity").ToString() + "@";
                            ReceivAreas += myGridView2.GetRowCellValue(i, "ReceivArea").ToString() + "@";
                            ConsignorNames += myGridView2.GetRowCellValue(i, "ConsignorName").ToString() + "@";
                            ConsignorCellPhones += myGridView2.GetRowCellValue(i, "ConsignorCellPhone").ToString() + "@";
                            Varietiess += myGridView2.GetRowCellValue(i, "Varieties").ToString() + "@";
                            Nums += myGridView2.GetRowCellValue(i, "Num").ToString() + "@";
                            FeeWeights += myGridView2.GetRowCellValue(i, "Weight").ToString() + "@";  // myGridView2.GetRowCellValue(i, "FeeWeight").ToString() + "@";
                            FeeVolumes += myGridView2.GetRowCellValue(i, "Weight").ToString() + "@"; // myGridView2.GetRowCellValue(i, "FeeVolume").ToString() + "@";
                            Weights += myGridView2.GetRowCellValue(i, "Weight").ToString() + "@";
                            Volumes += myGridView2.GetRowCellValue(i, "Volume").ToString() + "@";
                            Freights += myGridView2.GetRowCellValue(i, "Freight").ToString() + "@";
                            DeliFees += (myGridView2.GetRowCellValue(i, "DeliFee").ToString() == "" ? "0" : myGridView2.GetRowCellValue(i, "DeliFee").ToString()) + "@";
                            ReceivFees += (myGridView2.GetRowCellValue(i, "ReceivFee").ToString() == "" ? "0" : myGridView2.GetRowCellValue(i, "ReceivFee").ToString()) + "@";
                            DiscountTransfers += "0@"; //(myGridView2.GetRowCellValue(i, "DiscountTransfer").ToString() == "" ? "0" : myGridView2.GetRowCellValue(i, "DiscountTransfer").ToString()) + "@";
                            CollectionPays += (myGridView2.GetRowCellValue(i, "CollectionPay").ToString() == "" ? "0" : myGridView2.GetRowCellValue(i, "CollectionPay").ToString()) + "@";
                            PaymentModes += myGridView2.GetRowCellValue(i, "PaymentMode").ToString() + "@";
                            PaymentAmouts += "0@"; //(myGridView2.GetRowCellValue(i, "PaymentAmout").ToString() == "" ? "0" : myGridView2.GetRowCellValue(i, "PaymentAmout").ToString()) + "@";
                            NowPays += "0@"; // (myGridView2.GetRowCellValue(i, "NowPay").ToString() == "" ? "0" : myGridView2.GetRowCellValue(i, "NowPay").ToString()) + "@";
                            FetchPays += "0@";//(myGridView2.GetRowCellValue(i, "FetchPay").ToString() == "" ? "0" : myGridView2.GetRowCellValue(i, "FetchPay").ToString()) + "@";
                            MonthPays += (myGridView2.GetRowCellValue(i, "MonthPay").ToString() == "" ? "0" : myGridView2.GetRowCellValue(i, "MonthPay").ToString()) + "@";
                            ReceiptPays += (myGridView2.GetRowCellValue(i, "ReceiptPay").ToString() == "" ? "0" : myGridView2.GetRowCellValue(i, "ReceiptPay").ToString()) + "@";
                            BillMans += myGridView2.GetRowCellValue(i, "BillMan").ToString() + "@";
                            BegWebs += myGridView2.GetRowCellValue(i, "BegWeb").ToString() + "@";
                            ActualFreights += Convert.ToDecimal(myGridView2.GetRowCellValue(i, "CollectionPay").ToString() == "" ? "0" : myGridView2.GetRowCellValue(i, "CollectionPay").ToString()) + "@";//(Convert.ToDecimal(myGridView2.GetRowCellValue(i, "PaymentAmout")) - Convert.ToDecimal(myGridView2.GetRowCellValue(i, "DiscountTransfer")) - Convert.ToDecimal(myGridView2.GetRowCellValue(i, "CollectionPay"))) + "@";
                            companyids += myGridView2.GetRowCellValue(i, "companyid").ToString() + "@";
                            BillStates += "0@"; // myGridView2.GetRowCellValue(i, "BillState").ToString() + "@";
                            ReceiptConditions += myGridView2.GetRowCellValue(i, "ReceiptCondition").ToString() + "@";
                            BillRemarks += myGridView2.GetRowCellValue(i, "BillRemark").ToString() + "@";
                            Salesmans += "@"; //myGridView2.GetRowCellValue(i, "Salesman").ToString() + "@";

                            /* //2020-11-24  新增模板列 */
                            if (!string.IsNullOrEmpty(myGridView2.GetRowCellValue(i, "SupportValue").ToString()))
                            {
                                IsSupportValue += "1@";
                                SupportValue += Convert.ToDecimal(myGridView2.GetRowCellValue(i, "SupportValue").ToString()) + "@"; //保价费 是否保价IsSupportValue
                                DeclareValue += Convert.ToDecimal(myGridView2.GetRowCellValue(i, "DeclareValue").ToString()) + "@";  //声明价值
                            }
                            else
                            {
                                IsSupportValue += "0@";
                                SupportValue += Convert.ToDecimal(0) + "@"; //保价费 是否保价IsSupportValue
                                DeclareValue += Convert.ToDecimal(0) + "@";  //声明价值
                            }


                            if (!string.IsNullOrEmpty(myGridView2.GetRowCellValue(i, "HandleFee").ToString()))
                            {
                                IsHandleFee += "1@";
                                HandleFee += Convert.ToDecimal(myGridView2.GetRowCellValue(i, "HandleFee").ToString()) + "@";  //装卸费 是否装卸 IsHandleFee
                            }
                            else
                            {
                                IsHandleFee += "0@";
                                HandleFee += Convert.ToDecimal(0) + "@";  //装卸费 是否装卸 IsHandleFee
                            }


                            if (!string.IsNullOrEmpty(myGridView2.GetRowCellValue(i, "MatPay").ToString()))
                            {
                                IsMatPay += "1@";
                                MatPay += Convert.ToDecimal(myGridView2.GetRowCellValue(i, "MatPay").ToString()) + "@"; //垫付费 是否垫付 IsMatPay
                            }
                            else
                            {
                                IsMatPay += "0@";
                                MatPay += Convert.ToDecimal(0) + "@"; //垫付费 是否垫付 IsMatPay
                            }

                            if (!string.IsNullOrEmpty(myGridView2.GetRowCellValue(i, "AgentFee").ToString()))
                            {
                                AgentFee += Convert.ToDecimal(myGridView2.GetRowCellValue(i, "AgentFee").ToString()) + "@";  //手续费（代收货款手续费)                    
                            }
                            else
                            {
                                AgentFee += Convert.ToDecimal(0) + "@";  //手续费（代收货款手续费)  
                            }


                            if (!string.IsNullOrEmpty(myGridView2.GetRowCellValue(i, "UpstairFee").ToString()))
                            {
                                IsUpstairFee += "1@";
                                UpstairFee += Convert.ToDecimal(myGridView2.GetRowCellValue(i, "UpstairFee").ToString()) + "@";  //上楼费 是否上楼 IsUpstairFee
                            }
                            else
                            {
                                IsUpstairFee += "0@";
                                UpstairFee += Convert.ToDecimal(0) + "@";  //上楼费 是否上楼 IsUpstairFee
                            }


                            if (!string.IsNullOrEmpty(myGridView2.GetRowCellValue(i, "OtherFee").ToString()))
                            {
                                IsOtherFee += "1@";
                                OtherFee += Convert.ToDecimal(myGridView2.GetRowCellValue(i, "OtherFee").ToString()) + "@";  //其它费  其它费合计OtherTotalFee  是否收其它费IsOtherFee
                            }
                            else
                            {
                                IsOtherFee += "0@";
                                OtherFee += Convert.ToDecimal(0) + "@";
                            }

                            OperationWeights += "0@";
                            //decimal w=Convert.ToDecimal(myGridView2.GetRowCellValue(i, "FeeWeight").ToString());
                            //decimal v=Convert.ToDecimal(myGridView2.GetRowCellValue(i, "FeeVolume").ToString());
                            //结算重量
                            //if (myGridView2.GetRowCellValue(i, "Package").ToString() != "纸箱" && myGridView2.GetRowCellValue(i, "Package").ToString() != "纤袋" && myGridView2.GetRowCellValue(i, "Package").ToString() != "膜")
                            //{
                            //    OperationWeights += Math.Max(w, v / (decimal)3.8 * 1000) * (decimal)1.05 + "@";
                            //}
                            //else
                            //{
                            //    OperationWeights += Math.Max(w, v / (decimal)3.8 * 1000)  + "@";
                            //}

                            if (myGridView2.GetRowCellValue(i, "companyid").ToString() == "")
                            {
                                MsgBox.ShowOK("存在公司ID为空的数据，请检查！！！");
                                return;
                            }
                            if (myGridView2.GetRowCellValue(i, "BillNo").ToString() == "")
                            {
                                MsgBox.ShowOK("存在运单号为空的数据，请检查！！！");
                                return;
                            }
                            if (myGridView2.GetRowCellValue(i, "BillDate").ToString() == "")
                            {
                                MsgBox.ShowOK("存在开单时间为空的数据，请检查！！！");
                                return;
                            }
                            //if (myGridView2.GetRowCellValue(i, "StartSite").ToString() == "")
                            //{
                            //    MsgBox.ShowOK("存在始发站为空的数据，请检查！！！");
                            //    return;
                            //}
                            if (myGridView2.GetRowCellValue(i, "TransferMode").ToString() == "")
                            {
                                MsgBox.ShowOK("存在交接方式为空的数据，请检查！！！");
                                return;
                            }
                            //if (myGridView2.GetRowCellValue(i, "DestinationSite").ToString() == "")
                            //{
                            //    MsgBox.ShowOK("存在到站为空的数据，请检查！！！");
                            //    return;
                            //}
                            //if (myGridView2.GetRowCellValue(i, "TransferSite").ToString() == "")
                            //{
                            //    MsgBox.ShowOK("存在中转地为空的数据，请检查！！！");
                            //    return;
                            //}
                            //if (myGridView2.GetRowCellValue(i, "TransitLines").ToString() == "")
                            //{
                            //    MsgBox.ShowOK("存在运输线路为空的数据，请检查！！！");
                            //    return;
                            //}
                            if (myGridView2.GetRowCellValue(i, "TransitMode").ToString() == "")
                            {
                                MsgBox.ShowOK("存在运输方式为空的数据，请检查！！！");
                                return;
                            }
                            if (myGridView2.GetRowCellValue(i, "ConsigneeCellPhone").ToString() == "")
                            {
                                MsgBox.ShowOK("存在收货人手机为空的数据，请检查！！！");
                                return;
                            }
                            if (myGridView2.GetRowCellValue(i, "ConsigneeName").ToString() == "")
                            {
                                MsgBox.ShowOK("存在收货联系人为空的数据，请检查！！！");
                                return;
                            }
                            //if (myGridView2.GetRowCellValue(i, "PickGoodsSite").ToString() == "")
                            //{
                            //    MsgBox.ShowOK("存在目的网点为空的数据，请检查！！！");
                            //    return;
                            //}
                            if (myGridView2.GetRowCellValue(i, "ReceivProvince").ToString() == "")
                            {
                                MsgBox.ShowOK("存在收货省为空的数据，请检查！！！");
                                return;
                            }
                            if (myGridView2.GetRowCellValue(i, "ReceivCity").ToString() == "")
                            {
                                MsgBox.ShowOK("存在收货市为空的数据，请检查！！！");
                                return;
                            }
                            if (myGridView2.GetRowCellValue(i, "ConsignorName").ToString() == "")
                            {
                                MsgBox.ShowOK("存在发货联系人为空的数据，请检查！！！");
                                return;
                            }
                            if (myGridView2.GetRowCellValue(i, "ConsignorCellPhone").ToString() == "")
                            {
                                MsgBox.ShowOK("存在发货人手机为空的数据，请检查！！！");
                                return;
                            }
                            if (myGridView2.GetRowCellValue(i, "Varieties").ToString() == "")
                            {
                                MsgBox.ShowOK("存在品名为空的数据，请检查！！！");
                                return;
                            }
                            if (myGridView2.GetRowCellValue(i, "Num").ToString() == "")
                            {
                                MsgBox.ShowOK("存在件数为空的数据，请检查！！！");
                                return;
                            }
                            //if (ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "FeeWeight").ToString()) <= 0)
                            //{
                            //    MsgBox.ShowOK("计费重量不能小于等于0，请检查！！！");
                            //    return;
                            //}
                            //if (ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "FeeVolume").ToString()) <= 0)
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
                            string a = myGridView2.GetRowCellValue(i, "BegWeb").ToString();
                            string c = myGridView2.GetRowCellValue(i, "companyid").ToString();
                            getdata(a, c);
                        }
                        //GetBillNo(BillNos);
                        //if (billnum == 1) return;

                        List<SqlPara> list = new List<SqlPara>();
                        list.Add(new SqlPara("BillNos", BillNos));
                        list.Add(new SqlPara("BillDates", BillDates));
                        list.Add(new SqlPara("StartSites", StartSites));
                        list.Add(new SqlPara("TransferModes", TransferModes));
                        list.Add(new SqlPara("DestinationSites", DestinationSites));
                        list.Add(new SqlPara("TransferSites", TransferSites));
                        //list.Add(new SqlPara("TransitLiness", TransitLiness));
                        list.Add(new SqlPara("TransitModes", TransitModes));
                        list.Add(new SqlPara("CusOderNos", CusOderNos));
                        list.Add(new SqlPara("Packages", Packages));//hj20190118
                        list.Add(new SqlPara("ConsignorCompanys", ConsignorCompanys));//hj20190118
                        list.Add(new SqlPara("ConsigneeCellPhones", ConsigneeCellPhones));
                        list.Add(new SqlPara("ConsigneeNames", ConsigneeNames));
                        list.Add(new SqlPara("ConsigneeCompanys", ConsigneeCompanys));//hj20190118
                        list.Add(new SqlPara("ReceivAddresss", ReceivAddresss));//hj20190118
                        list.Add(new SqlPara("PickGoodsSites", PickGoodsSites));
                        list.Add(new SqlPara("ReceivProvinces", ReceivProvinces));
                        list.Add(new SqlPara("ReceivCitys", ReceivCitys));
                        list.Add(new SqlPara("ReceivAreas", ReceivAreas));
                        list.Add(new SqlPara("ConsignorNames", ConsignorNames));
                        list.Add(new SqlPara("ConsignorCellPhones", ConsignorCellPhones));
                        list.Add(new SqlPara("Varietiess", Varietiess));
                        list.Add(new SqlPara("Nums", Nums));
                        list.Add(new SqlPara("FeeWeights", FeeWeights));
                        list.Add(new SqlPara("FeeVolumes", FeeVolumes));
                        list.Add(new SqlPara("Weights", Weights));
                        list.Add(new SqlPara("Volumes", Volumes));
                        list.Add(new SqlPara("Freights", Freights));
                        list.Add(new SqlPara("DeliFees", DeliFees));
                        list.Add(new SqlPara("ReceivFees", ReceivFees));
                        list.Add(new SqlPara("DiscountTransfers", DiscountTransfers));
                        list.Add(new SqlPara("CollectionPays", CollectionPays));
                        list.Add(new SqlPara("PaymentModes", PaymentModes));
                        list.Add(new SqlPara("PaymentAmouts", PaymentAmouts));
                        list.Add(new SqlPara("NowPays", NowPays));
                        list.Add(new SqlPara("FetchPays", FetchPays));
                        list.Add(new SqlPara("MonthPays", MonthPays));
                        list.Add(new SqlPara("ReceiptPays", ReceiptPays));
                        list.Add(new SqlPara("BillMans", BillMans));
                        list.Add(new SqlPara("BegWebs", BegWebs));
                        list.Add(new SqlPara("ActualFreights", ActualFreights));
                        list.Add(new SqlPara("companyids", companyids));
                        list.Add(new SqlPara("BillStates", BillStates));
                        list.Add(new SqlPara("ReceiptConditions", ReceiptConditions));
                        list.Add(new SqlPara("BillRemarks", BillRemarks));
                        list.Add(new SqlPara("Salesmans", Salesmans));
                        list.Add(new SqlPara("OperationWeights", OperationWeights));
                        /* //2020-11-24  新增模板列 */
                        list.Add(new SqlPara("SupportValue",SupportValue));
                        list.Add(new SqlPara("IsSupportValue", IsSupportValue));
                        list.Add(new SqlPara("DeclareValue", DeclareValue));
                        list.Add(new SqlPara("HandleFee", HandleFee));
                        list.Add(new SqlPara("IsHandleFee", IsHandleFee));
                        list.Add(new SqlPara("MatPay", MatPay));
                        list.Add(new SqlPara("IsMatPay", IsMatPay));
                        list.Add(new SqlPara("UpstairFee", UpstairFee));
                        list.Add(new SqlPara("IsUpstairFee", IsUpstairFee));
                        list.Add(new SqlPara("OtherFee", OtherFee));
                        list.Add(new SqlPara("IsOtherFee", IsOtherFee));
                        list.Add(new SqlPara("AgentFee", AgentFee)); 
                         

                        SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "USP_ADD_WAYBILL_UPLOAD", list);
                        if (SqlHelper.ExecteNonQuery(spe) > 0)
                        {
                            MsgBox.ShowOK();
                            dsNew.Clear();
                        }

                        if (dsNew.Tables[0].Rows.Count == 0)
                        {
                            for (int m = 0; m < 250; m++)
                            {
                                ds.Tables[0].Rows[m].Delete();
                            }
                            ds.AcceptChanges();
                        }
                        if (ds.Tables[0].Rows.Count == 0 || myGridView1.RowCount == 0) return;
                        if (myGridView1.RowCount < 250 && myGridView1.RowCount > 0)
                        {
                            updata();
                            return;
                        }


                    }
                }
                if (myGridView1.RowCount < 250 && myGridView1.RowCount > 0)
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

            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                BillNos += myGridView1.GetRowCellValue(i, "BillNo").ToString() + "@";
                BillDates += myGridView1.GetRowCellValue(i, "BillDate").ToString() + "@";
                StartSites += myGridView1.GetRowCellValue(i, "StartSite").ToString() + "@";
                TransferModes += myGridView1.GetRowCellValue(i, "TransferMode").ToString() + "@";
                DestinationSites += myGridView1.GetRowCellValue(i, "DestinationSite").ToString() + "@";
                TransferSites += myGridView1.GetRowCellValue(i, "TransferSite").ToString() + "@";
                //TransitLiness += myGridView1.GetRowCellValue(i, "TransitLines").ToString() + "@";  //hj20190118注释
                TransitModes += myGridView1.GetRowCellValue(i, "TransitMode").ToString() + "@";
                CusOderNos += myGridView1.GetRowCellValue(i, "CusOderNo").ToString() + "@";
                Packages += myGridView1.GetRowCellValue(i, "Package").ToString() + "@";//hj20190118
                ConsignorCompanys += myGridView1.GetRowCellValue(i, "ConsignorCompany").ToString() + "@";//hj20190118
                ConsigneeCellPhones += myGridView1.GetRowCellValue(i, "ConsigneeCellPhone").ToString() + "@";
                ConsigneeNames += myGridView1.GetRowCellValue(i, "ConsigneeName").ToString() + "@";
                ConsigneeCompanys += myGridView1.GetRowCellValue(i, "ConsigneeCompany").ToString() + "@";//hj20190118
                ReceivAddresss += myGridView1.GetRowCellValue(i, "ReceivAddress").ToString() + "@";//hj20190118
                PickGoodsSites += myGridView1.GetRowCellValue(i, "PickGoodsSite").ToString() + "@";
                ReceivProvinces += myGridView1.GetRowCellValue(i, "ReceivProvince").ToString() + "@";
                ReceivCitys += myGridView1.GetRowCellValue(i, "ReceivCity").ToString() + "@";
                ReceivAreas += myGridView1.GetRowCellValue(i, "ReceivArea").ToString() + "@";
                ConsignorNames += myGridView1.GetRowCellValue(i, "ConsignorName").ToString() + "@";
                ConsignorCellPhones += myGridView1.GetRowCellValue(i, "ConsignorCellPhone").ToString() + "@";
                Varietiess += myGridView1.GetRowCellValue(i, "Varieties").ToString() + "@";
                Nums += myGridView1.GetRowCellValue(i, "Num").ToString() + "@";
                FeeWeights += myGridView1.GetRowCellValue(i, "Weight").ToString() + "@"; // myGridView1.GetRowCellValue(i, "FeeWeight").ToString() + "@";
                FeeVolumes += myGridView1.GetRowCellValue(i, "Volume").ToString() + "@"; // myGridView1.GetRowCellValue(i, "FeeVolume").ToString() + "@";
                Weights += myGridView1.GetRowCellValue(i, "Weight").ToString() + "@";
                Volumes += myGridView1.GetRowCellValue(i, "Volume").ToString() + "@";
                Freights += myGridView1.GetRowCellValue(i, "Freight").ToString() + "@";
                DeliFees += (myGridView1.GetRowCellValue(i, "DeliFee").ToString() == "" ? "0" : myGridView1.GetRowCellValue(i, "DeliFee").ToString()) + "@";
                ReceivFees += (myGridView1.GetRowCellValue(i, "ReceivFee").ToString() == "" ? "0" : myGridView1.GetRowCellValue(i, "ReceivFee").ToString()) + "@";
                DiscountTransfers += "0@"; //(myGridView1.GetRowCellValue(i, "DiscountTransfer").ToString() == "" ? "0" : myGridView1.GetRowCellValue(i, "DiscountTransfer").ToString()) + "@";
                CollectionPays += (myGridView1.GetRowCellValue(i, "CollectionPay").ToString() == "" ? "0" : myGridView1.GetRowCellValue(i, "CollectionPay").ToString()) + "@";
                PaymentModes += myGridView1.GetRowCellValue(i, "PaymentMode").ToString() + "@";
                PaymentAmouts += "0@"; //(myGridView1.GetRowCellValue(i, "PaymentAmout").ToString() == "" ? "0" : myGridView1.GetRowCellValue(i, "PaymentAmout").ToString()) + "@";
                NowPays += "0@"; //(myGridView1.GetRowCellValue(i, "NowPay").ToString() == "" ? "0" : myGridView1.GetRowCellValue(i, "NowPay").ToString()) + "@";
                FetchPays += "0@";//(myGridView1.GetRowCellValue(i, "FetchPay").ToString() == "" ? "0" : myGridView1.GetRowCellValue(i, "FetchPay").ToString()) + "@";
                MonthPays += (myGridView1.GetRowCellValue(i, "MonthPay").ToString() == "" ? "0" : myGridView1.GetRowCellValue(i, "MonthPay").ToString()) + "@";
                ReceiptPays += (myGridView1.GetRowCellValue(i, "ReceiptPay").ToString() == "" ? "0" : myGridView1.GetRowCellValue(i, "ReceiptPay").ToString()) + "@";
                BillMans += myGridView1.GetRowCellValue(i, "BillMan").ToString() + "@";
                BegWebs += myGridView1.GetRowCellValue(i, "BegWeb").ToString() + "@";
                ActualFreights += Convert.ToDecimal(myGridView1.GetRowCellValue(i, "CollectionPay").ToString() == "" ? "0" : myGridView1.GetRowCellValue(i, "CollectionPay").ToString()) + "@";//(Convert.ToDecimal(myGridView1.GetRowCellValue(i, "PaymentAmout")) - Convert.ToDecimal(myGridView1.GetRowCellValue(i, "DiscountTransfer")) - Convert.ToDecimal(myGridView1.GetRowCellValue(i, "CollectionPay"))) + "@";
                companyids += myGridView1.GetRowCellValue(i, "companyid").ToString() + "@";
                BillStates += "0@"; // myGridView1.GetRowCellValue(i, "BillState").ToString() + "@";
                ReceiptConditions += myGridView1.GetRowCellValue(i, "ReceiptCondition").ToString() + "@";
                BillRemarks += myGridView1.GetRowCellValue(i, "BillRemark").ToString() + "@";
                Salesmans += " @"; //myGridView1.GetRowCellValue(i, "Salesman").ToString() + "@";
                /* //2020-11-24  新增模板列 */
                if (!string.IsNullOrEmpty(myGridView1.GetRowCellValue(i, "SupportValue").ToString()))
                {
                    IsSupportValue += "1@";
                    SupportValue += Convert.ToDecimal(myGridView1.GetRowCellValue(i, "SupportValue").ToString()) + "@"; //保价费 是否保价IsSupportValue
                    DeclareValue += Convert.ToDecimal(myGridView1.GetRowCellValue(i, "DeclareValue").ToString()) + "@";  //声明价值
                }
                else
                {
                    IsSupportValue += "0@";
                    SupportValue += Convert.ToDecimal(0) + "@"; //保价费 是否保价IsSupportValue
                    DeclareValue += Convert.ToDecimal(0) + "@";  //声明价值
                }
               

                if (!string.IsNullOrEmpty(myGridView1.GetRowCellValue(i, "HandleFee").ToString()))
                {
                    IsHandleFee += "1@";
                    HandleFee += Convert.ToDecimal(myGridView1.GetRowCellValue(i, "HandleFee").ToString()) + "@";  //装卸费 是否装卸 IsHandleFee
                }
                else
                {
                    IsHandleFee += "0@";
                    HandleFee += Convert.ToDecimal(0) + "@";  //装卸费 是否装卸 IsHandleFee
                }


                if (!string.IsNullOrEmpty(myGridView1.GetRowCellValue(i, "MatPay").ToString()))
                {
                    IsMatPay += "1@";
                    MatPay += Convert.ToDecimal(myGridView1.GetRowCellValue(i, "MatPay").ToString()) + "@"; //垫付费 是否垫付 IsMatPay
                }
                else
                {
                    IsMatPay += "0@";
                    MatPay += Convert.ToDecimal(0) + "@"; //垫付费 是否垫付 IsMatPay
                }

                if (!string.IsNullOrEmpty(myGridView1.GetRowCellValue(i, "AgentFee").ToString()))
                {
                    AgentFee += Convert.ToDecimal(myGridView1.GetRowCellValue(i, "AgentFee").ToString()) + "@";  //手续费（代收货款手续费)                    
                }
                else
                {
                    AgentFee += Convert.ToDecimal(0) + "@";  //手续费（代收货款手续费)  
                }


                if (!string.IsNullOrEmpty(myGridView1.GetRowCellValue(i, "UpstairFee").ToString()))
                {
                    IsUpstairFee += "1@";
                    UpstairFee += Convert.ToDecimal(myGridView1.GetRowCellValue(i, "UpstairFee").ToString()) + "@";  //上楼费 是否上楼 IsUpstairFee
                }
                else
                {
                    IsUpstairFee += "0@";
                    UpstairFee += Convert.ToDecimal(0) + "@";  //上楼费 是否上楼 IsUpstairFee
                }


                if (!string.IsNullOrEmpty(myGridView1.GetRowCellValue(i, "OtherFee").ToString()))
                {
                    IsOtherFee += "1@";
                    OtherFee += Convert.ToDecimal(myGridView1.GetRowCellValue(i, "OtherFee").ToString()) + "@";  //其它费  其它费合计OtherTotalFee  是否收其它费IsOtherFee
                }
                else
                {
                    IsOtherFee += "0@";
                    OtherFee += Convert.ToDecimal(0) + "@";
                }
                
                
                OperationWeights += "0@";
                //decimal w = Convert.ToDecimal(myGridView1.GetRowCellValue(i, "FeeWeight").ToString());
                //decimal v = Convert.ToDecimal(myGridView1.GetRowCellValue(i, "FeeVolume").ToString());
                ////结算重量
                //if (myGridView1.GetRowCellValue(i, "Package").ToString() != "纸箱" && myGridView1.GetRowCellValue(i, "Package").ToString() != "纤袋" && myGridView1.GetRowCellValue(i, "Package").ToString() != "膜")
                //{
                //    OperationWeights += Math.Max(w, v / (decimal)3.8 * 1000) * (decimal)1.05 + "@";
                //}
                //else
                //{
                //    OperationWeights += Math.Max(w, v / (decimal)3.8 * 1000) + "@";
                //}


                if (myGridView1.GetRowCellValue(i, "companyid").ToString() == "")
                {
                    MsgBox.ShowOK("存在公司ID为空的数据，请检查！！！");
                    return;
                }
                if (myGridView1.GetRowCellValue(i, "BillNo").ToString() == "")
                {
                    MsgBox.ShowOK("存在运单号为空的数据，请检查！！！");
                    return;
                }
                if (myGridView1.GetRowCellValue(i, "BillDate").ToString() == "")
                {
                    MsgBox.ShowOK("存在开单时间为空的数据，请检查！！！");
                    return;
                }
                //if (myGridView1.GetRowCellValue(i, "StartSite").ToString() == "")
                //{
                //    MsgBox.ShowOK("存在始发站为空的数据，请检查！！！");
                //    return;
                //}
                if (myGridView1.GetRowCellValue(i, "TransferMode").ToString() == "")
                {
                    MsgBox.ShowOK("存在交接方式为空的数据，请检查！！！");
                    return;
                }
                //if (myGridView1.GetRowCellValue(i, "DestinationSite").ToString() == "")
                //{
                //    MsgBox.ShowOK("存在到站为空的数据，请检查！！！");
                //    return;
                //}
                //if (myGridView1.GetRowCellValue(i, "TransferSite").ToString() == "")
                //{
                //    MsgBox.ShowOK("存在中转地为空的数据，请检查！！！");
                //    return;
                //}
                //if (myGridView1.GetRowCellValue(i, "TransitLines").ToString() == "")
                //{
                //    MsgBox.ShowOK("存在运输线路为空的数据，请检查！！！");
                //    return;
                //}
                if (myGridView1.GetRowCellValue(i, "TransitMode").ToString() == "")
                {
                    MsgBox.ShowOK("存在运输方式为空的数据，请检查！！！");
                    return;
                }
                if (myGridView1.GetRowCellValue(i, "ConsigneeCellPhone").ToString() == "")
                {
                    MsgBox.ShowOK("存在收货人手机为空的数据，请检查！！！");
                    return;
                }
                if (myGridView1.GetRowCellValue(i, "ConsigneeName").ToString() == "")
                {
                    MsgBox.ShowOK("存在收货联系人为空的数据，请检查！！！");
                    return;
                }
                //if (myGridView1.GetRowCellValue(i, "PickGoodsSite").ToString() == "")
                //{
                //    MsgBox.ShowOK("存在目的网点为空的数据，请检查！！！");
                //    return;
                //}
                if (myGridView1.GetRowCellValue(i, "ReceivProvince").ToString() == "")
                {
                    MsgBox.ShowOK("存在收货省为空的数据，请检查！！！");
                    return;
                }
                if (myGridView1.GetRowCellValue(i, "ReceivCity").ToString() == "")
                {
                    MsgBox.ShowOK("存在收货市为空的数据，请检查！！！");
                    return;
                }
                if (myGridView1.GetRowCellValue(i, "ConsignorName").ToString() == "")
                {
                    MsgBox.ShowOK("存在发货联系人为空的数据，请检查！！！");
                    return;
                }
                if (myGridView1.GetRowCellValue(i, "ConsignorCellPhone").ToString() == "")
                {
                    MsgBox.ShowOK("存在发货人手机为空的数据，请检查！！！");
                    return;
                }
                if (myGridView1.GetRowCellValue(i, "Varieties").ToString() == "")
                {
                    MsgBox.ShowOK("存在品名为空的数据，请检查！！！");
                    return;
                }
                if (myGridView1.GetRowCellValue(i, "Num").ToString() == "")
                {
                    MsgBox.ShowOK("存在件数为空的数据，请检查！！！");
                    return;
                }
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
                //if (myGridView1.GetRowCellValue(i, "Weight").ToString() == "")
                //{
                //    MsgBox.ShowOK("存在重量为空的数据，请检查！！！");
                //    return;
                //}
                //if (myGridView1.GetRowCellValue(i, "Volume").ToString() == "")
                //{
                //    MsgBox.ShowOK("存在体积为空的数据，请检查！！！");
                //    return;
                //}
                //if (myGridView1.GetRowCellValue(i, "Freight").ToString() == "")
                //{
                //    MsgBox.ShowOK("存在基本运费为空的数据，请检查！！！");
                //    return;
                //}
                //if (myGridView1.GetRowCellValue(i, "DeliFee").ToString() == "")
                //{
                //    MsgBox.ShowOK("存在开单送货费为空的数据，请检查！！！");
                //    return;
                //}
                //if (myGridView1.GetRowCellValue(i, "PaymentMode").ToString() == "")
                //{
                //    MsgBox.ShowOK("存在付款方式为空的数据，请检查！！！");
                //    return;
                //}
                //if (myGridView1.GetRowCellValue(i, "PaymentAmout").ToString() == "")
                //{
                //    MsgBox.ShowOK("存在总运费为空的数据，请检查！！！");
                //    return;
                //}
                //if (myGridView1.GetRowCellValue(i, "BillMan").ToString() == "")
                //{
                //    MsgBox.ShowOK("存在开单人为空的数据，请检查！！！");
                //    return;
                //}
                //if (myGridView1.GetRowCellValue(i, "BegWeb").ToString() == "")
                //{
                //    MsgBox.ShowOK("存在开单网点为空的数据，请检查！！！");
                //    return;
                //}
                //if (myGridView1.GetRowCellValue(i, "BillState").ToString() == "")
                //{
                //    MsgBox.ShowOK("存在运单状态为空的数据，请检查！！！");
                //    return;
                //}

                string a = myGridView1.GetRowCellValue(i, "BegWeb").ToString();
                string c = myGridView1.GetRowCellValue(i, "companyid").ToString();
                getdata(a, c);
                //if (b == 1)
                //{
                //    MsgBox.ShowOK("单号：" + myGridView1.GetRowCellValue(i, "BillNo").ToString() + "的开单网点不属于你公司的网点，请检查！");
                //    return;
                //}
            }

            //GetBillNo(BillNos);
            //if (billnum == 1) return;
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillNos", BillNos));
            list.Add(new SqlPara("BillDates", BillDates));
            list.Add(new SqlPara("StartSites", StartSites));
            list.Add(new SqlPara("TransferModes", TransferModes));
            list.Add(new SqlPara("DestinationSites", DestinationSites));
            list.Add(new SqlPara("TransferSites", TransferSites));
            //list.Add(new SqlPara("TransitLiness", TransitLiness));
            list.Add(new SqlPara("TransitModes", TransitModes));
            list.Add(new SqlPara("CusOderNos", CusOderNos));
            list.Add(new SqlPara("Packages", Packages));//hj20190118
            list.Add(new SqlPara("ConsignorCompanys", ConsignorCompanys));//hj20190118
            list.Add(new SqlPara("ConsigneeCellPhones", ConsigneeCellPhones));
            list.Add(new SqlPara("ConsigneeNames", ConsigneeNames));
            list.Add(new SqlPara("ConsigneeCompanys", ConsigneeCompanys));//hj20190118
            list.Add(new SqlPara("ReceivAddresss", ReceivAddresss));//hj20190118
            list.Add(new SqlPara("PickGoodsSites", PickGoodsSites));
            list.Add(new SqlPara("ReceivProvinces", ReceivProvinces));
            list.Add(new SqlPara("ReceivCitys", ReceivCitys));
            list.Add(new SqlPara("ReceivAreas", ReceivAreas));
            list.Add(new SqlPara("ConsignorNames", ConsignorNames));
            list.Add(new SqlPara("ConsignorCellPhones", ConsignorCellPhones));
            list.Add(new SqlPara("Varietiess", Varietiess));
            list.Add(new SqlPara("Nums", Nums));
            list.Add(new SqlPara("FeeWeights", FeeWeights));
            list.Add(new SqlPara("FeeVolumes", FeeVolumes));
            list.Add(new SqlPara("Weights", Weights));
            list.Add(new SqlPara("Volumes", Volumes));
            list.Add(new SqlPara("Freights", Freights));
            list.Add(new SqlPara("DeliFees", DeliFees));
            list.Add(new SqlPara("ReceivFees", ReceivFees));
            list.Add(new SqlPara("DiscountTransfers", DiscountTransfers));
            list.Add(new SqlPara("CollectionPays", CollectionPays));
            list.Add(new SqlPara("PaymentModes", PaymentModes));
            list.Add(new SqlPara("PaymentAmouts", PaymentAmouts));
            list.Add(new SqlPara("NowPays", NowPays));
            list.Add(new SqlPara("FetchPays", FetchPays));
            list.Add(new SqlPara("MonthPays", MonthPays));
            list.Add(new SqlPara("ReceiptPays", ReceiptPays));
            list.Add(new SqlPara("BillMans", BillMans));
            list.Add(new SqlPara("BegWebs", BegWebs));
            list.Add(new SqlPara("companyids", companyids));
            list.Add(new SqlPara("ActualFreights", ActualFreights));
            list.Add(new SqlPara("BillStates", BillStates));
            list.Add(new SqlPara("ReceiptConditions", ReceiptConditions));
            list.Add(new SqlPara("BillRemarks", BillRemarks));
            list.Add(new SqlPara("Salesmans", Salesmans));
            list.Add(new SqlPara("OperationWeights", OperationWeights));
            /* //2020-11-24  新增模板列 */
            list.Add(new SqlPara("SupportValue", SupportValue));
            list.Add(new SqlPara("IsSupportValue", IsSupportValue));
            list.Add(new SqlPara("DeclareValue", DeclareValue));
            list.Add(new SqlPara("HandleFee", HandleFee));
            list.Add(new SqlPara("IsHandleFee", IsHandleFee));
            list.Add(new SqlPara("MatPay", MatPay));
            list.Add(new SqlPara("IsMatPay", IsMatPay));
            list.Add(new SqlPara("UpstairFee", UpstairFee));
            list.Add(new SqlPara("IsUpstairFee", IsUpstairFee));
            list.Add(new SqlPara("OtherFee", OtherFee));
            list.Add(new SqlPara("IsOtherFee", IsOtherFee));
            list.Add(new SqlPara("AgentFee", AgentFee)); 

            SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "USP_ADD_WAYBILL_UPLOAD", list);//USP_ADD_WAYBILL_UPLOAD_TX_485
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
            wd.name = "运单模板";
            wd.path = "http://8.129.7.49:8013/file/%E8%BF%90%E5%8D%95%E6%A8%A1%E6%9D%BF.xls"; //
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
                saveFileDialog.FileName = "运单模板.xls";
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