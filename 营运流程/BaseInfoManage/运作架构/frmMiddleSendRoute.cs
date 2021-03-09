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

namespace ZQTMS.UI.BaseInfoManage
{
    public partial class frmMiddleSendRoute : BaseForm
    {
        public frmMiddleSendRoute()
        {
            InitializeComponent();
        }

        private void frmMiddleSendRoute_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);

            CommonClass.SetCause(txtcauseName,true);
            bdate.DateTime = CommonClass.gbdate.AddDays(-15);
            edate.DateTime = CommonClass.gedate;
        }

        private void txtcauseName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(txtareaName,txtcauseName.Text.Trim(),true);
            CommonClass.SetCauseWeb(txtwebName,txtcauseName.Text.Trim(),txtareaName.Text.Trim(),true);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmMiddleRouteAdd frm = new frmMiddleRouteAdd();
            frm.flag = 1;
            frm.Show();
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (myGridView1.FocusedRowHandle < 0) return;
                string usesite = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "UseSiteName").ToString();
                string useweb = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "UseWebName").ToString();
                string endsite = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "EndSite").ToString();
                string ID = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "Id").ToString();
                frmMiddleRouteAdd frm = new frmMiddleRouteAdd();
                frm.UseSite = usesite;
                frm.UseWeb = useweb;
                frm.EndSite = endsite;
                frm.routeId = ID;
                frm.flag = 2;
                frm.Show();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate",edate.DateTime));
                list.Add(new SqlPara("causeName", txtcauseName.Text.Trim() == "全部" ? "%%" : txtcauseName.Text.Trim()));
                list.Add(new SqlPara("areaName", txtareaName.Text.Trim() == "全部" ? "%%" : txtareaName.Text.Trim()));
                list.Add(new SqlPara("webName", txtwebName.Text.Trim() == "全部" ? "%%" : txtwebName.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_frmMiddleSendRoute",list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds==null || ds.Tables.Count==0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (myGridView1.FocusedRowHandle < 0) return;
                string routeId = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle,"Id").ToString();
                string endSite = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "EndSite").ToString();
                if (MsgBox.ShowYesNo("确定要删除选中信息？") != DialogResult.Yes) return;
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_MiddleSendRoute", new List<SqlPara>() { new SqlPara("routeId", routeId), new SqlPara("endsite", endSite) });
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

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }
        
    }
}