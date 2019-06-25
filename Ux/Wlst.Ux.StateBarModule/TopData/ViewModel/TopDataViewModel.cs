using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreMims.TopDataInfo;
using Wlst.Cr.CoreOne.Models;
using Wlst.Sr.TimeTableSystem.Services;
using Wlst.Ux.StateBarModule.TopData.Services;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.ViewModel;

namespace Wlst.Ux.StateBarModule.TopData.ViewModel
{
    [Export(typeof(IITopDataViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class TopDataViewModel : Wlst.Cr.Core.CoreServices.ObservableObject, IITopDataViewModel
    {

        public TopDataViewModel()
        {
            InitAction();

            TopDataInfoServers.MySelf.OnTopDataChanged +=
                new EventHandler(MySelf_OnTopDataChanged);


            //如果本模块最后加载  则直接读取数据显示  否显示内容为空
            MySelf_OnTopDataChanged(this, EventArgs.Empty);


            ShowWarning = Visibility.Collapsed;
            LuxWarning = "";
            IsLuxVisi = Visibility.Collapsed;
            //Is1080ShowTopRight = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2101, 1, false);

            GetTimeTableParameter();
            if (LuxEffective == int.MinValue)
            {
                LuxEffective = 60;
                LightOnOffSet = 0;
                LightOffOffSet = 0;
            }

            Wlst.Cr.Coreb.Servers.QtzLp.AddQtz("null", 8888, DateTime.Now.Ticks, 1, UpdateTime);
        }

        private bool Is1080ShowTopRight = false;
        private void MySelf_OnTopDataChanged(object sender, EventArgs e)
        {

            for (int i = 0; i < 1; i++)
            {
                try
                {
                    var info = TopDataInfoServers.MySelf.GetDataInfo(i);
                    if (info == null) return;
                    //if (!string.IsNullOrEmpty(info.Item1) || !string.IsNullOrEmpty(info.Item2))
                    //{

                    //var sps = info.Item1.Split(':', '：');
                    //if (sps.Length < 2) return;


                    Items[i].Name = info.Item1;// sps[1];
                    Items[i].Value = info.Item2;
                    Items[i].margin = "5,0,5,0";
                    //}
                }
                catch (Exception ex)
                {

                }
            }
        }


        private ObservableCollection<TimeTableInfomationItem> _items;

        public ObservableCollection<TimeTableInfomationItem> TimeItems
        {
            get { return _items ?? (_items = new ObservableCollection<TimeTableInfomationItem>()); }
            set
            {
                if (value == _items) return;
                _items = value;
                this.RaisePropertyChanged(() => Items);
            }
        }

        private void GetTimeTableParameter()
        {
            TimeItems.Clear();

            foreach (var itemTable in WeekTimeTableInfoService.GeteekTimeTableInfoList(0))
            {
                TimeItems.Add(new TimeTableInfomationItem(itemTable, 0));
            }

            LuxEffective = int.MinValue;
            foreach (var tt in TimeItems)
            {
                if (tt.LuxId == CurrentSelectLux)
                {
                    bool flg = false;
                    foreach (var t in tt.RuleItems)
                    {
                        if (t.IsUsedLuxOn && t.IsUsedLuxOff)
                        {
                            flg = true;
                        }
                        else
                        {
                            flg = false;
                        }
                    }
                    if (!flg) continue;

                    LuxEffective = tt.LuxEffective;
                    LightOnOffSet = tt.LightOnOffset;
                    LightOffOffSet = tt.LightOffOffset;

                    break;
                }
            }
        }




        public bool IsShowWarning;
        public int LuxEffective;
        public int LightOnOffSet;
        public int SunSetAlarmValue;
        public int CurrentSelectLux;
        public double LuxValue;
        public bool IsSunSetSpeechWarning;
        private bool blLuxLowHighFirst = false;
        public int LuxOffValue;
        public bool IsTrunOnWarning;
        private bool blTurnOnOffFirst = false;
        public int SunSetOffSet;
        public int LightOffOffSet;
        public int SunRiseAlarmValue; 
        public bool IsSunRiseSpeechWarning;
        public int LuxOnValue;
        public bool IsTrunOffWarning;
        public int SunRiseOffSet;

        public bool IsShowOpenClose;
        public int SunOpenValue;
        public int SunCloseValue;

        void UpdateTime(object obj)
        {
            try
            {
                UpdateTime2();
            }
            catch (Exception ex)
            {

            }
        }

        void UpdateTime2()
        {
            Is1080ShowTopRight = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2101, 1, false);
            if (!Is1080ShowTopRight)
            {
                IsLuxVisi = Visibility.Collapsed;
                LuxWarning = "";
                return;
            }

            CurrentSelectLux = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2101, 2, 0);

            IsShowWarning = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2101, 3, false);
            ShowWarning = IsShowWarning ? Visibility.Visible : Visibility.Collapsed;

            //IsSunSetSpeechWarning = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2101, 5, false);

            SunSetAlarmValue = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2101, 8, 0);

            //IsTrunOnWarning = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2101, 10, false);
            //IsTrunOffWarning = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2101, 11, false);

            //IsShowOpenClose = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2101, 12, false);

            //SunOpenValue = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2101, 13, 0);
            //SunCloseValue = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2101, 14, 0);

            //if (DateTime.Now.Hour == 14 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 0)
            //{
            //    GetTimeTableParameter();
            //    if (LuxEffective == int.MinValue)
            //    {
            //        LuxEffective = 60;
            //        LightOnOffSet = 0;
            //        LightOffOffSet = 0;
            //    }
            //}

            var infox = TopDataInfoServers.MySelf.GetDataInfo(1);

            if (infox != null)
            {
                try
                {
                    var sps = infox.Item1.Split(':', '：');
                    if (sps.Length > 1)
                    {
                        string LuxInfo = infox.Item2;
                        string[] sp = new string[] { "\r\n" };
                        string[] spstring = LuxInfo.Split(sp, StringSplitOptions.None);
                        if (CurrentSelectLux == 0)
                        {
                            try
                            {
                                if (sps.Length > 1)
                                {
                                    if (Double.TryParse(sps[1], out LuxValue))
                                    {
                                        IsLuxVisi = Visibility.Visible;
                                        LuxValuex = LuxValue + "";
                                        LuxTooltips = infox.Item2;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {
                            if (
                                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(
                                    CurrentSelectLux))
                            {
                                var tm =
                                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[
                                        CurrentSelectLux] as Wlst.Sr.EquipmentInfoHolding.Model.Wj1080Lux;

                                for (int i = 0; i < spstring.Length; i++)
                                {
                                    if (spstring[i].Contains("物理地址") == true)
                                    {
                                        var sps1 = spstring[i].Split(':', '：');

                                        if (Convert.ToInt32(sps1[1]) == tm.RtuPhyId)
                                        {
                                            var sps2 = spstring[i + 2].Split(':', '：', ' ', '-');

                                            for (int j = 0; j < sps2.Length; j++)
                                            {
                                                if (sps2[j] != "")
                                                {
                                                    if (sps2[j].Contains("光照度") == false)
                                                    {
                                                        LuxValue = Convert.ToDouble(sps2[j]);
                                                        break;
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        IsLuxVisi = Visibility.Visible;
                        LuxValuex = LuxValue + "";
                        LuxTooltips = infox.Item2;

                        if (IsShowWarning == true)
                        {
                            //GetTimeTableParameter();
                            //if (LuxEffective == int.MinValue)
                            //{
                            //    LuxEffective = 60;
                            //    LightOnOffSet = 0;
                            //    LightOffOffSet = 0;
                            //}

                            var info =
                                Wlst.Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(
                                DateTime.Now.Month, DateTime.Now.Day);
                            int _dtNow = DateTime.Now.Minute + DateTime.Now.Hour * 60;

                            if ((_dtNow <= info.time_sunset + LightOnOffSet - LuxEffective) &&
                                (_dtNow >= info.time_sunrise + LightOffOffSet + LuxEffective))
                            {
                                if (LuxValue <= SunSetAlarmValue)
                                {
                                    LuxWarning = "光照度低";
                                }
                                else
                                {
                                    LuxWarning = "";
                                }
                            }
                            else
                            {
                                LuxWarning = "";
                            }
                        }
                        else
                        {
                            LuxWarning = "";
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void InitAction()
        {
            ProtocolServer.RegistProtocol(Sr.ProtocolPhone.LxRtuTime.wst_timetable_set,
                              ExSaveWeek, typeof(TopDataViewModel), this, true);
        }

        private void ExSaveWeek(string session, Wlst.mobile.MsgWithMobile infos)
        {
            GetTimeTableParameter();
            if (LuxEffective == int.MinValue)
            {
                LuxEffective = 60;
                LightOnOffSet = 0;
                LightOffOffSet = 0;
            }
        }



        private ObservableCollection<NameValue> _itemsdata;
        public ObservableCollection<NameValue> Items
        {
            get
            {
                if (_itemsdata == null)
                {
                    _itemsdata = new ObservableCollection<NameValue>();
                    for (int i = 0; i < 15; i++)
                    {
                        _itemsdata.Add(new NameValue() {Name = null, Value = null, margin = "0,0,0,0"});
                    }
                }
                return _itemsdata;
            }
        }

        private string _luxWarning;

        public string LuxWarning
        {
            get { return _luxWarning; }
            set
            {
                if (_luxWarning == value) return;
                _luxWarning = value;
                this.RaisePropertyChanged(() => this.LuxWarning);

            }
        }

        private Visibility _isShowWarning;

        public Visibility ShowWarning
        {
            get { return _isShowWarning; }
            set
            {
                if (value == _isShowWarning) return;
                _isShowWarning = value;
                this.RaisePropertyChanged(() => this.ShowWarning);
            }
        }


        private Visibility sdfIsLuxVisi;

        public Visibility IsLuxVisi
        {
            get { return sdfIsLuxVisi; }
            set
            {
                if (value == sdfIsLuxVisi) return;

                sdfIsLuxVisi = value;
                this.RaisePropertyChanged(() => this.IsLuxVisi);
            }
        }


        private string _luxValuex;
        public string LuxValuex
        {
            get { return _luxValuex; }
            set
            {
                if (_luxValuex == value) return;
                _luxValuex = value;
                this.RaisePropertyChanged(() => this.LuxValuex);
            }
        }

        private string _luxTooltips;
        public string LuxTooltips
        {
            get { return _luxTooltips; }
            set
            {
                if (_luxTooltips == value) return;
                _luxTooltips = value;
                this.RaisePropertyChanged(() => this.LuxTooltips);
            }
        }
    }

    public class NameValue : NameValueString
    {
        private string _mr;

        public string margin
        {
            get
            {
                if (string.IsNullOrEmpty(_mr)) _mr = "0,0,0,0";
                return _mr;
            }
            set
            {
                if (value == _mr) return;
                _mr = value;
                this.RaisePropertyChanged(() => this.margin);
            }
        }
    }


}