using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Wlst.Cr.CoreMims.ComponentHold;
using Wlst.Cr.CoreMims.CoreInterface;
using Wlst.Cr.Coreb.Servers;

namespace Wlst.Cr.CoreMims
{
    [ModuleExport(typeof (CrCoreMims), DependsOnModuleNames = new string[] {"CrCore"})]
    public class CrCoreMims : IModule, IPartImportsSatisfiedNotification
    {

        #region IModule 成员

        public void Initialize()
        {

        }

        #endregion

        public void OnImportsSatisfied()
        {
            this.OnImportsSatisfiedEquipmentModelItems();
        }





        #region IPartImportsSatisfiedNotification EquipmentModelComponent

        //private readonly MenuComponentHolding _menuComponentHolding = new MenuComponentHolding();

        private void OnImportsSatisfiedEquipmentModelItems()
        {
            //CheckSameMenuId();
            //var lst = MenuItems.Select(t => t.Value).ToList();
            //_menuComponentHolding.UpdateMenuItem(lst);
            EquipmentModelComponentHolding.DicEquipmentModels.Clear();

            foreach (var t in EquipmentModelItems)
            {
                if (!EquipmentModelComponentHolding.DicEquipmentModels.ContainsKey(t.Value.ModelKey))
                    EquipmentModelComponentHolding.DicEquipmentModels.Add(t.Value.ModelKey, t.Value);
            }

            TreeNodeExportHolding.DicEquipmentModels.Clear();

            foreach (var t in TreeNodeLoadItems)
            {
                if (t.Value.RtuModes != null)
                {
                    foreach (var f in t.Value.RtuModes)
                    {
                        if (!TreeNodeExportHolding.DicEquipmentModels.ContainsKey(f))
                            TreeNodeExportHolding.DicEquipmentModels.Add(f, t.Value);
                    }
                }
            }
            Wlst.Cr.Coreb.AsyncTask.Qtz.AddQtz("nu", 8888, DateTime.Now.AddHours(1).Ticks, 60 * 60, EmptyApp);

        }

        void EmptyApp(object obj)
        {
            try
            {
                EmptyInapp.StartEmptyInBinpath();
            }
            catch (Exception ex)
            {

            }

        }


        [ImportMany(AllowRecomposition = true)]
        public Lazy<IITreeNodeLoadExport>[] TreeNodeLoadItems { get; set; }


        [ImportMany(AllowRecomposition = true)]
        public Lazy<IIEquipmentModel>[] EquipmentModelItems { get; set; }

        #endregion
    }
}
