using Wlst.Cr.Core.CoreInterface;

namespace Wlst.Ux.EmergencyDispatch.ControlCenterManagDemo2.Services
{
    public interface IIShutDownOrReRun : IINavOnLoad
    {
        bool IsShutDownOrReRun { get; set; }
    }
}
