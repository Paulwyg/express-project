using System;
using System.Collections.ObjectModel;
using System.Windows;
using GMap.NET;
using GMap.NET.WindowsPresentation;
using Telerik.Windows.Controls.Map;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Ux.RadMapJpeg.MapJepg.ViewModels;

namespace Wlst.Ux.RadMapJpeg.MapJepg.Services
{
    public interface IIRadMapJpeg : IINavOnLoad, IITab
    {
        //ObservableCollection<MapNodeViewModel> MainEquipmentList { get; }
        //ObservableCollection<MapNodeViewModel> AttachEquipmentList { get; }
        //Action<PointLatLng> OnSelectEquipmentChange { get; set; }
        //MapNodeViewModel CurrentSelectNode { get; set; }

        ////Visibility VisiTooltips { get; set; }

        void SetGmap(GMapControl map);

    }
}