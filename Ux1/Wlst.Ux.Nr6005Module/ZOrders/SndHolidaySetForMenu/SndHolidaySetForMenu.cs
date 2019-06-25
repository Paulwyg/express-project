using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreMims.ShowMsgInfo;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.client;

namespace Wlst.Ux.Nr6005Module.ZOrders.SndHolidaySet
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    class SndHolidaySetForMenu : MenuItemBase
    {
        public SndHolidaySetForMenu()
        {
            Id = Services.MenuIdAssgin.SndHolidaySetForMenuId;// Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMenuId;
            Tag = "发送节假日设置";
            Description = "选测终端，ID为" + Services.MenuIdAssgin.SndHolidaySetForMenuId// Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMenuId 
            + ",类型为终端右键菜单。";
            Text = "发送节假日设置";
            this.Classic = "右键菜单-监控设备通用";
            Tooltips = "发送节假日设置";
            base.Command = new RelayCommand(Ex, CanEx,true );
           // IsPrivilegLeave = true;
        }
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            var equipment = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (equipment == null || equipment.RtuStateCode  == 0) return false;
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
            return Wlst.Cr.CoreMims.Services.UserInfo.CanX(areaId);
        }
        public override void InitDataWhenBeforeUse(object argu)
        {
            base.InitDataWhenBeforeUse(argu);
        }

        private void Ex()
        {
            var terminalInfo = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (terminalInfo == null) return;
            var rtuId = terminalInfo.RtuId;

            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_orders;//.wlst_cnt_request_wj3090_measure;
            info.Args.Addr.Add(rtuId);
            info.WstRtuOrders.RtuIds.Add(rtuId);
            info.WstRtuOrders.Op = 41;
            SndOrderServer.OrderSnd(info, 0, 0, true);


            //var args = new PublishEventArgs
            //{
            //    EventType = PublishEventType.Core,
            //    EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.UserOperateRtu,
            //};
            //args.AddParams(rtuId);
            //args.AddParams(OpType.SndHoliday);
            //args.AddParams(-1);
            //args.AddParams(terminalInfo.RtuModel);
            //args.AddParams(null);
            //EventPublish.PublishEvent(args);




            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
              rtuId, terminalInfo.RtuName, OperatrType.UserOperator, "发送节假日设置");

        }

        protected bool CanEx()
        {
            var terminalInfo = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (terminalInfo == null) return false;
            if ( terminalInfo .RtuStateCode  == 0) return false;
            //var rtuId = terminalInfo.RtuId;
            //if (EquipmentRunningInfoHolding.TmlRunningInfoDictionary.ContainsKey(rtuId))
            //{
            //    var t = EquipmentRunningInfoHolding.TmlRunningInfoDictionary[rtuId];
            return true ;

         
        }



    }
}
