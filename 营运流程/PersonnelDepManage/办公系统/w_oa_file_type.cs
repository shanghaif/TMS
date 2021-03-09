using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class w_oa_file_type : BaseForm
    {
        public w_oa_file_type()
        {
            InitializeComponent();
        }

        public CheckedComboBoxEdit edit;

        private void b_oa_file_type_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < edit.Properties.Items.Count; i++)
            {
                listBoxControl1.Items.Add(edit.Properties.Items[i].ToString());
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string type = textEdit1.Text.Trim();
            if (type == "")
            {
                MsgBox.ShowOK("请填写类型名称!");
                textEdit1.Focus();
                return;
            }
            if (listBoxControl1.FindStringExact(type) >= 0)
            {
                MsgBox.ShowOK("您添加的类型已存在!");
                textEdit1.Focus();
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("type", type));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_OA_FILE_TYPE", list)) == 0) return;
                listBoxControl1.Items.Add(type);
                edit.Properties.Items.Add(type);
                textEdit1.Text = "";
                textEdit1.Focus();
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (listBoxControl1.SelectedItem == null)
            {
                MsgBox.ShowOK("请选择需要删除的文档类型!");
            }
            string type = listBoxControl1.SelectedItem.ToString();
            if (MsgBox.ShowYesNo(string.Format("确定删除{0}?", type)) == DialogResult.No) return;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("type", type));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_DELETE_OA_FILE_TYPE", list)) == 0) return;
                listBoxControl1.Items.Remove(type);
                edit.Properties.Items.Remove(type);
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }
    }
}