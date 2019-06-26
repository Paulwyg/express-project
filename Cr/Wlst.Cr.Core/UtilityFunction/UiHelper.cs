using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Cr.Core.UtilityFunction
{
    public class UiHelper
    {
        /// <summary>
        /// 暂时 释放UI界面控制权
        /// </summary>
        public static void UiDoOtherUserEvent()
        {
            System.Windows.Forms.Application.DoEvents();
        }
    }
}
