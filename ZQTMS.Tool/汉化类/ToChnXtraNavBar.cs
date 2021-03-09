using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraNavBar;

namespace ZQTMS.Tool
{
    public class ToChnXtraNavBar:NavBarLocalizer
    {
        public override string GetLocalizedString(NavBarStringId id)
        {
            switch (id)
            {
                case NavBarStringId.NavPaneChevronHint: return "配置按钮";
                case NavBarStringId.NavPaneMenuAddRemoveButtons: return "添加或删除按钮(&A)";
                case NavBarStringId.NavPaneMenuShowFewerButtons: return "显示较少的按钮(&F)";
                case NavBarStringId.NavPaneMenuShowMoreButtons: return "显示更多的按钮(&M)";
            }
            return base.GetLocalizedString(id);
        }

    }
}
