using System;
using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.CoreDataEventMonitor.SocketDataMonitorViewModel.ViewModes
{

    public class ItemInfo : ObservableObject
    {
        public ItemInfo ()
        {
            DtTime = DateTime.Now;
        }

        private DateTime dateTime;
        public DateTime DtTime
        {
            get { return dateTime; }
            private set
            {
                if (value != dateTime)
                {
                    dateTime = value;
                    this.RaisePropertyChanged(() => this.DtTime);
                }
            }
        }

        private string _type;

        /// <summary>
        /// 
        /// </summary>
        public string SocketDataType
        {
            get { return _type; }
            set
            {
                if (_type != value)
                {
                    _type = value;
                    this.RaisePropertyChanged(() => this.SocketDataType);
                }
            }
        }

        private string _id;

        /// <summary>
        /// 
        /// </summary>
        public string Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    this.RaisePropertyChanged(() => this.Id);
                }
            }
        }

        private string _gid;

        /// <summary>
        /// 
        /// </summary>
        public string Guid
        {
            get { return _gid; }
            set
            {
                if (_gid != value)
                {
                    _gid = value;
                    this.RaisePropertyChanged(() => this.Guid);

                }
            }
        }

        private string _cmd;

        /// <summary>
        /// 
        /// </summary>
        public string Cmd
        {
            get { return _cmd; }
            set
            {
                if (_cmd != value)
                {

                    _cmd = value;
                    this.RaisePropertyChanged(() => this.Cmd);
                }
            }
        }


        private string _argu;

        /// <summary>
        /// 
        /// </summary>
        public string OtherArug
        {
            get { return _argu; }
            set
            {
                if (_argu != value)
                {

                    _argu = value;
                    this.RaisePropertyChanged(() => this.OtherArug);
                }
            }
        }


        private string _data;

        /// <summary>
        /// 
        /// </summary>
        public string Data
        {
            get { return _data; }
            set
            {
                if (_data != value)
                {

                    _data = value;
                    this.RaisePropertyChanged(() => this.Data);
                }
            }
        }
    }
}
