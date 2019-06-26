using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using Wlst.Cr.CoreMims.TopDataInfo;
using Wlst.Ux.StateBarModule.StateBarInBottom.Services;

namespace Wlst.Ux.StateBarModule.StateBarInBottom.ViewModel
{
    //[Export(typeof (IIStateTimeViewModule))]
    //[PartCreationPolicy(CreationPolicy.Shared)]
    public class StateTimeViewModel : INotifyPropertyChanged, IIStateTimeViewModule
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private string _timeNow;
       // private Thread timerServer;

        public StateTimeViewModel()
        {
            _timeNow = DateTime.Now.ToString(CultureInfo.InvariantCulture);

            Wlst.Cr.Coreb.AsyncTask  .Qtz  .AddQtz("null", 8888, DateTime.Now.Ticks+20000000, 60, UpdateTime);



            //timerServer = new Thread(TimerServer);
            //timerServer.Start();
        }

        void UpdateTime(object obj)
        {
            if (day != DateTime.Now.Day)
            {
                day = DateTime.Now.Day;
                UpdateTOdaytime();
            }
            if (updatesucc == false) UpdateTOdaytime();
            TimeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public string TimeNow
        {
            get { return _timeNow; }
            set
            {
                if (_timeNow == value) return;
                _timeNow = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("TimeNow"));
                }
            }
        }

        private int day = 0;
        private bool updatesucc = false;

        private void TimerServer()
        {
            while (true)
            {
                try
                {
                    if (day != DateTime.Now.Day)
                    {
                        day = DateTime.Now.Day;
                        UpdateTOdaytime();
                    }
                    if (updatesucc == false) UpdateTOdaytime();
                    TimeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                catch (Exception ex)
                {

                }
                Thread.Sleep(1000);
            }
        }


        private void UpdateTOdaytime()
        {

            var info = Wlst.Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(DateTime.Now.Month,
                                                                                                  DateTime.Now.Day);
            Wlst.Sr.EquipmentInfoHolding.Services.Others.Sunrise = info.time_sunrise;

            Wlst.Sr.EquipmentInfoHolding.Services.Others.Sunset = info.time_sunset;

            string tooltips = "" + Environment.NewLine;
            if (info == null)
            {
                updatesucc = false ;
                tooltips = "更新时间: ---" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + Environment.NewLine;
                tooltips += "日出时间:无法查阅基础数据。 ";
                tooltips += Environment.NewLine;

                TopDataInfoServers.MySelf.UpdateDataInfo("日出:无法查阅", tooltips, 10);

                tooltips = "更新时间: ---" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + Environment.NewLine;
                tooltips += "日落时间:无法查阅基础数据。 ";
                tooltips += Environment.NewLine;
                TopDataInfoServers.MySelf.UpdateDataInfo("日落:无法查阅", tooltips, 9);
                return;
            }
            updatesucc = true ;
            string main = "日出:" + (info.time_sunrise/60).ToString("D2") + ":" + (info.time_sunrise%60).ToString("D2");
            tooltips = "更新时间: ---" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + Environment.NewLine;
            tooltips += "日出时间:" + (info.time_sunrise/60).ToString("D2") + ":" + (info.time_sunrise%60).ToString("D2");
            tooltips += Environment.NewLine;

            TopDataInfoServers.MySelf.UpdateDataInfo(main, tooltips, 10);

            main = "日落:" + (info.time_sunset/60).ToString("D2") + ":" + (info.time_sunset%60).ToString("D2");
            tooltips = "更新时间: ---" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + Environment.NewLine;
            tooltips += "日落时间:" + (info.time_sunset/60).ToString("D2") + ":" + (info.time_sunset%60).ToString("D2");
            tooltips += Environment.NewLine;

            TopDataInfoServers.MySelf.UpdateDataInfo(main, tooltips, 9);

        }
    }
}
