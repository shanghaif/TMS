using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;

namespace ZQTMS.UI
{
    public partial class frmSendInventoryColor : BaseForm
    {
        public frmSendInventoryColor()
        {
            InitializeComponent();
        }

        private void frmSendInventoryColor_Load(object sender, EventArgs e)
        {
            try
            {
                txtDay1.Text = API.ReadINI("Color", "iWarningOne", "0", frmRuiLangService.configFileName).Trim();
                colorOne.Color = Color.FromArgb(
                    ConvertType.ToInt32(API.ReadINI("Color", "iColorOneR", "255", frmRuiLangService.configFileName)),
                    ConvertType.ToInt32(API.ReadINI("Color", "iColorOneG", "255", frmRuiLangService.configFileName)),
                    ConvertType.ToInt32(API.ReadINI("Color", "iColorOneB", "255", frmRuiLangService.configFileName)));

                txtDay2.Text = API.ReadINI("Color", "iWarningTwo", "2", frmRuiLangService.configFileName).Trim();
                colorTwo.Color = Color.FromArgb(
                    ConvertType.ToInt32(API.ReadINI("Color", "iColorTwoR", "0", frmRuiLangService.configFileName)),
                    ConvertType.ToInt32(API.ReadINI("Color", "iColorTwoG", "255", frmRuiLangService.configFileName)),
                    ConvertType.ToInt32(API.ReadINI("Color", "iColorTwoB", "0", frmRuiLangService.configFileName)));

                txtDay3.Text = API.ReadINI("Color", "iWarningThree", "4", frmRuiLangService.configFileName).Trim();
                colorThree.Color = Color.FromArgb(
                    ConvertType.ToInt32(API.ReadINI("Color", "iColorThreeR", "255", frmRuiLangService.configFileName)),
                    ConvertType.ToInt32(API.ReadINI("Color", "iColorThreeG", "0", frmRuiLangService.configFileName)),
                    ConvertType.ToInt32(API.ReadINI("Color", "iColorThreeB", "0", frmRuiLangService.configFileName)));
            }
            catch (Exception)
            { }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                API.WriteINI("Color", "iWarningOne", txtDay1.Text.Trim(), frmRuiLangService.configFileName);
                API.WriteINI("Color", "iColorOneR", colorOne.Color.R.ToString(), frmRuiLangService.configFileName);
                API.WriteINI("Color", "iColorOneG", colorOne.Color.G.ToString(), frmRuiLangService.configFileName);
                API.WriteINI("Color", "iColorOneB", colorOne.Color.B.ToString(), frmRuiLangService.configFileName);
                API.WriteINI("Color", "iWarningTwo", txtDay2.Text.Trim(), frmRuiLangService.configFileName);
                API.WriteINI("Color", "iColorTwoR", colorTwo.Color.R.ToString(), frmRuiLangService.configFileName);
                API.WriteINI("Color", "iColorTwoG", colorTwo.Color.G.ToString(), frmRuiLangService.configFileName);
                API.WriteINI("Color", "iColorTwoB", colorTwo.Color.B.ToString(), frmRuiLangService.configFileName);
                API.WriteINI("Color", "iWarningThree", txtDay3.Text.Trim(), frmRuiLangService.configFileName);
                API.WriteINI("Color", "iColorThreeR", colorThree.Color.R.ToString(), frmRuiLangService.configFileName);
                API.WriteINI("Color", "iColorThreeG", colorThree.Color.G.ToString(), frmRuiLangService.configFileName);
                API.WriteINI("Color", "iColorThreeB", colorThree.Color.B.ToString(), frmRuiLangService.configFileName);

                XtraMessageBox.Show("保存成功！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}