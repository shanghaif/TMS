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
    public partial class frmTurnTwoOff : BaseForm
    {
        public frmTurnTwoOff()
        {
            InitializeComponent();
        }

        private void frmTurnTwoOff_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("转二级取消查询");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            bdate.DateTime = CommonClass.gbdate.AddDays(-1);
            edate.DateTime = CommonClass.gedate;
            CommonClass.SetSite(toSite,true);
            CommonClass.SetWeb(toWeb, toSite.Text.Trim(), true);
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            List<SqlPara> list3 = new List<SqlPara>();
            list3.Add(new SqlPara("bdate", bdate.Text.Trim()));
            list3.Add(new SqlPara("edate", edate.Text.Trim()));
            list3.Add(new SqlPara("SendToWeb",toWeb.Text.Trim() == "全部"?"%%":toWeb.Text.Trim()));
            list3.Add(new SqlPara("SendToSite",toSite.Text.Trim() == "全部"?"%%":toSite.Text.Trim()));
            try
            {
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "FM_TURNTWOOFF", list3);
                DataSet ds = SqlHelper.GetDataSet(spe);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }

        private void toSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(toWeb, toSite.Text.Trim(), true);

        }
    }
}
