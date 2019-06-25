using Wlst.Cr.Core.CoreInterface;

namespace Wlst.Ux.Nr6005Module.ControlCenterManagDemo2.Services
{
    public interface IIShutDownOrReRun : IINavOnLoad
    {
        bool IsShutDownOrReRun { get; set; }
    }
}
