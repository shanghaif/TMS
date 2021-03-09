using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DevExpress.XtraEditors;
using System.Data.OleDb;
using System.Collections;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Columns;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmBankInfoAdd : BaseForm
    {
        public frmBankInfoAdd()
        {
            InitializeComponent();
        }

        public int id = -1;
        DataSet dsarea = new DataSet();
        public DataSet dsshipper = new DataSet();//汇款客户资料 打开银行信息平台就开始提取

        private void frmBankInfoAdd_Load(object sender, EventArgs e)
        {
            ZQTMS.Common.CommonClass.AreaManager.FillCityToImageComBoxEdit(edsheng, "0");
            edsheng.SelectedIndexChanged += new System.EventHandler(this.edsheng_SelectedIndexChanged);
 
            if (id < 0)
            {
                clear();
            }
            else
            {
                fill();
            }
        }

        private void fill()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("id", id));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BANKINFO_ByID", list);

                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                DataRow dr = ds.Tables[0].Rows[0];

                edbankman.EditValue = dr["bankman"];
                edbankcode.EditValue = dr["bankcode"];
                edbankname.EditValue = dr["bankname"];
                edbankchild.EditValue = dr["bankchild"];

                ZQTMS.Common.CommonClass.SetSelectIndex(dr["sheng"].ToString().Trim(), edsheng);
                ZQTMS.Common.CommonClass.SetSelectIndex(dr["city"].ToString().Trim(), edcity);
                edopertype.EditValue = dr["opertype"];
                edouttype.EditValue = dr["outtype"];
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void clear()
        {
            foreach (Control item in this.Controls)
            {
                if (item.GetType() == typeof(TextEdit) || item.GetType() == typeof(ComboBoxEdit))
                {
                    item.Text = "";
                }
            }
            id = -1;
        }

        /*
        private void comboBoxEdit2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                edcity.Properties.Items.Clear();
                edcity.Text = "";
                edcity.Tag = null;

                if (edsheng.Text.Trim() == "")
                {
                    edcity.Text = "";
                    return;
                }

                if (edsheng.SelectedIndex < 0) return;
                int id = Convert.ToInt32(((ArrayList)edsheng.Tag)[edsheng.SelectedIndex]);

                DataRow[] dr = dsarea.Tables[0].Select("parentid=" + id + "");
                ArrayList arr = new ArrayList();
                for (int i = 0; i < dr.Length; i++)
                {
                    edcity.Properties.Items.Add(dr[i]["city"]);
                    arr.Add(dr[i]["ID"]);
                }
                edcity.Tag = arr;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
         */

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string bankman = edbankman.Text.Trim();
            if (bankman == "")
            {
                MsgBox.ShowOK("必须填写开户姓名!");
                edbankman.Focus();
                return;
            }

            string bankcode = edbankcode.Text.Trim();
            if (bankcode == "")
            {
                MsgBox.ShowOK("必须填写银行账号!");
                edbankcode.Focus();
                return;
            }
            string bankname = edbankname.Text.Trim();
            if (bankname == "")
            {
                MsgBox.ShowOK("必须填写银行名称!");
                edbankname.Focus();
                return;
            }

            string bankchild = edbankchild.Text.Trim();

            string sheng = edsheng.Text.Trim();
            if (sheng == "")
            {
                MsgBox.ShowOK("必须填写所属省份!");
                edsheng.Focus();
                return;
            }

            string city = edcity.Text.Trim();
            if (city == "")
            {
                MsgBox.ShowOK("必须填写所属用户城市!");
                edcity.Focus();
                return;
            }

            string opertype = edopertype.Text.Trim();
            if (opertype == "")
            {
                MsgBox.ShowOK("必须填写转账类型!");
                edopertype.Focus();
                return;
            }

            try
            {

                List<SqlPara> list = new List<SqlPara>();

                list.Add(new SqlPara("id", id));
                list.Add(new SqlPara("bankman", edbankman.Text.Trim()));
                list.Add(new SqlPara("bankcode", edbankcode.Text.Trim()));
                list.Add(new SqlPara("bankname", edbankname.Text.Trim())); ;
                list.Add(new SqlPara("sheng", edsheng.Text.Trim()));  //4

                list.Add(new SqlPara("city", edcity.Text.Trim()));
                list.Add(new SqlPara("opertype", edopertype.Text.Trim()));
                list.Add(new SqlPara("outtype", edouttype.Text.Trim()));
                list.Add(new SqlPara("bankchild", edbankchild.Text.Trim()));


                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "USP_ADD_BANKINFO", list);
                SqlHelper.ExecteNonQuery(sps);
                MsgBox.ShowOK();
                clear();
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void edsheng_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZQTMS.Common.CommonClass.AreaManager.FillCityToImageComBoxEdit(edcity, edsheng.EditValue);
        }
    }
}