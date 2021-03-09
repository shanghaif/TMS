using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using DevExpress.Data;
using DevExpress.XtraEditors.Controls;

namespace ZQTMS.UI
{
    public partial class frmGridViewCol : BaseForm
    {
        string gridViewID = "";

        string url = HttpHelper.UrlPage;//临时记录当前数据库的对应的接口URL，本界面关闭的时候切回去

        public frmGridViewCol()
        {
            InitializeComponent();
        }

        private void SetUrl()
        {
            string db = comboBoxEdit1.Text.Trim();
            DataRow[] rows = CommonClass.GetDatabaseInfo().Select(string.Format("db='{0}'", db));
            if (rows.Length > 0)
            {
                HttpHelper.UrlPage = rows[0]["url"].ToString();
            }
            comboBoxEdit1.Enabled = false;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                SetUrl();

                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_GridViewCol_Main", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                if (ds != null && ds.Tables.Count > 0)
                {
                    gridControl1.DataSource = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            gridViewID = gridView1.GetRowCellValue(rowhandle, "GridViewID").ToString();            
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("GridViewID", gridViewID));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_GridViewCol_Detail", list);
                DataSet ds = SqlHelper.GetDataSet(sps);


                if (ds != null && ds.Tables.Count > 0)
                {
                    gridControl2.DataSource = ds.Tables[0];
                }
                gridView2.ClearColumnsFilter();
                
                SetEditState(false);
                simpleButton3.Visible = true;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            SetUrl();

            frmGridViewCol_View_Add frm = new frmGridViewCol_View_Add();
            frm.Text += string.Format("【{0}】", comboBoxEdit1.Text);
            frm.Show();
        }

        private void frmGridViewCol_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("网格对应");//xj/2019/5/29
            CommonClass.FormSet(this);

            foreach (int code in Enum.GetValues(typeof(UserDB)))
            {
                string strName = Enum.GetName(typeof(UserDB), code);//获取名称
                comboBoxEdit1.Properties.Items.Add(strName);
            }
            if (comboBoxEdit1.Properties.Items.Count > 0)
            {
                comboBoxEdit1.SelectedIndex = 0;
            }

            foreach (int code in Enum.GetValues(typeof(SummaryItemType)))
            {
                string strName = Enum.GetName(typeof(SummaryItemType), code);//获取名称
                repositoryItemImageComboBox1.Items.Add(new ImageComboBoxItem(strName, code, -1));
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            SetEditState(true);
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                gridView2.PostEditor();
                gridView2.UpdateCurrentRow();
                if (gridControl2.DataSource == null)
                {
                    return;
                }
                DataTable dt = gridControl2.DataSource as DataTable;

                DataTable cdt = dt.GetChanges(DataRowState.Modified);
                if (cdt == null || cdt.Rows.Count == 0)
                {
                    MsgBox.ShowOK("没有做任何更改，无需保存!");
                    return;
                }

                List<string> fileds = new List<string>() { "ColGuid", "ColCaption", "AllowEdit", "Visible", "AllowSummary" };//表参数包含的字段（按顺序的）
                for (int i = 0; i < fileds.Count; i++)
                {
                    if (!cdt.Columns.Contains(fileds[i]))
                    {
                        MsgBox.ShowOK("表参数缺少字段：" + fileds[i]);
                        return;
                    }
                    cdt.Columns[fileds[i]].SetOrdinal(i);
                }

                for (int i = cdt.Columns.Count - 1; i >= 0; i--)
                {
                    if (!fileds.Contains(cdt.Columns[i].ColumnName))
                    {
                        cdt.Columns.Remove(cdt.Columns[i]);
                    }
                }
                cdt.AcceptChanges();

                try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("GridViewID", gridViewID));
                    list.Add(new SqlPara("table", cdt));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_UPDATE_GridViewCol_Detail", list);
                    if (SqlHelper.ExecteNonQuery(sps) > 0)
                    {
                        MsgBox.ShowOK();
                        dt.AcceptChanges();

                        SetEditState(false);
                    }
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void SetEditState(bool state)
        {
            gridColumn6.OptionsColumn.AllowEdit = gridColumn6.OptionsColumn.AllowFocus = state;
            gridColumn8.OptionsColumn.AllowEdit = gridColumn8.OptionsColumn.AllowFocus = state;
            gridColumn9.OptionsColumn.AllowEdit = gridColumn9.OptionsColumn.AllowFocus = state;
            gridColumn10.OptionsColumn.AllowEdit = gridColumn10.OptionsColumn.AllowFocus = state;
            simpleButton4.Visible = state;

            gridColumn6.AppearanceCell.BackColor = gridColumn8.AppearanceCell.BackColor = gridColumn9.AppearanceCell.BackColor = gridColumn10.AppearanceCell.BackColor = state ? Color.FromArgb(255, 255, 192) : Color.Empty;

        }

        private void frmGridViewCol_FormClosing(object sender, FormClosingEventArgs e)
        {
            HttpHelper.UrlPage = url;
        }

    }
}
