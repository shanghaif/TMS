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
    public partial class w_fkpz : BaseForm
    {
        //commonclass cc = new commonclass();Cls_SqlHelper cs = new Cls_SqlHelper();


        public w_fkpz()
        {
            InitializeComponent();
        }

        private void getdata()
        {
            
            try
            {
                 
                //DataSet ds = new DataSet();
                //SqlCommand sq = new SqlCommand("QSP_GET_HONGHAO_FKPZ");
                //sq.CommandType = CommandType.StoredProcedure;
                //sq.Parameters.Add(new SqlParameter("@t1", SqlDbType.DateTime));
                //sq.Parameters.Add(new SqlParameter("@t2", SqlDbType.DateTime));

                //sq.Parameters[0].Value = bdate.DateTime;
                //sq.Parameters[1].Value = edate.DateTime;

                
                //ds.Clear();
                //ds = cs.GetDataSet(sq,gridControl2);
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1",bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_HONGHAO_FKPZ", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count > 0 || ds.Tables[0].Rows.Count == 0) return;
                gridControl2.DataSource = ds.Tables[0];
         
            }
            catch (Exception ex)
            {
                 
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void w_cygs_Load(object sender, EventArgs e)
        {
            //cc.RestoreGridLayout(gridControl2, "付款凭证");
            CommonClass.FormSet(this);
            GridOper.RestoreGridLayout(gridView2,this.Text);
            bdate.EditValue = CommonClass.gbdate;
            edate.EditValue = CommonClass.gedate;
            BarMagagerOper.SetBarPropertity(bar3); 
            //commonclass.SetBarPropertity(bar3);//工具
        }

        private void modify()
        {
            int rowhandle = gridView2.FocusedRowHandle;
            if (rowhandle >= 0)
            {
                w_fkpz_add wa = new w_fkpz_add();
                wa.bh = gridView2.GetRowCellValue(rowhandle, "unit") == DBNull.Value ? "" : gridView2.GetRowCellValue(rowhandle, "unit").ToString();
                wa.oper = "MODIFY";
                wa.ShowDialog();
            }
        }

        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            modify();
        }

        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(gridView2);
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(gridView2, this.Text);
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(gridView2);
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(gridView2, this.Text);
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {

            w_fkpz_add wa = new w_fkpz_add();
            wa.bh = "";
            wa.oper = "NEW";
            wa.ShowDialog();

        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            modify();
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rowhandle = gridView2.FocusedRowHandle;
            if (rowhandle >= 0)
            {
                string unit = gridView2.GetRowCellValue(rowhandle, "unit").ToString();
                if (XtraMessageBox.Show("确认要删除该笔记录吗?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {

                    //SqlCommand sq = new SqlCommand("USP_DELETE_HONGHAO");
                    //sq.CommandType = CommandType.StoredProcedure;
                    //sq.Parameters.Add(new SqlParameter("@unit", SqlDbType.VarChar));
                    //sq.Parameters[0].Value = unit;
                    //cs.ENQ(sq);


                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("unit", unit));
                    SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_DELETE_HONGHAO", list));
                     

                    gridView2.DeleteRow(rowhandle);
                }
            }
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            getdata();
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(gridView2,this.Text);
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            //cc.QuickSearch();
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }

}