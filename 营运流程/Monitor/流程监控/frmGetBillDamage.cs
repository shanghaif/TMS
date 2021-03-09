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
    public partial class frmGetBillDamage : BaseForm
    {
        public frmGetBillDamage()
        {
            InitializeComponent();
        }

        private void frmGetBillDamage_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);
            CommonClass.SetWeb(txtDamageWeb, true);
            txtDamageWeb.Text = CommonClass.UserInfo.WebName;

            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate.AddDays(7);
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("billNO",txtBillNo.Text.Trim()));
                list.Add(new SqlPara("WebName", txtDamageWeb.Text.Trim() == "全部" ? "%%" : txtDamageWeb.Text.Trim()));
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("optstate", cbbOptState.Text.Trim() == "全部" ? "%%" : cbbOptState.Text.Trim() == "已完成" ? cbbOptState.Text.Trim() : cbbOptState.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BillDamageInfo",list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void OptOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowHandle = myGridView1.FocusedRowHandle;
                if (rowHandle < 0)
                {
                    MsgBox.ShowOK("请选择一条信息！");
                    return;
                }
                string billno = myGridView1.GetRowCellValue(rowHandle,"billno").ToString();
                if (MsgBox.ShowYesNo("确定要操作完成此单" + "【" + billno + "】" + "") != DialogResult.Yes) return;
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("billNo", billno));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_UPDATE_BilldamageInfo", list);
                if (SqlHelper.ExecteNonQuery(sps)>0)
                {
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

    }
}