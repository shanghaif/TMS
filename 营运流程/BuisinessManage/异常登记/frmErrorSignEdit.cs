using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using Newtonsoft.Json;

namespace ZQTMS.UI
{
    public partial class frmErrorSignEdit : BaseForm
    {
        public frmErrorSignEdit()
        {
            InitializeComponent();
        }
        public string SignNO { get; set; }//签收单号
        //public Guid GuidSignNO = Guid.Empty;
        public string billNO = "";
        public string signType = "";
        public string OperationType { get; set; }//1、check代表查看2、modify代表修改3、add代表新增
        public string signMan = "";
        public string signManPhone="";
        public string signManCardID = "";
        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }
        
        #region 验证文本是否为空
        private bool Validate()
        {
            if (string.IsNullOrEmpty(this.txtBillNO.Text.Trim()))
            {
                MsgBox.ShowOK("用单号不允许为空，请输入！");
                this.txtBillNO.Focus();
                return false;
            }
            if (this.cbbSignType.Text.Trim() == "--请选择--")
            {
                MsgBox.ShowOK("请选择签收类型！");
                this.cbbSignType.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.txtSignMan.Text.Trim()))
            {
                MsgBox.ShowOK("签收人不允许为空，请输入！");
                this.txtSignMan.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.txtSignManPhone.Text.Trim()))
            {
                MsgBox.ShowOK("签收人电话不允许为空，请输入！");
                this.txtSignManPhone.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.txtSignManCardID.Text.Trim()))
            {
                MsgBox.ShowOK("签收人身份证不允许为空，请输入！");
                this.txtSignManCardID.Focus();
                return false;
            }           
            if (string.IsNullOrEmpty(this.txtSignOperator.Text.Trim()))
            {
                MsgBox.ShowOK("签收经办人不允许为空，请输入！");
                this.txtSignOperator.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.cbbSignWeb.Text.Trim()))
            {
                MsgBox.ShowOK("签收网点不允许为空，请输入！");
                this.cbbSignWeb.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.cbbSignSite.Text.Trim()))
            {
                MsgBox.ShowOK("签收站点不允许为空，请输入！");
                this.cbbSignSite.Focus();
                return false;
            }
            //if (string.IsNullOrEmpty(this.txtDepartureListNO.Text.Trim()))
            //{
            //    MsgBox.ShowOK("发车ID不允许为空，请输入！");
            //    this.txtDepartureListNO.Focus();
            //    return false;
            //}
            if (string.IsNullOrEmpty(this.cbbCauseName.Text.Trim()))
            {
                MsgBox.ShowOK("签收事业部不允许为空，请输入！");
                this.cbbCauseName.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.cbbAreaName.Text.Trim()))
            {
                MsgBox.ShowOK("签收大区不允许为空，请输入！");
                this.cbbAreaName.Focus();
                return false;
            }
            if (this.cbbExceptType.Text.Trim() == "--请选择--")
            {
                MsgBox.ShowOK("请选择异常类型！");
                this.cbbExceptType.Focus();
                return false;
            }
            //if (string.IsNullOrEmpty(this.txtSignContent.Text.Trim()))
            //{
            //    MsgBox.ShowOK("签收情况不允许为空，请输入！");
            //    this.txtSignContent.Focus();
            //    return false;
            //}
            if (string.IsNullOrEmpty(this.txtExceptContent.Text.Trim()))
            {
                MsgBox.ShowOK("异常描述不允许为空，请输入！");
                this.txtExceptContent.Focus();
                return false;
            }
            //if (string.IsNullOrEmpty(this.txtSignDesc.Text.Trim()))
            //{
            //    MsgBox.ShowOK("签收说明不允许为空，请输入！");
            //    this.txtSignDesc.Focus();
            //    return false;
            //}
            if (this.cbbExceptReason.Text.Trim() == "--请选择--")
            {
                MsgBox.ShowOK("请选择异常原因");
                return false;
            }
            return true;

        } 
        #endregion

        #region 保存异常签收记录
        private void Save()
        {

            #region 新增
           
                try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    SqlParasEntity sps = null;
                    if (Validate())
                    {
                        list.Add(new SqlPara("BillNO", txtBillNO.Text.Trim()));
                        list.Add(new SqlPara("SignType", cbbSignType.Text.Trim()));
                        list.Add(new SqlPara("SignMan", txtSignMan.Text.Trim()));
                        list.Add(new SqlPara("SignManPhone", txtSignManPhone.Text.Trim()));
                        list.Add(new SqlPara("SignManCardID", txtSignManCardID.Text.Trim()));
                        list.Add(new SqlPara("SignDate", bdate.Text.Trim()));
                        list.Add(new SqlPara("SignOperator", txtSignOperator.Text.Trim()));
                        list.Add(new SqlPara("SignSite", cbbSignSite.Text.Trim()));
                        list.Add(new SqlPara("SignWeb", cbbSignWeb.Text.Trim()));
                        //list.Add(new SqlPara("DepartureListNO", txtDepartureListNO.Text.Trim()));
                        list.Add(new SqlPara("CauseName", cbbCauseName.Text.Trim()));
                        list.Add(new SqlPara("AreaName", cbbAreaName.Text.Trim()));
                        list.Add(new SqlPara("ExceptType", cbbExceptType.Text.Trim()));
                        list.Add(new SqlPara("ExceptContent", txtExceptContent.Text.Trim()));
                        list.Add(new SqlPara("ExceptReason", cbbExceptReason.Text.Trim()));
                        sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLERRORSIGN", list);
                        if (SqlHelper.ExecteNonQuery(sps) > 0)
                        {
                            #region ZQTMS同步签收
                            //验证是否有做分拨
                            List<SqlPara> list1 = new List<SqlPara>();
                            list1.Add(new SqlPara("BillNoStr", txtBillNO.Text.Trim() + "@"));
                            SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_billDepartureFBList", list1);
                            DataTable dt = SqlHelper.GetDataTable(sps1);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                Dictionary<string, string> dic_ZQTMS = new Dictionary<string, string>();
                                dic_ZQTMS.Add("Bills", dt.Rows[0]["BillNo"].ToString());
                                dic_ZQTMS.Add("SendBatchs", cbbSignType.Text.Trim());//传签收类型（送货、中转、自提）签收
                                dic_ZQTMS.Add("SignType", "异常签收");
                                dic_ZQTMS.Add("SignMan", txtSignMan.Text.Trim());
                                dic_ZQTMS.Add("SignManCardID", txtSignManCardID.Text.Trim());
                                dic_ZQTMS.Add("AgentMan", "");
                                dic_ZQTMS.Add("SignManPhone", txtSignManPhone.Text.Trim());
                                dic_ZQTMS.Add("AgentCardId", txtSignManCardID.Text.Trim());
                                //dic_ZQTMS.Add("SignDate", @"\/Date(" + SignDate.DateTime.ToString() + @")\/");
                                dic_ZQTMS.Add("SignDesc", "");
                                dic_ZQTMS.Add("SignOperator", txtSignOperator.Text.Trim());
                                dic_ZQTMS.Add("SignSite", ConvertType.ToString(CommonClass.UserInfo.SiteName));
                                dic_ZQTMS.Add("SignWeb", ConvertType.ToString(CommonClass.UserInfo.WebName));
                                dic_ZQTMS.Add("SignContent", txtExceptContent.Text.Trim());

                                dic_ZQTMS.Add("LoginAreaName", CommonClass.UserInfo.AreaName);
                                dic_ZQTMS.Add("LoginCauseName", CommonClass.UserInfo.CauseName);
                                dic_ZQTMS.Add("LoginDepartName", CommonClass.UserInfo.DepartName);
                                dic_ZQTMS.Add("LoginSiteName", CommonClass.UserInfo.SiteName);
                                dic_ZQTMS.Add("LoginWebName", CommonClass.UserInfo.WebName);
                                dic_ZQTMS.Add("LoginUserAccount", CommonClass.UserInfo.UserAccount);
                                dic_ZQTMS.Add("LoginUserName", CommonClass.UserInfo.UserName);
                                dic_ZQTMS.Add("ExceptType", cbbExceptType.Text.Trim());
                                dic_ZQTMS.Add("ExceptContent", txtExceptContent.Text.Trim());
                                dic_ZQTMS.Add("ExceptReason", cbbExceptReason.Text.Trim());

                                //LMS签收同步接口调用
                               
                                string url = HttpHelper.urlSignSyn;

                          

                                string data = JsonConvert.SerializeObject(dic_ZQTMS);
                                //data = data.TrimStart('[').TrimEnd(']');
                                ResponseModelClone<string> res = HttpHelper.HttpPost(data, url);
                                if (res.State != "200")
                                {
                                    List<SqlPara> listLog = new List<SqlPara>();
                                    listLog.Add(new SqlPara("BillNo", txtBillNO.Text.Trim() + "@"));
                                    listLog.Add(new SqlPara("Batch", ""));
                                    listLog.Add(new SqlPara("ErrorNode", "送货签收"));
                                    listLog.Add(new SqlPara("ExceptMessage", res.Message));
                                    SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                                    SqlHelper.ExecteNonQuery(spsLog);
                                    //MsgBox.ShowError(res.State + "：" + res.Message);
                                }
                                else
                                {
                                    MsgBox.ShowOK("异常签收成功：ZQTMS已同步签收！");
                                }
                            }
                            else
                            {
                                MsgBox.ShowOK("异常签收成功！");
                            }
                            #endregion
                            //yzw 同步
                            CommonSyn.BILLERRORSIGN_SYN(txtBillNO.Text.Trim(), txtSignMan.Text.Trim(), cbbSignWeb.Text.Trim(), cbbSignWeb.Text.Trim());

                            this.DialogResult = DialogResult.OK;
                            //MsgBox.ShowOK("异常签收成功！");
                            this.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    
                    MsgBox.ShowException(ex) ;
                }
               
            
            #endregion

            

        } 
        #endregion

        #region 根据SingNO得到异常签收信息并初始化页面
        //根据SingNO得到异常签收信息并初始化页面
        private void GetBillErrorSignBySignNO()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SignNO", SignNO));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLERRORSIGN_ByID", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                DataRow dr = ds.Tables[0].Rows[0];
                txtBillNO.Text = dr["BillNO"].ToString();
                cbbSignType.Text = dr["SignType"].ToString();
                txtSignMan.Text = dr["SignMan"].ToString();
                txtSignManPhone.Text = dr["SignManPhone"].ToString();
                txtSignManCardID.Text = dr["SignManCardID"].ToString();
                bdate.Text = dr["SignDate"].ToString();
                txtSignOperator.Text = dr["SignOperator"].ToString();
                cbbSignSite.Text = dr["SignSite"].ToString();
                cbbSignWeb.Text = dr["SignWeb"].ToString();
                //txtDepartureListNO.Text = dr["DepartureListNO"].ToString();
                cbbCauseName.Text = dr["CauseName"].ToString();
                cbbAreaName.Text = dr["AreaName"].ToString();
                cbbExceptType.Text = dr["ExceptType"].ToString();
                txtExceptContent.Text = dr["ExceptContent"].ToString();
                cbbExceptReason.Text = dr["ExceptReason"].ToString();
            }
            catch (Exception ex)
            {
                
                MsgBox.ShowException(ex);
            }
        } 
        #endregion


        private void frmErrorSignEdit_Load(object sender, EventArgs e)
        {

            //if (OperationType=="check" && !string.IsNullOrEmpty(SignNO))
            //{
            //    GetBillErrorSignBySignNO();
            //    this.btnSave.Enabled = false;
            //}
            //else if (OperationType == "modify" && !string.IsNullOrEmpty(SignNO))
            //{
            //    GetBillErrorSignBySignNO();
            //    this.btnSave.Text = "修改";
            //}
            init();
           
        }
        private void init()
        {
            bdate.EditValue = CommonClass.gcdate;
            txtBillNO.Text = billNO;
            txtBillNO.Enabled = false;
            CommonClass.SetSite(cbbSignSite, true);
            CommonClass.SetWeb(cbbSignWeb, true);           
            CommonClass.SetCause(cbbCauseName, true);
            CommonClass.SetArea(cbbAreaName, "全部", true);
            cbbExceptType.Text = "--请选择--";          
            cbbSignType.Enabled = false;
            cbbExceptReason.Text = "--请选择--";
            txtSignOperator.Text = CommonClass.UserInfo.UserName;
            cbbSignSite.Text = CommonClass.UserInfo.SiteName;
            cbbCauseName.Text = CommonClass.UserInfo.CauseName;
            cbbAreaName.Text = CommonClass.UserInfo.AreaName;
            cbbSignWeb.Text = CommonClass.UserInfo.WebName;
            if (signMan != "")
            {
                txtSignMan.Text = signMan;
                txtSignMan.Enabled = false;
            
            }
            if (signManCardID != "")
            {
                txtSignManCardID.Text = signManCardID;
                txtSignManCardID.Enabled = false;
            }
            if (signManPhone != "")
            {
                txtSignManPhone.Text = signManPhone;
                txtSignManPhone.Enabled = false;

               
            }
            
            
            
            if (this.Text == "送货异常签收")
            {
                cbbSignType.Text = "送货签收";
            }
            if (this.Text == "自提异常签收")
            {
                cbbSignType.Text = "自提签收";
            }
            if (this.Text == "中转异常签收")
            {
                cbbSignType.Text = "中转签收";
            }
        
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

     
    }
}
