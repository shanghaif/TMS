namespace ZQTMS.UI
{
    partial class frmSendSms
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSendSms));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.edshipper = new DevExpress.XtraEditors.TextEdit();
            this.edmb = new DevExpress.XtraEditors.TextEdit();
            this.edcontent = new DevExpress.XtraEditors.MemoEdit();
            this.edbilldate = new DevExpress.XtraEditors.DateEdit();
            this.edcreateby = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.edshipper.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edmb.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edcontent.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edbilldate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edbilldate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edcreateby.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "接 收 人：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "接收号码：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 14);
            this.label3.TabIndex = 2;
            this.label3.Text = "短信内容：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 208);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 14);
            this.label4.TabIndex = 3;
            this.label4.Text = "发送日期：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(219, 208);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 14);
            this.label5.TabIndex = 3;
            this.label5.Text = "发送人：";
            // 
            // edshipper
            // 
            this.edshipper.EnterMoveNextControl = true;
            this.edshipper.Location = new System.Drawing.Point(87, 14);
            this.edshipper.Name = "edshipper";
            this.edshipper.Size = new System.Drawing.Size(268, 21);
            this.edshipper.TabIndex = 11;
            // 
            // edmb
            // 
            this.edmb.EnterMoveNextControl = true;
            this.edmb.Location = new System.Drawing.Point(87, 50);
            this.edmb.Name = "edmb";
            this.edmb.Size = new System.Drawing.Size(268, 21);
            this.edmb.TabIndex = 12;
            // 
            // edcontent
            // 
            this.edcontent.EnterMoveNextControl = true;
            this.edcontent.Location = new System.Drawing.Point(87, 88);
            this.edcontent.Name = "edcontent";
            this.edcontent.Size = new System.Drawing.Size(268, 84);
            this.edcontent.TabIndex = 13;
            this.edcontent.EditValueChanged += new System.EventHandler(this.edcontent_EditValueChanged);
            this.edcontent.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.edcontent_EditValueChanging);
            // 
            // edbilldate
            // 
            this.edbilldate.EditValue = null;
            this.edbilldate.EnterMoveNextControl = true;
            this.edbilldate.Location = new System.Drawing.Point(87, 205);
            this.edbilldate.Name = "edbilldate";
            this.edbilldate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.edbilldate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.edbilldate.Size = new System.Drawing.Size(115, 21);
            this.edbilldate.TabIndex = 14;
            // 
            // edcreateby
            // 
            this.edcreateby.EnterMoveNextControl = true;
            this.edcreateby.Location = new System.Drawing.Point(275, 205);
            this.edcreateby.Name = "edcreateby";
            this.edcreateby.Properties.ReadOnly = true;
            this.edcreateby.Size = new System.Drawing.Size(79, 21);
            this.edcreateby.TabIndex = 15;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(201, 237);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(57, 23);
            this.simpleButton1.TabIndex = 16;
            this.simpleButton1.Text = "发送";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(297, 237);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(58, 23);
            this.simpleButton2.TabIndex = 17;
            this.simpleButton2.Text = "关闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(84, 181);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(187, 15);
            this.label8.TabIndex = 28;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 183);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 14);
            this.label7.TabIndex = 27;
            this.label7.Text = "短信长度：";
            // 
            // frmSendSms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 274);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.edcreateby);
            this.Controls.Add(this.edbilldate);
            this.Controls.Add(this.edcontent);
            this.Controls.Add(this.edmb);
            this.Controls.Add(this.edshipper);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmSendSms";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "发送短信";
            this.Load += new System.EventHandler(this.sendsms_Load);
            ((System.ComponentModel.ISupportInitialize)(this.edshipper.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edmb.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edcontent.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edbilldate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edbilldate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edcreateby.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.TextEdit edshipper;
        private DevExpress.XtraEditors.TextEdit edmb;
        private DevExpress.XtraEditors.MemoEdit edcontent;
        private DevExpress.XtraEditors.DateEdit edbilldate;
        private DevExpress.XtraEditors.TextEdit edcreateby;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
    }
}