using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.CoreInterface;

namespace Wlst.Ux.Wj6005Module
{
    [Export(typeof(IIEquipmentModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
   public  class ModuleExport:IIEquipmentModel 
    {
        public bool CanBeAttachEquipmnet
        {
            get { return true  ; }
        }

        public bool CanBeMainEquipment
        {
            get { return false  ; }
        }


        public int ModelKey
        {
            get { return 601; }
        }

        public string ModuleDescription
        {
            get { return "节能设备"; }
        }


        public EquipmentModelArgs Args
        {
            get
            {
                return new EquipmentModelArgs()
                {
                    ImageSourcePath = "/Image/EquipmentImage/601.png",
                    ModelInfoSetViewAttachRegion = Cr.Core.CoreServices.DocumentRegionName.DocumentRegion,
                    ModelInfoSetViewId = Services.ViewIdAssign.Jd601TmlInfoSetViewIdViewId,
                    Name = "节能设备"

                };
            }
        }

        public string ModelName
        {
            get { return "节能设备"; }
        }
    }
}
