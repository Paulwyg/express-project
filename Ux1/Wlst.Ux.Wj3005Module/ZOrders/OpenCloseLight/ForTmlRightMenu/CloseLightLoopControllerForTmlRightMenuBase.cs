

using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreMims.ShowMsgInfo;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.CoreOne.Services;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;


namespace Wlst.Ux.WJ3005Module.ZOrders.OpenCloseLight.ForTmlRightMenu
{

    public class CloseLightLoopControllerForTmlRightMenuBase : MenuItemBase
    {
        /// <summary>
        /// 最底层开灯服务，无继承不可用 需实现LoopId赋值
        /// </summary>
        public CloseLightLoopControllerForTmlRightMenuBase()
        {
            Id = -1;
            Description = "最底层关灯服务，无继承不可用，类型为终端右键菜单。";
            Tag = "关灯最底层服务，不可能";
            this.Text = "Null";
            this.Tooltips = "最底层关灯服务，无继承不可用";
            LoopId = -1; this.Classic = "右键菜单-监控设备通用";
            base.Command = new RelayCommand(Ex, CanEx, true);
           // IsPrivilegLeave = true;
        }
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanX();
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
            //base.IsEnabled = SysRunInfo.CanOperator;
            var ter = argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;

   
            if(ter ==null ) return;
            var rtuIdOrGrpId = ter.RtuId;
            //var tmp=    Wlst.Sr.EquipmentInfoHolding .Services.ServicesGrpSingleInfoHold.GetRtuBelongGrp(ter.RtuId);


            //if (tmp  !=null  ) rtuIdOrGrpId = tmp .Item2 ;
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(ter.RtuId);
            string extmp = "";
            if (Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(4001, 3, false))
            {
                var tt = Wlst.Sr.TimeTableSystem.Services.WeekTimeTableInfoService.GetTmlLoopBandTimeTableNamex(areaId,
                                                                                                                rtuIdOrGrpId,
                                                                                                                LoopId);
                if (string.IsNullOrEmpty(tt)==false ) extmp = tt;
            }
            if (Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(4001, 4, false))
            {
                var ters = argu as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                if (ters == null || ters.WjSwitchOuts == null || ters.WjSwitchOuts.ContainsKey(LoopId) == false) return;
                if ( extmp =="")
                {
                    extmp = ters.WjSwitchOuts[LoopId].SwitchName;
                }else
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
            string lightIsAlwaysOnString = string.Empty;

            var terminalInfo = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;

            if (terminalInfo == null)
            {
                LogInfo.Log("无法执行关灯命令，参数错误....");
                return;
            }
            //西安 特殊功能   城市代号为5 lvf 2018年4月12日13:07:15
            if(Wlst.Sr.EquipmentInfoHolding.Services.Others.CityNum ==5)
            {
                var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(terminalInfo.RtuId);

                if (LoopId == 0)
                {
                    for (int i = 1; i < 9; i++)
                    {
                        var tmp =
                            Wlst.Sr.TimeTableSystem.Services.WeekTimeTableInfoService.
                                GetTmlLoopBandTimeTableTodayOpenCloseTimex
                                (areaId, terminalInfo.RtuId, i);

                        if (tmp != null)
                        {
                            if (tmp.TimeOnOff[0].Item2 == 1500)
                            {
                                lightIsAlwaysOnString = terminalInfo.RtuName + " 有长明灯K" + i + "回路 " + terminalInfo.GetLoopName(i);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    var tmp =
                        Wlst.Sr.TimeTableSystem.Services.WeekTimeTableInfoService.
                            GetTmlLoopBandTimeTableTodayOpenCloseTimex
                            (areaId, terminalInfo.RtuId, LoopId);

                    if (tmp != null)
                    {

                        if (tmp.TimeOnOff[0].Item2 == 1500)
                        {
                            lightIsAlwaysOnString = terminalInfo.RtuName + " 有长明灯K" + LoopId + "回路 " +
                                                    terminalInfo.GetLoopName(LoopId);
                        }
                    }
                }

            }


            if (Wlst.Sr.EquipmentInfoHolding.Services.Others.CloseLightSecondConfirm == 0)
            {
                if (lightIsAlwaysOnString != string.Empty)
                {
                    if (
                        Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                            "您将要对 (" + terminalInfo.RtuPhyId + " - " + terminalInfo.RtuName + ") 进行关灯操作，是否继续？\n" + lightIsAlwaysOnString, WlstMessageBoxType.YesNo) ==
                        WlstMessageBoxResults.No)
                    {
                        return;
                    }
                }
            }
            else if (Wlst.Sr.EquipmentInfoHolding.Services.Others.CloseLightSecondConfirm == 1)
            {
                if (
                    Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                        "您将要对 (" + terminalInfo.RtuPhyId + " - " + terminalInfo.RtuName + ") 进行关灯操作，是否继续？\n" + lightIsAlwaysOnString, WlstMessageBoxType.YesNo) ==
                    WlstMessageBoxResults.No)
                {
                    return;
                }
            }
            else if (Wlst.Sr.EquipmentInfoHolding.Services.Others.CloseLightSecondConfirm == 2)
            {


                var sss = UMessageBoxWantPassWord.Show("密码验证", "请输入您的用户密码\n" + lightIsAlwaysOnString, "");
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
            OrderServer.CloseLight(terminalInfo.RtuId, LoopId, lightIsAlwaysOnString != string.Empty ? true : false);

            //var args = new PublishEventArgs
            //{
            //    EventType = PublishEventType.Core,
            //    EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.UserOperateRtu,
            //};
            //args.AddParams(rtuId);
            //args.AddParams(OpType.RtuClose);
            //args.AddParams(LoopId);
            //args.AddParams(terminalInfo.RtuModel);
            //args.AddParams(null);
            //EventPublish.PublishEvent(args);

            //var arg = new List<int>();
            //arg.Add(rtuId);
            //var data = new OpenCloseLightData();
            //data.Open = 0; //# 开关灯指令 0 关 1 开
            //data.Loops.Add(LoopId);
            //int gid = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;
            //SndOrderServer .OrderSnd(PPProtocol.EventIdAssign.CloseLight, arg, data, gid);
            //Wlst.Cr.Core.UtilityFunction.LogInfo.Log(terminalInfo .RtuName  + "  关K" + LoopId + ",关灯命令已经发送");
            // Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(rtuId, terminalInfo.RtuName, LoopId,
            //                                                               PPProtocol.EventIdAssign.CloseLight , "关灯",
            //                                                               "等待", 1, null);
        }

    }
}
