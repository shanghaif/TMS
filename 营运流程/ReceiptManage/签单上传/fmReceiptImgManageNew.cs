using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class fmReceiptImgManageNew : BaseForm
    {
        public fmReceiptImgManageNew()
        {
            InitializeComponent();
        } 


        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            string bsite = comboBoxEdit1.Text.Trim() == "全部" ? "%%" : comboBoxEdit1.Text.Trim();
            string esite = comboBoxEdit2.Text.Trim() == "全部" ? "%%" : comboBoxEdit2.Text.Trim();


            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bsite", bsite));
                list.Add(new SqlPara("esite", esite));
                list.Add(new SqlPara("t1", dateEdit1.EditValue.ToString()));
                list.Add(new SqlPara("t2", dateEdit2.EditValue.ToString()));
                list.Add(new SqlPara("state", cbState.SelectedIndex));
                list.Add(new SqlPara("datetype", datetype.SelectedIndex));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_TBFILEINFO_ByReceiptNEW", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }　
  

        private void barBtnExport_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barBtnKz_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void barBtnExit_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void fmReceiptImgManage_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("签单管理");//xj/2019/5/28
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar1); //如果有具体的工具条，就引用其实例 

            CommonClass.SetSite(comboBoxEdit1, true);
            CommonClass.SetSite(comboBoxEdit2, true);

            dateEdit1.DateTime = CommonClass.gbdate;
            dateEdit2.DateTime = CommonClass.gedate;

            comboBoxEdit1.Text = "全部";
            comboBoxEdit2.Text = "全部";
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            fmFileUpload frm = new fmFileUpload();
            frm.UpType="upadd";
            frm.UserName = CommonClass.UserInfo.UserName;
            frm.ShowDialog();
        }
        

    }
}
