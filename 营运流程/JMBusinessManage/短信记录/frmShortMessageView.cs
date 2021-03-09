using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmShortMessageView : BaseForm
    {
        public frmShortMessageView()
        {
            InitializeComponent();
        }

        private void frmShortMessageView_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("短信查看");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            bedate.DateTime = CommonClass.gbdate;
            enddate.DateTime = CommonClass.gedate;
            edsite.EditValue = CommonClass.UserInfo.SiteName;
            edwebid.EditValue = CommonClass.UserInfo.WebName;
            CommonClass.SetSite(edsite, true);
            CommonClass.SetWeb(edwebid,true);
            BarMagagerOper.SetBarPropertity(bar3);
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            //if (bdate.DateTime.Date > edate.DateTime.Date)
            //{
            //    XtraMessageBox.Show("开始日期不能大于结束日期", "日期选择错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bedate", bedate.DateTime));
                list.Add(new SqlPara("enddate", enddate.DateTime));
                list.Add(new SqlPara("edsite", edsite.Text.Trim() == "全部" ? "%%" : edsite.Text.Trim()));
                list.Add(new SqlPara("edweb", edwebid.Text.Trim() == "全部" ? "%%" : edwebid.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_B_SMS_RECORD_JM", list);
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

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        //一键扣费
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (myGridView1.RowCount == 0) return;

                string idBatch = "";
                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    if (!myGridView1.GetRowCellValue(i, "ISFeeDeduction").Equals("1"))
                    {
                        idBatch += myGridView1.GetRowCellValue(i, "id") + "@";
                        
                    }
                }
                if (idBatch=="") return;
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("id", idBatch));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_ADD_B_SMS_RECORD_KF_BATCH", list);
                SqlHelper.ExecteNonQuery(spe);
                MsgBox.ShowOK();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, this.Text);
        }

     
    }
}