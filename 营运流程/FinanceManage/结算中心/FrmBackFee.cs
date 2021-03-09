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

namespace ZQTMS.UI
{
    public partial class FrmBackFee : BaseForm
    {
        public FrmBackFee()
        {
            InitializeComponent();
        }

        private void FrmBackFee_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("费用返款");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, myGridView2, myGridView3, myGridView4, myGridView5);
            GridOper.SetGridViewProperty(myGridView1, myGridView2, myGridView3, myGridView4, myGridView5);
            BarMagagerOper.SetBarPropertity(bar3, bar4, bar5, bar6, bar7); //如果有具体的工具条，就引用其实例
            #region 提付返款代码

            Fetchbsite.Text = CommonClass.UserInfo.SiteName;
            Fetchesite.Text = "全部";
            Fetchweb.Text = CommonClass.UserInfo.WebName;

            FetchCause.Text = CommonClass.UserInfo.CauseName;
            FetchArea.Text = CommonClass.UserInfo.AreaName;
            Fetchbdate.DateTime = CommonClass.gbdate;
            Fetchedate.DateTime = CommonClass.gedate;

            CommonClass.SetSite(Fetchbsite, true);
            CommonClass.SetSite(Fetchesite, true);
            CommonClass.SetCause(FetchCause, true);
            CommonClass.SetArea(FetchArea, FetchCause.Text, true);
            CommonClass.SetCauseWeb(Fetchweb, FetchCause.Text, FetchArea.Text);
            GridOper.RestoreGridLayout(myGridView1, myGridView2, myGridView3, myGridView4, myGridView5);
           // FixColumn fix = new FixColumn(myGridView1, barSubItem2);

            #endregion

            #region 送货费返款代码
           // CommonClass.FormSet(this);
            //CommonClass.GetGridViewColumns(myGridView1);
            //GridOper.SetGridViewProperty(myGridView1);
           // BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例

            SendBsite.Text = CommonClass.UserInfo.SiteName;
            SendEsite.Text = "全部";
            SendWeb.Text = CommonClass.UserInfo.WebName;

            SendCause.Text = CommonClass.UserInfo.CauseName;
            SendArea.Text = CommonClass.UserInfo.AreaName;
            SendBdate.DateTime = CommonClass.gbdate;
            SendEdate.DateTime = CommonClass.gedate;

            CommonClass.SetSite(SendBsite, true);
            CommonClass.SetSite(SendEsite, true);
            CommonClass.SetCause(SendCause, true);
            CommonClass.SetArea(SendArea, SendCause.Text, true);
            CommonClass.SetCauseWeb(SendWeb, SendCause.Text, SendArea.Text);
            //GridOper.RestoreGridLayout(myGridView1);
            //FixColumn fix = new FixColumn(myGridView1, barSubItem2);

            //不是总部的财务，事业部固定
            if (CommonClass.UserInfo.SiteName != "总部" && CommonClass.UserInfo.WebName.ToString().Contains("财务"))
            {
                SendCause.Enabled = false;
            }
            #endregion

            #region  中转费返款代码

            //CommonClass.FormSet(this);
            //CommonClass.GetGridViewColumns(myGridView1);
            //GridOper.SetGridViewProperty(myGridView1);
            //BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例

            MiddleBsite.Text = CommonClass.UserInfo.SiteName;
            MiddleEsite.Text = "全部";
            MiddleWeb.Text = CommonClass.UserInfo.WebName;

            MiddleCause.Text = CommonClass.UserInfo.CauseName;
            MiddleArea.Text = CommonClass.UserInfo.AreaName;
            MiddleBdate.DateTime = CommonClass.gbdate;
            MiddleEdate.DateTime = CommonClass.gedate;

            CommonClass.SetSite(MiddleBsite, true);
            CommonClass.SetSite(MiddleEsite, true);
            CommonClass.SetCause(MiddleCause, true);
            CommonClass.SetArea(MiddleArea, MiddleCause.Text, true);
            CommonClass.SetCauseWeb(MiddleWeb, MiddleCause.Text, MiddleArea.Text);
            //GridOper.RestoreGridLayout(myGridView1);
            //FixColumn fix = new FixColumn(myGridView1, barSubItem2);

            //不是总部的财务，事业部固定
            if (CommonClass.UserInfo.SiteName != "总部" && CommonClass.UserInfo.WebName.ToString().Contains("财务"))
            {
                MiddleCause.Enabled = false;
            }
            #endregion

            #region 附加费返款代码
            //CommonClass.FormSet(this);
            //CommonClass.GetGridViewColumns(myGridView1);
            //GridOper.SetGridViewProperty(myGridView1);
            //BarMagagerOper.SetBarPropertity(bar3);
            //GridOper.RestoreGridLayout(myGridView1);

            AdditionBdate.DateTime = CommonClass.gbdate;
            AdditionEdate.DateTime = CommonClass.gedate;
            AdditionCause.Text = CommonClass.UserInfo.CauseName;
            AdditionArea.Text = CommonClass.UserInfo.AreaName;
            AdditionWeb.Text = CommonClass.UserInfo.WebName;


