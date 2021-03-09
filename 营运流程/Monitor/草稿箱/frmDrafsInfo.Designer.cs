namespace ZQTMS.UI
{
    partial class frmDrafsInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDrafsInfo));
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.txt_Volume = new DevExpress.XtraEditors.TextEdit();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.saveadd = new DevExpress.XtraBars.BarButtonItem();
            this.exit = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.txt_FeeVolume = new DevExpress.XtraEditors.TextEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.txt_billno = new DevExpress.XtraEditors.TextEdit();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.txt_Num = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.txt_FeeWeight = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txt_Weight = new DevExpress.XtraEditors.TextEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.txt_Varieties = new DevExpress.XtraEditors.TextEdit();
            this.txt_Package = new DevExpress.XtraEditors.TextEdit();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.txt_Volume.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_FeeVolume.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_billno.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Num.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_FeeWeight.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Weight.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Varieties.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Package.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(50, 128);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(36, 14);
            this.labelControl8.TabIndex = 47;
            this.labelControl8.Text = "重量：";
            // 
            // txt_Volume
            // 
            this.txt_Volume.EnterMoveNextControl = true;
            this.txt_Volume.Location = new System.Drawing.Point(338, 125);
            this.txt_Volume.MenuManager = this.barManager1;
            this.txt_Volume.Name = "txt_Volume";
            this.txt_Volume.Size = new System.Drawing.Size(162, 21);
            this.txt_Volume.TabIndex = 7;
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
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(296, 128);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(36, 14);
            this.labelControl7.TabIndex = 45;
            this.labelControl7.Text = "体积：";
            // 
            // txt_FeeVolume
            // 
            this.txt_FeeVolume.EnterMoveNextControl = true;
            this.txt_FeeVolume.Location = new System.Drawing.Point(338, 88);
            this.txt_FeeVolume.MenuManager = this.barManager1;
            this.txt_FeeVolume.Name = "txt_FeeVolume";
            this.txt_FeeVolume.Size = new System.Drawing.Size(162, 21);
            this.txt_FeeVolume.TabIndex = 5;
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.txt_billno);
            this.panelControl1.Controls.Add(this.labelControl10);
            this.panelControl1.Controls.Add(this.txt_Num);
            this.panelControl1.Controls.Add(this.labelControl5);
            this.panelControl1.Controls.Add(this.txt_FeeWeight);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.txt_Weight);
            this.panelControl1.Controls.Add(this.labelControl8);
            this.panelControl1.Controls.Add(this.txt_Volume);
            this.panelControl1.Controls.Add(this.labelControl7);
            this.panelControl1.Controls.Add(this.txt_FeeVolume);
            this.panelControl1.Controls.Add(this.labelControl6);
            this.panelControl1.Controls.Add(this.txt_Varieties);
            this.panelControl1.Controls.Add(this.txt_Package);
            this.panelControl1.Controls.Add(this.labelControl9);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 40);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(537, 171);
            this.panelControl1.TabIndex = 49;
            // 
            // txt_billno
            // 
            this.txt_billno.Enabled = false;
            this.txt_billno.EnterMoveNextControl = true;
            this.txt_billno.Location = new System.Drawing.Point(94, 16);
            this.txt_billno.MenuManager = this.barManager1;
            this.txt_billno.Name = "txt_billno";
            this.txt_billno.Size = new System.Drawing.Size(162, 21);
            this.txt_billno.TabIndex = 61;
            // 
            // labelControl10
            // 
            this.labelControl10.Location = new System.Drawing.Point(50, 19);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(36, 14);
            this.labelControl10.TabIndex = 60;
            this.labelControl10.Text = "单号：";
            // 
            // txt_Num
            // 
            this.txt_Num.EnterMoveNextControl = true;
            this.txt_Num.Location = new System.Drawing.Point(94, 52);
            this.txt_Num.MenuManager = this.barManager1;
            this.txt_Num.Name = "txt_Num";
            this.txt_Num.Size = new System.Drawing.Size(162, 21);
            this.txt_Num.TabIndex = 9;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(50, 55);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(36, 14);
            this.labelControl5.TabIndex = 55;
            this.labelControl5.Text = "件数：";
            // 
            // txt_FeeWeight
            // 
            this.txt_FeeWeight.EnterMoveNextControl = true;
            this.txt_FeeWeight.Location = new System.Drawing.Point(94, 88);
            this.txt_FeeWeight.MenuManager = this.barManager1;
            this.txt_FeeWeight.Name = "txt_FeeWeight";
            this.txt_FeeWeight.Size = new System.Drawing.Size(162, 21);
            this.txt_FeeWeight.TabIndex = 4;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(26, 91);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(60, 14);
            this.labelControl3.TabIndex = 51;
            this.labelControl3.Text = "计费重量：";
            // 
            // txt_Weight
            // 
            this.txt_Weight.EnterMoveNextControl = true;
            this.txt_Weight.Location = new System.Drawing.Point(94, 125);
            this.txt_Weight.MenuManager = this.barManager1;
            this.txt_Weight.Name = "txt_Weight";
            this.txt_Weight.Size = new System.Drawing.Size(162, 21);
            this.txt_Weight.TabIndex = 6;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(272, 91);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(60, 14);
            this.labelControl6.TabIndex = 43;
            this.labelControl6.Text = "计费体积：";
            // 
            // txt_Varieties
            // 
            this.txt_Varieties.EnterMoveNextControl = true;
            this.txt_Varieties.Location = new System.Drawing.Point(338, 16);
            this.txt_Varieties.MenuManager = this.barManager1;
            this.txt_Varieties.Name = "txt_Varieties";
            this.txt_Varieties.Size = new System.Drawing.Size(162, 21);
            this.txt_Varieties.TabIndex = 1;
            // 
            // txt_Package
            // 
            this.txt_Package.EnterMoveNextControl = true;
            this.txt_Package.Location = new System.Drawing.Point(338, 52);
            this.txt_Package.MenuManager = this.barManager1;
            this.txt_Package.Name = "txt_Package";
            this.txt_Package.Size = new System.Drawing.Size(162, 21);
            this.txt_Package.TabIndex = 3;
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(296, 55);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(36, 14);
            this.labelControl9.TabIndex = 20;
            this.labelControl9.Text = "包装：";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(294, 19);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 14);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "品名：";
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
            // frmDrafsInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 211);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmDrafsInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "编辑草稿单";
            this.Load += new System.EventHandler(this.frmDrafsInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txt_Volume.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_FeeVolume.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_billno.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Num.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_FeeWeight.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Weight.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Varieties.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Package.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.TextEdit txt_Volume;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarButtonItem saveadd;
        private DevExpress.XtraBars.BarButtonItem exit;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.TextEdit txt_FeeVolume;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.TextEdit txt_Weight;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.TextEdit txt_Varieties;
        private DevExpress.XtraEditors.TextEdit txt_Package;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txt_FeeWeight;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit txt_Num;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.TextEdit txt_billno;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private System.Windows.Forms.ImageList imageList3;
    }
}