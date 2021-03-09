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
    public partial class frmOperationAging : BaseForm
    {
        public frmOperationAging()
        {
            InitializeComponent();
        }

       

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmOperationAging_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("省际干线运行时效表");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            //comboBoxEdit1.Text = CommonClass.UserInfo.WebName;
           // comboBoxEdit2.Text = CommonClass.UserInfo.WebName;
            comboBoxEdit1.Text = "全部";
            comboBoxEdit2.Text = "全部";

            CommonClass.SetSite(comboBoxEdit1, true);
            CommonClass.SetSite(comboBoxEdit2,true);

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("startSite", comboBoxEdit1.Text.Trim()=="全部" ? "%%" :comboBoxEdit1.Text.Trim()));
                list.Add(new SqlPara("endSite", comboBoxEdit2.Text.Trim()=="全部" ? "%%" :comboBoxEdit2.Text.Trim()));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_GX_YXSJB", list);
                DataSet ds = SqlHelper.GetDataSet(spe);
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "");
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmOperationAgingADD foa = new frmOperationAgingADD();
            foa.ShowDialog();
            //simpleButton1_Click(null,null);
        }

        //修改
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0)
            {
                MsgBox.ShowOK("请选择一条信息!");
                return;
            }
            string id = myGridView1.GetRowCellValue(rowhandle,"id").ToString();
            string startweb = myGridView1.GetRowCellValue(rowhandle, "startweb").ToString();
            string endweb = myGridView1.GetRowCellValue(rowhandle, "endweb").ToString();
            string runtime = myGridView1.GetRowCellValue(rowhandle, "runtime").ToString();
            string remarks = myGridView1.GetRowCellValue(rowhandle, "remarks").ToString();
            string startSite = myGridView1.GetRowCellValue(rowhandle, "StartSite").ToString();
            string endSite = myGridView1.GetRowCellValue(rowhandle, "EndSite").ToString();
            //2018.1.11wbw
            string StandardDepartureTime = myGridView1.GetRowCellValue(rowhandle, "StandardDepartureTime").ToString();
            string Shift = myGridView1.GetRowCellValue(rowhandle, "Shift").ToString();
            string Models = myGridView1.GetRowCellValue(rowhandle, "Models").ToString();
            string FlatStandardTime = myGridView1.GetRowCellValue(rowhandle, "FlatStandardTime").ToString();
            string FlatStandardArrivalTime = myGridView1.GetRowCellValue(rowhandle, "FlatStandardArrivalTime").ToString();
            string StandardArrivalTime = myGridView1.GetRowCellValue(rowhandle, "StandardArrivalTime").ToString();
            string Kilometre = myGridView1.GetRowCellValue(rowhandle, "Kilometre").ToString();

            frmOperationAgingADD foa = new frmOperationAgingADD();
            foa.id1 = id;
            foa.startweb1 = startweb;
            foa.endweb1 = endweb;
            foa.runtime1 = runtime;
            foa.remarks1 = remarks;
            foa.flag = 1;
            foa.startSite = startSite;
            foa.endSite = endSite;
            //2018.1.11wbw
            foa.StandardDepartureTime2 = StandardDepartureTime;
            foa.Shift2 = Shift;
            foa.Models2 = Models;
            foa.FlatStandardTime2 = FlatStandardTime;
            foa.FlatStandardArrivalTime2 = FlatStandardArrivalTime;
            foa.StandardArrivalTime2 = StandardArrivalTime;
            foa.Kilometre2 = Kilometre;

            foa.ShowDialog();
            simpleButton1_Click(null,null);


        }
        //删除
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0)
            {
                MsgBox.ShowOK("请选择一条信息");
                return;
            }
            if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
            {
                return;
            }
            string id = myGridView1.GetRowCellValue(rowhandle, "id").ToString();
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("id", id));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_DELETE_GX_YXSJB", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    MsgBox.ShowOK("删除成功！");
                    simpleButton1_Click(null, null);

                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmOperationAgingUP foa = new frmOperationAgingUP();
            foa.ShowDialog();
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

    }
}