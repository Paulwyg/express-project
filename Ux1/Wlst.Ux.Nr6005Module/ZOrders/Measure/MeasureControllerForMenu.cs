using System.ComponentModel.Composition;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreMims.ShowMsgInfo;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;


namespace Wlst.Ux.Nr6005Module.ZOrders.Measure
{

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MeasureControllerForMenu : MenuItemBase
    {

        public MeasureControllerForMenu()
        {
            Id = Services.MenuIdAssgin.MneuMeasureControllerForMenuId;// Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMenuId;
            Tag = "选测";
            Description = "选测终端，ID为" + Services.MenuIdAssgin.MneuMeasureControllerForMenuId// Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMenuId 
            + ",类型为终端右键菜单。";
            Text = "选测";
            this.Classic = "右键菜单-监控设备通用";
            Tooltips = "选测终端数据";
            base.Command = new RelayCommand(Ex, CanEx,true );
        }
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanR();
            var equipment = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (equipment == null || equipment.RtuStateCode  == 0) return false;
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
            return Wlst.Cr.CoreMims.Services.UserInfo.CanR(areaId);
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

            //var info = Wlst.Sr.ProtocolPhone.ServerListen.wlst_cnt_request_wj3090_measure;
            //info.Args.Addr.Add(rtuId);


            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_orders;//.wlst_cnt_request_wj3090_measure;
            info.Args.Addr.Add(rtuId );
            info.WstRtuOrders.RtuIds.Add(rtuId );
            info.WstRtuOrders.Op = 31;
            SndOrderServer.OrderSnd(info, 0, 0, true);


            //var args = new PublishEventArgs
            //{
            //    EventType = PublishEventType.Core,
            //    EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.UserOperateRtu,
            //};
            //args.AddParams(rtuId);
            //args.AddParams(OpType.RtuMeasure);
            //args.AddParams(-1);
            //args.AddParams(terminalInfo.RtuModel);
            //args.AddParams(null);
            //EventPublish.PublishEvent(args);


            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                rtuId, terminalInfo.RtuName, OperatrType.UserOperator, "选测终端");


            if (Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801,20, false))//IsCloseMeasureNewData 是否需要关闭最新数据框  lvf 2018年3月29日08:43:22
            {
                EventPublish.PublishEvent(new PublishEventArgs() { EventType = "MainWindow.Measure.close", EventAttachInfo = rtuId });//EventAttachInfo 选测终端id发布
            }else
            {
                EventPublish.PublishEvent(new PublishEventArgs() { EventType = "MainWindow.Measure.show" });
            }
            //if (OnMeasurShowDada)
                
            //  "MainWindow.Measure.show"

        }



        private static bool isLoad = false;
        private static bool mesf;

        public static bool OnMeasurShowDada
        {
            get
            {
                if (isLoad) return mesf;
                else
                {
                    mesf = GetOnMeasurShowDada();
                    isLoad = true;
                    return mesf;
                }
            }
        }

        private static bool GetOnMeasurShowDada()
        {

            var info =
                Wlst.Ux.Nr6005Module.Wj3005TmlInfoSetViewModel.ViewModel.NewDataSettingViewModel.
                    LoadNewDataLenghtSetConfg();
            return info.Item7.OnMeasureShowData;
        }

        protected bool CanEx()
        {
            var terminalInfo = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (terminalInfo == null) return false;
            if (terminalInfo.RtuStateCode == 0) return false;
            //var rtuId = terminalInfo.RtuId;
            //if (EquipmentRunningInfoHolding.TmlRunningInfoDictionary.ContainsKey(rtuId))
            //{
            //    var t = EquipmentRunningInfoHolding.TmlRunningInfoDictionary[rtuId];
            return true;

         
        }

    }
}
