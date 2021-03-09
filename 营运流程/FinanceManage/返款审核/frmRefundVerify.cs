using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmRefundVerify : BaseForm
    {
        private DataSet ds = new DataSet();
        private DataSet ds1 = new DataSet();

        public frmRefundVerify()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 按单号提取库存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            List<SqlPara> list = new List<SqlPara>();
            try
            {
                list.Add(new SqlPara("billNo", tbBillNo.Text.ToString()));
                ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_BILLOUT_REFUND_REPERTORY", list));
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];

                if (ds1 == null || ds1.Tables.Count == 0)
                {
                    ds1 = ds.Clone();
                    myGridControl2.DataSource = ds1.Tables[0];
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void frmRefundVerify_Load(object sender, EventArgs e)
        {
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, false, myGridView2);
            GridOper.SetGridViewProperty(myGridView1, myGridView2);
            BarMagagerOper.SetBarPropertity(bar1, bar2);  //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1, myGridView2); 
        }

        private void myGridControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GridViewMove.Move(myGridView1, ds, ds1);
        }

        private void myGridControl2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GridViewMove.Move(myGridView2, ds1, ds);
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myGridView1.SelectAll();
            GridViewMove.Move(myGridView1, ds, ds1);
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridViewMove.Move(myGridView1, ds, ds1);
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridViewMove.Move(myGridView2, ds1, ds);
        }

        private void barButtonItem14_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myGridView2.SelectAll();
            GridViewMove.Move(myGridView2, ds1, ds);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView2);
        }

        /// <summary>
        /// 作废单返款核销
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
             DataTable dt = (DataTable)myGridControl2.DataSource;
             string AIDs = "";
            string billNos = "";
            string serialNumbers = "";
            string causeNames = "";
            string areaNames = "";
            string webNames = "";
            string inorouts = "";
            string feeTypes = "";
            string moneys = "";

            //serialNumber = dt.Rows[0]["SerialNumber"].ToString();
            //causeName = dt.Rows[0]["CauseName"].ToString();
            //areaName = dt.Rows[0]["AreaName"].ToString();
            //webName = dt.Rows[0]["BegWeb"].ToString();
            for (int i = 0; i < dt.Rows.Count; i++ )
            {
                AIDs += dt.Rows[i]["AID"].ToString() + "@";
                inorouts += dt.Rows[i]["INOROUT"].ToString() + "@";
                moneys += dt.Rows[i]["Money"].ToString() + "@";
                feeTypes += dt.Rows[i]["FeeType"].ToString() + "@";
                billNos += dt.Rows[i]["billNo"].ToString() + "@";

                serialNumbers += dt.Rows[i]["SerialNumber"].ToString() + "@";
                causeNames += dt.Rows[i]["CauseName"].ToString() + "@";
                areaNames += dt.Rows[i]["AreaName"].ToString() + "@";
                webNames += dt.Rows[i]["BegWeb"].ToString() + "@";


                //string feeType = dt.Rows[i]["FeeType"].ToString();
                //if (feeType.IndexOf("付") > -1) // 如果费用类型包含“付”字则改成收
                //{
                //    feeType = feeType.Replace("付", "收");
                //}
                //else if (feeType.IndexOf("收") > -1) // 如果费用类型包含“付”字则改成收
                //{
                //    feeType = feeType.Replace("收", "付");
                //}
                //else
                //{
                //    feeType = "收" + feeType;
                //}
                //feeTypes += feeType + "@";
            }

           
            List<SqlPara> list = new List<SqlPara>();
            try
            {
                list.Add(new SqlPara("AIDs", AIDs));
                list.Add(new SqlPara("billNos", billNos));
                list.Add(new SqlPara("serialNumbers", serialNumbers));
                list.Add(new SqlPara("causeNames", causeNames));
                list.Add(new SqlPara("areaNames", areaNames));
                list.Add(new SqlPara("webNames", webNames));
                list.Add(new SqlPara("inorouts", inorouts));
                list.Add(new SqlPara("feeTypes", feeTypes));
                list.Add(new SqlPara("moneys", moneys));

                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "QSP_ADD_BILLOUTREFUND", list)) > 0)
                {
                    MsgBox.ShowOK();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 按时间提取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            List<SqlPara> list = new List<SqlPara>();
            try
            {
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_BILLOUT_REFUND_BYID", list));
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];

                if (ds1 == null || ds1.Tables.Count == 0)
                {
                    ds1 = ds.Clone();
                    myGridControl2.DataSource = ds1.Tables[0];
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem16_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
            GridOper.ShowAutoFilterRow(myGridView2);
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
            GridOper.AllowAutoFilter(myGridView2);
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
            GridOper.SaveGridLayout(myGridView2, myGridView2.Guid.ToString());
        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
            GridOper.DeleteGridLayout(myGridView2, myGridView2.Guid.ToString());
        }
    }
}
