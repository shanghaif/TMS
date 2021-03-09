namespace ZQTMS.UI
{
    partial class frmNewBudget
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.Cause = new DevExpress.XtraEditors.ComboBoxEdit();
            this.Area = new DevExpress.XtraEditors.ComboBoxEdit();
            this.Web = new DevExpress.XtraEditors.ComboBoxEdit();
            this.FeeType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.BudgetMoney = new DevExpress.XtraEditors.TextEdit();
            this.BelongMonth = new DevExpress.XtraEditors.ComboBoxEdit();
            this.FeeProject = new DevExpress.XtraEditors.ComboBoxEdit();
            this.BelongYear = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.registerDept = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.Cause.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Area.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Web.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FeeType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BudgetMoney.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BelongMonth.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FeeProject.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BelongYear.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.registerDept.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "预算事业部:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(277, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "预算大区:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(43, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 14);
            this.label3.TabIndex = 4;
            this.label3.Text = "预算网点:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(67, 132);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 14);
            this.label4.TabIndex = 6;
            this.label4.Text = "项目:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(277, 82);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 14);
            this.label5.TabIndex = 8;
            this.label5.Text = "费用类型:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(43, 174);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 14);
            this.label6.TabIndex = 10;
            this.label6.Text = "所属月份:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(277, 173);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 14);
            this.label7.TabIndex = 12;
            this.label7.Text = "预算金额:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(134, 267);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 16;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(301, 267);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 17;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Cause
            // 
            this.Cause.Location = new System.Drawing.Point(120, 37);
            this.Cause.Name = "Cause";
            this.Cause.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.Cause.Size = new System.Drawing.Size(116, 21);
            this.Cause.TabIndex = 18;
            this.Cause.SelectedIndexChanged += new System.EventHandler(this.Cause_SelectedIndexChanged);
            // 
            // Area
            // 
            this.Area.Location = new System.Drawing.Point(354, 37);
            this.Area.Name = "Area";
            this.Area.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.Area.Size = new System.Drawing.Size(116, 21);
            this.Area.TabIndex = 20;
            this.Area.SelectedIndexChanged += new System.EventHandler(this.Area_SelectedIndexChanged);
            // 
            // Web
            // 
            this.Web.Location = new System.Drawing.Point(120, 82);
            this.Web.Name = "Web";
            this.Web.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.Web.Size = new System.Drawing.Size(116, 21);
            this.Web.TabIndex = 21;
            // 
            // FeeType
            // 
            this.FeeType.Location = new System.Drawing.Point(354, 82);
            this.FeeType.Name = "FeeType";
            this.FeeType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FeeType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.FeeType.Size = new System.Drawing.Size(116, 21);
            this.FeeType.TabIndex = 22;
            this.FeeType.SelectedIndexChanged += new System.EventHandler(this.FeeType_SelectedIndexChanged);
            // 
            // BudgetMoney
            // 
            this.BudgetMoney.EnterMoveNextControl = true;
            this.BudgetMoney.Location = new System.Drawing.Point(354, 170);
            this.BudgetMoney.Name = "BudgetMoney";
            this.BudgetMoney.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.BudgetMoney.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BudgetMoney.Properties.Appearance.Options.UseFont = true;
            this.BudgetMoney.Properties.Appearance.Options.UseForeColor = true;
            this.BudgetMoney.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.BudgetMoney.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.BudgetMoney.Size = new System.Drawing.Size(116, 22);
            this.BudgetMoney.TabIndex = 23;
            this.BudgetMoney.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.BudgetMoney_KeyPress_1);
            // 
            // BelongMonth
            // 
            this.BelongMonth.Enabled = false;
            this.BelongMonth.Location = new System.Drawing.Point(120, 171);
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
            this.BelongMonth.Size = new System.Drawing.Size(116, 21);
            this.BelongMonth.TabIndex = 24;
            // 
            // FeeProject
            // 
            this.FeeProject.Location = new System.Drawing.Point(120, 129);
            this.FeeProject.Name = "FeeProject";
            this.FeeProject.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FeeProject.Size = new System.Drawing.Size(116, 21);
            this.FeeProject.TabIndex = 25;
            // 
            // BelongYear
            // 
            this.BelongYear.Enabled = false;
            this.BelongYear.Location = new System.Drawing.Point(354, 125);
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
            this.BelongYear.Size = new System.Drawing.Size(116, 21);
            this.BelongYear.TabIndex = 27;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(277, 128);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 14);
            this.label8.TabIndex = 26;
            this.label8.Text = "所属年份:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(43, 214);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 14);
            this.label9.TabIndex = 28;
            this.label9.Text = "登记部门:";
            // 
            // registerDept
            // 
            this.registerDept.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.registerDept.Location = new System.Drawing.Point(120, 211);
            this.registerDept.Name = "registerDept";
            this.registerDept.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.registerDept.Size = new System.Drawing.Size(141, 21);
            this.registerDept.TabIndex = 123;
            this.registerDept.TabStop = false;
            // 
            // frmNewBudget
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 341);
            this.Controls.Add(this.registerDept);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.BelongYear);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.FeeProject);
            this.Controls.Add(this.BelongMonth);
            this.Controls.Add(this.BudgetMoney);
            this.Controls.Add(this.FeeType);
            this.Controls.Add(this.Web);
            this.Controls.Add(this.Area);
            this.Controls.Add(this.Cause);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmNewBudget";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "新增预算";
            this.Load += new System.EventHandler(this.frmNewBudget_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.Cause.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Area.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Web.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FeeType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BudgetMoney.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BelongMonth.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FeeProject.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BelongYear.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.registerDept.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private DevExpress.XtraEditors.ComboBoxEdit Cause;
        private DevExpress.XtraEditors.ComboBoxEdit Area;
        private DevExpress.XtraEditors.ComboBoxEdit Web;
        private DevExpress.XtraEditors.ComboBoxEdit FeeType;
        private ZQTMS.Lib.MyGridView myGridView2;
        private DevExpress.XtraEditors.TextEdit BudgetMoney;
        private DevExpress.XtraEditors.ComboBoxEdit BelongMonth;
        private DevExpress.XtraEditors.ComboBoxEdit FeeProject;
        private DevExpress.XtraEditors.ComboBoxEdit BelongYear;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private DevExpress.XtraEditors.ComboBoxEdit registerDept;
    }
}