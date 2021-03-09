using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.Common;
using System.Threading;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmbasMiddleSite_Update : BaseForm
    {
        public frmbasMiddleSite_Update()
        {
            InitializeComponent();
        }

        //加载
        private void frmbasMiddleSite_Update_Load(object sender, EventArgs e)
        {

            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView2, false);
            GridOper.SetGridViewProperty(myGridView2);

            CommonClass.AreaManager.FillCityToImageComBoxEdit(MiddleProvince, "0");
            CommonClass.AreaManager.FillCityToImageComBoxEdit(cb_MiddleProvince, "0");
            //全部网点
            DataTable dt = GetWebAll();
            if (dt != null)
            {
                CommonClass.SetWeb(FetchWebName, dt, false);
                FetchWebName.Properties.Items.Add("");
                CommonClass.SetWeb(SendWebName, dt, false);
                SendWebName.Properties.Items.Add("");
            }
            CommonClass.SetSite(SiteName, false);
            SiteName.Properties.Items.Add("");
            CommonClass.SetSite(cb_Ascription);
        }

        private void MiddleProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                MiddleCity.Properties.Items.Clear();
                MiddleCity.Text = "";
                if (MiddleProvince.Text.Trim() == "")
                {
                    return;
                }
                CommonClass.AreaManager.FillCityToImageComBoxEdit(MiddleCity, MiddleProvince.Properties.Items[MiddleProvince.SelectedIndex].Value);
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message, "系统提示");
            }
        }

        private void MiddleCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                MiddleArea.Properties.Items.Clear();
                MiddleArea.Text = "";
                if (MiddleCity.Text.Trim() == "")
                {
                    return;
                }
                CommonClass.AreaManager.FillCityToImageComBoxEdit(MiddleArea, MiddleCity.Properties.Items[MiddleCity.SelectedIndex].Value);
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message, "系统提示");
            }
        }

        private void MiddleArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                MiddleStreet.Properties.Items.Clear();
                MiddleStreet.Text = "";
                if (MiddleArea.Text.Trim() == "")
                {
                    return;
                }
                CommonClass.AreaManager.FillCityToImageComBoxEdit(MiddleStreet, MiddleArea.Properties.Items[MiddleArea.SelectedIndex].Value);
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message, "系统提示");
            }
        }

        private void btnConcel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //提取
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("Province", ConvertType.ToString(ConvertType.ToString(MiddleProvince.Text) == "" ? "%%" : ConvertType.ToString(MiddleProvince.Text)));
                dic.Add("City", ConvertType.ToString(ConvertType.ToString(MiddleCity.Text) == "" ? "%%" : ConvertType.ToString(MiddleCity.Text)));
                dic.Add("Area", ConvertType.ToString(ConvertType.ToString(MiddleArea.Text) == "" ? "%%" : ConvertType.ToString(MiddleArea.Text)));
                dic.Add("Street", ConvertType.ToString(ConvertType.ToString(MiddleStreet.Text) == "" ? "%%" : ConvertType.ToString(MiddleStreet.Text)));
                dic.Add("Type", ConvertType.ToString(ConvertType.ToString(Type.Text) == "全部" ? "%%" : ConvertType.ToString(Type.Text)));
                string strSQL = string.Empty;
                frmOrgUnit frm = new frmOrgUnit();
                if (frm != null)
                {
                    frm.GetCompanyId_By_Proc(CommonClass.UserInfo.companyid, "QSP_GET_BASMIDDLESITE2", dic, ref strSQL);
                }
                if (strSQL == "无效存储过程名称")
                {
                    MsgBox.ShowOK(strSQL);
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                //list.Add(new SqlPara("Province", ConvertType.ToString(MiddleProvince.Text) == "" ? "%%" : ConvertType.ToString(MiddleProvince.Text)));
                //list.Add(new SqlPara("City", ConvertType.ToString(MiddleCity.Text) == "" ? "%%" : ConvertType.ToString(MiddleCity.Text)));
                //list.Add(new SqlPara("Area", ConvertType.ToString(MiddleArea.Text) == "" ? "%%" : ConvertType.ToString(MiddleArea.Text)));
                //list.Add(new SqlPara("Street", ConvertType.ToString(MiddleStreet.Text) == "" ? "%%" : ConvertType.ToString(MiddleStreet.Text)));
                //list.Add(new SqlPara("Type", ConvertType.ToString(Type.Text) == "全部" ? "%%" : ConvertType.ToString(Type.Text)));
                list.Add(new SqlPara("strSQL", strSQL));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "USP_BASMIDDLESITE_OptionSQL", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0)
                {
                    return;
                }
                else
                {
                    myGridControl1.DataSource = ds.Tables[0];
                    next = 0;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //保存
        private void btnSubmit_Click(object sender, EventArgs e)
        {

            try
            {
                DataTable dt = null;
                if (!CheckSelect(ref dt)) return;
                if (dt == null) return;

                string MiddleSiteId = "";
                foreach (DataRow dr in dt.Rows)
                {
                    MiddleSiteId += dr["MiddleSiteId"] + "@";
                }
                //begin 2017-05-19 mengdi
                if (string.IsNullOrEmpty(MiddleSiteId))
                {
                    MsgBox.ShowError("未找到需要修改的数据！");
                    return;
                }
                //end 
                List<SqlPara> list = new List<SqlPara>();
                //list.Add(new SqlPara("MiddleSiteId", MiddleSiteId));
                //list.Add(new SqlPara("FetchWebName", FetchWebName.Text.Trim()));
                //list.Add(new SqlPara("SendWebName", SendWebName.Text.Trim()));
                //list.Add(new SqlPara("SiteName", SiteName.Text.Trim()));
                //list.Add(new SqlPara("Ascription", cb_Ascription.Text.Trim()));//归属地
                //list.Add(new SqlPara("MiddleProvince", cb_MiddleProvince.Text.Trim()));//省份
                //list.Add(new SqlPara("MiddleCity", cb_MiddleCity.Text.Trim()));//城市
                //list.Add(new SqlPara("MiddleArea", cb_MiddleArea.Text.Trim()));//区县
                //list.Add(new SqlPara("MiddleStreet", cb_MiddleStreet.Text.Trim()));//街道
                //站点
                int isEditSite = 0;
                if (checkBox1.Checked)
                {
                    isEditSite = 1;
                }
                //list.Add(new SqlPara("isEditSite", isEditSite));

                //网点
                int isEditWeb = 0;
                if (checkBox2.Checked)
                {
                    isEditWeb = 1;
                }
                //list.Add(new SqlPara("isEditWeb", isEditWeb));

                //追加归属地
                int isEditAscription = 0;
                if (ck_add_ascription.Checked)
                {
                    isEditAscription = 1;
                    if (string.IsNullOrEmpty(cb_Ascription.Text.Trim()))
                    {
                        MsgBox.ShowOK("归属地不能为空！");
                        return;
                    }
                }

                //移除归属地
                int isEditRemoveAscription = 0;
                if (ck_remove_ascription.Checked)
                {
                    isEditRemoveAscription = 1;
                    if (string.IsNullOrEmpty(cb_Ascription.Text.Trim()))
                    {
                        MsgBox.ShowOK("归属地不能为空！");
                        return;
                    }
                    string[] arr = cb_Ascription.Text.Trim().Split(',');
                    if (arr.Length > 1)
                    {
                        MsgBox.ShowOK("请选择单个归属地！");
                        return;
                    }
                }

                //list.Add(new SqlPara("isEditAscription", isEditAscription));
                //list.Add(new SqlPara("isEditRemoveAscription", isEditRemoveAscription));

                //省份
                int isEditProvince = 0;
                if (ck_MiddleProvince.Checked)
                {
                    isEditProvince = 1;
                }
                //list.Add(new SqlPara("isEditProvince", isEditProvince));

                //城市
                int isEditCity = 0;
                if (ck_MiddleCity.Checked)
                {
                    isEditCity = 1;
                }
                //list.Add(new SqlPara("isEditCity", isEditCity));

                //区县
                int isEditArea = 0;
                if (ck_MiddleArea.Checked)
                {
                    isEditArea = 1;
                }
                //list.Add(new SqlPara("isEditArea", isEditArea));

                //街道
                int isEditStreet = 0;
                if (ck_MiddleStreet.Checked)
                {
                    isEditStreet = 1;
                }
                //list.Add(new SqlPara("isEditStreet", isEditStreet));

                if (isEditWeb == 0 && isEditSite == 0 && isEditAscription == 0 && isEditProvince == 0 && isEditCity == 0 && isEditArea == 0 && isEditStreet == 0 && isEditRemoveAscription == 0)
                {
                    MsgBox.ShowOK("请选择修改类型！");
                    return;
                }

                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("FetchWebName", FetchWebName.Text.Trim());
                dic.Add("SendWebName", SendWebName.Text.Trim());
                dic.Add("SiteName", SiteName.Text.Trim());
                dic.Add("MiddleSiteId", MiddleSiteId);
                dic.Add("Ascription", cb_Ascription.Text.Trim());
                dic.Add("MiddleProvince", cb_MiddleProvince.Text.Trim());
                dic.Add("MiddleCity", cb_MiddleCity.Text.Trim());
                dic.Add("MiddleArea", cb_MiddleArea.Text.Trim());
                dic.Add("MiddleStreet", cb_MiddleStreet.Text.Trim());
                dic.Add("isEditSite", isEditSite.ToString());
                dic.Add("isEditWeb", isEditWeb.ToString());
                dic.Add("isEditAscription", isEditAscription.ToString());
                dic.Add("isEditRemoveAscription", isEditRemoveAscription.ToString());
                dic.Add("isEditProvince", isEditProvince.ToString());
                dic.Add("isEditCity", isEditCity.ToString());
                dic.Add("isEditArea", isEditArea.ToString());
                dic.Add("isEditStreet", isEditStreet.ToString());


                string strSQL = string.Empty;
                frmOrgUnit frm = new frmOrgUnit();
                if (frm != null)
                {
                    frm.GetCompanyId_By_Proc(CommonClass.UserInfo.companyid, "USP_UPDATE_basMiddleSite", dic, ref strSQL);
                }
                if (strSQL == "无效存储过程名称")
                {
                    MsgBox.ShowOK(strSQL);
                    return;
                }
                list.Add(new SqlPara("strSQL", strSQL));

                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_BASMIDDLESITE_OptionSQL", list)) > 0)
                {
                    MsgBox.ShowOK();
                    //this.Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //检测是否钩选
        private bool CheckSelect(ref DataTable SelectDt)
        {
            myGridView2.PostEditor();
            DataTable dt = myGridControl1.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 0) return false;
            SelectDt = dt.Clone();


            foreach (DataRow dr in dt.Rows)
            {
                if (ConvertType.ToInt32(dr["ischecked"]) == 0) continue;
                SelectDt.ImportRow(dr);//将选择的行存到新表
            }
            if (SelectDt.Rows.Count == 0)
            {
                MsgBox.ShowOK("请选择数据!");
                return false;
            }
            if (SelectDt.Rows.Count > 200)
            {
                MsgBox.ShowOK("最多只能操作200条数据!");
                return false;
            }
            return true;

        }

        /// <summary>
        /// mengdi 2017-06-05 修改路由增加多选按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            int a = checkEdit1.Checked == true ? 1 : 0;
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                myGridView2.SetRowCellValue(i, "ischecked", a);
            }
            next = 0;
            if (this.checkEdit1.Checked) this.ck_back.Checked = false;
        }
        /// <summary>
        /// 省份带出城市
        /// </summary>       
        private void cb_MiddleProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cb_MiddleCity.Properties.Items.Clear();
                cb_MiddleCity.Text = "";
                if (cb_MiddleProvince.Text.Trim() == "")
                {
                    return;
                }
                CommonClass.AreaManager.FillCityToImageComBoxEdit(cb_MiddleCity, cb_MiddleProvince.Properties.Items[cb_MiddleProvince.SelectedIndex].Value);
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message, "系统提示");
            }
        }
        /// <summary>
        /// 城市带出区县
        /// </summary>       
        private void cb_MiddleCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cb_MiddleArea.Properties.Items.Clear();
                cb_MiddleArea.Text = "";
                if (cb_MiddleCity.Text.Trim() == "")
                {
                    return;
                }
                CommonClass.AreaManager.FillCityToImageComBoxEdit(cb_MiddleArea, cb_MiddleCity.Properties.Items[cb_MiddleCity.SelectedIndex].Value);
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message, "系统提示");
            }
        }
        /// <summary>
        /// 区县带出街道
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_MiddleArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cb_MiddleStreet.Properties.Items.Clear();
                cb_MiddleStreet.Text = "";
                if (cb_MiddleArea.Text.Trim() == "")
                {
                    return;
                }
                CommonClass.AreaManager.FillCityToImageComBoxEdit(cb_MiddleStreet, cb_MiddleArea.Properties.Items[cb_MiddleArea.SelectedIndex].Value);
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message, "系统提示");
            }
        }

        private void ck_add_ascription_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ck_add_ascription.Checked)
            {
                this.ck_remove_ascription.Checked = false;
            }
        }

        private void ck_remove_ascription_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ck_remove_ascription.Checked)
            {
                this.ck_add_ascription.Checked = false;
            }
        }

        //反选
        private void ck_back_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dt = myGridControl1.DataSource as DataTable;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row["ischecked"].ToString() == "1")
                    {
                        row["ischecked"] = 0;
                    }
                    else
                    {
                        row["ischecked"] = 1;
                    }
                }
            }
            next = 0;
            if (this.ck_back.Checked) this.checkEdit1.Checked = false;
        }

        //勾选200条
        private void ck_200_CheckedChanged(object sender, EventArgs e)
        {
            #region zaj
            for (int i = 0; i < 200; i++)
            {
                myGridView2.SetRowCellValue(i, "ischecked", 1);
            }
            #endregion

            //DataTable dt = myGridControl1.DataSource as DataTable;
            ////DataTable _dt = myGridControl1.DataSource as DataTable;
            //if (dt == null) return;

            //DataTable _dt = dt.Clone();
            //for (int i = 0; i < myGridView2.RowCount; i++)
            //{
            //    if (myGridView2.IsGroupRow(i))
            //        continue;
            //    var dr = myGridView2.GetDataRow(i);
            //    if (dr == null)
            //        continue;
            //    _dt.Rows.Add(dr.ItemArray);
            //}

            //if (_dt != null && _dt.Rows.Count > 0)
            //{
            //    if (_dt.Rows.Count >= 200)
            //    {
            //        int i = 0;
            //        foreach (DataRow row in _dt.Rows)
            //        {
            //            i++;
            //            if (i > 200)
            //            {
            //                if (row["ischecked"].ToString() == "1")
            //                {
            //                    row["ischecked"] = 0;
            //                }
            //            }
            //            else
            //            {
            //                if (this.ck_200.Checked)
            //                {
            //                    row["ischecked"] = 1;
            //                }
            //                else
            //                {
            //                    row["ischecked"] = 0;
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        foreach (DataRow row in _dt.Rows)
            //        {
            //            if (this.ck_200.Checked)
            //            {
            //                row["ischecked"] = 1;
            //            }
            //            else
            //            {
            //                row["ischecked"] = 0;
            //            }
            //        }
            //    }
            //    next = 1;

               
            //}
      
        }

        //勾选下批200条
        int next = 0;//批次数
        private void sb_Next_Click(object sender, EventArgs e)
        {
            #region  hj
            next++;
            for (int i = next * 200; i < (next + 1) * 200; i++)
            {
                myGridView2.SetRowCellValue(i, "ischecked", 1);
            }

            for (int i = (next - 1) * 200; i < next * 200; i++)
            {
                myGridView2.SetRowCellValue(i, "ischecked", 0);
            }

            #endregion 


            //DataTable dt = myGridControl1.DataSource as DataTable;
            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    if (dt.Rows.Count >= 200)
            //    {
            //        int i = 0;
            //        int j = 0;
            //        bool isfor = false;
            //        foreach (DataRow row in dt.Rows)
            //        {
            //            if (j == 200)
            //            {
            //                if (row["ischecked"].ToString() == "1")
            //                {
            //                    row["ischecked"] = 0;
            //                }
            //                continue;
            //            }
            //            i++;
            //            if (row["ischecked"].ToString() == "1")
            //            {
            //                row["ischecked"] = 0;
            //            }
            //            else
            //            {
            //                if (i >= (next * 200))
            //                {
            //                    row["ischecked"] = 1;
            //                    j++;
            //                    if (i == dt.Rows.Count)
            //                    {
            //                        next = 0;
            //                        isfor = true;
            //                        MsgBox.ShowOK("批量操作完成！");
            //                        break;
            //                    }
            //                }
            //            }
            //        }
            //        if (!isfor) next++;
            //    }
            //    else
            //    {
            //        MsgBox.ShowOK("总行数小于或等于200条！");
            //    }
            //}


        }

        //查询全部网点
        private DataTable GetWebAll()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_BASWEB_OPEN", list);
                DataTable dt = SqlHelper.GetDataTable(spe);
                return dt;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            return null;
        }

        //清空勾选
        private void sb_clear_Click(object sender, EventArgs e)
        {
            DataTable dt = myGridControl1.DataSource as DataTable;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    row["ischecked"] = 0;
                }
            }
            next = 0;
        }

    }
}
