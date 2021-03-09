using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

namespace ZQTMS.Tool
{
    /// <summary>
    /// 配置文件的数据，仅适用于带Section节点的配置文件，例如：
    /// <para>[Config]</para>
    /// <para>server=101.200.202.50,1091</para>
    /// <para>database=开发练习库131022</para>
    /// </summary>
    public class Config
    {
        Encoding coding = Encoding.GetEncoding("gb2312");

        List<ConfigInfo> list = new List<ConfigInfo>();

        string fullFileName;

        /// <summary>
        /// 初始化配置文件
        /// </summary>
        /// <param name="_fileName">配置文件的完整路径</param>
        public Config(string _fileName)
        {
            fullFileName = _fileName;

            if (!File.Exists(_fileName))
            {
                return;
            }

            using (StreamReader reader = new StreamReader(_fileName, coding))
            {
                string line;
                string Section = "";
                ConfigInfo info = new ConfigInfo();
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith(";") || string.IsNullOrEmpty(line))
                    {
                        info = new ConfigInfo();
                        info.Type = 0;
                        info.Section = line;
                        info.Key = "";
                        info.Value = "";
                        list.Add(info);
                    }
                    else if (line.StartsWith("[") && line.EndsWith("]"))//说明这个时候是节点Section
                    {
                        Section = line;
                        info = new ConfigInfo();
                        info.Type = 1;
                        info.Section = line;
                        info.Key = "";
                        info.Value = "";
                        list.Add(info);
                    }
                    else
                    {
                        info = new ConfigInfo();
                        string[] key_value = line.Split('=');
                        if (key_value.Length >= 2)
                        {
                            info.Type = 2;
                            info.Section = Section;
                            info.Key = key_value[0];
                            info.Value = key_value[1];
                            list.Add(info);
                        }
                        else
                        {
                            info.Type = 0;
                            info.Section = line;
                            info.Key = "";
                            info.Value = "";
                            list.Add(info);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
        }

        /// <summary>
        /// 获取Config配置文件的键值
        /// </summary>
        /// <param name="Section">配置节点</param>
        /// <param name="Key">键</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public string GetValue(string Section, string Key, string DefaultValue)
        {
            ConfigInfo config = list.Find(delegate(ConfigInfo info) { return StringEquals(info.Section, string.Format("[{0}]", Section)) && StringEquals(info.Key, Key); });
            if (config == null) return DefaultValue;
            return config.Value;
        }

        /// <summary>
        /// 获取Config配置文件的键值
        /// </summary>
        /// <param name="Section">配置节点</param>
        /// <param name="Key">键</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public int GetValue(string Section, string Key, int DefaultValue)
        {
            ConfigInfo config = list.Find(delegate(ConfigInfo info) { return StringEquals(info.Section, string.Format("[{0}]", Section)) && StringEquals(info.Key, Key); });
            if (config == null) return DefaultValue;
            int a = 0;
            if (int.TryParse(config.Value, out a))
            {
                return a;
            }
            return DefaultValue;
        }

        /// <summary>
        /// 设置Config配置文件的键值
        /// <para>设置完毕后，需要调用Save函数保存键值</para>
        /// </summary>
        /// <param name="Section">配置节点</param>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        public void SetKeyValue(string Section, string Key, string Value)
        {
            ConfigInfo config = list.Find(delegate(ConfigInfo info) { return StringEquals(info.Section, string.Format("[{0}]", Section)) && StringEquals(info.Key, Key); });
            if (config == null)
            {
                config = new ConfigInfo();
                config.Type = 2;
                config.Section = Section;
                config.Key = Key;
                config.Value = Value;
                int index = list.FindLastIndex(delegate(ConfigInfo info) { return StringEquals(info.Section, string.Format("[{0}]", Section)); });
                if (index >= 0) //有Section节点
                {
                    list.Insert(index + 1, config);
                }
                else
                {
                    ConfigInfo cf = new ConfigInfo();

                    cf.Type = 0;
                    cf.Section = "";
                    list.Add(cf);

                    cf = new ConfigInfo();
                    cf.Type = 1;
                    cf.Section = string.Format("[{0}]", Section);
                    list.Add(cf);

                    list.Add(config);
                }
            }
            else
            {
                config.Value = Value;
            }
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        public bool Save()
        {
            try
            {
                if (!File.Exists(fullFileName))
                {
                    using (StreamWriter sw = new StreamWriter(File.Create(fullFileName), coding))
                    {
                        sw.Close();
                        sw.Dispose();
                    }
                }

                using (StreamWriter sw = new StreamWriter(fullFileName, false, coding))
                {
                    List<ConfigInfo>.Enumerator en = list.GetEnumerator();
                    ConfigInfo info;
                    while (en.MoveNext())
                    {
                        info = en.Current;
                        if (info.Type == 0 || info.Type == 1)
                        {
                            sw.WriteLine(info.Section);
                        }
                        else //2
                        {
                            sw.WriteLine(info.Key + "=" + info.Value);
                        }
                    }
                    sw.Close();
                    sw.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("配置保存失败：\r\n" + ex.Message);
            }
            return true;
        }

        /// <summary>
        /// 字符串是否相同，不区分大小写
        /// </summary>
        /// <param name="StrA"></param>
        /// <param name="StrB"></param>
        /// <returns></returns>
        private bool StringEquals(string StrA, string StrB)
        {
            return string.Equals(StrA, StrB, StringComparison.OrdinalIgnoreCase);
        }
    }

    class ConfigInfo
    {
        /// <summary>
        /// 当前行的类型，0表示注释或者空白  1表示Section节点  2表示Key-Value
        /// </summary>
        public int Type = 0;

        /// <summary>
        /// 节点信息
        /// </summary>
        public string Section = "";//当Type=0的时候，Section保存的是注释或者空白

        /// <summary>
        /// 键
        /// </summary>
        public string Key = "";

        /// <summary>
        /// 值
        /// </summary>
        public string Value = "";
    }
    
}
