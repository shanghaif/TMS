using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Lib;
using ZQTMS.SqlDAL;
using System.Reflection;
using ZQTMS.Common;
using ZQTMS.Tool;

namespace ZQTMS.UI
{
    public partial class frmExcelPortPro : BaseForm
    {
        public frmExcelPortPro()
        {
            InitializeComponent();
        }
        public MyGridView myGridView;
        public string gridGuid = "";
        DataTable dtModel = new DataTable();

        private void frmExcelPortPro_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            getTemps();

            dtModel.Columns.Add("tempColumnName", typeof(string));
            dtModel.Columns.Add("ischecked", typeof(int));
            myGridControl3.DataSource = dtModel;

            getColumnName();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            int rowHandle = myGridView1.FocusedRowHandle;
            if (rowHandle < 0)
            {
                MsgBox.ShowOK("请选择一个模板！");
            }
            try
            {
                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("tempGridId", gridGuid));
                list.Add(new SqlPara("tempName", Convert.ToString(myGridView1.GetRowCellValue(rowHandle, "tempName"))));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_delete_gridExcelTemp", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();

                    myGridView1.DeleteRow(rowHandle);
                    myGridView1.PostEditor();
                    myGridView1.UpdateCurrentRow();
                    myGridView1.UpdateSummary();
                    DataTable dt = myGridControl1.DataSource as DataTable;
                    dt.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //根据模板的列打印  其他的全部设置不可见
            if (myGridView1.RowCount == 0)
            {
                MsgBox.ShowOK("当前模板为空！请重新选择！");
            }
            List<string> list = new List<string>();
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                list.Add(Convert.ToString(myGridView2.GetRowCellValue(i, "tempColumnName")));
            }
            string [,] sColumnState = new string [myGridView.Columns.Count+1,1];
            for (int i = 0; i < myGridView.Columns.Count; i++)
            {
                if (myGridView.Columns[i].Visible)
                    sColumnState[i,0] = "1";
                else
                    sColumnState[i,0] = "0";
                if (list.Contains(myGridView.Columns[i].Caption))
                    myGridView.Columns[i].Visible = true;
                else
                    myGridView.Columns[i].Visible = false;
            }
            GridOper.ExportToExcel(myGridView);
            for (int i = 0; i < myGridView.Columns.Count; i++)
            {
                if(sColumnState[i,0] == "1")
                    myGridView.Columns[i].Visible = true;
                else
                    myGridView.Columns[i].Visible = false;
            }
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            if (myGridView == null)
                return;
            if (myGridView4.RowCount == 0)
            {
                MsgBox.ShowOK("请重新选择！");
                return;
            }
            if (tempName.Text.Trim() == "")
            {
                MsgBox.ShowOK("请填写模板名称！");
                return;
            }
            string tempColumnName = "";
            for (int i = 0; i < myGridView4.RowCount; i++)
            {
                if (ConvertType.ToString(myGridView4.GetRowCellValue(i, "ischecked")) == "1")
                    tempColumnName += Convert.ToString(myGridView4.GetRowCellValue(i, "tempColumnName")) + "@";
            }
            if (tempColumnName == "")
            {
                MsgBox.ShowOK("请选择列再保存！");
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("tempGridId", gridGuid));
                list.Add(new SqlPara("tempName", tempName.Text));
                list.Add(new SqlPara("tempColumnName", tempColumnName));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_gridExcelTemp", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    getTemps();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }
        private void getColumnName()
        {
            if (myGridView != null)
            {
                for (int i = 0; i < myGridView.Columns.Count; i++)
                {
                    DataRow dr = dtModel.NewRow();
                    dr["tempColumnName"] = myGridView.Columns[i].Caption;
                    dr["ischecked"] = 1;
                    dtModel.Rows.Add(dr);
                }
            }
        }
        private void getTemps()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("tempGridId", gridGuid));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "qsp_get_gridExcelTemp_tempName", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                {
                    myGridControl2.DataSource = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void myGridView1_Click(object sender, EventArgs e)
        {
            int rowHandle = myGridView1.FocusedRowHandle;
            if (rowHandle < 0)
            {
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("tempGridId", gridGuid));
                list.Add(new SqlPara("tempName", Convert.ToString(myGridView1.GetRowCellValue(rowHandle, "tempName"))));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "qsp_get_gridExcelTemp_tempColumnName", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                {
                    myGridControl1.DataSource = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
    }
}
