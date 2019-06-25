using System.Collections.Generic;
using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreMims.ShowMsgInfo;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.CoreOne.Services;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.client;


namespace Wlst.Ux.Wj9001Module.ZOrder
{
    public class OperateLineControllerForLeakRightMenuBase : MenuItemBase
    {
        public OperateLineControllerForLeakRightMenuBase()
        {
            Id = -1;
            Description = "最底层分合闸服务，无继承不可用，类型为漏电右键菜单。";
            Tag = "分合闸最底层服务，不可能";
            this.Text = "Null";
            this.Classic = "右键菜单-漏电设备通用";
            this.Tooltips = "最底层分合闸服务，无继承不可用";
            base.Command = new RelayCommand(Ex, CanEx, true);
            //IsPrivilegLeave = true;
        }

        public override bool IsCanBeShowRwx()
        {
            //if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            var equipment = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.Wj9001Leak;
            if (equipment == null || equipment.RtuStateCode == 0) return false;
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);

            if (LeakLineId[0] > 4 && LeakLineId[0] !=10)   //todo
            {
                if (equipment.WjLeakLines.ContainsKey(LeakLineId[0]) == false) return false;
                if (equipment.WjLeakLines[LeakLineId[0]].LeakMode == 1) return true;
                return false;
            }
            return Wlst.Cr.CoreMims.Services.UserInfo.CanX(areaId);
        }

        private bool CanEx()
        {
            //if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            var equipment = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.Wj9001Leak;
            if (equipment == null || equipment.RtuStateCode == 0) return false;
            //if (LeakLineId.Count == 0) return false;
            //if (LeakLineId[0] > 4)   //todo
            //{
            //    if (equipment.WjLeakLines.ContainsKey(LeakLineId[0]) == false ) return false;
            //    if (equipment.WjLeakLines[LeakLineId[0]].LeakMode == 1) return true;
            //    return false;
            //}
            return true;
        }

        public List<int> LeakLineId = new List<int>();

        public int OpenClose;

        private void Ex()
        {

            if (Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm == 1)
            {
                if (
                    Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                        "您将要进行分合闸操作，是否继续？", WlstMessageBoxType.YesNo) == WlstMessageBoxResults.No)
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
            var equipment = Argu as Sr.EquipmentInfoHolding.Model.Wj9001Leak;// IIEquipmentInfo;
            if (equipment == null)
                return;
            int rtuId = equipment.RtuId;
            if (rtuId < 1)
                return;

            var nt = Wlst.Sr.ProtocolPhone.LxLeak.wst_leak_order_zcOrSet;
            var order = new LeakOrders.LeakOrderItem();
            order.Op = 3; //手动分合闸
            order.RtuId = rtuId;
            order.OrderBreaktype = OpenClose;
            if (LeakLineId[0] == 10)
            {
                LeakLineId.Clear();
                for (int i = 1; i < 5; i++)
                {
                    LeakLineId.Add(i);
                }
                //if(equipment.WjLeakLines.Count ==4)
                //{
                //    for (int i = 1; i < 5; i++)
                //    {
                //        LeakLineId.Add(i);
                //    }
                //}
                //else
                //{
                //    if (equipment.WjLeakLines[5].LeakMode == 1)
                //    {
                //        for (int i = 1; i < 9; i++)
                //        {
                //            LeakLineId.Add(i);
                //        }
                //    }
                //    else
                //    {
                //        for (int i = 1; i < 5; i++)
                //        {
                //            LeakLineId.Add(i);
                //        }
                //    }
                //}

                if(equipment.WjLeakLines.Count >4 && equipment.WjLeakLines[5].LeakMode ==1 )
                {
                    for (int i = 5; i < 9; i++)
                    {
                        LeakLineId.Add(i);
                    }
                }

            }

            order.LeakLineId = LeakLineId;
            nt.WstLeakOrderZcOrSet.Item.Add(order);
            SndOrderServer.OrderSnd(nt);
        }
    }


    #region 控制器选测 、选测未知控制器

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MeasureControllerForMenu : MenuItemBase
    {

        public MeasureControllerForMenu()
        {
            Id = Services.MenuIdAssgin.MenuSelectTestForLeakRightMenuId; // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMenuId;
            Tag = "选 测";
            Description = "选测漏电设备，ID为" + Id // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMenuId 
                          + ",类型为漏电右键菜单。";
            Text = "选 测";
            this.Classic = "右键菜单-漏电设备";
            Tooltips = "选测漏电数据";
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
            var equipment = Argu as Sr.EquipmentInfoHolding.Model.Wj9001Leak;// IIEquipmentInfo;
            if (equipment == null)
                return;
            int rtuId = equipment.RtuId;
            if (rtuId < 1)
                return;

            var nt = Wlst.Sr.ProtocolPhone.LxLeak.wst_leak_order_zcOrSet;
            var order = new LeakOrders.LeakOrderItem();
            order.Op = 11; //选测
            order.RtuId = rtuId;
            nt.WstLeakOrderZcOrSet.Item.Add(order);
            SndOrderServer.OrderSnd(nt);

        }

        protected bool CanEx()
        {
            var terminalInfo = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (terminalInfo == null) return false;
            if (terminalInfo.RtuStateCode == 0) return false;
            //var rtuId = terminalInfo.RtuId;
            //if (EquipmentRunningInfoHolding.TmlRunningInfoDictionary.ContainsKey(rtuId))
            //{
            //    var t = EquipmentRunningInfoHolding.TmlRunningInfoDictionary[rtuId];
            return true;


        }

    }
#endregion
}