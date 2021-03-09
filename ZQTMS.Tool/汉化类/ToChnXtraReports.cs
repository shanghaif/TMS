//using System;
//using System.Collections.Generic;
//using System.Text;
//using DevExpress.XtraReports.Localization;

//namespace ZQTMS.Tool
//{
//    public class ToChnXtraReports : ReportLocalizer
//    {
//        public override string GetLocalizedString(ReportStringId id)
//        {
//            switch (id)
//            {
//                case ReportStringId.BandDsg_QuantityPerPage: return "一个页面集合";
//                case ReportStringId.BandDsg_QuantityPerReport: return "一个报表集合";
//                case ReportStringId.BCForm_Lbl_Binding: return "结合";
//                case ReportStringId.BCForm_Lbl_Property: return "属性";
//                case ReportStringId.Cmd_AlignToGrid: return "对齐到网格线";
//                case ReportStringId.Cmd_BottomMargin: return "底端边缘";
//                case ReportStringId.Cmd_BringToFront: return "置于顶层";
//                case ReportStringId.Cmd_Copy: return "复制";
//                case ReportStringId.Cmd_Cut: return "剪贴";
//                case ReportStringId.Cmd_Delete: return "删除";
//                case ReportStringId.Cmd_Detail: return "详细";
//                case ReportStringId.Cmd_DetailReport: return "详细报表";
//                case ReportStringId.Cmd_GroupFooter: return "群组尾";
//                case ReportStringId.Cmd_GroupHeader: return "群组首";
//                case ReportStringId.Cmd_InsertBand: return "插入区段";
//                case ReportStringId.Cmd_InsertDetailReport: return "插入详细报表";
//                case ReportStringId.Cmd_BandMoveDown: return "下移";
//                case ReportStringId.Cmd_BandMoveUp: return "上移";
//                case ReportStringId.Cmd_InsertUnboundDetailReport: return "非绑定";
//                case ReportStringId.Cmd_PageFooter: return "页尾";
//                case ReportStringId.Cmd_PageHeader: return "页首";
//                case ReportStringId.Cmd_Paste: return "粘贴";

//                case ReportStringId.Cmd_Properties: return "属性";
//                case ReportStringId.Cmd_ReportFooter: return "报表尾";
//                case ReportStringId.Cmd_ReportHeader: return "报表首";
//                case ReportStringId.Cmd_RtfClear: return "清除";
//                case ReportStringId.Cmd_RtfLoad: return "加载文件...";
//                case ReportStringId.Cmd_SendToBack: return "置于顶层";
//                case ReportStringId.Cmd_TableDelete: return "删除(&L)";
//                case ReportStringId.Cmd_TableDeleteCell: return "单元格(&L)";
//                case ReportStringId.Cmd_TableDeleteColumn: return "列(&C)";
//                case ReportStringId.Cmd_TableDeleteRow: return "行(&R)";
//                case ReportStringId.Cmd_TableInsert: return "插入(&I)";
//                case ReportStringId.Cmd_TableInsertCell: return "单元格(&C)";
//                case ReportStringId.Cmd_TableInsertColumnToLeft: return "左列(&L)";
//                case ReportStringId.Cmd_TableInsertColumnToRight: return "右列(&R)";
//                case ReportStringId.Cmd_TableInsertRowAbove: return "上行(&A)";
//                case ReportStringId.Cmd_TableInsertRowBelow: return "下行(&B)";
//                case ReportStringId.Cmd_TopMargin: return "顶端边缘";
//                case ReportStringId.Cmd_ViewCode: return "检视代码";

