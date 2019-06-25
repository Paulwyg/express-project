using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.Services;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.ViewModel;
using Wlst.Ux.Wj1080Module.Wj1080ManageSettingViewModel.Services;


namespace Wlst.Ux.Wj1080Module.Wj1080ManageSettingViewModel.ViewModel
{
    [Export(typeof(IIWj1080ManageSettingViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class Wj1080ManageSettingViewModel : ObservableObject, IIWj1080ManageSettingViewModel
    {
        public Wj1080ManageSettingViewModel()
        {
            IsOnSelectNodeNavToParsSetView = wj1080TreeSetLoad.Myself.IsOnSelectNodeNavToParsSetView;
            IsShowGrpInTreeModelShowId = wj1080TreeSetLoad.Myself.IsShowGrpInTreeModelShowId;
            IsShowTreeOnTab = wj1080TreeSetLoad.Myself.IsShowTreeOnTab;
            IsShowArea = wj1080TreeSetLoad.Myself.IsShowArea;
            IsShowGrp = wj1080TreeSetLoad.Myself.IsShowGrp;
            IsShowFid = wj1080TreeSetLoad.Myself.IsShowFid;

            _dtApply = DateTime.Now.AddDays(-1);
            //this.NavOnLoad();

            //IsLuxLowTurnOn = wj1080TreeSetLoad.Myself.IsLuxLowTurnOn;

            //IsShowWarning = wj1080TreeSetLoad.Myself.IsShowWarning;
            //ShowWarning = IsShowWarning ? Visibility.Visible : Visibility.Collapsed;

            //StTime = wj1080TreeSetLoad.Myself.StTime;
            //EndTime = wj1080TreeSetLoad.Myself.EndTime;

            //IsSunRiseSpeechWarning = wj1080TreeSetLoad.Myself.IsSunRiseSpeechWarning;
            //IsSunSetSpeechWarning = wj1080TreeSetLoad.Myself.IsSunSetSpeechWarning;

            //SunRiseAlarmValue = wj1080TreeSetLoad.Myself.SunRiseAlarmValue;
            //SunRiseOffSet = wj1080TreeSetLoad.Myself.SunRiseOffSet;

            //SunSetAlarmValue = wj1080TreeSetLoad.Myself.SunSetAlarmValue;
            //SunSetOffset = wj1080TreeSetLoad.Myself.SunSetOffSet;

            //IsTrunOnWarning = wj1080TreeSetLoad.Myself.IsTrunOnWarning;
            //IsTrunOffWarning = wj1080TreeSetLoad.Myself.IsTrunOffWarning;



            //CurrentSelectLux = null;

            //for (int i = 0; i < LuxCollection.Count; i++)
            //{
            //    if (LuxCollection[i].Id == wj1080TreeSetLoad.Myself.CurrentSelectLux)
            //    {
            //        CurrentSelectLux = LuxCollection[i];
            //        break;
            //    }
            //}


            Is1080ShowTopRight = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2101, 1, false);//光控显示右上角

            for (int i = 0; i < LuxCollection.Count; i++)
            {
                if (LuxCollection[i].Id == Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2101, 2, 0))
                {
                    CurrentSelectLux = LuxCollection[i];
                    break;
                }
            }
           
            IsShowWarning = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2101, 3, false);
            ShowWarning = IsShowWarning ? Visibility.Visible : Visibility.Collapsed;

            //IsSunRiseSpeechWarning = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2101, 4, false);
            IsSunSetSpeechWarning = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2101, 5, false);

            //SunRiseAlarmValue = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2101, 6, 0);
            //SunRiseOffSet = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2101, 7, 0);

            SunSetAlarmValue = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2101, 8, 0);
           // SunSetOffset = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2101, 9, 0);

