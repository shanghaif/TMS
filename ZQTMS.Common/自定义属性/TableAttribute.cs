using System;
using System.Collections.Generic;
using System.Text;

namespace ZQTMS.Common
{
    /// <summary>
    /// 指定Model所对应的Table名称
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TableAttribute : Attribute
    {
        private string _tableName = string.Empty;
        private string _description = string.Empty;
        private string _primary;
        private string _identity;
        private string _unique;

        /// <summary>
        /// 表的名称
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        /// <summary>
        /// 表的描述
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// 表的主键
        /// </summary>
        public string Primary
        {
            get { return _primary; }
            set { _primary = value; }
        }

        /// <summary>
        /// 表的自增字段
        /// </summary>
        public string Identity
        {
            get { return _identity; }
            set { _identity = value; }
        }

        /// <summary>
        /// 表的唯一索引
        /// </summary>
        public string Unique
        {
            get { return _unique; }
            set { _unique = value; }
        }

        public TableAttribute()
        {
        }
    }
}
