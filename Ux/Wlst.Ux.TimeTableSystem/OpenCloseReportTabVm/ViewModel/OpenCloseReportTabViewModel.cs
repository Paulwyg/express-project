using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.PPProtocolSvrCnt.Common;
using Wlst.Ux.TimeTableSystem.OpenCloseReportTabVm.Services;
using Wlst.client;

namespace Wlst.Ux.TimeTableSystem.OpenCloseReportTabVm.ViewModel
{
    //[Export(typeof (IIOpenCloseReportTabVm))]
    //[PartCreationPolicy(CreationPolicy.Shared)]  
    public class OpenCloseReportTabViewModel : VmEventActionProperyChangedBase, IIOpenCloseReportTabVm
    {
        public OpenCloseReportTabViewModel()
        {
            Title = "开关灯报表";
            InitAction();
        }

        private void InitAction()
        {
            this.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxRtuTime .wst_request_timetable_x_report_record , MsgAction,true);
        }


        public override void NavOnLoadr(params object[] parsObjects)
        {
            this.TimeNameItems.Clear();
            this.Records.Clear();
            //base.NavOnLoadr(parsObjects);
        }


        private ObservableCollection<NameValueInt> _timeNameItems;


        public ObservableCollection<NameValueInt> TimeNameItems
        {
            get
            {
                if (_timeNameItems == null)
                {
                    _timeNameItems = new ObservableCollection<NameValueInt>();
                }
                return _timeNameItems;
            }
        }

        protected Dictionary<int,Wlst.client.TimeTableXReportRecord .TimeTableXReportItem  > HoldInfo = new Dictionary<int,Wlst.client .TimeTableXReportRecord .TimeTableXReportItem>();

        private void MsgAction(string session,Wlst .mobile .MsgWithMobile   xgt)
        {
            
            var infos = xgt.WstRtutimeRequestTimetableXReportRecord ;
            if (infos == null) return;
            if (infos.Op != 1) return;
            foreach (var f in infos.Items)
            {
                if (HoldInfo.ContainsKey(f.TimeTableId)) HoldInfo[f.TimeTableId] = f;
                else HoldInfo.Add(f.TimeTableId, f);
            }
            //UpdateInfo(infos.Data);

            bool bolfind = false;
            foreach (var t in TimeNameItems)
            {
                if (t.Value == infos.TimeTableId)
                {
                    bolfind = true;
                    break;
                }
            }
            if (!bolfind)
            {
                TimeNameItems.Add(new NameValueInt()
                                      {Name = infos.TimeTableId + "", Value = infos.TimeTableId});
            }
            if (_currentSelectTimeTableId == 0 || _currentSelectTimeTableId == infos.TimeTableId)
            {
                _currentSelectTimeTableId = infos.TimeTableId;
                OnSelectChanged(_currentSelectTimeTableId);
            }
        }


        private int _currentSelectTimeTableId;

        public void OnSelectChanged(int timetableid)
        {

            if (HoldInfo.ContainsKey(timetableid))
            {
                _currentSelectTimeTableId = timetableid;
                UpdateInfo(HoldInfo[timetableid]);
            }
        }

        private void UpdateInfo(TimeTableXReportRecord .TimeTableXReportItem data)
        {
            if (data == null) return;

            this.DateTimeGet =new DateTime(data.DateCreate ).ToString("yyyy-MM-dd HH:mm:ss") + "";
            TimeTableId = data.TimeTableId;
            IsOpenLight = data.IsOpenLight ? "开灯" : "关灯";
            TheXtimes = data.TheXTimes ;
            //ServerInfo = data.OtherInfo;
            Sucessfuls = data.RtuLoopsSuccess ;
            Faileds = data.RtuLoopsFailed ;

            var dir = new Dictionary<int, List<Wlst .client .TimeTableXReportRecord .TimeTableXReportItem.RtuLoopItem   >>();
            foreach (var t in data.DeviceIds )
            {
                if (!dir.ContainsKey(t.RtuId)) dir.Add(t.RtuId, new List<TimeTableXReportRecord.TimeTableXReportItem.RtuLoopItem >());
                dir[t.RtuId ].Add(t);
            }

          //  this.Records.Clear();
            var tmpsss = new ObservableCollection<OpenCloseReportItem>();
            var tmps = (from t in dir orderby t.Key ascending select t).ToList();
            foreach (var t in tmps)
            {
                var sss = new OpenCloseReportItem()
                              {
                                  RtuId = t.Key
                              };
                foreach (var ggg in t.Value)
                {
                    if (ggg.LoopId  < sss.Records.Count)
                    {
                        sss.Records[ggg.LoopId ].Value = ggg.IsSuccess  ? "成功" : "失败";
                    }
                }
                tmpsss.Add(sss);
            }
            this.Records = tmpsss;
        }

