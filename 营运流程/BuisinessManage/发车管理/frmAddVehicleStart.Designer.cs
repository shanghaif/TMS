namespace ZQTMS.UI
{
    partial class frmAddVehicleStart
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddVehicleStart));
            this.label9 = new System.Windows.Forms.Label();
            this.Type = new DevExpress.XtraEditors.ComboBoxEdit();
            this.BatchNO = new DevExpress.XtraEditors.TextEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.VehicleNO = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Driver = new DevExpress.XtraEditors.TextEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.StartTime = new DevExpress.XtraEditors.DateEdit();
            this.DriverPhone = new DevExpress.XtraEditors.TextEdit();
            this.label13 = new System.Windows.Forms.Label();
            this.StartSite = new DevExpress.XtraEditors.ComboBoxEdit();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.EstArriveTime = new DevExpress.XtraEditors.DateEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.ArriveSite = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.ArriveWeb = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label7 = new System.Windows.Forms.Label();
            this.StartWeb = new DevExpress.XtraEditors.ComboBoxEdit();
            this.LastStartTime = new DevExpress.XtraEditors.DateEdit();
            this.label11 = new System.Windows.Forms.Label();
            this.myGridControl1 = new ZQTMS.Lib.MyGridControl();
            this.myGridView1 = new ZQTMS.Lib.MyGridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.Type.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BatchNO.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VehicleNO.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Driver.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartTime.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DriverPhone.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartSite.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EstArriveTime.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EstArriveTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ArriveSite.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ArriveWeb.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartWeb.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LastStartTime.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LastStartTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myGridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(66, 39);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 14);
            this.label9.TabIndex = 80;
            this.label9.Text = "类型：";
            // 
            // Type
            // 
            this.Type.EditValue = "短驳";
            this.Type.EnterMoveNextControl = true;
            this.Type.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.Type.Location = new System.Drawing.Point(113, 36);
            this.Type.Name = "Type";
            this.Type.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.Type.Properties.Items.AddRange(new object[] {
            "短驳",
            "配载",
            "转二级"});
            this.Type.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.Type.Size = new System.Drawing.Size(148, 21);
            this.Type.TabIndex = 1;
            this.Type.SelectedIndexChanged += new System.EventHandler(this.Type_SelectedIndexChanged);
            // 
            // BatchNO
            // 
            this.BatchNO.EnterMoveNextControl = true;
            this.BatchNO.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.BatchNO.Location = new System.Drawing.Point(363, 36);
            this.BatchNO.Name = "BatchNO";
            this.BatchNO.Size = new System.Drawing.Size(148, 21);
            this.BatchNO.TabIndex = 2;
            this.BatchNO.MouseClick += new System.Windows.Forms.MouseEventHandler(this.BatchNO_MouseClick);
            this.BatchNO.Leave += new System.EventHandler(this.BatchNO_Leave);
            this.BatchNO.Enter += new System.EventHandler(this.BatchNO_Enter);
            this.BatchNO.Click += new System.EventHandler(this.BatchNO_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(305, 39);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 14);
            this.label8.TabIndex = 78;
            this.label8.Text = "批次号:";
            // 
            // VehicleNO
            // 
            this.VehicleNO.EnterMoveNextControl = true;
            this.VehicleNO.Location = new System.Drawing.Point(113, 74);
            this.VehicleNO.Name = "VehicleNO";
            this.VehicleNO.Size = new System.Drawing.Size(149, 21);
            this.VehicleNO.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 14);
            this.label2.TabIndex = 62;
            this.label2.Text = "承运车号：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(293, 116);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 14);
            this.label4.TabIndex = 64;
            this.label4.Text = "联系电话：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(41, 197);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 14);
            this.label6.TabIndex = 65;
            this.label6.Text = "发车站点：";
            // 
            // Driver
            // 
            this.Driver.EnterMoveNextControl = true;
            this.Driver.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.Driver.Location = new System.Drawing.Point(114, 113);
            this.Driver.Name = "Driver";
            this.Driver.Size = new System.Drawing.Size(148, 21);
            this.Driver.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(44, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 14);
            this.label3.TabIndex = 63;
            this.label3.Text = "驾  驶 员：";
            // 
            // StartTime
            // 
            this.StartTime.EditValue = new System.DateTime(2006, 8, 4, 0, 0, 0, 0);
            this.StartTime.EnterMoveNextControl = true;
            this.StartTime.Location = new System.Drawing.Point(363, 74);
            this.StartTime.Name = "StartTime";
            this.StartTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.StartTime.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.StartTime.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.StartTime.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.StartTime.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.StartTime.Properties.Mask.EditMask = "yyyy-MM-dd HH:mm";
            this.StartTime.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.StartTime.Size = new System.Drawing.Size(148, 21);
            this.StartTime.TabIndex = 4;
            // 
            // DriverPhone
            // 
            this.DriverPhone.EnterMoveNextControl = true;
            this.DriverPhone.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.DriverPhone.Location = new System.Drawing.Point(363, 113);
            this.DriverPhone.Name = "DriverPhone";
            this.DriverPhone.Size = new System.Drawing.Size(148, 21);
            this.DriverPhone.TabIndex = 6;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(293, 77);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(67, 14);
            this.label13.TabIndex = 68;
            this.label13.Text = "发车时间：";
            // 
            // StartSite
            // 
            this.StartSite.EnterMoveNextControl = true;
            this.StartSite.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.StartSite.Location = new System.Drawing.Point(114, 194);
            this.StartSite.Name = "StartSite";
            this.StartSite.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.StartSite.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.StartSite.Size = new System.Drawing.Size(148, 21);
            this.StartSite.TabIndex = 9;
            this.StartSite.SelectedIndexChanged += new System.EventHandler(this.StartSite_SelectedIndexChanged);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleButton2.Appearance.Options.UseFont = true;
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(295, 312);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(75, 25);
            this.simpleButton2.TabIndex = 14;
            this.simpleButton2.Text = "取消";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(186, 312);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 25);
            this.simpleButton1.TabIndex = 13;
            this.simpleButton1.Text = "完成";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // EstArriveTime
            // 
            this.EstArriveTime.EditValue = new System.DateTime(2006, 8, 4, 0, 0, 0, 0);
            this.EstArriveTime.EnterMoveNextControl = true;
            this.EstArriveTime.Location = new System.Drawing.Point(363, 154);
            this.EstArriveTime.Name = "EstArriveTime";
            this.EstArriveTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.EstArriveTime.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.EstArriveTime.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.EstArriveTime.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.EstArriveTime.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.EstArriveTime.Properties.Mask.EditMask = "yyyy-MM-dd HH:mm";
            this.EstArriveTime.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.EstArriveTime.Size = new System.Drawing.Size(148, 21);
            this.EstArriveTime.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(293, 157);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 14);
            this.label5.TabIndex = 81;
            this.label5.Text = "预到时间：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(41, 236);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(67, 14);
            this.label10.TabIndex = 84;
            this.label10.Text = "到车站点：";
            // 
            // ArriveSite
            // 
            this.ArriveSite.EnterMoveNextControl = true;
            this.ArriveSite.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.ArriveSite.Location = new System.Drawing.Point(114, 233);
            this.ArriveSite.Name = "ArriveSite";
            this.ArriveSite.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ArriveSite.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.ArriveSite.Size = new System.Drawing.Size(148, 21);
            this.ArriveSite.TabIndex = 11;
            this.ArriveSite.SelectedIndexChanged += new System.EventHandler(this.ArriveSite_SelectedIndexChanged);
            this.ArriveSite.TextChanged += new System.EventHandler(this.ArriveSite_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(292, 236);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 14);
            this.label1.TabIndex = 88;
            this.label1.Text = "到车网点：";
            // 
            // ArriveWeb
            // 
            this.ArriveWeb.EnterMoveNextControl = true;
            this.ArriveWeb.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.ArriveWeb.Location = new System.Drawing.Point(363, 233);
            this.ArriveWeb.Name = "ArriveWeb";
            this.ArriveWeb.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ArriveWeb.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.ArriveWeb.Size = new System.Drawing.Size(148, 21);
            this.ArriveWeb.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(292, 197);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 14);
            this.label7.TabIndex = 86;
            this.label7.Text = "发车网点：";
            // 
            // StartWeb
            // 
            this.StartWeb.EnterMoveNextControl = true;
            this.StartWeb.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.StartWeb.Location = new System.Drawing.Point(363, 194);
            this.StartWeb.Name = "StartWeb";
            this.StartWeb.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.StartWeb.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.StartWeb.Size = new System.Drawing.Size(148, 21);
            this.StartWeb.TabIndex = 10;
            // 
            // LastStartTime
            // 
            this.LastStartTime.EditValue = new System.DateTime(2006, 8, 4, 0, 0, 0, 0);
            this.LastStartTime.EnterMoveNextControl = true;
            this.LastStartTime.Location = new System.Drawing.Point(114, 154);
            this.LastStartTime.Name = "LastStartTime";
            this.LastStartTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LastStartTime.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.LastStartTime.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.LastStartTime.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.LastStartTime.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.LastStartTime.Properties.Mask.EditMask = "yyyy-MM-dd HH:mm";
            this.LastStartTime.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.LastStartTime.Size = new System.Drawing.Size(148, 21);
            this.LastStartTime.TabIndex = 7;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(20, 157);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(91, 14);
            this.label11.TabIndex = 89;
            this.label11.Text = "最晚发车时间：";
            // 
            // myGridControl1
            // 
            this.myGridControl1.Location = new System.Drawing.Point(113, 59);
            this.myGridControl1.MainView = this.myGridView1;
            this.myGridControl1.Name = "myGridControl1";
            this.myGridControl1.Size = new System.Drawing.Size(398, 224);
            this.myGridControl1.TabIndex = 32;
            this.myGridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.myGridView1});
            this.myGridControl1.Visible = false;
            this.myGridControl1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.myGridControl1_MouseDoubleClick);
            // 
            // myGridView1
            // 
            this.myGridView1.ColumnPanelRowHeight = 30;
            this.myGridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8,
            this.gridColumn9,
            this.gridColumn10,
            this.gridColumn11});
            this.myGridView1.GridControl = this.myGridControl1;
            this.myGridView1.GridViewRemark = "在途车辆";
            this.myGridView1.Guid = new System.Guid("9b83c54c-9624-4921-ad24-ff595a29b26e");
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
            this.gridColumn1.Caption = "类型";
            this.gridColumn1.FieldName = "Type";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "批次";
            this.gridColumn2.FieldName = "BatchNO";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 2;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "时间";
            this.gridColumn3.FieldName = "SDT";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 3;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "发车站点";
            this.gridColumn4.FieldName = "SSite";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 4;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "发车网点";
            this.gridColumn5.FieldName = "SWeb";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 5;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "车号";
            this.gridColumn6.FieldName = "CarNo";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 6;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "司机";
            this.gridColumn7.FieldName = "Driver";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 7;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "联系电话";
            this.gridColumn8.FieldName = "DriverPhone";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 8;
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "到达站点";
            this.gridColumn9.FieldName = "ESite";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 9;
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption = "到达网点";
            this.gridColumn10.FieldName = "EWeb";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 10;
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption = "序号";
            this.gridColumn11.FieldName = "rowid";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.OptionsColumn.AllowEdit = false;
            this.gridColumn11.OptionsColumn.AllowFocus = false;
            this.gridColumn11.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 1;
            // 
            // frmAddVehicleStart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 378);
            this.Controls.Add(this.myGridControl1);
            this.Controls.Add(this.LastStartTime);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ArriveWeb);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.StartWeb);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.ArriveSite);
            this.Controls.Add(this.EstArriveTime);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.Type);
            this.Controls.Add(this.BatchNO);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.VehicleNO);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.Driver);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.StartTime);
            this.Controls.Add(this.DriverPhone);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.StartSite);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmAddVehicleStart";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "新增发车";
            this.Load += new System.EventHandler(this.frmAddVehicleStart_Load);
            this.Click += new System.EventHandler(this.frmAddVehicleStart_Click);
            ((System.ComponentModel.ISupportInitialize)(this.Type.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BatchNO.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VehicleNO.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Driver.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartTime.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DriverPhone.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartSite.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EstArriveTime.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EstArriveTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ArriveSite.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ArriveWeb.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartWeb.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LastStartTime.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LastStartTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myGridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private System.Windows.Forms.Label label9;
        private DevExpress.XtraEditors.ComboBoxEdit Type;
        private DevExpress.XtraEditors.TextEdit BatchNO;
        private System.Windows.Forms.Label label8;
        private DevExpress.XtraEditors.TextEdit VehicleNO;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.TextEdit Driver;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.DateEdit StartTime;
        private DevExpress.XtraEditors.TextEdit DriverPhone;
        private System.Windows.Forms.Label label13;
        private DevExpress.XtraEditors.ComboBoxEdit StartSite;
        private DevExpress.XtraEditors.DateEdit EstArriveTime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label10;
        private DevExpress.XtraEditors.ComboBoxEdit ArriveSite;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.ComboBoxEdit ArriveWeb;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.ComboBoxEdit StartWeb;
        private DevExpress.XtraEditors.DateEdit LastStartTime;
        private System.Windows.Forms.Label label11;
        private ZQTMS.Lib.MyGridControl myGridControl1;
        private ZQTMS.Lib.MyGridView myGridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
    }
}