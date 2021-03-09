namespace ZQTMS.UI
{
    partial class frmEmployeeAdd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEmployeeAdd));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.EmpName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.EmpNO = new DevExpress.XtraEditors.TextEdit();
            this.EmpDept = new DevExpress.XtraEditors.ComboBoxEdit();
            this.EmpCuase = new DevExpress.XtraEditors.ComboBoxEdit();
            this.EmpPosition = new DevExpress.XtraEditors.ComboBoxEdit();
            this.EmpID = new DevExpress.XtraEditors.TextEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.EmpPost = new DevExpress.XtraEditors.ComboBoxEdit();
            this.EmpArea = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.EmpSite = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.EmpWeb = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.EmpName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmpNO.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmpDept.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmpCuase.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmpPosition.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmpID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmpPost.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmpArea.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmpSite.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmpWeb.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 384);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 322;
            this.labelControl1.Text = "所属站点";
            this.labelControl1.Visible = false;
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(12, 91);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(48, 14);
            this.labelControl8.TabIndex = 320;
            this.labelControl8.Text = "员工工号";
            // 
            // EmpName
            // 
            this.EmpName.Location = new System.Drawing.Point(68, 54);
            this.EmpName.Name = "EmpName";
            this.EmpName.Size = new System.Drawing.Size(281, 21);
            this.EmpName.TabIndex = 317;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(12, 57);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(48, 14);
            this.labelControl2.TabIndex = 318;
            this.labelControl2.Text = "员工姓名";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(205, 305);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 324;
            this.btnCancel.Text = "退出";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(98, 305);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 323;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(12, 196);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 325;
            this.labelControl3.Text = "所属部门";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(12, 232);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(48, 14);
            this.labelControl4.TabIndex = 326;
            this.labelControl4.Text = "员工岗位";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(12, 268);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(48, 14);
            this.labelControl5.TabIndex = 327;
            this.labelControl5.Text = "员工职务";
            // 
            // EmpNO
            // 
            this.EmpNO.Location = new System.Drawing.Point(68, 88);
            this.EmpNO.Name = "EmpNO";
            this.EmpNO.Size = new System.Drawing.Size(281, 21);
            this.EmpNO.TabIndex = 328;
            // 
            // EmpDept
            // 
            this.EmpDept.Location = new System.Drawing.Point(68, 193);
            this.EmpDept.Name = "EmpDept";
            this.EmpDept.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.EmpDept.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.EmpDept.Size = new System.Drawing.Size(281, 21);
            this.EmpDept.TabIndex = 332;
            // 
            // EmpCuase
            // 
            this.EmpCuase.Enabled = false;
            this.EmpCuase.Location = new System.Drawing.Point(68, 125);
            this.EmpCuase.Name = "EmpCuase";
            this.EmpCuase.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.EmpCuase.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.EmpCuase.Size = new System.Drawing.Size(281, 21);
            this.EmpCuase.TabIndex = 333;
            this.EmpCuase.EditValueChanged += new System.EventHandler(this.EmpCuase_EditValueChanged);
            // 
            // EmpPosition
            // 
            this.EmpPosition.Location = new System.Drawing.Point(68, 229);
            this.EmpPosition.Name = "EmpPosition";
            this.EmpPosition.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.EmpPosition.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.EmpPosition.Size = new System.Drawing.Size(281, 21);
            this.EmpPosition.TabIndex = 334;
            // 
            // EmpID
            // 
            this.EmpID.Enabled = false;
            this.EmpID.Location = new System.Drawing.Point(68, 20);
            this.EmpID.Name = "EmpID";
            this.EmpID.Size = new System.Drawing.Size(281, 21);
            this.EmpID.TabIndex = 335;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(12, 23);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(48, 14);
            this.labelControl6.TabIndex = 336;
            this.labelControl6.Text = "系统编号";
            // 
            // EmpPost
            // 
            this.EmpPost.Location = new System.Drawing.Point(68, 265);
            this.EmpPost.Name = "EmpPost";
            this.EmpPost.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.EmpPost.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.EmpPost.Size = new System.Drawing.Size(281, 21);
            this.EmpPost.TabIndex = 337;
            // 
            // EmpArea
            // 
            this.EmpArea.Location = new System.Drawing.Point(68, 159);
            this.EmpArea.Name = "EmpArea";
            this.EmpArea.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.EmpArea.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.EmpArea.Size = new System.Drawing.Size(281, 21);
            this.EmpArea.TabIndex = 339;
            this.EmpArea.SelectedIndexChanged += new System.EventHandler(this.EmpArea_SelectedIndexChanged);
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(24, 128);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(36, 14);
            this.labelControl7.TabIndex = 338;
            this.labelControl7.Text = "事业部";
            // 
            // EmpSite
            // 
            this.EmpSite.Location = new System.Drawing.Point(68, 381);
            this.EmpSite.Name = "EmpSite";
            this.EmpSite.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.EmpSite.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.EmpSite.Size = new System.Drawing.Size(281, 21);
            this.EmpSite.TabIndex = 341;
            this.EmpSite.Visible = false;
            this.EmpSite.SelectedIndexChanged += new System.EventHandler(this.EmpSite_SelectedIndexChanged);
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(12, 163);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(48, 14);
            this.labelControl9.TabIndex = 340;
            this.labelControl9.Text = "所属大区";
            // 
            // EmpWeb
            // 
            this.EmpWeb.Location = new System.Drawing.Point(68, 412);
            this.EmpWeb.Name = "EmpWeb";
            this.EmpWeb.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.EmpWeb.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.EmpWeb.Size = new System.Drawing.Size(281, 21);
            this.EmpWeb.TabIndex = 343;
            this.EmpWeb.Visible = false;
            // 
            // labelControl10
            // 
            this.labelControl10.Location = new System.Drawing.Point(12, 415);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(48, 14);
            this.labelControl10.TabIndex = 342;
            this.labelControl10.Text = "所属网点";
            this.labelControl10.Visible = false;
            // 
            // frmEmployeeAdd
            // 
            this.ClientSize = new System.Drawing.Size(372, 343);
            this.Controls.Add(this.EmpWeb);
            this.Controls.Add(this.labelControl10);
            this.Controls.Add(this.EmpSite);
            this.Controls.Add(this.labelControl9);
            this.Controls.Add(this.EmpArea);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.EmpPost);
            this.Controls.Add(this.EmpID);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.EmpPosition);
            this.Controls.Add(this.EmpCuase);
            this.Controls.Add(this.EmpDept);
            this.Controls.Add(this.EmpNO);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.EmpName);
            this.Controls.Add(this.labelControl2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEmployeeAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "员工";
            this.Load += new System.EventHandler(this.frmAddTitle_Load);
            ((System.ComponentModel.ISupportInitialize)(this.EmpName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmpNO.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmpDept.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmpCuase.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmpPosition.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmpID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmpPost.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmpArea.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmpSite.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmpWeb.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.TextEdit EmpName;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.TextEdit EmpNO;
        private DevExpress.XtraEditors.ComboBoxEdit EmpDept;
        private DevExpress.XtraEditors.ComboBoxEdit EmpCuase;
        private DevExpress.XtraEditors.ComboBoxEdit EmpPosition;
        private DevExpress.XtraEditors.TextEdit EmpID;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.ComboBoxEdit EmpPost;
        private DevExpress.XtraEditors.ComboBoxEdit EmpArea;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.ComboBoxEdit EmpSite;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.ComboBoxEdit EmpWeb;
        private DevExpress.XtraEditors.LabelControl labelControl10;
    }
}
