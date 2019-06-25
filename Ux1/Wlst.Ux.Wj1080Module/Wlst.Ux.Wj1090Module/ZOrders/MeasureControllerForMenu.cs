using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreMims.ShowMsgInfo;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.WjEquipmentBaseModels.Interface;

namespace Wlst.Ux.Wj1090Module.ZOrders
{

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MeasureControllerForMenu : MenuItemBase
    {

        public MeasureControllerForMenu()
        {
            Id = Wj1090Module.Services.MenuIdAssgin.MeasureControllerForMenuId;// Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMenuId;
            Tag = "选 测";
            Description = "选测防盗集中控制器，ID为" + Services.MenuIdAssgin.MeasureControllerForMenuId// Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMenuId 
            + ",类型为终端右键菜单。";
            Text = "选 测";
            this.Classic = "右键菜单-集中控制器";
            Tooltips = "选测集中控制器数据数据";
            base.Command = new RelayCommand(Ex, CanEx,true );
        }

        public override void InitDataWhenBeforeUse(object argu)
        {
            base.InitDataWhenBeforeUse(argu);
        }

        private void Ex()
        {
            var terminalInfo = this.Argu as IIEquipmentInfo;
            if (terminalInfo == null) return;
            var rtuId = terminalInfo.RtuId;
            //var lst = new List<int>();
            //lst.Add(rtuId);
            //int gid = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;
            //SndOrderServer.OrderSnd(Wlst.Sr.EquipmentNewData.Services.EventIdAssign.NewDataArrive, lst, null, gid);

            if (rtuId == 0) return;
            var info = Wlst.Sr.ProtocolCnt.ServerPart.wlst_Wj1090_clinet_order_Measure;
            info.AddrLst.Add(rtuId);
            info.Data.ControlId = 0;
            info.Data.RtuId = rtuId;
            SndOrderServer.OrderSnd(info);

            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
              rtuId, terminalInfo.RtuName, OperatrType.UserOperator, "选测集中控制器" );

        }

        protected bool CanEx()
        {
            var terminalInfo = this.Argu as IIEquipmentInfo;
            if (terminalInfo == null) return false;
            if (terminalInfo.RtuState != 2) return false;
            //var rtuId = terminalInfo.RtuId;
            //if (EquipmentRunningInfoHolding.TmlRunningInfoDictionary.ContainsKey(rtuId))
            //{
            //    var t = EquipmentRunningInfoHolding.TmlRunningInfoDictionary[rtuId];
            return true;


        }

    }
}
