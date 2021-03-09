using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace ZQTMS.Common
{
    public class OperResult
    {
        /// <summary>
        /// 执行状态
        /// <para>1：成功 0：失败</para>
        /// </summary>
        public int State
        {
            get;
            set;
        }

        /// <summary>
        /// 执行结果
        /// </summary>
        public string Msg
        {
            get;
            set;
        }

        /// <summary>
        /// 结果数据集
        /// </summary>
        public DataSet DataSet
        {
            get;
            set;
        }
    }

    public class SQLiteHelper
    {
        static string lolalFile = "Client.dll";//本地文件名
        static string _db = Path.Combine(Application.StartupPath, lolalFile);

        /// <summary>
        /// 本地数据库文件名，登录成功之后检测
        /// </summary>
        public static string LocalDB
        {
            get 
            {
                return lolalFile;
            }
            set
            {
                lolalFile = value;
                _db = Path.Combine(Application.StartupPath, lolalFile);
            }
        }

        string pwd = "lanqiao2016";
        string sqliteconnstr = "";//数据库路径

        public SQLiteHelper()
        {
            sqliteconnstr = string.Format("Data Source={0};Pooling=true;FailIfMissing=false", _db);
            CreateDbFile();
        }

        private SQLiteConnection GetConn()
        {
            SQLiteConnection conn = new SQLiteConnection(sqliteconnstr);
            if (pwd != "")
            {
                conn.SetPassword(pwd);
            }
            return conn;
        }

        public OperResult SetPwd(string oldPwd, string newPwd)
        {
            OperResult oper = new OperResult();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(sqliteconnstr))
                {
                    if (oldPwd != "")
                    {
                        conn.SetPassword(pwd);
                    }
                    conn.Open();
                    conn.ChangePassword(newPwd); //给数据库设置密码
                    conn.Close();
                }
                oper.State = 1;
            }
            catch (Exception ex)
            {
                oper.State = 0;
                oper.Msg = ex.Message;
            }
            return oper;
        }

        public OperResult CancelPwd()
        {
            OperResult oper = new OperResult();
            try
            {
                using (SQLiteConnection conn = GetConn())
                {
                    conn.Open();
                    conn.ChangePassword(""); //给数据库加密。读取的时候如果不用SetPassword设置密码，就打不开
                    conn.Close();
                }
                oper.State = 1;
            }
            catch (Exception ex)
            {
                oper.State = 0;
                oper.Msg = ex.Message;
            }
            return oper;
        }

        /// <summary>
        /// 创建本地数据库
        /// <para>创建成功返回1，失败返回0</para>
        /// </summary>
        /// <returns></returns>
        private OperResult CreateDbFile()
        {
            OperResult oper = new OperResult();
            try
            {
                if (File.Exists(_db))
                {
                    oper.State = 1;
                }
                else
                {
                    SQLiteConnection.CreateFile(_db); //创建一个空数据库
                    using (SQLiteConnection conn = GetConn())
                    {
                        conn.Open();
                        conn.ChangePassword(pwd); //设置数据库密码
                        conn.Close();
                    }
                    oper.State = 1;
                }
            }
            catch (Exception ex)
            {
                oper.State = 0;
                oper.Msg = ex.Message;
            }
            return oper;
        }

        /// <summary>
        /// 执行单个SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public OperResult ExecuteNonQuery(string sql)
        {
            OperResult oper = new OperResult();
            try
            {
                if (!File.Exists(_db))
                {
                    oper.State = 0;
                    oper.Msg = "没有本地数据库!";
                }
                else
                {
                    using (SQLiteConnection conn = GetConn())
                    {
                        SQLiteCommand cmd = new SQLiteCommand(sql, conn);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        conn.Dispose();
                    }
                    oper.State = 1; 
                }
            }
            catch (Exception ex)
            {
                oper.State = 0;
                oper.Msg = ex.Message;
            }
            return oper;
        }

        /// <summary>
        /// 执行SQL，返回数据集
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public OperResult GetDataSet(string sql)
        {
            OperResult oper = new OperResult();
            try
            {
                if (!File.Exists(_db))
                {
                    oper.State = 0;
                    oper.Msg = "没有本地数据库!";
                    oper.DataSet = null;
                }
                else
                {
                    using (SQLiteConnection conn = GetConn())
                    {
                        SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn);

                        DataSet ds = new DataSet();
                        da.Fill(ds);

                        oper.State = 1;
                        oper.DataSet = ds;
                    }
                }
            }
            catch (Exception ex)
            {
                oper.State = 0;
                oper.Msg = ex.Message;
            }
            return oper;
        }

        /// <summary>
        /// 批量执行多个SQL
        /// </summary>
        /// <param name="listSql"></param>
        /// <returns></returns>
        public OperResult ExecuteNonQuery(List<string> listSql)
        {
            OperResult oper = new OperResult();
            try
            {
                if (!File.Exists(_db))
                {
                    oper.State = 0;
                    oper.Msg = "没有本地数据库!";
                }
                else
                {
                    using (SQLiteConnection conn = GetConn())
                    {
                        conn.Open();
                        using (SQLiteTransaction trans = conn.BeginTransaction())
                        {
                            using (SQLiteCommand cmd = new SQLiteCommand(conn))
                            {
                                cmd.Transaction = trans;
                                foreach (string sql in listSql)
                                {
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            trans.Commit();
                            trans.Dispose();
                            conn.Close();
                            conn.Dispose();
                        }
                    }
                    oper.State = 1; 
                }
            }
            catch (Exception ex)
            {
                oper.State = 0;
                oper.Msg = ex.Message;
            }
            return oper;
        }

        public OperResult SaveDataTable(DataTable dt, string tableName)
        {
            OperResult oper = new OperResult();
            int times = 0;
        lb_save:
            try
            {
                using (SQLiteConnection _con = GetConn())
                {
                    SQLiteDataAdapter oda = new SQLiteDataAdapter("select * from " + tableName + " limit 1", _con);
                    SQLiteCommandBuilder ocb = new SQLiteCommandBuilder(oda);
                    oda.InsertCommand = ocb.GetInsertCommand();
                    //oda.UpdateCommand = ocb.GetUpdateCommand();
                    //oda.DeleteCommand = ocb.GetDeleteCommand();

                    _con.Open();

                    using (SQLiteTransaction _tran = _con.BeginTransaction())
                    {
                        int result = oda.Update(dt);
                        _tran.Commit();
                        _tran.Dispose();
                        oper.State = 1; 
                    }
                    _con.Close();
                    _con.Dispose();
                }
            }
            catch (Exception ex)
            {
                if (times < 3)
                {
                    times++;
                    goto lb_save;
                }
                oper.State = 0;
                oper.Msg = "保存数据缓存出错：" + ex.Message;
            }
            return oper;
        }

        /// <summary>     
        /// 判断SQLite数据库表是否存在    
        /// </summary>
        /// <param name="tableName">要检测的表名</param>
        public bool IsTableExist(string tableName)
        {
            using (SQLiteConnection conn = GetConn())
            {
                conn.Open();
                using (SQLiteCommand command = new SQLiteCommand(conn))
                {
                    command.CommandText = "SELECT COUNT(*) FROM sqlite_master where type='table' and name='" + tableName + "'";
                    int iaaa = Convert.ToInt32(command.ExecuteScalar());
                    if (Convert.ToInt32(command.ExecuteScalar()) == 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }

        /// <summary>     
        /// 查询数据库中的所有数据类型信息     
        /// </summary>     
        /// <returns></returns>     
        public DataTable GetSchema()
        {
            using (SQLiteConnection connection = GetConn())
            {
                connection.Open();
                DataTable data = connection.GetSchema("TABLES");
                connection.Close();
                return data;
            }
        } 

        public string DataColumnTypeToSqlite_Type(DataColumn dc)
        {
            if (dc.DataType == typeof(string))
            {
                return "VARCHAR(200)";
            }
            else if (dc.DataType == typeof(int) || dc.DataType == typeof(Int64) || dc.DataType == typeof(long))
            {
                return "INT64";
            }
            else if (dc.DataType == typeof(float) || dc.DataType == typeof(decimal) || dc.DataType == typeof(double) || dc.DataType == typeof(Single) || dc.DataType == typeof(decimal))
            {
                return "DECIMAL(10,3)";
            }
            else if (dc.DataType == typeof(Guid))
            {
                return "GUID";
            }
            else if (dc.DataType == typeof(DateTime))
            {
                return "DATETIME";
            }

            return "";
        }
    }
}
