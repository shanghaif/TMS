namespace ZQTMS.UI
{
    partial class frmUnusualRegister
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
        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.到站 = new System.Windows.Forms.Label();
            this.simpleButton13 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.RegisterDate = new DevExpress.XtraEditors.DateEdit();
            this.FeedBackContent = new DevExpress.XtraEditors.MemoEdit();
            this.label32 = new System.Windows.Forms.Label();
            this.BadImgSrc1 = new DevExpress.XtraEditors.ButtonEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.BadImgSrc2 = new DevExpress.XtraEditors.ButtonEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.RegisterMan = new DevExpress.XtraEditors.TextEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.FeedBackMan = new DevExpress.XtraEditors.TextEdit();
            this.FeedBackDept = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.RegisterDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RegisterDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FeedBackContent.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BadImgSrc1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BadImgSrc2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RegisterMan.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FeedBackMan.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FeedBackDept.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 14);
            this.label1.TabIndex = 126;
            this.label1.Text = "登记日期";
            // 
            // 到站
            // 
            this.到站.AutoSize = true;
            this.到站.Location = new System.Drawing.Point(24, 75);
            this.到站.Name = "到站";
            this.到站.Size = new System.Drawing.Size(55, 14);
            this.到站.TabIndex = 128;
            this.到站.Text = "登记内容";
            // 
            // simpleButton13
            // 
            this.simpleButton13.Location = new System.Drawing.Point(125, 265);
            this.simpleButton13.Name = "simpleButton13";
            this.simpleButton13.Size = new System.Drawing.Size(75, 23);
            this.simpleButton13.TabIndex = 139;
            this.simpleButton13.Text = "保存";
            this.simpleButton13.Click += new System.EventHandler(this.simpleButton13_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(304, 265);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 23);
            this.simpleButton1.TabIndex = 140;
            this.simpleButton1.Text = "取消";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // RegisterDate
            // 
            this.RegisterDate.EditValue = null;
            this.RegisterDate.Enabled = false;
            this.RegisterDate.Location = new System.Drawing.Point(87, 12);
            this.RegisterDate.Name = "RegisterDate";
            this.RegisterDate.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.RegisterDate.Properties.Appearance.Options.UseForeColor = true;
            this.RegisterDate.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.RegisterDate.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.RegisterDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.RegisterDate.Properties.Mask.EditMask = "";
            this.RegisterDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.RegisterDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.RegisterDate.Size = new System.Drawing.Size(180, 21);
            this.RegisterDate.TabIndex = 1;
            this.RegisterDate.TabStop = false;
            // 
            // FeedBackContent
            // 
            this.FeedBackContent.Location = new System.Drawing.Point(87, 73);
            this.FeedBackContent.Name = "FeedBackContent";
            this.FeedBackContent.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.FeedBackContent.Properties.Appearance.Options.UseForeColor = true;
            this.FeedBackContent.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.FeedBackContent.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.FeedBackContent.Size = new System.Drawing.Size(429, 108);
            this.FeedBackContent.TabIndex = 2;
            this.FeedBackContent.TabStop = false;
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(24, 201);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(62, 14);
            this.label32.TabIndex = 210;
            this.label32.Text = "货损照片1";
            // 
            // BadImgSrc1
            // 
            this.BadImgSrc1.Location = new System.Drawing.Point(87, 198);
            this.BadImgSrc1.Name = "BadImgSrc1";
            this.BadImgSrc1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.BadImgSrc1.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.BadImgSrc1.Size = new System.Drawing.Size(429, 21);
            this.BadImgSrc1.TabIndex = 211;
            this.BadImgSrc1.ToolTip = "选择货损照片...";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 228);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 14);
            this.label2.TabIndex = 212;
            this.label2.Text = "货损照片2";
            // 
            // BadImgSrc2
            // 
            this.BadImgSrc2.Location = new System.Drawing.Point(87, 225);
            this.BadImgSrc2.Name = "BadImgSrc2";
            this.BadImgSrc2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.BadImgSrc2.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.BadImgSrc2.Size = new System.Drawing.Size(429, 21);
            this.BadImgSrc2.TabIndex = 213;
            this.BadImgSrc2.ToolTip = "选择货损照片...";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(275, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 14);
            this.label3.TabIndex = 215;
            this.label3.Text = "登记人";
            // 
            // RegisterMan
            // 
            this.RegisterMan.Location = new System.Drawing.Point(336, 12);
            this.RegisterMan.Name = "RegisterMan";
            this.RegisterMan.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.RegisterMan.Properties.Appearance.Options.UseForeColor = true;
            this.RegisterMan.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.RegisterMan.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.RegisterMan.Properties.DisplayFormat.FormatString = "d";
            this.RegisterMan.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.RegisterMan.Properties.EditFormat.FormatString = "d";
            this.RegisterMan.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.RegisterMan.Properties.ReadOnly = true;
            this.RegisterMan.Size = new System.Drawing.Size(180, 21);
            this.RegisterMan.TabIndex = 214;
            this.RegisterMan.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 14);
            this.label4.TabIndex = 217;
            this.label4.Text = "反馈部门";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(275, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 14);
            this.label5.TabIndex = 219;
            this.label5.Text = "反馈人";
            // 
            // FeedBackMan
            // 
            this.FeedBackMan.Location = new System.Drawing.Point(336, 39);
            this.FeedBackMan.Name = "FeedBackMan";
            this.FeedBackMan.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.FeedBackMan.Properties.Appearance.Options.UseForeColor = true;
            this.FeedBackMan.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.FeedBackMan.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.FeedBackMan.Properties.DisplayFormat.FormatString = "d";
            this.FeedBackMan.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.FeedBackMan.Properties.EditFormat.FormatString = "d";
            this.FeedBackMan.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.FeedBackMan.Size = new System.Drawing.Size(180, 21);
            this.FeedBackMan.TabIndex = 218;
            this.FeedBackMan.TabStop = false;
            // 
            // FeedBackDept
            // 
            this.FeedBackDept.Location = new System.Drawing.Point(87, 39);
            this.FeedBackDept.Name = "FeedBackDept";
            this.FeedBackDept.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.FeedBackDept.Properties.Appearance.Options.UseForeColor = true;
            this.FeedBackDept.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.FeedBackDept.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.FeedBackDept.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FeedBackDept.Properties.DisplayFormat.FormatString = "d";
            this.FeedBackDept.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.FeedBackDept.Properties.EditFormat.FormatString = "d";
            this.FeedBackDept.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.FeedBackDept.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.FeedBackDept.Size = new System.Drawing.Size(180, 21);
            this.FeedBackDept.TabIndex = 216;
            this.FeedBackDept.TabStop = false;
            // 
            // frmUnusualRegister
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 325);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.FeedBackMan);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BadImgSrc2);
            this.Controls.Add(this.label32);
            this.Controls.Add(this.BadImgSrc1);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.simpleButton13);
            this.Controls.Add(this.到站);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RegisterDate);
            this.Controls.Add(this.FeedBackContent);
            this.Controls.Add(this.RegisterMan);
            this.Controls.Add(this.FeedBackDept);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUnusualRegister";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "异常登记";
            this.Load += new System.EventHandler(this.frmUnusualRegister_Load);
            ((System.ComponentModel.ISupportInitialize)(this.RegisterDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RegisterDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FeedBackContent.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BadImgSrc1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BadImgSrc2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RegisterMan.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FeedBackMan.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FeedBackDept.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label 到站;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SimpleButton simpleButton13;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.DateEdit RegisterDate;
        private DevExpress.XtraEditors.MemoEdit FeedBackContent;
        private System.Windows.Forms.Label label32;
        private DevExpress.XtraEditors.ButtonEdit BadImgSrc1;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.ButtonEdit BadImgSrc2;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.TextEdit RegisterMan;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.TextEdit FeedBackMan;
        private DevExpress.XtraEditors.ComboBoxEdit FeedBackDept;
    }
}