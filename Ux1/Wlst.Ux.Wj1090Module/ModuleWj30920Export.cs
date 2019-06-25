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
            get { return false ; }
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
            get { return "30XX型线路检测设备"; }
        }


        public EquipmentModelArgs Args
        {
            get
            {
                return new EquipmentModelArgs
                           {
                               ImageSourcePath = "/Image/EquipmentImage/30920.png",
                               ModelInfoSetViewAttachRegion = Cr.Core.CoreServices.DocumentRegionName.DocumentRegion,
                    ModelInfoSetViewId = Services.ViewIdAssign.LduInfoSetViewId,
                               Name = "30XX型六路线路检测设备"

                };
            }
        }

        public string ModelName
        {
            get { return "30XX型线路检测设备"; }
        }
    }
}
