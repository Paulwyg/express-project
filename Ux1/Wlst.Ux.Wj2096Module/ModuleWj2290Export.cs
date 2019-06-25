using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.CoreInterface;

namespace Wlst.Ux.Wj2096Module
{
    //铭泰nb单灯
    [Export(typeof (IIEquipmentModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ModuleWj2290Export : IIEquipmentModel
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
            get { return 2290; }
        }

        public string ModuleDescription
        {
            get { return "铭泰独立通信单灯设备"; }
        }


        public EquipmentModelArgs Args
        {
            get
            {
                return new EquipmentModelArgs
                           {
                               ImageSourcePath = "/Image/EquipmentImage/2090.png",
                               ModelInfoSetViewAttachRegion =
                                   Wlst.Cr.Core.CoreServices.DocumentRegionName.DocumentRegion,
                               //ModelInfoSetViewId = Services.ViewIdAssign.Wj2090SluInfoSetViewId,
                               Name = "2290独立通信单灯设备"

                           };
            }
        }

        public string ModelName
        {
            get { return "2290型独立通信单灯设备"; }
        }
    }
}
