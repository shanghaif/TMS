﻿namespace ZQTMS.UI
{
    partial class frmCustomerYsCount
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCustomerYsCount));
            this.myGridControl1 = new ZQTMS.Lib.MyGridControl();
            this.myGridView1 = new ZQTMS.Lib.MyGridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar11 = new DevExpress.XtraBars.Bar();
            this.barBtnCUFresh = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnCUFilter = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnCUExport = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnCUExit = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControl9 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl10 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl11 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl12 = new DevExpress.XtraBars.BarDockControl();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.barBtnvUserExport = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
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
            this.myGridControl1.Size = new System.Drawing.Size(707, 345);
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
            this.myGridView1.GridViewRemark = "客户档案-应收账款";
            this.myGridView1.Guid = new System.Guid("046264c6-18a8-4859-a447-94070eff5080");
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
            this.barBtnCUFilter,
            this.barBtnCUFresh,
            this.barBtnvUserExport,
            this.barBtnCUExit,
            this.barBtnCUExport});
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
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barBtnCUFresh, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barBtnCUFilter, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barBtnCUExport, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barBtnCUExit, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar11.OptionsBar.AllowQuickCustomization = false;
            this.bar11.OptionsBar.DrawDragBorder = false;
            this.bar11.OptionsBar.UseWholeRow = true;
            this.bar11.Text = "Tools";
            // 
            // barBtnCUFresh
            // 
            this.barBtnCUFresh.Caption = "刷新";
            this.barBtnCUFresh.Id = 4;
            this.barBtnCUFresh.ImageIndex = 24;
            this.barBtnCUFresh.Name = "barBtnCUFresh";
            this.barBtnCUFresh.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnCUFresh_ItemClick);
            // 
            // barBtnCUFilter
            // 
            this.barBtnCUFilter.Caption = "过滤器";
            this.barBtnCUFilter.Id = 3;
            this.barBtnCUFilter.ImageIndex = 29;
            this.barBtnCUFilter.Name = "barBtnCUFilter";
            this.barBtnCUFilter.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnCUFilter_ItemClick);
            // 
            // barBtnCUExport
            // 
            this.barBtnCUExport.Caption = "导出";
            this.barBtnCUExport.Id = 7;
            this.barBtnCUExport.ImageIndex = 33;
            this.barBtnCUExport.Name = "barBtnCUExport";
            this.barBtnCUExport.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnCUExport_ItemClick);
            // 
            // barBtnCUExit
            // 
            this.barBtnCUExit.Caption = "退出";
            this.barBtnCUExit.Id = 6;
            this.barBtnCUExit.ImageIndex = 28;
            this.barBtnCUExit.Name = "barBtnCUExit";
            this.barBtnCUExit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnCUExit_ItemClick);
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
            // barBtnvUserExport
            // 
            this.barBtnvUserExport.Caption = "导出";
            this.barBtnvUserExport.Id = 5;
            this.barBtnvUserExport.ImageIndex = 12;
            this.barBtnvUserExport.Name = "barBtnvUserExport";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "导出";
            this.barButtonItem1.Id = 5;
            this.barButtonItem1.ImageIndex = 12;
            this.barButtonItem1.Name = "barButtonItem1";
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
            // frmCustomerYsCount
            // 
            this.ClientSize = new System.Drawing.Size(707, 385);
            this.Controls.Add(this.myGridControl1);
            this.Controls.Add(this.barDockControl11);
            this.Controls.Add(this.barDockControl12);
            this.Controls.Add(this.barDockControl10);
            this.Controls.Add(this.barDockControl9);
            this.Name = "frmCustomerYsCount";
            this.Text = "应收账款查看";
            this.Load += new System.EventHandler(this.frmCustomerYsCount_Load);
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
        private DevExpress.XtraBars.BarButtonItem barBtnCUFilter;
        private DevExpress.XtraBars.BarButtonItem barBtnCUFresh;
        private DevExpress.XtraBars.BarButtonItem barBtnvUserExport;
        private DevExpress.XtraBars.BarButtonItem barBtnCUExit;
        private DevExpress.XtraBars.BarDockControl barDockControl9;
        private DevExpress.XtraBars.BarDockControl barDockControl10;
        private DevExpress.XtraBars.BarDockControl barDockControl11;
        private DevExpress.XtraBars.BarDockControl barDockControl12;
        private DevExpress.XtraBars.BarButtonItem barBtnCUExport;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private System.Windows.Forms.ImageList imageList2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private System.Windows.Forms.ImageList imageList3;
    }
}
