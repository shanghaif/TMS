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

namespace ZQTMS.UI
{
    public partial class frmReceiptBack : BaseForm
    {
        private DataSet ds_left = new DataSet();
        private DataSet ds_right = new DataSet();

        public frmReceiptBack()
        {
            InitializeComponent();
            barEditItem1.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem1_EditValueChanging);
            barEditItem1.Edit.KeyDown += new KeyEventHandler(barEditItem1_KeyDown);
            barEditItem2.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem2_EditValueChanging);
            barEditItem2.Edit.KeyDown += new KeyEventHandler(barEditItem2_KeyDown);
        }

        private void frmReceiptBack_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, false, myGridView2);
            GridOper.SetGridViewProperty(myGridView1, myGridView2);
            BarMagagerOper.SetBarPropertity(bar1, bar2);
            GridOper.RestoreGridLayout(myGridView1, myGridView2);
            FixColumn fix = new FixColumn(myGridView1, barSubItem3);
            fix = new FixColumn(myGridView2, barSubItem4);

            Operator.Text = CommonClass.UserInfo.UserName;
            OperateTime.EditValue = CommonClass.gcdate;

            string[] OperateStateList = CommonClass.Arg.ReceiptBackState.Split(',');
            if (OperateStateList.Length > 0)
            {
                for (int i = 0; i < OperateStateList.Length; i++)
                {
                    OperateState.Properties.Items.Add(OperateStateList[i]);
                }
                OperateState.SelectedIndex = 0;
            }

            CommonClass.SetSite(repositoryItemComboBox1, true);
            if (CommonClass.UserInfo.SiteName.Equals("�ܲ�"))
            {
                barEditItem3.EditValue = "ȫ��";
                barEditItem4.EditValue = "ȫ��";
            }
            else
            {
                barEditItem3.EditValue = CommonClass.UserInfo.SiteName;
                barEditItem4.EditValue = CommonClass.UserInfo.WebName;
            }
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
                list.Add(new SqlPara("siteName", barEditItem3.EditValue.ToString() == "ȫ��" ? "%%" : barEditItem3.EditValue));
                list.Add(new SqlPara("WebName", barEditItem4.EditValue.ToString() == "ȫ��" ? "%%" : barEditItem4.EditValue));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ReceiptBackList", list);
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
        /// ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (myGridView2.RowCount == 0)
            {
                XtraMessageBox.Show("û�з����κ���Ҫ���ص��嵥�������ڵڢٲ��й����嵥��", "ϵͳ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                list.Add(new SqlPara("ReceiptState", "�ص�����"));
                list.Add(new SqlPara("LinkTel", ""));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_Receipt", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    XtraMessageBox.Show("�ص��ѷ���", "ϵͳ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Dictionary<string, string> dicSyn = new Dictionary<string, string>();
                    dicSyn.Add("OperateState", OperateState.Text.Trim());
                    dicSyn.Add("Operator", Operator.Text.Trim());
                    dicSyn.Add("OperateTime", OperateTime.Text.Trim());
                    dicSyn.Add("allBillNo", allBillNo.Trim());
                    dicSyn.Add("OperateSite", CommonClass.UserInfo.SiteName);
                    dicSyn.Add("OperateWeb", CommonClass.UserInfo.WebName);
                    dicSyn.Add("RecBatch", Guid.NewGuid().ToString());
                    dicSyn.Add("OperateRemark", "");
                    dicSyn.Add("ToSite", "");
                    dicSyn.Add("ToWeb", "");
                    dicSyn.Add("SendNum", "");
                    dicSyn.Add("ReceiptState", "�ص�����");
                    dicSyn.Add("LinkTel", "");
                    dicSyn.Add("MailingType", "");
                    dicSyn.Add("express", "");
                    dicSyn.Add("CourierNumber", "");
                    dicSyn.Add("HDBillNo","");
                    dicSyn.Add("HDPCH","");
                    dicSyn.Add("LoginAreaName", CommonClass.UserInfo.AreaName);
                    dicSyn.Add("LoginCauseName", CommonClass.UserInfo.CauseName);
                    dicSyn.Add("LoginDepartName", CommonClass.UserInfo.DepartName);
                    dicSyn.Add("LoginSiteName", CommonClass.UserInfo.SiteName);
                    dicSyn.Add("LoginWebName", CommonClass.UserInfo.WebName);
                    dicSyn.Add("LoginUserAccount", CommonClass.UserInfo.UserAccount);
                    dicSyn.Add("LoginUserName", CommonClass.UserInfo.UserName);
                    dicSyn.Add("companyid",CommonClass.UserInfo.companyid);

                    CommonSyn.ReceiptSendeSyn(dicSyn);

                    barButtonItem1_ItemClick(null, null);
                }
            }
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

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView2);
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

        private void barEditItem3_EditValueChanged(object sender, EventArgs e)
        {
            repositoryItemComboBox2.Items.Clear();
            CommonClass.SetWeb(repositoryItemComboBox2, barEditItem3.EditValue + "");
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView2, myGridView2.Guid.ToString());
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView2);
        }

        private void barButtonItem16_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView2, myGridView2.Guid.ToString());
        }

        private void barButtonItem18_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem19_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem20_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

    }
}