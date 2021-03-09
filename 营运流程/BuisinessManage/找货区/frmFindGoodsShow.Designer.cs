namespace ZQTMS.UI
{
    partial class frmFindGoodsShow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFindGoodsShow));
            this.listView1 = new System.Windows.Forms.ListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.page = new DevExpress.XtraEditors.TextEdit();
            this.lblPageInfo = new System.Windows.Forms.Label();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.bdate = new DevExpress.XtraEditors.DateEdit();
            this.edate = new DevExpress.XtraEditors.DateEdit();
            this.FromWeb = new DevExpress.XtraEditors.ComboBoxEdit();
            this.FromArea = new DevExpress.XtraEditors.ComboBoxEdit();
            this.FromCause = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cbClose = new DevExpress.XtraEditors.SimpleButton();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.cbRetrieve = new DevExpress.XtraEditors.SimpleButton();
            this.Num = new DevExpress.XtraEditors.TextEdit();
            this.Package = new DevExpress.XtraEditors.TextEdit();
            this.Varieties = new DevExpress.XtraEditors.TextEdit();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.page.Properties)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bdate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FromWeb.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FromArea.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FromCause.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Num.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Package.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Varieties.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Location = new System.Drawing.Point(0, 70);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1330, 470);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(250, 250);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.linkLabel1);
            this.panel1.Controls.Add(this.page);
            this.panel1.Controls.Add(this.lblPageInfo);
            this.panel1.Controls.Add(this.simpleButton1);
            this.panel1.Controls.Add(this.simpleButton2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 540);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1330, 41);
            this.panel1.TabIndex = 1;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(344, 14);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(31, 14);
            this.linkLabel1.TabIndex = 5;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "跳转";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // page
            // 
            this.page.EnterMoveNextControl = true;
            this.page.Location = new System.Drawing.Point(280, 11);
            this.page.Name = "page";
            this.page.Size = new System.Drawing.Size(58, 21);
            this.page.TabIndex = 4;
            // 
            // lblPageInfo
            // 
            this.lblPageInfo.AutoSize = true;
            this.lblPageInfo.Location = new System.Drawing.Point(399, 14);
            this.lblPageInfo.Name = "lblPageInfo";
            this.lblPageInfo.Size = new System.Drawing.Size(0, 14);
            this.lblPageInfo.TabIndex = 2;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(118, 9);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 23);
            this.simpleButton1.TabIndex = 0;
            this.simpleButton1.Text = "上一页";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(199, 10);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(75, 23);
            this.simpleButton2.TabIndex = 1;
            this.simpleButton2.Text = "下一页";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.bdate);
            this.panel3.Controls.Add(this.edate);
            this.panel3.Controls.Add(this.FromWeb);
            this.panel3.Controls.Add(this.FromArea);
            this.panel3.Controls.Add(this.FromCause);
            this.panel3.Controls.Add(this.cbClose);
            this.panel3.Controls.Add(this.cbRetrieve);
            this.panel3.Controls.Add(this.Num);
            this.panel3.Controls.Add(this.Package);
            this.panel3.Controls.Add(this.Varieties);
            this.panel3.Controls.Add(this.labelControl10);
            this.panel3.Controls.Add(this.labelControl9);
            this.panel3.Controls.Add(this.labelControl8);
            this.panel3.Controls.Add(this.labelControl3);
            this.panel3.Controls.Add(this.labelControl2);
            this.panel3.Controls.Add(this.labelControl1);
            this.panel3.Controls.Add(this.labelControl5);
            this.panel3.Controls.Add(this.labelControl4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1330, 70);
            this.panel3.TabIndex = 2;
            // 
            // bdate
            // 
            this.bdate.EditValue = new System.DateTime(2006, 1, 26, 0, 0, 0, 0);
            this.bdate.Location = new System.Drawing.Point(85, 9);
            this.bdate.Name = "bdate";
            this.bdate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.bdate.Properties.DisplayFormat.FormatString = "yyyy-M-d H:mm";
            this.bdate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.bdate.Properties.Mask.EditMask = "yyyy-M-d H:mm";
            this.bdate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.bdate.Size = new System.Drawing.Size(114, 21);
            this.bdate.TabIndex = 42;
            // 
            // edate
            // 
            this.edate.EditValue = new System.DateTime(2006, 1, 26, 0, 0, 0, 0);
            this.edate.Location = new System.Drawing.Point(235, 9);
            this.edate.Name = "edate";
            this.edate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.edate.Properties.DisplayFormat.FormatString = "yyyy-M-d H:mm";
            this.edate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.edate.Properties.Mask.EditMask = "yyyy-M-d H:mm";
            this.edate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.edate.Size = new System.Drawing.Size(114, 21);
            this.edate.TabIndex = 43;
            // 
            // FromWeb
            // 
            this.FromWeb.Location = new System.Drawing.Point(648, 40);
            this.FromWeb.Name = "FromWeb";
            this.FromWeb.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FromWeb.Size = new System.Drawing.Size(143, 21);
            this.FromWeb.TabIndex = 28;
            this.FromWeb.TabStop = false;
            this.FromWeb.DoubleClick += new System.EventHandler(this.SelectCondition_DoubleClick);
            // 
            // FromArea
            // 
            this.FromArea.Location = new System.Drawing.Point(648, 9);
            this.FromArea.Name = "FromArea";
            this.FromArea.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FromArea.Size = new System.Drawing.Size(143, 21);
            this.FromArea.TabIndex = 26;
            this.FromArea.TabStop = false;
            this.FromArea.SelectedIndexChanged += new System.EventHandler(this.FromArea_SelectedIndexChanged);
            this.FromArea.DoubleClick += new System.EventHandler(this.SelectCondition_DoubleClick);
            // 
            // FromCause
            // 
            this.FromCause.Location = new System.Drawing.Point(433, 9);
            this.FromCause.Name = "FromCause";
            this.FromCause.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FromCause.Size = new System.Drawing.Size(143, 21);
            this.FromCause.TabIndex = 24;
            this.FromCause.TabStop = false;
            this.FromCause.SelectedIndexChanged += new System.EventHandler(this.FromCause_SelectedIndexChanged);
            this.FromCause.DoubleClick += new System.EventHandler(this.SelectCondition_DoubleClick);
            // 
            // cbClose
            // 
            this.cbClose.Appearance.Font = new System.Drawing.Font("宋体", 10F);
            this.cbClose.Appearance.Options.UseFont = true;
            this.cbClose.Image = global::ZQTMS.UI.Properties.Resources.Action_Close;
            this.cbClose.ImageIndex = 0;
            this.cbClose.Location = new System.Drawing.Point(881, 10);
            this.cbClose.Name = "cbClose";
            this.cbClose.ShowToolTips = false;
            this.cbClose.Size = new System.Drawing.Size(61, 48);
            this.cbClose.TabIndex = 10;
            this.cbClose.Text = "退出";
            this.cbClose.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Hand;
            this.cbClose.Click += new System.EventHandler(this.cbClose_Click);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "Shell32 028.ico");
            this.imageList2.Images.SetKeyName(1, "Shell32 190.ico");
            // 
            // cbRetrieve
            // 
            this.cbRetrieve.Appearance.Font = new System.Drawing.Font("宋体", 10F);
            this.cbRetrieve.Appearance.Options.UseFont = true;
            this.cbRetrieve.Image = global::ZQTMS.UI.Properties.Resources.Action_Search;
            this.cbRetrieve.Location = new System.Drawing.Point(808, 10);
            this.cbRetrieve.Name = "cbRetrieve";
            this.cbRetrieve.ShowToolTips = false;
            this.cbRetrieve.Size = new System.Drawing.Size(61, 48);
            this.cbRetrieve.TabIndex = 9;
            this.cbRetrieve.Text = "提取";
            this.cbRetrieve.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Hand;
            this.cbRetrieve.ToolTipTitle = "帮助";
            this.cbRetrieve.Click += new System.EventHandler(this.cbRetrieve_Click);
            // 
            // Num
            // 
            this.Num.EnterMoveNextControl = true;
            this.Num.Location = new System.Drawing.Point(433, 40);
            this.Num.Name = "Num";
            this.Num.Size = new System.Drawing.Size(143, 21);
            this.Num.TabIndex = 7;
            // 
            // Package
            // 
            this.Package.EnterMoveNextControl = true;
            this.Package.Location = new System.Drawing.Point(235, 40);
            this.Package.Name = "Package";
            this.Package.Size = new System.Drawing.Size(114, 21);
            this.Package.TabIndex = 5;
            // 
            // Varieties
            // 
            this.Varieties.EnterMoveNextControl = true;
            this.Varieties.Location = new System.Drawing.Point(85, 40);
            this.Varieties.Name = "Varieties";
            this.Varieties.Size = new System.Drawing.Size(114, 21);
            this.Varieties.TabIndex = 3;
            // 
            // labelControl10
            // 
            this.labelControl10.Location = new System.Drawing.Point(582, 43);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(60, 14);
            this.labelControl10.TabIndex = 29;
            this.labelControl10.Text = "来源网点：";
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(582, 12);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(60, 14);
            this.labelControl9.TabIndex = 27;
            this.labelControl9.Text = "来源大区：";
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(355, 12);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(72, 14);
            this.labelControl8.TabIndex = 25;
            this.labelControl8.Text = "来源事业部：";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(367, 43);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(60, 14);
            this.labelControl3.TabIndex = 6;
            this.labelControl3.Text = "件　　数：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(205, 43);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(24, 14);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "包装";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(31, 43);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "品　　名";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(217, 12);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(12, 14);
            this.labelControl5.TabIndex = 45;
            this.labelControl5.Text = "到";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(19, 12);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 14);
            this.labelControl4.TabIndex = 44;
            this.labelControl4.Text = "上传时间从";
            // 
            // frmFindGoodsShow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1330, 581);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Name = "frmFindGoodsShow";
            this.Text = "无标货图片预览";
            this.Load += new System.EventHandler(this.frmFindGoodsShow_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.page.Properties)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bdate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FromWeb.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FromArea.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FromCause.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Num.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Package.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Varieties.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        public System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private System.Windows.Forms.Panel panel3;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit Varieties;
        private DevExpress.XtraEditors.TextEdit Package;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit Num;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton cbClose;
        private DevExpress.XtraEditors.SimpleButton cbRetrieve;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.ComboBoxEdit FromCause;
        private DevExpress.XtraEditors.ComboBoxEdit FromArea;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.ComboBoxEdit FromWeb;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.Label lblPageInfo;
        private DevExpress.XtraEditors.TextEdit page;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private DevExpress.XtraEditors.DateEdit bdate;
        private DevExpress.XtraEditors.DateEdit edate;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
    }
}