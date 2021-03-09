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

namespace ZQTMS.UI
{
    public partial class FrmLG : BaseForm
    {
        public FrmLG()
        {
            InitializeComponent();
        }
        //点击事件
        private void btn_start_Click(object sender, EventArgs e)
        {
            double GPSLat = 0;
            double GPSLng = 0;
            double MiddleLat = 0;
            double MiddleLon = 0;
            double Weight=0;
            double.TryParse(clat.Text.Trim(), out GPSLat);
            double.TryParse(clon.Text.Trim(), out GPSLng);
            double.TryParse(slat.Text.Trim(), out MiddleLat);
            double.TryParse(slon.Text.Trim(), out MiddleLon);
            double.TryParse(OperationWeight.Text.Trim(), out Weight);
            double dPrice = 0;
            double.TryParse(Price.Text.Trim(), out dPrice);
            double gl = HarvenSin.Distance(GPSLat, GPSLng, MiddleLat, MiddleLon);
            txt_lg.Text = gl.ToString();

            if (Weight <= 2500)
            {
                Weight = 2500;
            }

            if (comboBoxEdit1.SelectedIndex == 0)
            {
                double acc = (gl * (Weight / 1000) * dPrice);
                DeliveryFee.Text = acc.ToString();
            }
            else
            {
                double acc = (gl  * dPrice);
                DeliveryFee.Text = acc.ToString();
            }
        
            //decimal acc = ((decimal)gl * (Weight / 1000) * (decimal)Price);
            //gridView1.SetRowCellValue(0, "DeliveryFee", Math.Round(acc, 2));

        }

        private void FrmLG_Load(object sender, EventArgs e)
        {
            comboBoxEdit1.SelectedIndex = 0;
            txt_order.Focus();
            txt_order.SelectAll();
        }

        private void txt_order_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PickGoodsSite.Text = "";
                ReceivStreet.Text = "";
                LowPrice.Text = "";
                clat.Text = "";
                clon.Text ="";
                slat.Text = "";
                slon.Text = "";
                OperationWeight.Text = "";
                txt_lg.Text = "";
                DeliveryFee.Text = "";
                if (!string.IsNullOrEmpty(txt_order.Text.Trim()))
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("billno", txt_order.Text.Trim()));
                    if (comboBoxEdit1.SelectedIndex == 0)
                    {
                        list.Add(new SqlPara("sendtype", 0));
                    }
                    else
                    {
                        list.Add(new SqlPara("sendtype", 1));
                    }

                    //list.Add(new SqlPara("FileName", _FileName));
                    //list.Add(new SqlPara("FileNameZip", _FileNameZip));
                    //list.Add(new SqlPara("AppPath", _AppPath));
                    //list.Add(new SqlPara("FileMD5", _FileMD5));
                    //list.Add(new SqlPara("FileLen", _FileLen));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BillLG", list);
                    DataSet ds = SqlHelper.GetDataSet(sps);
                    if (ds.Tables.Count > 0)
                    {
                        PickGoodsSite.Text = ds.Tables[0].Rows[0]["PickGoodsSite"].ToString();
                        ReceivStreet.Text = ds.Tables[0].Rows[0]["ReceivStreet"].ToString();
                        Price.Text = ds.Tables[0].Rows[0]["Price"].ToString();
                        LowPrice.Text = ds.Tables[0].Rows[0]["LowPrice"].ToString();
                        clat.Text = ds.Tables[0].Rows[0]["GPSLat"].ToString();
                        clon.Text = ds.Tables[0].Rows[0]["GPSLng"].ToString();
                        slat.Text = ds.Tables[0].Rows[0]["MiddleLat"].ToString();
                        slon.Text = ds.Tables[0].Rows[0]["MiddleLon"].ToString();
                        OperationWeight.Text = ds.Tables[0].Rows[0]["OperationWeight"].ToString();

                    }

                }




            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
