namespace ZQTMS.UI.BaseInfoManage.欠款管控
{
    partial class AddfrmArrearsControll
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
            this.WebResponsiblePerson = new DevExpress.XtraEditors.TextEdit();
            this.ArrearsControlDate = new DevExpress.XtraEditors.TextEdit();
            this.BefArrivalPayState = new System.Windows.Forms.CheckBox();
            this.OwePayState = new System.Windows.Forms.CheckBox();
            this.OpenState = new System.Windows.Forms.CheckBox();
            this.ReceiptPayState = new System.Windows.Forms.CheckBox();
            this.btn_cancel = new DevExpress.XtraEditors.SimpleButton();
            this.btn_save = new DevExpress.XtraEditors.SimpleButton();
            this.ArrearsAmount = new DevExpress.XtraEditors.TextEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.WebName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.SiteName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.WebResponsiblePerson.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ArrearsControlDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ArrearsAmount.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WebName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SiteName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // WebResponsiblePerson
            // 
            this.WebResponsiblePerson.Location = new System.Drawing.Point(145, 101);
            this.WebResponsiblePerson.Name = "WebResponsiblePerson";
            this.WebResponsiblePerson.Properties.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.WebResponsiblePerson.Properties.Appearance.Options.UseBackColor = true;
            this.WebResponsiblePerson.Properties.ReadOnly = true;
            this.WebResponsiblePerson.Size = new System.Drawing.Size(125, 21);
            this.WebResponsiblePerson.TabIndex = 39;
            // 
            // ArrearsControlDate
            // 
            this.ArrearsControlDate.Location = new System.Drawing.Point(418, 154);
            this.ArrearsControlDate.Name = "ArrearsControlDate";
            this.ArrearsControlDate.Size = new System.Drawing.Size(123, 21);
            this.ArrearsControlDate.TabIndex = 38;
            // 
            // BefArrivalPayState
            // 
            this.BefArrivalPayState.AutoSize = true;
            this.BefArrivalPayState.Location = new System.Drawing.Point(357, 228);
            this.BefArrivalPayState.Name = "BefArrivalPayState";
            this.BefArrivalPayState.Size = new System.Drawing.Size(122, 18);
            this.BefArrivalPayState.TabIndex = 37;
            this.BefArrivalPayState.Text = "是否启用货到前付";
            this.BefArrivalPayState.UseVisualStyleBackColor = true;
            // 
            // OwePayState
            // 
            this.OwePayState.AutoSize = true;
            this.OwePayState.Location = new System.Drawing.Point(75, 297);
            this.OwePayState.Name = "OwePayState";
            this.OwePayState.Size = new System.Drawing.Size(98, 18);
            this.OwePayState.TabIndex = 36;
            this.OwePayState.Text = "是否启用欠付";
            this.OwePayState.UseVisualStyleBackColor = true;
            // 
            // OpenState
            // 
            this.OpenState.AutoSize = true;
            this.OpenState.Location = new System.Drawing.Point(78, 228);
            this.OpenState.Name = "OpenState";
            this.OpenState.Size = new System.Drawing.Size(98, 18);
            this.OpenState.TabIndex = 35;
            this.OpenState.Text = "是否启用状态";
            this.OpenState.UseVisualStyleBackColor = true;
            // 
            // ReceiptPayState
            // 
            this.ReceiptPayState.AutoSize = true;
            this.ReceiptPayState.Location = new System.Drawing.Point(357, 297);
            this.ReceiptPayState.Name = "ReceiptPayState";
            this.ReceiptPayState.Size = new System.Drawing.Size(110, 18);
            this.ReceiptPayState.TabIndex = 34;
            this.ReceiptPayState.Text = "是否开启回单付";
            this.ReceiptPayState.UseVisualStyleBackColor = true;
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(327, 377);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_cancel.TabIndex = 33;
            this.btn_cancel.Text = "取消";
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(116, 377);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(75, 23);
            this.btn_save.TabIndex = 32;
            this.btn_save.Text = "保存";
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // ArrearsAmount
            // 
            this.ArrearsAmount.EnterMoveNextControl = true;
            this.ArrearsAmount.Location = new System.Drawing.Point(145, 158);
            this.ArrearsAmount.Name = "ArrearsAmount";
            this.ArrearsAmount.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.ArrearsAmount.Properties.Appearance.Options.UseForeColor = true;
            this.ArrearsAmount.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.ArrearsAmount.Size = new System.Drawing.Size(126, 21);
            this.ArrearsAmount.TabIndex = 31;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(354, 157);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 14);
            this.label5.TabIndex = 30;
            this.label5.Text = "欠款时间：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(75, 161);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 14);
            this.label4.TabIndex = 29;
            this.label4.Text = "欠款额度：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(72, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 14);
            this.label3.TabIndex = 28;
            this.label3.Text = "网点负责人：";
            // 
            // WebName
            // 
            this.WebName.EditValue = "全部";
            this.WebName.Location = new System.Drawing.Point(418, 37);
            this.WebName.Name = "WebName";
            this.WebName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.WebName.Size = new System.Drawing.Size(136, 21);
            this.WebName.TabIndex = 27;
            this.WebName.SelectedIndexChanged += new System.EventHandler(this.WebName_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(354, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 14);
            this.label2.TabIndex = 26;
            this.label2.Text = "网点名称：";
            // 
            // SiteName
            // 
            this.SiteName.EditValue = "全部";
            this.SiteName.Location = new System.Drawing.Point(145, 37);
            this.SiteName.Name = "SiteName";
            this.SiteName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SiteName.Size = new System.Drawing.Size(126, 21);
            this.SiteName.TabIndex = 25;
            this.SiteName.SelectedIndexChanged += new System.EventHandler(this.SiteName_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(72, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 14);
            this.label1.TabIndex = 24;
            this.label1.Text = "隶属站点：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(547, 157);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(19, 14);
            this.label6.TabIndex = 40;
            this.label6.Text = "天";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(277, 161);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(19, 14);
            this.label7.TabIndex = 41;
            this.label7.Text = "元";
            // 
            // AddfrmArrearsControll
            // 
            this.ClientSize = new System.Drawing.Size(575, 431);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.WebResponsiblePerson);
            this.Controls.Add(this.ArrearsControlDate);
            this.Controls.Add(this.BefArrivalPayState);
            this.Controls.Add(this.OwePayState);
            this.Controls.Add(this.OpenState);
            this.Controls.Add(this.ReceiptPayState);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.ArrearsAmount);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.WebName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SiteName);
            this.Controls.Add(this.label1);
            this.Name = "AddfrmArrearsControll";
            this.Text = "欠款管控";
            this.Load += new System.EventHandler(this.AddfrmArrearsControll_Load);
            ((System.ComponentModel.ISupportInitialize)(this.WebResponsiblePerson.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ArrearsControlDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ArrearsAmount.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WebName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SiteName.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit WebResponsiblePerson;
        private DevExpress.XtraEditors.TextEdit ArrearsControlDate;
        private System.Windows.Forms.CheckBox BefArrivalPayState;
        private System.Windows.Forms.CheckBox OwePayState;
        private System.Windows.Forms.CheckBox OpenState;
        private System.Windows.Forms.CheckBox ReceiptPayState;
        private DevExpress.XtraEditors.SimpleButton btn_cancel;
        private DevExpress.XtraEditors.SimpleButton btn_save;
        private DevExpress.XtraEditors.TextEdit ArrearsAmount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.ComboBoxEdit WebName;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.ComboBoxEdit SiteName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;

    }
}
