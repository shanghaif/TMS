using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraBars;
using System.Reflection;
using System.Windows.Forms;

namespace ZQTMS.Tool
{
    public static class BarMagagerOper
    {
        /// <summary>
        /// 设置工具条基本属性
        /// </summary>
        /// <param name="bar1"></param>
        public static void SetBarPropertity(Bar bar, params Bar[] bars)
        {
            List<Bar> list = new List<Bar>() { bar };
            foreach (Bar item in bars)
            {
                list.Add(item);
            }
            foreach (Bar bar1 in list)
            {
                bar1.BarItemVertIndent = 9;
                bar1.OptionsBar.AllowQuickCustomization = false;
                bar1.OptionsBar.DrawDragBorder = false;
                bar1.OptionsBar.UseWholeRow = true;
                bar1.Text = "工具栏";
                bar1.Manager.AllowCustomization = false;
                bar1.Manager.AllowShowToolbarsPopup = false;

                foreach (BarItem bi in bar1.Manager.Items)
                {
                    if (bi.GetType() == typeof(BarButtonItem) && bi.Caption == "快找")
                        bi.ItemClick += BarButtonItemShowBillSearch_ItemClick;
                }
            }
        }

        /// <summary>
        /// 打开快找事件(仅工具栏的快找)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void BarButtonItemShowBillSearch_ItemClick(object sender, ItemClickEventArgs e)
        {
            Assembly ass = Assembly.LoadFrom(Application.StartupPath + "\\Plugin\\ZQTMS.UI.BuisinessManage.dll");
            Type type = ass.GetType("ZQTMS.UI.frmBillSearchControl");
            Form frm = (Form)Activator.CreateInstance(type);
            frm.Show();
        }
    }
}
