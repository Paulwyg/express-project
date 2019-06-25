using System;
using System.Collections.ObjectModel;
using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQuerySZViewModel.ViewModel
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

        #region Records

        private ObservableCollection<EquipmentFaultViewModel> _record;

        public ObservableCollection<EquipmentFaultViewModel> Items
        {
            get { return _record ?? (_record = new ObservableCollection<EquipmentFaultViewModel>()); }
            set
            {
                if (_record != value)
                {
                    _record = value;
                    this.RaisePropertyChanged(() => this.Items);
                }
            }
        }

        #endregion
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
                    //todo
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

        private string  _a;

        public string A
        {
            get { return _a; }
            set
            {
                if (value != _a)
                {
                    _a = value;
                    this.RaisePropertyChanged(() => this.A);
                }
            }
        }

        private string _aeding;

        public string Aeding
        {
            get { return _aeding; }
            set
            {
                if (value != _aeding)
                {
                    _aeding = value;
                    this.RaisePropertyChanged(() => this.Aeding);
                }
            }
        }


        private string _aLower;

        public string ALower
        {
            get { return _aLower; }
            set
            {
                if (value != _aLower)
                {
                    _aLower = value;
                    this.RaisePropertyChanged(() => this.ALower);
                }
            }
        }

        private string _aUpper;

        public string AUpper
        {
            get { return _aUpper; }
            set
            {
                if (value != _aUpper)
                {
                    _aUpper = value;
                    this.RaisePropertyChanged(() => this.AUpper);
                }
            }
        }

        private string _v;

        public string V
        {
            get { return _v; }
            set
            {
                if (value != _v)
                {
                    _v = value;
                    this.RaisePropertyChanged(() => this.V);
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

        private string _cqj;

        /// <summary>
        /// 城区局
        /// </summary>
        public string CQJ
        {
            get { return _cqj; }
            set
            {
                if (value != _cqj)
                {
                    _cqj = value;
                    this.RaisePropertyChanged(() => this.CQJ);
                }
            }
        }

        private string  _dygh;

        /// <summary>
        /// 电源杆号
        /// </summary>
        public string DYGH
        {
            get { return _dygh; }
            set
            {
                if (value != _dygh)
                {
                    _dygh = value;
                    this.RaisePropertyChanged(() => this.DYGH);
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
