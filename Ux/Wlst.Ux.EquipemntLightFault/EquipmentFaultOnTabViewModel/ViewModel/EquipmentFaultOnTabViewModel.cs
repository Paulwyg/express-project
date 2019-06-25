using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;


using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipemntLightFault.Model;

using Wlst.Ux.EquipemntLightFault.EquipmentFaultOnTabViewModel.Services;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultOnTabViewModel.ViewModel
{
    [Export(typeof(IIEquipmentFaultOnTabViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class EquipmentFaultOnTabViewModel :
        EventHandlerHelperExtendNotifyProperyChanged,
        Services.IIEquipmentFaultOnTabViewModel
    {
        #region tab
        public int Index
        {
            get { return 1; }
        }
        public bool CanClose
        {
            get { return true; }
        }

        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        public bool CanFloat
        {
            get { return true; }
        }

        public bool CanUserPin
        {
            get { return true; }
        }

        public string Title
        {
            get { return "设备故障"; }
        }

        #endregion

        protected const int MaxRecordHold = 150;

        private ObservableCollection<OneTmlExistFaultViewModel> _record;

        public ObservableCollection<OneTmlExistFaultViewModel> Records
        {
            get
            {
                if (_record == null)
                    _record = new ObservableCollection<OneTmlExistFaultViewModel>();
                return _record;
            }
        }

        private OneTmlExistFaultViewModel _curr;

        public OneTmlExistFaultViewModel CurrentSelectTml
        {
            get { return _curr; }
            set
            {
                if (value == _curr) return;
                _curr = value;
                this.RaisePropertyChanged(() => this.CurrentSelectTml);
                if (value != null && value.EquipmentRtuId  > 0)
                {
                    var args = new PublishEventArgs
                                   {
                                       EventType = PublishEventType.Core,
                                       EventId =
                                           Sr.EquipmentInfoHolding.Services.EventIdAssign.
                                           EquipmentSelected,
                                   };
                    args.AddParams(value.EquipmentRtuId );

                    EventPublish.PublishEvent(args);
                }
            }
        }


        public EquipmentFaultOnTabViewModel()
        {
            //IsStopDynamicReflesh = true;
            this.InitEvent();
        }
    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class EquipmentFaultOnTabViewModel
    {
        protected int  SelectedRtuId=0 ;
        public void OnRequestServerData(OneTmlExistFaultViewModel info)
        {
            if (info == null) return;
            Sr.EquipemntLightFault.Services.PreErrorServices.RequestDataWhenErrorHappen(info.EquipmentRtuId, info.LoopId,
                                                                                        info.DateCreateId);
        }
        private void InitEvent()
        {
            //this.AddEventFilterInfo(Sr.EquipemntLightFault.Services.EventIdAssign.EquipmentExistFaultAddId,
            //                        PublishEventType.Core);
            this.AddEventFilterInfo(Sr.EquipemntLightFault.Services.EventIdAssign.EquipementExistFaultDeleteId,
                                    PublishEventType.Core);
            this.AddEventFilterInfo(Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                                    PublishEventType.Core);
            //this.AddEventFilterInfo(Sr.EquipmentGroupInfoHolding.Services.EventIdAssign.MainSingleTreeNodeActive,
            //                        PublishEventType.Core);

            this.AddEventFilterInfo(Sr.EquipemntLightFault.Services.EventIdAssign.EquipmentExistFaultAddId,
                                    PublishEventType.Core);

        }

        public override bool FundOrderFilterForExtendCheck(PublishEventArgs args)
        {

            switch (args.EventId)
            {
                case Sr.EquipemntLightFault.Services.EventIdAssign.EquipmentExistFaultAddId:
                    // if (IsStopDynamicReflesh) break;
                    var info = args.GetParams()[0] as List<int>;
                    if (info == null) return false;
                    foreach (var t in info)
                    {
                        var ntgs = Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(t);
                        if (ntgs == null) continue;
                        if (ntgs.RtuId == SelectedRtuId) return true;
                    }
                    return false;

                    break;
                case Sr.EquipemntLightFault.Services.EventIdAssign.EquipementExistFaultDeleteId:

                    var infos = args.GetParams()[0] as List<int>;
                    if (infos == null) return false;
                    var lst = Records.Where(t => infos.Contains(t.Id)).ToList();
                    return lst.Count > 0;

                    break;
            }
            return true;
        }


        public override void ExPublishedEvent(
            PublishEventArgs args)
        {
            //base.ExPublishedEvent(args);
            switch (args.EventId)
            {
                case Sr.EquipemntLightFault.Services.EventIdAssign.EquipmentExistFaultAddId:
                   // if (IsStopDynamicReflesh) break;
                    var info = args.GetParams()[0] as List<int >;
                    if (info == null) return;
                    foreach (var t in info )
                    {
                        var ntgs = Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(t);
                        if(ntgs !=null )
                        AddErrorInfo(ntgs ,true );
                    }

                    break;
                case Sr.EquipemntLightFault.Services.EventIdAssign.EquipementExistFaultDeleteId:
                   // if (IsStopDynamicReflesh) break;
                    var infos = args.GetParams()[0] as List<int >;
                    if (infos == null) return;
                    DeleteTmlFault(infos);
                    break;
                case Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected:
                    //  if (!IsStopDynamicReflesh) break;
                    var rtuId = Convert.ToInt32(args.GetParams()[0]);
                    if (rtuId > 1000000)
                    {
                        this.OnSelectTreeNodeChange(rtuId);
                    }
                    break;
            }
        }

        private void OnSelectTreeNodeChange(int rtuId)
        {

            SelectedRtuId = rtuId;
            this.Records.Clear();


            var ff =
                Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultLstInfoByRtuId(  rtuId);

            foreach (var t in ff)
            {
                
                this.AddErrorInfo(t,false );
            }


        }

        private void AddErrorInfo(FaultInfoBase error, bool dongtaiupdate)
        {
           // var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(id);
            if (error == null) return;
          
            if (dongtaiupdate)
            {
                if (error.RtuId != SelectedRtuId) return;
            }

            var infovm = new OneTmlExistFaultViewModel(error);
            Records.Insert(0, infovm);

            if (Records.Count > MaxRecordHold)
            {
                Records.RemoveAt(0);
            }
        }

        private void DeleteTmlFault(List<int> info)
        {
            var lst = Records.Where(t => info.Contains(t.Id)).ToList();

            foreach (var t in lst)
            {
                try
                {
                    if (Records.Contains(t))
                        Records.Remove(t);
                    //t.FaultRemak = "报警消警!!!";
                    //t.CreatTime = DateTime.Now.ToString();
                    //t.FaultName += " 消警";
                    //Records.Add( t);
                }
                catch (Exception ex)
                {
                }
            }

        }

    }
}
