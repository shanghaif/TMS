using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using ZQTMS.Tool;

namespace ZQTMS.UI
{
    /// <summary>
    /// 原有异常撤销(删除记录)流程更改(修改记录状态为已完结，且录入撤销原因)2017-09-14 yd
    /// </summary>
    public partial class frmRevocationCause : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 异常记录编号
        /// </summary>
        private string id;
        private string billNo;
        /// <summary>
        /// 窗体实例被创建时，初始化编号
        /// </summary>
        /// <param name="id"></param>
        public frmRevocationCause(string id, string billNo)
        {
            InitializeComponent();
            this.id = id;
            this.billNo = billNo;//HJ20180514
        }


        private void frmRevocationCause_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
        }


        /// <summary>
        /// 保存撤销信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            //异常原因非空验证
            string cancelCause = this.memoEdit1.Text;
            if (string.IsNullOrEmpty(cancelCause))
            {
                MsgBox.ShowOK("请输入撤销异常的原因！");
                this.memoEdit1.Focus();
                return;
            }
            if (DialogResult.Yes != MsgBox.ShowYesNo("确定撤销？")) return;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("id", id));
                list.Add(new SqlPara("isCancelRegister", "是"));
                list.Add(new SqlPara("cancelCause",cancelCause));
                list.Add(new SqlPara("abnormalityState", "已完结"));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_BAD_TYD_SA_New", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    List<SqlPara> listZQTMS = new List<SqlPara>();//maohui20180724(将数据同步到ZQTMS系统)
                    listZQTMS.Add(new SqlPara("id", id));
                    listZQTMS.Add(new SqlPara("isCancelRegister", "是"));
                    listZQTMS.Add(new SqlPara("cancelCause", cancelCause));
                    listZQTMS.Add(new SqlPara("abnormalityState", "已完结"));
                    SqlParasEntity spsZQTMS = new SqlParasEntity(OperType.Execute, "USP_DELETE_BAD_TYD_SA_New", listZQTMS);
                    SqlHelper.ExecteNonQuery_ZQTMS(spsZQTMS);
                    MsgBox.ShowOK();
                    //已完结状态后把数据同步到LMS hj20180511
                    //Common.CommonSyn.AbnormalSyn(billNo);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}