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
using DevExpress.XtraGrid.Views.Grid;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class w_员工情况汇总 : BaseForm
    {

        DataSet ds = new DataSet();
        GridView[] gv;
        string[] sql = new string[7];

        public w_员工情况汇总()
        {
            InitializeComponent();
        }
        
        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            int i = comboBoxEdit1.SelectedIndex;
            gridControl1.MainView = gv[i];
            try
            {
                //fixcolumn fc = new fixcolumn(gridView1, barSubItem2);

                //SqlCommand da = new SqlCommand(sql[i]);
                //da.CommandType = CommandType.StoredProcedure;
                DataSet ds = new DataSet();
                //ds = cs.GetDataSet(da,gridControl1);
                ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, sql[i]));
                gridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void w_fetch_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("员工汇总");//xj/2019/5/29
            GridOper.RestoreGridLayout(gridView1, "员工情况汇总");
            //
           // GridOper.LoadScheme(gridControl1);
            BarMagagerOper.SetBarPropertity(bar3);
            gv = new GridView[] { gridView1, gridView2, gridView3, gridView4, gridView5, gridView6, gridView7 };
            sql[0] = "QSP_GET_YG_ALL_1";
            sql[1] = "QSP_GET_YG_ALL_2";
            sql[2] = "QSP_GET_YG_ALL_3";
            sql[3] = "QSP_GET_YG_ALL_4";
            sql[4] = "QSP_GET_YG_ALL_5";
            sql[5] = "QSP_GET_YG_ALL_6";
            sql[6] = "QSP_GET_YG_ALL_7";
        }
       
        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(gridView1);
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(gridView1, "员工情况汇总");
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(gridView1, "员工情况汇总");
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(gridView1);
        }
                
        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(gridView1,this.Text);
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }
    }
}