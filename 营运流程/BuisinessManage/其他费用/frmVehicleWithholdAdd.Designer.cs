namespace ZQTMS.UI
{
    partial class frmVehicleWithholdAdd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmVehicleWithholdAdd));
            this.txtMoney = new DevExpress.XtraEditors.TextEdit();
            this.label20 = new System.Windows.Forms.Label();
            this.SendCarNO = new DevExpress.XtraEditors.TextEdit();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.ExpArriveDate = new ZQTMS.Common.DateEditEx();
            this.txtBank = new DevExpress.XtraEditors.ComboBoxEdit();
            this.oilCardCity = new DevExpress.XtraEditors.ComboBoxEdit();
            this.oilCardProvince = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label144 = new System.Windows.Forms.Label();
            this.label146 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.myGridControl3 = new ZQTMS.Lib.MyGridControl();
            this.myGridView3 = new ZQTMS.Lib.MyGridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cmbUse = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.txtAccountNo = new DevExpress.XtraEditors.TextEdit();
            this.txtAccountName = new DevExpress.XtraEditors.TextEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMan = new DevExpress.XtraEditors.TextEdit();
            this.label7 = new System.Windows.Forms.Label();
            this.txtCertificate = new DevExpress.XtraEditors.TextEdit();
            this.dOperDate = new DevExpress.XtraEditors.DateEdit();
            this.dBusinessDate = new DevExpress.XtraEditors.DateEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.txtRemark = new DevExpress.XtraEditors.MemoEdit();
            this.cbbType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.SendDriverPhone = new DevExpress.XtraEditors.TextEdit();
            this.SendDriver = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.txtMoney.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SendCarNO.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ExpArriveDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExpArriveDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBank.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.oilCardCity.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.oilCardProvince.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myGridControl3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myGridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUse.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccountNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccountName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMan.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCertificate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dOperDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dOperDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dBusinessDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dBusinessDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SendDriverPhone.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SendDriver.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtMoney
            // 
            this.txtMoney.Location = new System.Drawing.Point(407, 41);
            this.txtMoney.Name = "txtMoney";
            this.txtMoney.Size = new System.Drawing.Size(158, 21);
            this.txtMoney.TabIndex = 24;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(61, 291);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(43, 14);
            this.label20.TabIndex = 20;
            this.label20.Text = "摘要：";
            // 
            // SendCarNO
            // 
            this.SendCarNO.Location = new System.Drawing.Point(124, 12);
            this.SendCarNO.Name = "SendCarNO";
            this.SendCarNO.Size = new System.Drawing.Size(158, 21);
            this.SendCarNO.TabIndex = 19;
            this.SendCarNO.EditValueChanged += new System.EventHandler(this.SendCarNO_EditValueChanged);
            this.SendCarNO.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.SendCarNO_EditValueChanging);
            this.SendCarNO.Leave += new System.EventHandler(this.SendCarNO_Leave);
            this.SendCarNO.Enter += new System.EventHandler(this.SendCarNO_Enter);
            this.SendCarNO.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendCarNO_KeyDown);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(333, 44);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(67, 14);
            this.label16.TabIndex = 15;
            this.label16.Text = "代扣金额：";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(344, 98);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(55, 14);
            this.label15.TabIndex = 14;
            this.label15.Text = "凭证号：";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(50, 42);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(67, 14);
            this.label11.TabIndex = 10;
            this.label11.Text = "司机姓名：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(51, 71);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(67, 14);
            this.label10.TabIndex = 9;
            this.label10.Text = "司机电话：";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.ExpArriveDate);
            this.panelControl1.Controls.Add(this.txtBank);
            this.panelControl1.Controls.Add(this.oilCardCity);
            this.panelControl1.Controls.Add(this.oilCardProvince);
            this.panelControl1.Controls.Add(this.label144);
            this.panelControl1.Controls.Add(this.label146);
            this.panelControl1.Controls.Add(this.label9);
            this.panelControl1.Controls.Add(this.radioButton2);
            this.panelControl1.Controls.Add(this.radioButton1);
            this.panelControl1.Controls.Add(this.myGridControl3);
            this.panelControl1.Controls.Add(this.cmbUse);
            this.panelControl1.Controls.Add(this.label8);
            this.panelControl1.Controls.Add(this.txtAccountNo);
            this.panelControl1.Controls.Add(this.txtAccountName);
            this.panelControl1.Controls.Add(this.label6);
            this.panelControl1.Controls.Add(this.label5);
            this.panelControl1.Controls.Add(this.label4);
            this.panelControl1.Controls.Add(this.txtMan);
            this.panelControl1.Controls.Add(this.label7);
            this.panelControl1.Controls.Add(this.txtCertificate);
            this.panelControl1.Controls.Add(this.dOperDate);
            this.panelControl1.Controls.Add(this.dBusinessDate);
            this.panelControl1.Controls.Add(this.label3);
            this.panelControl1.Controls.Add(this.label21);
            this.panelControl1.Controls.Add(this.btnClose);
            this.panelControl1.Controls.Add(this.btnSave);
            this.panelControl1.Controls.Add(this.txtRemark);
            this.panelControl1.Controls.Add(this.cbbType);
            this.panelControl1.Controls.Add(this.SendDriverPhone);
            this.panelControl1.Controls.Add(this.SendDriver);
            this.panelControl1.Controls.Add(this.txtMoney);
            this.panelControl1.Controls.Add(this.label20);
            this.panelControl1.Controls.Add(this.SendCarNO);
            this.panelControl1.Controls.Add(this.label16);
            this.panelControl1.Controls.Add(this.label15);
            this.panelControl1.Controls.Add(this.label11);
            this.panelControl1.Controls.Add(this.label10);
            this.panelControl1.Controls.Add(this.label2);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(664, 518);
            this.panelControl1.TabIndex = 1;
            // 
            // ExpArriveDate
            // 
            this.ExpArriveDate.DateMode = ZQTMS.Common.DateEditEx.DateResultModeEnum.FirstDayOfMonth;
            this.ExpArriveDate.EditValue = null;
            this.ExpArriveDate.Location = new System.Drawing.Point(407, 122);
            this.ExpArriveDate.Name = "ExpArriveDate";
            this.ExpArriveDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ExpArriveDate.Properties.DisplayFormat.FormatString = "yyyy-MM";
            this.ExpArriveDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.ExpArriveDate.Properties.Mask.EditMask = "yyyy-MM";
            this.ExpArriveDate.Properties.ShowToday = false;
            this.ExpArriveDate.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;
            this.ExpArriveDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.ExpArriveDate.Size = new System.Drawing.Size(158, 21);
            this.ExpArriveDate.TabIndex = 245;
            // 
            // txtBank
            // 
            this.txtBank.EnterMoveNextControl = true;
            this.txtBank.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.txtBank.Location = new System.Drawing.Point(127, 179);
            this.txtBank.Name = "txtBank";
            this.txtBank.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtBank.Properties.Items.AddRange(new object[] {
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
            "平安银行"});
            this.txtBank.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.txtBank.Size = new System.Drawing.Size(155, 21);
            this.txtBank.TabIndex = 244;
            // 
            // oilCardCity
            // 
            this.oilCardCity.EnterMoveNextControl = true;
            this.oilCardCity.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.oilCardCity.Location = new System.Drawing.Point(407, 217);
            this.oilCardCity.Name = "oilCardCity";
            this.oilCardCity.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.oilCardCity.Properties.Items.AddRange(new object[] {
            ""});
            this.oilCardCity.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.oilCardCity.Size = new System.Drawing.Size(158, 21);
            this.oilCardCity.TabIndex = 243;
            // 
            // oilCardProvince
            // 
            this.oilCardProvince.EnterMoveNextControl = true;
            this.oilCardProvince.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.oilCardProvince.Location = new System.Drawing.Point(123, 216);
            this.oilCardProvince.Name = "oilCardProvince";
            this.oilCardProvince.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.oilCardProvince.Properties.Items.AddRange(new object[] {
            ""});
            this.oilCardProvince.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.oilCardProvince.Size = new System.Drawing.Size(158, 21);
            this.oilCardProvince.TabIndex = 242;
            this.oilCardProvince.SelectedIndexChanged += new System.EventHandler(this.oilCardProvince_SelectedIndexChanged);
            // 
            // label144
            // 
            this.label144.AutoSize = true;
            this.label144.Location = new System.Drawing.Point(324, 219);
            this.label144.Name = "label144";
            this.label144.Size = new System.Drawing.Size(79, 14);
            this.label144.TabIndex = 241;
            this.label144.Text = "开户行城市：";
            // 
            // label146
            // 
            this.label146.AutoSize = true;
            this.label146.Location = new System.Drawing.Point(41, 219);
            this.label146.Name = "label146";
            this.label146.Size = new System.Drawing.Size(79, 14);
            this.label146.TabIndex = 240;
            this.label146.Text = "开户行省份：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(344, 125);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 14);
            this.label9.TabIndex = 238;
            this.label9.Text = "月   份：";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(220, 156);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(49, 18);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "返款";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(124, 156);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(49, 18);
            this.radioButton1.TabIndex = 1;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "抵账";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // myGridControl3
            // 
            this.myGridControl3.Location = new System.Drawing.Point(-103, 277);
            this.myGridControl3.MainView = this.myGridView3;
            this.myGridControl3.Name = "myGridControl3";
            this.myGridControl3.Size = new System.Drawing.Size(402, 241);
            this.myGridControl3.TabIndex = 219;
            this.myGridControl3.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.myGridView3});
            this.myGridControl3.Visible = false;
            this.myGridControl3.DragOver += new System.Windows.Forms.DragEventHandler(this.myGridControl3_DragOver);
            this.myGridControl3.Leave += new System.EventHandler(this.myGridControl3_Leave);
            this.myGridControl3.KeyDown += new System.Windows.Forms.KeyEventHandler(this.myGridControl3_KeyDown);
            // 
            // myGridView3
            // 
            this.myGridView3.ColumnPanelRowHeight = 30;
            this.myGridView3.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1});
            this.myGridView3.GridControl = this.myGridControl3;
            this.myGridView3.GridViewRemark = "送货车辆档案信息";
            this.myGridView3.Guid = new System.Guid("b6889ad3-d9d9-4e86-9348-60a8d6478f32");
            this.myGridView3.HiddenFiledDic = ((System.Collections.Generic.Dictionary<string, object>)(resources.GetObject("myGridView3.HiddenFiledDic")));
            this.myGridView3.MenuName = "";
            this.myGridView3.Name = "myGridView3";
            this.myGridView3.OptionsBehavior.Editable = false;
            this.myGridView3.OptionsView.ColumnAutoWidth = false;
            this.myGridView3.OptionsView.ShowAutoFilterRow = true;
            this.myGridView3.OptionsView.ShowGroupPanel = false;
            this.myGridView3.WebControlBindFindName = "";
            this.myGridView3.DoubleClick += new System.EventHandler(this.myGridView3_DoubleClick);
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
            // cmbUse
            // 
            this.cmbUse.Location = new System.Drawing.Point(124, 122);
            this.cmbUse.Name = "cmbUse";
            this.cmbUse.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbUse.Properties.Items.AddRange(new object[] {
            "长途",
            "短途",
            "--请选择--"});
            this.cmbUse.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbUse.Size = new System.Drawing.Size(158, 21);
            this.cmbUse.TabIndex = 237;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(49, 125);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 14);
            this.label8.TabIndex = 236;
            this.label8.Text = "车辆用途：";
            // 
            // txtAccountNo
            // 
            this.txtAccountNo.Location = new System.Drawing.Point(407, 180);
            this.txtAccountNo.Name = "txtAccountNo";
            this.txtAccountNo.Size = new System.Drawing.Size(158, 21);
            this.txtAccountNo.TabIndex = 234;
            // 
            // txtAccountName
            // 
            this.txtAccountName.Location = new System.Drawing.Point(407, 153);
            this.txtAccountName.Name = "txtAccountName";
            this.txtAccountName.Size = new System.Drawing.Size(158, 21);
            this.txtAccountName.TabIndex = 233;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(41, 185);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 14);
            this.label6.TabIndex = 232;
            this.label6.Text = "返款开户行：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(333, 183);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 14);
            this.label5.TabIndex = 231;
            this.label5.Text = "返款账号：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(336, 156);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 14);
            this.label4.TabIndex = 230;
            this.label4.Text = "返款户名：";
            // 
            // txtMan
            // 
            this.txtMan.Location = new System.Drawing.Point(123, 95);
            this.txtMan.Name = "txtMan";
            this.txtMan.Size = new System.Drawing.Size(158, 21);
            this.txtMan.TabIndex = 229;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(61, 98);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 14);
            this.label7.TabIndex = 228;
            this.label7.Text = "登记人：";
            // 
            // txtCertificate
            // 
            this.txtCertificate.Location = new System.Drawing.Point(407, 95);
            this.txtCertificate.Name = "txtCertificate";
            this.txtCertificate.Size = new System.Drawing.Size(158, 21);
            this.txtCertificate.TabIndex = 221;
            // 
            // dOperDate
            // 
            this.dOperDate.EditValue = new System.DateTime(2006, 2, 18, 0, 0, 0, 0);
            this.dOperDate.EnterMoveNextControl = true;
            this.dOperDate.Location = new System.Drawing.Point(407, 68);
            this.dOperDate.Name = "dOperDate";
            this.dOperDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dOperDate.Properties.DisplayFormat.FormatString = "yyyy-M-d HH:mm:ss";
            this.dOperDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dOperDate.Properties.EditFormat.FormatString = "yyyy-M-d HH:mm:ss";
            this.dOperDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dOperDate.Properties.Mask.EditMask = "yyyy-M-d HH:mm:ss";
            this.dOperDate.Properties.ReadOnly = true;
            this.dOperDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dOperDate.Size = new System.Drawing.Size(158, 21);
            this.dOperDate.TabIndex = 220;
            // 
            // dBusinessDate
            // 
            this.dBusinessDate.EditValue = new System.DateTime(2006, 1, 1, 0, 0, 0, 0);
            this.dBusinessDate.EnterMoveNextControl = true;
            this.dBusinessDate.Location = new System.Drawing.Point(124, 252);
            this.dBusinessDate.Name = "dBusinessDate";
            this.dBusinessDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dBusinessDate.Properties.DisplayFormat.FormatString = "yyyy-M-d HH:mm:ss";
            this.dBusinessDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dBusinessDate.Properties.EditFormat.FormatString = "yyyy-M-d HH:mm:ss";
            this.dBusinessDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dBusinessDate.Properties.Mask.EditMask = "yyyy-M-d HH:mm:ss";
            this.dBusinessDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dBusinessDate.Size = new System.Drawing.Size(158, 21);
            this.dBusinessDate.TabIndex = 221;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(335, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 14);
            this.label3.TabIndex = 47;
            this.label3.Text = "登记日期：";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(38, 255);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(91, 14);
            this.label21.TabIndex = 48;
            this.label21.Text = "业务发生日期：";
            // 
            // btnClose
            // 
            this.btnClose.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Appearance.Options.UseFont = true;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(467, 403);
            this.btnClose.Name = "btnClose";
            this.btnClose.ShowToolTips = false;
            this.btnClose.Size = new System.Drawing.Size(58, 23);
            this.btnClose.TabIndex = 46;
            this.btnClose.Text = "关闭";
            this.btnClose.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Hand;
            this.btnClose.ToolTipTitle = "帮助";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Appearance.Options.UseFont = true;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(344, 403);
            this.btnSave.Name = "btnSave";
            this.btnSave.ShowToolTips = false;
            this.btnSave.Size = new System.Drawing.Size(58, 23);
            this.btnSave.TabIndex = 45;
            this.btnSave.Text = "保存";
            this.btnSave.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Hand;
            this.btnSave.ToolTipTitle = "帮助";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(124, 289);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(441, 109);
            this.txtRemark.TabIndex = 42;
            // 
            // cbbType
            // 
            this.cbbType.Location = new System.Drawing.Point(407, 14);
            this.cbbType.Name = "cbbType";
            this.cbbType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbbType.Properties.Items.AddRange(new object[] {
            "车贷",
            "挂靠费",
            "社保费",
            "保险费",
            "油料费",
            "油卡",
            "GPS服务费",
            "车身广告费",
            "年审费",
            "罚款",
            "代办证件费",
            "ETC费",
            "修理费",
            "其他代扣费",
            "代扣罚款",
            "奖励支出",
            "轮胎费",
            "管理费"});
            this.cbbType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbbType.Size = new System.Drawing.Size(158, 21);
            this.cbbType.TabIndex = 34;
            // 
            // SendDriverPhone
            // 
            this.SendDriverPhone.Location = new System.Drawing.Point(124, 68);
            this.SendDriverPhone.Name = "SendDriverPhone";
            this.SendDriverPhone.Size = new System.Drawing.Size(158, 21);
            this.SendDriverPhone.TabIndex = 32;
            // 
            // SendDriver
            // 
            this.SendDriver.Location = new System.Drawing.Point(124, 41);
            this.SendDriver.Name = "SendDriver";
            this.SendDriver.Size = new System.Drawing.Size(158, 21);
            this.SendDriver.TabIndex = 31;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(333, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "代扣类型：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "车牌号：";
            // 
            // frmVehicleWithholdAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 518);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmVehicleWithholdAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "新增车辆代扣款";
            this.Load += new System.EventHandler(this.frmVehicleWithholdAdd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtMoney.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SendCarNO.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ExpArriveDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExpArriveDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBank.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.oilCardCity.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.oilCardProvince.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myGridControl3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myGridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUse.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccountNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccountName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMan.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCertificate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dOperDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dOperDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dBusinessDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dBusinessDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SendDriverPhone.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SendDriver.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtMoney;
        private System.Windows.Forms.Label label20;
        private DevExpress.XtraEditors.TextEdit SendCarNO;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.MemoEdit txtRemark;
        private DevExpress.XtraEditors.TextEdit SendDriverPhone;
        private DevExpress.XtraEditors.TextEdit SendDriver;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label21;
        private ZQTMS.Lib.MyGridControl myGridControl3;
        private ZQTMS.Lib.MyGridView myGridView3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.ComboBoxEdit cbbType;
        public DevExpress.XtraEditors.DateEdit dOperDate;
        public DevExpress.XtraEditors.DateEdit dBusinessDate;
        private DevExpress.XtraEditors.TextEdit txtCertificate;
        private DevExpress.XtraEditors.TextEdit txtMan;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.TextEdit txtAccountNo;
        private DevExpress.XtraEditors.TextEdit txtAccountName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.ComboBoxEdit cmbUse;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Label label9;
        private DevExpress.XtraEditors.ComboBoxEdit oilCardCity;
        private DevExpress.XtraEditors.ComboBoxEdit oilCardProvince;
        private System.Windows.Forms.Label label144;
        private System.Windows.Forms.Label label146;
        private DevExpress.XtraEditors.ComboBoxEdit txtBank;
        private ZQTMS.Common.DateEditEx ExpArriveDate;
    }
}