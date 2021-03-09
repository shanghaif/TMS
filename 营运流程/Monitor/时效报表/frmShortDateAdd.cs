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

namespace ZQTMS.UI
{
    public partial class frmShortDateAdd : BaseForm
    {
        public DataRow dr = null;
        public frmShortDateAdd()
        {
            InitializeComponent();
        }

        private void fmDirectSendFeeAdd_Load(object sender, EventArgs e)
        {
            string[] VehicleTypeList = CommonClass.Arg.VehicleType.Split(',');
            if (VehicleTypeList.Length > 0)
            {
                for (int i = 0; i < VehicleTypeList.Length; i++)
                {
                    ShortModels.Properties.Items.Add(VehicleTypeList[i]);
                }
                //VehicleType.SelectedIndex = 0;
            }
          
            CommonClass.SetSite(ShortSite, false);
            ShortSite.EditValue = CommonClass.UserInfo.SiteName;
            CommonClass.SetSite(ShortEsite, false);

            ShortEsite.EditValue = CommonClass.UserInfo.SiteName;
           
            if (ShortEsite.Text.Trim() == "全部") ShortEsite.Text = "";
            if (ShortSite.Text.Trim() == "全部") ShortSite.Text = "";
            CommonClass.FormSet(this);
            if (dr != null)
            {
                ShortSite.EditValue = dr["ShortSite"];
                ShortTime.EditValue = dr["ShortTime"];
                ShortEsite.EditValue = dr["ShortEsite"];
                ShortModels.EditValue = dr["ShortModels"];
                ShortEweb.EditValue = dr["ShortEweb"];
                ShortWeb.EditValue = dr["ShortWeb"];
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ShortId", dr == null ? Guid.NewGuid() : dr["ShortId"]));
               
                list.Add(new SqlPara("ShortTime", Convert.ToDecimal(ShortTime.Text.Trim())));
            
                list.Add(new SqlPara("ShortModels", ShortModels.Text.Trim()));
             
                list.Add(new SqlPara("ShortSite",ShortSite.Text.Trim()));
                list.Add(new SqlPara("ShortWeb",ShortWeb .Text.Trim()));
                list.Add(new SqlPara("ShortEsite",ShortEsite .Text.Trim()));
                list.Add(new SqlPara("ShortEweb",ShortEweb .Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_basShortDate", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
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

        private void labelControl6_Click(object sender, EventArgs e)
        {

        }

        private void ShortEsiteChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(ShortEweb, ShortEsite.EditValue.ToString(),false);
           
        }

        private void ShortSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(ShortWeb, ShortSite.EditValue.ToString(),false);
            
        }

        
        
    }
}