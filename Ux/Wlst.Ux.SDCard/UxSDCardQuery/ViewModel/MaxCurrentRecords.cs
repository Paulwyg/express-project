using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.EventHandlerHelper;

namespace Wlst.Ux.SDCard.UxSDCardQuery.ViewModel
{
    public class MaxCurrentRecords : EventHandlerHelperExtendNotifyProperyChanged
    {
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

                    if (
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.
                            ContainsKey(_rtuid))
                    {

                        this.RtuName = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[
                            _rtuid].RtuName;
                    }
                }
            }
        }

        private string _rtuName;

        public string RtuName
        {
            get { return _rtuName; }
            set
            {
                if (_rtuName != value)
                {
                    _rtuName = value;
                    this.RaisePropertyChanged(() => this.RtuName);
                }
            }
        }

        private string _dtGetDataTime;

        public string DtGetDataTime
        {
            get { return _dtGetDataTime; }
            set
            {
                if (value != _dtGetDataTime)
                {
                    _dtGetDataTime = value;
                    this.RaisePropertyChanged(() => this.DtGetDataTime);
                }
            }
        }

        private int _loopid;

        public int LoopId
        {
            get { return _loopid; }
            set
            {
                if (_loopid != value)
                {
                    _loopid = value;
                    this.RaisePropertyChanged(() => this.LoopId);
                }
            }
        }

        private string _loopName;

        public string LoopName
        {
            get { return _loopName; }
            set
            {
                if (_loopName != value)
                {
                    _loopName = value;
                    this.RaisePropertyChanged(() => this.LoopName);
                }
            }
        }

        private string _maxCurrent;

        /// <summary>
        /// 电流
        /// </summary>
        public string MaxCurrent
        {
            get { return _maxCurrent; }
            set
            {
                if (value != _maxCurrent)
                {
                    _maxCurrent = value;
                    this.RaisePropertyChanged(() => this.MaxCurrent);
                }
            }
        }
   
    }
}
