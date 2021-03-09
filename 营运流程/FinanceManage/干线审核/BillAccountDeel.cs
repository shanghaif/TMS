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
    public enum BillVerifyType
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
        现付
    }
    public class BillAccountDeel
    {
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
                string voucherNo = (inOutType == "支出" ? "O" : "I") + DateTime.Now.ToString("yyyyMMddHHmmss");
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("VoucherNo", voucherNo));
                list.Add(new SqlPara("OneSubject", oneSubject));
                list.Add(new SqlPara("TwoSubject", twoSubject));
                list.Add(new SqlPara("ThreeSubject", threeSubject));
                list.Add(new SqlPara("Summary", summary));
                list.Add(new SqlPara("Remarks", remarks));
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

                string strProcName = "USP_ADD_VERIFYOFFACCOUNT_GS";
                if (CommonClass.IsZhanQuCompanyId(CommonClass.UserInfo.companyid))//战区查询
                {
                    strProcName = "USP_ADD_VERIFYOFFACCOUNT_GS_ZhanQu";
                }
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, strProcName, list);
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
        public void AddVerifyOffAccount(string oneSubject, string twoSubject, string threeSubject, string summary, string verifyType, string inOutType, string billNos, string batchNos, string moneys, string feeTypes,string WebNames,string InOrOut,string remarks)
        {
            try
            {
                string voucherNo = (inOutType == "支出" ? "O" : "I") + DateTime.Now.ToString("yyyyMMddHHmmss");
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("VoucherNo", voucherNo));
                list.Add(new SqlPara("OneSubject", oneSubject));
                list.Add(new SqlPara("TwoSubject", twoSubject));
                list.Add(new SqlPara("ThreeSubject", threeSubject));
                list.Add(new SqlPara("Summary", summary));
                list.Add(new SqlPara("Remarks", remarks));
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
                list.Add(new SqlPara("InOrOut", InOrOut));

                string strProcName = "USP_ADD_VERIFYOFFACCOUNT_GS";
                if (CommonClass.IsZhanQuCompanyId(CommonClass.UserInfo.companyid))//战区查询
                {
                    strProcName = "USP_ADD_VERIFYOFFACCOUNT_GS_ZhanQu";
                }
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, strProcName, list);
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
        ///// 添加核销现金日记账
        ///// </summary>
        ///// <param name="oneSubject">一级科目</param>
        ///// <param name="twoSubject">二级科目</param>
        ///// <param name="threeSubject">三级科目</param>
        ///// <param name="summary">摘要</param>
        ///// <param name="verifyType">核销费用类型</param>
        ///// <param name="inOutType">记账类型 支出、收入</param>
        ///// <param name="billNos">运单号字符串 多个用'@'隔开</param>
        ///// <param name="moneys">金额字符串 多个用'@'隔开</param>
        //public void AddVerifyOffAccount(string oneSubject, string twoSubject, string threeSubject, string summary, string verifyType, string inOutType, string billNos, string batchNos, string moneys, string feeTypes, string WebNames, string HandNums)
        //{
        //    try
        //    {
        //        string voucherNo = (inOutType == "支出" ? "O" : "I") + DateTime.Now.ToString("yyyyMMddHHmmss");
        //        List<SqlPara> list = new List<SqlPara>();
        //        list.Add(new SqlPara("VoucherNo", voucherNo));
        //        list.Add(new SqlPara("OneSubject", oneSubject));
        //        list.Add(new SqlPara("TwoSubject", twoSubject));
        //        list.Add(new SqlPara("ThreeSubject", threeSubject));
        //        list.Add(new SqlPara("Summary", summary));
        //        list.Add(new SqlPara("VerifyOffType", verifyType));
        //        list.Add(new SqlPara("InOutType", inOutType));
        //        list.Add(new SqlPara("CauseName", CommonClass.UserInfo.CauseName));
        //        list.Add(new SqlPara("AreaName", CommonClass.UserInfo.AreaName));
        //        list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));
        //        list.Add(new SqlPara("OptMan", CommonClass.UserInfo.UserName));
        //        list.Add(new SqlPara("BillNos", billNos));
        //        list.Add(new SqlPara("BatchNos", batchNos));
        //        list.Add(new SqlPara("Moneys", moneys));
        //        list.Add(new SqlPara("FeeTypes", feeTypes));
        //        list.Add(new SqlPara("WebNames", WebNames));
        //        list.Add(new SqlPara("HandNums", HandNums));

        //        SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_VERIFYOFFACCOUNT_GS", list);
        //        if (SqlHelper.ExecteNonQuery(sps) > 0)
        //        {
        //            MsgBox.ShowOK();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MsgBox.ShowException(ex);
        //    }
        //}
        
        public void SubmitVerify(MyGridView myGridView2, DataSet ds1, string verifyType, string column1, string column2, string column3, string balanceName,string InOrOuT)
        {
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
            }
            string subjectOne = "";
            string subjectTwo = "";
            string subjectThree = "";
            string summary = "";
            string remarks = "";
            if (CommonClass.UserInfo.companyid == "124")
            {
                frmChoiceSubject2 frm = new frmChoiceSubject2();
                frm.xm = verifyType;
                DialogResult result = frm.ShowDialog();
                if (result == DialogResult.Cancel) return;
                subjectOne = frm.SubjectOne;
                subjectTwo = frm.SubjectTwo;
                subjectThree = frm.SubjectThree;
                summary = frm.Summary;
                remarks = frm.Remarks;
            }
            else {
                frmChoiceSubject frm = new frmChoiceSubject();
                frm.xm = verifyType;
                DialogResult result = frm.ShowDialog();
                if (result == DialogResult.Cancel) return;
                subjectOne = frm.SubjectOne;
                subjectTwo = frm.SubjectTwo;
                subjectThree = frm.SubjectThree;
                summary = frm.Summary;
            }
            
            if (!string.IsNullOrEmpty(subjectOne) && !string.IsNullOrEmpty(subjectTwo) && !string.IsNullOrEmpty(subjectThree))
            {
                string billNoStr = "";
                string batchNoStr = "";
                string moneyStr = "";
                string feeTypes = "";
                string WebNames = "";
                string HandNums = "";
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
                    if (verifyType == "大车费")
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
                }

                AddVerifyOffAccount(subjectOne, subjectTwo, subjectThree, summary, verifyType, "支出", billNoStr, batchNoStr, moneyStr, feeTypes,WebNames,InOrOuT,remarks);
                ds1.Clear();
            }
        }
    }
}
