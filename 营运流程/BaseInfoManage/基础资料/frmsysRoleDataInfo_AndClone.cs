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
using DevExpress.XtraEditors.Controls;

namespace ZQTMS.UI.BaseInfoManage
{
    public partial class frmsysRoleDataInfo_AndClone : BaseForm
    {
        public frmsysRoleDataInfo_AndClone()
        {
            InitializeComponent();
        }
        public DataRow dr = null;
        private void frmsysRoleDataInfo_AndClone_Load(object sender, EventArgs e)
        {
            GetUserRightTag();
            SetWeb(WebName, false);
            if (this.dr != null)
            {
                SetCheckedItems(dr["GRCode"].ToString());
                WebName.EditValue = dr["SiteNames"];
                Remark.EditValue = dr["Remark"];
            }
        }
        //权限组名称
        private void GetUserRightTag()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_UserRightTag", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                GRCode.Properties.Items.Clear();

                GRCode.Properties.DataSource = ds.Tables[0];
                GRCode.Properties.DisplayMember = "GRName";
                GRCode.Properties.ValueMember = "GRCode";
                GRCode.RefreshEditValue();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //网点
        public static void SetWeb(CheckedComboBoxEdit cb, bool isall)
        {
            if (CommonClass.dsWeb == null || CommonClass.dsWeb.Tables.Count == 0) return;
            try
            {
                for (int i = 0; i < CommonClass.dsWeb.Tables[0].Rows.Count; i++)
                {
                    cb.Properties.Items.Add(CommonClass.dsWeb.Tables[0].Rows[i]["WebName"]);
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("GUID", dr == null ? Guid.NewGuid() : dr["GUID"]));
                list.Add(new SqlPara("GRCode", GRCode.EditValue.ToString().Replace(" ", "")));
                list.Add(new SqlPara("GRName", GRCode.Text.Trim().Replace(" ", "")));
                list.Add(new SqlPara("Remark", Remark.Text.Trim()));
                list.Add(new SqlPara("SiteNames", WebName.Text.Trim().Replace(" ", "")));
                SqlParasEntity sql = new SqlParasEntity(OperType.Execute, "USP_sysRoleDataInfoClone", list);
                if (SqlHelper.ExecteNonQuery(sql) > 0)
                {
                    MsgBox.ShowOK();
                    this.Close();
                }

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
        private void SetCheckedItems(string value)
        {
            string[] arr1 = value.Split(',');
            for (int i = 0; i < arr1.Length; i++)
            {
                foreach (CheckedListBoxItem item in GRCode.Properties.Items)
                {
                    if (item.Value.ToString() == arr1[i])
                    {
                        item.CheckState = CheckState.Checked;
                        continue;
                    }
                }
            }
        }
    }
}