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
using DevExpress.XtraEditors;

namespace ZQTMS.UI.BaseInfoManage
{
    public partial class frmHiddenFieldConfig : BaseForm
    {
        public frmHiddenFieldConfig()
        {
            InitializeComponent();
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
              frmHiddenFieldConfigAdd add = new frmHiddenFieldConfigAdd();
              add.ShowDialog();
              GetData();

        }

        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmHiddenFieldConfigAdd edit = new frmHiddenFieldConfigAdd();
            int rowHandle = myGridView1.FocusedRowHandle;
            if (rowHandle < 0) return;
            DataTable dt=myGridControl1.DataSource as DataTable;
            DataRow dr = dt.Rows[rowHandle];
            edit.operType = 1;
            edit.dr = dr;
            edit.ShowDialog();
            GetData();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("WebName", WebName.Text.Trim() == "全部" ? "%%" : WebName.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_HiddenFieldConfig", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                //if (ds == null || ds.Tables[0].Rows.Count <= 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void frmLinesConfiguration_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("快找隐藏字段配置");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            GetCompanys();
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
            GetAllWebId();
            GetData();
        }
        private void GetCompanys()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_COMPANYS", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables[0].Rows.Count <= 0) return;
                WebName.Properties.Items.Clear();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    WebName.Properties.Items.Add(ds.Tables[0].Rows[i]["gsqc"]);
                }
                WebName.Properties.Items.Add("全部");


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

                string ID = myGridView1.GetRowCellValue(rowHandle, "ID").ToString();
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID",ID));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DEL_HiddenFieldConfig", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    myGridView1.DeleteRow(rowHandle);
                    MsgBox.ShowOK();
                }
                GetData();

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

        public void GetAllWebId()
        {
            try
            {
                if (CommonClass.dsWeb.Tables.Count == 0) return;
                WebName.Properties.Items.Clear();
                for (int i = 0; i < CommonClass.dsWeb.Tables[0].Rows.Count; i++)
                {
                    WebName.Properties.Items.Add(CommonClass.dsWeb.Tables[0].Rows[i]["WebName"]);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
