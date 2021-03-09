using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraEditors.Repository;

namespace ZQTMS.Tool
{
    public static class RepItemComboBox
    {
        public static RepositoryItemComboBox CreateRepItemComboBox
        {
            get { return new RepositoryItemComboBox(); }
        }
    }
}