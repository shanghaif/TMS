namespace ZQTMS.UI
{
    partial class JMfrmCreditAccountAdd
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
            this.CreditLimit = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.AccountType = new DevExpress.XtraEditors.TextEdit();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.AccountNo = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.StartTime = new DevExpress.XtraEditors.DateEdit();
            this.EndTime = new DevExpress.XtraEditors.DateEdit();
            this.AccountName = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.CreditLimit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AccountType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AccountNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartTime.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndTime.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AccountName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // CreditLimit
            // 
            this.CreditLimit.Location = new System.Drawing.Point(130, 182);
            this.CreditLimit.Name = "CreditLimit";
            this.CreditLimit.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CreditLimit.Properties.Appearance.Options.UseFont = true;
            this.CreditLimit.Size = new System.Drawing.Size(201, 21);
            this.CreditLimit.TabIndex = 322;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(64, 275);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 328;
            this.labelControl1.Text = "结束日期";
            // 
            // AccountType
            // 
            this.AccountType.Enabled = false;
            this.AccountType.Location = new System.Drawing.Point(130, 137);
            this.AccountType.Name = "AccountType";
            this.AccountType.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AccountType.Properties.Appearance.Options.UseFont = true;
            this.AccountType.Size = new System.Drawing.Size(201, 21);
            this.AccountType.TabIndex = 321;
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(64, 140);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(48, 14);
            this.labelControl8.TabIndex = 327;
            this.labelControl8.Text = "账户类型";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(64, 185);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(48, 14);
            this.labelControl6.TabIndex = 326;
            this.labelControl6.Text = "授信额度";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(64, 230);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(48, 14);
            this.labelControl5.TabIndex = 325;
            this.labelControl5.Text = "启用日期";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(64, 46);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(48, 14);
            this.labelControl2.TabIndex = 330;
            this.labelControl2.Text = "账户名称";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(256, 331);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 332;
            this.btnCancel.Text = "退出";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(130, 331);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 331;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // AccountNo
            // 
            this.AccountNo.Enabled = false;
            this.AccountNo.Location = new System.Drawing.Point(130, 89);
            this.AccountNo.Name = "AccountNo";
            this.AccountNo.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AccountNo.Properties.Appearance.Options.UseFont = true;
            this.AccountNo.Size = new System.Drawing.Size(201, 21);
            this.AccountNo.TabIndex = 335;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(64, 92);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 334;
            this.labelControl3.Text = "账户编号";
            // 
            // StartTime
            // 
            this.StartTime.EditValue = new System.DateTime(2006, 1, 26, 0, 0, 0, 0);
            this.StartTime.Location = new System.Drawing.Point(130, 227);
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
            this.StartTime.Size = new System.Drawing.Size(201, 21);
            this.StartTime.TabIndex = 336;
            // 
            // EndTime
            // 
            this.EndTime.EditValue = new System.DateTime(2006, 1, 26, 0, 0, 0, 0);
            this.EndTime.Location = new System.Drawing.Point(130, 272);
            this.EndTime.Name = "EndTime";
            this.EndTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.EndTime.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.EndTime.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.EndTime.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.EndTime.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.EndTime.Properties.Mask.EditMask = "yyyy-MM-dd HH:mm";
            this.EndTime.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.EndTime.Size = new System.Drawing.Size(201, 21);
            this.EndTime.TabIndex = 337;
            // 
            // AccountName
            // 
            this.AccountName.Location = new System.Drawing.Point(130, 43);
            this.AccountName.Name = "AccountName";
            this.AccountName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.AccountName.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.AccountName.Size = new System.Drawing.Size(201, 21);
            this.AccountName.TabIndex = 338;
            this.AccountName.SelectedIndexChanged += new System.EventHandler(this.AccountName_SelectedIndexChanged);
            // 
            // JMfrmCreditAccountAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 418);
            this.Controls.Add(this.AccountName);
            this.Controls.Add(this.EndTime);
            this.Controls.Add(this.StartTime);
            this.Controls.Add(this.AccountNo);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.CreditLimit);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.AccountType);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.labelControl5);
            this.Name = "JMfrmCreditAccountAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "修改授信账户";
            this.Load += new System.EventHandler(this.JMfrmCreditAccountAdd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CreditLimit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AccountType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AccountNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartTime.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndTime.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AccountName.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit CreditLimit;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit AccountType;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave; 
        private DevExpress.XtraEditors.TextEdit AccountNo;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.DateEdit StartTime;
        private DevExpress.XtraEditors.DateEdit EndTime;
        private DevExpress.XtraEditors.ComboBoxEdit AccountName;
    }
}