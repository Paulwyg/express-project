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
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
 
using Wlst.client;


namespace Wlst.Ux.Wj2090Module.Zorder
{

    #region 控制器选测 、选测未知控制器

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MeasureControllerForMenu : MenuItemBase
    {
        public const int id = Wj2090Module.Services.MenuIdAssign.MeasureControllerForMenuId;
        public OpType op;
        public MeasureControllerForMenu()
        {
            Id = id; // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMenuId;
            Tag = "选 测";
            Description = "选测集中器，ID为" + id // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMenuId 
                          + ",类型为单灯右键菜单。";
            Text = "选 测";
            this.Classic = "右键菜单-单灯集中器";
            Tooltips = "选测单灯数据";
            base.Command = new RelayCommand(Ex, CanEx, true);
        }
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanR();
            var equipment = this.Argu as Wlst.Sr .EquipmentInfoHolding .Model .WjParaBase ;
            if (equipment == null || equipment.RtuStateCode  == 0) return false;
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
            return Wlst.Cr.CoreMims.Services.UserInfo.CanR(areaId);
        }

        private void Ex()
        {
            var terminalInfo = this.Argu as Wlst .Sr .EquipmentInfoHolding .Model .WjParaBase ;
            if (terminalInfo == null) return;
            var rtuId = terminalInfo.RtuId;
            //var lst = new List<int>();
            //lst.Add(rtuId);
            //int gid = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;
            //SndOrderServer.OrderSnd(Wlst.Sr.EquipmentNewData.Services.EventIdAssign.NewDataArrive, lst, null, gid);



            var info = Wlst.Sr.ProtocolPhone.LxSlu .wst_slu_ctrl_measure ;
            info.Args .Addr .Add(rtuId);
            info.WstSluMeasure.SluId = rtuId;
            info.WstSluMeasure.Type = 0;
            SndOrderServer.OrderSnd(info,0,0,true );

            //op = OpType.SluMeasure;
            //var args = new PublishEventArgs
            //{
            //    EventType = PublishEventType.Core,
            //    EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.UserOperateRtu,
            //};
            //args.AddParams(rtuId);
            //args.AddParams(op);
            //args.AddParams(-1);  //10 全部  21单数  20 双数
            //args.AddParams(EnumRtuModel.Wj2090);
            //args.AddParams(null);
            //EventPublish.PublishEvent(args);


            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                rtuId, terminalInfo.RtuName, OperatrType.UserOperator, "选测单灯");

