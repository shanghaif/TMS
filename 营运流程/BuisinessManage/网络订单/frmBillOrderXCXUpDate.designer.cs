namespace ZQTMS.UI
{
    partial class frmBillOrderXCXUpDate
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
            this.orderSn = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.num = new DevExpress.XtraEditors.TextEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.Weight = new DevExpress.XtraEditors.TextEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.Volume = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton6 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.orderSn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Weight.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Volume.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(87, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "订单号";
            // 
            // orderSn
            // 
            this.orderSn.Location = new System.Drawing.Point(141, 31);
            this.orderSn.Name = "orderSn";
            this.orderSn.Properties.ReadOnly = true;
            this.orderSn.Size = new System.Drawing.Size(166, 21);
            this.orderSn.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(87, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 14);
            this.label2.TabIndex = 0;
            this.label2.Text = "件数";
            // 
            // num
            // 
            this.num.Location = new System.Drawing.Point(141, 58);
            this.num.Name = "num";
            this.num.Properties.Mask.EditMask = "\\d+(\\R.\\d)?";
            this.num.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.num.Size = new System.Drawing.Size(166, 21);
            this.num.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(87, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 14);
            this.label3.TabIndex = 0;
            this.label3.Text = "重量";
            // 
            // Weight
            // 
            this.Weight.Location = new System.Drawing.Point(141, 85);
            this.Weight.Name = "Weight";
            this.Weight.Properties.Mask.EditMask = "\\d+(\\R.\\d{0,2})?";
            this.Weight.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.Weight.Size = new System.Drawing.Size(166, 21);
            this.Weight.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(87, 115);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 14);
            this.label4.TabIndex = 0;
            this.label4.Text = "体积";
            // 
            // Volume
            // 
            this.Volume.Location = new System.Drawing.Point(141, 112);
            this.Volume.Name = "Volume";
            this.Volume.Properties.Mask.EditMask = "\\d+(\\R.\\d{0,2})?";
            this.Volume.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.Volume.Size = new System.Drawing.Size(166, 21);
            this.Volume.TabIndex = 9;
            // 
            // simpleButton6
            // 
            this.simpleButton6.ImageIndex = 1;
            this.simpleButton6.Location = new System.Drawing.Point(156, 156);
            this.simpleButton6.Name = "simpleButton6";
            this.simpleButton6.Size = new System.Drawing.Size(70, 36);
            this.simpleButton6.TabIndex = 909;
            this.simpleButton6.Text = "保 存";
            this.simpleButton6.Click += new System.EventHandler(this.simpleButton6_Click);
            // 
            // frmBillOrderXCXUpDate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 223);
            this.Controls.Add(this.simpleButton6);
            this.Controls.Add(this.Volume);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Weight);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.num);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.orderSn);
            this.Controls.Add(this.label1);
            this.Name = "frmBillOrderXCXUpDate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "全国货运订单修改";
            this.Load += new System.EventHandler(this.BespeakSendGoods_Load);
            ((System.ComponentModel.ISupportInitialize)(this.orderSn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Weight.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Volume.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        public DevExpress.XtraEditors.TextEdit orderSn;
        private System.Windows.Forms.Label label2;
        public DevExpress.XtraEditors.TextEdit num;
        private System.Windows.Forms.Label label3;
        public DevExpress.XtraEditors.TextEdit Weight;
        private System.Windows.Forms.Label label4;
        public DevExpress.XtraEditors.TextEdit Volume;
        private DevExpress.XtraEditors.SimpleButton simpleButton6;

    }
}