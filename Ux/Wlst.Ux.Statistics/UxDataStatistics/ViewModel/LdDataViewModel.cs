using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.Statistics.UxDataStatistics.ViewModel
{
    public sealed class LdDataViewModel : ObservableObject
    {
        public LdDataViewModel(DateTime timeStamp, double value)
        {
            this.Date = timeStamp;
            this.A = value;
        }


        public LdDataViewModel(string category, double value)
        {
            this._category = category;
            this.A = value;
        }
        #region Attri

        #region RtuName

        private string _rtuName;

        public string RtuName
        {
            get { return _rtuName; }
            set
            {
                if (value != _rtuName)
                {
                    _rtuName = value;
                    RaisePropertyChanged(() => RtuName);
                }
            }
        }

        #endregion

        #region RtuId


        private int _rtuid;

        public int RtuId
        {
            get { return _rtuid; }
            set
            {

                if (value != _rtuid)
                {
                    _rtuid = value;
                    PhyId = value;
                    RaisePropertyChanged(() => RtuId);
                    RtuName = "Reserve";
                    if (
                        !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.
                             InfoItems.ContainsKey
                             (_rtuid))
                        return;
                    var tml =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                            [_rtuid];
                    RtuName = tml.RtuName;

                    if (tml.RtuFid == 0)
                        PhyId = tml.RtuPhyId;
                    else PhyId = value;
                }
            }
        }

        #endregion

        #region PhyId
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
        #endregion

        #region Index
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
        #endregion

        #region Date
        private DateTime _date;

        public DateTime Date
        {
            get { return _date; }
            set
            {
                if (_date != value)
                {
                    _date = value;
                    this.RaisePropertyChanged(() => this.Date);
                }
            }
        }
        #endregion

        #region 当前值

        private double _a;
        public double A
        {
            get { return _a; }
            set
            {
                if (value != _a)
                {

                    _a = value;
                    RaisePropertyChanged(() => A);
                }
            }
        }
        #endregion

        private string _category;
        public string Category
        {
            get
            {
                return this._category;
            }
            set
            {
                if (this._category != value)
                {
                    this._category = value;
                }
            }
        }

        #region Remark

        private string _remark;

        public string Remark
        {
            get { return _remark; }
            set
            {
                if (value != _remark)
                {
                    _remark = value;
                    RaisePropertyChanged(() => Remark);
                }
            }
        }

        #endregion


        #endregion
    
    }
}
