using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.fuyang.BroadcastStrategy.ViewModel
{
    public class OneItemStrategy : Wlst.Cr.Core.CoreServices.ObservableObject
    {

        private string _id;
        /// <summary>
        /// 序号
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

        private string _strategyNum;
        /// <summary>
        /// 策略主键
        /// </summary>
        public string StrategyNum
        {
            get { return _strategyNum; }
            set
            {
                if (_strategyNum != value)
                {
                    _strategyNum = value;
                    this.RaisePropertyChanged(() => this.StrategyNum);
                }
            }
        }

        private string _strategyName;
        /// <summary>
        /// 策略名称
        /// </summary>
        public string StrategyName
        {
            get { return _strategyName; }
            set
            {
                if (_strategyName != value)
                {
                    _strategyName = value;
                    this.RaisePropertyChanged(() => this.StrategyName);
                }
            }
        }

        private string _strategyStatus;
        /// <summary>
        /// 策略状态
        /// </summary>
        public string StrategyStatus
        {
            get { return _strategyStatus; }
            set
            {
                if (_strategyStatus != value)
                {
                    _strategyStatus = value;
                    this.RaisePropertyChanged(() => this.StrategyStatus);
                }
            }
        }

        private string _strategyExplain;
        /// <summary>
        /// 策略说明
        /// </summary>
        public string StrategyExplain
        {
            get { return _strategyExplain; }
            set
            {
                if (_strategyExplain != value)
                {
                    _strategyExplain = value;
                    this.RaisePropertyChanged(() => this.StrategyExplain);
                }
            }
        }
    }
}
