using System;
using System.Reflection;
using System.ComponentModel.Composition;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreMims.ShowMsgInfo;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Model;


namespace Wlst.Ux.WJ3005Module.ZOrders.Measure
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    class MeasureControllerForMainMenu : MenuItemBase 
    {
        private int _rtuId;
        //private  EventSubScriptionTokener _eventSubScriptionTokener;
        public MeasureControllerForMainMenu()
        {
            _rtuId = 1;
            Id = WJ3005Module.Services.MenuIdAssgin.MenuMeasureControllerForMainMenuId;// Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMainMenuId;
            Text = "选测";
            Tag = "选测";
            this.Classic = "主菜单";
            Description = "选测，ID 为" +Services .MenuIdAssgin .MenuMeasureControllerForMainMenuId +// Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMainMenuId +
                        "，类型为主界面全局菜单。选测当前选中终端。";
            Tooltips = "选测终端最新数据";
            base.Command = new RelayCommand(Ex, CanEx,true );
            base.IsCheckable = false;
            base.IsEnabled = true;

           EventPublish.AddEventTokener( 
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandler, FundOrderFilter);
        }
        public override bool IsCanBeShowRwx()
        {
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanR();
            return true;
        }


        #region EventSubScriptionTokener 

        public void FundEventHandler(PublishEventArgs args) // should do somework
        {
            _canEx = false;
            try
            {
                int x = 0;
                if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                {
                        x = Convert.ToInt32(args.GetParams()[0]);
                }
                if (x == 0) return;

                if (
                   Wlst . Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems  .
                        ContainsKey(x))
                {
                    if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[x].EquipmentType == WjParaBase.EquType.Rtu)
                    {

                        _rtuId = x;
                        _canEx = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        public bool FundOrderFilter(PublishEventArgs args) //接收终端选中变更事件
        {
            if (args.EventType == PublishEventType.Core)
            {
                if (args.EventId== Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                {
                        //return true;
                }
            }
            return false;
        }

        #endregion


        #region RelayCommand

        protected void Ex()
        {
            //var lst = new List<int>();
            //lst.Add(_rtuId);
            //int gid = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;
            //SndOrderServer.OrderSnd(Wlst.Sr.EquipmentNewData.Services.EventIdAssign.NewDataArrive, lst, null, gid);

            var info = Wlst.Sr.ProtocolPhone.LxRtu .wst_rtu_orders ;//.wlst_cnt_request_wj3090_measure;
            info.Args .Addr .Add(_rtuId);
            info.WstRtuOrders.RtuIds.Add(_rtuId);
            info.WstRtuOrders.Op = 31;
            SndOrderServer.OrderSnd(info);

            string name = "Unknown";
            if (
                Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems .
                    ContainsKey(_rtuId))
            {
                name = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.
                        InfoItems [_rtuId].RtuName;
            }
            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                _rtuId, name, OperatrType.UserOperator, "选测终端");
        }

        private bool _canEx;

        protected bool CanEx()
        {
             

            if(!Wlst .Sr.EquipmentInfoHolding .Services .EquipmentDataInfoHold  .InfoItems  .ContainsKey( _rtuId ) ||
            Wlst .Sr.EquipmentInfoHolding .Services .EquipmentDataInfoHold .InfoItems [_rtuId ].RtuStateCode  ==0)
            return false;

            return   _canEx ;
        }

        #endregion


    }
}
