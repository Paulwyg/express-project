using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreMims.ShowMsgInfo;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.CoreOne.Services;
using Wlst.client;

namespace Wlst.Ux.Wj2090Module.Zorder
{

    public class CtrlRightOperatorBase : MenuItemBase
    {
        /// <summary>
        /// 最底层开灯服务，无继承不可用 需实现LoopId赋值
        /// </summary>
        public CtrlRightOperatorBase()
        {
            Id = -1;
            Description = "最底层单灯控制器右键菜单。";
            Tag = "最底层单灯控制器右键菜单";
            this.Text = "Null";
            this.Tooltips = "最底层单灯控制器右键菜单，无继承不可用";
            //SluId = -1;
            //CtrlId = -1;
            //Xoperator = -1;
            this.Classic = "右键菜单-单灯控制器";
            base.Command = new RelayCommand(Ex, CanEx, true);
            //IsPrivilegLeave = true;
        }
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanX();
            var equipment = this.Argu as Tuple<int, int>;
            if (equipment == null ) return false;
            var equipmentPara = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(equipment.Item1);
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.Item1);
            return Wlst.Cr.CoreMims.Services.UserInfo.CanX(areaId);
        }
        private bool CanEx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            var equipment = this.Argu as Tuple<int, int>;
            if (equipment == null) return false;
            //if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsSlu(equipment.Item1) == false)
            //    return false;
            return true;
        }

        //public int SluId;
        //public int CtrlId;
        //public int Xoperator;
        public OpType op;

        private void Ex()
        {
            var terminalInfo = this.Argu as Tuple<int, int>;
            if (terminalInfo == null)
            {
                LogInfo.Log("无法执行关灯命令，参数错误....");
                return;
            }

            if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsSlu(terminalInfo.Item1) == false) return;
            //   OrderServer.CloseLight(terminalInfo.RtuId, LoopId);
            NextEx(terminalInfo.Item1, terminalInfo.Item2);

        }

        public virtual void NextEx(int sluId, int ctrlId)
        {

        }

        /// <summary>
        /// 混合控制或PWM操作
        /// </summary>
        /// <param name="sluId"></param>
        /// <param name="ctrlId"></param>
        /// <param name="isMix">是否为混合控制</param>
        /// <param name="mixOpe">如果为混合控制则控制的操作 1-开灯，2-1档节能，3-2档节能，4-关灯 </param>
        /// <param name="scalOpe">如果为pwm操作则 0-10 -> 0%-100%</param>
        protected static void SndOrderMix(int sluId, int ctrlId, bool isMix, int mixOpe, int scalOpe)
        {
            var info = Wlst.Sr.ProtocolPhone .LxSlu .wst_slu_right_operator ;// .wlst_cnt_wj2090_order_right_operator ;//.ServerPart.wlst_Wj2090_clinet_right_operator_slu;
            var data = new client.SluRightOperators.SluRightOperator();
            data .SluId = sluId;
            data.OperationOrder = 0;
            data.AddrType = 3;
            data.Addr = ctrlId;
            data.Addrs = new List<int>();
            data.CmdType = isMix ? 4 : 5;
            data.CmdMix = new List<int>() { mixOpe, mixOpe, mixOpe, mixOpe };
            data.CmdPwmField = new client.SluRightOperators.SluRightOperator.CmdPwm()
                                        {
                                            LoopCanDo = new List<int>() {1, 2, 3, 4},
                                            Scale = scalOpe
                                        };
            info.WstSluRightOperator .OperatorItems.Add(data);
            SndOrderServer.OrderSnd(info,0,0,true);
        }

    }


    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatorZcParas : CtrlRightOperatorBase
    {
        public CtrlRightOperatorZcParas()
        {

            Id = Wlst.Ux.Wj2090Module.Services.MenuIdAssign.CtrlRightOperatorZcParasId;
            // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMainMenuId;
            Text = "召测参数";
            Tag = "召测参数";
            this.Classic = "单灯控制器-右键菜单-20900";
            Description = "选 测，ID 为" + Id + // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMainMenuId +
                          "，类型为单灯控制器。召测当前控制器参数。";
            Tooltips = "召测当前控制器参数";
        }


        public override void NextEx(int sluId, int ctrlId)
        {
            var info = Wlst.Sr.ProtocolPhone .LxSlu .wst_read_ctrl_paras_in_slu  ;// .wlst_cnt_wj2090_order_zc_conn_paras ;//.ServerPart.wlst_Wj2090_clinet_zc_conn_paras;
            info.Args .Addr .Add(1);
            info.WstSluReadCtrlParasInSlu.CtrlIdStart = ctrlId;
            info.WstSluReadCtrlParasInSlu.SluId = sluId;
            info.WstSluReadCtrlParasInSlu.CtrlCount = 1;
            SndOrderServer.OrderSnd(info);
        }
    }

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatoZcArgss : CtrlRightOperatorBase
    {
       public CtrlRightOperatoZcArgss()
        {

            Id = Wlst.Ux.Wj2090Module.Services.MenuIdAssign.CtrlRightOperatorZcParasId + 1;
            // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMainMenuId;
            Text = "召测方案";
            Tag = "召测时间设置方案";
            this.Classic = "单灯控制器-右键菜单-20900";
            Description = "召测时间设置方案，ID 为" + Id + // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMainMenuId +
                          "，类型为单灯控制器。召测时间设置方案。";
            Tooltips = "召测时间设置方案";
        }


        public override void NextEx(int sluId, int ctrlId)
        {
            var info = Wlst.Sr.ProtocolPhone .LxSlu  .wst_read_ctrl_args   ;//.ServerPart.wlst_Wj2090_clinet_xc_conn_args;
            info.Args .Addr .Add(1);
            info.WstSluReadCtrlArgs   .CtrlId = ctrlId;
            info.WstSluReadCtrlArgs.SluId = sluId;
            //info.Data.ReadArgs = true;
            //info.Data.ReadSunriseset = true;
            //info.Data.ReadTimer = true;
            info.WstSluReadCtrlArgs.ReadTimetable = true;
            info.WstSluReadCtrlArgs.ReadSunrise = false; //20171129 修改为false
            //info.Data.ReadVer = true;
            //info.Data.ReadGroup = true;

            SndOrderServer.OrderSnd(info);
            var tu = new Tuple<int,int>(sluId,ctrlId);

            Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegionWithArgu(Wj2090Module.Services.ViewIdAssign.ZcConnLocalArgsViewId, tu);
        }
    }



    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatoZcArgss1 : CtrlRightOperatorBase
    {
        public CtrlRightOperatoZcArgss1()
        {

            Id = Wlst.Ux.Wj2090Module.Services.MenuIdAssign.CtrlRightOperatorZcParasId + 2;
            // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMainMenuId;
            Text = "召测基本参数";
            Tag = "召测基本参数";
            this.Classic = "单灯控制器-右键菜单-20900";
            Description = "召测时间设置方案，ID 为" + Id + // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMainMenuId +
                          "，类型为单灯控制器。召测基本参数时间、版本、分组。";
            Tooltips = "召测基本参数";
        }


        public override void NextEx(int sluId, int ctrlId)
        {
            var info = Wlst.Sr.ProtocolPhone .LxSlu .wst_read_ctrl_args ;// .wlst_cnt_wj2090_order_xc_conn_args ;//.ServerPart.wlst_Wj2090_clinet_xc_conn_args;
            info.Args .Addr .Add(1);
            info.WstSluReadCtrlArgs.CtrlId = ctrlId;
            info.WstSluReadCtrlArgs.SluId = sluId;
            info.WstSluReadCtrlArgs.ReadTimer = true;
            info.WstSluReadCtrlArgs.ReadVer = true;
            info.WstSluReadCtrlArgs.ReadGroup = true;

            SndOrderServer.OrderSnd(info);


            //lvf 记录 召测终端
            if (Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus.ContainsKey(sluId) == false)
            {
                Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus.Add(sluId, DateTime.Now);
            }
            else
            {
                Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus[sluId] = DateTime.Now;
            }


            //var _dtnow = DateTime.Now.Ticks;
            //while (DateTime.Now.Ticks < _dtnow + 20000000)
            //{
            //    Wlst.Cr.CoreOne.OtherHelper.Delay.DelayEvent();
            //}
            //info.WstSluReadCtrlArgs.ReadArgs = true;
            //info.WstSluReadCtrlArgs.ReadSunrise = true;
            //info.WstSluReadCtrlArgs.ReadTimer = false;
            //info.WstSluReadCtrlArgs.ReadVer = false;
            //info.WstSluReadCtrlArgs.ReadGroup = false;
            //info.Head.Gid += 1;
            //SndOrderServer.OrderSnd(info);
        }
    }



    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatoZcArgss3 : CtrlRightOperatorBase
    {
        public CtrlRightOperatoZcArgss3()
        {

            Id = Wlst.Ux.Wj2090Module.Services.MenuIdAssign.CtrlRightOperatorZcParasId + 4;
            // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMainMenuId;
            Text = "即时选测";
            Tag = "即时选测";
            this.Classic = "单灯控制器-右键菜单-20900";
            Description = "召测时间设置方案，ID 为" + Id +
                          // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMainMenuId +
                          "，类型为单灯控制器。即时选测。";
            Tooltips = "即时选测";
        }


        public override void NextEx(int sluId, int ctrlId)
        {
            var info = Wlst.Sr.ProtocolPhone .LxSlu  .wst_read_ctrl_args  ;//.ServerPart.wlst_Wj2090_clinet_xc_conn_args;
            info.Args .Addr .Add(1);
            info.WstSluReadCtrlArgs.CtrlId = ctrlId;
            info.WstSluReadCtrlArgs.SluId = sluId;
           // info.WstCntOrderWj2090XcConnArgs.ReadArgs = true;
            //info.Data.ReadSunriseset = true;
            //info.Data.ReadTimer = true;
            //info.Data.ReadTimetable = true;
            //info.Data.ReadSunriseset = true;
            //info.Data.ReadVer = true;
            //info.Data.ReadGroup = true;
            //info .Data .ReadCtrldata 
            info.WstSluReadCtrlArgs.ReadCtrldata = true;
            info.WstSluReadCtrlArgs.ReadData = true;
            SndOrderServer.OrderSnd(info,0,0,true );

            //op = OpType.CtrlMeasure;
            //var args = new PublishEventArgs
            //{
            //    EventType = PublishEventType.Core,
            //    EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.UserOperateRtu,
            //};
            //args.AddParams(sluId);
            //args.AddParams(op);
            //args.AddParams(ctrlId);  //10 全部  21单数  20 双数
            //args.AddParams(EnumRtuModel.Wj2090);
            //args.AddParams(null);
            //EventPublish.PublishEvent(args); 

        }
    }

    #region 混合控制  5-8 

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatoMix1 : CtrlRightOperatorBase
    {
        public CtrlRightOperatoMix1()
        {

            Id = Wlst.Ux.Wj2090Module.Services.MenuIdAssign.CtrlRightOperatorZcParasId + 5;
            // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMainMenuId;
            Text = "全部开灯";
            Tag = "开灯";
            this.Classic = "单灯控制器-右键菜单-20900";
            Description = "开灯，ID 为" + Id + // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMainMenuId +
                          "，类型为单灯控制器。开灯。";
            Tooltips = "开灯";
        }


        public override void NextEx(int sluId, int ctrlId)
        {

            //op = OpType.SluOpen;
            //var args = new PublishEventArgs
            //{
            //    EventType = PublishEventType.Core,
            //    EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.UserOperateRtu,
            //};
            //args.AddParams(sluId);
            //args.AddParams(op);
            //args.AddParams(ctrlId);  //10 全部  21单数  20 双数
            //args.AddParams(EnumRtuModel.Wj2090);
            //args.AddParams(null);
            //EventPublish.PublishEvent(args);  //todo lvf


            SndOrderMix(sluId, ctrlId, true, 1, 0);//todo
        }
    }

    //[Export(typeof (IIMenuItem))]
    //[PartCreationPolicy(CreationPolicy.Shared)]
    //public class CtrlRightOperatoMix2 : CtrlRightOperatorBase
    //{
    //    //public CtrlRightOperatoMix2()
    //    //{

    //    //    Id = Wlst.Ux.Wj2090Module.Services.MenuIdAssign.CtrlRightOperatorZcParasId + 6;
    //    //    // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMainMenuId;
    //    //    Text = "调档节能";
    //    //    Tag = "调档节能";
    //    //    this.Classic = "单灯控制器-右键菜单-20900";
    //    //    Description = "调档节能，ID 为" + Id + // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMainMenuId +
    //    //                  "，类型为单灯控制器。调档节能。";
    //    //    Tooltips = "调档节能";
    //    //}


    //    //public override void NextEx(int sluId, int ctrlId)
    //    //{
    //    //    SndOrderMix(sluId, ctrlId, true, 2, 0);  
    //    //}
    //}

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatoMix3 : CtrlRightOperatorBase
    {
        public CtrlRightOperatoMix3()
        {

            Id = Wlst.Ux.Wj2090Module.Services.MenuIdAssign.CtrlRightOperatorZcParasId + 7;
            // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMainMenuId;
            Text = "调光节能";
            Tag = "调光节能";
            this.Classic = "单灯控制器-右键菜单-20900";
            Description = "调光节能，ID 为" + Id + // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMainMenuId +
                          "，类型为单灯控制器。调光节能。";
            Tooltips = "调光节能";
        }


        public override void NextEx(int sluId, int ctrlId)
        {
            SndOrderMix(sluId, ctrlId, true, 3, 0);
        }
    }

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatoMix4 : CtrlRightOperatorBase
    {
        public CtrlRightOperatoMix4()
        {

            Id = Wlst.Ux.Wj2090Module.Services.MenuIdAssign.CtrlRightOperatorZcParasId + 8;
            // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMainMenuId;
            Text = "全部关灯";
            Tag = "关灯";
            this.Classic = "单灯控制器-右键菜单-20900";
            Description = "关灯，ID 为" + Id + // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMainMenuId +
                          "，类型为单灯控制器。关灯。";
            Tooltips = "关灯";
        }


        public override void NextEx(int sluId, int ctrlId)
        {
            //op = OpType.SluClose;
            //var args = new PublishEventArgs
            //{
            //    EventType = PublishEventType.Core,
            //    EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.UserOperateRtu,
            //};
            //args.AddParams(sluId);
            //args.AddParams(op);
            //args.AddParams(ctrlId);  //10 全部  21单数  20 双数
            //args.AddParams(EnumRtuModel.Wj2090);
            //args.AddParams(null);
            //EventPublish.PublishEvent(args);  //todo lvf

            SndOrderMix(sluId, ctrlId, true, 4, 0);  //todo
        }
    }

