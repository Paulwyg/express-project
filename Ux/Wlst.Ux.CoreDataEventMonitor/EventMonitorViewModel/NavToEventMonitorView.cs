using System.ComponentModel.Composition;
using Wlst.Cr.Core.Commands;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.Models;
using Wlst.Ux.CoreDataEventMonitor.Services;

namespace Wlst.Ux.CoreDataEventMonitor.EventMonitorViewModel
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToEventMonitorView : MenuItemBase
    {
        public NavToEventMonitorView()
        {
            Id =MenuIdAssgin .NavToEventMonitorViewId  ;
            Text = "底层事件";
            Tag = "底层事件监视";
            this.Classic = "主菜单";
            Description = "显示程序发布的所有事件，ID 为" + MenuIdAssgin.NavToEventMonitorViewId;
            Tooltips = "底层事件监视";
            Classic = "主菜单";
            base.IsEnabled = true;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex );
        }


        protected void Ex()
        {
            this.ExNavNoArgs(DocumentRegionName.DocumentRegion,
                             ViewIdAssign.EventMonitorViewId);
        }

    }
}
