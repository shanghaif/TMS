namespace ZQTMS.UI
{
    partial class frmBankAdjustUpt
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
            this.components = new System.ComponentModel.Container();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.textEdit17 = new DevExpress.XtraEditors.TextEdit();
            this.label19 = new System.Windows.Forms.Label();
            this.edbankname = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.oilCardProvince = new DevExpress.XtraEditors.ComboBoxEdit();
            this.oilCardCity = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit17.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edbankname.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.oilCardProvince.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.oilCardCity.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(123, 296);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 23);
            this.simpleButton1.TabIndex = 9;
            this.simpleButton1.Text = "保存";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(375, 296);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(75, 23);
            this.simpleButton2.TabIndex = 10;
            this.simpleButton2.Text = "取消";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(42, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 14);
            this.label4.TabIndex = 20;
            this.label4.Text = "开户姓名：";
            // 
            // textEdit17
            // 
            this.textEdit17.Location = new System.Drawing.Point(123, 208);
            this.textEdit17.Name = "textEdit17";
            this.textEdit17.Size = new System.Drawing.Size(159, 21);
            this.textEdit17.TabIndex = 44;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(37, 211);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(67, 14);
            this.label19.TabIndex = 43;
            this.label19.Text = "银行账户：";
            // 
            // edbankname
            // 
            this.edbankname.EnterMoveNextControl = true;
            this.edbankname.Location = new System.Drawing.Point(390, 51);
            this.edbankname.Name = "edbankname";
            this.edbankname.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.edbankname.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.edbankname.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.edbankname.Properties.DropDownRows = 20;
            this.edbankname.Properties.Items.AddRange(new object[] {
            "中国工商银行",
            "中国银行",
            "平安银行",
            "中国交通银行",
            "中国农业银行",
            "中国建设银行",
            "中国邮政储蓄",
            "中国招商银行",
            "中国中信银行",
            "中国兴业银行",
            "中国浦东发展银行",
            "中国华夏银行",
            "中国民生银行",
            "中国深圳发展银行",
            "中国光大银行",
            "广东发展银行"});
            this.edbankname.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.edbankname.Size = new System.Drawing.Size(137, 21);
            this.edbankname.TabIndex = 45;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(313, 52);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 14);
            this.label6.TabIndex = 46;
            this.label6.Text = "开户银行：";
            // 
            // textEdit1
            // 
            this.textEdit1.Location = new System.Drawing.Point(128, 49);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Size = new System.Drawing.Size(136, 21);
            this.textEdit1.TabIndex = 48;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(313, 131);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(67, 14);
            this.label9.TabIndex = 59;
            this.label9.Text = "开户城市：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(42, 131);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(67, 14);
            this.label10.TabIndex = 57;
            this.label10.Text = "开户省份：";
            this.label10.Click += new System.EventHandler(this.label10_Click);
            // 
            // oilCardProvince
            // 
            this.oilCardProvince.EnterMoveNextControl = true;
            this.oilCardProvince.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.oilCardProvince.Location = new System.Drawing.Point(128, 128);
            this.oilCardProvince.Name = "oilCardProvince";
            this.oilCardProvince.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.oilCardProvince.Properties.Items.AddRange(new object[] {
            ""});
            this.oilCardProvince.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.oilCardProvince.Size = new System.Drawing.Size(136, 21);
            this.oilCardProvince.TabIndex = 169;
            this.oilCardProvince.SelectedIndexChanged += new System.EventHandler(this.oilCardProvince_SelectedIndexChanged);
            // 
            // oilCardCity
            // 
            this.oilCardCity.EnterMoveNextControl = true;
            this.oilCardCity.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.oilCardCity.Location = new System.Drawing.Point(390, 128);
            this.oilCardCity.Name = "oilCardCity";
            this.oilCardCity.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.oilCardCity.Properties.Items.AddRange(new object[] {
            ""});
            this.oilCardCity.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.oilCardCity.Size = new System.Drawing.Size(136, 21);
            this.oilCardCity.TabIndex = 170;
            // 
            // frmBankAdjustUpt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 450);
            this.Controls.Add(this.oilCardCity);
            this.Controls.Add(this.oilCardProvince);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textEdit1);
            this.Controls.Add(this.edbankname);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textEdit17);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Name = "frmBankAdjustUpt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "修改";
            this.Load += new System.EventHandler(this.frmBankAdjustUpt_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit17.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edbankname.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.oilCardProvince.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.oilCardCity.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.TextEdit textEdit17;
        private System.Windows.Forms.Label label19;
        private DevExpress.XtraEditors.ComboBoxEdit edbankname;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.TextEdit textEdit1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private DevExpress.XtraEditors.ComboBoxEdit oilCardProvince;
        private DevExpress.XtraEditors.ComboBoxEdit oilCardCity;
    }
}