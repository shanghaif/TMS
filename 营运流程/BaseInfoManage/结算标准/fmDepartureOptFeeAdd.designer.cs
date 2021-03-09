namespace ZQTMS.UI
{
    partial class fmDepartureOptFeeAdd
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl14 = new DevExpress.XtraEditors.LabelControl();
            this.btnSubmit = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.Remark = new DevExpress.XtraEditors.MemoEdit();
            this.ParcelPriceMin = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.LightPrice = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.HeavyPrice = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.FromSite = new DevExpress.XtraEditors.ComboBoxEdit();
            this.Company = new DevExpress.XtraEditors.LabelControl();
            this.CompanyID = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txtTransitMode = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.Remark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ParcelPriceMin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LightPrice.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HeavyPrice.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FromSite.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CompanyID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransitMode.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(28, 36);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(44, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "始 发 站";
            // 
            // labelControl14
            // 
            this.labelControl14.Location = new System.Drawing.Point(28, 145);
            this.labelControl14.Name = "labelControl14";
            this.labelControl14.Size = new System.Drawing.Size(44, 14);
            this.labelControl14.TabIndex = 25;
            this.labelControl14.Text = "备     注";
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(99, 255);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 27;
            this.btnSubmit.Text = "保  存";
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(239, 255);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 28;
            this.btnCancel.Text = "取   消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // Remark
            // 
            this.Remark.Location = new System.Drawing.Point(78, 142);
            this.Remark.Name = "Remark";
            this.Remark.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Remark.Properties.Appearance.Options.UseFont = true;
            this.Remark.Size = new System.Drawing.Size(311, 70);
            this.Remark.TabIndex = 29;
            this.Remark.TabStop = false;
            // 
            // ParcelPriceMin
            // 
            this.ParcelPriceMin.Location = new System.Drawing.Point(273, 33);
            this.ParcelPriceMin.Name = "ParcelPriceMin";
            this.ParcelPriceMin.Size = new System.Drawing.Size(116, 21);
            this.ParcelPriceMin.TabIndex = 33;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(219, 36);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 32;
            this.labelControl3.Text = "最低一票";
            // 
            // LightPrice
            // 
            this.LightPrice.Location = new System.Drawing.Point(273, 73);
            this.LightPrice.Name = "LightPrice";
            this.LightPrice.Size = new System.Drawing.Size(116, 21);
            this.LightPrice.TabIndex = 37;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(219, 76);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(48, 14);
            this.labelControl4.TabIndex = 36;
            this.labelControl4.Text = "轻货价格";
            // 
            // HeavyPrice
            // 
            this.HeavyPrice.Location = new System.Drawing.Point(78, 73);
            this.HeavyPrice.Name = "HeavyPrice";
            this.HeavyPrice.Size = new System.Drawing.Size(116, 21);
            this.HeavyPrice.TabIndex = 35;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(24, 76);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(48, 14);
            this.labelControl5.TabIndex = 34;
            this.labelControl5.Text = "重货价格";
            // 
            // FromSite
            // 
            this.FromSite.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FromSite.EditValue = "";
            this.FromSite.Location = new System.Drawing.Point(78, 33);
            this.FromSite.Name = "FromSite";
            this.FromSite.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FromSite.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.FromSite.Size = new System.Drawing.Size(116, 21);
            this.FromSite.TabIndex = 38;
            // 
            // Company
            // 
            this.Company.Location = new System.Drawing.Point(34, 113);
            this.Company.Name = "Company";
            this.Company.Size = new System.Drawing.Size(36, 14);
            this.Company.TabIndex = 339;
            this.Company.Text = "公司ID";
            // 
            // CompanyID
            // 
            this.CompanyID.Location = new System.Drawing.Point(78, 110);
            this.CompanyID.Name = "CompanyID";
            this.CompanyID.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CompanyID.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.CompanyID.Size = new System.Drawing.Size(116, 21);
            this.CompanyID.TabIndex = 348;
            // 
            // txtTransitMode
            // 
            this.txtTransitMode.Location = new System.Drawing.Point(273, 110);
            this.txtTransitMode.Name = "txtTransitMode";
            this.txtTransitMode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtTransitMode.Properties.Items.AddRange(new object[] {
            "中强专线",
            "中强快线",
            "中强整车",
            "中强项目",
            "一票通"});
            this.txtTransitMode.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.txtTransitMode.Size = new System.Drawing.Size(116, 21);
            this.txtTransitMode.TabIndex = 352;
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(217, 113);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(48, 14);
            this.labelControl8.TabIndex = 351;
            this.labelControl8.Text = "运输方式";
            // 
            // fmDepartureOptFeeAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 311);
            this.Controls.Add(this.txtTransitMode);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.CompanyID);
            this.Controls.Add(this.Company);
            this.Controls.Add(this.FromSite);
            this.Controls.Add(this.LightPrice);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.HeavyPrice);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.ParcelPriceMin);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.Remark);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.labelControl14);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fmDepartureOptFeeAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "编辑结算始发操作费";
            this.Load += new System.EventHandler(this.fmDepartureOptFeeAdd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Remark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ParcelPriceMin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LightPrice.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HeavyPrice.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FromSite.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CompanyID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransitMode.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl14;
        private DevExpress.XtraEditors.SimpleButton btnSubmit;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.MemoEdit Remark;
        private DevExpress.XtraEditors.TextEdit ParcelPriceMin;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit LightPrice;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit HeavyPrice;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.ComboBoxEdit FromSite;
        private DevExpress.XtraEditors.LabelControl Company;
        private DevExpress.XtraEditors.ComboBoxEdit CompanyID;
        private DevExpress.XtraEditors.ComboBoxEdit txtTransitMode;
        private DevExpress.XtraEditors.LabelControl labelControl8;
    }
}