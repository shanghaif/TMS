using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using DevExpress.XtraGrid.Views.Grid;
using ZQTMS.Common;
using ZQTMS.SqlDAL;


namespace ZQTMS.UI
{
    public partial class frmAreaDivideUpt : BaseForm
    {
        public string Id = "";
        public GridView gv;

        public frmAreaDivideUpt()
        {
            InitializeComponent();
        }

        private void AreaDivideUpt_Load(object sender, EventArgs e)
        {
            if (Id == "") return;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("MiddleSiteId", Id);
            string strSQL = string.Empty;
            frmOrgUnit frm = new frmOrgUnit();
            if (frm != null)
            {
                frm.GetCompanyId_By_Proc(CommonClass.UserInfo.companyid, "QSP_GET_BASMIDDLESITE_ByID", dic, ref strSQL);
            }
            if (strSQL == "无效存储过程名称")
            {
                MsgBox.ShowOK(strSQL);
                return;
            }
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("strSQL", strSQL));

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "USP_BASMIDDLESITE_OptionSQL", list));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                Id = "";
                return;
            }
            DataRow dr = ds.Tables[0].Rows[0];
            coverageZoneTe.EditValue = dr["CoverageZone"];
            OptCoverageCbe.EditValue = dr["OptCoverage"];
            middlePartnerTe.EditValue = dr["MiddlePartner"];
            CommonClass.SetWeb(sbjWebCbe, dr["SiteName"] as string);
            sbjWebCbe.Properties.Items.Add("");
            sbjWebCbe.EditValue = dr["SbjWeb"];
        }

        private void btnConcel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("MiddleSiteId", Id);
            dic.Add("CoverageZone", coverageZoneTe.Text.Trim());
            dic.Add("OptCoverage", OptCoverageCbe.Text.Trim());
            dic.Add("SbjWeb", sbjWebCbe.Text.Trim());
            dic.Add("MiddlePartner", middlePartnerTe.Text.Trim());

            string strSQL = string.Empty;
            frmOrgUnit frm = new frmOrgUnit();
            if (frm != null)
            {
                frm.GetCompanyId_By_Proc(CommonClass.UserInfo.companyid, "USP_UPDATE_BASMIDDLE_AREA", dic, ref strSQL);
            }
            if (strSQL == "无效存储过程名称")
            {
                MsgBox.ShowOK(strSQL);
                return;
            }
            List<SqlPara> list = new List<SqlPara>();
            //list.Add(new SqlPara("MiddleSiteId", Id));
            //list.Add(new SqlPara("CoverageZone", coverageZoneTe.Text.Trim()));
            //list.Add(new SqlPara("OptCoverage", OptCoverageCbe.Text.Trim()));
            //list.Add(new SqlPara("SbjWeb", sbjWebCbe.Text.Trim()));
            //list.Add(new SqlPara("MiddlePartner", middlePartnerTe.Text.Trim()));
            list.Add(new SqlPara("strSQL", strSQL));

            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_BASMIDDLESITE_OptionSQL", list)) == 0) return;

            int rowhandle = gv.FocusedRowHandle;
            if (rowhandle >= 0)
            {
                gv.SetRowCellValue(rowhandle, "CoverageZone", coverageZoneTe.Text.Trim());
                gv.SetRowCellValue(rowhandle, "OptCoverage", OptCoverageCbe.Text.Trim());
                gv.SetRowCellValue(rowhandle, "SbjWeb", sbjWebCbe.Text.Trim());
                gv.SetRowCellValue(rowhandle, "MiddlePartner", middlePartnerTe.Text.Trim());

            }
            MsgBox.ShowOK("保存成功");
            this.Close();
        }
    }
}
