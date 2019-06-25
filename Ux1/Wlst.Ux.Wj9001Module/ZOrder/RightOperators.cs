using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreMims.ShowMsgInfo;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Sr.EquipmentInfoHolding.Model;

using Wlst.client;

namespace Wlst.Ux.Wj9001Module.ZOrder
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToWj9001ParaInfoSet : MenuItemBase
    {
        public NavToWj9001ParaInfoSet()
        {
            Id = Wj9001Module.Services.MenuIdAssgin.NavToLeakInfoSetViewfor9001Id;
            // Infrastructure.IdAssign.MenuIdAssign.NavToWj3005TmlInfoSetId;
            Text = "手动分合闸";
            Tag = "线路防盗集中器参数设置";
            Classic = "右键菜单-漏电保护器-专有";
            Description = "线路防盗集中器右键操作，防盗通用，ID 为" + Wj9001Module.Services.MenuIdAssgin.RightOperatorsBase40;
            // Infrastructure.IdAssign.MenuIdAssign.NavToWj3005TmlInfoSetId;
            Tooltips = "漏电设备手动分合闸";
            IsCheckable = false;
            IsEnabled = true;
            base.Command = new RelayCommand(Ex, CanEx, true);
            //    IsPrivilegLeave = true;

        }
        public override bool IsCanBeShowRwx()
        {
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanW();
            var equipment = this.Argu as WjParaBase;//Wlst.Cr.WjEquipmentBaseModels.Interface.IIEquipmentInfo;
            if (equipment == null) return false;
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
            return Wlst.Cr.CoreMims.Services.UserInfo.CanW(areaId);
        }
        private static bool CanEx()
        {
            return true;
        }
        protected void Ex()
        {
            var equipment = Argu as Sr.EquipmentInfoHolding.Model.Wj9001Leak;// IIEquipmentInfo;
            if (equipment == null)
                return;
            int rtuId = equipment.RtuId;
            if (rtuId < 1)
                return;

            var nt = Wlst.Sr.ProtocolPhone.LxLeak.wls_leak_order_zcOrSet;
            var order = new LeakOrders.LeakOrderItem();
            order.Op = 3;
            order.RtuId = rtuId ;   // todo  应该少一个参数
            nt.WstLeakOrderZcOrSet.Item.Add(order);   
            SndOrderServer.OrderSnd(nt);

            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                rtuId, equipment.RtuName, OperatrType.UserOperator, "选测终端");

        }

    }
   

}
