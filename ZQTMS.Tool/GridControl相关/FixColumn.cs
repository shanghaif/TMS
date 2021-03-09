using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Design;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraBars;
using System.Collections;
using System.Data;

namespace ZQTMS.Tool
{
    public class FixColumn
    {
        private DevExpress.XtraGrid.Views.Grid.GridView pgv;

        /// <summary>
        /// 用于选择冻结列
        /// </summary>
        /// <param name="gridView"></param>
        /// <param name="subItem"></param>
        public FixColumn(GridView gridView, BarSubItem subItem)
        {
            try
            {
                pgv = gridView;
                if (subItem != null)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("caption", typeof(string));
                    dt.Columns.Add("fieldname", typeof(string));

                    for (int i = 0; i < gridView.Columns.Count; i++)
                    {
                        dt.Rows.Add(new object[] { gridView.Columns[i].Caption, gridView.Columns[i].FieldName });
                    }
                    dt.DefaultView.Sort = "caption ASC";
                    dt = dt.DefaultView.ToTable();

                    subItem.ClearLinks();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        BarCheckItem bb = new BarCheckItem();
                        bb.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(menuclick);
                        bb.Caption = dt.Rows[i][0].ToString();
                        bb.Tag = dt.Rows[i][1];
                        subItem.AddItem(bb);
                    }

                    //for (int i = 0; i < gridView.Columns.Count; i++)
                    //{
                    //    BarCheckItem bb = new BarCheckItem();
                    //    bb.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(menuclick);
                    //    bb.Caption = gridView.Columns[i].Caption;
                    //    bb.Tag = gridView.Columns[i].FieldName;
                    //    subItem.AddItem(bb);
                    //}                    
                }
            }
            catch (Exception)
            { }
        }

        private void menuclick(object sender, ItemClickEventArgs e)
        {
            BarCheckItem item = sender as BarCheckItem;
            if (item.Checked)
            {
                FixedColumn(pgv.Columns[item.Tag.ToString()], 1);
            }
            else
            {
                FixedColumn(pgv.Columns[item.Tag.ToString()], 0);
            }
        }

        public void FixedColumn(GridColumn col, int direct)
        {
            if (direct == 0) //取消
            {
                col.Fixed = FixedStyle.None;
                return;
            }
            if (direct == 1)//左
            {
                col.Fixed = FixedStyle.Left;
                col.Visible = true;
                return;
            }
            if (direct == 2)//右
            {
                col.Fixed = FixedStyle.Right;
                col.Visible = true;
                return;
            }
        }
    }
}