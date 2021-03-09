namespace ZQTMS.UI.BaseInfoManage
{
    partial class frmRepaiDevicerNo
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
            this.txtDevicerName = new DevExpress.XtraEditors.TextEdit();
            this.txtDevicerAmount = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.txtApplyMer = new DevExpress.XtraEditors.TextEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDevicerNos = new DevExpress.XtraEditors.MemoEdit();
            this.txtCharger = new DevExpress.XtraEditors.ComboBoxEdit();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.txtDevicerName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDevicerAmount.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtApplyMer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDevicerNos.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCharger.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "设备名称：";
            // 
            // txtDevicerName
            // 
            this.txtDevicerName.Location = new System.Drawing.Point(85, 10);
            this.txtDevicerName.Name = "txtDevicerName";
            this.txtDevicerName.Size = new System.Drawing.Size(222, 21);
            this.txtDevicerName.TabIndex = 1;
            // 
            // txtDevicerAmount
            // 
            this.txtDevicerAmount.Location = new System.Drawing.Point(85, 49);
            this.txtDevicerAmount.Name = "txtDevicerAmount";
            this.txtDevicerAmount.Size = new System.Drawing.Size(222, 21);
            this.txtDevicerAmount.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "设备单价：";
            // 
            // txtApplyMer
            // 
            this.txtApplyMer.Location = new System.Drawing.Point(85, 93);
            this.txtApplyMer.Name = "txtApplyMer";
            this.txtApplyMer.Size = new System.Drawing.Size(222, 21);
            this.txtApplyMer.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 14);
            this.label3.TabIndex = 4;
            this.label3.Text = "供应商：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 137);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 14);
            this.label4.TabIndex = 6;
            this.label4.Text = "负责人：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 175);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 14);
            this.label5.TabIndex = 8;
            this.label5.Text = "设备编号：";
            // 
            // txtDevicerNos
            // 
            this.txtDevicerNos.Location = new System.Drawing.Point(85, 172);
            this.txtDevicerNos.Name = "txtDevicerNos";
            this.txtDevicerNos.Size = new System.Drawing.Size(222, 90);
            this.txtDevicerNos.TabIndex = 9;
            this.txtDevicerNos.TabStop = false;
            // 
            // txtCharger
            // 
            this.txtCharger.Location = new System.Drawing.Point(85, 134);
            this.txtCharger.Name = "txtCharger";
            this.txtCharger.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtCharger.Size = new System.Drawing.Size(222, 21);
            this.txtCharger.TabIndex = 7;
            this.txtCharger.TabStop = false;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(85, 286);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "保 存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(232, 286);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "取 消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(82, 263);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(230, 14);
            this.label6.TabIndex = 12;
            this.label6.Text = "*设备编号可输入多条，用英文逗号隔开。";
            // 
            // frmRepaiDevicerNo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 323);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtApplyMer);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtDevicerAmount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDevicerName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDevicerNos);
            this.Controls.Add(this.txtCharger);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRepaiDevicerNo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设备号维护";
            this.Load += new System.EventHandler(this.frmRepaiDevicerNo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtDevicerName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDevicerAmount.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtApplyMer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDevicerNos.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCharger.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit txtDevicerName;
        private DevExpress.XtraEditors.TextEdit txtDevicerAmount;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.TextEdit txtApplyMer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.MemoEdit txtDevicerNos;
        private DevExpress.XtraEditors.ComboBoxEdit txtCharger;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private System.Windows.Forms.Label label6;

    }
}