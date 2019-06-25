using System.ComponentModel.Composition;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.WjEquipmentBaseModels.Interface;

namespace Wlst.Ux.Wj6005Module.TmlInfoSetforJd601
{

    //[Export(typeof (IIMenuItem))]
    //[PartCreationPolicy(CreationPolicy.Shared)]
    //public class NavToJd601TmlInfoSet : MenuItemBase
    //{
    //    public NavToJd601TmlInfoSet()
    //    {
    //        Id = Wj6005Module.Services.MenuIdAssgin.NavToJd601TmlInfoSetId;
    //            // Infrastructure.IdAssign.MenuIdAssign.NavToTmlInfoSetforWj3090Id;
    //        Text = "参数设置";
    //        Tag = "Jd601附属设备参数设置";
    //        Description = "Jd601附属设备参数设置，ID 为" + Wj6005Module.Services.MenuIdAssgin.NavToJd601TmlInfoSetId;
    //            // Infrastructure.IdAssign.MenuIdAssign.NavToTmlInfoSetforWj3090Id;
    //        Tooltips = "设置当前Jd601附属设备参数";

    //        this.IsCheckable = false;
    //        this.IsEnabled = true;
    //        this.Command = new RelayCommand(Ex);
    //        this.IsPrivilegLeave = true;
    //    }



    //    protected void Ex()
    //    {
    //        var equipment = this.Argu as IIEquipmentInfo;
    //        if (equipment == null) return;
    //        int rtuId = equipment.RtuId;
    //        if (rtuId < 1) return;
    //        this.ExNavWithArgs(
    //            //ViewIdNameAssign.Wj3090ModuleTmlInfoSetforWj3090AttachRegion,
    //            //               ViewIdNameAssign.Wj3090ModuleTmlInfoSetforWj3090Id,
    //            Wj6005Module.Services.ViewIdAssign.Jd601TmlInfoSetViewIdAttachRegion,
    //            Wj6005Module.Services.ViewIdAssign.Jd601TmlInfoSetViewIdViewId,
    //            rtuId);
    //    }

    //}
}
