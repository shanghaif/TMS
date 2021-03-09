namespace ZQTMS.UI
{
    partial class frmBasAccount_add
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBasAccount_add));
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.label5 = new System.Windows.Forms.Label();
            this.StartSite = new DevExpress.XtraEditors.ComboBoxEdit();
            this.AccountName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.AccountNO = new DevExpress.XtraEditors.ComboBoxEdit();
            this.CompanyID = new DevExpress.XtraEditors.ComboBoxEdit();
            this.ToMiddle = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label7 = new System.Windows.Forms.Label();
            this.UserName = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.feeDedu = new DevExpress.XtraEditors.CheckEdit();
            this.ckIsEnable = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.StartSite.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AccountName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AccountNO.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CompanyID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToMiddle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.feeDedu.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckIsEnable.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(32, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 14);
            this.label3.TabIndex = 102;
            this.label3.Text = "始发站";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(301, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 14);
            this.label1.TabIndex = 104;
            this.label1.Text = "中转地";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(21, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 14);
            this.label2.TabIndex = 106;
            this.label2.Text = "账户名称";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(300, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 14);
            this.label4.TabIndex = 108;
            this.label4.Text = "公司ID";
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(304, 165);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(75, 24);
            this.simpleButton2.TabIndex = 110;
            this.simpleButton2.Text = "取   消";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(163, 165);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 24);
            this.simpleButton1.TabIndex = 109;
            this.simpleButton1.Text = "保  存";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(21, 99);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 14);
            this.label5.TabIndex = 115;
            this.label5.Text = "账户编码";
            // 
            // StartSite
            // 
            this.StartSite.Location = new System.Drawing.Point(83, 32);
            this.StartSite.Name = "StartSite";
            this.StartSite.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.StartSite.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.StartSite.Size = new System.Drawing.Size(175, 21);
            this.StartSite.TabIndex = 349;
            // 
            // AccountName
            // 
            this.AccountName.Location = new System.Drawing.Point(83, 64);
            this.AccountName.Name = "AccountName";
            this.AccountName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.AccountName.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.AccountName.Size = new System.Drawing.Size(175, 21);
            this.AccountName.TabIndex = 350;
            this.AccountName.SelectedValueChanged += new System.EventHandler(this.AccountName_SelectedValueChanged_1);
            // 
            // AccountNO
            // 
            this.AccountNO.Location = new System.Drawing.Point(83, 96);
            this.AccountNO.Name = "AccountNO";
            this.AccountNO.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.AccountNO.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.AccountNO.Size = new System.Drawing.Size(177, 21);
            this.AccountNO.TabIndex = 351;
            // 
            // CompanyID
            // 
            this.CompanyID.Location = new System.Drawing.Point(350, 64);
            this.CompanyID.Name = "CompanyID";
            this.CompanyID.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CompanyID.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.CompanyID.Size = new System.Drawing.Size(180, 21);
            this.CompanyID.TabIndex = 353;
            // 
            // ToMiddle
            // 
            this.ToMiddle.Location = new System.Drawing.Point(350, 32);
            this.ToMiddle.Name = "ToMiddle";
            this.ToMiddle.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ToMiddle.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.ToMiddle.Size = new System.Drawing.Size(180, 21);
            this.ToMiddle.TabIndex = 352;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(288, 99);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 14);
            this.label7.TabIndex = 358;
            this.label7.Text = "用户姓名";
            // 
            // UserName
            // 
            this.UserName.EnterMoveNextControl = true;
            this.UserName.Location = new System.Drawing.Point(350, 96);
            this.UserName.Name = "UserName";
            this.UserName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.UserName.Properties.DropDownRows = 20;
            this.UserName.Size = new System.Drawing.Size(180, 21);
            this.UserName.TabIndex = 359;
            // 
            // feeDedu
            // 
            this.feeDedu.EditValue = true;
            this.feeDedu.Location = new System.Drawing.Point(77, 131);
            this.feeDedu.Name = "feeDedu";
            this.feeDedu.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.feeDedu.Properties.Caption = "是否自动扣取结算费";
            this.feeDedu.Size = new System.Drawing.Size(143, 19);
            this.feeDedu.TabIndex = 360;
            this.feeDedu.TabStop = false;
            // 
            // ckIsEnable
            // 
            this.ckIsEnable.EditValue = true;
            this.ckIsEnable.Location = new System.Drawing.Point(299, 132);
            this.ckIsEnable.Name = "ckIsEnable";
            this.ckIsEnable.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.ckIsEnable.Properties.Caption = "是否启用";
            this.ckIsEnable.Size = new System.Drawing.Size(143, 19);
            this.ckIsEnable.TabIndex = 361;
            this.ckIsEnable.TabStop = false;
            // 
            // frmBasAccount_add
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 214);
            this.Controls.Add(this.ckIsEnable);
            this.Controls.Add(this.feeDedu);
            this.Controls.Add(this.UserName);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.CompanyID);
            this.Controls.Add(this.ToMiddle);
            this.Controls.Add(this.AccountNO);
            this.Controls.Add(this.AccountName);
            this.Controls.Add(this.StartSite);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmBasAccount_add";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "新增账户";
            this.Load += new System.EventHandler(this.frmBasAccount_add_Load);
            ((System.ComponentModel.ISupportInitialize)(this.StartSite.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AccountName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AccountNO.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CompanyID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToMiddle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.feeDedu.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckIsEnable.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.ComboBoxEdit StartSite;
        private DevExpress.XtraEditors.ComboBoxEdit AccountName;
        private DevExpress.XtraEditors.ComboBoxEdit AccountNO;
        private DevExpress.XtraEditors.ComboBoxEdit CompanyID;
        private DevExpress.XtraEditors.ComboBoxEdit ToMiddle;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.CheckedComboBoxEdit UserName;
        private DevExpress.XtraEditors.CheckEdit feeDedu;
        private DevExpress.XtraEditors.CheckEdit ckIsEnable;
    }
}