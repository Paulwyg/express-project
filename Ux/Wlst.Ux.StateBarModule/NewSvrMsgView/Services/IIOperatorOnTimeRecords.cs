using System.Collections.ObjectModel;
using Wlst.Ux.StateBarModule.NewSvrMsgView.ViewModel;

namespace Wlst.Ux.StateBarModule.NewSvrMsgView.Services
{
    public interface IIOperatorOnTimeRecords:Wlst .Cr .Core .CoreInterface .IITab 
    {
        ObservableCollection<OperatorRecordItem> Records { get; }
        void CurrentSelectItemDoubleClicked();

    }
}
