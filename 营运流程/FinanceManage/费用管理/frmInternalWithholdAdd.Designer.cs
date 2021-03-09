namespace ZQTMS.UI
{
    partial class frmInternalWithholdAdd
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
            this.textMonth = new DevExpress.XtraEditors.ComboBoxEdit();
            this.textBillNo = new DevExpress.XtraEditors.TextEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.textReamrk = new DevExpress.XtraEditors.TextEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.textMoney = new DevExpress.XtraEditors.TextEdit();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.textFeeType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.textItem = new DevExpress.XtraEditors.ComboBoxEdit();
            this.textCDDpartement = new DevExpress.XtraEditors.ComboBoxEdit();
            this.textEdit7 = new DevExpress.XtraEditors.ComboBoxEdit();
            this.BelongYear = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.textMonth.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBillNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textReamrk.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textMoney.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textFeeType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textItem.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textCDDpartement.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit7.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BelongYear.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "请输入运单号：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 14);
            this.label2.TabIndex = 3;
            this.label2.Text = "项   目:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 14);
            this.label3.TabIndex = 6;
            this.label3.Text = "费 用 类 型：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 14);
            this.label4.TabIndex = 8;
            this.label4.Text = "费用所属月份：";
            // 
            // textMonth
            // 
            this.textMonth.Cursor = System.Windows.Forms.Cursors.Hand;
            this.textMonth.EditValue = "";
            this.textMonth.Location = new System.Drawing.Point(109, 133);
            this.textMonth.Name = "textMonth";
            this.textMonth.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.textMonth.Properties.Items.AddRange(new object[] {
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
            this.textMonth.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.textMonth.Size = new System.Drawing.Size(130, 21);
            this.textMonth.TabIndex = 68;
            // 
            // textBillNo
            // 
            this.textBillNo.Location = new System.Drawing.Point(109, 8);
            this.textBillNo.Name = "textBillNo";
            this.textBillNo.Size = new System.Drawing.Size(130, 21);
            this.textBillNo.TabIndex = 69;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(44, 167);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 14);
            this.label5.TabIndex = 70;
            this.label5.Text = "摘    要：";
            // 
            // textReamrk
            // 
            this.textReamrk.Location = new System.Drawing.Point(109, 164);
            this.textReamrk.Name = "textReamrk";
            this.textReamrk.Size = new System.Drawing.Size(130, 21);
            this.textReamrk.TabIndex = 71;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(44, 195);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 14);
            this.label6.TabIndex = 72;
            this.label6.Text = "金    额：";
            // 
            // textMoney
            // 
            this.textMoney.Location = new System.Drawing.Point(109, 192);
            this.textMoney.Name = "textMoney";
            this.textMoney.Size = new System.Drawing.Size(130, 21);
            this.textMoney.TabIndex = 73;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(24, 231);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 14);
            this.label7.TabIndex = 74;
            this.label7.Text = "承 担 部 门：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(24, 265);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(79, 14);
            this.label8.TabIndex = 76;
            this.label8.Text = "收 益 部 门：";
            // 
            // simpleButton1
            // 
            this.simpleButton1.ImageIndex = 1;
            this.simpleButton1.Location = new System.Drawing.Point(85, 302);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 23);
            this.simpleButton1.TabIndex = 78;
            this.simpleButton1.Text = "确 定";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.ImageIndex = 0;
            this.simpleButton2.Location = new System.Drawing.Point(184, 302);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(75, 23);
            this.simpleButton2.TabIndex = 79;
            this.simpleButton2.Text = "关  闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // textFeeType
            // 
            this.textFeeType.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.textFeeType.Location = new System.Drawing.Point(109, 37);
            this.textFeeType.Name = "textFeeType";
            this.textFeeType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.textFeeType.Properties.Items.AddRange(new object[] {
            "结算始发分拨费",
            "结算始发操作费",
            "结算干线费",
            "结算终端操作费",
            "结算送货费",
            "结算支线费",
            "结算附加费",
            "接货成本",
            "短驳成本",
            "送货成本",
            "办公物料费",
            "理赔",
            "奖金及提成(加盟)",
            "奖金及提成(直营)",
            "其他费用",
            "罚款支出",
            "外请劳务费"});
            this.textFeeType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.textFeeType.Size = new System.Drawing.Size(130, 21);
            this.textFeeType.TabIndex = 131;
            this.textFeeType.TabStop = false;
            this.textFeeType.SelectedIndexChanged += new System.EventHandler(this.textFeeType_SelectedIndexChanged);
            // 
            // textItem
            // 
            this.textItem.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.textItem.Location = new System.Drawing.Point(109, 68);
            this.textItem.Name = "textItem";
            this.textItem.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.textItem.Size = new System.Drawing.Size(130, 21);
            this.textItem.TabIndex = 132;
            this.textItem.TabStop = false;
            // 
            // textCDDpartement
            // 
            this.textCDDpartement.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.textCDDpartement.Location = new System.Drawing.Point(109, 228);
            this.textCDDpartement.Name = "textCDDpartement";
            this.textCDDpartement.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.textCDDpartement.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.textCDDpartement.Size = new System.Drawing.Size(130, 21);
            this.textCDDpartement.TabIndex = 133;
            this.textCDDpartement.TabStop = false;
            this.textCDDpartement.SelectedIndexChanged += new System.EventHandler(this.textCDDpartement_SelectedIndexChanged);
            // 
            // textEdit7
            // 
            this.textEdit7.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.textEdit7.Location = new System.Drawing.Point(109, 262);
            this.textEdit7.Name = "textEdit7";
            this.textEdit7.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.textEdit7.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.textEdit7.Size = new System.Drawing.Size(130, 21);
            this.textEdit7.TabIndex = 134;
            this.textEdit7.TabStop = false;
            // 
            // BelongYear
            // 
            this.BelongYear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BelongYear.EditValue = "";
            this.BelongYear.Location = new System.Drawing.Point(107, 101);
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
            this.BelongYear.Size = new System.Drawing.Size(130, 21);
            this.BelongYear.TabIndex = 136;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 104);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(91, 14);
            this.label9.TabIndex = 135;
            this.label9.Text = "费用所属年份：";
            // 
            // frmInternalWithholdAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 337);
            this.Controls.Add(this.BelongYear);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textEdit7);
            this.Controls.Add(this.textCDDpartement);
            this.Controls.Add(this.textItem);
            this.Controls.Add(this.textFeeType);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textMoney);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textReamrk);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBillNo);
            this.Controls.Add(this.textMonth);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmInternalWithholdAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "新增/修改";
            this.Load += new System.EventHandler(this.frmInternalWithholdAdd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.textMonth.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBillNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textReamrk.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textMoney.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textFeeType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textItem.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textCDDpartement.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit7.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BelongYear.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.ComboBoxEdit textMonth;
        private DevExpress.XtraEditors.TextEdit textBillNo;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.TextEdit textReamrk;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.TextEdit textMoney;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.ComboBoxEdit textFeeType;
        private DevExpress.XtraEditors.ComboBoxEdit textItem;
        private DevExpress.XtraEditors.ComboBoxEdit textCDDpartement;
        private DevExpress.XtraEditors.ComboBoxEdit textEdit7;
        private DevExpress.XtraEditors.ComboBoxEdit BelongYear;
        private System.Windows.Forms.Label label9;
    }
}