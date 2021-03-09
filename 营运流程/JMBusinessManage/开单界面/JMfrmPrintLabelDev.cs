using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;
using DevExpress.XtraEditors;
using System.Drawing.Printing;
using System.Collections;
using DevExpress.XtraReports;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using System.IO;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class JMfrmPrintLabelDev : BaseForm
    {
        frmRuiLangService frls = new frmRuiLangService();
        public XtraReport rpt = new XtraReport();
        string fileName = "";
        DataTable DtBillNo = new DataTable();

        public JMfrmPrintLabelDev()
        {
            InitializeComponent();
        }

        public JMfrmPrintLabelDev(DataTable DtBillNo)
        {
            InitializeComponent();
            this.DtBillNo = DtBillNo;
        }

        private void w_print_lavel_Load(object sender, EventArgs e)
        {
            if (DtBillNo == null || DtBillNo.Rows.Count == 0)
            {
                simpleButton1.Enabled = false;
                return;
            }
            fileName = Application.StartupPath + "\\Reports\\" + CommonClass.GetLabelFileName;
            try
            {
                edprinters.Properties.Items.Clear();
                ArrayList al = new ArrayList();
                frls.GetPrinters(al);
                for (int i = 0; i < al.Count; i++)
                {
                    edprinters.Properties.Items.Add(al[i]);
                }
                if (!File.Exists(fileName))
                {
                    XtraMessageBox.Show("标签文件不存在,无法打印标签!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    simpleButton1.Enabled = false;
                    return;
                }

                if (rpt.Report.Name == "")
                {
                    rpt.LoadLayout(fileName);
                }

                string printer = API.ReadINI("RuiLangPrint", "PrintLabel", "", frmRuiLangService.configFileName);
                if (al.Contains(printer))
                {
                    edprinters.Text = printer;
                }

                int Num = ConvertType.ToInt32(DtBillNo.Rows[0]["Num"]);
                spinEdit1.Properties.MaxValue = Num;
                spinEdit1.Properties.MinValue = 1;
                spinEdit1.Text = Num.ToString();

                spinEdit2.EditValue = 1;
                spinEdit2.Properties.MaxValue = Num;
                spinEdit2.Properties.MinValue = 1;

                spinEdit3.EditValue = 1;
                spinEdit3.Properties.MaxValue = Num;
                spinEdit3.Properties.MinValue = 1;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void print()
        {
            DtBillNo.Rows[0]["Num"] = DtBillNo.Rows[0]["Num"] + "件";
            DtBillNo.Rows[0]["W_V"] = ConvertType.ToDecimal(DtBillNo.Rows[0]["FeeWeight"], "0") + "/" + ConvertType.ToDecimal(DtBillNo.Rows[0]["FeeVolume"], "0");
            int labelSeq = 0;
            if (DtBillNo.Columns.Contains("LabelSeq")) labelSeq = ConvertType.ToInt32(DtBillNo.Rows[0]["LabelSeq"]);
            try
            {
                #region 绑定条码控件
                for (int i = 0; i < rpt.Bands.Count; i++)
                {
                    if (rpt.Bands[i].GetType() == typeof(DetailBand))
                    {
                        for (int j = 0; j < rpt.Bands[i].Controls.Count; j++)
                        {
                            XRControl con = rpt.Bands[i].Controls[j];
                            if (con.GetType() == typeof(XRRichText) || con.GetType() == typeof(XRCheckBox) || con.GetType() == typeof(XRBarCode) || con.GetType() == typeof(XRZipCode))
                            {
                                for (int y = 0; y < DtBillNo.Columns.Count; y++)
                                {
                                    if (con.Name == DtBillNo.Columns[y].ColumnName || (DtBillNo.Columns[y].ColumnName == "code" && con.Name.Contains("code")))
                                    {
                                        con.DataBindings.Add("Text", DtBillNo, DtBillNo.Columns[y].ColumnName);
                                        break;
                                    }
                                }
                            }
                        }
                        break;
                    }
                }
                #endregion

                if (radioGroup1.SelectedIndex == 0)
                {
                    object[] dr = DtBillNo.Rows[0].ItemArray;
                    for (int i = 0; i < int.Parse(spinEdit1.EditValue.ToString()) - 1; i++)
                    {
                        DtBillNo.Rows.Add(dr);
                    }
                    for (int j = 0; j < DtBillNo.Columns.Count; j++)
                    {
                        if (DtBillNo.Columns[j].ColumnName.Contains("code") || DtBillNo.Columns[j].ColumnName.Contains("rowid"))
                        {
                            for (int i = 0; i < DtBillNo.Rows.Count; i++)
                            {
                                if (DtBillNo.Columns[j].ColumnName.Contains("code"))
                                    DtBillNo.Rows[i][j] = ConvertType.ToString(DtBillNo.Rows[i]["BillNo"]) + "-" + (labelSeq + i + 1); //打印条码
                                if (DtBillNo.Columns[j].ColumnName.Contains("rowid"))
                                    DtBillNo.Rows[i][j] = "第" + (i + 1) + "件";
                            }
                        }
                    }
                }
                else
                {
                    int start = int.Parse(spinEdit2.EditValue.ToString());
                    int end = int.Parse(spinEdit3.EditValue.ToString());
                    object[] dr = DtBillNo.Rows[0].ItemArray;
                    for (int i = start; i < end; i++)
                    {
                        DtBillNo.Rows.Add(dr);
                    }

                    for (int j = 0; j < DtBillNo.Columns.Count; j++)
                    {
                        if (DtBillNo.Columns[j].ColumnName.Contains("code") || DtBillNo.Columns[j].ColumnName.Contains("rowid"))
                        {
                            for (int i = 0; i < DtBillNo.Rows.Count; i++)
                            {
                                DtBillNo.Rows[i][j] = ConvertType.ToString(DtBillNo.Rows[i]["BillNo"]) + "-" + (labelSeq + start + i);
                                if (DtBillNo.Columns[j].ColumnName.Contains("rowid"))
                                    DtBillNo.Rows[i][j] = "第" + (start + i) + "件";
                            }
                        }
                    }
                }

                rpt.ShowPrintStatusDialog = false; //显示打印状态框
                rpt.ShowPrintMarginsWarning = false;
                rpt.DataSource = DtBillNo;
                rpt.DataMember = DtBillNo.TableName;
                printControl1.PrintingSystem = rpt.PrintingSystem;
                rpt.CreateDocument();
                printControl1.UpdatePageView();

                if (printControl1.PrintingSystem == null)
                {
                    return;
                }
                printControl1.PrintingSystem.Document.Name = "中强集团-标签打印";
                printControl1.PrintingSystem.PageSettings.PrinterName = edprinters.Text.Trim();
                printControl1.PrintingSystem.Print();
                //rpt.ExportToXls("d:\\1.xls");
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (edprinters.Text.Trim() == "")
                {
                    XtraMessageBox.Show("请选择打印机!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    edprinters.Focus();
                    return;
                }

                if (radioGroup1.SelectedIndex == 1 && int.Parse(spinEdit2.EditValue.ToString()) > int.Parse(spinEdit3.EditValue.ToString()))
                {
                    MsgBox.ShowOK("起止序号选择错误：\r\n截止序号必须大于起始序号!");
                    spinEdit3.Focus();
                }

                //保存标签序号
                if (DtBillNo.Columns.Contains("LabelSeq"))
                {
                    if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_BILL_LABELSEQ", new List<SqlPara> { new SqlPara("BillNo", DtBillNo.Rows[0]["BillNo"]), new SqlPara("LabelSeq", ConvertType.ToInt32(spinEdit1.Text)) })) == 0)
                    {
                        MsgBox.ShowOK("标签序号生成失败，请重试！", "系统提示");
                        return;
                    }
                }

                print();
                API.WriteINI("RuiLangPrint", "PrintLabel", edprinters.Text.Trim(), frmRuiLangService.configFileName);
                this.Close();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            spinEdit1.Enabled = radioGroup1.SelectedIndex == 0 ? true : false;
            spinEdit2.Enabled = spinEdit3.Enabled = radioGroup1.SelectedIndex == 0 ? false : true;
        }
    }
}