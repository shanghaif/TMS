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
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class w_department : BaseForm
    {

        public w_department()
        {
            InitializeComponent();
        }
 
        private void w_items_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            BuildTreeParent();
        }

        private void toolStripButton22_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton21_Click(object sender, EventArgs e)
        {
           // cc.QuickSearch();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            //cc.QuickSearch();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           // getitem();
        }

        private void Clear()
        {
            a.Text = "";
            b.Text = "";

        }
        private void simpleButton1_Click( object sender , EventArgs e )
        {
            if ( a.Text.Trim() == "" || b.Text.Trim() == "" )
            {
                XtraMessageBox.Show("部门代码和名称必须填写","系统提示",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
 
            TreeNode tn = new TreeNode();
            tn.Text = b.Text.Trim();
            tn.Tag = a.Text.Trim();
            tn.ImageIndex = 0;
            tn.ForeColor = Color.Black;
            tn.Name = edname.Text.Trim( );
            tv.Nodes.Add( tn );
            Clear();
        }

        private void simpleButton2_Click( object sender , EventArgs e )
        {
            TreeNode tn = new TreeNode();
            tn.Text = b.Text.Trim();
            tn.Tag = a.Text.Trim();
            tn.Name = edname.Text.Trim( );
            
            if (  tv.SelectedNode != null && tv.SelectedNode.Level < 3  )
            {
                Clear();                 
                tv.SelectedNode.Nodes.Add( tn );
                tn.Parent.Expand();
            }
            else
            {
                XtraMessageBox.Show( "必须选定一个上级科目" , "系统提示" , MessageBoxButtons.OK , MessageBoxIcon.Stop );
            }
        }

        private void simpleButton3_Click( object sender , EventArgs e )
        {            
            if(tv.SelectedNode!=null)
            tv.SelectedNode.Remove();            
        }

        private DataSet GetKm(int level)
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("level", level));
            list.Add(new SqlPara("bsite",CommonClass.UserInfo.SiteName));
            
            //SqlCommand sq = new SqlCommand( "QSP_GET_BM_BY_LEVEL" );
            //DataSet dsxm = new DataSet();
            //sq.CommandType = CommandType.StoredProcedure;
            
            //sq.Parameters.Add( new SqlParameter("@level",SqlDbType.Int) );
            //sq.Parameters.Add( new SqlParameter( "@bsite", SqlDbType.VarChar ) );
            
            //sq.Parameters[ 0 ].Value = level;
            //sq.Parameters [ 1 ].Value = commonclass.gbsite;


            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BM_BY_LEVEL", list);
            DataSet ds = SqlHelper.GetDataSet(sps);
            return ds;
        }

        private void BuildTreeParent ( )
        {
            string kmmc = "";
            string tag = "";
            string Parentid = "";
            string name = "";
            DataSet dstreeParent = GetKm( 0 );
            DataSet dstreeChild1 = GetKm( 1 );
            DataSet dstreeChild2 = GetKm( 2 );
            DataSet dstreeChild3 = GetKm( 3 );


            DataTable tParent = dstreeParent.Tables [ 0 ];
            DataTable tChild1 = dstreeChild1.Tables [ 0 ];
            DataTable tChild2 = dstreeChild2.Tables [ 0 ];
            DataTable tChild3 = dstreeChild3.Tables [ 0 ];

            tv.Nodes.Clear( );

            for ( int i = 0 ; i < tParent.Rows.Count ; i++ )
            {
                kmmc = tParent.Rows [ i ] [ "kmmc" ].ToString( );
                tag = tParent.Rows [ i ] [ "ID" ].ToString( );
                name = tParent.Rows [ i ] [ "withdo" ].ToString( ).Trim( );
                TreeNode tn = new TreeNode( );
                tn.Text = kmmc;
                tn.Tag = tag;
                tn.ImageIndex = 0;
                tn.Name = name;
                tv.Nodes.Add( tn );
                for ( int j = 0 ; j < tChild1.Rows.Count ; j++ )
                {

                    Parentid = tChild1.Rows [ j ] [ "ParentID" ].ToString( );

                    if ( Parentid == tn.Tag.ToString( ) )
                    {
                        kmmc = tChild1.Rows [ j ] [ "kmmc" ].ToString( );
                        tag = tChild1.Rows [ j ] [ "ID" ].ToString( );
                        name = tChild1.Rows [ j ] [ "withdo" ].ToString( ).Trim( );
           
                        TreeNode tn1 = new TreeNode( );
                        tn1.Text = kmmc;
                        tn1.Tag = tag;
                        tn1.Name = name;
                        tn1.ImageIndex = 0;
                        tn1.ForeColor = Color.Blue;
                        tn.Nodes.Add( tn1 );
                        for ( int k = 0 ; k < tChild2.Rows.Count ; k++ )
                        {
                            Parentid = tChild2.Rows [ k ] [ "ParentID" ].ToString( );

                            if ( Parentid == tn1.Tag.ToString( ) )
                            {
                                kmmc = tChild2.Rows [ k ] [ "kmmc" ].ToString( );
                                tag = tChild2.Rows [ k ] [ "ID" ].ToString( );
                                name = tChild2.Rows [ k ] [ "withdo" ].ToString( ).Trim( );
           
                                TreeNode tn2 = new TreeNode( );
                                tn2.Text = kmmc;
                                tn2.Tag = tag;
                                tn2.Name = name;
                                tn2.ImageIndex = 0;
                                tn2.ForeColor = Color.Red;
                                tn1.Nodes.Add( tn2 );
                                for ( int m = 0 ; m < tChild3.Rows.Count ; m++ )
                                {
                                    Parentid = tChild3.Rows [ m ] [ "ParentID" ].ToString( );

                                    if ( Parentid == tn2.Tag.ToString( ) )
                                    {
                                        kmmc = tChild3.Rows [ m] [ "kmmc" ].ToString( );
                                        tag = tChild3.Rows [ m ] [ "ID" ].ToString( );
                                        name = tChild3.Rows [ m ] [ "withdo" ].ToString( ).Trim( );
           
                                        TreeNode tn3 = new TreeNode( );
                                        tn3.Text = kmmc;
                                        tn3.Tag = tag;
                                        tn3.Name = name;
                                        tn3.ImageIndex = 0;
                                        tn3.ForeColor = Color.SkyBlue;
                                        tn2.Nodes.Add( tn3 );

                                    }


                                }
                            }
                        }
                    }

                }

            }
        }

        private void Save()
        {



            //项目名称 kmid , kmmc , pid, thelevel, parentid


            string kmmc , pid;
            int thelevel = 0 , parentid = 0 , kmid = 0;

            

            
            try
            {
                //删除原来的科目设置
                //SqlCommand sqdelete = new SqlCommand( "USP_DELETE_BM"  );
                //sqdelete.CommandType = CommandType.StoredProcedure;
                   
                //cs.ENQ(sqdelete);
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_BM");
                SqlHelper.ExecteNonQuery(sps); 
                //保存新的
                //SqlCommand sq = new SqlCommand( "USP_ADD_BM"  );

                //sq.Parameters.Add( new SqlParameter( "@kmid" , SqlDbType.Int ) );
                //sq.Parameters.Add( new SqlParameter( "@kmmc" , SqlDbType.VarChar ) );
                //sq.Parameters.Add( new SqlParameter( "@pid" , SqlDbType.VarChar ) );
                //sq.Parameters.Add( new SqlParameter( "@thelevel" , SqlDbType.Int ) );
                //sq.Parameters.Add( new SqlParameter( "@parentid" , SqlDbType.Int ) );
                //sq.Parameters.Add( new SqlParameter( "@withdo", SqlDbType.VarChar ) );
                //sq.Parameters.Add( new SqlParameter( "@bsite", SqlDbType.VarChar ) );

                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps1 = new SqlParasEntity(OperType.Execute, "USP_ADD_BASAREA", list);
                foreach ( TreeNode tn in tv.Nodes )  //level 0 
                {
                    kmid = int.Parse( tn.Tag.ToString() );
                    kmmc = tn.Text;
                    pid = "";
                    thelevel = 0;
                    parentid = 0;
                    //sq.Parameters[ 0 ].Value = kmid;
                    //sq.Parameters[ 1 ].Value = kmmc;
                    //sq.Parameters[ 2 ].Value = pid;
                    //sq.Parameters[ 3 ].Value = thelevel;
                    //sq.Parameters[ 4 ].Value = parentid;
                    //sq.Parameters [ 5 ].Value = tn.Name.ToString();
                    //sq.Parameters [ 6 ].Value = commonclass.gbsite;
                    list.Clear();
                    list.Add(new SqlPara("kmid", kmid));
                    list.Add(new SqlPara("kmmc", kmmc));
                    list.Add(new SqlPara("pid", pid));
                    list.Add(new SqlPara("thelevel", thelevel));
                    list.Add(new SqlPara("parentid", parentid));
                    list.Add(new SqlPara("withdo", tn.Name.ToString()));
                    list.Add(new SqlPara("bsite", CommonClass.UserInfo.SiteName));


                    SqlHelper.ExecteNonQuery(sps1);

                    foreach ( TreeNode tn1 in tn.Nodes ) //level 1
                    {
                        kmid = int.Parse( tn1.Tag.ToString() );
                        kmmc = tn1.Text;
                        pid = tn.Tag.ToString();
                        thelevel = 1;
                        parentid = int.Parse( tn.Tag.ToString() ); ;
                        list.Clear();
                        list.Add(new SqlPara("kmid", kmid));
                        list.Add(new SqlPara("kmmc", kmmc));
                        list.Add(new SqlPara("pid", pid));
                        list.Add(new SqlPara("thelevel", thelevel));
                        list.Add(new SqlPara("parentid", parentid));
                        list.Add(new SqlPara("withdo", tn.Name.ToString()));
                        list.Add(new SqlPara("bsite", CommonClass.UserInfo.SiteName));


                        SqlHelper.ExecteNonQuery(sps1);

                        foreach ( TreeNode tn2 in tn1.Nodes ) //level 2
                        {
                            kmid = int.Parse( tn2.Tag.ToString() );
                            kmmc = tn2.Text;
                            pid = tn1.Tag.ToString();
                            thelevel = 2;
                            parentid = int.Parse( tn1.Tag.ToString() );
                            list.Clear();
                            list.Add(new SqlPara("kmid", kmid));
                            list.Add(new SqlPara("kmmc", kmmc));
                            list.Add(new SqlPara("pid", pid));
                            list.Add(new SqlPara("thelevel", thelevel));
                            list.Add(new SqlPara("parentid", parentid));
                            list.Add(new SqlPara("withdo", tn.Name.ToString()));
                            list.Add(new SqlPara("bsite", CommonClass.UserInfo.SiteName));


                            SqlHelper.ExecteNonQuery(sps1);


                                foreach ( TreeNode tn3 in tn2.Nodes ) //level 2
                                {
                                    kmid = int.Parse( tn3.Tag.ToString( ) );
                                    kmmc = tn3.Text;
                                    pid = tn3.Tag.ToString( );
                                    thelevel = 3;
                                    parentid = int.Parse( tn2.Tag.ToString( ) );
                                    list.Clear();
                                    list.Add(new SqlPara("kmid", kmid));
                                    list.Add(new SqlPara("kmmc", kmmc));
                                    list.Add(new SqlPara("pid", pid));
                                    list.Add(new SqlPara("thelevel", thelevel));
                                    list.Add(new SqlPara("parentid", parentid));
                                    list.Add(new SqlPara("withdo", tn.Name.ToString()));
                                    list.Add(new SqlPara("bsite", CommonClass.UserInfo.SiteName));
                                    SqlHelper.ExecteNonQuery(sps1);


                                }
                        }
                    }

                }

                   
                XtraMessageBox.Show( "已经保存部门设置。" , "系统提示" , MessageBoxButtons.OK , MessageBoxIcon.Information );
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show( ex.Message , "系统提示" , MessageBoxButtons.OK , MessageBoxIcon.Stop );
            }
            finally
            {   }
        }
       

       

        private void tv_AfterSelect( object sender , TreeViewEventArgs e )
        {
            a.Text = e.Node.Tag.ToString();
            b.Text = e.Node.Text.ToString();
            edname.Text = e.Node.Name.ToString( );
        }

        private void simpleButton6_Click( object sender , EventArgs e )
        {
            Save();
        }

        private void simpleButton7_Click( object sender , EventArgs e )
        {
            this.Close();
        }

        private void simpleButton4_Click( object sender , EventArgs e )
        {
          
            tv.CollapseAll();
        }

        private void simpleButton5_Click( object sender , EventArgs e )
        {
            tv.ExpandAll();
        }

      
    }
}