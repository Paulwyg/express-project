using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.PrivilegesManage.AreaManageViewModel.ViewModels
{
    public class AreaTmlModel : ObservableObject
    {

        public AreaTmlModel()
        {

        }

        private bool _isChecked;

        /// <summary>
        /// 是否选中该条数据
        /// </summary>
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (value != _isChecked)
                {
                    _isChecked = value;
                    this.RaisePropertyChanged(() => this.IsChecked);
                }
            }
        }

        private int _id;

        /// <summary>
        /// 终端地址
        /// </summary>
        public int TmlId
        {
            get { return _id; }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    this.RaisePropertyChanged(() => this.TmlId);
                }
            }
        }

        /// <summary>
        /// 终端物理地址
        /// </summary>
        private int _physicalId;

        public int PhysicalId
        {
            get { return _physicalId; }
            set
            {
                if (_physicalId == value) return;
                _physicalId = value;
                RaisePropertyChanged(() => PhysicalId);
            }
        }

        private string _name;

        /// <summary>
        /// 终端名称
        /// </summary>
        public string TmlName
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    this.RaisePropertyChanged(() => this.TmlName);
                }
            }
        }

        private string _type;

        /// <summary>
        /// 设备类型
        /// </summary>
        public string TmlType
        {
            get { return _type; }
            set
            {
                if (value != _type)
                {
                    _type = value;
                    this.RaisePropertyChanged(() => this.TmlType);
                }
            }
        }

        /// <summary>
        /// 组地址，隐藏 或排序使用；
        /// </summary>
        public int AreaId;

        private string _areaName;

        /// <summary>
        /// 本终端归属组名称
        /// </summary>
        public string AreaName
        {
            get { return _areaName; }
            set
            {
                if (value != _areaName)
                {
                    _areaName = value;
                    this.RaisePropertyChanged(() => this.AreaName);
                }
            }
        }

    }

    
}

