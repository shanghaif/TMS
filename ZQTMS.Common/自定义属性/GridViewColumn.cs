
/*------------------------------------------------
// File Name:GridViewColInfo.cs
// File Description:GridViewCol Business Logic
// Author:蓝桥软件
// Create Time:2016-07-06 03:36:06
//------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Text;

namespace ZQTMS.Common
{
    /// <summary>
    /// 描述：存储过程的字段属性，用于产生GridView的网格
    /// </summary>
    [Serializable]
    public class GridViewColumn
    {
        //ColName,ColCaption,AllowEdit,AllowSummary
        #region 私有属性
        private string _colName;
        private string _colCaption;
        private int _allowEdit = 0;
        private int _visible = 1;
        private int _allowSummary;
        private string _colType;
        #endregion

        #region 构造方法
        /// <summary>
        /// GridViewCol 构造方法
        /// </summary>
        public GridViewColumn()
        {
        }
        #endregion

        #region 公有字段

        ///<Summary>
        /// ColName
        /// 网格字段名
        ///</Summary>
        public string ColName
        {
            get { return _colName; }
            set { _colName = value; }
        }

        ///<Summary>
        /// ColCaption
        /// 网格标题
        ///</Summary>
        public string ColCaption
        {
            get { return _colCaption; }
            set { _colCaption = value; }
        }

        ///<Summary>
        /// AllowEdit
        /// 可编辑
        ///</Summary>
        public int AllowEdit
        {
            get { return _allowEdit; }
            set { _allowEdit = value; }
        }

        ///<Summary>
        /// Visible
        /// 可见性
        ///</Summary>
        public int Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        ///<Summary>
        /// AllowSummary
        /// 合计类型，可转换为SummaryItemType
        ///</Summary>
        public int AllowSummary
        {
            get { return _allowSummary; }
            set { _allowSummary = value; }
        }

        /// <summary>
        /// ColType
        /// 字段类型
        /// </summary>
        public string ColType
        {
            get { return _colType; }
            set { _colType = value; }
        }

        #endregion
    }
}
