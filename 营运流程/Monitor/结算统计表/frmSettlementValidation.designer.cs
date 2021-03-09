namespace ZQTMS.UI
{
    partial class frmSettlementValidation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSettlementValidation));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.AreaName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.CauseName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label7 = new System.Windows.Forms.Label();
            this.BillMan = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.BegWeb = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.bdate = new DevExpress.XtraEditors.DateEdit();
            this.edate = new DevExpress.XtraEditors.DateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbClose = new DevExpress.XtraEditors.SimpleButton();
            this.cbRetrieve = new DevExpress.XtraEditors.SimpleButton();
            this.StartSite = new DevExpress.XtraEditors.ComboBoxEdit();
            this.TransferSite = new DevExpress.XtraEditors.ComboBoxEdit();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.barButtonItem4 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem5 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem6 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem7 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.myGridControl1 = new ZQTMS.Lib.MyGridControl();
            this.myGridView1 = new ZQTMS.Lib.MyGridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AreaName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CauseName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BillMan.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BegWeb.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartSite.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TransferSite.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myGridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.AreaName);
            this.panelControl1.Controls.Add(this.label8);
            this.panelControl1.Controls.Add(this.CauseName);
            this.panelControl1.Controls.Add(this.label7);
            this.panelControl1.Controls.Add(this.BillMan);
            this.panelControl1.Controls.Add(this.label6);
            this.panelControl1.Controls.Add(this.BegWeb);
            this.panelControl1.Controls.Add(this.label5);
            this.panelControl1.Controls.Add(this.bdate);
            this.panelControl1.Controls.Add(this.edate);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Controls.Add(this.label4);
            this.panelControl1.Controls.Add(this.label2);
            this.panelControl1.Controls.Add(this.label3);
            this.panelControl1.Controls.Add(this.cbClose);
            this.panelControl1.Controls.Add(this.cbRetrieve);
            this.panelControl1.Controls.Add(this.StartSite);
            this.panelControl1.Controls.Add(this.TransferSite);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(953, 63);
            this.panelControl1.TabIndex = 48;
            // 
            // AreaName
            // 
            this.AreaName.Location = new System.Drawing.Point(535, 8);
            this.AreaName.Name = "AreaName";
            this.AreaName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.AreaName.Properties.ImmediatePopup = true;
            this.AreaName.Size = new System.Drawing.Size(132, 21);
            this.AreaName.TabIndex = 51;
            this.AreaName.SelectedIndexChanged += new System.EventHandler(this.AreaName_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Location = new System.Drawing.Point(497, 11);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 14);
            this.label8.TabIndex = 50;
            this.label8.Text = "大区";
            // 
            // CauseName
            // 
            this.CauseName.Location = new System.Drawing.Point(365, 8);
            this.CauseName.Name = "CauseName";
            this.CauseName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CauseName.Properties.ImmediatePopup = true;
            this.CauseName.Size = new System.Drawing.Size(126, 21);
            this.CauseName.TabIndex = 49;
            this.CauseName.SelectedIndexChanged += new System.EventHandler(this.CauseName_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(319, 11);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 14);
            this.label7.TabIndex = 48;
            this.label7.Text = "事业部";
            // 
            // BillMan
            // 
            this.BillMan.Location = new System.Drawing.Point(535, 35);
            this.BillMan.Name = "BillMan";
            this.BillMan.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.BillMan.Properties.ImmediatePopup = true;
            this.BillMan.Size = new System.Drawing.Size(132, 21);
            this.BillMan.TabIndex = 47;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(498, 38);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 14);
            this.label6.TabIndex = 46;
            this.label6.Text = "制单";
            // 
            // BegWeb
            // 
            this.BegWeb.Location = new System.Drawing.Point(365, 35);
            this.BegWeb.Name = "BegWeb";
            this.BegWeb.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.BegWeb.Properties.ImmediatePopup = true;
            this.BegWeb.Size = new System.Drawing.Size(126, 21);
            this.BegWeb.TabIndex = 45;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(319, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 14);
            this.label5.TabIndex = 44;
            this.label5.Text = "网点";
            // 
            // bdate
            // 
            this.bdate.EditValue = new System.DateTime(2006, 1, 26, 0, 0, 0, 0);
            this.bdate.Location = new System.Drawing.Point(44, 8);
            this.bdate.Name = "bdate";
            this.bdate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.bdate.Properties.DisplayFormat.FormatString = "yyyy-M-d H:mm";
            this.bdate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.bdate.Properties.Mask.EditMask = "yyyy-M-d H:mm";
            this.bdate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.bdate.Size = new System.Drawing.Size(119, 21);
            this.bdate.TabIndex = 38;
            // 
            // edate
            // 
            this.edate.EditValue = new System.DateTime(2006, 1, 26, 0, 0, 0, 0);
            this.edate.Location = new System.Drawing.Point(188, 8);
            this.edate.Name = "edate";
            this.edate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.edate.Properties.DisplayFormat.FormatString = "yyyy-M-d H:mm";
            this.edate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.edate.Properties.Mask.EditMask = "yyyy-M-d H:mm";
            this.edate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.edate.Size = new System.Drawing.Size(119, 21);
            this.edate.TabIndex = 39;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(12, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 14);
            this.label1.TabIndex = 27;
            this.label1.Text = "开单";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(169, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(19, 14);
            this.label4.TabIndex = 30;
            this.label4.Text = "到";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(12, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 14);
            this.label2.TabIndex = 28;
            this.label2.Text = "站点";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(169, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(19, 14);
            this.label3.TabIndex = 29;
            this.label3.Text = "到";
            // 
            // cbClose
            // 
            this.cbClose.Appearance.Font = new System.Drawing.Font("宋体", 12F);
            this.cbClose.Appearance.Options.UseFont = true;
            this.cbClose.Image = global::ZQTMS.UI.Properties.Resources.Action_Close;
            this.cbClose.ImageIndex = 3;
            this.cbClose.Location = new System.Drawing.Point(761, 11);
            this.cbClose.Name = "cbClose";
            this.cbClose.ShowToolTips = false;
            this.cbClose.Size = new System.Drawing.Size(62, 45);
            this.cbClose.TabIndex = 37;
            this.cbClose.Text = "退出";
            this.cbClose.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Hand;
            this.cbClose.Click += new System.EventHandler(this.cbClose_Click);
            // 
            // cbRetrieve
            // 
            this.cbRetrieve.Appearance.Font = new System.Drawing.Font("宋体", 12F);
            this.cbRetrieve.Appearance.Options.UseFont = true;
            this.cbRetrieve.Image = global::ZQTMS.UI.Properties.Resources.Action_Search;
            this.cbRetrieve.Location = new System.Drawing.Point(689, 11);
            this.cbRetrieve.Name = "cbRetrieve";
            this.cbRetrieve.ShowToolTips = false;
            this.cbRetrieve.Size = new System.Drawing.Size(62, 45);
            this.cbRetrieve.TabIndex = 31;
            this.cbRetrieve.Text = "提取";
            this.cbRetrieve.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Hand;
            this.cbRetrieve.ToolTipTitle = "帮助";
            this.cbRetrieve.Click += new System.EventHandler(this.cbRetrieve_Click);
            // 
            // StartSite
            // 
            this.StartSite.Cursor = System.Windows.Forms.Cursors.Hand;
            this.StartSite.EditValue = "";
            this.StartSite.Location = new System.Drawing.Point(44, 35);
            this.StartSite.Name = "StartSite";
            this.StartSite.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.StartSite.Properties.ImmediatePopup = true;
            this.StartSite.Size = new System.Drawing.Size(119, 21);
            this.StartSite.TabIndex = 24;
            // 
            // TransferSite
            // 
            this.TransferSite.Cursor = System.Windows.Forms.Cursors.Hand;
            this.TransferSite.EditValue = "";
            this.TransferSite.Location = new System.Drawing.Point(188, 35);
            this.TransferSite.Name = "TransferSite";
            this.TransferSite.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.TransferSite.Properties.ImmediatePopup = true;
            this.TransferSite.Size = new System.Drawing.Size(119, 21);
            this.TransferSite.TabIndex = 25;
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar3});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Images = this.imageList3;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barSubItem1,
            this.barButtonItem1,
            this.barButtonItem2,
            this.barButtonItem3,
            this.barButtonItem4,
            this.barButtonItem5,
            this.barButtonItem6,
            this.barButtonItem7});
            this.barManager1.MaxItemId = 11;
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barSubItem1, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem1, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem2, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem3, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // barSubItem1
            // 
            this.barSubItem1.Caption = "外观设置";
            this.barSubItem1.Id = 3;
            this.barSubItem1.ImageIndex = 30;
            this.barSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem4, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem5, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem6, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem7, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.barSubItem1.Name = "barSubItem1";
            // 
            // barButtonItem4
            // 
            this.barButtonItem4.Caption = "自动筛选";
            this.barButtonItem4.Id = 7;
            this.barButtonItem4.ImageIndex = 1;
            this.barButtonItem4.Name = "barButtonItem4";
            this.barButtonItem4.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem4_ItemClick);
            // 
            // barButtonItem5
            // 
            this.barButtonItem5.Caption = "锁定外观";
            this.barButtonItem5.Id = 8;
            this.barButtonItem5.ImageIndex = 0;
            this.barButtonItem5.Name = "barButtonItem5";
            this.barButtonItem5.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem5_ItemClick);
            // 
            // barButtonItem6
            // 
            this.barButtonItem6.Caption = "取消外观";
            this.barButtonItem6.Id = 9;
            this.barButtonItem6.ImageIndex = 3;
            this.barButtonItem6.Name = "barButtonItem6";
            this.barButtonItem6.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem6_ItemClick);
            // 
            // barButtonItem7
            // 
            this.barButtonItem7.Caption = "过滤器";
            this.barButtonItem7.Id = 10;
            this.barButtonItem7.ImageIndex = 7;
            this.barButtonItem7.Name = "barButtonItem7";
            this.barButtonItem7.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem7_ItemClick);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "导出";
            this.barButtonItem1.Id = 4;
            this.barButtonItem1.ImageIndex = 33;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "计算";
            this.barButtonItem2.Id = 5;
            this.barButtonItem2.ImageIndex = 23;
            this.barButtonItem2.Name = "barButtonItem2";
            this.barButtonItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem2_ItemClick);
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "退出";
            this.barButtonItem3.Id = 6;
            this.barButtonItem3.ImageIndex = 28;
            this.barButtonItem3.Name = "barButtonItem3";
            this.barButtonItem3.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem3_ItemClick);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "Shell32 040.ico");
            this.imageList2.Images.SetKeyName(1, "Shell32 023.ico");
            this.imageList2.Images.SetKeyName(2, "Shell32 022.ico");
            this.imageList2.Images.SetKeyName(3, "Shell32 028.ico");
            this.imageList2.Images.SetKeyName(4, "Shell32 029.ico");
            this.imageList2.Images.SetKeyName(5, "Shell32 035.ico");
            this.imageList2.Images.SetKeyName(6, "Shell32 132.ico");
            this.imageList2.Images.SetKeyName(7, "Shell32 172.ico");
            this.imageList2.Images.SetKeyName(8, "check01c.gif");
            this.imageList2.Images.SetKeyName(9, "Shell32 048.ico");
            this.imageList2.Images.SetKeyName(10, "Clip.ico");
            this.imageList2.Images.SetKeyName(11, "Shell32 055.ico");
            this.imageList2.Images.SetKeyName(12, "icon_xls1.gif");
            this.imageList2.Images.SetKeyName(13, "Shell32 136.ico");
            this.imageList2.Images.SetKeyName(14, "Shell32 147.ico");
            this.imageList2.Images.SetKeyName(15, "Shell32 156.ico");
            this.imageList2.Images.SetKeyName(16, "delete.png");
            this.imageList2.Images.SetKeyName(17, "cadenas1.ico");
            this.imageList2.Images.SetKeyName(18, "Shell32 190.ico");
            this.imageList2.Images.SetKeyName(19, "Shell32 017.ico");
            this.imageList2.Images.SetKeyName(20, "Shell32 146.ico");
            // 
            // myGridControl1
            // 
            this.myGridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myGridControl1.Location = new System.Drawing.Point(0, 63);
            this.myGridControl1.MainView = this.myGridView1;
            this.myGridControl1.MenuManager = this.barManager1;
            this.myGridControl1.Name = "myGridControl1";
            this.myGridControl1.Size = new System.Drawing.Size(953, 339);
            this.myGridControl1.TabIndex = 53;
            this.myGridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.myGridView1});
            // 
            // myGridView1
            // 
            this.myGridView1.ColumnPanelRowHeight = 30;
            this.myGridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1});
            this.myGridView1.GridControl = this.myGridControl1;
            this.myGridView1.GridViewRemark = "结算信息验证表";
            this.myGridView1.Guid = new System.Guid("35962aba-1be3-4178-bbae-fcb1cffc2a5f");
            this.myGridView1.HiddenFiledDic = ((System.Collections.Generic.Dictionary<string, object>)(resources.GetObject("myGridView1.HiddenFiledDic")));
            this.myGridView1.MenuName = "";
            this.myGridView1.Name = "myGridView1";
            this.myGridView1.OptionsView.ColumnAutoWidth = false;
            this.myGridView1.OptionsView.ShowAutoFilterRow = true;
            this.myGridView1.OptionsView.ShowGroupPanel = false;
            this.myGridView1.WebControlBindFindName = "";
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "序号";
            this.gridColumn1.FieldName = "rowid";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.OptionsColumn.AllowFocus = false;
            this.gridColumn1.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
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
            this.imageList3.Images.SetKeyName(50, "Next_16x16 (2).png");
            this.imageList3.Images.SetKeyName(51, "Add_16x16.png");
            this.imageList3.Images.SetKeyName(52, "Delete_16x16.png");
            // 
            // frmSettlementValidation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(953, 428);
            this.Controls.Add(this.myGridControl1);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmSettlementValidation";
            this.Text = "结算信息验证表";
            this.Load += new System.EventHandler(this.frmSettlementValidation_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AreaName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CauseName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BillMan.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BegWeb.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartSite.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TransferSite.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myGridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit AreaName;
        private System.Windows.Forms.Label label8;
        private DevExpress.XtraEditors.ComboBoxEdit CauseName;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.ComboBoxEdit BillMan;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.ComboBoxEdit BegWeb;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.DateEdit bdate;
        private DevExpress.XtraEditors.DateEdit edate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.SimpleButton cbClose;
        private DevExpress.XtraEditors.SimpleButton cbRetrieve;
        private DevExpress.XtraEditors.ComboBoxEdit StartSite;
        private DevExpress.XtraEditors.ComboBoxEdit TransferSite;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private System.Windows.Forms.ImageList imageList2;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private ZQTMS.Lib.MyGridControl myGridControl1;
        private ZQTMS.Lib.MyGridView myGridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem4;
        private DevExpress.XtraBars.BarButtonItem barButtonItem5;
        private DevExpress.XtraBars.BarButtonItem barButtonItem6;
        private DevExpress.XtraBars.BarButtonItem barButtonItem7;
        private System.Windows.Forms.ImageList imageList3;
    }
}