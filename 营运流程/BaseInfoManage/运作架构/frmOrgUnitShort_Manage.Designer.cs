namespace ZQTMS.UI.BaseInfoManage.运作架构
{
    partial class frmOrgUnitShort_Manage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOrgUnitShort_Manage));
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.CompanyID = new DevExpress.XtraEditors.ComboBoxEdit();
            this.Company = new DevExpress.XtraEditors.LabelControl();
            this.btnConcel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSubmit = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl15 = new DevExpress.XtraEditors.LabelControl();
            this.MiddleStatus = new System.Windows.Forms.CheckBox();
            this.labelControl12 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.Type = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.MiddleStreet = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.MiddleCity = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.MiddleArea = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.MiddleProvince = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.WebName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl17 = new DevExpress.XtraEditors.LabelControl();
            this.Destination = new DevExpress.XtraEditors.TextEdit();
            this.Ascription = new DevExpress.XtraEditors.MemoEdit();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl11 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl13 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl14 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl16 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl18 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl19 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl20 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl21 = new DevExpress.XtraEditors.LabelControl();
            this.SbjWeb = new DevExpress.XtraEditors.ComboBoxEdit();
            this.SiteName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.CompanyID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Type.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MiddleStreet.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MiddleCity.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MiddleArea.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MiddleProvince.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WebName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Destination.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Ascription.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SbjWeb.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SiteName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "Shell32 028.ico");
            this.imageList2.Images.SetKeyName(1, "Shell32 190.ico");
            // 
            // CompanyID
            // 
            this.CompanyID.Location = new System.Drawing.Point(90, 197);
            this.CompanyID.Name = "CompanyID";
            this.CompanyID.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CompanyID.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.CompanyID.Size = new System.Drawing.Size(171, 21);
            this.CompanyID.TabIndex = 412;
            // 
            // Company
            // 
            this.Company.Location = new System.Drawing.Point(46, 200);
            this.Company.Name = "Company";
            this.Company.Size = new System.Drawing.Size(36, 14);
            this.Company.TabIndex = 411;
            this.Company.Text = "公司ID";
            // 
            // btnConcel
            // 
            this.btnConcel.ImageIndex = 28;
            this.btnConcel.ImageList = this.imageList3;
            this.btnConcel.Location = new System.Drawing.Point(447, 389);
            this.btnConcel.Name = "btnConcel";
            this.btnConcel.Size = new System.Drawing.Size(76, 29);
            this.btnConcel.TabIndex = 394;
            this.btnConcel.Text = "关  闭";
            this.btnConcel.Click += new System.EventHandler(this.btnConcel_Click);
            // 
            // btnSubmit
            // 
            this.btnSubmit.ImageIndex = 25;
            this.btnSubmit.ImageList = this.imageList3;
            this.btnSubmit.Location = new System.Drawing.Point(338, 389);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(76, 29);
            this.btnSubmit.TabIndex = 393;
            this.btnSubmit.Text = "保  存";
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // labelControl15
            // 
            this.labelControl15.Location = new System.Drawing.Point(36, 396);
            this.labelControl15.Name = "labelControl15";
            this.labelControl15.Size = new System.Drawing.Size(48, 14);
            this.labelControl15.TabIndex = 410;
            this.labelControl15.Text = "状　　态";
            // 
            // MiddleStatus
            // 
            this.MiddleStatus.AutoSize = true;
            this.MiddleStatus.Checked = true;
            this.MiddleStatus.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MiddleStatus.Location = new System.Drawing.Point(104, 395);
            this.MiddleStatus.Name = "MiddleStatus";
            this.MiddleStatus.Size = new System.Drawing.Size(50, 18);
            this.MiddleStatus.TabIndex = 392;
            this.MiddleStatus.Text = "启用";
            this.MiddleStatus.UseVisualStyleBackColor = true;
            // 
            // labelControl12
            // 
            this.labelControl12.Location = new System.Drawing.Point(24, 232);
            this.labelControl12.Name = "labelControl12";
            this.labelControl12.Size = new System.Drawing.Size(60, 14);
            this.labelControl12.TabIndex = 406;
            this.labelControl12.Text = "短线归属地";
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(301, 155);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(48, 14);
            this.labelControl7.TabIndex = 402;
            this.labelControl7.Text = "服务类型";
            // 
            // Type
            // 
            this.Type.EditValue = "自提";
            this.Type.EnterMoveNextControl = true;
            this.Type.Location = new System.Drawing.Point(355, 152);
            this.Type.Name = "Type";
            this.Type.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.Type.Properties.Items.AddRange(new object[] {
            "自提",
            "送货",
            "自提+送货"});
            this.Type.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.Type.Size = new System.Drawing.Size(181, 21);
            this.Type.TabIndex = 384;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(36, 110);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(48, 14);
            this.labelControl6.TabIndex = 401;
            this.labelControl6.Text = "区　　县";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(36, 66);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(48, 14);
            this.labelControl5.TabIndex = 400;
            this.labelControl5.Text = "省　　份";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(301, 110);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(48, 14);
            this.labelControl4.TabIndex = 399;
            this.labelControl4.Text = "街　　道";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(301, 66);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 398;
            this.labelControl3.Text = "城　　市";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(305, 22);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(44, 14);
            this.labelControl2.TabIndex = 397;
            this.labelControl2.Text = "目 的 地";
            // 
            // MiddleStreet
            // 
            this.MiddleStreet.EnterMoveNextControl = true;
            this.MiddleStreet.Location = new System.Drawing.Point(355, 107);
            this.MiddleStreet.Name = "MiddleStreet";
            this.MiddleStreet.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.MiddleStreet.Size = new System.Drawing.Size(181, 21);
            this.MiddleStreet.TabIndex = 382;
            // 
            // MiddleCity
            // 
            this.MiddleCity.EnterMoveNextControl = true;
            this.MiddleCity.Location = new System.Drawing.Point(355, 63);
            this.MiddleCity.Name = "MiddleCity";
            this.MiddleCity.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.MiddleCity.Size = new System.Drawing.Size(181, 21);
            this.MiddleCity.TabIndex = 380;
            this.MiddleCity.SelectedIndexChanged += new System.EventHandler(this.MiddleCity_SelectedIndexChanged);
            // 
            // MiddleArea
            // 
            this.MiddleArea.EnterMoveNextControl = true;
            this.MiddleArea.Location = new System.Drawing.Point(90, 107);
            this.MiddleArea.Name = "MiddleArea";
            this.MiddleArea.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.MiddleArea.Size = new System.Drawing.Size(171, 21);
            this.MiddleArea.TabIndex = 381;
            this.MiddleArea.SelectedIndexChanged += new System.EventHandler(this.MiddleArea_SelectedIndexChanged);
            // 
            // MiddleProvince
            // 
            this.MiddleProvince.EnterMoveNextControl = true;
            this.MiddleProvince.Location = new System.Drawing.Point(90, 63);
            this.MiddleProvince.Name = "MiddleProvince";
            this.MiddleProvince.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.MiddleProvince.Size = new System.Drawing.Size(171, 21);
            this.MiddleProvince.TabIndex = 379;
            this.MiddleProvince.SelectedIndexChanged += new System.EventHandler(this.MiddleProvince_SelectedIndexChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(36, 155);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 396;
            this.labelControl1.Text = "网点名称";
            // 
            // WebName
            // 
            this.WebName.EditValue = "";
            this.WebName.EnterMoveNextControl = true;
            this.WebName.Location = new System.Drawing.Point(90, 152);
            this.WebName.Name = "WebName";
            this.WebName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.WebName.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.WebName.Size = new System.Drawing.Size(171, 21);
            this.WebName.TabIndex = 383;
            // 
            // labelControl17
            // 
            this.labelControl17.Location = new System.Drawing.Point(12, 22);
            this.labelControl17.Name = "labelControl17";
            this.labelControl17.Size = new System.Drawing.Size(72, 14);
            this.labelControl17.TabIndex = 395;
            this.labelControl17.Text = "短线隶属站点";
            // 
            // Destination
            // 
            this.Destination.EnterMoveNextControl = true;
            this.Destination.Location = new System.Drawing.Point(355, 19);
            this.Destination.Name = "Destination";
            this.Destination.Size = new System.Drawing.Size(181, 21);
            this.Destination.TabIndex = 378;
            // 
            // Ascription
            // 
            this.Ascription.EnterMoveNextControl = true;
            this.Ascription.Location = new System.Drawing.Point(90, 229);
            this.Ascription.Name = "Ascription";
            this.Ascription.Properties.AcceptsReturn = false;
            this.Ascription.Size = new System.Drawing.Size(446, 131);
            this.Ascription.TabIndex = 391;
            // 
            // labelControl8
            // 
            this.labelControl8.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl8.Appearance.Options.UseForeColor = true;
            this.labelControl8.Location = new System.Drawing.Point(264, 23);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(7, 14);
            this.labelControl8.TabIndex = 410;
            this.labelControl8.Text = "*";
            // 
            // labelControl9
            // 
            this.labelControl9.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl9.Appearance.Options.UseForeColor = true;
            this.labelControl9.Location = new System.Drawing.Point(539, 22);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(7, 14);
            this.labelControl9.TabIndex = 410;
            this.labelControl9.Text = "*";
            // 
            // labelControl10
            // 
            this.labelControl10.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl10.Appearance.Options.UseForeColor = true;
            this.labelControl10.Location = new System.Drawing.Point(264, 66);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(7, 14);
            this.labelControl10.TabIndex = 410;
            this.labelControl10.Text = "*";
            // 
            // labelControl11
            // 
            this.labelControl11.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl11.Appearance.Options.UseForeColor = true;
            this.labelControl11.Location = new System.Drawing.Point(264, 110);
            this.labelControl11.Name = "labelControl11";
            this.labelControl11.Size = new System.Drawing.Size(7, 14);
            this.labelControl11.TabIndex = 410;
            this.labelControl11.Text = "*";
            // 
            // labelControl13
            // 
            this.labelControl13.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl13.Appearance.Options.UseForeColor = true;
            this.labelControl13.Location = new System.Drawing.Point(264, 155);
            this.labelControl13.Name = "labelControl13";
            this.labelControl13.Size = new System.Drawing.Size(7, 14);
            this.labelControl13.TabIndex = 410;
            this.labelControl13.Text = "*";
            // 
            // labelControl14
            // 
            this.labelControl14.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl14.Appearance.Options.UseForeColor = true;
            this.labelControl14.Location = new System.Drawing.Point(264, 200);
            this.labelControl14.Name = "labelControl14";
            this.labelControl14.Size = new System.Drawing.Size(7, 14);
            this.labelControl14.TabIndex = 410;
            this.labelControl14.Text = "*";
            // 
            // labelControl16
            // 
            this.labelControl16.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl16.Appearance.Options.UseForeColor = true;
            this.labelControl16.Location = new System.Drawing.Point(539, 66);
            this.labelControl16.Name = "labelControl16";
            this.labelControl16.Size = new System.Drawing.Size(7, 14);
            this.labelControl16.TabIndex = 410;
            this.labelControl16.Text = "*";
            // 
            // labelControl18
            // 
            this.labelControl18.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl18.Appearance.Options.UseForeColor = true;
            this.labelControl18.Location = new System.Drawing.Point(539, 155);
            this.labelControl18.Name = "labelControl18";
            this.labelControl18.Size = new System.Drawing.Size(7, 14);
            this.labelControl18.TabIndex = 410;
            this.labelControl18.Text = "*";
            // 
            // labelControl19
            // 
            this.labelControl19.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl19.Appearance.Options.UseForeColor = true;
            this.labelControl19.Location = new System.Drawing.Point(539, 232);
            this.labelControl19.Name = "labelControl19";
            this.labelControl19.Size = new System.Drawing.Size(7, 14);
            this.labelControl19.TabIndex = 410;
            this.labelControl19.Text = "*";
            // 
            // labelControl20
            // 
            this.labelControl20.Location = new System.Drawing.Point(278, 200);
            this.labelControl20.Name = "labelControl20";
            this.labelControl20.Size = new System.Drawing.Size(72, 14);
            this.labelControl20.TabIndex = 397;
            this.labelControl20.Text = "隶属分拨中心";
            // 
            // labelControl21
            // 
            this.labelControl21.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl21.Appearance.Options.UseForeColor = true;
            this.labelControl21.Location = new System.Drawing.Point(539, 200);
            this.labelControl21.Name = "labelControl21";
            this.labelControl21.Size = new System.Drawing.Size(7, 14);
            this.labelControl21.TabIndex = 410;
            this.labelControl21.Text = "*";
            // 
            // SbjWeb
            // 
            this.SbjWeb.Location = new System.Drawing.Point(355, 197);
            this.SbjWeb.Name = "SbjWeb";
            this.SbjWeb.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SbjWeb.Size = new System.Drawing.Size(181, 21);
            this.SbjWeb.TabIndex = 378;
            this.SbjWeb.TabStop = false;
            // 
            // SiteName
            // 
            this.SiteName.Location = new System.Drawing.Point(90, 19);
            this.SiteName.Name = "SiteName";
            this.SiteName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SiteName.Size = new System.Drawing.Size(171, 21);
            this.SiteName.TabIndex = 377;
            this.SiteName.TabStop = false;
            this.SiteName.Validating += new System.ComponentModel.CancelEventHandler(this.SiteName_Validating);
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
            // frmOrgUnitShort_Manage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 449);
            this.Controls.Add(this.CompanyID);
            this.Controls.Add(this.Company);
            this.Controls.Add(this.btnConcel);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.labelControl19);
            this.Controls.Add(this.labelControl18);
            this.Controls.Add(this.labelControl16);
            this.Controls.Add(this.labelControl21);
            this.Controls.Add(this.labelControl9);
            this.Controls.Add(this.labelControl14);
            this.Controls.Add(this.labelControl13);
            this.Controls.Add(this.labelControl11);
            this.Controls.Add(this.labelControl10);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.labelControl15);
            this.Controls.Add(this.MiddleStatus);
            this.Controls.Add(this.labelControl12);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.Type);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl20);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.MiddleStreet);
            this.Controls.Add(this.MiddleCity);
            this.Controls.Add(this.MiddleArea);
            this.Controls.Add(this.MiddleProvince);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.WebName);
            this.Controls.Add(this.labelControl17);
            this.Controls.Add(this.Destination);
            this.Controls.Add(this.Ascription);
            this.Controls.Add(this.SbjWeb);
            this.Controls.Add(this.SiteName);
            this.Name = "frmOrgUnitShort_Manage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "短线路由";
            this.Load += new System.EventHandler(this.frmOrgUnitShort_Manage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CompanyID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Type.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MiddleStreet.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MiddleCity.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MiddleArea.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MiddleProvince.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WebName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Destination.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Ascription.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SbjWeb.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SiteName.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList imageList2;
        private DevExpress.XtraEditors.ComboBoxEdit CompanyID;
        private DevExpress.XtraEditors.LabelControl Company;
        private DevExpress.XtraEditors.SimpleButton btnConcel;
        private DevExpress.XtraEditors.SimpleButton btnSubmit;
        private DevExpress.XtraEditors.LabelControl labelControl15;
        private System.Windows.Forms.CheckBox MiddleStatus;
        private DevExpress.XtraEditors.LabelControl labelControl12;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.ComboBoxEdit Type;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ImageComboBoxEdit MiddleStreet;
        private DevExpress.XtraEditors.ImageComboBoxEdit MiddleCity;
        private DevExpress.XtraEditors.ImageComboBoxEdit MiddleArea;
        private DevExpress.XtraEditors.ImageComboBoxEdit MiddleProvince;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit WebName;
        private DevExpress.XtraEditors.LabelControl labelControl17;
        private DevExpress.XtraEditors.TextEdit Destination;
        private DevExpress.XtraEditors.MemoEdit Ascription;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraEditors.LabelControl labelControl11;
        private DevExpress.XtraEditors.LabelControl labelControl13;
        private DevExpress.XtraEditors.LabelControl labelControl14;
        private DevExpress.XtraEditors.LabelControl labelControl16;
        private DevExpress.XtraEditors.LabelControl labelControl18;
        private DevExpress.XtraEditors.LabelControl labelControl19;
        private DevExpress.XtraEditors.LabelControl labelControl20;
        private DevExpress.XtraEditors.LabelControl labelControl21;
        private DevExpress.XtraEditors.ComboBoxEdit SbjWeb;
        private DevExpress.XtraEditors.ComboBoxEdit SiteName;
        private System.Windows.Forms.ImageList imageList3;

    }
}