using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryViewModel.ViewModel
{
    /// <summary>
    /// 操作类型模型定义
    /// </summary>
    public class OperatorTypeItem : ObservableObject
    {

        public event EventHandler OnIsSelectedChanged;


        private bool _check;
        public bool IsSelected
        {
            get { return _check; }
            set
            {
                if (_check != value)
                {
                    _check = value;
                    this.RaisePropertyChanged(() => this.IsSelected);
                    if (OnIsSelectedChanged != null)
                    {
                        OnIsSelectedChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        private bool _isShow;

        public bool IsShow
        {
            get { return _isShow; }
            set
            {
                if (_isShow != value)
                {
                    _isShow = value;
                    this.RaisePropertyChanged(() => this.IsShow);
                }
            }
        }

        private bool _checkall;

        public bool IsSelectedAll
        {
            get { return _checkall; }
            set
            {
                if (value == _checkall) return;
                _checkall = value;
                foreach (var item in Value)
                {
                    item.IsSelected = _checkall;
                }

                RaisePropertyChanged(() => IsSelectedAll);
            }
        }

        private string _name;
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }

        private ObservableCollection<NameIntBool> _value;
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<NameIntBool> Value
        {
            get { return _value ?? (_value = new ObservableCollection<NameIntBool>()); }
            set
            {
                if (value == _value) return;
                _value = value;
                RaisePropertyChanged(() => Value);
            }
        }
    }
}
