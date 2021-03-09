using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraEditors.Controls;


namespace ZQTMS.Tool
{


    public class ToChnEditor : Localizer
    {
        public override string GetLocalizedString(StringId id)
        {
            switch (id)
            {
                case StringId.FilterShowAll: return "(选择全部)";

                case StringId.None: return "";
                case StringId.CaptionError: return "系统提示";
                case StringId.InvalidValueText: return "非法值";
                case StringId.CheckChecked: return "选中";
                case StringId.CheckUnchecked: return "未选中";
                case StringId.CheckIndeterminate: return "Indeterminate";
                case StringId.DateEditToday: return "今天";
                case StringId.DateEditClear: return "清除";
                case StringId.OK: return "确定(&O)";
                case StringId.Cancel: return "取消(&C)";
                case StringId.NavigatorFirstButtonHint: return "首个";
                case StringId.NavigatorPreviousButtonHint: return "上一个";
                case StringId.NavigatorPreviousPageButtonHint: return "上一页";
                case StringId.NavigatorNextButtonHint: return "下一个";
                case StringId.NavigatorNextPageButtonHint: return "下一页";
                case StringId.NavigatorLastButtonHint: return "最后";
                case StringId.NavigatorAppendButtonHint: return "增加";
                case StringId.NavigatorRemoveButtonHint: return "删除";
                case StringId.NavigatorEditButtonHint: return "编辑";
                case StringId.NavigatorEndEditButtonHint: return "结束编辑";
                case StringId.NavigatorCancelEditButtonHint: return "取消编辑";
                case StringId.NavigatorTextStringFormat: return "记录 {0} 至 {1}";
                case StringId.FilterOutlookDateText: return "显示全部|按指定的日期过滤：|今年之后|今年晚些时候|本月晚些时候|下个星期|本周晚些时候|明天|今天|昨天|本周早些时候|上个星期|本月早些时候|今年早些时候|今年之前";
                
                //图片相关
                case StringId.PictureEditMenuCut: return "剪切";
                case StringId.PictureEditMenuCopy: return "复制";
                case StringId.PictureEditMenuPaste: return "粘贴";
                case StringId.PictureEditMenuDelete: return "删除";
                case StringId.PictureEditMenuLoad: return "打开";
                case StringId.PictureEditMenuSave: return "保存";
                case StringId.PictureEditOpenFileFilter: return "BMP格式图片 (*.bmp)|*.bmp|GIF格式图片 (*.gif)|*.gif|JPEG格式 (*.jpg;*.jpeg)|*.jpg;*.jpeg|ICO图标格式 (*.ico)|*.ico|所有图片格式 |*.bmp;*.gif;*.jpg;*.jpeg;*.ico;*.png;*.tif|所有 |*.*";
                case StringId.PictureEditSaveFileFilter: return "BMP格式图片 (*.bmp)|*.bmp|GIF格式图片 (*.gif)|*.gif|JPEG格式 (*.jpg)|*.jpg";
                case StringId.PictureEditOpenFileTitle: return "打开";
                case StringId.PictureEditSaveFileTitle: return "保存至";
                case StringId.PictureEditOpenFileError: return "错误的图片格式!";
                case StringId.PictureEditOpenFileErrorCaption: return "打开错误";
                
                
                case StringId.LookUpEditValueIsNull: return "[无数据]";
                case StringId.LookUpInvalidEditValueType: return "非法数据类型";
                case StringId.MaskBoxValidateError: return "The entered value is incomplete.  Do you want to correct it?\r\n\r\nYes - return to the editor and correct the value.\r\nNo - leave the value as is.\r\nCancel - reset to the previous value.\r\n";
                case StringId.UnknownPictureFormat: return "未知图像格式!!";
                case StringId.DataEmpty: return "无图像";
                case StringId.NotValidArrayLength: return "无效的数组长度";
                case StringId.ImagePopupEmpty: return "(空)";
                case StringId.ImagePopupPicture: return "(图像)";
                case StringId.ColorTabCustom: return "自定义颜色";
                case StringId.ColorTabWeb: return "Web颜色";
                case StringId.ColorTabSystem: return "系统颜色";
                
                //计算按钮
                case StringId.CalcButtonMC: return "MC";
                case StringId.CalcButtonMR: return "MR";
                case StringId.CalcButtonMS: return "MS";
                case StringId.CalcButtonMx: return "M+";
                case StringId.CalcButtonSqrt: return "开方";
                case StringId.CalcButtonBack: return "Back";
                case StringId.CalcButtonCE: return "CE";
                case StringId.CalcButtonC: return "C";
                case StringId.CalcError: return "计算错误";

                //自定义条件过滤窗体：
                case StringId.FilterClauseEquals: return "等于=";
                case StringId.FilterClauseDoesNotEqual: return "不等于<>";
                case StringId.FilterClauseGreater: return "大于>";
                case StringId.FilterClauseGreaterOrEqual: return "大于或等于>=";
                case StringId.FilterClauseLess: return "小于<";
                case StringId.FilterClauseLessOrEqual: return "小于或等于<=";
                case StringId.FilterClauseIsNull: return "空值";
                case StringId.FilterClauseIsNotNull: return "非空值";
                case StringId.FilterClauseContains: return "包含";
                case StringId.FilterClauseDoesNotContain: return "不包含";
                case StringId.FilterClauseLike: return "近似于";
                case StringId.FilterClauseNotLike: return "不相似";
                case StringId.FilterClauseBetween: return "在...之间";
                case StringId.FilterClauseNotBetween: return "在...之外";
                case StringId.FilterClauseAnyOf: return "任何值";
                case StringId.FilterClauseNoneOf: return "不在...之内";
                case StringId.FilterClauseBeginsWith: return "以...开头";
                case StringId.FilterClauseEndsWith: return "以...结尾";
                case StringId.FilterEmptyEnter: return "输入一个值";
                case StringId.FilterGroupAnd: return "并且";
                case StringId.FilterGroupNotAnd: return "异与";
                case StringId.FilterGroupNotOr: return "异或";
                case StringId.FilterGroupOr: return "或者";
                case StringId.FilterMenuClearAll: return "全部清除";
                case StringId.FilterMenuConditionAdd: return "添加条件";
                case StringId.FilterMenuGroupAdd: return "添加条件组";
                case StringId.FilterToolTipKeysAdd: return "添加条件";
                case StringId.FilterToolTipNodeAdd: return "添加条件";
                case StringId.FilterToolTipKeysRemove: return "移除条件";
                case StringId.FilterToolTipNodeRemove: return "移除当前条件";
                case StringId.FilterMenuRowRemove: return "移除条件组";

                case StringId.FilterCriteriaToStringGroupOperatorOr: return "或者";
                case StringId.FilterCriteriaToStringBinaryOperatorLike: return "包含";
                case StringId.FilterCriteriaInvalidExpression: return "非法表达式";
                case StringId.FilterCriteriaInvalidExpressionEx: return "非法表达式";
                case StringId.FilterCriteriaToStringBetween: return "在...之间";
                case StringId.FilterCriteriaToStringBinaryOperatorDivide: return "/";
                case StringId.FilterCriteriaToStringBinaryOperatorBitwiseAnd: return "@";
                case StringId.FilterCriteriaToStringBinaryOperatorBitwiseOr: return "|";
                case StringId.FilterCriteriaToStringBinaryOperatorBitwiseXor: return "^";
                case StringId.FilterCriteriaToStringBinaryOperatorEqual: return "=";
                case StringId.FilterCriteriaToStringBinaryOperatorGreater: return ">";
                case StringId.FilterCriteriaToStringBinaryOperatorGreaterOrEqual: return ">=";
                case StringId.FilterCriteriaToStringBinaryOperatorLess: return "<";
                case StringId.FilterCriteriaToStringBinaryOperatorLessOrEqual: return "<=";
                case StringId.FilterCriteriaToStringBinaryOperatorMinus: return "-";
                case StringId.FilterCriteriaToStringBinaryOperatorModulo: return "%";
                case StringId.FilterCriteriaToStringBinaryOperatorMultiply: return "*";
                case StringId.FilterCriteriaToStringBinaryOperatorNotEqual: return "<>";
                case StringId.FilterCriteriaToStringBinaryOperatorPlus: return "+";

                //文本框右键
                case StringId.TextEditMenuCopy: return "复制(&C)";
                case StringId.TextEditMenuCut: return "剪切(&T)";
                case StringId.TextEditMenuDelete: return "删除(&D)";
                case StringId.TextEditMenuPaste: return "粘贴(&P)";
                case StringId.TextEditMenuSelectAll: return "全选(&A)";
                case StringId.TextEditMenuUndo: return "撤销(&U)";

                //DevExpress.XtraEditors.XtraMessageBox
                case StringId.XtraMessageBoxAbortButtonText: return "中止(&A)";
                case StringId.XtraMessageBoxCancelButtonText: return "取消(&C)";
                case StringId.XtraMessageBoxIgnoreButtonText: return "忽略(&I)";
                case StringId.XtraMessageBoxNoButtonText: return "否(&N)";
                case StringId.XtraMessageBoxOkButtonText: return "确定(&O)";
                case StringId.XtraMessageBoxRetryButtonText: return "重试(&R)";
                case StringId.XtraMessageBoxYesButtonText: return "是(&Y)";

                //XtraTabControl的Header按钮
                case StringId.TabHeaderButtonClose: return "关闭";
                case StringId.TabHeaderButtonNext: return "下一页";
                case StringId.TabHeaderButtonPrev: return "上一页";
            }
            return base.GetLocalizedString(id);
        }
    }
}
