using System.Collections.ObjectModel;
using Wlst.Cr.Core.CoreInterface;

namespace Wlst.Ux.TimeTableSystem.TimeTableBandingViewModel.Services
{
    public interface IITimeTableBandingViewModel:IINavOnLoad ,IITab ,IIOnHideOrClose 
    {
        ObservableCollection<ListTreeNodeBase> ChildTreeItems { get; }

        void UpdatRtuTimeTable(bool isGroup, int rtuIdOrGroupId, int newTimeTabelId, int kLoops,
                               int applyRtuCls);
    }
}
