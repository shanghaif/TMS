using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class w_fy_project_list : BaseForm
    {

        bool isclick = false;
        public w_fy_project_list()
        {
            InitializeComponent();
        }

        private void w_fy_project_list_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            BarMagagerOper.SetBarPropertity(bar2);
            getdata();
        }

        private void getdata()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_FY_PROJECT", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                this.gridControl1.DataSource = ds.Tables[0];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    isclick = true;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void saveexit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.gridView1.SelectedRowsCount == 0)
            {
                return;
            }
            delete();
        }
        private void delete()
        {
            if (MessageBox.Show("确定要删除吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                int no = this.gridView1.FocusedRowHandle;
                string projectname = this.gridView1.GetRowCellValue(no, "projectname").ToString();

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("projectname", projectname));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "V_DELETE_FY_PROJECT", list);
                MsgBox.ShowOK();

                gridView1.DeleteRow(no);
                gridView1.PostEditor();
                gridView1.UpdateCurrentRow();
                gridView1.UpdateSummary();
                DataTable dt = gridControl1.DataSource as DataTable;
                dt.AcceptChanges();
            }
        }

        private void saveadd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.gridView1.AddNewRow();
            isclick = true;
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            saveproject();
        }

        private void saveproject()
        {
            if (isclick)
            {


                try
                {
                    this.gridView1.PostEditor();
                    this.gridView1.UpdateCurrentRow();
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("projectname", ""));
                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "V_DELETE_FY_PROJECT", list);
                    SqlHelper.ExecteNonQuery(sps);
                    for (int i = 0; i < this.gridView1.RowCount; i++)
                    {
                        string projectname = this.gridView1.GetRowCellValue(i, "projectname").ToString();
                        string type = this.gridView1.GetRowCellValue(i, "type").ToString();
                        List<SqlPara> list2 = new List<SqlPara>();
                        List<SqlPara> list3 = new List<SqlPara>();
                        list2.Add(new SqlPara("projectname", projectname));
                        list2.Add(new SqlPara("type", type));
                        if (projectname == "")
                        {
                            continue;
                        }
                        list3.Add(new SqlPara("projectname", projectname));
                        if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "V_CHECK_FY_PROJECT", list3)) > 0)
                        {
                            continue;
                        }
                        SqlParasEntity sps2 = new SqlParasEntity(OperType.Execute, "V_ADD_FY_PROJECT", list2);
                        SqlHelper.ExecteNonQuery(sps2);
                    }
                    isclick = false;
                    MsgBox.ShowOK();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void exit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
    }
}