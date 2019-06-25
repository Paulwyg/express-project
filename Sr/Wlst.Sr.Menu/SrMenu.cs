using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Wlst.Cr.Core.ComponentHold;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.CoreOne.ComponentHold;
using Wlst.Cr.CoreOne.CoreInterface;

namespace Wlst.Sr.Menu
{

    [ModuleExport(typeof(SrMenu), DependsOnModuleNames = new string[] { "CrCore" })]
    public class SrMenu : IModule, IPartImportsSatisfiedNotification
    {
        #region IModule 成员

        public void Initialize()
        {
           
        }

        #endregion


        public void OnImportsSatisfied()
        {
            this.OnImportsSatisfiedMenuItem();
        }


        #region IPartImportsSatisfiedNotification MenuItems

        private readonly MenuComponentHolding _menuComponentHolding = new MenuComponentHolding();

        private void OnImportsSatisfiedMenuItem()
        {
            CheckSameMenuId();
            var lst = MenuItems.Select(t => t.Value).ToList();
            _menuComponentHolding.UpdateMenuItem(lst);
        }

        private void CheckSameMenuId()
        {
            var lst = new List<int>();
            foreach (var t in MenuItems)
            {
                if (lst.Contains(t.Value.Id))
                {
                    Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Supper Design Error: Two Menu Has the Same Value:" +
                                                            t.Value.Id);
                }
                else
                {
                    lst.Add(t.Value.Id);
                }
            }
        }

        [ImportMany(AllowRecomposition = true)]
        public Lazy<IIMenuItem>[] MenuItems { get; set; }

        #endregion
    }
}
