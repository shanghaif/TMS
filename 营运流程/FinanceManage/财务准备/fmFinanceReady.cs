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
using DevExpress.XtraTreeList.Nodes;
using ZQTMS.Common;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class fmFinanceReady : BaseForm
    {
        int focusedstate = 0;
        DataSet dsxm = new DataSet();
        public fmFinanceReady()
        {
            InitializeComponent();
        }

        private void fmFinanceReady_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("会计科目");//xj/2019/5/28
            CommonClass.SetSite(edbsite, false);
            edbsite.Text = CommonClass.UserInfo.SiteName;
            focusedstate = 2;
        }

        private void Clear()
        {
            a.Text = "";
            b.Text = "";

        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (a.Text.Trim() == "" || b.Text.Trim() == "")
            {
                XtraMessageBox.Show("科目代码和名称必须填写", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string s = a.Text.Trim();
            for (int i = 0; i < 1000; i++)
            {
                TreeListNode tln = treeList1.FindNodeByID(i);
                if (tln == null)
                {
                    break;
                }
                if (tln.GetDisplayText("SubjectID") == s)
                {
                    focusedstate = 1;
                    treeList1.FocusedNode = tln;
                    focusedstate = 2;
                    XtraMessageBox.Show("科目代码重复。重复代码为：" + s, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            DataRow dr = dsxm.Tables[0].NewRow();
            dr["SubjectID"] = a.Text.Trim();
            dr["SubjectName"] = b.Text.Trim();
            dr["ParentID"] = 0;
            dr["SiteName"] = edbsite.Text.Trim();
            dsxm.Tables[0].Rows.Add(dr);
            treeList1.DataSource = dsxm.Tables[0];

            for (int i = 0; i < 1000; i++)
            {
                TreeListNode tln = treeList1.FindNodeByID(i);
                if (tln == null)
                {
                    break;
                }
                if (tln.GetDisplayText("SubjectID") == s)
                {
                    focusedstate = 1;
                    treeList1.FocusedNode = tln;
                    focusedstate = 2;
                    break;
                }
            }
            Clear();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (a.Text.Trim() == "" || b.Text.Trim() == "")
            {
                XtraMessageBox.Show("科目代码和名称必须填写", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int level = treeList1.FocusedNode.Level;
            if (level == -1)
            {
                XtraMessageBox.Show("必须选定一个上级科目", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (level >= 3)
            {
                XtraMessageBox.Show("目前最多只支持四级级科目!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string s = a.Text.Trim();
            for (int i = 0; i < 1000; i++)
            {
                TreeListNode tln = treeList1.FindNodeByID(i);
                if (tln == null)
                {
                    break;
                }
                if (tln.GetDisplayText("SubjectID") == s)
                {
                    focusedstate = 1;
                    treeList1.FocusedNode = tln;
                    focusedstate = 2;
                    XtraMessageBox.Show("科目代码重复。重复代码为：" + s, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            TreeListNode tl = treeList1.AppendNode(null, treeList1.FocusedNode.Id);
            tl.SetValue("SubjectName", b.Text.Trim());
            tl.SetValue("SubjectID", a.Text.Trim());
            treeList1.FocusedNode.Expanded = true;
            Clear();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (treeList1.FocusedNode != null)
            {
                treeList1.Nodes.Remove(treeList1.FocusedNode);
            }
        }

        private void BuildTreeParent(string site)
        {
            treeList1.ClearNodes();
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SiteName", site));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASSUBJECT", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                dsxm.Clear();
                dsxm = ds;
                treeList1.DataSource = dsxm.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        //hj20180821 获取默认模板
        private void BuildTreeParent1(string site)
        {
            treeList1.ClearNodes();
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SiteName", site));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASSUBJECT_1", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                dsxm.Clear();
                dsxm = ds;
                treeList1.DataSource = dsxm.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void Save()
        {
            try
            {

                string subjectIDs = "", subjectNames = "", subLevels = "", parentIds = "";
                string splitStr = "@";
                for (int i = 0; i < 1000; i++)
                {
                    TreeListNode tl = treeList1.FindNodeByID(i);
                    if (tl == null)
                    {
                        break;
                    }
                    subjectIDs += tl.GetValue("SubjectID").ToString() + splitStr;
                    subjectNames += tl.GetValue("SubjectName").ToString() + splitStr;
                    subLevels += tl.Level + splitStr;
                    parentIds += (tl.ParentNode == null ? "0" : tl.ParentNode.GetValue("SubjectID").ToString()) + splitStr;
                }

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SubjectIDs", subjectIDs));
                list.Add(new SqlPara("SubjectNames", subjectNames));
                list.Add(new SqlPara("SubLevels", subLevels));
                list.Add(new SqlPara("ParentIds", parentIds));
                list.Add(new SqlPara("SiteName", edbsite.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BASSUBJECT", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            treeList1.CollapseAll();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            treeList1.ExpandAll();
        }

        private void edbsite_SelectedIndexChanged(object sender, EventArgs e)
        {
            BuildTreeParent(edbsite.Text);
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            CopyToOtherCompany cp = new CopyToOtherCompany();
            cp.edbsite = edbsite.Text.Trim();
            cp.ShowDialog();
            #region
            //if (XtraMessageBox.Show("如果将此科目复制到其它分公司,将覆盖其它分公司原来的科目设置,确认是否继续?", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            //{
            //    try
            //    {
            //        List<SqlPara> list = new List<SqlPara>();
            //        list.Add(new SqlPara("SiteName", edbsite.Text.Trim()));
            //        SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BASSUBJECT_OtherCopy", list);
            //        if (SqlHelper.ExecteNonQuery(sps) > 0)
            //        {
            //            MsgBox.ShowOK();
            //            this.Close();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MsgBox.ShowException(ex);
            //    }
            //}
            #endregion
        }

        //获取所有科目id
        private string[] Get_All_kmid()
        {
            string s = "";
            foreach (TreeListNode tn in treeList1.Nodes)
            {
                Get_kmid(ref s, tn);
            }
            s = s.TrimEnd(',');
            return s.Split(',');
        }

        private void Get_kmid(ref string s, TreeListNode tn)
        {
            s += tn.GetValue("SubjectID").ToString() + ",";
            if (tn.Nodes.Count > 0)
            {
                foreach (TreeListNode tn1 in tn.Nodes)
                {
                    Get_kmid(ref s, tn1);
                }
            }
        }

        private void SetNodeState(string s)
        {
            foreach (TreeListNode tn in treeList1.Nodes)
            {
                abc(tn, s);
            }
        }

        private void abc(TreeListNode tl, string s)
        {
            DataRowView dr = (DataRowView)treeList1.GetDataRecordByNode(tl);
            if (dr.Row[0].ToString() == s)
            {
                treeList1.FocusedNode = tl;
                if (tl.ParentNode != null)
                {
                    tl.ParentNode.Expanded = true;
                }
            }
            if (tl.Nodes.Count == 0)
            {
                return;
            }
            foreach (TreeListNode tn in tl.Nodes)
            {
                abc(tn, s);
            }
        }

        /// <summary>
        /// 检测科目SubjectID是否重复
        /// </summary>
        private bool Check_KMID(ref string s)
        {
            string[] ArrKM = Get_All_kmid();
            List<string> list = new List<string>();
            for (int i = 0; i < ArrKM.Length; i++)
            {
                if (list.Contains(ArrKM[i]))
                {
                    s = ArrKM[i];
                    return true; //如果list中包含ArrKM[i],返回true
                }
                else
                {
                    list.Add(ArrKM[i]);
                }
            }
            return false;
        }

        private void treeList1_CustomDrawNodeCell(object sender, DevExpress.XtraTreeList.CustomDrawNodeCellEventArgs e)
        {
            int level = e.Node.Level;
            e.Appearance.ForeColor = level == 0 ? Color.Black : (level == 1 ? Color.Blue : (level == 2 ? Color.DarkOrange : (level == 3 ? Color.SkyBlue : Color.Green)));

        }

        private void treeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            if (focusedstate != 2 || treeList1.Nodes.Count <= 1) return;
            a.Text = e.Node.GetDisplayText(1);
            b.Text = e.Node.GetDisplayText(0);
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            BuildTreeParent1(edbsite.Text.Trim());
        }

    }
}