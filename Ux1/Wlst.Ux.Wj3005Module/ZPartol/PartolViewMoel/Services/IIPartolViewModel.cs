using System.Collections.ObjectModel;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Ux.WJ3005Module.ZPartol.PartolViewMoel.ViewModels;

namespace Wlst.Ux.WJ3005Module.ZPartol.PartolViewMoel.Services
{
    public interface IIPartolViewModel : IITab, IINavOnLoad, IIOnHideOrClose
    {
       ObservableCollection<PartolItemViewModel> MeasurePatrolData { get; }
    }
}
