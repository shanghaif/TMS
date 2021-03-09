using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ZQTMS.UpLoad
{
    /// <summary>
    /// 此类为抽象类，不允许实例化，在应用时直接调用即可
    /// </summary>
    public abstract class SqlHelper
    {
        private static int ver = 1;//函数版本

        //public static int ExecteNonQuery(SqlParasEntity ent)
        //{
        //    SqlResult result = new SqlResult();
        //    string json = JsonConvert.SerializeObject(ent, new JsonSerializerSettings { DateFormatString = "yyyy-MM-dd HH:mm:ss.fff" });
        //    string s1 = "";

        //    s1 = DataOper.Compress(json);
        //    if (string.IsNullOrEmpty(s1))
        //    {
        //        result.State = 0;
        //        result.Result = "发送数据压缩失败!";
        //    }
        //    else result = HttpHelper.HttpPost(string.Format("ver={0}&json={1}", ver, s1));

        //    if (result.State == 1)
        //    {
        //        return result.Result == "数据更新成功" ? 1 : 0;
        //    }
        //    else
        //    {
        //        throw new Exception("返回信息:\r\n" + result.Result.Replace("数据库访问异常：", "").Replace("错误信息：", "").Trim());
        //    }
        //}


        #region zaj 2017-11-7
        public static int ExecteNonQuery(SqlParasEntity ent)
        {
            DataSet dsSet = new DataSet();
            DataTable dt = new DataTable(ent.ProcedureName);
            dt.Columns.Add("pname", typeof(string));
            dt.Columns.Add("pvalue", typeof(string));
            dsSet.Tables.Add(dt);
            if (ent.ParaList == null)
            {
                List<SqlPara> list = new List<SqlPara>();
                ent.ParaList = list;
            }

          
            foreach (SqlPara item in ent.ParaList)
            {
                if (item.ParaValue.GetType() == typeof(DataTable))
                {
                    DataTable table = (item.ParaValue as DataTable).Copy();//直接作为子表传递参数
                    table.TableName = item.ParaName;
                    dsSet.Tables.Add(table);
                }
                else
                {
                    dt.Rows.Add(item.ParaName, item.ParaValue);
                }
            }
            string operType = ent.OperType.ToString();
            string dsJson = JsonConvert.SerializeObject(dsSet);
            ResponseModelClone<string> result = new ResponseModelClone<string>();
            RequestModelClone model = new RequestModelClone(operType, dsJson);
            string json = JsonConvert.SerializeObject(model);
            result = HttpHelper.HttpPost(json);
            if (result.State == "1")
            {
                int count = 0;
                if (ZQTMS.Tool.StringHelper.IsNumberId(result.Result))
                {
                    count = Convert.ToInt32(result.Result);
                }
                return count;
            }
            else
            {
                throw new Exception("返回信息:\r\n" + result.Result.Replace("数据库访问异常：", "").Replace("错误信息：", "").Trim());
            }
        }
        #endregion

        //public static DataSet GetDataSet(SqlParasEntity ent)
        //{
        //    if (ent.ParaList == null)
        //    {
        //        List<SqlPara> list = new List<SqlPara>();
        //        ent.ParaList = list;
        //    }

        //    SqlResult result = new SqlResult();
        //    JsonSerializerSettings setting = new JsonSerializerSettings();
        //    setting.DateFormatString = "yyyy-MM-dd HH:mm:ss.fff";
        //    string json = JsonConvert.SerializeObject(ent, setting);
        //    string s1 = DataOper.Compress(json);

        //    if (string.IsNullOrEmpty(s1))
        //    {
        //        result.State = 0;
        //        result.Result = "发送的数据压缩失败!";
        //    }
        //    else result = HttpHelper.HttpPost(string.Format("ver={0}&json={1}", ver, s1));

        //    if (result.State == 1)
        //    {
        //        DataSet ds;
        //        try
        //        {
        //            ds = JsonConvert.DeserializeObject<DataSet>(DataOper.Decompress(result.Result), setting);
        //            return ds;
        //        }
        //        catch
        //        {
        //            try
        //            {
        //                DataTable dt = JsonConvert.DeserializeObject<DataTable>(DataOper.Decompress(result.Result), setting);
        //                ds = new DataSet();
        //                ds.Tables.Add(dt);
        //                return ds;
        //            }
        //            catch (Exception ex)
        //            {
        //                throw new Exception(ex.Message + "—" + ent.ProcedureName);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception(result.Result);
        //    }
        //}


        #region zaj 2017-11-17
        public static DataSet GetDataSet(SqlParasEntity ent, int ver)
        {
            DataSet dsSet = new DataSet();
            DataTable dt = new DataTable(ent.ProcedureName);
            dt.Columns.Add("pname", typeof(string));
            dt.Columns.Add("pvalue", typeof(string));
            dsSet.Tables.Add(dt);
            if (ent.ParaList == null)
            {
                List<SqlPara> list = new List<SqlPara>();
                ent.ParaList = list;
            }
            #region 登录信息
           
            #endregion
            foreach (SqlPara item in ent.ParaList)
            {
                if (item.ParaValue == null)
                {
                    item.ParaValue = DBNull.Value;
                }
                if (item.ParaValue.GetType() == typeof(DataTable))
                {
                    DataTable table = (item.ParaValue as DataTable).Copy(); //直接作为子表传递参数
                    table.TableName = item.ParaName;
                    dsSet.Tables.Add(table);
                }
                else
                {
                    dt.Rows.Add(item.ParaName, item.ParaValue);
                }
            }
            string operType = ent.OperType.ToString();
            string dsJson = JsonConvert.SerializeObject(dsSet);
            ResponseModelClone<string> result = new ResponseModelClone<string>();
            RequestModelClone model = new RequestModelClone(operType, dsJson);
            string json = JsonConvert.SerializeObject(model);
            result = HttpHelper.HttpPost(json);
            if (result.State == "1")
            {
                DataSet ds = new DataSet();
                string error = "";
                if (DataOper.Decompress(result.Result, ref ds, ref error))
                {
                    return ds;
                }
                else
                {
                    throw new Exception("数据解压失败：\r\n" + error);
                }
                //try
                //{
                //    ds = JsonConvert.DeserializeObject<DataSet>(result.Result);
                //}
                //catch (Exception ex)
                //{
                //    throw new Exception("返回值转换失败！ 错误：" + ex.Message.ToString());
                //}

                //return ds;
            }
            else
            {
                throw new Exception(result.Result);
            }
            // return null;
        }
        #endregion

        #region DataTable相关
        public static string GetString(DataRow dr, string fieldName)
        {
            return dr[fieldName] == null || dr[fieldName] == DBNull.Value ? null : dr[fieldName].ToString(); //!dr.Table.Columns.Contains(fieldName)
        }

        public static DateTime GetDateTime(DataRow dr, string fieldName)
        {
            return dr[fieldName] == null || dr[fieldName] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(dr[fieldName]);
        }

        public static Guid GetGuid(DataRow dr, string fieldName)
        {
            return dr[fieldName] == null || dr[fieldName] == DBNull.Value ? System.Guid.NewGuid() : (System.Guid)dr[fieldName];
        }

        public static int GetInt(DataRow dr, string fieldName)
        {
            return dr[fieldName] == null || dr[fieldName] == DBNull.Value ? 0 : (int)dr[fieldName];
        }
        #endregion
    }
}