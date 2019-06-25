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
            get { return "Wj1090型防盗设备"; }
        }



        public EquipmentModelArgs Args
        {
            get
            {
                return new EquipmentModelArgs
                           {
                               ImageSourcePath = "/Image/EquipmentImage/1090.png",
                    ModelInfoSetViewAttachRegion = Services.ViewIdAssign.LduInfoSetViewAttachRegion,
                    ModelInfoSetViewId = Services.ViewIdAssign.LduInfoSetViewId,
                    Name = "1090型两路防盗设备"

                };
            }
        }

        public string ModelName
        {
            get { return "1090型防盗设备"; }
        }
    }
}
