using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.Common;
using DevExpress.XtraGrid.Columns;
using ZQTMS.SqlDAL;
using System.Text.RegularExpressions;

namespace ZQTMS.UI
{
    public partial class frmVehicleWithholdAdd : BaseForm
    {
        public frmVehicleWithholdAdd()
        {
            InitializeComponent();
        }
       public string id = string.Empty;
       public string certificateNo = string.Empty;
       public string oper = "";//1新增 2修改
        private void myGridControl3_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void frmVehicleWithholdAdd_Load(object sender, EventArgs e)
        {
            ExpArriveDate.EditValue = System.DateTime.Now; //赋值现行年月
            ExpArriveDate.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;//禁止编辑
            var formatString = "yyyy-MM";//格式化日期
            ExpArriveDate.Properties.Mask.EditMask = formatString;
            //ExpArriveDate.Properties.VistaCalendarInitialViewStyle = DevExpress.XtraEditors.VistaCalendarInitialViewStyle.YearView;
            //ExpArriveDate.Properties.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.YearView;
            ExpArriveDate.Properties.ShowToday = false;
            ExpArriveDate.Properties.Mask.UseMaskAsDisplayFormat = true;//允许格式化显示 
            CommonClass.GetGridViewColumns(myGridView3,false);
            GridOper.SetGridViewProperty(myGridView3);
           // CommonClass.SetArea(cbbArea, "全部",true);
           // CommonClass.SetWeb(cbbWeb, true);
           // CommonClass.SetCause(cbbCause,true);
          //  cbbWeb.Text = CommonClass.UserInfo.WebName;
            //cbbCause.Text = CommonClass.UserInfo.CauseName;
            //cbbArea.Text = CommonClass.UserInfo.AreaName;
            dOperDate.DateTime = CommonClass.gcdate;
            //jl20180911
            //dBusinessDate.DateTime = CommonClass.gcdate;
            DateTime dt = CommonClass.gcdate; //当前服务器时间
            //String startYear = dt.AddYears(dt.Year).ToString();
            //String startMonth = dt.AddMonths(dt.Month).ToString();
            DateTime startDay = dt.AddDays(1 - dt.Day);//本月月初

            DateTime startHour = startDay.AddHours(0 - startDay.Hour);
            DateTime startMinute = startHour.AddMinutes(0 - startHour.Minute);
            DateTime startSecond = startMinute.AddSeconds(0 - startMinute.Second);
            dBusinessDate.DateTime = startSecond;

            txtMan.Text = CommonClass.UserInfo.UserName;
            CommonClass.SetProvince(oilCardProvince);
            txtCertificate.Enabled = false;
            dOperDate.Enabled = false;
            cmbUse.Text = "--请选择--";
            if (oper == "1")
            {
                btnSave.Text = "新增";
            }
            if (oper == "2")
            {
                btnSave.Text = "修改";
            }
            if (id == string.Empty)
            {
                txtCertificate.EditValue = DateTime.Now.ToString("yyyyMMddHHmmss");

            }
            else
            {
                try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    SqlParasEntity sps = null;
                  
                        list.Add(new SqlPara("ID", id));
                        sps = new SqlParasEntity(OperType.Query, "QSP_GET_VEHICLEWITHOLD_ByID", list);
                        DataSet ds = SqlHelper.GetDataSet(sps);
                        if (ds == null || ds.Tables[0].Rows.Count <= 0)
                        { return; }
                        if (ds.Tables[0].Rows[0]["useFee"].ToString() == "抵账")
                        {
                            radioButton1.Checked = true;
                        }
                        else
                        {
                            radioButton2.Checked = true;
                        }
                        SendCarNO.Text = ds.Tables[0].Rows[0]["CarNo"].ToString();
                        cbbType.Text = ds.Tables[0].Rows[0]["WithholdType"].ToString();
                        SendDriver.Text = ds.Tables[0].Rows[0]["DriverName"].ToString();
                        txtMoney.Text = ds.Tables[0].Rows[0]["WithholdMoney"].ToString();
                        SendDriverPhone.Text = ds.Tables[0].Rows[0]["DriverPhone"].ToString();
                        SendDriverPhone.Text = ds.Tables[0].Rows[0]["DriverPhone"].ToString();
                        dOperDate.EditValue = ds.Tables[0].Rows[0]["OperDate"].ToString();
                        dBusinessDate.EditValue = ds.Tables[0].Rows[0]["BusinessDate"].ToString();
                        txtCertificate.Text = ds.Tables[0].Rows[0]["CertificateNo"].ToString();
                        txtRemark.Text = ds.Tables[0].Rows[0]["Remark"].ToString();
                        txtAccountName.Text = ds.Tables[0].Rows[0]["RefundAccountName"].ToString();
                        txtAccountNo.Text = ds.Tables[0].Rows[0]["AccountNO"].ToString();
                        txtBank.Text = ds.Tables[0].Rows[0]["BankName"].ToString();
                        cmbUse.Text = ds.Tables[0].Rows[0]["VehicleUse"].ToString();
                        ExpArriveDate.Text = ds.Tables[0].Rows[0]["belongMonth"].ToString();
                        oilCardProvince.Text = ds.Tables[0].Rows[0]["Province"].ToString();
                        oilCardCity.Text = ds.Tables[0].Rows[0]["City"].ToString();
                       
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            myGridControl3.DataSource = CommonClass.dsCar.Tables[0];
            myGridControl3.BringToFront();

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

        private void myGridView3_DoubleClick(object sender, EventArgs e)
        {
            SetCarInfo();
        }

        private void SendCarNO_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void SendCarNO_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue == null) return;
            string value = e.NewValue.ToString();
            myGridView3.Columns["CarNo"].FilterInfo = new ColumnFilterInfo(
                    "[CarNo] LIKE " + "'%" + value + "%'"
                    + " OR [DriverName] LIKE" + "'%" + value + "%'"
                    + " OR [DriverPhone] LIKE" + "'%" + value + "%'",
                    "");
        }
        private bool validate()
        {
            if (ExpArriveDate.Text.Trim() == "")
            {
                MsgBox.ShowOK("月份为必填项!");
                return false;
            }
            if (SendCarNO.Text.Trim() == "")
            {
                MsgBox.ShowOK("送货车号不能为空！");
                return false;
            }
            if (cbbType.Text.Trim() == "")
            {
                MsgBox.ShowOK("代扣类型不能为空！");
                return false;
            }
            if (cmbUse.Text == "--请选择--")
            {
                MsgBox.ShowOK("车辆用途必须填！");
                return false;
            }
            if (txtRemark.Text.Trim() == "")
            {
                MsgBox.ShowOK("摘要不能为空！");
                return false;
            }
            if (cbbType.Text.Trim() == "油料费")
            {
                MsgBox.ShowOK("代扣类型为油料费时不允许在此模块新增，请在油料登记模块进行新增！");
                return false;
            }
            string posPattern = @"^[0-9]+(.[0-9]{1,2})?$";//验证正数正则
            string negPattern = @"^\-[0-9]+(.[0-9]{1,2})?$";//验证负数正则
            if (cbbType.Text == "奖励支出")
            {
                if (!Regex.IsMatch(txtMoney.Text, negPattern))
                {
                    MsgBox.ShowOK("当代扣类型为奖励支出时，代扣金额只能为负数");
                    return false;
                }
            }
            else
            {
                if (!Regex.IsMatch(txtMoney.Text, posPattern))
                {
                    MsgBox.ShowOK("代扣金额格式不正确，请确认是正数并且是数字！");
                    return false;
                }
            }
            //jl20180911
            DialogResult result = MessageBox.Show("请确认业务发生日期是否正确?", "温馨提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                return true;
            }
            else
            {
                return false;
            }
            //return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (radioButton1.Checked == true)
            {
                txtAccountName.Text = "";
                txtAccountNo.Text = "";
                txtBank.Text = "";
            }
            if (radioButton2.Checked == true && (txtAccountName.Text.Trim() == "" || txtAccountNo.Text.Trim() == "" || txtBank.Text.Trim() == ""))
            {
                MsgBox.ShowOK("选择返款需填写银行信息！");
                return;
            }
                
            if (oper == "1")
            {
                
                //Regex.IsMatch()
                if (validate())
                {
                    try
                    {

                        List<SqlPara> list = new List<SqlPara>();
                        SqlParasEntity sps = null;
                        list.Add(new SqlPara("CarNo", SendCarNO.Text.Trim()));
                        list.Add(new SqlPara("WithholdType", cbbType.Text.Trim()));
                        list.Add(new SqlPara("DriverName", SendDriver.Text.Trim()));
                        list.Add(new SqlPara("WithholdMoney", Convert.ToDecimal(txtMoney.Text.Trim())));
                        list.Add(new SqlPara("DriverPhone", SendDriverPhone.Text.Trim()));
                        list.Add(new SqlPara("VehicleUse", cmbUse.Text.Trim()));
                        list.Add(new SqlPara("OperDate", dOperDate.Text.Trim()));
                        list.Add(new SqlPara("BusinessDate", dBusinessDate.Text.Trim()));//jl20180911
                        list.Add(new SqlPara("CertificateNo", txtCertificate.Text.Trim()));
                        list.Add(new SqlPara("Remark", txtRemark.Text.Trim()));
                        list.Add(new SqlPara("CauseName", CommonClass.UserInfo.CauseName));
                        list.Add(new SqlPara("AreaName", CommonClass.UserInfo.AreaName));
                        list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));
                        list.Add(new SqlPara("RegisterMan", txtMan.Text.Trim()));
                        list.Add(new SqlPara("RefundAccountName", txtAccountName.Text.Trim()));
                        list.Add(new SqlPara("AccountNO", txtAccountNo.Text.Trim()));
                        list.Add(new SqlPara("BankName", txtBank.Text.Trim()));
                        list.Add(new SqlPara("belongMonth", ExpArriveDate.Text.Trim()));
                        list.Add(new SqlPara("Province", oilCardProvince.Text.Trim()));
                        list.Add(new SqlPara("City", oilCardCity.Text.Trim()));

                        if (radioButton1.Checked == true)
                        {
                            list.Add(new SqlPara("useFee", radioButton1.Text.Trim()));
                        }
                        else
                        {
                            Regex r = new Regex(@"^\d{6,50}$");
                            Regex r2 = new Regex(@"[\u4e00-\u9fbb]");
                            if (!r.IsMatch(txtAccountNo.Text.Trim()))
                            {
                                MsgBox.ShowError("请输入正确的返款账户!");
                                return;
                            }
                            if (!r2.IsMatch(txtAccountName.Text.Trim()))
                            {
                                MsgBox.ShowError("银行户名只能输汉字");
                                return;
                            }
                            list.Add(new SqlPara("useFee", radioButton2.Text.Trim()));
                        }
                       


                        sps = new SqlParasEntity(OperType.Execute, "USP_ADD_VEHICLEWITHOLD", list);
                        if (SqlHelper.ExecteNonQuery(sps) > 0)
                        {
                            MsgBox.ShowOK("新增成功！");
                            this.Close();
                        }

                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                }
            }
            if (oper == "2")
            {
                if (validate())
                {
                    try
                    {
                        List<SqlPara> list = new List<SqlPara>();
                        SqlParasEntity sps = null;
                        list.Add(new SqlPara("ID", id));
                        list.Add(new SqlPara("CarNo", SendCarNO.Text.Trim()));
                        list.Add(new SqlPara("WithholdType", cbbType.Text.Trim()));
                        list.Add(new SqlPara("DriverName", SendDriver.Text.Trim()));
                        list.Add(new SqlPara("WithholdMoney", Convert.ToDecimal(txtMoney.Text.Trim())));
                        list.Add(new SqlPara("DriverPhone", SendDriverPhone.Text.Trim()));
                        list.Add(new SqlPara("VehicleUse", cmbUse.Text.Trim()));
                        list.Add(new SqlPara("OperDate", dOperDate.Text.Trim()));
                        list.Add(new SqlPara("BusinessDate", dBusinessDate.Text.Trim()));//jl20180911
                        list.Add(new SqlPara("CertificateNo", txtCertificate.Text.Trim()));
                        list.Add(new SqlPara("Remark", txtRemark.Text.Trim()));
                        list.Add(new SqlPara("RefundAccountName", txtAccountName.Text.Trim()));
                        list.Add(new SqlPara("AccountNO", txtAccountNo.Text.Trim()));
                        list.Add(new SqlPara("BankName", txtBank.Text.Trim()));

                        list.Add(new SqlPara("Province", oilCardProvince.Text.Trim()));
                        list.Add(new SqlPara("City", oilCardCity.Text.Trim()));

                        if (radioButton1.Checked == true)
                        {
                            list.Add(new SqlPara("useFee", radioButton1.Text.Trim()));
                        }
                        else
                        {
                            Regex r = new Regex(@"^\d{6,50}$");
                            Regex r2 = new Regex(@"[\u4e00-\u9fbb]");
                            if (!r.IsMatch(txtAccountNo.Text.Trim()))
                            {
                                MsgBox.ShowError("请输入正确的返款账户!");
                                return;
                            }
                            if (!r2.IsMatch(txtAccountName.Text.Trim()))
                            {
                                MsgBox.ShowError("银行户名只能输汉字");
                                return;
                            }
                            list.Add(new SqlPara("useFee", radioButton2.Text.Trim()));
                        }
                        sps = new SqlParasEntity(OperType.Execute, "USP_Modify_VEHICLEWITHOLD_ByID", list);
                        if (SqlHelper.ExecteNonQuery(sps) > 0)
                        {
                            MsgBox.ShowOK("修改成功！");
                            this.Close();
                        }

                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public string AccountName = "", AccountNo = "", Bank = "", ooilCardProvince = "", ooilCardCity = "";

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            AccountName = txtAccountName.Text;
            AccountNo = txtAccountNo.Text;
            Bank = txtBank.Text;
            ooilCardProvince = oilCardProvince.Text;
            ooilCardCity = oilCardCity.Text;

            txtAccountName.Text = "";
            txtAccountName.Enabled = false;
            txtAccountNo.Text = "";
            txtAccountNo.Enabled = false;
            txtBank.Text = "";
            txtBank.Enabled = false;
            oilCardProvince.Text="";
            oilCardProvince.Enabled = false;
            oilCardCity.Text = "";
            oilCardCity.Enabled = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            txtAccountName.Text = AccountName;
            txtAccountNo.Text = AccountNo;
            txtBank.Text = Bank;
            oilCardProvince.Text = ooilCardProvince;
            oilCardCity.Text = ooilCardCity;

           //// txtAccountName.Text = "";
           txtAccountName.Enabled = true;
           // //txtAccountNo.Text = "";
           txtAccountNo.Enabled = true;
           //// txtBank.Text = "";
           txtBank.Enabled = true;
           oilCardProvince.Enabled = true;
           oilCardCity.Enabled = true;
        }

        private void oilCardProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (oilCardProvince.Text.Trim() != "")
            {
                CommonClass.SetCity(oilCardProvince, oilCardCity);
            }
            else
            {
                oilCardCity.Text = "";
                oilCardCity.Properties.Items.Clear();
            }
           
        }



    }
}
