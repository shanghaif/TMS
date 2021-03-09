namespace KMS.UI.BaseInfoManage.客户资料
{
    partial class frmCompanyid_Receivables
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCompanyid_Receivables));
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnRetrieve = new DevExpress.XtraEditors.SimpleButton();
            this.myGridView3 = new KMS.Lib.MyGridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.myGridControl1 = new KMS.Lib.MyGridControl();
            this.bandedGridView1 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.gridBand9 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.bandedGridColumn55 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.bandedGridColumn56 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand10 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.bandedGridColumn67 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.bandedGridColumn68 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.bandedGridColumn65 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.bandedGridColumn101 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.bandedGridColumn99 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.bandedGridColumn66 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.bandedGridColumn78 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.bdate = new DevExpress.XtraEditors.DateEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.bar4 = new DevExpress.XtraBars.Bar();
            this.bar5 = new DevExpress.XtraBars.Bar();
            ((System.ComponentModel.ISupportInitialize)(this.myGridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myGridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bandedGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Appearance.Font = new System.Drawing.Font("宋体", 12F);
            this.btnClose.Appearance.Options.UseFont = true;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(973, 43);
            this.btnClose.Name = "btnClose";
            this.btnClose.ShowToolTips = false;
            this.btnClose.Size = new System.Drawing.Size(70, 25);
            this.btnClose.TabIndex = 85;
            this.btnClose.Text = "退出";
            this.btnClose.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Hand;
            // 
            // btnRetrieve
            // 
            this.btnRetrieve.Appearance.Font = new System.Drawing.Font("宋体", 12F);
            this.btnRetrieve.Appearance.Options.UseFont = true;
            this.btnRetrieve.Image = ((System.Drawing.Image)(resources.GetObject("btnRetrieve.Image")));
            this.btnRetrieve.Location = new System.Drawing.Point(821, 42);
            this.btnRetrieve.Name = "btnRetrieve";
            this.btnRetrieve.ShowToolTips = false;
            this.btnRetrieve.Size = new System.Drawing.Size(70, 25);
            this.btnRetrieve.TabIndex = 84;
            this.btnRetrieve.Text = "提取";
            this.btnRetrieve.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Hand;
            this.btnRetrieve.ToolTipTitle = "帮助";
            this.btnRetrieve.Click += new System.EventHandler(this.btnRetrieve_Click);
            // 
            // myGridView3
            // 
            this.myGridView3.ColumnPanelRowHeight = 30;
            this.myGridView3.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1});
            this.myGridView3.GridControl = this.myGridControl1;
            this.myGridView3.GridViewRemark = "单车毛利日清单";
            this.myGridView3.Guid = new System.Guid("880af1da-b320-4fec-9ed7-3e94ea84df52");
            this.myGridView3.Name = "myGridView3";
            this.myGridView3.OptionsView.ColumnAutoWidth = false;
            this.myGridView3.OptionsView.ShowAutoFilterRow = true;
            this.myGridView3.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "序号";
            this.gridColumn1.FieldName = "rowid";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.OptionsColumn.AllowFocus = false;
            this.gridColumn1.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // myGridControl1
            // 
            this.myGridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            gridLevelNode1.RelationName = "Level1";
            this.myGridControl1.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.myGridControl1.Location = new System.Drawing.Point(0, 73);
            this.myGridControl1.MainView = this.bandedGridView1;
            this.myGridControl1.Name = "myGridControl1";
            this.myGridControl1.Size = new System.Drawing.Size(1045, 386);
            this.myGridControl1.TabIndex = 23;
            this.myGridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.bandedGridView1,
            this.myGridView3});
            // 
            // bandedGridView1
            // 
            this.bandedGridView1.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand9,
            this.gridBand10,
            this.gridBand1});
            this.bandedGridView1.ColumnPanelRowHeight = 30;
            this.bandedGridView1.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.bandedGridColumn55,
            this.bandedGridColumn56,
            this.bandedGridColumn65,
            this.bandedGridColumn66,
            this.bandedGridColumn67,
            this.bandedGridColumn68,
            this.bandedGridColumn78,
            this.bandedGridColumn101,
            this.bandedGridColumn99});
            this.bandedGridView1.GridControl = this.myGridControl1;
            this.bandedGridView1.Name = "bandedGridView1";
            this.bandedGridView1.OptionsBehavior.Editable = false;
            this.bandedGridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.bandedGridView1.OptionsView.ColumnAutoWidth = false;
            this.bandedGridView1.OptionsView.ShowAutoFilterRow = true;
            this.bandedGridView1.OptionsView.ShowFooter = true;
            this.bandedGridView1.OptionsView.ShowGroupPanel = false;
            // 
            // gridBand9
            // 
            this.gridBand9.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand9.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand9.Caption = "公司与部门";
            this.gridBand9.Columns.Add(this.bandedGridColumn55);
            this.gridBand9.Columns.Add(this.bandedGridColumn56);
            this.gridBand9.Name = "gridBand9";
            this.gridBand9.Width = 244;
            // 
            // bandedGridColumn55
            // 
            this.bandedGridColumn55.Caption = "公司";
            this.bandedGridColumn55.FieldName = "company";
            this.bandedGridColumn55.Name = "bandedGridColumn55";
            this.bandedGridColumn55.OptionsColumn.ReadOnly = true;
            this.bandedGridColumn55.Visible = true;
            this.bandedGridColumn55.Width = 92;
            // 
            // bandedGridColumn56
            // 
            this.bandedGridColumn56.Caption = "部门";
            this.bandedGridColumn56.FieldName = "BegWeb";
            this.bandedGridColumn56.Name = "bandedGridColumn56";
            this.bandedGridColumn56.OptionsColumn.ReadOnly = true;
            this.bandedGridColumn56.Visible = true;
            this.bandedGridColumn56.Width = 152;
            // 
            // gridBand10
            // 
            this.gridBand10.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand10.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand10.Caption = "当日（万元）";
            this.gridBand10.Columns.Add(this.bandedGridColumn67);
            this.gridBand10.Columns.Add(this.bandedGridColumn68);
            this.gridBand10.Name = "gridBand10";
            this.gridBand10.Width = 174;
            // 
            // bandedGridColumn67
            // 
            this.bandedGridColumn67.Caption = "本日增加总运费";
            this.bandedGridColumn67.FieldName = "PaymentAmout_day";
            this.bandedGridColumn67.Name = "bandedGridColumn67";
            this.bandedGridColumn67.OptionsColumn.ReadOnly = true;
            this.bandedGridColumn67.SummaryItem.FieldName = "HandleFee";
            this.bandedGridColumn67.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            this.bandedGridColumn67.Visible = true;
            this.bandedGridColumn67.Width = 99;
            // 
            // bandedGridColumn68
            // 
            this.bandedGridColumn68.Caption = "本日回款";
            this.bandedGridColumn68.FieldName = "ReturnPayment_day";
            this.bandedGridColumn68.Name = "bandedGridColumn68";
            this.bandedGridColumn68.OptionsColumn.ReadOnly = true;
            this.bandedGridColumn68.SummaryItem.FieldName = "DeliFee";
            this.bandedGridColumn68.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            this.bandedGridColumn68.Visible = true;
            // 
            // gridBand1
            // 
            this.gridBand1.Caption = "                               当月（万元）";
            this.gridBand1.Columns.Add(this.bandedGridColumn65);
            this.gridBand1.Columns.Add(this.bandedGridColumn101);
            this.gridBand1.Columns.Add(this.bandedGridColumn99);
            this.gridBand1.Columns.Add(this.bandedGridColumn66);
            this.gridBand1.Columns.Add(this.bandedGridColumn78);
            this.gridBand1.Name = "gridBand1";
            this.gridBand1.Width = 396;
            // 
            // bandedGridColumn65
            // 
            this.bandedGridColumn65.Caption = "期初余额";
            this.bandedGridColumn65.FieldName = "Open_balance";
            this.bandedGridColumn65.Name = "bandedGridColumn65";
            this.bandedGridColumn65.OptionsColumn.ReadOnly = true;
            this.bandedGridColumn65.SummaryItem.FieldName = "JHFee";
            this.bandedGridColumn65.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            this.bandedGridColumn65.Visible = true;
            // 
            // bandedGridColumn101
            // 
            this.bandedGridColumn101.Caption = "本月增加总运费";
            this.bandedGridColumn101.FieldName = "PaymentAmout_month";
            this.bandedGridColumn101.Name = "bandedGridColumn101";
            this.bandedGridColumn101.Visible = true;
            this.bandedGridColumn101.Width = 96;
            // 
            // bandedGridColumn99
            // 
            this.bandedGridColumn99.Caption = "本月回款";
            this.bandedGridColumn99.FieldName = "ReturnPayment_month";
            this.bandedGridColumn99.Name = "bandedGridColumn99";
            this.bandedGridColumn99.Visible = true;
            // 
            // bandedGridColumn66
            // 
            this.bandedGridColumn66.Caption = "期末余额";
            this.bandedGridColumn66.FieldName = "Close_balance";
            this.bandedGridColumn66.Name = "bandedGridColumn66";
            this.bandedGridColumn66.OptionsColumn.ReadOnly = true;
            this.bandedGridColumn66.SummaryItem.FieldName = "SCFee";
            this.bandedGridColumn66.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            this.bandedGridColumn66.Visible = true;
            // 
            // bandedGridColumn78
            // 
            this.bandedGridColumn78.Caption = "回款率";
            this.bandedGridColumn78.FieldName = "Return_rate";
            this.bandedGridColumn78.Name = "bandedGridColumn78";
            this.bandedGridColumn78.OptionsColumn.ReadOnly = true;
            this.bandedGridColumn78.SummaryItem.FieldName = "ActualSendFee";
            this.bandedGridColumn78.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            this.bandedGridColumn78.Visible = true;
            // 
            // bdate
            // 
            this.bdate.EditValue = new System.DateTime(2006, 1, 26, 0, 0, 0, 0);
            this.bdate.Location = new System.Drawing.Point(55, 47);
            this.bdate.Name = "bdate";
            this.bdate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.bdate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.bdate.Size = new System.Drawing.Size(156, 21);
            this.bdate.TabIndex = 79;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.simpleButton1);
            this.panelControl1.Controls.Add(this.btnClose);
            this.panelControl1.Controls.Add(this.btnRetrieve);
            this.panelControl1.Controls.Add(this.bdate);
            this.panelControl1.Controls.Add(this.label4);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Controls.Add(this.label6);
            this.panelControl1.Controls.Add(this.label7);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1045, 73);
            this.panelControl1.TabIndex = 22;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("宋体", 12F);
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(897, 42);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.ShowToolTips = false;
            this.simpleButton1.Size = new System.Drawing.Size(70, 25);
            this.simpleButton1.TabIndex = 86;
            this.simpleButton1.Text = "导出";
            this.simpleButton1.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Asterisk;
            this.simpleButton1.ToolTipTitle = "帮助";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("宋体", 21F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(158, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 28);
            this.label4.TabIndex = 78;
            this.label4.Text = "2019年";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("宋体", 21F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(251, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 28);
            this.label1.TabIndex = 75;
            this.label1.Text = "12月";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(325, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(403, 29);
            this.label6.TabIndex = 74;
            this.label6.Text = "广州战区应收账款完成进度表";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("宋体", 13F);
            this.label7.Location = new System.Drawing.Point(5, 49);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 18);
            this.label7.TabIndex = 54;
            this.label7.Text = "日期";
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // bar1
            // 
            this.bar1.BarName = "Status bar";
            this.bar1.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Status bar";
            // 
            // bar2
            // 
            this.bar2.BarName = "Status bar";
            this.bar2.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar2.OptionsBar.AllowQuickCustomization = false;
            this.bar2.OptionsBar.DrawDragBorder = false;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Status bar";
            // 
            // bar4
            // 
            this.bar4.BarName = "Status bar";
            this.bar4.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar4.DockCol = 0;
            this.bar4.DockRow = 0;
            this.bar4.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar4.OptionsBar.AllowQuickCustomization = false;
            this.bar4.OptionsBar.DrawDragBorder = false;
            this.bar4.OptionsBar.UseWholeRow = true;
            this.bar4.Text = "Status bar";
            // 
            // bar5
            // 
            this.bar5.BarName = "Status bar";
            this.bar5.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar5.DockCol = 0;
            this.bar5.DockRow = 0;
            this.bar5.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar5.OptionsBar.AllowQuickCustomization = false;
            this.bar5.OptionsBar.DrawDragBorder = false;
            this.bar5.OptionsBar.UseWholeRow = true;
            this.bar5.Text = "Status bar";
            // 
            // frmCompanyid_Receivables
            // 
            this.ClientSize = new System.Drawing.Size(1045, 459);
            this.Controls.Add(this.myGridControl1);
            this.Controls.Add(this.panelControl1);
            this.Name = "frmCompanyid_Receivables";
            this.Text = "应收账款报表";
            this.Load += new System.EventHandler(this.frmCompanyid_Receivables_Load);
            ((System.ComponentModel.ISupportInitialize)(this.myGridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myGridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bandedGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn78;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.SimpleButton btnRetrieve;
        private KMS.Lib.MyGridView myGridView3;
        private KMS.Lib.MyGridControl myGridControl1;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView bandedGridView1;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand9;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn55;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn56;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand10;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn67;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn68;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn65;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn101;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn99;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn66;
        private DevExpress.XtraEditors.DateEdit bdate;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraBars.Bar bar4;
        private DevExpress.XtraBars.Bar bar5;
    }
}
