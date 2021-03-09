using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraBars;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors;
using System.Drawing;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using ZQTMS.Lib;

namespace ZQTMS.Tool
{
    public class GridViewRowColor
    {
        private Bar bar;
        
        private DropDownButton button;

        /// <summary>
        /// 要把颜色说明显示在工具条上
        /// <param name="bar">显示说明的工具条</param>
        /// </summary>
        public GridViewRowColor(Bar bar)
        {
            this.bar = bar;
            ShowInBar();
        }

        /// <summary>
        /// 要把颜色说明显示在控件上
        /// <param name="control">显示说明的控件</param>
        /// </summary>
        public GridViewRowColor(DropDownButton button)
        {
            this.button = button;
            ShowInButton();
        }

        private void ShowInBar()
        {
            PopupControlContainer pcc = CreatePopupControlContainer();
            if (pcc == null) return;

            bar.Manager.BeginInit();
            BarButtonItem Item = new BarButtonItem();
            Item.Caption = "颜色说明";
            
            //Item.Id = bar.Manager.MaxItemId + 1;
            bar.Manager.Items.Add(Item);

            int flag = 0;
            foreach (BarItemLink link in bar.ItemLinks)
            {
                if (link.Item.Caption.Contains("退出") || link.Item.Caption.Contains("关闭"))
                {
                    link.BeginGroup = true;
                    bar.InsertItem(link, Item);
                    flag++;
                    break;
                }
            }
            if (flag == 0)
            {
                bar.AddItem(Item);
            }

            Item.ActAsDropDown = true;
            Item.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown;
            Item.DropDownControl = pcc;

            bar.Manager.EndInit();
        }

        private void ShowInButton()
        {
            if (button == null) return;

            PopupControlContainer pcc = CreatePopupControlContainer();

            button.DropDownControl = pcc;
            button.ShowArrowButton = false;
        }

        private PopupControlContainer CreatePopupControlContainer()
        {
            BarManager bm = new BarManager();
            if (bar == null)
            {
                bm.Form = button.FindForm();//没有这一句，点击其他地方的时候，popupControlContainer1不关闭
            }
            else
            {
                bm = bar.Manager;
            }

            Form frm;
            if (button != null)
            {
                frm = FindForm(button);
            }
            else
            {
                frm = FindForm(bar.Manager.Form);
            }

            PopupControlContainer pcc = new PopupControlContainer();
            pcc.Manager = bm;
            pcc.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            pcc.Visible = false;

            #region 加载颜色
            List<GridView> list = new List<GridView>();
            GetGridView(frm, list);

            int LocationY = 0, pccwidith = 0, pccheight = 0;
            foreach (GridView gv in list)
            {
                foreach (StyleFormatCondition sfc in gv.FormatConditions)
                {
                    GridColumn col = sfc.Column;
                    if (col == null) continue;
                    if (sfc.Condition == FormatConditionEnum.None) continue;

                    UCLabelColor lc = new UCLabelColor();
                    lc.Location = new Point(0, LocationY);
                    lc.ItemColor = sfc.Appearance.BackColor;
                    lc.ItemText = col.Caption + GetConditionString(sfc);
                    pcc.Controls.Add(lc);
                    LocationY += lc.Size.Height;
                    pccwidith = lc.Size.Width;
                    pccheight += lc.Size.Height;
                }
            }
            pcc.Size = new Size(pccwidith, pccheight);

            if (pccheight == 0)
            {
                return null;//表示此此界面没有StyleFormatCondition
            }
            #endregion

            return pcc;
        }

        private Form FindForm(Control c)
        {
            if (c is Form)
                return c as Form;
            else if (c.Parent != null)
                return FindForm(c.Parent);
            else
                return null;
        }

        private void GetGridView(Control con, List<GridView> list)
        {
            foreach (Control item in con.Controls)
            {
                if (item.GetType() == typeof(GridControl))
                {
                    GridControl gc = item as GridControl;
                    foreach (GridView gv in gc.Views)
                    {
                        list.Add(gv);
                    }
                }
                else
                {
                    GetGridView(item, list);
                }
            }
        }

        private string GetConditionString(StyleFormatCondition sfc)
        {
            switch (sfc.Condition)
            {
                case FormatConditionEnum.Between:
                    return "在" + GetConditionValue(sfc, 1) + "和" + GetConditionValue(sfc, 2) + "之间";

                case FormatConditionEnum.Equal:
                    return "等于" + GetConditionValue(sfc, 1);

                case FormatConditionEnum.Expression:
                    return "";

                case FormatConditionEnum.Greater:
                    return "大于" + GetConditionValue(sfc, 1);

                case FormatConditionEnum.GreaterOrEqual:
                    return "大于等于" + GetConditionValue(sfc, 1);

                case FormatConditionEnum.Less:
                    return "小于" + GetConditionValue(sfc, 1);

                case FormatConditionEnum.LessOrEqual:
                    return "小于等于" + GetConditionValue(sfc, 1);

                case FormatConditionEnum.NotBetween:
                    return "不在" + GetConditionValue(sfc, 1) + "和" + GetConditionValue(sfc, 2) + "之间";

                case FormatConditionEnum.NotEqual:
                    return "不等于" + GetConditionValue(sfc, 1);
            }
            return "";
        }

        private string GetConditionValue(StyleFormatCondition sfc, int selectvalue)
        {
            try
            {
                if (sfc.Column == null)
                {
                    return "";
                }
                if (sfc.Column.ColumnEdit != null)
                {
                    if (sfc.Column.ColumnEdit.GetType() == typeof(RepositoryItemImageComboBox))
                    {
                        RepositoryItemImageComboBox riicb = sfc.Column.ColumnEdit as RepositoryItemImageComboBox;

                        int value = Convert.ToInt32(selectvalue == 1 ? sfc.Value1 : sfc.Value2);
                        ImageComboBoxItem icb = riicb.Items.GetItem(value);

                        return icb.Description;
                    }
                    else if (sfc.Column.ColumnEdit.GetType() == typeof(RepositoryItemCheckEdit))
                    {
                        //RepositoryItemCheckEdit riicb = sfc.Column.ColumnEdit as RepositoryItemCheckEdit;

                        object value = selectvalue == 1 ? sfc.Value1 : sfc.Value2;

                        return value.ToString();
                    }

                    return "";
                }
                else
                {
                    return (selectvalue == 1 ? sfc.Value1 : sfc.Value2).ToString();
                }
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
