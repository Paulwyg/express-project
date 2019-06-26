using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Cr.Core.CoreInterface
{
    /// <summary>
    /// 当用户关或隐藏页面的时候
    /// </summary>
    public interface IIOnHideOrClose
    {
        /// <summary>
        /// 当用户关或隐藏页面的时候
        /// </summary>
        void OnUserHideOrClosing();
    }
}
