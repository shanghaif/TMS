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

namespace ZQTMS.UI
{
    public partial class frmInsuranceEdit : BaseForm
    {
        public string insuranceNO { get; set; }
        public string responceState { get; set; }
        private string state = "";
        public frmInsuranceEdit()
        {
            InitializeComponent();
        }

        private void frmInsuranceEdit_Load(object sender, EventArgs e)
        {

            this.dSendTime.EditValue = CommonClass.gcdate;
            this.dCreateTime.EditValue = CommonClass.gcdate;
            //cbbState.Enabled = false;
            List<SqlPara> list = new List<SqlPara>();
            SqlParasEntity sps = null;
            list.Add(new SqlPara("insuranceNO", insuranceNO));
            list.Add(new SqlPara("responceState",responceState));
            sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLINSURANCE_ByNO", list);
            DataSet ds = SqlHelper.GetDataSet(sps);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
            DataRow dr = ds.Tables[0].Rows[0];
            txtBillNO.Text = dr["BillNo"].ToString();
            cbbState.Text = dr["State"].ToString() == "0" ? "未发送" : dr["State"].ToString() == "1" ? "请求失败" : "请求成功";
            dSendTime.Text = dr["SendDate"].ToString();
            dCreateTime.Text = dr["CreateDate"].ToString();
            txtRequestURL.Text = dr["RequestURL"].ToString();
            txtQueryURL.Text = dr["ResponseURL"].ToString();
            cbbResponseState.Text = dr["ResponceState"].ToString() == "1" ? "成功" : "";
            txtGUID.Text = dr["ResponcePolicyGuid"].ToString();
            txtCount.Text = dr["RequestCount"].ToString();
            txtCode.Text = dr["SequenceCode"].ToString();
            txtResponseMsg.Text = dr["ResponceContent"].ToString();
            txtRequestJson.Text = dr["JsonText"].ToString();
            dLastSentTime.Text = dr["LastSendDate"].ToString();
            state = cbbState.Text.Trim();




           

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();

            
        }

        private void Save()
        {
            try
            {
                if (state == "请求失败")
                {
                    List<SqlPara> list = new List<SqlPara>();
                    SqlParasEntity sps = null;
                    list.Add(new SqlPara("insuranceNO", insuranceNO));
                    list.Add(new SqlPara("BillNO", txtBillNO.Text.Trim()));
                    list.Add(new SqlPara("State", cbbState.Text.Trim() == "未发送" ? "0" : cbbState.Text.Trim() == "请求失败" ? "1" : "2"));
                    list.Add(new SqlPara("SendDate", dSendTime.Text.Trim()));
                    list.Add(new SqlPara("CreateDate", dCreateTime.Text.Trim()));
                    list.Add(new SqlPara("RequestURL", txtRequestURL.Text.Trim()));
                    list.Add(new SqlPara("ResponseURL", txtQueryURL.Text.Trim()));
                    list.Add(new SqlPara("ResponseState", cbbResponseState.Text.Trim() == "成功" ? "0" : "1"));
                    list.Add(new SqlPara("ResponsePolicyguid", txtGUID.Text.Trim()));
                    list.Add(new SqlPara("ResponseContent", txtResponseMsg.Text.Trim()));
                    list.Add(new SqlPara("JsonText", txtRequestJson.Text.Trim()));
                    sps = new SqlParasEntity(OperType.Execute, "USP_MODIFY_BILLINSURANCE", list);
                    if (SqlHelper.ExecteNonQuery(sps) > 0)
                    {
                        MsgBox.ShowOK();
                        this.Close();
                    }
                }
                else
                {
                    if (MsgBox.ShowYesNo("只能修改请求失败的投保信息！") == DialogResult.Yes)
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                
                MsgBox.ShowException(ex);
            }
        }



        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
