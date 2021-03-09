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
    public partial class frmExceptionManagement : BaseForm
    {
        DataSet ds = new DataSet();
        public frmExceptionManagement()
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

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_EXCEPTION_WEBNAME", list);
                DataSet dsWeb = SqlHelper.GetDataSet(sps);
                if (dsWeb == null || dsWeb.Tables.Count == 0)
                 {
                    WebName.Properties.Items.Add("ȫ��");
                    WebName.Text = "ȫ��";
                     return;
                 }
                DataRow[] dr = dsWeb.Tables[0].Select("");
                WebName.Properties.Items.Clear();
                WebName.Text = "";
                for (int i = 0; i < dr.Length; i++)
                {
                    WebName.Properties.Items.Add(dr[i]["ExtWeb"]);
                }
                WebName.Properties.Items.Add("ȫ��");
                WebName.Text = "ȫ��";
            }
            catch (Exception)
            {
                MsgBox.ShowOK("���ڼ��ػ������ϣ����Եȣ�");
            }
            CauseName.EditValue = "ȫ��";
            AreaName.EditValue = "ȫ��";
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

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_EXCEPTION", list);
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
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ExtId", new Guid(myGridView1.GetRowCellValue(rowhandle, "ExtId").ToString())));
                list.Add(new SqlPara("ExtOutDate", CommonClass.gcdate));
                list.Add(new SqlPara("ExtOutMen", CommonClass.UserInfo.UserName));
                if (myGridView1.GetRowCellValue(rowhandle, "ExtFeeType").ToString() == "�Ḷ")
                    list.Add(new SqlPara("ExtType", "�Ḷת����"));
                else if (myGridView1.GetRowCellValue(rowhandle, "ExtFeeType").ToString() == "�ָ�")
                    list.Add(new SqlPara("ExtType", "�ָ�ת����"));
                else if (myGridView1.GetRowCellValue(rowhandle, "ExtFeeType").ToString() == "�Ḷ�춯")
                    list.Add(new SqlPara("ExtType", "�Ḷ�춯ת����"));
                else if(myGridView1.GetRowCellValue(rowhandle, "ExtFeeType").ToString() == "��ת�Ḷ")
                    list.Add(new SqlPara("ExtType", "��ת�Ḷת����"));
                else
                    list.Add(new SqlPara("ExtType", "���Ḷ�춯ת����"));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_SET_EXCEPTION_OUT", list);
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

        private void CauseName_EditValueChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(AreaName, CauseName.EditValue.ToString());
        }

        private void AreaName_EditValueChanged(object sender, EventArgs e)
        {
            //CommonClass.SetCauseWeb(WebName, CauseName.Text.Trim(), AreaName.Text.Trim());
        }
    }
}