            //if (WJ3005Module.ZNewData.TmlNewDataViewModel.ViewModel.NewDataViewModel.OnMeasureShowData)
            //    EventPublish.PublishEvent(new PublishEventArgs() {EventType = "MainWindow.Measure.show"});
            ////  "MainWindow.Measure.show"

        }

        protected bool CanEx()
        {
            var terminalInfo = this.Argu as Wlst .Sr .EquipmentInfoHolding .Model .WjParaBase ;
            if (terminalInfo == null) return false;
            if (terminalInfo.RtuStateCode  == 0) return false;
            //var rtuId = terminalInfo.RtuId;
            //if (EquipmentRunningInfoHolding.TmlRunningInfoDictionary.ContainsKey(rtuId))
            //{
            //    var t = EquipmentRunningInfoHolding.TmlRunningInfoDictionary[rtuId];
            return true;


        }

    }



    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MeasureControllerForMenuNotRgCt : MenuItemBase
    {
        public const int id = Wj2090Module.Services.MenuIdAssign.MeasureControllerForMenuIdNotRgCt;
        
        public OpType op;

        public MeasureControllerForMenuNotRgCt()
        {
            Id = id; // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMenuId;
            Tag = "选测未知控制器";
            Description = "选测未知控制器，ID为" + id // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMenuId 
                          + ",类型为单灯右键菜单。";
            Text = "选测未知控制器";
            this.Classic = "右键菜单-单灯集中器";
            Tooltips = "选测未知控制器";
            base.Command = new RelayCommand(Ex, CanEx, true);
        }
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanR();
            var equipment = this.Argu as Wlst.Sr .EquipmentInfoHolding .Model .WjParaBase ;
            if (equipment == null || equipment.RtuStateCode  == 0) return false;
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
            return Wlst.Cr.CoreMims.Services.UserInfo.CanR(areaId);
        }

        private void Ex()
        { 
            var terminalInfo = this.Argu as Wlst .Sr .EquipmentInfoHolding .Model .WjParaBase ;
            if (terminalInfo == null) return;
            var rtuId = terminalInfo.RtuId;
            //var lst = new List<int>();
            //lst.Add(rtuId);
            //int gid = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;
            //SndOrderServer.OrderSnd(Wlst.Sr.EquipmentNewData.Services.EventIdAssign.NewDataArrive, lst, null, gid);

            Wj2090Module.NewData.PartolView.Models.PartolViewVm.SetCurrentSluId(rtuId);
            Wj2090Module.NewData.PartolView.Models.PartolViewVm.ShowThisView(2);

            var info = Wlst.Sr.ProtocolPhone.LxSlu .wst_slu_ctrl_measure ;
            info.Args .Addr .Add(rtuId);

            info.WstSluMeasure.SluId = rtuId;
            info.WstSluMeasure.Type = 2;
            SndOrderServer.OrderSnd(info);

            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                rtuId, terminalInfo.RtuName, OperatrType.UserOperator, "选测单灯");

            //if (WJ3005Module.ZNewData.TmlNewDataViewModel.ViewModel.NewDataViewModel.OnMeasureShowData)
            //    EventPublish.PublishEvent(new PublishEventArgs() {EventType = "MainWindow.Measure.show"});
            ////  "MainWindow.Measure.show"

        }

        protected bool CanEx()
        {
            var terminalInfo = this.Argu as Wlst .Sr .EquipmentInfoHolding .Model .WjParaBase ;
            if (terminalInfo == null) return false;
            if (terminalInfo.RtuStateCode  == 0) return false;
            //var rtuId = terminalInfo.RtuId;
            //if (EquipmentRunningInfoHolding.TmlRunningInfoDictionary.ContainsKey(rtuId))
            //{
            //    var t = EquipmentRunningInfoHolding.TmlRunningInfoDictionary[rtuId];
            return true;


        }

    }

    #endregion

    #region 开灯

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RightOperators1 : RightOperatorsBase
    {
        public RightOperators1()
        {
            OperatorId = 1;
            AddrType = 10;

            int addinfo = OperatorId*3 - 3;
            if (AddrType == 10) addinfo += 1;
            if (AddrType == 21) addinfo += 2;
            if (AddrType == 20) addinfo += 3;

            Id = Wj2090Module.Services.MenuIdAssign.RightOperatorsBase40 + addinfo;
            Description = "开灯_所有控制器，ID为" + Id
                          //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
                          + "，类型为单灯控制器右键菜单。";
            Tag = "开灯_所有节点";

            this.Text = " 全部开灯";
            this.Tooltips = "开灯_所有节点";

        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RightOperators2 : RightOperatorsBase
    {
        public RightOperators2()
        {
            OperatorId = 1;
            AddrType = 21;

            int addinfo = OperatorId*3 - 3;
            if (AddrType == 10) addinfo += 1;
            if (AddrType == 21) addinfo += 2;
            if (AddrType == 20) addinfo += 3;

            Id = Wj2090Module.Services.MenuIdAssign.RightOperatorsBase40 + addinfo;
            Description = "开灯_所有单数控制器，ID为" + Id
                          //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
                          + "，类型为单灯控制器右键菜单。";
            Tag = "开灯_所有单数控制器";

            this.Text = " 全部开灯";
            this.Tooltips = "开灯_所有单数控制器";

        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RightOperators3 : RightOperatorsBase
    {
        public RightOperators3()
        {
            OperatorId = 1;
            AddrType = 20;

            int addinfo = OperatorId*3 - 3;
            if (AddrType == 10) addinfo += 1;
            if (AddrType == 21) addinfo += 2;
            if (AddrType == 20) addinfo += 3;

            Id = Wj2090Module.Services.MenuIdAssign.RightOperatorsBase40 + addinfo;
            Description = "开灯_所有双数控制器，ID为" + Id
                          //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
                          + "，类型为单灯控制器右键菜单。";
            Tag = "开灯_所有双数控制器";

            this.Text = " 全部开灯";
            this.Tooltips = "开灯_所有双数控制器";

        }
    };

    #endregion


    #region 一档节能

    //[Export(typeof (IIMenuItem))]
    //[PartCreationPolicy(CreationPolicy.Shared)]
    //public class RightOperators21 : RightOperatorsBase
    //{
    //    public RightOperators21()
    //    {
    //        //OperatorId = 2;
    //        //AddrType = 10;

    //        //int addinfo = OperatorId*3 - 3;
    //        //if (AddrType == 10) addinfo += 1;
    //        //if (AddrType == 21) addinfo += 2;
    //        //if (AddrType == 20) addinfo += 3;

    //        //Id = Wj2090Module.Services.MenuIdAssign.RightOperatorsBase40 + addinfo;
    //        //Description = "调档节能_所有控制器，ID为" + Id
    //        //              //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
    //        //              + "，类型为单灯控制器右键菜单。";
    //        //Tag = "调档节能_所有节点";

    //        //this.Text = "调档节能";
    //        //this.Tooltips = "调档节能_所有节点";

    //    }
    //};

    //[Export(typeof (IIMenuItem))]
    //[PartCreationPolicy(CreationPolicy.Shared)]
    //public class RightOperators22 : RightOperatorsBase
    //{
    //    public RightOperators22()
    //    {
    //        OperatorId = 2;
    //        AddrType = 21;

    //        int addinfo = OperatorId*3 - 3;
    //        if (AddrType == 10) addinfo += 1;
    //        if (AddrType == 21) addinfo += 2;
    //        if (AddrType == 20) addinfo += 3;

    //        Id = Wj2090Module.Services.MenuIdAssign.RightOperatorsBase40 + addinfo;
    //        Description = "调档节能_所有单数控制器，ID为" + Id
    //                      //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
    //                      + "，类型为单灯控制器右键菜单。";
    //        Tag = "调档节能_所有单数控制器";

    //        this.Text = "调档节能";
    //        this.Tooltips = "调档节能_所有单数控制器";

    //    }
    //};

    //[Export(typeof (IIMenuItem))]
    //[PartCreationPolicy(CreationPolicy.Shared)]
    //public class RightOperators23 : RightOperatorsBase
    //{
    //    public RightOperators23()
    //    {
    //        OperatorId = 2;
    //        AddrType = 20;

    //        int addinfo = OperatorId*3 - 3;
    //        if (AddrType == 10) addinfo += 1;
    //        if (AddrType == 21) addinfo += 2;
    //        if (AddrType == 20) addinfo += 3;

    //        Id = Wj2090Module.Services.MenuIdAssign.RightOperatorsBase40 + addinfo;
    //        Description = "调档节能_所有双数控制器，ID为" + Id
    //                      //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
    //                      + "，类型为单灯控制器右键菜单。";
    //        Tag = "调档节能_所有双数控制器";

    //        this.Text = "调档节能";
    //        this.Tooltips = "调档节能_所有双数控制器";

    //    }
    //};

    #endregion

    #region 二档节能

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RightOperators31 : RightOperatorsBase
    {
        public RightOperators31()
        {
            OperatorId = 3;
            AddrType = 10;

            int addinfo = OperatorId*3 - 3;
            if (AddrType == 10) addinfo += 1;
            if (AddrType == 21) addinfo += 2;
            if (AddrType == 20) addinfo += 3;

            Id = Wj2090Module.Services.MenuIdAssign.RightOperatorsBase40 + addinfo;
            Description = "调光节能_所有控制器，ID为" + Id
                          //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
                          + "，类型为单灯控制器右键菜单。";
            Tag = "调光节能_所有节点";

            this.Text = "调光节能";
            this.Tooltips = "调光节能_所有节点";

        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RightOperators32 : RightOperatorsBase
    {
        public RightOperators32()
        {
            OperatorId = 3;
            AddrType = 21;

            int addinfo = OperatorId*3 - 3;
            if (AddrType == 10) addinfo += 1;
            if (AddrType == 21) addinfo += 2;
            if (AddrType == 20) addinfo += 3;

            Id = Wj2090Module.Services.MenuIdAssign.RightOperatorsBase40 + addinfo;
            Description = "调光节能_所有单数控制器，ID为" + Id
                          //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
                          + "，类型为单灯控制器右键菜单。";
            Tag = "调光节能_所有单数控制器";

            this.Text = "调光节能";
            this.Tooltips = "调光节能_所有单数控制器";

        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RightOperators33 : RightOperatorsBase
    {
        public RightOperators33()
        {
            OperatorId = 3;
            AddrType = 20;

            int addinfo = OperatorId*3 - 3;
            if (AddrType == 10) addinfo += 1;
            if (AddrType == 21) addinfo += 2;
            if (AddrType == 20) addinfo += 3;

            Id = Wj2090Module.Services.MenuIdAssign.RightOperatorsBase40 + addinfo;
            Description = "调光节能_所有双数控制器，ID为" + Id
                          //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
                          + "，类型为单灯控制器右键菜单。";
            Tag = "调光节能_所有双数控制器";

            this.Text = "调光节能";
            this.Tooltips = "调光节能_所有双数控制器";

        }
    };

    #endregion

    #region 关灯

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RightOperators41 : RightOperatorsBase
    {
        public RightOperators41()
        {
            OperatorId = 4;
            AddrType = 10;

            int addinfo = OperatorId*3 - 3;
            if (AddrType == 10) addinfo += 1;
            if (AddrType == 21) addinfo += 2;
            if (AddrType == 20) addinfo += 3;

            Id = Wj2090Module.Services.MenuIdAssign.RightOperatorsBase40 + addinfo;
            Description = "关灯_所有控制器，ID为" + Id
                          //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
                          + "，类型为单灯控制器右键菜单。";
            Tag = "关灯_所有节点";

            this.Text = " 全部关灯";
            this.Tooltips = "关灯_所有节点";

        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RightOperators42 : RightOperatorsBase
    {
        public RightOperators42()
        {
            OperatorId = 4;
            AddrType = 21;

            int addinfo = OperatorId*3 - 3;
            if (AddrType == 10) addinfo += 1;
            if (AddrType == 21) addinfo += 2;
            if (AddrType == 20) addinfo += 3;

            Id = Wj2090Module.Services.MenuIdAssign.RightOperatorsBase40 + addinfo;
            Description = "关灯_所有单数控制器，ID为" + Id
                          //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
                          + "，类型为单灯控制器右键菜单。";
            Tag = "关灯_所有单数控制器";

            this.Text = " 全部关灯";
            this.Tooltips = "关灯_所有单数控制器";

        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RightOperators43 : RightOperatorsBase
    {
        public RightOperators43()
        {
            OperatorId = 4;
            AddrType = 20;

            int addinfo = OperatorId*3 - 3;
            if (AddrType == 10) addinfo += 1;
            if (AddrType == 21) addinfo += 2;
            if (AddrType == 20) addinfo += 3;

            Id = Wj2090Module.Services.MenuIdAssign.RightOperatorsBase40 + addinfo;
            Description = "关灯_所有双数控制器，ID为" + Id
                          //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
                          + "，类型为单灯控制器右键菜单。";
            Tag = "关灯_所有双数控制器";

            this.Text = " 全部关灯";
            this.Tooltips = "关灯_所有双数控制器";

        }
    };

    #endregion

    #region 调光


    public class RiOpesTg : RightOperatorsBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="addrtype">10全部  21 单数 20双数</param>
        public RiOpesTg(int scale,int addrtype)
        {
            OperatorId = 5 + scale;
            AddrType = addrtype;

            int addinfo = OperatorId * 3 - 3;
            if (AddrType == 10) addinfo += 1;
            if (AddrType == 21) addinfo += 2;
            if (AddrType == 20) addinfo += 3;

            string att = scale == 0 ? "" : "" + scale;
            string sttr = AddrType == 10 ? "全部" : AddrType == 21 ? "单数" : "双数";

            Id = Wj2090Module.Services.MenuIdAssign.RightOperatorsBase40 + addinfo;
            Description = att + "0%调光_" + sttr + "控制器，ID为" + Id
                //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
                          + "，类型为单灯控制器右键菜单。";
            Tag = att + "0%调光_" + sttr + "节点";

            //this.Text = att + "0%调光";
            //lvf 2018年4月13日09:10:25 对齐对齐对齐对齐
            if (scale != 10)
            {
                Text = scale == 0 ? "  0%   调光" :" "+ scale + "0%  调光";
            }
            else
            {
                Text = scale + "0% 调光";
            }
            this.Tooltips = att + "0%调光_" + sttr + "节点";
        }
    };



    #region 0
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RiOpes010 : RiOpesTg
    {
        public RiOpes010()
            : base(0, 10)
        {

        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RiOpes021 : RiOpesTg
    {
        public RiOpes021()
            : base(0, 21)
        {

        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RiOpes020 : RiOpesTg
    {
        public RiOpes020()
            : base(0, 20)
        {

        }
    };
    #endregion

    #region 1
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RiOpes110 : RiOpesTg
    {
        public RiOpes110()
            : base(1, 10)
        {

        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RiOpes1121 : RiOpesTg
    {
        public RiOpes1121()
            : base(1, 21)
        {

        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RiOpes120 : RiOpesTg
    {
        public RiOpes120()
            : base(1, 20)
        {

        }
    };
    #endregion


    #region 0
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RiOpes210 : RiOpesTg
    {
        public RiOpes210()
            : base(2, 10)
        {

        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RiOpes221 : RiOpesTg
    {
        public RiOpes221()
            : base(2, 21)
        {

        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RiOpes220 : RiOpesTg
    {
        public RiOpes220()
            : base(2, 20)
        {

        }
    };
    #endregion


    #region 3
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RiOpes310 : RiOpesTg
    {
        public RiOpes310()
            : base(3, 10)
        {

        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RiOpes1321 : RiOpesTg
    {
        public RiOpes1321()
            : base(3, 21)
        {

        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RiOpes320 : RiOpesTg
    {
        public RiOpes320()
            : base(3, 20)
        {

        }
    };
    #endregion


    #region 4
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RiOpes410 : RiOpesTg
    {
        public RiOpes410()
            : base(4, 10)
        {

        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RiOpes421 : RiOpesTg
    {
        public RiOpes421()
            : base(4, 21)
        {

        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RiOpes420 : RiOpesTg
    {
        public RiOpes420()
            : base(4, 20)
        {

        }
    };
    #endregion


    #region 5
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RiOpes510 : RiOpesTg
    {
        public RiOpes510()
            : base(5, 10)
        {

        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RiOpes521 : RiOpesTg
    {
        public RiOpes521()
            : base(5, 21)
        {

        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RiOpes520 : RiOpesTg
    {
        public RiOpes520()
            : base(5, 20)
        {

        }
    };
    #endregion


    #region 6
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RiOpes610 : RiOpesTg
    {
        public RiOpes610()
            : base(6, 10)
        {

        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RiOpes621 : RiOpesTg
    {
        public RiOpes621()
            : base(6, 21)
        {

        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RiOpes620 : RiOpesTg
    {
        public RiOpes620()
            : base(6, 20)
        {

        }
    };
    #endregion


    #region 7
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RiOpes710 : RiOpesTg
    {
        public RiOpes710()
            : base(7, 10)
        {

        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RiOpes721 : RiOpesTg
    {
        public RiOpes721()
            : base(7, 21)
        {

        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RiOpes720 : RiOpesTg
    {
        public RiOpes720()
            : base(7, 20)
        {

        }
    };
    #endregion


    #region 8
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RiOpes810 : RiOpesTg
    {
        public RiOpes810()
            : base(8, 10)
        {

        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RiOpes821 : RiOpesTg
    {
        public RiOpes821()
            : base(8, 21)
        {

        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RiOpes820 : RiOpesTg
    {
        public RiOpes820()
            : base(8, 20)
        {

        }
    };
    #endregion


    #region 9
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RiOpes910 : RiOpesTg
    {
        public RiOpes910()
            : base(9, 10)
        {

        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RiOpes921 : RiOpesTg
    {
        public RiOpes921()
            : base(9, 21)
        {

        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RiOpes920 : RiOpesTg
    {
        public RiOpes920()
            : base(9, 20)
        {

        }
    };
    #endregion


    #region 10
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RiOpes1010 : RiOpesTg
    {
        public RiOpes1010()
            : base(10, 10)
        {

        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RiOpes1021 : RiOpesTg
    {
        public RiOpes1021()
            : base(10, 21)
        {

        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RiOpes1020 : RiOpesTg
    {
        public RiOpes1020()
            : base(10, 20)
        {

        }
    };
    #endregion


  


    #endregion


    #region 召测时间方案

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ZcSluTimeInfo : MenuItemBase
    {
        public const int id = Wj2090Module.Services.MenuIdAssign.ZcSluTimeInfo;

        public ZcSluTimeInfo()
        {
            Id = id; // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMenuId;
            Tag = "召测时间方案";
            Description = "召测时间方案，ID为" + id // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMenuId 
                          + ",类型为单灯控制器右键菜单。";
            Text = "召测时间方案";
            this.Classic = "右键菜单-单灯设备通用";
            Tooltips = "召测时间方案";
            base.Command = new RelayCommand(Ex, CanEx, true);
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
        private void Ex()
        {
            var terminalInfo = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (terminalInfo == null) return;
            var rtuId = terminalInfo.RtuId;
            //var lst = new List<int>();
            //lst.Add(rtuId);
            //int gid = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;
            //SndOrderServer.OrderSnd(Wlst.Sr.EquipmentNewData.Services.EventIdAssign.NewDataArrive, lst, null, gid);

            var info = Wlst.Sr.ProtocolPhone .LxSlu  .wst_read_time_plan_in_slu  ;//.ServerPart.wlst_Wj2090_clinet_zc_slu_short_time;
            info.Args .Addr .Add(rtuId);
            
            info.WstSluReadTimePlanInSlu  .SluId = rtuId;
            //info.Data.CtrlIdCount = 50;
            //info.Data.CtrlIdStart = 1;
            SndOrderServer.OrderSnd(info);


            //lvf 记录 召测终端
            if (Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus.ContainsKey(rtuId) == false)
            {
                Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus.Add(rtuId, DateTime.Now);
            }
            else
            {
                Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus[rtuId] = DateTime.Now;
            }

            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                rtuId, terminalInfo.RtuName, OperatrType.UserOperator, "召测集中器时间方案");

            //if (WJ3005Module.ZNewData.TmlNewDataViewModel.ViewModel.NewDataViewModel.OnMeasureShowData)
            //    EventPublish.PublishEvent(new PublishEventArgs() {EventType = "MainWindow.Measure.show"});
            ////  "MainWindow.Measure.show"

        }

        protected bool CanEx()
        {
            var terminalInfo = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (terminalInfo == null) return false;
            if (terminalInfo.RtuStateCode  == 0) return false;
            //var rtuId = terminalInfo.RtuId;
            //if (EquipmentRunningInfoHolding.TmlRunningInfoDictionary.ContainsKey(rtuId))
            //{
            //    var t = EquipmentRunningInfoHolding.TmlRunningInfoDictionary[rtuId];
            return true;


        }

    }

    #endregion

    #region 召测控制器运行参数

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ZcAllConnsInfo : MenuItemBase
    {
        public const int id = Wj2090Module.Services.MenuIdAssign.ZcAllConnsInfoId;

        public ZcAllConnsInfo()
        {
            Id = id; // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMenuId;
            Tag = "召测所有控制器参数";
            Description = "召测所有控制器参数，ID为" + id // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMenuId 
                          + ",类型为单灯控制器右键菜单。";
            Text = "召测控制器参数";
            this.Classic = "右键菜单-单灯设备通用";
            Tooltips = "召测所有控制器参数";
            base.Command = new RelayCommand(Ex, CanEx, true);
        }
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanR();
            var equipment = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (equipment == null || equipment.RtuStateCode == 0) return false;
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
            return Wlst.Cr.CoreMims.Services.UserInfo.CanR(areaId);
        }

        private void Ex()
        {
            var ars = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (ars == null) return;
            int sluId = ars.RtuId;
            if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsSlu(sluId) == false) return;

            var nts = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( sluId);
            if (nts == null) return;
            var gps = nts as Wlst.Sr.EquipmentInfoHolding.Model.Wj2090Slu ;
            if (gps == null||gps .WjSluCtrls ==null ) return;
            int count = gps.WjSluCtrls .Count;

            var info = Wlst.Sr.ProtocolPhone .LxSlu .wst_read_ctrl_paras_in_slu ;// .wlst_cnt_wj2090_order_zc_conn_paras ;//.ServerPart.wlst_Wj2090_clinet_zc_conn_paras;
            info.Args .Addr .Add(1);
            info.WstSluReadCtrlParasInSlu.CtrlIdStart = 1;
            info.WstSluReadCtrlParasInSlu.SluId = sluId;
            info.WstSluReadCtrlParasInSlu.CtrlCount = count;
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



            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                sluId, nts.RtuName, OperatrType.UserOperator, "召测控制器参数");

            //if (WJ3005Module.ZNewData.TmlNewDataViewModel.ViewModel.NewDataViewModel.OnMeasureShowData)
            //    EventPublish.PublishEvent(new PublishEventArgs() {EventType = "MainWindow.Measure.show"});
            ////  "MainWindow.Measure.show"

        }

        protected bool CanEx()
        {
            var terminalInfo = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (terminalInfo == null) return false;
            if (terminalInfo.RtuStateCode  == 0) return false;
            return true;


        }

    }

    #endregion


    #region 选测所有控制器数据、辅助数据、物理数据

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MeasureControllerForCtr : MenuItemBase
    {
        public const int id = Wj2090Module.Services.MenuIdAssign.RightOperatorsBase40 + 46;

        public MeasureControllerForCtr()
        {
            Id = id; // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMenuId;
            Tag = "巡测控制器数据";
            Description = "巡测控制器数据，ID为" + id // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMenuId 
                          + ",类型为单灯右键菜单。";
            Text = "巡测控制器数据";
            this.Classic = "右键菜单-单灯集中器";
            Tooltips = "巡测控制器数据";
            base.Command = new RelayCommand(Ex, CanEx, true);
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

        private void Ex()
        {
            var ars = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (ars == null) return;
            int sluId = ars.RtuId;
            if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsSlu(sluId) == false) return;


            var gps = ars as Wlst.Sr.EquipmentInfoHolding.Model.Wj2090Slu;
            if (gps == null) return;
            int count = gps.WjSluCtrls .Count;

            //var lst = new List<int>();
            //lst.Add(rtuId);
            //int gid = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;
            //SndOrderServer.OrderSnd(Wlst.Sr.EquipmentNewData.Services.EventIdAssign.NewDataArrive, lst, null, gid);
            Wj2090Module.NewData.PartolView.Models.PartolViewVm.SetCurrentSluId(sluId);
            Wj2090Module.NewData.PartolView.Models.PartolViewVm.ShowThisView(5);


            var info = Wlst.Sr.ProtocolPhone.LxSlu .wst_slu_ctrl_measure ;//.wlst_cnt_request_wj2090_measure;
            info.Args .Addr .Add(sluId);
            info.WstSluMeasure  .CtrlCount = count;
            info.WstSluMeasure.SluId = sluId;
            info.WstSluMeasure.CtrlIdStart = 1;
            info.WstSluMeasure.Type = 5;
            SndOrderServer.OrderSnd(info);

            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                sluId, "控制器：" + gps .RtuName, OperatrType.UserOperator, "选测控制器数据");

            //if (WJ3005Module.ZNewData.TmlNewDataViewModel.ViewModel.NewDataViewModel.OnMeasureShowData)
            //    EventPublish.PublishEvent(new PublishEventArgs() {EventType = "MainWindow.Measure.show"});
            ////  "MainWindow.Measure.show"

        }

        protected bool CanEx()
        {
            var terminalInfo = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            ;
            if (terminalInfo == null) return false;
            return true;


        }

    }



    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MeasureControllerForCtrFz : MenuItemBase
    {
        public const int id = Wj2090Module.Services.MenuIdAssign.RightOperatorsBase40 + 47;

        public MeasureControllerForCtrFz()
        {
            Id = id; // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMenuId;
            Tag = "巡测控制器辅助数据";
            Description = "选测控制器辅助数据，ID为" + id // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMenuId 
                          + ",类型为单灯右键菜单。";
            Text = "巡测控制器辅助数据";
            this.Classic = "右键菜单-单灯集中器";
            Tooltips = "巡测控制器辅助数据";
            base.Command = new RelayCommand(Ex, CanEx, true);
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


        private void Ex()
        {
            //var ars = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            //if (ars == null) return;
            //
            //if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsSlu(sluId) == false) return;


            var gps = Argu  as Wlst.Sr.EquipmentInfoHolding.Model.Wj2090Slu ;
         
            if (gps == null) return;  
            int sluId = gps .RtuId;
            int count = gps.WjSluCtrls.Count;
            Wj2090Module.NewData.PartolView.Models.PartolViewVm.SetCurrentSluId(sluId);
            Wj2090Module.NewData.PartolView.Models.PartolViewVm.ShowThisView(6);


            var info = Wlst.Sr.ProtocolPhone.LxSlu .wst_slu_ctrl_measure ;
            info.Args .Addr .Add(sluId);
            info.WstSluMeasure.CtrlCount = count;
            info.WstSluMeasure.SluId = sluId;
            info.WstSluMeasure.CtrlIdStart = 1;
            info.WstSluMeasure.Type = 6;
            SndOrderServer.OrderSnd(info);

            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                sluId, "控制器:" + gps .RtuName, OperatrType.UserOperator, "选测单灯");

            //if (WJ3005Module.ZNewData.TmlNewDataViewModel.ViewModel.NewDataViewModel.OnMeasureShowData)
            //    EventPublish.PublishEvent(new PublishEventArgs() {EventType = "MainWindow.Measure.show"});
            ////  "MainWindow.Measure.show"

        }

        protected bool CanEx()
        {
            var terminalInfo = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            ;
            if (terminalInfo == null) return false;
            return true;


        }

    }


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MeasureControllerForCtrFzQuery : MenuItemBase
    {
        public const int id = Wj2090Module.Services.MenuIdAssign.RightOperatorsBase40 + 50;

        public MeasureControllerForCtrFzQuery()
        {
            Id = id; // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMenuId;
            Tag = "查询控制器辅助数据";
            Description = "查询控制器辅助数据，ID为" + id // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMenuId 
                          + ",类型为单灯右键菜单。";
            Text = "查询控制器辅助数据";
            this.Classic = "右键菜单-单灯集中器";
            Tooltips = "查询控制器辅助数据";
            base.Command = new RelayCommand(Ex, CanEx, true);
        }

        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanR();
            var equipment = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (equipment == null || equipment.RtuStateCode == 0) return false;
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
            return Wlst.Cr.CoreMims.Services.UserInfo.CanR(areaId);
        }


        private void Ex()
        {
            //var ars = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            //if (ars == null) return;
            //
            //if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsSlu(sluId) == false) return;


            var gps = Argu as Wlst.Sr.EquipmentInfoHolding.Model.Wj2090Slu;

            if (gps == null) return;
            int sluId = gps.RtuId;
            int count = gps.WjSluCtrls.Count;




            //Wj2090Module.NewData.DataSluAssisQuery.DataSluAssisQueryViewModel.SetCurrentSluId(sluId);
            //Wj2090Module.NewData.PartolView.Models.PartolViewVm.SetCurrentSluId(sluId);
            //Wj2090Module.NewData.PartolView.Models.PartolViewVm.ShowThisView(6);

            this.ExNavWithArgs(
                Wj2090Module.Services.ViewIdAssign.DataSluAssisQuery,
                sluId);

            //var info = Wlst.Sr.ProtocolPhone.LxSlu.wst_slu_max_min_data;
            //info.Args.Addr.Add(sluId);
            //var slulst = new List<int>();
            //slulst.Add(sluId);
            //info.WstSluMaxMinData.SluId = slulst;
            //info.WstSluMaxMinData.DtEnd = DateTime.Now.Ticks;
            //info.WstSluMaxMinData.DtStart = DateTime.Now.AddDays(-7).Ticks;

            //SndOrderServer.OrderSnd(info);

            //Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
            //    sluId, "控制器:" + gps.RtuName, OperatrType.UserOperator, "选测单灯");

            //if (WJ3005Module.ZNewData.TmlNewDataViewModel.ViewModel.NewDataViewModel.OnMeasureShowData)
            //    EventPublish.PublishEvent(new PublishEventArgs() {EventType = "MainWindow.Measure.show"});
            ////  "MainWindow.Measure.show"

        }

        protected bool CanEx()
        {
            var terminalInfo = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            ;
            if (terminalInfo == null) return false;
            return true;


        }

    }


    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MeasureControllerForCtrPhy : MenuItemBase
    {
        public const int id = Wj2090Module.Services.MenuIdAssign.RightOperatorsBase40 + 48;

        public MeasureControllerForCtrPhy()
        {
            Id = id; // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMenuId;
            Tag = "巡测控制器物理信息";
            Description = "选测控制器物理信息，ID为" + id // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMenuId 
                          + ",类型为单灯右键菜单。";
            Text = "巡测控制器物理信息";
            this.Classic = "右键菜单-单灯集中器";
            Tooltips = "巡测控制器物理信息";
            base.Command = new RelayCommand(Ex, CanEx, true);
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

        private void Ex()
        {
            //var ars = this.Argu as Wlst.Cr.WjEquipmentBaseModels.Interface.IIEquipmentInfo;
            //if (ars == null) return;
            //
            //if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsSlu(sluId) == false) return;

            //var nts = Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetEquipmentInfo(sluId);
            //if (nts == null) return;
            var gps = Argu as Wlst.Sr.EquipmentInfoHolding.Model.Wj2090Slu;
            if (gps == null) return;int sluId = gps .RtuId;
            int count = gps.WjSluCtrls .Count;

            Wj2090Module.NewData.PartolView.Models.PartolViewVm.SetCurrentSluId(sluId
);
            Wj2090Module.NewData.PartolView.Models.PartolViewVm.ShowThisView(4);

            var info = Wlst.Sr.ProtocolPhone.LxSlu .wst_slu_ctrl_measure ;
            info.Args .Addr .Add(sluId);
            info.WstSluMeasure.CtrlCount = count;
            info.WstSluMeasure.SluId = sluId;
            info.WstSluMeasure.CtrlIdStart = 1;
            info.WstSluMeasure.Type = 4;
            SndOrderServer.OrderSnd(info);

            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                sluId, "控制器:" + gps .RtuName, OperatrType.UserOperator, "选测单灯物理数据");

            //if (WJ3005Module.ZNewData.TmlNewDataViewModel.ViewModel.NewDataViewModel.OnMeasureShowData)
            //    EventPublish.PublishEvent(new PublishEventArgs() {EventType = "MainWindow.Measure.show"});
            ////  "MainWindow.Measure.show"

        }

        protected bool CanEx()
        {
            var terminalInfo = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            ;
            if (terminalInfo == null) return false;
            return true;


        }

    }


    #endregion

    public class RightOperatorsBase : MenuItemBase
    {
        /// <summary>
        /// 最底层开灯服务，无继承不可用 需实现LoopId赋值
        /// </summary>
        public RightOperatorsBase()
        {
            Id = -1;
            Description = "最底层关灯服务，无继承不可用，类型为单灯集中器右键菜单。";
            Tag = "关灯最底层服务，不可能";
            this.Text = "Null";
            this.Tooltips = "最底层关灯服务，无继承不可用";
            OperatorId = -1;
            this.Classic = "右键菜单-单灯集中器";
            base.Command = new RelayCommand(Ex, CanEx, true);
         //   IsPrivilegLeave = true;
        }
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanX();
            var equipment = this.Argu as Wlst.Sr .EquipmentInfoHolding .Model.WjParaBase ;
            if (equipment == null || equipment.RtuStateCode  == 0) return false;
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
            return Wlst.Cr.CoreMims.Services.UserInfo.CanX(areaId);
        }
        private bool CanEx()
        {
            return true;
        }

        /// <summary>
        ///  1-开灯，2-1档节能，3-2档节能，4-关灯 ， 5-15 pwm中 0%-100%
        /// </summary>
        public int OperatorId;

        /// <summary>
        /// 10 全部，21 单数节点，20 双数节点
        /// </summary>
        public int AddrType;

        public OpType op;

        private void Ex()
        {
            var ars = this.Argu as Wlst.Sr .EquipmentInfoHolding .Model .WjParaBase ;
            if (ars == null) return;
            int sluId = ars.RtuId;
            if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsSlu(sluId) == false) return;


            if (AddrType == 0) return;


            if (Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm==1)
            {
                var str = "开灯";
                if (OperatorId == 1) str = "开灯";
                if (OperatorId == 2) str = "节能";
                if (OperatorId == 3) str = "节能";
                if (OperatorId == 4) str = "关灯";
                if(OperatorId >4)
                {
                    if (OperatorId == 5) str = "0% 节能";
                    else str = (OperatorId - 5) + "0% 节能";
                }



                if (
                    Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                        "您将要进行" + str + "操作，是否继续？", WlstMessageBoxType.YesNo) == WlstMessageBoxResults.No)
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


            var info = Wlst.Sr.ProtocolPhone.LxSlu .wst_slu_right_operator ;//.ServerPart.wlst_Wj2090_clinet_right_operator_slu;
            var datax = new client.SluRightOperators.SluRightOperator();
            datax.SluId = sluId;
            datax.OperationOrder = 0;
            datax.AddrType = 2;
            datax.Addr = AddrType;
            datax.Addrs = new List<int>();
            if (OperatorId < 5)
            {
                datax.CmdType = 4;
                datax.CmdMix = new List<int>() { OperatorId, OperatorId, OperatorId, OperatorId };
                datax.CmdPwmField = new client.SluRightOperators.SluRightOperator.CmdPwm()
                                            {
                                                LoopCanDo = new List<int>() { },
                                                Scale = 0
                                            };
                //if (OperatorId == 1) op = OpType.SluOpen;
                //if (OperatorId == 4) op = OpType.SluClose;
                //var args = new PublishEventArgs
                //{
                //    EventType = PublishEventType.Core,
                //    EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.UserOperateRtu,
                //};
                //args.AddParams(sluId);
                //args.AddParams(op);
                //args.AddParams(AddrType*100);  //10 全部  21单数  20 双数     *100
                //args.AddParams(EnumRtuModel.Wj2090);
                //args.AddParams(null);
                //EventPublish.PublishEvent(args);  // todo lvf


            }
            else
            {
                datax.CmdType = 5;
                datax.CmdMix = new List<int>() { };
                datax.CmdPwmField = new client.SluRightOperators.SluRightOperator.CmdPwm()
                                            {
                                                LoopCanDo = new List<int>() { 1, 2, 3, 4 },
                                                Scale = (OperatorId -5)*10        //调光 发真实值   *10   lvf 2018年6月27日16:36:19
                                            };
                //op = OpType.SluAdjust;

                //var args = new PublishEventArgs
                //{
                //    EventType = PublishEventType.Core,
                //    EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.UserOperateRtu,
                //};
                //args.AddParams(sluId);
                //args.AddParams(op);
                //args.AddParams(AddrType*100);  //10 全部  21单数  20 双数
                //args.AddParams(EnumRtuModel.Wj2090);
                //args.AddParams(OperatorId -5);
                //EventPublish.PublishEvent(args);  //todo lvf

            }
            info.WstSluRightOperator.OperatorItems.Add(datax);
            SndOrderServer.OrderSnd(info,0,0,true );



        }
    }
}
