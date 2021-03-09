using System;
using System.Collections.Generic;
using System.Text;
using ZQTMS.SqlDAL;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.Lib;
using DevExpress.XtraEditors;
using System.Windows.Forms;
using System.Data;

namespace ZQTMS.UI
{
    public enum VerifyType
    {
        中转费,
        派车费,
        大车费,
        折扣费,
        送货费,
        始发其他费,
        终端其他费,
        短驳费,
        货款回收,
        货款汇款,
        转送费,
        月结,
        提付,
        现付,
        欠付,
        回单付,
        货到前付,
        提付异动,  //maohui20180131
        垫付费,//tuxin20180919
        非提付异动,
        整车装卸费,
        短欠//maohui20190105
    }
    public class VerifyOffAccountDeel
    {
        string OilCardNo = "";
        decimal OilCardFee = 0;
        /// <summary>
        /// 添加核销现金日记账
        /// </summary>
        /// <param name="oneSubject">一级科目</param>
        /// <param name="twoSubject">二级科目</param>
        /// <param name="threeSubject">三级科目</param>
        /// <param name="summary">摘要</param>
        /// <param name="verifyType">核销费用类型</param>
        /// <param name="inOutType">记账类型 支出、收入</param>
        /// <param name="billNos">运单号字符串 多个用'@'隔开</param>
        /// <param name="moneys">金额字符串 多个用'@'隔开</param>
        public void AddVerifyOffAccount(string oneSubject, string twoSubject, string threeSubject, string summary, string verifyType, string inOutType, string billNos, string batchNos, string moneys, string feeTypes,string remarks)
        {
            try
            {
                Random r1 = new Random();
                int a1 = r1.Next(10, 100);
                string voucherNo = (inOutType == "支出" ? "O" : "I") + DateTime.Now.ToString("yyyyMMddHHmmss")+a1;
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("VoucherNo", voucherNo));
                list.Add(new SqlPara("OneSubject", oneSubject));
                list.Add(new SqlPara("TwoSubject", twoSubject));
                list.Add(new SqlPara("ThreeSubject", threeSubject));
                list.Add(new SqlPara("Summary", summary));
                list.Add(new SqlPara("Remarks", remarks));//jilei 20180831
                list.Add(new SqlPara("VerifyOffType", verifyType));
                list.Add(new SqlPara("InOutType", inOutType));
                list.Add(new SqlPara("CauseName", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("AreaName", CommonClass.UserInfo.AreaName));
                list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("OptMan", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("BillNos", billNos));
                list.Add(new SqlPara("BatchNos", batchNos));
                list.Add(new SqlPara("Moneys", moneys));
                list.Add(new SqlPara("FeeTypes", feeTypes));

                string strProcName = "USP_ADD_VERIFYOFFACCOUNT";
                //if (CommonClass.IsZhanQuCompanyId(CommonClass.UserInfo.companyid))//战区查询
                //{
                //    strProcName = "USP_ADD_VERIFYOFFACCOUNT_ZhanQu";
                //}
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, strProcName, list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();

                    //同步好多车付款状态 ld20181010
                    //if (verifyType == "大车费")
                    //{
                    //    CommonSyn.SynHaoDuoChePaymentStatus(batchNos, feeTypes);
                    //}
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        /// <summary>
        /// 添加核销现金日记账
        /// </summary>
        /// <param name="oneSubject">一级科目</param>
        /// <param name="twoSubject">二级科目</param>
        /// <param name="threeSubject">三级科目</param>
        /// <param name="summary">摘要</param>
        /// <param name="verifyType">核销费用类型</param>
        /// <param name="inOutType">记账类型 支出、收入</param>
        /// <param name="billNos">运单号字符串 多个用'@'隔开</param>
        /// <param name="moneys">金额字符串 多个用'@'隔开</param>
        public void AddVerifyOffAccount(string oneSubject, string twoSubject, string threeSubject, string summary, string verifyType, string inOutType, string billNos, string batchNos, string moneys, string feeTypes, string WebNames, string remarks)
        {
            try
            {
                Random r1 = new Random();
                int a1 = r1.Next(10, 100);
                string voucherNo = (inOutType == "支出" ? "O" : "I") + DateTime.Now.ToString("yyyyMMddHHmmss")+a1;
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("VoucherNo", voucherNo));
                list.Add(new SqlPara("OneSubject", oneSubject));
                list.Add(new SqlPara("TwoSubject", twoSubject));
                list.Add(new SqlPara("ThreeSubject", threeSubject));
                list.Add(new SqlPara("Summary", summary));
                list.Add(new SqlPara("Remarks", remarks));//jilei 20180831
                list.Add(new SqlPara("VerifyOffType", verifyType));
                list.Add(new SqlPara("InOutType", inOutType));
                list.Add(new SqlPara("CauseName", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("AreaName", CommonClass.UserInfo.AreaName));
                list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("OptMan", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("BillNos", billNos));
                list.Add(new SqlPara("BatchNos", batchNos));
                list.Add(new SqlPara("Moneys", moneys));
                list.Add(new SqlPara("FeeTypes", feeTypes));
                list.Add(new SqlPara("WebNames", WebNames));

                string strProcName = "USP_ADD_VERIFYOFFACCOUNT";
                //if (CommonClass.IsZhanQuCompanyId(CommonClass.UserInfo.companyid))//战区查询
                //{
                //    strProcName = "USP_ADD_VERIFYOFFACCOUNT_ZhanQu";
                //}
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, strProcName, list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();

                    //同步好多车付款状态 ld20181010
                    //if (verifyType == "大车费")
                    //{
                    //    CommonSyn.SynHaoDuoChePaymentStatus(batchNos, feeTypes);
                    //}
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        /// 添加核销现金日记账
        /// </summary>
        /// <param name="oneSubject">一级科目</param>
        /// <param name="twoSubject">二级科目</param>
        /// <param name="threeSubject">三级科目</param>
        /// <param name="summary">摘要</param>
        /// <param name="verifyType">核销费用类型</param>
        /// <param name="inOutType">记账类型 支出、收入</param>
        /// <param name="billNos">运单号字符串 多个用'@'隔开</param>
        /// <param name="moneys">金额字符串 多个用'@'隔开</param>
        public void AddVerifyOffAccount(string oneSubject, string twoSubject, string threeSubject, string Verifydirection, string summary, string verifyType, string inOutType, string billNos, string batchNos, string moneys, string feeTypes, string WebNames, string HandNums, DateTime credentialsTime, string remarks)
        {
            try
            {

                Random r1 = new Random();
                int a1 = r1.Next(10, 100);
                string voucherNo = (inOutType == "支出" ? "O" : "I") + DateTime.Now.ToString("yyyyMMddHHmmss")+a1;
                MsgBox.ShowOK("凭证号为：" + voucherNo);//gy20190726
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("VoucherNo", voucherNo));
                list.Add(new SqlPara("OneSubject", oneSubject));
                list.Add(new SqlPara("TwoSubject", twoSubject));
                list.Add(new SqlPara("ThreeSubject", threeSubject));
                list.Add(new SqlPara("Verifydirection", Verifydirection));
                list.Add(new SqlPara("Summary", summary));
                list.Add(new SqlPara("Remarks", remarks));//jilei 20180831
                list.Add(new SqlPara("VerifyOffType", verifyType));
                list.Add(new SqlPara("InOutType", inOutType));
                list.Add(new SqlPara("CauseName", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("AreaName", CommonClass.UserInfo.AreaName));
                list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("OptMan", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("credentialsTime", credentialsTime));//hj20180309 核销日期
                list.Add(new SqlPara("BillNos", billNos));
                list.Add(new SqlPara("BatchNos", batchNos));
                list.Add(new SqlPara("Moneys", moneys));
                list.Add(new SqlPara("FeeTypes", feeTypes));
                list.Add(new SqlPara("WebNames", WebNames));
                list.Add(new SqlPara("HandNums", HandNums));
                list.Add(new SqlPara("OilCardNo", OilCardNo));//hj20180717 油卡卡号

                string strProcName = "USP_ADD_VERIFYOFFACCOUNT";
                //if (CommonClass.IsZhanQuCompanyId(CommonClass.UserInfo.companyid))//战区查询
                //{
                //    strProcName = "USP_ADD_VERIFYOFFACCOUNT_ZhanQu";
                //}
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, strProcName, list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                }

                //同步好多车付款状态 ld20181010
                //if (verifyType == "大车费")
                //{
                //    CommonSyn.SynHaoDuoChePaymentStatus(batchNos, feeTypes);
                //}
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }



        // 添加核销现金日记账可通版本核销 hj20181114
        public void AddVerifyOffAccountKT(string AccountType, string MoneyAccount, string SubjectID, string SubjectName, string VerifyWebs, string summary, string verifyType, string inOutType, string billNos, string batchNos, string moneys, string feeTypes, string WebNames, string HandNums, DateTime credentialsTime)
        {
            try
            {
                Random r1 = new Random();
                int a1 = r1.Next(10, 100);
                string voucherNo = (inOutType == "支出" ? "O" : "I") + DateTime.Now.ToString("yyyyMMddHHmmss")+a1;
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("VoucherNo", voucherNo));
                list.Add(new SqlPara("AccountType", AccountType));//账号类型 hj
                list.Add(new SqlPara("MoneyAccount", MoneyAccount));//资金账号 hj
                list.Add(new SqlPara("SubjectID", SubjectID));
                list.Add(new SqlPara("SubjectName", SubjectName));
                list.Add(new SqlPara("VerifyWebs", VerifyWebs));//核算网点hj
                list.Add(new SqlPara("Summary", summary));
                list.Add(new SqlPara("VerifyOffType", verifyType));
                list.Add(new SqlPara("InOutType", inOutType));
                list.Add(new SqlPara("CauseName", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("AreaName", CommonClass.UserInfo.AreaName));
                list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("OptMan", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("credentialsTime", credentialsTime));//hj20180309 核销日期
                list.Add(new SqlPara("BillNos", billNos));
                list.Add(new SqlPara("BatchNos", batchNos));
                list.Add(new SqlPara("Moneys", moneys));
                list.Add(new SqlPara("FeeTypes", feeTypes));
                list.Add(new SqlPara("WebNames", WebNames));
                list.Add(new SqlPara("HandNums", HandNums));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_VERIFYOFFACCOUNT_KT", list);
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

        /// <summary>
        /// 核销
        /// </summary>
        /// <param name="myGridView2">挑选的清单网格</param>
        /// <param name="ds1">数据集</param>
        /// <param name="verifyType">核销类型</param>
        /// <param name="column1">单号</param>
        /// <param name="column2">批次</param>
        /// <param name="column3">本次核销金额</param>
        /// <param name="balanceName">余额</param>
        /// <param name="inOutType">收入/支出</param>
        public void SubmitVerify(MyGridView myGridView2, DataSet ds1, string verifyType, string column1, string column2, string column3, string balanceName, string inOutType)
        {
            decimal currVerifyFee = 0;//hj20180711
            int num = 0; //票数hj20181113
            myGridView2.PostEditor();
            int rowhandle = myGridView2.FocusedRowHandle;
            if (myGridView2.RowCount == 0)
            {
                XtraMessageBox.Show("没有发现任何需要核销的清单，请先在左侧列表构建清单。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                decimal balance = Convert.ToDecimal(myGridView2.GetRowCellValue(i, balanceName));
                decimal currentVerifyFee = Convert.ToDecimal(myGridView2.GetRowCellValue(i, "CurrentVerifyFee"));
                if (currentVerifyFee == 0)
                {
                    XtraMessageBox.Show("请输入本次核销金额", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (currentVerifyFee > balance)
                {
                    XtraMessageBox.Show("本次核销余额不能大于当前余额", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                currVerifyFee = Convert.ToDecimal(myGridView2.GetRowCellValue(i, "CurrentVerifyFee")) + currVerifyFee;
                num++;
                if (verifyType == "大车费")
                {
                    OilCardNo = myGridView2.GetRowCellValue(i, "OilCardNo").ToString();//hj 20180717 油卡卡号
                    //OilCardFee = ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "OilCardNo"));//hj 20180717 油卡卡号
                }

            }

            string subjectOne = "";
            string subjectTwo = "";
            string subjectThree = "";
            string Verifydirection = "";
            string summary = "";
            string remarks = "";
            string AccountType = "", MoneyAccount = "", SubjectID = "", SubjectName = "";//账号类型+资金账号 
            string hm = "", zh = "", khh = "", szs = "", szshi = "", zzlx = "";//wbw

            if (CommonClass.UserInfo.companyid == "124")
            {
                frmChoiceSubject2 frm = new frmChoiceSubject2();
                frm.xm = verifyType;
                frm.inOutType = inOutType;
                frm.currVerifyFee = currVerifyFee;
                frm.OilCardNo = OilCardNo;
                frm.CardFee = currVerifyFee;
                DialogResult result = frm.ShowDialog();
                if (result == DialogResult.Cancel) return;
                subjectOne = frm.SubjectOne;
                subjectTwo = frm.SubjectTwo;
                subjectThree = frm.SubjectThree;
                Verifydirection = frm.Verifydirection; //hj 核销类型20180104
                summary = frm.Summary;
                remarks = frm.Remarks;
                OilCardNo = frm.OilCardNo;//hj 油卡编号
            }
            //else if (CommonClass.UserInfo.companyid == "309")//同星公司ID是309
            //{
                //frmChooseMoneyAccount frm = new frmChooseMoneyAccount();
            //    frm.Num = num;
            //    frm.Money = currVerifyFee;
            //    DialogResult result = frm.ShowDialog();
            //    if (result == DialogResult.Cancel) return;
            //    AccountType = frm.AccountType;
            //    MoneyAccount = frm.MoneyAccount;
            //    SubjectID = frm.SubjectID;
            //    SubjectName = frm.SubjectName;
            //}
            else if (CommonClass.UserInfo.companyid == "309" || CommonClass.UserInfo.companyid == "490")//同星公司ID是309
            {
                frmChoiceSubjectTX frm = new frmChoiceSubjectTX();
                frm.Num = num;
                frm.Money = currVerifyFee;
                frm.xm = verifyType;
                frm.inOutType = inOutType;
                frm.currVerifyFee = currVerifyFee;
                frm.OilCardNo = OilCardNo;
                frm.CardFee = currVerifyFee;
                DialogResult result = frm.ShowDialog();
                if (result == DialogResult.Cancel) return;
                subjectOne = frm.SubjectOne;
                subjectTwo = frm.SubjectTwo;
                subjectThree = frm.SubjectThree;
                Verifydirection = frm.Verifydirection; //hj 核销类型20180104
                summary = frm.Summary;
                OilCardNo = frm.OilCardNo;//hj 油卡编号

                hm = frm.hm1;
                zh = frm.zh1;
                khh = frm.khh1;
                szs = frm.szs1;
                szshi = frm.szshi1;
                zzlx = frm.zzlx1;

            }
            else
            {
                frmChoiceSubject frm = new frmChoiceSubject();
                frm.xm = verifyType;
                frm.inOutType = inOutType;
                frm.currVerifyFee = currVerifyFee;
                frm.OilCardNo = OilCardNo;
                frm.CardFee = currVerifyFee;
                DialogResult result = frm.ShowDialog();
                if (result == DialogResult.Cancel) return;
                subjectOne = frm.SubjectOne;
                subjectTwo = frm.SubjectTwo;
                subjectThree = frm.SubjectThree;
                Verifydirection = frm.Verifydirection; //hj 核销类型20180104
                summary = frm.Summary;
                OilCardNo = frm.OilCardNo;//hj 油卡编号
            }
            
            
            if (!string.IsNullOrEmpty(subjectOne) && !string.IsNullOrEmpty(subjectTwo))
            {
                string billNoStr = "";
                string batchNoStr = "";
                string moneyStr = "";
                string feeTypes = "";
                string WebNames = "";
                string HandNums = "";
                //DateTime credentialsTime = DateTime.Now;
                DateTime credentialsTime = CommonClass.gcdate;


                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    if (!string.IsNullOrEmpty(column1))
                    {
                        billNoStr += myGridView2.GetRowCellValue(i, column1) + "@";
                    }
                    if (!string.IsNullOrEmpty(column2))
                    {
                        batchNoStr += myGridView2.GetRowCellValue(i, column2) + "@";
                    }
                    moneyStr += myGridView2.GetRowCellValue(i, column3) + "@";
                    if (verifyType == "大车费" || verifyType == "整车装卸费")
                    {
                        feeTypes += myGridView2.GetRowCellValue(i, "FeeType") + "@";
                    }
                    if (verifyType == "始发其他费" || verifyType == "终端其他费")
                    {
                        feeTypes += myGridView2.GetRowCellValue(i, "Project") + "@";
                    }
                    if (ConvertType.ToString(myGridView2.GetRowCellValue(i, "FeeType")) == "大车终端装卸费")
                    {
                        WebNames += ConvertType.ToString(myGridView2.GetRowCellValue(i, "WebName")) + "@";
                    }
                    if ((ConvertType.ToString(myGridView2.GetRowCellValue(i, "FeeType")) == "始发装卸费" || ConvertType.ToString(myGridView2.GetRowCellValue(i, "FeeType")) == "终端装卸费")
                             && CommonClass.UserInfo.UserDB != UserDB.ZQTMS3PL)
                    {
                        HandNums += ConvertType.ToString(myGridView2.GetRowCellValue(i, "HnadNum")) + "@";
                    }
                    credentialsTime = Convert.ToDateTime(myGridView2.GetRowCellValue(i, "credentialsTime"));//hj 20180309可修改的核销日期        
                }

                //AddVerifyOffAccount(subjectOne, subjectTwo, subjectThree, Verifydirection, summary, verifyType, inOutType, billNoStr, batchNoStr, moneyStr, feeTypes, WebNames, HandNums, credentialsTime, remarks);
                AddVerifyOffAccountTX(subjectOne, subjectTwo, subjectThree, Verifydirection, summary, verifyType, inOutType, billNoStr, batchNoStr, moneyStr, feeTypes, WebNames, HandNums, credentialsTime, remarks, hm, zh, khh, szs, szshi,zzlx);
                ds1.Clear();
                
            }

            //hj20181114
            if (string.IsNullOrEmpty(subjectOne) && string.IsNullOrEmpty(subjectTwo))
            {
                string billNoStr = "";
                string batchNoStr = "";
                string moneyStr = "";
                string feeTypes = "";
                string WebNames = "";
                string HandNums = "";
                string VerifyWebs = "";
                //DateTime credentialsTime = DateTime.Now;
                DateTime credentialsTime = CommonClass.gcdate;


                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    if (!string.IsNullOrEmpty(column1))
                    {
                        billNoStr += myGridView2.GetRowCellValue(i, column1) + "@";
                    }
                    if (!string.IsNullOrEmpty(column2))
                    {
                        batchNoStr += myGridView2.GetRowCellValue(i, column2) + "@";
                    }
                    moneyStr += myGridView2.GetRowCellValue(i, column3) + "@";
                    if (verifyType == "大车费" || verifyType == "整车装卸费")
                    {
                        feeTypes += myGridView2.GetRowCellValue(i, "FeeType") + "@";
                    }
                    if (verifyType == "始发其他费" || verifyType == "终端其他费")
                    {
                        feeTypes += myGridView2.GetRowCellValue(i, "Project") + "@";
                    }
                    if (ConvertType.ToString(myGridView2.GetRowCellValue(i, "FeeType")) == "大车终端装卸费")
                    {
                        WebNames += ConvertType.ToString(myGridView2.GetRowCellValue(i, "WebName")) + "@";
                    }
                    if ((ConvertType.ToString(myGridView2.GetRowCellValue(i, "FeeType")).Contains("装卸费"))
                             && CommonClass.UserInfo.UserDB != UserDB.ZQTMS3PL)
                    {
                        HandNums += ConvertType.ToString(myGridView2.GetRowCellValue(i, "HnadNum")) + "@";
                    }
                    credentialsTime = Convert.ToDateTime(myGridView2.GetRowCellValue(i, "credentialsTime"));//hj 20180309可修改的核销日期    
                    //增加核算网点字段
                    if (verifyType == "现付" || verifyType == "月结" || verifyType == "回单付" || verifyType == "短欠" || verifyType == "提付异动" || verifyType == "非提付异动"
                        || verifyType == "折扣费" || verifyType == "货款回收" || verifyType == "垫付费")
                    {
                        VerifyWebs += myGridView2.GetRowCellValue(i, "BegWeb") + "@";
                    }
                    if (verifyType == "提付")
                    {
                        VerifyWebs += myGridView2.GetRowCellValue(i, "OutWebName") + "@";
                    }
                    if (verifyType == "派车费")
                    {
                        VerifyWebs += myGridView2.GetRowCellValue(i, "ControlWeb") + "@";
                    }
                    if (verifyType == "短驳费")
                    {
                        VerifyWebs += myGridView2.GetRowCellValue(i, "SCWeb") + "@";
                    }
                    //if (verifyType == "大车费现付" || verifyType == "大车费回付" || verifyType == "大车费到付" || verifyType == "大车费代收" || verifyType == "大车始发装卸费" || verifyType == "大车油卡费" || verifyType == "大车终端装卸费")
                    if (verifyType == "大车费")
                    {
                        VerifyWebs += myGridView2.GetRowCellValue(i, "LoadWeb") + "@";
                    }
                    if (verifyType == "送货费")
                    {
                        VerifyWebs += myGridView2.GetRowCellValue(i, "SendWeb") + "@";
                    }
                    if (verifyType == "中转费" || verifyType == "转送费")
                    {
                        VerifyWebs += myGridView2.GetRowCellValue(i, "MiddleWebName") + "@";
                    }
                    if (verifyType == "整车装卸费" || verifyType == "始发其他费" || verifyType == "终端其他费")
                    {
                        VerifyWebs += myGridView2.GetRowCellValue(i, "WebName") + "@";
                    }

                }

                AddVerifyOffAccountKT(AccountType, MoneyAccount, SubjectID, SubjectName, VerifyWebs, summary, verifyType, inOutType, billNoStr, batchNoStr, moneyStr, feeTypes, WebNames, HandNums, credentialsTime);
                ds1.Clear();
            }
        }



        /// <summary>
        /// 核销欠款
        /// </summary>
        /// <param name="ds1">数据集</param>
        /// <param name="verifyType">核销类型</param>
        /// <param name="column1">单号</param>
        /// <param name="column2">批次</param>
        /// <param name="column3">本次核销金额</param>
        /// <param name="balanceName">余额</param>
        /// <param name="inOutType">收入/支出</param>
        public void SubmitVerifyNew(DataSet ds1, string column1, string column2, string column3, string balanceName, string inOutType)
        {
            if (ds1 == null || ds1.Tables.Count == 0 || ds1.Tables[0].Rows.Count == 0)
            {
                XtraMessageBox.Show("没有发现任何需要核销的清单，请先在左侧列表构建清单。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DataRow[] drMonth = ds1.Tables[0].Select("FeeType = '月结'");
            DataRow[] drReceipt = ds1.Tables[0].Select("FeeType = '回单付'");
            DataRow[] drBefArrival = ds1.Tables[0].Select("FeeType = '货到前付'");
            DataRow[] drOwe = ds1.Tables[0].Select("FeeType = '欠付'");
            if (drMonth.Length > 0)
            {
                decimal currVerifyFee = 0;

                for (int i = 0; i < drMonth.Length; i++)
                {
                    decimal balance = Convert.ToDecimal(drMonth[i]["AmountLeft"]);
                    decimal currentVerifyFee = Convert.ToDecimal(drMonth[i]["CurrentVerifyFee"]);
                    if (currentVerifyFee == 0)
                    {
                        XtraMessageBox.Show("请输入本次核销金额", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (currentVerifyFee > balance)
                    {
                        XtraMessageBox.Show("本次核销余额不能大于当前余额", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    currVerifyFee = Convert.ToDecimal(drMonth[i]["CurrentVerifyFee"]) + currVerifyFee;
                }

            string subjectOne = "";
            string subjectTwo = "";
            string subjectThree = "";
            string Verifydirection = "";
            string summary = "";
            string remarks = "";
            if (CommonClass.UserInfo.companyid == "124")
            {
                frmChoiceSubject2 frm = new frmChoiceSubject2();
                frm.xm = "月结";
                frm.inOutType = inOutType;
                frm.currVerifyFee = currVerifyFee;
                frm.OilCardNo = OilCardNo;
                frm.CardFee = currVerifyFee;
                DialogResult result = frm.ShowDialog();
                if (result == DialogResult.Cancel) return;
                 subjectOne = frm.SubjectOne;
                 subjectTwo = frm.SubjectTwo;
                 subjectThree = frm.SubjectThree;
                 Verifydirection = frm.Verifydirection; //hj 核销类型20180104
                 summary = frm.Summary;
                 remarks = frm.Remarks;
                OilCardNo = frm.OilCardNo;//hj 油卡编号
            }
            else {
                frmChoiceSubject frm = new frmChoiceSubject();
                frm.xm = "月结";
                frm.inOutType = inOutType;
                frm.currVerifyFee = currVerifyFee;
                frm.OilCardNo = OilCardNo;
                frm.CardFee = currVerifyFee;
                DialogResult result = frm.ShowDialog();
                if (result == DialogResult.Cancel) return;
                 subjectOne = frm.SubjectOne;
                 subjectTwo = frm.SubjectTwo;
                 subjectThree = frm.SubjectThree;
                 Verifydirection = frm.Verifydirection; //hj 核销类型20180104
                 summary = frm.Summary;
                OilCardNo = frm.OilCardNo;//hj 油卡编号
            }
                
               
                if (!string.IsNullOrEmpty(subjectOne) && !string.IsNullOrEmpty(subjectTwo))
                {
                    string billNoStr = "";
                    string batchNoStr = "";
                    string moneyStr = "";
                    string feeTypes = "";
                    string WebNames = "";
                    string HandNums = "";
                    //DateTime credentialsTime = DateTime.Now;
                    DateTime credentialsTime = CommonClass.gcdate;


                    for (int i = 0; i < drMonth.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(column1))
                        {
                            billNoStr += drMonth[i]["BillNo"].ToString() + "@";
                        }
                        moneyStr += drMonth[i]["CurrentVerifyFee"].ToString() + "@";
                        credentialsTime = Convert.ToDateTime(drMonth[i]["credentialsTime"]);//hj 20180309可修改的核销日期        
                    }

                    AddVerifyOffAccount(subjectOne, subjectTwo, subjectThree, Verifydirection, summary, "月结", inOutType, billNoStr, batchNoStr, moneyStr, feeTypes, WebNames, HandNums, credentialsTime, remarks);
                }
            }
            if (drReceipt.Length > 0)
            {
                decimal currVerifyFee = 0;

                for (int i = 0; i < drReceipt.Length; i++)
                {
                    decimal balance = Convert.ToDecimal(drReceipt[i]["AmountLeft"]);
                    decimal currentVerifyFee = Convert.ToDecimal(drReceipt[i]["CurrentVerifyFee"]);
                    if (currentVerifyFee == 0)
                    {
                        XtraMessageBox.Show("请输入本次核销金额", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (currentVerifyFee > balance)
                    {
                        XtraMessageBox.Show("本次核销余额不能大于当前余额", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    currVerifyFee = Convert.ToDecimal(drReceipt[i]["CurrentVerifyFee"]) + currVerifyFee;
                }

                string subjectOne = "";
                string subjectTwo = "";
                string subjectThree = "";
                string Verifydirection = "";
                string summary = "";
                string remarks = "";
                if (CommonClass.UserInfo.companyid == "124")
                {
                    frmChoiceSubject2 frm = new frmChoiceSubject2();
                    frm.xm = "回单付";
                    frm.inOutType = inOutType;
                    frm.currVerifyFee = currVerifyFee;
                    frm.OilCardNo = OilCardNo;
                    frm.CardFee = currVerifyFee;
                    DialogResult result = frm.ShowDialog();
                    if (result == DialogResult.Cancel) return;
                    subjectOne = frm.SubjectOne;
                    subjectTwo = frm.SubjectTwo;
                    subjectThree = frm.SubjectThree;
                    Verifydirection = frm.Verifydirection; //hj 核销类型20180104
                    summary = frm.Summary;
                    remarks = frm.Remarks;
                    OilCardNo = frm.OilCardNo;//hj 油卡编号
                }
                else {
                    frmChoiceSubject frm = new frmChoiceSubject();
                    frm.xm = "回单付";
                    frm.inOutType = inOutType;
                    frm.currVerifyFee = currVerifyFee;
                    frm.OilCardNo = OilCardNo;
                    frm.CardFee = currVerifyFee;
                    DialogResult result = frm.ShowDialog();
                    if (result == DialogResult.Cancel) return;
                    subjectOne = frm.SubjectOne;
                    subjectTwo = frm.SubjectTwo;
                    subjectThree = frm.SubjectThree;
                    Verifydirection = frm.Verifydirection; //hj 核销类型20180104
                    summary = frm.Summary;
                    OilCardNo = frm.OilCardNo;//hj 油卡编号
                }
                
                if (!string.IsNullOrEmpty(subjectOne) && !string.IsNullOrEmpty(subjectTwo))
                {
                    string billNoStr = "";
                    string batchNoStr = "";
                    string moneyStr = "";
                    string feeTypes = "";
                    string WebNames = "";
                    string HandNums = "";
                    //DateTime credentialsTime = DateTime.Now;
                    DateTime credentialsTime = CommonClass.gcdate;


                    for (int i = 0; i < drReceipt.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(column1))
                        {
                            billNoStr += drReceipt[i]["BillNo"].ToString() + "@";
                        }
                        moneyStr += drReceipt[i]["CurrentVerifyFee"].ToString() + "@";
                        credentialsTime = Convert.ToDateTime(drReceipt[i]["credentialsTime"]);//hj 20180309可修改的核销日期        
                    }

                    AddVerifyOffAccount(subjectOne, subjectTwo, subjectThree, Verifydirection, summary, "回单付", inOutType, billNoStr, batchNoStr, moneyStr, feeTypes, WebNames, HandNums, credentialsTime, remarks);
                }
            }
            if (drBefArrival.Length > 0)
            {
                decimal currVerifyFee = 0;

                for (int i = 0; i < drBefArrival.Length; i++)
                {
                    decimal balance = Convert.ToDecimal(drBefArrival[i]["AmountLeft"]);
                    decimal currentVerifyFee = Convert.ToDecimal(drBefArrival[i]["CurrentVerifyFee"]);
                    if (currentVerifyFee == 0)
                    {
                        XtraMessageBox.Show("请输入本次核销金额", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (currentVerifyFee > balance)
                    {
                        XtraMessageBox.Show("本次核销余额不能大于当前余额", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    currVerifyFee = Convert.ToDecimal(drBefArrival[i]["CurrentVerifyFee"]) + currVerifyFee;
                }

                 string subjectOne = "";
                string subjectTwo = "";
                string subjectThree = "";
                string Verifydirection = "";
                string summary = "";
                string remarks = "";
                if (CommonClass.UserInfo.companyid == "124")
                {
                    frmChoiceSubject2 frm = new frmChoiceSubject2();
                    frm.xm = "货到前付";
                    frm.inOutType = inOutType;
                    frm.currVerifyFee = currVerifyFee;
                    frm.OilCardNo = OilCardNo;
                    frm.CardFee = currVerifyFee;
                    DialogResult result = frm.ShowDialog();
                    if (result == DialogResult.Cancel) return;
                    subjectOne = frm.SubjectOne;
                    subjectTwo = frm.SubjectTwo;
                    subjectThree = frm.SubjectThree;
                    Verifydirection = frm.Verifydirection; //hj 核销类型20180104
                    summary = frm.Summary;
                    remarks = frm.Remarks;
                    OilCardNo = frm.OilCardNo;//hj 油卡编号
                }
                else {
                    frmChoiceSubject frm = new frmChoiceSubject();
                    frm.xm = "货到前付";
                    frm.inOutType = inOutType;
                    frm.currVerifyFee = currVerifyFee;
                    frm.OilCardNo = OilCardNo;
                    frm.CardFee = currVerifyFee;
                    DialogResult result = frm.ShowDialog();
                    if (result == DialogResult.Cancel) return;
                    subjectOne = frm.SubjectOne;
                    subjectTwo = frm.SubjectTwo;
                    subjectThree = frm.SubjectThree;
                    Verifydirection = frm.Verifydirection; //hj 核销类型20180104
                    summary = frm.Summary;
                    OilCardNo = frm.OilCardNo;//hj 油卡编号
                }
               
                if (!string.IsNullOrEmpty(subjectOne) && !string.IsNullOrEmpty(subjectTwo))
                {
                    string billNoStr = "";
                    string batchNoStr = "";
                    string moneyStr = "";
                    string feeTypes = "";
                    string WebNames = "";
                    string HandNums = "";
                    //DateTime credentialsTime = DateTime.Now;
                    DateTime credentialsTime = CommonClass.gcdate;


                    for (int i = 0; i < drBefArrival.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(column1))
                        {
                            billNoStr += drBefArrival[i]["BillNo"].ToString() + "@";
                        }
                        moneyStr += drBefArrival[i]["CurrentVerifyFee"].ToString() + "@";
                        credentialsTime = Convert.ToDateTime(drBefArrival[i]["credentialsTime"]);//hj 20180309可修改的核销日期        
                    }

                    AddVerifyOffAccount(subjectOne, subjectTwo, subjectThree, Verifydirection, summary, "货到前付", inOutType, billNoStr, batchNoStr, moneyStr, feeTypes, WebNames, HandNums, credentialsTime,remarks);
                }
            }
            if (drOwe.Length > 0)
            {
                decimal currVerifyFee = 0;

                for (int i = 0; i < drOwe.Length; i++)
                {
                    decimal balance = Convert.ToDecimal(drOwe[i]["AmountLeft"]);
                    decimal currentVerifyFee = Convert.ToDecimal(drOwe[i]["CurrentVerifyFee"]);
                    if (currentVerifyFee == 0)
                    {
                        XtraMessageBox.Show("请输入本次核销金额", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (currentVerifyFee > balance)
                    {
                        XtraMessageBox.Show("本次核销余额不能大于当前余额", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    currVerifyFee = Convert.ToDecimal(drOwe[i]["CurrentVerifyFee"]) + currVerifyFee;
                }

                string subjectOne = "";
                string subjectTwo = "";
                string subjectThree = "";
                string Verifydirection = "";
                string summary = "";
                string remarks = "";
                if (CommonClass.UserInfo.companyid == "124")
                {
                    frmChoiceSubject2 frm = new frmChoiceSubject2();
                    frm.xm = "欠付";
                    frm.inOutType = inOutType;
                    frm.currVerifyFee = currVerifyFee;
                    frm.OilCardNo = OilCardNo;
                    frm.CardFee = currVerifyFee;
                    DialogResult result = frm.ShowDialog();
                    if (result == DialogResult.Cancel) return;
                    subjectOne = frm.SubjectOne;
                    subjectTwo = frm.SubjectTwo;
                    subjectThree = frm.SubjectThree;
                    Verifydirection = frm.Verifydirection; //hj 核销类型20180104
                    summary = frm.Summary;
                    remarks = frm.Remarks;
                    OilCardNo = frm.OilCardNo;//hj 油卡编号
                }
                else {
                    frmChoiceSubject frm = new frmChoiceSubject();
                    frm.xm = "欠付";
                    frm.inOutType = inOutType;
                    frm.currVerifyFee = currVerifyFee;
                    frm.OilCardNo = OilCardNo;
                    frm.CardFee = currVerifyFee;
                    DialogResult result = frm.ShowDialog();
                    if (result == DialogResult.Cancel) return;
                    subjectOne = frm.SubjectOne;
                    subjectTwo = frm.SubjectTwo;
                    subjectThree = frm.SubjectThree;
                    Verifydirection = frm.Verifydirection; //hj 核销类型20180104
                    summary = frm.Summary;
                    OilCardNo = frm.OilCardNo;//hj 油卡编号
                }
                
                if (!string.IsNullOrEmpty(subjectOne) && !string.IsNullOrEmpty(subjectTwo))
                {
                    string billNoStr = "";
                    string batchNoStr = "";
                    string moneyStr = "";
                    string feeTypes = "";
                    string WebNames = "";
                    string HandNums = "";
                    //DateTime credentialsTime = DateTime.Now;
                    DateTime credentialsTime = CommonClass.gcdate;


                    for (int i = 0; i < drOwe.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(column1))
                        {
                            billNoStr += drOwe[i]["BillNo"].ToString() + "@";
                        }
                        moneyStr += drOwe[i]["CurrentVerifyFee"].ToString() + "@";
                        credentialsTime = Convert.ToDateTime(drOwe[i]["credentialsTime"]);//hj 20180309可修改的核销日期        
                    }

                    AddVerifyOffAccount(subjectOne, subjectTwo, subjectThree, Verifydirection, summary, "欠付", inOutType, billNoStr, batchNoStr, moneyStr, feeTypes, WebNames, HandNums, credentialsTime, remarks);
                }
            }
            ds1.Clear();//清空右侧数据，避免重复核销
        }



        /// 添加核销现金日记账同星
        /// </summary>
        /// <param name="oneSubject">一级科目</param>
        /// <param name="twoSubject">二级科目</param>
        /// <param name="threeSubject">三级科目</param>
        /// <param name="summary">摘要</param>
        /// <param name="verifyType">核销费用类型</param>
        /// <param name="inOutType">记账类型 支出、收入</param>
        /// <param name="billNos">运单号字符串 多个用'@'隔开</param>
        /// <param name="moneys">金额字符串 多个用'@'隔开</param>
        public void AddVerifyOffAccountTX(string oneSubject, string twoSubject, string threeSubject, string Verifydirection, string summary, string verifyType, string inOutType, string billNos, string batchNos, string moneys, string feeTypes, string WebNames, string HandNums, DateTime credentialsTime, string remarks, string hm, string zh, string khh, string szs, string szshi,string zzlx)
        {
            try
            {

                Random r1 = new Random();
                int a1 = r1.Next(10, 100);
                string voucherNo = (inOutType == "支出" ? "O" : "I") + DateTime.Now.ToString("yyyyMMddHHmmss")+a1;
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("VoucherNo", voucherNo));
                list.Add(new SqlPara("OneSubject", oneSubject));
                list.Add(new SqlPara("TwoSubject", twoSubject));
                list.Add(new SqlPara("ThreeSubject", threeSubject));
                list.Add(new SqlPara("Verifydirection", Verifydirection));
                list.Add(new SqlPara("Summary", summary));
                list.Add(new SqlPara("Remarks", remarks));//jilei 20180831
                list.Add(new SqlPara("VerifyOffType", verifyType));
                list.Add(new SqlPara("InOutType", inOutType));
                list.Add(new SqlPara("CauseName", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("AreaName", CommonClass.UserInfo.AreaName));
                list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("OptMan", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("credentialsTime", credentialsTime));//hj20180309 核销日期
                list.Add(new SqlPara("BillNos", billNos));
                list.Add(new SqlPara("BatchNos", batchNos));
                list.Add(new SqlPara("Moneys", moneys));
                list.Add(new SqlPara("FeeTypes", feeTypes));
                list.Add(new SqlPara("WebNames", WebNames));
                list.Add(new SqlPara("HandNums", HandNums));
                list.Add(new SqlPara("OilCardNo", OilCardNo));//hj20180717 油卡卡号

                list.Add(new SqlPara("hm", hm));
                list.Add(new SqlPara("zh", zh));
                list.Add(new SqlPara("khh", khh));
                list.Add(new SqlPara("szs", szs));
                list.Add(new SqlPara("szshi", szshi));
                list.Add(new SqlPara("zzlx", zzlx));

                string strProcName = "USP_ADD_VERIFYOFFACCOUNT";
                //if (CommonClass.IsZhanQuCompanyId(CommonClass.UserInfo.companyid))//战区查询
                //{
                //    strProcName = "USP_ADD_VERIFYOFFACCOUNT_ZhanQu";
                //}
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, strProcName, list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK("凭证号：" + voucherNo);
                    MsgBox.ShowOK();
                }

                //同步好多车付款状态 ld20181010
                //if (verifyType == "大车费")
                //{
                //    CommonSyn.SynHaoDuoChePaymentStatus(batchNos, feeTypes);
                //}
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }



    }
}
