using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using System.Threading;
using ZQTMS.UI.BaseInfoManage;
using DevExpress.XtraGrid.Columns;

namespace ZQTMS.UI
{
    public partial class frmOrgUnit : BaseForm
    {
        public frmOrgUnit()
        {
            InitializeComponent();
        }
        GridColumn gcIsseleckedMode;
        bool flag = false;
        private void frmOrgUnit_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("运营线路");//xj/2019/5/29
            CommonClass.FormSet(this);

            flag = UserRight.GetRight("161125");

            CommonClass.GetGridViewColumns(myGridView1, myGridView2, myGridView3);
            GridOper.SetGridViewProperty(myGridView1, myGridView2, myGridView3);
            BarMagagerOper.SetBarPropertity(bar2, bar3, bar4, bar5); //如果有具体的工具条，就引用其实例
            barButtonItem39.Enabled = false;
            SiteAll();
            getWeb();
            getMiddleSite();
            barButtonItem23.Enabled = true;

            simpleButton1.Visible = CommonClass.UserInfo.companyid == "101";
            if (CommonClass.UserInfo.companyid != "101")
            {

                xtraTabPage1.PageVisible = false;
               // xtraTabPage3.PageVisible = false;
            }
            gcIsseleckedMode = GridOper.GetGridViewColumn(myGridView3, "ischecked");
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmBasSite_Add aa = new frmBasSite_Add();
            aa.ShowDialog();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (panel1.Visible)
            {
                MsgBox.ShowOK("正在加载数据...\r\n请稍后再试");
                return;
            }
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                string id = myGridView1.GetRowCellValue(rowhandle, "SiteId").ToString();

                frmBasSite_Add aa = new frmBasSite_Add();
                aa.SiteId = id;
                aa.ShowDialog();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmBasWeb_Add fou = new frmBasWeb_Add();
            fou.ShowDialog();
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmbasMiddleSite_Add fou = new frmbasMiddleSite_Add();
            fou.ShowDialog();
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SiteAll();
        }

