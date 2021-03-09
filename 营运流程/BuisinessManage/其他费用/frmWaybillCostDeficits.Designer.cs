namespace ZQTMS.UI.其他费用
{
    partial class frmWaybillCostDeficits
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWaybillCostDeficits));
            this.Countdown = new System.Windows.Forms.Label();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.send = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.label1 = new System.Windows.Forms.Label();
            this.randomcode = new DevExpress.XtraEditors.TextEdit();
            this.cost = new DevExpress.XtraEditors.TextEdit();
            this.pay_heji = new DevExpress.XtraEditors.TextEdit();
            this.PaymentAmouts = new DevExpress.XtraEditors.TextEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.label_rate = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.randomcode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cost.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pay_heji.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PaymentAmouts.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // Countdown
            // 
            this.Countdown.AutoSize = true;
            this.Countdown.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Countdown.ForeColor = System.Drawing.Color.Red;
            this.Countdown.Location = new System.Drawing.Point(56, 212);
            this.Countdown.Name = "Countdown";
            this.Countdown.Size = new System.Drawing.Size(148, 20);
            this.Countdown.TabIndex = 374;
            this.Countdown.Text = "验证码剩余时间:120秒";
            this.Countdown.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.Countdown.Visible = false;
            // 
            // btnSave
            // 
            this.btnSave.Appearance.Font = new System.Drawing.Font("宋体", 12F);
            this.btnSave.Appearance.Options.UseFont = true;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(128, 291);
            this.btnSave.Name = "btnSave";
            this.btnSave.ShowToolTips = false;
            this.btnSave.Size = new System.Drawing.Size(76, 25);
            this.btnSave.TabIndex = 365;
            this.btnSave.Text = "确定";
            this.btnSave.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Hand;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // send
            // 
            this.send.Location = new System.Drawing.Point(242, 164);
            this.send.Name = "send";
            this.send.Size = new System.Drawing.Size(79, 23);
            this.send.TabIndex = 373;
            this.send.Text = "获取验证码";
            this.send.Click += new System.EventHandler(this.send_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Controls.Add(this.Countdown);
            this.panelControl1.Controls.Add(this.send);
            this.panelControl1.Controls.Add(this.randomcode);
            this.panelControl1.Controls.Add(this.cost);
            this.panelControl1.Controls.Add(this.pay_heji);
            this.panelControl1.Controls.Add(this.PaymentAmouts);
            this.panelControl1.Controls.Add(this.label6);
            this.panelControl1.Controls.Add(this.label_rate);
            this.panelControl1.Controls.Add(this.label3);
            this.panelControl1.Controls.Add(this.label4);
            this.panelControl1.Location = new System.Drawing.Point(8, 12);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(474, 253);
            this.panelControl1.TabIndex = 363;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(23, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(317, 20);
            this.label1.TabIndex = 375;
            this.label1.Text = "注：实收运费低于结算合计需填对验证码才可保存";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label1.Visible = false;
            // 
            // randomcode
            // 
            this.randomcode.EnterMoveNextControl = true;
            this.randomcode.Location = new System.Drawing.Point(107, 161);
            this.randomcode.Name = "randomcode";
            this.randomcode.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.randomcode.Properties.Appearance.Options.UseForeColor = true;
            this.randomcode.Size = new System.Drawing.Size(102, 21);
            this.randomcode.TabIndex = 372;
            // 
            // cost
            // 
            this.cost.EnterMoveNextControl = true;
            this.cost.Location = new System.Drawing.Point(103, 111);
            this.cost.Name = "cost";
            this.cost.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.cost.Properties.Appearance.Options.UseForeColor = true;
            this.cost.Properties.ReadOnly = true;
            this.cost.Size = new System.Drawing.Size(101, 21);
            this.cost.TabIndex = 370;
            // 
            // pay_heji
            // 
            this.pay_heji.EnterMoveNextControl = true;
            this.pay_heji.Location = new System.Drawing.Point(304, 60);
            this.pay_heji.Name = "pay_heji";
            this.pay_heji.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.pay_heji.Properties.Appearance.Options.UseForeColor = true;
            this.pay_heji.Properties.ReadOnly = true;
            this.pay_heji.Size = new System.Drawing.Size(102, 21);
            this.pay_heji.TabIndex = 367;
            // 
            // PaymentAmouts
            // 
            this.PaymentAmouts.EnterMoveNextControl = true;
            this.PaymentAmouts.Location = new System.Drawing.Point(103, 58);
            this.PaymentAmouts.Name = "PaymentAmouts";
            this.PaymentAmouts.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.PaymentAmouts.Properties.Appearance.Options.UseForeColor = true;
            this.PaymentAmouts.Properties.ReadOnly = true;
            this.PaymentAmouts.Size = new System.Drawing.Size(101, 21);
            this.PaymentAmouts.TabIndex = 366;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label6.Location = new System.Drawing.Point(43, 159);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 21);
            this.label6.TabIndex = 352;
            this.label6.Text = "验证码";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label_rate
            // 
            this.label_rate.AutoSize = true;
            this.label_rate.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label_rate.Location = new System.Drawing.Point(27, 56);
            this.label_rate.Name = "label_rate";
            this.label_rate.Size = new System.Drawing.Size(74, 21);
            this.label_rate.TabIndex = 344;
            this.label_rate.Text = "实收运费";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label3.Location = new System.Drawing.Point(224, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 21);
            this.label3.TabIndex = 346;
            this.label3.Text = "结算合计";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label4.Location = new System.Drawing.Point(39, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 21);
            this.label4.TabIndex = 348;
            this.label4.Text = "差额";
            // 
            // btnClose
            // 
            this.btnClose.Appearance.Font = new System.Drawing.Font("宋体", 12F);
            this.btnClose.Appearance.Options.UseFont = true;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(299, 291);
            this.btnClose.Name = "btnClose";
            this.btnClose.ShowToolTips = false;
            this.btnClose.Size = new System.Drawing.Size(76, 25);
            this.btnClose.TabIndex = 364;
            this.btnClose.Text = "取消";
            this.btnClose.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Hand;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // frmWaybillCostDeficits
            // 
            this.ClientSize = new System.Drawing.Size(492, 324);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.btnClose);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmWaybillCostDeficits";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "开单验证";
            this.Load += new System.EventHandler(this.frmWaybillCostDeficits_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.randomcode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cost.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pay_heji.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PaymentAmouts.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label Countdown;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton send;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.TextEdit randomcode;
        private DevExpress.XtraEditors.TextEdit cost;
        private DevExpress.XtraEditors.TextEdit pay_heji;
        private DevExpress.XtraEditors.TextEdit PaymentAmouts;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label_rate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
    }
}
