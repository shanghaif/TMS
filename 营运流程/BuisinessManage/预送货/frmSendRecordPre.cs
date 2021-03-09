using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using System.Threading;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using ZQTMS.Lib;
using DevExpress.XtraGrid.Views.Grid;

namespace ZQTMS.UI
{
    public partial class frmSendRecordPre : BaseForm
    {
        public frmSendRecordPre()
        {
            InitializeComponent();
        }
        private void getdata()
        {
            string proc = comboBoxEdit1.SelectedIndex == 0 ? "QSP_GET_SEND_DEPARTUREPRE" : "QSP_GET_SEND_BILLPRE";
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("StartSite", edbsite.Text.Trim() == "ȫ��" ? "%%" : edbsite.Text.Trim()));
                list.Add(new SqlPara("DestinationSite", edesite.Text.Trim() == "ȫ��" ? "%%" : edesite.Text.Trim()));
                list.Add(new SqlPara("WebName", edwebid.Text.Trim() == "ȫ��" ? "%%" : edwebid.Text.Trim()));
                list.Add(new SqlPara("WebType", comboBoxEdit2.SelectedIndex));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, proc, list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            myGridControl1.MainView = comboBoxEdit1.SelectedIndex == 0 ? myGridView2 : myGridView1; //0 ����  1 ��Ʊ
            toolStripMenuItem1.Visible = comboBoxEdit1.SelectedIndex == 1;
            getdata();
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView2);
        }

        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString(), myGridView2.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1, myGridView2);
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (CommonClass.UserInfo.UserDB == UserDB.ZQTMS20160713)
            {
                frmSendLoadPre wsl = frmSendLoadPre.Get_frmSendLoadPre;
                wsl.MdiParent = this.MdiParent;
                wsl.Show();
                wsl.Focus();
            }
            else if (CommonClass.UserInfo.UserDB == UserDB.ZQTMS3PL)
            {
                frmSendLoadPre3PL w3pl = new frmSendLoadPre3PL();
                w3pl.MdiParent = this.MdiParent;
                w3pl.Show();
                w3pl.Focus();
            }
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridControl1.MainView as MyGridView);
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MyGridView gv = myGridControl1.MainView as MyGridView;
            if (gv == null || gv.FocusedRowHandle < 0) return;

            frmBillSearch.ShowBillSearch(GridOper.GetRowCellValueString(gv, "BillNo"));
        }

        private void �����ͻ�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            barButtonItem3.PerformClick();
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            MyGridView gv = (MyGridView)myGridControl1.MainView;
            if (gv.FocusedRowHandle < 0) return;
            //�ͻ�����
            if (CommonClass.UserInfo.UserDB == UserDB.ZQTMS20160713)
            {
                frmSendDetailPre ws = new frmSendDetailPre();
                ws.gv = gv;
                ws.ShowDialog();
            }
            else if (CommonClass.UserInfo.UserDB == UserDB.ZQTMS3PL)
            {
                frmSendDetailPre3PL w3pl = new frmSendDetailPre3PL();
                w3pl.gv = gv;
                w3pl.ShowDialog();
            }
        }

        private void frmSendRecordPre_Load(object sender, EventArgs e)
        {
            if (CommonClass.UserInfo.UserDB == UserDB.ZQTMS20160713) barButtonItem10.Visibility = BarItemVisibility.Always;

            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, false, myGridView2);
            GridOper.SetGridViewProperty(myGridView1, myGridView2);
            BarMagagerOper.SetBarPropertity(bar3); //����о���Ĺ���������������ʵ��

            CommonClass.SetSite(edbsite, true);
            CommonClass.SetSite(edesite, true);

            GridOper.RestoreGridLayout(myGridView1, myGridView2);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            FixColumn fix2 = new FixColumn(myGridView2, barSubItem4);

            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            edesite.Text = CommonClass.UserInfo.SiteName;
            edwebid.Text = CommonClass.UserInfo.WebName;
            CommonClass.GetServerDate();
        }

        private void myGridControl1_DoubleClick(object sender, EventArgs e)
        {
            barButtonItem8.PerformClick();
        }

        private void edesite_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(edwebid, edesite.Text.Trim());
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridView gv = myGridControl1.MainView as GridView;
            string SendBatch = GridOper.GetRowCellValueString(gv, "SendBatch");
            if (SendBatch == "") return;

            if (GridOper.GetRowCellValueString(gv, "NewSendBatch") != "")
            {
                MsgBox.ShowOK("������תʵ�ͻ��������ظ�ת!");
                return;
            }

            if (MsgBox.ShowYesNo("ȷ����Ԥ�ͻ�����:" + SendBatch + "  תΪʵ�ͻ���") != DialogResult.Yes) return;

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
            list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));
            //ȡ�����п��
            DataSet dsSendLoad = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_SEND_LOAD", list));
            if (dsSendLoad == null || dsSendLoad.Tables.Count == 0 || dsSendLoad.Tables[0].Rows.Count == 0)
            {
                MsgBox.ShowOK("û��ȡ����棬�����Ի��Ժ�����!");
                return;
            }

            //ȡ����ǰ���εĻ�
            DataSet dsSendRecord = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_SENDPRE_RECORD_BY_BATCH", new List<SqlPara> { new SqlPara("SendBatch", SendBatch) }));
            if (dsSendRecord == null || dsSendRecord.Tables.Count == 0 || dsSendRecord.Tables[0].Rows.Count == 0)
            {
                MsgBox.ShowOK("û��ȡ��Ҫתʵ�͵�Ԥ�ͻ���¼,�����Ի��Ժ����ԣ����Ѿ�תʵ�ͻ�!");
                return;
            }

            string BillNo = "", BillNoStr = "", SendPCSStr = "", AccSendStr = "", IdStr = "", NewSendBatch = "", SendToProvinceStr = "", SendToCityStr = "", SendToAreaStr = "", SendToStreet = "";
            //Ԥ�ͻ����ļ�����ʵ���ͻ����
            int PreSendPCS = 0, RemainSendPCS = 0;
            for (int i = 0; i < dsSendRecord.Tables[0].Rows.Count; i++)
            {
                BillNo = ConvertType.ToString(dsSendRecord.Tables[0].Rows[i]["BillNo"]);
                for (int j = 0; j < dsSendLoad.Tables[0].Rows.Count; j++)
                {
                    if (BillNo == ConvertType.ToString(dsSendLoad.Tables[0].Rows[j]["BillNo"]))
                    {
                        PreSendPCS = ConvertType.ToInt32(dsSendRecord.Tables[0].Rows[i]["SendPCS"]);//Ԥ�ͻ��ķ�������
                        RemainSendPCS = ConvertType.ToInt32(dsSendLoad.Tables[0].Rows[j]["sendqty"]);//�ͻ�������

                        if (RemainSendPCS > 0)//ʣ���������0�ſ���תʵ��
                        {
                            BillNoStr += BillNo + ",";
                            //�ͻ�������С��Ԥ�ͻ��ļ���ʱ���ÿ�����
                            SendPCSStr += (PreSendPCS > RemainSendPCS ? RemainSendPCS : PreSendPCS) + ",";
                            AccSendStr += ConvertType.ToDecimal(dsSendRecord.Tables[0].Rows[i]["AccSend"]) + ",";
                            IdStr += ConvertType.ToString(dsSendRecord.Tables[0].Rows[i]["Id"]) + ",";

                            SendToProvinceStr += ConvertType.ToString(dsSendRecord.Tables[0].Rows[i]["SendToProvince"]) + ",";
                            SendToCityStr += ConvertType.ToString(dsSendRecord.Tables[0].Rows[i]["SendToCity"]) + ",";
                            SendToAreaStr += ConvertType.ToString(dsSendRecord.Tables[0].Rows[i]["SendToArea"]) + ",";
                            SendToStreet += ConvertType.ToString(dsSendRecord.Tables[0].Rows[i]["SendToStreet"]) + ",";
                        }
                        dsSendLoad.Tables[0].Rows.RemoveAt(j);//ɾ�������,����ѭ������
                        break;
                    }
                }
            }

            if (BillNoStr == "") return;
            NewSendBatch = GetMaxInOneVehicleFlag();//��ȡ�ͻ�����
            if (SendBatch == "")
            {
                MsgBox.ShowOK("��ȡ�ͻ�����ʧ��,�����Ի��Ժ�����!");
                return;
            }

            string SendToSite = ConvertType.ToString(dsSendRecord.Tables[0].Rows[0]["SendToSite"]);
            string SendToWeb = ConvertType.ToString(dsSendRecord.Tables[0].Rows[0]["SendToWeb"]);

            string proc = SendToSite == "" ? "USP_ADD_SEND_2" : "USP_ADD_SEND_TOSITE";

            list.Clear();
            list.Add(new SqlPara("SendCarNo", ConvertType.ToString(dsSendRecord.Tables[0].Rows[0]["SendCarNO"])));
            list.Add(new SqlPara("SendBatch", NewSendBatch));
            list.Add(new SqlPara("SendDate", CommonClass.gcdate));
            list.Add(new SqlPara("SendOperator", CommonClass.UserInfo.UserName));
            list.Add(new SqlPara("SendDriver", dsSendRecord.Tables[0].Rows[0]["SendDriver"]));
            list.Add(new SqlPara("SendDesc", dsSendRecord.Tables[0].Rows[0]["SendDesc"]));
            list.Add(new SqlPara("SendDriverPhone", dsSendRecord.Tables[0].Rows[0]["SendDriverPhone"]));
            list.Add(new SqlPara("SendWeb", CommonClass.UserInfo.WebName));
            list.Add(new SqlPara("SendSite", CommonClass.UserInfo.SiteName));
            list.Add(new SqlPara("billnos", BillNoStr));
            list.Add(new SqlPara("AccSends", AccSendStr));
            list.Add(new SqlPara("SendPCSs", SendPCSStr));
            list.Add(new SqlPara("IdStr", IdStr));
            list.Add(new SqlPara("OldSendBatch", SendBatch));//Ԥ�ͻ������� 

            if (SendToSite != "")
            {
                list.Add(new SqlPara("SendToSite", SendToSite));
                list.Add(new SqlPara("SendToWeb", SendToWeb));
            }
            else
            {
                list.Add(new SqlPara("SendToProvinceStr", SendToProvinceStr));
                list.Add(new SqlPara("SendToCityStr", SendToCityStr));
                list.Add(new SqlPara("SendToAreaStr", SendToAreaStr));
                list.Add(new SqlPara("SendToStreetStr", SendToStreet));
            }

            try
            {
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, proc, list)) == 0) return;
                gv.SetRowCellValue(gv.FocusedRowHandle, "SendBatch", SendBatch);
                MsgBox.ShowOK("תʵ�ͻ��ɹ�,���ͻ�����Ϊ:" + SendBatch);
                CommonSyn.TraceSyn(null, BillNoStr.Replace(",", "@"), SendToSite == "" ?12:11 , SendToSite == "" ? "�ͻ�����" : "ת�Ͷ���", 1, null, null);
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private string GetMaxInOneVehicleFlag()
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("bcode", CommonClass.UserInfo.LoginSiteCode));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SENDFLAG", list);
            DataSet ds = SqlHelper.GetDataSet(sps);
            if (ds == null || ds.Tables.Count == 0) return "";

            return ConvertType.ToString(ds.Tables[0].Rows[0][0]);
        }
    }
}