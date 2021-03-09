namespace ZQTMS.UI
{
    partial class frmAddTitle
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
            this.TitName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.TitInstruc = new DevExpress.XtraEditors.MemoEdit();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.TitRemark = new DevExpress.XtraEditors.MemoEdit();
            this.Company = new DevExpress.XtraEditors.LabelControl();
            this.CompanyID = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.TitName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TitInstruc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TitRemark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CompanyID.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(36, 196);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(24, 14);
            this.labelControl1.TabIndex = 322;
            this.labelControl1.Text = "备注";
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(12, 53);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(48, 14);
            this.labelControl8.TabIndex = 320;
            this.labelControl8.Text = "岗位说明";
            // 
            // TitName
            // 
            this.TitName.Location = new System.Drawing.Point(68, 23);
            this.TitName.Name = "TitName";
            this.TitName.Size = new System.Drawing.Size(281, 21);
            this.TitName.TabIndex = 317;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(12, 23);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(48, 14);
            this.labelControl2.TabIndex = 318;
            this.labelControl2.Text = "岗位名称";
            // 
            // TitInstruc
            // 
            this.TitInstruc.Location = new System.Drawing.Point(68, 50);
            this.TitInstruc.Name = "TitInstruc";
            this.TitInstruc.Size = new System.Drawing.Size(281, 95);
            this.TitInstruc.TabIndex = 319;
            this.TitInstruc.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(206, 315);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 324;
            this.btnCancel.Text = "退出";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(108, 315);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 323;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // TitRemark
            // 
            this.TitRemark.Location = new System.Drawing.Point(68, 193);
            this.TitRemark.Name = "TitRemark";
            this.TitRemark.Size = new System.Drawing.Size(281, 107);
            this.TitRemark.TabIndex = 321;
            this.TitRemark.TabStop = false;
            // 
            // Company
            // 
            this.Company.Location = new System.Drawing.Point(24, 164);
            this.Company.Name = "Company";
            this.Company.Size = new System.Drawing.Size(36, 14);
            this.Company.TabIndex = 351;
            this.Company.Text = "公司ID";
            // 
            // CompanyID
            // 
            this.CompanyID.Location = new System.Drawing.Point(68, 161);
            this.CompanyID.Name = "CompanyID";
            this.CompanyID.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CompanyID.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.CompanyID.Size = new System.Drawing.Size(281, 21);
            this.CompanyID.TabIndex = 352;
            // 
            // frmAddTitle
            // 
            this.ClientSize = new System.Drawing.Size(377, 359);
            this.Controls.Add(this.CompanyID);
            this.Controls.Add(this.Company);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.TitName);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.TitInstruc);
            this.Controls.Add(this.TitRemark);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddTitle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmAddTitle_Load);
            ((System.ComponentModel.ISupportInitialize)(this.TitName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TitInstruc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TitRemark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CompanyID.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.TextEdit TitName;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.MemoEdit TitInstruc;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.MemoEdit TitRemark;
        private DevExpress.XtraEditors.LabelControl Company;
        private DevExpress.XtraEditors.ComboBoxEdit CompanyID;
    }
}
