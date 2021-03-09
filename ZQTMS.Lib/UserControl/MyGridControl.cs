using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using DevExpress.XtraGrid.Columns;

namespace ZQTMS.Lib
{
    [ToolboxItem(true)]
    public partial class MyGridControl : DevExpress.XtraGrid.GridControl
    {
        protected override DevExpress.XtraGrid.Views.Base.BaseView CreateDefaultView()
        {
            return CreateView("MyGridView");
        }
        protected override void RegisterAvailableViewsCore(DevExpress.XtraGrid.Registrator.InfoCollection collection)
        {
            base.RegisterAvailableViewsCore(collection);
            collection.Add(new MyGridViewInfoRegistrator());
        }
        //public override DevExpress.XtraGrid.Views.Base.ViewInfo.BaseViewInfo CreateViewInfo(BaseView view)
        //{
        //    return new MyGridViewInfo(view as MyGridView);
        //}
        //public override DevExpress.XtraGrid.Views.Base.Handler.BaseViewHandler CreateHandler(BaseView view)
        //{
        //    return new MyGridHandler(view as MyGridView);
        //}
    }

    public class MyGridViewInfoRegistrator : DevExpress.XtraGrid.Registrator.GridInfoRegistrator
    {
        public override string ViewName { get { return "MyGridView"; } }
        public override DevExpress.XtraGrid.Views.Base.BaseView CreateView(DevExpress.XtraGrid.GridControl grid)
        {
            return new MyGridView(grid as DevExpress.XtraGrid.GridControl);
        }
    }

    [Designer(typeof(CDesigner), typeof(IDesigner)), Editor(typeof(CEditor), typeof(ComponentEditor))]
    public class MyGridView : DevExpress.XtraGrid.Views.Grid.GridView
    {
        /// <summary>
        ///函数
        /// </summary>
        /// <param name="ownerGrid"></param>
        public MyGridView(DevExpress.XtraGrid.GridControl ownerGrid)
            : base(ownerGrid)
        {
            this.OptionsView.ShowAutoFilterRow = true; //显示筛选
            this.OptionsView.ShowGroupPanel = false; //隐藏分组区
            this.OptionsView.ColumnAutoWidth = false; //是否自动列宽
            //this.OptionsBehavior.Editable = false; //禁止编辑
            this.ColumnPanelRowHeight = 30;//表头高度

            #region 不使用的自定义属性
            //this.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never; //是否显示过滤面板

            //this.OptionsView.EnableAppearanceEvenRow = false; //是否启用偶数行外观
            //this.OptionsView.EnableAppearanceOddRow = false; //是否启用奇数行外观

            //this.OptionsCustomization.AllowColumnMoving = false; //是否允许移动列
            //this.OptionsCustomization.AllowColumnResizing = false; //是否允许调整列宽
            //this.OptionsCustomization.AllowGroup = false; //是否允许分组
            //this.OptionsCustomization.AllowFilter = false; //是否允许过滤
            //this.OptionsCustomization.AllowSort = true; //是否允许排序 
            #endregion
        }

        /// <summary>
        /// 函数
        /// </summary>
        public MyGridView()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public object GetRowsInfo()
        {
            return this.ViewInfo.RowsInfo;
        }

        public object GetGridCellInfo(int RowHander, int index)
        {
            return this.ViewInfo.GetGridCellInfo(RowHander, index);
        }

