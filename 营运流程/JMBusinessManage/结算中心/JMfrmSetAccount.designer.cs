namespace ZQTMS.UI
{
    partial class JMfrmSetAccount
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
            this.NegwarnValue = new DevExpress.XtraEditors.TextEdit();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.LoanWarnValue = new DevExpress.XtraEditors.TextEdit();
            this.AccountReserved = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.AccountType = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.BankName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.BankAccountName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.BankAccountNO = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.NegwarnValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoanWarnValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AccountReserved.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AccountType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BankName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BankAccountName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BankAccountNO.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // NegwarnValue
            // 
            this.NegwarnValue.Location = new System.Drawing.Point(109, 203);
            this.NegwarnValue.Name = "NegwarnValue";
            this.NegwarnValue.Properties.Mask.EditMask = "f3";
            this.NegwarnValue.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.NegwarnValue.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.NegwarnValue.Size = new System.Drawing.Size(226, 21);
            this.NegwarnValue.TabIndex = 2;
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(42, 208);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(60, 14);
            this.labelControl8.TabIndex = 302;
            this.labelControl8.Text = "负数预警值";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(42, 246);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(60, 14);
            this.labelControl6.TabIndex = 300;
            this.labelControl6.Text = "贷款预警值";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(42, 166);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 296;
            this.labelControl2.Text = "账户预留额";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(235, 308);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "退出";
            this.btnCancel.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(109, 308);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // LoanWarnValue
            // 
            this.LoanWarnValue.Location = new System.Drawing.Point(109, 244);
            this.LoanWarnValue.Name = "LoanWarnValue";
            this.LoanWarnValue.Properties.Mask.EditMask = "f3";
            this.LoanWarnValue.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.LoanWarnValue.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.LoanWarnValue.Size = new System.Drawing.Size(226, 21);
            this.LoanWarnValue.TabIndex = 3;
            // 
            // AccountReserved
            // 
            this.AccountReserved.Location = new System.Drawing.Point(109, 164);
            this.AccountReserved.Name = "AccountReserved";
            this.AccountReserved.Properties.Mask.EditMask = "f3";
            this.AccountReserved.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.AccountReserved.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.AccountReserved.Size = new System.Drawing.Size(226, 21);
            this.AccountReserved.TabIndex = 1;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(42, 25);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 304;
            this.labelControl1.Text = "账户类型";
            // 
            // AccountType
            // 
            this.AccountType.Location = new System.Drawing.Point(109, 22);
            this.AccountType.Name = "AccountType";
            this.AccountType.Size = new System.Drawing.Size(226, 21);
            this.AccountType.TabIndex = 303;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(42, 57);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 306;
            this.labelControl3.Text = "银行名称";
            // 
            // BankName
            // 
            this.BankName.Location = new System.Drawing.Point(109, 55);
            this.BankName.Name = "BankName";
            this.BankName.Size = new System.Drawing.Size(226, 21);
            this.BankName.TabIndex = 305;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(42, 93);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 14);
            this.labelControl4.TabIndex = 308;
            this.labelControl4.Text = "银行开户人";
            // 
            // BankAccountName
            // 
            this.BankAccountName.Location = new System.Drawing.Point(109, 91);
            this.BankAccountName.Name = "BankAccountName";
            this.BankAccountName.Size = new System.Drawing.Size(226, 21);
            this.BankAccountName.TabIndex = 307;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(42, 128);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(48, 14);
            this.labelControl5.TabIndex = 310;
            this.labelControl5.Text = "银行账号";
            // 
            // BankAccountNO
            // 
            this.BankAccountNO.Location = new System.Drawing.Point(109, 126);
            this.BankAccountNO.Name = "BankAccountNO";
            this.BankAccountNO.Size = new System.Drawing.Size(226, 21);
            this.BankAccountNO.TabIndex = 309;
            // 
            // JMfrmSetAccount
            // 
            this.ClientSize = new System.Drawing.Size(389, 383);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.BankAccountNO);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.BankAccountName);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.BankName);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.AccountType);
            this.Controls.Add(this.LoanWarnValue);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.NegwarnValue);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.AccountReserved);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "JMfrmSetAccount";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "账户设置";
            this.Load += new System.EventHandler(this.frmAddPart_Load);
            ((System.ComponentModel.ISupportInitialize)(this.NegwarnValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoanWarnValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AccountReserved.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AccountType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BankName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BankAccountName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BankAccountNO.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit NegwarnValue;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.TextEdit LoanWarnValue;
        private DevExpress.XtraEditors.TextEdit AccountReserved;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit AccountType;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit BankName;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit BankAccountName;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.TextEdit BankAccountNO;
    }
}
