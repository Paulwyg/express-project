using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.AssetManagementModule.SimManage.ViewModel
{
    public class SimItemModel:ObservableObject
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
                if (value != _id)
                {
                    _id = value;
                    this.RaisePropertyChanged(() => this.Id);
                }
            }
        }

        private string _telNum;
        /// <summary>
        /// 电话号码
        /// </summary>
        [StringLength(12, ErrorMessage = "号码长度不得大于11")]
        public string TelNum
        {
            get { return _telNum; }
            set
            {
                if (value != _telNum)
                {
                    _telNum = value;
                    this.RaisePropertyChanged(() => this.TelNum);
                }
            }
        }

        private string _nodeName;
        /// <summary>
        /// 所属终端名称
        /// </summary>
        public string NodeName
        {
            get { return _nodeName; }
            set
            {
                if (value != _nodeName)
                {
                    _nodeName = value;
                    this.RaisePropertyChanged(() => this.NodeName);
                }
            }
        }

        private int _nodeId;
        /// <summary>
        /// 所属终端id
        /// </summary>
        public int NodeId
        {
            get { return _nodeId; }
            set
            {
                if (value != _nodeId)
                {
                    _nodeId = value;
                    this.RaisePropertyChanged(() => this.NodeId);
                }
            }
        }

        private string _ip;
        /// <summary>
        /// ip地址
        /// </summary>
        public string IP
        {
            get { return _ip; }
            set
            {
                if (value != _ip)
                {
                    _ip = value;
                    this.RaisePropertyChanged(() => this.IP);
                }
            }
        }

        private DateTime _activateTime;
        /// <summary>
        /// 开通时间
        /// </summary>
        public DateTime ActivateTime
        {
            get { return _activateTime; }
            set
            {
                if (value != _activateTime)
                {
                    _activateTime = value;
                    this.RaisePropertyChanged(() => this.ActivateTime);
                }
            }
        }

        private DateTime _chargeTime;
        /// <summary>
        /// 续费时间
        /// </summary>
        public DateTime ChargeTime
        {
            get { return _chargeTime; }
            set
            {
                if (value != _chargeTime)
                {
                    _chargeTime = value;
                    this.RaisePropertyChanged(() => this.ChargeTime);
                }
            }
        }

       
        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> _state;
        /// <summary>
        /// 状态
        /// </summary>
        public ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> State
        {
            get
            {
                if (_state == null)
                {
                    _state = new ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt>();
                }
                return _state;
            }
            set
            {
                if (value == _state) return;
                _state = value;
                this.RaisePropertyChanged(() => State);
            }
        }

        private Wlst.Cr.CoreOne.Models.NameValueInt _selectedState;
        /// <summary>
        /// 状态
        /// </summary>
        public Wlst.Cr.CoreOne.Models.NameValueInt SelectedState
        {
            get { return _selectedState; }
            set
            {
                if (value != _selectedState)
                {
                    _selectedState = value;
                    this.RaisePropertyChanged(() => this.SelectedState);
                }
            }
        }

    }
}
