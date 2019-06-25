using System;
using System.Windows.Input;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;

namespace Wlst.Ux.TimeTableSystem.TimeTableManagViewModel.ViewModel
{
    public class AddTimeTableViewModel:ObservableObject
    {
        #region Command
        #region CmdCancel

        private DateTime _dtCancel;
        private ICommand _cmdCancel;
        public ICommand CmdCancel
        {
            get
            {
                return _cmdCancel ?? (_cmdCancel = new RelayCommand(ExCmdCancel, CanCmdCancel, true));
            }
        }
        private   bool CanCmdCancel()
        {
            return DateTime.Now.Ticks - _dtCancel.Ticks>30000000;
        }
        private void ExCmdCancel()
        {
            _dtCancel = DateTime.Now;
            var ar = new PublishEventArgs
            {
                EventId = TimeTableSystem.Services.EventIdAssign.EventCancelTimeTableData,
                EventType = PublishEventType.Core
            };
            EventPublisher.EventPublish(ar);

            ExPublishAnimationEvent();

        }
        #endregion

        #region CmdSaveTimeTable

        private DateTime _dtSaveTimeTable;
        private ICommand _cmdSaveTimeTable;
        public ICommand CmdSaveTimeTable
        {
            get
            {
                return _cmdSaveTimeTable ?? (_cmdSaveTimeTable = new RelayCommand(ExCmdSaveTimeTable, CanCmdSaveTimeTable, true));
            }
        }
        private  bool CanCmdSaveTimeTable()
        {
            return DateTime.Now.Ticks-_dtSaveTimeTable.Ticks>30000000;
        }
        private void ExCmdSaveTimeTable()
        {
            _dtSaveTimeTable = DateTime.Now;
            ExPublishAnimationEvent();

            var ar = new PublishEventArgs
            {
                EventId = TimeTableSystem.Services.EventIdAssign.EventSaveTimeTableData,
                EventType = PublishEventType.Core
            };
            EventPublisher.EventPublish(ar);
        }
        #endregion
        private void ExPublishAnimationEvent()
        {
            //发布事件，返回原界面
            var ar = new PublishEventArgs
            {
                EventId = TimeTableSystem.Services.EventIdAssign.EventSaveOrCancelTimeTableAnimationId,
                EventType = PublishEventType.None
            };
            EventPublisher.EventPublish(ar);
        }
        #endregion

        #region OneItem

        private OneItemTimeTable _oneItemTimeTable;
        public OneItemTimeTable OneItemTimeTable
        {
            get { return _oneItemTimeTable; }
            set
            {
                if (_oneItemTimeTable == value) return;
                _oneItemTimeTable = value;
                RaisePropertyChanged(() => OneItemTimeTable);
            }
        }
        #endregion
    }
}
