namespace ZQTMS.UI
{
    partial class fmDepartureFeeAdd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fmDepartureFeeAdd));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.Price = new DevExpress.XtraEditors.TextEdit();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl14 = new DevExpress.XtraEditors.LabelControl();
            this.btnSubmit = new DevExpress.XtraEditors.SimpleButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.Descript = new DevExpress.XtraEditors.MemoEdit();
            this.Unit = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.FeeType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.Province = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.City = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.Area = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.Company = new DevExpress.XtraEditors.LabelControl();
            this.companyid = new DevExpress.XtraEditors.ComboBoxEdit();
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.Price.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Descript.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Unit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FeeType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Province.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.City.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Area.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.companyid.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(28, 36);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "费用类型";
            // 
            // Price
            // 
            this.Price.Location = new System.Drawing.Point(273, 33);
            this.Price.Name = "Price";
            this.Price.Size = new System.Drawing.Size(116, 21);
            this.Price.TabIndex = 18;
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(229, 36);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(36, 14);
            this.labelControl9.TabIndex = 17;
            this.labelControl9.Text = "价   格";
            // 
            // labelControl14
            // 
            this.labelControl14.Location = new System.Drawing.Point(36, 144);
            this.labelControl14.Name = "labelControl14";
            this.labelControl14.Size = new System.Drawing.Size(40, 14);
            this.labelControl14.TabIndex = 25;
            this.labelControl14.Text = "备    注";
            // 
            // btnSubmit
            // 
            this.btnSubmit.ImageIndex = 25;
            this.btnSubmit.ImageList = this.imageList3;
            this.btnSubmit.Location = new System.Drawing.Point(112, 223);
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
            this.btnCancel.Location = new System.Drawing.Point(253, 223);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 28;
            this.btnCancel.Text = "关   闭";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // Descript
            // 
            this.Descript.Location = new System.Drawing.Point(82, 124);
            this.Descript.Name = "Descript";
            this.Descript.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Descript.Properties.Appearance.Options.UseFont = true;
            this.Descript.Size = new System.Drawing.Size(307, 70);
            this.Descript.TabIndex = 29;
            this.Descript.TabStop = false;
            // 
            // Unit
            // 
            this.Unit.Location = new System.Drawing.Point(82, 71);
            this.Unit.Name = "Unit";
            this.Unit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.Unit.Properties.Items.AddRange(new object[] {
            "元/吨",
            "元/车",
            ""});
            this.Unit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.Unit.Size = new System.Drawing.Size(116, 21);
            this.Unit.TabIndex = 30;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(40, 74);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(36, 14);
            this.labelControl2.TabIndex = 31;
            this.labelControl2.Text = "单   位";
            // 
            // FeeType
            // 
            this.FeeType.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FeeType.EditValue = "";
            this.FeeType.Location = new System.Drawing.Point(82, 33);
            this.FeeType.Name = "FeeType";
            this.FeeType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FeeType.Properties.Items.AddRange(new object[] {
            "操作费",
            "分拣费",
            "保险费",
            "管理费"});
            this.FeeType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.FeeType.Size = new System.Drawing.Size(116, 21);
            this.FeeType.TabIndex = 38;
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
            this.Company.Location = new System.Drawing.Point(229, 74);
            this.Company.Name = "Company";
            this.Company.Size = new System.Drawing.Size(36, 14);
            this.Company.TabIndex = 341;
            this.Company.Text = "公司ID";
            // 
            // companyid
            // 
            this.companyid.Location = new System.Drawing.Point(273, 71);
            this.companyid.Name = "companyid";
            this.companyid.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.companyid.Properties.ReadOnly = true;
            this.companyid.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.companyid.Size = new System.Drawing.Size(116, 21);
            this.companyid.TabIndex = 348;
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
            // fmDepartureFeeAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 300);
            this.Controls.Add(this.companyid);
            this.Controls.Add(this.Company);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.Area);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.Province);
            this.Controls.Add(this.City);
            this.Controls.Add(this.FeeType);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.Unit);
            this.Controls.Add(this.Descript);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.labelControl14);
            this.Controls.Add(this.Price);
            this.Controls.Add(this.labelControl9);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fmDepartureFeeAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "编辑专线干线费";
            this.Load += new System.EventHandler(this.fmDepartureFeeAdd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Price.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Descript.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Unit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FeeType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Province.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.City.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Area.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.companyid.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit Price;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.LabelControl labelControl14;
        private DevExpress.XtraEditors.SimpleButton btnSubmit;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.MemoEdit Descript;
        private DevExpress.XtraEditors.ComboBoxEdit Unit;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ComboBoxEdit FeeType;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.ImageComboBoxEdit Province;
        private DevExpress.XtraEditors.ImageComboBoxEdit City;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.ImageComboBoxEdit Area;
        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraEditors.LabelControl Company;
        private DevExpress.XtraEditors.ComboBoxEdit companyid;
        private System.Windows.Forms.ImageList imageList3;
    }
}