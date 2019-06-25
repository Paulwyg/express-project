

using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreMims.ShowMsgInfo;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.CoreOne.Services;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
 
namespace Wlst.Ux.Nr6005Module.ZOrders.OpenCloseLight.ForTmlRightMenu
{

    public class OpenLightLoopControllerForTmlRightMenuBase : MenuItemBase
    {
        /// <summary>
        /// 最底层开灯服务，无继承不可用 需实现LoopId赋值
        /// </summary>
        public OpenLightLoopControllerForTmlRightMenuBase()
        {
            Id = -1;
            Description = "最底层开灯服务，无继承不可用，类型为终端右键菜单。";
            Tag = "开灯最底层服务，不可能";
            this.Text = "Null"; this.Classic = "右键菜单-监控设备通用";
            this.Tooltips = "最底层开灯服务，无继承不可用";
            LoopId = -1;
            base.Command = new RelayCommand(Ex, CanEx, true);
            //IsPrivilegLeave = true;
        }
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            var equipment = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (equipment == null || equipment.RtuStateCode  == 0) return false;
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
            return Wlst.Cr.CoreMims.Services.UserInfo.CanX(areaId);
        }
        bool CanEx()
        {
            //if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            var equipment = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (equipment == null || equipment.RtuStateCode  == 0) return false;
            return true;
        }

        public override void InitDataWhenBeforeUse(object argu)
        {
            base.InitDataWhenBeforeUse(argu);
           // base.IsEnabled = SysRunInfo.CanOperator;
            var ter = argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (ter == null) return;

            var rtuIdOrGrpId = ter.RtuId;var tmp=
                     Wlst.Sr.EquipmentInfoHolding .Services.ServicesGrpSingleInfoHold.GetRtuBelongGrp(ter.RtuId);
            // Wlst.Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuBelongGrp(ter.RtuId)


            if (tmp  !=null ) rtuIdOrGrpId = tmp .Item2 ;
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(ter.RtuId);
            //this.ExText = "-" +
            //    Wlst.Sr.TimeTableSystem.Services.WeekTimeTableInfoService.GetTmlLoopBandTimeTableNamex(areaId,rtuIdOrGrpId, LoopId);
            string extmp = "";
            if (Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(4001, 3, false))
            {
                var tt = Wlst.Sr.TimeTableSystem.Services.WeekTimeTableInfoService.GetTmlLoopBandTimeTableNamex(areaId,
                                                                                                              rtuIdOrGrpId,
                                                                                                              LoopId);
                if (string.IsNullOrEmpty(tt) == false) extmp =  tt;
            }
            if (Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(4001, 4, false))
            {
                var ters = argu as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                if ( ters == null ||ters.WjSwitchOuts == null ||ters.WjSwitchOuts.ContainsKey(LoopId)== false ) return;
                if (extmp == "")
                {
                    extmp = ters.WjSwitchOuts[LoopId].SwitchName;
                }
                else
                {
                    extmp += " - " +
                             ters.WjSwitchOuts[LoopId].SwitchName;
                }
            }
            this.ExText = extmp;
        }

        public int LoopId;

        private void Ex()
        {
            var terminalInfo = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (terminalInfo == null)
            {
                LogInfo .Log("无法执行开灯命令，参数错误....");
                return;
            }

            if (Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm ==1)
            {
                if (
                    Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                        "您将要对 (" + terminalInfo.RtuPhyId + " - " + terminalInfo.RtuName + ") 进行开灯操作，是否继续？", WlstMessageBoxType.YesNo) == WlstMessageBoxResults.No)
                {
                    return;
                }
            }
            else if (Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm == 2)
            {
                var sss = UMessageBoxWantPassWord.Show("密码验证", "请输入您的用户密码", "");
                if (sss == UMessageBoxWantPassWord.CancelReturn)
                {
                    return;
                }
                if (sss != UserInfo.UserLoginInfo.UserPassword)
                {
                    UMessageBox.Show("验证失败", "您输入的密码与本用户密码不匹配，请检查......",
                                                                       UMessageBoxButton.Yes);
                    return;
                }
            }
            var rtuId = terminalInfo.RtuId;
            if (rtuId < 1) return;

            OrderServer.OpenLight(rtuId, LoopId);

            //var args = new PublishEventArgs
            //{
            //    EventType = PublishEventType.Core,
            //    EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.UserOperateRtu,
            //};
            //args.AddParams(rtuId);
            //args.AddParams(OpType.RtuOpen );
            //args.AddParams(LoopId);
            //args.AddParams(terminalInfo.RtuModel);
            //args.AddParams(null);
            //EventPublish.PublishEvent(args);

           
            //var arg = new List<int>();
            //arg.Add(rtuId);

            //var data = new OpenCloseLightData();
            //data.Open = 1; //# 开关灯指令 0 关 1 开
            //data.Loops.Add(LoopId);
            //int gid = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;
            //SndOrderServer .OrderSnd(PPProtocol.EventIdAssign.OpenLight, arg, data, gid);
            //Wlst.Cr.Core.UtilityFunction.LogInfo.Log(terminalInfo.RtuName + "  开K" + LoopId + ",开灯命令已经发送");
            // Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(rtuId, terminalInfo.RtuName, LoopId,
            //                                                               PPProtocol.EventIdAssign.OpenLight, "开灯",
            //                                                               "等待", 1, null);
        }
    }
}
