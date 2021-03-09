namespace ZQTMS.UI
{
    partial class FrmAddGrouper
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
            this.label2 = new System.Windows.Forms.Label();
            this.txtGrouperName = new DevExpress.XtraEditors.TextEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.txtGroupJob = new DevExpress.XtraEditors.TextEdit();
            this.btnSure = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtGrouperName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtGroupJob.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "组员名：";
            // 
            // txtGrouperName
            // 
            this.txtGrouperName.Location = new System.Drawing.Point(85, 13);
            this.txtGrouperName.Name = "txtGrouperName";
            this.txtGrouperName.Size = new System.Drawing.Size(131, 21);
            this.txtGrouperName.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 14);
            this.label4.TabIndex = 6;
            this.label4.Text = "组员职位：";
            // 
            // txtGroupJob
            // 
            this.txtGroupJob.Location = new System.Drawing.Point(85, 50);
            this.txtGroupJob.Name = "txtGroupJob";
            this.txtGroupJob.Size = new System.Drawing.Size(131, 21);
            this.txtGroupJob.TabIndex = 7;
            // 
            // btnSure
            // 
            this.btnSure.Location = new System.Drawing.Point(85, 97);
            this.btnSure.Name = "btnSure";
            this.btnSure.Size = new System.Drawing.Size(75, 23);
            this.btnSure.TabIndex = 8;
            this.btnSure.Text = "确 定";
            this.btnSure.Click += new System.EventHandler(this.btnSure_Click);
            // 
            // FrmAddGrouper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(235, 130);
            this.Controls.Add(this.btnSure);
            this.Controls.Add(this.txtGroupJob);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtGrouperName);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAddGrouper";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "添加组员";
            ((System.ComponentModel.ISupportInitialize)(this.txtGrouperName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtGroupJob.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.TextEdit txtGrouperName;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.TextEdit txtGroupJob;
        private DevExpress.XtraEditors.SimpleButton btnSure;
    }
}