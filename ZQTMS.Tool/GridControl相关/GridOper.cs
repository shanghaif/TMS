using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DevExpress.XtraGrid.Design;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using System.Drawing;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.Utils;
using DevExpress.XtraGrid.Export;
using DevExpress.XtraExport;
using ZQTMS.Lib;
using System.Data;

namespace ZQTMS.Tool
{
    public static class GridOper
    {
        /// <summary>
        /// 设置网格MyGridView的相关属性
        /// </summary>
        /// <param name="gridView"></param>
        /// <param name="gridViewCollection"></param>
        /// <returns></returns>
        public static bool SetGridViewProperty(MyGridView gridView, params MyGridView[] gridViewCollection)
        {
            return true;
        }

        /// <summary>
        /// 加载网格外观
        /// </summary>
        /// <param name="gc">网格控件名</param>
        public static void LoadGridScheme(GridControl gridControl)
        {
            try
            {
                string filePath = Application.StartupPath + "\\DevExpress.XtraGrid.Appearances.xml";
                if (!File.Exists(filePath))
                {
                    filePath = Environment.GetFolderPath(System.Environment.SpecialFolder.System) + "\\DevExpress.XtraGrid.Appearances.xml";
                }
                if (File.Exists(filePath))
                {
                    XAppearances gvScheme = new XAppearances(filePath);
                    gridControl.ForceInitialize();
                    gridControl.KeyDown += (s, e) =>
                        {
                            if (e.Control && e.KeyCode == Keys.C)
                            {
                                e.Handled = true;
                                return;
                            }
                        };

                    foreach (GridView gv in gridControl.Views)
                    {
                        gv.ColumnPanelRowHeight = 30;
                        gv.Appearance.GroupPanel.ForeColor = Color.Black;
                        gv.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Default;
                        gvScheme.LoadScheme("Blue Office", gv); //默认皮肤

                        //设置数字 大于0小于1的，显示第一个0，即0.1
                        gv.CustomColumnDisplayText += (s, e) =>
                            {
                                try
                                {
                                    if (e == null || e.Value == null) return;
                                    if (e.Column.DisplayFormat.FormatType == DevExpress.Utils.FormatType.Numeric)
                                    {
                                        decimal c = ConvertType.ToDecimal(e.Value);
                                        if (c > 0 && c < 1)
                                        {
                                            e.DisplayText = c.ToString();
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    //throw new Exception("计算显示值出错：\r\n" + ex.Message);
                                }
                            };
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 检测文件夹是否存在
        /// </summary>
        private static void CheckDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// 保存网格外观样式
        /// </summary>
        /// <param name="gridView">要保存的网格</param>
        public static void SaveGridLayout(params MyGridView[] gridViews)
        {
            if (gridViews == null || gridViews.Length == 0) return;

            string path = Application.StartupPath + "\\Views\\";
            CheckDirectory(path);
            foreach (MyGridView gridView in gridViews)
            {
                gridView.SaveLayoutToXml(path + gridView.Guid.ToString() + ".xml");
            }
            MsgBox.ShowOK("当前的外观样式已经保存。");
        }

        /// <summary>
        /// 保存网格外观样式
        /// </summary>
        /// <param name="gridControl">要保存的网格</param>
        /// <param name="LayoutName">保存的名称。比如托运单记录网格样式的名字为：托运单记录</param> 
        /// <param name="SaveToDatabase">是否保存到数据库</param>
        public static void SaveGridLayout(GridView gridView, string LayoutName)
        {
            string path = Application.StartupPath + "\\Views\\";
            CheckDirectory(path);
            //LayoutName = LayoutName + "_" + commonclass.userid;
            gridView.SaveLayoutToXml(path + LayoutName + ".xml");

            MsgBox.ShowOK("当前的外观样式已经保存。");
        }

        public static void SaveGridLayout_ex(GridView gridView, string LayoutName)
        {
            string path = Application.StartupPath + "\\Views\\";
            CheckDirectory(path);
            //LayoutName = LayoutName + "_" + commonclass.userid;
            gridView.SaveLayoutToXml(path + LayoutName + ".xml");
        }

        public static void SaveGridLayout(GridView gridView1, string layoutName1, GridView gridView2, string layoutName2)
        {
            string path = Application.StartupPath + "\\Views\\";
            CheckDirectory(path);
            string file1 = path + layoutName1 + ".xml";//"_" + commonclass.userid +
            string file2 = path + layoutName2 + ".xml";//"_" + commonclass.userid + 
            gridView1.SaveLayoutToXml(file1);
            gridView2.SaveLayoutToXml(file2);
            MsgBox.ShowOK("当前的外观样式已经保存。");
        }

        public static void DeleteGridLayout(string layoutName)
        {
            string file = Application.StartupPath + "\\Views\\" + layoutName + ".xml";
            if (!File.Exists(file))
            {
                MsgBox.ShowOK("当前界面没有锁定外观, 不需要删除!");
                return;
            }
            File.Delete(file);
            MsgBox.ShowOK("当前外观删除成功，请关闭本界面并重新打开。");
        }

        public static void DeleteGridLayout(GridView gridView, string layoutName)
        {
            string path = Application.StartupPath + "\\Views\\";
            //LayoutName = LayoutName + "_" + commonclass.userid;
            string file = Application.StartupPath + "\\Views\\" + layoutName + ".xml";
            if (!File.Exists(file))
            {
                MsgBox.ShowOK("当前界面没有锁定外观, 不需要删除!");
                return;
            }
            File.Delete(file);
            MsgBox.ShowOK("当前外观删除成功，请关闭本界面并重新打开。");
        }

        public static void DeleteGridLayout(string layoutName1, params string[] layoutNames)
        {
            string path = Application.StartupPath + "\\Views\\";
            //LayoutName1 = LayoutName1 + "_" + commonclass.userid;
            string file1 = path + layoutName1 + ".xml";
            bool exists1 = File.Exists(file1);
            bool exists2 = true;

            foreach (string item in layoutNames)
            {
                if ((exists2 = File.Exists(path + item + ".xml")) == true)
                {
                    break;//找到一个存在的
                }
            }


            if (exists1 == false && exists2 == false)
            {
                MsgBox.ShowOK("当前界面没有锁定外观, 不需要删除!");
                return;
            }
            File.Delete(file1);

            foreach (string item in layoutNames)
            {
                if (File.Exists(path + item + ".xml"))
                {
                    File.Delete(path + item + ".xml");
                }
            }
            MsgBox.ShowOK("当前外观删除成功，请关闭本界面并重新打开。");
        }

        /// <summary>
        /// 恢复网格外观
        /// </summary>
        /// <param name="gridView">要恢复的网格</param>
        public static void RestoreGridLayout(params MyGridView[] gridViews)
        {
            if (gridViews == null || gridViews.Length == 0) return;
            string LayoutPath = "";

            foreach (MyGridView gridView in gridViews)
            {
                LayoutPath = Application.StartupPath + "\\Views\\" + gridView.Guid.ToString() + ".xml";
                if (File.Exists(LayoutPath))
                {
                    gridView.RestoreLayoutFromXml(LayoutPath);
                }
                gridView.ClearColumnsFilter();
                gridView.Appearance.ColumnFilterButtonActive.BackColor = Color.Yellow;
                gridView.Appearance.ColumnFilterButtonActive.BackColor2 = Color.Yellow;
            }
        }

        /// <summary>
        /// 恢复网格外观
        /// </summary>
        /// <param name="gridControl">要恢复的网格</param>
        /// <param name="LayoutName">网格名称</param>
        public static void RestoreGridLayout(GridView gridView, string layoutName)
        {
            //LayoutName += "_" + commonclass.userid;
            string LayoutPath = Application.StartupPath + "\\Views\\" + layoutName + ".xml";
            if (File.Exists(LayoutPath))
            {
                gridView.RestoreLayoutFromXml(LayoutPath);
            }
            GridView gv = gridView;
            gv.ClearColumnsFilter();
            //if (commonclass.reptitle.Contains("圣安"))
            //    gv.OptionsView.ShowAutoFilterRow = true;
            gv.Appearance.ColumnFilterButtonActive.BackColor = Color.Yellow;
            gv.Appearance.ColumnFilterButtonActive.BackColor2 = Color.Yellow;
        }

        /// <summary>
        /// 导出到Excel文件
        /// </summary>
        /// <param name="gridView">数据网格</param>
        public static void ExportToExcel(GridView gridView)
        {
            ExportToExcel_Plus(gridView, "");
        }

        /// <summary>
        /// 导出到Excel文件
        /// </summary>
        /// <param name="gridView">数据网格</param>
        /// <param name="filename"></param>
        public static void ExportToExcel(GridView gridView, string fileName)
        {
            ExportToExcel_Plus(gridView, fileName);
        }

        private static void ExportToExcel_Plus(GridView gridView, string fileName)
        {
            if (gridView.RowCount == 0)
            {
                MsgBox.ShowOK("没有数据需要导出。请先检索出数据，然后导出。");
                return;
            }
            //userright ur = new userright();
            //if (!ur.GetUserRightDetail("f4"))
            //{
            //    MsgBox.ShowOK("没有数据导出权限。如果确实需要，请联系公司相关负责人员给您授权!");
            //    return;
            //}
            Cursor currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "导出到Microsoft Excel文件【注意：不能覆盖已存在的文件】";
            sfd.Filter = "Microsoft Excel文件(*.xls;*.xlsx)|*.xls;*.xlsx";
            sfd.DefaultExt = "xls";
            sfd.FileName = fileName.Trim() == "" ? "" : fileName;
            GridColumn gc = GetGridViewColumn(gridView, "billstate");//获取状态列

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    GridViewExportLink gvLink = gridView.CreateExportLink(new ExportXlsProvider(sfd.FileName)) as GridViewExportLink;
                    gvLink.ExportAll = true; //true 导出所有 false 导出选中行
                    if (gc != null && gc.Visible) gvLink.ExportCellsAsDisplayText = true;
                    else gvLink.ExportCellsAsDisplayText = false;
                    //if (commonclass.reptitle.Contains("圣安") || commonclass.reptitle.Contains("华泉龙"))
                    //    gvLink.ExportCellsAsDisplayText = true;

                    //gvLink.ExportAppearance.Row.BackColor = Color.White;
                    //gvLink.ExportAppearance.EvenRow.BackColor = Color.White;

                    gvLink.ExportTo(true);
                    Cursor.Current = currentCursor;

                    if (MsgBox.ShowYesNo("导出成功!是否打开文件?\r\n\r\n" + sfd.FileName) == DialogResult.Yes)
                    {
                        try
                        {
                            System.Diagnostics.Process process = new System.Diagnostics.Process();
                            process.StartInfo.FileName = sfd.FileName;
                            process.StartInfo.Verb = "Open";
                            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                            process.Start();
                        }
                        catch
                        {
                            MsgBox.ShowOK("打开失败。您的系统中没有合适的程序打开该文件!");
                        }
                    }
                }
                catch (IOException)
                {
                    MsgBox.ShowOK("导出失败!\r\n文件已打开，无法覆盖!");
                }
                catch (Exception ex)
                {
                    MsgBox.ShowOK("导出失败：\r\n" + ex.Message);
                }
            }
        }

        /// <summary>
        /// 显示gridview的过滤条
        /// </summary>
        /// <param name="gridView"></param>
        public static void ShowAutoFilterRow(params GridView[] gridViews)
        {
            if (gridViews == null || gridViews.Length == 0) return;
            foreach (GridView gridView in gridViews)
                gridView.OptionsView.ShowAutoFilterRow = !gridView.OptionsView.ShowAutoFilterRow;
        }

        /// <summary>
        /// 允许自动过滤
        /// </summary>
        /// <param name="gridView"></param>
        public static void AllowAutoFilter(params GridView[] gridViews)
        {
            if (gridViews == null || gridViews.Length == 0) return;
            foreach (GridView gridView in gridViews)
                gridView.OptionsCustomization.AllowFilter = !gridView.OptionsCustomization.AllowFilter;
        }

        /// <summary>
        /// 获取GridView里指定列
        /// </summary>
        /// <param name="gv"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static GridColumn GetGridViewColumn(GridView gv, string fieldName)
        {
            if (gv == null || string.IsNullOrEmpty(fieldName)) return null;
            foreach (GridColumn gc in gv.Columns)
                if (gc.FieldName.ToLower() == fieldName.ToLower()) return gc;
            return null;
        }

        #region 获取GridView指定值
        /// <summary>
        /// 获取GridView里的指定值
        /// </summary>
        /// <param name="fieldName">字段名(不区分大小写)</param>
        /// <returns>返回指定值</returns>
        public static string GetRowCellValueString(GridView gv, string fieldName)
        {
            if (gv == null || gv.FocusedRowHandle < 0) return "";
            return GetRowCellValueString(gv, gv.FocusedRowHandle, fieldName);
        }

        /// <summary>
        /// 获取GridView里的指定值
        /// </summary>
        /// <param name="rowHandle">第几行,从0开始</param>
        /// <param name="fieldName">字段名(不区分大小写)</param>
        /// <returns>返回指定值</returns>
        public static string GetRowCellValueString(GridView gv, int rowHandle, string fieldName)
        {
            if (gv == null || rowHandle < 0 || string.IsNullOrEmpty(fieldName)) return "";
            return ConvertType.ToString(gv.GetRowCellValue(rowHandle, fieldName));
        }
        #endregion

        #region GridView设置颜色
        /// <summary>
        /// 设置GridView指定显示颜色,获取StyleFormatCondition对象
        /// </summary>
        /// <param name="gv">网格</param>
        /// <param name="fieldName">要设置的列字段名</param>
        /// <param name="obj">设置值</param>
        /// <param name="condition">匹配类型</param>
        /// <param name="color">颜色</param>
        /// <returns></returns>
        public static void CreateStyleFormatCondition(GridView gv, string fieldName, FormatConditionEnum condition, object obj, Color color)
        {
            CreateStyleFormatCondition(gv, fieldName, condition, obj, color, null);
        }

        /// <summary>
        /// 设置GridView指定显示颜色,获取StyleFormatCondition对象
        /// </summary>
        /// <param name="gv">网格</param>
        /// <param name="fieldName">要设置的列字段名</param>
        /// <param name="obj">设置值</param>
        /// <param name="condition">匹配类型</param>
        /// <param name="color">颜色</param>
        /// <param name="expression">表达式</param>
        /// <returns></returns>
        public static void CreateStyleFormatCondition(GridView gv, string fieldName, FormatConditionEnum condition, object obj, Color color, string expression)
        {
            CreateStyleFormatCondition(gv, fieldName, condition, obj, color, true, null);
        }

        /// <summary>
        /// 设置GridView指定显示颜色,获取StyleFormatCondition对象
        /// </summary>
        /// <param name="gv">网格</param>
        /// <param name="fieldName">要设置的列字段名</param>
        /// <param name="obj">设置值</param>
        /// <param name="condition">匹配类型</param>
        /// <param name="color">颜色</param>
        /// <param name="applyToRow">是否应用整行</param>
        /// <param name="expression">表达式</param>
        /// <returns></returns>
        public static void CreateStyleFormatCondition(GridView gv, string fieldName, FormatConditionEnum condition, object obj, Color color, bool applyToRow, string expression)
        {
            if (gv == null || string.IsNullOrEmpty(fieldName)) return;
            GridColumn gc = GetGridViewColumn(gv, fieldName);
            if (gc == null) return;
            StyleFormatCondition styleFormatCondition1 = new StyleFormatCondition();
            styleFormatCondition1.Appearance.BackColor = color;
            styleFormatCondition1.Appearance.Options.UseBackColor = true;
            styleFormatCondition1.ApplyToRow = applyToRow;
            styleFormatCondition1.Column = gc;
            styleFormatCondition1.Condition = condition;
            styleFormatCondition1.Value1 = obj;
            styleFormatCondition1.Expression = expression;

            gv.FormatConditions.Add(styleFormatCondition1);
        }
        #endregion

        /// <summary>
        /// 给网格行添加ID
        /// </summary>
        /// <param name="gv"></param>
        public static void AddGridViewID(GridView gv)
        {
            gv.CustomUnboundColumnData += (ss, ee) => { if (ee.Column.FieldName == "rowid") { ee.Value = (object)(ee.RowHandle + 1); } };
        }
    }
}