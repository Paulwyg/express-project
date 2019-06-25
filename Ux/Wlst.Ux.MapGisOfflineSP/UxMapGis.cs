using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

namespace Wlst.Ux.MapGisLocalSP
{
  

    [ModuleExport(typeof(UxMapGis))]
    public class UxMapGis : IModule
    {
        #region IModule 成员

        public void Initialize()
        {
            //new Setting.SettingExtend().InitLoad();
        }

        //View i36N id 11010000~11019999

        #endregion
    }
}
