using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ZQTMS.Tool
{
    public class MsgLabel : IMsg
    {
        private Label _lbl = null;
        public MsgLabel(Label lbl)
        {
            _lbl = lbl;
        }

        public void UpdateMessage(string msg)
        {
            _lbl.Text = msg;
            _lbl.Update();
        }
    }
}
