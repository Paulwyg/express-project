using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using SpeechLib;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Sr.EquipemntLightFault.Model;
using Wlst.Sr.EquipemntLightFault.Services;
using Wlst.Ux.EquipemntLightFault.CurrentEquipmentFaultViewModel.Services;

namespace Wlst.Ux.EquipemntLightFault.CurrentEquipmentFaultViewModel.ViewModel
{

    public class EventArgsRule : EventArgs
    {
        public Dictionary<int, bool> LstSixHave = new Dictionary<int, bool>();

        public EventArgsRule(bool r1, bool r2, bool r3, bool r4, bool r5)
        {
            LstSixHave.Clear();
            LstSixHave.Add(1, r1);
            LstSixHave.Add(2, r2);
            LstSixHave.Add(3, r3);
            LstSixHave.Add(4, r4);
            LstSixHave.Add(5, r5);
        }
    }

    //[Export(typeof(IICurrentEquipmentFaultView))]
    //[PartCreationPolicy(CreationPolicy.Shared)]
    public partial class CurrentEquipmentFaultViewModel : Wlst .Cr .Core .EventHandlerHelper .EventHandlerHelperExtendNotifyProperyChanged 
    {
        public CurrentEquipmentFaultViewModel()
        {
    
            this.InitAction();
            InitEvent();
            NavOnLoadr();
            Wlst.Cr.Coreb.AsyncTask.Qtz.AddQtz("null", 8888, DateTime.Now.Ticks, 5, Ac);
            Wlst.Cr.Coreb.AsyncTask.Qtz.AddQtz("null", 8888, DateTime.Now.Ticks, 1, AcVoice);
            Wlst.Cr.Coreb.AsyncTask.Qtz.AddQtz("null", 8888, DateTime.Now.Ticks, 1, OneSecondAc);
            if (_myself == null) _myself = this;
        }

        public event EventHandler<EventArgsRule> OnRuleChanged;
        public List<int> GetCurrentSelected()
        {
            var rtn = new List<int>();
            var lst = (from t in Records select t.FaultId).ToList();
            if (lst.Count < 5) return new List<int>();
            for (int i = 0; i < 5; i++)
            {
                if (lst[i].Count  > 0) rtn.Add(i + 1);
            }
            return rtn;

        }

        private int curid = 0;
        void Ac(object obj)
        {
            int _dtNow = DateTime.Now.Minute + DateTime.Now.Hour * 60;
            var info = SaveTime();

     
            if (info != null)
            {
                bool load = false ;
                foreach (var t in info)
                {


                    if (_dtNow == t.Item2)
                    {
                        load = true;
                        if (t.Item1 == curid) break;
                        LoadRulesAc(FaultRules[t.Item1 - 1]);
                        curid = t.Item1;
                        break;
                    }


                    int st = t.Item2;
                    int et = t.Item3;
                    if (st > et)
                    {
                        et = 24*60 + et;
                    }
                    if (_dtNow > st && _dtNow < et)
                    {
                        load = true;
                        if (t.Item1 == curid) break;
                        LoadRulesAc(FaultRules[t.Item1 - 1]);
                        curid = t.Item1;
                        break;
                    }
                }
                if (load == false)
                {
                    if (curid != 1)
                    {
                        LoadRulesAc(FaultRules[0]);
                        curid = 1;
                    }
                }

            }
        }

