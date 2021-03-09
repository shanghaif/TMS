namespace ZQTMS.UI
{
    partial class frmFoujue
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
            this.foujueyuanyin = new DevExpress.XtraEditors.MemoEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.BillStat = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.foujueyuanyin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BillStat.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // foujueyuanyin
            // 
            this.foujueyuanyin.Location = new System.Drawing.Point(71, 46);
            this.foujueyuanyin.Name = "foujueyuanyin";
            this.foujueyuanyin.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.foujueyuanyin.Properties.Appearance.Options.UseForeColor = true;
            this.foujueyuanyin.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.foujueyuanyin.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.foujueyuanyin.Size = new System.Drawing.Size(321, 90);
            this.foujueyuanyin.TabIndex = 134;
            this.foujueyuanyin.TabStop = false;
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(17, 12);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(48, 14);
            this.labelControl7.TabIndex = 165;
            this.labelControl7.Text = "运单号：";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(5, 48);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 166;
            this.labelControl1.Text = "否决原因：";
            // 
            // BillStat
            // 
            this.BillStat.Enabled = false;
            this.BillStat.Location = new System.Drawing.Point(71, 10);
            this.BillStat.Name = "BillStat";
            this.BillStat.Size = new System.Drawing.Size(132, 21);
            this.BillStat.TabIndex = 186;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(91, 170);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 23);
            this.simpleButton1.TabIndex = 187;
            this.simpleButton1.Text = "否决";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(202, 170);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(75, 23);
            this.simpleButton2.TabIndex = 188;
            this.simpleButton2.Text = "取消";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // frmFoujue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 232);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.BillStat);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.foujueyuanyin);
            this.Name = "frmFoujue";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "否决/取消";
            this.Load += new System.EventHandler(this.frmFoujue_Load);
            ((System.ComponentModel.ISupportInitialize)(this.foujueyuanyin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BillStat.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.MemoEdit foujueyuanyin;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit BillStat;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
    }
}