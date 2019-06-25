using System;
using System.ComponentModel.Composition;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreMims.ShowMsgInfo;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
 

namespace Wlst.Ux.WJ3005Module.ZZhaoCe.ZhaoCeRtuHolidaySetViewModel
{

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToZhaoCeRtuHolidaySetViewModelId : MenuItemBase
    {
        public NavToZhaoCeRtuHolidaySetViewModelId()
        {
            Id = WJ3005Module.Services.MenuIdAssgin.NavToZhaoCeRtuHolidaySetViewModelId;
            // Infrastructure.IdAssign.MenuIdAssign.NavToTmlInfoSetforWj3090Id;
            Text = "召测节假日设置";
            Tag = "召测节假日设置";
            Description = "召测节假日设置，ID 为" + WJ3005Module.Services.MenuIdAssgin.NavToZhaoCeRtuHolidaySetViewModelId;
            // Infrastructure.IdAssign.MenuIdAssign.NavToTmlInfoSetforWj3090Id;
            Tooltips = "召测节假日设置";
            this.Classic = "右键菜单-监控设备通用";

            this.IsCheckable = false;
            this.IsEnabled = true;
            this.Command = new RelayCommand(Ex, CanEx, true);
           // this.IsPrivilegLeave = true;
        }
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            var equipment = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (equipment == null || equipment.RtuStateCode  == 0) return false;
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
            return Wlst.Cr.CoreMims.Services.UserInfo.CanR(areaId);
        }

        private bool CanEx()
        {
            //if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            var equipment = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (equipment == null || equipment.RtuStateCode  == 0) return false;
            return true;
        }

        protected void Ex()
        {
            var equipment = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (equipment == null) return;
            int rtuId = equipment.RtuId;
            if (rtuId < 1) return;



            var nt = Wlst.Sr.ProtocolPhone.LxRtu.wst_zc_rtu_info;
            nt.Args.Addr.Add(rtuId);
            nt.WstRtuZcInfo.Op = 11;
            nt.WstRtuZcInfo.RtuId = rtuId;
            SndOrderServer.OrderSnd(nt);

            //lvf 记录 召测终端
            if (Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus.ContainsKey(rtuId) == false)
            {
                Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus.Add(rtuId, DateTime.Now);
            }
            else
            {
                Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus[rtuId] = DateTime.Now;
            }
            //var args = new PublishEventArgs
            //{
            //    EventType = PublishEventType.Core,
            //    EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.UserOperateRtu,
            //};
            //args.AddParams(rtuId);
            //args.AddParams(OpType.ZcHoliday);
            //EventPublish.PublishEvent(args);

            //LogInfo.Log(equipment.RtuName + "召测终端节假日设置命令已经发送");

            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                rtuId, equipment.RtuName, Wlst.Cr.CoreMims.ShowMsgInfo.OperatrType.UserOperator, "召测节假日设置");

        }

    }
}
