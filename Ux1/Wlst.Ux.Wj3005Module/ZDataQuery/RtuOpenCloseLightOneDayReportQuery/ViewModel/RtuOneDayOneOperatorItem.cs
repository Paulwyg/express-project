using System.Collections.ObjectModel;

namespace Wlst.Ux.WJ3005Module.ZDataQuery.RtuOpenCloseLightOneDayReportQuery.ViewModel
{
    public class RtuOneDayOneOperatorItem : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        #region attri

        private int _rtuId;

        public int RtuId
        {
            get { return _rtuId; }
            set
            {
                if (value == _rtuId) return;
                _rtuId = value;
                this.RaisePropertyChanged(() => this.RtuId);
                if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(value) == false)
                    return;
                PhyId =
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( value).RtuPhyId ;
               RtuName = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( value).RtuName ;
            }
        }

        private int _phyId;

        public int PhyId
        {
            get { return _phyId; }
            set
            {
                if (value == _phyId) return;
                _phyId = value;
                this.RaisePropertyChanged(() => this.PhyId);
            }
        }


        private string  _phname;

        public string  RtuName
        {
            get { return _phname; }
            set
            {
                if (value == _phname) return;
                _phname = value;
                this.RaisePropertyChanged(() => this.RtuName);
            }
        }

        private string _date;

        public string Date
        {
            get { return _date; }
            set
            {
                if (value == _date) return;
                _date = value;
                this.RaisePropertyChanged(() => this.Date);
            }
        }


        private string _doperator;

        public string Operator
        {
            get { return _doperator; }
            set
            {
                if (value == _doperator) return;
                _doperator = value;
                this.RaisePropertyChanged(() => this.Operator);
            }
        }

        #endregion

        private ObservableCollection<RtuOneLoopItem> _loopsItem;

        /// <summary>
        /// 后台已经执行初始化 并设定为K1-k6分别为 0-5
        /// </summary>
        public ObservableCollection<RtuOneLoopItem> LoopsItem
        {
            get
            {
                if (_loopsItem == null)
                {
                    _loopsItem = new ObservableCollection<RtuOneLoopItem>();
                    for (int i = 1; i < 17; i++)
                        _loopsItem.Add(new RtuOneLoopItem() {IsSucc = "---", LoopId = i, Time = "不操作-"});
                }
                return _loopsItem;
            }
        }
    }


    public class RtuOneLoopItem : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        #region attri

        private int _looId;

        public int LoopId
        {
            get { return _looId; }
            set
            {
                if (value == _looId) return;
                _looId = value;
                this.RaisePropertyChanged(() => this.LoopId);
            }
        }


        private string _isSucc;

        public string IsSucc
        {
            get { return _isSucc; }
            set
            {
                if (value == _isSucc) return;
                _isSucc = value;
                this.RaisePropertyChanged(() => this.IsSucc);
            }
        }


        private string _time;

        public string Time
        {
            get { return _time; }
            set
            {
                if (value == _time) return;
                _time = value;
                this.RaisePropertyChanged(() => this.Time);
            }
        }

        #endregion
    }
}
