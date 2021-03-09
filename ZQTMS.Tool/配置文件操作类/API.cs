using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ZQTMS.Tool
{
    public class API
    {
        [DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
        public static extern int GetSystemMetrics(int abc);

        [DllImport("user32.dll")]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hwnd, int id, KeyModifiers fsModifiers, Keys key);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hwnd, int id);
        [Flags()]
        private enum KeyModifiers
        {
            None = 0,
            Alt = 1,
            Ctrl = 2,
            Shift = 4,
            Windows = 8
        }

        /// <summary>
        /// 返回截图
        /// </summary>
        /// <param name="hdcDest">目标DC的句柄</param>
        /// <param name="nXDest">目标对象的左上角的X坐标</param>
        /// <param name="nYDest">目标对象的左上角的Y坐标</param>
        /// <param name="nWidth">目标对象的矩形的宽度</param>
        /// <param name="nHeight">目标对象的矩形的高度</param>
        /// <param name="hdcSrc">源DC的句柄 </param>
        /// <param name="nXSrc">源对象的左上角的X坐标</param>
        /// <param name="nYSrc">源对象的左上角的X坐标 </param>
        /// <param name="dwRop">光栅的处理数值</param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, Int32 dwRop);

        [DllImport("kernel32.dll", EntryPoint = "GetSystemDefaultLCID")]
        public static extern int GetSystemDefaultLCID();

        [DllImport("Kernel32.dll")]
        public static extern void SetLocalTime(SystemTime st);

        [StructLayout(LayoutKind.Sequential)]
        public class SystemTime
        {
            public ushort Year;
            public ushort Month;
            public ushort DayOfWeek;
            public ushort Day;
            public ushort Hour;
            public ushort Minute;
            public ushort Second;
            public ushort Miliseconds;
        }

        //设置系统日期格式
        [DllImport("kernel32.dll", EntryPoint = "SetLocaleInfoA")]
        public static extern int SetLocaleInfo(int Locale, int LCType, string lpLCData);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, byte[] retVal, int size, string filePath);

        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);

        //设置默认打印机
        [DllImport("Winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDefaultPrinter(string printerName);

        //获取默认打印机
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetDefaultPrinter(StringBuilder pszBuffer, ref int pcchBuffer);

        //检测物理设备是否连接：即网线连接到是否连接到路由器。在计算机系统中的网络连接里面，本地连接是否已经连接上了
        [DllImport("wininet.dll", EntryPoint = "InternetGetConnectedState")]
        public extern static bool InternetGetConnectedState(out int conState, int reder);//参数说明 constate 连接说明 ，reder保留值

        public static string ReadINI(string Section, string Key, string DefaultValue, string filename)
        {
            Byte[] Buffer = new Byte[65535];
            int bufferLen = GetPrivateProfileString(Section, Key, DefaultValue, Buffer, Buffer.GetUpperBound(0), Application.StartupPath + "\\" + filename + ".dll");
            string temp = Encoding.GetEncoding(0).GetString(Buffer);
            temp = temp.Substring(0, bufferLen);
            return temp.Trim().TrimEnd('\0').Trim().TrimEnd('\0');
        }

        public static int ReadINI(string Section, string Key, int DefaultValue, string filename)
        {
            try
            {
                Byte[] Buffer = new Byte[65535];
                int bufferLen = GetPrivateProfileString(Section, Key, DefaultValue.ToString(), Buffer, Buffer.GetUpperBound(0), Application.StartupPath + "\\" + filename + ".dll");
                string temp = Encoding.GetEncoding(0).GetString(Buffer);
                temp = temp.Substring(0, bufferLen).Trim().TrimEnd('\0').Trim().TrimEnd('\0');
                int result = DefaultValue;
                int.TryParse(temp, out result);
                return result;
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
                return DefaultValue;
            }
        }

        public static void WriteINI(string Section, string Key, string Value, string filename)
        {
            WritePrivateProfileString(Section, Key, Value, Application.StartupPath + "\\" + filename + ".dll");
        }
    }
}