//                case ReportStringId.FSForm_Btn_Delete: return "删除";
//                case ReportStringId.FSForm_GrBox_Sample: return "范例";
//                case ReportStringId.FSForm_Lbl_Category: return "类别";
//                case ReportStringId.FSForm_Lbl_CustomGeneral: return "一般格式不包含特殊数字格式";
//                case ReportStringId.FSForm_Lbl_Prefix: return "上标";
//                case ReportStringId.FSForm_Lbl_Suffix: return "下标";
//                case ReportStringId.FSForm_Msg_BadSymbol: return "损坏的符号";
//                case ReportStringId.FSForm_Tab_Custom: return "自定义";
//                case ReportStringId.FSForm_Tab_StandardTypes: return "标准类型";
//                case ReportStringId.Msg_CreateReportInstance: return "您试图打开一个不同类型的报表来编辑。是否确定建立实例？";
//                case ReportStringId.Msg_CreateSomeInstance: return "在窗体中不能建立两个实例类。";
//                case ReportStringId.Msg_CyclicBoormarks: return "报表循环书签";
//                case ReportStringId.Msg_DontSupportMulticolumn: return "详细报表不支持多字段。";
//                case ReportStringId.Msg_FileCorrupted: return "不能加载报表，文件可能被破坏或者报表组件丢失。";
//                case ReportStringId.Msg_FileNotFound: return "文件没有找到";
//                case ReportStringId.Msg_FillDataError: return "数据加载时发生错误。错误为：";
//                case ReportStringId.Msg_IncorrectArgument: return "参数值输入不正确";
//                case ReportStringId.Msg_IncorrectBandType: return "无效的带型";
//                case ReportStringId.Msg_InvalidMethodCall: return "对象的当前状态下不能调用此方法";
//                case ReportStringId.Msg_InvalidReportSource: return "无法设置原报表";
//                case ReportStringId.Msg_InvPropName: return "无效的属性名";
//                case ReportStringId.Msg_ScriptError: return "在脚本中发现错误： {0}";
//                case ReportStringId.Msg_ScriptExecutionError: return "在脚本执行过程中发现错误 {0}:  {1} 过程 {0} 被运行，将不能再被调用。";
//                case ReportStringId.Msg_WrongReportClassName: return "一个错误发生在并行化期间 - 可能是报表类名错误";
//                case ReportStringId.MultiColumnDesignMsg1: return "重复列之间的空位";
//                case ReportStringId.MultiColumnDesignMsg2: return "控件位置不正确，将会导致打印错误";
//                case ReportStringId.PanelDesignMsg: return "在此可放置不同控件";
//                case ReportStringId.RepTabCtl_Designer: return "设计";
//                case ReportStringId.RepTabCtl_HtmlView: return "HTML视图";
//                case ReportStringId.RepTabCtl_Preview: return "预览";
//                case ReportStringId.SSForm_Btn_Close: return "关闭";
//                case ReportStringId.SSForm_Caption: return "式样单编辑";
//                case ReportStringId.SSForm_Msg_FileFilter: return "Report StyleSheet files (*.repss)|*.repss|All files (*.*)|*.*";
//                case ReportStringId.SSForm_Msg_InvalidFileFormat: return "无效的文件格式";
//                case ReportStringId.SSForm_Msg_MoreThanOneStyle: return "你选择了多过一个以上的式样";
//                case ReportStringId.SSForm_Msg_NoStyleSelected: return "没有式样被选中";
//                case ReportStringId.SSForm_Msg_SelectedStylesText: return "被选中的式样...";
//                case ReportStringId.SSForm_Msg_StyleNamePreviewPostfix: return "式样";
//                case ReportStringId.SSForm_Msg_StyleSheetError: return "StyleSheet错误";
//                case ReportStringId.SSForm_TTip_AddStyle: return "添加式样";
//                case ReportStringId.SSForm_TTip_ClearStyles: return "清除式样";
//                case ReportStringId.SSForm_TTip_LoadStyles: return "从文件中读入式样";
//                case ReportStringId.SSForm_TTip_PurgeStyles: return "清除式样";
//                case ReportStringId.SSForm_TTip_RemoveStyle: return "移除式样";
//                case ReportStringId.SSForm_TTip_SaveStyles: return "保存式样到文件";
//                case ReportStringId.UD_FormCaption: return "报表设计";
//                case ReportStringId.UD_Msg_ReportChanged: return "报表内容已被修改，是否保存修改?";
//                case ReportStringId.UD_ReportDesigner: return "报表设计";
//                case ReportStringId.UD_TTip_AlignBottom: return "对齐主控项的下缘";
//                case ReportStringId.UD_TTip_AlignHorizontalCenters: return "对齐主控项的垂直中间";
//                case ReportStringId.UD_TTip_AlignLeft: return "对齐主控项的左缘";
//                case ReportStringId.UD_TTip_AlignRight: return "对齐主控项的右缘";
//                case ReportStringId.UD_TTip_AlignToGrid: return "对齐网格线";
//                case ReportStringId.UD_TTip_AlignTop: return "对齐主控项的上缘";
//                case ReportStringId.UD_TTip_AlignVerticalCenters: return "对齐主控项的水平中央";
//                case ReportStringId.UD_TTip_BringToFront: return "移到最上层";
//                case ReportStringId.UD_TTip_CenterHorizontally: return "水平置中";
//                case ReportStringId.UD_TTip_CenterVertically: return "垂直置中";
//                case ReportStringId.UD_TTip_EditCopy: return "复制";
//                case ReportStringId.UD_TTip_EditCut: return "剪贴";
//                case ReportStringId.UD_TTip_EditPaste: return "粘贴";
//                case ReportStringId.UD_TTip_FileOpen: return "打开文件";
//                case ReportStringId.UD_TTip_FileSave: return "保存文件";
//                case ReportStringId.UD_TTip_FormatAlignLeft: return "左对齐";
//                case ReportStringId.UD_TTip_FormatAlignRight: return "右对齐";
//                case ReportStringId.UD_TTip_FormatBackColor: return "背景颜色";
//                case ReportStringId.UD_TTip_FormatBold: return "粗体";
//                case ReportStringId.UD_TTip_FormatCenter: return "居中";
//                case ReportStringId.UD_TTip_FormatFontName: return "字体";
//                case ReportStringId.UD_TTip_FormatFontSize: return "大小";
//                case ReportStringId.UD_TTip_FormatForeColor: return "前景颜色";
//                case ReportStringId.UD_TTip_FormatItalic: return "斜体";
//                case ReportStringId.UD_TTip_FormatJustify: return "两端对齐";
//                case ReportStringId.UD_TTip_FormatUnderline: return "下划线";
//                case ReportStringId.UD_TTip_HorizSpaceConcatenate: return "移除水平间距";
//                case ReportStringId.UD_TTip_HorizSpaceDecrease: return "减少水平间距";
//                case ReportStringId.UD_TTip_HorizSpaceIncrease: return "增加水平间距";
//                case ReportStringId.UD_TTip_HorizSpaceMakeEqual: return "将垂直间距设为相等";
//                case ReportStringId.UD_TTip_Redo: return "恢复";
//                case ReportStringId.UD_TTip_SendToBack: return "移到最下层";
//                case ReportStringId.UD_TTip_SizeToControl: return "设置成相同大小";
//                case ReportStringId.UD_TTip_SizeToControlHeight: return "设置成相同高度";
//                case ReportStringId.UD_TTip_SizeToControlWidth: return "设置成相同宽度";
//                case ReportStringId.UD_TTip_SizeToGrid: return "依网格线调整大小";
//                case ReportStringId.UD_TTip_Undo: return "撤消";
//                case ReportStringId.UD_TTip_VertSpaceConcatenate: return "移除垂直间距";
//                case ReportStringId.UD_TTip_VertSpaceDecrease: return "减少垂直间距";
//                case ReportStringId.UD_TTip_VertSpaceIncrease: return "增加垂直间距";
//                case ReportStringId.UD_TTip_VertSpaceMakeEqual: return "将垂直间距设为相等";

