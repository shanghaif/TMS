namespace ZQTMS.UI
{
    partial class p_BillStock
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(p_BillStock));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.sBtnSearch = new DevExpress.XtraEditors.SimpleButton();
            this.cobeBillType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.sBtnExit = new DevExpress.XtraEditors.SimpleButton();
            this.gcRepertory = new DevExpress.XtraGrid.GridControl();
            this.gvRepertory = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.tbilltype = new DevExpress.XtraGrid.Columns.GridColumn();
            this.zrk = new DevExpress.XtraGrid.Columns.GridColumn();
            this.yck = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.sykc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.cobeBillType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcRepertory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvRepertory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Shell32 023.ico");
            this.imageList1.Images.SetKeyName(1, "Shell32 028.ico");
            this.imageList1.Images.SetKeyName(2, "refresh.JPG");
            // 
            // sBtnSearch
            // 
            this.sBtnSearch.ImageIndex = 29;
            this.sBtnSearch.ImageList = this.imageList3;
            this.sBtnSearch.Location = new System.Drawing.Point(209, 8);
            this.sBtnSearch.Name = "sBtnSearch";
            this.sBtnSearch.Size = new System.Drawing.Size(59, 23);
            this.sBtnSearch.TabIndex = 2;
            this.sBtnSearch.Text = "提 取";
            this.sBtnSearch.Click += new System.EventHandler(this.sBtnSearch_Click);
            // 
            // cobeBillType
            // 
            this.cobeBillType.Location = new System.Drawing.Point(85, 9);
            this.cobeBillType.Name = "cobeBillType";
            this.cobeBillType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cobeBillType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cobeBillType.Size = new System.Drawing.Size(100, 21);
            this.cobeBillType.TabIndex = 3;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(14, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "按票据类型";
            // 
            // sBtnExit
            // 
            this.sBtnExit.ImageIndex = 28;
            this.sBtnExit.ImageList = this.imageList3;
            this.sBtnExit.Location = new System.Drawing.Point(284, 8);
            this.sBtnExit.Name = "sBtnExit";
            this.sBtnExit.Size = new System.Drawing.Size(59, 23);
            this.sBtnExit.TabIndex = 5;
            this.sBtnExit.Text = "退 出";
            this.sBtnExit.Click += new System.EventHandler(this.sBtnExit_Click);
            // 
            // gcRepertory
            // 
            this.gcRepertory.Dock = System.Windows.Forms.DockStyle.Top;
            this.gcRepertory.Location = new System.Drawing.Point(0, 40);
            this.gcRepertory.MainView = this.gvRepertory;
            this.gcRepertory.Name = "gcRepertory";
            this.gcRepertory.Size = new System.Drawing.Size(922, 169);
            this.gcRepertory.TabIndex = 6;
            this.gcRepertory.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvRepertory});
            // 
            // gvRepertory
            // 
            this.gvRepertory.ColumnPanelRowHeight = 30;
            this.gvRepertory.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.tbilltype,
            this.zrk,
            this.yck,
            this.gridColumn1,
            this.sykc});
            this.gvRepertory.GridControl = this.gcRepertory;
            this.gvRepertory.Name = "gvRepertory";
            this.gvRepertory.OptionsView.ColumnAutoWidth = false;
            this.gvRepertory.OptionsView.ShowFooter = true;
            this.gvRepertory.OptionsView.ShowGroupPanel = false;
            this.gvRepertory.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.gvRepertory_RowStyle);
            // 
            // tbilltype
            // 
            this.tbilltype.AppearanceCell.Options.UseTextOptions = true;
            this.tbilltype.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.tbilltype.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.tbilltype.AppearanceHeader.Options.UseTextOptions = true;
            this.tbilltype.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.tbilltype.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.tbilltype.Caption = "票据类型";
            this.tbilltype.FieldName = "tbilltype";
            this.tbilltype.MinWidth = 100;
            this.tbilltype.Name = "tbilltype";
            this.tbilltype.OptionsColumn.AllowEdit = false;
            this.tbilltype.OptionsColumn.AllowFocus = false;
            this.tbilltype.SummaryItem.DisplayFormat = "{0}";
            this.tbilltype.SummaryItem.FieldName = "obilltype";
            this.tbilltype.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count;
            this.tbilltype.Visible = true;
            this.tbilltype.VisibleIndex = 0;
            this.tbilltype.Width = 100;
            // 
            // zrk
            // 
            this.zrk.AppearanceCell.Options.UseTextOptions = true;
            this.zrk.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.zrk.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.zrk.AppearanceHeader.Options.UseTextOptions = true;
            this.zrk.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.zrk.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.zrk.Caption = "总入库";
            this.zrk.FieldName = "zrk";
            this.zrk.MinWidth = 75;
            this.zrk.Name = "zrk";
            this.zrk.OptionsColumn.AllowEdit = false;
            this.zrk.OptionsColumn.AllowFocus = false;
            this.zrk.SummaryItem.DisplayFormat = "{0}";
            this.zrk.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            this.zrk.Visible = true;
            this.zrk.VisibleIndex = 1;
            this.zrk.Width = 100;
            // 
            // yck
            // 
            this.yck.AppearanceCell.Options.UseTextOptions = true;
            this.yck.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.yck.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.yck.AppearanceHeader.Options.UseTextOptions = true;
            this.yck.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.yck.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.yck.Caption = "已出库";
            this.yck.FieldName = "yck";
            this.yck.MinWidth = 75;
            this.yck.Name = "yck";
            this.yck.OptionsColumn.AllowEdit = false;
            this.yck.OptionsColumn.AllowFocus = false;
            this.yck.SummaryItem.DisplayFormat = "{0}";
            this.yck.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            this.yck.Visible = true;
            this.yck.VisibleIndex = 2;
            this.yck.Width = 100;
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "已作废";
            this.gridColumn1.FieldName = "zf";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.OptionsColumn.AllowFocus = false;
            this.gridColumn1.SummaryItem.DisplayFormat = "{0}";
            this.gridColumn1.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 3;
            // 
            // sykc
            // 
            this.sykc.AppearanceCell.Options.UseTextOptions = true;
            this.sykc.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.sykc.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.sykc.AppearanceHeader.Options.UseTextOptions = true;
            this.sykc.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.sykc.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.sykc.Caption = "未出库";
            this.sykc.FieldName = "sykc";
            this.sykc.MinWidth = 75;
            this.sykc.Name = "sykc";
            this.sykc.OptionsColumn.AllowEdit = false;
            this.sykc.OptionsColumn.AllowFocus = false;
            this.sykc.SummaryItem.DisplayFormat = "{0}";
            this.sykc.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            this.sykc.Visible = true;
            this.sykc.VisibleIndex = 4;
            this.sykc.Width = 100;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.cobeBillType);
            this.panelControl1.Controls.Add(this.sBtnSearch);
            this.panelControl1.Controls.Add(this.sBtnExit);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(922, 40);
            this.panelControl1.TabIndex = 7;
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 209);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(922, 343);
            this.gridControl1.TabIndex = 8;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.ColumnPanelRowHeight = 30;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn2,
            this.gridColumn7,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn2.Caption = "票据类型";
            this.gridColumn2.FieldName = "tbilltype";
            this.gridColumn2.MinWidth = 100;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.OptionsColumn.AllowFocus = false;
            this.gridColumn2.SummaryItem.DisplayFormat = "{0}";
            this.gridColumn2.SummaryItem.FieldName = "obilltype";
            this.gridColumn2.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 0;
            this.gridColumn2.Width = 100;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "网点";
            this.gridColumn7.FieldName = "webid";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsColumn.AllowEdit = false;
            this.gridColumn7.OptionsColumn.AllowFocus = false;
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 1;
            this.gridColumn7.Width = 203;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn3.Caption = "领取总数";
            this.gridColumn3.FieldName = "total";
            this.gridColumn3.MinWidth = 75;
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.OptionsColumn.AllowFocus = false;
            this.gridColumn3.SummaryItem.DisplayFormat = "{0}";
            this.gridColumn3.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 100;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn4.Caption = "已使用";
            this.gridColumn4.FieldName = "ysy";
            this.gridColumn4.MinWidth = 75;
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.OptionsColumn.AllowFocus = false;
            this.gridColumn4.SummaryItem.DisplayFormat = "{0}";
            this.gridColumn4.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            this.gridColumn4.Width = 100;
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.Caption = "已作废";
            this.gridColumn5.FieldName = "zf";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowEdit = false;
            this.gridColumn5.OptionsColumn.AllowFocus = false;
            this.gridColumn5.SummaryItem.DisplayFormat = "{0}";
            this.gridColumn5.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 4;
            this.gridColumn5.Width = 88;
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.gridColumn6.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn6.Caption = "未使用";
            this.gridColumn6.FieldName = "sykc";
            this.gridColumn6.MinWidth = 75;
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.AllowEdit = false;
            this.gridColumn6.OptionsColumn.AllowFocus = false;
            this.gridColumn6.SummaryItem.DisplayFormat = "{0}";
            this.gridColumn6.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 5;
            this.gridColumn6.Width = 100;
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
            // p_BillStock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(922, 552);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.gcRepertory);
            this.Controls.Add(this.panelControl1);
            this.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.Name = "p_BillStock";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "票据库存";
            this.Load += new System.EventHandler(this.RepertoryForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cobeBillType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcRepertory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvRepertory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton sBtnSearch;
        private DevExpress.XtraEditors.ComboBoxEdit cobeBillType;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraEditors.SimpleButton sBtnExit;
        private DevExpress.XtraGrid.GridControl gcRepertory;
        private DevExpress.XtraGrid.Views.Grid.GridView gvRepertory;
        private DevExpress.XtraGrid.Columns.GridColumn tbilltype;
        private DevExpress.XtraGrid.Columns.GridColumn yck;
        private DevExpress.XtraGrid.Columns.GridColumn zrk;
        private DevExpress.XtraGrid.Columns.GridColumn sykc;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private System.Windows.Forms.ImageList imageList3;

    }
}