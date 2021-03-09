namespace ZQTMS.UI
{
    partial class w_oa_file_add
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.edfile = new DevExpress.XtraEditors.ButtonEdit();
            this.edremark = new DevExpress.XtraEditors.TextEdit();
            this.edcreateby = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.eddocname = new DevExpress.XtraEditors.TextEdit();
            this.popupContainerControl1 = new DevExpress.XtraEditors.PopupContainerControl();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.checkEdit1 = new DevExpress.XtraEditors.CheckEdit();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.edfiletype = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.edreceive = new DevExpress.XtraEditors.PopupContainerEdit();
            ((System.ComponentModel.ISupportInitialize)(this.edfile.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edremark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edcreateby.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eddocname.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerControl1)).BeginInit();
            this.popupContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edfiletype.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edreceive.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "文件路径";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "接收对象";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 227);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 14);
            this.label3.TabIndex = 2;
            this.label3.Text = "文件备注";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 275);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 14);
            this.label5.TabIndex = 4;
            this.label5.Text = "上 传  人";
            // 
            // edfile
            // 
            this.edfile.EnterMoveNextControl = true;
            this.edfile.Location = new System.Drawing.Point(84, 32);
            this.edfile.Name = "edfile";
            this.edfile.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "浏览...", 40, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.edfile.Properties.ReadOnly = true;
            this.edfile.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.edfile.Size = new System.Drawing.Size(352, 21);
            this.edfile.TabIndex = 5;
            this.edfile.ToolTip = "选择文件...";
            this.edfile.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.edfilename_ButtonClick);
            // 
            // edremark
            // 
            this.edremark.EnterMoveNextControl = true;
            this.edremark.Location = new System.Drawing.Point(84, 224);
            this.edremark.Name = "edremark";
            this.edremark.Size = new System.Drawing.Size(352, 21);
            this.edremark.TabIndex = 7;
            // 
            // edcreateby
            // 
            this.edcreateby.EnterMoveNextControl = true;
            this.edcreateby.Location = new System.Drawing.Point(84, 272);
            this.edcreateby.Name = "edcreateby";
            this.edcreateby.Properties.ReadOnly = true;
            this.edcreateby.Size = new System.Drawing.Size(352, 21);
            this.edcreateby.TabIndex = 9;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(81, 319);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 23);
            this.simpleButton1.TabIndex = 10;
            this.simpleButton1.Text = "开始上传";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(309, 319);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(75, 23);
            this.simpleButton2.TabIndex = 11;
            this.simpleButton2.Text = "退  出";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 14);
            this.label4.TabIndex = 12;
            this.label4.Text = "文档标题";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(22, 179);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 14);
            this.label6.TabIndex = 13;
            this.label6.Text = "文档类型";
            // 
            // eddocname
            // 
            this.eddocname.EnterMoveNextControl = true;
            this.eddocname.Location = new System.Drawing.Point(84, 80);
            this.eddocname.Name = "eddocname";
            this.eddocname.Size = new System.Drawing.Size(352, 21);
            this.eddocname.TabIndex = 14;
            // 
            // popupContainerControl1
            // 
            this.popupContainerControl1.Controls.Add(this.treeView1);
            this.popupContainerControl1.Controls.Add(this.checkEdit1);
            this.popupContainerControl1.Controls.Add(this.simpleButton3);
            this.popupContainerControl1.Location = new System.Drawing.Point(563, 35);
            this.popupContainerControl1.Name = "popupContainerControl1";
            this.popupContainerControl1.Size = new System.Drawing.Size(268, 326);
            this.popupContainerControl1.TabIndex = 18;
            // 
            // treeView1
            // 
            this.treeView1.CheckBoxes = true;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 19);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(268, 284);
            this.treeView1.TabIndex = 24;
            // 
            // checkEdit1
            // 
            this.checkEdit1.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkEdit1.Location = new System.Drawing.Point(0, 0);
            this.checkEdit1.Name = "checkEdit1";
            this.checkEdit1.Properties.Caption = "发送给所有人";
            this.checkEdit1.Size = new System.Drawing.Size(268, 19);
            this.checkEdit1.TabIndex = 25;
            // 
            // simpleButton3
            // 
            this.simpleButton3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.simpleButton3.Location = new System.Drawing.Point(0, 303);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(268, 23);
            this.simpleButton3.TabIndex = 23;
            this.simpleButton3.Text = "确认选择";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // edfiletype
            // 
            this.edfiletype.Location = new System.Drawing.Point(84, 176);
            this.edfiletype.Name = "edfiletype";
            this.edfiletype.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", 18, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Plus, "", 17, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject3, "编辑文档类型", null, null, true)});
            this.edfiletype.Properties.DropDownRows = 15;
            this.edfiletype.Properties.SelectAllItemCaption = "全部选择";
            this.edfiletype.Size = new System.Drawing.Size(352, 21);
            this.edfiletype.TabIndex = 15;
            this.edfiletype.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.edtype_ButtonClick);
            // 
            // edreceive
            // 
            this.edreceive.Location = new System.Drawing.Point(84, 128);
            this.edreceive.Name = "edreceive";
            this.edreceive.Properties.Appearance.Options.UseTextOptions = true;
            this.edreceive.Properties.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.edreceive.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.edreceive.Properties.PopupControl = this.popupContainerControl1;
            this.edreceive.Size = new System.Drawing.Size(352, 21);
            this.edreceive.TabIndex = 17;
            this.edreceive.TabStop = false;
            // 
            // w_oa_file_add
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(461, 400);
            this.Controls.Add(this.edfiletype);
            this.Controls.Add(this.popupContainerControl1);
            this.Controls.Add(this.eddocname);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.edcreateby);
            this.Controls.Add(this.edremark);
            this.Controls.Add(this.edfile);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.edreceive);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.MaximizeBox = false;
            this.Name = "w_oa_file_add";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "添加办公文档";
            this.Load += new System.EventHandler(this.w_file_add_Load);
            ((System.ComponentModel.ISupportInitialize)(this.edfile.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edremark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edcreateby.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eddocname.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerControl1)).EndInit();
            this.popupContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edfiletype.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edreceive.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.ButtonEdit edfile;
        private DevExpress.XtraEditors.TextEdit edremark;
        private DevExpress.XtraEditors.TextEdit edcreateby;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.TextEdit eddocname;
        private DevExpress.XtraEditors.PopupContainerControl popupContainerControl1;
        private DevExpress.XtraEditors.CheckedComboBoxEdit edfiletype;
        private System.Windows.Forms.TreeView treeView1;
        private DevExpress.XtraEditors.CheckEdit checkEdit1;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.PopupContainerEdit edreceive;
    }
}