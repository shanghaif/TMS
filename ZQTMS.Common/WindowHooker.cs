using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ZQTMS.Common
{
    /// <summary>
    /// 页面统一图标
    /// </summary>
    public class WindowHooker
    {
        /// <summary>
        /// 页面统一图标
        /// 创建时间：2020.11.11
        /// </summary>
        public class HookControlEventArgs : EventArgs
        {
            public Control Control;

            public HookControlEventArgs(Control c)
            {
                this.Control = c;
            }
        }

        #region win32 api
        internal delegate IntPtr HOOKPROC(int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SetWindowsHookEx(int idHook, HOOKPROC lpfn, int hMod, int dwThreadId);
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
        #endregion

        internal IntPtr PHook = IntPtr.Zero;
        internal HOOKPROC PHookProc = null;

        public event EventHandler<HookControlEventArgs> OnHookControl;

        public void Hook()
        {
            PHookProc = new HOOKPROC(FnHookProc);
            // 如果代码改为：SetWindowsHookEx(5, FnHookProc, 0, AppDomain.GetCurrentThreadId());
            // 则会报如下错误：
            // ”类型的已垃圾回收委托进行了回调。这可能会导致应用程序崩溃、损坏和数据丢失。向非托管代码传递委托时，托管应用程序必须让这些委托保持活动状态，直到确信不会再次调用它们。”
            PHook = SetWindowsHookEx(5, PHookProc, 0, AppDomain.GetCurrentThreadId());
        }

        private IntPtr FnHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            switch (nCode)
            {
                case 5:
                    {
                        var control = Control.FromHandle(wParam);
                        if (control != null)
                        {
                            var frm = control as Form;
                            if (frm != null)
                            {
                                if (OnHookControl != null)
                                {
                                    OnHookControl(this, new HookControlEventArgs(frm));
                                }
                            }
                        }
                        break;
                    }
            }
            return CallNextHookEx(PHook, nCode, wParam, lParam);
        }
    }
}
