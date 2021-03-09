using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid;


namespace ZQTMS.UI
{
    public partial class frmOweMoneyAffirmList : BaseForm
    {
        public frmOweMoneyAffirmList()
        {
            InitializeComponent();
        }

        private void frmShortConnOutbound_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            FixColumn fix = new FixColumn(myGridView1, barSubItem4);

            DateTime bdt = CommonClass.gbdate;
            bdt = bdt.AddHours(-16);
            bdate.DateTime = bdt;
            DateTime edt = CommonClass.gedate;
            edt = edt.AddHours(-16);
            edate.DateTime = edt;
            CommonClass.SetCause(CauseName, true);
            CauseName.EditValue = CommonClass.UserInfo.CauseName;
            AreaName.EditValue = CommonClass.UserInfo.AreaName;
            WebName.EditValue = CommonClass.UserInfo.WebName;
            GridOper.CreateStyleFormatCondition(myGridView1, "OweMoneyAffirmState", FormatConditionEnum.Equal, "已确认", Color.LightGreen);
            GridOper.CreateStyleFormatCondition(myGridView1, "OweMoneyAffirmState", FormatConditionEnum.Equal, "部分确认", Color.FromArgb(255, 255, 128));
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            getData();
        }

        public void getData()
        {
            if (bdate.DateTime.Date > edate.DateTime.Date)
            {
                XtraMessageBox.Show("开始日期不能大于结束日期", "日期选择错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.EditValue));
                list.Add(new SqlPara("edate", edate.EditValue));
                list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text.Trim()));
                list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text.Trim()));
                list.Add(new SqlPara("WebName", WebName.Text.Trim() == "全部" ? "%%" : WebName.Text.Trim()));
                list.Add(new SqlPara("AffirmState", comboBoxEdit1.Text.Trim() == "全部" ? "%%" : comboBoxEdit1.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "[QSP_GET_OweMoney_CONFIRM]", list);
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
        private void simpleButton7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barCheckItem3_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barCheckItem4_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rows = myGridView1.FocusedRowHandle;
            if (rows < 0) return;
            if (myGridView1.GetRowCellValue(rows, "BillNo") == null)
            {
                return;
            }
            if (MsgBox.ShowYesNo("确定取消确认?") != DialogResult.Yes) return;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", Convert.ToString(myGridView1.GetRowCellValue(rows, "BillNo")).Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_CHECK_CONFIRM_Load_Cancel", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    getData();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void btnAduit_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmOweMoneyAffirmLoad frm = new frmOweMoneyAffirmLoad();
            frm.ShowDialog();
        }

        private void CauseName_EditValueChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(AreaName, CauseName.EditValue.ToString());
        }

        private void AreaName_EditValueChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(WebName, CauseName.Text.Trim(), AreaName.Text.Trim());
        }

        private void myGridView1_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e == null || myGridView1.FocusedRowHandle < 0) return;
            try
            {
                float FetchPayVerifBalance = ConvertType.ToFloat(myGridView1.GetFocusedRowCellValue("FetchPayVerifBalance"));
                float Amount = ConvertType.ToFloat(e.Value);
                if (Amount <= 0)
                {
                    e.Valid = false;
                    e.ErrorText = "审核金额必须大于0!";
                    return;
                }
                if (Amount > FetchPayVerifBalance)
                {
                    e.Valid = false;
                    e.ErrorText = "审核金额不能大于余额!";
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void myGridView1_InvalidValueException(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            MsgBox.ShowError(e.ErrorText);
        }

        private void barButtonItem16_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barButtonItem18_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmOweMoneyVerifyLoad frm = new frmOweMoneyVerifyLoad();
            frm.ShowDialog();
        }

        private void barButtonItem15_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rows = myGridView1.FocusedRowHandle;
            if (rows < 0) return;
            if (myGridView1.GetRowCellValue(rows, "BillNo") == null)
            {
                return;
            }
            if (ConvertType.ToString(myGridView1.GetRowCellValue(rows, "OweMoneyVerifyState")) == "已核销")
            {
                MsgBox.ShowError("已审核的不能取消！");
                return;
            }
            if (MsgBox.ShowYesNo("确定取消确认?") != DialogResult.Yes) return;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", Convert.ToString(myGridView1.GetRowCellValue(rows, "BillNo")).Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_CHECK_Verify_Load_Cancel", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    getData();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
    }
}