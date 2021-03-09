namespace ZQTMS.UI
{
    partial class frmArrivalConfirmSingle
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
            this.lblEndSite = new System.Windows.Forms.Label();
            this.lblBeginSite = new System.Windows.Forms.Label();
            this.lblCarNo = new System.Windows.Forms.Label();
            this.lblDriverPhone = new System.Windows.Forms.Label();
            this.lblDriverName = new System.Windows.Forms.Label();
            this.lblDepartureBatch = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.MeAddReason = new DevExpress.XtraEditors.MemoEdit();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.txtBillNo = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtnum = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.MeAddReason.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtnum.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // lblEndSite
            // 
            this.lblEndSite.AutoSize = true;
            this.lblEndSite.Location = new System.Drawing.Point(195, 81);
            this.lblEndSite.Name = "lblEndSite";
            this.lblEndSite.Size = new System.Drawing.Size(63, 14);
            this.lblEndSite.TabIndex = 16;
            this.lblEndSite.Text = "目 的 地：";
            // 
            // lblBeginSite
            // 
            this.lblBeginSite.AutoSize = true;
            this.lblBeginSite.Location = new System.Drawing.Point(14, 81);
            this.lblBeginSite.Name = "lblBeginSite";
            this.lblBeginSite.Size = new System.Drawing.Size(63, 14);
            this.lblBeginSite.TabIndex = 15;
            this.lblBeginSite.Text = "启 运 地：";
            // 
            // lblCarNo
            // 
            this.lblCarNo.AutoSize = true;
            this.lblCarNo.Location = new System.Drawing.Point(195, 11);
            this.lblCarNo.Name = "lblCarNo";
            this.lblCarNo.Size = new System.Drawing.Size(67, 14);
            this.lblCarNo.TabIndex = 14;
            this.lblCarNo.Text = "车　　号：";
            // 
            // lblDriverPhone
            // 
            this.lblDriverPhone.AutoSize = true;
            this.lblDriverPhone.Location = new System.Drawing.Point(195, 46);
            this.lblDriverPhone.Name = "lblDriverPhone";
            this.lblDriverPhone.Size = new System.Drawing.Size(67, 14);
            this.lblDriverPhone.TabIndex = 13;
            this.lblDriverPhone.Text = "司机电话：";
            // 
            // lblDriverName
            // 
            this.lblDriverName.AutoSize = true;
            this.lblDriverName.Location = new System.Drawing.Point(14, 46);
            this.lblDriverName.Name = "lblDriverName";
            this.lblDriverName.Size = new System.Drawing.Size(67, 14);
            this.lblDriverName.TabIndex = 12;
            this.lblDriverName.Text = "司机信息：";
            // 
            // lblDepartureBatch
            // 
            this.lblDepartureBatch.AutoSize = true;
            this.lblDepartureBatch.Location = new System.Drawing.Point(14, 11);
            this.lblDepartureBatch.Name = "lblDepartureBatch";
            this.lblDepartureBatch.Size = new System.Drawing.Size(67, 14);
            this.lblDepartureBatch.TabIndex = 11;
            this.lblDepartureBatch.Text = "发车批次：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 151);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 14);
            this.label2.TabIndex = 23;
            this.label2.Text = "原　因：";
            // 
            // MeAddReason
            // 
            this.MeAddReason.Location = new System.Drawing.Point(61, 149);
            this.MeAddReason.Name = "MeAddReason";
            this.MeAddReason.Size = new System.Drawing.Size(282, 96);
            this.MeAddReason.TabIndex = 20;
            // 
            // simpleButton3
            // 
            this.simpleButton3.ImageIndex = 3;
            this.simpleButton3.Location = new System.Drawing.Point(251, 266);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(81, 23);
            this.simpleButton3.TabIndex = 23;
            this.simpleButton3.Text = "关 闭";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.ImageIndex = 18;
            this.simpleButton2.Location = new System.Drawing.Point(138, 266);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(81, 23);
            this.simpleButton2.TabIndex = 22;
            this.simpleButton2.Text = "加入本车";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.ImageIndex = 4;
            this.simpleButton1.Location = new System.Drawing.Point(25, 266);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(81, 23);
            this.simpleButton1.TabIndex = 21;
            this.simpleButton1.Text = "查看运单";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // txtBillNo
            // 
            this.txtBillNo.EnterMoveNextControl = true;
            this.txtBillNo.Location = new System.Drawing.Point(61, 115);
            this.txtBillNo.Name = "txtBillNo";
            this.txtBillNo.Size = new System.Drawing.Size(141, 21);
            this.txtBillNo.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 118);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 14);
            this.label1.TabIndex = 17;
            this.label1.Text = "运单号：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(204, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 14);
            this.label3.TabIndex = 24;
            this.label3.Text = "件   数：";
            // 
            // txtnum
            // 
            this.txtnum.EditValue = "0";
            this.txtnum.EnterMoveNextControl = true;
            this.txtnum.Location = new System.Drawing.Point(258, 115);
            this.txtnum.Name = "txtnum";
            this.txtnum.Size = new System.Drawing.Size(85, 21);
            this.txtnum.TabIndex = 19;
            // 
            // frmArrivalConfirmSingle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 307);
            this.Controls.Add(this.MeAddReason);
            this.Controls.Add(this.txtnum);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.simpleButton3);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.txtBillNo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblEndSite);
            this.Controls.Add(this.lblBeginSite);
            this.Controls.Add(this.lblCarNo);
            this.Controls.Add(this.lblDriverPhone);
            this.Controls.Add(this.lblDriverName);
            this.Controls.Add(this.lblDepartureBatch);
            this.Name = "frmArrivalConfirmSingle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "强制点到";
            this.Load += new System.EventHandler(this.frmArrivalConfirmSingle_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MeAddReason.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtnum.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblEndSite;
        private System.Windows.Forms.Label lblBeginSite;
        private System.Windows.Forms.Label lblCarNo;
        private System.Windows.Forms.Label lblDriverPhone;
        private System.Windows.Forms.Label lblDriverName;
        private System.Windows.Forms.Label lblDepartureBatch;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.MemoEdit MeAddReason;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.TextEdit txtBillNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.TextEdit txtnum;
    }
}