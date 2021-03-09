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
    public partial class frmLongDateAdd : BaseForm
    {
        public DataRow dr = null;
        public frmLongDateAdd()
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
                    LongModels.Properties.Items.Add(VehicleTypeList[i]);
                }
                //VehicleType.SelectedIndex = 0;
            }

            CommonClass.SetSite(LongSite, false);
            CommonClass.SetSite(LongEsite, false);
            CommonClass.FormSet(this);
            if (dr != null)
            {
                LongSite.EditValue = dr["LongSite"];
                LongTime.EditValue = dr["LongTime"];
                LongEsite.EditValue = dr["LongEsite"];
                LongModels.EditValue = dr["LongModels"];
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
                list.Add(new SqlPara("LongId", dr == null ? Guid.NewGuid() : dr["LongId"]));
                list.Add(new SqlPara("LongSite", LongSite.Text.Trim()));
                list.Add(new SqlPara("LongTime", Convert.ToDecimal(LongTime.Text.Trim())));
                list.Add(new SqlPara("LongEsite", LongEsite.Text.Trim()));
                list.Add(new SqlPara("LongModels", LongModels.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_basLongDate", list);
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

        
        
    }
}