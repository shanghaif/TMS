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
using DevExpress.XtraEditors.Repository;

namespace ZQTMS.UI
{
    public partial class frmFetchToOwePayLoad : BaseForm
    {
        private DataSet ds = new DataSet();
        private DataSet ds1 = new DataSet();
        GridHitInfo hitInfo;
    
        private DataSet ds3 = new DataSet();//月结
        private DataSet ds4 = new DataSet();//回单付
        private DataSet ds5 = new DataSet();//货到前付
        private DataSet ds6 = new DataSet();//欠付

        public frmFetchToOwePayLoad()
        {
            InitializeComponent();
            barEditItem1.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem1_EditValueChanging);
            barEditItem1.Edit.KeyDown += new KeyEventHandler(barEditItem1_KeyDown);
            barEditItem2.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem2_EditValueChanging);
            barEditItem2.Edit.KeyDown += new KeyEventHandler(barEditItem2_KeyDown);
            this.panelControl1.Hide();
        }

        private void getdata()
        {
            myGridControl1.DataSource = myGridControl2.DataSource = null;
            try
            {
                ds.Clear();
                ds1.Clear();
                myGridView1.ClearColumnsFilter();
                myGridView2.ClearColumnsFilter();

                List<SqlPara> list = new List<SqlPara>();
                string CauseName = barEditCause == null || barEditCause.EditValue == null ? CommonClass.UserInfo.CauseName : barEditCause.EditValue.ToString();
                string AreaName = barEditArea == null || barEditArea.EditValue == null ? CommonClass.UserInfo.AreaName : barEditArea.EditValue.ToString();
                string WebName = barEditWeb == null || barEditWeb.EditValue == null ? CommonClass.UserInfo.WebName : barEditWeb.EditValue.ToString();
                CauseName = CauseName == "全部" ? "%%" : CauseName;
                AreaName = AreaName == "全部" ? "%%" : AreaName;
                WebName = WebName == "全部" ? "%%" : WebName;
                list.Add(new SqlPara("CauseName", CauseName));
                list.Add(new SqlPara("AreaName", AreaName));
                list.Add(new SqlPara("BegWeb", WebName));

                list.Add(new SqlPara("t1", barEditItem3.EditValue));
                list.Add(new SqlPara("t2", barEditItem4.EditValue));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_FetchToOwePay_InFee_Load_NEW", list);
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

        private void frmBatchFetchSign_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, false, myGridView2);
            GridOper.SetGridViewProperty(myGridView1, myGridView2);
            BarMagagerOper.SetBarPropertity(bar1, bar2);  //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1, myGridView2);

            CommonClass.Create_BarEditItem_Web(barManager1, bar1, barEditWeb);
            CommonClass.Create_BarEditItem_Area(barManager1, bar1, barEditArea);
            CommonClass.Create_BarEditItem_Cause(barManager1, bar1, barEditCause);
            barEditCause.EditValueChanged += new System.EventHandler(this.barEditItem1_EditValueChanged);
            barEditArea.EditValueChanged += new System.EventHandler(this.barEditItem1_EditValueChanged_2);
            barEditCause.EditValue = CommonClass.UserInfo.CauseName;
            barEditArea.EditValue = CommonClass.UserInfo.AreaName;
            barEditWeb.EditValue = CommonClass.UserInfo.WebName;
            barEditItem3.EditValue = "2019-03-01 0:00";
            barEditItem4.EditValue = CommonClass.gcdate;  //ywc20190315


            splitContainerControl1.Horizontal = false;
            splitContainerControl1.SplitterPosition = 480;
        }

        BarEditItem barEditCause = new BarEditItem(); //生成事业部
        BarEditItem barEditArea = new BarEditItem(); //生成大区
        BarEditItem barEditWeb = new BarEditItem(); //生成网点

        private void barEditItem1_EditValueChanged(object sender, EventArgs e)
        {
            RepositoryItemComboBox repositoryItemComboBox = (RepositoryItemComboBox)barEditArea.Edit;
            repositoryItemComboBox.Items.Clear();
            CommonClass.SetArea(repositoryItemComboBox, barEditCause.EditValue + "", true);
            barEditArea.EditValue = "全部";
        }
        private void barEditItem1_EditValueChanged_2(object sender, EventArgs e)
        {
            RepositoryItemComboBox repositoryItemComboBox = (RepositoryItemComboBox)barEditWeb.Edit;
            repositoryItemComboBox.Items.Clear();
            CommonClass.SetCauseWeb(repositoryItemComboBox, barEditCause.EditValue + "", barEditArea.EditValue + "");
            barEditWeb.EditValue = "全部";
        }

        #region 移动网格数据相关
        private void gridControl2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GridViewMove.Move(myGridView2, ds1, ds);
        }

        private void gridControl2_DragDrop(object sender, DragEventArgs e)
        {
            GridViewMove.Move(myGridView1, ds, ds1);
        }

        private void gridControl1_MouseMove(object sender, MouseEventArgs e)
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

        private void gridControl2_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void gridControl1_MouseDown(object sender, MouseEventArgs e)
        {
            hitInfo = myGridView1.CalcHitInfo(new Point(e.X, e.Y));
        }

        private void gridControl2_MouseMove(object sender, MouseEventArgs e)
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

        private void gridControl2_MouseDown(object sender, MouseEventArgs e)
        {
            hitInfo = myGridView2.CalcHitInfo(new Point(e.X, e.Y));
        }

        private void gridControl1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void gridControl1_DragDrop(object sender, DragEventArgs e)
        {
            GridViewMove.Move(myGridView2, ds1, ds);
        }

        private void gridControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GridViewMove.Move(myGridView1, ds, ds1);
        }
        #endregion

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            getdata();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView1.OptionsView.ShowAutoFilterRow = !myGridView1.OptionsView.ShowAutoFilterRow;
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            //GridViewMove.QuickSearch();
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

        private void btnClose_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView2);
        }

   
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

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            #region 限制未执行改单，不能核销 zb20190507
            try
            {
                StringBuilder sb = new StringBuilder();
                List<SqlPara> list = new List<SqlPara>();
                //获取批量运单号
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    sb.Append(myGridView2.GetRowCellValue(i, "BillNo"));
                    sb.Append("@");
                }
                list.Add(new SqlPara("BillNos", sb));
                //list.Add(new SqlPara("BillNos", "4407588@4385539@4460360@14254065@")); 测试
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_Get_ApplyStateByBillNo", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string BillNO = ds.Tables[0].Rows[i]["BillNO"].ToString();
                    int LastState = Convert.ToInt32(ds.Tables[0].Rows[i]["LastState"].ToString());
                    if (LastState < 3)
                    {
                        MsgBox.ShowOK("单号:" + BillNO + "未执行改单,不能进行核销，请先执行改单申请!");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                return;
            }
             string allBillNo = "";
          
            ds3 = ds1.Clone();
            ds3.Clear();
            ds4 = ds1.Clone();
            ds4.Clear();
            ds5= ds1.Clone();
            ds5.Clear();
            ds6 = ds1.Clone();
            ds6.Clear();

            VerifyOffAccountDeel voaDeel = new VerifyOffAccountDeel();

            if (myGridView2.RowCount == 0)
            {
                XtraMessageBox.Show("没有发现任何需要核销的单，请先在第①步中构建清单。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (myGridView2.RowCount > 0)
            {
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    string billno = myGridView2.GetRowCellValue(i, "BillNo").ToString();
                    allBillNo += billno + ",";
                    string type = myGridView2.GetRowCellValue(i, "FeeType").ToString();                 


                    if (type == "月结")
                    {
                        ds3.Tables[0].ImportRow(ds1.Tables[0].Rows[i]);
                    }

                    else if (type == "回单付")
                    {
                        ds4.Tables[0].ImportRow(ds1.Tables[0].Rows[i]);
                    }
                    else if (type == "欠付")
                    {
                        ds6.Tables[0].ImportRow(ds1.Tables[0].Rows[i]);
                    }
                    else
                    {
                        ds5.Tables[0].ImportRow(ds1.Tables[0].Rows[i]);
                    }
                }
            
            }

            if (ds3 != null && ds3.Tables.Count != 0 && ds3.Tables[0].Rows.Count != 0)
            {
                voaDeel.SubmitVerify(myGridView2, ds3, VerifyType.月结.ToString(), "BillNo", null, "CurrentVerifyFee", "AmountLeft", "收入");
               
            }
            if (ds4 != null && ds4.Tables.Count != 0 && ds4.Tables[0].Rows.Count != 0)
            {
                voaDeel.SubmitVerify(myGridView2, ds4, VerifyType.回单付.ToString(), "BillNo", null, "CurrentVerifyFee", "AmountLeft", "收入");
               
            }
            if (ds6 != null && ds6.Tables.Count != 0 && ds6.Tables[0].Rows.Count != 0)
            {
                voaDeel.SubmitVerify(myGridView2, ds6, VerifyType.欠付.ToString(), "BillNo", null, "CurrentVerifyFee", "AmountLeft", "收入");
                
            }
            if (ds5 != null && ds5.Tables.Count != 0 && ds5.Tables[0].Rows.Count != 0)
            {
                voaDeel.SubmitVerify(myGridView2, ds5, VerifyType.货到前付.ToString(), "BillNo", null, "CurrentVerifyFee", "AmountLeft", "收入");
            
            }
            ds1.Clear();

            #endregion
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem15_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem16_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem17_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView2);
        }

        private void barButtonItem18_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView2.Guid.ToString());
        }

        private void barButtonItem19_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView2);
        }


        private void barButtonItem22_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (splitContainerControl1.Horizontal == true)
            {
                splitContainerControl1.Horizontal = false;
                splitContainerControl1.SplitterPosition = 480;
                return;
            }
            if (splitContainerControl1.Horizontal == false)
            {
                splitContainerControl1.Horizontal = true;
                splitContainerControl1.SplitterPosition = 800;
                return;
            }
        }
        //打开
        private void barButtonItem23_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.panelControl1.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtBillNos.Text))
                {
                    XtraMessageBox.Show("请先输入单号或批次号。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string sb = txtBillNos.Text.Trim().Replace("\r\n", "@");
                string[] aa = sb.Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries);
                int rowHandle = myGridView1.RowCount;
                for (int i = 0; i < rowHandle; i++)
                {    
                    string sb3 = GridOper.GetRowCellValueString(myGridView1, i, "BillNo");
                    for (int j = 0; j < aa.Length; j++)
                    {
                        string sb4 = aa[j];
                        if (sb4 == sb3&&aa.Length == rowHandle)
                        {
                            txtBillNos.Text = "";
                            MsgBox.ShowOK("输入的单号已提取");
                            return;
                        }
                    }
                }
                List<SqlPara> list = new List<SqlPara>();
                string CauseName = barEditCause == null || barEditCause.EditValue == null ? CommonClass.UserInfo.CauseName : barEditCause.EditValue.ToString();
                string AreaName = barEditArea == null || barEditArea.EditValue == null ? CommonClass.UserInfo.AreaName : barEditArea.EditValue.ToString();
                string WebName = barEditWeb == null || barEditWeb.EditValue == null ? CommonClass.UserInfo.WebName : barEditWeb.EditValue.ToString();
                CauseName = CauseName == "全部" ? "%%" : CauseName;
                AreaName = AreaName == "全部" ? "%%" : AreaName;
                WebName = WebName == "全部" ? "%%" : WebName;
                list.Add(new SqlPara("CauseName", CauseName));
                list.Add(new SqlPara("AreaName", AreaName));
                list.Add(new SqlPara("BegWeb", WebName));
                list.Add(new SqlPara("t1", barEditItem3.EditValue));
                list.Add(new SqlPara("t2", barEditItem4.EditValue));
                list.Add(new SqlPara("BillNos", sb.ToString() + "@"));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_FetchToOwePay_InFee_Load_NEW__piliang", list);
                ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    XtraMessageBox.Show("查无此单", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (ds == null || ds.Tables.Count == 0) return;
                ds1 = ds.Clone();
                myGridControl1.DataSource = ds.Tables[0];
                myGridControl2.DataSource = ds1.Tables[0];

                string retrunStr = string.Empty;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    retrunStr = sb.Replace(row["BillNo"].ToString(), "");
                    sb = retrunStr;
                }
                string[] spStr = retrunStr.Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries);
                retrunStr = string.Join("@", spStr);
                txtBillNos.Text = retrunStr.Replace("@", "\r\n");

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.txtBillNos.Text = "";
            this.panelControl1.Hide();
        }
    }
}