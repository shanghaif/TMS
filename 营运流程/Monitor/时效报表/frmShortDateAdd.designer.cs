namespace ZQTMS.UI
{
    partial class frmShortDateAdd
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
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSubmit = new DevExpress.XtraEditors.SimpleButton();
            this.ShortSite = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.ShortTime = new DevExpress.XtraEditors.TextEdit();
            this.ShortEsite = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.ShortModels = new DevExpress.XtraEditors.ComboBoxEdit();
            this.ShortEweb = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.ShortWeb = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.ShortSite.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShortTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShortEsite.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShortModels.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShortEweb.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShortWeb.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(100, 226);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(82, 14);
            this.labelControl5.TabIndex = 34;
            this.labelControl5.Text = "短途时效(小时)";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(286, 316);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 98;
            this.btnCancel.Text = "取   消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(188, 316);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 97;
            this.btnSubmit.Text = "保  存";
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // ShortSite
            // 
            this.ShortSite.Location = new System.Drawing.Point(188, 50);
            this.ShortSite.Name = "ShortSite";
            this.ShortSite.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ShortSite.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.ShortSite.Size = new System.Drawing.Size(173, 21);
            this.ShortSite.TabIndex = 101;
            this.ShortSite.SelectedIndexChanged += new System.EventHandler(this.ShortSite_SelectedIndexChanged);
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(100, 53);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(48, 14);
            this.labelControl6.TabIndex = 100;
            this.labelControl6.Text = "始发站点";
            this.labelControl6.Click += new System.EventHandler(this.labelControl6_Click);
            // 
            // ShortTime
            // 
            this.ShortTime.Location = new System.Drawing.Point(188, 223);
            this.ShortTime.Name = "ShortTime";
            this.ShortTime.Size = new System.Drawing.Size(173, 21);
            this.ShortTime.TabIndex = 99;
            this.ShortTime.TabStop = false;
            // 
            // ShortEsite
            // 
            this.ShortEsite.Location = new System.Drawing.Point(188, 140);
            this.ShortEsite.Name = "ShortEsite";
            this.ShortEsite.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ShortEsite.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.ShortEsite.Size = new System.Drawing.Size(173, 21);
            this.ShortEsite.TabIndex = 103;
            this.ShortEsite.SelectedIndexChanged += new System.EventHandler(this.ShortEsiteChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(100, 147);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 102;
            this.labelControl1.Text = "目的站点";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(124, 274);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(24, 14);
            this.labelControl2.TabIndex = 104;
            this.labelControl2.Text = "车型";
            // 
            // ShortModels
            // 
            this.ShortModels.Location = new System.Drawing.Point(188, 267);
            this.ShortModels.Name = "ShortModels";
            this.ShortModels.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ShortModels.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.ShortModels.Size = new System.Drawing.Size(173, 21);
            this.ShortModels.TabIndex = 105;
            // 
            // ShortEweb
            // 
            this.ShortEweb.Location = new System.Drawing.Point(188, 178);
            this.ShortEweb.Name = "ShortEweb";
            this.ShortEweb.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ShortEweb.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.ShortEweb.Size = new System.Drawing.Size(173, 21);
            this.ShortEweb.TabIndex = 109;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(100, 181);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 108;
            this.labelControl3.Text = "目的网点";
            // 
            // ShortWeb
            // 
            this.ShortWeb.Location = new System.Drawing.Point(188, 96);
            this.ShortWeb.Name = "ShortWeb";
            this.ShortWeb.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ShortWeb.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.ShortWeb.Size = new System.Drawing.Size(173, 21);
            this.ShortWeb.TabIndex = 107;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(100, 99);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(48, 14);
            this.labelControl4.TabIndex = 106;
            this.labelControl4.Text = "始发网点";
            // 
            // frmShortDateAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 387);
            this.Controls.Add(this.ShortEweb);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.ShortWeb);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.ShortModels);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.ShortEsite);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.ShortSite);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.ShortTime);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmShortDateAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "编辑短途时效";
            this.Load += new System.EventHandler(this.fmDirectSendFeeAdd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ShortSite.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShortTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShortEsite.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShortModels.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShortEweb.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShortWeb.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSubmit;
        private DevExpress.XtraEditors.ComboBoxEdit ShortSite;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.TextEdit ShortTime;
        private DevExpress.XtraEditors.ComboBoxEdit ShortEsite;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ComboBoxEdit ShortModels;
        private DevExpress.XtraEditors.ComboBoxEdit ShortEweb;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.ComboBoxEdit ShortWeb;
        private DevExpress.XtraEditors.LabelControl labelControl4;
    }
}