namespace ZQTMS.UI
{
    partial class frmAddInventory
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtInventoryDate = new DevExpress.XtraEditors.DateEdit();
            this.txtInventorySite = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txtInventoryWeb = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txtInvCommond = new DevExpress.XtraEditors.MemoEdit();
            this.txtInvMark = new DevExpress.XtraEditors.MemoEdit();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtInventoryDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInventoryDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInventorySite.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInventoryWeb.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInvCommond.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInvMark.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "盘点批次:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "预计盘点完成时间:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 14);
            this.label3.TabIndex = 2;
            this.label3.Text = "盘点站点:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(31, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 14);
            this.label4.TabIndex = 3;
            this.label4.Text = "盘点网点:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(31, 198);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 14);
            this.label5.TabIndex = 4;
            this.label5.Text = "盘点要求:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(31, 269);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 14);
            this.label6.TabIndex = 5;
            this.label6.Text = "盘点备注:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(118, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 14);
            this.label7.TabIndex = 6;
            this.label7.Text = "label7";
            // 
            // txtInventoryDate
            // 
            this.txtInventoryDate.EditValue = null;
            this.txtInventoryDate.Location = new System.Drawing.Point(119, 63);
            this.txtInventoryDate.Name = "txtInventoryDate";
            this.txtInventoryDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtInventoryDate.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.txtInventoryDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtInventoryDate.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.txtInventoryDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtInventoryDate.Properties.Mask.EditMask = "yyyy-MM-dd HH:mm";
            this.txtInventoryDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtInventoryDate.Size = new System.Drawing.Size(215, 21);
            this.txtInventoryDate.TabIndex = 7;
            // 
            // txtInventorySite
            // 
            this.txtInventorySite.Location = new System.Drawing.Point(119, 104);
            this.txtInventorySite.Name = "txtInventorySite";
            this.txtInventorySite.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtInventorySite.Size = new System.Drawing.Size(215, 21);
            this.txtInventorySite.TabIndex = 8;
            this.txtInventorySite.SelectedIndexChanged += new System.EventHandler(this.txtInventorySite_SelectedIndexChanged);
            // 
            // txtInventoryWeb
            // 
            this.txtInventoryWeb.Location = new System.Drawing.Point(119, 149);
            this.txtInventoryWeb.Name = "txtInventoryWeb";
            this.txtInventoryWeb.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtInventoryWeb.Size = new System.Drawing.Size(215, 21);
            this.txtInventoryWeb.TabIndex = 9;
            // 
            // txtInvCommond
            // 
            this.txtInvCommond.Location = new System.Drawing.Point(119, 195);
            this.txtInvCommond.Name = "txtInvCommond";
            this.txtInvCommond.Size = new System.Drawing.Size(215, 66);
            this.txtInvCommond.TabIndex = 10;
            this.txtInvCommond.TabStop = false;
            // 
            // txtInvMark
            // 
            this.txtInvMark.Location = new System.Drawing.Point(119, 267);
            this.txtInvMark.Name = "txtInvMark";
            this.txtInvMark.Size = new System.Drawing.Size(215, 61);
            this.txtInvMark.TabIndex = 11;
            this.txtInvMark.TabStop = false;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(44, 353);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "保 存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(227, 353);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "关 闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmAddInventory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 390);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtInventoryWeb);
            this.Controls.Add(this.txtInventorySite);
            this.Controls.Add(this.txtInventoryDate);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtInvCommond);
            this.Controls.Add(this.txtInvMark);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmAddInventory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "安排盘点信息";
            this.Load += new System.EventHandler(this.frmAddInventory_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtInventoryDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInventoryDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInventorySite.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInventoryWeb.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInvCommond.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInvMark.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.DateEdit txtInventoryDate;
        private DevExpress.XtraEditors.ComboBoxEdit txtInventorySite;
        private DevExpress.XtraEditors.ComboBoxEdit txtInventoryWeb;
        private DevExpress.XtraEditors.MemoEdit txtInvCommond;
        private DevExpress.XtraEditors.MemoEdit txtInvMark;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnClose;
    }
}