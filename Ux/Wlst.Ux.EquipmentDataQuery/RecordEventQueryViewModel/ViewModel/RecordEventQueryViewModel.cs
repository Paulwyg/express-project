using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Services;
using Wlst.Cr.PPProtocolSvrCnt.Common;
using Wlst.Cr.WjEquipmentBaseModels.Interface;
using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface;
using Wlst.Sr.ProtocolCnt.RecordEvent;
using Wlst.Ux.EquipmentDataQuery.RecordEventQueryViewModel.Services;

namespace Wlst.Ux.EquipmentDataQuery.RecordEventQueryViewModel.ViewModel
{
    [Export(typeof (IIRecordEventQueryViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class RecordEventQueryViewModel :
        Wlst.Cr.Core.CoreServices .ObservableObject , IIRecordEventQueryViewModel
    {
        public RecordEventQueryViewModel()
        {
            this.InitAction();
        }

        private ObservableCollection<OneRecordEventViewModel> _record;

        public ObservableCollection<OneRecordEventViewModel> Record
        {
            get
            {
                if (_record == null)
                    _record = new ObservableCollection<OneRecordEventViewModel>();
                return _record;
            }
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            Record.Clear();
            DtStartTime = DateTime.Now.AddDays(-1);
            DtEndTime = DateTime.Now;
           
        }


        #region iitab

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
            get { return "事件记录查询"; }
        }

        #endregion

        #region arrti

        private DateTime _dtStartTime;

        /// <summary>
        /// 
        /// </summary>
        public DateTime DtStartTime
        {
            get { return _dtStartTime; }
            set
            {
                if (value != _dtStartTime)
                {
                    if (value.Ticks >= DateTime.Now.Ticks) value = DateTime.Now;
                    _dtStartTime = value;
                    this.RaisePropertyChanged(() => this.DtStartTime);
                }
            }
        }

        private DateTime _dtEndTime;

        /// <summary>
        /// 
        /// </summary>
        public DateTime DtEndTime
        {
            get { return _dtEndTime; }
            set
            {
                if (value != _dtEndTime)
                {
                    if (value.Ticks >= DateTime.Now.Ticks) value = DateTime.Now;
                    _dtEndTime = value;
                    this.RaisePropertyChanged(() => this.DtEndTime);
                }
            }
        }

        #endregion

        #region CmdQuery

        private ICommand _cmdQuery;

        public ICommand CmdQuery
        {
            get
            {
                if (_cmdQuery == null) _cmdQuery = new RelayCommand(ExQuery, CanExQuery, true);
                return _cmdQuery;
            }
        }

        private void ExQuery()
        {
            Query(DtStartTime, DtEndTime);
        }

        private bool CanExQuery()
        {
            if (DtStartTime.Ticks < DtEndTime.Ticks) return true;
            return false;
        }

        #endregion

    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class RecordEventQueryViewModel
    {
       

        public void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolCnt.ClientPart.wlst_RecordEvent_server_ans_clinet_request_event ,
                RecordEventRequest,
                typeof(RecordEventQueryViewModel), this);
        }

        public void RecordEventRequest(string session, ProtocolEncodingCnt<ExchangeReplyRecordEvent> infos)
        {
            var info = infos.Data;
            if (info == null) return;
            this.Record.Clear();
            foreach (var t in info.Info)
            {
                var lst = OneRecordEventViewModel.GetOneRecordVm(t);
                foreach (var g in lst)
                {
                    this.Record.Add(g);
                }
            }
        }

      
    }

    /// <summary>
    /// Socket
    /// </summary>
    public partial class RecordEventQueryViewModel
    {
        private void Query(DateTime dtstarttime, DateTime dtendtime)
        {
            //int waitId = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;
            //var info = new ExchangeRequestEventRecord()
            //              {
            //                  OperatorType = 0,
            //                  RequestEndTime = dtendtime,
            //                  RequestStartTime = dtstarttime,
            //                  Tml = 0,
            //                  UserName = ""
            //              };
            //SndOrderServer.OrderSnd(Wlst.Ux.EquipmentDataQuery.Services.EventIdAssign.RecordEventRequest, null, info,
            //                        waitId);


            var info =Wlst .Sr . ProtocolCnt.ServerPart.wlst_RecordEvent_clinet_request_event;
            info.Data.OperatorType = 0;
            info.Data.RequestEndTime = dtendtime;
            info.Data.RequestStartTime = dtstarttime;
            info.Data.Tml = 0;
            info.Data.UserName = "";
            SndOrderServer.OrderSnd(info);
        }
    }
}
