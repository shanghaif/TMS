﻿namespace ZQTMS.UI
{
    partial class frmDriverImageFileUp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDriverImageFileUp));
            this.btnUpload = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.progressBarControl1 = new DevExpress.XtraEditors.ProgressBarControl();
            this.lblstate = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.listLog = new System.Windows.Forms.ListBox();
            this.btnClearLog = new DevExpress.XtraEditors.SimpleButton();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDelFile = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.ImgeType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControl1.Properties)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImgeType.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(179, 333);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(58, 23);
            this.btnUpload.TabIndex = 0;
            this.btnUpload.Text = "上传";
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // gridControl1
            // 
            this.gridControl1.Location = new System.Drawing.Point(12, 59);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(489, 268);
            this.gridControl1.TabIndex = 2;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.ColumnPanelRowHeight = 30;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn3,
            this.gridColumn1,
            this.gridColumn4,
            this.gridColumn2,
            this.gridColumn5});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsFilter.AllowFilterEditor = false;
            this.gridView1.OptionsMenu.EnableColumnMenu = false;
            this.gridView1.OptionsMenu.EnableFooterMenu = false;
            this.gridView1.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.RowHeight = 30;
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "序号";
            this.gridColumn3.FieldName = "rowid";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.OptionsColumn.AllowFocus = false;
            this.gridColumn3.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 0;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "文件名";
            this.gridColumn1.FieldName = "filename";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.OptionsColumn.AllowFocus = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 1;
            this.gridColumn1.Width = 80;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "文件大小(KB)";
            this.gridColumn4.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn4.FieldName = "fileSize";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.OptionsColumn.AllowFocus = false;
            this.gridColumn4.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 2;
            this.gridColumn4.Width = 87;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "路径";
            this.gridColumn2.FieldName = "path";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.OptionsColumn.AllowFocus = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 3;
            this.gridColumn2.Width = 300;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "状态";
            this.gridColumn5.FieldName = "state";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowEdit = false;
            this.gridColumn5.OptionsColumn.AllowFocus = false;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 4;
            // 
            // progressBarControl1
            // 
            this.progressBarControl1.EditValue = "0";
            this.progressBarControl1.Enabled = false;
            this.progressBarControl1.Location = new System.Drawing.Point(3, 22);
            this.progressBarControl1.Name = "progressBarControl1";
            this.progressBarControl1.Size = new System.Drawing.Size(709, 13);
            this.progressBarControl1.TabIndex = 3;
            // 
            // lblstate
            // 
            this.lblstate.Location = new System.Drawing.Point(3, 1);
            this.lblstate.Name = "lblstate";
            this.lblstate.Size = new System.Drawing.Size(709, 18);
            this.lblstate.TabIndex = 4;
            this.lblstate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.progressBarControl1);
            this.panel1.Controls.Add(this.lblstate);
            this.panel1.Location = new System.Drawing.Point(11, 362);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(715, 38);
            this.panel1.TabIndex = 5;
            // 
            // listLog
            // 
            this.listLog.BackColor = System.Drawing.Color.White;
            this.listLog.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listLog.FormattingEnabled = true;
            this.listLog.HorizontalScrollbar = true;
            this.listLog.Items.AddRange(new object[] {
            "上传记录..."});
            this.listLog.Location = new System.Drawing.Point(507, 9);
            this.listLog.Name = "listLog";
            this.listLog.Size = new System.Drawing.Size(219, 316);
            this.listLog.TabIndex = 7;
            // 
            // btnClearLog
            // 
            this.btnClearLog.Location = new System.Drawing.Point(651, 331);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(75, 23);
            this.btnClearLog.TabIndex = 0;
            this.btnClearLog.Text = "清空记录";
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(503, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1, 345);
            this.label1.TabIndex = 9;
            // 
            // btnDelFile
            // 
            this.btnDelFile.Location = new System.Drawing.Point(96, 333);
            this.btnDelFile.Name = "btnDelFile";
            this.btnDelFile.Size = new System.Drawing.Size(58, 23);
            this.btnDelFile.TabIndex = 10;
            this.btnDelFile.Text = "删除";
            this.btnDelFile.Click += new System.EventHandler(this.btnDelFile_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(13, 333);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(58, 23);
            this.simpleButton1.TabIndex = 11;
            this.simpleButton1.Text = "导入";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // ImgeType
            // 
            this.ImgeType.Location = new System.Drawing.Point(135, 32);
            this.ImgeType.Name = "ImgeType";
            this.ImgeType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ImgeType.Properties.Items.AddRange(new object[] {
            "身份证",
            "行驶证",
            "驾驶证",
            "货运驾驶员从业资格证",
            "车辆营运证",
            "商业险保单",
            "交强险保单"});
            this.ImgeType.Size = new System.Drawing.Size(174, 21);
            this.ImgeType.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(14, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 14);
            this.label2.TabIndex = 13;
            this.label2.Text = "证件类型（必填）：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 14);
            this.label3.TabIndex = 14;
            this.label3.Text = "驾驶员姓名：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(84, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 14);
            this.label4.TabIndex = 15;
            this.label4.Text = "label4";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(198, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 14);
            this.label5.TabIndex = 16;
            this.label5.Text = "驾驶员编号：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(271, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 14);
            this.label6.TabIndex = 17;
            this.label6.Text = "label6";
            // 
            // frmDriverImageFileUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(738, 409);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ImgeType);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.btnDelFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listLog);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.btnClearLog);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.MaximizeBox = false;
            this.Name = "frmDriverImageFileUp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "上传图片";
            this.Load += new System.EventHandler(this.frmDriverImageFileUp_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControl1.Properties)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ImgeType.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnUpload;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraEditors.ProgressBarControl progressBarControl1;
        private System.Windows.Forms.Label lblstate;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private System.Windows.Forms.ListBox listLog;
        private DevExpress.XtraEditors.SimpleButton btnClearLog;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SimpleButton btnDelFile;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraEditors.ComboBoxEdit ImgeType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}