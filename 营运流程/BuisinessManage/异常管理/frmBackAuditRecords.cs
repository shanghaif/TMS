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

namespace ZQTMS.UI
{
    public partial class frmBackAuditRecords : BaseForm
    {
        DataSet ds = new DataSet();
        public frmBackAuditRecords()
        {
            InitializeComponent();
        }

        private void frmShortConnOutbound_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem4);

            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            CommonClass.SetUser(OptCancelMan,"全部");
            OptCancelMan.EditValue = CommonClass.UserInfo.UserName;
            string[] InOutTypeModeList = CommonClass.Arg.InOutType.Split(',');
            if (InOutTypeModeList.Length > 0)
            {
                for (int i = 0; i < InOutTypeModeList.Length; i++)
                {
                    InOutType.Properties.Items.Add(InOutTypeModeList[i]);
                }
                InOutType.SelectedIndex = InOutType.Properties.Items.Count - 1;
            }
            CommonClass.SetWeb(WebName,"全部");
            WebName.EditValue = CommonClass.UserInfo.WebName;
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
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate", edate.DateTime));
                list.Add(new SqlPara("InOutType", OptCancelMan.Text.Trim() == "全部" ? "%%" : InOutType.Text.Trim()));
                list.Add(new SqlPara("OptCancelMan", InOutType.Text.Trim() == "全部" ? "%%" : OptCancelMan.Text.Trim()));
                list.Add(new SqlPara("IsBill", IsBill.SelectedIndex));
                list.Add(new SqlPara("WebName", WebName.Text.Trim() == "全部" ? "%%" : WebName.Text.Trim()));


                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_VERIFYOFFACCOUNT_BACKCANCEL", list);
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

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            getData();
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
          
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void CauseName_EditValueChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(InOutType, OptCancelMan.EditValue.ToString());
        }

        private void AreaName_EditValueChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(WebName, OptCancelMan.Text.Trim(), InOutType.Text.Trim());
        }
    }
}