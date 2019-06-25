using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.Wj2096Module.TimeInfoSet.Services;

namespace Wlst.Ux.Wj2096Module.TimeInfoSet.ViewModel
{
    [Export(typeof(IISetYearTime))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class SetYearTimeVm : EventHandlerHelperExtendNotifyProperyChanged,IISetYearTime
    {
        #region IITab

        public int Index
        {
            get { return 1; }
        }

        public string Title
        {
            get { return "设置全年时间"; }
        }

        public bool CanClose
        {
            get { return true; }
        }

        public bool CanUserPin
        {
            get { return true; }
        }

        public bool CanFloat
        {
            get { return true; }
        }

        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        #endregion

        //界面是否开启
        private bool _isViewShow;
        public SetYearTimeVm()
        {
            
        }

        public void OnUserHideOrClosing()
        {
            _isViewShow = false;
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            _isViewShow = true;
            if (parsObjects.Count() == 0) return;
            var yearTime = parsObjects[0] as TimeInfoOneVm;
            if(yearTime==null) return;
            var week = new List<int>();
            for (int i = 0; i < yearTime.OperationWeekSet.Count; i++)
            {
                if (yearTime.OperationWeekSet[i].IsSelected)
                {
                    week.Add(i);
                }
            }

            InitSun(week, yearTime.OperatorAboutTime, yearTime.OperationArguOffset);
        }

        private void InitSun(List<int> week, string operatorTime,int offset)
        {
            RecordSun.Clear();


            for (int j = 1; j < 32; j++)
            {
                SunItem tmp = new SunItem();
                tmp.Records.Add(new NameValueInt() { Name = j + " " });
                for (int i = 1; i < 13; i++)
                {
                    if ((DateTime.Now.Year % 4 == 0 && DateTime.Now.Year % 100 != 0) || DateTime.Now.Year % 400 == 0)
                    {
                    }
                    else
                    {
                        if (j == 29 && i == 2)
                        {
                            tmp.Records.Add(new NameValueInt()
                            {
                                Name = "--:--"
                            });
                            continue;
                        }
                    }
                        var info = Wlst.Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(i, j);
                        if (info != null)
                        {
                            var x = new DateTime(DateTime.Now.Year, i, j).DayOfWeek.ToString();
                            if (week.Contains(Week(x)))
                                tmp.Records.Add(new NameValueInt()
                                                    {
                                                        Name =
                                                            operatorTime.Contains("日出")
                                                                ? ((info.time_sunrise + offset)/60).ToString("D2") + ":" +
                                                                  ((info.time_sunrise + offset)%60).ToString("D2")
                                                                : operatorTime.Contains("日落")
                                                                      ? ((info.time_sunset + offset)/60).ToString("D2") +
                                                                        ":" +
                                                                        ((info.time_sunset + offset)%60).ToString("D2")
                                                                      : operatorTime
                                                    });
                            else
                            {
                                tmp.Records.Add(new NameValueInt()
                                                    {
                                                        Name = "--:--"
                                                    });
                            }
                        }
                        else
                        {
                            tmp.Records.Add(new NameValueInt()
                                                {
                                                    Name = "--:--"
                                                });
                        }

                   
                }
                RecordSun.Add(tmp);

            }

        }

        public int Week(string weekName)
        {
            int week = 0;
            switch (weekName)
            {
                case "Sunday":
                    week = 6;
                    break;
                case "Monday":
                    week = 0;
                    break;
                case "Tuesday":
                    week = 1;
                    break;
                case "Wednesday":
                    week = 2;
                    break;
                case "Thursday":
                    week = 3;
                    break;
                case "Friday":
                    week = 4;
                    break;
                case "Saturday":
                    week = 5;
                    break;
            }
            return week;
        }

        #region Records

        private ObservableCollection<SunItem> _recordSun;

        public ObservableCollection<SunItem> RecordSun
        {
            get
            {

                if (_recordSun == null)
                {
                    _recordSun = new ObservableCollection<SunItem>();
                }
                return _recordSun;
            }
        }

        #endregion

     
    }

    public class SunItem
    {
        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> _records;

        public ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> Records
        {
            get
            {
                if (_records == null)
                    _records = new ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt>();
                return _records;
            }
        }
    }
}
