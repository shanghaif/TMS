using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class w_select_department : BaseForm
    {

        //private commonclass cc = new commonclass();Cls_SqlHelper cs = new Cls_SqlHelper();

        public w_select_department()
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

    
        private DataSet GetKm(int level)
        {

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("level", level));
            list.Add(new SqlPara("bsite", CommonClass.UserInfo.SiteName));
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

     
       

        private void simpleButton7_Click( object sender , EventArgs e )
        {
            this.Close();
        }

        private void tv_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //if (e.Node != null)
            //{
            //    if (e.Node.Parent != null)
            //    {
            //        commonclass.bmfgs= e.Node.Parent.Text;
            //        commonclass.bmbm = e.Node.Text;
            //    }
            //    else
            //    {
            //       commonclass.bmfgs= e.Node.Text;
            //      commonclass.bmbm= "";
            //    }
            //}
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tv_DoubleClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

      
    }
}