using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Wlst.Sr.EquipmentInfoHolding.Services;


namespace Wlst.Sr.EquipmentInfoHolding
{
    [ModuleExport(typeof (SrEquipmentInfoHolding))]
    public class SrEquipmentInfoHolding : IModule
    {
        #region IModule 成员

        public void Initialize()
        {
            //new PPProtocol().RegistProtocol();

            //    InfoHold.EquipmentDataInfoHold.MySlef.InitStart();
            Services.EquipmentDataInfoHold.MySlef.InitStart();
            AreaInfoHold.MySlef.InitStartService();
            Services.ServicesGrpSingleInfoHold.InitLoad();

            //加载地区 lvf 2019年5月6日16:56:24
            Services.ServiceGrpRegionInfoHold.InitLoad();

            ServicesGrpMulitInfoHold.MySlef.InitSvr();
            ServicesGrpMultiInfoHoldNew.InitLoad();
            Services.RunningInfoHold.InitServices();
            //new EquipmentRunningInfoHoldingExtend().InitLoad();

            Services.RtuSxxRuleInstancesHold.Myself.InitLoad();

            AreaEmeHold.MySlef.InitStartService();


            //20181229  优化添加
            int x = Wlst.Cr.Core.CoreServices.SystemOption.GetOption(1);
            //if (x == 1)
            //{
            //    Wlst.Cr.PPProtocolSvrCnt.Server.ProtocolServer.RunInUi = true;
            //}
            x = Wlst.Cr.Core.CoreServices.SystemOption.GetOption(2);
            if (x == 1)
            {
                Wlst.Cr.PPProtocolSvrCnt.Server.ProtocolServer.InitDebugTest = true;
               // Wlst.Cr.Coreb.EventHelper.EventPublisher.InitDebugTest = true;
            }
            //x = Wlst.Cr.Core.CoreServices.SystemOption.GetOption(3);
            //if (x == 1)
            //{
            //    Wlst.Cr.Coreb.EventHelper.EventPublisher.RunInUi = false ;
            //}
        }

        //View i36N id 11040000~11049999

        #endregion
    }
}