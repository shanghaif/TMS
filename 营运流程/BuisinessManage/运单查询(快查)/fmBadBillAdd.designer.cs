namespace ZQTMS.UI
{
    partial class fmBadBillAdd
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
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
            this.baddate = new DevExpress.XtraEditors.DateEdit();
            this.badcontent = new DevExpress.XtraEditors.MemoEdit();
            this.label32 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.badcreateby = new DevExpress.XtraEditors.TextEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.fangkuiman = new DevExpress.XtraEditors.TextEdit();
            this.fangkuiwebid = new DevExpress.XtraEditors.ComboBoxEdit();
            this.btnUpload = new DevExpress.XtraEditors.SimpleButton();
            this.lblUpText = new System.Windows.Forms.Label();
            this.ExceDepart = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.ExceType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.ExceType2 = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.baddate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.baddate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.badcontent.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.badcreateby.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fangkuiman.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fangkuiwebid.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExceDepart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExceType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExceType2.Properties)).BeginInit();
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
            this.到站.Location = new System.Drawing.Point(24, 136);
            this.到站.Name = "到站";
            this.到站.Size = new System.Drawing.Size(55, 14);
            this.到站.TabIndex = 128;
            this.到站.Text = "登记内容";
            // 
            // simpleButton13
            // 
            this.simpleButton13.Location = new System.Drawing.Point(122, 319);
            this.simpleButton13.Name = "simpleButton13";
            this.simpleButton13.Size = new System.Drawing.Size(75, 23);
            this.simpleButton13.TabIndex = 139;
            this.simpleButton13.Text = "保存";
            this.simpleButton13.Click += new System.EventHandler(this.simpleButton13_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(305, 319);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 23);
            this.simpleButton1.TabIndex = 140;
            this.simpleButton1.Text = "取消";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // baddate
            // 
            this.baddate.EditValue = null;
            this.baddate.Enabled = false;
            this.baddate.Location = new System.Drawing.Point(87, 12);
            this.baddate.Name = "baddate";
            this.baddate.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.baddate.Properties.Appearance.Options.UseForeColor = true;
            this.baddate.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.baddate.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.baddate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.baddate.Properties.Mask.EditMask = "";
            this.baddate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.baddate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.baddate.Size = new System.Drawing.Size(180, 21);
            this.baddate.TabIndex = 1;
            this.baddate.TabStop = false;
            // 
            // badcontent
            // 
            this.badcontent.Location = new System.Drawing.Point(87, 134);
            this.badcontent.Name = "badcontent";
            this.badcontent.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.badcontent.Properties.Appearance.Options.UseForeColor = true;
            this.badcontent.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.badcontent.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.badcontent.Size = new System.Drawing.Size(429, 108);
            this.badcontent.TabIndex = 2;
            this.badcontent.TabStop = false;
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(24, 259);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(55, 14);
            this.label32.TabIndex = 210;
            this.label32.Text = "货损照片";
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
            // badcreateby
            // 
            this.badcreateby.Location = new System.Drawing.Point(336, 12);
            this.badcreateby.Name = "badcreateby";
            this.badcreateby.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.badcreateby.Properties.Appearance.Options.UseForeColor = true;
            this.badcreateby.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.badcreateby.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.badcreateby.Properties.DisplayFormat.FormatString = "d";
            this.badcreateby.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.badcreateby.Properties.EditFormat.FormatString = "d";
            this.badcreateby.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.badcreateby.Properties.ReadOnly = true;
            this.badcreateby.Size = new System.Drawing.Size(180, 21);
            this.badcreateby.TabIndex = 214;
            this.badcreateby.TabStop = false;
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
            // fangkuiman
            // 
            this.fangkuiman.Location = new System.Drawing.Point(336, 39);
            this.fangkuiman.Name = "fangkuiman";
            this.fangkuiman.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.fangkuiman.Properties.Appearance.Options.UseForeColor = true;
            this.fangkuiman.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.fangkuiman.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.fangkuiman.Properties.DisplayFormat.FormatString = "d";
            this.fangkuiman.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.fangkuiman.Properties.EditFormat.FormatString = "d";
            this.fangkuiman.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.fangkuiman.Size = new System.Drawing.Size(180, 21);
            this.fangkuiman.TabIndex = 218;
            this.fangkuiman.TabStop = false;
            // 
            // fangkuiwebid
            // 
            this.fangkuiwebid.Location = new System.Drawing.Point(87, 39);
            this.fangkuiwebid.Name = "fangkuiwebid";
            this.fangkuiwebid.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.fangkuiwebid.Properties.Appearance.Options.UseForeColor = true;
            this.fangkuiwebid.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.fangkuiwebid.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.fangkuiwebid.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.fangkuiwebid.Properties.DisplayFormat.FormatString = "d";
            this.fangkuiwebid.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.fangkuiwebid.Properties.EditFormat.FormatString = "d";
            this.fangkuiwebid.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.fangkuiwebid.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.fangkuiwebid.Size = new System.Drawing.Size(180, 21);
            this.fangkuiwebid.TabIndex = 216;
            this.fangkuiwebid.TabStop = false;
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(86, 259);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(75, 23);
            this.btnUpload.TabIndex = 220;
            this.btnUpload.Text = "上传照片";
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // lblUpText
            // 
            this.lblUpText.AutoSize = true;
            this.lblUpText.ForeColor = System.Drawing.Color.Blue;
            this.lblUpText.Location = new System.Drawing.Point(83, 285);
            this.lblUpText.Name = "lblUpText";
            this.lblUpText.Size = new System.Drawing.Size(79, 14);
            this.lblUpText.TabIndex = 221;
            this.lblUpText.Text = "图片未上传！";
            // 
            // ExceDepart
            // 
            this.ExceDepart.Location = new System.Drawing.Point(336, 67);
            this.ExceDepart.Name = "ExceDepart";
            this.ExceDepart.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ExceDepart.Size = new System.Drawing.Size(180, 21);
            this.ExceDepart.TabIndex = 225;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(275, 70);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 14);
            this.label6.TabIndex = 224;
            this.label6.Text = "责任部门";
            // 
            // ExceType
            // 
            this.ExceType.Location = new System.Drawing.Point(87, 67);
            this.ExceType.Name = "ExceType";
            this.ExceType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ExceType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.ExceType.Size = new System.Drawing.Size(180, 21);
            this.ExceType.TabIndex = 223;
            this.ExceType.SelectedIndexChanged += new System.EventHandler(this.ExceType1_SelectedIndexChanged_1);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 14);
            this.label2.TabIndex = 222;
            this.label2.Text = "异常项目";
            // 
            // ExceType2
            // 
            this.ExceType2.Location = new System.Drawing.Point(86, 96);
            this.ExceType2.Name = "ExceType2";
            this.ExceType2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ExceType2.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.ExceType2.Size = new System.Drawing.Size(180, 21);
            this.ExceType2.TabIndex = 227;
            this.ExceType2.SelectedIndexChanged += new System.EventHandler(this.ExceType2_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(23, 99);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 14);
            this.label7.TabIndex = 226;
            this.label7.Text = "异常类型";
            // 
            // fmBadBillAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 372);
            this.Controls.Add(this.ExceType2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.ExceDepart);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.ExceType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblUpText);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.fangkuiman);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label32);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.simpleButton13);
            this.Controls.Add(this.到站);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.baddate);
            this.Controls.Add(this.badcontent);
            this.Controls.Add(this.badcreateby);
            this.Controls.Add(this.fangkuiwebid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fmBadBillAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "异常登记";
            this.Load += new System.EventHandler(this.w_bad_tyd_add_sa_Load);
            ((System.ComponentModel.ISupportInitialize)(this.baddate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.baddate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.badcontent.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.badcreateby.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fangkuiman.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fangkuiwebid.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExceDepart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExceType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExceType2.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label 到站;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SimpleButton simpleButton13;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.DateEdit baddate;
        private DevExpress.XtraEditors.MemoEdit badcontent;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.TextEdit badcreateby;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.TextEdit fangkuiman;
        private DevExpress.XtraEditors.ComboBoxEdit fangkuiwebid;
        private DevExpress.XtraEditors.SimpleButton btnUpload;
        private System.Windows.Forms.Label lblUpText;
        private DevExpress.XtraEditors.ComboBoxEdit ExceDepart;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.ComboBoxEdit ExceType;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.ComboBoxEdit ExceType2;
        private System.Windows.Forms.Label label7;
    }
}