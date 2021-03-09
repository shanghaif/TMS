using System;
using System.Collections.Generic;
using System.Text;

namespace ZQTMS.Tool
{
    /// <summary>
    /// 类型转换
    /// </summary>
    public static class ConvertType
    {
        /// <summary>
        /// 将object转换为Decimal类型的数字，如果转换失败，返回0
        /// </summary>
        /// <param name="o">要转换的值</param>
        /// <returns>返回Decimal类型的数字，如果转换失败返回0</returns>
        public static decimal ToDecimal(object o)
        {
            string s = (o + "").Trim();
            if (s == "") return 0;

            decimal result = 0;
            if (!decimal.TryParse(s, out result))
            {
                throw new Exception(string.Format("数值【{0}】转换类型失败!", o));
            }
            return result;
        }

        /// <summary>
        /// 将o转换为Decimal再转成字符串，如果转换失败返回r
        /// </summary>
        /// <param name="o">要转换的值</param>
        /// <param name="r">转换失败返回的值</param>
        /// <returns></returns>
        public static string ToDecimal(object o, string r)
        {
            string s = (o + "").Trim();
            if (s == "") return r;

            try
            {
                decimal d = Convert.ToDecimal(s);
                string d2 = d.ToString();
                d2 = d2.Contains(".") ? d2.TrimEnd('0').TrimEnd('.') : d2;
                return d == 0 ? r : d2;
            }
            catch
            {
                return r;
            }
        }

        /// <summary>
        /// 将o转换为Int类型的数字，如果转换失败，返回0
        /// </summary>
        /// <param name="o">要转换的值</param>
        /// <returns>返回Int32类型的数字，如果转换失败返回0</returns>
        public static int ToInt32(object o)
        {
            string s = (o + "").Trim();
            if (s == "") return 0;

            int result = 0;
            if (!int.TryParse(s, out result))
            {
                throw new Exception(string.Format("数值【{0}】转换失败!", o));
            }
            return result;
        }

        /// <summary>
        /// 将o转换为Long类型的数字，如果转换失败，返   回0
        /// </summary>
        /// <param name="o">要转换的值</param>
        /// <returns>返回Long类型的数字，如果转换失败返回0</returns>
        public static long ToLong(object o)
        {
            string s = (o + "").Trim();
            if (s == "") return 0;

            long result = 0;
            if (!long.TryParse(s, out result))
            {
                throw new Exception(string.Format("数值【{0}】转换失败!", o));
            }
            return result;
        }

        /// <summary>
        /// 将o转换为float类型的数字，如果转换失败，返回0
        /// </summary>
        /// <param name="o">要转换的值</param>
        /// <returns>返回float类型的数字，如果转换失败返回0</returns>
        public static float ToFloat(object o)
        {
            string s = (o + "").Trim();
            if (s == "") return 0;

            float result = 0;
            if (!float.TryParse(s, out result))
            {
                throw new Exception(string.Format("数值【{0}】转换类型失败!", o));
            }
            return result;
        }

        /// <summary>
        /// 将o转换为double类型的数字，如果转换失败，返回0
        /// </summary>
        /// <param name="o">要转换的值</param>
        /// <returns>返回double类型的数字，如果转换失败返回0</returns>
        public static double ToDouble(object o)
        {
            string s = (o + "").Trim();
            if (s == "") return 0;

            double result = 0;
            if (!double.TryParse(s, out result))
            {
                throw new Exception(string.Format("数值【{0}】转换类型失败!", o));
            }
            return result;
        }

        /// <summary>
        /// 将o转换为字符串类型，如果转换失败返回 ""
        /// </summary>
        /// <param name="o">要转换的值</param>
        /// <returns>将o转换为字符串类型，如果转换失败返回 ""</returns>
        public static string ToString(object o)
        {
            return (o + "").Trim();
        }

        /// <summary>
        /// 将对象转换为数值(DateTime)类型,转换失败返回DateTime.Now。
        /// </summary>
        /// <param name="o">对象。</param>
        /// <returns>DateTime值。</returns>
        public static DateTime ToDateTime(object o)
        {
            string s = (o + "").Trim();

            DateTime dt = DateTime.Now;
            if (!DateTime.TryParse(s, out dt))
            {
                throw new Exception(string.Format("数值【{0}】转换类型失败!", o));
            }
            if (dt <= DateTime.MinValue || dt >= DateTime.MaxValue)
            {
                throw new Exception(string.Format("时间值【{0}】超出范围!", o));
            }
            return dt;
        }
    }
}
