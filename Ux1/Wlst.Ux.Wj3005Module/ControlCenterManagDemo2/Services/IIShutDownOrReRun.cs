using Wlst.Cr.Core.CoreInterface;

namespace Wlst.Ux.WJ3005Module.ControlCenterManagDemo2.Services
{
    public interface IIShutDownOrReRun : IINavOnLoad
    {
        bool IsShutDownOrReRun { get; set; }
    }
}
