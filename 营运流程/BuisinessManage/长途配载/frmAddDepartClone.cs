using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using DevExpress.XtraEditors;
using ZQTMS.Tool;

namespace ZQTMS.UI
{
    public partial class frmAddDepartClone : BaseForm
    {
        public string siteOne = "";
        public string siteTwo = "";
        public string siteThree = "";
        public string batch = "";
        public string loadType = "";
        string strWebs = "";
        public string LoadWeb = "";
        public string LongSource = "";
        public string carType = "";
        public string firstweb = "";
        public string secondweb = "";
        public string thirdweb = "";
        public string loadSite = "";
        public string beginSite = "";
        public string carSoure = "";
        public string carNO = "";
        public string carrNO = "";
        public string driverName = "";
        public string driverPhone = "";
        public string contractNO = "";
        private DataSet dssj = new DataSet();//加载运行时效
        private DataSet dslongsx = new DataSet();//加载长途时效
        private DataSet bzDateTime = new DataSet();//加载长途时效
        public string strdate = "";//标准发车时间
        public DateTime departureDate ;//财务发运时间
        //public string isPhone = string.Empty;//发车验证手机号
        public string strPeiZaiType = string.Empty;//配载类型
        public frmAddDepartClone()
        {
            InitializeComponent();
        } 


