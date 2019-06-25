using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Ux.EquipmentInfo.EquipmentInfo.MainViewModel.Services;

namespace Wlst.Ux.EquipmentInfo.EquipmentInfo.MainViewModel.ViewModel
{
    [Export(typeof(IIMainViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MainViewModel : EventHandlerHelperExtendNotifyProperyChanged, IIMainViewModel
    {
         public void NavOnLoad(params object[] parsObjects)
         {}

        #region tab iinterface
        public int Index
        {
            get { return 1; }
        }
        public string Title
        {
            get
            {
                return "设备信息";
                //return "Setting";
            }
        }

        public bool CanClose
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this instance can pin; otherwise, <c>False</c>.
        /// 是否可锁定
        /// </summary>
        public bool CanUserPin
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this pane can float; otherwise, <c>false</c>.
        /// 是否可悬浮
        /// </summary>
        public bool CanFloat
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this instance can dock in the document host; otherwise, <c>false</c>.
        /// 是否可移动至document host
        /// </summary>
        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        #endregion

        public void OnUserHideOrClosing()
        { }
    }
}
