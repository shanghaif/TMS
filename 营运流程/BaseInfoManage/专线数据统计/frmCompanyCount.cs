using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI.BaseInfoManage
{
    public partial class frmCompanyCount : BaseForm
    {
        private DataSet dsZXSite;
        public frmCompanyCount()
        {
            InitializeComponent();
        }

        private void frmCompanyCount_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar2); //如果有具体的工具条，就引用其实例
            dateEdit1.DateTime = CommonClass.gbdate.AddHours(-16);
            dateEdit2.DateTime = CommonClass.gedate.AddHours(-16);
            dsZXSite = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_ALL_COMPANY"));
            string tmp = "";
            if (dsZXSite != null && dsZXSite.Tables.Count > 0 && dsZXSite.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsZXSite.Tables[0].Rows)
                {
                    tmp = ConvertType.ToString(dr["companyid"]) + "|" + ConvertType.ToString(dr["gsjc"]);
                    if (tmp != "" && !comboBoxEdit1.Properties.Items.Contains(tmp))
                        comboBoxEdit1.Properties.Items.Add(tmp);
                }
            }
            comboBoxEdit1.Properties.Items.Add("全部");
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", dateEdit1.DateTime));
                list.Add(new SqlPara("t2", dateEdit2.DateTime));
                list.Add(new SqlPara("acccompanyid", comboBoxEdit1.Text.Trim() == "全部" ? "%%" : comboBoxEdit1.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_CompanyCount", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    return;
                }
                else
                {
                    myGridControl1.DataSource = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                return;
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }
    }
}
