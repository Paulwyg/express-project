using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.TmlNewDataVmLeft.ViewModel;

namespace Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.TmlNewDataVmLeft.Services
{
    /// <summary>
    /// 终端最新窗口接口
    /// </summary>
    public interface IINewDataVmLeft:IITab ,IINavOnLoad
    {
        void MeasureRtu();
        event EventHandler VisiChanged;
        event EventHandler CompareVisiChanged;
        event EventHandler DetailVisiChanged;
        event EventHandler OnlineVisiChanged;
        event EventHandler<EventArsgLoopCount> LoopCountChanged;
        
        void RequestNearData();
        void ChangeVis();



        int RtuId { get; set; }
        ObservableCollection<LoopInfoLeft> LoopxInfo { get; }

    }

   public class EventArsgLoopCount:EventArgs
   {
       public int LoopCount;

       public List<bool > IsShowPro;

   }
}
