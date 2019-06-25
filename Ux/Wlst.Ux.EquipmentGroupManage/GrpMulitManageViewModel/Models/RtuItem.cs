using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Commands;

namespace Wlst.Ux.EquipmentGroupManage.GrpMulitManageViewModel.Models
{
    public class RtuItem : ObservableObject
    {

        public RtuItem()
        {
            _CurrentItem = new PathItem();
          
        }

        private bool _IsChecked;
        /// <summary>
        /// 目前选择的分组 本终端是否归属于该组
        /// </summary>
        public bool IsSelected
        {
            get { return _IsChecked; }
            set
            {
                if (value != _IsChecked)
                {
                    _IsChecked = value;
                    this.RaisePropertyChanged(() => this.IsSelected);
                }
            }
        }

        private int _id;
        /// <summary>
        /// 终端地址
        /// </summary>
        public int ID
        {
            get { return _id; }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    this.RaisePropertyChanged(() => this.ID);
                }
            }
        }

        private int _phyId;
        public int PhyId
        {
            get { return _phyId; }
            set
            {
                if(_phyId==value)return;
                _phyId = value;
                RaisePropertyChanged(()=>PhyId);
            }
        }

        private string _Name;
        /// <summary>
        /// 终端名称
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set
            {
                if (value != _Name)
                {
                    _Name = value;
                    this.RaisePropertyChanged(() => this.Name);
                }
            }
        }

        private string _type;
        /// <summary>
        /// 设备类型
        /// </summary>
        public string Type
        {
            get { return _type; }
            set
            {
                if (value != _type)
                {
                    _type = value;
                    this.RaisePropertyChanged(() => this.Type);
                }
            }
        }

        private string _Area;
        /// <summary>
        /// 终端所属区域
        /// </summary>
        public string Area
        {
            get { return _Area; }
            set
            {
                if (value != _Area)
                {
                    _Area = value;
                    this.RaisePropertyChanged(() => this.Area);
                }
            }
        }

        private int _areaId;
        /// <summary>
        /// 终端所属区域id
        /// </summary>
        public int AreaId
        {
            get { return _areaId; }
            set
            {
                if (value != _areaId)
                {
                    _areaId = value;
                    this.RaisePropertyChanged(() => this.AreaId);
                }
            }
        }

        private int _PathCounte;
        /// <summary>
        /// 本终端归属组的数量
        /// </summary>
        public int PathCounte
        {
            get { return _PathCounte; }
            set
            {
                if (value != _PathCounte)
                {
                    _PathCounte = value;
                    this.RaisePropertyChanged(() => this.PathCounte);
                }
            }
        }


        private ObservableCollection<PathItem> _pathItemsforthistml;
        /// <summary>
        /// 本终端归属组的所有详细路径
        /// </summary>
        public ObservableCollection<PathItem> PathItemsforthistml
        {

            get
            {
                if (_pathItemsforthistml == null) _pathItemsforthistml = new ObservableCollection<PathItem>();
                return _pathItemsforthistml;
            }
        }

        private Visibility _btnDetailVisible;
        public Visibility BtnDetailVisible
        {
            get
            {
                return  _btnDetailVisible;
            }
            set
            {
                if(value !=_btnDetailVisible)
                {
                    _btnDetailVisible = value;
                    RaisePropertyChanged(()=>this.BtnDetailVisible);
                }
            }
        }


        private ICommand _cmdWatchDetailInfo;
        public ICommand CmdWatchDetailInfo
        {
            get
            {
                if(_cmdWatchDetailInfo==null) _cmdWatchDetailInfo=new RelayCommand(ExWatchDetailInfo,CanWathcDetailInfo,true);
                return _cmdWatchDetailInfo;
            }
        }
        private void ExWatchDetailInfo()
        {
            if (CmdWatchDetailInfo == null) return;
            OnCmdWatchDetailInfo(this, EventArgs.Empty);
        }
        public event EventHandler OnCmdWatchDetailInfo;
        private bool CanWathcDetailInfo()
        {
            return true;
        }
        private PathItem _CurrentItem;
        /// <summary>
        ///
        /// </summary>
        public PathItem CurrentItem
        {
            get { return _CurrentItem; }
            set
            {
                if (_CurrentItem != value)
                {
                    _CurrentItem = value;
                    this.RaisePropertyChanged(() => this.CurrentItem);
                }
            }
        }
    }
}
