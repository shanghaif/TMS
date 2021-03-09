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
using Newtonsoft.Json;

namespace ZQTMS.UI
{
    public partial class frmBatchMidSign : BaseForm
    {
        private DataSet ds = new DataSet();
        private DataSet ds1 = new DataSet();
        GridHitInfo hitInfo = null;

        public frmBatchMidSign()
        {
            InitializeComponent();
            barEditItem1.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem1_EditValueChanging);
            barEditItem1.Edit.KeyDown += new KeyEventHandler(barEditItem1_KeyDown);
            barEditItem2.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem2_EditValueChanging);
            barEditItem2.Edit.KeyDown += new KeyEventHandler(barEditItem2_KeyDown);
        }
        public string sFrmName = "";
        public int isLocal = 0;

        //��ѯ
        private void getdata()
        {
            string proc = "QSP_GET_MIDDLE_FOR_SIGN";
            if (CommonClass.UserInfo.UserDB == UserDB.ZQTMS3PL)
            {
                proc = "QSP_GET_MIDDLE_FOR_SIGN_3PL";
            }
            try
            {
                ds.Clear();
                ds1.Clear();
                myGridView1.ClearColumnsFilter();
                myGridView2.ClearColumnsFilter();

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("webName", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("siteName", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("isLocal", isLocal));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, proc, list);
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

        private void frmBatchMidSign_Load(object sender, EventArgs e)
        {
            if (!CommonClass.FormSet(this, false, true)) return;
            CommonClass.GetGridViewColumns(myGridView1, false, myGridView2);
            GridOper.SetGridViewProperty(myGridView1, myGridView2);
            BarMagagerOper.SetBarPropertity(bar1, bar2);//����о���Ĺ���������������ʵ��
            GridOper.RestoreGridLayout(myGridView1, myGridView2);
            SignOperator.Text = ConvertType.ToString(CommonClass.UserInfo.UserName);
            SignDate.DateTime = CommonClass.gcdate;
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

        //��֤������
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

            float sumMiddlePay = 0;
            int sumCount = 0;
            int MiddleCount = 0;
            int MiddlePayStateCount = 0;
            int MiddleAccVerifStateCount = 0;
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                if (ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "AccMiddlePay")) > 0)
                {
                    MiddleCount++;
                    sumMiddlePay += ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "AccMiddlePay"));
                }
                if (ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "MiddlePayState")) == 1)
                {
                    MiddlePayStateCount++;
                }
                if (ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "MiddleAccVerifState")) == 1)
                {
                    MiddleAccVerifStateCount++;
                }
            }
            sumCount = myGridView2.RowCount;
            string sShowOK = "��ǰѡ�У�" + ConvertType.ToString(sumCount) + "Ʊ\r\n��ת���Ѻ����У�" + ConvertType.ToString(MiddleAccVerifStateCount) + "Ʊ\r\n��ת�����յ��У�" + ConvertType.ToString(MiddlePayStateCount)
                + "\r\n��ת���ܽ��Ϊ��" + ConvertType.ToString(sumMiddlePay) + "\r\n�Ƿ������";

            if (MsgBox.ShowYesNo(sShowOK) != DialogResult.Yes)
            {
                return;
            }
            string BillNo = "";
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                BillNo = BillNo + myGridView2.GetRowCellValue(i, "BillNo").ToString().Trim() + '@';
            }

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SignNO", Guid.NewGuid()));
                list.Add(new SqlPara("BillNo", BillNo));
                list.Add(new SqlPara("SignType", sFrmName));
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

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLSIGN_MIDDLE", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    #region ZQTMSͬ��ǩ��
                    //��֤�Ƿ������ֲ�
                    List<SqlPara> list1 = new List<SqlPara>();
                    list1.Add(new SqlPara("BillNoStr", BillNo));
                    SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_billDepartureFBList", list1);
                    DataTable dt = SqlHelper.GetDataTable(sps1);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        BillNo = string.Empty;
                        foreach (DataRow row in dt.Rows)
                        {
                            BillNo += row["BillNo"].ToString() + "@";
                        }
                        Dictionary<string, string> dic_ZQTMS = new Dictionary<string, string>();
                        dic_ZQTMS.Add("Bills", BillNo);
                        dic_ZQTMS.Add("SendBatchs", "");
                        dic_ZQTMS.Add("SignType", "��תǩ��");
                        dic_ZQTMS.Add("SignMan", SignMan.Text.Trim());
                        dic_ZQTMS.Add("SignManCardID", SignManCardID.Text.Trim());
                        dic_ZQTMS.Add("AgentMan", "");
                        dic_ZQTMS.Add("SignManPhone", SignManPhone.Text.Trim());
                        dic_ZQTMS.Add("AgentCardId", SignManCardID.Text.Trim());
                        //dic_ZQTMS.Add("SignDate", @"\/Date(" + SignDate.DateTime.ToString() + @")\/");
                        dic_ZQTMS.Add("SignDesc", "");
                        dic_ZQTMS.Add("SignOperator", SignOperator.Text.Trim());
                        dic_ZQTMS.Add("SignSite", ConvertType.ToString(CommonClass.UserInfo.SiteName));
                        dic_ZQTMS.Add("SignWeb", ConvertType.ToString(CommonClass.UserInfo.WebName));
                        dic_ZQTMS.Add("SignContent", SignContent.Text.Trim());

                        dic_ZQTMS.Add("LoginAreaName", CommonClass.UserInfo.AreaName);
                        dic_ZQTMS.Add("LoginCauseName", CommonClass.UserInfo.CauseName);
                        dic_ZQTMS.Add("LoginDepartName", CommonClass.UserInfo.DepartName);
                        dic_ZQTMS.Add("LoginSiteName", CommonClass.UserInfo.SiteName);
                        dic_ZQTMS.Add("LoginWebName", CommonClass.UserInfo.WebName);
                        dic_ZQTMS.Add("LoginUserAccount", CommonClass.UserInfo.UserAccount);
                        dic_ZQTMS.Add("LoginUserName", CommonClass.UserInfo.UserName);
                        dic_ZQTMS.Add("ExceptType", "");
                        dic_ZQTMS.Add("ExceptContent", "");
                        dic_ZQTMS.Add("ExceptReason", "");

                        //ZQTMSǩ��ͬ���ӿڵ���
                        
                        string url = HttpHelper.urlSignSyn;
                        string data = JsonConvert.SerializeObject(dic_ZQTMS);
                        //data = data.TrimStart('[').TrimEnd(']');
                        ResponseModelClone<string> res = HttpHelper.HttpPost(data, url);
                        if (res.State != "200")
                        {
                            string errorNode = isLocal == 0 ? "������תǩ��" : "�ն���תǩ��";
                            List<SqlPara> listLog = new List<SqlPara>();
                            listLog.Add(new SqlPara("BillNo", BillNo));
                            listLog.Add(new SqlPara("Batch", ""));
                            listLog.Add(new SqlPara("ErrorNode", errorNode));
                            listLog.Add(new SqlPara("ExceptMessage", res.Message));
                            SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                            SqlHelper.ExecteNonQuery(spsLog);
                            //MsgBox.ShowError(res.State + "��" + res.Message);
                        }
                        else
                        {
                            MsgBox.ShowOK("��������ɣ�ZQTMS��ͬ��ǩ�գ�");
                        }
                    }
                    else
                    {
                        MsgBox.ShowOK();
                    }
                    #endregion
                    //CommonSyn.SignTimeSyn(BillNo, "��תǩ��");//ZQTMSǩ��ʱЧͬ��
                    //CommonSyn.TraceSyn(null, BillNo, 15, "��תǩ��", 1, null, null);
                    setdata();
                    CommonSyn.MidConfirmSyn(BillNo, CommonClass.UserInfo.UserName, CommonClass.UserInfo.WebName);//��ת�Ḷͬ��zaj 2018-6-13
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

        //��ȡ
        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            getdata();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            //GridViewMove.ExportToExcel(myGridView1);
            GridOper.ExportToExcel(myGridView1); //maohui20180411
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
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
            GridOper.SaveGridLayout(myGridView2, myGridView2.Guid.ToString());
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

        // ȡ���˳�
        private void btnClose_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            //GridViewMove.ExportToExcel(myGridView2);
            GridOper.ExportToExcel(myGridView2);  //maohui20180411
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

        //����
        private void btnOk_Click(object sender, EventArgs e)
        {
            check();
        }

        //�ر�
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //�쳣ǩ��
        private void btnErrorSign_Click(object sender, EventArgs e)
        {
            string BillNo = "";
            if (myGridView2.RowCount > 1)
            {
                MsgBox.ShowOK("��⵽��ǩ���б����ж൥���쳣ǩ��ÿ��ֻ��ǩ��һ���������޳�������ĵ���");
                return;
            }
            if (myGridView2.RowCount == 0)
            { return; }
            if (myGridView2.RowCount > 0)
            {
                //�����½�ͻ����쳣ǩ��
                //string PaymentMode = myGridView2.GetRowCellValue(0, "PaymentMode").ToString();
                //if (PaymentMode == "�½�")
                //{
                //    MsgBox.ShowOK("�˵����ʽΪ�½ᣬ���������쳣ǩ�գ�");
                //    return;
                //}
                BillNo = myGridView2.GetRowCellValue(0, "BillNo").ToString();//�����½�ͻ����쳣ǩ��--ë��20170911
                try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("BillNo", BillNo));
                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_BILLPETTYPAY_BYBILLNO", list);
                    DataSet ds = SqlHelper.GetDataSet(sps);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        MsgBox.ShowOK("�˵�������С���⸶���������쳣ǩ��");
                        return;
                    }
                    if (SignMan.Text.Trim() == "")
                    {
                        MsgBox.ShowOK("������дǩ���ˣ�");
                        return;
                    }
                    if (SignContent.Text.Trim() == "")
                    {
                        MsgBox.ShowOK("�쳣ǩ�ձ�����д���˵����");
                        return;
                    }

                }
                catch (Exception ex)
                {

                    MsgBox.ShowException(ex);
                }
                frmErrorSignEdit edit = new frmErrorSignEdit();
                edit.billNO = BillNo;
                edit.signType = "��תǩ��";
                edit.Text = "��ת�쳣ǩ��";
                edit.signMan = SignMan.Text.Trim();
                edit.signManPhone = SignManPhone.Text.Trim();
                edit.signManCardID = SignManCardID.Text.Trim();
                edit.ShowDialog();
                if (edit.DialogResult != DialogResult.OK)
                {
                    return;
                }

            }
            check();  
        }
        private void setdata()
        {
            string unitstr = "";
            string a;
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                a = myGridView2.GetRowCellValue(i, "FetchPay").ToString().Trim();
                if (!String.IsNullOrEmpty(a))
                {
                    unitstr += myGridView2.GetRowCellValue(i, "BillNo").ToString() + "@";
                }
            }
            try
            {
                List<SqlPara> list1 = new List<SqlPara>();
                list1.Add(new SqlPara("BillNoStr", unitstr));
                list1.Add(new SqlPara("ConfirmMan", CommonClass.UserInfo.UserName));
                list1.Add(new SqlPara("ConfirmWeb", CommonClass.UserInfo.WebName));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "USP_CHECK_FETCHPAYADUIT_MID_CONFIRM", list1);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    MsgBox.ShowOK("ϵͳ��Ĭ������ת�Ḷȷ��");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
    }
}