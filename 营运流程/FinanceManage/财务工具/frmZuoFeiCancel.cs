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
    public partial class frmZuoFeiCancel : ZQTMS.Tool.BaseForm
    {
        public frmZuoFeiCancel()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string billNo = BillNo.Text.Trim();
            if (billNo == "")
            {
                MsgBox.ShowOK("请输入要恢复的运单！");
                BillNo.Focus();
                return;
            }

            CommonClass.ShowBillSearch(billNo);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string billNo = BillNo.Text.Trim();
            if (billNo == "")
            {
                MsgBox.ShowOK("请输入要恢复的运单！");
                BillNo.Focus();
                return;
            }
            if (MsgBox.ShowYesNo("确定要恢复已作废运单：" + billNo + "？") != DialogResult.Yes) return;

            try
            {
                //Type：1表示作废;2表示恢复
               // if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_WAYBILL_ZUOFEI_CANCEL", new List<SqlPara> { new SqlPara("BillNo", billNo) })) == 0) return;
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_WAYBILL_ZUOFEI_Recovery", new List<SqlPara> { new SqlPara("BillNo", billNo) })) == 0) return;
                BillNo.Text = "";
                MsgBox.ShowOK("运单:" + billNo + ",恢复成功!");
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmZuoFeiCancel_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("运单作废恢复");//xj/2019/5/28
        }
    }
}