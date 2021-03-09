namespace ZQTMS.UI
{
    partial class ExpenseReimbursementAdd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExpenseReimbursementAdd));
            this.label16 = new System.Windows.Forms.Label();
            this.CauseName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.ApplyDate = new DevExpress.XtraEditors.DateEdit();
            this.label13 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.AreaName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.ApplyDept = new DevExpress.XtraEditors.ComboBoxEdit();
            this.FeeType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.FeeProject = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.BelongMonth = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.Money = new DevExpress.XtraEditors.TextEdit();
            this.AssumeDept = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.Remark = new DevExpress.XtraEditors.MemoEdit();
            this.btnCanccel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.ApplyMan = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.BelongYear = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.CauseName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ApplyDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ApplyDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AreaName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ApplyDept.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FeeType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FeeProject.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BelongMonth.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Money.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AssumeDept.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Remark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ApplyMan.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BelongYear.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(251, 19);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(71, 14);
            this.label16.TabIndex = 123;
            this.label16.Text = "申报事业部:";
            // 
            // CauseName
            // 
            this.CauseName.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.CauseName.Location = new System.Drawing.Point(323, 17);
            this.CauseName.Name = "CauseName";
            this.CauseName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CauseName.Size = new System.Drawing.Size(141, 21);
            this.CauseName.TabIndex = 122;
            this.CauseName.TabStop = false;
            this.CauseName.SelectedIndexChanged += new System.EventHandler(this.CauseName_SelectedIndexChanged);
            // 
            // ApplyDate
            // 
            this.ApplyDate.EditValue = new System.DateTime(2007, 10, 29, 0, 0, 0, 0);
            this.ApplyDate.Enabled = false;
            this.ApplyDate.Location = new System.Drawing.Point(83, 16);
            this.ApplyDate.Name = "ApplyDate";
            this.ApplyDate.Properties.Appearance.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ApplyDate.Properties.Appearance.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.ApplyDate.Properties.Appearance.Options.UseFont = true;
            this.ApplyDate.Properties.Appearance.Options.UseForeColor = true;
            this.ApplyDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ApplyDate.Properties.DisplayFormat.FormatString = "yyyy-M-dd H:mm:ss";
            this.ApplyDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.ApplyDate.Properties.EditFormat.FormatString = "yyyy-M-dd H:mm:ss";
            this.ApplyDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.ApplyDate.Properties.Mask.EditMask = "yyyy-M-dd H:mm:ss";
            this.ApplyDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.ApplyDate.Size = new System.Drawing.Size(141, 22);
            this.ApplyDate.TabIndex = 125;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(21, 18);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(67, 14);
            this.label13.TabIndex = 124;
            this.label13.Text = "申报日期：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(479, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 14);
            this.label1.TabIndex = 127;
            this.label1.Text = "申报大区:";
            // 
            // AreaName
            // 
            this.AreaName.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.AreaName.Location = new System.Drawing.Point(538, 19);
            this.AreaName.Name = "AreaName";
            this.AreaName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.AreaName.Size = new System.Drawing.Size(141, 21);
            this.AreaName.TabIndex = 126;
            this.AreaName.TabStop = false;
            this.AreaName.SelectedIndexChanged += new System.EventHandler(this.AreaName_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 14);
            this.label4.TabIndex = 129;
            this.label4.Text = "申报部门:";
            // 
            // ApplyDept
            // 
            this.ApplyDept.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.ApplyDept.Location = new System.Drawing.Point(82, 44);
            this.ApplyDept.Name = "ApplyDept";
            this.ApplyDept.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ApplyDept.Size = new System.Drawing.Size(141, 21);
            this.ApplyDept.TabIndex = 128;
            this.ApplyDept.TabStop = false;
            this.ApplyDept.SelectedIndexChanged += new System.EventHandler(this.ApplyDept_SelectedIndexChanged);
            // 
            // FeeType
            // 
            this.FeeType.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.FeeType.Location = new System.Drawing.Point(323, 44);
            this.FeeType.Name = "FeeType";
            this.FeeType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FeeType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.FeeType.Size = new System.Drawing.Size(141, 21);
            this.FeeType.TabIndex = 130;
            this.FeeType.TabStop = false;
            this.FeeType.SelectedIndexChanged += new System.EventHandler(this.FeeType_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(251, 46);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 14);
            this.label5.TabIndex = 131;
            this.label5.Text = "费 用 类 型:";
            // 
            // FeeProject
            // 
            this.FeeProject.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.FeeProject.Location = new System.Drawing.Point(539, 44);
            this.FeeProject.Name = "FeeProject";
            this.FeeProject.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FeeProject.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.FeeProject.Properties.Click += new System.EventHandler(this.FeeProject_Properties_Click);
            this.FeeProject.Size = new System.Drawing.Size(141, 21);
            this.FeeProject.TabIndex = 132;
            this.FeeProject.TabStop = false;
            this.FeeProject.SelectedIndexChanged += new System.EventHandler(this.FeeProject_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(479, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 14);
            this.label6.TabIndex = 133;
            this.label6.Text = "项      目:";
            // 
            // BelongMonth
            // 
            this.BelongMonth.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.BelongMonth.Location = new System.Drawing.Point(539, 71);
            this.BelongMonth.Name = "BelongMonth";
            this.BelongMonth.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.BelongMonth.Properties.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12"});
            this.BelongMonth.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.BelongMonth.Size = new System.Drawing.Size(141, 21);
            this.BelongMonth.TabIndex = 134;
            this.BelongMonth.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(479, 74);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 14);
            this.label7.TabIndex = 135;
            this.label7.Text = "所属月份:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(21, 101);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 14);
            this.label8.TabIndex = 136;
            this.label8.Text = "金      额:";
            // 
            // Money
            // 
            this.Money.EnterMoveNextControl = true;
            this.Money.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.Money.Location = new System.Drawing.Point(81, 99);
            this.Money.Name = "Money";
            this.Money.Size = new System.Drawing.Size(141, 21);
            this.Money.TabIndex = 137;
            // 
            // AssumeDept
            // 
            this.AssumeDept.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.AssumeDept.Location = new System.Drawing.Point(81, 71);
            this.AssumeDept.Name = "AssumeDept";
            this.AssumeDept.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.AssumeDept.Size = new System.Drawing.Size(141, 21);
            this.AssumeDept.TabIndex = 138;
            this.AssumeDept.TabStop = false;
            this.AssumeDept.SelectedIndexChanged += new System.EventHandler(this.AssumeDept_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(21, 73);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 14);
            this.label9.TabIndex = 139;
            this.label9.Text = "承担部门:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(29, 129);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(51, 14);
            this.label10.TabIndex = 140;
            this.label10.Text = "摘    要:";
            // 
            // Remark
            // 
            this.Remark.Location = new System.Drawing.Point(81, 127);
            this.Remark.Name = "Remark";
            this.Remark.Properties.MaxLength = 100;
            this.Remark.Size = new System.Drawing.Size(601, 99);
            this.Remark.TabIndex = 141;
            // 
            // btnCanccel
            // 
            this.btnCanccel.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCanccel.Appearance.Options.UseFont = true;
            this.btnCanccel.Image = global::ZQTMS.UI.Properties.Resources.Action_Close;
            this.btnCanccel.Location = new System.Drawing.Point(440, 251);
            this.btnCanccel.Name = "btnCanccel";
            this.btnCanccel.Size = new System.Drawing.Size(75, 23);
            this.btnCanccel.TabIndex = 143;
            this.btnCanccel.Text = "取消";
            this.btnCanccel.Click += new System.EventHandler(this.btnCanccel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Appearance.Options.UseFont = true;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(216, 252);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 142;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ApplyMan
            // 
            this.ApplyMan.Enabled = false;
            this.ApplyMan.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.ApplyMan.Location = new System.Drawing.Point(322, 100);
            this.ApplyMan.Name = "ApplyMan";
            this.ApplyMan.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ApplyMan.Size = new System.Drawing.Size(141, 21);
            this.ApplyMan.TabIndex = 144;
            this.ApplyMan.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(261, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 14);
            this.label2.TabIndex = 145;
            this.label2.Text = "申 报 人:";
            // 
            // BelongYear
            // 
            this.BelongYear.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.BelongYear.Location = new System.Drawing.Point(322, 72);
            this.BelongYear.Name = "BelongYear";
            this.BelongYear.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.BelongYear.Properties.Items.AddRange(new object[] {
            "一月",
            "二月",
            "三月",
            "四月",
            "五月",
            "六月",
            "七月",
            "八月",
            "九月",
            "十月",
            "十一月",
            "十二月"});
            this.BelongYear.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.BelongYear.Size = new System.Drawing.Size(141, 21);
            this.BelongYear.TabIndex = 146;
            this.BelongYear.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(251, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 14);
            this.label3.TabIndex = 147;
            this.label3.Text = "所 属 年 份:";
            // 
            // ExpenseReimbursementAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 292);
            this.Controls.Add(this.BelongYear);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ApplyMan);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCanccel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.Remark);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.AssumeDept);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.Money);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.BelongMonth);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.FeeProject);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.FeeType);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ApplyDept);
            this.Controls.Add(this.AreaName);
            this.Controls.Add(this.ApplyDate);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.CauseName);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Name = "ExpenseReimbursementAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " 费用报销登记";
            this.Load += new System.EventHandler(this.ExpenseReimbursementAdd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CauseName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ApplyDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ApplyDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AreaName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ApplyDept.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FeeType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FeeProject.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BelongMonth.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Money.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AssumeDept.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Remark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ApplyMan.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BelongYear.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label16;
        private DevExpress.XtraEditors.ComboBoxEdit CauseName;
        private DevExpress.XtraEditors.DateEdit ApplyDate;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.ComboBoxEdit AreaName;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.ComboBoxEdit ApplyDept;
        private DevExpress.XtraEditors.ComboBoxEdit FeeType;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.ComboBoxEdit FeeProject;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.ComboBoxEdit BelongMonth;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private DevExpress.XtraEditors.TextEdit Money;
        private DevExpress.XtraEditors.ComboBoxEdit AssumeDept;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private DevExpress.XtraEditors.MemoEdit Remark;
        private DevExpress.XtraEditors.SimpleButton btnCanccel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.ComboBoxEdit ApplyMan;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.ComboBoxEdit BelongYear;
        private System.Windows.Forms.Label label3;
    }
}