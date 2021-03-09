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

namespace ZQTMS.UI.签单确认
{
    public partial class frmZQTMSFBSignQuery : BaseForm
    {
        public frmZQTMSFBSignQuery()
        {
            InitializeComponent();
        }

        //加载
        private void frmZQTMSFBSignQuery_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            bdate.EditValue = CommonClass.gbdate;
            edate.EditValue = CommonClass.gedate;
            //CommonClass.SetSite(bsite, true);
            //CommonClass.SetCause(CauseName, true);
            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
        }

        //查询
        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                if (!string.IsNullOrEmpty(this.mme_BillNo.Text.Trim()))
                {
                    StringBuilder sb = new StringBuilder();
                    string[] str = this.mme_BillNo.Lines;
                    foreach (string code in str)
                    {
                         if (code != "")
                         {
                             sb.Append(code + "@");
                         }
                    }
                    if (sb.Length > 0)
                    {
                        list.Add(new SqlPara("BillNO", sb.ToString().TrimEnd(',')));
                        list.Add(new SqlPara("begDate", ""));
                        list.Add(new SqlPara("endDate", ""));
                        list.Add(new SqlPara("CauseName", ""));
                        list.Add(new SqlPara("AreaName", ""));
                        list.Add(new SqlPara("SignSite", ""));
                        list.Add(new SqlPara("SignWeb", ""));
                        list.Add(new SqlPara("SignType", ""));
                    }
                }
                else
                {
                    list.Add(new SqlPara("BillNO", ""));
                    list.Add(new SqlPara("begDate", bdate.Text.Trim()));
                    list.Add(new SqlPara("endDate", edate.Text.Trim()));
                    list.Add(new SqlPara("CauseName", this.txt_CauseName.Text.Trim() == "" ? "%%" : this.txt_CauseName.Text));
                    list.Add(new SqlPara("AreaName", this.txt_AreaName.Text.Trim() == "" ? "%%" : this.txt_AreaName.Text));
                    list.Add(new SqlPara("SignSite", this.txt_SignSite.Text.Trim() == "" ? "%%" : this.txt_SignSite.Text.Trim()));
                    list.Add(new SqlPara("SignWeb", this.txt_SignWeb.Text.Trim() == "" ? "%%" : this.txt_SignWeb.Text.Trim()));
                    list.Add(new SqlPara("SignType", this.txt_SignType.Text.Trim() == "全部" ? "%%" : this.txt_SignType.Text.Trim()));
                }

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ZQTMSSignQuery", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                if (myGridView1.RowCount < 1000) myGridView1.BestFitColumns();
            }
        }

        //退出
        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //清空
        private void sb_clear_Click(object sender, EventArgs e)
        {
            this.mme_BillNo.Text = string.Empty;
        }
    }
}
