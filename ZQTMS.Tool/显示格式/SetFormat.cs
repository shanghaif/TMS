using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Utils;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraGrid.Columns;

namespace ZQTMS.Tool
{
    public static class SetFormat
    {
        /// <summary>
        /// 设置某列的显示格式和合计格式
        /// </summary>
        public static void SetGridColumnDisplayFormat(GridView gridView, string fieldName, FormatType formatType, string formatString, SummaryItemType summaryType, string summaryDisplayFormat)
        {
            if (gridView == null) return;
            if (string.IsNullOrEmpty(fieldName)) return;
            GridColumn col = gridView.Columns[fieldName];
            if (col == null) return;
            col.DisplayFormat.FormatType = formatType;
            col.DisplayFormat.FormatString = formatString;
            col.SummaryItem.SummaryType = summaryType;
            col.SummaryItem.DisplayFormat = summaryDisplayFormat;
        }

        /// <summary>
        /// 设置多列为统一的显示格式和合计格式
        /// </summary>
        public static void SetColsDisplayFormat(GridView gridView, string fieldsName, FormatType formatType, string formatString, SummaryItemType summaryType, string summaryDisplayFormat)
        {
            if (gridView == null) return;
            if (string.IsNullOrEmpty(fieldsName)) return;
            string[] arrFieldsName = fieldsName.Split(',');
            foreach (string fieldName in arrFieldsName)
            {
                SetGridColumnDisplayFormat(gridView, fieldName.Trim(), formatType, formatString, summaryType, summaryDisplayFormat);
            }
        }
        /// <summary>
        /// 设置某个文本框编辑和显示格式
        /// </summary>
        public static void SetTextEditFormat(TextEdit textEdit, FormatType formatType, string formatString, MaskType maskType, string editMask)
        {
            if (textEdit == null) return;
            textEdit.Properties.DisplayFormat.FormatType = formatType;
            textEdit.Properties.DisplayFormat.FormatString = formatString;
            textEdit.Properties.EditFormat.FormatType = formatType;
            textEdit.Properties.EditFormat.FormatString = formatString;
            textEdit.Properties.Mask.MaskType = maskType;
            textEdit.Properties.Mask.EditMask = editMask;
            textEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
        }

        /// <summary>
        /// 设置多个文本框编辑和显示格式
        /// </summary>
        public static void SetTextEditFormat(TextEdit[] textEdits, FormatType formatType, string formatString, MaskType maskType, string editMask)
        {
            if (textEdits == null) return;
            if (textEdits.Length <= 0) return;
            foreach (TextEdit textEdit in textEdits)
            {
                SetTextEditFormat(textEdit, formatType, formatString, maskType, editMask);
            }
        }
    }
}
