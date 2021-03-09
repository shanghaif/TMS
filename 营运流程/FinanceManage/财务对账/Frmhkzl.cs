using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using KMS.Tool;
using KMS.Common;
using KMS.SqlDAL;
using KMS.UI;

namespace KMS.UI
{
    public partial class Frmhkzl : BaseForm
    {
        public Frmhkzl()
        {
            InitializeComponent();
        }

        public void Frmhkzl_Load(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_hkzl", list);
                DataSet ds = SqlHelper.GetDataSet(spe);
                myGridControl1.DataSource = ds.Tables[0];
                CommonClass.FormSet(this);
                CommonClass.GetGridViewColumns(myGridView1);
                GridOper.SetGridViewProperty(myGridView1);

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        //修改
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0)
            {
                MsgBox.ShowOK("请选择一条信息！");
                return;
            }
            string hm = myGridView1.GetRowCellValue(rowhandle, "hm").ToString();
            string zh = myGridView1.GetRowCellValue(rowhandle, "zh").ToString();
            string khh = myGridView1.GetRowCellValue(rowhandle, "khh").ToString();
            string szs = myGridView1.GetRowCellValue(rowhandle, "szs").ToString();
            string szshi = myGridView1.GetRowCellValue(rowhandle, "szshi").ToString();
            string ID = myGridView1.GetRowCellValue(rowhandle, "ID").ToString();
            FrmhkzlADD frm = new FrmhkzlADD();
            frm.ID = ID;
            frm.hm = hm;
            frm.zh = zh;
            frm.khh = khh;
            frm.szs = szs;
            frm.szshi = szshi;
            frm.ShowDialog();
            List<SqlPara> list = new List<SqlPara>();
            SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_hkzl", list);
            DataSet ds = SqlHelper.GetDataSet(spe);
            myGridControl1.DataSource = ds.Tables[0];
        }
        //新增
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmhkzlADD frm = new FrmhkzlADD();
            frm.ShowDialog();
            List<SqlPara> list = new List<SqlPara>();
            SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_hkzl", list);
            DataSet ds = SqlHelper.GetDataSet(spe);
            myGridControl1.DataSource = ds.Tables[0];
        }
      
         //删除
        private void barButtonItem3_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0)
            {
                MsgBox.ShowOK("请选择一条信息！");
                return;
            }
            string ID = myGridView1.GetRowCellValue(rowhandle, "ID").ToString();
            try
            {

                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", ID));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_DEL_hkzl", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_hkzl", list);
                DataSet ds = SqlHelper.GetDataSet(spe);
                myGridControl1.DataSource = ds.Tables[0];

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }
        //刷新
        private void barButtonItem4_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_hkzl", list);
                DataSet ds = SqlHelper.GetDataSet(spe);
                myGridControl1.DataSource = ds.Tables[0];

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
    }
}
