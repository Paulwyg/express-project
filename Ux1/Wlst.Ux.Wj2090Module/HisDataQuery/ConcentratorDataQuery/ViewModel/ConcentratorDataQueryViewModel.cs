using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Sr.EquipmentInfoHolding.Services;
 
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Services;
using Wlst.Ux.Wj2090Module.HisDataQuery.ConcentratorDataQuery.ViewModel;
using Wlst.Ux.Wj2090Module.HisDataQuery.ConcentratorDataQuery.Service;
using Wlst.mobile;


namespace Wlst.Ux.Wj2090Module.HisDataQuery.ConcentratorDataQuery.ViewModel
{
    [Export(typeof (IIConcentratorDataQuery))]
    public partial class ConcentratorDataQueryViewModel : EventHandlerHelperExtendNotifyProperyChanged,
                                                          IIConcentratorDataQuery
    {
        public ConcentratorDataQueryViewModel()
        {
            InitAction();
            InitEvent();
            QueryStrinInfoChanged();
        }


        #region IITab
        public int Index
        {
            get { return 1; }
        }
        public string Title
        {
            get { return "单灯数据查询"; }
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
            Remind = "请通过点击左侧单灯树选择单灯集中器或单灯控制器进行终端数据查询...";
            BeginTime = DateTime.Now.AddDays(-1);
            EndTime = DateTime.Now;
            FlagVisiIndex = 1;
            _thisViewActive = true;
            FlagIsRtuUseed = false;
            IsShowAllLampData = false;
            IsPhy = false;
            if (parsObjects.Length > 0)
            {
                try
                {
                    if (parsObjects.Count() > 0)
                    {
                        var rtuls = Convert.ToInt32(parsObjects[0]);
                        if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsSlu(rtuls) )
                        {
                            this.SelectIdChange(rtuls);
                            FlagIsRtuUseed = true;
                            if (parsObjects.Count() > 1)
                            {
                                var ctrls = Convert.ToInt32(parsObjects[1]);
                                if (ctrls > 0)
                                {
                                    this.SelectIdChange(rtuls, ctrls);
                                    FlagIsCtrlUseed = true;
                                }
                            }
                        }
                        else if (rtuls > 1700000 && rtuls < 1800000)   // lvf 2018年5月28日16:11:44 物联网单灯
                        {
                            if (parsObjects.Count() > 1)
                            {
                                var ctrls = Convert.ToInt32(parsObjects[1]);
                                if (ctrls > 0 &&
                                    Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(rtuls) != null)
                                {
                                    FlagIsRtuUseed = true;
                                    FlagIsCtrlUseed = true;
                                    FlagVisiIndex = 3;
                                    this.SelectIdChange(rtuls, ctrls);
                                }
                            }

                        }


                    }
                }
                catch (Exception ex)
                {

                }
            }
            QueryStrinInfoChanged();
            ExQuery();
        }

        public void OnUserHideOrClosing()
        {
            _thisViewActive = false;
            ItemsSlu.Clear();
            ItemsLamp.Clear();
            ItemsCtrl.Clear();
            Remind = string.Empty;
            FlagIsRtuUseed = false;
            FlagIsCtrlUseed = false;
            ItemCount = 0;
            PageTotal = "";
        }


        public bool IsShowAllLampData { get; set; }

        #region Items

        private ObservableCollection<DataSluItem> _concentratorItems;

        public ObservableCollection<DataSluItem> ItemsSlu
        {
            get { return _concentratorItems ?? (_concentratorItems = new ObservableCollection<DataSluItem>()); }
            set
            {
                if (value == _concentratorItems) return;
                _concentratorItems = value;
                this.RaisePropertyChanged(() => this.ItemsSlu);
            }
        }


        private ObservableCollection<DataCtrlItem> _concentratorItemss;

        public ObservableCollection<DataCtrlItem> ItemsCtrl
        {
            get { return _concentratorItemss ?? (_concentratorItemss = new ObservableCollection<DataCtrlItem>()); }
            set
            {
                if (value == _concentratorItemss) return;
                _concentratorItemss = value;
                this.RaisePropertyChanged(() => this.ItemsCtrl);
            }
        }

        private ObservableCollection<DataLampItem> _concentratorItemsss;

        public ObservableCollection<DataLampItem> ItemsLamp
        {
            get { return _concentratorItemsss ?? (_concentratorItemsss = new ObservableCollection<DataLampItem>()); }
            set
            {
                if (value == _concentratorItemsss) return;
                _concentratorItemsss = value;
                this.RaisePropertyChanged(() => this.ItemsLamp);
            }
        }


        private ObservableCollection<DataPhyItem> _concentratorItemssss;

