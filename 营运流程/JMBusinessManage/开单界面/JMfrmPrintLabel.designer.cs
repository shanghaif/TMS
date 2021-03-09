namespace ZQTMS.UI
{
    partial class JMfrmPrintLabel
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.label1 = new System.Windows.Forms.Label();
            this.spinEdit1 = new DevExpress.XtraEditors.SpinEdit();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.label2 = new System.Windows.Forms.Label();
            this.spinEdit2 = new DevExpress.XtraEditors.SpinEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.lblbillno = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblproduct = new System.Windows.Forms.Label();
            this.lblconsignee = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.edprinters = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edprinters.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(168, 212);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(80, 28);
            this.simpleButton1.TabIndex = 906;
            this.simpleButton1.Text = "打印";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 14);
            this.label1.TabIndex = 907;
            this.label1.Text = "指定打印张数";
            // 
            // spinEdit1
            // 
            this.spinEdit1.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinEdit1.Location = new System.Drawing.Point(27, 117);
            this.spinEdit1.Name = "spinEdit1";
            this.spinEdit1.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spinEdit1.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.spinEdit1.Properties.Appearance.Options.UseFont = true;
            this.spinEdit1.Properties.Appearance.Options.UseForeColor = true;
            this.spinEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.spinEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinEdit1.Properties.Mask.EditMask = "n0";
            this.spinEdit1.Size = new System.Drawing.Size(126, 35);
            this.spinEdit1.TabIndex = 908;
            this.spinEdit1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.spinEdit1_KeyDown);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(267, 212);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(67, 30);
            this.simpleButton2.TabIndex = 909;
            this.simpleButton2.Text = "关闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(167, 126);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 14);
            this.label2.TabIndex = 910;
            this.label2.Text = "从第";
            // 
            // spinEdit2
            // 
            this.spinEdit2.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinEdit2.Location = new System.Drawing.Point(208, 121);
            this.spinEdit2.Name = "spinEdit2";
            this.spinEdit2.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.spinEdit2.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.spinEdit2.Properties.Appearance.Options.UseFont = true;
            this.spinEdit2.Properties.Appearance.Options.UseForeColor = true;
            this.spinEdit2.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.spinEdit2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinEdit2.Properties.Mask.EditMask = "n0";
            this.spinEdit2.Size = new System.Drawing.Size(66, 29);
            this.spinEdit2.TabIndex = 911;
            this.spinEdit2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.spinEdit2_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(279, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 14);
            this.label3.TabIndex = 912;
            this.label3.Text = "张打印起";
            // 
            // lblbillno
            // 
            this.lblbillno.AutoSize = true;
            this.lblbillno.ForeColor = System.Drawing.Color.Blue;
            this.lblbillno.Location = new System.Drawing.Point(66, 22);
            this.lblbillno.Name = "lblbillno";
            this.lblbillno.Size = new System.Drawing.Size(38, 14);
            this.lblbillno.TabIndex = 915;
            this.lblbillno.Text = "label4";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 14);
            this.label7.TabIndex = 913;
            this.label7.Text = "单号：";
            // 
            // lblproduct
            // 
            this.lblproduct.AutoSize = true;
            this.lblproduct.ForeColor = System.Drawing.Color.Blue;
            this.lblproduct.Location = new System.Drawing.Point(66, 56);
            this.lblproduct.Name = "lblproduct";
            this.lblproduct.Size = new System.Drawing.Size(38, 14);
            this.lblproduct.TabIndex = 919;
            this.lblproduct.Text = "label4";
            // 
            // lblconsignee
            // 
            this.lblconsignee.AutoSize = true;
            this.lblconsignee.ForeColor = System.Drawing.Color.Blue;
            this.lblconsignee.Location = new System.Drawing.Point(243, 22);
            this.lblconsignee.Name = "lblconsignee";
            this.lblconsignee.Size = new System.Drawing.Size(38, 14);
            this.lblconsignee.TabIndex = 920;
            this.lblconsignee.Text = "label4";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(17, 56);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(43, 14);
            this.label8.TabIndex = 918;
            this.label8.Text = "品名：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(184, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 14);
            this.label9.TabIndex = 917;
            this.label9.Text = "收货人：";
            // 
            // edprinters
            // 
            this.edprinters.EditValue = "";
            this.edprinters.EnterMoveNextControl = true;
            this.edprinters.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.edprinters.Location = new System.Drawing.Point(67, 172);
            this.edprinters.Name = "edprinters";
            this.edprinters.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.edprinters.Properties.Appearance.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.edprinters.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.edprinters.Properties.Appearance.Options.UseBackColor = true;
            this.edprinters.Properties.Appearance.Options.UseFont = true;
            this.edprinters.Properties.Appearance.Options.UseForeColor = true;
            this.edprinters.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow;
            this.edprinters.Properties.AppearanceFocused.BackColor2 = System.Drawing.Color.White;
            this.edprinters.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.edprinters.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            serializableAppearanceObject2.BackColor = System.Drawing.Color.WhiteSmoke;
            serializableAppearanceObject2.ForeColor = System.Drawing.Color.Teal;
            serializableAppearanceObject2.Options.UseBackColor = true;
            serializableAppearanceObject2.Options.UseForeColor = true;
            this.edprinters.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, false)});
            this.edprinters.Properties.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.edprinters.Properties.Sorted = true;
            this.edprinters.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.edprinters.Size = new System.Drawing.Size(267, 22);
            this.edprinters.TabIndex = 922;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 175);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 14);
            this.label4.TabIndex = 921;
            this.label4.Text = "打印机：";
            // 
            // JMfrmPrintLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 254);
            this.Controls.Add(this.edprinters);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblproduct);
            this.Controls.Add(this.lblconsignee);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lblbillno);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.spinEdit2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.spinEdit1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.simpleButton1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "JMfrmPrintLabel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "打印标签";
            this.Load += new System.EventHandler(this.w_print_lavel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edprinters.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SpinEdit spinEdit1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.SpinEdit spinEdit2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblbillno;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblproduct;
        private System.Windows.Forms.Label lblconsignee;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private DevExpress.XtraEditors.ComboBoxEdit edprinters;
        private System.Windows.Forms.Label label4;
    }
}