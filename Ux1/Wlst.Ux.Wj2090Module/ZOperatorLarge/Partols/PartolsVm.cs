using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Ux.Wj2090Module.HisDataQuery.ConcentratorDataQuery.ViewModel;
using Wlst.mobile;

namespace Wlst.Ux.Wj2090Module.ZOperatorLarge.Partols
{
    [Export(typeof (IIPartols))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class PartolsVm : Wlst.Cr.Core.EventHandlerHelper.EventHandlerHelperExtendNotifyProperyChanged,
                                     IIPartols
    {
        public PartolsVm()
        {
            this.InitAction();
            this.InitEvent();
        }

        #region IITab
        public int Index
        {
            get { return 1; }
        }
        public string Title
        {
            get { return "单灯巡测"; }
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

        private bool _thisViewActive = false;

        public void NavOnLoad(params object[] parsObjects)
        {
            IsLastPartolIsConces = true;
            _thisViewActive = true;
            IsCalcData = false;
            CanAnyButton = true;
            _dtQuery = DateTime.Now.AddDays(-1);
            FlagDataType = 1;

            NameSelected = ":无";
            IsSelectedRuts = false;
            CurrentSelectedRutOrGrpId = null;
        }

        public void OnUserHideOrClosing()
        {
            _thisViewActive = false;
            IsHasData = false;
            ItemsSlu.Clear();
            ItemsLamp.Clear();
            ItemsCtrl.Clear();
        }

        #region Items

        private ObservableCollection<DataSluItemInfo> _concentratorItems;

        public ObservableCollection<DataSluItemInfo> ItemsSlu
        {
            get { return _concentratorItems ?? (_concentratorItems = new ObservableCollection<DataSluItemInfo>()); }
            set
            {
                if (value == _concentratorItems) return;
                _concentratorItems = value;
                this.RaisePropertyChanged(() => this.ItemsSlu);
            }
        }


        private ObservableCollection<DataCtrlItemInfo> _concentratorItemss;

        public ObservableCollection<DataCtrlItemInfo> ItemsCtrl
        {
            get { return _concentratorItemss ?? (_concentratorItemss = new ObservableCollection<DataCtrlItemInfo>()); }
            set
            {
                if (value == _concentratorItemss) return;
                _concentratorItemss = value;
                this.RaisePropertyChanged(() => this.ItemsCtrl);
            }
        }

        private ObservableCollection<DataLampItemInfo> _concentratorItemsss;

        public ObservableCollection<DataLampItemInfo> ItemsLamp
        {
            get { return _concentratorItemsss ?? (_concentratorItemsss = new ObservableCollection<DataLampItemInfo>()); }
            set
            {
                if (value == _concentratorItemsss) return;
                _concentratorItemsss = value;
                this.RaisePropertyChanged(() => this.ItemsLamp);
            }
        }

        private ObservableCollection<DataLampItemInfo> _concentratorItemssss;

        public ObservableCollection<DataLampItemInfo> ItemsLampCalc
        {
            get { return _concentratorItemssss ?? (_concentratorItemssss = new ObservableCollection<DataLampItemInfo>()); }
            set
            {
                if (value == _concentratorItemssss) return;
                _concentratorItemssss = value;
                this.RaisePropertyChanged(() => this.ItemsLampCalc);
            }
        }



        #endregion

        #region attri

        private Wlst.Sr.EquipmentInfoHolding.Model.SelectedInfo CurrentSelectedRutOrGrpId;
        private int CurrentSelectedRutId;

        private string _flagNameSelectedsi;

        /// <summary>
        /// 是否是集中器巡测
        /// </summary>
        public string NameSelected
        {
            get { return _flagNameSelectedsi; }
            set
            {
                if (_flagNameSelectedsi == value) return;
                _flagNameSelectedsi = value;
                RaisePropertyChanged(() => NameSelected);
            }
        }

        private bool _flagVIsSelectedRuts;

        /// <summary>
        /// 是否是集中器巡测
        /// </summary>
        public bool IsSelectedRuts
        {
            get { return _flagVIsSelectedRuts; }
            set
            {
                if (_flagVIsSelectedRuts == value) return;
                _flagVIsSelectedRuts = value;
                RaisePropertyChanged(() => IsSelectedRuts);
            }
        }

        private bool _isCalcData;
        /// <summary>
        /// 是否统计数据
        /// </summary>
        public bool IsCalcData
        {
            get { return _isCalcData; }
            set
            {
                if (_isCalcData == value) return;
                _isCalcData = value;

                CanAnyButton = !_isCalcData;
                if (_isCalcData)
                {
                    FlagDataType = 4;
                }else
                {
                    FlagDataType = 1;
                }
                RaisePropertyChanged(() => IsCalcData);
            }
        }

        private bool _canAnybutton;
        /// <summary>
        /// 是否统计数据
        /// </summary>
        public bool CanAnyButton
        {
            get { return _canAnybutton; }
            set
            {
                if (_canAnybutton == value) return;
                _canAnybutton = value;
                RaisePropertyChanged(() => CanAnyButton);
            }
        }


        private bool _haveErrs;
        /// <summary>
        /// 统计有故障
        /// </summary>
        public bool HaveErrs
        {
            get { return _haveErrs; }
            set
            {
                if (_haveErrs == value) return;
                _haveErrs = value;
                RaisePropertyChanged(() => HaveErrs);
            }
        }

        private bool _haveNoErrs;
        /// <summary>
        /// 统计无故障
        /// </summary>
        public bool HaveNoErrs
        {
            get { return _haveNoErrs; }
            set
            {
                if (_haveNoErrs == value) return;
                _haveNoErrs = value;
                RaisePropertyChanged(() => HaveNoErrs);
            }
        }














        private bool _flagVsdfssdfsddisi;

        /// <summary>
        /// 是否是集中器巡测
        /// </summary>
        public bool IsLastPartolIsConces
        {
            get { return _flagVsdfssdfsddisi; }
            set
            {
                if (_flagVsdfssdfsddisi == value) return;
                _flagVsdfssdfsddisi = value;
                RaisePropertyChanged(() => IsLastPartolIsConces);
            }
        }

        private int _flagVsdfsdisi;

        /// <summary>
        /// 1 集中器数据，2控制器数据，3 灯具数据
        /// </summary>
        public int FlagDataType
        {
            get { return _flagVsdfsdisi; }
            set
            {
                if (value < 1) value = 1;
                //if (value > 3) value = 3;
                if (value > 4) value = 4;
                if (_flagVsdfsdisi == value) return;
                _flagVsdfsdisi = value;
                RaisePropertyChanged(() => FlagDataType);

                if (value == 1 || value == 2) CmdText = "灯具数据";
                else CmdText = "控制器数据";
            }
        }

        private bool _flsdfsdffsddisi;

        /// <summary>
        /// 
        /// </summary>
        public bool IsHasData
        {
            get { return _flsdfsdffsddisi; }
            set
            {
                if (_flsdfsdffsddisi == value) return;
                _flsdfsdffsddisi = value;
                RaisePropertyChanged(() => IsHasData);

            }
        }

        private string _fssdfsdfsdddisi;

        /// <summary>
        /// 
        /// </summary>
        public string CmdText
        {
            get { return _fssdfsdfsdddisi; }
            set
            {
                if (_fssdfsdfsdddisi == value) return;
                _fssdfsdfsdddisi = value;
                RaisePropertyChanged(() => CmdText);
            }
        }

        private int _sluId;

        /// <summary>
        /// 单灯逻辑地址
        /// </summary>
        public int CountAll
        {
            get { return _sluId; }
            set
            {
                if (_sluId == value) return;
                _sluId = value;
                RaisePropertyChanged(() => CountAll);
            }
        }



        private int _sluPhyId;

        /// <summary>
        /// 单灯物理地址
        /// </summary>
        public int CountReturn
        {
            get { return _sluPhyId; }
            set
            {
                if (value != _sluPhyId)
                {
                    _sluPhyId = value;
                    this.RaisePropertyChanged(() => this.CountReturn);
                }
            }
        }

        #region Remind

        private string _remind;

        public string Remind
        {
            get { return _remind; }
            set
            {
                if (_remind == value) return;
                _remind = value;
                RaisePropertyChanged(() => Remind);
            }
        }

        #endregion

        #endregion

        #region CmdQuery 巡测

        private DateTime _dtQuery;
        private ICommand _cmdQuery;

        public ICommand CmdQuery
        {
            get { return _cmdQuery ?? (_cmdQuery = new RelayCommand(ExQuery, CanQuery, true)); }
        }

        private void ExQuery()
        {
            _dtQuery = DateTime.Now;
            SluMeasure(IsLastPartolIsConces);
        }

        private bool CanQuery()
        {
            return DateTime.Now.Ticks - _dtQuery.Ticks > 50000000;
        }

        #endregion

        #region CmdQueryBc 补测


        private ICommand _cmdCmdQueryBc;

        public ICommand CmdCmdQueryBc
        {
            get { return _cmdCmdQueryBc ?? (_cmdCmdQueryBc = new RelayCommand(ExCmdQueryBc, CanCmdQueryBc, true)); }
        }

        private void ExCmdQueryBc()
        {
            _dtQuery = DateTime.Now;

            var lst =
                (from t in currentmeaserex
                 where
                     t > Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.SluStart &&
                     t < Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.SluEnd &&
                     !SlusReturn.Contains(t)
                 select t).ToList();


            SluMeasure(lst, IsLastPartolIsConces);
        }

        private bool CanCmdQueryBc()
        {
            if (SlusReturn.Count == 0) return false;
            return DateTime.Now.Ticks - _dtQuery.Ticks > 30000000;
        }

        #endregion

        #region CmdShowData 控制器数据和灯具数据互查


        private ICommand _cmdShowData;

        public ICommand CmdShowData
        {
            get { return _cmdShowData ?? (_cmdShowData = new RelayCommand(ExShowData, CanShowData, false)); }
        }

        private void ExShowData()
        {
            if (FlagDataType == 2) FlagDataType = 3;
            else
            {
                if (FlagDataType == 3) FlagDataType = 2;
            }

        }

        private bool CanShowData()
        {
            if (SlusReturn.Count == 0) return false;
            if (FlagDataType == 1) return false;
            return true;
        }

        #endregion


        #region CmdShowAlreadyData

        private DateTime _dtQueryffff = DateTime.Now;
        private ICommand _cmdCmdShowAlreadyDataQuery;

        public ICommand CmdShowAlreadyData
        {
            get
            {
                return _cmdCmdShowAlreadyDataQuery ??
                       (_cmdCmdShowAlreadyDataQuery =
                        new RelayCommand(ExCmdShowAlreadyDataQuery, CanCmdShowAlreadyDataQuery, true));
            }
        }

        private void ExCmdShowAlreadyDataQuery()
        {
            _dtQueryffff = DateTime.Now;


            List<int> curs = new List<int>();
            curs.Add(CurrentSelectedRutId);
            if (IsSelectedRuts && CurrentSelectedRutOrGrpId != null)
            {
                 curs =
                    Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGrpTmlList(
                        CurrentSelectedRutOrGrpId.AreaId, CurrentSelectedRutOrGrpId.RtuOrGrpId);
            }


            //var tmpx = curs.Count == 0
            //               ? (from t in Wlst .Sr .EquipmentInfoHolding .Services .RunningInfoHold .Info  
            //                  where  curs .Contains( t.Key ) &&  t.Value .SluCtrlNewData !=null 
            //                  orderby t.SluCtrlNewData .Item1 ascending
            //                  orderby t.Key.Item2 ascending
            //                  select t).ToList() ).ToList();
            FlagDataType = 3;
            this.ItemsLamp.Clear();
            foreach (var g in Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.Info)
            {
                if (!curs.Contains(g.Key)) continue;
                foreach (var gg in g.Value.SluCtrlNewData.Values)
                {
                    if (gg == null || gg.Data5 == null || gg.Data5.Items == null) continue;
                    foreach (var l in gg.Data5.Items)
                        this.ItemsLamp.Add(new DataLampItemInfo(l, this.ItemsLamp.Count + 1));
                }
            }

        }

        private bool CanCmdShowAlreadyDataQuery()
        {
            return DateTime.Now.Ticks - _dtQueryffff.Ticks > 30000000;
        }

        #endregion

        #region CmdShowAlreadyData

        //private DateTime _dtQueryffff = DateTime.Now;
        private ICommand _cmdQueryOpen;

        public ICommand CmdQueryOpenClose
        {
            get
            {
                return _cmdQueryOpen ??
                       (_cmdQueryOpen =
                        new RelayCommand<string>(ExCmdQueryOpenClose, CanCmdQueryOpenClose, true));
            }
        }

        private void ExCmdQueryOpenClose(string s)
        {
            _dtQueryffff = DateTime.Now;

            int x = 0;
            if (Int32.TryParse(s, out x))
            {
                List<int> curs = new List<int>();
                //curs.Add(CurrentSelectedRutId);
                //if (IsSelectedRuts && CurrentSelectedRutOrGrpId != null)
                //{
                //    curs =
                //       Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGrpTmlList(
                //           CurrentSelectedRutOrGrpId.AreaId, CurrentSelectedRutOrGrpId.RtuOrGrpId);
                //}


                //var tmpx = curs.Count == 0
                //               ? (from t in Wlst .Sr .EquipmentInfoHolding .Services .RunningInfoHold .Info  
                //                  where  curs .Contains( t.Key ) &&  t.Value .SluCtrlNewData !=null 
                //                  orderby t.SluCtrlNewData .Item1 ascending
                //                  orderby t.Key.Item2 ascending
                //                  select t).ToList() ).ToList();
                FlagDataType = 4;
                this.ItemsLampCalc.Clear();
                foreach (var g in Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.Info)
                {
                    //if (!curs.Contains(g.Key)) continue;
                    if ( g.Value.SluCtrlNewData==null ||g.Value.SluCtrlNewData.Count ==0) continue;
                    foreach (var gg in g.Value.SluCtrlNewData.Values)
                    {
                        if (gg == null || gg.Data5 == null || gg.Data5.Items == null) continue;
                        foreach (var l in gg.Data5.Items)
                        {
                            if (l.StateWorkingOn == x )
                            {
                                if (!HaveErrs && !HaveNoErrs)
                                {
                                    this.ItemsLampCalc.Add(new DataLampItemInfo(l, this.ItemsLampCalc.Count + 1));
                                    continue;
                                }
                                if (HaveErrs && l.Fault > 0) this.ItemsLampCalc.Add(new DataLampItemInfo(l, this.ItemsLampCalc.Count + 1));  //todo
                                if (HaveNoErrs && l.Fault == 0) this.ItemsLampCalc.Add(new DataLampItemInfo(l, this.ItemsLampCalc.Count + 1));
                            }
                               
                        }
                            
                    }
                }
            }

          

        }

        private bool CanCmdQueryOpenClose(string s)
        {
            return DateTime.Now.Ticks - _dtQueryffff.Ticks > 30000000;
        }

        #endregion

    }


    public partial class PartolsVm
    {
        private void InitEvent()
        {
            this.AddEventFilterInfo(Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                                    PublishEventType.Core);
            this.AddEventFilterInfo(Sr.EquipmentInfoHolding.Services.EventIdAssign.GroupSelected, PublishEventType.Core);
        }

        public override void ExPublishedEvent(
            PublishEventArgs args)
        {
            ////base.ExPublishedEvent(args);
            //if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
            //{
            //    
            //}
            //if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.GroupSelected )
            //{
            //    LoadTree();
            //}

            if (IsSelectedRuts == false) return;

            if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
            {
                int idx = 0;
                if (Int32.TryParse(args.GetParams()[0].ToString(), out idx))
                {
                    if (idx > Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.SluStart &&
                        idx < Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.SluEnd)
                    {
                        CurrentSelectedRutId = idx;

                        var info = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(idx);
                        if (info == null)
                        {
                            CurrentSelectedRutOrGrpId = null;
                            NameSelected = "：无";
                            return;
                        }
                        if (info.RtuFid == 0)
                        {
                            NameSelected = ":" + info.RtuPhyId.ToString("d4") + "-" + info.RtuName;
                            return;
                        }
                        var attinfo =
                            Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(
                                info.RtuFid);
                        if (attinfo == null)
                        {
                            NameSelected = ":" + info.RtuId.ToString("d4") + "-" + info.RtuName;
                            return;
                        }
                        NameSelected = ":" + attinfo.RtuPhyId.ToString("d4") + "-" + info.RtuName;
                        return;
                    }
                }
            }
            if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.GroupSelected)
            {
                CurrentSelectedRutOrGrpId = args.GetParams()[0] as SelectedInfo  ;
                NameSelected = "--";
                    if(CurrentSelectedRutOrGrpId !=null )
                    {
                        var grp =
                            Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(
                                CurrentSelectedRutOrGrpId.AreaId, CurrentSelectedRutOrGrpId.RtuOrGrpId);
                        if(grp !=null )
                        {
                            CurrentSelectedRutId = 0;
                            NameSelected = ":" + grp.GroupId + "-" + grp.GroupName;
                        }
                    }
            }
        }


    

    }

    /// <summary>
    /// Action
    /// </summary>
    public partial class PartolsVm
    {
        /// <summary>
        /// 记录应答的终端设备
        /// </summary>
        protected  List<int> SlusReturn = new List<int>(); 
        /// <summary>
        /// 集中器
        /// </summary>
        public void InitAction()
        {
            InitSluAction();
        }

        public void InitSluAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .LxSlu .wst_svr_ans_slu_ctrl_measure ,// .wlst_svr_ans_cnt_request_wj2090_measure ,
                SluMeasureBack,
                typeof (PartolsVm), this);
        }