        /// <summary>
        /// 1 success 2 failed  3 all
        /// </summary>
        /// <param name="succ"></param>
        private void OnCmdShowSuOrFa(int succ)
        {
            if (succ == 1)
            {
                if (HoldInfo.ContainsKey(_currentSelectTimeTableId))
                {
                    UpdateInfossssssss(HoldInfo[_currentSelectTimeTableId], true);
                }
            }
            if (succ == 2)
            {
                if (HoldInfo.ContainsKey(_currentSelectTimeTableId))
                {
                    UpdateInfossssssss(HoldInfo[_currentSelectTimeTableId], false);
                }
            }
            if (succ == 3)
            {
                OnSelectChanged(_currentSelectTimeTableId);
            }
        }

        private void UpdateInfossssssss(TimeTableXReportRecord.TimeTableXReportItem data, bool successs)
        {
            if (data == null) return;

            this.DateTimeGet =new DateTime(data.DateCreate ).ToString("yyyy-MM-dd HH:mm:ss") ;
            TimeTableId = data.TimeTableId;
            IsOpenLight = data.IsOpenLight ? "开灯" : "关灯";
            TheXtimes = data.TheXTimes ;
            //ServerInfo = data.OtherInfo;
            Sucessfuls = data.RtuLoopsSuccess ;
            Faileds = data.RtuLoopsFailed ;

            var dir = new Dictionary<int, List<Wlst.client.TimeTableXReportRecord.TimeTableXReportItem .RtuLoopItem >>();
            foreach (var t in data.DeviceIds )
            {
                if (!dir.ContainsKey(t.RtuId)) dir.Add(t.RtuId, new List<TimeTableXReportRecord.TimeTableXReportItem .RtuLoopItem >());
                dir[t.RtuId ].Add(t);
            }

            var needdelete = new List<int>();
            foreach (var t in dir)
            {
                bool same = false;
                foreach (var g in t.Value)
                {
                    if (g.IsSuccess  == successs)
                    {
                        same = true;
                        break;
                    }
                }
                if (!same) needdelete.Add(t.Key);
            }
            foreach (var t in needdelete)
                if (dir.ContainsKey(t)) dir.Remove(t);

            this.Records.Clear();

            var tmpgggg = new ObservableCollection<OpenCloseReportItem>();
            var tmps = (from t in dir orderby t.Key ascending select t).ToList();
            foreach (var t in tmps)
            {
                var sss = new OpenCloseReportItem()
                              {
                                  RtuId = t.Key
                              };
                foreach (var ggg in t.Value)
                {
                    if (ggg.LoopId  < sss.Records.Count)
                    {
                        sss.Records[ggg.LoopId ].Value = ggg.IsSuccess  ? "成功" : "失败";
                    }
                }
                tmpgggg.Add(sss);
            }
            this.Records = tmpgggg;
        }




        #region attri

        private ObservableCollection<OpenCloseReportItem> _records;

        /// <summary>
        /// 操作码+操作终端+操作回路 如222222+1+3  操作码22222 1终端3回路
        /// 操作码+操作终端+操作集合 如 22222+1+135  操作码22222 1终端1、3、5回路
        /// </summary>
        public ObservableCollection<OpenCloseReportItem> Records
        {
            get
            {
                if (_records == null) _records = new ObservableCollection<OpenCloseReportItem>();
                return _records;
            }
            set
            {
                if (_records == value) return;
                _records = value;
                this.RaisePropertyChanged(() => this.Records);
            }
        }


        private int _rtuId;

        public int TimeTableId
        {
            get { return _rtuId; }
            set
            {
                if (value != _rtuId)
                {
                    _rtuId = value;
                    this.RaisePropertyChanged(() => this.TimeTableId);
                    int areaId =Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(_rtuId);
                    var sss = Sr.TimeTableSystem.Services.WeekTimeTableInfoService.GeteekTimeTableInfo(areaId,_rtuId);
                    if (sss != null) TimeTableName = sss.TimeName;
                }
            }
        }

        private string _rtuIdgggg;

