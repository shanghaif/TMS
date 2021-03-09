using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using System.IO;
using System.Net;
using DevExpress.XtraEditors;

namespace ZQTMS.UI
{
    public partial class frmImageLookNew : BaseForm
    {
        public frmImageLookNew()
        {
            InitializeComponent();
        }
       public string billNo = "";
       public string signNo = "";
       public string ClaimWeb = "";
       public static string UpFileUrl = "http://ZQTMS.dekuncn.com:7020";
       string bdPath = System.Windows.Forms.Application.StartupPath + "/TempFile";
        private void frmImageLook_Load(object sender, EventArgs e)
        {
            init();
        }
        private void init()
        {
            try
            {
                //List<SqlPara> list = new List<SqlPara>();
                //list.Add(new SqlPara("SignNO", signNo));
                //SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLERRORSIGN_ByID", list);
                //DataSet ds = SqlHelper.GetDataSet(sps);

                //调用ZQTMS理赔审批信息
                DataSet ds = CommonSyn.GetZQTMSClaimMessage(signNo, "", "", "", "", "", "QSP_GET_BILLERRORSIGN_ByID");

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                DataRow dr = ds.Tables[0].Rows[0];
                if (dr["PicClaimReport"].ToString() != "")
                {
                    PicClaimReport.SizeMode = PictureBoxSizeMode.StretchImage;
                    PicClaimReport.BackgroundImageLayout = ImageLayout.Stretch;
                    string filename = dr["PicClaimReport"].ToString();
                    string bdFileName = bdPath + filename.Substring(filename.LastIndexOf("/"));
                    if (!Directory.Exists(bdPath)) Directory.CreateDirectory(bdPath);
                    if (!File.Exists(bdFileName))
                    {
                        WebClient wc = new WebClient();
                        wc.DownloadFile(UpFileUrl + filename, bdFileName);
                    }                   
                 
                    
                   this.PicClaimReport.Load(bdFileName);
                   btnPicClaimReport.Tag = dr["PicClaimReport"].ToString();
               
                }
                if (dr["PicBookingNote"].ToString() != "")
                {
                    PicBookingNote.SizeMode = PictureBoxSizeMode.StretchImage;
                    PicBookingNote.BackgroundImageLayout = ImageLayout.Stretch;
                    string filename = dr["PicBookingNote"].ToString();
                    string bdFileName = bdPath + filename.Substring(filename.LastIndexOf("/"));
                    if (!Directory.Exists(bdPath)) Directory.CreateDirectory(bdPath);
                    if (!File.Exists(bdFileName))
                    {
                        WebClient wc = new WebClient();
                        wc.DownloadFile(UpFileUrl + filename, bdFileName);
                    }       
                    this.PicBookingNote.Load(bdFileName);
                    btnPicBookingNote.Tag = dr["PicBookingNote"].ToString();
                    // lblPicBookingNote.Text = "已上传图片！";
                }
                if (dr["PicSignBill"].ToString() != "")
                {
                    PicSignBill.SizeMode = PictureBoxSizeMode.StretchImage;
                    PicSignBill.BackgroundImageLayout = ImageLayout.Stretch;
                    string filename = dr["PicSignBill"].ToString();
                    string bdFileName = bdPath + filename.Substring(filename.LastIndexOf("/"));
                    if (!Directory.Exists(bdPath)) Directory.CreateDirectory(bdPath);
                    if (!File.Exists(bdFileName))
                    {
                        WebClient wc = new WebClient();
                        wc.DownloadFile(UpFileUrl + filename, bdFileName);
                    }       
                    this.PicSignBill.Load(bdFileName);
                    btnPicSignBill.Tag = dr["PicSignBill"].ToString();
                }
                if (dr["PicIdentityCard"].ToString() != "")
                {
                    PicIdentityCard.SizeMode = PictureBoxSizeMode.StretchImage;
                    PicIdentityCard.BackgroundImageLayout = ImageLayout.Stretch;
                    string filename = dr["PicIdentityCard"].ToString();
                    string bdFileName = bdPath + filename.Substring(filename.LastIndexOf("/"));
                    if (!Directory.Exists(bdPath)) Directory.CreateDirectory(bdPath);
                    if (!File.Exists(bdFileName))
                    {
                        WebClient wc = new WebClient();
                        wc.DownloadFile(UpFileUrl + filename, bdFileName);
                    }       
                    this.PicIdentityCard.Load(bdFileName);
                    btnPicIdentityCard.Tag = dr["PicIdentityCard"].ToString();
                }
                if (dr["PicPriceProve"].ToString() != "")
                {
                    PicPriceProve.SizeMode = PictureBoxSizeMode.StretchImage;
                    PicPriceProve.BackgroundImageLayout = ImageLayout.Stretch;
                    string filename = dr["PicPriceProve"].ToString();
                    string bdFileName = bdPath + filename.Substring(filename.LastIndexOf("/"));
                    if (!Directory.Exists(bdPath)) Directory.CreateDirectory(bdPath);
                    if (!File.Exists(bdFileName))
                    {
                        WebClient wc = new WebClient();
                        wc.DownloadFile(UpFileUrl + filename, bdFileName);
                    }       
                    this.PicPriceProve.Load(bdFileName);
                    btnPicPriceProve.Tag = dr["PicPriceProve"].ToString();
                }
                if (dr["PicDamagedGoods"].ToString() != "")
                {
                    PicDamagedGoods.SizeMode = PictureBoxSizeMode.StretchImage;
                    PicDamagedGoods.BackgroundImageLayout = ImageLayout.Stretch;
                    string filename = dr["PicDamagedGoods"].ToString();
                    string bdFileName = bdPath + filename.Substring(filename.LastIndexOf("/"));
                    if (!Directory.Exists(bdPath)) Directory.CreateDirectory(bdPath);
                    if (!File.Exists(bdFileName))
                    {
                        WebClient wc = new WebClient();
                        wc.DownloadFile(UpFileUrl + filename, bdFileName);
                    }       
                    this.PicDamagedGoods.Load(bdFileName);
                    btnPicDamagedGoods.Tag = dr["PicDamagedGoods"].ToString();
                }
                if (dr["PicGoodsBill"].ToString() != "")
                {
                    PicGoodsBill.SizeMode = PictureBoxSizeMode.StretchImage;
                    PicGoodsBill.BackgroundImageLayout = ImageLayout.Stretch;
                    string filename = dr["PicGoodsBill"].ToString();
                    string bdFileName = bdPath + filename.Substring(filename.LastIndexOf("/"));
                    if (!Directory.Exists(bdPath)) Directory.CreateDirectory(bdPath);
                    if (!File.Exists(bdFileName))
                    {
                        WebClient wc = new WebClient();
                        wc.DownloadFile(UpFileUrl + filename, bdFileName);
                    }       
                    this.PicGoodsBill.Load(bdFileName);
                    btnPicGoodsBill.Tag = dr["PicGoodsBill"].ToString();
                }
                if (dr["PicMaintainReport"].ToString() != "")
                {
                    PicMaintainReport.SizeMode = PictureBoxSizeMode.StretchImage;
                    PicMaintainReport.BackgroundImageLayout = ImageLayout.Stretch;
                    string filename = dr["PicMaintainReport"].ToString();
                    string bdFileName = bdPath + filename.Substring(filename.LastIndexOf("/"));
                    if (!Directory.Exists(bdPath)) Directory.CreateDirectory(bdPath);
                    if (!File.Exists(bdFileName))
                    {
                        WebClient wc = new WebClient();
                        wc.DownloadFile(UpFileUrl + filename, bdFileName);
                    }       
                    this.PicMaintainReport.Load(bdFileName);
                    btnPicMaintainReport.Tag = dr["PicMaintainReport"].ToString();
                }
                if (dr["PicIndemnityAgreement"].ToString() != "")
                {
                    PicIndemnityAgreement.SizeMode = PictureBoxSizeMode.StretchImage;
                    PicIndemnityAgreement.BackgroundImageLayout = ImageLayout.Stretch;
                    string filename = dr["PicIndemnityAgreement"].ToString();
                    string bdFileName = bdPath + filename.Substring(filename.LastIndexOf("/"));
                    if (!Directory.Exists(bdPath)) Directory.CreateDirectory(bdPath);
                    if (!File.Exists(bdFileName))
                    {
                        WebClient wc = new WebClient();
                        wc.DownloadFile(UpFileUrl + filename, bdFileName);
                    }       
                    this.PicIndemnityAgreement.Load(bdFileName);
                    btnAgreement.Tag = dr["PicIndemnityAgreement"].ToString();
                }
            }
            catch (Exception ex)
            {
                
                MsgBox.ShowException(ex);
            }
        }

