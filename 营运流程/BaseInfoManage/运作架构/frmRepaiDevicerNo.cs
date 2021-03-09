using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI.BaseInfoManage
{
    public partial class frmRepaiDevicerNo : BaseForm
    {
        public string webId = "", webCode = "", webName = "";
        public frmRepaiDevicerNo()
        {
            InitializeComponent();
        }
        public frmRepaiDevicerNo(string _webId,string _webCode,string _webName)
        {
            this.webId = _webId;
            this.webCode = _webCode;
            this.webName = _webName;
        }
        private void frmRepaiDevicerNo_Load(object sender, EventArgs e)
        {
            CommonClass.SetUser(txtCharger, webName, false);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtDevicerName.Text))
                {
                    MsgBox.ShowOK("请输入设备名称！");
                    return;
                }
                if (string.IsNullOrEmpty(txtDevicerAmount.Text))
                {
                    MsgBox.ShowOK("请输入设备单价！");
                    return;
                }
                //if (string.IsNullOrEmpty(txtApplyMer.Text))
                //{
                //    MsgBox.ShowOK("请输入供应商！");
                //    return;
                //}
                //if (string.IsNullOrEmpty(txtCharger.Text))
                //{
                //    MsgBox.ShowOK("请输入负责人！");
                //    return;
                //}
                if (string.IsNullOrEmpty(txtDevicerNos.Text))
                {
                    MsgBox.ShowOK("请输入设备编号！");
                    return;
                }
                if (txtDevicerNos.Text.Contains("，"))
                {
                    MsgBox.ShowOK("设备单号里面包含中文逗号，请修改为英文逗号！");
                    return;
                }

                string DNs = txtDevicerNos.Text.Trim();
                string[] tdn = DNs.Split(',');
                string newStr = "";
                if (tdn.Length > 0)
                {
                    for (int j = 0; j < tdn.Length; j++)
                    {
                        newStr += tdn[j].Trim() + ",";
                    }
                }

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("webid", webId));
                list.Add(new SqlPara("webcode", webCode));
                list.Add(new SqlPara("webName", webName));
                list.Add(new SqlPara("equipcode", newStr));
                list.Add(new SqlPara("equipname", txtDevicerName.Text));
                list.Add(new SqlPara("eqprice", txtDevicerAmount.Text));
                list.Add(new SqlPara("EqManufacturer", txtApplyMer.Text));
                list.Add(new SqlPara("EqOpeter", txtCharger.Text));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_DevicerNoRepair",list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
