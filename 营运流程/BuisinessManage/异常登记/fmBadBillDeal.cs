using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace ZQTMS.UI
{
    public partial class fmBadBillDeal : BaseForm
    {
        public fmBadBillDeal()
        {
            InitializeComponent();
        }

        public int look = 0;
        private string ID = "";

        public GridView gv = null;

        private void fmBadBillDeal_Load(object sender, EventArgs e)
        {
            string siteName = CommonClass.UserInfo.SiteName;
            //CommonClass.SetWeb(zerenwebid1, "%%", false);
            CommonClass.SetDep(zerenwebid1, "%%", false);//zxw 将网点表的部门换成部门表的部门
            //CommonClass.SetWeb(zerenwebid2, "%%", false);
            CommonClass.SetDep(zerenwebid2, "%%", false);
            //CommonClass.SetWeb(zerenwebid3, "%%", false);
            CommonClass.SetDep(zerenwebid3, "%%", false);
            if (gv != null)
            {
                billno.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "BillNo").ToString();
                baddate.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "baddate").ToString();
                badcontent.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "badcontent").ToString();
                EXTType.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "badmode").ToString();//zxw 异常类型
                EXTDepart.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "badcreateweb").ToString();//zxw 异常产生部门

                badchulidate.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "badchulidate").ToString();
                badchuliyijian.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "badchuliyijian").ToString();

                zerenman1.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "badzerenman").ToString();

                badshdate.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "badshdate").ToString();
                badshremark.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "badshremark").ToString();

                badtype.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "badtype").ToString();
                zerenwebid1.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "zerenwebid1").ToString();
                zerenman1.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "zerenman1").ToString();
                zerenacc1.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "zerenacc1").ToString();

                zerenwebid2.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "zerenwebid2").ToString();
                zerenman2.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "zerenman2").ToString();
                zerenacc2.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "zerenacc2").ToString();

                zerenwebid3.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "zerenwebid3").ToString();
                zerenman3.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "zerenman3").ToString();
                zerenacc3.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "zerenacc3").ToString();

                zerencancelremark.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "zerencancelremark").ToString();
                badzerenchuliman.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "badzerenchuliman").ToString();
                badzerenchulidate.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "badzerenchulidate").ToString();

                edaccspe.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "accspe").ToString();
                edaccpfe.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "accpfe").ToString();
                result.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "result").ToString();
                SuopeiMan.Text = ConvertType.ToString(gv.GetRowCellValue(gv.FocusedRowHandle, "SuopeiMan"));
                SuopeiPhone.Text = ConvertType.ToString(gv.GetRowCellValue(gv.FocusedRowHandle, "SuopeiPhone"));
                SpBankName.Text = ConvertType.ToString(gv.GetRowCellValue(gv.FocusedRowHandle, "SpBankName"));
                SpBankAdd.Text = ConvertType.ToString(gv.GetRowCellValue(gv.FocusedRowHandle, "SpBankAdd"));
                SpBankAcount.Text = ConvertType.ToString(gv.GetRowCellValue(gv.FocusedRowHandle, "SpBankAcount"));
                SpWay.Text = ConvertType.ToString(gv.GetRowCellValue(gv.FocusedRowHandle, "SpWay"));


                int hfstate_s = Convert.ToInt32(gv.GetRowCellValue(gv.FocusedRowHandle, "hfstate").ToString() == "" ? "0" : gv.GetRowCellValue(gv.FocusedRowHandle, "hfstate").ToString());

                if (hfstate_s == 1)
                {
                    hfstate.Text = "同意";
                }
                else if (hfstate_s == 2)
                {
                    hfstate.Text = "否决";
                }

                ID = gv.GetRowCellValue(gv.FocusedRowHandle, "ID").ToString() == "" ? Guid.NewGuid().ToString() : gv.GetRowCellValue(gv.FocusedRowHandle, "ID").ToString();
            }

            if (look == 10)
            {
                simpleButton2.Visible = false;
                simpleButton1.Visible = false;
                simpleButton4.Visible = false;
                simpleButton3.Visible = false;

                simpleButton5.Visible = false;
                simpleButton6.Visible = false;
                simpleButton7.Visible = checkEdit1.Visible = false;
            }
            else if (look == 1)//流程反馈
            {
                simpleButton2.Visible = false;
                simpleButton4.Visible = false;
                simpleButton3.Visible = false;

                simpleButton5.Visible = false;
                simpleButton6.Visible = false;
                simpleButton7.Visible = checkEdit1.Visible = false;

                //simpleButton4.Enabled = simpleButton5.Visible = ur.GetUserRightDetail888("abadsa06");
                //simpleButton3.Enabled = simpleButton6.Visible = ur.GetUserRightDetail888("abadsa07");

                //barButtonItem4.Enabled = ur.GetUserRightDetail888("abadsa06");
                //barButtonItem8.Enabled = ur.GetUserRightDetail888("abadsa07");
            }
            else if (look == 2)//责任划分
            {
                simpleButton1.Visible = false;
                simpleButton3.Visible = false;
                simpleButton2.Visible = false;
                simpleButton6.Visible = false;
                simpleButton7.Visible = checkEdit1.Visible = false;

                int hfstate_s = Convert.ToInt32(gv.GetRowCellValue(gv.FocusedRowHandle, "hfstate").ToString() == "" ? "0" : gv.GetRowCellValue(gv.FocusedRowHandle, "hfstate").ToString());
                if (hfstate_s == 0)
                {
                    simpleButton5.Enabled = false;
                    shstate.Text = "同意";
                }
                else
                {
                    simpleButton4.Enabled = false;
                    shstate.Text = "否决";
                }
            }
            else if (look == 3)//审核
            {
                simpleButton1.Visible = false;
                simpleButton4.Visible = false;
                simpleButton2.Visible = false;

                simpleButton5.Visible = false;
                simpleButton7.Visible = checkEdit1.Visible = false;

                int shstate_s = Convert.ToInt32(gv.GetRowCellValue(gv.FocusedRowHandle, "shstate").ToString() == "" ? "0" : gv.GetRowCellValue(gv.FocusedRowHandle, "shstate").ToString());
                if (shstate_s == 0)
                {
                    simpleButton6.Enabled = false;
                }
                else
                {
                    simpleButton3.Enabled = false;
                }
            }
            else if (look == 4)//修改
            {
                simpleButton1.Visible = false;
                simpleButton4.Visible = false;
                simpleButton3.Visible = false;

                simpleButton5.Visible = false;
                simpleButton6.Visible = false;
                simpleButton7.Visible = checkEdit1.Visible = false;
            }
            else if (look == 5)  //理赔
            {
                simpleButton1.Visible = false;
                simpleButton2.Visible = false;
                simpleButton4.Visible = false;
                simpleButton3.Visible = false;
                simpleButton5.Visible = false;
                simpleButton6.Visible = false;
                simpleButton7.Visible = checkEdit1.Visible = true;
            }
            else if (look == 11)//快找过来
            {
                int hfstate_s = ConvertType.ToInt32(gv.GetRowCellValue(gv.FocusedRowHandle, "hfstate"));
                if (hfstate_s == 0)
                {
                    simpleButton5.Enabled = false;
                    shstate.Text = "同意";
                }
                else
                {
                    simpleButton4.Enabled = false;
                    shstate.Text = "否决";
                }

                int shstate_s = ConvertType.ToInt32(gv.GetRowCellValue(gv.FocusedRowHandle, "shstate"));
                if (shstate_s == 0)
                {
                    simpleButton6.Enabled = false;
                }
                else
                {
                    simpleButton3.Enabled = false;
                }

                //流程跟踪
                simpleButton1.Visible = simpleButton1.Visible && UserRight.GetRight("ZQTMS.UI.fmBadBill#barButtonItem3");
                //修改登记
                simpleButton2.Visible = simpleButton2.Visible && UserRight.GetRight("ZQTMS.UI.fmBadBill#barButtonItem10");
                //责任划分
                simpleButton4.Visible = simpleButton4.Visible && UserRight.GetRight("ZQTMS.UI.fmBadBill#barButtonItem4");
                simpleButton5.Visible = simpleButton5.Visible && UserRight.GetRight("ZQTMS.UI.fmBadBill#barButtonItem4");
                //审核流程
                simpleButton3.Visible = simpleButton3.Visible && UserRight.GetRight("ZQTMS.UI.fmBadBill#barButtonItem8");
                simpleButton6.Visible = simpleButton6.Visible && UserRight.GetRight("ZQTMS.UI.fmBadBill#barButtonItem8");

                //异常理赔
                simpleButton7.Visible = checkEdit1.Visible = simpleButton7.Visible && UserRight.GetRight("ZQTMS.UI.fmBadBill#barButtonItem13");

            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (badcreateby.Text.Trim() == "")
            {
                MsgBox.ShowOK("必须输入补充内容！");
                return;
            }
            try
            {
                string content = badcontent.Text.Trim() + "\r\n";
                int type = 0;
                string userName = CommonClass.UserInfo.UserName;
                DateTime gcdate = CommonClass.gcdate;
                if (!content.Contains("反馈人2"))
                {
                    if (fangkuiman2.Text.Trim() == "")
                    {
                        MsgBox.ShowOK("必须录入反馈人");
                        fangkuiman2.Focus();
                        return;
                    }
                    type = 2;
                    content = content + userName + ":" + badcreateby.Text.Trim() + "--" + gcdate + "--反馈人2:" + fangkuiman2.Text.Trim();
                }
                else if (!content.Contains("反馈人3"))
                {
                    if (fangkuiman2.Text.Trim() == "")
                    {
                        MsgBox.ShowOK("必须录入反馈人");
                        fangkuiman2.Focus();
                        return;
                    }
                    type = 3;
                    content = content + userName + ":" + badcreateby.Text.Trim() + "--" + gcdate + "--反馈人3:" + fangkuiman2.Text.Trim();
                }
                else if (!content.Contains("反馈人4"))
                {
                    if (fangkuiman2.Text.Trim() == "")
                    {
                        MsgBox.ShowOK("必须录入反馈人");
                        fangkuiman2.Focus();
                        return;
                    }
                    type = 4;
                    content = content + userName + ":" + badcreateby.Text.Trim() + "--" + gcdate + "--反馈人4:" + fangkuiman2.Text.Trim();
                }
                else
                {
                    content = content + userName + ":" + badcreateby.Text.Trim() + "--" + gcdate;
                }

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", ID));
                list.Add(new SqlPara("badcontent", content));
                list.Add(new SqlPara("fangkuiman", fangkuiman2.Text.Trim()));
                list.Add(new SqlPara("type", type));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_MODIFIED_BAD_TYD_SA", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    badcontent.Text = content;
                    gv.SetRowCellValue(gv.FocusedRowHandle, "badcontent", content);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (badchuliman.Text.Trim() == "")
            {
                MsgBox.ShowOK("必须输入反馈内容！");
                return;
            }
            try
            {
                string userName = CommonClass.UserInfo.UserName;
                DateTime gcdate = CommonClass.gcdate;
                string content = (badchuliyijian.Text.Trim() == "" ? "" : badchuliyijian.Text.Trim() + "\r\n") + userName + ":" + badchuliman.Text.Trim() + "--" + gcdate;

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", ID));
                list.Add(new SqlPara("badchuliman", userName));
                list.Add(new SqlPara("badchulidate", gcdate));
                list.Add(new SqlPara("badchuliyijian", content));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_BAD_CHULILIUCHENG", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    badchuliyijian.Text = content;
                    gv.SetRowCellValue(gv.FocusedRowHandle, "badchuliyijian", content);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "badchuliman", userName);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "badchulidate", gcdate);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            try
            {
                string userName = CommonClass.UserInfo.UserName;
                DateTime gcdate = CommonClass.gcdate;

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", ID));
                list.Add(new SqlPara("accspe", edaccspe.Text.Trim() == "" ? "0" : edaccspe.Text.Trim()));
                list.Add(new SqlPara("accpfe", edaccpfe.Text.Trim() == "" ? "0" : edaccpfe.Text.Trim()));
                list.Add(new SqlPara("accdate", gcdate));
                list.Add(new SqlPara("accman", userName));
                list.Add(new SqlPara("SuopeiMan", SuopeiMan.Text));
                list.Add(new SqlPara("SuopeiPhone", SuopeiPhone.Text));
                list.Add(new SqlPara("SpBankName", SpBankName.Text));
                list.Add(new SqlPara("SpBankAdd", SpBankAdd.Text));
                list.Add(new SqlPara("SpBankAcount", SpBankAcount.Text));
                list.Add(new SqlPara("SpWay", SpWay.Text));
                
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_BAD_ACCSPE", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    gv.SetRowCellValue(gv.FocusedRowHandle, "accspe", edaccspe.Text.Trim() == "" ? "0" : edaccspe.Text.Trim());
                    gv.SetRowCellValue(gv.FocusedRowHandle, "accpfe", edaccpfe.Text.Trim() == "" ? "0" : edaccpfe.Text.Trim());
                    gv.SetRowCellValue(gv.FocusedRowHandle, "accdate", gcdate);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "accman", userName);

                    //UpdateToOA(); //在服务器上自动推送了
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (badtype.Text.Trim() == "")
            {
                MsgBox.ShowOK("必须选择异常类型！");
                return;
            }
            if (hfstate.Text.Trim() == "")
            {
                MsgBox.ShowOK("必须选择责任划分状态！");
                return;
            }
            if (hfstate.Text.Trim() == "否决" && zerencancelremark.Text.Trim() == "")
            {
                MsgBox.ShowOK("必须输入否决原因");
                return;
            }
            try
            {
                string userName = CommonClass.UserInfo.UserName;
                DateTime gcdate = CommonClass.gcdate;

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", ID));
                list.Add(new SqlPara("badzerenchulidate", gcdate));
                list.Add(new SqlPara("badzerenchuliman", userName));

                list.Add(new SqlPara("badtype", badtype.Text.Trim()));
                list.Add(new SqlPara("zerenwebid1", zerenwebid1.Text.Trim()));
                list.Add(new SqlPara("zerenman1", zerenman1.Text.Trim()));
                list.Add(new SqlPara("zerenacc1", zerenacc1.Text.Trim() == "" ? "0" : zerenacc1.Text.Trim()));

                list.Add(new SqlPara("zerenwebid2", zerenwebid2.Text.Trim()));
                list.Add(new SqlPara("zerenman2", zerenman2.Text.Trim()));
                list.Add(new SqlPara("zerenacc2", zerenacc2.Text.Trim() == "" ? "0" : zerenacc2.Text.Trim()));

                list.Add(new SqlPara("zerenwebid3", zerenwebid3.Text.Trim()));
                list.Add(new SqlPara("zerenman3", zerenman3.Text.Trim()));
                list.Add(new SqlPara("zerenacc3", zerenacc3.Text.Trim() == "" ? "0" : zerenacc3.Text.Trim()));

                list.Add(new SqlPara("zerencancelremark", zerencancelremark.Text.Trim()));
                list.Add(new SqlPara("hfstate", hfstate.Text.Trim() == "同意" ? 1 : 2));
                list.Add(new SqlPara("accspe", edaccspe.Text.Trim() == "" ? "0" : edaccspe.Text.Trim()));
                list.Add(new SqlPara("accpfe", edaccpfe.Text.Trim() == "" ? "0" : edaccpfe.Text.Trim()));
                list.Add(new SqlPara("result", result.Text.Trim()));
                list.Add(new SqlPara("badxm", badxm.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_BAD_ZERENHUAFEN", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();

                    gv.SetRowCellValue(gv.FocusedRowHandle, "badtype", badtype.Text.Trim());
                    gv.SetRowCellValue(gv.FocusedRowHandle, "zerenwebid1", zerenwebid1.Text);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "zerenman1", zerenman1.Text);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "zerenacc1", zerenacc1.Text.Trim() == "" ? "0" : zerenacc1.Text.Trim());

                    gv.SetRowCellValue(gv.FocusedRowHandle, "zerenwebid2", zerenwebid2.Text);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "zerenman2", zerenman2.Text);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "zerenacc2", zerenacc2.Text.Trim() == "" ? "0" : zerenacc2.Text.Trim());

                    gv.SetRowCellValue(gv.FocusedRowHandle, "zerenwebid3", zerenwebid3.Text);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "zerenman3", zerenman3.Text);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "zerenacc3", zerenacc3.Text.Trim() == "" ? "0" : zerenacc3.Text.Trim());

                    gv.SetRowCellValue(gv.FocusedRowHandle, "zerencancelremark", zerencancelremark.Text);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "hfstate", hfstate.Text.Trim() == "同意" ? 1 : 2);


                    gv.SetRowCellValue(gv.FocusedRowHandle, "badzerenchuliman", userName);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "badzerenchulidate", gcdate);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "accspe", edaccspe.Text.Trim() == "" ? "0" : edaccspe.Text.Trim());
                    gv.SetRowCellValue(gv.FocusedRowHandle, "accpfe", edaccpfe.Text.Trim() == "" ? "0" : edaccpfe.Text.Trim());

                    simpleButton4.Enabled = false;
                    simpleButton5.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != MsgBox.ShowYesNo("确定取消责任划分？")) return;
            try
            {
                string userName = CommonClass.UserInfo.UserName;

                badzerenchuliman.EditValue = DBNull.Value;
                badzerenchulidate.EditValue = DBNull.Value;

                badtype.EditValue = DBNull.Value;
                zerenwebid1.EditValue = DBNull.Value;
                zerenman1.EditValue = DBNull.Value;
                zerenacc1.EditValue = DBNull.Value;

                zerenwebid2.EditValue = DBNull.Value;
                zerenman2.EditValue = DBNull.Value;
                zerenacc2.EditValue = DBNull.Value;

                zerenwebid3.EditValue = DBNull.Value;
                zerenman3.EditValue = DBNull.Value;
                zerenacc3.EditValue = DBNull.Value;

                zerencancelremark.EditValue = DBNull.Value;
                hfstate.EditValue = DBNull.Value;

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", ID));
                list.Add(new SqlPara("cancelman", userName));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_BAD_ZERENHUAFEN_CANCEL", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    gv.SetRowCellValue(gv.FocusedRowHandle, "badtype", DBNull.Value);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "zerenwebid1", DBNull.Value);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "zerenman1", DBNull.Value);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "zerenacc1", DBNull.Value);

                    gv.SetRowCellValue(gv.FocusedRowHandle, "zerenwebid2", DBNull.Value);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "zerenman2", DBNull.Value);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "zerenacc2", DBNull.Value);

                    gv.SetRowCellValue(gv.FocusedRowHandle, "zerenwebid3", DBNull.Value);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "zerenman3", DBNull.Value);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "zerenacc3", DBNull.Value);

                    gv.SetRowCellValue(gv.FocusedRowHandle, "zerencancelremark", DBNull.Value);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "hfstate", DBNull.Value);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "badzerenchuliman", DBNull.Value);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "badzerenchulidate", DBNull.Value);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "cancelman", userName);
                    simpleButton5.Enabled = false;
                    simpleButton4.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (shstate.Text.Trim() != "同意" && badshman.Text.Trim() == "")
            {
                MsgBox.ShowOK("必须输入审核意见！");
                return;
            }
            if (shstate.Text.Trim() == "")
            {
                MsgBox.ShowOK("必须选择审核状态！");
                return;
            }
            try
            {
                string userName = CommonClass.UserInfo.UserName;
                DateTime gcdate = CommonClass.gcdate;
                string content = (badshremark.Text.Trim() == "" ? "" : badshremark.Text.Trim() + "\r\n") + userName + "\r\t" + shstate.Text.Trim() + "\r\t" + badshman.Text.Trim() + "\r\t" + gcdate;
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", ID));
                list.Add(new SqlPara("badshman", userName));
                list.Add(new SqlPara("badshdate", gcdate));
                list.Add(new SqlPara("badshremark", content));
                list.Add(new SqlPara("shstate", shstate.Text.Trim() == "同意" ? 1 : 2));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_BAD_SHENHEYIJIAN", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    gv.SetRowCellValue(gv.FocusedRowHandle, "badshman", userName);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "badshdate", gcdate);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "badshremark", content);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "shstate", shstate.Text.Trim() == "同意" ? 1 : 2);
                    badshremark.Text = content;

                    simpleButton3.Enabled = false;
                    simpleButton6.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            try
            {
                string userName = CommonClass.UserInfo.UserName;
                DateTime gcdate = CommonClass.gcdate;
                string content = (badshremark.Text.Trim() == "" ? "" : badshremark.Text.Trim() + "\r\n") + userName + "\r\t" + "反审核" + "\r\t" + badshman.Text.Trim() + "\r\t" + gcdate;
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", ID));
                list.Add(new SqlPara("badshman", userName));
                list.Add(new SqlPara("badshdate", gcdate));
                list.Add(new SqlPara("badshremark", content));
                list.Add(new SqlPara("shstate", 0));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_BAD_SHENHEYIJIAN", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    gv.SetRowCellValue(gv.FocusedRowHandle, "badshman", DBNull.Value);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "badshdate", DBNull.Value);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "badshremark", content);
                    gv.SetRowCellValue(gv.FocusedRowHandle, "shstate", 0);
                    badshremark.Text = content;
                    simpleButton3.Enabled = true;
                    simpleButton6.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void badtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            badxm.Properties.Items.Clear();
            badxm.Text = "";
            if (badtype.Text.Trim() == "标签异常")
            {
                badxm.Properties.Items.Add("标签内容错误");
                badxm.Properties.Items.Add("标签黏贴错误");
                badxm.Properties.Items.Add("无标签");
            }
            else if (badtype.Text.Trim() == "货差异常")
            {
                badxm.Properties.Items.Add("有单无货");
                badxm.Properties.Items.Add("有单少货");
                badxm.Properties.Items.Add("有货无单");
                badxm.Properties.Items.Add("有单多货");
                badxm.Properties.Items.Add("窜货");
                badxm.Properties.Items.Add("内物少货");
            }
            else if (badtype.Text.Trim() == "货损异常")
            {
                badxm.Properties.Items.Add("包装破损");
                badxm.Properties.Items.Add("货物污染");
                badxm.Properties.Items.Add("货物潮湿");
                badxm.Properties.Items.Add("货物破损");
            }
            else if (badtype.Text.Trim() == "违禁品")
            {
                badxm.Properties.Items.Add("包装不合格");
                badxm.Properties.Items.Add("限运品");
                badxm.Properties.Items.Add("禁运品");
                badxm.Properties.Items.Add("危险品");
            }
            else if (badtype.Text.Trim() == "装车不规范")
            {
                badxm.Properties.Items.Add("混装");
                badxm.Properties.Items.Add("货物倒置");
            }
            else if (badtype.Text.Trim() == "一次性锁异常")
            {
                badxm.Properties.Items.Add("锁号错误");
                badxm.Properties.Items.Add("未上一次性锁");
            }
            else if (badtype.Text.Trim() == "超重超方")
            {
                badxm.Properties.Items.Add("超重超方");
            }
            else if (badtype.Text.Trim() == "制单异常")
            {
                badxm.Properties.Items.Add("包装不符");
                badxm.Properties.Items.Add("回单异常");
                badxm.Properties.Items.Add("品名不符");
                badxm.Properties.Items.Add("大票提货点错误");
                badxm.Properties.Items.Add("目的站错误");
                badxm.Properties.Items.Add("收货人地址错误");
                badxm.Properties.Items.Add("收货人资料不规范");
            }
        }

        private void UpdateToOA()
        {
            if (!checkEdit1.Checked) return;

            if (SuopeiMan.Text.Trim() == "" || SuopeiPhone.Text.Trim() == "" || SpWay.Text.Trim() == "" || edaccspe.Text.Trim() == "")
            {
                MsgBox.ShowOK("索赔联系人,电话,方式以及金额是必填项！请检查！！");
                return;
            }
            if (SpWay.Text.Trim() == "付款" && (SpBankName.Text == "" || SpBankAdd.Text == "" || SpBankAcount.Text == ""))
            {
                MsgBox.ShowOK("银行信息必填！请填写之后重新推送！");
                return;
            }

            #region 加载本单图片
            string file = "";
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", billno.Text.Trim()));
                list.Add(new SqlPara("BillType", 1));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_TBFILEINFO_BadBill", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds != null && ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string filename = ConvertType.ToString(ds.Tables[0].Rows[i]["FilePath"]);
                        string[] sArray = filename.Split('/');

                        string bdPath = System.Windows.Forms.Application.StartupPath + "/TempFile";
                        if (!Directory.Exists(bdPath)) Directory.CreateDirectory(bdPath);
                        string bdFileName = bdPath + filename.Substring(filename.LastIndexOf("/"));
                        WebClient wc = new WebClient();
                        if (!File.Exists(bdFileName))
                        {
                            wc.DownloadFile("http://ZQTMS.dekuncn.com:7020" + filename, bdFileName);
                        }

                        //上传图片到服务器
                        byte[] bt = wc.UploadFile(HttpHelper.OAUrlImage, "POST", bdFileName);
                        string json = Encoding.UTF8.GetString(bt);
                        OAFileUpResult result = JsonConvert.DeserializeObject<OAFileUpResult>(json);
                        if (result.Success == true)
                        {
                            file += string.Format("<File>{0}</File>", result.FileName);
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                return;
            } 
            #endregion

            string ConsignorCellPhone = gv.GetRowCellValue(gv.FocusedRowHandle, "ConsignorCellPhone").ToString();
            string ConsignorPhone = gv.GetRowCellValue(gv.FocusedRowHandle, "ConsignorPhone").ToString();
            string string19 = ConsignorCellPhone + "/" + ConsignorPhone;
            string19 = string19.Trim('/');

            string ConsigneeCellPhone = gv.GetRowCellValue(gv.FocusedRowHandle, "ConsigneeCellPhone").ToString();
            string ConsigneePhone = gv.GetRowCellValue(gv.FocusedRowHandle, "ConsigneePhone").ToString();
            string string26 = ConsigneeCellPhone + "/" + ConsigneePhone;
            string26 = string26.Trim('/');

            string zerenwebid1 = gv.GetRowCellValue(gv.FocusedRowHandle, "zerenwebid1").ToString();
            string zerenwebid2 = gv.GetRowCellValue(gv.FocusedRowHandle, "zerenwebid2").ToString();
            string zerenwebid3 = gv.GetRowCellValue(gv.FocusedRowHandle, "zerenwebid3").ToString();
            string string32 = zerenwebid1 + "/" + zerenwebid2 + "/" + zerenwebid3;
            string32 = string32.Trim('/');

            StringBuilder sb = new StringBuilder();
            StringBuilder sb1 = new StringBuilder();
            sb.AppendFormat("<date1>{0}</date1>", gv.GetRowCellValue(gv.FocusedRowHandle, "BillDate"));//ZQTMS开单日期
            float accspe = 0;
            if (ConvertType.ToFloat(edaccspe.Text) == 0)
                accspe = ConvertType.ToFloat(gv.GetRowCellValue(gv.FocusedRowHandle, "accspe"));
            else
                accspe = ConvertType.ToFloat(edaccspe.Text);
            sb.AppendFormat("<number2>{0}</number2>", accspe);//索赔金额
            sb.AppendFormat("<string24>{0}</string24>", gv.GetRowCellValue(gv.FocusedRowHandle, "webid"));//开单部门
            sb.AppendFormat("<string10>{0}</string10>", gv.GetRowCellValue(gv.FocusedRowHandle, "Varieties"));//品名
            sb.AppendFormat("<string1>{0}</string1>", gv.GetRowCellValue(gv.FocusedRowHandle, "Package"));//包装
            sb.AppendFormat("<number7>{0}</number7>", gv.GetRowCellValue(gv.FocusedRowHandle, "Num"));//件数
            //string sSignState = "正常签收";
            //if (ConvertType.ToString(gv.GetRowCellValue(gv.FocusedRowHandle, "qsstate")) == "")
            //    sSignState = "";
            sb.AppendFormat("<string28>{0}</string28>", comboBoxEdit2.Text.Trim());//签收情况
            sb.AppendFormat("<string4>{0}</string4>", gv.GetRowCellValue(gv.FocusedRowHandle, "ConsignorCompany"));//发货单位
            sb.AppendFormat("<string5>{0}</string5>", gv.GetRowCellValue(gv.FocusedRowHandle, "ConsignorName"));//发货人
            sb.AppendFormat("<string19>{0}</string19>", string19);//联系方式（发货人）
            sb.AppendFormat("<string2>{0}</string2>", gv.GetRowCellValue(gv.FocusedRowHandle, "PaymentMode"));//结算方式
            sb.AppendFormat("<string6>{0}</string6>", gv.GetRowCellValue(gv.FocusedRowHandle, "ConsigneeCompany"));//收货单位
            sb.AppendFormat("<string11>{0}</string11>", gv.GetRowCellValue(gv.FocusedRowHandle, "ConsigneeName"));//收货人
            sb.AppendFormat("<string26>{0}</string26>", string26);//联系方式（收货人）
            sb.AppendFormat("<string20>{0}</string20>", gv.GetRowCellValue(gv.FocusedRowHandle, "badtype"));//异常类型
            sb.AppendFormat("<string22>{0}</string22>", SpWay.Text);//理赔方式
            sb.AppendFormat("<string32>{0}</string32>", string32);//责任部门
            float accpfe = 0;
            if (ConvertType.ToFloat(edaccpfe.Text) == 0)
                accpfe = ConvertType.ToFloat(gv.GetRowCellValue(gv.FocusedRowHandle, "accpfe"));
            else
                accpfe = ConvertType.ToFloat(edaccpfe.Text);
            sb.AppendFormat("<number1>{0}</number1>", gv.GetRowCellValue(gv.FocusedRowHandle, "accpfe"));//实际赔付金额
            sb.AppendFormat("<string29>{0}</string29>", billno.Text.Trim());//运单号
            sb.AppendFormat("<string33>{0}</string33>", gv.GetRowCellValue(gv.FocusedRowHandle, "DestinationSite"));//到站
            sb.AppendFormat("<string21>{0}</string21>", SuopeiMan.Text);//索赔人
            sb.AppendFormat("<string27>{0}</string27>", SuopeiPhone.Text);//索赔人的联系方式
            sb.AppendFormat("<string7>{0}</string7>", SpBankName.Text);//银行户名
            sb.AppendFormat("<string8>{0}</string8>", SpBankAdd.Text);//开户行
            sb.AppendFormat("<string9>{0}</string9>", SpBankAcount.Text);//账号
            sb.AppendFormat("<string30>{0}</string30>", comboBoxEdit1.Text);//客户类型
            
            string msg = SubmitToOA.PostBadBill(billno.Text.Trim(), sb.ToString(), file);

            if (msg != "")//说明提交失败
            {
                MsgBox.ShowError("理赔金额保存成功，提交到OA流程失败，可尝试重新提交!");
                linkLabel1.Visible = true;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            UpdateToOA();
        }
    
    
    }
}