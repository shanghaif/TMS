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
using DevExpress.XtraEditors;

namespace ZQTMS.UI
{
    public partial class frmSettleCenterInsuranceAccList : BaseForm
    {
        string companyName = "";
        public frmSettleCenterInsuranceAccList()
        {
            InitializeComponent();
        }

        private void frmChargeApply_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("保险账户");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例


            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
            GetCompanyName(CommonClass.UserInfo.companyid);
           
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("companyName", Company.Text.Trim() == "全部" ? "%%" : Company.Text.Trim()));
            list.Add(new SqlPara("state", IsEnable.Text.Trim() == "全部" ? "%%" : IsEnable.Text.Trim()));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_Get_InsuranceAccount", list);
              DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            GridOper.ExportToExcel(myGridView1); 
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "InsuranceID").ToString());

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", id));
                if (myGridView1.GetRowCellValue(rowhandle, "State").ToString() == "启用")
                {
                    if (MsgBox.ShowYesNo("是否禁用该账户？\r\r请确认！") != DialogResult.Yes)
                    {
                        return;
                    }
                    list.Add(new SqlPara("IsEnable", 0));
                }
                else
                {
                    MsgBox.ShowOK("当前公司为禁用状态,不能再次禁用!");
                    return;
                }
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_UPDATE_InsuranceAccount", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    cbRetrieve_Click(sender, null);
                }
            }
            catch (Exception ex)
            {
                string errmsg = ex.Message.ToString();
                MsgBox.ShowOK(errmsg.Replace("数据库访问异常：", ""));
            }
        }

        /// <summary>
        /// 获取公司名称
        /// </summary>
        /// <param name="companyId"></param>
        private void GetCompanyName(string companyId)
        {
            List<SqlPara> list = new List<SqlPara>();
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "USP_Get_ComapnyName", list);
            DataSet ds =SqlHelper.GetDataSet(sps);
             SetCompanyName(Company,ds);
          
        }

        /// <summary>
        /// 加载公司名称
        /// </summary>
        /// <param name="cb"></param>
        /// <param name="ds"></param>
        /// <param name="isall"></param>
        public static void SetCompanyName(ComboBoxEdit cb,DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0) return;
            try
            {
              
                cb.Properties.Items.Clear();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    cb.Properties.Items.Add(ds.Tables[0].Rows[i]["gsqc"]);
                }
                if (CommonClass.UserInfo.companyid=="101")
                {
                    cb.Properties.Items.Add("全部");
                }
                cb.SelectedIndex =0;
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmSettleCenterInsuranceAccListAdd inAdd = new frmSettleCenterInsuranceAccListAdd();
            inAdd.Show();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "InsuranceID").ToString());

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", id));
                if (myGridView1.GetRowCellValue(rowhandle, "State").ToString() == "启用")
                {
                    MsgBox.ShowOK("当前公司为启用状态,不能再次启用");
                    return;
                }
                else
                {
                    if (MsgBox.ShowYesNo("是否启用该账户？\r\r请确认！") != DialogResult.Yes)
                    {
                        return;
                    }
                    list.Add(new SqlPara("IsEnable", 1));
                }
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_UPDATE_InsuranceAccount", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    cbRetrieve_Click(sender, null);
                }
            }
            catch (Exception ex)
            {
                string errmsg = ex.Message.ToString();
                MsgBox.ShowOK(errmsg.Replace("数据库访问异常：", ""));
            }
        }

    }
}
