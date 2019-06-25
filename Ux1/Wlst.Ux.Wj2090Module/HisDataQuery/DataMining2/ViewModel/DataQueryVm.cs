using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
 
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Ux.Wj2090Module.HisDataQuery.DataMining2.Services;

namespace Wlst.Ux.Wj2090Module.HisDataQuery.DataMining2.ViewModel
{
    [Export(typeof (IIDataMining))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class DataMininVm : Wlst.Cr.Core.EventHandlerHelper.EventHandlerHelperExtendNotifyProperyChanged, IIDataMining
    {

        public DataMininVm()
        {
            this.InitAction();
            this.InitEvent();

            QueryStrinInfoChanged();
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            _thisViewActive = true;
            IndexView = 2;
           
            FlagIsRtuUseed = false;
            BeginTime = DateTime.Now.AddDays(-1);
            EndTime = DateTime.Now;
            IsSome = true;

            QueryStrinInfoChanged();
        }

        private bool _thisViewActive = false;

        public void OnUserHideOrClosing()
        {
            this.ItemsCtrlSome.Clear();
            this.ItemsLamp.Clear();
            _thisViewActive = false;
        }

        #region IITab
        public int Index
        {
            get { return 1; }
        }
        public string Title
        {
            get { return "单灯电量查询"; }
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



        private ObservableCollection<DataLampItemMining> _concentratorItemsss;

        public ObservableCollection<DataLampItemMining> ItemsLamp
        {
            get { return _concentratorItemsss ?? (_concentratorItemsss = new ObservableCollection<DataLampItemMining>()); }
            set
            {
                if (value == _concentratorItemsss) return;
                _concentratorItemsss = value;
                this.RaisePropertyChanged(() => this.ItemsLamp);
            }
        }

        private ObservableCollection<DataCtrlMiningSome> _concentratorItemsdfsss;

        public ObservableCollection<DataCtrlMiningSome> ItemsCtrlSome
        {
            get
            {
                return _concentratorItemsdfsss ??
                       (_concentratorItemsdfsss = new ObservableCollection<DataCtrlMiningSome>());
            }
            set
            {
                if (value == _concentratorItemsdfsss) return;
                _concentratorItemsdfsss = value;
                this.RaisePropertyChanged(() => this.ItemsCtrlSome);
            }
        }


        private bool _isSome;
        public bool IsSome
        {
            get { return _isSome; }
            set
            {
                if (_isSome == value) return;
                _isSome = value;
                this.RaisePropertyChanged(() => this.IsSome);
            }
        }


        private int  _isdfsdsSome;
        public int IndexView
        {
            get { return _isdfsdsSome; }
            set
            {
                if (_isdfsdsSome == value) return;
                _isdfsdsSome = value;
                this.RaisePropertyChanged(() => this.IndexView);
            }
        }
    }

    /// <summary>
    /// 属性
    /// </summary>
    public partial class DataMininVm
    {
        private DateTime _dtPreQueryStartTime;
        private DateTime _dtPreQueryEndTime;


        private void QueryStrinInfoChanged()
        {
            QueryStrinInfo = GetQueryInfo();
        }

        private string GetQueryInfo()
        {

            if(FlagIsCtrlUseed && CtrId==0)
            {
                return "请使用单灯树选择控制器进行查询...";
            }

            if (FlagIsRtuUseed == false && FlagIsCtrlUseed == false)
            {
                return "查询：所有集中器电量统计信息";
            }
            if (FlagIsRtuUseed == true && FlagIsCtrlUseed == false)
            {
                if (SluId < 1)
                {
                    return "查询：所有集中器电量统计信息";
                }

                return "查询：集中器" + SluPhyId + "电量统计信息";

            }
            if (FlagIsRtuUseed == false && FlagIsCtrlUseed == true)
            {

                return "查询：所有控制器电量信息";

            }

            if (SluId < 1 && CtrId < 1)
            {
                return "查询：所有控制器电量信息";
            }
            if (CtrId < 1)
            {
                return "查询：集中器" + SluPhyId + "下所有控制器电量统计信息";
            }
            if (SluId < 1)
            {
                return "查询：所有控制器电量统计信息";
            }
            return "查询：集中器" + SluPhyId + "下控制器" + CtrPhyId + "电量信息";


        }

        #region attri 查询选项

        #region FlagVisiIndex 控制显示的页面

        private int _flagVisi;

        public int FlagVisiIndex
        {
            get { return _flagVisi; }
            set
            {
                if (value < 1) value = 1;
                if (value > 3) value = 3;
                if (_flagVisi == value) return;
                _flagVisi = value;
                RaisePropertyChanged(() => FlagVisiIndex);
            }
        }

        //private int _flagVsdfsdisi;
        ///// <summary>
        ///// 1 集中器数据，2控制器数据，3 灯具数据
        ///// </summary>
        //public int FlagDataType
        //{
        //    get { return _flagVsdfsdisi; }
        //    set
        //    {
        //        if (value < 1) value = 1;
        //        if (value > 3) value = 3;
        //        if (_flagVsdfsdisi == value) return;
        //        _flagVsdfsdisi = value;
        //        RaisePropertyChanged(() => FlagDataType);

        //        QueryStrinInfoChanged();
        //    }
        //}

        private bool _flagVsdfssdfsddisi;
        /// <summary>
        /// 是否使用终端参数
        /// </summary>
        public bool FlagIsRtuUseed
        {
            get { return _flagVsdfssdfsddisi; }
            set
            {
                if (_flagVsdfssdfsddisi == value) return;
                _flagVsdfssdfsddisi = value;
                RaisePropertyChanged(() => FlagIsRtuUseed);
 
               
                QueryStrinInfoChanged();
            }
        }


        private bool _flasdfsdgVsdfssdfsddisi;
        /// <summary>
        /// 是否使用终端参数
        /// </summary>
        public bool FlagIsCtrlUseed
        {
            get { return _flasdfsdgVsdfssdfsddisi; }
            set
            {

                if (_flasdfsdgVsdfssdfsddisi == value) return;
                if(value ==true  && FlagIsRtuUseed ==false )
                {
                    value = false;
                }
                _flasdfsdgVsdfssdfsddisi = value;

                RaisePropertyChanged(() => FlagIsCtrlUseed);
                if (value == false)
                {
                    CtrId = 0;
                }

                 if(value && CtrId ==0)
                 {
                     Remind = "请在单灯列表中选择需要查询详细数据的单灯控制器...";
                 }
                QueryStrinInfoChanged();
            }
        }
        //private int _flagsdfsdfi;
        ///// <summary>
        ///// 1-4 灯头 5 全部
        ///// </summary>
        //public int FlagLampId
        //{
        //    get { return _flagsdfsdfi; }
        //    set
        //    {
        //        if (value < 1) value = 1;
        //        if (value > 5) value = 5;
        //        if (_flagsdfsdfi == value) return;
        //        _flagsdfsdfi = value;
        //        RaisePropertyChanged(() => FlagLampId);
        //        QueryStrinInfoChanged();
        //    }
        //}

        //private bool _fsdfsdddisi;
        ///// <summary>
        ///// 是否使用终端参数
        ///// </summary>
        //public bool FlagLampIdEnable
        //{
        //    get { return _fsdfsdddisi; }
        //    set
        //    {
        //        if (_fsdfsdddisi == value) return;
        //        _fsdfsdddisi = value;
        //        RaisePropertyChanged(() => FlagLampIdEnable);
        //    }
        //}


        private string _fssdfsdfsdddisi;
        /// <summary>
        /// 
        /// </summary>
        public string QueryStrinInfo
        {
            get { return _fssdfsdfsdddisi; }
            set
            {
                if (_fssdfsdfsdddisi == value) return;
                _fssdfsdfsdddisi = value;
                RaisePropertyChanged(() => QueryStrinInfo);
            }
        }

        //void FlagLampIdEnableChanged()
        //{
        //    if (FlagIsRtuUseed == false)
        //    {
        //        FlagLampIdEnable = false;
        //        return;
        //    }

        //    if (SluId < 1)
        //    {
        //        FlagLampIdEnable = false;
        //        return;
        //    }
        //    if (CtrId < 1)
        //    {
        //        FlagLampIdEnable = false;
        //        return;
        //    }

        //    FlagLampIdEnable = true;
        //}


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
        #endregion

        #endregion

        #region attri 集中器 控制器

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

        #region BeginTime

        private DateTime _beginTime;

        public DateTime BeginTime
        {
            get { return _beginTime; }
            set
            {
                if (_beginTime == value) return;
                _beginTime = value;
                RaisePropertyChanged(() => BeginTime);
            }
        }

        #endregion

        #region EndTime

        private DateTime _endTime;

        public DateTime EndTime
        {
            get { return _endTime; }
            set
            {
                if (_endTime == value) return;
                _endTime = value;
                RaisePropertyChanged(() => EndTime);
            }
        }

        #endregion

        #region BtnName

        private string _btnName;

        public string BtnName
        {
            get { return _btnName; }
            set
            {
                if (_btnName == value) return;
                _btnName = value;
                RaisePropertyChanged(() => BtnName);
            }
        }

        #endregion


        #region SluId

        private int _sluId;

        /// <summary>
        /// 单灯逻辑地址
        /// </summary>
        public int SluId
        {
            get { return _sluId; }
            set
            {
                if (_sluId == value) return;
                _sluId = value;
                RaisePropertyChanged(() => SluId);
                var ntg =
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( value);
                if (ntg != null)
                {
                    this.SluPhyId = ntg.RtuPhyId .ToString
                        ("D4");
                    this.SluName = ntg.RtuName ; //.RtuName;
                }
            }
        }

        #endregion

        #region SluPhyId

        private string  _sluPhyId;

        /// <summary>
        /// 单灯物理地址
        /// </summary>
        public string  SluPhyId
        {
            get { return _sluPhyId; }
            set
            {
                if (value != _sluPhyId)
                {
                    _sluPhyId = value;
                    this.RaisePropertyChanged(() => this.SluPhyId);
                }
            }
        }

        #endregion

        #region SluName

        private string _sluName;

        public string SluName
        {
            get { return _sluName; }
            set
            {
                if (_sluName == value) return;
                _sluName = value;
                RaisePropertyChanged(() => SluName);
            }
        }

        #endregion


        #region CtrId

        private int _ctrId;

        public int CtrId
        {
            get { return _ctrId; }
            set
            {
                if (_ctrId == value) return;
                _ctrId = value;
                RaisePropertyChanged(() => CtrId);
                if (CtrId > 0)
                {
                    if (!EquipmentDataInfoHold.InfoItems .ContainsKey(SluId))
                        return;
                    var t = EquipmentDataInfoHold.InfoItems [SluId] as Wlst .Sr .EquipmentInfoHolding .Model .Wj2090Slu ;

                    if (t == null ||t.WjSluCtrls ==null )
                        return;
                    if (!t.WjSluCtrls .ContainsKey(CtrId))
                        return;
                    this.CtrPhyId = t.WjSluCtrls [CtrId].CtrlPhyId;
                    this.CtrName = t.WjSluCtrls [CtrId].LampCode;
                }
                else
                {
                    this.CtrPhyId = 0;
                    this.CtrName = "";
                }
            }
        }

        #endregion

        #region CtrPhyId

        private int _ctrPhyId;

        /// <summary>
        /// 单灯物理地址
        /// </summary>
        public int CtrPhyId
        {
            get { return _ctrPhyId; }
            set
            {
                if (value != _ctrPhyId)
                {
                    _ctrPhyId = value;
                    this.RaisePropertyChanged(() => this.CtrPhyId);
                }
            }
        }

        #endregion

        #region CtrName

        private string _ctrName;

        public string CtrName
        {
            get { return _ctrName; }
            set
            {
                if (_ctrName == value) return;
                _ctrName = value;
                RaisePropertyChanged(() => CtrName);
            }
        }

        #endregion

        #endregion


        #region ICommand

        #region CmdQuery

        private DateTime _dtQuery;
        private ICommand _cmdQuery;

        public ICommand CmdQuery
        {
            get { return _cmdQuery ?? (_cmdQuery = new RelayCommand(ExQuery, CanQuery, false)); }
        }

        private void ExQuery()
        {
            _dtQuery = DateTime.Now;
            Query();
            //  Remind = "集中器查询命令已发送...请等待数据反馈！";
        }

        private bool CanQuery()
        {
            if (BeginTime.Ticks > EndTime.Ticks) return false;

            if (FlagIsRtuUseed)
            {
                if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsSlu(SluId) == false) return false;
            }
            if (FlagIsCtrlUseed)
            {
                if (FlagIsRtuUseed == false) return false;
                if (CtrId < 1) return false;
            }

            return DateTime.Now.Ticks - _dtQuery.Ticks > 30000000;

        }


        private void Query()
        {
            //_dtPreQueryEndTime = this.EndTime;
            //_dtPreQueryStartTime = this.BeginTime;
            if (!GetCheckedInformation()) return;
            this.ItemsLamp.Clear();
            _dtPreQueryStartTime = new DateTime(BeginTime.Year, BeginTime.Month, BeginTime.Day, 0, 0, 1);
            _dtPreQueryEndTime = new DateTime(EndTime.Year, EndTime.Month, EndTime.Day, 23, 59, 59);



            // 如果为10则查询所有集中器数据，如果为11 则查询所有集中器的所有控制器数据，如果为12则查询所有集中器下所有控制器下的所有灯头数据；
            // 如果为1 则查询集中器数据，如果为2则查询该集中器下的所有控制器数据，如果为3则查询指定集中器下所有控制器下的所有灯头数据；
            // 如果为4 则查询指定控制器数据，如果为5 则查询指定控制器下所有灯头数据，如果为6 则查询控制器下的指定灯头数据</param>
            // this.Query(tStartTime, tEndTime, this.SluId); 

            int sluId = 0;
            int ctrlId = 0;


            //int flagtype = 1;
            if (FlagIsRtuUseed == false)
            {
                //flagtype = 1;
                Remind = "正在查询所有集中器电量统计数据...";
            }
            else
            {
                if (SluId < 10)
                {
                    //flagtype = 1;
                    Remind = "正在查询所有集中器电量统计数据...";
                }
                else
                {
                    sluId = SluId;

                    if (FlagIsCtrlUseed ==false )
                    {
                        //flagtype = 1;
                        Remind = "正在查询集中器" + SluPhyId + "电量统计数据...";
                    }
                    else
                    {
                        if (CtrId == 0)
                        {
                            //flagtype = 1;
                            Remind = "正在查询集中器" + SluPhyId + "电量统计数据...";
                        }
                        else
                        {
                            //flagtype = 2;
                            ctrlId = CtrId;
                            Remind = "正在查询集中器" + SluPhyId + "下控制器" + CtrPhyId + "电量详细数据...";

                        }
                    }
                }
            }

            //if (flagtype==1)
            //{
                var info = Wlst.Sr.ProtocolPhone .LxSlu .wst_slu_data_elec ;// .wlst_cnt_wj2090_request_datamining ;//.ServerPart.wlst_Wj2090_clinet_request_slu_datamining;
                info.WstSluDataElec.DtStart = _dtPreQueryStartTime.Ticks;
                info.WstSluDataElec.DtEnd = _dtPreQueryEndTime.Ticks;
               // info.Data.LampId = lampId;
                info.WstSluDataElec.SluId = sluId;
                info.WstSluDataElec.CtrlId = ctrlId;

                SndOrderServer.OrderSnd(info, 10, 3);
            //}
            //else
            //{
            //    var info = Wlst.Sr.ProtocolPhone .LxSlu.wst_slu_data_elec ;// .wlst_cnt_request_wj2090_measure_data ;//.ServerPart.wlst_Wj2090_clinet_request_slu_measure_data;
            //    info.WstSluData  .DtEnd = _dtPreQueryEndTime.Ticks;
            //    info.WstSluData.DtStart = _dtPreQueryStartTime.Ticks;
            // //   info.Data.LampId = lampId;
            //    info.WstSluData.SluId = sluId;
            //    info.WstSluData.CtrlId = ctrlId;
            //    info.WstSluData.Op = 2;

            //    SndOrderServer.OrderSnd(info, 10, 3);
            //}
        }

        #endregion

        private bool GetCheckedInformation()
        {
            if (BeginTime.AddDays(63) < EndTime)
            {
                UMessageBox.Show("提醒", "请重新选择时间，时间需选择在62天以内", UMessageBoxButton.Ok);
                //WLSTMessageBox.WpfMessageBox.Show("请重新选择时间，时间需选择在30天以内");
                return false;
            }
            return true;
        }

        #endregion


    }


    /// <summary>
    /// Event
    /// </summary>
    public partial class DataMininVm
    {
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
               Wlst.Sr.ProtocolPhone .LxSlu  .wst_slu_data_elec  ,//.ClientPart.wlst_Wj2090_svr_ans_clinet_request_slu_measure_data,
               RecordDataRequest,
               typeof (DataMininVm), this,true);

            //ProtocolServer.RegistProtocol(
            //   Wlst.Sr.ProtocolPhone.LxSlu.wst_slu_data_elec,//.ClientPart.wlst_Wj2090_svr_ans_clinet_request_slu_datamining ,
            //   RecordDatamining,
            //   typeof(DataMininVm), this);
        }

