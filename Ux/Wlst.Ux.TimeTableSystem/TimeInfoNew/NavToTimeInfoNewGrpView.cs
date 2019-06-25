using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.CoreOne.Services;

namespace Wlst.Ux.TimeTableSystem.TimeInfoNew
{



    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToTimeInfoNewGrpView : MenuItemBase //TreeModuleGrpMulitManageView
    {
        public NavToTimeInfoNewGrpView()
        {
            Id = TimeTableSystem.Services.MenuIdAssgin.NavToNavToTimeInfoNewGrpViewId;
            //Infrastructure.IdAssign.MenuIdAssign.NavToTimeTableBandingViewId;
            Text = "分组时间设置";
            this.Classic = "多终端树右键菜单";
            Tag = "分组时间设置";
            Description = "分组时间设置，ID 为" +
                          TimeTableSystem.Services.MenuIdAssgin.NavToNavToTimeInfoNewGrpViewId;
            //Infrastructure.IdAssign.MenuIdAssign.NavToTimeTableBandingViewId;
            Tooltips = "设置分组时间设置";
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
                //var equipment = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
                //if (equipment == null || equipment.RtuStateCode == 0) LogInfo.Log("无法打开时间设置，参数错误....");
                //var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
                //var rtuid = equipment.RtuId;
                //var grpid = -1;

                //var notgrp = Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(areaId);
                //if (!notgrp.Contains(rtuid))
                //{
                //    grpid = Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuBelongGrp(rtuid).Item2;
                //}
                //else
                //{
                //    grpid = rtuid;
                //}

                //交叉分组
                var rtusLst  = this.Argu as List<int>;

                this.ExNavWithArgs(
                    TimeTableSystem.Services.ViewIdAssign.TimeInfoMnViewId,
                    rtusLst);
                return;
            }
            var gprId = grpInfo.GroupId;
            if (gprId < 1) return;
            var areaid = grpInfo.AreaId;

            this.ExNavWithArgs(
                TimeTableSystem.Services.ViewIdAssign.TimeInfoMnViewId,
                areaid, gprId);
        }

        //var rtulst = Wlst.Sr.EquipmentInfoHolding.Services.ServiceGrpRegionInfoHold.GetRtulstByGrpRegionId(AreaId, _father.NodeId, NodeId);
        //        if (rtulst == null || rtulst.Count == 0) return;
        //        var rtlst = (from t in rtulst orderby t.Item1 select t.Item1).ToList();
        //var sortLst =
        //                   Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(rtlst);


    }
}
