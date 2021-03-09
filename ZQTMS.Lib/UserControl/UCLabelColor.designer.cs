namespace ZQTMS.Lib
{
    partial class UCLabelColor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbColor = new DevExpress.XtraEditors.LabelControl();
            this.lbText = new DevExpress.XtraEditors.LabelControl();
            this.lbLine = new DevExpress.XtraEditors.LabelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbColor
            // 
            this.lbColor.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.lbColor.Appearance.Options.UseBackColor = true;
            this.lbColor.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lbColor.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbColor.Location = new System.Drawing.Point(2, 2);
            this.lbColor.Name = "lbColor";
            this.lbColor.Size = new System.Drawing.Size(41, 21);
            this.lbColor.TabIndex = 0;
            // 
            // lbText
            // 
            this.lbText.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.lbText.Appearance.Options.UseBackColor = true;
            this.lbText.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lbText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbText.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lbText.LineLocation = DevExpress.XtraEditors.LineLocation.Left;
            this.lbText.LineVisible = true;
            this.lbText.Location = new System.Drawing.Point(43, 2);
            this.lbText.Name = "lbText";
            this.lbText.Size = new System.Drawing.Size(137, 21);
            this.lbText.TabIndex = 1;
            // 
            // lbLine
            // 
            this.lbLine.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lbLine.Appearance.Options.UseBackColor = true;
            this.lbLine.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lbLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbLine.Location = new System.Drawing.Point(0, 24);
            this.lbLine.Name = "lbLine";
            this.lbLine.Size = new System.Drawing.Size(182, 1);
            this.lbLine.TabIndex = 2;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.lbText);
            this.panelControl1.Controls.Add(this.lbColor);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(182, 25);
            this.panelControl1.TabIndex = 3;
            // 
            // LabelColor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbLine);
            this.Controls.Add(this.panelControl1);
            this.Name = "LabelColor";
            this.Size = new System.Drawing.Size(182, 25);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lbColor;
        private DevExpress.XtraEditors.LabelControl lbText;
        private DevExpress.XtraEditors.LabelControl lbLine;
        private DevExpress.XtraEditors.PanelControl panelControl1;


    }
}
