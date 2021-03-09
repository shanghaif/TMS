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

namespace ZQTMS.UI
{
    public partial class BespeakSendGoods_Manage : BaseForm
    {
        public BespeakSendGoods_Manage()
        {
            InitializeComponent();
        }

        private void BespeakSendGoods_Manage_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("预约送货管理");//xj/2019/5/28
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3);
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);

            bgdate.DateTime = CommonClass.gbdate;
            eddate.DateTime = CommonClass.gedate;
            CommonClass.SetCause(cause, true);
            cause.EditValue = CommonClass.UserInfo.CauseName;
            area.EditValue = CommonClass.UserInfo.AreaName;
            web.EditValue = CommonClass.UserInfo.WebName;
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bgdate.DateTime));
                list.Add(new SqlPara("edate", eddate.DateTime));
                list.Add(new SqlPara("cause", (cause.Text.Trim() == "全部" ? "%%" : cause.Text.Trim())));
                list.Add(new SqlPara("area", (area.Text.Trim() == "全部" ? "%%" : area.Text.Trim())));
                list.Add(new SqlPara("web", (web.Text.Trim() == "全部" ? "%%" : web.Text.Trim())));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BLLBESPEAKSG_BYWAYBILL", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0)
            {

                MsgBox.ShowOK("请选择一个运单！");
                return;
            }

            DataRow dr = myGridView1.GetDataRow(myGridView1.FocusedRowHandle);

            if (dr == null || dr["BillNO"] == null) return;

            string crrBillNO = dr["BillNO"].ToString();


            if (string.IsNullOrEmpty(crrBillNO))
            {
                MsgBox.ShowOK("请选择一个运单！");
                return;
            }

            frmAppointmentSend frm = new frmAppointmentSend();
            frm.crrBillNO = crrBillNO;
            frm.ShowDialog();
            simpleButton6_Click(null, null);
        }

        private void cause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(area, cause.Text.Trim(), true);
            CommonClass.SetCauseWeb(web, cause.Text.Trim(), area.Text.Trim(), true);
        }

        private void area_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(web, cause.Text.Trim(), area.Text.Trim(), true);
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

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.DataRowCount > 0)
            {
                DataTable dt = ((DataTable)myGridControl1.DataSource).Clone();

                dt.Columns.Add("xh");

                int xh = 1;
                for (int index = 0; index < myGridView1.RowCount; index++)
                {

                    dt.Rows.Add(myGridView1.GetDataRow(index).ItemArray);

                    dt.Rows[index]["xh"] = xh;
                    xh += 1;
                }
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);


                frmPrintRuiLang fpr = new frmPrintRuiLang("到货预约清单", ds);
                fpr.ShowDialog();
            }
            else
            {
                MessageBox.Show("没有数据可打印!");

            }
        }
    }
}