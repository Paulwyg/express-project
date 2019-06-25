using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.CoreServices;
using Wlst.Ux.Statistics.UxDataStatistics.Services;

namespace Wlst.Ux.Statistics.UxDataStatistics.ViewModel
{
    public sealed class LightRateStatisticsViewModel : ObservableObject
    {

        //public LightRateStatisticsViewModel( LightRateStatisticsViewModel lightRateData)
        //{
        //    this.RtuId = lightRateData.RtuId;
        //    var rtuInfo = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(lightRateData.RtuId);
        //    if (rtuInfo!=null )
        //    {
        //        this.RtuName = rtuInfo.RtuName;
        //        this.PhyId = rtuInfo.RtuPhyId;
        //    }else
        //    {
        //         this.RtuName = "未知设备";
        //        this.PhyId = 0;
        //    }
        //    this.Remark = lightRateData.Remark;
        //    this.Date = lightRateData.Date;
        //    this.BrightRate = lightRateData.BrightRate;

        //}
        public LightRateStatisticsViewModel()
        {
            this.Date = " -- ";
            this.Remark = " -- ";
            this.StrIsOnline = " -- ";
            this.StrBrightRate = " -- ";
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
                    PhyId = tml.RtuPhyId;
                    //if (tml.RtuFid == 0)
                    //    PhyId = tml.RtuPhyId;
                    //else PhyId = value;
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
        private string _date;

        public string  Date
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

        #region IsOnline
        private bool _isOnline;

        public bool IsOnline
        {
            get { return _isOnline; }
            set
            {
                if (_isOnline != value)
                {
                    _isOnline = value;
                    
                    this.RaisePropertyChanged(() => this.IsOnline);
                }
                StrIsOnline = _isOnline ? "在线" : "离线";
            }
        }


        #endregion

        #region StrIsOnline
        private string _isOnlineTxt;

        public string StrIsOnline
        {
            get { return _isOnlineTxt; }
            set
            {
                if (_isOnlineTxt != value)
                {
                    _isOnlineTxt = value;
                    this.RaisePropertyChanged(() => this.StrIsOnline);
                }
            }
        }


        #endregion



        #region BrightRate

        private double _brightRate;

        public double BrightRate
        {
            get { return _brightRate; }
            set
            {
                if (value != _brightRate)
                {
                    _brightRate = value;

                    StrBrightRate = string.Format("{0:0.00}", _brightRate * 100) + " %";
                    RaisePropertyChanged(() => BrightRate);
                }
            }
        }

        #endregion


        #region StrBrightRate

        private string _brightRateTxt;

        public string  StrBrightRate
        {
            get { return _brightRateTxt; }
            set
            {
                if (value != _brightRateTxt)
                {
                    _brightRateTxt = value;


                    RaisePropertyChanged(() => StrBrightRate);
                }
            }
        }

        #endregion

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
