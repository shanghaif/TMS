namespace ZQTMS.UI
{
    partial class frmOilCardVerification
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOilCardVerification));
            this.OilCardNo = new DevExpress.XtraEditors.TextEdit();
            this.OilCardFee = new DevExpress.XtraEditors.TextEdit();
            this.label66 = new System.Windows.Forms.Label();
            this.label65 = new System.Windows.Forms.Label();
            this.cbclose = new DevExpress.XtraEditors.SimpleButton();
            this.cbsave = new DevExpress.XtraEditors.SimpleButton();
            this.myGridControl1 = new ZQTMS.Lib.MyGridControl();
            this.myGridView1 = new ZQTMS.Lib.MyGridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Company = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.OilCardPsW = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.OilCardNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OilCardFee.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myGridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Company.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OilCardPsW.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // OilCardNo
            // 
            this.OilCardNo.EditValue = "";
            this.OilCardNo.EnterMoveNextControl = true;
            this.OilCardNo.Location = new System.Drawing.Point(94, 33);
            this.OilCardNo.Name = "OilCardNo";
            this.OilCardNo.Properties.Appearance.Options.UseTextOptions = true;
            this.OilCardNo.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.OilCardNo.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.OilCardNo.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.OilCardNo.Size = new System.Drawing.Size(123, 21);
            this.OilCardNo.TabIndex = 92;
            this.OilCardNo.Leave += new System.EventHandler(this.OilCardNo_Leave);
            this.OilCardNo.Enter += new System.EventHandler(this.OilCardNo_Enter);
            this.OilCardNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OilCardNo_KeyDown);
            // 
            // OilCardFee
            // 
            this.OilCardFee.EditValue = "";
            this.OilCardFee.EnterMoveNextControl = true;
            this.OilCardFee.Location = new System.Drawing.Point(345, 33);
            this.OilCardFee.Name = "OilCardFee";
            this.OilCardFee.Properties.Appearance.Options.UseTextOptions = true;
            this.OilCardFee.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.OilCardFee.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.OilCardFee.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.OilCardFee.Size = new System.Drawing.Size(94, 21);
            this.OilCardFee.TabIndex = 91;
            // 
            // label66
            // 
            this.label66.AutoSize = true;
            this.label66.Location = new System.Drawing.Point(22, 36);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(67, 14);
            this.label66.TabIndex = 90;
            this.label66.Text = "油卡编号：";
            // 
            // label65
            // 
            this.label65.AutoSize = true;
            this.label65.Location = new System.Drawing.Point(274, 36);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(67, 14);
            this.label65.TabIndex = 89;
            this.label65.Text = "油卡金额：";
            // 
            // cbclose
            // 
            this.cbclose.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbclose.Appearance.Options.UseFont = true;
            this.cbclose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cbclose.Image = global::ZQTMS.UI.Properties.Resources.Action_Close;
            this.cbclose.Location = new System.Drawing.Point(262, 133);
            this.cbclose.Name = "cbclose";
            this.cbclose.Size = new System.Drawing.Size(75, 23);
            this.cbclose.TabIndex = 94;
            this.cbclose.Text = "关 闭";
            this.cbclose.Click += new System.EventHandler(this.cbclose_Click);
            // 
            // cbsave
            // 
            this.cbsave.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbsave.Appearance.Options.UseFont = true;
            this.cbsave.Image = ((System.Drawing.Image)(resources.GetObject("cbsave.Image")));
            this.cbsave.Location = new System.Drawing.Point(168, 133);
            this.cbsave.Name = "cbsave";
            this.cbsave.Size = new System.Drawing.Size(75, 23);
            this.cbsave.TabIndex = 93;
            this.cbsave.Text = "保 存";
            this.cbsave.Click += new System.EventHandler(this.cbsave_Click);
            // 
            // myGridControl1
            // 
            this.myGridControl1.Location = new System.Drawing.Point(423, 36);
            this.myGridControl1.MainView = this.myGridView1;
            this.myGridControl1.Name = "myGridControl1";
            this.myGridControl1.Size = new System.Drawing.Size(400, 200);
            this.myGridControl1.TabIndex = 95;
            this.myGridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.myGridView1});
            this.myGridControl1.Visible = false;
            this.myGridControl1.DoubleClick += new System.EventHandler(this.myGridControl1_DoubleClick);
            this.myGridControl1.Leave += new System.EventHandler(this.myGridControl1_Leave);
            this.myGridControl1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.myGridControl1_KeyDown);
            // 
            // myGridView1
            // 
            this.myGridView1.ColumnPanelRowHeight = 30;
            this.myGridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1});
            this.myGridView1.GridControl = this.myGridControl1;
            this.myGridView1.Guid = new System.Guid("a252942f-2b98-4b2b-9d73-cb53d9cc84ab");
            this.myGridView1.HiddenFiledDic = ((System.Collections.Generic.Dictionary<string, object>)(resources.GetObject("myGridView1.HiddenFiledDic")));
            this.myGridView1.MenuName = "";
            this.myGridView1.Name = "myGridView1";
            this.myGridView1.OptionsView.ColumnAutoWidth = false;
            this.myGridView1.OptionsView.ShowAutoFilterRow = true;
            this.myGridView1.OptionsView.ShowGroupPanel = false;
            this.myGridView1.WebControlBindFindName = "";
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
            // Company
            // 
            this.Company.EditValue = "";
            this.Company.EnterMoveNextControl = true;
            this.Company.Location = new System.Drawing.Point(94, 84);
            this.Company.Name = "Company";
            this.Company.Properties.Appearance.Options.UseTextOptions = true;
            this.Company.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.Company.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Company.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Company.Size = new System.Drawing.Size(123, 21);
            this.Company.TabIndex = 97;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 14);
            this.label1.TabIndex = 96;
            this.label1.Text = "所属公司：";
            // 
            // OilCardPsW
            // 
            this.OilCardPsW.EditValue = "";
            this.OilCardPsW.EnterMoveNextControl = true;
            this.OilCardPsW.Location = new System.Drawing.Point(345, 84);
            this.OilCardPsW.Name = "OilCardPsW";
            this.OilCardPsW.Properties.Appearance.Options.UseTextOptions = true;
            this.OilCardPsW.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.OilCardPsW.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.OilCardPsW.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.OilCardPsW.Size = new System.Drawing.Size(94, 21);
            this.OilCardPsW.TabIndex = 99;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(274, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 14);
            this.label2.TabIndex = 98;
            this.label2.Text = "油卡密码：";
            // 
            // frmOilCardVerification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(547, 257);
            this.Controls.Add(this.myGridControl1);
            this.Controls.Add(this.OilCardPsW);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Company);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbclose);
            this.Controls.Add(this.cbsave);
            this.Controls.Add(this.OilCardNo);
            this.Controls.Add(this.OilCardFee);
            this.Controls.Add(this.label66);
            this.Controls.Add(this.label65);
            this.Name = "frmOilCardVerification";
            this.Text = "油卡选择";
            this.Load += new System.EventHandler(this.frmOilCardChoose_Load);
            ((System.ComponentModel.ISupportInitialize)(this.OilCardNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OilCardFee.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myGridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Company.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OilCardPsW.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit OilCardNo;
        private DevExpress.XtraEditors.TextEdit OilCardFee;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.Label label65;
        private DevExpress.XtraEditors.SimpleButton cbclose;
        private DevExpress.XtraEditors.SimpleButton cbsave;
        private ZQTMS.Lib.MyGridControl myGridControl1;
        private ZQTMS.Lib.MyGridView myGridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.TextEdit Company;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit OilCardPsW;
        private System.Windows.Forms.Label label2;
    }
}