using System;
using System.Collections.ObjectModel;
using Telerik.Windows.Controls.Map;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Ux.RadMapJpeg.MapJepg.ViewModels;

namespace Wlst.Ux.RadMapJpeg.MapJepg.Services
{
    public interface IIRadMapJpeg : IINavOnLoad ,IITab 
    {
        ObservableCollection<MapNodeViewModel> MainEquipmentList { get; }
        ObservableCollection<MapNodeViewModel> AttachEquipmentList { get; }
        Action<Location> OnSelectEquipmentChange { get; set; }
        MapNodeViewModel CurrentSelectNode { get; set; }
    }
}