        public override void EndInit()
        {
            base.EndInit();
            this.SelectionChanged += MyGridView_SelectionChanged;
            //this.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(gridView1_CustomDrawRowIndicator);
            //this.IndicatorWidth = 30;

            this.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(gridView1_CustomUnboundColumnData);
            if (this.Columns["rowid"] != null) return;
            GridColumn col = new GridColumn();
            col.FieldName = "rowid";
            col.Caption = "序号";
            col.OptionsColumn.AllowEdit = col.OptionsColumn.AllowFocus = false;
            col.Visible = true;
            col.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            col.VisibleIndex = 0;
            this.Columns.Add(col);

        }
        void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e == null || e.RowHandle < 0 || e.Column.FieldName != "rowid") return;
            e.Value = (object)(e.RowHandle + 1);
        }

        void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = e.RowHandle.ToString();
            }
        }

        void MyGridView_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            int[] grvarry = this.GetSelectedRows();
            if (grvarry.Length > 1)
            {
                GetDataRowList = new DataRow[grvarry.Length];
                for (int i = 0; i < grvarry.Length; i++)
                {
                    GetDataRowList[i] = this.GetDataRow(grvarry[i]);
                }
                GetList = grvarry;

            }
            else
            {
                GetDataRowInfo = this.GetFocusedDataRow();
            }
        }

        #region 私有属性
        private string _gridViewRemark = "";
        #endregion

        #region 自定义属性
        /// <summary>
        /// 获取用户选择行数据
        /// </summary>
        [Category("自定义属性"), Browsable(false), DefaultValue(null)]
        public DataRow GetDataRowInfo { get; set; }
        /// <summary>
        /// 返回选择行数组
        /// </summary>
        [Category("自定义属性"), Browsable(false), DefaultValue(null)]
        public DataRow[] GetDataRowList { get; set; }
        /// <summary>
        /// 返回用户选择行索引
        /// </summary>
        [Category("自定义属性"), Browsable(false), DefaultValue(null)]
        public int[] GetList { get; set; }

        private Guid _guid = Guid.NewGuid();

        private string _BindFiledName = string.Empty;
        private string _MenuName = string.Empty;
        private Dictionary<string, object> _HandFiledDic = new Dictionary<string, object>();
        /// <summary>
        /// 当前GridView的唯一标识(Guid)
        /// </summary>
        [Editor(typeof(ButtonEditor), typeof(UITypeEditor))]
        [Category("自定义属性"), Description("当前GridView的唯一标识(Guid)")] //, DisplayName("GridView的唯一标识")
        public Guid Guid
        {
            get
            {
                return _guid;
            }
            set
            {
                //if (value.Equals(Guid.Empty))
                //{
                //    throw new Exception("Guid不能为空!");
                //}
                _guid = value;
            }
        }

        //string _fileName = "";
        //[Editor(typeof(ButtonEditor), typeof(UITypeEditor))]
        //public String 文件
        //{
        //    get { return _fileName; }
        //    set { _fileName = value; }
        //}  

        /// <summary>
        /// 当前GridView的用途
        /// </summary>
        [Category("自定义属性"), Description("当前GridView的用途"), DefaultValue("")]
        public string GridViewRemark
        {
            get
            {
                return _gridViewRemark;
            }
            set
            {
                _gridViewRemark = value;
            }
        }

        // 针对网格隐藏屏蔽字段
        /// <summary>
        /// 绑定开单网点的字段名
        /// </summary>
        public string WebControlBindFindName
        {
            get
            {
                return _BindFiledName;
            }
            set
            {
                _BindFiledName = value;
            }
        }

        /// <summary>
        /// 设置隐藏屏蔽的字段名
        /// </summary>
        public Dictionary<string, object> HiddenFiledDic
        {
            get
            {
                return _HandFiledDic;
            }
            set
            {
                _HandFiledDic = value;
            }
        }
        /// <summary>
        /// 需要收隐藏字段模块限制的菜单
        /// </summary>
        public string MenuName
        {
            get
            {
                return _MenuName;
            }
            set
            {
                _MenuName = value;
            }
        }

        /// <summary>
        /// 是否设计模式
        /// </summary>
        protected new bool DesignMode
        {
            get
            {
                bool returnFlag = false;
#if DEBUG
                if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                {
                    returnFlag = true;
                }
                else if (System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToUpper().Equals("DEVENV"))
                {
                    returnFlag = true;
                }
#endif
                return returnFlag;
            }
        }

        #region 生成的结果，关掉界面之后会恢复原状，暂时取消

        //[DefaultValue(30)]
        //public override int ColumnPanelRowHeight
        //{
        //    get
        //    {
        //        return base.ColumnPanelRowHeight;
        //    }
        //    set
        //    {
        //        base.ColumnPanelRowHeight = value;
        //    }
        //}

        //protected override ColumnViewOptionsView CreateOptionsView()
        //{
        //    return new CustomGridOptionsView();
        //}

        //protected override ColumnViewOptionsBehavior CreateOptionsBehavior()
        //{
        //    return new CustomGridOptionsBehavior();
        //} 
        #endregion


        #endregion
    }

    public class CustomGridOptionsView : GridOptionsView
    {
        public CustomGridOptionsView()
        {
            this.ShowAutoFilterRow = true;
            this.ShowGroupPanel = false;
            this.ColumnAutoWidth = false;
        }


        [DefaultValue(true)]
        public override bool ShowAutoFilterRow
        {
            get
            {
                return base.ShowAutoFilterRow;
            }
            set
            {
                base.ShowAutoFilterRow = value;
            }
        }

        [DefaultValue(false)]
        public override bool ShowGroupPanel
        {
            get
            {
                return base.ShowGroupPanel;
            }
            set
            {
                base.ShowGroupPanel = value;
            }
        }

        [DefaultValue(false)]
        public override bool ColumnAutoWidth
        {
            get
            {
                return base.ColumnAutoWidth;
            }
            set
            {
                base.ColumnAutoWidth = value;
            }
        }
    }

    public class CustomGridOptionsBehavior : GridOptionsBehavior
    {
        public CustomGridOptionsBehavior()
        {
            this.Editable = false;
        }

        [DefaultValue(false)]
        public override bool Editable
        {
            get
            {
                return base.Editable;
            }
            set
            {
                base.Editable = value;
            }
        }
    }

    public class MyGridHandler : DevExpress.XtraGrid.Views.Grid.Handler.GridHandler
    {
        public MyGridHandler(DevExpress.XtraGrid.Views.Grid.GridView gridView) : base(gridView) { }

        //protected override void OnKeyDown(KeyEventArgs e)
        //{
        //    base.OnKeyDown(e);
        //    if (e.KeyData == Keys.Delete && View.State == GridState.Normal)
        //        View.DeleteRow(View.FocusedRowHandle);
        //}
    }

    public class MyGridViewInfo : DevExpress.XtraGrid.Views.Grid.ViewInfo.GridViewInfo
    {
        public MyGridViewInfo(DevExpress.XtraGrid.Views.Grid.GridView gridView) : base(gridView) { }

        public override int CalcRowHeight(Graphics graphics, int rowHandle, int min, int level, bool useCache, DevExpress.XtraGrid.Views.Grid.ViewInfo.GridColumnsInfo columns)
        {
            return base.CalcRowHeight(graphics, rowHandle, MinRowHeight, level, useCache, columns);
        }

        public override int MinRowHeight
        {
            get
            {
                return base.MinRowHeight - 2;
            }
        }
    }

    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public class CDesigner : DevExpress.XtraGrid.Design.BaseViewDesigner //ComponentDesigner
    {
        public CDesigner()
            : base()
        {
        }

        private DesignerActionListCollection _actionLists = null;

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (_actionLists == null)
                {
                    _actionLists = new DesignerActionListCollection();
                    _actionLists.AddRange(base.ActionLists);

                    // Add a custom DesignerActionList
                    _actionLists.Add(new ActionList(this));
                }
                return _actionLists;
            }
        }

        public class ActionList : DesignerActionList
        {
            private CDesigner _parent;
            private DesignerActionItemCollection _items;

            public ActionList(CDesigner parent)
                : base(parent.Component)
            {
                _parent = parent;
            }

            public override DesignerActionItemCollection GetSortedActionItems()
            {
                if (_items == null)
                {
                    _items = new DesignerActionItemCollection();
                    _items.Add(new DesignerActionMethodItem(this, "RefreshGuid", "刷新Guid属性", true));
                }
                return _items;
            }

            private void RefreshGuid()
            {
                MyGridView gridView = null;
                if (_parent.Component.GetType() == typeof(MyGridView))
                {
                    gridView = (MyGridView)_parent.Component;
                }
                if (gridView == null)
                {
                    MessageBox.Show("获取GridView对象失败，无法刷新!");
                    return;
                }

                PropertyDescriptor propDesc = TypeDescriptor.GetProperties(gridView)["Guid"]; //更改Guid字段的值
                if (propDesc == null)
                {
                    MessageBox.Show("获取GridView对象的Guid字段失败\r\n可能字段已更改!");
                    return;
                }

                object obj = propDesc.GetValue(gridView);
                if (obj.GetType() == typeof(Guid) && !((Guid)obj).Equals(Guid.Empty))
                {
                    string msg = string.Format("当前控件的Guid值不为空，是否继续刷新？\r\n\r\n如果继续，原值将被复制到剪贴板：\r\n【{0}】", gridView.Guid);
                    DialogResult dr = MessageBox.Show(msg, "刷新提醒", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (dr != DialogResult.Yes)
                    {
                        return;
                    }
                    Clipboard.SetText(gridView.Guid.ToString());
                }

                propDesc.SetValue(gridView, Guid.NewGuid());
                MessageBox.Show("刷新完成!");
            }
        }
    }

    public class CEditor : ComponentEditor
    {
        public override bool EditComponent(ITypeDescriptorContext context, object component)
        {
            return true;
        }
    }

    /// <summary>
    /// 针对Guid字段的Editor
    /// </summary>
    public class ButtonEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                return UITypeEditorEditStyle.Modal;
            }
            return base.GetEditStyle(context);
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (edSvc != null && value != null)
            {
                if (!string.IsNullOrEmpty(value.ToString()))
                {
                    string msg = string.Format("当前控件的Guid值不为空，是否继续刷新？\r\n\r\n如果继续，原值将被复制到剪贴板：\r\n【{0}】", value.ToString());
                    DialogResult dr = MessageBox.Show(msg, "刷新提醒", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (dr != DialogResult.Yes)
                    {
                        return value;
                    }
                    Clipboard.SetText(value.ToString());
                }
                return Guid.NewGuid();
            }
            return value;
        }
    }
}