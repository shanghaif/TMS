using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Net;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using DevExpress.XtraReports.UI;
using System.Threading;
using System.IO;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace ZQTMS.UI
{
    public partial class frmWayBillOrderXCX : BaseForm
    {
        XtraReport rpt = new XtraReport();//为了加快打印标签的显示速度

        public frmWayBillOrderXCX()
        {
            InitializeComponent();
        }
        private string url = "";
        public string billNo = "";
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bgdate.DateTime));
                list.Add(new SqlPara("t2", eddate.DateTime));
                list.Add(new SqlPara("orderState", orderType.Text.Trim() == "未处理" ? "0" : "1"));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WayBillForXMDD", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void BespeakSendGoods_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3);
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);

            bgdate.DateTime = CommonClass.gbdate;
            eddate.DateTime = CommonClass.gedate;

            //加载标签
            Thread tt = new Thread(load_rpt);
            tt.IsBackground = true;
            tt.Start();
            GridOper.CreateStyleFormatCondition(myGridView1, "confirmType", DevExpress.XtraGrid.FormatConditionEnum.Equal, "接受", Color.Green);
            GridOper.CreateStyleFormatCondition(myGridView1, "confirmType", DevExpress.XtraGrid.FormatConditionEnum.Equal, "拒绝", Color.Red);
        }

        private void load_rpt()
        {
            //加载标签
            string fileName = Application.StartupPath + "\\Reports\\标签.repx";
            if (File.Exists(fileName))
            {
                rpt.LoadLayout(fileName);
            }
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void createOrder_XCX(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rows = myGridView1.FocusedRowHandle;
                if (rows < 0) return;
                //if (myGridView1.GetRowCellValue(rows, "DK_state").ToString() == "已开单")
                //{
                //    MsgBox.ShowOK("运单已生成，不能再生成！");
                //    return;
                //}
                //if (myGridView1.GetRowCellValue(rows, "DK_state").ToString() == "已取消")
                //{
                //    MsgBox.ShowOK("运单已取消，不能生成！");
                //    return;
                //}
                string legNo = myGridView1.GetRowCellValue(rows, "orderSn").ToString();
                List<SqlPara> _list = new List<SqlPara>();
                _list.Add(new SqlPara("OrderSn", legNo));
                SqlParasEntity _sps = new SqlParasEntity(OperType.Query, "QSP_GET_WayBillForXMDD_ByOrder", _list);
                DataSet ds = SqlHelper.GetDataSet(_sps);
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    MsgBox.ShowOK("没有订单明细信息！");
                    return;
                }

                DataRow xmdr = myGridView1.GetDataRow(rows);
                frmWayBillAdd_JMGX_Upgrade fwb = new frmWayBillAdd_JMGX_Upgrade();
                fwb.xmdr = xmdr;
                fwb.dsReBillNoHy = ds;
                //fwb.companyid = myGridView1.GetRowCellValue(rows, "companyid").ToString();
                fwb.ShowDialog();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }
        /// <summary>
        /// 用MD5加密字符串
        /// </summary>
        /// <param name="password">待加密的字符串</param>
        /// <returns></returns>
        public static string MD5EncryptUTF_8(string password)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] hashedDataBytes;
            hashedDataBytes = md5Hasher.ComputeHash(Encoding.GetEncoding("UTF-8").GetBytes(password));
            StringBuilder tmp = new StringBuilder();
            foreach (byte i in hashedDataBytes)
            {
                tmp.Append(i.ToString("x2"));
            }
            return tmp.ToString();
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rows = myGridView1.FocusedRowHandle;
            if (rows < 0) return;
            int state = Convert.ToInt32(myGridView1.GetRowCellValue(rows, "ConfirmStatus").ToString() == "" ? 0 : myGridView1.GetRowCellValue(rows, "ConfirmStatus"));
            if (state == 1)
            {
                MsgBox.ShowOK("已确认订单不能修改！");
                return;
            }
            DataRow xmdr = myGridView1.GetDataRow(rows);
            frmBillOrderXCXUpDate fm = new frmBillOrderXCXUpDate();
            fm.dr = xmdr;
            fm.ShowDialog();
        }

        private void barButtonItem14_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            if (DialogResult.Yes != MsgBox.ShowYesNo("确定确认？")) return;
            int state = Convert.ToInt32(myGridView1.GetRowCellValue(rowhandle, "ConfirmStatus").ToString() == "" ? 0 : myGridView1.GetRowCellValue(rowhandle, "ConfirmStatus"));
            string orderSn = myGridView1.GetRowCellValue(rowhandle, "orderSn").ToString();
            if (state == 1)
            {
                MsgBox.ShowOK("该单号已经确认过了！");
                return;
            }
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("orderSn", orderSn));
            SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_Confirm_SmallProgramIDOrder", list);
            if (SqlHelper.ExecteNonQuery(sps) > 0)
            {
                MsgBox.ShowOK();
            }

        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmWayBillRecordZQWL frm = new frmWayBillRecordZQWL();
            frm.Show();
        }

        /// <summary>
        /// 批量转运单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem16_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                //if()
                for (int rows = 0; rows < myGridView1.RowCount; rows++)
                {
                    string legNo = myGridView1.GetRowCellValue(rows, "orderSn").ToString();
                    List<SqlPara> _list = new List<SqlPara>();
                    _list.Add(new SqlPara("OrderSn", legNo));
                    SqlParasEntity _sps = new SqlParasEntity(OperType.Query, "QSP_GET_WayBillForXMDD_ByOrder", _list);
                    DataSet ds = SqlHelper.GetDataSet(_sps);
                    if (ds == null || ds.Tables[0].Rows.Count == 0)
                    {
                        MsgBox.ShowOK("没有订单明细信息！");
                        return;
                    }

                    DataRow xmdr = myGridView1.GetDataRow(rows);
                    //把一些默认的补充到行 
                    frmWayBillAdd_JMGX_Upgrade fwb = new frmWayBillAdd_JMGX_Upgrade();
                    fwb.xmdr = xmdr;
                    fwb.dsReBillNoHy = ds;
                    fwb.Show();
                    fwb.orderSn = legNo;
                    fwb.barButtonItem1_ItemClick(sender, null);
                    fwb.Close();
                }
                //int rows = myGridView1.FocusedRowHandle;
                //if (rows < 0) return; 
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

    }
}
[Serializable]
public class XcxResult
{
    public int code { get; set; }

    public string Msg { get; set; }

    public string result { get; set; }
}