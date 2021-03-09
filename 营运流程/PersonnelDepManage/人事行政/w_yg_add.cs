using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Threading;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using Newtonsoft.Json;
using System.Net;

namespace ZQTMS.UI
{
    public partial class w_yg_add : BaseForm
    {

        public string bh = "";
        private int picIndex = 1;// 表示正在查看的张照片
        private int uploadIndex = -1;// 表示正在上传的照片,-1表示上传完成或没有在上传
        WebClient wc = new WebClient();

        public w_yg_add()
        {
            InitializeComponent();
        }

        private void w_yg_add_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            BarMagagerOper.SetBarPropertity(bar1);
            barEditItem1.EditValue = barEditItem2.EditValue = 100;
            //cc.FillRepCombox(edfgs);
            CommonClass.SetSite(edfgs,false);
            edfgs.EditValue =CommonClass.UserInfo.SiteName;
            clear();
            if (bh != "")
            {
                //SqlConnection Conn = cc.GetConn();
                try
                {
                    //SqlCommand da = new SqlCommand("QSP_GET_E_YGDA_BH");
                    //da.CommandType = CommandType.StoredProcedure;
                    //da.Parameters.Add(new SqlParameter("@bh", SqlDbType.VarChar));
                    //da.Parameters[0].Value = bh;
                    //DataSet ds = new DataSet();
                    //ds = cs.GetDataSet(da, null); 
                    List<SqlPara> list= new List<SqlPara>();
                    list.Add(new SqlPara("bh", bh));
                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_E_YGDA_BH", list);
                    DataSet ds = SqlHelper.GetDataSet(sps);
                    
                    if (ds == null) return;

                    if (ds.Tables[0].Rows.Count == 0) return;
                    DataRow dr = ds.Tables[0].Rows[0];
                    edbh.Text = bh;
                    edxm.EditValue = dr["xm"];
                    edjrbdwrq.EditValue = dr["jrbdwrq"];
                    edfgs.EditValue = dr["fgs"];
                    edbm.EditValue = dr["bm"];
                    edsex.EditValue = dr["sex"];
                    edzw.EditValue = dr["zw"];
                    edhf.EditValue = dr["hf"];
                    edbirthday.EditValue = dr["birthday"];
                    edhk.EditValue = dr["hk"];
                    edmz.EditValue = dr["mz"];
                    edbdwgl.EditValue = dr["bdwgl"];
                    edsfz.EditValue = dr["sfz"];
                    edbchtqdrq.EditValue = dr["bchtqdrq"];
                    edbchtzzrq.EditValue = dr["bchtzzrq"];
                    edjg.EditValue = dr["jg"];
                    edsb.EditValue = dr["sb"];
                    edsl.EditValue = dr["sl"];
                    edxx.EditValue = dr["xx"];
                    edtz.EditValue = dr["tz"];
                    edsg.EditValue = dr["sg"];
                    edkhyh.EditValue = dr["khyh"];
                    edyhzh.EditValue = dr["yhzh"];
                    edjz.EditValue = dr["jz"];
                    edyx.EditValue = dr["yx"];
                    edxl.EditValue = dr["xl"];
                    edzy.EditValue = dr["zy"];
                    edaddr.EditValue = dr["addr"];
                    edtel.EditValue = dr["tel"];
                    edzz.EditValue = dr["zz"];
                    edemail.EditValue = dr["email"];
                    edqq.EditValue = dr["qq"];
                    edmsn.EditValue = dr["msn"];
                    edyysp.EditValue = dr["yysp"];
                    eddnsp.EditValue = dr["dnsp"];
                    edgl.EditValue = dr["gl"];
                    edgzrq.EditValue = dr["gzrq"];
                    edstate.EditValue = dr["state"];
                    edremark.EditValue = dr["remark"];
                    edmm.EditValue = dr["mm"];

                    //jinjilianxi.EditValue = dr["jinjilianxi"];
                    //bankname.EditValue = dr["bankname"];
                    //bankcode.EditValue = dr["bankcode"];
                    //edruzhu.Text = ConvertType.ToString(dr["ruzhu"]);
                    //edruzhudate.EditValue = dr["ruzhudate"];

                    //if (ConvertType.ToInt32(dr["ispic"]) == 1)
                    //    buttonEdit1.Text = "已上传照片!";

                    //if (ConvertType.ToInt32(dr["ispic2"]) == 1)
                    //    buttonEdit2.Text = "已上传照片!";

                    //if (ConvertType.ToInt32(dr["ispic3"]) == 1)
                    //    buttonEdit3.Text = "已上传照片!";

                    //string worktime = dr["worktime"].ToString();
                    //try
                    //{
                    //    string[] worktimes = worktime.Split('-');
                    //    t1.Text = worktimes[0];
                    //    t2.Text = worktimes[1];
                    //}
                    //catch (Exception) { }
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    edbh.Properties.ReadOnly = true;
                    //Conn.Close();
                }
            }

