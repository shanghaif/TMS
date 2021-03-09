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
    public partial class frmTerminalOFList : BaseForm
    {
        DataSet ds = new DataSet();
        //commonclass cc = new commonclass();
        //private userright ur = new userright();
        string sOtherState = "�ն�";

        public frmTerminalOFList()
        {
            InitializeComponent();
        }

        private void frmShortConnOutbound_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("�ն������ѵǼ�");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //����о���Ĺ���������������ʵ��
            CommonClass.SetSite(endSite, true);
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            endSite.EditValue = CommonClass.UserInfo.SiteName;
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem4);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (bdate.DateTime.Date > edate.DateTime.Date)
            {
                XtraMessageBox.Show("��ʼ���ڲ��ܴ��ڽ�������", "����ѡ�����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate", edate.DateTime));
                list.Add(new SqlPara("OtherState", sOtherState));
                list.Add(new SqlPara("SiteName", endSite.Text.Trim() == "ȫ��" ? "%%" : endSite.Text.Trim()));
                list.Add(new SqlPara("WebName", WebName.Text.Trim() == "ȫ��" ? "%%" : WebName.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLOTHERFEE", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tvbsite_AfterSelect(object sender, TreeViewEventArgs e)
        {

            endSite.Text = e.Node.Text;
        }

        private void tvesite_AfterSelect(object sender, TreeViewEventArgs e)
        {

            endSite.Text = e.Node.Text;
        }

        private void tvbsite_MouseClick(object sender, MouseEventArgs e)
        {
            endSite.ClosePopup();
        }

        private void tvesite_MouseClick(object sender, MouseEventArgs e)
        {
            endSite.ClosePopup();
        }

        private void myGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;
            frmRecShortConn ws = new frmRecShortConn();
            ws.U_SCBatchNo = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCBatchNo").ToString();
            ws.U_SCDate = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCDate").ToString();
            ws.U_SCSite = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCSite").ToString();
            ws.U_SCWeb = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCWeb").ToString();
            ws.U_SCCarNo = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCCarNo").ToString();
            ws.U_SCDriver = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCDriver").ToString();
            ws.U_SCDesSite = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCDesSite").ToString();
            ws.U_SCDesWeb = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCDesWeb").ToString();
            ws.U_SCDContolMan = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCMan").ToString();
            ws.SCId = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCId").ToString();
            ws.isMod = false;
            ws.ShowDialog();

        }

        private void cancel()
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                if ((myGridView1.GetRowCellValue(rowhandle, "SCWeb").ToString()) != CommonClass.UserInfo.WebName)
                {
                    MsgBox.ShowOK("���ڶ̲�������д˲�����");
                }
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "SCId").ToString());
                string SCBatchNo = (myGridView1.GetRowCellValue(rowhandle, "SCBatchNo").ToString());

                if (MsgBox.ShowYesNo("�Ƿ�ɾ����\r\r�˲��������棬��ȷ�ϣ�") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SCId", id));
                list.Add(new SqlPara("SCBatchNo", SCBatchNo));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_SHORTCONN", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();

                    myGridView1.DeleteRow(rowhandle);
                    myGridView1.PostEditor();
                    myGridView1.UpdateCurrentRow();
                    myGridView1.UpdateSummary();
                    DataTable dt = myGridControl1.DataSource as DataTable;
                    dt.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barCheckItem3_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            //cc.SaveGridLayout(gridshow, "��;�Ӳ���¼", true);
        }

        private void barCheckItem4_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            //cc.ShowAutoFilterRow(gridView1);
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmAddShortconnect wpc = new frmAddShortconnect();
            wpc.ShowDialog();
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            cancel();
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "�ն���������ϸ");
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
        private void barButtonItem16_ItemClick(object sender, ItemClickEventArgs e)
        {
            //w_ɨ����ͳ�� wv = new w_ɨ����ͳ��();
            //wv.Text = "�̲�ж����ɨ����ͳ��";

            //foreach (Form form in this.MdiParent.MdiChildren)
            //{
            //    if (form.GetType() == typeof(w_ɨ����ͳ��) && form.Text == wv.Text)
            //    {
            //        form.Focus();
            //        return;
            //    }
            //}

            //wv.MdiParent = this.MdiParent;
            //wv.Dock = DockStyle.Fill;
            //wv.opertype = 1;
            //wv.Show();
        }

        private void endSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(WebName, endSite.EditValue.ToString());
        }

        private void btnAddOtherFee_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmTerminalOFAdd frm = new frmTerminalOFAdd();
            frm.ShowDialog();
        }

        private void barButtonItem11_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            if (ConvertType.ToInt32(myGridView1.GetRowCellValue(rowhandle, "AuditState")) == 1)
            {
                MsgBox.ShowOK("�����Ѻ���!�����޸�!");
                return;
            }
            frmTerminalOFAdd frm = new frmTerminalOFAdd();
            frm.sBillno = myGridView1.GetRowCellValue(rowhandle, "BillNo").ToString();
            frm.sOID = myGridView1.GetRowCellValue(rowhandle, "OID").ToString();
            frm.ShowDialog();
        }

        private void barButtonItem15_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            try
            {
                if (ConvertType.ToInt32(myGridView1.GetRowCellValue(rowhandle, "AuditState")) == 1)
                {
                    MsgBox.ShowOK("�����Ѻ���!�����޸�!");
                    return;
                }

                if (MsgBox.ShowYesNo("ȷ��ɾ���������ã�") != DialogResult.Yes) return;

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("OID", myGridView1.GetRowCellValue(rowhandle, "OID").ToString()));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_DELETE_BILLOTHERFEE", list)) == 0) return;
                myGridView1.DeleteSelectedRows();
                MsgBox.ShowOK("ɾ���ɹ�!");
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

    }
}