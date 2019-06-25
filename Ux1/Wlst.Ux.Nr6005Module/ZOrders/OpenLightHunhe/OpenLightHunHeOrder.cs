using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreMims.ShowMsgInfo;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.CoreOne.Services;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Ux.Nr6005Module.ZOrders.OpenCloseLight.ForSingleTreeGrpMenu;

namespace Wlst.Ux.Nr6005Module.ZOrders.OpenLightHunhe
{

    #region 开灯

    public class OpenLightHunHeOrderMenuBase : MenuItemBase
    {
        /// <summary>
        /// 最底层开灯服务，无继承不可用 需实现LoopId赋值
        /// </summary>
        public OpenLightHunHeOrderMenuBase()
        {
            Id = -1;
            Description = "最底层开灯服务，无继承不可用，类型为地图混合开关灯。";
            Tag = "开灯最底层服务";
            this.Text = "Null";
            this.Classic = "右键菜单-终端地图混合操作-30500";
            this.Tooltips = "最底层开灯服务，无继承不可用";
            LoopId = -1;
            base.Command = new RelayCommand(Ex, CanEx, true);
           // IsPrivilegLeave = true;
        }
        public override bool IsCanBeShowRwx()
        {
            //if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            var equipment = this.Argu as Wlst .Sr .EquipmentInfoHolding .Model .WjParaBase ;
            if (equipment == null || equipment.RtuStateCode  == 0) return false;
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
            return Wlst.Cr.CoreMims.Services.UserInfo.CanX(areaId);
        }
        private bool CanEx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            var lst = this.Argu as List<int>;
            if (lst == null || lst.Count == 0)
            {
                return false;
            }
            return true;
        }

        public int LoopId;

