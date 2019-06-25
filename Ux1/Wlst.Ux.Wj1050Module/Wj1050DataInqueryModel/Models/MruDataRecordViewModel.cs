using System;
using Wlst.client;


namespace Wlst.Ux.Wj1050Module.Wj1050DataInqueryModel.Models
{
    public class MruDataRecordViewModel : Cr.Core.CoreServices.ObservableObject
    {
        #region pri attri

        private int _value1;
        private int _value2;
        private string _value3;
        private string _value4;
        private string _value5;
        private double _value6;
        private string _value7;
        private int _value8;
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
                    RaisePropertyChanged(() => Id);
                }
            }
        }


        public string Differ
        {
            get { return _value7; }
            set
            {
                if (value.Equals(_value7)) return;
                _value7 = value;
                RaisePropertyChanged(() => Differ);
            }
        }


        private double _count;
        public double Count
        {
            get { return _count; }
            set
            {
                if (value.Equals(_count)) return;
                _count = value;
                RaisePropertyChanged(() => Count);
            }
        }

        private double _differData;
        public double DifferData
        {
            get { return _differData; }
            set
            {
                if (value.Equals(_differData)) return;
                _differData = value;
                RaisePropertyChanged(() => DifferData);
            }
        }

        private double _differTotal;
        public double DifferTotal
        {
            get { return _differTotal; }
            set
            {
                if (value.Equals(_differTotal)) return;
                _differTotal = value;
                RaisePropertyChanged(() => DifferTotal);
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
                if (value == _value2) return;
                _value2 = value;
                RaisePropertyChanged(() => RtuId);
            }
        }

        public int AttachRtuId
        {
            get { return _value8; }
            set
            {
                if (value == _value8) return;
                _value8 = value;
                RaisePropertyChanged(() => AttachRtuId);
            }
        }

        private string _sAttachRtuName;
        public string AttachRtuName
        {
            get { return _sAttachRtuName; }
            set
            {
                if (value == _sAttachRtuName) return;
                _sAttachRtuName = value;
                RaisePropertyChanged(() => AttachRtuName);
            }
        }


        private string _sAttaRemarkschRtuName;
        public string Remarks
        {
            get { return _sAttaRemarkschRtuName; }
            set
            {
                if (value == _sAttaRemarkschRtuName) return;
                _sAttaRemarkschRtuName = value;
                RaisePropertyChanged(() => Remarks);
            }
        }

        private string _sAttRtuNameaRemarkschRtuName;
        public string RtuName
        {
            get { return _sAttRtuNameaRemarkschRtuName; }
            set
            {
                if (value == _sAttRtuNameaRemarkschRtuName) return;
                _sAttRtuNameaRemarkschRtuName = value;
                RaisePropertyChanged(() => RtuName);
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
                    RaisePropertyChanged(() => DateCreate);
                }
            }
        }

        /// <summary>
        /// 起始时间
        /// </summary>
        private string _begTime1;
        public string BegTime1
        {
            get { return _begTime1; }
            set
            {
                if (value != _begTime1)
                {
                    _begTime1 = value;
                    RaisePropertyChanged(() => BegTime1);
                }
            }
        }

        /// <summary>
        /// 终止时间
        /// </summary>
        private string _endTime1;
        public string EndTime1
        {
            get { return _endTime1; }
            set
            {
                if (value != _endTime1)
                {
                    _endTime1 = value;
                    RaisePropertyChanged(() => EndTime1);
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
                    RaisePropertyChanged(() => DateTypeCode);
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
                    RaisePropertyChanged(() => MruTypeCode);
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
                if (value.Equals(_value6)) return;
                _value6 = value;
                RaisePropertyChanged(() => MruData);
            }
        }


        /// <summary>
        /// 起始抄表值
        /// </summary>
        private double _bMruData;
        public double BMruData
        {
            get { return _bMruData; }
            set
            {
                if (value.Equals(_bMruData)) return;
                _bMruData = value;
                RaisePropertyChanged(() => BMruData);
            }
        }


        /// <summary>
        /// 截止抄表值
        /// </summary>
        private double _eMruData;
        public double EMruData
        {
            get { return _eMruData; }
            set
            {
                if (value.Equals(_eMruData)) return;
                _eMruData = value;
                RaisePropertyChanged(() => EMruData);
            }
        }


        /// <summary>
        /// 起始电量
        /// </summary>
        private double _bMruTotal;
        public double BMruTotal
        {
            get { return _bMruTotal; }
            set
            {
                if (value == _bMruTotal) return;
                _bMruTotal = value;
                RaisePropertyChanged(() => BMruTotal);
            }
        }


        /// <summary>
        /// 截止电量
        /// </summary>
        private double _eMruTotal;
        public double EMruTotal
        {
            get { return _eMruTotal; }
            set
            {
                if (value == _eMruTotal) return;
                _eMruTotal = value;
                RaisePropertyChanged(() => EMruTotal);
            }
        }

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


        private string _mruAddr1;

        /// <summary>
        /// 电表地址
        /// </summary>
        public string MruAddr1
        {
            get { return _mruAddr1; }
            set
            {
                if (value != _mruAddr1)
                {
                    _mruAddr1 = value;
                    this.RaisePropertyChanged(() => this.MruAddr1);
                }
            }
        }

        private string _mruAddr2;

        /// <summary>
        /// 电表地址
        /// </summary>
        public string MruAddr2
        {
            get { return _mruAddr2; }
            set
            {
                if (value != _mruAddr2)
                {
                    _mruAddr2 = value;
                    this.RaisePropertyChanged(() => this.MruAddr2);
                }
            }
        }

        private string _mruAddr3;

        /// <summary>
        /// 电表地址
        /// </summary>
        public string MruAddr3
        {
            get { return _mruAddr3; }
            set
            {
                if (value != _mruAddr3)
                {
                    _mruAddr3 = value;
                    this.RaisePropertyChanged(() => this.MruAddr3);
                }
            }
        }


        private string _mruAddr4;

        /// <summary>
        /// 电表地址
        /// </summary>
        public string MruAddr4
        {
            get { return _mruAddr4; }
            set
            {
                if (value != _mruAddr4)
                {
                    _mruAddr4 = value;
                    this.RaisePropertyChanged(() => this.MruAddr4);
                }
            }
        }


        private string _mruAddr5;

        /// <summary>
        /// 电表地址
        /// </summary>
        public string MruAddr5
        {
            get { return _mruAddr5; }
            set
            {
                if (value != _mruAddr5)
                {
                    _mruAddr5 = value;
                    this.RaisePropertyChanged(() => this.MruAddr5);
                }
            }
        }

        private string _mruAddr6;

        /// <summary>
        /// 电表地址
        /// </summary>
        public string MruAddr6
        {
            get { return _mruAddr6; }
            set
            {
                if (value != _mruAddr6)
                {
                    _mruAddr6 = value;
                    this.RaisePropertyChanged(() => this.MruAddr6);
                }
            }
        }


        public double sacal;
        #endregion

        public MruDataRecordViewModel()
        {
            Id = 0;
            MruData = 0;
            DateCreate = "--";
            DateTypeCode = "--";
            MruTypeCode = "--";
            RtuId = 0;
            Differ = "--";
        }

        public MruDataRecordViewModel(MruDataRequest.MruDataItem info)
        {
            Id = 0;
            MruData = Math.Round(info.MruData, 2);
            DateCreate = new DateTime(info.DateCreate).ToString("yyyy-MM-dd HH:mm:ss") + "";
            Differ = "--";
            DateTypeCode = info.DateType == 0
                                    ? "当前"
                                    : info.DateType == 1 ? "上月" : "上上月";
            MruTypeCode = info.MruType == 4
                                   ? "总电量"
                                   : info.MruType == 1
                                         ? "A相"
                                         : info.MruType == 2
                                               ? "B相"
                                               : info.MruType == 3 ? "C相" : "--";
            RtuId = info.RtuId;
        }
    }
}
