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

namespace ZQTMS.UI
{
    public partial class JMfmBadBill : BaseForm
    {
        public JMfmBadBill()
        {
            InitializeComponent();
        }

        private void getdata()
        {
            if (bdate.DateTime > edate.DateTime)
            {
                XtraMessageBox.Show("��ʼ���ڲ��ܴ��ڽ�������", "����ѡ�����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("webid", edwebid.Text.Trim() == "ȫ��" ? "%%" : edwebid.Text.Trim()));
                string hfstate = "";
                string shstate = "";

                if (comboBoxEdit1.Text.Trim() == "ȫ��")
                {
                    hfstate = "%%";
                }
                else if (comboBoxEdit1.Text.Trim() == "�ѻ���")
                {
                    hfstate = "1";
                }
                else if (comboBoxEdit1.Text.Trim() == "δ����")
                {
                    hfstate = "0";
                }
                else if (comboBoxEdit1.Text.Trim() == "���")
                {
                    hfstate = "2";
                }

                if (comboBoxEdit2.Text.Trim() == "ȫ��")
                {
                    shstate = "%%";
                }
                else if (comboBoxEdit2.Text.Trim() == "�����")
                {
                    shstate = "1";
                }
                else if (comboBoxEdit2.Text.Trim() == "δ���")
                {
                    shstate = "0";
                }
                else if (comboBoxEdit2.Text.Trim() == "���")
                {
                    shstate = "2";
                }
                list.Add(new SqlPara("gzstate", dateEdit1.Text.Trim()));
                list.Add(new SqlPara("hfstate", hfstate));
                list.Add(new SqlPara("shstate", shstate));
                list.Add(new SqlPara("webid1", comboBoxEdit3.Text.Trim() == "ȫ��" ? "%%" : comboBoxEdit3.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BAD_TYD_CL", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;

                gridshow.DataSource = ds;
                gridshow.DataMember = ds.Tables[0].ToString();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void JMfmBadBill_Load(object sender, EventArgs e)
        {
            bdate.EditValue = CommonClass.gbdate;
            edate.EditValue = CommonClass.gedate;
            BarMagagerOper.SetBarPropertity(bar3); //����о���Ĺ���������������ʵ��
            edwebid.Text = CommonClass.UserInfo.SiteName;

            CommonClass.SetWeb(edwebid, edwebid.Text);
            CommonClass.SetWeb(comboBoxEdit3, edwebid.Text);
            GridOper.RestoreGridLayout(gridView2, "��������¼");
            FixColumn fix = new FixColumn(gridView2, barSubItem2);
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rows = gridView2.FocusedRowHandle;
            if (rows < 0) return;

            JMfmBadBillDeal wb = new JMfmBadBillDeal();
            wb.gv = gridView2;
            wb.look = 10;
            wb.ShowDialog();
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rows = gridView2.FocusedRowHandle;
            if (rows < 0) return;

            int id = Convert.ToInt32(gridView2.GetRowCellValue(rows, "id"));

            string badchuliman = gridView2.GetRowCellValue(rows, "badchuliman").ToString();//������
            string badzerenchuliman = gridView2.GetRowCellValue(rows, "badzerenchuliman").ToString();//���λ�����
            string badshman = gridView2.GetRowCellValue(rows, "badshman").ToString();//�����

            int shstate = Convert.ToInt32(gridView2.GetRowCellValue(rows, "shstate").ToString() == "" ? "0" : gridView2.GetRowCellValue(rows, "shstate").ToString());
            int hfstate = Convert.ToInt32(gridView2.GetRowCellValue(rows, "hfstate").ToString() == "" ? "0" : gridView2.GetRowCellValue(rows, "hfstate").ToString());



            if (shstate > 0)
            {
                MsgBox.ShowOK("�������������޸ģ�");
                return;
            }

            JMfmBadBillDeal wb = new JMfmBadBillDeal();
            wb.gv = gridView2;
            wb.look = 4;
            wb.ShowDialog();
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rows = gridView2.FocusedRowHandle;
            if (rows < 0) return;

            int id = Convert.ToInt32(gridView2.GetRowCellValue(rows, "id"));
            string badchuliman = gridView2.GetRowCellValue(rows, "badchuliman").ToString();//������
            string badzerenchuliman = gridView2.GetRowCellValue(rows, "badzerenchuliman").ToString();//���λ�����
            string badshman = gridView2.GetRowCellValue(rows, "badshman").ToString();//�����
            string badcreateby = gridView2.GetRowCellValue(rows, "badcreateby").ToString();//�����

            int shstate = Convert.ToInt32(gridView2.GetRowCellValue(rows, "shstate").ToString() == "" ? "0" : gridView2.GetRowCellValue(rows, "shstate").ToString());
            int hfstate = Convert.ToInt32(gridView2.GetRowCellValue(rows, "hfstate").ToString() == "" ? "0" : gridView2.GetRowCellValue(rows, "hfstate").ToString());

            string cancelman = gridView2.GetRowCellValue(rows, "cancelman").ToString();//ȡ����
            string badchuliyijian = gridView2.GetRowCellValue(rows, "badchuliyijian").ToString();//ȡ����


            if (badcreateby != CommonClass.UserInfo.UserName)
            {
                MsgBox.ShowOK("ֻ�г������˵Ǽǵ��쳣��");
                return;
            }

            else if (badchuliyijian != "")
            {
                MsgBox.ShowOK("�Ѹ��٣����ܳ�����");
                return;
            }
            else if (shstate > 0)
            {
                MsgBox.ShowOK("����ˣ����ܳ�����");
                return;
            }
            else if (hfstate > 0)
            {
                MsgBox.ShowOK("�����λ��֣����ܳ�����");
                return;
            }
            else if (cancelman != "")
            {
                MsgBox.ShowOK("�ѱ�ȡ�����λ��֣������ܳ�����");
                return;
            }

            if (DialogResult.Yes != MsgBox.ShowYesNo("ȷ��������")) return;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("id", id));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_BAD_TYD_SA", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    gridView2.DeleteRow(rows);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rows = gridView2.FocusedRowHandle;
            if (rows < 0) return;

            int id = Convert.ToInt32(gridView2.GetRowCellValue(rows, "id"));

            string badchuliman = gridView2.GetRowCellValue(rows, "badchuliman").ToString();//������
            string badzerenchuliman = gridView2.GetRowCellValue(rows, "badzerenchuliman").ToString();//���λ�����
            string badshman = gridView2.GetRowCellValue(rows, "badshman").ToString();//�����
            int shstate = Convert.ToInt32(gridView2.GetRowCellValue(rows, "shstate").ToString() == "" ? "0" : gridView2.GetRowCellValue(rows, "shstate").ToString());
            int hfstate = Convert.ToInt32(gridView2.GetRowCellValue(rows, "hfstate").ToString() == "" ? "0" : gridView2.GetRowCellValue(rows, "hfstate").ToString());
            JMfmBadBillDeal wb = new JMfmBadBillDeal();
            wb.gv = gridView2;
            wb.look = 1;
            wb.ShowDialog();
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rows = gridView2.FocusedRowHandle;
            if (rows < 0) return;

            int id = Convert.ToInt32(gridView2.GetRowCellValue(rows, "id"));
            string badchuliman = gridView2.GetRowCellValue(rows, "badchuliman").ToString();//������
            string badzerenchuliman = gridView2.GetRowCellValue(rows, "badzerenchuliman").ToString();//���λ�����
            int shstate = Convert.ToInt32(gridView2.GetRowCellValue(rows, "shstate").ToString() == "" ? "0" : gridView2.GetRowCellValue(rows, "shstate").ToString());

            //if (badchuliman == "")
            //{
            //    commonclass.MsgBox.ShowOK("δ�������ܽ������λ��֣�");
            //    return;
            //}
            //else if (badzerenchuliman != "")
            //{
            //    commonclass.MsgBox.ShowOK("�ѻ������Σ�����Ҫ�ٴ���");
            //    return;
            //}
            //else
            if (shstate > 0)
            {
                MsgBox.ShowOK("������������Ҫ�ٴ���");
                return;
            }
            JMfmBadBillDeal wb = new JMfmBadBillDeal();
            wb.gv = gridView2;
            wb.look = 2;
            wb.ShowDialog();
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rows = gridView2.FocusedRowHandle;
            if (rows < 0) return;

            int id = Convert.ToInt32(gridView2.GetRowCellValue(rows, "id"));
            string badchuliman = gridView2.GetRowCellValue(rows, "badchuliman").ToString();//������
            string badzerenchuliman = gridView2.GetRowCellValue(rows, "badzerenchuliman").ToString();//���λ�����
            string badshman = gridView2.GetRowCellValue(rows, "badshman").ToString();//�����

            int shstate = Convert.ToInt32(gridView2.GetRowCellValue(rows, "shstate").ToString() == "" ? "0" : gridView2.GetRowCellValue(rows, "shstate").ToString());
            int hfstate = Convert.ToInt32(gridView2.GetRowCellValue(rows, "hfstate").ToString() == "" ? "0" : gridView2.GetRowCellValue(rows, "hfstate").ToString());

            if (hfstate == 0)
            {
                MsgBox.ShowOK("δ�������λ��֣��������");
                return;
            }

            if (hfstate == 2)
            {
                MsgBox.ShowOK("���λ����ѽ��з�������������");
                return;
            }
            JMfmBadBillDeal wb = new JMfmBadBillDeal();
            wb.gv = gridView2;
            wb.look = 3;
            wb.ShowDialog();
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(gridView2);
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(gridView2);
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(gridView2, "��������¼");
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout("��������¼");
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(gridView2);
        }

        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rows = gridView2.FocusedRowHandle;
            if (rows < 0) return;

            int shstate = Convert.ToInt32(gridView2.GetRowCellValue(rows, "shstate").ToString() == "" ? "0" : gridView2.GetRowCellValue(rows, "shstate").ToString());

            if (shstate > 0)
            {
                MsgBox.ShowOK("������������Ҫ�ٴ���");
                return;
            }
            JMfmBadBillDeal wb = new JMfmBadBillDeal();
            wb.gv = gridView2;
            wb.look = 5;
            wb.ShowDialog();
        }

        private void gridView2_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e == null || e.Column.FieldName != "rowid") return;
            e.Value = e.RowHandle + 1;
        }

        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            CommonClass.ShowBillSearch(GridOper.GetRowCellValueString(gridView2, "BillNo"));
        }
    }
}