namespace ZQTMS.UI
{
    partial class frmUsersValidate
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
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.UserAccount = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.ValidationInfo = new DevExpress.XtraEditors.MemoEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.UserAccount.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ValidationInfo.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(202, 179);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 322;
            this.btnCancel.Text = "退出";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(92, 179);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 321;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(37, 19);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(48, 14);
            this.labelControl8.TabIndex = 295;
            this.labelControl8.Text = "登录账号";
            // 
            // UserAccount
            // 
            this.UserAccount.Location = new System.Drawing.Point(92, 16);
            this.UserAccount.Name = "UserAccount";
            this.UserAccount.Properties.ReadOnly = true;
            this.UserAccount.Size = new System.Drawing.Size(365, 21);
            this.UserAccount.TabIndex = 1;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(37, 63);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 323;
            this.labelControl1.Text = "验证信息";
            // 
            // ValidationInfo
            // 
            this.ValidationInfo.Location = new System.Drawing.Point(92, 63);
            this.ValidationInfo.Name = "ValidationInfo";
            this.ValidationInfo.Size = new System.Drawing.Size(365, 73);
            this.ValidationInfo.TabIndex = 324;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl2.Appearance.Options.UseForeColor = true;
            this.labelControl2.Location = new System.Drawing.Point(92, 146);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(182, 14);
            this.labelControl2.TabIndex = 325;
            this.labelControl2.Text = "*多个验证信息，用英文逗号分隔*";
            // 
            // frmUsersValidate
            // 
            this.ClientSize = new System.Drawing.Size(532, 233);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.ValidationInfo);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.UserAccount);
            this.Controls.Add(this.labelControl8);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUsersValidate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "用户验证信息";
            this.Load += new System.EventHandler(this.frmAddUsers_Load);
            ((System.ComponentModel.ISupportInitialize)(this.UserAccount.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ValidationInfo.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.TextEdit UserAccount;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.MemoEdit ValidationInfo;
        private DevExpress.XtraEditors.LabelControl labelControl2;
    }
}
