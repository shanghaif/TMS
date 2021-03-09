namespace ZQTMS.UI
{
    partial class FrmWalletAlterPwd
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
            this.txtOldPwd = new DevExpress.XtraEditors.TextEdit();
            this.label47 = new System.Windows.Forms.Label();
            this.txtNewPwd = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPwdConfirm = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_alter = new DevExpress.XtraEditors.SimpleButton();
            this.btn_cancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtOldPwd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNewPwd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPwdConfirm.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtOldPwd
            // 
            this.txtOldPwd.Location = new System.Drawing.Point(91, 34);
            this.txtOldPwd.Name = "txtOldPwd";
            this.txtOldPwd.Properties.PasswordChar = '*';
            this.txtOldPwd.Size = new System.Drawing.Size(148, 21);
            this.txtOldPwd.TabIndex = 251;
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(47, 39);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(43, 14);
            this.label47.TabIndex = 252;
            this.label47.Text = "密码：";
            // 
            // txtNewPwd
            // 
            this.txtNewPwd.Location = new System.Drawing.Point(91, 70);
            this.txtNewPwd.Name = "txtNewPwd";
            this.txtNewPwd.Properties.PasswordChar = '*';
            this.txtNewPwd.Size = new System.Drawing.Size(148, 21);
            this.txtNewPwd.TabIndex = 253;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 14);
            this.label1.TabIndex = 254;
            this.label1.Text = "新密码：";
            // 
            // txtPwdConfirm
            // 
            this.txtPwdConfirm.Location = new System.Drawing.Point(91, 105);
            this.txtPwdConfirm.Name = "txtPwdConfirm";
            this.txtPwdConfirm.Properties.PasswordChar = '*';
            this.txtPwdConfirm.Size = new System.Drawing.Size(148, 21);
            this.txtPwdConfirm.TabIndex = 255;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 14);
            this.label2.TabIndex = 256;
            this.label2.Text = "确认密码：";
            // 
            // btn_alter
            // 
            this.btn_alter.Location = new System.Drawing.Point(80, 150);
            this.btn_alter.Name = "btn_alter";
            this.btn_alter.Size = new System.Drawing.Size(64, 29);
            this.btn_alter.TabIndex = 257;
            this.btn_alter.Text = "确认修改";
            this.btn_alter.Click += new System.EventHandler(this.btn_alter_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(176, 149);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(64, 29);
            this.btn_cancel.TabIndex = 258;
            this.btn_cancel.Text = "取消";
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // FrmWalletAlterPwd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_alter);
            this.Controls.Add(this.txtPwdConfirm);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtNewPwd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtOldPwd);
            this.Controls.Add(this.label47);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmWalletAlterPwd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "修改密码";
            ((System.ComponentModel.ISupportInitialize)(this.txtOldPwd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNewPwd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPwdConfirm.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtOldPwd;
        private System.Windows.Forms.Label label47;
        private DevExpress.XtraEditors.TextEdit txtNewPwd;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit txtPwdConfirm;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.SimpleButton btn_alter;
        private DevExpress.XtraEditors.SimpleButton btn_cancel;
    }
}