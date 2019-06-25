using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Cr.CoreOne.OtherHelper
{
    public class Delay
    {
        public static void DelayEvent()
        {
            System.Windows.Forms.Application.DoEvents();
        }
    }
}
