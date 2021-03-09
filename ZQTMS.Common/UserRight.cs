using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraBars;
using ZQTMS.SqlDAL;
using ZQTMS.Tool;
using System.Data;

namespace ZQTMS.Common
{

    public static class UserRight
    {
        /// <summary>
        /// 获取权限数据集
        /// </summary>
        /// <param name="GRCode">权限组编号，多选</param>
        public static bool GetUserRightDataSet(string GRCode)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("GRCode", GRCode.Replace(',', '@') + "@"));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_UserRight_ByCode_KT", list);
                CommonClass.dsUserRight = SqlHelper.GetDataSet(sps);
                return true;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                return false;
            }
        }

        /// <summary>
        /// 根据指定的tag，检测是否具有权限
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static bool GetRight(string tag)
        {
            if (CommonClass.dsUserRight == null || CommonClass.dsUserRight.Tables.Count == 0)
            {
                return false;
            }
            int MenuId = 0;
            try
            {
                MenuId = ConvertType.ToInt32(tag);
            }
            catch { }
            return CommonClass.dsUserRight.Tables[0].Select(string.Format("(GRTag='{0}' or MenuID={1}) and GRFlag=1", tag, MenuId)).Length > 0;
        }

        /// <summary>
        /// 针对多个功能共用界面
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="MenuID"></param>
        /// <returns></returns>
        public static bool GetRight(string tag, string MenuID)
        {
            if (CommonClass.dsUserRight == null || CommonClass.dsUserRight.Tables.Count == 0)
            {
                return false;
            }
            int MId = 0;
            try
            {
                MId = ConvertType.ToInt32(tag);
            }
            catch { }
            return CommonClass.dsUserRight.Tables[0].Select(string.Format("(GRTag='{0}' or MenuID={1}) and GRFlag=1 and Convert(MenuID, 'System.String') like '{2}%'", tag, MId, MenuID)).Length > 0;
        }

        public static BarItemVisibility GetRightVisibility(string tag)
        {
            return GetRight(tag) ? BarItemVisibility.Always : BarItemVisibility.Never;
        }

        public static BarItemVisibility GetRightVisibility(string tag, string MenuID)
        {
            return GetRight(tag, MenuID) ? BarItemVisibility.Always : BarItemVisibility.Never;
        }

        /// <summary>
        ///  获取DataSet中第一个Table行数，为空则返回-1
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static int GetRowCount(DataSet ds)
        {
            return ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null ? ds.Tables[0].Rows.Count : -1;
        }

        /// <summary>
        ///  获取DataSet中指定Table行数，为空则返回-1
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int GetRowCount(DataSet ds, string tableName)
        {
            return ds != null && ds.Tables.Count > 0 && ds.Tables[tableName] != null ? ds.Tables[tableName].Rows.Count : -1;
        }

        /// <summary>
        /// 获取DataTable行数，为空则返回-1
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int GetRowCount(DataTable dt)
        {
            return dt != null ? dt.Rows.Count : -1;
        }
    }
}
