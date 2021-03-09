using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace ZQTMS.Tool
{
    public class RepItemImageComboBox
    {
        /// <summary>
        /// 初始化运单状态列的下拉列表
        /// <para>设计界面的时候无需再增加</para>
        /// <para>只需要调用即可，例如cc.GetUnitState(gridColumn2);</para>
        /// </summary>
        /// <param name="gc">要绑定状态的GridColumn列</param>
        public static void GetUnitState(GridColumn gc)
        {
            gc.ColumnEdit = CreateUnitStateItemImageComboBox();
        }

        /// <summary>
        /// 创建RepItemImageComboBox控件,指定
        /// </summary>
        /// <param name="gc">GridColumn</param>
        /// <param name="type">类型:1默认未核销,已核销,部分核销;2默认是,否;3默认启用,未启用;4本地代理/终端代理;5未完毕/已完毕;6未审核/已审核;7未审批/已审批;8未执行/已执行;100自定义</param>
        /// <param name="items"></param>
        public static void GetRepItemImageComboBox(GridColumn gc, int type, Dictionary<int, string> dic)
        {
            RepositoryItemImageComboBox riicb = CreateRepItemImgComboBox();
            if (type == 1)
            {
                riicb.Items.AddRange(new ImageComboBoxItem[] {
                new ImageComboBoxItem("未核销", 0, -1),
                new ImageComboBoxItem("已核销", 1, -1),
                new ImageComboBoxItem("部分核销", 2, -1)});
            }
            else if (type == 2)
            {
                riicb.Items.AddRange(new ImageComboBoxItem[] {
                new ImageComboBoxItem("否", 0, -1),
                new ImageComboBoxItem("是", 1, -1)});
            }
            else if (type == 3)
            {
                riicb.Items.AddRange(new ImageComboBoxItem[] {
                new ImageComboBoxItem("未启用", 0, -1),
                new ImageComboBoxItem("启用", 1, -1)});
            }
            else if (type == 4)
            {
                riicb.Items.AddRange(new ImageComboBoxItem[] {
                new ImageComboBoxItem("本地代理", 0, -1),
                new ImageComboBoxItem("终端代理", 1, -1)});
            }
            else if (type == 5)
            {
                riicb.Items.AddRange(new ImageComboBoxItem[] {
                new ImageComboBoxItem("未完毕", 0, -1),
                new ImageComboBoxItem("已完毕", 1, -1)});
            }
            else if (type == 6)
            {
                riicb.Items.AddRange(new ImageComboBoxItem[] {
                new ImageComboBoxItem("未审核", 0, -1),
                new ImageComboBoxItem("已审核", 1, -1)});
            }
            else if (type == 7)
            {
                riicb.Items.AddRange(new ImageComboBoxItem[] {
                new ImageComboBoxItem("未审批", 0, -1),
                new ImageComboBoxItem("已审批", 1, -1)});
            }
            else if (type == 8)
            {
                riicb.Items.AddRange(new ImageComboBoxItem[] {
                new ImageComboBoxItem("未执行", 0, -1),
                new ImageComboBoxItem("已执行", 1, -1)});
            }
            else if (type == 9)
            {
                riicb.Items.AddRange(new ImageComboBoxItem[] {
                new ImageComboBoxItem("直营", 0, -1),
                new ImageComboBoxItem("加盟", 1, -1)});
            }
            else if (type == 100)//自定义
            {
                if (dic == null || dic.Count == 0) return;
                foreach (int key in dic.Keys)
                {
                    if (!riicb.Items.Contains(key))
                        riicb.Items.Add(new ImageComboBoxItem(dic[key], key, -1));
                }
            }
            gc.ColumnEdit = riicb;
        }

        /// <summary>
        /// 添加下拉列表
        /// </summary>
        /// <param name="gv">要绑定的GridView</param>
        /// <param name="fieldName">要添加下拉的字段名</param>
        /// <returns></returns>
        public static RepositoryItemComboBox CreateRepItemComboBox(GridView gv, string fieldName)
        {
            if (gv == null || string.IsNullOrEmpty(fieldName)) return null;
            GridColumn col = gv.Columns[fieldName];
            if (col == null) return null;

            RepositoryItemComboBox riicb = new RepositoryItemComboBox();
            riicb.AutoHeight = false;
            //riicb.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            col.ColumnEdit = riicb;
            return riicb;
        }

        public static RepositoryItemImageComboBox CreateRepItemImgComboBox()
        {
            RepositoryItemImageComboBox riicb = new RepositoryItemImageComboBox();
            riicb.AutoHeight = false;
            riicb.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            return riicb;
        }

        /// <summary>
        /// 初始化运单状态列的下拉列表
        /// <para>设计界面的时候无需再增加</para>
        /// <para>只需要调用即可，例如：gridColumn2.ColumnEdit = CreateUnitStateItemImageComboBox();</para>
        /// </summary>
        /// <returns></returns>
        private static RepositoryItemImageComboBox CreateUnitStateItemImageComboBox()
        {
            RepositoryItemImageComboBox riicb = CreateRepItemImgComboBox();
            riicb.Items.AddRange(new ImageComboBoxItem[] {
                new ImageComboBoxItem("新开", 0, -1),
                new ImageComboBoxItem("短驳", 1, -1),
                new ImageComboBoxItem("短驳接收", 2, -1),
                new ImageComboBoxItem("再短驳", 3, -1),
                new ImageComboBoxItem("再短驳接收", 4, -1),
                new ImageComboBoxItem("发车", 5, -1),
                new ImageComboBoxItem("又发", 6, -1),
                new ImageComboBoxItem("到货", 7, -1),
                new ImageComboBoxItem("又到", 8, -1),
                new ImageComboBoxItem("中转", 9, -1),
                new ImageComboBoxItem("转送到网点", 10, 7),
                new ImageComboBoxItem("转送接收", 11, -1),
                new ImageComboBoxItem("又转送到网点", 12, -1),
                new ImageComboBoxItem("又转送接收", 13, -1),
                new ImageComboBoxItem("送货", 14, -1),
                new ImageComboBoxItem("送货退回", 15, -1),
                new ImageComboBoxItem("签收", 16, -1),
                new ImageComboBoxItem("分拨干线", 17, -1),
                new ImageComboBoxItem("干线接收", 18, -1),
                new ImageComboBoxItem("分拨终端",19,-1),
                new ImageComboBoxItem("终端接收",20,-1),
                new ImageComboBoxItem("退货", 99, -1),
                new ImageComboBoxItem("作废", 100, -1)});
            return riicb;
        }
    }
}
