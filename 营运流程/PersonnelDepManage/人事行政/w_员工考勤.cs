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
using DevExpress.XtraBars.Alerter;
using System.Threading;
using DevExpress.XtraGrid.Columns;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class w_员工考勤 : BaseForm
    {
        //commonclass cc = new commonclass();
        DataSet ds = new DataSet();
        //string exception = "";

        //userright ur = new userright();
        DataSet zwds = new DataSet();
        //Cls_SqlHelper cs = new Cls_SqlHelper();

        public w_员工考勤()
        {
            InitializeComponent();
        }

        private void getdata()
        {
            //SqlConnection connection = cc.GetConn();
            try
            {
                //SqlDataAdapter da = new SqlDataAdapter();
                //SqlCommand sq = new SqlCommand("QSP_GET_YGDA_KAOQIN_2", connection);
                //sq.CommandType = CommandType.StoredProcedure;
                //sq.Parameters.Add("@site", SqlDbType.VarChar).Value = comboBoxEdit1.Text.Trim() == "全部" ? "%%" : comboBoxEdit1.Text.Trim();
                //sq.Parameters.Add("@bm", SqlDbType.VarChar).Value = edbm.Text.Trim() == "全部" ? "%%" : edbm.Text.Trim();
                //da.SelectCommand = sq;
                //ds.Clear();
                //da.Fill(ds);
                //gridControl1.DataSource = ds.Tables[0];// FilterDs(ds);
                //SqlCommand sq = new SqlCommand("QSP_GET_YGDA_KAOQIN_2");
                //sq.CommandType = CommandType.StoredProcedure;
                //sq.Parameters.Add("@site", SqlDbType.VarChar).Value = comboBoxEdit1.Text.Trim() == "全部" ? "%%" : comboBoxEdit1.Text.Trim();
                //sq.Parameters.Add("@bm", SqlDbType.VarChar).Value = edbm.Text.Trim() == "全部" ? "%%" : edbm.Text.Trim();
                //ds = cs.GetDataSet(sq, gridControl1);

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("site", comboBoxEdit1.Text.Trim() == "全部" ? "%%" : comboBoxEdit1.Text.Trim()));
                list.Add(new SqlPara("bm", edbm.Text.Trim() == "全部" ? "%%" : edbm.Text.Trim()));
                ds =  SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_YGDA_KAOQIN_2", list));
                if (ds == null || ds.Tables.Count == 0) return;
                gridControl1.DataSource = ds.Tables[0];


            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //if (connection.State == ConnectionState.Open) connection.Close();
            }
        }

        /// <summary>
        /// 没有权限的将不会显示
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private DataTable FilterDs(DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0) return null;
            if (zwds == null || zwds.Tables.Count == 0) return ds.Tables[0];

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string zw = ConvertType.ToString(ds.Tables[0].Rows[i]["zw"]);
                int k = 0;
                for (int j = 0; j < zwds.Tables[0].Rows.Count; j++)
                {
                    string xm = zwds.Tables[0].Rows[j]["xm"].ToString();
                    if (xm == zw)
                    {
                        k += 1;
                    }
                }

                if (k == 0)
                {
                    ds.Tables[0].Rows.RemoveAt(i);
                    i--;
                }
                k = 0;
            }

            return ds.Tables[0];
        }

        private void getdatadetail()
        {
            //SqlConnection con = cc.GetConn();
            try
            {
                //SqlCommand sq = new SqlCommand("QSP_GET_USER_ZW");
                //sq.CommandType = CommandType.StoredProcedure;
                //sq.Parameters.Add("@username", SqlDbType.VarChar).Value = commonclass.username;
                ////SqlDataAdapter ada = new SqlDataAdapter(sq);
                //zwds = cs.GetDataSet(sq, new DevExpress.XtraGrid.GridControl());
                //ada.Fill(zwds);
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("username",CommonClass.UserInfo.UserName));
                zwds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_USER_ZW", list));

            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void w_cygs_Load(object sender, EventArgs e)
        {
            //fixcolumn fc = new fixcolumn(gridView1, barSubItem2);
            //GridOper.LoadScheme(gridView1,this.Text);
            CommonClass.InsertLog("员工考勤");//xj2019/5/29
            BarMagagerOper.SetBarPropertity(bar3);
            BarMagagerOper.SetBarPropertity(bar1);
            BarMagagerOper.SetBarPropertity(bar2);
            GridOper.RestoreGridLayout(gridView1, "员工考勤");

            //BarMagagerOper.FillRepCombox(comboBoxEdit1);
            comboBoxEdit1.Properties.Items.Add("全部");
            comboBoxEdit1.Text = CommonClass.UserInfo.SiteName;

            comboBoxEdit3.EditValue = CommonClass.gcdate;

            //cc.FillRepCombox(comboBoxEdit2);
            comboBoxEdit2.Properties.Items.Add("全部");
            comboBoxEdit2.Text = CommonClass.UserInfo.SiteName;


            dateEdit1.EditValue = CommonClass.gcdate;
            //cc.FillRepCombox(comboBoxEdit4);
            comboBoxEdit4.Properties.Items.Add("全部");
            comboBoxEdit4.Text = CommonClass.UserInfo.SiteName;

            dateEdit2.EditValue = CommonClass.gcdate;
            //cc.FillRepCombox(comboBoxEdit5);
            comboBoxEdit5.Properties.Items.Add("全部");
            comboBoxEdit5.Text = CommonClass.UserInfo.SiteName;

            ////考勤登记
            //comboBoxEdit1.Enabled = ur.GetUserRightDetail999("d1030102");
            ////考勤汇总
            //comboBoxEdit2.Enabled = ur.GetUserRightDetail999("d1030202");
            //barButtonItem12.Enabled = ur.GetUserRightDetail999("d1030203");
            ////考勤统计
            //comboBoxEdit4.Enabled = ur.GetUserRightDetail999("d1030302");
            ////排班表
            //comboBoxEdit5.Enabled = ur.GetUserRightDetail999("d1030402");
            //barButtonItem19.Enabled = ur.GetUserRightDetail999("d1030403");
            //barButtonItem25.Enabled = ur.GetUserRightDetail999("d1030404");

            Thread th = new Thread(getdatadetail);
            th.IsBackground = true;
            th.Start();

            fillCombobox_bm();
        }

        private void fillCombobox_bm()
        {
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query,"QSP_GET_ALL_BM"));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                edbm.Properties.Items.Add("全部");
                return;
            }

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (!edbm.Properties.Items.Contains(ds.Tables[0].Rows[i][0]))
                    edbm.Properties.Items.Add(ds.Tables[0].Rows[i][0]);
            }
            if (!edbm.Properties.Items.Contains("全部"))
                edbm.Properties.Items.Add("全部");
            edbm.SelectedIndex = 0;
        }

        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            //cc.GenSeq(e);
        }

        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(gridView1);
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(gridView1, "员工考勤");
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(gridView1, "员工考勤");
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(gridView1);
        }


        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(gridView1);
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            //cc.QuickSearch();
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void savekq(string status)
        {
            int rows = gridView1.FocusedRowHandle;
            if (rows < 0) return;

            try
            {
                if (status == "其他" && memoRemark.Text == "")
                {
                    XtraMessageBox.Show("必须填写备注!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                //SqlConnection con = cc.GetConn();
                //SqlCommand sq = new SqlCommand("USP_ADD_YGKQ_2");
                //sq.CommandType = CommandType.StoredProcedure;
                //sq.Parameters.Add("@ygid", SqlDbType.VarChar).Value = gridView1.GetRowCellValue(rows, "id").ToString();
                //sq.Parameters.Add("@status", SqlDbType.VarChar).Value = status;
                //sq.Parameters.Add("@billdate", SqlDbType.VarChar).Value = CommonClass.gcdate;
                //sq.Parameters.Add("@qitaRemark", SqlDbType.VarChar).Value = status == "其他" ? memoRemark.Text.Trim() : "";
                ////con.Open();
                ////sq.ExecuteNonQuery();
                ////con.Close();
                //cs.ENQ(sq);
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ygid",gridView1.GetRowCellValue(rows, "id").ToString()));
                list.Add(new SqlPara("status", status));
                list.Add(new SqlPara("billdate", CommonClass.gcdate));
                list.Add(new SqlPara("qitaRemark", status == "其他" ? memoRemark.Text.Trim() : ""));
                SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_YGKQ_2", list));
                gridView1.DeleteRow(rows);
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
            finally
            {
                panelControl5.Visible = false;
            }
        }

        private void 正常ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            savekq("正常");
        }
        private void 迟到ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            savekq("迟到");
        }

        private void 请假ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            savekq("请假");
        }

        private void 旷工ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            savekq("旷工");
        }

        private void 其他ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            memoRemark.Text = "";
            panelControl5.Visible = true;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //SqlConnection connection = cc.GetConn();
            try
            {
                //SqlDataAdapter da = new SqlDataAdapter();
                // SqlCommand sq = new SqlCommand("QSP_GET_YGKQ_TATOL");
                //sq.CommandType = CommandType.StoredProcedure;
                //sq.Parameters.Add("@site", SqlDbType.VarChar).Value = comboBoxEdit2.Text.Trim() == "全部" ? "%%" : comboBoxEdit2.Text.Trim();
                //sq.Parameters.Add("@billdate", SqlDbType.VarChar).Value = comboBoxEdit3.DateTime;
                ////da.SelectCommand = sq;
                //DataSet ds = new DataSet();
                //ds = cs.GetDataSet(sq, new DevExpress.XtraGrid.GridControl());
                ////da.Fill(ds);
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("site", comboBoxEdit2.Text.Trim() == "全部" ? "%%" : comboBoxEdit2.Text.Trim()));
                list.Add(new SqlPara("billdate", comboBoxEdit3.DateTime));
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_YGKQ_TATOL", list));
                if (ds == null || ds.Tables.Count == 0) return;
                //gridControl1.DataSource = ds.Tables[0];

                DataTable table2 = ds.Tables[1];

                DateTime dtNow = comboBoxEdit3.DateTime;
                int days = DateTime.DaysInMonth(dtNow.Year, dtNow.Month);
                for (int i = 1; i <= days; i++)
                {
                    ds.Tables[0].Columns.Add(i + "", typeof(string));
                    ds.Tables[0].Columns.Add("qitaRemark" + i, typeof(string));
                }

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int id = 0;
                    if (ds.Tables[0].Rows[i]["id"].ToString().Trim() != "")

                        id = Convert.ToInt32(ds.Tables[0].Rows[i]["id"]);
                    else
                        continue;

                    for (int n = 0; n < table2.Rows.Count; n++)
                    {
                        int ygid = 0;
                        if(table2.Rows[n]["ygid"].ToString().Trim() != "")
                           ygid = Convert.ToInt32(table2.Rows[n]["ygid"]);
                        if (id == ygid)
                        {
                            string billdate = Convert.ToInt32(Convert.ToDateTime(table2.Rows[n]["billdate"]).ToString("dd")) + "";

                            for (int k = 0; k < ds.Tables[0].Columns.Count; k++)
                            {
                                //if (ds.Tables[0].Columns[k].ColumnName!="xm")
                                if (billdate == ds.Tables[0].Columns[k].ColumnName)
                                {
                                    ds.Tables[0].Rows[i][ds.Tables[0].Columns[k].ColumnName] = table2.Rows[n]["status"].ToString();
                                    ds.Tables[0].Rows[i][ds.Tables[0].Columns["qitaRemark" + billdate].ColumnName] = table2.Rows[n]["qitaRemark"].ToString();
                                }
                            }
                        }
                    }
                }

                gridControl2.DataSource = ds.Tables[0];
                //foreach (GridColumn gc in gridView2.Columns)
                //{
                //    if (gc.FieldName.Contains("qitaRemark"))
                //        gc.Visible = gc.OptionsColumn.ShowInCustomizationForm = false;
                //}
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //if (connection.State == ConnectionState.Open) connection.Close();
            }
        }

        private void comboBoxEdit3_EditValueChanged(object sender, EventArgs e)
        {
            gridView2.Columns.Clear();

            GridColumn gridColumn121 = new DevExpress.XtraGrid.Columns.GridColumn();
            gridView2.Columns.Add(gridColumn121);
            gridColumn121.Caption = "员工姓名";
            gridColumn121.FieldName = "xm";
            gridColumn121.Name = "gridColumn6";
            gridColumn121.OptionsColumn.AllowEdit = false;
            gridColumn121.OptionsColumn.AllowFocus = false;
            gridColumn121.Visible = true;
            gridColumn121.Width = 100;

            GridColumn gridColumn122 = new DevExpress.XtraGrid.Columns.GridColumn();
            gridView2.Columns.Add(gridColumn122);
            gridColumn122.Caption = "职务";
            gridColumn122.FieldName = "zw";
            gridColumn122.Name = "gridColumn6";
            gridColumn122.OptionsColumn.AllowEdit = false;
            gridColumn122.OptionsColumn.AllowFocus = false;
            gridColumn122.Visible = true;
            gridColumn122.Width = 100;


            GridColumn gridColumn123 = new DevExpress.XtraGrid.Columns.GridColumn();
            gridView2.Columns.Add(gridColumn123);
            gridColumn123.Caption = "工作时间";
            gridColumn123.FieldName = "worktime";
            gridColumn123.Name = "gridColumn6";
            gridColumn123.OptionsColumn.AllowEdit = false;
            gridColumn123.OptionsColumn.AllowFocus = false;
            gridColumn123.Visible = true;
            gridColumn123.Width = 100;

            DateTime dtNow = comboBoxEdit3.DateTime;
            int days = DateTime.DaysInMonth(dtNow.Year, dtNow.Month);
            for (int i = 1; i <= days; i++)
            {
                GridColumn gv = new DevExpress.XtraGrid.Columns.GridColumn();
                gridView2.Columns.Add(gv);
                gv.Caption = i + "";
                gv.FieldName = i + "";
                gv.Name = "gridColumn" + i;
                gv.OptionsColumn.AllowEdit = false;
                gv.OptionsColumn.AllowFocus = false;
                gv.Visible = true;
                gv.Width = 100;

                gv = new GridColumn();
                gridView2.Columns.Add(gv);
                gv.Caption = "其他备注";
                gv.FieldName = "qitaRemark" + i;
                gv.Name = "qitaRemark" + i;
                gv.OptionsColumn.AllowEdit = gv.OptionsColumn.AllowFocus = false;
                gv.Visible = gv.OptionsColumn.ShowInCustomizationForm = false;
                gv.Width = 100;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(gridView2);
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void barCheckItem4_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(gridView2);
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rows = gridView2.FocusedRowHandle;
            if (rows < 0) return;

            try
            {
                w_ygkq_modified wy = new w_ygkq_modified();
                if (wy.ShowDialog() == DialogResult.Yes)
                {
                    //SqlConnection con = cc.GetConn();
                    //SqlCommand sq = new SqlCommand("USP_ADD_YGKQ");
                    //sq.CommandType = CommandType.StoredProcedure;
                    //sq.Parameters.Add("@ygid", SqlDbType.VarChar).Value = gridView2.GetRowCellValue(rows, "id").ToString();
                    //sq.Parameters.Add("@status", SqlDbType.VarChar).Value = wy.comboBoxEdit1.Text.Trim();
                    //sq.Parameters.Add("@billdate", SqlDbType.VarChar).Value = wy.dateEdit1.DateTime;
                    ////con.Open();
                    ////sq.ExecuteNonQuery();
                    ////con.Close();
                    //cs.ENQ(sq);
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("ygid", gridView2.GetRowCellValue(rows, "id").ToString()));
                    list.Add(new SqlPara("status",wy.comboBoxEdit1.Text.Trim()));
                    list.Add(new SqlPara("billdate", wy.dateEdit1.DateTime));
                    SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_YGKQ", list));
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
           // SqlConnection con = cc.GetConn();
            try
            {
                //SqlCommand sq = new SqlCommand("员工考勤统计");
                //sq.CommandType = CommandType.StoredProcedure;
                //sq.Parameters.Add("@bsite", SqlDbType.VarChar).Value = comboBoxEdit4.Text.Trim() == "全部" ? "%%" : comboBoxEdit4.Text.Trim();
                //sq.Parameters.Add("@billdate", SqlDbType.VarChar).Value = dateEdit1.DateTime;
                //SqlDataAdapter ada = new SqlDataAdapter(sq);
                //DataSet ds = new DataSet();
                ////ada.Fill(ds);
                ////gridControl3.DataSource = ds.Tables[0];
                //ds = cs.GetDataSet(sq, gridControl3);
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bsite",comboBoxEdit4.Text.Trim() == "全部" ? "%%" : comboBoxEdit4.Text.Trim()));
                list.Add(new SqlPara("billdate", dateEdit1.DateTime));
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "员工考勤统计", list));
                gridControl3.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void barButtonItem19_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = false;
                dialog.Filter = "图片文件(*.bmp;*.jpg;*.jpeg;*.gif;*.png)|*.bmp;*.jpg;*.jpeg;*.gif;*.png";
                dialog.CheckFileExists = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (((dialog.FileName.ToLower().EndsWith("bmp") || dialog.FileName.ToLower().EndsWith("jpg")) || (dialog.FileName.ToLower().EndsWith("jpeg") || dialog.FileName.ToLower().EndsWith("gif"))) || dialog.FileName.ToLower().EndsWith("png"))
                    {
                        byte[] buffer1 = null;

                        SendSmallImage(dialog.FileName, ref buffer1, 1024, 768);
                        if (buffer1 == null)
                        {
                            XtraMessageBox.Show("货损照片一加载失败，请重新选择!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        //SqlConnection conn = cc.GetConn();
                        //conn.Open();
                        //SqlCommand cmd = new SqlCommand("USP_ADD_KQ_PIC");
                        //cmd.CommandType = CommandType.StoredProcedure;

                        //cmd.Parameters.Add(new SqlParameter("@bsite", SqlDbType.VarChar)).Value = commonclass.gbsite;
                        //cmd.Parameters.Add(new SqlParameter("@billdate", SqlDbType.DateTime)).Value = commonclass.gcdate;
                        //cmd.Parameters.Add(new SqlParameter("@createby", SqlDbType.VarChar)).Value = commonclass.username;
                        //cmd.Parameters.Add(new SqlParameter("@pic", SqlDbType.Image)).Value = buffer1;
                        ////cmd.ExecuteNonQuery();
                        ////commonclass.ShowOK();
                        ////conn.Close();
                        //cs.ENQ(cmd);
                        List<SqlPara> list = new List<SqlPara>();
                        list.Add(new SqlPara("bsite",CommonClass.UserInfo.SiteName));
                        list.Add(new SqlPara("billdate",CommonClass.gcdate));
                        list.Add(new SqlPara("createby",CommonClass.UserInfo.UserName));
                        list.Add(new SqlPara("pic", buffer1));
                        SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_KQ_PIC", list));



                    }

                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
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

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            //SqlConnection con = cc.GetConn();
            try
            {
                //SqlCommand sq = new SqlCommand("QSP_GET_KQ_PIC");
                //sq.CommandType = CommandType.StoredProcedure;
                //sq.Parameters.Add("@bsite", SqlDbType.VarChar).Value = comboBoxEdit5.Text.Trim() == "全部" ? "%%" : comboBoxEdit5.Text.Trim();
                //sq.Parameters.Add("@billdate", SqlDbType.VarChar).Value = dateEdit2.DateTime;
                //SqlDataAdapter ada = new SqlDataAdapter(sq);
                //DataSet ds = new DataSet();
                //ada.Fill(ds);
                //gridControl4.DataSource = ds.Tables[0];
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bsite",comboBoxEdit5.Text.Trim() == "全部" ? "%%" : comboBoxEdit5.Text.Trim()));
                list.Add(new SqlPara("billdate", dateEdit2.DateTime));
                //ds = cs.GetDataSet(sq, gridControl4);
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_KQ_PIC", list));
                if (ds != null && ds.Tables.Count > 0) gridControl4.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void repositoryItemHyperLinkEdit1_Click(object sender, EventArgs e)
        {
            int rowshand = Convert.ToInt32(gridView4.FocusedRowHandle);
            if (rowshand < 0) return;
            int id = Convert.ToInt32(gridView4.GetRowCellValue(rowshand, "id"));
            //SqlCommand sq = new SqlCommand("QSP_GET_PIC_IMAGE");
            //sq.CommandType = CommandType.StoredProcedure;
            //sq.Parameters.Add("@id", SqlDbType.Int).Value = id;
            //DataSet ds = cs.GetDataSet(sq, null);
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("id", id));
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_PIC_IMAGE", list));
            if (ds.Tables[0].Rows.Count > 0)
            {
                byte[] buffers = (byte[])ds.Tables[0].Rows[0]["pic"];
                MemoryStream stream = new MemoryStream(buffers);
                Image image = Image.FromStream(stream);

                //w_quicksearch_pic pic = new w_quicksearch_pic();
                //pic.img = image;
                ////pic.units = edunit.Text;
                ////pic.types = type;
                ////pic.times = times;
                //pic.ShowDialog();

            }
        }

        private void barButtonItem25_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rowshand = Convert.ToInt32(gridView4.FocusedRowHandle);
            if (rowshand < 0) return;

            if (DialogResult.Yes != MsgBox.ShowYesNo("确定删除？")) return;

            //SqlConnection con = cc.GetConn();
            try
            {
                int id = Convert.ToInt32(gridView4.GetRowCellValue(rowshand, "id"));
                //SqlCommand sq = new SqlCommand("USP_DELETE_KQ_PIC");
                //sq.CommandType = CommandType.StoredProcedure;
                //sq.Parameters.Add("@id", SqlDbType.Int).Value = id;
                ////con.Open();
                ////sq.ExecuteNonQuery();
                ////con.Close();
                ////commonclass.ShowOK();
                //cs.ENQ(sq);
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("id", id));
                SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_DELETE_KQ_PIC", list));
                gridView4.DeleteRow(rowshand);
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }


        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {

            this.Close();
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            panelControl5.Visible = false;
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            savekq("其他");
        }

        private void gridView2_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            int rowhandle = gridView2.FocusedRowHandle;
            if (rowhandle < 0) return;

            switch (e.Column.FieldName)
            {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                case "10":
                case "11":
                case "12":
                case "13":
                case "14":
                case "15":
                case "16":
                case "17":
                case "18":
                case "19":
                case "20":
                case "21":
                case "22":
                case "23":
                case "24":
                case "25":
                case "26":
                case "27":
                case "28":
                case "29":
                case "30":
                case "31":
                    string remark = ConvertType.ToString(gridView2.GetRowCellValue(rowhandle, "qitaRemark" + e.Column.FieldName));
                    if (string.IsNullOrEmpty(remark)) return;

                    XtraMessageBox.Show("备注：" + remark, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                default:
                    break;
            }
        }

        private void gridView3_DoubleClick(object sender, EventArgs e)
        {
            int rowhandle = gridView3.FocusedRowHandle;
            if (rowhandle < 0) return;

            //SqlConnection con = cc.GetConn();
            //SqlCommand cmd = new SqlCommand("员工考勤统计_其他明细");
            //cmd.CommandType = CommandType.StoredProcedure;

            //cmd.Parameters.Add("@xm", SqlDbType.VarChar).Value = cc.ToString(gridView3.GetRowCellValue(rowhandle, "xm"));
            //cmd.Parameters.Add("@bsite", SqlDbType.VarChar).Value = comboBoxEdit4.Text.Trim() == "全部" ? "%%" : comboBoxEdit4.Text.Trim();
            //cmd.Parameters.Add("@billdate", SqlDbType.DateTime).Value = dateEdit1.DateTime;
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("xm", ConvertType.ToString(gridView3.GetRowCellValue(rowhandle, "xm"))));
            list.Add(new SqlPara("bsite", comboBoxEdit4.Text.Trim() == "全部" ? "%%" : comboBoxEdit4.Text.Trim()));
            list.Add(new SqlPara("xm", dateEdit1.DateTime));

            //SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            try
            {
                DataSet ds1 = new DataSet();
                //ds1 = cs.GetDataSet(cmd, new DevExpress.XtraGrid.GridControl());
                ds1 = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "员工考勤统计_其他明细", list));
                if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    gridControl5.DataSource = ds1.Tables[0];
                    panelControl6.Visible = true;
                    gridControl5.Focus();
                }
            }
            catch
            {
                gridControl5.DataSource = null;
                panelControl6.Visible = false;
            }
        }

        private void panelControl6_Leave(object sender, EventArgs e)
        {
            panelControl6.Visible = false;
        }

        private void gridControl5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                panelControl6.Visible = false;
        }

        private void 休假ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            savekq("休假");
        }
    }
}