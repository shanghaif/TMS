using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI.BaseInfoManage
{
    public partial class frmLinesConfiguration : BaseForm
    {
        public frmLinesConfiguration()
        {
            InitializeComponent();
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmLinesConfigurationAdd add = new frmLinesConfigurationAdd();
            add.ShowDialog();

        }

        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmLinesConfigurationAdd edit = new frmLinesConfigurationAdd();
            int rowHandle = myGridView1.FocusedRowHandle;
            if (rowHandle < 0) return;
            DataTable dt=myGridControl1.DataSource as DataTable;
            DataRow dr = dt.Rows[rowHandle];
            edit.operType = 1;
            edit.dr = dr;
            edit.ShowDialog();

        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("companyName", txtCompanyname.Text.Trim() == "全部" ? "%%" : txtCompanyname.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_LINESCONFIGURATION", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                //if (ds == null || ds.Tables[0].Rows.Count <= 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch(Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void frmLinesConfiguration_Load(object sender, EventArgs e)
        {

            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            GetCompanys();
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
        }
        private void GetCompanys()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_COMPANYS", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables[0].Rows.Count <= 0) return;
                txtCompanyname.Properties.Items.Clear();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    txtCompanyname.Properties.Items.Add(ds.Tables[0].Rows[i]["gsqc"]);
                }
                txtCompanyname.Properties.Items.Add("全部");


            }
            catch (Exception ex)
            { MsgBox.ShowException(ex); }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowHandle = myGridView1.FocusedRowHandle;
                if (rowHandle < 0) return;
               
                string mainCompanyid = myGridView1.GetRowCellValue(rowHandle, "MainCompanyid").ToString();
                List<SqlPara> list = new List<SqlPara>();
                if (mainCompanyid.Trim() == "101")
                {
                    MsgBox.ShowOK("公司101为基础配置不能删除!");
                    return;
                }
                list.Add(new SqlPara("mainCompanyid",mainCompanyid));
                
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DEL_LINESCONFIGURATION", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    myGridView1.DeleteRow(rowHandle);
                    MsgBox.ShowOK();
                }

            }
            catch(Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }





     
    }
}
