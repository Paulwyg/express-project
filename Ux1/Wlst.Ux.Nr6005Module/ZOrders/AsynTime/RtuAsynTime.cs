using System;
using System.ComponentModel.Composition;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreMims.ShowMsgInfo;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.CoreOne.Services;
 
using Wlst.client;
using System.Collections.Generic;

namespace Wlst.Ux.Nr6005Module.ZOrders.AsynTime
{
    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RtuAsynTime : MenuItemBase
    {
        /// <summary>
        ///
        /// </summary>
        public RtuAsynTime()
        {
            Id = Services.MenuIdAssgin.MenuRtuAsynTime;
            this.Text = "对时";
            Description = "终端对时，类型为终端右键菜单。";
            Tag = "终端对时";
            this.Classic = "右键菜单-监控设备通用";
            this.Tooltips = "终端对时";
            base.Command = new RelayCommand(Ex, CanEx, true);
         //   IsPrivilegLeave = true;
        }
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            var equipment = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (equipment == null || equipment.RtuStateCode  == 0) return false;
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
            return Wlst.Cr.CoreMims.Services.UserInfo.CanX(areaId);
        }
        private bool CanEx()
        {
            //if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            var equipment = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (equipment == null || equipment.RtuStateCode  == 0) return false;
            return true;
        }



