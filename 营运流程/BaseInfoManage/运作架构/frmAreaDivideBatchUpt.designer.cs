namespace ZQTMS.UI
{
    partial class frmAreaDivideBatchUpt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAreaDivideBatchUpt));
            this.sbjWebCbe = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl19 = new DevExpress.XtraEditors.LabelControl();
            this.OptCoverageCbe = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl18 = new DevExpress.XtraEditors.LabelControl();
            this.coverageZoneTe = new DevExpress.XtraEditors.TextEdit();
            this.labelControl16 = new DevExpress.XtraEditors.LabelControl();
            this.btnSubmit = new DevExpress.XtraEditors.SimpleButton();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.MiddleStreet = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.MiddleCity = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.MiddleArea = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.MiddleProvince = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.middlePartner = new DevExpress.XtraEditors.TextEdit();
            this.labelControl20 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.SiteName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.cb_Type = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.sbjWebCbe.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OptCoverageCbe.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.coverageZoneTe.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MiddleStreet.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MiddleCity.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MiddleArea.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MiddleProvince.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.middlePartner.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SiteName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cb_Type.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // sbjWebCbe
            // 
            this.sbjWebCbe.EditValue = "";
            this.sbjWebCbe.EnterMoveNextControl = true;
            this.sbjWebCbe.Location = new System.Drawing.Point(123, 53);
            this.sbjWebCbe.Name = "sbjWebCbe";
            this.sbjWebCbe.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.sbjWebCbe.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.sbjWebCbe.Size = new System.Drawing.Size(170, 21);
            this.sbjWebCbe.TabIndex = 397;
            // 
            // labelControl19
            // 
            this.labelControl19.Location = new System.Drawing.Point(41, 56);
            this.labelControl19.Name = "labelControl19";
            this.labelControl19.Size = new System.Drawing.Size(72, 14);
            this.labelControl19.TabIndex = 396;
            this.labelControl19.Text = "隶属分拨中心";
            // 
            // OptCoverageCbe
            // 
            this.OptCoverageCbe.EditValue = "";
            this.OptCoverageCbe.EnterMoveNextControl = true;
            this.OptCoverageCbe.Location = new System.Drawing.Point(123, 80);
            this.OptCoverageCbe.Name = "OptCoverageCbe";
            this.OptCoverageCbe.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.OptCoverageCbe.Properties.Items.AddRange(new object[] {
            "",
            "网点覆盖",
            "中心覆盖",
            "中转覆盖",
            "未覆盖"});
            this.OptCoverageCbe.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.OptCoverageCbe.Size = new System.Drawing.Size(170, 21);
            this.OptCoverageCbe.TabIndex = 395;
            // 
            // labelControl18
            // 
            this.labelControl18.Location = new System.Drawing.Point(65, 83);
            this.labelControl18.Name = "labelControl18";
            this.labelControl18.Size = new System.Drawing.Size(48, 14);
            this.labelControl18.TabIndex = 394;
            this.labelControl18.Text = "操作覆盖";
            // 
            // coverageZoneTe
            // 
            this.coverageZoneTe.EnterMoveNextControl = true;
            this.coverageZoneTe.Location = new System.Drawing.Point(123, 26);
            this.coverageZoneTe.Name = "coverageZoneTe";
            this.coverageZoneTe.Size = new System.Drawing.Size(170, 21);
            this.coverageZoneTe.TabIndex = 393;
            // 
            // labelControl16
            // 
            this.labelControl16.Location = new System.Drawing.Point(63, 29);
            this.labelControl16.Name = "labelControl16";
            this.labelControl16.Size = new System.Drawing.Size(48, 14);
            this.labelControl16.TabIndex = 392;
            this.labelControl16.Text = "覆盖分区";
            // 
            // btnSubmit
            // 
            this.btnSubmit.ImageIndex = 25;
            this.btnSubmit.ImageList = this.imageList3;
            this.btnSubmit.Location = new System.Drawing.Point(110, 288);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(76, 29);
            this.btnSubmit.TabIndex = 398;
            this.btnSubmit.Text = "保  存";
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "Shell32 028.ico");
            this.imageList2.Images.SetKeyName(1, "Shell32 190.ico");
            // 
            // btnClose
            // 
            this.btnClose.ImageIndex = 28;
            this.btnClose.ImageList = this.imageList3;
            this.btnClose.Location = new System.Drawing.Point(210, 288);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(76, 29);
            this.btnClose.TabIndex = 399;
            this.btnClose.Text = "关  闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(210, 61);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(24, 14);
            this.labelControl6.TabIndex = 407;
            this.labelControl6.Text = "区县";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(210, 36);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(24, 14);
            this.labelControl5.TabIndex = 406;
            this.labelControl5.Text = "省份";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(35, 89);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(24, 14);
            this.labelControl4.TabIndex = 405;
            this.labelControl4.Text = "街道";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(35, 63);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(24, 14);
            this.labelControl3.TabIndex = 404;
            this.labelControl3.Text = "城市";
            // 
            // MiddleStreet
            // 
            this.MiddleStreet.EnterMoveNextControl = true;
            this.MiddleStreet.Location = new System.Drawing.Point(63, 86);
            this.MiddleStreet.Name = "MiddleStreet";
            this.MiddleStreet.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.MiddleStreet.Size = new System.Drawing.Size(140, 21);
            this.MiddleStreet.TabIndex = 403;
            // 
            // MiddleCity
            // 
            this.MiddleCity.EnterMoveNextControl = true;
            this.MiddleCity.Location = new System.Drawing.Point(63, 60);
            this.MiddleCity.Name = "MiddleCity";
            this.MiddleCity.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.MiddleCity.Size = new System.Drawing.Size(140, 21);
            this.MiddleCity.TabIndex = 401;
            this.MiddleCity.SelectedIndexChanged += new System.EventHandler(this.MiddleCity_SelectedIndexChanged);
            // 
            // MiddleArea
            // 
            this.MiddleArea.EnterMoveNextControl = true;
            this.MiddleArea.Location = new System.Drawing.Point(238, 57);
            this.MiddleArea.Name = "MiddleArea";
            this.MiddleArea.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.MiddleArea.Size = new System.Drawing.Size(140, 21);
            this.MiddleArea.TabIndex = 402;
            this.MiddleArea.SelectedIndexChanged += new System.EventHandler(this.MiddleArea_SelectedIndexChanged);
            // 
            // MiddleProvince
            // 
            this.MiddleProvince.EnterMoveNextControl = true;
            this.MiddleProvince.Location = new System.Drawing.Point(238, 33);
            this.MiddleProvince.Name = "MiddleProvince";
            this.MiddleProvince.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.MiddleProvince.Size = new System.Drawing.Size(140, 21);
            this.MiddleProvince.TabIndex = 400;
            this.MiddleProvince.SelectedIndexChanged += new System.EventHandler(this.MiddleProvince_SelectedIndexChanged);
            // 
            // middlePartner
            // 
            this.middlePartner.EnterMoveNextControl = true;
            this.middlePartner.Location = new System.Drawing.Point(123, 107);
            this.middlePartner.Name = "middlePartner";
            this.middlePartner.Size = new System.Drawing.Size(171, 21);
            this.middlePartner.TabIndex = 409;
            // 
            // labelControl20
            // 
            this.labelControl20.Location = new System.Drawing.Point(53, 110);
            this.labelControl20.Name = "labelControl20";
            this.labelControl20.Size = new System.Drawing.Size(60, 14);
            this.labelControl20.TabIndex = 408;
            this.labelControl20.Text = "中转合作商";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.SiteName);
            this.groupControl1.Controls.Add(this.labelControl8);
            this.groupControl1.Controls.Add(this.MiddleProvince);
            this.groupControl1.Controls.Add(this.MiddleArea);
            this.groupControl1.Controls.Add(this.labelControl5);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.labelControl6);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.MiddleCity);
            this.groupControl1.Controls.Add(this.MiddleStreet);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(414, 112);
            this.groupControl1.TabIndex = 410;
            this.groupControl1.Text = "条件";
            // 
            // SiteName
            // 
            this.SiteName.Location = new System.Drawing.Point(63, 33);
            this.SiteName.Name = "SiteName";
            this.SiteName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SiteName.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.SiteName.Size = new System.Drawing.Size(140, 21);
            this.SiteName.TabIndex = 409;
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(10, 36);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(48, 14);
            this.labelControl8.TabIndex = 408;
            this.labelControl8.Text = "隶属站点";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.cb_Type);
            this.groupControl2.Controls.Add(this.labelControl1);
            this.groupControl2.Controls.Add(this.coverageZoneTe);
            this.groupControl2.Controls.Add(this.labelControl16);
            this.groupControl2.Controls.Add(this.middlePartner);
            this.groupControl2.Controls.Add(this.labelControl20);
            this.groupControl2.Controls.Add(this.sbjWebCbe);
            this.groupControl2.Controls.Add(this.labelControl19);
            this.groupControl2.Controls.Add(this.OptCoverageCbe);
            this.groupControl2.Controls.Add(this.labelControl18);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl2.Location = new System.Drawing.Point(0, 112);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(414, 170);
            this.groupControl2.TabIndex = 411;
            this.groupControl2.Text = "新值";
            // 
            // cb_Type
            // 
            this.cb_Type.EditValue = "";
            this.cb_Type.EnterMoveNextControl = true;
            this.cb_Type.Location = new System.Drawing.Point(123, 134);
            this.cb_Type.Name = "cb_Type";
            this.cb_Type.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cb_Type.Properties.Items.AddRange(new object[] {
            "",
            "自提",
            "送货",
            "自提+送货"});
            this.cb_Type.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cb_Type.Size = new System.Drawing.Size(170, 21);
            this.cb_Type.TabIndex = 411;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(64, 137);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 410;
            this.labelControl1.Text = "服务方式";
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
            // frmAreaDivideBatchUpt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 327);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSubmit);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAreaDivideBatchUpt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "终端区域覆盖批量划分";
            this.Load += new System.EventHandler(this.frmAreaDivideBatchUpt_Load);
            ((System.ComponentModel.ISupportInitialize)(this.sbjWebCbe.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OptCoverageCbe.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.coverageZoneTe.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MiddleStreet.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MiddleCity.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MiddleArea.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MiddleProvince.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.middlePartner.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SiteName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cb_Type.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.ComboBoxEdit sbjWebCbe;
        private DevExpress.XtraEditors.LabelControl labelControl19;
        private DevExpress.XtraEditors.ComboBoxEdit OptCoverageCbe;
        private DevExpress.XtraEditors.LabelControl labelControl18;
        private DevExpress.XtraEditors.TextEdit coverageZoneTe;
        private DevExpress.XtraEditors.LabelControl labelControl16;
        private DevExpress.XtraEditors.SimpleButton btnSubmit;
        private System.Windows.Forms.ImageList imageList2;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.ImageComboBoxEdit MiddleStreet;
        private DevExpress.XtraEditors.ImageComboBoxEdit MiddleCity;
        private DevExpress.XtraEditors.ImageComboBoxEdit MiddleArea;
        private DevExpress.XtraEditors.ImageComboBoxEdit MiddleProvince;
        private DevExpress.XtraEditors.TextEdit middlePartner;
        private DevExpress.XtraEditors.LabelControl labelControl20;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.ComboBoxEdit SiteName;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit cb_Type;
        private System.Windows.Forms.ImageList imageList3;
    }
}