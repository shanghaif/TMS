using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraEditors.Controls;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class frmBaseRegion : BaseForm
    {
        public frmBaseRegion()
        {
            InitializeComponent();
        }
        DataSet dsRegion = new DataSet();

        private void frmBaseRegion_Load(object sender, EventArgs e)
        {
            BarMagagerOper.SetBarPropertity(bar1);
            GetRegionData(); 
        }

        private bool GetRegionData()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASEREGION", list);
                dsRegion = SqlHelper.GetDataSet(sps);

                if (dsRegion == null || dsRegion.Tables.Count == 0) return true;
                treeList1.DataSource = dsRegion.Tables[0];
                return true;
            }
            catch (Exception ex)
            {
                MsgBox.ShowError("数据加载失败：\r\n" + ex.Message);
                return false;
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)//添加一级
        { 
            RegionName.Focus();
            TreeListNode node = treeList1.Nodes.LastNode;
            RegionGUID.Text = Guid.NewGuid().ToString();

            RegionName.Text = ""; 
            ParentID.Text = "";
            RegionLevel.EditValue = 1;
            ParentName.Text = "";
            RegionShortName.Text = "";
            RegionState.Checked = true;
 
            RegionID.Text = GetNewID(1) + "";
           // RegionID.EditValue = Convert.ToInt32(node.GetValue("RegionID")) + 1;
            //MenuOrder.EditValue = Convert.ToInt32(node.GetValue("MenuOrder")) + 1;
        }

        private int GetNewID(int level)
        {
            int result = 0;
            TreeListNode node = treeList1.FocusedNode;
            int _regionID = 0;

            if (level == int.Parse(node.GetValue("RegionLevel").ToString()))
                _regionID = int.Parse(node.GetValue("ParentID").ToString());
            else
                _regionID = int.Parse(node.GetValue("RegionID").ToString()); 

            try
            {
                result = int.Parse(dsRegion.Tables[0].Select(string.Format("ParentID={0}", _regionID), "RegionID DESC")[0]["RegionID"] + "");
                result = result + 1;
            }
            catch
            {
                switch (level)
                {
                    case 1:
                        result = 11;
                        break;
                    case 2:
                        result = int.Parse(_regionID + "01");
                        break;
                    case 3:
                        result = int.Parse(_regionID + "001");
                        break;
                    case 4:
                        result = int.Parse(_regionID + "01");
                        break;
                    default: break;
                }
            }
            return result;
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)//新增同级
        {
            RegionName.Focus();
            TreeListNode node = treeList1.FocusedNode;

            if (node == null)
            {
                MsgBox.ShowOK("没有选择区域，无法添加同级区域!");
                return;
            }

            RegionGUID.Text = Guid.NewGuid().ToString();
            RegionName.Text = "";

            RegionID.EditValue = GetNewID((int)node.GetValue("RegionLevel")) + "";

            ParentName.Properties.Items.Clear();

            if (node.ParentNode != null)
            {

                ParentName.Properties.Items.Add(new ImageComboBoxItem(node.ParentNode.GetValue("RegionName").ToString(), node.ParentNode.GetValue("RegionID").ToString()));

                ParentName.SelectedIndex = 0;

                ParentID.Text = node.ParentNode.GetValue("RegionID").ToString();
            }
            else//没有父级，说明此时是根区域
            { 
                ParentID.Text = "";
            }
  
            RegionLevel.EditValue = node.GetValue("RegionLevel");

            RegionShortName.Text = "";
            RegionRemark.Text = "";

            RegionState.Checked = true;

        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) //新增下级
        {
            RegionName.Focus();
            TreeListNode node = treeList1.FocusedNode;

            if (node == null)
            {
                MsgBox.ShowOK("没有选择区域，无法添加下级区域!");
                return;
            }

            if (node.GetValue("RegionLevel").ToString().Trim() == "4")
            { 
                MsgBox.ShowOK("最多只能有四级，无法添加下级区域!");
                return;
            }


            RegionGUID.Text = Guid.NewGuid().ToString(); 
            RegionName.Text = "";

            ParentName.Properties.Items.Clear();
            ParentName.Properties.Items.Add(new ImageComboBoxItem(node.GetValue("RegionName").ToString(), node.GetValue("RegionID").ToString())); 
            ParentName.SelectedIndex = 0;
            
            ParentID.Text = node.GetValue("RegionID").ToString();
 
            RegionID.EditValue = GetNewID((int)node.GetValue("RegionLevel")+1) + "";
 
            RegionLevel.EditValue = Convert.ToInt32(node.GetValue("RegionLevel")) + 1;
  
            RegionShortName.Text = "";

            RegionRemark.Text = "";
            RegionState.Checked = true; 
 
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)//修改
        {
            try
            {
                TreeListNode node = treeList1.FocusedNode;
                if (node == null) return;

                DataRowView dr = treeList1.GetDataRecordByNode(node) as DataRowView;

                ParentName.Properties.Items.Clear();

                if (node.ParentNode != null)
                {
                    ParentName.Properties.Items.Add(new ImageComboBoxItem(node.ParentNode.GetValue("RegionName").ToString(), node.ParentNode.GetValue("RegionID").ToString()));
                    ParentName.SelectedIndex = 0;
                }   

                RegionGUID.EditValue = dr["RegionGUID"];
                RegionState.Checked = Convert.ToInt32(dr["RegionState"]) == 1;
                RegionID.EditValue = dr["RegionID"];
                ParentID.EditValue = dr["ParentID"];
                RegionLevel.EditValue = dr["RegionLevel"]; 
                RegionName.EditValue = dr["RegionName"];
                RegionRemark.EditValue = dr["RegionRemark"];
                lon.EditValue = dr["lon"];
                lat.EditValue = dr["lat"];
                RegionShortName.EditValue = dr["RegionShortName"];  
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) //删除
        {
            try
            {
                TreeListNode node = treeList1.FocusedNode;
                if (node == null) return;

                if (MsgBox.ShowYesNo("是否删除？同时将删除对应的子区域！\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                } 
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("RegionID", node.GetValue("RegionID")));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_BASEREGION", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();

                    treeList1.PostEditor();
                    treeList1.DeleteNode(node);
                    dsRegion.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        { 
            GetRegionData();
        }


        private void AddRow(DataRow dr, string field, object value)
        {
            if (dr.Table.Columns.Contains(field))
            {
                dr[field] = value;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                #region 基本信息监测
                string _RegionGUID = RegionGUID.Text.Trim();
                if (_RegionGUID == "")
                {
                    MsgBox.ShowOK("系统编号不能为空!");
                    RegionGUID.Focus();
                    return;
                }

                int _RegionState = RegionState.Checked ? 1 : 0;

                int _RegionID = RegionID.Text.Trim() == "" ? 0 : Convert.ToInt32(RegionID.Text.Trim());
                if (_RegionID == 0)
                {
                    MsgBox.ShowOK("区域编号值不能为空!");
                    RegionID.Focus();
                    return;
                }

                int _ParentID = ParentID.Text.Trim() == "" ? 0 : Convert.ToInt32(ParentID.Text.Trim());


                int _RegionLevel = RegionLevel.Text.Trim() == "" ? 0 : Convert.ToInt32(RegionLevel.Text.Trim());
                if (_RegionLevel == 0)
                {
                    MsgBox.ShowOK("区域级别值不能为空!");
                    RegionLevel.Focus();
                    return;
                }


                string _RegionName = RegionName.Text.Trim();
                if (_RegionName == "")
                {
                    MsgBox.ShowOK("区域名称值不能为空!");
                    RegionName.Focus();
                    return;
                }

                string _ParentName = ParentName.Text.Trim();

                string _RegionShortName = RegionShortName.Text.Trim();
                string _RegionRemark = RegionRemark.Text.Trim();

                string _lon = string.IsNullOrEmpty(lon.Text.Trim()) ? "0" : lon.Text.Trim();
                string _lat = string.IsNullOrEmpty(lat.Text.Trim()) ? "0" : lat.Text.Trim();
         
                
                //if (!int.Equals(_RegionLevel * 2, _RegionID.ToString().Length))
                //{
                //    MsgBox.ShowOK("区域编号位数和区域级别不符：\r\n每一级区域编号增加两位数!");
                //    RegionID.Focus();
                //    return;
                //}
                #endregion

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("RegionGUID", _RegionGUID));
                list.Add(new SqlPara("ParentID", _ParentID));
                list.Add(new SqlPara("RegionID", _RegionID));
                list.Add(new SqlPara("ParentName", _ParentName));
                list.Add(new SqlPara("RegionName", _RegionName));
                list.Add(new SqlPara("RegionLevel", _RegionLevel));
                list.Add(new SqlPara("RegionShortname", _RegionShortName));
                list.Add(new SqlPara("RegionState", _RegionState));
                list.Add(new SqlPara("lon", _lon));
                list.Add(new SqlPara("lat", _lat));
                list.Add(new SqlPara("RegionRemark", _RegionRemark)); 

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BASEREGION", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();

                    DataRow dr = dsRegion.Tables[0].NewRow();

                    DataRow[] rows = dsRegion.Tables[0].Select(string.Format("RegionGUID='{0}'", _RegionGUID));
                    if (rows.Length > 0)
                    {
                        dr = rows[0];
                    }

                    AddRow(dr, "RegionGUID", _RegionGUID);
                    AddRow(dr, "RegionState", _RegionState);
                    AddRow(dr, "RegionID", _RegionID);
                    AddRow(dr, "ParentID", _ParentID);
                    AddRow(dr, "RegionLevel", _RegionLevel);
                    AddRow(dr, "RegionShortName", _RegionShortName);
                    AddRow(dr, "RegionName", _RegionName);
                    AddRow(dr, "RegionRemark", _RegionRemark);
                    AddRow(dr, "ParentName", _ParentName);
                    AddRow(dr, "lon", _lon);
                    AddRow(dr, "lat", _lat); 

                    if (rows.Length == 0)
                    {
                        dsRegion.Tables[0].Rows.Add(dr);
                    }
                    dsRegion.AcceptChanges();
                    Clear();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }


        private void Clear()
        {
            RegionGUID.Text = Guid.NewGuid().ToString();
            RegionState.Checked = true;
            RegionID.Text = "";
            //ParentID.Text = "";
            RegionLevel.Text = "";
            RegionShortName.Text = "";
            RegionName.Text = "";
            RegionRemark.Text = "";
            ParentID.Text = "";
            ParentName.Properties.Items.Clear();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void ParentName_SelectedIndexChanged(object sender, EventArgs e)
        { 
            ParentID.EditValue = ((ImageComboBoxItem)ParentName.SelectedItem).Value;
        }

        private void myGridControl1_Click(object sender, EventArgs e)
        {

        }
    }
}
