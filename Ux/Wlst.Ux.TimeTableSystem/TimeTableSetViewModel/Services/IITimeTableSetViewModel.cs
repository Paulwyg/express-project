using System.Collections.ObjectModel;
using System.Windows.Input;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Ux.TimeTableSystem.TimeTableSetViewModel.ViewModel;

namespace Wlst.Ux.TimeTableSystem.TimeTableSetViewModel.Services
{
    public interface IITimeTableSetViewModel:IINavOnLoad ,IITab 
    {
        ObservableCollection<TimeTableViewModel> TimeTables { get; }

        TimeTableViewModel CurrentSelectItem { get; set; }

        ICommand AddNewCmd { get; }

        ICommand SaveTimeTable { get; }
    }
}
