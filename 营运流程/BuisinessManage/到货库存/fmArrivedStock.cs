using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraBars;
using DevExpress.XtraGrid;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using System.IO;

namespace ZQTMS.UI
{
    public partial class fmArrivedStock : BaseForm
    {
        public fmArrivedStock()
        {
            InitializeComponent();
        }

        private void fmArrivedStock_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("�������");//xj2019/5/28
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, myGridView2, myGridView3);
            GridOper.SetGridViewProperty(myGridView1, myGridView2, myGridView3);
            GridOper.RestoreGridLayout(myGridView1, myGridView2, myGridView3);
            BarMagagerOper.SetBarPropertity(bar1, bar3, bar4);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            FixColumn fix2 = new FixColumn(myGridView2, barSubItem4);
            FixColumn fix3 = new FixColumn(myGridView3, barSubItem6);

            // �������
            CommonClass.SetSite(Bsite1, true);
            CommonClass.SetSite(Esite1, true);
            CommonClass.SetCause(Cause1, true);
            Cause1.Text = comboBoxEdit1.Text = CommonClass.UserInfo.CauseName;
            Area1.Text = comboBoxEdit2.Text = CommonClass.UserInfo.AreaName;
            WebName1.Text = web2.Text = CommonClass.UserInfo.WebName;
            //Bsite1.Text = bsite2.Text = "ȫ��";
            //Esite1.Text = esite2.Text = CommonClass.UserInfo.SiteName;
            Bsite1.Text = esite2.Text = CommonClass.UserInfo.SiteName;
            Esite1.Text = bsite2.Text = "ȫ��";
            dateEdit3.DateTime = CommonClass.gbdate;
            dateEdit4.DateTime = CommonClass.gedate;

            // �������
            CommonClass.SetSite(bsite, true);
            CommonClass.SetSite(esite, true);
            CommonClass.SetCause(Cause, true);
            Cause.Text = comboBoxEdit1.Text = CommonClass.UserInfo.CauseName;
            Area.Text = comboBoxEdit2.Text = CommonClass.UserInfo.AreaName;
            web.Text = web2.Text = CommonClass.UserInfo.WebName;
            bsite.Text = bsite2.Text = "ȫ��";
            esite.Text = esite2.Text = CommonClass.UserInfo.SiteName;
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
           
            // ������¼
            CommonClass.SetSite(bsite2, true);
            CommonClass.SetSite(esite2, true);
            CommonClass.SetCause(comboBoxEdit1, true);
            dateEdit2.DateTime = CommonClass.gbdate;
            dateEdit1.DateTime = CommonClass.gedate;