//                case ReportStringId.UD_Title_FieldList: return "字段列表";
//                case ReportStringId.UD_Title_GroupAndSort: return "分组和排序";
//                case ReportStringId.UD_Title_PropertyGrid: return "属性";
//                case ReportStringId.UD_Title_ReportExplorer: return "对象浏览器";
//                case ReportStringId.UD_Title_ToolBox: return "工具箱";
//                case ReportStringId.UD_Title_ErrorList: return "错误列表";

//                case ReportStringId.GroupSort_AddGroup: return "添加分组";
//                case ReportStringId.GroupSort_AddSort: return "添加排序条件";
//                case ReportStringId.GroupSort_Delete: return "删除";
//                case ReportStringId.GroupSort_MoveDown: return "下移";
//                case ReportStringId.GroupSort_MoveUp: return "上移";

//                case ReportStringId.ScriptEditor_Validate: return "验证";

//                case ReportStringId.UD_Capt_Undo: return "撤销";
//                case ReportStringId.UD_Capt_ZoomFactor: return "缩放比例:{0}%";
//                case ReportStringId.UD_XtraReportsPointerItemCaption: return "指针";
//                case ReportStringId.UD_XtraReportsToolboxCategoryName: return "标准工具控件";

//                case ReportStringId.RibbonXRDesign_PageText: return "设计视图";
//                case ReportStringId.RibbonXRDesign_HtmlPageText: return "HTML网页视图";

