using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraTreeList.Localization;

namespace ZQTMS.Tool
{
    public class ToChnXtraTreeList : TreeListLocalizer
    {
        public override string GetLocalizedString(TreeListStringId id)
        {
            switch (id)
            {
                case TreeListStringId.ColumnCustomizationText: return "自定显示字段";
                case TreeListStringId.ColumnNamePrefix: return "列名首标";
                case TreeListStringId.InvalidNodeExceptionText: return "是否确定要修改?";
                case TreeListStringId.MenuColumnBestFit: return "最佳匹配";
                case TreeListStringId.MenuColumnBestFitAllColumns: return "最佳匹配(所有列)";
                case TreeListStringId.MenuColumnColumnCustomization: return "列选择";
                case TreeListStringId.MenuColumnSortAscending: return "升序排序";
                case TreeListStringId.MenuColumnSortDescending: return "降序排序";
                case TreeListStringId.MenuFooterAllNodes: return "全部节点";
                case TreeListStringId.MenuFooterAverage: return "平均";
                case TreeListStringId.MenuFooterAverageFormat: return "平均值={0:#.##}";
                case TreeListStringId.MenuFooterCount: return "计数";
                case TreeListStringId.MenuFooterCountFormat: return "{0}";
                case TreeListStringId.MenuFooterMax: return "最大值";
                case TreeListStringId.MenuFooterMaxFormat: return "最大值={0}";
                case TreeListStringId.MenuFooterMin: return "最小值";
                case TreeListStringId.MenuFooterMinFormat: return "最小值={0}";
                case TreeListStringId.MenuFooterNone: return "无";
                case TreeListStringId.MenuFooterSum: return "合计";
                case TreeListStringId.MenuFooterSumFormat: return "合计={0:#.##}";
                case TreeListStringId.MultiSelectMethodNotSupported: return "OptionsBehavior.MultiSelect未激活时，指定方法不能工作.";
                case TreeListStringId.PrintDesignerDescription: return "为当前的树状列表设置不同的打印选项.";
                case TreeListStringId.PrintDesignerHeader: return "打印设置";
            }
            return base.GetLocalizedString(id);
        }

    }
}
