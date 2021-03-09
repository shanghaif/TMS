using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList.Nodes;
using ZQTMS.Common;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class CopyToOtherCompany : BaseForm
    {
        public CopyToOtherCompany()
        {
            InitializeComponent();
        }

        public string edbsite;

        private void CopyToOtherCompany_Load(object sender, EventArgs e)
        {
            try
            {
                checkedComboBoxEdit1.EditValue = edbsite;
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("edbsite", edbsite));
                SqlParasEntity sp = new SqlParasEntity(OperType.Query, "QSP_GET_SameSiteToOtherCompany", list);
                DataSet ds = SqlHelper.GetDataSet(sp);
                checkedComboBoxEdit2.Properties.Items.Clear();
                checkedComboBoxEdit2.Properties.DataSource = ds.Tables[0];
                checkedComboBoxEdit2.Properties.DisplayMember = "SiteName";
                checkedComboBoxEdit2.Properties.ValueMember = "SiteCode";
                checkedComboBoxEdit2.RefreshEditValue();
            }
            catch(Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string str = checkedComboBoxEdit2.Text.Trim();
                if (str == "")
                {
                    MsgBox.ShowError("尚未选择站点，请选择站点！");
                    return;
                }
                string[] newstr = str.Split(',');
                List<string> arr = new List<string>();
                for (int i = 0; i < newstr.Length; i++)
                {
                    newstr[i] = newstr[i].Trim();
                    arr.Add(newstr[i]);
                }
                string[] array = arr.ToArray();
                str  = string.Join("@", array);
                str = str + "@";
                if (XtraMessageBox.Show("如果将此科目复制到其它站点,将覆盖其它分公司原来的科目设置,确认是否继续?", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("edbsite", edbsite));
                    list.Add(new SqlPara("str", str));
                    SqlParasEntity sp = new SqlParasEntity(OperType.Execute, "USP_ADD_BASSUBJECT_OtherCopy", list);
                    if (SqlHelper.ExecteNonQuery(sp) > 0)
                    {
                        MsgBox.ShowOK();
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
    }
}
