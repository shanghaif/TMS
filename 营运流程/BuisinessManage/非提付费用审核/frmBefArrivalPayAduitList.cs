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
    public partial class frmBefArrivalPayAduitList : BaseForm
    {
        public frmBefArrivalPayAduitList()
        {
            InitializeComponent();
        }

        private void frmShortConnOutbound_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //����о���Ĺ���������������ʵ��
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem4);
            DateTime bdt = CommonClass.gbdate;
            bdt = bdt.AddDays(-1);
            bdt =  bdt.AddHours(8);
            bdate.DateTime = bdt;
            DateTime edt = CommonClass.gedate;
            edt = edt.AddHours(7-edt.Hour);
            edate.DateTime = edt;
            CommonClass.SetCause(CauseName, true);
            CauseName.EditValue = CommonClass.UserInfo.CauseName;
            AreaName.EditValue = CommonClass.UserInfo.AreaName;
            WebName.EditValue = CommonClass.UserInfo.WebName;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            getData();

        }
        public void getData()
        {
            if (bdate.DateTime.Date > edate.DateTime.Date)
            {
                MsgBox.ShowError("��ʼ���ڲ��ܴ��ڽ�������");
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate", edate.DateTime));
                list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "ȫ��" ? "%%" : CauseName.Text.Trim()));
                list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "ȫ��" ? "%%" : AreaName.Text.Trim()));
                list.Add(new SqlPara("WebName", WebName.Text.Trim() == "ȫ��" ? "%%" : WebName.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BEFARRIVALPAYFORADUIT", list);
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

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barCheckItem3_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barCheckItem4_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            try
            {
                string sShowOK = "����ǰ���쳣��" + ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "BefArrivalPayVerifBalance")) + "\r\n�Ƿ�ת�벿�ţ�" + ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "BegWeb")) + "\r\n�Ƿ������";

                if (MsgBox.ShowYesNo(sShowOK) != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", myGridView1.GetRowCellValue(rowhandle, "BillNo")));
                list.Add(new SqlPara("EXFee", myGridView1.GetRowCellValue(rowhandle, "BefArrivalPayVerifBalance")));//�쳣���
                list.Add(new SqlPara("ExtFeeType", "����ǰ��"));
                list.Add(new SqlPara("ExtInMen", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("ExtSite", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("ExtWeb", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("ExtCause", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("ExtArea", CommonClass.UserInfo.AreaName));
                list.Add(new SqlPara("ExtType", "����ǰ��ת�쳣"));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_INTO_BEFARRIVALPAYEXCEPTION", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    getData();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btnAduit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (myGridView1.RowCount == 0) return;
            myGridView1.PostEditor();
            string sBillNo = "", sAmount = "";
            float Amount = 0, BefArrivalPayVerifBalance = 0;
            float sumAmount = 0;
            try
            {
                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    Amount = ConvertType.ToFloat(myGridView1.GetRowCellValue(i, "Amount"));
                    BefArrivalPayVerifBalance = ConvertType.ToFloat(myGridView1.GetRowCellValue(i, "BefArrivalPayVerifBalance"));
                    if (Amount <= 0 || Amount > BefArrivalPayVerifBalance) continue;

                    sBillNo += myGridView1.GetRowCellValue(i, "BillNo") + "@";//�˵���
                    sAmount += Amount + "@";//��˵Ľ��
                    sumAmount = sumAmount + Amount;
                }
                string sShowOK = "����ǰ�������Ʊ����" + ConvertType.ToString(myGridView1.RowCount)
                 + "\r\n����ǰ������ܽ�" + ConvertType.ToString(sumAmount) + "\r\n����ǰ������ˣ�" + CommonClass.UserInfo.UserName + "\r\n�Ƿ������";

                if (MsgBox.ShowYesNo(sShowOK) != DialogResult.Yes)
                {
                    return;
                }

                if (sBillNo == "") return;

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNoStr", sBillNo));
                list.Add(new SqlPara("AmountStr", sAmount));
                list.Add(new SqlPara("AduitBillState", "����ǰ��"));
                list.Add(new SqlPara("AduitMan", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("AduitSite", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("AduitWeb", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("AduitCause", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("AduitArea", CommonClass.UserInfo.AreaName));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_CHECK_BEFARRIVALPAYADUIT", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    getData();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
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

        private void myGridView1_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e == null || myGridView1.FocusedRowHandle < 0) return;
            try
            {
                float BefArrivalPayVerifBalance = ConvertType.ToFloat(myGridView1.GetFocusedRowCellValue("BefArrivalPayVerifBalance"));
                float Amount = ConvertType.ToFloat(e.Value);
                if (Amount <= 0)
                {
                    e.Valid = false;
                    e.ErrorText = "��˽��������0!";
                    return;
                }
                if (Amount > BefArrivalPayVerifBalance)
                {
                    e.Valid = false;
                    e.ErrorText = "��˽��ܴ������!";
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
    }
}