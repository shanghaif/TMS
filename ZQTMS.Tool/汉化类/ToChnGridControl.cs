using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraGrid.Localization;

namespace ZQTMS.Tool
{
    public class ToChnGridControl : GridLocalizer
    {
        public override string GetLocalizedString(GridStringId id)
        {
            switch (id)
            {
                case GridStringId.FileIsNotFoundError: return "文件 {0} 未找到";
                case GridStringId.ColumnViewExceptionMessage: return " 确定修正数据?";
                case GridStringId.CustomizationCaption: return "字段隐藏窗口";
                case GridStringId.CustomizationColumns: return "数据列";
                case GridStringId.CustomizationBands: return "栏目组";
                case GridStringId.PopupFilterAll: return "(所有)";
                case GridStringId.PopupFilterCustom: return "(自定义)";
                case GridStringId.PopupFilterBlanks: return "(空值)";
                case GridStringId.PopupFilterNonBlanks: return "(非空值)";
                case GridStringId.CustomFilterDialogFormCaption: return "自定义筛选条件";
                case GridStringId.CustomFilterDialogCaption: return "条件为:";
                case GridStringId.CustomFilterDialogRadioAnd: return "并且";
                case GridStringId.CustomFilterDialogRadioOr: return "或者";
                case GridStringId.CustomFilterDialogOkButton: return "确定(&O)";
                case GridStringId.CustomFilterDialog2FieldCheck: return "Field";
                case GridStringId.CustomFilterDialogCancelButton: return "取消(&C)";
                case GridStringId.CustomFilterDialogConditionEQU: return "等于=";
                case GridStringId.CustomFilterDialogConditionNEQ: return "不等于<>";
                case GridStringId.CustomFilterDialogConditionGT: return "大于>";
                case GridStringId.CustomFilterDialogConditionGTE: return "大于或等于>=";
                case GridStringId.CustomFilterDialogConditionLT: return "小于<";
                case GridStringId.CustomFilterDialogConditionLTE: return "小于或等于<=";
                case GridStringId.CustomFilterDialogConditionBlanks: return "空值";
                case GridStringId.CustomFilterDialogConditionNonBlanks: return "非空值";
                case GridStringId.CustomFilterDialogConditionLike: return "包含";
                case GridStringId.CustomFilterDialogConditionNotLike: return "不包含";
                case GridStringId.MenuFooterSum: return "合计";
                case GridStringId.MenuFooterMin: return "最小值";
                case GridStringId.MenuFooterMax: return "最大值";
                case GridStringId.MenuFooterCount: return "计数";
                case GridStringId.MenuFooterAverage: return "平均值";
                case GridStringId.MenuFooterNone: return "取消统计";
                case GridStringId.MenuFooterSumFormat: return "{0:#.##}"; //合计={0:#.##}
                case GridStringId.MenuFooterMinFormat: return "{0}"; //最小值={0}
                case GridStringId.MenuFooterMaxFormat: return "{0}"; //最大值={0}
                case GridStringId.MenuFooterCountFormat: return "{0}";
                case GridStringId.MenuFooterAverageFormat: return "{0:#.##}"; //平均值={0:#.##}
                case GridStringId.MenuColumnSortAscending: return "升序排序";
                case GridStringId.MenuColumnSortDescending: return "降序排序";
                case GridStringId.MenuColumnGroup: return "按此列分组";
                case GridStringId.MenuColumnUnGroup: return "取消当前分组";
                case GridStringId.MenuColumnColumnCustomization: return "显示/隐藏字段";
                case GridStringId.MenuColumnBestFit: return "自动宽度(当前列)";
                case GridStringId.MenuColumnFilter: return "筛选";
                case GridStringId.MenuColumnClearFilter: return "清除筛选条件";
                case GridStringId.MenuColumnBestFitAllColumns: return "自动宽度(所有列)";
                case GridStringId.MenuGroupPanelFullExpand: return "展开全部分组";
                case GridStringId.MenuGroupPanelFullCollapse: return "收缩全部分组";
                case GridStringId.MenuGroupPanelClearGrouping: return "清除所有分组";                    
                case GridStringId.PrintDesignerGridView: return "数据网格打印设计";
                case GridStringId.PrintDesignerCardView: return "卡片打印设计";
                case GridStringId.PrintDesignerBandedView: return "分栏网格打印设计";
                case GridStringId.PrintDesignerBandHeader: return "栏目表头设计";
                case GridStringId.MenuColumnGroupBox: return "分组区";
                case GridStringId.MenuColumnClearSorting: return "清除排序";
                case GridStringId.MenuColumnRemoveColumn: return "移除此列";
                case GridStringId.MenuColumnShowColumn: return "显示此列";
                case GridStringId.MenuGroupPanelShow: return "显示分组区";
                case GridStringId.MenuGroupPanelHide: return "隐藏分组区";
                case GridStringId.MenuColumnFilterEditor: return "自定义筛选条件";
                case GridStringId.CustomizationFormBandHint: return "在此拖拉列分组来定制布局";
                case GridStringId.CustomizationFormColumnHint: return "在此拖拉列来定制布局";
                    
                //case GridStringId.MenuFooterCount: return "abc";
                    //case GridStringId.
                //自定义条件过滤窗体：
                case GridStringId.FilterBuilderApplyButton: return "应用(&A)";
                case GridStringId.FilterBuilderCancelButton: return "取消(&C)";
                case GridStringId.FilterBuilderOkButton: return "确定(&O)";
                case GridStringId.FilterPanelCustomizeButton: return "自定义";
                case GridStringId.FilterBuilderCaption: return "设定数据筛选条件";
                case GridStringId.CustomFilterDialogClearFilter: return "清除过滤器(&L)";
                case GridStringId.CustomFilterDialogEmptyOperator: return "(选择一个操作)";
                case GridStringId.CustomFilterDialogEmptyValue: return "(输入一个值)";


                case GridStringId.CardViewNewCard: return "新卡片";
                case GridStringId.CardViewQuickCustomizationButton: return "自定义格式";
                case GridStringId.CardViewQuickCustomizationButtonFilter: return "筛选";
                case GridStringId.CardViewQuickCustomizationButtonSort: return "排序";
                case GridStringId.GridGroupPanelText: return "拖曳一列页眉在此进行排序";
                case GridStringId.GridNewRowText: return "单击这里新增一行";
                case GridStringId.GridOutlookIntervals: return "一个月以上;上个月;三周前;两周前;上周;;;;;;;;昨天;今天;明天; ;;;;;;;下周;两周后;三周后;下个月;一个月之后;";                
                case GridStringId.WindowErrorCaption: return "错误";

                    

            }
            return base.GetLocalizedString(id);
        }
    } 
}
