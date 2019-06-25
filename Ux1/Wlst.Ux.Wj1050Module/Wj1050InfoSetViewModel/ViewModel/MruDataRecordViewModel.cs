using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.client;


namespace Wlst.Ux.Wj1050Module.Wj1050InfoSetViewModel.ViewModel
{
    public class MruDataRecordViewModel : Wlst.Cr.Core.CoreServices.ObservableObject
    {

        #region pri attri

        private int _value1;
        private int _value2;
        private string _value3;
        private string _value4;
        private string _value5;
        private double _value6;
        private double _Value7;

        #endregion

        #region attri

        public int Id
        {
            get { return _value1; }
            set
            {
                if (value != _value1)
                {
                    _value1 = value;
                    this.RaisePropertyChanged(() => this.Id);
                }
            }
        }


        public double Differ
        {
            get { return _Value7; }
            set
            {
                if (value != _Value7)
                {
                    _Value7 = value;
                    this.RaisePropertyChanged(() => this.Differ);
                }
            }
        }


        private double _count;
        public double Count
        {
            get { return _count; }
            set
            {
                if (value != _count)
                {
                    _count = value;
                    this.RaisePropertyChanged(() => this.Count);
                }
            }
        }
        /// <summary>
        /// 设备地址
        /// </summary>
        public int RtuId
        {
            get { return _value2; }
            set
            {
                if (value != _value2)
                {
                    _value2 = value;
                    this.RaisePropertyChanged(() => this.RtuId);
                }
            }
        }


        /// <summary>
        /// 抄表时间
        /// </summary>
        public string DateCreate
        {
            get { return _value3; }
            set
            {
                if (value != _value3)
                {
                    _value3 = value;
                    this.RaisePropertyChanged(() => this.DateCreate);
                }
            }
        }

        /// <summary>
        /// 抄表时间类型：0 当前时间；1上月时间；2上上月时间
        /// </summary>
        public string DateTypeCode
        {
            get { return _value4; }
            set
            {
                if (value != _value4)
                {
                    _value4 = value;
                    this.RaisePropertyChanged(() => this.DateTypeCode);
                }
            }
        }

        /// <summary>
        /// 抄表类型：0保留；1 A相；2 B相；3 C相；4 总电量
        /// </summary>
        public string MruTypeCode
        {
            get { return _value5; }
            set
            {
                if (value != _value5)
                {
                    _value5 = value;
                    this.RaisePropertyChanged(() => this.MruTypeCode);
                }
            }
        }

        /// <summary>
        /// 抄表值
        /// </summary>
        public double MruData
        {
            get { return _value6; }
            set
            {
                if (value != _value6)
                {
                    _value6 = value;
                    this.RaisePropertyChanged(() => this.MruData);
                }
            }
        }

        #endregion
        private double _stotal;
        public double MruTotal
        {
            get { return _stotal; }
            set
            {
                if (value == _stotal) return;
                _stotal = value;
                RaisePropertyChanged(() => MruTotal);
            }
        }

        public double sacal;
        public MruDataRecordViewModel()
        {
            this.Id = 0;
            this.MruData = 0;
            this.DateCreate = "--";
            this.DateTypeCode = "--";
            this.MruTypeCode = "--";
            this.RtuId = 0;
        }

        public MruDataRecordViewModel(MruDataRequest.MruDataItem info)
        {
            this.Id = 0;
            this.MruData = info.MruData;
            this.DateCreate = info.DateCreate + "";
            this.DateTypeCode = info.DateType == 0
                                    ? "当前"
                                    : info.DateType == 1 ? "上月" : "上上月";
            this.MruTypeCode = info.MruType == 4
                                   ? "总电量"
                                   : info.MruType == 1
                                         ? "A相"
                                         : info.MruType == 2
                                               ? "B相"
                                               : info.MruType == 3 ? "C相" : "--";
            this.RtuId = info.RtuId;


            var fidequ = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById(  info.RtuId);
            double scall = 0;
            if (fidequ != null)
            {

                var ntsdf =
                    fidequ as Wlst.Sr .EquipmentInfoHolding .Model .Wj1050Mru ;
                if (ntsdf != null) scall = ntsdf.WjMru .MruRatio/5.0;
                MruTotal = scall*info.MruData;
            }
        }
    }
}
