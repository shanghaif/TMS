using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System.IO;
using System.Data.OleDb;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using ZQTMS.UI.其他费用;

namespace ZQTMS.UI
{
    public partial class frmSendDetail : BaseForm
    {
        public frmSendDetail()
        {
            InitializeComponent();
        }
        public GridView gv;
        List<int> EditRows = new List<int>();
        DataSet dsCopy = new DataSet();
        string CSAccSends = "";
        public int IsSyn = 0;
        public decimal PaymentAmout = 0;
        public decimal costrate = 0;
        private void w_send_detail_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            CommonClass.GetGridViewColumns(myGridView2, myGridView3);
            GridOper.SetGridViewProperty(myGridView2, myGridView3);
            BarMagagerOper.SetBarPropertity(bar1); //如果有具体的工具条，就引用其实例
            FixColumn fix = new FixColumn(myGridView2, barSubItem2);
            PaymentAmout = ConvertType.ToDecimal(gv.GetFocusedRowCellValue("ActualFreight")); 
            GridOper.RestoreGridLayout(myGridView1, myGridView2);

            getdata();
            try
            {
                myGridControl3.DataSource = CommonClass.dsCar.Tables[0];
            }
            catch { }
        }

        private void getdata()
        {
            if (gv == null || gv.FocusedRowHandle < 0) return;

            string sendbatch = "";
            try
            {
                //填写送货信息
                sendbatch = ConvertType.ToString(gv.GetFocusedRowCellValue("SendBatch"));
                edinoneflag.Text = sendbatch;
                edvehicleno.Text = ConvertType.ToString(gv.GetFocusedRowCellValue("SendCarNO"));
                edsendman.Text = ConvertType.ToString(gv.GetFocusedRowCellValue("SendDriver"));
                edsendmantel.Text = ConvertType.ToString(gv.GetFocusedRowCellValue("SendDriverPhone"));
                edsenddate.DateTime = ConvertType.ToDateTime(gv.GetFocusedRowCellValue("SendDate"));
                edsendtosite.Text = ConvertType.ToString(gv.GetFocusedRowCellValue("SendToSite"));
                edsendtoweb.Text = ConvertType.ToString(gv.GetFocusedRowCellValue("SendToWeb"));
                edsendmadeby.Text = ConvertType.ToString(gv.GetFocusedRowCellValue("SendOperator"));
                edsendremark.Text = ConvertType.ToString(gv.GetFocusedRowCellValue("SendDesc"));

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SendBatch", sendbatch));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SEND_DETAIL", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl2.DataSource = ds.Tables[0];
                dsCopy = ds.Copy();

                List<SqlPara> list1 = new List<SqlPara>();
                list1.Add(new SqlPara("SendBatch", sendbatch));
                SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_SEND_TYPE", list1);
                DataSet ds1 = SqlHelper.GetDataSet(sps1);
                if (ds1 == null || ds1.Tables.Count == 0 || ds1.Tables[0].Rows.Count == 0)
                {
                    return;
                }
                else
                {
                    IsSyn = Convert.ToInt32(ds1.Tables[0].Rows[0]["IsSyn"]);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
            finally
            {
                if (myGridView2.RowCount < 1000) myGridView2.BestFitColumns();
            }
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1, myGridView2);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView2);
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString(), myGridView2.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1, myGridView2);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView2, "送货清单");
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SEND_LOAD", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;

                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (myGridView1.RowCount < 1000) myGridView1.BestFitColumns();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "送货库存");
        }

        private void cbadd_Click(object sender, EventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;

            string billno = GridOper.GetRowCellValueString(myGridView1, rowhandle, "BillNo");
            //309/490公司限制已审核不能做加入的操作
            string sendaduitstate = GridOper.GetRowCellValueString(myGridView1, rowhandle, "SendAduitState");
            if (CommonClass.UserInfo.companyid == "309" || CommonClass.UserInfo.companyid == "490")
            {
                if (sendaduitstate == "1")
                {
                    MsgBox.ShowOK("已审核不可加入，如需加入请先反审核！");
                    return;
                }
            }
            if (CommonClass.CheckKongHuo(myGridView1, 2))
            {
                MsgBox.ShowOK("选择的清单包含控货的运单,不能送货!");
                return;
            }
            if (XtraMessageBox.Show("确定将选中的运单加入到本车清单吗?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            myGridView1.PostEditor();
            int[] rows = myGridView1.GetSelectedRows();
            if (rows.Length == 0) return;

            string billnos = "", accsends = "", sendpcss = "", sendtosite = "", sendtoweb = "", IdStr = "";
            sendtosite = edsendtosite.Text.Trim();
            sendtoweb = edsendtoweb.Text.Trim();
            if (CheckInOneVehicle(ref billnos))
            {
                XtraMessageBox.Show("本车中已含有该票，不能同一票拆开，然后又装入同一车中。\r\n请先在本车中剔除该票，然后重新调入库存，再加入本车。\r\n本车中已存在的运单：\r\n" + billnos, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            billnos = "";
            string billnosLMS = "", billnosZQTMS = "", sendpcssLMS = "", sendpcssZQTMS = "", accsendsLMS = "", accsendsZQTMS = "", IdStrLMS = "", IdStrZQTMS = "";
            for (int i = 0; i < rows.Length; i++)
            {
                if (rows[i] < 0) continue;
                if (sendtosite == "" && ConvertType.ToString(myGridView1.GetRowCellValue(rows[i], "TransferMode")) == "自提") continue;
                billnos += GridOper.GetRowCellValueString(myGridView1, rows[i], "BillNo") + ",";
                accsends += ConvertType.ToDecimal(myGridView1.GetRowCellValue(rows[i], "AccSend")) + ",";
                sendpcss += ConvertType.ToInt32(myGridView1.GetRowCellValue(rows[i], "sendqty")) + ",";
                IdStr += ConvertType.ToString(myGridView1.GetRowCellValue(rows[i], "Id")) + ",";
                if (Convert.ToString(myGridView1.GetRowCellValue(i, "SystemSource")) == "LMS")
                {
                    billnosLMS += GridOper.GetRowCellValueString(myGridView1, rows[i], "BillNo") + ",";
                    sendpcssLMS += ConvertType.ToInt32(myGridView1.GetRowCellValue(rows[i], "sendqty")) + ",";
                    accsendsLMS += ConvertType.ToDecimal(myGridView1.GetRowCellValue(rows[i], "AccSend")) + ",";
                    IdStrLMS += ConvertType.ToString(myGridView1.GetRowCellValue(rows[i], "Id")) + ",";
                }
                else
                {
                    billnosZQTMS += GridOper.GetRowCellValueString(myGridView1, rows[i], "BillNo") + ",";
                    sendpcssZQTMS += ConvertType.ToInt32(myGridView1.GetRowCellValue(rows[i], "sendqty")) + ",";
                    accsendsZQTMS += ConvertType.ToDecimal(myGridView1.GetRowCellValue(rows[i], "AccSend")) + ",";
                    IdStrZQTMS += ConvertType.ToString(myGridView1.GetRowCellValue(rows[i], "Id")) + ",";
                }
            }
            if (billnos == "")
            {
                if (sendtosite == "")
                {
                    MsgBox.ShowOK("本车是送货上门的，请注意选择加货的运单不能是自提的!");
                }
                return;
            }

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("SendBatch", edinoneflag.Text.Trim()));
            list.Add(new SqlPara("SendDate", edsenddate.DateTime));
            list.Add(new SqlPara("billnos", billnos));
            list.Add(new SqlPara("AccSends", accsends));
            list.Add(new SqlPara("SendPCSs", sendpcss));
            list.Add(new SqlPara("type", 1));
            list.Add(new SqlPara("IdStr", IdStr));
            try
            {
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, sendtosite == "" ? "USP_ADD_SEND" : "USP_ADD_SEND_TOSITE", list)) == 0) return;
                CommonClass.SetOperLog(edinoneflag.Text.Trim(), "", "", CommonClass.UserInfo.UserName, "送货加货", "加入运单:" + billnos);
                myGridView1.DeleteSelectedRows();
                getdata();
                USP_ADD_SENDACCDETAIL(edinoneflag.Text.Trim());
                if (sendtosite != "")//maohui20181114
                {
                    if (IsSyn == 1)
                    {
                        if (billnosLMS != "")
                        {
                            List<SqlPara> listSYN = new List<SqlPara>();
                            listSYN.Add(new SqlPara("billnosLMS", billnosLMS));
                            listSYN.Add(new SqlPara("AcceptSiteName", edsendtosite.Text.Trim()));
                            listSYN.Add(new SqlPara("AcceptWebName", edsendtoweb.Text.Trim()));
                            listSYN.Add(new SqlPara("CarNo", edvehicleno.Text.Trim()));
                            listSYN.Add(new SqlPara("DriverName", edsendman.Text.Trim()));
                            listSYN.Add(new SqlPara("DriverPhone", edsendmantel.Text.Trim()));
                            listSYN.Add(new SqlPara("Remark", edsendremark.Text.Trim()));
                            listSYN.Add(new SqlPara("Batch", edinoneflag.Text.Trim()));
                            listSYN.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));
                            CommonSyn.LMSSynZQTMS(listSYN, "转二级运单数据同步", "UAP_ADD_FB_ONWaybill_SendToSite");
                        }
                        //CommonSyn.LMSSynZQTMS(list, "转二级补货", "UAP_ADD_FB_ON_SendToSite");
                        //yzw 转二级补货同步
                        CommonSyn.SEND_TOSITE_SYN(edsendtoweb.Text.Trim(), edsenddate.DateTime, billnos);
                    }
                }
                //CommonSyn.BillSendGoodsSyn(billnos, edinoneflag.Text.Trim());//zaj 2018-4-10 分拨同步
                //yzw 单票加入同步
                CommonSyn.USP_ADD_SEND_3_SYN(billnos, edvehicleno.Text.Trim(), edsenddate.DateTime);
                //CommonSyn.TimeSendUptSyn(billnos, CommonClass.UserInfo.WebName, SendToWeb, "USP_ADD_SEND_TOSITE");//同步送货修改时效 LD 2018-4-27
                //CommonSyn.TraceSyn(null, billnos.Replace(",", "@"), 12, "送货上门", 1, null, null);
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private bool CheckInOneVehicle(ref string units)
        {//gridView2清单
            int[] rows = myGridView1.GetSelectedRows();
            string billno = "";
            for (int i = 0; i < rows.Length; i++)
            {
                billno = ConvertType.ToString(myGridView1.GetRowCellValue(rows[i], "Billno"));
                for (int j = 0; j < myGridView2.RowCount; j++)
                {
                    if (ConvertType.ToString(myGridView2.GetRowCellValue(j, "Billno")) == billno)
                    {
                        units += billno.ToString() + ",";
                    }
                }
            }
            units = units.TrimEnd(',');
            return units == "" ? false : true;
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private bool CheckHexiao(ref string result)
        {
            return false;
        }

        private void cbdeletesingle_Click(object sender, EventArgs e)
        {
            int rowhandle = myGridView2.FocusedRowHandle;
            if (rowhandle < 0) return;

            string billno = GridOper.GetRowCellValueString(myGridView2, rowhandle, "BillNo");
            //309/490公司限制已审核不能做取消单票的操作
            string sendaduitstate = GridOper.GetRowCellValueString(myGridView2, rowhandle, "SendAduitState");
            if (CommonClass.UserInfo.companyid == "309" || CommonClass.UserInfo.companyid == "490")
            {
                if (sendaduitstate == "1")
                {
                    MsgBox.ShowOK("已审核不可取消单票，如需修改请先反审核！");
                    return;
                }
            }
            if (CommonClass.QSP_LOCK_4(billno, edsenddate.Text.Trim()))
            {
                return;
            }
            if (myGridView2.RowCount == 1)
            {
                if (DialogResult.Yes != MsgBox.ShowYesNo("本车只剩下最后一票，剔除后将自动作废，是否确认操作？"))
                {
                    return;
                }
            }
            if (MsgBox.ShowYesNo("确定剔除选中项?") != DialogResult.Yes) return;
            string sendBatch = edinoneflag.Text.Trim();
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillNo", billno));
            list.Add(new SqlPara("SendBatch", edinoneflag.Text.Trim()));
            list.Add(new SqlPara("SendPCS", ConvertType.ToInt32(myGridView2.GetRowCellValue(rowhandle, "SendPCS"))));
            list.Add(new SqlPara("SendNUM", ConvertType.ToInt32(myGridView2.GetRowCellValue(rowhandle, "SendNUM"))));
            list.Add(new SqlPara("SendDepartureListNo", GridOper.GetRowCellValueString(myGridView2, rowhandle, "SendDepartureListNo")));
            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_DELETE_SEND", list)) == 0) return;
            myGridView2.DeleteRow(rowhandle);
            MsgBox.ShowOK();
            if (myGridView2.RowCount == 0)
            {
                this.Close();
            }
            //CommonSyn.LMSSynZQTMS(list, "送货单票剔除", "USP_DELETE_SEND_Clone_FB");//maohui20181114
            //CommonSyn.BillSendCancelSyn(sendBatch, billno + "@", 1);//zaj 2018-4-11 分拨同步
            CommonSyn.DELETE_SEND_SYN(billno);//yzw 送货单票剔除
        }

        private void cbdeleteall_Click(object sender, EventArgs e)
        {
            int rowhandle = myGridView2.FocusedRowHandle;
            if (rowhandle < 0) return;

            string billno = GridOper.GetRowCellValueString(myGridView2, rowhandle, "BillNo");
            //309/490公司限制已审核不能做取消整车的操作
            string sendaduitstate = GridOper.GetRowCellValueString(myGridView2, rowhandle, "SendAduitState");
            if (CommonClass.UserInfo.companyid == "309" || CommonClass.UserInfo.companyid == "490")
            {
                if (sendaduitstate == "1")
                {
                    MsgBox.ShowOK("已审核不可取消整车，如需修改请先反审核！");
                    return;
                }
            }
            string result = "";
            if (CheckHexiao(ref result))
            {
                XtraMessageBox.Show("本车有些运单实际送货费已经经过财务销账，不能取消送货。必须经过财务反核销，然后才能取消送货!\r\n\r\n已经销账的运单号为：" + result, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MsgBox.ShowYesNo("将要取消送货，取消送货会以下情况：\n\r\n\r①该票货物会返回到库存，以后可以再送!\n\r\n\r确认进行吗？注意：系统会记录操作人!") != DialogResult.Yes) return;

            progressBar1.Visible = true;
            progressBar1.Maximum = myGridView2.RowCount;
            this.Enabled = false;
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                if (CommonClass.QSP_LOCK_4(GridOper.GetRowCellValueString(myGridView2, i, "BillNo"), edsenddate.Text.Trim()))
                {
                    return;
                }
            }

            List<SqlPara> list = new List<SqlPara>();
            string sendBatch = edinoneflag.Text.Trim();
            try
            {
                string BillNos = "";
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    BillNos = BillNos + GridOper.GetRowCellValueString(myGridView2, i, "BillNo").ToString() + "@";
                    list.Clear();
                    list.Add(new SqlPara("BillNo", GridOper.GetRowCellValueString(myGridView2, i, "BillNo")));
                    list.Add(new SqlPara("SendBatch", edinoneflag.Text.Trim()));
                    list.Add(new SqlPara("SendPCS", ConvertType.ToInt32(myGridView2.GetRowCellValue(i, "SendPCS"))));
                    list.Add(new SqlPara("SendNUM", ConvertType.ToInt32(myGridView2.GetRowCellValue(i, "SendNUM"))));
                    list.Add(new SqlPara("SendDepartureListNo", GridOper.GetRowCellValueString(myGridView2, i, "SendDepartureListNo")));
                    SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "USP_DELETE_SEND", list);
                    if (SqlHelper.ExecteNonQuery(spe) > 0)
                    {
                        CommonSyn.DELETE_SEND_SYN(GridOper.GetRowCellValueString(myGridView2, i, "BillNo"));
                        progressBar1.Value = i + 1;
                        Application.DoEvents();
                    }
                  
                    //CommonSyn.LMSSynZQTMS(list, "送货整车作废", "USP_DELETE_SEND_Clone_FB");//maohui20181114
                    //yzw  取消送货
                   

                   
                }
                gv.DeleteSelectedRows();
                MsgBox.ShowOK("整车作废成功!");
                this.Close();
                //CommonSyn.BillSendCancelSyn(sendBatch, BillNos, 0);//zaj 2018-4-11 分拨同步
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
            finally
            {
                this.Enabled = true;
                progressBar1.Visible = false;
            }
        }

        private void cbprint_Click(object sender, EventArgs e)
        {
            string sendBatch = edinoneflag.Text.Trim();
            if (sendBatch == "") return;

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_SEND_DETAIL_PRINT", new List<SqlPara> { new SqlPara("SendBatch", sendBatch) }));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                row["NowCompany"] = CommonClass.UserInfo.gsqc;
            }

            string shqd = "送货清单";
            if (File.Exists(Application.StartupPath + "\\Reports\\" + "送货清单" + "per.grf"))
            {
                shqd = shqd + "per";
            }

            frmPrintRuiLang fpr = new frmPrintRuiLang(shqd, ds);
            fpr.ShowDialog();
            USP_ADD_SENDACCDETAIL(sendBatch);
            if (IsSyn == 1)
            {
                List<SqlPara> listLMS = new List<SqlPara>();
                listLMS.Add(new SqlPara("SendBatch", sendBatch));
                listLMS.Add(new SqlPara("LoginAreaName", CommonClass.UserInfo.AreaName));
                listLMS.Add(new SqlPara("LoginCauseName", CommonClass.UserInfo.CauseName));
                listLMS.Add(new SqlPara("LoginDepartName", CommonClass.UserInfo.DepartName));
                listLMS.Add(new SqlPara("LoginSiteName", CommonClass.UserInfo.SiteName));
                listLMS.Add(new SqlPara("LoginWebName", CommonClass.UserInfo.WebName));
                listLMS.Add(new SqlPara("LoginUserAccount", CommonClass.UserInfo.UserAccount));
                listLMS.Add(new SqlPara("LoginUserName", CommonClass.UserInfo.UserName));
                CommonSyn.LMSSynZQTMS(listLMS, "转送到加盟二级网点扣费", "USP_ADD_SENDACCDETAIL_LMSSynZQTMS");
            }
        }

        public static void USP_ADD_SENDACCDETAIL(string SendBatch)
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("SendBatch", SendBatch));
            SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_SENDACCDETAIL", list));
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            int rowhandle = myGridView2.FocusedRowHandle;
            if (rowhandle < 0) return;

            string billno = GridOper.GetRowCellValueString(myGridView2, rowhandle, "BillNo");
            //309/490公司限制已审核不能做取消单票的操作
            string sendaduitstate = GridOper.GetRowCellValueString(myGridView2, rowhandle, "SendAduitState");
            if (CommonClass.UserInfo.companyid == "309" || CommonClass.UserInfo.companyid == "490")
            {
                if (sendaduitstate == "1")
                {
                    MsgBox.ShowOK("已审核不可修改送货费，如需修改请先反审核！");
                    return;
                }
            }
            string result = "";
            if (CheckHexiao(ref result))
            {
                XtraMessageBox.Show("本车有些运单实际送货费已经经过财务销账，不能再调整实际送货费。必须经过财务反核销，然后才能调整!\r\n\r\n已经销账的运单号为：\r\n" + result, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            simpleButton7.Enabled = true;
            GridColumn gc = myGridView2.Columns["AccSend"];
            
            gc.OptionsColumn.AllowEdit = gc.OptionsColumn.AllowFocus = true;
            myGridView2.FocusedColumn = gc;
        }

        public bool isDeficit(decimal ActualFreight, decimal AccSend)
        {
            try
            {
                bool ischeck = false;
                costrate = Math.Round(AccSend / ActualFreight, 2);
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SendWebName", CommonClass.UserInfo.WebName.ToString().Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_CostControlsList_rate_2", list);
                DataSet ds_check = SqlHelper.GetDataSet(sps);
                if (ds_check != null && ds_check.Tables.Count > 0 && ds_check.Tables[0].Rows.Count > 0)
                {
                    if (costrate > Convert.ToDecimal(ds_check.Tables[0].Rows[0]["TargetcostRate"].ToString()))
                    {


                        ischeck = true;
                    }


                }
                return ischeck;
            }

            catch (Exception)
            {

                throw;
            }
        }

        public void Save_485()
        {
            myGridView2.PostEditor();
            if (EditRows.Count == 0)
            {
                MsgBox.ShowOK("没有修改送货费!");
                return;
            }

            string billnos = "", AccSends = "";
            try
            {


                GridColumn gc = myGridView2.Columns["AccSend"];
                decimal accsend_lod = 0, accsend_new = 0;
                for (int i = 0; i < EditRows.Count; i++)
                {
                    billnos += ConvertType.ToString(myGridView2.GetRowCellValue(EditRows[i], "BillNo")) + ",";
                    AccSends += ConvertType.ToDecimal(myGridView2.GetRowCellValue(EditRows[i], gc)) + ",";
                    if (CommonClass.QSP_LOCK_4(myGridView2.GetRowCellValue(EditRows[i], "BillNo").ToString(), edsenddate.Text.Trim()))
                    {
                        return;
                    }

                }

                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    accsend_new += ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "AccSend"));
                }
                CSAccSends = "";
                for (int i = 0; i < dsCopy.Tables[0].Rows.Count; i++)
                {
                    accsend_lod += ConvertType.ToDecimal(dsCopy.Tables[0].Rows[i]["AccSend"]);
                    CSAccSends += ConvertType.ToDecimal(dsCopy.Tables[0].Rows[i]["AccSend"]) + ",";
                }

                if (isDeficit(PaymentAmout, accsend_new))
                {
                    frmIsCostDeficits Cost = new frmIsCostDeficits();
                    Cost.DepartureBatch = edinoneflag.Text.Trim();
                    Cost.SendWebName = CommonClass.UserInfo.WebName.ToString().Trim();
                    Cost.actual_rate = costrate;
                    Cost.MenuType = "送货亏损";
                    Cost.ShowDialog();
                    if (Cost.isprint == true)
                    {

                        if (DialogResult.Yes == MsgBox.ShowYesNo("本批次号送货费" + accsend_lod + "元调整为" + accsend_new + "元，是否确认保存？"))
                        {
                            if (billnos == "") return;
                            List<SqlPara> list = new List<SqlPara>();
                            list.Add(new SqlPara("sendbatch", edinoneflag.Text.Trim()));
                            list.Add(new SqlPara("billnos", billnos));
                            list.Add(new SqlPara("accsends", AccSends));
                            list.Add(new SqlPara("CSAccSends", CSAccSends));//初始送货费 hj20180724
                            SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_MODIFIED_ACCSEND_BY_BATCH", list));
                            gc.OptionsColumn.AllowEdit = gc.OptionsColumn.AllowFocus = false;
                            EditRows.Clear();
                            simpleButton7.Enabled = false;
                            MsgBox.ShowOK("保存成功");
                            list.Remove(new SqlPara("CSAccSends", CSAccSends));
                            list.Add(new SqlPara("accsend_new", accsend_new));
                            CommonSyn.LMSSynZQTMS(list, "送货费用调整", "USP_MODIFIED_ACCSEND_BY_BATCH2_FB");//maohui20181114
                        }
                    }
                }
                else
                {
                    if (DialogResult.Yes == MsgBox.ShowYesNo("本批次号送货费" + accsend_lod + "元调整为" + accsend_new + "元，是否确认保存？"))
                    {
                        if (billnos == "") return;
                        List<SqlPara> list = new List<SqlPara>();
                        list.Add(new SqlPara("sendbatch", edinoneflag.Text.Trim()));
                        list.Add(new SqlPara("billnos", billnos));
                        list.Add(new SqlPara("accsends", AccSends));
                        list.Add(new SqlPara("CSAccSends", CSAccSends));//初始送货费 hj20180724
                        SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_MODIFIED_ACCSEND_BY_BATCH", list));
                        gc.OptionsColumn.AllowEdit = gc.OptionsColumn.AllowFocus = false;
                        EditRows.Clear();
                        simpleButton7.Enabled = false;
                        MsgBox.ShowOK("保存成功");
                        list.Remove(new SqlPara("CSAccSends", CSAccSends));
                        list.Add(new SqlPara("accsend_new", accsend_new));
                        CommonSyn.LMSSynZQTMS(list, "送货费用调整", "USP_MODIFIED_ACCSEND_BY_BATCH2_FB");//maohui20181114
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }
        

        
        public void Save_486()
        {
            myGridView2.PostEditor();
            if (EditRows.Count == 0)
            {
                MsgBox.ShowOK("没有修改送货费!");
                return;
            }

            string billnos = "", AccSends = "";
            try
            {
                GridColumn gc = myGridView2.Columns["AccSend"];
                decimal accsend_lod = 0, accsend_new = 0;
                for (int i = 0; i < EditRows.Count; i++)
                {
                    billnos += ConvertType.ToString(myGridView2.GetRowCellValue(EditRows[i], "BillNo")) + ",";
                    AccSends += ConvertType.ToDecimal(myGridView2.GetRowCellValue(EditRows[i], gc)) + ",";
                    if (CommonClass.QSP_LOCK_4(myGridView2.GetRowCellValue(EditRows[i], "BillNo").ToString(), edsenddate.Text.Trim()))
                    {
                        return;
                    }

                }
                //for (int i = 0; i < myGridView2.RowCount; i++)
                //{
                //    CSAccSends += ConvertType.ToDecimal(dsCopy.Tables[0].Rows[i]["AccSend"]) + ",";
                //}

                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    accsend_new += ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "AccSend"));
                }
                CSAccSends = "";
                for (int i = 0; i < dsCopy.Tables[0].Rows.Count; i++)
                {
                    accsend_lod += ConvertType.ToDecimal(dsCopy.Tables[0].Rows[i]["AccSend"]);
                    CSAccSends += ConvertType.ToDecimal(dsCopy.Tables[0].Rows[i]["AccSend"]) + ",";
                }
                if (DialogResult.Yes == MsgBox.ShowYesNo("本批次号送货费" + accsend_lod + "元调整为" + accsend_new + "元，是否确认保存？"))
                {
                    if (billnos == "") return;
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("sendbatch", edinoneflag.Text.Trim()));
                    list.Add(new SqlPara("billnos", billnos));
                    list.Add(new SqlPara("accsends", AccSends));
                    list.Add(new SqlPara("CSAccSends", CSAccSends));//初始送货费 hj20180724
                    SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_MODIFIED_ACCSEND_BY_BATCH", list));
                    gc.OptionsColumn.AllowEdit = gc.OptionsColumn.AllowFocus = false;
                    EditRows.Clear();
                    simpleButton7.Enabled = false;
                    MsgBox.ShowOK("保存成功");
                    list.Remove(new SqlPara("CSAccSends", CSAccSends));
                    list.Add(new SqlPara("accsend_new", accsend_new));
                    CommonSyn.LMSSynZQTMS(list, "送货费用调整", "USP_MODIFIED_ACCSEND_BY_BATCH2_FB");//maohui20181114
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }

        }
        private void simpleButton7_Click(object sender, EventArgs e)
        {
            if (CommonClass.UserInfo.companyid == "485")
            {
                Save_485();
            }
            
            else
            {
                Save_486();

            }

        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            myGridView2.ClearColumnsFilter();
            if (textEdit3.Text.Trim() == "")
            {
                XtraMessageBox.Show("请输入整车送货费!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textEdit3.Focus();
                return;
            }

            try
            {
                int qtytotal = Convert.ToInt32(myGridView2.Columns["qty"].SummaryItem.SummaryValue);
                int type = radioGroup1.SelectedIndex;
                int acctotal = Convert.ToInt32(textEdit3.Text.Trim());

                if (type == 0)
                {//按件数分摊
                    for (int i = 0; i < myGridView2.RowCount; i++)
                    {

                    }
                    //barEditItem4.EditValue = "按件数";
                }
                else if (type == 1)
                {
                    int a = 0;

                    for (int i = 0; i < myGridView2.RowCount; i++)
                    {

                    }
                    //barEditItem4.EditValue = "按运费";
                }
                else
                {
                    decimal avg = Math.Floor(acctotal / Convert.ToDecimal(myGridView2.RowCount));
                    for (int i = 0; i < myGridView2.RowCount; i++)
                    {
                        if (i == myGridView2.RowCount - 1)
                        {
                            myGridView2.SetRowCellValue(i, "accsendout", acctotal - avg * (myGridView2.RowCount - 1));
                        }
                        else
                        {
                            myGridView2.SetRowCellValue(i, "accsendout", avg);
                        }
                    }
                    //barEditItem4.EditValue = "按票";
                }
                //gridColumn92.OptionsColumn.AllowEdit = false;
                //gridColumn92.OptionsColumn.AllowFocus = false;
                myGridView2.UpdateSummary();
                panelControl3.Visible = false;
                XtraMessageBox.Show("分摊成功!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                myGridView2.SetRowCellValue(i, "accsendout", 0);
            }
            //gridColumn92.OptionsColumn.AllowEdit = false;
            //gridColumn92.OptionsColumn.AllowFocus = false;
            panelControl3.Visible = false;
            simpleButton7.Enabled = false;
            XtraMessageBox.Show("取消分摊成功!实际送货费已清零!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            panelControl3.Visible = false;
        }

        private void myGridControl2_DoubleClick(object sender, EventArgs e)
        {

        }

        private void myGridControl1_DoubleClick(object sender, EventArgs e)
        {

        }

        private void panelControl3_Leave(object sender, EventArgs e)
        {
            textEdit3.Focus();
            if (!panelControl3.Focused)
            {
                panelControl3.Visible = false;
            }
        }

        private void myGridView1_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            try
            {
                GridColumn gc = ((DevExpress.XtraGrid.Views.Grid.GridView)sender).FocusedColumn;
                if (e == null || gc == null || gc.FieldName != "sendqty") return;
                int oldvalue = ConvertType.ToInt32(myGridView1.GetFocusedRowCellValue("sendremainqty"));//库存件数
                int newvalue = ConvertType.ToInt32(e.Value);//填的件数
                if (newvalue <= 0)
                {
                    e.Valid = false;
                    e.ErrorText = "实发件数必须大于0！";
                }
                else if (newvalue > oldvalue)
                {
                    e.Valid = false;
                    e.ErrorText = string.Format("实发件数不能大于库存件数(当前库存{0}件)！", oldvalue);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void myGridView1_InvalidValueException(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            MsgBox.ShowError(e.ErrorText);
        }

        private void barbtnPrintAll_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView2.RowCount == 0)
            {
                MsgBox.ShowOK("没有运单，不能打印!");
                return;
            }
            string str = "";
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                str += ConvertType.ToString(myGridView2.GetRowCellValue(i, "BillNo")) + "@";
            }
            PrintQSD(str);
            //cc.RegReport();
            //SqlConnection Conn = cc.GetConn();
            //int isprint = 0; //当为1时打印全部，0时停止所有打印
            //for (int i = 0; i < myGridView2.RowCount; i++)
            //{
            //    try
            //    {
            //        DataSet dsPrint = new DataSet();
            //        //grproLib.GridppReport Report = new grproLib.GridppReport();
            //        int iunit = Convert.ToInt32(myGridView2.GetRowCellValue(i, "unit"));

            //        SqlCommand sqlcmd = new SqlCommand("QSP_GET_FETCH_FOR_PRINT_RL", Conn);
            //        sqlcmd.CommandType = CommandType.StoredProcedure;
            //        sqlcmd.Parameters.AddWithValue("@unit", iunit);
            //        sqlcmd.Parameters.AddWithValue("@webid", commonclass.webid);
            //        //SqlDataAdapter sqlda = new SqlDataAdapter(sqlcmd);
            //        sqlda.Fill(dsPrint);

            //        Report.LoadFromFile(Application.StartupPath + "\\提货单(套打).grf");
            //        //Report.Printer.PaperLength = commonclass.thdh;
            //        //Report.Printer.PaperWidth = commonclass.thdw;
            //        //Report.Printer.PaperSize = 256;
            //        //Report.Title = commonclass.reptitle + "客户签收单";
            //        Report.LoadDataFromXML(dsPrint.GetXml());
            //        if (isprint == 0) isprint = Report.Printer.PrintDialog() ? 1 : 0;
            //        if (isprint == 1)
            //        {
            //            Report.Print(false);
            //        }
            //        else
            //        {
            //            break;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        break;
            //    }
            //}
        }

        private void ddbtnPrintQSD_Click(object sender, EventArgs e)
        {
            ddbtnPrintQSD.ShowDropDown();
        }

        private void myGridView2_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e == null || e.RowHandle < 0 || EditRows.Contains(e.RowHandle)) return;
            EditRows.Add(e.RowHandle);
        }

        private void barbtnPrintQSD_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int[] rows = myGridView2.GetSelectedRows();
            if (rows.Length == 0)
            {
                MsgBox.ShowOK("请选择要打印的运单!");
                return;
            }
            string str = "";
            for (int i = 0; i < rows.Length; i++)
            {
                str += ConvertType.ToString(myGridView2.GetRowCellValue(rows[i], "BillNo")) + "@";
            }
            PrintQSD(str);
        }

        private void PrintQSD(string BillNoStr)
        {
            if (string.IsNullOrEmpty(BillNoStr)) return;
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILLINFO_PRINT_QSD", new List<SqlPara> { new SqlPara("BillNoStr", BillNoStr) }));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                MsgBox.ShowError("没有找到选中的运单信息,打印失败,(请检查网络或运单是否已被删除)!");
                return;
            }
            DataTable dt = ds.Tables[0].Clone();
            frmPrintRuiLang fprl;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                dt.ImportRow(ds.Tables[0].Rows[i]);

            }
            fprl = new frmPrintRuiLang("套打签收单", dt);
            fprl.ShowDialog();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            try
            {
                if (edvehicleno.Text.Trim() == "")
                {
                    MsgBox.ShowError("送货车号必须填写!");
                    edvehicleno.Focus();
                    return;

                }

                if (edsendman.Text.Trim() == "")
                {
                    MsgBox.ShowError("送货司机必须填写!");
                    edsendman.Focus();
                    return;

                }

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SendBatch", edinoneflag.Text.Trim()));
                list.Add(new SqlPara("SendCarNo", edvehicleno.Text.Trim()));
                list.Add(new SqlPara("SendDriver", edsendman.Text.Trim()));
                list.Add(new SqlPara("SendDriverPhone", edsendmantel.Text.Trim()));
                list.Add(new SqlPara("SendDesc", edsendremark.Text.Trim()));

                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_SEND_NEW", list)) == 0) return;

                MsgBox.ShowOK();
                this.Close();

            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }

        }
        private void myGridControl3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                myGridControl3.Visible = false;
            }
            if (e.KeyCode == Keys.Enter)
            {
                SetCarInfo();
            }
        }

        private void myGridView3_DoubleClick(object sender, EventArgs e)
        {
            SetCarInfo();
        }

        private void SetCarInfo()
        {
            int rowhandle = myGridView3.FocusedRowHandle;
            if (rowhandle < 0) return;

            DataRow dr = myGridView3.GetDataRow(rowhandle);
            if (dr == null) return;

            myGridControl3.Visible = false;
            edvehicleno.EditValue = dr["CarNo"];
            edsendman.EditValue = dr["DriverName"];
            edsendmantel.EditValue = dr["DriverPhone"];
        }

        private void myGridControl3_Leave(object sender, EventArgs e)
        {
            myGridControl3.Visible = edvehicleno.Focused;
        }
        private void myGridControl3_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }





        private void edvehicleno_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            {
                if (e.NewValue == null) return;
                string value = e.NewValue.ToString();
                myGridView3.Columns["CarNo"].FilterInfo = new ColumnFilterInfo(
                        "[CarNo] LIKE " + "'%" + value + "%'"
                        + " OR [DriverName] LIKE" + "'%" + value + "%'"
                        + " OR [DriverPhone] LIKE" + "'%" + value + "%'",
                        "");
            }
        }

        private void edvehicleno_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Down)
                myGridControl3.Focus();
            if (e.KeyCode == Keys.Escape)
            {
                myGridControl3.Visible = false;
            }

        }

        private void edvehicleno_Enter(object sender, EventArgs e)
        {

            myGridControl3.Left = edvehicleno.Left;
            myGridControl3.Top = edvehicleno.Top + edvehicleno.Height + 30;
            myGridControl3.Visible = true;

        }

        private void edvehicleno_Leave(object sender, EventArgs e)
        {
            myGridControl3.Visible = myGridControl3.Focused;
        }
    }
}