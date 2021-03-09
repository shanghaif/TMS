using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DevExpress.XtraGrid.Views.Grid;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace ZQTMS.Tool
{
    public static class DataOper
    {
        /// <summary>
        /// List泛型对象转换为DataTable
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="data">要转换的数据列表</param>
        /// <returns></returns>
        public static DataTable ListToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dt = new DataTable(typeof(T).Name);

            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor property = properties[i];
                dt.Columns.Add(property.Name, property.PropertyType);
            }
            object[] values = new object[properties.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = properties[i].GetValue(item);
                }
                dt.Rows.Add(values);
            }
            return dt;
        }

        public static bool Compress(string data, ref string result)
        {
            try
            {
                byte[] buffer;
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(ms, data);
                    buffer = ms.ToArray();
                    ms.Close();
                    ms.Dispose();
                }

                using (MemoryStream msout = new MemoryStream())
                {
                    Deflater defl = new Deflater();
                    using (DeflaterOutputStream mem1 = new DeflaterOutputStream(msout, defl))
                    {
                        mem1.Write(buffer, 0, buffer.Length);
                        mem1.Close();
                        mem1.Dispose();
                    }
                    result = Convert.ToBase64String(msout.ToArray());
                    //result = result.Replace("+", "-").Replace("/", "_").Replace("=", ".");
                    msout.Close();
                    msout.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return false;
            }
        }

        //提取ZQTMS数据 hj20180423
        public static bool Compress_ZQTMS(string data, ref string result)
        {
            try
            {
                byte[] buffer;
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(ms, data);
                    buffer = ms.ToArray();
                    ms.Close();
                    ms.Dispose();
                }

                using (MemoryStream msout = new MemoryStream())
                {
                    Deflater defl = new Deflater(9);
                    using (DeflaterOutputStream mem1 = new DeflaterOutputStream(msout, defl))
                    {
                        mem1.Write(buffer, 0, buffer.Length);
                        mem1.Close();
                        mem1.Dispose();
                    }
                    result = Convert.ToBase64String(msout.ToArray());
                    result = result.Replace("+", "-").Replace("/", "_").Replace("=", ".");
                    msout.Close();
                    msout.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return false;
            }
        }


        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="inputdata"></param>
        /// <param name="ds"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool Decompress(string inputdata, ref DataSet ds, ref string error)
        {
            try
            {
                //inputdata = inputdata.Replace("-", "+").Replace("_", "/").Replace(".", "=");
                byte[] bb = Convert.FromBase64String(inputdata);
                MemoryStream data = new MemoryStream(bb);

                int s1 = (int)data.Length;

                using (MemoryStream m = new MemoryStream())
                {
                    using (InflaterInputStream mem = new InflaterInputStream(data))
                    {
                        byte[] buffer = new byte[4096];
                        while (true)
                        {
                            int size = mem.Read(buffer, 0, buffer.Length);
                            m.Write(buffer, 0, size);
                            if (size == 0)
                                break;
                        }
                        mem.Close();
                        mem.Dispose();
                    }

                    m.Seek(0, SeekOrigin.Begin);
                    BinaryFormatter bf = new BinaryFormatter();
                    ds = bf.Deserialize(m) as DataSet;
                    m.Close();
                    m.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public static bool Decompress(string inputdata, ref string outdata, ref string error)
        {
            try
            {
                inputdata = inputdata.Replace("-", "+").Replace("_", "/").Replace(".", "=");
                byte[] bb = Convert.FromBase64String(inputdata);
                MemoryStream data = new MemoryStream(bb);

                int s1 = (int)data.Length;

                using (MemoryStream m = new MemoryStream())
                {
                    using (InflaterInputStream mem = new InflaterInputStream(data))
                    {
                        byte[] buffer = new byte[4096];
                        while (true)
                        {
                            int size = mem.Read(buffer, 0, buffer.Length);
                            m.Write(buffer, 0, size);
                            if (size == 0)
                                break;
                        }
                        mem.Close();
                        mem.Dispose();
                    }

                    m.Seek(0, SeekOrigin.Begin);
                    BinaryFormatter bf = new BinaryFormatter();
                    outdata = bf.Deserialize(m) as string;
                    m.Close();
                    m.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }


        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string Compress(string param)
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(param);

            MemoryStream ms = new MemoryStream();
            Stream stream = new ICSharpCode.SharpZipLib.GZip.GZipOutputStream(ms);
            try
            {
                stream.Write(data, 0, data.Length);
            }
            finally
            {
                stream.Close();
                ms.Close();
            }
            return Convert.ToBase64String(ms.ToArray()).Trim().Replace("+", "@");
        }

        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string Decompress(string param)
        {
            string commonString = "";

            byte[] buffer = Convert.FromBase64String(param);
            MemoryStream ms = new MemoryStream(buffer);
            Stream sm = new ICSharpCode.SharpZipLib.GZip.GZipInputStream(ms);
            //这里要指明要读入的格式，要不就有乱码
            StreamReader reader = new StreamReader(sm, System.Text.Encoding.UTF8);
            try
            {
                commonString = reader.ReadToEnd();
            }
            finally
            {
                sm.Close();
                ms.Close();
            }
            return commonString;
        }
    }
}