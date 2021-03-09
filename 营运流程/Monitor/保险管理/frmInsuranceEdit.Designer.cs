namespace ZQTMS.UI
{
    partial class frmInsuranceEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInsuranceEdit));
            this.txtRequestURL = new DevExpress.XtraEditors.TextEdit();
            this.txtQueryURL = new DevExpress.XtraEditors.TextEdit();
            this.label20 = new System.Windows.Forms.Label();
            this.txtBillNO = new DevExpress.XtraEditors.TextEdit();
            this.label18 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.dLastSentTime = new DevExpress.XtraEditors.DateEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCount = new DevExpress.XtraEditors.TextEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCode = new DevExpress.XtraEditors.TextEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.txtGUID = new DevExpress.XtraEditors.TextEdit();
            this.dCreateTime = new DevExpress.XtraEditors.DateEdit();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.txtRequestJson = new DevExpress.XtraEditors.MemoEdit();
            this.dSendTime = new DevExpress.XtraEditors.DateEdit();
            this.cbbResponseState = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cbbState = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txtResponseMsg = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.txtRequestURL.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQueryURL.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillNO.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dLastSentTime.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dLastSentTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCount.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtGUID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dCreateTime.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dCreateTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRequestJson.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dSendTime.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dSendTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbResponseState.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbState.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtResponseMsg.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtRequestURL
            // 
            this.txtRequestURL.Location = new System.Drawing.Point(160, 87);
            this.txtRequestURL.Name = "txtRequestURL";
            this.txtRequestURL.Size = new System.Drawing.Size(158, 21);
            this.txtRequestURL.TabIndex = 29;
            // 
            // txtQueryURL
            // 
            this.txtQueryURL.Location = new System.Drawing.Point(499, 87);
            this.txtQueryURL.Name = "txtQueryURL";
            this.txtQueryURL.Size = new System.Drawing.Size(158, 21);
            this.txtQueryURL.TabIndex = 24;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(73, 90);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(64, 14);
            this.label20.TabIndex = 20;
            this.label20.Text = "请求URL：";
            // 
            // txtBillNO
            // 
            this.txtBillNO.Location = new System.Drawing.Point(160, 31);
            this.txtBillNO.Name = "txtBillNO";
            this.txtBillNO.Size = new System.Drawing.Size(158, 21);
            this.txtBillNO.TabIndex = 19;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(76, 209);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(91, 14);
            this.label18.TabIndex = 17;
            this.label18.Text = "请求返回消息：";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(400, 90);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(88, 14);
            this.label16.TabIndex = 15;
            this.label16.Text = "投保查询URL：";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(400, 117);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(95, 14);
            this.label15.TabIndex = 14;
            this.label15.Text = "请求返回GUID：";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(399, 38);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(67, 14);
            this.label11.TabIndex = 10;
            this.label11.Text = "请求状态：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(72, 59);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(67, 14);
            this.label10.TabIndex = 9;
            this.label10.Text = "请求时间：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(74, 116);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(91, 14);
            this.label8.TabIndex = 7;
            this.label8.Text = "请求返回状态：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(74, 236);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 14);
            this.label4.TabIndex = 3;
            this.label4.Text = "请求Json参数：";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.dLastSentTime);
            this.panelControl1.Controls.Add(this.label6);
            this.panelControl1.Controls.Add(this.txtCount);
            this.panelControl1.Controls.Add(this.label5);
            this.panelControl1.Controls.Add(this.txtCode);
            this.panelControl1.Controls.Add(this.label3);
            this.panelControl1.Controls.Add(this.txtGUID);
            this.panelControl1.Controls.Add(this.dCreateTime);
            this.panelControl1.Controls.Add(this.btnClose);
            this.panelControl1.Controls.Add(this.btnSave);
            this.panelControl1.Controls.Add(this.txtRequestJson);
            this.panelControl1.Controls.Add(this.dSendTime);
            this.panelControl1.Controls.Add(this.cbbResponseState);
            this.panelControl1.Controls.Add(this.cbbState);
            this.panelControl1.Controls.Add(this.txtResponseMsg);
            this.panelControl1.Controls.Add(this.txtRequestURL);
            this.panelControl1.Controls.Add(this.txtQueryURL);
            this.panelControl1.Controls.Add(this.label20);
            this.panelControl1.Controls.Add(this.txtBillNO);
            this.panelControl1.Controls.Add(this.label18);
            this.panelControl1.Controls.Add(this.label16);
            this.panelControl1.Controls.Add(this.label15);
            this.panelControl1.Controls.Add(this.label11);
            this.panelControl1.Controls.Add(this.label10);
            this.panelControl1.Controls.Add(this.label8);
            this.panelControl1.Controls.Add(this.label4);
            this.panelControl1.Controls.Add(this.label2);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(698, 521);
            this.panelControl1.TabIndex = 1;
            // 
            // dLastSentTime
            // 
            this.dLastSentTime.EditValue = new System.DateTime(2006, 1, 26, 0, 0, 0, 0);
            this.dLastSentTime.Location = new System.Drawing.Point(160, 173);
            this.dLastSentTime.Name = "dLastSentTime";
            this.dLastSentTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dLastSentTime.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.dLastSentTime.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dLastSentTime.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.dLastSentTime.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dLastSentTime.Properties.Mask.EditMask = "yyyy-MM-dd HH:mm";
            this.dLastSentTime.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dLastSentTime.Size = new System.Drawing.Size(159, 21);
            this.dLastSentTime.TabIndex = 54;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(72, 176);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 14);
            this.label6.TabIndex = 53;
            this.label6.Text = "最后请求时间：";
            // 
            // txtCount
            // 
            this.txtCount.Location = new System.Drawing.Point(160, 142);
            this.txtCount.Name = "txtCount";
            this.txtCount.Size = new System.Drawing.Size(158, 21);
            this.txtCount.TabIndex = 52;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(75, 143);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 14);
            this.label5.TabIndex = 51;
            this.label5.Text = "请求次数：";
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(499, 140);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(158, 21);
            this.txtCode.TabIndex = 50;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(400, 143);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 14);
            this.label3.TabIndex = 49;
            this.label3.Text = "流水号：";
            // 
            // txtGUID
            // 
            this.txtGUID.Location = new System.Drawing.Point(498, 113);
            this.txtGUID.Name = "txtGUID";
            this.txtGUID.Size = new System.Drawing.Size(158, 21);
            this.txtGUID.TabIndex = 48;
            // 
            // dCreateTime
            // 
            this.dCreateTime.EditValue = new System.DateTime(2006, 1, 26, 0, 0, 0, 0);
            this.dCreateTime.Location = new System.Drawing.Point(498, 60);
            this.dCreateTime.Name = "dCreateTime";
            this.dCreateTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dCreateTime.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.dCreateTime.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dCreateTime.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.dCreateTime.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dCreateTime.Properties.Mask.EditMask = "yyyy-MM-dd HH:mm";
            this.dCreateTime.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dCreateTime.Size = new System.Drawing.Size(159, 21);
            this.dCreateTime.TabIndex = 47;
            // 
            // btnClose
            // 
            this.btnClose.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Appearance.Options.UseFont = true;
            this.btnClose.Image = global::ZQTMS.UI.Properties.Resources.Action_Close;
            this.btnClose.Location = new System.Drawing.Point(523, 496);
            this.btnClose.Name = "btnClose";
            this.btnClose.ShowToolTips = false;
            this.btnClose.Size = new System.Drawing.Size(58, 23);
            this.btnClose.TabIndex = 46;
            this.btnClose.Text = "关闭";
            this.btnClose.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Hand;
            this.btnClose.ToolTipTitle = "帮助";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Appearance.Options.UseFont = true;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(601, 496);
            this.btnSave.Name = "btnSave";
            this.btnSave.ShowToolTips = false;
            this.btnSave.Size = new System.Drawing.Size(58, 23);
            this.btnSave.TabIndex = 45;
            this.btnSave.Text = "修改";
            this.btnSave.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Hand;
            this.btnSave.ToolTipTitle = "帮助";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtRequestJson
            // 
            this.txtRequestJson.Location = new System.Drawing.Point(160, 233);
            this.txtRequestJson.Name = "txtRequestJson";
            this.txtRequestJson.Size = new System.Drawing.Size(499, 224);
            this.txtRequestJson.TabIndex = 42;
            // 
            // dSendTime
            // 
            this.dSendTime.EditValue = new System.DateTime(2006, 1, 26, 0, 0, 0, 0);
            this.dSendTime.Location = new System.Drawing.Point(159, 60);
            this.dSendTime.Name = "dSendTime";
            this.dSendTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dSendTime.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.dSendTime.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dSendTime.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.dSendTime.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dSendTime.Properties.Mask.EditMask = "yyyy-MM-dd HH:mm";
            this.dSendTime.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dSendTime.Size = new System.Drawing.Size(159, 21);
            this.dSendTime.TabIndex = 37;
            // 
            // cbbResponseState
            // 
            this.cbbResponseState.Enabled = false;
            this.cbbResponseState.Location = new System.Drawing.Point(160, 114);
            this.cbbResponseState.Name = "cbbResponseState";
            this.cbbResponseState.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbbResponseState.Properties.Items.AddRange(new object[] {
            "成功",
            "失败",
            "--请选择--"});
            this.cbbResponseState.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbbResponseState.Size = new System.Drawing.Size(158, 21);
            this.cbbResponseState.TabIndex = 36;
            // 
            // cbbState
            // 
            this.cbbState.Location = new System.Drawing.Point(499, 35);
            this.cbbState.Name = "cbbState";
            this.cbbState.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbbState.Properties.Items.AddRange(new object[] {
            "未发送",
            "请求失败",
            "请求成功",
            "--请选择--"});
            this.cbbState.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbbState.Size = new System.Drawing.Size(158, 21);
            this.cbbState.TabIndex = 34;
            // 
            // txtResponseMsg
            // 
            this.txtResponseMsg.Location = new System.Drawing.Point(162, 206);
            this.txtResponseMsg.Name = "txtResponseMsg";
            this.txtResponseMsg.Size = new System.Drawing.Size(497, 21);
            this.txtResponseMsg.TabIndex = 32;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(400, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "开单时间：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(73, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "运单号：";
            // 
            // imageList3
            // 
            this.imageList3.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList3.ImageStream")));
            this.imageList3.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList3.Images.SetKeyName(0, "Shell32 040.ico");
            this.imageList3.Images.SetKeyName(1, "Shell32 023.ico");
            this.imageList3.Images.SetKeyName(2, "Shell32 022.ico");
            this.imageList3.Images.SetKeyName(3, "Shell32 028.ico");
            this.imageList3.Images.SetKeyName(4, "Shell32 029.ico");
            this.imageList3.Images.SetKeyName(5, "Shell32 035.ico");
            this.imageList3.Images.SetKeyName(6, "Shell32 132.ico");
            this.imageList3.Images.SetKeyName(7, "Shell32 172.ico");
            this.imageList3.Images.SetKeyName(8, "check01c.gif");
            this.imageList3.Images.SetKeyName(9, "Shell32 048.ico");
            this.imageList3.Images.SetKeyName(10, "Clip.ico");
            this.imageList3.Images.SetKeyName(11, "Shell32 055.ico");
            this.imageList3.Images.SetKeyName(12, "icon_xls1.gif");
            this.imageList3.Images.SetKeyName(13, "Shell32 136.ico");
            this.imageList3.Images.SetKeyName(14, "Shell32 147.ico");
            this.imageList3.Images.SetKeyName(15, "Shell32 156.ico");
            this.imageList3.Images.SetKeyName(16, "delete.png");
            this.imageList3.Images.SetKeyName(17, "cadenas1.ico");
            this.imageList3.Images.SetKeyName(18, "Shell32 190.ico");
            this.imageList3.Images.SetKeyName(19, "Shell32 017.ico");
            this.imageList3.Images.SetKeyName(20, "Shell32 146.ico");
            this.imageList3.Images.SetKeyName(21, "AddItem_16x16.png");
            this.imageList3.Images.SetKeyName(22, "Delete_16x16 (2).png");
            this.imageList3.Images.SetKeyName(23, "Action_Edit.png");
            this.imageList3.Images.SetKeyName(24, "Action_Refresh.png");
            this.imageList3.Images.SetKeyName(25, "Action_Save.png");
            this.imageList3.Images.SetKeyName(26, "Action_Printing_Print.png");
            this.imageList3.Images.SetKeyName(27, "Action_Apply_16x16.png");
            this.imageList3.Images.SetKeyName(28, "Action_Close.png");
            this.imageList3.Images.SetKeyName(29, "Action_Search.png");
            this.imageList3.Images.SetKeyName(30, "Action_Customization_16x16.png");
            this.imageList3.Images.SetKeyName(31, "ExistLink_16x16.png");
            this.imageList3.Images.SetKeyName(32, "iconfinder.png");
            this.imageList3.Images.SetKeyName(33, "output.png");
            this.imageList3.Images.SetKeyName(34, "Action_ResetViewSettings.png");
            this.imageList3.Images.SetKeyName(35, "ColumnsThree_16x16.png");
            this.imageList3.Images.SetKeyName(36, "Show_16x16.png");
            this.imageList3.Images.SetKeyName(37, "iconfinder__messenger.png");
            this.imageList3.Images.SetKeyName(38, "iconfinder_Truck.png");
            this.imageList3.Images.SetKeyName(39, "iconfinder_edit2_.png");
            this.imageList3.Images.SetKeyName(40, "iconfinder_extract.png");
            this.imageList3.Images.SetKeyName(41, "Chart_16x16.png");
            this.imageList3.Images.SetKeyName(42, "Undo_16x16 (2).png");
            this.imageList3.Images.SetKeyName(43, "Today_16x16.png");
            this.imageList3.Images.SetKeyName(44, "iconfinder_file-download_326639.png");
            this.imageList3.Images.SetKeyName(45, "Index_16x16.png");
            this.imageList3.Images.SetKeyName(46, "Info_16x16.png");
            this.imageList3.Images.SetKeyName(47, "Forward_16x16.png");
            this.imageList3.Images.SetKeyName(48, "SnapToCells_16x16.png");
            this.imageList3.Images.SetKeyName(49, "Action_InsertImage_16x16.png");
            this.imageList3.Images.SetKeyName(50, "Next_16x16 (2).png");
            this.imageList3.Images.SetKeyName(51, "Add_16x16.png");
            this.imageList3.Images.SetKeyName(52, "Delete_16x16.png");
            // 
            // frmInsuranceEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(698, 521);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmInsuranceEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "投保信息修改";
            this.Load += new System.EventHandler(this.frmInsuranceEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtRequestURL.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQueryURL.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillNO.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dLastSentTime.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dLastSentTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCount.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtGUID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dCreateTime.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dCreateTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRequestJson.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dSendTime.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dSendTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbResponseState.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbState.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtResponseMsg.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtRequestURL;
        private DevExpress.XtraEditors.TextEdit txtQueryURL;
        private System.Windows.Forms.Label label20;
        private DevExpress.XtraEditors.TextEdit txtBillNO;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.MemoEdit txtRequestJson;
        private DevExpress.XtraEditors.DateEdit dSendTime;
        private DevExpress.XtraEditors.ComboBoxEdit cbbResponseState;
        private DevExpress.XtraEditors.ComboBoxEdit cbbState;
        private DevExpress.XtraEditors.TextEdit txtResponseMsg;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.DateEdit dCreateTime;
        private DevExpress.XtraEditors.TextEdit txtGUID;
        private DevExpress.XtraEditors.TextEdit txtCount;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.TextEdit txtCode;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.DateEdit dLastSentTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ImageList imageList3;
    }
}