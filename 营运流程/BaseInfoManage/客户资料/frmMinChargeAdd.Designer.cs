namespace ZQTMS.UI.BaseInfoManage.客户资料
{
    partial class frmMinChargeAdd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMinChargeAdd));
            this.PaymentMode = new DevExpress.XtraEditors.ComboBoxEdit();
            this.BelongWeb = new DevExpress.XtraEditors.ComboBoxEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.ConsignorPhone = new DevExpress.XtraEditors.TextEdit();
            this.ConsignorCellPhone = new DevExpress.XtraEditors.TextEdit();
            this.ConsignorCompany = new DevExpress.XtraEditors.TextEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ConsignorName = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.PaymentMode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BelongWeb.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConsignorPhone.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConsignorCellPhone.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConsignorCompany.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConsignorName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // PaymentMode
            // 
            this.PaymentMode.EditValue = "全部";
            this.PaymentMode.Location = new System.Drawing.Point(343, 105);
            this.PaymentMode.Name = "PaymentMode";
            this.PaymentMode.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.PaymentMode.Properties.Appearance.Options.UseFont = true;
            this.PaymentMode.Properties.AutoComplete = false;
            this.PaymentMode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.PaymentMode.Size = new System.Drawing.Size(137, 26);
            this.PaymentMode.TabIndex = 363;
            // 
            // BelongWeb
            // 
            this.BelongWeb.EditValue = "全部";
            this.BelongWeb.Location = new System.Drawing.Point(84, 106);
            this.BelongWeb.Name = "BelongWeb";
            this.BelongWeb.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.BelongWeb.Properties.Appearance.Options.UseFont = true;
            this.BelongWeb.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.BelongWeb.Size = new System.Drawing.Size(139, 26);
            this.BelongWeb.TabIndex = 361;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.label11);
            this.panelControl1.Controls.Add(this.label10);
            this.panelControl1.Controls.Add(this.ConsignorPhone);
            this.panelControl1.Controls.Add(this.ConsignorCellPhone);
            this.panelControl1.Controls.Add(this.ConsignorCompany);
            this.panelControl1.Controls.Add(this.PaymentMode);
            this.panelControl1.Controls.Add(this.BelongWeb);
            this.panelControl1.Controls.Add(this.label8);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Controls.Add(this.ConsignorName);
            this.panelControl1.Controls.Add(this.label2);
            this.panelControl1.Controls.Add(this.label3);
            this.panelControl1.Location = new System.Drawing.Point(12, 12);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(506, 192);
            this.panelControl1.TabIndex = 359;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label11.Location = new System.Drawing.Point(263, 108);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(74, 21);
            this.label11.TabIndex = 370;
            this.label11.Text = "付款方式";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label10.Location = new System.Drawing.Point(263, 67);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 21);
            this.label10.TabIndex = 369;
            this.label10.Text = "联系手机";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ConsignorPhone
            // 
            this.ConsignorPhone.EnterMoveNextControl = true;
            this.ConsignorPhone.Location = new System.Drawing.Point(343, 67);
            this.ConsignorPhone.Name = "ConsignorPhone";
            this.ConsignorPhone.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.ConsignorPhone.Properties.Appearance.Options.UseForeColor = true;
            this.ConsignorPhone.Size = new System.Drawing.Size(135, 21);
            this.ConsignorPhone.TabIndex = 368;
            // 
            // ConsignorCellPhone
            // 
            this.ConsignorCellPhone.EnterMoveNextControl = true;
            this.ConsignorCellPhone.Location = new System.Drawing.Point(84, 69);
            this.ConsignorCellPhone.Name = "ConsignorCellPhone";
            this.ConsignorCellPhone.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.ConsignorCellPhone.Properties.Appearance.Options.UseForeColor = true;
            this.ConsignorCellPhone.Size = new System.Drawing.Size(139, 21);
            this.ConsignorCellPhone.TabIndex = 367;
            // 
            // ConsignorCompany
            // 
            this.ConsignorCompany.EnterMoveNextControl = true;
            this.ConsignorCompany.Location = new System.Drawing.Point(84, 29);
            this.ConsignorCompany.Name = "ConsignorCompany";
            this.ConsignorCompany.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.ConsignorCompany.Properties.Appearance.Options.UseForeColor = true;
            this.ConsignorCompany.Size = new System.Drawing.Size(139, 21);
            this.ConsignorCompany.TabIndex = 366;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label8.Location = new System.Drawing.Point(8, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 21);
            this.label8.TabIndex = 356;
            this.label8.Text = "联系电话";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label1.Location = new System.Drawing.Point(8, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 21);
            this.label1.TabIndex = 26;
            this.label1.Text = "发货单位";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ConsignorName
            // 
            this.ConsignorName.EnterMoveNextControl = true;
            this.ConsignorName.Location = new System.Drawing.Point(343, 29);
            this.ConsignorName.Name = "ConsignorName";
            this.ConsignorName.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.ConsignorName.Properties.Appearance.Options.UseForeColor = true;
            this.ConsignorName.Size = new System.Drawing.Size(135, 21);
            this.ConsignorName.TabIndex = 353;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label2.Location = new System.Drawing.Point(247, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 21);
            this.label2.TabIndex = 344;
            this.label2.Text = "发货联系人";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label3.Location = new System.Drawing.Point(5, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 21);
            this.label3.TabIndex = 346;
            this.label3.Text = "所属部门";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("宋体", 12F);
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(159, 217);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.ShowToolTips = false;
            this.simpleButton1.Size = new System.Drawing.Size(76, 25);
            this.simpleButton1.TabIndex = 358;
            this.simpleButton1.Text = "保存";
            this.simpleButton1.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Hand;
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // btnClose
            // 
            this.btnClose.Appearance.Font = new System.Drawing.Font("宋体", 12F);
            this.btnClose.Appearance.Options.UseFont = true;
            this.btnClose.Image = global::ZQTMS.UI.Properties.Resources.Action_Close;
            this.btnClose.Location = new System.Drawing.Point(327, 217);
            this.btnClose.Name = "btnClose";
            this.btnClose.ShowToolTips = false;
            this.btnClose.Size = new System.Drawing.Size(70, 25);
            this.btnClose.TabIndex = 357;
            this.btnClose.Text = "取消";
            this.btnClose.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Hand;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmMinChargeAdd
            // 
            this.ClientSize = new System.Drawing.Size(528, 254);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.btnClose);
            this.Name = "frmMinChargeAdd";
            this.Text = "编辑";
            this.Load += new System.EventHandler(this.frmMinChargeAdd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PaymentMode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BelongWeb.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConsignorPhone.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConsignorCellPhone.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConsignorCompany.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConsignorName.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.ComboBoxEdit PaymentMode;
        private DevExpress.XtraEditors.ComboBoxEdit BelongWeb;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit ConsignorName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.TextEdit ConsignorCellPhone;
        private DevExpress.XtraEditors.TextEdit ConsignorCompany;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private DevExpress.XtraEditors.TextEdit ConsignorPhone;
    }
}
