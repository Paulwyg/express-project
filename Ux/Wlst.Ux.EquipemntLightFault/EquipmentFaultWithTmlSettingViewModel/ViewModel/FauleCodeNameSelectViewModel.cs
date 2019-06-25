using System;
using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultWithTmlSettingViewModel.ViewModel
{
    public class FauleCodeNameSelectViewModel : ObservableObject
    {
        private int faultCode;

        public int FaultCode
        {
            get { return faultCode; }
            set
            {
                if (faultCode != value)
                {
                    faultCode = value;
                    this.RaisePropertyChanged(() => this.FaultCode);
                }
            }
        }

        private string faultName;

        /// <summary>
        /// 故障原始名称
        /// </summary>
        public string FaultName
        {
            get { return faultName; }
            set
            {
                if (faultName != value)
                {
                    faultName = value;
                    this.RaisePropertyChanged(() => this.FaultName);
                }
            }
        }

        
       public static event EventHandler OnSelectChanged;
        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    this.RaisePropertyChanged(() => this.IsSelected);
                    if (OnSelectChanged != null) OnSelectChanged(this, EventArgs.Empty);
                }
            }
        }

        private bool isnoalarm;

        public bool IsNoAlarm
        {
            get { return isnoalarm; }
            set
            {
                if (isnoalarm != value)
                {
                    isnoalarm = value;
                    this.RaisePropertyChanged(() => this.IsNoAlarm);
                    if (OnSelectChanged != null) OnSelectChanged(this, EventArgs.Empty);
                }
            }
        }

        private bool isEnable;

        public bool IsEnabel
        {
            get { return isEnable; }
            set
            {
                if (isEnable != value)
                {
                    isEnable = value;
                    this.RaisePropertyChanged(() => this.IsEnabel);
                }
            }
        }

        private string _color;

        /// <summary>
        /// 颜色
        /// </summary>
        public string Color
        {
            get { return _color; }
            set
            {
                if (_color != value)
                {
                    _color = value;
                    this.RaisePropertyChanged(() => this.Color);
                }
            }
        }
    }

    public class NoAlarmViewModel : ObservableObject
    {
        private int _rtuid;

        public int RtuId
        {
            get { return _rtuid; }
            set
            {
                if (_rtuid != value)
                {
                    _rtuid = value;
                    this.RaisePropertyChanged(() => this.RtuId);
                }
            }
        }

        private string _rtuname;

        public string RtuName
        {
            get { return _rtuname; }
            set
            {
                if (_rtuname != value)
                {
                    _rtuname = value;
                    this.RaisePropertyChanged(() => this.RtuName);
                }
            }
        }

        private int _phyid;

        public int PhyId
        {
            get { return _phyid; }
            set
            {
                if (_phyid != value)
                {
                    _phyid = value;
                    this.RaisePropertyChanged(() => this.PhyId);
                }
            }
        }

    }
}
