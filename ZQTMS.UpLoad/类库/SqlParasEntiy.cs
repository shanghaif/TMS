using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace ZQTMS.UpLoad
{
    /// <summary>
    /// 数据库操作，提交参数
    /// </summary>
    [Serializable]
    public class SqlParasTable
    {
        /// <summary>
        /// 操作类型：1表示查询操作  2表示执行操作  4表示获取架构
        /// </summary>
        public OperType OperType
        {
            get;
            set;
        }

        /// <summary>
        /// 参数的DataTable集合
        /// <para>第一个table存放简洁类型的参数对</para>
        /// <para>之后table存放Table类型的参数</para>
        /// </summary>
        public DataSet ParasDataSet
        {
            get;
            set;
        }

        public SqlParasTable(OperType OperType, DataSet ParasDataSet)
        {
            this.OperType = OperType;
            this.ParasDataSet = ParasDataSet;
        }
    }

    /// <summary>
    /// 数据库操作，提交参数
    /// </summary>
    [Serializable]
    public class SqlParasEntity
    {
        /// <summary>
        /// 操作类型：1表示查询操作  2表示执行操作  4表示获取架构
        /// </summary>
        public OperType OperType
        {
            get;
            set;
        }
        
        /// <summary>
        /// 存储过程名称
        /// </summary>
        public string ProcedureName
        {
            get;
            set;
        }

        /// <summary>
        /// 参数集合
        /// </summary>
        public List<SqlPara> ParaList
        {
            get;
            set;
        }

        public SqlParasEntity()
        { }

        /// <summary>
        /// 实例化对象SqlParasEntity
        /// </summary>
        /// <param name="OperType">操作类型：1表示查询操作  2表示执行操作  4表示获取架构</param>
        public SqlParasEntity(OperType OperType)
        {
            this.OperType = OperType;
        }

        /// <summary>
        /// 实例化对象SqlParasEntity
        /// </summary>
        /// <param name="OperType">操作类型：1表示查询操作  2表示执行操作  4表示获取架构</param>
        /// <param name="ProcedureName">存储过程名称</param>
        public SqlParasEntity(OperType OperType, string ProcedureName)
        {
            this.OperType = OperType;
            this.ProcedureName = ProcedureName;
        }

        public SqlParasEntity(OperType OperType, string ProcedureName, List<SqlPara> list)
        {
            this.OperType = OperType;
            this.ProcedureName = ProcedureName;
            this.ParaList = list;
        }
    }

    /// <summary>
    /// 参数对象
    /// </summary>
    [Serializable]
    public class SqlPara
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParaName
        {
            get;
            set;
        }

        /// <summary>
        /// 参数值
        /// </summary>
        public object ParaValue
        {
            get;
            set;
        }

        public SqlPara(string ParaName, object ParaValue)
        {
            this.ParaName = ParaName;
            this.ParaValue = ParaValue;
        }
    }

    public enum OperType
    {
        /// <summary>
        /// 1：表示做查询操作
        /// </summary>
        Query = 1,

        /// <summary>
        /// 2：表示做执行操作
        /// </summary>
        Execute = 2,

        /// <summary>
        /// 4：表示获取架构
        /// </summary>
        FillSchema = 4
    }
}