        void LoadRulesAc(CurrentItemViewModel id)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(
                                                           () =>
                                                               {
                                                                   Load(id);
                                                                   //执行任务
                                                               }));
        }

        public static CurrentEquipmentFaultViewModel Myself
        {
            get
            {
                if (_myself == null) new CurrentEquipmentFaultViewModel();
                return _myself;
            }
        }

        private static CurrentEquipmentFaultViewModel _myself;

        private void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Sr.ProtocolPhone.LxFault.wlst_fault_pre,
                OnRequestFault,
                typeof(CurrentEquipmentFaultViewModel), this);
        }

        private void InitEvent()
        {
            this.AddEventFilterInfo(Wlst.Ux.EquipemntLightFault.Services.EventIdAssign.EquipmentFaultIsCheckedCount, PublishEventType.Core);
            this.AddEventFilterInfo(Sr.EquipemntLightFault.Services.EventIdAssign.EquipmentExistFaultAddId,
                                    PublishEventType.Core);
            this.AddEventFilterInfo(Sr.EquipemntLightFault.Services.EventIdAssign.EquipementExistFaultDeleteId,
                                    PublishEventType.Core);
        }

        public  void NavOnLoadr()
        {
            //LoadFaultType();
            FaultRules.Clear();
            ShowSetting = Visibility.Collapsed;
            
            var infoitem = CurrentEquipmentFaultInfoHold.ReadRules();
            var info = infoitem.Item1;
            var type = infoitem.Item2;
            //chosenFaults = info[1].ChosenFaults;

            switch (type)
            {
                case 1: IsCheckedFaultType1 = true; break;
                case 2: IsCheckedFaultType2 = true; break;
                case 3: IsCheckedFaultType3 = true; break;
                case 4: IsCheckedFaultType4 = true; break;
                case 5: IsCheckedFaultType5 = true; break;
            }

            foreach (var t in info)
            {
                var showmoren = true;
                if (t.Value.Id == 1) showmoren = false;
                FaultRules.Add(new CurrentItemViewModel()
                {
                    StTime = t.Value.StTime,
                    EndTime = t.Value.EndTime,
                    Id = t.Value.Id,
                    
                     
                    IsEnable = t.Value.IsEnable,
                    ShowMoRen = showmoren,
                    SelectedFaults =t.Value .SelectedFaults ,
                    Names =t.Value .Names 
                });
            }
            while  (FaultRules.Count < 6)
            {
                var tmp = new CurrentItemViewModel()
                              {
                                  EndTime = 1110,
                                  StTime = 480,
                                  Id = 0,
                                  };
                tmp.SelectedFaults.Add(1, new List<int>());
                tmp.SelectedFaults.Add(2, new List<int>());
                tmp.SelectedFaults.Add(3, new List<int>());
                tmp.SelectedFaults.Add(4, new List<int>());
                tmp.SelectedFaults.Add(5, new List<int>());

                FaultRules.Add(tmp);

                //return;
            }
            for (int i = 0; i < FaultRules.Count; i++)
            {
                FaultRules[i].Id = i + 1;
                FaultRules[i].ShowMoRen = true;
            }

            FaultRules[0].EndTime = -1500;
            FaultRules[0].StTime = -1500;
            FaultRules[0].ShowMoRen = false;
            FaultRules[0].IsEnable  = true ;


            int _dtNow = DateTime.Now.Minute + DateTime.Now.Hour * 60;
            int count = 0;
            foreach (var t in SaveTime())
            {
                if (_dtNow >= t.Item2 && _dtNow <= t.Item3)
                {
                    Load(FaultRules[t.Item1 - 1]);
                    count++;
                }
            }
            if (count == 0) Load(FaultRules[0]);        
            
            
        }

  

        private int faultsCount;
        private List<int> chosenFaults; 

        public override void ExPublishedEvent(PublishEventArgs args)
        {
            
            if(args.EventId == Sr.EquipemntLightFault.Services.EventIdAssign.EquipmentExistFaultAddId)
            {
                var info = args.GetParams()[0] as List<int>;
                if (info == null) return;
                foreach (var t in info)
                {
                    var ntgs = Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(t);
                    if (ntgs != null)
                        AddErrorInfo(ntgs, info.Count <2);
                }
            }
            if (args.EventId == EventIdAssign.EquipementExistFaultDeleteId)
            {
                var infos = args.GetParams()[0] as List<int>;
                if (infos == null) return;
                DeleteTmlFault(infos);
            }
        }

        #region 定时器

       

      

        #endregion

     
    }

    //故障规则
    public partial class CurrentEquipmentFaultViewModel
    {
        private void OnRequestFault(string session, Wlst.mobile.MsgWithMobile infos)
        {                                    
        }

        #region Attribute

        //private ObservableCollection<FaultItemViewModel> _faultItems;

        ///// <summary>
        ///// 查看故障选择combobox
        ///// </summary>
        //public ObservableCollection<FaultItemViewModel> FaultItems
        //{
        //    get
        //    {
        //        if (_faultItems == null)
        //        {
        //            _faultItems = new ObservableCollection<FaultItemViewModel>();
        //        }
        //        return _faultItems;
        //    }
        //    set
        //    {
        //        if (value == _faultItems) return;
        //        _faultItems = value;
        //        this.RaisePropertyChanged(() => FaultItems);
        //    }
        //}

        private ObservableCollection<CurrentItemViewModel> _faultRules;

        /// <summary>
        /// 设置故障查看规则
        /// </summary>
        public ObservableCollection<CurrentItemViewModel> FaultRules
        {
            get
            {
                if (_faultRules == null)
                {
                    _faultRules = new ObservableCollection<CurrentItemViewModel>();
                }
                return _faultRules;
            }
            set
            {
                if (value == _faultRules) return;
                _faultRules = value;
                this.RaisePropertyChanged(() => FaultRules);
            }
        }

        private CurrentItemViewModel _currentFaultRules;
        /// <summary>
        /// 当前选中规则
        /// </summary>
        public CurrentItemViewModel CurrentFaultRules
        {
            get { return _currentFaultRules; }
            set
            {
                if (_currentFaultRules != value)
                {
                    _currentFaultRules = value;
                    this.RaisePropertyChanged(() => this.CurrentFaultRules);
                }
            }
        }


        #region 查看故障时间

        private bool _ischeckedfaulttype1;
        public bool IsCheckedFaultType1
        {
            get { return _ischeckedfaulttype1; }
            set
            {
                if (_ischeckedfaulttype1 != value)
                {
                    _ischeckedfaulttype1 = value;
                    this.RaisePropertyChanged(() => this.IsCheckedFaultType1);
                    if (IsCheckedFaultType1)
                    {
                        IsCheckedFaultType2 = false;
                        IsCheckedFaultType3 = false;
                        IsCheckedFaultType4 = false;
                        IsCheckedFaultType5 = false;
                        FaultTypeText = "所有故障";
                        LoadTimePeriod(1);
                    }

                }
            }
        }
        private bool _ischeckedfaulttype2;
        public bool IsCheckedFaultType2
        {
            get { return _ischeckedfaulttype2; }
            set
            {
                if (_ischeckedfaulttype2 != value)
                {
                    _ischeckedfaulttype2 = value;
                    this.RaisePropertyChanged(() => this.IsCheckedFaultType2);
                    if (IsCheckedFaultType2)
                    {
                        IsCheckedFaultType1 = false;
                        IsCheckedFaultType3 = false;
                        IsCheckedFaultType4 = false;
                        IsCheckedFaultType5 = false;
                        FaultTypeText = "当天故障";
                        LoadTimePeriod(2);
                    }
                }
            }
        }

        private bool _ischeckedfaulttype3;
        public bool IsCheckedFaultType3
        {
            get { return _ischeckedfaulttype3; }
            set
            {
                if (_ischeckedfaulttype3 != value)
                {
                    _ischeckedfaulttype3 = value;
                    this.RaisePropertyChanged(() => this.IsCheckedFaultType3);
                    if (IsCheckedFaultType3)
                    {
                        IsCheckedFaultType1 = false;
                        IsCheckedFaultType2 = false;
                        IsCheckedFaultType4 = false;
                        IsCheckedFaultType5 = false;
                        FaultTypeText = "两天内故障";
                        LoadTimePeriod(3);
                    }
                }
            }
        }

        private bool _ischeckedfaulttype4;
        public bool IsCheckedFaultType4
        {
            get { return _ischeckedfaulttype4; }
            set
            {
                if (_ischeckedfaulttype4 != value)
                {
                    _ischeckedfaulttype4 = value;
                    this.RaisePropertyChanged(() => this.IsCheckedFaultType4);
                    if (IsCheckedFaultType4)
                    {
                        IsCheckedFaultType1 = false;
                        IsCheckedFaultType2 = false;
                        IsCheckedFaultType3 = false;
                        IsCheckedFaultType5 = false;
                        FaultTypeText = "三天内故障";
                        LoadTimePeriod(4);
                    }
                }
            }
        }

        private bool _faultTypeText;
        public bool IsCheckedFaultType5
        {
            get { return _faultTypeText; }
            set
            {
                if (_faultTypeText != value)
                {
                    _faultTypeText = value;
                    this.RaisePropertyChanged(() => this.FaultTypeText);
                    if (IsCheckedFaultType5)
                    {
                        IsCheckedFaultType1 = false;
                        IsCheckedFaultType2 = false;
                        IsCheckedFaultType3 = false;
                        IsCheckedFaultType4 = false;
                        FaultTypeText = "6小时内故障";
                        LoadTimePeriod(5);
                    }
                }
            }
        }
        private string _faulttypetext;
        public string FaultTypeText
        {
            get { return _faulttypetext; }
            set
            {
                if (_faulttypetext != value)
                {
                    _faulttypetext = value;
                    this.RaisePropertyChanged(() => this.FaultTypeText);
                }
            }
        }

        #endregion
        private bool _isSettingChecked;
        /// <summary>
        /// 勾选“增加查看规则”
        /// </summary>
        public bool IsSettingChecked
        {
            get { return _isSettingChecked; }
            set
            {
                if (_isSettingChecked != value)
                {
                    _isSettingChecked = value;                   
                    this.RaisePropertyChanged(() => this.IsSettingChecked);
                    if(IsSettingChecked ==true) ShowSetting = Visibility.Visible;
                    else ShowSetting = Visibility.Collapsed;
                }
            }
        }


        private Visibility _showSetting;
        /// <summary>
        /// 查看规则设置
        /// </summary>
        public Visibility ShowSetting
        {
            get { return _showSetting; }
            set
            {
                if (_showSetting != value)
                {
                    _showSetting = value;
                    this.RaisePropertyChanged(() => this.ShowSetting);
                }
            }
        }

        #endregion

        private List<Tuple<int, int, int>> SaveTime()
        {
            var lst = new List<Tuple<int, int, int>>();
            foreach (var t in FaultRules)
            {
                if (t.IsEnable == true) lst.Add(new Tuple<int, int, int>(t.Id, t.StTime, t.EndTime));
            }
            var lst2 = (from x in lst orderby x.Item2 select x).ToList();
            return lst2;
        }
    }

    //Command
    public partial class CurrentEquipmentFaultViewModel 
    {
        #region　CmdAddRule

        //private int count;

        //private ICommand _cCmdCmdAddRule;

        //public ICommand CmdAddRule
        //{
        //    get { return _cCmdCmdAddRule ?? (_cCmdCmdAddRule = new RelayCommand(ExCmdAddRule, CanCmdAddRule, true)); }
        //}


        //private void ExCmdAddRule()
        //{
        //    count = this.FaultRules.Count;
        //    var tmp = new ObservableCollection<FaultItemViewModel>();
        //    foreach (var t in LoadFaultType()) tmp.Add(t);
        //    this.FaultRules.Add(new CurrentItemViewModel() { Id = count += 1, StTime = 0, EndTime = 720, SelectedFault = "just for test", FaultComboBox = tmp });
            
        //}

        //private bool CanCmdAddRule()
        //{
        //    return true;
        //}
        //#endregion

        //#region CmdDelRule

        //private ICommand _cmdDelRule;

        //public ICommand CmdDelRule
        //{
        //    get { return _cmdDelRule ?? (_cmdDelRule = new RelayCommand(ExCmdDelRule, CanExCmdDelRule, true)); }
        //}


        //private void ExCmdDelRule()
        //{
        //    if (FaultRules.Contains(CurrentFaultRules)) FaultRules.Remove(CurrentFaultRules);
        //}

        //private bool CanExCmdDelRule()
        //{
        //    return true;
        //}

        #endregion

        #region CmdSaveRule

        private ICommand _cmdSaveRule;

        public ICommand CmdSaveRule
        {
            get { return _cmdSaveRule ?? (_cmdSaveRule = new RelayCommand(ExCmdSaveRule, CanExCmdSaveRule, true)); }
        }


        private void ExCmdSaveRule()
        {
            var timelst =new List<Tuple<int,int>>();
            foreach (var t in FaultRules)
            {
                if (t.Id > 1 && t.IsEnable == true)
                {
                    var tu = new Tuple<int, int>(t.StTime, t.EndTime);
                    timelst.Add(tu);
                }
            }

            if (timelst.Count > 1)
            {
                timelst = (from t in timelst orderby t.Item1 select t).ToList();
                for (int i = 0; i < timelst.Count - 1; i++)
                {
                    if (timelst[i].Item2 > timelst[i + 1].Item1)
                    {
                        Wlst.Cr.MessageBoxOverride.MessageBoxOverride.UMessageBox.Show("警告", "选中规则的时间有重叠。", UMessageBoxButton.Ok);
                        return;
                    }
                }
            }

            var type = 1;
            if (IsCheckedFaultType1) type = 1;
            else if (IsCheckedFaultType2) type = 2;
            else if (IsCheckedFaultType3) type = 3;
            else if (IsCheckedFaultType4) type = 4;
            else if (IsCheckedFaultType5) type = 5;
            CurrentEquipmentFaultInfoHold.WriteRules(FaultRules,type);

            int _dtNow = DateTime.Now.Minute + DateTime.Now.Hour * 60;

            int count = 0;
            var info = SaveTime();
            foreach (var t in info)
            {
                if (_dtNow >= t.Item2 && _dtNow <= t.Item3)
                {
                    Load(FaultRules[t.Item1 - 1]);
                    count++;
                }
            }
            if (count == 0) Load(FaultRules[0]); 
            
            Wlst.Cr.MessageBoxOverride.MessageBoxOverride.UMessageBox.Show("提醒", "保存成功。", UMessageBoxButton.Ok);

        }

        private bool CanExCmdSaveRule()
        {
            return true;
        }

        #endregion 

        #region CmdRead

        private ICommand _cmdRead;

        public ICommand CmdRead
        {
            get { return _cmdRead ?? (_cmdRead = new RelayCommand(ExCmdRead, CanExCmdRead, true)); }
        }


        private void ExCmdRead()
        {
            foreach(var t in Records)
            {
                foreach (var ttt in t.RecordItems) ttt.Color = "#FFFFFF";
                t.FaultCountNew = 0;
                t.FaultCountNewShow = Visibility.Collapsed;

             
                CountNewError = 0;
            }
        }

        private bool CanExCmdRead()
        {
            return true;
        }

        #endregion 

        #region CmdVoiceAlarmClosed

        private ICommand _cmdVoiceAlarmClosed;

        public ICommand CmdVoiceAlarmClosed
        {
            get { return _cmdVoiceAlarmClosed ?? (_cmdVoiceAlarmClosed = new RelayCommand(ExCmdVoiceAlarmClosed, CanExCmdVoiceAlarmClosed, true)); }
        }


        private void ExCmdVoiceAlarmClosed()
        {
            //var args = new PublishEventArgs
            //{
            //    EventType = PublishEventType.Core,
            //    EventId =Wlst.Ux.EquipemntLightFault.Services.EventIdAssign.VoiceAlarmClosed 
            //};
            
            //EventPublisher.EventPublish(args);
            startStopVoice = true;
            last = 60;
            ClearVoiceReportItems();
        }

        private bool CanExCmdVoiceAlarmClosed()
        {
            return startStopVoice == false;
        }


        private int last = 60;
        private bool startStopVoice = false;
        void OneSecondAc(object obj)
        {
            if (startStopVoice == false) return;
            if(last <1)
            {
                startStopVoice =false ;
                CmdStr = "暂停语音报警60秒";
            }
            else
            {
                last -= 1;
                CmdStr = "暂停语音报警"+last +"秒";
            }

            
        
        }


        private string cmdstr;
        public string CmdStr
        {
            get
            {
                if (string.IsNullOrEmpty(cmdstr)) cmdstr = "暂停语音报警60秒";
                return cmdstr;
            }
            set
            {
                if (value == cmdstr) return;
                cmdstr = value;
                this.RaisePropertyChanged(() => this.CmdStr);
            }
        }
        #endregion 


        #region 语音报警


        private static bool _isVoiceReport = true;
         
        private static ConcurrentQueue<string> _voiceReportItems = new ConcurrentQueue<string>();


        void AcVoice(object obj)
        {
            try
            {


                SpeechVoiceSpeakFlags SpFlags = SpeechVoiceSpeakFlags.SVSFDefault; //.SVSFlagsAsync;

                var voice = new SpVoice();


                while (_isVoiceReport)
                {
                    try
                    {
                        if (_voiceReportItems.Count > 0)
                        {
                            string speaktext = "";
                            if (_voiceReportItems.TryDequeue(out speaktext))
                            {
                                if (_voiceReportItems.Count > 25) continue;
                                if (string.IsNullOrEmpty(speaktext)) continue;
                                voice.Speak(speaktext, SpFlags);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("语音报警出错:" + ex);
                    }
                    Thread.Sleep(200);
                }


            }
            catch (Exception)
            {

            }
        }

    
        public void ClearVoiceReportItems()
        {
            string speaktext = "";
            while (_voiceReportItems.Count > 0)
                _voiceReportItems.TryDequeue(out speaktext);

        }

        #endregion
    }
    
    //RadGridView
    public partial class CurrentEquipmentFaultViewModel 
    {

        public void OnSelectedFaults(FaultRecordViewModel data,bool isLeft)
        {
            OnRequestServerData(data,isLeft );
        }
        public void OnRequestServerData(FaultRecordViewModel info, bool isLeft)
        {
            if (info == null) return;
            if (isLeft == false)
            {
                Sr.EquipemntLightFault.Services.PreErrorServices.RequestDataWhenErrorHappen(info.RtuLgcId,0,
                                                                                            info.DateCreate);


            }
            //发布事件  选中当前节点
            var args = new PublishEventArgs
            {
                EventType = PublishEventType.Core,
                EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
            };
            args.AddParams(info.RtuLgcId);
            EventPublisher.EventPublish(args);
        }

        #region Attribute

        private ObservableCollection<AllFaultRecordsViewModel> _records;

        /// <summary>
        /// 故障表格
        /// </summary>
        public ObservableCollection<AllFaultRecordsViewModel> Records
        {
            get
            {
                if (_records == null)
                {
                    _records = new ObservableCollection<AllFaultRecordsViewModel>();
                }
                return _records;
            }
            set
            {
                if (value == _records) return;
                _records = value;
                this.RaisePropertyChanged(() => Records);
            }
        }

        #endregion

        private int cnewerror=0;

        private int CountNewError
        {
            get { return cnewerror; }
            set
            {
                if (value == cnewerror) return;
                cnewerror = value;
                var argss = new PublishEventArgs()
                {
                    EventType = PublishEventType.Core,
                    EventId = EquipemntLightFault.Services.EventIdAssign.PushErrNums
                };
                argss.AddParams(value );
                EventPublisher.EventPublish(argss);
            }
        }


        private void AddErrorInfo(FaultInfoBase error, bool isNewAdd)
        {
            // var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(id);
            if (error == null) return;

            var infovm = new FaultRecordViewModel
            {
                DataCreateTime = error.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
                RtuId = error.RtuPhyId,
                RtuLgcId = error .RtuId,
                RtuName = error.RtuName,
                LoopName = error.RtuLoopName,
                FaultId = error.FaultId,
                FaultName = error.FaultName ,
                Id = error.Id,
                LoopId =error .LoopId ,
                DateCreate =error .DateCreate.Ticks  ,
                  Color = isNewAdd ?"#FF3030": "#FFFFFF"
            };
            int count = 0;
            if (Records.Count != 0)
            {
                foreach (var t in Records)
                {
                    if (t.FaultId .Contains( infovm .FaultId ) )
                    {
                        t.RecordItems.Insert(0, infovm);
                        count++;
                      if(isNewAdd )  t.FaultCountNew += 1;
                    }
                    t.FaultCountOld = t.RecordItems.Count;
                }
                if (count == 0)
                {
                    Records[5].RecordItems.Insert(0, infovm);
                    Records[5].FaultCountOld = Records[5].RecordItems.Count;
                    if (isNewAdd) Records[5].FaultCountNew += 1;
                }
            }


            var tmp = 0;
            foreach (var g in Records )
            {
                tmp += g.FaultCountNew;
            }
            CountNewError = tmp;

            if (_isVoiceReport && startStopVoice==false && isNewAdd )
            {
                int alarmNum = 0;
                string temp;
                while (error.AlarmTimes > alarmNum)
                {
                    string tt = error.FaultName;
                    if (tt.Contains("防盗")) tt = "被盗";
                    if (tt.Contains("门")) tt = "打开";
                    temp = error.RtuName + error.LoopId + "        " + tt;
                    _voiceReportItems.Enqueue(temp);
                    alarmNum++;
                }
            }

        }

        private void DeleteTmlFault(List<int> info)
        {
            if (info == null) return;
            //var lst = new List<FaultRecordViewModel>();
            //for(int i =0;i<6;i++)
            //{
            //    lst.AddRange( Records[i].RecordItems.Where(t => info.Contains(t.Id)).ToList());
            //}
          

            //foreach (var t in lst)
            //{
            //    try
            //    {
            //        for (int i = 0; i < 6;i++ )
            //        {
            //            if(Records[i].RecordItems.Contains(t))
            //            {
            //                Records[i].RecordItems.Remove(t);
            //            }
            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //}


            foreach (var f in Records)
            {
                try
                {
                    for (int j=f.RecordItems .Count -1;j>=0;j--)
                    {
                        if(info .Contains(  f.RecordItems [j].Id ))
                        {
                            if(f.RecordItems [j ].Color .Equals( "#FF3030") && f.FaultCountNew >0)
                            {
                                f.FaultCountNew -= 1;
                            }
                            f.RecordItems.RemoveAt(j);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                f.FaultCountOld = f.RecordItems.Count;
            }
            var tmp = 0;
            foreach (var g in Records)
            {
                tmp += g.FaultCountNew;
            }
            CountNewError = tmp;
        }

        private void Load(CurrentItemViewModel rule)
        {
            var  faultId1 = rule.SelectedFaults[1];// .Id ;
            var  faultId2 = rule.SelectedFaults[2];
            var  faultId3 = rule.SelectedFaults[3];
            var  faultId4 = rule.SelectedFaults[4];
            var  faultId5 = rule.SelectedFaults[5];

            string faultName1 = rule.Names [0].Name   ;
            string faultName2 = rule.Names[1].Name;
            string faultName3 = rule.Names[2].Name;
            string faultName4 = rule.Names[3].Name;
            string faultName5 = rule.Names[4].Name;

            if(OnRuleChanged !=null )
            {
                OnRuleChanged(this,
                              new EventArgsRule(faultId1.Count > 0, faultId2.Count > 0, faultId3.Count > 0, faultId4.Count > 0, faultId5.Count > 0));
            }


            var lst = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Values;
            
            Records.Clear();
            var obs = new List<FaultRecordViewModel>();
            int intx = 0;
            if (IsCheckedFaultType1 ==true)
            {
                foreach (var t in lst)
                {
                    var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(t.Id);
                    if (error == null) continue;
                    intx++;
                    obs.Add(new FaultRecordViewModel
                    {
                        DataCreateTime = t.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
                        RtuId = t.RtuPhyId,
                        RtuLgcId = t.RtuId,
                        LoopId = error.LoopId,
                        DateCreate = error.DateCreate.Ticks,
                        RtuName = t.RtuName,
                        LoopName = t.RtuLoopName,
                        FaultId = t.FaultId,
                        FaultName = t.FaultName,
                        Id = t.Id,
                        Color = "#FFFFFF"
                    });
                }
            }
            if (IsCheckedFaultType2 == true)
            {
                foreach (var t in lst)
                {
                    var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(t.Id);
                    if (error == null) continue;
                    intx++;

                    if (t.DateCreate.Day == DateTime.Now.Day && t.DateCreate.Month == DateTime.Now.Month && t.DateCreate.Year == DateTime.Now.Year)
                    {
                        obs.Add(new FaultRecordViewModel
                        {
                            DataCreateTime = t.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
                            RtuId = t.RtuPhyId,
                            RtuLgcId = t.RtuId,
                            LoopId = error.LoopId,
                            DateCreate = error.DateCreate.Ticks,
                            RtuName = t.RtuName,
                            LoopName = t.RtuLoopName,
                            FaultId = t.FaultId,
                            FaultName = t.FaultName,
                            Id = t.Id,
                            Color = "#FFFFFF"
                        });
                    }
                }
            }
            if (IsCheckedFaultType3 == true)
            {
                foreach (var t in lst)
                {
                    var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(t.Id);
                    if (error == null) continue;
                    intx++;

                    if (t.DateCreate>= DateTime.Now.AddDays( -1))
                    {
                        obs.Add(new FaultRecordViewModel
                        {
                            DataCreateTime = t.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
                            RtuId = t.RtuPhyId,
                            RtuLgcId = t.RtuId,
                            LoopId = error.LoopId,
                            DateCreate = error.DateCreate.Ticks,
                            RtuName = t.RtuName,
                            LoopName = t.RtuLoopName,
                            FaultId = t.FaultId,
                            FaultName = t.FaultName,
                            Id = t.Id,
                            Color = "#FFFFFF"
                        });
                    }
                }
            }
            if (IsCheckedFaultType4 == true)
            {
                foreach (var t in lst)
                {
                    var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(t.Id);
                    if (error == null) continue;
                    intx++;
                    if (t.DateCreate >= DateTime.Now.AddDays(-2))
                    {
                        obs.Add(new FaultRecordViewModel
                        {
                            DataCreateTime = t.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
                            RtuId = t.RtuPhyId,
                            RtuLgcId = t.RtuId,
                            LoopId = error.LoopId,
                            DateCreate = error.DateCreate.Ticks,
                            RtuName = t.RtuName,
                            LoopName = t.RtuLoopName,
                            FaultId = t.FaultId,
                            FaultName = t.FaultName,
                            Id = t.Id,
                            Color = "#FFFFFF"
                        });
                    }
                }
            }
            if (IsCheckedFaultType5 == true)
            {
                foreach (var t in lst)
                {
                    var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(t.Id);
                    if (error == null) continue;
                    intx++;
                    if (t.DateCreate >= DateTime.Now.AddHours(-6))
                    {
                        obs.Add(new FaultRecordViewModel
                        {
                            DataCreateTime = t.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
                            RtuId = t.RtuPhyId,
                            RtuLgcId = t.RtuId,
                            LoopId = error.LoopId,
                            DateCreate = error.DateCreate.Ticks,
                            RtuName = t.RtuName,
                            LoopName = t.RtuLoopName,
                            FaultId = t.FaultId,
                            FaultName = t.FaultName,
                            Id = t.Id,
                            Color = "#FFFFFF"
                        });
                    }
                }
            }


            

            var list1 = (from t in obs where   faultId1.Contains( t.FaultId )  select t).ToList();
            var list2 = (from t in obs where faultId2.Contains(t.FaultId) select t).ToList();
            var list3 = (from t in obs where faultId3.Contains(t.FaultId) select t).ToList();
            var list4 = (from t in obs where faultId4.Contains(t.FaultId) select t).ToList();
            var list5 = (from t in obs where faultId5.Contains(t.FaultId) select t).ToList();

            var tmp = new List<int>();
            tmp.AddRange(faultId1); tmp.AddRange(faultId2); tmp.AddRange(faultId3); tmp.AddRange(faultId4);
            tmp.AddRange(faultId5);
            var list6 = (from t in obs where  !tmp .Contains( t.FaultId )    select t).ToList();
            var collection1 = new ObservableCollection<FaultRecordViewModel>();
            foreach (var f in list1) collection1.Add(f);
            var collection2 = new ObservableCollection<FaultRecordViewModel>();
            foreach (var f in list2) collection2.Add(f);
            var collection3 = new ObservableCollection<FaultRecordViewModel>();
            foreach (var f in list3) collection3.Add(f);
            var collection4 = new ObservableCollection<FaultRecordViewModel>();
            foreach (var f in list4) collection4.Add(f);
            var collection5 = new ObservableCollection<FaultRecordViewModel>();
            foreach (var f in list5) collection5.Add(f);
            var collection6 = new ObservableCollection<FaultRecordViewModel>();
            foreach (var f in list6) collection6.Add(f);


            Records.Add(new AllFaultRecordsViewModel() { FaultName = faultName1 ,FaultId = faultId1, RecordItems = collection1 });
            Records.Add(new AllFaultRecordsViewModel() { FaultName = faultName2, FaultId = faultId2, RecordItems = collection2 });
            Records.Add(new AllFaultRecordsViewModel() { FaultName = faultName3, FaultId = faultId3, RecordItems = collection3 });
            Records.Add(new AllFaultRecordsViewModel() { FaultName = faultName4, FaultId = faultId4, RecordItems = collection4 });
            Records.Add(new AllFaultRecordsViewModel() { FaultName = faultName5, FaultId = faultId5, RecordItems = collection5 });
            Records.Add(new AllFaultRecordsViewModel() { FaultName = "其他故障", IsOther =true , RecordItems = collection6 });

            foreach (var f  in Records )
            {
                f.FaultCountNew = 0;
                f.FaultCountNewShow = Visibility.Collapsed ;
                f.FaultCountOld = f.RecordItems.Count;
            }

            CountNewError = 0;
        }

        private void LoadTimePeriod(int type)
        {
            var faultLst = new List<Tuple<List< int >, string>>();

            if (Records.Count == 0) return;
            for (int i = 0; i < 6; i++)
            {
                faultLst.Add(new Tuple<List<int>, string>(Records[i].FaultId, Records[i].FaultName));
            }




            var lst = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Values;           
            Records.Clear();
            var obs = new List<FaultRecordViewModel>();
            int intx = 0;
            if (type == 1)
            {
                foreach (var t in lst)
                {
                    var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(t.Id);
                    if (error == null) continue;
                    intx++;
                    obs.Add(new FaultRecordViewModel
                                {
                                    DataCreateTime = t.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
                                    RtuId = t.RtuPhyId,
                                    RtuLgcId = t.RtuId,
                                    LoopId = error.LoopId,
                                    DateCreate = error.DateCreate.Ticks,
                                    RtuName = t.RtuName,
                                    LoopName = t.RtuLoopName,
                                    FaultId = t.FaultId,
                                    FaultName = t.FaultName,
                                    Id = t.Id,
                                    Color = "#FFFFFF"
                                });
                }
            }
            if (type == 2)
            {
                foreach (var t in lst)
                {
                    var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(t.Id);
                    if (error == null) continue;
                    intx++;

                    if (t.DateCreate.Day == DateTime.Now.Day && t.DateCreate.Month == DateTime.Now.Month && t.DateCreate.Year == DateTime.Now.Year )
                    {
                        obs.Add(new FaultRecordViewModel
                                    {
                                        DataCreateTime = t.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
                                        RtuId = t.RtuPhyId,
                                        RtuLgcId = t.RtuId,
                                        RtuName = t.RtuName,
                                        LoopName = t.RtuLoopName,
                                        FaultId = t.FaultId,
                                        FaultName = t.FaultName,
                                        Id = t.Id,
                                        Color = "#FFFFFF"
                                    });
                    }
                }
            }
            if (type == 3)
            {
                foreach (var t in lst)
                {
                    var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(t.Id);
                    if (error == null) continue;
                    intx++;

                    if (t.DateCreate >= DateTime.Now.AddDays(-1))
                    {
                        obs.Add(new FaultRecordViewModel
                                    {
                                        DataCreateTime = t.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
                                        RtuId = t.RtuPhyId,
                                        RtuLgcId = t.RtuId,
                                        LoopId = error.LoopId,
                                        DateCreate = error.DateCreate.Ticks,
                                        RtuName = t.RtuName,
                                        LoopName = t.RtuLoopName,
                                        FaultId = t.FaultId,
                                        FaultName = t.FaultName,
                                        Id = t.Id,
                                        Color = "#FFFFFF"
                                    });
                    }
                }
            }
            if (type == 4)
            {
                foreach (var t in lst)
                {
                    var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(t.Id);
                    if (error == null) continue;
                    intx++;
                    if (t.DateCreate >= DateTime.Now.AddDays(-2))
                    {
                        obs.Add(new FaultRecordViewModel
                                    {
                                        DataCreateTime = t.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
                                        RtuId = t.RtuPhyId,
                                        RtuLgcId = t.RtuId,
                                        LoopId = error.LoopId,
                                        DateCreate = error.DateCreate.Ticks,
                                        RtuName = t.RtuName,
                                        LoopName = t.RtuLoopName,
                                        FaultId = t.FaultId,
                                        FaultName = t.FaultName,
                                        Id = t.Id,
                                        Color = "#FFFFFF"
                                    });
                    }
                }
            }
            if (type==5)
            {
                foreach (var t in lst)
                {
                    var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(t.Id);
                    if (error == null) continue;
                    intx++;
                    if (t.DateCreate >= DateTime.Now.AddHours( -6))
                    {
                        obs.Add(new FaultRecordViewModel
                                    {
                                        DataCreateTime = t.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
                                        RtuId = t.RtuPhyId,
                                        RtuLgcId =t.RtuId ,
                                        LoopId = error.LoopId,
                                        DateCreate = error.DateCreate.Ticks,
                                        RtuName = t.RtuName,
                                        LoopName = t.RtuLoopName,
                                        FaultId = t.FaultId,
                                        FaultName = t.FaultName,
                                        Id = t.Id,
                                        Color = "#FFFFFF"
                                    });
                    }
                }
            }





            var list1 = (from t in obs where  faultLst[0].Item1.Contains( t.FaultId )  select t).ToList();
            var list2 = (from t in obs where faultLst[0].Item1.Contains(t.FaultId) select t).ToList();
            var list3 = (from t in obs where faultLst[0].Item1.Contains(t.FaultId) select t).ToList();
            var list4 = (from t in obs where faultLst[0].Item1.Contains(t.FaultId) select t).ToList();
            var list5 = (from t in obs where faultLst[0].Item1.Contains(t.FaultId) select t).ToList();
            var mtp = new List<int>();
            mtp.AddRange(faultLst[0].Item1);
            mtp.AddRange(faultLst[1].Item1);
            mtp.AddRange(faultLst[2].Item1);
            mtp.AddRange(faultLst[3].Item1);
            mtp.AddRange(faultLst[4].Item1);

            var list6 = (from t in obs where !mtp .Contains( t.FaultId ) select t).ToList();
            var collection1 = new ObservableCollection<FaultRecordViewModel>();
            foreach (var f in list1) collection1.Add(f);
            var collection2 = new ObservableCollection<FaultRecordViewModel>();
            foreach (var f in list2) collection2.Add(f);
            var collection3 = new ObservableCollection<FaultRecordViewModel>();
            foreach (var f in list3) collection3.Add(f);
            var collection4 = new ObservableCollection<FaultRecordViewModel>();
            foreach (var f in list4) collection4.Add(f);
            var collection5 = new ObservableCollection<FaultRecordViewModel>();
            foreach (var f in list5) collection5.Add(f);
            var collection6 = new ObservableCollection<FaultRecordViewModel>();
            foreach (var f in list6) collection6.Add(f);


            Records.Add(new AllFaultRecordsViewModel() { FaultName = faultLst[0].Item2, FaultId = faultLst[0].Item1, RecordItems = collection1 });
            Records.Add(new AllFaultRecordsViewModel() { FaultName = faultLst[1].Item2, FaultId = faultLst[1].Item1, RecordItems = collection2 });
            Records.Add(new AllFaultRecordsViewModel() { FaultName = faultLst[2].Item2, FaultId = faultLst[2].Item1, RecordItems = collection3 });
            Records.Add(new AllFaultRecordsViewModel() { FaultName = faultLst[3].Item2, FaultId = faultLst[3].Item1, RecordItems = collection4 });
            Records.Add(new AllFaultRecordsViewModel() { FaultName = faultLst[4].Item2, FaultId = faultLst[4].Item1, RecordItems = collection5 });
            Records.Add(new AllFaultRecordsViewModel() { FaultName = "其他故障", IsOther =true , RecordItems = collection6 });

            foreach (var f in Records)
            {
                f.FaultCountNew = 0;
                f.FaultCountNewShow = Visibility.Collapsed ;
                f.FaultCountOld = f.RecordItems.Count;
            } 
            CountNewError = 0;
        }

        
    }
}
