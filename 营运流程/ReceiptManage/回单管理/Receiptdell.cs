using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using ZQTMS.Lib;
using DevExpress.XtraEditors;
using Newtonsoft.Json;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Columns;

namespace ZQTMS.UI
{
    public partial class Receiptdell : BaseForm
    {

        private DataSet ds_left = new DataSet();
        private DataSet ds_right = new DataSet();

        GridColumn gcIsseleckedMode;
        public string str = "";
        public Receiptdell()
        {
            InitializeComponent();
            //速配
           // 回单寄出
            barEditItem3.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem3_EditValueChanging);
            barEditItem3.Edit.KeyDown += new KeyEventHandler(barEditItem3_KeyDown);
            barEditItem7.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem7_EditValueChanging);
            barEditItem7.Edit.KeyDown += new KeyEventHandler(barEditItem7_KeyDown);
            //回单返回
            barEditItem21.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem21_EditValueChanging);
            barEditItem21.Edit.KeyDown += new KeyEventHandler(barEditItem21_KeyDown);
            barEditItem23.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem23_EditValueChanging);
            barEditItem23.Edit.KeyDown += new KeyEventHandler(barEditItem23_KeyDown);
            //回单返厂
            barEditItem14.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem14_EditValueChanging);
            barEditItem14.Edit.KeyDown += new KeyEventHandler(barEditItem14_KeyDown);
            barEditItem24.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem24_EditValueChanging);
            barEditItem24.Edit.KeyDown += new KeyEventHandler(barEditItem24_KeyDown);
            //客户取单
            barEditItem18.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem18_EditValueChanging);
            barEditItem18.Edit.KeyDown += new KeyEventHandler(barEditItem18_KeyDown);
            barEditItem19.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem19_EditValueChanging);
            barEditItem19.Edit.KeyDown += new KeyEventHandler(barEditItem19_KeyDown);
        }


        private void Receiptdell_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("回单管理");//xj2019/5/28
            CommonClass.FormSet(this);
            //CommonClass.GetGridViewColumns(myGridView10, false, myGridView11, myGridView1, myGridView4, myGridView7, myGridView8, myGridView5, myGridView9);
            CommonClass.GetGridViewColumns(myGridView2, true);
            CommonClass.GetGridViewColumns(myGridView10, false);
            CommonClass.GetGridViewColumns(myGridView11, false);
            CommonClass.GetGridViewColumns(myGridView1, false);
            CommonClass.GetGridViewColumns(myGridView4, false);
            CommonClass.GetGridViewColumns(myGridView7, false);
            CommonClass.GetGridViewColumns(myGridView8, false);
            CommonClass.GetGridViewColumns(myGridView5, false);
            CommonClass.GetGridViewColumns(myGridView9, false);
            GridOper.SetGridViewProperty(myGridView2, myGridView10, myGridView11, myGridView1, myGridView4, myGridView7, myGridView8, myGridView5, myGridView9);//工具bar
            BarMagagerOper.SetBarPropertity(bar11, bar1, bar10, bar2, bar3, bar7, bar8, bar15, bar17);//工具bar
            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
            GridOper.RestoreGridLayout(myGridView2, myGridView2.Guid.ToString());
            GridOper.RestoreGridLayout(myGridView4, myGridView4.Guid.ToString());
            GridOper.RestoreGridLayout(myGridView5, myGridView5.Guid.ToString());
            GridOper.RestoreGridLayout(myGridView7, myGridView7.Guid.ToString());
            GridOper.RestoreGridLayout(myGridView8, myGridView8.Guid.ToString());
            GridOper.RestoreGridLayout(myGridView9, myGridView9.Guid.ToString());
            GridOper.RestoreGridLayout(myGridView10, myGridView10.Guid.ToString());
            GridOper.RestoreGridLayout(myGridView11, myGridView11.Guid.ToString());
            //冻结列
            FixColumn fix = new FixColumn(myGridView2, barSubItem9);//回单总账
            fix = new FixColumn(myGridView10, barSubItem14);//回单寄出左
            fix = new FixColumn(myGridView11, barSubItem4);//回单寄出右
            fix = new FixColumn(myGridView1, barSubItem7);//回单返回左
            //fix = new FixColumn(myGridView4, barSubItem7);//回单返回左
            fix = new FixColumn(myGridView7, barSubItem18);//回单返厂左
            //fix = new FixColumn(myGridView8, barSubItem7);//回单返厂右
            //HJ20181009
            if (str == "回单寄出")
            {
                xtraTabControl1.SelectedTabPage = xtraTabPage3;
            }
            if (str == "回单返回")
            {
                xtraTabControl1.SelectedTabPage = xtraTabPage1;
            }
            if (str == "回单返厂")
            {
                xtraTabControl1.SelectedTabPage = xtraTabPage4;
            }
            #region  回单总账page
            string[] stateList = CommonClass.Arg.ReceiptSelectState.Split(',');
            if (stateList.Length > 0)
            {
                for (int i = 0; i < stateList.Length; i++)
                {
                    state.Properties.Items.Add(stateList[i]);
                }
                state.SelectedIndex = 0;
            }

            CommonClass.SetSite(comboBoxEdit1, true);
            CommonClass.SetSite(comboBoxEdit2, true);
            bdate.DateTime = CommonClass.gbdate;
            dateEdit1.DateTime = CommonClass.gedate;
            comboBoxEdit1.Text = CommonClass.UserInfo.SiteName;
            comboBoxEdit2.Text = "全部";
            GridOper.CreateStyleFormatCondition(myGridView2, "SendOperateTime", DevExpress.XtraGrid.FormatConditionEnum.NotEqual, null, Color.FromArgb(255, 128, 128));//回单未寄出 红色
            GridOper.CreateStyleFormatCondition(myGridView2, "BackOperateTime", DevExpress.XtraGrid.FormatConditionEnum.NotEqual, null, Color.FromArgb(255, 255, 128));//回单返回 黄色
            GridOper.CreateStyleFormatCondition(myGridView2, "ReceiptState", DevExpress.XtraGrid.FormatConditionEnum.Equal, "回单返厂", Color.FromArgb(128, 255, 128));//回单返厂 绿色
            GridOper.CreateStyleFormatCondition(myGridView2, "ReceiptState", DevExpress.XtraGrid.FormatConditionEnum.Equal, "客户取单", Color.FromArgb(128, 255, 128));//客户取单 绿色
       
            gcIsseleckedMode = GridOper.GetGridViewColumn(myGridView2 ,"ischecked");
            #endregion

            #region 回单寄出page
            Operator.Text = CommonClass.UserInfo.UserName;
            OperateTime.EditValue = CommonClass.gcdate;
            string[] OperateStateList = CommonClass.Arg.OperateState.Split(',');
            if (OperateStateList.Length > 0)
            {
                for (int i = 0; i < OperateStateList.Length; i++)
                {
                    OperateState.Properties.Items.Add(OperateStateList[i]);
                }
                OperateState.SelectedIndex = 0;
            }
            CommonClass.SetSite(repositoryItemComboBox10, true);

            if (CommonClass.UserInfo.SiteName.Equals("总部"))
            {
                barEditItem22.EditValue = "全部";
                barEditItem2.EditValue = "全部";
            }
            else
            {
                barEditItem22.EditValue = CommonClass.UserInfo.SiteName;
                barEditItem2.EditValue = CommonClass.UserInfo.WebName;
            }
            #endregion

            #region   回单返回page
            //回单返回page
            Operator1.Text = CommonClass.UserInfo.UserName;
            OperateTime1.EditValue = CommonClass.gcdate;

            string[] OperateStateList1 = CommonClass.Arg.ReceiptBackState.Split(',');
            if (OperateStateList1.Length > 0)
            {
                for (int i = 0; i < OperateStateList1.Length; i++)
                {
                    OperateState1.Properties.Items.Add(OperateStateList1[i]);
                }
                OperateState1.SelectedIndex = 0;
            }

            CommonClass.SetSite(repositoryItemComboBox3, true);
            if (CommonClass.UserInfo.SiteName.Equals("总部"))
            {
                barEditItem8.EditValue = "全部";
                barEditItem9.EditValue = "全部";
            }
            else
            {
                barEditItem8.EditValue = CommonClass.UserInfo.SiteName;
                barEditItem9.EditValue = CommonClass.UserInfo.WebName;
            }
            #endregion

            #region 回单返厂page
            Operator2.Text = CommonClass.UserInfo.UserName;
            OperateTime2.EditValue = CommonClass.gcdate;

            string[] OperateStateList2 = CommonClass.Arg.ReceiptBackState.Split(',');
            if (OperateStateList2.Length > 0)
            {
                for (int i = 0; i < OperateStateList2.Length; i++)
                {
                    OperateState2.Properties.Items.Add(OperateStateList2[i]);
                }
                OperateState2.SelectedIndex = 0;
            }

            CommonClass.SetSite(repositoryItemComboBox5, true);
            if (CommonClass.UserInfo.SiteName.Equals("总部"))
            {
                barEditItem12.EditValue = "全部";
                barEditItem13.EditValue = "全部";
            }
            else
            {
                barEditItem12.EditValue = CommonClass.UserInfo.SiteName;
                barEditItem13.EditValue = CommonClass.UserInfo.WebName;
            }



            #endregion

            #region 客户取单page
            OperateTime3.EditValue = CommonClass.gcdate;

            string[] OperateStateList3 = CommonClass.Arg.ReceiptBackState.Split(',');
            if (OperateStateList3.Length > 0)
            {
                for (int i = 0; i < OperateStateList3.Length; i++)
                {
                    OperateState3.Properties.Items.Add(OperateStateList3[i]);
                }
                OperateState3.SelectedIndex = 0;
            }

            CommonClass.SetSite(repositoryItemComboBox9, true);
            if (CommonClass.UserInfo.SiteName.Equals("总部"))
            {
                barEditItem20.EditValue = "全部";
                barEditItem17.EditValue = "全部";
            }
            else
            {
                barEditItem20.EditValue = CommonClass.UserInfo.SiteName;
                barEditItem17.EditValue = CommonClass.UserInfo.WebName;
            }

            #endregion

            CommonClass.GetServerDate();

        }  
        


        #region  回单返回page
        //回单返回、完成
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            if (myGridView4.RowCount == 0)
            {
                XtraMessageBox.Show("没有发现任何需要返回的清单，请先在第①步中构建清单。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("OperateState", OperateState1.Text.Trim()));
                list.Add(new SqlPara("Operator", Operator1.Text.Trim()));
                list.Add(new SqlPara("OperateTime", OperateTime1.Text.Trim()));
                string allBillNo = "";
                if (myGridView4.RowCount > 0)
                {
                    for (int i = 0; i < myGridView4.RowCount; i++)
                    {
                        allBillNo += myGridView4.GetRowCellValue(i, "BillNo") + ",";
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
                list.Add(new SqlPara("ReceiptState", "回单返回"));
                list.Add(new SqlPara("LinkTel", ""));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_Receipt_GXSJ", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    XtraMessageBox.Show("回单已返回", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Dictionary<string, string> dicSyn = new Dictionary<string, string>();
                    dicSyn.Add("OperateState", OperateState1.Text.Trim());
                    dicSyn.Add("Operator", Operator1.Text.Trim());
                    dicSyn.Add("OperateTime", OperateTime1.Text.Trim());
                    dicSyn.Add("allBillNo", allBillNo.Trim());
                    dicSyn.Add("OperateSite", CommonClass.UserInfo.SiteName);
                    dicSyn.Add("OperateWeb", CommonClass.UserInfo.WebName);
                    dicSyn.Add("RecBatch", Guid.NewGuid().ToString());
                    dicSyn.Add("OperateRemark", "");
                    dicSyn.Add("ToSite", "");
                    dicSyn.Add("ToWeb", "");
                    dicSyn.Add("SendNum", "");
                    dicSyn.Add("ReceiptState", "回单返回");
                    dicSyn.Add("LinkTel", "");
                    dicSyn.Add("MailingType", "");
                    dicSyn.Add("express", "");
                    dicSyn.Add("CourierNumber", "");
                    dicSyn.Add("HDBillNo", "");
                    dicSyn.Add("HDPCH", "");
                    dicSyn.Add("LoginAreaName", CommonClass.UserInfo.AreaName);
                    dicSyn.Add("LoginCauseName", CommonClass.UserInfo.CauseName);
                    dicSyn.Add("LoginDepartName", CommonClass.UserInfo.DepartName);
                    dicSyn.Add("LoginSiteName", CommonClass.UserInfo.SiteName);
                    dicSyn.Add("LoginWebName", CommonClass.UserInfo.WebName);
                    dicSyn.Add("LoginUserAccount", CommonClass.UserInfo.UserAccount);
                    dicSyn.Add("LoginUserName", CommonClass.UserInfo.UserName);
                    dicSyn.Add("companyid", CommonClass.UserInfo.companyid);

                    CommonSyn.ReceiptSendeSyn(dicSyn);

                    barButtonItem10_ItemClick(null, null);
                }
            }
        }
        //回单返回提取库存
        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                ds_left.Clear();
                ds_right.Clear();
                myGridView1.ClearColumnsFilter();
                myGridView4.ClearColumnsFilter();

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("siteName", barEditItem8.EditValue.ToString() == "全部" ? "%%" : barEditItem8.EditValue));
                list.Add(new SqlPara("WebName", barEditItem9.EditValue.ToString() == "全部" ? "%%" : barEditItem9.EditValue));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ReceiptBackList", list);
                ds_left = SqlHelper.GetDataSet(sps);
                if (ds_left == null || ds_left.Tables.Count == 0) return;
                ds_right = ds_left.Clone();
                myGridControl1.DataSource = ds_left.Tables[0];
                myGridControl4.DataSource = ds_right.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        //回单返回取消
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barEditItem8_EditValueChanged(object sender, EventArgs e)
        {
            repositoryItemComboBox4.Items.Clear();
            CommonClass.SetWeb(repositoryItemComboBox4, barEditItem8.EditValue + "");
        }
        //回单返回过滤器左
        private void barButtonItem21_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }
        //回单返回自动筛选左
        private void barButtonItem18_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }
        //回单返回锁定外观左
        private void barButtonItem19_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }
        //回单返回取消外观左
        private void barButtonItem20_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }
        //回单返回导出左
        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }
        //回单返回左、单个右移
        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridViewMove.Move(myGridView1, ds_left, ds_right);
        }
        //回单返回左、全部右移
        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView1.SelectAll();
            GridViewMove.Move(myGridView1, ds_left, ds_right);
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
        private void myGridView4_DoubleClick(object sender, EventArgs e)
        {
            GridViewMove.Move(myGridView4, ds_right, ds_left);
        }

        /// <summary>
        /// 右边文本框改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barEditItem21_EditValueChanging(object sender, EventArgs e)
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
        private void barEditItem21_KeyDown(object sender, KeyEventArgs e)
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
        private void barEditItem23_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewValue.ToString().Trim()))
            {
                string szfilter = e.NewValue.ToString().Trim();
                if (szfilter != "")
                {
                    myGridView4.ClearColumnsFilter();
                    myGridView4.Columns["BillNo"].FilterInfo = new ColumnFilterInfo("[BillNo] LIKE " + "'%" + szfilter + "%'" + " OR [BillNo] LIKE" + "'%" + szfilter + "%'", "");
                }
                else
                {
                    myGridView4.ClearColumnsFilter();
                }
            }
            else
            {
                myGridView4.ClearColumnsFilter();
            }
        }

        /// <summary>
        /// 左边文本框回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barEditItem23_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || ((DevExpress.XtraEditors.TextEdit)sender).Text == "" || myGridView4.RowCount > 1)
            {
                e.Handled = true;
                return;
            }
            else
            {
                myGridView4.SelectRow(0);
                GridViewMove.Move(myGridView4, ds_right, ds_left);
                ((DevExpress.XtraEditors.TextEdit)sender).Text = "";

                myGridView4.ClearColumnsFilter();
                e.Handled = true;
            }
        }
          /// <summary>
        /// 右边单选返回左边
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridViewMove.Move(myGridView4, ds_right, ds_left);
        }
        /// <summary>
        /// 右边全选返回左边
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem17_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView4.SelectAll();
            GridViewMove.Move(myGridView4, ds_right, ds_left);
        }
        //导出右
        
        private void barButtonItem22_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView4);
        }
        //退出
        private void barButtonItem16_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }
        #endregion


        #region   回单返厂page

        private void barEditItem12_EditValueChanged(object sender, EventArgs e)
        {
            repositoryItemComboBox6.Items.Clear();
            CommonClass.SetWeb(repositoryItemComboBox6, barEditItem12.EditValue + "");
        }

        //提取库存
        private void barButtonItem24_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                ds_left.Clear();
                ds_right.Clear();
                myGridView7.ClearColumnsFilter();
                myGridView8.ClearColumnsFilter();

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("siteName", barEditItem12.EditValue.ToString() == "全部" ? "%%" : barEditItem12.EditValue));
                list.Add(new SqlPara("WebName", barEditItem13.EditValue.ToString() == "全部" ? "%%" : barEditItem13.EditValue));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ReceiptToFactoryList", list);
                ds_left = SqlHelper.GetDataSet(sps);
                if (ds_left == null || ds_left.Tables.Count == 0) return;
                ds_right = ds_left.Clone();
                myGridControl7.DataSource = ds_left.Tables[0];
                myGridControl8.DataSource = ds_right.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //自动筛选
        private void barButtonItem64_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView7);
        }
        //锁定外观
        private void barButtonItem65_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView7, myGridView7.Guid.ToString());
        }
        //取消外观
        private void barButtonItem66_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView7, myGridView7.Guid.ToString());
        }
        //过滤器
        private void barButtonItem67_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView7);
        }
        //导出
        private void barButtonItem26_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView7);
        }

        /// <summary>
        /// 右边文本框改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barEditItem14_EditValueChanging(object sender, EventArgs e)
        {
            string szfilter = ((DevExpress.XtraEditors.TextEdit)sender).Text.ToString().Trim();
            if (!string.IsNullOrEmpty(szfilter))
            {
                if (szfilter != "")
                {
                    myGridView7.ClearColumnsFilter();
                    myGridView7.Columns["BillNo"].FilterInfo = new ColumnFilterInfo("[BillNo] LIKE " + "'%" + szfilter + "%'" + " OR [BillNo] LIKE" + "'%" + szfilter + "%'", "");
                }
                else
                {
                    myGridView7.ClearColumnsFilter();
                }
            }
            else
            {
                myGridView7.ClearColumnsFilter();
            }
        }

        /// <summary>
        /// 右边文本框回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barEditItem14_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || ((DevExpress.XtraEditors.TextEdit)sender).Text == "" || myGridView7.RowCount > 1)
            {
                e.Handled = true;
                return;
            }
            else
            {
                myGridView7.SelectRow(0);
                GridViewMove.Move(myGridView7, ds_left, ds_right);
                ((DevExpress.XtraEditors.TextEdit)sender).Text = "";

                myGridView7.ClearColumnsFilter();
                e.Handled = true;
            }
        }

        /// <summary>
        /// 左边文本框改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barEditItem24_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewValue.ToString().Trim()))
            {
                string szfilter = e.NewValue.ToString().Trim();
                if (szfilter != "")
                {
                    myGridView8.ClearColumnsFilter();
                    myGridView8.Columns["BillNo"].FilterInfo = new ColumnFilterInfo("[BillNo] LIKE " + "'%" + szfilter + "%'" + " OR [BillNo] LIKE" + "'%" + szfilter + "%'", "");
                }
                else
                {
                    myGridView8.ClearColumnsFilter();
                }
            }
            else
            {
                myGridView8.ClearColumnsFilter();
            }
        }

        /// <summary>
        /// 左边文本框回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barEditItem24_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || ((DevExpress.XtraEditors.TextEdit)sender).Text == "" || myGridView8.RowCount > 1)
            {
                e.Handled = true;
                return;
            }
            else
            {
                myGridView8.SelectRow(0);
                GridViewMove.Move(myGridView8, ds_right, ds_left);
                ((DevExpress.XtraEditors.TextEdit)sender).Text = "";

                myGridView8.ClearColumnsFilter();
                e.Handled = true;
            }
        }
        //左边单选到右边
        private void barButtonItem27_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridViewMove.Move(myGridView7, ds_left, ds_right);
        }
        //左边全选到右边
        private void barButtonItem28_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView7.SelectAll();
            GridViewMove.Move(myGridView7, ds_left, ds_right);
        }
        //右边单选到左边
        private void barButtonItem60_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridViewMove.Move(myGridView8, ds_right, ds_left);
        }
        //右边全选到左边
        private void barButtonItem61_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView8.SelectAll();
            GridViewMove.Move(myGridView8, ds_right , ds_left);
        }
        //导出
        private void barButtonItem62_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView8);
        }
        //取消
        private void barButtonItem63_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }
        //退出
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 右左边双击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myGridView7_DoubleClick(object sender, EventArgs e)
        {
            GridViewMove.Move(myGridView7, ds_left, ds_right);
        }

        /// <summary>
        /// 右边双击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myGridView8_DoubleClick(object sender, EventArgs e)
        {
            GridViewMove.Move(myGridView8, ds_right, ds_left);
        }
        
        //完成
        private void btok2_Click(object sender, EventArgs e)
        {
            if (myGridView8.RowCount == 0)
            {
                XtraMessageBox.Show("没有发现任何需要返厂的清单，请先在第①步中构建清单。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("OperateState", OperateState2.Text.Trim()));
                list.Add(new SqlPara("Operator", Operator2.Text.Trim()));
                list.Add(new SqlPara("OperateTime", OperateTime2.Text.Trim()));
                string allBillNo = "";
                if (myGridView8.RowCount > 0)
                {
                    for (int i = 0; i < myGridView8.RowCount; i++)
                    {
                        allBillNo += myGridView8.GetRowCellValue(i, "BillNo") + ",";
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
                list.Add(new SqlPara("ReceiptState", "回单返厂"));
                list.Add(new SqlPara("LinkTel", ""));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_Receipt_GXSJ", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    XtraMessageBox.Show("回单已返厂", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Dictionary<string, string> dicSyn = new Dictionary<string, string>();
                    dicSyn.Add("OperateState", OperateState2.Text.Trim());
                    dicSyn.Add("Operator", Operator2.Text.Trim());
                    dicSyn.Add("OperateTime", OperateTime2.Text.Trim());
                    dicSyn.Add("allBillNo", allBillNo.Trim());
                    dicSyn.Add("OperateSite", CommonClass.UserInfo.SiteName);
                    dicSyn.Add("OperateWeb", CommonClass.UserInfo.WebName);
                    dicSyn.Add("RecBatch", Guid.NewGuid().ToString());
                    dicSyn.Add("OperateRemark", "");
                    dicSyn.Add("ToSite", "");
                    dicSyn.Add("ToWeb", "");
                    dicSyn.Add("SendNum", "");
                    dicSyn.Add("ReceiptState", "回单返厂");
                    dicSyn.Add("LinkTel", "");
                    dicSyn.Add("MailingType", "");
                    dicSyn.Add("express", "");
                    dicSyn.Add("CourierNumber", "");
                    dicSyn.Add("HDBillNo", "");
                    dicSyn.Add("HDPCH", "");
                    dicSyn.Add("LoginAreaName", CommonClass.UserInfo.AreaName);
                    dicSyn.Add("LoginCauseName", CommonClass.UserInfo.CauseName);
                    dicSyn.Add("LoginDepartName", CommonClass.UserInfo.DepartName);
                    dicSyn.Add("LoginSiteName", CommonClass.UserInfo.SiteName);
                    dicSyn.Add("LoginWebName", CommonClass.UserInfo.WebName);
                    dicSyn.Add("LoginUserAccount", CommonClass.UserInfo.UserAccount);
                    dicSyn.Add("LoginUserName", CommonClass.UserInfo.UserName);
                    dicSyn.Add("companyid", CommonClass.UserInfo.companyid);
                    CommonSyn.ReceiptSendeSyn(dicSyn);
                    barButtonItem24_ItemClick(null, null);
                }
            }
        }
        #endregion


        #region 客户取单page

        private void barEditItem20_EditValueChanged(object sender, EventArgs e)
        {
            repositoryItemComboBox8.Items.Clear();
            CommonClass.SetWeb(repositoryItemComboBox8, barEditItem20.EditValue + "");
        }
        //提取库存
        private void barButtonItem68_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                ds_left.Clear();
                ds_right.Clear();
                myGridView5.ClearColumnsFilter();
                myGridView9.ClearColumnsFilter();

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("siteName", barEditItem20.EditValue));
                list.Add(new SqlPara("WebName", barEditItem17.EditValue));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ReceiptCustGetList", list);
                ds_left = SqlHelper.GetDataSet(sps);
                if (ds_left == null || ds_left.Tables.Count == 0) return;
                ds_right = ds_left.Clone();
                myGridControl5.DataSource = ds_left.Tables[0];
                myGridControl9.DataSource = ds_right.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            } 

        }
        //导出
        private void barButtonItem11_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView5);
        }
        /// <summary>
        /// 右边文本框改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barEditItem18_EditValueChanging(object sender, EventArgs e)
        {
            string szfilter = ((DevExpress.XtraEditors.TextEdit)sender).Text.ToString().Trim();
            if (!string.IsNullOrEmpty(szfilter))
            {
                if (szfilter != "")
                {
                    myGridView5.ClearColumnsFilter();
                    myGridView5.Columns["BillNo"].FilterInfo = new ColumnFilterInfo("[BillNo] LIKE " + "'%" + szfilter + "%'" + " OR [BillNo] LIKE" + "'%" + szfilter + "%'", "");
                }
                else
                {
                    myGridView5.ClearColumnsFilter();
                }
            }
            else
            {
                myGridView5.ClearColumnsFilter();
            }
        }

        /// <summary>
        /// 右边文本框回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barEditItem18_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || ((DevExpress.XtraEditors.TextEdit)sender).Text == "" || myGridView5.RowCount > 1)
            {
                e.Handled = true;
                return;
            }
            else
            {
                myGridView5.SelectRow(0);
                GridViewMove.Move(myGridView5, ds_left, ds_right);
                ((DevExpress.XtraEditors.TextEdit)sender).Text = "";

                myGridView5.ClearColumnsFilter();
                e.Handled = true;
            }
        }

        /// <summary>
        /// 左边文本框改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barEditItem19_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewValue.ToString().Trim()))
            {
                string szfilter = e.NewValue.ToString().Trim();
                if (szfilter != "")
                {
                    myGridView9.ClearColumnsFilter();
                    myGridView9.Columns["BillNo"].FilterInfo = new ColumnFilterInfo("[BillNo] LIKE " + "'%" + szfilter + "%'" + " OR [BillNo] LIKE" + "'%" + szfilter + "%'", "");
                }
                else
                {
                    myGridView9.ClearColumnsFilter();
                }
            }
            else
            {
                myGridView9.ClearColumnsFilter();
            }
        }

        /// <summary>
        /// 左边文本框回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barEditItem19_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || ((DevExpress.XtraEditors.TextEdit)sender).Text == "" || myGridView9.RowCount > 1)
            {
                e.Handled = true;
                return;
            }
            else
            {
                myGridView9.SelectRow(0);
                GridViewMove.Move(myGridView9, ds_right, ds_left);
                ((DevExpress.XtraEditors.TextEdit)sender).Text = "";

                myGridView9.ClearColumnsFilter();
                e.Handled = true;
            }
        }
        //左边单选到右边
        private void barButtonItem70_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridViewMove.Move(myGridView5, ds_left, ds_right);
        }
        //左边全选到右边
        private void barButtonItem71_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView5.SelectAll();
            GridViewMove.Move(myGridView5, ds_left, ds_right);
        }
        //右边单选到左边
        private void barButtonItem72_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridViewMove.Move(myGridView9, ds_right, ds_left);
        }
        //右边全选到左边
        private void barButtonItem73_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView9.SelectAll();
            GridViewMove.Move(myGridView9, ds_right, ds_left);
        }
        //右边导出
        private void barButtonItem74_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView9);
        }
        //退出
        private void barButtonItem75_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 右左边双击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myGridView5_DoubleClick(object sender, EventArgs e)
        {
            GridViewMove.Move(myGridView5, ds_left, ds_right);
        }

        /// <summary>
        /// 右边双击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myGridView9_DoubleClick(object sender, EventArgs e)
        {
            GridViewMove.Move(myGridView9, ds_right, ds_left);
        }
        //完成
        private void simpleButton7_Click_1(object sender, EventArgs e)
        {
            if (myGridView9.RowCount == 0)
            {
                XtraMessageBox.Show("没有发现任何需要签收的清单，请先在第①步中构建清单。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("OperateState", OperateState3.Text.Trim()));
                list.Add(new SqlPara("Operator", Operator3.Text.Trim()));
                list.Add(new SqlPara("OperateTime", OperateTime3.Text.Trim()));
                string allBillNo = "";
                if (myGridView9.RowCount > 0)
                {
                    for (int i = 0; i < myGridView9.RowCount; i++)
                    {
                        allBillNo += myGridView9.GetRowCellValue(i, "BillNo") + ",";
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
                list.Add(new SqlPara("ReceiptState", "客户取单"));
                list.Add(new SqlPara("LinkTel", LinkTel.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_Receipt_GXSJ", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    XtraMessageBox.Show("回单已返厂", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Dictionary<string, string> dicSyn = new Dictionary<string, string>();
                    dicSyn.Add("OperateState", OperateState3.Text.Trim());
                    dicSyn.Add("Operator", Operator3.Text.Trim());
                    dicSyn.Add("OperateTime", OperateTime3.Text.Trim());
                    dicSyn.Add("allBillNo", allBillNo.Trim());
                    dicSyn.Add("OperateSite", CommonClass.UserInfo.SiteName);
                    dicSyn.Add("OperateWeb", CommonClass.UserInfo.WebName);
                    dicSyn.Add("RecBatch", Guid.NewGuid().ToString());
                    dicSyn.Add("OperateRemark", "");
                    dicSyn.Add("ToSite", "");
                    dicSyn.Add("ToWeb", "");
                    dicSyn.Add("SendNum", "");
                    dicSyn.Add("ReceiptState", "客户取单");
                    dicSyn.Add("LinkTel", "");
                    dicSyn.Add("MailingType", "");
                    dicSyn.Add("express", "");
                    dicSyn.Add("CourierNumber", "");
                    dicSyn.Add("HDBillNo", "");
                    dicSyn.Add("HDPCH", "");
                    dicSyn.Add("LoginAreaName", CommonClass.UserInfo.AreaName);
                    dicSyn.Add("LoginCauseName", CommonClass.UserInfo.CauseName);
                    dicSyn.Add("LoginDepartName", CommonClass.UserInfo.DepartName);
                    dicSyn.Add("LoginSiteName", CommonClass.UserInfo.SiteName);
                    dicSyn.Add("LoginWebName", CommonClass.UserInfo.WebName);
                    dicSyn.Add("LoginUserAccount", CommonClass.UserInfo.UserAccount);
                    dicSyn.Add("LoginUserName", CommonClass.UserInfo.UserName);
                    dicSyn.Add("companyid", CommonClass.UserInfo.companyid);
                    CommonSyn.ReceiptSendeSyn(dicSyn);

                  
                }


            }

        }
        //客户取单  自动筛选左
        private void barButtonItem76_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView5);
        }
        //锁定外观左
        private void barButtonItem77_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView5);
        }
        //取消外观左
        private void barButtonItem78_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView5.Guid.ToString());
        }
        //过滤器左
        private void barButtonItem79_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView5);
        }
        //导出左
        private void barButtonItem69_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView5);
        }

        
        //取消
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

     #endregion


        #region 回单寄出page
        /// <summary>
        /// 右边文本框改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barEditItem3_EditValueChanging(object sender, EventArgs e)
        {
            string szfilter = ((DevExpress.XtraEditors.TextEdit)sender).Text.ToString().Trim();
            if (!string.IsNullOrEmpty(szfilter))
            {
                if (szfilter != "")
                {
                    myGridView10.ClearColumnsFilter();
                    myGridView10.Columns["BillNo"].FilterInfo = new ColumnFilterInfo("[BillNo] LIKE " + "'%" + szfilter + "%'" + " OR [BillNo] LIKE" + "'%" + szfilter + "%'", "");
                }
                else
                {
                    myGridView10.ClearColumnsFilter();
                }
            }
            else
            {
                myGridView10.ClearColumnsFilter();
            }
        }

        /// <summary>
        /// 右边文本框回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barEditItem3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || ((DevExpress.XtraEditors.TextEdit)sender).Text == "" || myGridView10.RowCount > 1)
            {
                e.Handled = true;
                return;
            }
            else
            {
                myGridView10.SelectRow(0);
                GridViewMove.Move(myGridView10, ds_left, ds_right);
                ((DevExpress.XtraEditors.TextEdit)sender).Text = "";

                myGridView10.ClearColumnsFilter();
                e.Handled = true;
            }
        }

        /// <summary>
        /// 左边文本框改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barEditItem7_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewValue.ToString().Trim()))
            {
                string szfilter = e.NewValue.ToString().Trim();
                if (szfilter != "")
                {
                    myGridView11.ClearColumnsFilter();
                    myGridView11.Columns["BillNo"].FilterInfo = new ColumnFilterInfo("[BillNo] LIKE " + "'%" + szfilter + "%'" + " OR [BillNo] LIKE" + "'%" + szfilter + "%'", "");
                }
                else
                {
                    myGridView11.ClearColumnsFilter();
                }
            }
            else
            {
                myGridView11.ClearColumnsFilter();
            }
        }

        /// <summary>
        /// 左边文本框回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barEditItem7_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || ((DevExpress.XtraEditors.TextEdit)sender).Text == "" || myGridView11.RowCount > 1)
            {
                e.Handled = true;
                return;
            }
            else
            {
                myGridView11.SelectRow(0);
                GridViewMove.Move(myGridView11, ds_right, ds_left);
                ((DevExpress.XtraEditors.TextEdit)sender).Text = "";

                myGridView11.ClearColumnsFilter();
                e.Handled = true;
            }
        }

        private void barEditItem22_EditValueChanged(object sender, EventArgs e)
        {
            repositoryItemComboBox2.Items.Clear();
            CommonClass.SetWeb(repositoryItemComboBox2, barEditItem22.EditValue + "");
        }

        //回单寄出提取库存
        private void barButtonItem40_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            myGridView10.PostEditor();
            myGridView11.PostEditor();
            try
            {
                ds_left.Clear();
                ds_right.Clear();
                myGridView10.ClearColumnsFilter();
                myGridView11.ClearColumnsFilter();

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("siteName", barEditItem22.EditValue.ToString() == "全部" ? "%%" : barEditItem22.EditValue));
                list.Add(new SqlPara("WebName", barEditItem2.EditValue.ToString() == "全部" ? "%%" : barEditItem2.EditValue));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ReceiptSendList_GXSJ", list);//tuxin20181016
                ds_left = SqlHelper.GetDataSet(sps);
                if (ds_left == null || ds_left.Tables.Count == 0) return;
                ds_right = ds_left.Clone();
                myGridControl10.DataSource = ds_left.Tables[0];
                myGridControl11.DataSource = ds_right.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        
        //回单寄出导出左
        private void barButtonItem41_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView10, "回单寄出取库存");
        }
       
        //回单寄出自动筛选左
        private void barButtonItem45_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView10);
        }
       
        //回单寄出锁定外观左
        private void barButtonItem46_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView10, myGridView10.Guid.ToString());
        }
        //回单寄出取消外观左
        private void barButtonItem47_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView10, myGridView10.Guid.ToString());
        }
        private void barButtonItem48_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView10);
        }

        
        /// <summary>
        /// 左边全选到右边
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem43_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView10.SelectAll();
            GridViewMove.Move(myGridView10, ds_left, ds_right);
        }
        /// <summary>
        /// 左边单选到右边
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void barButtonItem42_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridViewMove.Move(myGridView10, ds_left, ds_right);
        }
      
        /// <summary>
        /// 右左边双击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myGridView10_DoubleClick(object sender, EventArgs e)
        {
            GridViewMove.Move(myGridView10, ds_left, ds_right);
        }

        /// <summary>
        /// 右边双击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myGridView11_DoubleClick(object sender, EventArgs e)
        {
            GridViewMove.Move(myGridView11, ds_right, ds_left);
        }

        //回单寄出导出右
        private void barButtonItem23_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView11, "回单寄出挑选库存");
        }

        

        //回单寄出退出右
        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }
        public string allBillNo = "";
        private void btnOK_Click_1(object sender, EventArgs e)
        {
            if (myGridView11.RowCount == 0)
            {
                XtraMessageBox.Show("没有发现任何需要寄出的清单，请先在第①步中构建清单。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
              
                StringBuilder sb = new StringBuilder();
                if (myGridView11.RowCount > 0)
                {
                    for (int i = 0; i < myGridView11.RowCount; i++)
                    {
                        string billno = myGridView11.GetRowCellValue(i, "BillNo").ToString();
                        allBillNo += billno + ",";
                        string state = myGridView11.GetRowCellValue(i, "BillState").ToString();
                        if (state != "16")
                        {
                            sb.Append(billno.Substring(billno.Length - 4, 4) + ",");
                        }
                    }
                }
                if (!string.IsNullOrEmpty(sb.ToString()))
                {
                    if (MsgBox.ShowYesNo("以下运单(后四位)还未签收，是否继续做回单寄出？\n" + sb.ToString()) == DialogResult.No)
                    {
                        return;
                    }
                }
                //获取短驳批次号
                try
                {
                    HDPCH.Properties.Items.Clear();
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
                panelControl7.Visible = true;

            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }



        //回单寄出自动筛选右
        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView11);
        }
        //回单寄出锁定外观右
        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView11, myGridView11.Guid.ToString());
        }
       
        //回单寄出取消外观右
        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView11, myGridView11.Guid.ToString());
        }

        //回单寄出自动筛选右
        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView11);
        }





        /// <summary>
        /// 右边单选返回左边
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void barButtonItem7_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            GridViewMove.Move(myGridView11, ds_right, ds_left);
        }
     
        /// <summary>
        /// 左边单选返回右边
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        /// <summary>
        /// 右边全部返回到左边
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void barButtonItem8_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            myGridView11.SelectAll();
            GridViewMove.Move(myGridView11, ds_right, ds_left);
        }
        #endregion

        #region  回单总账page
        //回单总账自动筛选
        private void barButtonItem29_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView2);
        }
        //回单总账锁定外观
        private void barButtonItem30_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView2);
        }
        //回单总账取消外观
        private void barButtonItem31_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView2.Guid.ToString());
        }
        //回单总账过滤器
        private void barButtonItem32_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView2);
        }
        //回单总账回单上传
        private void barButtonItem33_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            fmFileUploadClone fc = new fmFileUploadClone();
            fc.ShowDialog();
        }
        //hh 2019/1/2  回单总账原单上传
        private void barButtonItem82_ItemClick(object sender, ItemClickEventArgs e)
        {
            fmFileUploadYDSC fu = new fmFileUploadYDSC();
            fu.ShowDialog();
        }
        //回单总账回单确认
        private void barButtonItem34_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmHuiDanQueRen HDQR = new frmHuiDanQueRen();
            HDQR.ShowDialog();
        }
        //回单总账取消寄出
        private void barButtonItem35_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //frmReceiptCancel frc = new frmReceiptCancel();
            //frc.MenuType = "CancelSend";
            //frc.ShowDialog();
            Cancel("CancelSend");

        }
        //回单总账取消返回
        private void barButtonItem36_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //frmReceiptCancel frc = new frmReceiptCancel();
            //frc.MenuType = "CancelBack";
            //frc.ShowDialog();
            Cancel("CancelBack");
        }
        //回单总账取消返厂
        private void barButtonItem37_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //frmReceiptCancel frc = new frmReceiptCancel();
            //frc.MenuType = "CancelToFactory";
            //frc.ShowDialog();
            Cancel("CancelToFactory");
        }
        private void Cancel(string MenuType)
        {
            myGridView2.PostEditor();
            int num = 0;
            string billnos ="";
            if (MsgBox.ShowYesNo("是否取消") != DialogResult.Yes)
            {
                return;
            }
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                if (ConvertType.ToInt32(myGridView2.GetRowCellValue(i, "ischecked")) == 1)
                {
                    billnos += ConvertType.ToString(myGridView2.GetRowCellValue(i, "BillNo")) + "@";
                    if (MenuType == "CancelSend")
                    {
                        if (ConvertType.ToString(myGridView2.GetRowCellValue(i, "SendOperator")) == "")
                        {
                            MsgBox.ShowOK("第" + (i + 1) + "行未做回单寄出不能取消寄出，请检查后重试");
                            return;
                        }
                        if (ConvertType.ToString(myGridView2.GetRowCellValue(i, "ReceiptState")) != "回单寄出" || ConvertType.ToString(myGridView2.GetRowCellValue(i, "huidanqueren")) != "")
                        {
                            MsgBox.ShowOK("第" + (i + 1) + "行已做回单寄出之外的其他操作或已做回单确认，不能取消寄出，请检查后重试");
                            return;
                        }
                        if (ConvertType.ToString(myGridView2.GetRowCellValue(i, "SendOperator")) != CommonClass.UserInfo.UserName)
                        {
                            if (CommonClass.UserInfo.UserName != "高小玲" && CommonClass.UserInfo.UserName != "马先梅" && CommonClass.UserInfo.UserName != "卢洁敏")
                            {
                                MsgBox.ShowOK("第" + (i + 1) + "行不是当前账号操作的回单寄出，不能取消寄出，请检查后重试");
                                return;
                            }
                        }
                    }
                    if (MenuType == "CancelBack")
                    {
                        if (ConvertType.ToString(myGridView2.GetRowCellValue(i, "BackOperator")) == "")
                        {
                            MsgBox.ShowOK("第" + (i + 1) + "行未做回单返回不能取消返回，请检查后重试");
                            return;
                        }
                        if (ConvertType.ToString(myGridView2.GetRowCellValue(i, "ReceiptState")) != "回单返回" || ConvertType.ToString(myGridView2.GetRowCellValue(i, "huidanqueren")) != "")
                        {
                            MsgBox.ShowOK("第" + (i + 1) + "行已做回单返回之外的其他操作或已做回单确认，不能取消返回，请检查后重试");
                            return;
                        }
                        if (ConvertType.ToString(myGridView2.GetRowCellValue(i, "BackOperator")) != CommonClass.UserInfo.UserName)
                        {
                            if (CommonClass.UserInfo.UserName != "高小玲" && CommonClass.UserInfo.UserName != "马先梅" && CommonClass.UserInfo.UserName != "卢洁敏")
                            {
                                MsgBox.ShowOK("第" + (i + 1) + "行不是当前账号操作的回单返回，不能取消返回，请检查后重试");
                                return;
                            }
                        }
                    }
                    if (MenuType == "CancelToFactory")
                    {
                        if (ConvertType.ToString(myGridView2.GetRowCellValue(i, "ToFactoryOperator")) == "")
                        {
                            MsgBox.ShowOK("第" + (i + 1) + "行未做回单返厂不能取消返厂，请检查后重试");
                            return;
                        }
                        if (ConvertType.ToString(myGridView2.GetRowCellValue(i, "ReceiptState")) != "回单返厂" || ConvertType.ToString(myGridView2.GetRowCellValue(i, "huidanqueren")) != "")
                        {
                            MsgBox.ShowOK("第" + (i + 1) + "行已做回单返厂之外的其他操作或已做回单确认，不能取消返厂，请检查后重试");
                            return;
                        }
                        if (ConvertType.ToString(myGridView2.GetRowCellValue(i, "ToFactoryOperator")) != CommonClass.UserInfo.UserName)
                        {
                            if (CommonClass.UserInfo.UserName != "高小玲" && CommonClass.UserInfo.UserName != "马先梅" && CommonClass.UserInfo.UserName != "卢洁敏")
                            {
                                MsgBox.ShowOK("第" + (i + 1) + "行不是当前账号操作的回单返厂，不能取消返厂，请检查后重试");
                                return;
                            }
                        }
                    }
                    if (MenuType == "CancelToCust")
                    {
                        //if (ConvertType.ToString(myGridView2.GetRowCellValue(i, "ToFactoryOperator")) == "")
                        //{
                        //    MsgBox.ShowOK("第" + (i + 1) + "行未做客户取单不能取消取单，请检查后重试");
                        //    return;
                        //}
                        if (ConvertType.ToString(myGridView2.GetRowCellValue(i, "ReceiptState")) != "客户取单" || ConvertType.ToString(myGridView2.GetRowCellValue(i, "huidanqueren")) != "")
                        {
                            MsgBox.ShowOK("第" + (i + 1) + "行已做回单返厂之外的其他操作或已做回单确认，不能取消客户取单，请检查后重试");
                            return;
                        }
                        //if (ConvertType.ToString(myGridView2.GetRowCellValue(i, "ToFactoryOperator")) != CommonClass.UserInfo.UserName)
                        //{
                        //    if (CommonClass.UserInfo.UserName != "高小玲" && CommonClass.UserInfo.UserName != "马先梅" && CommonClass.UserInfo.UserName != "卢洁敏")
                        //    {
                        //        MsgBox.ShowOK("第" + (i + 1) + "行不是当前账号操作的客户取单，不能取消客户取单，请检查后重试");
                        //        return;
                        //    }
                        //}
                    }

                    num++;
                }
            }
            if (num == 0) return;
            string sShowOK = "取消条数：" + num
                     + "\r\n是否继续？";
            if (MsgBox.ShowYesNo(sShowOK) != DialogResult.Yes)
            {
                num = 0;
                return;
            }
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillNos", billnos));
            list.Add(new SqlPara("OperationType", MenuType));

            SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_cancel_receipt_PL", list);
            if (SqlHelper.ExecteNonQuery(sps) > 0)
            {
                XtraMessageBox.Show("取消成功", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //取消回单同步 zaj 2018-5-16
              
                string[]  str = billnos.Split('@');
                for (int i = 0; i < str.Length; i++)
                {
                    Dictionary<string, string> dicSyn = new Dictionary<string, string>();
                    dicSyn.Add("BillNo", str[i]);
                    dicSyn.Add("OperationType", MenuType);
                    dicSyn.Add("LoginAreaName", CommonClass.UserInfo.AreaName);
                    dicSyn.Add("LoginCauseName", CommonClass.UserInfo.CauseName);
                    dicSyn.Add("LoginDepartName", CommonClass.UserInfo.DepartName);
                    dicSyn.Add("LoginSiteName", CommonClass.UserInfo.SiteName);
                    dicSyn.Add("LoginWebName", CommonClass.UserInfo.WebName);
                    dicSyn.Add("LoginUserAccount", CommonClass.UserInfo.UserAccount);
                    dicSyn.Add("LoginUserName", CommonClass.UserInfo.UserName);
                    dicSyn.Add("companyid", CommonClass.UserInfo.companyid);
                    CommonSyn.ReceiptCancelSyn(dicSyn);//回单寄出同步  2018-5-15
                }
              
            }
        
        
        }
        //回单总账导出
        private void barButtonItem38_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView2, "回单总账");
        }
        //回单总账退出
        private void barButtonItem39_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close(); 
        }
        //回单总账提取
        private void btnRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SelectType", state.Text));
                list.Add(new SqlPara("dFrom", bdate.DateTime));
                list.Add(new SqlPara("dTo", dateEdit1.DateTime));
                list.Add(new SqlPara("FromSite", comboBoxEdit1.Text));
                list.Add(new SqlPara("ToSite", comboBoxEdit2.Text));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_Receipt_Select", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                //判断回单确认是否已审核
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string huidanqueren = ds.Tables[0].Rows[i]["huidanqueren"].ToString();
                    if (huidanqueren == "")
                    {
                        ds.Tables[0].Rows[i]["huidanqueren"] = "未审核";
                    }
                }

                myGridControl2.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                 //if (myGridView2.RowCount < 1000) myGridView1.BestFitColumns();
                GridOper.RestoreGridLayout(myGridView2);
            }
        }
        //退出
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void repositoryItemHyperLinkEdit1_Click_1(object sender, EventArgs e)
        {
            int rowhandle = myGridView2.FocusedRowHandle;
            string BillNo = GridOper.GetRowCellValueString(myGridView2, rowhandle, "BillNo");
            fmFileShow fm = new fmFileShow();
            fm.billNo = BillNo;
            fm.billType = 7;
            fm.ShowDialog();
        }
        //全选
        private void chkALL_CheckedChanged(object sender, EventArgs e)
        {
            int a = chkALL.Checked == true ? 1 : 0;
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                myGridView2.SetRowCellValue(i, gcIsseleckedMode, a);
            }
        }
        //反选
        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                if (ConvertType.ToInt32(myGridView2.GetRowCellValue(i, "ischecked")) == 1)
                    myGridView2.SetRowCellValue(i, gcIsseleckedMode, 0);
                else 
                { 
                    myGridView2.SetRowCellValue(i, gcIsseleckedMode, 1);
                }
            }
        }
        #endregion

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            if (comboBoxEdit3.Text == "快递寄出")
            {
                if (textEdit1.Text == "" || textEdit2.Text == "")
                {
                    MsgBox.ShowOK("选择快递寄出时，快递公司和快递单号不能为空！");
                    return;
                }
            }
            if (comboBoxEdit3.Text == "内部带货")
            {
                if (HDBillNo.Text == "")
                {
                    MsgBox.ShowOK("选择内部带货时，车牌号和司机名字不能为空！");
                    return;
                }
            }
            if (comboBoxEdit3.Text == "批次号带货")
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
            //yzw 2018/11/8
            list.Add(new SqlPara("MailingType", comboBoxEdit3.Text.Trim()));
            list.Add(new SqlPara("express", textEdit1.Text.Trim()));
            list.Add(new SqlPara("CourierNumber", textEdit2.Text.Trim()));
            list.Add(new SqlPara("HDBillNo", HDBillNo.Text.Trim()));
            list.Add(new SqlPara("HDPCH", HDPCH.Text.Trim()));
            SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_Receipt_GXSJ", list);
            if (SqlHelper.ExecteNonQuery(sps) > 0)
            {
                //2018.12.5wbw
                List<SqlPara> list7 = new List<SqlPara>();
                list7.Add(new SqlPara("allBillNo", allBillNo.Trim()));
                list7.Add(new SqlPara("express", textEdit1.Text.Trim()));
                list7.Add(new SqlPara("CourierNumber", textEdit2.Text.Trim()));
                list7.Add(new SqlPara("HDBillNo", HDBillNo.Text.Trim()));
                list7.Add(new SqlPara("HDPCH", HDPCH.Text.Trim()));
                list7.Add(new SqlPara("MailingType", comboBoxEdit3.Text.Trim()));
                list7.Add(new SqlPara("ReceiptState", "回单寄出"));
                list7.Add(new SqlPara("OperateState", OperateState.Text.Trim()));
                list7.Add(new SqlPara("RecBatch", Guid.NewGuid().ToString()));

                CommonSyn.LMSDepartureSysZQTMS(list, 2, "USP_UPDATE_ReturnStock_LMSJC", "", "", CommonClass.UserInfo.companyid);//LMS寄出同步ZQTMS 


                XtraMessageBox.Show("回单已寄出", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                panelControl7.Visible = false;
                textEdit1.Text = "";
                textEdit2.Text = "";
                HDPCH.Text = "";
                HDBillNo.Text = "";
                barButtonItem40_ItemClick_1(null, null);
            }
        }

        private void comboBoxEdit3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxEdit3.Text == "快递寄出")
            {
                textEdit1.Enabled = true;
                textEdit2.Enabled = true;

                HDBillNo.Enabled = false;
                HDBillNo.Text = "";
                HDPCH.Text = "";
                HDPCH.Enabled = false;
            }
            if (comboBoxEdit3.Text == "内部带货")
            {

                HDBillNo.Enabled = true;
                textEdit1.Enabled = false;
                textEdit2.Enabled = false;
                textEdit1.Text = "";
                textEdit2.Text = "";
                HDPCH.Text = "";
                HDPCH.Enabled = false;
            }
            if (comboBoxEdit3.Text == "批次号带货")
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

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            panelControl7.Visible = false;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string rowBillNo = "", type = "";
            if (myGridView11.RowCount > 0)
            {
                DataTable dt = myGridControl11.DataSource as DataTable;
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

        private void myGridControl2_Click(object sender, EventArgs e)
        {

        }

        private void barButtonItem94_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView4);
        }

        private void barButtonItem95_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView4, myGridView4.Guid.ToString());
        }

        private void barButtonItem96_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView4, myGridView4.Guid.ToString());
        }

        private void barButtonItem97_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView4);
        }

        private void barButtonItem89_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView8);
        }

        private void barButtonItem90_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView8, myGridView8.Guid.ToString());
        }

        private void barButtonItem91_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView8, myGridView8.Guid.ToString());
        }

        private void barButtonItem92_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView8);
        }

        private void barButtonItem84_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView9);
        }

        private void barButtonItem85_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView9, myGridView9.Guid.ToString());
        }

        private void barButtonItem86_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView9, myGridView9.Guid.ToString());
        }

        private void barButtonItem87_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView9);
        }

        private void xtraTabControl1_CloseButtonClick(object sender, EventArgs e)
        {
            xtraTabControl1.TabPages.Remove(this.xtraTabControl1.SelectedTabPage);
        }

        private void barButtonItem99_ItemClick(object sender, ItemClickEventArgs e)
        {
            Cancel("CancelToCust");
        }

        //private void xtraTabControl1_CloseButtonClick_1(object sender, EventArgs e)
        //{
        //    xtraTabControl1.TabPages.Remove(this.xtraTabControl1.SelectedTabPage);
        //}

       
    }
}
