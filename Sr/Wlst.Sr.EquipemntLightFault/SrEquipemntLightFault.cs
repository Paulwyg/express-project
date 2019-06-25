using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Wlst.Sr.EquipemntLightFault.Services;


namespace Wlst.Sr.EquipemntLightFault
{
    [ModuleExport(typeof (SrEquipemntLightFault))]
    public class SrEquipemntLightFault : IModule
    {
        #region IModule 成员

        public void Initialize()
        {
            //  new PPProtocol.PPProtocol().RegistProtocol();
            new Services.TmlExistFaultsInfoServices();
            new Services.FaultBandtoTmlInfoSerices();
            new Services.TmlFaultTypeInfoServices();
            Services.UserDisplayErrorSelfSetInfoHold.MySelf.InitStartService();

            new Services.FaultDataShow();
            //注册故障解析函数
            // RegisterTmlFaultAnaFun.RegisterFun();

            Wlst.Sr.EquipmentInfoHolding.Ioc.GetSluLampError =
                Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetSluLampErrs;
            Wlst.Sr.SlusglInfoHold.Ioc.GetSluLampError =
                Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetSluLampErrs;
        }

        #endregion
    }
}
