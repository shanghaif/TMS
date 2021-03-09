namespace ZQTMS.UI
{
    partial class frmSettleCenterInsuranceAccListAdd
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.gsjc = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.accountId = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.checkEdit1 = new DevExpress.XtraEditors.CheckEdit();
            this.gsqc = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.gsjc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gsqc.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(235, 199);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "退出";
            this.btnCancel.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(109, 199);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(42, 25);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 304;
            this.labelControl1.Text = "公司名称";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(42, 57);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 306;
            this.labelControl3.Text = "公司简称";
            // 
            // gsjc
            // 
            this.gsjc.Location = new System.Drawing.Point(109, 55);
            this.gsjc.Name = "gsjc";
            this.gsjc.Size = new System.Drawing.Size(226, 21);
            this.gsjc.TabIndex = 305;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(42, 93);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(48, 14);
            this.labelControl4.TabIndex = 308;
            this.labelControl4.Text = "账户编号";
            // 
            // accountId
            // 
            this.accountId.Location = new System.Drawing.Point(109, 91);
            this.accountId.Name = "accountId";
            this.accountId.Properties.ReadOnly = true;
            this.accountId.Size = new System.Drawing.Size(226, 21);
            this.accountId.TabIndex = 307;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(42, 128);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(48, 14);
            this.labelControl5.TabIndex = 310;
            this.labelControl5.Text = "是否启用";
            // 
            // checkEdit1
            // 
            this.checkEdit1.Location = new System.Drawing.Point(109, 128);
            this.checkEdit1.Name = "checkEdit1";
            this.checkEdit1.Properties.Caption = "";
            this.checkEdit1.Size = new System.Drawing.Size(22, 19);
            this.checkEdit1.TabIndex = 311;
            // 
            // gsqc
            // 
            this.gsqc.Location = new System.Drawing.Point(109, 22);
            this.gsqc.Name = "gsqc";
            this.gsqc.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gsqc.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.gsqc.Size = new System.Drawing.Size(226, 21);
            this.gsqc.TabIndex = 312;
            // 
            // frmSettleCenterInsuranceAccListAdd
            // 
            this.ClientSize = new System.Drawing.Size(389, 264);
            this.Controls.Add(this.gsqc);
            this.Controls.Add(this.checkEdit1);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.accountId);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.gsjc);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSettleCenterInsuranceAccListAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "账户设置";
            this.Load += new System.EventHandler(this.frmAddPart_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gsjc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gsqc.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit gsjc;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit accountId;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.CheckEdit checkEdit1;
        private DevExpress.XtraEditors.ComboBoxEdit gsqc;
    }
}