        private void Ex()
        {
            var terminalInfo = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (terminalInfo == null)
            {
                LogInfo.Log("无法执行对时命令，参数错误....");
                return;
            }
            var rtuId = terminalInfo.RtuId;
            if (rtuId < 1) return;

            if (rtuId < 1) return;

            //var arg = new List<int>();
            //arg.Add(rtuId);



            //SndOrderServer.OrderSnd(Nr6005Module.Services.EventIdAssign.RtuAsynTimeId, arg, null, 0);

            var info = Wlst.Sr.ProtocolPhone .LxRtu .wst_rtu_orders ;// .wlst_cnt_request_asyn_rtu_time ;//.ServerPart.wlst_asyntime_clinet_order_asynrtutime;
            info.WstRtuOrders .DtTime    = DateTime.Now.Ticks ;
            info.WstRtuOrders.RtuIds.Add(rtuId); info.WstRtuOrders.Op = 21;
            info.Args .Addr .Add(rtuId);
            SndOrderServer.OrderSnd(info,0,0,true );

            //var args = new PublishEventArgs
            //{
            //    EventType = PublishEventType.Core,
            //    EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.UserOperateRtu,
            //};
            //args.AddParams(rtuId);
            //args.AddParams(OpType.AsynTime);
            //args.AddParams(-1);
            //args.AddParams(terminalInfo.RtuModel);
            //args.AddParams(null);
            //EventPublish.PublishEvent(args);

            string rtuName = "" + rtuId + "号终端";
            var s = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems [rtuId];
            if (s != null)
            {
                rtuName = s.RtuName;
            }
           // LogInfo.Log(rtuName + "  对时命令已经发送");
             Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                rtuId, rtuName, OperatrType.UserOperator, "对时");
        }
    }

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RtuAsynTimeSingle : MenuItemBase
    {
        /// <summary>
        ///
        /// </summary>
        public RtuAsynTimeSingle()
        {
            Id = Services.MenuIdAssgin.MenuSingleAsynTime;
            this.Text = "对时";
            Description = "终端对时，类型为单终端右键菜单。";
            Tag = "终端对时";
            this.Classic = "右键菜单-单终端分组";
            this.Tooltips = "终端对时";
            base.Command = new RelayCommand(Ex, CanEx, true);
            //IsPrivilegLeave = true;
        }

        private bool CanEx()
        {
            return true;
        }
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            var equipment = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (equipment == null || equipment.RtuStateCode  == 0) return false;
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
            return Wlst.Cr.CoreMims.Services.UserInfo.CanX(areaId);
        }


        private void Ex()
        {
            var grpInfo = this.Argu as Wlst .client .GroupItemsInfo .GroupItem  ;
            var lst = new List<int>();
            int grpId = -1;
            if (grpInfo != null)
            {
                //LogInfo.Log("无法执行对时命令，参数错误....");
                //return;

                grpId = grpInfo.GroupId;
                if (grpId < 1) return;

                if (grpId < 1) return;
                lst = grpInfo.LstTml;// Wlst.Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GetGrpTmlList(gprId);
            }
            else
            {
                var regionInfo =this.Argu as List<int>;
                if (regionInfo == null) return;
                lst = regionInfo;
            }
           

           
            if (lst.Count == 0) return;

           // SndOrderServer.OrderSnd(Nr6005Module.Services.EventIdAssign.RtuAsynTimeId, lst, null, 0);


            var info = Wlst.Sr.ProtocolPhone .LxRtu .wst_rtu_orders ;// .wlst_cnt_request_asyn_rtu_time ;//.ServerPart.wlst_asyntime_clinet_order_asynrtutime ;
            info.Args.Addr.AddRange(lst); info.WstRtuOrders.Op = 21;
            info.WstRtuOrders .DtTime  = DateTime.Now.Ticks;
            info.WstRtuOrders.RtuIds.AddRange(lst);
            SndOrderServer.OrderSnd(info, 10, 6);

            if (grpInfo != null)
            {
                // LogInfo.Log(grpInfo.GroupName + "  对时命令已经发送");
                Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                    grpInfo.GroupId, grpInfo.GroupName, OperatrType.UserOperator,
                    "分组对时命令已经发送");
            }
            else
            {
                Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                    0,"地区分组", OperatrType.UserOperator,
                    "分组对时命令已经发送");
            }
        }
    }

    //[Export(typeof (IIMenuItem))]
    //[PartCreationPolicy(CreationPolicy.Shared)]
    //public class RtuAsynTimeMult : MenuItemBase
    //{
    //    /// <summary>
    //    ///
    //    /// </summary>
    //    public RtuAsynTimeMult()
    //    {
    //        Id = Services.MenuIdAssgin.MenuMultiAsynTime;
    //        this.Text = "对时";
    //        Description = "终端对时，类型为多终端右键菜单。";
    //        Tag = "终端对时";
    //        this.Classic = "右键菜单-多终端分组";
    //        this.Tooltips = "终端对时";
    //        base.Command = new RelayCommand(Ex, CanEx, true);
    //        //IsPrivilegLeave = true;
    //    }

    //    private bool CanEx()
    //    {
    //        return true;
    //    }
    //    public override bool IsCanBeShowRwx()
    //    {
    //        var equipment = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
    //        if (equipment == null || equipment.RtuStateCode  == 0) return false;
    //        return Wlst.Cr.CoreMims.Services.UserInfo.CanX(equipment.AreaId);
    //    }


    //    private void Ex()
    //    {
    //        var grpInfo = this.Argu as Wlst.client.GroupItemsInfo.GroupItem;
    //        ;
    //        if (grpInfo == null)
    //        {
    //            LogInfo.Log("无法执行对时命令，参数错误....");
    //            return;
    //        }
    //        var gprId = grpInfo.GroupId;
    //        if (gprId < 1) return;

    //        if (gprId < 1) return;

    //        var lst = Wlst.Sr.EquipmentGroupInfoHolding.Services.ServicesGrpMultiInfoHold.GetGrpTmlList(gprId);
    //        if (lst.Count == 0) return;

    //        //SndOrderServer.OrderSnd(Nr6005Module.Services.EventIdAssign.RtuAsynTimeId, lst, null, 0);


    //        var info = Wlst.Sr.ProtocolPhone .LxRtu .wst_rtu_orders ;// .wlst_cnt_request_asyn_rtu_time ;//.ServerPart.wlst_asyntime_clinet_order_asynrtutime;
    //        info.Args .Addr .AddRange(lst);
    //        info.WstRtuOrders.Op = 21;
    //        info.WstRtuOrders .DtTime  = DateTime.Now.Ticks;
    //        info.WstRtuOrders.RtuIds.AddRange(lst);
    //        SndOrderServer.OrderSnd(info, 10, 6);

    //      //  LogInfo.Log(grpInfo.GroupName + "  对时命令已经发送");
    //        Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
    //            grpInfo.GroupId, grpInfo.GroupName, OperatrType.UserOperator,
    //            "分组对时命令已经发送");

    //    }
    //}
    
     [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RtuCallTime : MenuItemBase
    {
        /// <summary>
        ///
        /// </summary>
         public RtuCallTime()
        {
            Id = Services.MenuIdAssgin.MenuRtuCallTime;
            this.Text = "召测时钟";
            Description = "终端召测时钟，类型为终端右键菜单。";
            Tag = "终端对时";
            this.Classic = "右键菜单-监控设备通用";
            this.Tooltips = "召测时钟";
            base.Command = new RelayCommand(Ex, CanEx, true);
            //IsPrivilegLeave = false ;
        }
         public override bool IsCanBeShowRwx()
         {
             if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
             var equipment = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
             if (equipment == null || equipment.RtuStateCode  == 0) return false;
             var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
             return Wlst.Cr.CoreMims.Services.UserInfo.CanX(areaId);
         }
        private bool CanEx()
         {
             //if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            var equipment = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (equipment == null || equipment.RtuStateCode  == 0) return false;
            return true;



        }



        private void Ex()
        {
            var terminalInfo = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (terminalInfo == null)
            {
                LogInfo.Log("无法执行对时命令，参数错误....");
                return;
            }
            var rtuId = terminalInfo.RtuId;
            if (rtuId < 1) return;

            if (rtuId < 1) return;

            //var arg = new List<int>();
            //arg.Add(rtuId);



            //SndOrderServer.OrderSnd(Nr6005Module.Services.EventIdAssign.RtuAsynTimeId, arg, null, 0);

            var info = Wlst.Sr.ProtocolPhone .LxRtu .wst_rtu_orders ;// .wlst_cnt_request_call_rtu_time ;//.ServerPart.wlst_asyntime_clinet_order_callrtutime ;
            info.Args .Addr .Add(rtuId);
            info.WstRtuOrders.Op = 22;
            info.WstRtuOrders.RtuIds.Add(rtuId);
            SndOrderServer.OrderSnd(info,0,0,true );


            //var args = new PublishEventArgs
            //{
            //    EventType = PublishEventType.Core,
            //    EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.UserOperateRtu,
            //};
            //args.AddParams(rtuId);
            //args.AddParams(OpType.ZcTime);
            //args.AddParams(-1);
            //args.AddParams(terminalInfo.RtuModel);
            //args.AddParams(null);
            //EventPublish.PublishEvent(args);



            string rtuName = "" + rtuId + "号终端";
            var s = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems [rtuId];
            if (s != null)
            {
                rtuName = s.RtuName;
            }
           // LogInfo.Log(rtuName + "  招测时钟已经发送");
             Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                rtuId, rtuName, OperatrType.UserOperator, "时钟");
        }
    }

}
