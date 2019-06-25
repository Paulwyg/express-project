using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.CoreOne.Services;

namespace Wlst.Ux.TimeTableSystem.TimeInfoMn
{

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToTimeInfoMnGrpView : MenuItemBase //TreeModuleGrpMulitManageView
    {
        public NavToTimeInfoMnGrpView()
        {
            Id = TimeTableSystem.Services.MenuIdAssgin.NavToNavToTimeInfoMnGrpViewId;
            //Infrastructure.IdAssign.MenuIdAssign.NavToTimeTableBandingViewId;
            Text = "当前时间设置";
            this.Classic = "多终端树右键菜单";
            Tag = "当前时间设置";
            Description = "当前时间设置，ID 为" +
                          TimeTableSystem.Services.MenuIdAssgin.NavToNavToTimeInfoMnGrpViewId;
            //Infrastructure.IdAssign.MenuIdAssign.NavToTimeTableBandingViewId;
            Tooltips = "设置当前时间设置";
            base.IsCheckable = false;
            base.IsEnabled = true;
            base.Command = new RelayCommand(Ex, CanEx, true);
            // this.IsPrivilegLeave = true;
        }

        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;

            var grpInfo = this.Argu as Wlst.client.GroupItemsInfo.GroupItem;
            if (grpInfo == null) return false;
            var areaid = grpInfo.AreaId;

            return Wlst.Cr.CoreMims.Services.UserInfo.CanR(areaid);
        }

        private bool CanEx()
        {
            return true;
        }

        protected void Ex()
        {
            var grpInfo = this.Argu as Wlst.client.GroupItemsInfo.GroupItem;
            if (grpInfo == null)
            {
                var equipment = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
                if (equipment == null || equipment.RtuStateCode == 0) LogInfo.Log("无法打开时间设置，参数错误....");
                var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
                var rtuid = equipment.RtuId;
                var grpid = -1;

                var notgrp = Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(areaId);
                if (!notgrp.Contains(rtuid))
                {
                    grpid = Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuBelongGrp(rtuid).Item2;
                }
                else
                {
                    grpid = rtuid;
                }

                this.ExNavWithArgs(
                    TimeTableSystem.Services.ViewIdAssign.TimeInfoMnViewId,
                    areaId, grpid);
                return;
            }
            var gprId = grpInfo.GroupId;
            if (gprId < 1) return;
            var areaid = grpInfo.AreaId;

            this.ExNavWithArgs(
                TimeTableSystem.Services.ViewIdAssign.TimeInfoMnViewId,
                areaid, gprId);
        }

    }
}
