namespace ZQTMS.UI.BaseInfoManage
{
    partial class frmCostManageAdd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCostManageAdd));
            this.ProjectName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.TargetcostRate = new DevExpress.XtraEditors.TextEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.ChargePerson = new System.Windows.Forms.TextBox();
            this.MiddleWebName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.SendWebname = new DevExpress.XtraEditors.ComboBoxEdit();
            this.SiteName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.DestinationSite = new DevExpress.XtraEditors.ComboBoxEdit();
            this.TelPhone = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ProjectName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TargetcostRate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MiddleWebName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SendWebname.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SiteName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DestinationSite.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // ProjectName
            // 
            this.ProjectName.EditValue = "全部";
            this.ProjectName.Location = new System.Drawing.Point(164, 16);
            this.ProjectName.Name = "ProjectName";
            this.ProjectName.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.ProjectName.Properties.Appearance.Options.UseFont = true;
            this.ProjectName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ProjectName.Properties.Items.AddRange(new object[] {
            "汽运成本率",
            "送货成本率",
            "中转成本率"});
            this.ProjectName.Size = new System.Drawing.Size(179, 26);
            this.ProjectName.TabIndex = 27;
            this.ProjectName.SelectedIndexChanged += new System.EventHandler(this.ProjectName_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label1.Location = new System.Drawing.Point(84, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 21);
            this.label1.TabIndex = 26;
            this.label1.Text = "项目类型";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("宋体", 12F);
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(143, 480);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.ShowToolTips = false;
            this.simpleButton1.Size = new System.Drawing.Size(76, 25);
            this.simpleButton1.TabIndex = 343;
            this.simpleButton1.Text = "保存";
            this.simpleButton1.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Hand;
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // btnClose
            // 
            this.btnClose.Appearance.Font = new System.Drawing.Font("宋体", 12F);
            this.btnClose.Appearance.Options.UseFont = true;
            this.btnClose.Image = global::ZQTMS.UI.Properties.Resources.Action_Close;
            this.btnClose.Location = new System.Drawing.Point(281, 480);
            this.btnClose.Name = "btnClose";
            this.btnClose.ShowToolTips = false;
            this.btnClose.Size = new System.Drawing.Size(70, 25);
            this.btnClose.TabIndex = 342;
            this.btnClose.Text = "取消";
            this.btnClose.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Hand;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label2.Location = new System.Drawing.Point(100, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 21);
            this.label2.TabIndex = 344;
            this.label2.Text = "始发站";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label3.Location = new System.Drawing.Point(101, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 21);
            this.label3.TabIndex = 346;
            this.label3.Text = "目的站";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label4.Location = new System.Drawing.Point(84, 153);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 21);
            this.label4.TabIndex = 348;
            this.label4.Text = "送货网点";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label5.Location = new System.Drawing.Point(84, 194);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 21);
            this.label5.TabIndex = 350;
            this.label5.Text = "中转网点";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // TargetcostRate
            // 
            this.TargetcostRate.EnterMoveNextControl = true;
            this.TargetcostRate.Location = new System.Drawing.Point(160, 240);
            this.TargetcostRate.Name = "TargetcostRate";
            this.TargetcostRate.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.TargetcostRate.Properties.Appearance.Options.UseForeColor = true;
            this.TargetcostRate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.TargetcostRate.Size = new System.Drawing.Size(183, 21);
            this.TargetcostRate.TabIndex = 353;
            this.TargetcostRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TargetcostRate_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label6.Location = new System.Drawing.Point(69, 238);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 21);
            this.label6.TabIndex = 352;
            this.label6.Text = "目标成本率";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label7.Location = new System.Drawing.Point(96, 282);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 21);
            this.label7.TabIndex = 354;
            this.label7.Text = "负责人";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.ChargePerson);
            this.panelControl1.Controls.Add(this.MiddleWebName);
            this.panelControl1.Controls.Add(this.SendWebname);
            this.panelControl1.Controls.Add(this.SiteName);
            this.panelControl1.Controls.Add(this.DestinationSite);
            this.panelControl1.Controls.Add(this.TelPhone);
            this.panelControl1.Controls.Add(this.richTextBox1);
            this.panelControl1.Controls.Add(this.label9);
            this.panelControl1.Controls.Add(this.label8);
            this.panelControl1.Controls.Add(this.ProjectName);
            this.panelControl1.Controls.Add(this.label7);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Controls.Add(this.label6);
            this.panelControl1.Controls.Add(this.TargetcostRate);
            this.panelControl1.Controls.Add(this.label2);
            this.panelControl1.Controls.Add(this.label5);
            this.panelControl1.Controls.Add(this.label3);
            this.panelControl1.Controls.Add(this.label4);
            this.panelControl1.Location = new System.Drawing.Point(12, 13);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(459, 449);
            this.panelControl1.TabIndex = 356;
            // 
            // ChargePerson
            // 
            this.ChargePerson.Location = new System.Drawing.Point(160, 284);
            this.ChargePerson.Name = "ChargePerson";
            this.ChargePerson.Size = new System.Drawing.Size(183, 22);
            this.ChargePerson.TabIndex = 365;
            // 
            // MiddleWebName
            // 
            this.MiddleWebName.EditValue = "全部";
            this.MiddleWebName.Location = new System.Drawing.Point(164, 192);
            this.MiddleWebName.Name = "MiddleWebName";
            this.MiddleWebName.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.MiddleWebName.Properties.Appearance.Options.UseFont = true;
            this.MiddleWebName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.MiddleWebName.Size = new System.Drawing.Size(179, 26);
            this.MiddleWebName.TabIndex = 364;
            // 
            // SendWebname
            // 
            this.SendWebname.EditValue = "全部";
            this.SendWebname.Location = new System.Drawing.Point(164, 151);
            this.SendWebname.Name = "SendWebname";
            this.SendWebname.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.SendWebname.Properties.Appearance.Options.UseFont = true;
            this.SendWebname.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SendWebname.Size = new System.Drawing.Size(179, 26);
            this.SendWebname.TabIndex = 363;
            // 
            // SiteName
            // 
            this.SiteName.EditValue = "全部";
            this.SiteName.Location = new System.Drawing.Point(164, 60);
            this.SiteName.Name = "SiteName";
            this.SiteName.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.SiteName.Properties.Appearance.Options.UseFont = true;
            this.SiteName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SiteName.Size = new System.Drawing.Size(179, 26);
            this.SiteName.TabIndex = 362;
            // 
            // DestinationSite
            // 
            this.DestinationSite.EditValue = "全部";
            this.DestinationSite.Location = new System.Drawing.Point(164, 104);
            this.DestinationSite.Name = "DestinationSite";
            this.DestinationSite.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.DestinationSite.Properties.Appearance.Options.UseFont = true;
            this.DestinationSite.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DestinationSite.Size = new System.Drawing.Size(179, 26);
            this.DestinationSite.TabIndex = 361;
            // 
            // TelPhone
            // 
            this.TelPhone.Location = new System.Drawing.Point(160, 328);
            this.TelPhone.Name = "TelPhone";
            this.TelPhone.Size = new System.Drawing.Size(183, 22);
            this.TelPhone.TabIndex = 360;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(160, 369);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(183, 59);
            this.richTextBox1.TabIndex = 359;
            this.richTextBox1.Text = "";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label9.Location = new System.Drawing.Point(105, 379);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 21);
            this.label9.TabIndex = 358;
            this.label9.Text = "备注";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label8.Location = new System.Drawing.Point(96, 326);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 21);
            this.label8.TabIndex = 356;
            this.label8.Text = "手机号";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // frmCostManageAdd
            // 
            this.ClientSize = new System.Drawing.Size(484, 517);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.btnClose);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCostManageAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "编辑成本管控标准";
            this.Load += new System.EventHandler(this.frmCostManageAdd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ProjectName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TargetcostRate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MiddleWebName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SendWebname.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SiteName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DestinationSite.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.ComboBoxEdit ProjectName;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.TextEdit TargetcostRate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox TelPhone;
        private DevExpress.XtraEditors.ComboBoxEdit DestinationSite;
        private DevExpress.XtraEditors.ComboBoxEdit SiteName;
        private DevExpress.XtraEditors.ComboBoxEdit SendWebname;
        private System.Windows.Forms.TextBox ChargePerson;
        private DevExpress.XtraEditors.ComboBoxEdit MiddleWebName;
    }
}
