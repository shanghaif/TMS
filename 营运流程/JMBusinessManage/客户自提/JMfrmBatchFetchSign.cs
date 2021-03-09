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
using System.IO;
using System.Threading;

namespace ZQTMS.UI
{
    public partial class JMfrmBatchFetchSign : BaseForm
    {
        private DataSet ds = new DataSet();
        private DataSet ds1 = new DataSet();
        GridHitInfo hitInfo = null;

        static JMfrmBatchFetchSign fbfs;

        public static JMfrmBatchFetchSign Get_JMfrmBatchFetchSign { get { if (fbfs == null || fbfs.IsDisposed)fbfs = new JMfrmBatchFetchSign(); return fbfs; } }

        public JMfrmBatchFetchSign()
        {
            InitializeComponent();
            barEditItem1.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem1_EditValueChanging);
            barEditItem1.Edit.KeyDown += new KeyEventHandler(barEditItem1_KeyDown);
            barEditItem2.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem2_EditValueChanging);
            barEditItem2.Edit.KeyDown += new KeyEventHandler(barEditItem2_KeyDown);
        }

        #region ���֤���
        int USB_State = 0;//���֤�豸״̬
        static StringBuilder cName = new StringBuilder(200); //����
        static StringBuilder Gender = new StringBuilder(200); //�Ա�
        static StringBuilder Folk = new StringBuilder(200); //����
        static StringBuilder BirthDay = new StringBuilder(200);//��������
        static StringBuilder Code = new StringBuilder(200);//���֤��
        static StringBuilder Address = new StringBuilder(200); //��ַ
        static StringBuilder Agency = new StringBuilder(200);//ǩ֤����
        static StringBuilder ExpireStart = new StringBuilder(200); //��Ч����ʼ
        static StringBuilder ExpireEnd = new StringBuilder(200); //��Ч�ڽ���

        static byte[] photo;
        string path = Application.StartupPath + @"\1.jpg";
        #endregion

        #region ValidateCode
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                //��Ҫ4���ļ���SavePhoto.dll   sdtapi.dll   JpgDll.dll    Dewlt.dll
                if (!File.Exists(Application.StartupPath + "\\sdtapi.dll"))
                {
                    timer1.Enabled = false;
                    return;
                }
                if (USB_State == 0)
                {
                    if (IdentityCard.InitComm(1001) == 1)
                    {
                        label33.Text = "���֤��֤�豸�Ѿ���!";
                        USB_State = 1;
                        timer1.Interval = 500;
                        timer1.Enabled = false;
                        timer1.Enabled = true;
                    }
                    else
                    {
                        label33.Text = "���֤��֤�豸δ���Ӻ�!";
                        USB_State = 0;
                    }
                }
                else
                {
                    int ret = IdentityCard.Authenticate();
                    if (ret == 1)   //�ҵ������� �Ƿ�Ҳ��ô��ʾ?
                    {
                        int state = IdentityCard.ReadBaseInfos(cName, Gender, Folk, BirthDay, Code, Address, Agency, ExpireStart, ExpireEnd);
                        if (cName != null && cName.ToString().Trim() != "")
                        {
                            SignMan.Text = cName.ToString().Trim();
                            SignManCardID.Text = Code.ToString().Trim();
                        }
                        int a = 0;
                        while (true)
                        {
                            if (File.Exists(path))
                            {
                                if (a == 10)
                                {
                                    return;
                                }
                                using (FileStream stream = new FileInfo(path).OpenRead())
                                {
                                    photo = new byte[stream.Length];
                                    stream.Read(photo, 0, Convert.ToInt32(stream.Length));
                                    Image img = new Bitmap(stream);
                                    stream.Close();
                                    stream.Dispose();

                                    alertControl1.Show(this, "", "", img);
                                    timer1.Enabled = false;
                                    return;
                                }
                            }
                            a++;
                            Thread.Sleep(200);
                        }

                        //if (state == 1)
                        //{
                        //    string path = Application.StartupPath + @"\1.jpg";

                        //    int a = 0;
                        //    while (true)
                        //    {
                        //        if (File.Exists(path))
                        //        {
                        //            if (a == 10)
                        //            {
                        //                MsgBox.ShowOK("��ȡ���֤ʶ����Ϣʧ��!");
                        //                return;
                        //            }
                        //            FileStream stream = new FileInfo(path).OpenRead();
                        //            photo = new byte[stream.Length];
                        //            stream.Read(photo, 0, Convert.ToInt32(stream.Length));
                        //            Image img = new Bitmap(stream);
                        //            stream.Close();

                        //            SignMan.Text = cName.ToString().Trim();
                        //            SignManCardID.Text = Code.ToString().Trim();

                        //            alertControl1.Show(this, "", "", img);
                        //            timer1.Enabled = false;
                        //            return;
                        //        }
                        //        a++;
                        //        Thread.Sleep(200);
                        //    }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                timer1.Enabled = false;
                XtraMessageBox.Show(ex.Message, "ϵͳ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void JMfrmBatchFetchSign_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                timer1.Enabled = false;
                IdentityCard.CloseComm();
                if (File.Exists(Application.StartupPath + @"\1.jpg"))
                {
                    File.Delete(Application.StartupPath + @"\1.jpg");
                    File.Delete(Application.StartupPath + @"\2.jpg");
                    File.Delete(Application.StartupPath + @"\photo.bmp");
                }
            }
            catch (Exception) { }
        }

        private void alertControl1_BeforeFormShow(object sender, DevExpress.XtraBars.Alerter.AlertFormEventArgs e)
        {
            e.AlertForm.Size = new Size(355, 220);
        }
        #endregion


        private void getdata()
        {
            try
            {
                ds.Clear();
                ds1.Clear();
                myGridView1.ClearColumnsFilter();
                myGridView2.ClearColumnsFilter();

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("webName", CommonClass.UserInfo.WebName == null ? "" : CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("siteName", CommonClass.UserInfo.SiteName == null ? "" : CommonClass.UserInfo.SiteName));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_FETCH_FOR_SIGN", list);
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

        private void JMfrmBatchFetchSign_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this, false);
            CommonClass.GetGridViewColumns(myGridView1, false, myGridView2);
            GridOper.SetGridViewProperty(myGridView1, myGridView2);
            BarMagagerOper.SetBarPropertity(bar1, bar2);//����о���Ĺ���������������ʵ��
            GridOper.RestoreGridLayout(myGridView1, myGridView2);
            SignOperator.Text = ConvertType.ToString(CommonClass.UserInfo.UserName);
            SignDate.DateTime = CommonClass.gcdate;

            timer1.Enabled = true;
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        private void myGridControl2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GridViewMove.Move(myGridView2, ds1, ds);
        }

        private void myGridControl2_DragDrop(object sender, DragEventArgs e)
        {
            GridViewMove.Move(myGridView1, ds, ds1);
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

        private void myGridControl1_DragDrop(object sender, DragEventArgs e)
        {
            GridViewMove.Move(myGridView2, ds1, ds);
        }

        private void check()
        {
            if (myGridView2.RowCount == 0)
            {
                XtraMessageBox.Show("û�з����κ���Ҫǩ�յ��嵥�������ڵڢٲ��й����嵥��", "ϵͳ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (chkSMS.Checked)
            {
                if (0 == 0)
                {
                    XtraMessageBox.Show("��ѡ���˶���֪ͨ��ȴû�й�ѡ�κ���Ҫ���Ͷ��ŵ��˵�!", "ϵͳ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            if (SignMan.Text.Trim() == "")
            {
                MsgBox.ShowOK("������дǩ���ˣ�");
                return;
            }
            //�ж����е��������״̬ ƴ��״̬�ַ���

            float sumCollectionPay = 0;
            float sumFetchPay = 0;
            int collectionCount = 0;
            int fetchCount = 0;
            int collectionStateCount = 0;
            int fetchStateCount = 0;
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                //sumCollectionPay += ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "CollectionPay"));
                //sumFetchPay += ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "FetchPay"));
                if (ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "CollectionPay")) > 0)
                {
                    collectionCount++;
                    sumCollectionPay += ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "CollectionPay"));
                }
                if (ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "FetchPay")) > 0)
                {
                    fetchCount++;
                    sumFetchPay += ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "FetchPay"));
                }
                if (ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "CollectionPayState")) == 1)
                {
                    collectionStateCount++;
                }
                if (ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "FetchPayVerifState")) == 1)
                {
                    fetchStateCount++;
                }
            }
            //��ʾ��Ϣ
            if ((new JMfrmShowInfo(sumCollectionPay, sumFetchPay, myGridView2.RowCount)).ShowDialog() != DialogResult.Yes) return;

            string BillNo = "";
            string DepartureListNO = "";
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                BillNo += ConvertType.ToString(myGridView2.GetRowCellValue(i, "BillNo")) + '@';
                DepartureListNO += ConvertType.ToString(myGridView2.GetRowCellValue(i, "Id")) + '@';
            }

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SignNO", Guid.NewGuid()));
                list.Add(new SqlPara("BillNoStr", BillNo));
                list.Add(new SqlPara("SignType", "���ǩ��"));
                list.Add(new SqlPara("SignMan", SignMan.Text.Trim()));
                list.Add(new SqlPara("SignManCardID", SignManCardID.Text.Trim()));
                list.Add(new SqlPara("AgentMan", ""));
                list.Add(new SqlPara("SignManPhone", SignManPhone.Text.Trim()));
                list.Add(new SqlPara("AgentCardId", SignManCardID.Text.Trim()));
                list.Add(new SqlPara("SignDate", SignDate.DateTime));
                list.Add(new SqlPara("SignDesc", ""));
                list.Add(new SqlPara("SignOperator", SignOperator.Text.Trim()));
                list.Add(new SqlPara("SignSite", ConvertType.ToString(CommonClass.UserInfo.SiteName)));
                list.Add(new SqlPara("SignWeb", ConvertType.ToString(CommonClass.UserInfo.WebName)));
                list.Add(new SqlPara("SignContent", SignContent.Text.Trim()));
                list.Add(new SqlPara("IdStr", DepartureListNO));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLSIGN", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    ds1.Clear();
                    SignMan.Text = SignManCardID.Text = SignContent.Text = "";
                    SignDate.DateTime = CommonClass.gcdate;
                    SignOperator.Text = CommonClass.UserInfo.UserName;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void myGridControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GridViewMove.Move(myGridView1, ds, ds1);
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            getdata();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "���ǩ�տ��");
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView1.OptionsView.ShowAutoFilterRow = !myGridView1.OptionsView.ShowAutoFilterRow;
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
            GridOper.ExportToExcel(myGridView2, "���ǩ����ѡ���");
        }

        ////////////////////////////////////////
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

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (CommonClass.CheckKongHuo(myGridView2, 1))
            {
                MsgBox.ShowOK("ѡ����嵥�����ػ����˵�,�������!");
                return;
            }
            check();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void myGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GridViewMove.Move(myGridView1, ds, ds1);

            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (CommonClass.CheckKongHuo(myGridView2, 1))
            {
                MsgBox.ShowOK("ѡ����嵥�����ػ����˵�,�������!");
                return;
            }
            string str = "";
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                str += ConvertType.ToString(myGridView2.GetRowCellValue(i, "BillNo")) + "@";
            }
            check();
            PrintQSD(str);

        }

        private void PrintQSD(string BillNoStr)
        {
            if (string.IsNullOrEmpty(BillNoStr)) return;
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILLINFO_PRINT_QSD", new List<SqlPara> { new SqlPara("BillNoStr", BillNoStr) }));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                MsgBox.ShowError("û���ҵ�ѡ�е��˵���Ϣ,��ӡʧ��,(����������˵��Ƿ��ѱ�ɾ��)!");
                return;
            }
            //DataTable dt = ds.Tables[0].Clone();
            //frmPrintRuiLang fprl;
            //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //{
            //    dt.ImportRow(ds.Tables[0].Rows[i]);
            //    fprl = new frmPrintRuiLang("�����(�״�)", dt);
            //    fprl.ShowDialog();
            //}
            frmRuiLangService.Print("�����(�״�)", ds.Tables[0]);
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem15_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem16_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView2);
        }

        private void barButtonItem17_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView2.Guid.ToString());
        }

        private void barButtonItem18_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView2);
        }

    }
}