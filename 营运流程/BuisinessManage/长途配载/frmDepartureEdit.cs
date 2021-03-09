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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using System.Data.SqlClient;

namespace ZQTMS.UI
{
    public partial class frmDepartureEdit : DevExpress.XtraEditors.XtraForm
    {
        public frmDepartureEdit()
        {
            InitializeComponent();
        }

        public frmDepartureEdit(string sysType)
        {
            InitializeComponent();
            this.SystemType = sysType;
        }

        //配载类型，整车的目的地才能选择客户处
        //mengdi 2017-06-29
        public string loadType = "";
        private DataSet ds = new DataSet();//LMS数据源
        private DataTable dt_ZQTMS = new DataTable();//ZQTMS数据源
        string SystemType = string.Empty;//系统类型

        #region 自定义属性
        public string RequestSite { get; set; }
        public string RequestWeb { get; set; }
        public bool IsModify { get; set; }
        public string strEndSite1 { get; set; }
        public string strEndSite2 { get; set; }
        public string strEndSite3 { get; set; }
        public string strEndSite4 { get; set; }
        public string strSites { get; set; }
        public string state { get; set; }
        public string strTransitMode { get; set; }
        public string LoadingType { get; set; } //zb20190710
        #endregion

        #region 页面初始化
        private void frmDepartureEdit_Load(object sender, EventArgs e)
        {
            IsModify = false;
            //strSites = GetSites();
            //SetSite(EndSite1, dr);
            //SetSite(EndSite2, dr);
            //SetSite(EndSite3, dr);
            strTransitMode = "," + strTransitMode + ",";
            if (strTransitMode.Contains(",中强整车,"))
            {
                CommonClass.SetSite(EndSite1, false);
                CommonClass.SetSite(EndSite2, false);
                CommonClass.SetSite(EndSite3, false);
                CommonClass.SetSite(EndSite4, false);//maohui20180104
            }
            else
            {
                try
                {
                    string arrStr="";
                    List<SqlPara> list = new List<SqlPara>();
                    SqlParasEntity sps = null;
                    //if (SystemType == "LMS")
                    //{
                        GetWebNameForMain();//zaj 2018-4-18

                        if (loadType.Contains("整车配载"))  //zb20190710 lms-4115 
                        {
                            sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASSITE_BySiteName_485", list); 
                        }
                        else
                        {
                            sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASSITE_BySiteName", list); 
                        }
                        DataSet ds = SqlHelper.GetDataSet(sps);
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                arrStr += ds.Tables[0].Rows[i]["SiteName"].ToString() + ",";
                            }
                            string[] arr1 = arrStr.Split(',');
                            foreach (string str in arr1)
                            {
                                if (!string.IsNullOrEmpty(str.Trim()))
                                {
                                    EndSite1.Properties.Items.Add(str.Trim());
                                    EndSite2.Properties.Items.Add(str.Trim());
                                    EndSite3.Properties.Items.Add(str.Trim());
                                    EndSite4.Properties.Items.Add(str.Trim());
                                }
                            }
                        }
                    //}
                    //else//ZQTMS
                    //{
                    //    GetWebNameForMainZQTMS();

                    //    sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASSITE_BySiteName_LMS", list);
                    //    ds = SqlHelper.GetDataSet_ZQTMS(sps);
                    //    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    //    {
                    //        //string[] arr = ds.Tables[0].Rows[0]["EndSiteRang"].ToString().Split(',');
                    //        foreach (DataRow row in ds.Tables[0].Rows)
                    //        {
                    //            //if (!string.IsNullOrEmpty(str.Trim()))
                    //            //{
                    //                EndSite1.Properties.Items.Add(row["SiteName"].ToString());
                    //                EndSite2.Properties.Items.Add(row["SiteName"].ToString());
                    //                EndSite3.Properties.Items.Add(row["SiteName"].ToString());
                    //                EndSite4.Properties.Items.Add(row["SiteName"].ToString());
                    //            //}
                    //        }
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
            }

