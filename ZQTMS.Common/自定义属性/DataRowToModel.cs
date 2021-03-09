using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Data;

namespace ZQTMS.Common
{
    /// <summary>
    /// DataRow转换为实体类
    /// </summary>
    public static class DataRowToModel
    {
        public static T FillModel<T>(DataRow dr)
        {
            if (dr == null)
            {
                return default(T);
            }

            T model = (T)Activator.CreateInstance(typeof(T));

            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                PropertyInfo propertyInfo = model.GetType().GetProperty(dr.Table.Columns[i].ColumnName);

                if (propertyInfo != null && dr[i] != DBNull.Value)
                {
                    propertyInfo.SetValue(model, dr[i], null);
                }
                else
                {
                    continue;
                }
            }

            //foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            //{
            //    if (dr.Table.Columns.Contains(propertyInfo.Name) && dr[propertyInfo.Name] != DBNull.Value)
            //        propertyInfo.SetValue(model, dr[propertyInfo.Name], null);
            //    else continue;
            //}
            return model;
        }

        public static List<T> FillModelCollection<T>(DataTable dt)
        {
            List<T> list = new List<T>();
            foreach (DataRow item in dt.Rows)
            {
                list.Add(FillModel<T>(item));
            }
            return list;
        }
    }
}
