using System.Collections.Generic;
using System.Collections.ObjectModel;
using Wlst.Cr.CoreOne.Models;
using Wlst.client;

namespace Wlst.Ux.Wj9001Module.Wj9001ParaInfoSet.ViewModel
{
    public class Wj9001Model : Cr.Core.CoreServices.ObservableObject
    {
        #region ZcItems

        private ObservableCollection<Wj9001Model> _zcItems;
        public ObservableCollection<Wj9001Model> ZcItems
        {
            get { return _zcItems ?? (_zcItems = new ObservableCollection<Wj9001Model>()); }
        }
        #endregion

        #region DataType

        private string _dataType;
        public string DataType
        {
            get { return _dataType; }
            set
            {
                if(_dataType==value) return;
                _dataType = value;
                RaisePropertyChanged(()=>DataType);
            }
        }
        #endregion

        #region LeakLineId 线路序号
        private int _leakLineId;
        public int LeakLineId
        {
            get { return _leakLineId; }
            set
            {
                if (_leakLineId == value) return;
                _leakLineId = value;
                RaisePropertyChanged(() => LeakLineId);
            }
        }

        #endregion

        #region LeakId 设备虚拟地址
        private int _leakId;
        public int LeakId
        {
            get { return _leakId; }
            set
            {
                if (_leakId == value) return;
                _leakId = value;
                RaisePropertyChanged(() => LeakId);
            }
        }

        #endregion

        #region AutoBreakOrAutoAlarm 自动分闸 自动报警
        private int _autoBreakOrAutoAlarm;
        public int AutoBreakOrAutoAlarm
        {
            get { return _autoBreakOrAutoAlarm; }
            set
            {
                if (_autoBreakOrAutoAlarm == value) return;
                _autoBreakOrAutoAlarm = value;
                RaisePropertyChanged(() => AutoBreakOrAutoAlarm);
            }
        }

        #endregion

        #region LeakCommTypeCode 通信方式
        private int _leakCommTypeCode;
        public int LeakCommTypeCode
        {
            get { return _leakCommTypeCode; }
            set
            {
                if (_leakCommTypeCode == value) return;
                _leakCommTypeCode = value;
                RaisePropertyChanged(() => LeakCommTypeCode);
            }
        }

        #endregion

        #region LeakMode 漏电模式
        private int _leakMode;
        public int LeakMode
        {
            get { return _leakMode; }
            set
            {
                if (_leakMode == value) return;
                _leakMode = value;
                RaisePropertyChanged(() => LeakMode);
            }
        }

        #endregion

        #region TimeDelayforBreak 延迟时间
        private int _timeDelayforBreak;
        public int TimeDelayforBreak
        {
            get { return _timeDelayforBreak; }
            set
            {
                if (_timeDelayforBreak == value) return;
                _timeDelayforBreak = value;
                RaisePropertyChanged(() => TimeDelayforBreak);
            }
        }

        #endregion

        #region UpperAlarmOrBreakforLeakOrTemperature 报警上下限
        private int _upperAlarmOrBreakforLeakOrTemperature;
        public int UpperAlarmOrBreakforLeakOrTemperature
        {
            get { return _upperAlarmOrBreakforLeakOrTemperature; }
            set
            {
                if (_upperAlarmOrBreakforLeakOrTemperature == value) return;
                _upperAlarmOrBreakforLeakOrTemperature = value;
                RaisePropertyChanged(() => UpperAlarmOrBreakforLeakOrTemperature);
            }
        }

        #endregion

        #region LeakEndLampportSn 末端备用序号
        private string _leakEndLampportSn;
        public string LeakEndLampportSn
        {
            get { return _leakEndLampportSn; }
            set
            {
                if (_leakEndLampportSn != value)
                {
                    _leakEndLampportSn = value;
                    RaisePropertyChanged(() => LeakEndLampportSn);
                }
            }
        }
        #endregion 

        #region 备注
        private string _remark;
        public string Remark
        {
            get { return _remark; }
            set
            {
                if (_remark != value)
                {
                    _remark = value;
                    RaisePropertyChanged(() => Remark);
                }
            }
        }
        #endregion 

        #region IsEdit 编辑
        private bool _isEdit;
        public bool IsEdit
        {
            get { return _isEdit; }
            set
            {
                if (value == _isEdit) return;
                _isEdit = value;
                RaisePropertyChanged(() => IsEdit);
            }
        }
        #endregion

