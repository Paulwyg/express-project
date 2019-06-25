using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.client;


namespace Wlst.Ux.Wj2090Module.Zorder
{

    #region 开灯

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RightOperators321 : RightOperatorHunheCtrlBase
    {
        public RightOperators321()
        {
            OperatorId = 1;


            Id = Wj2090Module.Services.MenuIdAssign.RightOperatorHunheCtrlBaseId + 0;
            Description = "开灯_所有控制器，ID为" + Id
                //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
                          + "，类型为混合单灯控制器右键菜单。";
            Tag = "开灯_所有节点";

            this.Text = "全部开灯";
            this.Tooltips = "开灯_所有节点";

        }
    };


    #endregion


    #region 一档节能

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RightOperators3221 : RightOperatorHunheCtrlBase
    {
        public RightOperators3221()
        {
            OperatorId = 2;


        
            Id = Wj2090Module.Services.MenuIdAssign.RightOperatorHunheCtrlBaseId + 1;
            Description = "调档节能_所有控制器，ID为" + Id
                //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
                          + "，类型为混合单灯控制器右键菜单。";
            Tag = "调档节能_所有节点";

            this.Text = "调档节能";
            this.Tooltips = "调档节能_所有节点";

        }
    };



    #endregion

    #region 二档节能

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RightOperators3231 : RightOperatorHunheCtrlBase
    {
        public RightOperators3231()
        {
            OperatorId = 3;


         
            Id = Wj2090Module.Services.MenuIdAssign.RightOperatorHunheCtrlBaseId + 2;
            Description = "调光节能_所有控制器，ID为" + Id
                //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
                          + "，类型为混合单灯控制器右键菜单。";
            Tag = "调光节能_所有节点";

            this.Text = "调光节能";
            this.Tooltips = "调光节能_所有节点";

        }
    };


    #endregion

    #region 关灯

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RightOperators3241 : RightOperatorHunheCtrlBase
    {
        public RightOperators3241()
        {
            OperatorId = 4;

         
            Id = Wj2090Module.Services.MenuIdAssign.RightOperatorHunheCtrlBaseId + 3;
            Description = "关灯_所有控制器，ID为" + Id
                //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
                          + "，类型为混合单灯控制器右键菜单。";
            Tag = "关灯_所有节点";

            this.Text = "全部关灯";
            this.Tooltips = "关灯_所有节点";

        }
    };



    #endregion



    #region 调光

