using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using DevExpress.XtraEditors;
using System.Text.RegularExpressions;


namespace ZQTMS.UI
{
    public partial class frmMonthLyAudit : BaseForm
    {
        public frmMonthLyAudit()
        {
            InitializeComponent();
        }

        private void frmMonthLyAudit_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3);
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);

            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;

            CommonClass.SetCause(Cuase, true);
            Cuase.EditValue = CommonClass.UserInfo.CauseName;
            Area.EditValue = CommonClass.UserInfo.AreaName;
            web.EditValue = CommonClass.UserInfo.WebName;
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem18_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmAuditBatch_Month frm = new frmAuditBatch_Month();
            frm.Show();
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmAuditBatch_ShortOwe frm = new frmAuditBatch_ShortOwe();
            frm.Show();
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate", edate.DateTime));
                list.Add(new SqlPara("cuasename", Cuase.Text.Trim()));
                list.Add(new SqlPara("areaname", Area.Text.Trim()));
                list.Add(new SqlPara("webname", web.Text.Trim()));
                if (comboBoxEdit1.Text == "开单时间")
                {
                    list.Add(new SqlPara("type", 1));
                }
                if (comboBoxEdit1.Text == "审核时间")
                {
                    list.Add(new SqlPara("type", 2));
                }
                //list.Add(new SqlPara("ArrConfirmState", ArrConfirmState.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "[QSP_Get_WayBill_ArrConfirm_Aduit_new]", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void Cuase_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(Area, Cuase.Text, true);
            CommonClass.SetCauseWeb(web, Cuase.Text, Area.Text);
        }

        private void Area_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(web, Cuase.Text, Area.Text);
        }

    
    }
}