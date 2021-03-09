using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class frmSubjectCorr : BaseForm
    {

        private DataSet dsxm = new DataSet();
        private DataSet ds = new DataSet();
        public frmSubjectCorr()
        {
            InitializeComponent();
        }
        private void getparent()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_KM", list);
                dsxm = SqlHelper.GetDataSet(sps);
                if (dsxm == null || dsxm.Tables.Count == 0) return;

                repositoryItemComboBox1.Items.Clear();
                if (dsxm.Tables.Count > 0)
                {
                    for (int i = 0; i < dsxm.Tables[0].Rows.Count; i++)
                    {
                        if (Convert.ToInt32(dsxm.Tables[0].Rows[i]["thelevel"]) == 0)  //一级科目
                        {
                            repositoryItemComboBox1.Items.Add(dsxm.Tables[0].Rows[i]["kmmc"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }
       
        private int getkmid(string kmmc)
        {
            if (dsxm == null || dsxm.Tables.Count == 0) return -1;
            for (int i = 0; i < dsxm.Tables[0].Rows.Count; i++)
            {
                if (dsxm.Tables[0].Rows[i]["kmmc"].ToString().Trim() == kmmc.Trim())
                {
                    return Convert.ToInt32(dsxm.Tables[0].Rows[i]["ID"]);
                }
            }
            return -1;
        }

        private void getchild(int parentid, DevExpress.XtraEditors.Repository.RepositoryItemComboBox cbe)
        {
            cbe.Items.Clear();
            if (dsxm == null || dsxm.Tables.Count == 0) return;
            for (int i = 0; i < dsxm.Tables[0].Rows.Count; i++)
            {
                if (Convert.ToInt32(dsxm.Tables[0].Rows[i]["parentid"]) == parentid)
                {
                    cbe.Items.Add(dsxm.Tables[0].Rows[i]["kmmc"]);
                }
            }
        }

        private void getchildchild(int parentid, DevExpress.XtraEditors.Repository.RepositoryItemComboBox cbe)
        {
            cbe.Items.Clear();
            if (dsxm == null || dsxm.Tables.Count == 0) return;
            for (int i = 0; i < dsxm.Tables[0].Rows.Count; i++)
            {
                if (parentid == Convert.ToInt32(dsxm.Tables[0].Rows[i]["parentid"]))
                {
                    cbe.Items.Add(dsxm.Tables[0].Rows[i]["kmmc"]);
                }
            }
        }

        private int getkmid2(string parentkmid, string kmmc)
        {
            string lkmmc = "";
            string childkmid = "";
            if (dsxm == null || dsxm.Tables.Count == 0) return -1;
            for (int i = 0; i < dsxm.Tables[0].Rows.Count; i++)
            {
                lkmmc = dsxm.Tables[0].Rows[i]["kmmc"].ToString().Trim();
                if (lkmmc == kmmc)
                {
                    childkmid = dsxm.Tables[0].Rows[i]["ID"].ToString();
                    if (childkmid.IndexOf(parentkmid) >= 0) return int.Parse(childkmid);
                }
            }
            return -1;
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName.ToString() == "km1")
            {
                getchild(getkmid(e.Value.ToString().Trim()), repositoryItemComboBox2);
                gridView1.SetRowCellValue(e.RowHandle, "km2", DBNull.Value);
                gridView1.SetRowCellValue(e.RowHandle, "km3", DBNull.Value);
                gridView1.SetRowCellValue(e.RowHandle, "km4", DBNull.Value);

            }
            if (e.Column.FieldName.ToString() == "km2")
            {
                string km1 = gridView1.GetRowCellValue(e.RowHandle, "km1").ToString();
                string km2 = gridView1.GetRowCellValue(e.RowHandle, "km2").ToString();
                getchildchild(getkmid2(getkmid(km1).ToString(), km2), repositoryItemComboBox3);
                gridView1.SetRowCellValue(e.RowHandle, "km3", DBNull.Value);
                gridView1.SetRowCellValue(e.RowHandle, "km4", DBNull.Value);
            }

            if (e.Column.FieldName.ToString() == "km3")
            {
                gridView1.SetRowCellValue(e.RowHandle, "km4", DBNull.Value);
            }
        }
        
        private void getdefine()
        {
            List<SqlPara> list = new List<SqlPara>();
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SubjectCorr", list);
            ds = SqlHelper.GetDataSet(sps);
            if (ds == null || ds.Tables.Count < 1)
                return;
            
            string[] xm ={VerifyType.大车费.ToString(),
                          VerifyType.短驳费.ToString(),
                          VerifyType.货款回收.ToString(),
                          VerifyType.货款汇款.ToString(),
                          VerifyType.派车费.ToString(),
                          VerifyType.始发其他费.ToString(),
                          VerifyType.送货费.ToString(),
                          VerifyType.折扣费.ToString(),
                          VerifyType.中转费.ToString(),
                          VerifyType.终端其他费.ToString(),
                          VerifyType.转送费.ToString(),
                          VerifyType.现付.ToString(), /*hj 20180115*/
                          VerifyType.提付.ToString(),/*hj 20180115*/
                          VerifyType.月结.ToString(),/*hj 20180115*/
                          VerifyType.货到前付.ToString(),/*hj 20180115*/
                          VerifyType.回单付.ToString(), /*hj 20180115*/
                          VerifyType.欠付.ToString(),  /*hj 20180115*/
                          VerifyType.提付异动.ToString(),  //maohui20180131
                          VerifyType.非提付异动.ToString()
                         };
            for (int i = 0; i < xm.Length; i++)
            {
                int a = 0;
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    if (xm[i].Trim() == ds.Tables[0].Rows[j]["xm"].ToString())
                    {
                        a++;
                        break;
                    }
                }
                if (a == 0)
                {
                    DataRow dr = ds.Tables[0].NewRow();
                    dr["xm"] = xm[i];
                    dr["km1"] = "";
                    dr["km2"] = "";
                    dr["km3"] = "";
                    dr["km4"] = "";
                    ds.Tables[0].Rows.Add(dr);
                }
            }
            gridControl1.DataSource = ds.Tables[0];


            if (gridView1.RowCount == 0)
            {
                for (int i = 0; i < xm.Length; i++)
                {
                    gridView1.AddNewRow();
                }

                for (int i = 0; i < xm.Length; i++)
                {
                    gridView1.SetRowCellValue(i, "xm", xm[i]);
                }
                gridView1.PostEditor();
            }
        }

        private void w_km_for_hexiao_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("科目对应");//xj/2019/5/28
            getparent();
            getdefine();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            gridView1.AddNewRow();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                int row = e.FocusedRowHandle;
                string km1 = gridView1.GetRowCellValue(row, "km1").ToString();
                string km2 = gridView1.GetRowCellValue(row, "km2").ToString();
                getchildchild(getkmid2(getkmid(km1).ToString(), km2), repositoryItemComboBox3);
                getchild(getkmid(km1), repositoryItemComboBox2);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void save()
        {
            try
            {
                string xm = "", km1 = "", km2 = "", km3 = "", km4 = "";
                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    xm += gridView1.GetRowCellValue(i, "xm").ToString() + "@";
                    km1 += gridView1.GetRowCellValue(i, "km1").ToString() + "@";
                    km2 += gridView1.GetRowCellValue(i, "km2").ToString() + "@";
                    km3 += gridView1.GetRowCellValue(i, "km3").ToString() + "@";
                    km4 += gridView1.GetRowCellValue(i, "km4").ToString() + "@";
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("xm", xm));
                list.Add(new SqlPara("km1", km1));
                list.Add(new SqlPara("km2", km2));
                list.Add(new SqlPara("km3", km3));
                list.Add(new SqlPara("km4", km4));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_SubjectCorr", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            gridView1.PostEditor();
            save();
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            gridControl1.DataSource = null; 
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_KM_FOR_HEXIAO");
                DataSet ds = SqlHelper.GetDataSet(sps);
                gridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
             
        }
    }
}