        private void btnPicClaimReport_Click(object sender, EventArgs e)
        {
            SimpleButton button = (SimpleButton)sender;
            if (button.Tag == null || button.Tag.ToString() == "")
            {
                MsgBox.ShowOK("图片还未上传！");
                return;
            }
            try
            {
                string filename = button.Tag.ToString();

                if (!Directory.Exists(bdPath)) Directory.CreateDirectory(bdPath);
                string bdFileName = bdPath + filename.Substring(filename.LastIndexOf("/"));

                if (!File.Exists(bdFileName))
                {
                    WebClient wc = new WebClient();
                    wc.DownloadFile(UpFileUrl + filename, bdFileName);
                }

                if (!string.IsNullOrEmpty(bdFileName))
                {
                    FileStream fileStream = new FileStream(bdFileName, FileMode.Open, FileAccess.Read);

                    int byteLength = (int)fileStream.Length;
                    byte[] fileBytes = new byte[byteLength];
                    fileStream.Read(fileBytes, 0, byteLength);

                    //文件流关閉,文件解除锁定
                    fileStream.Close();
                    fileStream.Dispose();

                    // pictureBox1.Image = Image.FromStream(new MemoryStream(fileBytes));

                }

                frmShowPic frm = new frmShowPic();
                frm.imgPath = bdFileName;
                frm.Show();
            }
            catch (Exception ee)
            {
                MsgBox.ShowOK("打开失败。您的系统中没有合适的程序打开该文件!\r\n" + ee.Message);
            }
        }

