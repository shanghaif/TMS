namespace ZQTMS.UI
{
    partial class frmRPofDriversUpdate
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
            this.label8 = new System.Windows.Forms.Label();
            this.DepartureBatch = new DevExpress.XtraEditors.TextEdit();
            this.WebDate = new DevExpress.XtraEditors.DateEdit();
            this.bsite = new DevExpress.XtraEditors.ComboBoxEdit();
            this.Tosite = new DevExpress.XtraEditors.ComboBoxEdit();
            this.DriverName = new DevExpress.XtraEditors.TextEdit();
            this.CarNO = new DevExpress.XtraEditors.TextEdit();
            this.cbMoneyType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.Amount = new DevExpress.XtraEditors.TextEdit();
            this.label9 = new System.Windows.Forms.Label();
            this.RPContent = new DevExpress.XtraEditors.MemoEdit();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.DepartureBatch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WebDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WebDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsite.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tosite.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DriverName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CarNO.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbMoneyType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Amount.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RPContent.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "发车批次号：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "发站：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(173, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 14);
            this.label3.TabIndex = 3;
            this.label3.Text = "到站：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 14);
            this.label4.TabIndex = 4;
            this.label4.Text = "发车日期：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(36, 137);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 14);
            this.label5.TabIndex = 5;
            this.label5.Text = "车牌号：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(181, 171);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 14);
            this.label6.TabIndex = 6;
            this.label6.Text = "金额：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(24, 110);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 14);
            this.label7.TabIndex = 7;
            this.label7.Text = "司机姓名：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(24, 171);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 14);
            this.label8.TabIndex = 8;
            this.label8.Text = "选择类型：";
            // 
            // DepartureBatch
            // 
            this.DepartureBatch.Location = new System.Drawing.Point(89, 15);
            this.DepartureBatch.Name = "DepartureBatch";
            this.DepartureBatch.Size = new System.Drawing.Size(190, 21);
            this.DepartureBatch.TabIndex = 9;
            // 
            // WebDate
            // 
            this.WebDate.EditValue = new System.DateTime(2006, 1, 26, 0, 0, 0, 0);
            this.WebDate.Location = new System.Drawing.Point(89, 46);
            this.WebDate.Name = "WebDate";
            this.WebDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.WebDate.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.WebDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.WebDate.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.WebDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.WebDate.Properties.Mask.EditMask = "yyyy-MM-dd HH:mm";
            this.WebDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.WebDate.Size = new System.Drawing.Size(190, 21);
            this.WebDate.TabIndex = 39;
            // 
            // bsite
            // 
            this.bsite.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bsite.EditValue = "全部";
            this.bsite.Location = new System.Drawing.Point(89, 76);
            this.bsite.Name = "bsite";
            this.bsite.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.bsite.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.bsite.Size = new System.Drawing.Size(69, 21);
            this.bsite.TabIndex = 40;
            // 
            // Tosite
            // 
            this.Tosite.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Tosite.EditValue = "全部";
            this.Tosite.Location = new System.Drawing.Point(210, 76);
            this.Tosite.Name = "Tosite";
            this.Tosite.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.Tosite.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.Tosite.Size = new System.Drawing.Size(69, 21);
            this.Tosite.TabIndex = 41;
            // 
            // DriverName
            // 
            this.DriverName.Location = new System.Drawing.Point(89, 107);
            this.DriverName.Name = "DriverName";
            this.DriverName.Size = new System.Drawing.Size(190, 21);
            this.DriverName.TabIndex = 42;
            // 
            // CarNO
            // 
            this.CarNO.Location = new System.Drawing.Point(89, 134);
            this.CarNO.Name = "CarNO";
            this.CarNO.Size = new System.Drawing.Size(190, 21);
            this.CarNO.TabIndex = 43;
            // 
            // cbMoneyType
            // 
            this.cbMoneyType.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbMoneyType.EditValue = "全部";
            this.cbMoneyType.Location = new System.Drawing.Point(89, 166);
            this.cbMoneyType.Name = "cbMoneyType";
            this.cbMoneyType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbMoneyType.Properties.Items.AddRange(new object[] {
            "奖励支出",
            "代扣罚款"});
            this.cbMoneyType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbMoneyType.Size = new System.Drawing.Size(86, 21);
            this.cbMoneyType.TabIndex = 44;
            // 
            // Amount
            // 
            this.Amount.Location = new System.Drawing.Point(222, 168);
            this.Amount.Name = "Amount";
            this.Amount.Size = new System.Drawing.Size(57, 21);
            this.Amount.TabIndex = 45;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(48, 201);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 14);
            this.label9.TabIndex = 46;
            this.label9.Text = "备注：";
            // 
            // RPContent
            // 
            this.RPContent.Location = new System.Drawing.Point(89, 199);
            this.RPContent.Name = "RPContent";
            this.RPContent.Size = new System.Drawing.Size(190, 82);
            this.RPContent.TabIndex = 47;
            this.RPContent.TabStop = false;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(50, 302);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 48;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(204, 302);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 49;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmRPofDriversUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 346);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.RPContent);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.Amount);
            this.Controls.Add(this.cbMoneyType);
            this.Controls.Add(this.CarNO);
            this.Controls.Add(this.DriverName);
            this.Controls.Add(this.Tosite);
            this.Controls.Add(this.bsite);
            this.Controls.Add(this.WebDate);
            this.Controls.Add(this.DepartureBatch);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmRPofDriversUpdate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "修改";
            this.Load += new System.EventHandler(this.frmRPofDriversUpdate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DepartureBatch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WebDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WebDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsite.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tosite.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DriverName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CarNO.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbMoneyType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Amount.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RPContent.Properties)).EndInit();
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
        private System.Windows.Forms.Label label8;
        private DevExpress.XtraEditors.TextEdit DepartureBatch;
        private DevExpress.XtraEditors.DateEdit WebDate;
        private DevExpress.XtraEditors.ComboBoxEdit bsite;
        private DevExpress.XtraEditors.ComboBoxEdit Tosite;
        private DevExpress.XtraEditors.TextEdit DriverName;
        private DevExpress.XtraEditors.TextEdit CarNO;
        private DevExpress.XtraEditors.ComboBoxEdit cbMoneyType;
        private DevExpress.XtraEditors.TextEdit Amount;
        private System.Windows.Forms.Label label9;
        private DevExpress.XtraEditors.MemoEdit RPContent;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
    }
}