#endregion
    
    #region 调光 9-19 使用

    public class CtrlRightOperatoPwm : CtrlRightOperatorBase
    {
        private int indexf;

        public CtrlRightOperatoPwm(int index)
        {
            indexf = index;
            Id = Wlst.Ux.Wj2090Module.Services.MenuIdAssign.CtrlRightOperatorZcParasId + 9 + index;
            // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMainMenuId;
            
            
            //lvf 2018年4月13日09:10:25 对齐对齐对齐对齐
            if( index !=100)
            {
                Text = index == 0 ? "  0%   调光" :" "+ index + "0%  调光";
            }else
            {
                Text = index + "% 调光";
            }
            
            Tag = index == 0 ? "  0% 调光" : index + "0% 调光";
            this.Classic = "单灯控制器-右键菜单-20900";
            Description = index == 0
                              ? "  0% 调光"
                              : index + "0% 调光，ID 为" + Id +
                                // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMainMenuId +
                                "，类型为单灯控制器调光。";
            Tooltips = index == 0 ? "  0% 调光" : index + "0% 调光";
        }


        public override void NextEx(int sluId, int ctrlId)
        {
            //op = OpType.SluAdjust;
            //var args = new PublishEventArgs
            //{
            //    EventType = PublishEventType.Core,
            //    EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.UserOperateRtu,
            //};
            //args.AddParams(sluId);
            //args.AddParams(op);
            //args.AddParams(ctrlId);  //10 全部  21单数  20 双数
            //args.AddParams(EnumRtuModel.Wj2090);
            //args.AddParams(indexf);
            //EventPublish.PublishEvent(args);  //todo lvf


            SndOrderMix(sluId, ctrlId, false, 0, indexf*10);    //todo
        }
    }


    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatoPwm0 : CtrlRightOperatoPwm
    {
        public CtrlRightOperatoPwm0()
            : base(0)
        {
        }
    }


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatoPwm1 : CtrlRightOperatoPwm
    {
        public CtrlRightOperatoPwm1()
            : base(1)
        {
        }
    }

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatoPwm2 : CtrlRightOperatoPwm
    {
        public CtrlRightOperatoPwm2()
            : base(2)
        {
        }
    }

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatoPwm3 : CtrlRightOperatoPwm
    {
        public CtrlRightOperatoPwm3()
            : base(3)
        {
        }
    }

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatoPwm4 : CtrlRightOperatoPwm
    {
        public CtrlRightOperatoPwm4()
            : base(4)
        {
        }
    }

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatoPwm5 : CtrlRightOperatoPwm
    {
        public CtrlRightOperatoPwm5()
            : base(5)
        {
        }
    }

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatoPwm6 : CtrlRightOperatoPwm
    {
        public CtrlRightOperatoPwm6()
            : base(6)
        {
        }
    }
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatoPwm7 : CtrlRightOperatoPwm
    {
        public CtrlRightOperatoPwm7()
            : base(7)
        {
        }
    }

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatoPwm8 : CtrlRightOperatoPwm
    {
        public CtrlRightOperatoPwm8()
            : base(8)
        {
        }
    }

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatoPwm9 : CtrlRightOperatoPwm
    {
        public CtrlRightOperatoPwm9()
            : base(9)
        {
        }
    }

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CtrlRightOperatoPwm10 : CtrlRightOperatoPwm
    {
        public CtrlRightOperatoPwm10()
            : base(10)
        {
        }
    }
    #endregion




}
