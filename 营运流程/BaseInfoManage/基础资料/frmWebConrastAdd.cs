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

namespace ZQTMS.UI.BaseInfoManage
{
    public partial class frmWebConrastAdd : BaseForm
    {
        public frmWebConrastAdd()
        {
            InitializeComponent();
        }
        public int operType = 0;
        public string id = "";

        private void frmWebConrastAdd_Load(object sender, EventArgs e)
        {
            GetCompanyId();
            txtPerson.Enabled=false;
            txtWeb.Enabled=false;
            txtPerson.Text = CommonClass.UserInfo.UserName;
            txtWeb.Text = CommonClass.UserInfo.WebName;
            if (id != "")
            { 
                CompanyID.Enabled=false;
                cbbBeginWeb.Enabled = false;
                DataSet ds=GetWebContrast();
                if(ds!=null && ds.Tables[0].Rows.Count>0)
                {
                    CompanyID.Text = ds.Tables[0].Rows[0]["companyid"].ToString();
                    cbbBeginWeb.Text = ds.Tables[0].Rows[0]["BeginWeb"].ToString();
                    cbbEndWeb.Text = ds.Tables[0].Rows[0]["EndWeb"].ToString();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private DataSet GetWebContrast()
        {
            DataSet ds = null;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", id));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WebContrast",list);
                 ds = SqlHelper.GetDataSet(sps);
            }
            catch (Exception ex)
            {
                
                MsgBox.ShowException(ex);
            }
            return ds;
          
        }


        public void GetCompanyId()
        {
            try
            {
               
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_USP_GET_companyid");
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    CompanyID.Properties.Items.Add(dr[0]);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void CompanyID_SelectedIndexChanged(object sender, EventArgs e)
        {
            string company = CompanyID.Text.Trim();
            DataSet ds = GetWebNameByCompanyId(company);
            if (ds == null || ds.Tables.Count == 0) return;
            CompanyID.Properties.Items.Clear();
            cbbEndWeb.Properties.Items.Clear();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                cbbBeginWeb.Properties.Items.Add(dr["WebName"]);
                cbbEndWeb.Properties.Items.Add(dr["WebName"]);
            }

        }

        private DataSet  GetWebNameByCompanyId(string company)
        {
            DataSet ds = null;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("company", company));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GetWebName_By_Company", list);
                 ds = SqlHelper.GetDataSet(sps);
              
            }
            catch (Exception ex) 
            {

                MsgBox.ShowException(ex);
            }
            return ds;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("webName",cbbBeginWeb.Text.Trim()));
                list.Add(new SqlPara("endWeb", cbbEndWeb.Text.Trim()));
                list.Add(new SqlPara("company",CompanyID.Text.Trim()));
                list.Add(new SqlPara("person",txtPerson.Text.Trim()));
                list.Add(new SqlPara("operWeb", txtWeb.Text.Trim()));
                list.Add(new SqlPara("operType",operType));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_WebContrast", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK("操作成功！");
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
