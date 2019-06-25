using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.CoreInterface;

namespace Wlst.Ux.Wj1050Module
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
            get { return 1050; }
        }

        public string ModuleDescription
        {
            get { return "抄表设备"; }
        }

        public EquipmentModelArgs Args
        {
            get
            {
                return new EquipmentModelArgs()
                {
                    ImageSourcePath = "/Image/EquipmentImage/1050.png",
                    ModelInfoSetViewAttachRegion = Services.ViewIdAssign.Wj1050ManageViewAttachRegion,
                    ModelInfoSetViewId = Services.ViewIdAssign.Wj1050InfoSetViewId,
                    Name = "电表抄表设备"
                    
                };
            }
        }

        public string ModelName
        {
            get { return "抄表设备"; }
        }
    }
}
