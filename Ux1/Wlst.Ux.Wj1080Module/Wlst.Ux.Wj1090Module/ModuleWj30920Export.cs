using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.CoreInterface;

namespace Wlst.Ux.Wj1090Module
{

    [Export(typeof(IIEquipmentModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ModuleWj30920Export : IIEquipmentModel
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
            get { return 30920; }
        }

        public string ModuleDescription
        {
            get { return "Wj3090型防盗设备"; }
        }


        public EquipmentModelArgs Args
        {
            get
            {
                return new EquipmentModelArgs
                           {
                               ImageSourcePath = "/Image/EquipmentImage/30920.png",
                    ModelInfoSetViewAttachRegion = Services.ViewIdAssign.LduInfoSetViewAttachRegion,
                    ModelInfoSetViewId = Services.ViewIdAssign.LduInfoSetViewId,
                    Name = "3090型六路防盗设备"

                };
            }
        }

        public string ModelName
        {
            get { return "3090型防盗设备"; }
        }
    }
}
