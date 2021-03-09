namespace ZQTMS.UI
{
    partial class frmDepartmentPrizePenaltyADD
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
            this.label9 = new System.Windows.Forms.Label();
            this.DJWeb = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Billno = new DevExpress.XtraEditors.TextEdit();
            this.Type = new DevExpress.XtraEditors.ComboBoxEdit();
            this.Money = new DevExpress.XtraEditors.TextEdit();
            this.ResponsibilityWeb = new DevExpress.XtraEditors.ComboBoxEdit();
            this.ResponsibilityMan = new DevExpress.XtraEditors.TextEdit();
            this.Abstract = new DevExpress.XtraEditors.MemoEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.DJWeb.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Billno.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Type.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Money.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResponsibilityWeb.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResponsibilityMan.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abstract.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(50, 21);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 14);
            this.label9.TabIndex = 72;
            this.label9.Text = "登记部门";
            // 
            // DJWeb
            // 
            this.DJWeb.EditValue = "全部";
            this.DJWeb.Enabled = false;
            this.DJWeb.Location = new System.Drawing.Point(122, 18);
            this.DJWeb.Name = "DJWeb";
            this.DJWeb.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DJWeb.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.DJWeb.Size = new System.Drawing.Size(254, 21);
            this.DJWeb.TabIndex = 76;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(50, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 14);
            this.label1.TabIndex = 77;
            this.label1.Text = "运 单 号";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(50, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 14);
            this.label2.TabIndex = 78;
            this.label2.Text = "奖罚类型";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(50, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 14);
            this.label3.TabIndex = 79;
            this.label3.Text = "金     额";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(50, 149);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 14);
            this.label4.TabIndex = 80;
            this.label4.Text = "责任部门";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(50, 188);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 14);
            this.label5.TabIndex = 81;
            this.label5.Text = "责 任 人";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(50, 227);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 14);
            this.label6.TabIndex = 82;
            this.label6.Text = "摘    要";
            // 
            // Billno
            // 
            this.Billno.Location = new System.Drawing.Point(122, 48);
            this.Billno.Name = "Billno";
            this.Billno.Size = new System.Drawing.Size(254, 21);
            this.Billno.TabIndex = 83;
            // 
            // Type
            // 
            this.Type.Location = new System.Drawing.Point(122, 82);
            this.Type.Name = "Type";
            this.Type.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.Type.Properties.Items.AddRange(new object[] {
            "奖励",
            "扣款"});
            this.Type.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.Type.Size = new System.Drawing.Size(254, 21);
            this.Type.TabIndex = 84;
            // 
            // Money
            // 
            this.Money.Location = new System.Drawing.Point(122, 112);
            this.Money.Name = "Money";
            this.Money.Size = new System.Drawing.Size(254, 21);
            this.Money.TabIndex = 85;
            // 
            // ResponsibilityWeb
            // 
            this.ResponsibilityWeb.Location = new System.Drawing.Point(122, 146);
            this.ResponsibilityWeb.Name = "ResponsibilityWeb";
            this.ResponsibilityWeb.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ResponsibilityWeb.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.ResponsibilityWeb.Size = new System.Drawing.Size(254, 21);
            this.ResponsibilityWeb.TabIndex = 86;
            // 
            // ResponsibilityMan
            // 
            this.ResponsibilityMan.Location = new System.Drawing.Point(122, 185);
            this.ResponsibilityMan.Name = "ResponsibilityMan";
            this.ResponsibilityMan.Size = new System.Drawing.Size(254, 21);
            this.ResponsibilityMan.TabIndex = 87;
            // 
            // Abstract
            // 
            this.Abstract.Location = new System.Drawing.Point(122, 231);
            this.Abstract.Name = "Abstract";
            this.Abstract.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.Abstract.Properties.Appearance.Options.UseForeColor = true;
            this.Abstract.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Abstract.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.Abstract.Size = new System.Drawing.Size(254, 68);
            this.Abstract.TabIndex = 88;
            this.Abstract.TabStop = false;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(122, 317);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 23);
            this.simpleButton1.TabIndex = 89;
            this.simpleButton1.Text = "保存";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(236, 316);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(75, 23);
            this.simpleButton2.TabIndex = 90;
            this.simpleButton2.Text = "退出";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // frmDepartmentPrizePenaltyADD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 392);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.Abstract);
            this.Controls.Add(this.ResponsibilityMan);
            this.Controls.Add(this.ResponsibilityWeb);
            this.Controls.Add(this.Money);
            this.Controls.Add(this.Type);
            this.Controls.Add(this.Billno);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DJWeb);
            this.Controls.Add(this.label9);
            this.Name = "frmDepartmentPrizePenaltyADD";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "新增/修改";
            this.Load += new System.EventHandler(this.frmDepartmentPrizePenaltyADD_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DJWeb.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Billno.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Type.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Money.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResponsibilityWeb.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResponsibilityMan.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abstract.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label9;
        private DevExpress.XtraEditors.ComboBoxEdit DJWeb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.TextEdit Billno;
        private DevExpress.XtraEditors.ComboBoxEdit Type;
        private DevExpress.XtraEditors.TextEdit Money;
        private DevExpress.XtraEditors.ComboBoxEdit ResponsibilityWeb;
        private DevExpress.XtraEditors.TextEdit ResponsibilityMan;
        private DevExpress.XtraEditors.MemoEdit Abstract;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
    }
}