        public void SluMeasureBack(string session, MsgWithMobile infos)
        {
            var info = infos.WstSluSvrAnsSluMeasure  ;
            if (info == null) return;
            if (_thisViewActive == false) return;
            if (!currentmeaserex.Contains(info.SluId)) return;


            if (!SlusReturn.Contains(info.SluId)) SlusReturn.Add(info.SluId);
            CountReturn = SlusReturn.Count;
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"    " +info.SluId+ " 应答巡测.";

            
            if (info.Type == 0 && info.InfoSlu0 .Count >0 && LastPartolIsConces )
            {
                this.ItemsSlu.Add(new DataSluItemInfo(info.InfoSlu0[0], this.ItemsSlu.Count + 1));
                IsHasData = true;
                return;
            }
            if (LastPartolIsConces == false)
            {
                if (info.Type == 5 || info.Type == 7)
                {
                    if (info.InfoBaseic5 == null) return;


                    foreach (var g in info.InfoBaseic5)
                    {
                        if (g != null && g.Info != null)
                        {
                            this.ItemsCtrl.Add(new DataCtrlItemInfo(g.Info, ItemsCtrl.Count + 1));

                            IsHasData = true;
                        }
                        if (g == null || g.Items == null) continue;
                        foreach (var gg in g.Items)
                        {
                            if (gg == null) continue;
                            this.ItemsLamp.Add(new DataLampItemInfo(gg, this.ItemsLamp.Count + 1));
                        }
                    }
                }
            }
        }


    }

    /// <summary>
    /// Socket
    /// </summary>
    public partial class PartolsVm
    {
        /// <summary>
        /// 上一次点击巡测是否是集中器巡测
        /// </summary>
        protected bool LastPartolIsConces = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isPartolsConce">是否为集中器设备</param>
        private void SluMeasure(bool isPartolsConce)
        {
            LastPartolIsConces = isPartolsConce;
            if (isPartolsConce) FlagDataType = 1;
            else FlagDataType = 2;

            this.ItemsCtrl.Clear();
            this.ItemsLamp.Clear();
            this.ItemsSlu.Clear();
            SlusReturn.Clear();
            IsHasData = false;
            currentmeaserex.Clear();
            if (IsSelectedRuts == false || CurrentSelectedRutOrGrpId == null )
            {
                currentmeaserex =
                    (from t in
                         Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems 
                     where
                         t .Value .EquipmentType ==WjParaBase.EquType.Slu 
                     select t.Key ).ToList();
                CountAll = currentmeaserex.Count;


                var info = Wlst.Sr.ProtocolPhone.LxSlu .wst_slu_ctrl_measure ;//.wlst_cnt_request_wj2090_measure;
                info.Args.Addr.Add(0);
                info.WstSluMeasure.CtrlCount = 255;
                info.WstSluMeasure.SluId = -99;
                info.WstSluMeasure.CtrlIdStart = 1;
                info.WstSluMeasure.Type = isPartolsConce ? 0 : 7;
                SndOrderServer.OrderSnd(info);
            }
            else
            {
                var grpx =
                    Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(
                        CurrentSelectedRutOrGrpId.AreaId, CurrentSelectedRutOrGrpId.RtuOrGrpId);
                if (grpx != null)
                {

                    var lst = (from t in grpx.LstTml
                               where
                                   t > Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.SluStart &&
                                 t <  Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.SluEnd
                               select t).ToList();

                    CountAll = lst.Count();


                        var info = Wlst.Sr.ProtocolPhone.LxSlu.wst_slu_ctrl_measure; //.wlst_cnt_request_wj2090_measure;
                        info.WstSluMeasure.CtrlCount = 255;

                        info.WstSluMeasure.CtrlIdStart = 1;
                        info.WstSluMeasure.Type = isPartolsConce ? 0 : 7;

                        foreach (var f in lst)
                        {
                            info.WstSluMeasure.SluId = f;
                            info.Head.Gid += 1000;
                            SndOrderServer.OrderSnd(info);
                        }
                        currentmeaserex.AddRange(lst);
                    
                }
                else
                {
                    CountAll = 1;
                    var info = Wlst.Sr.ProtocolPhone.LxSlu .wst_slu_ctrl_measure ;
                    info.WstSluMeasure.CtrlCount = 255;
                    info.WstSluMeasure.SluId = CurrentSelectedRutId ;
                    info.WstSluMeasure.CtrlIdStart = 1;
                    info.WstSluMeasure.Type = isPartolsConce ? 0 : 7;
                    SndOrderServer.OrderSnd(info);
                    currentmeaserex.Add(CurrentSelectedRutId);

                }
            }
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 巡测命令已发送...";
        }

        private List<int> currentmeaserex = new List<int>(); 


        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstSlus"> </param>
        /// <param name="isPartolsConce">是否为集中器设备</param>
        private void SluMeasure(List<int> lstSlus, bool isPartolsConce)
        {
            LastPartolIsConces = isPartolsConce;

            long last = DateTime.Now.Ticks;
            foreach (var g in lstSlus)
            {
                var info = Wlst.Sr.ProtocolPhone.LxSlu .wst_slu_ctrl_measure ;
                info.Args .Addr .Add(g);
                info.WstSluMeasure.CtrlCount = 255;
                info.WstSluMeasure.SluId = g;
                info.WstSluMeasure.CtrlIdStart = 1;
                info.WstSluMeasure.Type = isPartolsConce ? 0 : 7;
                if (info.Head .Gid  == last)
                {
                    info.Head .Gid  = last + 1;
                }
                last = info.Head .Gid ;

                SndOrderServer.OrderSnd(info);
            }

            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 补测命令已发送...";
        }
    }



}
