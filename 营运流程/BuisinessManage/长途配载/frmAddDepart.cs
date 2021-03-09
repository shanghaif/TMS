using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Common;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmAddDepart : Form
    {
        public string siteOne = "";
        public string siteTwo = "";
        public string batch = "";

        public frmAddDepart()
        {
            InitializeComponent();
        }

        public string SiteOne
        {
            get { return siteOne; }
            set { siteOne = value; }
        }

        public string SiteTwo
        {
            get { return siteTwo; }
            set { siteTwo = value; }
        }

        public string Batch
        {
            get { return batch; }
            set { batch = value; }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int flag = 0;
                string carStartType = "";
                if (!string.IsNullOrEmpty(destiSite2.Text.Trim()))
                {
                    carStartType = "两地车";
                }
                else
                {
                    carStartType = "一地车";
                }

                if (string.IsNullOrEmpty(destiSite1.Text.Trim()))
                {
                    MsgBox.ShowError("①目的站点不能为空！");
                    return;
                }
                if (string.IsNullOrEmpty(desitiWeb1.Text.Trim()))
                {
                    MsgBox.ShowError("①目的网点不能为空！");
                    return;
                }

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", Guid.NewGuid()));
                list.Add(new SqlPara("destiSite1", destiSite1.Text.Trim()));
                list.Add(new SqlPara("desitiWeb1", desitiWeb1.Text.Trim()));
                list.Add(new SqlPara("dateEdit1", dateEdit1.Text.Trim()));
                list.Add(new SqlPara("dateEdit2", dateEdit2.Text.Trim()));
                list.Add(new SqlPara("operSendTime", operSendTime1.DateTime));
                list.Add(new SqlPara("batchNo", Batch));
                list.Add(new SqlPara("carStartType", carStartType));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLVEHICLESTAR_TWO", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0) flag = 1;

                if (!string.IsNullOrEmpty(destiSite2.Text.Trim()))
                {
                    if (string.IsNullOrEmpty(desitiWeb2.Text.Trim()))
                    {
                        MsgBox.ShowError("②目的网点不能为空！");
                        return;
                    }
                    List<SqlPara> list_1 = new List<SqlPara>();
                    list_1.Add(new SqlPara("ID", Guid.NewGuid()));
                    list_1.Add(new SqlPara("destiSite2", destiSite2.Text.Trim()));
                    list_1.Add(new SqlPara("desitiWeb2", desitiWeb2.Text.Trim()));
                    list_1.Add(new SqlPara("dateEdit3", dateEdit3.Text.Trim()));
                    list_1.Add(new SqlPara("dateEdit4", dateEdit4.Text.Trim()));
                    list_1.Add(new SqlPara("batchNo", Batch));
                    list_1.Add(new SqlPara("operSendTime", operSendTime2.DateTime));
                    list_1.Add(new SqlPara("carStartType", carStartType));
                    SqlParasEntity SPS = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLVEHICLESTAR_Three", list_1);
                    if (SqlHelper.ExecteNonQuery(SPS) > 0) flag = 1;
                 }

                if (flag==1)
                {
                    MsgBox.ShowOK();
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                string[] arr = ex.Message.Split('：');
                MsgBox.ShowError(arr[arr.Length - 1]);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddDepart_Load(object sender, EventArgs e)
        {
            dateEdit1.DateTime = CommonClass.gcdate;
            dateEdit2.DateTime = CommonClass.gcdate;
            dateEdit3.DateTime = CommonClass.gcdate;
            dateEdit4.DateTime = CommonClass.gcdate;
            operSendTime1.DateTime = CommonClass.gcdate;
            operSendTime2.DateTime = CommonClass.gcdate;

            destiSite1.Text = SiteOne;
            destiSite2.Text = SiteTwo;

            CommonClass.SetSite(destiSite1, false);
            CommonClass.SetSite(destiSite2, false);

        }

        private void destiSite1_TextChanged(object sender, EventArgs e)
        {
            desitiWeb1.Properties.Items.Clear();
            string[] site = destiSite1.Text.Trim().Split(',');
            for (int i = 0; i < site.Length; i++)
            {
                DataRow[] dr = CommonClass.dsWeb.Tables[0].Select("SiteName='" + site[i] + "'");
                for (int k = 0; k < dr.Length; k++)
                {
                    desitiWeb1.Properties.Items.Add(dr[k]["WebName"]);
                }
            }
        }

        private void destiSite2_TextChanged(object sender, EventArgs e)
        {
            desitiWeb2.Properties.Items.Clear();
            string[] site = destiSite2.Text.Trim().Split(',');
            for (int i = 0; i < site.Length; i++)
            {
                DataRow[] dr = CommonClass.dsWeb.Tables[0].Select("SiteName='" + site[i] + "'");
                for (int k = 0; k < dr.Length; k++)
                {
                    desitiWeb2.Properties.Items.Add(dr[k]["WebName"]);
                }
            }
        }
    }
}
