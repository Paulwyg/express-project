//using System.ComponentModel.Composition;
//using Infrastructure.IdAssign;
//using Wlst.Cr.Core.CoreInterface;
//using Wlst.Cr.CoreOne.Commands;
//using Wlst.Cr.CoreOne.CoreInterface;
//using Wlst.Cr.CoreOne.Models;

//namespace Wlst.Ux.Wj1080Module.Wj1080ManageViewModel
//{

//    [Export(typeof(IIMenuItem))]
//    [PartCreationPolicy(CreationPolicy.Shared)]
//    public class NavToWj1080ManageView : MenuItemBase
//    {
//        public NavToWj1080ManageView()
//        {
//            Id = Wj1080Module.Services.MenuIdAssgin.NavToWj1080ManageViewId;
//            //Infrastructure.IdAssign.MenuIdAssign.NavToWj1080ManageViewId;
//            Text = "光控管理";
//            Tag = "WJ1080设备[光控]管理";
//            this.Classic = "主菜单";
//            Description = "WJ1080设备[光控]管理，ID 为" + Wj1080Module.Services.MenuIdAssgin.NavToWj1080ManageViewId;
//            //Infrastructure.IdAssign.MenuIdAssign.NavToWj1080ManageViewId;
//            Tooltips = "WJ1080设备[光控]管理";
//            base.IsEnabled = true;
//            base.IsCheckable = false;
//            base.Command = new RelayCommand(Ex );
//        }

//        protected void Ex()
//        {
//            this.ExNavWithArgs(
//                //ViewIdNameAssign.Wj1080ModuleWj1080ManageViewAttachRegion ,
//                //               ViewIdNameAssign.Wj1080ModuleWj1080ManageViewId ,
//                Wj1080Module .Services .ViewIdAssign .Wj1080ManageViewAttachRegion ,
//                Wj1080Module .Services .ViewIdAssign .Wj1080ManageViewId ,
//                               1);
//        }
//    }
//}
