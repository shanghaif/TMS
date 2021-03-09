using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using DevExpress.XtraReports.UI;
using System.Threading;
using System.IO;

namespace ZQTMS.UI
{
    public partial class frmWayBillOrder : BaseForm
    {
        XtraReport rpt = new XtraReport();//为了加快打印标签的显示速度

        public frmWayBillOrder()
        {
            InitializeComponent();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bgdate.DateTime));
                list.Add(new SqlPara("t2", eddate.DateTime));
                list.Add(new SqlPara("web", (yy_dept.Text.Trim() == "全部" ? "%%" : yy_dept.Text.Trim())));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYORDER", list);
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
            CommonClass.InsertLog("项目订单"); //xj2019/5/28
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3);
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);

            bgdate.DateTime = CommonClass.gbdate;
            eddate.DateTime = CommonClass.gedate;
            CommonClass.SetWeb(yy_dept, "全部", true);

            //if (CommonClass.UserInfo.WebName == "总部")
            //{
            //    yy_dept.Text = "全部";
            //}
            //else
            //{
            //    yy_dept.Text = CommonClass.UserInfo.WebName;
            //    yy_dept.Enabled = false;
            //}

            //加载标签
            Thread tt = new Thread(load_rpt);
            tt.IsBackground = true;
            tt.Start();
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

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rows = myGridView1.FocusedRowHandle;
                if (rows < 0) return;
                string province = myGridView1.GetRowCellValue(rows, "ReceivProvince").ToString();
                string city = myGridView1.GetRowCellValue(rows, "ReceivCity").ToString();
                string area = myGridView1.GetRowCellValue(rows, "ReceivArea").ToString();
                string street = myGridView1.GetRowCellValue(rows, "ReceivStreet").ToString();
                string transferSite = myGridView1.GetRowCellValue(rows, "TransferSite").ToString();

                List<SqlPara> _list = new List<SqlPara>();
                _list.Add(new SqlPara("province", province));
                _list.Add(new SqlPara("city", city));
                _list.Add(new SqlPara("area", area));
                _list.Add(new SqlPara("street", street));
                _list.Add(new SqlPara("transferSite", transferSite));
                SqlParasEntity _sps = new SqlParasEntity(OperType.Query, "QSP_GET_basMiddleSite_Auto", _list);
                DataSet ds = SqlHelper.GetDataSet(_sps);
                if (ds==null || ds.Tables[0].Rows.Count==0)
                {
                    MsgBox.ShowOK("中转地有误，不能生成运单！");
                    return;
                }


                string ZQTMSState = myGridView1.GetRowCellValue(rows, "ZQTMSState").ToString();
                if (ZQTMSState != "")
                {
                    MsgBox.ShowOK("运单已生成，不能再生成！");
                    return;
                }

                DataRow xmdr = myGridView1.GetDataRow(rows);
                //frmWayBillAdd fwb = frmWayBillAdd.Get_frmWayBillAdd;
                frmWayBillAdd fwb = new frmWayBillAdd();
                fwb.rpt = rpt;
                fwb.xmdr = xmdr;
                fwb.ShowDialog();

                if (fwb.xmbillno != "")
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("BillId", myGridView1.GetRowCellValue(rows, "BillId")));
                    list.Add(new SqlPara("ZQTMSbillno", fwb.xmbillno));
                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_WayBillORDER", list);
                    if (SqlHelper.ExecteNonQuery(sps) > 0)
                    {
                        MsgBox.ShowOK();
                        myGridView1.SetRowCellValue(rows, "ZQTMSState", "受理");
                        myGridView1.PostEditor();
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CommonClass.UserInfo.UserDB == UserDB.ZQTMS3PL)
            {
                try
                {
                    int rows = myGridView1.FocusedRowHandle;
                    if (rows < 0) return;

                    string ZQTMSState = myGridView1.GetRowCellValue(rows, "ZQTMSState").ToString();
                    if (ZQTMSState != "")
                    {
                        MsgBox.ShowOK("运单已生成，不能删除！请先通知专线作废运单！");
                        return;
                    }

                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("BillNo", myGridView1.GetRowCellValue(rows, "BillNo")));
                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_WayBillORDER", list);
                    if (SqlHelper.ExecteNonQuery(sps) > 0)
                    {
                        MsgBox.ShowOK();
                        myGridView1.DeleteRow(rows);
                        myGridView1.PostEditor();
                        myGridView1.UpdateCurrentRow();
                        myGridView1.UpdateSummary();
                        DataTable dt = myGridControl1.DataSource as DataTable;
                        dt.AcceptChanges();
                    }
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }

            }
        }
    }
}