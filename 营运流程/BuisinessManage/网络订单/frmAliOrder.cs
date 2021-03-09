using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using System.Net;
using System.IO;
using System.Web;
using System.Xml;
using System.Security.Cryptography;
using System.Threading;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using ZQTMS.Tool;
using ZQTMS.Common;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ZQTMS.SqlDAL;
using DevExpress.XtraReports.UI;

namespace ZQTMS.UI
{
    public partial class frmAliOrder : BaseForm
    {
        public frmAliOrder()
        {
            InitializeComponent();
        }

        DataSet ds = new DataSet();
        XtraReport rpt = new XtraReport();//为了加快打印标签的显示速度

        //加载事件
        private void w_online_order_Load(object sender, EventArgs e)
        {
            GridOper.RestoreGridLayout(gridView1, "阿里网上订单");
            bdate.EditValue = CommonClass.gbdate;  //开始时间推前两个星期
            edate.EditValue = CommonClass.gedate;  //当天时间

            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            FixColumn fix = new FixColumn(gridView1, barSubItem2);

            //加载标签
            Thread tt = new Thread(load_rpt);
            tt.IsBackground = true;
            tt.Start();

            //权限设置
            //barButtonItem1.Enabled =  ur.GetUserRightDetail888("ali1");
            //barButtonItem2.Enabled =  ur.GetUserRightDetail888("ali1");

            //barButtonItem3.Enabled =  ur.GetUserRightDetail888("ali2");

            //barButtonItem13.Enabled = ur.GetUserRightDetail888("ali3");
            //barButtonItem14.Enabled =  ur.GetUserRightDetail888("ali3");

            //barButtonItem12.Enabled = ur.GetUserRightDetail888("ali4");
            //barButtonItem15.Enabled = ur.GetUserRightDetail888("ali4");

            //barButtonItem16.Enabled = ur.GetUserRightDetail888("ali6");


            //if (commonclass.issa)
            //{
            //    GetALIState(gridColumn30);
            //    GetALIState(gridColumn3);

            //}
            //if (commonclass.reptitle.Contains("明亮"))
            //{
            //    GetALIStateML(gridColumn30);
            //    GetALIStateML(gridColumn3);
            //}
            CommonClass.SetSite(comboBoxEdit4, true);
        }

        private void load_rpt()
        {
            //加载标签
            string fileName = Application.StartupPath + "\\Reports\\标签.repx";
            if (File.Exists(fileName))
            {
                rpt.LoadLayout(fileName);
            }
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            gridControl1.DataSource = null;

            if (bdate.DateTime > edate.DateTime)
            {
                XtraMessageBox.Show("开始日期不能大于结束日期.", "日期选择错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string site = comboBoxEdit4.Text.Trim() == "全部" ? "%%" : comboBoxEdit4.Text.Trim();
            string statusType = orderstate.Text.Trim();

            if (statusType == "受理")
            {
                statusType = "ACCEPT";
            }
            else if (statusType == "不受理")
            {

                statusType = "UNACCEPT";
            }
            else if (statusType == "揽收")
            {

                statusType = "GOT";
            }
            else if (statusType == "揽收失败")
            {
                statusType = "NOGET";

            }
            else if (statusType == "签收")
            {
                statusType = "SIGNSUCCESS";

            }
            else if (statusType == "签收异常")
            {
                statusType = "SIGNFAILED";

            }
            else if (statusType == "新")
            {
                statusType = "NEW";

            }
            else if (statusType == "撤销")
            {
                statusType = "CANCEL";

            }


            else if (statusType == "全部")
            {
                statusType = "%%";
            }




            //DataSet dataSet = new DataSet();
            //try
            //{
            //    SqlCommand cmd = new SqlCommand("QSP_GET_WEB_ORDER_EX");
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.Add("@t1", SqlDbType.DateTime).Value = bdate.DateTime;
            //    cmd.Parameters.Add("@t2", SqlDbType.DateTime).Value = edate.DateTime;
            //    cmd.Parameters.Add("@site", SqlDbType.VarChar).Value = site;
            //    cmd.Parameters.Add("@state", SqlDbType.VarChar).Value = statusType;
            //    SqlDataAdapter da = new SqlDataAdapter(cmd);
            //    da.Fill(dataSet);
            //    gridControl1.DataSource = dataSet.Tables[0];
            //}
            //catch (Exception ex)
            //{
            //    XtraMessageBox.Show(ex.Message);
            //}
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.Text.Trim()));
                list.Add(new SqlPara("t2", edate.Text.Trim()));
                list.Add(new SqlPara("site", site));
                list.Add(new SqlPara("state", statusType));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WEB_ORDER_EX", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                gridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }



        #region 查看订单信息   开始
        private void gridControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //OrderInfo();

        }