            getdata();
        }

        private void getdata()
        {
            ////SqlConnection con = cc.GetConn();
            //try
            //{
            //    SqlCommand sq = new SqlCommand("QSP_GET_B_ZW");
            //    sq.CommandType = CommandType.StoredProcedure;
            //    sq.Parameters.Add("@bsiteName ", SqlDbType.VarChar).Value = commonclass.username;
            //    DataSet ds = new DataSet();
            //    ds = cs.GetDataSet(sq,null);
            //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //    {
            //        string xm = ds.Tables[0].Rows[i][0].ToString();
            //        edzw.Properties.Items.Add(xm);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    commonclass.MsgBox.ShowOK(ex.Message);
            //}
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            w_select_department wsd = new w_select_department();
            if (wsd.ShowDialog() == DialogResult.OK)
            {
                edfgs.Text = CommonClass.UserInfo.DepartName;
                edbm.Text = CommonClass.UserInfo.DepartName;
            }
        }

        private void GetDetail(string bh, string cmd, DevExpress.XtraGrid.GridControl gc)
        {
            //SqlConnection connection = cc.GetConn();
            try
            {
                ////if (connection.State == ConnectionState.Closed) connection.Open();
                //DataSet dataset = new DataSet();
                //SqlDataAdapter da = new SqlDataAdapter();
                //SqlCommand sq = new SqlCommand(cmd);
                //sq.Parameters.Add(new SqlParameter("@bh", SqlDbType.VarChar));
                //sq.Parameters.Add("@t1", SqlDbType.DateTime).Value = Convert.ToDateTime("2010-1-1 00:00:00");
                //sq.Parameters.Add("@t2", SqlDbType.DateTime).Value = Convert.ToDateTime("9999-12-30 23:59:59");
                //sq.Parameters[0].Value = bh;
                //sq.CommandType = CommandType.StoredProcedure;
                ////cs.ENQ(sq);
                //dataset.Clear();
                //dataset = cs.GetDataSet(sq, gc);
                ////if (connection.State == ConnectionState.Open) connection.Close();

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bh", bh));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, cmd, list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count > 0 || ds.Tables[0].Rows.Count == 0) return;
                gc.DataSource = ds.Tables[0];
                gc.DataMember = ds.Tables[0].ToString();

            }
            catch (Exception ex)
            {
                //if (connection.State == ConnectionState.Open) connection.Close();
                //XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (bh == "") return;
            if (xtraTabControl1.SelectedTabPage == xtraTabPage3)  //工作经历
                GetDetail(bh, "QSP_GET_YG_WORK", gridControl3);

            if (xtraTabControl1.SelectedTabPage == xtraTabPage2)  //调动情况
                GetDetail(bh, "QSP_GET_YG_TRUN", gridControl1);

            if (xtraTabControl1.SelectedTabPage == xtraTabPage4)  //奖惩
                GetDetail(bh, "QSP_GET_YG_JC", gridControl2);

            if (xtraTabControl1.SelectedTabPage == xtraTabPage7)  //请假
                GetDetail(bh, "QSP_GET_YG_QJ", gridControl4);

            if (xtraTabControl1.SelectedTabPage == xtraTabPage8)  //工伤
                GetDetail(bh, "QSP_GET_YG_GS", gridControl5);

            if (xtraTabControl1.SelectedTabPage == xtraTabPage5)  //培训
                GetDetail(bh, "QSP_GET_YG_PX", gridControl6);


            if (xtraTabControl1.SelectedTabPage == xtraTabPage6)  //考评
                GetDetail(bh, "QSP_GET_YG_KP", gridControl7);
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            clear();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (edbh.Text.Trim() == "" || edxm.Text.Trim() == "")
            {
                XtraMessageBox.Show("请输入编号和姓名!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                edbh.Focus();
                return;
            }
            if (edjrbdwrq.Text.Trim() == "")
            {
                XtraMessageBox.Show("请选择入职日期!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                edjrbdwrq.Focus();
                return;
            }
            if (edbirthday.Text.Trim() == "")
            {
                XtraMessageBox.Show("请选择出生日期!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                edbirthday.Focus();
                return;
            }
            if (edbchtqdrq.Text.Trim() == "")
            {
                XtraMessageBox.Show("请选择合同签订日期!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                edbchtqdrq.Focus();
                return;
            }
            if (edbchtzzrq.Text.Trim() == "")
            {
                XtraMessageBox.Show("请选择合同签订日期!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                edbchtzzrq.Focus();
                return;
            }

            //SqlConnection Conn = cc.GetConn();
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("id",bh == "" ? 0 : 1));
                list.Add(new SqlPara("bh",edbh.Text.Trim()));
                list.Add(new SqlPara("xm",edxm.Text.Trim()));
                list.Add(new SqlPara("jrbdwrq",edjrbdwrq.Text.Trim()));
                list.Add(new SqlPara("fgs",edfgs.Text.Trim()));
                list.Add(new SqlPara("bm",edbm.Text.Trim()));
                list.Add(new SqlPara("sex",edsex.Text.Trim()));
                list.Add(new SqlPara("zw",edzw.Text.Trim()));
                list.Add(new SqlPara("hf",edhf.Text.Trim()));
                list.Add(new SqlPara("birthday",edbirthday.DateTime));
                list.Add(new SqlPara("hk", edhk.Text.Trim()));
                list.Add(new SqlPara("mz",edmz.Text.Trim()));
                list.Add(new SqlPara("bdwgl",edbdwgl.Text.Trim()));
                list.Add(new SqlPara("sfz",edsfz.Text.Trim()));
                list.Add(new SqlPara("bchtqdrq",edbchtzzrq.DateTime));
                list.Add(new SqlPara("bchtzzrq",edbchtzzrq.DateTime));
                list.Add(new SqlPara("jg",edjg.Text.Trim()));
                list.Add(new SqlPara("sb",edsb.Text.Trim()));
                list.Add(new SqlPara("sl",edsl.Text.Trim()));
                list.Add(new SqlPara("xx",edxx.Text.Trim()));
                list.Add(new SqlPara("tz",edtz.Text.Trim()));
                list.Add(new SqlPara("sg",edsg.Text.Trim()));
                list.Add(new SqlPara("khyh",edkhyh.Text.Trim()));
                list.Add(new SqlPara("yhzh",edyhzh.Text.Trim()));
                list.Add(new SqlPara("jz",edjz.Text.Trim()));
                list.Add(new SqlPara("yx",edyx.Text.Trim()));
                list.Add(new SqlPara("xl",edxl.Text.Trim()));
                list.Add(new SqlPara("zy",edzy.Text.Trim()));
                list.Add(new SqlPara("addr",edaddr.Text.Trim()));
                list.Add(new SqlPara("tel",edtel.Text.Trim()));
                list.Add(new SqlPara("zz",edzz.Text.Trim()));
                list.Add(new SqlPara("email",edemail.Text.Trim()));
                list.Add(new SqlPara("qq",edqq.Text.Trim()));
                list.Add(new SqlPara("msn",edmsn.Text.Trim()));
                list.Add(new SqlPara("yysp", edyysp.Text.Trim()));
                list.Add(new SqlPara("dnsp",eddnsp.Text.Trim()));
                list.Add(new SqlPara("gl", edgl.Text.Trim()));


                if (edstate.Text.Trim().Contains("解聘") || edstate.Text.Trim().Contains("辞职") || edstate.Text.Trim().Contains("自动离职"))
                {
                    if (edgzrq.Text.Trim() == "")
                    {
                        XtraMessageBox.Show("请选择离职日期", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        edgzrq.Focus();
                        return;
                    }
                    list.Add(new SqlPara("gzrq", edgzrq.DateTime));
                    
                }
                else
                {
                   list.Add(new SqlPara("gzrq",  DBNull.Value));
                }
                list.Add(new SqlPara("state", edstate.Text.Trim()));
                list.Add(new SqlPara("mm", edmm.Text.Trim()));
                list.Add(new SqlPara("remark", edremark.Text.Trim()));



                //后补增加字段
                //list.Add(new SqlPara("jinjilianxi", edstate.Text.Trim()));
                //list.Add(new SqlPara("bankname", edmm.Text.Trim()));
                //list.Add(new SqlPara("bankcode", edremark.Text.Trim()));
                //list.Add(new SqlPara("ruzhu", edremark.Text.Trim()));


                //Cmd.Parameters.Add(new SqlParameter("@remark", SqlDbType.VarChar)).Value = edremark.Text.Trim();
                //if (edruzhudate.Text.Trim() == "")
                //    list.Add(new SqlPara("ruzhudate", DBNull.Value));
                //else
                //    list.Add(new SqlPara("ruzhudate", edruzhudate.DateTime));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_YG_DA", list);
                SqlHelper.ExecteNonQuery(sps);
                MsgBox.ShowOK();
                edbh.Properties.ReadOnly = false;
                bh = "";
                clear();

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //if (Conn.State == ConnectionState.Open) Conn.Close();
            }
        }

        private string update()
        {
            string sql = "update b_ygda set ";
            sql += "xm='" + edxm.Text.Trim() + "',";
            sql += "jrbdwrq='" + edjrbdwrq.DateTime + "',";
            sql += "fgs='" + edfgs.Text.Trim() + "',";
            sql += "bm='" + edbm.Text.Trim() + "',";
            sql += "sex='" + edsex.Text.Trim() + "',";

            sql += "zw='" + edzw.Text.Trim() + "',";
            sql += "hf='" + edhf.Text.Trim() + "',";
            sql += "birthday='" + edbirthday.DateTime + "',";
            sql += "hk='" + edhk.Text.Trim() + "',";
            sql += "mz='" + edmz.Text.Trim() + "',";

            sql += "bdwgl='" + edbdwgl.Text.Trim() + "',";
            sql += "sfz='" + edsfz.Text.Trim() + "',";
            sql += "bchtqdrq='" + edbchtqdrq.DateTime + "',";
            sql += "bchtzzrq='" + edbchtzzrq.DateTime + "',";

            sql += "jg='" + edjg.Text.Trim() + "',";

            sql += "sb='" + edsb.Text.Trim() + "',";
            sql += "sl='" + edsl.Text.Trim() + "',";
            sql += "xx='" + edxx.Text.Trim() + "',";
            sql += "tz='" + edtz.Text.Trim() + "',";
            sql += "sg='" + edsg.Text.Trim() + "',";

            sql += "khyh='" + edkhyh.Text.Trim() + "',";
            sql += "yhzh='" + edyhzh.Text.Trim() + "',";
            sql += "jz='" + edjz.Text.Trim() + "',";
            sql += "yx='" + edyx.Text.Trim() + "',";
            sql += "xl='" + edxl.Text.Trim() + "',";

            sql += "zy='" + edzy.Text.Trim() + "',";
            sql += "addr='" + edaddr.Text.Trim() + "',";
            sql += "tel='" + edtel.Text.Trim() + "',";
            sql += "zz='" + edzz.Text.Trim() + "',";
            sql += "email='" + edemail.Text.Trim() + "',";

            sql += "qq='" + edqq.Text.Trim() + "',";
            sql += "msn='" + edmsn.Text.Trim() + "',";
            sql += "yysp='" + edyysp.Text.Trim() + "',";
            sql += "dnsp='" + eddnsp.Text.Trim() + "',";
            sql += "gl='" + edgl.Text.Trim() + "',";

            if (edstate.Text.Trim().Contains("解聘") || edstate.Text.Trim().Contains("辞职") || edstate.Text.Trim().Contains("自动离职"))
            {
                if (edgzrq.Text.Trim() == "")
                {
                    XtraMessageBox.Show("请选择离职日期", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    edgzrq.Focus();
                    return "";
                }
                sql += "gzrq='" + edgzrq.DateTime + "',";
            }
            else
            {
                sql += "gzrq=null,";
            }

            sql += "state='" + edstate.Text.Trim() + "',";
            sql += "remark='" + edremark.Text.Trim() + "',";
            sql += "jinjilianxi='" + jinjilianxi.Text.Trim() + "',";
            sql += "bankname='" + bankname.Text.Trim() + "',";
            sql += "bankcode='" + bankcode.Text.Trim() + "',";
            sql += "worktime='" + (t1.Text.Trim() + "-" + t2.Text.Trim()) + "',";
            sql += "ruzhu='" + edruzhu.Text.Trim() + "',";
            sql += "mm='" + edmm.Text.Trim() + "',";
            if (edruzhudate.Text.Trim() == "")
                sql += "ruzhudate=null";
            else
                sql += "ruzhudate='" + edruzhudate.DateTime + "'";
            sql += " where bh='" + bh + "'";
            return sql;
        }

        private string insert()
        {
            string sql = "insert into b_ygda (bh,xm,jrbdwrq,fgs,bm,sex,zw,hf,birthday,hk,mz,bdwgl,sfz,bchtqdrq,bchtzzrq,jg,sb,";
            sql += "sl,xx,tz,sg,khyh,yhzh,jz,yx,xl,zy,addr,tel,zz,email,qq,msn,yysp,dnsp,gl,gzrq,state,remark,jinjilianxi,bankname,bankcode,worktime,mm,ruzhu,ruzhudate) values (";

            sql += "'" + edbh.Text.Trim() + "',";
            sql += "'" + edxm.Text.Trim() + "',";
            sql += "'" + edjrbdwrq.DateTime + "',";
            sql += "'" + edfgs.Text.Trim() + "',";
            sql += "'" + edbm.Text.Trim() + "',";
            sql += "'" + edsex.Text.Trim() + "',";

            sql += "'" + edzw.Text.Trim() + "',";
            sql += "'" + edhf.Text.Trim() + "',";
            sql += "'" + edbirthday.DateTime + "',";
            sql += "'" + edhk.Text.Trim() + "',";
            sql += "'" + edmz.Text.Trim() + "',";

            sql += "'" + edbdwgl.Text.Trim() + "',";
            sql += "'" + edsfz.Text.Trim() + "',";
            sql += "'" + edbchtqdrq.DateTime + "',";
            sql += "'" + edbchtzzrq.DateTime + "',";

            sql += "'" + edjg.Text.Trim() + "',";

            sql += "'" + edsb.Text.Trim() + "',";
            sql += "'" + edsl.Text.Trim() + "',";
            sql += "'" + edxx.Text.Trim() + "',";
            sql += "'" + edtz.Text.Trim() + "',";
            sql += "'" + edsg.Text.Trim() + "',";

            sql += "'" + edkhyh.Text.Trim() + "',";
            sql += "'" + edyhzh.Text.Trim() + "',";
            sql += "'" + edjz.Text.Trim() + "',";
            sql += "'" + edyx.Text.Trim() + "',";
            sql += "'" + edxl.Text.Trim() + "',";

            sql += "'" + edzy.Text.Trim() + "',";
            sql += "'" + edaddr.Text.Trim() + "',";
            sql += "'" + edtel.Text.Trim() + "',";
            sql += "'" + edzz.Text.Trim() + "',";
            sql += "'" + edemail.Text.Trim() + "',";

            sql += "'" + edqq.Text.Trim() + "',";
            sql += "'" + edmsn.Text.Trim() + "',";
            sql += "'" + edyysp.Text.Trim() + "',";
            sql += "'" + eddnsp.Text.Trim() + "',";
            sql += "'" + edgl.Text.Trim() + "',";

            if (edstate.Text.Trim().Contains("解聘") || edstate.Text.Trim().Contains("辞职") || edstate.Text.Trim().Contains("自动离职"))
            {
                if (edgzrq.Text.Trim() == "")
                {
                    XtraMessageBox.Show("请选择离职日期", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    edgzrq.Focus();
                    return "";
                }
                sql += "'" + edgzrq.DateTime + "',";
            }
            else
            {
                sql += " null, ";
            }
            sql += "'" + edstate.Text.Trim() + "',";

            sql += "'" + edremark.Text.Trim() + "',";

            sql += "'" + jinjilianxi.Text.Trim() + "',";
            sql += "'" + bankname.Text.Trim() + "',";
            sql += "'" + bankcode.Text.Trim() + "',";
            sql += "'" + (t1.Text.Trim() + "-" + t2.Text.Trim()) + "',";
            sql += "'" + edmm.Text.Trim() + "',";
            sql += "'" + edruzhu.Text.Trim() + "',";
            if (edruzhudate.Text.Trim() == "")
                sql += "null,";
            else
                sql += "'" + edruzhudate.DateTime + "')";

            return sql;
        }

        void clear()
        {
            foreach (Control ctl in panelControl1.Controls)
            {
                if (ctl.GetType() == typeof(DevExpress.XtraEditors.TextEdit))
                {
                    ctl.Text = "";
                }
                if (ctl.GetType() == typeof(DevExpress.XtraEditors.DateEdit))
                {
                    ((DevExpress.XtraEditors.DateEdit)ctl).DateTime = DateTime.Now;
                }
            }
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void buttonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Filter = "图片文件(*.bmp;*.jpg;*.jpeg;*.gif;*.png)|*.bmp;*.jpg;*.jpeg;*.gif;*.png";
            dialog.CheckFileExists = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (((dialog.FileName.ToLower().EndsWith("bmp") || dialog.FileName.ToLower().EndsWith("jpg")) || (dialog.FileName.ToLower().EndsWith("jpeg") || dialog.FileName.ToLower().EndsWith("gif"))) || dialog.FileName.ToLower().EndsWith("png"))
                {
                    ((DevExpress.XtraEditors.ButtonEdit)sender).Text = dialog.FileName;
                    return;
                }
                XtraMessageBox.Show("选择的图片格式错误!请重新选择!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            buttonEdit1.Text = "";
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            //if (uploadIndex != -1)
            //{
            //    XtraMessageBox.Show("正在上传照片中...\r\n请稍后再上传!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}
            uploadIndex = 1;
            pictureBox1.Visible = false;
            pictureBox1.Image = null;

            Thread th = new Thread(ThreadUploadPhoto);
            th.IsBackground = true;
            th.Start();
        }

        private void ThreadUploadPhoto()
        {
            switch (uploadIndex)
            {
                case 2:
                    UpLoadFile(buttonEdit2.Text.Trim(), 2);
                    break;
                case 3:
                    UpLoadFile(buttonEdit3.Text.Trim(), 3);
                    break;
                default:
                    UpLoadFile(buttonEdit1.Text.Trim(), 1);
                    break;
            }
        }

        private void uploadPhoto(string path, int picIndex)
        {
            if (bh == "" && edbh.Text == "")
            {
                uploadIndex = -1;
                return;
            }

            if (path == "")
            {
                uploadIndex = -1;
                XtraMessageBox.Show("请先选择照片!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (path == "已上传照片!")
            {
                uploadIndex = -1;
                return;
            }

            if (!File.Exists(path))
            {
                uploadIndex = -1;
                XtraMessageBox.Show("您选择的照片不存在!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            byte[] buffer1 = null;
            SendSmallImage(path, ref buffer1, 1024, 768);
            if (buffer1 == null)
            {
                uploadIndex = -1;
                XtraMessageBox.Show("员工照片一加载失败，请重新选择!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //SqlConnection con = cc.GetConn();
            //SqlCommand cmd = new SqlCommand("USP_ADD_YG_PHOTO_2");
            //cmd.CommandType = CommandType.StoredProcedure;

            //cmd.Parameters.Add("@bh", SqlDbType.VarChar).Value = bh == "" ? edbh.Text.Trim() : bh;
            //cmd.Parameters.Add("@picIndex", SqlDbType.Int).Value = picIndex;
            //cmd.Parameters.Add("@picdate", SqlDbType.DateTime).Value = CommonClass.gcdate;
            //cmd.Parameters.Add("@picman", SqlDbType.VarChar).Value = CommonClass.UserInfo.UserName;
            //cmd.Parameters.Add("@pic", SqlDbType.Image).Value = buffer1;
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("bh",bh == "" ? edbh.Text.Trim() : bh));
            list.Add(new SqlPara("picIndex",picIndex));
            list.Add(new SqlPara("picdate",CommonClass.gcdate));
            list.Add(new SqlPara("picman",CommonClass.UserInfo.UserName));
            list.Add(new SqlPara("pic", buffer1));

            //con.Open();
            try
            {
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_YG_PHOTO_2", list)) > 0)
                {
                    //commonclass.ShowOK();
                    XtraMessageBox.Show("上传完成!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                uploadIndex = -1;
                //con.Close();
            }
        }

        private void SendSmallImage(string fileName, ref byte[] buffer, int maxWidth, int maxHeight)
        {
            Image img = Image.FromFile(fileName);
            ImageFormat thisFormat = img.RawFormat;
            Size newSize = NewSize(maxWidth, maxHeight, img.Width, img.Height);
            Bitmap outBmp = new Bitmap(newSize.Width, newSize.Height);
            Graphics g = Graphics.FromImage(outBmp);
            try
            {
                // 设置画布的描绘质量
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(img, new Rectangle(0, 0, newSize.Width, newSize.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
                g.Dispose();
                // 以下代码为保存图片时,设置压缩质量
                EncoderParameters encoderParams = new EncoderParameters();
                long[] quality = new long[1];
                quality[0] = 100;
                EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                encoderParams.Param[0] = encoderParam;
                //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象.
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICI = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICI = arrayICI[x]; //设置JPEG编码
                        break;
                    }
                }
                MemoryStream stream = new MemoryStream();
                if (jpegICI != null)
                {
                    outBmp.Save(stream, jpegICI, encoderParams);
                }
                else
                {
                    outBmp.Save(stream, thisFormat);
                }
                buffer = stream.ToArray();
                stream.Write(buffer, 0, Convert.ToInt32(stream.Length));
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
            finally
            {
                img.Dispose();
                outBmp.Dispose();
            }
        }

        private Size NewSize(int maxWidth, int maxHeight, int width, int height)
        {
            double w = 0.0;
            double h = 0.0;
            double sw = Convert.ToDouble(width);
            double sh = Convert.ToDouble(height);
            double mw = Convert.ToDouble(maxWidth);
            double mh = Convert.ToDouble(maxHeight);
            if (sw < mw && sh < mh)
            {
                w = sw;
                h = sh;
            }
            else if ((sw / sh) > (mw / mh))
            {
                w = maxWidth;
                h = (w * sh) / sw;
            }
            else
            {
                h = maxHeight;
                w = (h * sw) / sh;
            }
            return new Size(Convert.ToInt32(w), Convert.ToInt32(h));
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            ShowImg(1);
        }

        private void LookPhoto(int picIndex)
        {
            if (pictureBox1.Image != null && this.picIndex == picIndex)
            {
                pictureBox1.Visible = true;
                return;
            }
            this.picIndex = picIndex;

            //SqlConnection con = cc.GetConn();
            //SqlCommand cmd = new SqlCommand("QSP_GET_YG_PHOTO_2");
            //cmd.CommandType = CommandType.StoredProcedure;

            //cmd.Parameters.Add("@bh", SqlDbType.VarChar).Value = bh == "" ? edbh.Text.Trim() : bh;
            //cmd.Parameters.Add("@picIndex", SqlDbType.Int).Value = picIndex;
   
            ////con.Open();
            //DataSet ds_pic = cs.GetDataSet(cmd,null);

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("bh", bh == "" ? edbh.Text.Trim() : bh));
            list.Add(new SqlPara("picIndex",picIndex));
            DataSet ds_pic = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_YG_PHOTO_2", list));
            try
            {


                if (ds_pic.Tables[0].Rows.Count>0)
                {
                    if (ds_pic.Tables[0].Rows[0]["pic"] == DBNull.Value)
                    {
                        //reader.Close();
                        XtraMessageBox.Show("该车辆没有上传照片!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    byte[] buffer = (byte[])ds_pic.Tables[0].Rows[0]["pic"];
                    if (buffer.Length == 0)
                    {
                        //reader.Close();
                        XtraMessageBox.Show("该车辆没有上传照片!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    MemoryStream stream = new MemoryStream();
                    stream.Read(buffer,0,buffer.Length);
                    progressBarControl1.Visible = true;
                    progressBarControl1.Properties.Minimum = 0;
                    progressBarControl1.Properties.Maximum = buffer.Length;
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        stream.WriteByte(buffer[i]);
                        if (i % 1000 == 0)
                        {
                            progressBarControl1.Position = i;
                            progressBarControl1.Refresh();
                        }
                        Application.DoEvents();
                    }
                    Image image = Image.FromStream(stream);

                    pictureBox1.Visible = true;
                    this.pictureBox1.Image = image;
                    pictureBox1.Width = image.Width + 1;//(image.Width > 760 ? 760 : image.Width) + 1;
                    pictureBox1.Height = image.Height + 1; //(image.Height > 550 ? 550 : image.Height) + 1;
                    stream.Close();
                }
                //reader.Close();
            }
            catch (Exception ex)
            {
                pictureBox1.Visible = false;
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //con.Close();
                progressBarControl1.Position = 0;
                progressBarControl1.Visible = false;
            }
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            pictureBox1.Visible = false;
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            //if (uploadIndex != -1)
            //{
            //    XtraMessageBox.Show("正在上传照片中...\r\n请稍后再上传!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}
            uploadIndex = 2;
            pictureBox1.Visible = false;
            pictureBox1.Image = null;

            Thread th = new Thread(ThreadUploadPhoto);
            th.IsBackground = true;
            th.Start();
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            //if (uploadIndex != -1)
            //{
            //    XtraMessageBox.Show("正在上传照片中...\r\n请稍后再上传!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}
            uploadIndex = 3;
            pictureBox1.Visible = false;
            pictureBox1.Image = null;

            Thread th = new Thread(ThreadUploadPhoto);
            th.IsBackground = true;
            th.Start();
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            buttonEdit2.Text = "";
        }

        private void buttonEdit2_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Filter = "图片文件(*.bmp;*.jpg;*.jpeg;*.gif;*.png)|*.bmp;*.jpg;*.jpeg;*.gif;*.png";
            dialog.CheckFileExists = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (((dialog.FileName.ToLower().EndsWith("bmp") || dialog.FileName.ToLower().EndsWith("jpg")) || (dialog.FileName.ToLower().EndsWith("jpeg") || dialog.FileName.ToLower().EndsWith("gif"))) || dialog.FileName.ToLower().EndsWith("png"))
                {
                    ((DevExpress.XtraEditors.ButtonEdit)sender).Text = dialog.FileName;
                    return;
                }
                XtraMessageBox.Show("选择的图片格式错误!请重新选择!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            buttonEdit3.Text = "";
        }

        private void buttonEdit3_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Filter = "图片文件(*.bmp;*.jpg;*.jpeg;*.gif;*.png)|*.bmp;*.jpg;*.jpeg;*.gif;*.png";
            dialog.CheckFileExists = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (((dialog.FileName.ToLower().EndsWith("bmp") || dialog.FileName.ToLower().EndsWith("jpg")) || (dialog.FileName.ToLower().EndsWith("jpeg") || dialog.FileName.ToLower().EndsWith("gif"))) || dialog.FileName.ToLower().EndsWith("png"))
                {
                    ((DevExpress.XtraEditors.ButtonEdit)sender).Text = dialog.FileName;
                    return;
                }
                XtraMessageBox.Show("选择的图片格式错误!请重新选择!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            ShowImg(2);
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            ShowImg(3);
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            pictureBox1.Width += ConvertType.ToInt32(barEditItem1.EditValue);
            pictureBox1.Height += ConvertType.ToInt32(barEditItem1.EditValue);
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            pictureBox1.Width -= ConvertType.ToInt32(barEditItem2.EditValue);
            pictureBox1.Height -= ConvertType.ToInt32(barEditItem2.EditValue);
        }

        private void edfgs_EditValueChanged(object sender, EventArgs e)
        {
           CommonClass.SetWeb(edbm,edfgs.Text.Trim(),false);
        }

        private void UpLoadFile(string path,int PicIndex)
        {
            if (path == "")
            {
                MsgBox.ShowError("请选择一个图片文件！");
                return;
            }
            if (path.EndsWith(".jpg") || path.EndsWith(".jpeg") || path.EndsWith(".png"))
            {
                PicDeal.SendSmallImage(path, ref path, 800, 600);
            }
            string ResultPath = string.Format("/Files/{0}/{3}-{1}{2}", DateTime.Now.ToString("yyyy-MM-dd"), Guid.NewGuid().ToString(), Path.GetExtension(path), edbh.Text.Trim());
            if (!this.IsHandleCreated) return;
            try
            {
                this.Invoke((MethodInvoker)delegate
                {
                    labelControl1.Text = "文件上传中，请稍后";
                    simpleButton8.Enabled = false;
                    simpleButton9.Enabled = false;
                    //timer1.Enabled = true;
                    Application.DoEvents();
                });

                byte[] bt = wc.UploadFile(new Uri(string.Format("{1}/FileUpLoad.ashx?Path={0}", ResultPath, FileUpload.UpFileUrl)), "POST", path);
                string json = Encoding.UTF8.GetString(bt);
                frmUpLoadFile.UploadResult result = JsonConvert.DeserializeObject<frmUpLoadFile.UploadResult>(json);

                if (result.State == 1)
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("bh", bh == "" ? edbh.Text.Trim() : bh));
                    list.Add(new SqlPara("picIndex", picIndex));
                    list.Add(new SqlPara("picdate", CommonClass.gcdate));
                    list.Add(new SqlPara("picman", CommonClass.UserInfo.UserName));
                    list.Add(new SqlPara("pic", ResultPath));

                    //con.Open();
                    try
                    {
                        if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_YG_PHOTO_2", list)) > 0)
                        {
                            //commonclass.ShowOK();
                            XtraMessageBox.Show("上传完成!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                            result.State = 0;
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                if (!this.IsHandleCreated) return;
                this.Invoke((MethodInvoker)delegate
                {
                    if (result.State == 1)
                    {
                        labelControl1.Text = "上传成功!";
                    }
                    else
                    {
                        labelControl1.Text = "上传失败!";
                    }
                    //timer1.Enabled = false;
                    simpleButton8.Enabled = true;
                    simpleButton9.Enabled = true;
                });
            }
            catch (Exception ex)
            {
                if (IsHandleCreated)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        labelControl1.Text = "上传失败  原因：" + ex.Message;
                        //timer1.Enabled = false;
                        simpleButton8.Enabled = true;
                        simpleButton9.Enabled = true;
                    });
                }
            }
            finally
            {
                if (IsHandleCreated)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        //labelControl99.Text = "未选择";
                    });
                }
            }
        }
        private  void ShowImg(int picIndex)
        {
            this.picIndex = picIndex;

            //SqlConnection con = cc.GetConn();
            //SqlCommand cmd = new SqlCommand("QSP_GET_YG_PHOTO_2");
            //cmd.CommandType = CommandType.StoredProcedure;

            //cmd.Parameters.Add("@bh", SqlDbType.VarChar).Value = bh == "" ? edbh.Text.Trim() : bh;
            //cmd.Parameters.Add("@picIndex", SqlDbType.Int).Value = picIndex;
   
            ////con.Open();
            //DataSet ds_pic = cs.GetDataSet(cmd,null);

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("bh", bh == "" ? edbh.Text.Trim() : bh));
            list.Add(new SqlPara("picIndex",picIndex));
            DataSet ds_pic = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_YG_PHOTO_2", list));
            if (ds_pic.Tables[0].Rows.Count>0)
            {
                if (ds_pic.Tables[0].Rows[0]["pic"] == DBNull.Value)
                {
                    //reader.Close();
                    XtraMessageBox.Show("该车辆没有上传照片!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
               string buffer = ConvertType.ToString(ds_pic.Tables[0].Rows[0]["pic"]);
                if (buffer == "")
                {
                    //reader.Close();
                    XtraMessageBox.Show("该车辆没有上传照片!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            try
            {
                string filename = ConvertType.ToString(ds_pic.Tables[0].Rows[0]["pic"]);
                string bdPath = System.Windows.Forms.Application.StartupPath + "/TempFile";
                if (!Directory.Exists(bdPath)) Directory.CreateDirectory(bdPath);
                string bdFileName = bdPath + filename.Substring(filename.LastIndexOf("/"));

                if (!File.Exists(bdFileName))
                {
                    WebClient wc = new WebClient();
                    wc.DownloadFile(FileUpload.UpFileUrl + filename, bdFileName);
                }

                frmShowPic frm = new frmShowPic();
                frm.imgPath = bdFileName;
                frm.Show();

                //System.Diagnostics.Process process = new System.Diagnostics.Process();
                //process.StartInfo.FileName = bdFileName;
                //process.StartInfo.Verb = "Open";
                //process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                //process.Start();
            }
            catch (Exception ee)
            {
                MsgBox.ShowOK("打开失败。您的系统中没有合适的程序打开该文件!\r\n" + ee.Message);
            }
        }
    }
}