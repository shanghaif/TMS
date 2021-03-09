using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using System.IO;
using System.Data.OleDb;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class fmBadBillAddClone : BaseForm
    {
        DataSet ds = new DataSet();
        private string money = "";

        public string billNo = "";
        public string billNos = "";
        public string paths = "";
        public fmBadBillAddClone()
        {
            InitializeComponent();
        }


        private void save()
        {
            if (registerDeprtment.Text.Trim() == "")
            {
                MsgBox.ShowOK("必须填写登记部门！");
                registerDeprtment.Focus();
                return;
            }
            
            if (discoverMan1.Text.Trim() == "")
            {
                MsgBox.ShowOK("必须填写发现人1！");
                discoverMan1.Focus();
                return;
            }

            if (ExceType1.Text.Trim() == "")
            {
                MsgBox.ShowOK("必须填写异常项目！");
                ExceType1.Focus();
                return;
            }

            if (ExceType2.Text.Trim() == "")
            {
                MsgBox.ShowOK("必须填写异常类型！");
                ExceType2.Focus();
                return;
            }

            if (ExceDepart.Text.Trim() == "")
            {
                MsgBox.ShowOK("必须填写初始责任部门！");
                ExceDepart.Focus();
                return;
            }
            if (badcontent.Text.Trim() == "")
            {
                MsgBox.ShowOK("必须填写异常内容！");
                badcontent.Focus();
                return;
            }
            //if (paths.Trim() == "")
            //{
            //    MsgBox.ShowOK("图片未上传！");
            //    return;
            //}

            int a = 0;
            if (discoverMan1.Text != "")
            {
                a++;
            }
            if (discoverMan2.Text != "")
            {
                a++;
            }
            if (discoverMan3.Text != "")
            {
                a++;
            }

            try
            {
                string userName = CommonClass.UserInfo.UserName;
                string id = Convert.ToString(Guid.NewGuid()); //maohui20180725
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("id", id));
                list.Add(new SqlPara("BillNo", billNo));
                list.Add(new SqlPara("baddate", CommonClass.gcdate));
                list.Add(new SqlPara("badcontent", userName + ":" + badcontent.Text.Trim() + "--" + CommonClass.gcdate + "--发现人1：" + discoverMan1.Text.Trim()));
                list.Add(new SqlPara("badcreateby", badcreateby.Text.Trim()));
                list.Add(new SqlPara("registerDeprtment", registerDeprtment.Text.Trim()));
                list.Add(new SqlPara("discoverMan1", discoverMan1.Text.Trim()));
                list.Add(new SqlPara("discoverMan2", discoverMan2.Text.Trim()));
                list.Add(new SqlPara("discoverMan3", discoverMan3.Text.Trim()));
                list.Add(new SqlPara("badtype", ExceType1.Text.Trim()));
                list.Add(new SqlPara("badtypeTwo", ExceType2.Text.Trim()));
                list.Add(new SqlPara("ExceDepart", ExceDepart.Text.Trim()));
                list.Add(new SqlPara("zerenacc1", money));
                list.Add(new SqlPara("abnormalityState", "待处理"));
                list.Add(new SqlPara("FXRGS", a));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_AbnormalRegistration", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    List<SqlPara> list1 = new List<SqlPara>();
                    list1.Add(new SqlPara("BillNo", billNo));
                    SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_IsFBlistandIsFBlisttwo", list1);
                    DataSet ds = SqlHelper.GetDataSet(sps1);
                    if (!(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0))
                    {
                        List<SqlPara> listZQTMS = new List<SqlPara>();
                        listZQTMS.Add(new SqlPara("id",id));
                        listZQTMS.Add(new SqlPara("BillNo", billNo));
                        listZQTMS.Add(new SqlPara("baddate", CommonClass.gcdate));
                        listZQTMS.Add(new SqlPara("badcontent", userName + ":" + badcontent.Text.Trim() + "--" + CommonClass.gcdate + "--发现人1：" + discoverMan1.Text.Trim()));
                        listZQTMS.Add(new SqlPara("badcreateby", badcreateby.Text.Trim()));
                        listZQTMS.Add(new SqlPara("registerDeprtment", registerDeprtment.Text.Trim()));
                        listZQTMS.Add(new SqlPara("discoverMan1", discoverMan1.Text.Trim()));
                        listZQTMS.Add(new SqlPara("discoverMan2", discoverMan2.Text.Trim()));
                        listZQTMS.Add(new SqlPara("discoverMan3", discoverMan3.Text.Trim()));
                        listZQTMS.Add(new SqlPara("badtype", ExceType1.Text.Trim()));
                        listZQTMS.Add(new SqlPara("badtypeTwo", ExceType2.Text.Trim()));
                        listZQTMS.Add(new SqlPara("ExceDepart", ExceDepart.Text.Trim()));
                        listZQTMS.Add(new SqlPara("zerenacc1", money));
                        listZQTMS.Add(new SqlPara("abnormalityState", "待处理"));
                        listZQTMS.Add(new SqlPara("FXRGS", a));
                        SqlParasEntity spsZQTMS = new SqlParasEntity(OperType.Execute, "USP_ADD_AbnormalRegistration", listZQTMS);
                        SqlHelper.ExecteNonQuery_ZQTMS(spsZQTMS);
                    }
                    FileUpload.AddUpLoadMoreImg(1, billNos, paths, userName);
                    MsgBox.ShowOK();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }





        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            save();
        }

        private void w_bad_tyd_add_sa_Load(object sender, EventArgs e)
        {
            //CommonClass.SetWeb(registerDeprtment, "全部", false);
            //CommonClass.SetWeb(ExceDepart,"全部",false);
            baddate.DateTime = CommonClass.gcdate;
            badcreateby.Text = CommonClass.UserInfo.UserName;
            registerDeprtment.Text = CommonClass.UserInfo.WebName;//反馈部门默认当前
            discoverMan1.Text = CommonClass.UserInfo.UserName;//反馈人默认当前
            //获得异常表数据
            setData();
            setweb();

            //页面接收运单号查询得到异常类型然后过滤掉现在的异常类型
            //try
            //{
            //    List<SqlPara> list = new List<SqlPara>();
            //    list.Add(new SqlPara("unit", billNo));
            //    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BAD_TYD_Q", list);
            //    DataSet ds = SqlHelper.GetDataSet(sps);

            //    if (ds == null || ds.Tables.Count == 0) return;
            //    foreach (var row in ds.Tables[0].Rows)
            //    {
                    
            //    }
            //    //ds.Tables[0];
            //}
            //catch (Exception ex)
            //{
            //    MsgBox.ShowException(ex);
            //}
        }
        //wwb9.14
        private void setweb()
        {
            #region
            //List<SqlPara> list = new List<SqlPara>();
            //list.Add(new SqlPara("BillNo",billNo));
            //SqlParasEntity spe = new SqlParasEntity(OperType.Query, "USP_Get_WEB_BILL", list);
            //DataSet ds = SqlHelper.GetDataSet(spe);
            //string begweb = ds.Tables[0].Rows[0]["begweb"].ToString();
            ////string SCDesWeb = ds.Tables[0].Rows[0]["SCDesWeb"].ToString();
            //string PickGoodsSite = ds.Tables[0].Rows[0]["PickGoodsSite"].ToString();
            //string TermtranWeb = ds.Tables[0].Rows[0]["TermtranWeb"].ToString();
            //string LoadWeb = ds.Tables[0].Rows[0]["LoadWeb"].ToString();
            //string LoadEweb = ds.Tables[0].Rows[0]["LoadEweb"].ToString();
            //string SCDesWeb = "";
            //List<SqlPara> list1 = new List<SqlPara>();
            //list1.Add(new SqlPara("BillNO", billNo));
            //SqlParasEntity spe1 = new SqlParasEntity(OperType.Query, "QSP_GET_SHORTCONN_ByBillNO_New", list1);
            //DataSet ds1 = SqlHelper.GetDataSet(spe1);
            ////2017.11.20
            //List<SqlPara> list2 = new List<SqlPara>();
            //list2.Add(new SqlPara("BillNO", billNo));
            //SqlParasEntity spe2 = new SqlParasEntity(OperType.Query, "QSP_Get_billSendGoods_SendWeb_bybillno", list2);
            //DataSet ds2 = SqlHelper.GetDataSet(spe2);
            //string SendWeb="";
            ////2017.11.21
            //List<SqlPara> list3 = new List<SqlPara>();
            //list3.Add(new SqlPara("BillNO", billNo));
            //SqlParasEntity spe3 = new SqlParasEntity(OperType.Query, "QSP_GET_billDepartureList_ByBillNO", list3);
            //DataSet ds3 = SqlHelper.GetDataSet(spe3);
            //string AcceptWebName = "";
            //if (ds3 != null &&ds3.Tables[0].Rows.Count!=0)
            //{
            //    AcceptWebName = ds3.Tables[0].Rows[0]["AcceptWebName"].ToString();
            //}
            //if (begweb != "")
            //{
            //    ExceDepart.Properties.Items.Add(begweb);
            //}
            //if ( SCDesWeb != begweb)
            //{
            //    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            //    {
            //        SCDesWeb = ds1.Tables[0].Rows[i]["SCDesWeb"].ToString();
            //        ExceDepart.Properties.Items.Add(SCDesWeb);
            //    }
            //}
            ////2017.11.20wbw
            //if (SendWeb != begweb && SendWeb != SCDesWeb)
            //{
            //    for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
            //    {
            //        SendWeb = ds2.Tables[0].Rows[i]["SendWeb"].ToString();
            //        ExceDepart.Properties.Items.Add(SendWeb);
            //    }
            //}
            //if (AcceptWebName != "" && AcceptWebName != begweb && AcceptWebName != SCDesWeb && AcceptWebName != SendWeb)
            //{
            //    ExceDepart.Properties.Items.Add(AcceptWebName);
            //}

            //if (PickGoodsSite != "" && PickGoodsSite != begweb && PickGoodsSite != SCDesWeb && PickGoodsSite != SendWeb && PickGoodsSite != AcceptWebName)
            //{
            //    ExceDepart.Properties.Items.Add(PickGoodsSite);
            //}
            //if (TermtranWeb != "" && TermtranWeb != begweb && TermtranWeb != SCDesWeb && TermtranWeb != PickGoodsSite && TermtranWeb != SendWeb && TermtranWeb != AcceptWebName)
            //{
            //    ExceDepart.Properties.Items.Add(TermtranWeb);
            //}
            //if (LoadWeb != "" && LoadWeb != begweb && LoadWeb != SCDesWeb && LoadWeb != PickGoodsSite && LoadWeb != TermtranWeb && LoadWeb != SendWeb && LoadWeb != AcceptWebName)
            //{
            //    ExceDepart.Properties.Items.Add(LoadWeb);
            //}
            ////if (LoadEweb != "" && LoadEweb != begweb && LoadEweb != SCDesWeb && LoadEweb != PickGoodsSite && LoadEweb != TermtranWeb && LoadEweb != LoadWeb && LoadEweb != SendWeb)
            ////{
            ////    ExceDepart.Properties.Items.Add(LoadEweb);
            ////}
            #endregion
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillNo", billNo));
            SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_ExceDepart_Trace", list);
            DataSet ds = SqlHelper.GetDataSet(spe);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return;
            }
            else
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ExceDepart.Properties.Items.Add(ds.Tables[0].Rows[i]["optwebname"].ToString());
                }
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            fmFileUploadM fm = new fmFileUploadM();
            fm.ishowdel = false;
            fm.UserName = CommonClass.UserInfo.UserName;
            fm.billNo = billNo;
            fm.ShowDialog();
            billNos = fm.billNos;
            paths = fm.paths;

            if (paths.Trim() != "")
                lblUpText.Text = "图片上传成功";
                
        }

        //DataSet ds = null;
        private void setData()
        {
            SqlParasEntity sql = new SqlParasEntity(OperType.Query, "USP_Get_badInfo_xm");
            ds = SqlHelper.GetDataSet(sql);
            DataRow[] rows = ds.Tables[0].Select("PID=0");//异常项目
            for (int i = 0; i < rows.Length; i++)
            {
                ExceType1.Properties.Items.Add(rows[i]["title"]);
            }
        }


        private void ExceType1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try
            {
                if (ds == null || ds.Tables.Count == 0) return;
                ExceType2.Text = "";
                ExceType2.Properties.Items.Clear();
                string xmname = ExceType1.Text;
                DataRow[] rows = ds.Tables[0].Select("title='" + xmname + "'and pID=0");

                DataRow[] rows1 = ds.Tables[0].Select("pid=" + rows[0]["ID"]);
                for (int i = 0; i < rows1.Length; i++)
                {
                    ExceType2.Properties.Items.Add(rows1[i]["title"]);//类型名称
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void ExceType2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ds == null || ds.Tables.Count == 0) return;
                string typename = ExceType2.Text;
                //DataRow[] rows = ds.Tables[0].Select("title='" + typename + "'");
                DataRow[] rows = ds.Tables[0].Select(string.Format("title='{0}'and pID<>{1}", typename, 0));
                if (rows.Length > 0)
                {
                    money = rows[0]["money"].ToString();
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

    }
}