        private void btnPicClaimReportM_Click(object sender, EventArgs e)
        {
            alterForm("lblPicClaimReport");
        }

        private void btnPicBookingNoteM_Click(object sender, EventArgs e)
        {
            alterForm("lblPicBookingNote");
        }

        private void btnPicSignBillM_Click(object sender, EventArgs e)
        {
            alterForm("lblPicSignBill");
        }

        private void btnPicIdentityCardM_Click(object sender, EventArgs e)
        {
            alterForm("lblPicIdentityCard");
        }

        private void btnPicPriceProveM_Click(object sender, EventArgs e)
        {
            alterForm("lblPicPriceProve");
        }

        private void btnPicDamagedGoodsM_Click(object sender, EventArgs e)
        {
            alterForm("lblPicDamagedGoods");
        }

        private void btnPicGoodsBillM_Click(object sender, EventArgs e)
        {
            alterForm("lblPicGoodsBill");
        }

        private void btnPicMaintainReportM_Click(object sender, EventArgs e)
        {
            alterForm("lblPicMaintainReport");
        }

        private void btnAgreementM_Click(object sender, EventArgs e)
        {
            alterForm("lblAgreement");
        }

        //点击上传按钮弹出窗体
        private void alterForm(string lblName)
        {
            //if (CommonClass.UserInfo.WebName != ClaimWeb)
            //{
            //    MsgBox.ShowOK("只有申请部门才能修改");
            //    return;
            //}
            //string paths = "";
            //fmFileUploadM fm = new fmFileUploadM();
            //fm.ishowdel = false;
            //fm.UserName = CommonClass.UserInfo.UserName;
            //fm.billNo = billNo;
            //fm.ShowDialog();
            //paths = fm.paths;
            //if (paths != "")
            //{
            //    paths = paths.Substring(0, paths.IndexOf('@'));
            //}
            //if (paths.Trim() != "")
            //{
            //    updateData(lblName, paths);
            //    init();
            //}

        }

        private void updateData(string lblName, string path)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("signNo", signNo));
                list.Add(new SqlPara("lblName", lblName));
                list.Add(new SqlPara("path", path));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USD_Update_ClaimPic", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                }
                else
                {
                    MsgBox.ShowError("操作失败！");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }
    }
}
