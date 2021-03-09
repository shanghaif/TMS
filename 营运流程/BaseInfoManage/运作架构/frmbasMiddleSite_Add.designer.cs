namespace ZQTMS.UI
{
    partial class frmbasMiddleSite_Add
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition1 = new DevExpress.XtraGrid.StyleFormatCondition();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MiddleStatus = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl17 = new DevExpress.XtraEditors.LabelControl();
            this.btnConcel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSubmit = new DevExpress.XtraEditors.SimpleButton();
            this.label3 = new System.Windows.Forms.Label();
            this.myGridControl1 = new ZQTMS.Lib.MyGridControl();
            this.myGridView1 = new ZQTMS.Lib.MyGridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnCreate = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.WebName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.Type = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.MiddleCity = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.MiddleStreet = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.MiddleArea = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.Remark = new DevExpress.XtraEditors.MemoEdit();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.btnDuibi = new DevExpress.XtraEditors.SimpleButton();
            this.MiddleProvince = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.SiteName = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.Company = new DevExpress.XtraEditors.LabelControl();
            this.CompanyID = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.MiddleStatus.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myGridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WebName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Type.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MiddleCity.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MiddleStreet.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MiddleArea.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Remark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MiddleProvince.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SiteName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CompanyID.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gridColumn6
            // 
            this.gridColumn6.FieldName = "iscancel";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.AllowEdit = false;
            this.gridColumn6.OptionsColumn.AllowFocus = false;
            // 
            // MiddleStatus
            // 
            this.MiddleStatus.EditValue = true;
            this.MiddleStatus.Location = new System.Drawing.Point(674, 119);
            this.MiddleStatus.Name = "MiddleStatus";
            this.MiddleStatus.Properties.Caption = "是否启用";
            this.MiddleStatus.Size = new System.Drawing.Size(75, 19);
            this.MiddleStatus.TabIndex = 16;
            // 
            // labelControl17
            // 
            this.labelControl17.Location = new System.Drawing.Point(29, 34);
            this.labelControl17.Name = "labelControl17";
            this.labelControl17.Size = new System.Drawing.Size(48, 14);
            this.labelControl17.TabIndex = 33;
            this.labelControl17.Text = "隶属站点";
            // 
            // btnConcel
            // 
            this.btnConcel.Location = new System.Drawing.Point(504, 426);
            this.btnConcel.Name = "btnConcel";
            this.btnConcel.Size = new System.Drawing.Size(75, 23);
            this.btnConcel.TabIndex = 58;
            this.btnConcel.Text = "取   消";
            this.btnConcel.Click += new System.EventHandler(this.btnConcel_Click);
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(398, 426);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 57;
            this.btnSubmit.Text = "保  存";
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(46, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 14);
            this.label3.TabIndex = 309;
            this.label3.Text = "省份";
            // 
            // myGridControl1
            // 
            gridLevelNode1.RelationName = "Level1";
            this.myGridControl1.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.myGridControl1.Location = new System.Drawing.Point(83, 165);
            this.myGridControl1.MainView = this.myGridView1;
            this.myGridControl1.Name = "myGridControl1";
            this.myGridControl1.Size = new System.Drawing.Size(415, 244);
            this.myGridControl1.TabIndex = 314;
            this.myGridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.myGridView1});
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
            this.gridColumn7});
            styleFormatCondition1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            styleFormatCondition1.Appearance.Options.UseBackColor = true;
            styleFormatCondition1.ApplyToRow = true;
            styleFormatCondition1.Column = this.gridColumn6;
            styleFormatCondition1.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
            styleFormatCondition1.Value1 = 1;
            this.myGridView1.FormatConditions.AddRange(new DevExpress.XtraGrid.StyleFormatCondition[] {
            styleFormatCondition1});
            this.myGridView1.GridControl = this.myGridControl1;
            this.myGridView1.Guid = new System.Guid("9b33f2b1-293c-41af-ba1e-2670092b8bfc");
            this.myGridView1.Name = "myGridView1";
            this.myGridView1.OptionsBehavior.Editable = false;
            this.myGridView1.OptionsView.ColumnAutoWidth = false;
            this.myGridView1.OptionsView.ShowAutoFilterRow = true;
            this.myGridView1.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "省份";
            this.gridColumn1.FieldName = "MiddleProvince";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.OptionsColumn.AllowFocus = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "城市";
            this.gridColumn2.FieldName = "MiddleCity";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.OptionsColumn.AllowFocus = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 2;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "区县";
            this.gridColumn3.FieldName = "MiddleArea";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.OptionsColumn.AllowFocus = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 3;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "乡镇街道";
            this.gridColumn4.FieldName = "MiddleStreet";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.OptionsColumn.AllowFocus = false;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 4;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "目的地";
            this.gridColumn5.FieldName = "Destination";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowEdit = false;
            this.gridColumn5.OptionsColumn.AllowFocus = false;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 5;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "序号";
            this.gridColumn7.FieldName = "rowid";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsColumn.AllowEdit = false;
            this.gridColumn7.OptionsColumn.AllowFocus = false;
            this.gridColumn7.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 1;
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(559, 117);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 315;
            this.btnCreate.Text = "生成";
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(263, 34);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 316;
            this.labelControl1.Text = "服务网点";
            // 
            // WebName
            // 
            this.WebName.EditValue = "";
            this.WebName.Location = new System.Drawing.Point(317, 31);
            this.WebName.Name = "WebName";
            this.WebName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.WebName.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.WebName.Size = new System.Drawing.Size(181, 21);
            this.WebName.TabIndex = 317;
            this.WebName.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(280, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 14);
            this.label1.TabIndex = 318;
            this.label1.Text = "城市";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(522, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 14);
            this.label2.TabIndex = 319;
            this.label2.Text = "区县";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(36, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 14);
            this.label4.TabIndex = 320;
            this.label4.Text = "街道";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(505, 34);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(48, 14);
            this.labelControl2.TabIndex = 321;
            this.labelControl2.Text = "服务类型";
            // 
            // Type
            // 
            this.Type.EditValue = "自提";
            this.Type.Location = new System.Drawing.Point(559, 31);
            this.Type.Name = "Type";
            this.Type.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.Type.Properties.Items.AddRange(new object[] {
            "自提",
            "送货",
            "自提+送货"});
            this.Type.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.Type.Size = new System.Drawing.Size(181, 21);
            this.Type.TabIndex = 322;
            this.Type.TabStop = false;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(29, 281);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 323;
            this.labelControl3.Text = "城市列表";
            // 
            // MiddleCity
            // 
            this.MiddleCity.Location = new System.Drawing.Point(317, 76);
            this.MiddleCity.Name = "MiddleCity";
            this.MiddleCity.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.MiddleCity.Size = new System.Drawing.Size(181, 21);
            this.MiddleCity.TabIndex = 332;
            this.MiddleCity.EditValueChanged += new System.EventHandler(this.MiddleCity_EditValueChanged);
            // 
            // MiddleStreet
            // 
            this.MiddleStreet.Location = new System.Drawing.Point(73, 119);
            this.MiddleStreet.Name = "MiddleStreet";
            this.MiddleStreet.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.MiddleStreet.Size = new System.Drawing.Size(181, 21);
            this.MiddleStreet.TabIndex = 333;
            // 
            // MiddleArea
            // 
            this.MiddleArea.Location = new System.Drawing.Point(559, 77);
            this.MiddleArea.Name = "MiddleArea";
            this.MiddleArea.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.MiddleArea.Size = new System.Drawing.Size(181, 21);
            this.MiddleArea.TabIndex = 334;
            this.MiddleArea.EditValueChanged += new System.EventHandler(this.MiddleArea_EditValueChanged);
            // 
            // Remark
            // 
            this.Remark.Location = new System.Drawing.Point(504, 163);
            this.Remark.Name = "Remark";
            this.Remark.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Remark.Properties.Appearance.Options.UseFont = true;
            this.Remark.Size = new System.Drawing.Size(329, 217);
            this.Remark.TabIndex = 337;
            this.Remark.TabStop = false;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(281, 426);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 338;
            this.btnDelete.Text = "删  除";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnDuibi
            // 
            this.btnDuibi.Location = new System.Drawing.Point(504, 386);
            this.btnDuibi.Name = "btnDuibi";
            this.btnDuibi.Size = new System.Drawing.Size(75, 23);
            this.btnDuibi.TabIndex = 339;
            this.btnDuibi.Text = "对  比";
            this.btnDuibi.Click += new System.EventHandler(this.btnDuibi_Click);
            // 
            // MiddleProvince
            // 
            this.MiddleProvince.Location = new System.Drawing.Point(83, 76);
            this.MiddleProvince.Name = "MiddleProvince";
            this.MiddleProvince.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.MiddleProvince.Size = new System.Drawing.Size(171, 21);
            this.MiddleProvince.TabIndex = 340;
            this.MiddleProvince.SelectedIndexChanged += new System.EventHandler(this.MiddleProvince_SelectedIndexChanged);
            // 
            // SiteName
            // 
            this.SiteName.Location = new System.Drawing.Point(83, 31);
            this.SiteName.Name = "SiteName";
            this.SiteName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SiteName.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.SiteName.Size = new System.Drawing.Size(171, 21);
            this.SiteName.TabIndex = 341;
            this.SiteName.EditValueChanged += new System.EventHandler(this.SiteName_EditValueChanged);
            // 
            // Company
            // 
            this.Company.Location = new System.Drawing.Point(271, 122);
            this.Company.Name = "Company";
            this.Company.Size = new System.Drawing.Size(36, 14);
            this.Company.TabIndex = 349;
            this.Company.Text = "公司ID";
            // 
            // CompanyID
            // 
            this.CompanyID.Location = new System.Drawing.Point(317, 119);
            this.CompanyID.Name = "CompanyID";
            this.CompanyID.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CompanyID.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.CompanyID.Size = new System.Drawing.Size(181, 21);
            this.CompanyID.TabIndex = 350;
            // 
            // frmbasMiddleSite_Add
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 488);
            this.Controls.Add(this.CompanyID);
            this.Controls.Add(this.Company);
            this.Controls.Add(this.SiteName);
            this.Controls.Add(this.MiddleProvince);
            this.Controls.Add(this.btnDuibi);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.Remark);
            this.Controls.Add(this.MiddleArea);
            this.Controls.Add(this.MiddleStreet);
            this.Controls.Add(this.MiddleCity);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.Type);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.WebName);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.myGridControl1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnConcel);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.labelControl17);
            this.Controls.Add(this.MiddleStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmbasMiddleSite_Add";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "新增";
            this.Load += new System.EventHandler(this.frmOrgUnit_Web_Add_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MiddleStatus.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myGridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WebName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Type.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MiddleCity.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MiddleStreet.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MiddleArea.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Remark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MiddleProvince.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SiteName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CompanyID.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.CheckEdit MiddleStatus;
        private DevExpress.XtraEditors.LabelControl labelControl17;
        private DevExpress.XtraEditors.SimpleButton btnConcel;
        private DevExpress.XtraEditors.SimpleButton btnSubmit;
        private System.Windows.Forms.Label label3;
        private ZQTMS.Lib.MyGridControl myGridControl1;
        private ZQTMS.Lib.MyGridView myGridView1;
        private DevExpress.XtraEditors.SimpleButton btnCreate;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit WebName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ComboBoxEdit Type;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.CheckedComboBoxEdit MiddleCity;
        private DevExpress.XtraEditors.CheckedComboBoxEdit MiddleStreet;
        private DevExpress.XtraEditors.CheckedComboBoxEdit MiddleArea;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraEditors.MemoEdit Remark;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private DevExpress.XtraEditors.SimpleButton btnDuibi;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraEditors.ImageComboBoxEdit MiddleProvince;
        private DevExpress.XtraEditors.CheckedComboBoxEdit SiteName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraEditors.LabelControl Company;
        private DevExpress.XtraEditors.ComboBoxEdit CompanyID;
    }
}