using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.CoreInterface;

namespace Wlst.Ux.Wj1080Module
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
            get { return true ; }
        }

        public int ModelKey
        {
            get { return 1080; }
        }

        public string ModuleDescription
        {
            get { return "光控设备"; }
        }

        public EquipmentModelArgs Args
        {
            get
            {
                return new EquipmentModelArgs()
                {
                    ImageSourcePath = "/Image/EquipmentImage/1080.png",
                    ModelInfoSetViewAttachRegion = Wlst .Cr .Core .CoreServices .DocumentRegionName .DocumentRegion ,
                    ModelInfoSetViewId = Services.ViewIdAssign.Wj1080TmlInfoSetViewId,
                    Name = "光控设备"

                };
            }
        }

        public string ModelName
        {
            get { return "型光控设备"; }
        }
    }
}
