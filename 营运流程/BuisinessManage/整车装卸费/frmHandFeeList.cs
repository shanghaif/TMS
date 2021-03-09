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
    public partial class frmHandFeeList : BaseForm
    {
        DataSet ds = new DataSet();
        //commonclass cc = new commonclass();
        //private userright ur = new userright();

        public frmHandFeeList()
        {
            InitializeComponent();
        }

        public string FeeType = "";
        public frmHandFeeList(string sFeeType)
        {
            InitializeComponent();
            FeeType = sFeeType;
            if (sFeeType == "ʼ��")
            {
                this.Text = "ʼ������װж����ϸ";
            }
            else
            {
                this.Text = "�ն�����װж����ϸ";
            }
        }


        private void frmShortConnOutbound_Load(object sender, EventArgs e)
        {
            if (FeeType == "ʼ��")
            {
                CommonClass.InsertLog("ʼ������װж��");//xj/2019/4/29
            }
            else
            {
                CommonClass.InsertLog("�ն�����װж��");//xj/2019/4/29
            }
           
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //����о���Ĺ���������������ʵ��
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem4);
            CommonClass.SetSite(endSite, true);
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            endSite.EditValue = CommonClass.UserInfo.SiteName;
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
                list.Add(new SqlPara("Type", FeeType));
                list.Add(new SqlPara("SiteName", endSite.Text.Trim() == "ȫ��" ? "%%" : endSite.Text.Trim()));
                list.Add(new SqlPara("WebName", WebName.Text.Trim() == "ȫ��" ? "%%" : WebName.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLHANDFEEBYCAR", list);
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
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, this.Text.Trim());
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
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
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
            frmHandFeeAdd frm = new frmHandFeeAdd();
            frm.sOtherState = FeeType;
            frm.ShowDialog();
        }

        private void barButtonItem11_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
           if ((myGridView1.GetRowCellValue(rowhandle, "aduitstate").ToString()=="�Ѻ���"))
            {
              MsgBox.ShowOK("�����Ѻ���!�����޸�!");
               return;
           }
            frmHandFeeAdd frm = new frmHandFeeAdd();
            frm.sOtherState = FeeType;
            frm.sDepartureBatch = myGridView1.GetRowCellValue(rowhandle, "DepartureBatch").ToString();
            frm.sid = myGridView1.GetRowCellValue(rowhandle, "id").ToString();
            frm.ShowDialog();
        }

        private void barButtonItem15_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            try
            {
                //if (ConvertType.ToInt32(myGridView1.GetRowCellValue(rowhandle, "AduitState")) == 1)
                //{
                //    MsgBox.ShowOK("�����Ѻ���!�����޸�!");
                //    return;
                //}

                if (MsgBox.ShowYesNo("ȷ��ɾ���������ã�") != DialogResult.Yes) return;

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("id", myGridView1.GetRowCellValue(rowhandle, "id").ToString()));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_DELETE_BILLHANDFEEBYCAR", list)) == 0) return;
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