            this.EndSite1.Properties.Items.Add(""); this.EndSite1.Text = "";
            this.EndSite2.Properties.Items.Add(""); this.EndSite2.Text = "";
            this.EndSite3.Properties.Items.Add(""); this.EndSite3.Text = "";
            this.EndSite4.Properties.Items.Add(""); this.EndSite4.Text = "";

            //if (SystemType == "ZQTMS")
            //{
            //    if (EndSite1.Properties.Items.Contains("嘉定")) EndSite1.Properties.Items.Remove("嘉定");
            //    if (EndSite2.Properties.Items.Contains("嘉定")) EndSite2.Properties.Items.Remove("嘉定");
            //    if (EndSite3.Properties.Items.Contains("嘉定")) EndSite3.Properties.Items.Remove("嘉定");
            //    if (EndSite4.Properties.Items.Contains("嘉定")) EndSite4.Properties.Items.Remove("嘉定");

            //    if (EndSite1.Properties.Items.Contains("永康")) EndSite1.Properties.Items.Remove("永康");
            //    if (EndSite2.Properties.Items.Contains("永康")) EndSite2.Properties.Items.Remove("永康");
            //    if (EndSite3.Properties.Items.Contains("永康")) EndSite3.Properties.Items.Remove("永康");
            //    if (EndSite4.Properties.Items.Contains("永康")) EndSite4.Properties.Items.Remove("永康");

            //    if (!string.IsNullOrEmpty(loadType) && !loadType.Equals("整车配载"))
            //    {
            //        if (EndSite1.Properties.Items.Contains("客户处"))
            //        {
            //            EndSite1.Properties.Items.Remove("客户处");
            //        }
            //        if (EndSite2.Properties.Items.Contains("客户处"))
            //        {
            //            EndSite2.Properties.Items.Remove("客户处");
            //        }
            //        if (EndSite3.Properties.Items.Contains("客户处"))
            //        {
            //            EndSite3.Properties.Items.Remove("客户处");
            //        }
            //        if (EndSite4.Properties.Items.Contains("客户处"))
            //        {
            //            EndSite4.Properties.Items.Remove("客户处");
            //        }
            //    }
            //}

            //CommonClass.SetWeb(EndSite1, CommonClass.UserInfo.SiteName, false);
            //CommonClass.SetWeb(EndSite2, CommonClass.UserInfo.SiteName, false);
            //CommonClass.SetWeb(EndSite3, CommonClass.UserInfo.SiteName, false);

            #region 循环请求过来的,目的地信息进行分类
            var strSite = RequestSite.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (strSite.Length > 0)
            {
                if (strSite[0] != null)
                {
                    if (EndSite1.Properties.Items.Contains(strSite[0]))
                    {
                        //EndSite1.Properties.Items.Add(strSite[0]);

                        //EndSite2.Properties.Items.Remove(strSite[0]);
                        //EndSite3.Properties.Items.Remove(strSite[0]);
                        this.EndSite1.Text = strSite[0];
                        strEndSite1 = strSite[0];
                    }
                }
                if (strSite.Length > 1 && strSite[1] != null)
                {
                    if (EndSite2.Properties.Items.Contains(strSite[1]))
                    {
                        //EndSite2.Properties.Items.Add(strSite[1]);

                        //EndSite1.Properties.Items.Remove(strSite[1]);
                        //EndSite3.Properties.Items.Remove(strSite[1]);
                        this.EndSite2.Text = strSite[1];
                        strEndSite2 = strSite[1];
                    }
                }
                if (strSite.Length > 2 && strSite[2] != null)
                {
                    if (EndSite3.Properties.Items.Contains(strSite[2]))
                    {
                        //EndSite3.Properties.Items.Add(strSite[2]);

                        //EndSite1.Properties.Items.Remove(strSite[2]);
                        //EndSite2.Properties.Items.Remove(strSite[2]);
                        this.EndSite3.Text = strSite[2];
                        strEndSite3 = strSite[2];
                    }
                }
                if (strSite.Length > 3 && strSite[3] != null)
                {
                    if (EndSite4.Properties.Items.Contains(strSite[3]))
                    {
                        this.EndSite4.Text = strSite[3];
                        strEndSite4 = strSite[3];
                    }
                }
            }
            var strWeb = RequestWeb.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (strWeb.Length > 0)
            {
                if (strWeb[0] != null)
                {
                    this.EndWeb1.Text = strWeb[0];
                }
                if (strWeb.Length > 1 && strWeb[1] != null)
                {
                    this.EndWeb2.Text = strWeb[1];
                }
                if (strWeb.Length > 2 && strWeb[2] != null)
                {
                    this.EndWeb3.Text = strWeb[2];
                }
                if (strWeb.Length > 3 && strWeb[3] != null)
                {
                    this.EndWeb4.Text = strWeb[3];
                }
            }
            #endregion

