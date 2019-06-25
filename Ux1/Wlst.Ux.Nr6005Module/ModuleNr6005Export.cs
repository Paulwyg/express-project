using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.CoreMims.CoreInterface;

namespace Wlst.Ux.Nr6005Module
{
    [Export(typeof(IIEquipmentModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
   public  class ModuleWj3005Export:IIEquipmentModel 
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
            get { return 6005; }
        }

        public string ModuleDescription
        {
            get { return "    该设备最多支持6路开关量输出,16路开关量输入,36路模拟量采样." +Environment.NewLine+
                         "    该设备具有一个485接口,可外接电表设备、节电设备、分布式光控设备等附属设备。"; }
        }

        

 

        public EquipmentModelArgs Args
        {
            get { return new EquipmentModelArgs()
                             {
                                 ImageSourcePath = "/Image/EquipmentImage/3005.png",
                                 ModelInfoSetViewAttachRegion = Cr .Core .CoreServices .DocumentRegionName .DocumentRegion ,
                                 ModelInfoSetViewId = Services.ViewIdAssign.Wj3005TmlInfoSetViewId,
                                 Name = "6005型路灯控制设备"

                             }; }
        }

        public string ModelName
        {
            get { return "6005型终端设备"; }
        }
    }
}
