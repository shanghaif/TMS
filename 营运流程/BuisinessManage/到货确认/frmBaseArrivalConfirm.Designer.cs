namespace ZQTMS.UI
{
    partial class frmBaseArrivalConfirm
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
            this.cbbAgentSite = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cbbAgentWeb = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.lBatchNo = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lCarNo = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtBillNo = new DevExpress.XtraEditors.TextEdit();
            this.label12 = new System.Windows.Forms.Label();
            this.txtBillNum = new DevExpress.XtraEditors.TextEdit();
            this.label13 = new System.Windows.Forms.Label();
            this.txtReason = new DevExpress.XtraEditors.MemoEdit();
            this.lDriver = new System.Windows.Forms.Label();
            this.lStartSite = new System.Windows.Forms.Label();
            this.lDriverPhone = new System.Windows.Forms.Label();
            this.lEndDestination = new System.Windows.Forms.Label();
            this.btnQueryBill = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddCar = new DevExpress.XtraEditors.SimpleButton();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.cbbAgentSite.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbAgentWeb.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillNum.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReason.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "代理站点：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(218, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "代理网点：";
            // 
            // cbbAgentSite
            // 
            this.cbbAgentSite.Location = new System.Drawing.Point(64, 16);
            this.cbbAgentSite.Name = "cbbAgentSite";
            this.cbbAgentSite.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbbAgentSite.Size = new System.Drawing.Size(129, 21);
            this.cbbAgentSite.TabIndex = 2;
            this.cbbAgentSite.SelectedIndexChanged += new System.EventHandler(this.cbbAgentSite_SelectedIndexChanged);
            // 
            // cbbAgentWeb
            // 
            this.cbbAgentWeb.Location = new System.Drawing.Point(277, 16);
            this.cbbAgentWeb.Name = "cbbAgentWeb";
            this.cbbAgentWeb.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbbAgentWeb.Size = new System.Drawing.Size(129, 21);
            this.cbbAgentWeb.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 14);
            this.label3.TabIndex = 4;
            this.label3.Text = "发车批次：";
            // 
            // lBatchNo
            // 
            this.lBatchNo.AutoSize = true;
            this.lBatchNo.Location = new System.Drawing.Point(61, 57);
            this.lBatchNo.Name = "lBatchNo";
            this.lBatchNo.Size = new System.Drawing.Size(19, 14);
            this.lBatchNo.TabIndex = 5;
            this.lBatchNo.Text = "无";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(218, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 14);
            this.label5.TabIndex = 6;
            this.label5.Text = "车  号：";
            // 
            // lCarNo
            // 
            this.lCarNo.AutoSize = true;
            this.lCarNo.Location = new System.Drawing.Point(274, 57);
            this.lCarNo.Name = "lCarNo";
            this.lCarNo.Size = new System.Drawing.Size(19, 14);
            this.lCarNo.TabIndex = 7;
            this.lCarNo.Text = "无";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 95);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 14);
            this.label7.TabIndex = 8;
            this.label7.Text = "司  机：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(218, 95);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 14);
            this.label8.TabIndex = 9;
            this.label8.Text = "司机电话：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 131);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 14);
            this.label9.TabIndex = 10;
            this.label9.Text = "启运站：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(218, 131);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 14);
            this.label10.TabIndex = 11;
            this.label10.Text = "目的地：";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(4, 170);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(55, 14);
            this.label11.TabIndex = 12;
            this.label11.Text = "运单号：";
            // 
            // txtBillNo
            // 
            this.txtBillNo.Location = new System.Drawing.Point(64, 167);
            this.txtBillNo.Name = "txtBillNo";
            this.txtBillNo.Size = new System.Drawing.Size(129, 21);
            this.txtBillNo.TabIndex = 13;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(218, 170);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(63, 14);
            this.label12.TabIndex = 14;
            this.label12.Text = "件     数：";
            // 
            // txtBillNum
            // 
            this.txtBillNum.EditValue = "0";
            this.txtBillNum.Location = new System.Drawing.Point(286, 167);
            this.txtBillNum.Name = "txtBillNum";
            this.txtBillNum.Size = new System.Drawing.Size(120, 21);
            this.txtBillNum.TabIndex = 15;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(4, 216);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(59, 14);
            this.label13.TabIndex = 16;
            this.label13.Text = "原    因：";
            // 
            // txtReason
            // 
            this.txtReason.Location = new System.Drawing.Point(64, 213);
            this.txtReason.Name = "txtReason";
            this.txtReason.Size = new System.Drawing.Size(342, 81);
            this.txtReason.TabIndex = 17;
            this.txtReason.TabStop = false;
            // 
            // lDriver
            // 
            this.lDriver.AutoSize = true;
            this.lDriver.Location = new System.Drawing.Point(61, 95);
            this.lDriver.Name = "lDriver";
            this.lDriver.Size = new System.Drawing.Size(19, 14);
            this.lDriver.TabIndex = 18;
            this.lDriver.Text = "无";
            // 
            // lStartSite
            // 
            this.lStartSite.AutoSize = true;
            this.lStartSite.Location = new System.Drawing.Point(61, 131);
            this.lStartSite.Name = "lStartSite";
            this.lStartSite.Size = new System.Drawing.Size(19, 14);
            this.lStartSite.TabIndex = 19;
            this.lStartSite.Text = "无";
            // 
            // lDriverPhone
            // 
            this.lDriverPhone.AutoSize = true;
            this.lDriverPhone.Location = new System.Drawing.Point(274, 95);
            this.lDriverPhone.Name = "lDriverPhone";
            this.lDriverPhone.Size = new System.Drawing.Size(19, 14);
            this.lDriverPhone.TabIndex = 20;
            this.lDriverPhone.Text = "无";
            // 
            // lEndDestination
            // 
            this.lEndDestination.AutoSize = true;
            this.lEndDestination.Location = new System.Drawing.Point(274, 131);
            this.lEndDestination.Name = "lEndDestination";
            this.lEndDestination.Size = new System.Drawing.Size(19, 14);
            this.lEndDestination.TabIndex = 21;
            this.lEndDestination.Text = "无";
            // 
            // btnQueryBill
            // 
            this.btnQueryBill.Location = new System.Drawing.Point(64, 307);
            this.btnQueryBill.Name = "btnQueryBill";
            this.btnQueryBill.Size = new System.Drawing.Size(75, 23);
            this.btnQueryBill.TabIndex = 22;
            this.btnQueryBill.Text = "查看运单";
            this.btnQueryBill.Click += new System.EventHandler(this.btnQueryBill_Click);
            // 
            // btnAddCar
            // 
            this.btnAddCar.Location = new System.Drawing.Point(171, 307);
            this.btnAddCar.Name = "btnAddCar";
            this.btnAddCar.Size = new System.Drawing.Size(75, 23);
            this.btnAddCar.TabIndex = 23;
            this.btnAddCar.Text = "加入本车";
            this.btnAddCar.Click += new System.EventHandler(this.btnAddCar_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(286, 307);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 24;
            this.btnClose.Text = "关  闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmBaseArrivalConfirm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 342);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnAddCar);
            this.Controls.Add(this.btnQueryBill);
            this.Controls.Add(this.lEndDestination);
            this.Controls.Add(this.lDriverPhone);
            this.Controls.Add(this.lStartSite);
            this.Controls.Add(this.lDriver);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtBillNum);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtBillNo);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lCarNo);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lBatchNo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbbAgentWeb);
            this.Controls.Add(this.cbbAgentSite);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtReason);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBaseArrivalConfirm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "强制点到";
            this.Load += new System.EventHandler(this.frmBaseArrivalConfirm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cbbAgentSite.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbAgentWeb.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillNum.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReason.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.ComboBoxEdit cbbAgentSite;
        private DevExpress.XtraEditors.ComboBoxEdit cbbAgentWeb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lBatchNo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lCarNo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private DevExpress.XtraEditors.TextEdit txtBillNo;
        private System.Windows.Forms.Label label12;
        private DevExpress.XtraEditors.TextEdit txtBillNum;
        private System.Windows.Forms.Label label13;
        private DevExpress.XtraEditors.MemoEdit txtReason;
        private System.Windows.Forms.Label lDriver;
        private System.Windows.Forms.Label lStartSite;
        private System.Windows.Forms.Label lDriverPhone;
        private System.Windows.Forms.Label lEndDestination;
        private DevExpress.XtraEditors.SimpleButton btnQueryBill;
        private DevExpress.XtraEditors.SimpleButton btnAddCar;
        private DevExpress.XtraEditors.SimpleButton btnClose;
    }
}