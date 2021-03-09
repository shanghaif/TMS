using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using System.IO;
using System.Diagnostics;
using System.Drawing.Imaging;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using Newtonsoft.Json;
using System.Net;
using System.Threading;

namespace ZQTMS.UI
{
    public partial class w_oa_file_add : BaseForm
    {
        public w_oa_file_add()
        {
            InitializeComponent();
        }
        WebClient wc = new WebClient();
        string spath = "";
        private void w_file_add_Load(object sender, EventArgs e)
        {
            //cc.GetAllUsers();
            edcreateby.Text = CommonClass.UserInfo.UserName
                ;
            string bsite = "";
            if (CommonClass.dsSite.Tables.Count == 0 || CommonClass.dsWeb.Tables.Count == 0 || CommonClass.dsUsers.Tables.Count == 0) return;
            for (int i = 0; i < CommonClass.dsSite.Tables[0].Rows.Count; i++)
            {
                bsite = CommonClass.dsSite.Tables[0].Rows[i]["SiteName"].ToString();
                TreeNode tn = new TreeNode(bsite);
                DataRow[] drs = CommonClass.dsWeb.Tables[0].Select(string.Format("SiteName='{0}'", bsite));
                foreach (DataRow dr in drs)
                {
                    string webid = dr["WebName"].ToString();
                    TreeNode tn1 = new TreeNode(webid);
                    DataRow[] rows = CommonClass.dsUsers.Tables[0].Select(string.Format("SiteName ='{0}' and WebName ='{1}'", bsite, webid));
                    foreach (DataRow row in rows)
                    {
                        tn1.Nodes.Add(row["UserAccount"].ToString() + "|" + row["UserName"].ToString());
                    }
                    tn.Nodes.Add(tn1);
                }
                treeView1.Nodes.Add(tn);
            }
            GetFiletype();
        }

        private void GetFiletype()
        {
            try
            {
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_OA_FILE_TYPE"));
                if (ds == null || ds.Tables.Count == 0) return;
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    edfiletype.Properties.Items.Add(item["type"].ToString());
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
                TreeNodeParentChecked(e.Node);
                TreeNodeChildChecked(e.Node);
            }
        }

        /// <summary>
        /// 如果当前节点的父节点不为空，则选中父节点
        /// </summary>
        /// <param name="tn">当前节点</param>
        private void TreeNodeParentChecked(TreeNode tn)
        {
            if (tn.Parent != null)
            {
                if (tn.Checked)
                {
                    tn.Parent.Checked = tn.Checked;
                }
                else
                {
                    int aa = 0;
                    foreach (TreeNode t1 in tn.Parent.Nodes)
                    {
                        if (t1.Checked)
                        {
                            aa++;
                        }
                    }
                    tn.Parent.Checked = aa != 0;
                }
                tn.Parent.ForeColor = tn.Parent.Checked == true ? Color.Red : Color.Black;
                TreeNodeParentChecked(tn.Parent);
            }
        }

