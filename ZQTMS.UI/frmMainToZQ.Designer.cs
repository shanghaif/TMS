namespace ZQTMS.UI
{
    partial class frmMainToZQ
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMainToZQ));
            this.ribbonMain = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.imageCollection = new DevExpress.Utils.ImageCollection(this.components);
            this.barExit = new DevExpress.XtraBars.BarStaticItem();
            this.barWeCome = new DevExpress.XtraBars.BarStaticItem();
            this.barEnd = new DevExpress.XtraBars.BarStaticItem();
            this.barQuery = new DevExpress.XtraBars.BarStaticItem();
            this.barHelp = new DevExpress.XtraBars.BarStaticItem();
            this.repositoryItemButtonEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemTextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemRichTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit();
            this.repositoryItemButtonEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemTextEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemComboBox2 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.defaultLookAndFeel = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.imageListTreeList = new System.Windows.Forms.ImageList(this.components);
            this.xtraTabbedMdiManager = new DevExpress.XtraTabbedMdi.XtraTabbedMdiManager(this.components);
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.edunit = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRichTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edunit.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonMain
            // 
            this.ribbonMain.ApplicationIcon = ((System.Drawing.Bitmap)(resources.GetObject("ribbonMain.ApplicationIcon")));
            this.ribbonMain.Images = this.imageCollection;
            this.ribbonMain.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barExit,
            this.barWeCome,
            this.barEnd,
            this.barQuery,
            this.barHelp});
            this.ribbonMain.Location = new System.Drawing.Point(0, 0);
            this.ribbonMain.MaxItemId = 27;
            this.ribbonMain.Name = "ribbonMain";
            this.ribbonMain.PageHeaderItemLinks.Add(this.barQuery);
            this.ribbonMain.PageHeaderItemLinks.Add(this.barExit);
            this.ribbonMain.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemButtonEdit1,
            this.repositoryItemTextEdit1,
            this.repositoryItemTextEdit2,
            this.repositoryItemRichTextEdit1,
            this.repositoryItemButtonEdit2,
            this.repositoryItemTextEdit3,
            this.repositoryItemComboBox1,
            this.repositoryItemComboBox2});
            this.ribbonMain.Size = new System.Drawing.Size(1213, 53);
            this.ribbonMain.StatusBar = this.ribbonStatusBar;
            this.ribbonMain.Toolbar.ItemLinks.Add(this.barHelp);
            this.ribbonMain.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ribbonMain_ItemClick);
            // 
            // imageCollection
            // 
            this.imageCollection.ImageSize = new System.Drawing.Size(48, 48);
            this.imageCollection.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection.ImageStream")));
            this.imageCollection.Images.SetKeyName(0, "box_address.png");
            this.imageCollection.Images.SetKeyName(1, "button_delete_red.png");
            this.imageCollection.Images.SetKeyName(2, "button_plus_red.png");
            this.imageCollection.Images.SetKeyName(3, "desktop_aqua_blue.png");
            this.imageCollection.Images.SetKeyName(4, "exec.png");
            this.imageCollection.Images.SetKeyName(5, "help.png");
            this.imageCollection.Images.SetKeyName(6, "printer.png");
            this.imageCollection.Images.SetKeyName(7, "search.png");
            this.imageCollection.Images.SetKeyName(8, "system_log_out.png");
            this.imageCollection.Images.SetKeyName(9, "user.png");
            this.imageCollection.Images.SetKeyName(10, "user_group_colored.png");
            this.imageCollection.Images.SetKeyName(11, "view_sidetree.png");
            this.imageCollection.Images.SetKeyName(12, "客商管理.png");
            this.imageCollection.Images.SetKeyName(13, "流程费用.png");
            this.imageCollection.Images.SetKeyName(14, "matrix.png");
            this.imageCollection.Images.SetKeyName(15, "users.png");
            this.imageCollection.Images.SetKeyName(16, "货量统计管理.png");
            this.imageCollection.Images.SetKeyName(17, "gnome_finance.png");
            this.imageCollection.Images.SetKeyName(18, "invoice.png");
            this.imageCollection.Images.SetKeyName(19, "car.png");
            this.imageCollection.Images.SetKeyName(20, "调度派车.png");
            this.imageCollection.Images.SetKeyName(21, "browser.png");
            this.imageCollection.Images.SetKeyName(22, "car_key.png");
            this.imageCollection.Images.SetKeyName(23, "car_repair.png");
            this.imageCollection.Images.SetKeyName(24, "company.png");
            this.imageCollection.Images.SetKeyName(25, "credit_cards.png");
            this.imageCollection.Images.SetKeyName(26, "empty_atm.png");
            this.imageCollection.Images.SetKeyName(27, "passport.png");
            this.imageCollection.Images.SetKeyName(28, "time_management.png");
            this.imageCollection.Images.SetKeyName(29, "curriculum_vitae.png");
            this.imageCollection.Images.SetKeyName(30, "entry_add.png");
            this.imageCollection.Images.SetKeyName(31, "entry_save.png");
            this.imageCollection.Images.SetKeyName(32, "computers.png");
            this.imageCollection.Images.SetKeyName(33, "network.png");
            this.imageCollection.Images.SetKeyName(34, "comment_edit.png");
            this.imageCollection.Images.SetKeyName(35, "document_pen.png");
            this.imageCollection.Images.SetKeyName(36, "edit_paste.png");
            this.imageCollection.Images.SetKeyName(37, "kdeprint_testprinter.png");
            this.imageCollection.Images.SetKeyName(38, "user_female_edit.png");
            this.imageCollection.Images.SetKeyName(39, "view_process_system.png");
            this.imageCollection.Images.SetKeyName(40, "server_accounting.png");
            this.imageCollection.Images.SetKeyName(41, "approval.png");
            this.imageCollection.Images.SetKeyName(42, "presentation.png");
            this.imageCollection.Images.SetKeyName(43, "to_do_list_checked3.png");
            this.imageCollection.Images.SetKeyName(44, "cash_register_sh.png");
            this.imageCollection.Images.SetKeyName(45, "folder_green_todos.png");
            this.imageCollection.Images.SetKeyName(46, "folder_blue_todos.png");
            this.imageCollection.Images.SetKeyName(47, "money_10dollar_coins.png");
            this.imageCollection.Images.SetKeyName(48, "txt_file.png");
            this.imageCollection.Images.SetKeyName(49, "object_rotate_right.png");
            this.imageCollection.Images.SetKeyName(50, "kgpg_key1_kopete.png");
            this.imageCollection.Images.SetKeyName(51, "zip.png");
            this.imageCollection.Images.SetKeyName(52, "车辆档案.png");
            // 
            // barExit
            // 
            this.barExit.Caption = "退出";
            this.barExit.Id = 6;
            this.barExit.ImageIndex = 8;
            this.barExit.Name = "barExit";
            this.barExit.TextAlignment = System.Drawing.StringAlignment.Near;
            this.barExit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barExit_ItemClick);
            // 
            // barWeCome
            // 
            this.barWeCome.Caption = "欢迎使用  中强信息管理系统";
            this.barWeCome.Id = 10;
            this.barWeCome.Name = "barWeCome";
            this.barWeCome.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barEnd
            // 
            this.barEnd.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barEnd.Caption = "当前用户";
            this.barEnd.Id = 11;
            this.barEnd.Name = "barEnd";
            this.barEnd.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barQuery
            // 
            this.barQuery.Caption = "快找";
            this.barQuery.Id = 16;
            this.barQuery.ImageIndex = 7;
            this.barQuery.Name = "barQuery";
            this.barQuery.TextAlignment = System.Drawing.StringAlignment.Near;
            this.barQuery.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barQuery_ItemClick);
            // 
            // barHelp
            // 
            this.barHelp.Caption = "帮助";
            this.barHelp.Id = 17;
            this.barHelp.ImageIndex = 5;
            this.barHelp.Name = "barHelp";
            this.barHelp.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // repositoryItemButtonEdit1
            // 
            this.repositoryItemButtonEdit1.AutoHeight = false;
            this.repositoryItemButtonEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemButtonEdit1.Name = "repositoryItemButtonEdit1";
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // repositoryItemTextEdit2
            // 
            this.repositoryItemTextEdit2.AutoHeight = false;
            this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
            // 
            // repositoryItemRichTextEdit1
            // 
            this.repositoryItemRichTextEdit1.Name = "repositoryItemRichTextEdit1";
            // 
            // repositoryItemButtonEdit2
            // 
            this.repositoryItemButtonEdit2.AutoHeight = false;
            this.repositoryItemButtonEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemButtonEdit2.Name = "repositoryItemButtonEdit2";
            // 
            // repositoryItemTextEdit3
            // 
            this.repositoryItemTextEdit3.AutoHeight = false;
            this.repositoryItemTextEdit3.Name = "repositoryItemTextEdit3";
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // repositoryItemComboBox2
            // 
            this.repositoryItemComboBox2.AutoHeight = false;
            this.repositoryItemComboBox2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox2.Name = "repositoryItemComboBox2";
            // 
            // ribbonStatusBar
            // 
            this.ribbonStatusBar.ItemLinks.Add(this.barWeCome);
            this.ribbonStatusBar.ItemLinks.Add(this.barEnd);
            this.ribbonStatusBar.Location = new System.Drawing.Point(0, 705);
            this.ribbonStatusBar.Name = "ribbonStatusBar";
            this.ribbonStatusBar.Ribbon = this.ribbonMain;
            this.ribbonStatusBar.Size = new System.Drawing.Size(1213, 24);
            // 
            // defaultLookAndFeel
            // 
            this.defaultLookAndFeel.LookAndFeel.SkinName = "Black";
            // 
            // imageListTreeList
            // 
            this.imageListTreeList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTreeList.ImageStream")));
            this.imageListTreeList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTreeList.Images.SetKeyName(0, "submodule.ico");
            // 
            // xtraTabbedMdiManager
            // 
            this.xtraTabbedMdiManager.AllowDragDrop = DevExpress.Utils.DefaultBoolean.False;
            this.xtraTabbedMdiManager.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InActiveTabPageHeader;
            this.xtraTabbedMdiManager.MdiParent = this;
            this.xtraTabbedMdiManager.MouseUp += new System.Windows.Forms.MouseEventHandler(this.xtraTabbedMdiManager_MouseUp);
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Caption = "Exit";
            this.barStaticItem1.Id = 6;
            this.barStaticItem1.ImageIndex = 17;
            this.barStaticItem1.Name = "barStaticItem1";
            this.barStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // edunit
            // 
            this.edunit.Dock = System.Windows.Forms.DockStyle.Top;
            this.edunit.EditValue = "输入单号后回车";
            this.edunit.Location = new System.Drawing.Point(0, 53);
            this.edunit.Name = "edunit";
            this.edunit.Properties.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.edunit.Properties.Appearance.Options.UseFont = true;
            this.edunit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.edunit.Properties.Items.AddRange(new object[] {
            "按更多条件查找"});
            this.edunit.Size = new System.Drawing.Size(1213, 20);
            this.edunit.TabIndex = 21;
            this.edunit.SelectedIndexChanged += new System.EventHandler(this.edunit_SelectedIndexChanged);
            this.edunit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.edunit_KeyDown);
            this.edunit.Click += new System.EventHandler(this.edunit_Click);
            // 
            // frmMainToZQ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1213, 729);
            this.Controls.Add(this.edunit);
            this.Controls.Add(this.ribbonStatusBar);
            this.Controls.Add(this.ribbonMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.Name = "frmMainToZQ";
            this.Ribbon = this.ribbonMain;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.StatusBar = this.ribbonStatusBar;
            this.Text = "中强信息管理系统";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMainToZQ_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMainToZQ_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRichTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edunit.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonMain;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
        private DevExpress.Utils.ImageCollection imageCollection;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel;
        private DevExpress.XtraBars.BarStaticItem barExit;
        private System.Windows.Forms.ImageList imageListTreeList;
        private DevExpress.XtraTabbedMdi.XtraTabbedMdiManager xtraTabbedMdiManager;
        private DevExpress.XtraBars.BarStaticItem barWeCome;
        private DevExpress.XtraBars.BarStaticItem barEnd;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private DevExpress.XtraBars.BarStaticItem barQuery;
        private DevExpress.XtraBars.BarStaticItem barHelp;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit2;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit2;
        private DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit repositoryItemRichTextEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit3;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraEditors.ComboBoxEdit edunit;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox2;
    }
}