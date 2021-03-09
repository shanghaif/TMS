using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.Common;
using System.IO;
using DevExpress.XtraGrid.Columns;

namespace ZQTMS.UI
{
    public partial class frmGridViewCol_SelectGuid : BaseForm
    {
        public string guid = "", gridViewRemark = "";
        static List<FormModel> list = new List<FormModel>();

        public frmGridViewCol_SelectGuid()
        {
            InitializeComponent();
        }

        private void frmGridViewCol_SelectGuid_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            GetGridViewCaption();
            GetAllForms();
        }

        /// <summary>
        /// 获取网格本页面网格表头
        /// </summary>
        private void GetGridViewCaption()
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(FormModel));
            HelpAttribute helpatt;

            #region 构建表头
            int k = 0;
            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor property = properties[i];
                helpatt = property.Attributes[typeof(HelpAttribute)] as HelpAttribute;
                if (helpatt.Visible == false)
                {
                    continue;
                }

                GridColumn col = new GridColumn();
                col.FieldName = property.Name;
                col.Caption = helpatt.Caption;
                col.OptionsColumn.AllowEdit = false;
                col.OptionsColumn.AllowFocus = false;
                col.VisibleIndex = k++;
                gridView1.Columns.Add(col);
            }
            #endregion

            #region 构建表头
            k = 0;
            properties = TypeDescriptor.GetProperties(typeof(GridViewModel));
            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor property = properties[i];
                helpatt = property.Attributes[typeof(HelpAttribute)] as HelpAttribute;
                if (helpatt.Visible == false)
                {
                    continue;
                }

                GridColumn col = new GridColumn();
                col.FieldName = property.Name;
                col.Caption = helpatt.Caption;
                col.OptionsColumn.AllowEdit = false;
                col.OptionsColumn.AllowFocus = false;
                col.VisibleIndex = k++;
                gridView2.Columns.Add(col);
            }
            #endregion
        }

        private void GetAllForms()
        {
            if (list.Count > 0)
            {
                gridControl1.DataSource = list;
                gridView1.BestFitColumns();
                panelControl1.Visible = false;
                return;
            }
            Thread th = new Thread((ThreadStart)delegate
            {
                try
                {
                    #region 加载窗体列表
                    string path = Application.StartupPath;

                    List<string> listFiles = new List<string>(Directory.GetFiles(path, "ZQTMS.*.dll", SearchOption.TopDirectoryOnly));
                    listFiles.AddRange(Directory.GetFiles(Path.Combine(path, "Plugin"), "ZQTMS.*.dll", SearchOption.TopDirectoryOnly));
                    listFiles.Add(Path.Combine(path, Assembly.GetExecutingAssembly().GetModules(false)[0].Name));

                    foreach (string item in listFiles)
                    {
                        Assembly ass = Assembly.LoadFile(item);
                        Type[] types = ass.GetExportedTypes();
                        foreach (Type t in types)
                        {
                            if (t.BaseType == typeof(BaseForm))
                            {
                                try
                                {
                                    Form frm = null;
                                    try
                                    {
                                        frm = (Form)Activator.CreateInstance(t);
                                    }
                                    catch (Exception ex)
                                    {
                                        if (ex.InnerException != null && ex.InnerException.Message.Contains("当前线程不在单线程单元中")) //界面上有锐浪报表，报表不支持在线程里加载
                                        {
                                            if (!this.IsHandleCreated) return;
                                            this.Invoke((MethodInvoker)delegate
                                            {
                                                frm = (Form)Activator.CreateInstance(t); //回到主线程加载
                                            });
                                        }
                                        else
                                        {
                                            throw ex;
                                        }
                                    }
                                    FormModel model = new FormModel();
                                    model.FileName = Path.GetFileName(item);
                                    model.FilePath = item;
                                    model.FormName = t.Name;
                                    model.FullName = t.FullName;
                                    model.FormText = frm.Text;
                                    model.FormType = t;

                                    list.Add(model);
                                }
                                catch (Exception ex)
                                {
                                    if (!this.IsHandleCreated) return;
                                    this.Invoke((MethodInvoker)delegate
                                    {
                                        //MsgBox.ShowOK(string.Format("界面加载失败，请检查：{0}/r/n{1}", t.FullName,ex.Message));
                                    });
                                }
                            }
                        }
                    }
                    #endregion

                    if (!this.IsHandleCreated) return;
                    this.Invoke((MethodInvoker)delegate
                    {
                        gridControl1.DataSource = list;
                        for (int i = gridView1.Columns.Count - 1; i >= 0; i--)
                        {
                            gridView1.Columns[i].OptionsColumn.AllowEdit = false;
                            gridView1.Columns[i].OptionsColumn.AllowFocus = false;
                        }
                        gridView1.BestFitColumns();
                        panelControl1.Visible = false;
                    });
                }
                catch (Exception ex)
                {
                    if (!this.IsHandleCreated) return;
                    this.Invoke((MethodInvoker)delegate
                    {
                        panelControl1.Visible = false;
                        MsgBox.ShowError("获取窗体信息失败!\r\n" + ex.Message);
                    });
                }
            });
            th.IsBackground = true;
            //th.SetApartmentState(ApartmentState.STA); 
            th.Start();
        }

        private void gridView1_Click(object sender, EventArgs e)
        {
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle < 0) return;

            Type t = (Type)gridView1.GetRowCellValue(rowhandle, "FormType");
            Form frm = (Form)Activator.CreateInstance(t);

            List<ZQTMS.Lib.MyGridView> list = new List<ZQTMS.Lib.MyGridView>();
            GetMyGridView(frm, list);

            List<GridViewModel> lt = new List<GridViewModel>();
            foreach (ZQTMS.Lib.MyGridView item in list)
            {
                GridViewModel model = new GridViewModel();
                model.Guid = item.Guid;
                model.GridViewRemark = item.GridViewRemark;
                lt.Add(model);
            }
            gridControl2.DataSource = lt;
            for (int i = gridView2.Columns.Count - 1; i >= 0; i--)
            {
                gridView2.Columns[i].OptionsColumn.AllowEdit = false;
                gridView2.Columns[i].OptionsColumn.AllowFocus = false;
            }
            gridView2.BestFitColumns();

        }

        private void GetMyGridView(Control con, List<ZQTMS.Lib.MyGridView> list)
        {
            foreach (Control item in con.Controls)
            {
                if (item.GetType() == typeof(ZQTMS.Lib.MyGridControl))
                {
                    ZQTMS.Lib.MyGridControl grid = (ZQTMS.Lib.MyGridControl)item;
                    try
                    {
                        foreach (ZQTMS.Lib.MyGridView view in grid.ViewCollection)
                        {
                            list.Add(view);
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
                GetMyGridView(item, list);
            }
        }

        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            int rowhandle = gridView2.FocusedRowHandle;
            if (rowhandle < 0) return;
            guid = gridView2.GetRowCellValue(rowhandle, "Guid").ToString();
            gridViewRemark = gridView2.GetRowCellValue(rowhandle, "GridViewRemark") == DBNull.Value || gridView2.GetRowCellValue(rowhandle, "GridViewRemark") == null ? "" : gridView2.GetRowCellValue(rowhandle, "GridViewRemark").ToString();
            this.DialogResult = DialogResult.Yes;
        }
    }


    public class FormModel
    {
        /// <summary>
        /// 窗体所在的文件名
        /// </summary>
        [Help(Caption = "文件名称", Visible = true)]
        public string FileName
        {
            get;
            set;
        }

        /// <summary>
        /// 文件路径
        /// </summary>
        [Help(Caption = "文件路径", Visible = false)]
        public string FilePath
        {
            get;
            set;
        }

        /// <summary>
        /// 实例名称
        /// </summary>
        [Help(Caption = "实例名称", Visible = true)]
        public string FormName
        {
            get;
            set;
        }

        /// <summary>
        /// 窗体类完整名称
        /// </summary>
        [Help(Caption = "完整类名", Visible = true)]
        public string FullName
        {
            get;
            set;
        }

        /// <summary>
        /// 窗体名称
        /// </summary>
        [Help(Caption = "窗体名称", Visible = true)]
        public string FormText
        {
            get;
            set;
        }

        /// <summary>
        /// 窗体类型
        /// </summary>
        [Help(Caption = "窗体类型", Visible = false)]
        public Type FormType
        {
            get;
            set;
        }
    }

    public class GridViewModel
    {
        /// <summary>
        /// 网格Guid
        /// </summary>
        [Help(Caption = "网格Guid", Visible = true)]
        public Guid Guid
        {
            get;
            set;
        }

        /// <summary>
        /// 网格用途备注
        /// </summary>
        [Help(Caption = "网格用途备注", Visible = true)]
        public string GridViewRemark
        {
            get;
            set;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class HelpAttribute : Attribute
    {

        /// <summary>
        /// 显示在网格中的可见性
        /// </summary>
        public virtual bool Visible
        {
            get;
            set;
        }

        /// <summary>
        /// 显示在网格中的标题
        /// </summary>
        public virtual string Caption
        {
            get;
            set;
        }
    }
}
