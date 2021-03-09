using System;
using System.Collections.Generic;
using System.Text;

namespace ZQTMS.Common
{
    /// <summary>
    /// 点击主界面左侧菜单，调整3PL的实际使用界面
    /// </summary>
    public static class FormClassManage
    {
        /// <summary>
        /// 获取对应的3PL界面
        /// </summary>
        /// <param name="dllPath"></param>
        /// <param name="dllName"></param>
        /// <param name="nameSpace"></param>
        /// <param name="assName"></param>
        /// <param name="paras"></param>
        public static void GetNewFormClass(ref string dllPath, ref string dllName, ref string nameSpace, ref string assName, ref string paras)
        {
            if (CommonClass.UserInfo.UserDB == UserDB.ZQTMS3PL)
            {
                if (assName == "frmWayBillAdd")
                {
                    assName = "frmWayBillAdd_3PL";
                }
            }
        }
    }
}
