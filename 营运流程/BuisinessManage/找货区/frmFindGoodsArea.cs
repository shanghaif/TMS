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

namespace ZQTMS.UI
{
    public partial class frmFindGoodsArea : BaseForm
    {
        public frmFindGoodsArea()
        {
            InitializeComponent();
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                myGridControl1.DataSource = null;
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("OperType", comboBoxEdit1.SelectedIndex));
                list.Add(new SqlPara("State", State.SelectedIndex));
                list.Add(new SqlPara("DealState", DealState.SelectedIndex));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_FINDGOODS_RECORD", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                if (myGridView1.RowCount < 2000) myGridView1.BestFitColumns();
            }
        }

        private void WayBillRecord_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("找货区记录");//xj/2019/5/28
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, false);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);

            //超过3天未处理的
            GridOper.CreateStyleFormatCondition(myGridView1, "LckDay", DevExpress.XtraGrid.FormatConditionEnum.Equal, 1, Color.Red);
            //超过3天已认领未处理的
            GridOper.CreateStyleFormatCondition(myGridView1, "remaindays", DevExpress.XtraGrid.FormatConditionEnum.Equal, 1, Color.Green);

            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "找货区记录");
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmFindGoodsAdd foc = new frmFindGoodsAdd();
            foc.type = 0;  //maohui20180622
            foc.Show();
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;

            frmFindGoodsAdd foc = frmFindGoodsAdd.Get_frmFindGoodsAdd;
            foc.Show();
            foc.id = GridOper.GetRowCellValueString(myGridView1, rowhandle, "Id");
            foc.type = 1;   //maohui20180623
            foc.getdata();
            foc.TopMost = true;
            foc.TopMost = false;
        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;

            frmFindGoodsFileUp ffg = new frmFindGoodsFileUp();
            ffg.Id = GridOper.GetRowCellValueString(myGridView1, rowhandle, "Id");
            ffg.ShowDialog();
        }

        private void myGridControl1_DoubleClick(object sender, EventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;

            frmFindGoodsImage ffgi = frmFindGoodsImage.Get_frmFindGoodsImage;
            ffgi.Show();
            ffgi.Id = GridOper.GetRowCellValueString(myGridView1, "Id");
            ffgi.getdata();
            ffgi.TopMost = true;
            ffgi.TopMost = false;
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string Id = GridOper.GetRowCellValueString(myGridView1, "Id");
            if (Id == "") return;

            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_SET_FINDGOODS_DEAL", new List<SqlPara> { new SqlPara("Type", 0), new SqlPara("Id", Id) })) == 0) return;

            if (SqlHelper.ExecteNonQuery_ZQTMS(new SqlParasEntity(OperType.Execute, "USP_SET_FINDGOODS_DEAL", new List<SqlPara> { new SqlPara("Type", 0), new SqlPara("Id", Id) })) == 0) return;  //maohui20180718
                MsgBox.ShowOK("处理完成!");
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string billNo = GridOper.GetRowCellValueString(myGridView1, "BillNo");
            if (billNo == "")
            {
                MsgBox.ShowOK("没有找到要打印的运单或者未认领,请重试或稍后再试!");
                return;
            }
            foreach (string s in billNo.Split(','))
            {
                if (s.Trim() == "") continue;
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTLABEL_DEV", new List<SqlPara> { new SqlPara("BillNo", s) }));
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    MsgBox.ShowOK("没有找到要打印的运单,请稍后再试");
                    return;
                }

                frmPrintLabelDev fpld = new frmPrintLabelDev(ds.Tables[0]);
                fpld.ShowDialog();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string Id = GridOper.GetRowCellValueString(myGridView1, "Id");
            if (Id == "")
            {
                MsgBox.ShowOK("请选择要认领的记录!");
                return;
            }
            string billNo = BillNo.Text.Trim().TrimEnd(',');
            if (billNo == "")
            {
                MsgBox.ShowOK("请输入认领运单号!");
                return;
            }
            if (MsgBox.ShowYesNo("确定认领？") != DialogResult.Yes) return;
            billNo += ",";
            List<SqlPara> list1 = new List<SqlPara>();
            list1.Add(new SqlPara("BillNo", billNo));

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("Id", Id));
            list.Add(new SqlPara("BillNo", billNo));
            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_FindGoods_Claim", list)) == 0) return;
            List<SqlPara> listZQTMS = new List<SqlPara>();
            listZQTMS.Add(new SqlPara("Id", Id)); //maohui20180706
            listZQTMS.Add(new SqlPara("BillNo", billNo));
            if (SqlHelper.ExecteNonQuery_ZQTMS(new SqlParasEntity(OperType.Execute, "USP_FindGoods_Claim", listZQTMS)) == 0) return;
            MsgBox.ShowOK("认领成功！");
            BillNo.Text = "";
            panelControl2.Visible = false;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            BillNo.Text = "";
            panelControl2.Visible = false;
        }

        private void barButtonItem18_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            string Id = GridOper.GetRowCellValueString(myGridView1, rowhandle, "Id");
            if (Id == "") return;
            if (MsgBox.ShowYesNo("确定取消认领？") != DialogResult.Yes) return;

            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_FindGoods_CANCEL_Claim", new List<SqlPara> { new SqlPara("Id", Id) })) == 0) return;

            if (SqlHelper.ExecteNonQuery_ZQTMS(new SqlParasEntity(OperType.Execute, "USP_FindGoods_CANCEL_Claim", new List<SqlPara> { new SqlPara("Id", Id) })) == 0) return;
            myGridView1.SetRowCellValue(rowhandle, "ClaimDate", DBNull.Value);
            myGridView1.SetRowCellValue(rowhandle, "BillNo", "");
            myGridView1.SetRowCellValue(rowhandle, "ClaimMan", "");
            myGridView1.SetRowCellValue(rowhandle, "ClaimCauseName", "");
            myGridView1.SetRowCellValue(rowhandle, "ClaimAreaName", "");
            myGridView1.SetRowCellValue(rowhandle, "ClaimDepName", "");
            myGridView1.SetRowCellValue(rowhandle, "ClaimSiteName", "");
            myGridView1.SetRowCellValue(rowhandle, "ClaimWebName", "");
            MsgBox.ShowOK("取消认领成功!");
        }

        private void barButtonItem16_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string Id = GridOper.GetRowCellValueString(myGridView1, "Id");
            if (Id == "") return;
            if (MsgBox.ShowYesNo("确定删除？") != DialogResult.Yes) return;

            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_DELETE_FINDGOODS_BY_ID", new List<SqlPara> { new SqlPara("Id", Id) })) == 0) 
                return;
            if (SqlHelper.ExecteNonQuery_ZQTMS(new SqlParasEntity(OperType.Execute, "USP_DELETE_FINDGOODS_BY_ID", new List<SqlPara> { new SqlPara("Id", Id) })) == 0)  //maohui20180706
                return;
            myGridView1.DeleteSelectedRows();
            MsgBox.ShowOK("删除成功!");
        }

        private void barButtonItem17_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;
            panelControl2.Visible = true;
            BillNo.Focus();
        }

        private void panelControl2_Leave(object sender, EventArgs e)
        {
            BillNo.Text = "";
            panelControl2.Visible = false;
        }
    }
}