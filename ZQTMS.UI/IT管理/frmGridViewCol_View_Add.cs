using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using DevExpress.Data;
using DevExpress.XtraEditors.Controls;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmGridViewCol_View_Add : BaseForm
    {
        public frmGridViewCol_View_Add()
        {
            InitializeComponent();
        }

        DataSet dscols = new DataSet();

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string guid = edGirdViewGuid.Text.Trim();
                string proc = ucLabelBox1.Text.Trim();
                if (guid == "")
                {
                    MsgBox.ShowOK("填写选择Guid!");
                    edGirdViewGuid.Focus();
                    return;
                }
                if (proc.ToLower().Contains("update"))
                {
                    DialogResult dialg = MsgBox.ShowYesNo("当前存储过程用于更新操作，可能没有返回字段。\r\n此操作可能会影响数据，是否确认继续？");
                    if (dialg != DialogResult.Yes)
                    {
                        ucLabelBox1.Focus();
                        return;
                    }
                }
                if (proc.ToLower().Contains("delete"))
                {
                    DialogResult dialg = MsgBox.ShowYesNo("当前存储过程用于删除操作，可能没有返回字段。\r\n此操作可能会影响数据，是否确认继续？");
                    if (dialg != DialogResult.Yes)
                    {
                        ucLabelBox1.Focus();
                        return;
                    }
                }

                SqlParasEntity sps = new SqlParasEntity(OperType.FillSchema, proc);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0)
                {
                    return;
                }
                DataTable dt = new DataTable();
                dt.Columns.Add("ColName", typeof(string)); //字段名  GirdViewRemark应该放在文本框里
                dt.Columns.Add("ColCaption", typeof(string));//中文名
                dt.Columns.Add("ColGuid", typeof(string));  //字段ID
                dt.Columns.Add("AllowEdit", typeof(int));  //是否可编辑 (1是 0否)
                dt.Columns.Add("Visible", typeof(int));  //是否可见 (1是 0否)
                dt.Columns.Add("AllowSummary", typeof(int));  //汇总类型 (0Sum  1Min 2Max 3Count 4Average 5Custom  6 None)

                string ColNames = "", col = "";
                DataRow dr;
                foreach (DataColumn column in ds.Tables[0].Columns)
                {
                    col = column.ColumnName;
                    ColNames += col + "@";

                    dr = dt.NewRow();
                    dr["ColName"] = col;
                    dr["AllowEdit"] = 0;
                    dr["Visible"] = 1;
                    dr["AllowSummary"] = 6;
                    dt.Rows.Add(dr);
                }

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ColNames", ColNames));
                list.Add(new SqlPara("GridViewGuid", guid));

                SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_ColID_ByColName", list);
                dscols = SqlHelper.GetDataSet(sps1);

                if (dscols == null || dscols.Tables.Count == 0)
                {
                    return;
                }
                DataRow[] drs;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    drs = dscols.Tables[0].Select(string.Format("ColName='{0}' and FromProc='{1}'", dt.Rows[i]["ColName"], proc));
                    if (drs.Length != 1)
                    {
                        drs = dscols.Tables[0].Select(string.Format("ColName='{0}'", dt.Rows[i]["ColName"]));
                    }
                    if (drs.Length == 1)
                    {
                        dt.Rows[i]["ColGuid"] = drs[0]["ColGuid"];
                        dt.Rows[i]["ColCaption"] = drs[0]["ColCaption"];
                        dt.Rows[i]["AllowEdit"] = drs[0]["AllowEdit"];
                        dt.Rows[i]["Visible"] = drs[0]["Visible"] == DBNull.Value ? 1 : Convert.ToInt32(drs[0]["Visible"]);
                        dt.Rows[i]["AllowSummary"] = drs[0]["AllowSummary"];
                    }
                }

                dt.AcceptChanges();
                gridControl1.DataSource = dt;

                edgridViewRemark.ReadOnly = false;
                simpleButton2.Enabled = true;

                if (dscols.Tables.Count > 1)
                {
                    string dbname = dscols.Tables[1].Rows[0]["DBName"].ToString();
                    if (!dbname.Equals("ZQTMSLEY"))
                    {
                        checkEdit1.Visible = true;
                    }
                    else
                    {
                        checkEdit1.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            selectColumnName();
        }

        private void selectColumnName()
        {
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle < 0)
            {
                return;
            }
            try
            {
                if (dscols == null || dscols.Tables.Count == 0)
                {
                    return;
                }

                string ColName = gridView1.GetRowCellValue(rowhandle, "ColName").ToString();

                DataSet ds = dscols.Clone();
                DataRow[] drs = dscols.Tables[0].Select(string.Format("ColName='{0}'", ColName));
                foreach (DataRow item in drs)
                {
                    ds.Tables[0].ImportRow(item);
                }
                ds.AcceptChanges();

                frmGridViewCol_View_Select frm = new frmGridViewCol_View_Select();
                frm.ds = ds;
                frm.gv = gridView1;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                gridView1.PostEditor();
                gridView1.UpdateCurrentRow();
                if (gridControl1.DataSource == null)
                {
                    return;
                }
                DataTable dt = gridControl1.DataSource as DataTable;
                dt.AcceptChanges();
                DataRow[] drs = dt.Select("ColGuid is null or ColGuid='' or ColCaption is null or ColCaption=''");
                if (drs.Length > 0)
                {
                    MsgBox.ShowOK("有些字段还没有做对应，请核对应完毕之后再保存!");
                    return;
                }

                string GridViewID = edGirdViewGuid.Text.Trim(); //如果网格能够自动生成Guid，此处去查找对应界面上的对应网格的Guid
                string ColName = "", ColCaption = "", ColGuid = "", AllowEdit = "", Visible = "", AllowSummary = "";
                string FromProc = ucLabelBox1.Text.Trim();
                string GirdViewRemark = edgridViewRemark.Text.Trim();

                if (GridViewID == "")
                {
                    MsgBox.ShowOK("请选择需要匹配的网格(GridView)!");
                    ucLabelBox1.Focus();
                    return;
                }
                if (FromProc == "")
                {
                    MsgBox.ShowOK("请填写存储过程名称!");
                    ucLabelBox1.Focus();
                    return;
                }
                if (GirdViewRemark == "")
                {
                    MsgBox.ShowOK("请填写用途!");
                    edgridViewRemark.Focus();
                    return;
                }

                if (checkEdit1.Checked)
                {
                    if (MsgBox.ShowYesNo("勾选了同步正式库，正式库该网格将会同步，是否继续？") != DialogResult.Yes)
                    {
                        return;
                    }
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ColName += dt.Rows[i]["ColName"] + "@";
                    ColCaption += dt.Rows[i]["ColCaption"] + "@";
                    ColGuid += dt.Rows[i]["ColGuid"] + "@";
                    AllowEdit += dt.Rows[i]["AllowEdit"] + "@";
                    Visible += dt.Rows[i]["Visible"] + "@";
                    AllowSummary += dt.Rows[i]["AllowSummary"] + "@";
                }

                try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("GridViewID", GridViewID));
                    list.Add(new SqlPara("ColName", ColName));
                    list.Add(new SqlPara("ColGuid", ColGuid));
                    list.Add(new SqlPara("ColCaption", ColCaption));
                    list.Add(new SqlPara("AllowEdit", AllowEdit));
                    list.Add(new SqlPara("Visible", Visible));
                    list.Add(new SqlPara("AllowSummary", AllowSummary));
                    list.Add(new SqlPara("FromProc", FromProc));
                    list.Add(new SqlPara("GirdViewRemark", GirdViewRemark));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_Add_GridViewCol_All", list);
                    if (SqlHelper.ExecteNonQuery(sps) > 0)
                    {
                        if (!checkEdit1.Checked)
                        {
                            MsgBox.ShowOK();
                        }
                        else
                        {
                            List<SqlPara> list2 = new List<SqlPara>();
                            list2.Add(new SqlPara("GridViewID", GridViewID));
                            list2.Add(new SqlPara("ColName", ColName));
                            list2.Add(new SqlPara("ColGuid", ColGuid));
                            list2.Add(new SqlPara("ColCaption", ColCaption));
                            list2.Add(new SqlPara("AllowEdit", AllowEdit));
                            list2.Add(new SqlPara("Visible", Visible));
                            list2.Add(new SqlPara("AllowSummary", AllowSummary));
                            list2.Add(new SqlPara("FromProc", FromProc));
                            list2.Add(new SqlPara("GirdViewRemark", GirdViewRemark));

                            SqlParasEntity sps2 = new SqlParasEntity(OperType.Execute, "USP_Add_GridViewCol_All", list2);
                            if (SqlHelper.ExecteNonQuery(sps2,1, HttpHelper.domaimZS) > 0)
                            {
                                MsgBox.ShowOK();
                            }
                        }
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

        private void frmGridViewCol_View_Add_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            foreach (int code in Enum.GetValues(typeof(SummaryItemType)))
            {
                string strName = Enum.GetName(typeof(SummaryItemType), code);//获取名称
                repositoryItemImageComboBox1.Items.Add(new ImageComboBoxItem(strName, code, -1));
            }
        }

        private void buttonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            frmGridViewCol_SelectGuid frm = new frmGridViewCol_SelectGuid();
            DialogResult dlg = frm.ShowDialog();
            if (dlg == DialogResult.Yes)
            {
                edGirdViewGuid.Text = frm.guid;
                edgridViewRemark.Text = frm.gridViewRemark;
            }
        }

        private void tsmiDeleteRow_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            if (MsgBox.ShowYesNo("确定删除选中项？") != DialogResult.Yes) return;
            if (MsgBox.ShowYesNo("确定继续,请三思？") != DialogResult.Yes) return;
            gridView1.DeleteSelectedRows();
        }

        private void gridControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) selectColumnName();
        }

        private void edGirdViewGuid_EditValueChanged(object sender, EventArgs e)
        {
            string guid = (sender as ButtonEdit).Text.Trim();
            if (guid == "") return;
            GuidConverter convert = new GuidConverter();
            if (!convert.IsValid(guid)) return;

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("GridViewGuid", guid));

                SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_GridViewProc_ByGuid", list);
                dscols = SqlHelper.GetDataSet(sps1);

                if (dscols == null || dscols.Tables.Count == 0 || dscols.Tables[0].Rows.Count == 0)
                {
                    return;
                }

                ucLabelBox1.Text = dscols.Tables[0].Rows[0]["FromProc"] == DBNull.Value ? "" : dscols.Tables[0].Rows[0]["FromProc"].ToString();

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName != "AllowSummary") return;
            string field = e.Column.FieldName;
            string value = gridView1.GetRowCellValue(e.RowHandle, field).ToString();
            try
            {
                gridView1.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(gridView1_CellValueChanged);
                for (int i = e.RowHandle + 1; i < gridView1.RowCount; i++)
                {
                    if (gridView1.GetRowCellValue(i, field) != DBNull.Value) continue;
                    gridView1.SetRowCellValue(i, field, value);
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                gridView1.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(gridView1_CellValueChanged);
            }
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit1.Checked)
            {
                MsgBox.ShowOK("勾选了同步正式库，正式库该网格将会同步!!!!!!");
            }
        }
    }
}