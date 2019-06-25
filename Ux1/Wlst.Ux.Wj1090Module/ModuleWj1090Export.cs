using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.CoreInterface;

namespace Wlst.Ux.Wj1090Module
{
    [Export(typeof (IIEquipmentModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ModuleWj1090Export : IIEquipmentModel
    {
        public bool CanBeAttachEquipmnet
        {
            get { return true; }
        }

        public bool CanBeMainEquipment
        {
            get { return false; }
        }

        public int ModelKey
        {
            get { return 1090; }
        }

        public string ModuleDescription
        {
            get { return "10XX型线路检测设备"; }
        }



        public EquipmentModelArgs Args
        {
            get
            {
                return new EquipmentModelArgs
                           {
                               ImageSourcePath = "/Image/EquipmentImage/1090.png",
                               ModelInfoSetViewAttachRegion = Cr.Core.CoreServices.DocumentRegionName.DocumentRegion,
                    ModelInfoSetViewId = Services.ViewIdAssign.LduInfoSetViewId,
                               Name = "10XX型两路线路检测设备"

                };
            }
        }

        public string ModelName
        {
            get { return "10XX型线路检测设备"; }
        }
    }
}
