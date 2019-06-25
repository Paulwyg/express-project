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
using Wlst.Ux.Wj2090Module.Services;
using Wlst.client;


namespace Wlst.Ux.Wj2096Module.ZOrder
{

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

            Id = Ux.Wj2096Module.Services.MenuIdAssign.OpenCloseChangeLightMenuId + addinfo;
            Description = "开灯_所有控制器，ID为" + Id
                         + "，类型为Wj2096域右键菜单。";
            Tag = "开灯_所有节点";

            this.Text = "开灯";
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

            Id = Ux.Wj2096Module.Services.MenuIdAssign.OpenCloseChangeLightMenuId + addinfo;
            Description = "开灯_所有单数控制器，ID为" + Id
                         
                          + "，类型为Wj2096域右键菜单。";
            Tag = "开灯_所有单数控制器";

            this.Text = "开灯";
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

            Id = Ux.Wj2096Module.Services.MenuIdAssign.OpenCloseChangeLightMenuId + addinfo;
            Description = "开灯_所有双数控制器，ID为" + Id
                          //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
                          + "，类型为Wj2096域右键菜单。";
            Tag = "开灯_所有双数控制器";

            this.Text = "开灯";
            this.Tooltips = "开灯_所有双数控制器";

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

            Id = Ux.Wj2096Module.Services.MenuIdAssign.OpenCloseChangeLightMenuId + addinfo;
            Description = "关灯_所有控制器，ID为" + Id
                          //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
                          + "，类型为Wj2096域右键菜单。";
            Tag = "关灯_所有节点";

            this.Text = "关灯";
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

            Id = Ux.Wj2096Module.Services.MenuIdAssign.OpenCloseChangeLightMenuId + addinfo;
            Description = "关灯_所有单数控制器，ID为" + Id
                          //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
                          + "，类型为Wj2096域右键菜单。";
            Tag = "关灯_所有单数控制器";

            this.Text = "关灯";
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

            Id = Ux.Wj2096Module.Services.MenuIdAssign.OpenCloseChangeLightMenuId + addinfo;
            Description = "关灯_所有双数控制器，ID为" + Id
                          + "，类型为Wj2096域右键菜单。";
            Tag = "关灯_所有双数控制器";

            this.Text = "关灯";
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

            Id = Ux.Wj2096Module.Services.MenuIdAssign.OpenCloseChangeLightMenuId + addinfo;
            Description = att + "0%调光_" + sttr + "控制器，ID为" + Id
                          + "，类型为Wj2096域右键菜单。";
            Tag = att + "0%调光_" + sttr + "节点";

            this.Text = att + "0%调光";
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




    #region 巡测所有控制器数据

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MeasureControllerForCtr : MenuItemBase
    {
        public const int id = Ux.Wj2096Module.Services.MenuIdAssign.MeasureControllerForMenuId;

        public MeasureControllerForCtr()
        {
            Id = id;
            Tag = "巡测控制器数据";
            Description = "巡测控制器数据，ID为" + id 
                          + ",类型为Wj2096域右键菜单。";
            Text = "巡测控制器数据";
            this.Classic = "右键菜单-Wj2096-域";
            Tooltips = "巡测控制器数据";
            base.Command = new RelayCommand(Ex, CanEx, true);
        }

        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;




            var equipment = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField((int)this.Argu); 
            if (equipment == null) return false;

            var areaId = equipment.AreaId;
            return Wlst.Cr.CoreMims.Services.UserInfo.CanR(areaId);
        }

        private void Ex()
        {
            var ars = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField((int)this.Argu);
            if (ars == null)
            {
                Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                    "未分组控制器无法批量选测。", WlstMessageBoxType.Ok);
                return;
            }
            int sluId = ars.FieldId;

            int count = ars.CtrlLst.Count;

            Ux.Wj2096Module.NewData.CtrlDataGrid.ViewModel.PartolViewVm.SetCurrentSluId(sluId);
            Ux.Wj2096Module.NewData.CtrlDataGrid.ViewModel.PartolViewVm.ShowThisView(5);


