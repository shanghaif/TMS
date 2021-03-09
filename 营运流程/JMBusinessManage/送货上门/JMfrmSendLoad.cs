using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using System.Collections;
using System.Data.OleDb;
using System.IO;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.Controls;
using System.Threading;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class JMfrmSendLoad : BaseForm
    {
        private DataSet dataset1 = new DataSet();
        private DataSet dataset3 = new DataSet();
        GridHitInfo hitInfo = null;
        static JMfrmSendLoad fsl;

        /// <summary>
        /// ��ȡ�������
        /// </summary>
        public static JMfrmSendLoad Get_JMfrmSendLoad { get { if (fsl == null || fsl.IsDisposed) fsl = new JMfrmSendLoad(); return fsl; } }

        public JMfrmSendLoad()
        {
            InitializeComponent();
            barEditItem1.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem1_EditValueChanging);
            barEditItem1.Edit.KeyDown += new KeyEventHandler(barEditItem1_KeyDown);
            barEditItem2.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem2_EditValueChanging);
            barEditItem2.Edit.KeyDown += new KeyEventHandler(barEditItem2_KeyDown);
        }

        private void getdata()
        {
            if (dataset1 != null)
            {
                dataset1.Tables.Clear();
                dataset3.Tables.Clear();
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SEND_LOAD_JM", list);
                dataset1 = SqlHelper.GetDataSet(sps);
                if (dataset1 == null || dataset1.Tables.Count == 0) return;

                dataset3 = dataset1.Clone();
                myGridControl1.DataSource = dataset1.Tables[0];
                myGridControl2.DataSource = dataset3.Tables[0];
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

        private void myGridControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GridViewMove.Move(myGridView1, dataset1, dataset3);
        }

        private void myGridControl2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GridViewMove.Move(myGridView2, dataset3, dataset1);
        }

        private void myGridControl2_DragDrop(object sender, DragEventArgs e)
        {
            GridViewMove.Move(myGridView1, dataset1, dataset3);
        }

        private void myGridControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (hitInfo == null) return;
            Rectangle dragRect = new Rectangle(new Point(
            hitInfo.HitPoint.X - SystemInformation.DragSize.Width / 2,
           hitInfo.HitPoint.Y - SystemInformation.DragSize.Height / 2), SystemInformation.DragSize);
            if (!dragRect.Contains(new Point(e.X, e.Y)))
            {
                if (hitInfo.InRowCell)
                    myGridControl1.DoDragDrop("��Ҫ��ȥ��....", DragDropEffects.All);
            }
        }

        private void myGridControl2_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void myGridControl1_MouseDown(object sender, MouseEventArgs e)
        {
            hitInfo = myGridView1.CalcHitInfo(new Point(e.X, e.Y));
        }

        private void myGridControl2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (hitInfo == null) return;
            Rectangle dragRect = new Rectangle(new Point(
            hitInfo.HitPoint.X - SystemInformation.DragSize.Width / 2,
            hitInfo.HitPoint.Y - SystemInformation.DragSize.Height / 2), SystemInformation.DragSize);
            if (!dragRect.Contains(new Point(e.X, e.Y)))
            {
                if (hitInfo.InRowCell)
                    myGridControl2.DoDragDrop("��Ҫ��ȥ��....", DragDropEffects.All);
            }
        }

        private void myGridControl2_MouseDown(object sender, MouseEventArgs e)
        {
            hitInfo = myGridView2.CalcHitInfo(new Point(e.X, e.Y));
        }

        private void myGridControl1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void myGridControl3_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void myGridControl1_DragDrop(object sender, DragEventArgs e)
        {
            GridViewMove.Move(myGridView2, dataset3, dataset1);
        }

        private void senddirect()
        {
            string sendcarno = SendCarNO.Text.Trim();
            string sendbatch = SendBatch.Text.Trim();
            if ((sendcarno == "") || (sendbatch == ""))
            {
                XtraMessageBox.Show("���š��������Ρ�������д��", "ϵͳ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                if (GridOper.GetRowCellValueString(myGridView2, i, "SendToProvince") == "" || GridOper.GetRowCellValueString(myGridView2, i, "SendToCity") == "" || GridOper.GetRowCellValueString(myGridView2, i, "SendToArea") == "" || GridOper.GetRowCellValueString(myGridView2, i, "SendToStreet") == "")
                {
                    MsgBox.ShowOK("����д��" + (i + 1) + "���ͻ���ַ!");
                    return;
                }
            }

            string billnos = "", sendpcss = "", accsends = "", IdStr = "", SendToProvinceStr = "", SendToCityStr = "", SendToAreaStr = "", SendToStreet = "";
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                billnos += GridOper.GetRowCellValueString(myGridView2, i, "BillNo") + ",";
                sendpcss += ConvertType.ToInt32(myGridView2.GetRowCellValue(i, "sendqty")) + ",";
                accsends += ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "AccSend")) + ",";
                IdStr += ConvertType.ToString(myGridView2.GetRowCellValue(i, "Id")) + ",";

                SendToProvinceStr += GridOper.GetRowCellValueString(myGridView2, i, "SendToProvince") + ",";
                SendToCityStr += GridOper.GetRowCellValueString(myGridView2, i, "SendToCity") + ",";
                SendToAreaStr += GridOper.GetRowCellValueString(myGridView2, i, "SendToArea") + ",";
                SendToStreet += GridOper.GetRowCellValueString(myGridView2, i, "SendToStreet") + ",";
            }
            if (billnos == "") return;
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("SendCarNo", sendcarno));
            list.Add(new SqlPara("SendBatch", sendbatch));
            list.Add(new SqlPara("SendDate", SendDate.DateTime));
            list.Add(new SqlPara("SendOperator", CommonClass.UserInfo.UserName));
            list.Add(new SqlPara("SendDriver", SendDriver.Text.Trim()));
            list.Add(new SqlPara("SendDesc", SendDesc.Text.Trim()));
            list.Add(new SqlPara("SendDriverPhone", SendDriverPhone.Text.Trim()));
            list.Add(new SqlPara("SendWeb", CommonClass.UserInfo.WebName));
            list.Add(new SqlPara("SendSite", CommonClass.UserInfo.SiteName));
            list.Add(new SqlPara("billnos", billnos));
            list.Add(new SqlPara("AccSends", accsends));
            list.Add(new SqlPara("SendPCSs", sendpcss));
            list.Add(new SqlPara("IdStr", IdStr));
            list.Add(new SqlPara("SendToProvinceStr", SendToProvinceStr));
            list.Add(new SqlPara("SendToCityStr", SendToCityStr));
            list.Add(new SqlPara("SendToAreaStr", SendToAreaStr));
            list.Add(new SqlPara("SendToStreetStr", SendToStreet));

            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_SEND_2", list)) == 0) return;
            dataset3.Tables[0].Rows.Clear();
            MsgBox.ShowOK();

            //��ӡ�ͻ���
            if (checkEdit3.Checked)
            {
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_SEND_DETAIL_PRINT", new List<SqlPara> { new SqlPara("SendBatch", sendbatch) }));
                if (ds == null || ds.Tables.Count == 0) return;
                frmRuiLangService.Print("�ͻ��嵥.grf", ds);
            }
            CommonSyn.TraceSyn(null, billnos.Replace(",", "@"), 12, "�ͻ�����", 1, null, null);
            Clear();
        }

        private void save()
        {
            myGridView2.ClearColumnsFilter();
            myGridView2.PostEditor();
            myGridView2.UpdateCurrentRow();
            myGridView2.UpdateSummary();
            if (myGridView2.RowCount == 0)
            {
                XtraMessageBox.Show("û��ѡ���ͻ��嵥,���ڵڢٲ�����ѡ�嵥!", "ϵͳ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            decimal AccSend = ConvertType.ToDecimal(GridOper.GetGridViewColumn(myGridView2, "AccSend").SummaryItem.SummaryValue);
            if (AccSend == 0)
            {
                if (XtraMessageBox.Show("û����д�ͻ���!\r\n��д�������ڵڢٲ���ѡ�񹤾����ϵķ�̯�ͻ��ѣ�Ҳ�����ֶ���д�ͻ��ѡ�\r\n\r\nȷ������Ҫ��д�ͻ��ѣ�ֱ�ӱ���?", "ϵͳ��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;
            }
            SendDate.DateTime = CommonClass.gcdate;

            //for (int i = 0; i < myGridView2.RowCount; i++)
            //{
            //    if (myGridView2.GetRowCellValue(i, "TransferMode").ToString() == "����")
            //    {
            //        MsgBox.ShowOK("�ͻ��嵥�������������飡");
            //        return;
            //    }
            //}

            int a = 0;
            if (checkEdit1.Checked)
            {
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    if (ConvertType.ToInt32(myGridView2.GetRowCellValue(i, "ischecked")) == 1)
                    {
                        a++;
                        break;
                    }
                }
                if (a == 0)
                {
                    XtraMessageBox.Show("����ʧ��!\r\n\r\n��ѡ���ˡ�����֪ͨ�����ˡ���ȴû��ѡ��Ҫ���Ͷ��ŵ��˵�!\r\n\r\n���ڵڢٲ��ͻ��嵥�й�ѡҪ���Ͷ��ŵ��˵�!", "ϵͳ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            if (checkEdit1.Checked)
            {
                sms.send_shipper(myGridView3, this, SendDate.DateTime, SendDriver.Text.Trim(), SendDriverPhone.Text.Trim());
            }
            if (checkEdit4.Checked)
            {
                if (checkEdit1.Checked) //�ȸ������˷�����ţ�ischecked�ᱻ��Ϊ2
                {
                    for (int i = 0; i < myGridView3.RowCount; i++)
                    {
                        myGridView3.SetRowCellValue(i, "ischecked", 1);
                    }
                }
                sms.send_consignee(myGridView3, this, SendDate.DateTime, SendDriver.Text.Trim(), SendDriverPhone.Text.Trim());
            }
            SendBatch.Text = GetMaxInOneVehicleFlag(); //������
            if (SendBatch.Text.Trim() == "") return;
            senddirect();

        }

        private void PrintSendBills(string inoneflag)
        {
            //w_ht_print whp = new w_ht_print();
            //whp.printtype = "��ӡ�ͻ��嵥";
            //whp.inoneflag = inoneflag;
            //whp.Show();
        }

        private void Clear()
        {
            SendBatch.Text = GetMaxInOneVehicleFlag();
            SendDate.DateTime = CommonClass.gcdate;
            SendCarNO.Text = SendDriver.Text = SendDriverPhone.Text = SendDesc.Text = "";
        }

        private string GetMaxInOneVehicleFlag()
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("bcode", CommonClass.UserInfo.LoginSiteCode));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SENDFLAG", list);
            DataSet ds = SqlHelper.GetDataSet(sps);
            if (ds == null || ds.Tables.Count == 0) return "";

            return ConvertType.ToString(ds.Tables[0].Rows[0][0]);
        }

        private void w_send_load_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this, false);
            CommonClass.GetGridViewColumns(myGridView1, false, myGridView2, myGridView3);
            GridOper.SetGridViewProperty(myGridView1, myGridView2, myGridView3);
            BarMagagerOper.SetBarPropertity(bar1, bar2); //����о���Ĺ���������������ʵ��
            GridOper.RestoreGridLayout(myGridView1, myGridView2);

            //��ȡ�ͻ�����
            GridColumn gc = GridOper.GetGridViewColumn(myGridView2, "AccSend");
            gc.ColumnEdit = this.repositoryItemPopupContainerEdit2;

            //edsite.Properties.Items.Add(CommonClass.UserInfo.SiteName);
            SendDate.DateTime = CommonClass.gcdate;
            SendBatch.Text = GetMaxInOneVehicleFlag();

            try
            {
                CommonClass.AreaManager.FillCityToImageComBoxEdit(edsheng, "0");
                myGridControl3.DataSource = CommonClass.dsCar.Tables[0];
            }
            catch { }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (CommonClass.CheckKongHuo(myGridView2, 1))
            {
                MsgBox.ShowOK("ѡ����嵥�����ػ����˵�,�����ͻ�!");
                return;
            }
            try
            {
                save();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "ϵͳ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ��ѯ�˵�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //cc.ShowBillDetail(myGridView2);
        }

        private void �鿴�˵�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //cc.ShowBillDetail(myGridView1);
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            getdata();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "�ͻ�����嵥");
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridViewMove.Move(myGridView1, dataset1, dataset3);
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView1.SelectAll();
            GridViewMove.Move(myGridView1, dataset1, dataset3);
        }

        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridViewMove.Move(myGridView2, dataset3, dataset1);
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView2.SelectAll();
            GridViewMove.Move(myGridView2, dataset3, dataset1);
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = tp3;
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barCheckItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            //cc.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem15_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView2);
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView2);
        }

        private void barEditItem1_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue == null) return;
            string szfilter = e.NewValue.ToString().Trim();
            if (szfilter != "")
            {
                myGridView1.ClearColumnsFilter();
                myGridView1.Columns["BillNo"].FilterInfo = new ColumnFilterInfo("[BillNo] LIKE " + "'%" + szfilter + "%'", "");
            }
            else
                myGridView1.ClearColumnsFilter();
        }

        private void barEditItem1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || ((DevExpress.XtraEditors.TextEdit)sender).Text == "" || myGridView1.RowCount > 1)
            {
                e.Handled = true;
                return;
            }
            myGridView1.SelectRow(0);
            GridViewMove.Move(myGridView1, dataset1, dataset3);
            ((DevExpress.XtraEditors.TextEdit)sender).Text = "";

            myGridView1.ClearColumnsFilter();
            e.Handled = true;
        }

        private void barEditItem2_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue == null) return;
            string szfilter = e.NewValue.ToString().Trim();
            if (szfilter != "")
            {
                myGridView2.ClearColumnsFilter();
                myGridView2.Columns["BillNo"].FilterInfo = new ColumnFilterInfo("[BillNo] LIKE" + "'%" + szfilter + "%'", "");
            }
            else
                myGridView2.ClearColumnsFilter();
        }

        private void barEditItem2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || ((DevExpress.XtraEditors.TextEdit)sender).Text == "" || myGridView2.RowCount > 1)
            {
                e.Handled = true;
                return;
            }
            myGridView2.SelectRow(0);
            GridViewMove.Move(myGridView2, dataset3, dataset1);
            ((DevExpress.XtraEditors.TextEdit)sender).Text = "";

            myGridView2.ClearColumnsFilter();
            e.Handled = true;
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            myGridView2.ClearColumnsFilter();
            if (myGridView2.RowCount == 0)
            {
                XtraMessageBox.Show("������ѡҪ�ͻ����˵�!", "ϵͳ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (textEdit3.Text.Trim() == "")
            {
                XtraMessageBox.Show("�����������ͻ���!", "ϵͳ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                decimal qtytotal = Convert.ToDecimal(myGridView2.Columns["Num"].SummaryItem.SummaryValue);//�ܼ���
                int type = radioGroup1.SelectedIndex;
                decimal acctotal = Convert.ToDecimal(textEdit3.Text.Trim());//����̯���ͻ���
                decimal sum = 0, accrow = 0;

                string filedname = "AccSend"; //��̯�ͻ���

                if (type == 0)
                {//��������̯
                    sum = 0;
                    for (int i = 0; i < myGridView2.RowCount; i++)
                    {
                        accrow = Math.Floor(acctotal * ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "Num")) / qtytotal);
                        sum += accrow;
                        if (i == myGridView2.RowCount - 1)
                        {
                            myGridView2.SetRowCellValue(i, filedname, accrow + (acctotal - sum));
                        }
                        else
                        {
                            myGridView2.SetRowCellValue(i, filedname, accrow);
                        }
                    }
                    barEditItem4.EditValue = "������";
                }
                else if (type == 1)
                {
                    decimal a = ConvertType.ToDecimal(myGridView2.Columns["Freight"].SummaryItem.SummaryValue);//�˷Ѻϼ�
                    sum = 0;

                    for (int i = 0; i < myGridView2.RowCount; i++)
                    {
                        accrow = Math.Floor(acctotal * ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "Freight")) / a);
                        sum += accrow;

                        if (i == myGridView2.RowCount - 1)
                        {
                            myGridView2.SetRowCellValue(i, filedname, accrow + (acctotal - sum));
                        }
                        else
                        {
                            myGridView2.SetRowCellValue(i, filedname, accrow);
                        }
                    }
                    barEditItem4.EditValue = "���˷�";
                }
                else if (type == 2)
                {
                    decimal avg = Math.Floor(acctotal / Convert.ToDecimal(myGridView2.RowCount));//��Ʊ
                    for (int i = 0; i < myGridView2.RowCount; i++)
                    {
                        if (i == myGridView2.RowCount - 1)
                        {
                            myGridView2.SetRowCellValue(i, filedname, acctotal - avg * (myGridView2.RowCount - 1));
                        }
                        else
                        {
                            myGridView2.SetRowCellValue(i, filedname, avg);
                        }
                    }
                    barEditItem4.EditValue = "��Ʊ";
                }
                else  //type=3  ����������
                {
                    decimal a = Convert.ToDecimal(myGridView2.Columns["Weight"].SummaryItem.SummaryValue);//�����ϼ�
                    sum = 0;

                    for (int i = 0; i < myGridView2.RowCount; i++)
                    {
                        accrow = Math.Floor(acctotal * ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "Weight")) / a);
                        sum += accrow;

                        if (i == myGridView2.RowCount - 1)
                        {
                            myGridView2.SetRowCellValue(i, filedname, accrow + (acctotal - sum));
                        }
                        else
                        {
                            myGridView2.SetRowCellValue(i, filedname, accrow);
                        }
                    }
                    barEditItem4.EditValue = "������";
                }

                myGridView2.UpdateSummary();
                XtraMessageBox.Show("��̯�ɹ�!", "ϵͳ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                myGridView2.Focus();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "ϵͳ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                myGridView2.SetRowCellValue(i, "accsendout", 0);
            }
            XtraMessageBox.Show("ȡ����̯�ɹ�!ʵ���ͻ���������!", "ϵͳ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    myGridView2.SetRowCellValue(i, "ischecked", 1);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "ϵͳ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void myGridView2_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            try
            {
                GridColumn gc = ((DevExpress.XtraGrid.Views.Grid.GridView)sender).FocusedColumn;
                if (e == null || gc == null || gc.FieldName != "sendqty") return;
                int oldvalue = ConvertType.ToInt32(myGridView2.GetFocusedRowCellValue("sendremainqty"));//������
                int newvalue = ConvertType.ToInt32(e.Value);//��ļ���
                if (newvalue <= 0)
                {
                    e.Valid = false;
                    e.ErrorText = "ʵ�������������0��";
                }
                else if (newvalue > oldvalue)
                {
                    e.Valid = false;
                    e.ErrorText = string.Format("ʵ���������ܴ��ڿ�����(��ǰ���{0}��)��", oldvalue);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void myGridView2_InvalidValueException(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            MsgBox.ShowError(e.ErrorText);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MsgBox.ShowTip("��������̯���õ�����/�����嵥�ܼ���", linkLabel1, 5);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MsgBox.ShowTip("���˷ѷ�̯�������˷�/�ܻ����˷�", linkLabel2, 5);
        }

        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem16_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem17_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView2.Guid.ToString());
        }

        private void myGridControl3_Leave(object sender, EventArgs e)
        {
            myGridControl3.Visible = SendCarNO.Focused;
        }

        private void SetCarInfo()
        {
            int rowhandle = myGridView3.FocusedRowHandle;
            if (rowhandle < 0) return;

            DataRow dr = myGridView3.GetDataRow(rowhandle);
            if (dr == null) return;

            myGridControl3.Visible = false;
            SendCarNO.EditValue = dr["CarNo"];
            SendDriver.EditValue = dr["DriverName"];
            SendDriverPhone.EditValue = dr["DriverPhone"];
        }

        private void myGridControl3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                myGridControl3.Visible = false;
            }
            if (e.KeyCode == Keys.Enter)
            {
                SetCarInfo();
            }
        }

        private void myGridView3_DoubleClick(object sender, EventArgs e)
        {
            SetCarInfo();
        }

        private void SendCarNO_Enter(object sender, EventArgs e)
        {
            myGridControl3.Left = SendCarNO.Left;
            myGridControl3.Top = SendCarNO.Top + SendCarNO.Height + 2;
            myGridControl3.Visible = true;
        }

        private void SendCarNO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
                myGridControl3.Focus();
            if (e.KeyCode == Keys.Escape)
            {
                myGridControl3.Visible = false;
            }
        }

        private void SendCarNO_Leave(object sender, EventArgs e)
        {
            myGridControl3.Visible = myGridControl3.Focused;
        }

        private void barEditItem4_ShownEditor(object sender, ItemClickEventArgs e)
        {
            textEdit3.Focus();
        }

        private void textEdit3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) simpleButton6.PerformClick();
        }

        private void myGridView2_RowCountChanged(object sender, EventArgs e)
        {
            //if (myGridView2.RowCount < myGridView2RowCount) return;
            //myGridView2RowCount = myGridView2.RowCount;
            //int rowhandle = myGridView2.RowCount - 1;
            //if (rowhandle < 0) return;
            //string s = ConvertType.ToString(myGridView2.GetRowCellValue(rowhandle, "BespeakContent"));
            //if (s != "")
            //{
            //    if (XtraMessageBox.Show("ԤԼ��Ϣ:" + s + "\r\n�Ƿ��޳��˵���", "ϵͳ��ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.OK) return;
            //    myGridView2.SelectRow(rowhandle);
            //    GridViewMove.Move(myGridView2, dataset3, dataset1);//�ƻ�ȥ
            //}
        }

        private void SendCarNO_EditValueChanging(object sender, ChangingEventArgs e)
        {
            if (e.NewValue == null) return;
            string value = e.NewValue.ToString();
            myGridView3.Columns["CarNo"].FilterInfo = new ColumnFilterInfo(
                    "[CarNo] LIKE " + "'%" + value + "%'"
                    + " OR [DriverName] LIKE" + "'%" + value + "%'"
                    + " OR [DriverPhone] LIKE" + "'%" + value + "%'",
                    "");
        }

        private void edsheng_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.AreaManager.FillCityToImageComBoxEdit(edcity, edsheng.EditValue);
        }

        private void edcity_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.AreaManager.FillCityToImageComBoxEdit(edarea, edcity.EditValue);
        }

        private void edarea_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.AreaManager.FillCityToImageComBoxEdit(edtown, edarea.EditValue);
        }

        private void repositoryItemPopupContainerEdit2_Popup(object sender, EventArgs e)
        {
            int rowhandle = myGridView2.FocusedRowHandle;
            if (rowhandle < 0) return;

            edbillno.Text = GridOper.GetRowCellValueString(myGridView2, rowhandle, "BillNo");
            edweight.Text = ConvertType.ToDecimal(myGridView2.GetRowCellValue(rowhandle, "Weight"), "");
            edvolumn.Text = ConvertType.ToDecimal(myGridView2.GetRowCellValue(rowhandle, "Volume"), "");

            CommonClass.SetSelectIndex(GridOper.GetRowCellValueString(myGridView2, rowhandle, "SendToProvince"), edsheng);
            CommonClass.SetSelectIndex(GridOper.GetRowCellValueString(myGridView2, rowhandle, "SendToCity"), edcity);
            CommonClass.SetSelectIndex(GridOper.GetRowCellValueString(myGridView2, rowhandle, "SendToArea"), edarea);
            CommonClass.SetSelectIndex(GridOper.GetRowCellValueString(myGridView2, rowhandle, "SendToStreet"), edtown);
            textEdit4.Text = ConvertType.ToDecimal(myGridView2.GetRowCellValue(rowhandle, "AccSend"), "");
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            int rowhandle = myGridView2.FocusedRowHandle;
            if (rowhandle < 0) return;

            myGridView2.SetRowCellValue(rowhandle, "SendToProvince", edsheng.Text.Trim());
            myGridView2.SetRowCellValue(rowhandle, "SendToCity", edcity.Text.Trim());
            myGridView2.SetRowCellValue(rowhandle, "SendToArea", edarea.Text.Trim());
            myGridView2.SetRowCellValue(rowhandle, "SendToStreet", edtown.Text.Trim());

            myGridView2.SetRowCellValue(rowhandle, "AccSend", ConvertType.ToDecimal(textEdit4.Text));
            edsheng.Text = textEdit4.Text = "";
        }
    }
}