using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Cr.CoreMims.Services
{
    public class ShowNewDataServices
    {
        /// <summary>
        /// 不允许外部注册
        /// </summary>
        public static event EventHandler<ShowNewDataEventArgs> OnUserWantShowNewDataView;

        /// <summary>
        /// 
        /// </summary>
        public static event EventHandler<ShowNewDataEventArgs> OnUserWantCloseNewDataView;

        /// <summary>
        /// 界面最下方最新数据驱动显示
        /// </summary>
        /// <param name="viewId"></param>
        public static void ShowNewDataView(int viewId)
        {
            if (OnUserWantShowNewDataView != null)
            {
                OnUserWantShowNewDataView(null, new ShowNewDataEventArgs() {ViewId = viewId});
            }
        }

        /// <summary>
        /// 界面最下方最新数据驱动关闭
        /// </summary>
        /// <param name="viewId"></param>
        public static void CloseNewDataView(int viewId)
        {
            if (OnUserWantCloseNewDataView != null)
            {
                OnUserWantCloseNewDataView(null, new ShowNewDataEventArgs() { ViewId = viewId });
            }
        }

    }


    public class ShowNewDataEventArgs : EventArgs
    {
        public int ViewId;
    }
}
