namespace ZQTMS.UI
{
    partial class frmExecuteDeduction
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmExecuteDeduction));
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem5 = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.BillnoStr = new DevExpress.XtraEditors.TextEdit();
            this.label9 = new System.Windows.Forms.Label();
            this.WebName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.Principal = new DevExpress.XtraEditors.ComboBoxEdit();
            this.PrincipalPhone = new DevExpress.XtraEditors.ComboBoxEdit();
            this.AmountMoney = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.VerificationCode = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.Countdown = new DevExpress.XtraEditors.LabelControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BillnoStr.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WebName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Principal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PrincipalPhone.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AmountMoney.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VerificationCode.Properties)).BeginInit();
            this.SuspendLayout();
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
            this.barManager1.Images = this.imageList2;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItem2,
            this.barButtonItem5,
            this.barButtonItem1});
            this.barManager1.MainMenu = this.bar1;
            this.barManager1.MaxItemId = 8;
            // 
            // bar1
            // 
            this.bar1.BarName = "Custom 2";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem2, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem5, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Custom 2";
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "保存";
            this.barButtonItem2.Id = 1;
            this.barButtonItem2.ImageIndex = 18;
            this.barButtonItem2.Name = "barButtonItem2";
            this.barButtonItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem2_ItemClick);
            // 
            // barButtonItem5
            // 
            this.barButtonItem5.Caption = "退出";
            this.barButtonItem5.Id = 4;
            this.barButtonItem5.ImageIndex = 3;
            this.barButtonItem5.Name = "barButtonItem5";
            this.barButtonItem5.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem5_ItemClick);
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
            // barButtonItem1
            // 
            this.barButtonItem1.Id = 7;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // BillnoStr
            // 
            this.BillnoStr.EnterMoveNextControl = true;
            this.BillnoStr.Location = new System.Drawing.Point(101, 43);
            this.BillnoStr.Name = "BillnoStr";
            this.BillnoStr.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.BillnoStr.Properties.Appearance.Options.UseForeColor = true;
            this.BillnoStr.Properties.ReadOnly = true;
            this.BillnoStr.Size = new System.Drawing.Size(169, 21);
            this.BillnoStr.TabIndex = 202;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(22, 46);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 14);
            this.label9.TabIndex = 203;
            this.label9.Text = "运单号:";
            // 
            // WebName
            // 
            this.WebName.EnterMoveNextControl = true;
            this.WebName.Location = new System.Drawing.Point(103, 84);
            this.WebName.Name = "WebName";
            this.WebName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 11F);
            this.WebName.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.WebName.Properties.Appearance.Options.UseFont = true;
            this.WebName.Properties.Appearance.Options.UseForeColor = true;
            this.WebName.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.WebName.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.WebName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.WebName.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.WebName.Size = new System.Drawing.Size(167, 24);
            this.WebName.TabIndex = 210;
            this.WebName.SelectedValueChanged += new System.EventHandler(this.WebName_SelectedValueChanged);
            // 
            // Principal
            // 
            this.Principal.EnterMoveNextControl = true;
            this.Principal.Location = new System.Drawing.Point(103, 130);
            this.Principal.Name = "Principal";
            this.Principal.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 11F);
            this.Principal.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.Principal.Properties.Appearance.Options.UseFont = true;
            this.Principal.Properties.Appearance.Options.UseForeColor = true;
            this.Principal.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.Principal.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.Principal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.Principal.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.Principal.Size = new System.Drawing.Size(167, 24);
            this.Principal.TabIndex = 211;
            // 
            // PrincipalPhone
            // 
            this.PrincipalPhone.EnterMoveNextControl = true;
            this.PrincipalPhone.Location = new System.Drawing.Point(103, 177);
            this.PrincipalPhone.Name = "PrincipalPhone";
            this.PrincipalPhone.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 11F);
            this.PrincipalPhone.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.PrincipalPhone.Properties.Appearance.Options.UseFont = true;
            this.PrincipalPhone.Properties.Appearance.Options.UseForeColor = true;
            this.PrincipalPhone.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.PrincipalPhone.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.PrincipalPhone.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.PrincipalPhone.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.PrincipalPhone.Size = new System.Drawing.Size(167, 24);
            this.PrincipalPhone.TabIndex = 212;
            // 
            // AmountMoney
            // 
            this.AmountMoney.EnterMoveNextControl = true;
            this.AmountMoney.Location = new System.Drawing.Point(103, 224);
            this.AmountMoney.Name = "AmountMoney";
            this.AmountMoney.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.AmountMoney.Properties.Appearance.Options.UseForeColor = true;
            this.AmountMoney.Properties.ReadOnly = true;
            this.AmountMoney.Size = new System.Drawing.Size(169, 21);
            this.AmountMoney.TabIndex = 213;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 228);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 14);
            this.label1.TabIndex = 214;
            this.label1.Text = "扣费金额";
            // 
            // VerificationCode
            // 
            this.VerificationCode.EnterMoveNextControl = true;
            this.VerificationCode.Location = new System.Drawing.Point(103, 267);
            this.VerificationCode.Name = "VerificationCode";
            this.VerificationCode.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.VerificationCode.Properties.Appearance.Options.UseForeColor = true;
            this.VerificationCode.Size = new System.Drawing.Size(169, 21);
            this.VerificationCode.TabIndex = 215;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 271);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 14);
            this.label2.TabIndex = 216;
            this.label2.Text = "验证码验证:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 14);
            this.label3.TabIndex = 217;
            this.label3.Text = "扣费网点:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 14);
            this.label4.TabIndex = 218;
            this.label4.Text = "网点负责人:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 180);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 14);
            this.label5.TabIndex = 219;
            this.label5.Text = "负责人电话:";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(310, 265);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(79, 23);
            this.simpleButton1.TabIndex = 220;
            this.simpleButton1.Text = "点击发送";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // Countdown
            // 
            this.Countdown.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.Countdown.Appearance.ForeColor = System.Drawing.Color.Red;
            this.Countdown.Appearance.Options.UseFont = true;
            this.Countdown.Appearance.Options.UseForeColor = true;
            this.Countdown.Location = new System.Drawing.Point(101, 303);
            this.Countdown.Name = "Countdown";
            this.Countdown.Size = new System.Drawing.Size(0, 14);
            this.Countdown.TabIndex = 221;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // frmExecuteDeduction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 383);
            this.Controls.Add(this.Countdown);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.VerificationCode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.AmountMoney);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PrincipalPhone);
            this.Controls.Add(this.Principal);
            this.Controls.Add(this.WebName);
            this.Controls.Add(this.BillnoStr);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.MaximizeBox = false;
            this.Name = "frmExecuteDeduction";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "执行扣费";
            this.Load += new System.EventHandler(this.frmExecuteDeduction_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BillnoStr.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WebName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Principal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PrincipalPhone.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AmountMoney.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VerificationCode.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem5;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private System.Windows.Forms.ImageList imageList2;
        private DevExpress.XtraEditors.TextEdit BillnoStr;
        private System.Windows.Forms.Label label9;
        private DevExpress.XtraEditors.TextEdit VerificationCode;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.TextEdit AmountMoney;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.ComboBoxEdit PrincipalPhone;
        private DevExpress.XtraEditors.ComboBoxEdit Principal;
        private DevExpress.XtraEditors.ComboBoxEdit WebName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.LabelControl Countdown;
        private System.Windows.Forms.Timer timer1;




    }
}