        private void Ex()
        {
            var lst = this.Argu as List<int>;
            if (lst == null)
            {
                LogInfo.Log("无法执行混合开灯命令，无法定位参数，参数错误....");
                return;
            }





            var lstslt = new List<int>();
            string name = "";
            foreach (var t in lst)
            {
                if (
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems .
                        ContainsKey(t) &&
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems [t].
                        RtuStateCode  == 2
                    )
                {
                    var infos = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[t];
                    name = infos.RtuPhyId + " - " + infos.RtuName;
                    lstslt.Add(t);

                }
            }
            if (lst.Count == 0) return;
            if (lst.Count > 1) name += "等，" + lst.Count + "个终端";
            if (Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm==1)
            {
                if (
                    Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                        "您将要 对(" + name + ") 进行开关灯操作，是否继续？", WlstMessageBoxType.YesNo) == WlstMessageBoxResults.No)
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



            var info = Wlst.Sr.ProtocolPhone .LxRtu .wst_cnt_order_rtu_open_close_light ;// .wlst_cnt_wj3090_order_open_close_light ;//.ServerPart.wlst_OpenCloseLight_clinet_order_opencloseLight;
            info.Args .Addr .AddRange(lstslt);
            info.WstRtuCntOrderOpenCloseLight.Loops.Add(LoopId);
            info.WstRtuCntOrderOpenCloseLight.IsOpen = 1; //# 开关灯指令 0 关 1 开
           
            SndOrderServer.OrderSnd(info);


            LogInfo.Log("混合开关灯开K" + LoopId + ",开灯命令已经发送");
            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                0, "混合开关灯开K" + LoopId, OperatrType.UserOperator, "混合开关灯开K" + LoopId);
        }
    }



    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightHunHeOrderMenu1 : OpenLightHunHeOrderMenuBase
    {
        public OpenLightHunHeOrderMenu1()
        {
            int index = 1;
            Id = Services.MenuIdAssgin.OpenLightHunHeOrderMenuOpenBase + index - 1;
            // Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForSingleTreeGrpRightMenuId;
            Description = "地图混合终端开灯，执行回路K1开灯，ID为" + Id +
                          // Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForSingleTreeGrpRightMenuId +
                          "，类型为地图混合开关灯右键菜单。";
            Tag = "开灯_K" + index;
            this.Text = "开K" + index;
            this.Tooltips = "开启K" + index + "回路";
            LoopId = index;
        }
    }


    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightHunHeOrderMenu2 : OpenLightHunHeOrderMenuBase
    {
        public OpenLightHunHeOrderMenu2()
        {
            int index = 2;
            Id = Services.MenuIdAssgin.OpenLightHunHeOrderMenuOpenBase + index - 1;
            // Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForSingleTreeGrpRightMenuId;
            Description = "地图混合终端开灯，执行回路K2开灯，ID为" + Id +
                          // Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForSingleTreeGrpRightMenuId +
                          "，类型为地图混合开关灯右键菜单。";
            Tag = "开灯_K" + index;
            this.Text = "开K" + index;
            this.Tooltips = "开启K" + index + "回路";
            LoopId = index;
        }
    }


    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightHunHeOrderMenu3 : OpenLightHunHeOrderMenuBase
    {
        public OpenLightHunHeOrderMenu3()
        {
            int index = 3;
            Id = Services.MenuIdAssgin.OpenLightHunHeOrderMenuOpenBase + index - 1;
            // Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForSingleTreeGrpRightMenuId;
            Description = "地图混合终端开灯，执行回路K3开灯，ID为" + Id +
                          // Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForSingleTreeGrpRightMenuId +
                          "，类型为地图混合开关灯右键菜单。";
            Tag = "开灯_K" + index;
            this.Text = "开K" + index;
            this.Tooltips = "开启K" + index + "回路";
            LoopId = index;
        }
    }


    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightHunHeOrderMenu4 : OpenLightHunHeOrderMenuBase
    {
        public OpenLightHunHeOrderMenu4()
        {
            int index = 4;
            Id = Services.MenuIdAssgin.OpenLightHunHeOrderMenuOpenBase + index - 1;
            // Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForSingleTreeGrpRightMenuId;
            Description = "地图混合终端开灯，执行回路K4开灯，ID为" + Id +
                          // Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForSingleTreeGrpRightMenuId +
                          "，类型为地图混合开关灯右键菜单。";
            Tag = "开灯_K" + index;
            this.Text = "开K" + index;
            this.Tooltips = "开启K" + index + "回路";
            LoopId = index;
        }
    }


    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightHunHeOrderMenu5 : OpenLightHunHeOrderMenuBase
    {
        public OpenLightHunHeOrderMenu5()
        {
            int index = 5;
            Id = Services.MenuIdAssgin.OpenLightHunHeOrderMenuOpenBase + index - 1;
            // Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForSingleTreeGrpRightMenuId;
            Description = "地图混合终端开灯，执行回路K5开灯，ID为" + Id +
                          // Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForSingleTreeGrpRightMenuId +
                          "，类型为地图混合开关灯右键菜单。";
            Tag = "开灯_K" + index;
            this.Text = "开K" + index;
            this.Tooltips = "开启K" + index + "回路";
            LoopId = index;
        }
    }


    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightHunHeOrderMenu6 : OpenLightHunHeOrderMenuBase
    {
        public OpenLightHunHeOrderMenu6()
        {
            int index = 6;
            Id = Services.MenuIdAssgin.OpenLightHunHeOrderMenuOpenBase + index - 1;
            // Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForSingleTreeGrpRightMenuId;
            Description = "地图混合终端开灯，执行回路K6开灯，ID为" + Id +
                          // Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForSingleTreeGrpRightMenuId +
                          "，类型为地图混合开关灯右键菜单。";
            Tag = "开灯_K" + index;
            this.Text = "开K" + index;
            this.Tooltips = "开启K" + index + "回路";
            LoopId = index;
        }
    }

    [Export(typeof(IIMenuItem))]    //lvf
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightHunHeOrderMenu7 : OpenLightHunHeOrderMenuBase
    {
        public OpenLightHunHeOrderMenu7()
        {
            int index = 7;
            Id = Services.MenuIdAssgin.OpenLightHunHeOrderMenuOpenBase + index - 1;
            // Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForSingleTreeGrpRightMenuId;
            Description = "地图混合终端开灯，执行回路K7开灯，ID为" + Id +
                // Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForSingleTreeGrpRightMenuId +
                          "，类型为地图混合开关灯右键菜单。";
            Tag = "开灯_K" + index;
            this.Text = "开K" + index;
            this.Tooltips = "开启K" + index + "回路";
            LoopId = index;
        }
    }

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightHunHeOrderMenu8 : OpenLightHunHeOrderMenuBase
    {
        public OpenLightHunHeOrderMenu8()
        {
            int index = 8;
            Id = Services.MenuIdAssgin.OpenLightHunHeOrderMenuOpenBase + index - 1;
            // Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForSingleTreeGrpRightMenuId;
            Description = "地图混合终端开灯，执行回路K8开灯，ID为" + Id +
                // Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForSingleTreeGrpRightMenuId +
                          "，类型为地图混合开关灯右键菜单。";
            Tag = "开灯_K" + index;
            this.Text = "开K" + index;
            this.Tooltips = "开启K" + index + "回路";
            LoopId = index;
        }
    }

    #endregion


    #region 关灯

    public class OpenLightHunHeOrderCloseMenuBase : MenuItemBase
    {
        /// <summary>
        /// 最底层开灯服务，无继承不可用 需实现LoopId赋值
        /// </summary>
        public OpenLightHunHeOrderCloseMenuBase()
        {
            Id = -1;
            Description = "最底层关灯服务，无继承不可用，类型为地图混合开关灯。";
            Tag = "关灯最底层服务";
            this.Text = "Null";
            this.Classic = "右键菜单-终端地图混合操作-30500";
            this.Tooltips = "最底层关灯服务，无继承不可用";
            LoopId = -1;
            base.Command = new RelayCommand(Ex, CanEx, true);
           // IsPrivilegLeave = true;
        }
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            var equipment = this.Argu as Wlst.Sr .EquipmentInfoHolding .Model .WjParaBase ;
            if (equipment == null || equipment.RtuStateCode  == 0) return false;
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
            return Wlst.Cr.CoreMims.Services.UserInfo.CanX(areaId);
        }
        private bool CanEx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            var lst = this.Argu as List<int>;
            if (lst == null || lst.Count == 0)
            {
                return false;
            }
            return true;
        }

        public int LoopId;

        private void Ex()
        {
            var lst = this.Argu as List<int>;
            if (lst == null)
            {
                LogInfo.Log("无法执行混合开灯命令，无法定位参数，参数错误....");
                return;
            }





            var lstslt = new List<int>();
            string name = "";
            foreach (var t in lst)
            {
                if (
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems .
                        ContainsKey(t) &&
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems [t].
                        RtuStateCode  == 2
                    )
                {
                    var infos = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[t];
                    name = infos.RtuPhyId + " - " + infos.RtuName;
                    lstslt.Add(t);
                }
            }

            if (lst.Count == 0) return;
            if (lst.Count > 1) name += "等，" + lst.Count + "个终端";
            if (Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm==1)
            {
                if (
                    Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                        "您将要 对(" + name + ") 进行关灯操作，是否继续？", WlstMessageBoxType.YesNo) == WlstMessageBoxResults.No)
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



            var info = Wlst.Sr.ProtocolPhone .LxRtu .wst_cnt_order_rtu_open_close_light ;// .wlst_cnt_wj3090_order_open_close_light;// .ServerPart.wlst_OpenCloseLight_clinet_order_opencloseLight;
            info.Args .Addr .AddRange(lstslt);
            info.WstRtuCntOrderOpenCloseLight.Loops.Add(LoopId);
            info.WstRtuCntOrderOpenCloseLight.IsOpen = 2; //# 开关灯指令 0 关 1 开
          
            SndOrderServer.OrderSnd(info);




            LogInfo.Log("混合开关灯关K" + LoopId + ",关灯命令已经发送");
            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                0, "混合开关灯关K" + LoopId, OperatrType.UserOperator, "混合开关灯关K" + LoopId);
        }
    }



    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightHunHeOrderCloseMenu1 : OpenLightHunHeOrderCloseMenuBase
    {
        public OpenLightHunHeOrderCloseMenu1()
        {
            int index = 1;
            Id = Services.MenuIdAssgin.OpenLightHunHeOrderMenuCloseBase + index - 1;
            // Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForSingleTreeGrpRightMenuId;
            Description = "地图混合终端关灯，执行回路K1关灯，ID为" + Id +
                          // Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForSingleTreeGrpRightMenuId +
                          "，类型为地图混合开关灯右键菜单。";
            Tag = "关灯_K" + index;
            this.Text = "关K" + index;
            this.Tooltips = "关闭K" + index + "回路";
            LoopId = index;
        }
    }


    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightHunHeOrderCloseMenu2 : OpenLightHunHeOrderCloseMenuBase
    {
        public OpenLightHunHeOrderCloseMenu2()
        {
            int index = 2;
            Id = Services.MenuIdAssgin.OpenLightHunHeOrderMenuCloseBase + index - 1;
            // Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForSingleTreeGrpRightMenuId;
            Description = "地图混合终端关灯，执行回路K2关灯，ID为" + Id +
                          // Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForSingleTreeGrpRightMenuId +
                          "，类型为地图混合开关灯右键菜单。";
            Tag = "关灯_K" + index;
            this.Text = "关K" + index;
            this.Tooltips = "关闭K" + index + "回路";
            LoopId = index;
        }
    }


    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightHunHeOrderCloseMenu3 : OpenLightHunHeOrderCloseMenuBase
    {
        public OpenLightHunHeOrderCloseMenu3()
        {
            int index = 3;
            Id = Services.MenuIdAssgin.OpenLightHunHeOrderMenuCloseBase + index - 1;
            // Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForSingleTreeGrpRightMenuId;
            Description = "地图混合终端关灯，执行回路K3关灯，ID为" + Id +
                          // Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForSingleTreeGrpRightMenuId +
                          "，类型为地图混合开关灯右键菜单。";
            Tag = "关灯_K" + index;
            this.Text = "关K" + index;
            this.Tooltips = "关闭K" + index + "回路";
            LoopId = index;
        }
    }


    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightHunHeOrderCloseMenu4 : OpenLightHunHeOrderCloseMenuBase
    {
        public OpenLightHunHeOrderCloseMenu4()
        {
            int index = 4;
            Id = Services.MenuIdAssgin.OpenLightHunHeOrderMenuCloseBase + index - 1;
            // Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForSingleTreeGrpRightMenuId;
            Description = "地图混合终端关灯，执行回路K4关灯，ID为" + Id +
                          // Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForSingleTreeGrpRightMenuId +
                          "，类型为地图混合开关灯右键菜单。";
            Tag = "关灯_K" + index;
            this.Text = "关K" + index;
            this.Tooltips = "关闭K" + index + "回路";
            LoopId = index;
        }
    }


    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightHunHeOrderCloseMenu5 : OpenLightHunHeOrderCloseMenuBase
    {
        public OpenLightHunHeOrderCloseMenu5()
        {
            int index = 5;
            Id = Services.MenuIdAssgin.OpenLightHunHeOrderMenuCloseBase + index - 1;
            // Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForSingleTreeGrpRightMenuId;
            Description = "地图混合终端关灯，执行回路K5关灯，ID为" + Id +
                          // Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForSingleTreeGrpRightMenuId +
                          "，类型为地图混合开关灯右键菜单。";
            Tag = "关灯_K" + index;
            this.Text = "关K" + index;
            this.Tooltips = "关闭K" + index + "回路";
            LoopId = index;
        }
    }


    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightHunHeOrderCloseMenu6 : OpenLightHunHeOrderCloseMenuBase
    {
        public OpenLightHunHeOrderCloseMenu6()
        {
            int index = 6;
            Id = Services.MenuIdAssgin.OpenLightHunHeOrderMenuCloseBase + index - 1;
            // Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForSingleTreeGrpRightMenuId;
            Description = "地图混合终端关灯，执行回路K6关灯，ID为" + Id +
                          // Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForSingleTreeGrpRightMenuId +
                          "，类型为地图混合开关灯右键菜单。";
            Tag = "关灯_K" + index;
            this.Text = "关K" + index;
            this.Tooltips = "关闭K" + index + "回路";
            LoopId = index;
        }
    }

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightHunHeOrderCloseMenu7 : OpenLightHunHeOrderCloseMenuBase
    {
        public OpenLightHunHeOrderCloseMenu7()
        {
            int index = 7;
            Id = Services.MenuIdAssgin.OpenLightHunHeOrderMenuCloseBase + index - 1;
            // Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForSingleTreeGrpRightMenuId;
            Description = "地图混合终端关灯，执行回路K7关灯，ID为" + Id +
                // Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForSingleTreeGrpRightMenuId +
                          "，类型为地图混合开关灯右键菜单。";
            Tag = "关灯_K" + index;
            this.Text = "关K" + index;
            this.Tooltips = "关闭K" + index + "回路";
            LoopId = index;
        }
    }

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightHunHeOrderCloseMenu8 : OpenLightHunHeOrderCloseMenuBase
    {
        public OpenLightHunHeOrderCloseMenu8()
        {
            int index = 8;
            Id = Services.MenuIdAssgin.OpenLightHunHeOrderMenuCloseBase + index - 1;
            // Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForSingleTreeGrpRightMenuId;
            Description = "地图混合终端关灯，执行回路K8关灯，ID为" + Id +
                // Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForSingleTreeGrpRightMenuId +
                          "，类型为地图混合开关灯右键菜单。";
            Tag = "关灯_K" + index;
            this.Text = "关K" + index;
            this.Tooltips = "关闭K" + index + "回路";
            LoopId = index;
        }
    }

    #endregion



    #region measure

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MeasureHunheControllerForMenu : MenuItemBase
    {

        public MeasureHunheControllerForMenu()
        {
            Id = Services.MenuIdAssgin.OpenLightHunHeOrderMeasure;
            // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMenuId;
            Tag = "选测";
            Description = "选测终端，ID为" + Services.MenuIdAssgin.OpenLightHunHeOrderMeasure
                          // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMenuId 
                          + ",类型为地图混合终端右键菜单。";
            Text = "选测";
            this.Classic = "右键菜单-终端地图混合操作-30500";
            Tooltips = "地图混合终端选测";
            base.Command = new RelayCommand(Ex, CanEx, true);
        }


        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            var equipment = this.Argu as Wlst .Sr .EquipmentInfoHolding .Model .WjParaBase ;
            if (equipment == null || equipment.RtuStateCode  == 0) return false;
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
            return Wlst.Cr.CoreMims.Services.UserInfo.CanR(areaId);
        }


        private void Ex()
        {
            var lst = this.Argu as List<int>;
            if (lst == null)
            {
                LogInfo.Log("无法执行混合开灯命令，无法定位参数，参数错误....");
                return;
            }

            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_orders ;// .wlst_cnt_request_wj3090_measure;
            info.Args .Addr .AddRange(lst);
            info.WstRtuOrders.Op = 31;
            info.WstRtuOrders.RtuIds.AddRange(lst);
            SndOrderServer.OrderSnd(info);

            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                0, "混合终端选测", OperatrType.UserOperator, "混合终端选测");


        }

        protected bool CanEx()
        {
            var lst = this.Argu as List<int>;
            if (lst == null || lst.Count == 0)
            {
                return false;
            }
            return true;

        }

    }

    #endregion
}
