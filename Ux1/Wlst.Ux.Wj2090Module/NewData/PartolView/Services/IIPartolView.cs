using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.Wj2090Module.NewData.PartolView.Services
{
    public interface IIPartolView : Wlst.Cr.Core.CoreInterface.IITab, Wlst.Cr.Core.CoreInterface.IIOnHideOrClose,
                                    Wlst.Cr.Core.CoreInterface.IINavOnLoad
    {
        int IndexView { get; set; }
    }
}
