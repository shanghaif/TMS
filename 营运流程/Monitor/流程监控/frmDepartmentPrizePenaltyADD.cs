using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{ 
    public partial class frmDepartmentPrizePenaltyADD : BaseForm
    {
        public string ID1 = "", DJWeb1 = "", Type1 = "", ResponsibilityWeb1 = "", ResponsibilityMan1 = "", Money1 = "",
            Abstract1 = "", Billno1 = "", Month1 = "",ResponsDepartNature1="",WithdrawingIssuDepart1="",Filenumber1="";
        public frmDepartmentPrizePenaltyADD()
        {
            InitializeComponent();
        }

        private void frmDepartmentPrizePenaltyADD_Load(object sender, EventArgs e)
        {
            DJWeb.Text = CommonClass.UserInfo.WebName;
            CommonClass.SetWeb(ResponsibilityWeb,false);
            //CommonClass.SetWeb(WithdrawingIssuDepart, false);
            //Month.Text = DateTime.Now.ToString("MM");
            if (ID1 != "")
            {
                DJWeb.Text = DJWeb1;
                //Month.Text = Month1;
                Billno.Text = Billno1;
                Type.Text = Type1;
                Money.Text = Money1;
                ResponsibilityWeb.Text = ResponsibilityWeb1;
                ResponsibilityMan.Text = ResponsibilityMan1;
                Abstract.Text = Abstract1;
                //ResponsDepartNature.Text = ResponsDepartNature1;
                //WithdrawingIssuDepart.Text = WithdrawingIssuDepart1;
                //Filenumber.Text = Filenumber1;
                
            }

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
             try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("ID", ID1 == "" ? Guid.NewGuid().ToString() : ID1.ToString()));
                    list.Add(new SqlPara("DJWeb", DJWeb.Text.Trim()));
                    list.Add(new SqlPara("Billno", Billno.Text.Trim()));
                    list.Add(new SqlPara("Type", Type.Text.Trim()));
                    list.Add(new SqlPara("Money", Money.Text.Trim()));
                    list.Add(new SqlPara("ResponsibilityWeb", ResponsibilityWeb.Text.Trim()));
                    list.Add(new SqlPara("ResponsibilityMan", ResponsibilityMan.Text.Trim()));
                    list.Add(new SqlPara("Abstract", Abstract.Text.Trim()));
                    //list.Add(new SqlPara("Month", Month.Text.Trim()));
                    //list.Add(new SqlPara("ResponsDepartNature", ResponsDepartNature.Text.Trim()));
                    //list.Add(new SqlPara("WithdrawingIssuDepart", WithdrawingIssuDepart.Text.Trim()));
                    //list.Add(new SqlPara("Filenumber", Filenumber.Text.Trim()));

                    SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_ADD_RewardData", list);
                    if (SqlHelper.ExecteNonQuery(spe) > 0)
                    {
                        MsgBox.ShowOK();
                        DJWeb.Text = "";
                        //Month.Text = "";
                        Billno.Text = "";
                        Type.Text = "";
                        Money.Text = "";
                        ResponsibilityWeb.Text = "";
                        ResponsibilityMan.Text = "";
                        Abstract.Text = "";
                        //ResponsDepartNature.Text = "";
                        //WithdrawingIssuDepart.Text = "";
                        //Filenumber.Text = "";

                    }
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
            }


    }
}
