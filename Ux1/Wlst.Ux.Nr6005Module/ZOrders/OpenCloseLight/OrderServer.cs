using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Services;

namespace Wlst.Ux.Nr6005Module.ZOrders.OpenCloseLight
{
   public  class OrderServer
   {
       private static long _guidopen = 0;
       private static long _guidclose = 0;
       public static void OpenLight(int rtuId, int loop)
       {
           if (rtuId < 1) return;

           //var arg = new List<int>();
           //arg.Add(rtuId);


           //var data = new OpenCloseLightData();
           //data.Open = 1; //# 开关灯指令 0 关 1 开
           //data.Loops.Add(loop);
           //int gid = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;
           //SndOrderServer.OrderSnd(Nr6005Module.Services.EventIdAssign.OpenLight, arg, data, gid);

           //var info = Wlst.Sr.ProtocolPhone .ServerListen .wlst_cnt_wj3090_order_open_close_light ;//.ServerPart.wlst_OpenCloseLight_clinet_order_opencloseLight ;
           //info.WstCntOrderWj3090OpenCloseLigth.Open = 1;//# 开关灯指令 0 关 1 开
           //info.Args .Addr .Add(rtuId);
           //if (info.Head .Gid  <= _guidopen) info.Head .Gid  = _guidopen + 1;
           //_guidopen = info.Head .Gid ;

           //info.WstCntOrderWj3090OpenCloseLigth.Loops.Add(loop);
           //SndOrderServer.OrderSnd(info);



           var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_cnt_order_rtu_open_close_light;// .wlst_cnt_wj3090_order_open_close_light ;//.ServerPart.wlst_OpenCloseLight_clinet_order_opencloseLight  ;
           info.Args.Addr.Add( rtuId );
           info.WstRtuCntOrderOpenCloseLight.Loops.Add(loop);
           info.WstRtuCntOrderOpenCloseLight.IsOpen = 1; //# 开关灯指令 0 关 1 开
           if (info.Head.Gid <= _guidopen) info.Head.Gid = _guidopen + 1;
           SndOrderServer.OrderSnd(info, 0, 0, true);

           string rtuName = "" + rtuId + "号终端";
           var s = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( rtuId);
           if (s != null)
           {
               rtuName = s.RtuName;
           }
           LogInfo.Log(rtuName + "  开K" + loop + ",开灯命令已经发送");
            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
            rtuId, rtuName, Wlst .Cr .CoreMims .ShowMsgInfo .OperatrType.UserOperator, "开启回路K" + loop);
       }

       public static void CloseLight(int rtuId, int loop, bool ifLightIsAlwaysOn)
       {
           if (rtuId < 1) return;

           //var arg = new List<int>();
           //arg.Add(rtuId);

           //var data = new OpenCloseLightData();
           //data.Open = 0; //# 开关灯指令 0 关 1 开
           //data.Loops.Add(loop);
           //int gid = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;

           //SndOrderServer.OrderSnd(Nr6005Module.Services.EventIdAssign.CloseLight, arg, data, gid);


           //var info = Wlst.Sr.ProtocolPhone .ServerListen .wlst_cnt_wj3090_order_open_close_light ;//.ServerPart.wlst_OpenCloseLight_clinet_order_opencloseLight;
           //info.WstCntOrderWj3090OpenCloseLigth.Open = 2;//# 开关灯指令 0 关 1 开
           //if (info.Head .Gid  <= _guidclose) info.Head .Gid  = _guidclose + 1;
           //_guidclose = info.Head .Gid ;

           //info.Args .Addr .Add(rtuId);
           //info.WstCntOrderWj3090OpenCloseLigth.Loops.Add(loop);

           //SndOrderServer.OrderSnd(info);



           var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_cnt_order_rtu_open_close_light;// .wlst_cnt_wj3090_order_open_close_light ;//.ServerPart.wlst_OpenCloseLight_clinet_order_opencloseLight  ;
           info.Args.Addr.Add(rtuId);

           info.Args.Cid = ifLightIsAlwaysOn == true ? 100 : 0;

           info.WstRtuCntOrderOpenCloseLight.Loops.Add(loop);
           info.WstRtuCntOrderOpenCloseLight.IsOpen = 2; //# 开关灯指令 0 关 1 开
           if (info.Head.Gid <= _guidopen) info.Head.Gid = _guidopen + 1;
           SndOrderServer.OrderSnd(info, 0, 0, true);

           string rtuName = "" + rtuId + "号终端";
           var s = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(rtuId);
           if (s != null)
           {
               rtuName = s.RtuName;
           }

           LogInfo.Log(rtuName + "  关K" + loop + ",关灯命令已经发送");
           Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
           rtuId, rtuName, Wlst.Cr.CoreMims.ShowMsgInfo.OperatrType.UserOperator, "关闭回路K" + loop);
       }
    }
}
