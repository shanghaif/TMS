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
    public partial class fmBadBillAdd : BaseForm
    {
        DataSet ds = new DataSet();

        public string billNo = "";
        public string billNos = "";
        public string paths = "";
        public fmBadBillAdd()
        {
            InitializeComponent();
        }


        private void save()
        {
            
            if (fangkuiwebid.Text.Trim() == "")
            {
                MsgBox.ShowOK("必须填写反馈部门！");
                fangkuiwebid.Focus();
                return;
            }
            if (fangkuiman.Text.Trim() == "")
            {
                MsgBox.ShowOK("必须填写反馈人！");
                fangkuiman.Focus();
                return;
            }

            if (ExceType.Text.Trim()=="")
            {
                MsgBox.ShowOK("必须填写异常项目！");
                ExceType.Focus();
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
                MsgBox.ShowOK("必须填写异常部门！");
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


            try
            {
                string userName = CommonClass.UserInfo.UserName;
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", billNo));
                list.Add(new SqlPara("baddate", CommonClass.gcdate));
                list.Add(new SqlPara("badcontent", userName + ":" + badcontent.Text.Trim() + "--" + CommonClass.gcdate + "--反馈人1：" + fangkuiman.Text.Trim()));
                list.Add(new SqlPara("badcreateby", badcreateby.Text.Trim()));
                list.Add(new SqlPara("fangkuiwebid", fangkuiwebid.Text.Trim()));
                list.Add(new SqlPara("fangkuiman", fangkuiman.Text.Trim()));
                list.Add(new SqlPara("ExceType", ExceType.Text.Trim()));
                list.Add(new SqlPara("ExceType2", ExceType2.Text.Trim()));//plh
                list.Add(new SqlPara("ExceDepart", ExceDepart.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BAD_TYD_SA", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    FileUpload.AddUpLoadMoreImg(1, billNos, paths, userName);
                    //异常登记同步到ZQTMS
                    Common.CommonSyn.AbnormalSyn(billNo);
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
            string userName = CommonClass.UserInfo.UserName;
            CommonClass.SetWeb(fangkuiwebid, "全部",false);
            setData();//plh
            //ZQTMS转分波过来的默认责任部门是开单部门 hj20180510
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query,"QSP_WAYBILL_FB_STATE",new List<SqlPara>(){ new SqlPara("billNo", billNo) }));
            DataSet ds1 = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_WAYBILL_FB_STATE_1", new List<SqlPara>() { new SqlPara("billNo", billNo) }));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //ExceDepart.Text = ds.Tables[1].Rows[0]["BegWeb"].ToString();     //whf20190430	注释
                //whf20190430     add
                ExceDepart.Properties.Items.Clear();
                foreach (DataRow item in ds.Tables[1].Rows)
                {
                    ExceDepart.Properties.Items.Add(item["BegWeb"]);
                }
            }
            else
            {
                CommonClass.SetWeb(ExceDepart, "全部", false);
                if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[1].Rows[0]["PickGoodsSite"].ToString() != "")
                    {
                        //ExceDepart.Properties.Items.Add(ds.Tables[1].Rows[0]["PickGoodsSite"].ToString());     //whf20190430	注释
                        //whf20190430     add
                        ExceDepart.Properties.Items.Clear();
                        foreach (DataRow item in ds.Tables[1].Rows)
                        {
                            ExceDepart.Properties.Items.Add(item["PickGoodsSite"]);
                        }
                    }
                }
            }
            baddate.DateTime = CommonClass.gcdate;
            badcreateby.Text = userName;
            fangkuiwebid.Text = CommonClass.UserInfo.WebName;//反馈部门默认当前
            fangkuiman.Text = CommonClass.UserInfo.UserName;//反馈人默认当前
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

        private void setData()
        {
            SqlParasEntity sql = new SqlParasEntity(OperType.Query, "USP_Get_badInfo_xm");
            ds = SqlHelper.GetDataSet(sql);
            DataRow[] rows = ds.Tables[0].Select("PID=0");//异常项目
            for (int i = 0; i < rows.Length; i++)
            {
                ExceType.Properties.Items.Add(rows[i]["title"]);
            }
        }



        private void ExceType1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try
            {
                if (ds == null || ds.Tables.Count == 0) return;
                ExceType2.Text = "";
                ExceType2.Properties.Items.Clear();
                string xmname = ExceType.Text;
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
                //if (rows.Length > 0)
                //{
                //    money = rows[0]["money"].ToString();
                //}

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }



    }
}