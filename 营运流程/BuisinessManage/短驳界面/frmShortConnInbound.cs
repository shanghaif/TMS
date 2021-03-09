using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;

namespace ZQTMS.UI
{
    public partial class frmShortConnInbound : BaseForm
    {
        DataSet ds = new DataSet();

        public frmShortConnInbound()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("�̲�����");//xj2019/5/28
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            CommonClass.GetGridViewColumns(myGridView2);
            GridOper.SetGridViewProperty(myGridView2);
            BarMagagerOper.SetBarPropertity(bar1, bar2);
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            CommonClass.SetSite(endSite, true);
            endSite.EditValue = CommonClass.UserInfo.SiteName;
            WebName.EditValue = CommonClass.UserInfo.WebName;
            getdata();
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            if (bdate.DateTime > edate.DateTime)
            {
                XtraMessageBox.Show("��ʼ���ڲ��ܴ��ڽ�������", "����ѡ�����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string esite = endSite.Text == "ȫ��" ? "%%" : endSite.Text;
            string webname = WebName.Text == "ȫ��" ? "%%" : WebName.Text;
            try
            {
                //if (CommonClass.IsZhanQuCompanyId(CommonClass.UserInfo.companyid))//ս����ѯ
                //{
                //    webname = CommonClass.UserInfo.WebName;
                //}
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SCDesSite", esite));
                list.Add(new SqlPara("SCDESWeb", webname));
                list.Add(new SqlPara("begDate", bdate.DateTime));
                list.Add(new SqlPara("endDate", edate.DateTime));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SHORTCONN_ARRIVED", list);
                myGridControl2.DataSource = SqlHelper.GetDataTable(sps);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                //if (myGridView2.RowCount < 1000) myGridView2.BestFitColumns();
            }
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btnLockStyle_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void btnStyleCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void btnFilter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void myGridView1_DoubleClick(object sender, EventArgs e)
        {
            //��ȡ������ϸ
            if (myGridView1.FocusedRowHandle < 0) return;
            frmRecShortConn frm = new frmRecShortConn();
            frm.SCId = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCId"));
            frm.U_SCBatchNo = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCBatchNo"));
            frm.U_SCDate = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCDate"));
            frm.U_SCSite = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCSite"));
            frm.U_SCWeb = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCWeb"));
            frm.U_SCCarNo = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCCarNo"));
            frm.U_SCDriver = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCDriver"));
            frm.U_SCDesSite = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCDesSite"));
            frm.U_SCDesWeb = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCDesWeb"));
            frm.U_SCDContolMan = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCDContolMan"));
            frm.systemType = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SystemType"));
            frm.shortType = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "ShortType"));
            frm.ShowDialog();
        }

