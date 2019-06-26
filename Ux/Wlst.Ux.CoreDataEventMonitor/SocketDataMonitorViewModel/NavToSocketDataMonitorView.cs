using System.ComponentModel.Composition;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.CoreDataEventMonitor.Services;

namespace Wlst.Ux.CoreDataEventMonitor.SocketDataMonitorViewModel
{

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToSocketDataMonitorView : MenuItemBase
    {
        public NavToSocketDataMonitorView()
        {
            Id = MenuIdAssgin.NavToSocketDataMonitorViewId;
            Text = "底层数据";
            Tag = "底层Socket接收与发送的所有数据监视";
            this.Classic = "主菜单";
            Description = "显示发送接收底层原始数据，ID 为" + MenuIdAssgin.NavToSocketDataMonitorViewId;
            Tooltips = "接收与发送的所有数据监视";
            Classic = "主菜单";
            base.IsEnabled = true;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex );
        }


        protected void Ex()
        {
            this.ExNavNoArgs(
                             ViewIdAssign.SocketDataMonitorId);
        }

    }
}
