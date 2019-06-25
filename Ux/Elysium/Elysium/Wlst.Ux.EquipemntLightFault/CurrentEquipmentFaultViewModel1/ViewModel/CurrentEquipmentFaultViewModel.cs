using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Sr.EquipemntLightFault.Model;
using Wlst.Sr.EquipemntLightFault.Services;
using Wlst.Ux.EquipemntLightFault.CurrentEquipmentFaultViewModel.Services;

namespace Wlst.Ux.EquipemntLightFault.CurrentEquipmentFaultViewModel.ViewModel
{
    //[Export(typeof(IICurrentEquipmentFaultView))]
    //[PartCreationPolicy(CreationPolicy.Shared)]
    public partial class CurrentEquipmentFaultViewModel : Wlst .Cr .Core .EventHandlerHelper .EventHandlerHelperExtendNotifyProperyChanged 
    {
        public CurrentEquipmentFaultViewModel()
        {
    
            this.InitAction();
            InitEvent();
            NavOnLoadr();
            Wlst.Cr.Coreb.AsyncTask.Qtz.AddQtz("null", 8888, DateTime.Now.Ticks, 60, Ac);
            if (_myself == null) _myself = this;
        }

        void Ac(object obj)
        {
            int _dtNow = DateTime.Now.Minute + DateTime.Now.Hour * 60;
            var info = SaveTime();

            int count = 0;
            if (info != null)
            {
                foreach (var t in info)
                {
                    if (_dtNow == t.Item2) Load(FaultRules[t.Item1 - 1]);


                    if (_dtNow == t.Item3 && info.Count > t.Item1 && _dtNow < FaultRules[t.Item1].StTime) Load(FaultRules[0]);
                    if (_dtNow == t.Item3 && info.Count == t.Item1) Load(FaultRules[0]);
                } 
            }
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
            LoadFaultType();
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
                    FaultComboBox = t.Value.FaultComboBox,
                    StTime = t.Value.StTime,
                    EndTime = t.Value.EndTime,
                    Id = t.Value.Id,
                    Fault1 = t.Value.Fault1,
                    Fault2 = t.Value.Fault2,
                    Fault3 = t.Value.Fault3,
                    Fault4 = t.Value.Fault4,
                    Fault5 = t.Value.Fault5,
                    IsEnable = t.Value.IsEnable,
                    ShowMoRen = showmoren
                });
            }
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
                        AddErrorInfo(ntgs, true);
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

        public static List<FaultItemViewModel > LoadFaultType()
        {
            var tmpssss = new List<FaultItemViewModel>();
            int i = 0;
            tmpssss.Add(new FaultItemViewModel()
            {
                Id = -1,
                FaultName = "无",
                Index = i
            });
            foreach (var t in Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.InfoDictionary)
            {
                if (t.Value.IsEnable)
                {
                    i++;
                    tmpssss.Add(new FaultItemViewModel()
                    {
                        Id = t.Value.FaultId,
                        FaultName = t.Value.FaultNameByDefine,
                        Index = i
                    });
                }
            }
            //var fcsort=FaultCollection.Select()
            var tmpggg = (from t in tmpssss orderby t.Id select t).ToList();
            //foreach (var t in tmpggg) FaultItems.Add(t);
            return tmpggg;
        }

        
    }

    //故障规则
    public partial class CurrentEquipmentFaultViewModel
    {
        private void OnRequestFault(string session, Wlst.mobile.MsgWithMobile infos)
        {                                    
        }

        #region Attribute

        private ObservableCollection<FaultItemViewModel> _faultItems;

        /// <summary>
        /// 查看故障选择combobox
        /// </summary>
        public ObservableCollection<FaultItemViewModel> FaultItems
        {
            get
            {
                if (_faultItems == null)
                {
                    _faultItems = new ObservableCollection<FaultItemViewModel>();
                }
                return _faultItems;
            }
            set
            {
                if (value == _faultItems) return;
                _faultItems = value;
                this.RaisePropertyChanged(() => FaultItems);
            }
        }

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
            foreach (var t in SaveTime())
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
            var args = new PublishEventArgs
            {
                EventType = PublishEventType.Core,
                EventId =Wlst.Ux.EquipemntLightFault.Services.EventIdAssign.VoiceAlarmClosed 
            };
            EventPublisher.EventPublish(args);
        }

        private bool CanExCmdVoiceAlarmClosed()
        {
            return true;
        }

        #endregion 

    }
    
    //RadGridView
    public partial class CurrentEquipmentFaultViewModel 
    {
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


        private void AddErrorInfo(FaultInfoBase error, bool dongtaiupdate)
        {
            // var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(id);
            if (error == null) return;

            var infovm = new FaultRecordViewModel
            {
                DataCreateTime = error.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
                RtuId = error.RtuPhyId,
                RtuName = error.RtuName,
                LoopName = error.RtuLoopName,
                FaultId = error.FaultId,
                FaultName = error.FaultName ,
                Id = error.Id,
                Color = "#FF3030"
            };
            int count = 0;
            if (Records.Count != 0)
            {
                foreach (var t in Records)
                {
                    if (t.FaultName == infovm.FaultName)
                    {
                        t.RecordItems.Insert(0, infovm);
                        count++;
                    }
                }
                if (count == 0) Records[5].RecordItems.Insert(0, infovm);
            }

        }

        private void DeleteTmlFault(List<int> info)
        {
            var lst = new List<FaultRecordViewModel>();
            for(int i =0;i<6;i++)
            {
                lst.AddRange( Records[i].RecordItems.Where(t => info.Contains(t.Id)).ToList());
            }
          

            foreach (var t in lst)
            {
                try
                {
                    for (int i = 0; i < 6;i++ )
                    {
                        if(Records[i].RecordItems.Contains(t))
                        {
                            Records[i].RecordItems.Remove(t);
                        }
                    }

                }
                catch (Exception ex)
                {
                }
            }

        }

        private void Load(CurrentItemViewModel rule)
        {
            int faultId1=0;
            int faultId2 = 0;
            int faultId3 = 0;
            int faultId4 = 0;
            int faultId5 = 0;

            string faultName1 = "";
            string faultName2= "";
            string faultName3 = "";
            string faultName4 = "";
            string faultName5 = "";
            foreach(var t in LoadFaultType() )
            {
                if(t.Id ==rule.Fault1  )
                {
                    faultName1 = t.FaultName;
                    faultId1 = t.Id;
                }
                if(t.Id ==rule.Fault2  )
                {
                    faultName2 = t.FaultName;
                    faultId2 = t.Id;
                }
                if(t.Id ==rule.Fault3  )
                {
                    faultName3 = t.FaultName;
                    faultId3 = t.Id;
                }
                if(t.Id ==rule.Fault4  )
                {
                    faultName4 = t.FaultName;
                    faultId4 = t.Id;
                }
                if(t.Id ==rule.Fault5  )
                {
                    faultName5 = t.FaultName;
                    faultId5 = t.Id;
                }
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


            

            var list1 = (from t in obs where t.FaultId == rule.Fault1 select t).ToList();
            var list2 = (from t in obs where t.FaultId == rule.Fault2 select t).ToList();
            var list3 = (from t in obs where t.FaultId == rule.Fault3 select t).ToList();
            var list4 = (from t in obs where t.FaultId == rule.Fault4 select t).ToList();
            var list5 = (from t in obs where t.FaultId == rule.Fault5 select t).ToList();
            var list6 = (from t in obs where (t.FaultId != rule.Fault1 && t.FaultId != rule.Fault2 && t.FaultId != rule.Fault3 && t.FaultId != rule.Fault4 && t.FaultId != rule.Fault5) select t).ToList();
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
            Records.Add(new AllFaultRecordsViewModel() { FaultName = "其他故障", FaultId = -1, RecordItems = collection6 });
        }

        private void LoadTimePeriod(int type)
        {
            var faultLst = new List<Tuple<int, string>>();

            if (Records.Count == 0) return;
            for (int i = 0; i < 6; i++)
            {
                faultLst.Add(new Tuple<int, string>(Records[i].FaultId, Records[i].FaultName));
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





            var list1 = (from t in obs where t.FaultId == faultLst[0].Item1  select t).ToList();
            var list2 = (from t in obs where t.FaultId == faultLst[1].Item1 select t).ToList();
            var list3 = (from t in obs where t.FaultId == faultLst[2].Item1 select t).ToList();
            var list4 = (from t in obs where t.FaultId == faultLst[3].Item1 select t).ToList();
            var list5 = (from t in obs where t.FaultId == faultLst[4].Item1 select t).ToList();
            var list6 = (from t in obs where (t.FaultId != faultLst[0].Item1 && t.FaultId != faultLst[1].Item1 && t.FaultId != faultLst[2].Item1 && t.FaultId != faultLst[3].Item1 && t.FaultId != faultLst[4].Item1) select t).ToList();
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
            Records.Add(new AllFaultRecordsViewModel() { FaultName = "其他故障", FaultId = -1, RecordItems = collection6 });
        }

        
    }
}
