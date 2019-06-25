using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindowForWlst;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.ViewModel;

namespace Wlst.Ux.TimeTableSystem.TimeInfoMn.Views.BaseView
{
    /// <summary>
    /// AddTimeTableHelp.xaml 的交互逻辑
    /// </summary>
    public partial class AddTimeTableHelp : CustomChromeWindow
    {
        public AddTimeTableHelp()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private AddTimeTableHelpStyle frmAddTimeTableHelp = new AddTimeTableHelpStyle();
        private AddTimeTableHelpStyle FirstSet = new AddTimeTableHelpStyle();


        public void SetDataContext(int luxon, int luxoff, string luxequipment, int lightonset, int lightoffset, int luxtime)
        {
            frmAddTimeTableHelp = new AddTimeTableHelpStyle();
            frmAddTimeTableHelp.LuxOn = luxon;
            frmAddTimeTableHelp.LuxOff = luxoff;
            frmAddTimeTableHelp.LuxEquipment = luxequipment;
            frmAddTimeTableHelp.LightOnSet = lightonset;
            frmAddTimeTableHelp.LightOffSet = lightoffset;
            frmAddTimeTableHelp.LuxTime = luxtime;

            var oneDaysOnOffTime = Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo( DateTime.Now.Month, DateTime.Now.Day);
            if (oneDaysOnOffTime == null)
            {
                frmAddTimeTableHelp.SunRise = 360;
                frmAddTimeTableHelp.SunSet = 1080;
            }
            else
            {
                frmAddTimeTableHelp.SunRise = oneDaysOnOffTime.time_sunrise;
                frmAddTimeTableHelp.SunSet = oneDaysOnOffTime.time_sunset;
            }

            frmAddTimeTableHelp.TimeOn = frmAddTimeTableHelp.SunSet + frmAddTimeTableHelp.LightOnSet;
            frmAddTimeTableHelp.TimeOff = frmAddTimeTableHelp.SunRise + frmAddTimeTableHelp.LightOffSet;
            frmAddTimeTableHelp.TimeOnLight = frmAddTimeTableHelp.TimeOn - frmAddTimeTableHelp.LuxTime;
            frmAddTimeTableHelp.TimeOffLight = frmAddTimeTableHelp.TimeOff - frmAddTimeTableHelp.LuxTime;

            if (lightonset > 0) frmAddTimeTableHelp.ImageOn = "ImageHelp\\LightHelpOn+.jpg";
            else if (lightonset == 0) frmAddTimeTableHelp.ImageOn = "ImageHelp\\LightHelpOn0.jpg";
            else frmAddTimeTableHelp.ImageOn = "ImageHelp\\LightHelpOn-.jpg";

            if (lightoffset > 0) frmAddTimeTableHelp.ImageOff = "ImageHelp\\LightHelpOff+.jpg";
            else if (lightoffset == 0) frmAddTimeTableHelp.ImageOff = "ImageHelp\\LightHelpOff0.jpg";
            else frmAddTimeTableHelp.ImageOff = "ImageHelp\\LightHelpOff-.jpg";

            DataContext = frmAddTimeTableHelp;


            FirstSet = new AddTimeTableHelpStyle();
            FirstSet.LuxOn = luxon;
            FirstSet.LuxOff = luxoff;
            FirstSet.LightOnSet = lightonset;
            FirstSet.LightOffSet = lightoffset;
            FirstSet.LuxTime = luxtime;
            DockPanel.DataContext = FirstSet;
        }

        //public event EventHandler<EventArgsFrmSelectTimeTable> OnFormBtnOkClick;

        public class AddTimeTableHelpStyle : Wlst.Cr.Core.CoreServices.ObservableObject
        {
            private int _luxon;
            public int LuxOn
            {
                get
                {
                    return _luxon;
                }
                set
                {
                    if (_luxon != value)
                    {
                         if (Convert.ToInt32(value) < 0)
                            {
                                value = 15;
                            }

                        
                        _luxon = value;
                        this.RaisePropertyChanged(() => this.LuxOn);
                    }
                }
            }

            private int _luxoff;
            public int LuxOff
            {
                get
                {
                    return _luxoff;
                }
                set
                {
                    if (_luxoff != value)
                    {
                            if (Convert.ToInt32(value) < 0)
                            {
                                value = 15;
                            }
                        _luxoff = value;
                        this.RaisePropertyChanged(() => this.LuxOff);
                    }
                }
            }

            private string _luxequipment;
            public string LuxEquipment
            {
                get
                {
                    return _luxequipment;
                }
                set
                {
                    if (_luxequipment != value)
                    {
                        _luxequipment = value;
                        this.RaisePropertyChanged(() => this.LuxEquipment);
                    }
                }
            }

