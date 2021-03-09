namespace ZQTMS.UI
{
    partial class fmModifyTime
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
            this.shTime = new DevExpress.XtraEditors.DateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.cbRetrieve = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton5 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.shTime.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.shTime.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // shTime
            // 
            this.shTime.EditValue = new System.DateTime(2006, 1, 26, 0, 0, 0, 0);
            this.shTime.Location = new System.Drawing.Point(128, 52);
            this.shTime.Name = "shTime";
            this.shTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.shTime.Properties.DisplayFormat.FormatString = "yyyy-M-d H:mm";
            this.shTime.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.shTime.Properties.Mask.EditMask = "yyyy-M-d H:mm";
            this.shTime.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.shTime.Size = new System.Drawing.Size(123, 21);
            this.shTime.TabIndex = 39;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(43, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 14);
            this.label1.TabIndex = 40;
            this.label1.Text = "审核日期";
            // 
            // cbRetrieve
            // 
            this.cbRetrieve.Appearance.Font = new System.Drawing.Font("宋体", 9F);
            this.cbRetrieve.Appearance.Options.UseFont = true;
            this.cbRetrieve.Location = new System.Drawing.Point(75, 134);
            this.cbRetrieve.Name = "cbRetrieve";
            this.cbRetrieve.ShowToolTips = false;
            this.cbRetrieve.Size = new System.Drawing.Size(47, 26);
            this.cbRetrieve.TabIndex = 41;
            this.cbRetrieve.Text = "确定";
            this.cbRetrieve.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Hand;
            this.cbRetrieve.ToolTipTitle = "帮助";
            this.cbRetrieve.Click += new System.EventHandler(this.cbRetrieve_Click);
            // 
            // simpleButton5
            // 
            this.simpleButton5.Appearance.Font = new System.Drawing.Font("宋体", 9F);
            this.simpleButton5.Appearance.Options.UseFont = true;
            this.simpleButton5.Location = new System.Drawing.Point(170, 134);
            this.simpleButton5.Name = "simpleButton5";
            this.simpleButton5.ShowToolTips = false;
            this.simpleButton5.Size = new System.Drawing.Size(45, 26);
            this.simpleButton5.TabIndex = 42;
            this.simpleButton5.Text = "取消";
            this.simpleButton5.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Hand;
            this.simpleButton5.ToolTipTitle = "帮助";
            this.simpleButton5.Click += new System.EventHandler(this.simpleButton5_Click);
            // 
            // fmModifyTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 196);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.shTime);
            this.Controls.Add(this.simpleButton5);
            this.Controls.Add(this.cbRetrieve);
            this.Name = "fmModifyTime";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.fmModifyTime_Load);
            ((System.ComponentModel.ISupportInitialize)(this.shTime.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.shTime.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private DevExpress.XtraEditors.DateEdit shTime;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SimpleButton cbRetrieve;
        private DevExpress.XtraEditors.SimpleButton simpleButton5;
    }
}