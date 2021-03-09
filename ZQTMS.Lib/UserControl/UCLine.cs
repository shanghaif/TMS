using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Windows.Forms.Design;
using System.Collections;

namespace ZQTMS.Lib
{
    [Designer(typeof(UserControlDesigner))]//为控件指定设计器
    public partial class UCLine : DevExpress.XtraEditors.XtraUserControl
    {
        public UCLine()
        {
            InitializeComponent();
        }
    }


    class UserControlDesigner : System.Windows.Forms.Design.ControlDesigner
    {
        public UserControlDesigner()
            : base()
        {
        }

        /// <summary> 
        /// 重载SelectionRules属性自定义选择规则 
        /// </summary> 
        public override SelectionRules SelectionRules
        {
            get
            {
                SelectionRules selectionRules = SelectionRules.Moveable | SelectionRules.Visible | SelectionRules.AllSizeable;
                return selectionRules;
            }
        }

        //想要去掉的属性 
        private static readonly string[] unbrowsableProperties = { "Dock" }; //

        /// <summary> 
        /// 重载PostFilterProperties方法隐藏属性 
        /// </summary> 
        protected override void PostFilterProperties(IDictionary properties)
        {
            foreach (string prop in unbrowsableProperties)
            {
                properties.Remove(prop);
            }
            base.PostFilterProperties(properties);
        }
    }
}
