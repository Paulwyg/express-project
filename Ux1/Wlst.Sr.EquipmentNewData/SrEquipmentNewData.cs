using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Wlst.Sr.EquipmentInfoHolding.Services;


namespace Wlst.Sr.EquipmentNewData
{
    [ModuleExport(typeof(SrEquipmentNewData))]
    public class SrEquipmentNewData : IModule
    {
        #region IModule 成员

        public void Initialize()
        {
            //new PPProtocol.PPProtocol().RegistProtocol();
            //new DataHoldingExtend.NewDataHoldingExtend().InitLoad();
            new RtuNewDataService();
        }

        #endregion

        ////View i36N id 11050000~11059999
    }
}
