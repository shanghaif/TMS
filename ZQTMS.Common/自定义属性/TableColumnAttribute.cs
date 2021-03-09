using System;
using System.Collections.Generic;
using System.Text;

namespace ZQTMS.Common
{
    /// <summary>
    ///  数据表的列属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class TableColumnAttribute : Attribute
    {
        private bool _isprimary;
        private bool _isidentity;
        private bool _isUnique;
        private string _description;

        /// <summary>
        /// 是否是主键
        /// </summary>
        public bool IsPrimary
        {
            get { return _isprimary; }
            set { _isprimary = value; }
        }

        /// <summary>
        /// 是否自增长
        /// </summary>
        public bool IsIdentity
        {
            get { return _isidentity; }
            set { _isidentity = value; }
        }

        /// <summary>
        /// 是否唯一索引
        /// </summary>
        public bool IsUnique
        {
            get { return _isUnique; }
            set { _isUnique = value; }
        }

        /// <summary>
        /// 字段说明
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public TableColumnAttribute()
        {
        }
    }
}
