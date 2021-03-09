using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class frmSelectOneBillSign : BaseForm
    {
        public string SignType = "";
        public int isLocal = 0;
        public DataSet rltDs = null;
        public string webName = "";
        public string siteName = "";

        public frmSelectOneBillSign()
        {
            InitializeComponent();
        }

        private void frmSelectOneBillSign_Load(object sender, EventArgs e)
        {
        }
        
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textEdit1.Text))
            {
                MsgBox.ShowError("请输入运单号！");
                return;
            }
            else
            {
                try
                {
                    List<SqlPara> lst = new List<SqlPara>();
                    lst.Add(new SqlPara("billno", textEdit1.Text));
                    lst.Add(new SqlPara("webName", webName));
                    lst.Add(new SqlPara("siteName", siteName));

                    string proc = "";
                    if (SignType.Equals("FetchSign")) //自提签收  1
                    {
                        proc = "QSP_GET_FETCH_FOR_SIGN_ForOne";
                    }
                    if (SignType.Equals("SendSign")) //送货签收 1
                    {
                        proc = "QSP_GET_SEND_FOR_SIGN_ForOne";
                    }
                    if (SignType.Equals("MiddleSign")) //中转签收  2
                    {
                        lst.Add(new SqlPara("isLocal", isLocal));
                        proc = "QSP_GET_MIDDLE_FOR_SIGN_ForOne";
                    }
                    if (SignType.Equals("SelectOneSendLoad")) //送货上门单票提取
                    {
                        proc = "QSP_GET_SEND_LOAD_ForOne";
                    }


                    if (SignType.Equals("SJZS")) //司机直送单票提取 1
                    {
                        proc = "QSP_GET_SEND_FOR_SJZS_NEW_ForOne";   
                    }


                    if (SignType.Equals("SelectOneShortConnOut"))//短驳出库单票提取
                    {
                        if (CommonClass.UserInfo.UserAccount.StartsWith("x"))
                        {
                            proc = "QSP_GET_PACKAGE_LOAD_ForOne2";
                        }
                        else
                        {
                            proc = "QSP_GET_PACKAGE_LOAD_ForOne";
                        }
                    }
                    if (!string.IsNullOrEmpty(proc))
                    {
                        SqlParasEntity sps = new SqlParasEntity(OperType.Query, proc, lst);
                        rltDs = SqlHelper.GetDataSet(sps);
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
            }
        }

    }
}