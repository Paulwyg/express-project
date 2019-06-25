using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;
using Wlst.client;

namespace Wlst.Ux.WJ3005Module.ZZhaoCe.ZhaoCeRtuWeekSetViewModel.ViewModel
{
    public class OneRtuZhaoCeTime : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        public OneRtuZhaoCeTime()
        {
            
        }

        public OneRtuZhaoCeTime(int rtuId, List<Wlst.client.ZhaoCeInfo.ZhaoCeOneLoopOneWeekTime> zhaoCe,Dictionary<int,TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem> timeTableItem,bool flg)
        {
            this.RtuId = rtuId;
            
            var para = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuId];

            var high = (int)Elysium.ThemesSet.FontSet.FontAttriXaml.RowHeightt * 7 + (int)Elysium.ThemesSet.FontSet.FontAttriXaml.RowHeadHeightt;
            if (para.RtuModel==EnumRtuModel.Wj3090)
            {
                GridVisibility = new List<string>() { "0", "0" };
            }
            else if (para.RtuModel == EnumRtuModel.Wj3005)
            {
                GridVisibility = new List<string>() { "200", "0" };
            }
            else if (para.RtuModel == EnumRtuModel.Wj3006)
            {
                GridVisibility = new List<string>() { "200", "200" };
            }


            var zhaoce = (from t in zhaoCe orderby t.LoopId select t);
            foreach (var t in zhaoce)
            {
                if (timeTableItem.ContainsKey(t.LoopId))
                {
                    this.WeekTime.Add(new OneLoopOneWeekTimeViewModel(t, timeTableItem[t.LoopId], flg));
                }
                else
                {
                    this.WeekTime.Add(new OneLoopOneWeekTimeViewModel(t, new TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem(), flg));//??????
                }
                
            }
            

        }

        public OneRtuZhaoCeTime(int rtuId, List<List<List<Wlst.client.ZhaoCeInfo.ZhaoCeWeekSetYear>>> timeyear, int maxsection,Dictionary<int,TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem> timeTableItem)
        {
            this.RtuId = rtuId;
            var weektime = new ObservableCollection<OneLoopOneWeekTimeViewModel>();
            DateTimeRecevie = DateTime.Now;

            for (int i = 0; i < 8;i++ )
            {
                if (timeTableItem.ContainsKey(i+1))
                {
                    this.WeekTime.Add(new OneLoopOneWeekTimeViewModel(timeyear[i], maxsection, timeTableItem[i+1],i));
                }
                else
                {
                    this.WeekTime.Add(new OneLoopOneWeekTimeViewModel(timeyear[i], maxsection, new TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem(),i));
                }

            }

        }

        //public OneRtuZhaoCeTime(int rtuId, ZhaoCeK4K6 zhaoCe)
        //{
        //    this.RtuId = rtuId;
        //    foreach (var t in zhaoCe.K4K6OpenCloseTime)
        //    {
        //        this.WeekTime.Add(new OneLoopOneWeekTimeViewModel(t));
        //    }
        //    DateTimeRecevie = DateTime.Now;
        //}

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

        private List<string> _gridvisibility;

        public List<string> GridVisibility
        {
            get { return _gridvisibility; }
            set
            {
                if (_gridvisibility != value)
                {
                    _gridvisibility = value;
                    this.RaisePropertyChanged(() => this.GridVisibility);
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
            return this.WeekTime.Count > 0;
        }

        private void ExDelete()
        {
            this.WeekTime.Clear();
        }
        #endregion

        private ObservableCollection<OneLoopOneWeekTimeViewModel> _weekTime;

        public ObservableCollection<OneLoopOneWeekTimeViewModel> WeekTime
        {
            get
            {
                if (_weekTime == null)
                    _weekTime = new ObservableCollection<OneLoopOneWeekTimeViewModel>();

                return _weekTime;
            }
        }
    }
}
