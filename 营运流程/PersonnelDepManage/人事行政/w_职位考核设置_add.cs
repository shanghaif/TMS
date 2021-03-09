using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class w_职位考核设置_add : BaseForm
    {
        DataSet dsgroup = new DataSet();
        public string groupid = "";
        public string groupname = "";
        public string zw = "";
        public w_职位考核设置_add()
        {
            InitializeComponent();
        }
        

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
                TreeNodeParentChecked(e.Node); 
                
            }
        }

        /// <summary>
        /// 设置所有节点置的选中状态
        /// </summary>
        /// <param name="tn">当前节点</param>
        /// <param name="CheckState">是否选中</param>
        private void SetTreeViewAllCheckBoxState(TreeNode tn, bool CheckState)
        {
            tn.Checked = CheckState;
            tn.ForeColor = CheckState == true ? Color.Red : Color.Black;
            //foreach (TreeNode n in tn.Nodes)
            //{
            //    SetTreeViewAllCheckBoxState(n, CheckState);
            //}
        }

        /// <summary>
        /// 如果当前节点的父节点不为空，则选中父节点
        /// </summary>
        /// <param name="tn">当前节点</param>
        private void TreeNodeParentChecked(TreeNode tn)
        {
            if (tn.Parent != null)
            {
                //if (tn.Checked)
                //{
                //    tn.Parent.Checked = tn.Checked;
                //}
                //else
                //{
                //    int aa = 0;
                //    foreach (TreeNode t1 in tn.Parent.Nodes)
                //    {
                //        if (t1.Checked)
                //        {
                //            aa++;
                //        }
                //    }
                //    tn.Parent.Checked = aa != 0;
                //}
                //tn.Parent.ForeColor = tn.Parent.Checked == true ? Color.Red : Color.Black;
                
                //TreeNodeParentChecked(tn.Parent);
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
                    treeView1.Nodes[5].Nodes[8].ForeColor = treeView1.Nodes[5].Nodes[8].Checked == true ? Color.Red : Color.Black;
                    treeView1.Nodes[5].Nodes[8].Checked = false;
                    TreeNodeChildChecked(tn1);
                }
                
            }
        }

        /// <summary>
        /// 设置当前节点的文字颜色
        /// </summary>
        /// <param name="tn"></param>
        private void TreeNodeForeColor(TreeNode tn)
        {
            tn.ForeColor = tn.Checked == true ? Color.Red : Color.Black;
        }

        private void setflag(string tag, int flag)
        {

            try
            {
                foreach (TreeNode tn in treeView1.Nodes)  //level1
                {
                    if (tn.Tag.ToString() == tag)
                    {
                        tn.Checked = flag == 1;
                        TreeNodeForeColor(tn);
                        break;
                    }
                    foreach (TreeNode tn1 in tn.Nodes)  //level 2
                    {
                        if (tn1.Tag.ToString() == tag)
                        {
                            tn1.Checked = flag == 1;
                            TreeNodeForeColor(tn1);
                            break;
                        }

                        foreach (TreeNode tn2 in tn1.Nodes)  //level 3
                        {
                            if (tn2.Tag.ToString() == tag)
                            {
                                tn2.Checked = flag == 1;
                                TreeNodeForeColor(tn2);
                                break;
                            }

                            foreach (TreeNode tn3 in tn2.Nodes)  //level 4
                            {
                                if (tn3.Tag.ToString() == tag)
                                {
                                    tn3.Checked = flag == 1;
                                    TreeNodeForeColor(tn3);
                                    break;
                                }

                                foreach (TreeNode tn4 in tn3.Nodes)  //level 5
                                {
                                    if (tn4.Tag.ToString() == tag)
                                    {
                                        tn4.Checked = flag == 1;
                                        TreeNodeForeColor(tn4);
                                        break;
                                    }
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
        }

        private void save()
        {
            delete();;
            try
            {
                //SqlCommand sq = new SqlCommand("USP_ADD_ZW_KH_XM", connection);
                //sq.CommandType = CommandType.StoredProcedure;
                //sq.Parameters.Add(new SqlParameter("@zw", SqlDbType.VarChar)).Value = edgpname.Text.Trim();
                //sq.Parameters.Add(new SqlParameter("@xm", SqlDbType.VarChar));

                List<SqlPara> list = new List<SqlPara>();
                foreach (TreeNode tn in treeView1.Nodes)  //level1
                {
                    list.Clear();
                    if (tn.Checked)
                    {
                        list.Add(new SqlPara("zw", edgpname.Text.Trim()));
                        list.Add(new SqlPara("xm", ""));
                        //sq.Parameters["@xm"].Value = tn.Text.Trim() ;
                        //sq.ExecuteNonQuery(); 
                        SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_ZW_KH_XM", list));
                    }
                }
                XtraMessageBox.Show("保存成功!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information); 
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
            }
        }

        private void delete()
        {
            //SqlConnection connection = cc.GetConn();
            try
            {
                //connection.Open();
                //SqlCommand sq = new SqlCommand("USP_DELETE_ZW_KH_XM");
                //sq.CommandType = CommandType.StoredProcedure;
                //sq.Parameters.Add(new SqlParameter("@zw", SqlDbType.VarChar)).Value = edgpname.Text.Trim();
                ////sq.ExecuteNonQuery();
                //cs.ENQ(sq);
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("zw", edgpname.Text.Trim()));
                SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_DELETE_ZW_KH_XM", list));
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                //connection.Close();
            }
        }


        private void w_add_right_Load(object sender, EventArgs e)
        {
            BarMagagerOper.SetBarPropertity(bar1);
            edgpname.Text = zw;
            getdata();
            getdatadetail();
        }


        private void getdata()
        {
            //SqlConnection con = cc.GetConn();
            try
            {
                //SqlCommand sq = new SqlCommand("QSP_GET_B_KH_XM");
                //sq.CommandType = CommandType.StoredProcedure;
                //sq.Parameters.Add("@bsiteName ", SqlDbType.VarChar).Value = commonclass.username;
                //DataSet ds=new DataSet();
                ////ada.Fill(ds);
                //ds = cs.GetDataSet(sq,new DevExpress.XtraGrid.GridControl());


                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bsiteName",CommonClass.UserInfo.UserName));
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_B_KH_XM", list));
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string xm = ds.Tables[0].Rows[i][0].ToString();
                    treeView1.Nodes.Add(xm);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }


        private void getdatadetail()
        {
            //SqlConnection con = cc.GetConn();
            try
            {
                //SqlCommand sq = new SqlCommand("QSP_GET_ZW_KH_XM");
                //sq.CommandType = CommandType.StoredProcedure;
                //sq.Parameters.Add("@zw",SqlDbType.VarChar).Value=zw;
                ////SqlDataAdapter ada = new SqlDataAdapter(sq);
                //DataSet ds = new DataSet();
                ////ada.Fill(ds);
                //ds = cs.GetDataSet(sq,new DevExpress.XtraGrid.GridControl());

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("zw", zw));
                DataSet ds = SqlHelper.GetDataSet( new SqlParasEntity(OperType.Query, "QSP_GET_ZW_KH_XM", list));
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string xm = ds.Tables[0].Rows[i]["xm"].ToString();


                    foreach (TreeNode tn in treeView1.Nodes)  //level1
                    {
                        if (tn.Text == xm)
                        {
                            tn.Checked=true;
                        }
                    }


                }

                

            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }
        
        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            treeView1.ExpandAll();
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            treeView1.CollapseAll();
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (TreeNode tn in treeView1.Nodes)
            {
                SetTreeViewAllCheckBoxState(tn, true);
            }
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (TreeNode tn in treeView1.Nodes)
            {
                SetTreeViewAllCheckBoxState(tn, false);
            }
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {            
            save();
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }
    }    
}