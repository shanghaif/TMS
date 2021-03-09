using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using System.Threading;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using ZQTMS.Lib;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;

namespace ZQTMS.UI
{
    public partial class frmPayInfos : BaseForm
    {
        GridColumn gcIsseleckedMode;
        List<int> editRows = new List<int>();
        public frmPayInfos()
        {
            InitializeComponent();
        }
        private void getdata()
        {
            myGridView1.ClearColumnsFilter();
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("billno",tBillno.Text.Trim()));
                list.Add(new SqlPara("PayWay",cbbPayWay.Text.Trim()));
             
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_PayInfos", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            getdata();
        }
 
        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }
         
        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridControl1.MainView as MyGridView);
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void frmPayInfos_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("支付信息记录");//xj/2019/5/28
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例

          

            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);

            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            gcIsseleckedMode = GridOper.GetGridViewColumn(myGridView1, "ischecked"); 
      
       
        }

        //手动同步
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.textBox1.Text))
            {
                MsgBox.ShowOK("请输入批次号！");
                return;
            }
            List<SqlPara> list_check = new List<SqlPara>();
            list_check.Add(new SqlPara("Batch", this.textBox1.Text.Trim()));
            SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_Get_InterFace_By_Data", list_check);
            DataSet ds_bill = SqlHelper.GetDataSet(spe);
            if (ds_bill == null || ds_bill.Tables.Count == 0 || ds_bill.Tables[0].Rows.Count == 0)
            {
                MsgBox.ShowOK("获取失败！");
                return;
            }
            foreach (DataRow row in ds_bill.Tables[0].Rows)
            {
                if (row["FaceUrl"].ToString().Contains("LMSSysDeparttureZQTMS"))
                {
                    CommonSyn.ShouDongLMSDepartureSysZQTMS(row["FaceJson"].ToString(), 0, row["Face_Guid"].ToString());
                }
                else
                {
                    CommonSyn.ShouDongLMSDepartureSysZQTMS(row["FaceJson"].ToString(), 2, row["Face_Guid"].ToString());
                }
            }
        }
        //新增
        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmPayInfosAdd frm = new frmPayInfosAdd();
            frm.ShowDialog();
        }
        //确认
        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            string ConfirmState = myGridView1.GetRowCellValue(rowhandle, "ConfirmState").ToString();
            if (ConfirmState == "已确认")
            {
                MsgBox.ShowOK("该支付信息已确认,无法再次确认!");
                return;
            }
            string inid = myGridView1.GetRowCellValue(rowhandle, "inid").ToString();
            string billno = myGridView1.GetRowCellValue(rowhandle, "billno").ToString();
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("inid", inid));
            list.Add(new SqlPara("billno", billno));
            SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "QSP_Confirm_PayInfos", list);
            if (SqlHelper.ExecteNonQuery(sps) > 0)
            {
                MsgBox.ShowOK();
                getdata();
            }
        }
        //取消确认
        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            string ConfirmState = myGridView1.GetRowCellValue(rowhandle, "ConfirmState").ToString();
            if (ConfirmState != "已确认")
            {
                MsgBox.ShowOK("该支付信息未确认,无法取消!");
                return;
            }
            string inid = myGridView1.GetRowCellValue(rowhandle, "inid").ToString();
            string billno = myGridView1.GetRowCellValue(rowhandle, "billno").ToString();
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("inid", inid));
            list.Add(new SqlPara("billno", billno));
            SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "QSP_REConfirm_PayInfos", list);
            if (SqlHelper.ExecteNonQuery(sps) > 0)
            {
                MsgBox.ShowOK();
                getdata();
            }
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            int a = checkEdit1.Checked == true ? 1 : 0;
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                myGridView1.SetRowCellValue(i, gcIsseleckedMode, a);
            }
        }
        /// <summary>
        /// 批量确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView1.PostEditor();
            string billnostr = "";
            string ConfirmState = "";
            string billStr = "";
            string inidStr = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {

                if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) == 0) continue;
                ConfirmState = myGridView1.GetRowCellValue(i, "ConfirmState").ToString();
                billStr = myGridView1.GetRowCellValue(i, "billno").ToString();
                if (ConfirmState == "已确认")
                {
                    MsgBox.ShowOK("运单:" + billStr + "支付信息已确认,无法再次确认!");
                    return;
                }
                inidStr += GridOper.GetRowCellValueString(myGridView1, i, "inid") + "@";
                billnostr += GridOper.GetRowCellValueString(myGridView1, i, "billno") + "@";
            }
            if (billnostr == "") return;
            if (MsgBox.ShowYesNo("是否确认选中的运单？") != DialogResult.Yes) return;
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("inids", inidStr));
            list.Add(new SqlPara("billnos", billnostr));
            SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "QSP_Confirm_PayInfos_ByInids", list);
            if (SqlHelper.ExecteNonQuery(sps) > 0)
            {
                MsgBox.ShowOK();
                getdata();
            }
        }
    }
}