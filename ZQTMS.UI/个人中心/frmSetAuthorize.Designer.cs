namespace ZQTMS.UI
{
    partial class frmSetAuthorize
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
            this.UserAccount = new DevExpress.XtraEditors.TextEdit();
            this.Authorize = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.UserAccount.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Authorize.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // UserAccount
            // 
            this.UserAccount.Location = new System.Drawing.Point(99, 25);
            this.UserAccount.Name = "UserAccount";
            this.UserAccount.Properties.ReadOnly = true;
            this.UserAccount.Size = new System.Drawing.Size(207, 21);
            this.UserAccount.TabIndex = 2;
            this.UserAccount.TabStop = false;
            // 
            // Authorize
            // 
            this.Authorize.Location = new System.Drawing.Point(99, 78);
            this.Authorize.Name = "Authorize";
            this.Authorize.Size = new System.Drawing.Size(207, 21);
            this.Authorize.TabIndex = 3;
            // 
            // simpleButton1
            // 
            this.simpleButton1.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.simpleButton1.Location = new System.Drawing.Point(99, 139);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 23);
            this.simpleButton1.TabIndex = 4;
            this.simpleButton1.Text = "确定";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.DialogResult = System.Windows.Forms.DialogResult.No;
            this.simpleButton2.Location = new System.Drawing.Point(231, 139);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(75, 23);
            this.simpleButton2.TabIndex = 5;
            this.simpleButton2.Text = "取消";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 14);
            this.label1.TabIndex = 6;
            this.label1.Text = "登录帐号";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 14);
            this.label2.TabIndex = 7;
            this.label2.Text = "授 权 码";
            // 
            // frmSetAuthorize
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(345, 192);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.Authorize);
            this.Controls.Add(this.UserAccount);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSetAuthorize";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "授权码设置";
            this.Load += new System.EventHandler(this.frmSetAuthorize_Load);
            ((System.ComponentModel.ISupportInitialize)(this.UserAccount.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Authorize.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.TextEdit UserAccount;
        private DevExpress.XtraEditors.TextEdit Authorize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}