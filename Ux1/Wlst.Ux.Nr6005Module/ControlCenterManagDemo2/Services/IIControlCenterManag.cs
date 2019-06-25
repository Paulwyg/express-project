using System;
using Wlst.Cr.Core.CoreInterface;

namespace Wlst.Ux.Nr6005Module.ControlCenterManagDemo2.Services
{
    interface IIControlCenterManagDemo2 : IITab, IINavOnLoad,IIOnHideOrClose, IINavInitBeforShow
    {
        void SndDeleteAlarmTime();
        void SelectAllSwitchOut(int kx);
        event EventHandler OnNavOnLoadSelectdRtus;
    }
}
