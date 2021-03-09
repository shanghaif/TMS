using System;
using System.Collections.Generic;
using System.Text;

namespace ZQTMS.Common
{
    /// <summary>
    /// 提交OA信息的返回结果实体
    /// </summary>
    [Serializable]
    public class PostOAResult
    {
        public long msgid
        {
            get;
            set;
        }
        public string msg
        {
            get;
            set;
        }
    }
}
