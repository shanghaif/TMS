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
using System.Reflection;

namespace ZQTMS.UI
{
    public partial class frmWithdrowCashApply : BaseForm
    {
        public frmWithdrowCashApply()
        {
            InitializeComponent();
        }

        private void frmWithdrowCashApply_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例

            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;

            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());

            CommonClass.SetCause(Cause, true);
            CommonClass.SetArea(Area, Cause.Text);
            CommonClass.SetWeb(Web, Area.Text);
            Cause.Text = CommonClass.UserInfo.CauseName;
            Area.Text = CommonClass.UserInfo.AreaName;
            Web.Text = CommonClass.UserInfo.WebName;

            GridOper.CreateStyleFormatCondition(myGridView1, "applyState", DevExpress.XtraGrid.FormatConditionEnum.Equal, "通过", Color.Green);//已通过 绿色
            GridOper.CreateStyleFormatCondition(myGridView1, "applyState", DevExpress.XtraGrid.FormatConditionEnum.Equal, "否决", Color.Red);//已否决，黄色

            //不是总部的财务，事业部固定
            if (CommonClass.UserInfo.SiteName != "总部" && CommonClass.UserInfo.WebName.ToString().Contains("财务"))
            {
                Cause.Enabled = false;
            }


        }

        protected void ShowWindow()
        {
            DateTime bdt = bdate.DateTime;
            DateTime edt = edate.DateTime;

            string st = ApplyState.Text.Trim();
            st = st == "全部" ? "%%" : st;

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdt));
                list.Add(new SqlPara("edate", edt));
                list.Add(new SqlPara("cause", Cause.Text.Trim() == "全部" ? "%%" : Cause.Text.Trim()));
                list.Add(new SqlPara("area", Area.Text.Trim() == "全部" ? "%%" : Area.Text.Trim()));
                list.Add(new SqlPara("web", Web.Text.Trim() == "全部" ? "%%" : Web.Text.Trim()));
                list.Add(new SqlPara("state", st));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLWITHDRAWCASHAPPLY", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            ShowWindow();
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmwithdrowCashApplyAdd frm = new frmwithdrowCashApplyAdd();
            frm.ShowDialog();
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SH("通过");
        }

        private void SH(string state)
        {
            if (MsgBox.ShowYesNo("是否" + state + "?\r\r此操作不可逆，请确认！") != DialogResult.Yes)
            {
                return;
            }
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "id").ToString());

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", id));
                list.Add(new SqlPara("man", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("Type", state));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_SH_BILLWITHDRAWCASHAPPLY", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                string errmsg = ex.Message.ToString();
                MsgBox.ShowOK(errmsg.Replace("数据库访问异常： 错误信息：", ""));
            }
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SH("否决");
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void Cause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(Area, Cause.Text.Trim(), true);
            CommonClass.SetCauseWeb(Web, Cause.Text.Trim(), Area.Text.Trim());
        }

        private void Area_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(Web, Cause.Text.Trim(), Area.Text.Trim());
        }

        private void Web_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void barButtonItem16_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Assembly ass = Assembly.LoadFrom(Application.StartupPath + "\\Plugin\\ZQTMS.UI.BuisinessManage.dll");
            if (ass == null) return;
            Type type = ass.GetType("ZQTMS.UI.frmPrintRuiLangWithCondition");
            if (type == null) return;
            Form frm = (Form)Activator.CreateInstance(type, "23");
            if (frm == null) return;
            frm.Show();
        }

        private void barButtonItem17_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SH("执行");
        }
    }
}