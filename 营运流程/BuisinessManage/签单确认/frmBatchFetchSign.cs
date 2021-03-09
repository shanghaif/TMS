using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace ZQTMS.UI
{
    public partial class frmBatchFetchSign : BaseForm
    {
        private DataSet ds = new DataSet();
        private DataSet ds1 = new DataSet();
        private string btnName = "";
        string billno = "";
        GridHitInfo hitInfo = null;
        sms frm = new sms();
        public string sFrmName = "";
        public int isLocal = 0;

        static frmBatchFetchSign fbfs;

        public static frmBatchFetchSign Get_frmBatchFetchSign { get { if (fbfs == null || fbfs.IsDisposed)fbfs = new frmBatchFetchSign(); return fbfs; } }

        public frmBatchFetchSign()
        {
            InitializeComponent();
            barEditItem1.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem1_EditValueChanging);
            barEditItem1.Edit.KeyDown += new KeyEventHandler(barEditItem1_KeyDown);
            barEditItem2.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem2_EditValueChanging);
            barEditItem2.Edit.KeyDown += new KeyEventHandler(barEditItem2_KeyDown);
        }

        #region 身份证相关
        int USB_State = 0;//身份证设备状态
        static StringBuilder cName = new StringBuilder(200); //姓名
        static StringBuilder Gender = new StringBuilder(200); //性别
        static StringBuilder Folk = new StringBuilder(200); //民族
        static StringBuilder BirthDay = new StringBuilder(200);//出生日期
        static StringBuilder Code = new StringBuilder(200);//身份证号
        static StringBuilder Address = new StringBuilder(200); //地址
        static StringBuilder Agency = new StringBuilder(200);//签证机关
        static StringBuilder ExpireStart = new StringBuilder(200); //有效期起始
        static StringBuilder ExpireEnd = new StringBuilder(200); //有效期截至

        static byte[] photo;
        string path = Application.StartupPath + @"\1.jpg";
        #endregion

        #region ValidateCode
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                //需要4个文件：SavePhoto.dll   sdtapi.dll   JpgDll.dll    Dewlt.dll
                if (!File.Exists(Application.StartupPath + "\\sdtapi.dll"))
                {
                    timer1.Enabled = false;
                    return;
                }
                if (USB_State == 0)
                {
                    if (IdentityCard.InitComm(1001) == 1)
                    {
                        label33.Text = "身份证验证设备已就绪!";
                        USB_State = 1;
                        timer1.Interval = 500;
                        timer1.Enabled = false;
                        timer1.Enabled = true;
                    }
                    else
                    {
                        label33.Text = "身份证验证设备未连接好!";
                        USB_State = 0;
                    }
                }
                else
                {
                    int ret = IdentityCard.Authenticate();
                    if (ret == 1)   //找到其他卡 是否也这么提示?
                    {
                        int state = IdentityCard.ReadBaseInfos(cName, Gender, Folk, BirthDay, Code, Address, Agency, ExpireStart, ExpireEnd);
                        if (cName != null && cName.ToString().Trim() != "")
                        {
                            SignMan.Text = cName.ToString().Trim();
                            SignManCardID.Text = Code.ToString().Trim();
                        }
                        int a = 0;
                        while (true)
                        {
                            if (File.Exists(path))
                            {
                                if (a == 10)
                                {
                                    return;
                                }
                                using (FileStream stream = new FileInfo(path).OpenRead())
                                {
                                    photo = new byte[stream.Length];
                                    stream.Read(photo, 0, Convert.ToInt32(stream.Length));
                                    Image img = new Bitmap(stream);
                                    stream.Close();
                                    stream.Dispose();

                                    alertControl1.Show(this, "", "", img);
                                    timer1.Enabled = false;
                                    return;
                                }
                            }
                            a++;
                            Thread.Sleep(200);
                        }

                        //if (state == 1)
                        //{
                        //    string path = Application.StartupPath + @"\1.jpg";

                        //    int a = 0;
                        //    while (true)
                        //    {
                        //        if (File.Exists(path))
                        //        {
                        //            if (a == 10)
                        //            {
                        //                MsgBox.ShowOK("提取身份证识别信息失败!");
                        //                return;
                        //            }
                        //            FileStream stream = new FileInfo(path).OpenRead();
                        //            photo = new byte[stream.Length];
                        //            stream.Read(photo, 0, Convert.ToInt32(stream.Length));
                        //            Image img = new Bitmap(stream);
                        //            stream.Close();

                        //            SignMan.Text = cName.ToString().Trim();
                        //            SignManCardID.Text = Code.ToString().Trim();

                        //            alertControl1.Show(this, "", "", img);
                        //            timer1.Enabled = false;
                        //            return;
                        //        }
                        //        a++;
                        //        Thread.Sleep(200);
                        //    }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                timer1.Enabled = false;
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void frmBatchFetchSign_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                timer1.Enabled = false;
                IdentityCard.CloseComm();
                if (File.Exists(Application.StartupPath + @"\1.jpg"))
                {
                    File.Delete(Application.StartupPath + @"\1.jpg");
                    File.Delete(Application.StartupPath + @"\2.jpg");
                    File.Delete(Application.StartupPath + @"\photo.bmp");
                }
            }
            catch (Exception) { }
        }

        private void alertControl1_BeforeFormShow(object sender, DevExpress.XtraBars.Alerter.AlertFormEventArgs e)
        {
            e.AlertForm.Size = new Size(355, 220);
        }
        #endregion

        //查询方法
        private void getdata()
        {
            if (barEditItem8.EditValue.ToString() == "自提签收")
            {
                string proc = "QSP_GET_FETCH_FOR_SIGN_NEW";
                if (CommonClass.IsZhanQuCompanyId(CommonClass.UserInfo.companyid))
                {
                    proc = "QSP_GET_FETCH_FOR_SIGN_NEW_ZQ";
                }
                try
                {
                    ds.Clear();
                    ds1.Clear();
                    myGridView1.ClearColumnsFilter();
                    myGridView2.ClearColumnsFilter();

                    List<SqlPara> list = new List<SqlPara>();
                    if (CommonClass.IsZhanQuCompanyId(CommonClass.UserInfo.companyid))
                    {
                        list.Add(new SqlPara("webName", CommonClass.UserInfo.WebName));
                    }
                    else
                    {
                        list.Add(new SqlPara("webName", CommonClass.UserInfo.WebName == null ? "" : CommonClass.UserInfo.WebName));
                    }
                    list.Add(new SqlPara("siteName", CommonClass.UserInfo.SiteName == null ? "" : CommonClass.UserInfo.SiteName));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, proc, list);
                    ds = SqlHelper.GetDataSet(sps);
                    if (ds == null || ds.Tables.Count == 0) return;
                    ds1 = ds.Clone();
                    myGridControl1.DataSource = ds.Tables[0];
                    myGridControl2.DataSource = ds1.Tables[0];
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
            }
            else if (barEditItem8.EditValue.ToString() == "送货签收")       
            {
                string proc = "QSP_GET_SEND_FOR_SIGN_NEW";
                if (CommonClass.IsZhanQuCompanyId(CommonClass.UserInfo.companyid))
                {
                    proc = "QSP_GET_SEND_FOR_SIGN_NEW_ZQ";
                }
                try
                {
                    ds.Clear();
                    ds1.Clear();
                    myGridView1.ClearColumnsFilter();
                    myGridView2.ClearColumnsFilter();

                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("webName", CommonClass.UserInfo.WebName));
                    list.Add(new SqlPara("siteName", CommonClass.UserInfo.SiteName));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, proc, list);
                    ds = SqlHelper.GetDataSet(sps);
                    if (ds == null || ds.Tables.Count == 0) return;
                    ds1 = ds.Clone();
                    myGridControl1.DataSource = ds.Tables[0];
                    myGridControl2.DataSource = ds1.Tables[0];
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
            }

            else if (barEditItem8.EditValue.ToString() == "本地中转签收")       
            {


                string proc = "QSP_GET_MIDDLE_FOR_SIGN_NEW1";
                if (CommonClass.IsZhanQuCompanyId(CommonClass.UserInfo.companyid))
                {
                    proc = "QSP_GET_MIDDLE_FOR_SIGN_NEW1_ZQ";
                }
                try
                {
                    isLocal = 0;
                    frmBatchMidSign frm = new frmBatchMidSign();
                    frm.isLocal = 0;
                    frm.Text = "本地中转签收";
                    

                    ds.Clear();
                    ds1.Clear();
                    myGridView1.ClearColumnsFilter();
                    myGridView2.ClearColumnsFilter();

                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("webName", CommonClass.UserInfo.WebName));
                    list.Add(new SqlPara("siteName", CommonClass.UserInfo.SiteName));
                    list.Add(new SqlPara("isLocal", isLocal));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, proc, list);
                    ds = SqlHelper.GetDataSet(sps);
                    if (ds == null || ds.Tables.Count == 0) return;
                    ds1 = ds.Clone();
                    myGridControl1.DataSource = ds.Tables[0];
                    myGridControl2.DataSource = ds1.Tables[0];
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
            }

            else if (barEditItem8.EditValue.ToString() == "终端中转签收")       
            {


                string proc = "QSP_GET_MIDDLE_FOR_SIGN_NEW1";
                if (CommonClass.IsZhanQuCompanyId(CommonClass.UserInfo.companyid))
                {
                    proc = "QSP_GET_MIDDLE_FOR_SIGN_NEW1_ZQ";
                }
                try
                {
                    isLocal = 1;
                    frmBatchMidSign frm = new frmBatchMidSign();
                    frm.isLocal = 1;
                    frm.Text = "终端中转签收";
                    

                    ds.Clear();
                    ds1.Clear();
                    myGridView1.ClearColumnsFilter();
                    myGridView2.ClearColumnsFilter();

                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("webName", CommonClass.UserInfo.WebName));
                    list.Add(new SqlPara("siteName", CommonClass.UserInfo.SiteName));
                    list.Add(new SqlPara("isLocal", isLocal));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, proc, list);
                    ds = SqlHelper.GetDataSet(sps);
                    if (ds == null || ds.Tables.Count == 0) return;
                    ds1 = ds.Clone();
                    myGridControl1.DataSource = ds.Tables[0];
                    myGridControl2.DataSource = ds1.Tables[0];
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
            }
            else if (barEditItem8.EditValue.ToString() == "司机直送签收")       
            {
                string proc = "QSP_GET_SEND_FOR_SJZS_NEW";
                if (CommonClass.IsZhanQuCompanyId(CommonClass.UserInfo.companyid))
                {
                    proc = "QSP_GET_SEND_FOR_SJZS_NEW_ZQ";
                }
                try
                {
                    ds.Clear();
                    ds1.Clear();
                    myGridView1.ClearColumnsFilter();
                    myGridView2.ClearColumnsFilter();

                    List<SqlPara> list = new List<SqlPara>();
                    //list.Add(new SqlPara("webName", CommonClass.UserInfo.WebName));
                    //list.Add(new SqlPara("siteName", CommonClass.UserInfo.SiteName));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, proc, list);
                    ds = SqlHelper.GetDataSet(sps);
                    if (ds == null || ds.Tables.Count == 0) return;
                    ds1 = ds.Clone();
                    myGridControl1.DataSource = ds.Tables[0];
                    myGridControl2.DataSource = ds1.Tables[0];
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
            }
            else        
            {
                string proc = "QSP_GETALL_FOR_SIGN";
                if (CommonClass.IsZhanQuCompanyId(CommonClass.UserInfo.companyid))
                {
                    proc = "QSP_GETALL_FOR_SIGN_ZQ";
                }
                try
                {
                    ds.Clear();
                    ds1.Clear();
                    myGridView1.ClearColumnsFilter();
                    myGridView2.ClearColumnsFilter();

                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("webName", CommonClass.UserInfo.WebName));
                    list.Add(new SqlPara("siteName", CommonClass.UserInfo.SiteName));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, proc, list);
                    ds = SqlHelper.GetDataSet(sps);
                    if (ds == null || ds.Tables.Count == 0) return;
                    ds1 = ds.Clone();
                    myGridControl1.DataSource = ds.Tables[0];
                    myGridControl2.DataSource = ds1.Tables[0];
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }

            }


        }

        private void frmBatchFetchSign_Load(object sender, EventArgs e)
        {
            if (!CommonClass.FormSet(this, false, true)) return;
            CommonClass.GetGridViewColumns(myGridView1, false, myGridView2);
            GridOper.SetGridViewProperty(myGridView1, myGridView2);
            BarMagagerOper.SetBarPropertity(bar1, bar2);//如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1, myGridView2);
            SignOperator.Text = CommonClass.UserInfo.UserName;
            SignDate.DateTime = CommonClass.gcdate;
            repositoryItemComboBox2.Items.Add("自提签收");
            repositoryItemComboBox2.Items.Add("送货签收");
            repositoryItemComboBox2.Items.Add("本地中转签收");
            repositoryItemComboBox2.Items.Add("终端中转签收");
            repositoryItemComboBox2.Items.Add("司机直送签收");
            //repositoryItemComboBox2.Items.Add("全部");
            if (CommonClass.UserInfo.companyid == "486")  //鸿达武汉线放开打印限制
            {
                TDQSlist.Text = "打印托运单";
                btnPrintSign.Text = "完成并打印托运单";
            }
            timer1.Enabled = true;
            if (File.Exists(path))
            {
                File.Delete(path);
            }

           

        }

        private void myGridControl2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GridViewMove.Move(myGridView2, ds1, ds);
        }

        private void myGridControl2_DragDrop(object sender, DragEventArgs e)
        {
            GridViewMove.Move(myGridView1, ds, ds1);
        }

        private void myGridControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (hitInfo == null) return;
            Rectangle dragRect = new Rectangle(new Point(
            hitInfo.HitPoint.X - SystemInformation.DragSize.Width / 2,
           hitInfo.HitPoint.Y - SystemInformation.DragSize.Height / 2), SystemInformation.DragSize);
            if (!dragRect.Contains(new Point(e.X, e.Y)))
            {
                if (hitInfo.InRowCell)
                    myGridControl1.DoDragDrop("我要过去了....", DragDropEffects.All);
            }
        }

        private void myGridControl2_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void myGridControl1_MouseDown(object sender, MouseEventArgs e)
        {
            hitInfo = myGridView1.CalcHitInfo(new Point(e.X, e.Y));
        }

        private void myGridControl2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (hitInfo == null) return;
            Rectangle dragRect = new Rectangle(new Point(
            hitInfo.HitPoint.X - SystemInformation.DragSize.Width / 2,
           hitInfo.HitPoint.Y - SystemInformation.DragSize.Height / 2), SystemInformation.DragSize);
            if (!dragRect.Contains(new Point(e.X, e.Y)))
            {
                if (hitInfo.InRowCell)
                    myGridControl2.DoDragDrop("我要过去了....", DragDropEffects.All);
            }
        }

        private void myGridControl2_MouseDown(object sender, MouseEventArgs e)
        {
            hitInfo = myGridView2.CalcHitInfo(new Point(e.X, e.Y));
        }

        private void myGridControl1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void myGridControl1_DragDrop(object sender, DragEventArgs e)
        {
            GridViewMove.Move(myGridView2, ds1, ds);
        }

        private int sendMsg1(string billno)
        {
            
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_waybill_companyid", new List<SqlPara>() { new SqlPara("billno", billno) }));
            string companyid1 = ds.Tables[0].Rows[0]["companyid"].ToString();
            myGridView2.GetRowCellValue(0, "ConsignorCellPhone").ToString();
            return frm.sendSignsms_to_shipper(myGridView2, new DateTime(), companyid1, SignMan.Text.Trim(), SignManPhone.Text.Trim(), companyname(companyid1));
        }

        private int sendMsg2(string billno)
        {


            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_waybill_companyid", new List<SqlPara>() { new SqlPara("billno", billno) }));
            string companyid1 = ds.Tables[0].Rows[0]["companyid"].ToString();

            return frm.sendSignsms_to_shipper2(myGridView2, new DateTime(), companyid1, SignMan.Text.Trim(), SignManPhone.Text.Trim(), companyname(companyid1));
        }

        private string companyname(string companyid)
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("companyId0", companyid));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_COMPANYNAMW", list);
            DataSet ds = SqlHelper.GetDataSet(sps);
            return ds.Tables[0].Rows[0]["gsqc"].ToString();

        }

        //验证并保存
        private void check()
        {            
            string allBillNo = "";
            StringBuilder extract = new StringBuilder();
            StringBuilder transfer = new StringBuilder();
            StringBuilder send = new StringBuilder();
            StringBuilder driver = new StringBuilder();
            if (myGridView2.RowCount == 0)
            {
                XtraMessageBox.Show("没有发现任何需要签收的清单，请先在第①步中构建清单。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (myGridView2.RowCount > 0)
            {
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    billno = myGridView2.GetRowCellValue(i, "BillNo").ToString();


                    fun(billno);
                    allBillNo += billno + ",";
                    string type = myGridView2.GetRowCellValue(i, "SignType").ToString();

                    if (type == "自提签收")
                    {
                        extract.Append(billno + ",");

                    }

                    else if (type == "中转签收" || type == "本地中转签收" || type == "终端中转签收")
                    {
                        transfer.Append(billno + ",");

                    }

                    else if (type == "送货签收")
                    {
                        send.Append(billno + ",");
                    }
                    else
                    {
                        driver.Append(billno + ",");
                    }
                }

            }
            if (!string.IsNullOrEmpty(extract.ToString()))
            {
               
                if (chkSMS.Checked)
                {
                    if (0 == 0)
                    {
                        XtraMessageBox.Show("您选择了短信通知，却没有勾选任何需要发送短信的运单!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                if (SignMan.Text.Trim() == "")
                {
                    MsgBox.ShowOK("必须填写签收人！");
                    return;
                }
                //判断所有的提货数据状态 拼接状态字符串

    
                float sumCollectionPay = 0;
                float sumFetchPay = 0;
                int collectionCount = 0;
                int fetchCount = 0;
                int collectionStateCount = 0;
                int fetchStateCount = 0;
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    //sumCollectionPay += ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "CollectionPay"));
                    //sumFetchPay += ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "FetchPay"));
                    if (ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "CollectionPay")) > 0)
                    {
                        collectionCount++;
                        sumCollectionPay += ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "CollectionPay"));
                    }
                    if (ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "FetchPay")) > 0)
                    {
                        fetchCount++;
                        sumFetchPay += ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "FetchPay"));
                    }
                    if (ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "CollectionPayState")) == 1)
                    {
                        collectionStateCount++;
                    }
                    if (ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "FetchPayVerifState")) == 1)
                    {
                        fetchStateCount++;
                    }
                }
                //提示信息
                if ((new frmShowInfo(sumCollectionPay, sumFetchPay, myGridView2.RowCount)).ShowDialog() != DialogResult.Yes) return;

                Dictionary<string, string> dic = new Dictionary<string, string>();//字典存储运单号和批次号，一对一
                string BillNo = "";
                string DepartureListNO = "";
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                        string type = myGridView2.GetRowCellValue(i, "SignType").ToString();
                        if(type == "自提签收")
                        {
                            BillNo += ConvertType.ToString(myGridView2.GetRowCellValue(i, "BillNo")) + '@';
                            DepartureListNO += ConvertType.ToString(myGridView2.GetRowCellValue(i, "Id")) + '@';

                            dic.Add(ConvertType.ToString(myGridView2.GetRowCellValue(i, "BillNo")), ConvertType.ToString(myGridView2.GetRowCellValue(i, "Id")));
                        }
                    
                }

                try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("SignNO", Guid.NewGuid()));
                    list.Add(new SqlPara("BillNoStr", BillNo));
                    list.Add(new SqlPara("SignType", "提货签收"));
                    list.Add(new SqlPara("SignMan", SignMan.Text.Trim()));
                    list.Add(new SqlPara("SignManCardID", SignManCardID.Text.Trim()));
                    list.Add(new SqlPara("AgentMan", ""));
                    list.Add(new SqlPara("SignManPhone", SignManPhone.Text.Trim()));
                    list.Add(new SqlPara("AgentCardId", SignManCardID.Text.Trim()));
                    list.Add(new SqlPara("SignDate", SignDate.DateTime));
                    list.Add(new SqlPara("SignDesc", ""));
                    list.Add(new SqlPara("SignOperator", SignOperator.Text.Trim()));
                    list.Add(new SqlPara("SignSite", ConvertType.ToString(CommonClass.UserInfo.SiteName)));
                    list.Add(new SqlPara("SignWeb", ConvertType.ToString(CommonClass.UserInfo.WebName)));
                    list.Add(new SqlPara("SignContent", SignContent.Text.Trim()));
                    list.Add(new SqlPara("IdStr", DepartureListNO));
                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLSIGN", list);
                    DataSet ddd1 = SqlHelper.GetDataSet(sps);
                    DataRow dddr = ddd1.Tables[0].Rows[0];
                    if (SqlHelper.ExecteNonQuery(sps) > 0)
                    {                                   

                        #region ZQTMS同步签收
                        //验证是否有做分拨
                        List<SqlPara> list1 = new List<SqlPara>();
                        list1.Add(new SqlPara("BillNoStr", BillNo));
                        SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_billDepartureFBList", list1);
                        DataTable dt = SqlHelper.GetDataTable(sps1);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            BillNo = string.Empty;
                            DepartureListNO = string.Empty;
                            foreach (DataRow row in dt.Rows)
                            {
                                BillNo += row["BillNo"].ToString() + "@";
                                DepartureListNO += dic[row["BillNo"].ToString()].ToString() + "@";
                            }
                            Dictionary<string, string> dic_ZQTMS = new Dictionary<string, string>();
                            dic_ZQTMS.Add("Bills", BillNo);
                            dic_ZQTMS.Add("SendBatchs", DepartureListNO);
                            dic_ZQTMS.Add("SignType", "提货签收");
                            dic_ZQTMS.Add("SignMan", SignMan.Text.Trim());
                            dic_ZQTMS.Add("SignManCardID", SignManCardID.Text.Trim());
                            dic_ZQTMS.Add("AgentMan", "");
                            dic_ZQTMS.Add("SignManPhone", SignManPhone.Text.Trim());
                            dic_ZQTMS.Add("AgentCardId", SignManCardID.Text.Trim());
                            //dic_ZQTMS.Add("SignDate", @"\/Date(" + SignDate.DateTime.ToString() + @")\/");
                            dic_ZQTMS.Add("SignDesc", "");
                            dic_ZQTMS.Add("SignOperator", SignOperator.Text.Trim());
                            dic_ZQTMS.Add("SignSite", ConvertType.ToString(CommonClass.UserInfo.SiteName));
                            dic_ZQTMS.Add("SignWeb", ConvertType.ToString(CommonClass.UserInfo.WebName));
                            dic_ZQTMS.Add("SignContent", SignContent.Text.Trim());

                            dic_ZQTMS.Add("LoginAreaName", CommonClass.UserInfo.AreaName);
                            dic_ZQTMS.Add("LoginCauseName", CommonClass.UserInfo.CauseName);
                            dic_ZQTMS.Add("LoginDepartName", CommonClass.UserInfo.DepartName);
                            dic_ZQTMS.Add("LoginSiteName", CommonClass.UserInfo.SiteName);
                            dic_ZQTMS.Add("LoginWebName", CommonClass.UserInfo.WebName);
                            dic_ZQTMS.Add("LoginUserAccount", CommonClass.UserInfo.UserAccount);
                            dic_ZQTMS.Add("LoginUserName", CommonClass.UserInfo.UserName);
                            dic_ZQTMS.Add("ExceptType", "");
                            dic_ZQTMS.Add("ExceptContent", "");
                            dic_ZQTMS.Add("ExceptReason", "");

                            //ZQTMS签收同步接口调用                     
                            string url = HttpHelper.urlSignSyn;

                            string data = JsonConvert.SerializeObject(dic_ZQTMS);
                            //data = data.TrimStart('[').TrimEnd(']');
                            ResponseModelClone<string> res = HttpHelper.HttpPost(data, url);
                            if (res.State != "200")
                            {
                                //string errorNode = isLocal == 0 ? "本地中转签收" : "终端中转签收";
                                List<SqlPara> listLog = new List<SqlPara>();
                                listLog.Add(new SqlPara("BillNo", BillNo));
                                listLog.Add(new SqlPara("Batch", ""));
                                listLog.Add(new SqlPara("ErrorNode", "提货签收"));
                                listLog.Add(new SqlPara("ExceptMessage", res.Message));
                                SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                                SqlHelper.ExecteNonQuery(spsLog);
                                // MsgBox.ShowError(res.State + "：" +res.Message);
                            }
                            else
                            {
                                MsgBox.ShowOK("操作已完成：ZQTMS已同步签收！");
                            }
                        }
                        else
                        {
                            MsgBox.ShowOK();
                        }
                        #endregion
                        //yzw 自提签收同步
                        CommonSyn.BILLSIGN_MIDDLESign_SYN(BillNo, SignMan.Text.Trim(), "");
                        //CommonSyn.SignTimeSyn(BillNo, "提货签收");//ZQTMS签收时效同步
                        //CommonSyn.TraceSyn(null, BillNo, 14, "提货签收", 1, null, null);
                        extract.Length = 0;
                        //SignMan.Text = SignManCardID.Text = SignContent.Text = "";
                        SignDate.DateTime = CommonClass.gcdate;
                        SignOperator.Text = CommonClass.UserInfo.UserName;
                    }
                    int a = 0;
                    int b = 0;
                    if (checkEdit1.Checked)
                    {
                        a = sendMsg1(billno);

                    }
                    if (checkEdit2.Checked)
                    {
                        b = sendMsg2(billno);

                    }
                    if (a + b != 0)
                    {
                        MsgBox.ShowOK("发送了" + (a + b) + "条短信");
                    }
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
            }
            if (!string.IsNullOrEmpty(transfer.ToString()))     //中转 
            {         

                if (chkSMS.Checked)
                {
                    if (0 == 0)
                    {
                        XtraMessageBox.Show("您选择了短信通知，却没有勾选任何需要发送短信的运单!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                if (SignMan.Text.Trim() == "")
                {
                    MsgBox.ShowOK("必须填写签收人！");
                    return;
                }
                //判断所有的提货数据状态 拼接状态字符串

                float sumMiddlePay = 0;
                int sumCount = 0;
                int MiddleCount = 0;
                int MiddlePayStateCount = 0;
                int MiddleAccVerifStateCount = 0;
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    if (ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "AccMiddlePay")) > 0)
                    {
                        MiddleCount++;
                        sumMiddlePay += ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "AccMiddlePay"));
                    }
                    if (ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "MiddlePayState")) == 1)
                    {
                        MiddlePayStateCount++;
                    }
                    if (ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "MiddleAccVerifState")) == 1)
                    {
                        MiddleAccVerifStateCount++;
                    }
                }
                sumCount = myGridView2.RowCount;
                string sShowOK = "当前选中：" + ConvertType.ToString(sumCount) + "票\r\n中转费已核销有：" + ConvertType.ToString(MiddleAccVerifStateCount) + "票\r\n中转费已收的有：" + ConvertType.ToString(MiddlePayStateCount)
                    + "\r\n中转费总金额为：" + ConvertType.ToString(sumMiddlePay) + "\r\n是否继续？";

                if (MsgBox.ShowYesNo(sShowOK) != DialogResult.Yes)
                {
                    return;
                }
                string BillNo = "";
                string frmName = "";
                string typeName = "";
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                  
                        string type = myGridView2.GetRowCellValue(i, "SignType").ToString();

                        if (type == "中转签收" || type == "本地中转签收" || type == "终端中转签收")
                        {
                            BillNo = BillNo + myGridView2.GetRowCellValue(i, "BillNo").ToString().Trim() + '@';

                            string middleType = myGridView2.GetRowCellValue(i, "MiddleType").ToString();
                            
                            if (middleType == "0")
                            {
                                frmName = "本地中转签收";
                                
                            }
                            else
                            {
                                frmName = "终端中转签收";
                               
                            }
                            typeName = typeName + frmName + '@';
                            
                        }
                       
                           
                        
                       
                }

                try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("SignNO", Guid.NewGuid()));
                    list.Add(new SqlPara("BillNo", BillNo));
                    list.Add(new SqlPara("SignType", typeName));
                    list.Add(new SqlPara("SignMan", SignMan.Text.Trim()));
                    list.Add(new SqlPara("SignManCardID", SignManCardID.Text.Trim()));
                    list.Add(new SqlPara("AgentMan", ""));
                    list.Add(new SqlPara("SignManPhone", SignManPhone.Text.Trim()));
                    list.Add(new SqlPara("AgentCardId", SignManCardID.Text.Trim()));
                    list.Add(new SqlPara("SignDate", SignDate.DateTime));
                    list.Add(new SqlPara("SignDesc", ""));
                    list.Add(new SqlPara("SignOperator", SignOperator.Text.Trim()));
                    list.Add(new SqlPara("SignSite", ConvertType.ToString(CommonClass.UserInfo.SiteName)));
                    list.Add(new SqlPara("SignWeb", ConvertType.ToString(CommonClass.UserInfo.WebName)));
                    list.Add(new SqlPara("SignContent", SignContent.Text.Trim()));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLSIGN_MIDDLE_New", list);
                    DataSet ddd1 = SqlHelper.GetDataSet(sps);
                    DataRow dddr = ddd1.Tables[0].Rows[0];
                    if (SqlHelper.ExecteNonQuery(sps) > 0)
                    {
                        ////跟踪节点信息同步接口 (中转)
                                              
       
                        #region ZQTMS同步签收
                        //验证是否有做分拨
                        List<SqlPara> list1 = new List<SqlPara>();
                        list1.Add(new SqlPara("BillNoStr", BillNo));
                        SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_billDepartureFBList", list1);
                        DataTable dt = SqlHelper.GetDataTable(sps1);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            BillNo = string.Empty;
                            foreach (DataRow row in dt.Rows)
                            {
                                BillNo += row["BillNo"].ToString() + "@";
                            }
                            Dictionary<string, string> dic_ZQTMS = new Dictionary<string, string>();
                            dic_ZQTMS.Add("Bills", BillNo);
                            dic_ZQTMS.Add("SendBatchs", "");
                            dic_ZQTMS.Add("SignType", "中转签收");
                            dic_ZQTMS.Add("SignMan", SignMan.Text.Trim());
                            dic_ZQTMS.Add("SignManCardID", SignManCardID.Text.Trim());
                            dic_ZQTMS.Add("AgentMan", "");
                            dic_ZQTMS.Add("SignManPhone", SignManPhone.Text.Trim());
                            dic_ZQTMS.Add("AgentCardId", SignManCardID.Text.Trim());
                            //dic_ZQTMS.Add("SignDate", @"\/Date(" + SignDate.DateTime.ToString() + @")\/");
                            dic_ZQTMS.Add("SignDesc", "");
                            dic_ZQTMS.Add("SignOperator", SignOperator.Text.Trim());
                            dic_ZQTMS.Add("SignSite", ConvertType.ToString(CommonClass.UserInfo.SiteName));
                            dic_ZQTMS.Add("SignWeb", ConvertType.ToString(CommonClass.UserInfo.WebName));
                            dic_ZQTMS.Add("SignContent", SignContent.Text.Trim());

                            dic_ZQTMS.Add("LoginAreaName", CommonClass.UserInfo.AreaName);
                            dic_ZQTMS.Add("LoginCauseName", CommonClass.UserInfo.CauseName);
                            dic_ZQTMS.Add("LoginDepartName", CommonClass.UserInfo.DepartName);
                            dic_ZQTMS.Add("LoginSiteName", CommonClass.UserInfo.SiteName);
                            dic_ZQTMS.Add("LoginWebName", CommonClass.UserInfo.WebName);
                            dic_ZQTMS.Add("LoginUserAccount", CommonClass.UserInfo.UserAccount);
                            dic_ZQTMS.Add("LoginUserName", CommonClass.UserInfo.UserName);
                            dic_ZQTMS.Add("ExceptType", "");
                            dic_ZQTMS.Add("ExceptContent", "");
                            dic_ZQTMS.Add("ExceptReason", "");

                            //ZQTMS签收同步接口调用

                            string url = HttpHelper.urlSignSyn;
                            string data = JsonConvert.SerializeObject(dic_ZQTMS);
                            //data = data.TrimStart('[').TrimEnd(']');
                            ResponseModelClone<string> res = HttpHelper.HttpPost(data, url);
                            if (res.State != "200")
                            {
                                string errorNode = isLocal == 0 ? "本地中转签收" : "终端中转签收";
                                List<SqlPara> listLog = new List<SqlPara>();
                                listLog.Add(new SqlPara("BillNo", BillNo));
                                listLog.Add(new SqlPara("Batch", ""));
                                listLog.Add(new SqlPara("ErrorNode", errorNode));
                                listLog.Add(new SqlPara("ExceptMessage", res.Message));
                                SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                                SqlHelper.ExecteNonQuery(spsLog);
                                //MsgBox.ShowError(res.State + "：" + res.Message);
                            }
                            else
                            {
                                MsgBox.ShowOK("操作已完成：ZQTMS已同步签收！");
                            }
                        }
                        else
                        {
                            MsgBox.ShowOK();
                        }
                        #endregion
                        //yzw 中转签收同步
                        CommonSyn.BILLSIGN_MIDDLE_SYN(BillNo, SignMan.Text.Trim(), "中转签收");
                        //CommonSyn.SignTimeSyn(BillNo, "中转签收");//ZQTMS签收时效同步
                        //CommonSyn.TraceSyn(null, BillNo, 15, "中转签收", 1, null, null);
                        setdata();
                        //CommonSyn.MidConfirmSyn(BillNo, CommonClass.UserInfo.UserName, CommonClass.UserInfo.WebName);//中转提付同步zaj 2018-6-13
                        //ds1.Clear();
                        transfer.Length = 0;
                        //SignMan.Text = SignManCardID.Text = SignContent.Text = "";
                        SignDate.DateTime = CommonClass.gcdate;
                        SignOperator.Text = CommonClass.UserInfo.UserName;
                    }
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
            }
            if (!string.IsNullOrEmpty(send.ToString()))  //送货
            {

                if (chkSMS.Checked)
                {
                    if (0 == 0)
                    {
                        XtraMessageBox.Show("您选择了短信通知，却没有勾选任何需要发送短信的运单!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                if (SignMan.Text.Trim() == "")
                {
                    MsgBox.ShowOK("必须填写签收人！");
                    return;
                }
                //判断所有的提货数据状态 拼接状态字符串

                float sumAccSend = 0;
                int sumCount = 0;
                int AccSendCount = 0;
                int SendVerifStateCount = 0;
                for (int i = 0; i < myGridView2.RowCount; i++)
                {                   
                    if (ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "AccSend")) > 0)
                    {
                        AccSendCount++;
                        sumAccSend += ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "AccSend"));
                    }
                    if (ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "SendVerifState")) == 1)
                    {
                        SendVerifStateCount++;
                    }
                }
                sumCount = myGridView2.RowCount;
                string sShowOK = "当前选中：" + ConvertType.ToString(sumCount) + "票\r\n送货费核销有：" +

                    ConvertType.ToString(SendVerifStateCount) + "票\r\n送货费总金额为：" + ConvertType.ToString(sumAccSend) + "\r\n是否继续？";

                if (MsgBox.ShowYesNo(sShowOK) != DialogResult.Yes)
                {
                    return;
                }
                Dictionary<string, string> dic = new Dictionary<string, string>();//字典存储运单号和批次号，一对一
                string BillNoStr = "";
                string SendBatchStr = "";
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                
                        string type = myGridView2.GetRowCellValue(i, "SignType").ToString();
                        if (type == "送货签收")
                        {
                            BillNoStr += ConvertType.ToString(myGridView2.GetRowCellValue(i, "BillNo")) + '@';
                            SendBatchStr += ConvertType.ToString(myGridView2.GetRowCellValue(i, "SendBatch")) + '@';

                            dic.Add(ConvertType.ToString(myGridView2.GetRowCellValue(i, "BillNo")), ConvertType.ToString(myGridView2.GetRowCellValue(i, "SendBatch")));
                        }                   
                }


                try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("BillNoStr", BillNoStr));
                    list.Add(new SqlPara("SignType", "送货签收"));
                    list.Add(new SqlPara("SignMan", SignMan.Text.Trim()));
                    list.Add(new SqlPara("SignManCardID", SignManCardID.Text.Trim()));
                    list.Add(new SqlPara("AgentMan", ""));
                    list.Add(new SqlPara("SignManPhone", SignManPhone.Text.Trim()));
                    list.Add(new SqlPara("AgentCardId", SignManCardID.Text.Trim()));
                    list.Add(new SqlPara("SignDate", SignDate.DateTime));
                    list.Add(new SqlPara("SignDesc", ""));
                    list.Add(new SqlPara("SignOperator", SignOperator.Text.Trim()));
                    list.Add(new SqlPara("SignSite", CommonClass.UserInfo.SiteName));
                    list.Add(new SqlPara("SignWeb", CommonClass.UserInfo.WebName));
                    list.Add(new SqlPara("SignContent", SignContent.Text.Trim()));
                    list.Add(new SqlPara("SendBatchStr", SendBatchStr));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLSIGN_SEND", list);
                    DataSet ddd1 = SqlHelper.GetDataSet(sps);
                    DataRow dddr = ddd1.Tables[0].Rows[0];
                    if (SqlHelper.ExecteNonQuery(sps) > 0)
                    {

                       
                        ////跟踪节点信息同步接口 (中转)
                       
                        #region ZQTMS同步签收
                        //验证是否有做分拨
                        List<SqlPara> list1 = new List<SqlPara>();
                        list1.Add(new SqlPara("BillNoStr", BillNoStr));
                        SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_billDepartureFBList", list1);
                        DataTable dt = SqlHelper.GetDataTable(sps1);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            BillNoStr = string.Empty;
                            SendBatchStr = string.Empty;
                            foreach (DataRow row in dt.Rows)
                            {
                                BillNoStr += row["BillNo"].ToString() + "@";
                                SendBatchStr += dic[row["BillNo"].ToString()].ToString() + "@";
                            }
                            Dictionary<string, string> dic_ZQTMS = new Dictionary<string, string>();
                            dic_ZQTMS.Add("Bills", BillNoStr);
                            dic_ZQTMS.Add("SendBatchs", SendBatchStr);
                            dic_ZQTMS.Add("SignType", "送货签收");
                            dic_ZQTMS.Add("SignMan", SignMan.Text.Trim());
                            dic_ZQTMS.Add("SignManCardID", SignManCardID.Text.Trim());
                            dic_ZQTMS.Add("AgentMan", "");
                            dic_ZQTMS.Add("SignManPhone", SignManPhone.Text.Trim());
                            dic_ZQTMS.Add("AgentCardId", SignManCardID.Text.Trim());
                            //dic_ZQTMS.Add("SignDate", @"\/Date(" + SignDate.DateTime.ToString() + @")\/");
                            dic_ZQTMS.Add("SignDesc", "");
                            dic_ZQTMS.Add("SignOperator", SignOperator.Text.Trim());
                            dic_ZQTMS.Add("SignSite", ConvertType.ToString(CommonClass.UserInfo.SiteName));
                            dic_ZQTMS.Add("SignWeb", ConvertType.ToString(CommonClass.UserInfo.WebName));
                            dic_ZQTMS.Add("SignContent", SignContent.Text.Trim());

                            dic_ZQTMS.Add("LoginAreaName", CommonClass.UserInfo.AreaName);
                            dic_ZQTMS.Add("LoginCauseName", CommonClass.UserInfo.CauseName);
                            dic_ZQTMS.Add("LoginDepartName", CommonClass.UserInfo.DepartName);
                            dic_ZQTMS.Add("LoginSiteName", CommonClass.UserInfo.SiteName);
                            dic_ZQTMS.Add("LoginWebName", CommonClass.UserInfo.WebName);
                            dic_ZQTMS.Add("LoginUserAccount", CommonClass.UserInfo.UserAccount);
                            dic_ZQTMS.Add("LoginUserName", CommonClass.UserInfo.UserName);
                            dic_ZQTMS.Add("ExceptType", "");
                            dic_ZQTMS.Add("ExceptContent", "");
                            dic_ZQTMS.Add("ExceptReason", "");

                            //ZQTMS签收同步接口调用

                            string url = HttpHelper.urlSignSyn;



                            string data = JsonConvert.SerializeObject(dic_ZQTMS);
                            //data = data.TrimStart('[').TrimEnd(']');
                            ResponseModelClone<string> res = HttpHelper.HttpPost(data, url);
                            if (res.State != "200")
                            {
                                List<SqlPara> listLog = new List<SqlPara>();
                                listLog.Add(new SqlPara("BillNo", BillNoStr));
                                listLog.Add(new SqlPara("Batch", ""));
                                listLog.Add(new SqlPara("ErrorNode", "送货签收"));
                                listLog.Add(new SqlPara("ExceptMessage", res.Message));
                                SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_SYNLOG", listLog);
                                SqlHelper.ExecteNonQuery(spsLog);
                                //MsgBox.ShowError(res.State + "：" + res.Message);
                            }
                            else
                            {
                                MsgBox.ShowOK("操作已完成：ZQTMS已同步签收！");
                            }
                        }
                        else
                        {
                            MsgBox.ShowOK();
                        }
                        #endregion
                        //yzw 送货签收同步
                        CommonSyn.BILLSIGN_MIDDLE_SYN(BillNoStr, SignMan.Text.Trim(), "送货签收");
                        //CommonSyn.SignTimeSyn(BillNoStr, "送货签收");//ZQTMS签收时效同步
                        //CommonSyn.TraceSyn(null, BillNoStr, 13, "送货签收", 1, null, null); 
                        send.Length = 0;
                        //SignMan.Text = SignManCardID.Text = SignContent.Text = "";
                        SignDate.DateTime = CommonClass.gcdate;
                        SignOperator.Text = CommonClass.UserInfo.UserName;

                    }
                    
                    int a = 0;
                    int b = 0;
                    if (checkEdit1.Checked)
                    {
                        a = sendMsg1(billno);

                    }
                    if (checkEdit2.Checked)
                    {
                        b = sendMsg2(billno);

                    }
                    if (a + b != 0)
                    {
                        MsgBox.ShowOK("发送了" + (a + b) + "条短信");
                    }
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
            }
            if (!string.IsNullOrEmpty(driver.ToString()))      //司机直送
            {
                if (SignMan.Text.Trim() == "")
                {
                    MsgBox.ShowOK("必须填写签收人！");
                    return;
                }
                float sumAccSend = 0;
                int AccSendCount = 0;
                int SendVerifStateCount = 0;
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    if (ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "AccSend")) > 0)
                    {
                        AccSendCount++;
                        sumAccSend += ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "AccSend"));
                    }
                    if (ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "SendVerifState")) == 1)
                    {
                        SendVerifStateCount++;
                    }
                }
                string BillNoStr = "";
                string SendBatchStr = "";
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                   
                        string type = myGridView2.GetRowCellValue(i, "SignType").ToString();
                        if (type == "司机直送签收")
                        {
                            BillNoStr += ConvertType.ToString(myGridView2.GetRowCellValue(i, "BillNo")) + '@';
                            SendBatchStr += ConvertType.ToString(myGridView2.GetRowCellValue(i, "SendBatch")) + '@';

                        }
                  
                }
                try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("BillNoStr", BillNoStr));
                    list.Add(new SqlPara("SignType", "司机直送签收"));
                    list.Add(new SqlPara("SignMan", SignMan.Text.Trim()));
                    list.Add(new SqlPara("SignManCardID", SignManCardID.Text.Trim()));
                    list.Add(new SqlPara("AgentMan", ""));
                    list.Add(new SqlPara("SignManPhone", SignManPhone.Text.Trim()));
                    list.Add(new SqlPara("AgentCardId", SignManCardID.Text.Trim()));
                    list.Add(new SqlPara("SignDate", SignDate.DateTime));
                    list.Add(new SqlPara("SignDesc", ""));
                    list.Add(new SqlPara("SignOperator", SignOperator.Text.Trim()));
                    list.Add(new SqlPara("SignSite", CommonClass.UserInfo.SiteName));
                    list.Add(new SqlPara("SignWeb", CommonClass.UserInfo.WebName));
                    list.Add(new SqlPara("SignContent", SignContent.Text.Trim()));
                    list.Add(new SqlPara("SendBatchStr", SendBatchStr));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLSIGN_SEND_SJZS", list);
                    DataSet ddd1 = SqlHelper.GetDataSet(sps);
                    DataRow dddr = ddd1.Tables[0].Rows[0];
                    if (SqlHelper.ExecteNonQuery(sps) > 0)
                    {
                        MsgBox.ShowOK();
                                               
                        
                        //ds.Clear();
                        driver.Length = 0;
                        //SignMan.Text = SignManCardID.Text = SignContent.Text = "";
                        SignDate.DateTime = CommonClass.gcdate;
                        SignOperator.Text = CommonClass.UserInfo.UserName;
                        simpleButton6_Click(null, null);
                        CommonSyn.TraceSyn(null, BillNoStr, 13, "送货签收", 1, null, null);
                    }
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
                }

           
            if (extract.Length == 0 && transfer.Length == 0 && send.Length == 0 && driver.Length == 0)
            {
                //ds.Clear();
                ds1.Clear();
                SignMan.Text = SignManCardID.Text = SignContent.Text = "";
            }
            
        }




        private void fun(string billno)
        {   
                #region //跟踪节点信息同步接口 (签收)
                {                    
                        List<SqlPara> lists = new List<SqlPara>();
                        lists.Add(new SqlPara("BillNo", billno));
                        SqlParasEntity spsa = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_ByID_Auto_BillNo2", lists);
                        DataTable dt = SqlHelper.GetDataTable(spsa);

                        if (dt.Rows[0]["BegWeb"].ToString() == "三方")
                        {
                            Dictionary<string, object> hashMap = new Dictionary<string, object>();
                            hashMap.Add("carriageSn", dt.Rows[0]["BillNo"]);
                            hashMap.Add("orderStatusCode", 2060);
                            hashMap.Add("traceRemarks", "签收完成");
                            string json = JsonConvert.SerializeObject(hashMap);
                            string url = "http://120.76.141.227:9882/umsv2.biz/open/track/record_trunk_order_status";
                            try
                            {
                                HttpHelper.HttpPostJava(json, url);
                                
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    
                }
                #endregion               
               
           
        }

        private void myGridControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GridViewMove.Move(myGridView1, ds, ds1);
        }

        //提取
        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            
            getdata();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "提货签收库存");
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView1.OptionsView.ShowAutoFilterRow = !myGridView1.OptionsView.ShowAutoFilterRow;
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridViewMove.Move(myGridView1, ds, ds1);
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView1.SelectAll();
            GridViewMove.Move(myGridView1, ds, ds1);
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView2);
        }

        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridViewMove.Move(myGridView2, ds1, ds);
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView2.SelectAll();
            GridViewMove.Move(myGridView2, ds1, ds);
        }

        //关闭
        private void btnClose_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView2, "提货签收挑选库存");
        }

        ////////////////////////////////////////
        private void barEditItem1_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue == null) return;
            string szfilter = e.NewValue.ToString().Trim();
            if (szfilter != "")
            {
                myGridView1.ClearColumnsFilter();
                myGridView1.Columns["BillNo"].FilterInfo = new ColumnFilterInfo("[BillNo] LIKE " + "'%" + szfilter + "%'" + " OR [BillNo] LIKE" + "'%" + szfilter + "%'", "");
            }
            else
                myGridView1.ClearColumnsFilter();
        }

        private void barEditItem1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || ((DevExpress.XtraEditors.TextEdit)sender).Text == "" || myGridView1.RowCount > 1)
            {
                e.Handled = true;
                return;
            }
            myGridView1.SelectRow(0);
            GridViewMove.Move(myGridView1, ds, ds1);
            ((DevExpress.XtraEditors.TextEdit)sender).Text = "";

            myGridView1.ClearColumnsFilter();
            e.Handled = true;
        }

        private void barEditItem2_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue == null) return;
            string szfilter = e.NewValue.ToString().Trim();
            if (szfilter != "")
            {
                myGridView2.ClearColumnsFilter();
                myGridView2.Columns["BillNo"].FilterInfo = new ColumnFilterInfo("[BillNo] LIKE " + "'%" + szfilter + "%'" + " OR [BillNo] LIKE" + "'%" + szfilter + "%'", "");
            }
            else
                myGridView2.ClearColumnsFilter();
        }

        private void barEditItem2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || ((DevExpress.XtraEditors.TextEdit)sender).Text == "" || myGridView2.RowCount > 1)
            {
                e.Handled = true;
                return;
            }
            myGridView2.SelectRow(0);
            GridViewMove.Move(myGridView2, ds1, ds);
            ((DevExpress.XtraEditors.TextEdit)sender).Text = "";

            myGridView2.ClearColumnsFilter();
            e.Handled = true;
        }

        //保存
        private void btnOk_Click(object sender, EventArgs e)
        {
            
            if (CommonClass.CheckKongHuo(myGridView2, 1))
            {
                MsgBox.ShowOK("选择的清单包含控货的运单,不能提货!");
                return;
            }
            check();
        }

        //关闭
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void myGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GridViewMove.Move(myGridView1, ds, ds1);

            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }

        private void PrintQSD(string BillNoStr)
        {
            if (string.IsNullOrEmpty(BillNoStr)) return;
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILLINFO_PRINT_QSD", new List<SqlPara> { new SqlPara("BillNoStr", BillNoStr) }));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                MsgBox.ShowError("没有找到选中的运单信息,打印失败,(请检查网络或运单是否已被删除)!");
                return;
            }
            //DataTable dt = ds.Tables[0].Clone();
            //frmPrintRuiLang fprl;
            //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //{
            //    dt.ImportRow(ds.Tables[0].Rows[i]);
            //    fprl = new frmPrintRuiLang("提货单(套打)", dt);
            //    fprl.ShowDialog();
            //}

            if (CommonClass.UserInfo.companyid == "486")
            {
                string name = "";
                //int rowhandle = myGridView2.FocusedRowHandle;
                //if (rowhandle < 0) return;
                //string BillNo = myGridView2.GetRowCellValue(rowhandle, "BillNo").ToString();


                if (CommonClass.UserInfo.companyid == "486" && CommonClass.UserInfo.WebName == "宝虹广州项目部")
                {
                    name = "宝虹广州项目部托运单";
                }
                else if (CommonClass.UserInfo.companyid == "486" && CommonClass.UserInfo.WebName == "宝虹武汉东西湖营业部")
                {
                    name = "宝虹广州总部配送部_签收单";
                }
                else if (CommonClass.UserInfo.companyid == "486" && CommonClass.UserInfo.WebName == "武汉遵义营业部")
                {
                    name = "武汉遵义营业部托运单";
                }
                else
                {
                    name = "宝虹托运单_签收单";
                }

                frmRuiLangService.Print(name, SqlHelper.GetDataTable(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTBIll_TX_1", new List<SqlPara> { new SqlPara("BillNoStr", BillNoStr) })), "");
            }

            
            else
            {


                if (btnName == "simpleButton1")
                {
                    frmRuiLangService.Print("提货单(套打)", ds.Tables[0]);
                }
                if (btnName == "btnPrintSign")
                {
                    //frmRuiLangService.Print("提货单", ds.Tables[0]);
                    //jl20181127
                    if (CommonClass.UserInfo.WebName == "上海青浦操作部"
                        || CommonClass.UserInfo.WebName == "上海青浦操作部1"
                        || CommonClass.UserInfo.WebName == "杭州操作部"
                        || CommonClass.UserInfo.WebName == "杭州操作部1"
                        || CommonClass.UserInfo.WebName == "无锡二级分拨中心"
                        || CommonClass.UserInfo.WebName == "无锡二级分拨中心1"
                        || CommonClass.UserInfo.WebName == "宁波操作部"
                        || CommonClass.UserInfo.WebName == "宁波操作部1"
                        || CommonClass.UserInfo.WebName == "济南二级分拨中心"
                        || CommonClass.UserInfo.WebName == "济南二级分拨中心1"
                        || CommonClass.UserInfo.WebName == "无锡二级分拨中心"
                        || CommonClass.UserInfo.WebName == "无锡二级分拨中心1"
                        || CommonClass.UserInfo.WebName == "武汉二级分拨中心"
                        || CommonClass.UserInfo.WebName == "武汉二级分拨中心1"
                        || CommonClass.UserInfo.WebName == "广州操作部"
                        || CommonClass.UserInfo.WebName == "广州操作部1"
                        || CommonClass.UserInfo.WebName == "东莞大坪分拨中心"
                        || CommonClass.UserInfo.WebName == "东莞大坪分拨中心1"
                        || CommonClass.UserInfo.WebName == "青岛二级分拨中心"
                        || CommonClass.UserInfo.WebName == "青岛二级分拨中心1")
                    {
                        frmRuiLangService.Print("提货单大坪", ds.Tables[0]);
                    }
                    else
                    {
                        frmRuiLangService.Print("提货单", ds.Tables[0]);
                    }
                }
            }
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem15_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem16_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView2);
        }

        private void barButtonItem17_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView2.Guid.ToString());
        }

        private void barButtonItem18_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView2);
        }

        private void simpleButton3_Click(object sender, EventArgs e)  //maohui20180320
        {
            btnName = "simpleButton3";
            if (CommonClass.CheckKongHuo(myGridView2, 1))
            {
                MsgBox.ShowOK("选择的清单包含控货的运单,不能提货!");
                return;
            }
            string str = "";
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                str += ConvertType.ToString(myGridView2.GetRowCellValue(i, "BillNo")) + "@";
            }
            check();
            PrintQSD(str);
        }

        //异常签收
        private void btnErrorSign_Click(object sender, EventArgs e)
        {
            

            string BillNo = "";
            string type = "";
            if (myGridView2.RowCount > 1)
            {
                MsgBox.ShowOK("检测到待签收列表中有多单，异常签收每次只能签收一单，请先剔除掉多余的单！");
                return;
            }
            if (myGridView2.RowCount == 0)
            { return; }
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                type = myGridView2.GetRowCellValue(i, "SignType").ToString();
            }
            if (type == "自提签收")
            {
                if (myGridView2.RowCount > 0)
                {
                    //string PaymentMode = myGridView2.GetRowCellValue(0, "PaymentMode").ToString();
                    //if (PaymentMode == "月结")
                    //{
                    //    MsgBox.ShowOK("此单付款方式为月结，不允许做异常签收！");
                    //    return;
                    //}
                    BillNo = myGridView2.GetRowCellValue(0, "BillNo").ToString(); //允许月结客户做异常签收--毛慧20170911
                    try
                    {
                        List<SqlPara> list = new List<SqlPara>();
                        list.Add(new SqlPara("BillNo", BillNo));
                        SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_BILLPETTYPAY_BYBILLNO", list);
                        DataSet ds = SqlHelper.GetDataSet(sps);
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            MsgBox.ShowOK("此单已做过小额赔付，不允许异常签收 ");
                            return;
                        }
                        if (SignMan.Text.Trim() == "")
                        {
                            MsgBox.ShowOK("必须填写签收人！");
                            return;
                        }
                        if (SignContent.Text.Trim() == "")
                        {
                            MsgBox.ShowOK("异常签收必须填写提货说明！");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {

                        MsgBox.ShowException(ex);
                    }
                    frmErrorSignEdit edit = new frmErrorSignEdit();
                    edit.billNO = BillNo;
                    edit.signType = "自提签收";
                    edit.Text = "自提异常签收";
                    edit.signMan = SignMan.Text.Trim();
                    edit.signManPhone = SignManPhone.Text.Trim();
                    edit.signManCardID = SignManCardID.Text.Trim();
                    edit.ShowDialog();
                    if (edit.DialogResult != DialogResult.OK)
                    {
                        return;
                    }

                }
            }
            else if (type == "中转签收")  
            {
                if (myGridView2.RowCount > 0)
                {
                    //允许月结客户做异常签收
                    //string PaymentMode = myGridView2.GetRowCellValue(0, "PaymentMode").ToString();
                    //if (PaymentMode == "月结")
                    //{
                    //    MsgBox.ShowOK("此单付款方式为月结，不允许做异常签收！");
                    //    return;
                    //}
                    BillNo = myGridView2.GetRowCellValue(0, "BillNo").ToString();//允许月结客户做异常签收--毛慧20170911
                    try
                    {
                        List<SqlPara> list = new List<SqlPara>();
                        list.Add(new SqlPara("BillNo", BillNo));
                        SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_BILLPETTYPAY_BYBILLNO", list);
                        DataSet ds = SqlHelper.GetDataSet(sps);
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            MsgBox.ShowOK("此单已做过小额赔付，不允许异常签收");
                            return;
                        }
                        if (SignMan.Text.Trim() == "")
                        {
                            MsgBox.ShowOK("必须填写签收人！");
                            return;
                        }
                        if (SignContent.Text.Trim() == "")
                        {
                            MsgBox.ShowOK("异常签收必须填写提货说明！");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {

                        MsgBox.ShowException(ex);
                    }
                    frmErrorSignEdit edit = new frmErrorSignEdit();
                    edit.billNO = BillNo;
                    edit.signType = "中转签收";
                    edit.Text = "中转异常签收";
                    edit.signMan = SignMan.Text.Trim();
                    edit.signManPhone = SignManPhone.Text.Trim();
                    edit.signManCardID = SignManCardID.Text.Trim();
                    edit.ShowDialog();
                    if (edit.DialogResult != DialogResult.OK)
                    {
                        return;
                    }

                }
            }

                else if (type == "送货签收")       
                {
                    if (myGridView2.RowCount > 0)
                    {
                        //string PaymentMode = myGridView2.GetRowCellValue(0, "PaymentMode").ToString();
                        //if (PaymentMode == "月结")
                        //{
                        //    MsgBox.ShowOK("此单付款方式为月结，不允许做异常签收！");
                        //    return;
                        //}
                        BillNo = myGridView2.GetRowCellValue(0, "BillNo").ToString();//允许月结客户做异常签收--毛慧20170911
                        try
                        {
                            List<SqlPara> list = new List<SqlPara>();
                            list.Add(new SqlPara("BillNo", BillNo));
                            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_BILLPETTYPAY_BYBILLNO", list);
                            DataSet ds = SqlHelper.GetDataSet(sps);
                            if (ds != null && ds.Tables[0].Rows.Count > 0)
                            {
                                MsgBox.ShowOK("此单已做过小额赔付，不允许异常签收");
                                return;
                            }
                            if (SignMan.Text.Trim() == "")
                            {
                                MsgBox.ShowOK("必须填写签收人！");
                                return;
                            }
                            if (SignContent.Text.Trim() == "")
                            {
                                MsgBox.ShowOK("异常签收必须填写提货说明！");
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            MsgBox.ShowException(ex);
                            return;
                        }
                        frmErrorSignEdit edit = new frmErrorSignEdit();
                        edit.billNO = BillNo;
                        edit.signType = "送货签收";
                        edit.Text = "送货异常签收";
                        edit.signMan = SignMan.Text.Trim();
                        edit.signManPhone = SignManPhone.Text.Trim();
                        edit.signManCardID = SignManCardID.Text.Trim();
                        edit.ShowDialog();
                        if (edit.DialogResult != DialogResult.OK)
                        {
                            return;
                        }

                    }
                }
                else if (type == "司机直送签收")
                {
                    MsgBox.ShowOK("司机直送没有异常签收！");
                    return;
                }

            

            check();  
        }
        private void setdata()
        {
            string unitstr = "";
            string a;
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                a = myGridView2.GetRowCellValue(i, "FetchPay").ToString().Trim();
                if (!String.IsNullOrEmpty(a))
                {
                    unitstr += myGridView2.GetRowCellValue(i, "BillNo").ToString() + "@";
                }
            }
            try
            {
                List<SqlPara> list1 = new List<SqlPara>();
                list1.Add(new SqlPara("BillNoStr", unitstr));
                list1.Add(new SqlPara("ConfirmMan", CommonClass.UserInfo.UserName));
                list1.Add(new SqlPara("ConfirmWeb", CommonClass.UserInfo.WebName));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "USP_CHECK_FETCHPAYADUIT_MID_CONFIRM", list1);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    MsgBox.ShowOK("系统已默认做中转提付确认");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_SEND_FOR_SJZS_NEW", list);
                ds = SqlHelper.GetDataSet(spe);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                if (myGridView1.RowCount < 1000) myGridView1.BestFitColumns();
            }
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            btnName = "simpleButton1";
            if (CommonClass.CheckKongHuo(myGridView2, 1))
            {
                MsgBox.ShowOK("选择的清单包含控货的运单,不能提货!");
                return;
            }
            string str = "";
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                str += ConvertType.ToString(myGridView2.GetRowCellValue(i, "BillNo")) + "@";
            }
            check();

            PrintQSD(str);
        }

        private void btnPrintSign_Click(object sender, EventArgs e)
        {
            btnName = "btnPrintSign";
            if (CommonClass.CheckKongHuo(myGridView2, 1))
            {
                MsgBox.ShowOK("选择的清单包含控货的运单,不能提货!");
                return;
            }
            string str = "";
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                str += ConvertType.ToString(myGridView2.GetRowCellValue(i, "BillNo")) + "@";
            }
            check();
            PrintQSD(str);
        }

        private void TDQSlist_Click(object sender, EventArgs e)
        {
            TDQSlist.ShowDropDown();
        }

        private void barButtonItem19_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void barButtonItem20_ItemClick(object sender, ItemClickEventArgs e)
        {
            string BillNoStr = "";
            string signMan = SignMan.Text.Trim();
            string signManCardID = SignManCardID.Text.Trim();
            string signManPhone = SignManPhone.Text.Trim();
            string signContent = SignContent.Text.Trim();
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                BillNoStr = BillNoStr + myGridView2.GetRowCellValue(i, "BillNo") + "@";

            }
            if (string.IsNullOrEmpty(BillNoStr))
            {
                return;
            }
            if (CommonClass.UserInfo.companyid == "486")
            {
                string name = "";
                //int rowhandle = myGridView2.FocusedRowHandle;
                //if (rowhandle < 0) return;
                //string BillNo = myGridView2.GetRowCellValue(rowhandle, "BillNo").ToString();
                if (CommonClass.UserInfo.BookNote == "")
                {


                    name = CommonClass.UserInfo.IsAutoBill == false ? "托运单" : "托运单(打印条码)";
                }
                else
                {

                    if (CommonClass.UserInfo.companyid == "486" && CommonClass.UserInfo.WebName == "宝虹广州项目部")
                    {
                        name = "宝虹广州项目部托运单";
                    }
                    else if (CommonClass.UserInfo.companyid == "486" && CommonClass.UserInfo.WebName == "宝虹武汉东西湖营业部")
                    {
                        name = "宝虹广州总部配送部_签收单";
                    }
                    else if (CommonClass.UserInfo.companyid == "486" && CommonClass.UserInfo.WebName == "武汉遵义营业部")
                    {
                        name = "武汉遵义营业部托运单";
                    }

                    else
                    {
                        name = "宝虹托运单_签收单";
                    }

                   


                }
                frmRuiLangService.Print(name, SqlHelper.GetDataTable(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTBIll_TX_1", new List<SqlPara> { new SqlPara("BillNoStr", BillNoStr), new SqlPara("signman", SignMan.Text.Trim()), new SqlPara("signmanNo", SignManCardID.Text.Trim()) })), "");
            }
            else
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNoStr", BillNoStr));
                list.Add(new SqlPara("SignMan", SignMan.Text.Trim()));
                list.Add(new SqlPara("signManCardID", SignManCardID.Text.Trim()));
                list.Add(new SqlPara("signManPhone", SignManPhone.Text.Trim()));
                list.Add(new SqlPara("SignContent", SignContent.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLINFO_PRINT_QSD_new", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                MsgBox.ShowOK("输入成功！");
                //frmRuiLangService.Print("提货单", ds.Tables[0]);
                //jl20181127
                if (CommonClass.UserInfo.WebName == "上海青浦操作部"
                    || CommonClass.UserInfo.WebName == "上海青浦操作部1"
                    || CommonClass.UserInfo.WebName == "杭州操作部"
                    || CommonClass.UserInfo.WebName == "杭州操作部1"
                    || CommonClass.UserInfo.WebName == "无锡二级分拨中心"
                    || CommonClass.UserInfo.WebName == "无锡二级分拨中心1"
                    || CommonClass.UserInfo.WebName == "宁波操作部"
                    || CommonClass.UserInfo.WebName == "宁波操作部1"
                    || CommonClass.UserInfo.WebName == "济南二级分拨中心"
                    || CommonClass.UserInfo.WebName == "济南二级分拨中心1"
                    || CommonClass.UserInfo.WebName == "无锡二级分拨中心"
                    || CommonClass.UserInfo.WebName == "无锡二级分拨中心1"
                    || CommonClass.UserInfo.WebName == "武汉二级分拨中心"
                    || CommonClass.UserInfo.WebName == "武汉二级分拨中心1"
                    || CommonClass.UserInfo.WebName == "广州操作部"
                    || CommonClass.UserInfo.WebName == "广州操作部1"
                    || CommonClass.UserInfo.WebName == "东莞大坪分拨中心"
                    || CommonClass.UserInfo.WebName == "东莞大坪分拨中心1"
                    || CommonClass.UserInfo.WebName == "青岛二级分拨中心"
                    || CommonClass.UserInfo.WebName == "青岛二级分拨中心1")
                {
                    frmRuiLangService.Print("提货单大坪", ds.Tables[0]);
                }
                else
                {
                    frmRuiLangService.Print("提货单", ds.Tables[0]);
                }

            }



        }

        private void barButtonItem24_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmSelectOneBillSign frm = new frmSelectOneBillSign();
            if (barEditItem8.EditValue.ToString() == "自提签收")
            {
                frm.SignType = "FetchSign";
            }

            else if (barEditItem8.EditValue.ToString() == "送货签收")
            {
                frm.SignType = "SendSign";
            }

            else if (barEditItem8.EditValue.ToString() == "本地中转签收")
            {
                isLocal = 0;
                frm.SignType = "MiddleSign";
                frm.isLocal = isLocal;
            }

            else if (barEditItem8.EditValue.ToString() == "终端中转签收")
            {
                isLocal = 1;
                frm.SignType = "MiddleSign";
                frm.isLocal = isLocal;
            }

            else if (barEditItem8.EditValue.ToString() == "司机直送签收")
            {
                frm.SignType = "SJZS";
            }


            frm.webName = CommonClass.UserInfo.WebName;
            frm.siteName = CommonClass.UserInfo.SiteName;
            frm.ShowDialog();

            if (frm.rltDs != null && frm.rltDs.Tables.Count > 0)
            {
                ds.Clear();
                ds1.Clear();
                myGridView1.ClearColumnsFilter();
                myGridView2.ClearColumnsFilter();

                ds = frm.rltDs;
                ds1 = frm.rltDs.Clone();
                myGridControl1.DataSource = ds.Tables[0];
                myGridControl2.DataSource = ds1.Tables[0];
            }
        }

    }
}