namespace ZQTMS.UI
{
    partial class JMfrmAddSettlementAccount
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
            this.BankAccountName = new DevExpress.XtraEditors.TextEdit();
            this.BankAccount = new DevExpress.XtraEditors.TextEdit();
            this.BankAddress = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.BankName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.CauseName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.BankAccountName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BankAccount.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BankAddress.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BankName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CauseName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // BankAccountName
            // 
            this.BankAccountName.Location = new System.Drawing.Point(108, 191);
            this.BankAccountName.Name = "BankAccountName";
            this.BankAccountName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BankAccountName.Properties.Appearance.Options.UseFont = true;
            this.BankAccountName.Size = new System.Drawing.Size(255, 21);
            this.BankAccountName.TabIndex = 323;
            // 
            // BankAccount
            // 
            this.BankAccount.Location = new System.Drawing.Point(108, 146);
            this.BankAccount.Name = "BankAccount";
            this.BankAccount.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BankAccount.Properties.Appearance.Options.UseFont = true;
            this.BankAccount.Size = new System.Drawing.Size(255, 21);
            this.BankAccount.TabIndex = 322;
            // 
            // BankAddress
            // 
            this.BankAddress.Location = new System.Drawing.Point(108, 236);
            this.BankAddress.Name = "BankAddress";
            this.BankAddress.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BankAddress.Properties.Appearance.Options.UseFont = true;
            this.BankAddress.Size = new System.Drawing.Size(255, 21);
            this.BankAddress.TabIndex = 324;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(42, 239);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 328;
            this.labelControl1.Text = "开户行地址";
            // 
            // BankName
            // 
            this.BankName.Location = new System.Drawing.Point(108, 101);
            this.BankName.Name = "BankName";
            this.BankName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BankName.Properties.Appearance.Options.UseFont = true;
            this.BankName.Size = new System.Drawing.Size(255, 21);
            this.BankName.TabIndex = 321;
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(54, 104);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(48, 14);
            this.labelControl8.TabIndex = 327;
            this.labelControl8.Text = "银行名称";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(54, 149);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(48, 14);
            this.labelControl6.TabIndex = 326;
            this.labelControl6.Text = "银行账号";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(54, 194);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(48, 14);
            this.labelControl5.TabIndex = 325;
            this.labelControl5.Text = "银行户名";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(54, 52);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(48, 14);
            this.labelControl2.TabIndex = 330;
            this.labelControl2.Text = "账户名称";
            // 
            // CauseName
            // 
            this.CauseName.Enabled = false;
            this.CauseName.Location = new System.Drawing.Point(108, 49);
            this.CauseName.Name = "CauseName";
            this.CauseName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CauseName.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.CauseName.Size = new System.Drawing.Size(255, 21);
            this.CauseName.TabIndex = 329;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(234, 295);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 332;
            this.btnCancel.Text = "退出";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(108, 295);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 331;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // JMfrmAddSettlementAccount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 378);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.CauseName);
            this.Controls.Add(this.BankAccountName);
            this.Controls.Add(this.BankAccount);
            this.Controls.Add(this.BankAddress);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.BankName);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.labelControl5);
            this.Name = "JMfrmAddSettlementAccount";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "修改结算中心账户";
            this.Load += new System.EventHandler(this.JMfrmAddSettlementAccount_Load);
            ((System.ComponentModel.ISupportInitialize)(this.BankAccountName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BankAccount.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BankAddress.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BankName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CauseName.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit BankAccountName;
        private DevExpress.XtraEditors.TextEdit BankAccount;
        private DevExpress.XtraEditors.TextEdit BankAddress;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit BankName;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ComboBoxEdit CauseName;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
    }
}