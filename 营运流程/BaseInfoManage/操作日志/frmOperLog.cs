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
    public partial class frmOperLog : BaseForm
    {
        public frmOperLog()
        {
            InitializeComponent();
        }

        private void frmShortConnOutbound_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //����о���Ĺ���������������ʵ��
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            getData();

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
                list.Add(new SqlPara("billno", billno.Text.Trim()));
                list.Add(new SqlPara("Batch", Batch.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_OPERLOG", list);
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

        //private void tvbsite_AfterSelect(object sender, TreeViewEventArgs e)
        //{

        //    endSite.Text = e.Node.Text;
        //}

        //private void tvesite_AfterSelect(object sender, TreeViewEventArgs e)
        //{

        //    endSite.Text = e.Node.Text;
        //}

        //private void tvbsite_MouseClick(object sender, MouseEventArgs e)
        //{
        //    endSite.ClosePopup();
        //}

        //private void tvesite_MouseClick(object sender, MouseEventArgs e)
        //{
        //    endSite.ClosePopup();
        //}

        private void myGridView1_DoubleClick(object sender, EventArgs e)
        {


        }

       

        private void barCheckItem3_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            //cc.AllowAutoFilter(gridView1);
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
          
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "������־");
        }

        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            //cc.QuickSearch();
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem15_ItemClick(object sender, ItemClickEventArgs e)
        {
            //int rowhandle = gridView1.FocusedRowHandle;
            //if (rowhandle < 0) return;

            //w_ɨ��ͳ�� ws = new w_ɨ��ͳ��();
            //ws.dtvehicleno = gridView1.GetRowCellValue(rowhandle, "dtvehicleno").ToString();
            //ws.dtchauffer = gridView1.GetRowCellValue(rowhandle, "dtchauffer").ToString();
            //ws.dtsenddate = Convert.ToDateTime(gridView1.GetRowCellValue(rowhandle, "dtsenddate"));
            //ws.dtinoneflag = gridView1.GetRowCellValue(rowhandle, "dtinoneflag") == DBNull.Value ? "" : gridView1.GetRowCellValue(rowhandle, "dtinoneflag").ToString();
            //ws.type = 1;
            //ws.ShowDialog();
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

        private void btnAduit_ItemClick(object sender, ItemClickEventArgs e)
        {
           
        }

        private void CauseName_EditValueChanged(object sender, EventArgs e)
        {
           
        }
        private bool checkPay()
        {
            if (myGridView1.RowCount > 0)
            {
                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    if (ConvertType.ToFloat(myGridView1.GetRowCellValue(i, "FetchPayVerifBalance")) == 0 && (ConvertType.ToFloat(myGridView1.GetRowCellValue(i, "Amount")) > ConvertType.ToFloat(myGridView1.GetRowCellValue(i, "FetchPay"))))
                    {
                        myGridView1.SelectRow(i);
                        return false;
                    }
                    if (ConvertType.ToFloat(myGridView1.GetRowCellValue(i, "FetchPayVerifBalance")) != 0 && (ConvertType.ToFloat(myGridView1.GetRowCellValue(i, "Amount")) > ConvertType.ToFloat(myGridView1.GetRowCellValue(i, "FetchPayVerifBalance"))))
                    {
                        myGridView1.SelectRow(i);
                        return false;
                    }
                }
                return true;
            }
            else
                return false;
        }

        private void WebName_EditValueChanged(object sender, EventArgs e)
        {
           // CommonClass.SetCauseWeb(WebName, billno.Text.Trim(), Batch.Text.Trim()); 
        }

        private void AreaName_EditValueChanged(object sender, EventArgs e)
        {
            //CommonClass.SetCauseWeb(WebName,billno.Text.Trim(),Batch.Text.Trim());
        }
    }
}