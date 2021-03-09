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
    public partial class frmNowPayTransaction : BaseForm
    {
        public frmNowPayTransaction()
        {
            InitializeComponent();
        }

        private void frmNowPayTransaction_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar2); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            GridOper.CreateStyleFormatCondition(myGridView1, "ChangeState", FormatConditionEnum.Equal, "1", Color.LightGreen);
            GridOper.CreateStyleFormatCondition(myGridView1, "ChangeState", FormatConditionEnum.Equal, "0", Color.FromArgb(255, 255, 128));

            DateTime bdt = CommonClass.gbdate;
            bdt = bdt.AddDays(-1);
            bdt = bdt.AddHours(8);
            bdate.DateTime = bdt;
            DateTime edt = CommonClass.gedate;
            edt = edt.AddHours(7 - edt.Hour);
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
                MsgBox.ShowError("开始日期不能大于结束日期");
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate", edate.DateTime));
                list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text.Trim()));
                list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text.Trim()));
                list.Add(new SqlPara("WebName", WebName.Text.Trim() == "全部" ? "%%" : WebName.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_TRANSACTIONFORADUIT_NOWPAY_Only_105", list);
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

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmNowPayTransactionLoad frm = new frmNowPayTransactionLoad();
            frm.Show();
            //try
            //{
            //    string sBillNo = "";
            //    string sAmount = "";
            //    string sNowPayVerifBalance = "";
            //    float sumAmount = 0;
            //    if (myGridView1.RowCount > 0)
            //    {
            //        for (int i = 0; i < myGridView1.RowCount; i++)
            //        {
            //            sumAmount = sumAmount + ConvertType.ToFloat(myGridView1.GetRowCellValue(i, "Amount"));
            //            sBillNo = sBillNo + myGridView1.GetRowCellValue(i, "ID") + "@";
            //            sAmount = sAmount + myGridView1.GetRowCellValue(i, "Amount") + "@";
            //            sNowPayVerifBalance = sNowPayVerifBalance + myGridView1.GetRowCellValue(i, "NowPayVerifBalance") + "@";
            //        }
            //    }
            //    else
            //        return;
            //    if (sBillNo == "") return;
            //    string sShowOK = "现付异动审核总票数：" + ConvertType.ToString(myGridView1.RowCount)
            //             + "\r\n现付异动审核总金额：" + ConvertType.ToString(sumAmount) + "\r\n现付异动审核人：" + CommonClass.UserInfo.UserName + "\r\n是否继续？";

            //    if (MsgBox.ShowYesNo(sShowOK) != DialogResult.Yes)
            //    {
            //        return;
            //    }
            //    frmChoiceSubject frm = new frmChoiceSubject();
            //    frm.xm = "非提付异动";
            //    DialogResult result = frm.ShowDialog();
            //    if (result == DialogResult.Cancel) return;
            //    string subjectOne = frm.SubjectOne;
            //    string subjectTwo = frm.SubjectTwo;
            //    string subjectThree = frm.SubjectThree;
            //    string Verifydirection = frm.Verifydirection;
            //    string summary = frm.Summary;
            //    List<SqlPara> list = new List<SqlPara>();
            //    list.Add(new SqlPara("billNo", sBillNo));
            //    list.Add(new SqlPara("AduitDate", CommonClass.gcdate));
            //    list.Add(new SqlPara("Amount", sAmount));
            //    list.Add(new SqlPara("AduitBillState", "非提付异动"));
            //    list.Add(new SqlPara("AduitMan", CommonClass.UserInfo.UserName));
            //    list.Add(new SqlPara("AduitSite", CommonClass.UserInfo.SiteName));
            //    list.Add(new SqlPara("AduitWeb", CommonClass.UserInfo.WebName));
            //    list.Add(new SqlPara("AduitCause", CommonClass.UserInfo.CauseName));
            //    list.Add(new SqlPara("AduitArea", CommonClass.UserInfo.AreaName));
            //    list.Add(new SqlPara("NowPayVerifBalance", sNowPayVerifBalance));
            //    list.Add(new SqlPara("subjectOne", subjectOne));
            //    list.Add(new SqlPara("subjectTwo", subjectTwo));
            //    list.Add(new SqlPara("subjectThree", subjectThree));
            //    list.Add(new SqlPara("Verifydirection", Verifydirection));
            //    list.Add(new SqlPara("summary", summary));
            //    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_CHECK_TRANSACTIONADUIT_NOWPAY_Only_105", list);
            //    if (SqlHelper.ExecteNonQuery(sps) > 0)
            //    {
            //        MsgBox.ShowOK();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MsgBox.ShowException(ex);
            //}
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
    }
}
