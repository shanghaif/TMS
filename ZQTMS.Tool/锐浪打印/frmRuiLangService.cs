using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Printing;
using System.Collections;
using System.Data;
using gregn6Lib;
using System.Windows.Forms;

namespace ZQTMS.Tool
{
    public class frmRuiLangService
    {
        /// <summary>
        /// 配置/参数文件名称
        /// </summary>
        public static string configFileName = "config.dll";

        #region 锐浪相关
        /// <summary>
        /// 替换运算符
        /// </summary>
        /// <param name="c">要替换的中文</param>
        /// <returns></returns>
        public string GetStr(string str)
        {
            string c = "";
            switch (str)
            {
                case "等于":
                    c = "=";
                    break;
                case "不等于":
                    c = "<>";
                    break;
                case "包含":
                    c = "like";
                    break;
                case "不包含":
                    c = "not like";
                    break;
                case "为空":
                    c = "is null";
                    break;
                case "不为空":
                    c = "is not null";
                    break;
                case "小于":
                    c = "<";
                    break;
                case "小于等于":
                    c = "<=";
                    break;
                case "大于":
                    c = ">";
                    break;
                case "大于等于":
                    c = ">=";
                    break;
                default:
                    break;

            }
            return c;
        }

        public DataTable datatable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("columnName");
            dt.Columns.Add("operators");
            dt.Columns.Add("value");
            for (int i = 0; i < 2; i++)
            {
                dt.Rows.Add(new object[] { "", "", "" });
            }
            return dt;
        }

        //在列表框中列出所有的打印机, 
        public void GetPrinters(ArrayList al)
        {
            foreach (String strPrinter in PrinterSettings.InstalledPrinters)
            {
                al.Add(strPrinter);
            }
        }

        //检查是否有效打印机
        public bool CheckPrinters(string printer)
        {
            foreach (String strPrinter in PrinterSettings.InstalledPrinters)
            {
                if (printer == strPrinter)
                    return true;
            }
            return false;
        }
        #endregion

        /// <summary>
        /// 获取报表设置的默认打印名称
        /// </summary>
        /// <returns></returns>
        public static string GetPrinterName(string reportName)
        {
            string printerName = "";
            switch (reportName)
            {
                case "托运单.grf":
                    printerName = API.ReadINI("RuiLangPrint", "PrintWayBill", "", frmRuiLangService.configFileName);
                    break;
                case "标签.grf":
                    printerName = API.ReadINI("RuiLangPrint", "PrintLabel", "", frmRuiLangService.configFileName);
                    break;
                case "信封.grf":
                    printerName = API.ReadINI("RuiLangPrint", "PrintEnvelope", "", frmRuiLangService.configFileName);
                    break;
                case "派车单.grf":
                    printerName = API.ReadINI("RuiLangPrint", "PrintDelivery", "", frmRuiLangService.configFileName);
                    break;
                case "短驳清单.grf":
                    printerName = API.ReadINI("RuiLangPrint", "PrintShortConn", "", frmRuiLangService.configFileName);
                    break;
                case "配载清单.grf":
                case "司机运输协议.grf":
                case "装车清单.grf":
                    printerName = API.ReadINI("RuiLangPrint", "PrintDeparture", "", frmRuiLangService.configFileName);
                    break;
                case "送货清单":
                case "预送货清单":
                    printerName = API.ReadINI("RuiLangPrint", "PrintSendGoods", "", frmRuiLangService.configFileName);
                    break;
                case "提货单.grf":
                case "提货单(套打).grf":
                    printerName = API.ReadINI("RuiLangPrint", "PrintSign", "", frmRuiLangService.configFileName);
                    break;
                case "回扣凭证.grf":
                    printerName = API.ReadINI("RuiLangPrint", "PrintVouchers", "", frmRuiLangService.configFileName);
                    break;
                default:
                    break;
            }
            if (printerName == "")
            {
                PrintDocument pd = new PrintDocument();
                printerName = pd.PrinterSettings.PrinterName;
            }
            return printerName;
        }

        public static void Print(string reportName, DataSet printDs)
        {
            if (string.IsNullOrEmpty(reportName) || printDs == null || printDs.Tables.Count == 0 || printDs.Tables[0].Rows.Count == 0) return;
            reportName = (reportName.EndsWith(".grf") ? reportName : reportName + ".grf");
            string reportpath = Application.StartupPath + "\\Reports\\" + reportName;
            if (!System.IO.File.Exists(reportpath))
            {
                MsgBox.ShowError("找不到相应的打印模板文件【" + reportName + "】");
                return;
            }
            if (printDs == null)
            {
                MsgBox.ShowError("没有数据,不能打印!");
                return;
            }
            GridppReport Report = new GridppReport();
            Report.LoadFromFile(reportpath);
            Report.LoadDataFromXML(printDs.GetXml());
            Report.Printer.PrinterName = GetPrinterName(reportName);
            Report.Print(false);
        }

        public static void Print(string reportName, DataTable printDt, string strCompanyName)
        {
            if (string.IsNullOrEmpty(reportName) || printDt == null || printDt.Rows.Count == 0) return;

            DataSet ds = new DataSet();
            DataTable dt = printDt.Clone();
            ds.Tables.Add(dt);
            for (int i = 0; i < printDt.Rows.Count; i++)
            {
                dt.Rows.Clear();
                dt.ImportRow(printDt.Rows[i]);
                Print(reportName, ds);
            }
        }

        //public static void Print(string p, DataTable dataTable, string p_3)
        //{
        //    throw new NotImplementedException();
        //}

        //public static void Print(string p, DataSet ds, string p_3)
        //{
        //    throw new NotImplementedException();
        //}

        public static void Print(string p1, DataSet ds, string p2)
        {
            throw new NotImplementedException();
        }

        public static void Print(string p, DataTable dataTable)
        {
            //throw new NotImplementedException();
            if (string.IsNullOrEmpty(p) || dataTable == null || dataTable.Rows.Count == 0) return;

            DataSet ds = new DataSet();
            DataTable dt = dataTable.Clone();
            ds.Tables.Add(dt);
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                dt.Rows.Clear();
                dt.ImportRow(dataTable.Rows[i]);
                Print(p, ds);
            }
        }
    }
}