//                case ReportStringId.RibbonXRDesign_PageGroup_Report: return "报表";
//                case ReportStringId.RibbonXRDesign_PageGroup_Edit: return "编辑";
//                case ReportStringId.RibbonXRDesign_PageGroup_Font: return "字体";
//                case ReportStringId.RibbonXRDesign_PageGroup_Alignment: return "对齐";
//                case ReportStringId.RibbonXRDesign_PageGroup_Zoom: return "缩放";
//                case ReportStringId.RibbonXRDesign_PageGroup_View: return "视图";
//                case ReportStringId.RibbonXRDesign_PageGroup_Scripts: return "脚本";

//                #region 报表
//                case ReportStringId.RibbonXRDesign_NewReport_Caption: return "新建";
//                case ReportStringId.RibbonXRDesign_NewReport_STipTitle: return "新建空白报表 (Ctrl + N)";
//                case ReportStringId.RibbonXRDesign_NewReport_STipContent: return "新建一个空白报表,你可以设计报表,\r\n在其中插入字段或控件";
//                case ReportStringId.RibbonXRDesign_NewReportWizard_Caption: return "使用向导创建报表";
//                case ReportStringId.RibbonXRDesign_NewReportWizard_STipTitle: return "使用向导创建报表";
//                case ReportStringId.RibbonXRDesign_NewReportWizard_STipContent: return "使用向导创建报表";
//                case ReportStringId.RibbonXRDesign_OpenFile_Caption: return "打开";
//                case ReportStringId.RibbonXRDesign_OpenFile_STipTitle: return "打开报表";
//                case ReportStringId.RibbonXRDesign_OpenFile_STipContent: return "打开一个已存在的报表,对其进行编辑";
//                case ReportStringId.RibbonXRDesign_SaveFile_Caption: return "保存";
//                case ReportStringId.RibbonXRDesign_SaveFile_STipTitle: return "保存报表";
//                case ReportStringId.RibbonXRDesign_SaveFile_STipContent: return "保存报表";
//                case ReportStringId.RibbonXRDesign_SaveFileAs_Caption: return "另存为";
//                case ReportStringId.RibbonXRDesign_SaveFileAs_STipTitle: return "另存为";
//                case ReportStringId.RibbonXRDesign_SaveFileAs_STipContent: return "另存为";
//                #endregion

//                #region 编辑
//                case ReportStringId.RibbonXRDesign_Cut_Caption: return "剪切";
//                case ReportStringId.RibbonXRDesign_Cut_STipTitle: return "剪切";
//                case ReportStringId.RibbonXRDesign_Cut_STipContent: return "剪切选中的控件到剪贴板";
//                case ReportStringId.RibbonXRDesign_Copy_Caption: return "复制";
//                case ReportStringId.RibbonXRDesign_Copy_STipTitle: return "复制";
//                case ReportStringId.RibbonXRDesign_Copy_STipContent: return "复制选定的控件到剪贴板";
//                case ReportStringId.RibbonXRDesign_Paste_Caption: return "粘贴";
//                case ReportStringId.RibbonXRDesign_Paste_STipTitle: return "粘贴";
//                case ReportStringId.RibbonXRDesign_Paste_STipContent: return "粘贴剪切板中控件";

