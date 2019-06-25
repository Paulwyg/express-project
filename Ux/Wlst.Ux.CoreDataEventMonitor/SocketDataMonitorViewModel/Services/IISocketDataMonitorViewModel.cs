using System.Collections.ObjectModel;
using System.Windows.Input;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Ux.CoreDataEventMonitor.SocketDataMonitorViewModel.ViewModes;

namespace Wlst.Ux.CoreDataEventMonitor.SocketDataMonitorViewModel.Services
{
   public  interface IISocketDataMonitorViewModel:IITab 
    {
        ObservableCollection<ItemInfo> Items { get; }
       ICommand CmdMonitor { get; }
       string CmdName { get; }
    }
}
