using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.Utils;
using System.Windows.Forms;

namespace ZQTMS.Tool
{
    public static class ToolTip
    {
        /// <summary>
        /// 显示气球提示
        /// </summary>
        /// <param name="toolTipContent">提示内容</param>
        /// <param name="control">要提示的控件</param>
        /// <param name="seconds">显示时间(秒)</param>
        /// <returns></returns>
        public static bool ShowTip(string toolTipContent, Control control, int seconds)
        {
            ToolTipController toolTip1 = new ToolTipController();
            toolTip1.Rounded = true; //圆角 方角
            toolTip1.CloseOnClick = DefaultBoolean.True; //点击关闭
            toolTip1.AutoPopDelay = seconds * 1000; //显示时间
            toolTip1.ShowShadow = true; //显示阴影
            toolTip1.ShowBeak = true;//显示出气球

            ToolTipControllerShowEventArgs args = toolTip1.CreateShowArgs();
            args.ToolTip = toolTipContent;
            args.Title = "小提示";
            args.IconType = ToolTipIconType.Information;
            args.ToolTipLocation = ToolTipLocation.BottomCenter;
            toolTip1.ShowHint(args, control);
            return true;
        }

        /// <summary>
        /// 显示气球提示(显示在右侧)
        /// <para>针对某些特殊界面的提示，只显示3秒</para>
        /// </summary>
        /// <param name="toolTipContent">提示内容</param>
        /// <param name="control">要提示的控件</param>
        /// <param name="Seconds">显示时间(秒)</param>
        /// <returns></returns>
        public static bool ShowTip(string toolTipContent, Control control)
        {
            ToolTipController toolTip1 = new ToolTipController();
            toolTip1.Rounded = true; //圆角 方角
            toolTip1.CloseOnClick = DefaultBoolean.True; //点击关闭
            toolTip1.AutoPopDelay = 3 * 1000; //显示时间
            toolTip1.ShowShadow = true; //显示阴影
            toolTip1.ShowBeak = true;//显示出气球

            ToolTipControllerShowEventArgs args = toolTip1.CreateShowArgs();
            args.ToolTip = toolTipContent;
            args.Title = "小提示";
            args.IconType = ToolTipIconType.Information;
            args.ToolTipLocation = ToolTipLocation.RightCenter;
            toolTip1.ShowHint(args, control);
            return true;
        }

        /// <summary>
        /// 显示气球提示(显示在任意位置)
        /// <para>针对某些特殊界面的提示，只显示3秒</para>
        /// </summary>
        /// <param name="toolTipContent">提示内容</param>
        /// <param name="control">要提示的控件</param>
        /// <param name="Seconds">显示时间(秒)</param>
        /// <param name="location">显示位置</param>
        /// <returns></returns>
        public static bool ShowTip(string toolTipContent, Control control, ToolTipLocation location)
        {
            ToolTipController toolTip1 = new ToolTipController();
            toolTip1.Rounded = true; //圆角 方角
            toolTip1.CloseOnClick = DefaultBoolean.True; //点击关闭
            toolTip1.AutoPopDelay = 3 * 1000; //显示时间
            toolTip1.ShowShadow = true; //显示阴影
            toolTip1.ShowBeak = true;//显示出气球
            toolTip1.AllowHtmlText = true;
            ToolTipControllerShowEventArgs args = toolTip1.CreateShowArgs();
            args.ToolTip = toolTipContent;
            args.Title = "提示";
            args.IconType = ToolTipIconType.Information;
            args.ToolTipLocation = location;
            
            toolTip1.ShowHint(args, control);
            return true;
        }
    }
}
