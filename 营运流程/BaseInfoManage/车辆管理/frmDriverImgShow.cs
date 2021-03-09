using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class frmDriverImgShow : BaseForm
    {
        public frmDriverImgShow()
        {
            InitializeComponent();
        }
        private string imageUtl = "http://ZQTMS.dekuncn.com:7020";
        private string _driverNo;
        public frmDriverImgShow(string driverNo)
        {
            InitializeComponent();
            _driverNo = driverNo;
        }

        private void frmDriverImgShow_Load(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DriverId", _driverNo));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_DriverImage_SHOW", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //清除老图片
                    imageList1.Images.Clear();
                    listView1.Items.Clear();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            string type = ds.Tables[0].Rows[i]["imagetype"].ToString();
                            if (type.Contains("证"))
                            {
                                imageList1.Images.Add(Image.FromStream(System.Net.WebRequest.Create(imageUtl + ds.Tables[0].Rows[i]["FilePath"].ToString() + "").GetResponse().GetResponseStream()), Color.Transparent);
                                this.listView1.View = View.LargeIcon;
                                this.listView1.LargeImageList = this.imageList1;
                                this.listView1.BeginUpdate();
                            }
                            else if (type.Contains("险保"))
                            {
                                imageList2.Images.Add(Image.FromStream(System.Net.WebRequest.Create(imageUtl + ds.Tables[0].Rows[i]["FilePath"].ToString() + "").GetResponse().GetResponseStream()), Color.Transparent);
                                this.listView2.View = View.LargeIcon;
                                this.listView2.LargeImageList = this.imageList2;
                                this.listView2.BeginUpdate();
                            }
                        }
                        catch { }
                    }

                    int image2index = 0;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DriverImage di = new DriverImage();
                        di.Id = ds.Tables[0].Rows[i]["Id"].ToString();
                        di.chaufferno = Convert.ToInt16(ds.Tables[0].Rows[i]["chaufferno"]);
                        di.chauffername = ds.Tables[0].Rows[i]["chauffername"].ToString();
                        di.FilePath = ds.Tables[0].Rows[i]["FilePath"].ToString();
                        di.OperMan = ds.Tables[0].Rows[i]["OperMan"].ToString();
                        di.OperDate = ds.Tables[0].Rows[i]["OperDate"].ToString();
                        di.imagetype = ds.Tables[0].Rows[i]["imagetype"].ToString();

                        if (ds.Tables[0].Rows[i]["imagetype"].ToString().Contains("证"))
                        {
                            ListViewItem lvi = new ListViewItem();
                            lvi.ImageIndex = i;
                            lvi.Text = "证件类型：" + ds.Tables[0].Rows[i]["imagetype"].ToString() + "\r\n" + "上传日期：" + ds.Tables[0].Rows[i]["OperDate"].ToString();
                            lvi.Tag = di;
                            this.listView1.Items.Add(lvi);
                            this.listView1.EndUpdate();
                        }
                        else if (ds.Tables[0].Rows[i]["imagetype"].ToString().Contains("险保"))
                        {
                            ListViewItem lvi_1 = new ListViewItem();
                            lvi_1.ImageIndex = image2index;
                            lvi_1.Text = "证件类型：" + ds.Tables[0].Rows[i]["imagetype"].ToString() + "\r\n" + "上传日期：" + ds.Tables[0].Rows[i]["OperDate"].ToString();
                            lvi_1.Tag = di;
                            this.listView2.Items.Add(lvi_1);
                            this.listView2.EndUpdate();

                            image2index++;
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

    }

    public class DriverImage
    {
        public string Id { get; set; }
        public int chaufferno { get; set; }
        public string chauffername { get; set; }
        public string FilePath { get; set; }
        public string OperMan { get; set; }
        public string OperDate { get; set; }
        public string imagetype { get; set; }
    }
}