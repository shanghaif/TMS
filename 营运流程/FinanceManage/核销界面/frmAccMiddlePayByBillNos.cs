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
using System.Reflection;


namespace ZQTMS.UI
{
    public partial class frmAccMiddlePayByBillNos : BaseForm
    {
        private DataSet ds = new DataSet();
        private DataSet ds1 = new DataSet();
        GridHitInfo hitInfo = null;
        public int selectType { get; set; } //查询类型 0按单号 1按中转运单号 2按中转承运商

        public frmAccMiddlePayByBillNos()
        {
            InitializeComponent();
            barEditItem1.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem1_EditValueChanging);
            barEditItem1.Edit.KeyDown += new KeyEventHandler(barEditItem1_KeyDown);
            barEditItem2.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem2_EditValueChanging);
            barEditItem2.Edit.KeyDown += new KeyEventHandler(barEditItem2_KeyDown);
        }

        private void getdata()
        {
            try
            {
                if (string.IsNullOrEmpty(txtBillNos.Text) && string.IsNullOrEmpty(txtBillNo.Text))
                {
                    XtraMessageBox.Show("请先输入单号或批次号。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string sb = txtBillNos.Text.Trim().Replace("\r\n", "@");
                List<SqlPara> list = new List<SqlPara>();
                if (selectType == 0)
                {
                    list.Add(new SqlPara("selectType", 0));
                    list.Add(new SqlPara("BillNos", sb.ToString() + "@"));
                }
                else
                {
                    list.Add(new SqlPara("selectType", 1));
                    list.Add(new SqlPara("MiddleBillNo", sb.ToString() + "@"));
                }

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_AccMiddlePay_Verify_BybillNos", new List<SqlPara>(list));
                ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    XtraMessageBox.Show("查无此单", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                //2017.7.1wwb
                if (myGridView2.RowCount > 0)
                {
                    int rowhandle = myGridView2.RowCount;
                    string allBillNo = "";
                    for (int i = 0; i < myGridView2.RowCount; i++)
                    {
                        allBillNo += myGridView2.GetRowCellValue(i, "BillNo") + "@";
                    }
                    string[] billNos;
                    string BillNo;
                    string BillNo2;

                    billNos = allBillNo.Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < billNos.Length; j++)
                    {
                        BillNo = billNos[j];
                        for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                        {
                            BillNo2 = ds.Tables[0].Rows[k]["BillNo"].ToString();
                            if (BillNo == BillNo2)
                            {
                                ds.Tables[0].Select("BillNo='" + BillNo2 + "'")[0].Delete();
                                ds.AcceptChanges();
                            }

                        }

                    }

                }







                    myGridControl1.DataSource = ds.Tables[0];
                if (ds1 == null || ds1.Tables.Count == 0)
                {
                    ds1 = ds.Clone();
                    myGridControl2.DataSource = ds1.Tables[0];
                }

                //留下没有查到的数据
                string retrunStr = string.Empty;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (selectType == 0)
                    {
                        retrunStr = sb.Replace(row["BillNo"].ToString(), "");
                        sb = retrunStr;
                    }
                    if (selectType == 1)
                    {
                        retrunStr = sb.Replace(row["MiddleBillNo"].ToString(), "");
                        sb = retrunStr;
                    }
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
                this.txtBillNos.Focus();
            } 
        }

        private void frmBatchFetchSign_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, false, myGridView2);
            GridOper.SetGridViewProperty(myGridView1, myGridView2);
            BarMagagerOper.SetBarPropertity(bar1, bar2);  //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1, myGridView2);

            splitContainerControl1.Horizontal = false;
            splitContainerControl1.SplitterPosition = 480;
        } 


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

        ////////////////////////////////////////
        private void barEditItem1_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue == null) return;
            string szfilter = e.NewValue.ToString().Trim();
            if (szfilter != "")
            {
                myGridView1.ClearColumnsFilter();
                myGridView1.Columns["BillNo"].FilterInfo = new ColumnFilterInfo("[BillNo] LIKE " + "'%" + szfilter + "%'", "");
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
                myGridView2.Columns["BillNo"].FilterInfo = new ColumnFilterInfo("[BillNo] LIKE " + "'%" + szfilter + "%'", "");
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
            VerifyOffAccountDeel voaDeel = new VerifyOffAccountDeel();
            voaDeel.SubmitVerify(myGridView2, ds1, VerifyType.中转费.ToString(), "BillNo", null, "CurrentVerifyFee", "AccMiddlePayLeft", "支出");
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
            GridOper.ShowAutoFilterRow(myGridView2);
        }

        private void barButtonItem19_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView2);
        }

        /// <summary>
        /// 按中转承运商提取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMiddleCarry_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtCarry.Text))
            {
                XtraMessageBox.Show("请先输入车牌号。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("selectType", 2)); //按中转承运商查询
            list.Add(new SqlPara("MiddleCarrier", this.txtCarry.Text.Trim()));

            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_AccMiddlePay_Verify_BybillNos", new List<SqlPara>(list));
            ds = SqlHelper.GetDataSet(sps);
            if (ds == null || ds.Tables[0].Rows.Count == 0)
            {
                XtraMessageBox.Show("查无此单", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //2017.7.1wwb
            if (myGridView2.RowCount > 0)
            {
                int rowhandle = myGridView2.RowCount;
                string allBillNo = "";
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    allBillNo += myGridView2.GetRowCellValue(i, "BillNo") + "@";
                }
                string[] billNos;
                string BillNo;
                string BillNo2;

                billNos = allBillNo.Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < billNos.Length; j++)
                {
                    BillNo = billNos[j];
                    for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                    {
                        BillNo2 = ds.Tables[0].Rows[k]["BillNo"].ToString();
                        if (BillNo == BillNo2)
                        {
                            ds.Tables[0].Select("BillNo='" + BillNo2 + "'")[0].Delete();
                            ds.AcceptChanges();
                        }

                    }

                }

            }

            myGridControl1.DataSource = ds.Tables[0];
            if (ds1 == null || ds1.Tables.Count == 0)
            {
                ds1 = ds.Clone();
                myGridControl2.DataSource = ds1.Tables[0];
            }
        }

        private void txtBillNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.txtBillNos.Text = this.txtBillNo.Text;
                selectType = 0;
                Check();
                getdata();
                this.txtBillNo.Text = "";
                this.txtBillNo.Focus();
            }
        }

        private void txtBillNos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                selectType = 0;
                Check();
            }
        }

        private void Check()
        {
            string[] spStr = this.txtBillNos.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            if (ds != null && ds.Tables.Count > 0)
            {
                string str = spStr[spStr.Length - 1];
                if (selectType == 0)
                {
                    if (ds.Tables[0].Select("BillNO=" + str + "") != null && ds.Tables[0].Select("BillNO=" + str + "").Length > 0)
                    {
                        string strRep = this.txtBillNos.Text.Replace(str, "");
                        string[] strArr = strRep.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                        this.txtBillNos.Text = string.Join("\r\n", strArr);
                        //string[] strArr = strRep.Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries);
                        //this.txtBillNos.Text = string.Join("@", spStr);
                        return;
                    }


                    if (ds1 != null && ds1.Tables.Count > 0)
                    {
                        if (ds1.Tables[0].Select("BillNO=" + str + "") != null && ds1.Tables[0].Select("BillNO=" + str + "").Length > 0)
                        {
                            string strRep = this.txtBillNos.Text.Replace(str, "");
                            string[] strArr = strRep.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                            this.txtBillNos.Text = string.Join("\r\n", strArr);
                            //string[] strArr = strRep.Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries);
                            //this.txtBillNos.Text = string.Join("@", spStr);
                            return;
                        }
                    }
                }
                else if (selectType == 1)
                {
                    if (ds.Tables[0].Select("MiddleBillNo='" + str + "'") != null && ds.Tables[0].Select("MiddleBillNo='" + str + "'").Length > 0)
                    {
                        string strRep = this.txtBillNos.Text.Replace(str, "");
                        string[] strArr = strRep.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                        this.txtBillNos.Text = string.Join("\r\n", strArr);
                        //string[] strArr = strRep.Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries);
                        //this.txtBillNos.Text = string.Join("@", spStr);
                        return;
                    }


                    if (ds1 != null && ds1.Tables.Count > 0)
                    {
                        if (ds1.Tables[0].Select("MiddleBillNo='" + str + "'") != null && ds1.Tables[0].Select("MiddleBillNo='" + str + "'").Length > 0)
                        {
                            string strRep = this.txtBillNos.Text.Replace(str, "");
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

        private void barButtonItem20_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rows = myGridView1.FocusedRowHandle;
            string a = myGridView1.GetRowCellValue(rows, "BillNo").ToString();
            if (rows < 0) return;
            Assembly ass = Assembly.LoadFrom(Application.StartupPath + "\\Plugin\\ZQTMS.UI.BuisinessManage.dll");
            if (ass == null) return;
            Type type = ass.GetType("ZQTMS.UI.frmBillSearchControl");
            if (type == null) return;
            Form frm = (Form)Activator.CreateInstance(type);
            if (frm == null) return;
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Tag = a;
            frm.ShowDialog();

        }    //plh20200521 LMS--6539


     

        private void myGridView1_MouseUp(object sender, MouseEventArgs e)
        {
            PopMenu.ShowPopupMenu(myGridView1, e, popupMenu1);
            int rows = myGridView1.FocusedRowHandle;
            if (rows < 0) return;

        }

        private void barButtonItem21_ItemClick(object sender, ItemClickEventArgs e)
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
        }  //plh20200521 LMS--6539
    }
}