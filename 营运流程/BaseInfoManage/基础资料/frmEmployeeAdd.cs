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
    public partial class frmEmployeeAdd : BaseForm
    {
        public DataRow dr = null;
        public frmEmployeeAdd()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("EmpID", (string.IsNullOrEmpty(EmpID.Text.Trim()) ? Guid.NewGuid().ToString() : EmpID.Text.Trim())));
                list.Add(new SqlPara("EmpName", EmpName.Text.Trim()));
                list.Add(new SqlPara("EmpNO", EmpNO.Text.Trim()));
                list.Add(new SqlPara("EmpSite", EmpSite.Text.Trim()));
                list.Add(new SqlPara("EmpDept", EmpDept.Text.Trim()));
                list.Add(new SqlPara("EmpPosition", EmpPosition.Text.Trim()));
                list.Add(new SqlPara("EmpPost", EmpPost.Text.Trim()));
                list.Add(new SqlPara("EmpWeb", EmpWeb.Text.Trim()));
                list.Add(new SqlPara("EmpCuase", EmpCuase.Text.Trim()));
                list.Add(new SqlPara("EmpArea", EmpArea.Text.Trim()));


                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BASEMPLOYEE", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
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

        private void frmAddTitle_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);

            CommonClass.SetSite(EmpSite, false);
            CommonClass.SetCause(EmpCuase, false);
            EmpCuase.Text = CommonClass.UserInfo.CauseName;//只能修改当前事业部

            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASJOBS");
                EmpPosition.Properties.Items.Clear();

                foreach (DataRow dr_exam in SqlHelper.GetDataSet(sps).Tables[0].Rows)
                {
                    EmpPosition.Properties.Items.Add(dr_exam["JobName"]);
                }

                SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_BASTITLE");
                EmpPost.Properties.Items.Clear();

                foreach (DataRow dr_exam1 in SqlHelper.GetDataSet(sps1).Tables[0].Rows)
                {
                    EmpPost.Properties.Items.Add(dr_exam1["TitName"]);
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.ToString());
            }

            if (CommonClass.UserInfo.SiteName.Contains("总部")) EmpCuase.Enabled = true;

            if (dr != null)
            {
                EmpName.Enabled = false;

                EmpID.EditValue = dr["EmpID"];
                EmpName.EditValue = dr["EmpName"];
                EmpNO.EditValue = dr["EmpNO"];
                EmpCuase.EditValue = dr["EmpCuase"];
                EmpDept.EditValue = dr["EmpDept"];
                EmpPosition.EditValue = dr["EmpPosition"];
                EmpPost.EditValue = dr["EmpPost"];
                EmpArea.EditValue = dr["EmpArea"];
                EmpWeb.EditValue = dr["EmpWeb"];
                EmpSite.EditValue = dr["EmpSite"];
            }
        }

        private void EmpCuase_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void EmpArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetDep(EmpDept, EmpArea.Text.Trim(), false);
        }

        private void EmpSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(EmpWeb, EmpSite.Text.Trim(), false);
        }

        private void EmpCuase_EditValueChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(EmpArea, EmpCuase.Text.Trim(), false);
        }
    }
}