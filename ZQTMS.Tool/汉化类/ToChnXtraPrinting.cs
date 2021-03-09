using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraPrinting.Localization;

namespace ZQTMS.Tool
{
    public class ToChnXtraPrinting:PreviewLocalizer
    {
        public override string GetLocalizedString(PreviewStringId id)
        {
            switch (id)
            {
                case PreviewStringId.Button_Apply: return "应用";
                case PreviewStringId.Button_Cancel: return "取消";
                case PreviewStringId.Button_Help: return "帮助";
                case PreviewStringId.Button_Ok: return "确定";
                case PreviewStringId.BarText_MainMenu: return "主菜单";
                case PreviewStringId.BarText_StatusBar: return "状态栏";
                case PreviewStringId.BarText_Toolbar: return "工具栏";

                case PreviewStringId.EMail_From: return "From";
                case PreviewStringId.MenuItem_XlsDocument: return "Xls文档";
                case PreviewStringId.MenuItem_XlsxDocument: return "Xlsx文档";
                case PreviewStringId.Margin_BottomMargin: return "下边界";
                case PreviewStringId.Margin_Inch: return "英寸";
                case PreviewStringId.Margin_LeftMargin: return "左边界";
                case PreviewStringId.Margin_Millimeter: return "毫米";
                case PreviewStringId.Margin_RightMargin: return "右边界";
                case PreviewStringId.Margin_TopMargin: return "上边界";
                case PreviewStringId.MenuItem_BackgrColor: return "颜色(&C)...";
                case PreviewStringId.MenuItem_Background: return "背景(&B)";
                case PreviewStringId.MenuItem_CsvDocument: return "Csv文件";
                case PreviewStringId.MenuItem_Exit: return "退出(&X)";
                case PreviewStringId.MenuItem_Export: return "导出(&E)";
                case PreviewStringId.MenuItem_File: return "文件(&F)";
                case PreviewStringId.MenuItem_GraphicDocument: return "图片文件";
                case PreviewStringId.MenuItem_HtmDocument: return "Html文件";
                case PreviewStringId.MenuItem_MhtDocument: return "Mht文件";
                case PreviewStringId.MenuItem_PageSetup: return "页面设置(&U)";
                case PreviewStringId.MenuItem_PdfDocument: return "Pdf文件";
                case PreviewStringId.MenuItem_Print: return "打印(&P)";
                case PreviewStringId.MenuItem_PrintDirect: return "直接打印(&R)";
                case PreviewStringId.MenuItem_RtfDocument: return "Rtf文件";
                case PreviewStringId.MenuItem_Send: return "发送(&D)";
                case PreviewStringId.MenuItem_TxtDocument: return "Txt文件";
                case PreviewStringId.MenuItem_View: return "视图(&V)";
                case PreviewStringId.MenuItem_ViewStatusbar: return "状态栏(&S)";
                case PreviewStringId.MenuItem_ViewToolbar: return "工具栏(&T)";
                case PreviewStringId.MenuItem_Watermark: return "水印(&W)";
                case PreviewStringId.MenuItem_PageLayout: return "页面视图";
                case PreviewStringId.MenuItem_ViewFacing: return "隐藏空白";
                case PreviewStringId.MenuItem_ViewContinuous: return "显示空白";
                case PreviewStringId.MenuItem_ZoomPageWidth: return "页面宽度";
                case PreviewStringId.MenuItem_ZoomTextWidth: return "文字宽度";
                case PreviewStringId.MenuItem_ZoomTwoPages: return "双页";
                case PreviewStringId.MenuItem_ZoomWholePage: return "整个页面";

                case PreviewStringId.ScalePopup_AdjustTo: return "调整为：";
                case PreviewStringId.ScalePopup_NormalSize: return "% 的正常大小";
                case PreviewStringId.ScalePopup_FitTo: return "调整为：";
                case PreviewStringId.ScalePopup_PagesWide: return "的页面宽度";
                case PreviewStringId.ScalePopup_GroupText: return "调整比例";

                case PreviewStringId.MPForm_Lbl_Pages: return "第 {0} 页";
                case PreviewStringId.Msg_CreatingDocument: return "正在生成文件...";
                case PreviewStringId.Msg_CustomDrawWarning: return "警告!";
                case PreviewStringId.Msg_EmptyDocument: return "此文件没有页面.";
                case PreviewStringId.Msg_FontInvalidNumber: return "字体大小不能为0或负数";
                case PreviewStringId.Msg_IncorrectPageRange: return "设置的页边界不正确";
                case PreviewStringId.Msg_NeedPrinter: return "没有安装打印机.";
                case PreviewStringId.Msg_NotSupportedFont: return "这种字体不被支持";
                case PreviewStringId.Msg_PageMarginsWarning: return "一个或以上的边界超出了打印范围.是否继续?";
                case PreviewStringId.Msg_Caption: return "系统提示";
                case PreviewStringId.Msg_WrongPageSettings: return "打印机不支持所选的纸张大小. 是否继续打印?";
                case PreviewStringId.Msg_WrongPrinter: return "无效的打印机名称.请检查打印机的设置是否正确";
                case PreviewStringId.Msg_SearchDialogFinishedSearching: return "搜索完成";
                case PreviewStringId.Msg_SearchDialogTotalFound: return "共找到：";
                case PreviewStringId.Msg_SearchDialogReady: return "准备";
                case PreviewStringId.Msg_CantFitBarcodeToControlBounds: return "条形码控件的边界太小";
                case PreviewStringId.Msg_InvalidBarcodeText: return "条形码中有无效的字符";
                case PreviewStringId.Msg_InvalidBarcodeTextFormat: return "无效的文本格式";
                case PreviewStringId.PageInfo_PageDate: return "[打印日期]";
                case PreviewStringId.PageInfo_PageNumber: return "[页码 #]";
                case PreviewStringId.PageInfo_PageNumberOfTotal: return "[第 # 页  共 # 页]";
                case PreviewStringId.PageInfo_PageTime: return "[打印时间]";
                case PreviewStringId.PageInfo_PageUserName: return "[用户名]";
                case PreviewStringId.PreviewForm_Caption: return "打印预览";
                case PreviewStringId.SaveDlg_FilterBmp: return "BMP 图片文件";
                case PreviewStringId.SaveDlg_FilterCsv: return "CSV 文件";
                case PreviewStringId.SaveDlg_FilterGif: return "GIF 图片文件";
                case PreviewStringId.SaveDlg_FilterHtm: return "HTML 文件";
                case PreviewStringId.SaveDlg_FilterJpeg: return "JPEG 图片文件";
                case PreviewStringId.SaveDlg_FilterMht: return "MHT 文件";
                case PreviewStringId.SaveDlg_FilterPdf: return "PDF 文件";
                case PreviewStringId.SaveDlg_FilterPng: return "PNG 图片文件";
                case PreviewStringId.SaveDlg_FilterRtf: return "RTF 文件";
                case PreviewStringId.SaveDlg_FilterTiff: return "TIFF 图片文件";
                case PreviewStringId.SaveDlg_FilterTxt: return "TXT 文本文件";
                case PreviewStringId.SaveDlg_FilterWmf: return "WMF 图片文件";
                case PreviewStringId.SaveDlg_FilterXls: return "Excel 文件";
                case PreviewStringId.SaveDlg_FilterEmf: return "EMF 图片文件";
                case PreviewStringId.SaveDlg_Title: return "另存为";
                case PreviewStringId.SB_PageInfo: return "第 {0} 页 共 {1} 页";
                case PreviewStringId.SB_PageNone: return "无";
                case PreviewStringId.SB_ZoomFactor: return "缩放比例:";
                case PreviewStringId.SB_PageOfPages: return "第 {0} 页 共 {1} 页";
                case PreviewStringId.SB_TTip_Stop: return "停止";
                case PreviewStringId.ScrollingInfo_Page: return "页";
                case PreviewStringId.TB_TTip_Backgr: return "背景色";
                case PreviewStringId.TB_TTip_Close: return "退出";
                case PreviewStringId.TB_TTip_Customize: return "自定义";
                case PreviewStringId.TB_TTip_EditPageHF: return "页眉页脚";
                case PreviewStringId.TB_TTip_Export: return "导出文件...";
                case PreviewStringId.TB_TTip_FirstPage: return "首页";
                case PreviewStringId.TB_TTip_HandTool: return "移动工具";
                case PreviewStringId.TB_TTip_LastPage: return "尾页";
                case PreviewStringId.TB_TTip_Magnifier: return "放大/缩小";
                case PreviewStringId.TB_TTip_Map: return "文档视图";
                case PreviewStringId.TB_TTip_MultiplePages: return "多页显示";
                case PreviewStringId.TB_TTip_NextPage: return "下一页";
                case PreviewStringId.TB_TTip_PageSetup: return "页面设置";
                case PreviewStringId.TB_TTip_PreviousPage: return "上一页";
                case PreviewStringId.TB_TTip_Print: return "打印";
                case PreviewStringId.TB_TTip_PrintDirect: return "直接打印";
                case PreviewStringId.TB_TTip_Search: return "查找";
                case PreviewStringId.TB_TTip_Send: return "发送E-Mail...";
                case PreviewStringId.TB_TTip_Watermark: return "水印";
                case PreviewStringId.TB_TTip_Zoom: return "缩放";
                case PreviewStringId.TB_TTip_ZoomIn: return "放大";
                case PreviewStringId.TB_TTip_ZoomOut: return "缩小";
                case PreviewStringId.TB_TTip_Open: return "打开文档";
                case PreviewStringId.TB_TTip_Save: return "保存文档";
                case PreviewStringId.TB_TTip_Parameters: return "参数";
                case PreviewStringId.TB_TTip_Scale: return "页面比例";

                case PreviewStringId.WMForm_Direction_BackwardDiagonal: return "反向倾斜";
                case PreviewStringId.WMForm_Direction_ForwardDiagonal: return "正向倾斜";
                case PreviewStringId.WMForm_Direction_Horizontal: return "横向";
                case PreviewStringId.WMForm_Direction_Vertical: return "纵向";
                case PreviewStringId.WMForm_HorzAlign_Center: return "置中";
                case PreviewStringId.WMForm_HorzAlign_Left: return "靠左";
                case PreviewStringId.WMForm_HorzAlign_Right: return "靠右";
                case PreviewStringId.WMForm_ImageClip: return "剪辑";
                case PreviewStringId.WMForm_ImageStretch: return "拉伸";
                case PreviewStringId.WMForm_ImageZoom: return "缩放";
                case PreviewStringId.WMForm_PageRangeRgrItem_All: return "全部";
                case PreviewStringId.WMForm_PageRangeRgrItem_Pages: return "页码";
                case PreviewStringId.WMForm_PictureDlg_Title: return "选择图片";
                case PreviewStringId.WMForm_VertAlign_Bottom: return "底端";
                case PreviewStringId.WMForm_VertAlign_Middle: return "中间";
                case PreviewStringId.WMForm_VertAlign_Top: return "顶端";
                case PreviewStringId.WMForm_Watermark_Asap: return "ASAP";
                case PreviewStringId.WMForm_Watermark_Confidential: return "机密";
                case PreviewStringId.WMForm_Watermark_Copy: return "副本";
                case PreviewStringId.WMForm_Watermark_DoNotCopy: return "禁止拷贝";
                case PreviewStringId.WMForm_Watermark_Draft: return "草稿";
                case PreviewStringId.WMForm_Watermark_Evaluation: return "评价";
                case PreviewStringId.WMForm_Watermark_Original: return "原文";
                case PreviewStringId.WMForm_Watermark_Personal: return "个人";
                case PreviewStringId.WMForm_Watermark_Sample: return "样稿";
                case PreviewStringId.WMForm_Watermark_TopSecret: return "高度机密";
                case PreviewStringId.WMForm_Watermark_Urgent: return "简稿";
                case PreviewStringId.WMForm_ZOrderRgrItem_Behind: return "在后面";
                case PreviewStringId.WMForm_ZOrderRgrItem_InFront: return "在前面";


                case PreviewStringId.ExportOption_PdfPageRange: return "页面范围：";
                case PreviewStringId.ExportOption_PdfNeverEmbeddedFonts: return "不嵌入字体：";
                case PreviewStringId.ExportOption_PdfImageQuality: return "图片质量：";
                case PreviewStringId.ExportOption_PdfImageQuality_Highest: return "最高";
                case PreviewStringId.ExportOption_PdfImageQuality_High: return "高";
                case PreviewStringId.ExportOption_PdfImageQuality_Medium: return "中等";
                case PreviewStringId.ExportOption_PdfImageQuality_Low: return "低";
                case PreviewStringId.ExportOption_PdfImageQuality_Lowest: return "最低";

                case PreviewStringId.ExportOption_PdfCompressed: return "压缩";
                case PreviewStringId.ExportOption_PdfShowPrintDialogOnOpen: return "打开时显示打印对话框";
                case PreviewStringId.ExportOption_PdfPasswordSecurityOptions: return "安全密码：";
                case PreviewStringId.ExportOption_PdfDocumentApplication: return "程序";
                case PreviewStringId.ExportOption_PdfDocumentAuthor: return "作者";
                case PreviewStringId.ExportOption_PdfDocumentKeywords: return "关键字";
                case PreviewStringId.ExportOption_PdfDocumentSubject: return "主题";
                case PreviewStringId.ExportOption_PdfDocumentTitle: return "标题";

                case PreviewStringId.ExportOption_ImageFormat: return "图片格式：";
                case PreviewStringId.ExportOption_ImageResolution: return "分辨率(dpi)：";
                case PreviewStringId.ExportOption_ImageExportMode: return "导出模式：";
                case PreviewStringId.ExportOption_ImageExportMode_SingleFile: return "单个文件";
                case PreviewStringId.ExportOption_ImageExportMode_DifferentFiles: return "不同文件";
                case PreviewStringId.ExportOption_ImageExportMode_SingleFilePageByPage: return "每页一个文件";
                case PreviewStringId.ExportOption_ImagePageRange: return "页面范围：";
                case PreviewStringId.ExportOption_ImagePageBorderColor: return "边框颜色：";
                case PreviewStringId.ExportOption_ImagePageBorderWidth: return "边框宽度：";

                case PreviewStringId.ExportOptionsForm_CaptionCsv: return "导出设置(CSV)";
                case PreviewStringId.ExportOptionsForm_CaptionHtml: return "导出设置(HTML)";
                case PreviewStringId.ExportOptionsForm_CaptionImage: return "导出设置(图片)";
                case PreviewStringId.ExportOptionsForm_CaptionMht: return "导出设置(MHT)";
                case PreviewStringId.ExportOptionsForm_CaptionPdf: return "导出设置(PDF)";
                case PreviewStringId.ExportOptionsForm_CaptionRtf: return "导出设置(RTF)";
                case PreviewStringId.ExportOptionsForm_CaptionTxt: return "导出设置(TXT)";
                case PreviewStringId.ExportOptionsForm_CaptionXls: return "导出设置(XLS)";
                case PreviewStringId.ExportOptionsForm_CaptionXlsx: return "导出设置(XLSX)";
                case PreviewStringId.ExportOptionsForm_CaptionXps: return "导出设置(XPS)";

                /////
                case PreviewStringId.RibbonPreview_PageText: return "预览视图";

                case PreviewStringId.RibbonPreview_ClosePreview_Caption: return "关闭预览";
                case PreviewStringId.RibbonPreview_ClosePreview_STipTitle: return "关闭预览";
                case PreviewStringId.RibbonPreview_ClosePreview_STipContent: return "关闭预览";


                #region 主分组
                case PreviewStringId.RibbonPreview_PageGroup_Document: return "文档";
                case PreviewStringId.RibbonPreview_PageGroup_Print: return "打印";
                case PreviewStringId.RibbonPreview_PageGroup_PageSetup: return "页面设置";
                case PreviewStringId.RibbonPreview_PageGroup_PageSetup_STipTitle: return "页面设置";
                case PreviewStringId.RibbonPreview_PageGroup_PageSetup_STipContent: return "页面设置";
                case PreviewStringId.RibbonPreview_PageGroup_Navigation: return "导航";
                case PreviewStringId.RibbonPreview_PageGroup_Zoom: return "缩放";
                case PreviewStringId.RibbonPreview_PageGroup_Background: return "页面背景";
                case PreviewStringId.RibbonPreview_PageGroup_Export: return "导出";
                #endregion

                #region 文档
                case PreviewStringId.RibbonPreview_Open_Caption: return "打开";
                case PreviewStringId.RibbonPreview_Open_STipTitle: return "打开";
                case PreviewStringId.RibbonPreview_Open_STipContent: return "打开一个报表文档";

                case PreviewStringId.RibbonPreview_Save_Caption: return "保存";
                case PreviewStringId.RibbonPreview_Save_STipTitle: return "保存";
                case PreviewStringId.RibbonPreview_Save_STipContent: return "保存";
                #endregion

                #region 打印
                case PreviewStringId.RibbonPreview_Print_Caption: return "打印";
                case PreviewStringId.RibbonPreview_Print_STipTitle: return "打印";
                case PreviewStringId.RibbonPreview_Print_STipContent: return "打印";

                case PreviewStringId.RibbonPreview_PrintDirect_Caption: return "快速打印";
                case PreviewStringId.RibbonPreview_PrintDirect_STipTitle: return "快速打印";
                case PreviewStringId.RibbonPreview_PrintDirect_STipContent: return "快速打印";

                case PreviewStringId.RibbonPreview_PageSetup_Caption: return "打印设置";
                case PreviewStringId.RibbonPreview_PageSetup_STipTitle: return "打印设置";
                case PreviewStringId.RibbonPreview_PageSetup_STipContent: return "打印设置";

                case PreviewStringId.RibbonPreview_Customize_Caption: return "自定义设置";
                case PreviewStringId.RibbonPreview_Customize_STipTitle: return "自定义设置";
                case PreviewStringId.RibbonPreview_Customize_STipContent: return "自定义设置";

                case PreviewStringId.RibbonPreview_Parameters_Caption: return "报表参数";
                case PreviewStringId.RibbonPreview_Parameters_STipTitle: return "报表参数";
                case PreviewStringId.RibbonPreview_Parameters_STipContent: return "报表参数";
                #endregion

                #region 页面设置
                case PreviewStringId.RibbonPreview_EditPageHF_Caption: return "页眉页脚";
                case PreviewStringId.RibbonPreview_EditPageHF_STipTitle: return "页眉页脚";
                case PreviewStringId.RibbonPreview_EditPageHF_STipContent: return "页眉页脚";

                case PreviewStringId.RibbonPreview_Scale_Caption: return "比例";
                case PreviewStringId.RibbonPreview_Scale_STipTitle: return "比例";
                case PreviewStringId.RibbonPreview_Scale_STipContent: return "设置打印缩放百分比";

                case PreviewStringId.RibbonPreview_PageMargins_Caption: return "页边距";
                case PreviewStringId.RibbonPreview_PageMargins_STipTitle: return "页边距";
                case PreviewStringId.RibbonPreview_PageMargins_STipContent: return "页边距";

                case PreviewStringId.RibbonPreview_PageOrientation_Caption: return "页面方向";
                case PreviewStringId.RibbonPreview_PageOrientation_STipTitle: return "页面方向";
                case PreviewStringId.RibbonPreview_PageOrientation_STipContent: return "设置页面的横纵向";

                case PreviewStringId.RibbonPreview_PaperSize_Caption: return "纸张";
                case PreviewStringId.RibbonPreview_PaperSize_STipTitle: return "纸张";
                case PreviewStringId.RibbonPreview_PaperSize_STipContent: return "设置纸张类型";

                #endregion

                #region 导航
                case PreviewStringId.RibbonPreview_Find_Caption: return "查找";
                case PreviewStringId.RibbonPreview_Find_STipTitle: return "查找";
                case PreviewStringId.RibbonPreview_Find_STipContent: return "查找报表中包含的文字";

                case PreviewStringId.RibbonPreview_DocumentMap_Caption: return "文档地图";
                case PreviewStringId.RibbonPreview_DocumentMap_STipTitle: return "文档地图";
                case PreviewStringId.RibbonPreview_DocumentMap_STipContent: return "文档地图";

                case PreviewStringId.RibbonPreview_ShowFirstPage_Caption: return "首页";
                case PreviewStringId.RibbonPreview_ShowFirstPage_STipTitle: return "首页";
                case PreviewStringId.RibbonPreview_ShowFirstPage_STipContent: return "首页";

                case PreviewStringId.RibbonPreview_ShowPrevPage_Caption: return "上一页";
                case PreviewStringId.RibbonPreview_ShowPrevPage_STipTitle: return "上一页";
                case PreviewStringId.RibbonPreview_ShowPrevPage_STipContent: return "上一页";

                case PreviewStringId.RibbonPreview_ShowNextPage_Caption: return "下一页";
                case PreviewStringId.RibbonPreview_ShowNextPage_STipTitle: return "下一页";
                case PreviewStringId.RibbonPreview_ShowNextPage_STipContent: return "下一页";

                case PreviewStringId.RibbonPreview_ShowLastPage_Caption: return "尾页";
                case PreviewStringId.RibbonPreview_ShowLastPage_STipTitle: return "尾页";
                case PreviewStringId.RibbonPreview_ShowLastPage_STipContent: return "尾页";

                #endregion

                #region 缩放
                case PreviewStringId.RibbonPreview_Pointer_Caption: return "指针鼠标";
                case PreviewStringId.RibbonPreview_Pointer_STipTitle: return "指针鼠标";
                case PreviewStringId.RibbonPreview_Pointer_STipContent: return "指针鼠标";

                case PreviewStringId.RibbonPreview_HandTool_Caption: return "手形鼠标";
                case PreviewStringId.RibbonPreview_HandTool_STipTitle: return "手形鼠标";
                case PreviewStringId.RibbonPreview_HandTool_STipContent: return "让鼠标指针变成手形,用于移动页面";

                case PreviewStringId.RibbonPreview_Magnifier_Caption: return "缩放鼠标";
                case PreviewStringId.RibbonPreview_Magnifier_STipTitle: return "缩放鼠标";
                case PreviewStringId.RibbonPreview_Magnifier_STipContent: return "让鼠标指针变成手形,用于快速缩放页面";

                case PreviewStringId.RibbonPreview_MultiplePages_Caption: return "多页面视图";
                case PreviewStringId.RibbonPreview_MultiplePages_STipTitle: return "多页面视图";
                case PreviewStringId.RibbonPreview_MultiplePages_STipContent: return "多页面视图";

                case PreviewStringId.RibbonPreview_ZoomOut_Caption: return "缩小";
                case PreviewStringId.RibbonPreview_ZoomOut_STipTitle: return "缩小";
                case PreviewStringId.RibbonPreview_ZoomOut_STipContent: return "缩小";

                case PreviewStringId.RibbonPreview_Zoom_Caption: return "缩放";
                case PreviewStringId.RibbonPreview_Zoom_STipTitle: return "缩放";
                case PreviewStringId.RibbonPreview_Zoom_STipContent: return "缩放";
                case PreviewStringId.RibbonPreview_ZoomExact_Caption: return "自定义缩放比例";

                case PreviewStringId.RibbonPreview_ZoomIn_Caption: return "放大";
                case PreviewStringId.RibbonPreview_ZoomIn_STipTitle: return "放大";
                case PreviewStringId.RibbonPreview_ZoomIn_STipContent: return "放大";
                #endregion

                #region 页面背景
                case PreviewStringId.RibbonPreview_FillBackground_Caption: return "背景颜色";
                case PreviewStringId.RibbonPreview_FillBackground_STipTitle: return "背景颜色";
                case PreviewStringId.RibbonPreview_FillBackground_STipContent: return "背景颜色";

                case PreviewStringId.RibbonPreview_Watermark_Caption: return "水印";
                case PreviewStringId.RibbonPreview_Watermark_STipTitle: return "水印";
                case PreviewStringId.RibbonPreview_Watermark_STipContent: return "插入一个字符串或图片作为水印";
                #endregion

                #region
                case PreviewStringId.RibbonPreview_ExportFile_Caption: return "导出";
                case PreviewStringId.RibbonPreview_ExportFile_STipTitle: return "导出";
                case PreviewStringId.RibbonPreview_ExportFile_STipContent: return "导出";

                case PreviewStringId.RibbonPreview_ExportPdf_Caption: return "Pdf文档";
                case PreviewStringId.RibbonPreview_ExportPdf_STipTitle: return "Pdf文档";
                case PreviewStringId.RibbonPreview_ExportPdf_STipContent: return "Pdf文档";
                case PreviewStringId.RibbonPreview_ExportPdf_Description: return "Pdf格式文件";

                case PreviewStringId.RibbonPreview_ExportHtm_Caption: return "Html文档";
                case PreviewStringId.RibbonPreview_ExportHtm_STipTitle: return "Html文档";
                case PreviewStringId.RibbonPreview_ExportHtm_STipContent: return "Html文档";
                case PreviewStringId.RibbonPreview_ExportHtm_Description: return "Html网页文件";

                case PreviewStringId.RibbonPreview_ExportMht_Caption: return "Mht文档";
                case PreviewStringId.RibbonPreview_ExportMht_STipTitle: return "Mht文档";
                case PreviewStringId.RibbonPreview_ExportMht_STipContent: return "Mht文档";
                case PreviewStringId.RibbonPreview_ExportMht_Description: return "Mht单页面文件";

                case PreviewStringId.RibbonPreview_ExportRtf_Caption: return "Rtf文档";
                case PreviewStringId.RibbonPreview_ExportRtf_STipTitle: return "Rtf文档";
                case PreviewStringId.RibbonPreview_ExportRtf_STipContent: return "Rtf文档";
                case PreviewStringId.RibbonPreview_ExportRtf_Description: return "Rtf格式文件";

                case PreviewStringId.RibbonPreview_ExportXls_Caption: return "Xls文档";
                case PreviewStringId.RibbonPreview_ExportXls_STipTitle: return "Xls文档";
                case PreviewStringId.RibbonPreview_ExportXls_STipContent: return "Xls文档";
                case PreviewStringId.RibbonPreview_ExportXls_Description: return "Microsoft Excel 2000-2003格式工作表";

                case PreviewStringId.RibbonPreview_ExportXlsx_Caption: return "Xlsx文档";
                case PreviewStringId.RibbonPreview_ExportXlsx_STipTitle: return "Xlsx文档";
                case PreviewStringId.RibbonPreview_ExportXlsx_STipContent: return "Xlsx文档";
                case PreviewStringId.RibbonPreview_ExportXlsx_Description: return "Microsoft Excel 2007-2010格式工作表";

                case PreviewStringId.RibbonPreview_ExportCsv_Caption: return "Csv文档";
                case PreviewStringId.RibbonPreview_ExportCsv_STipTitle: return "Csv文档";
                case PreviewStringId.RibbonPreview_ExportCsv_STipContent: return "Csv文档";
                case PreviewStringId.RibbonPreview_ExportCsv_Description: return "Csv格式的由符号分隔的文件";

                case PreviewStringId.RibbonPreview_ExportTxt_Caption: return "Txt文档";
                case PreviewStringId.RibbonPreview_ExportTxt_STipTitle: return "Txt文档";
                case PreviewStringId.RibbonPreview_ExportTxt_STipContent: return "Txt文档";
                case PreviewStringId.RibbonPreview_ExportTxt_Description: return "Txt记事本文件";

                case PreviewStringId.RibbonPreview_ExportGraphic_Caption: return "图片文件";
                case PreviewStringId.RibbonPreview_ExportGraphic_STipTitle: return "图片文件";
                case PreviewStringId.RibbonPreview_ExportGraphic_STipContent: return "图片文件";
                case PreviewStringId.RibbonPreview_ExportGraphic_Description: return "导出为Gif/Bmp/Jpeg/Png/Tiff/Emf/Wmf格式的图片";

                case PreviewStringId.RibbonPreview_ExportXps_Caption: return "Xps文档";
                case PreviewStringId.RibbonPreview_ExportXps_Description: return "Xps格式文件";

                ////////////
                case PreviewStringId.RibbonPreview_SendFile_Caption: return "邮件发送";
                case PreviewStringId.RibbonPreview_SendFile_STipTitle: return "邮件发送";
                case PreviewStringId.RibbonPreview_SendFile_STipContent: return "邮件发送";

                case PreviewStringId.RibbonPreview_SendPdf_Caption: return "Pdf文档";
                case PreviewStringId.RibbonPreview_SendPdf_STipTitle: return "Pdf文档";
                case PreviewStringId.RibbonPreview_SendPdf_STipContent: return "Pdf文档";
                case PreviewStringId.RibbonPreview_SendPdf_Description: return "Pdf格式文件";

                case PreviewStringId.RibbonPreview_SendMht_Caption: return "Mht文档";
                case PreviewStringId.RibbonPreview_SendMht_STipTitle: return "Mht文档";
                case PreviewStringId.RibbonPreview_SendMht_STipContent: return "Mht文档";
                case PreviewStringId.RibbonPreview_SendMht_Description: return "Mht单页面文件";

                case PreviewStringId.RibbonPreview_SendRtf_Caption: return "Rtf文档";
                case PreviewStringId.RibbonPreview_SendRtf_STipTitle: return "Rtf文档";
                case PreviewStringId.RibbonPreview_SendRtf_STipContent: return "Rtf文档";
                case PreviewStringId.RibbonPreview_SendRtf_Description: return "Rtf格式文件";

                case PreviewStringId.RibbonPreview_SendXls_Caption: return "Xls文档";
                case PreviewStringId.RibbonPreview_SendXls_STipTitle: return "Xls文档";
                case PreviewStringId.RibbonPreview_SendXls_STipContent: return "Xls文档";
                case PreviewStringId.RibbonPreview_SendXls_Description: return "Microsoft Excel 2000-2003格式工作表";

                case PreviewStringId.RibbonPreview_SendXlsx_Caption: return "Xlsx文档";
                case PreviewStringId.RibbonPreview_SendXlsx_STipTitle: return "Xlsx文档";
                case PreviewStringId.RibbonPreview_SendXlsx_STipContent: return "Xlsx文档";
                case PreviewStringId.RibbonPreview_SendXlsx_Description: return "Microsoft Excel 2007-2010格式工作表";

                case PreviewStringId.RibbonPreview_SendCsv_Caption: return "Csv文档";
                case PreviewStringId.RibbonPreview_SendCsv_STipTitle: return "Csv文档";
                case PreviewStringId.RibbonPreview_SendCsv_STipContent: return "Csv文档";
                case PreviewStringId.RibbonPreview_SendCsv_Description: return "Csv格式的由符号分隔的文件";

                case PreviewStringId.RibbonPreview_SendTxt_Caption: return "Txt文档";
                case PreviewStringId.RibbonPreview_SendTxt_STipTitle: return "Txt文档";
                case PreviewStringId.RibbonPreview_SendTxt_STipContent: return "Txt文档";
                case PreviewStringId.RibbonPreview_SendTxt_Description: return "Txt记事本文件";

                case PreviewStringId.RibbonPreview_SendGraphic_Caption: return "图片文件";
                case PreviewStringId.RibbonPreview_SendGraphic_STipTitle: return "图片文件";
                case PreviewStringId.RibbonPreview_SendGraphic_STipContent: return "图片文件";
                case PreviewStringId.RibbonPreview_SendGraphic_Description: return "导出为Gif/Bmp/Jpeg/Png/Tiff/Emf/Wmf格式的图片";

                case PreviewStringId.RibbonPreview_SendXps_Caption: return "Xps文档";
                case PreviewStringId.RibbonPreview_SendXps_Description: return "Xps格式文件";
                #endregion
                    
            }
            return base.GetLocalizedString(id);
        }

    }
}
