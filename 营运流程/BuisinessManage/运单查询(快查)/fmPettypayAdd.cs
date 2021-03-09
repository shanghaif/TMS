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
                MsgBox.ShowOK("������д�������ţ�");
                fangkuiwebid.Focus();
                return;
            }
            if (fangkuiman.Text.Trim() == "")
            {
                MsgBox.ShowOK("������д�����ˣ�");
                fangkuiman.Focus();
                return;
            }

            if (ExceType.Text.Trim()=="")
            {
                MsgBox.ShowOK("������д�쳣���ͣ�");
                ExceType.Focus();
                return;
            }
            if (ExceDepart.Text.Trim() == "")
            {
                MsgBox.ShowOK("������д�쳣���ţ�");
                ExceDepart.Focus();
                return;
            }
            if (badcontent.Text.Trim() == "")
            {
                MsgBox.ShowOK("������д�쳣���ݣ�");
                badcontent.Focus();
                return;
            }
            //if (paths.Trim() == "")
            //{
            //    MsgBox.ShowOK("ͼƬδ�ϴ���");
            //    return;
            //}
            if (ConvertType.ToDecimal(FetchPay.Text) < ConvertType.ToDecimal(Pettypay.Text))
            {
                MsgBox.ShowOK("�쳣���ܴ����Ḷ�");
                Pettypay.Focus();
                return;
            }
            if (ConvertType.ToDecimal(Pettypay.Text) > 500)
            {
                MsgBox.ShowOK("�쳣���ܴ���500��");
                Pettypay.Focus();
                return;
            }
            if (ConvertType.ToDecimal(Pettypay.Text) < 0)
            {
                MsgBox.ShowOK("�쳣�������0��");
                Pettypay.Focus();
                return;
            }


            try
            {
                string userName = CommonClass.UserInfo.UserName;
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", billNo));
                list.Add(new SqlPara("baddate", CommonClass.gcdate));
                list.Add(new SqlPara("badcontent", userName + ":" + badcontent.Text.Trim() + "--" + CommonClass.gcdate + "--������1��" + fangkuiman.Text.Trim()));
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
            CommonClass.SetWeb(fangkuiwebid, "ȫ��",false);
            CommonClass.SetWeb(ExceDepart,"ȫ��",false);
            baddate.DateTime = CommonClass.gcdate;
            badcreateby.Text = userName;
            fangkuiwebid.Text = CommonClass.UserInfo.WebName;//��������Ĭ�ϵ�ǰ
            fangkuiman.Text = CommonClass.UserInfo.UserName;//������Ĭ�ϵ�ǰ
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
                lblUpText.Text = "ͼƬ�ϴ��ɹ�";
        }



    }
}