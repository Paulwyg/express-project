using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Services;
using Wlst.client;

namespace Wlst.Ux.Wj6005Module.Models.BaseViewModel
{
    public class Jd601ParViewModel : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        #region

        private int _RtuId;
        private int _EsuOperateId;
        private int _EsuOperatoeValue;
        private int _EsuOperateTimeHour;
        private int _EsuOperateTimeMinute;

        #endregion


        public void UpdateInfo(ReplyEsyPar.Jd601Par info)
        {
            this.RtuId = info.RtuId;
            this.EsuOperateId = info.EsuOperateId;
            this.EsuOperatoeValue = info.EsuOperatoeValue;
            this.EsuOperateTimeHour = info.EsuOperateTime/60;
            this.EsuOperateTimeMinute = info.EsuOperateTime%60;
        }

        public ReplyEsyPar.Jd601Par BackToPar()
        {
            return new ReplyEsyPar.Jd601Par()
                       {
                           EsuOperateId = this.EsuOperateId,
                           EsuOperateTime = this.EsuOperateTimeHour*60 + EsuOperateTimeMinute,
                           EsuOperatoeValue = this.EsuOperatoeValue,
                           RtuId = this.RtuId,
                           UpdateTime = DateTime.Now.Ticks ,
                           UpdateUsername = UserInfo.UserLoginInfo.UserName
                       };
        }

        /// <summary>
        /// 节能设备地址
        /// </summary>

        public int RtuId
        {
            get { return _RtuId; }
            set
            {
                if (_RtuId == value) return;
                _RtuId = value;
                this.RaisePropertyChanged(() => this.RtuId);
            }
        }

        /// <summary>
        /// 操作序号 共有8个时间需要 1~8
        /// </summary>

        public int EsuOperateId
        {
            get { return _EsuOperateId; }
            set
            {
                if (_EsuOperateId == value) return;
                _EsuOperateId = value;
                this.RaisePropertyChanged(() => this.EsuOperateId);
            }
        }

        /// <summary>
        /// 操作节能值
        /// </summary>

        public int EsuOperatoeValue
        {
            get { return _EsuOperatoeValue; }
            set
            {
                if (_EsuOperatoeValue == value) return;
                _EsuOperatoeValue = value;
                this.RaisePropertyChanged(() => this.EsuOperatoeValue);
            }
        }

        /// <summary>
        /// 操作时间 如00：30  则为30 时*60+分
        /// </summary>

        public int EsuOperateTimeHour
        {
            get { return _EsuOperateTimeHour; }
            set
            {
                if (_EsuOperateTimeHour == value) return;
                if (value < 0) return;
                if (value > 23) return;
                _EsuOperateTimeHour = value;
                this.RaisePropertyChanged(() => this.EsuOperateTimeHour);
            }
        }

        public int EsuOperateTimeMinute
        {
            get { return _EsuOperateTimeMinute; }
            set
            {
                if (_EsuOperateTimeMinute == value) return;
                if (value < 0) return;
                if (value > 59) return;
                _EsuOperateTimeMinute = value;
                this.RaisePropertyChanged(() => this.EsuOperateTimeMinute);
            }
        }

        ///// <summary>
        ///// 本条信息更新时间
        ///// </summary>

        //public DateTime UpdateTime { get; set; }

        ///// <summary>
        ///// 更新本条信息的用户
        ///// </summary>

        //public string UpdateUsername { get; set; }
    }
}
