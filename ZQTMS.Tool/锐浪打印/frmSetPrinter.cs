using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Collections;
using ZQTMS.Common;
//using ZQTMS.Common;

namespace ZQTMS.Tool
{
    public partial class frmSetPrinter : BaseForm
    {
        frmRuiLangService frl = new frmRuiLangService();
        static frmSetPrinter fsp;

        public frmSetPrinter()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取当前对象实例
        /// </summary>
        public static frmSetPrinter Get_frmSetPrinter { get { if (fsp == null || fsp.IsDisposed)fsp = new frmSetPrinter(); return fsp; } }

        private void frmSetPrinter_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("打印机设置");//xj/2019/5/29
            cbWayBill.Text = API.ReadINI("RuiLangPrint", "printWayBill", "", frmRuiLangService.configFileName).TrimEnd('\0').Trim();
            cbLabel.Text = API.ReadINI("RuiLangPrint", "printLabel", "", frmRuiLangService.configFileName).TrimEnd('\0').Trim();
            cbEnvelope.Text = API.ReadINI("RuiLangPrint", "PrintEnvelope", "", frmRuiLangService.configFileName).TrimEnd('\0').Trim();
            cbDelivery.Text = API.ReadINI("RuiLangPrint", "PrintDelivery", "", frmRuiLangService.configFileName).TrimEnd('\0').Trim();
            cbShortConn.Text = API.ReadINI("RuiLangPrint", "PrintShortConn", "", frmRuiLangService.configFileName).TrimEnd('\0').Trim();
            cbDeparture.Text = API.ReadINI("RuiLangPrint", "PrintDeparture", "", frmRuiLangService.configFileName).TrimEnd('\0').Trim();
            cbSendGoods.Text = API.ReadINI("RuiLangPrint", "PrintSendGoods", "", frmRuiLangService.configFileName).TrimEnd('\0').Trim();
            cbSign.Text = API.ReadINI("RuiLangPrint", "PrintSign", "", frmRuiLangService.configFileName).TrimEnd('\0').Trim();
            cbVouchers.Text = API.ReadINI("RuiLangPrint", "PrintVouchers", "", frmRuiLangService.configFileName).TrimEnd('\0').Trim();//tuxin20181025 打印回扣凭证
            fill();
        }

        private void fill()
        {
            ArrayList al = new ArrayList();
            frl.GetPrinters(al);
            for (int i = 0; i < al.Count; i++)
            {
                cbWayBill.Properties.Items.Add(al[i]);
                cbLabel.Properties.Items.Add(al[i]);
                cbEnvelope.Properties.Items.Add(al[i]);
                cbDelivery.Properties.Items.Add(al[i]);
                cbShortConn.Properties.Items.Add(al[i]);
                cbDeparture.Properties.Items.Add(al[i]);
                cbSendGoods.Properties.Items.Add(al[i]);
                cbSign.Properties.Items.Add(al[i]);
                cbVouchers.Properties.Items.Add(al[i]);//tuxin20181025

            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            API.WriteINI("RuiLangPrint", "printWayBill", cbWayBill.Text.Trim(), frmRuiLangService.configFileName);
            API.WriteINI("RuiLangPrint", "printLabel", cbLabel.Text.Trim(), frmRuiLangService.configFileName);
            API.WriteINI("RuiLangPrint", "PrintEnvelope", cbEnvelope.Text.Trim(), frmRuiLangService.configFileName);
            API.WriteINI("RuiLangPrint", "PrintDelivery", cbDelivery.Text.Trim(), frmRuiLangService.configFileName);
            API.WriteINI("RuiLangPrint", "PrintShortConn", cbShortConn.Text.Trim(), frmRuiLangService.configFileName);
            API.WriteINI("RuiLangPrint", "PrintDeparture", cbDeparture.Text.Trim(), frmRuiLangService.configFileName);
            API.WriteINI("RuiLangPrint", "PrintSendGoods", cbSendGoods.Text.Trim(), frmRuiLangService.configFileName);
            API.WriteINI("RuiLangPrint", "PrintSign", cbSign.Text.Trim(), frmRuiLangService.configFileName);
            API.WriteINI("RuiLangPrint", "PrintVouchers", cbVouchers.Text.Trim(), frmRuiLangService.configFileName);//tuxin20181025
            MsgBox.ShowOK();
        }
    }
}