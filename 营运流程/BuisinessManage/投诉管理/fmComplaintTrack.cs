using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using ZQTMS.Tool;
using ZQTMS.Common;
using System.Windows.Forms;
using ZQTMS.SqlDAL;
using DevExpress.XtraGrid.Views.Grid;

namespace ZQTMS.UI
{
    public partial class fmComplaintTrack : BaseForm
    {
        public string id = "";
        public GridView gv = null;
        public fmComplaintTrack()
        {
            InitializeComponent();
        }

        private void fmComplaintTrack_Load(object sender, EventArgs e)
        {
            string siteName = CommonClass.UserInfo.SiteName;

            CommonClass.SetWeb(ResponWeb1, siteName);
            CommonClass.SetWeb(ResponWeb2, siteName);
            CommonClass.SetWeb(ResponWeb3, siteName);
            if (gv != null)
            {
                id = gv.GetRowCellValue(gv.FocusedRowHandle, "Qid").ToString();

                ResponWeb1.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "ResponWeb1").ToString();
                ResponMan1.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "ResponMan1").ToString();
                Fines1.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "Fines1").ToString();

                ResponWeb2.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "ResponWeb2").ToString();
                ResponMan2.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "ResponMan2").ToString();
                Fines2.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "Fines2").ToString();

                ResponWeb3.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "ResponWeb3").ToString();
                ResponMan3.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "ResponMan3").ToString();
                Fines3.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "Fines3").ToString();

                ComplainType.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "ComplainType").ToString(); ;
                ResponDivideDate.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "ResponDivideDate").ToString();
                ResponDivideMan.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "ResponDivideMan").ToString();
            }
        }

        private void btnDivideRespon_Click(object sender, EventArgs e)
        {
            try
            {

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("Qid", id));
                list.Add(new SqlPara("ComplainType", ComplainType.Text.Trim()));

                list.Add(new SqlPara("ResponWeb1", ResponWeb1.Text.Trim()));
                list.Add(new SqlPara("ResponMan1", ResponMan1.Text.Trim()));
                list.Add(new SqlPara("Fines1", Fines1.Text.Trim() == "" ? "0" : Fines1.Text.Trim()));

                list.Add(new SqlPara("ResponWeb2", ResponWeb2.Text.Trim()));
                list.Add(new SqlPara("ResponMan2", ResponMan2.Text.Trim()));
                list.Add(new SqlPara("Fines2", Fines2.Text.Trim() == "" ? "0" : Fines2.Text.Trim()));

                list.Add(new SqlPara("ResponWeb3", ResponWeb3.Text.Trim()));
                list.Add(new SqlPara("ResponMan3", ResponMan3.Text.Trim()));
                list.Add(new SqlPara("Fines3", Fines3.Text.Trim() == "" ? "0" : Fines3.Text.Trim()));

                list.Add(new SqlPara("ResponDivideMan", ResponDivideMan.Text.Trim()));
                list.Add(new SqlPara("ResponDivideDate", ResponDivideDate.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_MODIFY_BILLCUSTQUERRYLOG_ByResponDivide", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {

                    gv.SetRowCellValue(gv.FocusedRowHandle, "ComplainType", ComplainType.Text.Trim());

                    gv.SetRowCellValue(gv.FocusedRowHandle, "ResponWeb1", ResponWeb1.Text);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "ResponMan1", ResponMan1.Text);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "Fines1", Fines1.Text.Trim() == "" ? "0" : Fines1.Text.Trim());

                    gv.SetRowCellValue(gv.FocusedRowHandle, "ResponWeb2", ResponWeb2.Text);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "ResponMan2", ResponMan2.Text);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "Fines2", Fines2.Text.Trim() == "" ? "0" : Fines2.Text.Trim());

                    gv.SetRowCellValue(gv.FocusedRowHandle, "ResponWeb3", ResponWeb3.Text);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "ResponMan3", ResponMan3.Text);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "Fines3", Fines3.Text.Trim() == "" ? "0" : Fines3.Text.Trim());

                    gv.SetRowCellValue(gv.FocusedRowHandle, "ResponDivideMan", ResponDivideMan.Text);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "ResponDivideDate", ResponDivideDate.Text);

                    MsgBox.ShowOK();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void btnDivideResponCancel_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != MsgBox.ShowYesNo("确定取消责任划分？")) return;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("Qid", id));
                list.Add(new SqlPara("ComplainType", ComplainType.Text.Trim()));

                list.Add(new SqlPara("ResponWeb1", DBNull.Value));
                list.Add(new SqlPara("ResponMan1", DBNull.Value));
                list.Add(new SqlPara("Fines1", DBNull.Value));

                list.Add(new SqlPara("ResponWeb2", DBNull.Value));
                list.Add(new SqlPara("ResponMan2", DBNull.Value));
                list.Add(new SqlPara("Fines2", DBNull.Value));

                list.Add(new SqlPara("ResponWeb3", DBNull.Value));
                list.Add(new SqlPara("ResponMan3", DBNull.Value));
                list.Add(new SqlPara("Fines3", DBNull.Value));

                list.Add(new SqlPara("ResponDivideMan", DBNull.Value));
                list.Add(new SqlPara("ResponDivideDate", DBNull.Value));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_MODIFY_BILLCUSTQUERRYLOG_ByResponDivide", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    gv.SetRowCellValue(gv.FocusedRowHandle, "ResponWeb1", DBNull.Value);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "ResponMan1", DBNull.Value);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "Fines1", DBNull.Value);

                    gv.SetRowCellValue(gv.FocusedRowHandle, "ResponWeb2", DBNull.Value);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "ResponMan2", DBNull.Value);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "Fines2", DBNull.Value);

                    gv.SetRowCellValue(gv.FocusedRowHandle, "ResponWeb3", DBNull.Value);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "ResponMan3", DBNull.Value);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "Fines3", DBNull.Value);

                    gv.SetRowCellValue(gv.FocusedRowHandle, "ResponDivideMan", DBNull.Value);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "ResponDivideDate", DBNull.Value);


                    MsgBox.ShowOK();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }
    }
}
