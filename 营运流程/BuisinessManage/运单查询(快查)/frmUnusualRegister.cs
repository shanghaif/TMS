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
    public partial class frmUnusualRegister : BaseForm
    {
        #region 变量定义

        public string BillNO;

        #endregion

        public frmUnusualRegister()
        {
            InitializeComponent();
        }

        private void frmUnusualRegister_Load(object sender, EventArgs e)
        {
            RegisterMan.EditValue = CommonClass.UserInfo.UserName;
            RegisterDate.EditValue = DateTime.Now;

            CommonClass.SetDep(FeedBackDept,"全部"); 
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("Unid", Guid.NewGuid()));
                list.Add(new SqlPara("RegisterMan", RegisterMan.Text.Trim()));
                list.Add(new SqlPara("FeedBackDept", FeedBackDept.Text.Trim()));
                list.Add(new SqlPara("FeedBackMan", FeedBackMan.Text.Trim()));
                list.Add(new SqlPara("FeedBackContent", FeedBackContent.Text.Trim()));
                list.Add(new SqlPara("BadImgSrc1", BadImgSrc1.Text.Trim()));
                list.Add(new SqlPara("BadImgSrc2", BadImgSrc2.Text.Trim()));
                list.Add(new SqlPara("BillNO", BillNO));
                list.Add(new SqlPara("RegisterDate", RegisterDate.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLUNUSUALREGISTER", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

    }
}
