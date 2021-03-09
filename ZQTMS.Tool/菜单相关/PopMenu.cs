using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraGrid.Views.Grid;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraTreeList;

namespace ZQTMS.Tool
{
    public static class PopMenu
    {
        /// <summary>
        /// 显示右键菜单
        /// </summary>
        /// <param name="gv"></param>
        /// <param name="e"></param>
        /// <param name="popupMenu1"></param>
        public static void ShowPopupMenu(GridView gv, MouseEventArgs e, PopupMenu popupMenu1)
        {
            if (gv.FocusedRowHandle < 0) return;
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi = gv.CalcHitInfo(e.Location);
            if (hi.InRowCell == true && e.Button == MouseButtons.Right)
            {
                popupMenu1.ShowPopup(Control.MousePosition);
            }
        }

        /// <summary>
        /// 显示右键菜单
        /// <para>调用示例：treeList1.MouseUp += (ss, ee) => { PopMenu.ShowPopupMenu(treeList1, ee, popupMenu1); };</para>
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="e"></param>
        /// <param name="popupMenu1"></param>
        public static void ShowPopupMenu(TreeList tree, MouseEventArgs e, PopupMenu popupMenu1)
        {
            DevExpress.XtraTreeList.TreeListHitInfo hi = tree.CalcHitInfo(e.Location);
            if (hi.HitInfoType == HitInfoType.Cell && hi.Node != null && e.Button == MouseButtons.Right)
            {
                tree.FocusedNode = hi.Node;
                popupMenu1.ShowPopup(popupMenu1.Manager, Control.MousePosition);
            }
        }

        /// <summary>
        /// 显示右键菜单
        /// </summary>
        /// <param name="e"></param>
        /// <param name="popupMenu1"></param>
        public static void ShowPopupMenu(MouseEventArgs e, PopupMenu popupMenu1)
        {
            if (e.Button == MouseButtons.Right)
            {
                popupMenu1.ShowPopup(Control.MousePosition);
            }
        }
    }
}
