using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.Common;
using DevExpress.XtraEditors.Controls;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmDePartAdd : BaseForm
    {
        public DataRow dr = null;
        public frmDePartAdd()
        {
            InitializeComponent();
        }

        private void frmDePartAdd_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);

            //AreaAll();
            CommonClass.SetArea(DepArea, "全部", false);//填充当前事业部的大区
            DepArea.Text = CommonClass.UserInfo.AreaName;
            if (CommonClass.UserInfo.SiteName.Contains("总部"))
            {
                DepRight.Enabled = true;
                CommonClass.SetSelectIndexByValue("0", DepRight);
            }

            if (dr != null)
            {
                //新增后不给修改
                DepArea.Enabled=DepName.Enabled = DepCode.Enabled = false;

                // DepId.EditValue = dr["DepId"];
                DepArea.EditValue = dr["DepArea"];
                DepName.EditValue = dr["DepName"];
                DepCode.EditValue = dr["DepCode"];
                DepMan.EditValue = dr["DepMan"];
                DepPhone.EditValue = dr["DepPhone"];
                DepRemark.EditValue = dr["DepRemark"];
                CommonClass.SetSelectIndexByValue(dr["DepRight"].ToString().Trim(), DepRight);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepId", dr == null ? Guid.NewGuid() : dr["DepId"]));
                list.Add(new SqlPara("DepArea", DepArea.Text.Trim()));
                list.Add(new SqlPara("DepName", DepName.Text.Trim()));
                list.Add(new SqlPara("DepCode", DepCode.Text.Trim()));
                list.Add(new SqlPara("DepMan", DepMan.Text.Trim()));
                list.Add(new SqlPara("DepPhone", DepPhone.Text.Trim()));
                list.Add(new SqlPara("DepRemark", DepRemark.Text.Trim()));
                list.Add(new SqlPara("DepRight", DepRight.EditValue == null ? 0 : DepRight.EditValue));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BASDEPART", list);
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

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void AreaAll()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASAREA", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds != null && ds.Tables.Count != 0)
                {
                    if (ds != null && ds.Tables.Count != 0)
                    {
                        DepArea.Properties.Items.Clear();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)

                            DepArea.Properties.Items.Add(ds.Tables[0].Rows[i]["AreaName"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

     
    }
}
