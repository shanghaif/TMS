namespace ZQTMS.UI
{
    partial class frmDepartureAddSingle
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDepartureAddSingle));
            this.label1 = new System.Windows.Forms.Label();
            this.txtBillNo = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.lblDepartureBatch = new System.Windows.Forms.Label();
            this.lblDriverName = new System.Windows.Forms.Label();
            this.lblDriverPhone = new System.Windows.Forms.Label();
            this.lblCarNo = new System.Windows.Forms.Label();
            this.lblEndSite = new System.Windows.Forms.Label();
            this.lblBeginSite = new System.Windows.Forms.Label();
            this.MeAddReason = new DevExpress.XtraEditors.MemoEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.txtBillNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MeAddReason.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(71, 142);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "运单号：";
            // 
            // txtBillNo
            // 
            this.txtBillNo.EnterMoveNextControl = true;
            this.txtBillNo.Location = new System.Drawing.Point(132, 139);
            this.txtBillNo.Name = "txtBillNo";
            this.txtBillNo.Size = new System.Drawing.Size(221, 21);
            this.txtBillNo.TabIndex = 1;
            // 
            // simpleButton1
            // 
            this.simpleButton1.ImageIndex = 35;
            this.simpleButton1.ImageList = this.imageList3;
            this.simpleButton1.Location = new System.Drawing.Point(91, 298);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(81, 23);
            this.simpleButton1.TabIndex = 2;
            this.simpleButton1.Text = "查看运单";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "Shell32 040.ico");
            this.imageList2.Images.SetKeyName(1, "Shell32 023.ico");
            this.imageList2.Images.SetKeyName(2, "Shell32 022.ico");
            this.imageList2.Images.SetKeyName(3, "Shell32 028.ico");
            this.imageList2.Images.SetKeyName(4, "Shell32 029.ico");
            this.imageList2.Images.SetKeyName(5, "Shell32 035.ico");
            this.imageList2.Images.SetKeyName(6, "Shell32 132.ico");
            this.imageList2.Images.SetKeyName(7, "Shell32 172.ico");
            this.imageList2.Images.SetKeyName(8, "check01c.gif");
            this.imageList2.Images.SetKeyName(9, "Shell32 048.ico");
            this.imageList2.Images.SetKeyName(10, "Clip.ico");
            this.imageList2.Images.SetKeyName(11, "Shell32 055.ico");
            this.imageList2.Images.SetKeyName(12, "icon_xls1.gif");
            this.imageList2.Images.SetKeyName(13, "Shell32 136.ico");
            this.imageList2.Images.SetKeyName(14, "Shell32 147.ico");
            this.imageList2.Images.SetKeyName(15, "Shell32 156.ico");
            this.imageList2.Images.SetKeyName(16, "delete.png");
            this.imageList2.Images.SetKeyName(17, "cadenas1.ico");
            this.imageList2.Images.SetKeyName(18, "Shell32 190.ico");
            this.imageList2.Images.SetKeyName(19, "Shell32 017.ico");
            this.imageList2.Images.SetKeyName(20, "Shell32 146.ico");
            this.imageList2.Images.SetKeyName(21, "refresh.JPG");
            this.imageList2.Images.SetKeyName(22, "Shell32 058.ico");
            this.imageList2.Images.SetKeyName(23, "report1.ico");
            this.imageList2.Images.SetKeyName(24, "truck.ico");
            // 
            // simpleButton2
            // 
            this.simpleButton2.ImageIndex = 21;
            this.simpleButton2.ImageList = this.imageList3;
            this.simpleButton2.Location = new System.Drawing.Point(192, 298);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(81, 23);
            this.simpleButton2.TabIndex = 3;
            this.simpleButton2.Text = "加入本车";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton3
            // 
            this.simpleButton3.ImageIndex = 28;
            this.simpleButton3.ImageList = this.imageList3;
            this.simpleButton3.Location = new System.Drawing.Point(293, 298);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(81, 23);
            this.simpleButton3.TabIndex = 4;
            this.simpleButton3.Text = "关 闭";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // lblDepartureBatch
            // 
            this.lblDepartureBatch.AutoSize = true;
            this.lblDepartureBatch.Location = new System.Drawing.Point(27, 21);
            this.lblDepartureBatch.Name = "lblDepartureBatch";
            this.lblDepartureBatch.Size = new System.Drawing.Size(67, 14);
            this.lblDepartureBatch.TabIndex = 5;
            this.lblDepartureBatch.Text = "发车批次：";
            // 
            // lblDriverName
            // 
            this.lblDriverName.AutoSize = true;
            this.lblDriverName.Location = new System.Drawing.Point(27, 56);
            this.lblDriverName.Name = "lblDriverName";
            this.lblDriverName.Size = new System.Drawing.Size(67, 14);
            this.lblDriverName.TabIndex = 6;
            this.lblDriverName.Text = "司机信息：";
            // 
            // lblDriverPhone
            // 
            this.lblDriverPhone.AutoSize = true;
            this.lblDriverPhone.Location = new System.Drawing.Point(208, 56);
            this.lblDriverPhone.Name = "lblDriverPhone";
            this.lblDriverPhone.Size = new System.Drawing.Size(67, 14);
            this.lblDriverPhone.TabIndex = 7;
            this.lblDriverPhone.Text = "司机电话：";
            // 
            // lblCarNo
            // 
            this.lblCarNo.AutoSize = true;
            this.lblCarNo.Location = new System.Drawing.Point(208, 21);
            this.lblCarNo.Name = "lblCarNo";
            this.lblCarNo.Size = new System.Drawing.Size(67, 14);
            this.lblCarNo.TabIndex = 8;
            this.lblCarNo.Text = "车　　号：";
            // 
            // lblEndSite
            // 
            this.lblEndSite.AutoSize = true;
            this.lblEndSite.Location = new System.Drawing.Point(208, 91);
            this.lblEndSite.Name = "lblEndSite";
            this.lblEndSite.Size = new System.Drawing.Size(63, 14);
            this.lblEndSite.TabIndex = 10;
            this.lblEndSite.Text = "目 的 地：";
            // 
            // lblBeginSite
            // 
            this.lblBeginSite.AutoSize = true;
            this.lblBeginSite.Location = new System.Drawing.Point(27, 91);
            this.lblBeginSite.Name = "lblBeginSite";
            this.lblBeginSite.Size = new System.Drawing.Size(63, 14);
            this.lblBeginSite.TabIndex = 9;
            this.lblBeginSite.Text = "启 运 地：";
            // 
            // MeAddReason
            // 
            this.MeAddReason.Location = new System.Drawing.Point(132, 176);
            this.MeAddReason.Name = "MeAddReason";
            this.MeAddReason.Size = new System.Drawing.Size(221, 96);
            this.MeAddReason.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(71, 179);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 14);
            this.label2.TabIndex = 12;
            this.label2.Text = "原　因：";
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
            // 
            // frmDepartureAddSingle
            // 
            this.ClientSize = new System.Drawing.Size(408, 343);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.MeAddReason);
            this.Controls.Add(this.lblEndSite);
            this.Controls.Add(this.lblBeginSite);
            this.Controls.Add(this.lblCarNo);
            this.Controls.Add(this.lblDriverPhone);
            this.Controls.Add(this.lblDriverName);
            this.Controls.Add(this.lblDepartureBatch);
            this.Controls.Add(this.simpleButton3);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.txtBillNo);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmDepartureAddSingle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "配载单票强装";
            this.Load += new System.EventHandler(this.frmDepartureAddSingle_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtBillNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MeAddReason.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit txtBillNo;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private System.Windows.Forms.Label lblDepartureBatch;
        private System.Windows.Forms.Label lblDriverName;
        private System.Windows.Forms.Label lblDriverPhone;
        private System.Windows.Forms.Label lblCarNo;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.Label lblEndSite;
        private System.Windows.Forms.Label lblBeginSite;
        private DevExpress.XtraEditors.MemoEdit MeAddReason;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ImageList imageList3;
    }
}