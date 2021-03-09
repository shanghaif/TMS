namespace ZQTMS.UI
{
    partial class frmDePartAdd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDePartAdd));
            this.DepName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.DepPhone = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.DepRemark = new DevExpress.XtraEditors.MemoEdit();
            this.DepCode = new DevExpress.XtraEditors.TextEdit();
            this.DepMan = new DevExpress.XtraEditors.TextEdit();
            this.DepArea = new DevExpress.XtraEditors.ComboBoxEdit();
            this.DepRight = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.labelControl15 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.DepName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DepPhone.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DepRemark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DepCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DepMan.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DepArea.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DepRight.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // DepName
            // 
            this.DepName.Location = new System.Drawing.Point(106, 50);
            this.DepName.Name = "DepName";
            this.DepName.Size = new System.Drawing.Size(226, 21);
            this.DepName.TabIndex = 2;
            //this.DepName.EditValueChanged += new System.EventHandler(this.DepName_EditValueChanged);
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(52, 56);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(48, 14);
            this.labelControl8.TabIndex = 302;
            this.labelControl8.Text = "部门名称";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(52, 93);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(48, 14);
            this.labelControl6.TabIndex = 300;
            this.labelControl6.Text = "部门代码";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(64, 128);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(36, 14);
            this.labelControl5.TabIndex = 299;
            this.labelControl5.Text = "负责人";
            // 
            // DepPhone
            // 
            this.DepPhone.Location = new System.Drawing.Point(106, 157);
            this.DepPhone.Name = "DepPhone";
            this.DepPhone.Size = new System.Drawing.Size(226, 21);
            this.DepPhone.TabIndex = 5;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(40, 163);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 14);
            this.labelControl4.TabIndex = 298;
            this.labelControl4.Text = "负责人电话";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(52, 18);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(48, 14);
            this.labelControl2.TabIndex = 296;
            this.labelControl2.Text = "所属大区";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(76, 226);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(24, 14);
            this.labelControl1.TabIndex = 308;
            this.labelControl1.Text = "备注";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(232, 293);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "退出";
            this.btnCancel.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(106, 293);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // DepRemark
            // 
            this.DepRemark.Location = new System.Drawing.Point(106, 191);
            this.DepRemark.Name = "DepRemark";
            this.DepRemark.Size = new System.Drawing.Size(226, 86);
            this.DepRemark.TabIndex = 6;
            this.DepRemark.TabStop = false;
            // 
            // DepCode
            // 
            this.DepCode.Location = new System.Drawing.Point(106, 90);
            this.DepCode.Name = "DepCode";
            this.DepCode.Size = new System.Drawing.Size(226, 21);
            this.DepCode.TabIndex = 3;
            // 
            // DepMan
            // 
            this.DepMan.Location = new System.Drawing.Point(106, 125);
            this.DepMan.Name = "DepMan";
            this.DepMan.Size = new System.Drawing.Size(226, 21);
            this.DepMan.TabIndex = 4;
            // 
            // DepArea
            // 
            this.DepArea.Location = new System.Drawing.Point(106, 15);
            this.DepArea.Name = "DepArea";
            this.DepArea.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DepArea.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.DepArea.Size = new System.Drawing.Size(226, 21);
            this.DepArea.TabIndex = 1;
            // 
            // DepRight
            // 
            this.DepRight.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.DepRight.Location = new System.Drawing.Point(121, 342);
            this.DepRight.Name = "DepRight";
            this.DepRight.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DepRight.Properties.DropDownRows = 10;
            this.DepRight.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.ImageComboBoxItem[] {
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("只能查询自己网点", 0, -1),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("可查询所有事业部", 1, -1),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("可查询所有大区", 2, -1),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("可查询所有网点", 3, -1)});
            this.DepRight.Size = new System.Drawing.Size(226, 21);
            this.DepRight.TabIndex = 333;
            this.DepRight.Visible = false;
            // 
            // labelControl15
            // 
            this.labelControl15.Location = new System.Drawing.Point(66, 345);
            this.labelControl15.Name = "labelControl15";
            this.labelControl15.Size = new System.Drawing.Size(48, 14);
            this.labelControl15.TabIndex = 332;
            this.labelControl15.Text = "部门权限";
            this.labelControl15.Visible = false;
            // 
            // frmDePartAdd
            // 
            this.ClientSize = new System.Drawing.Size(377, 375);
            this.Controls.Add(this.DepRight);
            this.Controls.Add(this.labelControl15);
            this.Controls.Add(this.DepMan);
            this.Controls.Add(this.DepCode);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.DepName);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.DepPhone);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.DepRemark);
            this.Controls.Add(this.DepArea);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDePartAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "部门";
            this.Load += new System.EventHandler(this.frmDePartAdd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DepName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DepPhone.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DepRemark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DepCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DepMan.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DepArea.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DepRight.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit DepName;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.TextEdit DepPhone;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.MemoEdit DepRemark;
        private DevExpress.XtraEditors.TextEdit DepCode;
        private DevExpress.XtraEditors.TextEdit DepMan;
        private DevExpress.XtraEditors.ComboBoxEdit DepArea;
        private DevExpress.XtraEditors.ImageComboBoxEdit DepRight;
        private DevExpress.XtraEditors.LabelControl labelControl15;
    }
}
