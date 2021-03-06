﻿using System.Collections.Generic;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreMims.ShowMsgInfo;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.CoreOne.Services;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.client;


namespace Wlst.Ux.WJ3005Module.ZOrders.OpenCloseLight.ForMulitTreeGrpMenu
{

    public class CloseLightLoopControllerForMulitGrpRightMenuBase : MenuItemBase
    {
        /// <summary>
        /// 最底层开灯服务，无继承不可用 需实现LoopId赋值
        /// </summary>
        public CloseLightLoopControllerForMulitGrpRightMenuBase()
        {
            Id = -1;
            Description = "最底层关灯服务，无继承不可用，类型为终端右键菜单。";
            Tag = "关灯最底层服务，不可能";
            this.Text = "Null";
            this.Tooltips = "最底层关灯服务，无继承不可用";
            LoopId = -1; this.Classic = "右键菜单-多终端分组";
            base.Command = new RelayCommand(Ex,CanEx ,true);
            //IsPrivilegLeave = true;
        }
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanX();
            var equipment = this.Argu as Wlst.Sr .EquipmentInfoHolding.Model  .GroupInformation ;// .IIEquipmentInfo;
            if (equipment == null  ) return false;
            return true;
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanX(equipment.AreaId);
        }
        bool CanEx()
        {
            return true;
        }

        public override void InitDataWhenBeforeUse(object argu)
        {
            base.InitDataWhenBeforeUse(argu);
            //base.IsEnabled = SysRunInfo.CanOperator;
        }

        public int LoopId;

        private void Ex()
        {
            var grpInfo = this.Argu as Wlst.client.GroupItemsInfo.GroupItem;
            if (grpInfo == null)
            {
                LogInfo.Log("无法执行开灯命令，参数错误....");
                return;
            }
            if (Wlst.Sr.EquipmentInfoHolding.Services.Others.CloseLightSecondConfirm ==1)
            {
                if (
                    Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                        "您将要进行开关灯操作，是否继续？", WlstMessageBoxType.YesNo) == WlstMessageBoxResults.No)
                {
                    return;
                }
            }
            else if (Wlst.Sr.EquipmentInfoHolding.Services.Others.CloseLightSecondConfirm == 2)
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
            var gprId = grpInfo.GroupId;
            if (gprId < 1) return;
          //  var lst = Wlst.Sr.EquipmentGroupInfoHolding.Services.ServicesGrpMultiInfoHold .GetGrpTmlList(gprId);
            if (grpInfo .LstTml .Count == 0) return;
            var lstslt = new List<int>();
            foreach (var t in grpInfo.LstTml)
            {
                if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems .ContainsKey(t) &&
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems [t].RtuStateCode  == 2
                    ) lstslt.Add(t);
            }

            //var data = new OpenCloseLightData();
            //data.Open = 0; //# 开关灯指令 0 关 1 开
            //data.Loops.Add(LoopId);
            //int gid = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;
            //SndOrderServer.OrderSnd(WJ3005Module.Services.EventIdAssign.CloseLight, lst, data, gid);

            var info = Wlst.Sr.ProtocolPhone .LxRtu .wst_cnt_order_rtu_open_close_light ;// .wlst_cnt_wj3090_order_open_close_light ;//.ServerPart.wlst_OpenCloseLight_clinet_order_opencloseLight  ;
            info.Args .Addr .AddRange(lstslt);
            info.WstRtuCntOrderOpenCloseLight.Loops.Add(LoopId);
            info.WstRtuCntOrderOpenCloseLight.IsOpen  = 2; //# 开关灯指令 0 关 1 开
            SndOrderServer.OrderSnd(info);


            LogInfo.Log(grpInfo.GroupName + " 分组关K" + LoopId + ",关灯命令已经发送");
            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
               grpInfo.GroupId, grpInfo.GroupName, OperatrType.UserOperator, "分组关K" + LoopId);
        }

    }
}
