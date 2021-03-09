using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace ZQTMS.Tool
{
    class SerialPort : System.IO.Ports.SerialPort
    {
        private string _splitchar = "";
        private decimal _weight = 0;
        private bool _start = false;

        /// <summary>
        /// 串口接收的数据分隔符
        /// </summary>
        public string SplitChar
        {
            get
            {
                return _splitchar.Trim();
            }
            set
            {
                _splitchar = value;
            }
        }

        /// <summary>
        /// 串口读到的数字 重量
        /// </summary>
        public decimal Weight
        {
            get
            {
                if (!this.IsOpen)
                {
                    throw new Exception("电子秤没有打开，可能未启用，或者没有设置参数!");
                    //return 0;
                }
                decimal weight = _weight;
                _weight = 0;
                return weight;
            }
            set
            {
                _weight = value;
            }
        }

        public bool Start
        {
            get { return _start; }
            set { _start = value; }
        }

        /// <summary>
        /// 获取电子称参数
        /// </summary>
        public void GetElectronicScaleParas(SerialPort sp, Form frm)
        {
            try
            {
                string path = Path.Combine(Application.StartupPath, "msconfig.dll");
                string enable = INIFile.IniReadValue("SerialPort", "enable", "0", path);

                int a = 0;
                if (!int.TryParse(enable, out a) || a != 1)//没有启用
                {
                    sp.Close();
                    sp.Dispose();
                    return;
                }

                string portName = INIFile.IniReadValue("SerialPort", "portName", "", path);//int
                string baudRate = INIFile.IniReadValue("SerialPort", "baudRate", "", path);//int
                string parity = INIFile.IniReadValue("SerialPort", "parity", "", path);  //int
                string dataBits = INIFile.IniReadValue("SerialPort", "dataBits", "", path);//int
                string stopBits = INIFile.IniReadValue("SerialPort", "stopBits", "", path);
                string split = INIFile.IniReadValue("SerialPort", "split", "=", path);

                sp.PortName = portName;

                if (int.TryParse(baudRate, out a))
                {
                    sp.BaudRate = a;
                }
                if (Enum.IsDefined(typeof(Parity), parity))
                {
                    sp.Parity = (Parity)Enum.Parse(typeof(Parity), parity, true);
                }
                if (int.TryParse(dataBits, out a))
                {
                    sp.DataBits = a;
                }
                if (int.TryParse(stopBits, out a))
                {
                    sp.StopBits = (StopBits)a;
                }
                sp.SplitChar = split;
                if (!sp.IsOpen)
                {
                    sp.Close();
                    sp.Open(); //线程不支持
                }
                sp.Start = true;
                frm.FormClosed += (obj, arg) =>
                {
                    sp.Start = false;
                    sp.DataReceived -= new SerialDataReceivedEventHandler(sp_DataReceived);
                    if (sp != null)
                    {
                        sp.Close();
                        sp.Dispose();
                    }
                };
                sp.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("COM"))
                {
                    throw new Exception("电子秤参数打开失败，请关闭软件，重新登录!\r\n如果无须使用电子秤，请在称重菜单中关闭电子秤功能!");
                }
            }
        }

        private void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (sender == null) return;
            SerialPort sp = sender as SerialPort;
            try
            {
                if (sp.IsOpen)
                {
                    if (!sp.Start) return;
                    Thread.Sleep(100);
                    sp.Start = false;
                    int buffersize = sp.ReadBufferSize;   //十六进制数的长度
                    byte[] readBuffer = new byte[buffersize];
                    int count = sp.Read(readBuffer, 0, buffersize);
                    string str = Encoding.UTF8.GetString(readBuffer).Trim().TrimEnd('\0');

                    if (str == "" || str == "=" || str == "0")
                    {
                        sp.Weight = 0;
                        return;
                    }

                    int flag1 = str.IndexOf(sp.SplitChar);
                    str = str.Substring(flag1 + 1);
                    flag1 = str.IndexOf(sp.SplitChar);
                    if (flag1 <= 0) return;
                    string result = str.Substring(0, flag1);
                    char[] chars = result.ToCharArray();
                    Array.Reverse(chars);

                    result = new string(chars);
                    if (result.Contains("-"))
                    {
                        sp.Weight = 0;
                        return;
                    }
                    decimal weight = 0;
                    if (decimal.TryParse(result, out weight))
                    {
                        sp.Weight = weight;
                    }
                    else
                    {
                        sp.Weight = 0;
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception("读取电子秤读数错误：" + err);
            }
            finally
            {
                sp.Start = true;
            }

        }
    }
}
