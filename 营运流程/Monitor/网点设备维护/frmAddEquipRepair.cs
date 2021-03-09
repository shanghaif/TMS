using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmAddEquipRepair : BaseForm
    {
        public frmAddEquipRepair()
        {
            InitializeComponent();
        }

        private void frmAddEquipRepair_Load(object sender, EventArgs e)
        {
            CommonClass.SetCause(cbbCause, false);
            CommonClass.SetUser(cbbCharger,cbbWeb.Text.Trim(), false);
        }

        private void cbbCause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(cbbArea, cbbCause.Text.Trim(), false);
            CommonClass.SetWeb(cbbWeb, cbbArea.Text.Trim(), false);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //USP_ADD_WebEquipRepair
                List<SqlPara> list = new List<SqlPara>();
                if (!StringHelper.IsNumberId(txtPerPrice.Text.Trim()))
                {
                    MsgBox.ShowOK("设备单价输入有误，请输入数字");
                    return;
                }
                string equipNums = txtEquipNos.Text.Trim();
                if (equipNums.Contains("，"))
                {
                    MsgBox.ShowOK("设备编号中包含中文逗号，请换成英文逗号");
                    return;
                }
                string[] equipNo = equipNums.Split(',');
                string newequipNums = "";
                if (equipNo.Length > 0)
                {
                    for (int i = 0; i < equipNo.Length; i++)
                    {
                        newequipNums += equipNo[i] + ",";
                    }
                }
                list.Add(new SqlPara("cbbCause",cbbCause.Text.Trim()));
                list.Add(new SqlPara("cbbArea",cbbArea.Text.Trim()));
                list.Add(new SqlPara("cbbWeb",cbbWeb.Text.Trim()));
                list.Add(new SqlPara("EquipName",txtEquipName.Text.Trim()));
                list.Add(new SqlPara("PerPrice", txtPerPrice.Text.Trim()));
                list.Add(new SqlPara("MerName",txtMerName.Text.Trim()));
                list.Add(new SqlPara("Charger", cbbCharger.Text.Trim()));                
                list.Add(new SqlPara("EquipNos", newequipNums));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_WebEquipRepair", list)) > 0) {

                    MsgBox.ShowOK();
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}