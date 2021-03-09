namespace ZQTMS.UI
{
    partial class frmAreaDivideUpt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAreaDivideUpt));
            this.middlePartnerTe = new DevExpress.XtraEditors.TextEdit();
            this.labelControl20 = new DevExpress.XtraEditors.LabelControl();
            this.sbjWebCbe = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl19 = new DevExpress.XtraEditors.LabelControl();
            this.OptCoverageCbe = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl18 = new DevExpress.XtraEditors.LabelControl();
            this.coverageZoneTe = new DevExpress.XtraEditors.TextEdit();
            this.labelControl16 = new DevExpress.XtraEditors.LabelControl();
            this.btnConcel = new DevExpress.XtraEditors.SimpleButton();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.btnSubmit = new DevExpress.XtraEditors.SimpleButton();
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.middlePartnerTe.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbjWebCbe.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OptCoverageCbe.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.coverageZoneTe.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // middlePartnerTe
            // 
            this.middlePartnerTe.EnterMoveNextControl = true;
            this.middlePartnerTe.Location = new System.Drawing.Point(96, 153);
            this.middlePartnerTe.Name = "middlePartnerTe";
            this.middlePartnerTe.Size = new System.Drawing.Size(171, 21);
            this.middlePartnerTe.TabIndex = 389;
            // 
            // labelControl20
            // 
            this.labelControl20.Location = new System.Drawing.Point(30, 156);
            this.labelControl20.Name = "labelControl20";
            this.labelControl20.Size = new System.Drawing.Size(60, 14);
            this.labelControl20.TabIndex = 388;
            this.labelControl20.Text = "中转合作商";
            // 
            // sbjWebCbe
            // 
            this.sbjWebCbe.EditValue = "";
            this.sbjWebCbe.EnterMoveNextControl = true;
            this.sbjWebCbe.Location = new System.Drawing.Point(96, 66);
            this.sbjWebCbe.Name = "sbjWebCbe";
            this.sbjWebCbe.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.sbjWebCbe.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.sbjWebCbe.Size = new System.Drawing.Size(171, 21);
            this.sbjWebCbe.TabIndex = 387;
            // 
            // labelControl19
            // 
            this.labelControl19.Location = new System.Drawing.Point(18, 69);
            this.labelControl19.Name = "labelControl19";
            this.labelControl19.Size = new System.Drawing.Size(72, 14);
            this.labelControl19.TabIndex = 386;
            this.labelControl19.Text = "隶属分拨中心";
            // 
            // OptCoverageCbe
            // 
            this.OptCoverageCbe.EditValue = "";
            this.OptCoverageCbe.EnterMoveNextControl = true;
            this.OptCoverageCbe.Location = new System.Drawing.Point(96, 110);
            this.OptCoverageCbe.Name = "OptCoverageCbe";
            this.OptCoverageCbe.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.OptCoverageCbe.Properties.Items.AddRange(new object[] {
            "",
            "网点覆盖",
            "中心覆盖",
            "中转覆盖",
            "未覆盖"});
            this.OptCoverageCbe.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.OptCoverageCbe.Size = new System.Drawing.Size(171, 21);
            this.OptCoverageCbe.TabIndex = 385;
            // 
            // labelControl18
            // 
            this.labelControl18.Location = new System.Drawing.Point(42, 113);
            this.labelControl18.Name = "labelControl18";
            this.labelControl18.Size = new System.Drawing.Size(48, 14);
            this.labelControl18.TabIndex = 384;
            this.labelControl18.Text = "操作覆盖";
            // 
            // coverageZoneTe
            // 
            this.coverageZoneTe.EnterMoveNextControl = true;
            this.coverageZoneTe.Location = new System.Drawing.Point(96, 23);
            this.coverageZoneTe.Name = "coverageZoneTe";
            this.coverageZoneTe.Size = new System.Drawing.Size(171, 21);
            this.coverageZoneTe.TabIndex = 383;
            // 
            // labelControl16
            // 
            this.labelControl16.Location = new System.Drawing.Point(42, 26);
            this.labelControl16.Name = "labelControl16";
            this.labelControl16.Size = new System.Drawing.Size(48, 14);
            this.labelControl16.TabIndex = 382;
            this.labelControl16.Text = "覆盖分区";
            // 
            // btnConcel
            // 
            this.btnConcel.ImageIndex = 28;
            this.btnConcel.ImageList = this.imageList3;
            this.btnConcel.Location = new System.Drawing.Point(164, 223);
            this.btnConcel.Name = "btnConcel";
            this.btnConcel.Size = new System.Drawing.Size(76, 29);
            this.btnConcel.TabIndex = 391;
            this.btnConcel.Text = "关  闭";
            this.btnConcel.Click += new System.EventHandler(this.btnConcel_Click);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "Shell32 028.ico");
            this.imageList2.Images.SetKeyName(1, "Shell32 190.ico");
            // 
            // btnSubmit
            // 
            this.btnSubmit.ImageIndex = 25;
            this.btnSubmit.ImageList = this.imageList3;
            this.btnSubmit.Location = new System.Drawing.Point(55, 223);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(76, 29);
            this.btnSubmit.TabIndex = 390;
            this.btnSubmit.Text = "保  存";
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
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
            // 
            // frmAreaDivideUpt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 286);
            this.Controls.Add(this.btnConcel);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.middlePartnerTe);
            this.Controls.Add(this.labelControl20);
            this.Controls.Add(this.sbjWebCbe);
            this.Controls.Add(this.labelControl19);
            this.Controls.Add(this.OptCoverageCbe);
            this.Controls.Add(this.labelControl18);
            this.Controls.Add(this.coverageZoneTe);
            this.Controls.Add(this.labelControl16);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmAreaDivideUpt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "终端区域覆盖划分";
            this.Load += new System.EventHandler(this.AreaDivideUpt_Load);
            ((System.ComponentModel.ISupportInitialize)(this.middlePartnerTe.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbjWebCbe.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OptCoverageCbe.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.coverageZoneTe.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit middlePartnerTe;
        private DevExpress.XtraEditors.LabelControl labelControl20;
        private DevExpress.XtraEditors.ComboBoxEdit sbjWebCbe;
        private DevExpress.XtraEditors.LabelControl labelControl19;
        private DevExpress.XtraEditors.ComboBoxEdit OptCoverageCbe;
        private DevExpress.XtraEditors.LabelControl labelControl18;
        private DevExpress.XtraEditors.TextEdit coverageZoneTe;
        private DevExpress.XtraEditors.LabelControl labelControl16;
        private DevExpress.XtraEditors.SimpleButton btnConcel;
        private DevExpress.XtraEditors.SimpleButton btnSubmit;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.ImageList imageList3;
    }
}