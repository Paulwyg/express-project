using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.ViewModel;

namespace Wlst.Ux.TimeTableSystem.TimeInfoMn.Services
{
   

    public interface IITimeInfoMn : IINavOnLoad, IITab, IIOnHideOrClose
    {
        ObservableCollection<TimeTableInfomationItem> Items { get; set; }

        void UpdateNodeTimeTable(int rtuOrGrpId, int loop, int newTimeTableId, string newTimeTableName,
                                 string newTimeTimeDesc);

        event EventHandler<EventArgsEx > OnUserWantSetGroupWeekSet;
    }

    public class EventArgsEx:EventArgs
    {
        public OneGrpOrRtuLoopsSet Info;

    }
}
