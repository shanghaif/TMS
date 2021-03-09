namespace ZQTMS.UI
{
    partial class frmAddCause
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
            this.CauseCode = new DevExpress.XtraEditors.TextEdit();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.CauseName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.CauseRemark = new DevExpress.XtraEditors.MemoEdit();
            this.CauseMan = new DevExpress.XtraEditors.TextEdit();
            this.CausePhone = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.userDB = new DevExpress.XtraEditors.ComboBoxEdit();
            this.Company = new DevExpress.XtraEditors.LabelControl();
            this.CompanyID = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.CauseCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CauseName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CauseRemark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CauseMan.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CausePhone.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userDB.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CompanyID.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // CauseCode
            // 
            this.CauseCode.Location = new System.Drawing.Point(77, 50);
            this.CauseCode.Name = "CauseCode";
            this.CauseCode.Size = new System.Drawing.Size(217, 21);
            this.CauseCode.TabIndex = 2;
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(47, 55);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(24, 14);
            this.labelControl8.TabIndex = 278;
            this.labelControl8.Text = "代码";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(35, 91);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(36, 14);
            this.labelControl6.TabIndex = 276;
            this.labelControl6.Text = "负责人";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(11, 127);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(60, 14);
            this.labelControl5.TabIndex = 275;
            this.labelControl5.Text = "负责人电话";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(47, 186);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(24, 14);
            this.labelControl4.TabIndex = 274;
            this.labelControl4.Text = "备注";
            // 
            // CauseName
            // 
            this.CauseName.Location = new System.Drawing.Point(77, 15);
            this.CauseName.Name = "CauseName";
            this.CauseName.Size = new System.Drawing.Size(217, 21);
            this.CauseName.TabIndex = 2;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(47, 20);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(24, 14);
            this.labelControl2.TabIndex = 272;
            this.labelControl2.Text = "名称";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(82, 276);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(61, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(213, 276);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(61, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "退出";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // CauseRemark
            // 
            this.CauseRemark.Location = new System.Drawing.Point(77, 186);
            this.CauseRemark.Name = "CauseRemark";
            this.CauseRemark.Size = new System.Drawing.Size(217, 74);
            this.CauseRemark.TabIndex = 5;
            this.CauseRemark.TabStop = false;
            // 
            // CauseMan
            // 
            this.CauseMan.Location = new System.Drawing.Point(77, 88);
            this.CauseMan.Name = "CauseMan";
            this.CauseMan.Size = new System.Drawing.Size(217, 21);
            this.CauseMan.TabIndex = 3;
            // 
            // CausePhone
            // 
            this.CausePhone.Location = new System.Drawing.Point(77, 124);
            this.CausePhone.Name = "CausePhone";
            this.CausePhone.Size = new System.Drawing.Size(217, 21);
            this.CausePhone.TabIndex = 4;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(348, 248);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 279;
            this.labelControl1.Text = "登录环境";
            this.labelControl1.Visible = false;
            // 
            // userDB
            // 
            this.userDB.EditValue = "ZQTMSLEY";
            this.userDB.Location = new System.Drawing.Point(402, 245);
            this.userDB.Name = "userDB";
            this.userDB.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.userDB.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.userDB.Size = new System.Drawing.Size(113, 21);
            this.userDB.TabIndex = 280;
            this.userDB.Visible = false;
            // 
            // Company
            // 
            this.Company.Location = new System.Drawing.Point(33, 160);
            this.Company.Name = "Company";
            this.Company.Size = new System.Drawing.Size(36, 14);
            this.Company.TabIndex = 345;
            this.Company.Text = "公司ID";
            // 
            // CompanyID
            // 
            this.CompanyID.Location = new System.Drawing.Point(75, 157);
            this.CompanyID.Name = "CompanyID";
            this.CompanyID.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CompanyID.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.CompanyID.Size = new System.Drawing.Size(220, 21);
            this.CompanyID.TabIndex = 347;
            // 
            // frmAddCause
            // 
            this.ClientSize = new System.Drawing.Size(307, 309);
            this.Controls.Add(this.CompanyID);
            this.Controls.Add(this.Company);
            this.Controls.Add(this.userDB);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.CausePhone);
            this.Controls.Add(this.CauseMan);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.CauseCode);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.CauseName);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.CauseRemark);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddCause";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "事业部";
            this.Load += new System.EventHandler(this.frmAddCause_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CauseCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CauseName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CauseRemark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CauseMan.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CausePhone.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userDB.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CompanyID.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit CauseCode;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit CauseName;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.MemoEdit CauseRemark;
        private DevExpress.XtraEditors.TextEdit CauseMan;
        private DevExpress.XtraEditors.TextEdit CausePhone;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit userDB;
        private DevExpress.XtraEditors.LabelControl Company;
        private DevExpress.XtraEditors.ComboBoxEdit CompanyID;
    }
}
