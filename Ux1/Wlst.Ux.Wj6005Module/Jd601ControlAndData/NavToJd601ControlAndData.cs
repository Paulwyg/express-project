//using System.ComponentModel.Composition;
//using Wlst.Cr.Core.CoreInterface;
//using Wlst.Cr.CoreOne.Commands;
//using Wlst.Cr.CoreOne.CoreInterface;
//using Wlst.Cr.CoreOne.Models;
//using Wlst.Cr.WjEquipmentBaseModels.Interface;

//namespace Wlst.Ux.Wj6005Module.Jd601ControlAndData
//{

//    [Export(typeof (IIMenuItem))]
//    [PartCreationPolicy(CreationPolicy.Shared)]
//    public class NavToJd601ControlAndData : MenuItemBase
//    {
//        public NavToJd601ControlAndData()
//        {
//            Id = Wj6005Module.Services.MenuIdAssgin.NavToJd601ControlAndDataId;
//            // Infrastructure.IdAssign.MenuIdAssign.NavToTmlInfoSetforWj3090Id;
//            Text = "数据与控制";
//            Tag = "Jd601附属设备数据查询与与控制";
//            Description = "Jd601附属设备数据查询与与控制，ID 为" + Wj6005Module.Services.MenuIdAssgin.NavToJd601ControlAndDataId;
//            // Infrastructure.IdAssign.MenuIdAssign.NavToTmlInfoSetforWj3090Id;
//            Tooltips = "Jd601附属设备数据查询与与控制";

//            this.IsCheckable = false;
//            this.IsEnabled = true;
//            this.Command = new RelayCommand(Ex);
//            this.IsPrivilegLeave = true;
//        }



//        protected void Ex()
//        {
//            var equipment = this.Argu as IIEquipmentInfo;
//            if (equipment == null) return;
//            int rtuId = equipment.RtuId;
//            if (rtuId < 1) return;
//            this.ExNavWithArgs(
//                Wj6005Module.Services.ViewIdAssign.Jd601ControlAndDataViewAttachRegion,
//                Wj6005Module.Services.ViewIdAssign.Jd601ControlAndDataViewId,
//                rtuId);
//        }

//    }
//}