    public class RightOperatorsTgct : RightOperatorHunheCtrlBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scale">0 - 10</param>
        public RightOperatorsTgct(int scale)
        {
            OperatorId = 5 + scale;

            string att = scale == 0 ? "  " : "" + scale;

            Id = Wj2090Module.Services.MenuIdAssign.RightOperatorHunheCtrlBaseId +OperatorId -1;
            Description = att+"0% 调光_控制器，ID为" + Id
                //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
                          + "，类型为混合单灯控制器右键菜单。";
            Tag = att + "0% 调光";

            //lvf  2018年4月13日08:47:00 整理格式对齐
            var txttmp = scale == 10 ? att + "0%调光" : att + "0% 调光";
            this.Text = txttmp;//att + "0% 调光";
            this.Tooltips = att + "0% 调光_所有灯头";

        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RightOperatorsc0 : RightOperatorsTgct
    {
        public RightOperatorsc0():base(0)
        {
            
        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RightOperatorsc1 : RightOperatorsTgct
    {
        public RightOperatorsc1()
            : base(1)
        {

        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RightOperatorsc2 : RightOperatorsTgct
    {
        public RightOperatorsc2()
            : base(2)
        {

        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RightOperatorsc3 : RightOperatorsTgct
    {
        public RightOperatorsc3()
            : base(3)
        {

        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RightOperatorsc4 : RightOperatorsTgct
    {
        public RightOperatorsc4()
            : base(4)
        {

        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RightOperatorsc5 : RightOperatorsTgct
    {
        public RightOperatorsc5()
            : base(5)
        {

        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RightOperatorsc6 : RightOperatorsTgct
    {
        public RightOperatorsc6()
            : base(6)
        {

        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RightOperatorsc7 : RightOperatorsTgct
    {
        public RightOperatorsc7()
            : base(7)
        {

        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RightOperatorsc8 : RightOperatorsTgct
    {
        public RightOperatorsc8()
            : base(8)
        {

        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RightOperatorsc9 : RightOperatorsTgct
    {
        public RightOperatorsc9()
            : base(9)
        {

        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RightOperatorsc10 : RightOperatorsTgct
    {
        public RightOperatorsc10()
            : base(10)
        {

        }
    };


    #endregion





    public class RightOperatorHunheCtrlBase : MenuItemBase
    {
        /// <summary>
        /// 最底层开灯服务，无继承不可用 需实现LoopId赋值
        /// </summary>
        public RightOperatorHunheCtrlBase()
        {
            Id = -1;
            Description = "最底层混合控制服务，无继承不可用，类型为混合单灯控制器。";
            Tag = "混合控制最底层服务，不可能";
            this.Text = "Null";
            this.Tooltips = "最底层混合控制服务，无继承不可用";
            OperatorId = -1;
            this.Classic = "右键菜单-混合单灯控制器-20902";
            base.Command = new RelayCommand(Ex, CanEx, true);
           // IsPrivilegLeave = true;
        }
        public override bool IsCanBeShowRwx()
        {
            bool canX = false;
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanX();
            var equipment = this.Argu as Dictionary<int, List<int>>;//Tuple<int, List<int>>;
            if (equipment == null) return false;
            foreach (var g in equipment)
            {
                var equipmentPara = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g.Key);
                var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(g.Key );
                canX = Wlst.Cr.CoreMims.Services.UserInfo.CanX(areaId);
                if (canX == false) break;
            }
            return canX;
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanX(areaId);
        }
        private bool CanEx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            var ars = this.Argu as Dictionary<int, List<int>>;
            if (ars == null || ars.Count == 0) return false;
            return true;
        }

        /// <summary>
        ///  1-开灯，2-1档节能，3-2档节能，4-关灯 ， 5-15 pwm中 0%-100%
        /// </summary>
        public int OperatorId;

        ///// <summary>
        ///// 10 全部，21 单数节点，20 双数节点
        ///// </summary>
        //public int AddrType;


        private static long _guid = 0;

        private void Ex()
        {
            var ars = this.Argu as Dictionary<int, List<int>>;
            if (ars == null || ars.Count == 0) return;

            var info = Wlst.Sr.ProtocolPhone.LxSlu .wst_slu_right_operator ;//.wlst_cnt_wj2090_order_right_operator;
            foreach (var g in ars)
            {
                int sluId = g.Key;
                if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsSlu(sluId) == false) continue;

                var tmps = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( g.Key);
                if (tmps == null || tmps.RtuFid  > 0) continue;


                //if (AddrType == 0) return;


                //if (info.Guid <= _guid) info.Guid = _guid + 1;
                //_guid = info.Guid;

                var data = new client.SluRightOperators.SluRightOperator();

                data.SluId = sluId;
                data.OperationOrder = 0;
                data.AddrType = 4;
                data.Addr = 0;
                data.Addrs = g.Value;
                if (OperatorId < 5)
                {
                    data.CmdType = 4;
                    data.CmdMix = new List<int>() { OperatorId, OperatorId, OperatorId, OperatorId };
                    data.CmdPwmField = new client.SluRightOperators.SluRightOperator.CmdPwm()
                    {
                        LoopCanDo = new List<int>() { },
                        Scale = 0
                    };
                }
                else
                {
                    data.CmdType = 5;
                    data.CmdMix = new List<int>() { };
                    data.CmdPwmField = new client.SluRightOperators.SluRightOperator.CmdPwm()
                    {
                        LoopCanDo = new List<int>() { 1, 2, 3, 4 },
                        Scale = (OperatorId -5)*10   //调光*10   lvf 2018年6月27日16:39:38
                    };
                }
                info.WstSluRightOperator .OperatorItems.Add(data);


            }
            SndOrderServer.OrderSnd(info);
        }

    }
}
