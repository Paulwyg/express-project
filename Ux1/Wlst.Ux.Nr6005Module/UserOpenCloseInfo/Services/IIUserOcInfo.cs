using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Wlst.Ux.Nr6005Module.UserOpenCloseInfo.ViewModel;


namespace Wlst.Ux.Nr6005Module.UserOpenCloseInfo.Services
{
    public interface IIUserOcInfo 
    {
        ObservableCollection<OcInfoItems> Records { get; }
        void CurrentSelectItemDoubleClicked();
    
    }
}
