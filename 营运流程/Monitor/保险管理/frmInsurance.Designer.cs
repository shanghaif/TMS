namespace ZQTMS.UI
{
    partial class frmInsurance
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInsurance));
            this.FrmDelByTel = new DevExpress.XtraBars.BarButtonItem();
            this.gcInsurance = new ZQTMS.Lib.MyGridControl();
            this.myGridView3 = new ZQTMS.Lib.MyGridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FrmDelByBillNo = new DevExpress.XtraBars.BarButtonItem();
            this.label2 = new System.Windows.Forms.Label();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem4 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem5 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem6 = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem2 = new DevExpress.XtraBars.BarSubItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.FrmDelbyFocus = new DevExpress.XtraBars.BarButtonItem();
            this.barModify = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            this.barBillDetail = new DevExpress.XtraBars.BarButtonItem();
            this.barInsuranceInfo = new DevExpress.XtraBars.BarButtonItem();
            this.barAlter = new DevExpress.XtraBars.BarButtonItem();
            this.edate = new DevExpress.XtraEditors.DateEdit();
            this.bdate = new DevExpress.XtraEditors.DateEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbRetrieve = new DevExpress.XtraEditors.SimpleButton();
            this.cbClose = new DevExpress.XtraEditors.SimpleButton();
            this.billno = new DevExpress.XtraEditors.TextEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.cbbState = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gcInsurance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myGridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.billno.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbbState.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            this.SuspendLayout();
            // 
            // FrmDelByTel
            // 
            this.FrmDelByTel.Caption = "按手机号删除";
            this.FrmDelByTel.Id = 9;
            this.FrmDelByTel.ImageIndex = 6;
            this.FrmDelByTel.Name = "FrmDelByTel";
            // 
            // gcInsurance
            // 
            this.gcInsurance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcInsurance.Location = new System.Drawing.Point(2, 57);
            this.gcInsurance.MainView = this.myGridView3;
            this.gcInsurance.Name = "gcInsurance";
            this.gcInsurance.Size = new System.Drawing.Size(852, 411);
            this.gcInsurance.TabIndex = 2;
            this.gcInsurance.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.myGridView3});
            // 
            // myGridView3
            // 
            this.myGridView3.ColumnPanelRowHeight = 30;
            this.myGridView3.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1});
            this.myGridView3.CustomizationFormBounds = new System.Drawing.Rectangle(1006, 333, 216, 250);
            this.myGridView3.GridControl = this.gcInsurance;
            this.myGridView3.Guid = new System.Guid("c6cf54f4-aaf3-48b2-a8af-8ecceb18c40b");
            this.myGridView3.HiddenFiledDic = ((System.Collections.Generic.Dictionary<string, object>)(resources.GetObject("myGridView3.HiddenFiledDic")));
            this.myGridView3.MenuName = "";
            this.myGridView3.Name = "myGridView3";
            this.myGridView3.OptionsView.ColumnAutoWidth = false;
            this.myGridView3.OptionsView.ShowAutoFilterRow = true;
            this.myGridView3.OptionsView.ShowGroupPanel = false;
            this.myGridView3.WebControlBindFindName = "";
            this.myGridView3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.myGridView3_MouseUp);
            this.myGridView3.DoubleClick += new System.EventHandler(this.myGridView3_DoubleClick);
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
            // FrmDelByBillNo
            // 
            this.FrmDelByBillNo.Caption = "按单号删除";
            this.FrmDelByBillNo.Id = 8;
            this.FrmDelByBillNo.ImageIndex = 6;
            this.FrmDelByBillNo.Name = "FrmDelByBillNo";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(334, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 14);
            this.label2.TabIndex = 61;
            this.label2.Text = "订单号:";
            this.label2.Visible = false;
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
            this.barModify,
            this.FrmDelByBillNo,
            this.FrmDelByTel,
            this.FrmDelbyFocus,
            this.barBillDetail,
            this.barInsuranceInfo,
            this.barAlter,
            this.barSubItem2});
            this.barManager1.MaxItemId = 17;
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
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.FrmDelbyFocus, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barModify, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem2, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // barSubItem1
            // 
            this.barSubItem1.Caption = "外观设置";
            this.barSubItem1.Id = 0;
            this.barSubItem1.ImageIndex = 30;
            this.barSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem3, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem4, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem5, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem6, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem2)});
            this.barSubItem1.Name = "barSubItem1";
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "自动筛选";
            this.barButtonItem3.Id = 3;
            this.barButtonItem3.ImageIndex = 1;
            this.barButtonItem3.Name = "barButtonItem3";
            this.barButtonItem3.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem3_ItemClick);
            // 
            // barButtonItem4
            // 
            this.barButtonItem4.Caption = "取消锁定";
            this.barButtonItem4.Id = 4;
            this.barButtonItem4.ImageIndex = 5;
            this.barButtonItem4.Name = "barButtonItem4";
            this.barButtonItem4.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem4_ItemClick);
            // 
            // barButtonItem5
            // 
            this.barButtonItem5.Caption = "外观锁定";
            this.barButtonItem5.Id = 5;
            this.barButtonItem5.ImageIndex = 0;
            this.barButtonItem5.Name = "barButtonItem5";
            this.barButtonItem5.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem5_ItemClick);
            // 
            // barButtonItem6
            // 
            this.barButtonItem6.Caption = "过滤器";
            this.barButtonItem6.Id = 6;
            this.barButtonItem6.ImageIndex = 7;
            this.barButtonItem6.Name = "barButtonItem6";
            this.barButtonItem6.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem6_ItemClick);
            // 
            // barSubItem2
            // 
            this.barSubItem2.Caption = "冻结列";
            this.barSubItem2.Id = 16;
            this.barSubItem2.ImageIndex = 10;
            this.barSubItem2.Name = "barSubItem2";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "导出明细";
            this.barButtonItem1.Id = 1;
            this.barButtonItem1.ImageIndex = 33;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // FrmDelbyFocus
            // 
            this.FrmDelbyFocus.Caption = "删除";
            this.FrmDelbyFocus.Id = 10;
            this.FrmDelbyFocus.ImageIndex = 22;
            this.FrmDelbyFocus.Name = "FrmDelbyFocus";
            this.FrmDelbyFocus.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // barModify
            // 
            this.barModify.Caption = "修改";
            this.barModify.Id = 7;
            this.barModify.ImageIndex = 23;
            this.barModify.Name = "barModify";
            this.barModify.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barModify.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barModify_ItemClick);
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "退出";
            this.barButtonItem2.Id = 2;
            this.barButtonItem2.ImageIndex = 28;
            this.barButtonItem2.Name = "barButtonItem2";
            this.barButtonItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem2_ItemClick);
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
            // barBillDetail
            // 
            this.barBillDetail.Caption = "运单信息";
            this.barBillDetail.Id = 11;
            this.barBillDetail.Name = "barBillDetail";
            this.barBillDetail.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBillDetail_ItemClick);
            // 
            // barInsuranceInfo
            // 
            this.barInsuranceInfo.Caption = "投保信息";
            this.barInsuranceInfo.Id = 12;
            this.barInsuranceInfo.Name = "barInsuranceInfo";
            this.barInsuranceInfo.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barInsuranceInfo_ItemClick);
            // 
            // barAlter
            // 
            this.barAlter.Caption = "修改";
            this.barAlter.Id = 13;
            this.barAlter.Name = "barAlter";
            this.barAlter.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barAlter_ItemClick);
            // 
            // edate
            // 
            this.edate.EditValue = new System.DateTime(2016, 1, 26, 0, 0, 0, 0);
            this.edate.Location = new System.Drawing.Point(196, 13);
            this.edate.Name = "edate";
            this.edate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.edate.Properties.DisplayFormat.FormatString = "yyyy-M-d H:mm";
            this.edate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.edate.Properties.Mask.EditMask = "yyyy-M-d H:mm";
            this.edate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.edate.Size = new System.Drawing.Size(119, 21);
            this.edate.TabIndex = 42;
            // 
            // bdate
            // 
            this.bdate.EditValue = new System.DateTime(2016, 1, 26, 0, 0, 0, 0);
            this.bdate.Location = new System.Drawing.Point(60, 13);
            this.bdate.Name = "bdate";
            this.bdate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.bdate.Properties.DisplayFormat.FormatString = "yyyy-M-d H:mm";
            this.bdate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.bdate.Properties.Mask.EditMask = "yyyy-M-d H:mm";
            this.bdate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.bdate.Size = new System.Drawing.Size(120, 21);
            this.bdate.TabIndex = 41;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(180, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(19, 14);
            this.label3.TabIndex = 40;
            this.label3.Text = "到";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(4, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 14);
            this.label1.TabIndex = 28;
            this.label1.Text = "开单日期:";
            // 
            // cbRetrieve
            // 
            this.cbRetrieve.Appearance.Font = new System.Drawing.Font("宋体", 9F);
            this.cbRetrieve.Appearance.Options.UseFont = true;
            this.cbRetrieve.Image = global::ZQTMS.UI.Properties.Resources.Action_Search;
            this.cbRetrieve.Location = new System.Drawing.Point(519, 13);
            this.cbRetrieve.Name = "cbRetrieve";
            this.cbRetrieve.ShowToolTips = false;
            this.cbRetrieve.Size = new System.Drawing.Size(62, 25);
            this.cbRetrieve.TabIndex = 32;
            this.cbRetrieve.Text = "提取";
            this.cbRetrieve.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Hand;
            this.cbRetrieve.ToolTipTitle = "帮助";
            this.cbRetrieve.Click += new System.EventHandler(this.cbRetrieve_Click);
            // 
            // cbClose
            // 
            this.cbClose.Appearance.Font = new System.Drawing.Font("宋体", 9F);
            this.cbClose.Appearance.Options.UseFont = true;
            this.cbClose.ImageIndex = 28;
            this.cbClose.ImageList = this.imageList3;
            this.cbClose.Location = new System.Drawing.Point(587, 14);
            this.cbClose.Name = "cbClose";
            this.cbClose.ShowToolTips = false;
            this.cbClose.Size = new System.Drawing.Size(62, 25);
            this.cbClose.TabIndex = 58;
            this.cbClose.Text = "退出";
            this.cbClose.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Hand;
            // 
            // billno
            // 
            this.billno.Location = new System.Drawing.Point(387, 13);
            this.billno.MenuManager = this.barManager1;
            this.billno.Name = "billno";
            this.billno.Size = new System.Drawing.Size(122, 21);
            this.billno.TabIndex = 62;
            this.billno.Visible = false;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.cbbState);
            this.panelControl1.Controls.Add(this.label4);
            this.panelControl1.Controls.Add(this.billno);
            this.panelControl1.Controls.Add(this.label2);
            this.panelControl1.Controls.Add(this.edate);
            this.panelControl1.Controls.Add(this.cbClose);
            this.panelControl1.Controls.Add(this.cbRetrieve);
            this.panelControl1.Controls.Add(this.bdate);
            this.panelControl1.Controls.Add(this.label3);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(2, 2);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(852, 55);
            this.panelControl1.TabIndex = 1;
            this.panelControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.panelControl1_Paint);
            // 
            // cbbState
            // 
            this.cbbState.Location = new System.Drawing.Point(389, 14);
            this.cbbState.Name = "cbbState";
            this.cbbState.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbbState.Properties.Items.AddRange(new object[] {
            "未发送请求",
            "请求失败",
            "请求成功",
            "全部"});
            this.cbbState.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbbState.Size = new System.Drawing.Size(120, 21);
            this.cbbState.TabIndex = 64;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(332, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 14);
            this.label4.TabIndex = 63;
            this.label4.Text = "请求状态:";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.gcInsurance);
            this.panelControl2.Controls.Add(this.panelControl1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(856, 470);
            this.panelControl2.TabIndex = 12;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "bsite.jpg");
            this.imageList1.Images.SetKeyName(1, "middlesite.jpg");
            this.imageList1.Images.SetKeyName(2, "qubie1.ico");
            // 
            // popupMenu1
            // 
            this.popupMenu1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barBillDetail),
            new DevExpress.XtraBars.LinkPersistInfo(this.barInsuranceInfo),
            new DevExpress.XtraBars.LinkPersistInfo(this.barAlter)});
            this.popupMenu1.Manager = this.barManager1;
            this.popupMenu1.Name = "popupMenu1";
            // 
            // frmInsurance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 496);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmInsurance";
            this.Text = "保险管理";
            this.Load += new System.EventHandler(this.frmInsurance_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcInsurance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myGridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.billno.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbbState.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarButtonItem FrmDelByTel;
        private ZQTMS.Lib.MyGridControl gcInsurance;
        private ZQTMS.Lib.MyGridView myGridView3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraBars.BarButtonItem FrmDelByBillNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ImageList imageList2;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarButtonItem barButtonItem4;
        private DevExpress.XtraBars.BarButtonItem barButtonItem5;
        private DevExpress.XtraBars.BarButtonItem barButtonItem6;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem FrmDelbyFocus;
        private DevExpress.XtraBars.BarButtonItem barModify;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraEditors.DateEdit edate;
        private DevExpress.XtraEditors.DateEdit bdate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SimpleButton cbRetrieve;
        private DevExpress.XtraEditors.SimpleButton cbClose;
        private DevExpress.XtraEditors.TextEdit billno;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.ComboBoxEdit cbbState;
        private DevExpress.XtraBars.BarButtonItem barBillDetail;
        private DevExpress.XtraBars.BarButtonItem barInsuranceInfo;
        private DevExpress.XtraBars.BarButtonItem barAlter;
        private DevExpress.XtraBars.PopupMenu popupMenu1;
        private DevExpress.XtraBars.BarSubItem barSubItem2;
        private System.Windows.Forms.ImageList imageList3;

    }
}