using System;
using System.Collections.Generic;
using System.Text;

namespace ZQTMS.UpLoad
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
}
