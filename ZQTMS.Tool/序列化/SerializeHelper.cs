using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization.Formatters.Binary;

namespace ZQTMS.Tool
{
    public static class SerializeHelper
    {
        /// <summary>
        /// XML序列化
        /// </summary>
        public static class Xml
        {
            /// <summary>
            /// 文件化XML序列化
            /// </summary>
            /// <param name="obj">对象</param>
            /// <param name="filename">文件路径</param>
            public static void Save(object obj, string filename)
            {
                FileStream fs = null;
                try
                {
                    fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                    XmlSerializer serializer = new XmlSerializer(obj.GetType());
                    serializer.Serialize(fs, obj);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (fs != null) fs.Close();
                }
            }

            /// <summary>
            /// 文件化XML反序列化
            /// </summary>
            /// <param name="type">对象类型</param>
            /// <param name="filename">文件路径</param>
            public static object Load(Type type, string filename)
            {
                FileStream fs = null;
                try
                {
                    fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    XmlSerializer serializer = new XmlSerializer(type);
                    return serializer.Deserialize(fs);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (fs != null) fs.Close();
                }
            }

            /// <summary>
            /// 文本化XML序列化
            /// </summary>
            /// <param name="item">对象</param>
            public static string ToXml<T>(T item)
            {
                XmlSerializer serializer = new XmlSerializer(item.GetType());
                StringBuilder sb = new StringBuilder();
                using (XmlWriter writer = XmlWriter.Create(sb))
                {
                    serializer.Serialize(writer, item);
                    return sb.ToString();
                }
            }

            /// <summary>
            /// 文本化XML反序列化
            /// </summary>
            /// <param name="str">字符串序列</param>
            public static T FromXml<T>(string str)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (XmlReader reader = new XmlTextReader(new StringReader(str)))
                {
                    return (T)serializer.Deserialize(reader);
                }
            }
        }

        
        /// <summary>
        /// SoapFormatter序列化
        /// </summary>
        public static class Soap
        {
            /// <summary>
            /// SoapFormatter序列化
            /// </summary>
            /// <param name="item">对象</param>
            public static string ToSoap<T>(T item)
            {
                SoapFormatter formatter = new SoapFormatter();
                using (MemoryStream ms = new MemoryStream())
                {
                    formatter.Serialize(ms, item);
                    ms.Position = 0;
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ms);
                    ms.Close();
                    ms.Dispose();
                    return xmlDoc.InnerXml;
                }
            }

            /// <summary>
            /// SoapFormatter反序列化
            /// </summary>
            /// <param name="str">字符串序列</param>
            public static T FromSoap<T>(string str)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(str);
                SoapFormatter formatter = new SoapFormatter();
                T obj;
                using (MemoryStream ms = new MemoryStream())
                {
                    xmlDoc.Save(ms);
                    ms.Position = 0;
                    obj = (T)formatter.Deserialize(ms);
                    ms.Close();
                    ms.Dispose();
                    return obj;
                }
            }
        }

        
        /// <summary>
        /// BinaryFormatter序列化
        /// </summary>
        public static class Binary
        {

            /// <summary>
            /// BinaryFormatter序列化
            /// </summary>
            /// <param name="item">对象</param>
            public static string ToBinary<T>(T item)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream())
                {
                    formatter.Serialize(ms, item);
                    ms.Position = 0;
                    byte[] bytes = ms.ToArray();
                    StringBuilder sb = new StringBuilder();
                    foreach (byte bt in bytes)
                    {
                        sb.Append(string.Format("{0:X2}", bt));
                    }
                    ms.Close();
                    ms.Dispose();
                    return sb.ToString();
                }
            }

            /// <summary>
            /// BinaryFormatter反序列化
            /// </summary>
            /// <param name="str">字符串序列</param>
            public static T FromBinary<T>(string str)
            {
                int intLen = str.Length / 2;
                byte[] bytes = new byte[intLen];
                for (int i = 0; i < intLen; i++)
                {
                    int ibyte = Convert.ToInt32(str.Substring(i * 2, 2), 16);
                    bytes[i] = (byte)ibyte;
                }
                BinaryFormatter formatter = new BinaryFormatter();
                T obj;
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    obj = (T)formatter.Deserialize(ms);
                    ms.Close();
                    ms.Dispose();
                    return obj;
                }
            }
        }
    }
}