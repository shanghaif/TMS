namespace ZQTMS.UI
{
    partial class frmHandFeeAdd_KT
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
            this.serchBillNo = new DevExpress.XtraEditors.TextEdit();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.Amount = new DevExpress.XtraEditors.TextEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnSearch = new DevExpress.XtraEditors.SimpleButton();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.label6 = new System.Windows.Forms.Label();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.ExceDepart = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.FeeType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.DischargerPhone = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.Discharger = new DevExpress.XtraEditors.TextEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.ReMark = new DevExpress.XtraEditors.MemoEdit();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.CarrNO = new DevExpress.XtraEditors.TextEdit();
            this.label53 = new System.Windows.Forms.Label();
            this.ActVolume = new DevExpress.XtraEditors.TextEdit();
            this.label28 = new System.Windows.Forms.Label();
            this.DriverName = new DevExpress.XtraEditors.TextEdit();
            this.DriverPhone = new DevExpress.XtraEditors.TextEdit();
            this.ActWeight = new DevExpress.XtraEditors.TextEdit();
            this.label29 = new System.Windows.Forms.Label();
            this.CarNO = new DevExpress.XtraEditors.TextEdit();
            this.DepartureBatch = new DevExpress.XtraEditors.TextEdit();
            this.label55 = new System.Windows.Forms.Label();
            this.label56 = new System.Windows.Forms.Label();
            this.label57 = new System.Windows.Forms.Label();
            this.label58 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.serchBillNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Amount.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ExceDepart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FeeType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DischargerPhone.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Discharger.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReMark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CarrNO.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ActVolume.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DriverName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DriverPhone.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ActWeight.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CarNO.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DepartureBatch.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "输入批次号：";
            // 
            // serchBillNo
            // 
            this.serchBillNo.Location = new System.Drawing.Point(113, 14);
            this.serchBillNo.Name = "serchBillNo";
            this.serchBillNo.Size = new System.Drawing.Size(138, 21);
            this.serchBillNo.TabIndex = 8;
            this.serchBillNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.edunit_KeyDown);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(67, 293);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(151, 293);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // Amount
            // 
            this.Amount.Location = new System.Drawing.Point(90, 149);
            this.Amount.Name = "Amount";
            this.Amount.Size = new System.Drawing.Size(136, 21);
            this.Amount.TabIndex = 118;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 14);
            this.label4.TabIndex = 18;
            this.label4.Text = "装卸费：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(44, 188);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 14);
            this.label7.TabIndex = 23;
            this.label7.Text = "备注：";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(281, 12);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 25;
            this.btnSearch.Text = "提取";
            this.btnSearch.Click += new System.EventHandler(this.simpleButton4_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(371, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 26;
            this.btnClose.Text = "退出";
            this.btnClose.Click += new System.EventHandler(this.simpleButton5_Click);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.label6.Location = new System.Drawing.Point(12, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(653, 2);
            this.label6.TabIndex = 27;
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.ExceDepart);
            this.groupControl2.Controls.Add(this.label8);
            this.groupControl2.Controls.Add(this.FeeType);
            this.groupControl2.Controls.Add(this.label5);
            this.groupControl2.Controls.Add(this.DischargerPhone);
            this.groupControl2.Controls.Add(this.label2);
            this.groupControl2.Controls.Add(this.Discharger);
            this.groupControl2.Controls.Add(this.label3);
            this.groupControl2.Controls.Add(this.btnSave);
            this.groupControl2.Controls.Add(this.btnCancel);
            this.groupControl2.Controls.Add(this.label4);
            this.groupControl2.Controls.Add(this.label7);
            this.groupControl2.Controls.Add(this.Amount);
            this.groupControl2.Controls.Add(this.ReMark);
            this.groupControl2.Location = new System.Drawing.Point(304, 48);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(265, 330);
            this.groupControl2.TabIndex = 29;
            this.groupControl2.Text = "增加费用项目";
            // 
            // ExceDepart
            // 
            this.ExceDepart.EnterMoveNextControl = true;
            this.ExceDepart.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.ExceDepart.Location = new System.Drawing.Point(90, 64);
            this.ExceDepart.Name = "ExceDepart";
            this.ExceDepart.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ExceDepart.Properties.Items.AddRange(new object[] {
            "装卸费-短驳出库",
            "装卸费-短驳到货",
            "装卸费-大车",
            "装卸费-大车到货",
            "装卸费-二级到货",
            "装卸费-转二级"});
            this.ExceDepart.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.ExceDepart.Size = new System.Drawing.Size(136, 21);
            this.ExceDepart.TabIndex = 116;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(23, 68);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 14);
            this.label8.TabIndex = 117;
            this.label8.Text = "承担部门：";
            // 
            // FeeType
            // 
            this.FeeType.EnterMoveNextControl = true;
            this.FeeType.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.FeeType.Location = new System.Drawing.Point(90, 36);
            this.FeeType.Name = "FeeType";
            this.FeeType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FeeType.Properties.Items.AddRange(new object[] {
            "装卸费-短驳出库",
            "装卸费-短驳到货",
            "装卸费-大车",
            "装卸费-大车到货",
            "装卸费-二级到货",
            "装卸费-转二级"});
            this.FeeType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.FeeType.Size = new System.Drawing.Size(136, 21);
            this.FeeType.TabIndex = 114;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 14);
            this.label5.TabIndex = 115;
            this.label5.Text = "费用类型：";
            // 
            // DischargerPhone
            // 
            this.DischargerPhone.EnterMoveNextControl = true;
            this.DischargerPhone.Location = new System.Drawing.Point(90, 121);
            this.DischargerPhone.Name = "DischargerPhone";
            this.DischargerPhone.Properties.Mask.EditMask = "f0";
            this.DischargerPhone.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.DischargerPhone.Size = new System.Drawing.Size(136, 21);
            this.DischargerPhone.TabIndex = 36;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 14);
            this.label2.TabIndex = 35;
            this.label2.Text = "卸货人电话：";
            // 
            // Discharger
            // 
            this.Discharger.EnterMoveNextControl = true;
            this.Discharger.Location = new System.Drawing.Point(90, 92);
            this.Discharger.Name = "Discharger";
            this.Discharger.Size = new System.Drawing.Size(136, 21);
            this.Discharger.TabIndex = 34;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 14);
            this.label3.TabIndex = 33;
            this.label3.Text = "卸货人：";
            // 
            // ReMark
            // 
            this.ReMark.Location = new System.Drawing.Point(90, 185);
            this.ReMark.Name = "ReMark";
            this.ReMark.Size = new System.Drawing.Size(136, 87);
            this.ReMark.TabIndex = 21;
            this.ReMark.TabStop = false;
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.CarrNO);
            this.groupControl3.Controls.Add(this.label53);
            this.groupControl3.Controls.Add(this.ActVolume);
            this.groupControl3.Controls.Add(this.label28);
            this.groupControl3.Controls.Add(this.DriverName);
            this.groupControl3.Controls.Add(this.DriverPhone);
            this.groupControl3.Controls.Add(this.ActWeight);
            this.groupControl3.Controls.Add(this.label29);
            this.groupControl3.Controls.Add(this.CarNO);
            this.groupControl3.Controls.Add(this.DepartureBatch);
            this.groupControl3.Controls.Add(this.label55);
            this.groupControl3.Controls.Add(this.label56);
            this.groupControl3.Controls.Add(this.label57);
            this.groupControl3.Controls.Add(this.label58);
            this.groupControl3.Location = new System.Drawing.Point(11, 48);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(291, 330);
            this.groupControl3.TabIndex = 89;
            this.groupControl3.Text = "车辆信息";
            // 
            // CarrNO
            // 
            this.CarrNO.EnterMoveNextControl = true;
            this.CarrNO.Location = new System.Drawing.Point(76, 92);
            this.CarrNO.Name = "CarrNO";
            this.CarrNO.Properties.ReadOnly = true;
            this.CarrNO.Size = new System.Drawing.Size(141, 21);
            this.CarrNO.TabIndex = 100;
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.Location = new System.Drawing.Point(7, 94);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(63, 14);
            this.label53.TabIndex = 101;
            this.label53.Text = "车 厢 号：";
            // 
            // ActVolume
            // 
            this.ActVolume.EditValue = "0";
            this.ActVolume.EnterMoveNextControl = true;
            this.ActVolume.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.ActVolume.Location = new System.Drawing.Point(76, 234);
            this.ActVolume.Name = "ActVolume";
            this.ActVolume.Properties.ReadOnly = true;
            this.ActVolume.Size = new System.Drawing.Size(141, 21);
            this.ActVolume.TabIndex = 91;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(11, 238);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(67, 14);
            this.label28.TabIndex = 90;
            this.label28.Text = "实际容积：";
            // 
            // DriverName
            // 
            this.DriverName.EnterMoveNextControl = true;
            this.DriverName.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.DriverName.Location = new System.Drawing.Point(76, 120);
            this.DriverName.Name = "DriverName";
            this.DriverName.Properties.ReadOnly = true;
            this.DriverName.Size = new System.Drawing.Size(141, 21);
            this.DriverName.TabIndex = 76;
            // 
            // DriverPhone
            // 
            this.DriverPhone.EnterMoveNextControl = true;
            this.DriverPhone.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.DriverPhone.Location = new System.Drawing.Point(76, 147);
            this.DriverPhone.Name = "DriverPhone";
            this.DriverPhone.Properties.ReadOnly = true;
            this.DriverPhone.Size = new System.Drawing.Size(141, 21);
            this.DriverPhone.TabIndex = 77;
            // 
            // ActWeight
            // 
            this.ActWeight.EditValue = "0";
            this.ActWeight.EnterMoveNextControl = true;
            this.ActWeight.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.ActWeight.Location = new System.Drawing.Point(76, 191);
            this.ActWeight.Name = "ActWeight";
            this.ActWeight.Properties.ReadOnly = true;
            this.ActWeight.Size = new System.Drawing.Size(141, 21);
            this.ActWeight.TabIndex = 89;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(11, 195);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(67, 14);
            this.label29.TabIndex = 88;
            this.label29.Text = "实际载重：";
            // 
            // CarNO
            // 
            this.CarNO.EnterMoveNextControl = true;
            this.CarNO.Location = new System.Drawing.Point(76, 64);
            this.CarNO.Name = "CarNO";
            this.CarNO.Properties.ReadOnly = true;
            this.CarNO.Size = new System.Drawing.Size(141, 21);
            this.CarNO.TabIndex = 75;
            // 
            // DepartureBatch
            // 
            this.DepartureBatch.EnterMoveNextControl = true;
            this.DepartureBatch.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.DepartureBatch.Location = new System.Drawing.Point(76, 33);
            this.DepartureBatch.Name = "DepartureBatch";
            this.DepartureBatch.Properties.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.DepartureBatch.Properties.Appearance.Options.UseBackColor = true;
            this.DepartureBatch.Properties.ReadOnly = true;
            this.DepartureBatch.Size = new System.Drawing.Size(141, 21);
            this.DepartureBatch.TabIndex = 78;
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Location = new System.Drawing.Point(7, 67);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(63, 14);
            this.label55.TabIndex = 80;
            this.label55.Text = "车 牌 号：";
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.Location = new System.Drawing.Point(9, 122);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(67, 14);
            this.label56.TabIndex = 81;
            this.label56.Text = "司机姓名：";
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.Location = new System.Drawing.Point(9, 151);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(67, 14);
            this.label57.TabIndex = 82;
            this.label57.Text = "联系电话：";
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.Location = new System.Drawing.Point(9, 37);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(67, 14);
            this.label58.TabIndex = 83;
            this.label58.Text = "发车批次：";
            // 
            // frmHandFeeAdd_KT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 383);
            this.Controls.Add(this.groupControl3);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.serchBillNo);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmHandFeeAdd_KT";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "整车装卸费登记";
            this.Load += new System.EventHandler(this.frmOtherFeeAdd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.serchBillNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Amount.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ExceDepart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FeeType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DischargerPhone.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Discharger.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReMark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            this.groupControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CarrNO.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ActVolume.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DriverName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DriverPhone.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ActWeight.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CarNO.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DepartureBatch.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit serchBillNo;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.TextEdit Amount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.SimpleButton btnSearch;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.MemoEdit ReMark;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.TextEdit CarrNO;
        private System.Windows.Forms.Label label53;
        private DevExpress.XtraEditors.TextEdit ActVolume;
        private System.Windows.Forms.Label label28;
        private DevExpress.XtraEditors.TextEdit DriverName;
        private DevExpress.XtraEditors.TextEdit DriverPhone;
        private DevExpress.XtraEditors.TextEdit ActWeight;
        private System.Windows.Forms.Label label29;
        private DevExpress.XtraEditors.TextEdit CarNO;
        private DevExpress.XtraEditors.TextEdit DepartureBatch;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.Label label58;
        private DevExpress.XtraEditors.TextEdit DischargerPhone;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.TextEdit Discharger;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.ComboBoxEdit FeeType;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.ComboBoxEdit ExceDepart;
        private System.Windows.Forms.Label label8;
    }
}