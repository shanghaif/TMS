namespace ZQTMS.UI
{
    partial class frmDeparture
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDeparture));
            this.gridDepartureList = new ZQTMS.Lib.MyGridControl();
            this.myGridView1 = new ZQTMS.Lib.MyGridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar11 = new DevExpress.XtraBars.Bar();
            this.barSbiSetUp = new DevExpress.XtraBars.BarSubItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem4 = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.barBtnAdd = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnMod = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnVehicleDel = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnLoadScan = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnLoadScanPeop = new DevExpress.XtraBars.BarButtonItem();
            this.updateDriverTakePay = new DevExpress.XtraBars.BarButtonItem();
            this.btnDepart = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem6 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem5 = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnvExport = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnExit = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem8 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem9 = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControl9 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl10 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl11 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl12 = new DevExpress.XtraBars.BarDockControl();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.barButtonItem7 = new DevExpress.XtraBars.BarButtonItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btn_save = new DevExpress.XtraEditors.SimpleButton();
            this.CarStartState = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.web = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.Area = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.Cause = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.edate = new DevExpress.XtraEditors.DateEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.bdate = new DevExpress.XtraEditors.DateEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.label4 = new System.Windows.Forms.Label();
            this.btnRetrieve = new DevExpress.XtraEditors.SimpleButton();
            this.bSite = new DevExpress.XtraEditors.ComboBoxEdit();
            this.eSite = new DevExpress.XtraEditors.ComboBoxEdit();
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gridDepartureList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CarStartState.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.web.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Area.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Cause.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bSite.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eSite.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gridDepartureList
            // 
            this.gridDepartureList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDepartureList.Location = new System.Drawing.Point(0, 71);
            this.gridDepartureList.MainView = this.myGridView1;
            this.gridDepartureList.Name = "gridDepartureList";
            this.gridDepartureList.Size = new System.Drawing.Size(1199, 288);
            this.gridDepartureList.TabIndex = 1;
            this.gridDepartureList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.myGridView1});
            this.gridDepartureList.DoubleClick += new System.EventHandler(this.gridDepartureList_DoubleClick);
            // 
            // myGridView1
            // 
            this.myGridView1.ColumnPanelRowHeight = 30;
            this.myGridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1});
            this.myGridView1.GridControl = this.gridDepartureList;
            this.myGridView1.GridViewRemark = "配载记录";
            this.myGridView1.Guid = new System.Guid("c0981100-a2b8-44ee-8d6f-5c7f675408b0");
            this.myGridView1.HiddenFiledDic = ((System.Collections.Generic.Dictionary<string, object>)(resources.GetObject("myGridView1.HiddenFiledDic")));
            this.myGridView1.MenuName = "";
            this.myGridView1.Name = "myGridView1";
            this.myGridView1.OptionsBehavior.Editable = false;
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
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar11});
            this.barManager1.DockControls.Add(this.barDockControl9);
            this.barManager1.DockControls.Add(this.barDockControl10);
            this.barManager1.DockControls.Add(this.barDockControl11);
            this.barManager1.DockControls.Add(this.barDockControl12);
            this.barManager1.Form = this;
            this.barManager1.Images = this.imageList3;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barBtnAdd,
            this.barBtnMod,
            this.barBtnVehicleDel,
            this.barBtnLoadScan,
            this.barBtnLoadScanPeop,
            this.barBtnvExport,
            this.barBtnExit,
            this.barSbiSetUp,
            this.barButtonItem1,
            this.barButtonItem2,
            this.barButtonItem3,
            this.barButtonItem4,
            this.barSubItem1,
            this.barButtonItem5,
            this.btnDepart,
            this.updateDriverTakePay,
            this.barButtonItem6,
            this.barButtonItem7,
            this.barButtonItem8,
            this.barButtonItem9});
            this.barManager1.MaxItemId = 22;
            // 
            // bar11
            // 
            this.bar11.BarItemVertIndent = 9;
            this.bar11.BarName = "Tools";
            this.bar11.DockCol = 0;
            this.bar11.DockRow = 0;
            this.bar11.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar11.FloatSize = new System.Drawing.Size(525, 40);
            this.bar11.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barSbiSetUp, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barBtnAdd, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barBtnMod, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barBtnVehicleDel, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barBtnLoadScan, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barBtnLoadScanPeop, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.updateDriverTakePay, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnDepart, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem6, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem5, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barBtnvExport, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barBtnExit, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem8, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem9, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar11.OptionsBar.AllowQuickCustomization = false;
            this.bar11.OptionsBar.DrawDragBorder = false;
            this.bar11.OptionsBar.UseWholeRow = true;
            this.bar11.Text = "Tools";
            // 
            // barSbiSetUp
            // 
            this.barSbiSetUp.Caption = "外观设置";
            this.barSbiSetUp.Id = 7;
            this.barSbiSetUp.ImageIndex = 30;
            this.barSbiSetUp.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem3),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem4),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem1)});
            this.barSbiSetUp.Name = "barSbiSetUp";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "自动筛选";
            this.barButtonItem1.Id = 8;
            this.barButtonItem1.ImageIndex = 1;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "锁定外观";
            this.barButtonItem2.Id = 9;
            this.barButtonItem2.ImageIndex = 0;
            this.barButtonItem2.Name = "barButtonItem2";
            this.barButtonItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem2_ItemClick);
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "取消外观";
            this.barButtonItem3.Id = 10;
            this.barButtonItem3.ImageIndex = 20;
            this.barButtonItem3.Name = "barButtonItem3";
            this.barButtonItem3.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem3_ItemClick);
            // 
            // barButtonItem4
            // 
            this.barButtonItem4.Caption = "过滤器";
            this.barButtonItem4.Id = 11;
            this.barButtonItem4.ImageIndex = 7;
            this.barButtonItem4.Name = "barButtonItem4";
            this.barButtonItem4.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem4_ItemClick);
            // 
            // barSubItem1
            // 
            this.barSubItem1.Caption = "冻结列";
            this.barSubItem1.Id = 12;
            this.barSubItem1.ImageIndex = 10;
            this.barSubItem1.Name = "barSubItem1";
            // 
            // barBtnAdd
            // 
            this.barBtnAdd.Caption = "新增";
            this.barBtnAdd.Id = 0;
            this.barBtnAdd.ImageIndex = 21;
            this.barBtnAdd.Name = "barBtnAdd";
            this.barBtnAdd.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnAdd_ItemClick);
            // 
            // barBtnMod
            // 
            this.barBtnMod.Caption = "修改";
            this.barBtnMod.Id = 1;
            this.barBtnMod.ImageIndex = 23;
            this.barBtnMod.Name = "barBtnMod";
            this.barBtnMod.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnMod_ItemClick);
            // 
            // barBtnVehicleDel
            // 
            this.barBtnVehicleDel.Caption = "整车作废";
            this.barBtnVehicleDel.Id = 2;
            this.barBtnVehicleDel.ImageIndex = 22;
            this.barBtnVehicleDel.Name = "barBtnVehicleDel";
            this.barBtnVehicleDel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnVehicleDel_ItemClick);
            // 
            // barBtnLoadScan
            // 
            this.barBtnLoadScan.Caption = "配载装车扫描统计";
            this.barBtnLoadScan.Id = 3;
            this.barBtnLoadScan.ImageIndex = 41;
            this.barBtnLoadScan.Name = "barBtnLoadScan";
            this.barBtnLoadScan.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnLoadScan_ItemClick);
            // 
            // barBtnLoadScanPeop
            // 
            this.barBtnLoadScanPeop.Caption = "配载装车扫描人统计";
            this.barBtnLoadScanPeop.Id = 4;
            this.barBtnLoadScanPeop.ImageIndex = 41;
            this.barBtnLoadScanPeop.Name = "barBtnLoadScanPeop";
            this.barBtnLoadScanPeop.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnLoadScanPeop_ItemClick);
            // 
            // updateDriverTakePay
            // 
            this.updateDriverTakePay.Caption = "修改司机代收款";
            this.updateDriverTakePay.Id = 15;
            this.updateDriverTakePay.ImageIndex = 23;
            this.updateDriverTakePay.Name = "updateDriverTakePay";
            this.updateDriverTakePay.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.updateDriverTakePay_ItemClick);
            // 
            // btnDepart
            // 
            this.btnDepart.Caption = "点击发车";
            this.btnDepart.Id = 14;
            this.btnDepart.ImageIndex = 38;
            this.btnDepart.Name = "btnDepart";
            this.btnDepart.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDepart_ItemClick);
            // 
            // barButtonItem6
            // 
            this.barButtonItem6.Caption = "取消发车";
            this.barButtonItem6.Id = 16;
            this.barButtonItem6.ImageIndex = 22;
            this.barButtonItem6.Name = "barButtonItem6";
            this.barButtonItem6.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem6_ItemClick);
            // 
            // barButtonItem5
            // 
            this.barButtonItem5.Caption = "快找";
            this.barButtonItem5.Id = 13;
            this.barButtonItem5.ImageIndex = 29;
            this.barButtonItem5.Name = "barButtonItem5";
            // 
            // barBtnvExport
            // 
            this.barBtnvExport.Caption = "导出";
            this.barBtnvExport.Id = 5;
            this.barBtnvExport.ImageIndex = 33;
            this.barBtnvExport.Name = "barBtnvExport";
            this.barBtnvExport.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnvExport_ItemClick);
            // 
            // barBtnExit
            // 
            this.barBtnExit.Caption = "退出";
            this.barBtnExit.Id = 6;
            this.barBtnExit.ImageIndex = 28;
            this.barBtnExit.Name = "barBtnExit";
            this.barBtnExit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnExit_ItemClick);
            // 
            // barButtonItem8
            // 
            this.barButtonItem8.Caption = "整车装卸费录入";
            this.barButtonItem8.Id = 18;
            this.barButtonItem8.ImageIndex = 23;
            this.barButtonItem8.Name = "barButtonItem8";
            this.barButtonItem8.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem8_ItemClick);
            // 
            // barButtonItem9
            // 
            this.barButtonItem9.Caption = "查看车辆轨迹";
            this.barButtonItem9.Id = 21;
            this.barButtonItem9.ImageIndex = 36;
            this.barButtonItem9.Name = "barButtonItem9";
            this.barButtonItem9.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem9_ItemClick);
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
            this.imageList2.Images.SetKeyName(21, "refresh.JPG");
            this.imageList2.Images.SetKeyName(22, "Shell32 058.ico");
            // 
            // barButtonItem7
            // 
            this.barButtonItem7.Caption = "barButtonItem7";
            this.barButtonItem7.Id = 17;
            this.barButtonItem7.Name = "barButtonItem7";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btn_save);
            this.panelControl1.Controls.Add(this.CarStartState);
            this.panelControl1.Controls.Add(this.label6);
            this.panelControl1.Controls.Add(this.web);
            this.panelControl1.Controls.Add(this.label5);
            this.panelControl1.Controls.Add(this.Area);
            this.panelControl1.Controls.Add(this.label8);
            this.panelControl1.Controls.Add(this.Cause);
            this.panelControl1.Controls.Add(this.label7);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Controls.Add(this.edate);
            this.panelControl1.Controls.Add(this.label2);
            this.panelControl1.Controls.Add(this.bdate);
            this.panelControl1.Controls.Add(this.label3);
            this.panelControl1.Controls.Add(this.btnClose);
            this.panelControl1.Controls.Add(this.label4);
            this.panelControl1.Controls.Add(this.btnRetrieve);
            this.panelControl1.Controls.Add(this.bSite);
            this.panelControl1.Controls.Add(this.eSite);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1199, 71);
            this.panelControl1.TabIndex = 0;
            // 
            // btn_save
            // 
            this.btn_save.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_save.Appearance.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_save.Appearance.ForeColor = System.Drawing.Color.Red;
            this.btn_save.Appearance.Options.UseFont = true;
            this.btn_save.Appearance.Options.UseForeColor = true;
            this.btn_save.Location = new System.Drawing.Point(1025, 26);
            this.btn_save.Name = "btn_save";
            this.btn_save.ShowToolTips = false;
            this.btn_save.Size = new System.Drawing.Size(79, 33);
            this.btn_save.TabIndex = 78;
            this.btn_save.Text = "?计算公式";
            this.btn_save.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Hand;
            this.btn_save.ToolTipTitle = "帮助";
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // CarStartState
            // 
            this.CarStartState.EditValue = "全部";
            this.CarStartState.Location = new System.Drawing.Point(644, 42);
            this.CarStartState.Name = "CarStartState";
            this.CarStartState.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CarStartState.Properties.Items.AddRange(new object[] {
            "未发车",
            "在途中",
            "已到车",
            "已到货",
            "全部"});
            this.CarStartState.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.CarStartState.Size = new System.Drawing.Size(143, 21);
            this.CarStartState.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(583, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 14);
            this.label6.TabIndex = 71;
            this.label6.Text = "状    态";
            // 
            // web
            // 
            this.web.Location = new System.Drawing.Point(434, 42);
            this.web.Name = "web";
            this.web.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.web.Size = new System.Drawing.Size(143, 21);
            this.web.TabIndex = 4;
            this.web.DoubleClick += new System.EventHandler(this.SelectCondition_DoubleClick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(385, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 14);
            this.label5.TabIndex = 69;
            this.label5.Text = "网　点";
            // 
            // Area
            // 
            this.Area.Location = new System.Drawing.Point(644, 11);
            this.Area.Name = "Area";
            this.Area.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.Area.Size = new System.Drawing.Size(143, 21);
            this.Area.TabIndex = 3;
            this.Area.SelectedIndexChanged += new System.EventHandler(this.Area_SelectedIndexChanged);
            this.Area.DoubleClick += new System.EventHandler(this.SelectCondition_DoubleClick);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Location = new System.Drawing.Point(583, 14);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 14);
            this.label8.TabIndex = 67;
            this.label8.Text = "大    区";
            // 
            // Cause
            // 
            this.Cause.Location = new System.Drawing.Point(434, 11);
            this.Cause.Name = "Cause";
            this.Cause.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.Cause.Size = new System.Drawing.Size(143, 21);
            this.Cause.TabIndex = 2;
            this.Cause.SelectedIndexChanged += new System.EventHandler(this.Cause_SelectedIndexChanged);
            this.Cause.DoubleClick += new System.EventHandler(this.SelectCondition_DoubleClick);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(385, 14);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 14);
            this.label7.TabIndex = 66;
            this.label7.Text = "事业部";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(7, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 14);
            this.label1.TabIndex = 56;
            this.label1.Text = "发车时间从";
            // 
            // edate
            // 
            this.edate.EditValue = new System.DateTime(2006, 1, 29, 0, 0, 0, 0);
            this.edate.Location = new System.Drawing.Point(245, 11);
            this.edate.Name = "edate";
            this.edate.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.edate.Properties.AppearanceFocused.BackColor2 = System.Drawing.Color.White;
            this.edate.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.edate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.edate.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.edate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.edate.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.edate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.edate.Properties.Mask.EditMask = "yyyy-MM-dd HH:mm";
            this.edate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.edate.Size = new System.Drawing.Size(134, 21);
            this.edate.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(7, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 14);
            this.label2.TabIndex = 57;
            this.label2.Text = "区　间　从";
            // 
            // bdate
            // 
            this.bdate.EditValue = new System.DateTime(2006, 1, 29, 0, 0, 0, 0);
            this.bdate.Location = new System.Drawing.Point(80, 11);
            this.bdate.Name = "bdate";
            this.bdate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.bdate.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.bdate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.bdate.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.bdate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.bdate.Properties.Mask.EditMask = "yyyy-MM-dd HH:mm";
            this.bdate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.bdate.Size = new System.Drawing.Size(134, 21);
            this.bdate.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(220, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(19, 14);
            this.label3.TabIndex = 58;
            this.label3.Text = "到";
            // 
            // btnClose
            // 
            this.btnClose.Appearance.Font = new System.Drawing.Font("宋体", 12F);
            this.btnClose.Appearance.Options.UseFont = true;
            this.btnClose.Image = global::ZQTMS.UI.Properties.Resources.Action_Close;
            this.btnClose.Location = new System.Drawing.Point(873, 11);
            this.btnClose.Name = "btnClose";
            this.btnClose.ShowToolTips = false;
            this.btnClose.Size = new System.Drawing.Size(60, 51);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "退出";
            this.btnClose.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Hand;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(220, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(19, 14);
            this.label4.TabIndex = 59;
            this.label4.Text = "到";
            // 
            // btnRetrieve
            // 
            this.btnRetrieve.Appearance.Font = new System.Drawing.Font("宋体", 12F);
            this.btnRetrieve.Appearance.Options.UseFont = true;
            this.btnRetrieve.Image = global::ZQTMS.UI.Properties.Resources.Action_Search;
            this.btnRetrieve.Location = new System.Drawing.Point(801, 11);
            this.btnRetrieve.Name = "btnRetrieve";
            this.btnRetrieve.ShowToolTips = false;
            this.btnRetrieve.Size = new System.Drawing.Size(60, 51);
            this.btnRetrieve.TabIndex = 8;
            this.btnRetrieve.Text = "提取";
            this.btnRetrieve.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Hand;
            this.btnRetrieve.ToolTipTitle = "帮助";
            this.btnRetrieve.Click += new System.EventHandler(this.btnRetrieve_Click);
            // 
            // bSite
            // 
            this.bSite.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bSite.EditValue = "广州";
            this.bSite.Location = new System.Drawing.Point(80, 42);
            this.bSite.MenuManager = this.barManager1;
            this.bSite.Name = "bSite";
            this.bSite.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.bSite.Properties.PopupSizeable = true;
            this.bSite.Size = new System.Drawing.Size(134, 21);
            this.bSite.TabIndex = 5;
            this.bSite.DoubleClick += new System.EventHandler(this.SelectCondition_DoubleClick);
            // 
            // eSite
            // 
            this.eSite.Cursor = System.Windows.Forms.Cursors.Hand;
            this.eSite.EditValue = "北京";
            this.eSite.Location = new System.Drawing.Point(243, 42);
            this.eSite.MenuManager = this.barManager1;
            this.eSite.Name = "eSite";
            this.eSite.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.eSite.Size = new System.Drawing.Size(136, 21);
            this.eSite.TabIndex = 6;
            this.eSite.DoubleClick += new System.EventHandler(this.SelectCondition_DoubleClick);
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
            // 
            // frmDeparture
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1199, 399);
            this.Controls.Add(this.gridDepartureList);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.barDockControl11);
            this.Controls.Add(this.barDockControl12);
            this.Controls.Add(this.barDockControl10);
            this.Controls.Add(this.barDockControl9);
            this.Name = "frmDeparture";
            this.Text = "已配载完成的发车记录";
            this.Load += new System.EventHandler(this.frmDeparture_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridDepartureList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CarStartState.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.web.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Area.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Cause.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bSite.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eSite.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ZQTMS.Lib.MyGridControl gridDepartureList;
        private ZQTMS.Lib.MyGridView myGridView1;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar11;
        private DevExpress.XtraBars.BarButtonItem barBtnAdd;
        private DevExpress.XtraBars.BarButtonItem barBtnMod;
        private DevExpress.XtraBars.BarButtonItem barBtnVehicleDel;
        private DevExpress.XtraBars.BarButtonItem barBtnLoadScan;
        private DevExpress.XtraBars.BarButtonItem barBtnLoadScanPeop;
        private DevExpress.XtraBars.BarButtonItem barBtnvExport;
        private DevExpress.XtraBars.BarButtonItem barBtnExit;
        private DevExpress.XtraBars.BarDockControl barDockControl9;
        private DevExpress.XtraBars.BarDockControl barDockControl10;
        private DevExpress.XtraBars.BarDockControl barDockControl11;
        private DevExpress.XtraBars.BarDockControl barDockControl12;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.DateEdit edate;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.DateEdit bdate;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.SimpleButton btnRetrieve;
        private DevExpress.XtraBars.BarSubItem barSbiSetUp;
        private System.Windows.Forms.ImageList imageList2;
        private DevExpress.XtraEditors.ComboBoxEdit bSite;
        private DevExpress.XtraEditors.ComboBoxEdit eSite;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarButtonItem barButtonItem4;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraEditors.ComboBoxEdit Area;
        private System.Windows.Forms.Label label8;
        private DevExpress.XtraEditors.ComboBoxEdit Cause;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.ComboBoxEdit web;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraBars.BarButtonItem barButtonItem5;
        private DevExpress.XtraEditors.ComboBoxEdit CarStartState;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraBars.BarButtonItem btnDepart;
        private DevExpress.XtraBars.BarButtonItem updateDriverTakePay;
        private DevExpress.XtraBars.BarButtonItem barButtonItem6;
        private DevExpress.XtraBars.BarButtonItem barButtonItem7;
        private DevExpress.XtraBars.BarButtonItem barButtonItem8;
        private DevExpress.XtraBars.BarButtonItem barButtonItem9;
        private DevExpress.XtraEditors.SimpleButton btn_save;
        private System.Windows.Forms.ImageList imageList3;
    }
}