//                case ReportStringId.RibbonXRDesign_Undo_Caption: return "撤销";
//                case ReportStringId.RibbonXRDesign_Undo_STipTitle: return "撤销";
//                case ReportStringId.RibbonXRDesign_Undo_STipContent: return "撤销上一步操作";
//                case ReportStringId.RibbonXRDesign_Redo_Caption: return "恢复";
//                case ReportStringId.RibbonXRDesign_Redo_STipTitle: return "恢复";
//                case ReportStringId.RibbonXRDesign_Redo_STipContent: return "恢复上一步操作";
//                #endregion

//                #region 字体
//                case ReportStringId.RibbonXRDesign_FontName_STipTitle: return "字体";
//                case ReportStringId.RibbonXRDesign_FontName_STipContent: return "设置当前字体";

//                case ReportStringId.RibbonXRDesign_BackColor_Caption: return "背景颜色";
//                case ReportStringId.RibbonXRDesign_BackColor_STipTitle: return "背景颜色";
//                case ReportStringId.RibbonXRDesign_BackColor_STipContent: return "背景颜色";

//                case ReportStringId.RibbonXRDesign_ForeColor_Caption: return "前景颜色";
//                case ReportStringId.RibbonXRDesign_ForeColor_STipTitle: return "前景颜色";
//                case ReportStringId.RibbonXRDesign_ForeColor_STipContent: return "前景颜色";

//                case ReportStringId.RibbonXRDesign_FontSize_STipTitle: return "字体大小";
//                case ReportStringId.RibbonXRDesign_FontSize_STipContent: return "设置字体大小";

//                case ReportStringId.RibbonXRDesign_FontBold_Caption: return "加粗";
//                case ReportStringId.RibbonXRDesign_FontBold_STipTitle: return "加粗";
//                case ReportStringId.RibbonXRDesign_FontBold_STipContent: return "加粗";

//                case ReportStringId.RibbonXRDesign_FontItalic_Caption: return "倾斜";
//                case ReportStringId.RibbonXRDesign_FontItalic_STipTitle: return "倾斜";
//                case ReportStringId.RibbonXRDesign_FontItalic_STipContent: return "倾斜";

//                case ReportStringId.RibbonXRDesign_FontUnderline_Caption: return "下划线";
//                case ReportStringId.RibbonXRDesign_FontUnderline_STipTitle: return "下划线";
//                case ReportStringId.RibbonXRDesign_FontUnderline_STipContent: return "下划线";

//                case ReportStringId.RibbonXRDesign_JustifyLeft_Caption: return "左对齐";
//                case ReportStringId.RibbonXRDesign_JustifyLeft_STipTitle: return "左对齐";
//                case ReportStringId.RibbonXRDesign_JustifyLeft_STipContent: return "左对齐";

//                case ReportStringId.RibbonXRDesign_JustifyCenter_Caption: return "居中";
//                case ReportStringId.RibbonXRDesign_JustifyCenter_STipTitle: return "居中";
//                case ReportStringId.RibbonXRDesign_JustifyCenter_STipContent: return "居中";

//                case ReportStringId.RibbonXRDesign_JustifyRight_Caption: return "右对齐";
//                case ReportStringId.RibbonXRDesign_JustifyRight_STipTitle: return "右对齐";
//                case ReportStringId.RibbonXRDesign_JustifyRight_STipContent: return "右对齐";

//                case ReportStringId.RibbonXRDesign_JustifyJustify_Caption: return "两端对齐";
//                case ReportStringId.RibbonXRDesign_JustifyJustify_STipTitle: return "两端对齐";
//                case ReportStringId.RibbonXRDesign_JustifyJustify_STipContent: return "两端对齐";
//                #endregion

