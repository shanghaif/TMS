namespace ZQTMS.UI.BaseInfoManage
{
    partial class frmAddPlatAccount
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddPlatAccount));
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.DBName = new DevExpress.XtraEditors.TextEdit();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.isEnable = new DevExpress.XtraEditors.CheckEdit();
            this.isKKuan = new DevExpress.XtraEditors.CheckEdit();
            this.userName = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.accNo = new DevExpress.XtraEditors.ComboBoxEdit();
            this.accName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.mSite = new DevExpress.XtraEditors.ComboBoxEdit();
            this.bSite = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.companyID = new DevExpress.XtraEditors.TextEdit();
            this.btnSubmit = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.accountName = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.DBName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.isEnable.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.isKKuan.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mSite.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bSite.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.companyID.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(47, 117);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(48, 14);
            this.labelControl5.TabIndex = 410;
            this.labelControl5.Text = "数据库名";
            this.labelControl5.Visible = false;
            // 
            // DBName
            // 
            this.DBName.Location = new System.Drawing.Point(97, 114);
            this.DBName.Name = "DBName";
            this.DBName.Properties.ReadOnly = true;
            this.DBName.Size = new System.Drawing.Size(141, 21);
            this.DBName.TabIndex = 409;
            this.DBName.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(314, 176);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 408;
            this.btnCancel.Text = "关  闭";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // isEnable
            // 
            this.isEnable.EditValue = true;
            this.isEnable.Location = new System.Drawing.Point(331, 141);
            this.isEnable.Name = "isEnable";
            this.isEnable.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.isEnable.Properties.Caption = "是否启用";
            this.isEnable.Size = new System.Drawing.Size(143, 19);
            this.isEnable.TabIndex = 407;
            this.isEnable.TabStop = false;
            this.isEnable.Visible = false;
            // 
            // isKKuan
            // 
            this.isKKuan.EditValue = true;
            this.isKKuan.Location = new System.Drawing.Point(70, 141);
            this.isKKuan.Name = "isKKuan";
            this.isKKuan.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.isKKuan.Properties.Caption = "是否自动扣取结算费";
            this.isKKuan.Size = new System.Drawing.Size(143, 19);
            this.isKKuan.TabIndex = 406;
            this.isKKuan.TabStop = false;
            this.isKKuan.Visible = false;
            // 
            // userName
            // 
            this.userName.EnterMoveNextControl = true;
            this.userName.Location = new System.Drawing.Point(333, 90);
            this.userName.Name = "userName";
            this.userName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.userName.Properties.DropDownRows = 20;
            this.userName.Size = new System.Drawing.Size(141, 21);
            this.userName.TabIndex = 405;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(279, 93);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(48, 14);
            this.labelControl4.TabIndex = 404;
            this.labelControl4.Text = "用户姓名";
            // 
            // accNo
            // 
            this.accNo.Location = new System.Drawing.Point(333, 57);
            this.accNo.Name = "accNo";
            this.accNo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.accNo.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.accNo.Size = new System.Drawing.Size(141, 21);
            this.accNo.TabIndex = 403;
            // 
            // accName
            // 
            this.accName.Location = new System.Drawing.Point(97, 57);
            this.accName.Name = "accName";
            this.accName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.accName.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.accName.Size = new System.Drawing.Size(141, 21);
            this.accName.TabIndex = 402;
            this.accName.SelectedIndexChanged += new System.EventHandler(this.accName_SelectedIndexChanged);
            // 
            // mSite
            // 
            this.mSite.Location = new System.Drawing.Point(333, 35);
            this.mSite.Name = "mSite";
            this.mSite.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.mSite.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.mSite.Size = new System.Drawing.Size(141, 21);
            this.mSite.TabIndex = 401;
            this.mSite.Visible = false;
            // 
            // bSite
            // 
            this.bSite.Location = new System.Drawing.Point(97, 35);
            this.bSite.Name = "bSite";
            this.bSite.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.bSite.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.bSite.Size = new System.Drawing.Size(141, 21);
            this.bSite.TabIndex = 400;
            this.bSite.Visible = false;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(47, 93);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(44, 14);
            this.labelControl2.TabIndex = 399;
            this.labelControl2.Text = "公 司 ID";
            // 
            // companyID
            // 
            this.companyID.Location = new System.Drawing.Point(97, 90);
            this.companyID.Name = "companyID";
            this.companyID.Properties.ReadOnly = true;
            this.companyID.Size = new System.Drawing.Size(141, 21);
            this.companyID.TabIndex = 398;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(178, 176);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 397;
            this.btnSubmit.Text = "保  存";
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(283, 38);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(44, 14);
            this.labelControl7.TabIndex = 396;
            this.labelControl7.Text = "中 转 站";
            this.labelControl7.Visible = false;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(267, 61);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(60, 14);
            this.labelControl3.TabIndex = 395;
            this.labelControl3.Text = "账 户 编 号";
            // 
            // accountName
            // 
            this.accountName.Location = new System.Drawing.Point(31, 60);
            this.accountName.Name = "accountName";
            this.accountName.Size = new System.Drawing.Size(60, 14);
            this.accountName.TabIndex = 394;
            this.accountName.Text = "账 户 名 称";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(47, 38);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(44, 14);
            this.labelControl1.TabIndex = 393;
            this.labelControl1.Text = "始 发 站";
            this.labelControl1.Visible = false;
            // 
            // frmAddPlatAccount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 249);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.DBName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.isEnable);
            this.Controls.Add(this.isKKuan);
            this.Controls.Add(this.userName);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.accNo);
            this.Controls.Add(this.accName);
            this.Controls.Add(this.mSite);
            this.Controls.Add(this.bSite);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.companyID);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.accountName);
            this.Controls.Add(this.labelControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmAddPlatAccount";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "新增/修改";
            this.Load += new System.EventHandler(this.frmAddPlatAccount_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DBName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.isEnable.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.isKKuan.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mSite.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bSite.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.companyID.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.TextEdit DBName;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.CheckEdit isEnable;
        private DevExpress.XtraEditors.CheckEdit isKKuan;
        private DevExpress.XtraEditors.CheckedComboBoxEdit userName;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.ComboBoxEdit accNo;
        private DevExpress.XtraEditors.ComboBoxEdit accName;
        private DevExpress.XtraEditors.ComboBoxEdit mSite;
        private DevExpress.XtraEditors.ComboBoxEdit bSite;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit companyID;
        private DevExpress.XtraEditors.SimpleButton btnSubmit;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl accountName;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}