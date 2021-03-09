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

namespace ZQTMS.UI
{
    public partial class frmUsers : BaseForm
    {
        public frmUsers()
        {
            InitializeComponent();
        }
        private int isFirst = 0;
        private void frmUsers_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("用户管理");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar11); //如果有具体的工具条，就引用其实例
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            GridOper.RestoreGridLayout(myGridView1);
            GridOper.CreateStyleFormatCondition(myGridView1, "IsRestart", DevExpress.XtraGrid.FormatConditionEnum.Equal, 1, Color.FromArgb(255, 255, 128));
            
            freshData();
        }

        private void barBtnUserAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmAddUsers frm = new frmAddUsers();
            frm.ShowDialog();
            freshData();
        }

        private void barBtnUserMod_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "UserId").ToString());

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("UserId", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SYSUSERINFO_ByID", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                DataRow dr = ds.Tables[0].Rows[0];

                frmAddUsers frm = new frmAddUsers();
                frm.dr = dr;
                frm.ShowDialog();
                freshData();

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barBtnUserFilter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barBtnUserDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "UserId").ToString());

                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("UserId", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_SYSUSERINFO", list);
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

        private void barBtnUserFresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            freshData();
        }

        private void barBtnvUserExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "用户信息");
        }

        private void barBtnUserExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
        private void freshData()
        {
            panel1.Visible = true;
            Thread th = new Thread(() =>
            {
                try
                {
                    List<SqlPara> list = new List<SqlPara>();

                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SYSUSERINFO", list);
                    DataSet ds = SqlHelper.GetDataSet(sps);

                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                    if (!this.IsHandleCreated) return;
                    this.Invoke((MethodInvoker)delegate
                    {
                        myGridControl1.DataSource = ds.Tables[0];
                        myGridView1_FocusedRowChanged(null,null);
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
                            panel1.Visible = false;
                            if (myGridView1.RowCount < 1000) myGridView1.BestFitColumns();
                        });
                    }
                }
            });
            th.IsBackground = true;
            th.Start();

            //CommonInfo model = new CommonInfo();
            //model.ProcName = "";
            //List<System.sql
            //    Dictionary<String,String> dis= new Dictionary<string,string>();
            //dis.Add(""

            //CommonBLL bll = new CommonBLL();
        }

        private void myGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (barBtnUserMod.Enabled) barBtnUserMod.PerformClick();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "UserId").ToString());

                if (MsgBox.ShowYesNo("是否重置该用户密码？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("userid", id));
                list.Add(new SqlPara("userpsw", ""));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_RESET_USERPASS", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "UserId").ToString());

                if (MsgBox.ShowYesNo("是否让当前选中的用户强制重新登录？") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("userid", id));
                list.Add(new SqlPara("oper", 1));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_SET_User_Restart", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    myGridView1.SetRowCellValue(rowhandle, "IsRestart", 1);
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (MsgBox.ShowYesNo("是否让所有用户强制重新登录？") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("userid", Guid.NewGuid()));
                list.Add(new SqlPara("oper", 2));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_SET_User_Restart", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    for (int i = 0; i < myGridView1.RowCount; i++)
                    {
                        myGridView1.SetRowCellValue(i, "IsRestart", 1);
                    }
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void CheckAll(int enabled)
        {
            try
            {
                string msg = enabled == 1 ? "启用" : "禁用";
                if (MsgBox.ShowYesNo(string.Format("是否让所有用户【{0}】登录验证？", msg)) != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("Enabled", enabled));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_UPDATE_UserInfo_Validate_All", list);
                SqlHelper.ExecteNonQuery(sps);

                if (myGridControl1.DataSource == null) return;
                DataTable dt = myGridControl1.DataSource as DataTable;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["EnableValidate"] = enabled;
                }
                MsgBox.ShowOK();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void CheckSub(int enabled)
        {
            try
            {
                if (MsgBox.ShowYesNo("是否让界面列表中的用户启用登录验证？") != DialogResult.Yes)
                {
                    return;
                }

                string UserAccount = "";
                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    UserAccount = myGridView1.GetRowCellValue(i, "UserAccount").ToString();
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("UserAccount", UserAccount));
                    list.Add(new SqlPara("Enabled", enabled));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_UPDATE_UserInfo_Validate_Signle", list);
                    SqlHelper.ExecteNonQuery(sps);

                    myGridView1.SetRowCellValue(i, "EnableValidate", enabled);
                }
                MsgBox.ShowOK();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void CheckSignle(int enabled)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;

                if (MsgBox.ShowYesNo("是否让当前选中的用户启用登录验证？") != DialogResult.Yes)
                {
                    return;
                }

                string UserAccount = myGridView1.GetRowCellValue(rowhandle, "UserAccount").ToString();

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("UserAccount", UserAccount));
                list.Add(new SqlPara("Enabled", enabled));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_UPDATE_UserInfo_Validate_Signle", list);
                SqlHelper.ExecteNonQuery(sps);

                myGridView1.SetRowCellValue(rowhandle, "EnableValidate", enabled);

                MsgBox.ShowOK();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CheckAll(1);
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CheckAll(0);
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CheckSub(1);
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CheckSub(0);
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CheckSignle(1);
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CheckSignle(0);
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;

            string UserAccount = myGridView1.GetRowCellValue(rowhandle, "UserAccount").ToString();
            frmUsersValidate frm = new frmUsersValidate();
            frm.userAccount = UserAccount;
            frm.Show();
        }

        private void barButtonItem14_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0)
                {
                    MsgBox.ShowOK("请选择要清空验证信息的用户!");
                    return;
                }
                string UserAccount = myGridView1.GetRowCellValue(rowhandle, "UserAccount").ToString();

                if (MsgBox.ShowYesNo(string.Format("此操作将清空账号【{0}】的登录验证信息。是否继续？", UserAccount)) == DialogResult.No) return;

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("UserAccount", UserAccount));
                list.Add(new SqlPara("ValidationInfo", ""));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_User_ValidateInfo", list);
                MsgBox.ShowOK();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "UserId").ToString());
                if (MsgBox.ShowYesNo("是否禁止该用户登陆系统?") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("userid", id));
                list.Add(new SqlPara("IsLoginEnable", "1"));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_SET_IsLoginEnable", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    freshData();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem16_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "UserId").ToString());

                if (MsgBox.ShowYesNo("是否恢复该用户登陆！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("userid", id));
                list.Add(new SqlPara("IsLoginEnable", "0"));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_SET_IsLoginEnable", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    freshData();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        private void myGridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (isFirst == 0)
            {
                rowhandle = 0;
                isFirst++;
            }
            if (rowhandle < 0) return;
            if (myGridView1.GetRowCellValue(rowhandle, "IsLoginEnable").ToString() == "1")
            {
                barButtonItem16.Enabled = true;
                barButtonItem15.Enabled = false;
            }
            else
            {
                barButtonItem16.Enabled = false;
                barButtonItem15.Enabled = true;
            }
        }
    }
}