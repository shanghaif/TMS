using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.Utils;

namespace ZQTMS.Tool
{
    public static class MsgBox
    {
        /// <summary>
        /// 提示异常信息
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static DialogResult ShowException(Exception ex)
        {
            return ShowException(ex, "系统提示");
        }

        public static DialogResult ShowException(Exception ex, string caption)
        {
            string msg = "";
            if (ex.Message.Contains("SqlDbType 枚举值 30 无效") || ex.Message.Contains("get_SafeFileName"))
            {
                msg = "当前系统环境版本太低，请安装Microsoft .Net 2.0 sp1以上版本。";
            }
            else if (ex.Message.Contains("FIPS"))
            {
                msg = "系统设置条件不足，请在注册表中按以下路径打开，然后将enable的值设置为0即可：\r\n" +
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\FipsAlgorithmPolicy";
            }
            else if (ex.Message.Contains("Unable to generate a temporary class"))
            {
                msg = "Microsoft .Net环境损坏，请重新安装Microsoft .Net 2.0 sp1以上版本。";
            }
            else if (ex.Message.Contains("RPC服务器不可用"))
            {
                msg = "Windows系统RPC(打印)服务可能已停止。";
            }
            else if (ex.Message.Contains("无法为数据库") && ex.Message.Contains("分配空间，因为'PRIMARY'文件组已满"))
            {
                msg = "服务器硬盘空间不足，请联系系统管理员。";
            }

            if (msg != "")
            {
                msg += "\r\n";
            }
            msg += ex.Message;
            return XtraMessageBox.Show(msg, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 显示一个带有OK按钮的提示框
        /// </summary>
        /// <param name="Information">提示内容</param>
        /// <returns></returns>
        public static DialogResult ShowOK(string Information)
        {
            return ShowOK(Information, "系统提示");
        }

        /// <summary>
        /// 显示一个带有OK按钮的提示框
        /// </summary>
        /// <param name="Information">提示内容</param>
        /// <param name="caption">提示框标题</param>
        /// <returns></returns>
        public static DialogResult ShowOK(string Information, string Caption)
        {
            return XtraMessageBox.Show(Information, Caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 显示一个带有OK按钮的错误提示框
        /// </summary>
        /// <param name="Information">提示内容</param>
        /// <returns></returns>
        public static DialogResult ShowError(string Information)
        {
            return XtraMessageBox.Show(Information, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 显示一个带有YesNo按钮的提示框
        /// </summary>
        /// <param name="Information">提示内容</param>
        /// <returns></returns>
        public static DialogResult ShowYesNo(string Information)
        {
            return XtraMessageBox.Show(Information, "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        /// <summary>
        /// 显示一个带有YesNoCancel按钮的提示框
        /// </summary>
        /// <param name="Information">提示内容</param>
        /// <returns></returns>
        public static DialogResult ShowYesNoCancel(string Information)
        {
            return XtraMessageBox.Show(Information, "系统提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }

        /// <summary>
        /// 直接弹出操作已完成，并自动关闭
        /// </summary>
        public static void ShowOK()
        {
            w_save_ok wso = new w_save_ok();
            wso.ShowDialog();
        }

        /// <summary>
        /// 显示气球提示
        /// </summary>
        /// <param name="ToolTipContent">提示内容</param>
        /// <param name="Control">要提示的控件</param>
        /// <param name="Seconds">显示时间(秒)</param>
        /// <returns></returns>
        public static bool ShowTip(string ToolTipContent, Control Control, int Seconds)
        {
            ToolTipController toolTip1 = new ToolTipController();
            toolTip1.Rounded = true; //圆角 方角
            toolTip1.CloseOnClick = DefaultBoolean.True; //点击关闭
            toolTip1.AutoPopDelay = Seconds * 1000; //显示时间
            toolTip1.ShowShadow = true; //显示阴影
            toolTip1.ShowBeak = true;//显示出气球

            ToolTipControllerShowEventArgs args = toolTip1.CreateShowArgs();
            args.ToolTip = ToolTipContent;
            args.Title = "小提示";
            args.IconType = ToolTipIconType.Information;
            args.ToolTipLocation = ToolTipLocation.BottomCenter;
            toolTip1.ShowHint(args, Control);
            return true;
        }

        /// <summary>
        /// 显示气球提示(显示在右侧)
        /// <para>针对某些特殊界面的提示，只显示3秒</para>
        /// </summary>
        /// <param name="ToolTipContent">提示内容</param>
        /// <param name="Control">要提示的控件</param>
        /// <param name="Seconds">显示时间(秒)</param>
        /// <returns></returns>
        public static bool ShowTip(string ToolTipContent, Control Control)
        {
            ToolTipController toolTip1 = new ToolTipController();
            toolTip1.Rounded = true; //圆角 方角
            toolTip1.CloseOnClick = DefaultBoolean.True; //点击关闭
            toolTip1.AutoPopDelay = 3 * 1000; //显示时间
            toolTip1.ShowShadow = true; //显示阴影
            toolTip1.ShowBeak = true;//显示出气球

            ToolTipControllerShowEventArgs args = toolTip1.CreateShowArgs();
            args.ToolTip = ToolTipContent;
            args.Title = "小提示";
            args.IconType = ToolTipIconType.Information;
            args.ToolTipLocation = ToolTipLocation.RightCenter;
            toolTip1.ShowHint(args, Control);
            return true;
        }
    }
}