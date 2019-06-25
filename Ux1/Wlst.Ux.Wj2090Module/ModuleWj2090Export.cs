using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.CoreInterface;

namespace Wlst.Ux.Wj2090Module
{


    [Export(typeof(IIEquipmentModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ModuleWj2090Export : IIEquipmentModel
    {
        public bool CanBeAttachEquipmnet
        {
            get { return false; }
        }

        public bool CanBeMainEquipment
        {
            get { return true; }
        }

        public int ModelKey
        {
            get { return 2090; }
        }

        public string ModuleDescription
        {
            get { return "2090型单灯设备"; }
        }


        public EquipmentModelArgs Args
        {
            get
            {
                return new EquipmentModelArgs
                           {
                               ImageSourcePath = "/Image/EquipmentImage/2090.png",
                               ModelInfoSetViewAttachRegion = Wlst.Cr.Core.CoreServices.DocumentRegionName.DocumentRegion,
                               ModelInfoSetViewId = Services.ViewIdAssign.Wj2090SluInfoSetViewId,
                               Name = "2090型单灯设备"

                           };
            }
        }

        public string ModelName
        {
            get { return "2090型单灯设备"; }
        }
    }
}
