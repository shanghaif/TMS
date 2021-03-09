using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using System.Net;
using System.IO;
using System.Web;
using System.Xml;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Threading;
using Newtonsoft.Json.Linq;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using System.Collections;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmPtOrder : BaseForm
    {
        public frmPtOrder()
        {
            InitializeComponent();
        }

        DataSet ds = new DataSet();


        //加载事件
        private void w_online_order_Load(object sender, EventArgs e)
        {
            GridOper.RestoreGridLayout(gridView1, "平台订单");
            bdate.EditValue = CommonClass.gbdate;  //开始时间推前两个星期
            edate.EditValue = CommonClass.gedate;  //当天时间

            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            FixColumn fix = new FixColumn(gridView1, barSubItem2);
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            gridControl1.DataSource = null;

            if (bdate.DateTime > edate.DateTime)
            {
                XtraMessageBox.Show("开始日期不能大于结束日期.", "日期选择错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //DataSet dataSet = new DataSet();
            //try
            //{
            //    SqlCommand cmd = new SqlCommand("QSP_GET_TYD_ZTD");
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.Add("@t1", SqlDbType.DateTime).Value = bdate.DateTime;
            //    cmd.Parameters.Add("@t2", SqlDbType.DateTime).Value = edate.DateTime;
            //    SqlDataAdapter da = new SqlDataAdapter(cmd);
            //    da.Fill(dataSet);
            //    gridControl1.DataSource = dataSet.Tables[0];
            //}
            //catch (Exception ex)
            //{
            //    XtraMessageBox.Show(ex.Message);
            //}

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_TYD_ZTD", list);
                gridControl1.DataSource = SqlHelper.GetDataSet(sps).Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                if (gridView1.RowCount < 1000) gridView1.BestFitColumns();
            }
        }

        //自动筛选
        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(gridView1);
        }

        //锁定外观
        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(gridView1, "平台订单");
        }

        //取消外观
        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(gridView1, "平台订单");
        }

        //过滤器
        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(gridView1);
        }

        //受理
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle < 0) return;

            if (DialogResult.Yes != MsgBox.ShowYesNo("确定授理？")) return;
            int state = Convert.ToInt32(gridView1.GetRowCellValue(rowhandle, "state"));
            string billno = gridView1.GetRowCellValue(rowhandle, "billno").ToString();
            if (state == 1)
            {
                MsgBox.ShowOK("本票已经授理，不用再授理！");
            }
            else if (state == 2)
            {
                MsgBox.ShowOK("本票已经拒绝授理，不用授理！");
            }
            else if (state == 3)
            {
                MsgBox.ShowOK("本票已经生成运单，不用再授理！");
            }
            else if (state == 0)
            {
                USP_APPLY_TYD_YH(billno, 1);
            }
        }

        //不受理
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle < 0) return;

            if (DialogResult.Yes != MsgBox.ShowYesNo("确定不授理？")) return;
            int state = Convert.ToInt32(gridView1.GetRowCellValue(rowhandle, "state"));
            string billno = gridView1.GetRowCellValue(rowhandle, "billno").ToString();
            if (state == 1)
            {
                MsgBox.ShowOK("本票已经授理，不用再授理！");
            }
            else if (state == 2)
            {
                MsgBox.ShowOK("本票已经拒绝授理，不用授理！");
            }
            else if (state == 3)
            {
                MsgBox.ShowOK("本票已经生成运单，不用再授理！");
            }
            else if (state == 0)
            {
                USP_APPLY_TYD_YH(billno, 2);
            }
        }

        //生成运单
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle >= 0)
            {
                int state = Convert.ToInt32(gridView1.GetRowCellValue(rowhandle, "state"));
                if (state < 3)
                {
                    DataRow dr = gridView1.GetDataRow(rowhandle);

                    //w_input_new_row_sa wi = new w_input_new_row_sa();
                    //wi.yhdr = dr;
                    //wi.ShowDialog();

                    //if (wi.yhunit == "") return;
                    //try
                    //{
                    //    string billno = gridView1.GetRowCellValue(rowhandle, "billno").ToString();
                    //    SqlConnection con = cc.GetConn();
                    //    con.Open();
                    //    SqlCommand com = new SqlCommand("USP_CHENGCHENG_TYD_ZTD", con);
                    //    com.CommandType = System.Data.CommandType.StoredProcedure;
                    //    com.Parameters.Add("@billno", SqlDbType.VarChar).Value = billno;
                    //    com.Parameters.Add("@unit", SqlDbType.VarChar).Value = wi.yhunit;
                    //    com.Parameters.Add("@billno1", SqlDbType.VarChar).Value = wi.yhbillno;
                    //    com.ExecuteNonQuery();
                    //    con.Close();
                    //    gridView1.SetRowCellValue(rowhandle, "unit", wi.yhunit);
                    //    gridView1.SetRowCellValue(rowhandle, "billno1", wi.yhbillno);
                    //    gridView1.SetRowCellValue(rowhandle, "state", 3);
                    //    commonclass.ShowOK();
                    //}
                    //catch (Exception ex)
                    //{
                    //    XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //}
                }
            }
        }

        //导出Excel
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(gridView1);
        }

        //关闭阿里网上订单窗体
        private void cbClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            //cc.GenSeq(e);
        }

        private void barButtonItem16_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle >= 0)
            {

                string logisticID = gridView1.GetRowCellValue(rowhandle, "logisticID").ToString();

                frmBillNo wmm = new frmBillNo();
                wmm.edlogisticID.Text = logisticID;
                DialogResult dr = wmm.ShowDialog();

                if (dr == DialogResult.OK)
                {
                    try
                    {
                        if (wmm.edmailNo.Text.Trim() == "")
                        {
                            XtraMessageBox.Show("", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        SqlCommand com = new SqlCommand("USP_ADD_mailNo");
                        com.CommandType = System.Data.CommandType.StoredProcedure;
                        com.Parameters.Add("@logisticID", SqlDbType.VarChar).Value = logisticID;
                        com.Parameters.Add("@mailNo", SqlDbType.VarChar).Value = wmm.edmailNo.Text.Trim();
                        com.ExecuteNonQuery();
                        gridView1.SetRowCellValue(rowhandle, "mailNo", wmm.edmailNo.Text.Trim());
                        MsgBox.ShowOK();
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle < 0) return;

            int unit = gridView1.GetRowCellValue(rowhandle, "unit") == DBNull.Value ? 0 : Convert.ToInt32(gridView1.GetRowCellValue(rowhandle, "unit"));
            if (unit == 0) return;
            //cc.ShowBillDetail(unit);
        }

        private void USP_APPLY_TYD_YH(string billno, int state)
        {
            try
            {
                //SqlCommand sq = new SqlCommand("USP_APPLY_TYD_ZTD");
                //sq.CommandType = CommandType.StoredProcedure;
                //sq.Parameters.Add("@billno", SqlDbType.VarChar).Value = billno;
                //sq.Parameters.Add("@state", SqlDbType.VarChar).Value = state;
                //MsgBox.ShowOK();
                //gridView1.SetRowCellValue(gridView1.FocusedRowHandle,"state",state);
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("billno", billno));
                list.Add(new SqlPara("state", state));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_APPLY_TYD_ZTD", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = gridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                int state = Convert.ToInt32(gridView1.GetRowCellValue(rowhandle, "state"));
                if (state < 3)
                {
                    MsgBox.ShowOK("本票未生成运单，不用签收！");
                    return;
                }
                else if (state == 4)
                {
                    MsgBox.ShowOK("本票已签收，不用再签收！");
                    return;
                }
                if (DialogResult.Yes != MsgBox.ShowYesNo("确认签收？")) return;

                string billno = gridView1.GetRowCellValue(rowhandle, "billno").ToString();
                string fetchman = gridView1.GetRowCellValue(rowhandle, "fetchdate").ToString();
                if (fetchman == "")
                {
                    MsgBox.ShowOK("末端未签收，不能签收！");
                    return;
                }

                System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
                long time_c = (long)Math.Round((CommonClass.gcdate - startTime).TotalMilliseconds, 0);

                string jason = "";
                DataTable dt = CreateJasonOperatorTable();
                DataRow row = dt.NewRow();
                row["tscOrdercode"] = billno;
                row["tscSigner"] = fetchman;
                row["tscSigntime"] = time_c;
                dt.Rows.Add(row);
                ArrayList list = new ArrayList();
                foreach (DataRow dataRow in dt.Rows)
                {
                    Dictionary<string, object> dictionary = new Dictionary<string, object>();  //实例化一个参数集合
                    foreach (DataColumn dataColumn in dt.Columns)
                    {
                        dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName].ToString());
                    }
                    list.Add(dictionary); //ArrayList集合中添加键值
                }

                jason = "\"datas\":" + JsonConvert.SerializeObject(list);
                jason = "{\"validation\":null," + jason + ",\"data\":null}";

                string url = "http://121.201.107.31:8083/IForLanQiao/seller/sign/saveSign";
                string pa = "optData=" + jason;
                string result = sendMessage(url, pa);

                if (result.Contains("200"))
                {
                    USP_APPLY_TYD_YH(billno, 4);
                    MsgBox.ShowOK();
                }
                else
                {
                    MsgBox.ShowOK(result);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        //post
        public static string sendMessage(string strUrl, string PostStr)
        {
            string strResponse = "";
            try
            {
                CookieContainer objCookieContainer = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUrl);
                request.Method = "Post";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Referer = strUrl;//.Remove(strUrl.LastIndexOf("/"));
                Console.WriteLine(strUrl);
                if (objCookieContainer == null)
                    objCookieContainer = new CookieContainer();

                request.CookieContainer = objCookieContainer;
                Console.WriteLine(objCookieContainer.ToString());
                byte[] byteData = Encoding.UTF8.GetBytes(PostStr.ToString().TrimEnd('&'));
                request.ContentLength = byteData.Length;
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(byteData, 0, byteData.Length);
                    // reqStream.Close();
                }
                using (HttpWebResponse res = (HttpWebResponse)request.GetResponse())
                {
                    objCookieContainer = request.CookieContainer;
                    Console.WriteLine(objCookieContainer);
                    Console.WriteLine(res.Server);
                    Console.WriteLine(res.ResponseUri);
                    using (Stream resStream = res.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(resStream, Encoding.UTF8))//.UTF8))
                        {
                            strResponse = sr.ReadToEnd();
                        }
                    }
                    res.Close();
                }
                return strResponse;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private DataTable CreateJasonOperatorTable()
        {
            DataTable dtOperator = new DataTable("datas");
            dtOperator.Columns.Add("tscBillcode", typeof(string));
            dtOperator.Columns.Add("tscOrdercode", typeof(string));
            dtOperator.Columns.Add("tscZdcode", typeof(string));
            dtOperator.Columns.Add("tscSinglecode", typeof(string));
            dtOperator.Columns.Add("tscType", typeof(string));

            dtOperator.Columns.Add("tscCompanyid", typeof(string));
            dtOperator.Columns.Add("tscCompany", typeof(string));
            dtOperator.Columns.Add("tscScanerid", typeof(string));
            dtOperator.Columns.Add("tscScanercode", typeof(string));
            dtOperator.Columns.Add("tscScaner", typeof(string));

            dtOperator.Columns.Add("tscScansiteid", typeof(string));
            dtOperator.Columns.Add("tscScansitecode", typeof(string));
            dtOperator.Columns.Add("tscScansite", typeof(string));
            dtOperator.Columns.Add("tscScantime", typeof(string));
            dtOperator.Columns.Add("tscUpdatetime", typeof(string));

            dtOperator.Columns.Add("tscBusserid", typeof(string));
            dtOperator.Columns.Add("tscBussercode", typeof(string));
            dtOperator.Columns.Add("tscBusser", typeof(string));
            dtOperator.Columns.Add("tscSum", typeof(string));
            dtOperator.Columns.Add("tscWeight", typeof(string));

            dtOperator.Columns.Add("tscGoodstype", typeof(string));
            dtOperator.Columns.Add("tscTranstype", typeof(string));
            dtOperator.Columns.Add("tscCarno", typeof(string));
            dtOperator.Columns.Add("tscPackerid", typeof(string));
            dtOperator.Columns.Add("tscPacker", typeof(string));

            dtOperator.Columns.Add("tscGoods", typeof(string));
            dtOperator.Columns.Add("tscLenght", typeof(string));
            dtOperator.Columns.Add("tscWidth", typeof(string));
            dtOperator.Columns.Add("tscHeight", typeof(string));
            dtOperator.Columns.Add("tscCube", typeof(string));

            dtOperator.Columns.Add("tscRemark", typeof(string));
            dtOperator.Columns.Add("tscCreatdate", typeof(string));
            dtOperator.Columns.Add("tscMachine", typeof(string));
            dtOperator.Columns.Add("tscSendsiteid", typeof(string));
            dtOperator.Columns.Add("tscSendsitecode", typeof(string));

            dtOperator.Columns.Add("tscSendsite", typeof(string));
            dtOperator.Columns.Add("tscRecsiteid", typeof(string));
            dtOperator.Columns.Add("tscRecsitecode", typeof(string));
            dtOperator.Columns.Add("tscRecsite", typeof(string));
            dtOperator.Columns.Add("tscViasiteid", typeof(string));

            dtOperator.Columns.Add("tscViasitecode", typeof(string));
            dtOperator.Columns.Add("tscViasite", typeof(string));
            dtOperator.Columns.Add("tscCarriersid", typeof(string));
            dtOperator.Columns.Add("tscCarriers", typeof(string));
            dtOperator.Columns.Add("tscPaytype", typeof(string));

            dtOperator.Columns.Add("tscPayment", typeof(string));
            dtOperator.Columns.Add("tscPaycome", typeof(string));
            dtOperator.Columns.Add("tscBasicfree", typeof(string));
            dtOperator.Columns.Add("tscSinglefree", typeof(string));
            dtOperator.Columns.Add("tscPsfree", typeof(string));

            dtOperator.Columns.Add("tscUnloadfree", typeof(string));
            dtOperator.Columns.Add("tscUpstairsfree", typeof(string));
            dtOperator.Columns.Add("tscOtherfree", typeof(string));
            dtOperator.Columns.Add("tscSigner", typeof(string));
            dtOperator.Columns.Add("tscSigntime", typeof(string));
            return dtOperator;
        }

        private DataTable CreateJasonBackQtyTable()
        {
            DataTable dtOperator = new DataTable("datas");
            dtOperator.Columns.Add("trbBillcode", typeof(string));
            dtOperator.Columns.Add("trbRbillcode", typeof(string));
            dtOperator.Columns.Add("trbBlreturn", typeof(string));
            dtOperator.Columns.Add("trbReturntime", typeof(string));
            dtOperator.Columns.Add("trbRbillHandleman", typeof(string));
            return dtOperator;
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = gridView1.FocusedRowHandle;
                if (rowhandle < 0) return;

                string backqty = gridView1.GetRowCellValue(rowhandle, "backqty").ToString();
                if (backqty == "")
                {
                    MsgBox.ShowOK("本票无回单，不需要返回！");
                    return;
                }
                int state = Convert.ToInt32(gridView1.GetRowCellValue(rowhandle, "state"));
                if (state < 4)
                {
                    MsgBox.ShowOK("本票未签收，不用回单返回！");
                    return;
                }
                else if (state == 5)
                {
                    MsgBox.ShowOK("本票已签收，不用再签收！");
                    return;
                }
                if (DialogResult.Yes != MsgBox.ShowYesNo("确认回单返回？")) return;

                string billno = gridView1.GetRowCellValue(rowhandle, "billno").ToString();
                string fetchman = gridView1.GetRowCellValue(rowhandle, "fetchman").ToString();
                if (fetchman == "")
                {
                    MsgBox.ShowOK("末端未签收，不能返回单！");
                    return;
                }

                System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
                long time_c = (long)Math.Round((CommonClass.gcdate - startTime).TotalMilliseconds, 0);

                string jason = "";
                DataTable dt = CreateJasonBackQtyTable();
                DataRow row = dt.NewRow();
                row["trbBillcode"] = billno;
                row["trbBlreturn"] = 1;
                row["trbReturntime"] = time_c;
                row["trbRbillHandleman"] = CommonClass.UserInfo.UserName;
                dt.Rows.Add(row);
                ArrayList list = new ArrayList();
                foreach (DataRow dataRow in dt.Rows)
                {
                    Dictionary<string, object> dictionary = new Dictionary<string, object>();  //实例化一个参数集合
                    foreach (DataColumn dataColumn in dt.Columns)
                    {
                        dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName].ToString());
                    }
                    list.Add(dictionary); //ArrayList集合中添加键值
                }

                jason = "\"datas\":" + JsonConvert.SerializeObject(list);
                jason = "{\"validation\":null," + jason + ",\"data\":null}";
                string url = "http://121.201.107.31:8083/IForLanQiao/seller/returnbill/create";
                string pa = "optData=" + jason;
                string result = sendMessage(url, pa);

                if (result.Contains("200"))
                {
                    USP_APPLY_TYD_YH(billno, 5);
                    MsgBox.ShowOK();
                }
                else
                {
                    MsgBox.ShowOK(result);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }
    }
}