            IsTrunOnWarning = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2101, 10, false);
            IsTrunOffWarning = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2101, 11, false);

            IsShowOpenClose = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2101, 12, false);
            ShowOpenClose = IsShowOpenClose ? Visibility.Visible : Visibility.Collapsed;
            //SunOpenValue = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2101, 13, 0);
            //SunCloseValue = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2101, 14, 0);

            SunBefore = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2101, 15, 0); 
            TimeBefore = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2101, 16, 0);
        }

        #region  define

        private bool _isShowGrpInTreeModelShowId;

        /// <summary>
        /// 分组显示是否显示ID
        /// </summary>
        public bool IsShowGrpInTreeModelShowId
        {
            get { return _isShowGrpInTreeModelShowId; }
            set
            {
                if (value != _isShowGrpInTreeModelShowId)
                {
                    _isShowGrpInTreeModelShowId = value;
                    this.RaisePropertyChanged(() => this.IsShowGrpInTreeModelShowId);
                }
            }
        }



        private bool _isShowTreeOnTab;

        /// <summary>
        /// 是否在主界面显示 
        /// </summary>
        public bool IsShowTreeOnTab
        {
            get { return _isShowTreeOnTab; }
            set
            {
                if (value != _isShowTreeOnTab)
                {
                    _isShowTreeOnTab = value;
                    this.RaisePropertyChanged(() => this.IsShowTreeOnTab);
                }
            }
        }

        private bool _isShowArea;

        /// <summary>
        /// 是否在显示区域
        /// </summary>
        public bool IsShowArea
        {
            get { return _isShowArea; }
            set
            {
                if (value != _isShowArea)
                {
                    _isShowArea = value;
                    this.RaisePropertyChanged(() => this.IsShowArea);
                }
            }
        }

        private bool _isShowGrp;

        /// <summary>
        /// 是否在显示分组
        /// </summary>
        public bool IsShowGrp
        {
            get { return _isShowGrp; }
            set
            {
                if (value != _isShowGrp)
                {
                    _isShowGrp = value;
                    this.RaisePropertyChanged(() => this.IsShowGrp);
                }
            }
        }

        private bool _isShowFid;

        /// <summary>
        /// 是否在显示终端信息
        /// </summary>
        public bool IsShowFid
        {
            get { return _isShowFid; }
            set
            {
                if (value != _isShowFid)
                {
                    _isShowFid = value;
                    this.RaisePropertyChanged(() => this.IsShowFid);
                }
            }
        }


        private bool _iIsOnSelectNodeNavToParsSetView;

        /// <summary>
        /// 是否在点击终端的时候 直接导航到设置界面
        /// </summary>
        public bool IsOnSelectNodeNavToParsSetView
        {
            get { return _iIsOnSelectNodeNavToParsSetView; }
            set
            {
                if (value != _iIsOnSelectNodeNavToParsSetView)
                {
                    _iIsOnSelectNodeNavToParsSetView = value;
                    this.RaisePropertyChanged(() => this.IsOnSelectNodeNavToParsSetView);
                }
            }
        }



        private bool _isLuxLowTurnOn;

        /// <summary>
        /// 是否在光照度低时报警
        /// </summary>
        public bool IsLuxLowTurnOn
        {
            get { return _isLuxLowTurnOn; }
            set
            {
                if (value != _isLuxLowTurnOn)
                {
                    _isLuxLowTurnOn = value;
                    this.RaisePropertyChanged(() => this.IsLuxLowTurnOn);
                }
                if (value)
                {
                    IsLuxLowTurnOnVisi = Visibility.Visible;
                }
                else
                {
                    IsLuxLowTurnOnVisi = Visibility.Collapsed;
                }
            }
        }
        private Visibility _isLuxLowTurnOnVisi;

        /// <summary>
        /// 是否在光照度低时报警
        /// </summary>
        public Visibility IsLuxLowTurnOnVisi
        {
            get { return _isLuxLowTurnOnVisi; }
            set
            {
                if (value != _isLuxLowTurnOnVisi)
                {
                    _isLuxLowTurnOnVisi = value;
                    this.RaisePropertyChanged(() => this.IsLuxLowTurnOnVisi);
                }
            }
        }


        private int _stTime;

        /// <summary>
        /// 起始时间
        /// </summary>
        public int StTime
        {
            get { return _stTime; }
            set
            {
                if (value != _stTime)
                {
                    _stTime = value;
                    this.RaisePropertyChanged(() => this.StTime);
                }
            }
        }

        private int _endTime;

        /// <summary>
        /// 结束时间
        /// </summary>
        public int EndTime
        {
            get { return _endTime; }
            set
            {
                if (value != _endTime)
                {
                    _endTime = value;
                    this.RaisePropertyChanged(() => this.EndTime);
                }
            }
        }

        private bool _isSunSetSpeechWarning;
        /// <summary>
        /// 日落语音报警
        /// </summary>
        public bool IsSunSetSpeechWarning
        {
            get { return _isSunSetSpeechWarning; }
            set
            {
                if (value != _isSunSetSpeechWarning)
                {
                    _isSunSetSpeechWarning = value;
                    this.RaisePropertyChanged(() => this.IsSunSetSpeechWarning);
                }
            }
        }

        private bool _isSunRiseSpeechWarning;
        /// <summary>
        /// 日出语音报警
        /// </summary>
        public bool IsSunRiseSpeechWarning
        {
            get { return _isSunRiseSpeechWarning; }
            set
            {
                if (value != _isSunRiseSpeechWarning)
                {
                    _isSunRiseSpeechWarning = value;
                    this.RaisePropertyChanged(() => this.IsSunRiseSpeechWarning);
                }
            }
        }

        private int _sunSetAlarmValue;

        /// <summary>
        /// 日出报警值
        /// </summary>
        public int SunSetAlarmValue
        {
            get { return _sunSetAlarmValue; }
            set
            {
                if (value != _sunSetAlarmValue)
                {
                    _sunSetAlarmValue = value;
                    this.RaisePropertyChanged(() => this.SunSetAlarmValue);
                }
            }
        }

        private int _sunOpenValue;

        /// <summary>
        /// 开灯报警值
        /// </summary>
        public int SunOpenValue
        {
            get { return _sunOpenValue; }
            set
            {
                if (value != _sunOpenValue)
                {
                    _sunOpenValue = value;
                    this.RaisePropertyChanged(() => this.SunOpenValue);
                }
            }
        }


        private int _sunCloseValue;

        /// <summary>
        /// 关灯报警值
        /// </summary>
        public int SunCloseValue
        {
            get { return _sunCloseValue; }
            set
            {
                if (value != _sunCloseValue)
                {
                    _sunCloseValue = value;
                    this.RaisePropertyChanged(() => this.SunCloseValue);
                }
            }
        }

        private int _sunRiseAlarmValue;

        /// <summary>
        /// 日出报警值
        /// </summary>
        public int SunRiseAlarmValue
        {
            get { return _sunRiseAlarmValue; }
            set
            {
                if (value != _sunRiseAlarmValue)
                {
                    _sunRiseAlarmValue = value;
                    this.RaisePropertyChanged(() => this.SunRiseAlarmValue);
                }
            }
        }

        private int _sunSetOffset;

        /// <summary>
        /// 日落偏移值
        /// </summary>
        public int SunSetOffset
        {
            get { return _sunSetOffset; }
            set
            {
                if (value != _sunSetOffset)
                {
                    _sunSetOffset = value;
                    this.RaisePropertyChanged(() => this.SunSetOffset);
                }
            }
        }

        private int _sunRiseOffSet;

        /// <summary>
        /// 日出偏移值
        /// </summary>
        public int SunRiseOffSet
        {
            get { return _sunRiseOffSet; }
            set
            {
                if (value != _sunRiseOffSet)
                {
                    _sunRiseOffSet = value;
                    this.RaisePropertyChanged(() => this.SunRiseOffSet);
                }
            }
        }

        private bool _isShowWarning;
        /// <summary>
        /// 勾选低光控提示
        /// </summary>
        public bool IsShowWarning
        {
            get { return _isShowWarning; }
            set
            {
                if (_isShowWarning != value)
                {
                    _isShowWarning = value;
                    this.RaisePropertyChanged(() => this.IsShowWarning);
                    if (IsShowWarning == true) ShowWarning = Visibility.Visible;
                    else ShowWarning = Visibility.Collapsed;
                }
            }
        }

        private bool _isShowOpenClose;
        /// <summary>
        /// 勾选开关灯提醒
        /// </summary>
        public bool IsShowOpenClose
        {
            get { return _isShowOpenClose; }
            set
            {
                if (_isShowOpenClose != value)
                {
                    _isShowOpenClose = value;
                    this.RaisePropertyChanged(() => this.IsShowOpenClose);
                    if (IsShowOpenClose == true) ShowOpenClose = Visibility.Visible;
                    else ShowOpenClose = Visibility.Collapsed;
                }
            }
        }

        private Visibility _showWarning;
        /// <summary>
        /// 显示提示规则
        /// </summary>
        public Visibility ShowWarning
        {
            get { return _showWarning; }
            set
            {
                if (_showWarning != value)
                {
                    _showWarning = value;
                    this.RaisePropertyChanged(() => this.ShowWarning);
                }
            }
        }

        private Visibility _showOpenClose;
        /// <summary>
        /// 显示开关灯提示规则
        /// </summary>
        public Visibility ShowOpenClose
        {
            get { return _showOpenClose; }
            set
            {
                if (_showOpenClose != value)
                {
                    _showOpenClose = value;
                    this.RaisePropertyChanged(() => this.ShowOpenClose);
                }
            }
        }

        private bool _isTrunOnWarning;
        /// <summary>
        /// 开灯提示
        /// </summary>
        public bool IsTrunOnWarning
        {
            get { return _isTrunOnWarning; }
            set
            {
                if (_isTrunOnWarning != value)
                {
                    _isTrunOnWarning = value;
                    this.RaisePropertyChanged(() => this.IsTrunOnWarning);
                }
            }
        }

        private bool _isTrunOffWarning;
        /// <summary>
        /// 关灯提示
        /// </summary>
        public bool IsTrunOffWarning
        {
            get { return _isTrunOffWarning; }
            set
            {
                if (_isTrunOffWarning != value)
                {
                    _isTrunOffWarning = value;
                    this.RaisePropertyChanged(() => this.IsTrunOffWarning);
                }
            }
        }

        private int _sunBefore;

        /// <summary>
        /// 光控提前
        /// </summary>
        public int SunBefore
        {
            get { return _sunBefore; }
            set
            {
                if (value != _sunBefore)
                {
                    _sunBefore = value;
                    this.RaisePropertyChanged(() => this.SunBefore);
                }
            }
        }

        private int _timeBefore;

        /// <summary>
        /// 最后时限提前
        /// </summary>
        public int TimeBefore
        {
            get { return _timeBefore; }
            set
            {
                if (value != _timeBefore)
                {
                    _timeBefore = value;
                    this.RaisePropertyChanged(() => this.TimeBefore);
                }
            }
        }


        private ObservableCollection<IdNameDesc> _luxCollection;
        public ObservableCollection<IdNameDesc> LuxCollection
        {
            get
            {
                if (_luxCollection == null)
                {
                    _luxCollection = new ObservableCollection<IdNameDesc>();
                    foreach (var t in LuxGetServer.GetAllLuxEquipment)
                    {
                        _luxCollection.Add(new IdNameDesc { Id = t.Value, Name = t.Name, NameDesc = t.Value2.ToString("d4") + "-" + t.Name });
                    }
                }
                return _luxCollection;
            }
        }

        private IdNameDesc _currentSelectLux;

        /// <summary>
        /// 当前选中的光控
        /// </summary>
        public IdNameDesc CurrentSelectLux
        {
            get { return _currentSelectLux ?? (_currentSelectLux = new IdNameDesc()); }
            set
            {
                if (_currentSelectLux == value) return;
                _currentSelectLux = value;
                RaisePropertyChanged(() => CurrentSelectLux);

            }
        }


        private bool _is1080ShowTopRight;

        /// <summary>
        /// 光控是否显示在右上角
        /// </summary>
        public bool Is1080ShowTopRight
        {
            get { return _is1080ShowTopRight; }
            set
            {
                if (value != _is1080ShowTopRight)
                {
                    _is1080ShowTopRight = value;
                    this.RaisePropertyChanged(() => this.Is1080ShowTopRight);
                }
            }
        }

        #endregion


        private DateTime _dtApply;
        private ICommand _cmdApply;

        public ICommand CmdApply
        {
            get
            {

                if (_cmdApply == null) _cmdApply = new RelayCommand(Ex, CanEx, false);
                return _cmdApply;
            }
        }

        //todo 目前未作对终端过滤  如停运不发送选测等
        private void Ex()
        {
            _dtApply = DateTime.Now;
            wj1080TreeSetLoad.Myself.IsOnSelectNodeNavToParsSetView = IsOnSelectNodeNavToParsSetView;
            wj1080TreeSetLoad.Myself.IsShowGrpInTreeModelShowId = IsShowGrpInTreeModelShowId;
            wj1080TreeSetLoad.Myself.IsShowTreeOnTab = IsShowTreeOnTab;
            wj1080TreeSetLoad.Myself.IsShowArea = IsShowArea;
            wj1080TreeSetLoad.Myself.IsShowGrp = IsShowGrp;
            wj1080TreeSetLoad.Myself.IsShowFid = IsShowFid;
            //wj1080TreeSetLoad.Myself.IsLuxLowTurnOn = IsLuxLowTurnOn;
            //wj1080TreeSetLoad.Myself.StTime = StTime;
            //wj1080TreeSetLoad.Myself.EndTime = EndTime;

            wj1080TreeSetLoad.Myself.IsShowWarning = IsShowWarning;



            wj1080TreeSetLoad.Myself.CurrentSelectLux = CurrentSelectLux.Id;
            
            
            if (IsShowWarning)
            {
                //wj1080TreeSetLoad.Myself.IsSunRiseSpeechWarning = IsSunRiseSpeechWarning;
                wj1080TreeSetLoad.Myself.IsSunSetSpeechWarning = IsSunSetSpeechWarning;

                //wj1080TreeSetLoad.Myself.SunRiseAlarmValue = SunRiseAlarmValue;
                //wj1080TreeSetLoad.Myself.SunRiseOffSet = SunRiseOffSet;

                wj1080TreeSetLoad.Myself.SunSetAlarmValue = SunSetAlarmValue;
                //wj1080TreeSetLoad.Myself.SunSetOffSet = SunSetOffset;



                //wj1080TreeSetLoad.Myself.IsTrunOnWarning = IsTrunOnWarning;
                //wj1080TreeSetLoad.Myself.IsTrunOffWarning = IsTrunOffWarning;
            }
            else
            {
                //wj1080TreeSetLoad.Myself.IsSunRiseSpeechWarning = IsSunRiseSpeechWarning = false;
                wj1080TreeSetLoad.Myself.IsSunSetSpeechWarning = IsSunSetSpeechWarning = false;

                //wj1080TreeSetLoad.Myself.SunRiseAlarmValue = SunRiseAlarmValue = 0;
                //wj1080TreeSetLoad.Myself.SunRiseOffSet = SunRiseOffSet = 0;

                wj1080TreeSetLoad.Myself.SunSetAlarmValue = SunSetAlarmValue = 0;
                //wj1080TreeSetLoad.Myself.SunSetOffSet = SunSetOffset = 0;

                //wj1080TreeSetLoad.Myself.IsTrunOnWarning = IsTrunOnWarning = false;
                //wj1080TreeSetLoad.Myself.IsTrunOffWarning = IsTrunOffWarning = false;
            }

            wj1080TreeSetLoad.Myself.IsShowOpenClose = IsShowOpenClose;


            if (IsShowOpenClose)
            {
                //wj1080TreeSetLoad.Myself.SunCloseValue = SunCloseValue;
                //wj1080TreeSetLoad.Myself.SunOpenValue = SunOpenValue;
                
                wj1080TreeSetLoad.Myself.SunBefore = SunBefore ;
                wj1080TreeSetLoad.Myself.TimeBefore = TimeBefore ;

                wj1080TreeSetLoad.Myself.IsTrunOnWarning = IsTrunOnWarning;
                wj1080TreeSetLoad.Myself.IsTrunOffWarning = IsTrunOffWarning;

            }
            else
            {
                //wj1080TreeSetLoad.Myself.SunCloseValue = SunCloseValue = 0;
                //wj1080TreeSetLoad.Myself.SunOpenValue = SunOpenValue =0;

                wj1080TreeSetLoad.Myself.SunBefore = SunBefore = 0;
                wj1080TreeSetLoad.Myself.TimeBefore = TimeBefore = 0;

                wj1080TreeSetLoad.Myself.IsTrunOnWarning = IsTrunOnWarning = false;
                wj1080TreeSetLoad.Myself.IsTrunOffWarning = IsTrunOffWarning = false;
            }





            wj1080TreeSetLoad.Myself.Is1080ShowTopRight = Is1080ShowTopRight;

            wj1080TreeSetLoad.Myself.SavConfig();

        }

        private bool CanEx()
        {
            return DateTime.Now.Ticks - _dtApply.Ticks > 30000000;
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            //LoadConfig();
        }
    }

    public class wj1080TreeSetLoad
    {
        public bool IsShowGrpInTreeModelShowId;
        public bool IsOnSelectNodeNavToParsSetView;
        public bool IsShowTreeOnTab;
        public bool IsShowArea;
        public bool IsShowGrp;
        public bool IsShowFid;


        public bool IsShowOpenClose;
        public int SunOpenValue;
        public int SunCloseValue;

   //     public int StTime;
    //    public int EndTime;

        public int SunBefore;
        public int TimeBefore;

        public bool IsShowWarning;
        public int SunRiseAlarmValue;
        public int SunSetAlarmValue;
        public int SunRiseOffSet;
        public int SunSetOffSet;
        public bool IsSunRiseSpeechWarning;
        public bool IsSunSetSpeechWarning;

        public bool IsTrunOnWarning;
        public bool IsTrunOffWarning;

        public int CurrentSelectLux;

    //    public bool IsLuxLowTurnOn;

        public bool Is1080ShowTopRight;


        static wj1080TreeSetLoad myself;
        public static wj1080TreeSetLoad Myself
        {
            get
            {
                if (myself == null) new wj1080TreeSetLoad();
                return myself;
            }
        }


        private bool _loadCOnfig = false;
        public const string XmlConfigName = "Wj1080SetConfg";

        private wj1080TreeSetLoad()
        {
            if (_loadCOnfig) return;
            if (myself == null) myself = this;
            _loadCOnfig = true;

            var info = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read(XmlConfigName);
            if (info.ContainsKey("IsShowGrpInTreeModelShowId"))
            {
                IsShowGrpInTreeModelShowId = info["IsShowGrpInTreeModelShowId"].Contains("yes");
            }
            else IsShowGrpInTreeModelShowId = true;

            if (info.ContainsKey("IsOnSelectNodeNavToParsSetView"))
            {
                IsOnSelectNodeNavToParsSetView = info["IsOnSelectNodeNavToParsSetView"].Contains("yes");
            }
            else IsOnSelectNodeNavToParsSetView = true;

            if (info.ContainsKey("IsShowTreeOnTab"))
            {
                IsShowTreeOnTab = info["IsShowTreeOnTab"].Contains("yes");
            }
            else IsShowTreeOnTab = true;

            if (info.ContainsKey("IsShowArea"))
            {
                IsShowArea = info["IsShowArea"].Contains("yes");
            }
            else IsShowArea = true;

            if (info.ContainsKey("IsShowGrp"))
            {
                IsShowGrp = info["IsShowGrp"].Contains("yes");
            }
            else IsShowArea = true;

            if (info.ContainsKey("IsShowFid"))
            {
                IsShowFid = info["IsShowFid"].Contains("yes");
            }
            else IsShowFid = true;


            //if (info.ContainsKey("IsLuxLowTurnOn"))
            //{
            //    IsLuxLowTurnOn = info["IsLuxLowTurnOn"].Contains("yes");
            //}
            //else IsLuxLowTurnOn = false;

            string name;

            //if (info.TryGetValue("StTime", out name))
            //{
            //    StTime = int.Parse(name.Trim());
            //}
            //else StTime = 0;

            //if (info.TryGetValue("EndTime", out name))
            //{
            //    EndTime = int.Parse(name.Trim());
            //}
            //else EndTime = 0;

            //if (info.TryGetValue("IsShowWarning", out name))
            //{
            //    IsShowWarning = info["IsShowWarning"].Contains("yes");
            //}
            //else IsShowWarning = false;

            //if (info.TryGetValue("IsSunRiseSpeechWarning", out name))
            //{
            //    IsSunRiseSpeechWarning = info["IsSunRiseSpeechWarning"].Contains("yes");
            //}
            //else IsSunRiseSpeechWarning = false;

            //if (info.TryGetValue("IsSunSetSpeechWarning", out name))
            //{
            //    IsSunSetSpeechWarning = info["IsSunSetSpeechWarning"].Contains("yes");
            //}
            //else IsSunSetSpeechWarning = false;

            //if (info.TryGetValue("SunRiseAlarmValue", out name))
            //{
            //    SunRiseAlarmValue = int.Parse(name.Trim());
            //}
            //else SunRiseAlarmValue = 0;

            //if (info.TryGetValue("SunSetAlarmValue", out name))
            //{
            //    SunSetAlarmValue = int.Parse(name.Trim());
            //}
            //else SunSetAlarmValue = 0;

            //if (info.TryGetValue("SunRiseOffSet", out name))
            //{
            //    SunRiseOffSet = int.Parse(name.Trim());
            //}
            //else SunRiseOffSet = 0;

            //if (info.TryGetValue("SunSetOffSet", out name))
            //{
            //    SunSetOffSet = int.Parse(name.Trim());
            //}
            //else SunSetOffSet = 0;

            //if (info.TryGetValue("IsTrunOnWarning", out name))
            //{
            //    IsTrunOnWarning = info["IsTrunOnWarning"].Contains("yes");
            //}
            //else IsTrunOnWarning = false;

            //if (info.TryGetValue("IsTrunOffWarning", out name))
            //{
            //    IsTrunOffWarning = info["IsTrunOffWarning"].Contains("yes");
            //}
            //else IsTrunOffWarning = false;

            //if (info.TryGetValue("CurrentSelectLux", out name))
            //{
            //    CurrentSelectLux = int.Parse(name.Trim());
            //}
            //else CurrentSelectLux = 0;

            Wj1080Module.Wj1080ManageViewModel.ViewModel.Wj1080ManageViewModel.OnSelectNodeChangeNavToParsSet =
                IsOnSelectNodeNavToParsSetView;

            Is1080ShowTopRight = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2101, 1, false);
            CurrentSelectLux = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2101, 2, 0);

            IsShowWarning = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2101, 3, false);

            //IsSunRiseSpeechWarning = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2101, 4, false);
            IsSunSetSpeechWarning = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2101, 5, false);

            //SunRiseAlarmValue = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2101, 6, 0);
            //SunRiseOffSet = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2101, 7, 0);

            SunSetAlarmValue = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2101, 8, 0);
            //SunSetOffSet = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2101, 9, 0);

            IsTrunOnWarning = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2101, 10, false);
            IsTrunOffWarning = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2101, 11, false);

            IsShowOpenClose = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2101, 12, false);

            //SunOpenValue = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2101, 13, 0);
            //SunCloseValue = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2101, 14, 0);

            SunBefore = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2101, 15, 0);
            TimeBefore = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2101, 16, 0);

        }




        public void SavConfig()
        {
            var info = new Dictionary<string, string>();
            if (IsShowGrpInTreeModelShowId) info.Add("IsShowGrpInTreeModelShowId", "yes");
            else info.Add("IsShowGrpInTreeModelShowId", "no");

            if (IsOnSelectNodeNavToParsSetView) info.Add("IsOnSelectNodeNavToParsSetView", "yes");
            else info.Add("IsOnSelectNodeNavToParsSetView", "no");

            if (IsShowTreeOnTab) info.Add("IsShowTreeOnTab", "yes");
            else info.Add("IsShowTreeOnTab", "no");


            //if (IsLuxLowTurnOn) info.Add("IsLuxLowTurnOn", "yes");
            //else info.Add("IsLuxLowTurnOn", "no");

            if (IsShowArea) info.Add("IsShowArea", "yes");
            else info.Add("IsShowArea", "no");

            if (IsShowGrp) info.Add("IsShowGrp", "yes");
            else info.Add("IsShowGrp", "no");

            if (IsShowFid) info.Add("IsShowFid", "yes");
            else info.Add("IsShowFid", "no");

            //if (IsShowWarning) info.Add("StTime", StTime + "");
            //else info.Add("StTime", 0 + "");

            //if (IsShowWarning) info.Add("EndTime", EndTime + "");
            //else info.Add("EndTime", 0 + "");


            if (IsShowWarning) info.Add("IsShowWarning", "yes");
            else info.Add("IsShowWarning", "no");

            if (IsShowWarning)
            { info.Add("SunRiseAlarmValue", "" + SunRiseAlarmValue); }
            else
            {
                info.Add("SunRiseAlarmValue", "" + 0);
            }

            if (IsShowWarning) info.Add("SunSetAlarmValue", "" + SunSetAlarmValue);
            else
            {
                info.Add("SunSetAlarmValue", "" + 0);
            }

            if (IsShowWarning) info.Add("SunRiseOffSet", SunRiseOffSet + "");
            else
            {
                info.Add("SunRiseOffSet", 0 + "");
            }

            if (IsShowWarning) info.Add("SunSetOffSet", SunSetOffSet + "");
            else
            {
                info.Add("SunSetOffSet", 0 + "");
            }

            if (IsShowWarning)
            {
                if (IsSunRiseSpeechWarning) info.Add("IsSunRiseSpeechWarning", "yes");
                else info.Add("IsSunRiseSpeechWarning", "no");

                if (IsSunSetSpeechWarning) info.Add("IsSunSetSpeechWarning", "yes");
                else info.Add("IsSunSetSpeechWarning", "no");

                if (IsTrunOnWarning) info.Add("IsTrunOnWarning", "yes");
                else info.Add("IsTrunOnWarning", "no");

                if (IsTrunOffWarning) info.Add("IsTrunOffWarning", "yes");
                else info.Add("IsTrunOffWarning", "no");
            }
            else
            {
                info.Add("IsSunRiseSpeechWarning", "no");
                info.Add("IsSunSetSpeechWarning", "no");

                info.Add("IsTrunOnWarning", "no");
                info.Add("IsTrunOffWarning", "no");
            }


            info.Add("CurrentSelectLux", "" + CurrentSelectLux);

            Wlst.Cr.CoreOne.Services.SystemXmlConfig.Save(info, XmlConfigName);


            Wj1080Module.Wj1080ManageViewModel.ViewModel.Wj1080ManageViewModel.OnSelectNodeChangeNavToParsSet =
                IsOnSelectNodeNavToParsSetView;

            RegionManage.ShowViewByIdAttachRegion(
                Ux.Wj1080Module.Services.ViewIdAssign.Wj1080ManageViewId,

                IsShowTreeOnTab);

            var dicOp = new Dictionary<int, string>();
            var dicDesc = new Dictionary<int, string>();

            dicOp.Add(1, Is1080ShowTopRight ? "1" : "0");
            dicDesc.Add(1, "光控是否显示在右上角");

            dicOp.Add(2, CurrentSelectLux + "" );
            dicDesc.Add(2, "显示的光控");

            dicOp.Add(3, IsShowWarning ? "1" : "0");
            dicDesc.Add(3, "光控报警");

            //dicOp.Add(4, IsSunRiseSpeechWarning ? "1" : "0");
            //dicDesc.Add(4, "日出语音报警");

            dicOp.Add(5, IsSunSetSpeechWarning ? "1" : "0");
            dicDesc.Add(5, "日落语音报警");

            //dicOp.Add(6, SunRiseAlarmValue + "");
            //dicDesc.Add(6, "日出光控值");
            //dicOp.Add(7, SunRiseOffSet + "");
            //dicDesc.Add(7, "日出光控偏移");
            dicOp.Add(8, SunSetAlarmValue + "");
            dicDesc.Add(8, "日落光控值");
            //dicOp.Add(9, SunSetOffSet + "");
            //dicDesc.Add(9, "日落光控偏移");

            dicOp.Add(10, IsTrunOnWarning ? "1" : "0");
            dicDesc.Add(10, "开灯语音");
            dicOp.Add(11, IsTrunOffWarning ? "1" : "0");
            dicDesc.Add(11, "关灯语音");

            dicOp.Add(12, IsShowOpenClose ? "1" : "0");
            dicDesc.Add(12, "开关灯语音提醒");
            //dicOp.Add(13, SunOpenValue + "");
            //dicDesc.Add(13, "开灯光控值");
            //dicOp.Add(14, SunCloseValue + "");
            //dicDesc.Add(14, "关灯光控值");

            dicOp.Add(15, SunBefore + "");
            dicDesc.Add(15, "光控提前值");
            dicOp.Add(16, TimeBefore + "");
            dicDesc.Add(16, "最后时限提前分钟");

            Wlst.Cr.CoreOne.Services.OptionXmlSvr.SaveXml(2101, dicOp, dicDesc);

        }

    };

}

