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
using DevExpress.XtraBars.Alerter;
using System.Threading;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class w_ְλ�������� : BaseForm
    {
        DataSet ds = new DataSet();
        //string exception = "";


        public w_ְλ��������()
        {
            InitializeComponent();
        }

        private void getdata()
        {
            //SqlConnection connection = cc.GetConn();
            try
            {
                //SqlDataAdapter da = new SqlDataAdapter();
                //SqlCommand sq = new SqlCommand("QSP_GET_B_ZW");
                //sq.CommandType = CommandType.StoredProcedure;
                //sq.Parameters.Add("@bsiteName ", SqlDbType.VarChar).Value = commonclass.username;
                ////da.SelectCommand = sq;
                ////ds.Clear();
                ////da.Fill(ds);
                ////gridControl1.DataSource = ds.Tables[0];
                //ds = cs.GetDataSet(sq, gridControl1);
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bsiteName",CommonClass.UserInfo.UserName));
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_B_ZW", list));
                if (ds == null || ds.Tables.Count == 0) return;
                gridControl1.DataSource = ds.Tables[0];

                
                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "ϵͳ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //if (connection.State == ConnectionState.Open) connection.Close();
            }
        }

        //userright ur = new userright();

        private void w_cygs_Load(object sender, EventArgs e)
        {
            //cc.LoadScheme(gridControl1);
            CommonClass.InsertLog("ְλ�������");//xj/2019/5/29
            BarMagagerOper.SetBarPropertity(bar3);
            //cc.RestoreGridLayout(gridControl1, "ְλ��������");
            getdata();


            //barButtonItem12.Enabled = ur.GetUserRightDetail999("d101101");
            //barButtonItem4.Enabled = ur.GetUserRightDetail999("d101102");

        }

        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
           // GridOper.GenSeq(e);
        }

        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(gridView1);
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(gridView1, "ְλ��������");
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(gridView1, "ְλ��������");
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(gridView1);
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {

            w_������Ŀ���� ww = new w_������Ŀ����();
            ww.ShowDialog();

        }


        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            getdata();
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(gridView1);
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            //cc.QuickSearch();
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rows = gridView1.FocusedRowHandle;
            if (rows < 0) return;

            string zw = gridView1.GetRowCellValue(rows,"zw").ToString();
            w_ְλ��������_add ww = new w_ְλ��������_add();
            ww.zw = zw;
            ww.ShowDialog();

        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rows = gridView1.FocusedRowHandle;
            if (rows < 0) return;

            string zw = gridView1.GetRowCellValue(rows, "zw").ToString();
            w_����ְ������_add ww = new w_����ְ������_add();
            ww.zw = zw;
            ww.ShowDialog();
        }

        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            w_ְ����� ww = new w_ְ�����();
            ww.ShowDialog();
        }
    }
}