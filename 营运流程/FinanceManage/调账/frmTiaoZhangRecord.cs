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
using DevExpress.XtraEditors;
using ZQTMS.Lib;

namespace ZQTMS.UI
{
    public partial class frmTiaoZhangRecord : BaseForm
    {
        private DataSet ds = new DataSet();   //maohui20180531
        private DataSet dsNew = new DataSet(); //maohui20180531

        public frmTiaoZhangRecord()
        {
            InitializeComponent();
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("UserName", UserName.Text.Trim()));
                list.Add(new SqlPara("Type", Type.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_TIAOZHANG_RECORD", list);
                ds = SqlHelper.GetDataSet(sps);
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                if (myGridView1.RowCount < 2000) myGridView1.BestFitColumns();
            }
        }

        private void WayBillRecord_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("调账管理");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例

            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;

            GridOper.RestoreGridLayout(myGridView1);
            CommonClass.SetUser(UserName,"全部",true);
            UserName.EditValue = CommonClass.UserInfo.UserName;
            GridOper.CreateStyleFormatCondition(myGridView1, "ApplyState", DevExpress.XtraGrid.FormatConditionEnum.Equal, "审核", Color.Green);//已通过 绿色
            GridOper.CreateStyleFormatCondition(myGridView1, "ApplyState", DevExpress.XtraGrid.FormatConditionEnum.Equal, "否决", Color.Red);//已否决，黄色
            
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "调账记录");
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmTiaoZhangAdd frm = new frmTiaoZhangAdd();
            frm.ShowDialog();
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SH("审核");
        }
        

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SH("否决");
        }

        private void SH(string state)
        {
            if (MsgBox.ShowYesNo("是否" + state + "?\r\r此操作不可逆，请确认！") != DialogResult.Yes)
            {
                return;
            }
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid TjId = new Guid(myGridView1.GetRowCellValue(rowhandle, "TjId").ToString());

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("TjId", TjId));
                list.Add(new SqlPara("ApplyState", state));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_TiaoZhang_APPly", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    myGridView1.SetRowCellValue(rowhandle, "ApplyState", state);
                    myGridView1.SetRowCellValue(rowhandle, "ApplyMan", CommonClass.UserInfo.UserName);
                    myGridView1.SetRowCellValue(rowhandle, "ApplyDate", CommonClass.gcdate);
                }
            }
            catch (Exception ex)
            {
                string errmsg = ex.Message.ToString();
                MsgBox.ShowOK(errmsg.Replace("数据库访问异常：", ""));
            }
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmTiaoZhangUP frm = new frmTiaoZhangUP();
            frm.ShowDialog();
            cbRetrieve_Click(null,null);
        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)  //maohui20180531
        {
            if (MsgBox.ShowYesNo("是否审核？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
            {
                return;
            }
            dsNew = ds.Clone();
            int rowhandle = myGridView1.RowCount;
            for (int i = 0; i < rowhandle; i++)
            {
                System.Threading.Thread.Sleep(300);
                if (myGridView1.RowCount >= 150)
                {
                    if (myGridView1.RowCount % 150 >= 0)
                    {
                        DataTable table = ds.Tables[0];
                        DataTable table2 = dsNew.Tables[0];
                        table2.Rows.Clear();

                        for (int j = 0; j < 150; j++)
                        {
                            table2.Rows.Add(table.Rows[j].ItemArray);

                        }
                        table2.AcceptChanges();
                        myGridControl2.DataSource = dsNew.Tables[0];
                    }

                    senddata(myGridView2);

                    if (dsNew.Tables[0].Rows.Count == 0)
                    {
                        for (int k = 0; k < 150; k++)
                        {
                            ds.Tables[0].Rows[k].Delete();
                        }
                        ds.AcceptChanges();
                    }
                    if (ds.Tables[0].Rows.Count == 0 || myGridView1.RowCount == 0)
                    {
                        cbRetrieve_Click(null, null);
                        return;
                    }

                    if (myGridView1.RowCount < 150 && myGridView1.RowCount > 0)
                    {
                        senddata(myGridView1);
                        cbRetrieve_Click(null, null);
                        return;
                    }
                }
                if (myGridView1.RowCount < 150)
                {
                    senddata(myGridView1);
                    cbRetrieve_Click(null, null);
                    return;
                }
            }
        }

        public void senddata(MyGridView myGridView3)  //maouhui20180531
        {
            try
            {
                string TjIds = "";
                for (int i = 0; i < myGridView3.RowCount; i++)
                {

                    TjIds += myGridView3.GetRowCellValue(i, "TjId").ToString() + ",";


                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("TjIds", TjIds));
                list.Add(new SqlPara("ApplyState", "审核"));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "USP_TiaoZhang_APPlys", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    MsgBox.ShowOK();
                    dsNew.Clear();

                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
    }
}