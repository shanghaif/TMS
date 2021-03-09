namespace ZQTMS.UI
{
    partial class frmEmployeeManage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEmployeeManage));
            this.myGridControl1 = new ZQTMS.Lib.MyGridControl();
            this.myGridView1 = new ZQTMS.Lib.MyGridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar11 = new DevExpress.XtraBars.Bar();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.barButtonItem4 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnUserFilter = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem2 = new DevExpress.XtraBars.BarSubItem();
            this.barBtnUserAdd = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnUserMod = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnvUserExport = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnUserFresh = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnUserExit = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControl9 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl10 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl11 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl12 = new DevExpress.XtraBars.BarDockControl();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.barBtnUserDel = new DevExpress.XtraBars.BarButtonItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.label2 = new System.Windows.Forms.Label();
            this.WebName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.simpleButton12 = new DevExpress.XtraEditors.SimpleButton();
            this.label4 = new System.Windows.Forms.Label();
            this.AreaName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.CauseName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cbClose = new DevExpress.XtraEditors.SimpleButton();
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.myGridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WebName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AreaName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CauseName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // myGridControl1
            // 
            this.myGridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myGridControl1.Location = new System.Drawing.Point(0, 43);
            this.myGridControl1.MainView = this.myGridView1;
            this.myGridControl1.Name = "myGridControl1";
            this.myGridControl1.Size = new System.Drawing.Size(831, 315);
            this.myGridControl1.TabIndex = 5;
            this.myGridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.myGridView1});
            // 
            // myGridView1
            // 
            this.myGridView1.ColumnPanelRowHeight = 30;
            this.myGridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1});
            this.myGridView1.GridControl = this.myGridControl1;
            this.myGridView1.GridViewRemark = "员工信息";
            this.myGridView1.Guid = new System.Guid("a84aac10-c6e8-465c-867c-5a05b0a6e98e");
            this.myGridView1.HiddenFiledDic = ((System.Collections.Generic.Dictionary<string, object>)(resources.GetObject("myGridView1.HiddenFiledDic")));
            this.myGridView1.MenuName = "";
            this.myGridView1.Name = "myGridView1";
            this.myGridView1.OptionsBehavior.Editable = false;
            this.myGridView1.OptionsView.ColumnAutoWidth = false;
            this.myGridView1.OptionsView.ShowAutoFilterRow = true;
            this.myGridView1.OptionsView.ShowGroupPanel = false;
            this.myGridView1.WebControlBindFindName = "";
            this.myGridView1.DoubleClick += new System.EventHandler(this.myGridView1_DoubleClick);
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
            this.barBtnUserAdd,
            this.barBtnUserMod,
            this.barBtnUserDel,
            this.barBtnUserFilter,
            this.barBtnUserFresh,
            this.barBtnvUserExport,
            this.barBtnUserExit,
            this.barSubItem1,
            this.barButtonItem2,
            this.barButtonItem3,
            this.barButtonItem4,
            this.barSubItem2});
            this.barManager1.MaxItemId = 15;
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
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barSubItem1, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barBtnUserAdd, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barBtnUserMod, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barBtnvUserExport, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barBtnUserFresh, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barBtnUserExit, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar11.OptionsBar.AllowQuickCustomization = false;
            this.bar11.OptionsBar.DrawDragBorder = false;
            this.bar11.OptionsBar.UseWholeRow = true;
            this.bar11.Text = "Tools";
            // 
            // barSubItem1
            // 
            this.barSubItem1.Caption = "外观设置";
            this.barSubItem1.Id = 8;
            this.barSubItem1.ImageIndex = 30;
            this.barSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem4),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem3),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barBtnUserFilter, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem2)});
            this.barSubItem1.Name = "barSubItem1";
            // 
            // barButtonItem4
            // 
            this.barButtonItem4.Caption = "自动筛选";
            this.barButtonItem4.Id = 11;
            this.barButtonItem4.ImageIndex = 1;
            this.barButtonItem4.Name = "barButtonItem4";
            this.barButtonItem4.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem4_ItemClick);
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
            // barBtnUserFilter
            // 
            this.barBtnUserFilter.Caption = "过滤器";
            this.barBtnUserFilter.Id = 3;
            this.barBtnUserFilter.ImageIndex = 7;
            this.barBtnUserFilter.Name = "barBtnUserFilter";
            this.barBtnUserFilter.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnUserFilter_ItemClick);
            // 
            // barSubItem2
            // 
            this.barSubItem2.Caption = "冻结列";
            this.barSubItem2.Id = 12;
            this.barSubItem2.ImageIndex = 10;
            this.barSubItem2.Name = "barSubItem2";
            // 
            // barBtnUserAdd
            // 
            this.barBtnUserAdd.Caption = "新增";
            this.barBtnUserAdd.Id = 0;
            this.barBtnUserAdd.ImageIndex = 21;
            this.barBtnUserAdd.Name = "barBtnUserAdd";
            this.barBtnUserAdd.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnUserAdd_ItemClick);
            // 
            // barBtnUserMod
            // 
            this.barBtnUserMod.Caption = "修改";
            this.barBtnUserMod.Id = 1;
            this.barBtnUserMod.ImageIndex = 23;
            this.barBtnUserMod.Name = "barBtnUserMod";
            this.barBtnUserMod.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnUserMod_ItemClick);
            // 
            // barBtnvUserExport
            // 
            this.barBtnvUserExport.Caption = "导出";
            this.barBtnvUserExport.Id = 5;
            this.barBtnvUserExport.ImageIndex = 33;
            this.barBtnvUserExport.Name = "barBtnvUserExport";
            this.barBtnvUserExport.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnvUserExport_ItemClick);
            // 
            // barBtnUserFresh
            // 
            this.barBtnUserFresh.Caption = "刷新";
            this.barBtnUserFresh.Id = 4;
            this.barBtnUserFresh.ImageIndex = 24;
            this.barBtnUserFresh.Name = "barBtnUserFresh";
            this.barBtnUserFresh.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnUserFresh_ItemClick);
            // 
            // barBtnUserExit
            // 
            this.barBtnUserExit.Caption = "退出";
            this.barBtnUserExit.Id = 6;
            this.barBtnUserExit.ImageIndex = 28;
            this.barBtnUserExit.Name = "barBtnUserExit";
            this.barBtnUserExit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnUserExit_ItemClick);
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
            // barBtnUserDel
            // 
            this.barBtnUserDel.Caption = "删除";
            this.barBtnUserDel.Id = 2;
            this.barBtnUserDel.ImageIndex = 16;
            this.barBtnUserDel.Name = "barBtnUserDel";
            this.barBtnUserDel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnUserDel_ItemClick);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 358);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(831, 31);
            this.panel1.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(20, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "正在加载员工信息......";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.label2);
            this.panelControl2.Controls.Add(this.WebName);
            this.panelControl2.Controls.Add(this.simpleButton12);
            this.panelControl2.Controls.Add(this.label4);
            this.panelControl2.Controls.Add(this.AreaName);
            this.panelControl2.Controls.Add(this.label5);
            this.panelControl2.Controls.Add(this.CauseName);
            this.panelControl2.Controls.Add(this.cbClose);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(831, 43);
            this.panelControl2.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(383, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 14);
            this.label2.TabIndex = 61;
            this.label2.Text = "网点";
            // 
            // WebName
            // 
            this.WebName.Cursor = System.Windows.Forms.Cursors.Hand;
            this.WebName.EditValue = "全部";
            this.WebName.Location = new System.Drawing.Point(420, 12);
            this.WebName.Name = "WebName";
            this.WebName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.WebName.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.WebName.Size = new System.Drawing.Size(117, 21);
            this.WebName.TabIndex = 60;
            // 
            // simpleButton12
            // 
            this.simpleButton12.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.simpleButton12.Appearance.Options.UseFont = true;
            this.simpleButton12.ImageIndex = 29;
            this.simpleButton12.ImageList = this.imageList3;
            this.simpleButton12.Location = new System.Drawing.Point(578, 10);
            this.simpleButton12.Name = "simpleButton12";
            this.simpleButton12.ShowToolTips = false;
            this.simpleButton12.Size = new System.Drawing.Size(61, 23);
            this.simpleButton12.TabIndex = 7;
            this.simpleButton12.Text = "提取";
            this.simpleButton12.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Hand;
            this.simpleButton12.Click += new System.EventHandler(this.simpleButton12_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(202, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 14);
            this.label4.TabIndex = 59;
            this.label4.Text = "大区";
            // 
            // AreaName
            // 
            this.AreaName.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AreaName.EditValue = "全部";
            this.AreaName.Location = new System.Drawing.Point(239, 12);
            this.AreaName.Name = "AreaName";
            this.AreaName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.AreaName.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.AreaName.Size = new System.Drawing.Size(117, 21);
            this.AreaName.TabIndex = 3;
            this.AreaName.EditValueChanged += new System.EventHandler(this.AreaName_EditValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(9, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 14);
            this.label5.TabIndex = 57;
            this.label5.Text = "事业部";
            // 
            // CauseName
            // 
            this.CauseName.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CauseName.EditValue = "全部";
            this.CauseName.Enabled = false;
            this.CauseName.Location = new System.Drawing.Point(58, 12);
            this.CauseName.Name = "CauseName";
            this.CauseName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CauseName.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.CauseName.Size = new System.Drawing.Size(117, 21);
            this.CauseName.TabIndex = 2;
            this.CauseName.EditValueChanged += new System.EventHandler(this.CauseName_EditValueChanged);
            // 
            // cbClose
            // 
            this.cbClose.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbClose.Appearance.Options.UseFont = true;
            this.cbClose.ImageIndex = 28;
            this.cbClose.ImageList = this.imageList3;
            this.cbClose.Location = new System.Drawing.Point(652, 10);
            this.cbClose.Name = "cbClose";
            this.cbClose.ShowToolTips = false;
            this.cbClose.Size = new System.Drawing.Size(61, 23);
            this.cbClose.TabIndex = 8;
            this.cbClose.Text = "退出";
            this.cbClose.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Hand;
            this.cbClose.Click += new System.EventHandler(this.cbClose_Click);
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
            // frmEmployeeManage
            // 
            this.ClientSize = new System.Drawing.Size(831, 429);
            this.Controls.Add(this.myGridControl1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.barDockControl11);
            this.Controls.Add(this.barDockControl12);
            this.Controls.Add(this.barDockControl10);
            this.Controls.Add(this.barDockControl9);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmEmployeeManage";
            this.Text = "员工信息";
            this.Load += new System.EventHandler(this.frmUsers_Load);
            ((System.ComponentModel.ISupportInitialize)(this.myGridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WebName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AreaName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CauseName.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ZQTMS.Lib.MyGridControl myGridControl1;
        private ZQTMS.Lib.MyGridView myGridView1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar11;
        private DevExpress.XtraBars.BarButtonItem barBtnUserAdd;
        private DevExpress.XtraBars.BarButtonItem barBtnUserMod;
        private DevExpress.XtraBars.BarButtonItem barBtnUserDel;
        private DevExpress.XtraBars.BarButtonItem barBtnUserFilter;
        private DevExpress.XtraBars.BarButtonItem barBtnUserFresh;
        private DevExpress.XtraBars.BarButtonItem barBtnvUserExport;
        private DevExpress.XtraBars.BarButtonItem barBtnUserExit;
        private DevExpress.XtraBars.BarDockControl barDockControl9;
        private DevExpress.XtraBars.BarDockControl barDockControl10;
        private DevExpress.XtraBars.BarDockControl barDockControl11;
        private DevExpress.XtraBars.BarDockControl barDockControl12;
        private System.Windows.Forms.ImageList imageList2;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarButtonItem barButtonItem4;
        private DevExpress.XtraBars.BarSubItem barSubItem2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton simpleButton12;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.ComboBoxEdit AreaName;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.ComboBoxEdit CauseName;
        private DevExpress.XtraEditors.SimpleButton cbClose;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.ComboBoxEdit WebName;
        private System.Windows.Forms.ImageList imageList3;

    }
}