//                #region 对齐
//                case ReportStringId.RibbonXRDesign_AlignToGrid_Caption: return "对齐到网格";
//                case ReportStringId.RibbonXRDesign_AlignToGrid_STipTitle: return "对齐到网格";
//                case ReportStringId.RibbonXRDesign_AlignToGrid_STipContent: return "对齐到网格";

//                case ReportStringId.RibbonXRDesign_AlignLeft_Caption: return "左对齐";
//                case ReportStringId.RibbonXRDesign_AlignLeft_STipTitle: return "左对齐";
//                case ReportStringId.RibbonXRDesign_AlignLeft_STipContent: return "左对齐";

//                case ReportStringId.RibbonXRDesign_AlignVerticalCenters_Caption: return "居中对齐";
//                case ReportStringId.RibbonXRDesign_AlignVerticalCenters_STipTitle: return "居中对齐";
//                case ReportStringId.RibbonXRDesign_AlignVerticalCenters_STipContent: return "居中对齐";

//                case ReportStringId.RibbonXRDesign_AlignRight_Caption: return "右对齐";
//                case ReportStringId.RibbonXRDesign_AlignRight_STipTitle: return "右对齐";
//                case ReportStringId.RibbonXRDesign_AlignRight_STipContent: return "右对齐";

//                case ReportStringId.RibbonXRDesign_AlignTop_Caption: return "顶端对齐";
//                case ReportStringId.RibbonXRDesign_AlignTop_STipTitle: return "顶端对齐";
//                case ReportStringId.RibbonXRDesign_AlignTop_STipContent: return "顶端对齐";

//                case ReportStringId.RibbonXRDesign_AlignHorizontalCenters_Caption: return "中间对齐";
//                case ReportStringId.RibbonXRDesign_AlignHorizontalCenters_STipTitle: return "中间对齐";
//                case ReportStringId.RibbonXRDesign_AlignHorizontalCenters_STipContent: return "中间对齐";

//                case ReportStringId.RibbonXRDesign_AlignBottom_Caption: return "底端对齐";
//                case ReportStringId.RibbonXRDesign_AlignBottom_STipTitle: return "底端对齐";
//                case ReportStringId.RibbonXRDesign_AlignBottom_STipContent: return "底端对齐";
//                #endregion

//                #region 大小和布局
//                case ReportStringId.RibbonXRDesign_SizeToGrid_Caption: return "对齐到网格";
//                case ReportStringId.RibbonXRDesign_SizeToGrid_STipTitle: return "对齐到网格";
//                case ReportStringId.RibbonXRDesign_SizeToGrid_STipContent: return "对齐到网格";

//                case ReportStringId.RibbonXRDesign_SizeToControlWidth_Caption: return "使宽度相同";
//                case ReportStringId.RibbonXRDesign_SizeToControlWidth_STipTitle: return "使宽度相同";
//                case ReportStringId.RibbonXRDesign_SizeToControlWidth_STipContent: return "使宽度相同";

//                case ReportStringId.RibbonXRDesign_SizeToControlHeight_Caption: return "使高度相同";
//                case ReportStringId.RibbonXRDesign_SizeToControlHeight_STipTitle: return "使高度相同";
//                case ReportStringId.RibbonXRDesign_SizeToControlHeight_STipContent: return "使高度相同";

//                case ReportStringId.RibbonXRDesign_SizeToControl_Caption: return "使大小相同";
//                case ReportStringId.RibbonXRDesign_SizeToControl_STipTitle: return "使大小相同";
//                case ReportStringId.RibbonXRDesign_SizeToControl_STipContent: return "使大小相同";

//                case ReportStringId.RibbonXRDesign_HorizSpaceMakeEqual_Caption: return "使水平间距相等";
//                case ReportStringId.RibbonXRDesign_HorizSpaceMakeEqual_STipTitle: return "使水平间距相等";
//                case ReportStringId.RibbonXRDesign_HorizSpaceMakeEqual_STipContent: return "使水平间距相等";

//                case ReportStringId.RibbonXRDesign_HorizSpaceIncrease_Caption: return "增加水平间距";
//                case ReportStringId.RibbonXRDesign_HorizSpaceIncrease_STipTitle: return "增加水平间距";
//                case ReportStringId.RibbonXRDesign_HorizSpaceIncrease_STipContent: return "增加水平间距";

