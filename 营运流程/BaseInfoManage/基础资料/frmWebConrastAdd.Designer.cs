namespace ZQTMS.UI.BaseInfoManage
{
    partial class frmWebConrastAdd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWebConrastAdd));
            this.label2 = new System.Windows.Forms.Label();
            this.cbbBeginWeb = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.CompanyID = new DevExpress.XtraEditors.ComboBoxEdit();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPerson = new DevExpress.XtraEditors.TextEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.txtWeb = new DevExpress.XtraEditors.TextEdit();
            this.cbbEndWeb = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbBeginWeb.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CompanyID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPerson.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWeb.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbEndWeb.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(303, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 14);
            this.label2.TabIndex = 63;
            this.label2.Text = "开单网点";
            // 
            // cbbBeginWeb
            // 
            this.cbbBeginWeb.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbbBeginWeb.EditValue = "";
            this.cbbBeginWeb.Location = new System.Drawing.Point(359, 12);
            this.cbbBeginWeb.Name = "cbbBeginWeb";
            this.cbbBeginWeb.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbbBeginWeb.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbbBeginWeb.Size = new System.Drawing.Size(207, 21);
            this.cbbBeginWeb.TabIndex = 62;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(48, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 14);
            this.label1.TabIndex = 65;
            this.label1.Text = "目的网点";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(57, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 14);
            this.label3.TabIndex = 66;
            this.label3.Text = "公司ID";
            // 
            // CompanyID
            // 
            this.CompanyID.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CompanyID.EditValue = "";
            this.CompanyID.Location = new System.Drawing.Point(112, 12);
            this.CompanyID.Name = "CompanyID";
            this.CompanyID.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CompanyID.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.CompanyID.Size = new System.Drawing.Size(173, 21);
            this.CompanyID.TabIndex = 67;
            this.CompanyID.SelectedIndexChanged += new System.EventHandler(this.CompanyID_SelectedIndexChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(372, 112);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 69;
            this.btnCancel.Text = "退出";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(175, 112);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 68;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(62, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 14);
            this.label4.TabIndex = 70;
            this.label4.Text = "编辑人";
            // 
            // txtPerson
            // 
            this.txtPerson.Enabled = false;
            this.txtPerson.EnterMoveNextControl = true;
            this.txtPerson.Location = new System.Drawing.Point(111, 72);
            this.txtPerson.Name = "txtPerson";
            this.txtPerson.Size = new System.Drawing.Size(174, 21);
            this.txtPerson.TabIndex = 71;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(293, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 14);
            this.label5.TabIndex = 72;
            this.label5.Text = "编辑人网点";
            // 
            // txtWeb
            // 
            this.txtWeb.Enabled = false;
            this.txtWeb.EnterMoveNextControl = true;
            this.txtWeb.Location = new System.Drawing.Point(360, 70);
            this.txtWeb.Name = "txtWeb";
            this.txtWeb.Size = new System.Drawing.Size(207, 21);
            this.txtWeb.TabIndex = 73;
            // 
            // cbbEndWeb
            // 
            this.cbbEndWeb.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbbEndWeb.EditValue = "";
            this.cbbEndWeb.Location = new System.Drawing.Point(112, 43);
            this.cbbEndWeb.Name = "cbbEndWeb";
            this.cbbEndWeb.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbbEndWeb.Size = new System.Drawing.Size(454, 21);
            this.cbbEndWeb.TabIndex = 64;
            // 
            // frmWebConrastAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 172);
            this.Controls.Add(this.txtWeb);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtPerson);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.CompanyID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbbBeginWeb);
            this.Controls.Add(this.cbbEndWeb);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmWebConrastAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "编辑网点对照表";
            this.Load += new System.EventHandler(this.frmWebConrastAdd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cbbBeginWeb.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CompanyID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPerson.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWeb.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbEndWeb.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.ComboBoxEdit cbbBeginWeb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.ComboBoxEdit CompanyID;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.TextEdit txtPerson;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.TextEdit txtWeb;
        private DevExpress.XtraEditors.CheckedComboBoxEdit cbbEndWeb;
    }
}