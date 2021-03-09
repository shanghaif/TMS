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
using System.Threading;
using System.Text.RegularExpressions;

namespace ZQTMS.UI
{
    public partial class frmAuditBatch_ShortOwe : BaseForm
    {
        private DataSet ds = new DataSet();
        private DataSet ds1 = new DataSet();
        GridHitInfo hitInfo;
        public int NO;

        public frmAuditBatch_ShortOwe()
        {
            InitializeComponent();
            barEditItem1.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem1_EditValueChanging);
            barEditItem1.Edit.KeyDown += new KeyEventHandler(barEditItem1_KeyDown);
            barEditItem2.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem2_EditValueChanging);
            barEditItem2.Edit.KeyDown += new KeyEventHandler(barEditItem2_KeyDown);

            ds.Clear();
            myGridView2.ClearColumnsFilter(); 
        }

        private void getdata()
        {
            try
            {
                if (string.IsNullOrEmpty(txtBillNos.Text))
                {
                    XtraMessageBox.Show("请先输入单号。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string sb = txtBillNos.Text.Trim().Replace("\r\n", "@");
                //2017.5.26 wwb
                string[] aa = sb.Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries);
                int rowHandle = myGridView2.RowCount;
                for (int i = 0; i < rowHandle;i++ )
                {
                   string sb2 = GridOper.GetRowCellValueString(myGridView2, i, "BillNo");
                   for (int j = 0; j < aa.Length; j++)
                   {
                       string sb3 = aa[j];
                       if (sb3 == sb2)
                       {
                           txtBillNos.Text = "";
                           return;
                       }
                   }
                }

                List<SqlPara> list = new List<SqlPara>();
               // list.Add(new SqlPara("selectType", 1));
                list.Add(new SqlPara("BillNos", sb.ToString() + "@"));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_billCancellation_2", new List<SqlPara>(list));
                ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    XtraMessageBox.Show("查无此单", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                myGridControl1.DataSource = ds.Tables[0];
                if (ds1 == null || ds1.Tables.Count == 0)
                {
                    ds1 = ds.Clone();
                    myGridControl2.DataSource = ds1.Tables[0];
                }

                //留下没有查到的数据
                //string[] retrunStr = sb.Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries);
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
            finally
            {
                //this.txtBillNos.Text = "";
                this.txtBillNos.Focus();
            }
        }

        private void frmAuditBatch_ShortOwe_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, false, myGridView2);
            GridOper.SetGridViewProperty(myGridView1, myGridView2);
            BarMagagerOper.SetBarPropertity(bar1, bar2);  //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1, myGridView2);

            //GridOper.GetGridViewColumn(myGridView1, "DiscountTransfer").AppearanceCell.BackColor = Color.Yellow;
            //GridOper.GetGridViewColumn(myGridView1, "isIncom").AppearanceCell.BackColor = Color.Yellow; 
        } 

        private void barEditItem1_EditValueChanged(object sender, EventArgs e)
        { 
        }
        private void barEditItem1_EditValueChanged_2(object sender, EventArgs e)
        { 
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
        //导出
        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }
        //锁定外观
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
        //右边锁定外观
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

        //private void btnClose_ItemClick(object sender, ItemClickEventArgs e)
        //{
        //    this.Close();
        //}
        //右边导出
        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView2);
        }

        ////////////////////////////////////////
        //左边速配网格
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
        //左边速配网格事件
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
        //右边速配网格
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
        //右边速配网格事件
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

        //外观筛选
        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }
        //取消外观
        private void barButtonItem15_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }
        //过滤器
        private void barButtonItem16_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }
        //右边筛选
        private void barButtonItem17_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView2);
        }
        //右边取消外观
        private void barButtonItem18_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView2.Guid.ToString());
        }
        //右边过滤器
        private void barButtonItem19_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView2);
        }



        /// <summary>
        /// 剔除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barEditItem3_EditValueChanged(object sender, ItemClickEventArgs e)
        {
            try
            {
                this.txtBillNos.Text = "";
                this.txtBillNos.Focus();
                //int rowhandle = myGridView1.FocusedRowHandle;
                //if (rowhandle < 0) return;

                //if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                //{
                //    return;
                //}

                //ds.Tables[0].Rows.RemoveAt(rowhandle);
                //myGridView1.DeleteRow(rowhandle);
                //myGridView1.PostEditor();
                //myGridView1.UpdateCurrentRow();
                //myGridView1.UpdateSummary();
                //myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        //单个单号提取键入事件
        private void txtBillNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.txtBillNos.Text = this.txtBillNo.Text;
                txtBillNos_KeyDown(sender, e);
                getdata();
                this.txtBillNo.Text = "";
                this.txtBillNo.Focus();
            }
        }
        //批量
        private void txtBillNos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string[] spStr = this.txtBillNos.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                if (ds != null && ds.Tables.Count > 0)
                {
                    for (int i = 0; i < spStr.Length; i++)
                    {
                        if (ds.Tables[0].Select("BillNO=" + spStr[i] + "") != null && ds.Tables[0].Select("BillNO=" + spStr[i] + "").Length > 0)
                        {
                            string strRep = this.txtBillNos.Text.Replace(spStr[i], "");
                            string[] strArr = strRep.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                            this.txtBillNos.Text = string.Join("\r\n", strArr);
                            //string[] strArr = strRep.Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries); 
                            //this.txtBillNos.Text = string.Join("@", spStr);
                            return;
                        }


                        if (ds1 != null && ds1.Tables.Count > 0)
                        {
                            if (ds1.Tables[0].Select("BillNO=" + spStr[i] + "") != null && ds1.Tables[0].Select("BillNO=" + spStr[i] + "").Length > 0)
                            {
                                string strRep = this.txtBillNos.Text.Replace(spStr[i], "");
                                string[] strArr = strRep.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                                this.txtBillNos.Text = string.Join("\r\n", strArr);
                                //string[] strArr = strRep.Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries);
                                //this.txtBillNos.Text = string.Join("@", spStr);
                                return;
                            }
                        }
                    }
                }
            }
        }

        public string strCustomerCompany { get; set; }
        private void EndSite_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //frmCustomerQuery frm = new frmCustomerQuery();
            //frm.Owner = this;
            //frm.ShowDialog();

            //this.EndSite.Text = strCustomerCompany;
        }


        //根据客户名称提取
        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textEdit1.Text))
                {
                    XtraMessageBox.Show("请先选择客户名称。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string sb = textEdit1.Text.Trim().Replace(",", "@");
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("NO", NO));
                list.Add(new SqlPara("clients", sb.ToString() + "@"));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_billCancellation_Byclient", new List<SqlPara>(list));
                ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    XtraMessageBox.Show("查无此单", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                myGridControl1.DataSource = ds.Tables[0];
                if (ds1 == null || ds1.Tables.Count == 0)
                {
                    ds1 = ds.Clone();
                    myGridControl2.DataSource = ds1.Tables[0];
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                this.textEdit1.Text = "";
                this.textEdit1.Focus();
            }
        }


       //完成 
        private void barButtonItem11_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            if (MsgBox.ShowYesNo("是否确认金额？") != DialogResult.Yes) return;
            string BillNos = "";
            string ArrComfirmFees = "";
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                BillNos += myGridView2.GetRowCellValue(i, "BillNo").ToString() + "@";
                ArrComfirmFees += myGridView2.GetRowCellValue(i, "ArrComfirmFee").ToString() + "@";

            }
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillNos", BillNos));
            list.Add(new SqlPara("ArrComfirmFees", ArrComfirmFees));
            SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "USP_ADD_ArrConfirm_dq", list);
            if (SqlHelper.ExecteNonQuery(spe) > 0)
            {
                MsgBox.ShowOK();
            }
            }
        } 
    }