        private void OrderModify(string statusType)
        {
            try
            {
                int rows = gridView1.FocusedRowHandle;
                if (rows < 0)
                {
                    return;
                }

                //statusType:
                //ACCEPT	受理
                //UNACCEPT 不受理
                //GOT 揽收成功
                //NOGET 揽收失败
                //SIGNSUCCESS 签收成功
                //SIGNFAILED 签收异常
                //CANCEL 撤销
                DialogResult dr = DialogResult.No;

                if (statusType == "ACCEPT")
                {
                    dr = XtraMessageBox.Show("是否受理订单？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                }
                else if (statusType == "UNACCEPT")
                {
                    dr = XtraMessageBox.Show("是否不受理订单？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                }
                else if (statusType == "GOT")
                {
                    dr = XtraMessageBox.Show("是否揽收订单？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                }
                else if (statusType == "NOGET")
                {
                    dr = XtraMessageBox.Show("是否订单揽收失败？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                }
                else if (statusType == "SIGNSUCCESS")
                {
                    dr = XtraMessageBox.Show("是否签收订单？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                }
                else if (statusType == "SIGNFAILED")
                {
                    dr = XtraMessageBox.Show("是否签收异常？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                }


                if (dr == DialogResult.Yes)
                {
                    string logisticID = gridView1.GetRowCellValue(rows, "logisticID").ToString();
                    string logisticCompanyID = gridView1.GetRowCellValue(rows, "logisticCompanyID").ToString();
                    string mailNo = gridView1.GetRowCellValue(rows, "mailNo").ToString();

                    System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
                    long time_c = (long)Math.Round((CommonClass.gcdate - startTime).TotalMilliseconds, 0);

                    order order = new order();
                    order.logisticCompanyID = logisticCompanyID;
                    order.logisticID = logisticID;
                    order.gmtUpdated = time_c;
                    order.mailNo = mailNo;
                    order.statusType = statusType;
                    order.comments = "";


                    if (statusType == "GOT")
                    {
                        if (mailNo == "")
                        {
                            XtraMessageBox.Show("还未生成运单号，不能揽收！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }


                    string param = "";// JsonConvert.SerializeObject(order);

                    param = "{\"logisticCompanyID\":\"" + logisticCompanyID + "\",\"logisticID\":\"" + logisticID + "\",\"mailNo\":\"" + mailNo + "\",\"gmtUpdated\":" + time_c + ",\"statusType\":\"" + statusType + "\",\"comments\":\"\"}";

                    string p = param + 1006126 + "nhOgXK}kxDSS";
                    //string p = param + 1006949 + ";3ep.9_Ng7B3";
                    //string md = FormsAuthentication.HashPasswordForStoringInConfigFile(p, "MD5");
                    string md = md5(p);
                    //加密base64
                    byte[] bytes = Encoding.Default.GetBytes(md);
                    string digest = Convert.ToBase64String(bytes);

                    //解码base64
                    //byte[] outputb = Convert.FromBase64String(Convert.ToBase64String(bytes));
                    //string orgStr = Encoding.Default.GetString(outputb);

                    //string url = "http://gw.open.china.alibaba.com/openapi/param2/1/cn.alibaba.logistics/alp.update/1006126?_aop_responseFormat=json";

                    string url = "http://gw.open.1688.com/openapi/param2/1/cn.alibaba.logistics/alp.update/1006126?_aop_responseFormat=json";

                    //string url = "http://110.75.196.33/openapi/param2/1/cn.alibaba.logistics/alp.update/1006126?";

                    string pa = "format=json&params=" + param + "&timestamp=" + time_c + "&digest=" + digest;

                    string result = sendMessage(url, pa);

                    //string result = "{\"InvokeStartTime\":\"20131104142658305+0800\",\"InvokeCostTime\":39,\"Status\":{\"Code\":200,\"Message\":\"OK\"},\"Responses\":[{\"Status\":{\"Code\":200,\"Message\":\"OK\"},\"Result\":{\"logisticID\":\"AL00018382099897\",\"result\":true,\"reason\":null,\"resultCode\":\"1000\",\"resultInfo\":\"成功\"}}]}";

                    JObject json = JsonConvert.DeserializeObject(result) as JObject;//执行反序列化
                    JArray a = ((JArray)(json["Responses"]));

                    JObject obj = a[0] as JObject;
                    JObject Result = (JObject)obj.SelectToken("Result");
                    String resultCode = Result["resultCode"].ToString(); // obj.SelectToken("resultCode").ToString();

                    if (resultCode == "1000")
                    {
                        saveState(logisticID, statusType);
                        gridView1.SetRowCellValue(rows, "statusType", statusType);
                        XtraMessageBox.Show("操作成功！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        String reason = Result["reason"].ToString();
                        XtraMessageBox.Show("操作失败，阿里巴巴提示：" + reason, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 查看订单信息   结束

        public string md5(string passWord)
        {
            Byte[] clearBytes = Encoding.Default.GetBytes(passWord);
            Byte[] hashedBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);
            string result = BitConverter.ToString(hashedBytes).Replace("-", "");
            return result.ToLower();
        }


        //post
        public static string sendMessage(string strUrl, string PostStr)
        {
            string strResponse = "";
            try
            {
                CookieContainer objCookieContainer = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUrl);
                request.Method = "Post";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Referer = strUrl;//.Remove(strUrl.LastIndexOf("/"));
                Console.WriteLine(strUrl);
                if (objCookieContainer == null)
                    objCookieContainer = new CookieContainer();

                request.CookieContainer = objCookieContainer;
                Console.WriteLine(objCookieContainer.ToString());
                byte[] byteData = Encoding.UTF8.GetBytes(PostStr.ToString().TrimEnd('&'));
                request.ContentLength = byteData.Length;
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(byteData, 0, byteData.Length);
                    // reqStream.Close();
                }


                using (HttpWebResponse res = (HttpWebResponse)request.GetResponse())
                {
                    objCookieContainer = request.CookieContainer;
                    Console.WriteLine(objCookieContainer);
                    Console.WriteLine(res.Server);
                    Console.WriteLine(res.ResponseUri);
                    using (Stream resStream = res.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(resStream, Encoding.UTF8))//.UTF8))
                        {
                            strResponse = sr.ReadToEnd();
                        }
                    }
                    res.Close();
                }
                return strResponse;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }


        private void saveState(string logisticID, string statusType)
        {
            //try
            //{
            //    SqlCommand com = new SqlCommand("USP_ADD_MODIFY_ORDETstatusType");
            //    com.CommandType = System.Data.CommandType.StoredProcedure;
            //    com.Parameters.Add("@logisticID", SqlDbType.VarChar).Value = logisticID;
            //    com.Parameters.Add("@statusType", SqlDbType.VarChar).Value = statusType;

            //}
            //catch (Exception ex)
            //{
            //    XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("logisticID", logisticID));
                list.Add(new SqlPara("statusType", statusType));
                list.Add(new SqlPara("createby", CommonClass.UserInfo.UserName));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_MODIFY_ORDETstatusType", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }




        }

        //数据行背景颜色
        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            //if(e.RowHandle >=0)
            //{
            //    string sState = gridView1.GetRowCellDisplayText(e.RowHandle, gridView1.Columns["state"]);
            //    switch (sState)
            //    {
            //        case "已受理":
            //            e.Appearance.BackColor = ColorTranslator.FromHtml("#C0FFC0");
            //            break;
            //        case "已作废":
            //            e.Appearance.BackColor = Color.Pink;
            //            break;
            //    }
            //}

        }

        //自动筛选
        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(gridView1);
        }

        //锁定外观
        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(gridView1, "阿里网上订单");
        }

        //取消外观
        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(gridView1, "阿里网上订单");
        }

        //过滤器
        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(gridView1);
        }

        //受理
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OrderModify("ACCEPT");
        }

        //不受理
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OrderModify("UNACCEPT");
        }

        // 揽收成功
        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OrderModify("GOT");
        }
        // 揽收失败
        private void barButtonItem14_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OrderModify("NOGET");
        }

        //生成运单
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle >= 0)
            {
                DataRow dr = gridView1.GetDataRow(rowhandle);
                //frmWayBillAdd fwb = frmWayBillAdd.Get_frmWayBillAdd;
                frmWayBillAdd fwb = new frmWayBillAdd();
                fwb.alidr = dr;
                fwb.rpt = rpt;
                fwb.ShowDialog();
                if (fwb.alibillno == "") return;
                try
                {
                    string logisticID = gridView1.GetRowCellValue(rowhandle, "logisticID").ToString();
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("logisticID", logisticID));
                    list.Add(new SqlPara("mailNo", fwb.alibillno));
                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_mailNo", list);
                    if (SqlHelper.ExecteNonQuery(sps) > 0)
                    {
                        MsgBox.ShowOK();
                        gridView1.SetRowCellValue(rowhandle, "mailNo", fwb.alibillno);
                    }
                    MsgBox.ShowOK();
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
        }
        // 签收成功
        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OrderModify("SIGNSUCCESS");
        }
        // 签收异常
        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OrderModify("SIGNFAILED");
        }

        //导出Excel
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(gridView1);
        }

        //关闭阿里网上订单窗体
        private void cbClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            //cc.GenSeq(e);
        }

        private void barButtonItem16_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle >= 0)
            {

                string logisticID = gridView1.GetRowCellValue(rowhandle, "logisticID").ToString();

                frmBillNo wmm = new frmBillNo();
                wmm.edlogisticID.Text = logisticID;
                DialogResult dr = wmm.ShowDialog();

                if (dr == DialogResult.OK)
                {
                    try
                    {

                        if (wmm.edmailNo.Text.Trim() == "")
                        {
                            XtraMessageBox.Show("", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        //SqlCommand com = new SqlCommand("USP_ADD_mailNo");
                        //com.CommandType = System.Data.CommandType.StoredProcedure;
                        //com.Parameters.Add("@logisticID", SqlDbType.VarChar).Value = logisticID;
                        //com.Parameters.Add("@mailNo", SqlDbType.VarChar).Value = wmm.edmailNo.Text.Trim();
                        //com.ExecuteNonQuery();

                        List<SqlPara> list = new List<SqlPara>();
                        list.Add(new SqlPara("logisticID", logisticID));
                        list.Add(new SqlPara("mailNo", wmm.edmailNo.Text.Trim()));

                        SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_mailNo", list);
                        if (SqlHelper.ExecteNonQuery(sps) > 0)
                        {
                            gridView1.SetRowCellValue(rowhandle, "mailNo", wmm.edmailNo.Text.Trim());
                            MsgBox.ShowOK();
                        }



                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }




            }

        }

        public void GetALIState(GridColumn gc)
        {
            RepositoryItemImageComboBox riicb = new RepositoryItemImageComboBox();
            riicb.AutoHeight = false;
            riicb.Buttons.AddRange(new EditorButton[] {
            new EditorButton(ButtonPredefines.Combo)});

            riicb.Items.AddRange(new ImageComboBoxItem[] {
            
            new ImageComboBoxItem("杭州石大营业部", "sazj003", -1),
            new ImageComboBoxItem("杭州拱墅登云路", "sazj030", -1),
            new ImageComboBoxItem("杭州西湖文三路", "sazj029", -1),
            new ImageComboBoxItem("杭州萧山合作点", "sazj031", -1),
            new ImageComboBoxItem("杭州余杭营业部", "sazj002", -1),

            new ImageComboBoxItem("杭州桐庐合作点", "sazj036", -1),
            new ImageComboBoxItem("杭州淳安合作点", "sazj035", -1),
            new ImageComboBoxItem("杭州建德合作点", "sazj033", -1),
            new ImageComboBoxItem("杭州富阳合作点", "sazj032", -1),
            new ImageComboBoxItem("宁波海曙东渡路", "sazj022", -1),



            new ImageComboBoxItem("宁波江东朝晖路", "sazj023", -1),
            new ImageComboBoxItem("宁波镇海派送部", "sazj010", -1),
            new ImageComboBoxItem("宁波镇海营业部", "sazj001", -1),
            new ImageComboBoxItem("宁波鄞州合作点", "sazj021", -1),
            new ImageComboBoxItem("宁波象山合作点", "sazj028", -1),



            new ImageComboBoxItem("宁波宁海合作点", "sazj027", -1),
            new ImageComboBoxItem("宁波余姚合作点", "sazj025", -1),
            new ImageComboBoxItem("宁波慈溪营业部", "sazj024", -1),
            new ImageComboBoxItem("宁波慈溪营业部", "szjz006", -1),
            new ImageComboBoxItem("宁波奉化合作点", "sazj026", -1),


            new ImageComboBoxItem("温州鹿城飞霞路", "sazj047", -1),
            new ImageComboBoxItem("温州瓯海营业部", "sajz007", -1),
            new ImageComboBoxItem("温州永嘉合作点", "sazj053", -1),
            new ImageComboBoxItem("温州平阳合作点", "sazj052", -1),
            new ImageComboBoxItem("温州苍南合作点", "sazj050", -1),
            		

            new ImageComboBoxItem("温州文成合作点", "sazj055", -1),
            new ImageComboBoxItem("温州泰顺合作点", "sazj054", -1),
            new ImageComboBoxItem("温州瑞安合作点", "sazj049", -1),
            new ImageComboBoxItem("嘉兴南湖禾兴路", "sazj063", -1),
            new ImageComboBoxItem("嘉兴南湖营业部", "sazj005", -1),


            new ImageComboBoxItem("嘉兴秀洲合作点", "sazj064", -1),
            new ImageComboBoxItem("嘉兴海盐合作点", "sazj069", -1),
            new ImageComboBoxItem("嘉兴海宁合作点", "sazj065", -1),
            new ImageComboBoxItem("嘉兴平湖合作点", "sazj066", -1),
            new ImageComboBoxItem("嘉兴桐乡合作点", "sazj067", -1),


            new ImageComboBoxItem("湖州吴兴合作点", "sazj070", -1),
            new ImageComboBoxItem("湖州安吉合作点", "sazj073", -1),
            new ImageComboBoxItem("绍兴越城中兴路", "sazj075", -1),
            new ImageComboBoxItem("绍兴越城合作点", "sazj074", -1),
            new ImageComboBoxItem("绍兴柯桥合作点", "sazj076", -1),
            		

            new ImageComboBoxItem("绍兴新昌合作点", "sazj080", -1),
            new ImageComboBoxItem("绍兴诸暨合作点", "sazj077", -1),
            new ImageComboBoxItem("绍兴上虞合作点", "sazj078", -1),
            new ImageComboBoxItem("绍兴嵊州合作点", "sazj079", -1),
            new ImageComboBoxItem("金华婺城环城路", "sazj040", -1),
            		

            new ImageComboBoxItem("金华金东合作点", "sazj039", -1),
            new ImageComboBoxItem("金华武义合作点", "sazj046", -1),
            new ImageComboBoxItem("金华浦江合作点", "sazj045", -1),
            new ImageComboBoxItem("金华磐安合作点", "sazj044", -1),
            new ImageComboBoxItem("金华兰溪合作点", "sazj042", -1),
            		


            new ImageComboBoxItem("义乌国际商贸城", "sazj037", -1),
            new ImageComboBoxItem("义乌江东营业部", "sajz009", -1),
            new ImageComboBoxItem("义乌江北下朱营业部", "sazj004", -1),
            new ImageComboBoxItem("义乌稠州宾王路", "sazj038", -1),
            new ImageComboBoxItem("金华东阳合作点", "sazj043", -1),


            new ImageComboBoxItem("金华永康合作点", "sazj041", -1),
            new ImageComboBoxItem("衢州柯城合作点", "sazj081", -1),
            new ImageComboBoxItem("衢州常山合作点", "sazj083", -1),
            new ImageComboBoxItem("衢州开化合作点", "sazj084", -1),
            new ImageComboBoxItem("衢州龙游合作点", "sazj085", -1),
            	

            		
            new ImageComboBoxItem("衢州江山合作点", "sazj082", -1),
            new ImageComboBoxItem("舟山定海合作点", "sazj095", -1),
            new ImageComboBoxItem("台州路桥腾达路", "sazj056", -1),
            new ImageComboBoxItem("台州路桥营业部", "sajz008", -1),
            new ImageComboBoxItem("台州玉环合作点", "sazj062", -1),

            		

            new ImageComboBoxItem("台州三门合作点", "sazj059", -1),
            new ImageComboBoxItem("台州天台合作点", "sazj060", -1),
            new ImageComboBoxItem("台州仙居合作点", "sazj061", -1),
            new ImageComboBoxItem("台州温岭合作点", "sazj057", -1),
            new ImageComboBoxItem("台州临海合作点", "sazj058", -1),


            new ImageComboBoxItem("丽水莲都合作点", "sazj086", -1),
            new ImageComboBoxItem("丽水青田合作点", "sazj088", -1),
            new ImageComboBoxItem("丽水缙云合作点", "sazj094", -1),
            new ImageComboBoxItem("丽水松阳合作点", "sazj092", -1),
            new ImageComboBoxItem("丽水庆元合作点", "sazj091", -1),


            new ImageComboBoxItem("丽水龙泉合作点", "sazj087", -1),
            new ImageComboBoxItem("深圳清水河营业部", "sagd014", -1),
            new ImageComboBoxItem("深圳华强北营业部", "sagd015", -1),
            new ImageComboBoxItem("深圳华通源营业部", "sagd013", -1),
            new ImageComboBoxItem("深圳宝运达营业部", "sagd012", -1),

            	
            new ImageComboBoxItem("深圳机场营业部", "sagd011", -1),
            new ImageComboBoxItem("深圳松岗营业部", "sagd010", -1),
            new ImageComboBoxItem("深圳沙井营业部", "sagd009", -1),
            new ImageComboBoxItem("深圳石岩营业部", "sagd008", -1),
            new ImageComboBoxItem("深圳同乐营业部", "sagd007", -1),
            

            new ImageComboBoxItem("深圳良安田营业部", "sagd006", -1),
            new ImageComboBoxItem("深圳金鹏营业一部", "sagd005", -1),
            new ImageComboBoxItem("惠州惠城营业部", "sagd004", -1),
            new ImageComboBoxItem("东莞万江营业部", "sagd002", -1),
            new ImageComboBoxItem("东莞大朗营业部", "sagd003", -1),

            new ImageComboBoxItem("圣安东莞总部", "sagd001", -1)
            		
            
            });
            gc.ColumnEdit = riicb;
        }


        public void GetALIStateML(GridColumn gc)
        {
            RepositoryItemImageComboBox riicb = new RepositoryItemImageComboBox();
            riicb.AutoHeight = false;
            riicb.Buttons.AddRange(new EditorButton[] {
            new EditorButton(ButtonPredefines.Combo)});

            riicb.Items.AddRange(new ImageComboBoxItem[] {
            
            new ImageComboBoxItem("上海嘉定营业部", "13", -1),
            new ImageComboBoxItem("上海松江分公司", "07", -1),
            new ImageComboBoxItem("无锡分公司", "10", -1),
            new ImageComboBoxItem("无锡营业二部", "34", -1),
            new ImageComboBoxItem("徐州合作营业部", "20", -1),

            new ImageComboBoxItem("苏州分公司", "09", -1),
            new ImageComboBoxItem("昆山分公司", "08", -1),
            new ImageComboBoxItem("如皋合作营业部", "22", -1),
            new ImageComboBoxItem("盐城合作营业部", "21", -1),
            new ImageComboBoxItem("丹阳营业部", "26", -1),

            new ImageComboBoxItem("杨中营业部", "28", -1),
            new ImageComboBoxItem("镇江营业部", "27", -1),
            new ImageComboBoxItem("泰州营业部", "24", -1),
            new ImageComboBoxItem("清水河营业部", "30", -1),
            new ImageComboBoxItem("长城营业部", "12", -1),

            new ImageComboBoxItem("华强北营业部", "33", -1),
            new ImageComboBoxItem("华通源营业部", "15", -1),
            new ImageComboBoxItem("宝安宝运达营业部", "03", -1),
            new ImageComboBoxItem("宝安松岗营业部", "04", -1),
            new ImageComboBoxItem("沙井营业部", "11", -1),

            new ImageComboBoxItem("金华伦营业部", "01", -1),
            new ImageComboBoxItem("金鹏营业部", "02", -1),
            new ImageComboBoxItem("龙岗营业部", "05", -1),
            new ImageComboBoxItem("东莞万江营业部", "14", -1),
            new ImageComboBoxItem("东莞凤岗营业部", "06", -1),


            new ImageComboBoxItem("大朗营业部", "29", -1),
            new ImageComboBoxItem("南宁营业部", "17", -1),
            new ImageComboBoxItem("柳州营业部", "19", -1),
            new ImageComboBoxItem("桂林营业部", "18", -1)


            		
            
            });
            gc.ColumnEdit = riicb;
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle < 0) return;

            int unit = gridView1.GetRowCellValue(rowhandle, "unit") == DBNull.Value ? 0 : Convert.ToInt32(gridView1.GetRowCellValue(rowhandle, "unit"));
            if (unit == 0) return;

            //cc.ShowBillDetail(unit);
        }
    }


    class order
    {
        private string LogisticCompanyID;

        public string logisticCompanyID
        {
            get { return LogisticCompanyID; }
            set { LogisticCompanyID = value; }
        }

        private string LogisticID;

        public string logisticID
        {
            get { return LogisticID; }
            set { LogisticID = value; }
        }

        private string MailNo;

        public string mailNo
        {
            get { return MailNo; }
            set { MailNo = value; }
        }

        private long GmtUpdated;

        public long gmtUpdated
        {
            get { return GmtUpdated; }
            set { GmtUpdated = value; }
        }

        private string StatusType;

        public string statusType
        {
            get { return StatusType; }
            set { StatusType = value; }
        }

        private string Comments;

        public string comments
        {
            get { return Comments; }
            set { Comments = value; }
        }
    }
}
