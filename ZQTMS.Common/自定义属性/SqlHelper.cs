using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Newtonsoft.Json;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using Newtonsoft.Json.Converters;

namespace ZQTMS.Common
{
    /// <summary>
    /// 此类为抽象类，不允许实例化，在应用时直接调用即可
    /// </summary>
    public abstract class SqlHelper
    {
        //public static readonly string connectionString = System.Configuration.ConfigurationSettings.AppSettings["con"].ToString().Trim();
        //private static readonly string connectionString = "server=.;database=ty2015;uid=sa;pwd=goushi;Pooling=true;Min Pool Size=0;Max Pool Size=500";
        //private static readonly string connectionString = "server=114.55.109.85;database=ZQTMS20160713;uid=.;pwd=123456;Pooling=true;Min Pool Size=0;Max Pool Size=500";

        private static int ver = 1;//函数版本

        /// <summary>
        /// 返回数据集DataSet
        /// </summary>
        /// <param name="ent">SqlParasEntity实体对象，参数集合；默认执行SQL操作</param>
        /// <returns></returns>
        public static int ExecteNonQuery(SqlParasEntity ent)
        {
            int flag = ExecteNonQuery(ent, ver);
            return flag;
        }
        #region zaj 2017-11-7
        public static int ExecteNonQuery(SqlParasEntity ent, int ver)
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
            ent.ParaList.Add(new SqlPara("LoginAreaName", CommonClass.UserInfo.AreaName));
            ent.ParaList.Add(new SqlPara("LoginCauseName", CommonClass.UserInfo.CauseName));
            ent.ParaList.Add(new SqlPara("LoginDepartName", CommonClass.UserInfo.DepartName));
            ent.ParaList.Add(new SqlPara("LoginSiteName", CommonClass.UserInfo.SiteName));
            ent.ParaList.Add(new SqlPara("LoginWebName", CommonClass.UserInfo.WebName));
            ent.ParaList.Add(new SqlPara("LoginUserAccount", CommonClass.UserInfo.UserAccount));
            ent.ParaList.Add(new SqlPara("LoginUserName", CommonClass.UserInfo.UserName));
            ent.ParaList.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));
            #endregion
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
            //将数据压缩在调用接口 zaj 2018-5-24
            string s1 = "";
            if (!DataOper.Compress(dsJson, ref s1))
            {
                throw new Exception("数据压缩失败：\r\n" + s1);
            }
            ResponseModelClone<string> result = new ResponseModelClone<string>();
            //RequestModelClone model = new RequestModelClone(operType, dsJson);
             RequestModelClone model = new RequestModelClone(operType, s1);

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

        /// <summary>
        /// 执行数据库操作
        /// </summary>
        /// <param name="ent">SqlParasEntity实体对象，参数集合</param>
        /// <param name="ver">版本号：1默认为SQL操作  88OA数据库的相关操作</param>
        /// <returns></returns>
        //public static int ExecteNonQuery(SqlParasEntity ent, int ver)
        //{
        //    #region 登录信息
        //    ent.ParaList.Add(new SqlPara("LoginAreaName", CommonClass.UserInfo.AreaName));
        //    ent.ParaList.Add(new SqlPara("LoginCauseName", CommonClass.UserInfo.CauseName));
        //    ent.ParaList.Add(new SqlPara("LoginDepartName", CommonClass.UserInfo.DepartName));
        //    ent.ParaList.Add(new SqlPara("LoginSiteName", CommonClass.UserInfo.SiteName));
        //    ent.ParaList.Add(new SqlPara("LoginWebName", CommonClass.UserInfo.WebName));
        //    ent.ParaList.Add(new SqlPara("LoginUserAccount", CommonClass.UserInfo.UserAccount));
        //    ent.ParaList.Add(new SqlPara("LoginUserName", CommonClass.UserInfo.UserName));
        //    ent.ParaList.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));
        //    #endregion

        //    SqlResult result = new SqlResult();
        //    JsonSerializerSettings setting = new JsonSerializerSettings();
        //    setting.DateFormatString = "yyyy-MM-dd HH:mm:ss.fff";
        //    string json = JsonConvert.SerializeObject(ent, setting);
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
        //        return ConvertType.ToString(result.Result).Equals("数据更新成功") ? 1 : 0;
        //    }
        //    else
        //    {
        //        throw new Exception("执行操作返回信息:\r\n" + result.Result.Replace("数据库访问异常：", "").Replace("错误信息：", "").Trim());
        //    }
        //}

        /// <summary>
        /// 返回数据集DataSet
        /// </summary>
        /// <param name="ent">SqlParasEntity实体对象，参数集合；默认执行SQL操作</param>
        /// <returns></returns>
        public static DataSet GetDataSet(SqlParasEntity ent)
        {
            DataSet ds = GetDataSet(ent, ver);
            return ds;
        }

        /// <summary>
        /// 返回数据集DataSet
        /// </summary>
        /// <param name="ent">SqlParasEntity实体对象，参数集合</param>
        /// <param name="ver">版本号：1默认为SQL操作   99提取服务端时间  88提取OA数据库的相关操作</param>
        /// <returns></returns>
        //public static DataSet GetDataSet(SqlParasEntity ent, int ver)
        //{
        //    if (ent.ParaList == null)
        //    {
        //        List<SqlPara> list = new List<SqlPara>();
        //        ent.ParaList = list;
        //    }

        //    #region 登录信息
        //    ent.ParaList.Add(new SqlPara("LoginAreaName", CommonClass.UserInfo.AreaName));
        //    ent.ParaList.Add(new SqlPara("LoginCauseName", CommonClass.UserInfo.CauseName));
        //    ent.ParaList.Add(new SqlPara("LoginDepartName", CommonClass.UserInfo.DepartName));
        //    ent.ParaList.Add(new SqlPara("LoginSiteName", CommonClass.UserInfo.SiteName));
        //    ent.ParaList.Add(new SqlPara("LoginWebName", CommonClass.UserInfo.WebName));
        //    ent.ParaList.Add(new SqlPara("LoginUserAccount", CommonClass.UserInfo.UserAccount));
        //    ent.ParaList.Add(new SqlPara("LoginUserName", CommonClass.UserInfo.UserName));
        //    ent.ParaList.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));
        //    #endregion
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
        //            return CommonClass.CheckDataSetNullRow(ds);
        //        }
        //        catch
        //        {
        //            try
        //            {
        //                DataTable dt = JsonConvert.DeserializeObject<DataTable>(DataOper.Decompress(result.Result));
        //                ds = new DataSet();
        //                ds.Tables.Add(dt);
        //                return CommonClass.CheckDataSetNullRow(ds);
        //            }
        //            catch (Exception ex)
        //            {
        //                MsgBox.ShowOK(ex.Message + "—" + ent.ProcedureName);
        //                return null;
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
            ent.ParaList.Add(new SqlPara("LoginAreaName", CommonClass.UserInfo.AreaName));
            ent.ParaList.Add(new SqlPara("LoginCauseName", CommonClass.UserInfo.CauseName));
            ent.ParaList.Add(new SqlPara("LoginDepartName", CommonClass.UserInfo.DepartName));
            ent.ParaList.Add(new SqlPara("LoginSiteName", CommonClass.UserInfo.SiteName));
            ent.ParaList.Add(new SqlPara("LoginWebName", CommonClass.UserInfo.WebName));
            ent.ParaList.Add(new SqlPara("LoginUserAccount", CommonClass.UserInfo.UserAccount));
            ent.ParaList.Add(new SqlPara("LoginUserName", CommonClass.UserInfo.UserName));
            ent.ParaList.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));
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
            //将数据压缩在调用接口 zaj 2018-5-24
            string s1 = "";
            if (!DataOper.Compress(dsJson, ref s1))
            {
                throw new Exception("数据压缩失败：\r\n" + s1);
            }

            ResponseModelClone<string> result = new ResponseModelClone<string>();
            //RequestModelClone model = new RequestModelClone(operType, dsJson);
             RequestModelClone model = new RequestModelClone(operType, s1);

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





        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <param name="ent">SqlParasEntity实体对象，参数集合</param>
        /// <returns></returns>
        public static DataTable GetDataTable(SqlParasEntity ent)
        {
            DataSet ds = GetDataSet(ent);
            if (ds == null || ds.Tables.Count == 0) return null;
            return ds.Tables[0];
        }

        /// <summary>
        /// 获取应用服务器时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetServerDate()
        {
            try
            {
                SqlResult result = new SqlResult();
                result = HttpHelper.HttpPost1(string.Format("ver={0}&json={1}", 99, ""));
                if (result.State == 1)
                {
                    return Convert.ToDateTime(result.Result);
                }
                else
                {
                    return DateTime.Now;
                }
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
        }

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

        #region hj20180423 提取ZQTMS整站库存
        /// <summary>
        /// 返回数据集DataSet
        /// </summary>
        /// <param name="ent">SqlParasEntity实体对象，参数集合；默认执行SQL操作</param>
        /// <returns></returns>
        public static DataSet GetDataSet_ZQTMS(SqlParasEntity ent)
        {
            DataSet ds = GetDataSet_ZQTMS(ent, ver);
            return ds;
        }

        /// <summary>
        /// 返回数据集DataSet，提取ZQTMS的数据 hj20180423
        /// </summary>
        /// <param name="ent">SqlParasEntity实体对象，参数集合</param>
        /// <param name="ver"></param>
        /// <returns></returns>
        public static DataSet GetDataSet_ZQTMS(SqlParasEntity ent, int ver)
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
            ent.ParaList.Add(new SqlPara("LoginAreaName", CommonClass.UserInfo.AreaName));
            ent.ParaList.Add(new SqlPara("LoginCauseName", CommonClass.UserInfo.CauseName));
            ent.ParaList.Add(new SqlPara("LoginDepartName", CommonClass.UserInfo.DepartName));
            ent.ParaList.Add(new SqlPara("LoginSiteName", CommonClass.UserInfo.SiteName));
            ent.ParaList.Add(new SqlPara("LoginWebName", CommonClass.UserInfo.WebName));
            ent.ParaList.Add(new SqlPara("LoginUserAccount", CommonClass.UserInfo.UserAccount));
            ent.ParaList.Add(new SqlPara("LoginUserName", CommonClass.UserInfo.UserName));
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
            SqlResult result = new SqlResult();

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            SqlParasTable data = new SqlParasTable(ent.OperType, dsSet);
            string json = JsonConvert.SerializeObject(data, settings);

            string s1 = "";

            if (!DataOper.Compress_ZQTMS(json, ref s1))
            {
                result.State = 0;
                result.Result = s1;
            }

            result = HttpHelper.HttpPost_ZQTMS(string.Format("ver={0}&json={1}", ver, s1));
            if (result.State == 1)
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
            }
            else
            {
                throw new Exception(result.Result);
            }
        }
        #endregion
        /// <summary>
        /// 返回数据集DataSet
        /// </summary>
        /// <param name="ent">SqlParasEntity实体对象，参数集合；默认执行SQL操作</param>
        /// <returns></returns>
        public static int ExecteNonQuery_ZQTMS(SqlParasEntity ent)  //maohui20180622
        {
            int flag = ExecteNonQuery_ZQTMS(ent, ver);
            return flag;
        }

        public static int ExecteNonQuery_ZQTMS(SqlParasEntity ent, int ver)  //maohui20180622
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
            ent.ParaList.Add(new SqlPara("LoginAreaName", CommonClass.UserInfo.AreaName));
            ent.ParaList.Add(new SqlPara("LoginCauseName", CommonClass.UserInfo.CauseName));
            ent.ParaList.Add(new SqlPara("LoginDepartName", CommonClass.UserInfo.DepartName));
            ent.ParaList.Add(new SqlPara("LoginSiteName", CommonClass.UserInfo.SiteName));
            ent.ParaList.Add(new SqlPara("LoginWebName", CommonClass.UserInfo.WebName));
            ent.ParaList.Add(new SqlPara("LoginUserAccount", CommonClass.UserInfo.UserAccount));
            ent.ParaList.Add(new SqlPara("LoginUserName", CommonClass.UserInfo.UserName));
            #endregion

            foreach (SqlPara item in ent.ParaList)
            {
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

            SqlResult result = new SqlResult();

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            SqlParasTable data = new SqlParasTable(ent.OperType, dsSet);
            string json = JsonConvert.SerializeObject(data, settings);
            string s1 = "";

            if (!DataOper.Compress_ZQTMS(json, ref s1))
            {
                result.State = 0;
                result.Result = s1;
            }

            result = HttpHelper.HttpPost_ZQTMS_Execte(string.Format("ver={0}&json={1}", ver, s1));
            if (result.State == 1)
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

        #region lyj 添加URL
        public static int ExecteNonQuery(SqlParasEntity ent, int ver, string url)
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
            ent.ParaList.Add(new SqlPara("LoginAreaName", CommonClass.UserInfo.AreaName));
            ent.ParaList.Add(new SqlPara("LoginCauseName", CommonClass.UserInfo.CauseName));
            ent.ParaList.Add(new SqlPara("LoginDepartName", CommonClass.UserInfo.DepartName));
            ent.ParaList.Add(new SqlPara("LoginSiteName", CommonClass.UserInfo.SiteName));
            ent.ParaList.Add(new SqlPara("LoginWebName", CommonClass.UserInfo.WebName));
            ent.ParaList.Add(new SqlPara("LoginUserAccount", CommonClass.UserInfo.UserAccount));
            ent.ParaList.Add(new SqlPara("LoginUserName", CommonClass.UserInfo.UserName));
            ent.ParaList.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));
            #endregion
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
            //将数据压缩在调用接口 zaj 2018-5-24
            string s1 = "";
            if (!DataOper.Compress(dsJson, ref s1))
            {
                throw new Exception("数据压缩失败：\r\n" + s1);
            }
            ResponseModelClone<string> result = new ResponseModelClone<string>();
            RequestModelClone model = new RequestModelClone(operType, s1);

            string json = JsonConvert.SerializeObject(model);
            result = HttpHelper.HttpPost(json, url);
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
    }
}