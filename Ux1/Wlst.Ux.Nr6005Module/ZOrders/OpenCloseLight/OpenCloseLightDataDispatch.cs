using System;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;


namespace Wlst.Ux.Nr6005Module.ZOrders.OpenCloseLight
{
    public class OpenCloseLightDataDispatch
    {

        //internal static bool IsControlCenterManagDemo2TakeOverOcOrderShow = false;

        public OpenCloseLightDataDispatch()
        {
            InitAction();
        }


        public void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .LxRtu .wst_svr_ans_cnt_order_rtu_open_close_light ,// .wlst_svr_ans_cnt_wj3090_order_open_close_light ,//.ClientPart.wlst_OpenCloseLight_server_ans_clinet_order_opencloseLight ,
                ExExecuteOpenLight,
                typeof(OpenCloseLightDataDispatch), this);

            //ProtocolServer.RegistProtocol(
            //    Wlst.Sr.ProtocolCnt.ClientPart.wlst_OpenCloseLight_server_ans_clinet_order_closeLight,
            //    ExExecuteCloseLight,
            //    typeof(OpenCloseLightDataDispatch), this);

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .LxRtu .wst_rtu_orders ,// .wlst_svr_ans_cnt_request_asyn_rtu_time ,//.ClientPart.wlst_asyntime_server_ans_client_order_asynrtutime,
                ExAsynTime,
                typeof(OpenCloseLightDataDispatch), this);
        }


        /// <summary>
        /// 具体执行
        /// </summary>
        // private void ExExecuteOpenLight(List< int > lst,object data,int guid)
        private void ExExecuteOpenLight(string session, Wlst .mobile .MsgWithMobile  args)
        {
            //if (IsControlCenterManagDemo2TakeOverOcOrderShow) return;

            var datax = args.WstRtuSvrAnsCntOrderOpenCloseLight  ;
            var lst = args.Args .Addr ;
            if (lst == null) return;
         

            //var info = data as RfData;
            //if (info == null) return;

            ////发布事件，控制中心共享数据
            //var ar = new PublishEventArgs()
            //{
            //    EventId = Cr.CoreOne.CoreIdAssign.EventIdAssign.OpenOrCloseLightReceiveEventId,
            //    EventType = PublishEventType.Core
            //};
            //ar.AddParams(datax.RtuId);
            //ar.AddParams(datax.LoopId);
            //ar.AddParams(datax.IsOpen);
            //EventPublish.PublishEvent(ar);

            string strisk = datax.IsSingle == false ? "组合开关灯命令" : datax.IsOpen ? "开灯" : "关灯";
          //  string str = strisk+"终端回复：终端地址：";
            foreach (var f in lst)
            {
                string tmlName = "Reserve";
               // str += f;
                var g = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( f);
                if (g != null)
                {

                  //  str += ":" + g.RtuName + ";";
                    tmlName = g.RtuName;

                }
              //  else str += ";";
                //if(info !=null )

                 Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                    f, tmlName, Wlst .Cr .CoreMims .ShowMsgInfo .OperatrType.ServerReply, strisk+"成功");
            }
            //str = str.Substring(0, str.Length - 1);
            //str = str + "" +strisk+ "执行回复。";
          //  LogInfo.Log(str);
        }

       

        //#endregion

        private void ExAsynTime(string session, Wlst .mobile .MsgWithMobile  args)
        {
            if (args.WstRtuOrders == null || args.WstRtuOrders.Op != 21) return;

            var lst = args.WstRtuOrders.RtuIds;

            if (lst == null) return;

            var ar = new PublishEventArgs
            {
                EventId = Cr.CoreOne.CoreIdAssign.EventIdAssign.AsyncTimeEventId,
                EventType = PublishEventType.Core
            };
            ar.AddParams(lst);
            EventPublish.PublishEvent(ar);

            foreach (var f in lst)
            {
                string tmlName = "Reserve";
                var g = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( f);
                if (g != null)
                {

                    tmlName = g.RtuName;

                }
                 Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                    f, tmlName, Wlst .Cr .CoreMims .ShowMsgInfo .OperatrType.ServerReply, "对时成功");

            }
        }
    }
}
