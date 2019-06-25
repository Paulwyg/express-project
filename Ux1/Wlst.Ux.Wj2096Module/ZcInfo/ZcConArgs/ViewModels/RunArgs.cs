using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.Wj2096Module.ZcInfo.ZcConArgs.ViewModels
{
    public class RunArgs : Wlst.Cr.Core.CoreServices.ObservableObject
    {       
        /// <summary>
        /// 域名 
        /// </summary>
        #region Domain
        private int _domain;
        public int Domain
        {
            get { return _domain; }
            set
            {
                if (_domain == value) return;
                _domain = value;
                RaisePropertyChanged(() => Domain);
            }
        }
        #endregion

        /// <summary>
        /// 经纬度
        /// </summary>
        #region Latitude_longitude
        private string _latitude_longitude;
        public string Latitude_longitude
        {
            get { return _latitude_longitude; }
            set
            {
                if (_latitude_longitude == value) return;
                _latitude_longitude = value;
                RaisePropertyChanged(() => Latitude_longitude);
            }
        }
        #endregion

        /// <summary>
        /// 投运
        /// </summary>
        #region IsRun
        private bool _isRun;
        public bool IsRun
        {
            get { return _isRun; }
            set
            {
                if (_isRun == value) return;
                _isRun = value;
                RaisePropertyChanged(() => IsRun);
            }
        }
        #endregion

        /// <summary>
        /// 主报
        /// </summary>
        #region IsActiveAlarm
        private bool _isActiveAlarm;
        public bool IsActiveAlarm
        {
            get { return _isActiveAlarm; }
            set
            {
                if (_isActiveAlarm == value) return;
                _isActiveAlarm = value;
                RaisePropertyChanged(() => IsActiveAlarm);
            }
        }
        #endregion

        /// <summary>
        /// 主报周期
        /// </summary>
        #region UplinkTimer
        private int _uplinkTimer;
        public int UplinkTimer
        {
            get { return _uplinkTimer; }
            set
            {
                if (_uplinkTimer == value) return;
                _uplinkTimer = value;
                RaisePropertyChanged(() => UplinkTimer);
            }
        }
        #endregion

        /// <summary>
        /// 是否应答
        /// </summary>
        #region UplinkReply
        private string _uplinkReply;
        public string UplinkReply
        {
            get { return _uplinkReply; }
            set
            {
                if (_uplinkReply == value) return;
                _uplinkReply = value;
                RaisePropertyChanged(() => UplinkReply);
            }
        }
        #endregion

        /// <summary>
        /// 回路数量 
        /// </summary>
        #region LoopCount
        private int _loopCount;
        public int LoopCount
        {
            get { return _loopCount; }
            set
            {
                if (_loopCount == value) return;
                _loopCount = value;
                RaisePropertyChanged(() => LoopCount);
                foreach (var tttt in IsVisiByLoop)
                {
                    tttt.IsSelected = false;
                    if (tttt.Value <= _loopCount)
                        tttt.IsSelected = true;
                }
            }
        }
        #endregion

        /// <summary>
        /// 根据回路数确定显示相关的数据
        /// </summary>
        ObservableCollection<NameIntBool> _isVisiByLoop;
        public ObservableCollection<NameIntBool> IsVisiByLoop
        {
            get
            {
                if (_isVisiByLoop == null)
                {
                    _isVisiByLoop = new ObservableCollection<NameIntBool>();
                    for (int i = 1; i <= 4; i++)
                    {
                        _isVisiByLoop.Add(new NameIntBool() { IsSelected = false, Value = i });
                    }
                }
                return _isVisiByLoop;
            }
        }

        /// <summary>
        /// 上电开灯1-4
        /// </summary>
        ObservableCollection<NameIntBool> _isPowerOnLight;
        public ObservableCollection<NameIntBool> IsPowerOnLight
        {
            get
            {
                if (_isPowerOnLight == null)
                {
                    _isPowerOnLight = new ObservableCollection<NameIntBool>();
                    for (int i = 1; i <= 4; i++)
                    {
                        _isPowerOnLight.Add(new NameIntBool() { IsSelected = true, Value = i });
                    }
                }
                return _isPowerOnLight;
            }
        }

        /// <summary>
        /// 回路1-4矢量
        /// </summary>
        ObservableCollection<NameIntBool> _loopVector;
        public ObservableCollection<NameIntBool> LoopVector
        {
            get
            {
                if (_loopVector == null)
                {
                    _loopVector = new ObservableCollection<NameIntBool>();
                    for (int i = 1; i <= 4; i++)
                    {
                        _loopVector.Add(new NameIntBool() {Value = i });
                    }
                }
                return _loopVector;
            }
        }

        /// <summary>
        /// 回路1-4额定功率
        /// </summary>
        ObservableCollection<NameIntBool> _loopRatePower;
        public ObservableCollection<NameIntBool> LoopRatePower
        {
            get
            {
                if (_loopRatePower == null)
                {
                    _loopRatePower = new ObservableCollection<NameIntBool>();
                    for (int i = 1; i <= 4; i++)
                    {
                        _loopRatePower.Add(new NameIntBool());
                    }
                }
                return _loopRatePower;
            }
        }
    }
}