//                case ReportStringId.RibbonXRDesign_HorizSpaceDecrease_Caption: return "减小水平间距";
//                case ReportStringId.RibbonXRDesign_HorizSpaceDecrease_STipTitle: return "减小水平间距";
//                case ReportStringId.RibbonXRDesign_HorizSpaceDecrease_STipContent: return "减小水平间距";

//                case ReportStringId.RibbonXRDesign_HorizSpaceConcatenate_Caption: return "移除水平间距";
//                case ReportStringId.RibbonXRDesign_HorizSpaceConcatenate_STipTitle: return "移除水平间距";
//                case ReportStringId.RibbonXRDesign_HorizSpaceConcatenate_STipContent: return "移除水平间距";

//                case ReportStringId.RibbonXRDesign_VertSpaceMakeEqual_Caption: return "使垂直间距相等";
//                case ReportStringId.RibbonXRDesign_VertSpaceMakeEqual_STipTitle: return "使垂直间距相等";
//                case ReportStringId.RibbonXRDesign_VertSpaceMakeEqual_STipContent: return "使垂直间距相等";

//                case ReportStringId.RibbonXRDesign_VertSpaceIncrease_Caption: return "增加垂直间距";
//                case ReportStringId.RibbonXRDesign_VertSpaceIncrease_STipTitle: return "增加垂直间距";
//                case ReportStringId.RibbonXRDesign_VertSpaceIncrease_STipContent: return "增加垂直间距";

//                case ReportStringId.RibbonXRDesign_VertSpaceDecrease_Caption: return "减少垂直间距";
//                case ReportStringId.RibbonXRDesign_VertSpaceDecrease_STipTitle: return "减少垂直间距";
//                case ReportStringId.RibbonXRDesign_VertSpaceDecrease_STipContent: return "减少垂直间距";

//                case ReportStringId.RibbonXRDesign_VertSpaceConcatenate_Caption: return "移除垂直间距";
//                case ReportStringId.RibbonXRDesign_VertSpaceConcatenate_STipTitle: return "移除垂直间距";
//                case ReportStringId.RibbonXRDesign_VertSpaceConcatenate_STipContent: return "移除垂直间距";

//                case ReportStringId.RibbonXRDesign_CenterHorizontally_Caption: return "水平居中";
//                case ReportStringId.RibbonXRDesign_CenterHorizontally_STipTitle: return "水平居中";
//                case ReportStringId.RibbonXRDesign_CenterHorizontally_STipContent: return "水平居中";

//                case ReportStringId.RibbonXRDesign_CenterVertically_Caption: return "垂直居中";
//                case ReportStringId.RibbonXRDesign_CenterVertically_STipTitle: return "垂直居中";
//                case ReportStringId.RibbonXRDesign_CenterVertically_STipContent: return "垂直居中";

//                case ReportStringId.RibbonXRDesign_BringToFront_Caption: return "置于顶层";
//                case ReportStringId.RibbonXRDesign_BringToFront_STipTitle: return "置于顶层";
//                case ReportStringId.RibbonXRDesign_BringToFront_STipContent: return "置于顶层";

//                case ReportStringId.RibbonXRDesign_SendToBack_Caption: return "置于底层";
//                case ReportStringId.RibbonXRDesign_SendToBack_STipTitle: return "置于底层";
//                case ReportStringId.RibbonXRDesign_SendToBack_STipContent: return "置于底层";
//                #endregion

//                #region 缩放
//                case ReportStringId.RibbonXRDesign_ZoomOut_Caption: return "缩小";
//                case ReportStringId.RibbonXRDesign_ZoomOut_STipTitle: return "缩小";
//                case ReportStringId.RibbonXRDesign_ZoomOut_STipContent: return "缩小";
//                case ReportStringId.RibbonXRDesign_Zoom_Caption: return "缩放";
//                case ReportStringId.RibbonXRDesign_Zoom_STipTitle: return "缩放";
//                case ReportStringId.RibbonXRDesign_Zoom_STipContent: return "缩放";
//                case ReportStringId.RibbonXRDesign_ZoomIn_Caption: return "放大";
//                case ReportStringId.RibbonXRDesign_ZoomIn_STipTitle: return "放大";
//                case ReportStringId.RibbonXRDesign_ZoomIn_STipContent: return "放大";
//                case ReportStringId.RibbonXRDesign_ZoomExact_Caption: return "自定义缩放比例";
//                #endregion

