using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.client;


namespace Wlst.Ux.WJ3005Module.ZDataQuery.RtuOpenCloseLightQuery.Services
{
    public class ExecuteItem : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        private int a;
        public ExecuteItem(Wlst .client .TimeTableExecuteRecord .TimeTableExecuteInfoItem   info)
        {
            a = info.AreaId;
            this.TimeTableId = info.TimeTableId;
            this.IsOpenLight = info.OpenOrClose == 1 ? "开灯" : "关灯";
            this.DateTimeGet = new DateTime(info.DateCreate).ToString("yyy-MM-dd HH:mm:ss");
            this.Lux = info.LuxValue < 0 ? "--" : info.LuxValue.ToString("f2");
        }

        #region attri

        private int _rtd;

        public int Id
        {
            get { return _rtd; }
            set
            {
                if (value != _rtd)
                {
                    _rtd = value;
                    this.RaisePropertyChanged(() => this.Id);
                }
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
                    var sss = Sr.TimeTableSystem.Services.WeekTimeTableInfoService.GeteekTimeTableInfo(a,_rtuId);
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



        private string _rtuIdggsdfgg;

        public string OneSum
        {
            get { return _rtuIdggsdfgg; }
            set
            {
                if (value != _rtuIdggsdfgg)
                {
                    _rtuIdggsdfgg = value;
                    this.RaisePropertyChanged(() => this.OneSum);
                }
            }
        }


        private string  _rtuIdsssssa;

        public string AllSum
        {
            get { return _rtuIdsssssa; }
            set
            {
                if (value != _rtuIdsssssa)
                {
                    _rtuIdsssssa = value;
                    this.RaisePropertyChanged(() => this.AllSum);
                }
            }
        }


        private string _rtuIdsss;

        public string Lux
        {
            get { return _rtuIdsss; }
            set
            {
                if (value != _rtuIdsss)
                {
                    _rtuIdsss = value;
                    this.RaisePropertyChanged(() => this.Lux);
                }
            }
        }


        #endregion
    }
}
