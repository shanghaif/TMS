//using System;
//using System.Collections.Generic;
//using System.Text;
//using DevExpress.XtraCharts.Localization;

//namespace ZQTMS.Tool
//{
//    public class ToChnXtraChart : ChartLocalizer
//    {
//        public override string GetLocalizedString(ChartStringId id)
//        {
//            switch (id)
//            {
//                case ChartStringId.SeriesPrefix: return "级联 ";
//                case ChartStringId.PalettePrefix: return "调色板 ";
//                case ChartStringId.ArgumentMember: return "参数";
//                case ChartStringId.ValueMember: return "值";
//                case ChartStringId.LowValueMember: return "低";
//                case ChartStringId.HighValueMember: return "高";
//                case ChartStringId.OpenValueMember: return "开启";
//                case ChartStringId.CloseValueMember: return "关闭";
//                case ChartStringId.DefaultDataFilterName: return "数据筛选";
//                case ChartStringId.DefaultChartTitle: return "图表标题";
//                case ChartStringId.MsgSeriesViewDoesNotExist: return "{0}级联视图不存在。";
//                case ChartStringId.MsgEmptyArrayOfValues: return "数组值为空。";
//                case ChartStringId.MsgItemNotInCollection: return "此聚集不能包含指定项。";
//                case ChartStringId.MsgIncorrectValue: return "指定的值非法： {0}\n参数名： {1}";
//                case ChartStringId.MsgIncompatiblePointType: return "The type of the \"{0}\" point isn't compatible with the {1} scale.";
//                case ChartStringId.MsgIncompatibleArgumentDataMember: return "The type of the \"{0}\" argument data member isn't compatible with the {1} scale.";
//                case ChartStringId.MsgDesignTimeOnlySetting: return "此属性不能设置运行时间。";
//                case ChartStringId.MsgInvalidDataSource: return "无效的数据源类型(不支持接口实施)";
//                case ChartStringId.MsgIncorrectDataMember: return "指定的数据源中不包含\"{0}\"字段";
//                case ChartStringId.MsgInvalidSortingKey: return "不能设置排序关键词的值为 {0}";
//                case ChartStringId.MsgInvalidFilterCondition: return "条件 {0} 不能适用于 \"{1}\" 数据";
//                case ChartStringId.MsgIncorrectDataAdapter: return "{0} 对象不是数据适配器";
//                case ChartStringId.MsgDataSnapshot: return "The data snapshot is complete. All series data now statically persist in the chart. Note, this also means that the chart is now in unbound mode.";
//                case ChartStringId.MsgModifyDefaultPaletteError: return "调色板是默认的，于是不能被修改";
//                case ChartStringId.MsgAddExistingPaletteError: return "{0} 调色板已经存在于储存裤中";
//                case ChartStringId.MsgInternalPropertyChangeError: return "此属性仅仅在内部使用，你不允许改变它的值";
//                case ChartStringId.MsgPaletteNotFound: return "图表不能包含 {0} 调色板";
//                case ChartStringId.MsgLabelSettingRuntimeError: return "\"标签\"属性不能设置运行时间";
//                case ChartStringId.MsgPointOptionsSettingRuntimeError: return "\"点选项\"属性不能设置运行时间";
//                case ChartStringId.MsgIncorrectNumericPrecision: return "精确度应该大于等于0";
//                case ChartStringId.MsgIncorrectAxisThickness: return "坐标宽度应该大于0";
//                case ChartStringId.MsgIncorrectBarWidth: return "条宽应该大于等于0";
//                case ChartStringId.MsgIncorrectBorderThickness: return "边框宽度应该大于0";
//                case ChartStringId.MsgIncorrectChartTitleIndent: return "标题缩进应该大于等于0小于1000";
//                case ChartStringId.MsgIncorrectLegendMarkerSize: return "图例大小应该大于0小于1000";
//                case ChartStringId.MsgIncorrectLegendSpacingVertical: return "图例垂直间距应该大于等于0小于1000";
//                case ChartStringId.MsgIncorrectLegendSpacingHorizontal: return "图例水平间距应该大于等于0小于1000";
//                case ChartStringId.MsgIncorrectMarkerSize: return "标记大小应该大于1";
//                case ChartStringId.MsgIncorrectMarkerStarPointCount: return "点的数目应该大于3小于101";
//                case ChartStringId.MsgIncorrectPieSeriesLabelColumnIndent: return "柱型图缩进应该大于等于0";
//                case ChartStringId.MsgIncorrectPercentPrecision: return "百分比的精确度应该大于0";
//                case ChartStringId.MsgIncorrectSeriesLabelLineLength: return "线条长度应该大于等于0小于1000";
//                case ChartStringId.MsgIncorrectStripMinLimit: return "条最小界限应该小于最大界限";
//                case ChartStringId.MsgIncorrectStripMaxLimit: return "条最大界限应该大于最小界限";
//                case ChartStringId.MsgIncorrectLineThickness: return "线条宽度应该大于0";
//                case ChartStringId.MsgIncorrectShadowSize: return "阴影大小应该大于0";
//                case ChartStringId.MsgIncorrectTickmarkThickness: return "刻度线宽度应该大于0";
//                case ChartStringId.MsgIncorrectTickmarkLength: return "刻度线长度应该大于0";
//                case ChartStringId.MsgIncorrectTickmarkMinorThickness: return "刻度线较小的宽度应该大于0";
//                case ChartStringId.MsgIncorrectTickmarkMinorLength: return "刻度线较小的长度应该大于0";
//                case ChartStringId.MsgIncorrectPercentValue: return "百分率应该大于等于0小于等于100";
//                case ChartStringId.MsgIncorrectSimpleDiagramDimension: return "简单图表的尺寸应该大于0小于100";
//                case ChartStringId.MsgIncorrectStockLevelLineLengthValue: return "股票的水平线长度应该大于等于0";
//                case ChartStringId.MsgIncorrectReductionColorValue: return "降低颜色值不能为空";
//                case ChartStringId.MsgIncorrectLabelAngle: return "标签的角度应该大于等于-360小于等于360";
//                case ChartStringId.MsgIncorrectImageBounds: return "不能创建图像为指定的大小";
//                case ChartStringId.MsgIncorrectUseImageProperty: return "图像属性不能使用在Web图表控件，请使用图像URL属性代替";
//                case ChartStringId.MsgIncorrectUseImageUrlProperty: return "图像URL属性只能使用在Web图表控件，请使用图像属性代替";
//                case ChartStringId.MsgEmptyArgument: return "参数不能为空。";
//                case ChartStringId.MsgIncorrectImageFormat: return "不能导出图表为指定的图像格式";
//                case ChartStringId.MsgIncorrectValueDataMemberCount: return "必须指定当前级联视图 {0} 数据项值。";
//                case ChartStringId.MsgPaletteEditingIsntAllowed: return "不允许编辑！";
//                case ChartStringId.MsgPaletteDoubleClickToEdit: return "双击进行编辑...";
//                case ChartStringId.MsgInvalidPaletteName: return "Can't add a palette which has an empty name (\"\") to the palette repository. Please, specify a name for the palette.";
//                case ChartStringId.VerbAbout: return "关于";
//                case ChartStringId.VerbAboutDescription: return "在XtraCharts显示基本信息";
//                case ChartStringId.VerbPopulate: return "填充";
//                case ChartStringId.VerbPopulateDescription: return "填充图表数据源";
//                case ChartStringId.VerbClearDataSource: return "清除数据源";
//                case ChartStringId.VerbClearDataSourceDescription: return "清除图表数据源";
//                case ChartStringId.VerbDataSnapshot: return "数据抽点打印";
//                case ChartStringId.VerbDataSnapshotDescription: return "从图表范围数据源复制数据和拆分数据源。";
//                case ChartStringId.VerbSeries: return "级联...";

