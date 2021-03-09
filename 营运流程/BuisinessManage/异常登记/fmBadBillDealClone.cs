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
    public partial class fmBadBillDealClone : BaseForm
    {

        public string crrBillNO = "";
        public string billNo = "";
        public string billNos = "";
        public string paths = "";

        public fmBadBillDealClone()
        {
            InitializeComponent();
            
        }

        public int look = 0;
        public string took = "";
        private string ID = "";
        private string abnormalityState = "";
        private string spbSureSubmiter1 = "";
        private string spbSureSubmiter2 = "";
        private string spbSureSubmiter3 = "";

        public GridView gv = null;

        private void fmBadBillDeal_Load(object sender, EventArgs e)
        {
           
            string siteName = CommonClass.UserInfo.SiteName;
            fangkuibm.Text = CommonClass.UserInfo.WebName;
            fangkuiman2.Text = CommonClass.UserInfo.UserName;
            fangkuibm.Text = CommonClass.UserInfo.WebName;
            if (look == 10)
            {
                simpleButton1.Visible = false;
            }

            if (look == 11)
            {
                simpleButton1.Visible = true;
            }

            if (CommonClass.UserInfo.WebName.Contains("品质管理部"))
            {
                simpleButton2.Enabled = true;
                simpleButton9.Enabled = false;
                simpleButton10.Enabled = true;
                simpleButton11.Enabled = true;
                simpleButton4.Enabled = true;
                tijiaofankui.Enabled = true;
                simpleButton15.Enabled = false;
                simpleButton17.Enabled = true;
                simpleButton16.Enabled = true;
                simpleButton18.Enabled = false;
                simpleButton20.Enabled = true;
                simpleButton19.Enabled = true;
                simpleButton7.Enabled = true;
                simpleButton3.Enabled = true;
                simpleButton5.Enabled = true;
                simpleButton6.Enabled = true;
                simpleButton12.Enabled = true;
                simpleButton13.Enabled = true;
                simpleButton22.Enabled = true;
                simpleButton21.Enabled = true;
                simpleButton24.Enabled = true;
                simpleButton23.Enabled = true;
                simpleButton14.Enabled = true;
                simpleButton25.Enabled = true;

                badtype.Enabled = true;
                badtypeTwo.Enabled = true;
                ngePerCapitaEnergizing.Enabled = true;
            }

            if (gv != null)
            {
                billno.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "BillNo").ToString();
                billNo = billno.Text;
                baddate.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "baddate").ToString();
                badcontent.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "badcontent").ToString();
                EXTType.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "badtype").ToString();//zxw 异常类型
                //EXTDepart.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "badcreateweb").ToString();//zxw 异常产生部门
                //EXTDepart.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "registerDeprtment").ToString();//zxw 异常产生部门
                //badchulidate.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "badchulidate").ToString();
                //badchuliyijian.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "badchuliyijian").ToString();
                //badtype.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "badtype").ToString();
                //int hfstate_s = Convert.ToInt32(gv.GetRowCellValue(gv.FocusedRowHandle, "hfstate").ToString() == "" ? "0" : gv.GetRowCellValue(gv.FocusedRowHandle, "hfstate").ToString());

                ID = gv.GetRowCellValue(gv.FocusedRowHandle, "ID").ToString() == "" ? Guid.NewGuid().ToString() : gv.GetRowCellValue(gv.FocusedRowHandle, "ID").ToString();
            }


            //获取异常表数据wwb2017.9.15
            getData();
            

            //成本提取
            CommonClass.SetWeb(CostDepartment,false);

            //接受反馈
            CommonClass.SetWeb(zerenwebid11, false);
            CommonClass.SetWeb(zerenwebid22, false);
            CommonClass.SetWeb(zerenwebid33, false);

            //货差买赔确认
            CommonClass.SetWeb(buyToCompensateDep1, false);
            CommonClass.SetWeb(buyToCompensateDep2, false);
            CommonClass.SetWeb(buyToCompensateDep3, false);

           //获取数据
            setdata();
           

            if (zerenwebid1.Text == CommonClass.UserInfo.WebName  )
            {
                if (abnormalityState == "待处理" )
                {
                    simpleButton9.Enabled = true;
                }
                if (abnormalityState == "已处理" || abnormalityState == "已仲裁")
                {
                    if (spbSureSubmiter1 == "")
                    {
                        simpleButton10.Enabled = true;
                    }
                }
                if (took == "1")
                {
                    simpleButton11.Enabled = true;
                }
                if (abnormalityState == "待处理")
                {
                    simpleButton4.Enabled = true;
                    tijiaofankui.Enabled = true;
                }
                //spbSurePeople1.Text = CommonClass.UserInfo.UserName;
                
            }
            if (zerenwebid2.Text == CommonClass.UserInfo.WebName)
            {
                if (abnormalityState == "待处理" )
                {
                    simpleButton15.Enabled = true;
                }
                if (abnormalityState == "已处理" || abnormalityState == "已仲裁")
                {
                    if (spbSureSubmiter2 == "")
                    {
                        simpleButton17.Enabled = true;
                    }
                }
                if (took == "2")
                {
                    simpleButton16.Enabled = true;
                }
                //spbSurePeople2.Text = CommonClass.UserInfo.UserName;
            }
            if (zerenwebid3.Text == CommonClass.UserInfo.WebName)
            {
                if (abnormalityState == "待处理")
                {
                    simpleButton18.Enabled = true;
                }
                if (abnormalityState == "已处理" || abnormalityState == "已仲裁")
                {
                    if (spbSureSubmiter3 == "")
                    {
                        simpleButton20.Enabled = true;
                    }
                }
                if (took == "3")
                {
                    simpleButton19.Enabled = true;
                }
                //spbSurePeople3.Text = CommonClass.UserInfo.UserName;
            }
        
      
        }

        private void setdata()
        {
            SqlParasEntity sql = new SqlParasEntity(OperType.Query, "USP_Get_badInfo_xm");
            ds = SqlHelper.GetDataSet(sql);
            DataRow[] rows = ds.Tables[0].Select("PID=0");//异常项目
            for (int i = 0; i < rows.Length; i++)
            {
                badtype.Properties.Items.Add(rows[i]["title"]);
            }
        }

        private void getData()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID",ID));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_AbnormalRegistration", list);
                DataSet ds = SqlHelper.GetDataSet(spe);
                registerDeprtment.Text=ds.Tables[0].Rows[0]["registerDeprtment"].ToString();
                badtype.Text = ds.Tables[0].Rows[0]["badtype"].ToString();
                badtypeTwo.Text = ds.Tables[0].Rows[0]["badtypeTwo"].ToString();
                zerenwebid1.Text = ds.Tables[0].Rows[0]["zerenwebid1"].ToString();
                ngePerCapitaEnergizing.Text = ds.Tables[0].Rows[0]["zerenacc1"].ToString();
                spbSurePeople1.Text = ds.Tables[0].Rows[0]["spbSurePeople1"].ToString();
                spbDepart_o_People1.Text = ds.Tables[0].Rows[0]["spbDepart_o_People1"].ToString();
                spbDepart_o_People2.Text = ds.Tables[0].Rows[0]["spbDepart_o_People2"].ToString();
                spbDepart_o_People3.Text = ds.Tables[0].Rows[0]["spbDepart_o_People3"].ToString();
                zerenwebid2.Text = ds.Tables[0].Rows[0]["zerenwebid2"].ToString();
                spbSurePeople2.Text = ds.Tables[0].Rows[0]["spbSurePeople2"].ToString();
                spbDepart_t_People1.Text = ds.Tables[0].Rows[0]["spbDepart_t_People1"].ToString();
                spbDepart_t_People2.Text = ds.Tables[0].Rows[0]["spbDepart_t_People2"].ToString();
                spbDepart_t_People3.Text = ds.Tables[0].Rows[0]["spbDepart_t_People3"].ToString();
                zerenwebid3.Text = ds.Tables[0].Rows[0]["zerenwebid3"].ToString();
                spbSurePeople3.Text = ds.Tables[0].Rows[0]["spbSurePeople3"].ToString();
                spbDepart_tr_People1.Text = ds.Tables[0].Rows[0]["spbDepart_tr_People1"].ToString();
                spbDepart_tr_People2.Text = ds.Tables[0].Rows[0]["spbDepart_tr_People2"].ToString();
                spbDepart_tr_People3.Text = ds.Tables[0].Rows[0]["spbDepart_tr_People3"].ToString();
                zerenwebid11.Text = ds.Tables[0].Rows[0]["zerenwebid1"].ToString();
                zerenwebid22.Text = ds.Tables[0].Rows[0]["zerenwebid2"].ToString();
                zerenwebid33.Text = ds.Tables[0].Rows[0]["zerenwebid3"].ToString();
                badzerenchuliman.Text = ds.Tables[0].Rows[0]["badzerenchuliman"].ToString();
                abnormalityState = ds.Tables[0].Rows[0]["abnormalityState"].ToString();
                comboBoxEdit6.EditValue = ds.Tables[0].Rows[0]["abnormalityState"].ToString();
                spbFeedbackPeople.Text = ds.Tables[0].Rows[0]["spbFeedbackPeople"].ToString();
                RepresentationsWeb.Text = ds.Tables[0].Rows[0]["RepresentationsWeb"].ToString();
                RejectPeople.Text = ds.Tables[0].Rows[0]["RejectPeople"].ToString();
                Reject.Text = ds.Tables[0].Rows[0]["RejectTime"].ToString();
                VotoPeople.Text = ds.Tables[0].Rows[0]["VotoPeople"].ToString();
                spbFeedbackContent.Text = ds.Tables[0].Rows[0]["spbFeedbackContent"].ToString();
                took = ds.Tables[0].Rows[0]["took"].ToString();
                RejectCause.Text = ds.Tables[0].Rows[0]["RejectCause"].ToString();
                Reject.Text = ds.Tables[0].Rows[0]["RejectTime"].ToString();
                VetoCause.Text = ds.Tables[0].Rows[0]["VetoCause"].ToString();
                VotoPunishment.Text = ds.Tables[0].Rows[0]["VotoPunishment"].ToString();
                buyToCompensateDep1.Text = ds.Tables[0].Rows[0]["buyToCompensateDep1"].ToString();
                buyToCompensateDep2.Text = ds.Tables[0].Rows[0]["buyToCompensateDep2"].ToString();
                buyToCompensateDep3.Text = ds.Tables[0].Rows[0]["buyToCompensateDep3"].ToString();
                buyToCompensateSpe1.Text = ds.Tables[0].Rows[0]["buyToCompensateSpe1"].ToString();
                buyToCompensateSpe2.Text = ds.Tables[0].Rows[0]["buyToCompensateSpe2"].ToString();
                buyToCompensateSpe3.Text = ds.Tables[0].Rows[0]["buyToCompensateSpe3"].ToString();
                buyToCompensateClaim1.Text = ds.Tables[0].Rows[0]["buyToCompensateClaim1"].ToString();
                buyToCompensateClaim2.Text = ds.Tables[0].Rows[0]["buyToCompensateClaim2"].ToString();
                buyToCompensateClaim3.Text = ds.Tables[0].Rows[0]["buyToCompensateClaim3"].ToString();
                extractAmount.Text = ds.Tables[0].Rows[0]["extractAmount"].ToString();
                CostDepartment.Text = ds.Tables[0].Rows[0]["CostDepartment"].ToString();
                badchuliyijian.Text = ds.Tables[0].Rows[0]["badchuliyijian"].ToString();
                badchuliman.Text = ds.Tables[0].Rows[0]["badchuliman"].ToString();
                ggzrryy.Text = ds.Tables[0].Rows[0]["ggzrryy"].ToString();
                EXTDepart.Text = ds.Tables[0].Rows[0]["zerenwebid1"].ToString();
                //2018.1.8wbw
                spbSureSubmiter1 = ds.Tables[0].Rows[0]["spbSureSubmiter1"].ToString();
                spbSureSubmiter2 = ds.Tables[0].Rows[0]["spbSureSubmiter2"].ToString();
                spbSureSubmiter3 = ds.Tables[0].Rows[0]["spbSureSubmiter3"].ToString();

                RepresentationsReward.Text = ds.Tables[0].Rows[0]["RepresentationsReward"].ToString();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }



        }


        DataSet ds = null;
     

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
                    content = content + userName + ":" + badcreateby.Text.Trim() + "--" + gcdate + "--反馈人2:" + fangkuiman2.Text.Trim() + " " + "反馈部门:" + fangkuibm.Text.Trim();
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
                    content = content + userName + ":" + badcreateby.Text.Trim() + "--" + gcdate + "--反馈人3:" + fangkuiman2.Text.Trim() + " " + "反馈部门:" + fangkuibm.Text.Trim();
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
                    content = content + userName + ":" + badcreateby.Text.Trim() + "--" + gcdate + "--反馈人4:" + fangkuiman2.Text.Trim() + " " + "反馈部门:" + fangkuibm.Text.Trim();
                }
                else
                {
                    content = content + userName + ":" + badcreateby.Text.Trim() + "--" + gcdate + " " + "反馈部门:" + fangkuibm.Text.Trim();
                }

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", ID));
                list.Add(new SqlPara("badcontent", content));
                list.Add(new SqlPara("fangkuiman", fangkuiman2.Text.Trim()));
                list.Add(new SqlPara("fangkuiwebid", fangkuibm.Text.Trim()));
                list.Add(new SqlPara("type", type));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_MODIFIED_AbnormalRegistration_SA", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    List<SqlPara> listZQTMS = new List<SqlPara>();  //maohui20180724(将数据同步到ZQTMS)
                    listZQTMS.Add(new SqlPara("ID", ID));
                    listZQTMS.Add(new SqlPara("badcontent", content));
                    listZQTMS.Add(new SqlPara("fangkuiman", fangkuiman2.Text.Trim()));
                    listZQTMS.Add(new SqlPara("fangkuiwebid", fangkuibm.Text.Trim()));
                    listZQTMS.Add(new SqlPara("type", type));
                    SqlParasEntity spsZQTMS = new SqlParasEntity(OperType.Execute, "USP_MODIFIED_AbnormalRegistration_SA", listZQTMS);
                    SqlHelper.ExecteNonQuery_ZQTMS(spsZQTMS);
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
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_BAD_AbnormalRegistration", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    List<SqlPara> listZQTMS = new List<SqlPara>();  //maohui20180724(将数据同步到ZQTMS)
                    listZQTMS.Add(new SqlPara("ID", ID));
                    listZQTMS.Add(new SqlPara("badchuliman", userName));
                    listZQTMS.Add(new SqlPara("badchulidate", gcdate));
                    listZQTMS.Add(new SqlPara("badchuliyijian", content));
                    SqlParasEntity spsZQTMS = new SqlParasEntity(OperType.Execute, "USP_BAD_AbnormalRegistration", listZQTMS);
                    SqlHelper.ExecteNonQuery_ZQTMS(spsZQTMS);
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

       

       

        

        



        private void UpdateToOA()
        {
          

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
      
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            UpdateToOA();
        }



        private void simpleButton8_Click(object sender, EventArgs e)
        {
           
            fmFileShow fm = new fmFileShow();
            fm.billNo = crrBillNO;
            fm.billType = 1;
            fm.ShowDialog();
        }


  

        //确认责任1 wwb2017.9.15
        private void simpleButton9_Click(object sender, EventArgs e)
        {
            if (MsgBox.ShowYesNo("是否确认责任？") != DialogResult.Yes) return;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", ID));
                list.Add(new SqlPara("spbSurePeople1", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("spbSure", DateTime.Now.ToString()));
                list.Add(new SqlPara("abnormalityState","已处理"));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_ADD_AbnormalRegistration_zrqr1", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    List<SqlPara> listZQTMS = new List<SqlPara>();//maohui20180724(将数据同步到ZQTMS)
                    listZQTMS.Add(new SqlPara("ID", ID));
                    listZQTMS.Add(new SqlPara("spbSurePeople1", CommonClass.UserInfo.UserName));
                    listZQTMS.Add(new SqlPara("spbSure", DateTime.Now.ToString()));
                    listZQTMS.Add(new SqlPara("abnormalityState", "已处理"));
                    SqlParasEntity spsZQTMS = new SqlParasEntity(OperType.Execute, "QSP_ADD_AbnormalRegistration_zrqr1", listZQTMS);
                    SqlHelper.ExecteNonQuery_ZQTMS(spsZQTMS);
                    MsgBox.ShowOK("责任部门确认成功！");
                    getData();
                    simpleButton9.Enabled = false;
                    simpleButton10.Enabled = true;
                    //simpleButton11.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //提交责任人1wwb2017.9.15
        private void simpleButton10_Click(object sender, EventArgs e)
        {
            if (spbDepart_o_People1.Text == "")
            {
                MsgBox.ShowOK("责任人1不能为空！");
                return;
            }
            if (MsgBox.ShowYesNo("是否提交责任人？") != DialogResult.Yes) return;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("spbDepart_o_People1", spbDepart_o_People1.Text.Trim()));
                list.Add(new SqlPara("spbDepart_o_People2", spbDepart_o_People2.Text.Trim()));
                list.Add(new SqlPara("spbDepart_o_People3", spbDepart_o_People3.Text.Trim()));
                list.Add(new SqlPara("spbSureSubmiter1", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("spbSureSbTime1", DateTime.Now.ToString()));
                if (zerenwebid2.Text == "" || spbDepart_t_People1.Text != "" || (spbDepart_t_People1.Text != "" && spbDepart_tr_People1.Text != ""))
                {
                    list.Add(new SqlPara("abnormalityState", "已完结"));
                }
                else
                {
                    list.Add(new SqlPara("abnormalityState", "已仲裁"));
                }
                list.Add(new SqlPara("took", '1'));
                list.Add(new SqlPara("ID", ID));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_ADD_AbnormalRegistration_tjzrr", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    List<SqlPara> listZQTMS = new List<SqlPara>();//maohui20480724(将数据同步到ZQTMS系统)
                    listZQTMS.Add(new SqlPara("spbDepart_o_People1", spbDepart_o_People1.Text.Trim()));
                    listZQTMS.Add(new SqlPara("spbDepart_o_People2", spbDepart_o_People2.Text.Trim()));
                    listZQTMS.Add(new SqlPara("spbDepart_o_People3", spbDepart_o_People3.Text.Trim()));
                    listZQTMS.Add(new SqlPara("spbSureSubmiter1", CommonClass.UserInfo.UserName));
                    listZQTMS.Add(new SqlPara("spbSureSbTime1", DateTime.Now.ToString()));
                    if (zerenwebid2.Text == "" || spbDepart_t_People1.Text != "" || (spbDepart_t_People1.Text != "" && spbDepart_tr_People1.Text != ""))
                    {
                        listZQTMS.Add(new SqlPara("abnormalityState", "已完结"));
                    }
                    else
                    {
                        listZQTMS.Add(new SqlPara("abnormalityState", "已仲裁"));
                    }
                    listZQTMS.Add(new SqlPara("took", '1'));
                    listZQTMS.Add(new SqlPara("ID", ID));
                    SqlParasEntity spsZQTMS = new SqlParasEntity(OperType.Execute, "QSP_ADD_AbnormalRegistration_tjzrr", listZQTMS);
                    SqlHelper.ExecteNonQuery_ZQTMS(spsZQTMS);
                    simpleButton10.Enabled = false;
                    simpleButton11.Enabled = true;
                    MsgBox.ShowOK("提交责任人成功！");
                    //已完结状态后把数据同步到LMS hj20180511
                    //Common.CommonSyn.AbnormalSyn(billNo);
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }



        }

        //更改责任人
        private void simpleButton11_Click(object sender, EventArgs e)
        {
           
            //List<SqlPara> list1 = new List<SqlPara>();
            //list1.Add(new SqlPara("ID",ID));
            //list1.Add(new SqlPara("TYPE", "1"));
            //SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_AbnormalRegistration_ID", list1);
            //DataSet ds = SqlHelper.GetDataSet(spe);
            //string a = ds.Tables[0].Rows[0]["spbSureTime1"].ToString();
            //DateTime b = DateTime.Parse(a);
            //DateTime c = DateTime.Now;
            //if (b > DateTime.Parse(b.ToShortDateString() + " 04:00:00"))
            //{
            //    if (c > DateTime.Parse(b.ToShortDateString() + " 04:00:00").AddDays(3))
            //    {
            //        MsgBox.ShowOK("更改责任人时间已过！");
            //        return;
            //    }

            //}
            //if (b < DateTime.Parse(b.ToShortDateString() + " 04:00:00"))
            //{
            //    if (c > DateTime.Parse(b.ToShortDateString() + " 04:00:00").AddDays(2))
            //    {
            //        MsgBox.ShowOK("更改责任人时间已过！");
            //        return;
            //    }

            //}


            //System.TimeSpan d = c - b;
            //double getHours = d.TotalHours;
            //if (getHours > 72)
            //{
            //    MsgBox.ShowOK("现在距离提交责任人已过去72小时，无法更改责任人！");
            //    return;
            //}

            if (MsgBox.ShowYesNo("是否更改责任人？请慎重，更改责任人只能更改一次") != DialogResult.Yes) return;
            if (spbDepart_o_People1.Text == "")
            {
                MsgBox.ShowOK("责任人1不能为空！");
                return;
            }

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("spbDepart_o_People1", spbDepart_o_People1.Text.Trim()));
                list.Add(new SqlPara("spbDepart_o_People2", spbDepart_o_People2.Text.Trim()));
                list.Add(new SqlPara("spbDepart_o_People3", spbDepart_o_People3.Text.Trim()));
                list.Add(new SqlPara("spbSureEter1", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("spbSureEdTime1", DateTime.Now.ToString()));
                list.Add(new SqlPara("ID", ID));
                SqlParasEntity spe1 = new SqlParasEntity(OperType.Execute, "QSP_ADD_b_bad_tyd_ggzrr", list);
                if (SqlHelper.ExecteNonQuery(spe1) > 0)
                {
                    List<SqlPara> listZQTMS = new List<SqlPara>();  //maohui20180724(将数据同步到ZQTMS系统)
                    listZQTMS.Add(new SqlPara("spbDepart_o_People1", spbDepart_o_People1.Text.Trim()));
                    listZQTMS.Add(new SqlPara("spbDepart_o_People2", spbDepart_o_People2.Text.Trim()));
                    listZQTMS.Add(new SqlPara("spbDepart_o_People3", spbDepart_o_People3.Text.Trim()));
                    listZQTMS.Add(new SqlPara("spbSureEter1", CommonClass.UserInfo.UserName));
                    listZQTMS.Add(new SqlPara("spbSureEdTime1", DateTime.Now.ToString()));
                    listZQTMS.Add(new SqlPara("ID", ID));
                    SqlParasEntity spsZQTMS = new SqlParasEntity(OperType.Execute, "QSP_ADD_b_bad_tyd_ggzrr", listZQTMS);
                    SqlHelper.ExecteNonQuery_ZQTMS(spsZQTMS);
                    simpleButton11.Enabled = false;
                    if (CommonClass.UserInfo.WebName.Contains("品质管理部"))
                    {
                        simpleButton11.Enabled = true;
                    }
                    MsgBox.ShowOK("更改责任人成功！");
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

   
        //提交反馈wwb2017.9.15
        private void tijiaofankui_Click(object sender, EventArgs e)
        {

            if (spbFeedbackContent.Text == "")
            {
                MsgBox.ShowOK("申述内容不能为空！");
                return;
            }
            if (MsgBox.ShowYesNo("是否提交申述？") != DialogResult.Yes) return;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("spbFeedbackContent", spbFeedbackContent.Text.Trim()));
                list.Add(new SqlPara("RepresentationsWeb", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("spbFeedbackPeople", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("feedbackTime", DateTime.Now.ToString()));
                list.Add(new SqlPara("ID", ID));
                list.Add(new SqlPara("abnormalityState", "待仲裁"));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_ADD_AbnormalRegistration", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    List<SqlPara> listZQTMS = new List<SqlPara>();//maohui20180724(将数据同步到ZQTMS系统)
                    listZQTMS.Add(new SqlPara("spbFeedbackContent", spbFeedbackContent.Text.Trim()));
                    listZQTMS.Add(new SqlPara("RepresentationsWeb", CommonClass.UserInfo.WebName));
                    listZQTMS.Add(new SqlPara("spbFeedbackPeople", CommonClass.UserInfo.UserName));
                    listZQTMS.Add(new SqlPara("feedbackTime", DateTime.Now.ToString()));
                    listZQTMS.Add(new SqlPara("ID", ID));
                    listZQTMS.Add(new SqlPara("abnormalityState", "待仲裁"));
                    SqlParasEntity spsZQTMS = new SqlParasEntity(OperType.Execute, "QSP_ADD_AbnormalRegistration", listZQTMS);
                    SqlHelper.ExecteNonQuery_ZQTMS(spsZQTMS);
                    MsgBox.ShowOK("提交申述成功！");
                    tijiaofankui.Enabled = false;
                    FileUpload.AddUpLoadMoreImg(1, billNos, paths, CommonClass.UserInfo.UserName);
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }

        //驳回
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            if (MsgBox.ShowYesNo("是否驳回？") != DialogResult.Yes) return;
            if (RejectCause.Text.Trim() == "")
            {
                MsgBox.ShowOK("驳回原因不能为空！");
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", ID));
                list.Add(new SqlPara("RejectTime", DateTime.Now.ToString()));
                list.Add(new SqlPara("RejectCause", RejectCause.Text.Trim()));
                list.Add(new SqlPara("RejectPeople", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("zerenacc1", ngePerCapitaEnergizing.Text.Trim()));
                list.Add(new SqlPara("abnormalityState", "已仲裁"));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_ADD_b_AbnormalRegistration_bohui", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    List<SqlPara> listZQTMS = new List<SqlPara>();//maohui20180724(将数据同步到ZQTMS系统)
                    listZQTMS.Add(new SqlPara("ID", ID));
                    listZQTMS.Add(new SqlPara("RejectTime", DateTime.Now.ToString()));
                    listZQTMS.Add(new SqlPara("RejectCause", RejectCause.Text.Trim()));
                    listZQTMS.Add(new SqlPara("RejectPeople", CommonClass.UserInfo.UserName));
                    listZQTMS.Add(new SqlPara("zerenacc1", ngePerCapitaEnergizing.Text.Trim()));
                    listZQTMS.Add(new SqlPara("abnormalityState", "已仲裁"));
                    SqlParasEntity spsZQTMS = new SqlParasEntity(OperType.Execute, "QSP_ADD_b_AbnormalRegistration_bohui", listZQTMS);
                    SqlHelper.ExecteNonQuery_ZQTMS(spsZQTMS);
                    MsgBox.ShowOK("驳回成功！");
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }


        }
    
        //否决
        private void simpleButton14_Click(object sender, EventArgs e)
        {

            if (MsgBox.ShowYesNo("是否否决？") != DialogResult.Yes) return;
            if (VetoCause.Text.Trim() == "")
            {
                MsgBox.ShowOK("否决内容不能为空！");
                return;
            }
            if (VotoPunishment.Text == "")
            {
                MsgBox.ShowOK("否决处罚不能为空！");
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID",ID));
                list.Add(new SqlPara("VetoCause", VetoCause.Text.Trim()));
                list.Add(new SqlPara("VotoPeople", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("VotoPunishment", VotoPunishment.Text.Trim()));
                list.Add(new SqlPara("abnormalityState", "已完结"));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_ADD_AbnormalRegistration_foujue", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    List<SqlPara> listZQTMS = new List<SqlPara>();//maohui20180724(将数据同步到ZQTMS系统)
                    listZQTMS.Add(new SqlPara("ID", ID));
                    listZQTMS.Add(new SqlPara("VetoCause", VetoCause.Text.Trim()));
                    listZQTMS.Add(new SqlPara("VotoPeople", CommonClass.UserInfo.UserName));
                    listZQTMS.Add(new SqlPara("VotoPunishment", VotoPunishment.Text.Trim()));
                    listZQTMS.Add(new SqlPara("abnormalityState", "已完结"));
                    SqlParasEntity spsZQTMS = new SqlParasEntity(OperType.Execute, "QSP_ADD_AbnormalRegistration_foujue", listZQTMS);
                    SqlHelper.ExecteNonQuery_ZQTMS(spsZQTMS);
                    MsgBox.ShowOK("否决成功！");
                    getData();
                }



            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }


        }

        //确认责任划分
        private void simpleButton12_Click(object sender, EventArgs e)
        {
            if (ggzrryy.Text == "")
            {
                MsgBox.ShowOK("更改原因不能为空！");
                return;
            }
            
            if (zerenwebid11.EditValue == "")
            {
                MsgBox.ShowOK("责任部门1不能为空！");
                return;
            }
            if (MsgBox.ShowYesNo("是否确认责任划分？") != DialogResult.Yes) return;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID",ID));
                list.Add(new SqlPara("zerenwebid1", zerenwebid11.Text.Trim()));
                list.Add(new SqlPara("zerenwebid2", zerenwebid22.Text.Trim()));
                list.Add(new SqlPara("zerenwebid3", zerenwebid33.Text.Trim()));
                list.Add(new SqlPara("ggzrryy", ggzrryy.Text.Trim()));
                list.Add(new SqlPara("badzerenchuliman",CommonClass.UserInfo.UserName ));
                list.Add(new SqlPara("badzerenchulidate",DateTime.Now.ToString()));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_UPDATE_AbnormalRegistration_zerenwebid1", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    List<SqlPara> listZQTMS = new List<SqlPara>();//maohui20180724(将数据同步到ZQTMS系统)
                    listZQTMS.Add(new SqlPara("ID", ID));
                    listZQTMS.Add(new SqlPara("zerenwebid1", zerenwebid11.Text.Trim()));
                    listZQTMS.Add(new SqlPara("zerenwebid2", zerenwebid22.Text.Trim()));
                    listZQTMS.Add(new SqlPara("zerenwebid3", zerenwebid33.Text.Trim()));
                    listZQTMS.Add(new SqlPara("ggzrryy", ggzrryy.Text.Trim()));
                    listZQTMS.Add(new SqlPara("badzerenchuliman", CommonClass.UserInfo.UserName));
                    listZQTMS.Add(new SqlPara("badzerenchulidate", DateTime.Now.ToString()));
                    SqlParasEntity spsZQTMS = new SqlParasEntity(OperType.Execute, "QSP_UPDATE_AbnormalRegistration_zerenwebid1", listZQTMS);
                    SqlHelper.ExecteNonQuery_ZQTMS(spsZQTMS);
                    MsgBox.ShowOK("责任部门确认成功！");
                    simpleButton9.Enabled = false;
                    getData();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //取消责任部门
        private void simpleButton13_Click(object sender, EventArgs e)
        {
            if (MsgBox.ShowYesNo("是否取消责任部门？") != DialogResult.Yes) return;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", ID));
                list.Add(new SqlPara("badzerenchuliman", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("badzerenchulidate", DateTime.Now.ToString()));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_QUXIAO_AbnormalRegistration_zerenwebid1", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    List<SqlPara> listZQTMS = new List<SqlPara>();  //maohui20180724(将数据同步到ZQTMS系统)
                    listZQTMS.Add(new SqlPara("ID", ID));
                    listZQTMS.Add(new SqlPara("badzerenchuliman", CommonClass.UserInfo.UserName));
                    listZQTMS.Add(new SqlPara("badzerenchulidate", DateTime.Now.ToString()));
                    SqlParasEntity spsZQTMS = new SqlParasEntity(OperType.Execute, "QSP_QUXIAO_AbnormalRegistration_zerenwebid1", listZQTMS);
                    SqlHelper.ExecteNonQuery_ZQTMS(spsZQTMS);
                    MsgBox.ShowOK("取消责任部门成功！");
                    getData();

                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //确认提取
        private void simpleButton7_Click(object sender, EventArgs e)
        {
            if (MsgBox.ShowYesNo("是否确认提取？") != DialogResult.Yes) return;
            if (extractAmount.Text == "")
            {
                MsgBox.ShowOK("成本提取金额不能为空！");
                return;
            }

            if (CostDepartment.Text == "")
            {
                MsgBox.ShowOK("成本提取部门不能为空！");
                return;
            }

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("extractAmount", extractAmount.Text.Trim()));
                list.Add(new SqlPara("CostDepartment", CostDepartment.Text.Trim()));
                list.Add(new SqlPara("ID", ID));
                list.Add(new SqlPara("extractPeople", CommonClass.UserInfo.UserName));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_ADD_b_bad_tyd_cbtq", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    List<SqlPara> listZQTMS = new List<SqlPara>();//maohui20180724(将数据同步到ZQTMS系统)
                    listZQTMS.Add(new SqlPara("extractAmount", extractAmount.Text.Trim()));
                    listZQTMS.Add(new SqlPara("CostDepartment", CostDepartment.Text.Trim()));
                    listZQTMS.Add(new SqlPara("ID", ID));
                    listZQTMS.Add(new SqlPara("extractPeople", CommonClass.UserInfo.UserName));
                    SqlParasEntity spsZQTMS = new SqlParasEntity(OperType.Execute, "QSP_ADD_b_bad_tyd_cbtq", listZQTMS);
                    SqlHelper.ExecteNonQuery_ZQTMS(spsZQTMS);
                    MsgBox.ShowOK("成本提取确认成功！");
                }


            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }



        }

        //货差买赔确认
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (MsgBox.ShowYesNo("是否货差买赔确认？") != DialogResult.Yes) return;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID",ID));
                list.Add(new SqlPara("buyToCompensateDep1", buyToCompensateDep1.EditValue));
                list.Add(new SqlPara("buyToCompensateDep2", buyToCompensateDep2.EditValue));
                list.Add(new SqlPara("buyToCompensateDep3", buyToCompensateDep3.EditValue));
                list.Add(new SqlPara("buyToCompensateSpe1", buyToCompensateSpe1.Text.Trim()));
                list.Add(new SqlPara("buyToCompensateSpe2", buyToCompensateSpe2.Text.Trim()));
                list.Add(new SqlPara("buyToCompensateSpe3", buyToCompensateSpe3.Text.Trim()));
                list.Add(new SqlPara("buyToCompensateClaim1", buyToCompensateClaim1.Text.Trim()));
                list.Add(new SqlPara("buyToCompensateClaim2", buyToCompensateClaim2.Text.Trim()));
                list.Add(new SqlPara("buyToCompensateClaim3", buyToCompensateClaim3.Text.Trim()));
                list.Add(new SqlPara("hcmpqrr", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("hcmptime",DateTime.Now.ToString()));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_ADD_AbnormalRegistration_hcmp", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    List<SqlPara> listZQTMS = new List<SqlPara>(); //maohui20180724(将数据同步到ZQTMS系统)
                    listZQTMS.Add(new SqlPara("ID", ID));
                    listZQTMS.Add(new SqlPara("buyToCompensateDep1", buyToCompensateDep1.EditValue));
                    listZQTMS.Add(new SqlPara("buyToCompensateDep2", buyToCompensateDep2.EditValue));
                    listZQTMS.Add(new SqlPara("buyToCompensateDep3", buyToCompensateDep3.EditValue));
                    listZQTMS.Add(new SqlPara("buyToCompensateSpe1", buyToCompensateSpe1.Text.Trim()));
                    listZQTMS.Add(new SqlPara("buyToCompensateSpe2", buyToCompensateSpe2.Text.Trim()));
                    listZQTMS.Add(new SqlPara("buyToCompensateSpe3", buyToCompensateSpe3.Text.Trim()));
                    listZQTMS.Add(new SqlPara("buyToCompensateClaim1", buyToCompensateClaim1.Text.Trim()));
                    listZQTMS.Add(new SqlPara("buyToCompensateClaim2", buyToCompensateClaim2.Text.Trim()));
                    listZQTMS.Add(new SqlPara("buyToCompensateClaim3", buyToCompensateClaim3.Text.Trim()));
                    listZQTMS.Add(new SqlPara("hcmpqrr", CommonClass.UserInfo.UserName));
                    listZQTMS.Add(new SqlPara("hcmptime", DateTime.Now.ToString()));
                    SqlParasEntity spsZQTMS = new SqlParasEntity(OperType.Execute, "QSP_ADD_AbnormalRegistration_hcmp", listZQTMS);
                    SqlHelper.ExecteNonQuery_ZQTMS(spsZQTMS);
                    MsgBox.ShowOK("货差买赔确认成功！");
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }

        //责任部门2确认
        private void simpleButton15_Click(object sender, EventArgs e)
        {
            if (MsgBox.ShowYesNo("是否确认责任？") != DialogResult.Yes) return;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", ID));
                list.Add(new SqlPara("spbSurePeople2", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("spbSureTime2", DateTime.Now.ToString()));
                list.Add(new SqlPara("abnormalityState", "已处理"));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_ADD_AbnormalRegistration_zrqr2", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    List<SqlPara> listZQTMS = new List<SqlPara>();  //maohui20180724(将数据同步到ZQTMS系统)
                    listZQTMS.Add(new SqlPara("ID", ID));
                    listZQTMS.Add(new SqlPara("spbSurePeople2", CommonClass.UserInfo.UserName));
                    listZQTMS.Add(new SqlPara("spbSureTime2", DateTime.Now.ToString()));
                    listZQTMS.Add(new SqlPara("abnormalityState", "已处理"));
                    SqlParasEntity spsZQTMS = new SqlParasEntity(OperType.Execute, "QSP_ADD_AbnormalRegistration_zrqr2", listZQTMS);
                    SqlHelper.ExecteNonQuery_ZQTMS(spsZQTMS);
                    MsgBox.ShowOK("责任部门确认成功！");
                    simpleButton17.Enabled = true;
                    //simpleButton16.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }


        }

        //责任部门3确认
        private void simpleButton18_Click(object sender, EventArgs e)
        {
            if (MsgBox.ShowYesNo("是否确认责任？") != DialogResult.Yes) return;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", ID));
                list.Add(new SqlPara("spbSurePeople3", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("spbSureTime3", DateTime.Now.ToString()));
                list.Add(new SqlPara("abnormalityState", "已处理"));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_ADD_AbnormalRegistration_zrqr3", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    List<SqlPara> listZQTMS = new List<SqlPara>();  //maohui20180724(将数据同步到ZQTMS系统)
                    listZQTMS.Add(new SqlPara("ID", ID));
                    listZQTMS.Add(new SqlPara("spbSurePeople3", CommonClass.UserInfo.UserName));
                    listZQTMS.Add(new SqlPara("spbSureTime3", DateTime.Now.ToString()));
                    listZQTMS.Add(new SqlPara("abnormalityState", "已处理"));
                    SqlParasEntity spsZQTMS = new SqlParasEntity(OperType.Execute, "QSP_ADD_AbnormalRegistration_zrqr3", listZQTMS);
                    SqlHelper.ExecteNonQuery_ZQTMS(spsZQTMS);
                    MsgBox.ShowOK("责任部门确认成功！");
                    simpleButton20.Enabled = true;
                    simpleButton19.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //提交责任人2wwb2017.9.15
        private void simpleButton17_Click(object sender, EventArgs e)
        {
            if (spbDepart_t_People1.Text == "")
            {
                MsgBox.ShowOK("责任人1不能为空！");
                return;
            }
            if (MsgBox.ShowYesNo("是否提交责任人？") != DialogResult.Yes) return;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("spbDepart_t_People1", spbDepart_t_People1.Text.Trim()));
                list.Add(new SqlPara("spbDepart_t_People2", spbDepart_t_People2.Text.Trim()));
                list.Add(new SqlPara("spbDepart_t_People3", spbDepart_t_People3.Text.Trim()));
                list.Add(new SqlPara("spbSureSubmiter2", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("spbSureSbTime2", DateTime.Now.ToString()));
                if ((spbDepart_o_People1.Text != "" && zerenwebid3.Text == "") || (spbDepart_o_People1.Text != "" && spbDepart_tr_People1.Text != ""))
                {
                    list.Add(new SqlPara("abnormalityState", "已完结"));
                }
                else
                {
                    list.Add(new SqlPara("abnormalityState", "已仲裁"));
                }
                list.Add(new SqlPara("ID", ID));
                list.Add(new SqlPara("took", '2'));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_ADD_AbnormalRegistration_tjzrr2", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    List<SqlPara> listZQTMS = new List<SqlPara>();//maohui20180724(将数据同步到ZQTMS系统)
                    listZQTMS.Add(new SqlPara("spbDepart_t_People1", spbDepart_t_People1.Text.Trim()));
                    listZQTMS.Add(new SqlPara("spbDepart_t_People2", spbDepart_t_People2.Text.Trim()));
                    listZQTMS.Add(new SqlPara("spbDepart_t_People3", spbDepart_t_People3.Text.Trim()));
                    listZQTMS.Add(new SqlPara("spbSureSubmiter2", CommonClass.UserInfo.UserName));
                    listZQTMS.Add(new SqlPara("spbSureSbTime2", DateTime.Now.ToString()));
                    if ((spbDepart_o_People1.Text != "" && zerenwebid3.Text == "") || (spbDepart_o_People1.Text != "" && spbDepart_tr_People1.Text != ""))
                    {
                        listZQTMS.Add(new SqlPara("abnormalityState", "已完结"));
                    }
                    else
                    {
                        listZQTMS.Add(new SqlPara("abnormalityState", "已仲裁"));
                    }
                    listZQTMS.Add(new SqlPara("ID", ID));
                    listZQTMS.Add(new SqlPara("took", '2'));
                    SqlParasEntity spsZQTMS = new SqlParasEntity(OperType.Execute, "QSP_ADD_AbnormalRegistration_tjzrr2", listZQTMS);
                    SqlHelper.ExecteNonQuery_ZQTMS(spsZQTMS);
                    simpleButton17.Enabled = false;
                    MsgBox.ShowOK("提交责任人成功！");
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //提交责任人3
        private void simpleButton20_Click(object sender, EventArgs e)
        {
            if (spbDepart_tr_People1.Text == "")
            {
                MsgBox.ShowOK("责任人1不能为空！");
                return;
            }
            if (MsgBox.ShowYesNo("是否提交责任人？") != DialogResult.Yes) return;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("spbDepart_tr_People1", spbDepart_tr_People1.Text.Trim()));
                list.Add(new SqlPara("spbDepart_tr_People2", spbDepart_tr_People2.Text.Trim()));
                list.Add(new SqlPara("spbDepart_tr_People3", spbDepart_tr_People3.Text.Trim()));
                list.Add(new SqlPara("spbSureSubmiter3", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("spbSureSbTime3", DateTime.Now.ToString()));
                list.Add(new SqlPara("ID", ID));
                if (spbDepart_o_People1.Text != "" && spbDepart_tr_People1.Text != "")
                {
                    list.Add(new SqlPara("abnormalityState", "已完结"));
                }
                else
                {
                    list.Add(new SqlPara("abnormalityState", "已仲裁"));
                }
                list.Add(new SqlPara("took", '3'));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_ADD_AbnormalRegistration_tjzrr3", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    List<SqlPara> listZQTMS = new List<SqlPara>(); //maohui20180724(将数据同步到ZQTMS系统)
                    listZQTMS.Add(new SqlPara("spbDepart_tr_People1", spbDepart_tr_People1.Text.Trim()));
                    listZQTMS.Add(new SqlPara("spbDepart_tr_People2", spbDepart_tr_People2.Text.Trim()));
                    listZQTMS.Add(new SqlPara("spbDepart_tr_People3", spbDepart_tr_People3.Text.Trim()));
                    listZQTMS.Add(new SqlPara("spbSureSubmiter3", CommonClass.UserInfo.UserName));
                    listZQTMS.Add(new SqlPara("spbSureSbTime3", DateTime.Now.ToString()));
                    listZQTMS.Add(new SqlPara("ID", ID));
                    if (spbDepart_o_People1.Text != "" && spbDepart_tr_People1.Text != "")
                    {
                        listZQTMS.Add(new SqlPara("abnormalityState", "已完结"));
                    }
                    else
                    {
                        listZQTMS.Add(new SqlPara("abnormalityState", "已仲裁"));
                    }
                    listZQTMS.Add(new SqlPara("took", '3'));
                    SqlParasEntity spsZQTMS = new SqlParasEntity(OperType.Execute, "QSP_ADD_AbnormalRegistration_tjzrr3", listZQTMS);
                    SqlHelper.ExecteNonQuery_ZQTMS(spsZQTMS);
                    simpleButton20.Enabled = false;
                    MsgBox.ShowOK("提交责任人成功！");
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }


        }

        //更改责任人2
        private void simpleButton16_Click(object sender, EventArgs e)
        {
            if (MsgBox.ShowYesNo("是否更改责任人？请慎重，更改责任人只能更改一次") != DialogResult.Yes) return;
            if (spbDepart_t_People1.Text == "")
            {
                MsgBox.ShowOK("责任人1不能为空！");
                return;
            }

            //List<SqlPara> list1 = new List<SqlPara>();
            //list1.Add(new SqlPara("ID", ID));
            //list1.Add(new SqlPara("TYPE", "2"));
            //SqlParasEntity spe1 = new SqlParasEntity(OperType.Query, "QSP_GET_AbnormalRegistration_ID", list1);
            //DataSet ds = SqlHelper.GetDataSet(spe1);
            //string a = ds.Tables[0].Rows[0]["spbSureSbTime2"].ToString();
            //DateTime b = DateTime.Parse(a);
            //DateTime c = DateTime.Now;
            //if (b > DateTime.Parse(b.ToShortDateString() + " 04:00:00"))
            //{
            //    if (c > DateTime.Parse(b.ToShortDateString() + " 04:00:00").AddDays(3))
            //    {
            //        MsgBox.ShowOK("更改责任人时间已过！");
            //        return;
            //    }

            //}
            //if (b < DateTime.Parse(b.ToShortDateString() + " 04:00:00"))
            //{
            //    if (c > DateTime.Parse(b.ToShortDateString() + " 04:00:00").AddDays(2))
            //    {
            //        MsgBox.ShowOK("更改责任人时间已过！");
            //        return;
            //    }

            //}

            //System.TimeSpan d = c - b;
            //double getHours = d.TotalHours;
            //if (getHours > 72)
            //{
            //    MsgBox.ShowOK("现在距离提交责任人已过去72小时，无法更改责任人！");
            //    return;
            //}


            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("spbDepart_t_People1", spbDepart_t_People1.Text.Trim()));
                list.Add(new SqlPara("spbDepart_t_People2", spbDepart_t_People1.Text.Trim()));
                list.Add(new SqlPara("spbDepart_t_People3", spbDepart_t_People1.Text.Trim()));
                list.Add(new SqlPara("spbSureEter2", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("spbSureEdTime2", DateTime.Now.ToString()));
                list.Add(new SqlPara("ID", ID));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_ADD_b_bad_tyd_ggzrr2", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    List<SqlPara> listZQTMS = new List<SqlPara>();  //maohui20180724
                    listZQTMS.Add(new SqlPara("spbDepart_t_People1", spbDepart_t_People1.Text.Trim()));
                    listZQTMS.Add(new SqlPara("spbDepart_t_People2", spbDepart_t_People1.Text.Trim()));
                    listZQTMS.Add(new SqlPara("spbDepart_t_People3", spbDepart_t_People1.Text.Trim()));
                    listZQTMS.Add(new SqlPara("spbSureEter2", CommonClass.UserInfo.UserName));
                    listZQTMS.Add(new SqlPara("spbSureEdTime2", DateTime.Now.ToString()));
                    listZQTMS.Add(new SqlPara("ID", ID));
                    SqlParasEntity spsZQTMS = new SqlParasEntity(OperType.Execute, "QSP_ADD_b_bad_tyd_ggzrr2", listZQTMS);
                    SqlHelper.ExecteNonQuery_ZQTMS(spsZQTMS);
                    simpleButton16.Enabled = false;
                    if (CommonClass.UserInfo.WebName.Contains("品质管理部"))
                    {
                        simpleButton16.Enabled = true;
                    }
                    MsgBox.ShowOK("更改责任人成功！");
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //更改责任人3
        private void simpleButton19_Click(object sender, EventArgs e)
        {
            if (MsgBox.ShowYesNo("是否更改责任人？请慎重，更改责任人只能更改一次") != DialogResult.Yes) return;
            if (spbDepart_tr_People1.Text == "")
            {
                MsgBox.ShowOK("责任人1不能为空！");
                return;
            }
            //List<SqlPara> list1 = new List<SqlPara>();
            //list1.Add(new SqlPara("ID", ID));
            //list1.Add(new SqlPara("TYPE", "3"));
            //SqlParasEntity spe1 = new SqlParasEntity(OperType.Query, "QSP_GET_AbnormalRegistration_ID", list1);
            //DataSet ds = SqlHelper.GetDataSet(spe1);
            //string a = ds.Tables[0].Rows[0]["spbSureSbTime3"].ToString();
            //DateTime b = DateTime.Parse(a);
            //DateTime c = DateTime.Now;
            //if (b > DateTime.Parse(b.ToShortDateString() + " 04:00:00"))
            //{
            //    if (c > DateTime.Parse(b.ToShortDateString() + " 04:00:00").AddDays(3))
            //    {
            //        MsgBox.ShowOK("更改责任人时间已过！");
            //        return;
            //    }

            //}
            //if (b < DateTime.Parse(b.ToShortDateString() + " 04:00:00"))
            //{
            //    if (c > DateTime.Parse(b.ToShortDateString() + " 04:00:00").AddDays(2))
            //    {
            //        MsgBox.ShowOK("更改责任人时间已过！");
            //        return;
            //    }

            //}

            //System.TimeSpan d = c - b;
            //double getHours = d.TotalHours;
            //if (getHours > 72)
            //{
            //    MsgBox.ShowOK("现在距离提交责任人已过去72小时，无法更改责任人！");
            //    return;
            //}
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("spbDepart_tr_People1", spbDepart_tr_People1.Text.Trim()));
                list.Add(new SqlPara("spbDepart_tr_People2", spbDepart_tr_People2.Text.Trim()));
                list.Add(new SqlPara("spbDepart_tr_People3", spbDepart_tr_People3.Text.Trim()));
                list.Add(new SqlPara("spbSureEter3 ", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("spbSureEdTime3", DateTime.Now.ToString()));
                list.Add(new SqlPara("ID", ID));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_ADD_b_bad_tyd_ggzrr3", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    List<SqlPara> listZQTMS = new List<SqlPara>();//maohui20180724(将数据同步到ZQTMS系统)
                    listZQTMS.Add(new SqlPara("spbDepart_tr_People1", spbDepart_tr_People1.Text.Trim()));
                    listZQTMS.Add(new SqlPara("spbDepart_tr_People2", spbDepart_tr_People2.Text.Trim()));
                    listZQTMS.Add(new SqlPara("spbDepart_tr_People3", spbDepart_tr_People3.Text.Trim()));
                    listZQTMS.Add(new SqlPara("spbSureEter3 ", CommonClass.UserInfo.UserName));
                    listZQTMS.Add(new SqlPara("spbSureEdTime3", DateTime.Now.ToString()));
                    listZQTMS.Add(new SqlPara("ID", ID));
                    SqlParasEntity spsZQTMS = new SqlParasEntity(OperType.Execute, "QSP_ADD_b_bad_tyd_ggzrr3", listZQTMS);
                    SqlHelper.ExecteNonQuery_ZQTMS(spsZQTMS);
                    simpleButton19.Enabled = false;
                    if (CommonClass.UserInfo.WebName.Contains("品质管理部"))
                    {
                        simpleButton19.Enabled = true;
                    }
                    MsgBox.ShowOK("更改责任人成功！");
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //确认责任划分2
        private void simpleButton22_Click(object sender, EventArgs e)
        {
            if (zerenwebid22.EditValue == "")
            {
                MsgBox.ShowOK("责任部门2不能为空！");
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", ID));
                list.Add(new SqlPara("zerenwebid2", zerenwebid22.Text.Trim()));
                list.Add(new SqlPara("badzerenchuliman", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("badzerenchulidate", DateTime.Now.ToString()));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_UPDATE_AbnormalRegistration_zerenwebid2", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    MsgBox.ShowOK("责任部门2确认成功！");
                    simpleButton15.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //确认责任部门3
        private void simpleButton24_Click(object sender, EventArgs e)
        {
            if (zerenwebid33.EditValue == "")
            {
                MsgBox.ShowOK("责任部门3不能为空！");
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", ID));
                list.Add(new SqlPara("zerenwebid3", zerenwebid33.Text.Trim()));
                list.Add(new SqlPara("badzerenchuliman", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("badzerenchulidate", DateTime.Now.ToString()));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_UPDATE_AbnormalRegistration_zerenwebid3", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    MsgBox.ShowOK("责任部门3确认成功！");
                    simpleButton18.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //取消责任部门2
        private void simpleButton21_Click(object sender, EventArgs e)
        {
            if (zerenwebid22.EditValue == "")
            {
                MsgBox.ShowOK("责任部门2不能为空！");
                return;
            }
            if (MsgBox.ShowYesNo("是否取消责任部门2？") != DialogResult.Yes) return;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", ID));
                list.Add(new SqlPara("badzerenchuliman", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("badzerenchulidate", DateTime.Now.ToString()));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_QUXIAO_AbnormalRegistration_zerenwebid2", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    MsgBox.ShowOK("取消责任部门2成功！");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //取消责任部门3
        private void simpleButton23_Click(object sender, EventArgs e)
        {
            if (zerenwebid33.EditValue == "")
            {
                MsgBox.ShowOK("责任部门3不能为空！");
                return;
            }
            if (MsgBox.ShowYesNo("是否取消责任部门3？") != DialogResult.Yes) return;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", ID));
                list.Add(new SqlPara("badzerenchuliman", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("badzerenchulidate", DateTime.Now.ToString()));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_QUXIAO_AbnormalRegistration_zerenwebid3", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    MsgBox.ShowOK("责任部门3取消成功");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //上传证据
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            fmFileUploadM fm = new fmFileUploadM();
            fm.ishowdel = false;
            fm.UserName = CommonClass.UserInfo.UserName;
            fm.billNo = billNo;
            fm.ShowDialog();
            billNos = fm.billNos;
            paths = fm.paths;

            if (paths.Trim() != "")
            {
               
            }
               
        }

        //查看上传数据
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            fmFileShow fm = new fmFileShow();
            fm.billNo = crrBillNO;
            fm.billType = 1;
            fm.ShowDialog();
        }

        private void simpleButton25_Click(object sender, EventArgs e)
        {
            if (MsgBox.ShowYesNo("是否修改？") != DialogResult.Yes) return;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("badtype", badtype.Text.Trim()));
                list.Add(new SqlPara("badtypeTwo", badtypeTwo.Text.Trim()));
                list.Add(new SqlPara("ngePerCapitaEnergizing", ngePerCapitaEnergizing.Text.Trim()));
                list.Add(new SqlPara("ID", ID));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_update_AbnormalRegistration_ID", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    List<SqlPara> listZQTMS = new List<SqlPara>();  //maohui20180724(将数据同步到ZQTMS)
                    listZQTMS.Add(new SqlPara("badtype", badtype.Text.Trim()));
                    listZQTMS.Add(new SqlPara("badtypeTwo", badtypeTwo.Text.Trim()));
                    listZQTMS.Add(new SqlPara("ngePerCapitaEnergizing", ngePerCapitaEnergizing.Text.Trim()));
                    listZQTMS.Add(new SqlPara("ID", ID));
                    SqlParasEntity spsZQTMS = new SqlParasEntity(OperType.Execute, "QSP_update_AbnormalRegistration_ID", listZQTMS);
                    SqlHelper.ExecteNonQuery_ZQTMS(spsZQTMS);
                    MsgBox.ShowOK("修改成功！");
                    return;

                }


            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void badtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            try
            {
                if (ds == null || ds.Tables.Count == 0) return;
                badtypeTwo.Text = "";
                badtypeTwo.Properties.Items.Clear();
                string xmname = badtype.Text;
                DataRow[] rows = ds.Tables[0].Select("title='" + xmname + "'and pID=0");

                DataRow[] rows1 = ds.Tables[0].Select("pid=" + rows[0]["ID"]);
                for (int i = 0; i < rows1.Length; i++)
                {
                    badtypeTwo.Properties.Items.Add(rows1[i]["title"]);//类型名称
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        
        }


       

    
    
    
    }
}