            GridOper.CreateStyleFormatCondition(myGridView1, "remaindays", FormatConditionEnum.GreaterOrEqual, 3, Color.FromArgb(255, 255, 128));
            GridOper.CreateStyleFormatCondition(myGridView3, "LckDay", FormatConditionEnum.Equal, 0, Color.FromArgb(255, 255, 255));//��ɫ�̶�--��ɫ
            GridOper.CreateStyleFormatCondition(myGridView3, "LckDay", FormatConditionEnum.Equal, 1, Color.FromArgb(0, 255, 0));//��ɫ�̶�--��ɫ
            GridOper.CreateStyleFormatCondition(myGridView3, "LckDay", FormatConditionEnum.Equal, 2, Color.FromArgb(255, 255, 0));//��ɫ�̶�--��ɫ
            GridOper.CreateStyleFormatCondition(myGridView3, "LckDay", FormatConditionEnum.GreaterOrEqual, 3, Color.FromArgb(255, 0, 0));//��ɫ�̶�--��ɫ
        }

        /// <summary>
        /// ������������ɫ
        /// </summary>
        private void SetGridColor()
        {
            myGridView3.FormatConditions.Clear();
            GridOper.CreateStyleFormatCondition(myGridView3, "LckDay", DevExpress.XtraGrid.FormatConditionEnum.GreaterOrEqual,
                ConvertType.ToInt32(API.ReadINI("Color", "iWarningOne", "0", frmRuiLangService.configFileName)), Color.FromArgb(
                ConvertType.ToInt32(ConvertType.ToInt32(API.ReadINI("Color", "iColorOneR", "255", frmRuiLangService.configFileName))),
                ConvertType.ToInt32(API.ReadINI("Color", "iColorOneG", "255", frmRuiLangService.configFileName)),
                ConvertType.ToInt32(API.ReadINI("Color", "iColorOneB", "255", frmRuiLangService.configFileName))));

            GridOper.CreateStyleFormatCondition(myGridView3, "LckDay", DevExpress.XtraGrid.FormatConditionEnum.GreaterOrEqual,
                ConvertType.ToInt32(API.ReadINI("Color", "iWarningTwo", "0", frmRuiLangService.configFileName)), Color.FromArgb(
                ConvertType.ToInt32(ConvertType.ToInt32(API.ReadINI("Color", "iColorTwoR", "255", frmRuiLangService.configFileName))),
                ConvertType.ToInt32(API.ReadINI("Color", "iColorTwoG", "255", frmRuiLangService.configFileName)),
                ConvertType.ToInt32(API.ReadINI("Color", "iColorTwoB", "255", frmRuiLangService.configFileName))));

            GridOper.CreateStyleFormatCondition(myGridView3, "LckDay", DevExpress.XtraGrid.FormatConditionEnum.GreaterOrEqual,
                ConvertType.ToInt32(API.ReadINI("Color", "iWarningThree", "0", frmRuiLangService.configFileName)), Color.FromArgb(
                ConvertType.ToInt32(ConvertType.ToInt32(API.ReadINI("Color", "iColorThreeR", "255", frmRuiLangService.configFileName))),
                ConvertType.ToInt32(API.ReadINI("Color", "iColorThreeG", "255", frmRuiLangService.configFileName)),
                ConvertType.ToInt32(API.ReadINI("Color", "iColorThreeB", "255", frmRuiLangService.configFileName))));
        }

        private void Cause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(Area, Cause.Text);
            CommonClass.SetCauseWeb(web, Cause.Text, Area.Text);
        }

        private void Area_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(web, Cause.Text, Area.Text);
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            string StartSite = "", DestinationSite = "", WebName = "", CauseName = "", AreaName = "";
            StartSite = bsite.Text.Trim() == "ȫ��" ? "%%" : bsite.Text.Trim();
            DestinationSite = esite.Text.Trim() == "ȫ��" ? "%%" : esite.Text.Trim();
            WebName = web.Text.Trim() == "ȫ��" ? "%%" : web.Text.Trim();
            CauseName = Cause.Text == "ȫ��" ? "%%" : Cause.Text;
            AreaName = Area.Text == "ȫ��" ? "%%" : Area.Text;

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("StartSite", StartSite));
                list.Add(new SqlPara("DestinationSite", DestinationSite));
                list.Add(new SqlPara("WebName", WebName));
                list.Add(new SqlPara("CauseName", CauseName));
                list.Add(new SqlPara("AreaName", AreaName));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ARRIVEDSTOCK", list);
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

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(comboBoxEdit2, comboBoxEdit1.Text);
            CommonClass.SetCauseWeb(web2, comboBoxEdit1.Text, comboBoxEdit2.Text);
        }

        private void comboBoxEdit2_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(web2, comboBoxEdit1.Text, comboBoxEdit2.Text);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", dateEdit2.DateTime));
                list.Add(new SqlPara("t2", dateEdit1.DateTime));
                list.Add(new SqlPara("StartSite", bsite2.Text.Trim() == "ȫ��" ? "%%" : bsite2.Text.Trim()));
                list.Add(new SqlPara("DestinationSite", esite2.Text.Trim() == "ȫ��" ? "%%" : esite2.Text.Trim()));
                list.Add(new SqlPara("WebName", web2.Text.Trim() == "ȫ��" ? "%%" : web2.Text.Trim()));
                list.Add(new SqlPara("CauseName", comboBoxEdit1.Text.Trim() == "ȫ��" ? "%%" : comboBoxEdit1.Text.Trim()));
                list.Add(new SqlPara("AreaName", comboBoxEdit2.Text.Trim() == "ȫ��" ? "%%" : comboBoxEdit2.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ARRIVEDSTOCK_RECORD", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl2.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                if (myGridView2.RowCount < 1000) myGridView2.BestFitColumns();
            }
        }

        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "�������");
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barCheckItem3_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView2);
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView2);
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView2.Guid.ToString());
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView2, "������¼");
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

        private void barCheckItem4_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView1.PostEditor();
            string BillNoStr = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) > 0)
                    BillNoStr += GridOper.GetRowCellValueString(myGridView1, i, "BillNo") + "@";
            }
           
            if (CommonClass.UserInfo.companyid == "486")
            {
                //int rowhandle = myGridView1.FocusedRowHandle;
                //if (rowhandle < 0) return;
                //string BillNo = myGridView1.GetRowCellValue(rowhandle, "BillNo").ToString();
                string name = "";
                if (CommonClass.UserInfo.BookNote == "")
                {


                    name = CommonClass.UserInfo.IsAutoBill == false ? "���˵�" : "���˵�(��ӡ����)";
                }
                else
                {

                    if (CommonClass.UserInfo.companyid == "486" && CommonClass.UserInfo.WebName == "���������Ŀ��")
                    {
                        name = "���������Ŀ�����˵�";
                    }
                    else if (CommonClass.UserInfo.companyid == "486" && CommonClass.UserInfo.WebName == "�����人������Ӫҵ��")
                    {
                        name = "��������ܲ����Ͳ�_ǩ�յ�";
                    }
                    else if (CommonClass.UserInfo.companyid == "486" && CommonClass.UserInfo.WebName == "�人����Ӫҵ��")
                    {
                        name = "�人����Ӫҵ�����˵�";
                    }
                    else
                    {
                        name = CommonClass.UserInfo.BookNote;
                    }

                  


                }

                frmRuiLangService.Print(name, SqlHelper.GetDataTable(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTBIll_TX_1", new List<SqlPara> { new SqlPara("BillNoStr", BillNoStr) })), "");

            }
            else
            {

                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILLINFO_PRINT_ARRIVED_QSD", new List<SqlPara> { new SqlPara("BillNoStr", BillNoStr) }));
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    MsgBox.ShowOK("û���ҵ�ѡ�е��˵���Ϣ,��ӡʧ��,(����������˵��Ƿ��ѱ�ɾ��)!");
                    return;
                }
                //jl20181127
                if (CommonClass.UserInfo.WebName == "�Ϻ����ֲ�����"
                    || CommonClass.UserInfo.WebName == "�Ϻ����ֲ�����1"
                    || CommonClass.UserInfo.WebName == "���ݲ�����"
                    || CommonClass.UserInfo.WebName == "���ݲ�����1"
                    || CommonClass.UserInfo.WebName == "���������ֲ�����"
                    || CommonClass.UserInfo.WebName == "���������ֲ�����1"
                    || CommonClass.UserInfo.WebName == "����������"
                    || CommonClass.UserInfo.WebName == "����������1"
                    || CommonClass.UserInfo.WebName == "���϶����ֲ�����"
                    || CommonClass.UserInfo.WebName == "���϶����ֲ�����1"
                    || CommonClass.UserInfo.WebName == "���������ֲ�����"
                    || CommonClass.UserInfo.WebName == "���������ֲ�����1"
                    || CommonClass.UserInfo.WebName == "�人�����ֲ�����"
                    || CommonClass.UserInfo.WebName == "�人�����ֲ�����1"
                    || CommonClass.UserInfo.WebName == "���ݲ�����"
                    || CommonClass.UserInfo.WebName == "���ݲ�����1"
                    || CommonClass.UserInfo.WebName == "��ݸ��ƺ�ֲ�����"
                    || CommonClass.UserInfo.WebName == "��ݸ��ƺ�ֲ�����1"
                    || CommonClass.UserInfo.WebName == "�ൺ�����ֲ�����"
                    || CommonClass.UserInfo.WebName == "�ൺ�����ֲ�����1")
                {
                    frmRuiLangService.Print("�������ƺ", ds.Tables[0], CommonClass.UserInfo.gsqc);
                }
                else
                {
                    frmRuiLangService.Print("�����", ds.Tables[0], CommonClass.UserInfo.gsqc);
                }

            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            int s = simpleButton3.Text.Trim().Equals("ȫ ѡ") ? 1 : 0;
            simpleButton3.Text = simpleButton3.Text == "ȫ ѡ" ? "ȫ��ѡ" : "ȫ ѡ";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                myGridView1.SetRowCellValue(i, "ischecked", s);
            }
        }

        private void barCheckItem5_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView3);
        }

        private void Cause1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(Area1, Cause1.Text);
            CommonClass.SetCauseWeb(WebName1, Cause1.Text, Area1.Text);
        }

        private void Area1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(WebName1, Cause1.Text, Area1.Text);
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", dateEdit3.DateTime));
                list.Add(new SqlPara("t2", dateEdit4.DateTime));
                list.Add(new SqlPara("CauseName", Cause1.Text.Trim() == "ȫ��" ? "%%" : Cause1.Text.Trim()));
                list.Add(new SqlPara("AreaName", Area1.Text.Trim() == "ȫ��" ? "%%" : Area1.Text.Trim()));

                list.Add(new SqlPara("StartSite", Bsite1.Text.Trim() == "ȫ��" ? "%%" : Bsite1.Text.Trim()));
                list.Add(new SqlPara("TransferSite", Esite1.Text.Trim() == "ȫ��" ? "%%" : Esite1.Text.Trim()));
                list.Add(new SqlPara("BegWeb", WebName1.Text.Trim() == "ȫ��" ? "%%" : WebName1.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_SendInventory", list);
                myGridControl3.DataSource = SqlHelper.GetDataTable(sps);
                //maoihui20180103����DLL
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

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView3, myGridView3.Guid.ToString());
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView3, myGridView3.Guid.ToString());
        }

        private void barCheckItem6_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView3);
        }

        private void barButtonItem15_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                myGridView3.PostEditor();
                myGridView3.UpdateCurrentRow();
                string BillNo = "";
                for (int i = 0; i < myGridView3.RowCount; i++)
                {
                    if (ConvertType.ToInt32(myGridView3.GetRowCellValue(i, "ischecked")) > 0 /*&& myGridView1.GetRowCellValue(i, "TransitMode").ToString() == "��ǿ�Ǽ�"*/)
                    {
                        BillNo += myGridView3.GetRowCellValue(i, "BillNo").ToString() + "@";
                    }

                    //if (CommonClass.UserInfo.WebName != "���ڹ�Դʡ�ʲ�����")
                    //{
                    if (myGridView3.GetRowCellValue(i, "BegWeb").ToString() != CommonClass.UserInfo.WebName
                        && myGridView3.GetRowCellValue(i, "SCDesWeb").ToString() != CommonClass.UserInfo.WebName)
                    {
                        MsgBox.ShowOK("ֻ�ܵ�����ǰ��������˵���");
                        return;
                    }
                    //}
                }
                if (BillNo == "")
                {
                    MsgBox.ShowOK("��ѡ����Ҫ�������˵���");
                    return;
                }
                frmSendCity fs = new frmSendCity();
                DialogResult dr = fs.ShowDialog();
                if (dr != DialogResult.Yes) return;
                if (fs.SiteName.Text.Trim() == "")
                {
                    MsgBox.ShowOK("����ѡ��Ŀ��վ��");
                    return;
                }
                if (fs.WebName.Text.Trim() == "")
                {
                    MsgBox.ShowOK("����ѡ��Ŀ������");
                    return;
                }
                if (MsgBox.ShowYesNo("ȷ������ѡ�е��˵���") != DialogResult.Yes) return;
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", BillNo));
                list.Add(new SqlPara("AcceptSiteName", fs.SiteName.Text.Trim()));
                list.Add(new SqlPara("AcceptWebName", fs.WebName.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_SNEDTOCITY", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    for (int i = myGridView3.RowCount - 1; i >= 0; i--)
                    {
                        if (ConvertType.ToInt32(myGridView3.GetRowCellValue(i, "ischecked")) > 0)
                        {
                            myGridView3.DeleteRow(i);
                        }
                    }
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void barButtonItem16_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                myGridView3.PostEditor();
                myGridView3.UpdateCurrentRow();
                string BillNo = "";
                for (int i = 0; i < myGridView3.RowCount; i++)
                {
                    if (ConvertType.ToInt32(myGridView3.GetRowCellValue(i, "ischecked")) > 0 && myGridView3.GetRowCellValue(i, "TransitMode").ToString() != "��ǿ�Ǽ�")
                    {
                        BillNo += myGridView3.GetRowCellValue(i, "BillNo").ToString() + "@";
                    }
                }
                if (BillNo == "")
                {
                    MsgBox.ShowOK("��ѡ����Ҫ������ʡ���˵���");
                    return;
                }

                frmSendDeparture fs = new frmSendDeparture();
                DialogResult dr = fs.ShowDialog();
                if (dr != DialogResult.Yes) return;
                if (fs.DepartureBatch.Text.Trim() == "")
                {
                    MsgBox.ShowOK("���뷢������");
                    return;
                }
                if (fs.SiteName.Text.Trim() == "")
                {
                    MsgBox.ShowOK("����ѡ��Ŀ��վ��");
                    return;
                }
                if (fs.WebName.Text.Trim() == "")
                {
                    MsgBox.ShowOK("����ѡ��Ŀ������");
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", BillNo));
                list.Add(new SqlPara("AcceptSiteName", fs.SiteName.Text.Trim()));
                list.Add(new SqlPara("AcceptWebName", fs.WebName.Text.Trim()));
                list.Add(new SqlPara("DepartureBatch", fs.DepartureBatch.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_SNEDDeparture", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    for (int i = myGridView3.RowCount - 1; i >= 0; i--)
                    {
                        if (ConvertType.ToInt32(myGridView3.GetRowCellValue(i, "ischecked")) > 0)
                        {
                            myGridView3.DeleteRow(i);
                        }
                    }
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void barButtonItem17_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmSendCityRecord fs = new FrmSendCityRecord();
            fs.ShowDialog();
        }

        private void barButtonItem18_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmSendInventoryColor fsic = new frmSendInventoryColor();
            fsic.ShowDialog();
            SetGridColor();
        }

        private void barButtonItem21_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void barButtonItem22_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridView gv = new GridView();

            gv = myGridControl3.MainView as GridView;

            if (gv == null || gv.FocusedRowHandle < 0) return;

            DataSet ds = new DataSet();
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", dateEdit3.DateTime));
                list.Add(new SqlPara("t2", dateEdit4.DateTime));
                list.Add(new SqlPara("CauseName", Cause1.Text.Trim() == "ȫ��" ? "%%" : Cause1.Text.Trim()));
                list.Add(new SqlPara("AreaName", Area1.Text.Trim() == "ȫ��" ? "%%" : Area1.Text.Trim()));

                list.Add(new SqlPara("StartSite", Bsite1.Text.Trim() == "ȫ��" ? "%%" : Bsite1.Text.Trim()));
                list.Add(new SqlPara("TransferSite", Esite1.Text.Trim() == "ȫ��" ? "%%" : Esite1.Text.Trim()));
                list.Add(new SqlPara("BegWeb", WebName1.Text.Trim() == "ȫ��" ? "%%" : WebName1.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_SendInventory", list);
                ds = SqlHelper.GetDataSet(sps);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                if (myGridView3.RowCount < 1000) myGridView1.BestFitColumns();
            }
            string sendList = "��������嵥";
            if (File.Exists(Application.StartupPath + "\\Reports\\" + sendList + "per.grf"))
            {
                sendList = sendList + "per.grf";
            }
            else
            {
                sendList = "��������嵥.grf";
            }


            frmPrintRuiLang fpr = new frmPrintRuiLang(sendList, ds);
            fpr.ShowDialog();
        }

        private void barButtonItem23_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView3, "�������");
        }
    }
}