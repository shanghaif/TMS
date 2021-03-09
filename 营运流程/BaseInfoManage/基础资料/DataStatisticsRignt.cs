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
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class DataStatisticsRignt : BaseForm
    {
        public DataStatisticsRignt()
        {
            InitializeComponent();
        }



        private void DataStatisticsRignt_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3);
            getdata();
        }



        private void getdata()
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_DataStatisticsRignt");
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }



      


        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            DataStatisticsRignt_add frm = new DataStatisticsRignt_add();
            frm.ShowDialog();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                string companyid = myGridView1.GetRowCellValue(rowhandle, "companyid").ToString();
                if (companyid != CommonClass.UserInfo.companyid)
                {
                    MsgBox.ShowOK("只能修改自己公司数据！");
                    return;
                }
                int id = Convert.ToInt32(myGridView1.GetRowCellValue(rowhandle, "ID").ToString());

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_DataStatisticsRignt_byID", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                DataRow dr = ds.Tables[0].Rows[0];

                DataStatisticsRignt_add frm = new DataStatisticsRignt_add();
                frm.dr = dr;
                frm.isModify = 1;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                int id = Convert.ToInt32(myGridView1.GetRowCellValue(rowhandle, "ID").ToString());
                string companyid = myGridView1.GetRowCellValue(rowhandle, "companyid").ToString();
                if (companyid != CommonClass.UserInfo.companyid)
                {

                    MsgBox.ShowOK("只能删除自己公司的数据！");
                    return;
                }

                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_DataStatisticsRignt", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    myGridView1.DeleteRow(rowhandle);
                    myGridView1.PostEditor();
                    myGridView1.UpdateCurrentRow();
                    myGridView1.UpdateSummary();
                    DataTable dt = myGridControl1.DataSource as DataTable;
                    dt.AcceptChanges();
                    getdata();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            getdata();
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void myGridControl1_DoubleClick(object sender, EventArgs e)
        {
            barButtonItem2.PerformClick();
        }
    }
}
