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
    public partial class w_yg_tsl : BaseForm
    {
        DataSet ds = new DataSet();


        public w_yg_tsl()
        {
            InitializeComponent();
        }

        private void getdata()
        {
            
            try
            {

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_YGDA_TOTAL");
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                gridControl1.DataSource = ds.Tables[0];

            }
            catch (Exception ex)
            {
                 
                XtraMessageBox.Show(ex.Message, "ϵͳ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void w_cygs_Load(object sender, EventArgs e)
        {
            //fixcolumn fc = new fixcolumn(gridView1, barSubItem2);//������
            //cc.LoadScheme(gridControl1);//�������
            //commonclass.SetBarPropertity(bar3);//����
            //cc.RestoreGridLayout(gridControl1, "Ա��ͨѶ¼");
            CommonClass.InsertLog("Ա��ͨ��¼");//xj/2019/5/29
            getdata();
        }

        private void modify()
        {
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle >= 0)
            {
                w_yg_add wa = new w_yg_add();
                wa.bh = gridView1.GetRowCellValue(rowhandle, "bh") == DBNull.Value ? "" : gridView1.GetRowCellValue(rowhandle, "bh").ToString();
                wa.ShowDialog();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(gridView1);
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            //cc.SaveGridLayout(gridControl1, "Ա��ͨѶ¼", true);
            GridOper.SaveGridLayout(gridView1,this.Text);
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
           // cc.ShowAutoFilterRow(gridView1);
            GridOper.ShowAutoFilterRow(gridView1);
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            //cc.DeleteGridLayout(gridControl1, "Ա��ͨѶ¼");
            GridOper.DeleteGridLayout(gridView1,this.Text);
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            getdata();
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
           // cc.ExportToExcel(gridView1);
            GridOper.ExportToExcel(gridView1,this.Text);
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            //cc.QuickSearch();
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }
    }
}