//                case ChartStringId.VerbSeriesDescription: return "打开编辑聚集级联";
//                case ChartStringId.VerbEditPalettes: return "编辑调色板...";
//                case ChartStringId.VerbEditPalettesDescription: return "打开编辑调色板。";
//                case ChartStringId.VerbWizard: return "向导...";
//                case ChartStringId.VerbWizardDescription: return "运行图表向导，允许编辑哪个图表属性。";
//                case ChartStringId.PieIncorrectValuesText: return "饼图不能描绘负数。所有的值必须大于等于0。";
//                case ChartStringId.FontFormat: return "{0}, {1}pt, {2}";
//                case ChartStringId.TrnSeriesChanged: return "级联更改";
//                case ChartStringId.TrnDataFiltersChanged: return "数据筛选更改";
//                case ChartStringId.TrnChartTitlesChanged: return "图表标题更改";
//                case ChartStringId.TrnPalettesChanged: return "调色板更改";
//                case ChartStringId.TrnConstantLinesChanged: return "不变行更改";
//                case ChartStringId.TrnStripsChanged: return "条更改";
//                case ChartStringId.TrnCustomAxisLabelChanged: return "自定坐标更改";
//                case ChartStringId.TrnChartWizard: return "图表向导设置应用";
//                case ChartStringId.TrnSeriesDeleted: return "删除级联";
//                case ChartStringId.TrnChartTitleDeleted: return "删除图表标题";
//                case ChartStringId.TrnConstantLineDeleted: return "删除不变行";
//                case ChartStringId.AxisXDefaultTitle: return "坐标参数";
//                case ChartStringId.AxisYDefaultTitle: return "坐标值";
//                case ChartStringId.MenuItemAdd: return "新增";
//                case ChartStringId.MenuItemInsert: return "插入";
//                case ChartStringId.MenuItemDelete: return "删除";
//                case ChartStringId.MenuItemClear: return "清除";
//                case ChartStringId.MenuItemMoveUp: return "上移";
//                case ChartStringId.MenuItemMoveDown: return "下移";
//                case ChartStringId.WizBarSeriesLabelPositionTop: return "顶端";
//                case ChartStringId.WizBarSeriesLabelPositionCenter: return "居中";
//                case ChartStringId.WizPieSeriesLabelPositionInside: return "里面";
//                case ChartStringId.WizPieSeriesLabelPositionOutside: return "外面";
//                case ChartStringId.WizPieSeriesLabelPositionTwoColumns: return "两列";
//                case ChartStringId.SvnSideBySideBar: return "柱形图";
//                case ChartStringId.SvnStackedBar: return "横条图";
//                case ChartStringId.SvnFullStackedBar: return "100%堆叠柱形图";
//                case ChartStringId.SvnPie: return "饼图";
//                case ChartStringId.SvnPoint: return "泡泡图";
//                case ChartStringId.SvnLine: return "折线线";
//                case ChartStringId.SvnStepLine: return "分段折线图";
//                case ChartStringId.SvnStock: return "股票图(最高-最低-收盘)";
//                case ChartStringId.SvnCandleStick: return "股票图(开盘-最高-最低-收盘)";
//            }
//            return base.GetLocalizedString(id);
//        }
//    }
//}
