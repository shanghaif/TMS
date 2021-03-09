using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace ZQTMS.Tool
{
    class StringSimilar
    {
        #region 私有变量
        /// <summary>
        /// 字符串1
        /// </summary>
        private char[] _ArrChar1;
        /// <summary>
        /// 字符串2
        /// </summary>
        private char[] _ArrChar2;
        /// <summary>
        /// 统计结果
        /// </summary>
        private Result _Result;
        /// <summary>
        /// 开始时间
        /// </summary>
        private DateTime _BeginTime;
        /// <summary>
        /// 结束时间
        /// </summary>
        private DateTime _EndTime;
        /// <summary>
        /// 计算次数
        /// </summary>
        private int _ComputeTimes;
        /// <summary>
        /// 算法矩阵
        /// </summary>
        private int[,] _Matrix;
        /// <summary>
        /// 矩阵列数
        /// </summary>
        private int _Column;
        /// <summary>
        /// 矩阵行数
        /// </summary>
        private int _Row;
        #endregion

        #region 属性
        public Result ComputeResult
        {
            get { return _Result; }
        }
        #endregion

        #region 构造函数
        public StringSimilar(string str1, string str2)
        {
            this.LevenshteinDistanceInit(str1, str2);
        }
        public StringSimilar()
        {
        }
        #endregion

        #region 算法实现
        /// <summary>
        /// 初始化算法基本信息
        /// </summary>
        /// <param name="str1">字符串1</param>
        /// <param name="str2">字符串2</param>
        private void LevenshteinDistanceInit(string str1, string str2)
        {
            _ArrChar1 = str1.ToCharArray();
            _ArrChar2 = str2.ToCharArray();
            _Result = new Result();
            _ComputeTimes = 0;
            _Row = _ArrChar1.Length + 1;
            _Column = _ArrChar2.Length + 1;
            _Matrix = new int[_Row, _Column];
        }

        /// <summary>
        /// 计算相似度
        /// </summary>
        public void Compute()
        {
            _BeginTime = DateTime.Now;
            this.InitMatrix();
            int intCost = 0;
            for (int i = 1; i < _Row; i++)
            {
                for (int j = 1; j < _Column; j++)
                {
                    if (_ArrChar1[i - 1] == _ArrChar2[j - 1])
                    {
                        intCost = 0;
                    }
                    else
                    {
                        intCost = 1;
                    }
                    _Matrix[i, j] = this.Minimum(_Matrix[i - 1, j] + 1, _Matrix[i, j - 1] + 1, _Matrix[i - 1, j - 1] + intCost);
                    _ComputeTimes++;
                }
            }
            _EndTime = DateTime.Now;
            int intLength = _Row > _Column ? _Row : _Column;
            _Result.Rate = (1 - (double)_Matrix[_Row - 1, _Column - 1] / intLength).ToString();//.Substring(0, 6);
            if (_Result.Rate.Length > 6)
            {
                _Result.Rate = _Result.Rate.Substring(0, 6);
            }
            _Result.UseTime = (_EndTime - _BeginTime).TotalMilliseconds.ToString() + " 毫秒";
            _Result.ComputeTimes = _ComputeTimes.ToString() + " 距离为：" + _Matrix[_Row - 1, _Column - 1].ToString();
        }

        /// <summary>
        /// 计算相似度
        /// </summary>
        /// <param name="str1">字符串1</param>
        /// <param name="str2">字符串2</param>
        public void Compute(string str1, string str2)
        {
            this.LevenshteinDistanceInit(str1, str2);
            this.Compute();
        }

        /// <summary>
        /// 初始化矩阵的第一行和第一列
        /// </summary>
        private void InitMatrix()
        {
            for (int i = 0; i < _Column; i++)
            {
                _Matrix[0, i] = i;
            }
            for (int i = 0; i < _Row; i++)
            {
                _Matrix[i, 0] = i;
            }
        }

        /// <summary>
        /// 取三个数中的最小值
        /// </summary>
        /// <param name="First"></param>
        /// <param name="Second"></param>
        /// <param name="Third"></param>
        /// <returns></returns>
        private int Minimum(int First, int Second, int Third)
        {
            return Math.Min(Math.Min(First, Second), Third);
        }
        #endregion

        /// <summary>
        /// 计算结果
        /// </summary>
        public struct Result
        {
            /// <summary>
            /// 相似度
            /// </summary>
            public string Rate;
            /// <summary>
            /// 对比次数
            /// </summary>
            public string ComputeTimes;
            /// <summary>
            /// 使用时间
            /// </summary>
            public string UseTime;
        }

        /// <summary>
        /// 自动匹配
        /// </summary>
        /// <param name="ds1">要匹配的数据集</param>
        /// <param name="repositoryItemImageComboBox1">下拉选项</param>
        /// <param name="projectno">只有信封特殊，其他可以传空值</param>
        public static void GetRate(DataSet ds1, DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox repositoryItemImageComboBox1, string projectno)
        {
            try
            {
                decimal rate = 0; //相似度
                int index = -1; //匹配的索引
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    rate = 0;
                    index = -1;
                    for (int j = 0; j < repositoryItemImageComboBox1.Items.Count; j++)
                    {
                        if (ds1.Tables[0].Rows[i]["billitem"].ToString() == "提货费" && repositoryItemImageComboBox1.Items[j].Description == "接货费")
                        {
                            rate = 0; index = -1;
                            ds1.Tables[0].Rows[i]["systemitem"] = repositoryItemImageComboBox1.Items[j].Value;
                            break;
                        }
                        if (ds1.Tables[0].Rows[i]["billitem"].ToString() == "货物名称" && repositoryItemImageComboBox1.Items[j].Description == "品名1")
                        {
                            rate = 0; index = -1;
                            ds1.Tables[0].Rows[i]["systemitem"] = repositoryItemImageComboBox1.Items[j].Value;
                            break;
                        }
                        if (ds1.Tables[0].Rows[i]["billitem"].ToString() == "卸货费" && repositoryItemImageComboBox1.Items[j].Description == "装卸费")
                        {
                            rate = 0; index = -1;
                            ds1.Tables[0].Rows[i]["systemitem"] = repositoryItemImageComboBox1.Items[j].Value;
                            break;
                        }
                        if (ds1.Tables[0].Rows[i]["billitem"].ToString() == "代理费" && repositoryItemImageComboBox1.Items[j].Description == "回扣")
                        {
                            rate = 0; index = -1;
                            ds1.Tables[0].Rows[i]["systemitem"] = repositoryItemImageComboBox1.Items[j].Value;
                            break;
                        }
                        if (ds1.Tables[0].Rows[i]["billitem"].ToString() == "重量单价" && repositoryItemImageComboBox1.Items[j].Description == "元/Kg")
                        {
                            rate = 0; index = -1;
                            ds1.Tables[0].Rows[i]["systemitem"] = repositoryItemImageComboBox1.Items[j].Value;
                            break;
                        }
                        if (ds1.Tables[0].Rows[i]["billitem"].ToString() == "体积单价" && repositoryItemImageComboBox1.Items[j].Description == "元/方")
                        {
                            rate = 0; index = -1;
                            ds1.Tables[0].Rows[i]["systemitem"] = repositoryItemImageComboBox1.Items[j].Value;
                            break;
                        }
                        if (ds1.Tables[0].Rows[i]["billitem"].ToString() == "件数单价" && repositoryItemImageComboBox1.Items[j].Description == "元/件")
                        {
                            rate = 0; index = -1;
                            ds1.Tables[0].Rows[i]["systemitem"] = repositoryItemImageComboBox1.Items[j].Value;
                            break;
                        }
                        if ((ds1.Tables[0].Rows[i]["billitem"].ToString() == "交接方式" || ds1.Tables[0].Rows[i]["billitem"].ToString() == "提货方式" || ds1.Tables[0].Rows[i]["billitem"].ToString() == "送货方式") && repositoryItemImageComboBox1.Items[j].Description == "交货方式（送货或自提）显示文字")
                        {
                            rate = 0; index = -1;
                            ds1.Tables[0].Rows[i]["systemitem"] = repositoryItemImageComboBox1.Items[j].Value;
                            break;
                        }
                        if ((ds1.Tables[0].Rows[i]["billitem"].ToString() == "运输方式" || ds1.Tables[0].Rows[i]["billitem"].ToString() == "运输要求") && repositoryItemImageComboBox1.Items[j].Description == "发货要求文字")
                        {
                            rate = 0; index = -1;
                            ds1.Tables[0].Rows[i]["systemitem"] = repositoryItemImageComboBox1.Items[j].Value;
                            break;
                        }
                        if (ds1.Tables[0].Rows[i]["billitem"].ToString() == "代理费" && repositoryItemImageComboBox1.Items[j].Description == "回扣")
                        {
                            ds1.Tables[0].Rows[i]["systemitem"] = repositoryItemImageComboBox1.Items[j].Value;
                            break;
                        }
                        if (ds1.Tables[0].Rows[i]["billitem"].ToString() == "扣运费" && repositoryItemImageComboBox1.Items[j].Description == "货款扣")
                        {
                            ds1.Tables[0].Rows[i]["systemitem"] = repositoryItemImageComboBox1.Items[j].Value;
                            break;
                        }
                        if (ds1.Tables[0].Rows[i]["billitem"].ToString() == "扣运费" && repositoryItemImageComboBox1.Items[j].Description == "货款扣")
                        {
                            ds1.Tables[0].Rows[i]["systemitem"] = repositoryItemImageComboBox1.Items[j].Value;
                            break;
                        }
                        if (projectno == "信封")
                        {
                            if (ds1.Tables[0].Rows[i]["billitem"].ToString() == "备注" && repositoryItemImageComboBox1.Items[j].Description == "品名")
                            {
                                rate = 0; index = -1;
                                ds1.Tables[0].Rows[i]["systemitem"] = repositoryItemImageComboBox1.Items[j].Value;
                                break;
                            }
                            if (ds1.Tables[0].Rows[i]["billitem"].ToString() == "品名" && repositoryItemImageComboBox1.Items[j].Description == "备注")
                            {
                                rate = 0; index = -1;
                                ds1.Tables[0].Rows[i]["systemitem"] = repositoryItemImageComboBox1.Items[j].Value;
                                break;
                            }
                        }

                        StringSimilar similar = new StringSimilar(ds1.Tables[0].Rows[i]["billitem"].ToString(), repositoryItemImageComboBox1.Items[j].Description);
                        similar.Compute();

                        StringSimilar.Result result = similar.ComputeResult;

                        if (Convert.ToDecimal(result.Rate) > rate)
                        {
                            rate = Convert.ToDecimal(result.Rate);
                            index = j;
                        }
                    }
                    if (rate > (decimal)0.45)
                    {
                        ds1.Tables[0].Rows[i]["systemitem"] = repositoryItemImageComboBox1.Items[index].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            } 
        }
    }
}
