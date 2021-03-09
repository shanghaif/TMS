namespace ZQTMS.UI
{
    partial class frmCusData
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCusData));
            this.myGridControl1 = new ZQTMS.Lib.MyGridControl();
            this.myGridView1 = new ZQTMS.Lib.MyGridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar11 = new DevExpress.XtraBars.Bar();
            this.barBtnCusAdd = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnCusMod = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnCusDel = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnCusFilter = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnCusFresh = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnvCusExport = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnCusExit = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControl9 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl10 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl11 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl12 = new DevExpress.XtraBars.BarDockControl();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.myGridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // myGridControl1
            // 
            this.myGridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myGridControl1.Location = new System.Drawing.Point(0, 0);
            this.myGridControl1.MainView = this.myGridView1;
            this.myGridControl1.Name = "myGridControl1";
            this.myGridControl1.Size = new System.Drawing.Size(672, 386);
            this.myGridControl1.TabIndex = 6;
            this.myGridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.myGridView1});
            // 
            // myGridView1
            // 
            this.myGridView1.ColumnPanelRowHeight = 30;
            this.myGridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1});
            this.myGridView1.GridControl = this.myGridControl1;
            this.myGridView1.GridViewRemark = "发货人资料表";
            this.myGridView1.Guid = new System.Guid("a729bdc6-6184-4c2c-8f1d-866740510045");
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
            this.barBtnCusAdd,
            this.barBtnCusMod,
            this.barBtnCusDel,
            this.barBtnCusFilter,
            this.barBtnCusFresh,
            this.barBtnvCusExport,
            this.barBtnCusExit,
            this.barButtonItem1});
            this.barManager1.MaxItemId = 8;
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
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barBtnCusAdd, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barBtnCusMod, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barBtnCusDel, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barBtnCusFilter, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barBtnCusFresh, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barBtnvCusExport, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barBtnCusExit, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem1, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar11.OptionsBar.AllowQuickCustomization = false;
            this.bar11.OptionsBar.DrawDragBorder = false;
            this.bar11.OptionsBar.UseWholeRow = true;
            this.bar11.Text = "Tools";
            // 
            // barBtnCusAdd
            // 
            this.barBtnCusAdd.Caption = "新增";
            this.barBtnCusAdd.Id = 0;
            this.barBtnCusAdd.ImageIndex = 21;
            this.barBtnCusAdd.Name = "barBtnCusAdd";
            this.barBtnCusAdd.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnCusAdd_ItemClick);
            // 
            // barBtnCusMod
            // 
            this.barBtnCusMod.Caption = "修改";
            this.barBtnCusMod.Id = 1;
            this.barBtnCusMod.ImageIndex = 23;
            this.barBtnCusMod.Name = "barBtnCusMod";
            this.barBtnCusMod.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnCusMod_ItemClick);
            // 
            // barBtnCusDel
            // 
            this.barBtnCusDel.Caption = "删除";
            this.barBtnCusDel.Id = 2;
            this.barBtnCusDel.ImageIndex = 22;
            this.barBtnCusDel.Name = "barBtnCusDel";
            this.barBtnCusDel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnCusDel_ItemClick);
            // 
            // barBtnCusFilter
            // 
            this.barBtnCusFilter.Caption = "过滤器";
            this.barBtnCusFilter.Id = 3;
            this.barBtnCusFilter.ImageIndex = 29;
            this.barBtnCusFilter.Name = "barBtnCusFilter";
            this.barBtnCusFilter.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnCusFilter_ItemClick);
            // 
            // barBtnCusFresh
            // 
            this.barBtnCusFresh.Caption = "刷新";
            this.barBtnCusFresh.Id = 4;
            this.barBtnCusFresh.ImageIndex = 24;
            this.barBtnCusFresh.Name = "barBtnCusFresh";
            this.barBtnCusFresh.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnCusFresh_ItemClick);
            // 
            // barBtnvCusExport
            // 
            this.barBtnvCusExport.Caption = "导出";
            this.barBtnvCusExport.Id = 5;
            this.barBtnvCusExport.ImageIndex = 33;
            this.barBtnvCusExport.Name = "barBtnvCusExport";
            this.barBtnvCusExport.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnvCusExport_ItemClick);
            // 
            // barBtnCusExit
            // 
            this.barBtnCusExit.Caption = "退出";
            this.barBtnCusExit.Id = 6;
            this.barBtnCusExit.ImageIndex = 28;
            this.barBtnCusExit.Name = "barBtnCusExit";
            this.barBtnCusExit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnCusExit_ItemClick);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "导入";
            this.barButtonItem1.Id = 7;
            this.barButtonItem1.ImageIndex = 32;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
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
            // frmCusData
            // 
            this.ClientSize = new System.Drawing.Size(672, 426);
            this.Controls.Add(this.myGridControl1);
            this.Controls.Add(this.barDockControl11);
            this.Controls.Add(this.barDockControl12);
            this.Controls.Add(this.barDockControl10);
            this.Controls.Add(this.barDockControl9);
            this.Name = "frmCusData";
            this.Text = "发货人信息";
            this.Load += new System.EventHandler(this.frmCusData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.myGridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ZQTMS.Lib.MyGridControl myGridControl1;
        private ZQTMS.Lib.MyGridView myGridView1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar11;
        private DevExpress.XtraBars.BarButtonItem barBtnCusAdd;
        private DevExpress.XtraBars.BarButtonItem barBtnCusMod;
        private DevExpress.XtraBars.BarButtonItem barBtnCusDel;
        private DevExpress.XtraBars.BarButtonItem barBtnCusFilter;
        private DevExpress.XtraBars.BarButtonItem barBtnCusFresh;
        private DevExpress.XtraBars.BarButtonItem barBtnvCusExport;
        private DevExpress.XtraBars.BarButtonItem barBtnCusExit;
        private DevExpress.XtraBars.BarDockControl barDockControl9;
        private DevExpress.XtraBars.BarDockControl barDockControl10;
        private DevExpress.XtraBars.BarDockControl barDockControl11;
        private DevExpress.XtraBars.BarDockControl barDockControl12;
        private System.Windows.Forms.ImageList imageList2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private System.Windows.Forms.ImageList imageList3;
    }
}
