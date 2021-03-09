using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DevExpress.XtraEditors;
using System.Data.OleDb;
using System.Collections;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Columns;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using DevExpress.XtraGrid;

namespace ZQTMS.UI
{
    public partial class frmKickbackOrCollectionOrMatPayAudit : BaseForm
    {
        public frmKickbackOrCollectionOrMatPayAudit()
        {
            InitializeComponent();
        }

        //加载
        private void frmKickbackOrCollectionOrMatPayAudit_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("代收货款/垫付/回扣核销");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例 
            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
            //设置颜色
            GridOper.CreateStyleFormatCondition(myGridView1, "VerifState_View", FormatConditionEnum.Equal, "已核销", Color.LightGreen);
            GridOper.CreateStyleFormatCondition(myGridView1, "VerifState_View", FormatConditionEnum.Equal, "部分核销", Color.FromArgb(255, 255, 128));//部分核销颜色隐藏
            GridOper.GetGridViewColumn(myGridView1, "VerifMoney").AppearanceCell.BackColor = Color.Yellow;
            GridOper.GetGridViewColumn(myGridView1, "isIncom").AppearanceCell.BackColor = Color.Yellow;

            if (CommonClass.UserInfo.SiteName == "总部")
            {
                bsite.Text = "全部";
            }
            else
            {
                bsite.Text = CommonClass.UserInfo.SiteName;
            }
            //bsite.Text = CommonClass.UserInfo.SiteName;  tll 2017-6-22
            esite.Text = "全部";
            //web.Text = CommonClass.UserInfo.WebName;

            //Cause.Text = CommonClass.UserInfo.CauseName;
            //Area.Text = CommonClass.UserInfo.AreaName;    tll 2017-6-21

            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;

            CommonClass.SetSite(bsite, true);
            CommonClass.SetSite(esite, true);
            CommonClass.SetCause(Cause, true);
            //CommonClass.SetArea(Area, Cause.Text, true);  tll 2017-6-21
            CommonClass.SetArea(Area, Cause.Text);
            CommonClass.SetCauseWeb(web, Cause.Text, Area.Text);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);

            Cause.Text = CommonClass.UserInfo.CauseName;
            Area.Text = CommonClass.UserInfo.AreaName;
            web.Text = CommonClass.UserInfo.WebName;
            label13.Visible = false;
            label12.Visible = false;
        }

        //提取
        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("CauseName", Cause.Text.Trim() == "全部" ? "%%" : Cause.Text.Trim()));
                list.Add(new SqlPara("AreaName", Area.Text.Trim() == "全部" ? "%%" : Area.Text.Trim()));
                list.Add(new SqlPara("BegWeb", web.Text.Trim() == "全部" ? "%%" : web.Text.Trim()));
                list.Add(new SqlPara("StartSite", bsite.Text.Trim() == "全部" ? "%%" : bsite.Text.Trim()));
                list.Add(new SqlPara("TransferSite", esite.Text.Trim() == "全部" ? "%%" : esite.Text.Trim()));
                list.Add(new SqlPara("AdultType", AdultType.Text.Trim()));//按核销类型提取
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_DiscountTransfer_Or_CollectionPay_Or_MatPay", list);
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
            //设置颜色显示
        }

        //关闭
        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //核销
        frmKickbackOrCollectionOrMayPayLoad ww;
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //frmKickbackOrCollectionOrMayPayLoad ww = new frmKickbackOrCollectionOrMayPayLoad();
            //ww.Owner = this;
            //ww.ShowDialog();
            
            string frm = "frmKickbackOrCollectionOrMayPayLoad";
            if (CommonClass.CheckFormIsOpen(frm) == false)
            {
                ww = new frmKickbackOrCollectionOrMayPayLoad();
                ww.Owner = this;
                ww.Show();
            }
            else
            {
                ww.Focus();
            }
        }

        



        //安单号核销
        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //frmDiscountTransferLoadByBillNo frm = new frmDiscountTransferLoadByBillNo();
            //frm.Owner = this;
            //frm.Show();
        }

        //导出
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "回扣核销明细");
        }

        //快找
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        //退出
        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        //确认
        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0)
            {
                MsgBox.ShowOK("请至少选择一条信息");
                return;
            }

            string allBillNo = "";
            string DisTranVerifState = "";
            string ConfirmStatus = "";
            myGridView1.PostEditor();
            if (rowhandle >= 0)
            {
                for (int i = 0; i < myGridView1.RowCount; i++)
                {

                    if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) > 0)
                    {
                        allBillNo += myGridView1.GetRowCellValue(i, "BillNo") + ",";
                        DisTranVerifState = myGridView1.GetRowCellValue(i, "DisTranVerifState").ToString();
                        if (DisTranVerifState != "1")
                        {
                            MsgBox.ShowOK("存在未核销的单，不能做确认！");
                            return;
                        }
                        ConfirmStatus = myGridView1.GetRowCellValue(i, "ConfirmStatus").ToString();
                        if (ConfirmStatus == "已确认")
                        {
                            MsgBox.ShowOK("存在已确认的单，不能重复确认，请从新确认！");
                            return;
                        }
                    }

                }
            }
            if (MsgBox.ShowYesNo("是否确认？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
            {
                return;
            }
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("allBillNo", allBillNo));
            SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_UPDATE_ConfirmStatus", list);
            if (SqlHelper.ExecteNonQuery(spe) > 0)
            {
                MsgBox.ShowOK();
                cbRetrieve_Click(null, null);
                return;
            }
        }

        //打印
        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
        }

        //付款
        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //frmFukuanDetail frm = new frmFukuanDetail();
            //frm.Owner = this;
            //frm.ShowDialog();
        }

        private void Cause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(Area, Cause.Text);
        }

        private void Area_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(web, Cause.Text, Area.Text);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        //双击修改银行信息
        private void barButtonItem16_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //DataRow row = this.myGridView1.GetFocusedDataRow();
            //if (row != null)
            //{
            //    if (!string.IsNullOrEmpty(row["OptMan"].ToString()) && !string.IsNullOrEmpty(row["OptTime"].ToString()))
            //    {
            //        MsgBox.ShowOK(row["BillNo"].ToString() + "：已核销，不允许修改！");
            //        return;
            //    }
            //    frmAduitBankModify frm = new frmAduitBankModify(row);
            //    frm.Owner = this;
            //    frm.ShowDialog();
            //}
            //else
            //{
            //    MsgBox.ShowOK("请选中行！");
            //}
        }
    }
}
