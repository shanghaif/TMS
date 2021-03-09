using System;
using System.Collections.Generic;
using System.Text;

namespace ZQTMS.Common
{
    /// <summary>
    /// 存储输入参数实体的属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ProcedureAttribute : Attribute
    {
        private string _procedureName = string.Empty;

        /// <summary>
        /// 存储过程名称
        /// </summary>
        public string ProcedureName
        {
            get { return _procedureName; }
            set { _procedureName = value; }
        }
    }
}
