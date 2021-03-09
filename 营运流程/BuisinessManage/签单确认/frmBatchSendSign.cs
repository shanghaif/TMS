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
    public partial class frmBatchSendSign : BaseForm
    {
        private DataSet ds = new DataSet();
        private DataSet ds1 = new DataSet();
        GridHitInfo hitInfo;

        public frmBatchSendSign()
        {
            InitializeComponent();
            barEditItem1.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem1_EditValueChanging);
            barEditItem1.Edit.KeyDown += new KeyEventHandler(barEditItem1_KeyDown);
            barEditItem2.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem2_EditValueChanging);
            barEditItem2.Edit.KeyDown += new KeyEventHandler(barEditItem2_KeyDown);
        }

        //查询
        private void getdata()
        {
            string proc = "QSP_GET_SEND_FOR_SIGN";
            if (CommonClass.UserInfo.UserDB == UserDB.ZQTMS3PL)
            {
                proc = "QSP_GET_SEND_FOR_SIGN_3PL";
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

        private void frmBatchSendSign_Load(object sender, EventArgs e)
        {
            if (!CommonClass.FormSet(this, false, true)) return;
            CommonClass.GetGridViewColumns(myGridView1, false, myGridView2);
            GridOper.SetGridViewProperty(myGridView1, myGridView2);
            BarMagagerOper.SetBarPropertity(bar1, bar2);//如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1, myGridView2);
            SignOperator.Text = ConvertType.ToString(CommonClass.UserInfo.UserName);
            SignDate.DateTime = CommonClass.gcdate;
           
        }

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
                    myGridControl1.DoDragDrop("我要过去了....", DragDropEffects.All);
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
                    myGridControl2.DoDragDrop("我要过去了....", DragDropEffects.All);
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

        //验证并保存数据
        private void check()
        {
            if (myGridView2.RowCount == 0)
            {
                XtraMessageBox.Show("没有发现任何需要签收的清单，请先在第①步中构建清单。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (chkSMS.Checked)
            {
                if (0 == 0)
                {
                    XtraMessageBox.Show("您选择了短信通知，却没有勾选任何需要发送短信的运单!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            if (SignMan.Text.Trim() == "")
            {
                MsgBox.ShowOK("必须填写签收人！");
                return;
            }
            //判断所有的提货数据状态 拼接状态字符串

            float sumAccSend = 0;
            int sumCount = 0;
            int AccSendCount = 0;
            int SendVerifStateCount = 0;
            for (int i = 0; i < myGridView2.RowCount; i++)
            {

                if (ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "AccSend")) > 0)
                {
                    AccSendCount++;
                    sumAccSend += ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "AccSend"));
                }
                if (ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "SendVerifState")) == 1)
                {
                    SendVerifStateCount++;
                }
            }
            sumCount = myGridView2.RowCount;
            string sShowOK = "当前选中：" + ConvertType.ToString(sumCount) + "票\r\n送货费核销有：" +

                ConvertType.ToString(SendVerifStateCount) + "票\r\n送货费总金额为：" + ConvertType.ToString(sumAccSend) + "\r\n是否继续？";

            if (MsgBox.ShowYesNo(sShowOK) != DialogResult.Yes)
            {
                return;
            }
            Dictionary<string, string> dic = new Dictionary<string, string>();//字典存储运单号和批次号，一对一
            string BillNoStr = "";
            string SendBatchStr = "";
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                BillNoStr += ConvertType.ToString(myGridView2.GetRowCellValue(i, "BillNo")) + '@';
                SendBatchStr += ConvertType.ToString(myGridView2.GetRowCellValue(i, "SendBatch")) + '@';

                dic.Add(ConvertType.ToString(myGridView2.GetRowCellValue(i, "BillNo")), ConvertType.ToString(myGridView2.GetRowCellValue(i, "SendBatch")));
            }

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNoStr", BillNoStr));
                list.Add(new SqlPara("SignType", "送货签收"));
                list.Add(new SqlPara("SignMan", SignMan.Text.Trim()));
                list.Add(new SqlPara("SignManCardID", SignManCardID.Text.Trim()));
                list.Add(new SqlPara("AgentMan", ""));
                list.Add(new SqlPara("SignManPhone", SignManPhone.Text.Trim()));
                list.Add(new SqlPara("AgentCardId", SignManCardID.Text.Trim()));
                list.Add(new SqlPara("SignDate", SignDate.DateTime));
                list.Add(new SqlPara("SignDesc", ""));
                list.Add(new SqlPara("SignOperator", SignOperator.Text.Trim()));
                list.Add(new SqlPara("SignSite", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("SignWeb", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("SignContent", SignContent.Text.Trim()));
                list.Add(new SqlPara("SendBatchStr", SendBatchStr));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLSIGN_SEND", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    #region ZQTMS同步签收
                    //验证是否有做分拨
                    List<SqlPara> list1 = new List<SqlPara>();
                    list1.Add(new SqlPara("BillNoStr", BillNoStr));
                    SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_billDepartureFBList", list1);
                    DataTable dt = SqlHelper.GetDataTable(sps1);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        BillNoStr = string.Empty;
                        SendBatchStr = string.Empty;
                        foreach (DataRow row in dt.Rows)
                        {
                            BillNoStr += row["BillNo"].ToString() + "@";
                            SendBatchStr += dic[row["BillNo"].ToString()].ToString() + "@";
                        }
                        Dictionary<string, string> dic_ZQTMS = new Dictionary<string, string>();
                        dic_ZQTMS.Add("Bills", BillNoStr);
                        dic_ZQTMS.Add("SendBatchs", SendBatchStr);
                        dic_ZQTMS.Add("SignType", "送货签收");
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

                        //ZQTMS签收同步接口调用
                    
                        string url = HttpHelper.urlSignSyn;

                  

                        string data = JsonConvert.SerializeObject(dic_ZQTMS);
                        //data = data.TrimStart('[').TrimEnd(']');
                        ResponseModelClone<string> res = HttpHelper.HttpPost(data, url);
                        if (res.State != "200")
                        {
                            List<SqlPara> listLog = new List<SqlPara>();
                            listLog.Add(new SqlPara("BillNo", BillNoStr));
                            listLog.Add(new SqlPara("Batch", ""));
                            listLog.Add(new SqlPara("ErrorNode", "送货签收"));
                            listLog.Add(new SqlPara("ExceptMessage", res.Message));
                            SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                            SqlHelper.ExecteNonQuery(spsLog);
                            //MsgBox.ShowError(res.State + "：" + res.Message);
                        }
                        else
                        {
                            MsgBox.ShowOK("操作已完成：ZQTMS已同步签收！");
                        }
                    }
                    else
                    {
                        MsgBox.ShowOK();
                    }
                    #endregion
                    //CommonSyn.SignTimeSyn(BillNoStr, "送货签收");//ZQTMS签收时效同步
                    //CommonSyn.TraceSyn(null, BillNoStr, 13, "送货签收", 1, null, null); 
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

        private void gridControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GridViewMove.Move(myGridView1, ds, ds1);
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            getdata();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            //GridViewMove.ExportToExcel(myGridView1);
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
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

        //关闭
        private void btnClose_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            //GridViewMove.ExportToExcel(myGridView2);
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

        //保存
        private void btnOk_Click(object sender, EventArgs e)
        {
            check();
        }

        //取消退出
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void myGridView1_MouseUp(object sender, MouseEventArgs e)
        {
            PopMenu.ShowPopupMenu(myGridView1,e,popupMenu1);
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rows = myGridView1.FocusedRowHandle;
            if (rows < 0)return;
            CommonClass.ShowBillSearch(myGridView1.GetRowCellValue(rows, "BillNo").ToString());
        }

        //异常签收
        private void btnErrorSign_Click(object sender, EventArgs e)
        {
            string BillNo = "";
            if (myGridView2.RowCount > 1)
            {
                MsgBox.ShowOK("检测到待签收列表中有多单，异常签收每次只能签收一单，请先剔除掉多余的单！");
                return;
            }
            if (myGridView2.RowCount == 0)
            { return; }
            if (myGridView2.RowCount > 0)
            {
                //string PaymentMode = myGridView2.GetRowCellValue(0, "PaymentMode").ToString();
                //if (PaymentMode == "月结")
                //{
                //    MsgBox.ShowOK("此单付款方式为月结，不允许做异常签收！");
                //    return;
                //}
                BillNo = myGridView2.GetRowCellValue(0, "BillNo").ToString();//允许月结客户做异常签收--毛慧20170911
                try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("BillNo", BillNo));
                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_BILLPETTYPAY_BYBILLNO", list);
                    DataSet ds = SqlHelper.GetDataSet(sps);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        MsgBox.ShowOK("此单已做过小额赔付，不允许异常签收");
                        return;
                    }
                    if (SignMan.Text.Trim() == "")
                    {
                        MsgBox.ShowOK("必须填写签收人！");
                        return;
                    }
                    if (SignContent.Text.Trim() == "")
                    {
                        MsgBox.ShowOK("异常签收必须填写提货说明！");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                    return;
                }
                frmErrorSignEdit edit = new frmErrorSignEdit();
                edit.billNO = BillNo;
                edit.signType = "送货签收";
                edit.Text = "送货异常签收";
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
    }
}