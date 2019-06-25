//using System.ComponentModel.Composition;
//using Wlst.Cr.Core.CoreInterface;
//using Wlst.Cr.CoreOne.Commands;
//using Wlst.Cr.CoreOne.CoreInterface;
//using Wlst.Cr.CoreOne.Models;
//using Wlst.Cr.WjEquipmentBaseModels.Interface;

//namespace Wlst.Ux.Wj6005Module.Jd601ManageViewModel
//{

//    [Export(typeof (IIMenuItem))]
//    [PartCreationPolicy(CreationPolicy.Shared)]
//    public class NavToJd601ManageView : MenuItemBase
//    {
//        public NavToJd601ManageView()
//        {
//            Id = Wj6005Module.Services.MenuIdAssgin.NavToJd601ManageViewId;
//            // Infrastructure.IdAssign.MenuIdAssign.NavToTmlInfoSetforWj3090Id;
//            Text = "节电设备管理";
//            Tag = "Jd601节电设备管理";
//            Description = "Jd601节电设备管理，ID 为" + Wj6005Module.Services.MenuIdAssgin.NavToJd601ManageViewId;
//            // Infrastructure.IdAssign.MenuIdAssign.NavToTmlInfoSetforWj3090Id;
//            Tooltips = "Jd601节电设备管理";
//            this.Classic = "主菜单";
//            this.IsCheckable = false;
//            this.IsEnabled = true;
//            this.Command = new RelayCommand(Ex);
//           // this.IsPrivilegLeave = true;
//        }



//        protected void Ex()
//        {
//            //var equipment = this.Argu as IIEquipmentInfo;
//            //if (equipment == null) return;
//            //int rtuId = equipment.RtuId;
//            //if (rtuId < 1) return;
//            this.ExNavWithArgs(
//                //ViewIdNameAssign.Wj3090ModuleTmlInfoSetforWj3090AttachRegion,
//                //               ViewIdNameAssign.Wj3090ModuleTmlInfoSetforWj3090Id,
//                Wj6005Module.Services.ViewIdAssign.Jd601ManageViewAttachRegion,
//                Wj6005Module.Services.ViewIdAssign.Jd601ManageViewId,
//                0);
//        }

//    }
//}
