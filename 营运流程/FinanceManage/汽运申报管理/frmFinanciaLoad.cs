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
    public partial class frmFinanciaLoad : BaseForm
    {
        private DataSet ds = new DataSet();
        private DataSet ds1 = new DataSet();
        private DataSet ds2 = new DataSet();//ԭʼ����
        DataTable dt = new DataTable();
        GridHitInfo hitInfo;

        public frmFinanciaLoad()
        {
            InitializeComponent();
            barEditItem1.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem1_EditValueChanging);
            barEditItem1.Edit.KeyDown += new KeyEventHandler(barEditItem1_KeyDown);
            barEditItem2.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem2_EditValueChanging);
            barEditItem2.Edit.KeyDown += new KeyEventHandler(barEditItem2_KeyDown);
           
        }

        private void getdata()
        {
            try
            {
                ds.Clear();
                ds1.Clear();
                myGridView1.ClearColumnsFilter();
                myGridView2.ClearColumnsFilter();
            
                List<SqlPara> list = new List<SqlPara>();
                //string CauseName = barEditCause.EditValue == "ȫ��" ? "%%": barEditCause.EditValue.ToString();
                //string AreaName = barEditArea.EditValue == "ȫ��"  ?"%%" : barEditArea.EditValue.ToString();
                //string WebName = barEditWeb.EditValue == "ȫ��" ? "%%" : barEditWeb.EditValue.ToString();
               
              
                //CauseName = CauseName == "ȫ��" ? "%%" : CauseName;
                //AreaName = AreaName == "ȫ��" ? "%%" : AreaName;
                //WebName = WebName == "ȫ��" ? "%%" : WebName;
                //Btnderp = Btnderp == "ȫ��" ? "%%" : Btnderp;
                //list.Add(new SqlPara("CauseName", CauseName));
                //list.Add(new SqlPara("AreaName", AreaName));
                //list.Add(new SqlPara("BegWeb", WebName));
                //list.Add(new SqlPara("paichedepartment", Btnderp));
                //SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_Delivery_Verify_Load", list);
                list.Add(new SqlPara("ConfirmState", "%%"));
                list.Add(new SqlPara("bdate", "2018-10-30 00:00:00"));
                list.Add(new SqlPara("edate", "2018-10-30 00:00:00"));
                list.Add(new SqlPara("CauseName", "%%"));
                list.Add(new SqlPara("AreaName", "%%"));
                list.Add(new SqlPara("WebName", "%%"));
                list.Add(new SqlPara("Type", "ȫ��"));
                //list.Add(new SqlPara("CauseName", CauseName.Text.Trim()));
                //list.Add(new SqlPara("AreaName", AreaName.Text.Trim()));
                //list.Add(new SqlPara("WebName", WebName.Text.Trim()));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_basfinancialAudit", list);
                ds = SqlHelper.GetDataSet(spe);
                if (ds == null || ds.Tables.Count == 0) return;
                ds1 = ds.Clone();
                ds2 = ds.Clone();
                ////dt = ds.Tables[0].Clone();
                //DataRow[] Rows = ds2.Tables[0].Select("money > 0");
                //dt = ds2.Tables[0].Clone();
                //foreach (DataRow DR in Rows)
                //{
                //    dt.ImportRow(DR);
                //}
         //       ds.Tables.Add(dt.Copy());
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

            //CommonClass.Create_BarEditItem_Web(barManager1, bar1, barEditWeb);
            //CommonClass.Create_BarEditItem_Area(barManager1, bar1, barEditArea);
            //CommonClass.Create_BarEditItem_Cause(barManager1, bar1, barEditCause);
            //barEditCause.EditValueChanged += new System.EventHandler(this.barEditItem1_EditValueChanged);
            //barEditArea.EditValueChanged += new System.EventHandler(this.barEditItem1_EditValueChanged_2);
            //barEditCause.EditValue = CommonClass.UserInfo.CauseName;
            //barEditArea.EditValue = CommonClass.UserInfo.AreaName;
            //barEditWeb.EditValue = CommonClass.UserInfo.WebName;

        }

        //BarEditItem barEditCause = new BarEditItem(); //������ҵ��
        //BarEditItem barEditArea = new BarEditItem(); //���ɴ���
        //BarEditItem barEditWeb = new BarEditItem(); //��������
        
        //private void barEditItem1_EditValueChanged(object sender, EventArgs e)
        //{
        //    RepositoryItemComboBox repositoryItemComboBox = (RepositoryItemComboBox)barEditArea.Edit;
        //    repositoryItemComboBox.Items.Clear();
        //    CommonClass.SetArea(repositoryItemComboBox, barEditCause.EditValue + "", true);
        //    barEditArea.EditValue = "ȫ��";
        //}
        //private void barEditItem1_EditValueChanged_2(object sender, EventArgs e)
        //{
        //    RepositoryItemComboBox repositoryItemComboBox = (RepositoryItemComboBox)barEditWeb.Edit;
        //    repositoryItemComboBox.Items.Clear();
        //    CommonClass.SetCauseWeb(repositoryItemComboBox, barEditCause.EditValue + "", barEditArea.EditValue + "");
        //    barEditWeb.EditValue = "ȫ��";
        //}

        private void gridControl2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //int rowHandle = myGridView1.FocusedRowHandle;
            //string feeType = ConvertType.ToString(myGridView1.GetRowCellValue(rowHandle, "FeeType"));
            //decimal money = ConvertType.ToDecimal(myGridView1.GetRowCellValue(rowHandle, "money"));
            //if (feeType == "��������" || feeType == "�����Ϸ�" || feeType == "��˾��������" || (feeType == "��˾��������" && money<=0))
            //{
            //    myGridView2.DeleteSelectedRows();
            //}
            //else
            //{
            //    GridViewMove.Move(myGridView2, ds1, ds);
            //}
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
            //GridViewMove.Move(myGridView2, ds1, ds);
            GridViewMove.MoveQY(myGridView1, ds, ds1, ds2);
        }

        private void gridControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            GridViewMove.MoveQY(myGridView1, ds, ds1,ds2);
           
        }

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

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            //if (CommonClass.UserInfo.WebRole != "����" && CommonClass.UserInfo.WebRole != "ֱӪ"
            //    && CommonClass.UserInfo.WebRole != "����" && CommonClass.UserInfo.WebRole != "")
            //{
            //    MsgBox.ShowError("��ǰ�û���ɫ���������ò�����");
            //    return;
            //}
            //string strMessage = string.Empty;//������Ϣ
           //GridViewMove.Move(myGridView1, ds, ds1);
           
            //GridViewMove.Move(myGridView1, ds, ds1);
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            //if (CommonClass.UserInfo.WebRole != "����" && CommonClass.UserInfo.WebRole != "ֱӪ"
            //    && CommonClass.UserInfo.WebRole != "����" && CommonClass.UserInfo.WebRole != "")
            //{
            //    MsgBox.ShowError("��ǰ�û���ɫ���������ò�����");
            //    return;
            //}
            //myGridView1.SelectAll();

            //GridViewMove.Move(myGridView1, ds, ds1);
            //string strMessage = string.Empty;//������Ϣ
            //strMessage = GridViewMove.Move(myGridView1, ds, ds1, CommonClass.UserInfo.WebRole, CommonClass.UserInfo.DepartName, CommonClass.UserInfo.UserName, "DeliCode");
            //if (!string.IsNullOrEmpty(strMessage))
            //{
            //    if (strMessage.Contains("��ǰ���Ż��¼�˲���������ɳ���"))
            //    {
            //        MsgBox.ShowError(strMessage);
            //    }
            //    else
            //    {
            //        MsgBox.ShowError("�����û�ֻ�ܺ����ɳ�����Ϊ ���������ɡ������ݣ�����Ϊ�������ɳ��������ݲ����������" + strMessage);
            //    }
            //}
            //GridViewMove.Move(myGridView1, ds, ds1);
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView2);
        }

        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            //GridViewMove.Move(myGridView2, ds1, ds);
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            //myGridView2.SelectAll();
            //GridViewMove.Move(myGridView2, ds1, ds);
        }

        private void btnClose_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView2);
        }

        ////////////////////////////////////////
        private void barEditItem1_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue == null) return;
            string szfilter = e.NewValue.ToString().Trim();
            if (szfilter != "")
            {
                myGridView1.ClearColumnsFilter();
                myGridView1.Columns["DepartureBatch"].FilterInfo = new ColumnFilterInfo("[DepartureBatch LIKE " + "'%" + szfilter + "%'", "");
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
                myGridView2.Columns["DepartureBatch"].FilterInfo = new ColumnFilterInfo("[DepartureBatch] LIKE " + "'%" + szfilter + "%'", "");
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
            string AID = "";
            string BbusinessDate = "";
            double Money = 0.0;
            string FeeType = "";
            string FeeTypeOne = "";
            string CarNO = "";

            for (int i = 0; i < myGridView2.RowCount; i++)
            {

                FeeTypeOne = ConvertType.ToString(myGridView2.GetRowCellValue(i, "FeeType"));
                if ((ConvertType.ToString(myGridView2.GetRowCellValue(i, "BankMan")) == "" ||
                        ConvertType.ToString(myGridView2.GetRowCellValue(i, "BankCode")) == "")
                        && (FeeTypeOne == "�󳵷��ָ�"
                        || FeeTypeOne == "���Ϳ���"))
                    {
                        if (Convert.ToDecimal(myGridView2.GetRowCellValue(i, "money")) <= 0)
                        {
                            MsgBox.ShowOK("����С���㣺" + myGridView2.GetRowCellValue(i, "DepartureBatch"));
                            return;
                        }
                        MsgBox.ShowOK("������Ϣ����Ϊ�գ�" + myGridView2.GetRowCellValue(i, "DepartureBatch"));
                        return;
                    }
                if (ConvertType.ToString(myGridView2.GetRowCellValue(i, "ConfirmState")) == "��ȷ��")
                    {
                        MsgBox.ShowOK("������ȷ�����ݣ���" + myGridView2.GetRowCellValue(i, "DepartureBatch"));
                        return;
                    }

                if (FeeTypeOne=="�󳵷ѻظ�")
                {
                    CarNO+=myGridView2.GetRowCellValue(i, "CarNO") + "@";
                }else
                {
                    CarNO += "xxxxxx@";
                }
                AID += myGridView2.GetRowCellValue(i, "AID") + "@";
                BbusinessDate += myGridView2.GetRowCellValue(i, "BbusinessDate") + "@";
                   
                    FeeType += FeeTypeOne + "@";

            }

            try
            {
                string sumMoney = "";
                List<SqlPara> list4 = new List<SqlPara>();
                list4.Add(new SqlPara("AID", AID));
               // list4.Add(new SqlPara("CarNO", CarNO));
                SqlParasEntity spe4 = new SqlParasEntity(OperType.Query, "QSP_GET_FinancialAuditSumMoney3", list4);
                DataSet ds4 = SqlHelper.GetDataSet(spe4);
                sumMoney = ds4.Tables[0].Rows[0][0].ToString();
                if (MsgBox.ShowYesNo(sumMoney + "���Ƿ�ȷ�ϣ�") != DialogResult.Yes)
                {
                    return;
                }

                string SerialNumber = "";
                List<SqlPara> list2 = new List<SqlPara>();
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_FinancialAuditBatch", list2);
                DataSet ds = SqlHelper.GetDataSet(spe);
                SerialNumber = ds.Tables[0].Rows[0][0].ToString();
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("AID", AID));
                list.Add(new SqlPara("BbusinessDate", ""));
                list.Add(new SqlPara("CarNO", ""));
                list.Add(new SqlPara("DepartureBatch", BbusinessDate));
                list.Add(new SqlPara("money", Money));
                list.Add(new SqlPara("FeeType", FeeType));
                list.Add(new SqlPara("SerialNumber", SerialNumber));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_CHECK_FinancialAudit3", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    //List<SqlPara> list3 = new List<SqlPara>();
                    //list3.Add(new SqlPara("SerialNumber", SerialNumber));
                    //SqlParasEntity spe2 = new SqlParasEntity(OperType.Query, "QSP_GET_FinancialAuditSumMoney", list3);
                    //DataSet ds2 = SqlHelper.GetDataSet(spe2);
                    //MsgBox.ShowOK("����ȷ�ϣ�" + ds2.Tables[0].Rows[0][0].ToString());
                    MsgBox.ShowOK("ȷ�ϳɹ���");
                    ds1.Clear();
                }
            }
            catch (Exception ex)
            {
                //string strEx=ex.ToString();

                //strEx = strEx.Substring(strEx.IndexOf("50000��") + 6, strEx.Length);
                //strEx = strEx.Substring(strEx.IndexOf("50000��") + 6, strEx.IndexOf("\r\n ") - 1);
                //MsgBox.ShowOK(strEx);
                //return;
                MsgBox.ShowException(ex);
            }
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

        private void myGridView1_DoubleClick(object sender, EventArgs e)
        {


            string str = CommonClass.UserInfo.DepartName;

        }

       
    }
}