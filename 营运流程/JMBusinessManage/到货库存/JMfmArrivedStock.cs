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

namespace ZQTMS.UI
{
    public partial class JMfmArrivedStock : BaseForm
    {
        public JMfmArrivedStock()
        {
            InitializeComponent();
        }

        private void fmArrivedStock_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, myGridView2);
            GridOper.SetGridViewProperty(myGridView1, myGridView2);
            BarMagagerOper.SetBarPropertity(bar1, bar3);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            FixColumn fix2 = new FixColumn(myGridView2, barSubItem4);

            CommonClass.SetSite(bsite, true);
            CommonClass.SetSite(bsite2, true);
            CommonClass.SetSite(esite, true);
            CommonClass.SetSite(esite2, true);
            CommonClass.SetCause(Cause, true);
            CommonClass.SetCause(comboBoxEdit1, true);

            Cause.Text = comboBoxEdit1.Text = CommonClass.UserInfo.CauseName;
            Area.Text = comboBoxEdit2.Text = CommonClass.UserInfo.AreaName;
            bsite.Text = bsite2.Text = "ȫ��";
            esite.Text = esite2.Text = CommonClass.UserInfo.SiteName;
            web.Text = web2.Text = CommonClass.UserInfo.WebName;
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;

            GridOper.RestoreGridLayout(myGridView1, myGridView2);
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            dateEdit2.DateTime = CommonClass.gbdate;
            dateEdit1.DateTime = CommonClass.gedate;

            GridOper.CreateStyleFormatCondition(myGridView1, "remaindays", FormatConditionEnum.GreaterOrEqual, 3, Color.FromArgb(255, 255, 128));
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

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)//zxw �¼ӡ��״�ǩ�յ��� 2016-12-24
        {
            myGridView1.PostEditor();
            string BillNoStr = ""; 
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) > 0)
                    BillNoStr += GridOper.GetRowCellValueString(myGridView1, i, "BillNo") + "@";
            }
            if (BillNoStr == "") return;

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILLINFO_PRINT_ARRIVED_QSD", new List<SqlPara> { new SqlPara("BillNoStr", BillNoStr) }));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                MsgBox.ShowOK("û���ҵ�ѡ�е��˵���Ϣ,��ӡʧ��,(����������˵��Ƿ��ѱ�ɾ��)!");
                return;
            }
            //frmRuiLangService.Print("�����", ds.Tables[0]);
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
                frmRuiLangService.Print("�������ƺ", ds.Tables[0]);
            }
            else
            {
                frmRuiLangService.Print("�����", ds.Tables[0]);
            }
        }
    }
}