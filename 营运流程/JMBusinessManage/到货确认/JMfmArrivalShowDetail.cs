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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using System.Threading;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class JMfmArrivalShowDetail : BaseForm
    {
        string _departureBatch;
        int _hitorder;
        DateTime _arriveddate;
        GridColumn gcBespeakContent;
        List<int> editRows = new List<int>();

        public int Hitorder
        {
            get { return _hitorder; }
            set { _hitorder = value; }
        }

        public string DepartureBatch
        {
            get { return _departureBatch; }
            set { _departureBatch = value; }
        }

        public DateTime Arriveddate
        {
            get { return _arriveddate; }
            set { _arriveddate = value; }
        }

        public JMfmArrivalShowDetail()
        {
            InitializeComponent();
        }

        private void fmArrivalShowDetail_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar1); //����о���Ĺ���������������ʵ��
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            gcBespeakContent = myGridView1.Columns["BespeakContent"];

            if (_hitorder == 1)
            {
                //simpleButton1.Visible = simpleButton7.Visible = false;

                getbilnos_inroad();
                this.Text = "��;�嵥";
            }
            if (_hitorder == 2)
            {
                getbilnos_arrived();
            }

            getDepartureInfo();
        }

        /// <summary>
        /// ��ȡ��;��ϸ
        /// </summary>
        private void getbilnos_inroad()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", DepartureBatch));
                list.Add(new SqlPara("site", CommonClass.UserInfo.SiteName));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURE_INROAD_FCD", list);
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

        /// <summary>
        /// ��ȡ������ϸ
        /// </summary>
        private void getbilnos_arrived()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", DepartureBatch));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURE_ARRIVED_FCD", list);
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

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string billnostr = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) == 0) continue;
                billnostr += GridOper.GetRowCellValueString(myGridView1, i, "BillNO") + "@";

                if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "BillState")) != 7
                   && ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "BillState")) != 8)
                {
                    MsgBox.ShowOK("�õ��Ѿ������δ�������޷�ȡ��������");
                    return;
                }
            }
            if (billnostr == "") return;
            if (MsgBox.ShowYesNo("ȷ��ȡ��������") != DialogResult.Yes) return;
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("DepartureBatch", _departureBatch));
            list.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
            list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));
            list.Add(new SqlPara("BillNoStr", billnostr));
            list.Add(new SqlPara("man", ""));
            list.Add(new SqlPara("type", 2));

            //��ǰ��ȡ���켣��Ϣ
            List<SqlPara> lists = new List<SqlPara>();
            lists.Add(new SqlPara("DepartureBatch", null));
            lists.Add(new SqlPara("BillNO", billnostr));
            lists.Add(new SqlPara("tracetype", "���ﵽ��"));
            lists.Add(new SqlPara("num", 7));
            DataSet dss = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_TraceSyn", lists));

            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_SET_UPDATE_BILL_ARRIVED_OK", list)) == 0) return;
            CommonClass.SetOperLog(_departureBatch, "", "", CommonClass.UserInfo.UserName, "ȡ������", "�����嵥ȡ����������");

            if (_hitorder == 1) getbilnos_inroad();
            if (_hitorder == 2) getbilnos_arrived();
            MsgBox.ShowOK();
            CommonSyn.TraceSyn(null, billnostr, 7, "���ﵽ��", 2, null,dss);
            #region ԭ�еĵ�Ʊȡ������
            /*
            string billno = "";
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle >= 0)
            {
                int state = Convert.ToInt32(myGridView1.GetRowCellValue(rowhandle, "BillState"));
                if (state != 7 && state != 8)
                {
                    MsgBox.ShowOK("�õ��Ѿ������δ�������޷�ȡ������!\r\nr���ȷʵ��Ҫȡ������,����ȡ������!");
                    return;
                }
                billno = ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "BillNO"));

                DialogResult dl = XtraMessageBox.Show("���ţ�" + billno + " �����ó���;״̬,ȷ����?", "ϵͳ��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (dl == DialogResult.Yes)
                {
                    try
                    {
                        List<SqlPara> list = new List<SqlPara>();
                        list.Add(new SqlPara("DepartureBatch", DepartureBatch));
                        list.Add(new SqlPara("BillNo", billno));
                        list.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
                        list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));

                        SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_SET_BILLDEPARTURE_ARRIVED_CANCEL_SINGLE", list);
                        if (SqlHelper.ExecteNonQuery(sps) > 0)
                        {
                            if (_hitorder == 1) getbilnos_inroad();
                            if (_hitorder == 2) getbilnos_arrived();
                            MsgBox.ShowOK();
                        }
                    }
                    catch (Exception ex)
                    {
                        MsgBox.ShowException(ex);
                    }
                }
            }
            */
            #endregion
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            string billnostr = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) == 0) continue;
                billnostr += GridOper.GetRowCellValueString(myGridView1, i, "BillNO") + "@";

                if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "BillState")) != 5
                   && ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "BillState")) != 6)
                {
                    MsgBox.ShowOK("�˵�δ��;������Ҫ������");
                    return;
                }
            }
            if (billnostr == "") return;
            if (MsgBox.ShowYesNo("ȷ����ѡ�еĵ�������") != DialogResult.Yes) return;
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("DepartureBatch", _departureBatch));
            list.Add(new SqlPara("BillNoStr", billnostr));
            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_SET_BILLDEPARTURE_ARRIVED_OK_SING", list)) == 0) return;

            if (_hitorder == 1) getbilnos_inroad();
            if (_hitorder == 2) getbilnos_arrived();
            MsgBox.ShowOK();
            CommonSyn.TraceSyn(null, billnostr, 7, "���ﵽ��", 1, null, null);
            #region ԭ�еĵ�Ʊ����
            /*
            int state = 0;
            string billno = "";

            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            try
            {
                billno = ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "BillNO"));
                state = ConvertType.ToInt32(myGridView1.GetRowCellValue(rowhandle, "BillState"));

                if (state >= 7)
                {
                    XtraMessageBox.Show("����Ϊ��" + billno + "�������õ���״̬,��Ʊ���Ѿ��ǵ�����״̬!", "ϵͳ��ʾ", MessageBoxButtons.OK);
                    return;
                }

                if (XtraMessageBox.Show("����Ϊ��" + billno + "������Ϊ����״̬��ȷ����", "ϵͳ��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", DepartureBatch));
                list.Add(new SqlPara("BillNo", billno));
                list.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("AcceptWebName", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("CauseName", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("AreaName", CommonClass.UserInfo.AreaName));
                list.Add(new SqlPara("DepName", CommonClass.UserInfo.DepartName));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_SET_BILLDEPARTURE_ARRIVED_STATE", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    if (_hitorder == 1) getbilnos_inroad();
                    if (_hitorder == 2) getbilnos_arrived();
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
            */
            #endregion
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// ��ȡ
        /// </summary>
        private void getDepartureInfo()
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("DepartureBatch", _departureBatch));
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_DEPARTURE_BY_BATCH", list));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
            DataRow dr = ds.Tables[0].Rows[0];//ȡ��0��
            ContractNO.EditValue = dr["ContractNO"];
            DepartureBatchs.EditValue = dr["DepartureBatch"];
            CarNO.EditValue = dr["CarNO"];
            CarrNO.EditValue = dr["CarrNO"];
            DriverName.EditValue = dr["DriverName"];
            DriverPhone.EditValue = dr["DriverPhone"];
            LoadWeight.EditValue = dr["LoadWeight"];
            LoadVolume.EditValue = dr["LoadVolume"];
            BeginSite.EditValue = dr["BeginSite"];
            EndSite.EditValue = dr["EndSite"];
            ActWeight.EditValue = dr["ActWeight"];
            ActVolume.EditValue = dr["ActVolume"];
            DepartureDate.EditValue = dr["DepartureDate"];
            ExpArriveDate.EditValue = dr["ExpArriveDate"];
            LoadPeoples.EditValue = dr["LoadPeoples"];
            Creator.EditValue = dr["Creator"];
            LoadingType.EditValue = dr["LoadingType"];
            BoxNO.EditValue = dr["BoxNO"];
            BigCarDescr.EditValue = dr["BigCarDescr"];
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;

            DataRow dr = myGridView1.GetDataRow(myGridView1.FocusedRowHandle);

            if (dr == null) return;

            JMfrmAppointmentSend frm = new JMfrmAppointmentSend();
            frm.crrBillNO = dr["BillNO"].ToString();
            frm.ShowDialog();

            //billnostr
            //if (simpleButton2.Text == "ԤԼ�ͻ�")
            //{
            //    simpleButton2.Text = "������Ϣ";
            //    gcBespeakContent.OptionsColumn.AllowEdit = gcBespeakContent.OptionsColumn.AllowFocus = true;
            //    myGridView1.FocusedColumn = gcBespeakContent;
            //}
            //else
            //{
            //    simpleButton2.Text = "ԤԼ�ͻ�";
            //    gcBespeakContent.OptionsColumn.AllowEdit = gcBespeakContent.OptionsColumn.AllowFocus = false;
            //    //����ԤԼ�ͻ���Ϣ
            //    string billnostr = "", bespeakContentstr = "";
            //    for (int i = 0; i < editRows.Count; i++)
            //    {
            //        billnostr += ConvertType.ToString(myGridView1.GetRowCellValue(editRows[i], "BillNO")) + "@";
            //        bespeakContentstr += ConvertType.ToString(myGridView1.GetRowCellValue(editRows[i], gcBespeakContent)).Replace('@', '_') + "@";
            //    }
            //    editRows.Clear();
            //    if (billnostr == "") return;

            //    List<SqlPara> list = new List<SqlPara>();
            //    list.Add(new SqlPara("BillNoStr", billnostr));
            //    list.Add(new SqlPara("BespeakContentStr", bespeakContentstr));
            //    if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_SET_SEND_BESPEAKCONTENT", list)) == 0) return;
            //    CommonClass.SetOperLog(_departureBatch, "", "", CommonClass.UserInfo.UserName, "ԤԼ�ͻ�", "�����嵥ԤԼ�ͻ�����");
            //    MsgBox.ShowOK();
            //}
        }

        private void myGridView1_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e == null || e.RowHandle < 0 || editRows.Contains(e.RowHandle)) return;
            editRows.Add(e.RowHandle);
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            //sms.nowsendsms_to_shipper(myGridView1, _arriveddate);
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            //sms.nowsendsms(myGridView1, this, "1");
        }

        private void cbprint_Click(object sender, EventArgs e)
        {
            if (DepartureBatch == "") return;
            string middleSite = "";
            JMfrmSelectPrintDepartureList fsp = new JMfrmSelectPrintDepartureList();
            bool flag = false;
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                middleSite = ConvertType.ToString(myGridView1.GetRowCellValue(i, "TransferSite"));
                if (middleSite == "") continue;
                flag = false;
                for (int j = 0; j < fsp.checkedListBox1.Items.Count; j++)
                {
                    if (ConvertType.ToString(fsp.checkedListBox1.Items[j]) == middleSite) flag = true;
                }
                if (!flag) fsp.checkedListBox1.Items.Add(middleSite);
            }
            if (fsp.ShowDialog() != DialogResult.OK) return;

            if (fsp.printSite == "")
            {
                MsgBox.ShowOK("û��ѡ���ӡվ��!");
                return;
            }

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_DEPARTURELIST_BY_DEPARTUREBATCH_PRINT", new List<SqlPara> { new SqlPara("DepartureBatch", DepartureBatch), new SqlPara("MiddleSiteStr", fsp.printSite) }));
            if (ds == null || ds.Tables.Count == 0) return;

            string tmps = "";
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                tmps = ConvertType.ToString(ds.Tables[0].Rows[i]["DestinationSite"]);
                if (tmps == "") continue;
                try { ds.Tables[0].Rows[i]["DestinationSite"] = tmps.Split(' ')[1]; }
                catch { }
            }

            //zaj 2018-1-15 ˾������Э����ݹ�˾ID������
            string transprotocol = CommonClass.UserInfo.Transprotocol == "" ? "˾������Э��" : CommonClass.UserInfo.Transprotocol;
           // frmPrintRuiLang fpr = new frmPrintRuiLang((fsp.printType == 0 ? "�����嵥" : fsp.printType == 1 ? "װ���嵥" : "˾������Э��"), ds);
            frmPrintRuiLang fpr = new frmPrintRuiLang((fsp.printType == 0 ? "�����嵥" : fsp.printType == 1 ? "װ���嵥" : transprotocol), ds);

            fpr.ShowDialog();

            //if (string.IsNullOrEmpty(DepartureBatch)) return;
            //frmPrintRuiLang fprl = new frmPrintRuiLang("�����嵥", SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURE_ARRIVED_FCD_PRINT", new List<SqlPara> { new SqlPara("DepartureBatch", DepartureBatch) })));
            //fprl.ShowDialog();
        }

        private void barbtnPrintQSD_ItemClick(object sender, ItemClickEventArgs e)
        {
            int[] rows = myGridView1.GetSelectedRows();
            if (rows.Length == 0)
            {
                MsgBox.ShowOK("��ѡ��Ҫ��ӡ���˵�!");
                return;
            }
            string str = "";
            for (int i = 0; i < rows.Length; i++)
            {
                str += ConvertType.ToString(myGridView1.GetRowCellValue(rows[i], "BillNO")) + "@";
            }
            PrintQSD(str);
        }

        private void barbtnPrintGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (myGridView1.RowCount == 0)
            {
                MsgBox.ShowOK("û���˵������ܴ�ӡ!");
                return;
            }
            string str = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                str += ConvertType.ToString(myGridView1.GetRowCellValue(i, "BillNO")) + "@";
            }
            PrintQSD(str);
        }

        private void PrintQSD(string BillNoStr)
        {
            if (string.IsNullOrEmpty(BillNoStr)) return;
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILLINFO_PRINT_QSD", new List<SqlPara> { new SqlPara("BillNoStr", BillNoStr), new SqlPara("DepartureBatch", DepartureBatch) }));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                MsgBox.ShowError("û���ҵ�ѡ�е��˵���Ϣ,��ӡʧ��,(����������˵��Ƿ��ѱ�ɾ��)!");
                return;
            }
            //DataTable dt = ds.Tables[0].Clone();
            //frmPrintRuiLang fprl;
            //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //{
            //    dt.ImportRow(ds.Tables[0].Rows[i]);
            //    //fprl = new frmPrintRuiLang("�����", dt);
            //    //fprl.ShowDialog();
            //}
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

        private void ddbtnPrintQSD_Click(object sender, EventArgs e)
        {
            ddbtnPrintQSD.ShowDropDown();
        }
    }
}