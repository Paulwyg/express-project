using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Sr.EquipmentInfoHolding.Model;

namespace Wlst.Ux.Wj6005Module.Jd601TmlInfo
{


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToJd601TmlInfoView : MenuItemBase
    {
        public NavToJd601TmlInfoView()
        {
            Id = Wj6005Module.Services.MenuIdAssgin.NavToJd601TmlInfoSetId;
            // Infrastructure.IdAssign.MenuIdAssign.NavToTmlInfoSetforWj3090Id;
            Text = "参数设置";
            Tag = "Jd601附属设备参数设置";
            Description = "Jd601附属设备参数设置，ID 为" + Wj6005Module.Services.MenuIdAssgin.NavToJd601TmlInfoSetId;
            // Infrastructure.IdAssign.MenuIdAssign.NavToTmlInfoSetforWj3090Id;
            Tooltips = "设置当前Jd601附属设备参数";
            Classic = "右键菜单-节电设备";
            this.IsCheckable = false;
            this.IsEnabled = true;
            this.Command = new RelayCommand(Ex);
           // this.IsPrivilegLeave = true;
        }

        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanW();
            var equipment = this.Argu as Sr.EquipmentInfoHolding.Model.WjParaBase;//Wlst.Cr.WjEquipmentBaseModels.Interface.IIEquipmentInfo;
            if (equipment == null || equipment.RtuStateCode == 0) return false;
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
            return Wlst.Cr.CoreMims.Services.UserInfo.CanW(areaId );
        }

        protected void Ex()
        {
            
            var equipment = this.Argu as WjParaBase;// IIEquipmentInfo;
            if (equipment == null) return;
            int rtuId = equipment.RtuId;
            if (rtuId < 1) return;
            this.ExNavWithArgs(
                //ViewIdNameAssign.Wj3090ModuleTmlInfoSetforWj3090AttachRegion,
                //               ViewIdNameAssign.Wj3090ModuleTmlInfoSetforWj3090Id,
                Wj6005Module.Services.ViewIdAssign.Jd601TmlInfoViewId,
                rtuId);
        }

    }
}
