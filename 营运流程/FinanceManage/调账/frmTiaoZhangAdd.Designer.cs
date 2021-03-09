namespace ZQTMS.UI
{
    partial class frmTiaoZhangAdd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTiaoZhangAdd));
            this.label1 = new System.Windows.Forms.Label();
            this.FromMan = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.OperDate = new DevExpress.XtraEditors.DateEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.OperMan = new DevExpress.XtraEditors.TextEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.Account = new DevExpress.XtraEditors.TextEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.ToMan = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.Remark = new DevExpress.XtraEditors.TextEdit();
            this.label7 = new System.Windows.Forms.Label();
            this.BillNo = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.label8 = new System.Windows.Forms.Label();
            this.Project = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label9 = new System.Windows.Forms.Label();
            this.FeeType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label10 = new System.Windows.Forms.Label();
            this.AreaName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label11 = new System.Windows.Forms.Label();
            this.InOrOut = new DevExpress.XtraEditors.ComboBoxEdit();
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.FromMan.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OperDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OperDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OperMan.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Account.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToMan.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Remark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BillNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Project.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FeeType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AreaName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InOrOut.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 124);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "调账所属事业部";
            // 
            // FromMan
            // 
            this.FromMan.EnterMoveNextControl = true;
            this.FromMan.Location = new System.Drawing.Point(120, 121);
            this.FromMan.Name = "FromMan";
            this.FromMan.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FromMan.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.FromMan.Size = new System.Drawing.Size(232, 21);
            this.FromMan.TabIndex = 2;
            this.FromMan.SelectedIndexChanged += new System.EventHandler(this.FromMan_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 228);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "调账日期";
            // 
            // OperDate
            // 
            this.OperDate.EditValue = new System.DateTime(2006, 1, 26, 0, 0, 0, 0);
            this.OperDate.Location = new System.Drawing.Point(120, 224);
            this.OperDate.Name = "OperDate";
            this.OperDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.OperDate.Properties.DisplayFormat.FormatString = "yyyy-M-d H:mm";
            this.OperDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.OperDate.Properties.Mask.EditMask = "yyyy-M-d H:mm";
            this.OperDate.Properties.ReadOnly = true;
            this.OperDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.OperDate.Size = new System.Drawing.Size(232, 21);
            this.OperDate.TabIndex = 3;
            this.OperDate.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(49, 265);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "转 账 人";
            // 
            // OperMan
            // 
            this.OperMan.Location = new System.Drawing.Point(120, 261);
            this.OperMan.Name = "OperMan";
            this.OperMan.Properties.ReadOnly = true;
            this.OperMan.Size = new System.Drawing.Size(232, 21);
            this.OperMan.TabIndex = 4;
            this.OperMan.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(43, 304);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "转账金额";
            // 
            // Account
            // 
            this.Account.EnterMoveNextControl = true;
            this.Account.Location = new System.Drawing.Point(120, 302);
            this.Account.Name = "Account";
            this.Account.Size = new System.Drawing.Size(232, 21);
            this.Account.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 188);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 17);
            this.label5.TabIndex = 8;
            this.label5.Text = "调账账户名称";
            // 
            // ToMan
            // 
            this.ToMan.EnterMoveNextControl = true;
            this.ToMan.Location = new System.Drawing.Point(120, 186);
            this.ToMan.Name = "ToMan";
            this.ToMan.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ToMan.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.ToMan.Size = new System.Drawing.Size(232, 21);
            this.ToMan.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(43, 340);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 17);
            this.label6.TabIndex = 10;
            this.label6.Text = "摘　　要";
            // 
            // Remark
            // 
            this.Remark.EnterMoveNextControl = true;
            this.Remark.Location = new System.Drawing.Point(120, 338);
            this.Remark.Name = "Remark";
            this.Remark.Size = new System.Drawing.Size(232, 21);
            this.Remark.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(49, 378);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 17);
            this.label7.TabIndex = 12;
            this.label7.Text = "运 单 号";
            // 
            // BillNo
            // 
            this.BillNo.Location = new System.Drawing.Point(120, 376);
            this.BillNo.Name = "BillNo";
            this.BillNo.Size = new System.Drawing.Size(232, 21);
            this.BillNo.TabIndex = 8;
            // 
            // simpleButton2
            // 
            this.simpleButton2.ImageIndex = 28;
            this.simpleButton2.ImageList = this.imageList3;
            this.simpleButton2.Location = new System.Drawing.Point(255, 418);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(75, 23);
            this.simpleButton2.TabIndex = 10;
            this.simpleButton2.Text = "关  闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "Shell32 028.ico");
            this.imageList2.Images.SetKeyName(1, "Shell32 190.ico");
            // 
            // simpleButton1
            // 
            this.simpleButton1.ImageIndex = 25;
            this.simpleButton1.ImageList = this.imageList3;
            this.simpleButton1.Location = new System.Drawing.Point(159, 418);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 23);
            this.simpleButton1.TabIndex = 9;
            this.simpleButton1.Text = "确 定";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(43, 56);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 17);
            this.label8.TabIndex = 14;
            this.label8.Text = "汇总项目";
            // 
            // Project
            // 
            this.Project.EnterMoveNextControl = true;
            this.Project.Location = new System.Drawing.Point(120, 52);
            this.Project.Name = "Project";
            this.Project.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.Project.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.Project.Size = new System.Drawing.Size(232, 21);
            this.Project.TabIndex = 0;
            this.Project.SelectedIndexChanged += new System.EventHandler(this.Project_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(43, 90);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 17);
            this.label9.TabIndex = 16;
            this.label9.Text = "费用类型";
            // 
            // FeeType
            // 
            this.FeeType.EnterMoveNextControl = true;
            this.FeeType.Location = new System.Drawing.Point(120, 86);
            this.FeeType.Name = "FeeType";
            this.FeeType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FeeType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.FeeType.Size = new System.Drawing.Size(232, 21);
            this.FeeType.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 154);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(92, 17);
            this.label10.TabIndex = 18;
            this.label10.Text = "调账大区名称";
            // 
            // AreaName
            // 
            this.AreaName.EnterMoveNextControl = true;
            this.AreaName.Location = new System.Drawing.Point(120, 152);
            this.AreaName.Name = "AreaName";
            this.AreaName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.AreaName.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.AreaName.Size = new System.Drawing.Size(232, 21);
            this.AreaName.TabIndex = 17;
            this.AreaName.SelectedIndexChanged += new System.EventHandler(this.AreaName_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(43, 21);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(64, 17);
            this.label11.TabIndex = 20;
            this.label11.Text = "收支类型";
            // 
            // InOrOut
            // 
            this.InOrOut.EditValue = "";
            this.InOrOut.EnterMoveNextControl = true;
            this.InOrOut.Location = new System.Drawing.Point(120, 17);
            this.InOrOut.Name = "InOrOut";
            this.InOrOut.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.InOrOut.Properties.Items.AddRange(new object[] {
            "收入",
            "支出"});
            this.InOrOut.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.InOrOut.Size = new System.Drawing.Size(232, 21);
            this.InOrOut.TabIndex = 19;
            this.InOrOut.SelectedIndexChanged += new System.EventHandler(this.comboBoxEdit1_SelectedIndexChanged);
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
            // frmTiaoZhangAdd
            // 
            this.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(394, 450);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.InOrOut);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.AreaName);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.FeeType);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.Project);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.BillNo);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.Remark);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ToMan);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Account);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.OperDate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FromMan);
            this.Controls.Add(this.OperMan);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmTiaoZhangAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "调账";
            this.Load += new System.EventHandler(this.frmTiaoZhangAdd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.FromMan.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OperDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OperDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OperMan.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Account.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToMan.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Remark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BillNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Project.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FeeType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AreaName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InOrOut.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.ComboBoxEdit FromMan;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.DateEdit OperDate;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.TextEdit OperMan;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.TextEdit Account;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.ComboBoxEdit ToMan;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.TextEdit Remark;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.TextEdit BillNo;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.Label label8;
        private DevExpress.XtraEditors.ComboBoxEdit Project;
        private System.Windows.Forms.Label label9;
        private DevExpress.XtraEditors.ComboBoxEdit FeeType;
        private System.Windows.Forms.Label label10;
        private DevExpress.XtraEditors.ComboBoxEdit AreaName;
        private System.Windows.Forms.Label label11;
        private DevExpress.XtraEditors.ComboBoxEdit InOrOut;
        private System.Windows.Forms.ImageList imageList3;
    }
}