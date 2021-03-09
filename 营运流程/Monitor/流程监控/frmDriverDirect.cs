using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Lib;
using ZQTMS.SqlDAL;
using ZQTMS.Tool;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class frmDriverDirect : BaseForm
    {
        DataSet ds = new DataSet();
        public frmDriverDirect()
        {
            InitializeComponent();
        }

        private void frmDriverDirect_Load(object sender, EventArgs e)
        {
            
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            //CommonClass.GetGridViewColumns(myGridView2);
            //GridOper.SetGridViewProperty(myGridView2);
            CommonClass.GetGridViewColumns(myGridView3);
            GridOper.SetGridViewProperty(myGridView3);
            BarMagagerOper.SetBarPropertity(bar5);
            xtraTabPage2.PageVisible = false;

            bdate.EditValue = CommonClass.gbdate.AddDays(-1);
            edate.EditValue = CommonClass.gedate;
            SignOperator.Text = CommonClass.UserInfo.UserName;
            SignDate.EditValue = System.DateTime.Now;

            bedate.EditValue = CommonClass.gbdate.AddDays(-1);
            enddate.EditValue = CommonClass.gedate;
            OperateTime.EditValue = System.DateTime.Now;
            Operator.Text = CommonClass.UserInfo.UserName;
            string[] OperateStateList = CommonClass.Arg.OperateState.Split(',');
            if (OperateStateList.Length > 0)
            {
                for (int i = 0; i < OperateStateList.Length; i++)
                {
                    OperateState.Properties.Items.Add(OperateStateList[i]);
                }
                OperateState.SelectedIndex = 0;
            }
            //取消送货签收
            dateEdit1.EditValue = CommonClass.gbdate.AddDays(-1);
            dateEdit2.EditValue = CommonClass.gedate;
           







        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.Text));
                list.Add(new SqlPara("edate", edate.Text));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_SEND_FOR_SJZS", list);
                ds = SqlHelper.GetDataSet(spe);
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
        //送货签收
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            int rowHandle = myGridView1.FocusedRowHandle;
            if (rowHandle < 0)
            {
                MsgBox.ShowOK("请选择至少一条信息");
                return;
            }
            if (SignMan.Text.Trim() == "")
            {
                MsgBox.ShowOK("必须填写签收人！");
                return;
            }
            float sumAccSend = 0;
            int AccSendCount = 0;
            int SendVerifStateCount = 0;
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) > 0)
                {
                    if (ConvertType.ToFloat(myGridView1.GetRowCellValue(i, "AccSend")) > 0)
                    {
                        AccSendCount++;
                        sumAccSend += ConvertType.ToFloat(myGridView1.GetRowCellValue(i, "AccSend"));
                    }
                    if (ConvertType.ToFloat(myGridView1.GetRowCellValue(i, "SendVerifState")) == 1)
                    {
                        SendVerifStateCount++;
                    }
                }
            }

            string BillNoStr = "";
            string SendBatchStr = "";
            for (int i = 0; i <= myGridView1.RowCount; i++)
            {
                if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) > 0)
                {
                    BillNoStr += ConvertType.ToString(myGridView1.GetRowCellValue(i, "BillNo")) + '@';
                    SendBatchStr += ConvertType.ToString(myGridView1.GetRowCellValue(i, "SendBatch")) + '@';
                }
            }
            if (BillNoStr == "")
            {
                MsgBox.ShowOK("请选择要签收运单！");
                return;
            }

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNoStr", BillNoStr));
                list.Add(new SqlPara("SignType", "送货签收"));
                list.Add(new SqlPara("SignMan", SignMan.Text.Trim()));
                list.Add(new SqlPara("SignManCardID", SignManCardID.Text.Trim()));
                list.Add(new SqlPara("AgentMan", ""));
                list.Add(new SqlPara("SignManPhone", SignManPhone.Text.Trim()));
                list.Add(new SqlPara("AgentCardId", SignManCardID.Text.Trim()));
                list.Add(new SqlPara("SignDate", SignDate.DateTime));
                list.Add(new SqlPara("SignDesc", ""));
                list.Add(new SqlPara("SignOperator", SignOperator.Text.Trim()));
                list.Add(new SqlPara("SignSite", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("SignWeb", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("SignContent", SignContent.Text.Trim()));
                list.Add(new SqlPara("SendBatchStr", SendBatchStr));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLSIGN_SEND_SJZS", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    //ds.Clear();
                    SignMan.Text = SignManCardID.Text = SignContent.Text = "";
                    SignDate.DateTime = CommonClass.gcdate;
                    SignOperator.Text = CommonClass.UserInfo.UserName;
                    simpleButton6_Click(null, null);
                    CommonSyn.TraceSyn(null, BillNoStr, 13, "送货签收", 1, null,null);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bedate", bedate.Text));
                list.Add(new SqlPara("enddate", enddate.Text));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_ReceiptSendList_sjzs", list);
                DataSet ds = SqlHelper.GetDataSet(spe);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl2.DataSource = ds.Tables[0];
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
        //回单寄出
        private void simpleButton3_Click(object sender, EventArgs e)
        {
           
            int rowhandle = myGridView2.FocusedRowHandle;
            if (rowhandle < 0)
            {
                MsgBox.ShowOK("请至少选择一条信息");
                return;
            }
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("OperateState", OperateState.Text.Trim()));
            list.Add(new SqlPara("Operator", Operator.Text.Trim()));
            list.Add(new SqlPara("OperateTime", OperateTime.Text.Trim()));
            string allBillNo = "";
            if (rowhandle >= 0)
            {
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    if (ConvertType.ToInt32(myGridView2.GetRowCellValue(i, "ischecked")) > 0)
                    {
                        allBillNo += myGridView2.GetRowCellValue(i, "BillNo") + ",";
                    }
                }
            }
            list.Add(new SqlPara("allBillNo", allBillNo.Trim()));
            list.Add(new SqlPara("OperateSite", CommonClass.UserInfo.SiteName));
            list.Add(new SqlPara("OperateWeb", CommonClass.UserInfo.WebName));
            list.Add(new SqlPara("RecBatch", Guid.NewGuid().ToString()));
            list.Add(new SqlPara("OperateRemark", ""));
            list.Add(new SqlPara("ToSite", ""));
            list.Add(new SqlPara("ToWeb", ""));
            list.Add(new SqlPara("SendNum", ""));
            list.Add(new SqlPara("ReceiptState", "回单寄出"));
            list.Add(new SqlPara("LinkTel", ""));
            SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_Receipt", list);
            if (SqlHelper.ExecteNonQuery(sps) > 0)
            {
                XtraMessageBox.Show("回单已寄出", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                simpleButton9_Click(null, null);
            }

        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Tool.GridOper.ExportToExcel(myGridView2, "回单签收");
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Tool.GridOper.ExportToExcel(myGridView1, "送货签收");
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView2);
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView2, myGridView2.Guid.ToString());
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView2, myGridView2.Guid.ToString());
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView2);
        }
       

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", dateEdit1.Text.Trim()));
                list.Add(new SqlPara("edate", dateEdit2.Text.Trim()));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_SEND_FOR_SJZSQS", list);
                DataSet ds = SqlHelper.GetDataSet(spe);
                myGridControl3.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }


        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView3.FocusedRowHandle;
                if (rowhandle < 0)
                {
                    MsgBox.ShowOK("请至少选择一条信息");
                    return;
                }
                Guid SignNO = new Guid(ConvertType.ToString(myGridView3.GetRowCellValue(rowhandle, "SignNO")));
                string billNo = myGridView3.GetRowCellValue(rowhandle, "BillNo").ToString();//LHD
                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SignNO", SignNO));
                list.Add(new SqlPara("SignType", "送货签收"));

                //提前获取到轨迹信息
                //List<SqlPara> lists = new List<SqlPara>();
                //lists.Add(new SqlPara("DepartureBatch", null));
                //lists.Add(new SqlPara("BillNO", billNo + "@"));
                //lists.Add(new SqlPara("tracetype", "送货签收"));
                //lists.Add(new SqlPara("num", 13));
                //DataSet dss = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_TraceSyn", lists));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_BILLSIGN", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    cbRetrieve_Click(null, null);
                   // CommonSyn.TraceSyn(null, billNo + "@", 13, "送货签收", 2, null, dss);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }

        private void barButtonItem17_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem16_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Tool.GridOper.ExportToExcel(myGridView2, "司机直送签收");
        }

        private void barButtonItem18_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView3);
        }

        private void barButtonItem19_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView3, myGridView3.Guid.ToString());
        }

        private void barButtonItem20_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView3, myGridView3.Guid.ToString());
        }

        private void barButtonItem21_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView3);
        }

      

        }

    
}