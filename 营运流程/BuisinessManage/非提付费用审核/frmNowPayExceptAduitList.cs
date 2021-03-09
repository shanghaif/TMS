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
    public partial class frmNowPayExceptAduitList : BaseForm
    {
        public frmNowPayExceptAduitList()
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
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            CommonClass.SetCause(CauseName, true);
            CauseName.EditValue = CommonClass.UserInfo.CauseName;
            AreaName.EditValue = CommonClass.UserInfo.AreaName;
            WebName.EditValue = CommonClass.UserInfo.WebName;
        }
        public void getData()
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
                list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "ȫ��" ? "%%" : CauseName.Text.Trim()));
                list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "ȫ��" ? "%%" : AreaName.Text.Trim()));
                list.Add(new SqlPara("WebName", WebName.Text.Trim() == "ȫ��" ? "%%" : WebName.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_EXCEPTFORADUIT_NOWPAY", list);
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
            getData();
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

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            try
            {
                string sShowOK = "ת�쳣��" + ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "EXFee")) + "\r\n�Ƿ�ת�벿�ţ�" + ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "BegWeb")) + "\r\n�Ƿ������";

                if (MsgBox.ShowYesNo(sShowOK) != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("billNo", myGridView1.GetRowCellValue(rowhandle, "billno_c")));
                list.Add(new SqlPara("EXFee", myGridView1.GetRowCellValue(rowhandle, "EXFee")));
                list.Add(new SqlPara("ExtFeeType", myGridView1.GetRowCellValue(rowhandle, "ExtFeeType")));
                list.Add(new SqlPara("ExtInDate", CommonClass.gcdate));
                list.Add(new SqlPara("ExtInMen", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("ExtSite", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("ExtWeb", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("ExtCause", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("ExtArea", CommonClass.UserInfo.AreaName));
                if ((myGridView1.GetRowCellValue(rowhandle, "ExtFeeType").ToString() == "�ָ�"))
                    list.Add(new SqlPara("ExtType", "�ָ�ת�쳣"));
                if ((myGridView1.GetRowCellValue(rowhandle, "ExtFeeType").ToString() == "���Ḷ�춯"))
                    list.Add(new SqlPara("ExtType", "���Ḷ�춯ת�쳣"));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_INTO_EXCEPTEXCEPTION_NOWPAY", list);
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

        private void btnAddOtherFee_ItemClick(object sender, ItemClickEventArgs e)
        {
            nowPayAduit();
            nowTransactionAduit();
            getData();
        }
        private void nowPayAduit()
        {
            try
            {
                string sBillNo = "";
                string sAmount = "";
                string sNowPayVerifBalance = "";
                float sumAmount = 0;
                if (myGridView1.RowCount > 0)
                {
                    for (int i = 0; i < myGridView1.RowCount; i++)
                    {
                        if (myGridView1.GetRowCellValue(i, "ExtType").ToString() == "�ָ�ת����")
                        {
                            sumAmount = sumAmount + ConvertType.ToFloat(myGridView1.GetRowCellValue(i, "Amount"));
                            sBillNo = sBillNo + myGridView1.GetRowCellValue(i, "billno_c") + "@";
                            sAmount = sAmount + myGridView1.GetRowCellValue(i, "Amount") + "@";
                            sNowPayVerifBalance = sNowPayVerifBalance + '0' + "@";
                        }
                    }
                }

                if (sBillNo == "")
                {
                    return;
                }
                if (sBillNo == "") return;
                string sShowOK = "�ָ�ת�쳣�����Ʊ����" + ConvertType.ToString(myGridView1.RowCount)
                    + "\r\n�ָ�ת�쳣����ܽ�" + ConvertType.ToString(sumAmount) + "\r\n�ָ�ת�쳣����ˣ�" + CommonClass.UserInfo.UserName + "\r\n�Ƿ������";

                if (MsgBox.ShowYesNo(sShowOK) != DialogResult.Yes)
                {
                    return;
                }

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("billNo", sBillNo));
                list.Add(new SqlPara("AduitDate", CommonClass.gcdate));
                list.Add(new SqlPara("Amount", sAmount));
                list.Add(new SqlPara("AduitBillState", "�ָ�ת����"));
                list.Add(new SqlPara("AduitMan", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("AduitSite", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("AduitWeb", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("AduitCause", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("AduitArea", CommonClass.UserInfo.AreaName));
                list.Add(new SqlPara("NowPayVerifBalance", sNowPayVerifBalance));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_CHECK_EXCEPTADUIT_NOWPAY", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        private void nowTransactionAduit()
        {
            try
            {
                string sBillNo = "";
                string sAmount = "";
                string sNowPayVerifBalance = "";
                float sumAmount = 0;
                if (myGridView1.RowCount > 0)
                {
                    for (int i = 0; i < myGridView1.RowCount; i++)
                    {
                        if (myGridView1.GetRowCellValue(i, "ExtType").ToString() == "���Ḷ�춯ת����")
                        {
                            sumAmount = sumAmount + ConvertType.ToFloat(myGridView1.GetRowCellValue(i, "Amount"));
                            sBillNo = sBillNo + myGridView1.GetRowCellValue(i, "billno_c") + "@";
                            sAmount = sAmount + myGridView1.GetRowCellValue(i, "Amount") + "@";
                            sNowPayVerifBalance = sNowPayVerifBalance + '0' + "@";
                        }
                    }
                }

                if (sBillNo == "")
                {
                    return;
                }
                if (sBillNo == "") return;
                string sShowOK = "�ָ�ת�쳣�����Ʊ����" + ConvertType.ToString(myGridView1.RowCount)
                    + "\r\n�ָ�ת�쳣����ܽ�" + ConvertType.ToString(sumAmount) + "\r\n�ָ�ת�쳣����ˣ�" + CommonClass.UserInfo.UserName + "\r\n�Ƿ������";

                if (MsgBox.ShowYesNo(sShowOK) != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("billNo", sBillNo));
                list.Add(new SqlPara("AduitDate", CommonClass.gcdate));
                list.Add(new SqlPara("Amount", sAmount));
                list.Add(new SqlPara("AduitBillState", "���Ḷ�춯ת����"));
                list.Add(new SqlPara("AduitMan", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("AduitSite", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("AduitWeb", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("AduitCause", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("AduitArea", CommonClass.UserInfo.AreaName));
                list.Add(new SqlPara("NowPayVerifBalance", sNowPayVerifBalance));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_CHECK_EXCEPTADUIT_NOWPAY", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
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

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
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