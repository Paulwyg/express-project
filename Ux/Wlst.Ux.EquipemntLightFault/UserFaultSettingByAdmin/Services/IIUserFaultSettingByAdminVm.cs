using System;
using Wlst.Cr.Core.CoreInterface;

namespace Wlst.Ux.EquipemntLightFault.UserFaultSettingByAdmin.Services
{
    public interface IIUserFaultSettingByAdminVm : IINavOnLoad, IITab, IIOnHideOrClose
    {
        event EventHandler OnChanged;
    }
}
