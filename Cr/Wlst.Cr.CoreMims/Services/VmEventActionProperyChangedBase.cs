using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.mobile;

namespace Wlst.Cr.CoreMims.Services
{
    /// <summary>
    /// 页面显示继承的
    /// </summary>
    public class VmEventActionProperyChangedBase :
        Wlst.Cr.Core.EventHandlerHelper.EventHandlerHelperExtendNotifyProperyChanged,
        Wlst.Cr.Core.CoreInterface.IITab,
        Wlst.Cr.Core.CoreInterface.IINavOnLoad,
        Wlst.Cr.Core.CoreInterface.IIOnHideOrClose
    {

        public VmEventActionProperyChangedBase()
        {
            Title = "未设置";
            CanClose = true;
            CanDockInDocumentHost = true;
            CanFloat = true;
            CanUserPin = true;
            Guidr = this.GetType().GUID;
        }


        public void RegistProtocol(MsgWithMobile cmd, Action<string, MsgWithMobile> action,bool runInUiThread=false )
        {
            ProtocolServer.RegistProtocol(cmd, action, this.GetType(), this,runInUiThread );
        }

        public Guid Guidr { get; private set; }
        public int Index
        {
            get { return 1; }
        }
        public string Title { get; set; }

        public bool CanClose { get; set; }

        public bool CanDockInDocumentHost { get; set; }

        public bool CanFloat { get; set; }

        public bool CanUserPin { get; set; }



        //public event EventHandler OnExitEventHandler;

        //public void OnExit()
        //{
        //    UnsubscribeEvent();
        //    ProtocolServer.UnsubscribeProtocols(this);
        //    OnExitr();
        //    if (OnExitEventHandler != null) OnExitEventHandler(this, EventArgs.Empty);
        //    Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogDebug(this.GetType() + "   OnExit");

        //}

        //public virtual void OnExitr()
        //{

        //}

        protected bool IsViewShowd = false;

        /// <summary>
        /// 底层导航到本页面的时候除了函数  重定位到NavOnLoadr函数  
        /// </summary>
        /// <param name="parsObjects"></param>
        public void NavOnLoad(params object[] parsObjects)
        {
            this.NavOnLoadr(parsObjects);
            IsViewShowd = true;
        }

        /// <summary>
        /// 页面打开的时候需要处理的事
        /// </summary>
        /// <param name="parsObjects"></param>
        public virtual void NavOnLoadr(params object[] parsObjects)
        {

        }

        public void OnUserHideOrClosing()
        {
            IsViewShowd = false;
            OnUserHideOrClosingr();
        }

        /// <summary>
        /// 页面关闭 时候需要处理的事件
        /// </summary>
        public virtual void OnUserHideOrClosingr()
        {

        }
    }
}
