namespace ZQTMS.UI
{
    partial class frmAddEquipRepair
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
            this.cbbCause = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.cbbArea = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cbbWeb = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtEquipName = new DevExpress.XtraEditors.TextEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPerPrice = new DevExpress.XtraEditors.TextEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.txtMerName = new DevExpress.XtraEditors.TextEdit();
            this.label7 = new System.Windows.Forms.Label();
            this.cbbCharger = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.txtEquipNos = new DevExpress.XtraEditors.MemoEdit();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnExit = new DevExpress.XtraEditors.SimpleButton();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.cbbCause.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbArea.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbWeb.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEquipName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPerPrice.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMerName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbCharger.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEquipNos.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "事 业 部:";
            // 
            // cbbCause
            // 
            this.cbbCause.Location = new System.Drawing.Point(74, 9);
            this.cbbCause.Name = "cbbCause";
            this.cbbCause.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbbCause.Size = new System.Drawing.Size(200, 21);
            this.cbbCause.TabIndex = 1;
            this.cbbCause.SelectedIndexChanged += new System.EventHandler(this.cbbCause_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "大    区:";
            // 
            // cbbArea
            // 
            this.cbbArea.Location = new System.Drawing.Point(74, 36);
            this.cbbArea.Name = "cbbArea";
            this.cbbArea.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbbArea.Size = new System.Drawing.Size(200, 21);
            this.cbbArea.TabIndex = 3;
            // 
            // cbbWeb
            // 
            this.cbbWeb.Location = new System.Drawing.Point(74, 64);
            this.cbbWeb.Name = "cbbWeb";
            this.cbbWeb.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbbWeb.Size = new System.Drawing.Size(200, 21);
            this.cbbWeb.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 14);
            this.label3.TabIndex = 5;
            this.label3.Text = "网    点:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 14);
            this.label4.TabIndex = 6;
            this.label4.Text = "设备名称:";
            // 
            // txtEquipName
            // 
            this.txtEquipName.Location = new System.Drawing.Point(74, 91);
            this.txtEquipName.Name = "txtEquipName";
            this.txtEquipName.Size = new System.Drawing.Size(200, 21);
            this.txtEquipName.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 14);
            this.label5.TabIndex = 8;
            this.label5.Text = "设备单价:";
            // 
            // txtPerPrice
            // 
            this.txtPerPrice.Location = new System.Drawing.Point(74, 118);
            this.txtPerPrice.Name = "txtPerPrice";
            this.txtPerPrice.Size = new System.Drawing.Size(200, 21);
            this.txtPerPrice.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 149);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 14);
            this.label6.TabIndex = 10;
            this.label6.Text = "供应商:";
            // 
            // txtMerName
            // 
            this.txtMerName.Location = new System.Drawing.Point(74, 145);
            this.txtMerName.Name = "txtMerName";
            this.txtMerName.Size = new System.Drawing.Size(200, 21);
            this.txtMerName.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 176);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 14);
            this.label7.TabIndex = 12;
            this.label7.Text = "负责人:";
            // 
            // cbbCharger
            // 
            this.cbbCharger.Location = new System.Drawing.Point(74, 172);
            this.cbbCharger.Name = "cbbCharger";
            this.cbbCharger.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbbCharger.Size = new System.Drawing.Size(200, 21);
            this.cbbCharger.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 202);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 14);
            this.label8.TabIndex = 14;
            this.label8.Text = "设备编号:";
            // 
            // txtEquipNos
            // 
            this.txtEquipNos.Location = new System.Drawing.Point(74, 199);
            this.txtEquipNos.Name = "txtEquipNos";
            this.txtEquipNos.Size = new System.Drawing.Size(200, 100);
            this.txtEquipNos.TabIndex = 15;
            this.txtEquipNos.TabStop = false;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(64, 330);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "保 存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(189, 330);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 17;
            this.btnExit.Text = "退 出";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(71, 302);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(230, 14);
            this.label9.TabIndex = 18;
            this.label9.Text = "*设备编号可输入多条，用英文逗号隔开。";
            // 
            // frmAddEquipRepair
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(321, 370);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cbbCharger);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtMerName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtPerPrice);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtEquipName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbbWeb);
            this.Controls.Add(this.cbbArea);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbbCause);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtEquipNos);
            this.Name = "frmAddEquipRepair";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "新增网点设备信息";
            this.Load += new System.EventHandler(this.frmAddEquipRepair_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cbbCause.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbArea.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbWeb.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEquipName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPerPrice.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMerName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbCharger.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEquipNos.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.ComboBoxEdit cbbCause;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.ComboBoxEdit cbbArea;
        private DevExpress.XtraEditors.ComboBoxEdit cbbWeb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.TextEdit txtEquipName;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.TextEdit txtPerPrice;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.TextEdit txtMerName;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.ComboBoxEdit cbbCharger;
        private System.Windows.Forms.Label label8;
        private DevExpress.XtraEditors.MemoEdit txtEquipNos;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnExit;
        private System.Windows.Forms.Label label9;
    }
}