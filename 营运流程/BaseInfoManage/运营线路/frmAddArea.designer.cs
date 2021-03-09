namespace ZQTMS.UI
{
    partial class frmAddArea
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
            this.AreaName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.AreaRemark = new DevExpress.XtraEditors.MemoEdit();
            this.AreaPhone = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.AreaCode = new DevExpress.XtraEditors.TextEdit();
            this.AreaMan = new DevExpress.XtraEditors.TextEdit();
            this.AreaCause = new DevExpress.XtraEditors.ComboBoxEdit();
            this.Company = new DevExpress.XtraEditors.LabelControl();
            this.CompanyID = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.AreaName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AreaRemark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AreaPhone.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AreaCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AreaMan.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AreaCause.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CompanyID.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // AreaName
            // 
            this.AreaName.Location = new System.Drawing.Point(91, 47);
            this.AreaName.Name = "AreaName";
            this.AreaName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AreaName.Properties.Appearance.Options.UseFont = true;
            this.AreaName.Size = new System.Drawing.Size(255, 21);
            this.AreaName.TabIndex = 2;
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(37, 50);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(48, 14);
            this.labelControl8.TabIndex = 290;
            this.labelControl8.Text = "区域名称";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(37, 87);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(48, 14);
            this.labelControl6.TabIndex = 288;
            this.labelControl6.Text = "区域代码";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(25, 122);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(60, 14);
            this.labelControl5.TabIndex = 287;
            this.labelControl5.Text = "区域负责人";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(61, 224);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(24, 14);
            this.labelControl4.TabIndex = 286;
            this.labelControl4.Text = "备注";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(25, 12);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 284;
            this.labelControl2.Text = "所属事业部";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(238, 305);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 318;
            this.btnCancel.Text = "退出";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(112, 305);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 317;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // AreaRemark
            // 
            this.AreaRemark.Location = new System.Drawing.Point(91, 222);
            this.AreaRemark.Name = "AreaRemark";
            this.AreaRemark.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AreaRemark.Properties.Appearance.Options.UseFont = true;
            this.AreaRemark.Size = new System.Drawing.Size(255, 70);
            this.AreaRemark.TabIndex = 6;
            this.AreaRemark.TabStop = false;
            // 
            // AreaPhone
            // 
            this.AreaPhone.Location = new System.Drawing.Point(91, 154);
            this.AreaPhone.Name = "AreaPhone";
            this.AreaPhone.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AreaPhone.Properties.Appearance.Options.UseFont = true;
            this.AreaPhone.Size = new System.Drawing.Size(255, 21);
            this.AreaPhone.TabIndex = 5;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(1, 155);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(84, 14);
            this.labelControl1.TabIndex = 320;
            this.labelControl1.Text = "区域负责人电话";
            // 
            // AreaCode
            // 
            this.AreaCode.Location = new System.Drawing.Point(91, 83);
            this.AreaCode.Name = "AreaCode";
            this.AreaCode.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AreaCode.Properties.Appearance.Options.UseFont = true;
            this.AreaCode.Size = new System.Drawing.Size(255, 21);
            this.AreaCode.TabIndex = 3;
            // 
            // AreaMan
            // 
            this.AreaMan.Location = new System.Drawing.Point(91, 117);
            this.AreaMan.Name = "AreaMan";
            this.AreaMan.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AreaMan.Properties.Appearance.Options.UseFont = true;
            this.AreaMan.Size = new System.Drawing.Size(255, 21);
            this.AreaMan.TabIndex = 4;
            // 
            // AreaCause
            // 
            this.AreaCause.Location = new System.Drawing.Point(91, 9);
            this.AreaCause.Name = "AreaCause";
            this.AreaCause.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.AreaCause.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.AreaCause.Size = new System.Drawing.Size(255, 21);
            this.AreaCause.TabIndex = 1;
            // 
            // Company
            // 
            this.Company.Location = new System.Drawing.Point(47, 194);
            this.Company.Name = "Company";
            this.Company.Size = new System.Drawing.Size(36, 14);
            this.Company.TabIndex = 345;
            this.Company.Text = "公司ID";
            // 
            // CompanyID
            // 
            this.CompanyID.Location = new System.Drawing.Point(91, 191);
            this.CompanyID.Name = "CompanyID";
            this.CompanyID.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CompanyID.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.CompanyID.Size = new System.Drawing.Size(255, 21);
            this.CompanyID.TabIndex = 348;
            // 
            // frmAddArea
            // 
            this.ClientSize = new System.Drawing.Size(383, 352);
            this.Controls.Add(this.CompanyID);
            this.Controls.Add(this.Company);
            this.Controls.Add(this.AreaMan);
            this.Controls.Add(this.AreaCode);
            this.Controls.Add(this.AreaPhone);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.AreaName);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.AreaRemark);
            this.Controls.Add(this.AreaCause);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddArea";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmAddArea_Load);
            ((System.ComponentModel.ISupportInitialize)(this.AreaName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AreaRemark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AreaPhone.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AreaCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AreaMan.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AreaCause.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CompanyID.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit AreaName;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.MemoEdit AreaRemark;
        private DevExpress.XtraEditors.TextEdit AreaPhone;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit AreaCode;
        private DevExpress.XtraEditors.TextEdit AreaMan;
        private DevExpress.XtraEditors.ComboBoxEdit AreaCause;
        private DevExpress.XtraEditors.LabelControl Company;
        private DevExpress.XtraEditors.ComboBoxEdit CompanyID;
    }
}
