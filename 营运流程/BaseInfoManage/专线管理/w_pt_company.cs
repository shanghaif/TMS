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
    public partial class w_pt_company : BaseForm
    {
        DataSet ds = new DataSet();

        public w_pt_company()
        {
            InitializeComponent();
        }

        private void getdata()
        {
            List<SqlPara> list = new List<SqlPara>();
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_COMPANY", list);
            DataSet ds = SqlHelper.GetDataSet(sps);
            if (ds == null || ds.Tables.Count == 0) return;
            gridControl1.DataSource = ds.Tables[0];
        }


        private void w_cygs_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("专线管理");//xj/2019/5/29
            BarMagagerOper.SetBarPropertity(bar1); 
            getdata();
            
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            barButtonItem5_ItemClick(null, null);
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            w_pt_company_add war = new w_pt_company_add();
            war.i = 1;  //毛慧20171204
            war.ShowDialog();
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            int handle = gridView1.FocusedRowHandle;

            if (handle >= 0)
            {
                w_pt_company_add war = new w_pt_company_add();
                war.gv = gridView1;
                war.ds = ds;
                war.id = gridView1.GetRowCellValue(handle, "id").ToString();
                war.i = 0;  //毛慧20171204
                war.ShowDialog();
            }
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = gridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid Id = new Guid(gridView1.GetRowCellValue(rowhandle, "id").ToString());

                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("id", Id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_COMPANY", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();

                    gridView1.DeleteRow(rowhandle);
                    gridView1.PostEditor();
                    gridView1.UpdateCurrentRow();
                    gridView1.UpdateSummary();
                    DataTable dt = gridControl1.DataSource as DataTable;
                    dt.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            getdata();
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        
    }

}








