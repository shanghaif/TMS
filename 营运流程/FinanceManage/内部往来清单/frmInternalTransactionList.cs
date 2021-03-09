using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmInternalTransactionList : BaseForm
    {
        public frmInternalTransactionList()
        {
            InitializeComponent();
        }

        public string IDS = "";

        private void frmInternalTransactionList_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("内部往来清单");//xj/2019/5/29
            FixColumn fix = new FixColumn(myGridView1, barSubItem1);
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例 
            CommonClass.SetCause(BearSubject, true);
            CommonClass.SetCause(BenefitSubject, true);
            BearSubject.Text = CommonClass.UserInfo.CauseName;
            BenefitSubject.Text = CommonClass.UserInfo.CauseName;
            InsideType.Text = "";
            SerialNumber.Text = "";

            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
        }
        /// <summary>
        /// 提取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            getdata();
        }
        public void getdata()
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("bdate", bdate.DateTime));
            list.Add(new SqlPara("edate", edate.DateTime));
            list.Add(new SqlPara("BearSubject", BearSubject.Text.Trim() == "全部" ? "%%" : BearSubject.Text.Trim()));
            list.Add(new SqlPara("BenefitSubject", BenefitSubject.Text.Trim() == "全部" ? "%%" : BenefitSubject.Text.Trim()));
            list.Add(new SqlPara("InsideType", InsideType.Text.Trim() == "" ? "%%" : InsideType.Text.Trim()));
            list.Add(new SqlPara("SerialNumber", SerialNumber.Text.Trim()));
            SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_InternalTransactionList", list);
            DataSet ds = SqlHelper.GetDataSet(spe);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                myGridControl1.DataSource = ds.Tables[0];
            }
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmInternalTransactionListAdd frm = new frmInternalTransactionListAdd();
            frm.type = 0;
            frm.ShowDialog();
            getdata();
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            string scmen = myGridView1.GetRowCellValue(rowhandle, "UploadMen").ToString();

            if (CommonClass.UserInfo.UserName != scmen)
            {
                MsgBox.ShowOK("只有上传人才能进行修改!");
                return;
            }
            string ID = myGridView1.GetRowCellValue(rowhandle, "ID").ToString();
            if (ID == "") return;
           DataRow dr = getInfo(ID);
           if (dr != null)
           {
               if (dr["AuditStatus"].ToString() != "待审核")
               {
                   MsgBox.ShowOK("该数据已审核,无法进行修改!");
                   return;
               }
               else
               {
                   frmInternalTransactionListAdd frm = new frmInternalTransactionListAdd();
                   frm.ID = ID;
                   frm.type = 1;
                   frm.ShowDialog();
                   getdata();
               }
           }
           


        }
       

        private void checkall_CheckedChanged(object sender, EventArgs e)
        {
            int chk = checkall.Checked ? 1 : 0;
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                myGridView1.SetRowCellValue(i, "ischecked", chk);
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myGridView1.PostEditor();
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                 string ck = myGridView1.GetRowCellValue(i, "ischecked").ToString();
                 if (ck == "1")
                 {
                     string state = myGridView1.GetRowCellValue(i, "AuditStatus").ToString();
                     string scmen = myGridView1.GetRowCellValue(i, "UploadMen").ToString();
                     if (state != "待审核")
                     {
                         MsgBox.ShowOK("第" +(i+1)+ "行数据状态不为待审核状态,请刷新核对!");
                         return;
                     }
                     if (CommonClass.UserInfo.UserName != scmen)
                     {
                         MsgBox.ShowOK("勾选了不是您上传的数据,无法删除,请核对!");
                         return;
                     }

                 }
              
            }
            getIDS();
            if (IDS == "") return;
            else
            {
                if (MsgBox.ShowYesNo("确定要删除吗?") != DialogResult.Yes) return;
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("IDS", IDS));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "DSP_InternalTransactionList", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    if (myGridView1.RowCount == 1)
                    {
                        myGridView1.DeleteRow(myGridView1.FocusedRowHandle);
                    }
                    MsgBox.ShowOK();
                    getdata();
                }
            }
        }

        public void getIDS()
        {
            myGridView1.PostEditor();
            IDS = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                string ck = myGridView1.GetRowCellValue(i, "ischecked").ToString();
                if (ck == "1")
                {
                    string id = myGridView1.GetRowCellValue(i, "ID").ToString();
                    IDS += id + "@";
                }
            }
        }

        private void barButtonItem16_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "内部往来清单");
        }


        /// <summary>
        /// 承担主体审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem17_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myGridView1.PostEditor();
            string webname = CommonClass.UserInfo.WebName;
            string allIDS = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                string ck = myGridView1.GetRowCellValue(i, "ischecked").ToString();
                if (ck == "1")
                {
                    string BearDep = myGridView1.GetRowCellValue(i, "BearDep").ToString();
                    if (webname != BearDep)
                    {
                        MsgBox.ShowOK("勾选了当前网点不是承担部门的数据,请确认!");
                        return;
                    }
                    string state = myGridView1.GetRowCellValue(i, "AuditStatus").ToString();
                    if (state != "待审核" && state != "受益主体已审核")
                    {
                        MsgBox.ShowOK("勾选了当前状态无法审核的数据,请检查!");
                        return;
                    }
                    string ID=myGridView1.GetRowCellValue(i,"ID").ToString();
                    allIDS += ID + "@";
                }
            }
            if (allIDS == "") return;
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("allIDS", allIDS));
            list.Add(new SqlPara("type", 0));
            SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "USP_InternalTransactionList_HEX", list);
            if (SqlHelper.ExecteNonQuery(spe) > 0)
            {
                MsgBox.ShowOK();
                getdata();
            }           
          
        }


        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DataRow getInfo(string ID)
        {
           
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("ID", ID));
            SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_InternalTransactionList_BY_ID", list);
            DataSet ds = SqlHelper.GetDataSet(spe);
            if (ds != null & ds.Tables[0].Rows.Count > 0)
            {
               
                return ds.Tables[0].Rows[0];
            }
            else return null;
        }
        /// <summary>
        /// 承担主体反审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem20_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myGridView1.PostEditor();
            string webname = CommonClass.UserInfo.WebName;
            string allIDS = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                string ck = myGridView1.GetRowCellValue(i, "ischecked").ToString();
                if (ck == "1")
                {
                    string BearDep = myGridView1.GetRowCellValue(i, "BearDep").ToString();
                    if (webname != BearDep)
                    {
                        MsgBox.ShowOK("勾选了当前网点不是承担部门的数据,请确认!");
                        return;
                    }
                    string state = myGridView1.GetRowCellValue(i, "AuditStatus").ToString();
                    if (state != "承担主体已审核")
                    {
                        MsgBox.ShowOK("勾选了当前状态无法反审核的数据,请检查!");
                        return;
                    }
                    string ID = myGridView1.GetRowCellValue(i, "ID").ToString();
                    allIDS += ID + "@";
                }
            }
            if (allIDS == "") return;
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("allIDS", allIDS));
            list.Add(new SqlPara("type", 1));
            SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "USP_InternalTransactionList_HEX", list);
            if (SqlHelper.ExecteNonQuery(spe) > 0)
            {
                MsgBox.ShowOK();
                getdata();
            }           
          
        }
        /// <summary>
        /// 受益主体审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem18_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myGridView1.PostEditor();
            string webname = CommonClass.UserInfo.WebName;
            string allIDS = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                string ck = myGridView1.GetRowCellValue(i, "ischecked").ToString();
                if (ck == "1")
                {
                    string BenefitDep = myGridView1.GetRowCellValue(i, "BenefitDep").ToString();
                    if (webname != BenefitDep)
                    {
                        MsgBox.ShowOK("勾选了当前网点不是受益部门的数据,请确认!");
                        return;
                    }
                    string state = myGridView1.GetRowCellValue(i, "AuditStatus").ToString();
                    if (state != "待审核" && state != "承担主体已审核")
                    {
                        MsgBox.ShowOK("勾选了当前状态无法审核的数据,请检查!");
                        return;
                    }
                    string ID = myGridView1.GetRowCellValue(i, "ID").ToString();
                    allIDS += ID + "@";
                }
            }
            if (allIDS == "") return;
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("allIDS", allIDS));
            list.Add(new SqlPara("type", 2));
            SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "USP_InternalTransactionList_HEX", list);
            if (SqlHelper.ExecteNonQuery(spe) > 0)
            {
                MsgBox.ShowOK();
                getdata();
            }           
        }
        /// <summary>
        /// 受益主体反审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem19_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myGridView1.PostEditor();
            string webname = CommonClass.UserInfo.WebName;
            string allIDS = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                string ck = myGridView1.GetRowCellValue(i, "ischecked").ToString();
                if (ck == "1")
                {
                    string BenefitDep = myGridView1.GetRowCellValue(i, "BenefitDep").ToString();
                    if (webname != BenefitDep)
                    {
                        MsgBox.ShowOK("勾选了当前网点不是受益部门的数据,请确认!");
                        return;
                    }
                    string state = myGridView1.GetRowCellValue(i, "AuditStatus").ToString();
                    if (state != "受益主体已审核")
                    {
                        MsgBox.ShowOK("勾选了当前状态无法反审核的数据,请检查!");
                        return;
                    }
                    string ID = myGridView1.GetRowCellValue(i, "ID").ToString();
                    allIDS += ID + "@";
                }
            }
            if (allIDS == "") return;
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("allIDS", allIDS));
            list.Add(new SqlPara("type", 3));
            SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "USP_InternalTransactionList_HEX", list);
            if (SqlHelper.ExecteNonQuery(spe) > 0)
            {
                MsgBox.ShowOK();
                getdata();
            }           
        }
        /// <summary>
        /// 超级反审
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem21_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myGridView1.PostEditor();
            string allIDS = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                string ck = myGridView1.GetRowCellValue(i, "ischecked").ToString();
                if (ck == "1")
                {
                    string state = myGridView1.GetRowCellValue(i, "AuditStatus").ToString();
                    if (state != "已完成")
                    {
                        MsgBox.ShowOK("勾选了当前状态无法反审核的数据,请检查!");
                        return;
                    }
                    string ID = myGridView1.GetRowCellValue(i, "ID").ToString();
                    allIDS += ID + "@";
                }
            }
            if (allIDS == "") return;
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("allIDS", allIDS));
            list.Add(new SqlPara("type", 4));
            SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "USP_InternalTransactionList_HEX", list);
            if (SqlHelper.ExecteNonQuery(spe) > 0)
            {
                MsgBox.ShowOK();
                getdata();
            }           
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmInternalTransactionListUpLoad frm = new frmInternalTransactionListUpLoad();
            frm.ShowDialog();
            getdata();
        }

        private void barButtonItem22_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmInternalType frm = new frmInternalType();
            frm.ShowDialog();
        }

    }
}