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
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using DevExpress.XtraGrid.Views.Grid;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace ZQTMS.UI
{
    public partial class fmBadBill : BaseForm
    {
        public fmBadBill()
        {
            InitializeComponent();
        }

        private void getdata()
        {
            if (bdate.DateTime > edate.DateTime)
            {
                XtraMessageBox.Show("开始日期不能大于结束日期", "日期选择错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("webid", edwebid.Text.Trim() == "全部" ? "%%" : edwebid.Text.Trim()));
                string hfstate = "";
                string shstate = "";

                if (comboBoxEdit1.Text.Trim() == "全部")
                {
                    hfstate = "%%";
                }
                else if (comboBoxEdit1.Text.Trim() == "已划分")
                {
                    hfstate = "1";
                }
                else if (comboBoxEdit1.Text.Trim() == "未划分")
                {
                    hfstate = "0";
                }
                else if (comboBoxEdit1.Text.Trim() == "否决")
                {
                    hfstate = "2";
                }

                if (comboBoxEdit2.Text.Trim() == "全部")
                {
                    shstate = "%%";
                }
                else if (comboBoxEdit2.Text.Trim() == "已审核")
                {
                    shstate = "1";
                }
                else if (comboBoxEdit2.Text.Trim() == "未审核")
                {
                    shstate = "0";
                }
                else if (comboBoxEdit2.Text.Trim() == "否决")
                {
                    shstate = "2";
                }
                list.Add(new SqlPara("gzstate", dateEdit1.Text.Trim()));
                list.Add(new SqlPara("hfstate", hfstate));
                list.Add(new SqlPara("shstate", shstate));
                list.Add(new SqlPara("webid1", comboBoxEdit3.Text.Trim() == "全部" ? "%%" : comboBoxEdit3.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BAD_TYD_CL", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;

                gridshow.DataSource = ds;
                gridshow.DataMember = ds.Tables[0].ToString();                
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void fmBadBill_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("异常登记");//xj/2019/5/28
            CommonClass.FormSet(this);
            bdate.EditValue = CommonClass.gbdate;
            edate.EditValue = CommonClass.gedate;
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            //edwebid.Text = CommonClass.UserInfo.SiteName;
            GridOper.RestoreGridLayout(gridView2, "货差货损记录");

            CommonClass.SetWeb(edwebid, edwebid.Text);
            CommonClass.SetWeb(comboBoxEdit3, edwebid.Text);
            GridOper.RestoreGridLayout(gridView2, "货差货损记录");
            FixColumn fix = new FixColumn(gridView2, barSubItem2);

            barButtonItem14.Visibility = BarItemVisibility.Never;//提交到OA不显示了。在服务器上自动推送

            //未处理
            GridOper.CreateStyleFormatCondition(gridView2, "AllState", DevExpress.XtraGrid.FormatConditionEnum.Equal, "否决", Color.Red);
            //已处理
            GridOper.CreateStyleFormatCondition(gridView2, "AllState", DevExpress.XtraGrid.FormatConditionEnum.Equal, "未划分", Color.Red);
            //未仲裁
            GridOper.CreateStyleFormatCondition(gridView2, "AllState", DevExpress.XtraGrid.FormatConditionEnum.Equal, "已划分", Color.FromArgb(192, 0, 0));
            //已仲裁
            GridOper.CreateStyleFormatCondition(gridView2, "AllState", DevExpress.XtraGrid.FormatConditionEnum.Equal, "已审核", Color.Cyan);
          
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rows = gridView2.FocusedRowHandle;
            if (rows < 0) return;

            fmBadBillDeal wb = new fmBadBillDeal();
            wb.gv = gridView2;
            wb.look = 10;
            wb.ShowDialog();
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rows = gridView2.FocusedRowHandle;
            if (rows < 0) return;

            //int id = Convert.ToInt32(gridView2.GetRowCellValue(rows, "id"));
            string id = gridView2.GetRowCellValue(rows, "ID").ToString();
            string badchuliman = gridView2.GetRowCellValue(rows, "badchuliman").ToString();//处理人
            string badzerenchuliman = gridView2.GetRowCellValue(rows, "badzerenchuliman").ToString();//责任划分人
            string badshman = gridView2.GetRowCellValue(rows, "badshman").ToString();//审核人

            int shstate = Convert.ToInt32(gridView2.GetRowCellValue(rows, "shstate").ToString() == "" ? "0" : gridView2.GetRowCellValue(rows, "shstate").ToString());
            int hfstate = Convert.ToInt32(gridView2.GetRowCellValue(rows, "hfstate").ToString() == "" ? "0" : gridView2.GetRowCellValue(rows, "hfstate").ToString());



            if (shstate > 0)
            {
                MsgBox.ShowOK("已审批，不能修改！");
                return;
            }

            fmBadBillDeal wb = new fmBadBillDeal();
            wb.gv = gridView2;
            wb.look = 4;
            wb.ShowDialog();
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rows = gridView2.FocusedRowHandle;
            if (rows < 0) return;

            //int id = Convert.ToInt32(gridView2.GetRowCellValue(rows, "id"));
            string id = gridView2.GetRowCellValue(rows, "ID").ToString();
            string badchuliman = gridView2.GetRowCellValue(rows, "badchuliman").ToString();//处理人
            string badzerenchuliman = gridView2.GetRowCellValue(rows, "badzerenchuliman").ToString();//责任划分人
            string badshman = gridView2.GetRowCellValue(rows, "badshman").ToString();//审核人
            string badcreateby = gridView2.GetRowCellValue(rows, "badcreateby").ToString();//审核人

            int shstate = Convert.ToInt32(gridView2.GetRowCellValue(rows, "shstate").ToString() == "" ? "0" : gridView2.GetRowCellValue(rows, "shstate").ToString());
            int hfstate = Convert.ToInt32(gridView2.GetRowCellValue(rows, "hfstate").ToString() == "" ? "0" : gridView2.GetRowCellValue(rows, "hfstate").ToString());

            string cancelman = gridView2.GetRowCellValue(rows, "cancelman").ToString();//取消人
            string badchuliyijian = gridView2.GetRowCellValue(rows, "badchuliyijian").ToString();//取消人
            string billNo = gridView2.GetRowCellValue(rows, "BillNo").ToString();//HJ20180514
            string abnormalityState=gridView2.GetRowCellValue(rows, "abnormalityState").ToString();//HJ20180514


            if (badcreateby != CommonClass.UserInfo.UserName)
            {
                MsgBox.ShowOK("只有撤销本人登记的异常！");
                return;
            }

            else if (badchuliyijian != "")
            {
                MsgBox.ShowOK("已跟踪，不能撤销！");
                return;
            }
            else if (shstate > 0)
            {
                MsgBox.ShowOK("已审核，不能撤销！");
                return;
            }
            else if (hfstate > 0)
            {
                MsgBox.ShowOK("已责任划分，不能撤销！");
                return;
            }
            else if (cancelman != "")
            {
                MsgBox.ShowOK("已被取消责任划分，但不能撤销！");
                return;
            }
            else if (abnormalityState == "已完结")
            {
                MsgBox.ShowOK("本运单异常处理状态为已完结，不能撤销异常！");
                return;
            }

            if (DialogResult.Yes != MsgBox.ShowYesNo("确定撤销？")) return;
            //撤销登记 数据要同步到ZQTMS  hj20180514
            Common.CommonSyn.UndoAbnormalSyn(billNo);
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("id", id));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_BAD_TYD_SA", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    gridView2.DeleteRow(rows);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rows = gridView2.FocusedRowHandle;
            if (rows < 0) return;

            //int id = Convert.ToInt32(gridView2.GetRowCellValue(rows, "id"));
            string id = gridView2.GetRowCellValue(rows, "ID").ToString();
            string badchuliman = gridView2.GetRowCellValue(rows, "badchuliman").ToString();//处理人
            string badzerenchuliman = gridView2.GetRowCellValue(rows, "badzerenchuliman").ToString();//责任划分人
            string badshman = gridView2.GetRowCellValue(rows, "badshman").ToString();//审核人
            int shstate = Convert.ToInt32(gridView2.GetRowCellValue(rows, "shstate").ToString() == "" ? "0" : gridView2.GetRowCellValue(rows, "shstate").ToString());
            int hfstate = Convert.ToInt32(gridView2.GetRowCellValue(rows, "hfstate").ToString() == "" ? "0" : gridView2.GetRowCellValue(rows, "hfstate").ToString());
            fmBadBillDeal wb = new fmBadBillDeal();
            wb.gv = gridView2;
            wb.look = 1;
            wb.ShowDialog();
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rows = gridView2.FocusedRowHandle;
            if (rows < 0) return;

            //int id = Convert.ToInt32(gridView2.GetRowCellValue(rows, "id"));
            string id = gridView2.GetRowCellValue(rows, "ID").ToString();
            string badchuliman = gridView2.GetRowCellValue(rows, "badchuliman").ToString();//处理人
            string badzerenchuliman = gridView2.GetRowCellValue(rows, "badzerenchuliman").ToString();//责任划分人
            int shstate = Convert.ToInt32(gridView2.GetRowCellValue(rows, "shstate").ToString() == "" ? "0" : gridView2.GetRowCellValue(rows, "shstate").ToString());

            //if (badchuliman == "")
            //{
            //    commonclass.MsgBox.ShowOK("未处理，不能进行责任划分！");
            //    return;
            //}
            //else if (badzerenchuliman != "")
            //{
            //    commonclass.MsgBox.ShowOK("已划分责任，不需要再处理！");
            //    return;
            //}
            //else
            if (shstate > 0)
            {
                MsgBox.ShowOK("已审批，不需要再处理！");
                return;
            }
            fmBadBillDeal wb = new fmBadBillDeal();
            wb.gv = gridView2;
            wb.look = 2;
            wb.ShowDialog();
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rows = gridView2.FocusedRowHandle;
            if (rows < 0) return;

            //int id = Convert.ToInt32(gridView2.GetRowCellValue(rows, "id"));
            string id = gridView2.GetRowCellValue(rows, "ID").ToString();
            string badchuliman = gridView2.GetRowCellValue(rows, "badchuliman").ToString();//处理人
            string badzerenchuliman = gridView2.GetRowCellValue(rows, "badzerenchuliman").ToString();//责任划分人
            string badshman = gridView2.GetRowCellValue(rows, "badshman").ToString();//审核人

            int shstate = Convert.ToInt32(gridView2.GetRowCellValue(rows, "shstate").ToString() == "" ? "0" : gridView2.GetRowCellValue(rows, "shstate").ToString());
            int hfstate = Convert.ToInt32(gridView2.GetRowCellValue(rows, "hfstate").ToString() == "" ? "0" : gridView2.GetRowCellValue(rows, "hfstate").ToString());

            if (hfstate == 0)
            {
                MsgBox.ShowOK("未进行责任划分，不能审核");
                return;
            }

            if (hfstate == 2)
            {
                MsgBox.ShowOK("责任划分已进行否决，不用再审核");
                return;
            }
            fmBadBillDeal wb = new fmBadBillDeal();
            wb.gv = gridView2;
            wb.look = 3;
            wb.ShowDialog();
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(gridView2);
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(gridView2);
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(gridView2, "货差货损记录");
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout("货差货损记录");
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(gridView2);
        }

        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rows = gridView2.FocusedRowHandle;
            if (rows < 0) return;

            int shstate = Convert.ToInt32(gridView2.GetRowCellValue(rows, "shstate").ToString() == "" ? "0" : gridView2.GetRowCellValue(rows, "shstate").ToString());

            //if (shstate > 0)
            //{
            //    MsgBox.ShowOK("已审批，不需要再处理！");
            //    return;
            //}
            fmBadBillDeal wb = new fmBadBillDeal();
            wb.gv = gridView2;
            wb.look = 5;
            wb.ShowDialog();
        }

        private void gridView2_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e == null || e.Column.FieldName != "rowid") return;
            e.Value = e.RowHandle + 1;
        }

        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            frmBillSearchControl.ShowBillSearch(GridOper.GetRowCellValueString(gridView2, "BillNo"));
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridView gv = gridView2;
            if (gv.FocusedRowHandle < 0)
                return;

            string billno = gv.GetRowCellValue(gv.FocusedRowHandle, "BillNo").ToString();

            #region 加载本单图片
            string file = "";
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", billno));
                list.Add(new SqlPara("BillType", 1));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_TBFILEINFO_BadBill", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds != null && ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string filename = ConvertType.ToString(ds.Tables[0].Rows[i]["FilePath"]);
                        string[] sArray = filename.Split('/');

                        string bdPath = System.Windows.Forms.Application.StartupPath + "/TempFile";
                        if (!Directory.Exists(bdPath)) Directory.CreateDirectory(bdPath);
                        string bdFileName = bdPath + filename.Substring(filename.LastIndexOf("/"));
                        WebClient wc = new WebClient();
                        if (!File.Exists(bdFileName))
                        {
                            wc.DownloadFile("http://ZQTMS.dekuncn.com:7020" + filename, bdFileName);
                        }

                        //上传图片到OA服务器
                        byte[] bt = wc.UploadFile(HttpHelper.OAUrlImage, "POST", bdFileName);
                        string json = Encoding.UTF8.GetString(bt);
                        OAFileUpResult result = JsonConvert.DeserializeObject<OAFileUpResult>(json);
                        if (result.Success == true)
                        {
                            file += string.Format("<File>{0}</File>", result.FileName);
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                return;
            }
            #endregion

            string ConsignorCellPhone = gv.GetRowCellValue(gv.FocusedRowHandle, "ConsignorCellPhone").ToString();
            string ConsignorPhone = gv.GetRowCellValue(gv.FocusedRowHandle, "ConsignorPhone").ToString();
            string string19 = ConsignorCellPhone + "/" + ConsignorPhone;
            string19 = string19.Trim('/');

            string ConsigneeCellPhone = gv.GetRowCellValue(gv.FocusedRowHandle, "ConsigneeCellPhone").ToString();
            string ConsigneePhone = gv.GetRowCellValue(gv.FocusedRowHandle, "ConsigneePhone").ToString();
            string string26 = ConsigneeCellPhone + "/" + ConsigneePhone;
            string26 = string26.Trim('/');

            string zerenwebid1 = gv.GetRowCellValue(gv.FocusedRowHandle, "zerenwebid1").ToString();
            string zerenwebid2 = gv.GetRowCellValue(gv.FocusedRowHandle, "zerenwebid2").ToString();
            string zerenwebid3 = gv.GetRowCellValue(gv.FocusedRowHandle, "zerenwebid3").ToString();
            string string32 = zerenwebid1 + "/" + zerenwebid2 + "/" + zerenwebid3;
            string32 = string32.Trim('/');


            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<date1>{0}</date1>", gv.GetRowCellValue(gv.FocusedRowHandle, "BillDate"));//ZQTMS开单日期
            sb.AppendFormat("<number2>{0}</number2>", gv.GetRowCellValue(gv.FocusedRowHandle, "accspe"));//索赔金额
            sb.AppendFormat("<string24>{0}</string24>", gv.GetRowCellValue(gv.FocusedRowHandle, "webid"));//开单部门
            sb.AppendFormat("<string10>{0}</string10>", gv.GetRowCellValue(gv.FocusedRowHandle, "Varieties"));//品名
            sb.AppendFormat("<string1>{0}</string1>", gv.GetRowCellValue(gv.FocusedRowHandle, "Package"));//包装
            sb.AppendFormat("<number7>{0}</number7>", gv.GetRowCellValue(gv.FocusedRowHandle, "Num"));//件数
            //sb.AppendFormat("<string28>{0}</string28>", gv.GetRowCellValue(gv.FocusedRowHandle, "qsstate"));//签收情况
            sb.AppendFormat("<string4>{0}</string4>", gv.GetRowCellValue(gv.FocusedRowHandle, "ConsignorCompany"));//发货单位
            sb.AppendFormat("<string5>{0}</string5>", gv.GetRowCellValue(gv.FocusedRowHandle, "ConsignorName"));//发货人
            sb.AppendFormat("<string19>{0}</string19>", string19);//联系方式（发货人）
            sb.AppendFormat("<string2>{0}</string2>", gv.GetRowCellValue(gv.FocusedRowHandle, "PaymentMode"));//结算方式
            sb.AppendFormat("<string6>{0}</string6>", gv.GetRowCellValue(gv.FocusedRowHandle, "ConsigneeCompany"));//收货单位
            sb.AppendFormat("<string11>{0}</string11>", gv.GetRowCellValue(gv.FocusedRowHandle, "ConsigneeName"));//收货人
            sb.AppendFormat("<string26>{0}</string26>", string26);//联系方式（收货人）
            sb.AppendFormat("<string20>{0}</string20>", gv.GetRowCellValue(gv.FocusedRowHandle, "badtype"));//异常类型
            sb.AppendFormat("<string22>{0}</string22>", ConvertType.ToString(gv.GetRowCellValue(gv.FocusedRowHandle, "badtype")) == "" ? "付款" : ConvertType.ToString(gv.GetRowCellValue(gv.FocusedRowHandle, "badtype")));//理赔方式
            sb.AppendFormat("<string32>{0}</string32>", string32);//责任部门
            sb.AppendFormat("<number1>{0}</number1>", gv.GetRowCellValue(gv.FocusedRowHandle, "accpfe"));//实际赔付金额
            sb.AppendFormat("<string29>{0}</string29>", billno);//运单号
            sb.AppendFormat("<string33>{0}</string33>", gv.GetRowCellValue(gv.FocusedRowHandle, "DestinationSite"));//到站

            sb.AppendFormat("<string21>{0}</string21>", gv.GetRowCellValue(gv.FocusedRowHandle, "SuopeiMan"));//索赔人
            sb.AppendFormat("<string27>{0}</string27>",  gv.GetRowCellValue(gv.FocusedRowHandle, "SuopeiPhone"));//索赔人的联系方式
            sb.AppendFormat("<string7>{0}</string7>", gv.GetRowCellValue(gv.FocusedRowHandle, "SpBankName"));//银行户名
            sb.AppendFormat("<string8>{0}</string8>", gv.GetRowCellValue(gv.FocusedRowHandle, "SpBankAdd"));//开户行
            sb.AppendFormat("<string9>{0}</string9>", gv.GetRowCellValue(gv.FocusedRowHandle, "SpBankAcount"));//账号
            sb.AppendFormat("<string30>{0}</string30>","三方客户");//客户类型
            sb.AppendFormat("<string28>{0}</string28>", "正常签收");//签收情况
            //MsgBox.ShowOK(sb.ToString());

            string msg = SubmitToOA.PostBadBill(billno, sb.ToString(), file);
            if (msg != "")//说明提交失败
            {
                MsgBox.ShowError("异常理赔信息提交到OA流程失败，可尝试重新提交!" + msg);
                return;
            }
            MsgBox.ShowOK();
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            //frmBillSearch frm = new frmBillSearch();

        }
    }
}