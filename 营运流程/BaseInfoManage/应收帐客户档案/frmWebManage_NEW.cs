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
using System.Threading;

namespace ZQTMS.UI
{
    public partial class frmWebManage_NEW : BaseForm
    {
        /// <summary>
        /// 开启短欠的行
        /// </summary>
        int rowhandle = -1; 

        public frmWebManage_NEW()
        {
            InitializeComponent();
        }

        private void frmUsers_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar11); //如果有具体的工具条，就引用其实例
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            GridOper.RestoreGridLayout(myGridView1);
            GridOper.CreateStyleFormatCondition(myGridView1, "IsRestart", DevExpress.XtraGrid.FormatConditionEnum.Equal, 1, Color.FromArgb(255, 255, 128));
            CommonClass.SetCause(CauseName, true);
            CauseName.EditValue = CommonClass.UserInfo.CauseName;
            freshData();
        }

        private void barBtnUserAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmWebAdd frm = new frmWebAdd();
            frm.ShowDialog();
            freshData();
        }

        private void barBtnUserMod_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (panel1.Visible)
            {
                MsgBox.ShowOK("正在加载数据...\r\n请稍后再试");
                return;
            }
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "WebId").ToString());

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("WebId", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASWEB_ByID", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                DataRow dr = ds.Tables[0].Rows[0];

                frmWebAdd frm = new frmWebAdd();
                frm.dr = dr;
                frm.ShowDialog();
                freshData();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barBtnUserFilter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barBtnUserDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "WebId").ToString());

                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("WebId", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_BASWEB", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();

                    myGridView1.DeleteRow(rowhandle);
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

        private void barBtnUserFresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            freshData();
        }

        private void barBtnvUserExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "网点信息");
        }

        private void barBtnUserExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
        private void freshData()
        {
            panel1.Visible = true;
            Thread th = new Thread(() =>
            {
                try
                {
                    List<SqlPara> list = new List<SqlPara>();

                    list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text.Trim()));
                    list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text.Trim()));
                    list.Add(new SqlPara("WebName", WebName.Text.Trim() == "全部" ? "%%" : WebName.Text.Trim()));
                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASWEB_CONDITION", list);
                    DataSet ds = SqlHelper.GetDataSet(sps);

                    if (ds == null || ds.Tables.Count == 0) return;
                    if (!this.IsHandleCreated) return;
                    this.Invoke((MethodInvoker)delegate
                    {
                        myGridControl1.DataSource = ds.Tables[0];
                    });
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
                finally
                {
                    if (this.IsHandleCreated)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            panel1.Visible = false;
                            if (myGridView1.RowCount < 1000) myGridView1.BestFitColumns();
                        });
                    }
                }
            });
            th.IsBackground = true;
            th.Start();

            //CommonInfo model = new CommonInfo();
            //model.ProcName = "";
            //List<System.sql
            //    Dictionary<String,String> dis= new Dictionary<string,string>();
            //dis.Add(""

            //CommonBLL bll = new CommonBLL();
        }

        private void myGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (barBtnUserMod.Enabled) barBtnUserMod.PerformClick();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            freshData();
        }

        private void CauseName_EditValueChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(AreaName, CauseName.Text, true);
            CommonClass.SetCauseWeb(WebName, CauseName.Text, AreaName.Text, true);
        }

        private void AreaName_EditValueChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(WebName, CauseName.Text, AreaName.Text, true);
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;

            lblWebId.Text = GridOper.GetRowCellValueString(myGridView1, "WebId");
            edaccShortfall.EditValue = GridOper.GetRowCellValueString(myGridView1, "accShortfall");
            if (lblWebId.Text == "") return;
            panelControl1.Visible = true;
            edaccShortfall.Focus();
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rows = myGridView1.FocusedRowHandle;
            if (rows < 0) return;
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid WebId = new Guid(myGridView1.GetRowCellValue(rowhandle, "WebId").ToString());

                if (MsgBox.ShowYesNo("是否禁用？请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("WebId", WebId));
                list.Add(new SqlPara("IsQk", "禁用"));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_WEB_ISQK_NEW", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    myGridView1.SetRowCellValue(rowhandle, "IsQk", "禁用");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rows = myGridView1.FocusedRowHandle;
            if (rows < 0) return;
            if (myGridView1.GetRowCellValue(rows, "IsAcceptejSend") == null)
            {
                return;
            }
            if (ConvertType.ToString(myGridView1.GetRowCellValue(rows, "IsAcceptejSend")) == "启用")
            {
                MsgBox.ShowError("已启用的不需再次启用！");
                return;
            }
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid WebId = new Guid(myGridView1.GetRowCellValue(rowhandle, "WebId").ToString());

                if (MsgBox.ShowYesNo("是否启用？请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("WebId", WebId));
                list.Add(new SqlPara("IsAcceptejSend", 1));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_UPDATE_BASWEB_AcceptejSend_NEW", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    freshData();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rows = myGridView1.FocusedRowHandle;
            if (rows < 0) return;
            if (myGridView1.GetRowCellValue(rows, "IsAcceptejSend") == null)
            {
                return;
            }
            if (ConvertType.ToString(myGridView1.GetRowCellValue(rows, "IsAcceptejSend")) == "停用")
            {
                MsgBox.ShowError("已停用的不需再次启停用！");
                return;
            }
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid WebId = new Guid(myGridView1.GetRowCellValue(rowhandle, "WebId").ToString());

                if (MsgBox.ShowYesNo("是否停用？请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("WebId", WebId));
                list.Add(new SqlPara("IsAcceptejSend", 0));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_UPDATE_BASWEB_AcceptejSend", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    freshData();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (rowhandle < 0) return;
            string webId = lblWebId.Text.Trim();
            decimal accShortfall = ConvertType.ToDecimal(edaccShortfall.Text);
            if (accShortfall < 0)
            {
                MsgBox.ShowOK("短欠金额不能小于0");
                edaccShortfall.Focus();
                return;
            }
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("WebId", webId));
            list.Add(new SqlPara("IsQK", "开启"));
            list.Add(new SqlPara("accShortfall", accShortfall));

            if (MsgBox.ShowYesNo("确定保存短欠金额？") != DialogResult.Yes) return;

            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_WEB_ISQK_NEW", list)) == 0) return;

            myGridView1.SetRowCellValue(rowhandle, "IsQk", "开启");
            myGridView1.SetRowCellValue(rowhandle, "accShortfall", accShortfall);
            lblWebId.Text = "";
            rowhandle = 0;
            panelControl1.Visible = false;
            MsgBox.ShowOK();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            panelControl1.Visible = false;
        }
    }
}