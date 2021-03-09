using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Reflection;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using System.Threading;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmDepartureModyPre : BaseForm
    {
        public frmDepartureModyPre()
        {
            InitializeComponent();
        }

        public string sDepartureBatch = "";
        public object _arriveDate = null;
        private DataSet dataset2 = new DataSet();
        private int state = 0;//Ϊ1ʱ��ȥ��ȡ�ӻ�����

        private void frmDepartureMody_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, myGridView2);
            GridOper.SetGridViewProperty(myGridView1, myGridView2);


            BarMagagerOper.SetBarPropertity(bar1); //����о���Ĺ���������������ʵ��
            FixColumn fix = new FixColumn(myGridView2, barSubItem2);
            GridOper.RestoreGridLayout(myGridView1, myGridView2);

            CommonClass.SetSite(UnloadAddr, false);
            CommonClass.SetSite(edesite1, false);
            CommonClass.SetSite(edesite2, false);
            CommonClass.SetSite(edesite3, false);

            edesite1.Properties.Items.Remove(CommonClass.UserInfo.SiteName);
            edesite2.Properties.Items.Remove(CommonClass.UserInfo.SiteName);
            edesite3.Properties.Items.Remove(CommonClass.UserInfo.SiteName);

            CommonClass.SetWeb(ShtBargeDept, CommonClass.UserInfo.SiteName, false);
            CommonClass.SetWeb(TakeDept, CommonClass.UserInfo.SiteName, false);

            //���ݷ������κ�  ȡ�ñ����г������嵥
            freshData();
            getDepartureInfo();
        }
        private void freshData()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", sDepartureBatch));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURELIST_BY_DEPARTUREBATCHPRE", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                dataset2 = ds;
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

        /// <summary>
        /// ��ȡ
        /// </summary>
        private void getDepartureInfo()
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("DepartureBatch", sDepartureBatch));
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_DEPARTUREPRE_BY_BATCH", list));
            if (ds == null || ds.Tables.Count == 0) return;
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];//ȡ��0��
                ContractNO.EditValue = dr["ContractNO"]; labContractNO.Text = ContractNO.Text;
                DepartureBatch.EditValue = dr["DepartureBatch"]; labDepartureBatch.Text = DepartureBatch.Text;
                CarNO.EditValue = dr["CarNO"];
                CarrNO.EditValue = dr["CarrNO"];
                DriverName.EditValue = dr["DriverName"]; labDriverName.Text = DriverName.Text;
                DriverPhone.EditValue = dr["DriverPhone"];
                LoadWeight.EditValue = dr["LoadWeight"];
                LoadVolume.EditValue = dr["LoadVolume"];
                BeginSite.EditValue = dr["BeginSite"];
                EndSite.EditValue = dr["EndSite"];
                ActWeight.EditValue = dr["ActWeight"];
                ActVolume.EditValue = dr["ActVolume"];
                DepartureDate.EditValue = dr["DepartureDate"]; labDepartureDate.Text = DepartureDate.Text;
                ExpArriveDate.EditValue = dr["ExpArriveDate"];
                LoadPeoples.EditValue = dr["LoadPeoples"]; labLoadPeoples.Text = LoadPeoples.Text;
                Creator.EditValue = dr["Creator"]; lbcreateby.Text = Creator.Text;
                LoadingType.EditValue = dr["LoadingType"];
                BoxNO.EditValue = dr["BoxNO"];
                BigCarDescr.EditValue = dr["BigCarDescr"];
                AccLine.EditValue = dr["AccLine"];
                FecthSite.EditValue = dr["FecthSite"];
                AccBigCarFecth.EditValue = dr["AccBigCarFecth"];
                FecthBillNO.EditValue = dr["FecthBillNO"];
                AccBigCarSend.EditValue = dr["AccBigCarSend"];
                SendBillNO.EditValue = dr["SendBillNO"];
                SendAddr.EditValue = dr["SendAddr"];
                AccShtBarge.EditValue = dr["AccShtBarge"];
                ShtBargeDept.EditValue = dr["ShtBargeDept"];
                AccSomeLoad.EditValue = dr["AccSomeLoad"];
                UnloadAddr.EditValue = dr["UnloadAddr"];
                AccBigcarOther.EditValue = dr["AccBigcarOther"];
                TakeDept.EditValue = dr["TakeDept"];
                DriverTakePay.EditValue = dr["DriverTakePay"];
                AccTakeCar.EditValue = dr["AccTakeCar"];
                AccCollectPremium.EditValue = dr["AccCollectPremium"];
                NowPayDriver.EditValue = dr["NowPayDriver"];
                ToPayDriver.EditValue = dr["ToPayDriver"];
                BackPayDriver.EditValue = dr["BackPayDriver"];
                AccBigcarTotal.EditValue = dr["AccBigcarTotal"];//�󳵷Ѻϼ�
                DriverTakePay.EditValue = dr["DriverTakePay"];
                AccTakeCar.EditValue = dr["AccTakeCar"];
                AccCollectPremium.EditValue = dr["AccCollectPremium"];
                NowPayDriver.EditValue = dr["NowPayDriver"];
                BackPayDriver.EditValue = dr["BackPayDriver"];
                edrepremark.EditValue = dr["BigCarDescr"];

                if (LoadingType.Text == "��;����")
                {
                    btnFetchData.Enabled = true;
                    simpleButton15.Enabled = simpleButton16.Enabled = false;
                }
                else if (LoadingType.Text == "��������")
                {
                    simpleButton15.Enabled = true;
                    btnFetchData.Enabled = simpleButton16.Enabled = false;
                }
                else//��ǿ��Ŀ
                {
                    simpleButton16.Enabled = true;
                    btnFetchData.Enabled = simpleButton15.Enabled = false;
                }

                try
                {
                    int isover = ConvertType.ToInt32(dr["isover"]);
                    SetButtonState(isover == 0);
                }
                catch { }
            }
            //ȡ������ʻԱ
            if (ds.Tables.Count < 2 || ds.Tables[1].Rows.Count == 0) return;
            try
            {
                edesite1.EditValue = ds.Tables[1].Rows[0][0];
                edesiteacc1.EditValue = ds.Tables[1].Rows[0][1];
                edesite2.EditValue = ds.Tables[1].Rows[1][0];
                edesiteacc2.EditValue = ds.Tables[1].Rows[1][1];
                edesite3.EditValue = ds.Tables[1].Rows[2][0];
                edesiteacc3.EditValue = ds.Tables[1].Rows[2][1];
            }
            catch { }
        }

        private void btnFetchData_Click(object sender, EventArgs e)
        {
            getdata(1);
        }

        private void getdata(int type)
        {
            //ȡ�ÿ��
            //if (!HtIsPrint(sDepartureBatch))
           // {
                try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
                    list.Add(new SqlPara("webName", CommonClass.UserInfo.WebName));
                    list.Add(new SqlPara("LoadType", type));
                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_STOWAGE_LOADPRE", list);
                    DataSet ds = SqlHelper.GetDataSet(sps);
                    if (ds == null || ds.Tables.Count == 0) return;
                    myGridControl1.DataSource = ds.Tables[0];
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
           // }
        }

        private bool HtIsPrint(string sDepartureBatch)
        {
            return true;
            //�жϷ�����ͬ�Ƿ��Ѿ���ӡ
            /*
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", sDepartureBatch));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURE_ISPRINT", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return false;

                if (ConvertType.ToString(ds.Tables[0].Rows[0][0]) != "" && ConvertType.ToString(ds.Tables[0].Rows[0][1]) != "")
                {
                    MsgBox.ShowOK("�����Ѿ���ӡ��ͬ���嵥�����ɵ���������!\r\n���ȷʵ��Ҫ����������ϵ�����Ա��Ȩ!");
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                return false;
            }
            */
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //������
            int[] rows = myGridView1.GetSelectedRows();
            if (rows.Length == 0)
            {
                XtraMessageBox.Show("�뵥��ѡ��Ҫ������˵�!\r\nҲ���԰�סCtrl��,Ȼ����Ҫ������˵��ϵ�����������ѡ���Ʊ!", "ϵͳ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (XtraMessageBox.Show("ȷ����ѡ�е��˵����뵽�����嵥��?", "ϵͳ��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;
            try
            {
                string BillNos = "", factqtys = "";
                for (int i = 0; i < rows.Length; i++)
                {
                    BillNos += ConvertType.ToString(myGridView1.GetRowCellValue(rows[i], "BillNo")) + ",";
                    factqtys += ConvertType.ToInt32(myGridView1.GetRowCellValue(rows[i], "factqty")) + ",";
                }
                if (BillNos == "") return;

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", sDepartureBatch));
                list.Add(new SqlPara("CarNO", labCarNO.Text.Trim()));
                list.Add(new SqlPara("DriverName", labDriverName.Text.Trim()));
                list.Add(new SqlPara("WebDate", labDepartureDate.Text.Trim()));
                list.Add(new SqlPara("BillNOStr", BillNos));
                list.Add(new SqlPara("FactQtyStr", factqtys));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLDEPARTURELISTPRE", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                //�����б�
                freshData();
            }
        }
        private bool CheckInOneVehicle(ref string sBillNO)
        {//gridView2�嵥
            int[] rows = myGridView1.GetSelectedRows();
            string BillNO = "";
            for (int i = 0; i < rows.Length; i++)
            {
                BillNO = ConvertType.ToString(myGridView1.GetRowCellValue(rows[i], "BillNO"));
                DataRow[] dr = dataset2.Tables[0].Select("BillNO=" + BillNO + "");
                if (dr.Length > 0)
                {
                    sBillNO += BillNO + ",";
                }
                myGridView1.DeleteRow(rows[i]);
                myGridView1.PostEditor();
                myGridView1.UpdateCurrentRow();
                myGridView1.UpdateSummary();
            }
            sBillNO = sBillNO.TrimEnd(',');
            return sBillNO == "" ? false : true;
        }

        private void chkALL_CheckedChanged(object sender, EventArgs e)
        {
            int a = chkALL.Checked == true ? 1 : 0;
            for (int i = 0; i < dataset2.Tables[0].Rows.Count; i++)
            {
                dataset2.Tables[0].Rows[i]["ischecked"] = a;
            }
            myGridControl2.DataSource = dataset2.Tables[0];
        }

        private void cbdeleteall_Click(object sender, EventArgs e)
        {
            if (HtIsPrint(sDepartureBatch)) return;
            try
            {
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    if (ConvertType.ToString(myGridView2.GetRowCellValue(i, "ToDate")) != "")
                    {
                        MsgBox.ShowOK("�����Ѿ��е���,��������!\r\n���ȷʵ��Ҫ����,����ϵ������վ������Աȡ����������!");
                        return;
                    }
                }

                if (MsgBox.ShowYesNo("ȷ�����ϱ�����") != DialogResult.Yes) return;

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", sDepartureBatch));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_DELETE_BILLDEPARTUREPRE", list)) == 0) return;
                MsgBox.ShowOK("�������ϳɹ�!");
                cbcancelover.Enabled = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void cbdeletesingle_Click(object sender, EventArgs e)
        {
            myGridView2.ClearColumnsFilter();
            if (myGridView2.RowCount == 1)
            {
                if (MsgBox.ShowYesNo("����ֻʣ��һƱ��,����޳���ǰ�˵�,������������Ҳ������!\r\n\r\nȷ��Ҫ������?") == DialogResult.Yes)
                {
                    cbdeleteall.PerformClick();
                }
                return;
            }

            int rowhandle = myGridView2.FocusedRowHandle;
            if (rowhandle < 0) return;
            try
            {
                string billno = ConvertType.ToString(myGridView2.GetRowCellValue(rowhandle, "BillNO"));
                if (ConvertType.ToString(myGridView2.GetRowCellValue(rowhandle, "ToDate")) != "")
                {
                    MsgBox.ShowOK("�����Ѿ�����,�����޳�!\r\n���ȷʵ��Ҫ�޳�,����ϵ������վ������Աȡ����������!");
                    return;
                }

                if (MsgBox.ShowYesNo("ȷ��Ҫ�޳���Ʊ��") != DialogResult.Yes) return;

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", ConvertType.ToString(myGridView2.GetRowCellValue(rowhandle, "DepartureBatch"))));
                list.Add(new SqlPara("BillNo", billno));
                list.Add(new SqlPara("WebPCS", ConvertType.ToInt32(myGridView2.GetRowCellValue(rowhandle, "WebPCS"))));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_DELETE_DEPARTURE_SINGPRE", list)) == 0) return;
                myGridView2.DeleteRow(rowhandle);
                MsgBox.ShowOK();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1, myGridView2);
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView2);
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString(), myGridView2.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1, myGridView2);
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView2, "Ԥ������ϸ�嵥");
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            GridOper.ExportToExcel(myGridView2, "Ԥ���ؿ���嵥");
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            groupControl1.Visible = !groupControl1.Visible;
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            decimal accBigcarTotal = ConvertType.ToDecimal(AccBigcarTotal.Text);
            if (accBigcarTotal <= 0)
            {
                if (MsgBox.ShowYesNo("��ͬû�����˷ѣ�ȷ����ɱ�����") != DialogResult.Yes) return;
            }
            if (!SetBillDepartureState(1)) return;
            SetButtonState(false);
        }

        /// <summary>
        /// �����״̬
        /// </summary>
        /// <param name="state">0δ���,1�����</param>
        private bool SetBillDepartureState(int state)
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("DepartureBatch", sDepartureBatch));
            list.Add(new SqlPara("state", state));
            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_SET_BILLDEPARTURE_STATEPRE", list)) == 0) return false;
            MsgBox.ShowOK();
            return true;
        }

        /// <summary>
        /// ���ð�ť״̬
        /// </summary>
        /// <param name="state">�ؼ�����״̬</param>
        private void SetButtonState(bool state)
        {
            btnAdd.Enabled = cbdeletesingle.Enabled = cbdeleteall.Enabled = simpleButton12.Enabled = simpleButton18.Enabled = simpleButton11.Enabled = simpleButton3.Enabled = state;
            cbcancelover.Enabled = !state;
        }

        private void cbcancelover_Click(object sender, EventArgs e)
        {
            if (!SetBillDepartureState(0)) return;
            SetButtonState(true);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            string vehicleno = CarNO.Text.Trim();
            if (vehicleno == "")
            {
                MsgBox.ShowError("���ƺű�����д!");
                CarNO.Focus();
                return;
            }
            string loadingType = LoadingType.Text.Trim();
            if (loadingType == "")
            {
                MsgBox.ShowError("��ѡ����������!");
                LoadingType.Focus();
                return;
            }
            decimal accLine = ConvertType.ToDecimal(AccLine.Text);
            if (accLine == 0)
            {
                MsgBox.ShowError("����д�����˷�!");
                AccLine.Focus();
                return;
            }
            decimal accBigCarFecth = ConvertType.ToDecimal(AccBigCarFecth.Text);
            string fecthBillNO = FecthBillNO.Text.Trim();
            if (accBigCarFecth != 0 && fecthBillNO == "")
            {
                MsgBox.ShowError("�����˴󳵽ӻ���,������д�ӻ�����!");
                FecthBillNO.Focus();
                return;
            }
            decimal accBigCarSend = ConvertType.ToDecimal(AccBigCarSend.Text);
            string sendBillNO = SendBillNO.Text.Trim();
            if (accBigCarSend != 0 && sendBillNO == "")
            {
                MsgBox.ShowError("�����˴��ͻ���,������д�ͻ�����(����ͻ������á�/������)!");
                SendBillNO.Focus();
                return;
            }
            decimal accShtBarge = ConvertType.ToDecimal(AccShtBarge.Text);
            string shtBargeDept = ShtBargeDept.Text.Trim();
            if (accShtBarge != 0 && shtBargeDept == "")
            {
                MsgBox.ShowError("�����˶̲���,������д�̲��ѵĳ��˲���!");
                ShtBargeDept.Focus();
                return;
            }
            decimal accBigcarOther = ConvertType.ToDecimal(AccBigcarOther.Text);
            string takeDept = TakeDept.Text.Trim();
            if (accBigcarOther != 0 && takeDept == "")
            {
                MsgBox.ShowError("�����˴�������,������д�������ѵĳ��˲���!");
                TakeDept.Focus();
                return;
            }
            string bigcarOtherRemark = BigcarOtherRemark.Text.Trim();
            if (accBigcarOther != 0 && bigcarOtherRemark == "")
            {
                MsgBox.ShowError("��������ע����!");
                BigcarOtherRemark.Focus();
                return;
            }
            decimal driverTakePay = 0, accTakeCar = 0, accCollectPremium = 0, nowPayDriver = 0, toPayDriver = 0, backPayDriver = 0;
            driverTakePay = ConvertType.ToDecimal(DriverTakePay.Text);//˾������
            accTakeCar = ConvertType.ToDecimal(AccTakeCar.Text);//�����ɳ���
            accCollectPremium = ConvertType.ToDecimal(AccCollectPremium.Text);//���ձ��շ�
            nowPayDriver = ConvertType.ToDecimal(NowPayDriver.Text);
            toPayDriver = ConvertType.ToDecimal(ToPayDriver.Text);
            backPayDriver = ConvertType.ToDecimal(BackPayDriver.Text);
            if (ConvertType.ToDecimal(AccBigcarTotal.Text) != (driverTakePay + accTakeCar + accCollectPremium + nowPayDriver + toPayDriver + backPayDriver))
            {
                MsgBox.ShowError("�󳵷Ѻϼ�=˾�����տ�+�����ɳ���+���ձ��շ�+�ָ���ʻԱ+������ʻԱ+�ظ���ʻԱ,����!");
                return;
            }
            string esite1 = edesite1.Text.Trim();
            decimal eacc1 = esite1 == "" ? 0 : ConvertType.ToDecimal(edesiteacc1.Text);
            if (esite1 == "" && eacc1 != 0)
            {
                MsgBox.ShowError(string.Format("�����˵�վ�㡮{0}��,������{0}�ķ���!", esite1));
                edesiteacc1.Focus();
                return;
            }
            string esite2 = edesite2.Text.Trim();
            decimal eacc2 = esite2 == "" ? 0 : ConvertType.ToDecimal(edesiteacc2.Text);
            if (esite2 == "" && eacc2 != 0)
            {
                MsgBox.ShowError(string.Format("�����˵�վ�㡮{0}��,������{0}�ķ���!", esite2));
                edesiteacc2.Focus();
                return;
            }
            string esite3 = edesite3.Text.Trim();
            decimal eacc3 = esite3 == "" ? 0 : ConvertType.ToDecimal(edesiteacc3.Text);
            if (esite3 == "" && eacc3 != 0)
            {
                MsgBox.ShowError(string.Format("�����˵�վ�㡮{0}��,������{0}�ķ���!", esite3));
                edesiteacc3.Focus();
                return;
            }

            if (MsgBox.ShowYesNo("�Ƿ񱣴��ͬ��") != DialogResult.Yes) return;

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ContractNO", ContractNO.Text.Trim()));
                list.Add(new SqlPara("DepartureBatch", DepartureBatch.Text.Trim()));
                list.Add(new SqlPara("CarNO", CarNO.Text.Trim()));
                list.Add(new SqlPara("CarrNO", CarrNO.Text.Trim()));
                list.Add(new SqlPara("DriverName", DriverName.Text.Trim()));
                list.Add(new SqlPara("DriverPhone", DriverPhone.Text.Trim()));
                list.Add(new SqlPara("BeginSite", BeginSite.Text.Trim()));
                list.Add(new SqlPara("EndSite", EndSite.Text.Trim()));
                list.Add(new SqlPara("DepartureDate", DepartureDate.DateTime));
                list.Add(new SqlPara("ExpArriveDate", ExpArriveDate.DateTime));
                list.Add(new SqlPara("LoadWeight", ConvertType.ToDecimal(LoadWeight.Text)));
                list.Add(new SqlPara("LoadVolume", ConvertType.ToDecimal(LoadVolume.Text)));
                list.Add(new SqlPara("ActWeight", ConvertType.ToDecimal(ActWeight.Text)));
                list.Add(new SqlPara("ActVolume", ConvertType.ToDecimal(ActVolume.Text)));
                list.Add(new SqlPara("LoadPeoples", LoadPeoples.Text.Trim()));
                list.Add(new SqlPara("Creator", Creator.Text.Trim()));
                list.Add(new SqlPara("BoxNO", BoxNO.Text.Trim()));
                list.Add(new SqlPara("BigCarDescr", BigCarDescr.Text.Trim()));
                list.Add(new SqlPara("LoadingType", loadingType));
                list.Add(new SqlPara("AccLine", accLine));//�����˷�
                list.Add(new SqlPara("FecthSite", FecthSite.Text.Trim()));//�ӻ���
                list.Add(new SqlPara("AccBigCarFecth", accBigCarFecth));//�󳵽ӻ���
                list.Add(new SqlPara("FecthBillNO", fecthBillNO));
                list.Add(new SqlPara("AccBigCarSend", accBigCarSend));
                list.Add(new SqlPara("SendBillNO", sendBillNO));
                list.Add(new SqlPara("SendAddr", SendAddr.Text.Trim()));
                list.Add(new SqlPara("AccShtBarge", ConvertType.ToDecimal(AccShtBarge.Text)));
                list.Add(new SqlPara("ShtBargeDept", ShtBargeDept.Text.Trim()));
                list.Add(new SqlPara("AccSomeLoad", ConvertType.ToDecimal(AccSomeLoad.Text)));
                list.Add(new SqlPara("UnloadAddr", UnloadAddr.Text));
                list.Add(new SqlPara("AccBigcarOther", accBigcarOther));
                list.Add(new SqlPara("TakeDept", takeDept));
                list.Add(new SqlPara("AccBigcarTotal", ConvertType.ToDecimal(AccBigcarTotal.Text)));
                list.Add(new SqlPara("DriverTakePay", driverTakePay));
                list.Add(new SqlPara("AccTakeCar", accTakeCar));
                list.Add(new SqlPara("AccCollectPremium", accCollectPremium));
                list.Add(new SqlPara("NowPayDriver", nowPayDriver));
                list.Add(new SqlPara("ToPayDriver", toPayDriver));
                list.Add(new SqlPara("BackPayDriver", backPayDriver));
                list.Add(new SqlPara("edesite1", esite1));
                list.Add(new SqlPara("edesiteacc1", eacc1));
                list.Add(new SqlPara("edesite2", esite2));
                list.Add(new SqlPara("edesiteacc2", eacc2));
                list.Add(new SqlPara("edesite3", esite3));
                list.Add(new SqlPara("edesiteacc3", eacc3));
                list.Add(new SqlPara("BigcarOtherRemark", bigcarOtherRemark));
                list.Add(new SqlPara("DeliCode", fecthBillNO));

                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_UPDATE_BILLDEPARTUREPRE", list)) == 0) return;
                MsgBox.ShowOK("��ͬ����ɹ�!");
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void myGridView1_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e == null || e.Value == null) return;
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;

            try
            {
                int leftNum = ConvertType.ToInt32(myGridView1.GetRowCellValue(rowhandle, "LeftNum"));
                int factQty = ConvertType.ToInt32(e.Value);

                if (factQty <= 0)
                {
                    e.Valid = false;
                    e.ErrorText = "ʵ�������������0!";
                }
                else if (factQty > leftNum)
                {
                    e.Valid = false;
                    e.ErrorText = "ʵ���������ܴ���ʣ�����!";
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void myGridView1_InvalidValueException(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            MsgBox.ShowError(e.ErrorText);
        }

        private void CalAccBigcarTotal(object sender, EventArgs e)
        {
            try
            {
                AccBigcarTotal.Text = (ConvertType.ToDecimal(AccLine.Text) + ConvertType.ToDecimal(AccBigCarFecth.Text) + ConvertType.ToDecimal(AccBigCarSend.Text) + ConvertType.ToDecimal(AccShtBarge.Text) + ConvertType.ToDecimal(AccSomeLoad.Text) + ConvertType.ToDecimal(AccBigcarOther.Text)).ToString();
            }
            catch (Exception ex)
            {
                AccBigcarTotal.Text = "";
                MsgBox.ShowError(ex.Message);
            }
        }

        private void edesiteacc1_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (edesite1.Text.Trim() == "")
                e.Cancel = true;
        }

        /// <summary>
        /// ���㵽��˾��
        /// </summary>
        private void CalToPayDriver(object sender, EventArgs e)
        {
            try
            {
                ToPayDriver.Text = (ConvertType.ToDecimal(edesiteacc1.Text) + ConvertType.ToDecimal(edesiteacc2.Text) + ConvertType.ToDecimal(edesiteacc3.Text)).ToString();
            }
            catch (Exception ex) { ToPayDriver.Text = ""; MsgBox.ShowError(ex.Message); }
        }

        private void frmDepartureMody_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cbcancelover.Enabled) return;
            if (MsgBox.ShowYesNo("������δ��ɣ��Ƿ���ɱ�����") != DialogResult.Yes) return;
            SetBillDepartureState(1);
        }

        private void edesiteacc2_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (edesite2.Text.Trim() == "")
                e.Cancel = true;
        }

        private void edesiteacc3_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (edesite3.Text.Trim() == "")
                e.Cancel = true;
        }

        private void edesite1_EditValueChanged(object sender, EventArgs e)
        {
            if (edesite1.Text.Trim() == "")
            {
                edesiteacc1.EditValueChanging -= edesiteacc1_EditValueChanging;
                edesiteacc1.Text = "";
                edesiteacc1.EditValueChanging += edesiteacc1_EditValueChanging;
            }
        }

        private void edesite2_EditValueChanged(object sender, EventArgs e)
        {
            if (edesite2.Text.Trim() == "")
            {
                edesiteacc2.EditValueChanging -= edesiteacc2_EditValueChanging;
                edesiteacc2.Text = "";
                edesiteacc2.EditValueChanging += edesiteacc2_EditValueChanging;
            }
        }

        private void edesite3_EditValueChanged(object sender, EventArgs e)
        {
            if (edesite3.Text.Trim() == "")
            {
                edesiteacc3.EditValueChanging -= edesiteacc3_EditValueChanging;
                edesiteacc3.Text = "";
                edesiteacc3.EditValueChanging += edesiteacc3_EditValueChanging;
            }
        }

        private void edesite1_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            string s = ConvertType.ToString(e.NewValue);
            if (s != "" && (edesite2.Text.Trim() == s || edesite3.Text.Trim() == s))
            {
                e.Cancel = true;
            }
        }

        private void edesite2_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            string s = ConvertType.ToString(e.NewValue);
            if (s != "" && (edesite1.Text.Trim() == s || edesite3.Text.Trim() == s))
            {
                e.Cancel = true;
            }
        }

        private void edesite3_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            string s = ConvertType.ToString(e.NewValue);
            if (s != "" && (edesite2.Text.Trim() == s || edesite1.Text.Trim() == s))
            {
                e.Cancel = true;
            }
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            //������
            DateTime senddate = Convert.ToDateTime(labDepartureDate.Text.ToString().Trim());
            sms.fcdsendsms(myGridView2, this, "1", senddate, ExpArriveDate.DateTime);
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            //�ջ���
            if (_arriveDate == null)
            {
                if (XtraMessageBox.Show("����δ����,ȷ�Ϸ��ͻ���ǰ������֪ͨ?\r\nϵͳ��Ĭ�ϵ���ʱ��Ϊ��ǰʱ��!", "ϵͳ��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
            }
            sms.fukuansms(myGridView2, this, "1", _arriveDate == null ? CommonClass.gcdate : Convert.ToDateTime(_arriveDate));
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            //����ǰ��
            DateTime senddate = Convert.ToDateTime(labDepartureDate.Text.ToString().Trim());
            sms.fcdsendsms_consignee(myGridView2, this, "1", senddate, labCarNO.Text.Trim());
        }

        private void cbprint_Click(object sender, EventArgs e)
        {
            if (sDepartureBatch == "") return;
            string middleSite = "";
            frmSelectPrintDepartureList fsp = new frmSelectPrintDepartureList();
            bool flag = false;
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                middleSite = ConvertType.ToString(myGridView2.GetRowCellValue(i, "TransferSite"));
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

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_DEPARTURELIST_BY_DEPARTUREBATCH_PRINTPRE", new List<SqlPara> { new SqlPara("DepartureBatch", sDepartureBatch), new SqlPara("MiddleSiteStr", fsp.printSite) }));
            if (ds == null || ds.Tables.Count == 0) return;

            //zaj 2018-1-15 ˾������Э����ݹ�˾id������
            string transprotocol = CommonClass.UserInfo.Transprotocol == "" ? "˾������Э��" : CommonClass.UserInfo.Transprotocol;
            string departList = CommonClass.UserInfo.DepartList == "" ? "�����嵥" : CommonClass.UserInfo.DepartList;  //maohui20180315
            //frmPrintRuiLang fpr = new frmPrintRuiLang((fsp.printType == 0 ? "�����嵥" : fsp.printType == 1 ? "װ���嵥" : "˾������Э��"), ds);
            //frmPrintRuiLang fpr = new frmPrintRuiLang((fsp.printType == 0 ? "�����嵥" : fsp.printType == 1 ? "װ���嵥" : transprotocol), ds);
            frmPrintRuiLang fpr = new frmPrintRuiLang((fsp.printType == 0 ? departList : fsp.printType == 1 ? "װ���嵥" : transprotocol), ds, CommonClass.UserInfo.gsqc);

            fpr.ShowDialog();
        }

        private void EndSite_Leave(object sender, EventArgs e)
        {
            string endSite = EndSite.Text.Trim();
            if (endSite == "") return;
            string[] tmps = endSite.Split(',');
            if (tmps == null || tmps.Length == 0) return;

            try
            {
                edesite1.Text = tmps[0].Trim();
                edesite2.Text = tmps[1].Trim();
                edesite3.Text = tmps[2].Trim();
            }
            catch { }
            edesiteacc1.Text = edesite1.Text == "" ? "" : edesiteacc1.Text;
            edesiteacc2.Text = edesite2.Text == "" ? "" : edesiteacc2.Text;
            edesiteacc3.Text = edesite3.Text == "" ? "" : edesiteacc3.Text;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_DEPARTURELIST_BY_DEPARTUREBATCH_PRINTPRE", new List<SqlPara> { new SqlPara("DepartureBatch", sDepartureBatch), new SqlPara("PrintMan", CommonClass.UserInfo.UserName) }));
            frmPrintRuiLang fpr = new frmPrintRuiLang("�������ͬ", ds);
            fpr.ShowDialog();
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (e.Page != tp3) return;
            string fetchBillno = "";
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                if (ConvertType.ToString(myGridView2.GetRowCellValue(i, GridOper.GetGridViewColumn(myGridView2, "TransferMode"))) == "˾��ֱ��")
                {
                    fetchBillno += ConvertType.ToString(myGridView2.GetRowCellValue(i, GridOper.GetGridViewColumn(myGridView2, "BillNo"))) + ",";
                }
            }
            FecthBillNO.Text = fetchBillno.TrimEnd(',');
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //���
            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_SET_DEPARTURE_VERIFY_STATEPre", new List<SqlPara> { new SqlPara("DepartureBatch", sDepartureBatch), new SqlPara("Man", CommonClass.UserInfo.UserName), new SqlPara("type", 1) })) == 0) return;
            MsgBox.ShowOK();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            //�����
            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_SET_DEPARTURE_VERIFY_STATEPre", new List<SqlPara> { new SqlPara("DepartureBatch", sDepartureBatch), new SqlPara("type", 2) })) == 0) return;
            MsgBox.ShowOK();
        }

        private void simpleButton15_Click(object sender, EventArgs e)
        {
            getdata(2);
        }

        private void simpleButton16_Click(object sender, EventArgs e)
        {
            getdata(3);
        }
    }
}