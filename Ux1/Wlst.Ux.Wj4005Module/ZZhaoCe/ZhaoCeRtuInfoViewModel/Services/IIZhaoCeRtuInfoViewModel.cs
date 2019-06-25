using System.Collections.ObjectModel;
using System.Windows.Input;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Ux.WJ4005Module.ZZhaoCe.ZhaoCeRtuInfoViewModel.ViewModel;

namespace Wlst.Ux.WJ4005Module.ZZhaoCe.ZhaoCeRtuInfoViewModel.Services
{
    public interface IIZhaoCeRtuInfoViewModel : IITab,IIOnHideOrClose
    {
        ObservableCollection<RtuZhaoCeParsViewModel> RtusZhaoCeInfo { get; }

        RtuZhaoCeParsViewModel CurrentSelectedItem { get; }

        ICommand DeleteCurrentCommand { get; }
    }
}
