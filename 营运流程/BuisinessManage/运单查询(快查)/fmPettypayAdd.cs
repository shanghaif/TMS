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
    public partial class fmPettypayAdd : BaseForm
    {
        DataSet ds = new DataSet();

        public string billNo = "";
        public string billNos = "";
        public string paths = "";
        public fmPettypayAdd()
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
                MsgBox.ShowOK("必须填写异常类型！");
                ExceType.Focus();
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
            if (ConvertType.ToDecimal(FetchPay.Text) < ConvertType.ToDecimal(Pettypay.Text))
            {
                MsgBox.ShowOK("异常金额不能大于提付款！");
                Pettypay.Focus();
                return;
            }
            if (ConvertType.ToDecimal(Pettypay.Text) > 500)
            {
                MsgBox.ShowOK("异常金额不能大于500！");
                Pettypay.Focus();
                return;
            }
            if (ConvertType.ToDecimal(Pettypay.Text) < 0)
            {
                MsgBox.ShowOK("异常必须大于0！");
                Pettypay.Focus();
                return;
            }


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
                list.Add(new SqlPara("ExceDepart", ExceDepart.Text.Trim()));
                list.Add(new SqlPara("Pettypay", ConvertType.ToDecimal(Pettypay.Text)));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BillPettypay", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    FileUpload.AddUpLoadMoreImg(2, billNos, paths, userName);
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
            CommonClass.SetWeb(ExceDepart,"全部",false);
            baddate.DateTime = CommonClass.gcdate;
            badcreateby.Text = userName;
            fangkuiwebid.Text = CommonClass.UserInfo.WebName;//反馈部门默认当前
            fangkuiman.Text = CommonClass.UserInfo.UserName;//反馈人默认当前
            getFetchPay();
        }

        private void getFetchPay()
        {
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_FetchPay", new List<SqlPara> { new SqlPara("BillNo", billNo) }));
            if (ds.Tables[0].Rows.Count > 0)
            {
                FetchPay.Text = ds.Tables[0].Rows[0]["FetchPay"].ToString();
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            fmFileUploadS fm = new fmFileUploadS();
            fm.ishowdel = false;
            fm.UserName = CommonClass.UserInfo.UserName;
            fm.billNo = billNo;
            fm.ShowDialog();
            billNos = fm.billNos;
            paths = fm.paths;

            if (paths.Trim() != "")
                lblUpText.Text = "图片上传成功";
        }



    }
}