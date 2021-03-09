namespace ZQTMS.UI
{
    partial class w_add_pj
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(w_add_pj));
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.saveadd = new DevExpress.XtraBars.BarButtonItem();
            this.exit = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.saveexit = new DevExpress.XtraBars.BarButtonItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.count = new DevExpress.XtraEditors.TextEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.jhprice = new DevExpress.XtraEditors.TextEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.pjname = new DevExpress.XtraEditors.TextEdit();
            this.danwei = new DevExpress.XtraEditors.TextEdit();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.count.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.jhprice.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pjname.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.danwei.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Images = this.imageList3;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.saveadd,
            this.saveexit,
            this.exit});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 5;
            // 
            // bar2
            // 
            this.bar2.BarItemVertIndent = 9;
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.saveadd, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.exit, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar2.OptionsBar.AllowQuickCustomization = false;
            this.bar2.OptionsBar.DrawDragBorder = false;
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // saveadd
            // 
            this.saveadd.Caption = "保存";
            this.saveadd.Id = 0;
            this.saveadd.ImageIndex = 25;
            this.saveadd.Name = "saveadd";
            this.saveadd.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.saveadd_ItemClick);
            // 
            // exit
            // 
            this.exit.Caption = "退出";
            this.exit.Id = 4;
            this.exit.ImageIndex = 28;
            this.exit.Name = "exit";
            this.exit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.exit_ItemClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Shell32 132.ico");
            this.imageList1.Images.SetKeyName(1, "Shell32 055.ico");
            this.imageList1.Images.SetKeyName(2, "Shell32 028.ico");
            this.imageList1.Images.SetKeyName(3, "Shell32 190.ico");
            // 
            // saveexit
            // 
            this.saveexit.Caption = "保存退出";
            this.saveexit.Id = 1;
            this.saveexit.ImageIndex = 3;
            this.saveexit.Name = "saveexit";
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.count);
            this.panelControl1.Controls.Add(this.labelControl7);
            this.panelControl1.Controls.Add(this.jhprice);
            this.panelControl1.Controls.Add(this.labelControl6);
            this.panelControl1.Controls.Add(this.pjname);
            this.panelControl1.Controls.Add(this.danwei);
            this.panelControl1.Controls.Add(this.labelControl9);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 40);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(370, 132);
            this.panelControl1.TabIndex = 49;
            // 
            // count
            // 
            this.count.EnterMoveNextControl = true;
            this.count.Location = new System.Drawing.Point(115, 178);
            this.count.MenuManager = this.barManager1;
            this.count.Name = "count";
            this.count.Size = new System.Drawing.Size(240, 21);
            this.count.TabIndex = 4;
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(49, 181);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(60, 14);
            this.labelControl7.TabIndex = 45;
            this.labelControl7.Text = "当前数量：";
            // 
            // jhprice
            // 
            this.jhprice.EnterMoveNextControl = true;
            this.jhprice.Location = new System.Drawing.Point(115, 100);
            this.jhprice.MenuManager = this.barManager1;
            this.jhprice.Name = "jhprice";
            this.jhprice.Size = new System.Drawing.Size(240, 21);
            this.jhprice.TabIndex = 3;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(73, 103);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(36, 14);
            this.labelControl6.TabIndex = 43;
            this.labelControl6.Text = "单价：";
            // 
            // pjname
            // 
            this.pjname.EnterMoveNextControl = true;
            this.pjname.Location = new System.Drawing.Point(115, 20);
            this.pjname.MenuManager = this.barManager1;
            this.pjname.Name = "pjname";
            this.pjname.Size = new System.Drawing.Size(240, 21);
            this.pjname.TabIndex = 1;
            // 
            // danwei
            // 
            this.danwei.EnterMoveNextControl = true;
            this.danwei.Location = new System.Drawing.Point(115, 58);
            this.danwei.MenuManager = this.barManager1;
            this.danwei.Name = "danwei";
            this.danwei.Size = new System.Drawing.Size(240, 21);
            this.danwei.TabIndex = 2;
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(73, 61);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(36, 14);
            this.labelControl9.TabIndex = 20;
            this.labelControl9.Text = "单位：";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(13, 23);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(96, 14);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "配件名称及规格：";
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
            // w_add_pj
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(370, 172);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "w_add_pj";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "添加配件";
            this.Load += new System.EventHandler(this.w_add_pj_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.count.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.jhprice.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pjname.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.danwei.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarButtonItem saveadd;
        private DevExpress.XtraBars.BarButtonItem saveexit;
        private DevExpress.XtraBars.BarButtonItem exit;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.TextEdit count;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.TextEdit jhprice;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.TextEdit pjname;
        private DevExpress.XtraEditors.TextEdit danwei;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.ImageList imageList3;
    }
}