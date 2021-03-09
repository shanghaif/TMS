namespace ZQTMS.UI
{
    partial class JMfrmArrivalCount
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JMfrmArrivalCount));
            this.myGridControl1 = new ZQTMS.Lib.MyGridControl();
            this.myGridView1 = new ZQTMS.Lib.MyGridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.repositoryItemImageComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.isOrder = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.edwebid = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label9 = new System.Windows.Forms.Label();
            this.TransferMode = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.cbClose = new DevExpress.XtraEditors.SimpleButton();
            this.cbRetrieve = new DevExpress.XtraEditors.SimpleButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.edate = new DevExpress.XtraEditors.DateEdit();
            this.bdate = new DevExpress.XtraEditors.DateEdit();
            this.edesite = new DevExpress.XtraEditors.ComboBoxEdit();
            this.gridColumn83 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.barCheckItem1 = new DevExpress.XtraBars.BarCheckItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.barCheckItem2 = new DevExpress.XtraBars.BarCheckItem();
            this.barSubItem2 = new DevExpress.XtraBars.BarSubItem();
            this.barbtnPrintQSD = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem4 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem5 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem6 = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barbtnPrintAll = new DevExpress.XtraBars.BarButtonItem();
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.myGridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.isOrder.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edwebid.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TransferMode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edesite.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // myGridControl1
            // 
            this.myGridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myGridControl1.Location = new System.Drawing.Point(0, 67);
            this.myGridControl1.MainView = this.myGridView1;
            this.myGridControl1.Name = "myGridControl1";
            this.myGridControl1.Size = new System.Drawing.Size(783, 313);
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
            this.myGridView1.GridViewRemark = "到货货量统计";
            this.myGridView1.Guid = new System.Guid("a3c5a378-75f0-4800-ab29-3020faa73a29");
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
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Shell32 040.ico");
            this.imageList1.Images.SetKeyName(1, "Shell32 023.ico");
            this.imageList1.Images.SetKeyName(2, "Shell32 022.ico");
            this.imageList1.Images.SetKeyName(3, "Shell32 028.ico");
            this.imageList1.Images.SetKeyName(4, "Shell32 029.ico");
            this.imageList1.Images.SetKeyName(5, "Shell32 035.ico");
            this.imageList1.Images.SetKeyName(6, "Shell32 132.ico");
            this.imageList1.Images.SetKeyName(7, "Shell32 172.ico");
            this.imageList1.Images.SetKeyName(8, "check01c.gif");
            this.imageList1.Images.SetKeyName(9, "Shell32 048.ico");
            this.imageList1.Images.SetKeyName(10, "Clip.ico");
            this.imageList1.Images.SetKeyName(11, "Shell32 055.ico");
            this.imageList1.Images.SetKeyName(12, "icon_xls1.gif");
            this.imageList1.Images.SetKeyName(13, "Shell32 136.ico");
            this.imageList1.Images.SetKeyName(14, "Shell32 147.ico");
            this.imageList1.Images.SetKeyName(15, "Shell32 156.ico");
            this.imageList1.Images.SetKeyName(16, "delete.png");
            this.imageList1.Images.SetKeyName(17, "cadenas1.ico");
            this.imageList1.Images.SetKeyName(18, "Shell32 190.ico");
            this.imageList1.Images.SetKeyName(19, "Shell32 017.ico");
            this.imageList1.Images.SetKeyName(20, "Shell32 146.ico");
            // 
            // repositoryItemImageComboBox1
            // 
            this.repositoryItemImageComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemImageComboBox1.Items.AddRange(new DevExpress.XtraEditors.Controls.ImageComboBoxItem[] {
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("广州", 0, -1),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("上海", 1, -1),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("南京", 2, -1),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("北京", 3, -1),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("昆明", 4, -1),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("沈阳", 5, -1)});
            this.repositoryItemImageComboBox1.Name = "repositoryItemImageComboBox1";
            // 
            // panelControl1
            // 
            this.panelControl1.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.panelControl1.Appearance.Options.UseBackColor = true;
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.isOrder);
            this.panelControl1.Controls.Add(this.label5);
            this.panelControl1.Controls.Add(this.edwebid);
            this.panelControl1.Controls.Add(this.label9);
            this.panelControl1.Controls.Add(this.TransferMode);
            this.panelControl1.Controls.Add(this.label3);
            this.panelControl1.Controls.Add(this.cbClose);
            this.panelControl1.Controls.Add(this.cbRetrieve);
            this.panelControl1.Controls.Add(this.label4);
            this.panelControl1.Controls.Add(this.label2);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Controls.Add(this.edate);
            this.panelControl1.Controls.Add(this.bdate);
            this.panelControl1.Controls.Add(this.edesite);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(783, 67);
            this.panelControl1.TabIndex = 0;
            // 
            // isOrder
            // 
            this.isOrder.EditValue = "";
            this.isOrder.Location = new System.Drawing.Point(459, 38);
            this.isOrder.Name = "isOrder";
            this.isOrder.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.isOrder.Properties.Items.AddRange(new object[] {
            "全部",
            "有",
            "无"});
            this.isOrder.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.isOrder.Size = new System.Drawing.Size(98, 21);
            this.isOrder.TabIndex = 55;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(398, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 14);
            this.label5.TabIndex = 54;
            this.label5.Text = "是否预约";
            // 
            // edwebid
            // 
            this.edwebid.EditValue = "";
            this.edwebid.Location = new System.Drawing.Point(252, 38);
            this.edwebid.Name = "edwebid";
            this.edwebid.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.edwebid.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.edwebid.Size = new System.Drawing.Size(140, 21);
            this.edwebid.TabIndex = 53;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(215, 41);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(31, 14);
            this.label9.TabIndex = 52;
            this.label9.Text = "网点";
            // 
            // TransferMode
            // 
            this.TransferMode.EditValue = "";
            this.TransferMode.Location = new System.Drawing.Point(459, 10);
            this.TransferMode.Name = "TransferMode";
            this.TransferMode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.TransferMode.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.TransferMode.Size = new System.Drawing.Size(98, 21);
            this.TransferMode.TabIndex = 51;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(398, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 14);
            this.label3.TabIndex = 50;
            this.label3.Text = "交接方式";
            // 
            // cbClose
            // 
            this.cbClose.Appearance.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbClose.Appearance.Options.UseFont = true;
            this.cbClose.Image = global::ZQTMS.UI.Properties.Resources.Action_Close;
            this.cbClose.Location = new System.Drawing.Point(654, 13);
            this.cbClose.Name = "cbClose";
            this.cbClose.ShowToolTips = false;
            this.cbClose.Size = new System.Drawing.Size(58, 42);
            this.cbClose.TabIndex = 49;
            this.cbClose.Text = "退出";
            this.cbClose.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Hand;
            this.cbClose.Click += new System.EventHandler(this.cbClose_Click);
            // 
            // cbRetrieve
            // 
            this.cbRetrieve.Appearance.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbRetrieve.Appearance.Options.UseFont = true;
            this.cbRetrieve.Image = global::ZQTMS.UI.Properties.Resources.Action_Search;
            this.cbRetrieve.Location = new System.Drawing.Point(584, 13);
            this.cbRetrieve.Name = "cbRetrieve";
            this.cbRetrieve.ShowToolTips = false;
            this.cbRetrieve.Size = new System.Drawing.Size(58, 42);
            this.cbRetrieve.TabIndex = 48;
            this.cbRetrieve.Text = "提取";
            this.cbRetrieve.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Hand;
            this.cbRetrieve.ToolTipTitle = "帮助";
            this.cbRetrieve.Click += new System.EventHandler(this.cbRetrieve_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(31, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 14);
            this.label4.TabIndex = 47;
            this.label4.Text = "站点";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(227, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 14);
            this.label2.TabIndex = 43;
            this.label2.Text = "到";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 14);
            this.label1.TabIndex = 42;
            this.label1.Text = "发车时间";
            // 
            // edate
            // 
            this.edate.EditValue = new System.DateTime(2006, 2, 10, 0, 0, 0, 0);
            this.edate.Location = new System.Drawing.Point(252, 10);
            this.edate.Name = "edate";
            this.edate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.edate.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.edate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.edate.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.edate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.edate.Properties.Mask.EditMask = "yyyy-MM-dd HH:mm:ss";
            this.edate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.edate.Size = new System.Drawing.Size(140, 21);
            this.edate.TabIndex = 41;
            // 
            // bdate
            // 
            this.bdate.EditValue = new System.DateTime(2006, 2, 10, 0, 0, 0, 0);
            this.bdate.Location = new System.Drawing.Point(68, 10);
            this.bdate.Name = "bdate";
            this.bdate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.bdate.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.bdate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.bdate.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.bdate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.bdate.Properties.Mask.EditMask = "yyyy-MM-dd HH:mm:ss";
            this.bdate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.bdate.Size = new System.Drawing.Size(141, 21);
            this.bdate.TabIndex = 40;
            // 
            // edesite
            // 
            this.edesite.Cursor = System.Windows.Forms.Cursors.Hand;
            this.edesite.EditValue = "";
            this.edesite.Location = new System.Drawing.Point(68, 38);
            this.edesite.Name = "edesite";
            this.edesite.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.edesite.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.edesite.Size = new System.Drawing.Size(141, 21);
            this.edesite.TabIndex = 45;
            this.edesite.EditValueChanged += new System.EventHandler(this.edesite_EditValueChanged);
            // 
            // gridColumn83
            // 
            this.gridColumn83.Caption = "gridColumn83";
            this.gridColumn83.Name = "gridColumn83";
            this.gridColumn83.Visible = true;
            this.gridColumn83.VisibleIndex = 50;
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Images = this.imageList3;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barSubItem1,
            this.barButtonItem1,
            this.barCheckItem1,
            this.barButtonItem2,
            this.barButtonItem3,
            this.barCheckItem2,
            this.barSubItem2,
            this.barButtonItem4,
            this.barButtonItem5,
            this.barButtonItem6,
            this.barbtnPrintAll,
            this.barbtnPrintQSD});
            this.barManager1.MaxItemId = 14;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar1.FloatLocation = new System.Drawing.Point(40, 615);
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barSubItem1, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barbtnPrintQSD, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem4, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem5, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem6, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar1.Text = "Tools";
            // 
            // barSubItem1
            // 
            this.barSubItem1.Caption = "外观设置";
            this.barSubItem1.Id = 0;
            this.barSubItem1.ImageIndex = 30;
            this.barSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barCheckItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem3),
            new DevExpress.XtraBars.LinkPersistInfo(this.barCheckItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem2)});
            this.barSubItem1.Name = "barSubItem1";
            // 
            // barCheckItem1
            // 
            this.barCheckItem1.Caption = "自动筛选";
            this.barCheckItem1.Id = 2;
            this.barCheckItem1.ImageIndex = 1;
            this.barCheckItem1.Name = "barCheckItem1";
            this.barCheckItem1.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem1_CheckedChanged_1);
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "锁定外观";
            this.barButtonItem2.Id = 3;
            this.barButtonItem2.ImageIndex = 0;
            this.barButtonItem2.Name = "barButtonItem2";
            this.barButtonItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem2_ItemClick);
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "取消外观";
            this.barButtonItem3.Id = 4;
            this.barButtonItem3.ImageIndex = 20;
            this.barButtonItem3.Name = "barButtonItem3";
            this.barButtonItem3.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem3_ItemClick);
            // 
            // barCheckItem2
            // 
            this.barCheckItem2.Caption = "过滤器";
            this.barCheckItem2.Id = 5;
            this.barCheckItem2.ImageIndex = 7;
            this.barCheckItem2.Name = "barCheckItem2";
            this.barCheckItem2.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem2_CheckedChanged);
            // 
            // barSubItem2
            // 
            this.barSubItem2.Caption = "冻结列";
            this.barSubItem2.Id = 6;
            this.barSubItem2.ImageIndex = 10;
            this.barSubItem2.Name = "barSubItem2";
            // 
            // barbtnPrintQSD
            // 
            this.barbtnPrintQSD.Caption = "预约送货";
            this.barbtnPrintQSD.Id = 13;
            this.barbtnPrintQSD.ImageIndex = 38;
            this.barbtnPrintQSD.Name = "barbtnPrintQSD";
            this.barbtnPrintQSD.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barbtnPrintQSD_ItemClick);
            // 
            // barButtonItem4
            // 
            this.barButtonItem4.Caption = "快找";
            this.barButtonItem4.Id = 7;
            this.barButtonItem4.ImageIndex = 29;
            this.barButtonItem4.Name = "barButtonItem4";
            // 
            // barButtonItem5
            // 
            this.barButtonItem5.Caption = "导出";
            this.barButtonItem5.Id = 8;
            this.barButtonItem5.ImageIndex = 33;
            this.barButtonItem5.Name = "barButtonItem5";
            this.barButtonItem5.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem5_ItemClick);
            // 
            // barButtonItem6
            // 
            this.barButtonItem6.Caption = "退出";
            this.barButtonItem6.Id = 9;
            this.barButtonItem6.ImageIndex = 28;
            this.barButtonItem6.Name = "barButtonItem6";
            this.barButtonItem6.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem6_ItemClick);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "自动筛选";
            this.barButtonItem1.Id = 1;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // barbtnPrintAll
            // 
            this.barbtnPrintAll.Caption = "当前显示的所有记录";
            this.barbtnPrintAll.Id = 12;
            this.barbtnPrintAll.ImageIndex = 19;
            this.barbtnPrintAll.Name = "barbtnPrintAll";
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
            // 
            // JMfrmArrivalCount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(783, 406);
            this.Controls.Add(this.myGridControl1);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.Name = "JMfrmArrivalCount";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "到货货量统计";
            this.Load += new System.EventHandler(this.frmSendRecord_Load);
            ((System.ComponentModel.ISupportInitialize)(this.myGridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.isOrder.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edwebid.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TransferMode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edesite.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox repositoryItemImageComboBox1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton cbClose;
        private DevExpress.XtraEditors.SimpleButton cbRetrieve;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.DateEdit edate;
        private DevExpress.XtraEditors.DateEdit bdate;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.ComboBoxEdit @TransferMode;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn83;
        private DevExpress.XtraEditors.ComboBoxEdit edwebid;
        private System.Windows.Forms.Label label9;
        private ZQTMS.Lib.MyGridControl myGridControl1;
        private ZQTMS.Lib.MyGridView myGridView1;
        private DevExpress.XtraEditors.ComboBoxEdit edesite;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.ComboBoxEdit isOrder;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraBars.BarCheckItem barCheckItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarCheckItem barCheckItem2;
        private DevExpress.XtraBars.BarSubItem barSubItem2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem4;
        private DevExpress.XtraBars.BarButtonItem barButtonItem5;
        private DevExpress.XtraBars.BarButtonItem barButtonItem6;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barbtnPrintAll;
        private DevExpress.XtraBars.BarButtonItem barbtnPrintQSD;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private System.Windows.Forms.ImageList imageList3;




    }
}