        private void endSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(WebName, endSite.EditValue.ToString());
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private void getdata()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SCDesSite", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("SCDESWeb", CommonClass.UserInfo.WebName));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SHORTCONN_INROAD", list);
                myGridControl1.DataSource = SqlHelper.GetDataTable(sps);
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

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            //���ձ���
            try
            {
                if (MsgBox.ShowYesNo("ȷ�ϵ���ȷ�ϣ�") != DialogResult.Yes) return;
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                string SCBatchNo = GridOper.GetRowCellValueString(myGridView1, rowhandle, "SCBatchNo");
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SCBatchNo", SCBatchNo));
                list.Add(new SqlPara("SCAcceptMan", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("type", 1));//1��ʾ���ձ���,2��ʾȡ������
                string json = CreateJson.GetShortJson(list, 6, "USP_UPDATE_RECSHORTCONN_ZQTMS", "", SCBatchNo, CommonClass.UserInfo.companyid);
                string url = HttpHelper.urlLMSSysExecuteZQTMSCurrency;
                list.Add(new SqlPara("Json", json));
                list.Add(new SqlPara("URL", url));
                //list.Add(new SqlPara("ProcName", "USP_UPDATE_RECSHORTCONN_ZQTMS"));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_UPDATE_RECSHORTCONN_KT", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    string systemType = myGridView1.GetRowCellValue(rowhandle, "SystemType").ToString();
                    string shortType = myGridView1.GetRowCellValue(rowhandle, "ShortType").ToString();
                    MsgBox.ShowOK();
                    myGridView1.DeleteRow(rowhandle);

                    CommonSyn.TimeOtherUptSyn("", "", "", "", "", SCBatchNo, CommonClass.UserInfo.WebName, "USP_UPDATE_RECSHORTCONN_KT", "");//ͬ�������޸�ʱЧ LD 2018-4-27
                    //CommonSyn.TraceSyn(SCBatchNo, null, 2, "�̲�����", 1, "�̲�");
                    if (shortType == "1")
                    {
                        CommonSyn.ShortReplaceLms(list, 6, "USP_UPDATE_RECSHORTCONN_ZQTMS", "", SCBatchNo, "");
                    }

                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

            #region ֱ�ӽ��ճ���,����Ҫ����ϸ��
            /*
            if (ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCState")) != "δ��")
                return;
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            frmRecShortConn frm = new frmRecShortConn();
            frm.SCId = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCId"));
            frm.U_SCBatchNo = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCBatchNo"));
            frm.U_SCDate = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCDate"));
            frm.U_SCSite = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCSite"));
            frm.U_SCWeb = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCWeb"));
            frm.U_SCCarNo = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCCarNo"));
            frm.U_SCDriver = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCDriver"));
            frm.U_SCDesSite = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCDesSite"));
            frm.U_SCDesWeb = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCDesWeb"));
            frm.U_SCDContolMan = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCDContolMan"));
            frm.isMod = true;
            frm.SCId = myGridView1.GetRowCellValue(rowhandle, "SCId").ToString();
            frm.ShowDialog();
            getdata();
            */
            #endregion
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void myGridView2_DoubleClick(object sender, EventArgs e)
        {
            if (myGridView2.FocusedRowHandle < 0) return;
            frmRecShortConn ws = new frmRecShortConn();
            ws.U_SCBatchNo = myGridView2.GetRowCellValue(myGridView2.FocusedRowHandle, "SCBatchNo").ToString();
            ws.U_SCDate = myGridView2.GetRowCellValue(myGridView2.FocusedRowHandle, "SCDate").ToString();
            ws.U_SCSite = myGridView2.GetRowCellValue(myGridView2.FocusedRowHandle, "SCSite").ToString();
            ws.U_SCWeb = myGridView2.GetRowCellValue(myGridView2.FocusedRowHandle, "SCWeb").ToString();
            ws.U_SCCarNo = myGridView2.GetRowCellValue(myGridView2.FocusedRowHandle, "SCCarNo").ToString();
            ws.U_SCDriver = myGridView2.GetRowCellValue(myGridView2.FocusedRowHandle, "SCDriver").ToString();
            ws.U_SCDesSite = myGridView2.GetRowCellValue(myGridView2.FocusedRowHandle, "SCDesSite").ToString();
            ws.U_SCDesWeb = myGridView2.GetRowCellValue(myGridView2.FocusedRowHandle, "SCDesWeb").ToString();
            ws.U_SCDContolMan = myGridView2.GetRowCellValue(myGridView2.FocusedRowHandle, "SCMan").ToString();
            ws.SCId = myGridView2.GetRowCellValue(myGridView2.FocusedRowHandle, "SCId").ToString();
            ws.systemType = myGridView2.GetRowCellValue(myGridView2.FocusedRowHandle, "SystemType").ToString();
            ws.shortType = myGridView2.GetRowCellValue(myGridView2.FocusedRowHandle, "ShortType").ToString();
            ws.isArrivedInBound = true;
            ws.ShowDialog();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "�̲���;����");
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (MsgBox.ShowYesNo("ȷ��ȡ�����ձ�����") != DialogResult.Yes) return;
            //ȡ������
            try
            {
                int rowhandle = myGridView2.FocusedRowHandle;
                if (rowhandle < 0) return;
                string SCBatchNo = GridOper.GetRowCellValueString(myGridView2, rowhandle, "SCBatchNo");
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SCBatchNo", SCBatchNo));
                list.Add(new SqlPara("SCAcceptMan", ""));
                list.Add(new SqlPara("type", 2));//1��ʾ���ձ���,2��ʾȡ������
                string json = CreateJson.GetShortJson(list, 6, "USP_UPDATE_RECSHORTCONN_ZQTMS", "", SCBatchNo, CommonClass.UserInfo.companyid);
                string url = HttpHelper.urlLMSSysExecuteZQTMSCurrency;
                list.Add(new SqlPara("Json", json));
                list.Add(new SqlPara("URL", url));
                //list.Add(new SqlPara("ProcName", "USP_UPDATE_RECSHORTCONN_ZQTMS"));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_UPDATE_RECSHORTCONN_KT", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    string systemType = myGridView2.GetRowCellValue(rowhandle, "SystemType").ToString();
                    string shortType = myGridView2.GetRowCellValue(rowhandle, "ShortType").ToString();
                    myGridView2.DeleteRow(rowhandle);
                    MsgBox.ShowOK();

                    CommonSyn.TimeCancelSyn("", SCBatchNo, "", "USP_UPDATE_RECSHORTCONN_KT");//ʱЧȡ��ͬ�� LD 2018-4-27
                    if (shortType == "1")
                    {
                        CommonSyn.ShortReplaceLms(list, 6, "USP_UPDATE_RECSHORTCONN_ZQTMS", "", SCBatchNo, CommonClass.UserInfo.companyid);
                    }
                    //CommonSyn.TraceSyn(SCBatchNo, null, 2, "�̲�����", 2, "�̲�");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// ɸѡ�������˫���¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectCondition_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                (sender as ComboBoxEdit).SelectAll();
            }
            catch { }
        }

        private void barButtonItem21_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barButtonItem18_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView2);
        }

        private void barButtonItem20_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem22_ItemClick(object sender, ItemClickEventArgs e)
        {
            GetPDA();
        }

        private void GetPDA()
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;

            frmPDA ws = new frmPDA();
            ws.dtvehicleno = myGridView1.GetRowCellValue(rowhandle, "SCCarNo").ToString();
            ws.dtchauffer = myGridView1.GetRowCellValue(rowhandle, "SCDriver").ToString();
            ws.dtsenddate = Convert.ToDateTime(myGridView1.GetRowCellValue(rowhandle, "SCDate"));
            ws.dtinoneflag = myGridView1.GetRowCellValue(rowhandle, "SCBatchNo").ToString();
            ws.type = 1;
            ws.ShowDialog();
        }

        private void barButtonItem23_ItemClick(object sender, ItemClickEventArgs e)
        {
            GetPDA();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            int rowhandle = myGridView2.FocusedRowHandle;
            if (rowhandle < 0) return;

            if (MsgBox.ShowYesNo("ȷ��ж����ɣ�") != DialogResult.Yes) return;

            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_SET_SHORTCONN_DOWN_GOODS", new List<SqlPara> { new SqlPara("SCBatchNo", GridOper.GetRowCellValueString(myGridView2, rowhandle, "SCBatchNo")) })) == 0)
                return;

            MsgBox.ShowOK("ж�����");
        }


        private void barButtonItem25_ItemClick(object sender, ItemClickEventArgs e)
        {
            int row = myGridView2.FocusedRowHandle;
            if (row < 0) return;
            //{
            //    MsgBox.ShowError("��ѡ��һ����Ϣ��");
            //    return;
            //}
            string DepartureBatch = GridOper.GetRowCellValueString(myGridView2, "SCBatchNo");
            frmHandFeeAdd_KT frm = new frmHandFeeAdd_KT();
            frm.sDepartureBatch = DepartureBatch;
            frm.sFeeType = "װж��-�̲�����";
            frm.ShowDialog();
        }
    }
}