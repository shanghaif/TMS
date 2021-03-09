namespace ZQTMS.UI
{
    partial class FrmUpdateGoodsInfoByBillNo
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
            this.txtCarLode = new DevExpress.XtraEditors.TextEdit();
            this.btnSure = new DevExpress.XtraEditors.SimpleButton();
            this.label3 = new System.Windows.Forms.Label();
            this.LBillNo = new System.Windows.Forms.Label();
            this.txtNewer = new DevExpress.XtraEditors.TextEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.Lolder = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.txtCarLode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNewer.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "新操作人：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "车  位：";
            // 
            // txtCarLode
            // 
            this.txtCarLode.Location = new System.Drawing.Point(90, 101);
            this.txtCarLode.Name = "txtCarLode";
            this.txtCarLode.Size = new System.Drawing.Size(124, 21);
            this.txtCarLode.TabIndex = 3;
            // 
            // btnSure
            // 
            this.btnSure.Location = new System.Drawing.Point(90, 147);
            this.btnSure.Name = "btnSure";
            this.btnSure.Size = new System.Drawing.Size(75, 23);
            this.btnSure.TabIndex = 4;
            this.btnSure.Text = "确 定";
            this.btnSure.Click += new System.EventHandler(this.btnSure_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 14);
            this.label3.TabIndex = 5;
            this.label3.Text = "运单号:";
            // 
            // LBillNo
            // 
            this.LBillNo.AutoSize = true;
            this.LBillNo.Location = new System.Drawing.Point(87, 9);
            this.LBillNo.Name = "LBillNo";
            this.LBillNo.Size = new System.Drawing.Size(19, 14);
            this.LBillNo.TabIndex = 6;
            this.LBillNo.Text = "无";
            // 
            // txtNewer
            // 
            this.txtNewer.Location = new System.Drawing.Point(90, 68);
            this.txtNewer.Name = "txtNewer";
            this.txtNewer.Size = new System.Drawing.Size(124, 21);
            this.txtNewer.TabIndex = 2;
            this.txtNewer.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 14);
            this.label4.TabIndex = 7;
            this.label4.Text = "原操作人：";
            // 
            // Lolder
            // 
            this.Lolder.AutoSize = true;
            this.Lolder.Location = new System.Drawing.Point(87, 39);
            this.Lolder.Name = "Lolder";
            this.Lolder.Size = new System.Drawing.Size(19, 14);
            this.Lolder.TabIndex = 8;
            this.Lolder.Text = "无";
            // 
            // FrmUpdateGoodsInfoByBillNo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 182);
            this.Controls.Add(this.Lolder);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.LBillNo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnSure);
            this.Controls.Add(this.txtCarLode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtNewer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmUpdateGoodsInfoByBillNo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "修改操作人和车位";
            this.Load += new System.EventHandler(this.FrmUpdateGoodsInfoByBillNo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtCarLode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNewer.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.TextEdit txtCarLode;
        private DevExpress.XtraEditors.SimpleButton btnSure;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label LBillNo;
        private DevExpress.XtraEditors.TextEdit txtNewer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label Lolder;
    }
}