            private int _lightonset;
            public int LightOnSet
            {
                get
                {
                    return _lightonset;
                }
                set
                {
                    if (_lightonset != value)
                    {
                            if (Convert.ToInt32(value) < -60 || Convert.ToInt32(value) > 60)
                            {
                                value = 15;
                            }

                        _lightonset = value;
                        this.RaisePropertyChanged(() => this.LightOnSet);
                    }
                }
            }

            private int _lightoffset;
            public int LightOffSet
            {
                get
                {
                    return _lightoffset;
                }
                set
                {
                    if (_lightoffset != value)
                    {
                       
                            if (Convert.ToInt32(value) < -60 || Convert.ToInt32(value) > 60)
                            {
                                value = -15;
                            }

                        _lightoffset = value;
                        this.RaisePropertyChanged(() => this.LightOffSet);
                    }
                }
            }

            private int _luxtime;
            public int LuxTime
            {
                get
                {
                    return _luxtime;
                }
                set
                {
                    if (_luxtime != value)
                    {
                        _luxtime = value;
                        this.RaisePropertyChanged(() => this.LuxTime);
                    }
                }
            }

            private int _sunrise;
            public int SunRise
            {
                get
                {
                    return _sunrise;
                }
                set
                {
                    if (_sunrise != value)
                    {
                        _sunrise = value;
                        this.RaisePropertyChanged(() => this.SunRise);
                    }
                }
            }

            private int _sunset;
            public int SunSet
            {
                get
                {
                    return _sunset;
                }
                set
                {
                    if (_sunset != value)
                    {
                        _sunset = value;
                        this.RaisePropertyChanged(() => this.SunSet);
                    }
                }
            }

            private int _timeon;
            public int TimeOn
            {
                get
                {
                    return _timeon;
                }
                set
                {
                    if (_timeon != value)
                    {
                        _timeon = value;
                        this.RaisePropertyChanged(() => this.TimeOn);
                    }
                }
            }

            private int _timeoff;
            public int TimeOff
            {
                get
                {
                    return _timeoff;
                }
                set
                {
                    if (_timeoff != value)
                    {
                        _timeoff = value;
                        this.RaisePropertyChanged(() => this.TimeOff);
                    }
                }
            }

            private int _timeonlight;
            public int TimeOnLight
            {
                get
                {
                    return _timeonlight;
                }
                set
                {
                    if (_timeonlight != value)
                    {
                        _timeonlight = value;
                        this.RaisePropertyChanged(() => this.TimeOnLight);
                    }
                }
            }

            private int _timeofflight;
            public int TimeOffLight
            {
                get
                {
                    return _timeofflight;
                }
                set
                {
                    if (_timeofflight != value)
                    {
                        _timeofflight = value;
                        this.RaisePropertyChanged(() => this.TimeOffLight);
                    }
                }
            }

            private string _imageon;
            public string ImageOn
            {
                get
                {
                    return _imageon;
                }
                set
                {
                    if (_imageon != value)
                    {
                        _imageon = value;
                        this.RaisePropertyChanged(() => this.ImageOn);
                    }
                }
            }

            private string _imageoff;
            public string ImageOff
            {
                get
                {
                    return _imageoff;
                }
                set
                {
                    if (_imageoff != value)
                    {
                        _imageoff = value;
                        this.RaisePropertyChanged(() => this.ImageOff);
                    }
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            frmAddTimeTableHelp.LuxOn = FirstSet.LuxOn;
            frmAddTimeTableHelp.LuxOff = FirstSet.LuxOff;
            frmAddTimeTableHelp.LightOnSet = FirstSet.LightOnSet;
            frmAddTimeTableHelp.LightOffSet = FirstSet.LightOffSet;
            frmAddTimeTableHelp.LuxTime = FirstSet.LuxTime;


            if (frmAddTimeTableHelp.LightOnSet > 0) frmAddTimeTableHelp.ImageOn = "ImageHelp\\LightHelpOn+.jpg";
            else if (frmAddTimeTableHelp.LightOnSet == 0) frmAddTimeTableHelp.ImageOn = "ImageHelp\\LightHelpOn0.jpg";
            else frmAddTimeTableHelp.ImageOn = "ImageHelp\\LightHelpOn-.jpg";

            if (frmAddTimeTableHelp.LightOffSet > 0) frmAddTimeTableHelp.ImageOff = "ImageHelp\\LightHelpOff+.jpg";
            else if (frmAddTimeTableHelp.LightOffSet == 0) frmAddTimeTableHelp.ImageOff = "ImageHelp\\LightHelpOff0.jpg";
            else frmAddTimeTableHelp.ImageOff = "ImageHelp\\LightHelpOff-.jpg";
            
        }

        private void CustomChromeWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

    }
}
