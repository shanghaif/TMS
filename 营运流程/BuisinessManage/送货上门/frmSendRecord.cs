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
    public partial class frmSendRecord : BaseForm
    {
        public frmSendRecord()
        {
            InitializeComponent();
        }
        private void getdata()
        {
            (myGridControl1.MainView as GridView).ClearColumnsFilter();
            string proc = comboBoxEdit1.SelectedIndex == 0 ? "QSP_GET_SEND_DEPARTURE_GXKT" : "QSP_GET_SEND_BILL";
            //string proc = comboBoxEdit1.SelectedIndex == 0 ? "QSP_GET_SEND_DEPARTURE_GXKT_TEST" : "QSP_GET_SEND_BILL";
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                //if (CommonClass.IsZhanQuCompanyId(CommonClass.UserInfo.companyid))//ս����ѯ
                //{
                //    proc = comboBoxEdit1.SelectedIndex == 0 ? "QSP_GET_SEND_DEPARTURE_GXKT_ZQ" : "QSP_GET_SEND_BILL_ZQ";
                //    list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));
                //}
                //else
                //{
                list.Add(new SqlPara("WebName", edwebid.Text.Trim() == "ȫ��" ? "%%" : edwebid.Text.Trim()));
                //}
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("StartSite", edbsite.Text.Trim() == "ȫ��" ? "%%" : edbsite.Text.Trim()));
                list.Add(new SqlPara("DestinationSite", edesite.Text.Trim() == "ȫ��" ? "%%" : edesite.Text.Trim()));
                list.Add(new SqlPara("WebType", comboBoxEdit2.SelectedIndex));
                list.Add(new SqlPara("CarStartState", CarStartState.Text.Trim() == "ȫ��" ? "%%" : CarStartState.Text));
                if (CommonClass.UserInfo.companyid == "309" || CommonClass.UserInfo.companyid == "490")  //zb20190617
                {
                    list.Add(new SqlPara("CauseType", comboBoxEdit3.Text.Trim() == "ȫ��" ? "%%" : comboBoxEdit3.Text));
                }
                else
                {
                    list.Add(new SqlPara("CauseType", ""));
                }
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, proc, list);
                myGridControl1.DataSource = SqlHelper.GetDataTable(sps);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

            PrePrintItems();
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            myGridControl1.MainView = comboBoxEdit1.SelectedIndex == 0 ? myGridView2 : myGridView1; //0 ����  1 ��Ʊ
            toolStripMenuItem1.Visible = comboBoxEdit1.SelectedIndex == 1;
            getdata();
        }

        private void PrePrintItems()
        {
            //toolStripButton1
            int have = 0;
            string sendinoneflag = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                sendinoneflag = ConvertType.ToString(myGridView1.GetRowCellValue(i, "SendBatch"));

                if (barSubItem3.ItemLinks.Count > 0)
                {
                    have = 0;
                    for (int j = 0; j < barSubItem3.ItemLinks.Count; j++)
                        if (barSubItem3.ItemLinks[j].Caption == sendinoneflag)
                        { have = 1; }
                }

                if (have == 0)
                {
                    DevExpress.XtraBars.BarButtonItem item = new DevExpress.XtraBars.BarButtonItem();
                    item.Caption = sendinoneflag;
                    item.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(item_ItemClick);
                    barSubItem3.ItemLinks.Add(item);
                }
            }
        }

        private void item_ItemClick(object sender, ItemClickEventArgs e)
        {
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
            frmSendLoad_GX wsl = frmSendLoad_GX.Get_frmSendLoad;
            wsl.MdiParent = this.MdiParent;
            wsl.Show();
            wsl.Focus();
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

            frmBillSearchControl.ShowBillSearch(GridOper.GetRowCellValueString(gv, "BillNo"));
        }

        private void �����ͻ�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            barButtonItem3.PerformClick();
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            //�ͻ�����
            MyGridView gv = (MyGridView)myGridControl1.MainView;
            if (gv.FocusedRowHandle < 0) return;
            frmSendDetail ws = new frmSendDetail();
            ws.gv = gv;
            ws.ShowDialog();
            getdata();   //plh
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            //DevExpress.XtraGrid.Views.Grid.GridView gv = (DevExpress.XtraGrid.Views.Grid.GridView)gridControl1.MainView;
            //if (gv.FocusedRowHandle < 0) return;
            //string sendvehicleno = gv.GetRowCellValue(gv.FocusedRowHandle, "sendvehicleno") == DBNull.Value ? "" : gv.GetRowCellValue(gv.FocusedRowHandle, "sendvehicleno").ToString();
            //if (sendvehicleno == "")
            //{
            //    commonclass.MsgBox.ShowOK("û��ȡ�ó���,�޷���λ!");
            //    return;
            //}
            //WebServiceHelper.GetPos(sendvehicleno);
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            //foreach (Form form in this.MdiParent.MdiChildren)
            //{
            //    if (form.GetType() == typeof(w_md_accept_record_cancel))
            //    {
            //        form.Focus();
            //        return;
            //    }
            //}

            //w_md_accept_record_cancel fm = new w_md_accept_record_cancel();
            //fm.MdiParent = this.MdiParent;
            //fm.Dock = DockStyle.Fill;
            //fm.Show();
        }

        private void frmSendRecord_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("�ͻ�����");//xj2019/5/28
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, false, myGridView2);
            GridOper.SetGridViewProperty(myGridView1, myGridView2);
            BarMagagerOper.SetBarPropertity(bar3); //����о���Ĺ���������������ʵ��

            CommonClass.SetSite(edbsite, true);
            CommonClass.SetSite(edesite, true);
            CommonClass.SetWeb(edwebid, CommonClass.UserInfo.SiteName);

            GridOper.RestoreGridLayout(myGridView1, myGridView2);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            FixColumn fix2 = new FixColumn(myGridView2, barSubItem4);

            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            edesite.Text = CommonClass.UserInfo.SiteName;
            edwebid.Text = CommonClass.UserInfo.WebName;
            CommonClass.GetServerDate();

            if (CommonClass.UserInfo.companyid != "309" && CommonClass.UserInfo.companyid != "490")
            {
                barButtonItem15.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                barButtonItem16.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            //zb20190612
            else
            {
                label8.Visible = true;
                comboBoxEdit3.Visible = true;
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("CauseType", comboBoxEdit3.SelectedIndex));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_BASWEB_ByCompanyID", list);
                DataSet ds = SqlHelper.GetDataSet(spe);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    edwebid.Properties.Items.Add(ds.Tables[0].Rows[i]["WebName"].ToString());
                }
                GridOper.CreateStyleFormatCondition(myGridView2, "SendAduitState", DevExpress.XtraGrid.FormatConditionEnum.Equal, "�����", Color.Red);
            }

        }

        private void edbsite_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(edwebid, edbsite.Text.Trim());
        }

        private void myGridControl1_DoubleClick(object sender, EventArgs e)
        {
            if (comboBoxEdit1.Text == "����")
                barButtonItem8.PerformClick();
        }

        private void myGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;
            frmBillSearchControl.ShowBillSearch(myGridView1, "BillNo");
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                //if (comboBoxEdit1.SelectedIndex == 1)
                //{
                //    MsgBox.ShowOK("�ͻ���ʽΪ���������Ĳ��ܵ������,������ѡ��");
                //    return;
                //}    

                int rowhandle = myGridView2.FocusedRowHandle;   //zhengjiafeng20181113
                if (rowhandle < 0) return;
                if (ConvertType.ToString(myGridView2.GetRowCellValue(rowhandle, "Carstate")) != "δ����")
                {
                    MsgBox.ShowOK("�������Ѿ������������");
                    return;
                }

                string batchNo = GridOper.GetRowCellValueString(myGridView2, myGridView2.FocusedRowHandle, "SendBatch");
                if (batchNo.Trim() == "")
                {
                    MsgBox.ShowOK("�ͻ���ʽΪ���������Ĳ��ܵ������,������ѡ��");
                    return;
                }
                if (myGridView2.FocusedRowHandle < 0)
                {
                    MsgBox.ShowOK("��ѡ��һ��������Ϣ");
                    return;
                }
                string sendToSite = GridOper.GetRowCellValueString(myGridView2, myGridView2.FocusedRowHandle, "SendToSite");
                string sendToWeb = GridOper.GetRowCellValueString(myGridView2, myGridView2.FocusedRowHandle, "SendToWeb");
                if (sendToSite.Trim() == "")
                {
                    MsgBox.ShowOK("ֻ��ת���������β��ܵ��������������ѡ��");
                    return;
                }
                if (batchNo.Substring(0, 2) == "KS")
                {
                    MsgBox.ShowOK("���ͻ�������ZQTMS�ģ�������������");
                    return;
                }
                frmSendToSiteVehicleStart frm = new frmSendToSiteVehicleStart();
                frm.batchNo = batchNo;
                frm.sendToSite = sendToSite;
                frm.sendToWeb = sendToWeb;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                int row = myGridView2.FocusedRowHandle;
                if (row < 0)
                {
                    MsgBox.ShowError("��ѡ��һ����Ϣ��");
                    return;
                }
                if (MsgBox.ShowYesNo("ȷ��ȡ��������") != DialogResult.Yes) return;

                string DepartureBatch = GridOper.GetRowCellValueString(myGridView2, "SendBatch");
                if (DepartureBatch.Substring(0, 2) == "KS")
                {
                    MsgBox.ShowError("������ΪZQTMS�������Σ�����ȡ����");
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("batchNo", DepartureBatch));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_DELETE_SendBILLVEHICLESTAR", list)) == 0) return;
                MsgBox.ShowOK();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            int row = myGridView2.FocusedRowHandle;
            if (row < 0) return;
            //{
            //    MsgBox.ShowError("��ѡ��һ����Ϣ��");
            //    return;
            //}
            string sendToSite = GridOper.GetRowCellValueString(myGridView2, myGridView2.FocusedRowHandle, "SendToSite");
            if (sendToSite.Trim() == "")
            {
                MsgBox.ShowOK("ֻ��ת���������β���¼������װж�ѣ�������ѡ��");
                return;
            }
            string DepartureBatch = GridOper.GetRowCellValueString(myGridView2, "SendBatch");
            frmHandFeeAdd_KT frm = new frmHandFeeAdd_KT();
            frm.sDepartureBatch = DepartureBatch;
            frm.sFeeType = "װж��-ת����";
            frm.ShowDialog();
        }
        //���
        private void barButtonItem15_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView2.PostEditor();
            try
            {
                string AduitType = "�ͻ������";
                string BatchNos = "";
                if (MsgBox.ShowYesNo("�Ƿ�ȷ����ˣ�") != DialogResult.Yes) return;
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    if (Convert.ToInt32(myGridView2.GetRowCellValue(i, "isChecked")) > 0)
                    {
                        string aduitstate = myGridView2.GetRowCellValue(i, "SendAduitState").ToString();
                        if (aduitstate == "�����")
                        {
                            MsgBox.ShowError("�����ͻ������������ͻ�����ˣ������ظ���ˣ�");
                            return;
                        }
                        BatchNos += myGridView2.GetRowCellValue(i, "SendBatch") + "@";
                    }
                }
                if (string.IsNullOrEmpty(BatchNos))
                {
                    MsgBox.ShowError("δ�ҵ���Ҫ��˵����ݣ�");
                    return;
                }

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("WebName", edwebid.Text.Trim() == "ȫ��" ? "%%" : edwebid.Text.Trim()));
                list.Add(new SqlPara("AduitType", AduitType));
                list.Add(new SqlPara("BatchNos", BatchNos));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "USP_UPDATE_SENDADUITSTATE_ByBatchNOs", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    MsgBox.ShowOK();

                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            getdata();
        }
        //�����
        private void barButtonItem16_ItemClick(object sender, ItemClickEventArgs e)
        {
            int row = myGridView2.FocusedRowHandle;
            string AduitType = "�ͻ��ѷ����";
            if (row < 0)
            {
                MsgBox.ShowError("��ѡ��һ����Ϣ��");
                return;
            }
            if (MsgBox.ShowYesNo("�Ƿ�ȷ������ˣ�") != DialogResult.Yes) return;
            string SendBatch = GridOper.GetRowCellValueString(myGridView2, myGridView2.FocusedRowHandle, "SendBatch");
            string SendAduitState = GridOper.GetRowCellValueString(myGridView2, "SendAduitState");
            if (SendAduitState == "δ���")
            {
                MsgBox.ShowError("�����λ�δ����ˣ�");
                return;
            }


            try
            {
                #region �жϷ��������Ƿ�������˵��˵�
                List<SqlPara> list1 = new List<SqlPara>();
                list1.Add(new SqlPara("SendBatch", SendBatch));
                SqlParasEntity spe1 = new SqlParasEntity(OperType.Query, "QSP_GET_BILLSENDGOODS_BY_SendBatch", list1);
                DataSet ds = SqlHelper.GetDataSet(spe1);
                if (ds != null || ds.Tables.Count > 0 || ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (ds.Tables[0].Rows[i]["SendAduitState"].ToString() == "1")
                        {
                            MsgBox.ShowOK("�ͻ����Ѻ������ɷ���ˣ������޸����ȷ�������");
                            return;
                        }
                    }

                }
                #endregion


                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("WebName", edwebid.Text.Trim() == "ȫ��" ? "%%" : edwebid.Text.Trim()));
                list.Add(new SqlPara("AduitType", AduitType));
                list.Add(new SqlPara("SendBatch", SendBatch));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "USP_UPDATE_SENDADUITSTATE", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    MsgBox.ShowOK();
                }

            }

            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            getdata();
        }

        private void comboBoxEdit3_SelectedValueChanged(object sender, EventArgs e)
        {
            edwebid.Properties.Items.Clear();
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("CauseType",comboBoxEdit3.SelectedIndex));
            SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_BASWEB_ByCompanyID", list);
            DataSet ds = SqlHelper.GetDataSet(spe);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                edwebid.Properties.Items.Add(ds.Tables[0].Rows[i]["WebName"].ToString());
            }
            if (comboBoxEdit3.SelectedIndex == 0)
            {
                edwebid.Properties.Items.Add("ȫ��");
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            MsgBox.ShowOK("����ë���� ����Ʊ=�����ͻ���+������ת��-ʵ���ͻ��ѣ�������(���ת�͵�����Ϊ�գ���Ϊ((�����ͻ���+������ת��+�ն˽����)*ʵ�ͼ���/����)-ʵ���ͻ��ѣ�����Ϊ0)��"
               + "\n" + "̯���ͻ��ѣ�����Ʊ=ʵ���ͻ��ѣ�������̯���ͻ��� ��");
        }
    }
}