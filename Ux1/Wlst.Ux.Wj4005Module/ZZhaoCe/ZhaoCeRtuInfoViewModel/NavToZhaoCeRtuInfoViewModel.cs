using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;


namespace Wlst.Ux.WJ4005Module.ZZhaoCe.ZhaoCeRtuInfoViewModel
{

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToZhaoCeRtuInfoViewModel : MenuItemBase
    {
        public NavToZhaoCeRtuInfoViewModel()
        {
            Id = WJ4005Module.Services.MenuIdAssgin.NavToZhaoCeRtuInfoViewModelId;
            Text = "召测终端参数";
            Tag = "召测终端参数";
            Description = "召测终端参数，ID 为" + WJ4005Module.Services.MenuIdAssgin.NavToZhaoCeRtuInfoViewModelId;
                // Infrastructure.IdAssign.MenuIdAssign.NavToTmlInfoSetforWj3090Id;
            Tooltips = "召测终端参数";
            this.Classic = "右键菜单-监控设备通用";

            this.IsCheckable = false;
            this.IsEnabled = true;
            this.Command = new RelayCommand(Ex,CanEx ,true );
            //this.IsPrivilegLeave = true;

        }

        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            var equipment = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (equipment == null || equipment.RtuStateCode == 0) return false;
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
            return Wlst.Cr.CoreMims.Services.UserInfo.CanR(areaId);
        }
        private bool CanEx()
        {
            //if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            var equipment = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (equipment == null || equipment.RtuStateCode == 0) return false;
            return true;
        }

        protected void Ex()
        {
            var equipment = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (equipment == null) return;
            int rtuId = equipment.RtuId;
            if (rtuId < 1) return;

            //var arg = new List<int>();
            //arg.Add(rtuId);
            //int gid = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;
           


            var nt = Wlst.Sr.ProtocolPhone.LxRtu.wst_zc_rtu_info;// .wlst_cnt_request_zc_rtu_k1k3 ;//.ServerPart.wlst_ZhaoCeRtuWeekSet_clinet_order_ZcRtuWeekSetInfo;
            nt.Args.Addr.Add(rtuId);
            nt.WstRtuZcInfo.Op = 21;
            nt.WstRtuZcInfo.RtuId = rtuId;

            SndOrderServer.OrderSnd(nt, 0, 0, true);

            //Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegion(
            //    WJ4005Module.Services.ViewIdAssign.ZhaoCeRtuInfoViewId, true);

            
           // LogInfo.Log(equipment.RtuName + "召测终端参数命令已经发送");

            //var args = new PublishEventArgs
            //{
            //    EventType = PublishEventType.Core,
            //    EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.UserOperateRtu,
            //};
            //args.AddParams(rtuId);
            //args.AddParams(OpType.ZcRtuPara);
            //EventPublish.PublishEvent(args);


             Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
           rtuId, equipment.RtuName, Wlst .Cr .CoreMims .ShowMsgInfo .OperatrType.UserOperator, "召测终端参数");
        }

    }
}
