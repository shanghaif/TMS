using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Repository;

namespace ZQTMS.Tool
{
    public class RepItemCheckEdit
    {
        /// <summary>
        /// 创建RepositoryItemCheckEdit控件(复选框)
        /// </summary>
        public static void GetRepositoryItemCheckEdit(params GridColumn[] gcs)
        {
            if (gcs == null || gcs.Length == 0) return;

            RepositoryItemCheckEdit riicb = new RepositoryItemCheckEdit();
            riicb.AutoHeight = false;
            riicb.ValueChecked = 1;
            riicb.ValueGrayed = 0;
            riicb.ValueUnchecked = 0;
            //绑定
            foreach (GridColumn gc in gcs)
                gc.ColumnEdit = riicb;
        }
    }
}