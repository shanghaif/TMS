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
using DevExpress.XtraGrid.Views.Grid;

namespace ZQTMS.UI
{
    public partial class frmMiddleFollow_ADD : BaseForm
    {
        public string sConsignorCellPhone = "";
        public string sConsignorPhone = "";
        public string sConsignorName = "";

        public string sConsigneeCellPhone = "";
        public string sConsigneePhone = "";
        public string sConsigneeName = "";

        public string crrBillNO;
        public frmMiddleFollow_ADD()
        {
            InitializeComponent();
        }
        public string billNo = "";
        public string sPossiblArrivalTime = "";
        public string sArrivalTime = "";

        public string sMiddleSendTime = "";
        public int MiddleTraceState = 0;
        GridView _gv;

        /// <summary>
        /// 中转跟踪记录网格
        /// </summary>
        public GridView Gv
        {
            get { return _gv; }
            set { _gv = value; }
        }

        private void frmMiddleFollow_ADD_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            getTraceData();
            ArrivalTime.EditValue = sArrivalTime;
            PossiblArrivalTime.EditValue = sPossiblArrivalTime;
            MiddleSendTime.EditValue = sMiddleSendTime;
            ArrivalTime.EditValue = sArrivalTime;
            stn_cancel.Enabled = UserRight.GetRight("ZQTMS.UI.frmMiddleFollow#barButtonItem9");//取消跟踪结束
            if (CommonClass.UserInfo.companyid == "486")
            {
                btnDelete.Enabled = true;
            }
            if (MiddleTraceState == 1)
            {
                btnAdd.Enabled = false;
                btnClear.Enabled = false;
                btnDelete.Enabled = false;
                btnEndTrace.Enabled = false;
                btnNotice.Enabled = false;
                btnSave.Enabled = false;
                
            }
        }

        private void getTraceData()
        {
            if (billNo == "")
                return;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", billNo));
               // SqlParasEntity sps = new SqlParasEntity(OperType.QueryThreeTable, "QSP_GET_BILLMIDDLETRACE_ByBillNO", list);
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLMIDDLETRACE_ByBillNO", list);//zaj

                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];

                if (ds.Tables.Count < 2) return;
                chkArivel.Checked = true;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (TraceContent.Text.Trim() == "" && (this.radioButton1.Checked == false && this.radioButton2.Checked == false && this.radioButton3.Checked == false && this.radioButton4.Checked == false)) return;
            try
            {
                string checkedtext = "";
                if (this.radioButton1.Checked)
                {
                    checkedtext +="||"+ radioButton1.Text.ToString();
                }
                if (this.radioButton2.Checked)
                {

                    checkedtext += "||"+ radioButton2.Text.ToString();
                }
                if (this.radioButton3.Checked)
                {

                    checkedtext += "||" + radioButton3.Text.ToString();
                }
                if (this.radioButton4.Checked)
                {

                    checkedtext += "||" + radioButton4.Text.ToString();
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("TraceId", Guid.NewGuid()));
                list.Add(new SqlPara("BillNo", billNo));
                list.Add(new SqlPara("TraceDate", CommonClass.gcdate));
                list.Add(new SqlPara("TraceContent", TraceContent.Text.Trim()+checkedtext.ToString()));
                list.Add(new SqlPara("TraceMan", CommonClass.UserInfo.UserName)); ;

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLMIDDLETRACE", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    getTraceData();
                }

                if (chkNotice.Checked)
                {
                    string mb = sConsignorCellPhone;

                    if (mb == "" || mb.Substring(0, 1) != "1")
                    {
                        mb = sConsignorPhone;
                        if (mb != "")
                        {
                            if (mb.Substring(0, 1) != "1")
                            {
                                MsgBox.ShowYesNoCancel("发货人手机号不正确，短信未发送!");
                                return;
                            }
                        }
                    }

                    if (!sms.SaveSMSS("1", sConsignorName, mb, MessagePreview.Text.Trim(), DateTime.Now, billNo)) return;
                    if (sms.sendsms(mb, MessagePreview.Text.Trim()))
                    {
                        MsgBox.ShowOK("短信发送完成!");
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;

                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("TraceId", new Guid(myGridView1.GetRowCellValue(rowhandle, "TraceId").ToString())));
                list.Add(new SqlPara("BillNo", billNo));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_BILLMIDDLETRACE", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();

                    myGridView1.DeleteRow(rowhandle);
                    myGridView1.PostEditor();
                    myGridView1.UpdateCurrentRow();
                    myGridView1.UpdateSummary();
                    DataTable dt = myGridControl1.DataSource as DataTable;
                    dt.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            MessagePreview.Text = "";
            TraceContent.Text = "";
        }