        public ObservableCollection<DataPhyItem> ItemsPhyInfo
        {
            get { return _concentratorItemssss ?? (_concentratorItemssss = new ObservableCollection<DataPhyItem>()); }
            set
            {
                if (value == _concentratorItemssss) return;
                _concentratorItemssss = value;
                this.RaisePropertyChanged(() => this.ItemsPhyInfo);
            }
        }

        #endregion


   
    }

    /// <summary>
    /// Attri
    /// </summary>
    public partial class ConcentratorDataQueryViewModel
    {



        private DateTime _dtPreQueryStartTime;
        private DateTime _dtPreQueryEndTime;

        private void Query(int pageIndex,int pagingFlag)
        {


            _dtPreQueryEndTime = this.EndTime;
            _dtPreQueryStartTime = this.BeginTime;

            var tStartTime = new DateTime(BeginTime.Year, BeginTime.Month, BeginTime.Day, 0, 0, 1);
            var tEndTime = new DateTime(EndTime.Year, EndTime.Month, EndTime.Day, 23, 59, 59);

            // 如果为10则查询所有集中器数据，如果为11 则查询所有集中器的所有控制器数据，如果为12则查询所有集中器下所有控制器下的所有灯头数据；
            // 如果为1 则查询集中器数据，如果为2则查询该集中器下的所有控制器数据，如果为3则查询指定集中器下所有控制器下的所有灯头数据；
            // 如果为4 则查询指定控制器数据，如果为5 则查询指定控制器下所有灯头数据，如果为6 则查询控制器下的指定灯头数据</param>
            // this.Query(tStartTime, tEndTime, this.SluId); 


            int typeflat = 1;
            int sluid = 0;
            int ctrlid = 0;
            long diftime = (EndTime.Ticks - BeginTime.Ticks) / 10000000 / 60;


            if (FlagIsRtuUseed == false && FlagIsCtrlUseed == false  && !IsPhy)
            {
                Remind = "查询：所有集中器数据";
                typeflat = 1;
            }
            if (FlagIsRtuUseed == true && FlagIsCtrlUseed == false  && !IsPhy)
            {
                if (SluId < 1)
                {
                    Remind = "查询：所有集中器数据";
                    typeflat = 1;
                }
                else
                {
                    //int diftime = 360;


                    if (diftime > 360)
                    {
                        Remind = "查询：所有集中器" + SluId + "数据";
                        typeflat = 1;
                        sluid = SluId;
                    }
                    else
                    {
                        Remind = "查询：集中器" + SluId + "下所有控制器数据数据";
                        typeflat = 2;
                        sluid = SluId;
                    }
                }

            }
            if (FlagIsRtuUseed == false && FlagIsCtrlUseed == true  && !IsPhy)
            {

                Remind = "查询：所有控制器数据";
                typeflat = 2;
            }

            if (FlagIsCtrlUseed && FlagIsRtuUseed && !IsPhy)
            {
                if (SluId < 1 && CtrId < 1)
                {
                    Remind = "查询：所有控制器数据";
                    typeflat = 2;

                }
                else if (CtrId < 1)
                {
                    Remind = "查询：集中器" + SluPhyId + "下控制器所有控制器数据";
                    typeflat = 2;
                    sluid = SluId;

                }
                else if (SluId < 1)
                {
                    Remind = "查询：所有集中器数据";
                    typeflat = 2;

                }
                else
                {
                    Remind = "查询：所有集中器" + SluPhyId + "下控制器" + CtrPhyId + "数据";
                    typeflat = 2;
                    sluid = SluId;
                    ctrlid = CtrId;
                }
            }

            if (IsPhy)
            {
                if (FlagIsRtuUseed == false && FlagIsCtrlUseed == false)
                {
                    Remind = "查询：所有控制器物理信息";
                    typeflat = 3;
                }
                else if (FlagIsRtuUseed == true && FlagIsCtrlUseed == false)
                {
                    if (SluId < 1)
                    {
                        Remind = "查询：所有控制器物理信息";
                        typeflat = 3;
                    }
                    else
                    {
                        Remind = "查询：集中器" + SluId + "下所有控制器物理信息";
                        typeflat = 3;
                        sluid = SluId;
                    }
                }
                else if (FlagIsRtuUseed == false && FlagIsCtrlUseed == true)
                {
                    Remind = "查询：所有控制器物理信息";
                    typeflat = 3;
                }
                else if (FlagIsRtuUseed == true && FlagIsCtrlUseed == true)
                {
                    if (SluId < 1 && CtrId < 1)
                    {
                        Remind = "查询：所有控制器物理信息";
                        typeflat = 3;
                    }
                    else if (CtrId < 1)
                    {
                        Remind = "查询：集中器" + SluPhyId + "下控制器所有控制器物理信息";
                        typeflat = 3;
                        sluid = SluId;

                    }
                    else if (SluId < 1)
                    {
                        Remind = "查询：所有控制器物理信息";
                        typeflat = 3;
                    }
                    else
                    {
                        Remind = "查询：所有集中器" + SluPhyId + "下控制器" + CtrPhyId + "物理信息";
                        typeflat = 3;
                        sluid = SluId;
                        ctrlid = CtrId;
                    }
                }

            }



            if (IsPhy)
            {
                //this.Query(BeginTime, EndTime, sluid, ctrlid, 3);
                this.RequestHttpData(tStartTime, tEndTime, sluid, ctrlid, 3, pageIndex, pagingFlag);
            }
            else
            {
                if (sluid > 0 && typeflat == 2 && ctrlid == 0)
                {
                    this.RequestHttpData(tStartTime, tEndTime, sluid, ctrlid, typeflat, pageIndex, pagingFlag);
                }
                else
                {
                    this.RequestHttpData(tStartTime, tEndTime, sluid, ctrlid, typeflat, pageIndex, pagingFlag);
                }
            }
        }

        private void QueryStrinInfoChanged()
        {
            QueryStrinInfo = GetQueryInfo();
        }

        private string GetQueryInfo()
        {
            if (IsPhy)
            {
                 if (FlagIsRtuUseed == false && FlagIsCtrlUseed == false)
                {
                    return "查询：所有控制器物理信息";
                }
                if (FlagIsRtuUseed == true && FlagIsCtrlUseed == false)
                {
                    if (SluId < 1)
                    {
                        return "查询：所有控制器物理信息";
                    }

                    return "查询：集中器" + SluPhyId + "下所有控制器物理信息";

                }
                if (FlagIsRtuUseed == false && FlagIsCtrlUseed == true)
                {

                    return "查询：所有控制器物理信息";

                }

                if (SluId < 1 && CtrId < 1)
                {
                    return "查询：所有控制器物理信息";
                }
                if (CtrId < 1)
                {
                    return "查询：集中器" + SluPhyId + "下所有控制器物理信息";
                }
                if (SluId < 1)
                {
                    return "查询：所有控制器数据";
                }
                return "查询：集中器" + SluPhyId + "下控制器" + CtrPhyId + "物理信息";
            
            }
            else
            {
                if (FlagIsRtuUseed == false && FlagIsCtrlUseed == false)
                {
                    return "查询：所有集中器数据";
                }
                if (FlagIsRtuUseed == true && FlagIsCtrlUseed == false)
                {
                    if (SluId < 1)
                    {
                        return "查询：所有集中器数据";
                    }

                    return "查询：集中器" + SluPhyId + "数据";

                }
                if (FlagIsRtuUseed == false && FlagIsCtrlUseed == true)
                {

                    return "查询：所有控制器数据";

                }

                if (SluId < 1 && CtrId < 1)
                {
                    return "查询：所有控制器数据";
                }
                if (CtrId < 1)
                {
                    return "查询：集中器" + SluPhyId + "下所有控制器数据";
                }
                if (SluId < 1)
                {
                    return "查询：所有控制器数据";
                }
                return "查询：集中器" + SluPhyId + "下控制器" + CtrPhyId + "数据";
            }
            


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
                if (value > 4) value = 4;
                if (_flagVisi == value) return;
                _flagVisi = value;
                RaisePropertyChanged(() => FlagVisiIndex);
            }
        }


        private Wlst .Cr .CoreOne .Models .ObservableObjectCollection< string > _flagsdfVisi;

        public Wlst.Cr.CoreOne.Models.ObservableObjectCollection<string > FlagVisiIndexsdfsd
        {
            get
            {
                if (_flagsdfVisi == null) _flagsdfVisi = new ObservableObjectCollection<string >(5);
                return _flagsdfVisi;
            }
            set
            {
                if (_flagsdfVisi == value) return;
                _flagsdfVisi = value;
                RaisePropertyChanged(() => FlagVisiIndexsdfsd);
            }
        }


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
                _flasdfsdgVsdfssdfsddisi = value;
                RaisePropertyChanged(() => FlagIsCtrlUseed);
                if(value ==false )
                {
                    CtrId = 0;
                }
                QueryStrinInfoChanged();
            }
        }



        private string  _fssdfsdfsdddisi;
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
        
        #region 物理信息

        private bool _isPhy;

        public bool IsPhy
        {
            get { return _isPhy; }
            set
            {
                if (_isPhy == value) return;
                _isPhy = value;
                RaisePropertyChanged(() => IsPhy);
                QueryStrinInfoChanged();
            }
        }

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
        public int SluId   //todo NB
        {
            get { return _sluId; }
            set
            {
                if (_sluId == value) return;
                _sluId = value;
                RaisePropertyChanged(() => SluId);


                if (EquipmentDataInfoHold.InfoItems .ContainsKey(SluId))
                {
                    var t = EquipmentDataInfoHold.InfoItems[SluId] as Wlst.Sr.EquipmentInfoHolding.Model.Wj2090Slu;
                    if (t == null) return;

                    //  this.SluPhyId = t.PhyId;
                    this.SluName = t.RtuName;
                    if (t.RtuFid == 0)
                    {
                        SluPhyId = t.RtuPhyId.ToString("D4");
                    }
                    else
                    {
                        var ntg =
                            Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(t.RtuFid);
                        if (ntg != null)
                        {
                            SluPhyId = ntg.RtuPhyId.ToString("D4");
                        }
                        else
                        {
                            SluPhyId = value.ToString();
                        }
                    }
                    //  QueryStrinInfoChanged();
                }
                else if (
                        Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(SluId) != null)
                {
                    var tml =
                        Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(SluId);
                    SluName = tml.FieldName;
                    SluPhyId = tml.PhyId.ToString();
                    //if (tml.PhyId == 0)
                    //    SluPhyId = tml.PhyId.ToString();
                    //else SluPhyId = value.ToString();
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
         
        public int CtrId   //todo NB
        {
            get { return _ctrId; }
            set
            {
                if (_ctrId == value) return;
                _ctrId = value;
                RaisePropertyChanged(() => CtrId);
                if (CtrId > 0)
                {
                    if (EquipmentDataInfoHold.InfoItems.ContainsKey(SluId))
                    {
                        var t = EquipmentDataInfoHold.InfoItems[SluId] as Wlst.Sr.EquipmentInfoHolding.Model.Wj2090Slu;

                        if (t == null)
                            return;
                        if (!t.WjSluCtrls.ContainsKey(CtrId))
                            return;
                        this.CtrPhyId = t.WjSluCtrls[CtrId].CtrlPhyId;
                        //this.CtrName = t.WjSluCtrls [CtrId].RtuName;

                        if (string.IsNullOrEmpty(t.WjSluCtrls[CtrId].LampCode))
                            this.CtrName = t.WjSluCtrls[CtrId].RtuName;
                        else this.CtrName = t.WjSluCtrls[CtrId].LampCode;
                    }
                    else if (
                        Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(SluId) != null)
                    {
                        var t =
                            Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(SluId);

                        if (t == null)
                            return;

                        var flg = false;
                        foreach (var tt in t.CtrlLst)
                        {
                            if (tt.CtrlId == CtrId)
                            {
                                flg = true;
                            }
                        }

                        if (!flg) return;

                        var para = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Get(SluId, CtrId);
                        this.CtrPhyId = para.CtrlId;
                        this.CtrName = para.CtrlName;
                    }
                }
                else
                {
                    this.CtrPhyId = 0;
                    this.CtrName = "";
                }
               // GetQueryInfo();
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

        #region 分页

        #region PageTotal

        private string _pageTotal;
        public string PageTotal
        {
            get { return _pageTotal; }
            set
            {
                if (value == _pageTotal) return;
                _pageTotal = value;
                RaisePropertyChanged(() => PageTotal);
            }
        }
        #endregion

        #region   PageIndex

        private int _pageIndex;

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex
        {
            get { return _pageIndex; }
            set
            {
                if (value != _pageIndex)
                {
                    _pageIndex = value;
                    this.RaisePropertyChanged(() => this.PageIndex);
                    Query( value, 1);
                    //RequestHttpData(BeginDate, EndDate, RtuId, _lstOpetatorType, _selectedsetsndans, _selecteduser,
                    //                PageIndex, 1);

                }
            }
        }
        #endregion

        #region   ItemCount
        private int _itemCount;

        /// <summary>
        /// 数据总数
        /// </summary>
        public int ItemCount
        {
            get { return _itemCount; }
            set
            {
                if (value != _itemCount)
                {
                    _itemCount = value;
                    this.RaisePropertyChanged(() => this.ItemCount);
                }
            }
        }
        #endregion

        #region   PageSize
        private int _pageSize;

        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                if (value != _pageSize)
                {
                    _pageSize = value;
                    this.RaisePropertyChanged(() => this.PageSize);
                }
            }
        }
        #endregion

        #region PagerVisi

        private Visibility _pagerVisi = Visibility.Visible;
        public Visibility PagerVisi
        {
            get { return _pagerVisi; }
            set
            {
                if (value == _pagerVisi) return;
                _pagerVisi = value;
                RaisePropertyChanged(() => PagerVisi);
            }
        }
        #endregion

        #region   RequestInfo
        private mobile.MsgWithMobile _requestInfo;

        /// <summary>
        /// 请求回来的数据
        /// </summary>
        public mobile.MsgWithMobile RequestInfo
        {
            get { return _requestInfo; }
            set
            {
                if (value != _requestInfo)
                {
                    _requestInfo = value;
                    this.RaisePropertyChanged(() => this.RequestInfo);
                }
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
          //  FlagVisiIndexsdfsd.Value0 = DateTime.Now.Ticks+" f";
            _dtQuery = DateTime.Now;
            PageIndex = 0;
            Query(PageIndex, 0);
            //  Remind = "集中器查询命令已发送...请等待数据反馈！";
        }

        private bool CanQuery()
        {
            if (BeginTime.Ticks > EndTime.Ticks) return false;

            if (FlagIsRtuUseed )
            {
                if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsSlu(SluId) == false &&
                    Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(SluId) == null) return false;
            }
            if (FlagIsCtrlUseed)
            {
                if (FlagIsRtuUseed == false) return false;
                //if (CtrId < 1 || CtrId > 4) return false;
            }

            return DateTime.Now.Ticks - _dtQuery.Ticks > 30000000;

        }

        #endregion

        //#region CmdExport

        //private DateTime _dtCmdExport;
        //private ICommand _cmdCmdExport;

        //public ICommand CmdExport
        //{
        //    get
        //    {
        //        if (_cmdCmdExport == null)
        //            _cmdCmdExport = new RelayCommand(ExCmdExport, CanExCmdExport, false);
        //        return _cmdCmdExport;
        //    }
        //}

        //private void ExCmdExport()
        //{
        //    _dtCmdExport = DateTime.Now;
        //    try
        //    {
        //        var lsttitle = new List<Object>();
        //        lsttitle.Add("序号");
        //        lsttitle.Add("采样时间");
        //        lsttitle.Add("运行状态");
        //        lsttitle.Add("主报状态");
        //        lsttitle.Add("开机状态");
        //        lsttitle.Add("通信方式");
        //        lsttitle.Add("参数状态");
        //        lsttitle.Add("硬件状态");
        //        lsttitle.Add("未知控制器数量");
        //        lsttitle.Add("复位次数");
        //        lsttitle.Add("Zigbee通信信道");

        //        var lstobj = new List<List<object>>();

        //        foreach (var g in ItemsSlu)
        //        {
        //            var tmp = new List<object>();
        //            tmp.Add(g.Index);
        //            tmp.Add(g.SampleTime);
        //            tmp.Add(g.RunState);
        //            tmp.Add(g.AlarmState);
        //            tmp.Add(g.PowerOnState);
        //            tmp.Add(g.CommunicationState);
        //            tmp.Add(g.ParameterState);
        //            tmp.Add(g.HardwareState);
        //            tmp.Add(g.UnkownControlNum);
        //            tmp.Add(g.ResetNum);
        //            tmp.Add(g.ZgbCommunication);

        //            lstobj.Add(tmp);
        //        }
        //        Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(lsttitle, lstobj);
        //        lstobj = null;
        //        lsttitle = null;
        //    }
        //    catch (Exception e)
        //    {
        //        Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("导出集中器数据报表时报错:" + e);
        //    }
        //}

        //private bool CanExCmdExport()
        //{
        //    if (ItemsSlu.Count < 1) return false;
        //    return DateTime.Now.Ticks - _dtCmdExport.Ticks > 30000000;
        //}

        //#endregion

        #endregion
    }

    /// <summary>
    /// Action Event
    /// </summary>
    public partial class ConcentratorDataQueryViewModel
    {

        private void InitEvent()
        {
            this.AddEventFilterInfo(EventIdAssign.EquipmentSelected,
                                    PublishEventType.Core);
        }


        public override void ExPublishedEvent(
             PublishEventArgs args)                  //todo NB
        {
            if (_thisViewActive == false) return;
            try
            {

                if (args.EventId == EventIdAssign.EquipmentSelected) 
                {
                    if (args.GetParams().Count > 1)
                    {
                        int sluId = Convert.ToInt32(args.GetParams()[0]);
                        if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsSlu(sluId) == true)
                        {
                            int ctrId = Convert.ToInt32(args.GetParams()[1]);

                            FlagIsRtuUseed = true;
                            FlagIsCtrlUseed = true;
                            SelectIdChange(sluId, ctrId);
                        }
                        else if (
                            Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(sluId) != null)
                        {
                            int ctrId = Convert.ToInt32(args.GetParams()[1]);

                            FlagIsRtuUseed = true;
                            FlagIsCtrlUseed = true;
                            SelectIdChange(sluId, ctrId);
                        }
                    }
                    else
                    {
                        int id = Convert.ToInt32(args.GetParams()[0]);
                        if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsSlu(id))
                        {
                            FlagIsRtuUseed = true;
                            FlagIsCtrlUseed = false;
                            SelectIdChange(id);
                        }
                        else if (
                            Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(id) != null)
                        {
                            FlagIsRtuUseed = true;
                            FlagIsCtrlUseed = false;
                            SelectIdChange(id);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void SelectIdChange(int sluId, int ctrId = 0) //todo NB
        {
            if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsSlu(sluId) == true)
            {

                this.SluId = sluId;

                if (ctrId > 0) this.CtrId = ctrId;
                else
                {
                    CtrId = 0;
                    CtrName = "--";
                }
                //FlagLampIdEnableChanged();
                QueryStrinInfoChanged();
            }
            else if (Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(sluId) != null)
            {
                this.SluId = sluId; 
                if (ctrId > 0) this.CtrId =ctrId;
                else
                {
                    CtrId = 0;
                    CtrName = "--";
                }
                //FlagLampIdEnableChanged();
                QueryStrinInfoChanged();
            }
        }

        /// <summary>
        /// 集中器
        /// </summary>
        public void InitAction()
        {
            InitSluAction();
        }

        public void InitSluAction()
        {
            //ProtocolServer.RegistProtocol(
            //    Wlst.Sr.ProtocolPhone .LxSlu .wst_slu_or_ctrl_data ,// .wlst_svr_ans_cnt_request_wj2090_measure_data ,//.ProtocolCnt.ClientPart.wlst_Wj2090_svr_ans_clinet_request_slu_measure_data,
            //    RecordDataRequest,
            //    typeof (ConcentratorDataQueryViewModel), this,true);
        }

        public void RecordDataRequest(Wlst .mobile .MsgWithMobile  infos,int pagingFlag,int type)
        {
            var info = infos.WstSluData  ;
            if (info == null) return;
            if (_thisViewActive == false) return;
            if (pagingFlag == 0)
            {
                PageSize = infos.Head.PagingNum;
                ItemCount = infos.Head.PagingRecordTotal;
                var count = ItemCount / PageSize + (ItemCount % PageSize > 0 ? 1 : 0);
                PagerVisi = count < 2 ? Visibility.Collapsed : Visibility.Visible;
                PageTotal = "页     " + ItemCount + " 条";
            }

            int index = 1+PageIndex*PageSize;
            IsHasData = false;

            this.ItemsCtrl.Clear();
            this.ItemsLamp.Clear();
            this.ItemsSlu.Clear();
            this.ItemsPhyInfo.Clear();
            if (type == 1)
            {
                if (info.ItemsSlu.Count > 0)
                {
                    var tmps = (from t in info.ItemsSlu orderby t.DateCreate ascending select t).ToList();
                    foreach (var t in tmps)//info.ItemsSlu
                    {
                        ItemsSlu.Add(new DataSluItem(t, index));
                        index++;
                    }
                    index--;

                    var tmo = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(
                        info.SluId);

                    Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" +
                       (tmo == null ? info.SluId : tmo.RtuPhyId) + "--单灯集中器数据查询成功，共计" + ItemCount + " 条数据.";
                    FlagVisiIndex = 1;IsHasData = ItemsSlu.Count > 0;
                    return;
                }
                else
                {
                    FlagVisiIndex = 1; IsHasData = ItemsSlu.Count > 0;
                    Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "-- 数据查询成功，0条数据.";
                }
            }
            else if (type == 2)
            {
                if (info.ItemsLamps.Count > 0)
                {
                    var sg = new ObservableCollection<DataLampItem>();

                    var tmps = (from t in info.ItemsLamps orderby t.DateCreate ascending select t).ToList();
                    foreach (var tt in tmps)//info.ItemsLamps
                    {
                        if (tt.DateCtrlCreate < 0 && IsShowAllLampData == false) continue;
                        sg.Add(new DataLampItem(tt, index));
                        index++;
                    }
                    this.ItemsLamp = sg;
                    index--;
                    FlagVisiIndex = 3; IsHasData = ItemsLamp.Count > 0;
                    Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "--单灯控制器数据查询成功，共计" + ItemCount +
                             " 条数据.";
                    return;
                }
                else
                {
                    FlagVisiIndex = 3; IsHasData = ItemsLamp.Count > 0;
                    Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "-- 数据查询成功，0条数据.";
                }

            }
            else if (type == 3)
            {
                if (info.ItemsPhys.Count > 0)
                {
                    var sg = new ObservableCollection<DataPhyItem>();

                    foreach (var g in info.ItemsPhys)
                    {
                        if (g == null) continue;
                        sg.Add(new DataPhyItem(g, g.SluId, g.CtrlId));
                        index++;
                    }
                    this.ItemsPhyInfo = sg;
                    index--;
                    FlagVisiIndex = 4; IsHasData = ItemsPhyInfo.Count > 0;
                    Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "--单灯控制器物理信息查询成功，共计" + ItemCount +
                             " 条数据.";
                    return;
                }
                else
                {
                    FlagVisiIndex = 4; IsHasData = ItemsPhyInfo.Count > 0;
                    Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "-- 单灯控制器物理信息查询成功，0条数据.";
                }
            }
            else
            {
                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "-- 数据查询成功，0条数据.";
            }


        }



    }

    /// <summary>
    /// Socket
    /// </summary>
    public partial class ConcentratorDataQueryViewModel
    {
      

       

       /// <summary>
        ///  
        /// 
       /// </summary>
       /// <param name="dtstarttime"></param>
       /// <param name="dtendtime"></param>
       /// <param name="sluId"></param>
       /// <param name="ctrId"></param>
        /// <param name="type"> 1 集中器数据  2 控制器数据 3 物理信息</param>
 
        private void Query(DateTime dtstarttime, DateTime dtendtime, int sluId, int ctrId,  int type)
        {
            if (!GetCheckedInformation()) return;
            this.ItemsLamp.Clear();
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询 ...";
            var info = Wlst.Sr.ProtocolPhone .LxSlu  .wst_slu_or_ctrl_data  ;//.ServerPart.wlst_Wj2090_clinet_request_slu_measure_data;
            info.WstSluData  .DtEnd = dtendtime.Ticks;
            info.WstSluData.DtStart = dtstarttime.Ticks;
          //  info.Data.LampId = lampId;
            info.WstSluData.SluId = sluId;
            info.WstSluData.CtrlId = ctrId;
            info.WstSluData.Op = type;

            SndOrderServer.OrderSnd(info, 10, 3);
        }

        private void RequestHttpData(DateTime dtstarttime, DateTime dtendtime, int sluId, int ctrId, int type, int pageIndex, int pagingFlag)
        {
            if (!GetCheckedInformation()) return;
            this.ItemsLamp.Clear();
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询 ...";
            MsgWithMobile info;
            if (type == 1)
                info = Wlst.Sr.ProtocolPhone.LxSluHttp.wst_slu_or_ctrl_data_http1;
                    //.ServerPart.wlst_Wj2090_clinet_request_slu_measure_data;
            else if (type == 2)
                info = Wlst.Sr.ProtocolPhone.LxSluHttp.wst_slu_or_ctrl_data_http2;
            else
                info = Wlst.Sr.ProtocolPhone.LxSluHttp.wst_slu_or_ctrl_data_http3;
            info.WstSluData.DtEnd = dtendtime.Ticks;
            info.WstSluData.DtStart = dtstarttime.Ticks;
            //  info.Data.LampId = lampId;
            info.WstSluData.SluId = sluId;
            info.WstSluData.CtrlId = ctrId;
            info.WstSluData.Op = type;
            info.Head.PagingIdx = pageIndex + 1;
            info.Head.PagingFlag = pagingFlag;
            var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp(info);
            if (data == null) return;
            RecordDataRequest(data, pagingFlag, type);
        }

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
    }
   
    
    
    
    
    ///// <summary>
    ///// 控制器查询
    ///// </summary>
    //public partial class ConcentratorDataQueryViewModel
    //{
 
    //    #region IsLeakElec
    //    private bool _isLeakElec;
    //    public bool IsLeakElec
    //    {
    //        get { return _isLeakElec; }
    //        set
    //        {
    //            if (_isLeakElec == value) return;
    //            _isLeakElec = value;
    //            RaisePropertyChanged(() => IsLeakElec);
    //        }
    //    }
    //    #endregion

    //    #region IsControlOut

    //    private bool _isControlOut;
    //    public bool IsControlOut
    //    {
    //        get { return _isControlOut; }
    //        set
    //        {
    //            if (_isControlOut == value) return;
    //            _isControlOut = value;
    //            RaisePropertyChanged(() => IsControlOut);
    //        }
    //    }
    //    #endregion

    //    #region IsFaultAlarm

    //    private bool _isFaultAlarm;
    //    public bool IsFaultAlarm
    //    {
    //        get { return _isFaultAlarm; }
    //        set
    //        {
    //            if (_isFaultAlarm == value) return;
    //            _isFaultAlarm = value;
    //            RaisePropertyChanged(() => IsFaultAlarm);
    //        }
    //    }
    //    #endregion

    //    #region IsPowerAlarm

    //    private bool _isPowerAlarm;
    //    public bool IsPowerAlarm
    //    {
    //        get { return _isPowerAlarm; }
    //        set
    //        {
    //            if (_isPowerAlarm == value) return;
    //            _isPowerAlarm = value;
    //            RaisePropertyChanged(() => IsPowerAlarm);
    //        }
    //    }
    //    #endregion
       


    //    #region CmdCtrQuery
    //    private DateTime _dtCtrQuery;
    //    private ICommand _cmdCtrQuery;
    //    public ICommand CmdCtrQuery
    //    {
    //        get { return _cmdCtrQuery ?? (_cmdCtrQuery = new RelayCommand(ExCtrQuery, CanCtrQuery, false)); }
    //    }
    //    private void ExCtrQuery()
    //    {
    //        _dtCtrQuery = DateTime.Now;
    //        CtrQuery();
    //        Remind = "控制器查询命令已发送...请等待数据反馈！";
    //        CtrItems.Clear();
    //    }
    //    private bool CanCtrQuery()
    //    {
    //        //if (IsLocked) return false;
    //        if (BeginTime > EndTime) return false;
    //        if (_dtPreQueryEndTime.Ticks != this.EndTime.Ticks || _dtPreQueryStartTime.Ticks != this.BeginTime.Ticks)
    //        {
    //            return DateTime.Now.Ticks - _dtCtrQuery.Ticks > 30000000;
    //        }
    //        return false;
    //    }
    //    #endregion

    //    #region CmdCtrExport
    //    private DateTime _dtCmdCtrExport;
    //    private ICommand _cmdCmdCtrExport;

    //    public ICommand CmdCtrExport
    //    {
    //        get
    //        {
    //            if (_cmdCmdCtrExport == null)
    //                _cmdCmdCtrExport = new RelayCommand(ExCmdCtrExport, CanExCmdCtrExport, false);
    //            return _cmdCmdCtrExport;
    //        }
    //    }

    //    private void ExCmdCtrExport()
    //    {
    //        _dtCmdCtrExport = DateTime.Now;
    //        try
    //        {
    //            var lsttitle = new List<Object>();
    //            lsttitle.Add("序号");
    //            lsttitle.Add("地址");
    //            lsttitle.Add("灯号");
    //            lsttitle.Add("采样时间");
    //            lsttitle.Add("电压");
    //            lsttitle.Add("电流");
    //            lsttitle.Add("功率");
    //            lsttitle.Add("电量");
    //            lsttitle.Add("控制状态");
    //            lsttitle.Add("灯状态");
    //            lsttitle.Add("漏电状态");
    //            lsttitle.Add("功率状态");

    //            var lstobj = new List<List<object>>();

    //            foreach (var g in CtrItems)
    //            {
    //                var tmp = new List<object>();
    //                tmp.Add(g.Index);
    //                tmp.Add(g.ControlId);
    //                tmp.Add(g.LightNum);
    //                tmp.Add(g.SampleTime);
    //                tmp.Add(g.V);
    //                tmp.Add(g.A);
    //                tmp.Add(g.ActivePower);
    //                tmp.Add(g.Electricity);
    //                tmp.Add(g.ControlStatus);
    //                tmp.Add(g.LightStatus);
    //                tmp.Add(g.LeakageStatus);
    //                tmp.Add(g.PowerStatus);
    //                lstobj.Add(tmp);
    //            }
    //            Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(lsttitle, lstobj);
    //            lstobj = null;
    //            lsttitle = null;
    //        }
    //        catch (Exception e)
    //        {
    //            Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("导出控制器数据报表时报错:" + e);
    //        }
    //    }

    //    private bool CanExCmdCtrExport()
    //    {
    //        if (CtrItems.Count < 1) return false;
    //        return DateTime.Now.Ticks - _dtCmdCtrExport.Ticks > 30000000;
    //    }

    //    #endregion

    //    #region IsCtrHasData

    //    private bool _isCtrHasData;
    //    public bool IsCtrHasData
    //    {
    //        get { return _isHasData; }
    //        set
    //        {
    //            if (_isCtrHasData == value) return;
    //            _isCtrHasData = value;
    //            RaisePropertyChanged(() => IsCtrHasData);
    //        }
    //    }

    //    #endregion

    //    #region IsAdvancedChecked

    //    private bool _isAdvancedChecked;
    //    public bool IsAdvancedChecked
    //    {
    //        get { return _isAdvancedChecked; }
    //        set
    //        {
    //            if (_isAdvancedChecked == value) return;
    //            _isAdvancedChecked = value;
    //            RaisePropertyChanged(() => IsAdvancedChecked);
    //        }
    //    }

    //    #endregion

    //}
}
