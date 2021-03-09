using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class frmSysParamSetting : BaseForm
    {
        private DataTable dt;
        public frmSysParamSetting()
        {
            InitializeComponent();
        }

        private void frmSysParamSetting_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("参数设置");//xj/2019/5/29
            getWindowData();
            tabControl1.SelectTab(1);
        }

        private void getWindowData()
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SYSPARAMSETTING");
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0) return;

                dt = ds.Tables[0];
                myGridControl1.DataSource = dt;

                myGridControl1_Click(null, null);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 确定
        /// </summary> 
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string ptype = ParamType.Text.Trim();
            if (string.IsNullOrEmpty(ptype))
            {
                MsgBox.ShowOK("请选择一个参数！");
                return;
            }

            foreach (DataRow dr_exam in dt.Rows)
            {
                if (ptype == (dr_exam["ParamType"] + ""))
                {
                    dr_exam["ParamValue"] = ParamValue.EditValue;
                    dr_exam["ParamDescription"] = ParamDescription.EditValue;
                    break;
                }
            }

            myGridControl1.Refresh();
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            ParamType.EditValue = "";
            ParamValue.EditValue = "";
            ParamDescription.EditValue = "";
            myGridView1.SelectRow(-1);
        }
        /// <summary>
        /// 清空
        /// </summary> 
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            ParamValue.EditValue = "";
            ParamDescription.EditValue = "";
        }

        /// <summary>
        /// grid 行点击事件
        /// </summary> 
        private void myGridControl1_Click(object sender, EventArgs e)
        { 
            if (myGridView1.FocusedRowHandle < 0) return;

            DataRow dr = myGridView1.GetDataRow(myGridView1.FocusedRowHandle);

            if (dr == null) return;

            ParamDescription.EditValue = dr["ParamDescription"];
            ParamType.EditValue = dr["ParamType"];
            ParamValue.EditValue = dr["ParamValue"];

        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 点击应用按钮
        /// </summary> 
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            StringBuilder strBuilder = new StringBuilder();
            
            //根据公司id来设置参数 zaj 2017-11-19
            //strBuilder.Append("delete sysParamSetting where companyid='" + CommonClass.UserInfo.companyid + "' insert into dbo.sysParamSetting (ParamType,ParamValue,ParamDescription,LastEditor,companyid,LastEditeTime) values ");

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    DataRow dr = dt.Rows[i];

            //    if (i != 0)
            //        strBuilder.Append(",");

            //    strBuilder.Append(" ('" + dr["ParamType"] + "','" + dr["ParamValue"] + "','" + dr["ParamDescription"] + "','" + CommonClass.UserInfo.UserName
            //        + "','" + CommonClass.UserInfo.companyid + "',getdate())");
            //}
           //hj 20171213
            int rowHandel = myGridView1.FocusedRowHandle;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ParamType", ParamType.Text.Trim()));
                list.Add(new SqlPara("paramValue", ParamValue.Text.Trim()));
                list.Add(new SqlPara("ParamDescription", ParamDescription.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "QSP_SAVE_SYSPARAMSETTING", list);

                int num = SqlHelper.ExecteNonQuery(sps);

                if (num > 0)
                {
                    MsgBox.ShowOK();
                    getWindowData();

                    myGridView1.FocusedRowHandle = rowHandel;
                    myGridControl1_Click(null, null);

                }
                else
                    MsgBox.ShowOK("操作失败！");

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }
    }
}