        public void RecordDataRequest(string session,Wlst .mobile .MsgWithMobile    infos)
        {
            var info = infos.WstSluDataElec;
            if (info == null) return;
            if (_thisViewActive == false) return;
            this.ItemsLamp.Clear();
            this.ItemsCtrlSome.Clear();

            if (info.SluId == 0 || (info.SluId != 0 && info.CtrlId == 0))
            {
                #region
                //if (IsSome == false)
                //{
                //    IndexView = 3;
                //    return;
                //}
                if (info.ItemsSlu.Count > 0)
                {
                    IndexView = 2;
                    var gts = (from t in info.ItemsSlu orderby t.SluId, t.CtrlId, t.LampId ascending select t).ToList();

                    var dir = new Dictionary<Tuple<int, int>, DataCtrlMiningSome>();

                    foreach (var tt in gts)
                    {

                        var tu = new Tuple<int, int>(tt.SluId, tt.CtrlId);
                        if (!dir.ContainsKey(tu))
                            dir.Add(tu,
                                    new DataCtrlMiningSome()
                                    {
                                        SluId = tt.SluId,
                                        CtrlId = tt.CtrlId.ToString(),
                                        ActiveTimeTotal1 = "--",
                                        ActiveTimeTotal2 = "--",
                                        ActiveTimeTotal3 = "--",
                                        ActiveTimeTotal4 = "--",
                                        ElectricityTotalLamp1 = "--",
                                        ElectricityTotalLamp2 = "--",
                                        ElectricityTotalLamp3 = "--",
                                        ElectricityTotalLamp4 = "--"
                                    });

                        var tu0 = new Tuple<int, int>(tt.SluId, -1);
                        if (!dir.ContainsKey(tu0))
                            dir.Add(tu0,
                                    new DataCtrlMiningSome()
                                    {
                                        SluId = tt.SluId,
                                        CtrlId = "--",
                                        ActiveTimeTotal1 = "--",
                                        ActiveTimeTotal2 = "--",
                                        ActiveTimeTotal3 = "--",
                                        ActiveTimeTotal4 = "--",
                                        ElectricityTotalLamp1 = "--",
                                        ElectricityTotalLamp2 = "--",
                                        ElectricityTotalLamp3 = "--",
                                        ElectricityTotalLamp4 = "--"
                                    });



                        if (tt.LampId == 1)
                        {
                            dir[tu].ActiveTimeTotal1 = (tt.TheseDaysOpenTime / 60).ToString("f2");
                            dir[tu].ElectricityTotalLamp1 =
                                (tt.TheseDaysUsePower).ToString("f2");

                            dir[tu].t1 += tt.TheseDaysOpenTime;
                            dir[tu].e1 += tt.TheseDaysUsePower;

                            dir[tu0].t1 += tt.TheseDaysOpenTime;
                            dir[tu0].e1 += tt.TheseDaysUsePower;
                            dir[tu0].ActiveTimeTotal1 = (dir[tu0].t1 / 60).ToString("f2");
                            dir[tu0].ElectricityTotalLamp1 = (dir[tu0].e1).ToString("f2");
                        }
                        else if (tt.LampId == 2)
                        {
                            dir[tu].ActiveTimeTotal2 = (tt.TheseDaysOpenTime / 60).ToString("f2");
                            dir[tu].ElectricityTotalLamp2 =
                                tt.TheseDaysUsePower.ToString("f2");
                            dir[tu].t2 += tt.TheseDaysOpenTime;
                            dir[tu].e2 += tt.TheseDaysUsePower;

                            dir[tu0].t2 += tt.TheseDaysOpenTime;
                            dir[tu0].e2 += tt.TheseDaysUsePower;
                            dir[tu0].ActiveTimeTotal2 = (dir[tu0].t2 / 60).ToString("f2");
                            dir[tu0].ElectricityTotalLamp2 = (dir[tu0].e2).ToString("f2");
                        }
                        else if (tt.LampId == 3)
                        {
                            dir[tu].ActiveTimeTotal3 = (tt.TheseDaysOpenTime / 60).ToString("f2");
                            dir[tu].ElectricityTotalLamp3 =
                                tt.TheseDaysUsePower.ToString("f2");
                            dir[tu].t3 += tt.TheseDaysOpenTime;
                            dir[tu].e3 += tt.TheseDaysUsePower;

                            dir[tu0].t3 += tt.TheseDaysOpenTime;
                            dir[tu0].e3 += tt.TheseDaysUsePower;
                            dir[tu0].ActiveTimeTotal3 = (dir[tu0].t3 / 60).ToString("f2");
                            dir[tu0].ElectricityTotalLamp3 = (dir[tu0].e3).ToString("f2");
                        }
                        else if (tt.LampId == 4)
                        {
                            dir[tu].ActiveTimeTotal4 = (tt.TheseDaysOpenTime / 60).ToString("f2");
                            dir[tu].ElectricityTotalLamp4 =
                                tt.TheseDaysUsePower.ToString("f2");
                            dir[tu].t4 += tt.TheseDaysOpenTime;
                            dir[tu].e4 += tt.TheseDaysUsePower;

                            dir[tu0].t4 += tt.TheseDaysOpenTime;
                            dir[tu0].e4 += tt.TheseDaysUsePower;
                            dir[tu0].ActiveTimeTotal4 = (dir[tu0].t4 / 60).ToString("f2");
                            dir[tu0].ElectricityTotalLamp4 = (dir[tu0].e4).ToString("f2");
                        }

                        //index++;
                    }
                    //  this.ItemsLamp = sg;
                    var ntgs = (from t in dir.Values orderby t.SluId, t.CtrlId ascending select t).ToList();

                    DataCtrlMiningSome last = null;
                    int indexgg = 1;
                    foreach (var g in ntgs)
                    {

                        g.Index = indexgg++;
                        g.ElectricityTotalLampx = (g.e1 + g.e2 + g.e3 + g.e4).ToString("f2");
                        g.ActiveTimeTotax = ((g.t1 + g.t2 + g.t3 + g.t4) / 60).ToString("f2");

                        if (g.CtrlId == "--")
                        {
                            if (last != null)
                            {
                                ItemsCtrlSome.Add(last);
                            }
                            last = g;
                            g.Index = 0;
                            indexgg = 1;
                            continue;
                        }
                        if (last != null)
                        {
                            g.Index = indexgg++;
                            last.ItemsCtrlSome.Add(g);
                        }
                    }
                    if (last != null) ItemsCtrlSome.Add(last);

                    if (info.SluId == 0)
                    {
                        var lst = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                        foreach (var g in ItemsCtrlSome)
                        {
                            double flg = 0;
                            if (double.TryParse(g.ActiveTimeTotal1,out flg)) lst[0] += flg;
                            if (double.TryParse(g.ActiveTimeTotal2, out flg)) lst[2] += flg;
                            if (double.TryParse(g.ActiveTimeTotal3, out flg)) lst[4] += flg;
                            if (double.TryParse(g.ActiveTimeTotal4, out flg)) lst[6] += flg;
                            if (double.TryParse(g.ActiveTimeTotax, out flg)) lst[8] += flg;

                            if (double.TryParse(g.ElectricityTotalLamp1, out flg)) lst[1] += flg;
                            if (double.TryParse(g.ElectricityTotalLamp2, out flg)) lst[3] += flg;
                            if (double.TryParse(g.ElectricityTotalLamp3, out flg)) lst[5] += flg;
                            if (double.TryParse(g.ElectricityTotalLamp4, out flg)) lst[7] += flg;
                            if (double.TryParse(g.ElectricityTotalLampx, out flg)) lst[9] += flg;

                        }


                        ItemsCtrlSome.Add(new DataCtrlMiningSome()
                        {
                            ActiveTimeTotal1 = lst[0].ToString("f2"),
                            ActiveTimeTotal2 = lst[2].ToString("f2"),
                            ActiveTimeTotal3 = lst[4].ToString("f2"),
                            ActiveTimeTotal4 = lst[6].ToString("f2"),
                            ActiveTimeTotax = lst[8].ToString("f2"),
                            ElectricityTotalLamp1 = lst[1].ToString("f2"),
                            ElectricityTotalLamp2 = lst[3].ToString("f2"),
                            ElectricityTotalLamp3 = lst[5].ToString("f2"),
                            ElectricityTotalLamp4 = lst[7].ToString("f2"),
                            ElectricityTotalLampx = lst[9].ToString("f2"),
                            CtrlId = "--",
                            SluShowId = "--",
                            SluName = "总计"
                        }
                            );

                    }

                    Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "--数据查询成功. （时间单位为小时/电量单位为千瓦时）";
                    return;
                }

                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "-- 数据查询成功，0条数据.";
                #endregion
            }
            //else if (info.CtrlId == 0)
            //{
            //    #region

            //    #endregion
            //}
            else
            {
                #region
                IndexView = 1;
                int index = 0;//1

                if (info.ItemsCtrl.Count > 0)
                {
                    var sg = new ObservableCollection<DataLampItemMining>();
                    var nts = (from t in info.ItemsCtrl orderby t.SluId, t.CtrlId, t.LampId, t.DateCreate ascending  select t).ToList();

                    Dictionary<int, DataLampItemMining> firstSlu = new Dictionary<int, DataLampItemMining>(); 
                    Dictionary<int, DataLampItemMining> beforeSlu = new Dictionary<int, DataLampItemMining>();
                    Dictionary<int, DataLampItemMining> lastSlu = new Dictionary<int, DataLampItemMining>();

                    double x1 = 0;
                    double x2 = 0;

                    Dictionary<Tuple<int, int, int>, double> elecs = new Dictionary<Tuple<int, int, int>, double>();
                    Dictionary<Tuple<int, int, int>, double> times = new Dictionary<Tuple<int, int, int>, double>();

                    foreach (var tt in nts)
                    {
                        #region
                        //var mtp = new DataLampItemMining(tt, index);
                        //index++;

                        //if (lastSlu == null)
                        //{
                        //    lastSlu = mtp;
                        //    sg.Add(mtp);
                        //    continue;
                        //}
                        //if (lastSlu.SluId != tt.SluId || lastSlu.CtrlId != tt.CtrlId || lastSlu.LampId != tt.LampId)
                        //{
                        //    lastSlu = mtp;
                        //    sg.Add(mtp);
                        //    continue;
                        //}
                        //mtp.TotalTime = Math.Round(tt.ActiveTimeTotal / 60 - lastSlu.ActiveTimeTotal + lastSlu.TotalTime, 2);
                        //mtp.TotalElec = Math.Round(tt.ElectricityTotal - lastSlu.ElectricityTotal + lastSlu.TotalElec, 2);


                        //lastSlu = mtp;
                        //sg.Add(mtp);
                        #endregion

                        //if (tt.BaseTimeThatdayLastRecord == 0) continue;

                        index++;
                        var mtp = new DataLampItemMining(tt, index);

                        var ke = new Tuple<int, int, int>(tt.SluId, tt.CtrlId, tt.LampId);
                        if (elecs.ContainsKey(ke) == false) elecs.Add(ke, tt.OneDayUsePower);
                        else elecs[ke] += tt.OneDayUsePower;


                        if (times.ContainsKey(ke) == false) times.Add(ke, tt.OneDayOpenTime );
                        else times[ke] += tt.OneDayOpenTime;

                        mtp.TotalTime = Math.Round(times[ke ] / 60, 2);
                        mtp.TotalElec = Math.Round(elecs [ke ], 2);

                        mtp.ActiveTime = Math.Round(tt.OneDayOpenTime / 60, 2);
                        mtp.Electricity = Math.Round(tt.OneDayUsePower, 2);
                        sg.Add(mtp);
                        continue;

                        //if (index == 1)
                        //    mtp = new DataLampItemMining(tt, index, true);

                        if (!firstSlu.ContainsKey(tt.LampId))
                        {
                            firstSlu.Add(index, mtp);

                            if (tt.IsThatdayChangedCtrl == 0)
                            {
                                mtp.TotalTime = Math.Round(tt.OneDayOpenTime / 60, 2);
                                mtp.TotalElec = Math.Round(tt.OneDayUsePower, 2);
                                sg.Add(mtp);

                                if (lastSlu.ContainsKey(tt.LampId))//
                                    lastSlu[tt.LampId] = mtp;
                                else
                                    lastSlu.Add(tt.LampId, mtp);
                                
                            }
                            else if (tt.IsThatdayChangedCtrl == 1)
                            {
                                if (beforeSlu.ContainsKey(tt.LampId))
                                {
                                    beforeSlu[tt.LampId] = mtp;
                                }
                                else
                                {
                                    beforeSlu.Add(tt.LampId, mtp);
                                }
                            }


                        }

                        //if (tt.LampId == index)
                        //{
                        //    if (firstSlu.ContainsKey(index))
                        //    {
                        //        firstSlu[index] = mtp;
                        //    }
                        //    else
                        //    {
                        //        firstSlu.Add(index, mtp);
                        //    }

                        //    if (tt.IsThatdayChangedCtrl == 0)
                        //    {
                        //        mtp.TotalTime = Math.Round(tt.OneDayOpenTime / 60, 2);
                        //        mtp.TotalElec = Math.Round(tt.OneDayUsePower, 2);
                        //        sg.Add(mtp);
                        //    }
                        //    else if (tt.IsThatdayChangedCtrl == 1)
                        //    {
                        //        if (beforeSlu.ContainsKey(tt.LampId))
                        //        {
                        //            beforeSlu[tt.LampId] = mtp;
                        //        }
                        //        else
                        //        {
                        //            beforeSlu.Add(tt.LampId, mtp);
                        //        }
                        //    }
                           
                        //}
                        else if (tt.IsThatdayChangedCtrl == 0)
                        {
                            //mtp.TotalTime = Math.Round(tt.BaseTimeThatdayLastRecord / 60 + firstSlu[tt.LampId].ActiveTime - firstSlu[tt.LampId].ActiveTimeTotal, 2);
                            //mtp.TotalElec = Math.Round(tt.BasePowerThatdayLastRecord + firstSlu[tt.LampId].Electricity - firstSlu[tt.LampId].ElectricityTotal, 2);
                            //mtp.ActiveTime = Math.Round(mtp.TotalTime - lastSlu[tt.LampId].TotalTime, 2);//
                            //mtp.Electricity = Math.Round(mtp.TotalElec - lastSlu[tt.LampId].TotalElec, 2);//

                            mtp.TotalTime = Math.Round(tt.OneDayOpenTime / 60 + firstSlu[tt.LampId].TotalTime, 2);
                            mtp.TotalElec = Math.Round(tt.OneDayUsePower + firstSlu[tt.LampId].TotalElec, 2);
                             

                            mtp.ActiveTime = Math.Round(tt.OneDayOpenTime / 60, 2);
                            mtp.Electricity = Math.Round(tt.OneDayUsePower, 2);

                            sg.Add(mtp);

                            if (lastSlu.ContainsKey(tt.LampId))//
                                lastSlu[tt.LampId] = mtp;
                            else
                                lastSlu.Add(tt.LampId, mtp);
                        }
                        else if (tt.IsThatdayChangedCtrl == 1)
                        {
                            if (beforeSlu.ContainsKey(tt.LampId))
                            {
                                beforeSlu[tt.LampId] = mtp;
                            }
                            else
                            {
                                beforeSlu.Add(tt.LampId, mtp);
                            }
                        }
                        else if (tt.IsThatdayChangedCtrl == 2)
                        {
                            if (firstSlu.ContainsKey(tt.LampId))
                            {
                                firstSlu[tt.LampId].ActiveTimeTotal = tt.BaseTimeThatdayLastRecord +
                                                                      firstSlu[tt.LampId].ActiveTimeTotal -
                                                                      beforeSlu[tt.LampId].ActiveTimeTotal;
                                firstSlu[tt.LampId].ElectricityTotal = tt.BasePowerThatdayLastRecord +
                                                                       firstSlu[tt.LampId].ElectricityTotal -
                                                                       beforeSlu[tt.LampId].ElectricityTotal;
                            }
                            else
                            {
                                firstSlu.Add(tt.LampId, mtp);
                            }

                            mtp.TotalTime = Math.Round(tt.BaseTimeThatdayLastRecord / 60 + firstSlu[tt.LampId].ActiveTime - firstSlu[tt.LampId].ActiveTimeTotal + beforeSlu[tt.LampId].ActiveTime, 2);
                            mtp.TotalElec = Math.Round(tt.BasePowerThatdayLastRecord + firstSlu[tt.LampId].Electricity - firstSlu[tt.LampId].ElectricityTotal + beforeSlu[tt.LampId].Electricity, 2);

                            if (lastSlu.ContainsKey(tt.LampId))//
                            {
                                mtp.ActiveTime = Math.Round(mtp.TotalTime - lastSlu[tt.LampId].TotalTime, 2);
                                mtp.Electricity = Math.Round(mtp.TotalElec - lastSlu[tt.LampId].TotalElec, 2);
                            }
                            
                            sg.Add(mtp);

                            if (lastSlu.ContainsKey(tt.LampId))//
                                lastSlu[tt.LampId] = mtp;
                            else
                                lastSlu.Add(tt.LampId, mtp);
                        }



                    }
                    var sg1 = (from t in sg orderby t.SluId, t.CtrlId,t.LampId,t.DateCreate ascending select t).ToList();
                    foreach (var t in sg1)
                    {
                        this.ItemsLamp.Add(t);
                    }

                   // this.ItemsLamp = sg;

                    Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "--数据查询成功，共计" + ItemsLamp.Count +
                             " 条数据.";
                    return;
                }

                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "-- 数据查询成功，0条数据.";
                #endregion
            }
        }


