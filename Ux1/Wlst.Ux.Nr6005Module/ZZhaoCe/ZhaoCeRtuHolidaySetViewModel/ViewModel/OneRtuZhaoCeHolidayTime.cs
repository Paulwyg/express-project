using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;
using Wlst.client;


namespace Wlst.Ux.Nr6005Module.ZZhaoCe.ZhaoCeRtuHolidaySetViewModel.ViewModel
{
    public class OneRtuZhaoCeHolidayTime : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        public OneRtuZhaoCeHolidayTime(int rtuId, List<Wlst.client.HolidaySchduleTimeItem> zhaoCe, int op)
        {
            this.RtuId = rtuId;
            int index = 1;
            if (op == 2) index = 5;
            foreach (var t in zhaoCe)
            {
                this.HolidayTime.Add(new TimeSchduleItemItme(t) {Id = index});
                index++;
            }
            DateTimeRecevie = DateTime.Now;

        }

        public void AddHolidayData(int rtuId, List<Wlst.client.HolidaySchduleTimeItem> zhaoCe,int op)
        {
            if (RtuId != rtuId) return;
            int index = 1;
            if (op  == 2) index = 5;

            foreach (var t in zhaoCe)
            {
                this.HolidayTime.Add(new TimeSchduleItemItme(t) {Id = index});
                index++;
            }
        }

        #region gen

        private int _rtuId;

        /// <summary>
        /// 终端地址
        /// </summary>
        public int RtuId
        {
            get { return _rtuId; }
            set
            {
                if (_rtuId != value)
                {
                    _rtuId = value;
                    PhyId = value;
                    this.RaisePropertyChanged(() => this.RtuId);
                    if (
                        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .
                            InfoItems  .
                            ContainsKey(this.RtuId))
                    {
                        var rtuInfomation =
                            Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .
                                InfoItems  [this.RtuId];
                        if (rtuInfomation != null)
                        {
                            this.RtuName = rtuInfomation.RtuName;
                            PhyId = rtuInfomation.RtuPhyId ;
                            if (rtuInfomation.RtuModel == EnumRtuModel.Wj3005 || rtuInfomation.RtuModel == EnumRtuModel.Wj3090 || rtuInfomation.RtuModel == EnumRtuModel.Gz6005) this.RtuType8 = false;
                            else this.RtuType8 = true;
                        }
                    }
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
        //private int _rtutype6;

        ///// <summary>
        ///// 是否支持6路
        ///// </summary>
        //public int RtuType6
        //{
        //    get { return _rtutype6; }
        //    set
        //    {
        //        if (_rtutype6 != value)
        //        {
        //            _rtutype6 = value;
        //            this.RaisePropertyChanged(() => this.RtuType6);
        //        }
        //    }
        //}

        private bool _rtutype8;

        /// <summary>
        /// 是否支持8路
        /// </summary>
        public bool RtuType8
        {
            get { return _rtutype8; }
            set
            {
                if (_rtutype8 != value)
                {
                    _rtutype8 = value;
                    this.RaisePropertyChanged(() => this.RtuType8);
                }
            }
        }

        private string _rtuName;

        /// <summary>
        /// 终端名称
        /// </summary>
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



        private DateTime _datatime;

        /// <summary>
        /// 接收时间  
        /// </summary>
        public DateTime DateTimeRecevie
        {
            get { return _datatime; }
            set
            {
                if (_datatime != value)
                {
                    _datatime = value;
                    this.RaisePropertyChanged(() => this.DateTimeRecevie);
                }
            }
        }
        #endregion

        #region delete current
        private ICommand _deleteCurrentCommand;

        public ICommand DeleteCurrentCommand
        {
            get
            {
                if (_deleteCurrentCommand == null)
                    _deleteCurrentCommand = new RelayCommand(ExDelete, CanExDelete,false );
                return _deleteCurrentCommand;
            }
        }

        private bool CanExDelete()
        {
            return this.HolidayTime.Count > 0;
        }

        private void ExDelete()
        {
            this.HolidayTime.Clear();
        }
        #endregion

        private ObservableCollection<TimeSchduleItemItme> _weekTime;

        public ObservableCollection<TimeSchduleItemItme> HolidayTime
        {
            get
            {
                if (_weekTime == null)
                    _weekTime = new ObservableCollection<TimeSchduleItemItme>();
                return _weekTime;
            }
        }
    }
}
