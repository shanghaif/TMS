namespace ZQTMS.UI.BaseInfoManage
{
    partial class frmLinesConfigurationAdd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLinesConfigurationAdd));
            this.txtSubCompanyID = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtOperMan = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txtOperWeb = new DevExpress.XtraEditors.TextEdit();
            this.txtDate = new DevExpress.XtraEditors.DateEdit();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.txtCompanyID = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSubCompanyID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOperMan.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOperWeb.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCompanyID.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSubCompanyID
            // 
            this.txtSubCompanyID.Location = new System.Drawing.Point(95, 51);
            this.txtSubCompanyID.Name = "txtSubCompanyID";
            this.txtSubCompanyID.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtSubCompanyID.Properties.DropDownRows = 20;
            this.txtSubCompanyID.Size = new System.Drawing.Size(344, 21);
            this.txtSubCompanyID.TabIndex = 11;
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(58, 27);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(36, 14);
            this.labelControl8.TabIndex = 296;
            this.labelControl8.Text = "公司：";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(249, 27);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 297;
            this.labelControl1.Text = "操作人：";
            // 
            // txtOperMan
            // 
            this.txtOperMan.Location = new System.Drawing.Point(292, 24);
            this.txtOperMan.Name = "txtOperMan";
            this.txtOperMan.Size = new System.Drawing.Size(147, 21);
            this.txtOperMan.TabIndex = 298;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(46, 55);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(48, 14);
            this.labelControl2.TabIndex = 299;
            this.labelControl2.Text = "子公司：";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(22, 82);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(72, 14);
            this.labelControl3.TabIndex = 300;
            this.labelControl3.Text = "操作人网点：";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(242, 82);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 14);
            this.labelControl4.TabIndex = 301;
            this.labelControl4.Text = "操作时间：";
            // 
            // txtOperWeb
            // 
            this.txtOperWeb.Location = new System.Drawing.Point(95, 79);
            this.txtOperWeb.Name = "txtOperWeb";
            this.txtOperWeb.Size = new System.Drawing.Size(139, 21);
            this.txtOperWeb.TabIndex = 302;
            // 
            // txtDate
            // 
            this.txtDate.EditValue = new System.DateTime(2006, 1, 26, 0, 0, 0, 0);
            this.txtDate.Location = new System.Drawing.Point(298, 79);
            this.txtDate.Name = "txtDate";
            this.txtDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDate.Properties.DisplayFormat.FormatString = "yyyy-M-d H:mm";
            this.txtDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtDate.Properties.Mask.EditMask = "yyyy-M-d H:mm";
            this.txtDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtDate.Size = new System.Drawing.Size(141, 21);
            this.txtDate.TabIndex = 303;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(295, 121);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 305;
            this.btnCancel.Text = "退出";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(98, 121);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 304;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtCompanyID
            // 
            this.txtCompanyID.Location = new System.Drawing.Point(95, 24);
            this.txtCompanyID.Name = "txtCompanyID";
            this.txtCompanyID.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtCompanyID.Size = new System.Drawing.Size(140, 21);
            this.txtCompanyID.TabIndex = 10;
            // 
            // frmLinesConfigurationAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 185);
            this.Controls.Add(this.txtDate);
            this.Controls.Add(this.txtOperWeb);
            this.Controls.Add(this.txtSubCompanyID);
            this.Controls.Add(this.txtOperMan);
            this.Controls.Add(this.txtCompanyID);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.labelControl8);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLinesConfigurationAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "编辑专线配置";
            this.Load += new System.EventHandler(this.frmLinesConfigurationAdd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtSubCompanyID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOperMan.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOperWeb.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCompanyID.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.CheckedComboBoxEdit txtSubCompanyID;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtOperMan;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit txtOperWeb;
        private DevExpress.XtraEditors.DateEdit txtDate;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.CheckedComboBoxEdit txtCompanyID;
    }
}