using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.ViewModel;
using Wlst.Ux.TimeTableSystem.TimeInfoNew.ViewModel;

namespace Wlst.Ux.TimeTableSystem.TimeInfoNew.Services
{
    public interface IITimeInfoNew : IITab, IINavOnLoad, IIOnHideOrClose
    {
        ObservableCollection<TimeTableInfomationItem> Items { get; set; }

        void UpdateNodeTimeTable(int rtuOrGrpId, int loop, int newTimeTableId, string newTimeTableName,
                                 string newTimeTimeDesc);

        event EventHandler<EventArgsEx> OnUserWantSetGroupWeekSet;

        event EventHandler OnNavOnLoadSelectdRtus;

        void SetTimeInfoMnVm(TimeInfoMnVm data);

         ObservableCollection<TreeGrpNodes> TreeItems { get; set; }
    }
    public class EventArgsEx : EventArgs
    {
        public TreeGrpNodes Info;

    }
}
