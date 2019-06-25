using System.Collections.ObjectModel;
using System.Windows.Input;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Ux.CoreDataEventMonitor.EventMonitorViewModel.ViewModels;

namespace Wlst.Ux.CoreDataEventMonitor.EventMonitorViewModel.Services
{
    public interface IIEventMonitorViewModel : IITab
    {
       ICommand CmdMonitor { get; }
       ObservableCollection<EventItemInfo> Items { get; }
       string CmdName { get; }
    }
}
