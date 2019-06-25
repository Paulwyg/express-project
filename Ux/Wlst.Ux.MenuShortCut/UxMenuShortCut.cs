using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

namespace Wlst.Ux.MenuShortCut
{

    [ModuleExport(typeof(UxMenuShortCut), DependsOnModuleNames = new string[] { "CrCore", "SrMenu" })]
    public class UxMenuShortCut : IModule
    {
        #region IModule 成员

        public void Initialize()
        {

        }



        #endregion
    }
}