        /// <summary>
        /// 如果当前节点包含子节点，将当前节点选中状态设置为和当前节点一样
        /// </summary>
        /// <param name="tn">当前节点</param>
        private void TreeNodeChildChecked(TreeNode tn)
        {
            tn.ForeColor = tn.Checked == true ? Color.Red : Color.Black;
            if (tn.Nodes.Count > 0)
            {
                foreach (TreeNode tn1 in tn.Nodes)
                {
                    tn1.Checked = tn.Checked;
                    tn1.ForeColor = tn1.Checked == true ? Color.Red : Color.Black;
                    TreeNodeChildChecked(tn1);
                }
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (edfile.Text.Trim() == "")
            {
                XtraMessageBox.Show("请选择要上传的文件!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                edfile.Focus();
                return;
            }
            if (!File.Exists(edfile.Text.Trim()))
            {
                XtraMessageBox.Show("选择的文件不存在!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                edfile.Focus();
                return;
            }
            string docname = eddocname.Text.Trim();
            if (docname == "")
            {
                MsgBox.ShowOK("必须填写文档标题!");
                eddocname.Focus();
                return;
            }

            string receiveusername = edreceive.Text.Trim();
            if (receiveusername == "")
            {
                MsgBox.ShowOK("必须选择接收对象!");
                edreceive.ShowPopup();
                return;
            }
            string filetype = edfiletype.Text.Trim();
            if (filetype == "")
            {
                MsgBox.ShowOK("必须选择文档类型!");
                edfiletype.ShowPopup();
                return;
            }

            FileInfo info = new FileInfo(edfile.Text.Trim());
            if (info.Length > 5 * 1024 * 1024)
            {
                XtraMessageBox.Show("上传失败! 上传文件最大为5M\r\n当前文件大小：" + Math.Round(Convert.ToDecimal(info.Length) / Convert.ToDecimal(1024 * 1024), 2).ToString() + "M", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                edfile.Focus();
                return;
            }
            spath = string.Format("/Files/{0}/{1}{2}", DateTime.Now.ToString("yyyy-MM-dd"), Guid.NewGuid().ToString(), Path.GetExtension(edfile.Text.ToString()));
            Thread th = new Thread(new ThreadStart(UpLoadFiles));
            try
            {
                th.IsBackground = true;

                th.Start();
                if (th.ThreadState == System.Threading.ThreadState.Stopped)
                {
                    th.Abort();
                }

                string bsite = "", webid = "";
                string[] s;
                string receiveuserid = "";
                foreach (TreeNode tn in treeView1.Nodes)
                {
                    bsite = tn.Text;
                    foreach (TreeNode tn1 in tn.Nodes)
                    {
                        webid = tn1.Text;
                        foreach (TreeNode tn2 in tn1.Nodes)
                        {
                            if (checkEdit1.Checked)
                            {
                                s = tn2.Text.Split('|');
                                receiveuserid += s[0] + "@";
                            }
                            else
                            {
                                if (tn2.Checked)
                                {
                                    s = tn2.Text.Split('|');
                                    receiveuserid += s[0] + "@";
                                }
                            }
                        }
                    }
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("id", 0));
                list.Add(new SqlPara("docname", docname));
                list.Add(new SqlPara("filetype", filetype));
                list.Add(new SqlPara("length", ConvertType.ToInt32(new FileInfo(edfile.Text.Trim()).Length)));
                list.Add(new SqlPara("createby", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("receiveusername", edreceive.Text.Trim()));
                list.Add(new SqlPara("filedata", spath));
                list.Add(new SqlPara("remark", edremark.Text.Trim()));
                list.Add(new SqlPara("bsite", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("webid", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("receiveuserid", receiveuserid));
                SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Query, "USP_ADD_OA_FILE", list));

                XtraMessageBox.Show("上传完毕!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                edfile.Text = "";
                eddocname.Text = "";
                edreceive.Text = "";
                edfiletype.Text = "";
                edremark.Text = "";

                #region
                foreach (TreeNode tn in treeView1.Nodes)
                {
                    tn.Checked = false;
                }
                #endregion
            }
            catch (Exception ex)
            {

                th.Abort();
                XtraMessageBox.Show("上传失败!\r\n" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private byte[] GetByte()
        {
            string FromPath = edfile.Text.Trim();
            string SavePath = Application.StartupPath + @"\" + Path.GetFileNameWithoutExtension(edfile.Text.Trim()) + ".rar";

            byte[] buffer = null;
            Process Process1 = new Process();
            try
            {
                if (File.Exists(SavePath))
                {
                    File.Delete(SavePath);
                }
                Process1.StartInfo.FileName = Application.StartupPath + @"\Rar.exe";
                Process1.StartInfo.CreateNoWindow = true;
                Process1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                Process1.StartInfo.Arguments = " a -ep " + "\"" + SavePath + "\"" + " " + "\"" + FromPath + "\"";  //压缩
                Process1.Start();
                int ID = Process1.Id;
                Process1.WaitForExit();
                if (Process1.HasExited)
                {
                    int ExitCode = Process1.ExitCode;
                    if (ExitCode == 0)
                    {
                        FileStream stream = new FileInfo(SavePath).OpenRead();
                        buffer = new byte[stream.Length];
                        stream.Read(buffer, 0, Convert.ToInt32(stream.Length));
                        stream.Close();
                        Process1.Close();
                        if (File.Exists(SavePath))
                        {
                            File.Delete(SavePath);
                        }
                        return buffer;
                    }
                    else
                    {
                        Process1.Close();
                        XtraMessageBox.Show("压缩失败!");
                        return null;
                    }
                }
                return buffer;
            }
            catch (Exception ex)
            {
                Process1.Close();
                MsgBox.ShowOK(ex.Message);
                return null;
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void edfilename_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.CheckFileExists = true;
            dialog.Filter = "Microsoft Office 文档(*.doc;*.docx;*.xls;*.xlsx)|*.doc;*.docx;*.xls;*.xlsx";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string ext = Path.GetExtension(dialog.FileName).ToLower();
                edfile.Text = dialog.FileName;
                eddocname.Text = Path.GetFileNameWithoutExtension(dialog.FileName);
            }
        }

        private void edtype_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Plus)
            {
                w_oa_file_type bf = new w_oa_file_type();
                bf.edit = edfiletype;
                bf.ShowDialog();
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            string s = "";
            if (checkEdit1.Checked)
            {
                s = "全部";
            }
            else
            {
                foreach (TreeNode tn in treeView1.Nodes)
                {
                    foreach (TreeNode tn1 in tn.Nodes)
                    {
                        foreach (TreeNode tn2 in tn1.Nodes)
                        {
                            if (tn2.Checked)
                            {
                                s += tn2.Text + ",";
                            }
                        }
                    }
                }
            }

            edreceive.Text = s.TrimEnd(',');
            edreceive.ClosePopup();
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit1.Checked)
            {
                foreach (TreeNode tn in treeView1.Nodes)
                {
                    tn.Checked = false;
                }
            }
        }

        private void UpLoadFiles()
        {
            string path = edfile.Text.ToString();
            string pathNew = spath;
            try
            {
                byte[] bt = wc.UploadFile(new Uri(string.Format("{1}/FileUpLoad.ashx?Path={0}", pathNew, FileUpload.UpFileUrl)), "POST", path);
                string json = Encoding.UTF8.GetString(bt);
                ZQTMS.Tool.frmUpLoadFile.UploadResult result = JsonConvert.DeserializeObject<ZQTMS.Tool.frmUpLoadFile.UploadResult>(json);
            }
            catch
            {
                //this.Invoke((EventHandler)(delegate
                //{

                //}));
            }
        }
    }
}