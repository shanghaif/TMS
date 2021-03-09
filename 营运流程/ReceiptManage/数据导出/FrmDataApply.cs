using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using KMS.Tool;
using KMS.SqlDAL;
using KMS.Common;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using System.IO;
using System.Threading;
using System.Web;


namespace KMS.UI
{
    public partial class FrmDataApply :BaseForm
    {

        public string modelName { get; set; }
        public int statu=0;
        public string remarks = "";

        public FrmDataApply()
        { 
            InitializeComponent();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (bdate.DateTime > edate.DateTime)
            {
                XtraMessageBox.Show("开始日期不能大于结束日期", "日期选择错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("t1", bdate.DateTime));
            list.Add(new SqlPara("t2", edate.DateTime));

            list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text.Trim()));
            list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text.Trim()));

            list.Add(new SqlPara("StartSite", StartSite.Text.Trim() == "全部" ? "%%" : StartSite.Text.Trim()));
            list.Add(new SqlPara("TransferSite", TransferSite.Text.Trim() == "全部" ? "%%" : TransferSite.Text.Trim()));
            list.Add(new SqlPara("BegWeb", BegWeb.Text.Trim() == "全部" ? "%%" : BegWeb.Text.Trim()));
            list.Add(new SqlPara("BillMan", BillMan.Text.Trim() == "全部" ? "%%" : BillMan.Text.Trim()));
            try
            {
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "USP_Add_ExportDataApply", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    MsgBox.ShowOK("操作成功!");
                    this.Close();
                }
                else
                {
                    MsgBox.ShowOK("操作失败!");
                    return;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        
        }

        private void FrmDataApply_Load(object sender, EventArgs e)
        {
            textEdit1.Text = modelName;
            applypeople.Text = CommonClass.UserInfo.UserName;
            CommonClass.SetSite(StartSite, true);
            CommonClass.SetSite(TransferSite, true);
            CommonClass.SetWeb(BegWeb,true);
            CommonClass.SetCause(CauseName, true);
            CommonClass.SetArea(AreaName,CauseName.Text);
            CommonClass.SetUser(BillMan, BegWeb.Text);
            bdate.DateTime = CommonClass.gbdate.AddDays(-1);
            edate.DateTime = CommonClass.gedate;
            CauseName.Text = CommonClass.UserInfo.CauseName;
            AreaName.Text = CommonClass.UserInfo.AreaName;
            StartSite.Text = CommonClass.UserInfo.SiteName;
            TransferSite.Text = "全部";
            BegWeb.Text = CommonClass.UserInfo.WebName;
            BillMan.Text = CommonClass.UserInfo.UserName;
          
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BegWeb_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetUser(BillMan, BegWeb.Text);
        }

        private void CauseName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(AreaName, CauseName.Text);
            CommonClass.SetCauseWeb(BegWeb, CauseName.Text, AreaName.Text);
        }

        private void AreaName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(BegWeb, CauseName.Text, AreaName.Text);
        }
    }
}
