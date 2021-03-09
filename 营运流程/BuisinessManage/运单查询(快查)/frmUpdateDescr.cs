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
using Newtonsoft.Json;

namespace ZQTMS.UI
{
    public partial class frmUpdateDescr : BaseForm
    {
        public string ModifyRemark;
        public string BillNO;
        public frmUpdateDescr()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string memostr = memoEdit1.Text.ToString().Replace("\r\n", ";");
           // string textstr = textEdit1.Text.ToString() + "|" + CommonClass.UserInfo.UserName + "|" + CommonClass.gcdate.ToString("yyyy-MM-dd HH:mm:ss");

            string textstr = textEdit1.Text.ToString() + "|" + comboBoxEdit1.Text.ToString() + "|" + CommonClass.UserInfo.UserName + "|" + CommonClass.gcdate.ToString("yyyy-MM-dd HH:mm:ss");
            //plh20191128 
            
            string mremark = textstr + ";" + memostr;

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillNO", BillNO));
            list.Add(new SqlPara("ModifyRemark", mremark));
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_UPDATE_WayBill_ModifyRemark", list);
                int result = SqlHelper.ExecteNonQuery(sps);
                if (result > 0)
                {
                    ModifyRemark = mremark;
                    MsgBox.ShowOK("修改成功！");
                    CommonSyn.NoteSyn(BillNO, ModifyRemark);//hj 备注同步 2018-4-28
                    CommonSyn.LMSSynZQTMS(list, "LMS同步在线修改备注", "USP_UPDATE_WayBill_ModifyRemark_FromLMS");

                    this.Close();
                }
                else
                    MsgBox.ShowOK("修改失败！");
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void frmUpdateDescr_Load(object sender, EventArgs e)
        {
            memoEdit1.EditValue = ModifyRemark.Replace(";", "\r\n");
        }

        private void textEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                simpleButton1.PerformClick();
            }
        }
    }
}