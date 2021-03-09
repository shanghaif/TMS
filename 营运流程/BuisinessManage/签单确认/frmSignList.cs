using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class frmSignList : BaseForm
    {
        public int num = 0;
        public frmSignList()
        {
            InitializeComponent();
        }

        private void btnFetch_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmBatchFetchSign frm = frmBatchFetchSign.Get_frmBatchFetchSign;
            frm.MdiParent = this.MdiParent;
            frm.Show();
            frm.Focus();
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            getData();
        }

        private void getData()
        {
             if (ConvertType.ToString(type.EditValue) == "自提签收")
            {
             
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("begDate", bdate.Text.Trim()));
                list.Add(new SqlPara("endDate", edate.Text.Trim()));
                list.Add(new SqlPara("bsite", bsite.Text.Trim() == "全部" ? "%%" : bsite.Text.Trim()));
                list.Add(new SqlPara("esite", esite.Text.Trim() == "全部" ? "%%" : esite.Text.Trim()));
                list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text));
                list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text));
                //if (CommonClass.IsZhanQuCompanyId(CommonClass.UserInfo.companyid))//战区查询
                //{
                //    list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));
                //}
                //else
                //{
                    list.Add(new SqlPara("WebName", WebName.Text.Trim() == "全部" ? "%%" : WebName.Text.Trim()));
                //}

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_FETCH_SIGN", list);
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
             else if (ConvertType.ToString(type.EditValue) == "送货签收")       //zhengjiafeng20180910
             {
                 try
                 {
                     List<SqlPara> list = new List<SqlPara>();
                     list.Add(new SqlPara("begDate", bdate.Text.Trim()));
                     list.Add(new SqlPara("endDate", edate.Text.Trim()));
                     list.Add(new SqlPara("bsite", bsite.Text.Trim() == "全部" ? "%%" : bsite.Text.Trim()));
                     list.Add(new SqlPara("esite", esite.Text.Trim() == "全部" ? "%%" : esite.Text.Trim()));
                     list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text));
                     list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text));
                     //if (CommonClass.IsZhanQuCompanyId(CommonClass.UserInfo.companyid))//战区查询
                     //{
                     //    list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));
                     //}
                     //else
                     //{
                         list.Add(new SqlPara("WebName", WebName.Text.Trim() == "全部" ? "%%" : WebName.Text.Trim()));
                     //}
                     SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SEND_SIGN", list);
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
             else if (ConvertType.ToString(type.EditValue) == "中转签收")       //zhengjiafeng20180910
             {
                 try
                 {
                     List<SqlPara> list = new List<SqlPara>();
                     list.Add(new SqlPara("begDate", bdate.Text.Trim()));
                     list.Add(new SqlPara("endDate", edate.Text.Trim()));
                     //list.Add(new SqlPara("bsite", bsite.Text.Trim() == "全部" ? "%%" : bsite.Text.Trim()));
                     list.Add(new SqlPara("esite", esite.Text.Trim() == "全部" ? "%%" : esite.Text.Trim()));
                     //list.Add(new SqlPara("MiddleType", isLocal.SelectedIndex));
                     list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text));
                     list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text));
                     //if (CommonClass.IsZhanQuCompanyId(CommonClass.UserInfo.companyid))//战区查询
                     //{
                     //    list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));
                     //}
                     //else
                     //{
                         list.Add(new SqlPara("WebName", WebName.Text.Trim() == "全部" ? "%%" : WebName.Text.Trim()));
                     //}

                     SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_MIDDLE_SIGN1", list);
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
             else if (ConvertType.ToString(type.EditValue) == "司机直送签收")       //zhengjiafeng20180910
             {
                 try
                 {
                     List<SqlPara> list = new List<SqlPara>();
                     list.Add(new SqlPara("bdate", bdate.Text));
                     list.Add(new SqlPara("edate", edate.Text));
                     SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_SEND_FOR_SJZSQS", list);
                     DataSet ds = SqlHelper.GetDataSet(spe);

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
             else if (ConvertType.ToString(type.EditValue) == "异常签收")       //zhengjiafeng20180910
             {
                 try
                 {
                     List<SqlPara> list = new List<SqlPara>();
                     list.Add(new SqlPara("begDate", bdate.Text.Trim()));
                     list.Add(new SqlPara("endDate", edate.Text.Trim()));
                     list.Add(new SqlPara("bsite", bsite.Text.Trim() == "全部" ? "%%" : bsite.Text.Trim()));
                     list.Add(new SqlPara("esite", esite.Text.Trim() == "全部" ? "%%" : esite.Text.Trim()));
                     list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text));
                     list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text));
                     //if (CommonClass.IsZhanQuCompanyId(CommonClass.UserInfo.companyid))//战区查询
                     //{
                     //    list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));
                     //}
                     //else
                     //{
                         list.Add(new SqlPara("WebName", WebName.Text.Trim() == "全部" ? "%%" : WebName.Text.Trim()));
                     //}
                     SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_billErrorSign", list);
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
             else
             {
                 try
                 {
                     List<SqlPara> list = new List<SqlPara>();
                     list.Add(new SqlPara("begDate", bdate.Text.Trim()));
                     list.Add(new SqlPara("endDate", edate.Text.Trim()));
                     list.Add(new SqlPara("bsite", bsite.Text.Trim() == "全部" ? "%%" : bsite.Text.Trim()));
                     list.Add(new SqlPara("esite", esite.Text.Trim() == "全部" ? "%%" : esite.Text.Trim()));
                     list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text));
                     list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text));
                     //if (CommonClass.IsZhanQuCompanyId(CommonClass.UserInfo.companyid))//战区查询
                     //{
                     //    list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));
                     //}
                     //else
                     //{
                         list.Add(new SqlPara("WebName", WebName.Text.Trim() == "全部" ? "%%" : WebName.Text.Trim()));
                     //}

                     SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GETSIGN_ALL", list);
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
        }

        private void btnFetchCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "SignNO").ToString());

                if (CommonClass.QSP_LOCK_5(myGridView1.GetRowCellValue(rowhandle, "BillNo").ToString(), myGridView1.GetRowCellValue(rowhandle, "SignDate").ToString()))
                {
                    return;
                }

                string billNo=myGridView1.GetRowCellValue(rowhandle, "BillNo").ToString();
                string SignType = ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "SignType"));
                
                if (SignType == "送货签收")
                {
                    num = 13;
                }
                else if (SignType == "提货签收")
                {
                    num = 14;
                }
                else if (SignType == "本地中转签收" || SignType == "终端中转签收")
                {
                    num = 15;
                }
                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }

                //提前获取到轨迹信息
                List<SqlPara> lists = new List<SqlPara>();
                lists.Add(new SqlPara("DepartureBatch", null));
                lists.Add(new SqlPara("BillNO", billNo + "@"));
                lists.Add(new SqlPara("tracetype", SignType));
                lists.Add(new SqlPara("num", num));
                DataSet dss = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_TraceSyn", lists));
                                                                                       
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SignNO", id));
                list.Add(new SqlPara("SignType", ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "SignType"))));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "[USP_DELETE_BILLSIGN_NEW]", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    myGridView1.DeleteRow(rowhandle);

                    CommonSyn.DELETE_BILLSIGN_SYN(billNo, SignType);

                    //CommonSyn.SignCancelSyn(billNo);//分拨同步 zaj 2018-4-13
                    //CommonSyn.TimeCancelSyn((billNo + "@"), "", "", "USP_DELETE_BILLSIGN");//时效取消同步 LD 2018-4-27
                    //CommonSyn.TraceSyn(null, (billNo + "@"), num, SignType, 2, null,dss);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void frmSignList_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("签收录入");//xj/2019/5/28
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            bdate.EditValue = CommonClass.gbdate;
            edate.EditValue = CommonClass.gedate;
            CommonClass.SetSite(bsite, true);
            CommonClass.SetSite(esite, true);
            CommonClass.SetCause(CauseName, true);
            esite.EditValue = CommonClass.UserInfo.SiteName;
            bsite.EditValue = "全部";
            CauseName.Text = CommonClass.UserInfo.CauseName;
            AreaName.Text = CommonClass.UserInfo.AreaName;
            WebName.Text = CommonClass.UserInfo.WebName;
        }

        private void btnScanSignBill_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void btnEport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLockStyle_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void btnStyleCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void btnFilter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void CauseName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(AreaName, CauseName.Text);
            CommonClass.SetCauseWeb(WebName, CauseName.Text, AreaName.Text);
        }

        private void AreaName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(WebName, CauseName.Text, AreaName.Text);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "客户提货记录");
        }
    }
}