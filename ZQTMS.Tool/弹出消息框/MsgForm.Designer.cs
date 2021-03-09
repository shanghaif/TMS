namespace ZQTMS.Tool
{
    partial class MsgForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MsgForm));
            this.ucLine1 = new ZQTMS.Lib.UCLine();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.lbTitle = new DevExpress.XtraEditors.LabelControl();
            this.lbContent = new DevExpress.XtraEditors.MemoEdit();
            this.ucLine2 = new ZQTMS.Lib.UCLine();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.btn1 = new DevExpress.XtraEditors.SimpleButton();
            this.btn2 = new DevExpress.XtraEditors.SimpleButton();
            this.btn3 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.lbContent.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // ucLine1
            // 
            this.ucLine1.Location = new System.Drawing.Point(1, 41);
            this.ucLine1.Name = "ucLine1";
            this.ucLine1.Size = new System.Drawing.Size(358, 2);
            this.ucLine1.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.ImageIndex = 0;
            this.labelControl1.Appearance.ImageList = this.imageList1;
            this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl1.Location = new System.Drawing.Point(11, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(40, 31);
            this.labelControl1.TabIndex = 1;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "msg-error.png");
            this.imageList1.Images.SetKeyName(1, "msg-info.png");
            this.imageList1.Images.SetKeyName(2, "msg-uestion.png");
            this.imageList1.Images.SetKeyName(3, "msg-warning.png");
            // 
            // lbTitle
            // 
            this.lbTitle.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lbTitle.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.lbTitle.Appearance.Options.UseFont = true;
            this.lbTitle.Appearance.Options.UseForeColor = true;
            this.lbTitle.Location = new System.Drawing.Point(58, 20);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(60, 16);
            this.lbTitle.TabIndex = 2;
            this.lbTitle.Text = "信息提示";
            // 
            // lbContent
            // 
            this.lbContent.EditValue = "";
            this.lbContent.Location = new System.Drawing.Point(54, 43);
            this.lbContent.Name = "lbContent";
            this.lbContent.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.lbContent.Properties.Appearance.Options.UseBackColor = true;
            this.lbContent.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.lbContent.Properties.ReadOnly = true;
            this.lbContent.Properties.UseParentBackground = true;
            this.lbContent.Size = new System.Drawing.Size(301, 99);
            this.lbContent.TabIndex = 3;
            // 
            // ucLine2
            // 
            this.ucLine2.Location = new System.Drawing.Point(-2, 144);
            this.ucLine2.Name = "ucLine2";
            this.ucLine2.Size = new System.Drawing.Size(358, 2);
            this.ucLine2.TabIndex = 4;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.ForeColor = System.Drawing.Color.Blue;
            this.linkLabel1.LinkColor = System.Drawing.Color.Blue;
            this.linkLabel1.Location = new System.Drawing.Point(15, 126);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(31, 14);
            this.linkLabel1.TabIndex = 5;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "复制";
            this.linkLabel1.VisitedLinkColor = System.Drawing.Color.Blue;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // btn1
            // 
            this.btn1.Location = new System.Drawing.Point(54, 158);
            this.btn1.Name = "btn1";
            this.btn1.Size = new System.Drawing.Size(75, 23);
            this.btn1.TabIndex = 6;
            this.btn1.Text = "确定1";
            this.btn1.Visible = false;
            // 
            // btn2
            // 
            this.btn2.Location = new System.Drawing.Point(159, 158);
            this.btn2.Name = "btn2";
            this.btn2.Size = new System.Drawing.Size(75, 23);
            this.btn2.TabIndex = 7;
            this.btn2.Text = "确定2";
            this.btn2.Visible = false;
            // 
            // btn3
            // 
            this.btn3.Location = new System.Drawing.Point(262, 158);
            this.btn3.Name = "btn3";
            this.btn3.Size = new System.Drawing.Size(75, 23);
            this.btn3.TabIndex = 8;
            this.btn3.Text = "确定3";
            this.btn3.Visible = false;
            // 
            // MsgForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 193);
            this.Controls.Add(this.btn3);
            this.Controls.Add(this.btn2);
            this.Controls.Add(this.btn1);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.ucLine2);
            this.Controls.Add(this.lbContent);
            this.Controls.Add(this.lbTitle);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.ucLine1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MsgForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "中强集团TMS";
            this.Load += new System.EventHandler(this.MsgForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lbContent.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ZQTMS.Lib.UCLine ucLine1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraEditors.LabelControl lbTitle;
        private DevExpress.XtraEditors.MemoEdit lbContent;
        private ZQTMS.Lib.UCLine ucLine2;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private DevExpress.XtraEditors.SimpleButton btn1;
        private DevExpress.XtraEditors.SimpleButton btn2;
        private DevExpress.XtraEditors.SimpleButton btn3;



    }
}