        private void SiteAll()
        {
            bar2.Visible = false;
            panel1.Visible = true;
            Thread th = new Thread(() =>
            {
                try
                {
                    List<SqlPara> list = new List<SqlPara>();

                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASSITE", list);
                    DataSet ds = SqlHelper.GetDataSet(sps);

                    if (ds == null || ds.Tables.Count == 0) return;
                    if (!this.IsHandleCreated) return;
                    this.Invoke((MethodInvoker)delegate
                    {
                        myGridControl1.DataSource = ds.Tables[0];
                    });
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
                finally
                {
                    if (this.IsHandleCreated)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            bar2.Visible = true;
                            panel1.Visible = false;
                            if (myGridView1.RowCount < 1000) myGridView1.BestFitColumns();
                        });
                    }
                }
            });
            th.IsBackground = true;
            th.Start();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                Guid SiteId = new Guid(myGridView1.GetRowCellValue(rowhandle, "SiteId").ToString());
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SiteId", SiteId));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_BASSITE", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();

                    myGridView1.DeleteRow(rowhandle);
                    myGridView1.PostEditor();
                    myGridView1.UpdateCurrentRow();
                    myGridView1.UpdateSummary();
                    DataTable dt = myGridControl1.DataSource as DataTable;
                    dt.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem17_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView2.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid WebId = new Guid(myGridView2.GetRowCellValue(rowhandle, "WebId").ToString());

                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("WebId", WebId));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_BASWEB", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();

                    myGridView2.DeleteRow(rowhandle);
                    myGridView2.PostEditor();
                    myGridView2.UpdateCurrentRow();
                    myGridView2.UpdateSummary();
                    DataTable dt = myGridControl2.DataSource as DataTable;
                    dt.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem16_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (panel2.Visible)
            {
                MsgBox.ShowOK("正在加载数据...\r\n请稍后再试");
                return;
            }
            try
            {
                int rowhandle = myGridView2.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView2.GetRowCellValue(rowhandle, "WebId").ToString());

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("WebId", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASWEB_ByID", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                DataRow dr = ds.Tables[0].Rows[0];
                frmBasWeb_Add fou = new frmBasWeb_Add();
                fou.dr = dr;
                fou.ShowDialog();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                //int rowhandle = myGridView3.FocusedRowHandle;
                //if (rowhandle < 0) return;
                myGridView3.PostEditor();
                string MiddleSiteIdStr = "";
                //if (rowhandle >= 0)
                {
                    if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                    {
                        return;
                    }
                    for (int i = 0; i < myGridView3.RowCount; i++)
                    {
                        if (ConvertType.ToInt32(myGridView3.GetRowCellValue(i, "ischecked")) > 0)
                        {
                            if (MiddleSiteIdStr.Length > 5000)
                            {
                                Dictionary<string, string> dic1 = new Dictionary<string, string>();
                                dic1.Add("MiddleSiteId", MiddleSiteIdStr);
                                string strSQL1 = string.Empty;
                                GetCompanyId_By_Proc(CommonClass.UserInfo.companyid, "USP_DELETE_BASMIDDLESITE", dic1, ref strSQL1);
                                if (strSQL1 == "无效存储过程名称")
                                {
                                    MsgBox.ShowOK(strSQL1);
                                    return;
                                }
                                List<SqlPara> list1 = new List<SqlPara>();
                                list1.Add(new SqlPara("strSQL", strSQL1));
                                SqlParasEntity sps1 = new SqlParasEntity(OperType.Execute, "USP_BASMIDDLESITE_OptionSQL", list1);
                                if (SqlHelper.ExecteNonQuery(sps1) <= 0)
                                {
                                    MsgBox.ShowOK("操作失败");
                                    return;
                                }
                                MiddleSiteIdStr = "";
                                strSQL1 = "";
                                dic1.Clear();
                            }
                            MiddleSiteIdStr = MiddleSiteIdStr + myGridView3.GetRowCellValue(i, "MiddleSiteId").ToString() + "@";
                        }
                    }
                }

                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("MiddleSiteId", MiddleSiteIdStr);
                string strSQL = string.Empty;
                GetCompanyId_By_Proc(CommonClass.UserInfo.companyid, "USP_DELETE_BASMIDDLESITE", dic, ref strSQL);
                if (strSQL == "无效存储过程名称")
                {
                    MsgBox.ShowOK(strSQL);
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("strSQL", strSQL));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_BASMIDDLESITE_OptionSQL", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();

                    //myGridView3.DeleteRow(rowhandle);
                    //myGridView3.PostEditor();
                    //myGridView3.UpdateCurrentRow();
                    //myGridView3.UpdateSummary();
                    //DataTable dt = myGridControl3.DataSource as DataTable;
                    //dt.AcceptChanges();
                    getMiddleSite();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        /// <summary>
        /// 修改二级中转城市
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (panel3.Visible)
            {
                MsgBox.ShowOK("正在加载数据...\r\n请稍后再试");
                return;
            }
            int rowhandle = myGridView3.FocusedRowHandle;
            if (rowhandle < 0) return;

            frmBasMiddleSiteUpdate fmsu = new frmBasMiddleSiteUpdate();
            fmsu.Id = GridOper.GetRowCellValueString(myGridView3, rowhandle, "MiddleSiteId");
            fmsu.gv = myGridView3;
            fmsu.ShowDialog();

            /*
            try
            {
                int rowhandle = myGridView3.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView3.GetRowCellValue(rowhandle, "MiddleSiteId").ToString());

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("MiddleSiteId", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASMIDDLESITE_ByID", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                DataRow dr = ds.Tables[0].Rows[0];

                frmbasMiddleSite_Add fou = new frmbasMiddleSite_Add();
                fou.dr = dr;
                fou.ShowDialog();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
             */
        }

        private void barButtonItem19_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            getWeb();
        }

        private void getWeb()
        {
            bar5.Visible = false;
            panel2.Visible = true;
            Thread th = new Thread(() =>
            {
                try
                {
                    List<SqlPara> list = new List<SqlPara>();

                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASWEB", list);
                    DataSet ds = SqlHelper.GetDataSet(sps);

                    if (ds == null || ds.Tables.Count == 0) return;
                    if (!this.IsHandleCreated) return;
                    this.Invoke((MethodInvoker)delegate
                    {
                        myGridControl2.DataSource = ds.Tables[0];
                        myGridView2_FocusedRowChanged(null, null);
                    });
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
                finally
                {
                    if (this.IsHandleCreated)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            bar5.Visible = true;
                            panel2.Visible = false;
                            if (myGridView2.RowCount < 1000) myGridView2.BestFitColumns();
                        });
                    }
                }
            });
            th.IsBackground = true;
            th.Start();
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            getMiddleSite();
        }

        private void getMiddleSite()
        {
            bar3.Visible = false;
            panel3.Visible = true;
            Thread th = new Thread(() =>
            {
                try
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    List<SqlPara> list = new List<SqlPara>();
                    string strSQL = string.Empty;
                    //GetCompanyId_By_Proc(CommonClass.UserInfo.companyid, "提取", ref proc);
                    GetCompanyId_By_Proc(CommonClass.UserInfo.companyid, "QSP_GET_BASMIDDLESITE", dic, ref strSQL);
                    if (strSQL == "无效存储过程名称")
                    {
                        MsgBox.ShowOK(strSQL);
                        return;
                    }
                    list.Add(new SqlPara("strSQL", strSQL));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "USP_BASMIDDLESITE_OptionSQL", list);
                    DataSet ds = SqlHelper.GetDataSet(sps);

                    if (ds == null || ds.Tables.Count == 0) return;
                    if (!this.IsHandleCreated) return;
                    this.Invoke((MethodInvoker)delegate
                    {
                        myGridControl3.DataSource = ds.Tables[0];
                    });
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
                finally
                {
                    if (this.IsHandleCreated)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            bar3.Visible = true;
                            panel3.Visible = false;
                            if (myGridView3.RowCount < 1000) myGridView3.BestFitColumns();
                        });
                    }
                }
            });
            th.IsBackground = true;
            th.Start();
        }

        //根据公司ID取存储过程
        //public void GetCompanyId_By_Proc(string strCompngyId, string strProc, ref string strSQL) { }
        public void GetCompanyId_By_Proc(string strCompngyId, string strProc, Dictionary<string, string> dic, ref string strSQL)
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("strProc", strProc));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_baseProcedure", list);
            DataSet ds = SqlHelper.GetDataSet(sps);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                strSQL = ds.Tables[0].Rows[0]["ProcSql"].ToString();
                if (strProc == "QSP_GET_BASMIDDLESITE")
                {
                    if (strCompngyId == "101")
                    {
                        strSQL = strSQL.Replace("@tabName", "basMiddleSite_JMZX");
                    }
                    else if (strCompngyId == "124")
                    {
                        strSQL = strSQL.Replace("@tabName", "basMiddleSite_HD");
                    }
                    else
                    {
                        strSQL = strSQL.Replace("@tabName", "basMiddleSite_" + strCompngyId);
                    }
                }
                else if (strProc == "QSP_GET_BASMIDDLESITE_ByID" && dic != null)
                {
                    if (strCompngyId == "101")
                    {
                        strSQL = strSQL.Replace("@tabName", "basMiddleSite_JMZX");
                    }
                    else if (strCompngyId == "124")
                    {
                        strSQL = strSQL.Replace("@tabName", "basMiddleSite_HD");
                    }
                    else
                    {
                        strSQL = strSQL.Replace("@tabName", "basMiddleSite_" + strCompngyId);
                    }
                    strSQL = strSQL.Replace("@MiddleSiteId", "'" + dic["MiddleSiteId"] + "'");
                }
                else if (strProc == "USP_UPDATE_BASMIDDLE" && dic != null)
                {
                    if (strCompngyId == "101")
                    {
                        strSQL = strSQL.Replace("@tabName", "basMiddleSite_JMZX");
                    }
                    else if (strCompngyId == "124")
                    {
                        strSQL = strSQL.Replace("@tabName", "basMiddleSite_HD");
                    }
                    else
                    {
                        strSQL = strSQL.Replace("@tabName", "basMiddleSite_" + strCompngyId);
                    }
                    strSQL = strSQL.Replace("@SiteName", "'" + dic["SiteName"] + "'").Replace("@Destination", "'" + dic["Destination"] + "'").Replace("@MiddleProvince", "'" + dic["MiddleProvince"] + "'");
                    strSQL = strSQL.Replace("@MiddleCity", "'" + dic["MiddleCity"] + "'").Replace("@MiddleArea", "'" + dic["MiddleArea"] + "'").Replace("@MiddleStreet", "'" + dic["MiddleStreet"] + "'").Replace("@WebName", "'" + dic["WebName"] + "'");
                    strSQL = strSQL.Replace("@Type", "'" + dic["Type"] + "'").Replace("@MiddleLon", "'" + dic["MiddleLon"] + "'").Replace("@MiddleLat", "'" + dic["MiddleLat"] + "'").Replace("@FetchStorageLoca", "'" + dic["FetchStorageLoca"] + "'");
                    strSQL = strSQL.Replace("@SendStorageLoca", "'" + dic["SendStorageLoca"] + "'").Replace("@Ascription", "'" + dic["Ascription"] + "'").Replace("@ShengStore", "'" + dic["ShengStore"] + "'").Replace("@AreaStore", "'" + dic["AreaStore"] + "'");
                    strSQL = strSQL.Replace("@MiddleStatus", "'" + dic["MiddleStatus"] + "'").Replace("@companyid1", "'" + dic["companyid1"] + "'").Replace("@MiddleSiteId", "'" + dic["MiddleSiteId"] + "'");
                }
                else if (strProc == "USP_DELETE_BASMIDDLESITE" && dic != null)
                {
                    if (strCompngyId == "101")
                    {
                        strSQL = strSQL.Replace("@tabName", "basMiddleSite_JMZX");
                    }
                    else if (strCompngyId == "124")
                    {
                        strSQL = strSQL.Replace("@tabName", "basMiddleSite_HD");
                    }
                    else
                    {
                        strSQL = strSQL.Replace("@tabName", "basMiddleSite_" + strCompngyId);
                    }
                    strSQL = strSQL.Replace("@MiddleSiteId", "'" + dic["MiddleSiteId"] + "'").Replace("@LoginSiteName", "'" + CommonClass.UserInfo.SiteName + "'").Replace("@LoginWebName", "'" + CommonClass.UserInfo.WebName + "'");
                    strSQL = strSQL.Replace("@LoginUserName", "'" + CommonClass.UserInfo.UserName + "'");
                }
                else if (strProc == "USP_SET_BASMIDDLE_LAT_LNG" && dic != null)
                {
                    if (strCompngyId == "101")
                    {
                        strSQL = strSQL.Replace("@tabName", "basMiddleSite_JMZX");
                    }
                    else if (strCompngyId == "124")
                    {
                        strSQL = strSQL.Replace("@tabName", "basMiddleSite_HD");
                    }
                    else
                    {
                        strSQL = strSQL.Replace("@tabName", "basMiddleSite_" + strCompngyId);
                    }
                    strSQL = strSQL.Replace("@tabThree", "L_SplitStr_Three").Replace("@midStr", "'" + dic["midStr"] + "'").Replace("@latStr", "'" + dic["latStr"] + "'").Replace("@lngStr", "'" + dic["lngStr"] + "'");
                    strSQL = strSQL.Replace("@Symbol", "'@'").Replace("@companyid", "'" + strCompngyId + "'").Replace("@tabStr", "'basMiddleSite'");
                }
                else if (strProc == "USP_SAVE_MIDDLESITE_CW" && dic != null)
                {
                    if (strCompngyId == "101")
                    {
                        strSQL = strSQL.Replace("@tabName", "basMiddleSite_JMZX");
                    }
                    else if (strCompngyId == "124")
                    {
                        strSQL = strSQL.Replace("@tabName", "basMiddleSite_HD");
                    }
                    else
                    {
                        strSQL = strSQL.Replace("@tabName", "basMiddleSite_" + strCompngyId);
                    }
                    strSQL = strSQL.Replace("@tabThree", "L_SplitStr_Three").Replace("@MiddleSiteId", "'" + dic["MiddleSiteId"] + "'").Replace("@FetchStorageLoca", "'" + dic["FetchStorageLoca"] + "'").Replace("@SendStorageLoca", "'" + dic["SendStorageLoca"] + "'");
                    strSQL = strSQL.Replace("@Symbol", "'@'").Replace("@companyid", "'" + strCompngyId + "'");
                }
                else if (strProc == "QSP_GET_BASMIDDLESITE2" && dic != null)
                {
                    if (strCompngyId == "101")
                    {
                        strSQL = strSQL.Replace("@tabName", "basMiddleSite_JMZX");
                    }
                    else if (strCompngyId == "124")
                    {
                        strSQL = strSQL.Replace("@tabName", "basMiddleSite_HD");
                    }
                    else
                    {
                        strSQL = strSQL.Replace("@tabName", "basMiddleSite_" + strCompngyId);
                    }
                    strSQL = strSQL.Replace("@Province", "'" + (dic["Province"].ToString() == "" ? "%%" : dic["Province"]) + "'").Replace("@City", "'" + (dic["City"].ToString() == "" ? "%%" : dic["City"]) + "'").Replace("@Area", "'" + (dic["Area"].ToString() == "" ? "%%" : dic["Area"]) + "'").Replace("@Street", "'" + (dic["Street"].ToString() == "" ? "%%" : dic["Street"]) + "'");
                    strSQL = strSQL.Replace("@Type", "'" + (dic["Type"].ToString() == "" ? "%%" : dic["Type"]) + "'").Replace("@companyid", "'" + strCompngyId + "'");
                }
                else if (strProc == "USP_UPDATE_basMiddleSite" && dic != null)
                {
                    if (strCompngyId == "101")
                    {
                        strSQL = strSQL.Replace("@tabName", "basMiddleSite_JMZX");
                    }
                    else if (strCompngyId == "124")
                    {
                        strSQL = strSQL.Replace("@tabName", "basMiddleSite_HD");
                    }
                    else
                    {
                        strSQL = strSQL.Replace("@tabName", "basMiddleSite_" + strCompngyId);
                    }
                    strSQL = strSQL.Replace("@FetchWebName", "'" + dic["FetchWebName"] + "'").Replace("@SendWebName", "'" + dic["SendWebName"] + "'").Replace("@SiteName", "'" + dic["SiteName"] + "'").Replace("@MiddleSiteId", "'" + dic["MiddleSiteId"] + "'");
                    strSQL = strSQL.Replace("@Ascription", "'" + dic["Ascription"] + "'").Replace("@MiddleProvince", "'" + dic["MiddleProvince"] + "'").Replace("@MiddleCity", "'" + dic["MiddleCity"] + "'").Replace("@MiddleArea", "'" + dic["MiddleArea"] + "'");
                    strSQL = strSQL.Replace("@MiddleStreet", "'" + dic["MiddleStreet"] + "'").Replace("@isEditSite", "" + dic["isEditSite"] + "").Replace("@isEditWeb", "" + dic["isEditWeb"] + "").Replace("@isEditAscription", "" + dic["isEditAscription"] + "");
                    strSQL = strSQL.Replace("@isEditRemoveAscription", "" + dic["isEditRemoveAscription"] + "").Replace("@isEditProvince", "" + dic["isEditProvince"] + "").Replace("@isEditCity", "" + dic["isEditCity"] + "").Replace("@isEditArea", "" + dic["isEditArea"] + "");
                    strSQL = strSQL.Replace("@isEditStreet", "" + dic["isEditStreet"] + "");
                    strSQL = strSQL.Replace("@LoginAreaName", "'" + CommonClass.UserInfo.AreaName + "'").Replace("@LoginCauseName", "'" + CommonClass.UserInfo.CauseName + "'");
                    strSQL = strSQL.Replace("@LoginDepartName", "'" + CommonClass.UserInfo.DepartName + "'").Replace("@LoginSiteName", "'" + CommonClass.UserInfo.SiteName + "'").Replace("@LoginWebName", "'" + CommonClass.UserInfo.WebName + "'").Replace("@LoginUserAccount", "'" + CommonClass.UserInfo.UserAccount + "'");
                    strSQL = strSQL.Replace("@LoginUserName", "'" + CommonClass.UserInfo.UserName + "'").Replace("@companyid", "'" + strCompngyId + "'");
                }
                else if (strProc == "USP_UPDATE_BASMIDDLE_AREA")
                {
                    if (strCompngyId == "101")
                    {
                        strSQL = strSQL.Replace("@tabName", "basMiddleSite_JMZX");
                    }
                    else if (strCompngyId == "124")
                    {
                        strSQL = strSQL.Replace("@tabName", "basMiddleSite_HD");
                    }
                    else
                    {
                        strSQL = strSQL.Replace("@tabName", "basMiddleSite_" + strCompngyId);
                    }
                    strSQL = strSQL.Replace("@CoverageZone", "'" + dic["CoverageZone"] + "'").Replace("@OptCoverage", "'" + dic["OptCoverage"] + "'").Replace("@SbjWeb", "'" + dic["SbjWeb"] + "'").Replace("@MiddlePartner", "'" + dic["MiddlePartner"] + "'");
                    strSQL = strSQL.Replace("@MiddleSiteId", "'" + dic["MiddleSiteId"] + "'").Replace("@companyid", "'" + strCompngyId + "'").Replace("@tabStr", "'basMiddleSite'");
                }
                else if (strProc == "USP_BATCH_UPDATE_BASMIDDLE_BY_ADDRESS")
                {
                    if (strCompngyId == "101")
                    {
                        strSQL = strSQL.Replace("@tabName", "basMiddleSite_JMZX");
                    }
                    else if (strCompngyId == "124")
                    {
                        strSQL = strSQL.Replace("@tabName", "basMiddleSite_HD");
                    }
                    else
                    {
                        strSQL = strSQL.Replace("@tabName", "basMiddleSite_" + strCompngyId);
                    }
                    strSQL = strSQL.Replace("@SiteName", "'" + dic["SiteName"] + "'").Replace("@MiddleProvince", "'" + dic["MiddleProvince"] + "'").Replace("@MiddleCity", "'" + dic["MiddleCity"] + "'").Replace("@MiddleArea", "'" + dic["MiddleArea"] + "'");
                    strSQL = strSQL.Replace("@MiddleStreet", "'" + dic["MiddleStreet"] + "'").Replace("@CoverageZone", "'" + dic["CoverageZone"] + "'").Replace("@OptCoverage", "'" + dic["OptCoverage"] + "'").Replace("@SbjWeb", "'" + dic["SbjWeb"] + "'");
                    strSQL = strSQL.Replace("@MiddlePartner", "'" + dic["MiddlePartner"] + "'").Replace("@Type", "'" + dic["Type"] + "'").Replace("@LoginAreaName", "'" + CommonClass.UserInfo.AreaName + "'").Replace("@LoginCauseName", "'" + CommonClass.UserInfo.CauseName + "'");
                    strSQL = strSQL.Replace("@LoginDepartName", "'" + CommonClass.UserInfo.DepartName + "'").Replace("@LoginSiteName", "'" + CommonClass.UserInfo.SiteName + "'").Replace("@LoginWebName", "'" + CommonClass.UserInfo.WebName + "'").Replace("@LoginUserAccount", "'" + CommonClass.UserInfo.UserAccount + "'");
                    strSQL = strSQL.Replace("@LoginUserName", "'" + CommonClass.UserInfo.UserName + "'").Replace("@companyid", "'" + strCompngyId + "'");
                }
            }
            else
            {
                strSQL = "无效存储过程名称";
            }
            #region 注释代码
            //if (type == "提取")
            //{
            //    strProc = "QSP_GET_BASMIDDLESITE_" + strCompngyId;
            //}
            //else if (type == "新增")
            //{
            //    strProc = "USP_ADD_basMiddleSite_UPLOAD_" + strCompngyId;
            //}
            //else if (type == "修改")
            //{
            //    strProc = "USP_UPDATE_BASMIDDLE_" + strCompngyId;
            //}
            //else if (type == "删除")
            //{
            //    strProc = "USP_DELETE_BASMIDDLESITE_" + strCompngyId;
            //}
            //else if (type == "经纬度")
            //{
            //    strProc = "USP_SET_BASMIDDLE_LAT_LNG_" + strCompngyId;
            //}
            //else if (type == "库位")
            //{
            //    strProc = "USP_SAVE_MIDDLESITE_CW" + strCompngyId;
            //}
            //else if (type == "路由")
            //{
            //    strProc = "QSP_GET_BASMIDDLESITE2_" + strCompngyId;
            //}
            //else if (type == "修改路由")
            //{
            //    strProc = "USP_UPDATE_basMiddleSite_" + strCompngyId;
            //}
            //else if (type == "终端")
            //{
            //    strProc = "QSP_GET_BASMIDDLESITE_ByID_" + strCompngyId;
            //}
            //else if (type == "修改终端")
            //{
            //    strProc = "USP_UPDATE_BASMIDDLE_AREA_" + strCompngyId;
            //}
            //else if (type == "批量修改终端")
            //{
            //    strProc = "USP_BATCH_UPDATE_BASMIDDLE_BY_ADDRESS_" + strCompngyId;
            //}
            #endregion
        }

        private void myGridView2_DoubleClick(object sender, EventArgs e)
        {
            if (barButtonItem16.Enabled) barButtonItem16.PerformClick();
        }

        private void myGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (barButtonItem2.Enabled) barButtonItem2.PerformClick();
        }

        private void myGridView3_DoubleClick(object sender, EventArgs e)
        {
            if (barButtonItem10.Enabled) barButtonItem10.PerformClick();
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem21_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView3);
        }

        private void barButtonItem22_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmMiddleSiteUp up = new frmMiddleSiteUp();
            up.ShowDialog();
        }

        private void barButtonItem14_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView3, "二级中转市县");
        }

        private void barButtonItem20_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView2, "同城网点");
        }

        private void barButtonItem18_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView2);
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "直达站点");
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem23_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int[] rows = myGridView3.GetSelectedRows();
                if (rows.Length == 0)
                {
                    MsgBox.ShowOK("请选择需要保存的数据！");
                    return;
                }
                string MiddleSiteId = "";
                string FetchStorageLoca = "";
                string SendStorageLoca = "";
                for (int i = 0; i < rows.Length; i++)
                {
                    MiddleSiteId += myGridView3.GetRowCellValue(rows[i], "MiddleSiteId").ToString() + "@";
                    FetchStorageLoca += myGridView3.GetRowCellValue(rows[i], "FetchStorageLoca").ToString() + "@";
                    SendStorageLoca += myGridView3.GetRowCellValue(rows[i], "SendStorageLoca").ToString() + "@";
                }
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("MiddleSiteId", MiddleSiteId);
                dic.Add("FetchStorageLoca", FetchStorageLoca);
                dic.Add("SendStorageLoca", SendStorageLoca);
                string strSQL = string.Empty;
                GetCompanyId_By_Proc(CommonClass.UserInfo.companyid, "USP_SAVE_MIDDLESITE_CW", dic, ref strSQL);
                if (strSQL == "无效存储过程名称")
                {
                    MsgBox.ShowOK(strSQL);
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                //list.Add(new SqlPara("MiddleSiteId", MiddleSiteId));
                //list.Add(new SqlPara("FetchStorageLoca", FetchStorageLoca));
                //list.Add(new SqlPara("SendStorageLoca", SendStorageLoca));
                list.Add(new SqlPara("strSQL", strSQL));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_BASMIDDLESITE_OptionSQL", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void barButtonItem24_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rows = myGridView2.FocusedRowHandle;
            if (rows < 0) return;
            if (myGridView2.GetRowCellValue(rows, "WebStatus") == null)
            {
                return;
            }
            if (ConvertType.ToString(myGridView2.GetRowCellValue(rows, "WebStatus")) == "启用")
            {
                MsgBox.ShowError("已启用的不需再次启用！");
                return;
            }
            try
            {
                int rowhandle = myGridView2.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid WebId = new Guid(myGridView2.GetRowCellValue(rowhandle, "WebId").ToString());

                if (MsgBox.ShowYesNo("是否启用？请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("WebId", WebId));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_UPDATE_BASWEB_STATUS", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    getWeb();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem25_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rows = myGridView2.FocusedRowHandle;
            if (rows < 0) return;
            if (myGridView2.GetRowCellValue(rows, "WebStatus") == null)
            {
                return;
            }
            if (ConvertType.ToString(myGridView2.GetRowCellValue(rows, "WebStatus")) == "停用")
            {
                MsgBox.ShowError("已停用的不需再次启停用！");
                return;
            }
            try
            {
                int rowhandle = myGridView2.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid WebId = new Guid(myGridView2.GetRowCellValue(rowhandle, "WebId").ToString());

                if (MsgBox.ShowYesNo("是否停用？请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("WebId", WebId));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_UPDATE_BASWEB_STATUS", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    getWeb();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem26_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rows = myGridView2.FocusedRowHandle;
            if (rows < 0) return;
            if (ConvertType.ToString(myGridView2.GetRowCellValue(rows, "IsQk")) == "启用")
            {
                MsgBox.ShowError("已启用的不需再次启用！");
                return;
            }
            try
            {
                int rowhandle = myGridView2.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid WebId = new Guid(myGridView2.GetRowCellValue(rowhandle, "WebId").ToString());

                if (MsgBox.ShowYesNo("是否启用？请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("WebId", WebId));
                list.Add(new SqlPara("IsQk", "开启"));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_WEB_ISQK", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    myGridView2.SetRowCellValue(rowhandle, "IsQk", "开启");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem27_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rows = myGridView2.FocusedRowHandle;
            if (rows < 0) return;
            try
            {
                int rowhandle = myGridView2.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid WebId = new Guid(myGridView2.GetRowCellValue(rowhandle, "WebId").ToString());

                if (MsgBox.ShowYesNo("是否禁用？请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("WebId", WebId));
                list.Add(new SqlPara("IsQk", "禁用"));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_WEB_ISQK", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    myGridView2.SetRowCellValue(rowhandle, "IsQk", "禁用");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem28_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView3.RowCount == 0) return;
            if (myGridView3.RowCount > 2000)
            {
                MsgBox.ShowOK("每次获取不能超过2000条数据!");
                return;
            }
            string address = "";
            float lat = 0, lng = 0;
            for (int i = 0; i < myGridView3.RowCount; i++)
            {
                //拼接地址
                address = GridOper.GetRowCellValueString(myGridView3, i, "MiddleProvince") + GridOper.GetRowCellValueString(myGridView3, i, "MiddleCity") + GridOper.GetRowCellValueString(myGridView3, i, "MiddleArea") + GridOper.GetRowCellValueString(myGridView3, i, "MiddleStreet");

                frmGPS.GetGPS(address, ref lat, ref lng);
                if (lat == 0 || lng == 0) continue;

                myGridView3.SetRowCellValue(i, "MiddleLat", lat);
                myGridView3.SetRowCellValue(i, "MiddleLon", lng);
            }

            string latStr = "", lngStr = "", midStr = "";
            for (int i = 0; i < myGridView3.RowCount; i++)
            {
                lat = ConvertType.ToFloat(myGridView3.GetRowCellValue(i, "MiddleLat"));
                lng = ConvertType.ToFloat(myGridView3.GetRowCellValue(i, "MiddleLon"));

                if (lat == 0 || lng == 0) continue;

                latStr += lat + "@";
                lngStr += lng + "@";
                midStr += ConvertType.ToInt32(myGridView3.GetRowCellValue(i, "mid")) + "@";
            }
            if (midStr == "") return;

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("midStr", midStr);
            dic.Add("latStr", latStr);
            dic.Add("lngStr", lngStr);
            string strSQL = string.Empty;
            GetCompanyId_By_Proc(CommonClass.UserInfo.companyid, "USP_SET_BASMIDDLE_LAT_LNG", dic, ref strSQL);
            if (strSQL == "无效存储过程名称")
            {
                MsgBox.ShowOK(strSQL);
                return;
            }
            List<SqlPara> list = new List<SqlPara>();
            //list.Add(new SqlPara("midStr", midStr));
            //list.Add(new SqlPara("latStr", latStr));
            //list.Add(new SqlPara("lngStr", lngStr));
            list.Add(new SqlPara("strSQL", strSQL));

            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_BASMIDDLESITE_OptionSQL", list)) == 0) return;
            MsgBox.ShowOK("获取完成!");
        }

        private void barButtonItem35_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rows = myGridView2.FocusedRowHandle;
            if (rows < 0) return;
            if (myGridView2.GetRowCellValue(rows, "IsAcceptejSend") == null)
            {
                return;
            }
            if (ConvertType.ToString(myGridView2.GetRowCellValue(rows, "IsAcceptejSend")) == "启用")
            {
                MsgBox.ShowError("已启用的不需再次启用！");
                return;
            }
            try
            {
                int rowhandle = myGridView2.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid WebId = new Guid(myGridView2.GetRowCellValue(rowhandle, "WebId").ToString());

                if (MsgBox.ShowYesNo("是否启用？请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("WebId", WebId));
                list.Add(new SqlPara("IsAcceptejSend", 1));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_UPDATE_BASWEB_AcceptejSend", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    getWeb();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem38_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rows = myGridView2.FocusedRowHandle;
            if (rows < 0) return;
            if (myGridView2.GetRowCellValue(rows, "IsAcceptejSend") == null)
            {
                return;
            }
            if (ConvertType.ToString(myGridView2.GetRowCellValue(rows, "IsAcceptejSend")) == "停用")
            {
                MsgBox.ShowError("已停用的不需再次启停用！");
                return;
            }
            try
            {
                int rowhandle = myGridView2.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid WebId = new Guid(myGridView2.GetRowCellValue(rowhandle, "WebId").ToString());

                if (MsgBox.ShowYesNo("是否停用？请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("WebId", WebId));
                list.Add(new SqlPara("IsAcceptejSend", 0));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_UPDATE_BASWEB_AcceptejSend", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    getWeb();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        private int isFirst = 0;
        private void myGridView2_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (flag == false)
            {
                return;
            }
            int rowhandle = myGridView2.FocusedRowHandle;
            if (isFirst == 0)
            {
                rowhandle = 0;
                isFirst++;
            }
            if (rowhandle < 0) return;
            if (GridOper.GetRowCellValueString(myGridView2, rowhandle, "IsLoginEnable") == "1")
            {
                barButtonItem40.Enabled = true;
                barButtonItem39.Enabled = false;
            }
            else
            {
                barButtonItem40.Enabled = false;
                barButtonItem39.Enabled = true;
            }
        }

        private void barButtonItem39_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView2.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid WebId = new Guid(GridOper.GetRowCellValueString(myGridView2, rowhandle, "WebId"));
                if (MsgBox.ShowYesNo("是否禁止该用户登陆系统?") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("WebId", WebId));
                list.Add(new SqlPara("IsLoginEnable", "1"));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_SET_IsLoginEnableWeb", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    getWeb();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem40_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView2.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid WebId = new Guid(GridOper.GetRowCellValueString(myGridView2, rowhandle, "WebId"));

                if (MsgBox.ShowYesNo("是否恢复该用户登陆！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("WebId", WebId));
                list.Add(new SqlPara("IsLoginEnable", "0"));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_SET_IsLoginEnableWeb", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    getWeb();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem41_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView2.FocusedRowHandle;
                if (rowhandle < 0) return;
                string webid = myGridView2.GetRowCellValue(rowhandle, "WebId").ToString();
                string webcode = myGridView2.GetRowCellValue(rowhandle, "WebCode").ToString();
                string webname = myGridView2.GetRowCellValue(rowhandle, "WebName").ToString();
                frmRepaiDevicerNo frm = new frmRepaiDevicerNo();
                frm.webId = webid;
                frm.webCode = webcode;
                frm.webName = webname;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem42_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmbasMiddleSite_Update frm = new frmbasMiddleSite_Update();
            frm.Show();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            w_pt_company wp = new w_pt_company();
            wp.ShowDialog();
        }

        //二级中转市数据同步（从ZQTMS同步到lms） zaj 2017-11-20
        private void btnSynMiddleSite_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MsgBox.ShowYesNo("确定同步数据！") == DialogResult.No) return;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "SynMiddleSiteToLms", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK("数据同步成功！");
                }
            }
            catch (Exception ex)
            {
                
                MsgBox.ShowException(ex);
            }

        }
        //直达站点数据同步（从ZQTMS同步到lms） zaj 2017-11-20
        private void btnSynSite_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MsgBox.ShowYesNo("确定同步数据！") == DialogResult.No) return;
            try
            {
                //List<SqlPara> list = new List<SqlPara>();
                //SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "SynSiteToLms", list);
                //if (SqlHelper.ExecteNonQuery(sps)>0)
                //{
                    MsgBox.ShowOK("数据同步成功！");
                //}
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem44_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (panel3.Visible)
            {
                MsgBox.ShowOK("正在加载数据...\r\n请稍后再试");
                return;
            }
            frmAreaDivideBatchUpt upt = new frmAreaDivideBatchUpt();

            upt.ShowDialog();
        }

        private void barButtonItem43_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (panel3.Visible)
            {
                MsgBox.ShowOK("正在加载数据...\r\n请稍后再试");
                return;
            }
            int rowhandle = myGridView3.FocusedRowHandle;
            if (rowhandle < 0) return;

            frmAreaDivideUpt upt = new frmAreaDivideUpt();
            upt.Id = GridOper.GetRowCellValueString(myGridView3, rowhandle, "MiddleSiteId");
            upt.gv = myGridView3;
            upt.ShowDialog();
        }

        private void chkALL_CheckedChanged(object sender, EventArgs e)
        {
            int a = chkALL.Checked == true ? 1 : 0;
            for (int i = 0; i < myGridView3.RowCount; i++)
            {
                myGridView3.SetRowCellValue(i, gcIsseleckedMode, a);
            }
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < myGridView3.RowCount; i++)
            {
                if (ConvertType.ToInt32(myGridView3.GetRowCellValue(i, "ischecked")) == 1)
                    myGridView3.SetRowCellValue(i, gcIsseleckedMode, 0);
                else
                {
                    myGridView3.SetRowCellValue(i, gcIsseleckedMode, 1);
                }
            }
        }
    }
}