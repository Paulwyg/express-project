using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.fuyang.OnlineStatus.ViewModel
{
    public class OneItemOnline : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        private int _id;
        /// <summary>
        /// 序号
        /// </summary>
        public int Id
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

        private string _num;
        /// <summary>
        /// 设备编号
        /// </summary>
        public string Num
        {
            get { return _num; }
            set
            {
                if (_num != value)
                {
                    _num = value;
                    this.RaisePropertyChanged(() => this.Num);
                }
            }
        }

        private string _name;
        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    this.RaisePropertyChanged(() => this.Name);
                }
            }
        }

        private string _isOffline;
        /// <summary>
        /// 设备状态
        /// </summary>
        public string IsOffline
        {
            get { return _isOffline; }
            set
            {
                if (_isOffline != value)
                {
                    _isOffline = value;
                    this.RaisePropertyChanged(() => this.IsOffline);
                }
            }
        }

        private string _offlineTime;
        /// <summary>
        /// 设备站离线时间
        /// </summary>
        public string OfflineTime
        {
            get { return _offlineTime; }
            set
            {
                if (_offlineTime != value)
                {
                    _offlineTime = value;
                    this.RaisePropertyChanged(() => this.OfflineTime);
                }
            }
        }
    }
}
