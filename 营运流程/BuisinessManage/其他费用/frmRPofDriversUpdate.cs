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
using System.Text.RegularExpressions;

namespace ZQTMS.UI
{
    public partial class frmRPofDriversUpdate : BaseForm
    {
        public DataRow dr = null;
        public frmRPofDriversUpdate()
        {
            InitializeComponent();
        }

        //退出
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmRPofDriversUpdate_Load(object sender, EventArgs e)
        {

            CommonClass.SetSite(bsite, false);
            CommonClass.SetSite(Tosite, false);
            WebDate.EditValue = CommonClass.gbdate;
            CommonClass.FormSet(this);
            if (dr != null)
            {
                Amount.EditValue = dr["Amount"];
                RPContent.EditValue = dr["RPContent"];
                CarNO.EditValue = dr["CarNO"];
                DepartureBatch.EditValue = dr["DepartureBatch"];
                DriverName.EditValue = dr["DriverName"];
                bsite.EditValue = dr["BSite"];
                Tosite.EditValue = dr["ToSite"];
                WebDate.EditValue = dr["WebDate"];
                cbMoneyType.EditValue = dr["cbMoneyType"];
                DepartureBatch.Enabled = false;

            }
        }

        //保存
        private void btnSave_Click(object sender, EventArgs e)
        {
            string posPattern = @"^[0-9]+(.[0-9]{1,2})?$";//验证正数正则
            string negPattern = @"^\-[0-9]+(.[0-9]{1,2})?$";//验证负数正则
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("RPofdriverID", dr == null ? Guid.NewGuid() : dr["RPofdriverID"]));
                list.Add(new SqlPara("WebDate", WebDate.Text.Trim()));
                list.Add(new SqlPara("Amount", Convert.ToDecimal(Amount.Text.Trim())));
                list.Add(new SqlPara("DepartureBatch", DepartureBatch.Text.Trim()));
                list.Add(new SqlPara("CarNO", CarNO.Text.Trim()));
                list.Add(new SqlPara("DriverName", DriverName.Text.Trim()));
                list.Add(new SqlPara("RPContent", RPContent.Text.Trim()));
                list.Add(new SqlPara("bsite", bsite.Text.Trim()));
                list.Add(new SqlPara("Tosite", Tosite.Text.Trim()));
                list.Add(new SqlPara("cbMoneyType", cbMoneyType.Text.Trim()));
                list.Add(new SqlPara("Ischeck", UserRight.GetRight("82") ? 0 : 1));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_UPDATE_RPofDriverList", list);
               
                if (cbMoneyType.Text.Trim() == "奖励支出" && !Regex.IsMatch(Amount.Text.Trim(),posPattern))
                {
                    MessageBox.Show("奖励支出必须为正数", "错误");
                    return;
                }
                if (cbMoneyType.Text.Trim() == "代扣罚款" && !Regex.IsMatch(Amount.Text.Trim(),negPattern))
                {
                    MessageBox.Show("代扣罚款必须为负数", "错误");
                    return;
                }
                if (MsgBox.ShowYesNo("确定修改本条记录？") != DialogResult.Yes) return;
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK("修改成功！");
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
