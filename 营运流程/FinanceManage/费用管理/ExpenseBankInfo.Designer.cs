namespace ZQTMS.UI
{
    partial class ExpenseBankInfo
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
            this.label1 = new System.Windows.Forms.Label();
            this.edbankman = new DevExpress.XtraEditors.TextEdit();
            this.edbankcode = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.edbankname = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.edopertype = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.edaccout = new DevExpress.XtraEditors.TextEdit();
            this.label7 = new System.Windows.Forms.Label();
            this.edaccin = new DevExpress.XtraEditors.TextEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.edbilldate = new DevExpress.XtraEditors.DateEdit();
            this.edouttype = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.edbankchild = new DevExpress.XtraEditors.TextEdit();
            this.label12 = new System.Windows.Forms.Label();
            this.edsheng = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.edcity = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.textproject = new DevExpress.XtraEditors.TextEdit();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.textwd = new DevExpress.XtraEditors.TextEdit();
            this.ck_DaiDiZhang = new DevExpress.XtraEditors.CheckEdit();
            this.ck_DaiFuKuan = new DevExpress.XtraEditors.CheckEdit();
            this.label15 = new System.Windows.Forms.Label();
            this.txtBatch = new DevExpress.XtraEditors.TextEdit();
            this.edremark = new DevExpress.XtraEditors.MemoEdit();
            ((System.ComponentModel.ISupportInitialize)(this.edbankman.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edbankcode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edbankname.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edopertype.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edaccout.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edaccin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edbilldate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edbilldate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edouttype.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edbankchild.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edsheng.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edcity.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textproject.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textwd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ck_DaiDiZhang.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ck_DaiFuKuan.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBatch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edremark.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "开户姓名";
            // 
            // edbankman
            // 
            this.edbankman.EnterMoveNextControl = true;
            this.edbankman.Location = new System.Drawing.Point(96, 56);
            this.edbankman.Name = "edbankman";
            this.edbankman.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.edbankman.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.edbankman.Size = new System.Drawing.Size(127, 25);
            this.edbankman.TabIndex = 0;
            this.edbankman.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.edbankman_EditValueChanging);
            this.edbankman.Leave += new System.EventHandler(this.edbankman_Leave);
            this.edbankman.Enter += new System.EventHandler(this.edbankman_Enter);
            this.edbankman.KeyDown += new System.Windows.Forms.KeyEventHandler(this.edbankman_KeyDown);
            // 
            // edbankcode
            // 
            this.edbankcode.EnterMoveNextControl = true;
            this.edbankcode.Location = new System.Drawing.Point(294, 56);
            this.edbankcode.Name = "edbankcode";
            this.edbankcode.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.edbankcode.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.edbankcode.Size = new System.Drawing.Size(191, 25);
            this.edbankcode.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(233, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "银行账号";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(492, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "开户银行";
            // 
            // edbankname
            // 
            this.edbankname.EnterMoveNextControl = true;
            this.edbankname.Location = new System.Drawing.Point(554, 56);
            this.edbankname.Name = "edbankname";
            this.edbankname.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.edbankname.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.edbankname.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.edbankname.Properties.DropDownRows = 20;
            this.edbankname.Properties.Items.AddRange(new object[] {
            "中国工商银行",
            "中国银行",
            "中国交通银行",
            "中国农业银行",
            "中国建设银行",
            "中国邮政储蓄",
            "中国招商银行",
            "中国中信银行",
            "中国兴业银行",
            "中国浦东发展银行",
            "中国华夏银行",
            "中国民生银行",
            "中国深圳发展银行",
            "中国光大银行",
            "广东发展银行"});
            this.edbankname.Size = new System.Drawing.Size(137, 25);
            this.edbankname.TabIndex = 2;
            this.edbankname.Enter += new System.EventHandler(this.edbankname_Enter);
            this.edbankname.Leave += new System.EventHandler(this.edbankname_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(233, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 18);
            this.label4.TabIndex = 6;
            this.label4.Text = "所属省份";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(492, 86);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 18);
            this.label5.TabIndex = 8;
            this.label5.Text = "所属城市";
            // 
            // edopertype
            // 
            this.edopertype.EnterMoveNextControl = true;
            this.edopertype.Location = new System.Drawing.Point(96, 115);
            this.edopertype.Name = "edopertype";
            this.edopertype.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.edopertype.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.edopertype.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.edopertype.Properties.Items.AddRange(new object[] {
            "行内转账",
            "同城跨行",
            "异地跨行"});
            this.edopertype.Size = new System.Drawing.Size(127, 25);
            this.edopertype.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(36, 116);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 18);
            this.label6.TabIndex = 10;
            this.label6.Text = "转账类型";
            // 
            // edaccout
            // 
            this.edaccout.EnterMoveNextControl = true;
            this.edaccout.Location = new System.Drawing.Point(294, 115);
            this.edaccout.Name = "edaccout";
            this.edaccout.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.edaccout.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.edaccout.Properties.ReadOnly = true;
            this.edaccout.Size = new System.Drawing.Size(191, 25);
            this.edaccout.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(233, 116);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 18);
            this.label7.TabIndex = 12;
            this.label7.Text = "支出金额";
            // 
            // edaccin
            // 
            this.edaccin.EnterMoveNextControl = true;
            this.edaccin.Location = new System.Drawing.Point(851, 100);
            this.edaccin.Name = "edaccin";
            this.edaccin.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.edaccin.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.edaccin.Size = new System.Drawing.Size(127, 25);
            this.edaccin.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(790, 103);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(68, 18);
            this.label8.TabIndex = 14;
            this.label8.Text = "收入金额";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(33, 179);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 18);
            this.label9.TabIndex = 16;
            this.label9.Text = "付款日期";
            // 
            // edbilldate
            // 
            this.edbilldate.EditValue = null;
            this.edbilldate.EnterMoveNextControl = true;
            this.edbilldate.Location = new System.Drawing.Point(96, 175);
            this.edbilldate.Name = "edbilldate";
            this.edbilldate.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.edbilldate.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.edbilldate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.edbilldate.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.edbilldate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.edbilldate.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.edbilldate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.edbilldate.Properties.Mask.EditMask = "yyyy-MM-dd HH:mm:ss";
            this.edbilldate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.edbilldate.Size = new System.Drawing.Size(127, 25);
            this.edbilldate.TabIndex = 11;
            // 
            // edouttype
            // 
            this.edouttype.EnterMoveNextControl = true;
            this.edouttype.Location = new System.Drawing.Point(554, 115);
            this.edouttype.Name = "edouttype";
            this.edouttype.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.edouttype.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.edouttype.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.edouttype.Properties.Items.AddRange(new object[] {
            "员工",
            "客户"});
            this.edouttype.Size = new System.Drawing.Size(137, 25);
            this.edouttype.TabIndex = 8;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(492, 116);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(68, 18);
            this.label10.TabIndex = 18;
            this.label10.Text = "支出类型";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(233, 179);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(68, 18);
            this.label11.TabIndex = 20;
            this.label11.Text = "备      注";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(183, 264);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 23);
            this.simpleButton1.TabIndex = 13;
            this.simpleButton1.Text = "保     存";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(369, 264);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(75, 23);
            this.simpleButton2.TabIndex = 14;
            this.simpleButton2.Text = "取     消";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // gridControl1
            // 
            this.gridControl1.Location = new System.Drawing.Point(670, 219);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(655, 202);
            this.gridControl1.TabIndex = 13;
            this.gridControl1.TabStop = false;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            this.gridControl1.Visible = false;
            this.gridControl1.DoubleClick += new System.EventHandler(this.gridControl1_DoubleClick);
            this.gridControl1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridControl1_KeyDown);
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.ShowAutoFilterRow = true;
            this.gridView1.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowHorzLines = false;
            this.gridView1.OptionsView.ShowVertLines = false;
            this.gridView1.RowHeight = 20;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "账户";
            this.gridColumn1.FieldName = "bankman";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.OptionsColumn.AllowFocus = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "卡号";
            this.gridColumn2.FieldName = "bankcode";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.OptionsColumn.AllowFocus = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 160;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "开户行";
            this.gridColumn3.FieldName = "bankname";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.OptionsColumn.AllowFocus = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "所属省份";
            this.gridColumn4.FieldName = "sheng";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.OptionsColumn.AllowFocus = false;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "所属城市";
            this.gridColumn5.FieldName = "city";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowEdit = false;
            this.gridColumn5.OptionsColumn.AllowFocus = false;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 4;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "转账类型";
            this.gridColumn6.FieldName = "opertype";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.AllowEdit = false;
            this.gridColumn6.OptionsColumn.AllowFocus = false;
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 5;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "支出类型";
            this.gridColumn7.FieldName = "outtype";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsColumn.AllowEdit = false;
            this.gridColumn7.OptionsColumn.AllowFocus = false;
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 6;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "开户支行";
            this.gridColumn8.FieldName = "bankchild";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.OptionsColumn.AllowEdit = false;
            this.gridColumn8.OptionsColumn.AllowFocus = false;
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 7;
            // 
            // edbankchild
            // 
            this.edbankchild.EnterMoveNextControl = true;
            this.edbankchild.Location = new System.Drawing.Point(96, 84);
            this.edbankchild.Name = "edbankchild";
            this.edbankchild.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.edbankchild.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.edbankchild.Size = new System.Drawing.Size(127, 25);
            this.edbankchild.TabIndex = 3;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(35, 86);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(68, 18);
            this.label12.TabIndex = 927;
            this.label12.Text = "开户支行";
            // 
            // edsheng
            // 
            this.edsheng.EnterMoveNextControl = true;
            this.edsheng.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.edsheng.Location = new System.Drawing.Point(294, 83);
            this.edsheng.Name = "edsheng";
            this.edsheng.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.edsheng.Properties.DropDownRows = 10;
            this.edsheng.Size = new System.Drawing.Size(191, 25);
            this.edsheng.TabIndex = 4;
            this.edsheng.SelectedIndexChanged += new System.EventHandler(this.edsheng_SelectedIndexChanged);
            // 
            // edcity
            // 
            this.edcity.EnterMoveNextControl = true;
            this.edcity.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.edcity.Location = new System.Drawing.Point(554, 84);
            this.edcity.Name = "edcity";
            this.edcity.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.edcity.Properties.DropDownRows = 10;
            this.edcity.Size = new System.Drawing.Size(137, 25);
            this.edcity.TabIndex = 5;
            // 
            // textproject
            // 
            this.textproject.EnterMoveNextControl = true;
            this.textproject.Location = new System.Drawing.Point(294, 146);
            this.textproject.Name = "textproject";
            this.textproject.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.textproject.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.textproject.Properties.ReadOnly = true;
            this.textproject.Size = new System.Drawing.Size(191, 25);
            this.textproject.TabIndex = 10;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(233, 150);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(68, 18);
            this.label13.TabIndex = 931;
            this.label13.Text = "项      目";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(33, 149);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(68, 18);
            this.label14.TabIndex = 930;
            this.label14.Text = "申报部门";
            // 
            // textwd
            // 
            this.textwd.EnterMoveNextControl = true;
            this.textwd.Location = new System.Drawing.Point(94, 146);
            this.textwd.Name = "textwd";
            this.textwd.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.textwd.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.textwd.Properties.ReadOnly = true;
            this.textwd.Size = new System.Drawing.Size(127, 25);
            this.textwd.TabIndex = 9;
            // 
            // ck_DaiDiZhang
            // 
            this.ck_DaiDiZhang.Location = new System.Drawing.Point(36, 12);
            this.ck_DaiDiZhang.Name = "ck_DaiDiZhang";
            this.ck_DaiDiZhang.Properties.Caption = "抵账";
            this.ck_DaiDiZhang.Size = new System.Drawing.Size(47, 23);
            this.ck_DaiDiZhang.TabIndex = 7102;
            this.ck_DaiDiZhang.CheckedChanged += new System.EventHandler(this.ck_DaiDiZhang_CheckedChanged);
            // 
            // ck_DaiFuKuan
            // 
            this.ck_DaiFuKuan.Location = new System.Drawing.Point(89, 12);
            this.ck_DaiFuKuan.Name = "ck_DaiFuKuan";
            this.ck_DaiFuKuan.Properties.Caption = "付款";
            this.ck_DaiFuKuan.Size = new System.Drawing.Size(47, 23);
            this.ck_DaiFuKuan.TabIndex = 7103;
            this.ck_DaiFuKuan.CheckedChanged += new System.EventHandler(this.ck_DaiFuKuan_CheckedChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(492, 148);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(53, 18);
            this.label15.TabIndex = 7104;
            this.label15.Text = "批次号";
            // 
            // txtBatch
            // 
            this.txtBatch.EnterMoveNextControl = true;
            this.txtBatch.Location = new System.Drawing.Point(555, 146);
            this.txtBatch.Name = "txtBatch";
            this.txtBatch.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.txtBatch.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtBatch.Properties.ReadOnly = true;
            this.txtBatch.Size = new System.Drawing.Size(136, 25);
            this.txtBatch.TabIndex = 7105;
            // 
            // edremark
            // 
            this.edremark.Location = new System.Drawing.Point(294, 177);
            this.edremark.Name = "edremark";
            this.edremark.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.edremark.Properties.Appearance.Options.UseForeColor = true;
            this.edremark.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.edremark.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.edremark.Properties.ReadOnly = true;
            this.edremark.Size = new System.Drawing.Size(397, 47);
            this.edremark.TabIndex = 7106;
            this.edremark.TabStop = false;
            // 
            // ExpenseBankInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(748, 340);
            this.Controls.Add(this.edremark);
            this.Controls.Add(this.txtBatch);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.ck_DaiDiZhang);
            this.Controls.Add(this.ck_DaiFuKuan);
            this.Controls.Add(this.textwd);
            this.Controls.Add(this.textproject);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.edsheng);
            this.Controls.Add(this.edcity);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.edbankchild);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.edouttype);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.edbilldate);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.edaccin);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.edaccout);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.edopertype);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.edbankname);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.edbankcode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.edbankman);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "ExpenseBankInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "银行信息平台";
            this.Load += new System.EventHandler(this.ExpenseBankInfo_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.w_bank_add_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.edbankman.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edbankcode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edbankname.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edopertype.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edaccout.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edaccin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edbilldate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edbilldate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edouttype.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edbankchild.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edsheng.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edcity.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textproject.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textwd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ck_DaiDiZhang.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ck_DaiFuKuan.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBatch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edremark.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit edbankman;
        private DevExpress.XtraEditors.TextEdit edbankcode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.ComboBoxEdit edbankname;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.ComboBoxEdit edopertype;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.TextEdit edaccout;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.TextEdit edaccin;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private DevExpress.XtraEditors.DateEdit edbilldate;
        private DevExpress.XtraEditors.ComboBoxEdit edouttype;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraEditors.TextEdit edbankchild;
        private System.Windows.Forms.Label label12;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraEditors.ImageComboBoxEdit edsheng;
        private DevExpress.XtraEditors.ImageComboBoxEdit edcity;
        private DevExpress.XtraEditors.TextEdit textproject;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private DevExpress.XtraEditors.TextEdit textwd;
        private DevExpress.XtraEditors.CheckEdit ck_DaiDiZhang;
        private DevExpress.XtraEditors.CheckEdit ck_DaiFuKuan;
        private System.Windows.Forms.Label label15;
        private DevExpress.XtraEditors.TextEdit txtBatch;
        private DevExpress.XtraEditors.MemoEdit edremark;

    }
}