        private void btnEndTrace_Click(object sender, EventArgs e)
        {
            //if (chkArivel.Checked == false) return;
            //if (ArrivalTime.Text.Trim() == "")
            //{
            //    MsgBox.ShowOK("请输入货物到货的时间！");
            //    return;
            //}
            try
            {
                if (MsgBox.ShowYesNo("本单确定已经跟踪完毕?\r\n\r\n跟踪完毕之后,本单将不可以再做跟踪!") != DialogResult.Yes) return;

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNO ", billNo));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_SET_MIDDLETRACE_ARRIVE_CONDIRM", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK("本单跟踪结束成功!");
                    if (_gv != null) _gv.SetRowCellValue(_gv.FocusedRowHandle, "MiddleTraceState", 1);//修改记录为跟踪结束
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void btnNotice_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessagePreview.Text.Trim() == "")
                {
                    MsgBox.ShowOK("没有填写短信内容!\r\n\r\n请点击“确定”, 系统将自动生成短信内容，如果不满意短信格式，请手动修改!");
                    return;
                }

                if (MsgBox.ShowYesNo("确定发送短信吗? 当前短信内容为：" + "\r\n" + MessagePreview.Text.Trim()) == DialogResult.No) return;

                if (chkArivel.Checked)
                {
                    string mb = sConsigneeCellPhone;

                    if (mb == "" || mb.Substring(0, 1) != "1")
                    {
                        mb = sConsigneePhone;
                        if (mb != "")
                        {
                            if (mb.Substring(0, 1) != "1")
                            {
                                MsgBox.ShowOK("收货人手机号不正确，短信未发送!");
                                return;
                            }
                        }
                    }
                    if (!sms.SaveSMSS("1", sConsigneeName, mb, MessagePreview.Text.Trim(), CommonClass.gcdate, billNo)) return;
                    if (sms.sendsms(mb, MessagePreview.Text.Trim()))
                    {
                        MsgBox.ShowOK("已短信通知收货人!");
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNO ", billNo));
                list.Add(new SqlPara("MiddleSendTime", MiddleSendTime.Text.Trim() == "" ? DBNull.Value : MiddleSendTime.EditValue));
                list.Add(new SqlPara("PossiblArrivalTime", PossiblArrivalTime.Text.Trim() == "" ? DBNull.Value : PossiblArrivalTime.EditValue));
                list.Add(new SqlPara("ArrivalTime", chkArivel.Checked ? ArrivalTime.EditValue : DBNull.Value));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLMIDDLETRACE_TIMES", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    if (_gv == null) return;
                    int rowhandle = _gv.FocusedRowHandle;
                    if (rowhandle < 0) return;
                    _gv.SetRowCellValue(rowhandle, "MiddleSendTime", MiddleSendTime.EditValue);
                    _gv.SetRowCellValue(rowhandle, "PossiblArrivalTime", PossiblArrivalTime.EditValue);
                    _gv.SetRowCellValue(rowhandle, "ArrivalTime", chkArivel.Checked ? ArrivalTime.EditValue : DBNull.Value);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void chkArivel_CheckedChanged(object sender, EventArgs e)
        {
            if (chkArivel.Checked && (ArrivalTime.Text.Trim() == "" || ArrivalTime.DateTime < CommonClass.gcdate.AddYears(10))) ArrivalTime.DateTime = CommonClass.gcdate;
        }

        private void stn_cancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (MsgBox.ShowYesNo("本单已跟踪结束！\r\n\r\n是否确定取消跟踪结束？") != DialogResult.Yes) return;

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNO ", billNo));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_SET_MIDDLETRACE_ARRIVE_Restart", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK("本单取消跟踪结束成功!");
                    if (_gv != null) _gv.SetRowCellValue(_gv.FocusedRowHandle, "MiddleTraceState", 0);//修改记录为跟踪结束
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

       
    }
}