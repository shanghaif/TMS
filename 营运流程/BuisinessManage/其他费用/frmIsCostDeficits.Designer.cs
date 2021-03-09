namespace ZQTMS.UI.其他费用
{
    partial class frmIsCostDeficits
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmIsCostDeficits));
            this.send = new DevExpress.XtraEditors.SimpleButton();
            this.randomcode = new DevExpress.XtraEditors.TextEdit();
            this.TelPhone = new DevExpress.XtraEditors.TextEdit();
            this.ChargePerson = new DevExpress.XtraEditors.TextEdit();
            this.costrate = new DevExpress.XtraEditors.TextEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.Countdown = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.TargetcostRate = new DevExpress.XtraEditors.TextEdit();
            this.actually_rate = new DevExpress.XtraEditors.TextEdit();
            this.label_pc = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.billno = new DevExpress.XtraEditors.TextEdit();
            this.label_rate = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.randomcode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TelPhone.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChargePerson.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.costrate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TargetcostRate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.actually_rate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.billno.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // send
            // 
            this.send.Location = new System.Drawing.Point(251, 241);
            this.send.Name = "send";
            this.send.Size = new System.Drawing.Size(79, 23);
            this.send.TabIndex = 373;
            this.send.Text = "点击发送";
            this.send.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // randomcode
            // 
            this.randomcode.EnterMoveNextControl = true;
            this.randomcode.Location = new System.Drawing.Point(103, 243);
            this.randomcode.Name = "randomcode";
            this.randomcode.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.randomcode.Properties.Appearance.Options.UseForeColor = true;
            this.randomcode.Size = new System.Drawing.Size(102, 21);
            this.randomcode.TabIndex = 372;
            // 
            // TelPhone
            // 
            this.TelPhone.EnterMoveNextControl = true;
            this.TelPhone.Location = new System.Drawing.Point(288, 189);
            this.TelPhone.Name = "TelPhone";
            this.TelPhone.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.TelPhone.Properties.Appearance.Options.UseForeColor = true;
            this.TelPhone.Properties.ReadOnly = true;
            this.TelPhone.Size = new System.Drawing.Size(122, 21);
            this.TelPhone.TabIndex = 371;
            // 
            // ChargePerson
            // 
            this.ChargePerson.EnterMoveNextControl = true;
            this.ChargePerson.Location = new System.Drawing.Point(106, 189);
            this.ChargePerson.Name = "ChargePerson";
            this.ChargePerson.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.ChargePerson.Properties.Appearance.Options.UseForeColor = true;
            this.ChargePerson.Properties.ReadOnly = true;
            this.ChargePerson.Size = new System.Drawing.Size(101, 21);
            this.ChargePerson.TabIndex = 370;
            // 
            // costrate
            // 
            this.costrate.EnterMoveNextControl = true;
            this.costrate.Location = new System.Drawing.Point(107, 140);
            this.costrate.Name = "costrate";
            this.costrate.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.costrate.Properties.Appearance.Options.UseForeColor = true;
            this.costrate.Properties.ReadOnly = true;
            this.costrate.Size = new System.Drawing.Size(101, 21);
            this.costrate.TabIndex = 369;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.Countdown);
            this.panelControl1.Controls.Add(this.send);
            this.panelControl1.Controls.Add(this.randomcode);
            this.panelControl1.Controls.Add(this.TelPhone);
            this.panelControl1.Controls.Add(this.ChargePerson);
            this.panelControl1.Controls.Add(this.costrate);
            this.panelControl1.Controls.Add(this.label10);
            this.panelControl1.Controls.Add(this.TargetcostRate);
            this.panelControl1.Controls.Add(this.actually_rate);
            this.panelControl1.Controls.Add(this.label_pc);
            this.panelControl1.Controls.Add(this.label6);
            this.panelControl1.Controls.Add(this.billno);
            this.panelControl1.Controls.Add(this.label_rate);
            this.panelControl1.Controls.Add(this.label5);
            this.panelControl1.Controls.Add(this.label3);
            this.panelControl1.Controls.Add(this.label4);
            this.panelControl1.Location = new System.Drawing.Point(12, 12);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(474, 314);
            this.panelControl1.TabIndex = 360;
            // 
            // Countdown
            // 
            this.Countdown.AutoSize = true;
            this.Countdown.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Countdown.ForeColor = System.Drawing.Color.Red;
            this.Countdown.Location = new System.Drawing.Point(103, 281);
            this.Countdown.Name = "Countdown";
            this.Countdown.Size = new System.Drawing.Size(148, 20);
            this.Countdown.TabIndex = 374;
            this.Countdown.Text = "验证码剩余时间:180秒";
            this.Countdown.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.Countdown.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label10.Location = new System.Drawing.Point(17, 138);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 21);
            this.label10.TabIndex = 368;
            this.label10.Text = "亏损率";
            // 
            // TargetcostRate
            // 
            this.TargetcostRate.EnterMoveNextControl = true;
            this.TargetcostRate.Location = new System.Drawing.Point(308, 84);
            this.TargetcostRate.Name = "TargetcostRate";
            this.TargetcostRate.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.TargetcostRate.Properties.Appearance.Options.UseForeColor = true;
            this.TargetcostRate.Properties.ReadOnly = true;
            this.TargetcostRate.Size = new System.Drawing.Size(102, 21);
            this.TargetcostRate.TabIndex = 367;
            // 
            // actually_rate
            // 
            this.actually_rate.EnterMoveNextControl = true;
            this.actually_rate.Location = new System.Drawing.Point(107, 84);
            this.actually_rate.Name = "actually_rate";
            this.actually_rate.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.actually_rate.Properties.Appearance.Options.UseForeColor = true;
            this.actually_rate.Properties.ReadOnly = true;
            this.actually_rate.Size = new System.Drawing.Size(101, 21);
            this.actually_rate.TabIndex = 366;
            // 
            // label_pc
            // 
            this.label_pc.AutoSize = true;
            this.label_pc.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label_pc.Location = new System.Drawing.Point(17, 34);
            this.label_pc.Name = "label_pc";
            this.label_pc.Size = new System.Drawing.Size(74, 21);
            this.label_pc.TabIndex = 26;
            this.label_pc.Text = "发车批次";
            this.label_pc.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label6.Location = new System.Drawing.Point(39, 241);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 21);
            this.label6.TabIndex = 352;
            this.label6.Text = "验证码";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // billno
            // 
            this.billno.EnterMoveNextControl = true;
            this.billno.Location = new System.Drawing.Point(103, 36);
            this.billno.Name = "billno";
            this.billno.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.billno.Properties.Appearance.Options.UseForeColor = true;
            this.billno.Properties.ReadOnly = true;
            this.billno.Size = new System.Drawing.Size(304, 21);
            this.billno.TabIndex = 353;
            // 
            // label_rate
            // 
            this.label_rate.AutoSize = true;
            this.label_rate.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label_rate.Location = new System.Drawing.Point(17, 82);
            this.label_rate.Name = "label_rate";
            this.label_rate.Size = new System.Drawing.Size(58, 21);
            this.label_rate.TabIndex = 344;
            this.label_rate.Text = "成本率";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label5.Location = new System.Drawing.Point(224, 189);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 21);
            this.label5.TabIndex = 350;
            this.label5.Text = "手机号";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label3.Location = new System.Drawing.Point(213, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 21);
            this.label3.TabIndex = 346;
            this.label3.Text = "目标成本率";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label4.Location = new System.Drawing.Point(39, 187);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 21);
            this.label4.TabIndex = 348;
            this.label4.Text = "负责人";
            // 
            // btnClose
            // 
            this.btnClose.Appearance.Font = new System.Drawing.Font("宋体", 12F);
            this.btnClose.Appearance.Options.UseFont = true;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(290, 348);
            this.btnClose.Name = "btnClose";
            this.btnClose.ShowToolTips = false;
            this.btnClose.Size = new System.Drawing.Size(76, 25);
            this.btnClose.TabIndex = 361;
            this.btnClose.Text = "取消";
            this.btnClose.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Hand;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click_1);
            // 
            // btnSave
            // 
            this.btnSave.Appearance.Font = new System.Drawing.Font("宋体", 12F);
            this.btnSave.Appearance.Options.UseFont = true;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(119, 348);
            this.btnSave.Name = "btnSave";
            this.btnSave.ShowToolTips = false;
            this.btnSave.Size = new System.Drawing.Size(76, 25);
            this.btnSave.TabIndex = 362;
            this.btnSave.Text = "确定";
            this.btnSave.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Hand;
            this.btnSave.Click += new System.EventHandler(this.simpleButton1_Click_1);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // frmIsCostDeficits
            // 
            this.ClientSize = new System.Drawing.Size(495, 394);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmIsCostDeficits";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "亏损发送短信";
            this.Load += new System.EventHandler(this.frmIsCostDeficits_Load);
            ((System.ComponentModel.ISupportInitialize)(this.randomcode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TelPhone.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChargePerson.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.costrate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TargetcostRate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.actually_rate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.billno.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton send;
        private DevExpress.XtraEditors.TextEdit randomcode;
        private DevExpress.XtraEditors.TextEdit TelPhone;
        private DevExpress.XtraEditors.TextEdit ChargePerson;
        private DevExpress.XtraEditors.TextEdit costrate;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.Label label10;
        private DevExpress.XtraEditors.TextEdit TargetcostRate;
        private DevExpress.XtraEditors.TextEdit actually_rate;
        private System.Windows.Forms.Label label_pc;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.TextEdit billno;
        private System.Windows.Forms.Label label_rate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label Countdown;
    }
}
