using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using DevExpress.XtraGrid.Columns;
using ZQTMS.Tool;
using ZQTMS.UI.BaseInfoManage.基础资料;


namespace ZQTMS.UI.BaseInfoManage
{
    public partial class frmCostManage : ZQTMS.Tool.BaseForm
    {
        public frmCostManage()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbRetrieve_1_Click(object sender, EventArgs e)
        {
            Getdata();
        }

        public void Getdata()
        {
            List<SqlPara> list = new List<SqlPara>();


            list.Add(new SqlPara("StartSite", StartSite_1.Text.Trim() == "全部" ? "%%" : StartSite_1.Text.Trim()));
            list.Add(new SqlPara("DestinationSite", TransferSite_1.Text.Trim() == "全部" ? "%%" : TransferSite_1.Text.Trim()));

            list.Add(new SqlPara("SendWebName", sendweb.Text.Trim() == "全部" ? "%%" : sendweb.Text.Trim()));
            list.Add(new SqlPara("MiddleWebName", middleweb.Text.Trim() == "全部" ? "%%" : middleweb.Text.Trim()));
            list.Add(new SqlPara("ProjectType", projecttype.Text.Trim() == "全部" ? "%%" : projecttype.Text.Trim()));

            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_CostControlsList", list);
            myGridControl1.DataSource = SqlHelper.GetDataTable(sps);


        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmCostManageAdd cost = new frmCostManageAdd();
            cost.ShowDialog();
        }

        private void frmCostManage_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView2);
            GridOper.SetGridViewProperty(myGridView2);
            BarMagagerOper.SetBarPropertity(bar9); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);
            CommonClass.SetSite(StartSite_1, true);
            CommonClass.SetSite(TransferSite_1, true);
            CommonClass.SetWeb(sendweb, true);
            CommonClass.SetWeb(middleweb, true);


            StartSite_1.Text = CommonClass.UserInfo.SiteName;
            TransferSite_1.Text = CommonClass.UserInfo.SiteName;
            sendweb.Text = CommonClass.UserInfo.WebName;
            middleweb.Text = CommonClass.UserInfo.WebName;
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (myGridView2.RowCount == 0) return;

                myGridView2.PostEditor();

                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                string Ids = "";
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    if (ConvertType.ToString(myGridView2.GetRowCellValue(i, "ischecked")) == "1")
                    {
                        Ids += myGridView2.GetRowCellValue(i, "ID") + "@";//单号
                    }
                }
                if (Ids == "") return;

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ControlId", Ids));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_CostControlsList", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    Getdata();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }


        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
        

            try
            {
                int rowhandle = myGridView2.FocusedRowHandle;
                if (rowhandle < 0) return;
                string ControlId = myGridView2.GetRowCellValue(rowhandle, "ID").ToString();
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", ControlId));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_CostControlsList_ByID", list);//根据id获取其他字段
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                DataRow dr = ds.Tables[0].Rows[0];
                frmCostManageAdd frm = new frmCostManageAdd();
                frm.dr =dr ;
                frm.ShowDialog();
                

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Getdata();
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView2);
        }

        private void barButtonItem14_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmCostManageUp frm = new frmCostManageUp();
            frm.ShowDialog();
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView2);
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

    }
}
