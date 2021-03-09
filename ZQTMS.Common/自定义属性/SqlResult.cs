using System;
using System.Collections.Generic;
using System.Text;

namespace ZQTMS.Common
{
    /// <summary>
    /// 数据库操作，返回结果
    /// </summary>
    [Serializable]
    public class SqlResult
    {
        /// <summary>
        /// SQL执行或查询返回状态
        /// <para>1表示成功；0表示失败</para>
        /// </summary>
        public int State
        {
            get;
            set;
        }

        /// <summary>
        /// 如果State=1，返回结果集(DataSet)；State=0返回错误原因
        /// </summary>
        public string Result
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 接口操作，返回结果
    /// </summary>
    [Serializable]
    public class ApiResult
    {
        /// <summary>
        /// SQL执行或查询返回状态
        /// <para>1表示成功；0表示失败</para>
        /// </summary>
        public int code
        {
            get;
            set;
        }

        /// <summary>
        /// SQL执行或查询返回状态
        /// <para>1表示成功；0表示失败</para>
        /// </summary>
        public int retCode
        {
            get;
            set;
        }

        public int orderId
        {
            get;
            set;
        }

        /// <summary>
        /// 如果State=1，返回结果集(DataSet)；State=0返回错误原因
        /// </summary>
        public string desc
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 接口操作，返回结果
    /// </summary>
    [Serializable]
    public class ApiDataResult
    {
        public string driverName { get; set; }
        public string carNumber { get; set; }
        public double carLongitude { get; set; }
        public double carLatitude { get; set; }
        public string carAddress { get; set; }
        public string driverphone { get; set; }
        public double phoneLongitude { get; set; }
        public double phoneLatitude { get; set; }
        public string address { get; set; }
    }
}
