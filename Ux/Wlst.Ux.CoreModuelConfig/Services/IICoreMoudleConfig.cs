using System.Collections.ObjectModel;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Ux.CoreModuelConfig.ViewModel;

namespace Wlst.Ux.CoreModuelConfig.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IICoreMoudleConfig : IITab,IINavOnLoad 
    {
        ObservableCollection<ModuleItemInfoModel> ItemsModules { get; }
    }
}
