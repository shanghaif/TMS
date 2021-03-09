using System;
using System.Collections.Generic;
using System.Text;

namespace ZQTMS.Tool
{
    /// <summary>
    /// 汉化
    /// </summary>
    public static class ToChn
    {
        /// <summary>
        /// 控件汉化
        /// </summary>
        public static void ConvertToChn()
        {
            DevExpress.XtraEditors.Controls.Localizer.Active = new ToChnEditor();
            DevExpress.XtraGrid.Localization.GridLocalizer.Active = new ToChnGridControl();
            DevExpress.XtraBars.Localization.BarLocalizer.Active = new ToChnBar();
            DevExpress.XtraPrinting.Localization.PreviewLocalizer.Active = new ToChnXtraPrinting();
            //DevExpress.XtraReports.Localization.ReportLocalizer.Active = new ToChnXtraReports();
            //DevExpress.XtraCharts.Localization.ChartLocalizer.Active = new ToChnXtraChart();
            DevExpress.XtraTreeList.Localization.TreeListLocalizer.Active = new ToChnXtraTreeList();
            //DevExpress.XtraVerticalGrid.Localization.VGridLocalizer.Active = new ToChnXtraVerticalGrid();
        }
    }
}
