namespace ZQTMS.UI
{
    partial class frmLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            this.UserAccount = new DevExpress.XtraEditors.TextEdit();
            this.UserPsw = new DevExpress.XtraEditors.TextEdit();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.lbmsg = new System.Windows.Forms.Label();
            this.Companyid = new DevExpress.XtraEditors.TextEdit();
            this.cbRetrieve = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.UserAccount.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserPsw.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Companyid.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // UserAccount
            // 
            this.UserAccount.EditValue = "";
            this.UserAccount.EnterMoveNextControl = true;
            this.UserAccount.Location = new System.Drawing.Point(250, 212);
            this.UserAccount.Name = "UserAccount";
            this.UserAccount.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.UserAccount.Properties.Appearance.Font = new System.Drawing.Font("宋体", 12F);
            this.UserAccount.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.UserAccount.Properties.Appearance.Options.UseBackColor = true;
            this.UserAccount.Properties.Appearance.Options.UseFont = true;
            this.UserAccount.Properties.Appearance.Options.UseForeColor = true;
            this.UserAccount.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.UserAccount.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.UserAccount.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.UserAccount.Size = new System.Drawing.Size(186, 23);
            this.UserAccount.TabIndex = 1;
            // 
            // UserPsw
            // 
            this.UserPsw.EditValue = "";
            this.UserPsw.Location = new System.Drawing.Point(250, 257);
            this.UserPsw.Name = "UserPsw";
            this.UserPsw.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.UserPsw.Properties.Appearance.Font = new System.Drawing.Font("宋体", 12F);
            this.UserPsw.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.UserPsw.Properties.Appearance.Options.UseBackColor = true;
            this.UserPsw.Properties.Appearance.Options.UseFont = true;
            this.UserPsw.Properties.Appearance.Options.UseForeColor = true;
            this.UserPsw.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.UserPsw.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.UserPsw.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.UserPsw.Properties.PasswordChar = '*';
            this.UserPsw.Size = new System.Drawing.Size(186, 23);
            this.UserPsw.TabIndex = 2;
            this.UserPsw.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UserPsw_KeyDown);
            // 
            // button1
            // 
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.Location = new System.Drawing.Point(414, 97);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 31);
            this.button1.TabIndex = 3;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // button2
            // 
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.Location = new System.Drawing.Point(414, 60);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(101, 31);
            this.button2.TabIndex = 4;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // lbmsg
            // 
            this.lbmsg.AutoSize = true;
            this.lbmsg.BackColor = System.Drawing.Color.Transparent;
            this.lbmsg.ForeColor = System.Drawing.Color.Blue;
            this.lbmsg.Location = new System.Drawing.Point(32, 319);
            this.lbmsg.Name = "lbmsg";
            this.lbmsg.Size = new System.Drawing.Size(67, 14);
            this.lbmsg.TabIndex = 5;
            this.lbmsg.Text = "准备就绪...";
            this.lbmsg.Visible = false;
            // 
            // Companyid
            // 
            this.Companyid.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Companyid.EditValue = "";
            this.Companyid.EnterMoveNextControl = true;
            this.Companyid.Location = new System.Drawing.Point(250, 166);
            this.Companyid.Name = "Companyid";
            this.Companyid.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.Companyid.Properties.Appearance.Font = new System.Drawing.Font("宋体", 12F);
            this.Companyid.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.Companyid.Properties.Appearance.Options.UseBackColor = true;
            this.Companyid.Properties.Appearance.Options.UseFont = true;
            this.Companyid.Properties.Appearance.Options.UseForeColor = true;
            this.Companyid.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.Companyid.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.Companyid.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.Companyid.Size = new System.Drawing.Size(186, 23);
            this.Companyid.TabIndex = 0;
            // 
            // cbRetrieve
            // 
            this.cbRetrieve.Appearance.Font = new System.Drawing.Font("宋体", 12F);
            this.cbRetrieve.Appearance.Options.UseFont = true;
            this.cbRetrieve.Image = ((System.Drawing.Image)(resources.GetObject("cbRetrieve.Image")));
            this.cbRetrieve.Location = new System.Drawing.Point(391, 319);
            this.cbRetrieve.Name = "cbRetrieve";
            this.cbRetrieve.ShowToolTips = false;
            this.cbRetrieve.Size = new System.Drawing.Size(85, 35);
            this.cbRetrieve.TabIndex = 32;
            this.cbRetrieve.Text = "登 录";
            this.cbRetrieve.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Hand;
            this.cbRetrieve.ToolTipTitle = "帮助";
            this.cbRetrieve.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("宋体", 12F);
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(483, 319);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.ShowToolTips = false;
            this.simpleButton1.Size = new System.Drawing.Size(85, 35);
            this.simpleButton1.TabIndex = 33;
            this.simpleButton1.Text = "退 出";
            this.simpleButton1.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Hand;
            this.simpleButton1.ToolTipTitle = "帮助";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayoutStore = System.Windows.Forms.ImageLayout.Tile;
            this.BackgroundImageStore = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImageStore")));
            this.ClientSize = new System.Drawing.Size(594, 394);
            this.ControlBox = false;
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.cbRetrieve);
            this.Controls.Add(this.Companyid);
            this.Controls.Add(this.lbmsg);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.UserPsw);
            this.Controls.Add(this.UserAccount);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmLogin_Load);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmLogin_MouseUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmLogin_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmLogin_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.UserAccount.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserPsw.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Companyid.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit UserAccount;
        private DevExpress.XtraEditors.TextEdit UserPsw;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label lbmsg;
        private DevExpress.XtraEditors.TextEdit Companyid;
        private DevExpress.XtraEditors.SimpleButton cbRetrieve;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}