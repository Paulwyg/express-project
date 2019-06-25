using System.ComponentModel.Composition;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreMims.ShowMsgInfo;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
 

namespace Wlst.Ux.WJ3005Module.ZOrders.SndWeekSet
{

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class SndWeekSetForMenu : MenuItemBase
    {

        public SndWeekSetForMenu()
        {
            Id = Services.MenuIdAssgin.SndWeekSetForMenuId ;// Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMenuId;
            Tag = "发送周设置";
            Description = "选测终端，ID为" + Services.MenuIdAssgin.SndWeekSetForMenuId// Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMenuId 
            + ",类型为终端右键菜单。";
            Text = "发送周设置";
            this.Classic = "右键菜单-监控设备通用";
            Tooltips = "发送周设置";
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
            //var lst = new List<int>();
            //lst.Add(rtuId);
            //int gid = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;
            //SndOrderServer.OrderSnd(Wlst.Sr.EquipmentNewData.Services.EventIdAssign.NewDataArrive, lst, null, gid);

            //var info = Wlst.Sr.ProtocolPhone .ServerListen .wlst_cnt_request_snd_rtu_time ;//.ServerPart.wlst_asyntime_clinet_order_sendweekset ;
            //info.Args .Addr .Add(rtuId);
            //SndOrderServer.OrderSnd(info);

            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_orders;//.wlst_cnt_request_wj3090_measure;
            info.Args.Addr.Add(rtuId );
            info.WstRtuOrders.RtuIds.Add(rtuId );
            info.WstRtuOrders.Op = 11;
            SndOrderServer.OrderSnd(info,0,0,true);

            //var args = new PublishEventArgs
            //{
            //    EventType = PublishEventType.Core,
            //    EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.UserOperateRtu,
            //};
            //args.AddParams(rtuId);
            //args.AddParams(OpType.SndWeek);
            //EventPublish.PublishEvent(args);

            //Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
            //  rtuId, terminalInfo.RtuName, OperatrType.UserOperator, "发送周设置");




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
