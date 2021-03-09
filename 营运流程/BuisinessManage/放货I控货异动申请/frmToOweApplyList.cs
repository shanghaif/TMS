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
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid;
using Newtonsoft.Json;

namespace ZQTMS.UI
{
    public partial class frmToOweApplyList : BaseForm
    {
        //public string ApplyType = "控货/放货";

        public frmToOweApplyList()
        {
            InitializeComponent();
        }

        //public frmToOweApplyList(string atype)
        //{
        //    InitializeComponent();
        //    ApplyType = atype;

        //    this.Text += "-->" + ApplyType;
        //}

        private void frmApplyList_Load(object sender, EventArgs e)
        {
            //CommonClass.FormSet(this);
           // GetRight();
            CommonClass.InsertLog("提付转欠");//xj/2019/5/29
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3);
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);

            bdate.DateTime = CommonClass.gbdate.AddDays(-1);
            edate.DateTime = CommonClass.gedate;

            CommonClass.SetWeb(sqWeb, "全部");

            GridOper.CreateStyleFormatCondition(myGridView1, "AuditingState", FormatConditionEnum.Equal, "1", Color.FromArgb(192, 192, 255));//审核状态
            GridOper.CreateStyleFormatCondition(myGridView1, "LastState", FormatConditionEnum.Equal, "否决", Color.Red);//否决
            GridOper.CreateStyleFormatCondition(myGridView1, "LastState", FormatConditionEnum.Equal, "取消", Color.Red);//否决
        }

        //申请
        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmToMonthlyToOwe frm = new frmToMonthlyToOwe();
            frm.ShowDialog();
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "提付转欠款确认记录");
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            UpdateApplyState("审核");
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            string a = comboBoxEdit1.Text;
            string b = comboBoxEdit2.Text;
            string c = comboBoxEdit3.Text;

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ApplyDate_begin", bdate.DateTime));
                list.Add(new SqlPara("ApplyDate_end", edate.DateTime));
                list.Add(new SqlPara("ApplyWeb", sqWeb.Text));
                list.Add(new SqlPara("ApplyType", "提付转欠款"));
                list.Add(new SqlPara("AuditingState", (a == "全部" ? 99 : (a == "已审核" ? 1 : 2))));
                list.Add(new SqlPara("ApprovalState", (b == "全部" ? 99 : (b == "已审批" ? 1 : 2))));
                list.Add(new SqlPara("ExcuteState", (c == "全部" ? 99 : (c == "已执行" ? 1 : 2))));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "[QSP_GET_BILLAPPLY_SH]", list);
                myGridControl1.DataSource = SqlHelper.GetDataTable(sps);
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.ToString());
            }
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult result = MsgBox.ShowYesNo("确定是否取消？");
            if (result == DialogResult.Yes)
                UpdateApplyState("取消");
        }

        private void UpdateApplyState(string Type)
        {
            try
            {
                if (myGridView1.FocusedRowHandle < 0) return;

                DataRow dr = myGridView1.GetDataRow(myGridView1.FocusedRowHandle);

                if (dr == null || dr["ApplyID"] == null)
                {
                    MsgBox.ShowOK("数据异常！");
                    return;
                }

              

                string reson = "";
                if (Type != "取消")
                {
                    frmConfirmWithTxt frm = new frmConfirmWithTxt();
                    frm.Type = Type;
                    frm.ApplyContent.Text = GridOper.GetRowCellValueString(myGridView1, "ApplyContent");
                    frm.ShowDialog();
                    if (frm.DialogResult != DialogResult.Yes)
                    {
                        return;
                    }

                    reson = frm.Reson;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ApplyID", dr["ApplyID"]));
                list.Add(new SqlPara("Type", Type));
                list.Add(new SqlPara("Man", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("Reson", reson));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "[QSP_UpdateApplyState_SH]", list);
                int result = SqlHelper.ExecteNonQuery(sps);
                if (result > 0)
                {
                    MsgBox.ShowOK("操作成功！");
                    }
                else
                {
                    MsgBox.ShowOK("操作成失败！");
                }
            }
            catch (Exception ex)
            {
                string errmsg = ex.Message.ToString();
                MsgBox.ShowOK(errmsg.Replace("数据库访问异常：", ""));
            }
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            UpdateApplyState("否决");
        }

    }
}