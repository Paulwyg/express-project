using System;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Wlst.Cr.Setting.Interfaces;
using Wlst.Cr.Setting.SettingHolding;

namespace Wlst.Cr.Setting
{

    [ModuleExport(typeof (CrSetting), DependsOnModuleNames = new string[] {"CrCore"})]
    public class CrSetting : IModule, IPartImportsSatisfiedNotification
    {
        #region IModule 成员

        public void Initialize()
        {

        }

        #endregion



        #region IPartImportsSatisfiedNotification

        private SettingComponentHolding _settingComponentHolding = new SettingComponentHolding();

        private EventSchduleTaskComponentHolding _eventSchduleTaskComponentHolding =
            new EventSchduleTaskComponentHolding();

        public void OnImportsSatisfied()
        {
            var lst = SettingItems.Select(t => t.Value).ToList();
            _settingComponentHolding.UpdateMenuItem(lst);

            var estLst = EventSchduleTaskItems.Select(t => t.Value).ToList();
            _eventSchduleTaskComponentHolding.UpdateEventSchduleTaskItem(estLst);
        }

        [ImportMany(AllowRecomposition = true)]
        public Lazy<IISetting>[] SettingItems { get; set; }


        [ImportMany(AllowRecomposition = true)]
        public Lazy<IIEventSchduleTask>[] EventSchduleTaskItems { get; set; }

        #endregion
    }
}
