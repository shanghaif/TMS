using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
//using ZQTMS.Common;
using ZQTMS.SqlDAL;
using System.Threading;
using ZQTMS.Common;


namespace ZQTMS.Tool
{
    public partial class BaseForm : XtraForm
    {
        //private Guid _guid = Guid.NewGuid();
        //public Guid Guid
        //{
        //    get
        //    {
        //        return _guid;
        //    }
        //    set
        //    {
        //        _guid = value;
        //    }
        //}

        public BaseForm()
        {
            InitializeComponent();
        }

        /// <summary> 
        /// 做一个标记,避免重复设置Enter事件 
        /// </summary> 
        private bool m_AttachProcessed = false;
        /// <summary> 
        /// 在Load事件中遍历控件,如果是文本框,自动切换到半角状态 
        /// </summary> 
        /// <param name="e"></param> 
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!m_AttachProcessed)
            {
                //this.Icon = new Icon("favicon.icon");
                SetImeToHangul(this.Controls);
                RegDateEvent(this);
                SetGridViewNumberFormat(this);
                m_AttachProcessed = true;
            }
            IsReStart();
        }

        /// <summary> 
        /// 通过递归,遍历当前窗口的全部控件 
        /// </summary> 
        /// <param name="p_Controls">容器</param> 
        private void SetImeToHangul(System.Windows.Forms.Control.ControlCollection p_Controls)
        {
            foreach (System.Windows.Forms.Control ctl in p_Controls)
            {
                if (ctl.GetType() == typeof(DevExpress.XtraBars.BarDockControl))
                {
                    DevExpress.XtraBars.BarManager bm = (ctl as DevExpress.XtraBars.BarDockControl).Manager;
                    if (bm != null && bm.Bars.Count > 0)
                    {
                        GridViewRowColor gvr = new GridViewRowColor(bm.Bars[0]);
                    }
                    break;
                }
                XtraTabControl tabcontrol = ctl as XtraTabControl;
                if (tabcontrol != null)
                {
                    SetImeToHangul(tabcontrol.Controls);
                    continue;
                }
                XtraTabPage tabpage = ctl as XtraTabPage;
                if (tabpage != null)
                {
                    SetImeToHangul(tabpage.Controls);
                    continue;
                }
                GroupControl groupControl = ctl as GroupControl;
                if (groupControl != null)
                {
                    SetImeToHangul(groupControl.Controls);
                    continue;
                }

                PanelControl panel = ctl as PanelControl;
                {
                    if (panel != null)
                    {
                        SetImeToHangul(panel.Controls);
                        continue;
                    }
                }
                //文本框进入时,自动切换到半角,如果要控制其他可输入控件,参照下面的代码完成 
                TextEdit txtbox = ctl as TextEdit;
                if (txtbox != null)
                {
                    txtbox.Enter += new EventHandler(ControlEnter_Enter);
                }

                ComboBoxEdit combo = ctl as ComboBoxEdit;
                if (combo != null)
                {
                    combo.Enter += new EventHandler(ControlEnter_Enter);
                }
            }
        }

        private void ControlEnter_Enter(object sender, EventArgs e)
        {
            Control ctl = sender as Control;
            if (ctl == null)
                return;
            if (ctl.ImeMode != ImeMode.Hangul)
                ctl.ImeMode = ImeMode.Hangul;
        }

        #region 注册DateEdit的相关时间。解决：如果选了日期控件的“今天”按钮，自动变成0:00:00，通过这2个事件解决
        private void RegDateEvent(Control Con)
        {
            if (Con.GetType() == typeof(DevExpress.XtraEditors.DateEdit))
            {
                ((DevExpress.XtraEditors.DateEdit)Con).Popup += new EventHandler(DateEdit_Popup);
                ((DevExpress.XtraEditors.DateEdit)Con).Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(DateEdit_Closed);
                ((DevExpress.XtraEditors.DateEdit)Con).EditValueChanged += new EventHandler(DateEdit_EditValueChanged);
                ((DevExpress.XtraEditors.DateEdit)Con).Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.False;
            }
            foreach (System.Windows.Forms.Control ctl in Con.Controls)
            {
                RegDateEvent(ctl);
            }
        }

        private void DateEdit_Popup(object sender, EventArgs e)
        {
            try
            {
                //((PopupDateEditForm)(((DevExpress.Utils.Win.IPopupControl)sender).PopupWindow)).Calendar.TodayButton;
                DevExpress.XtraEditors.DateEdit edate = sender as DevExpress.XtraEditors.DateEdit;
                edate.Tag = edate.DateTime;
            }
            catch (Exception) { }
        }

        public void DateEdit_Closed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
        {
            try
            {
                DevExpress.XtraEditors.DateEdit edate = sender as DevExpress.XtraEditors.DateEdit;
                if (edate.Tag.GetType() == typeof(DateTime))
                {
                    if (edate.DateTime.Date.Equals(Convert.ToDateTime("0001-1-1 0:00:00"))) return;

                    DateTime dt = Convert.ToDateTime(edate.Tag);
                    if (dt.Date.Equals(Convert.ToDateTime("0001-1-1 0:00:00")))
                    {
                        //dt = commonclass.gcdate;
                    }
                    edate.DateTime = edate.DateTime.Date.AddHours(dt.Hour).AddMinutes(dt.Minute).AddSeconds(dt.Second);
                }
            }
            catch (Exception) { }
        }

        //手工删除控件中的日期或者点击清除按钮，默认为1950-1-1
        private void DateEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                DevExpress.XtraEditors.DateEdit edate = sender as DevExpress.XtraEditors.DateEdit;
                if (edate.DateTime.Date.Equals(Convert.ToDateTime("0001-1-1 0:00:00")))
                {
                    edate.DateTime = Convert.ToDateTime("1950-1-1");
                }
            }
            catch (Exception) { }
        }
        #endregion

        #region 设置数字 大于0小于1的，显示第一个0，即0.1
        /// <summary>
        /// 设置数字显示格式
        /// </summary>
        private void SetGridViewNumberFormat(Control Con)
        {
            if (Con.GetType() == typeof(DevExpress.XtraGrid.GridControl) || Con.GetType() == typeof(ZQTMS.Lib.MyGridControl))
            {
                ColumnView cv = (Con as GridControl).MainView as ColumnView;
                foreach (GridColumn col in cv.Columns)
                {
                    col.OptionsFilter.FilterPopupMode = FilterPopupMode.CheckedList;
                }
                cv.OptionsFilter.MaxCheckedListItemCount = 200; //默认25个
                cv.OptionsFilter.ColumnFilterPopupRowCount = 200;
                cv.OptionsFilter.ColumnFilterPopupMaxRecordsCount = -1;

                cv.CustomColumnDisplayText += new CustomColumnDisplayTextEventHandler(BaseForm_CustomColumnDisplayText);
                return;
            }
            foreach (System.Windows.Forms.Control ctl in Con.Controls)
            {
                SetGridViewNumberFormat(ctl);
            }
        }

        void BaseForm_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e == null) return;
            if (e.Column.DisplayFormat.FormatType == DevExpress.Utils.FormatType.Numeric)
            {
                try
                {
                    e.DisplayText = ConvertType.ToDecimal(e.Value, "");
                }
                catch { }
            }
            #region zaj 2017当gridview中列为时间格式并且值中包含1900则显示空字符串
            if (e.Column.DisplayFormat.FormatType == DevExpress.Utils.FormatType.DateTime)
            {
                try
                {

                    e.DisplayText = ToDateTime(e.Value, "");

                }
                catch (Exception ex)
                {

                    MsgBox.ShowException(ex);
                }
            }
            #endregion
        }
        public static string ToDateTime(object o, string r)
        {
            string s = (o + "").Trim();
            if (s == "") return r;

            try
            {
                DateTime d = Convert.ToDateTime(s);
                string d2 = d.ToString();
                return d2.IndexOf("1900") >= 0 ? r : d2;
            }
            catch
            {
                return r;
            }
        }
        /// <summary>
        /// 检测是否需要重启
        /// </summary>
        private void IsReStart()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SysUserInfo_IsRestart", list);

                if (SqlHelper.GetDataSet(sps).Tables[0].Rows.Count > 0)
                {
                    MsgBox.ShowOK("系统资料有更新，需要重新启动，点击确定后继续!");
                    Application.ExitThread();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                return;
            }
        }
     


        #endregion
    }
}