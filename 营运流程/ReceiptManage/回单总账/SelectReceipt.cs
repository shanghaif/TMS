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
    public partial class SelectReceipt : BaseForm
    {
        public SelectReceipt()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SelectReceipt_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar1);
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem4);

            string[] stateList = CommonClass.Arg.ReceiptSelectState.Split(',');
            if (stateList.Length > 0)
            {
                for (int i = 0; i < stateList.Length; i++)
                {
                    state.Properties.Items.Add(stateList[i]);
                }
                state.SelectedIndex = 0;
            }

            CommonClass.SetSite(comboBoxEdit1, true);
            CommonClass.SetSite(comboBoxEdit2, true);
            bdate.DateTime = CommonClass.gbdate;
            dateEdit1.DateTime = CommonClass.gedate;
            comboBoxEdit1.Text = CommonClass.UserInfo.SiteName;
            comboBoxEdit2.Text = "全部";
            GridOper.CreateStyleFormatCondition(myGridView1, "SendOperateTime", DevExpress.XtraGrid.FormatConditionEnum.NotEqual, null, Color.FromArgb(255, 128, 128));//回单未寄出 红色
            GridOper.CreateStyleFormatCondition(myGridView1, "BackOperateTime", DevExpress.XtraGrid.FormatConditionEnum.NotEqual, null, Color.FromArgb(255, 255, 128));//回单返回 黄色
            GridOper.CreateStyleFormatCondition(myGridView1, "ToFactoryOperateTime", DevExpress.XtraGrid.FormatConditionEnum.NotEqual, null, Color.FromArgb(128, 255, 128));//回单返厂 绿色
        }

        private void btnRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SelectType", state.Text));
                list.Add(new SqlPara("dFrom", bdate.DateTime));
                list.Add(new SqlPara("dTo", dateEdit1.DateTime));
                list.Add(new SqlPara("FromSite", comboBoxEdit1.Text));
                list.Add(new SqlPara("ToSite", comboBoxEdit2.Text));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_Receipt_Select", list);
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

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem16_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem17_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "回单总账");
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
    }
}