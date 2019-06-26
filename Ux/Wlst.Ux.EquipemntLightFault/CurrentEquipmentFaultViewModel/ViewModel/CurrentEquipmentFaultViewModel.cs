using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.Practices.Prism;


using SpeechLib;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.ModuleServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Sr.AssetManageInfoHold.Model;
using Wlst.Sr.EquipemntLightFault.Model;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Ux.EquipemntLightFault.CurrentEquipmentFaultViewModel.Services;
using Wlst.Ux.EquipemntLightFault.SendOrderViewModel.ViewModel;
using Wlst.client;
using EventIdAssign = Wlst.Sr.EquipemntLightFault.Services.EventIdAssign;

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
    public partial class CurrentEquipmentFaultViewModel : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        public int width;

        private long dtloadtime = 0;

        public CurrentEquipmentFaultViewModel()
        {

            dtloadtime = DateTime.Now.Ticks;
            this.InitAction();
            InitEvent();
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(NavOnLoadr, 2, DelayEventHappen.EventOne);

            Wlst.Cr.Coreb.AsyncTask.Qtz.AddQtz("null", 8888, DateTime.Now.Ticks, 5, Ac);
            Wlst.Cr.Coreb.AsyncTask.Qtz.AddQtz("null", 8888, DateTime.Now.Ticks, 1, AcVoice);
            Wlst.Cr.Coreb.AsyncTask.Qtz.AddQtz("null", 8888, DateTime.Now.Ticks, 1, OneSecondAc);
    
            //th = new Thread(Run);
            //th.Start();
            if (_myself == null) _myself = this;
        }

      //  private Thread th = null;



        public event EventHandler<EventArgsRule> OnRuleChanged;

        public List<int> GetCurrentSelected()
        {
            var rtn = new List<int>();
            var lst = (from t in Records select t.FaultId).ToList();
            if (lst.Count < 5) return new List<int>();
            for (int i = 0; i < 5; i++)
            {
                if (lst[i].Count > 0) rtn.Add(i + 1);
            }
            return rtn;

        }

        private int curid = 0;

        private void Ac(object obj)
        {

            ManageInfoExist = IfManageInfoExist();
            if (ManageInfoExist) VisiManageInfoExist = 30;
            else VisiManageInfoExist = 0;

            if (VisiManageInfoExist == 30 && EquipemntLightFaultSetting.IsShowCQJandDGGH) ManageInfoExist = true;
            else ManageInfoExist = false;

            IsShowVAPHL = EquipemntLightFaultSetting.IsShowVAPHL;

            int _dtNow = DateTime.Now.Minute + DateTime.Now.Hour*60;
            var info = SaveTime();


            if (info != null)
            {
                bool load = false;
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
                if (load == false && FaultRules.Count > 0)
                {
                    if (curid != 1)
                    {
                        LoadRulesAc(FaultRules[0]);
                        curid = 1;
                    }
                }

            }
        }

        private void LoadRulesAc(CurrentItemViewModel id)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(
                                                           () =>
                                                               {
                                                                   LoadX(id);
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
                typeof (CurrentEquipmentFaultViewModel), this,true);

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxFault.wlst_fault_set_rtu_fault_level,
                UpdateFaultPriorityLevel,
                typeof(CurrentEquipmentFaultViewModel), this, true);

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxFault.wlst_fault_set_rtu_fault_pandan,
                UpdatePaidanResult, typeof(CurrentEquipmentFaultViewModel), this, true);
        }

        private void InitEvent()
        {
            EventPublish.AddEventTokener(
                Assembly.GetExecutingAssembly().GetName().ToString(), FundOrderFiltersPublishedEvent, FundOrderFilters,true
              );
        }

        public bool FundOrderFilters(PublishEventArgs args) //接收终端选中变更事件
        {
            if (args.EventId == Wlst.Ux.EquipemntLightFault.Services.EventIdAssign.EquipmentFaultIsCheckedCount)
                return true;
            if (args.EventId == EventIdAssign.EquipmentExistFaultAddId)
                return true;
            if (args.EventId == EventIdAssign.EquipementExistFaultDeleteId)
                return true;
            return false;
        }

        private Dictionary<int, LampInfo> _lampInfoList;

        private bool IfManageInfoExist()
        {
            _lampInfoList = Wlst.Sr.AssetManageInfoHold.Services.LampInfoHold.GetData();

            if (_lampInfoList.Count != 0)
            {
                return true;
            }

            return false;
        }

        private void GetCqjAndDygh(int rtuId, ref string cqj, ref string dygh)
        {
            if (ManageInfoExist == true)
            {
                foreach (var t in _lampInfoList)
                {
                    if (t.Value.RtuId == rtuId)
                    {
                        cqj = t.Value.Cqj;
                        dygh = t.Value.Dygh;

                        break;
                    }
                }
            }
        }

        private void GetVaplh(int Id, ref string V, ref string A, ref string P, ref string L, ref string H)
        {
            bool isloopError = false;

            var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(Id);

            if (error != null)
            {
                if (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 && error.Aeding < 0.0001 &&
                    error.V < 0.0001)
                {
                }
                else
                {
                    isloopError = true;
                }

                V = !isloopError ? "---" : error.V + "";
                A = !isloopError ? "---" : error.A + "";
                H = !isloopError ? "---" : error.AUpper + "";
                L = !isloopError ? "---" : error.ALower + "";
                P = !isloopError ? "---" : error.Aeding + "";
            }
            else
            {
                V = "---";
                A = "---";
                H = "---";
                L = "---";
                P = "---";
            }
        }

        public void NavOnLoadr()
        {
            PaiDanVisibility = Visibility.Collapsed;
            EnablePaiDan = false;

            var file = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read("EquipmentFaultSetting");

            if (file.ContainsKey("EnablePaidan"))
            {
                EnablePaiDan = file["EnablePaidan"].Contains("yes");
                
            }
            else EnablePaiDan = false;

            if (EnablePaiDan)
            {
                PaiDanVisibility = Visibility.Visible;
            }

            if (Sr.EquipmentInfoHolding.Services.Others.IsShowErrsCal &&
                Sr.EquipmentInfoHolding.Services.Others.IsShowTimeTableOnTime)
                FirstHappened = true;
            else FirstHappened = false;

            //LoadFaultType();
            FaultRules.Clear();
            ShowSetting = Visibility.Collapsed;

            _isAllowVoiceReport = Wlst.Sr.EquipmentInfoHolding.Services.Others.IsAllowVoiceReport;
            AllowVoice = _isAllowVoiceReport ? Visibility.Visible : Visibility.Collapsed;

            var infoitem = CurrentEquipmentFaultInfoHold.ReadRules();
            var info = infoitem.Item1;
            var type = infoitem.Item2;
            var visi = infoitem.Item3;
            //chosenFaults = info[1].ChosenFaults;

            switch (type)
            {
                case 1:
                    IsCheckedFaultType1 = true;
                    break;
                case 2:
                    IsCheckedFaultType2 = true;
                    break;
                case 3:
                    IsCheckedFaultType3 = true;
                    break;
                case 4:
                    IsCheckedFaultType4 = true;
                    break;
                case 5:
                    IsCheckedFaultType5 = true;
                    break;
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
                                       SelectedFaults = t.Value.SelectedFaults,
                                       Names = t.Value.Names
                                   });
            }
            while (FaultRules.Count < 6)
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
            FaultRules[0].IsEnable = true;


            int _dtNow = DateTime.Now.Minute + DateTime.Now.Hour*60;
            int count = 0;
            foreach (var t in SaveTime())
            {
                if (_dtNow >= t.Item2 && _dtNow <= t.Item3)
                {
                    LoadX(FaultRules[t.Item1 - 1]);
                    count++;
                }
            }
            if (count == 0) LoadX(FaultRules[0]);

            IsManageInfoExist = visi == 1 ? true : false;

        }



        private int faultsCount;
        private List<int> chosenFaults;

        public void FundOrderFiltersPublishedEvent(PublishEventArgs args)
        {

            if (args.EventId == Sr.EquipemntLightFault.Services.EventIdAssign.EquipmentExistFaultAddId)
            {
                var info = args.GetParams()[0] as List<int>;
                if (info == null || info.Count == 0) return;
             //   ErrorQueue.Enqueue(new Tuple<List<int>, bool>(info, true));


                foreach (var t in info)
                {
                    var ntgs = Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(t);
                    if (ntgs != null)
                        AddErrorInfo(ntgs, info.Count < 2);
                }
            }
            if (args.EventId == EventIdAssign.EquipementExistFaultDeleteId)
            {
                var infos = args.GetParams()[0] as List<int>;
                if (infos == null || infos.Count == 0) return;
               // ErrorQueue.Enqueue(new Tuple<List<int>, bool>(infos, false));
                 DeleteTmlFault(infos);
            }
        }

        //private void Run()
        //{
        //    while (true)
        //    {
        //        try
        //        {
        //            while (ErrorQueue.Count > 0)
        //            {
        //                Tuple<List<int>, bool> tu = null;
        //                if (ErrorQueue.TryDequeue(out tu))
        //                {
        //                    if (tu.Item2)
        //                    {
        //                        foreach (var t in tu.Item1)
        //                        {
        //                            var ntgs =
        //                                Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById
        //                                    (t);
        //                            if (ntgs != null)
        //                                AddErrorInfo(ntgs, tu.Item1.Count < 2);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        DeleteTmlFault(tu.Item1);
        //                    }
        //                }
        //            }
        //            Thread.Sleep(100);
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //    }
        //}

        //protected ConcurrentQueue<Tuple<List<int>, bool>> ErrorQueue = new ConcurrentQueue<Tuple<List<int>, bool>>();


        #region 定时器





        #endregion


    }

    //故障规则
    public partial class CurrentEquipmentFaultViewModel
    {
        private void OnRequestFault(string session, Wlst.mobile.MsgWithMobile infos)
        {                                    
        }

        private void UpdatePaidanResult(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (infos.WstFaultCurr.Op == 0)
            {
                foreach (var tt in infos.WstFaultCurr.FaultItemsAdd)
                {
                    bool find = false;

                    foreach (var t in Records)
                    {
                        foreach (var ttt in t.RecordItems)
                        {
                            if ((ttt.RtuLgcId == tt.RtuId) && (ttt.FaultId == tt.FaultId) && (ttt.LoopId == tt.LoopId))
                            {
                                find = true;

                                ttt.Paidan = "已派单";
                                ttt.OrderId = tt.PaiDan;

                                break;
                            }
                        }

                        if (find)
                        {
                            break;
                        }
                    }


                    //sr中更改派单
                    foreach (var t in Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Values)
                    {
                        if ((t.RtuId == tt.RtuId) && (t.FaultId == tt.FaultId) && (t.LoopId == tt.LoopId))
                        {
                            t.Paidan = tt.PaiDan;
                            break;
                        }
                    }
                }


            }
        }

        private void UpdateFaultPriorityLevel(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (infos.WstFaultCurr.Op == 2)
            {
                for (int i = 0; i < infos.WstFaultCurr.FaultItemsAdd.Count; i++)
                {
                    foreach (var t in Records)
                    {
                        foreach (var ttt in t.RecordItems)
                        {
                            if ((infos.WstFaultCurr.FaultItemsAdd[i].FaultId == ttt.FaultId)
                                && (infos.WstFaultCurr.FaultItemsAdd[i].LoopId == ttt.LoopId)
                                 && (infos.WstFaultCurr.FaultItemsAdd[i].RtuId == ttt.RtuLgcId))
                            {
                                t.RecordItems.Remove(ttt);
                                break;
                            }
                        }
                    }
                }

                foreach (var f in Records)
                {
                    int intFaultCountNew = 0;

                    for (int j = 0; j < f.RecordItems.Count; j++)
                    {

                        if (f.RecordItems[j].Color.Equals("#FF3030"))
                        {
                            intFaultCountNew++;
                        }
                    }

                    f.FaultCountNew = intFaultCountNew;

                    f.FaultCountNewShow = intFaultCountNew != 0 ? Visibility.Visible : Visibility.Hidden;

                    f.FaultCountOld = f.RecordItems.Count;
                }

                //sr中更改等级
                foreach (var t in Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Values)
                {

                    for (int i = 0; i < infos.WstFaultCurr.FaultItemsAdd.Count; i++)
                    {
                        if ((infos.WstFaultCurr.FaultItemsAdd[i].FaultId == t.FaultId)
                            && (infos.WstFaultCurr.FaultItemsAdd[i].LoopId == t.LoopId)
                            && (infos.WstFaultCurr.FaultItemsAdd[i].RtuId == t.RtuId))
                        {
                            t.PriorityLevel = 1;
                            break;
                        }
                    }
                }
            }
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

            var isinfo = 0;
            if (IsManageInfoExist) isinfo = 1;

            CurrentEquipmentFaultInfoHold.WriteRules(FaultRules, type, isinfo);

            int _dtNow = DateTime.Now.Minute + DateTime.Now.Hour * 60;

            int count = 0;
            var info = SaveTime();
            foreach (var t in info)
            {
                if (_dtNow >= t.Item2 && _dtNow <= t.Item3)
                {
                    LoadX(FaultRules[t.Item1 - 1]);
                    count++;
                }
            }
            if (count == 0) LoadX(FaultRules[0]); 
            
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
                foreach (var ttt in t.RecordItems) ttt.Color = "#000000";
                t.FaultCountNew = 0;
                t.FaultCountNewShow = Visibility.Collapsed;

             
                CountNewError = 0;
            }
            ClearVoiceReportItems();
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
            
            //EventPublish.PublishEvent(args);
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

        #region CmdPriorityLevelOne

        private ICommand _cmdPriorityLevelOne;

        public ICommand CmdPriorityLevelOne
        {
            get { return _cmdPriorityLevelOne ?? (_cmdPriorityLevelOne = new RelayCommand(ExCmdPriorityLevelOne, CanExCmdPriorityLevelOne, true)); }
        }


        private void ExCmdPriorityLevelOne()
        {
            int count = 0;

            var xxx = Wlst.Sr.ProtocolPhone.LxFault.wlst_fault_set_rtu_fault_level;
            xxx.WstFaultCurr.Op = 2;

            foreach (var t in Records)
            {
                foreach (var ttt in t.RecordItems)
                {
                    if (ttt.IsSelected == true)
                    {
                        var tmp = new EquipmentFaultCurr.OneFaultItem();
                        tmp.FaultId = ttt.FaultId;
                        tmp.LoopId = ttt.LoopId;
                        tmp.RtuId = ttt.RtuLgcId;
                        tmp.PriorityLevel = 3;

                        xxx.WstFaultCurr.FaultItemsAdd.Add(tmp);
                        count++;
                    }
                }
            }

            if (count != 0)
            {
                SndOrderServer.OrderSnd(xxx, 10, 6);
            }
        }

        private bool CanExCmdPriorityLevelOne()
        {
            return true;
        }

        #endregion

        #region CmdPriorityLevelTwo

        private ICommand _cmdPriorityLevelTwo;

        public ICommand CmdPriorityLevelTwo
        {
            get { return _cmdPriorityLevelTwo ?? (_cmdPriorityLevelTwo = new RelayCommand(ExCmdPriorityLevelTwo, CanExCmdPriorityLevelTwo, true)); }
        }


        private void ExCmdPriorityLevelTwo()
        {
            int count = 0;

            var xxx = Wlst.Sr.ProtocolPhone.LxFault.wlst_fault_set_rtu_fault_level;
            xxx.WstFaultCurr.Op = 2;

            foreach (var t in Records)
            {
                foreach (var ttt in t.RecordItems)
                {
                    if (ttt.IsSelected == true)
                    {
                        var tmp = new EquipmentFaultCurr.OneFaultItem();
                        tmp.FaultId = ttt.FaultId;
                        tmp.LoopId = ttt.LoopId;
                        tmp.RtuId = ttt.RtuLgcId;
                        tmp.PriorityLevel = 0;

                        xxx.WstFaultCurr.FaultItemsAdd.Add(tmp);
                        count++;
                    }
                }
            }

            if (count != 0)
            {
                SndOrderServer.OrderSnd(xxx, 10, 6);
            }
        }

        private bool CanExCmdPriorityLevelTwo()
        {
            return true;
        }

        #endregion

        #region CmdPriorityLevelThree

        private ICommand _cmdPriorityLevelThree;

        public ICommand CmdPriorityLevelThree
        {
            get { return _cmdPriorityLevelThree ?? (_cmdPriorityLevelTwo = new RelayCommand(ExCmdPriorityLevelThree, CanExCmdPriorityLevelThree, true)); }
        }


        private void ExCmdPriorityLevelThree()
        {
            int count = 0;

            var xxx = Wlst.Sr.ProtocolPhone.LxFault.wlst_fault_set_rtu_fault_level;
            xxx.WstFaultCurr.Op = 2;

            foreach (var t in Records)
            {
                foreach (var ttt in t.RecordItems)
                {
                    if (ttt.IsSelected == true)
                    {
                        var tmp = new EquipmentFaultCurr.OneFaultItem();
                        tmp.FaultId = ttt.FaultId;
                        tmp.LoopId = ttt.LoopId;
                        tmp.RtuId = ttt.RtuLgcId;
                        tmp.PriorityLevel = 1;

                        xxx.WstFaultCurr.FaultItemsAdd.Add(tmp);
                        count++;
                    }
                }
            }



            if (count != 0)
            {
                SndOrderServer.OrderSnd(xxx, 10, 6);
            }
        }

        private bool CanExCmdPriorityLevelThree()
        {
            return true;
        }

        #endregion

        #region CmdSelectAll

        private ICommand _cmdSelectAll;

        public ICommand CmdSelectAll
        {
            get { return _cmdSelectAll ?? (_cmdSelectAll = new RelayCommand(ExCmdSelectAll, CanExCmdSelectAll, true)); }
        }


        private void ExCmdSelectAll()
        {
            foreach (var t in Records)
            {
                foreach (var ttt in t.RecordItems)
                {
                    ttt.IsSelected = true;
                }
            }


        }

        private bool CanExCmdSelectAll()
        {
            return true;
        }

        #endregion

        #region CmdSelectNone

        private ICommand _cmdSelectNone;

        public ICommand CmdSelectNone
        {
            get { return _cmdSelectNone ?? (_cmdSelectNone = new RelayCommand(ExCmdSelectNone, CanExCmdSelectNone, true)); }
        }


        private void ExCmdSelectNone()
        {
            foreach (var t in Records)
            {
                foreach (var ttt in t.RecordItems)
                {
                    ttt.IsSelected = false;
                }
            }


        }

        private bool CanExCmdSelectNone()
        {
            return true;
        }

        #endregion

        #region CmdInvertSelect

        private ICommand _cmdInvertSelect;

        public ICommand CmdInvertSelect
        {
            get { return _cmdInvertSelect ?? (_cmdInvertSelect = new RelayCommand(ExCmdInvertSelect, CanExCmdInvertSelect, true)); }
        }


        private void ExCmdInvertSelect()
        {
            foreach (var t in Records)
            {
                foreach (var ttt in t.RecordItems)
                {
                    ttt.IsSelected = !ttt.IsSelected;
                }
            }


        }

        private bool CanExCmdInvertSelect()
        {
            return true;
        }

        #endregion

        #region CmdTest

        private ICommand _cmdTest;

        public ICommand CmdTest
        {
            get { return _cmdTest ?? (_cmdTest = new RelayCommand(ExCmdTest, CanExCmdTest, true)); }
        }


        private void ExCmdTest()
        {

            var xxx = Wlst.Sr.ProtocolPhone.LxFault.wlst_fault_set_rtu_fault_level;
            xxx.WstFaultCurr.Op = 1;
            Wlst.Sr.PPPandSocketSvr.Server.SocketClient.SndData(xxx);

        }

        private bool CanExCmdTest()
        {
            return true;
        }

        #endregion 

        #region 语音报警


        private Visibility _allowVoice;
        /// <summary>
        /// 是否允许语音报警，隐藏暂停语音报警按钮
        /// </summary>
        public Visibility AllowVoice
        {
            get { return _allowVoice; }
            set
            {
                if (_allowVoice != value)
                {
                    _allowVoice = value;
                    this.RaisePropertyChanged(() => this.AllowVoice);
                }
            }
        }

        private static bool _isVoiceReport = true;

        private static bool _isAllowVoiceReport ;
         
        private static ConcurrentQueue<string> _voiceReportItems = new ConcurrentQueue<string>();

        private SpVoice _voice =new SpVoice();

        void AcVoice(object obj)
        {
            try
            {


                SpeechVoiceSpeakFlags SpFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync ;//.SVSFDefault; //.SVSFlagsAsync;
                _voice = new SpVoice();

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
                                _voice.Speak(speaktext, SpFlags);
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

            _voice.Pause();
            _voice = new SpVoice();

            while (_voiceReportItems.Count > 0)
                _voiceReportItems.TryDequeue(out speaktext);

        }

        #endregion

               
        #region CmdSendOrder

        private ICommand _cmdSendOrder;

        public ICommand CmdSendOrder
        {
            get { return _cmdSendOrder ?? (_cmdSendOrder = new RelayCommand(ExCmdSendOrder, CanExCmdSendOrder, true)); }
        }


        private void ExCmdSendOrder()
        {
            SendOrderViewModel.ViewModel.SendOrderViewModel.FaultRecord.Clear();

            foreach (var t in Records)
            {
                foreach (var ttt in t.RecordItems)
                {
                    if (ttt.IsSelected == true)
                    {

                        var inputFaultInfo = new InputFaultRecord
                                            {
                                                RtuId = ttt.RtuLgcId,
                                                LoopID = ttt.LoopId,
                                                PriorityLevel = ttt.PriorityLevel,
                                                FaultName = ttt.FaultName,
                                                FaultID = ttt.FaultId,
                                                RtuName = ttt.RtuName,
                                                Time = ttt.DateCreate,
                                                OrderID = string.IsNullOrEmpty(ttt.OrderId) ? 0 : Convert.ToUInt64(ttt.OrderId)
                                            };

                        SendOrderViewModel.ViewModel.SendOrderViewModel.FaultRecord.Add(inputFaultInfo);
                    }
                }
            }

            if (SendOrderViewModel.ViewModel.SendOrderViewModel.FaultRecord.Count != 0)
            {
                RegionManage.ShowViewByIdAttachRegionWithArgu(EquipemntLightFault.Services.ViewIdAssign.SendOrderViewId, 0);
            }
        }

        private bool CanExCmdSendOrder()
        {
            return true;
        }

        #endregion

        //#region CmdSendOrder

        //private ICommand _cmdSendOrder;

        //public ICommand CmdSendOrder
        //{
        //    get { return _cmdSendOrder ?? (_cmdTest = new RelayCommand(ExCmdSendOrder, CanExCmdSendOrder, true)); }
        //}


        //public static Views.SendOrder SendOrderView = null;

        //private string Return_PriorityLevel_Desc(int PriorityLevel)
        //{
        //    if(PriorityLevel == 3)
        //    {
        //        return "急修单";
        //    }

        //    return "维修单";
        //}

        //private bool Return_PriorityLevel_Enable(int PriorityLevel)
        //{
        //    if (PriorityLevel == 3)
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        //private static string Return_Status_Desc(bool _status)
        //{
        //    if (_status == false)
        //    {
        //        return "未派单";
        //    }

        //    return "已派单";
        //}

        //private string Get_Four_Number(int x)
        //{
        //    string _num = Convert.ToString(x);

        //    int _len = _num.Length;

        //    if(_num.Length <= 4)
        //    {
        //        for (int i = 0; i < 4 - _len; i++)
        //        {
        //            _num = "0" + _num;
        //        }
        //    }
        //    else
        //    {
        //        _num = "0001";
        //    }

        //    return _num;
        //}

        //private string Get_Fault_ID()
        //{
        //    string dir = Directory.GetCurrentDirectory() + "\\SystemXmlConfig";
        //    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        //    string path = dir + "\\" + "FaultID.txt";

        //    try
        //    {

        //        if (File.Exists(path))
        //        {
        //            StreamReader sr = new StreamReader(path, Encoding.Default);

        //            String line = sr.ReadLine();

        //            sr.Close();


        //            string[] value = line.Split(',');

        //            if (value.Length == 2)
        //            {
        //                if (value[0] == DateTime.Now.ToString("yyyyMMdd"))
        //                {
        //                    if (value[1].Length == 4)
        //                    {
        //                        int x = Convert.ToInt32(value[1]);

        //                        x++;

        //                        if(x > 9999)
        //                        {
        //                            x = 1;
        //                        }

        //                        string _num = Get_Four_Number(x);

    
        //                        File.Delete(path);
        //                        Wlst.Ux.EquipemntLightFault.Services.fileread.Write(dir + "\\" + "FaultID.txt", DateTime.Now.ToString("yyyyMMdd") + "," + _num);

        //                        return DateTime.Now.ToString("yyyyMMdd") + _num;
        //                    }
        //                }

        //            }
        //        }

        //        if (File.Exists(path))  File.Delete(path);

        //        Wlst.Ux.EquipemntLightFault.Services.fileread.Write(dir + "\\" + "FaultID.txt", DateTime.Now.ToString("yyyyMMdd") + ",0001");

        //        return DateTime.Now.ToString("yyyyMMdd") + "0001";
        //    }
        //    catch (Exception)
        //    {

        //        if (File.Exists(path))  File.Delete(path);

        //        Wlst.Ux.EquipemntLightFault.Services.fileread.Write(path, DateTime.Now.ToString("yyyyMMdd") + ",0001");

        //        return DateTime.Now.ToString("yyyyMMdd") + "0001";
        //    }

            
        //}

        //public static ObservableCollection<SendOrderItems> AllSendOrderList = new ObservableCollection<SendOrderItems>();
        //public static SendOrderItems CurrentSendOrderItem = new SendOrderItems();
        //public static int CurrentSendOrderIndex = 0;

        //private void ExCmdSendOrder()
        //{
        //    SendOrderViewModel.ViewModel.SendOrderViewModel.FaultRecord.Clear();

        //    foreach (var t in Records)
        //    {
        //        foreach (var ttt in t.RecordItems)
        //        {
        //            if (ttt.IsSelected == true)
        //            {

        //                var InputFaultInfo = new InputFaultRecord
        //                                    {
        //                                        RtuId = ttt.RtuLgcId,
        //                                        PriorityLevel = ttt.PriorityLevel,
        //                                        FaultName = ttt.FaultName,
        //                                        RtuName = ttt.RtuName
        //                                    };

        //                SendOrderViewModel.ViewModel.SendOrderViewModel.FaultRecord.Add(InputFaultInfo);
        //            }
        //        }
        //    }
            
        //    if(SendOrderViewModel.ViewModel.SendOrderViewModel.FaultRecord.Count != 0)
        //    {
        //        RegionManage.ShowViewByIdAttachRegionWithArgu(EquipemntLightFault.Services.ViewIdAssign.SendOrderViewId, 0);
        //    }
        //    //AllSendOrderList.Clear();

        //    //int m = 0;

        //    //foreach (var t in Records)
        //    //{
        //    //    foreach (var ttt in t.RecordItems)
        //    //    {
        //    //        if (ttt.IsSelected == true)
        //    //        {
        //    //            var tmp = ServicesGrpSingleInfoHold.GetRtuBelongGrp(ttt.RtuLgcId);

        //    //            var grpName =
        //    //                (ServicesGrpSingleInfoHold.InfoGroups[new Tuple<int, int>(tmp.Item1, tmp.Item2)]).GroupName;

        //    //            var orderInfo = new SendOrderItems
        //    //                                {
        //    //                                    Id = m++,
        //    //                                    RtuId = ttt.RtuLgcId,
        //    //                                    AdminName = UserInfo.UserLoginInfo.UserName,
        //    //                                    MergencyLocation = string.Empty,
        //    //                                    MergencyLocationEnable = Return_PriorityLevel_Enable(ttt.PriorityLevel),
        //    //                                    OrderFaultId = Get_Fault_ID(),
        //    //                                    OrderFaultName = ttt.FaultName,
        //    //                                    OrderTime = string.Empty,
        //    //                                    PriorityLevel = Return_PriorityLevel_Desc(ttt.PriorityLevel),
        //    //                                    RepairContent = string.Empty,
        //    //                                    RtuGroup = grpName,
        //    //                                    RtuName = ttt.RtuName,
        //    //                                    Status = Return_Status_Desc(false)
        //    //                                };

        //    //            AllSendOrderList.Add(orderInfo);
        //    //        }
        //    //    }
        //    //}

        //    //if (AllSendOrderList.Count != 0)
        //    //{
        //    //    CurrentSendOrderIndex = 0;
        //    //    CurrentSendOrderItem = AllSendOrderList[CurrentSendOrderIndex];

        //    //    if (SendOrderView != null)
        //    //    {
        //    //        SendOrderView.Close();
        //    //    }

        //    //    var xxxx = new SendOrderView { SendOrderList = AllSendOrderList, CurrentSendOrder = CurrentSendOrderItem, MessageShow = string.Empty };

        //    //    SendOrderView = new SendOrder();
        //    //    SendOrderView.Width = 1020;
        //    //    SendOrderView.Height = 480;
        //    //    SendOrderView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        //    //    SendOrderView.SetContext(xxxx);
        //    //    SendOrderView.Show();
        //    //}
        //}

        //private bool CanExCmdSendOrder()
        //{
        //    return true;
        //}

        //#endregion

        //private static string SendDatatoWebServices()
        //{
        //    var test = new WebService1();
        //    var data = new WebService1.WsErrorModel()
        //                   {
        //                       ErrorId = CurrentSendOrderItem.OrderFaultId,
        //                       PaperId = CurrentSendOrderItem.PriorityLevel,
        //                       Facility = CurrentSendOrderItem.RtuGroup,
        //                       Content = CurrentSendOrderItem.RepairContent,
        //                       Manager = CurrentSendOrderItem.AdminName,
        //                       Time = CurrentSendOrderItem.OrderTime,
        //                       Location = CurrentSendOrderItem.MergencyLocation,
        //                       Tml = CurrentSendOrderItem.RtuName
        //                   };
        //    return test.SendData(data);
        //}

        //public static int SendOrderIndex = 0;
        //public static void SendOrder()
        //{
        //    SendOrderIndex = CurrentSendOrderIndex;
        //    CurrentSendOrderItem.OrderTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

        //    string sendDatatoWebServices = SendDatatoWebServices();

        //    if (sendDatatoWebServices == "发送成功")
        //    {


        //        CurrentSendOrderItem = AllSendOrderList[SendOrderIndex];
        //        CurrentSendOrderItem.Status = Return_Status_Desc(true);
        //        CurrentSendOrderIndex++;

        //        if (CurrentSendOrderIndex >= AllSendOrderList.Count)
        //        {
        //            CurrentSendOrderIndex = AllSendOrderList.Count - 1;
        //        }

        //        CurrentSendOrderItem = AllSendOrderList[CurrentSendOrderIndex];


        //    }
        //    else
        //    {
        //        CurrentSendOrderItem = AllSendOrderList[SendOrderIndex];
        //        CurrentSendOrderItem.OrderTime = string.Empty;
        //    }

        //    var xxxx = new SendOrderView { SendOrderList = AllSendOrderList, CurrentSendOrder = CurrentSendOrderItem, MessageShow = sendDatatoWebServices };
        //    SendOrderView.SetContext(xxxx);

        //}

        //public static void SelectCurrentOrder(int _index)
        //{
        //    CurrentSendOrderIndex = _index;
        //    CurrentSendOrderItem = AllSendOrderList[CurrentSendOrderIndex];

        //    var xxxx = new SendOrderView { SendOrderList = AllSendOrderList, CurrentSendOrder = CurrentSendOrderItem, MessageShow = string.Empty };
        //    SendOrderView.SetContext(xxxx);
        //}
    


    
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
            //if (isLeft == false)  
            //{
                Sr.EquipemntLightFault.Services.PreErrorServices.RequestDataWhenErrorHappen(info.RtuLgcId,info.LoopId,
                                                                                            info.DateCreate);


            //}
            //发布事件  选中当前节点
            var args = new PublishEventArgs
            {
                EventType = PublishEventType.Core,
                EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
            };
            args.AddParams(info.RtuLgcId);
            EventPublish.PublishEvent(args);
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

        private bool _firsthappened;
        /// <summary>
        /// 第一次发生时间
        /// </summary>
        public bool FirstHappened
        {
            get { return _firsthappened; }
            set
            {
                if (_firsthappened != value)
                {
                    _firsthappened = value;
                    this.RaisePropertyChanged(() => this.FirstHappened);
                }
            }
        }

        #region ManageInfoExist

        private bool _manageInfoExist = false;
        /// <summary>
        /// 资产管理信息存在
        /// </summary>
        public bool ManageInfoExist
        {
            get { return _manageInfoExist; }
            set
            {
                if (value == _manageInfoExist) return;
                _manageInfoExist = value;
                RaisePropertyChanged(() => ManageInfoExist);
            }
        }

        #endregion

        #region IsManageInfoExist

        private bool _ismanageInfoExist;
        /// <summary>
        /// 资产管理信息是否显示
        /// </summary>
        public bool IsManageInfoExist
        {
            get { return _ismanageInfoExist; }
            set
            {
                if (value == _ismanageInfoExist) return;
                _ismanageInfoExist = value;
                RaisePropertyChanged(() => IsManageInfoExist);
            }
        }

        #endregion


        private bool _enablePaiDan;
        /// <summary>
        /// 派单显示
        /// </summary>
        public bool EnablePaiDan
        {
            get { return _enablePaiDan; }
            set
            {
                _enablePaiDan = value;
                RaisePropertyChanged(() => EnablePaiDan);
            }
        }

        private Visibility _paiDanVisibility;
        /// <summary>
        /// 派单可见
        /// </summary>
        public Visibility PaiDanVisibility
        {
            get { return _paiDanVisibility; }
            set
            {
                _paiDanVisibility = value;
                RaisePropertyChanged(() => PaiDanVisibility);
            }
        }

        private bool _isShowVAPHL;
        /// <summary>
        /// 是否显示电压电流功率上下限
        /// </summary>
        public bool IsShowVAPHL
        {
            get { return _isShowVAPHL; }
            set
            {
                _isShowVAPHL = value;
                RaisePropertyChanged(() => IsShowVAPHL);
            }
        }


        #region VisiManageInfoExist

        private int _visimanageInfoExist = 0;
        /// <summary>
        /// 资产管理信息是否存在
        /// </summary>
        public int VisiManageInfoExist
        {
            get { return _visimanageInfoExist; }
            set
            {
                if (value == _visimanageInfoExist) return;
                _visimanageInfoExist = value;
                RaisePropertyChanged(() => VisiManageInfoExist);
            }
        }

        #endregion
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
                EventPublish.PublishEvent(argss);
            }
        }

        private string bkctmp = "";
        private void AddErrorInfo(FaultInfoBase error, bool isNewAdd)
        {
            if (error.PriorityLevel == 1)
                return;

            // var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(id);
            if (error == null) return;

            string cqj = string.Empty;
            string dygh = string.Empty;

            GetCqjAndDygh(error.RtuId, ref cqj, ref dygh);

            var infovm = new FaultRecordViewModel
                             {
                                 DataCreateTime = error.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
                                 RtuId = error.RtuPhyId,
                                 RtuLgcId = error.RtuId,
                                 CQJ = cqj,
                                 DYGH = dygh,
                                 RtuName = error.RtuName,
                                 LoopName = error.RtuLoopName,
                                 FaultId = error.FaultId,
                                 FaultName = error.FaultName,
                                 Id = error.Id,
                                 LoopId = error.LoopId,
                                 DateCreate = error.DateCreate.Ticks,
                                 Color = isNewAdd ? "#FF3030" : "#000000",
                                 AlarmCount = error.AlarmCount,
                                 DataFirstTime =
                                     error.DateFirst.Ticks < 1 ? "--" : error.DateFirst.ToString("yyyy-MM-dd HH:mm:ss"),
                                 Voltage =
                                     (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                      error.Aeding < 0.0001 && error.V < 0.0001)
                                         ? "--"
                                         : error.V.ToString("f2") ,//(CultureInfo.InvariantCulture),
                                 Current =
                                     (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                      error.Aeding < 0.0001 && error.V < 0.0001)
                                         ? "--"
                                         : error.A.ToString("f2") ,//(CultureInfo.InvariantCulture),
                                 Power =
                                     (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                      error.Aeding < 0.0001 && error.V < 0.0001)
                                         ? "--"
                                         : error.Aeding.ToString("f2"),//(CultureInfo.InvariantCulture),
                                 HighLimit =
                                     (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                      error.Aeding < 0.0001 && error.V < 0.0001)
                                         ? "--"
                                         : error.AUpper.ToString("f2"),//(CultureInfo.InvariantCulture),
                                 LowLimit =
                                     (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                      error.Aeding < 0.0001 && error.V < 0.0001)
                                         ? "--"
                                         : error.ALower.ToString("f2"),//(CultureInfo.InvariantCulture),
                                 PriorityLevel = error.PriorityLevel,
                                 Paidan = string.IsNullOrEmpty(error.Paidan) ? "" : "已派单",
                                 OrderId = error.Paidan
                             };
            int count = 0;
            if (Records.Count != 0)
            {
                foreach (var t in Records)
                {
                    if (t.FaultId.Contains(infovm.FaultId))
                    {
                        if (isNewAdd)
                            t.RecordItems.Insert(0, infovm);
                        else t.RecordItems.Add(infovm);

                        count++;
                        if (isNewAdd) t.FaultCountNew += 1;
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
            foreach (var g in Records)
            {
                tmp += g.FaultCountNew;
            }
            CountNewError = tmp;

            if (_isAllowVoiceReport && _isVoiceReport && startStopVoice == false && isNewAdd)
            {

                int alarmNum = 0;
                string temp;
                while (error.AlarmTimes > alarmNum)
                {
                    string tt = error.FaultName;
                    if (tt.Contains("防盗")) tt = "被盗";
                    if (tt.Contains("门")) tt = "打开";

                    string rtuName = error.RtuName;

                    if (rtuName.Contains("*"))
                    {
                        temp = rtuName.Substring(0, rtuName.IndexOf('*')) + error.RtuLoopName + "        " + tt;
                    }
                    else
                    {
                        temp = rtuName + error.RtuLoopName + "        " + tt;
                    }

                    _voiceReportItems.Enqueue(temp);
                    alarmNum++;
                }
            }

            if (isNewAdd)
            {

                if (Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowThisViewOnNewErrArriveInfo) //选项中设定
                    //Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryViewModel.ViewModel.
                    //    EquipmentFaultRecordQueryViewModel.IsShowThisViewOnNewErrArriveInfo)
                {
                    // NavToCurrentEquipmentFaultView.ShowView();

                    Wlst.Cr.Core.CoreServices.RegionManage.DispatcherInvoke(NavToCurrentEquipmentFaultViewShowView,
                                                                            null);
                }

            }

        }

        void NavToCurrentEquipmentFaultViewShowView(object obj)
        {
            NavToCurrentEquipmentFaultView.ShowView();
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



        private int  firsload = 0;
        private void LoadX(CurrentItemViewModel rule)
        {
            bool noNeedCalError = false ;
            firsload++;
            if (firsload < 4 && DateTime.Now.Ticks - dtloadtime < 60 * 10000000L) 
                noNeedCalError = true;
                 //在主界面 存在最新故障第一次加载的时候 显示了所有故障的总数 此处做预防措施
                


            var faultId1 = rule.SelectedFaults[1]; // .Id ;
            var faultId2 = rule.SelectedFaults[2];
            var faultId3 = rule.SelectedFaults[3];
            var faultId4 = rule.SelectedFaults[4];
            var faultId5 = rule.SelectedFaults[5];

            string faultName1 = rule.Names[0].Name;
            string faultName2 = rule.Names[1].Name;
            string faultName3 = rule.Names[2].Name;
            string faultName4 = rule.Names[3].Name;
            string faultName5 = rule.Names[4].Name;

            if (OnRuleChanged != null)
            {
                OnRuleChanged(this,
                              new EventArgsRule(faultId1.Count > 0, faultId2.Count > 0, faultId3.Count > 0,
                                                faultId4.Count > 0, faultId5.Count > 0));
            }


            var lst = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Values;

            //查阅当前故障 中最新故障 用户未点击已查看的  lvf  2018年6月29日11:12:41  下面源代码报错
            var lstIds = new List<int>();
            for (int i = Records.Count-1; i >=0;i-- )
            {
                for(int j =Records[i].RecordItems.Count-1 ; j>=0;j--)
                {
                    if (Records[i].RecordItems[j].Color.Equals("#FFFFFF")) continue;
                    if (noNeedCalError)
                    {
                        Records[i].RecordItems[j].Color = "#FFFFFF";
                        continue;
                    }
                    lstIds.Add(Records[i].RecordItems[j].Id);
                }

            }

            //foreach (var f in Records)
            //    {
            //        foreach (var g in f.RecordItems)
            //        {
            //            if (g.Color.Equals("#FFFFFF")) continue;
            //            if (noNeedCalError)
            //            {
            //                g.Color = "#FFFFFF";
            //                continue;
            //            }
            //            lstIds.Add(g.Id);
            //        }
            //    }
            Records.Clear();
            var obs = new List<FaultRecordViewModel>();
            int intx = 0;
            if (IsCheckedFaultType1 == true)
            {
                int rtuIdtmp = 0;
                foreach (var t in lst)
                {
                    var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(t.Id);
                    if (error == null) continue;
                    intx++;

                    if (error.PriorityLevel != 1)
                    {
                        string cqj = string.Empty;
                        string dygh = string.Empty;

                        GetCqjAndDygh(t.RtuId, ref cqj, ref dygh);

                        obs.Add(new FaultRecordViewModel
                                    {
                                        DataCreateTime = t.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
                                        RtuId = t.RtuPhyId,
                                        RtuLgcId = t.RtuId,
                                        CQJ = cqj,
                                        DYGH = dygh,
                                        LoopId = error.LoopId,
                                        DateCreate = error.DateCreate.Ticks,
                                        RtuName = t.RtuName,
                                        LoopName = t.RtuLoopName,
                                        FaultId = t.FaultId,
                                        FaultName = t.FaultName,
                                        Id = t.Id,
                                        Color = lstIds.Contains(t.Id) ? "#FF3030" : "#000000",
                                        AlarmCount = error.AlarmCount,
                                        DataFirstTime =
                                            error.DateFirst.Ticks < 1
                                                ? "--"
                                                : error.DateFirst.ToString("yyyy-MM-dd HH:mm:ss"),
                                        Voltage =
                                            (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                             error.Aeding < 0.0001 && error.V < 0.0001)
                                                ? "--"
                                                : error.V.ToString("f2"),//(CultureInfo.InvariantCulture),
                                        Current =
                                            (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                             error.Aeding < 0.0001 && error.V < 0.0001)
                                                ? "--"
                                                : error.A.ToString("f2"),//(CultureInfo.InvariantCulture),
                                        Power =
                                            (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                             error.Aeding < 0.0001 && error.V < 0.0001)
                                                ? "--"
                                                : error.Aeding.ToString("f2"),//(CultureInfo.InvariantCulture),
                                        HighLimit =
                                            (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                             error.Aeding < 0.0001 && error.V < 0.0001)
                                                ? "--"
                                                : error.AUpper.ToString("f2"),//(CultureInfo.InvariantCulture),
                                        LowLimit =
                                            (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                             error.Aeding < 0.0001 && error.V < 0.0001)
                                                ? "--"
                                                : error.ALower.ToString("f2"),//(CultureInfo.InvariantCulture),
                                        PriorityLevel = error.PriorityLevel,
                                        Paidan = string.IsNullOrEmpty(error.Paidan) ? "" : "已派单",
                                        OrderId = error.Paidan
                                    });
                        rtuIdtmp = t.RtuId;
                    }
                }
            }
            if (IsCheckedFaultType2 == true)
            {
                foreach (var t in lst)
                {
                    var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(t.Id);
                    if (error == null) continue;
                    intx++;

                    if (error.PriorityLevel != 1)
                    {
                        if (t.DateCreate.Day == DateTime.Now.Day && t.DateCreate.Month == DateTime.Now.Month &&
                            t.DateCreate.Year == DateTime.Now.Year)
                        {
                            string cqj = string.Empty;
                            string dygh = string.Empty;

                            GetCqjAndDygh(t.RtuId, ref cqj, ref dygh);

                            obs.Add(new FaultRecordViewModel
                                        {
                                            DataCreateTime = t.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
                                            RtuId = t.RtuPhyId,
                                            RtuLgcId = t.RtuId,
                                            CQJ = cqj,
                                            DYGH = dygh,
                                            LoopId = error.LoopId,
                                            DateCreate = error.DateCreate.Ticks,
                                            RtuName = t.RtuName,
                                            LoopName = t.RtuLoopName,
                                            FaultId = t.FaultId,
                                            FaultName = t.FaultName,
                                            Id = t.Id,
                                            Color = lstIds.Contains(t.Id) ? "#FF3030" : "#000000",
                                            AlarmCount = error.AlarmCount,
                                            DataFirstTime =
                                                error.DateFirst.Ticks < 1
                                                    ? "--"
                                                    : error.DateFirst.ToString("yyyy-MM-dd HH:mm:ss"),
                                            Voltage =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.V.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            Current =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.A.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            Power =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.Aeding.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            HighLimit =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.AUpper.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            LowLimit =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.ALower.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            PriorityLevel = error.PriorityLevel,
                                            Paidan = string.IsNullOrEmpty(error.Paidan) ? "" : "已派单",
                                            OrderId = error.Paidan
                                        });
                        }
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

                    if (error.PriorityLevel != 1)
                    {
                        if (t.DateCreate >= DateTime.Now.AddDays(-1))
                        {
                            string cqj = string.Empty;
                            string dygh = string.Empty;

                            GetCqjAndDygh(t.RtuId, ref cqj, ref dygh);

                            obs.Add(new FaultRecordViewModel
                                        {
                                            DataCreateTime = t.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
                                            RtuId = t.RtuPhyId,
                                            RtuLgcId = t.RtuId,
                                            CQJ = cqj,
                                            DYGH = dygh,
                                            LoopId = error.LoopId,
                                            DateCreate = error.DateCreate.Ticks,
                                            RtuName = t.RtuName,
                                            LoopName = t.RtuLoopName,
                                            FaultId = t.FaultId,
                                            FaultName = t.FaultName,
                                            Id = t.Id,
                                            Color = lstIds.Contains(t.Id) ? "#FF3030" : "#000000",
                                            AlarmCount = error.AlarmCount,
                                            DataFirstTime =
                                                error.DateFirst.Ticks < 1
                                                    ? "--"
                                                    : error.DateFirst.ToString("yyyy-MM-dd HH:mm:ss"),
                                            Voltage =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.V.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            Current =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.A.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            Power =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.Aeding.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            HighLimit =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.AUpper.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            LowLimit =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.ALower.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            PriorityLevel = error.PriorityLevel,
                                            Paidan = string.IsNullOrEmpty(error.Paidan) ? "" : "已派单",
                                            OrderId = error.Paidan
                                        });
                        }
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

                    if (error.PriorityLevel != 1)
                    {
                        if (t.DateCreate >= DateTime.Now.AddDays(-2))
                        {
                            string cqj = string.Empty;
                            string dygh = string.Empty;

                            GetCqjAndDygh(t.RtuId, ref cqj, ref dygh);

                            obs.Add(new FaultRecordViewModel
                                        {
                                            DataCreateTime = t.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
                                            RtuId = t.RtuPhyId,
                                            RtuLgcId = t.RtuId,
                                            CQJ = cqj,
                                            DYGH = dygh,
                                            LoopId = error.LoopId,
                                            DateCreate = error.DateCreate.Ticks,
                                            RtuName = t.RtuName,
                                            LoopName = t.RtuLoopName,
                                            FaultId = t.FaultId,
                                            FaultName = t.FaultName,
                                            Id = t.Id,
                                            Color = lstIds.Contains(t.Id) ? "#FF3030" : "#000000",
                                            AlarmCount = error.AlarmCount,
                                            DataFirstTime =
                                                error.DateFirst.Ticks < 1
                                                    ? "--"
                                                    : error.DateFirst.ToString("yyyy-MM-dd HH:mm:ss"),
                                            Voltage =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.V.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            Current =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.A.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            Power =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.Aeding.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            HighLimit =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.AUpper.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            LowLimit =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.ALower.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            PriorityLevel = error.PriorityLevel,
                                            Paidan = string.IsNullOrEmpty(error.Paidan) ? "" : "已派单",
                                            OrderId = error.Paidan
                                        });
                        }
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

                    if (error.PriorityLevel != 1)
                    {
                        if (t.DateCreate >= DateTime.Now.AddHours(-6))
                        {
                            string cqj = string.Empty;
                            string dygh = string.Empty;

                            GetCqjAndDygh(t.RtuId, ref cqj, ref dygh);

                            obs.Add(new FaultRecordViewModel
                                        {
                                            DataCreateTime = t.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
                                            RtuId = t.RtuPhyId,
                                            RtuLgcId = t.RtuId,
                                            CQJ = cqj,
                                            DYGH = dygh,
                                            LoopId = error.LoopId,
                                            DateCreate = error.DateCreate.Ticks,
                                            RtuName = t.RtuName,
                                            LoopName = t.RtuLoopName,
                                            FaultId = t.FaultId,
                                            FaultName = t.FaultName,
                                            Id = t.Id,
                                            Color = lstIds.Contains(t.Id) ? "#FF3030" : "#000000",
                                            AlarmCount = error.AlarmCount,
                                            DataFirstTime =
                                                error.DateFirst.Ticks < 1
                                                    ? "--"
                                                    : error.DateFirst.ToString("yyyy-MM-dd HH:mm:ss"),
                                            Voltage =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.V.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            Current =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.A.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            Power =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.Aeding.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            HighLimit =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.AUpper.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            LowLimit =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.ALower.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            PriorityLevel = error.PriorityLevel,
                                            Paidan = string.IsNullOrEmpty(error.Paidan) ? "" : "已派单",
                                            OrderId = error.Paidan
                                        });
                        }
                    }
                }
            }




            var list1 =
                (from t in obs where faultId1.Contains(t.FaultId) orderby t.DateCreate descending select t).ToList();
            var list2 =
                (from t in obs where faultId2.Contains(t.FaultId) orderby t.DateCreate descending select t).ToList();
            var list3 =
                (from t in obs where faultId3.Contains(t.FaultId) orderby t.DateCreate descending select t).ToList();
            var list4 =
                (from t in obs where faultId4.Contains(t.FaultId) orderby t.DateCreate descending select t).ToList();
            var list5 =
                (from t in obs where faultId5.Contains(t.FaultId) orderby t.DateCreate descending select t).ToList();

            var tmp = new List<int>();
            tmp.AddRange(faultId1);
            tmp.AddRange(faultId2);
            tmp.AddRange(faultId3);
            tmp.AddRange(faultId4);
            tmp.AddRange(faultId5);
            var list6 = (from t in obs where !tmp.Contains(t.FaultId) orderby t.DateCreate descending select t).ToList();
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


            Records.Add(new AllFaultRecordsViewModel()
                            {FaultName = faultName1, FaultId = faultId1, RecordItems = collection1});
            Records.Add(new AllFaultRecordsViewModel()
                            {FaultName = faultName2, FaultId = faultId2, RecordItems = collection2});
            Records.Add(new AllFaultRecordsViewModel()
                            {FaultName = faultName3, FaultId = faultId3, RecordItems = collection3});
            Records.Add(new AllFaultRecordsViewModel()
                            {FaultName = faultName4, FaultId = faultId4, RecordItems = collection4});
            Records.Add(new AllFaultRecordsViewModel()
                            {FaultName = faultName5, FaultId = faultId5, RecordItems = collection5});
            if (faultName1 == faultName2 && faultName1 == faultName3 && faultName1 == faultName4
                && faultName1 == faultName5)
            {
                Records.Add(new AllFaultRecordsViewModel()
                                {FaultName = "所有故障", IsOther = true, RecordItems = collection6});

            }
            else
            {
                Records.Add(new AllFaultRecordsViewModel()
                                {FaultName = "其他故障", IsOther = true, RecordItems = collection6});
            }


            int tmpCr = 0;
            foreach (var f in Records)
            {
                f.FaultCountNew = (from t in f.RecordItems where t.Color.Equals("#FF3030") select t).Count();
                f.FaultCountNewShow = f.FaultCountNew == 0 ? Visibility.Collapsed : Visibility.Visible;
                f.FaultCountOld = f.RecordItems.Count;

                tmpCr += f.FaultCountNew;
            }

       
                CountNewError = tmpCr;

          
        }

        private void LoadTimePeriod(int type)
        {
            var faultLst = new List<Tuple<List< int >, string>>();

            if (Records.Count == 0) return;
            for (int i = 0; i < 6; i++)
            {
                faultLst.Add(new Tuple<List<int>, string>(Records[i].FaultId, Records[i].FaultName));
            }

            //查阅当前故障 中最新故障 用户未点击已查看的  
            var lstIds = new List<int>();
            foreach (var f in Records)
            {
                foreach (var g in f.RecordItems)
                {
                    if (g.Color.Equals("#000000")) continue;
                    lstIds.Add(g.Id);
                }
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

                    if (error.PriorityLevel != 1)
                    {
                        string cqj = string.Empty;
                        string dygh = string.Empty;

                        GetCqjAndDygh(t.RtuId, ref cqj, ref dygh);

                        obs.Add(new FaultRecordViewModel
                                    {
                                        DataCreateTime = t.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
                                        RtuId = t.RtuPhyId,
                                        RtuLgcId = t.RtuId,
                                        CQJ = cqj,
                                        DYGH = dygh,
                                        LoopId = error.LoopId,
                                        DateCreate = error.DateCreate.Ticks,
                                        RtuName = t.RtuName,
                                        LoopName = t.RtuLoopName,
                                        FaultId = t.FaultId,
                                        FaultName = t.FaultName,
                                        Id = t.Id,
                                        Color = lstIds.Contains(t.Id) ? "#FF3030" : "#000000",
                                        AlarmCount = error.AlarmCount,
                                        DataFirstTime =
                                            error.DateFirst.Ticks < 1
                                                ? "--"
                                                : error.DateFirst.ToString("yyyy-MM-dd HH:mm:ss"),
                                        Voltage =
                                            (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                             error.Aeding < 0.0001 && error.V < 0.0001)
                                                ? "--"
                                                : error.V.ToString("f2"),//(CultureInfo.InvariantCulture),
                                        Current =
                                            (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                             error.Aeding < 0.0001 && error.V < 0.0001)
                                                ? "--"
                                                : error.A.ToString("f2"),//(CultureInfo.InvariantCulture),
                                        Power =
                                            (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                             error.Aeding < 0.0001 && error.V < 0.0001)
                                                ? "--"
                                                : error.Aeding.ToString("f2"),//(CultureInfo.InvariantCulture),
                                        HighLimit =
                                            (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                             error.Aeding < 0.0001 && error.V < 0.0001)
                                                ? "--"
                                                : error.AUpper.ToString("f2"),//(CultureInfo.InvariantCulture),
                                        LowLimit =
                                            (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                             error.Aeding < 0.0001 && error.V < 0.0001)
                                                ? "--"
                                                : error.ALower.ToString("f2"),//(CultureInfo.InvariantCulture),
                                        PriorityLevel = error.PriorityLevel,
                                        Paidan = string.IsNullOrEmpty(error.Paidan) ? "" : "已派单",
                                        OrderId = error.Paidan
                                    });
                    }
                }
            }
            if (type == 2)
            {
                foreach (var t in lst)
                {
                    var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(t.Id);
                    if (error == null) continue;
                    intx++;
                    if (error.PriorityLevel != 1)
                    {
                        if (t.DateCreate.Day == DateTime.Now.Day && t.DateCreate.Month == DateTime.Now.Month &&
                            t.DateCreate.Year == DateTime.Now.Year)
                        {
                            string cqj = string.Empty;
                            string dygh = string.Empty;

                            GetCqjAndDygh(t.RtuId, ref cqj, ref dygh);

                            obs.Add(new FaultRecordViewModel
                                        {
                                            DataCreateTime = t.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
                                            RtuId = t.RtuPhyId,
                                            RtuLgcId = t.RtuId,
                                            CQJ = cqj,
                                            DYGH = dygh,
                                            RtuName = t.RtuName,
                                            LoopName = t.RtuLoopName,
                                            FaultId = t.FaultId,
                                            FaultName = t.FaultName,
                                            Id = t.Id,
                                            Color = lstIds.Contains(t.Id) ? "#FF3030" : "#000000",
                                            AlarmCount = error.AlarmCount,
                                            DataFirstTime =
                                                error.DateFirst.Ticks < 1
                                                    ? "--"
                                                    : error.DateFirst.ToString("yyyy-MM-dd HH:mm:ss"),
                                            Voltage =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.V.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            Current =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.A.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            Power =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.Aeding.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            HighLimit =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.AUpper.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            LowLimit =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.ALower.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            PriorityLevel = error.PriorityLevel,
                                            Paidan = string.IsNullOrEmpty(error.Paidan) ? "" : "已派单",
                                            OrderId = error.Paidan
                                        });
                        }
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

                    if (error.PriorityLevel != 1)
                    {
                        if (t.DateCreate >= DateTime.Now.AddDays(-1))
                        {
                            string cqj = string.Empty;
                            string dygh = string.Empty;

                            GetCqjAndDygh(t.RtuId, ref cqj, ref dygh);

                            obs.Add(new FaultRecordViewModel
                                        {
                                            DataCreateTime = t.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
                                            RtuId = t.RtuPhyId,
                                            RtuLgcId = t.RtuId,
                                            CQJ = cqj,
                                            DYGH = dygh,
                                            LoopId = error.LoopId,
                                            DateCreate = error.DateCreate.Ticks,
                                            RtuName = t.RtuName,
                                            LoopName = t.RtuLoopName,
                                            FaultId = t.FaultId,
                                            FaultName = t.FaultName,
                                            Id = t.Id,
                                            Color = lstIds.Contains(t.Id) ? "#FF3030" : "#000000",
                                            AlarmCount = error.AlarmCount,
                                            DataFirstTime =
                                                error.DateFirst.Ticks < 1
                                                    ? "--"
                                                    : error.DateFirst.ToString("yyyy-MM-dd HH:mm:ss"),
                                            Voltage =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.V.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            Current =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.A.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            Power =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.Aeding.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            HighLimit =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.AUpper.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            LowLimit =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.ALower.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            PriorityLevel = error.PriorityLevel,
                                            Paidan = string.IsNullOrEmpty(error.Paidan) ? "" : "已派单",
                                            OrderId = error.Paidan
                                        });
                        }
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

                    if (error.PriorityLevel != 1)
                    {
                        if (t.DateCreate >= DateTime.Now.AddDays(-2))
                        {
                            string cqj = string.Empty;
                            string dygh = string.Empty;

                            GetCqjAndDygh(t.RtuId, ref cqj, ref dygh);

                            obs.Add(new FaultRecordViewModel
                                        {
                                            DataCreateTime = t.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
                                            RtuId = t.RtuPhyId,
                                            RtuLgcId = t.RtuId,
                                            CQJ = cqj,
                                            DYGH = dygh,
                                            LoopId = error.LoopId,
                                            DateCreate = error.DateCreate.Ticks,
                                            RtuName = t.RtuName,
                                            LoopName = t.RtuLoopName,
                                            FaultId = t.FaultId,
                                            FaultName = t.FaultName,
                                            Id = t.Id,
                                            Color = lstIds.Contains(t.Id) ? "#FF3030" : "#000000",
                                            AlarmCount = error.AlarmCount,
                                            DataFirstTime =
                                                error.DateFirst.Ticks < 1
                                                    ? "--"
                                                    : error.DateFirst.ToString("yyyy-MM-dd HH:mm:ss"),
                                            Voltage =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.V.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            Current =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.A.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            Power =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.Aeding.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            HighLimit =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.AUpper.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            LowLimit =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.ALower.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            PriorityLevel = error.PriorityLevel,
                                            Paidan = string.IsNullOrEmpty(error.Paidan) ? "" : "已派单",
                                            OrderId = error.Paidan
                                        });
                        }
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

                    if (error.PriorityLevel != 1)
                    {
                        if (t.DateCreate >= DateTime.Now.AddHours(-6))
                        {
                            string cqj = string.Empty;
                            string dygh = string.Empty;

                            GetCqjAndDygh(t.RtuId, ref cqj, ref dygh);

                            obs.Add(new FaultRecordViewModel
                                        {
                                            DataCreateTime = t.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
                                            RtuId = t.RtuPhyId,
                                            RtuLgcId = t.RtuId,
                                            CQJ = cqj,
                                            DYGH = dygh,
                                            LoopId = error.LoopId,
                                            DateCreate = error.DateCreate.Ticks,
                                            RtuName = t.RtuName,
                                            LoopName = t.RtuLoopName,
                                            FaultId = t.FaultId,
                                            FaultName = t.FaultName,
                                            Id = t.Id,
                                            Color = lstIds.Contains(t.Id) ? "#FF3030" : "#000000",
                                            AlarmCount = error.AlarmCount,
                                            DataFirstTime =
                                                error.DateFirst.Ticks < 1
                                                    ? "--"
                                                    : error.DateFirst.ToString("yyyy-MM-dd HH:mm:ss"),
                                            Voltage =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.V.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            Current =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.A.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            Power =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.Aeding.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            HighLimit =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.AUpper.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            LowLimit =
                                                (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 &&
                                                 error.Aeding < 0.0001 && error.V < 0.0001)
                                                    ? "--"
                                                    : error.ALower.ToString("f2"),//(CultureInfo.InvariantCulture),
                                            PriorityLevel = error.PriorityLevel,
                                            Paidan = string.IsNullOrEmpty(error.Paidan) ? "" : "已派单",
                                            OrderId = error.Paidan
                                        });
                        }
                    }
                }
            }





            var list1 = (from t in obs where  faultLst[0].Item1.Contains( t.FaultId ) orderby t.DateCreate descending  select t).ToList();
            var list2 = (from t in obs where faultLst[0].Item1.Contains(t.FaultId) orderby t.DateCreate descending select t).ToList();
            var list3 = (from t in obs where faultLst[0].Item1.Contains(t.FaultId) orderby t.DateCreate descending select t).ToList();
            var list4 = (from t in obs where faultLst[0].Item1.Contains(t.FaultId) orderby t.DateCreate descending select t).ToList();
            var list5 = (from t in obs where faultLst[0].Item1.Contains(t.FaultId) orderby t.DateCreate descending select t).ToList();
            var mtp = new List<int>();
            mtp.AddRange(faultLst[0].Item1);
            mtp.AddRange(faultLst[1].Item1);
            mtp.AddRange(faultLst[2].Item1);
            mtp.AddRange(faultLst[3].Item1);
            mtp.AddRange(faultLst[4].Item1);

            var list6 = (from t in obs where !mtp.Contains(t.FaultId) orderby t.DateCreate descending select t).ToList();
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

            if (faultLst[0].Item2 == faultLst[1].Item2 && faultLst[0].Item2 == faultLst[2].Item2
                && faultLst[0].Item2 == faultLst[3].Item2 && faultLst[0].Item2 == faultLst[4].Item2)
            {
                Records.Add(new AllFaultRecordsViewModel()
                                { FaultName = "所有故障", IsOther = true, RecordItems = collection6 });

            }
            else
            {
                Records.Add(new AllFaultRecordsViewModel() 
                                { FaultName = "其他故障", IsOther = true, RecordItems = collection6 });
            }
            //foreach (var f in Records)
            //{
            //    f.FaultCountNew = 0;
            //    f.FaultCountNewShow = Visibility.Collapsed ;
            //    f.FaultCountOld = f.RecordItems.Count;
            //} 
            //CountNewError = 0;

            int tmpCr = 0;
            foreach (var f in Records)
            {
                f.FaultCountNew = (from t in f.RecordItems where t.Color.Equals("#FF3030") select t).Count();
                f.FaultCountNewShow = f.FaultCountNew == 0 ? Visibility.Collapsed : Visibility.Visible;
                f.FaultCountOld = f.RecordItems.Count;

                tmpCr += f.FaultCountNew;
            }

            CountNewError = tmpCr;
        }

        
    }
}
