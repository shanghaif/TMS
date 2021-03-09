using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ZQTMS.Tool
{
    /// <summary>
    /// 精伦身份证识别
    /// </summary>
    public static class IdentityCard
    {
        /// <summary>
        /// 检查设备是否连接到USB 0：未连接 1：已连接
        /// </summary>
        /// <param name="iPort">USB号 一般为1001</param>
        /// <returns></returns>
        [DllImport("sdtapi.dll")]
        unsafe public static extern int InitComm(int iPort);

        /// <summary>
        /// 关闭端口 1：关闭成功
        /// </summary>
        /// <returns></returns>
        [DllImport("sdtapi.dll")]
        unsafe public static extern int CloseComm();

        /// <summary>
        /// 发现身份证卡并选择卡 1：正确 0：错误
        /// </summary>
        /// <returns></returns>
        [DllImport("sdtapi.dll")]
        unsafe public static extern int Authenticate();

        /// <summary>
        /// 本函数用于读取卡中基本信息，包括文字信息与图像信息
        /// <para>文字信息以字符串格式输出</para>
        /// <para>图象信息被解码后存为文件photo.bmp，身份证正面图片1.jpg，身份证反面图片2.jpg（在当前工作目录下）</para>
        /// </summary>
        /// <param name="Name">姓名 字节数不小31</param>
        /// <param name="Gender">性别 字节数不小3</param>
        /// <param name="Folk">民族 字节数不小10</param>
        /// <param name="BirthDay">出生日期 字节数不小9 格式为：CCYYMMDD</param>
        /// <param name="Code">身份证号码 字节数不小19</param>
        /// <param name="Address">地址 字节数不小71</param>
        /// <param name="Agency">签证机关 字节数不小31</param>
        /// <param name="ExpireStart">有效期起始日期 字节数不小9 格式为：CCYYMMDD</param>
        /// <param name="ExpireEnd">有效期截至日期 字节数不小9，格式为：CCYYMMDD</param>
        /// <returns></returns>
        [DllImport("sdtapi.dll")]
        unsafe public static extern int ReadBaseInfos(StringBuilder Name, StringBuilder Gender, StringBuilder Folk, StringBuilder BirthDay, StringBuilder Code, StringBuilder Address, StringBuilder Agency, StringBuilder ExpireStart, StringBuilder ExpireEnd);

    }
}
