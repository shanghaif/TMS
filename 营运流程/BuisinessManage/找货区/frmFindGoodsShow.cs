using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using System.Threading;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using DevExpress.XtraEditors;

namespace ZQTMS.UI
{
    public partial class frmFindGoodsShow : BaseForm
    {
        public frmFindGoodsShow()
        {
            InitializeComponent();
        }

        private string imageUtl = "http://ZQTMS.dekuncn.com:7020";
        private int PageIndex = 1;

        private void frmFindGoodsShow_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("无标货图片");//xj/2019/5/28
            bdate.DateTime = CommonClass.gbdate.AddDays(-3);
            edate.DateTime = CommonClass.gedate;
            CommonClass.SetCause(FromCause, true);

            //getdata();
        }

        private void getdata()
        {
            string varieties = Varieties.Text.Trim();
            string package = Package.Text.Trim();
            int num = ConvertType.ToInt32(Num.Text);
            varieties = varieties == "" ? "%%" : "%" + varieties + "%";
            package = package == "" ? "%%" : "%" + package + "%";
            string fromCause = FromCause.Text.Trim();
            string fromArea = FromArea.Text.Trim();
            string fromWeb = FromWeb.Text.Trim();
            fromCause = fromCause == "" || fromCause == "全部" ? "%%" : fromCause;
            fromArea = fromArea == "" || fromArea == "全部" ? "%%" : fromArea;
            fromWeb = fromWeb == "" || fromWeb == "全部" ? "%%" : fromWeb;

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("PageIndex", PageIndex));
                list.Add(new SqlPara("PageSize", 20));
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("FromCause", fromCause));
                list.Add(new SqlPara("FromArea", fromArea));
                list.Add(new SqlPara("FromWeb", fromWeb));
                list.Add(new SqlPara("Varieties", varieties));
                list.Add(new SqlPara("Package", package));
                list.Add(new SqlPara("Num", num));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_FindGoodsImage_SHOW", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //清除老图片
                    imageList1.Images.Clear();
                    listView1.Items.Clear();

                    lblPageInfo.Text = "第" + PageIndex + "页 共" + ds.Tables[0].Rows[0]["TotalPage"] + "页 " + ds.Tables[0].Rows[0]["TotalCount"] + "张图片";
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            imageList1.Images.Add(Image.FromStream(System.Net.WebRequest.Create(imageUtl + ds.Tables[0].Rows[i]["FileThPath"].ToString() + "").GetResponse().GetResponseStream()), Color.Transparent);
                        }
                        catch { }
                    }
                    this.listView1.View = View.LargeIcon;
                    this.listView1.LargeImageList = this.imageList1;
                    this.listView1.BeginUpdate();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        GoodsImage gi = new GoodsImage();
                        gi.Id = ds.Tables[0].Rows[i]["Id"].ToString();
                        gi.FileThPath = ds.Tables[0].Rows[i]["FileThPath"].ToString();
                        gi.GoodsId = ds.Tables[0].Rows[i]["GoodsId"].ToString();
                        gi.FilePath = ds.Tables[0].Rows[i]["FilePath"].ToString();
                        gi.OperMan = ds.Tables[0].Rows[i]["OperMan"].ToString();
                        gi.OperDate = ds.Tables[0].Rows[i]["OperDate"].ToString();
                        gi.Varieties = ds.Tables[0].Rows[i]["Varieties"].ToString();
                        gi.Package = ds.Tables[0].Rows[i]["Package"].ToString();
                        ListViewItem lvi = new ListViewItem();
                        lvi.ImageIndex = i;
                        lvi.Text = "品名：" + ds.Tables[0].Rows[i]["Varieties"] + " 包装：" + ds.Tables[0].Rows[i]["Package"] + "\r\n" + "发现日期：" + ConvertType.ToDateTime(ds.Tables[0].Rows[i]["OperDate"]).ToString("yyyy-MM-dd HH:mm");
                        lvi.Tag = gi;
                        this.listView1.Items.Add(lvi);
                    }
                    this.listView1.EndUpdate();
                }
                else
                {
                    if (PageIndex > 1) PageIndex--;
                    MsgBox.ShowOK("亲，已经没有图片了!");
                    return;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            PageIndex++;
            getdata();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (PageIndex == 1)
            {
                MsgBox.ShowOK("没有上一页了!");
                return;
            }
            PageIndex--;
            getdata();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                ListViewItem lvi = listView1.FocusedItem;
                GoodsImage gi = (GoodsImage)lvi.Tag;
                frmShowBigImg fsb = new frmShowBigImg();
                fsb.url = imageUtl + gi.FilePath;
                fsb.gi = gi;
                fsb.ShowDialog();
                if (fsb.isok == 1)
                {
                    listView1.Items.Clear();
                    getdata();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            PageIndex = 1;
            getdata();
        }

        private void FromCause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(FromArea, FromCause.Text);
            CommonClass.SetCauseWeb(FromWeb, FromCause.Text, FromArea.Text);
        }

        private void FromArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(FromWeb, FromCause.Text, FromArea.Text);
        }

        /// <summary>
        /// 筛选条件框的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectCondition_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                (sender as ComboBoxEdit).SelectAll();
            }
            catch { }
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int p = ConvertType.ToInt32(page.Text);
            if (p <= 0) return;
            PageIndex = p;
            getdata();
        }
    }

    public class GoodsImage
    {
        public string Id { set; get; }
        public string FileThPath { set; get; }
        public string GoodsId { set; get; }
        public string FilePath { set; get; }
        public string OperMan { set; get; }
        public string OperDate { set; get; }
        public string Varieties { set; get; }
        public string Package { set; get; }
    }
}