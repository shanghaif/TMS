namespace ZQTMS.UI
{
    partial class frmAddOilPlants
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
            this.txtCarNo = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.txtoilName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.txtoilType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.txtoilCardNo = new DevExpress.XtraEditors.TextEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.txtoildate = new DevExpress.XtraEditors.DateEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.txtoilVolume = new DevExpress.XtraEditors.TextEdit();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtoilPerPrice = new DevExpress.XtraEditors.TextEdit();
            this.label9 = new System.Windows.Forms.Label();
            this.txtoilAccount = new DevExpress.XtraEditors.TextEdit();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtMark = new DevExpress.XtraEditors.MemoEdit();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.label13 = new System.Windows.Forms.Label();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtCarNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtoilName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtoilType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtoilCardNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtoildate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtoildate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtoilVolume.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtoilPerPrice.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtoilAccount.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "车 牌 号：";
            // 
            // txtCarNo
            // 
            this.txtCarNo.Location = new System.Drawing.Point(96, 18);
            this.txtCarNo.Name = "txtCarNo";
            this.txtCarNo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtCarNo.Size = new System.Drawing.Size(120, 21);
            this.txtCarNo.TabIndex = 1;
            this.txtCarNo.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "加油站名称：";
            // 
            // txtoilName
            // 
            this.txtoilName.Location = new System.Drawing.Point(96, 80);
            this.txtoilName.Name = "txtoilName";
            this.txtoilName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtoilName.Properties.Items.AddRange(new object[] {
            "大坪加油站"});
            this.txtoilName.Size = new System.Drawing.Size(120, 21);
            this.txtoilName.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 14);
            this.label3.TabIndex = 4;
            this.label3.Text = "加油方式：";
            // 
            // txtoilType
            // 
            this.txtoilType.EditValue = "油卡";
            this.txtoilType.Location = new System.Drawing.Point(96, 48);
            this.txtoilType.Name = "txtoilType";
            this.txtoilType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtoilType.Properties.Items.AddRange(new object[] {
            "油卡",
            "现金",
            "记名"});
            this.txtoilType.Size = new System.Drawing.Size(120, 21);
            this.txtoilType.TabIndex = 5;
            this.txtoilType.SelectedIndexChanged += new System.EventHandler(this.oilType_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(262, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 14);
            this.label4.TabIndex = 6;
            this.label4.Text = "油 卡 号：";
            // 
            // txtoilCardNo
            // 
            this.txtoilCardNo.Enabled = false;
            this.txtoilCardNo.Location = new System.Drawing.Point(338, 50);
            this.txtoilCardNo.Name = "txtoilCardNo";
            this.txtoilCardNo.Size = new System.Drawing.Size(120, 21);
            this.txtoilCardNo.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(262, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 14);
            this.label5.TabIndex = 8;
            this.label5.Text = "加油时间：";
            // 
            // txtoildate
            // 
            this.txtoildate.EditValue = null;
            this.txtoildate.Location = new System.Drawing.Point(338, 18);
            this.txtoildate.Name = "txtoildate";
            this.txtoildate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtoildate.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.txtoildate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtoildate.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.txtoildate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtoildate.Properties.Mask.EditMask = "yyyy-MM-dd HH:mm";
            this.txtoildate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtoildate.Size = new System.Drawing.Size(120, 21);
            this.txtoildate.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(262, 83);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 14);
            this.label6.TabIndex = 10;
            this.label6.Text = "加 油 量：";
            // 
            // txtoilVolume
            // 
            this.txtoilVolume.EditValue = "0";
            this.txtoilVolume.Location = new System.Drawing.Point(338, 80);
            this.txtoilVolume.Name = "txtoilVolume";
            this.txtoilVolume.Size = new System.Drawing.Size(120, 21);
            this.txtoilVolume.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(464, 83);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(13, 14);
            this.label7.TabIndex = 12;
            this.label7.Text = "L";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(19, 114);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 14);
            this.label8.TabIndex = 13;
            this.label8.Text = "单  价：";
            // 
            // txtoilPerPrice
            // 
            this.txtoilPerPrice.EditValue = "0";
            this.txtoilPerPrice.Location = new System.Drawing.Point(96, 111);
            this.txtoilPerPrice.Name = "txtoilPerPrice";
            this.txtoilPerPrice.Size = new System.Drawing.Size(120, 21);
            this.txtoilPerPrice.TabIndex = 14;
            this.txtoilPerPrice.EditValueChanged += new System.EventHandler(this.txtoilPerPrice_EditValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(262, 114);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 14);
            this.label9.TabIndex = 15;
            this.label9.Text = "总 金 额：";
            // 
            // txtoilAccount
            // 
            this.txtoilAccount.EditValue = "0";
            this.txtoilAccount.Location = new System.Drawing.Point(338, 111);
            this.txtoilAccount.Name = "txtoilAccount";
            this.txtoilAccount.Size = new System.Drawing.Size(120, 21);
            this.txtoilAccount.TabIndex = 16;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(222, 114);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(19, 14);
            this.label10.TabIndex = 17;
            this.label10.Text = "元";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(464, 114);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(19, 14);
            this.label11.TabIndex = 18;
            this.label11.Text = "元";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(17, 177);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(47, 14);
            this.label12.TabIndex = 19;
            this.label12.Text = "备 注：";
            // 
            // txtMark
            // 
            this.txtMark.Location = new System.Drawing.Point(96, 174);
            this.txtMark.Name = "txtMark";
            this.txtMark.Size = new System.Drawing.Size(362, 91);
            this.txtMark.TabIndex = 20;
            this.txtMark.TabStop = false;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(96, 284);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 21;
            this.btnSave.Text = "保  存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(265, 284);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 22;
            this.btnClose.Text = "关  闭";
            this.btnClose.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(225, 137);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 23);
            this.simpleButton1.TabIndex = 23;
            this.simpleButton1.Text = "关联配载";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // textEdit1
            // 
            this.textEdit1.Enabled = false;
            this.textEdit1.Location = new System.Drawing.Point(96, 138);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Properties.ReadOnly = true;
            this.textEdit1.Size = new System.Drawing.Size(120, 21);
            this.textEdit1.TabIndex = 25;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(20, 141);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(67, 14);
            this.label13.TabIndex = 24;
            this.label13.Text = "配载批次：";
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(309, 137);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(94, 23);
            this.simpleButton2.TabIndex = 26;
            this.simpleButton2.Text = "关联已配载批次";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click_1);
            // 
            // frmAddOilPlants
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 321);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.textEdit1);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtoilAccount);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtoilPerPrice);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtoilVolume);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtoildate);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtoilCardNo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtoilType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtoilName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCarNo);
            this.Controls.Add(this.txtMark);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddOilPlants";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "油料登记";
            this.Load += new System.EventHandler(this.frmAddOilPlants_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtCarNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtoilName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtoilType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtoilCardNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtoildate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtoildate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtoilVolume.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtoilPerPrice.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtoilAccount.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.ComboBoxEdit txtCarNo;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.ComboBoxEdit txtoilName;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.ComboBoxEdit txtoilType;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.TextEdit txtoilCardNo;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.DateEdit txtoildate;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.TextEdit txtoilVolume;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private DevExpress.XtraEditors.TextEdit txtoilPerPrice;
        private System.Windows.Forms.Label label9;
        private DevExpress.XtraEditors.TextEdit txtoilAccount;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private DevExpress.XtraEditors.MemoEdit txtMark;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.TextEdit textEdit1;
        private System.Windows.Forms.Label label13;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
    }
}