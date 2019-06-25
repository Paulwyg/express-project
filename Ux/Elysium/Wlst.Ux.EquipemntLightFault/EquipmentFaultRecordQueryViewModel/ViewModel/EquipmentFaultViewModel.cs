using System;
using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryViewModel.ViewModel
{
    public class EquipmentFaultViewModel : ObservableObject
    {
        public EquipmentFaultViewModel()
        {
            this.DtCreateTime = "--";
            this.DtRemoceTime = "--";

            Telerik.Windows.Controls.RadTreeListView tr;

        }

        public long DateCreateId;
        public long DateRemoveId;
        public int IsShowAtTop;

        private int _index;

        public int Index
        {
            get { return _index; }
            set
            {
                if (_index != value)
                {
                    _index = value;
                    this.RaisePropertyChanged(() => this.Index);
                }
            }
        }

        private string _dtCreateTime;

        public string DtCreateTime
        {
            get { return _dtCreateTime; }
            set
            {
                if (_dtCreateTime != value)
                {
                    _dtCreateTime = value;
                    this.RaisePropertyChanged(() => this.DtCreateTime);
                }
            }
        }


        private string _dtRemoveTime;

        public string DtRemoceTime
        {
            get { return _dtRemoveTime; }
            set
            {
                if (_dtRemoveTime != value)
                {
                    _dtRemoveTime = value;
                    this.RaisePropertyChanged(() => this.DtRemoceTime);
                }
            }
        }

        private int _rtuId;

        /// <summary>
        /// 终端地址
        /// </summary>
        public int RtuId
        {
            get { return _rtuId; }
            set
            {
                if (value != _rtuId)
                {
                    _rtuId = value;
                    this.RaisePropertyChanged(() => this.RtuId);
                }
            }
        }

        private int _iphyd;

        public int PhyId
        {
            get { return _iphyd; }
            set
            {
                if (_iphyd != value)
                {
                    _iphyd = value;
                    this.RaisePropertyChanged(() => this.PhyId);
                }
            }
        }

        public int LampId;
        private int _rtuLoops;

        /// <summary>
        /// 终端回路地址
        /// </summary>
        public int RtuLoops
        {
            get { return _rtuLoops; }
            set
            {
                if (value != _rtuLoops)
                {
                    _rtuLoops = value;
                    this.RaisePropertyChanged(() => this.RtuLoops);
                }
            }
        }

        private string _rtuLoopName;

        /// <summary>
        /// 终端回路地址
        /// </summary>
        public string RtuLoopName
        {
            get { return _rtuLoopName; }
            set
            {
                if (value != _rtuLoopName)
                {
                    _rtuLoopName = value;
                    this.RaisePropertyChanged(() => this.RtuLoopName);
                }
            }
        }

        private string _rtuName;

        public string RtuName
        {
            get { return _rtuName; }
            set
            {
                if (value != _rtuName)
                {
                    _rtuName = value;
                    this.RaisePropertyChanged(() => this.RtuName);
                }
            }
        }

        private int _faultId;

        public int FaultId
        {
            get { return _faultId; }
            set
            {
                if (value != _faultId)
                {
                    _faultId = value;
                    this.RaisePropertyChanged(() => this.FaultId);
                }
            }
        }


        private string _Color;

        /// <summary>
        /// 报警数据显示颜色
        /// </summary>
        public string Color
        {
            get { return _Color; }
            set
            {
                if (value != _Color)
                {
                    _Color = value;
                    this.RaisePropertyChanged(() => this.Color);
                }
            }
        }


        private int _Count;

        /// <summary>
        /// 指定时间段的报警次数
        /// </summary>
        public int Count
        {
            get { return _Count; }
            set
            {
                if (value != _Count)
                {
                    _Count = value;
                    this.RaisePropertyChanged(() => this.Count);
                }
            }
        }

        private string _faultName;

        public string FaultName
        {
            get { return _faultName; }
            set
            {
                if (value != _faultName)
                {
                    _faultName = value;
                    this.RaisePropertyChanged(() => this.FaultName);
                }
            }
        }


        private string _faulre;

        public string Remark
        {
            get { return _faulre; }
            set
            {
                if (value != _faulre)
                {
                    _faulre = value;
                    this.RaisePropertyChanged(() => this.Remark);
                }
            }
        }
              
    }


    //public class NameIntBoolXg : ObservableObject
    //{
    //    private string _name;

    //    public string Name
    //    {
    //        get { return _name; }
    //        set
    //        {
    //            if (_name != value)
    //            {
    //                _name = value;
    //                this.RaisePropertyChanged(() => this.Name);
    //            }
    //        }
    //    }

    //    private int _value;

    //    public int Value
    //    {
    //        get { return _value; }
    //        set
    //        {
    //            if (_value != value)
    //            {
    //                _value = value;
    //                this.RaisePropertyChanged(() => this.Value);
    //            }
    //        }
    //    }

    //    private bool _check;

    //    public bool IsSelected
    //    {
    //        get { return _check; }
    //        set
    //        {
    //            if (_check != value)
    //            {
    //                _check = value;
    //                this.RaisePropertyChanged(() => this.IsSelected);
    //                if (IsMonitor && OnIsSelectedChanged != null) OnIsSelectedChanged(this, EventArgs.Empty);
    //            }
    //        }
    //    }

    //    public bool IsMonitor;
    //    public event EventHandler OnIsSelectedChanged;
    //}
}
