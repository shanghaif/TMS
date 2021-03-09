using System;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.GZip;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace ZQTMS.Update
{
    public static class CZip
    {
        //解压调用：
        //CZip.DeCompress(Application.StartupPath + "\\" + filename);
        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="filename">下载的rar文件，包含完整路径</param>
        public static void DeCompress(string filename)
        {
            using (Stream s = new GZipInputStream(File.OpenRead(filename)))
            {
                string dir = Path.GetDirectoryName(filename);
                string file = Path.Combine(dir, Path.GetFileNameWithoutExtension(filename));
                using (FileStream fs = File.Create(file))
                {
                    int size = 2048;//指定压缩块的大小，一般为2048的倍数 
                    byte[] writeData = new byte[size];  //指定缓冲区的大小 
                    while (true)
                    {
                        size = s.Read(writeData, 0, size);//读入一个压缩块 
                        if (size > 0)
                        {
                            fs.Write(writeData, 0, size);//写入解压文件代表的文件流 
                        }
                        else
                        {
                            break;//若读到压缩文件尾，则结束 
                        }
                    }
                    fs.Close();
                    fs.Dispose();
                }
                s.Close();
                s.Dispose();
            }
        }

        /// <summary>
        /// 压缩，返回压缩文件的路径
        /// </summary>
        /// <param name="filename">原文件名，包含完整路径</param>
        public static string Compress(string filename)
        {
            string zipPath = filename + ".rar";
            if (File.Exists(zipPath)) File.Delete(zipPath);
            using (Stream s = new GZipOutputStream(File.Create(zipPath)))
            {
                using (FileStream fs = File.OpenRead(filename))
                {
                    byte[] writeData = new byte[fs.Length];
                    fs.Read(writeData, 0, (int)fs.Length);
                    s.Write(writeData, 0, writeData.Length);
                    fs.Close();
                    fs.Dispose();
                }
                s.Close();
                s.Dispose();
            }
            return zipPath;
        }
    }
}