        private void btnSave_Click(object sender, EventArgs e)
        {
            //string delayReson = "";
            //if (dateoper.DateTime > dateLast.DateTime)
            //{
                //if (cbbReson.Text.Trim() == "")
                //{
                //    MsgBox.ShowOK("延误原因必填！");
                //    return;
                //}
                //else
                //{
                //    if (cbbReson.Text.Trim() == "其他原因")
                //    {
                //        if (txtOtherReson.Text.Trim() == "")
                //        {
                //            MsgBox.ShowOK("延误原因必填！");
                //            return;
                //        }
                //        else
                //        {
                //            delayReson = txtOtherReson.Text.Trim();
                //        }
                //    }
                //    else
                //    {
                //        delayReson = cbbReson.Text.Trim();
                //    }
                //}
            //}
            //else
            //{
            //    delayReson = "";

            //}

            try
            {
                string carStartType = "";
                if (!string.IsNullOrEmpty(destiSite3.Text.Trim()))
                {
                    carStartType = "三地车";
                }
                if (!string.IsNullOrEmpty(destiSite2.Text.Trim()) && !string.IsNullOrEmpty(destiSite1.Text.Trim()) && string.IsNullOrEmpty(destiSite3.Text.Trim()))
                {
                    carStartType = "二地车";
                }
                if (string.IsNullOrEmpty(destiSite3.Text.Trim()) && string.IsNullOrEmpty(destiSite2.Text.Trim()))
                {
                    carStartType = "一地车";
                }
                if (string.IsNullOrEmpty(destiSite1.Text.Trim()))
                {
                    MsgBox.ShowError("①目的站点不能为空！");
                    return;
                }
                if (string.IsNullOrEmpty(desitiWeb1.Text.Trim()))
                {
                    MsgBox.ShowError("①目的网点不能为空！");
                    return;
                }
                if (!string.IsNullOrEmpty(destiSite2.Text.Trim()))
                {
                    if (string.IsNullOrEmpty(desitiWeb2.Text.Trim()))
                    {
                        MsgBox.ShowError("②目的网点不能为空！");
                        return;
                    }
                }
                if (!string.IsNullOrEmpty(destiSite3.Text.Trim()))
                {
                    if (string.IsNullOrEmpty(desitiWeb2.Text.Trim()))
                    {
                        MsgBox.ShowError("③目的网点不能为空！");
                        return;
                    }
                }
                //if (!flag)
                //{
                //    MsgBox.ShowOK(msg1 + "\r\n" + msg2 + "\r\n" + msg3 + "\r\n，请先联系运管陈晓业(18025371662)完善运行时效表！");
                //    return;
                //}
                List<SqlPara> list = new List<SqlPara>();
               // list.Add(new SqlPara("ID", Guid.NewGuid()));
                list.Add(new SqlPara("destiSite1", destiSite1.Text.Trim()));
                list.Add(new SqlPara("desitiWeb1", desitiWeb1.Text.Trim()));
                list.Add(new SqlPara("predictTime1", predictTime1));
                list.Add(new SqlPara("destiSite2", destiSite2.Text.Trim()));
                list.Add(new SqlPara("desitiWeb2", desitiWeb2.Text.Trim()));
                list.Add(new SqlPara("predictTime2", predictTime2));
                list.Add(new SqlPara("destiSite3", destiSite3.Text.Trim()));
                list.Add(new SqlPara("desitiWeb3", desitiWeb3.Text.Trim()));
                list.Add(new SqlPara("predictTime3", predictTime3));
                //list.Add(new SqlPara("dateoper", dateoper.DateTime));
                //list.Add(new SqlPara("dateLast", dateLast.DateTime));               
                list.Add(new SqlPara("batchNo", batch));
                list.Add(new SqlPara("carStartType", carStartType));
                //list.Add(new SqlPara("DelayReson", delayReson));
                list.Add(new SqlPara("LoadWeb", LoadWeb));
                //list.Add(new SqlPara("SendCarPhone", isPhone));//发车验证手机号
                list.Add(new SqlPara("ContractNO", ContractNO.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLVEHICLESTAR_Merge_KT", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    //isPhone = string.Empty;

                    //提前获取到轨迹信息
                    List<SqlPara> lists = new List<SqlPara>();
                    lists.Add(new SqlPara("DepartureBatch", batch));
                    lists.Add(new SqlPara("BillNO", null));
                    lists.Add(new SqlPara("tracetype", "车辆出发"));
                    //lists.Add(new SqlPara("num", "6,5,"));//查询配载发车和车辆出发两条记录lhd
                    lists.Add(new SqlPara("num", 6));
                    DataSet dss = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_TraceSyn", lists));

                    if (strPeiZaiType == "ZQTMS" && dss != null && dss.Tables.Count > 0)
                    {
                        int i = dss.Tables.Count - 1;
                        DataRow[] dr1 = dss.Tables[i].Select(" ArriveSite = '" + destiSite1.Text.Trim() + "' and ArriveWeb = '" + desitiWeb1.Text.Trim() + "'");
                        if (dr1.Length > 0)
                        {
                            list.Add(new SqlPara("id1", dr1[0]["ID"].ToString()));
                        }
                        DataRow[] dr2 = dss.Tables[i].Select(" ArriveSite = '" + destiSite2.Text.Trim() + "' and ArriveWeb = '" + desitiWeb2.Text.Trim() + "'");
                        if (dr2.Length > 0)
                        {
                            list.Add(new SqlPara("id2", dr2[0]["ID"].ToString()));
                        }
                        DataRow[] dr3 = dss.Tables[i].Select(" ArriveSite = '" + destiSite3.Text.Trim() + "' and ArriveWeb = '" + desitiWeb3.Text.Trim() + "'");
                        if (dr3.Length > 0)
                        {
                            list.Add(new SqlPara("id3", dr3[0]["ID"].ToString()));
                        }

                        CommonSyn.LMSDepartureSysZQTMS(list, 2, "USP_ADD_BILLVEHICLESTAR_Merge_LMS", "", batch, CommonClass.UserInfo.companyid);//LMS配载同步ZQTMS（完成发车）                        
                    }
                    else
                    {
                        CommonSyn.TraceSyn(null, null, 6, "车辆出发", 1, "车辆出发", dss);
                    }
                    frmDeparture frm = this.Owner as frmDeparture;
                    if (frm != null)
                    {
                        frm.isbl = true;//修改发车是否成功标识
                    }
                    MsgBox.ShowOK();
                    this.Close();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

        private void frmAddDepartClone_Load(object sender, EventArgs e)
        {
            dateoper.Enabled = true;
            //dateLast.DateTime = CommonClass.gcdate;
            dateoper.DateTime = CommonClass.gcdate;
           // predictTime1.DateTime = CommonClass.gcdate;
           // predictTime2.DateTime = CommonClass.gcdate;
           // predictTime3.DateTime = CommonClass.gcdate;
            destiSite1.Text = siteOne;
            destiSite2.Text = siteTwo;
            destiSite3.Text = siteThree;
   
            desitiWeb1.Text = firstweb;
            desitiWeb2.Text = secondweb;
            desitiWeb3.Text = thirdweb;

            destiSite1.Enabled = false;
            destiSite2.Enabled = false;
            destiSite3.Enabled = false;
            desitiWeb1.Enabled = false;
            desitiWeb2.Enabled = false;
            desitiWeb3.Enabled = false;
             //CommonClass.SetSite(destiSite1, false);
             //CommonClass.SetSite(destiSite2, false);
             //CommonClass.SetSite(destiSite3, false);
             //strWebs = GetSites();
             //if (destiSite1.Properties.Items.Contains("嘉定"))
             //    destiSite1.Properties.Items.Remove("嘉定");

             //if (destiSite2.Properties.Items.Contains("嘉定"))
             //    destiSite2.Properties.Items.Remove("嘉定");

             //if (destiSite3.Properties.Items.Contains("嘉定"))
             //    destiSite3.Properties.Items.Remove("嘉定");

             //if (destiSite1.Properties.Items.Contains("永康"))
             //    destiSite1.Properties.Items.Remove("永康");

             //if (destiSite2.Properties.Items.Contains("永康"))
             //    destiSite2.Properties.Items.Remove("永康");

             //if (destiSite3.Properties.Items.Contains("永康"))
             //    destiSite3.Properties.Items.Remove("永康");


             if (!string.IsNullOrEmpty(loadType) && !loadType.Equals("整车配载"))
             {
                 if (destiSite1.Properties.Items.Contains("客户处"))
                 {
                     destiSite1.Properties.Items.Remove("客户处");
                 }
                 if (destiSite2.Properties.Items.Contains("客户处"))
                 {
                     destiSite2.Properties.Items.Remove("客户处");
                 }
                 if (destiSite3.Properties.Items.Contains("客户处"))
                 {
                     destiSite3.Properties.Items.Remove("客户处");
                 }
             }
 
           
          
             GetRunTime();
             //if (siteThree == "")
             //{
             //    lbldestiSite3.Visible = false;
             //    lbldesitiWeb3.Visible = false;
             //   // lblpredictTime3.Visible = false;
             //    destiSite3.Visible = false;
             //    desitiWeb3.Visible = false;
             //   // predictTime3.Visible = false;
             //}
            //初始化最晚发车时间
             try
             {
                 //DataRow[] drs = dslongsx.Tables[0].Select("bsite='" + LoadWeb + "' and esite='" + firstweb + "' and LongModels='" + carType + "' and LongSource='" + LongSource + "'");
               /*  DataRow[] drs = dslongsx.Tables[0].Select("bsite='" + LoadWeb + "' and esite='" + firstweb + "'");
                 if (drs.Length <= 0)
                 {
                     dateLast.DateTime = DateTime.Now;
                 }
                 else
                 {
                     string dateStandard = DateTime.Parse(drs[0]["godate"].ToString()).ToString("hh:mm:ss");
                     string nowTime = DateTime.Now.ToString("yyyy-MM-dd");
                     dateLast.DateTime = DateTime.Parse(nowTime + " " + dateStandard);
                 }
                * */
                 DataRow[] dr = dssj.Tables[0].Select("EndSite='" + siteOne + "' and EndWeb='" + firstweb + "'" + " and StartSite='" + beginSite + "'" , "Shift desc ");
                 if (dr.Length <= 0)
                 {
                     // msg1 = "无法确定发车网点【" + startWeb + "】到目的网点①【" + endWeb + "】的预计到达时间!";
                     predictTime1 = DateTime.Now;
                     //dateLast.DateTime = DateTime.Now;
                     //flag = false;
                     // return;
                 }
                 else
                 {
                    string time= dr[0]["StandardDepartureTime"].ToString();
                    string hours = time.Substring(0, time.LastIndexOf(":"));
                    string minutes = time.Substring(time.LastIndexOf(":")+1);
                    int runHours = int.Parse(hours);
                    int runMinutes = int.Parse(minutes);
                    DateTime departureDate1 = Convert.ToDateTime(departureDate.ToShortDateString());
                     //yzw 0:00时，日期+1
                    if (time == "0:00")
                    {
                      
                        //predictTime1 = CommonClass.gbdate.AddDays(+1).AddHours(runHours).AddMinutes(runMinutes);
                        predictTime1 = departureDate1.AddDays(+1).AddHours(runHours).AddMinutes(runMinutes);
                    }
                    else
                    {
                        predictTime1 = departureDate1.AddHours(runHours).AddMinutes(runMinutes);
                    }
                  
                     //dateLast.DateTime = predictTime1;
                     flag = true;
                 }
             }
             catch (Exception ex)
             {
                 
                 throw ex;
             }

             ContractNO.Text = contractNO;
             DepartureBatch.Text = batch;
             CarSoure.Text = carSoure;
             CarNO.Text = carNO;
             CarrNO.Text = carrNO;
             DriverName.Text = driverName;
             DriverPhone.Text = driverPhone;
        }

        /// <summary>
        /// 获取所有运行时效
        /// </summary>
        private void GetRunTime()
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_gx_yxsjb_ALL");
                dssj = SqlHelper.GetDataSet(sps);
                SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_basLongDate");
                dslongsx = SqlHelper.GetDataSet(sps1);

            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        /// <summary>
        /// 获取标准发车时间
        /// </summary>
        private void GetRunTime2()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SendSite", siteOne));
                list.Add(new SqlPara("SendWeb", firstweb));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_TurnTwoTime", list);
                bzDateTime = SqlHelper.GetDataSet(sps);
                strdate = bzDateTime.Tables[0].Rows[0]["RunStandardTime"].ToString();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        private string GetSites()
        {
            try
            {
                //获取所有指定的目的网点
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "USP_sysRoleDataInfo_All");
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    XtraMessageBox.Show("没有任何指定目的网点", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }
                DataRow[] dr = ds.Tables[0].Select(string.Format("GRCode='{0}'", 205));
                if (dr == null || dr.Length == 0)
                {
                    XtraMessageBox.Show("没有找到任何指定目的网点", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }
                return dr[0]["SiteNames"].ToString();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

         private void destiSite1_TextChanged(object sender, EventArgs e)
          {
             /* strWebs = GetSites();
              desitiWeb1.Properties.Items.Clear();
              desitiWeb1.Text = "";
              string[] site = destiSite1.Text.Trim().Split(',');
              ComboBoxEdit cmb1 = new ComboBoxEdit();
              for (int i = 0; i < site.Length; i++)
              {
                  DataRow[] dr = CommonClass.dsWeb.Tables[0].Select("SiteName='" + site[i] + "'");
                  for (int k = 0; k < dr.Length; k++)
                  {
                      //desitiWeb1.Properties.Items.Add(dr[k]["WebName"]);
                      cmb1.Properties.Items.Add(dr[k]["WebName"]);
                  }
              }
              if (cmb1.Properties.Items.Count > 0)
              {
                  foreach (var item in cmb1.Properties.Items)
                  {
                      if (strWebs.Contains(item.ToString()))
                      {
                          desitiWeb1.Properties.Items.Add(item);
                      }
                  }
              }
              else
              {
                  desitiWeb1.Text = destiSite1.Text.Trim();
              }*/
          }

          private void destiSite2_TextChanged(object sender, EventArgs e)
          {
              /*strWebs = GetSites();
              desitiWeb2.Properties.Items.Clear();
              desitiWeb2.Text = "";
              string[] site = destiSite2.Text.Trim().Split(',');
              ComboBoxEdit cmb1 = new ComboBoxEdit();
              for (int i = 0; i < site.Length; i++)
              {
                  DataRow[] dr = CommonClass.dsWeb.Tables[0].Select("SiteName='" + site[i] + "'");
                  for (int k = 0; k < dr.Length; k++)
                  {
                      //desitiWeb2.Properties.Items.Add(dr[k]["WebName"]);
                      cmb1.Properties.Items.Add(dr[k]["WebName"]);
                  }
              }
              if (cmb1.Properties.Items.Count > 0)
              {
                  foreach (var item in cmb1.Properties.Items)
                  {
                      if (strWebs.Contains(item.ToString()))
                      {
                          desitiWeb2.Properties.Items.Add(item);
                      }
                  }
              }
              else
              {
                  desitiWeb2.Text = destiSite2.Text.Trim();
              }*/
          }

          private void destiSite3_TextChanged(object sender, EventArgs e)
          {
             /* strWebs = GetSites();
              desitiWeb3.Properties.Items.Clear();
              desitiWeb3.Text = "";
              string[] site = destiSite3.Text.Trim().Split(',');
              ComboBoxEdit cmb1 = new ComboBoxEdit();
              for (int i = 0; i < site.Length; i++)
              {
                  DataRow[] dr = CommonClass.dsWeb.Tables[0].Select("SiteName='" + site[i] + "'");
                  for (int k = 0; k < dr.Length; k++)
                  {
                      //desitiWeb3.Properties.Items.Add(dr[k]["WebName"]);
                      cmb1.Properties.Items.Add(dr[k]["WebName"]);
                  }
              }
              if (cmb1.Properties.Items.Count > 0)
              {
                  foreach (var item in cmb1.Properties.Items)
                  {
                      if (strWebs.Contains(item.ToString()))
                      {
                          desitiWeb3.Properties.Items.Add(item);
                      }
                  }
              }
              else
              {
                  desitiWeb3.Text = destiSite3.Text.Trim();
              }*/
          }

        public string msg1 = "",msg2="",msg3="";
        public DateTime predictTime1=DateTime.Now, predictTime2=DateTime.Now, predictTime3=DateTime.Now;
        public bool flag = true;
        private void desitiWeb1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string startWeb = LoadWeb;
            string endWeb = desitiWeb1.Text.Trim();
            string endSite=destiSite1.Text.Trim();
            /*
            try
            {
                //DataRow[] drs = dslongsx.Tables[0].Select("bsite='" + LoadWeb + "' and esite='" + firstweb + "' and LongModels='" + carType + "' and LongSource='" + LongSource + "'");
                DataRow[] drs = dslongsx.Tables[0].Select("bsite='" + LoadWeb + "' and esite='" + endWeb + "'");
                if (drs.Length <= 0)
                {
                    dateLast.DateTime = DateTime.Now;
                }
                else
                {
                    string dateStandard = DateTime.Parse(drs[0]["godate"].ToString()).ToString("hh:mm:ss");
                    string nowTime = DateTime.Now.ToString("yyyy-MM-dd");
                    dateLast.DateTime = DateTime.Parse(nowTime + " " + dateStandard);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
             * */

            DataRow[] dr = dssj.Tables[0].Select("StartSite='" + loadSite + "' and EndSite='" + endSite + "'");
            if (dr.Length <= 0)
            {
               // msg1 = "无法确定发车网点【" + startWeb + "】到目的网点①【" + endWeb + "】的预计到达时间!";
                predictTime1 = DateTime.Now;
                //dateLast.DateTime = DateTime.Now;
                //flag = false;
                // return;
            }
            else
            {
                int runTime = int.Parse(dr[0]["runtime"].ToString());
                predictTime1 = DateTime.Now.AddHours(runTime);
                //dateLast.DateTime = predictTime1;
                flag = true;

            }
            
        }

        private void desitiWeb2_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
           string startWeb = LoadWeb;
            string endSite = destiSite2.Text.Trim();
            DataRow[] dr = dssj.Tables[0].Select("StartSite='" + loadSite + "' and EndSite='" + endSite + "'");
            if (dr.Length <= 0)
            {
               // msg2 = "无法确定发车网点【" + startWeb + "】到目的网点②【" + endWeb + "】的预计到达时间!";
                predictTime2 = DateTime.Now;
               // flag = false;
                // return;
            }
            else
            {
                int runTime = int.Parse(dr[0]["runtime"].ToString());
                predictTime2 = DateTime.Now.AddHours(runTime);
                flag = true;
            }
             */
        }

        private void desitiWeb3_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            string startWeb = LoadWeb;
            string endSite = destiSite3.Text.Trim();
            DataRow[] dr = dssj.Tables[0].Select("StartSite='" + loadSite + "' and EndSite='" + endSite + "'");
            if (dr.Length <= 0)
            {
                //msg3 = "无法确定发车网点【" + startWeb + "】到目的网点③【" + endWeb + "】的预计到达时间!";
                predictTime3 = DateTime.Now;
               // flag = false;
                //return;
            }
            else
            {
                int runTime = int.Parse(dr[0]["runtime"].ToString());
                predictTime3 = DateTime.Now.AddHours(runTime);
                flag = true;
            }
             * */
        }

        //private void cbbReson_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (cbbReson.Text == "其他原因")
        //    {
        //        lblOtherReson.Visible = true;
        //        txtOtherReson.Visible = true;
        //    }
        //    else
        //    {
        //        lblOtherReson.Visible = false;
        //        txtOtherReson.Visible = false;
        //        txtOtherReson.Text = "";

        //    }
        //}

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
