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
    #region 混合控制 1-4

    #region 开灯

    [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators11 : RightOperatorHunheSluBase
   {
       public RightOperators11()
       {
           OperatorId = 1;
           AddrType = 10;

           int addinfo = OperatorId * 3 - 3;
           if (AddrType == 10) addinfo += 1;
           if (AddrType == 21) addinfo += 2;
           if (AddrType == 20) addinfo += 3;

           Id = Wj2090Module.Services.MenuIdAssign.RightOperatorHunheSluBaseId + addinfo;
           Description = "开灯_所有控制器，ID为" + Id
               //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
                         + "，类型为混合单灯集中器右键菜单。";
           Tag = "开灯_所有节点";

           this.Text = "开灯";
           this.Tooltips = "开灯_所有节点";

       }
   };

   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators12 : RightOperatorHunheSluBase
   {
       public RightOperators12()
       {
           OperatorId = 1;
           AddrType = 21;

           int addinfo = OperatorId * 3 - 3;
           if (AddrType == 10) addinfo += 1;
           if (AddrType == 21) addinfo += 2;
           if (AddrType == 20) addinfo += 3;

           Id = Wj2090Module.Services.MenuIdAssign.RightOperatorHunheSluBaseId + addinfo;
           Description = "开灯_所有单数控制器，ID为" + Id
               //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
                         + "，类型为混合单灯集中器右键菜单。";
           Tag = "开灯_所有单数控制器";

           this.Text = "开灯";
           this.Tooltips = "开灯_所有单数控制器";

       }
   };

   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators13 : RightOperatorHunheSluBase
   {
       public RightOperators13()
       {
           OperatorId = 1;
           AddrType = 20;

           int addinfo = OperatorId * 3 - 3;
           if (AddrType == 10) addinfo += 1;
           if (AddrType == 21) addinfo += 2;
           if (AddrType == 20) addinfo += 3;

           Id = Wj2090Module.Services.MenuIdAssign.RightOperatorHunheSluBaseId + addinfo;
           Description = "开灯_所有双数控制器，ID为" + Id
               //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
                         + "，类型为混合单灯集中器右键菜单。";
           Tag = "开灯_所有双数控制器";

           this.Text = "开灯";
           this.Tooltips = "开灯_所有双数控制器";

       }
   };

   #endregion


   #region 一档节能

   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators121 : RightOperatorHunheSluBase
   {
       public RightOperators121()
       {
           OperatorId = 2;
           AddrType = 10;

           int addinfo = OperatorId * 3 - 3;
           if (AddrType == 10) addinfo += 1;
           if (AddrType == 21) addinfo += 2;
           if (AddrType == 20) addinfo += 3;

           Id = Wj2090Module.Services.MenuIdAssign.RightOperatorHunheSluBaseId + addinfo;
           Description = "调档节能_所有控制器，ID为" + Id
               //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
                         + "，类型为混合单灯集中器右键菜单。";
           Tag = "调档节能_所有节点";

           this.Text = "调档节能";
           this.Tooltips = "调档节能_所有节点";

       }
   };

   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators122 : RightOperatorHunheSluBase
   {
       public RightOperators122()
       {
           OperatorId = 2;
           AddrType = 21;

           int addinfo = OperatorId * 3 - 3;
           if (AddrType == 10) addinfo += 1;
           if (AddrType == 21) addinfo += 2;
           if (AddrType == 20) addinfo += 3;

           Id = Wj2090Module.Services.MenuIdAssign.RightOperatorHunheSluBaseId + addinfo;
           Description = "调档节能_所有单数控制器，ID为" + Id
               //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
                         + "，类型为混合单灯集中器右键菜单。";
           Tag = "调档节能_所有单数控制器";

           this.Text = "调档节能";
           this.Tooltips = "调档节能_所有单数控制器";

       }
   };

   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators123 : RightOperatorHunheSluBase
   {
       public RightOperators123()
       {
           OperatorId = 2;
           AddrType = 20;

           int addinfo = OperatorId * 3 - 3;
           if (AddrType == 10) addinfo += 1;
           if (AddrType == 21) addinfo += 2;
           if (AddrType == 20) addinfo += 3;

           Id = Wj2090Module.Services.MenuIdAssign.RightOperatorHunheSluBaseId + addinfo;
           Description = "调档节能_所有双数控制器，ID为" + Id
               //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
                         + "，类型为混合单灯集中器右键菜单。";
           Tag = "调档节能_所有双数控制器";

           this.Text = "调档节能";
           this.Tooltips = "调档节能_所有双数控制器";

       }
   };

   #endregion

   #region 二档节能

   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators131 : RightOperatorHunheSluBase
   {
       public RightOperators131()
       {
           OperatorId = 3;
           AddrType = 10;

           int addinfo = OperatorId * 3 - 3;
           if (AddrType == 10) addinfo += 1;
           if (AddrType == 21) addinfo += 2;
           if (AddrType == 20) addinfo += 3;

           Id = Wj2090Module.Services.MenuIdAssign.RightOperatorHunheSluBaseId + addinfo;
           Description = "调光节能_所有控制器，ID为" + Id
               //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
                         + "，类型为混合单灯集中器右键菜单。";
           Tag = "调光节能_所有节点";

           this.Text = "调光节能";
           this.Tooltips = "调光节能_所有节点";

       }
   };

   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators132 : RightOperatorHunheSluBase
   {
       public RightOperators132()
       {
           OperatorId = 3;
           AddrType = 21;

           int addinfo = OperatorId * 3 - 3;
           if (AddrType == 10) addinfo += 1;
           if (AddrType == 21) addinfo += 2;
           if (AddrType == 20) addinfo += 3;

           Id = Wj2090Module.Services.MenuIdAssign.RightOperatorHunheSluBaseId + addinfo;
           Description = "调光节能_所有单数控制器，ID为" + Id
               //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
                         + "，类型为混合单灯集中器右键菜单。";
           Tag = "调光节能_所有单数控制器";

           this.Text = "调光节能";
           this.Tooltips = "调光节能_所有单数控制器";

       }
   };

   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators133 : RightOperatorHunheSluBase
   {
       public RightOperators133()
       {
           OperatorId = 3;
           AddrType = 20;

           int addinfo = OperatorId * 3 - 3;
           if (AddrType == 10) addinfo += 1;
           if (AddrType == 21) addinfo += 2;
           if (AddrType == 20) addinfo += 3;

           Id = Wj2090Module.Services.MenuIdAssign.RightOperatorHunheSluBaseId + addinfo;
           Description = "调光节能_所有双数控制器，ID为" + Id
               //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
                         + "，类型为混合单灯集中器右键菜单。";
           Tag = "调光节能_所有双数控制器";

           this.Text = "调光节能";
           this.Tooltips = "调光节能_所有双数控制器";

       }
   };

   #endregion

   #region 关灯

   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators141 : RightOperatorHunheSluBase
   {
       public RightOperators141()
       {
           OperatorId = 4;
           AddrType = 10;

           int addinfo = OperatorId * 3 - 3;
           if (AddrType == 10) addinfo += 1;
           if (AddrType == 21) addinfo += 2;
           if (AddrType == 20) addinfo += 3;

           Id = Wj2090Module.Services.MenuIdAssign.RightOperatorHunheSluBaseId + addinfo;
           Description = "关灯_所有控制器，ID为" + Id
               //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
                         + "，类型为混合单灯集中器右键菜单。";
           Tag = "关灯_所有节点";

           this.Text = "关灯";
           this.Tooltips = "关灯_所有节点";

       }
   };

   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators142 : RightOperatorHunheSluBase
   {
       public RightOperators142()
       {
           OperatorId = 4;
           AddrType = 21;

           int addinfo = OperatorId * 3 - 3;
           if (AddrType == 10) addinfo += 1;
           if (AddrType == 21) addinfo += 2;
           if (AddrType == 20) addinfo += 3;

           Id = Wj2090Module.Services.MenuIdAssign.RightOperatorHunheSluBaseId + addinfo;
           Description = "关灯_所有单数控制器，ID为" + Id
               //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
                         + "，类型为混合单灯集中器右键菜单。";
           Tag = "关灯_所有单数控制器";

           this.Text = "关灯";
           this.Tooltips = "关灯_所有单数控制器";

       }
   };

   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators143 : RightOperatorHunheSluBase
   {
       public RightOperators143()
       {
           OperatorId = 4;
           AddrType = 20;

           int addinfo = OperatorId * 3 - 3;
           if (AddrType == 10) addinfo += 1;
           if (AddrType == 21) addinfo += 2;
           if (AddrType == 20) addinfo += 3;

           Id = Wj2090Module.Services.MenuIdAssign.RightOperatorHunheSluBaseId + addinfo;
           Description = "关灯_所有双数控制器，ID为" + Id
               //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
                         + "，类型为混合单灯集中器右键菜单。";
           Tag = "关灯_所有双数控制器";

           this.Text = "关灯";
           this.Tooltips = "关灯_所有双数控制器";

       }
   };

   #endregion
    #endregion

   #region 调光 5-15

   public class RightOperatorsTg : RightOperatorHunheSluBase
   {
       /// <summary>
       /// 
       /// </summary>
       /// <param name="scale"></param>
       /// <param name="addrtype">10全部  21 单数 20双数</param>
       public RightOperatorsTg(int scale,int addrtype)
       {
           OperatorId = 5 + scale;
           AddrType = addrtype;

           int addinfo = OperatorId * 3 - 3;
           if (AddrType == 10) addinfo += 1;
           if (AddrType == 21) addinfo += 2;
           if (AddrType == 20) addinfo += 3;

           string att = scale == 0 ? "" : "" + scale;
           string sttr = AddrType == 10 ? "全部" : AddrType == 21 ? "单数" : "双数";

           Id = Wj2090Module.Services.MenuIdAssign.RightOperatorHunheSluBaseId + addinfo;
           Description = att + "0% 调光_" + sttr + "控制器，ID为" + Id
               //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
                         + "，类型为混合单灯集中器右键菜单。";
           Tag = att + "0% 调光_" + sttr + "节点";

           this.Text = att + "0% 调光";
           this.Tooltips = att + "0% 调光_" + sttr + "节点";

       }
   };

   #region 0
   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators010 : RightOperatorsTg
   {
       public RightOperators010()
           : base(0, 10)
       {
         
       }
   };


   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators021 : RightOperatorsTg
   {
       public RightOperators021()
           : base(0, 21)
       {

       }
   };


   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators020 : RightOperatorsTg
   {
       public RightOperators020()
           : base(0, 20)
       {

       }
   };
   #endregion

   #region 1
   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators110 : RightOperatorsTg
   {
       public RightOperators110()
           : base(1, 10)
       {

       }
   };


   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators1121 : RightOperatorsTg
   {
       public RightOperators1121()
           : base(1, 21)
       {

       }
   };


   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators120 : RightOperatorsTg
   {
       public RightOperators120()
           : base(1, 20)
       {

       }
   };
   #endregion


   #region 0
   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators210 : RightOperatorsTg
   {
       public RightOperators210()
           : base(2, 10)
       {

       }
   };


   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators221 : RightOperatorsTg
   {
       public RightOperators221()
           : base(2, 21)
       {

       }
   };


   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators220 : RightOperatorsTg
   {
       public RightOperators220()
           : base(2, 20)
       {

       }
   };
   #endregion


   #region 3
   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators310 : RightOperatorsTg
   {
       public RightOperators310()
           : base(3, 10)
       {

       }
   };


   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators1321 : RightOperatorsTg
   {
       public RightOperators1321()
           : base(3, 21)
       {

       }
   };


   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators320 : RightOperatorsTg
   {
       public RightOperators320()
           : base(3, 20)
       {

       }
   };
   #endregion


   #region 4
   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators410 : RightOperatorsTg
   {
       public RightOperators410()
           : base(4, 10)
       {

       }
   };


   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators421 : RightOperatorsTg
   {
       public RightOperators421()
           : base(4, 21)
       {

       }
   };


   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators420 : RightOperatorsTg
   {
       public RightOperators420()
           : base(4, 20)
       {

       }
   };
   #endregion


   #region 5
   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators510 : RightOperatorsTg
   {
       public RightOperators510()
           : base(5, 10)
       {

       }
   };


   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators521 : RightOperatorsTg
   {
       public RightOperators521()
           : base(5, 21)
       {

       }
   };


   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators520 : RightOperatorsTg
   {
       public RightOperators520()
           : base(5, 20)
       {

       }
   };
   #endregion


   #region 6
   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators610 : RightOperatorsTg
   {
       public RightOperators610()
           : base(6, 10)
       {

       }
   };


   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators621 : RightOperatorsTg
   {
       public RightOperators621()
           : base(6, 21)
       {

       }
   };


   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators620 : RightOperatorsTg
   {
       public RightOperators620()
           : base(6, 20)
       {

       }
   };
   #endregion


   #region 7
   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators710 : RightOperatorsTg
   {
       public RightOperators710()
           : base(7, 10)
       {

       }
   };


   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators721 : RightOperatorsTg
   {
       public RightOperators721()
           : base(7, 21)
       {

       }
   };


   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators720 : RightOperatorsTg
   {
       public RightOperators720()
           : base(7, 20)
       {

       }
   };
   #endregion


   #region 8
   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators810 : RightOperatorsTg
   {
       public RightOperators810()
           : base(8, 10)
       {

       }
   };


   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators821 : RightOperatorsTg
   {
       public RightOperators821()
           : base(8, 21)
       {

       }
   };


   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators820 : RightOperatorsTg
   {
       public RightOperators820()
           : base(8, 20)
       {

       }
   };
   #endregion


   #region 9
   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators910 : RightOperatorsTg
   {
       public RightOperators910()
           : base(9, 10)
       {

       }
   };


   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators921 : RightOperatorsTg
   {
       public RightOperators921()
           : base(9, 21)
       {

       }
   };


   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators920 : RightOperatorsTg
   {
       public RightOperators920()
           : base(9, 20)
       {

       }
   };
   #endregion


   #region 10
   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators1010 : RightOperatorsTg
   {
       public RightOperators1010()
           : base(10, 10)
       {

       }
   };


   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators1021 : RightOperatorsTg
   {
       public RightOperators1021()
           : base(10, 21)
       {

       }
   };


   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class RightOperators1020 : RightOperatorsTg
   {
       public RightOperators1020()
           : base(10, 20)
       {

       }
   };
   #endregion


 

   #endregion


   public class RightOperatorHunheSluBase : MenuItemBase
    {
        /// <summary>
        /// 最底层开灯服务，无继承不可用 需实现LoopId赋值
        /// </summary>
        public RightOperatorHunheSluBase()
        {
            Id = -1;
            Description = "最底层混合控制服务，无继承不可用，类型为混合单灯集中器。";
            Tag = "混合控制服务，不可能";
            this.Text = "Null";
            this.Tooltips = "最底层混合控制服务，无继承不可用";
            OperatorId = -1;
            this.Classic = "右键菜单-混合单灯集中器-20901";
            base.Command = new RelayCommand(Ex, CanEx, true);
            //IsPrivilegLeave = true;
        }
        public override bool IsCanBeShowRwx()
        {
            bool canX = false;
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanX();
            var equipment = this.Argu as List<int>;//Wlst.Sr .EquipmentInfoHolding.Model.Wj2090Slu ;
            if (equipment == null ) return false;
            foreach (var g in equipment )
            {
                var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(g);
                canX = Wlst.Cr.CoreMims.Services.UserInfo.CanX(areaId);
                if (canX == false) break;
            }
            return canX;

        }
        private bool CanEx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            var ars = this.Argu as List<int>;
            if (ars == null || ars.Count == 0) return false;
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


        private static long _guid = 0;

        private void Ex()
        {
            var ars = this.Argu as List<int>;
            if (ars == null || ars.Count == 0) return;

            var info = Wlst.Sr.ProtocolPhone.LxSlu .wst_slu_right_operator ;//.wlst_cnt_wj2090_order_right_operator;//.ServerPart.wlst_Wj2090_clinet_right_operator_slu;
            if (info.Head.Gid <= _guid) info.Head.Gid = _guid + 1;
            _guid = info.Head.Gid;
            foreach (var g in ars)
            {
                int sluId = g;
                if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsSlu(sluId) == false) continue;


                if (AddrType == 0) return;

                var data = new client.SluRightOperators.SluRightOperator();

                data .SluId = sluId;
                data.OperationOrder = 0;
                data.AddrType = 2;
                data.Addr = AddrType;
                data.Addrs = new List<int>();
                if (OperatorId < 5)
                {
                    data.CmdType = 4;
                    data.CmdMix = new List<int>() { OperatorId, OperatorId, OperatorId, OperatorId };
                    data.CmdPwmField = new client.SluRightOperators.SluRightOperator.CmdPwm()
                                                {
                                                    LoopCanDo = new List<int>() {},
                                                    Scale = 0
                                                };
                }
                else
                {
                    data.CmdType = 5;
                    data.CmdMix = new List<int>() { };
                    data.CmdPwmField = new client.SluRightOperators.SluRightOperator.CmdPwm()
                                                {
                                                    LoopCanDo = new List<int>() {1, 2, 3, 4},
                                                    Scale = (OperatorId -5)*10  //lvf  调光*10 2018年6月27日16:40:43
                                                };
                }
                info.WstSluRightOperator .OperatorItems.Add(data);

                
            }
            SndOrderServer.OrderSnd(info);
        }
    }
}
