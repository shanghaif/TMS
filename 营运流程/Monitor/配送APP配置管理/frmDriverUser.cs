using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmDriverUser : BaseForm
    {
        public frmDriverUser()
        {
            InitializeComponent();
        }
        private void frmDriverUser_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("司机信息");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
            BarMagagerOper.SetBarPropertity(bar3);
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private void getdata()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("username", username.Text == "全部" ? "%%" : username.Text.Trim()));
              
                if (checkstate.Text == "未审核")
                {
                    list.Add(new SqlPara("checkstate", 0));
                }
                if (checkstate.Text == "已审核")
                {
                    list.Add(new SqlPara("checkstate", 1));
                }
                if (checkstate.Text == "全部")
                {
                    list.Add(new SqlPara("checkstate", 2));
                }
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "USP_GET_userinfo", list);
                DataSet ds = SqlHelper.GetDataSet(spe);
                myGridControl1.DataSource = ds.Tables[0];
                
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //审核 
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0)
            {
                MsgBox.ShowOK("请选择一条信息！！！");
                return;
            }
            string userid = myGridView1.GetRowCellValue(rowhandle, "userid").ToString();
            string chauffeurxz = myGridView1.GetRowCellValue(rowhandle, "chauffeurxz").ToString();
            string settleinterval = myGridView1.GetRowCellValue(rowhandle, "settleinterval").ToString();
            panelControl1.Visible = true;
            textEdit2.Text = settleinterval;
            CauseName.Text = chauffeurxz;
            label5.Text = userid;
            //try
            //{
            //    List<SqlPara> list = new List<SqlPara>();
            //    list.Add(new SqlPara("userid", userid));
            //    SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "USP_UPDATE_userinfo", list);
            //    if (SqlHelper.ExecteNonQuery(spe) > 0)
            //    {
            //        MsgBox.ShowOK();
            //        getdata();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MsgBox.ShowException(ex);
            //}
        }

        //取消审核
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0)
            {
                MsgBox.ShowOK("请选择一条信息！！！");
                return;
            }
            if (MsgBox.ShowYesNo("是否取消审核？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
            {
                return;
            }
            string userid = myGridView1.GetRowCellValue(rowhandle, "userid").ToString();
            string username = myGridView1.GetRowCellValue(rowhandle, "username").ToString();
            panelControl5.Visible = true;
            textEdit1.Text = username;
            label4.Text = userid;
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            panelControl5.Visible = false;
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("userid", label4.Text.Trim()));
                list.Add(new SqlPara("checkremark", checkremark.Text.Trim()));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "USP_UPDATE_userinfo_QXSH", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    MsgBox.ShowOK();
                    panelControl5.Visible = false;
                    label4.Text = "";
                    checkremark.Text = "";
                    textEdit1.Text = "";
                    getdata();
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //新增
        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //int rowhandle = myGridView1.FocusedRowHandle;
            //if (rowhandle < 0)
            //{
            //    MsgBox.ShowOK("请选择一条信息！！！");
            //    return;
            //}
            //string userid = myGridView1.GetRowCellValue(rowhandle, "userid").ToString();
            //string username = myGridView1.GetRowCellValue(rowhandle, "username").ToString();
            //string vehicleno = myGridView1.GetRowCellValue(rowhandle, "vehicleno").ToString();
            //string cyzgno = myGridView1.GetRowCellValue(rowhandle, "cyzgno").ToString();
            //string CarAscription = myGridView1.GetRowCellValue(rowhandle, "CarAscription").ToString();
            //string Province = myGridView1.GetRowCellValue(rowhandle, "Province").ToString();
            //string settleinterval = myGridView1.GetRowCellValue(rowhandle, "settleinterval").ToString();
            //string chauffeurxz= myGridView1.GetRowCellValue(rowhandle, "chauffeurxz").ToString();
            frmDriverUserADD frm = new frmDriverUserADD("新增");
            //frm.ID = userid;
            //frm.username1 = username;
            //frm.vehicleno1 = vehicleno;
            //frm.cyzgno1 = cyzgno;
            //frm.CarAscription1 = CarAscription;
            //frm.Province1 = Province;
            //frm.settle1 = settleinterval;
            //frm.chauffeur1 = chauffeurxz;
            frm.ShowDialog();
            getdata();
        }


        //修改
        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0)
            {
                MsgBox.ShowOK("请选择一条信息！！！");
                return;
            }
            string userid = myGridView1.GetRowCellValue(rowhandle, "userid").ToString();
            string username = myGridView1.GetRowCellValue(rowhandle, "username").ToString();
            string usermb = myGridView1.GetRowCellValue(rowhandle, "usermb").ToString();
            string vehicleno = myGridView1.GetRowCellValue(rowhandle, "vehicleno").ToString();
            string cyzgno = myGridView1.GetRowCellValue(rowhandle, "cyzgno").ToString();
            string CarAscription = myGridView1.GetRowCellValue(rowhandle, "CarAscription").ToString();
            string Province = myGridView1.GetRowCellValue(rowhandle, "Province").ToString();
            string settleinterval = myGridView1.GetRowCellValue(rowhandle, "settleinterval").ToString();
            string chauffeurxz = myGridView1.GetRowCellValue(rowhandle, "chauffeurxz").ToString();
            frmDriverUserADD frm = new frmDriverUserADD("修改");
            frm.ID = userid;
            frm.username1 = username;
            frm.usermb1 = usermb;
            frm.vehicleno1 = vehicleno;
            frm.cyzgno1 = cyzgno;
            frm.CarAscription1 = CarAscription;
            frm.Province1 = Province;
            frm.settle1 = settleinterval;
            frm.chauffeur1 = chauffeurxz;
            frm.ShowDialog();
            getdata();
        }


        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (textEdit2.Text.Trim() == "")
                {
                    MsgBox.ShowOK("请填写结算周期!");
                    textEdit2.Focus();
                    return;
                }

                if (CauseName.Text.Trim() == "")
                {
                    MsgBox.ShowOK("请填写车辆性质!");
                    CauseName.Focus();
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                ;
                list.Add(new SqlPara("userid", label5.Text.Trim()));
                list.Add(new SqlPara("settleinterval", textEdit2.Text.Trim()));
                list.Add(new SqlPara("chauffeurxz", CauseName.Text.Trim()));
                //CauseName.SelectedIndex = 0;
                //CauseName.Text = "临时外请";
                //CauseName.Properties.Items.Add("临时外请");
                //CauseName.Properties.Items.Add("挂靠");
                //CauseName.Properties.Items.Add("合同");
                //CauseName.Properties.Items.Add("长期外请");
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "USP_UPDATE_userinfo", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    MsgBox.ShowOK();
                    panelControl1.Visible = false;
                    textEdit2.Text = "";
                    CauseName.Text = "";
                    label5.Text = "";
                    getdata();
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            panelControl1.Visible = false;
        }
    }
}