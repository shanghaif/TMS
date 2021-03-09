using System;
using System.Collections.Generic;
using System.Text;
using ZQTMS.SqlDAL;
using Newtonsoft.Json;
using System.Data;

namespace ZQTMS.Common
{
    public static class CreateJson
    {
        public static string GetShortJson(List<SqlPara> list_syn, int i, string procName, string billNos, string Batch, string compngyID)
        {
            string BillNoSyns = "";
            string json = "";
            //list_syn.Add(new SqlPara("LoginAreaName", CommonClass.UserInfo.AreaName));
            //list_syn.Add(new SqlPara("LoginCauseName", CommonClass.UserInfo.CauseName));
            //list_syn.Add(new SqlPara("LoginDepartName", CommonClass.UserInfo.DepartName));
            //list_syn.Add(new SqlPara("LoginSiteName", CommonClass.UserInfo.SiteName));
            //list_syn.Add(new SqlPara("LoginWebName", CommonClass.UserInfo.WebName));
            //list_syn.Add(new SqlPara("LoginUserAccount", CommonClass.UserInfo.UserAccount));
            //list_syn.Add(new SqlPara("LoginUserName", CommonClass.UserInfo.UserName));
            try
            {
                if (i == 0 || i == 3)
                {
                    List<SqlPara> list_check = new List<SqlPara>();
                    list_check.Add(new SqlPara("BillNos", billNos));
                    SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_Check_Waybill_Short", list_check);
                    DataSet ds_bill = SqlHelper.GetDataSet_ZQTMS(spe);
                    if (ds_bill == null || ds_bill.Tables.Count == 0 || ds_bill.Tables[0].Rows.Count == 0)
                    {
                        //strState = "失败";
                        //strMessage = "QSP_Check_Waybill_By_FB：不存在需要同步配载至LMS系统的单号！";
                    }
                    else
                    {
                        BillNoSyns = ds_bill.Tables[0].Rows[0]["BillNos"].ToString();
              

                    }
                    //list_syn.Add(new SqlPara("BillNoSyns", BillNoSyns));
                    ////list_syn.Add(new SqlPara("BillData", billJson));//运单数据/分拨数据
                    //list_syn.Add(new SqlPara("OptionType", i == 0 ? "新增短驳" : "单票加入"));
                    //list_syn.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));
                    //list_syn.Add(new SqlPara("SystemType", "ZQTMS"));
                    //list_syn.Add(new SqlPara("ProcName", procName));
                   string strJson = JsonConvert.SerializeObject(list_syn);
                    RequestModel<string> request = new RequestModel<string>();
                    request.Request = strJson;
                    request.OperType = 0;
                    json = JsonConvert.SerializeObject(request);

                    //list_syn.Remove(new SqlPara("BillNoSyns", BillNoSyns));
                    ////list_syn.Add(new SqlPara("BillData", billJson));//运单数据/分拨数据
                    //list_syn.Remove(new SqlPara("OptionType", i == 0 ? "新增短驳" : "单票加入"));
                    //list_syn.Remove(new SqlPara("companyid", CommonClass.UserInfo.companyid));
                    //list_syn.Remove(new SqlPara("SystemType", "ZQTMS"));
                    //list_syn.Remove(new SqlPara("ProcName", procName));
                  
                }
                if (i == 1)
                {
                    //list_syn.Add(new SqlPara("ProcName", procName));
                    string strJson = JsonConvert.SerializeObject(list_syn);
                    RequestModel<string> request = new RequestModel<string>();
                    request.Request = strJson;
                    request.OperType = 0;
                    json = JsonConvert.SerializeObject(request);
                    //list_syn.Remove(new SqlPara("ProcName", procName));
                   
                }
                if (i == 2)
                {
                    //list_syn.Add(new SqlPara("BillNos", billNos + ","));
                    //list_syn.Add(new SqlPara("SCBatchNo", Batch));
                    //list_syn.Add(new SqlPara("ProcName", procName));
                    string strJson = JsonConvert.SerializeObject(list_syn);
                    RequestModel<string> request = new RequestModel<string>();
                    request.Request = strJson;
                    request.OperType = 0;
                    json = JsonConvert.SerializeObject(request);
                    //list_syn.Remove(new SqlPara("BillNos", billNos + ","));
                    //list_syn.Remove(new SqlPara("SCBatchNo", Batch));
                    //list_syn.Remove(new SqlPara("ProcName", procName));
                     
                }
                if (i == 6)
                {
                    //list_syn.Add(new SqlPara("BillNo", billNos));
                    //list_syn.Add(new SqlPara("SCBatchNo", Batch));
                    //list_syn.Add(new SqlPara("ProcName", procName));
                    string strJson = JsonConvert.SerializeObject(list_syn);
                    RequestModel<string> request = new RequestModel<string>();
                    request.Request = strJson;
                    request.OperType = 0;
                    json = JsonConvert.SerializeObject(request);
                    //list_syn.Remove(new SqlPara("BillNo", billNos));
                    //list_syn.Remove(new SqlPara("SCBatchNo", Batch));
                    //list_syn.Remove(new SqlPara("ProcName", procName));
                    

                }

                //list_syn.Remove(new SqlPara("LoginAreaName", CommonClass.UserInfo.AreaName));
                //list_syn.Remove(new SqlPara("LoginCauseName", CommonClass.UserInfo.CauseName));
                //list_syn.Remove(new SqlPara("LoginDepartName", CommonClass.UserInfo.DepartName));
                //list_syn.Remove(new SqlPara("LoginSiteName", CommonClass.UserInfo.SiteName));
                //list_syn.Remove(new SqlPara("LoginWebName", CommonClass.UserInfo.WebName));
                //list_syn.Remove(new SqlPara("LoginUserAccount", CommonClass.UserInfo.UserAccount));
                //list_syn.Remove(new SqlPara("LoginUserName", CommonClass.UserInfo.UserName));
            }
            catch (Exception ex)
            {
                json = "";
            }
            return json;
        }

    }
}
