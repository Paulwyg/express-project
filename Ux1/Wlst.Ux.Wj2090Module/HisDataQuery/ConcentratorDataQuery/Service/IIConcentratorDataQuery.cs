using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.CoreInterface;

namespace Wlst.Ux.Wj2090Module.HisDataQuery.ConcentratorDataQuery.Service
{
    public interface IIConcentratorDataQuery : IITab, IINavOnLoad, IIOnHideOrClose
    {
        bool IsShowAllLampData { get; set; }
    }
}
