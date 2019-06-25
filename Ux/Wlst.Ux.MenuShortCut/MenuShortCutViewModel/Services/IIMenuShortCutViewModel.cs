using System.Collections.ObjectModel;
using System.Windows.Input;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Ux.MenuShortCut.MenuShortCutViewModel.ViewModel;

namespace Wlst.Ux.MenuShortCut.MenuShortCutViewModel.Services
{
    public interface IIMenuShortCutViewModel : IINavOnLoad, IITab
    {
        ObservableCollection<ShortCutTreeItemViewModel> ChildTreeItems { get; }

        int MenuId { get; set; }

        string Warning { get; set; }

        string Name { get; set; }

        string Tooltips { get; set; }

        string ShortCuts { get; set; }

        ICommand CmdOk { get; }

        ICommand CmdSaveAll { get; }
    }
}
