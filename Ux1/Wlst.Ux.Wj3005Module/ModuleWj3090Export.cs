using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.CoreInterface;

namespace Wlst.Ux.WJ3005Module
{
 
    [Export(typeof(IIEquipmentModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ModuleWj3090Export : IIEquipmentModel
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
            get { return 3090; }
        }



        public string ModuleDescription
        {
            get
            {
                return "    该设备最多支持4路开关量输出,6路开关量输入,16路模拟量采样." + Environment.NewLine +
                       "    该设备自带6路防盗线路检测，具有一个485接口,可外接电表设备、节电设备、分布式光控设备等附属设备。";
            }
        }



 

        public EquipmentModelArgs Args
        {
            get
            {
                return new EquipmentModelArgs()
                {
                    ImageSourcePath = "/Image/EquipmentImage/3090.png",
                    ModelInfoSetViewAttachRegion = Cr.Core.CoreServices.DocumentRegionName.DocumentRegion,
                    ModelInfoSetViewId = Services.ViewIdAssign.Wj3005TmlInfoSetViewId,
                    Name = "3090型线路检测控制终端"

                };
            }
        }

        public string ModelName
        {
            get { return "3090型线路检测终端设备"; }
        }
    }
}
