namespace ZQTMS.UI
{
    partial class frmConfirmWithContent
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
            this.txtapply = new DevExpress.XtraEditors.MemoEdit();
            this.txtOperReason = new DevExpress.XtraEditors.MemoEdit();
            this.btnSure = new DevExpress.XtraEditors.SimpleButton();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtapply.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOperReason.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "申请内容";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 159);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "操作理由";
            // 
            // txtapply
            // 
            this.txtapply.Location = new System.Drawing.Point(73, 12);
            this.txtapply.Name = "txtapply";
            this.txtapply.Size = new System.Drawing.Size(467, 139);
            this.txtapply.TabIndex = 2;
            this.txtapply.TabStop = false;
            // 
            // txtOperReason
            // 
            this.txtOperReason.Location = new System.Drawing.Point(73, 179);
            this.txtOperReason.Name = "txtOperReason";
            this.txtOperReason.Size = new System.Drawing.Size(470, 72);
            this.txtOperReason.TabIndex = 3;
            this.txtOperReason.TabStop = false;
            // 
            // btnSure
            // 
            this.btnSure.Location = new System.Drawing.Point(344, 272);
            this.btnSure.Name = "btnSure";
            this.btnSure.Size = new System.Drawing.Size(75, 23);
            this.btnSure.TabIndex = 4;
            this.btnSure.Text = "确 定";
            this.btnSure.Click += new System.EventHandler(this.btnSure_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(468, 272);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "取 消";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmConfirmWithContent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 310);
            this.Controls.Add(this.txtapply);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSure);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtOperReason);
            this.Name = "frmConfirmWithContent";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "确定要进行此操作？";
            this.Load += new System.EventHandler(this.frmConfirmWithContent_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtapply.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOperReason.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.MemoEdit txtapply;
        private DevExpress.XtraEditors.MemoEdit txtOperReason;
        private DevExpress.XtraEditors.SimpleButton btnSure;
        private DevExpress.XtraEditors.SimpleButton btnClose;
    }
}