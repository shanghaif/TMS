namespace ZQTMS.UI
{
    partial class frmLongDateAdd
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
            this.LongSite = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.LongTime = new DevExpress.XtraEditors.TextEdit();
            this.LongEsite = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.LongModels = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.LongSite.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LongTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LongEsite.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LongModels.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(19, 83);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(72, 14);
            this.labelControl5.TabIndex = 34;
            this.labelControl5.Text = "时间（小时）";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(193, 154);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 98;
            this.btnCancel.Text = "取   消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(95, 154);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 97;
            this.btnSubmit.Text = "保  存";
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // LongSite
            // 
            this.LongSite.Location = new System.Drawing.Point(95, 15);
            this.LongSite.Name = "LongSite";
            this.LongSite.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LongSite.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.LongSite.Size = new System.Drawing.Size(173, 21);
            this.LongSite.TabIndex = 101;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(19, 18);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(52, 14);
            this.labelControl6.TabIndex = 100;
            this.labelControl6.Text = "发       站";
            // 
            // LongTime
            // 
            this.LongTime.Location = new System.Drawing.Point(95, 80);
            this.LongTime.Name = "LongTime";
            this.LongTime.Size = new System.Drawing.Size(173, 21);
            this.LongTime.TabIndex = 99;
            this.LongTime.TabStop = false;
            // 
            // LongEsite
            // 
            this.LongEsite.Location = new System.Drawing.Point(95, 48);
            this.LongEsite.Name = "LongEsite";
            this.LongEsite.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LongEsite.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.LongEsite.Size = new System.Drawing.Size(173, 21);
            this.LongEsite.TabIndex = 103;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(19, 51);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(52, 14);
            this.labelControl1.TabIndex = 102;
            this.labelControl1.Text = "到       站";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(19, 120);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(24, 14);
            this.labelControl2.TabIndex = 104;
            this.labelControl2.Text = "车型";
            // 
            // LongModels
            // 
            this.LongModels.Location = new System.Drawing.Point(95, 117);
            this.LongModels.Name = "LongModels";
            this.LongModels.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LongModels.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.LongModels.Size = new System.Drawing.Size(173, 21);
            this.LongModels.TabIndex = 105;
           // this.LongModels.Enter += new System.EventHandler(this.LongModels_Enter);
            // 
            // frmLongDateAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(357, 217);
            this.Controls.Add(this.LongModels);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.LongEsite);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.LongSite);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.LongTime);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLongDateAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "编辑长途时效";
            this.Load += new System.EventHandler(this.fmDirectSendFeeAdd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.LongSite.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LongTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LongEsite.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LongModels.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSubmit;
        private DevExpress.XtraEditors.ComboBoxEdit LongSite;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.TextEdit LongTime;
        private DevExpress.XtraEditors.ComboBoxEdit LongEsite;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ComboBoxEdit LongModels;
    }
}