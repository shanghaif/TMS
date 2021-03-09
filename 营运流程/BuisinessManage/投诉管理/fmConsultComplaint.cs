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
using ZQTMS.Common;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using DevExpress.XtraGrid;

namespace ZQTMS.UI
{
    public partial class fmConsultComplaint : BaseForm
    {
        public fmConsultComplaint()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void w_ask_query_count_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("客户投诉");//xj/2019/5/28
            dateEdit1.EditValue = CommonClass.gbdate;
            dateEdit2.EditValue = CommonClass.gedate;
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例

            GridOper.CreateStyleFormatCondition(myGridView1, "QResult", FormatConditionEnum.Equal, "已处理", Color.Lime);//执行状态
        }

        private void getdata()
        {
            if (dateEdit1.DateTime.Date > dateEdit1.DateTime.Date)
            {
                XtraMessageBox.Show("开始日期不能大于结束日期", "日期选择错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", dateEdit1.DateTime));
                list.Add(new SqlPara("t2", dateEdit2.DateTime));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLCUSTQUERRYLOG", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                if (myGridView1.RowCount < 1000) myGridView1.BestFitColumns();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            frmCustSearchLog_Add add = new frmCustSearchLog_Add();
            add.ShowDialog();
        }

        private void barButtonItem15_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView1.PostEditor();
            try
            {
                if (myGridControl1.DataSource == null) return;
                DataTable dt = myGridControl1.DataSource as DataTable;
                DataRow[] drs = dt.Select("ischecked=1 and QResult='处理中'");
                if (drs.Length == 0)
                {
                    MsgBox.ShowOK("没有勾选本次需要处理的查询记录!");
                    return;
                }
                if (MsgBox.ShowYesNo("确定要将已勾选的记录置为“已处理”状态吗？") == DialogResult.No) return;

                string ids = "";
                for (int i = 0; i < drs.Length; i++)
                {
                    ids += drs[i]["Qid"].ToString() + "@";
                }

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ids", ids));
                list.Add(new SqlPara("man", CommonClass.UserInfo.UserName));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_MODIFY_BILLCUSTQUERRYLOG_ByIDs", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    for (int i = 0; i < drs.Length; i++)
                    {
                        drs[i]["QResult"] = "已处理";
                    }
                    dt.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (myGridControl1.DataSource == null) return;
            DataTable dt = myGridControl1.DataSource as DataTable;
            int check = checkEdit1.Checked ? 1 : 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["ischecked"] = check;
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            int rows = myGridView1.FocusedRowHandle;
            if (rows < 0) return;

            fmComplaintTrack fmCt = new fmComplaintTrack();
            fmCt.gv = myGridView1;
            fmCt.ShowDialog();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "客户查询统计");
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barCheckItem4_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }
    }
}