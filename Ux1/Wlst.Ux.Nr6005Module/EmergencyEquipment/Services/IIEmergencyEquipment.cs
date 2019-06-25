using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.CoreInterface;

namespace Wlst.Ux.Nr6005Module.EmergencyEquipment.Services
{
    public interface IIEmergencyEquipment : IITab, IINavOnLoad, IIOnHideOrClose
    {
        void SelectAllSwitchOut(int kx);
    }
}
