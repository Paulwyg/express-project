using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.AssetManagementModule.LampManage.ViewModel
{
    public class LampItemModel:ObservableObject
    {
        private int _index;
        /// <summary>
        /// 序号
        /// </summary>
        public int Index
        {
            get { return _index; }
            set
            {
                if (value != _index)
                {
                    _index = value;
                    this.RaisePropertyChanged(() => this.Index);
                }
            }
        }

        private int _nodeId;
        /// <summary>
        /// 终端id
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

        private int _logicalId;
        /// <summary>
        /// 终端逻辑id
        /// </summary>
        public int LogicalId
        {
            get { return _logicalId; }
            set
            {
                if (value != _logicalId)
                {
                    _logicalId = value;
                    this.RaisePropertyChanged(() => this.LogicalId);
                }
            }
        }

        private string _nodeName;
        /// <summary>
        /// 终端名称
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

        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> _bureauList;
        /// <summary>
        /// 城区局列表
        /// </summary>
        public ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> BureauList
        {
            get
            {
                if (_bureauList == null)
                {
                    _bureauList = new ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt>();
                }
                return _bureauList;
            }
            set
            {
                if (value == _bureauList) return;
                _bureauList = value;
                this.RaisePropertyChanged(() => BureauList);
            }
        }

        private Wlst.Cr.CoreOne.Models.NameValueInt _selectedBureau;
        /// <summary>
        /// 选择的城区局
        /// </summary>
        public Wlst.Cr.CoreOne.Models.NameValueInt SelectedBureau
        {
            get { return _selectedBureau; }
            set
            {
                if (value != _selectedBureau)
                {
                    _selectedBureau = value;
                    this.RaisePropertyChanged(() => this.SelectedBureau);
                }
            }
        }

        //private string _bureau;
        ///// <summary>
        ///// 城区局
        ///// </summary>
        //public string Bureau
        //{
        //    get { return _bureau; }
        //    set
        //    {
        //        if (value != _bureau)
        //        {
        //            _bureau = value;
        //            this.RaisePropertyChanged(() => this.Bureau);
        //        }
        //    }
        //}

        private string _powerNum;
        /// <summary>
        /// 电源杆号
        /// </summary>
        public string PowerNum
        {
            get { return _powerNum; }
            set
            {
                if (value != _powerNum)
                {
                    _powerNum = value;
                    this.RaisePropertyChanged(() => this.PowerNum);
                }
            }
        }

        private string _ammeter;
        /// <summary>
        /// 电表标号
        /// </summary>
        public string AmmeterNum
        {
            get { return _ammeter; }
            set
            {
                if (value != _ammeter)
                {
                    _ammeter = value;
                    this.RaisePropertyChanged(() => this.AmmeterNum);
                }
            }
        }

        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> _transferState;
        /// <summary>
        /// 移交状态
        /// </summary>
        public ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> TransferState
        {
            get
            {
                if (_transferState == null)
                {
                    _transferState = new ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt>();
                }
                return _transferState;
            }
            set
            {
                if (value == _transferState) return;
                _transferState = value;
                this.RaisePropertyChanged(() => TransferState);
            }
        }

        private Wlst.Cr.CoreOne.Models.NameValueInt _selectedState;
        /// <summary>
        /// 移交状态
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