            //#region zxw 20170928 已审核的不允许修改目的地，但是可以修改目的网点。 吴沐鸿
            //if (state == "已审核" && SystemType == "ZQTMS")
            //{
            //    EndSite1.Enabled = false;
            //    EndSite2.Enabled = false;
            //    EndSite3.Enabled = false;
            //    EndSite4.Enabled = false;
            //}
            //#endregion
            //if (SystemType == "ZQTMS") this.label3.Text = "*请严格按到车顺序调整，当前加载的为ZQTMS目的站点和网点！！";
        }
        #endregion

        #region 按钮事件
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCanccel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string strResponseSite = "";
            string strResponseWeb = "";
            if (!loadType.Contains( "整车配载")) //zb20190710 lms-4115
            {

                if (!string.IsNullOrEmpty(this.EndSite1.Text.Trim()) && string.IsNullOrEmpty(this.EndWeb1.Text.Trim()))
                {
                    XtraMessageBox.Show("选择目的站点1后必须选择目的网点1！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
            }
            strResponseSite = this.EndSite1.Text.Trim();
            strResponseWeb = this.EndWeb1.Text.Trim();
            if (!string.IsNullOrEmpty(this.EndSite2.Text.Trim()) && string.IsNullOrEmpty(this.EndWeb2.Text.Trim()))
            {
                XtraMessageBox.Show("选择目的站点2后必须选择目的网点2！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                //配载发车 目的地允许重复选择，目的网点不允许重复的调整。
                //陈晓业 提出
                //mengdi 2017-06-05
                if (!string.IsNullOrEmpty(this.EndWeb2.Text.Trim()) && this.EndWeb1.Text.Trim().Equals(this.EndWeb2.Text.Trim()))
                {
                    XtraMessageBox.Show("目的网点1与目的网点2重复！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    if (!string.IsNullOrEmpty(this.EndSite2.Text.Trim()) && !string.IsNullOrEmpty(this.EndWeb2.Text.Trim()))
                    {
                        strResponseSite += "," + this.EndSite2.Text.Trim();
                        strResponseWeb += "," + this.EndWeb2.Text.Trim();
                    }
                }
            }

            if (!string.IsNullOrEmpty(this.EndSite3.Text.Trim()) && string.IsNullOrEmpty(this.EndWeb3.Text.Trim()))
            {
                XtraMessageBox.Show("选择目的站点3后必须选择目的网点3！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                if (!string.IsNullOrEmpty(this.EndWeb1.Text.Trim()) && this.EndWeb3.Text.Trim().Equals(this.EndWeb1.Text.Trim()))
                {
                    XtraMessageBox.Show("目的网点1与目的网点3重复！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (!string.IsNullOrEmpty(this.EndWeb2.Text.Trim()) && this.EndWeb3.Text.Trim().Equals(this.EndWeb2.Text.Trim()))
                {
                    XtraMessageBox.Show("目的网点2与目的网点3重复！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    if (!string.IsNullOrEmpty(this.EndSite3.Text.Trim()) && !string.IsNullOrEmpty(this.EndWeb3.Text.Trim()))
                    {
                        strResponseSite += "," + this.EndSite3.Text.Trim();
                        strResponseWeb += "," + this.EndWeb3.Text.Trim();
                    }
                }
            }
            if (!string.IsNullOrEmpty(this.EndSite4.Text.Trim()) && string.IsNullOrEmpty(this.EndWeb4.Text.Trim()))
            {
                XtraMessageBox.Show("选择目的站点4后必须选择目的网点4！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                if (!string.IsNullOrEmpty(this.EndWeb1.Text.Trim()) && this.EndWeb4.Text.Trim().Equals(this.EndWeb1.Text.Trim()))
                {
                    XtraMessageBox.Show("目的网点1与目的网点4重复！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (!string.IsNullOrEmpty(this.EndWeb2.Text.Trim()) && this.EndWeb4.Text.Trim().Equals(this.EndWeb2.Text.Trim()))
                {
                    XtraMessageBox.Show("目的网点2与目的网点4重复！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (!string.IsNullOrEmpty(this.EndWeb3.Text.Trim()) && this.EndWeb4.Text.Trim().Equals(this.EndWeb3.Text.Trim()))
                {
                    XtraMessageBox.Show("目的网点3与目的网点4重复！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    if (!string.IsNullOrEmpty(this.EndSite4.Text.Trim()) && !string.IsNullOrEmpty(this.EndWeb4.Text.Trim()))
                    {
                        strResponseSite += "," + this.EndSite4.Text.Trim();
                        strResponseWeb += "," + this.EndWeb4.Text.Trim();
                    }
                }
            }

            //if (!string.IsNullOrEmpty(this.EndWeb1.Text.Trim())) strResponseWeb = this.EndWeb1.Text.Trim();
            //if (!string.IsNullOrEmpty(this.EndWeb2.Text.Trim())) strResponseWeb += "," + this.EndWeb2.Text.Trim();
            //if (!string.IsNullOrEmpty(this.EndWeb3.Text.Trim())) strResponseWeb += "," + this.EndWeb3.Text.Trim();
            if (this.Owner.Name.Contains("frmDepartureAdd"))
            {
                var frm = (frmDepartureAdd)this.Owner;
                frm.ResponseSite = strResponseSite;
                frm.ResponseWeb = strResponseWeb;
            }
            else if (this.Owner.Name.Contains("frmDepartureMody"))
            {
                var frm = (frmDepartureMody)this.Owner;
                frm.ResponseSite = strResponseSite;
                frm.ResponseWeb = strResponseWeb;
            }
            this.IsModify = true;
            this.Close();
        }
        #endregion

        #region 获取制定目的网点
        //private string GetSites()
        //{
        //    //获取所有指定的目的网点
        //    List<SqlPara> list = new List<SqlPara>();
        //    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "USP_sysRoleDataInfo_All");
        //    DataSet ds = SqlHelper.GetDataSet(sps);
        //    if (ds == null || ds.Tables[0].Rows.Count == 0)
        //    {
        //        XtraMessageBox.Show("没有任何指定目的网点", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return null;
        //    }
        //    DataRow[] dr = ds.Tables[0].Select(string.Format("GRCode='{0}'", 205));
        //    if (dr == null || dr.Length == 0)
        //    {
        //        XtraMessageBox.Show("没有找到任何指定目的网点", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return null;
        //    }
        //    return dr[0]["SiteNames"].ToString();
        //}
        //private DataRow[] GetSite()
        //{
        //    //获取所有指定的目的网点
        //    List<SqlPara> list = new List<SqlPara>();
        //    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "USP_sysRoleDataInfo_All");
        //    DataSet ds = SqlHelper.GetDataSet(sps);
        //    if (ds == null || ds.Tables[0].Rows.Count == 0)
        //    {
        //        XtraMessageBox.Show("没有任何指定目的网点", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return null;
        //    }
        //    DataRow[] dr = ds.Tables[0].Select(string.Format("GRCode='{0}'", 205));
        //    if (dr == null || dr.Length == 0)
        //    {
        //        XtraMessageBox.Show("没有找到任何指定目的网点", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return null;
        //    }
        //    return dr;
        //}
        //private void SetSite(ComboBoxEdit cb, DataRow[] dr)
        //{
        //    foreach (var d in dr)
        //    {
        //        string[] strArray = d["SiteNames"].ToString().Split(new string[] { "," }, StringSplitOptions.None);
        //        foreach (var item in strArray)
        //        {
        //            if (!cb.Properties.Items.Contains(item))
        //            {
        //                cb.Properties.Items.Add(item);
        //            }
        //        }

        //    }
        //}
        #endregion

        private void EndSite1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(EndSite1.Text.Trim()))
            {
                EndWeb1.Properties.Items.Clear();
                EndWeb1.Text = "";
                return;
            }

            string site = EndSite1.Text.Trim();
            //if (EndSite2.Properties.Items.Contains(site)) EndSite2.Properties.Items.Remove(site);
            //if (EndSite3.Properties.Items.Contains(site)) EndSite3.Properties.Items.Remove(site);
            if (!string.IsNullOrEmpty(strEndSite1) && site != strEndSite1)
            {
                EndSite2.Properties.Items.Add(strEndSite1);
                EndSite3.Properties.Items.Add(strEndSite1);
                EndSite4.Properties.Items.Add(strEndSite1);
            }
            strEndSite1 = site;
            EndWeb1.Properties.Items.Clear();
            EndWeb1.Text = "";
            //if (SystemType == "LMS")
            //{
                CommonClass.SetWeb(EndWeb1, site, false);
                //zaj 2018-4-18 可配载到总公司的网点去
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow[] drs = ds.Tables[0].Select("SiteName='" + EndSite1.Text.Trim() + "'");
                    for (int i = 0; i < drs.Length; i++)
                    {
                        if (!EndWeb1.Properties.Items.Contains(drs[i]["WebName"].ToString()))
                        {
                            EndWeb1.Properties.Items.Add(drs[i]["WebName"].ToString());
                        }
                    }
                }
            //}
            //else
            //{
            //    if (dt_ZQTMS != null && dt_ZQTMS.Rows.Count > 0)
            //    {
            //        DataRow[] drs = dt_ZQTMS.Select("SiteName='" + EndSite1.Text.Trim() + "'");
            //        for (int i = 0; i < drs.Length; i++)
            //        {
            //            if (!EndWeb1.Properties.Items.Contains(drs[i]["WebName"].ToString()))
            //            {
            //                EndWeb1.Properties.Items.Add(drs[i]["WebName"].ToString());
            //            }
            //        }
            //    }
                
            //    ComboBoxEdit cmb1 = new ComboBoxEdit();
            //    foreach (var item in EndWeb1.Properties.Items)
            //    {
            //        if (!string.IsNullOrEmpty(strSites))
            //        {
            //            if (strSites.Contains(item.ToString()))
            //            {
            //                cmb1.Properties.Items.Add(item);
            //            }
            //        }
            //    }
            //    EndWeb1.Properties.Items.Clear();
            //    EndWeb1.Text = "";
            //    if (cmb1.Properties.Items.Count == 0)
            //    {
            //        EndWeb1.Properties.Items.Add(strEndSite1);
            //        EndWeb1.Text = strEndSite1;
            //    }
            //    else
            //    {
            //        foreach (var item in cmb1.Properties.Items)
            //        {
            //            EndWeb1.Properties.Items.Add(item);
            //        }
            //    }
            //}
        }

        private void EndSite2_SelectedIndexChanged(object sender, EventArgs e)
        { 
            if (string.IsNullOrEmpty(EndSite2.Text.Trim()))
            {
                EndWeb2.Properties.Items.Clear();
                EndWeb2.Text = "";
                return;
            }

            string site = EndSite2.Text.Trim();
            //if (EndSite1.Properties.Items.Contains(site)) EndSite1.Properties.Items.Remove(site);
            //if (EndSite3.Properties.Items.Contains(site)) EndSite3.Properties.Items.Remove(site);
            if (!string.IsNullOrEmpty(strEndSite2) && site != strEndSite2)
            {
                EndSite1.Properties.Items.Add(strEndSite2);
                EndSite3.Properties.Items.Add(strEndSite2);
                EndSite4.Properties.Items.Add(strEndSite2);
            }
            strEndSite2 = site;
            EndWeb2.Properties.Items.Clear();
            EndWeb2.Text = "";
            //if (SystemType == "LMS")
            //{
                CommonClass.SetWeb(EndWeb2, site, false);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow[] drs = ds.Tables[0].Select("SiteName='" + EndSite2.Text.Trim() + "'");
                    for (int i = 0; i < drs.Length; i++)
                    {
                        if (!EndWeb2.Properties.Items.Contains(drs[i]["WebName"].ToString()))
                        {
                            EndWeb2.Properties.Items.Add(drs[i]["WebName"].ToString());
                        }
                    }
                }
            //}
            //else
            //{
            //    if (dt_ZQTMS != null && dt_ZQTMS.Rows.Count > 0)
            //    {
            //        DataRow[] drs = dt_ZQTMS.Select("SiteName='" + EndSite2.Text.Trim() + "'");
            //        for (int i = 0; i < drs.Length; i++)
            //        {
            //            if (!EndWeb2.Properties.Items.Contains(drs[i]["WebName"].ToString()))
            //            {
            //                EndWeb2.Properties.Items.Add(drs[i]["WebName"].ToString());
            //            }
            //        }
            //    }

                //ComboBoxEdit cmb2 = new ComboBoxEdit();
                //foreach (var item in EndWeb2.Properties.Items)
                //{
                //    if (!string.IsNullOrEmpty(strSites))
                //    {
                //        if (strSites.Contains(item.ToString()))
                //        {
                //            cmb2.Properties.Items.Add(item);
                //        }
                //    }
                //}
                //EndWeb2.Properties.Items.Clear();
                //EndWeb2.Text = "";
                //if (cmb2.Properties.Items.Count == 0)
                //{
                //    EndWeb2.Properties.Items.Add(strEndSite2);
                //    EndWeb2.Text = strEndSite2;
                //}
                //else
                //{
                //    foreach (var item in cmb2.Properties.Items)
                //    {
                //        EndWeb2.Properties.Items.Add(item);
                //    }
                //} 
            //}
        }

        private void EndSite3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(EndSite3.Text.Trim()))
            {
                EndWeb3.Properties.Items.Clear();
                EndWeb3.Text = "";
                return;
            }

            string site = EndSite3.Text.Trim();
            //if (EndSite1.Properties.Items.Contains(site)) EndSite1.Properties.Items.Remove(site);
            //if (EndSite2.Properties.Items.Contains(site)) EndSite2.Properties.Items.Remove(site);
            if (!string.IsNullOrEmpty(strEndSite3) && site != strEndSite3)
            {
                EndSite1.Properties.Items.Add(strEndSite3);
                EndSite2.Properties.Items.Add(strEndSite3);
                EndSite4.Properties.Items.Add(strEndSite3);
            }
            strEndSite3 = site;
            EndWeb3.Properties.Items.Clear();
            EndWeb3.Text = "";
            //if (SystemType == "LMS")
            //{
                CommonClass.SetWeb(EndWeb3, site, false);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow[] drs = ds.Tables[0].Select("SiteName='" + EndSite3.Text.Trim() + "'");
                    for (int i = 0; i < drs.Length; i++)
                    {
                        if (!EndWeb3.Properties.Items.Contains(drs[i]["WebName"].ToString()))
                        {
                            EndWeb3.Properties.Items.Add(drs[i]["WebName"].ToString());
                        }
                    }
                }
            //}
            //else
            //{
            //    if (dt_ZQTMS != null && dt_ZQTMS.Rows.Count > 0)
            //    {
            //        DataRow[] drs = dt_ZQTMS.Select("SiteName='" + EndSite3.Text.Trim() + "'");
            //        for (int i = 0; i < drs.Length; i++)
            //        {
            //            if (!EndWeb3.Properties.Items.Contains(drs[i]["WebName"].ToString()))
            //            {
            //                EndWeb3.Properties.Items.Add(drs[i]["WebName"].ToString());
            //            }
            //        }
                //}

                //ComboBoxEdit cmb3 = new ComboBoxEdit();
                //foreach (var item in EndWeb3.Properties.Items)
                //{
                //    if (!string.IsNullOrEmpty(strSites))
                //    {
                //        if (strSites.Contains(item.ToString()))
                //        {
                //            cmb3.Properties.Items.Add(item);
                //        }
                //    }
                //}
                //EndWeb3.Properties.Items.Clear();
                //EndWeb3.Text = "";
                //if (cmb3.Properties.Items.Count == 0)
                //{
                //    EndWeb3.Properties.Items.Add(strEndSite3);
                //    EndWeb3.Text = strEndSite3;
                //}
                //else
                //{
                //    foreach (var item in cmb3.Properties.Items)
                //    {
                //        EndWeb3.Properties.Items.Add(item);
                //    }
                //} 
            //}
        }

        //获取总公司网点
        private void GetWebNameForMain()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ShortWeb", list);
                ds = SqlHelper.GetDataSet(sps);

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //获取ZQTMS网点
        private void GetWebNameForMainZQTMS()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASWEB_STATUS", list);
                DataSet ds = SqlHelper.GetDataSet_ZQTMS(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                dt_ZQTMS = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void EndSite4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(EndSite4.Text.Trim()))
            {
                EndWeb4.Properties.Items.Clear();
                EndWeb4.Text = "";
                return;
            }

            string site = EndSite4.Text.Trim();
            if (!string.IsNullOrEmpty(strEndSite4) && site != strEndSite4)
            {
                EndSite1.Properties.Items.Add(strEndSite4);
                EndSite2.Properties.Items.Add(strEndSite4);
                EndSite4.Properties.Items.Add(strEndSite4);
            }
            strEndSite4 = site;
            EndWeb4.Properties.Items.Clear();
            EndWeb4.Text = "";
            //if (SystemType == "LMS")
            //{
                CommonClass.SetWeb(EndWeb4, site, false);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow[] drs = ds.Tables[0].Select("SiteName='" + EndSite4.Text.Trim() + "'");
                    for (int i = 0; i < drs.Length; i++)
                    {
                        if (!EndWeb4.Properties.Items.Contains(drs[i]["WebName"].ToString()))
                        {
                            EndWeb4.Properties.Items.Add(drs[i]["WebName"].ToString());
                        }
                    }
                }
            //}
            //else
            //{
            //    if (dt_ZQTMS != null && dt_ZQTMS.Rows.Count > 0)
            //    {
            //        DataRow[] drs = dt_ZQTMS.Select("SiteName='" + EndSite4.Text.Trim() + "'");
            //        for (int i = 0; i < drs.Length; i++)
            //        {
            //            if (!EndWeb4.Properties.Items.Contains(drs[i]["WebName"].ToString()))
            //            {
            //                EndWeb4.Properties.Items.Add(drs[i]["WebName"].ToString());
            //            }
            //        }
            //    }
            //}
        }
    }
}