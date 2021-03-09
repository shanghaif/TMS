namespace ZQTMS.UI
{
    partial class frmInternalTransactionListAdd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInternalTransactionListAdd));
            this.label1 = new System.Windows.Forms.Label();
            this.SerialNumber = new DevExpress.XtraEditors.TextEdit();
            this.ReportNumber = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.BearSubject = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.BenefitSubject = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.BearDep = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.BenefitDep = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.year = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.month = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.Amount = new DevExpress.XtraEditors.TextEdit();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.Remark = new System.Windows.Forms.TextBox();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.InsideType = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.SerialNumber.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReportNumber.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BearSubject.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BenefitSubject.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BearDep.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BenefitDep.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.year.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.month.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Amount.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InsideType.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(40, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 14);
            this.label1.TabIndex = 28;
            this.label1.Text = "往来编号";
            // 
            // SerialNumber
            // 
            this.SerialNumber.Location = new System.Drawing.Point(101, 19);
            this.SerialNumber.Name = "SerialNumber";
            this.SerialNumber.Properties.ReadOnly = true;
            this.SerialNumber.Size = new System.Drawing.Size(116, 21);
            this.SerialNumber.TabIndex = 29;
            // 
            // ReportNumber
            // 
            this.ReportNumber.Location = new System.Drawing.Point(316, 19);
            this.ReportNumber.Name = "ReportNumber";
            this.ReportNumber.Size = new System.Drawing.Size(100, 21);
            this.ReportNumber.TabIndex = 31;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(255, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 14);
            this.label2.TabIndex = 30;
            this.label2.Text = "报表编号";
            // 
            // BearSubject
            // 
            this.BearSubject.Location = new System.Drawing.Point(101, 61);
            this.BearSubject.Name = "BearSubject";
            this.BearSubject.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.BearSubject.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.BearSubject.Size = new System.Drawing.Size(116, 21);
            this.BearSubject.TabIndex = 220;
            this.BearSubject.SelectedIndexChanged += new System.EventHandler(this.BearSubject_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(40, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 14);
            this.label3.TabIndex = 219;
            this.label3.Text = "承担主体";
            // 
            // BenefitSubject
            // 
            this.BenefitSubject.Location = new System.Drawing.Point(101, 104);
            this.BenefitSubject.Name = "BenefitSubject";
            this.BenefitSubject.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.BenefitSubject.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.BenefitSubject.Size = new System.Drawing.Size(116, 21);
            this.BenefitSubject.TabIndex = 222;
            this.BenefitSubject.SelectedIndexChanged += new System.EventHandler(this.BenefitSubject_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(40, 106);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 14);
            this.label4.TabIndex = 221;
            this.label4.Text = "受益主体";
            // 
            // BearDep
            // 
            this.BearDep.Location = new System.Drawing.Point(316, 61);
            this.BearDep.Name = "BearDep";
            this.BearDep.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.BearDep.Properties.Items.AddRange(new object[] {
            "2017-10",
            "2017-11",
            "2017-12",
            "2018-01",
            "2018-02",
            "2018-03",
            "2018-04",
            "2018-05",
            "2018-06",
            "2018-07",
            "2018-08",
            "2018-09",
            "2018-10",
            "2018-11",
            "2018-12",
            "2019-01",
            "2019-02",
            "2019-03",
            "2019-04",
            "2019-05",
            "2019-06",
            "2019-07",
            "2019-08",
            "2019-09",
            "2019-10",
            "2019-11",
            "2019-12",
            "2020-01",
            "2020-02",
            "2020-03",
            "2020-04",
            "2020-05",
            "2020-06",
            "2020-07",
            "2020-08",
            "2020-09",
            "2020-10",
            "2020-11",
            "2020-12",
            "2021-01",
            "2021-02",
            "2021-03",
            "2021-04",
            "2021-05",
            "2021-06",
            "2021-07",
            "2021-08",
            "2021-09",
            "2021-10",
            "2021-11",
            "2021-12",
            "2022-01",
            "2022-02",
            "2022-03",
            "2022-04",
            "2022-05",
            "2022-06",
            "2022-07",
            "2022-08",
            "2022-09",
            "2022-10",
            "2022-11",
            "2022-12",
            "2023-01",
            "2023-02",
            "2023-03",
            "2023-04",
            "2023-05",
            "2023-06",
            "2023-07",
            "2023-08",
            "2023-09",
            "2023-10",
            "2023-11",
            "2023-12",
            "2024-01",
            "2024-02",
            "2024-03",
            "2024-04",
            "2024-05",
            "2024-06",
            "2024-07",
            "2024-08",
            "2024-09",
            "2024-10",
            "2024-11",
            "2024-12",
            "2025-01",
            "2025-02",
            "2025-03",
            "2025-04",
            "2025-05",
            "2025-06",
            "2025-07",
            "2025-08",
            "2025-09",
            "2025-10",
            "2025-11",
            "2025-12",
            "2026-01",
            "2026-02",
            "2026-03",
            "2026-04",
            "2026-05",
            "2026-06",
            "2026-07",
            "2026-08",
            "2026-09",
            "2026-10",
            "2026-11",
            "2026-12",
            "2027-01",
            "2027-02",
            "2027-03",
            "2027-04",
            "2027-05",
            "2027-06",
            "2027-07",
            "2027-08",
            "2027-09",
            "2027-10",
            "2027-11",
            "2027-12",
            "2028-01",
            "2028-02",
            "2028-03",
            "2028-04",
            "2028-05",
            "2028-06",
            "2028-07",
            "2028-08",
            "2028-09",
            "2028-10",
            "2028-11",
            "2028-12",
            "2029-01",
            "2029-02",
            "2029-03",
            "2029-04",
            "2029-05",
            "2029-06",
            "2029-07",
            "2029-08",
            "2029-09",
            "2029-10",
            "2029-11",
            "2029-12",
            "2030-01",
            "2030-02",
            "2030-03",
            "2030-04",
            "2030-05",
            "2030-06",
            "2030-07",
            "2030-08",
            "2030-09",
            "2030-10",
            "2030-11",
            "2030-12"});
            this.BearDep.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.BearDep.Size = new System.Drawing.Size(100, 21);
            this.BearDep.TabIndex = 224;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(255, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 14);
            this.label5.TabIndex = 223;
            this.label5.Text = "承担部门";
            // 
            // BenefitDep
            // 
            this.BenefitDep.Location = new System.Drawing.Point(316, 104);
            this.BenefitDep.Name = "BenefitDep";
            this.BenefitDep.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.BenefitDep.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.BenefitDep.Size = new System.Drawing.Size(100, 21);
            this.BenefitDep.TabIndex = 226;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(255, 107);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 14);
            this.label6.TabIndex = 225;
            this.label6.Text = "受益部门";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(38, 156);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 14);
            this.label7.TabIndex = 227;
            this.label7.Text = "所属期间";
            // 
            // year
            // 
            this.year.Location = new System.Drawing.Point(99, 153);
            this.year.Name = "year";
            this.year.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.year.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.year.Size = new System.Drawing.Size(118, 21);
            this.year.TabIndex = 228;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Location = new System.Drawing.Point(259, 156);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(19, 14);
            this.label8.TabIndex = 229;
            this.label8.Text = "年";
            // 
            // month
            // 
            this.month.EditValue = "";
            this.month.Location = new System.Drawing.Point(316, 153);
            this.month.Name = "month";
            this.month.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.month.Properties.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12"});
            this.month.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.month.Size = new System.Drawing.Size(100, 21);
            this.month.TabIndex = 230;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(418, 156);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(19, 14);
            this.label9.TabIndex = 231;
            this.label9.Text = "月";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Location = new System.Drawing.Point(51, 203);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(31, 14);
            this.label10.TabIndex = 232;
            this.label10.Text = "金额";
            // 
            // Amount
            // 
            this.Amount.Location = new System.Drawing.Point(99, 200);
            this.Amount.Name = "Amount";
            this.Amount.Properties.Mask.EditMask = "f";
            this.Amount.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.Amount.Size = new System.Drawing.Size(118, 21);
            this.Amount.TabIndex = 235;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Location = new System.Drawing.Point(231, 203);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(79, 14);
            this.label11.TabIndex = 234;
            this.label11.Text = "内部往来类型";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Location = new System.Drawing.Point(44, 250);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(31, 14);
            this.label12.TabIndex = 237;
            this.label12.Text = "备注";
            // 
            // Remark
            // 
            this.Remark.Location = new System.Drawing.Point(99, 250);
            this.Remark.Multiline = true;
            this.Remark.Name = "Remark";
            this.Remark.Size = new System.Drawing.Size(317, 71);
            this.Remark.TabIndex = 238;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(115, 365);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 23);
            this.simpleButton1.TabIndex = 239;
            this.simpleButton1.Text = "确认";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(285, 365);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(75, 23);
            this.simpleButton2.TabIndex = 240;
            this.simpleButton2.Text = "取消";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // InsideType
            // 
            this.InsideType.Location = new System.Drawing.Point(317, 200);
            this.InsideType.Name = "InsideType";
            this.InsideType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.InsideType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.InsideType.Size = new System.Drawing.Size(100, 21);
            this.InsideType.TabIndex = 241;
            // 
            // frmInternalTransactionListAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 431);
            this.Controls.Add(this.InsideType);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.Remark);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.Amount);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.month);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.year);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.BenefitDep);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.BearDep);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.BenefitSubject);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.BearSubject);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ReportNumber);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SerialNumber);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmInternalTransactionListAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "新增内部往来清单";
            this.Load += new System.EventHandler(this.frmInternalTransactionListAdd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SerialNumber.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReportNumber.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BearSubject.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BenefitSubject.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BearDep.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BenefitDep.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.year.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.month.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Amount.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InsideType.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit SerialNumber;
        private DevExpress.XtraEditors.TextEdit ReportNumber;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.ComboBoxEdit BearSubject;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.ComboBoxEdit BenefitSubject;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.ComboBoxEdit BearDep;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.ComboBoxEdit BenefitDep;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.ComboBoxEdit year;
        private System.Windows.Forms.Label label8;
        private DevExpress.XtraEditors.ComboBoxEdit month;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private DevExpress.XtraEditors.TextEdit Amount;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox Remark;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.ComboBoxEdit InsideType;
    }
}