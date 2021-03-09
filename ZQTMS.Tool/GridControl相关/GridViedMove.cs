using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraGrid.Views.Grid;
using System.Data;

namespace ZQTMS.Tool
{
    public class GridViewMove
    {
        /// <summary>
        /// 从一个网格的数据源中，把数据行移动到另外一个数据表
        /// <para>前提：两个数据表格式必须一致</para>
        /// </summary>
        /// <param name="gv">源网格</param>
        /// <param name="fromTable">原数据表</param>
        /// <param name="toTable">目的数据表</param>
        public static void Move(GridView gv, DataTable fromTable, DataTable toTable)
        {
            try
            {
                int[] r = gv.GetSelectedRows();
                for (int i = 0; i < r.Length; i++)
                {
                    DataRow dr = gv.GetDataRow(r[i]);
                    toTable.ImportRow(dr);
                }
                gv.DeleteSelectedRows();
                fromTable.AcceptChanges();
                toTable.AcceptChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 从一个网格的数据源中，把数据行移动到另外一个数据表
        /// <para>前提：两个数据表格式必须一致</para>
        /// <para>默认移动第一个表</para>
        /// </summary>
        /// <param name="gv">源网格</param>
        /// <param name="fromDataSet">原数据表</param>
        /// <param name="toDataSet">目的数据表</param>
        public static void Move(GridView gv, DataSet fromDataSet, DataSet toDataSet)
        {
            if (fromDataSet == null || fromDataSet.Tables.Count == 0 || toDataSet == null || toDataSet.Tables.Count == 0) return;
            Move(gv, fromDataSet.Tables[0], toDataSet.Tables[0]);
        }
        public static void MoveQY(GridView gv, DataSet fromDataSet, DataSet toDataSet, DataSet oldDataSet)
        {
            if (fromDataSet == null || fromDataSet.Tables.Count == 0 || toDataSet == null || toDataSet.Tables.Count == 0) return;
            MoveQY(gv, fromDataSet.Tables[0], toDataSet.Tables[0], oldDataSet.Tables[0]);
        }

        /// <summary>
        /// 汽运数据
        /// <para>前提：两个数据表格式必须一致</para>
        /// </summary>
        /// <param name="gv">源网格</param>
        /// <param name="fromTable">原数据表</param>
        /// <param name="toTable">目的数据表</param>
        public static void MoveQY(GridView gv, DataTable fromTable, DataTable toTable, DataTable oldTable)
        {
            try
            {

                string carNo = "";
                string batch = "";
                string feeType = "";
                DateTime dt = new DateTime();

                string carNo2 = "";
                string batch2 = "";
                string feeType2 = "";
                DateTime dt2 = new DateTime();

                int[] r = gv.GetSelectedRows();
                int[] dl = new int[gv.RowCount];
                for (int i = 0; i < r.Length; i++)
                {
                    DataRow dr = gv.GetDataRow(r[i]);
                    toTable.ImportRow(dr);
                    carNo = ConvertType.ToString(gv.GetRowCellValue(r[i], "CarNO"));
                    batch = ConvertType.ToString(gv.GetRowCellValue(r[i], "DepartureBatch"));
                    feeType = ConvertType.ToString(gv.GetRowCellValue(r[i], "FeeType"));
                    dt = ConvertType.ToDateTime(gv.GetRowCellValue(r[i], "BbusinessDate"));
                    if (feeType == "大车费回付")
                    {
                        for (int v = 0; v < gv.RowCount; v++) //遍历所有行
                        {
                            if (r[i] != v)
                            {
                                carNo2 = ConvertType.ToString(gv.GetRowCellValue(v, "CarNO"));
                                batch2 = ConvertType.ToString(gv.GetRowCellValue(v, "DepartureBatch"));
                                feeType2 = ConvertType.ToString(gv.GetRowCellValue(v, "FeeType"));
                                dt2 = ConvertType.ToDateTime(gv.GetRowCellValue(v, "BbusinessDate"));
                                if (carNo == carNo2 && dt2 <= dt && (feeType2 == "车辆代扣" || feeType2 == "大车油料费"))
                                {
                                    DataRow dr3 = gv.GetDataRow(v);
                                    toTable.ImportRow(dr3);
                                    dl[v] = v + 1;
                                    //gv.DeleteRow(v);
                                }
                                if ((feeType2 == "大车司机奖罚费" || feeType2 == "大车增减款") && batch == batch2)
                                {
                                    DataRow dr3 = gv.GetDataRow(v);
                                    toTable.ImportRow(dr3);
                                    dl[v] = v + 1;
                                    //gv.DeleteRow(v);
                                }
                            }
                        }
                        //DataRow[] oldDt = fromTable.Select("CarNO='" + carNo + "' And BbusinessDate <= '" + dt + "'" + "and (FeeType='车辆代扣' or FeeType='大车油料费') or (FeeType='大车司机奖罚费' and DepartureBatch='" + batch + "') or (FeeType='大车增减款' and DepartureBatch='" + batch + "')");
                        //if (oldDt.Length > 0)
                        //{
                        //    DataRow[] toDt = toTable.Select();
                        //    if (toDt.Length == 0)
                        //    {
                        //        for (int o = 0; o < oldDt.Length; o++)
                        //        {
                        //            toTable.ImportRow(oldDt[o]);
                        //            gv.DeleteRow(i);
                        //        }
                        //    }
                        //}
                    }


                }
                for (int d = dl.Length - 1; d >= 0; d--)
                {
                    if (dl[d] != 0)
                    {
                        gv.DeleteRow(dl[d] - 1);
                    }
                }
                gv.DeleteSelectedRows();
                fromTable.AcceptChanges();
                toTable.AcceptChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //try
            //{
            //    string carNo = "";
            //    string batch = "";
            //    string feeType = "";
            //    DateTime dt = new DateTime();
            //    int[] r = gv.GetSelectedRows();
            //    for (int i = 0; i < r.Length; i++)
            //    {

            //        carNo = ConvertType.ToString(gv.GetRowCellValue(r[i], "CarNO"));
            //        batch = ConvertType.ToString(gv.GetRowCellValue(r[i], "DepartureBatch"));
            //        feeType = ConvertType.ToString(gv.GetRowCellValue(r[i], "FeeType"));
            //        dt = ConvertType.ToDateTime(gv.GetRowCellValue(r[i], "BbusinessDate"));
            //        if (feeType == "大车费回付")
            //        {
            //            //DataRow[] oldDt = oldTable.Select("CarNO='" + carNo + "' And BbusinessDate < '" + dt + "'" );
            //            DataRow[] oldDt = fromTable.Select("CarNO='" + carNo + "' And BbusinessDate <= '" + dt + "'" + "and (FeeType='车辆代扣' or FeeType='大车油料费') or (FeeType='大车司机奖罚费' and DepartureBatch='" + batch + "') or (FeeType='大车增减款' and DepartureBatch='" + batch + "')");
            //            if (oldDt.Length>0)
            //            {
            //                DataRow[] toDt=toTable.Select();
            //                if(toDt.Length==0)
            //                {
            //                    for (int o = 0; o < oldDt.Length; o++)
            //                    {
            //                        toTable.ImportRow(oldDt[o]);
            //                    }
            //                }
            //                else
            //                {
            //                    for (int o = 0; o < oldDt.Length; o++)
            //                    {
            //                        int oldId = Convert.ToInt32(oldDt[o]["AID"]);
            //                        int oldNum = 0;
            //                        for (int t = 0; t < toDt.Length;t++ )
            //                        {
            //                            if (oldId == Convert.ToInt32(toDt[t]["AID"]))
            //                            {
            //                                oldNum = 1;
            //                            }
            //                        }
            //                        if (oldNum==0)
            //                        {
            //                            toTable.ImportRow(oldDt[o]);
            //                        }

            //                    }
            //                }
            //                //DataRow dr = gv.GetDataRow(r[i]);
            //                //toTable.ImportRow(dr);
            //            }
            //            //for (int j = 0; j < gv.RowCount; j++)
            //            //{
            //            //    if (ConvertType.ToString(gv.GetRowCellValue(j, "CarNO")) == carNo && dt > ConvertType.ToDateTime(gv.GetRowCellValue(j, "BbusinessDate"))
            //            //        && (ConvertType.ToString(gv.GetRowCellValue(j, "FeeType")) == "车辆代扣" 
            //            //        || ConvertType.ToString(gv.GetRowCellValue(j, "FeeType")) == "大车油料费"
            //            //         || ConvertType.ToString(gv.GetRowCellValue(j, "FeeType")) == "大车司机奖罚费"))
            //            //    {
            //            //        DataRow dr1 = gv.GetDataRow(j);
            //            //        toTable.ImportRow(dr1);
            //            //    }
            //            //    if (ConvertType.ToString(gv.GetRowCellValue(j, "FeeType")) == "大车增减款" && ConvertType.ToString(gv.GetRowCellValue(j, "DepartureBatch")) == batch)
            //            //    {
            //            //        DataRow dr1 = gv.GetDataRow(j);
            //            //        toTable.ImportRow(dr1);
            //            //    }
            //            //}
            //        }
            //        DataRow drw = gv.GetDataRow(r[i]);
            //        toTable.ImportRow(drw);
            //    }
            //    gv.DeleteSelectedRows();
            //    fromTable.AcceptChanges();
            //    toTable.AcceptChanges();
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }
    }
}