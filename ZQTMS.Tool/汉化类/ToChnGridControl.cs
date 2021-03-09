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
                case GridStringId.FileIsNotFoundError: return "�ļ� {0} δ�ҵ�";
                case GridStringId.ColumnViewExceptionMessage: return " ȷ����������?";
                case GridStringId.CustomizationCaption: return "�ֶ����ش���";
                case GridStringId.CustomizationColumns: return "������";
                case GridStringId.CustomizationBands: return "��Ŀ��";
                case GridStringId.PopupFilterAll: return "(����)";
                case GridStringId.PopupFilterCustom: return "(�Զ���)";
                case GridStringId.PopupFilterBlanks: return "(��ֵ)";
                case GridStringId.PopupFilterNonBlanks: return "(�ǿ�ֵ)";
                case GridStringId.CustomFilterDialogFormCaption: return "�Զ���ɸѡ����";
                case GridStringId.CustomFilterDialogCaption: return "����Ϊ:";
                case GridStringId.CustomFilterDialogRadioAnd: return "����";
                case GridStringId.CustomFilterDialogRadioOr: return "����";
                case GridStringId.CustomFilterDialogOkButton: return "ȷ��(&O)";
                case GridStringId.CustomFilterDialog2FieldCheck: return "Field";
                case GridStringId.CustomFilterDialogCancelButton: return "ȡ��(&C)";
                case GridStringId.CustomFilterDialogConditionEQU: return "����=";
                case GridStringId.CustomFilterDialogConditionNEQ: return "������<>";
                case GridStringId.CustomFilterDialogConditionGT: return "����>";
                case GridStringId.CustomFilterDialogConditionGTE: return "���ڻ����>=";
                case GridStringId.CustomFilterDialogConditionLT: return "С��<";
                case GridStringId.CustomFilterDialogConditionLTE: return "С�ڻ����<=";
                case GridStringId.CustomFilterDialogConditionBlanks: return "��ֵ";
                case GridStringId.CustomFilterDialogConditionNonBlanks: return "�ǿ�ֵ";
                case GridStringId.CustomFilterDialogConditionLike: return "����";
                case GridStringId.CustomFilterDialogConditionNotLike: return "������";
                case GridStringId.MenuFooterSum: return "�ϼ�";
                case GridStringId.MenuFooterMin: return "��Сֵ";
                case GridStringId.MenuFooterMax: return "���ֵ";
                case GridStringId.MenuFooterCount: return "����";
                case GridStringId.MenuFooterAverage: return "ƽ��ֵ";
                case GridStringId.MenuFooterNone: return "ȡ��ͳ��";
                case GridStringId.MenuFooterSumFormat: return "{0:#.##}"; //�ϼ�={0:#.##}
                case GridStringId.MenuFooterMinFormat: return "{0}"; //��Сֵ={0}
                case GridStringId.MenuFooterMaxFormat: return "{0}"; //���ֵ={0}
                case GridStringId.MenuFooterCountFormat: return "{0}";
                case GridStringId.MenuFooterAverageFormat: return "{0:#.##}"; //ƽ��ֵ={0:#.##}
                case GridStringId.MenuColumnSortAscending: return "��������";
                case GridStringId.MenuColumnSortDescending: return "��������";
                case GridStringId.MenuColumnGroup: return "�����з���";
                case GridStringId.MenuColumnUnGroup: return "ȡ����ǰ����";
                case GridStringId.MenuColumnColumnCustomization: return "��ʾ/�����ֶ�";
                case GridStringId.MenuColumnBestFit: return "�Զ����(��ǰ��)";
                case GridStringId.MenuColumnFilter: return "ɸѡ";
                case GridStringId.MenuColumnClearFilter: return "���ɸѡ����";
                case GridStringId.MenuColumnBestFitAllColumns: return "�Զ����(������)";
                case GridStringId.MenuGroupPanelFullExpand: return "չ��ȫ������";
                case GridStringId.MenuGroupPanelFullCollapse: return "����ȫ������";
                case GridStringId.MenuGroupPanelClearGrouping: return "������з���";                    
                case GridStringId.PrintDesignerGridView: return "���������ӡ���";
                case GridStringId.PrintDesignerCardView: return "��Ƭ��ӡ���";
                case GridStringId.PrintDesignerBandedView: return "���������ӡ���";
                case GridStringId.PrintDesignerBandHeader: return "��Ŀ��ͷ���";
                case GridStringId.MenuColumnGroupBox: return "������";
                case GridStringId.MenuColumnClearSorting: return "�������";
                case GridStringId.MenuColumnRemoveColumn: return "�Ƴ�����";
                case GridStringId.MenuColumnShowColumn: return "��ʾ����";
                case GridStringId.MenuGroupPanelShow: return "��ʾ������";
                case GridStringId.MenuGroupPanelHide: return "���ط�����";
                case GridStringId.MenuColumnFilterEditor: return "�Զ���ɸѡ����";
                case GridStringId.CustomizationFormBandHint: return "�ڴ������з��������Ʋ���";
                case GridStringId.CustomizationFormColumnHint: return "�ڴ������������Ʋ���";
                    
                //case GridStringId.MenuFooterCount: return "abc";
                    //case GridStringId.
                //�Զ����������˴��壺
                case GridStringId.FilterBuilderApplyButton: return "Ӧ��(&A)";
                case GridStringId.FilterBuilderCancelButton: return "ȡ��(&C)";
                case GridStringId.FilterBuilderOkButton: return "ȷ��(&O)";
                case GridStringId.FilterPanelCustomizeButton: return "�Զ���";
                case GridStringId.FilterBuilderCaption: return "�趨����ɸѡ����";
                case GridStringId.CustomFilterDialogClearFilter: return "���������(&L)";
                case GridStringId.CustomFilterDialogEmptyOperator: return "(ѡ��һ������)";
                case GridStringId.CustomFilterDialogEmptyValue: return "(����һ��ֵ)";


                case GridStringId.CardViewNewCard: return "�¿�Ƭ";
                case GridStringId.CardViewQuickCustomizationButton: return "�Զ����ʽ";
                case GridStringId.CardViewQuickCustomizationButtonFilter: return "ɸѡ";
                case GridStringId.CardViewQuickCustomizationButtonSort: return "����";
                case GridStringId.GridGroupPanelText: return "��ҷһ��ҳü�ڴ˽�������";
                case GridStringId.GridNewRowText: return "������������һ��";
                case GridStringId.GridOutlookIntervals: return "һ��������;�ϸ���;����ǰ;����ǰ;����;;;;;;;;����;����;����; ;;;;;;;����;���ܺ�;���ܺ�;�¸���;һ����֮��;";                
                case GridStringId.WindowErrorCaption: return "����";

                    

            }
            return base.GetLocalizedString(id);
        }
    } 
}
