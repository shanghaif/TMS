using System;
using System.Collections.Generic;
using System.Text;
using ZQTMS.SqlDAL;
using System.Data;
using Newtonsoft.Json;
using ZQTMS.Tool;

namespace ZQTMS.Common
{
    public static class CommonSyn
    {
        /// <summary>
        /// 配载同步
        /// </summary>
        /// <param name="billNoStr"></param>
        public static void BillDepartureSyn(string billNoStr, string departureBatch)
        {
            try
            {
                List<SqlPara> listQuery = new List<SqlPara>();
                listQuery.Add(new SqlPara("BillNOStr", billNoStr));
                listQuery.Add(new SqlPara("DepartureBatch", departureBatch));
                SqlParasEntity spsQuery = new SqlParasEntity(OperType.Query, "QSP_GET_DepartureSysInfo", listQuery);
                DataSet ds = SqlHelper.GetDataSet(spsQuery);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                string dsJson = JsonConvert.SerializeObject(ds);
                RequestModel<string> request = new RequestModel<string>();
                request.Request = dsJson;
                request.OperType = 0;
                string json = JsonConvert.SerializeObject(request);
                //string url = "http://localhost:42936/KDLMSService/ZQTMSDepartureSys";
                // string url = "http://lms.dekuncn.com:7016/KDLMSService/ZQTMSDepartureSys";
                string url = HttpHelper.urlBilldepartureSyn;
                ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                if (model.State != "200")
                {
                    List<SqlPara> listLog = new List<SqlPara>();
                    listLog.Add(new SqlPara("BillNo", billNoStr));
                    listLog.Add(new SqlPara("Batch", departureBatch));
                    listLog.Add(new SqlPara("ErrorNode", "配载"));
                    listLog.Add(new SqlPara("ExceptMessage", model.Message));
                    SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                    SqlHelper.ExecteNonQuery(spsLog);

                    // MsgBox.ShowOK(model.Message);
                }
                string billnoStrs = "";
                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                {
                    billnoStrs = billnoStrs + ds.Tables[0].Rows[k]["BillNo"].ToString() + ",";
                }
                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_billDepartureList_lms"));
                listAsy.Add(new SqlPara("FaceUrl", url));
                listAsy.Add(new SqlPara("FaceJson", json));
                listAsy.Add(new SqlPara("BillNos", billnoStrs));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "配载"));
                listAsy.Add(new SqlPara("SystemSource", "LMS干线升级版"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));

                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 送货同步
        /// </summary>
        /// <param name="billnos"></param>
        public static void BillSendGoodsSyn(string billnos, string sendBatch)
        {
            try
            {
                List<SqlPara> listQuery = new List<SqlPara>();
                listQuery.Add(new SqlPara("BillNOStr", billnos));
                listQuery.Add(new SqlPara("SendBatch", sendBatch));
                SqlParasEntity spsQuery = new SqlParasEntity(OperType.Query, "QSP_GET_BillSendGoodSynInfo", listQuery);
                DataSet dsQuery = SqlHelper.GetDataSet(spsQuery);
                if (dsQuery == null || dsQuery.Tables.Count == 0 || dsQuery.Tables[0].Rows.Count == 0) return;
                string dsJson = JsonConvert.SerializeObject(dsQuery);
                RequestModel<string> request = new RequestModel<string>();
                request.Request = dsJson;
                request.OperType = 0;
                string json = JsonConvert.SerializeObject(request);
                //string url = "http://localhost:42936/KDLMSService/ZQTMSBillSendGoodsSyn";
                //string url = "http://lms.dekuncn.com:7016/KDLMSService/ZQTMSBillSendGoodsSyn";
                string url = HttpHelper.urlBillSendSys;
                ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                if (model.State != "200")
                {
                    List<SqlPara> listLog = new List<SqlPara>();
                    listLog.Add(new SqlPara("BillNo", billnos));
                    listLog.Add(new SqlPara("Batch", sendBatch));
                    listLog.Add(new SqlPara("ErrorNode", "送货，转二级"));
                    listLog.Add(new SqlPara("ExceptMessage", model.Message));
                    SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                    SqlHelper.ExecteNonQuery(spsLog);

                    //  MsgBox.ShowOK(model.Message);
                }
                //插入异步同步信息表


                string billnoStrs = "";
                for (int k = 0; k < dsQuery.Tables[0].Rows.Count; k++)
                {
                    billnoStrs = billnoStrs + dsQuery.Tables[0].Rows[k]["BillNo"].ToString() + ",";
                }
                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_BillSendGoods_syn"));
                listAsy.Add(new SqlPara("FaceUrl", url));
                listAsy.Add(new SqlPara("FaceJson", json));
                listAsy.Add(new SqlPara("BillNos", billnoStrs));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "送货，转二级"));
                listAsy.Add(new SqlPara("SystemSource", "LMS干线升级版"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));

                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 到货确认同步
        /// </summary>
        /// <param name="DepartureBatch"></param>
        public static void BillArrivalConfirmSyn(string DepartureBatch, string BillNOStr, int type)
        {
            try
            {
                List<SqlPara> listQuery = new List<SqlPara>();
                listQuery.Add(new SqlPara("BillNOStr", BillNOStr));
                listQuery.Add(new SqlPara("DepartureBatch", DepartureBatch));
                listQuery.Add(new SqlPara("Type", type));
                SqlParasEntity spsQuery = new SqlParasEntity(OperType.Query, "QSP_ArrivalConfirmSyn", listQuery);
                DataSet dsQuery = SqlHelper.GetDataSet(spsQuery);
                if (dsQuery == null || dsQuery.Tables.Count == 0 || dsQuery.Tables[0].Rows.Count == 0) return;
                string dsJson = JsonConvert.SerializeObject(dsQuery);
                RequestModel<string> request = new RequestModel<string>();
                request.Request = dsJson;
                request.OperType = 0;
                string json = JsonConvert.SerializeObject(request);
                // string url = "http://localhost:42936/KDLMSService/ZQTMSArrivalConfirmSyn";
                //string url = "http://lms.dekuncn.com:7016/KDLMSService/ZQTMSBillSendGoodsSyn";
                string url = HttpHelper.urlArrivalConfirmSyn;
                ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                if (model.State != "200")
                {
                    List<SqlPara> listLog = new List<SqlPara>();
                    listLog.Add(new SqlPara("BillNo", BillNOStr));
                    listLog.Add(new SqlPara("Batch", DepartureBatch));
                    listLog.Add(new SqlPara("ErrorNode", "到货确认"));
                    listLog.Add(new SqlPara("ExceptMessage", model.Message));
                    SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                    SqlHelper.ExecteNonQuery(spsLog);
                    // MsgBox.ShowOK(model.Message);
                }
                //插入异步同步信息表

                string billnoStrs = "";
                for (int k = 0; k < dsQuery.Tables[0].Rows.Count; k++)
                {
                    billnoStrs = billnoStrs + dsQuery.Tables[0].Rows[k]["BillNo"].ToString() + ",";
                }
                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_ArrivalConfirm_syn"));
                listAsy.Add(new SqlPara("FaceUrl", url));
                listAsy.Add(new SqlPara("FaceJson", json));
                listAsy.Add(new SqlPara("BillNos", billnoStrs));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "到货确认"));
                listAsy.Add(new SqlPara("SystemSource", "LMS干线升级版"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));


                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 中转同步
        /// </summary>
        /// <param name="BillNos"></param>
        public static void MiddleSyn(string BillNos)
        {
            try
            {
                List<SqlPara> listQuery = new List<SqlPara>();
                listQuery.Add(new SqlPara("BillNos", BillNos));
                SqlParasEntity spsQuery = new SqlParasEntity(OperType.Query, "QSP_ZQTMSMiddleSYS", listQuery);
                DataSet ds = SqlHelper.GetDataSet(spsQuery);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                string dsJson = JsonConvert.SerializeObject(ds);
                RequestModel<string> request = new RequestModel<string>();
                request.Request = dsJson;
                request.OperType = 0;
                string json = JsonConvert.SerializeObject(request);
                //string url = "http://localhost:42936/KDLMSService/ZQTMSMiddleSys";
                // string url = "http://lms.dekuncn.com:7016/KDLMSService/ZQTMSMiddleSys";
                string url = HttpHelper.urlMiddleSyn;


                ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                if (model.State != "200")
                {
                    List<SqlPara> listLog = new List<SqlPara>();
                    listLog.Add(new SqlPara("BillNo", BillNos));
                    listLog.Add(new SqlPara("Batch", ""));
                    listLog.Add(new SqlPara("ErrorNode", "中转"));
                    listLog.Add(new SqlPara("ExceptMessage", model.Message));
                    SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                    SqlHelper.ExecteNonQuery(spsLog);
                    //MsgBox.ShowOK(model.Message);
                }
                //插入异步同步信息表

                string billnoStrs = "";
                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                {
                    billnoStrs = billnoStrs + ds.Tables[0].Rows[k]["BillNo"].ToString() + ",";
                }
                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_MiddleInfo_sys"));
                listAsy.Add(new SqlPara("FaceUrl", url));
                listAsy.Add(new SqlPara("FaceJson", json));
                listAsy.Add(new SqlPara("BillNos", billnoStrs));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "中转"));
                listAsy.Add(new SqlPara("SystemSource", "LMS干线升级版"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));

                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 拨入接收同步
        /// </summary>
        /// <param name="billnos"></param>
        public static void BillDepartureFBAcceptSyn(string billnos, string HDBillno)
        {
            try
            {
                List<SqlPara> listQuery = new List<SqlPara>();
                listQuery.Add(new SqlPara("BillNOStr", billnos));
                listQuery.Add(new SqlPara("HDBillno", HDBillno));

                SqlParasEntity spsQuery = new SqlParasEntity(OperType.Query, "QSP_GET_BillDepartureFBAcceptSyn", listQuery);
                DataSet dsQuery = SqlHelper.GetDataSet(spsQuery);
                if (dsQuery == null || dsQuery.Tables.Count == 0 || dsQuery.Tables[0].Rows.Count == 0) return;
                string dsJson = JsonConvert.SerializeObject(dsQuery);
                RequestModel<string> request = new RequestModel<string>();
                request.Request = dsJson;
                request.OperType = 0;
                string json = JsonConvert.SerializeObject(request);
                // string url = "http://localhost:42936/KDLMSService/ZQTMSBillDepartueFBAcceptSyn";
                //string url = "http://lms.dekuncn.com:7016/KDLMSService/ZQTMSBillSendGoodsSyn";
                //string url = HttpHelper.urlAllocateAcceptSyn;
                ////string url = "http://localhost:46663/KDLMSService/ZQTMSBillDepartueFBAcceptSyn";
                //ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                //if (model.State != "200")
                //{
                //    List<SqlPara> listLog = new List<SqlPara>();
                //    listLog.Add(new SqlPara("BillNo", billnos));
                //    listLog.Add(new SqlPara("Batch", ""));
                //    listLog.Add(new SqlPara("ErrorNode", "拨入接收"));
                //    listLog.Add(new SqlPara("ExceptMessage", model.Message));
                //    SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                //    SqlHelper.ExecteNonQuery(spsLog);
                //   // MsgBox.ShowOK(model.Message);
                //}
                //插入异步同步信息表

                string billnoStrs = "";
                for (int k = 0; k < dsQuery.Tables[0].Rows.Count; k++)
                {
                    billnoStrs = billnoStrs + dsQuery.Tables[0].Rows[k]["BillNo"].ToString() + ",";
                }
                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_BillDepartureFBAccept_syn"));
                listAsy.Add(new SqlPara("FaceUrl", ""));
                listAsy.Add(new SqlPara("FaceJson", json));
                listAsy.Add(new SqlPara("BillNos", billnoStrs));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "拨入接收"));
                listAsy.Add(new SqlPara("SystemSource", "TMS"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));

                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 分拨取消接收同步 hj 20180412
        /// </summary>
        /// <param name="billnos"></param>
        public static void BillDepartureFBCancelSyn(string billnos)
        {
            try
            {
                List<SqlPara> listQuery = new List<SqlPara>();
                listQuery.Add(new SqlPara("BillNOStr", billnos));
                SqlParasEntity spsQuery = new SqlParasEntity(OperType.Query, "QSP_GET_BillDepartureFBCancelSyn", listQuery);
                DataSet dsQuery = SqlHelper.GetDataSet(spsQuery);
                if (dsQuery == null || dsQuery.Tables.Count == 0 || dsQuery.Tables[0].Rows.Count == 0) return;
                string dsJson = JsonConvert.SerializeObject(dsQuery);
                RequestModel<string> request = new RequestModel<string>();
                request.Request = dsJson;
                request.OperType = 0;
                string json = JsonConvert.SerializeObject(request);
                // string url = "http://localhost:42936/KDLMSService/ZQTMSBillDepartueFBAcceptSyn";
                //string url = "http://lms.dekuncn.com:7016/KDLMSService/ZQTMSBillSendGoodsSyn";
                string url = HttpHelper.urlAllocateCancelSyn;
                //string url = "http://localhost:46663/KDLMSService/ZQTMSBillDepartueFBCancelSyn";
                ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                if (model.State != "200")
                {
                    //插入错误日志
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("BillNo", billnos));
                    list.Add(new SqlPara("Batch", ""));
                    list.Add(new SqlPara("ErrorNode", "分拨取消接收"));
                    list.Add(new SqlPara("ExceptMessage", model.Message));
                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", list);
                    SqlHelper.ExecteNonQuery(sps);
                    //MsgBox.ShowOK(model.Message);
                }
                //插入异步同步信息表

                string billnoStrs = "";
                for (int k = 0; k < dsQuery.Tables[0].Rows.Count; k++)
                {
                    billnoStrs = billnoStrs + dsQuery.Tables[0].Rows[k]["BillNo"].ToString() + ",";
                }
                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_BillDepartureFBCancel_syn"));
                listAsy.Add(new SqlPara("FaceUrl", url));
                listAsy.Add(new SqlPara("FaceJson", json));
                listAsy.Add(new SqlPara("BillNos", billnoStrs));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "分拨取消接收"));
                listAsy.Add(new SqlPara("SystemSource", "LMS干线升级版"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));

                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 转二级到货确认同步
        /// </summary>
        /// <param name="SendBatch"></param>
        public static void SendToSiteSyn(string SendBatch)
        {
            try
            {
                List<SqlPara> listQuery = new List<SqlPara>();
                // listQuery.Add(new SqlPara("BillNOStr", ""));
                listQuery.Add(new SqlPara("SendBatch", SendBatch));
                //listQuery.Add(new SqlPara("Type", 0));
                SqlParasEntity spsQuery = new SqlParasEntity(OperType.Query, "QSP_GET_SendToSiteSyn", listQuery);
                DataSet dsQuery = SqlHelper.GetDataSet(spsQuery);
                if (dsQuery == null || dsQuery.Tables.Count == 0 || dsQuery.Tables[0].Rows.Count == 0) return;
                string dsJson = JsonConvert.SerializeObject(dsQuery);
                RequestModel<string> request = new RequestModel<string>();
                request.Request = dsJson;
                request.OperType = 0;
                string json = JsonConvert.SerializeObject(request);
                //string url = "http://localhost:42936/KDLMSService/ZQTMSSendToSiteSyn";
                //string url = "http://lms.dekuncn.com:7016/KDLMSService/ZQTMSBillSendGoodsSyn";
                // string url = HttpHelper.urlArrivalConfirmSyn;
                string url = HttpHelper.urlSendToSiteConfirm;
                ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                if (model.State != "200")
                {
                    List<SqlPara> listLog = new List<SqlPara>();
                    listLog.Add(new SqlPara("BillNo", ""));
                    listLog.Add(new SqlPara("Batch", SendBatch));
                    listLog.Add(new SqlPara("ErrorNode", "转二级到货确认"));
                    listLog.Add(new SqlPara("ExceptMessage", model.Message));
                    SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                    SqlHelper.ExecteNonQuery(spsLog);
                    // MsgBox.ShowOK(model.Message);
                }
                //插入异步同步信息表
                string billnoStrs = "";
                for (int k = 0; k < dsQuery.Tables[0].Rows.Count; k++)
                {
                    billnoStrs = billnoStrs + dsQuery.Tables[0].Rows[k]["BillNO"].ToString() + ",";
                }
                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_SendToSiteSyn"));
                listAsy.Add(new SqlPara("FaceUrl", url));
                listAsy.Add(new SqlPara("FaceJson", json));
                listAsy.Add(new SqlPara("BillNos", billnoStrs));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "转二级到货确认"));
                listAsy.Add(new SqlPara("SystemSource", "LMS干线升级版"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));


                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }

        /// <summary>
        /// 取消分单配载同步
        /// </summary>
        public static void DepartureDeleteSyn(string departureBatch, string billNos, int type)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", departureBatch));
                list.Add(new SqlPara("BillNos", billNos));
                list.Add(new SqlPara("Type", type));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_DepartureDeleteSyn", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                string dsJson = JsonConvert.SerializeObject(ds);
                RequestModel<string> request = new RequestModel<string>();
                request.Request = dsJson;
                request.OperType = 0;
                string json = JsonConvert.SerializeObject(request);
                string url = HttpHelper.urlDepartureDeleteSyn;
                ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                if (model.State != "200")
                {
                    List<SqlPara> listLog = new List<SqlPara>();
                    listLog.Add(new SqlPara("BillNo", billNos));
                    listLog.Add(new SqlPara("Batch", departureBatch));
                    listLog.Add(new SqlPara("ErrorNode", "分单配载单票剔除，整车作废"));
                    listLog.Add(new SqlPara("ExceptMessage", model.Message));
                    SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                    SqlHelper.ExecteNonQuery(spsLog);
                    //MsgBox.ShowOK(model.Message);
                }
                //插入异步同步信息表
                string billnoStrs = "";
                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                {
                    billnoStrs = billnoStrs + ds.Tables[0].Rows[k]["BillNO"].ToString() + ",";
                }
                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_SendToSiteSyn"));
                listAsy.Add(new SqlPara("FaceUrl", url));
                listAsy.Add(new SqlPara("FaceJson", json));
                listAsy.Add(new SqlPara("BillNos", billnoStrs));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "分单配载单票剔除，整车作废"));
                listAsy.Add(new SqlPara("SystemSource", "LMS干线升级版"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));

                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        /// <summary>
        /// 取消到货确认同步
        /// </summary>
        /// <param name="departureBatch"></param>
        /// <param name="billNos"></param>
        /// <param name="type"></param>
        public static void ArrivalConfirmCancelSyn(string departureBatch, string billNos, int type)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", departureBatch));
                list.Add(new SqlPara("BillNos", billNos));
                list.Add(new SqlPara("Type", type));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ArrivalConfirmCalcelSyn", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                string dsJson = JsonConvert.SerializeObject(ds);
                RequestModel<string> request = new RequestModel<string>();
                request.Request = dsJson;
                request.OperType = 0;
                string json = JsonConvert.SerializeObject(request);
                string url = HttpHelper.urlArrivalConfirmCancel;
                ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                if (model.State != "200")
                {
                    List<SqlPara> listLog = new List<SqlPara>();
                    listLog.Add(new SqlPara("BillNo", billNos));
                    listLog.Add(new SqlPara("Batch", departureBatch));
                    listLog.Add(new SqlPara("ErrorNode", "取消到货确认"));
                    listLog.Add(new SqlPara("ExceptMessage", model.Message));
                    SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                    SqlHelper.ExecteNonQuery(spsLog);
                    //MsgBox.ShowOK(model.Message);
                }
                //插入异步同步信息表
                string billnoStrs = "";
                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                {
                    billnoStrs = billnoStrs + ds.Tables[0].Rows[k]["BillNO"].ToString() + ",";
                }
                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_DepartureDeleteSyn"));
                listAsy.Add(new SqlPara("FaceUrl", url));
                listAsy.Add(new SqlPara("FaceJson", json));
                listAsy.Add(new SqlPara("BillNos", billnoStrs));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "取消到货确认"));
                listAsy.Add(new SqlPara("SystemSource", "LMS干线升级版"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));


                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 取消送货
        /// </summary>
        /// <param name="sendBatch"></param>
        /// <param name="billNos"></param>
        /// <param name="type"></param>
        public static void BillSendCancelSyn(string sendBatch, string billNos, int type)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SendBatch", sendBatch));
                list.Add(new SqlPara("BillNos", billNos));
                list.Add(new SqlPara("Type", type));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BillSendCancelSyn", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                string dsJson = JsonConvert.SerializeObject(ds);
                RequestModel<string> request = new RequestModel<string>();
                request.Request = dsJson;
                request.OperType = 0;
                string json = JsonConvert.SerializeObject(request);
                string url = HttpHelper.urlBillSendCancelSyn;
                ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                if (model.State != "200")
                {
                    List<SqlPara> listLog = new List<SqlPara>();
                    listLog.Add(new SqlPara("BillNo", billNos));
                    listLog.Add(new SqlPara("Batch", sendBatch));
                    listLog.Add(new SqlPara("ErrorNode", "取消送货"));
                    listLog.Add(new SqlPara("ExceptMessage", model.Message));
                    SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                    SqlHelper.ExecteNonQuery(spsLog);
                    // MsgBox.ShowOK(model.Message);
                }
                //插入异步同步信息表
                string billnoStrs = "";
                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                {
                    billnoStrs = billnoStrs + ds.Tables[0].Rows[k]["BillNO"].ToString() + ",";
                }
                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_BillSendCancelSyn"));
                listAsy.Add(new SqlPara("FaceUrl", url));
                listAsy.Add(new SqlPara("FaceJson", json));
                listAsy.Add(new SqlPara("BillNos", billnoStrs));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "取消送货"));
                listAsy.Add(new SqlPara("SystemSource", "LMS干线升级版"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));


                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        /// <summary>
        /// 取消转二级到货
        /// </summary>
        /// <param name="sendBatch"></param>
        public static void SendToSiteConfirmCancelSyn(string sendBatch)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SendBatch", sendBatch));
                // list.Add(new SqlPara("BillNos", billNos));
                // list.Add(new SqlPara("Type", type));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SendToSiteConfirmCancel", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                string dsJson = JsonConvert.SerializeObject(ds);
                RequestModel<string> request = new RequestModel<string>();
                request.Request = dsJson;
                request.OperType = 0;
                string json = JsonConvert.SerializeObject(request);
                string url = HttpHelper.urlSendtoSiteConfirmCancel;
                ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                if (model.State != "200")
                {
                    List<SqlPara> listLog = new List<SqlPara>();
                    listLog.Add(new SqlPara("BillNo", ""));
                    listLog.Add(new SqlPara("Batch", sendBatch));
                    listLog.Add(new SqlPara("ErrorNode", "取消转二级到货"));
                    listLog.Add(new SqlPara("ExceptMessage", model.Message));
                    SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                    SqlHelper.ExecteNonQuery(spsLog);
                    // MsgBox.ShowOK(model.Message);
                }
                //插入异步同步信息表
                string billnoStrs = "";
                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                {
                    billnoStrs = billnoStrs + ds.Tables[0].Rows[k]["BillNO"].ToString() + ",";
                }
                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_SendToSiteConfircCancelSyn"));
                listAsy.Add(new SqlPara("FaceUrl", url));
                listAsy.Add(new SqlPara("FaceJson", json));
                listAsy.Add(new SqlPara("BillNos", billnoStrs));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "取消转二级到货"));
                listAsy.Add(new SqlPara("SystemSource", "LMS干线升级版"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));


                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 取消中转
        /// </summary>
        /// <param name="billNo"></param>
        public static void MiddleCancel(string billNo)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", billNo));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_MiddleCancel", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                string dsJson = JsonConvert.SerializeObject(ds);
                RequestModel<string> request = new RequestModel<string>();
                request.Request = dsJson;
                request.OperType = 0;
                string json = JsonConvert.SerializeObject(request);
                string url = HttpHelper.urlMiddleCancel;
                ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                if (model.State != "200")
                {
                    List<SqlPara> listLog = new List<SqlPara>();
                    listLog.Add(new SqlPara("BillNo", billNo));
                    listLog.Add(new SqlPara("Batch", ""));
                    listLog.Add(new SqlPara("ErrorNode", "取消中转"));
                    listLog.Add(new SqlPara("ExceptMessage", model.Message));
                    SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                    SqlHelper.ExecteNonQuery(spsLog);
                    // MsgBox.ShowOK(model.Message);
                }
                //插入异步同步信息表
                string billnoStrs = "";
                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                {
                    billnoStrs = billnoStrs + ds.Tables[0].Rows[k]["BillNO"].ToString() + ",";
                }
                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_MiddleCancelSyn"));
                listAsy.Add(new SqlPara("FaceUrl", url));
                listAsy.Add(new SqlPara("FaceJson", json));
                listAsy.Add(new SqlPara("BillNos", billnoStrs));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "取消中转"));
                listAsy.Add(new SqlPara("SystemSource", "LMS干线升级版"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));


                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 取消签收
        /// </summary>
        public static void SignCancelSyn(string billNo)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", billNo));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SignCancelSyn", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                string dsJson = JsonConvert.SerializeObject(ds);
                RequestModel<string> request = new RequestModel<string>();
                request.Request = dsJson;
                request.OperType = 0;
                string json = JsonConvert.SerializeObject(request);
                string url = HttpHelper.urlSignCancelSyn;
                ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                if (model.State != "200")
                {
                    List<SqlPara> listLog = new List<SqlPara>();
                    listLog.Add(new SqlPara("BillNo", billNo));
                    listLog.Add(new SqlPara("Batch", ""));
                    listLog.Add(new SqlPara("ErrorNode", "取消签收"));
                    listLog.Add(new SqlPara("ExceptMessage", model.Message));
                    SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                    SqlHelper.ExecteNonQuery(spsLog);
                    // MsgBox.ShowOK(model.Message);
                }
                //插入异步同步信息表
                string billnoStrs = "";
                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                {
                    billnoStrs = billnoStrs + ds.Tables[0].Rows[k]["BillNo"].ToString() + ",";
                }
                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_SignCancelSyn"));
                listAsy.Add(new SqlPara("FaceUrl", url));
                listAsy.Add(new SqlPara("FaceJson", json));
                listAsy.Add(new SqlPara("BillNos", billnoStrs));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "取消签收同步"));
                listAsy.Add(new SqlPara("SystemSource", "LMS干线升级版"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));


                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 放货同步
        /// </summary>
        /// <param name="billNo"></param>
        public static void ReleaseSyn(string billNo, string consigneeCompany_K, string consigneeName_K, string consigneePhone_K, string consigneeCellPhone_K)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", billNo));
                list.Add(new SqlPara("ConsigneeCompany_K", consigneeCompany_K));
                list.Add(new SqlPara("ConsigneeName_K", consigneeName_K));
                list.Add(new SqlPara("ConsigneePhone_K", consigneePhone_K));
                list.Add(new SqlPara("ConsigneeCellPhone_K", consigneeCellPhone_K));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ReleaseSyn", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                string dsJson = JsonConvert.SerializeObject(ds);
                RequestModel<string> request = new RequestModel<string>();
                request.Request = dsJson;
                request.OperType = 0;
                string json = JsonConvert.SerializeObject(request);
                string url = HttpHelper.urlReleaseSyn;
                ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                if (model.State != "200")
                {
                    List<SqlPara> listLog = new List<SqlPara>();
                    listLog.Add(new SqlPara("BillNo", billNo));
                    listLog.Add(new SqlPara("Batch", ""));
                    listLog.Add(new SqlPara("ErrorNode", "放货同步"));
                    listLog.Add(new SqlPara("ExceptMessage", model.Message));
                    SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                    SqlHelper.ExecteNonQuery(spsLog);
                    // MsgBox.ShowOK(model.Message);
                }

                //插入异步同步信息表
                string billnoStrs = "";
                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                {
                    billnoStrs = billnoStrs + ds.Tables[0].Rows[k]["BillNo"].ToString() + ",";
                }
                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_ReleaseSyn"));
                listAsy.Add(new SqlPara("FaceUrl", url));
                listAsy.Add(new SqlPara("FaceJson", json));
                listAsy.Add(new SqlPara("BillNos", billnoStrs));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "放货同步"));
                listAsy.Add(new SqlPara("SystemSource", "LMS干线升级版"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));


                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                List<SqlPara> listLog = new List<SqlPara>();
                listLog.Add(new SqlPara("BillNo", billNo));
                listLog.Add(new SqlPara("Batch", ""));
                listLog.Add(new SqlPara("ErrorNode", "放货同步"));
                listLog.Add(new SqlPara("ExceptMessage", ex.Message));
                SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                SqlHelper.ExecteNonQuery(spsLog);
                //MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// （送货、中转、提货、异常）签收同步时效
        /// </summary>
        public static void SignTimeSyn(string billNos, string signType)
        {
            try
            {
                if (string.IsNullOrEmpty(billNos))
                {
                    return;
                }
                List<SqlPara> list_check = new List<SqlPara>();
                DataSet ds = null;
                if (!string.IsNullOrEmpty(billNos))
                {
                    list_check.Add(new SqlPara("BillNos", billNos));
                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_CheckBillFB", list_check);
                    ds = SqlHelper.GetDataSet(sps);
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                }
                billNos = "";
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    billNos = billNos + row["BillNo"].ToString() + "@";
                }

                List<string> list = new List<string>();
                list.Add(billNos);
                list.Add(CommonClass.UserInfo.WebName);
                list.Add(signType);
                string dsJson = JsonConvert.SerializeObject(list);
                RequestModel<string> request = new RequestModel<string>();
                request.Request = dsJson;
                request.OperType = 0;
                string json = JsonConvert.SerializeObject(request);
                string url = HttpHelper.urlSignTimeSyn;//获取接口地址
                ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                if (model.State != "200")
                {
                    List<SqlPara> listLog = new List<SqlPara>();
                    listLog.Add(new SqlPara("BillNo", billNos));
                    listLog.Add(new SqlPara("Batch", ""));
                    listLog.Add(new SqlPara("ErrorNode", signType));
                    listLog.Add(new SqlPara("ExceptMessage", model.Message + "《时效同步》"));
                    SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                    SqlHelper.ExecteNonQuery(spsLog);
                    // MsgBox.ShowOK(model.Message);
                }
                //插入异步同步信息表
                string billnoStrs = "";
                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                {
                    billnoStrs = billnoStrs + ds.Tables[0].Rows[k]["BillNo"].ToString() + ",";
                }
                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_SignTimeSyn"));
                listAsy.Add(new SqlPara("FaceUrl", url));
                listAsy.Add(new SqlPara("FaceJson", json));
                listAsy.Add(new SqlPara("BillNos", billnoStrs));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", signType));
                listAsy.Add(new SqlPara("SystemSource", "LMS干线升级版"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));

                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 同步取消时效
        /// </summary>
        public static void TimeCancelSyn(string BillNos, string SCBatchNo, string SendWeb, string ProcedureName)
        {
            try
            {
                if (string.IsNullOrEmpty(ProcedureName) || (string.IsNullOrEmpty(BillNos) && string.IsNullOrEmpty(SCBatchNo)) || (!string.IsNullOrEmpty(BillNos) && !BillNos.Contains("@")))
                {
                    return;
                }
                List<SqlPara> list_check = new List<SqlPara>();
                DataSet ds = null;
                if (!string.IsNullOrEmpty(BillNos))
                {
                    list_check.Add(new SqlPara("BillNos", BillNos));
                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_CheckBillFB", list_check);
                    ds = SqlHelper.GetDataSet(sps);
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                }
                if (!string.IsNullOrEmpty(SCBatchNo))
                {
                    list_check.Add(new SqlPara("DepartureBatch", SCBatchNo));
                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_CheckBillFB", list_check);
                    ds = SqlHelper.GetDataSet(sps);
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                }
                BillNos = "";
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    BillNos = BillNos + row["BillNo"].ToString() + "@";
                }

                List<string> list = new List<string>();
                list.Add(BillNos);
                list.Add(SCBatchNo);
                list.Add(SendWeb);
                list.Add(ProcedureName);
                string dsJson = JsonConvert.SerializeObject(list);
                RequestModel<string> request = new RequestModel<string>();
                request.Request = dsJson;
                request.OperType = 0;
                string json = JsonConvert.SerializeObject(request);
                string url = HttpHelper.urlTimeCancelSyn;//获取接口地址
                ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                if (model.State != "200")
                {
                    List<SqlPara> listLog = new List<SqlPara>();
                    listLog.Add(new SqlPara("BillNo", BillNos));
                    listLog.Add(new SqlPara("Batch", SCBatchNo));
                    listLog.Add(new SqlPara("ErrorNode", ProcedureName));
                    listLog.Add(new SqlPara("ExceptMessage", model.Message + "《时效同步》"));
                    SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                    SqlHelper.ExecteNonQuery(spsLog);
                    // MsgBox.ShowOK(model.Message);
                }
                //插入异步同步信息表
                string billnoStrs = "";
                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                {
                    billnoStrs = billnoStrs + ds.Tables[0].Rows[k]["BillNo"].ToString() + ",";
                }
                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_TimeCancelSyn"));
                listAsy.Add(new SqlPara("FaceUrl", url));
                listAsy.Add(new SqlPara("FaceJson", json));
                listAsy.Add(new SqlPara("BillNos", billnoStrs));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "同步取消时效"));
                listAsy.Add(new SqlPara("SystemSource", "LMS干线升级版"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));


                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 同步配载修改时效
        /// </summary>
        public static void TimeDepartUptSyn(string BillNos, string DepartureBatch, string WebDate, string DepartTWeb, string LoginWebName, string ProcedureName)
        {
            try
            {
                if (string.IsNullOrEmpty(ProcedureName) || string.IsNullOrEmpty(BillNos) || string.IsNullOrEmpty(DepartureBatch))
                {
                    return;
                }
                List<SqlPara> list_check = new List<SqlPara>();
                DataSet ds = null;
                if (!string.IsNullOrEmpty(BillNos))
                {
                    list_check.Add(new SqlPara("BillNos", BillNos));
                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_CheckBillFB", list_check);
                    ds = SqlHelper.GetDataSet(sps);
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                }
                BillNos = "";
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    BillNos = BillNos + row["BillNo"].ToString() + "@";
                }

                List<string> list = new List<string>();
                list.Add(BillNos);
                list.Add(DepartureBatch);
                list.Add(WebDate);
                list.Add(DepartTWeb);
                list.Add(LoginWebName);
                list.Add(ProcedureName);
                string dsJson = JsonConvert.SerializeObject(list);
                RequestModel<string> request = new RequestModel<string>();
                request.Request = dsJson;
                request.OperType = 0;
                string json = JsonConvert.SerializeObject(request);
                string url = HttpHelper.urlTimeDepartUptSyn;//获取接口地址
                ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                if (model.State != "200")
                {
                    List<SqlPara> listLog = new List<SqlPara>();
                    listLog.Add(new SqlPara("BillNo", BillNos));
                    listLog.Add(new SqlPara("Batch", DepartureBatch));
                    listLog.Add(new SqlPara("ErrorNode", ProcedureName));
                    listLog.Add(new SqlPara("ExceptMessage", model.Message + "《时效同步》"));
                    SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                    SqlHelper.ExecteNonQuery(spsLog);
                    // MsgBox.ShowOK(model.Message);
                }
                //插入异步同步信息表
                string billnoStrs = "";
                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                {
                    billnoStrs = billnoStrs + ds.Tables[0].Rows[k]["BillNo"].ToString() + ",";
                }
                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_TimeDepartSyn"));
                listAsy.Add(new SqlPara("FaceUrl", url));
                listAsy.Add(new SqlPara("FaceJson", json));
                listAsy.Add(new SqlPara("BillNos", billnoStrs));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "同步配载修改时效"));
                listAsy.Add(new SqlPara("SystemSource", "LMS干线升级版"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));


                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 同步其他修改时效
        /// </summary>
        public static void TimeOtherUptSyn(string BillNos, string DepartureBatch, string MiddleWebName, string AcceptWebName, string SendBatch, string SCBatchNo, string LoginWebName, string ProcedureName, string SendToWeb)
        {
            try
            {
                if (string.IsNullOrEmpty(ProcedureName) || (!string.IsNullOrEmpty(BillNos) && !BillNos.Contains("@")))
                {
                    return;
                }
                List<SqlPara> list_check = new List<SqlPara>();
                DataSet ds = null;
                if (!string.IsNullOrEmpty(BillNos))
                {
                    list_check.Add(new SqlPara("BillNos", BillNos));
                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_CheckBillFB", list_check);
                    ds = SqlHelper.GetDataSet(sps);
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                    return;
                }
                if (!string.IsNullOrEmpty(DepartureBatch))
                {
                    list_check.Add(new SqlPara("DepartureBatch", DepartureBatch));
                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_CheckBillFB", list_check);
                    ds = SqlHelper.GetDataSet(sps);
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                    return;
                }
                if (!string.IsNullOrEmpty(SCBatchNo))
                {
                    list_check.Add(new SqlPara("SCBatchNo", SCBatchNo));
                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_CheckBillFB", list_check);
                    ds = SqlHelper.GetDataSet(sps);
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                    return;
                }
                BillNos = "";
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    BillNos = BillNos + row["BillNo"].ToString() + "@";
                }

                List<string> list = new List<string>();
                list.Add(BillNos);
                list.Add(DepartureBatch);
                list.Add(MiddleWebName);
                list.Add(AcceptWebName);
                list.Add(SendBatch);
                list.Add(SCBatchNo);
                list.Add(LoginWebName);
                list.Add(ProcedureName);
                list.Add(SendToWeb);
                string dsJson = JsonConvert.SerializeObject(list);
                RequestModel<string> request = new RequestModel<string>();
                request.Request = dsJson;
                request.OperType = 0;
                string json = JsonConvert.SerializeObject(request);
                string url = HttpHelper.urlTimeOtherUptSyn;//获取接口地址
                ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                if (model.State != "200")
                {
                    List<SqlPara> listLog = new List<SqlPara>();
                    listLog.Add(new SqlPara("BillNo", BillNos));
                    listLog.Add(new SqlPara("Batch", DepartureBatch));
                    listLog.Add(new SqlPara("ErrorNode", ProcedureName));
                    listLog.Add(new SqlPara("ExceptMessage", model.Message + "《时效同步》"));
                    SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                    SqlHelper.ExecteNonQuery(spsLog);
                    // MsgBox.ShowOK(model.Message);
                }
                //插入异步同步信息表
                string billnoStrs = "";
                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                {
                    billnoStrs = billnoStrs + ds.Tables[0].Rows[k]["BillNo"].ToString() + ",";
                }
                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_TimeOtherSyn"));
                listAsy.Add(new SqlPara("FaceUrl", url));
                listAsy.Add(new SqlPara("FaceJson", json));
                listAsy.Add(new SqlPara("BillNos", billnoStrs));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "同步其他修改时效"));
                listAsy.Add(new SqlPara("SystemSource", "LMS干线升级版"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));


                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 同步送货修改时效
        /// </summary>
        public static void TimeSendUptSyn(string BillNos, string SendWeb, string SendToWeb, string ProcedureName)
        {
            try
            {
                if (string.IsNullOrEmpty(ProcedureName) || string.IsNullOrEmpty(BillNos))
                {
                    return;
                }
                List<SqlPara> list_check = new List<SqlPara>();
                DataSet ds = null;
                if (!string.IsNullOrEmpty(BillNos))
                {
                    list_check.Add(new SqlPara("BillNos", BillNos));
                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_CheckBillFB", list_check);
                    ds = SqlHelper.GetDataSet(sps);
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                    return;
                }
                BillNos = "";
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    BillNos = BillNos + row["BillNo"].ToString() + "@";
                }

                List<string> list = new List<string>();
                list.Add(BillNos);
                list.Add(SendWeb);
                list.Add(SendToWeb);
                list.Add(ProcedureName);
                string dsJson = JsonConvert.SerializeObject(list);
                RequestModel<string> request = new RequestModel<string>();
                request.Request = dsJson;
                request.OperType = 0;
                string json = JsonConvert.SerializeObject(request);
                string url = HttpHelper.urlTimeSendUptSyn;//获取接口地址
                ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                if (model.State != "200")
                {
                    List<SqlPara> listLog = new List<SqlPara>();
                    listLog.Add(new SqlPara("BillNo", BillNos));
                    listLog.Add(new SqlPara("Batch", ""));
                    listLog.Add(new SqlPara("ErrorNode", ProcedureName));
                    listLog.Add(new SqlPara("ExceptMessage", model.Message + "《时效同步》"));
                    SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                    SqlHelper.ExecteNonQuery(spsLog);
                    // MsgBox.ShowOK(model.Message);
                }
                //插入异步同步信息表
                string billnoStrs = "";
                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                {
                    billnoStrs = billnoStrs + ds.Tables[0].Rows[k]["BillNo"].ToString() + ",";
                }
                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_TimeSendSyn"));
                listAsy.Add(new SqlPara("FaceUrl", url));
                listAsy.Add(new SqlPara("FaceJson", json));
                listAsy.Add(new SqlPara("BillNos", billnoStrs));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "同步送货修改时效"));
                listAsy.Add(new SqlPara("SystemSource", "LMS干线升级版"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));


                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 备注同步
        /// </summary>
        /// <param name="billNo"></param>
        public static void NoteSyn(string BillNO, string ModifyRemark)
        {
            //hj20180428同步备注信息
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_WayBill_ModifyRemark", new List<SqlPara> { new SqlPara("BillNO", BillNO), new SqlPara("ModifyRemark", ModifyRemark) }));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
            string dsJson = JsonConvert.SerializeObject(ds);
            RequestModel<string> request = new RequestModel<string>();
            request.Request = dsJson;
            request.OperType = 0;
            string json = JsonConvert.SerializeObject(request);
            string url = HttpHelper.urlNoteSyn;
            ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
            if (model.State != "200")
            {
                List<SqlPara> listLog = new List<SqlPara>();
                listLog.Add(new SqlPara("BillNo", BillNO));
                listLog.Add(new SqlPara("Batch", ""));
                listLog.Add(new SqlPara("ErrorNode", "修改备注"));
                listLog.Add(new SqlPara("ExceptMessage", model.Message));
                SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                SqlHelper.ExecteNonQuery(spsLog);
            }
            //插入异步同步信息表
            string billnoStrs = "";
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                billnoStrs = billnoStrs + ds.Tables[0].Rows[i]["BillNo"].ToString() + ",";
            }
            List<SqlPara> listAsy = new List<SqlPara>();
            listAsy.Add(new SqlPara("FuntionName", "USP_NoteSyn"));
            listAsy.Add(new SqlPara("FaceUrl", url));
            listAsy.Add(new SqlPara("FaceJson", json));
            listAsy.Add(new SqlPara("BillNos", billnoStrs));
            listAsy.Add(new SqlPara("Batch", ""));
            listAsy.Add(new SqlPara("NodeName", "修改备注"));
            listAsy.Add(new SqlPara("SystemSource", "LMS干线升级版"));
            listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));


            SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
            SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
        }

        /// <summary>
        /// 改单审批同步
        /// </summary>
        /// <param name="BillNo"></param>
        public static void ReviewSyn(string BillNo)
        {
            DataSet dsSP = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILLAPPLY_BYBILLNO_SP", new List<SqlPara>() { new SqlPara("BillNo", BillNo) }));
            if (dsSP == null || dsSP.Tables.Count == 0 || dsSP.Tables[0].Rows.Count == 0) return;
            string dsJson = JsonConvert.SerializeObject(dsSP);
            RequestModel<string> request = new RequestModel<string>();
            request.Request = dsJson;
            request.OperType = 0;
            string json = JsonConvert.SerializeObject(request);
            string url = HttpHelper.urlReviewSyn;
            ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
            if (model.State != "200")
            {
                List<SqlPara> listLog = new List<SqlPara>();
                listLog.Add(new SqlPara("BillNo", BillNo));
                listLog.Add(new SqlPara("Batch", ""));
                listLog.Add(new SqlPara("ErrorNode", "改单审批"));
                listLog.Add(new SqlPara("ExceptMessage", model.Message));
                SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                SqlHelper.ExecteNonQuery(spsLog);
            }
            //插入异步同步信息表
            string billnoStrs = "";
            for (int k = 0; k < dsSP.Tables[0].Rows.Count; k++)
            {
                billnoStrs = billnoStrs + dsSP.Tables[0].Rows[k]["BillNO"].ToString() + ",";
            }
            List<SqlPara> listAsy = new List<SqlPara>();
            listAsy.Add(new SqlPara("FuntionName", "USP_ReviewSyn"));
            listAsy.Add(new SqlPara("FaceUrl", url));
            listAsy.Add(new SqlPara("FaceJson", json));
            listAsy.Add(new SqlPara("BillNos", billnoStrs));
            listAsy.Add(new SqlPara("Batch", ""));
            listAsy.Add(new SqlPara("NodeName", "改单审批"));
            listAsy.Add(new SqlPara("SystemSource", "LMS干线升级版"));
            listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));


            SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
            SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
        }


        /// <summary>
        /// 轨迹同步
        /// tracetype 轨迹状态说明
        /// num 轨迹状态标识
        /// opertype 1插入2删除
        /// type 配载 短驳 送货 ....
        /// </summary>
        /// <param name="DepartureBatch"></param>
        /// <param name="billNo"></param>
        /// <param name="tracetype"></param>
        /// <param name="num"></param>
        /// <param name="opertype"></param>
        /// <param name="type"></param>
        public static void TraceSyn(string DepartureBatch, string billNos, int num, string tracetype, int opertype, string type, DataSet ds)
        {
            try
            {
                //取消操作因轨迹信息清除提前查询到轨迹信息传过来
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    if (!string.IsNullOrEmpty(DepartureBatch) || string.IsNullOrEmpty(tracetype))//tracetype等于null，因为中转跟踪结束需要根据MiddleType 判断中转跟踪是中转跟踪还是中转终端跟踪，在存储过程中判断并获取该值
                    {
                        List<SqlPara> lists = new List<SqlPara>();
                        lists.Add(new SqlPara("DepartureBatch", DepartureBatch));
                        lists.Add(new SqlPara("types", type));
                        DataSet dss = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILLVEHICLESTAR_Arrive", lists));
                        if (dss == null || dss.Tables.Count == 0) return;

                        num = Int32.Parse(Convert.ToString(dss.Tables[0].Rows[0]["num"]));
                        if (tracetype.Equals(null)) { tracetype = Convert.ToString(dss.Tables[0].Rows[0]["tracetype"]); };
                        for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                        {
                            billNos += Convert.ToString(dss.Tables[0].Rows[i]["BillNo"]) + "@";
                        }
                    }

                    if (!string.IsNullOrEmpty(billNos) && !string.IsNullOrEmpty(tracetype))
                    {
                        List<SqlPara> list = new List<SqlPara>();
                        list.Add(new SqlPara("DepartureBatch", null));
                        list.Add(new SqlPara("BillNo", billNos));
                        list.Add(new SqlPara("tracetype", tracetype));
                        list.Add(new SqlPara("num", num));
                        SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_TraceSyn", list);
                        ds = SqlHelper.GetDataSet(sps);
                    }
                }
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                if (ds.Tables[0].Rows[0]["opertype"].ToString() == "0") return;//maohui20181227
                ds.Tables[0].Rows[0]["opertype"] = opertype;
                string dsJson = JsonConvert.SerializeObject(ds);
                RequestModel<string> request = new RequestModel<string>();
                request.Request = dsJson;
                request.OperType = 0;
                string json = JsonConvert.SerializeObject(request);
                string url = HttpHelper.urlTraceSyn;
                ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                if (model.State != "200")
                {
                    List<SqlPara> listLog = new List<SqlPara>();
                    listLog.Add(new SqlPara("BillNo", billNos));
                    listLog.Add(new SqlPara("Batch", ""));
                    listLog.Add(new SqlPara("ErrorNode", tracetype + "轨迹同步"));
                    listLog.Add(new SqlPara("ExceptMessage", model.Message));
                    SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                    SqlHelper.ExecteNonQuery(spsLog);
                    // MsgBox.ShowOK(model.Message);
                }
                //插入异步同步信息表
                string billnoStrs = "";
                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                {
                    billnoStrs = billnoStrs + ds.Tables[0].Rows[k]["billno"].ToString() + ",";
                }
                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_Trace_Syn"));
                listAsy.Add(new SqlPara("FaceUrl", url));
                listAsy.Add(new SqlPara("FaceJson", json));
                listAsy.Add(new SqlPara("BillNos", billnoStrs));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", tracetype + "轨迹同步"));
                listAsy.Add(new SqlPara("SystemSource", "LMS干线升级版"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));


                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
                List<SqlPara> listLog = new List<SqlPara>();
                listLog.Add(new SqlPara("BillNo", billNos));
                listLog.Add(new SqlPara("Batch", ""));
                listLog.Add(new SqlPara("ErrorNode", tracetype + "轨迹同步"));
                listLog.Add(new SqlPara("ExceptMessage", ex.Message));
                SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                SqlHelper.ExecteNonQuery(spsLog);
            }
        }

        /// <summary>
        /// yzw 配载同步新
        /// </summary>
        /// <param name="billNoStr"></param>
        /// <param name="departureBatch"></param>
        /// <param name="numStr"></param>
        public static void BillDepartureSynNew(string billNoStr, string departureBatch, string numStr)
        {
            try
            {
                //判断是否有转分拨
                List<SqlPara> listQuery = new List<SqlPara>();
                listQuery.Add(new SqlPara("BillNOStr", billNoStr.Replace(",", "@")));
                SqlParasEntity spsQuery = new SqlParasEntity(OperType.Query, "QSP_GET_DepartureSysInfo_FCD", listQuery);
                DataSet ds = SqlHelper.GetDataSet(spsQuery);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                Dictionary<string, string> dty = new Dictionary<string, string>();
                dty.Add("billNoStr", billNoStr);
                dty.Add("numStr", numStr);
                string json = JsonConvert.SerializeObject(dty);
                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_ADD_BILLDEPARTURE_SYN"));
                listAsy.Add(new SqlPara("FaceUrl", ""));
                listAsy.Add(new SqlPara("FaceJson", json));
                if (!billNoStr.Contains("@") && !billNoStr.Contains(","))
                {
                    billNoStr += ",";
                }
                else if (billNoStr.Contains("@"))
                {
                    billNoStr = billNoStr.Replace("@", ",");
                }
                listAsy.Add(new SqlPara("BillNos", billNoStr));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "配载"));
                listAsy.Add(new SqlPara("SystemSource", "TMS"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));

                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //yzw 配载点击发车同步
        /// <summary>
        /// 发车点到同步
        /// </summary>
        /// <param name="ds"></param>
        public static void VEHICLESTAR_SYN(DataSet ds)
        {
            try
            {
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                string billnos = "", tracetypes = "", optwebnames = "", contents = "", createdates = "", tracedates = "", nums = "";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    billnos += ds.Tables[0].Rows[i]["billno"].ToString() + "@";
                    tracetypes += ds.Tables[0].Rows[i]["tracetype"].ToString() + "@";
                    optwebnames += ds.Tables[0].Rows[i]["optwebname"].ToString() + "@";
                    contents += ds.Tables[0].Rows[i]["content"].ToString() + "@";
                    createdates += ds.Tables[0].Rows[i]["createdate"].ToString() + "@";
                    tracedates += ds.Tables[0].Rows[i]["tracedate"].ToString() + "@";
                    nums += ds.Tables[0].Rows[i]["num"].ToString() + "@";
                }

                Dictionary<string, string> dty = new Dictionary<string, string>();
                dty.Add("billnos", billnos);
                dty.Add("tracetypes", tracetypes);
                dty.Add("optwebnames", optwebnames);
                dty.Add("contents", contents);
                dty.Add("createdates", createdates);
                dty.Add("tracedates", tracedates);
                dty.Add("nums", nums);

                string json = JsonConvert.SerializeObject(dty);

                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_ADD_BILLVEHICLESTAR_Merge_SYN"));
                listAsy.Add(new SqlPara("FaceUrl", ""));
                listAsy.Add(new SqlPara("FaceJson", json));
                listAsy.Add(new SqlPara("BillNos", billnos.Replace("@", ",")));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "配载发车"));
                listAsy.Add(new SqlPara("SystemSource", "TMS"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));

                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        /// <summary>
        /// yzw配载取消发车
        /// </summary>
        /// <param name="DepartureBatch"></param>
        public static void CancelVecheilStart(string DepartureBatch)
        {
            try
            {
                if (DepartureBatch == "") return;
                List<SqlPara> listSYN = new List<SqlPara>();
                listSYN.Add(new SqlPara("DepartureBatch", DepartureBatch));
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_CancelVechelStart_SYN", listSYN));
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                string BillNoStr = "", BillStates = "";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    BillNoStr += ds.Tables[0].Rows[i]["billno"].ToString() + "@";
                    BillStates += ds.Tables[0].Rows[i]["BillState"].ToString() + "@";
                }
                Dictionary<string, string> dty = new Dictionary<string, string>();
                dty.Add("BillNoStr", BillNoStr);
                dty.Add("BillStates", BillStates);

                string json = JsonConvert.SerializeObject(dty);

                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_CancelVecheilStart"));
                listAsy.Add(new SqlPara("FaceUrl", ""));
                listAsy.Add(new SqlPara("FaceJson", json));
                listAsy.Add(new SqlPara("BillNos", BillNoStr.Replace("@", ",")));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "配载取消发车"));
                listAsy.Add(new SqlPara("SystemSource", "TMS"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));

                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        /// <summary>
        /// yzw 配载整车作废
        /// </summary>
        /// <param name="BillNoStr"></param>
        public static void CancelVecheil(string BillNoStr)
        {
            List<SqlPara> listSYN = new List<SqlPara>();
            listSYN.Add(new SqlPara("BillNoStr", BillNoStr));
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_ISFB_SYN", listSYN));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
            Dictionary<string, string> dty = new Dictionary<string, string>();
            dty.Add("BillNoStr", BillNoStr);
            string json = JsonConvert.SerializeObject(dty);

            List<SqlPara> listAsy = new List<SqlPara>();
            listAsy.Add(new SqlPara("FuntionName", "USP_CANCELVECHEL"));
            listAsy.Add(new SqlPara("FaceUrl", ""));
            listAsy.Add(new SqlPara("FaceJson", json));
            listAsy.Add(new SqlPara("BillNos", BillNoStr.Replace("@", ",")));
            listAsy.Add(new SqlPara("Batch", ""));
            listAsy.Add(new SqlPara("NodeName", "配载整车作废"));
            listAsy.Add(new SqlPara("SystemSource", "TMS"));
            listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));

            SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
            SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
        }

        //yzw 配载到车同步(富诚达不存在短驳,转二级到车)
        /// <summary>
        ///yzw 配载到车同步(富诚达不存在短驳,转二级到车)
        /// </summary>
        /// <param name="type">1为接收,2为取消到车</param>
        /// <param name="DepartureBatch"></param>
        /// <param name="arriveSite"></param>
        /// <param name="arriveWeb"></param>
        /// <param name="types">类型:配载</param>
        public static void VEHICLESTAR_Arrive_SYN(string type, string DepartureBatch, string arriveSite, string arriveWeb, string types)
        {
            try
            {
                if (DepartureBatch == "") return;
                List<SqlPara> listSYN = new List<SqlPara>();
                listSYN.Add(new SqlPara("DepartureBatch", DepartureBatch));
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_VEHICLESTAR_Arrive_SYN", listSYN));
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;



                Dictionary<string, string> dty = new Dictionary<string, string>();
                dty.Add("type", type);
                dty.Add("DepartureBatch", DepartureBatch);
                dty.Add("BillNoStr", ds.Tables[0].Rows[0]["BillNoStr"].ToString());
                dty.Add("arriveSite", arriveSite);
                dty.Add("arriveWeb", arriveWeb);
                dty.Add("types", types);
                string json = JsonConvert.SerializeObject(dty);


                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "Upd_BILLVEHICLESTAR_Arrive_SYN"));
                listAsy.Add(new SqlPara("FaceUrl", ""));
                listAsy.Add(new SqlPara("FaceJson", json));
                listAsy.Add(new SqlPara("BillNos", ds.Tables[0].Rows[0]["BillNoStr"].ToString().Replace("@", ",")));
                listAsy.Add(new SqlPara("Batch", DepartureBatch));
                if (type == "1")
                {
                    listAsy.Add(new SqlPara("NodeName", "车辆点到"));
                }
                else
                {
                    listAsy.Add(new SqlPara("NodeName", "取消车辆点到"));
                }
                listAsy.Add(new SqlPara("SystemSource", "TMS"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));

                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }
        //yzw 配载到货确认同步
        /// <summary>
        /// 配载到货确认同步
        /// </summary> yzw
        /// <param name="type"></param>
        /// <param name="DepartureBatch"></param>
        /// <param name="arriveSite"></param>
        /// <param name="arriveWeb"></param>
        /// <param name="types"></param>
        public static void BILLDEPARTURE_ARRIVED_OK_SYN(string DepartureBatch)
        {
            try
            {
                List<SqlPara> listSYN = new List<SqlPara>();
                listSYN.Add(new SqlPara("DepartureBatch", DepartureBatch));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURE_ARRIVED_OK_SYN", listSYN);
                DataSet ds = SqlHelper.GetDataSet(spe);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                Dictionary<string, string> dty = new Dictionary<string, string>();
                dty.Add("DepartureBatch", DepartureBatch);
                dty.Add("BillNoStr", ds.Tables[0].Rows[0]["BillNoStr"].ToString());//@
                dty.Add("LoginWebName", CommonClass.UserInfo.WebName);
                dty.Add("LoginSiteName", CommonClass.UserInfo.SiteName);
                string json = JsonConvert.SerializeObject(dty);

                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_SET_BILLDEPARTURE_ARRIVED_OK_SYN"));
                listAsy.Add(new SqlPara("FaceUrl", ""));
                listAsy.Add(new SqlPara("FaceJson", json));
                listAsy.Add(new SqlPara("BillNos", ds.Tables[0].Rows[0]["BillNoStr"].ToString().Replace("@", ",")));
                listAsy.Add(new SqlPara("Batch", DepartureBatch));
                listAsy.Add(new SqlPara("NodeName", "配载到货"));
                listAsy.Add(new SqlPara("SystemSource", "TMS"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));

                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        /// <summary>
        /// yzw配载取消到货同步
        /// </summary>
        /// <param name="DepartureBatch"></param>
        public static void BILLDEPARTURE_ARRIVED_CANCEL_SYN(string DepartureBatch)
        {
            try
            {
                List<SqlPara> listSYN = new List<SqlPara>();
                listSYN.Add(new SqlPara("DepartureBatch", DepartureBatch));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURE_ARRIVED_CANCEL_SYN", listSYN);
                DataSet ds = SqlHelper.GetDataSet(spe);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                Dictionary<string, string> dty = new Dictionary<string, string>();
                dty.Add("DepartureBatch", DepartureBatch);
                dty.Add("BillNoStr", ds.Tables[0].Rows[0]["BillNoStr"].ToString());//@
                dty.Add("LoginWebName", CommonClass.UserInfo.WebName);

                string json = JsonConvert.SerializeObject(dty);

                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_SET_BILLDEPARTURE_ARRIVED_CANCEL_SYN"));
                listAsy.Add(new SqlPara("FaceUrl", ""));
                listAsy.Add(new SqlPara("FaceJson", json));
                listAsy.Add(new SqlPara("BillNos", ds.Tables[0].Rows[0]["BillNoStr"].ToString().Replace("@", ",")));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "配载取消到货"));
                listAsy.Add(new SqlPara("SystemSource", "TMS"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));

                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        /// <summary>
        /// yzw中转同步
        /// </summary>
        /// <param name="BillNos"></param>
        /// <param name="MiddleBillnos"></param>
        public static void MIDDLE_NEW_SYN(string BillNos, string MiddleBillnos, string MiddleType)
        {
            try
            {
                List<SqlPara> listSYN = new List<SqlPara>();
                listSYN.Add(new SqlPara("BillNos", BillNos));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_MIDDLE_NEW_SYN", listSYN);
                DataSet ds = SqlHelper.GetDataSet(spe);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
               string MiddleDatestr = string.Empty,
                      MiddleStartSitePhonestr = string.Empty,
                      AccMiddlePaystr = string.Empty,
                      MiddleRemarkstr = string.Empty,
                      MiddleCarrierstr = string.Empty,
                      MiddleEndSitePhonestr = string.Empty,
                      MiddleOperatorstr = string.Empty,
                      MiddleBatchstr = string.Empty,
                      MiddleFetchAddressstr = string.Empty;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    MiddleDatestr += ds.Tables[0].Rows[i]["MiddleDate"].ToString() + "@";
                    MiddleStartSitePhonestr += ds.Tables[0].Rows[i]["MiddleStartSitePhone"].ToString() + "@";
                    AccMiddlePaystr += ds.Tables[0].Rows[i]["AccMiddlePay"].ToString() + "@";
                    MiddleRemarkstr += ds.Tables[0].Rows[i]["MiddleRemark"].ToString() + "@";
                    MiddleCarrierstr += ds.Tables[0].Rows[i]["MiddleCarrier"].ToString() + "@";
                    MiddleEndSitePhonestr += ds.Tables[0].Rows[i]["MiddleEndSitePhone"].ToString() + "@";
                    MiddleOperatorstr += ds.Tables[0].Rows[i]["MiddleOperator"].ToString() + "@";
                    MiddleBatchstr += ds.Tables[0].Rows[i]["MiddleBatch"].ToString() + "@";
                    MiddleFetchAddressstr += ds.Tables[0].Rows[i]["MiddleFetchAddress"].ToString() + "@";
                }


                Dictionary<string, string> dty = new Dictionary<string, string>();
                dty.Add("BillNos", BillNos);
                dty.Add("MiddleBillnos", MiddleBillnos);
                dty.Add("MiddleType", MiddleType);
                dty.Add("LoginWebName", CommonClass.UserInfo.WebName);
                dty.Add("MiddleDate", MiddleDatestr);                           //chenxiang,2019-03-05
                dty.Add("MiddleStartSitePhone", MiddleStartSitePhonestr);
                dty.Add("AccMiddlePay", AccMiddlePaystr);
                dty.Add("MiddleRemark", MiddleRemarkstr);
                dty.Add("MiddleCarrier", MiddleCarrierstr);
                dty.Add("MiddleEndSitePhone", MiddleEndSitePhonestr);
                dty.Add("MiddleOperator", MiddleOperatorstr);
                dty.Add("MiddleBatch", MiddleBatchstr);
                dty.Add("MiddleFetchAddress", MiddleFetchAddressstr);

                string json = JsonConvert.SerializeObject(dty);
                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_ADD_MIDDLE_NEW_SYN"));
                listAsy.Add(new SqlPara("FaceUrl", ""));
                listAsy.Add(new SqlPara("FaceJson", json));
                if (!BillNos.Contains("@") && !BillNos.Contains(","))
                {
                    BillNos += ",";
                }
                else if (BillNos.Contains("@"))
                {
                    BillNos = BillNos.Replace("@", ",");
                }
                listAsy.Add(new SqlPara("BillNos", BillNos));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "中转"));
                listAsy.Add(new SqlPara("SystemSource", "TMS"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));
                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        /// <summary>
        /// yzw 取消中转
        /// </summary>
        /// <param name="BillNo"></param>
        public static void DELETE_MIDDLE_RECORD_NEW_SYN(string BillNo)
        {
            try
            {
                List<SqlPara> listSYN = new List<SqlPara>();
                listSYN.Add(new SqlPara("BillNo", BillNo));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_MiddleCancel_SYN", listSYN);
                DataSet ds = SqlHelper.GetDataSet(spe);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                Dictionary<string, string> dty = new Dictionary<string, string>();
                dty.Add("BillNo", BillNo);
                string json = JsonConvert.SerializeObject(dty);

                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_DELETE_MIDDLE_RECORD_NEW_SYN"));
                listAsy.Add(new SqlPara("FaceUrl", ""));
                listAsy.Add(new SqlPara("FaceJson", json));
                if (!BillNo.Contains("@") && !BillNo.Contains(","))
                {
                    BillNo += ",";
                }
                else if (BillNo.Contains("@"))
                {
                    BillNo = BillNo.Replace("@", ",");
                }
                listAsy.Add(new SqlPara("BillNos", BillNo));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "取消中转"));
                listAsy.Add(new SqlPara("SystemSource", "TMS"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));
                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }

            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }


        }

        /// <summary>
        /// yzw 自提签收同步
        /// </summary>
        /// <param name="BillNos"></param>
        /// <param name="MiddleBillnos"></param>
        /// <param name="MiddleType"></param>
        public static void BILLSIGN_MIDDLESign_SYN(string BillNoStr, string SignMan, string AgentMan)
        {
            try
            {
                List<SqlPara> listQuery = new List<SqlPara>();
                listQuery.Add(new SqlPara("BillNoStr", BillNoStr));
                SqlParasEntity spsQuery = new SqlParasEntity(OperType.Query, "QSP_BILLSIGN_FETCH_SYN", listQuery);
                DataSet ds = SqlHelper.GetDataSet(spsQuery);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                Dictionary<string, string> dty = new Dictionary<string, string>();
                dty.Add("BillNoStr", BillNoStr);
                dty.Add("SignMan", SignMan);
                dty.Add("AgentMan", AgentMan);
                dty.Add("LoginWebName", CommonClass.UserInfo.WebName);
                string json = JsonConvert.SerializeObject(dty);

                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_ADD_BILLSIGN_SYN"));
                listAsy.Add(new SqlPara("FaceUrl", ""));
                listAsy.Add(new SqlPara("FaceJson", json));
                if (!BillNoStr.Contains("@") && !BillNoStr.Contains(","))
                {
                    BillNoStr += ",";
                }
                else if (BillNoStr.Contains("@"))
                {
                    BillNoStr = BillNoStr.Replace("@", ",");
                }
                listAsy.Add(new SqlPara("BillNos", BillNoStr));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "自提签收"));
                listAsy.Add(new SqlPara("SystemSource", "TMS"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));
                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        /// <summary>
        /// yzw 中转,送货签收
        /// </summary>
        /// <param name="BillNo"></param>
        /// <param name="SignMan"></param>
        public static void BILLSIGN_MIDDLE_SYN(string BillNo, string SignMan, string type)
        {
            try
            {
                List<SqlPara> listQuery = new List<SqlPara>();
                listQuery.Add(new SqlPara("BillNo", BillNo));
                SqlParasEntity spsQuery = new SqlParasEntity(OperType.Query, "QSP_BILLSIGN_MIDDLE_SYN", listQuery);
                DataSet ds = SqlHelper.GetDataSet(spsQuery);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                Dictionary<string, string> dty = new Dictionary<string, string>();
                dty.Add("BillNoStr", BillNo);
                dty.Add("SignMan", SignMan);
                dty.Add("LoginWebName", CommonClass.UserInfo.WebName);
                string json = JsonConvert.SerializeObject(dty);

                List<SqlPara> listAsy = new List<SqlPara>();
                if (type == "中转签收")
                {
                    listAsy.Add(new SqlPara("FuntionName", "USP_ADD_BILLSIGN_MIDDLE_SYN"));
                }
                else//送货签收
                {
                    listAsy.Add(new SqlPara("FuntionName", "USP_ADD_BILLSIGN_SEND_SYN"));
                }

                listAsy.Add(new SqlPara("FaceUrl", ""));
                listAsy.Add(new SqlPara("FaceJson", json));
                if (!BillNo.Contains("@") && !BillNo.Contains(","))
                {
                    BillNo += ",";
                }
                else if (BillNo.Contains("@"))
                {
                    BillNo = BillNo.Replace("@", ",");
                }
                listAsy.Add(new SqlPara("BillNos", BillNo));
                listAsy.Add(new SqlPara("Batch", ""));
                if (type == "中转签收")
                {
                    listAsy.Add(new SqlPara("NodeName", "中转签收"));
                }
                else
                {
                    listAsy.Add(new SqlPara("NodeName", "送货签收"));
                }

                listAsy.Add(new SqlPara("SystemSource", "TMS"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));
                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        /// <summary>
        /// yzw 异常签收
        /// </summary>
        /// <param name="BillNo"></param>
        /// <param name="SignMan"></param>
        /// <param name="SignSite"></param>
        /// <param name="SignWeb"></param>
        public static void BILLERRORSIGN_SYN(string BillNo, string SignMan, string SignSite, string SignWeb)
        {
            try
            {
                List<SqlPara> listQuery = new List<SqlPara>();
                listQuery.Add(new SqlPara("BillNo", BillNo));
                SqlParasEntity spsQuery = new SqlParasEntity(OperType.Query, "QSP_BILLERRORSIGN_SYN", listQuery);
                DataSet ds = SqlHelper.GetDataSet(spsQuery);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                Dictionary<string, string> dty = new Dictionary<string, string>();
                dty.Add("BillNo", BillNo);
                dty.Add("SignMan", SignMan);
                dty.Add("SignSite", SignSite);
                dty.Add("SignWeb", SignWeb);
                string json = JsonConvert.SerializeObject(dty);

                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_ADD_BILLERRORSIGN_SYN"));
                listAsy.Add(new SqlPara("FaceUrl", ""));
                listAsy.Add(new SqlPara("FaceJson", json));
                if (!BillNo.Contains("@") && !BillNo.Contains(","))
                {
                    BillNo += ",";
                }
                else if (BillNo.Contains("@"))
                {
                    BillNo = BillNo.Replace("@", ",");
                }
                listAsy.Add(new SqlPara("BillNos", BillNo));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "异常签收"));
                listAsy.Add(new SqlPara("SystemSource", "TMS"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));
                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        /// <summary>
        /// yzw 取消签收
        /// </summary>
        /// <param name="BillNo"></param>
        /// <param name="SignType"></param>
        public static void DELETE_BILLSIGN_SYN(string BillNo, string SignType)
        {
            try
            {
                List<SqlPara> listQuery = new List<SqlPara>();
                listQuery.Add(new SqlPara("BillNo", BillNo));
                SqlParasEntity spsQuery = new SqlParasEntity(OperType.Query, "QSP_DELETE_BILLSIGN_SYN", listQuery);
                DataSet ds = SqlHelper.GetDataSet(spsQuery);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                Dictionary<string, string> dty = new Dictionary<string, string>();
                dty.Add("BillNo", BillNo);
                dty.Add("BillState", ds.Tables[0].Rows[0]["BillState"].ToString());
                dty.Add("SignType", SignType);
                string json = JsonConvert.SerializeObject(dty);

                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_DELETE_BILLSIGN_SYN"));
                listAsy.Add(new SqlPara("FaceUrl", ""));
                listAsy.Add(new SqlPara("FaceJson", json));
                if (!BillNo.Contains("@") && !BillNo.Contains(","))
                {
                    BillNo += ",";
                }
                else if (BillNo.Contains("@"))
                {
                    BillNo = BillNo.Replace("@", ",");
                }
                listAsy.Add(new SqlPara("BillNos", BillNo));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", SignType + "取消签收"));
                listAsy.Add(new SqlPara("SystemSource", "TMS"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));
                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// yzw 送货上门同步
        /// </summary>
        /// <param name="BillNo"></param>
        /// <param name="SignType"></param>
        public static void USP_ADD_SEND_3_SYN(string billnos, string SendCarNo, DateTime SendDate)
        {
            try
            {
                List<SqlPara> listQuery = new List<SqlPara>();
                listQuery.Add(new SqlPara("billnos", billnos));
                SqlParasEntity spsQuery = new SqlParasEntity(OperType.Query, "USP_ADD_SEND_3_SYN_New", listQuery);
                DataSet ds = SqlHelper.GetDataSet(spsQuery);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                Dictionary<string, string> dty = new Dictionary<string, string>();
                dty.Add("billnos", billnos);
                dty.Add("SendCarNo", SendCarNo);
                dty.Add("SendDate", SendDate.ToString("yyyy-MM-dd HH:mm:ss"));
                dty.Add("LoginWebName", CommonClass.UserInfo.WebName);
                string json = JsonConvert.SerializeObject(dty);

                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_ADD_SEND_3_SYN"));
                listAsy.Add(new SqlPara("FaceUrl", ""));
                listAsy.Add(new SqlPara("FaceJson", json));
                if (!billnos.Contains("@") && !billnos.Contains(","))
                {
                    billnos += ",";
                }
                else if (billnos.Contains("@"))
                {
                    billnos = billnos.Replace("@", ",");
                }
                listAsy.Add(new SqlPara("BillNos", billnos));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "送货上门"));
                listAsy.Add(new SqlPara("SystemSource", "TMS"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));
                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        /// <summary>
        /// yzw 取消转二级,送货
        /// </summary>
        /// <param name="Billno"></param>
        /// <param name="BillState"></param>
        public static void DELETE_SEND_SYN(string Billno)
        {
            try
            {
               List<SqlPara> listQuery = new List<SqlPara>();
                listQuery.Add(new SqlPara("Billno", Billno));
                SqlParasEntity spsQuery = new SqlParasEntity(OperType.Query, "USP_DELETE_SEND_SYN_New", listQuery);
                DataSet ds = SqlHelper.GetDataSet(spsQuery);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                Dictionary<string, string> dty = new Dictionary<string, string>();
                dty.Add("Billno", Billno);
                dty.Add("BillState", ds.Tables[0].Rows[0]["BillState"].ToString());
                string json = JsonConvert.SerializeObject(dty);

                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_DELETE_SEND_SYN"));
                listAsy.Add(new SqlPara("FaceUrl", ""));
                listAsy.Add(new SqlPara("FaceJson", json));
                if (!Billno.Contains("@") && !Billno.Contains(","))
                {
                    Billno += ",";
                }
                else if (Billno.Contains("@"))
                {
                    Billno = Billno.Replace("@", ",");
                }
                listAsy.Add(new SqlPara("BillNos", Billno));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "取消送货上门"));
                listAsy.Add(new SqlPara("SystemSource", "TMS"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));
                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        //yzw 转二级同步
        /// <summary>
        /// 转二级同步
        /// </summary>
        /// <param name="SendToWeb"></param>
        /// <param name="SendDate"></param>
        /// <param name="billnos"></param>
        public static void SEND_TOSITE_SYN(string SendToWeb, DateTime SendDate, string billnos)
        {
            try
            {
                List<SqlPara> listQuery = new List<SqlPara>();
                listQuery.Add(new SqlPara("billnos", billnos));
                SqlParasEntity spsQuery = new SqlParasEntity(OperType.Query, "USP_SEND_TOSITE_SYN", listQuery);
                DataSet ds = SqlHelper.GetDataSet(spsQuery);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                Dictionary<string, string> dty = new Dictionary<string, string>();
                dty.Add("SendToWeb", SendToWeb);
                dty.Add("SendDate", SendDate.ToString("yyyy-MM-dd HH:mm:ss"));
                dty.Add("billnos", billnos);
                dty.Add("LoginWebName", CommonClass.UserInfo.WebName);
                string json = JsonConvert.SerializeObject(dty);

                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_ADD_SEND_TOSITE_SYN"));
                listAsy.Add(new SqlPara("FaceUrl", ""));
                listAsy.Add(new SqlPara("FaceJson", json));
                if (!billnos.Contains("@") && !billnos.Contains(","))
                {
                    billnos += ",";
                }
                else if (billnos.Contains("@"))
                {
                    billnos = billnos.Replace("@", ",");
                }
                listAsy.Add(new SqlPara("BillNos", billnos));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "转二级"));
                listAsy.Add(new SqlPara("SystemSource", "TMS"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));
                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// yzw转二级到货确认同步新  
        /// </summary>
        /// <param name="SendBatch"></param>
        public static void SendToSiteSynNEW(string SendBatch)
        {
            try
            {
                List<SqlPara> listQuery = new List<SqlPara>();
                listQuery.Add(new SqlPara("SendBatch", SendBatch));
                SqlParasEntity spsQuery = new SqlParasEntity(OperType.Query, "QSP_GET_SendToSiteSyn_SYN", listQuery);
                DataSet dsQuery = SqlHelper.GetDataSet(spsQuery);
                if (dsQuery == null || dsQuery.Tables.Count == 0 || dsQuery.Tables[0].Rows.Count == 0) return;

                Dictionary<string, string> dty = new Dictionary<string, string>();
                dty.Add("SendBatch", SendBatch);
                dty.Add("BillNoStr", dsQuery.Tables[0].Rows[0]["BillNoStr"].ToString());//@
                string json = JsonConvert.SerializeObject(dty);

                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_SET_SENDSHORTCONN_CONFIRM_SYN"));
                listAsy.Add(new SqlPara("FaceUrl", ""));
                listAsy.Add(new SqlPara("FaceJson", json));
                listAsy.Add(new SqlPara("BillNos", dsQuery.Tables[0].Rows[0]["BillNoStr"].ToString().Replace("@", ",")));
                listAsy.Add(new SqlPara("Batch", SendBatch));
                listAsy.Add(new SqlPara("NodeName", "转二级到货确认"));
                listAsy.Add(new SqlPara("SystemSource", "TMS"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));
                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }

        /// <summary>
        /// //yzw取消转二级到货   
        /// </summary>
        /// <param name="sendBatch"></param>
        public static void SendToSiteConfirmCancelSynNew(string sendBatch)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SendBatch", sendBatch));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SendToSiteConfirmCancel_SYN", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                Dictionary<String, String> dty = new Dictionary<string, string>();
                dty.Add("SendBatch", sendBatch);
                dty.Add("BillNoStr", ds.Tables[0].Rows[0]["BillNoStr"].ToString());
                string json = JsonConvert.SerializeObject(dty);
                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_SET_SENDSHORTCONN_CANCEL_SYN"));
                listAsy.Add(new SqlPara("FaceUrl", ""));
                listAsy.Add(new SqlPara("FaceJson", json));
                listAsy.Add(new SqlPara("BillNos", ds.Tables[0].Rows[0]["BillNoStr"].ToString().Replace("@", ",")));
                listAsy.Add(new SqlPara("Batch", sendBatch));
                listAsy.Add(new SqlPara("NodeName", "取消转二级到货"));
                listAsy.Add(new SqlPara("SystemSource", "TMS"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));


                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //yzw 改单执行同步
        public static void BillApplyExcute(string ApplyID)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ApplyID", ApplyID));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BillApplyInfoToZQTMS_SYN", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                Dictionary<string, string> dty = new Dictionary<string, string>();
                dty.Add("ApplyID", ds.Tables[0].Rows[0]["ApplyID"].ToString());
                dty.Add("ExcuteState", ds.Tables[0].Rows[0]["ExcuteState"].ToString());
                dty.Add("ExcuteMan", ds.Tables[0].Rows[0]["ExcuteMan"].ToString());
                dty.Add("ExcuteDate", ds.Tables[0].Rows[0]["ExcuteDate"].ToString());
                dty.Add("LastState", ds.Tables[0].Rows[0]["LastState"].ToString());
                dty.Add("ExcuteRemark", ds.Tables[0].Rows[0]["ExcuteRemark"].ToString());
                string json = JsonConvert.SerializeObject(dty);
                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_BillApplyExcute_SYN"));
                listAsy.Add(new SqlPara("FaceUrl", ""));
                listAsy.Add(new SqlPara("FaceJson", json));
                listAsy.Add(new SqlPara("BillNos", ds.Tables[0].Rows[0]["BillNO"].ToString() + ","));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "改单执行"));
                listAsy.Add(new SqlPara("SystemSource", "TMS"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));


                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// yzw 拨入接收同步
        /// </summary>
        /// <param name="billnos"></param>
        public static void BillDepartureFBAcceptSynNew(string BillNoStr, string Batch)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNoStr", BillNoStr));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BillToFB_SYN", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                Dictionary<string, string> dty = new Dictionary<string, string>();
                dty.Add("BillNoStr", BillNoStr);
                dty.Add("Batch", Batch);
                dty.Add("LoginUserName", CommonClass.UserInfo.UserName);
                dty.Add("LoginWebName", ds.Tables[0].Rows[0]["LoginWebName"].ToString());

                string json = JsonConvert.SerializeObject(dty);
                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_ZQTMSBLJS"));
                listAsy.Add(new SqlPara("FaceUrl", ""));
                listAsy.Add(new SqlPara("FaceJson", json));
                if (!BillNoStr.Contains("@") && !BillNoStr.Contains(","))
                {
                    BillNoStr += ",";
                }
                else if (BillNoStr.Contains("@"))
                {
                    BillNoStr = BillNoStr.Replace("@", ",");
                }
                listAsy.Add(new SqlPara("BillNos", BillNoStr));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "拨入接收"));
                listAsy.Add(new SqlPara("SystemSource", "TMS"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));


                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        /// <summary>
        /// yzw 取消拨入接收同步
        /// </summary>
        /// <param name="BillNoStr"></param>
        public static void QXBillDepartureFBAcceptSynNew(string BillNoStr, string Batch)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNoStr", BillNoStr));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BillQXFB_SYN", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                Dictionary<string, string> dty = new Dictionary<string, string>();
                dty.Add("BillNoStr", BillNoStr);
                dty.Add("Batch", Batch);
                string json = JsonConvert.SerializeObject(dty);
                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_ZQTMS_QXFB_SYN"));
                listAsy.Add(new SqlPara("FaceUrl", ""));
                listAsy.Add(new SqlPara("FaceJson", json));
                if (!BillNoStr.Contains("@") && !BillNoStr.Contains(","))
                {
                    BillNoStr += ",";
                }
                else if (BillNoStr.Contains("@"))
                {
                    BillNoStr = BillNoStr.Replace("@", ",");
                }
                listAsy.Add(new SqlPara("BillNos", BillNoStr));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "取消拨入接收"));
                listAsy.Add(new SqlPara("SystemSource", "TMS"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));
                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        /// <summary>
        /// yzw 费用异动执行,否决同步
        /// </summary>
        /// <param name="ds"></param>
        public static void BILLAPPLYEXSYN(DataSet ds, string type)
        {
            try
            {
                Dictionary<string, string> dty = new Dictionary<string, string>();
                if (type == "执行")
                {
                    dty.Add("Type", "执行");
                    dty.Add("ApplyID", ds.Tables[0].Rows[0]["ApplyID"].ToString());
                    dty.Add("ExcuteState", ds.Tables[0].Rows[0]["ExcuteState"].ToString());
                    dty.Add("ExcuteMan", ds.Tables[0].Rows[0]["ExcuteMan"].ToString());
                    dty.Add("ExcuteDate", ds.Tables[0].Rows[0]["ExcuteDate"].ToString());
                    dty.Add("LastState", ds.Tables[0].Rows[0]["LastState"].ToString());
                    dty.Add("ExcuteRemark", ds.Tables[0].Rows[0]["ExcuteRemark"].ToString());
                }
                else if (type == "否决")
                {
                    dty.Add("Type", "否决");
                    dty.Add("ApplyID", ds.Tables[0].Rows[0]["ApplyID"].ToString());
                    dty.Add("VetoMan", ds.Tables[0].Rows[0]["VetoMan"].ToString());
                    dty.Add("VetoDate", ds.Tables[0].Rows[0]["VetoDate"].ToString());
                    dty.Add("LastState", ds.Tables[0].Rows[0]["LastState"].ToString());
                    dty.Add("VetoRemark", ds.Tables[0].Rows[0]["VetoRemark"].ToString());
                }
                string json = JsonConvert.SerializeObject(dty);
                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_EXCuteBillApply_SYN"));
                listAsy.Add(new SqlPara("FaceUrl", ""));
                listAsy.Add(new SqlPara("FaceJson", json));
                listAsy.Add(new SqlPara("BillNos", ds.Tables[0].Rows[0]["BillNO"].ToString() + ","));
                listAsy.Add(new SqlPara("Batch", ""));

                if (type == "执行")
                {
                    listAsy.Add(new SqlPara("NodeName", "执行费用异动"));
                }
                else if (type == "否决")
                {
                    listAsy.Add(new SqlPara("NodeName", "否决费用异动"));
                }
                listAsy.Add(new SqlPara("SystemSource", "TMS"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));
                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 回单，签单上传
        /// </summary>
        public static void ReceiptUploadSyn(int billType, string billNos, string filePaths, string userName)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNos", billNos));
                list.Add(new SqlPara("BillType", billType));
                list.Add(new SqlPara("FilePaths", filePaths));
                list.Add(new SqlPara("UpFileMan", userName));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ReceiptSyn_lms", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                string dsJson = JsonConvert.SerializeObject(ds);
                RequestModel<string> request = new RequestModel<string>();
                request.Request = dsJson;
                request.OperType = 0;
                string json = JsonConvert.SerializeObject(request);
                string url = HttpHelper.urlReceiptUploadSyn;
                ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                if (model.State != "200")
                {
                    List<SqlPara> listLog = new List<SqlPara>();
                    listLog.Add(new SqlPara("BillNo", billNos));
                    listLog.Add(new SqlPara("Batch", ""));
                    listLog.Add(new SqlPara("ErrorNode", "回单，签单上传同步"));
                    listLog.Add(new SqlPara("ExceptMessage", model.Message));
                    SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                    SqlHelper.ExecteNonQuery(spsLog);
                    // MsgBox.ShowOK(model.Message);
                }
                //插入异步同步信息表
                string billnoStrs = "";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    billnoStrs = billnoStrs + ds.Tables[0].Rows[i]["BillNo"].ToString() + ",";
                }
                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_ReceiptUpload_Syn_ZQTMS"));
                listAsy.Add(new SqlPara("FaceUrl", url));
                listAsy.Add(new SqlPara("FaceJson", json));
                listAsy.Add(new SqlPara("BillNos", billnoStrs));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "回单签单上传"));
                listAsy.Add(new SqlPara("SystemSource", "LMS干线升级版"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));


                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                List<SqlPara> listLog = new List<SqlPara>();
                listLog.Add(new SqlPara("BillNo", billNos));
                listLog.Add(new SqlPara("Batch", ""));
                listLog.Add(new SqlPara("ErrorNode", "回单，签单上传同步"));
                listLog.Add(new SqlPara("ExceptMessage", ex.Message));
                SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                SqlHelper.ExecteNonQuery(spsLog);
                // MsgBox.ShowOK(model.Message);
            }
        }


        /// <summary>
        /// 回单问题件同步
        /// </summary>
        /// <param name="BillNo"></param>
        public static void HDProblemPartsSyn(string BillNo, string type, int operType)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("BillNo", typeof(string)));
                dt.Columns.Add(new DataColumn("type", typeof(string)));
                dt.Columns.Add(new DataColumn("operType", typeof(int)));
                dt.Columns.Add(new DataColumn("WebName", typeof(string)));
                dt.Columns.Add(new DataColumn("UserName", typeof(string)));


                DataRow dr = dt.NewRow();
                dr["BillNo"] = BillNo;
                dr["type"] = type;
                dr["operType"] = operType;
                dr["WebName"] = CommonClass.UserInfo.WebName;
                dr["UserName"] = CommonClass.UserInfo.UserName;
                dt.Rows.Add(dr);
                ds.Tables.Add(dt);
                string dsJson = JsonConvert.SerializeObject(ds);
                RequestModel<string> request = new RequestModel<string>();
                request.Request = dsJson;
                request.OperType = 0;
                string json = JsonConvert.SerializeObject(request);
                string url = HttpHelper.urlHDProblemPartsSyn;
                ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                if (model.State != "200")
                {
                    List<SqlPara> listLog = new List<SqlPara>();
                    listLog.Add(new SqlPara("BillNo", BillNo));
                    listLog.Add(new SqlPara("Batch", ""));
                    listLog.Add(new SqlPara("ErrorNode", "拨入接收问题件同步"));
                    listLog.Add(new SqlPara("ExceptMessage", model.Message));
                    SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                    SqlHelper.ExecteNonQuery(spsLog);
                }
                //插入异步同步信息表
                string billnoStrs = "";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    billnoStrs = billnoStrs + ds.Tables[0].Rows[i]["BillNo"].ToString() + ",";
                }
                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_ReturnStock_BILLNO_Syn"));
                listAsy.Add(new SqlPara("FaceUrl", url));
                listAsy.Add(new SqlPara("FaceJson", json));
                listAsy.Add(new SqlPara("BillNos", billnoStrs));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "拨入接收问题件同步"));
                listAsy.Add(new SqlPara("SystemSource", "LMS干线升级版"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));


                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);

            }
            catch (Exception ex)
            {
                List<SqlPara> listLog = new List<SqlPara>();
                listLog.Add(new SqlPara("BillNo", BillNo));
                listLog.Add(new SqlPara("Batch", ""));
                listLog.Add(new SqlPara("ErrorNode", "拨入接收问题件同步"));
                listLog.Add(new SqlPara("ExceptMessage", ex.Message));
                SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                SqlHelper.ExecteNonQuery(spsLog);
            }
        }



        /// <summary>
        /// 回单取消同步
        /// </summary>
        public static void ReceiptCancelSyn(Dictionary<string, string> dic)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", dic["BillNo"].ToString()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ReceiptCancel_Syn_LMS", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count < 0) return;
                string BillNo = ds.Tables[0].Rows[0]["BillNo"].ToString();
                if (BillNo == "") return;

                dic["BillNo"] = BillNo;
                //SqlParasEntity sps=new SqlParasEntity(OperType.Query,"",list);
                string listJson = JsonConvert.SerializeObject(dic);
                RequestModel<string> request = new RequestModel<string>();
                request.Request = listJson;
                request.OperType = 0;
                string json = JsonConvert.SerializeObject(request);
                string url = HttpHelper.urlReceiptCancelSyn;
                ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                if (model.State != "200")
                {
                    List<SqlPara> listLog = new List<SqlPara>();
                    listLog.Add(new SqlPara("BillNo", dic["BillNo"].ToString()));
                    listLog.Add(new SqlPara("Batch", ""));
                    listLog.Add(new SqlPara("ErrorNode", "回单取消"));
                    listLog.Add(new SqlPara("ExceptMessage", model.Message));
                    SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                    SqlHelper.ExecteNonQuery(spsLog);
                    // MsgBox.ShowOK(model.Message);
                }
                //插入异步同步信息表
                string billnoStrs = "";
                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                {
                    billnoStrs = billnoStrs + ds.Tables[0].Rows[k]["BillNO"].ToString() + ",";
                }
                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_cancel_receipt_ZQTMS_Syn"));
                listAsy.Add(new SqlPara("FaceUrl", url));
                listAsy.Add(new SqlPara("FaceJson", json));
                listAsy.Add(new SqlPara("BillNos", billnoStrs));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "回单取消"));
                listAsy.Add(new SqlPara("SystemSource", "LMS干线升级版"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));


                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                List<SqlPara> listLog = new List<SqlPara>();
                listLog.Add(new SqlPara("BillNo", dic["BillNo"].ToString()));
                listLog.Add(new SqlPara("Batch", ""));
                listLog.Add(new SqlPara("ErrorNode", "回单取消"));
                listLog.Add(new SqlPara("ExceptMessage", ex.Message));
                SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                SqlHelper.ExecteNonQuery(spsLog);
            }
        }

        /// <summary>
        /// 回单寄出同步
        /// </summary>
        public static void ReceiptSendeSyn(Dictionary<string, string> dic)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNos", dic["allBillNo"].ToString()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_Receipt_Syn", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count < 0) return;
                string BillNos = ds.Tables[0].Rows[0]["BillNos"].ToString();
                if (BillNos == "") return;

                dic["allBillNo"] = BillNos;
                //SqlParasEntity sps=new SqlParasEntity(OperType.Query,"",list);
                string listJson = JsonConvert.SerializeObject(dic);
                RequestModel<string> request = new RequestModel<string>();
                request.Request = listJson;
                request.OperType = 0;
                string json = JsonConvert.SerializeObject(request);
                string url = HttpHelper.urlReceiptSendNewSyn;
                ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                if (model.State != "200")
                {
                    List<SqlPara> listLog = new List<SqlPara>();
                    listLog.Add(new SqlPara("BillNo", dic["allBillNo"].ToString()));
                    listLog.Add(new SqlPara("Batch", ""));
                    listLog.Add(new SqlPara("ErrorNode", dic["ReceiptState"].ToString()));
                    listLog.Add(new SqlPara("ExceptMessage", model.Message));
                    SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                    SqlHelper.ExecteNonQuery(spsLog);
                    // MsgBox.ShowOK(model.Message);
                }

                //插入异步同步信息表
                string billnoStrs = "";
                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                {
                    billnoStrs = billnoStrs + ds.Tables[0].Rows[k]["BillNO"].ToString() + ",";
                }
                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_ADD_Receipt_NEW_Syn"));
                listAsy.Add(new SqlPara("FaceUrl", url));
                listAsy.Add(new SqlPara("FaceJson", json));
                listAsy.Add(new SqlPara("BillNos", billnoStrs));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "回单寄出同步"));
                listAsy.Add(new SqlPara("SystemSource", "LMS干线升级版"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));


                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                List<SqlPara> listLog = new List<SqlPara>();
                listLog.Add(new SqlPara("BillNo", dic["allBillNo"].ToString()));
                listLog.Add(new SqlPara("Batch", ""));
                listLog.Add(new SqlPara("ErrorNode", dic["ReceiptState"].ToString()));
                listLog.Add(new SqlPara("ExceptMessage", ex.Message));
                SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                SqlHelper.ExecteNonQuery(spsLog);
            }
        }


        /// <summary>
        /// 异常登记同步
        /// </summary>
        /// <param name="BillNo"></param>
        public static void AbnormalSyn(string BillNo)
        {
            DataSet dsDJ = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILLAPPLY_BYBILLNO_register", new List<SqlPara>() { new SqlPara("BillNo", BillNo) }));
            if (dsDJ == null || dsDJ.Tables.Count == 0 || dsDJ.Tables[0].Rows.Count == 0) return;
            string dsJson = JsonConvert.SerializeObject(dsDJ);
            RequestModel<string> request = new RequestModel<string>();
            request.Request = dsJson;
            request.OperType = 0;
            string json = JsonConvert.SerializeObject(request);
            string url = HttpHelper.urlAbnormalSyn;
            ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
            if (model.State != "200")
            {
                List<SqlPara> listLog = new List<SqlPara>();
                listLog.Add(new SqlPara("BillNo", BillNo));
                listLog.Add(new SqlPara("Batch", ""));
                listLog.Add(new SqlPara("ErrorNode", "异常登记"));
                listLog.Add(new SqlPara("ExceptMessage", model.Message));
                SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                SqlHelper.ExecteNonQuery(spsLog);
            }
            //插入异步同步信息表
            string billnoStrs = "";
            for (int k = 0; k < dsDJ.Tables[0].Rows.Count; k++)
            {
                billnoStrs = billnoStrs + dsDJ.Tables[0].Rows[k]["BillNO"].ToString() + ",";
            }
            List<SqlPara> listAsy = new List<SqlPara>();
            listAsy.Add(new SqlPara("FuntionName", "USP_AbnormalSyn"));
            listAsy.Add(new SqlPara("FaceUrl", url));
            listAsy.Add(new SqlPara("FaceJson", json));
            listAsy.Add(new SqlPara("BillNos", billnoStrs));
            listAsy.Add(new SqlPara("Batch", ""));
            listAsy.Add(new SqlPara("NodeName", "异常登记"));
            listAsy.Add(new SqlPara("SystemSource", "LMS干线升级版"));
            listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));


            SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
            SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
        }

        /// <summary>
        /// 撤销异常登记同步
        /// </summary>
        /// <param name="BillNo"></param>
        public static void UndoAbnormalSyn(string BillNo)
        {
            DataSet dsDJ = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILLAPPLY_BYBILLNO_register", new List<SqlPara>() { new SqlPara("BillNo", BillNo) }));
            if (dsDJ == null || dsDJ.Tables.Count == 0 || dsDJ.Tables[0].Rows.Count == 0) return;
            string dsJson = JsonConvert.SerializeObject(dsDJ);
            RequestModel<string> request = new RequestModel<string>();
            request.Request = dsJson;
            request.OperType = 0;
            string json = JsonConvert.SerializeObject(request);
            string url = HttpHelper.urlUndoAbnormalSyn;
            ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
            if (model.State != "200")
            {
                List<SqlPara> listLog = new List<SqlPara>();
                listLog.Add(new SqlPara("BillNo", BillNo));
                listLog.Add(new SqlPara("Batch", ""));
                listLog.Add(new SqlPara("ErrorNode", "异常登记"));
                listLog.Add(new SqlPara("ExceptMessage", model.Message));
                SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                SqlHelper.ExecteNonQuery(spsLog);
            }
            //插入异步同步信息表
            string billnoStrs = "";
            for (int k = 0; k < dsDJ.Tables[0].Rows.Count; k++)
            {
                billnoStrs = billnoStrs + dsDJ.Tables[0].Rows[k]["BillNO"].ToString() + ",";
            }
            List<SqlPara> listAsy = new List<SqlPara>();
            listAsy.Add(new SqlPara("FuntionName", "USP_UndoAbnormalSyn"));
            listAsy.Add(new SqlPara("FaceUrl", url));
            listAsy.Add(new SqlPara("FaceJson", json));
            listAsy.Add(new SqlPara("BillNos", billnoStrs));
            listAsy.Add(new SqlPara("Batch", ""));
            listAsy.Add(new SqlPara("NodeName", "异常登记"));
            listAsy.Add(new SqlPara("SystemSource", "LMS干线升级版"));
            listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));


            SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
            SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
        }

        /// <summary>
        /// LMS查询ZQTMS理赔审批信息
        /// </summary>
        public static DataSet GetZQTMSClaimMessage(string BillNo, string bdate, string edate, string CauseName, string AreaName, string BegWeb, string ProcedureName)
        {
            try
            {
                if (string.IsNullOrEmpty(ProcedureName) || (string.IsNullOrEmpty(BillNo) && string.IsNullOrEmpty(bdate)))
                {
                    return null;
                }
                DataSet ds = new DataSet();
                string strError = string.Empty;
                List<string> list = new List<string>();
                list.Add(BillNo);
                list.Add(bdate);
                list.Add(edate);
                list.Add(CauseName);
                list.Add(AreaName);
                list.Add(BegWeb);
                list.Add(ProcedureName);
                string dsJson = JsonConvert.SerializeObject(list);
                RequestModel<string> request = new RequestModel<string>();
                request.Request = dsJson;
                request.OperType = 0;
                string json = JsonConvert.SerializeObject(request);
                string url = HttpHelper.urlGetZQTMSClaimMessage;//获取接口地址
                ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                if (model.State != "1")
                {
                    List<SqlPara> listLog = new List<SqlPara>();
                    listLog.Add(new SqlPara("BillNo", BillNo));
                    listLog.Add(new SqlPara("Batch", ""));
                    listLog.Add(new SqlPara("ErrorNode", "ZQTMS理赔审批信息查询"));
                    listLog.Add(new SqlPara("ExceptMessage", model.Message));
                    SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                    SqlHelper.ExecteNonQuery(spsLog);
                    return null;
                }
                DataOper.Decompress(model.Result, ref ds, ref strError);
                return ds;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                return null;
            }
        }

        /// <summary>
        /// 费用异动执行同步  //maohui20180514
        /// </summary>
        /// <param name="BillNo"></param>
        /// <param name="ApplyID"></param>
        public static void FreightChangesExcute(string ApplyID)
        {
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_FreightChangesExcute_LMS", new List<SqlPara>() { new SqlPara("ApplyID", ApplyID) }));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return;
            string dsJson = JsonConvert.SerializeObject(ds);
            RequestModel<string> request = new RequestModel<string>();
            request.Request = dsJson;
            request.OperType = 0;
            string json = JsonConvert.SerializeObject(request);
            string url = HttpHelper.urlFreightChangesExcuteSyn;
            ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
            if (model.State != "200")
            {
                List<SqlPara> listLog = new List<SqlPara>();
                listLog.Add(new SqlPara("BillNo", ds.Tables[0].Rows[0]["BillNo"]));
                listLog.Add(new SqlPara("Batch", ""));
                listLog.Add(new SqlPara("ErrorNode", "费用异动申请"));
                listLog.Add(new SqlPara("ExceptMessage", model.Message));
                SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                SqlHelper.ExecteNonQuery(spsLog);
            }
            //插入异步同步信息表
            string billnoStrs = "";
            for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
            {
                billnoStrs = billnoStrs + ds.Tables[0].Rows[k]["BillNO"].ToString() + ",";
            }
            List<SqlPara> listAsy = new List<SqlPara>();
            listAsy.Add(new SqlPara("FuntionName", "USP_LMSFreightChangesExcuteSyn_LMS"));
            listAsy.Add(new SqlPara("FaceUrl", url));
            listAsy.Add(new SqlPara("FaceJson", json));
            listAsy.Add(new SqlPara("BillNos", billnoStrs));
            listAsy.Add(new SqlPara("Batch", ""));
            listAsy.Add(new SqlPara("NodeName", "费用异动申请"));
            listAsy.Add(new SqlPara("SystemSource", "LMS干线升级版"));
            listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));


            SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
            SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
        }

        /// <summary>
        /// 费用异动否决同步  //maohui20180514
        /// </summary>
        /// <param name="ApplyID"></param>
        public static void FreightChangesVeto(string ApplyID)
        {
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_FreightChangesExcute_LMS", new List<SqlPara>() { new SqlPara("ApplyID", ApplyID) }));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return;
            string dsJson = JsonConvert.SerializeObject(ds);
            RequestModel<string> request = new RequestModel<string>();
            request.Request = dsJson;
            request.OperType = 0;
            string json = JsonConvert.SerializeObject(request);
            string url = HttpHelper.urlFreightChangesVetoSyn;
            ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
            if (model.State != "200")
            {
                List<SqlPara> listLog = new List<SqlPara>();
                listLog.Add(new SqlPara("BillNo", ds.Tables[0].Rows[0]["BillNo"]));
                listLog.Add(new SqlPara("Batch", ""));
                listLog.Add(new SqlPara("ErrorNode", "费用异动申请"));
                listLog.Add(new SqlPara("ExceptMessage", model.Message));
                SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                SqlHelper.ExecteNonQuery(spsLog);
            }
            //插入异步同步信息表
            string billnoStrs = "";
            for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
            {
                billnoStrs = billnoStrs + ds.Tables[0].Rows[k]["BillNO"].ToString() + ",";
            }
            List<SqlPara> listAsy = new List<SqlPara>();
            listAsy.Add(new SqlPara("FuntionName", "USP_LMSFreightChangesVetoSyn_LMS"));
            listAsy.Add(new SqlPara("FaceUrl", url));
            listAsy.Add(new SqlPara("FaceJson", json));
            listAsy.Add(new SqlPara("BillNos", billnoStrs));
            listAsy.Add(new SqlPara("Batch", ""));
            listAsy.Add(new SqlPara("NodeName", "费用异动申请"));
            listAsy.Add(new SqlPara("SystemSource", "LMS干线升级版"));
            listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));


            SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
            SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
        }

        ///<summary>
        ///费用异动新增同步 maohui20180514
        ///<param name="BillNo"></param>
        ///<param name="recType"></param>
        public static void FreightChangesADDSyn(string BillNo, int recType)
        {
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_FreightChangeAdd_LMS", new List<SqlPara>() { new SqlPara("BillNo", BillNo), new SqlPara("recType", recType) }));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return;
            string dsJson = JsonConvert.SerializeObject(ds);
            RequestModel<string> request = new RequestModel<string>();
            request.Request = dsJson;
            request.OperType = 0;
            string json = JsonConvert.SerializeObject(request);
            string url = HttpHelper.urlFreightChangesADDSyn;
            ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
            if (model.State != "200")
            {
                List<SqlPara> listLog = new List<SqlPara>();
                listLog.Add(new SqlPara("BillNo", BillNo));
                listLog.Add(new SqlPara("Batch", ""));
                listLog.Add(new SqlPara("ErrorNode", "费用异动申请"));
                listLog.Add(new SqlPara("ExceptMessage", model.Message));
                SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                SqlHelper.ExecteNonQuery(spsLog);
            }
            //插入异步同步信息表
            string billnoStrs = "";
            for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
            {
                billnoStrs = billnoStrs + ds.Tables[0].Rows[k]["BillNO"].ToString() + ",";
            }
            List<SqlPara> listAsy = new List<SqlPara>();
            listAsy.Add(new SqlPara("FuntionName", "USP_LMSFreightChangesADDSyn_LMS"));
            listAsy.Add(new SqlPara("FaceUrl", url));
            listAsy.Add(new SqlPara("FaceJson", json));
            listAsy.Add(new SqlPara("BillNos", billnoStrs));
            listAsy.Add(new SqlPara("Batch", ""));
            listAsy.Add(new SqlPara("NodeName", "费用异动申请"));
            listAsy.Add(new SqlPara("SystemSource", "LMS干线升级版"));
            listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));


            SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
            SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
        }

        ///<summary>
        ///费用异动取消同步 maohui20180514
        ///<param name="ApplyId"></param>
        public static void FreightChangesCancelSyn(string ApplyId)
        {
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_FreightChangeCancel_ZQTMS", new List<SqlPara>() { new SqlPara("ApplyId", ApplyId) }));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return;
            string dsJson = JsonConvert.SerializeObject(ds);
            RequestModel<string> request = new RequestModel<string>();
            request.Request = dsJson;
            request.OperType = 0;
            string json = JsonConvert.SerializeObject(request);
            string url = HttpHelper.urlFreightChangesCancelSyn;
            ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
            if (model.State != "200")
            {
                List<SqlPara> listLog = new List<SqlPara>();
                listLog.Add(new SqlPara("BillNo", ds.Tables[0].Rows[0]["BillNO"]));
                listLog.Add(new SqlPara("Batch", ""));
                listLog.Add(new SqlPara("ErrorNode", "费用异动取消"));
                listLog.Add(new SqlPara("ExceptMessage", model.Message));
                SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                SqlHelper.ExecteNonQuery(spsLog);
            }
            //插入异步同步信息表
            string billnoStrs = "";
            for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
            {
                billnoStrs = billnoStrs + ds.Tables[0].Rows[k]["BillNO"].ToString() + ",";
            }
            List<SqlPara> listAsy = new List<SqlPara>();
            listAsy.Add(new SqlPara("FuntionName", "USP_LMSFreightChangesCancelSyn_LMS"));
            listAsy.Add(new SqlPara("FaceUrl", url));
            listAsy.Add(new SqlPara("FaceJson", json));
            listAsy.Add(new SqlPara("BillNos", billnoStrs));
            listAsy.Add(new SqlPara("Batch", ""));
            listAsy.Add(new SqlPara("NodeName", "费用异动取消"));
            listAsy.Add(new SqlPara("SystemSource", "LMS干线升级版"));
            listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));


            SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
            SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);

        }
        /// <summary>
        /// i 0新增 1整车作废 2单票剔除 3加入
        /// </summary>
        /// <param name="list_syn"></param>
        /// <param name="i"></param>
        /// <param name="procName"></param>
        /// <param name="strBillNos"></param>
        /// <param name="Batch"></param>
        /// <param name="compngyID"></param>
        public static void ShortReplaceLms(List<SqlPara> list_syn, int i, string procName, string strBillNos, string Batch, string compngyID)
        {
            string strMessage = string.Empty;
            string strState = string.Empty;
            string strJson = string.Empty;
            string url = string.Empty;
            string billJson = string.Empty;
            string BillNoSyns = string.Empty;
            string json = string.Empty;
            try
            {
                if (list_syn == null || list_syn.Count == 0 || string.IsNullOrEmpty(procName))
                {
                    return;
                }
                if (i == 0 || i == 3)
                {
                    List<SqlPara> list_check = new List<SqlPara>();
                    list_check.Add(new SqlPara("BillNos", strBillNos));
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
                        //List<SqlPara> list_bill = new List<SqlPara>();
                        //list_bill.Add(new SqlPara("BillNos", ds_bill.Tables[0].Rows[0]["BillNos"].ToString()));
                        //list_bill.Add(new SqlPara("Batch", Batch));
                        ////list_bill.Add(new SqlPara("companyid", compngyID));
                        //SqlParasEntity spe_bill = new SqlParasEntity(OperType.Query, "QSP_Get_Waybill_By_Param_Short", list_bill);
                        //DataSet ds_data = SqlHelper.GetDataSet(spe_bill);
                        //if (ds_data == null || ds_data.Tables.Count == 0 || ds_data.Tables[0].Rows.Count == 0)
                        //{
                        //    strState = "失败";
                        //    strMessage = "QSP_Get_Waybill_By_Param：查询同步参数失败！";
                        //}
                        //else
                        //{
                        //    billJson = JsonConvert.SerializeObject(ds_data);

                        //}

                    }
                    list_syn.Add(new SqlPara("BillNoSyns", BillNoSyns));
                    //list_syn.Add(new SqlPara("BillData", billJson));//运单数据/分拨数据
                    list_syn.Add(new SqlPara("OptionType", i == 0 ? "新增短驳" : "单票加入"));
                    list_syn.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));
                    list_syn.Add(new SqlPara("SystemType", "ZQTMS"));
                    list_syn.Add(new SqlPara("ProcName", procName));
                    strJson = JsonConvert.SerializeObject(list_syn);
                    RequestModel<string> request = new RequestModel<string>();
                    string resultJson = string.Empty;
                    bool istrue = DataOper.Compress(strJson, ref resultJson);
                    request.Request = resultJson;
                    request.OperType = 0;
                    json = JsonConvert.SerializeObject(request);
                    url = HttpHelper.urlShortReplace;
                    if (istrue)
                    {
                        ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                        if (model.State == "200")
                        {
                            strState = "成功";
                            strMessage = model.Message;
                        }
                        else
                        {
                            strState = "失败";
                            strMessage = model.Message;
                        }
                    }
                    else
                    {
                        MsgBox.ShowOK("压缩数据失败，请稍后再试！");
                        return;
                    }
                }
                if (i == 1)
                {
                    list_syn.Add(new SqlPara("ProcName", procName));
                    strJson = JsonConvert.SerializeObject(list_syn);
                    RequestModel<string> request = new RequestModel<string>();
                    string resultJson = string.Empty;
                    bool istrue = DataOper.Compress(strJson, ref resultJson);
                    request.Request = resultJson;
                    request.OperType = 0;
                    json = JsonConvert.SerializeObject(request);
                    url = HttpHelper.urlShortCommon;
                    if (istrue)
                    {
                        ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                        if (model.State == "200")
                        {
                            strState = "成功";
                            strMessage = model.Message;
                        }
                        else
                        {
                            strState = "失败";
                            strMessage = model.Message;
                        }
                    }
                    else
                    {
                        MsgBox.ShowOK("压缩数据失败，请稍后再试！");
                        return;
                    }
                }
                if (i == 2)
                {
                    list_syn.Add(new SqlPara("BillNos", strBillNos + ","));
                    list_syn.Add(new SqlPara("SCBatchNo", Batch));
                    list_syn.Add(new SqlPara("ProcName", procName));
                    strJson = JsonConvert.SerializeObject(list_syn);
                    RequestModel<string> request = new RequestModel<string>();
                    string resultJson = string.Empty;
                    bool istrue = DataOper.Compress(strJson, ref resultJson);
                    request.Request = resultJson;
                    request.OperType = 0;
                    json = JsonConvert.SerializeObject(request);
                    url = HttpHelper.urlShortCommon;
                    if (istrue)
                    {
                        ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                        if (model.State == "200")
                        {
                            strState = "成功";
                            strMessage = model.Message;
                        }
                        else
                        {
                            strState = "失败";
                            strMessage = model.Message;
                        }
                    }
                    else
                    {
                        MsgBox.ShowOK("压缩数据失败，请稍后再试！");
                        return;
                    }
                }
                if (i == 4)
                {
                    list_syn.Add(new SqlPara("BillNo", strBillNos));
                    list_syn.Add(new SqlPara("SCBatchNo", Batch));
                    list_syn.Add(new SqlPara("ProcName", procName));
                    strJson = JsonConvert.SerializeObject(list_syn);
                    RequestModel<string> request = new RequestModel<string>();
                    string resultJson = string.Empty;
                    bool istrue = DataOper.Compress(strJson, ref resultJson);
                    request.Request = resultJson;
                    request.OperType = 0;
                    json = JsonConvert.SerializeObject(request);
                    url = HttpHelper.urlShortCommon;
                    if (istrue)
                    {
                        ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                        if (model.State == "200")
                        {
                            strState = "成功";
                            strMessage = model.Message;
                        }
                        else
                        {
                            strState = "失败";
                            strMessage = model.Message;
                        }
                    }
                    else
                    {
                        MsgBox.ShowOK("压缩数据失败，请稍后再试！");
                        return;
                    }
                }

                if (i == 6)
                {
                    list_syn.Add(new SqlPara("BillNo", strBillNos));
                    list_syn.Add(new SqlPara("SCBatchNo", Batch));
                    list_syn.Add(new SqlPara("ProcName", procName));
                    strJson = JsonConvert.SerializeObject(list_syn);
                    RequestModel<string> request = new RequestModel<string>();
                    string resultJson = string.Empty;
                    bool istrue = DataOper.Compress(strJson, ref resultJson);
                    request.Request = resultJson;
                    request.OperType = 0;
                    json = JsonConvert.SerializeObject(request);
                    url = HttpHelper.urlShortCommon;
                    if (istrue)
                    {
                        ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                        if (model.State == "200")
                        {
                            strState = "成功";
                            strMessage = model.Message;
                        }
                        else
                        {
                            strState = "失败";
                            strMessage = model.Message;
                        }
                    }
                    else
                    {
                        MsgBox.ShowOK("压缩数据失败，请稍后再试！");
                        return;
                    }

                }

            }
            catch (Exception ex)
            {
                strState = "失败";
                strMessage = ex.Message;
            }
            finally
            {
                List<SqlPara> listLog = new List<SqlPara>();
                listLog.Add(new SqlPara("Batch", Batch));
                listLog.Add(new SqlPara("FutionName", procName));
                listLog.Add(new SqlPara("FaceUrl", url));
                listLog.Add(new SqlPara("FaceJson", strJson));
                listLog.Add(new SqlPara("ResultMessage", strMessage));
                listLog.Add(new SqlPara("FaceState", strState));
                listLog.Add(new SqlPara("BillNo", strBillNos));
                // listLog.Add(new SqlPara("companyid", compngyID));
                SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_InterFace_Log", listLog);
                SqlHelper.ExecteNonQuery(spsLog);
                //strJson = JsonConvert.SerializeObject(list_syn);
                //RequestModel<string> request = new RequestModel<string>();
                //request.Request = strJson;
                //request.OperType = 0;
                //string json = JsonConvert.SerializeObject(request);
                //url = HttpHelper.urlShortCommon;
                //ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
            }


        }

        /// <summary>
        /// i 0 新增
        /// </summary>
        /// <param name="list_syn"></param>
        /// <param name="i"></param>
        /// <param name="procName"></param>
        /// <param name="strBillNos"></param>
        /// <param name="Batch"></param>
        /// <param name="compngyID"></param>
        public static void ShortStateSynCommon(List<SqlPara> list_syn, int i, string procName, string strBillNos, string Batch, string compngyID)
        {
            try
            {
                string billSyns = "";
                string billstateSyns = "";
                string strJson = "";
                string url = "";
                string strMessage = string.Empty;
                string strState = string.Empty;
                if (i == 0)
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("BillNoStr", strBillNos));
                    list.Add(new SqlPara("Batch", Batch));
                    list.Add(new SqlPara("Type", i));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLSTATES", list);
                    DataSet ds = SqlHelper.GetDataSet(sps);
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        billSyns = billSyns + ds.Tables[0].Rows[j]["BillNo"].ToString() + "@";
                        billstateSyns = billstateSyns + ds.Tables[0].Rows[j]["BillState"].ToString() + "@";
                    }
                    if (!string.IsNullOrEmpty(billSyns))
                    {
                        list_syn.Add(new SqlPara("billSyns", billSyns));
                        list_syn.Add(new SqlPara("  ", billstateSyns));
                        list_syn.Add(new SqlPara("ProcName", procName));
                        strJson = JsonConvert.SerializeObject(list_syn);
                        RequestModel<string> request = new RequestModel<string>();
                        request.Request = strJson;
                        request.OperType = 0;
                        string json = JsonConvert.SerializeObject(request);
                        url = HttpHelper.urlShortCommon;
                        ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                        if (model.State == "200")
                        {
                            strState = "成功";
                            strMessage = model.Message;
                        }
                        else
                        {
                            strState = "失败";
                            strMessage = model.Message;
                        }
                    }
                }
                if (i == 2)
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("BillNoStr", strBillNos));
                    list.Add(new SqlPara("Batch", Batch));
                    list.Add(new SqlPara("Type", i));
                    //DataSet ds = SqlHelper.GetDataSet(sps);
                    //if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static void MidConfirmSyn(string billNoStr, string confirmMan, string confirmWeb)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNoStr", billNoStr));
                list.Add(new SqlPara("ConfirmMan", confirmMan));
                list.Add(new SqlPara("ConfrimWeb", confirmWeb));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_MidConfirmSyn", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                string dsJson = JsonConvert.SerializeObject(ds);
                RequestModel<string> request = new RequestModel<string>();
                request.Request = dsJson;
                request.OperType = 0;
                string json = JsonConvert.SerializeObject(request);
                string url = HttpHelper.urlMidConfirmSyn;
                ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                if (model.State != "200")
                {
                    List<SqlPara> listLog = new List<SqlPara>();
                    listLog.Add(new SqlPara("BillNo", billNoStr));
                    listLog.Add(new SqlPara("Batch", ""));
                    listLog.Add(new SqlPara("ErrorNode", "中转提付确认"));
                    listLog.Add(new SqlPara("ExceptMessage", model.Message));
                    SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                    SqlHelper.ExecteNonQuery(spsLog);
                    // MsgBox.ShowOK(model.Message);
                }
                //插入异步同步信息表
                string billnoStrs = "";
                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                {
                    billnoStrs = billnoStrs + ds.Tables[0].Rows[k]["BillNo"].ToString() + ",";
                }
                List<SqlPara> listAsy = new List<SqlPara>();
                listAsy.Add(new SqlPara("FuntionName", "USP_MidConfirmSyn_LMS"));
                listAsy.Add(new SqlPara("FaceUrl", url));
                listAsy.Add(new SqlPara("FaceJson", json));
                listAsy.Add(new SqlPara("BillNos", billnoStrs));
                listAsy.Add(new SqlPara("Batch", ""));
                listAsy.Add(new SqlPara("NodeName", "中转提付确认"));
                listAsy.Add(new SqlPara("SystemSource", "LMS干线升级版"));
                listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));


                SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
                SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
            }
            catch (Exception ex)
            {
                List<SqlPara> listLog = new List<SqlPara>();
                listLog.Add(new SqlPara("BillNo", billNoStr));
                listLog.Add(new SqlPara("Batch", ""));
                listLog.Add(new SqlPara("ErrorNode", "中转提付确认"));
                listLog.Add(new SqlPara("ExceptMessage", ex.Message));
                SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                SqlHelper.ExecteNonQuery(spsLog);
            }
        }

        /// <summary>
        /// LMS代配载同步ZQTMS配载
        /// i 0新增 1整车作废 2单票剔除 3加入
        /// </summary>
        /// <param name="billNoStr"></param>
        /// <param name="departureBatch"></param>
        /// <param name="i"></param>
        /// <param name="tracetype"></param>
        /// <param name="num"></param>
        public static void LMSDepartureSysZQTMS(List<SqlPara> list_syn, int i, string procName, string strBillNos, string Batch, string compngyID)
        {
            string strMessage = string.Empty;
            string strState = string.Empty;
            string strJson = string.Empty;
            string url = string.Empty;
            string json = string.Empty;
            try
            {
                if (list_syn == null || list_syn.Count == 0 || string.IsNullOrEmpty(procName))
                {
                    return;
                }
                string strOptionType = "其它";
                if (i == 0 || i == 3)
                {
                    strOptionType = (i == 0 ? "新增配载" : "单票加入");

                    #region 注释代码
                    ////检查ZQTMS运单表 是否存在数据
                    //List<SqlPara> list_check = new List<SqlPara>();
                    //list_check.Add(new SqlPara("BillNos", strBillNos));
                    //SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_Check_Waybill_By_FB", list_check);
                    //DataSet ds_bill = SqlHelper.GetDataSet_ZQTMS(spe);
                    //if (ds_bill == null || ds_bill.Tables.Count == 0 || ds_bill.Tables[0].Rows.Count == 0)
                    //{
                    //    strState = "失败";
                    //    strMessage = "QSP_Check_Waybill_By_FB：不存在需要同步配载至ZQTMS系统的单号！";
                    //}
                    //else
                    //{
                    //    //根据不存在ZQTMS库的运单号，查询LMS库拼接新增运单参数
                    //    List<SqlPara> list_bill = new List<SqlPara>();
                    //    list_bill.Add(new SqlPara("BillNos", ds_bill.Tables[0].Rows[0]["BillNos"].ToString()));
                    //    list_bill.Add(new SqlPara("Batch", Batch));
                    //    //list_bill.Add(new SqlPara("companyid", compngyID));
                    //    SqlParasEntity spe_bill = new SqlParasEntity(OperType.Query, "QSP_Get_Waybill_By_Param", list_bill);
                    //    DataSet ds_data = SqlHelper.GetDataSet(spe_bill);
                    //    if (ds_data == null || ds_data.Tables.Count == 0 || ds_data.Tables[0].Rows.Count == 0)
                    //    {
                    //        strState = "失败";
                    //        strMessage = "QSP_Get_Waybill_By_Param：查询同步参数失败！";
                    //    }
                    //    else
                    //    {
                    //        string billJson = JsonConvert.SerializeObject(ds_data);

                    //        list_syn.Add(new SqlPara("BillData", CommonClass.Encrypt(billJson, "3E67D26C-8BDB-48F1-8E64-90CC7A7E6EE5")));//运单数据/分拨数据
                    //        list_syn.Add(new SqlPara("OptionType", i == 0 ? "新增配载" : "单票加入"));
                    //    }
                    //}
                    #endregion
                }
                //else if (i == 1)
                //{
                //    list_syn.Add(new SqlPara("BillNos", strBillNos));
                //}
                //if (strState != "失败")
                //{
                //公用参数
                if (!list_syn.Contains(new SqlPara("companyid", compngyID))) list_syn.Add(new SqlPara("companyid", compngyID));
                list_syn.Add(new SqlPara("SystemType", "LMS"));
                list_syn.Add(new SqlPara("ProcName", procName));
                list_syn.Add(new SqlPara("OptionType", strOptionType));
                if (!list_syn.Contains(new SqlPara("BillNos", strBillNos))) list_syn.Add(new SqlPara("BillNos", strBillNos));
                if (!list_syn.Contains(new SqlPara("Batch", Batch))) list_syn.Add(new SqlPara("Batch", Batch));

                strJson = JsonConvert.SerializeObject(list_syn);
                RequestModel<string> request = new RequestModel<string>();
                string resultJson = string.Empty;
                bool istrue = DataOper.Compress(strJson, ref resultJson);//压缩文件
                request.Request = resultJson;
                request.OperType = 0;
                json = JsonConvert.SerializeObject(request);
                if (istrue)
                {
                    if (i == 0 || i == 3)
                    {
                        url = HttpHelper.urlLMSSysDeparttureZQTMS;
                    }
                    else
                    {
                        url = HttpHelper.urlLMSSysExecuteZQTMSCurrency;
                    }
                    ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                    if (model.State == "200")
                    {
                        strState = "成功";
                        strMessage = model.Message;
                    }
                    else
                    {
                        strState = "失败";
                        strMessage = model.Message;
                    }
                }
                else
                {
                    MsgBox.ShowOK("压缩数据失败，请稍后再试！");
                    return;
                }
                //}
            }
            catch (Exception ex)
            {
                //记录同步失败信息
                strState = "失败";
                strMessage = ex.Message;
            }
            try
            {
                List<SqlPara> listLog = new List<SqlPara>();
                listLog.Add(new SqlPara("Batch", Batch));
                listLog.Add(new SqlPara("FutionName", procName));
                listLog.Add(new SqlPara("FaceUrl", url));
                listLog.Add(new SqlPara("FaceJson", json));
                listLog.Add(new SqlPara("ResultMessage", strMessage));
                listLog.Add(new SqlPara("FaceState", strState));
                listLog.Add(new SqlPara("BillNo", strBillNos));
                //listLog.Add(new SqlPara("companyid", compngyID));
                SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_InterFace_Log", listLog);
                SqlHelper.ExecteNonQuery(spsLog);
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// LMS代配载同步ZQTMS配载，手动同步失败数据（frmPayInfos，支付信息记录界面做手动同步）
        /// i 0新增 1整车作废 2单票剔除 3加入
        /// </summary>
        /// <param name="billNoStr"></param>
        /// <param name="departureBatch"></param>
        /// <param name="i"></param>
        /// <param name="tracetype"></param>
        /// <param name="num"></param>
        public static void ShouDongLMSDepartureSysZQTMS(string strJson, int i, string GUID)
        {
            string strMessage = string.Empty;
            string strState = string.Empty;
            string url = string.Empty;
            try
            {
                //if (list_syn == null || list_syn.Count == 0 || string.IsNullOrEmpty(procName))
                //{
                //    return;
                //}
                //string strOptionType = "其它";
                //if (i == 0 || i == 3)
                //{
                //    strOptionType = (i == 0 ? "新增配载" : "单票加入");

                #region 注释代码
                ////检查ZQTMS运单表 是否存在数据
                //List<SqlPara> list_check = new List<SqlPara>();
                //list_check.Add(new SqlPara("BillNos", strBillNos));
                //SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_Check_Waybill_By_FB", list_check);
                //DataSet ds_bill = SqlHelper.GetDataSet_ZQTMS(spe);
                //if (ds_bill == null || ds_bill.Tables.Count == 0 || ds_bill.Tables[0].Rows.Count == 0)
                //{
                //    strState = "失败";
                //    strMessage = "QSP_Check_Waybill_By_FB：不存在需要同步配载至ZQTMS系统的单号！";
                //}
                //else
                //{
                //    //根据不存在ZQTMS库的运单号，查询LMS库拼接新增运单参数
                //    List<SqlPara> list_bill = new List<SqlPara>();
                //    list_bill.Add(new SqlPara("BillNos", ds_bill.Tables[0].Rows[0]["BillNos"].ToString()));
                //    list_bill.Add(new SqlPara("Batch", Batch));
                //    //list_bill.Add(new SqlPara("companyid", compngyID));
                //    SqlParasEntity spe_bill = new SqlParasEntity(OperType.Query, "QSP_Get_Waybill_By_Param", list_bill);
                //    DataSet ds_data = SqlHelper.GetDataSet(spe_bill);
                //    if (ds_data == null || ds_data.Tables.Count == 0 || ds_data.Tables[0].Rows.Count == 0)
                //    {
                //        strState = "失败";
                //        strMessage = "QSP_Get_Waybill_By_Param：查询同步参数失败！";
                //    }
                //    else
                //    {
                //        string billJson = JsonConvert.SerializeObject(ds_data);

                //        list_syn.Add(new SqlPara("BillData", CommonClass.Encrypt(billJson, "3E67D26C-8BDB-48F1-8E64-90CC7A7E6EE5")));//运单数据/分拨数据
                //        list_syn.Add(new SqlPara("OptionType", i == 0 ? "新增配载" : "单票加入"));
                //    }
                //}
                #endregion
                //}
                //else if (i == 1)
                //{
                //    list_syn.Add(new SqlPara("BillNos", strBillNos));
                //}
                //if (strState != "失败")
                //{
                //公用参数
                //if (!list_syn.Contains(new SqlPara("companyid", compngyID))) list_syn.Add(new SqlPara("companyid", compngyID));
                //list_syn.Add(new SqlPara("SystemType", "LMS"));
                //list_syn.Add(new SqlPara("ProcName", procName));
                //list_syn.Add(new SqlPara("OptionType", strOptionType));
                //if (!list_syn.Contains(new SqlPara("BillNos", strBillNos))) list_syn.Add(new SqlPara("BillNos", strBillNos));
                //if (!list_syn.Contains(new SqlPara("Batch", Batch))) list_syn.Add(new SqlPara("Batch", Batch));

                //strJson = JsonConvert.SerializeObject(list_syn);

                RequestModel<string> request = new RequestModel<string>();
                request.Request = strJson;
                request.OperType = 0;
                string json = JsonConvert.SerializeObject(request);
                if (i == 0 || i == 3)
                {
                    url = HttpHelper.urlLMSSysDeparttureZQTMS;
                }
                else
                {
                    url = HttpHelper.urlLMSSysExecuteZQTMSCurrency;
                }
                ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                if (model.State == "200")
                {
                    strState = "成功";
                    strMessage = model.Message;
                }
                else
                {
                    strState = "失败";
                    strMessage = model.Message;
                }
                //}
            }
            catch (Exception ex)
            {
                //记录同步失败信息
                strState = "失败";
                strMessage = ex.Message;
            }
            try
            {
                //List<SqlPara> listLog = new List<SqlPara>();
                //listLog.Add(new SqlPara("Batch", Batch));
                //listLog.Add(new SqlPara("FutionName", procName));
                //listLog.Add(new SqlPara("FaceUrl", url));
                //listLog.Add(new SqlPara("FaceJson", strJson));
                //listLog.Add(new SqlPara("ResultMessage", strMessage));
                //listLog.Add(new SqlPara("FaceState", strState));
                //listLog.Add(new SqlPara("BillNo", strBillNos));
                ////listLog.Add(new SqlPara("companyid", compngyID));
                //SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_InterFace_Log", listLog);
                //SqlHelper.ExecteNonQuery(spsLog);
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// 毛慧加
        /// </summary>
        /// <param name="list"></param>
        /// <param name="SynNode"></param>
        /// <param name="ProcName"></param>
        public static void LMSSynZQTMS(List<SqlPara> list, string SynNode, string ProcName)
        {
            string strMessage = string.Empty;
            string strState = string.Empty;
            string strJson = string.Empty;
            string url = string.Empty;
            string json = string.Empty;
            try
            {
                if (list == null || list.Count == 0 || string.IsNullOrEmpty(ProcName))
                {
                    return;
                }
                list.Add(new SqlPara("ProcName", ProcName));
                string ListJson = JsonConvert.SerializeObject(list);
                RequestModel<string> request = new RequestModel<string>();
                string resultJson = string.Empty;
                bool istrue = DataOper.Compress(ListJson, ref resultJson);//压缩文件
                request.Request = resultJson;
                request.OperType = 0;
                json = JsonConvert.SerializeObject(request);
                url = HttpHelper.urlLMSSysExecuteZQTMSCurrency;
                if (istrue)
                {
                    ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                    if (model.State == "200")
                    {
                        strState = "成功";
                        strMessage = model.Message;
                    }
                    else
                    {
                        strState = "失败";
                        strMessage = model.Message;
                    }
                }
                else
                {
                    MsgBox.ShowOK("数据压缩失败，请稍后再试！");
                    return;
                }
            }
            catch (Exception ex)
            {
                strState = "失败";
                strMessage = ex.Message;
            }
            finally
            {
                List<SqlPara> listLog = new List<SqlPara>();
                listLog.Add(new SqlPara("Batch", ""));
                listLog.Add(new SqlPara("FutionName", ProcName));
                listLog.Add(new SqlPara("FaceUrl", url));
                listLog.Add(new SqlPara("FaceJson", json));
                listLog.Add(new SqlPara("ResultMessage", strMessage));
                listLog.Add(new SqlPara("FaceState", strState));
                listLog.Add(new SqlPara("BillNo", ""));
                // listLog.Add(new SqlPara("companyid", compngyID));
                SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_InterFace_Log", listLog);
                SqlHelper.ExecteNonQuery(spsLog);
            }
        }

        /// <summary>
        /// 配载发送订单 ld20180912
        /// </summary>
        /// <param name="ds">订单源</param>
        public static string SendHaoDuoCheOrder(DataSet ds, string CompanyId, string CompanyName, string strToken)
        {
            string strDomaim = string.Empty;
            HttpHelper.GetDomaim(ref strDomaim);
            if (strDomaim != "http://120.78.229.221:7060/")
            {
                return strDomaim + "测试库不允许推送订单！";
            }
            string strMessage = string.Empty;
            string strJson = string.Empty;
            string url = string.Empty;
            ApiResult apiResult = new ApiResult();
            try
            {
                strJson = JsonConvert.SerializeObject(ds).ToString();
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0 || ds.Tables[1].Rows.Count == 0)
                {
                    return "Table0或Table1不存在数据行！@" + strJson;
                }
                //检查是否已推送成功
                //List<SqlPara> list = new List<SqlPara>();
                //list.Add(new SqlPara("DepartureBatch", ds.Tables[0].Rows[0]["orderCoding"].ToString()));
                //SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_Get_CheckOrderCoding_IsSend", list);
                //DataSet ds_check = SqlHelper.GetDataSet(spe);
                //if (ds_check != null && ds_check.Tables.Count > 0 && ds_check.Tables[0].Rows.Count > 0)
                //{
                //    return ds.Tables[0].Rows[0]["orderCoding"].ToString() + "：已推送，不允许重复操作！@" + strJson;
                //}
                if (ds.Tables.Count > 2)
                {
                    int index = ds.Tables[1].Rows.Count - 1;
                    string addr = ds.Tables[1].Rows[index]["Address"].ToString();
                    if (ds.Tables[2].Rows.Count > 0 && ds.Tables[2].Rows[0]["Address"].ToString() != addr)//当第二卸货地址存在并且和第一卸货地址不一致才推送第二卸货地址
                    {
                        DataTable dt = ds.Tables[1];
                        DataRow dr = dt.NewRow();
                        dr["sn"] = ds.Tables[2].Rows[0]["sn"].ToString();
                        dr["name"] = ds.Tables[2].Rows[0]["Address"].ToString();
                        dr["Address"] = ds.Tables[2].Rows[0]["Address"].ToString();
                        dr["cityName"] = "";
                        dr["code"] = "";
                        dr["cityCode"] = "";
                        dr["loadName"] = "";
                        dr["loadPhone"] = "";
                        dr["lon"] = "";
                        dr["lat"] = "";
                        dr["nodeType"] = ds.Tables[2].Rows[0]["nodeType"].ToString();
                        if (ds.Tables[1].Rows[0]["planArriveTime"].ToString() != "")
                        {
                            dr["planArriveTime"] = ds.Tables[1].Rows[0]["planArriveTime"].ToString();
                        }
                        else
                        {
                            dr["planArriveTime"] = DBNull.Value;
                        }

                        dr["planSendTime"] = DBNull.Value;
                        dt.Rows.Add(dr);
                        ds.Tables.RemoveAt(2);
                        ds.Tables.RemoveAt(1);
                        ds.Tables.Add(dt);
                        int times = ds.Tables[1].Rows.Count;
                        string t1 = ds.Tables[1].Rows[0]["planArriveTime"].ToString();
                        string t2 = ds.Tables[1].Rows[0]["planSendTime"].ToString();
                        //ds.Tables[1].Columns["planArriveTime"].DefaultValue = DBNull.Value;
                        //ds.Tables[1].Columns["planSendTime"].DefaultValue = DBNull.Value;
                        for (int i = 0; i < times; i++)
                        {
                            ds.Tables[1].Rows[i]["planArriveTime"] = DBNull.Value;
                            ds.Tables[1].Rows[i]["planSendTime"] = DBNull.Value;
                        }
                        ds.Tables[1].Rows[times - 1]["planArriveTime"] = t1;
                        if (t2 == "")
                        {
                            ds.Tables[1].Rows[0]["planSendTime"] = DBNull.Value;
                        }
                        else
                        {
                            ds.Tables[1].Rows[0]["planSendTime"] = t2;
                        }
                    }
                }
                else
                {
                    string t1 = ds.Tables[1].Rows[0]["planArriveTime"].ToString();
                    string t2 = ds.Tables[1].Rows[0]["planSendTime"].ToString();
                    int t = ds.Tables[1].Rows.Count;
                    for (int i = 0; i < t; i++)
                    {
                        ds.Tables[1].Rows[i]["planArriveTime"] = DBNull.Value;
                        ds.Tables[1].Rows[i]["planSendTime"] = DBNull.Value;
                    }
                    ds.Tables[1].Rows[t - 1]["planArriveTime"] = t1;
                    if (t2 == "")
                    {
                        ds.Tables[1].Rows[0]["planSendTime"] = DBNull.Value;
                    }
                    else
                    {
                        ds.Tables[1].Rows[0]["planSendTime"] = t2;
                    }
                }

                string strSignData = CommonClass.MD5Encrypt(strToken + "dekun");
                ds.Tables[1].TableName = "nodeList";
                ds.Tables[0].Rows[0]["signData"] = strSignData;
                ds.Tables[0].Rows[0]["regionName"] = CompanyName;
                ds.Tables[0].Rows[0]["regionCode"] = CompanyId;
                ds.Tables[0].Rows[0]["token"] = strToken;
                strJson = JsonConvert.SerializeObject(ds).ToString().Replace("\"Table\":[{", "").Replace("}],", ",");

                #region 注释代码
                //ResponseModelHaoDuoChe<DataTable> rmh = new ResponseModelHaoDuoChe<DataTable>();
                //rmh.token = "3E67D26C-8BDB-48F1-8E64-90CC7A7E6EE1";
                //rmh.signData = strSignData;
                //rmh.orderCoding = ds.Tables[0].Rows[0]["orderCoding"].ToString();
                //rmh.goodsName = ds.Tables[0].Rows[0]["goodsName"] == null ? "" : ds.Tables[0].Rows[0]["goodsName"].ToString();
                //rmh.weight = ds.Tables[0].Rows[0]["weight"] == null ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["weight"]);
                //rmh.volume = ds.Tables[0].Rows[0]["volume"] == null ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["volume"]);
                //rmh.VehicleNo = ds.Tables[0].Rows[0]["vehicleNo"] == null ? "" : ds.Tables[0].Rows[0]["vehicleNo"].ToString();
                //rmh.vehicleTypeId = ds.Tables[0].Rows[0]["vehicleTypeId"] == null ? 0 : Convert.ToInt32(ds.Tables[0].Rows[0]["vehicleTypeId"]);
                //rmh.vehicleLength = ds.Tables[0].Rows[0]["vehicleLength"] == null ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["vehicleLength"]);
                //rmh.DriverPhone = ds.Tables[0].Rows[0]["driverPhone"] == null ? "" : ds.Tables[0].Rows[0]["driverPhone"].ToString();
                //rmh.DirverName = ds.Tables[0].Rows[0]["dirverName"] == null ? "" : ds.Tables[0].Rows[0]["dirverName"].ToString();
                //rmh.DriverIDCardNo = ds.Tables[0].Rows[0]["driverIDCardNo"] == null ? "" : ds.Tables[0].Rows[0]["driverIDCardNo"].ToString();
                //rmh.timeLine = ds.Tables[0].Rows[0]["timeLine"] == null ? 0 : Convert.ToInt32(ds.Tables[0].Rows[0]["timeLine"]);
                //rmh.price = ds.Tables[0].Rows[0]["price"] == null ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["price"]);
                //rmh.SendPayment = ds.Tables[0].Rows[0]["sendPayment"] == null ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["sendPayment"]);
                //rmh.ToPayment = ds.Tables[0].Rows[0]["toPayment"] == null ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["toPayment"]);
                //rmh.ReceiptPayment = ds.Tables[0].Rows[0]["receiptPayment"] == null ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["receiptPayment"]);
                //rmh.OilCardPayment = ds.Tables[0].Rows[0]["oilCardPayment"] == null ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["oilCardPayment"]);
                //rmh.OilPayment = ds.Tables[0].Rows[0]["oilPayment"] == null ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["oilPayment"]);
                //rmh.SevicesMoney = ds.Tables[0].Rows[0]["sevicesMoney"] == null ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["sevicesMoney"]);
                //rmh.nodeList = ds.Tables[1];

                //strJson = JsonConvert.SerializeObject(rmh);
                #endregion
                //strJson = "{\"token\":\"3E67D26C-8BDB-48F1-8E64-90CC7A7E6EE1\",\"signData\":\"6acbe47dce423b779ca4ada9f8e274e1\", \"orderCoding\":\"DeKun001\",\"goodsName\": \"普货\",\"weight\": 8000,\"volume\": 100,\"vehicleNo\": \"陕D68123\",\"vehicleTypeId\": 5,\"vehicleLength\": 9600,\"driverPhone\": \"18089185555\",\"dirverName\": \"司机名称\",\"driverIDCardNo\": \"610404198504250513\",\"timeLine\":72,\"price\":10000, \"sendPayment\": 2000,\"toPayment\": 2000,\"receiptPayment\": 2000,\"oilCardPayment\": 2000,\"oilPayment\": 2000,\"sevicesMoney\": 500,\"nodeList\": [{\"sn\": 0, \"name\":\"A网点\",\"Address\":\"A网点地址\",\"code\":\"A网点编码\",\"cityName\":\"西安市\",\"cityCode\":710000,\"loadName\":\"张某某\",\"loadPhone\":\"18089182222\",\"lat\":111.111111,\"lon\":111.111111,\"nodeType\":0,\"planArriveTime\":\"2018-09-13 10:00\",\"planSendTime\":\"2018-09-10 10:00\"},{\"sn\": 1, \"name\":\"B网点\",\"cityCode\":820000,\"address\":\"B网点地址\",\"code\":\"B网点编码\",\"cityName\":\"北京市\",\"loadName\":\"李某某\",\"loadPhone\":\"18089181111\",\"lat\":222.222222,\"lon\":222.222222,\"nodeType\":0,\"planArriveTime\":\"2018-09-13 10:00\",\"planSendTime\":\"2018-09-10 10:00\"}]}";

                url = "http://open.hdc56.com/ApiV2/CreateOrder";
                //url = "http://hdc.open.e6gpshk.com/ApiV2/CreateOrder";
                //url = "http://ZQTMS.dekuncn.com:8079/UnionQrTransferWhiteList";
                string ResultJson = HttpHelper.HttpPostJava(strJson, url);
                if (ResultJson.Contains("{"))
                {
                    apiResult = JsonConvert.DeserializeObject<ApiResult>(ResultJson);
                }

                if (apiResult != null)
                {
                    if (apiResult.code == 1)
                    {
                        //MsgBox.ShowOK("下单成功：" + apiResult.orderId.ToString());
                        strMessage = "下单成功：" + apiResult.orderId.ToString();
                    }
                    else
                    {
                        strMessage = apiResult.desc;
                    }
                }
                else
                {
                    strMessage = ResultJson;
                }
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(strJson))
                {
                    strJson = ds.Tables[0].Rows[0]["orderCoding"].ToString();
                }
                strMessage = ex.Message;
            }
            strMessage = strMessage == null ? "" : strMessage;
            try
            {
                List<SqlPara> listLog = new List<SqlPara>();
                listLog.Add(new SqlPara("Batch", ds.Tables[0].Rows[0]["orderCoding"].ToString()));
                listLog.Add(new SqlPara("FutionName", "SendHaoDuoCheOrder"));
                listLog.Add(new SqlPara("FaceUrl", url));
                listLog.Add(new SqlPara("FaceJson", strJson));
                listLog.Add(new SqlPara("ResultMessage", strMessage));
                listLog.Add(new SqlPara("FaceState", apiResult.code == 1 ? "成功" : "失败"));
                SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_InterFace_Log", listLog);
                if (SqlHelper.ExecteNonQuery(spsLog) > 0)
                {
                    return "成功";
                }
                else
                {
                    return strMessage + "@" + strJson;
                }
            }
            catch (Exception ex)
            {
                return ex.Message + "@" + strJson;
            }
        }

        /// <summary>
        /// 取消配载订单 ld20180912
        /// </summary>
        /// <param name="ContractNO">合同编号</param>
        public static void CancleHaoDuoCheOrder(string ContractNO, string strToken)
        {
            string strMessage = string.Empty;
            string strJson = string.Empty;
            string url = string.Empty;
            ApiResult apiResult = new ApiResult();
            try
            {
                if (string.IsNullOrEmpty(ContractNO))
                {
                    return;
                }
                //检查是否已推送成功
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", ContractNO));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_Get_CheckOrderCoding_IsSend", list);
                DataSet ds_check = SqlHelper.GetDataSet(spe);
                if (ds_check != null && ds_check.Tables.Count > 0 && ds_check.Tables[0].Rows.Count == 0)
                {
                    return;
                }

                string strSignData = CommonClass.MD5Encrypt(strToken + "dekun");

                strJson = "{\"token\":\"" + strToken + "\",\"signData\":\"" + strSignData + "\",\"orderCoding\":\"" + ContractNO + "\"}";

                url = "http://open.hdc56.com/ApiV2/CancleOrder";
                //url = "http://hdc.open.e6gpshk.com/ApiV2/CancleOrder";

                string ResultJson = HttpHelper.HttpPostJava(strJson, url);
                if (ResultJson.Contains("{"))
                {
                    apiResult = JsonConvert.DeserializeObject<ApiResult>(ResultJson);
                }

                if (apiResult != null)
                {
                    if (apiResult.retCode == 1)
                    {
                        //MsgBox.ShowOK("取消成功！");
                        strMessage = "取消成功";
                    }
                    else
                    {
                        strMessage = apiResult.desc;
                    }
                }
                else
                {
                    strMessage = ResultJson;
                }
            }
            catch (Exception ex)
            {
                strMessage = ex.Message;
            }
            try
            {
                List<SqlPara> listLog = new List<SqlPara>();
                listLog.Add(new SqlPara("Batch", ContractNO));
                listLog.Add(new SqlPara("FutionName", "CancleHaoDuoCheOrder"));
                listLog.Add(new SqlPara("FaceUrl", url));
                listLog.Add(new SqlPara("FaceJson", strJson));
                listLog.Add(new SqlPara("ResultMessage", strMessage == null ? "" : strMessage));
                listLog.Add(new SqlPara("FaceState", apiResult.retCode == 1 ? "成功" : "失败"));
                SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_InterFace_Log", listLog);
                SqlHelper.ExecteNonQuery(spsLog);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 取消发车打卡 ld20180912
        /// </summary>
        /// <param name="ContractNO">合同编号</param>
        public static void CancleHaoDuoCheSendDaKa(string ContractNO, string strToken)
        {
            string strMessage = string.Empty;
            string strJson = string.Empty;
            string url = string.Empty;
            ApiResult apiResult = new ApiResult();
            try
            {
                if (string.IsNullOrEmpty(ContractNO))
                {
                    return;
                }
                //检查是否已推送成功
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", ContractNO));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_Get_CheckOrderCoding_IsSend", list);
                DataSet ds_check = SqlHelper.GetDataSet(spe);
                if (ds_check != null && ds_check.Tables.Count > 0 && ds_check.Tables[0].Rows.Count == 0)
                {
                    return;
                }

                string strSignData = CommonClass.MD5Encrypt(strToken + "dekun");

                strJson = "{\"token\":\"" + strToken + "\",\"signData\":\"" + strSignData + "\",\"orderCoding\":\"" + ContractNO + "\"}";

                url = "http://open.hdc56.com/ApiV2/CancleCardSign";

                string ResultJson = HttpHelper.HttpPostJava(strJson, url);
                if (ResultJson.Contains("{"))
                {
                    apiResult = JsonConvert.DeserializeObject<ApiResult>(ResultJson);
                }

                if (apiResult != null)
                {
                    if (apiResult.retCode == 1)
                    {
                        //MsgBox.ShowOK("取消打卡成功！");
                        strMessage = "取消打卡成功";
                    }
                    else
                    {
                        strMessage = apiResult.desc;
                    }
                }
                else
                {
                    strMessage = ResultJson;
                }
            }
            catch (Exception ex)
            {
                strMessage = ex.Message;
            }
            try
            {
                List<SqlPara> listLog = new List<SqlPara>();
                listLog.Add(new SqlPara("Batch", ContractNO));
                listLog.Add(new SqlPara("FutionName", "CancleHaoDuoCheSendDaKa"));
                listLog.Add(new SqlPara("FaceUrl", url));
                listLog.Add(new SqlPara("FaceJson", strJson));
                listLog.Add(new SqlPara("ResultMessage", strMessage == null ? "" : strMessage));
                listLog.Add(new SqlPara("FaceState", apiResult.retCode == 1 ? "成功" : "失败"));
                SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_InterFace_Log", listLog);
                SqlHelper.ExecteNonQuery(spsLog);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 订单实时位置查询 ld20180912
        /// </summary>
        /// <param name="ContractNO">合同编号</param>
        public static DataTable GetHaoDuoCheCodeData(string ContractNO, string strToken)
        {
            DataTable dt_data = null;
            string strMessage = string.Empty;
            string strJson = string.Empty;
            string url = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(ContractNO))
                {
                    return null;
                }
                //检查是否已推送成功
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", ContractNO));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_Get_CheckOrderCoding_IsSend", list);
                DataSet ds_check = SqlHelper.GetDataSet(spe);
                if (ds_check != null && ds_check.Tables.Count > 0 && ds_check.Tables[0].Rows.Count == 0)
                {
                    return null;
                }

                ApiDataResult apiResult = null;
                ApiResult ar = null;
                string strSignData = CommonClass.MD5Encrypt(strToken + "dekun");

                strJson = "{\"token\":\"" + strToken + "\",\"signData\":\"" + strSignData + "\",\"orderCoding\":\"" + ContractNO + "\"}";

                url = "http://open.hdc56.com/ApiV2/GetPosition";
                //url = "http://hdc.open.e6gpshk.com/ApiV2/GetPosition";

                string ResultJson = HttpHelper.HttpPostJava(strJson, url);
                if (ResultJson.Contains("driverName"))
                {
                    apiResult = JsonConvert.DeserializeObject<ApiDataResult>(ResultJson);
                    dt_data = JsonConvert.DeserializeObject<DataTable>("[" + ResultJson + "]");
                }
                else if (ResultJson.Contains("{"))
                {
                    ar = JsonConvert.DeserializeObject<ApiResult>(ResultJson);
                }

                if (ar != null || apiResult != null)
                {
                    if (apiResult != null)
                    {
                        //MsgBox.ShowOK("订单位置查询成功！");
                        strMessage = "订单位置查询成功";
                    }
                    else
                    {
                        strMessage = ar.desc;
                    }
                }
                else
                {
                    strMessage = ResultJson;
                }
            }
            catch (Exception ex)
            {
                strMessage = ex.Message;
            }
            try
            {
                List<SqlPara> listLog = new List<SqlPara>();
                listLog.Add(new SqlPara("Batch", ContractNO));
                listLog.Add(new SqlPara("FutionName", "GetHaoDuoCheCodeData"));
                listLog.Add(new SqlPara("FaceUrl", url));
                listLog.Add(new SqlPara("FaceJson", strJson));
                listLog.Add(new SqlPara("ResultMessage", strMessage == null ? "" : strMessage));
                listLog.Add(new SqlPara("FaceState", strMessage == "订单位置查询成功" ? "成功" : "失败"));
                SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_InterFace_Log", listLog);
                SqlHelper.ExecteNonQuery(spsLog);
            }
            catch (Exception)
            {

            }

            return dt_data;
        }

        /// <summary>
        /// 历史轨迹查询 ld20180912
        /// </summary>
        /// <param name="ContractNO">合同编号</param>
        public static void GetHaoDuoCheTrajectory(string ContractNO, string strToken)
        {
            string strMessage = string.Empty;
            string strJson = string.Empty;
            string url = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(ContractNO))
                {
                    return;
                }
                //检查是否已推送成功
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", ContractNO));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_Get_CheckOrderCoding_IsSend", list);
                DataSet ds_check = SqlHelper.GetDataSet(spe);
                if (ds_check != null && ds_check.Tables.Count > 0 && ds_check.Tables[0].Rows.Count == 0)
                {
                    return;
                }

                string strSignData = CommonClass.MD5Encrypt(strToken + "dekun");

                url = "http://open.hdc56.com/ApiV2/GetOrderPathTrack?";

                url = string.Format("{0}token={1}&signData={2}&orderCoding={3}", url, strToken, strSignData, ContractNO);
                //调用系统默认的浏览器 
                System.Diagnostics.Process.Start(url);
                strMessage = "轨迹查询成功";
            }
            catch (Exception ex)
            {
                strMessage = ex.Message;
            }
            try
            {
                List<SqlPara> listLog = new List<SqlPara>();
                listLog.Add(new SqlPara("Batch", ContractNO));
                listLog.Add(new SqlPara("FutionName", "GetHaoDuoCheTrajectory"));
                listLog.Add(new SqlPara("FaceUrl", url));
                listLog.Add(new SqlPara("FaceJson", strJson));
                listLog.Add(new SqlPara("ResultMessage", strMessage == null ? "" : strMessage));
                listLog.Add(new SqlPara("FaceState", strMessage == "轨迹查询成功" ? "成功" : "失败"));
                SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_InterFace_Log", listLog);
                SqlHelper.ExecteNonQuery(spsLog);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 同步好多车付款状态 ld20180912
        /// </summary>
        /// <param name="ContractNO">合同编号</param>
        public static void SynHaoDuoChePaymentStatus(string ContractNOs, string FeeTypes, string strToken)
        {
            string strDomaim = string.Empty;
            HttpHelper.GetDomaim(ref strDomaim);
            if (strDomaim != "http://120.78.229.221:7060/")
            {
                return;
            }
            string strMessage = string.Empty;
            string strJson = string.Empty;
            string url = string.Empty;
            ApiResult apiResult = new ApiResult();
            StringBuilder strContractNO = new StringBuilder();
            try
            {
                if (string.IsNullOrEmpty(ContractNOs) || string.IsNullOrEmpty(FeeTypes))
                {
                    return;
                }
                //根据合同编号获取司机银行信息
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatchs", ContractNOs));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_Get_OrderCoding_By_Bank", list);
                DataSet ds_data = SqlHelper.GetDataSet(spe);
                if (ds_data == null || ds_data.Tables.Count == 0 || ds_data.Tables[0].Rows.Count == 0)
                {
                    return;
                }

                DataTable dt2 = new DataTable();
                dt2.Columns.Add("OrderCoding");
                dt2.Columns.Add("PayStep");
                dt2.Columns.Add("Name");
                dt2.Columns.Add("Phone");
                dt2.Columns.Add("OilCardNo");
                dt2.Columns.Add("BankName");
                dt2.Columns.Add("BankNo");
                dt2.Columns.Add("BranchName");
                dt2.TableName = "orderPaymentList";

                string[] strs1 = ContractNOs.Split(new char[] { '@' });
                string[] strs2 = FeeTypes.Split(new char[] { '@' });
                string feeType = string.Empty;
                for (int i = 0; i < strs1.Length; i++)
                {
                    feeType = string.Empty;
                    switch (strs2[i])
                    {
                        case "大车费现付":
                            feeType = "1";
                            break;
                        case "大车费到付":
                            feeType = "2";
                            break;
                        case "大车费回付":
                            feeType = "3";
                            break;
                        case "大车油卡费":
                            feeType = "4";
                            break;
                        case "大车油料费":
                            feeType = "6";
                            break;
                        default:
                            break;
                    }
                    if (!string.IsNullOrEmpty(feeType))
                    {
                        strContractNO.Append(strs1[i] + "@");
                        DataRow[] dr = ds_data.Tables[0].Select(" DepartureBatch ='" + strs1[i] + "'");

                        DataRow row = dt2.NewRow();
                        row["OrderCoding"] = strs1[i];
                        row["PayStep"] = feeType;
                        if (dr.Length > 0)
                        {
                            row["Phone"] = dr[0]["DriverPhone"].ToString();
                            row["OilCardNo"] = dr[0]["OilCardNo"].ToString();
                            if (strs2[i] == "大车费现付")
                            {
                                row["Name"] = dr[0]["NowPayAccontName"].ToString();
                                row["BankName"] = dr[0]["NowPayBankName"].ToString();
                                row["BankNo"] = dr[0]["NowPayAccountNO"].ToString();
                                row["BranchName"] = dr[0]["NowPayBankName"].ToString();
                            }
                            else if (strs2[i] == "大车费回付")
                            {
                                row["Name"] = dr[0]["BackPayAccontName"].ToString();
                                row["BankName"] = dr[0]["BackPayBankName"].ToString();
                                row["BankNo"] = dr[0]["BackPayAccountNO"].ToString();
                                row["BranchName"] = dr[0]["BackPayBankName"].ToString();
                            }
                            else if (strs2[i] == "大车油卡费")
                            {
                                row["Name"] = dr[0]["oilCardManName"].ToString();
                                row["BankName"] = dr[0]["oilCardBank"].ToString();
                                row["BankNo"] = dr[0]["oilCardAccount"].ToString();
                                row["BranchName"] = dr[0]["oilCardBank"].ToString();
                            }
                            else
                            {
                                row["Name"] = "";
                                row["BankName"] = "";
                                row["BankNo"] = "";
                                row["BranchName"] = "";
                            }
                        }
                        dt2.Rows.Add(row);
                    }
                }
                if (dt2.Rows.Count == 0)
                {
                    return;
                }

                string strSignData = CommonClass.MD5Encrypt(strToken + "dekun");

                string strJsonList = JsonConvert.SerializeObject(dt2).ToString();

                strJson = "{\"token\":\"" + strToken + "\",\"signData\":\"" + strSignData + "\",\"orderPaymentList\":" + strJsonList + "}";

                url = "http://open.hdc56.com/ApiV2/SynOrderPaymentStatus";
                //url = "http://hdc.open.e6gpshk.com/ApiV2/SynOrderPaymentStatus";

                string ResultJson = HttpHelper.HttpPostJava(strJson, url);
                if (ResultJson.Contains("{"))
                {
                    apiResult = JsonConvert.DeserializeObject<ApiResult>(ResultJson);
                }

                if (apiResult != null)
                {
                    if (apiResult.retCode == 1)
                    {
                        //MsgBox.ShowOK("取消打卡成功！");
                        strMessage = "同步成功";
                    }
                    else
                    {
                        strMessage = apiResult.desc;
                    }
                }
                else
                {
                    strMessage = ResultJson;
                }
            }
            catch (Exception ex)
            {
                strMessage = ex.Message;
            }
            try
            {
                List<SqlPara> listLog = new List<SqlPara>();
                listLog.Add(new SqlPara("Batch", strContractNO.ToString()));
                listLog.Add(new SqlPara("FutionName", "SynHaoDuoChePaymentStatus"));
                listLog.Add(new SqlPara("FaceUrl", url));
                listLog.Add(new SqlPara("FaceJson", strJson));
                listLog.Add(new SqlPara("ResultMessage", strMessage == null ? "" : strMessage));
                listLog.Add(new SqlPara("FaceState", apiResult.retCode == 1 ? "成功" : "失败"));
                SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_InterFace_Log", listLog);
                SqlHelper.ExecteNonQuery(spsLog);
            }
            catch (Exception)
            {

            }
        }


        /// <summary>
        /// BillRoute插入数据
        /// </summary>
        /// 
        public static void ADDBillRoute(string billno, string companyid1, string companyid2)
        {
            List<SqlPara> listAsy = new List<SqlPara>();
            listAsy.Add(new SqlPara("billno", billno));
            listAsy.Add(new SqlPara("companyid1", companyid1));
            listAsy.Add(new SqlPara("companyid2", companyid2));

            SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_ADD_BillRoute", listAsy);
            SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
        }


        /// <summary>
        /// hj 费用异动申请同步
        /// </summary>
        /// <param name="ds"></param>
        public static void BILLAPPLYYDSYN(DataSet ds)
        {
            Dictionary<string, string> dty = new Dictionary<string, string>();
            dty.Add("ApplyID", ds.Tables[0].Rows[0]["ApplyID"].ToString());
            dty.Add("BillNO", ds.Tables[0].Rows[0]["BillNO"].ToString());
            dty.Add("BillingDate", ds.Tables[0].Rows[0]["BillingDate"].ToString());
            dty.Add("ApplyContent", ds.Tables[0].Rows[0]["ApplyContent"].ToString());
            dty.Add("ApplyDate", ds.Tables[0].Rows[0]["ApplyDate"].ToString());
            dty.Add("BeginSite", ds.Tables[0].Rows[0]["BeginSite"].ToString());
            dty.Add("EndSite", ds.Tables[0].Rows[0]["EndSite"].ToString());
            dty.Add("BillingWeb", ds.Tables[0].Rows[0]["BillingWeb"].ToString());
            dty.Add("ApplyWeb", ds.Tables[0].Rows[0]["ApplyWeb"].ToString());
            dty.Add("ApplyMan", ds.Tables[0].Rows[0]["ApplyMan"].ToString());
            dty.Add("ApplyType", ds.Tables[0].Rows[0]["ApplyType"].ToString());
            dty.Add("LastState", ds.Tables[0].Rows[0]["LastState"].ToString());
            dty.Add("SqlStr", ds.Tables[0].Rows[0]["SqlStr"].ToString());
            dty.Add("AuditingDate", ds.Tables[0].Rows[0]["AuditingDate"].ToString());
            dty.Add("AuditingMan", ds.Tables[0].Rows[0]["AuditingMan"].ToString());
            dty.Add("AuditingState", ds.Tables[0].Rows[0]["AuditingState"].ToString());
            dty.Add("ApprovalMan", ds.Tables[0].Rows[0]["ApprovalMan"].ToString());
            dty.Add("ApprovalDate", ds.Tables[0].Rows[0]["ApprovalDate"].ToString());
            dty.Add("ApprovalState", ds.Tables[0].Rows[0]["ApprovalState"].ToString());
            dty.Add("ExcuteDate", ds.Tables[0].Rows[0]["ExcuteDate"].ToString());
            dty.Add("ExcuteMan", ds.Tables[0].Rows[0]["ExcuteMan"].ToString());
            dty.Add("ExcuteState", ds.Tables[0].Rows[0]["ExcuteState"].ToString());
            dty.Add("ExcuVetoSite", ds.Tables[0].Rows[0]["ExcuVetoSite"].ToString());
            dty.Add("ExcuVetoWeb", ds.Tables[0].Rows[0]["ExcuVetoWeb"].ToString());
            dty.Add("ChangeMoney", ds.Tables[0].Rows[0]["ChangeMoney"].ToString());
            dty.Add("AmountMoney", ds.Tables[0].Rows[0]["AmountMoney"].ToString());
            dty.Add("SerialNumber", ds.Tables[0].Rows[0]["SerialNumber"].ToString());
            dty.Add("ChangePlusObj", ds.Tables[0].Rows[0]["ChangePlusObj"].ToString());
            dty.Add("unFetchCharge", ds.Tables[0].Rows[0]["unFetchCharge"].ToString());
            dty.Add("RecWeb", ds.Tables[0].Rows[0]["RecWeb"].ToString());
            string json = JsonConvert.SerializeObject(dty);
            List<SqlPara> listAsy = new List<SqlPara>();
            listAsy.Add(new SqlPara("FuntionName", "USP_ADDBIllAppLYYD_SYN_ZQTMS"));
            listAsy.Add(new SqlPara("FaceUrl", ""));
            listAsy.Add(new SqlPara("FaceJson", json));
            listAsy.Add(new SqlPara("BillNos", ds.Tables[0].Rows[0]["BillNO"].ToString() + ","));
            listAsy.Add(new SqlPara("Batch", ""));
            listAsy.Add(new SqlPara("NodeName", "费用异动申请"));
            listAsy.Add(new SqlPara("SystemSource", "TMS"));
            listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));
            SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
            SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);

        }

        /// <summary>
        /// hj20190425 改单同步新
        /// </summary>
        /// <param name="ds"></param>
        public static void BILLAPPLYSYN(DataSet ds, string type)
        {
            Dictionary<string, string> dty = new Dictionary<string, string>();
            if (type == "审核")
            {
                dty.Add("Type", "审核");
                dty.Add("ApplyID", ds.Tables[0].Rows[0]["ApplyID"].ToString());
                dty.Add("AuditingState", ds.Tables[0].Rows[0]["AuditingState"].ToString());
                dty.Add("AuditingMan", ds.Tables[0].Rows[0]["AuditingMan"].ToString());
                dty.Add("AuditingDate", ds.Tables[0].Rows[0]["AuditingDate"].ToString());
                dty.Add("LastState", ds.Tables[0].Rows[0]["LastState"].ToString());
                dty.Add("AuditingRemark", ds.Tables[0].Rows[0]["AuditingRemark"].ToString());
            }
            else if (type == "审批")
            {
                dty.Add("Type", "审批");
                dty.Add("ApplyID", ds.Tables[0].Rows[0]["ApplyID"].ToString());
                dty.Add("ApprovalState", ds.Tables[0].Rows[0]["ApprovalState"].ToString());
                dty.Add("ApprovalMan", ds.Tables[0].Rows[0]["ApprovalMan"].ToString());
                dty.Add("ApprovalDate", ds.Tables[0].Rows[0]["ApprovalDate"].ToString());
                dty.Add("LastState", ds.Tables[0].Rows[0]["LastState"].ToString());
                dty.Add("ApprovalRemark", ds.Tables[0].Rows[0]["ApprovalRemark"].ToString());
            }
            else if (type == "取消")
            {
                dty.Add("Type", "取消");
                dty.Add("ApplyID", ds.Tables[0].Rows[0]["ApplyID"].ToString());
                dty.Add("CancelMan", ds.Tables[0].Rows[0]["CancelMan"].ToString());
                dty.Add("CancelDate", ds.Tables[0].Rows[0]["CancelDate"].ToString());
                dty.Add("LastState", ds.Tables[0].Rows[0]["LastState"].ToString());
            }
            else if (type == "否决")
            {
                dty.Add("Type", "否决");
                dty.Add("ApplyID", ds.Tables[0].Rows[0]["ApplyID"].ToString());
                dty.Add("VetoMan", ds.Tables[0].Rows[0]["VetoMan"].ToString());
                dty.Add("VetoDate", ds.Tables[0].Rows[0]["VetoDate"].ToString());
                dty.Add("LastState", ds.Tables[0].Rows[0]["LastState"].ToString());
                dty.Add("VetoRemark", ds.Tables[0].Rows[0]["VetoRemark"].ToString());
            }

            string json = JsonConvert.SerializeObject(dty);
            List<SqlPara> listAsy = new List<SqlPara>();
            listAsy.Add(new SqlPara("FuntionName", "USP_BillApplys_SYN"));
            listAsy.Add(new SqlPara("FaceUrl", ""));
            listAsy.Add(new SqlPara("FaceJson", json));
            listAsy.Add(new SqlPara("BillNos", ds.Tables[0].Rows[0]["BillNO"].ToString() + ","));
            listAsy.Add(new SqlPara("Batch", ""));
            if (type == "审核")
            {
                listAsy.Add(new SqlPara("NodeName", "改单审核"));
            }
            else if (type == "审批")
            {
                listAsy.Add(new SqlPara("NodeName", "改单审批"));
            }
            else if (type == "取消")
            {
                listAsy.Add(new SqlPara("NodeName", "改单取消"));
            }
            else if (type == "否决")
            {
                listAsy.Add(new SqlPara("NodeName", "改单否决"));
            }
            listAsy.Add(new SqlPara("SystemSource", "TMS"));
            listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));
            SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
            SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
        }

        /// <summary>
        /// hj 费用异动取消,否决同步
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="type"></param>
        public static void BILLAPPLYYExSYN(DataSet ds, string type)
        {
            Dictionary<string, string> dty = new Dictionary<string, string>();
            if (type == "取消")
            {
                dty.Add("Type", "取消");
                dty.Add("ApplyID", ds.Tables[0].Rows[0]["ApplyID"].ToString());
                dty.Add("CancelMan", ds.Tables[0].Rows[0]["CancelMan"].ToString());
                dty.Add("CancelDate", ds.Tables[0].Rows[0]["CancelDate"].ToString());
                dty.Add("LastState", ds.Tables[0].Rows[0]["LastState"].ToString());
            }
            else if (type == "否决")
            {
                dty.Add("Type", "否决");
                dty.Add("ApplyID", ds.Tables[0].Rows[0]["ApplyID"].ToString());
                dty.Add("VetoMan", ds.Tables[0].Rows[0]["VetoMan"].ToString());
                dty.Add("VetoDate", ds.Tables[0].Rows[0]["VetoDate"].ToString());
                dty.Add("LastState", ds.Tables[0].Rows[0]["LastState"].ToString());
            }
            string json = JsonConvert.SerializeObject(dty);
            List<SqlPara> listAsy = new List<SqlPara>();
            listAsy.Add(new SqlPara("FuntionName", "USP_EXITBILLAPPLYYD_SYN_ZQTMS"));
            listAsy.Add(new SqlPara("FaceUrl", ""));
            listAsy.Add(new SqlPara("FaceJson", json));
            listAsy.Add(new SqlPara("BillNos", ds.Tables[0].Rows[0]["BillNO"].ToString() + ","));
            listAsy.Add(new SqlPara("Batch", ""));

            if (type == "取消")
            {
                listAsy.Add(new SqlPara("NodeName", "取消费用异动"));
            }
            else if (type == "否决")
            {
                listAsy.Add(new SqlPara("NodeName", "否决费用异动"));
            }
            listAsy.Add(new SqlPara("SystemSource", "TMS"));
            listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));
            SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
            SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);
        }

        /// <summary>
        /// zb 网点线路议价申请同步
        /// </summary>
        /// <param name="ds"></param>
        public static void BargainAPPLYYDSYN(DataSet ds)
        {
            Dictionary<string, string> dty = new Dictionary<string, string>();
            dty.Add("priceId", ds.Tables[0].Rows[0]["priceId"].ToString());
            dty.Add("BillNo", ds.Tables[0].Rows[0]["BillNo"].ToString());
            dty.Add("siteName", ds.Tables[0].Rows[0]["BillSite"].ToString());
            dty.Add("webName", ds.Tables[0].Rows[0]["BillWeb"].ToString());
            dty.Add("BillState", ds.Tables[0].Rows[0]["BillState"].ToString());
            dty.Add("mainLinefee", ds.Tables[0].Rows[0]["mainLineFee"].ToString());
            dty.Add("perMainLine", ds.Tables[0].Rows[0]["mainLinePer"].ToString());
            dty.Add("newMainLinefee", ds.Tables[0].Rows[0]["newMainLinefee"].ToString());
            dty.Add("Varieties", ds.Tables[0].Rows[0]["Varieties"].ToString());
            dty.Add("Num", ds.Tables[0].Rows[0]["Num"].ToString());
            dty.Add("TransferSite", ds.Tables[0].Rows[0]["TransferSite"].ToString());
            dty.Add("TransitMode", ds.Tables[0].Rows[0]["TransitMode"].ToString());
            dty.Add("TransitLines", ds.Tables[0].Rows[0]["TransitLines"].ToString());
            dty.Add("actualFreight", ds.Tables[0].Rows[0]["ActualFreight"].ToString());
            dty.Add("ApplyMan", ds.Tables[0].Rows[0]["ApplyMan"].ToString());
            dty.Add("ApplyDate", ds.Tables[0].Rows[0]["ApplyDate"].ToString());
            dty.Add("ApplyWebName", ds.Tables[0].Rows[0]["ApplyWebName"].ToString());
            dty.Add("OperationWeight", ds.Tables[0].Rows[0]["OperationWeight"].ToString());
            dty.Add("BargainingMark", ds.Tables[0].Rows[0]["BargainingMark"].ToString());
            dty.Add("Acompanyid", ds.Tables[0].Rows[0]["companyid"].ToString());
            dty.Add("DeliveryFee", ds.Tables[0].Rows[0]["DeliveryFee"].ToString());
            dty.Add("NewDeliveryFee", ds.Tables[0].Rows[0]["NewDeliveryFee"].ToString());
            dty.Add("DeliveryFeePer", ds.Tables[0].Rows[0]["DeliveryFeePer"].ToString());
            dty.Add("AcceptCompany", ds.Tables[0].Rows[0]["AcceptCompany"].ToString());
        
      
            string json = JsonConvert.SerializeObject(dty);
            List<SqlPara> listAsy = new List<SqlPara>();
            listAsy.Add(new SqlPara("FuntionName", "USP_ADDBargainAppLYYD_SYN_ZQTMS"));
            listAsy.Add(new SqlPara("FaceUrl", ""));
            listAsy.Add(new SqlPara("FaceJson", json));
            listAsy.Add(new SqlPara("BillNos", ds.Tables[0].Rows[0]["BillNO"].ToString() + ","));
            listAsy.Add(new SqlPara("Batch", ""));
            listAsy.Add(new SqlPara("NodeName", "网点线路议价申请"));
            listAsy.Add(new SqlPara("SystemSource", "TMS"));
            listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));
            SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
            SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);

        }
        /// <summary>
        /// zb 取消网点线路议价申请同步
        /// </summary>
        /// <param name="ds"></param>
        public static void DelBargainAPPLYYDSYN(string id,string billno)
        {
            Dictionary<string, string> dty = new Dictionary<string, string>();
            dty.Add("priceId",id);

            string json = JsonConvert.SerializeObject(dty);
            List<SqlPara> listAsy = new List<SqlPara>();
            listAsy.Add(new SqlPara("FuntionName", "USP_DelBargainAppLYYD_SYN_ZQTMS"));
            listAsy.Add(new SqlPara("FaceUrl", ""));
            listAsy.Add(new SqlPara("FaceJson", json));
            listAsy.Add(new SqlPara("BillNos",billno));
            listAsy.Add(new SqlPara("Batch", ""));
            listAsy.Add(new SqlPara("NodeName", "网点线路议价申请"));
            listAsy.Add(new SqlPara("SystemSource", "TMS"));
            listAsy.Add(new SqlPara("companyid", CommonClass.UserInfo.companyid));
            SqlParasEntity spsAsy = new SqlParasEntity(OperType.Execute, "USP_SynFBState", listAsy);
            SqlHelper.ExecteNonQuery_ZQTMS(spsAsy);

        } 
    }
}
