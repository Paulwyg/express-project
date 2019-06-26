using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Cr.Core.CoreInterface
{

    /// <summary>
    /// 界面呈现前对界面进行处理，获取前初始化则必须实现的接口
    /// </summary>
    public interface IINavInitBeforShow
    {
        /// <summary>
        /// 界面呈现前对界面进行处理，耗时的UI显示请在 NavOnLoad中执行
        /// </summary>
        /// <param name="parsObjects"></param>
        void NavInitBeforShow(params object[] parsObjects);

    }
}
