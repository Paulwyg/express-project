using System.Collections.ObjectModel;
using System.Windows.Controls;
using Wlst.Cr.Core.CoreInterface;

namespace Wlst.Ux.Menu.Services
{
    public interface IIMenuViewModule
    {
        ObservableCollection<MenuItem> Items { get; }
    }
}
