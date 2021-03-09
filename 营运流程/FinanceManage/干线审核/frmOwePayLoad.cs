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
using DevExpress.XtraEditors.Controls;
using System.Reflection;

namespace ZQTMS.UI
{
    public partial class frmOwePayLoad : BaseForm
    {
        private DataSet ds = new DataSet();
        private DataSet ds1 = new DataSet();
        GridHitInfo hitInfo;
        private DataSet ds2 = new DataSet();//��Ƿ
        private DataSet ds3 = new DataSet();//�½�
        private DataSet ds4 = new DataSet();//�ص���
        private DataSet ds5 = new DataSet();//����ǰ��
        private DataSet ds6 = new DataSet();//Ƿ��
        public string bdate;
        public string edate;
        public frmOwePayLoad()
        {
            InitializeComponent();
            barEditItem1.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem1_EditValueChanging);
            barEditItem1.Edit.KeyDown += new KeyEventHandler(barEditItem1_KeyDown);
            barEditItem2.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem2_EditValueChanging);
            barEditItem2.Edit.KeyDown += new KeyEventHandler(barEditItem2_KeyDown);
            this.panelControl1.Hide();
            //barEditItem5.DropDownSuperTip.Items.Add("�Ѻ���");
            //barEditItem5.DropDownSuperTip.Items.Add("δ����");
        }

        private void getdata()
        {
            myGridControl1.DataSource = myGridControl2.DataSource = null;
            try
            {
                ds.Clear();
                ds1.Clear();
                myGridView1.ClearColumnsFilter();
                myGridView2.ClearColumnsFilter();

                List<SqlPara> list = new List<SqlPara>();
                string CauseName = barEditCause == null || barEditCause.EditValue == null ? CommonClass.UserInfo.CauseName : barEditCause.EditValue.ToString();
                string AreaName = barEditArea == null || barEditArea.EditValue == null ? CommonClass.UserInfo.AreaName : barEditArea.EditValue.ToString();
                string WebName = barEditWeb == null || barEditWeb.EditValue == null ? CommonClass.UserInfo.WebName : barEditWeb.EditValue.ToString();
                string TypeName = barType.EditValue.ToString();
                
                CauseName = CauseName == "ȫ��" ? "%%" : CauseName;
                AreaName = AreaName == "ȫ��" ? "%%" : AreaName;
                WebName = WebName == "ȫ��" ? "%%" : WebName;
                list.Add(new SqlPara("CauseName", CauseName));
                list.Add(new SqlPara("AreaName", AreaName));
                list.Add(new SqlPara("BegWeb", WebName));
                list.Add(new SqlPara("Type", TypeName));
                list.Add(new SqlPara("t1", barEditItem3.EditValue));
                list.Add(new SqlPara("t2", barEditItem4.EditValue));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GetALL_InFee_Load_NEW_FCD", list);
                ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                ds1 = ds.Clone();
                myGridControl1.DataSource = ds.Tables[0];
                myGridControl2.DataSource = ds1.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void frmBatchFetchSign_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, false, myGridView2);
            GridOper.SetGridViewProperty(myGridView1, myGridView2);
            BarMagagerOper.SetBarPropertity(bar1, bar2);  //����о���Ĺ���������������ʵ��
            GridOper.RestoreGridLayout(myGridView1, myGridView2);

            CommonClass.Create_BarEditItem_Web(barManager1, bar1, barEditWeb);
            CommonClass.Create_BarEditItem_Area(barManager1, bar1, barEditArea);
            CommonClass.Create_BarEditItem_Cause(barManager1, bar1, barEditCause);
            Create_BarEditItem_Type(barManager1, bar1, barType);
            
            barEditCause.EditValueChanged += new System.EventHandler(this.barEditItem1_EditValueChanged);
            barEditArea.EditValueChanged += new System.EventHandler(this.barEditItem1_EditValueChanged_2);
            barEditCause.EditValue = CommonClass.UserInfo.CauseName;
            barEditArea.EditValue = CommonClass.UserInfo.AreaName;
            barEditWeb.EditValue = CommonClass.UserInfo.WebName;
            barType.EditValue = "ȫ��";


            splitContainerControl1.Horizontal = false;
            splitContainerControl1.SplitterPosition = 480;
            barEditItem3.EditValue = bdate;
            barEditItem4.EditValue = edate; 
        }

        BarEditItem barEditCause = new BarEditItem(); //������ҵ��
        BarEditItem barEditArea = new BarEditItem(); //���ɴ���
        BarEditItem barEditWeb = new BarEditItem(); //��������
        BarEditItem barType = new BarEditItem();//��������
       


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

        #region �ƶ������������
        private void gridControl2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GridViewMove.Move(myGridView2, ds1, ds);
        }

        private void gridControl2_DragDrop(object sender, DragEventArgs e)
        {
            GridViewMove.Move(myGridView1, ds, ds1);
        }

        private void gridControl1_MouseMove(object sender, MouseEventArgs e)
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

        private void gridControl2_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void gridControl1_MouseDown(object sender, MouseEventArgs e)
        {
            hitInfo = myGridView1.CalcHitInfo(new Point(e.X, e.Y));
        }

        private void gridControl2_MouseMove(object sender, MouseEventArgs e)
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

        private void gridControl2_MouseDown(object sender, MouseEventArgs e)
        {
            hitInfo = myGridView2.CalcHitInfo(new Point(e.X, e.Y));
        }

        private void gridControl1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void gridControl1_DragDrop(object sender, DragEventArgs e)
        {
            GridViewMove.Move(myGridView2, ds1, ds);
        }

        private void gridControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GridViewMove.Move(myGridView1, ds, ds1);
        }
        #endregion

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            getdata();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView1.OptionsView.ShowAutoFilterRow = !myGridView1.OptionsView.ShowAutoFilterRow;
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            //GridViewMove.QuickSearch();
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridViewMove.Move(myGridView1, ds, ds1);
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView1.SelectAll();
            GridViewMove.Move(myGridView1, ds, ds1);
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView2);
        }

        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridViewMove.Move(myGridView2, ds1, ds);
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView2.SelectAll();
            GridViewMove.Move(myGridView2, ds1, ds);
        }

        private void btnClose_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView2);
        }

        
        private void barEditItem1_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue == null) return;
            string szfilter = e.NewValue.ToString().Trim();
            if (szfilter != "")
            {
                myGridView1.ClearColumnsFilter();
                myGridView1.Columns["BillNo"].FilterInfo = new ColumnFilterInfo("[BillNo] LIKE " + "'%" + szfilter + "%'" + " OR [BillNo] LIKE" + "'%" + szfilter + "%'", "");
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
            GridViewMove.Move(myGridView1, ds, ds1);
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
                myGridView2.Columns["BillNo"].FilterInfo = new ColumnFilterInfo("[BillNo] LIKE " + "'%" + szfilter + "%'" + " OR [BillNo] LIKE" + "'%" + szfilter + "%'", "");
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
            GridViewMove.Move(myGridView2, ds1, ds);
            ((DevExpress.XtraEditors.TextEdit)sender).Text = "";

            myGridView2.ClearColumnsFilter();
            e.Handled = true;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {

            #region ����δִ�иĵ������ܺ��� zb20190507
            try
            {
                StringBuilder sb = new StringBuilder();
                List<SqlPara> list = new List<SqlPara>();
                //��ȡ�����˵���
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    sb.Append(myGridView2.GetRowCellValue(i, "BillNo"));
                    sb.Append("@");
                }
                list.Add(new SqlPara("BillNos", sb));
                //list.Add(new SqlPara("BillNos", "4407588@4385539@4460360@14254065@")); ����
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_Get_ApplyStateByBillNo", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                sb.Remove(0, sb.Length);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string BillNO = ds.Tables[0].Rows[i]["BillNO"].ToString();
                        sb.Append(BillNO);
                        sb.Append(",");
                        MsgBox.ShowOK("����:" + sb + "δִ�иĵ�,���ܽ��к���������ִ�иĵ�����!");
                        return;

                    }
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                return;
            }
            #endregion
            string allBillNo = "";
            //StringBuilder owePay = new StringBuilder();
            //StringBuilder monthPay = new StringBuilder();
            //StringBuilder receiptPay = new StringBuilder();
            //StringBuilder berArrivalPay = new StringBuilder();
            ds2 = ds1.Clone();
            ds2.Clear();
            ds3 = ds1.Clone();
            ds3.Clear();
            ds4 = ds1.Clone();
            ds4.Clear();
            ds5= ds1.Clone();
            ds5.Clear();
            ds6 = ds1.Clone();
            ds6.Clear();

            VerifyOffAccountDeel voaDeel = new VerifyOffAccountDeel();

            if (myGridView2.RowCount == 0)
            {
                XtraMessageBox.Show("û�з����κ���Ҫ�����ĵ��������ڵڢٲ��й����嵥��", "ϵͳ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (myGridView2.RowCount > 0)
            {
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    string billno = myGridView2.GetRowCellValue(i, "BillNo").ToString();
                    allBillNo += billno + ",";
                    string type = myGridView2.GetRowCellValue(i, "FeeType").ToString();




                    #region ���Ʋ�ͬ���ʽ����һ�����
                    if (i> 0)
                    {
                        if (myGridView2.GetRowCellValue(i, "FeeType").ToString() != myGridView2.GetRowCellValue(i - 1, "FeeType").ToString())
                        {
                            MsgBox.ShowOK("��ͬ���ʽ������һ�����");
                            return;
                        }

                    }
                    #endregion

                    if (type == "��Ƿ")
                    {
                        //owePay.Append(billno + ",");
                        ds2.Tables[0].ImportRow(ds1.Tables[0].Rows[i]);
                        
                    }

                    else if (type == "�½�")
                    {
                        //monthPay.Append(billno + ",");
                        ds3.Tables[0].ImportRow(ds1.Tables[0].Rows[i]);
                    }

                    else if (type == "�ص���")
                    {
                        //receiptPay.Append(billno + ",");
                        ds4.Tables[0].ImportRow(ds1.Tables[0].Rows[i]);
                    }
                    else if (type == "Ƿ��")
                    {
                        ds6.Tables[0].ImportRow(ds1.Tables[0].Rows[i]);
                    }
                    else
                    {
                        //berArrivalPay.Append(billno + ",");
                        ds5.Tables[0].ImportRow(ds1.Tables[0].Rows[i]);
                    }
                }
            
            }


            if (ds2 != null && ds2.Tables.Count != 0 && ds2.Tables[0].Rows.Count != 0)
            {
                voaDeel.SubmitVerify(myGridView2, ds2, VerifyType.��Ƿ.ToString(), "BillNo", null, "CurrentVerifyFee", "AmountLeft", "����");
               // owePay.Length = 0;
            }
            if (ds3 != null && ds3.Tables.Count != 0 && ds3.Tables[0].Rows.Count != 0)
            {
                voaDeel.SubmitVerify(myGridView2, ds3, VerifyType.�½�.ToString(), "BillNo", null, "CurrentVerifyFee", "AmountLeft", "����");
                //monthPay.Length = 0;
            }
            if (ds4 != null && ds4.Tables.Count != 0 && ds4.Tables[0].Rows.Count != 0)
            {
                voaDeel.SubmitVerify(myGridView2, ds4, VerifyType.�ص���.ToString(), "BillNo", null, "CurrentVerifyFee", "AmountLeft", "����");
                //receiptPay.Length = 0;
            }
            if (ds6 != null && ds6.Tables.Count != 0 && ds6.Tables[0].Rows.Count != 0)
            {
                voaDeel.SubmitVerify(myGridView2, ds6, VerifyType.Ƿ��.ToString(), "BillNo", null, "CurrentVerifyFee", "AmountLeft", "����");
                //berArrivalPay.Length = 0;
            }
            if (ds5 != null && ds5.Tables.Count != 0 && ds5.Tables[0].Rows.Count != 0)
            {
                voaDeel.SubmitVerify(myGridView2, ds5, VerifyType.����ǰ��.ToString(), "BillNo", null, "CurrentVerifyFee", "AmountLeft", "����");
                //berArrivalPay.Length = 0;
            }
            ds1.Clear();


            //if (owePay.Length == 0 && monthPay.Length == 0 && receiptPay.Length == 0 && berArrivalPay.Length == 0)
            //{
            //    ds1.Clear();
            //}  
            
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem15_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem16_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem17_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView2);
        }

        private void barButtonItem18_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView2.Guid.ToString());
        }

        private void barButtonItem19_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView2);
        }

        public void Create_BarEditItem_Type(BarManager barManager1, Bar bar1, BarEditItem barEditItem)
        {
            RepositoryItemComboBox repositoryItemComboBox = new RepositoryItemComboBox();
            //RepositoryItemComboBox repositoryItemComboBox = (RepositoryItemComboBox)barType.Edit;
            //repositoryItemComboBox.Items.Clear();
            barManager1.BeginInit();
            repositoryItemComboBox.BeginInit();

            barEditItem.Caption = "����";
            barEditItem.Edit = repositoryItemComboBox;

            repositoryItemComboBox.AutoHeight = false;
            repositoryItemComboBox.Sorted = true;
            repositoryItemComboBox.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
            repositoryItemComboBox.DropDownRows = 10;
            repositoryItemComboBox.TextEditStyle = TextEditStyles.DisableTextEditor;

            bar1.LinksPersistInfo.Insert(0, new LinkPersistInfo(((BarLinkUserDefines)((BarLinkUserDefines.PaintStyle | BarLinkUserDefines.Width))), barEditItem, "", false, true, true, 88, null, BarItemPaintStyle.CaptionGlyph));

            if (barManager1.Items.Equals(barEditItem))
                barManager1.Items.Remove(barEditItem);
            else
                barManager1.Items.Add(barEditItem);

            barManager1.RepositoryItems.Add(repositoryItemComboBox);
            repositoryItemComboBox.Items.Add("Ƿ��");
            repositoryItemComboBox.Items.Add("��Ƿ");
            repositoryItemComboBox.Items.Add("�½�");
            repositoryItemComboBox.Items.Add("�ص���");
            repositoryItemComboBox.Items.Add("����ǰ��");
            repositoryItemComboBox.Items.Add("ȫ��");

            barManager1.EndInit();
            repositoryItemComboBox.EndInit();
        }


        private void barButtonItem21_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (splitContainerControl1.Horizontal == true)
            {
                splitContainerControl1.Horizontal = false;
                splitContainerControl1.SplitterPosition = 480;
                return;
            }
            if (splitContainerControl1.Horizontal == false)
            {
                splitContainerControl1.Horizontal = true;
                splitContainerControl1.SplitterPosition = 800;
                return;
            }
        }
        //��������
        private void barButtonItem22_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.panelControl1.Show();
        }
        //����
        private void button1_Click(object sender, EventArgs e)
        {
          try
            {
                if (string.IsNullOrEmpty(txtBillNos.Text))
                {
                    XtraMessageBox.Show("�������뵥�Ż����κš�", "ϵͳ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string sb = txtBillNos.Text.Trim().Replace("\r\n", "@");
                string[] aa = sb.Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries);
                int rowHandle = myGridView2.RowCount;
                for (int i = 0; i < rowHandle; i++)
                {
                    string sb2 = GridOper.GetRowCellValueString(myGridView2, i, "SendBatch");
                    string sb3 = GridOper.GetRowCellValueString(myGridView2, i, "BillNo");
                    for (int j = 0; j < aa.Length; j++)
                    {
                        string sb4 = aa[j];
                        if (sb4 == sb2)
                        {
                            txtBillNos.Text = "";
                            MsgBox.ShowOK("��������κ�����ȡ");
                            return;
                        }
                        if (sb4 == sb3)
                        {
                            txtBillNos.Text = "";
                            MsgBox.ShowOK("����ĵ�������ȡ");
                            return;
                        }
                    }
                }
                List<SqlPara> list = new List<SqlPara>();
                string CauseName = barEditCause == null || barEditCause.EditValue == null ? CommonClass.UserInfo.CauseName : barEditCause.EditValue.ToString();
                string AreaName = barEditArea == null || barEditArea.EditValue == null ? CommonClass.UserInfo.AreaName : barEditArea.EditValue.ToString();
                string WebName = barEditWeb == null || barEditWeb.EditValue == null ? CommonClass.UserInfo.WebName : barEditWeb.EditValue.ToString();
                string TypeName = barType.EditValue.ToString();

                CauseName = CauseName == "ȫ��" ? "%%" : CauseName;
                AreaName = AreaName == "ȫ��" ? "%%" : AreaName;
                WebName = WebName == "ȫ��" ? "%%" : WebName;
                list.Add(new SqlPara("CauseName", CauseName));
                list.Add(new SqlPara("AreaName", AreaName));
                list.Add(new SqlPara("BegWeb", WebName));
                list.Add(new SqlPara("Type", TypeName));
                list.Add(new SqlPara("t1", barEditItem3.EditValue));
                list.Add(new SqlPara("t2", barEditItem4.EditValue));
                list.Add(new SqlPara("BillNos", sb.ToString() + "@"));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GetALL_InFee_Load_NEW_FCD_pilian", list);
                ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    XtraMessageBox.Show("���޴˵�", "ϵͳ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                    if (ds == null || ds.Tables.Count == 0) return;
                    ds1 = ds.Clone();
                    myGridControl1.DataSource = ds.Tables[0];
                    myGridControl2.DataSource = ds1.Tables[0];

                    string retrunStr = string.Empty;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        retrunStr = sb.Replace(row["BillNo"].ToString(), "");
                        sb = retrunStr;
                    }
                    string[] spStr = retrunStr.Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries);
                    retrunStr = string.Join("@", spStr);
                    txtBillNos.Text = retrunStr.Replace("@", "\r\n");
                }
               catch (Exception ex)
                 {
                MsgBox.ShowException(ex);
                 }
        }
        //�˳�
        private void button2_Click(object sender, EventArgs e)
        {
            this.txtBillNos.Text = "";
            this.panelControl1.Hide();
        }

        private void barButtonItem23_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rows = myGridView1.FocusedRowHandle;
            string a = myGridView1.GetRowCellValue(rows, "BillNo").ToString();
            if (rows < 0) return;
            Assembly ass = Assembly.LoadFrom(Application.StartupPath + "\\Plugin\\ZQTMS.UI.BuisinessManage.dll");
            if (ass == null) return;
            Type type = ass.GetType("ZQTMS.UI.frmBillSearchControl");
            if (type == null) return;
            Form frm = (Form)Activator.CreateInstance(type);
            if (frm == null) return;
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Tag = a;
            frm.ShowDialog();
        }    //plh20200521 LMS--6539


      

        private void myGridView1_MouseUp(object sender, MouseEventArgs e)
        {
            PopMenu.ShowPopupMenu(myGridView1, e, popupMenu1);
            int rows = myGridView1.FocusedRowHandle;
            if (rows < 0) return;

        }  //plh20200521 LMS--6539
    }
}