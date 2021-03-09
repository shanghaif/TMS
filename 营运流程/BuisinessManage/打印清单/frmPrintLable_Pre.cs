using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Collections;
using ZQTMS.Tool;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class frmPrintLable_Pre : BaseForm
    {
        public frmPrintLable_Pre()
        {
            InitializeComponent();
            //this.KeyPreview = true;
            //this.KeyDown += new KeyEventHandler(w_print_label_ex_KeyDown);
        }

        void frmPrintLable_Pre_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F12)
                {
                    simpleButton1.PerformClick();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void w_arrived_cofirm_Load(object sender, EventArgs e)
        {
            //edwebid.Text = commonclass.webid;
            //edbilldate.DateTime = commonclass.gcdate;
            //cc.BuildSelectSiteTreeEx(tvbsite);
            //tvbsite.Nodes.RemoveAt(tvbsite.Nodes.Count - 1);

            //#region 运输方式
            //string s = commonclass.transneedsort;
            //if (s != "" && s.Length > 0)
            //{
            //    string[] arr = s.Split(',');
            //    edaccneed.Properties.Items.Clear();
            //    for (int j = 0; j < arr.Length; j++)
            //    {
            //        edaccneed.Properties.Items.Add(arr[j].Trim());
            //    }
            //    edaccneed.Text = "";
            //    if (edaccneed.Properties.Items.Count > 0) edaccneed.SelectedIndex = 0;
            //}
            //#endregion
            CommonClass.SetSite(toSite, false);
            billdate.DateTime = CommonClass.gcdate;
            string[] TransitModeList = CommonClass.Arg.TransitMode.Split(',');
            if (TransitModeList.Length > 0)
            {
                for (int i = 0; i < TransitModeList.Length; i++)
                {
                    transMod.Properties.Items.Add(TransitModeList[i]);
                }
                transMod.SelectedIndex = 0;
            }
            string[] TransferModeList = CommonClass.Arg.TransferMode.Split(',');
            if (TransferModeList.Length > 0)
            {
                for (int i = 0; i < TransferModeList.Length; i++)
                {
                    TransferMod.Properties.Items.Add(TransferModeList[i]);
                }
                TransferMod.SelectedIndex = 0;
            }
            string[] CustomTypeModeList = CommonClass.Arg.CustomType.Split(',');
            if (CustomTypeModeList.Length > 0)
            {
                for (int i = 0; i < CustomTypeModeList.Length; i++)
                {
                    CustomType.Properties.Items.Add(CustomTypeModeList[i]);
                }
                CustomType.SelectedIndex = 0;
            }
            begWeb.Text = CommonClass.UserInfo.SiteName;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (toSite.Text.Trim() == "" || middlesite.Text.Trim() == "" || billno.Text.Trim() == ""

                || transMod.Text.Trim() == "" || ConsigneeName.Text.Trim() == ""
                || package.Text.Trim() == "" || TransferMod.Text.Trim() == "" || toWebName.Text.Trim() == "")
            {
                MsgBox.ShowOK("每一项都必须填写，其中货号必须包含件数，请检查!");
                billno.Focus();
                return;
            }

            if (ConvertType.ToDecimal(FeeWeight.Text.Trim()) == 0 || ConvertType.ToInt32(num.Text.Trim()) == 0)
            {
                MsgBox.ShowOK("每一项都必须填写，请检查!");
                num.Focus();
                return;
            }
            //DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataColumn dc = null;

            dc = dt.Columns.Add("BillNo", Type.GetType("System.String"));
            dc = dt.Columns.Add("NowSite", Type.GetType("System.String"));
            dc = dt.Columns.Add("DestinationSite", Type.GetType("System.String"));
            dc = dt.Columns.Add("TransferSite", Type.GetType("System.String"));
            dc = dt.Columns.Add("Varieties", Type.GetType("System.String"));
            dc = dt.Columns.Add("Num", Type.GetType("System.String"));
            dc = dt.Columns.Add("TransitMode", Type.GetType("System.String"));
            dc = dt.Columns.Add("ConsigneeName", Type.GetType("System.String"));
            dc = dt.Columns.Add("TransferMode", Type.GetType("System.String"));
            dc = dt.Columns.Add("BillDate", Type.GetType("System.String"));
            dc = dt.Columns.Add("BegWeb", Type.GetType("System.String"));
            dc = dt.Columns.Add("Package", Type.GetType("System.String"));
            dc = dt.Columns.Add("FeeWeight", Type.GetType("System.String"));
            dc = dt.Columns.Add("FeeVolume", Type.GetType("System.String"));
            dc = dt.Columns.Add("ReceivArea", Type.GetType("System.String"));
            dc = dt.Columns.Add("ReceivStreet", Type.GetType("System.String"));
            dc = dt.Columns.Add("PickGoodsSite", Type.GetType("System.String"));
            dc = dt.Columns.Add("CusType", Type.GetType("System.String"));
            dc = dt.Columns.Add("MiddleSend", Type.GetType("System.String"));
            dc = dt.Columns.Add("W_V", Type.GetType("System.String"));
            dc = dt.Columns.Add("LabelSeq", Type.GetType("System.String"));
            dc = dt.Columns.Add("Dzly", Type.GetType("System.String"));
            dc = dt.Columns.Add("BillnoNum", Type.GetType("System.String"));
            dc = dt.Columns.Add("Tjweight", Type.GetType("System.String"));
            dc = dt.Columns.Add("Nowsites", Type.GetType("System.String"));
            dc = dt.Columns.Add("BegWebTel", Type.GetType("System.String"));//网点客服电话
            DataRow newRow;
            newRow = dt.NewRow();
            newRow["BillNo"] = billno.Text.Trim();
            //newRow["StartSite"] = "";
            newRow["DestinationSite"] = toSite.Text.Trim();
            newRow["TransferSite"] = middlesite.Text.Trim();
            newRow["Varieties"] = Varieties.Text.Trim();
            newRow["Num"] = num.Text.Trim();
            newRow["TransitMode"] = transMod.Text.Trim();
            newRow["ConsigneeName"] = ConsigneeName.Text.Trim();
            newRow["TransferMode"] = TransferMod.Text.Trim();
            newRow["BillDate"] =Convert.ToDateTime( billdate.Text.Trim()).ToShortDateString();
            newRow["BegWeb"] = begWeb.Text.Trim();
            newRow["Package"] = package.Text.Trim();
            newRow["FeeWeight"] = FeeWeight.Text.Trim();
            newRow["FeeVolume"] = "";
            newRow["NowSite"] = CommonClass.UserInfo.WebName.ToString();//该网点名称
            newRow["ReceivStreet"] = "";
            newRow["PickGoodsSite"] = toWebName.Text.Trim();
            newRow["CusType"] = CustomType.Text.Trim();
            newRow["MiddleSend"] = middlesite.Text.Trim();
            newRow["W_V"] = "";
            newRow["LabelSeq"] = "";
            newRow["Dzly"] = middlesite.Text.Trim() + "--" + toSite.Text.Trim();
            newRow["BillnoNum"] = billno.Text.Trim() +"-"+ num.Text.Trim();
            newRow["Tjweight"] =FeeWeight.Text.Trim()+"/0";
            newRow["NowSites"] = CommonClass.UserInfo.WebName.ToString() + "--";//该网点名称
            newRow["BegWebTel"] = "";//客服网点电话
            dt.Rows.Add(newRow);
            //ds.Tables.Add(dt);
            frmPrintLabelDev fpld = new frmPrintLabelDev(dt);
            fpld.ShowDialog();
            //锐浪打印标签
            //frmPrintLabel frm = new frmPrintLabel(billno.Text.Trim(), ds);
            //frm.ShowDialog();
            this.Close();
        }

        private void tvbsite_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //if (e.Action == TreeViewAction.Collapse || e.Action == TreeViewAction.Expand || e.Action == TreeViewAction.Unknown) return;
            //if (e.Node.Parent != null)
            //{
            //    edesite.Text = e.Node.Text;

            //    edmiddlesite.Text = GetRootNodeText(e.Node);
            //}
            //else
            //{
            //    edesite.Text = e.Node.Text;
            //    edmiddlesite.Text = e.Node.Text;
            //}
            //for (int i = 0; i < e.Node.Nodes.Count; i++)
            //{
            //    if (e.Node.Nodes[i].Text == e.Node.Text)
            //    {//如果中转地中有和到站一样的名字，经由也显示
            //        edmiddlesite.Text = e.Node.Nodes[i].Text;
            //        break;
            //    }
            //}

            //if (e.Node.Nodes.Count > 0)
            //{
            //    edesite.Text = "";
            //}
            //else
            //{
            //    edesite.Text = e.Node.Text;
            //}
        }


        private string GetRootNodeText(TreeNode node)
        {
            //string txt = "";
            //if (node.Parent != null)
            //{
            //    if (node.Parent.Parent != null)
            //    {
            //        txt = node.Parent.Parent.Text;
            //        if (node.Parent.Parent.Parent != null)
            //        {
            //            txt = node.Parent.Parent.Parent.Text;
            //        }
            //        else
            //        {
            //            txt = node.Parent.Parent.Text;
            //        }
            //    }
            //    else
            //    {
            //        txt = node.Parent.Text;
            //    }
            //}
            //else
            //{
            //    txt = node.Text;
            //}
            //return txt;
            return "";
        }

        private void tvbsite_Click(object sender, EventArgs e)
        {
            //if (tvbsite.SelectedNode != null)
            //{
            //    if (tvbsite.SelectedNode.Parent != null)
            //    {
            //        edesite.Text = tvbsite.SelectedNode.Text;
            //        edmiddlesite.Text = tvbsite.SelectedNode.Parent.Text;
            //    }
            //    else
            //    {
            //        edesite.Text = tvbsite.SelectedNode.Text;
            //        edmiddlesite.Text = tvbsite.SelectedNode.Text;
            //    }
            //}
        }

        private void tvbsite_DoubleClick(object sender, EventArgs e)
        {
            toSite.ClosePopup();
        }

        private void tvbsite_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                toSite.Text = tvbsite.SelectedNode.Text;
                toSite.ClosePopup();
            }
        }

        private void edesite_EditValueChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(toWebName, toSite.Text.Trim(), false);
        }

        private string findesite(string ineasycode)
        {
            //string easycode = "";
            //string cname = "";
            //try
            //{
            //    for (int i = 0; i < commonclass.dsBsite.Tables[0].Rows.Count; i++)
            //    {
            //        cname = commonclass.dsBsite.Tables[0].Rows[i]["bsite"].ToString().Trim();
            //        easycode = commonclass.getPY(cname);
            //        if (easycode == ineasycode) return cname;
            //    }
            //    for (int i = 0; i < commonclass.dsMiddleSite.Tables[0].Rows.Count; i++)
            //    {
            //        cname = commonclass.dsMiddleSite.Tables[0].Rows[i]["site"].ToString().Trim();
            //        easycode = commonclass.getPY(cname);
            //        if (easycode == ineasycode) return cname;
            //    }
            //}
            //catch (Exception)
            //{
            //}
            //return ineasycode;
            return "";
        }

        private string getmiddlesite(string esite)
        {
            //DataRow[] dr = commonclass.dsMiddleSite.Tables[0].Select("site='" + esite + "'");
            //if (dr.Length > 0)
            //{
            //    return dr[0]["parentsite"].ToString();
            //}
            //return commonclass.gbsite;
            return "";
        }

        private void edbillno_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void edmiddlesite_EditValueChanged(object sender, EventArgs e)
        {
            //cc.FillRepWebByParent(edewebid, edmiddlesite.Text.Trim(), false);
            //if (commonclass.dsWeb.Tables.Count > 0)
            //{
            //    DataRow[] dr = commonclass.dsWeb.Tables[0].Select(string.Format("parentsite='{0}' and isdefault=1", edmiddlesite.Text.Trim())); //找默认网点
            //    if (dr.Length > 0)
            //    {
            //        edewebid.EditValue = dr[0]["site"];
            //    }
            //}

            gettydeweb();
        }

        private void gettydeweb()
        {
            ////找二级中转地(到站)是否设置了开单目的网点。和经由地及交接方式有关
            //if (edokprocess.Text.Trim() == "")
            //{
            //    edewebid.Text = "";
            //    edewebid.Properties.Items.Clear();
            //    return;
            //}
            //string site3 = "", site4 = "";
            //DataRow[] rows = null;
            //string esite = edesite.Text.Trim();
            //if (esite != "")
            //{
            //    TreeNode[] nodes = tvbsite.Nodes.Find(esite, true);
            //    if (nodes.Length > 0)
            //    {
            //        if (nodes[0].Level == 0) //0级节点 即直达的
            //        {
            //            rows = commonclass.dsMiddleSite.Tables[0].Select(string.Format("parentsite='{0}' and site='{1}'", nodes[0].Text, esite));
            //        }
            //        if (nodes[0].Level == 1 && nodes[0].Nodes.Count == 0) //一级节点，并且没有子节点，就说明没有2级和3级节点，即：没有三级和四级机构，用二级中转地的
            //        {
            //            rows = commonclass.dsMiddleSite.Tables[0].Select(string.Format("parentsite='{0}' and site='{1}'", nodes[0].Parent.Text, esite));
            //        }
            //        if (nodes[0].Level == 2)
            //        {
            //            site3 = nodes[0].Text;
            //            rows = commonclass.dsMiddleSite.Tables[1].Select(string.Format("parentsite='{0}' and site3='{1}'", nodes[0].Parent.Text, site3));
            //        }
            //        if (nodes[0].Level == 3)
            //        {
            //            site3 = nodes[0].Parent.Text;
            //            site4 = nodes[0].Text;
            //            rows = commonclass.dsMiddleSite.Tables[1].Select(string.Format("parentsite='{0}' and site3='{1}' and site4='{2}'", nodes[0].Parent.Parent.Text, site3, site4));
            //        }
            //    }
            //}

            //string tydewebid = "";
            //if (rows != null && rows.Length > 0)
            //{
            //    string field = "tydewebid";//自提的目的网点字段
            //    if (edokprocess.Text.Trim() != "自提")
            //    {
            //        field = "tydewebidsend";//送货的目的网点字段
            //    }
            //    tydewebid = rows[0][field] == DBNull.Value ? "" : rows[0][field].ToString();
            //    if (tydewebid.Trim() != "")
            //    {
            //        edewebid.Text = "";
            //        string[] arr = tydewebid.Trim().Split(',');
            //        edewebid.Properties.Items.Clear();
            //        foreach (string item in arr)
            //        {
            //            edewebid.Properties.Items.Add(item.Trim());
            //        }
            //    }
            //}
            //if ((rows != null && rows.Length == 0) || tydewebid == "")
            //{
            //    edewebid.Text = "";
            //    cc.FillRepWebByParent(edewebid, edmiddlesite.Text.Trim(), false);
            //}
            //if (commonclass.dsWeb.Tables.Count > 0)
            //{
            //    DataRow[] dr = commonclass.dsWeb.Tables[0].Select(string.Format("parentsite='{0}' and isdefault=1", edmiddlesite.Text.Trim())); //找默认网点
            //    if (dr.Length > 0 && edewebid.Properties.Items.Contains(dr[0]["site"].ToString()))
            //    {
            //        edewebid.EditValue = dr[0]["site"];
            //    }
            //}
            //if (edewebid.Properties.Items.Count == 1) edewebid.SelectedIndex = 0;
        }

        private void edokprocess_EditValueChanged(object sender, EventArgs e)
        {
            toWebName.Enabled = TransferMod.Text.Trim() != "";
            gettydeweb();
        }
    }
}