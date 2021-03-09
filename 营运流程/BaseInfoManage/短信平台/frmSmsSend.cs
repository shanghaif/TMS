using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;
using System.IO;
using DevExpress.XtraEditors;
using System.Data.OleDb;
using System.Collections;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using ZQTMS.Common;
using DevExpress;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmSmsSend : BaseForm
    {
        private sms sms = new sms();
        ArrayList listsms = new ArrayList(new string[] { sms.SmsXM.开单.ToString(), sms.SmsXM.配载完成.ToString(), sms.SmsXM.提货签收.ToString(), sms.SmsXM.送货完成.ToString() });

        public frmSmsSend()
        {
            InitializeComponent();
        }

        private void getdata()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SMSGROUPS");
                sps.ParaList = list;

                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0)
                {
                    return;
                }

                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        listView1.Items.Add(ds.Tables[0].Rows[i][0].ToString(), 0);
                        comboBoxEdit1.Properties.Items.Add(ds.Tables[0].Rows[i][0]);
                    }
                }
                comboBoxEdit1.Properties.Items.Add("全部");
                comboBoxEdit1.Text = "全部";
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void getcustomerdata(string dtype)
        {
            try
            {
                ;
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_CUSTOMER_SMS_DTYPE");
                sps.ParaList = list;
                list.Add(new SqlPara("mType", dtype));
                DataSet ds = SqlHelper.GetDataSet(sps);


                if (ds == null || ds.Tables.Count == 0)
                {
                    return;
                }
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void savetreeview()
        {
            try
            {

                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_SMSGROUPS");

                SqlHelper.ExecteNonQuery(sps);


                List<SqlPara> list_1 = new List<SqlPara>();

                SqlParasEntity sps_1 = new SqlParasEntity(OperType.Execute, "USP_ADD_SMSGROUPS");

                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    list.Add(new SqlPara("smsgroup", listView1.Items[i].Text.Trim()));
                    SqlHelper.ExecteNonQuery(sps);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void w_sms_send_Load(object sender, EventArgs e)
        {
            try
            {
                CommonClass.InsertLog("短信平台");//xj/2019/5/29
                CommonClass.FormSet(this);
                xtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
                //cc.LoadScheme(myGridControl1);
                //cc.LoadScheme(gridControl2);
                //navBarItem3.Visible = ur.GetUserRightDetail("f801");
                //navBarItem8.Visible = ur.GetUserRightDetail("f802");
                getdata();
                getsmsid();
                CommonClass.GetGridViewColumns(myGridView4);
                GridOper.SetGridViewProperty(myGridView4);

                CommonClass.SetSite(comboBoxEdit2, true);
                CommonClass.SetSite(comboBoxEdit3, true);
                CommonClass.SetSite(edsite, false);

                comboBoxEdit2.Text = "全部";
                comboBoxEdit3.Text = "全部";
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
            try
            {
                foreach (SimpleButton simpleButton in groupControl1.Controls) //定义短信按钮
                {
                    simpleButton.Click += new EventHandler(simpleButton_Click);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void simpleButton_Click(object sender, EventArgs e)
        {
            try
            {
                int rowhandle = gridView3.FocusedRowHandle;
                if (rowhandle < 0) return;
                if (gridView3.ActiveEditor == null) return;

                MemoEdit edit = (MemoEdit)gridView3.ActiveEditor;
                int pos = edit.SelectionStart;
                string s = edit.Text.Substring(pos);
                int p1 = s.IndexOf("[");
                int p2 = s.IndexOf("]");
                if (p1 > p2 || (p1 < 0 && p2 >= 0)) //说明光标是在[]之间的  sssdf[nn]wfe[NN]fiiff
                {
                    MsgBox.ShowOK("不能在已有项目之间插入其他项目!请检查!");
                    return;
                }

                SimpleButton simpleButton = sender as SimpleButton;
                string text = simpleButton.Text;
                p1 = text.IndexOf("[");
                p2 = text.IndexOf("]");
                text = text.Substring(p1, p2 - p1 + 1);

                edit.Text = edit.Text.Insert(pos, text);
                edit.SelectionStart = pos + text.Length;
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void navBarItem1_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            tp1.PageVisible = false;
            tp3.PageVisible = false;
            tp4.PageVisible = false;
            tp5.PageVisible = false;
            tp2.PageVisible = true;
            tp2.Select();
        }

        private void navBarItem4_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //tp1.PageVisible = false;
            //tp2.PageVisible = false;
            //tp3.PageVisible = false;
            //tp4.PageVisible = false;
            //tp5.PageVisible = true;
            //tp5.Select();

            string[] arr = ThisisLanQiaoSoft.SMS.Check0verage(CommonClass.smsuserid, CommonClass.smsid, CommonClass.smspassword).Split('|');
            if (Convert.ToInt32(arr[0]) == 0)
            {
               MsgBox.ShowOK(arr[1]);
                return;
            }
            MsgBox.ShowOK("短信余额：" + arr[1]);
        }

        private void navBarItem7_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            tp1.PageVisible = false;
            tp2.PageVisible = false;
            tp3.PageVisible = false;
            tp3.PageVisible = false;
            tp5.PageVisible = true;
            tp4.Select();
        }

        private void simpleButton16_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {
            buttonEdit1.Text = "";

            panel8.Visible = true;
            buttonEdit1.Focus();
        }

        private void simpleButton18_Click(object sender, EventArgs e)
        {
            savetreeview();
        }

        private void simpleButton17_Click(object sender, EventArgs e)
        {
            listView1.Items.Remove(listView1.FocusedItem);

        }

        private void buttonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (buttonEdit1.Text.Trim() == "")
            {
                panel8.Visible = false;
                return;
            }

            listView1.Items.Add(buttonEdit1.Text.Trim(), 0);
            panel8.Visible = false;

        }

        private void navBarControl1_ActiveGroupChanged(object sender, DevExpress.XtraNavBar.NavBarGroupEventArgs e)
        {
            if (e.Group.Caption == "地址薄")
            {
                tp2.PageVisible = false;
                tp3.PageVisible = false;
                tp4.PageVisible = false;
                tp5.PageVisible = false;
                tp6.PageVisible = false;

                tp1.PageVisible = true;
                tp1.Select();
            }

            if (e.Group.Caption == "系统管理")
            {
                tp1.PageVisible = false;
                tp2.PageVisible = false;
                tp3.PageVisible = false;
                tp4.PageVisible = false;
                tp6.PageVisible = false;

                //tp5.PageVisible = true;
                //tp5.Select();
            }


            if (e.Group.Caption == "短信操作")
            {
                tp1.PageVisible = false;
                tp3.PageVisible = false;
                tp4.PageVisible = false;
                tp5.PageVisible = false;
                tp6.PageVisible = false;

                tp2.PageVisible = true;
                tp2.Select();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            frmSmsCustomerAdd wsca = new frmSmsCustomerAdd();
            wsca.ShowDialog();
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            getcustomerdata(listView1.FocusedItem.Text.Trim());
        }

        private void simpleButton15_Click(object sender, EventArgs e)
        {
            string xm = "";
            int handle = myGridView1.FocusedRowHandle;

            if (handle < 0) return;
            xm = myGridView1.GetRowCellValue(handle, "project").ToString();
            if (myGridView1.GetRowCellValue(handle, "opertype").ToString() != "手动") return;

            frmSmsCustomerAdd wsca = new frmSmsCustomerAdd();
            wsca.inxm = xm;
            wsca.ShowDialog();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            int handle = myGridView1.FocusedRowHandle;
            if (handle < 0) return;
            string project = myGridView1.GetRowCellValue(handle, "project").ToString();
            if (myGridView1.GetRowCellValue(handle, "opertype").ToString() != "手动") return;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("project", project));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_CUSTOMER_SMS_XM");
                sps.ParaList = list;
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    myGridView1.DeleteRow(handle);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void getsmsid()
        {
            edsmsid.Text = CommonClass.smsid;
            edsmspassword.Text = CommonClass.smspassword;
            if (edsmsid.Text != "") simpleButton13.Enabled = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            groupBox7.Enabled = checkBox2.Checked;
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            try
            {
                if (!sms.checksmsid()) return;
                string mb = "";
                string nr1 = smscontent.Text.Trim();

                int len = nr1.Length;
                if (len == 0)
                {
                    MsgBox.ShowOK("请输入短信内容!");
                    smscontent.Focus();
                    return;
                }

                if (!(nr1.Contains("【") && nr1.Contains("】") && nr1.EndsWith("】")))
                {
                    MsgBox.ShowOK("短信中可能没有签名,无法发送!短信签名必须放在短信内容结尾!\r\n签名示例：比如公司名称是“深圳中强有限责任公司”，公司签名为“【中强】”");
                    return;
                }

                if (rbsingle.Checked) //单条发送
                {
                    if (edmb.Text.Trim() == "")
                    {
                        MsgBox.ShowOK("请输入手机号码!");
                        return;
                    }
                    if (edmb.Text.Contains("，"))
                    {
                        MsgBox.ShowOK("请用英文逗号分隔手机号码!");
                        return;
                    }

                    if (XtraMessageBox.Show("确定发送吗?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

                    //string[] str = edmb.Text.Trim().Split(',');
                    //for (int i = 0; i < str.Length; i++)
                    //{
                    //    sms.SaveSMSS(11, "短信中心发送", str[i], nr1, CommonClass.gcdate, 0);
                    //}
                    if (!sms.SaveSMSS("11", "短信中心发送", edmb.Text.Trim(), nr1, CommonClass.gcdate, "0")) return;
                    if (!sms.sendsms(edmb.Text.Trim(), nr1)) return;
                }

                if (rbmore.Checked) //群发
                {
                    if (gridView2.RowCount == 0) return;
                    if ((gridControl2.DataSource as DataTable).Select("ischecked=1").Length == 0)
                    {
                        MsgBox.ShowOK("请勾选需要发送短信的用户!");
                        return;
                    }
                    if (XtraMessageBox.Show("确定发送吗?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;
                    label14.Visible = true;
                    progressBarControl1.Visible = true;
                    progressBarControl1.Properties.Maximum = gridView2.RowCount - 1;
                    progressBarControl1.Position = 0;

                    int per = 50;//一次提交的手机号码数量
                    List<object> obj = new List<object>();
                    string tempmb = "";
                    int start = 0;
                    for (int i = 0; i < gridView2.RowCount; i++)
                    {
                        progressBarControl1.Position = i;

                        if (Convert.ToInt32(gridView2.GetRowCellValue(i, "ischecked")) == 0) continue;

                        tempmb = gridView2.GetRowCellValue(i, "mb").ToString().Trim();

                        if (tempmb.Length != 11) continue;
                        if (mb == "") start = i;
                        obj.Add(new object[] { gridView2.GetRowCellValue(i, "xm").ToString().Trim(), tempmb });

                        mb += tempmb + ",";
                        if (mb.TrimEnd(',').Split(',').Length == per || i == gridView2.RowCount - 1)
                        {
                            mb = mb.TrimEnd(',');

                            #region 保存
                            object[] arr;
                            for (int m = 0; m < obj.Count; m++)
                            {
                                arr = (object[])obj[m];
                                if (!sms.SaveSMSS("10", arr[0].ToString(), arr[1].ToString(), nr1, CommonClass.gcdate, "0")) return;
                            }
                            #endregion

                            if (sms.sendsms(mb, nr1))
                            {
                                for (int j = start; j < i + 1; j++)
                                {
                                    gridView2.SetRowCellValue(j, "ischecked", 0);
                                    gridView2.SetRowCellValue(j, "state", 1);
                                    gridView2.FocusedRowHandle = j;
                                }

                                Application.DoEvents();

                                mb = "";
                                tempmb = "";
                                start = 0;
                                obj.Clear();
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                }
                XtraMessageBox.Show("短信发送完毕", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                label14.Visible = false;
                progressBarControl1.Visible = false;
            }
        }

        private void rbsingle_CheckedChanged(object sender, EventArgs e)
        {
            groupsingle.Enabled = rbsingle.Checked;
            if (rbsingle.Checked) edmb.Focus();
        }

        private void rbmore_CheckedChanged(object sender, EventArgs e)
        {
            panel5.Enabled = rbmore.Checked;
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            int handle = 0;
            handle = gridView2.FocusedRowHandle;
            if (handle >= 0)
                gridView2.DeleteRow(handle);
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                string bsite = comboBoxEdit2.Text.Trim() == "全部" ? "%%" : comboBoxEdit2.Text.Trim();
                string esite = comboBoxEdit3.Text.Trim() == "全部" ? "%%" : comboBoxEdit3.Text.Trim();
                string dtype = comboBoxEdit1.Text.Trim();

                //ArrayList arr = new ArrayList();
                //arr.Add(new object[] { "bsite", SqlDbType.VarChar, bsite });
                //arr.Add(new object[] { "esite", SqlDbType.VarChar, esite });
                //arr.Add(new object[] { "dtype", SqlDbType.VarChar, dtype });
                //arr.Add(new object[] { "name", SqlDbType.VarChar, "" });
                //arr.Add(new object[] { "companyid", SqlDbType.VarChar, CommonClass.companyid });
                //arr.Add(new object[] { "gstype", SqlDbType.VarChar, CommonClass.gstype });

                //DownLoad dl = new DownLoad();
                ////DataSet dataset = dl.GetCommonData(4, "QSP_GET_CUSTOMER_SMS", arr);
                //if (dataset == null || dataset.Tables.Count == 0) return;

                List<SqlPara> pList = new List<SqlPara>();
                pList.Add(new SqlPara("bsite", bsite));
                pList.Add(new SqlPara("esite", esite));
                pList.Add(new SqlPara("dtype", dtype));
                pList.Add(new SqlPara("name", ""));
                //pList.Add(new SqlPara("companyid", ""));
                pList.Add(new SqlPara("gstype", ""));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_CUSTOMER_SMS");
                sps.ParaList = pList;

                DataSet dataset = SqlHelper.GetDataSet(sps);


                if (dataset == null || dataset.Tables.Count == 0)
                {
                    return;
                }

                int count = dataset.Tables[0].Rows.Count;
                List<string> list = new List<string>();
                string tempmb = "";

                for (int i = count - 1; i >= 0; i--)
                {
                    tempmb = dataset.Tables[0].Rows[i]["mb"].ToString();
                    if (!list.Contains(tempmb))
                    {
                        list.Add(tempmb);
                        if (dataset.Tables[0].Rows[i]["dtype"].ToString() == "1")
                        {
                            dataset.Tables[0].Rows[i]["dtype"] = "发货人";
                        }
                        if (dataset.Tables[0].Rows[i]["dtype"].ToString() == "2")
                        {
                            dataset.Tables[0].Rows[i]["dtype"] = "收货人";
                        }
                    }
                    else
                    {
                        dataset.Tables[0].Rows.RemoveAt(i);
                    }
                }
                if (!dataset.Tables[0].Columns.Contains("ischecked")) //勾选
                {
                    DataColumn ischecked = new DataColumn("ischecked", typeof(System.Int32));
                    ischecked.DefaultValue = 1;
                    dataset.Tables[0].Columns.Add(ischecked);
                }

                if (!dataset.Tables[0].Columns.Contains("state")) //发送状态
                {
                    DataColumn state = new DataColumn("state", typeof(System.Int32));
                    state.DefaultValue = 0;
                    dataset.Tables[0].Columns.Add(state);
                }

                gridControl2.DataSource = dataset.Tables[0];
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void smscontent_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            int len = smscontent.Text.Trim().Length;
            label8.Text = len.ToString() + " / 70  拆分条数：" + Math.Ceiling((decimal)len / (decimal)70).ToString();
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (gridControl2.DataSource == null) return;
            int a = checkEdit1.Checked ? 1 : 0;
            for (int i = 0; i < gridView2.RowCount; i++)
            {
                gridView2.SetRowCellValue(i, "ischecked", a);
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            if (MsgBox.ShowYesNo("导入之前请确认您要导入的Excel表格第一列为客户名称,第二列为手机号码!\r\n\r\n是否继续导入?") == DialogResult.No) return;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "导入客户信息：第一列为客户名称,第二列为手机号码!";
            ofd.Filter = "Microsoft Execl文件(*.xls)|*.xls";
            ofd.FilterIndex = 1;
            ofd.DefaultExt = "xls";
            ofd.FileName = "客户信息";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (!ofd.CheckFileExists)
                {
                    XtraMessageBox.Show("文件不存在，请重新选择!", "文件导入失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                DataSet dataset = DsExecl(ofd.FileName);
                if (dataset == null || dataset.Tables.Count == 0) { return; }

                if (!dataset.Tables[0].Columns.Contains("ischecked")) //勾选
                {
                    DataColumn ischecked = new DataColumn("ischecked", typeof(System.Int32));
                    ischecked.DefaultValue = 1;
                    dataset.Tables[0].Columns.Add(ischecked);
                }

                if (!dataset.Tables[0].Columns.Contains("state")) //发送状态
                {
                    DataColumn state = new DataColumn("state", typeof(System.Int32));
                    state.DefaultValue = 0;
                    dataset.Tables[0].Columns.Add(state);
                }
                gridControl2.DataSource = dataset.Tables[0];
            }
        }

        private DataSet DsExecl(string filePath)
        {
            string str = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + filePath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
            OleDbConnection Conn = new OleDbConnection(str);
            try
            {
                Conn.Open();
                DataTable dt = Conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                string tablename = "", sql = "";
                w_import_select_table wi = new w_import_select_table();
                wi.dt = dt;
                if (wi.ShowDialog() == DialogResult.Yes)
                {
                    tablename = wi.listBoxControl1.Text.Trim();
                    sql = "select * from [" + tablename + "]";
                }
                else
                {
                    return null;
                }

                OleDbDataAdapter da = new OleDbDataAdapter(sql, Conn);
                DataSet ds = new DataSet();
                da.Fill(ds, tablename);


                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Columns.Count < 2)
                    {
                        MsgBox.ShowOK("请确保您选择的表格第一列为客户名称,第二列为手机号码!");
                        return null;
                    }
                    ds.Tables[0].Columns[0].ColumnName = "xm";
                    ds.Tables[0].Columns[1].ColumnName = "mb";
                }

                int count = ds.Tables[0].Rows.Count;
                List<string> list = new List<string>();
                string tempmb = "";
                for (int i = count - 1; i >= 0; i--)
                {
                    tempmb = ds.Tables[0].Rows[i]["mb"].ToString();
                    if (tempmb.Length != 11 || !tempmb.StartsWith("1"))
                    {
                        ds.Tables[0].Rows.RemoveAt(i);
                        continue;
                    }
                    if (!list.Contains(tempmb))
                    {
                        list.Add(tempmb);
                    }
                    else
                    {
                        ds.Tables[0].Rows.RemoveAt(i);
                    }
                }
                return ds;
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
                return null;
            }
            finally
            {
                if (Conn.State != ConnectionState.Closed) Conn.Close();
            }
        }

        private void navBarItem3_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            tp1.PageVisible = false;
            tp2.PageVisible = false;
            tp3.PageVisible = false;
            tp4.PageVisible = false;
            tp6.PageVisible = false;

            tp5.PageVisible = true;
            tp5.Select();
        }

        #region 定义短信
        private void simpleButton9_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_MSG");
                sps.ParaList = list;

                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0)
                {
                    return;
                }

                if (ds.Tables.Count == 0)
                {
                    MsgBox.ShowOK("没有取得短信样式!");
                    simpleButton6.Enabled = false;
                    return;
                }

                Array arr = Enum.GetValues(typeof(sms.SmsXM));

                for (int i = 0; i < arr.Length; i++)
                {
                    if (ds.Tables[0].Select(string.Format("project='{0}'", arr.GetValue(i))).Length == 0)
                    {
                        DataRow dr = ds.Tables[0].NewRow();
                        dr["project"] = arr.GetValue(i);
                        dr["forShipper"] = "";
                        dr["forConsignee"] = "";
                        ds.Tables[0].Rows.Add(dr);
                    }
                }
                gridControl3.DataSource = ds.Tables[0];
                simpleButton6.Enabled = true;
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private bool checksms(string content)
        {
            if (content.Length > 0)
            {
                if (!content.Contains("[gs]")) //不包含公司签名
                {
                    MsgBox.ShowOK("短信格式中没有签名!在短信结尾点击“公司签名”按钮即可插入签名!\r\n提醒：设置完短信格式后,请在“定义短信电话”界面中设置具体签名内容!");
                    return false;
                }
                if (!content.EndsWith("[gs]")) //公司签名是否放在短信结尾
                {
                    MsgBox.ShowOK("公司签名必须放在短信结尾!\r\n公司签名后边不能再添加任何字符!");
                    return false;
                }
            }
            return true;
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            gridView3.PostEditor();
            gridView3.UpdateCurrentRow();

            for (int i = 0; i < gridView3.RowCount; i++)
            {
                if (checksms(gridView3.GetRowCellValue(i, "forShipper").ToString().Trim()) == false)
                {
                    gridView3.FocusedColumn = gridView3.Columns["forShipper"];
                    gridView3.FocusedRowHandle = i;
                    return;
                }
                if (checksms(gridView3.GetRowCellValue(i, "forConsignee").ToString().Trim()) == false)
                {
                    gridView3.FocusedColumn = gridView3.Columns["forConsignee"];
                    gridView3.FocusedRowHandle = i;
                    return;
                }
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_MSG");
                for (int i = 0; i < gridView3.RowCount; i++)
                {
                    list.Clear();
                    list.Add(new SqlPara("project", gridView3.GetRowCellValue(i, "project").ToString().Trim()));
                    list.Add(new SqlPara("forShipper", gridView3.GetRowCellValue(i, "forShipper").ToString().Trim()));
                    list.Add(new SqlPara("forConsignee", gridView3.GetRowCellValue(i, "forConsignee").ToString().Trim()));
                    sps.ParaList = list;
                    SqlHelper.ExecteNonQuery(sps);
                }

                CommonClass.dsmsg.Tables.Clear();
                CommonClass.dsmsg.Tables.Add((gridControl3.DataSource as DataTable).Copy());
                //cc.Log(0, 0, "短信定义", "保存短信格式");
                MsgBox.ShowOK("保存成功!资料设置完毕后请重新登录软件!");
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            GridOper.ExportToExcel(gridView3, "短信样式");
            //cc.ExportToExcel(gridView3, "短信样式");
        }
        #endregion

        #region 定义短信电话
        private void navBarItem8_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            tp1.PageVisible = false;
            tp2.PageVisible = false;
            tp3.PageVisible = false;
            tp4.PageVisible = false;
            tp5.PageVisible = false;

            tp6.PageVisible = true;
            tp6.Select();
            try
            {
                //cc.FillRepComboxBsite(edsite, false);
            }
            catch (Exception) { }
        }

        private void simpleButton58_Click(object sender, EventArgs e)
        {
            string site = edsite.Text.Trim();
            if (site == "") return;
            //SqlConnection Conn = cc.GetConn();
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("site", site));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_MSG_TEL_SITE");
                sps.ParaList = list;

                DataSet ds = SqlHelper.GetDataSet(sps);


                if (ds == null || ds.Tables.Count == 0)
                {
                    return;
                }
                Array arr = Enum.GetValues(typeof(sms.SmsXM));

                for (int i = 0; i < arr.Length; i++)
                {
                    if (ds.Tables[0].Select(string.Format("project='{0}'", arr.GetValue(i))).Length == 0)
                    {
                        DataRow dr = ds.Tables[0].NewRow();
                        dr["site"] = site;
                        dr["project"] = arr.GetValue(i);
                        dr["telephone"] = "";
                        ds.Tables[0].Rows.Add(dr);
                    }
                }
                if (ds.Tables[0].Select("project='公司签名'").Length == 0)
                {
                    DataRow row = ds.Tables[0].NewRow();
                    row["site"] = site;
                    row["project"] = "公司签名";
                    row["telephone"] = "";
                    ds.Tables[0].Rows.Add(row);
                }
                myGridControl4.DataSource = ds.Tables[0];
                simpleButton59.Enabled = true;
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void simpleButton59_Click(object sender, EventArgs e)
        {
            myGridView4.PostEditor();
            myGridView4.UpdateCurrentRow();

            if (myGridControl4.DataSource == null)
            {
                MsgBox.ShowOK("没有提取短信电话信息,不能保存!");
                return;
            }
            DataRow[] dr = (myGridControl4.DataSource as DataTable).Select("project='公司签名'");

            bool check = true;
            if (dr.Length == 0 || dr[0]["telephone"] == DBNull.Value || dr[0]["telephone"].ToString() == "")
            {
                check = false;
            }
            if (dr.Length > 0)
            {
                string content = dr[0]["telephone"] == DBNull.Value ? "" : dr[0]["telephone"].ToString();
                if (content != "")
                {
                    int p1 = 0, p2 = 0;
                    p1 = content.IndexOf("【");
                    p2 = content.IndexOf("】");
                    if (p1 > p2 || p1 < 0 || p2 < 0)
                    {
                        check = false;
                    }
                    if (content.Replace("【", "").Length < content.Length - 1 || content.Replace("】", "").Length < content.Length - 1)//包含多个大括号
                    {
                        check = false;
                    }
                }
                else
                { check = false; }
            }
            if (check == false)
            {
                MsgBox.ShowOK("没有为短信定义公司签名!\r\n请注意：公司签名必须加黑括号!\r\n比如公司名称是“广州市XX物流有限公司”，公司签名为：【XX物流】");
                myGridView4.FocusedRowHandle = myGridView4.RowCount - 1;
                return;
            }
            try
            {
                //SqlCommand Cmd = new SqlCommand("USP_ADD_MSG_TEL", Conn);
                //Cmd.CommandType = CommandType.StoredProcedure;

                //Cmd.Parameters.Add("@site", SqlDbType.VarChar);
                //Cmd.Parameters.Add("@xm", SqlDbType.VarChar);
                //Cmd.Parameters.Add("@tel", SqlDbType.VarChar);
                //cc.SqlParameterAddCompanyID(Cmd);

                //Conn.Open();
                //for (int i = 0; i < myGridView4.RowCount; i++)
                //{
                //    Cmd.Parameters["@site"].Value = myGridView4.GetRowCellValue(i, "site").ToString();
                //    Cmd.Parameters["@xm"].Value = myGridView4.GetRowCellValue(i, "xm").ToString();
                //    Cmd.Parameters["@tel"].Value = myGridView4.GetRowCellValue(i, "tel").ToString();

                //    Cmd.ExecuteNonQuery();
                //}
                //Conn.Close();


                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_basSmsTel");
                for (int i = 0; i < gridView3.RowCount; i++)
                {
                    list.Add(new SqlPara("InfoId", gridView3.GetRowCellValue(i, "InfoId") == DBNull.Value ? Guid.NewGuid().ToString() : gridView3.GetRowCellValue(i, "InfoId").ToString()));
                    list.Add(new SqlPara("site", gridView3.GetRowCellValue(i, "site").ToString().Trim()));
                    list.Add(new SqlPara("project", gridView3.GetRowCellValue(i, "project").ToString().Trim()));
                    list.Add(new SqlPara("telephone", gridView3.GetRowCellValue(i, "telephone").ToString().Trim()));
                    sps.ParaList = list;
                    SqlHelper.ExecteNonQuery(sps);
                }

                MsgBox.ShowOK("保存成功!资料设置完毕后请重新登录软件!");
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void simpleButton57_Click(object sender, EventArgs e)
        {
            GridOper.ExportToExcel(myGridView4);
            //cc.ExportToExcel(myGridView4);
        }
        #endregion

        private void myGridView3_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            int rowhandle = gridView3.FocusedRowHandle;
            if (rowhandle < 0) return;
            string xm = gridView3.GetRowCellValue(rowhandle, "project").ToString();
            foreach (Control Con in groupControl1.Controls)
            {
                Con.Enabled = false;
            }
            if (xm == sms.SmsXM.开单.ToString())
            {
                #region 开单
                simpleButton44.Enabled = true; //发货人
                simpleButton43.Enabled = true; //收货人
                simpleButton42.Enabled = true; //发站
                simpleButton41.Enabled = true; //到站
                simpleButton40.Enabled = true; //中转地
                simpleButton39.Enabled = true; //运单号
                simpleButton38.Enabled = true; //货号
                simpleButton37.Enabled = true; //品名
                simpleButton36.Enabled = true; //件数
                simpleButton34.Enabled = true; //代收货款 给收货人的时候使用
                simpleButton50.Enabled = true; //提付 给收货人的时候使用
                simpleButton72.Enabled = true; //开单网点
                simpleButton33.Enabled = true; //公司名称

                simpleButton22.Enabled = true; //开单日期
                simpleButton23.Enabled = true; //年
                simpleButton24.Enabled = true; //月
                simpleButton25.Enabled = true; //日
                simpleButton45.Enabled = true; //时
                simpleButton46.Enabled = true; //分

                simpleButton29.Enabled = true; //当前日期
                simpleButton28.Enabled = true; //年
                simpleButton27.Enabled = true; //月
                simpleButton26.Enabled = true; //日
                simpleButton48.Enabled = true; //时
                simpleButton47.Enabled = true; //分

                simpleButton60.Enabled = true;//开单查询电话
                simpleButton94.Enabled = true;//公司签名
                #endregion
            }
            if (xm == sms.SmsXM.配载完成.ToString())
            {
                #region 配载完成
                simpleButton44.Enabled = true; //发货人
                simpleButton43.Enabled = true; //收货人
                simpleButton42.Enabled = true; //发站
                simpleButton41.Enabled = true; //到站
                simpleButton40.Enabled = true; //中转地
                simpleButton39.Enabled = true; //运单号
                simpleButton38.Enabled = true; //货号
                simpleButton37.Enabled = true; //品名
                simpleButton36.Enabled = true; //件数
                simpleButton34.Enabled = true; //代收货款
                simpleButton50.Enabled = true; //提付
                simpleButton72.Enabled = true; //开单网点
                simpleButton33.Enabled = true; //公司名称

                simpleButton22.Enabled = true; //开单日期
                simpleButton23.Enabled = true; //年
                simpleButton24.Enabled = true; //月
                simpleButton25.Enabled = true; //日
                simpleButton45.Enabled = true; //时
                simpleButton46.Enabled = true; //分

                simpleButton29.Enabled = true; //当前日期
                simpleButton28.Enabled = true; //年
                simpleButton27.Enabled = true; //月
                simpleButton26.Enabled = true; //日
                simpleButton48.Enabled = true; //时
                simpleButton47.Enabled = true; //分

                simpleButton76.Enabled = true; //配载日期
                simpleButton75.Enabled = true;//年
                simpleButton74.Enabled = true;//月
                simpleButton73.Enabled = true;//日
                simpleButton35.Enabled = true;//时
                simpleButton32.Enabled = true;//分

                simpleButton83.Enabled = true;//货到前付款提示
                simpleButton61.Enabled = true; //配载完成查询电话
                simpleButton94.Enabled = true;//公司签名
                if (CommonClass.smsCompanyName.Contains("广明"))
                {
                    simpleButton31.Enabled = true; //站点地址 
                }
                #endregion
            }
            if (xm == sms.SmsXM.在途跟踪.ToString())
            {
                #region
                simpleButton44.Enabled = true; //发货人
                simpleButton43.Enabled = true; //收货人
                simpleButton42.Enabled = true; //发站
                simpleButton41.Enabled = true; //到站
                simpleButton40.Enabled = true; //中转地
                simpleButton39.Enabled = true; //运单号
                simpleButton38.Enabled = true; //货号
                simpleButton37.Enabled = true; //品名
                simpleButton36.Enabled = true; //件数
                simpleButton34.Enabled = true; //代收货款
                simpleButton50.Enabled = true; //提付
                simpleButton72.Enabled = true; //开单网点
                simpleButton33.Enabled = true; //公司名称

                simpleButton22.Enabled = true; //开单日期
                simpleButton23.Enabled = true; //年
                simpleButton24.Enabled = true; //月
                simpleButton25.Enabled = true; //日
                simpleButton45.Enabled = true; //时
                simpleButton46.Enabled = true; //分

                simpleButton29.Enabled = true; //当前日期
                simpleButton28.Enabled = true; //年
                simpleButton27.Enabled = true; //月
                simpleButton26.Enabled = true; //日
                simpleButton48.Enabled = true; //时
                simpleButton47.Enabled = true; //分

                simpleButton62.Enabled = true; //专线跟踪查询电话
                simpleButton84.Enabled = true; //专线和三方跟踪的时候，操作员填写的跟踪内容
                simpleButton94.Enabled = true;//公司签名
                #endregion
            }
            if (xm == sms.SmsXM.中转完成.ToString())
            {
                #region
                simpleButton44.Enabled = true; //发货人
                simpleButton43.Enabled = true; //收货人
                simpleButton42.Enabled = true; //发站
                simpleButton41.Enabled = true; //到站
                //simpleButton40.Enabled = true; //中转地
                simpleButton39.Enabled = true; //运单号
                simpleButton38.Enabled = true; //货号
                simpleButton37.Enabled = true; //品名
                simpleButton36.Enabled = true; //件数
                //simpleButton34.Enabled = true; //代收货款
                simpleButton50.Enabled = true; //提付
                simpleButton72.Enabled = true; //开单网点
                simpleButton33.Enabled = true; //公司名称

                simpleButton22.Enabled = true; //开单日期
                simpleButton23.Enabled = true; //年
                simpleButton24.Enabled = true; //月
                simpleButton25.Enabled = true; //日
                simpleButton45.Enabled = true; //时
                simpleButton46.Enabled = true; //分

                simpleButton29.Enabled = true; //当前日期
                simpleButton28.Enabled = true; //年
                simpleButton27.Enabled = true; //月
                simpleButton26.Enabled = true; //日
                simpleButton48.Enabled = true; //时
                simpleButton47.Enabled = true; //分

                simpleButton30.Enabled = true; //中转公司
                simpleButton11.Enabled = true;
                simpleButton20.Enabled = true;
                simpleButton21.Enabled = true;
                simpleButton49.Enabled = true;
                simpleButton89.Enabled = true;
                simpleButton88.Enabled = true;
                simpleButton87.Enabled = true;
                simpleButton86.Enabled = true;
                simpleButton85.Enabled = true;

                simpleButton63.Enabled = true; //中转完成查询电话
                simpleButton94.Enabled = true;//公司签名
                #endregion
            }
            if (xm == sms.SmsXM.中转跟踪.ToString())
            {
                #region
                simpleButton44.Enabled = true; //发货人
                simpleButton43.Enabled = true; //收货人
                simpleButton42.Enabled = true; //发站
                simpleButton41.Enabled = true; //到站
                simpleButton39.Enabled = true; //运单号
                simpleButton38.Enabled = true; //货号
                simpleButton37.Enabled = true; //品名
                simpleButton36.Enabled = true; //件数
                simpleButton50.Enabled = true; //提付
                simpleButton33.Enabled = true; //公司名称

                simpleButton22.Enabled = true; //开单日期
                simpleButton23.Enabled = true; //年
                simpleButton24.Enabled = true; //月
                simpleButton25.Enabled = true; //日
                simpleButton45.Enabled = true; //时
                simpleButton46.Enabled = true; //分

                simpleButton29.Enabled = true; //当前日期
                simpleButton28.Enabled = true; //年
                simpleButton27.Enabled = true; //月
                simpleButton26.Enabled = true; //日
                simpleButton48.Enabled = true; //时
                simpleButton47.Enabled = true; //分

                simpleButton11.Enabled = true;//中转单号
                simpleButton20.Enabled = true;//本地电话
                simpleButton21.Enabled = true;//到站电话

                simpleButton64.Enabled = true; //中转跟踪查询电话
                simpleButton84.Enabled = true; //专线和三方跟踪的时候，操作员填写的跟踪内容
                simpleButton94.Enabled = true;//公司签名
                #endregion
            }
            if (xm == sms.SmsXM.到货通知自提.ToString() || xm == sms.SmsXM.到货通知送货.ToString() || xm == sms.SmsXM.到货等通知放货.ToString())
            {
                #region
                simpleButton44.Enabled = true; //发货人
                simpleButton43.Enabled = true; //收货人
                simpleButton42.Enabled = true; //发站
                simpleButton41.Enabled = true; //到站
                simpleButton39.Enabled = true; //运单号
                simpleButton38.Enabled = true; //货号
                simpleButton37.Enabled = true; //品名
                simpleButton36.Enabled = true; //件数
                simpleButton33.Enabled = true; //公司名称
                simpleButton31.Enabled = true; //站点地址

                simpleButton22.Enabled = true; //开单日期
                simpleButton23.Enabled = true; //年
                simpleButton24.Enabled = true; //月
                simpleButton25.Enabled = true; //日
                simpleButton45.Enabled = true; //时
                simpleButton46.Enabled = true; //分

                simpleButton29.Enabled = true; //当前日期
                simpleButton28.Enabled = true; //年
                simpleButton27.Enabled = true; //月
                simpleButton26.Enabled = true; //日
                simpleButton48.Enabled = true; //时
                simpleButton47.Enabled = true; //分

                simpleButton65.Enabled = true; //到货通知联系电话
                simpleButton90.Enabled = true; //登录站点
                simpleButton91.Enabled = true; //(如果有提付和代收) 运费提示
                simpleButton94.Enabled = true;//公司签名
                #endregion
            }
            if (xm == sms.SmsXM.提货签收.ToString())
            {
                #region 提货
                simpleButton44.Enabled = true; //发货人
                simpleButton43.Enabled = true; //收货人
                simpleButton42.Enabled = true; //发站
                simpleButton41.Enabled = true; //到站
                simpleButton40.Enabled = true; //中转地
                simpleButton39.Enabled = true; //运单号
                simpleButton38.Enabled = true; //货号
                simpleButton37.Enabled = true; //品名
                simpleButton36.Enabled = true; //件数
                simpleButton33.Enabled = true; //公司名称

                simpleButton22.Enabled = true; //开单日期
                simpleButton23.Enabled = true; //年
                simpleButton24.Enabled = true; //月
                simpleButton25.Enabled = true; //日
                simpleButton45.Enabled = true; //时
                simpleButton46.Enabled = true; //分

                simpleButton29.Enabled = true; //当前日期
                simpleButton28.Enabled = true; //年
                simpleButton27.Enabled = true; //月
                simpleButton26.Enabled = true; //日
                simpleButton48.Enabled = true; //时
                simpleButton47.Enabled = true; //分

                simpleButton66.Enabled = true;//完成提货电话
                simpleButton82.Enabled = true;//提货日期
                simpleButton81.Enabled = true;
                simpleButton80.Enabled = true;
                simpleButton79.Enabled = true;
                simpleButton78.Enabled = true;
                simpleButton77.Enabled = true;
                simpleButton92.Enabled = true;//提货人
                simpleButton94.Enabled = true;//公司签名
                #endregion
            }
            if (xm == sms.SmsXM.送货完成.ToString())
            {
                #region 送货
                simpleButton44.Enabled = true; //发货人
                simpleButton43.Enabled = true; //收货人
                simpleButton42.Enabled = true; //发站
                simpleButton41.Enabled = true; //到站
                simpleButton40.Enabled = true; //中转地
                simpleButton39.Enabled = true; //运单号
                simpleButton38.Enabled = true; //货号
                simpleButton37.Enabled = true; //品名
                simpleButton36.Enabled = true; //件数

                simpleButton22.Enabled = true; //开单日期
                simpleButton23.Enabled = true; //年
                simpleButton24.Enabled = true; //月
                simpleButton25.Enabled = true; //日
                simpleButton45.Enabled = true; //时
                simpleButton46.Enabled = true; //分

                simpleButton29.Enabled = true; //当前日期
                simpleButton28.Enabled = true; //年
                simpleButton27.Enabled = true; //月
                simpleButton26.Enabled = true; //日
                simpleButton48.Enabled = true; //时
                simpleButton47.Enabled = true; //分

                simpleButton67.Enabled = true;//完成送货电话
                simpleButton51.Enabled = true;//提货日期
                simpleButton56.Enabled = true;
                simpleButton55.Enabled = true;
                simpleButton54.Enabled = true;
                simpleButton53.Enabled = true;
                simpleButton52.Enabled = true;
                simpleButton94.Enabled = true;//公司签名

                simpleButton97.Enabled = true; //送货司机
                simpleButton98.Enabled = true; //司机电话
                #endregion
            }
            if (xm == sms.SmsXM.送货签收.ToString())
            {
                #region 送货签收
                simpleButton44.Enabled = true; //发货人
                simpleButton43.Enabled = true; //收货人
                simpleButton42.Enabled = true; //发站
                simpleButton41.Enabled = true; //到站
                simpleButton40.Enabled = true; //中转地
                simpleButton39.Enabled = true; //运单号
                simpleButton38.Enabled = true; //货号
                simpleButton37.Enabled = true; //品名
                simpleButton36.Enabled = true; //件数

                simpleButton22.Enabled = true; //开单日期
                simpleButton23.Enabled = true; //年
                simpleButton24.Enabled = true; //月
                simpleButton25.Enabled = true; //日
                simpleButton45.Enabled = true; //时
                simpleButton46.Enabled = true; //分

                simpleButton29.Enabled = true; //当前日期
                simpleButton28.Enabled = true; //年
                simpleButton27.Enabled = true; //月
                simpleButton26.Enabled = true; //日
                simpleButton48.Enabled = true; //时
                simpleButton47.Enabled = true; //分

                simpleButton93.Enabled = true;//送货签收日期
                simpleButton68.Enabled = true;//完成送货签收电话
                simpleButton94.Enabled = true;//公司签名
                simpleButton99.Enabled = true;//送货签收人
                #endregion
            }

            if (xm == sms.SmsXM.回单寄出.ToString())
            {
                #region 回单寄出
                simpleButton44.Enabled = true; //发货人
                simpleButton43.Enabled = true; //收货人
                simpleButton42.Enabled = true; //发站
                simpleButton41.Enabled = true; //到站
                simpleButton40.Enabled = true; //中转地
                simpleButton39.Enabled = true; //运单号
                simpleButton38.Enabled = true; //货号
                simpleButton37.Enabled = true; //品名
                simpleButton36.Enabled = true; //件数

                simpleButton22.Enabled = true; //开单日期
                simpleButton23.Enabled = true; //年
                simpleButton24.Enabled = true; //月
                simpleButton25.Enabled = true; //日
                simpleButton45.Enabled = true; //时
                simpleButton46.Enabled = true; //分

                simpleButton29.Enabled = true; //当前日期
                simpleButton28.Enabled = true; //年
                simpleButton27.Enabled = true; //月
                simpleButton26.Enabled = true; //日
                simpleButton48.Enabled = true; //时
                simpleButton47.Enabled = true; //分

                simpleButton70.Enabled = true;//回单寄出查询电话
                simpleButton94.Enabled = true;//公司签名
                #endregion
            }
            if (xm == sms.SmsXM.回单返回.ToString())
            {
                #region 回单返回
                simpleButton44.Enabled = true; //发货人
                simpleButton43.Enabled = true; //收货人
                simpleButton42.Enabled = true; //发站
                simpleButton41.Enabled = true; //到站
                simpleButton40.Enabled = true; //中转地
                simpleButton39.Enabled = true; //运单号
                simpleButton38.Enabled = true; //货号
                simpleButton37.Enabled = true; //品名
                simpleButton36.Enabled = true; //件数

                simpleButton22.Enabled = true; //开单日期
                simpleButton23.Enabled = true; //年
                simpleButton24.Enabled = true; //月
                simpleButton25.Enabled = true; //日
                simpleButton45.Enabled = true; //时
                simpleButton46.Enabled = true; //分

                simpleButton29.Enabled = true; //当前日期
                simpleButton28.Enabled = true; //年
                simpleButton27.Enabled = true; //月
                simpleButton26.Enabled = true; //日
                simpleButton48.Enabled = true; //时
                simpleButton47.Enabled = true; //分

                simpleButton71.Enabled = true;//回单返回查询电话
                simpleButton94.Enabled = true;//公司签名
                #endregion
            }
            if (xm == sms.SmsXM.货款回收.ToString() || xm == sms.SmsXM.货款到账.ToString() || xm == sms.SmsXM.货款发放.ToString())
            {
                #region 货款
                simpleButton44.Enabled = true; //发货人
                simpleButton43.Enabled = true; //收货人
                simpleButton42.Enabled = true; //发站
                simpleButton41.Enabled = true; //到站
                simpleButton39.Enabled = true; //运单号
                simpleButton38.Enabled = true; //货号
                simpleButton37.Enabled = true; //品名
                simpleButton36.Enabled = true; //件数
                simpleButton34.Enabled = true; //代收货款

                simpleButton22.Enabled = true; //开单日期
                simpleButton23.Enabled = true; //年
                simpleButton24.Enabled = true; //月
                simpleButton25.Enabled = true; //日
                simpleButton45.Enabled = true; //时
                simpleButton46.Enabled = true; //分

                simpleButton29.Enabled = true; //当前日期
                simpleButton28.Enabled = true; //年
                simpleButton27.Enabled = true; //月
                simpleButton26.Enabled = true; //日
                simpleButton48.Enabled = true; //时
                simpleButton47.Enabled = true; //分

                simpleButton69.Enabled = true; //货款到账电话
                simpleButton95.Enabled = true; //货款回收电话
                simpleButton96.Enabled = true; //货款发放电话

                simpleButton94.Enabled = true;//公司签名
                #endregion
            }
            //simpleButton.Enabled = true;
        }
    }
}