        #region 使用 IsUsed
        private bool _isUsed;
        public bool IsUsed
        {
            get { return _isUsed; }
            set
            {
                if (_isUsed == value) return;
                _isUsed = value;
                RaisePropertyChanged(() => IsUsed);
            }
        }
        #endregion

        #region LineName 线路名称
        private string _lineName;
        public string LineName
        {
            get { return _lineName; }
            set
            {
                if (_lineName != value)
                {
                    _lineName = value;
                    RaisePropertyChanged(() => LineName);
                }
            }
        }
        #endregion 

        #region 序号

        private int _index;
        public int Index
        {
            get { return _index; }
            set
            {
                if (value != _index)
                {

                    _index = value;
                    RaisePropertyChanged(() => Index);
                }
            }
        }


        #endregion

        private readonly LeakParameter _inmodel;
        public Wj9001Model(LeakParameter cnt)//List<NameValueInt> list
        {
            //foreach (var item in list)
            //{
            //    LoopCollection.Add(new NameValueInt { Name = item.Name, Value = item.Value });
            //}
            //LoopCollection.Add(new NameValueInt{Name = "请选择回路",Value = 0});
            _inmodel = cnt;
            AutoBreakOrAutoAlarm = cnt.AutoBreakOrAutoAlarm;//自动分闸
            IsUsed =cnt.IsUsed;  //是否使用
            LeakCommTypeCode= cnt.LeakCommTypeCode;
            LeakEndLampportSn=cnt.LeakEndLampportSn;
            LeakId = cnt.LeakId;
            LeakLineId = cnt.LeakLineId;
            LeakMode=cnt.LeakMode;
            LineName= cnt.LineName; 
            Remark = cnt.Remark;
            TimeDelayforBreak = cnt.TimeDelayforBreak;
            UpperAlarmOrBreakforLeakOrTemperature = cnt.UpperAlarmOrBreakforLeakOrTemperature;
            IsEdit = false;
        }

        public LeakParameter BackToLeakLineParameter()
        {
            if (_inmodel == null) return null;
            var info = new LeakParameter
                           {
                                AutoBreakOrAutoAlarm = AutoBreakOrAutoAlarm==0?2:1,//1自动分闸  2仅报警
                                IsUsed =IsUsed,  //是否使用
                                LeakCommTypeCode= LeakCommTypeCode,
                                LeakEndLampportSn=LeakEndLampportSn,
                                LeakId = LeakId,
                                LeakLineId = LeakLineId,
                                LeakMode=LeakMode,//前4个回路是漏电模式  后四个是温度模式
                                LineName= LineName, 
                                Remark =Remark,
                                TimeDelayforBreak = TimeDelayforBreak,
                                UpperAlarmOrBreakforLeakOrTemperature = UpperAlarmOrBreakforLeakOrTemperature

                           };
            return info;
        }

        //#region 回路序号  本线路检测设备检测的终端回路的回路序号 ,,,,,,,
        //private int _lduLoopID;
        //public int LduLoopID
        //{
        //    get { return _lduLoopID; }
        //    set
        //    {
        //        if (_lduLoopID != value)
        //        {
        //            _lduLoopID = value;
        //            RaisePropertyChanged("LduLoopID");
        //        }
        //        foreach (var item in LoopCollection)
        //        {
        //            if (value != item.Value)
        //                continue;
        //            SelectedLoopVlue = item;
        //        }
        //    }
        //}
        //#endregion 

        //private NameValueInt _selectLoopValue;
        //public NameValueInt SelectedLoopVlue
        //{
        //    get { return _selectLoopValue ?? (_selectLoopValue = new NameValueInt()); }
        //    set
        //    {
        //        if(value  !=SelectedLoopVlue)
        //        {
        //            _selectLoopValue = value;
        //            RaisePropertyChanged("SelectedLoopVlue");
        //            LduLoopID = _selectLoopValue.Value;
                    
        //        }
        //    }
        //}

        //private ObservableCollection<NameValueInt> _loopCollection;
        //public ObservableCollection<NameValueInt> LoopCollection
        //{
        //    get { return _loopCollection ?? (_loopCollection = new ObservableCollection<NameValueInt>()); }
        //}
    }
}
