using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Collections;
using System.Drawing.Printing;
using gregn6Lib;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using System.IO;

namespace ZQTMS.UI
{
    public partial class JMfrmPrintLabel : BaseForm
    {
        frmRuiLangService frls = new frmRuiLangService();
        GridppReport Report = new GridppReport();
        DataSet ds = new DataSet();
        int qty = 0;
        /// <summary>
        /// 运单号
        /// </summary>
        string BillNo = "";

        public JMfrmPrintLabel()
        {
            InitializeComponent();
        }

        public JMfrmPrintLabel(string BillNo, DataSet PrintDs)
        {
            InitializeComponent();
            this.BillNo = BillNo;
            this.ds = PrintDs;
        }

        private void w_print_lavel_Load(object sender, EventArgs e)
        {
            try
            {
                edprinters.Properties.Items.Clear();
                ArrayList al = new ArrayList();
                frls.GetPrinters(al);
                for (int i = 0; i < al.Count; i++)
                {
                    edprinters.Properties.Items.Add(al[i]);
                }
                //读取标签最大打印张数，0为不限
                edprinters.Text = API.ReadINI("RuiLangPrint", "PrintLabel", "", frmRuiLangService.configFileName).TrimEnd('\0').Trim();
                qty = ConvertType.ToInt32(ds.Tables[0].Rows[0]["Num"]);

                spinEdit1.EditValue = qty;
                lblbillno.Text = BillNo;
                lblconsignee.Text = ds.Tables[0].Rows[0]["ConsigneeName"].ToString();
                lblproduct.Text = ds.Tables[0].Rows[0]["Varieties"].ToString();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private bool print()
        {
            int nums = 0, sn = 0;
            if (edprinters.Text.Trim() == "")
            {
                MsgBox.ShowError("请选择打印机。");
                return false;
            }

            string ReportPath = Application.StartupPath + "\\Reports\\标签.grf";

            nums = Convert.ToInt32(spinEdit1.EditValue);
            sn = Convert.ToInt32(spinEdit2.EditValue);

            if (sn > qty)
            {
                MsgBox.ShowError("指定打印第几张大于总件数。");
                return false;
            }

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return false;

            if (!ds.Tables[0].Columns.Contains("rowid"))
            {
                DataColumn dc = new DataColumn("rowid", typeof(int));
                ds.Tables[0].Columns.Add(dc);
            }
            if (!ds.Tables[0].Columns.Contains("code"))
            {
                DataColumn dc1 = new DataColumn("code", typeof(string));
                ds.Tables[0].Columns.Add(dc1);
            }
            try
            {
                bool flag = false;
                string filePath = Application.StartupPath + "\\Reports\\标签.grf";
                string reportText = FileOperate.ReadFile(filePath);
                if (!reportText.Contains("PrinterName"))
                {
                    flag = true;
                    File.Move(filePath, filePath + "bak");
                    reportText = reportText.Replace("Object Printer", "Object Printer\r\n\t\tPrinterName='" + edprinters.Text.Trim() + "'");
                    FileOperate.WriteFile(filePath, reportText);
                }



                for (int i = sn; i < nums + sn; i++)
                {
                    ds.Tables[0].Rows[0]["rowid"] = i;
                    ds.Tables[0].Rows[0]["code"] = lblbillno.Text + "-" + i.ToString();
                    PrintEx(ReportPath, ds.Tables[0], edprinters.Text.Trim(), 0, 0, false);
                }

                //DataRow dr = ds.Tables[0].Rows[0];
                //DataSet data = ds.Clone();
                //for (int i = sn; i < nums + sn; i++)
                //{
                //    dr["rowid"] = i;
                //    dr["code"] = lblbillno.Text + "-" + i.ToString();
                //    data.Tables[0].Rows.Add(dr.ItemArray);
                //}
                //data.AcceptChanges();
                //PrintEx(ReportPath, data.Tables[0], edprinters.Text.Trim(), 0, 0, false);

                if (!flag) return false;
                File.Delete(filePath);
                File.Move(filePath + "bak", filePath);
            }
            catch 
            { }
            return true;
        }

        public void PrintEx(string ReportPath, DataTable dt, string printername, float paperlength, float paperwidth, bool ShowPrintDialog)
        {
            try
            {
                if (!System.IO.File.Exists(ReportPath))
                {
                    XtraMessageBox.Show("缺少相应的打印模板文件【" + ReportPath + "】!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (printername == "" || !frls.CheckPrinters(printername))
                {
                    PrintDocument prtdoc = new PrintDocument();
                    printername = prtdoc.PrinterSettings.PrinterName;//获取默认的打印机名 
                    ShowPrintDialog = true;
                }

                Report.LoadFromFile(ReportPath);
                if (dt.Rows.Count == 0)
                {
                    return;
                }
                ds = new DataSet();
                ds.Tables.Add(dt.Copy());
                if (paperlength != 0)
                {
                    Report.Printer.PaperLength = paperlength;
                    Report.Printer.PaperWidth = paperwidth;
                    Report.Printer.PaperSize = 256;
                    Report.Printer.SheetPages = GRSheetPages.grsp1Pages;
                }
                Report.Printer.PrinterName = printername;
                Report.LoadDataFromXML(ds.GetXml());
                Report.Print(ShowPrintDialog);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (print()) //打印完毕之后关闭
            {
                this.Close();
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void spinEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                print();
                this.Close();
            }
        }

        private void spinEdit2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                print();
                this.Close();
            }
        }
    }
}