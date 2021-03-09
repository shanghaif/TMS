using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SendSmsService
{
    class SqlHelper
    {
        public static SqlConnection GetConn { get { return new SqlConnection("Data Source=120.76.233.81,5433;Initial Catalog=KMS20160713;User ID=lq;Password=lq123!@#"); } }

        public static DataSet GetDataSet(string text, params SqlParameter[] param)
        {
            if (string.IsNullOrEmpty(text)) return null;

            SqlCommand cmd = new SqlCommand(text, GetConn);
            cmd.CommandType = CommandType.StoredProcedure;

            if (param != null && param.Length > 0)
                cmd.Parameters.AddRange(param);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            try
            {
                adapter.Fill(ds);
                if (ds == null || ds.Tables.Count == 0) return null;

                return ds;
            }
            catch
            {
                return null;
            }
        }

        public static DataTable GetDataTable(string text, params SqlParameter[] param)
        {
            DataSet ds = GetDataSet(text, param);
            if (ds == null || ds.Tables.Count == 0) return null;
            return ds.Tables[0];
        }

        public static int ENQ(string text, params SqlParameter[] param)
        {
            SqlConnection con = GetConn;

            SqlCommand cmd = new SqlCommand(text, con);
            cmd.CommandType = CommandType.StoredProcedure;

            if (param != null && param.Length > 0)
                cmd.Parameters.AddRange(param);

            con.Open();
            try
            {
                int result = cmd.ExecuteNonQuery();
                return result == 0 ? 1 : result;
            }
            catch { return 0; }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
            }
        }
    }
}
