using System.Collections.ObjectModel;
using System.Windows.Input;
using Wlst.Ux.WJ3005Module.ZZhaoCe.ZhaoCeRtuWeekSetViewModel.ViewModel;

namespace Wlst.Ux.WJ3005Module.ZZhaoCe.ZhaoCeRtuWeekSetViewModel.Services
{
   public  interface IIZhaoCeRtuWeekSetViewModel:Wlst .Cr .Core .CoreInterface .IITab ,Wlst .Cr .Core .CoreInterface .IIOnHideOrClose 
    {

        ObservableCollection<OneRtuZhaoCeTime> RtusWeekSet { get; }

        OneRtuZhaoCeTime CurrentSelectedItem { get; }

        ICommand DeleteCurrentCommand { get; }

    }
}
