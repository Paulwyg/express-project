using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Microsoft.Win32;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.ComponentHold;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.CoreOne.Services;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Cr.PPProtocolSvrCnt.Common;
using Wlst.Sr.EquipemntLightFault.Model;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultOnTabViewModel.ViewModel;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryViewModel.Services;
using Wlst.Ux.EquipemntLightFault.Models.Exchange;
using Wlst.Ux.EquipemntLightFault.Services;
using Wlst.client;


namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryViewModel.ViewModel
{
    [Export(typeof(IIEquipmentFaultRecordQueryViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class EquipmentFaultRecordQueryViewModel : ObservableObject, IIEquipmentFaultRecordQueryViewModel
    {
        public EquipmentFaultRecordQueryViewModel()
        {
            ClickTime = DateTime.Now;
            InitEvent();
            InitAction();
            //LoadIsShowThisViewOnNewErrArrive();
        }

        void ntg_OnIsSelectedChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();

            var ggg = sender as OperatorTypeItem;
            if (ggg == null || ggg.Value.Count == 0) return;
            foreach (var g in ggg.Value)
            {
                g.IsSelected = ggg.IsSelected;
            }
        }


        private bool isgaojiselected = false;
        private List<int> lastselectedinfo = new List<int>();
        public void NavOnLoad(params object[] parsObjects)
        {
            
            var lsthase = new List<int>();
            foreach (var f in EquipmentModelComponentHolding.DicEquipmentModels.Values)
            {
                lsthase.Add(f.ModelKey);
            }

            //throw new NotImplementedException();
            isgaojiselected = IsAdvancedQueryChecked;
            // foreach (var t in TypeItems )if(t.IsSelected )lastselectedinfo .Add( t.Value )
            CmdDeleteVisi = Visibility.Collapsed;
            var count = 0;
            foreach (var g in TypeItems) count += g.Value.Count;
            if (count < 5)
            {
                TypeItems.Clear();

                //var ntg = new NameIntBoolXg {Name = "所有报警", Value = 0, IsSelected = true};
                //ntg.OnIsSelectedChanged += new EventHandler(ntg_OnIsSelectedChanged);
                //ntg.IsMonitor = true;

                //  FaultType.Add(ntg);

                var lst = new Dictionary<Tuple<int, int, int, string>, List<Tuple<int, string>>>();

                foreach (var g in Sr.EquipemntLightFault.Services.FaultClassisDef.FaultClass)
                {
                    if (!lsthase.Contains(g.Item1)) continue;
                    if (lst.ContainsKey(g)) continue;
                    lst.Add(g, new List<Tuple<int, string>>());

                }

                foreach (var t in Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.InfoDictionary)
                {
                    if (!t.Value.IsEnable) continue;
                    foreach (var f in lst)
                    {
                        if (t.Key >= f.Key.Item2 && t.Key <= f.Key.Item3)
                        {
                            if (!f.Value.Contains(new Tuple<int, string>(t.Key, t.Value.FaultNameByDefine)))
                                f.Value.Add(new Tuple<int, string>(t.Key, t.Value.FaultNameByDefine));
                        }
                    }
                }


                foreach (var g in lst)
                {

                    var gnt = new OperatorTypeItem()
                                  {
                                      IsSelected = false,
                                      Name = g.Key.Item4,
                                      Value = new ObservableCollection<NameIntBool>()
                                  };
                    foreach (var f in g.Value)
                    {
                        gnt.Value.Add(new NameIntBool() { IsSelected = false, Name = f.Item2, Value = f.Item1 });
                    }
                    if (gnt.Value.Count > 0)
                    {
                        gnt.OnIsSelectedChanged += new EventHandler(ntg_OnIsSelectedChanged);
                        TypeItems.Add(gnt);
                    }
                }
            }
            // if (FaultType.Count > 0) CurrentSelectType = FaultType[0];
            DtEndTime = DateTime.Now;
            DtStartTime = DateTime.Now.AddDays(-1);
            _dtQuery = DateTime.Now.AddDays(-1);
            IsSingleEquipmentQuery = false;
            IsOldFaultQuery = false;


            GetAllFaultName();


            int rutid = 0;
            try
            {
                rutid = Convert.ToInt32(parsObjects[0]);
            }
            catch (Exception ex)
            {
            }
            if (rutid > 0)
            {
                RtuId = rutid;
                this.IsAdvancedQueryChecked = true;
                this.IsSingleEquipmentQuery = true;
                this.Ex();
            }
            IsNewAllQuery = true;

            if (rutid == -1)
            {
                //  this.IsAdvancedQueryChecked = true;
                if (_thisViewActive) return;
                this.Ex();//todo

            }
            if(rutid ==-2)
            {
                ClickTime = DateTime.Now;
                if (_thisViewActive) return;
                this.Ex();//todo
            }
            _thisViewActive = true;
        }



        public void OnUserHideOrClosing()
        {
            //_thisViewActive = false;
            //Records = new ObservableCollection<EquipmentFaultViewModel>();
            //ExportVisi = Visibility.Collapsed;
            //if (Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Count
            //    > 0)
            //    ClickTime =
            //        (from t in
            //             Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.
            //             Values
            //         orderby t.DateCreate descending
            //         select t).ToList()[0].DateCreate;
            //else ClickTime = DateTime.Now;

            //var argss = new PublishEventArgs()
            //                {
            //                    EventType = PublishEventType.Core,
            //                    EventId = EventIdAssign.PushErrNums
            //                };
            //argss.AddParams(0);
            //EventPublisher.EventPublish(argss);
            this.Records.Clear();
        }

        #region tab

        public bool CanClose
        {
            get { return true; }
        }

        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        public bool CanFloat
        {
            get { return true; }
        }

        public bool CanUserPin
        {
            get { return true; }
        }

        public string Title
        {
            get { return "终端故障查询"; }
        }

        public DateTime ClickTime { get; set; }

        #endregion

    }


    /// <summary>
    /// Field ,Attri, ICommand ,Methods
    /// </summary>
    public partial class EquipmentFaultRecordQueryViewModel
    {

        #region Field

        private static bool _isOnExport = true;

        #endregion

        #region Attri

        #region ExportVisi

        private Visibility _exportVisi = Visibility.Collapsed;

        public Visibility ExportVisi
        {
            get { return _exportVisi; }
            set
            {
                if (value == _exportVisi) return;
                _exportVisi = value;
                RaisePropertyChanged(() => ExportVisi);
            }
        }

        #endregion

        #region IsAdvancedQueryChecked

        private bool _isAdvancedQueryChecked;

        public bool IsAdvancedQueryChecked
        {
            get { return _isAdvancedQueryChecked; }
            set
            {
                if (_isAdvancedQueryChecked != value)
                {
                    _isAdvancedQueryChecked = value;

                    if (!_isAdvancedQueryChecked)
                    {
                        IsSingleEquipmentQuery = false;
                        //foreach (var nameIntBool in TypeItems )
                        //{
                        //    nameIntBool.IsSelected = nameIntBool.Value == 0;
                        //}

                    }
                    RaisePropertyChanged(() => IsAdvancedQueryChecked);
                }
            }
        }

        #endregion

        #region RtuName

        private string _rtuName;

        public string RtuName
        {
            get { return _rtuName; }
            set
            {
                if (value != _rtuName)
                {
                    _rtuName = value;
                    RaisePropertyChanged(() => RtuName);
                }
            }
        }

        #endregion

        #region RtuId

        private int _iphyd;

        public int PhyId
        {
            get { return _iphyd; }
            set
            {
                if (_iphyd != value)
                {
                    _iphyd = value;
                    this.RaisePropertyChanged(() => this.PhyId);
                }
            }
        }

        private int _rtuid;

        public int RtuId
        {
            get { return _rtuid; }
            set
            {

                if (value != _rtuid)
                {
                    _rtuid = value;
                    PhyId = value;
                    RaisePropertyChanged(() => RtuId);
                    RtuName = "Reserve";
                    if (
                        !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.
                             InfoItems.ContainsKey
                             (_rtuid))
                        return;
                    var tml =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                            [_rtuid];
                    RtuName = tml.RtuName;

                    if (tml.RtuFid == 0)
                        PhyId = tml.RtuPhyId;
                    else PhyId = value;
                }
            }
        }

        #endregion

        #region Records

        private ObservableCollection<EquipmentFaultViewModel> _record;

        public ObservableCollection<EquipmentFaultViewModel> Records
        {
            get { return _record ?? (_record = new ObservableCollection<EquipmentFaultViewModel>()); }
            set
            {
                if (_record != value)
                {
                    _record = value;
                    this.RaisePropertyChanged(() => this.Records);
                }
            }
        }

        #endregion

        #region CurrentSelectRecord

        private EquipmentFaultViewModel _currentSelectRecord;

        public EquipmentFaultViewModel CurrentSelectRecord
        {
            get { return _currentSelectRecord ?? (_currentSelectRecord = new EquipmentFaultViewModel()); }
            set
            {
                if (_currentSelectRecord == value) return;
                _currentSelectRecord = value;
                RaisePropertyChanged(() => CurrentSelectRecord);
            }
        }

        #endregion

        #region FaultType

        //private ObservableCollection<NameIntBoolXg> _fauleType;

        //public ObservableCollection<NameIntBoolXg> FaultType
        //{
        //    get { return _fauleType ?? (_fauleType = new ObservableCollection<NameIntBoolXg>()); }
        //}


        private ObservableCollection<OperatorTypeItem> _typeItems;
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<OperatorTypeItem> TypeItems
        {
            get { return _typeItems ?? (_typeItems = new ObservableCollection<OperatorTypeItem>()); }
            set
            {
                if (value == _typeItems) return;
                _typeItems = value;
                this.RaisePropertyChanged(() => TypeItems);
            }
        }
        #endregion

        #region IsSingleEquipmentQuery

        private bool _isSingleEquipmentQuery;

        /// <summary>
        /// 是否为单个终端故障查询
        /// </summary>
        public bool IsSingleEquipmentQuery
        {
            get { return _isSingleEquipmentQuery; }
            set
            {
                if (_isSingleEquipmentQuery != value)
                {
                    _isSingleEquipmentQuery = value;
                    RaisePropertyChanged(() => IsSingleEquipmentQuery);
                }
            }
        }

        #endregion

        #region IsOldFaultQuery

        private bool _isOldFaultQuery;

        /// <summary>
        /// 是否为新故障查询，是否就为历史故障查询
        /// </summary>
        public bool IsOldFaultQuery
        {
            get { return _isOldFaultQuery; }
            set
            {
                if (_isOldFaultQuery == value) return;
                _isOldFaultQuery = value;
                RaisePropertyChanged(() => IsOldFaultQuery);
                OnStartTimeEnableChange();
            }
        }


        private bool _isOlIsNewAllQuerydFaultQuery;

        /// <summary>
        /// 
        /// </summary>
        public bool IsNewAllQuery
        {
            get { return _isOlIsNewAllQuerydFaultQuery; }
            set
            {
                if (_isOlIsNewAllQuerydFaultQuery == value) return;
                _isOlIsNewAllQuerydFaultQuery = value;
                RaisePropertyChanged(() => IsNewAllQuery);
                OnStartTimeEnableChange();
            }
        }


        private bool _isOlIsNewIsFaultQueryTimeStartEnableAllQuerydFaultQuery;

        /// <summary>
        /// 
        /// </summary>
        public bool IsFaultQueryTimeStartEnable
        {
            get { return _isOlIsNewIsFaultQueryTimeStartEnableAllQuerydFaultQuery; }
            set
            {
                if (_isOlIsNewIsFaultQueryTimeStartEnableAllQuerydFaultQuery == value) return;
                _isOlIsNewIsFaultQueryTimeStartEnableAllQuerydFaultQuery = value;
                RaisePropertyChanged(() => IsFaultQueryTimeStartEnable);
            }
        }


        void OnStartTimeEnableChange()
        {
            if (IsOldFaultQuery) IsFaultQueryTimeStartEnable = true;
            else
            {
                if (IsNewAllQuery) IsFaultQueryTimeStartEnable = false;
                else IsFaultQueryTimeStartEnable = true;
            }
        }

        #endregion

        #region DtEndTime

        private DateTime _dtEndTime;

        /// <summary>
        /// 故障查询结束时间
        /// </summary>
        public DateTime DtEndTime
        {
            get { return _dtEndTime; }
            set
            {
                if (_dtEndTime != value)
                {
                    _dtEndTime = value;
                    RaisePropertyChanged(() => DtEndTime);
                }
            }
        }

        #endregion

        #region DtStartTime

        private DateTime _dtStartTime;

        /// <summary>
        /// 故障查询起始时间
        /// </summary>
        public DateTime DtStartTime
        {
            get { return _dtStartTime; }
            set
            {
                if (_dtStartTime != value)
                {
                    _dtStartTime = value;
                    RaisePropertyChanged(() => DtStartTime);
                }
            }
        }

        #endregion

        #region Remind

        private string _remind;

        public string Remind
        {
            get { return _remind; }
            set
            {
                if (value == _remind) return;
                _remind = value;
                RaisePropertyChanged(() => Remind);
            }
        }

        #endregion

        #endregion

        #region ICommand

        #region CmdQuery

        private DateTime _dtQuery;

        public ICommand CmdQuery
        {
            get { return new RelayCommand(Ex, CanEx, true); }
        }

        private List<int> GetSelectFaultType()
        {
            var ntg = new List<int>();

            foreach (var g in TypeItems)
            {
                foreach (var f in g.Value)
                {
                    if (f.IsSelected && !ntg.Contains(f.Value)) ntg.Add(f.Value);
                }
            }
            return ntg;
        }

        private void Ex()
        {
            CmdDeleteVisi = Visibility.Collapsed;
            //Remind = "查询命令已发送...请等待数据反馈！";
            _dtQuery = DateTime.Now;
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询 ...";
            Records.Clear();
            if (!GetCheckedInformation()) return;
            this.Records.Clear();
            var tmptype = GetSelectFaultType();

            if (IsSingleEquipmentQuery) //单个终端查询
            {
                if (RtuId == 0) return;
                if (!IsOldFaultQuery) //新故障查询
                {
                    // if (tmptype.Count == 0) return;
                    if (tmptype.Count == 0) //所有故障
                    {
                        QueryNewErrorSingleFault(RtuId);
                    }
                    else //单个故障
                    {
                        // if (tmptype.Contains(0)) tmptype.Remove(0);

                        QueryNewErrorSingleFault(RtuId, tmptype);
                    }
                }
                else
                {
                    // if (tmptype.Count == 0) return;
                    if (tmptype.Count == 0) //所有故障
                    {
                        QueryPreErrorSingleFault(RtuId);
                    }
                    else //单个故障
                    {
                        //  if (tmptype.Contains(0)) tmptype.Remove(0);
                        QueryPreErrorSingleFault(RtuId, tmptype);
                    }
                }
            }
            else
            {
                if (!IsOldFaultQuery) //新故障查询
                {
                    //  if (tmptype.Count == 0) return;
                    if (tmptype.Count == 0) //所有故障
                    {
                        //QueryNewErrorAllRtuFault();
                        ResolveNewErrorAllRtuFault();
                    }
                    else //单个故障
                    {
                        //if (tmptype.Contains(0)) tmptype.Remove(0);
                        QueryNewErrorAllRtuFault(tmptype);
                    }
                }
                else
                {
                    // if (tmptype.Count == 0) return;
                    if (tmptype.Count == 0) //所有故障
                    {
                        QueryPreErrorAllRtuFault();
                    }
                    else //单个故障
                    {
                        //if (tmptype.Contains(0)) tmptype.Remove(0);
                        QueryPreErrorAllRtuFault(tmptype);
                    }
                }
            }
            _isOnExport = false;
            ExportVisi = Visibility.Visible;

        }

        private DateTime _dtPreQueryStartTime;
        private DateTime _dtPreQueryEndTime;

        private bool CanEx()
        {
            if (IsOldFaultQuery)
            {
                if (DtStartTime > DtEndTime) return false;
            }
            else return DateTime.Now.Ticks - _dtQuery.Ticks > 30000000;
            return (_dtPreQueryEndTime.Ticks != DtEndTime.Ticks || _dtPreQueryStartTime.Ticks != DtStartTime.Ticks) &&
                   DateTime.Now.Ticks - _dtQuery.Ticks > 30000000;
        }

        #endregion

        #endregion

        #region Methods

        private static string GetRtuName(int rtuId)
        {
            if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(rtuId))
            {
                return "Unknown";
            }
            var fff =
                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuId];

            return fff.RtuName;

        }

        private bool GetCheckedInformation()
        {
            if (DtStartTime.AddDays(63) < DtEndTime)
            {
                UMessageBox.Show("提醒", "请重新选择时间，时间需选择在62天以内", UMessageBoxButton.Ok);
                //WLSTMessageBox.WpfMessageBox.Show("请重新选择时间，时间需选择在30天以内");
                return false;
            }
            return true;
        }



        #endregion


        #region DeleteFault

        //// EquipemntLightFault.Services.MenuIdAssgin.NavToEquipmentFaultDefineSettingViewId
        //private bool GetIsCurrentHasRightToDelete()
        //{
        //    return
        //        Wlst.Cr.CoreOne.Models.MenuItemBase.IsglobalIdHasOperatorRight(
        //            EquipemntLightFault.Services.MenuIdAssgin.NavToEquipmentFaultDefineSettingViewId);
        //}

        private int _counterLableDoubleClick;

        public int CounterLableDoubleClick
        {
            get { return _counterLableDoubleClick; }
            set
            {
                if (_counterLableDoubleClick == value) return;

                _counterLableDoubleClick = value;
                if (_counterLableDoubleClick > 2)
                {
                    _counterLableDoubleClick = 0;

                    CmdDeleteVisi = Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D ? Visibility.Visible : Visibility.Collapsed;

                }
            }
        }

        private Visibility _cmdDeleteVisi;

        public Visibility CmdDeleteVisi
        {
            get { return _cmdDeleteVisi; }
            set
            {
                if (value == _cmdDeleteVisi) return;
                _cmdDeleteVisi = value;
                this.RaisePropertyChanged(() => this.CmdDeleteVisi);
            }
        }


        #region CmdDelete


        public ICommand CmdDelete
        {
            get { return new RelayCommand(ExDelete, CanExDelete, true); }
        }

        private DateTime _dtDelete;

        private void ExDelete()
        {
            CmdDeleteVisi = Visibility.Collapsed;
            Remind = "删除命令已经发送，1秒后可重新查询...";

            this.DeleteQuery();
            Records.Clear();
        }


        private bool CanExDelete()
        {

            if (this.Records.Count < 1) return false;
            return DateTime.Now.Ticks - _dtDelete.Ticks > 30000000;
        }

        #endregion

        #region CmdExport
        private DateTime _dtCmdExport;
        private ICommand _cmdCmdExport;

        public ICommand CmdExport
        {
            get
            {
                if (_cmdCmdExport == null)
                    _cmdCmdExport = new RelayCommand(ExCmdExport, CanExCmdExport, false);
                return _cmdCmdExport;
            }
        }

        private void ExCmdExport()
        {
            _dtCmdExport = DateTime.Now;
            try
            {
                var lsttitle = new List<Object>();
                lsttitle.Add("序号");
                lsttitle.Add("终端地址");
                lsttitle.Add("终端名称");
                lsttitle.Add("故障回路");
                lsttitle.Add("故障名称");
                lsttitle.Add("发生时间");
                if (IsOldFaultQuery) lsttitle.Add("消除时间");
                lsttitle.Add("备注");
                //if (IsOldFaultQuery) 
                    lsttitle.Add("电流上下限备注");
                

                var lstobj = new List<List<object>>();

                foreach (var g in Records)
                {
                    var tmp = new List<object>();
                    tmp.Add(g.Index);
                    tmp.Add(g.PhyId);
                    tmp.Add(g.RtuName);
                    tmp.Add(g.RtuLoopName);
                    tmp.Add(g.FaultName);
                    tmp.Add(g.DtCreateTime);
                    if (IsOldFaultQuery) tmp.Add(g.DtRemoceTime);
                    tmp.Add(g.Remark);
                    //if (IsOldFaultQuery)
                    //{
                        try
                        {
                            if (g.FaultName == "回路电流越上限")
                            {
                                var zhi = g.Remark.Substring(0, g.Remark.IndexOf(","));
                                zhi = zhi.Substring(3, zhi.Length - 4);
                                var xian = g.Remark.Substring(g.Remark.IndexOf(",") + 4, g.Remark.Length - 4 - g.Remark.IndexOf(","));
                                double dzhi, dxian, chazhi;
                                if (double.TryParse(zhi, out dzhi) && double.TryParse(xian, out dxian))
                                {
                                    chazhi = dzhi - dxian;
                                    tmp.Add(chazhi.ToString("F3"));
                                }
                                else
                                {
                                    tmp.Add("Error");
                                }

                            }
                            else if (g.FaultName == "回路电流越下限")
                            {
                                var zhi = g.Remark.Substring(0, g.Remark.IndexOf(","));
                                zhi = zhi.Substring(3, zhi.Length - 4);
                                var xian = g.Remark.Substring(g.Remark.IndexOf(",") + 4, g.Remark.Length - 4 - g.Remark.IndexOf(","));
                                double dzhi, dxian, chazhi;
                                if (double.TryParse(zhi, out dzhi) && double.TryParse(xian, out dxian))
                                {
                                    chazhi = dxian - dzhi;
                                    tmp.Add(chazhi.ToString("F3"));
                                }
                                else
                                {
                                    tmp.Add("Error");
                                }
                            }
                            else
                            {
                                tmp.Add("");
                            }
                        }
                        catch
                        {
                            tmp.Add("Error");
                        }
                    //}

                    lstobj.Add(tmp);
                }
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(lsttitle, lstobj);
                lstobj = null;
                lsttitle = null;
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("导出报表时报错:" + ex);
            }

        }

        private bool CanExCmdExport()
        {
            if (Records.Count < 1) return false;
            return DateTime.Now.Ticks - _dtCmdExport.Ticks > 30000000;
            return false;
        }

        #endregion



        #endregion
    }


    /// <summary>
    /// Data Query
    /// </summary>
    public partial class EquipmentFaultRecordQueryViewModel
    {
        private void QueryNewErrorSingleFault(int rtuId, List<int> faultIds)
        {

            var sss = new List<FaultInfoBase>();
            if (IsNewAllQuery)
            {
                sss = (from gg in Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Values
                       where gg.RtuId == rtuId && faultIds.Contains(gg.FaultId)
                       select gg).ToList();
            }
            else
            {
                var dts = new DateTime(DtStartTime.Year, DtStartTime.Month, DtStartTime.Day, 0, 0, 1).Ticks;
                sss = (from gg in Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Values
                       where gg.DateCreate.Ticks > dts && gg.RtuId == rtuId && faultIds.Contains(gg.FaultId)
                       select gg).ToList();
            }

            Records.Clear();
            var obs = new ObservableCollection<EquipmentFaultViewModel>();
            //var ff = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetLstInfoByRtuId(FaultWarmType.Rtu,
            //               rtuId);
            int intx = 0;
            foreach (var t in sss)
            {
                //this.AddErrorInfo(t);
                var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(t.Id);
                if (error == null) continue;
                intx++;




                obs.Add(new EquipmentFaultViewModel
                            {
                                Index = intx,
                                DtCreateTime = error.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
                                DtRemoceTime = "--",
                                FaultId = t.FaultId,
                                RtuId = rtuId,
                                RtuLoopName = error.RtuLoopName,
                                RtuLoops = t.LoopId,
                                FaultName = error.FaultName,
                                PhyId = error.RtuPhyId,
                                Count = t.AlarmCount + 1,
                                RtuName = error.RtuName,
                                Color = error.Color,
                                Remark = error.Remark,
                                DateCreateId = error.RecordId,
                                DateRemoveId = 0,
                                LampId = error.LampId,
                            });
            }
            Records = obs;
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 故障记录查询成功，共计" + Records.Count + " 条数据.";
        }

        private void QueryNewErrorSingleFault(int rtuId)
        {

            var sss = new List<FaultInfoBase>();
            if (IsNewAllQuery)
            {
                sss = (from gg in Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Values
                       where gg.RtuId == rtuId
                       select gg).ToList();
            }
            else
            {
                var dts = new DateTime(DtStartTime.Year, DtStartTime.Month, DtStartTime.Day, 0, 0, 1).Ticks;
                sss =
                (from gg in Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Values
                 where gg.DateCreate.Ticks > dts && gg.RtuId == rtuId
                 select gg).ToList();
            }



            Records.Clear();
            var obs = new ObservableCollection<EquipmentFaultViewModel>();
            //var ff = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetLstInfoByRtuId(FaultWarmType.Rtu,
            //                                                                                      rtuId);
            int intx = 0;
            foreach (var t in sss)
            {
                //this.AddErrorInfo(t);
                var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(t.Id);
                if (error == null) continue;
                intx++;
                obs.Add(new EquipmentFaultViewModel
                            {
                                Index = intx,
                                DtCreateTime = error.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
                                DtRemoceTime = "--",
                                FaultId = t.FaultId,
                                PhyId = error.RtuPhyId,
                                RtuId = rtuId,
                                RtuLoopName = error.RtuLoopName,
                                RtuLoops = t.LoopId,
                                FaultName = error.FaultName,
                                Count = t.AlarmCount + 1,
                                Color = error.Color,
                                RtuName = error.RtuName,
                                Remark = error.Remark,
                                DateCreateId = error.RecordId,
                                DateRemoveId = 0,
                                LampId = error.LampId,
                            });
            }
            Records = obs;
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 查询成功，共" + Records.Count + " 条数据.";
        }

        private void ResolveNewErrorAllRtuFault()
        {


            var dts = new DateTime(DtStartTime.Year, DtStartTime.Month, DtStartTime.Day, 0, 0, 1).Ticks;
            Records.Clear();
            var obs = new ObservableCollection<EquipmentFaultViewModel>();
            int intx = 0;

            var tmox = (from t in Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Values
                        orderby  t.DateCreate descending
                        select t).ToList();               //t.IsShowAtTop descending ,
            foreach (var t in tmox)
            {
                if (IsNewAllQuery == false)
                {
                    if (t.DateCreate.Ticks < dts) continue;
                }
                //this.AddErrorInfo(t);
                var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(t.Id);

                if (error == null) continue;
                intx++;
                obs.Add(new EquipmentFaultViewModel
                            {
                                Index = intx,
                                DtCreateTime = error.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
                                DtRemoceTime = "--",
                                FaultId = t.FaultId,
                                RtuId = t.RtuId,
                                RtuLoopName = error.RtuLoopName,
                                RtuLoops = t.LoopId,
                                PhyId = error.RtuPhyId,
                                FaultName = error.FaultName,
                                Count = t.AlarmCount + 1,
                                Color = error.Color,
                                RtuName = error.RtuName,
                                Remark = error.Remark,
                                DateCreateId = error.RecordId,
                                DateRemoveId = 0,
                            });
            }
            Records = obs;
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 查询成功，共" + Records.Count + " 条数据.";
        }

        private void QueryNewErrorAllRtuFault(List<int> faultIds)
        {
            Records.Clear();
            var obs = new ObservableCollection<EquipmentFaultViewModel>();
            int intx = 0; var dts = new DateTime(DtStartTime.Year, DtStartTime.Month, DtStartTime.Day, 0, 0, 1).Ticks;
            foreach (var t in Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Values)
            {
                if (!faultIds.Contains(t.FaultId)) continue;
                if (IsNewAllQuery == false)
                {
                    if (t.DateCreate.Ticks < dts) continue;
                }
                //this.AddErrorInfo(t);
                var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(t.Id);
                if (error == null) continue;
                int rtuphyid;
                //if (error.FaultId == 48) rtuphyid = error.RtuFhyId;
                //else rtuphyid = error.RtuPhyId;

                intx++;
                obs.Add(new EquipmentFaultViewModel
                            {
                                Index = intx,
                                DtCreateTime = error.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
                                DtRemoceTime = "--",
                                FaultId = t.FaultId,
                                RtuId = t.RtuId,
                                RtuLoopName = error.RtuLoopName,
                                RtuLoops = t.LoopId,
                                PhyId = error.RtuPhyId,
                                FaultName = error.FaultName,
                                Count = t.AlarmCount + 1,
                                Color = error.Color,
                                RtuName = error.RtuName,
                                Remark = error.Remark,
                                DateCreateId = error.RecordId,
                                DateRemoveId = 0,
                            });
            }
            Records = obs;
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 查询成功，共" + Records.Count + " 条数据.";
        }


        private void QueryPreErrorSingleFault(int rtuId, List<int> faultIds)
        {
            Sr.EquipemntLightFault.Services.PreErrorServices.ReqeustPreExistError(rtuId, DtStartTime, DtEndTime,
                                                                                  faultIds);
        }

        private void QueryPreErrorSingleFault(int rtuId)
        {
            Sr.EquipemntLightFault.Services.PreErrorServices.ReqeustPreExistError(rtuId, DtStartTime, DtEndTime);
        }

        private void QueryPreErrorAllRtuFault()
        {
            Sr.EquipemntLightFault.Services.PreErrorServices.ReqeustPreExistError(DtStartTime, DtEndTime);
        }

        private void QueryPreErrorAllRtuFault(List<int> faultIds)
        {
            Sr.EquipemntLightFault.Services.PreErrorServices.ReqeustPreExistError(DtStartTime, DtEndTime, faultIds);
        }

        private void DeleteQuery()
        {
            if (this.Records.Count == 0) return;

            var nt = Wlst.Sr.ProtocolPhone.LxFault.wlst_delete_falut_cur;//.ProtocolCnt.ServerPart.wlst_EquipemntLightFault_clinet_delete_curFault;
            foreach (var t in this.Records)
            {


                nt.WstFaultDeleteCurr.DeleteItems.Add(new EquipmentFaultDelete.EquipmentFaultDeleteItem()
                                           {
                                               FaultCode = t.FaultId,
                                               LoopId = t.RtuLoops,
                                               RtuId = t.RtuId,
                                               LampId = t.LampId
                                           });
            }


            Wlst.Sr.PPPandSocketSvr.Server.SocketClient.SndData(nt);
        }

        //private void OnPreDataBack(EquipmentPreFaultExChange info)
        //{
        //    int index = 1;
        //    Records.Clear();
        //    var obs = new ObservableCollection<EquipmentFaultViewModel>();
        //    foreach (var t in info.Info)
        //    {

        //      var mtpsss = Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetEquipmentInfo(RtuId);




        //        var itemsss = new EquipmentFaultViewModel
        //                          {
        //                              DtCreateTime = t.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
        //                              DtRemoceTime = t.DateRemove.ToString("yyyy-MM-dd HH:mm:ss"),
        //                              FaultId = t.FaultCodeId,
        //                              FaultName = "",

        //                              Index = index,
        //                              RtuId = t.RtuId,
        //                              PhyId = mtpsss == null ? 0 : mtpsss.PhyId,
        //                              RtuLoopName = mtpsss == null ? t.LoopId + "" : mtpsss.GetRtuLoopName( t.LoopId ),
        //                              RtuLoops = t.LoopId,
        //                              RtuName = mtpsss == null ? "" : mtpsss.RtuName,
        //                              Remark = t.Remark,

        //                              DateCreateId = t.RecordAlarmId,
        //                              DateRemoveId = t.RecordRemoveId,
        //                              LampId = t.LampId
        //                          };
        //        var typ = Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.GetInfoById(t.FaultCodeId);
        //        if (typ != null)
        //        {
        //            itemsss.FaultName = typ.FaultNameByDefine;
        //            itemsss.Color = typ.Color;
        //        }
        //        if (mtpsss == null)
        //        {


        //            itemsss.RtuName = GetRtuName(t.RtuId);
        //            itemsss.PhyId =
        //                Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetPhysicalIdByLogicalId(t.RtuId);
        //        }
        //        obs.Add(itemsss);
        //        index++;
        //    }
        //    Records = obs;
        //    Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 故障记录查询成功，共计" + Records.Count + " 条数据.";
        //}
    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class EquipmentFaultRecordQueryViewModel
    {
        public void OnRequestServerData(EquipmentFaultViewModel info)
        {
            if (info == null) return;
            Sr.EquipemntLightFault.Services.PreErrorServices.RequestDataWhenErrorHappen(info.RtuId, info.RtuLoops,
                                                                                        info.DateCreateId);



            //发布事件  选中当前节点
            var args = new PublishEventArgs
                           {
                               EventType = PublishEventType.Core,
                               EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                           };
            args.AddParams(info.RtuId);
            EventPublisher.EventPublish(args);
        }

        private void InitEvent()
        {
            EventPublisher.AddEventSubScriptionTokener(
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandler,
                FundOrderFilter);

        }
        


        #region EventSubScriptionTokener

        private bool _thisViewActive;

        public void FundEventHandler(PublishEventArgs args) // should do somework
        {

            
            try
            {
                if (args.EventType == PublishEventType.Core)
                {


                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                    {
                        if (!_thisViewActive) return;
                        int id = Convert.ToInt32(args.GetParams()[0]);
                        if (id < Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.RtuStart) return;
                        if (RtuId == id) return;
                        RtuId = id;
                        Records.Clear();
                        Ex();
                    }
                    //if (args.EventId == Sr.EquipemntLightFault.Services.EventIdAssign.EquipmentExistFaultAddId)
                    //{
                    //    if (Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowNewErrArriveOnUi)
                    //    {
                    //        if (args.GetParams().Count > 0)
                    //        {
                    //            var rtx = args.GetParams()[0] as List<int>;
                    //            if (rtx == null) return;

                    //            if (rtx.Count > 1)
                    //            {
                    //                if (Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Count
                    //                    > 0)
                    //                    ClickTime =
                    //                        (from t in
                    //                             Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.
                    //                             InfoDictionary.
                    //                             Values
                    //                         orderby t.DateCreate descending
                    //                         select t).ToList()[0].DateCreate;
                    //            }
                    //            else
                    //            {
                    //                var tmox =
                    //                    (from t in
                    //                         Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.
                    //                         Values
                    //                     where t.DateCreate > ClickTime && t.IsThisUserShow
                    //                     orderby t.DateCreate descending
                    //                     select t).ToList();

                    //                var argss = new PublishEventArgs()
                    //                                {
                    //                                    EventType = PublishEventType.Core,
                    //                                    EventId = EventIdAssign.PushErrNums
                    //                                };
                    //                argss.AddParams(tmox.Count);
                    //                EventPublisher.EventPublish(argss);

                                    
                    //            }
                    //        }
                    //    }
                    //    if (!_thisViewActive) return;
                    //    if (IsLockThisViewOnNewErrArrive) return;
                    //    var info = args.GetParams()[0] as List<int>;
                    //    if (info == null) return;
                    //    foreach (var t in info)
                    //    {
                    //        var ntgs =
                    //            Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.
                    //                GetFaultInfoById
                    //                (t);
                    //        if (ntgs != null)
                    //            AddErrorInfo(ntgs, true);
                    //    }


                    //}
                }
                //if (args.EventId == Sr.EquipemntLightFault.Services.EventIdAssign.PreExistErrorRequestId)
                //{
                //    var infos = args.GetParams()[1] as EquipmentPreFaultExChange;
                //    if (infos == null) return;
                //    OnPreDataBack(infos);
                //}
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError(
                    "EquipmentDataQuery.EquipmentFaultRecordQueryViewModel FundEventHandler occer an error:" +
                    ex);
            }

        }

        public bool FundOrderFilter(PublishEventArgs args) //接收终端选中变更事件
        {
            if (!_thisViewActive) return false;
            // if (!IsSingleEquipmentQuery) return false;
            try
            {
                if (args.EventType == PublishEventType.Core)
                {
                    //if (!IsSingleEquipmentQuery) return false;  lvf
                    //if (args.EventId == Sr.EquipmentGroupInfoHolding.Services.EventIdAssign.MainSingleTreeNodeActive)
                    //{
                    //    if (Convert.ToInt32(args.GetParams()[1]) == 2)
                    //    {
                    //        return true;
                    //    }
                    //}
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                    {
                        return true;
                    }
                    //if (args.EventId ==Sr.EquipemntLightFault.Services.EventIdAssign.EquipmentExistFaultAddId)
                    //{
                    //    return true;
                    //}

                }
                //if (args.EventType == PublishEventType.Sevr &&
                //    args.EventId == Sr.EquipemntLightFault.Services.EventIdAssign.PreExistErrorRequestId)
                //{
                //    return true;
                //}
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Error:" + ex);
            }
            return false;
        }

        private void AddErrorInfo(FaultInfoBase error, bool dongtaiupdate)
        {
            return;
            // var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(id);
            if (error == null) return;

            var infovm = new EquipmentFaultViewModel
            {
                Index = 0,
                DtCreateTime = error.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
                DtRemoceTime = "--",
                FaultId = error.FaultId,
                RtuId = error.RtuId,
                RtuLoopName = error.RtuLoopName,
                RtuLoops = error.LoopId,
                PhyId = error.RtuPhyId,
                FaultName = error.FaultName,
                Count = error.AlarmCount + 1,
                Color = error.Color,
                RtuName = error.RtuName,
                Remark = error.Remark,
                DateCreateId = error.RecordId,
                DateRemoveId = 0,
            };

            Records.Insert(0, infovm);


            //if (Records.Count > MaxRecordHold)
            //{
            //    Records.RemoveAt(0);
            //}
        }

        #endregion

        private void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Sr.ProtocolPhone.LxFault.wlst_fault_pre,
                OnRequestFaultPre,
                typeof(EquipmentFaultRecordQueryViewModel), this);
        }


        //private string pathcx = Environment.CurrentDirectory + "\\Config\\IsShowThisViewOnNewErrArrive.txt";
        //private void LoadIsShowThisViewOnNewErrArrive()
        //{
        //    var ft = Wlst.Ux.EquipemntLightFault.Services.fileread.Read(pathcx);
        //    if (string.IsNullOrEmpty(ft)) IsShowThisViewOnNewErrArrive = false;
        //    else
        //    {
        //        int x = 0;
        //        if (Int32.TryParse(ft, out x))
        //        {
        //            IsShowThisViewOnNewErrArrive = (x == 1);
        //        }
        //    }
        //}

        //public static bool IsShowThisViewOnNewErrArriveInfo = false;
        private bool _cheIsLockThisViewOnNewErrArriveck;

        public bool IsLockThisViewOnNewErrArrive
        {
            get { return _cheIsLockThisViewOnNewErrArriveck; }
            set
            {
                if (_cheIsLockThisViewOnNewErrArriveck != value)
                {
                    _cheIsLockThisViewOnNewErrArriveck = value;
                    this.RaisePropertyChanged(() => this.IsLockThisViewOnNewErrArrive);
                    IsLockThisViewOnNewErrArrive = value;
                }
            }
        }

        public void OnRequestFaultPre(string session, Wlst.mobile.MsgWithMobile infos)
        {
            var list = infos.WstFaultPre;

            //var list = obj as List<PreErrorItem>;
            //if (list == null) return;
            //  Records.Clear();
            var tmp = new Dictionary<Tuple<int, int, int>, int>();

           Records.Clear();
           var obs = new ObservableCollection<EquipmentFaultViewModel>();

            int indexx = 0;
            foreach (var item in list.FaultItems)
            {
                int count;
                //if (IsUseTimeLongQuery)
                //{

                var tu = new Tuple<int, int, int>(item.RtuId, item.LoopId, item.FaultId);
                if (tmp.ContainsKey(tu)) count = tmp[tu];
                else
                {
                    count =
                        (from t in list.FaultItems
                         where
                             t.RtuId == item.RtuId && t.LoopId == item.LoopId &&
                             t.FaultId == item.FaultId
                         select t).Count();
                    tmp.Add(tu, count);

                }
                indexx++;
                var mtpsss = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(item.RtuId);
                int py = item.RtuId;
                string rtuname = "";
                if (mtpsss != null)
                {
                    rtuname = mtpsss.RtuName;
                    if (mtpsss.RtuFid > 0)
                    {
                        var nfx =
                            Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(
                                mtpsss.RtuFid);
                        if (nfx != null)
                        {
                            rtuname = nfx.RtuName + "-" + mtpsss.RtuName;
                            py = nfx.RtuPhyId;
                        }
                    }
                    else py = mtpsss.RtuPhyId;
                }

                var loopName = mtpsss != null
                                                ? mtpsss.GetLoopName(item.LoopId)
                                                  : item.LoopId.ToString(CultureInfo.InvariantCulture);
                if (item.FaultId == 21) loopName = "开关量输出K" + item.LoopId;


                obs.Add(new EquipmentFaultViewModel
                            {
                                DtCreateTime = new DateTime(item.DateCreate).ToString("yyyy-MM-dd HH:mm:ss"),
                                DtRemoceTime = new DateTime(item.DateRemove).ToString("yyyy-MM-dd HH:mm:ss"),
                                Index = indexx,
                                FaultId = item.FaultId,
                                RtuId = item.RtuId,
                                PhyId = py,
                                RtuLoopName = loopName,
                                RtuLoops = item.LoopId,
                                RtuName = rtuname,//mtpsss != null ? mtpsss.RtuName : GetRtuName(item.RtuId),
                                FaultName = GetFaultName(item.FaultId).Item1,
                                Color = GetFaultColor(item.FaultId),
                                Count = count,
                                Remark = item.Remark,
                                DateCreateId = item.DateCreate,
                                DateRemoveId = item.DateRemove,
                                LampId = item.LampId,
                                IsShowAtTop = GetFaultName(item.FaultId).Item2
                            });
               // if (indexx % 100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent(); //todo
            }

            var mtp = (from t in obs orderby t.DateCreateId ascending select t).ToList();//t.IsShowAtTop descending,
            
            Records.Clear();
            foreach (var f in mtp )
            {
                indexx++;
                Records.Add(f);
                if (indexx % 100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
            }
            
            //  Remind = "数据已反馈完毕，请查看数据！";
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 查询成功，共" + list.FaultItems.Count + " 条数据.";
        }

        private readonly List<NameIntBool> _faultName = new List<NameIntBool>();

        private void GetAllFaultName()
        {
            foreach (var item in Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.InfoDictionary)
            {
                _faultName.Add(new NameIntBool { Name = item.Value.FaultNameByDefine, Value = item.Value.FaultId ,AreaId = item.Value.PriorityLevel});
            }
        }

        private Tuple<string,int> GetFaultName(int faultid)
        {
            foreach (var item in _faultName.Where(item => faultid == item.Value))
            {
                return new Tuple<string, int>(item.Name,item.AreaId);
            }
            return new Tuple<string, int>("no name", 0); 
        }


        private string GetFaultColor(int faultid)
        {
            var tmp = Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.GetInfoById(faultid);
            if (tmp != null) return tmp.Color;
            return null;
        }
    }




    /// <summary>
    /// 操作类型模型定义
    /// </summary>
    public class OperatorTypeItem : ObservableObject
    {

        public event EventHandler OnIsSelectedChanged;


        private bool _check;
        public bool IsSelected
        {
            get { return _check; }
            set
            {
                if (_check != value)
                {
                    _check = value;
                    this.RaisePropertyChanged(() => this.IsSelected);
                    if (OnIsSelectedChanged != null)
                    {
                        OnIsSelectedChanged(this, EventArgs.Empty);
                    }
                }
            }
        }





        private string _name;
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }

        private ObservableCollection<NameIntBool> _value;
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<NameIntBool> Value
        {
            get { return _value ?? (_value = new ObservableCollection<NameIntBool>()); }
            set
            {
                if (value == _value) return;
                _value = value;
                RaisePropertyChanged(() => Value);
            }
        }
    }







}

