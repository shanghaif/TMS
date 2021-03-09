using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using DevExpress.XtraEditors.Repository;

namespace ZQTMS.UI
{
    public partial class frmReceiptSendNEW : BaseForm
    {
        private DataSet ds_left = new DataSet();
        private DataSet ds_right = new DataSet();

        public frmReceiptSendNEW()
        {
            InitializeComponent();
            barEditItem1.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem1_EditValueChanging);
            barEditItem1.Edit.KeyDown += new KeyEventHandler(barEditItem1_KeyDown);
            barEditItem2.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem2_EditValueChanging);
            barEditItem2.Edit.KeyDown += new KeyEventHandler(barEditItem2_KeyDown);
        }

        private void frmReceiptSendNEW_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, false, myGridView2);
            GridOper.SetGridViewProperty(myGridView1, myGridView2);
            BarMagagerOper.SetBarPropertity(bar1, bar2);
            GridOper.RestoreGridLayout(myGridView1, myGridView2);
            FixColumn fix = new FixColumn(myGridView1, barSubItem4);
            fix = new FixColumn(myGridView2, barSubItem2);

            Operator.Text = CommonClass.UserInfo.UserName;
            OperateTime.EditValue = CommonClass.gcdate;

            //if (CommonClass.Arg.CourierFirm != "")
            //{
            //    foreach (string courierFirm in CommonClass.Arg.CourierFirm.Split(','))
            //    {
            //        if (courierFirm != "" && !edcourierFirm.Properties.Items.Contains(courierFirm))
            //            edcourierFirm.Properties.Items.Add(courierFirm);
            //    }
            //}

            string[] OperateStateList = CommonClass.Arg.OperateState.Split(',');
            if (OperateStateList.Length > 0)
            {
                for (int i = 0; i < OperateStateList.Length; i++)
                {
                    OperateState.Properties.Items.Add(OperateStateList[i]);
                }
                OperateState.SelectedIndex = 0;
            }
            CommonClass.SetSite(repositoryItemComboBox4, true);
            //if (CommonClass.UserInfo.SiteName.Equals("�ܲ�"))
            //{
            //    barEditItem3.EditValue = "ȫ��";
            //    barEditItem4.EditValue = "ȫ��";
            //}
            //else
            //{
            //    barEditItem3.EditValue = CommonClass.UserInfo.SiteName;
            //    barEditItem4.EditValue = CommonClass.UserInfo.WebName;
            //}

            CommonClass.Create_BarEditItem_Web(barManager1, bar1, barEditWeb);
            CommonClass.Create_BarEditItem_Area(barManager1, bar1, barEditArea);
            CommonClass.Create_BarEditItem_Cause(barManager1, bar1, barEditCause);
            barEditCause.EditValueChanged += new System.EventHandler(this.barEditItem1_EditValueChanged);
            barEditArea.EditValueChanged += new System.EventHandler(this.barEditItem1_EditValueChanged_2);
            barEditCause.EditValue = CommonClass.UserInfo.CauseName;
            barEditArea.EditValue = CommonClass.UserInfo.AreaName;
            barEditWeb.EditValue = CommonClass.UserInfo.WebName;
        }
        BarEditItem barEditCause = new BarEditItem(); //������ҵ��
        BarEditItem barEditArea = new BarEditItem(); //���ɴ���
        BarEditItem barEditWeb = new BarEditItem(); //��������

        private void barEditItem1_EditValueChanged(object sender, EventArgs e)
        {
            RepositoryItemComboBox repositoryItemComboBox = (RepositoryItemComboBox)barEditArea.Edit;
            repositoryItemComboBox.Items.Clear();
            CommonClass.SetArea(repositoryItemComboBox, barEditCause.EditValue + "", true);
            barEditArea.EditValue = "ȫ��";
        }
        private void barEditItem1_EditValueChanged_2(object sender, EventArgs e)
        {
            RepositoryItemComboBox repositoryItemComboBox = (RepositoryItemComboBox)barEditWeb.Edit;
            repositoryItemComboBox.Items.Clear();
            CommonClass.SetCauseWeb(repositoryItemComboBox, barEditCause.EditValue + "", barEditArea.EditValue + "");
            barEditWeb.EditValue = "ȫ��";
        }

        /// <summary>
        /// ��ȡ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                ds_left.Clear();
                ds_right.Clear();
                myGridView1.ClearColumnsFilter();
                myGridView2.ClearColumnsFilter();

                List<SqlPara> list = new List<SqlPara>();
                //list.Add(new SqlPara("siteName", barEditItem3.EditValue));
                //list.Add(new SqlPara("WebName", barEditItem4.EditValue.ToString() == "ȫ��" ? "%%" : barEditItem4.EditValue));
                list.Add(new SqlPara("Cause", barEditCause.EditValue));
                list.Add(new SqlPara("Area", barEditArea.EditValue));
                list.Add(new SqlPara("Web", barEditWeb.EditValue));


                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ReceiptSendList_NEW", list);
                ds_left = SqlHelper.GetDataSet(sps);
                if (ds_left == null || ds_left.Tables.Count == 0) return;
                ds_right = ds_left.Clone();
                myGridControl1.DataSource = ds_left.Tables[0];
                myGridControl2.DataSource = ds_right.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// ��ߵ�ѡ���ұ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridViewMove.Move(myGridView1, ds_left, ds_right);
        }

        /// <summary>
        /// ���ȫѡ���ұ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView1.SelectAll();
            GridViewMove.Move(myGridView1, ds_left, ds_right);
        }

        /// <summary>
        /// ��ߵ�ѡ�����ұ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridViewMove.Move(myGridView2, ds_right, ds_left);
        }

        /// <summary>
        /// ���ȫ�����ص��ұ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView2.SelectAll();
            GridViewMove.Move(myGridView2, ds_right, ds_left);
        }

        /// <summary>
        /// �ұ��ı���ı��¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barEditItem1_EditValueChanging(object sender, EventArgs e)
        {
            string szfilter = ((DevExpress.XtraEditors.TextEdit)sender).Text.ToString().Trim();
            if (!string.IsNullOrEmpty(szfilter))
            {
                if (szfilter != "")
                {
                    myGridView1.ClearColumnsFilter();
                    myGridView1.Columns["BillNo"].FilterInfo = new ColumnFilterInfo("[BillNo] LIKE " + "'%" + szfilter + "%'" + " OR [BillNo] LIKE" + "'%" + szfilter + "%'", "");
                }
                else
                {
                    myGridView1.ClearColumnsFilter();
                }
            }
            else
            {
                myGridView1.ClearColumnsFilter();
            }
        }

        /// <summary>
        /// �ұ��ı���س��¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barEditItem1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || ((DevExpress.XtraEditors.TextEdit)sender).Text == "" || myGridView1.RowCount > 1)
            {
                e.Handled = true;
                return;
            }
            else
            {
                myGridView1.SelectRow(0);
                GridViewMove.Move(myGridView1, ds_left, ds_right);
                ((DevExpress.XtraEditors.TextEdit)sender).Text = "";

                myGridView1.ClearColumnsFilter();
                e.Handled = true;
            }
        }

        /// <summary>
        /// ����ı���ı��¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barEditItem2_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewValue.ToString().Trim()))
            {
                string szfilter = e.NewValue.ToString().Trim();
                if (szfilter != "")
                {
                    myGridView2.ClearColumnsFilter();
                    myGridView2.Columns["BillNo"].FilterInfo = new ColumnFilterInfo("[BillNo] LIKE " + "'%" + szfilter + "%'" + " OR [BillNo] LIKE" + "'%" + szfilter + "%'", "");
                }
                else
                {
                    myGridView2.ClearColumnsFilter();
                }
            }
            else
            {
                myGridView2.ClearColumnsFilter();
            }
        }

        /// <summary>
        /// ����ı���س��¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barEditItem2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || ((DevExpress.XtraEditors.TextEdit)sender).Text == "" || myGridView2.RowCount > 1)
            {
                e.Handled = true;
                return;
            }
            else
            {
                myGridView2.SelectRow(0);
                GridViewMove.Move(myGridView2, ds_right, ds_left);
                ((DevExpress.XtraEditors.TextEdit)sender).Text = "";

                myGridView2.ClearColumnsFilter();
                e.Handled = true;
            }
        }

        /// <summary>
        /// ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            panelControl5.Visible = true;
            //��ȡ�̲����κ�
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "USP_get_ShortConn_DBPCH", list);
                DataSet ds = SqlHelper.GetDataSet(spe);
                string SCBatchNo = "";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    SCBatchNo = ds.Tables[0].Rows[i]["SCBatchNo"].ToString();
                    HDPCH.Properties.Items.Add(SCBatchNo);

                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

            //��д�����Ϣ
            //panelControl4.Visible = true;
            //edcourierFirm.Focus();
        }

        /// <summary>
        /// ȡ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// �����˫��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myGridView1_DoubleClick(object sender, EventArgs e)
        {
            GridViewMove.Move(myGridView1, ds_left, ds_right);
        }

        /// <summary>
        /// �ұ�˫��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myGridView2_DoubleClick(object sender, EventArgs e)
        {
            GridViewMove.Move(myGridView2, ds_right, ds_left);
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void barEditItem3_EditValueChanged(object sender, EventArgs e)
        {
            repositoryItemComboBox5.Items.Clear();
            CommonClass.SetWeb(repositoryItemComboBox5, barEditItem3.EditValue + "");
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView2, "�ص��ĳ���ѡ���");
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "�ص��ĳ�ȡ���");
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView2, myGridView2.Guid.ToString());
        }

        private void barButtonItem16_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView2, myGridView2.Guid.ToString());
        }

        private void barButtonItem17_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem15_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView2);
        }

        private void barButtonItem18_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem3_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView2);
        }

        /// <summary>
        /// ��ӡ�嵥
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem19_ItemClick(object sender, ItemClickEventArgs e)
        {
            string rowBillNo = "", type = "";
            if (myGridView2.RowCount > 0)
            {
                DataTable dt = myGridControl2.DataSource as DataTable;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    rowBillNo += dt.Rows[i]["BillNo"] + ",";
                }
                type = "�ص��ĳ�";
            }
            else
            {
                XtraMessageBox.Show("û�пɴ�ӡ���˵���Ϣ����ӵڢٲ���ѡ��", "ϵͳ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_ReceiptBackBill_KT", new List<SqlPara>() { new SqlPara("rowBillNos", rowBillNo), new SqlPara("type", type) }));
            if (ds == null || ds.Tables.Count == 0) return;
            frmPrintRuiLang fpr = new frmPrintRuiLang("�ص����ӵ�.grf", ds);
            fpr.ShowDialog();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            /*
            string courierFirm = edcourierFirm.Text.Trim();
            string trackingNo = edtrackingNo.Text.Trim();
            if (courierFirm == "" && trackingNo != "")
            {
                MsgBox.ShowOK("�����˿�ݵ���,��û��ѡ���ݹ�˾!");
                edcourierFirm.Focus();
                return;
            }
            else if (courierFirm != "" && trackingNo == "")
            {
                MsgBox.ShowOK("��ѡ���˿�ݹ�˾,��û�����ݵ���!");
                edtrackingNo.Focus();
                return;
            }

            if (myGridView2.RowCount == 0)
            {
                XtraMessageBox.Show("û�з����κ���Ҫ�ĳ����嵥�������ڵڢٲ��й����嵥��", "ϵͳ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("OperateState", OperateState.Text.Trim()));
                list.Add(new SqlPara("Operator", Operator.Text.Trim()));
                list.Add(new SqlPara("OperateTime", OperateTime.Text.Trim()));
                string allBillNo = "";
                if (myGridView2.RowCount > 0)
                {
                    for (int i = 0; i < myGridView2.RowCount; i++)
                    {
                        allBillNo += myGridView2.GetRowCellValue(i, "BillNo") + ",";
                    }
                }
                list.Add(new SqlPara("allBillNo", allBillNo.Trim()));
                list.Add(new SqlPara("OperateSite", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("OperateWeb", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("RecBatch", Guid.NewGuid().ToString()));
                list.Add(new SqlPara("OperateRemark", ""));
                list.Add(new SqlPara("ToSite", ""));
                list.Add(new SqlPara("ToWeb", ""));
                list.Add(new SqlPara("SendNum", ""));
                list.Add(new SqlPara("ReceiptState", "�ص��ĳ�"));
                list.Add(new SqlPara("LinkTel", ""));
                list.Add(new SqlPara("courierFirm", courierFirm));
                list.Add(new SqlPara("trackingNo", trackingNo));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_Receipt", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    XtraMessageBox.Show("�ص��Ѽĳ�", "ϵͳ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    barButtonItem1_ItemClick(null, null);
                }
            }
            */
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            panelControl5.Visible = false;
        }

        //����2018.1.3wbw
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            if (myGridView2.RowCount == 0)
            {
                XtraMessageBox.Show("û�з����κ���Ҫ�ĳ����嵥�������ڵڢٲ��й����嵥��", "ϵͳ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if (comboBoxEdit1.Text == "��ݼĳ�")
                {
                    if (textEdit1.Text == "" || textEdit2.Text == "")
                    {
                        MsgBox.ShowOK("ѡ���ݼĳ�ʱ����ݹ�˾�Ϳ�ݵ��Ų���Ϊ�գ�");
                        return;
                    }
                }
                if (comboBoxEdit1.Text == "�ڲ�����")
                {
                    if ( HDBillNo.Text == "")
                    {
                        MsgBox.ShowOK("ѡ���ڲ�����ʱ�����ƺź�˾�����ֲ���Ϊ�գ�");
                        return;
                    }
                }
                if (comboBoxEdit1.Text == "���κŴ���")
                {
                    if (HDPCH.Text == "")
                    {
                        MsgBox.ShowOK("ѡ�����κŴ���ʱ�����κŲ���Ϊ�գ�");
                        return;
                    }
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("OperateState", OperateState.Text.Trim()));
                list.Add(new SqlPara("Operator", Operator.Text.Trim()));
                list.Add(new SqlPara("OperateTime", OperateTime.Text.Trim()));
                string allBillNo = "";
                string huidanqueren = "";
                string UpFileDate = "";
                string a = "";
                if (myGridView2.RowCount > 0)
                {
                    for (int i = 0; i < myGridView2.RowCount; i++)
                    {
                        allBillNo += myGridView2.GetRowCellValue(i, "BillNo") + ",";
                        huidanqueren = myGridView2.GetRowCellValue(i, "huidanqueren").ToString();
                        UpFileDate = myGridView2.GetRowCellValue(i, "UpFileDate").ToString();
                        a = myGridView2.GetRowCellValue(i, "BillNo").ToString();
                        if (UpFileDate == "")
                        {
                            MsgBox.ShowOK(a+"�����ϴ��ص�ͼƬ��");
                            return;
                        }
                        if (UpFileDate != "")
                        {
                            if (huidanqueren != "�����")
                            {
                               // MsgBox.ShowOK("����δȷ�ϵĵ������飡");
                                //return;
                            }
                        }
                    }
                }
                list.Add(new SqlPara("allBillNo", allBillNo.Trim()));
                list.Add(new SqlPara("OperateSite", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("OperateWeb", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("RecBatch", Guid.NewGuid().ToString()));
                list.Add(new SqlPara("OperateRemark", ""));
                list.Add(new SqlPara("ToSite", ""));
                list.Add(new SqlPara("ToWeb", ""));
                list.Add(new SqlPara("SendNum", ""));
                list.Add(new SqlPara("ReceiptState", "�ص��ĳ�"));
                list.Add(new SqlPara("LinkTel", ""));
                list.Add(new SqlPara("MailingType", comboBoxEdit1.Text.Trim()));
                list.Add(new SqlPara("express", textEdit1.Text.Trim()));
                list.Add(new SqlPara("CourierNumber", textEdit2.Text.Trim()));
                list.Add(new SqlPara("HDBillNo", HDBillNo.Text.Trim()));
                list.Add(new SqlPara("HDPCH", HDPCH.Text.Trim()));

                //List<SqlPara> listSyn = new List<SqlPara>(list.ToArray());
                //List<SqlPara> listSyn = new List<SqlPara>();//ͬ��list 
                //list.ForEach(i=>listSyn.Add(i));
               
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_Receipt_NEW", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    XtraMessageBox.Show("�ص��Ѽĳ�", "ϵͳ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    panelControl5.Visible = false;
                    //�ص��ĳ�ͬ��  zaj 2018-5-14
                    Dictionary<string, string> dicSyn = new Dictionary<string, string>();
                    list.ForEach(i => dicSyn.Add(i.ParaName, i.ParaValue.ToString()));

                    CommonSyn.ReceiptSendeSyn(dicSyn);
                    textEdit1.Text = "";
                    textEdit2.Text = "";
                    
                    HDBillNo.Text = "";
                    barButtonItem1_ItemClick(null, null);
               
                }
            }
        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxEdit1.Text == "��ݼĳ�")
            {
                textEdit1.Enabled = true;
                textEdit2.Enabled = true;
              
                HDBillNo.Enabled = false;
                HDBillNo.Text = "";
                HDPCH.Text = "";
                HDPCH.Enabled = false;
            }
            if (comboBoxEdit1.Text == "�ڲ�����")
            {
                
                HDBillNo.Enabled = true;
                textEdit1.Enabled = false;
                textEdit2.Enabled = false;
                textEdit1.Text = "";
                textEdit2.Text = "";
                HDPCH.Text = "";
                HDPCH.Enabled = false;
            }
            if (comboBoxEdit1.Text == "���κŴ���")
            {
                HDPCH.Text = "";
                HDPCH.Enabled = true;
                textEdit1.Text = "";
                textEdit1.Enabled = false;
                textEdit2.Text = "";
                textEdit2.Enabled = false;
                HDBillNo.Text = "";
                HDBillNo.Enabled = false;
            }

        }

        //�ص��㵽
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            frmReceiptDD frm = new frmReceiptDD();
            frm.ShowDialog();

        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            frmReceiptQZDD frm = new frmReceiptQZDD();
            frm.ShowDialog();
        }

        private void myGridView1_DoubleClick_1(object sender, EventArgs e)
        {
            GridViewMove.Move(myGridView1, ds_left, ds_right);
        }

        private void myGridView2_DoubleClick_1(object sender, EventArgs e)
        {
            GridViewMove.Move(myGridView2, ds_right, ds_left);
        }
    }
}