using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraBars.Localization;

namespace ZQTMS.Tool
{
    public class ToChnBar : BarLocalizer
    {
        public override string GetLocalizedString(BarString id)
        {
            switch (id)
            {
                case BarString.AddOrRemove: return "新增或删除按钮(&A)";
                case BarString.CustomizeButton: return "自定义(&C)...";
                case BarString.CustomizeWindowCaption: return "自定义";
                case BarString.MenuAnimationFade: return "减弱";
                case BarString.MenuAnimationNone: return "空";
                case BarString.MenuAnimationRandom: return "任意";
                case BarString.MenuAnimationSlide: return "滑动";
                case BarString.MenuAnimationSystem: return "(系统默认值)";
                case BarString.MenuAnimationUnfold: return "展开";
                case BarString.NewToolbarCaption: return "新建工具栏";
                case BarString.None: return "";
                case BarString.RenameToolbarCaption: return "重命名";
                case BarString.ResetBar: return "是否确实要重置对 '{0}' 工具栏所作的修改？";
                case BarString.ResetBarCaption: return "自定义";
                case BarString.ResetButton: return "重设工具栏(&R)";
                case BarString.ToolBarMenu: return "重设(&R)$删除(&D)$!重命名(&N)$!标准图标(&L)$只显示文字(&T)$在菜单中只用文字(&O)$显示图像与文本(&A)$!使用分隔线(&G)$常用菜单(&M)";
                case BarString.ToolbarNameCaption: return "工具栏名称(&T)：";
                case BarString.BarUnassignedItems: return "未分配项";
            }
            return base.GetLocalizedString(id);
        }

    }
}
