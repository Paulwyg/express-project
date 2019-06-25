using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.CoreMims.CoreInterface;

namespace Wlst.Ux.WJ4005Module
{
    [Export(typeof(IIEquipmentModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
   public  class ModuleWj4005Export:IIEquipmentModel 
    {
        public bool CanBeAttachEquipmnet
        {
            get { return false ; }
        }

        public bool CanBeMainEquipment
        {
            get { return true ; }
        }

        public int ModelKey
        {
            get { return 4005; }
        }

        public string ModuleDescription
        {
            get { return "    该设备最多支持8路开关量输出,16路开关量输入,56路模拟量采样." +Environment.NewLine+
                         "    "; }
        }

        

 

        public EquipmentModelArgs Args
        {
            get { return new EquipmentModelArgs()
                             {
                                 ImageSourcePath = "/Image/EquipmentImage/3005.png",
                                 ModelInfoSetViewAttachRegion = Cr .Core .CoreServices .DocumentRegionName .DocumentRegion ,
                                 ModelInfoSetViewId = Services.ViewIdAssign.Wj4005TmlInfoSetViewId,
                                 Name = "4005型路灯控制设备"

                             }; }
        }

        public string ModelName
        {
            get { return "4005型终端设备"; }
        }
    }
}
