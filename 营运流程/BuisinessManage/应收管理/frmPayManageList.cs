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
    public partial class frmPayManageList : BaseForm
    {
        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        DataSet ds2 = new DataSet();

        public frmPayManageList()
        {
            InitializeComponent();
        }

        private void frmShortConnOutbound_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("Ӧ�չ���");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //����о���Ĺ���������������ʵ��
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem4);

            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            Cause.Text = CommonClass.UserInfo.CauseName;
            Area.Text = CommonClass.UserInfo.AreaName;
            Web.Text = CommonClass.UserInfo.WebName;
            CommonClass.SetSite(bsite, true);
            CommonClass.SetSite(esite, true);
            CommonClass.SetCause(Cause, true);
            bsite.EditValue = CommonClass.UserInfo.SiteName;
            esite.EditValue = "ȫ��";
        }

        public void getData()
        {
            ds.Clear();
            if (bdate.DateTime.Date > edate.DateTime.Date)
            {
                XtraMessageBox.Show("��ʼ���ڲ��ܴ��ڽ�������", "����ѡ�����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (PaymentMode.Text.Trim() == "")
            {
                XtraMessageBox.Show("��ѡ�񸶿ʽ","���ʽΪ��", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate", edate.DateTime));
                list.Add(new SqlPara("Cause", Cause.Text.Trim() == "ȫ��" ? "%%" : Cause.Text.Trim()));
                list.Add(new SqlPara("Area", Area.Text.Trim() == "ȫ��" ? "%%" : Area.Text.Trim()));
                list.Add(new SqlPara("Web", Web.Text.Trim() == "ȫ��" ? "%%" : Web.Text.Trim()));
                list.Add(new SqlPara("bsite", bsite.Text.Trim() == "ȫ��" ? "%%" : bsite.Text.Trim()));
                list.Add(new SqlPara("esite", esite.Text.Trim() == "ȫ��" ? "%%" : esite.Text.Trim()));
                list.Add(new SqlPara("PaymentMode",PaymentMode.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_Get_WayBill_PAY_MANAGE_NEW", list);
                ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            //finally
            //{
            //    if (myGridView1.RowCount < 1000) myGridView1.BestFitColumns();
            //}
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

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            //int rowhandle = myGridView1.FocusedRowHandle;
            //if (rowhandle < 0) return;
            //try
            //{
            //    List<SqlPara> list = new List<SqlPara>();
            //    list.Add(new SqlPara("ExtId", new Guid(myGridView1.GetRowCellValue(rowhandle, "ExtId").ToString())));
            //    list.Add(new SqlPara("ExtOutDate", CommonClass.gcdate));
            //    list.Add(new SqlPara("ExtOutMen", CommonClass.UserInfo.UserName));
            //    if (myGridView1.GetRowCellValue(rowhandle, "ExtFeeType").ToString() == "�Ḷ")
            //        list.Add(new SqlPara("ExtType", "�Ḷת����"));
            //    else if (myGridView1.GetRowCellValue(rowhandle, "ExtFeeType").ToString() == "�ָ�")
            //        list.Add(new SqlPara("ExtType", "�ָ�ת����"));
            //    else if (myGridView1.GetRowCellValue(rowhandle, "ExtFeeType").ToString() == "�Ḷ�춯")
            //        list.Add(new SqlPara("ExtType", "�Ḷ�춯ת����"));
            //    else
            //        list.Add(new SqlPara("ExtType", "���Ḷ�춯ת����"));
            //    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_SET_EXCEPTION_OUT", list);
            //    if (SqlHelper.ExecteNonQuery(sps) > 0)
            //    {
            //        MsgBox.ShowOK();
            //        getData();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MsgBox.ShowException(ex);
            //}
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void CauseName_EditValueChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(esite, bsite.EditValue.ToString());
        }

        private void AreaName_EditValueChanged(object sender, EventArgs e)
        {
            //CommonClass.SetCauseWeb(WebName, CauseName.Text.Trim(), AreaName.Text.Trim());
        }

        private void Cause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(Area, Cause.Text);
            CommonClass.SetCauseWeb(Web, Cause.Text, Area.Text);
        }

        private void Area_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(Web, Cause.Text, Area.Text);
        }

        //��ȡ�ͻ�����
        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            int rowhandle = myGridView1.RowCount;
            if (rowhandle <= 0)
            {
                MsgBox.ShowOK("������");
                return;
            }
            string billnos = "";
            string PaymentMode = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                PaymentMode=myGridView1.GetRowCellValue(i,"PaymentMode").ToString();
                if(PaymentMode=="�Ḷ"||PaymentMode=="���ʸ�-�Ḷ"){
                billnos += myGridView1.GetRowCellValue(i,"BillNo")+"@";
                }
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("billnos", billnos));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_Get_billSendGoods_SendWeb", list);
                ds1 = SqlHelper.GetDataSet(spe);
                if (ds1.Tables[0] == null)
                {
                    return;
                }
                string billno1 = "";
                string billno = "";
                string WebName = "";
                for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                {
                     billno = ds1.Tables[0].Rows[j]["BillNO"].ToString();
                     WebName = ds1.Tables[0].Rows[j]["WebName"].ToString();
                    for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                    {
                         billno1 = ds.Tables[0].Rows[k]["BillNo"].ToString();
                        //string EndWebName = ds.Tables[0].Rows[j]["EndWebName"].ToString();
                        if (billno == billno1)
                        {
                            ds.Tables[0].Rows[k]["EndWebName"] = WebName;
                        }
                    }
                }
                myGridControl1.DataSource = ds.Tables[0];

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }


        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            int rowhandle = myGridView1.RowCount;
            if (rowhandle <= 0)
            {
                MsgBox.ShowOK("������");
                return;
            }
            string billnos = "";
            string PaymentMode = "";
            string EndWebName = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                PaymentMode = myGridView1.GetRowCellValue(i, "PaymentMode").ToString();
                EndWebName = myGridView1.GetRowCellValue(i, "EndWebName").ToString();
                if (PaymentMode == "�Ḷ" || PaymentMode == "���ʸ�-�Ḷ")
                {
                    if (EndWebName == "")
                    {
                        billnos += myGridView1.GetRowCellValue(i, "BillNo") + "@";
                    }
                }

            }

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("billnos", billnos));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_billDepartureList_BillNo", list);
                 ds2 = SqlHelper.GetDataSet(spe);
                if (ds2.Tables[0] == null)
                {
                    return;
                }
                string billno = "";
                string billno1 = "";
                string WebName = "";
                for (int j = 0; j < ds2.Tables[0].Rows.Count; j++)
                {
                    billno = ds2.Tables[0].Rows[j]["BillNO"].ToString();
                    WebName = ds2.Tables[0].Rows[j]["WebName"].ToString();
                    for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                    {
                        billno1 = ds.Tables[0].Rows[k]["BillNO"].ToString();
                        if (billno == billno1)
                        {
                            ds.Tables[0].Rows[k]["EndWebName"] = WebName;
                        }
                    }
                }
                myGridControl1.DataSource = ds.Tables[0];



            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }


        }
    }
}