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
            //if (CommonClass.UserInfo.SiteName.Equals("总部"))
            //{
            //    barEditItem3.EditValue = "全部";
            //    barEditItem4.EditValue = "全部";
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
        BarEditItem barEditCause = new BarEditItem(); //生成事业部
        BarEditItem barEditArea = new BarEditItem(); //生成大区
        BarEditItem barEditWeb = new BarEditItem(); //生成网点

        private void barEditItem1_EditValueChanged(object sender, EventArgs e)
        {
            RepositoryItemComboBox repositoryItemComboBox = (RepositoryItemComboBox)barEditArea.Edit;
            repositoryItemComboBox.Items.Clear();
            CommonClass.SetArea(repositoryItemComboBox, barEditCause.EditValue + "", true);
            barEditArea.EditValue = "全部";
        }
        private void barEditItem1_EditValueChanged_2(object sender, EventArgs e)
        {
            RepositoryItemComboBox repositoryItemComboBox = (RepositoryItemComboBox)barEditWeb.Edit;
            repositoryItemComboBox.Items.Clear();
            CommonClass.SetCauseWeb(repositoryItemComboBox, barEditCause.EditValue + "", barEditArea.EditValue + "");
            barEditWeb.EditValue = "全部";
        }

        /// <summary>
        /// 提取库存
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
                //list.Add(new SqlPara("WebName", barEditItem4.EditValue.ToString() == "全部" ? "%%" : barEditItem4.EditValue));
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
        /// 左边单选到右边
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridViewMove.Move(myGridView1, ds_left, ds_right);
        }

        /// <summary>
        /// 左边全选到右边
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView1.SelectAll();
            GridViewMove.Move(myGridView1, ds_left, ds_right);
        }

        /// <summary>
        /// 左边单选返回右边
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridViewMove.Move(myGridView2, ds_right, ds_left);
        }

        /// <summary>
        /// 左边全部返回到右边
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView2.SelectAll();
            GridViewMove.Move(myGridView2, ds_right, ds_left);
        }

        /// <summary>
        /// 右边文本框改变事件
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
        /// 右边文本框回车事件
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
        /// 左边文本框改变事件
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
        /// 左边文本框回车事件
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
        /// 完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            panelControl5.Visible = true;
            //获取短驳批次号
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

            //填写快递信息
            //panelControl4.Visible = true;
            //edcourierFirm.Focus();
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 右左边双击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myGridView1_DoubleClick(object sender, EventArgs e)
        {
            GridViewMove.Move(myGridView1, ds_left, ds_right);
        }

        /// <summary>
        /// 右边双击
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
            GridOper.ExportToExcel(myGridView2, "回单寄出挑选库存");
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "回单寄出取库存");
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
        /// 打印清单
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
                type = "回单寄出";
            }
            else
            {
                XtraMessageBox.Show("没有可打印的运单信息，请从第①步中选择！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_ReceiptBackBill_KT", new List<SqlPara>() { new SqlPara("rowBillNos", rowBillNo), new SqlPara("type", type) }));
            if (ds == null || ds.Tables.Count == 0) return;
            frmPrintRuiLang fpr = new frmPrintRuiLang("回单交接单.grf", ds);
            fpr.ShowDialog();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            /*
            string courierFirm = edcourierFirm.Text.Trim();
            string trackingNo = edtrackingNo.Text.Trim();
            if (courierFirm == "" && trackingNo != "")
            {
                MsgBox.ShowOK("您填了快递单号,但没有选择快递公司!");
                edcourierFirm.Focus();
                return;
            }
            else if (courierFirm != "" && trackingNo == "")
            {
                MsgBox.ShowOK("您选择了快递公司,但没有填快递单号!");
                edtrackingNo.Focus();
                return;
            }

            if (myGridView2.RowCount == 0)
            {
                XtraMessageBox.Show("没有发现任何需要寄出的清单，请先在第①步中构建清单。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                list.Add(new SqlPara("ReceiptState", "回单寄出"));
                list.Add(new SqlPara("LinkTel", ""));
                list.Add(new SqlPara("courierFirm", courierFirm));
                list.Add(new SqlPara("trackingNo", trackingNo));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_Receipt", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    XtraMessageBox.Show("回单已寄出", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    barButtonItem1_ItemClick(null, null);
                }
            }
            */
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            panelControl5.Visible = false;
        }

        //保存2018.1.3wbw
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            if (myGridView2.RowCount == 0)
            {
                XtraMessageBox.Show("没有发现任何需要寄出的清单，请先在第①步中构建清单。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if (comboBoxEdit1.Text == "快递寄出")
                {
                    if (textEdit1.Text == "" || textEdit2.Text == "")
                    {
                        MsgBox.ShowOK("选择快递寄出时，快递公司和快递单号不能为空！");
                        return;
                    }
                }
                if (comboBoxEdit1.Text == "内部带货")
                {
                    if ( HDBillNo.Text == "")
                    {
                        MsgBox.ShowOK("选择内部带货时，车牌号和司机名字不能为空！");
                        return;
                    }
                }
                if (comboBoxEdit1.Text == "批次号带货")
                {
                    if (HDPCH.Text == "")
                    {
                        MsgBox.ShowOK("选择批次号带货时，批次号不能为空！");
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
                            MsgBox.ShowOK(a+"请先上传回单图片！");
                            return;
                        }
                        if (UpFileDate != "")
                        {
                            if (huidanqueren != "已审核")
                            {
                               // MsgBox.ShowOK("存在未确认的单！请检查！");
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
                list.Add(new SqlPara("ReceiptState", "回单寄出"));
                list.Add(new SqlPara("LinkTel", ""));
                list.Add(new SqlPara("MailingType", comboBoxEdit1.Text.Trim()));
                list.Add(new SqlPara("express", textEdit1.Text.Trim()));
                list.Add(new SqlPara("CourierNumber", textEdit2.Text.Trim()));
                list.Add(new SqlPara("HDBillNo", HDBillNo.Text.Trim()));
                list.Add(new SqlPara("HDPCH", HDPCH.Text.Trim()));

                //List<SqlPara> listSyn = new List<SqlPara>(list.ToArray());
                //List<SqlPara> listSyn = new List<SqlPara>();//同步list 
                //list.ForEach(i=>listSyn.Add(i));
               
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_Receipt_NEW", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    XtraMessageBox.Show("回单已寄出", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    panelControl5.Visible = false;
                    //回单寄出同步  zaj 2018-5-14
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
            if (comboBoxEdit1.Text == "快递寄出")
            {
                textEdit1.Enabled = true;
                textEdit2.Enabled = true;
              
                HDBillNo.Enabled = false;
                HDBillNo.Text = "";
                HDPCH.Text = "";
                HDPCH.Enabled = false;
            }
            if (comboBoxEdit1.Text == "内部带货")
            {
                
                HDBillNo.Enabled = true;
                textEdit1.Enabled = false;
                textEdit2.Enabled = false;
                textEdit1.Text = "";
                textEdit2.Text = "";
                HDPCH.Text = "";
                HDPCH.Enabled = false;
            }
            if (comboBoxEdit1.Text == "批次号带货")
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

        //回单点到
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