            CommonClass.SetCause(AdditionCause, true);
            
            #endregion


            #region 作废单返款
            //CommonClass.FormSet(this);
            //CommonClass.GetGridViewColumns(myGridView1);
            //GridOper.SetGridViewProperty(myGridView1);
            //BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            ObsoleteBdate.DateTime = CommonClass.gbdate;
            ObsoleteEdate.DateTime = CommonClass.gedate;

            CommonClass.SetCause(ObsoleteCause, true);
            ObsoleteCause.EditValue = CommonClass.UserInfo.CauseName;
            ObsoleteArea.EditValue = CommonClass.UserInfo.AreaName;
            ObsoleteWeb.EditValue = CommonClass.UserInfo.WebName;
            #endregion
        }

        #region 提付返款代码
        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FetchArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(Fetchweb, FetchCause.Text, FetchArea.Text);
        }

        //private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    GridOper.SaveGridLayout(myGridView1);
        //}

        //private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        //}

        //private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    GridOper.ShowAutoFilterRow(myGridView1);
        //}

        //private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    GridOper.ExportToExcel(myGridView1, this.Text);
        //}

        private void FetchAudit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmBackFetchFeeLoad ww = new frmBackFetchFeeLoad();
            ww.Show();
        }
        private void FetchRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", Fetchbdate.DateTime));
                list.Add(new SqlPara("edate", Fetchedate.DateTime));
                list.Add(new SqlPara("causeName", FetchCause.Text.Trim() == "全部" ? "%%" : FetchCause.Text.Trim()));
                list.Add(new SqlPara("AreaName", FetchArea.Text.Trim() == "全部" ? "%%" : FetchArea.Text.Trim()));
                list.Add(new SqlPara("webName", Fetchweb.Text.Trim() == "全部" ? "%%" : Fetchweb.Text.Trim()));
                list.Add(new SqlPara("StartSite", Fetchbsite.Text.Trim() == "全部" ? "%%" : Fetchbsite.Text.Trim()));
                list.Add(new SqlPara("TransferSite", Fetchesite.Text.Trim() == "全部" ? "%%" : Fetchesite.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_FetchFee_Aduit", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
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
      

        //private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    GridOper.AllowAutoFilter(myGridView1);
        //}

        //private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    this.Close();
        //}
        #endregion

        #region 送货费返款代码
        private void SendClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SendCause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(SendArea, SendCause.Text);
        }

        private void SendArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(SendWeb, SendCause.Text, SendArea.Text);
        }

        //private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    GridOper.SaveGridLayout(myGridView1);
        //}

        //private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        //}

        //private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    GridOper.ShowAutoFilterRow(myGridView1);
        //}

        //private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    GridOper.ExportToExcel(myGridView1, this.Text);
        //}

        private void SendAudit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmBackSendFeeLoad ww = new frmBackSendFeeLoad();
            ww.Show();
        }

        private void SendRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", SendBdate.DateTime));
                list.Add(new SqlPara("edate", SendEdate.DateTime));
                list.Add(new SqlPara("causeName", SendCause.Text.Trim() == "全部" ? "%%" : SendCause.Text.Trim()));
                list.Add(new SqlPara("AreaName", SendArea.Text.Trim() == "全部" ? "%%" : SendArea.Text.Trim()));
                list.Add(new SqlPara("webName", SendWeb.Text.Trim() == "全部" ? "%%" : SendWeb.Text.Trim()));
                list.Add(new SqlPara("StartSite", SendBsite.Text.Trim() == "全部" ? "%%" : SendBsite.Text.Trim()));
                list.Add(new SqlPara("TransferSite", SendEsite.Text.Trim() == "全部" ? "%%" : SendEsite.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_DeliveryFee", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl2.DataSource = ds.Tables[0];
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

        //private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    GridOper.AllowAutoFilter(myGridView1);
        //}

        //private void SendClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    this.Close();
        //}

        #endregion


        #region  中转费返款代码
        private void MilldeClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MiddleCause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(MiddleArea, MiddleCause.Text);
        }

        private void MiddleArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(MiddleWeb, MiddleCause.Text, MiddleArea.Text);
        }

        //private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    GridOper.SaveGridLayout(myGridView1);
        //}

        //private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        //}

        //private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    GridOper.ShowAutoFilterRow(myGridView1);
        //}

        //private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    GridOper.ExportToExcel(myGridView1, this.Text);
        //}

        private void MiddleAudit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmBackTransferFeeLoad ww = new frmBackTransferFeeLoad();
            ww.Show();
        }

        private void MiddleRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", MiddleBdate.DateTime));
                list.Add(new SqlPara("edate", MiddleEdate.DateTime));
                list.Add(new SqlPara("causeName", MiddleCause.Text.Trim() == "全部" ? "%%" : MiddleCause.Text.Trim()));
                list.Add(new SqlPara("AreaName", MiddleArea.Text.Trim() == "全部" ? "%%" : MiddleArea.Text.Trim()));
                list.Add(new SqlPara("webName", MiddleWeb.Text.Trim() == "全部" ? "%%" : MiddleWeb.Text.Trim()));
                list.Add(new SqlPara("StartSite", MiddleBsite.Text.Trim() == "全部" ? "%%" : MiddleBsite.Text.Trim()));
                list.Add(new SqlPara("TransferSite", MiddleEsite.Text.Trim() == "全部" ? "%%" : MiddleEsite.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_TransferFee", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl3.DataSource = ds.Tables[0];
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

        //private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    GridOper.AllowAutoFilter(myGridView1);
        //}

        //private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    this.Close();
        //}

        #endregion

        #region 附加费返款代码
        /// <summary>
        /// 提取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdditionRetrieve_Click(object sender, EventArgs e)
        {
            List<SqlPara> lst = new List<SqlPara>();
            try
            {
                lst.Add(new SqlPara("bdate", AdditionBdate.DateTime));
                lst.Add(new SqlPara("edate", AdditionEdate.DateTime));
                lst.Add(new SqlPara("causeName", AdditionCause.Text.Trim() == "全部" ? "%%" : AdditionCause.Text.Trim()));
                lst.Add(new SqlPara("AreaName", AdditionArea.Text.Trim() == "全部" ? "%%" : AdditionArea.Text.Trim()));
                lst.Add(new SqlPara("webName", AdditionWeb.Text.Trim() == "全部" ? "%%" : AdditionWeb.Text.Trim()));
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BackExtracharge", lst));
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl4.DataSource = ds.Tables[0];
                GridOper.CreateStyleFormatCondition(myGridView1, "UpState", DevExpress.XtraGrid.FormatConditionEnum.Expression, "", Color.Green, false, "UpState='已返款'");
                GridOper.CreateStyleFormatCondition(myGridView1, "ZXState", DevExpress.XtraGrid.FormatConditionEnum.Expression, "", Color.Green, false, "ZXState='已返款'");
                GridOper.CreateStyleFormatCondition(myGridView1, "JCState", DevExpress.XtraGrid.FormatConditionEnum.Expression, "", Color.Green, false, "JCState='已返款'");
                GridOper.CreateStyleFormatCondition(myGridView1, "CCState", DevExpress.XtraGrid.FormatConditionEnum.Expression, "", Color.Green, false, "CCState='已返款'");
                GridOper.CreateStyleFormatCondition(myGridView1, "ChaCheState", DevExpress.XtraGrid.FormatConditionEnum.Expression, "", Color.Green, false, "ChaCheState='已返款'");
                GridOper.CreateStyleFormatCondition(myGridView1, "KHState", DevExpress.XtraGrid.FormatConditionEnum.Expression, "", Color.Green, false, "KHState='已返款'");
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdditionClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 自动筛选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    GridOper.AllowAutoFilter(myGridView1);
        //}
        /// <summary>
        /// 锁定外观
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        //}
        /// <summary>
        /// 取消外观
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        //}
        /// <summary>
        /// 过滤器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    GridOper.ShowAutoFilterRow(myGridView1);
        //}
        /// <summary>
        /// 核销
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdditionAudit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmExtrachargeLoad frm = new frmExtrachargeLoad();
            frm.Show();
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    GridOper.ExportToExcel(myGridView1, "附加费返款明细");
        //}
        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    this.Close();
        //}

        private void AdditionCause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(AdditionArea,AdditionCause.Text.Trim(), true);
        }

        private void AdditionArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(AdditionWeb, AdditionCause.Text.Trim(), AdditionArea.Text.Trim(), true);
        }

        #endregion

        #region 作废单返款
        private void Cause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(ObsoleteArea, ObsoleteCause.Text, true);
            CommonClass.SetCauseWeb(ObsoleteWeb, ObsoleteCause.Text, ObsoleteArea.Text, true);
        }

        private void ObsoleteRetrieve_Click(object sender, EventArgs e)
        {
            List<SqlPara> list = new List<SqlPara>();
            try
            {
                list.Add(new SqlPara("t1", ObsoleteBdate.DateTime));
                list.Add(new SqlPara("t2", ObsoleteEdate.DateTime));
                list.Add(new SqlPara("Cause", ObsoleteCause.Text.Trim() == "全部" ? "%%" : ObsoleteCause.Text.Trim()));
                list.Add(new SqlPara("Area", ObsoleteArea.Text.Trim() == "全部" ? "%%" : ObsoleteArea.Text.Trim()));
                list.Add(new SqlPara("Web", ObsoleteWeb.Text.Trim() == "全部" ? "%%" : ObsoleteWeb.Text.Trim()));
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_BILL_OUT_REFUND", list));
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl5.DataSource = ds.Tables[0];


            }
            catch (Exception ex)
            {

                MsgBox.ShowException(ex);
            }
        }

        private void ObsoleteAudit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmRefundVerify frm = new frmRefundVerify();
            frm.ShowDialog();
        }

        //private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    GridOper.ExportToExcel(myGridView1);
        //}

        //private void cbClose_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}

        //private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    this.Close();
        //}

        //private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    GridOper.AllowAutoFilter(myGridView1);
        //}

        //private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        //}

        //private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        //}

        //private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    GridOper.ShowAutoFilterRow(myGridView1);
        //}

        #endregion


    }
}