            //var info = Wlst.Sr.ProtocolPhone.LxSlu .wst_slu_ctrl_measure ;//.wlst_cnt_request_wj2090_measure;
            //info.Args .Addr .Add(sluId);
            //info.WstSluMeasure  .CtrlCount = count;
            //info.WstSluMeasure.SluId = sluId;
            //info.WstSluMeasure.CtrlIdStart = 1;
            //info.WstSluMeasure.Type = 5;
            var info = Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slu_sgl_ctrl_measure;
            foreach (var t in ars.CtrlLst)
            {
                info.Args.Addr.Add(t.CtrlId);
            }
            //info.WstSluMeasure.Type = 5;
            info.Args.Cid = 0;
            SndOrderServer.OrderSnd(info);

            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                sluId, "域：" + ars .FieldName, OperatrType.UserOperator, "巡测控制器数据");


        }

        protected bool CanEx()
        {
            //var equipment = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField((int)this.Argu); 
            //if (equipment == null) return false;

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
            Description = "最底层关灯服务，无继承不可用，类型为Wj2096域右键菜单。";
            Tag = "关灯最底层服务，不可能";
            this.Text = "Null";
            this.Tooltips = "最底层关灯服务，无继承不可用";
            OperatorId = -1;
            this.Classic = "右键菜单-Wj2096-域";
            base.Command = new RelayCommand(Ex, CanEx, true);
         //   IsPrivilegLeave = true;
        }
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;

            var equipment = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField((int)this.Argu);
            if (equipment == null) return false;

            var areaId = equipment.AreaId;
            return Wlst.Cr.CoreMims.Services.UserInfo.CanX(areaId);
        }

        private bool CanEx()
        {
            return true;
        }

        /// <summary>
        ///  1-开灯 4-关灯 ， 5-15 pwm中 0%-100%
        /// </summary>
        public int OperatorId;

        /// <summary>
        /// 10 全部，21 单数节点，20 双数节点
        /// </summary>
        public int AddrType;

        public OpType op;

        private void Ex()
        {

            var info = Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slu_sgl_right_operator;
            var datax = new client.SluRightOperators.SluRightOperator();


            var ars1 = this.Argu as Tuple<int, int>;
            if (ars1 == null)
            {
                if ((int) this.Argu > 1700000 && (int) this.Argu < 1800000)
                {
                    var ars = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField((int) this.Argu);
                    if (ars == null) return;
                    int sluId = ars.FieldId;

                    if (AddrType == 0) return;

                    if (Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm == 1)
                    {
                        var str = "开灯";
                        if (OperatorId == 1) str = "开灯";
                        if (OperatorId == 4) str = "关灯";
                        if (OperatorId > 4)
                        {
                            if (OperatorId == 5) str = "0%节能";
                            else str = (OperatorId - 5) + "0%节能";
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

                    datax.SluId = sluId;
                    datax.AddrType = 4;
                    datax.Addrs = new List<int>();

                    var ctrllist = (from t in ars.CtrlLst orderby t.OrderId select t).ToList();
                    if (AddrType == 10)
                    {
                        foreach (var t in ctrllist)
                        {
                            datax.Addrs.Add(t.CtrlId);
                        }
                    }
                    else if (AddrType == 21)
                    {
                        foreach (var t in ctrllist)
                        {
                            if (t.OrderId%2 == 1)
                            {
                                datax.Addrs.Add(t.CtrlId);
                            }
                        }
                    }
                    else if (AddrType == 20)
                    {
                        foreach (var t in ctrllist)
                        {
                            if (t.OrderId%2 == 0)
                            {
                                datax.Addrs.Add(t.CtrlId);
                            }
                        }
                    }
                    ;


                    if (OperatorId < 5)
                    {
                        datax.CmdType = 4;
                        datax.CmdMix = new List<int>() {OperatorId, OperatorId, OperatorId, OperatorId};
                        datax.CmdPwmField = new client.SluRightOperators.SluRightOperator.CmdPwm()
                                                {
                                                    LoopCanDo = new List<int>() {},
                                                    Scale = 0
                                                };
                    }
                    else
                    {
                        datax.CmdType = 5;
                        datax.CmdMix = new List<int>() {};
                        datax.CmdPwmField = new client.SluRightOperators.SluRightOperator.CmdPwm()
                                                {
                                                    LoopCanDo = new List<int>() {1, 2, 3, 4},
                                                    Scale = (OperatorId - 5) * 10  //lvf 调光*10 2018年6月27日16:42:38
                                                };
                    }

                    info.WstSluRightOperator.OperatorItems.Add(datax);
                    SndOrderServer.OrderSnd(info, 0, 0, true);
                }
            }
            else 
            {
                var ars = Wlst.Sr.SlusglInfoHold.Services.SluSglFieldGrpHold.MySlef.Get(ars1.Item1,ars1.Item2);
                if (ars == null)
                {
                    Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                        "未分组控制器无法批量控制。", WlstMessageBoxType.Ok);
                    return;
                }
                int sluId = ars.FieldId;

                if (AddrType != 10) return;
                

                if (Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm == 1)
                {
                    var str = "开灯";
                    if (OperatorId == 1) str = "开灯";
                    if (OperatorId == 4) str = "关灯";
                    if (OperatorId > 4)
                    {
                        if (OperatorId == 5) str = "0%节能";
                        else str = (OperatorId - 5) + "0%节能";
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

                datax.SluId = sluId;
                datax.AddrType = 4;
                datax.Addrs = new List<int>();

                //var ctrllist = (from t in ars.CtrlLst orderby t select t).ToList();
                if (AddrType == 10)
                {
                    foreach (var t in ars.CtrlLst)
                    {
                        datax.Addrs.Add(t);
                    }
                }

                if (OperatorId < 5)
                {
                    datax.CmdType = 4;
                    datax.CmdMix = new List<int>() { OperatorId, OperatorId, OperatorId, OperatorId };
                    datax.CmdPwmField = new client.SluRightOperators.SluRightOperator.CmdPwm()
                    {
                        LoopCanDo = new List<int>() { },
                        Scale = 0
                    };

                }
                else
                {
                    datax.CmdType = 5;
                    datax.CmdMix = new List<int>() { };
                    datax.CmdPwmField = new client.SluRightOperators.SluRightOperator.CmdPwm()
                    {
                        LoopCanDo = new List<int>() { 1, 2, 3, 4 },
                        Scale = (OperatorId - 5)*10  // lvf 调光 *10 2018年6月27日16:37:01
                    };

                }

                info.WstSluRightOperator.OperatorItems.Add(datax);
                SndOrderServer.OrderSnd(info, 0, 0, true);

            }
        }
    }
}
