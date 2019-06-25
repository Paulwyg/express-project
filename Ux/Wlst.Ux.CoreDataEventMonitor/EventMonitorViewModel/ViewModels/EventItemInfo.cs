using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.CoreDataEventMonitor.EventMonitorViewModel.ViewModels
{
    public class EventItemInfo : ObservableObject
    {
        private string _eventType;
        /// <summary>
        /// 
        /// </summary>
        public string EventType
        {
            get { return _eventType; }
            set
            {
                if (_eventType != value)
                {
                    _eventType = value;
                    this.RaisePropertyChanged(() => this.EventType);
                }
            }
        }

        private string _eventPars;
        /// <summary>
        /// 
        /// </summary>
        public string EventPars
        {
            get { return _eventPars; }
            set
            {
                if (_eventPars != value)
                {
                    _eventPars = value;
                    this.RaisePropertyChanged(() => this.EventPars);

                }
            }
        }

        private int _eventId;
        /// <summary>
        /// 
        /// </summary>
        public int  EventId
        {
            get { return _eventId; }
            set
            {
                if (_eventId != value)
                {

                    _eventId = value;
                    this.RaisePropertyChanged(() => this.EventId);
                }
            }
        }

    }
}
