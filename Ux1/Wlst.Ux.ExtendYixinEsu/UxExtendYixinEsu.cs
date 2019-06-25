using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

namespace Wlst.Ux.ExtendYixinEsu
{

    /// <summary>
    /// 宜兴节能 扩展模块
    /// </summary>
    [ModuleExport(typeof (UxExtendYixinEsu))]
    public class UxExtendYixinEsu : IModule
    {
        #region IModule 成员

        public void Initialize()
        {

        }




        #endregion
    }
}
