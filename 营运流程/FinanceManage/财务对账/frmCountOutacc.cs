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
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using System.Reflection;

namespace ZQTMS.UI
{
    public partial class frmCountOutacc : BaseForm
    {
        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        DateTime t1, t2;
        string accstate, bsite,site;
        public frmCountOutacc()
        {
            InitializeComponent();
        }
        
        private void gridshow_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show();
                contextMenuStrip1.Top = e.Y + contextMenuStrip1.Height;
                contextMenuStrip1.Left = e.X;
            }
        }
        
        private void fillcygs()
        {
            DataSet dscygs = new DataSet();
            try
            {
                accstate = edaccstate.Text.Trim();
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate", edate.DateTime));
                list.Add(new SqlPara("accstate", accstate));
                list.Add(new SqlPara("site", bsite = comboBoxEdit1.Text.Trim() == "ȫ��" ? "%%" : comboBoxEdit1.Text.Trim()));
                list.Add (new SqlPara("WebName",    comboBoxEdit2.Text.Trim()=="ȫ��" ? "%%" :comboBoxEdit2.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_LOCAL_CYGS", list);
                 ds1 = SqlHelper.GetDataSet(sps);
                //if (ds1 == null || ds1.Tables.Count == 0 || ds1.Tables[0].Rows.Count == 0) return;
                gridControl2.DataSource = ds1.Tables[0];
                if (gridView3.RowCount == 1)
                {
                    cygs.Text = gridView3.GetRowCellValue(0, "outcygs").ToString();
                    site = gridView3.GetRowCellValue(gridView3.FocusedRowHandle, "site").ToString();
                    getdata(cygs.Text,site);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "ϵͳ��ʾ");
            }
        }
        
        private void getdata(string cygs,string site)
        {
            try
            {
                if (bdate.DateTime.Date > edate.DateTime.Date)
                {
                    XtraMessageBox.Show("��ʼ���ڲ��ܴ��ڽ�������", "����ѡ�����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("accstate", accstate));
                list.Add(new SqlPara("site",site));
                list.Add(new SqlPara("cygs", cygs));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_CYGS_ACCOUNT", list);
                ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                gridshow.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "ϵͳ��ʾ");
            }
        }
        
        private void simpleButton7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void w_package_out_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("�������");//xj/2019/5/28
            bdate.EditValue = CommonClass.gbdate;
            edate.EditValue = CommonClass.gedate;
            CommonClass.SetSite(comboBoxEdit1,true);
            CommonClass.SetWeb(comboBoxEdit2, true);
            BarMagagerOper.SetBarPropertity(bar3);
            GridOper.RestoreGridLayout(gridView1, "�������-����");
            GridOper.RestoreGridLayout(gridView3, "�������-��ϸ");
        }

        private void gridshow_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //cc.ShowBillDetail(gridView1);
        }

        private void gridView3_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridView3.FocusedRowHandle >= 0)
                {
                    cygs.Text = gridView3.GetRowCellValue(gridView3.FocusedRowHandle, "outcygs").ToString();                    
                    gridView1.GroupPanelText = "���˹�˾��" + cygs.Text + "   �����أ�" + gridView3.GetRowCellValue(gridView3.FocusedRowHandle, "site").ToString();
                    site=gridView3.GetRowCellValue(gridView3.FocusedRowHandle, "site").ToString();
                    getdata(cygs.Text,site);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "ϵͳ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            fillcygs();
        }

        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            //cc.AllowAutoFilter(gridView1);
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(gridView1, "�������-����", gridView3, "�������-��ϸ");
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(gridView1, "�������-����");
            GridOper.DeleteGridLayout(gridView3, "�������-��ϸ");
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(gridView1, gridView3);
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(gridView1, "������ˣ���ϸ��");
            //GridOper.ExportToExcel(gridView1);  zhengjiafeng20181007
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            //cc.QuickSearch();
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void tvbsite_AfterSelect(object sender, TreeViewEventArgs e)
        {
            
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (ds1 == null || ds1.Tables.Count == 0 || ds1.Tables[0].Rows.Count == 0) return;

            frmPrintRuiLang fpr = new frmPrintRuiLang("�������(����).grf", ds1);
            fpr.ShowDialog();
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

            frmPrintRuiLang fpr = new frmPrintRuiLang("�������(��ϸ).grf", ds);
            fpr.ShowDialog();
        }

        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(gridView3, "�������(����)");
            //GridOper.ExportToExcel(gridView3);
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            CommonClass.ShowBillSearch(GridOper.GetRowCellValueString(gridView1, "billno"));
            //Assembly ass = Assembly.LoadFrom(Application.StartupPath + "\\Plugin\\ZQTMS.UI.BuisinessManage.dll");
            //if (ass == null) return;
            //Type type = ass.GetType("ZQTMS.UI.frmBillSearch");
            //if (type == null) return;
            //Form frm = (Form)Activator.CreateInstance(type, GridOper.GetRowCellValueString(gridView1, "billno"));
            //frm.Show();
        }

    }
}