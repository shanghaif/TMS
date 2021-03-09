namespace ZQTMS.UI
{
    partial class frmShortlineSiteAdd
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmShortlineSiteAdd));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.LineCode = new DevExpress.XtraEditors.TextEdit();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.btnSubmit = new DevExpress.XtraEditors.SimpleButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.SiteAddress = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.SiteRemark = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.Province = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.City = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.Area = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.Company = new DevExpress.XtraEditors.LabelControl();
            this.SiteName = new DevExpress.XtraEditors.TextEdit();
            this.companyid = new DevExpress.XtraEditors.ComboBoxEdit();
            this.SiteProvince = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.SiteCity = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.LineCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SiteAddress.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SiteRemark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Province.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.City.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Area.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SiteName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.companyid.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SiteProvince.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SiteCity.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(39, 36);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "站点名称";
            // 
            // LineCode
            // 
            this.LineCode.Location = new System.Drawing.Point(273, 33);
            this.LineCode.Name = "LineCode";
            this.LineCode.Size = new System.Drawing.Size(116, 21);
            this.LineCode.TabIndex = 18;
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(223, 36);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(48, 14);
            this.labelControl9.TabIndex = 17;
            this.labelControl9.Text = "站点代码";
            // 
            // btnSubmit
            // 
            this.btnSubmit.ImageIndex = 25;
            this.btnSubmit.ImageList = this.imageList3;
            this.btnSubmit.Location = new System.Drawing.Point(112, 264);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 27;
            this.btnSubmit.Text = "保  存";
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Shell32 028.ico");
            this.imageList1.Images.SetKeyName(1, "Shell32 190.ico");
            // 
            // btnCancel
            // 
            this.btnCancel.ImageIndex = 28;
            this.btnCancel.ImageList = this.imageList3;
            this.btnCancel.Location = new System.Drawing.Point(253, 264);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 28;
            this.btnCancel.Text = "关   闭";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(43, 74);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(44, 14);
            this.labelControl2.TabIndex = 31;
            this.labelControl2.Text = "省     份";
            // 
            // SiteAddress
            // 
            this.SiteAddress.Location = new System.Drawing.Point(93, 153);
            this.SiteAddress.Name = "SiteAddress";
            this.SiteAddress.Size = new System.Drawing.Size(296, 21);
            this.SiteAddress.TabIndex = 33;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(39, 156);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 32;
            this.labelControl3.Text = "站点地址";
            // 
            // SiteRemark
            // 
            this.SiteRemark.Location = new System.Drawing.Point(93, 112);
            this.SiteRemark.Name = "SiteRemark";
            this.SiteRemark.Size = new System.Drawing.Size(296, 21);
            this.SiteRemark.TabIndex = 37;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(43, 115);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(44, 14);
            this.labelControl4.TabIndex = 36;
            this.labelControl4.Text = "摘     要";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(227, 74);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(44, 14);
            this.labelControl5.TabIndex = 34;
            this.labelControl5.Text = "城     市";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(219, 355);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(48, 14);
            this.labelControl6.TabIndex = 90;
            this.labelControl6.Text = "城      市";
            this.labelControl6.Visible = false;
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(28, 355);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(44, 14);
            this.labelControl7.TabIndex = 89;
            this.labelControl7.Text = "省     份";
            this.labelControl7.Visible = false;
            // 
            // Province
            // 
            this.Province.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.Province.Location = new System.Drawing.Point(82, 352);
            this.Province.Name = "Province";
            this.Province.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.Province.Properties.DropDownRows = 10;
            this.Province.Size = new System.Drawing.Size(116, 21);
            this.Province.TabIndex = 87;
            this.Province.Visible = false;
            // 
            // City
            // 
            this.City.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.City.Location = new System.Drawing.Point(273, 352);
            this.City.Name = "City";
            this.City.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.City.Properties.DropDownRows = 10;
            this.City.Size = new System.Drawing.Size(116, 21);
            this.City.TabIndex = 88;
            this.City.Visible = false;
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(128, 391);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(44, 14);
            this.labelControl8.TabIndex = 92;
            this.labelControl8.Text = "区     县";
            this.labelControl8.Visible = false;
            // 
            // Area
            // 
            this.Area.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.Area.Location = new System.Drawing.Point(182, 388);
            this.Area.Name = "Area";
            this.Area.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.Area.Properties.DropDownRows = 10;
            this.Area.Size = new System.Drawing.Size(116, 21);
            this.Area.TabIndex = 91;
            this.Area.Visible = false;
            // 
            // Company
            // 
            this.Company.Location = new System.Drawing.Point(27, 199);
            this.Company.Name = "Company";
            this.Company.Size = new System.Drawing.Size(60, 14);
            this.Company.TabIndex = 341;
            this.Company.Text = "所属公司ID";
            // 
            // SiteName
            // 
            this.SiteName.Location = new System.Drawing.Point(93, 33);
            this.SiteName.Name = "SiteName";
            this.SiteName.Size = new System.Drawing.Size(116, 21);
            this.SiteName.TabIndex = 349;
            // 
            // companyid
            // 
            this.companyid.Location = new System.Drawing.Point(93, 196);
            this.companyid.Name = "companyid";
            this.companyid.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.companyid.Properties.ReadOnly = true;
            this.companyid.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.companyid.Size = new System.Drawing.Size(116, 21);
            this.companyid.TabIndex = 348;
            // 
            // SiteProvince
            // 
            this.SiteProvince.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.SiteProvince.Location = new System.Drawing.Point(92, 71);
            this.SiteProvince.Name = "SiteProvince";
            this.SiteProvince.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SiteProvince.Properties.DropDownRows = 10;
            this.SiteProvince.Size = new System.Drawing.Size(117, 21);
            this.SiteProvince.TabIndex = 351;
            // 
            // SiteCity
            // 
            this.SiteCity.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.SiteCity.Location = new System.Drawing.Point(273, 71);
            this.SiteCity.Name = "SiteCity";
            this.SiteCity.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SiteCity.Properties.DropDownRows = 10;
            this.SiteCity.Size = new System.Drawing.Size(116, 21);
            this.SiteCity.TabIndex = 352;
            // 
            // imageList3
            // 
            this.imageList3.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList3.ImageStream")));
            this.imageList3.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList3.Images.SetKeyName(0, "Shell32 040.ico");
            this.imageList3.Images.SetKeyName(1, "Shell32 023.ico");
            this.imageList3.Images.SetKeyName(2, "Shell32 022.ico");
            this.imageList3.Images.SetKeyName(3, "Shell32 028.ico");
            this.imageList3.Images.SetKeyName(4, "Shell32 029.ico");
            this.imageList3.Images.SetKeyName(5, "Shell32 035.ico");
            this.imageList3.Images.SetKeyName(6, "Shell32 132.ico");
            this.imageList3.Images.SetKeyName(7, "Shell32 172.ico");
            this.imageList3.Images.SetKeyName(8, "check01c.gif");
            this.imageList3.Images.SetKeyName(9, "Shell32 048.ico");
            this.imageList3.Images.SetKeyName(10, "Clip.ico");
            this.imageList3.Images.SetKeyName(11, "Shell32 055.ico");
            this.imageList3.Images.SetKeyName(12, "icon_xls1.gif");
            this.imageList3.Images.SetKeyName(13, "Shell32 136.ico");
            this.imageList3.Images.SetKeyName(14, "Shell32 147.ico");
            this.imageList3.Images.SetKeyName(15, "Shell32 156.ico");
            this.imageList3.Images.SetKeyName(16, "delete.png");
            this.imageList3.Images.SetKeyName(17, "cadenas1.ico");
            this.imageList3.Images.SetKeyName(18, "Shell32 190.ico");
            this.imageList3.Images.SetKeyName(19, "Shell32 017.ico");
            this.imageList3.Images.SetKeyName(20, "Shell32 146.ico");
            this.imageList3.Images.SetKeyName(21, "AddItem_16x16.png");
            this.imageList3.Images.SetKeyName(22, "Delete_16x16 (2).png");
            this.imageList3.Images.SetKeyName(23, "Action_Edit.png");
            this.imageList3.Images.SetKeyName(24, "Action_Refresh.png");
            this.imageList3.Images.SetKeyName(25, "Action_Save.png");
            this.imageList3.Images.SetKeyName(26, "Action_Printing_Print.png");
            this.imageList3.Images.SetKeyName(27, "Action_Apply_16x16.png");
            this.imageList3.Images.SetKeyName(28, "Action_Close.png");
            this.imageList3.Images.SetKeyName(29, "Action_Search.png");
            this.imageList3.Images.SetKeyName(30, "Action_Customization_16x16.png");
            this.imageList3.Images.SetKeyName(31, "ExistLink_16x16.png");
            this.imageList3.Images.SetKeyName(32, "iconfinder.png");
            this.imageList3.Images.SetKeyName(33, "output.png");
            this.imageList3.Images.SetKeyName(34, "Action_ResetViewSettings.png");
            this.imageList3.Images.SetKeyName(35, "ColumnsThree_16x16.png");
            this.imageList3.Images.SetKeyName(36, "Show_16x16.png");
            this.imageList3.Images.SetKeyName(37, "iconfinder__messenger.png");
            this.imageList3.Images.SetKeyName(38, "iconfinder_Truck.png");
            this.imageList3.Images.SetKeyName(39, "iconfinder_edit2_.png");
            this.imageList3.Images.SetKeyName(40, "iconfinder_extract.png");
            this.imageList3.Images.SetKeyName(41, "Chart_16x16.png");
            this.imageList3.Images.SetKeyName(42, "Undo_16x16 (2).png");
            this.imageList3.Images.SetKeyName(43, "Today_16x16.png");
            this.imageList3.Images.SetKeyName(44, "iconfinder_file-download_326639.png");
            this.imageList3.Images.SetKeyName(45, "Index_16x16.png");
            this.imageList3.Images.SetKeyName(46, "Info_16x16.png");
            this.imageList3.Images.SetKeyName(47, "Forward_16x16.png");
            this.imageList3.Images.SetKeyName(48, "SnapToCells_16x16.png");
            this.imageList3.Images.SetKeyName(49, "Action_InsertImage_16x16.png");
            // 
            // frmShortlineSiteAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 300);
            this.Controls.Add(this.SiteCity);
            this.Controls.Add(this.SiteProvince);
            this.Controls.Add(this.SiteName);
            this.Controls.Add(this.companyid);
            this.Controls.Add(this.Company);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.Area);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.Province);
            this.Controls.Add(this.City);
            this.Controls.Add(this.SiteRemark);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.SiteAddress);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.LineCode);
            this.Controls.Add(this.labelControl9);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmShortlineSiteAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "编辑短线站点";
            this.Load += new System.EventHandler(this.fmMainlineFeeAdd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.LineCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SiteAddress.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SiteRemark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Province.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.City.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Area.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SiteName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.companyid.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SiteProvince.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SiteCity.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit LineCode;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.SimpleButton btnSubmit;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit SiteAddress;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit SiteRemark;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.ImageComboBoxEdit Province;
        private DevExpress.XtraEditors.ImageComboBoxEdit City;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.ImageComboBoxEdit Area;
        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraEditors.LabelControl Company;
        private DevExpress.XtraEditors.TextEdit SiteName;
        private DevExpress.XtraEditors.ComboBoxEdit companyid;
        private DevExpress.XtraEditors.ImageComboBoxEdit SiteProvince;
        private DevExpress.XtraEditors.ImageComboBoxEdit SiteCity;
        private System.Windows.Forms.ImageList imageList3;
    }
}