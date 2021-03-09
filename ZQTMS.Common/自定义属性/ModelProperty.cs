using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Data;
using ZQTMS.SqlDAL;

namespace ZQTMS.Common
{
    /// <summary>
    /// 实体属性反射
    /// </summary>
    public static class ModelProperty
    {
        /// <summary>
        /// 获取实体的属性
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sps">参数对象</param>
        /// <param name="model">实体对象</param>
        private static void GetProperty<T>(SqlParasEntity sps, T model)
        {
            Type type = typeof(T);
            object[] attributes = type.GetCustomAttributes(typeof(ProcedureAttribute), false);//
            if (attributes.Length > 0)
            {
                ProcedureAttribute attr = (ProcedureAttribute)attributes[0];
                sps.ProcedureName = attr.ProcedureName;
            }
            else
            {
                throw new Exception("没有为输入参数实体指定存储过程!");
            }

            List<SqlPara> list = new List<SqlPara>();
            PropertyInfo[] propertycoll = type.GetProperties();//BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.IgnoreCase
            foreach (PropertyInfo pi in propertycoll)
            {
                list.Add(new SqlPara(pi.Name, pi.GetValue(model, null)));
            }
            sps.ParaList = list;
        }

        /// <summary>
        /// 用于增、删、改的操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int Execute<T>(T model)
        {
            SqlParasEntity sps = new SqlParasEntity(OperType.Execute);
            GetProperty<T>(sps, model);
            return SqlHelper.ExecteNonQuery(sps);
        }

        /// <summary>
        /// 返回单个实体对象
        /// </summary>
        /// <typeparam name="T">返回实体的类型</typeparam>
        /// <typeparam name="T1">参数提示的类型</typeparam>
        /// <returns></returns>
        public static T GetModel<T, T1>(T1 model)
        {
            SqlParasEntity sps = new SqlParasEntity(OperType.Query);
            GetProperty<T1>(sps, model);

            DataSet ds = SqlHelper.GetDataSet(sps);
            T t = default(T);
            if (ds.Tables[0].Rows.Count > 0)
            {
                t = DataRowToModel.FillModel<T>(ds.Tables[0].Rows[0]);
            }
            return t;
        }

        /// <summary>
        /// 返回存储过程数据集DataSet
        /// </summary>
        /// <typeparam name="T">查询参数实体</typeparam>
        /// <returns></returns>
        public static DataSet GetDataSet<T>(T model)
        {
            SqlParasEntity sps = new SqlParasEntity(OperType.Query);
            GetProperty<T>(sps, model);

            DataSet ds = SqlHelper.GetDataSet(sps);
            return ds;
        }
    }
}
