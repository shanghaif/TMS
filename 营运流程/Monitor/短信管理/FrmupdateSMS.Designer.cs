namespace ZQTMS.UI
{
    partial class FrmupdateSMS
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
            this.TextNewTel = new System.Windows.Forms.TextBox();
            this.SWTelNum = new System.Windows.Forms.Label();
            this.SWBillNo = new System.Windows.Forms.Label();
            this.btnSure = new DevExpress.XtraEditors.SimpleButton();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "运单号:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "手机号：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 14);
            this.label3.TabIndex = 2;
            this.label3.Text = "新手机号：";
            // 
            // TextNewTel
            // 
            this.TextNewTel.Location = new System.Drawing.Point(91, 113);
            this.TextNewTel.Name = "TextNewTel";
            this.TextNewTel.Size = new System.Drawing.Size(100, 22);
            this.TextNewTel.TabIndex = 4;
            // 
            // SWTelNum
            // 
            this.SWTelNum.AutoSize = true;
            this.SWTelNum.Location = new System.Drawing.Point(90, 70);
            this.SWTelNum.Name = "SWTelNum";
            this.SWTelNum.Size = new System.Drawing.Size(19, 14);
            this.SWTelNum.TabIndex = 5;
            this.SWTelNum.Text = "无";
            // 
            // SWBillNo
            // 
            this.SWBillNo.AutoSize = true;
            this.SWBillNo.Location = new System.Drawing.Point(89, 24);
            this.SWBillNo.Name = "SWBillNo";
            this.SWBillNo.Size = new System.Drawing.Size(19, 14);
            this.SWBillNo.TabIndex = 6;
            this.SWBillNo.Text = "无";
            // 
            // btnSure
            // 
            this.btnSure.Location = new System.Drawing.Point(32, 170);
            this.btnSure.Name = "btnSure";
            this.btnSure.Size = new System.Drawing.Size(75, 23);
            this.btnSure.TabIndex = 7;
            this.btnSure.Text = "确 定";
            this.btnSure.Click += new System.EventHandler(this.btnSure_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(155, 170);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "关 闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FrmupdateSMS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 220);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSure);
            this.Controls.Add(this.SWBillNo);
            this.Controls.Add(this.SWTelNum);
            this.Controls.Add(this.TextNewTel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FrmupdateSMS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "修改手机号";
            this.Load += new System.EventHandler(this.FrmupdateSMS_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TextNewTel;
        private System.Windows.Forms.Label SWTelNum;
        private System.Windows.Forms.Label SWBillNo;
        private DevExpress.XtraEditors.SimpleButton btnSure;
        private DevExpress.XtraEditors.SimpleButton btnClose;
    }
}