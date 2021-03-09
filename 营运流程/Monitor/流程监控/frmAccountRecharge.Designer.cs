namespace ZQTMS.UI
{
    partial class frmAccountRecharge
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAccountRecharge));
            this.btnRecharge = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.radio_TFTpay = new System.Windows.Forms.RadioButton();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBalance = new System.Windows.Forms.Label();
            this.txtAccountNo = new System.Windows.Forms.Label();
            this.txtAccountName = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPhone = new DevExpress.XtraEditors.TextEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRechargeNum = new DevExpress.XtraEditors.SpinEdit();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.radio_Unionpay = new System.Windows.Forms.RadioButton();
            this.label13 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.radio_LLpay = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtPhone.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRechargeNum.Properties)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRecharge
            // 
            this.btnRecharge.Location = new System.Drawing.Point(39, 284);
            this.btnRecharge.Name = "btnRecharge";
            this.btnRecharge.Size = new System.Drawing.Size(75, 25);
            this.btnRecharge.TabIndex = 6;
            this.btnRecharge.Text = "充 值";
            this.btnRecharge.Click += new System.EventHandler(this.btnRecharge_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(204, 284);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "取 消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // radio_TFTpay
            // 
            this.radio_TFTpay.AutoSize = true;
            this.radio_TFTpay.Location = new System.Drawing.Point(222, 29);
            this.radio_TFTpay.Margin = new System.Windows.Forms.Padding(0);
            this.radio_TFTpay.Name = "radio_TFTpay";
            this.radio_TFTpay.Size = new System.Drawing.Size(61, 18);
            this.radio_TFTpay.TabIndex = 38;
            this.radio_TFTpay.Text = "腾付通";
            this.radio_TFTpay.UseVisualStyleBackColor = true;
            this.radio_TFTpay.Visible = false;
            this.radio_TFTpay.CheckedChanged += new System.EventHandler(this.radio_TFTpay_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(238, 249);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 14);
            this.label9.TabIndex = 36;
            this.label9.Text = "0.0000 元";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(103, 249);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(62, 14);
            this.label8.TabIndex = 35;
            this.label8.Text = "0.0000 元";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(177, 249);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 14);
            this.label7.TabIndex = 34;
            this.label7.Text = "实充金额：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(39, 249);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 14);
            this.label5.TabIndex = 33;
            this.label5.Text = "手续费：";
            // 
            // txtBalance
            // 
            this.txtBalance.AutoSize = true;
            this.txtBalance.Location = new System.Drawing.Point(129, 152);
            this.txtBalance.Name = "txtBalance";
            this.txtBalance.Size = new System.Drawing.Size(19, 14);
            this.txtBalance.TabIndex = 32;
            this.txtBalance.Text = "无";
            // 
            // txtAccountNo
            // 
            this.txtAccountNo.AutoSize = true;
            this.txtAccountNo.Location = new System.Drawing.Point(129, 123);
            this.txtAccountNo.Name = "txtAccountNo";
            this.txtAccountNo.Size = new System.Drawing.Size(19, 14);
            this.txtAccountNo.TabIndex = 31;
            this.txtAccountNo.Text = "无";
            // 
            // txtAccountName
            // 
            this.txtAccountName.AutoSize = true;
            this.txtAccountName.Location = new System.Drawing.Point(129, 94);
            this.txtAccountName.Name = "txtAccountName";
            this.txtAccountName.Size = new System.Drawing.Size(19, 14);
            this.txtAccountName.TabIndex = 30;
            this.txtAccountName.Text = "无";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 156);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 14);
            this.label6.TabIndex = 29;
            this.label6.Text = " 账户余额：";
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(109, 217);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(194, 21);
            this.txtPhone.TabIndex = 28;
            this.txtPhone.EditValueChanged += new System.EventHandler(this.txtPhone_EditValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 218);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 14);
            this.label4.TabIndex = 27;
            this.label4.Text = "手机号码：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 187);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 14);
            this.label3.TabIndex = 25;
            this.label3.Text = "充值金额：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 125);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 14);
            this.label2.TabIndex = 24;
            this.label2.Text = "账户编号：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(27, 67);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(67, 14);
            this.label10.TabIndex = 22;
            this.label10.Text = "支付方式：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 14);
            this.label1.TabIndex = 23;
            this.label1.Text = "账户名称：";
            // 
            // txtRechargeNum
            // 
            this.txtRechargeNum.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtRechargeNum.Location = new System.Drawing.Point(109, 181);
            this.txtRechargeNum.Name = "txtRechargeNum";
            this.txtRechargeNum.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtRechargeNum.Properties.Mask.EditMask = "n";
            this.txtRechargeNum.Properties.MaxLength = 8;
            this.txtRechargeNum.Size = new System.Drawing.Size(194, 21);
            this.txtRechargeNum.TabIndex = 26;
            this.txtRechargeNum.TabStop = false;
            this.txtRechargeNum.EditValueChanged += new System.EventHandler(this.txtRechargeNum_EditValueChanged);
            this.txtRechargeNum.TextChanged += new System.EventHandler(this.txtRechargeNum_TextChanged);
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.label12.ForeColor = System.Drawing.Color.Red;
            this.label12.Location = new System.Drawing.Point(29, 3);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(276, 14);
            this.label12.TabIndex = 42;
            this.label12.Text = "\"\"";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.label11.ForeColor = System.Drawing.Color.Red;
            this.label11.Location = new System.Drawing.Point(29, 2);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(276, 15);
            this.label11.TabIndex = 42;
            this.label11.Text = "手续费：网银借记卡信用卡0.15%，企业网银8元一笔";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radio_Unionpay
            // 
            this.radio_Unionpay.AutoSize = true;
            this.radio_Unionpay.Location = new System.Drawing.Point(158, 29);
            this.radio_Unionpay.Margin = new System.Windows.Forms.Padding(0);
            this.radio_Unionpay.Name = "radio_Unionpay";
            this.radio_Unionpay.Size = new System.Drawing.Size(49, 18);
            this.radio_Unionpay.TabIndex = 46;
            this.radio_Unionpay.Text = "银联";
            this.radio_Unionpay.UseVisualStyleBackColor = true;
            this.radio_Unionpay.CheckedChanged += new System.EventHandler(this.radio_Unionpay_CheckedChanged);
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.label13.ForeColor = System.Drawing.Color.Red;
            this.label13.Location = new System.Drawing.Point(29, 2);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(275, 14);
            this.label13.TabIndex = 47;
            this.label13.Text = "“”";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label13.Visible = false;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "支付宝扫码",
            "微信扫码",
            "转账充值"});
            this.comboBox1.Location = new System.Drawing.Point(109, 64);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(194, 22);
            this.comboBox1.TabIndex = 45;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label18);
            this.panel1.Controls.Add(this.label17);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Location = new System.Drawing.Point(410, 48);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(287, 225);
            this.panel1.TabIndex = 48;
            this.panel1.Visible = false;
            // 
            // label18
            // 
            this.label18.Font = new System.Drawing.Font("微软雅黑", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.Red;
            this.label18.Location = new System.Drawing.Point(13, 152);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(271, 51);
            this.label18.TabIndex = 49;
            this.label18.Text = "注：未加入白名单前转账无法自动充值到系统账户，\r\n       需线下找财务确认充值。\r\n       转账充值不支持微信、支付宝使用银行卡转账";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(4, 125);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(134, 14);
            this.label17.TabIndex = 50;
            this.label17.Text = "3、系统自动完成充值。";
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(4, 93);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(280, 32);
            this.label16.TabIndex = 51;
            this.label16.Text = "2、向中强账户转账，银行名称：深圳中强物流有\r\n     限公司，卡号：95588 531 0000 571 3205；";
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(3, 48);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(281, 48);
            this.label15.TabIndex = 52;
            this.label15.Text = "1、在系统菜单\"账户管理\"—\"转账充值白名单\"下添\r\n     加转账账户信息，系统自动添加到银联白名单。\r\n    （认真核对卡号）";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(3, 17);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(139, 14);
            this.label14.TabIndex = 53;
            this.label14.Text = "银联转账自动充值步骤：";
            // 
            // radio_LLpay
            // 
            this.radio_LLpay.AutoSize = true;
            this.radio_LLpay.Checked = true;
            this.radio_LLpay.Location = new System.Drawing.Point(87, 29);
            this.radio_LLpay.Margin = new System.Windows.Forms.Padding(0);
            this.radio_LLpay.Name = "radio_LLpay";
            this.radio_LLpay.Size = new System.Drawing.Size(49, 18);
            this.radio_LLpay.TabIndex = 49;
            this.radio_LLpay.TabStop = true;
            this.radio_LLpay.Text = "连连\r\n";
            this.radio_LLpay.UseVisualStyleBackColor = true;
            // 
            // frmAccountRecharge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(768, 331);
            this.Controls.Add(this.radio_LLpay);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.radio_Unionpay);
            this.Controls.Add(this.radio_TFTpay);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtBalance);
            this.Controls.Add(this.txtAccountNo);
            this.Controls.Add(this.txtAccountName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRecharge);
            this.Controls.Add(this.txtRechargeNum);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAccountRecharge";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "账户充值";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmAccountRecharge_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtPhone.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRechargeNum.Properties)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton btnRecharge;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private System.Windows.Forms.RadioButton radio_TFTpay;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label txtBalance;
        private System.Windows.Forms.Label txtAccountNo;
        private System.Windows.Forms.Label txtAccountName;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.TextEdit txtPhone;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SpinEdit txtRechargeNum;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.RadioButton radio_Unionpay;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.RadioButton radio_LLpay;
    }
}