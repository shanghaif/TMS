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
    public partial class frmDepartmentPrizePenalty : BaseForm
    {
        public frmDepartmentPrizePenalty()
        {
            InitializeComponent();
        }


        private void frmDepartmentPrizePenalty_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("部门奖罚数据");//xj/2019/5/28
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3);
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            CommonClass.SetWeb(DJWeb, true);
            ResponsibilityMan.Text = CommonClass.UserInfo.UserName;
            CommonClass.SetUser(ResponsibilityMan, DJWeb.Text);

        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.Text.Trim()));
                list.Add(new SqlPara("edate", edate.Text.Trim()));
                list.Add(new SqlPara("DJWeb", DJWeb.Text.Trim() == "全部" ? "%%" : DJWeb.Text.Trim()));
                list.Add(new SqlPara("Type", Type.Text.Trim() == "全部" ? "%%" : Type.Text.Trim()));
                list.Add(new SqlPara("ResponsibilityMan", ResponsibilityMan.Text.Trim() == "全部" ? "%%" : ResponsibilityMan.Text.Trim()));
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_DepRewardsPunishmentData", list));
                //if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmDepartmentPrizePenaltyADD frm = new frmDepartmentPrizePenaltyADD();
            frm.ShowDialog();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowHandle = myGridView1.FocusedRowHandle;
            if (rowHandle < 0) return;
            string ID = myGridView1.GetRowCellValue(rowHandle, "ID").ToString();
            //string b = myGridView1.GetRowCellValue(rowHandle, "b").ToString();
            string DJWeb = myGridView1.GetRowCellValue(rowHandle, "DJWeb").ToString();
            string Type = myGridView1.GetRowCellValue(rowHandle, "Type").ToString();
            string ResponsibilityWeb = myGridView1.GetRowCellValue(rowHandle, "ResponsibilityWeb").ToString();
            string ResponsibilityMan = myGridView1.GetRowCellValue(rowHandle, "ResponsibilityMan").ToString();
            string Money = myGridView1.GetRowCellValue(rowHandle, "Money").ToString();
            string Abstract = myGridView1.GetRowCellValue(rowHandle, "Abstract").ToString();
            string Billno = myGridView1.GetRowCellValue(rowHandle, "Billno").ToString();
            string Month = myGridView1.GetRowCellValue(rowHandle, "TheMonth").ToString();
            //string ResponsDepartNature = myGridView1.GetRowCellValue(rowHandle, "ResponsDepartNature").ToString();//责任部门性质
            //string WithdrawingIssuDepart = myGridView1.GetRowCellValue(rowHandle, "WithdrawingIssuDepart").ToString();//扣款发文部门
            //string Filenumber = myGridView1.GetRowCellValue(rowHandle, "Filenumber").ToString();//文件号

            frmDepartmentPrizePenaltyADD frm = new frmDepartmentPrizePenaltyADD();
            frm.ID1 = ID;
            frm.DJWeb1 = DJWeb;
            frm.Type1 = Type;
            frm.ResponsibilityWeb1 = ResponsibilityWeb;
            frm.ResponsibilityMan1 = ResponsibilityMan;
            frm.Money1 = Money;
            frm.Abstract1 = Abstract;
            frm.Billno1 = Billno;
            frm.Month1 = Month;
            //frm.ResponsDepartNature1 = ResponsDepartNature;
            //frm.WithdrawingIssuDepart1 = WithdrawingIssuDepart;
            //frm.Filenumber1 = Filenumber;
            frm.ShowDialog();

        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowHandle = myGridView1.FocusedRowHandle;
            if (rowHandle < 0) return;
            string ID = myGridView1.GetRowCellValue(rowHandle, "ID").ToString();
            if (MsgBox.ShowYesNo("是否删除?此操作不可逆，请确认！") != DialogResult.Yes) return;

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", ID));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_DELETE_RewardData", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    MsgBox.ShowOK();
                    cbRetrieve_Click(sender, e);
                }
                else
                {
                    MsgBox.ShowOK("不能删除！只能删除您录的数据！");
                    return;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmDepartmentPrizePenaltyUP frm = new frmDepartmentPrizePenaltyUP();
            frm.ShowDialog();
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }





    }
}
