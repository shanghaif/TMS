namespace ZQTMS.UI
{
    partial class frmCauseBankInfoAdd
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
            this.edsheng = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.edcity = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.edbankchild = new DevExpress.XtraEditors.TextEdit();
            this.label12 = new System.Windows.Forms.Label();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.edouttype = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label10 = new System.Windows.Forms.Label();
            this.edopertype = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.edbankname = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.edbankcode = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.edbankman = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.edsheng.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edcity.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edbankchild.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edouttype.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edopertype.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edbankname.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edbankcode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edbankman.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // edsheng
            // 
            this.edsheng.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.edsheng.Location = new System.Drawing.Point(108, 144);
            this.edsheng.Name = "edsheng";
            this.edsheng.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.edsheng.Properties.DropDownRows = 10;
            this.edsheng.Size = new System.Drawing.Size(186, 21);
            this.edsheng.TabIndex = 949;
            this.edsheng.SelectedIndexChanged += new System.EventHandler(this.edsheng_SelectedIndexChanged);
            // 
            // edcity
            // 
            this.edcity.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.edcity.Location = new System.Drawing.Point(108, 175);
            this.edcity.Name = "edcity";
            this.edcity.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.edcity.Properties.DropDownRows = 10;
            this.edcity.Size = new System.Drawing.Size(186, 21);
            this.edcity.TabIndex = 950;
            // 
            // edbankchild
            // 
            this.edbankchild.EnterMoveNextControl = true;
            this.edbankchild.Location = new System.Drawing.Point(108, 111);
            this.edbankchild.Name = "edbankchild";
            this.edbankchild.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.edbankchild.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.edbankchild.Size = new System.Drawing.Size(186, 21);
            this.edbankchild.TabIndex = 948;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(34, 114);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(55, 14);
            this.label12.TabIndex = 947;
            this.label12.Text = "开户支行";
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(219, 286);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(75, 23);
            this.simpleButton2.TabIndex = 946;
            this.simpleButton2.Text = "取     消";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(91, 286);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 23);
            this.simpleButton1.TabIndex = 945;
            this.simpleButton1.Text = "保     存";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // edouttype
            // 
            this.edouttype.EnterMoveNextControl = true;
            this.edouttype.Location = new System.Drawing.Point(108, 242);
            this.edouttype.Name = "edouttype";
            this.edouttype.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.edouttype.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.edouttype.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.edouttype.Properties.Items.AddRange(new object[] {
            "员工",
            "客户"});
            this.edouttype.Size = new System.Drawing.Size(186, 21);
            this.edouttype.TabIndex = 943;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(34, 245);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 14);
            this.label10.TabIndex = 944;
            this.label10.Text = "支出类型";
            // 
            // edopertype
            // 
            this.edopertype.EnterMoveNextControl = true;
            this.edopertype.Location = new System.Drawing.Point(108, 208);
            this.edopertype.Name = "edopertype";
            this.edopertype.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.edopertype.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.edopertype.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.edopertype.Properties.Items.AddRange(new object[] {
            "行内转账",
            "同城跨行",
            "异地跨行"});
            this.edopertype.Size = new System.Drawing.Size(186, 21);
            this.edopertype.TabIndex = 942;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(34, 211);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 14);
            this.label6.TabIndex = 941;
            this.label6.Text = "转账类型";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(34, 178);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 14);
            this.label5.TabIndex = 940;
            this.label5.Text = "所属城市";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 147);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 14);
            this.label4.TabIndex = 939;
            this.label4.Text = "所属省份";
            // 
            // edbankname
            // 
            this.edbankname.EnterMoveNextControl = true;
            this.edbankname.Location = new System.Drawing.Point(108, 78);
            this.edbankname.Name = "edbankname";
            this.edbankname.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.edbankname.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.edbankname.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.edbankname.Properties.DropDownRows = 20;
            this.edbankname.Properties.Items.AddRange(new object[] {
            "中国工商银行",
            "中国银行",
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
            this.edbankname.Size = new System.Drawing.Size(186, 21);
            this.edbankname.TabIndex = 938;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 14);
            this.label3.TabIndex = 937;
            this.label3.Text = "开户银行";
            // 
            // edbankcode
            // 
            this.edbankcode.EnterMoveNextControl = true;
            this.edbankcode.Location = new System.Drawing.Point(108, 49);
            this.edbankcode.Name = "edbankcode";
            this.edbankcode.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.edbankcode.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.edbankcode.Size = new System.Drawing.Size(186, 21);
            this.edbankcode.TabIndex = 936;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 14);
            this.label2.TabIndex = 935;
            this.label2.Text = "银行账号";
            // 
            // edbankman
            // 
            this.edbankman.EnterMoveNextControl = true;
            this.edbankman.Location = new System.Drawing.Point(108, 19);
            this.edbankman.Name = "edbankman";
            this.edbankman.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.edbankman.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.edbankman.Size = new System.Drawing.Size(186, 21);
            this.edbankman.TabIndex = 934;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 14);
            this.label1.TabIndex = 933;
            this.label1.Text = "开户姓名";
            // 
            // frmCauseBankInfoAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(337, 339);
            this.Controls.Add(this.edsheng);
            this.Controls.Add(this.edcity);
            this.Controls.Add(this.edbankchild);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.edouttype);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.edopertype);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.edbankname);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.edbankcode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.edbankman);
            this.Controls.Add(this.label1);
            this.Name = "frmCauseBankInfoAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "事业部银行客户资料";
            this.Load += new System.EventHandler(this.frmCauseBankInfoAdd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.edsheng.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edcity.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edbankchild.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edouttype.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edopertype.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edbankname.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edbankcode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edbankman.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ImageComboBoxEdit edsheng;
        private DevExpress.XtraEditors.ImageComboBoxEdit edcity;
        private DevExpress.XtraEditors.TextEdit edbankchild;
        private System.Windows.Forms.Label label12;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.ComboBoxEdit edouttype;
        private System.Windows.Forms.Label label10;
        private DevExpress.XtraEditors.ComboBoxEdit edopertype;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.ComboBoxEdit edbankname;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.TextEdit edbankcode;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.TextEdit edbankman;
        private System.Windows.Forms.Label label1;
    }
}