        public void RecordDatamining(string session,Wlst .mobile .MsgWithMobile    infos)
        {
            var info = infos.WstSluDatamining  ;
            if (info == null) return;
            if (_thisViewActive == false) return;

            this.ItemsLamp.Clear();
            this.ItemsCtrlSome.Clear();

            if (IsSome==false )
            {


                IndexView = 3;
                return;
            }
            if (info.Info.Count > 0)
            {
                IndexView = 2;
                //var gts = (from t in info.Info orderby t.SluId , t.CtrlId , t.LampId ascending select t).ToList();

                var dir = new Dictionary<Tuple<int, int>, DataCtrlMiningSome>();

                foreach (var tt in info.Info)
                {
                    
                    var tu = new Tuple<int, int>(tt.SluId, tt.CtrlId);
                    if (!dir.ContainsKey(tu))
                        dir.Add(tu,
                                new DataCtrlMiningSome()
                                    {
                                        SluId = tt.SluId ,
                                        CtrlId = tt.CtrlId.ToString("d4"),
                                        ActiveTimeTotal1 = "--",
                                        ActiveTimeTotal2 = "--",
                                        ActiveTimeTotal3 = "--",
                                        ActiveTimeTotal4 = "--",
                                        ElectricityTotalLamp1 = "--",
                                        ElectricityTotalLamp2 = "--",
                                        ElectricityTotalLamp3 = "--",
                                        ElectricityTotalLamp4 = "--"
                                    });

                    var tu0 = new Tuple<int, int>(tt.SluId, -1);
                    if (!dir.ContainsKey(tu0))
                        dir.Add(tu0,
                                new DataCtrlMiningSome()
                                    {
                                        SluId = tt.SluId,
                                        CtrlId = "--",
                                        ActiveTimeTotal1 = "--",
                                        ActiveTimeTotal2 = "--",
                                        ActiveTimeTotal3 = "--",
                                        ActiveTimeTotal4 = "--",
                                        ElectricityTotalLamp1 = "--",
                                        ElectricityTotalLamp2 = "--",
                                        ElectricityTotalLamp3 = "--",
                                        ElectricityTotalLamp4 = "--"
                                    });



                    if (tt.LampId == 1)
                    {
                        dir[tu].ActiveTimeTotal1 = ((tt.LastActiveTimeTotal - tt.FirstActiveTimeTotal) / 60).ToString("f2");
                        dir[tu].ElectricityTotalLamp1 =
                            (tt.LastElectricityTotal - tt.FirstElectricityTotal).ToString("f2");

                        dir[tu].t1 += tt.LastActiveTimeTotal - tt.FirstActiveTimeTotal;
                        dir[tu].e1 += tt.LastElectricityTotal - tt.FirstElectricityTotal;

                        dir[tu0].t1 += tt.LastActiveTimeTotal - tt.FirstActiveTimeTotal;
                        dir[tu0].e1 += tt.LastElectricityTotal - tt.FirstElectricityTotal;
                        dir[tu0].ActiveTimeTotal1 = (dir[tu0].t1 / 60).ToString("f2");
                        dir[tu0].ElectricityTotalLamp1 = (dir[tu0].e1).ToString("f2");
                    }
                    if (tt.LampId == 2)
                    {
                        dir[tu].ActiveTimeTotal2 = ((tt.LastActiveTimeTotal - tt.FirstActiveTimeTotal) / 60).ToString("f2");
                        dir[tu].ElectricityTotalLamp2 =
                            (tt.LastElectricityTotal - tt.FirstElectricityTotal).ToString("f2");
                        dir[tu].t2 += tt.LastActiveTimeTotal - tt.FirstActiveTimeTotal;
                        dir[tu].e2 += tt.LastElectricityTotal - tt.FirstElectricityTotal;

                        dir[tu0].t2 += tt.LastActiveTimeTotal - tt.FirstActiveTimeTotal;
                        dir[tu0].e2 += tt.LastElectricityTotal - tt.FirstElectricityTotal;
                        dir[tu0].ActiveTimeTotal2 = (dir[tu0].t2 / 60).ToString("f2");
                        dir[tu0].ElectricityTotalLamp2 = (dir[tu0].e2).ToString("f2");
                    }
                    if (tt.LampId == 3)
                    {
                        dir[tu].ActiveTimeTotal3 = ((tt.LastActiveTimeTotal - tt.FirstActiveTimeTotal) / 60).ToString("f2");
                        dir[tu].ElectricityTotalLamp3 =
                            (tt.LastElectricityTotal - tt.FirstElectricityTotal).ToString("f2");
                        dir[tu].t3 += tt.LastActiveTimeTotal - tt.FirstActiveTimeTotal;
                        dir[tu].e3 += tt.LastElectricityTotal - tt.FirstElectricityTotal;

                        dir[tu0].t3 += tt.LastActiveTimeTotal - tt.FirstActiveTimeTotal;
                        dir[tu0].e3 += tt.LastElectricityTotal - tt.FirstElectricityTotal;
                        dir[tu0].ActiveTimeTotal3 = (dir[tu0].t3 / 60).ToString("f2");
                        dir[tu0].ElectricityTotalLamp3 = (dir[tu0].e3).ToString("f2");
                    }
                    if (tt.LampId == 4)
                    {
                        dir[tu].ActiveTimeTotal4 = ((tt.LastActiveTimeTotal - tt.FirstActiveTimeTotal)/ 60).ToString("f2");
                        dir[tu].ElectricityTotalLamp4 =
                            (tt.LastElectricityTotal - tt.FirstElectricityTotal).ToString("f2");
                        dir[tu].t4 += tt.LastActiveTimeTotal - tt.FirstActiveTimeTotal;
                        dir[tu].e4 += tt.LastElectricityTotal - tt.FirstElectricityTotal;

                        dir[tu0].t4 += tt.LastActiveTimeTotal - tt.FirstActiveTimeTotal;
                        dir[tu0].e4 += tt.LastElectricityTotal - tt.FirstElectricityTotal;
                        dir[tu0].ActiveTimeTotal4 = (dir[tu0].t4 / 60).ToString("f2");
                        dir[tu0].ElectricityTotalLamp4 = (dir[tu0].e4).ToString("f2");
                    }

                    //index++;
                }
                //  this.ItemsLamp = sg;
                var ntgs = (from t in dir.Values orderby t.SluId , t.CtrlId ascending select t).ToList();

                DataCtrlMiningSome last = null;
                int indexgg = 1;
                foreach (var g in ntgs)
                {

                    g.Index = indexgg++;
                    g.ElectricityTotalLampx = (g.e1 + g.e2 + g.e3 + g.e4).ToString( "f2");
                    g.ActiveTimeTotax = ((g.t1 + g.t2 + g.t3 + g.t4) / 60).ToString("f2");

                    if (g.CtrlId == "--")
                    {
                        if (last != null)
                        {
                            ItemsCtrlSome.Add(last);
                        }
                        last = g;
                        g.Index = 0;
                        indexgg = 1;
                        continue;
                    }
                    if (last != null)
                    {
                        g.Index = indexgg++;
                        last.ItemsCtrlSome.Add(g);
                    }
                }
                if (last != null) ItemsCtrlSome.Add(last);


                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "--数据查询成功.";
                return;
            }

            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "-- 数据查询成功，0条数据.";
        }