        public string TimeTableName
        {
            get { return _rtuIdgggg; }
            set
            {
                if (value != _rtuIdgggg)
                {
                    _rtuIdgggg = value;
                    this.RaisePropertyChanged(() => this.TimeTableName);
                }
            }
        }

        private int _rtuIdsss;

        public int TheXtimes
        {
            get { return _rtuIdsss; }
            set
            {
                if (value != _rtuIdsss)
                {
                    _rtuIdsss = value;
                    this.RaisePropertyChanged(() => this.TheXtimes);
                }
            }
        }

        private string _rtuName;

        public string DateTimeGet
        {
            get { return _rtuName; }
            set
            {
                if (value != _rtuName)
                {
                    _rtuName = value;
                    this.RaisePropertyChanged(() => this.DateTimeGet);
                }
            }
        }


        private string _rtuNamesss;

        public string IsOpenLight
        {
            get { return _rtuNamesss; }
            set
            {
                if (value != _rtuNamesss)
                {
                    _rtuNamesss = value;
                    this.RaisePropertyChanged(() => this.IsOpenLight);
                }
            }
        }


        private string _rtuNamesssss;

        public string ServerInfo
        {
            get { return _rtuNamesssss; }
            set
            {
                if (value != _rtuNamesssss)
                {
                    _rtuNamesssss = value;
                    this.RaisePropertyChanged(() => this.ServerInfo);
                }
            }
        }


        private int _rtuIdggsdfgg;

        public int Sucessfuls
        {
            get { return _rtuIdggsdfgg; }
            set
            {
                if (value != _rtuIdggsdfgg)
                {
                    _rtuIdggsdfgg = value;
                    this.RaisePropertyChanged(() => this.Sucessfuls);
                }
            }
        }


        private int _rtuIdsssssa;

        public int Faileds
        {
            get { return _rtuIdsssssa; }
            set
            {
                if (value != _rtuIdsssssa)
                {
                    _rtuIdsssssa = value;
                    this.RaisePropertyChanged(() => this.Faileds);
                }
            }
        }

        #endregion

        #region CmdShowSuccess

        private ICommand _cmdShowSuccess;

        public ICommand CmdShowSuccess
        {
            get
            {
                return _cmdShowSuccess ??
                       (_cmdShowSuccess = new RelayCommand(ExCmdShowSuccess, CanExCmdShowSuccess, false));
            }
        }

        private void ExCmdShowSuccess()
        {
            OnCmdShowSuOrFa(1);
        }


        private DateTime _dtAdd = DateTime.Now.AddHours(-1);

        private bool CanExCmdShowSuccess()
        {
            if (_currentSelectTimeTableId == 0) return false;
            if (!HoldInfo.ContainsKey(_currentSelectTimeTableId)) return false;
            return DateTime.Now.Ticks - _dtAdd.Ticks > 30000000;
        }

        #endregion

        #region CmdShowFailed

        private ICommand _cmdShowFail;

        public ICommand CmdShowFailed
        {
            get
            {
                return _cmdShowFail ??
                       (_cmdShowFail = new RelayCommand(ExCmdShowFailed, CanExCmdShowFailed, false));
            }
        }

        private void ExCmdShowFailed()
        {
            OnCmdShowSuOrFa(2);
        }


        private DateTime _dtCmdShowFailed = DateTime.Now.AddHours(-1);

        private bool CanExCmdShowFailed()
        {
            if (_currentSelectTimeTableId == 0) return false;
            if (!HoldInfo.ContainsKey(_currentSelectTimeTableId)) return false;
            return DateTime.Now.Ticks - _dtCmdShowFailed.Ticks > 30000000;
        }

        #endregion

        #region CmdShowAll

        private ICommand _cmdCmdShowAll;

        public ICommand CmdShowAll
        {
            get
            {
                return _cmdCmdShowAll ??
                       (_cmdCmdShowAll = new RelayCommand(ExCmdShowAll, CanExCmdShowAll, false));
            }
        }

        private void ExCmdShowAll()
        {
            OnCmdShowSuOrFa(3);
        }


        private DateTime _dtCmdShowAll = DateTime.Now.AddHours(-1);

        private bool CanExCmdShowAll()
        {
            if (_currentSelectTimeTableId == 0) return false;
            if (!HoldInfo.ContainsKey(_currentSelectTimeTableId)) return false;
            return DateTime.Now.Ticks - _dtCmdShowAll.Ticks > 30000000;
        }

        #endregion
    }
}