//                #region 视图
//                case ReportStringId.RibbonXRDesign_Windows_Caption: return "窗口";
//                case ReportStringId.RibbonXRDesign_Windows_STipTitle: return "显示/隐藏窗口";
//                case ReportStringId.RibbonXRDesign_Windows_STipContent: return "显示/隐藏工具箱、对象浏览器、\r\n字段列表、属性等窗口";
//                #endregion

//                #region 脚本
//                case ReportStringId.RibbonXRDesign_Scripts_Caption: return "脚本";
//                case ReportStringId.RibbonXRDesign_Scripts_STipTitle: return "显示/隐藏脚本";
//                case ReportStringId.RibbonXRDesign_Scripts_STipContent: return "显示/隐藏脚本编辑器";
//                #endregion

//                #region 错误列表
//                case ReportStringId.ScriptEditor_ErrorDescription: return "错误内容";
//                case ReportStringId.ScriptEditor_ErrorLine: return "错误行";
//                case ReportStringId.ScriptEditor_ErrorColumn: return "错误列";
//                #endregion

//                #region 属性
//                case ReportStringId.CatDesign: return "设计";
//                case ReportStringId.CatParameters: return "报表参数";
//                case ReportStringId.CatPrinting: return "打印设置";
//                case ReportStringId.CatStructure: return "架构";
//                case ReportStringId.CatAppearance: return "外观";
//                case ReportStringId.CatNavigation: return "杂项";
//                case ReportStringId.CatData: return "数据";
//                case ReportStringId.CatBehavior: return "行为";
//                case ReportStringId.CatLayout: return "布局";
//                case ReportStringId.CatPageSettings: return "页面设置";
//                #endregion

//                #region report tasks
//                case ReportStringId.Verb_ReportWizard: return "使用报表向导设计报表...";
//                case ReportStringId.Verb_EditBands: return "编辑当前报表";
//                #endregion

//                #region Html视图
//                case ReportStringId.RibbonXRDesign_HtmlBackward_Caption: return "后退";
//                case ReportStringId.RibbonXRDesign_HtmlBackward_STipTitle: return "后退";
//                case ReportStringId.RibbonXRDesign_HtmlBackward_STipContent: return "返回到上一个页面";
//                case ReportStringId.RibbonXRDesign_HtmlForward_Caption: return "前进";
//                case ReportStringId.RibbonXRDesign_HtmlForward_STipTitle: return "前进";
//                case ReportStringId.RibbonXRDesign_HtmlForward_STipContent: return "前进到下一个页面";
//                case ReportStringId.RibbonXRDesign_HtmlFind_Caption: return "查找";
//                case ReportStringId.RibbonXRDesign_HtmlFind_STipTitle: return "查找";
//                case ReportStringId.RibbonXRDesign_HtmlFind_STipContent: return "搜索网页中包含的文字";

//                case ReportStringId.RibbonXRDesign_HtmlHome_Caption: return "首页";
//                case ReportStringId.RibbonXRDesign_HtmlHome_STipTitle: return "首页";
//                case ReportStringId.RibbonXRDesign_HtmlHome_STipContent: return "进入首页";
//                case ReportStringId.RibbonXRDesign_HtmlRefresh_Caption: return "刷新";
//                case ReportStringId.RibbonXRDesign_HtmlRefresh_STipTitle: return "刷新";
//                case ReportStringId.RibbonXRDesign_HtmlRefresh_STipContent: return "重新加载页面";
//                #endregion
                    
//            }
//            return base.GetLocalizedString(id);
//        }

//    }
//}