        private void InitEvent()
        {
            this.AddEventFilterInfo(EventIdAssign.EquipmentSelected,
                                    PublishEventType.Core);
        }


        public override void ExPublishedEvent(
            PublishEventArgs args)
        {
            if (_thisViewActive == false) return;
            try
            {

                if (args.EventId == EventIdAssign.EquipmentSelected)
                {
                    if (args.GetParams().Count > 1)
                    {
                        int sluId = Convert.ToInt32(args.GetParams()[0]);
                        if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsSlu(sluId) == false) return;
                        int ctrId = Convert.ToInt32(args.GetParams()[1]);
                        SelectIdChange(sluId, ctrId);

                        FlagIsRtuUseed = true;
                        FlagIsCtrlUseed = true;
                    }
                    else
                    {
                        int id = Convert.ToInt32(args.GetParams()[0]);
                        if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsSlu(id) == false) return;
                        SelectIdChange(id);

                        FlagIsRtuUseed = true;
                        FlagIsCtrlUseed = false ;
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void SelectIdChange(int sluId, int ctrId = 0)
        {
            if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsSlu(sluId) == false) return;

            this.SluId = sluId;

            if (ctrId > 0) this.CtrId = ctrId;
            else
            {
                CtrId = 0;
                CtrName = "--";
            }
            QueryStrinInfoChanged();
        }



    }
}
