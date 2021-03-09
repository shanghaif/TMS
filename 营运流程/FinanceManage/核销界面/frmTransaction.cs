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
using DevExpress.XtraGrid;

namespace ZQTMS.UI
{
    public partial class frmTransaction : BaseForm
    {
        public frmTransaction()
        {
            InitializeComponent();
        }

        private void frmTransaction_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("异动款费用核销");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar2); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            //GridOper.CreateStyleFormatCondition(myGridView1, "ChangeState", FormatConditionEnum.Equal, "1", Color.LightGreen);
            //GridOper.CreateStyleFormatCondition(myGridView1, "ChangeState", FormatConditionEnum.Equal, "0", Color.FromArgb(255, 255, 128));
            GridOper.CreateStyleFormatCondition(myGridView1, "virefState", FormatConditionEnum.Equal, "已核销", Color.LightGreen);

            DateTime bdt = CommonClass.gbdate;
            bdt = bdt.AddDays(-2);
            bdt = bdt.AddHours(18);
            bdate.DateTime = bdt;
            DateTime edt = CommonClass.gedate;
            edt = edt.AddDays(-1);
            edt = edt.AddHours(17 - edt.Hour);
            edate.DateTime = edt;
            CommonClass.SetCause(CauseName, true);
            CauseName.EditValue = CommonClass.UserInfo.CauseName;
            AreaName.EditValue = CommonClass.UserInfo.AreaName;
            WebName.EditValue = CommonClass.UserInfo.WebName;
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
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
                list.Add(new SqlPara("transactionType",transactionType.Text.Trim()));


                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_TRANSACTIONFORADUIT_NEW", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void CauseName_EditValueChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(AreaName, CauseName.EditValue.ToString());
            CommonClass.SetCauseWeb(WebName, CauseName.Text.Trim(), AreaName.Text.Trim());
        }

        private void AreaName_EditValueChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(WebName, CauseName.Text.Trim(), AreaName.Text.Trim());
        }
        frmTransactionLoad frm;
        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            //frmTransactionLoad frm = new frmTransactionLoad();
            //frm.ShowDialog();
            string frmname = "frmTransactionLoad";
            if (CommonClass.CheckFormIsOpen(frmname) == false)
            {
                frm = new frmTransactionLoad();
                frm.Show();
            }
            else
            {
                frm.Focus();
            }
        }
    }
}
