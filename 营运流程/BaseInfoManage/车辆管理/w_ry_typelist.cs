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
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class w_ry_typelist : BaseForm
    {
        private bool isclick = false;
        public w_ry_typelist()
        {
            InitializeComponent();
        }

        private void w_ry_typelist_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            BarMagagerOper.SetBarPropertity(bar2);
            select();
        }
        private void select()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_RYTYPE_LIST", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                this.gridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void saveexit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridView1.SelectedRowsCount == 0)
            {
                return;
            }
            delete();
        }

        private void delete()
        {
            if (XtraMessageBox.Show("确定要删除吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                int no = this.gridView1.FocusedRowHandle;
                string name = this.gridView1.GetRowCellValue(no, "typename").ToString();
                try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("typename", name));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "V_DELETE_RYTYPE", list);
                    if (SqlHelper.ExecteNonQuery(sps) > 0)
                    {
                        MsgBox.ShowOK();
                    }
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
            }


        }

        private void saveadd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.gridView1.AddNewRow();
            isclick = true;
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            save();
        }

        private void save()
        {
            if (isclick)
            {
                try
                {
                    this.gridView1.PostEditor();
                    this.gridView1.UpdateCurrentRow();
                    List<SqlPara> list = new List<SqlPara>();
                    List<SqlPara> list2 = new List<SqlPara>();
                    List<SqlPara> list3 = new List<SqlPara>();
                    list.Add(new SqlPara("typename", ""));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "V_DELETE_RYTYPE", list);
                    for (int i = 0; i < this.gridView1.RowCount; i++)
                    {
                        string typename = this.gridView1.GetRowCellValue(i, "typename").ToString();
                        string price = this.gridView1.GetRowCellValue(i, "price").ToString();
                        list2.Add(new SqlPara("typename", typename));
                        list2.Add(new SqlPara("price", price));
                        if (typename == "")
                        {
                            continue;
                        }
                        list3.Add(new SqlPara("projectname", typename));
                        if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "V_CHECK_RYNAME", list3)) > 0)
                        {
                            continue;
                        }
                        SqlParasEntity sps2 = new SqlParasEntity(OperType.Execute, "V_ADD_RYTYPE", list2);
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