namespace ZQTMS.UI
{
    partial class frmAddJob
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.JobName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.JobInstruc = new DevExpress.XtraEditors.MemoEdit();
            this.JobRemark = new DevExpress.XtraEditors.MemoEdit();
            this.Company = new DevExpress.XtraEditors.LabelControl();
            this.CompanyID = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.JobName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.JobInstruc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.JobRemark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CompanyID.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(46, 192);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(24, 14);
            this.labelControl1.TabIndex = 314;
            this.labelControl1.Text = "备注";
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(22, 55);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(48, 14);
            this.labelControl8.TabIndex = 312;
            this.labelControl8.Text = "岗位说明";
            // 
            // JobName
            // 
            this.JobName.Location = new System.Drawing.Point(78, 12);
            this.JobName.Name = "JobName";
            this.JobName.Size = new System.Drawing.Size(252, 21);
            this.JobName.TabIndex = 309;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(22, 12);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(48, 14);
            this.labelControl2.TabIndex = 310;
            this.labelControl2.Text = "岗位名称";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(107, 298);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 315;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(233, 298);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 316;
            this.btnCancel.Text = "退出";
            this.btnCancel.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // JobInstruc
            // 
            this.JobInstruc.Location = new System.Drawing.Point(78, 52);
            this.JobInstruc.Name = "JobInstruc";
            this.JobInstruc.Size = new System.Drawing.Size(252, 85);
            this.JobInstruc.TabIndex = 311;
            this.JobInstruc.TabStop = false;
            // 
            // JobRemark
            // 
            this.JobRemark.Location = new System.Drawing.Point(78, 189);
            this.JobRemark.Name = "JobRemark";
            this.JobRemark.Size = new System.Drawing.Size(252, 103);
            this.JobRemark.TabIndex = 313;
            this.JobRemark.TabStop = false;
            // 
            // Company
            // 
            this.Company.Location = new System.Drawing.Point(34, 155);
            this.Company.Name = "Company";
            this.Company.Size = new System.Drawing.Size(36, 14);
            this.Company.TabIndex = 349;
            this.Company.Text = "公司ID";
            // 
            // CompanyID
            // 
            this.CompanyID.Location = new System.Drawing.Point(78, 152);
            this.CompanyID.Name = "CompanyID";
            this.CompanyID.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CompanyID.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.CompanyID.Size = new System.Drawing.Size(252, 21);
            this.CompanyID.TabIndex = 350;
            // 
            // frmAddJob
            // 
            this.ClientSize = new System.Drawing.Size(367, 342);
            this.Controls.Add(this.CompanyID);
            this.Controls.Add(this.Company);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.JobName);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.JobInstruc);
            this.Controls.Add(this.JobRemark);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddJob";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmAddJob_Load);
            ((System.ComponentModel.ISupportInitialize)(this.JobName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.JobInstruc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.JobRemark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CompanyID.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.TextEdit JobName;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.MemoEdit JobInstruc;
        private DevExpress.XtraEditors.MemoEdit JobRemark;
        private DevExpress.XtraEditors.LabelControl Company;
        private DevExpress.XtraEditors.ComboBoxEdit CompanyID;
    }
}
