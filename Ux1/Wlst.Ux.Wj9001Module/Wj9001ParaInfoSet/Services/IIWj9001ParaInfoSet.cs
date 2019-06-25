using System;
using Wlst.Cr.Core.CoreInterface;

namespace Wlst.Ux.Wj9001Module.Wj9001ParaInfoSet.Services
{
    public interface IIWj9001ParaInfoSet : IITab, IINavOnLoad, IIOnHideOrClose
    {
        event EventHandler OnNavOnLoadSelectdRtus;
    }
}
