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
                case StringId.FilterShowAll: return "(ѡ��ȫ��)";

                case StringId.None: return "";
                case StringId.CaptionError: return "ϵͳ��ʾ";
                case StringId.InvalidValueText: return "�Ƿ�ֵ";
                case StringId.CheckChecked: return "ѡ��";
                case StringId.CheckUnchecked: return "δѡ��";
                case StringId.CheckIndeterminate: return "Indeterminate";
                case StringId.DateEditToday: return "����";
                case StringId.DateEditClear: return "���";
                case StringId.OK: return "ȷ��(&O)";
                case StringId.Cancel: return "ȡ��(&C)";
                case StringId.NavigatorFirstButtonHint: return "�׸�";
                case StringId.NavigatorPreviousButtonHint: return "��һ��";
                case StringId.NavigatorPreviousPageButtonHint: return "��һҳ";
                case StringId.NavigatorNextButtonHint: return "��һ��";
                case StringId.NavigatorNextPageButtonHint: return "��һҳ";
                case StringId.NavigatorLastButtonHint: return "���";
                case StringId.NavigatorAppendButtonHint: return "����";
                case StringId.NavigatorRemoveButtonHint: return "ɾ��";
                case StringId.NavigatorEditButtonHint: return "�༭";
                case StringId.NavigatorEndEditButtonHint: return "�����༭";
                case StringId.NavigatorCancelEditButtonHint: return "ȡ���༭";
                case StringId.NavigatorTextStringFormat: return "��¼ {0} �� {1}";
                case StringId.FilterOutlookDateText: return "��ʾȫ��|��ָ�������ڹ��ˣ�|����֮��|������Щʱ��|������Щʱ��|�¸�����|������Щʱ��|����|����|����|������Щʱ��|�ϸ�����|������Щʱ��|������Щʱ��|����֮ǰ";
                
                //ͼƬ���
                case StringId.PictureEditMenuCut: return "����";
                case StringId.PictureEditMenuCopy: return "����";
                case StringId.PictureEditMenuPaste: return "ճ��";
                case StringId.PictureEditMenuDelete: return "ɾ��";
                case StringId.PictureEditMenuLoad: return "��";
                case StringId.PictureEditMenuSave: return "����";
                case StringId.PictureEditOpenFileFilter: return "BMP��ʽͼƬ (*.bmp)|*.bmp|GIF��ʽͼƬ (*.gif)|*.gif|JPEG��ʽ (*.jpg;*.jpeg)|*.jpg;*.jpeg|ICOͼ���ʽ (*.ico)|*.ico|����ͼƬ��ʽ |*.bmp;*.gif;*.jpg;*.jpeg;*.ico;*.png;*.tif|���� |*.*";
                case StringId.PictureEditSaveFileFilter: return "BMP��ʽͼƬ (*.bmp)|*.bmp|GIF��ʽͼƬ (*.gif)|*.gif|JPEG��ʽ (*.jpg)|*.jpg";
                case StringId.PictureEditOpenFileTitle: return "��";
                case StringId.PictureEditSaveFileTitle: return "������";
                case StringId.PictureEditOpenFileError: return "�����ͼƬ��ʽ!";
                case StringId.PictureEditOpenFileErrorCaption: return "�򿪴���";
                
                
                case StringId.LookUpEditValueIsNull: return "[������]";
                case StringId.LookUpInvalidEditValueType: return "�Ƿ���������";
                case StringId.MaskBoxValidateError: return "The entered value is incomplete.  Do you want to correct it?\r\n\r\nYes - return to the editor and correct the value.\r\nNo - leave the value as is.\r\nCancel - reset to the previous value.\r\n";
                case StringId.UnknownPictureFormat: return "δ֪ͼ���ʽ!!";
                case StringId.DataEmpty: return "��ͼ��";
                case StringId.NotValidArrayLength: return "��Ч�����鳤��";
                case StringId.ImagePopupEmpty: return "(��)";
                case StringId.ImagePopupPicture: return "(ͼ��)";
                case StringId.ColorTabCustom: return "�Զ�����ɫ";
                case StringId.ColorTabWeb: return "Web��ɫ";
                case StringId.ColorTabSystem: return "ϵͳ��ɫ";
                
                //���㰴ť
                case StringId.CalcButtonMC: return "MC";
                case StringId.CalcButtonMR: return "MR";
                case StringId.CalcButtonMS: return "MS";
                case StringId.CalcButtonMx: return "M+";
                case StringId.CalcButtonSqrt: return "����";
                case StringId.CalcButtonBack: return "Back";
                case StringId.CalcButtonCE: return "CE";
                case StringId.CalcButtonC: return "C";
                case StringId.CalcError: return "�������";

                //�Զ����������˴��壺
                case StringId.FilterClauseEquals: return "����=";
                case StringId.FilterClauseDoesNotEqual: return "������<>";
                case StringId.FilterClauseGreater: return "����>";
                case StringId.FilterClauseGreaterOrEqual: return "���ڻ����>=";
                case StringId.FilterClauseLess: return "С��<";
                case StringId.FilterClauseLessOrEqual: return "С�ڻ����<=";
                case StringId.FilterClauseIsNull: return "��ֵ";
                case StringId.FilterClauseIsNotNull: return "�ǿ�ֵ";
                case StringId.FilterClauseContains: return "����";
                case StringId.FilterClauseDoesNotContain: return "������";
                case StringId.FilterClauseLike: return "������";
                case StringId.FilterClauseNotLike: return "������";
                case StringId.FilterClauseBetween: return "��...֮��";
                case StringId.FilterClauseNotBetween: return "��...֮��";
                case StringId.FilterClauseAnyOf: return "�κ�ֵ";
                case StringId.FilterClauseNoneOf: return "����...֮��";
                case StringId.FilterClauseBeginsWith: return "��...��ͷ";
                case StringId.FilterClauseEndsWith: return "��...��β";
                case StringId.FilterEmptyEnter: return "����һ��ֵ";
                case StringId.FilterGroupAnd: return "����";
                case StringId.FilterGroupNotAnd: return "����";
                case StringId.FilterGroupNotOr: return "���";
                case StringId.FilterGroupOr: return "����";
                case StringId.FilterMenuClearAll: return "ȫ�����";
                case StringId.FilterMenuConditionAdd: return "�������";
                case StringId.FilterMenuGroupAdd: return "���������";
                case StringId.FilterToolTipKeysAdd: return "�������";
                case StringId.FilterToolTipNodeAdd: return "�������";
                case StringId.FilterToolTipKeysRemove: return "�Ƴ�����";
                case StringId.FilterToolTipNodeRemove: return "�Ƴ���ǰ����";
                case StringId.FilterMenuRowRemove: return "�Ƴ�������";

                case StringId.FilterCriteriaToStringGroupOperatorOr: return "����";
                case StringId.FilterCriteriaToStringBinaryOperatorLike: return "����";
                case StringId.FilterCriteriaInvalidExpression: return "�Ƿ����ʽ";
                case StringId.FilterCriteriaInvalidExpressionEx: return "�Ƿ����ʽ";
                case StringId.FilterCriteriaToStringBetween: return "��...֮��";
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

                //�ı����Ҽ�
                case StringId.TextEditMenuCopy: return "����(&C)";
                case StringId.TextEditMenuCut: return "����(&T)";
                case StringId.TextEditMenuDelete: return "ɾ��(&D)";
                case StringId.TextEditMenuPaste: return "ճ��(&P)";
                case StringId.TextEditMenuSelectAll: return "ȫѡ(&A)";
                case StringId.TextEditMenuUndo: return "����(&U)";

                //DevExpress.XtraEditors.XtraMessageBox
                case StringId.XtraMessageBoxAbortButtonText: return "��ֹ(&A)";
                case StringId.XtraMessageBoxCancelButtonText: return "ȡ��(&C)";
                case StringId.XtraMessageBoxIgnoreButtonText: return "����(&I)";
                case StringId.XtraMessageBoxNoButtonText: return "��(&N)";
                case StringId.XtraMessageBoxOkButtonText: return "ȷ��(&O)";
                case StringId.XtraMessageBoxRetryButtonText: return "����(&R)";
                case StringId.XtraMessageBoxYesButtonText: return "��(&Y)";

                //XtraTabControl��Header��ť
                case StringId.TabHeaderButtonClose: return "�ر�";
                case StringId.TabHeaderButtonNext: return "��һҳ";
                case StringId.TabHeaderButtonPrev: return "��һҳ";
            }
            return base.GetLocalizedString(id);
        }
    }
}
