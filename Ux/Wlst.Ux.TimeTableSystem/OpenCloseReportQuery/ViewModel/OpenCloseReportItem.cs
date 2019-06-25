using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Wlst.Ux.TimeTableSystem.OpenCloseReportQuery.ViewModel
{


    public class OpenCloseReportItem : Wlst.Cr.Core.CoreServices.ObservableObject
    {

        public OpenCloseReportItem (int areaid, Wlst.client .TimeTableXReportRecord  .TimeTableXReportItem   info)
        {
            this.TimeTableId = info.TimeTableId;
            var tu = new Tuple<int, int>(areaid, info.TimeTableId);
            if (Wlst.Sr.TimeTableSystem.Services.WeekTimeTableInfoService.WeekTimeTableInfoDictionary.ContainsKey(tu))
            {
                this.TimeTableName =
                Wlst.Sr.TimeTableSystem.Services.WeekTimeTableInfoService.WeekTimeTableInfoDictionary[tu].TimeName;
            }
            this.TheXtimes = info.TheXTimes;
            this.Sucessfuls = info.RtuLoopsSuccess;
            this.Faileds = info.RtuLoopsFailed;
            this.IsOpenLight = info.IsOpenLight?"开灯":"关灯";
            this.DateTimeGet = new DateTime(info.DateCreate).ToString("dd HH:mm:ss") + "";
            this .UpdateRtuLoops(info .DeviceIds ,info .DeviceIdsSuccess );

            var edit = 0;
            if (this.TheXtimes == 1) edit = -5;
            else if (this.TheXtimes == 2) edit = -8;
            else if (this.TheXtimes == 3) edit = -10;

            this.DateTimeGetX = new DateTime(info.DateCreate).AddMinutes(edit).ToString("dd HH:mm") + ":03";
        }

        void UpdateRtuLoops(List<Wlst.client.TimeTableXReportRecord.TimeTableXReportItem.RtuLoopItem> info, List<Wlst.client.TimeTableXReportRecord.TimeTableXReportItem .RtuLoopItem > infosucc)
        {
            var dir = new Dictionary<int, List<int>>();
            var dirsucc = new Dictionary<int, List<int>>();
            foreach (var t in info)
            {
                if (!dir.ContainsKey(t.RtuId )) dir.Add(t.RtuId , new List<int>());
                if (!dir[t.RtuId ].Contains(t.LoopId )) dir[t.RtuId ].Add(t.LoopId );
            }
            foreach (var t in infosucc)
            {
                if (!dirsucc.ContainsKey(t.RtuId )) dirsucc.Add(t.RtuId , new List<int>());
                if (!dirsucc[t.RtuId ].Contains(t.LoopId )) dirsucc[t.RtuId ].Add(t.LoopId );
            }
            var lst = dir.Keys.ToList();
            foreach (var t in dirsucc.Keys) if (!lst.Contains(t)) lst.Add(t);
            var tmps = (from t in lst orderby t ascending select t).ToList();

            // var tmps = (from t in dir orderby t.Key select t).ToList();
            foreach (var t in tmps)
            {
                var ggg = new OpenCloseReportRtuItem()
                              {
                                  RtuId = t
                              };
                if (dir.ContainsKey(t))
                {
                    foreach (var g in dir[t])
                    {
                        if (ggg.Records.Count > g)
                        {
                            ggg.Records[g].Value = "失败";
                        }
                    }
                }
                if (dirsucc.ContainsKey(t))
                {

                    foreach (var g in dirsucc[t])
                    {
                        if (ggg.Records.Count > g)
                        {
                            ggg.Records[g].Value = "成功";
                        }
                    }
                }

                this.Records.Add(ggg);
            }
        }

        #region attri
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
                    //int areaId = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoRtuBelong[_rtuId].Item1;
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

        private string _datetimegetx;

        public string DateTimeGetX
        {
            get { return _datetimegetx; }
            set
            {
                if (value != _datetimegetx)
                {
                    _datetimegetx = value;
                    this.RaisePropertyChanged(() => this.DateTimeGetX);
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

        private ObservableCollection<OpenCloseReportRtuItem> _records;

        public ObservableCollection<OpenCloseReportRtuItem> Records
        {
            get
            {
                if (_records == null) _records = new ObservableCollection<OpenCloseReportRtuItem>();
                return _records;
            }
        }
        #endregion

    };
}
