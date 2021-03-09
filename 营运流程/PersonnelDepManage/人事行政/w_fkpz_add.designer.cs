namespace ZQTMS.UI
{
    partial class w_fkpz_add
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(w_fkpz_add));
            this.dw_1 = new Sybase.DataWindow.DataWindowControl();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.dw_2 = new Sybase.DataWindow.DataWindowControl();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dw_1
            // 
            this.dw_1.BorderStyle = Sybase.DataWindow.DataWindowBorderStyle.None;
            this.dw_1.DataWindowObject = "宏浩付款凭证录入";
            this.dw_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dw_1.LibraryList = "D:\\研究中的版本\\lanqiao2006\\bin\\Release\\reports.pbl;D:\\研究中的版本\\lanqiao2006\\bin\\Release\\b" +
                "illprint.pbl";
            this.dw_1.Location = new System.Drawing.Point(0, 35);
            this.dw_1.Name = "dw_1";
            this.dw_1.Size = new System.Drawing.Size(696, 375);
            this.dw_1.TabIndex = 0;
            this.dw_1.Text = "dataWindowControl1";
            this.dw_1.ItemChanged += new Sybase.DataWindow.ItemChangedEventHandler(this.dw_1_ItemChanged);
            this.dw_1.EndRetrieve += new Sybase.DataWindow.EndRetrieveEventHandler(this.dw_1_EndRetrieve);
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton4,
            this.toolStripButton3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(696, 35);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            //this.toolStripButton1.Image = global::ThisisLanQiaoSoft.Properties.Resources.Shell32_055;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(55, 32);
            this.toolStripButton1.Text = "新增 ";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            //this.toolStripButton2.Image = global::ThisisLanQiaoSoft.Properties.Resources.Shell32_190;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(55, 32);
            this.toolStripButton2.Text = "保存 ";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(55, 32);
            this.toolStripButton4.Text = "打印 ";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripButton3
            // 
            //this.toolStripButton3.Image = global::ThisisLanQiaoSoft.Properties.Resources.Shell32_132;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(49, 32);
            this.toolStripButton3.Text = "退出";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // dw_2
            // 
            this.dw_2.BorderStyle = Sybase.DataWindow.DataWindowBorderStyle.None;
            this.dw_2.DataWindowObject = "宏浩付款凭证打印";
            this.dw_2.LibraryList = "D:\\研究中的版本\\lanqiao2006\\bin\\Release\\reports.pbl;D:\\研究中的版本\\lanqiao2006\\bin\\Release\\b" +
                "illprint.pbl";
            this.dw_2.Location = new System.Drawing.Point(451, 86);
            this.dw_2.Name = "dw_2";
            this.dw_2.Size = new System.Drawing.Size(83, 150);
            this.dw_2.TabIndex = 2;
            this.dw_2.Text = "dataWindowControl1";
            // 
            // w_fkpz_add
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(696, 410);
            this.Controls.Add(this.dw_1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.dw_2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "w_fkpz_add";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "付款凭证";
            this.Load += new System.EventHandler(this.w_yg_add_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        public Sybase.DataWindow.DataWindowControl dw_1;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        public Sybase.DataWindow.DataWindowControl dw_2;
    }
}